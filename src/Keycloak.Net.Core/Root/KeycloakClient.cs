using Keycloak.Net.Models.Root;

namespace Keycloak.Net;

public partial class KeycloakClient
{
    public async Task<ServerInfo> GetServerInfoAsync(string realm, CancellationToken cancellationToken = default) => await GetBaseUrl(realm)
        .AppendPathSegment("/admin/serverinfo/")
        .GetJsonAsync<ServerInfo>(HttpCompletionOption.ResponseContentRead, cancellationToken)
        .ConfigureAwait(false);

    public async Task<bool> CorsPreflightAsync(string realm, CancellationToken cancellationToken = default)
    {
        var response = await GetBaseUrl(realm)
            .AppendPathSegment("/admin/serverinfo/")
            .OptionsAsync(HttpCompletionOption.ResponseContentRead, cancellationToken)
            .ConfigureAwait(false);
        return response.ResponseMessage.IsSuccessStatusCode;
    }
}