using Newtonsoft.Json;

namespace MicrosoftGraphClient.Models.MessagesService
{
    public class Body
    {
        [JsonProperty("contentType")]
        public string ContentType { get; set; }

        [JsonProperty("content")]
        public string Content { get; set; }
    }
}