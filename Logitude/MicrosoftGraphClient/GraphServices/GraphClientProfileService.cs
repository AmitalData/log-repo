using RestSharp;
using MicrosoftGraphClient.GraphServices.Abstract;
using MicrosoftGraphClient.IGraphServices;
using MicrosoftGraphClient.Constants;
using MicrosoftGraphClient.Models;
using MicrosoftGraphClient.GraphAPI;

namespace MicrosoftGraphClient.GraphServices
{
    public class GraphClientProfileService : GraphClientResourceService, IGraphClientProfileService
    {
        public GraphClientProfileService(string token = null) : base(GraphClientApiUrls.Profile, token) { }

        public Profile Get()
        {
            return GraphAPICaller.Call<Profile>(Token, Url, Method.GET);
        }
    }
}