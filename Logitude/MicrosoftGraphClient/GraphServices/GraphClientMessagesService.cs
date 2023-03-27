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
        public GraphClientMessagesService(string token = null) : base(GraphClientApiUrls.Messages, token) { }

        protected override IGraphClientMessagesService GetInstance()
        {
            return this;
        }

        public Message Get(string messageId)
        {
            return GraphAPICaller.Call<Message>(new GraphAPICallerParams { Token = Token, Url = Url + "/" + messageId, Method = Method.GET });
        }

        public string GetEml(string messageId)
        {
            return GraphAPICaller.Call(new GraphAPICallerParams { Token = Token, Url = Url + "/" + messageId + "/$value", Method = Method.GET });
        }
    }
}