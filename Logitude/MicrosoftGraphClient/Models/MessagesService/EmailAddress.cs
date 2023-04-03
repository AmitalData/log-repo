using Newtonsoft.Json;

namespace MicrosoftGraphClient.Models.MessagesService
{
    public class EmailAddress
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }
    }
}