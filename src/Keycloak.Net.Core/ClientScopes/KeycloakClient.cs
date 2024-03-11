﻿using Keycloak.Net.Models.ClientScopes;

namespace Keycloak.Net
{
    public partial class KeycloakClient
    {
        public async Task<bool> CreateClientScopeAsync(string realm, ClientScope clientScope, CancellationToken cancellationToken = default)
        {
            var response = await GetBaseUrl(realm)
                .AppendPathSegment($"/admin/realms/{realm}/client-scopes")
                .PostJsonAsync(clientScope, HttpCompletionOption.ResponseContentRead, cancellationToken)
                .ConfigureAwait(false);
            return response.ResponseMessage.IsSuccessStatusCode;
        }

        public async Task<IEnumerable<ClientScope>> GetClientScopesAsync(string realm, CancellationToken cancellationToken = default) => await GetBaseUrl(realm)
            .AppendPathSegment($"/admin/realms/{realm}/client-scopes")
            .GetJsonAsync<IEnumerable<ClientScope>>(HttpCompletionOption.ResponseContentRead, cancellationToken)
            .ConfigureAwait(false);

        public async Task<ClientScope> GetClientScopeAsync(string realm, string clientScopeId, CancellationToken cancellationToken = default) => await GetBaseUrl(realm)
            .AppendPathSegment($"/admin/realms/{realm}/client-scopes/{clientScopeId}")
            .GetJsonAsync<ClientScope>(HttpCompletionOption.ResponseContentRead, cancellationToken)
            .ConfigureAwait(false);

        public async Task<bool> UpdateClientScopeAsync(string realm, string clientScopeId, ClientScope clientScope, CancellationToken cancellationToken = default)
        {
            var response = await GetBaseUrl(realm)
                .AppendPathSegment($"/admin/realms/{realm}/client-scopes/{clientScopeId}")
                .PutJsonAsync(clientScope, HttpCompletionOption.ResponseContentRead, cancellationToken)
                .ConfigureAwait(false);
            return response.ResponseMessage.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteClientScopeAsync(string realm, string clientScopeId, CancellationToken cancellationToken = default)
        {
            var response = await GetBaseUrl(realm)
                .AppendPathSegment($"/admin/realms/{realm}/client-scopes/{clientScopeId}")
                .DeleteAsync(HttpCompletionOption.ResponseContentRead, cancellationToken)
                .ConfigureAwait(false);
            return response.ResponseMessage.IsSuccessStatusCode;
        }
    }
}
