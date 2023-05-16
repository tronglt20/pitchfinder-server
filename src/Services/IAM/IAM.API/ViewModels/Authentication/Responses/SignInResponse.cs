namespace IAM.API.ViewModels.Authentication.Responses
{
    public class SignInResponse 
    {
        public string HomeUrl { get; set; }
        public string TokenType { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public int ExpiresIn { get; set; }
    }
}
