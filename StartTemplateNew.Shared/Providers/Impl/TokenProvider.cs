using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using StartTemplateNew.Shared.Exceptions;
using StartTemplateNew.Shared.Helpers.Extensions;
using StartTemplateNew.Shared.Models;
using StartTemplateNew.Shared.Models.Dto.Requests;
using StartTemplateNew.Shared.Models.Dto.Responses;
using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace StartTemplateNew.Shared.Providers.Impl
{
    [SuppressMessage("Roslynator", "RCS1163:Unused parameter")]
    [SuppressMessage("Style", "IDE0060:Remove unused parameter")]
    [SuppressMessage("Major Code Smell", "S1172:Unused method parameters should be removed")]
    public class TokenProvider(IOptions<IssuerCredentials> options) : ITokenProvider
    {
        private readonly IssuerCredentials _issuerCredentials = options.Value;

        public async Task<TokenResponse> GenerateTokenAsync(TokenRequest tokenRequest)
        {
            try
            {
                //string? refreshToken = tokenRequest.RefreshToken
                string clientId = tokenRequest.ClientId;
                string clientSecret = tokenRequest.ClientSecret;
                string grantType = tokenRequest.GrantType;

                if (grantType == "client_credentials")
                {
                    // Validazione del client

                    // Generazione del token
                    return await GenerateJwtTokenAsync(clientId, clientSecret).ConfigureAwait(false);
                }
                else if (grantType == "password")
                {
                    // Validazione dell'utente
                    string? username = tokenRequest.Username;
                    string? password = tokenRequest.Password;

                    if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                        throw new TokenGenerationException("Username and password are required for password grant type");

                    // Implementa la logica di validazione dell'utente qui
                    bool isValidUser = ValidateUser(username, password);
                    if (!isValidUser)
                        throw new TokenGenerationException("Invalid username or password");

                    // Generazione del token
                    return await GenerateJwtTokenAsync(clientId, clientSecret).ConfigureAwait(false);
                }
                else if (grantType == "authorization_code")
                {
                    // Validazione del codice di autorizzazione
                    string? code = tokenRequest.Code;
                    if (string.IsNullOrEmpty(code))
                        throw new TokenGenerationException("Authorization code is required for authorization_code grant type");

                    // Implementa la logica di validazione del codice di autorizzazione qui
                    bool isValidCode = ValidateAuthorizationCode(code, clientId, clientSecret);
                    if (!isValidCode)
                        throw new TokenGenerationException("Invalid authorization code");

                    // Generazione del token
                    return await GenerateJwtTokenAsync(clientId, clientSecret).ConfigureAwait(false);
                }
                else if (grantType == "refresh_token")
                {
                    // Implement refresh token logic here
                    // Validate the refresh token and generate a new access token
                    throw new NotImplementedException("Refresh token grant type is not implemented");
                }
                else
                {
                    throw new TokenGenerationException("Unsupported grant type");
                }
            }
            catch (SecurityTokenEncryptionFailedException ex)
            {
                throw new TokenGenerationException($"Encryption of the token failed.\n{ex.GetFullMessage()}", ex);
            }
            catch (Exception ex)
            {
                throw new TokenGenerationException($"Error generating token.\n{ex.GetFullMessage()}", ex);
            }
        }

        public TokenResponse GenerateToken(TokenRequest tokenRequest)
        {
            try
            {
                //string? refreshToken = tokenRequest.RefreshToken
                string clientId = tokenRequest.ClientId;
                string clientSecret = tokenRequest.ClientSecret;
                string grantType = tokenRequest.GrantType;

                if (grantType == "client_credentials")
                {
                    // Validazione del client

                    // Generazione del token
                    return GenerateJwtToken(clientId, clientSecret);
                }
                else if (grantType == "password")
                {
                    // Validazione dell'utente
                    string? username = tokenRequest.Username;
                    string? password = tokenRequest.Password;

                    if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                        throw new TokenGenerationException("Username and password are required for password grant type");

                    // Implementa la logica di validazione dell'utente qui
                    bool isValidUser = ValidateUser(username, password);
                    if (!isValidUser)
                        throw new TokenGenerationException("Invalid username or password");

                    // Generazione del token
                    return GenerateJwtToken(clientId, clientSecret);
                }
                else if (grantType == "authorization_code")
                {
                    // Validazione del codice di autorizzazione
                    string? code = tokenRequest.Code;
                    if (string.IsNullOrEmpty(code))
                        throw new TokenGenerationException("Authorization code is required for authorization_code grant type");

                    // Implementa la logica di validazione del codice di autorizzazione qui
                    bool isValidCode = ValidateAuthorizationCode(code, clientId, clientSecret);
                    if (!isValidCode)
                        throw new TokenGenerationException("Invalid authorization code");

                    // Generazione del token
                    return GenerateJwtToken(clientId, clientSecret);
                }
                else if (grantType == "refresh_token")
                {
                    // Implement refresh token logic here
                    // Validate the refresh token and generate a new access token
                    throw new NotImplementedException("Refresh token grant type is not implemented");
                }
                else
                {
                    throw new TokenGenerationException("Unsupported grant type");
                }
            }
            catch (SecurityTokenEncryptionFailedException ex)
            {
                throw new TokenGenerationException($"Encryption of the token failed.\n{ex.GetFullMessage()}", ex);
            }
            catch (Exception ex)
            {
                throw new TokenGenerationException($"Error generating token.\n{ex.GetFullMessage()}", ex);
            }
        }

        private Task<TokenResponse> GenerateJwtTokenAsync(string clientId, string clientSecret)
        {
            Claim[] claims =
            [
                new Claim(JwtRegisteredClaimNames.Sub, clientId),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            ];

            SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(clientSecret));
            SigningCredentials creds = new(key, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken token = new(
                issuer: _issuerCredentials.Issuer,
                audience: clientId,
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return Task.FromResult(new TokenResponse()
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                TokenType = "Bearer",
                ExpiresIn = 1800,
                Scope = "read write"
            });
        }

        private TokenResponse GenerateJwtToken(string clientId, string clientSecret)
        {
            Claim[] claims =
            [
                new Claim(JwtRegisteredClaimNames.Sub, clientId),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            ];

            SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(clientSecret));
            SigningCredentials creds = new(key, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken token = new(
                issuer: _issuerCredentials.Issuer,
                audience: clientId,
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return new TokenResponse()
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                TokenType = "Bearer",
                ExpiresIn = 1800,
                Scope = "read write"
            };
        }

        private static bool ValidateUser(string username, string password)
        {
            // Implementa la logica di validazione dell'utente qui
            // Ad esempio, puoi controllare le credenziali in un database
            return true; // Sostituisci con la logica di validazione reale
        }

        private static bool ValidateAuthorizationCode(string code, string clientId, string clientSecret)
        {
            // Implementa la logica di validazione del codice di autorizzazione qui
            // Ad esempio, puoi controllare il codice in un database o in memoria
            return true; // Sostituisci con la logica di validazione reale
        }
    }
}
