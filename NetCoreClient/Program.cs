using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace NetCoreClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var handler = new HttpClientHandler
            {
                MaxAutomaticRedirections = 5,
            };

            var httpClient = new HttpClient(handler)
            {
                BaseAddress = new Uri("http://localhost:5000"),
            };

            foreach (string method in new[] { "DELETE", "GET", "HEAD", "OPTIONS", "PATCH", "POST", "PUT" })
                foreach (int statusCode in new[] { 300, 301, 302, 303, 307, 308 })
                {
                    var url = $"/Redirect/initial/{statusCode}";
                    var request = new HttpRequestMessage(new HttpMethod(method), url);
                    var result = await httpClient.SendAsync(request);
                    var content = await result.Content.ReadAsStringAsync();
                    Console.WriteLine($"{method} {statusCode} -> {result.StatusCode} {content}");
                }
        }
    }
}
