namespace Cardmanagement.WebUI.Models.Authentication
{
    public class LoginResponse
    {
        public string Token { get; set; }
        public DateTime expiration { get; set; }
    }
}
