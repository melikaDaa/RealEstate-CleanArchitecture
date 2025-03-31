using Microsoft.AspNetCore.Components;
using System.Net;

namespace RealEstateApp.Presentation.UI.Client.Errors
{
    public class GlobalErrorHandler
    {
        private readonly HttpClient _httpClient;
        private readonly NavigationManager _navigation;

        public GlobalErrorHandler(HttpClient httpClient, NavigationManager navigation)
        {
            _httpClient = httpClient;
            _navigation = navigation;
        }

        public async Task<T> HandleRequestAsync<T>(Func<Task<HttpResponseMessage>> apiCall)
        {
            try
            {
                var response = await apiCall();

                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();

                    if (response.StatusCode == HttpStatusCode.NotFound)
                        throw new KeyNotFoundException("موردی یافت نشد.");

                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                        _navigation.NavigateTo("/login"); // هدایت به صفحه لاگین

                    throw new Exception($"خطا: {error}");
                }

                return await response.Content.ReadFromJsonAsync<T>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ خطا در درخواست API: {ex.Message}");
                throw;
            }
        }
    }

}
