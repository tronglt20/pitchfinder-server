using IdentityModel.Client;

namespace IAM.API.ViewModels.Authentication.Responses
{
    public class SignInResponse
    {
        public SignInResponse(TokenResponse tokenResponse)
        {
            TokenType = tokenResponse.TokenType;
            AccessToken = tokenResponse.AccessToken;
            RefreshToken = tokenResponse.RefreshToken;
            ExpiresIn = tokenResponse.ExpiresIn;
        }

        public string HomeUrl { get; set; }
        public string TokenType { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public int ExpiresIn { get; set; }
    }
}
