using System.Net.Http.Json;
using System.Text.Json;

namespace MarcoPortefolioServer.Functions.v1.lib.server
{
    public class HttpRequest
    {
        private static readonly HttpClient Client = new()
        {
            Timeout = TimeSpan.FromSeconds(15)
        };

        // ============================================================
        // GET tipado
        // ============================================================
        public static async Task<T?> GetAsync<T>(string url, Dictionary<string, string>? headers = null, bool log = false)
        {
            ApplyHeaders(headers);

            using var response = await Client.GetAsync(url);
            await HandleResponse(response, log);

            return await response.Content.ReadFromJsonAsync<T>();
        }

        // ============================================================
        // POST tipado (envia JSON)
        // ============================================================
        public static async Task<T?> PostAsync<T>(string url, object data, Dictionary<string, string>? headers = null, bool log = false)
        {
            ApplyHeaders(headers);

            using var response = await Client.PostAsJsonAsync(url, data);
            await HandleResponse(response, log);

            return await response.Content.ReadFromJsonAsync<T>();
        }

        // ============================================================
        // PUT tipado
        // ============================================================
        public static async Task<T?> PutAsync<T>(string url, object data, Dictionary<string, string>? headers = null, bool log = false)
        {
            ApplyHeaders(headers);

            using var response = await Client.PutAsJsonAsync(url, data);
            await HandleResponse(response, log);

            return await response.Content.ReadFromJsonAsync<T>();
        }

        // ============================================================
        // DELETE
        // ============================================================
        public static async Task<bool> DeleteAsync(string url, Dictionary<string, string>? headers = null, bool log = false)
        {
            ApplyHeaders(headers);

            using var response = await Client.DeleteAsync(url);
            await HandleResponse(response, log);

            return response.IsSuccessStatusCode;
        }

        // ============================================================
        // Apply Headers
        // ============================================================
        private static void ApplyHeaders(Dictionary<string, string>? headers)
        {
            Client.DefaultRequestHeaders.Clear();

            if (headers != null)
            {
                foreach (var h in headers)
                    Client.DefaultRequestHeaders.Add(h.Key, h.Value);
            }
        }

        // ============================================================
        // Log + erro
        // ============================================================
        private static async Task HandleResponse(HttpResponseMessage response, bool log)
        {
            if (log)
            {
                Console.WriteLine("----- HTTP REQUEST -----");
                Console.WriteLine($"URL: {response.RequestMessage!.RequestUri}");
                Console.WriteLine($"METHOD: {response.RequestMessage.Method}");
                Console.WriteLine($"STATUS: {response.StatusCode}");

                string body = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"BODY: {body}");
                Console.WriteLine("------------------------");
            }

            response.EnsureSuccessStatusCode();
        }
    }
}
