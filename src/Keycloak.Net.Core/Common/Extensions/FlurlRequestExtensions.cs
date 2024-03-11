﻿using Flurl;
using Flurl.Http.Newtonsoft;

namespace Keycloak.Net.Common.Extensions
{
    public static class FlurlRequestExtensions
    {
        private static async Task<string> GetAccessTokenAsync(string url, string realm, string userName, string password, KeycloakOptions options = null)
        {
            options ??= new();
            var result = await url
                .AppendPathSegment($"{options.Prefix}/realms/{realm}/protocol/openid-connect/token")
                .WithHeader("Accept", "application/json")
                .PostUrlEncodedAsync(new List<KeyValuePair<string, string>>
                {
                    new("grant_type", "password"),
                    new("username", userName),
                    new("password", password),
                    new("client_id", options.AdminClientId)
                })
                .ReceiveJson().ConfigureAwait(false);

            string accessToken = result
                .access_token.ToString();

            return accessToken;
        }

        private static string GetAccessToken(string url, string realm, string userName, string password, KeycloakOptions options = null) => GetAccessTokenAsync(url, realm, userName, password, options).GetAwaiter().GetResult();

        private static async Task<string> GetAccessTokenAsync(string url, string realm, string clientSecret, KeycloakOptions options = null)
        {
            options ??= new();
            var result = await url
                .AppendPathSegment($"{options.Prefix}/realms/{realm}/protocol/openid-connect/token")
                .WithHeader("Content-Type", "application/x-www-form-urlencoded")
                .PostUrlEncodedAsync(new List<KeyValuePair<string, string>>
                {
                    new("grant_type", "client_credentials"),
                    new("client_secret", clientSecret),
                    new("client_id", options.AdminClientId)
                })
                .ReceiveJson().ConfigureAwait(false);

            string accessToken = result
                .access_token.ToString();

            return accessToken;
        }

        private static string GetAccessToken(string url, string realm, string clientSecret, KeycloakOptions options = null) => GetAccessTokenAsync(url, realm, clientSecret, options).GetAwaiter().GetResult();

        public static IFlurlRequest WithAuthentication(this IFlurlRequest request, Func<string> getToken, string url, string realm, string userName, string password, string clientSecret, KeycloakOptions options = null)
        {
            string token = null;

            if (getToken != null)
            {
                token = getToken();
            }
            else if (clientSecret != null)
            {
                token = GetAccessToken(url, realm, clientSecret, options);
            }
            else
            {
                token = GetAccessToken(url, realm, userName, password, options);
            }

            return request.WithOAuthBearerToken(token);
        }

        public static IFlurlRequest WithForwardedHttpHeaders(this IFlurlRequest request, ForwardedHttpHeaders forwardedHeaders)
        {
            if (!string.IsNullOrEmpty(forwardedHeaders?.forwardedFor))
            {
                request = request.WithHeader("X-Forwarded-For", forwardedHeaders.forwardedFor);
            }

            if (!string.IsNullOrEmpty(forwardedHeaders?.forwardedProto))
            {
                request = request.WithHeader("X-Forwarded-Proto", forwardedHeaders.forwardedProto);
            }

            if (!string.IsNullOrEmpty(forwardedHeaders?.forwardedHost))
            {
                request = request.WithHeader("X-Forwarded-Host", forwardedHeaders.forwardedHost);
            }

            return request;
        }
    }
}
