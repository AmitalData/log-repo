using RestSharp;
using MicrosoftGraphClient.GraphServices.Base;
using MicrosoftGraphClient.IGraphServices;
using MicrosoftGraphClient.Constants;
using MicrosoftGraphClient.Models.MessagesService;
using MicrosoftGraphClient.GraphAPI;

namespace MicrosoftGraphClient.GraphServices
{
    public class GraphClientMessagesService : GraphClientResourceService<IGraphClientMessagesService>, IGraphClientMessagesService
    {
        public GraphClientMessagesService(string token = null) : base(GraphClientApiUrls.Messages, token) { }

        protected override IGraphClientMessagesService GetInstance()
        {
            return this;
        }

        public Message Get(string messageId)
        {
            return GraphAPICaller.Call<Message>(Token, Url + "/" + messageId, Method.GET);
        }

        public string GetEml(string messageId)
        {
            return GraphAPICaller.Call(Token, Url + "/" + messageId + "/$value", Method.GET);
        }
    }
}