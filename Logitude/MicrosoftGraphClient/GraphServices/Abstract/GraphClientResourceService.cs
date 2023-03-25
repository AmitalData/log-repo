namespace MicrosoftGraphClient.GraphServices.Abstract
{
    public abstract class GraphClientResourceService
    {
        protected string Url { get; set; }
        protected string Token { get; set; }

        public GraphClientResourceService(string url, string token)
        {
            Url = url;
            Token = token;
        }

        public void SetToken(string token)
        {
            Token = token;
        }
    }
}