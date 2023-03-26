namespace MicrosoftGraphClient.GraphServices.Base
{
    public abstract class GraphClientResourceService<T>
    {
        protected string Url { get; set; }
        protected string Token { get; set; }

        public GraphClientResourceService(string url, string token)
        {
            Url = url;
            Token = token;
        }

        protected abstract T GetInstance();

        public T SetToken(string token)
        {
            Token = token;
            return GetInstance();
        }
    }
}