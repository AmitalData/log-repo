using RestSharp;
using MicrosoftGraphClient.GraphServices.Base;
using MicrosoftGraphClient.IGraphServices;
using MicrosoftGraphClient.Constants;
using MicrosoftGraphClient.Models.ProfileService;
using MicrosoftGraphClient.GraphAPI;

namespace MicrosoftGraphClient.GraphServices
{
    public class GraphClientProfileService : GraphClientResourceService<IGraphClientProfileService>, IGraphClientProfileService
    {
        public GraphClientProfileService(string token = null) : base(GraphClientApiUrls.Profile, token) { }

        protected override IGraphClientProfileService GetInstance()
        {
            return this;
        }

        public Profile Get()
        {
            return GraphAPICaller.Call<Profile>(Token, Url, Method.GET);
        }
    }
}