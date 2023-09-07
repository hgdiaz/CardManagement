using Blazored.LocalStorage;
using Cardmanagement.WebUI.Models.Authentication;
using Microsoft.AspNetCore.Components.Authorization;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Cardmanagement.WebUI.Services
{
    public class CustomStateProvider : AuthenticationStateProvider
    {
        private readonly CardAPIService _api;
        private CurrentUser _currentUser;
        private ILocalStorageService _localStorage;

        public CustomStateProvider(CardAPIService api, ILocalStorageService localStorage)
        {
            _api = api;
            _localStorage = localStorage;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var identity = new ClaimsIdentity();
            try
            {
                var userInfo = await GetCurrentUser();
                if (userInfo.IsAuthenticated)
                {
                    var claims = new[] { new Claim(ClaimTypes.Name, _currentUser.UserName) }.Concat(_currentUser.Claims.Select(c => new Claim(c.Key, c.Value)));
                    identity = new ClaimsIdentity(claims, "Server authentication");
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine("Request failed:" + ex.ToString());
            }

            return new AuthenticationState(new ClaimsPrincipal(identity));
        }

        private async Task<CurrentUser> GetCurrentUser()
        {
            //if (_currentUser != null && _currentUser.IsAuthenticated) return _currentUser;

            _currentUser = new CurrentUser() { UserName = "", IsAuthenticated = false };
            try
            {
                var token = await _localStorage.GetItemAsync<string>("token");
                if (!string.IsNullOrEmpty(token))
                {
                    var handler = new JwtSecurityTokenHandler();
                    var jsonToken = handler.ReadToken(token);
                    var tokenS = jsonToken as JwtSecurityToken;
                    string user = tokenS.Claims.First(claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name").Value;
                    string role = tokenS.Claims.First(claim => claim.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role").Value;
                    var claims = new Dictionary<string, string>();
                    claims.Add("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", user);
                    claims.Add("http://schemas.microsoft.com/ws/2008/06/identity/claims/role", role);
                    _currentUser = new CurrentUser() { UserName = user, IsAuthenticated = true, Claims = claims };
                }
            }
            catch (Exception)
            {
            }

            return _currentUser;
        }
        public async Task Logout()
        {
            await _api.Logout();
            _currentUser = null;
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
        public async Task Login(LoginRequest loginParameters)
        {
            await _api.Login(loginParameters);
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
        public async Task Register(RegisterRequest registerParameters)
        {
            await _api.Register(registerParameters);
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
        public async Task RegisterAdmin(RegisterRequest registerParameters)
        {
            await _api.RegisterAdmin(registerParameters);
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
    }
}
