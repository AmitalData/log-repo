namespace MicrosoftGraphClient.Constants
{
    public static class GraphClientApiUrls
    {
        public static readonly string Profile = GraphClientConfigurations.BaseUrl + "me";
        public static readonly string Messages = GraphClientConfigurations.BaseUrl + "me/messages";
        public static readonly string Subscriptions = GraphClientConfigurations.BaseUrl + "subscriptions";
    }
}