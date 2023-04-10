using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace MicrosoftGraphClient.Models.MessagesService
{
    public class Message
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("createdDateTime")]
        public DateTime CreatedDateTime { get; set; }

        [JsonProperty("lastModifiedDateTime")]
        public DateTime LastModifiedDateTime { get; set; }

        [JsonProperty("changeKey")]
        public string ChangeKey { get; set; }

        [JsonProperty("categories")]
        public List<string> Categories { get; set; }

        [JsonProperty("receivedDateTime")]
        public DateTime ReceivedDateTime { get; set; }

        [JsonProperty("sentDateTime")]
        public DateTime SentDateTime { get; set; }

        [JsonProperty("hasAttachments")]
        public bool HasAttachments { get; set; }

        [JsonProperty("internetMessageId")]
        public string InternetMessageId { get; set; }

        [JsonProperty("subject")]
        public string Subject { get; set; }

        [JsonProperty("bodyPreview")]
        public string BodyPreview { get; set; }

        [JsonProperty("importance")]
        public string Importance { get; set; }

        [JsonProperty("parentFolderId")]
        public string ParentFolderId { get; set; }

        [JsonProperty("conversationId")]
        public string ConversationId { get; set; }

        [JsonProperty("conversationIndex")]
        public string ConversationIndex { get; set; }

        [JsonProperty("isDeliveryReceiptRequested")]
        public bool IsDeliveryReceiptRequested { get; set; }

        [JsonProperty("isReadReceiptRequested")]
        public bool IsReadReceiptRequested { get; set; }

        [JsonProperty("isRead")]
        public bool IsRead { get; set; }

        [JsonProperty("isDraft")]
        public bool IsDraft { get; set; }

        [JsonProperty("webLink")]
        public string WebLink { get; set; }

        [JsonProperty("inferenceClassification")]
        public string InferenceClassification { get; set; }

        [JsonProperty("body")]
        public Body Body { get; set; }

        [JsonProperty("sender")]
        public Recipient Sender { get; set; }

        [JsonProperty("from")]
        public Recipient From { get; set; }

        [JsonProperty("toRecipients")]
        public List<Recipient> ToRecipients { get; set; }

        [JsonProperty("ccRecipients")]
        public List<Recipient> CcRecipients { get; set; }

        [JsonProperty("bccRecipients")]
        public List<Recipient> BccRecipients { get; set; }

        [JsonProperty("replyTo")]
        public List<Recipient> ReplyTo { get; set; }
    }
}