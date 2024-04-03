using Newtonsoft.Json;

namespace MicrosoftGraphClient.Models.MessagesService
{
    public class Recipient
    {
        [JsonProperty("emailAddress")]
        public EmailAddress EmailAddress { get; set; }
    }
}