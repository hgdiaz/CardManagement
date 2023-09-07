using Blazored.LocalStorage;
using Cardmanagement.WebUI.Models;
using Cardmanagement.WebUI.Models.Authentication;
using Cardmanagement.WebUI.Pages;
using Cardmanagement.WebUI.Pages.Authentication;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace Cardmanagement.WebUI.Services
{
    public class CardAPIService
    {
        private readonly HttpClient _httpClient;
        private ILocalStorageService _localStorage;

        public CardAPIService(HttpClient httpClient, ILocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
        }

        private async Task<string> getToken()
        {
            var token = await _localStorage.GetItemAsync<string>("token");
            //TODO: check if token has expired
            return token;
        }

        public async Task<LoginResponse> Login(LoginRequest login)
        {
            using HttpResponseMessage response = await _httpClient.PostAsJsonAsync("/api/Authenticate/Login", login);
            try
            {
                response.EnsureSuccessStatusCode();
                string content = await response.Content.ReadAsStringAsync();
                LoginResponse resp = JsonConvert.DeserializeObject<LoginResponse>(content);

                await _localStorage.SetItemAsync("token", resp.Token);

                return resp;
            }
            catch (Exception)
            {

                return null;
            }
        }

        public async Task Logout()
        { }

        public async Task Register(RegisterRequest register)
        {
            using HttpResponseMessage response = await _httpClient.PostAsJsonAsync("/api/Authenticate/register", register);
            try
            {
                response.EnsureSuccessStatusCode();
                string content = await response.Content.ReadAsStringAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task RegisterAdmin(RegisterRequest register)
        {
            using HttpResponseMessage response = await _httpClient.PostAsJsonAsync("/api/Authenticate/register-admin", register);
            try
            {
                response.EnsureSuccessStatusCode();
                string content = await response.Content.ReadAsStringAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<GetAllCardsResponse>> GetAllCards()
        {

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getToken());
            using HttpResponseMessage response = await _httpClient.GetAsync("/api/card/getAll");
            try
            {
                response.EnsureSuccessStatusCode();
                string content = await response.Content.ReadAsStringAsync();
                List<GetAllCardsResponse> resp = JsonConvert.DeserializeObject<List<GetAllCardsResponse>>(content);
                return resp;
            }
            catch (Exception)
            {

                return null;
            }
        }

        public async Task<GetCardByNumberQueryResponse> Get(string number)
        {

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getToken());
            using HttpResponseMessage response = await _httpClient.GetAsync($"/api/card/{number}");
            try
            {
                response.EnsureSuccessStatusCode();
                string content = await response.Content.ReadAsStringAsync();
                GetCardByNumberQueryResponse resp = JsonConvert.DeserializeObject<GetCardByNumberQueryResponse>(content);
                return resp;
            }
            catch (Exception)
            {

                return null;
            }
        }

        public async Task<int> Create(AddCard card)
        {

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getToken());
            string str = JsonConvert.SerializeObject(card);
            using HttpResponseMessage response = await _httpClient.PostAsync($"/api/card", new StringContent(str, Encoding.UTF8, "application/json"));
            try
            {
                response.EnsureSuccessStatusCode();
                string content = await response.Content.ReadAsStringAsync();
                int resp = JsonConvert.DeserializeObject<int>(content);
                return resp;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public async Task<bool> Update(EditCard card)
        {

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getToken());
            string str = JsonConvert.SerializeObject(card);
            using HttpResponseMessage response = await _httpClient.PutAsync($"/api/card", new StringContent(str, Encoding.UTF8, "application/json"));
            try
            {
                response.EnsureSuccessStatusCode();
                return true;
                //string content = await response.Content.ReadAsStringAsync();
                //bool resp = JsonConvert.DeserializeObject<bool>(content);
                //return resp;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> Delete(int id)
        {

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getToken());
            using HttpResponseMessage response = await _httpClient.DeleteAsync($"/api/card/{id}");
            try
            {
                response.EnsureSuccessStatusCode();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

    }
}
