﻿using Auth0.AuthenticationApi.Models;
using System;
using System.Threading.Tasks;

namespace Auth0.AuthenticationApi
{
    /// <summary>
    /// Client for communicating with the Auth0 Authentication API.
    /// </summary>
    /// <remarks>
    /// Full documentation for the Authentication API is available at https://auth0.com/docs/auth-api
    /// </remarks>
    public interface IAuthenticationApiClient
    {
        /// <summary>
        /// Base URI that will be used for all the requests.
        /// </summary>
        Uri BaseUri { get; }

        /// <summary>
        /// Given the user's details, Auth0 will send a forgot password email.
        /// </summary>
        /// <param name="request">The <see cref="ChangePasswordRequest"/> specifying the user and connection details.</param>
        /// <returns>A task object with a string containing the message returned from Auth0.</returns>
        Task<string> ChangePasswordAsync(ChangePasswordRequest request);

        /// <summary>
        /// Generates a link that can be used once to log in as a specific user.
        /// </summary>
        /// <param name="request">The <see cref="ImpersonationRequest"/> containing the details of the user to impersonate.</param>
        /// <returns>A <see cref="Uri"/> which can be used to sign in as the specified user.</returns>
        Task<Uri> GetImpersonationUrlAsync(ImpersonationRequest request);

        /// <summary>
        /// Returns the SAML 2.0 meta data for a client.
        /// </summary>
        /// <param name="clientId">The client (App) ID for which meta data must be returned.</param>
        /// <returns>The meta data XML .</returns>
        Task<string> GetSamlMetadataAsync(string clientId);

        /// <summary>
        /// Returns the user information based on the Auth0 access token (obtained during login).
        /// </summary>
        /// <param name="accessToken">The access token.</param>
        /// <returns>The <see cref="UserInfo"/> associated with the token.</returns>
        Task<UserInfo> GetUserInfoAsync(string accessToken);

        /// <summary>
        /// Returns the WS Federation meta data.
        /// </summary>
        /// <returns>The meta data XML</returns>
        Task<string> GetWsFedMetadataAsync();

        /// <summary>
        /// Given the user credentials, the connection specified and the Auth0 account information, it will create a new user. 
        /// </summary>
        /// <param name="request">The <see cref="SignupUserRequest"/> containing information of the user to sign up.</param>
        /// <returns>A <see cref="SignupUserResponse"/> with the information of the signed up user.</returns>
        Task<SignupUserResponse> SignupUserAsync(SignupUserRequest request);

        /// <summary>
        /// Starts a new Passwordless email flow.
        /// </summary>
        /// <param name="request">The <see cref="PasswordlessEmailRequest"/> containing the information about the new Passwordless flow to start.</param>
        /// <returns>A <see cref="PasswordlessEmailResponse"/> containing the response.</returns>
        Task<PasswordlessEmailResponse> StartPasswordlessEmailFlowAsync(PasswordlessEmailRequest request);

        /// <summary>
        /// Starts a new Passwordless SMS flow.
        /// </summary>
        /// <param name="request">The <see cref="PasswordlessSmsRequest"/> containing the information about the new Passwordless flow to start.</param>
        /// <returns>A <see cref="PasswordlessSmsResponse"/> containing the response.</returns>
        Task<PasswordlessSmsResponse> StartPasswordlessSmsFlowAsync(PasswordlessSmsRequest request);

        /// <summary>
        /// Unlinks a secondary account from a primary account.
        /// </summary>
        /// <param name="request">The <see cref="UnlinkUserRequest"/> containing the information of the accounts to unlink.</param>
        /// <returns>Nothing</returns>
        Task UnlinkUserAsync(UnlinkUserRequest request);

        /// <summary>
        /// Request an Access Token using the Authorization Code Grant flow.
        /// </summary>
        /// <param name="request">The <see cref="ClientCredentialsTokenRequest"/> containing the information of the request.</param>
        /// <returns>An <see cref="AccessTokenResponse"/> containing the token information</returns>
        Task<AccessTokenResponse> GetTokenAsync(AuthorizationCodeTokenRequest request);

        /// <summary>
        /// Request an Access Token using the Authorization Code (PKCE) flow.
        /// </summary>
        /// <param name="request">The <see cref="ClientCredentialsTokenRequest"/> containing the information of the request.</param>
        /// <returns>An <see cref="AccessTokenResponse"/> containing the token information</returns>
        Task<AccessTokenResponse> GetTokenAsync(AuthorizationCodePkceTokenRequest request);

        /// <summary>
        /// Request an Access Token using the Client Credentials Grant flow.
        /// </summary>
        /// <param name="request">The <see cref="ClientCredentialsTokenRequest"/> containing the information of the request.</param>
        /// <returns>An <see cref="AccessTokenResponse"/> containing the token information</returns>
        Task<AccessTokenResponse> GetTokenAsync(ClientCredentialsTokenRequest request);

        /// <summary>
        /// Given a <see cref="RefreshTokenRequest"/>, it will retrieve a refreshed access token from the authorization server.
        /// </summary>
        /// <param name="request">The refresh token request details, containing a valid refresh token.</param>
        /// <returns>The new token issued by the server.</returns>
        Task<AccessTokenResponse> GetTokenAsync(RefreshTokenRequest request);

        /// <summary>
        /// Given an <see cref="ResourceOwnerTokenRequest" />, it will do the authentication on the provider and return an <see cref="AccessTokenResponse"/>.
        /// </summary>
        /// <param name="request">The authentication request details containing information regarding the username, password etc.</param>
        /// <returns>An <see cref="AccessTokenResponse" /> with the response.</returns>
        Task<AccessTokenResponse> GetTokenAsync(ResourceOwnerTokenRequest request);
    }
}