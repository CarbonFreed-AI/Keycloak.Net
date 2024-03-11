﻿using Keycloak.Net.Models.OpenIDConfiguration;

namespace Keycloak.Net
{
    public partial class KeycloakClient
    {
        public async Task<OpenIDConfiguration> GetOpenIDConfigurationAsync(string realm, CancellationToken cancellationToken = default) => await GetBaseUrl(realm)
            .AppendPathSegment($"/realms/{realm}/.well-known/openid-configuration")
            .GetJsonAsync<OpenIDConfiguration>(HttpCompletionOption.ResponseContentRead, cancellationToken)
            .ConfigureAwait(false);
    }
}
