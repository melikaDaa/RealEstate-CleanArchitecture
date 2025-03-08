using System.Net.Http;

namespace RealEstateApp.Presentation.UI.Client.Data
{
    public class UserService
    {
        private readonly HttpClient httpClient;

        public UserService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        // دریافت تمام نقش‌ها
        public async Task<List<string>> GetRolesAsync()
        {
            return await httpClient.GetFromJsonAsync<List<string>>("https://localhost:7044/api/User/roles");
        }

    }
}
