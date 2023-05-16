using IAM.API.ViewModels.Authentication.Responses;
using Shared.Infrastructure.Dtos;

namespace IAM.API.Services
{
    public partial class AuthenticationService
    {
        protected readonly IdentitySettings IdentitySettings;

        public AuthenticationService(IdentitySettings identitySettings)
        {
            IdentitySettings = identitySettings;
        }

        public async Task<SignInResponse> SignInAsync(string userName
            , string password)
        {
            var client = new HttpClient();

            var disco = await GetDiscoveryDocumentAsync(client, IdentitySettings.Authority);

            if (disco.IsError)
                throw new Exception(disco.Exception.Message);

            var tokenResponse = await RequestPasswordTokenAsync(client
                , disco
                , IdentitySettings
                , userName
                , password);

            if (tokenResponse.IsError)
                throw new Exception(tokenResponse.Exception.Message);

            return new SignInResponse
            {
                TokenType = tokenResponse.TokenType,
                AccessToken = tokenResponse.AccessToken,
                RefreshToken = tokenResponse.RefreshToken,
                ExpiresIn = tokenResponse.ExpiresIn,
            };
        }
    }
}
