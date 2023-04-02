using RestSharp;
using MicrosoftGraphClient.GraphServices.Base;
using MicrosoftGraphClient.IGraphServices;
using MicrosoftGraphClient.Constants;
using MicrosoftGraphClient.Models.ProfileService;
using MicrosoftGraphClient.GraphAPI;
using MicrosoftGraphClient.Models.GraphAPI;

namespace MicrosoftGraphClient.GraphServices
{
    public class GraphClientProfileService : GraphClientResourceService<IGraphClientProfileService>, IGraphClientProfileService
    {
        public GraphClientProfileService() : base(GraphClientApiUrls.Profile) { }

        protected override IGraphClientProfileService GetInstance()
        {
            return this;
        }

        public Profile Get()
        {
            return GraphAPICaller.Call<Profile>(new GraphAPICallerParameters { Url = Url, AccessToken = AccessToken, Method = Method.GET });
        }
    }
}