﻿using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Keycloak.Net.Tests;

public partial class KeycloakClientShould
{
    [Theory]
    [InlineData("Insurance", "insurance-product")]
    public async Task GetClientRoleMappingsForGroupAsync(string realm, string clientId)
    {
        var groups = await _client.GetGroupHierarchyAsync(realm);
        string groupId = groups.FirstOrDefault()?.Id;
        if (groupId != null)
        {
            var clients = await _client.GetClientsAsync(realm);
            string clientsId = clients.FirstOrDefault(x => x.ClientId == clientId)?.Id;
            if (clientId != null)
            {
                var result = await _client.GetClientRoleMappingsForGroupAsync(realm, groupId, clientsId);
                Assert.NotNull(result);
            }
        }
    }

    [Theory]
    [InlineData("Insurance", "insurance-product")]
    public async Task GetAvailableClientRoleMappingsForGroupAsync(string realm, string clientId)
    {
        var groups = await _client.GetGroupHierarchyAsync(realm);
        string groupId = groups.FirstOrDefault()?.Id;
        if (groupId != null)
        {
            var clients = await _client.GetClientsAsync(realm);
            string clientsId = clients.FirstOrDefault(x => x.ClientId == clientId)?.Id;
            if (clientId != null)
            {
                var result = await _client.GetAvailableClientRoleMappingsForGroupAsync(realm, groupId, clientsId);
                Assert.NotNull(result);
            }
        }
    }

    [Theory]
    [InlineData("Insurance", "insurance-product")]
    public async Task GetEffectiveClientRoleMappingsForGroupAsync(string realm, string clientId)
    {
        var groups = await _client.GetGroupHierarchyAsync(realm);
        string groupId = groups.FirstOrDefault()?.Id;
        if (groupId != null)
        {
            var clients = await _client.GetClientsAsync(realm);
            string clientsId = clients.FirstOrDefault(x => x.ClientId == clientId)?.Id;
            if (clientId != null)
            {
                var result = await _client.GetEffectiveClientRoleMappingsForGroupAsync(realm, groupId, clientsId);
                Assert.NotNull(result);
            }
        }
    }

    [Theory]
    [InlineData("Insurance", "insurance-product")]
    public async Task GetClientRoleMappingsForUserAsync(string realm, string clientId)
    {
        var users = await _client.GetUsersAsync(realm);
        string userId = users.FirstOrDefault()?.Id;
        if (userId != null)
        {
            var clients = await _client.GetClientsAsync(realm);
            string clientsId = clients.FirstOrDefault(x => x.ClientId == clientId)?.Id;
            if (clientId != null)
            {
                var result = await _client.GetClientRoleMappingsForUserAsync(realm, userId, clientsId);
                Assert.NotNull(result);
            }
        }
    }

    [Theory]
    [InlineData("Insurance", "insurance-product")]
    public async Task GetAvailableClientRoleMappingsForUserAsync(string realm, string clientId)
    {
        var users = await _client.GetUsersAsync(realm);
        string userId = users.FirstOrDefault()?.Id;
        if (userId != null)
        {
            var clients = await _client.GetClientsAsync(realm);
            string clientsId = clients.FirstOrDefault(x => x.ClientId == clientId)?.Id;
            if (clientId != null)
            {
                var result = await _client.GetAvailableClientRoleMappingsForUserAsync(realm, userId, clientsId);
                Assert.NotNull(result);
            }
        }
    }

    [Theory]
    [InlineData("Insurance", "insurance-product")]
    public async Task GetEffectiveClientRoleMappingsForUserAsync(string realm, string clientId)
    {
        var users = await _client.GetUsersAsync(realm);
        string userId = users.FirstOrDefault()?.Id;
        if (userId != null)
        {
            var clients = await _client.GetClientsAsync(realm);
            string clientsId = clients.FirstOrDefault(x => x.ClientId == clientId)?.Id;
            if (clientId != null)
            {
                var result = await _client.GetEffectiveClientRoleMappingsForUserAsync(realm, userId, clientsId);
                Assert.NotNull(result);
            }
        }
    }
}