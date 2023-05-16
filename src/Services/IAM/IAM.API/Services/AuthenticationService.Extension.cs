using IdentityModel.Client;
using Shared.Infrastructure.Dtos;

namespace IAM.API.Services
{
    public partial class AuthenticationService
    {
        private async Task<TokenResponse> RequestPasswordTokenAsync(HttpClient client
            , DiscoveryDocumentResponse disco
            , IdentitySettings identitySettings
            , string userName
            , string password)
        {
            var passwordRequest =  new PasswordTokenRequest()
            {
                Address = disco.TokenEndpoint,
                ClientId = IdentitySettings.ClientId,
                ClientSecret = IdentitySettings.ClientSecret,
                Scope = IdentitySettings.Scope,
                UserName = userName,
                Password = password,
            };

            return await client.RequestPasswordTokenAsync(passwordRequest);
        }


        private async Task<DiscoveryDocumentResponse> GetDiscoveryDocumentAsync(HttpClient client, string authority)
        {
            return await client.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
            {
                Address = authority,
                Policy =
                {
                    ValidateIssuerName = false,
                    ValidateEndpoints = false,
                    AdditionalEndpointBaseAddresses = GetEndpoints(authority).ToList()
                },
            });
        }

        private IEnumerable<string> GetEndpoints(string authority)
        {
            yield return $"{authority}/.well-known/openid-configuration/jwks";
            yield return $"{authority}/connect/authorize";
            yield return $"{authority}/connect/token";
            yield return $"{authority}/connect/userinfo";
            yield return $"{authority}/connect/endsession";
            yield return $"{authority}/connect/checksession";
            yield return $"{authority}/connect/revocation";
            yield return $"{authority}/connect/introspect";
            yield return $"{authority}/connect/deviceauthorization";
        }
    }
}
