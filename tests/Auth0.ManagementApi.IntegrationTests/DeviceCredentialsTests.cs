﻿using System;
using System.Threading.Tasks;
using Auth0.ManagementApi.Models;
using Auth0.Tests.Shared;
using FluentAssertions;
using Xunit;

namespace Auth0.ManagementApi.IntegrationTests
{
    public class DeviceCredentialsTests : TestBase, IAsyncLifetime
    {
        private Client _client;
        private Connection _connection;
        private User _user;
        private const string Password = "4cX8awB3T%@Aw-R:=h@ae@k?";

        public async Task InitializeAsync()
        {
            using (var apiClient = new ManagementApiClient(GetVariable("AUTH0_TOKEN_DEVICE_CREDENTIALS"), GetVariable("AUTH0_MANAGEMENT_API_URL")))
            {
                // Set up the correct Client, Connection and User
                _client = await apiClient.Clients.CreateAsync(new ClientCreateRequest
                {
                    Name = Guid.NewGuid().ToString("N")
                });
                _connection = await apiClient.Connections.CreateAsync(new ConnectionCreateRequest
                {
                    Name = "Temp-Int-Test-" + MakeRandomName(),
                    Strategy = "auth0",
                    EnabledClients = new[] { _client.ClientId }
                });
                _user = await apiClient.Users.CreateAsync(new UserCreateRequest
                {
                    Connection = _connection.Name,
                    Email = $"{Guid.NewGuid():N}@nonexistingdomain.aaa",
                    EmailVerified = true,
                    Password = Password
                });
            }
        }

        public async Task DisposeAsync()
        {
            using (var apiClient = new ManagementApiClient(GetVariable("AUTH0_TOKEN_DEVICE_CREDENTIALS"), GetVariable("AUTH0_MANAGEMENT_API_URL")))
            {
                await apiClient.Clients.DeleteAsync(_client.ClientId);
                await apiClient.Connections.DeleteAsync(_connection.Id);
                await apiClient.Users.DeleteAsync(_user.UserId);
            }
        }

        [Fact(Skip = "Can't create device credentials using management API v2 token")]
        public async Task Test_device_credentials_crud_sequence()
        {
            using (var apiClient = new ManagementApiClient(GetVariable("AUTH0_TOKEN_DEVICE_CREDENTIALS"), GetVariable("AUTH0_MANAGEMENT_API_URL")))
            {
                //Get all the device credentials
                var credentialsBefore = await apiClient.DeviceCredentials.GetAllAsync();

                //Create a new device credential
                var newCredentialRequest = new DeviceCredentialCreateRequest
                {
                    DeviceName = Guid.NewGuid().ToString("N"),
                    DeviceId = Guid.NewGuid().ToString("N"),
                    ClientId = _client.ClientId,
                    Type = "public_key",
                    Value = "new-key-value"
                };
                var newCredentialResponse = await apiClient.DeviceCredentials.CreateAsync(newCredentialRequest);
                newCredentialResponse.Should().NotBeNull();
                newCredentialResponse.DeviceId.Should().Be(newCredentialRequest.DeviceId);
                newCredentialResponse.DeviceName.Should().Be(newCredentialRequest.DeviceName);
                newCredentialResponse.ClientId.Should().Be(newCredentialRequest.ClientId);

                // Check that we now have one more device credential
                var credentialsAfterCreate = await apiClient.DeviceCredentials.GetAllAsync();
                credentialsAfterCreate.Count.Should().Be(credentialsBefore.Count + 1);

                // Delete the device credential
                await apiClient.DeviceCredentials.DeleteAsync(newCredentialResponse.Id);

                // Check that we now have one less device credential
                var credentialsAfterDelete = await apiClient.DeviceCredentials.GetAllAsync();
                credentialsAfterDelete.Count.Should().Be(credentialsAfterCreate.Count - 1);
            }
        }
    }
}