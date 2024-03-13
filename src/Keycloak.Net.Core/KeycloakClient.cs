using System.Text;
using Flurl.Http.Configuration;
using Keycloak.Net.Common.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json.Serialization;

namespace Keycloak.Net;

public partial class KeycloakClient
{
    private ISerializer _serializer = new NewtonsoftJsonSerializer(new()
    {
        ContractResolver = new CamelCasePropertyNamesContractResolver(),
        NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore
    });

    private readonly Url _url;
    private readonly string _userName;
    private readonly string _password;
    private readonly string _clientSecret;
    private readonly Func<string> _getToken;
    private readonly KeycloakOptions _options;

    private KeycloakClient(string url, KeycloakOptions options)
    {
        _url = url;
        _options = options ?? new KeycloakOptions();
    }

    public KeycloakClient(string url, string userName, string password, KeycloakOptions options = null)
        : this(url, options)
    {
        _userName = userName;
        _password = password;
    }

    public KeycloakClient(string url, string clientSecret, KeycloakOptions options = null)
        : this(url, options)
    {
        _clientSecret = clientSecret;
    }

    public KeycloakClient(string url, Func<string> getToken, KeycloakOptions options = null)
        : this(url, options)
    {
        _getToken = getToken;
    }

    public void SetSerializer(ISerializer serializer)
    {
        _serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
    }

    private IFlurlRequest GetBaseUrl(string authenticationRealm)
    {
        var request = new Url(_url)
            .AppendPathSegment(_options.Prefix)
            .WithSettings(settings => settings.JsonSerializer = _serializer)
            .WithAuthentication(_getToken, _url, _options.AuthenticationRealm ?? authenticationRealm, _userName, _password, _clientSecret, _options);
        if (_options.EnableRequestLogging)
            request.BeforeCall(c =>
            {
                using var mem = new MemoryStream();

                string? content = null;

                if (c.HttpRequestMessage.Content != null)
                {
                    c.HttpRequestMessage.Content.CopyTo(mem, null, CancellationToken.None);
                    mem.Seek(0, SeekOrigin.Begin);
                    using var reader = new StreamReader(mem, Encoding.UTF8, leaveOpen: false);
                    content = reader.ReadToEnd();
                }

                _options.Logger.LogDebug("Request to {Url} with headers {Headers} and body: {Body}.", c.Request.Url, c.Request.Headers.Select(h => new { h.Name, h.Value }), content);
            });
        if (_options.EnableResponseLogging)
            request.AfterCall(c =>
            {
                using var mem = new MemoryStream();
                
                c.HttpResponseMessage.Content.CopyTo(mem, null, CancellationToken.None);
                mem.Seek(0, SeekOrigin.Begin);

                using var reader = new StreamReader(mem, Encoding.UTF8, leaveOpen: false);
                _options.Logger.LogDebug("Response [{Status}] for {Url} with headers {Headers} and body: {Body}.", c.Response.StatusCode, c.Request.Url,
                    c.Response.Headers.Select(h => new { h.Name, h.Value }), reader.ReadToEnd());
            });

        return request;
    }
}

public class KeycloakOptions(string prefix = "", string adminClientId = "admin-cli", string authenticationRealm = default)
{
    public string Prefix { get; } = prefix.TrimStart('/').TrimEnd('/');

    public string AdminClientId { get; } = adminClientId;

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   It is used only when the authorization realm differs from the target one.   </summary>
    ///
    /// <value> The authentication realm.   </value>
    ///-------------------------------------------------------------------------------------------------
    public string AuthenticationRealm { get; } = authenticationRealm;

    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Enables logging of request content. Default is false.
    /// </summary>
    ///
    /// <value> True if enable request logging, false if not.   </value>
    ///-------------------------------------------------------------------------------------------------
    public bool EnableRequestLogging { get; set; }

    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Enables logging of response content. Default is false.
    /// </summary>
    ///
    /// <value> True if enable response logging, false if not.  </value>
    ///-------------------------------------------------------------------------------------------------
    public bool EnableResponseLogging { get; set; }

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Gets or sets the logger.    </summary>
    ///
    /// <value> The logger. </value>
    ///-------------------------------------------------------------------------------------------------
    public ILogger Logger { get; set; } = NullLogger<KeycloakClient>.Instance;
}