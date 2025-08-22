using BlogTalks.Application.Abstractions;
using Newtonsoft.Json;
using System.Text;

namespace BlogTalks.Infrastructure.Messaging
{
    internal class MessagingHttpService : IMessagingService
    {
        private readonly HttpClient _httpClient;

        public MessagingHttpService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task Send(BlogTalks.Application.Contracts.EmailDto emailDto)
        {
            var json = JsonConvert.SerializeObject(emailDto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("/email", content);

            response.EnsureSuccessStatusCode();
        }
    }
}
