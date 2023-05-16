namespace Shared.Infrastructure.Dtos
{
    public class IdentitySettings
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string Scope { get; set; }
        public string Authority { get; set; }
    }
}
