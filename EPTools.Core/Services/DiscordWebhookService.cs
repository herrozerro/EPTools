using System.Text;

namespace EPTools.Core.Services
{
    public class DiscordWebhookService(HttpClient httpClient)
    {
        private HttpClient HttpClient { get; set; } = httpClient;

        public async Task SendWebhook()
        {
            var url = "";

            var payload = new StringContent(
                            @"{""content"": ""TEST"", 
	                            ""username"": ""Praxia2"",
	                            ""embeds"":[
		                            {
			                            ""title"":""Rolled Result 5"",
                                        ""description"":""Rolled a thing"",
                                        ""author"": { ""name"": ""Rolled Freefall""},
                                        ""footer"": { ""text"":""Something""}
		                            }
                                ]
                            }", Encoding.UTF8, "application/json"); 
            await HttpClient.PostAsync(url, payload);
        }
    }
}
