using RestSharp;
using MicrosoftGraphClient.GraphServices.Base;
using MicrosoftGraphClient.IGraphServices;
using MicrosoftGraphClient.Constants;
using MicrosoftGraphClient.Models.MessagesService;
using MicrosoftGraphClient.GraphAPI;
using MicrosoftGraphClient.Models.GraphAPI;

namespace MicrosoftGraphClient.GraphServices
{
    public class GraphClientMessagesService : GraphClientResourceService<IGraphClientMessagesService>, IGraphClientMessagesService
    {
        public GraphClientMessagesService() : base(GraphClientApiUrls.Messages) { }

        protected override IGraphClientMessagesService GetInstance()
        {
            return this;
        }

        public Message Get(string messageId)
        {
            return GraphAPICaller.Call<Message>(new GraphAPICallerParameters { Url = Url + "/" + messageId, AccessToken = AccessToken, Method = Method.Get });
        }

        public string GetEml(string messageId)
        {
            return GraphAPICaller.Call(new GraphAPICallerParameters { Url = Url + "/" + messageId + "/$value", AccessToken = AccessToken, Method = Method.Get });
        }
    }
}