using Logitude.CargoTracking.BL.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CargoTracking.BL.Utilities
{
    public class MixPanelTrackingService
    {
        private static readonly HttpClient client = new HttpClient();
        private const string trackEventAPIURI = "https://api.mixpanel.com/track#live-event";

        public MixPanelTrackingService()
        {
        }

        public async Task TrackSearchActionAsync(MixPanelTrackingEvent trackingEvent)
        {
            Dictionary<string, string> requestParameters = BuildHttpRequestParameters(trackingEvent);
            var response = await client.PostAsync(trackEventAPIURI, new FormUrlEncodedContent(requestParameters));
            var responseString = await response.Content.ReadAsStringAsync();
        }

        private static Dictionary<string, string> BuildHttpRequestParameters(MixPanelTrackingEvent trackingEvent)
        {
            return new Dictionary<string, string>
                {
                    {
                    "data",
                        "{ \"event\": \"Search\"," +
                        " \"properties\": {" +
                            " \"distinct_id\": \"13793\"," +
                            " \"token\": \"24322335b969b8bc7cc9e3fd6af39aa0\"," +
                            " \"time\":\""+ GetCurrentDateInUnixFormat() +"\"," +
                            " \"search_key\":\"" + trackingEvent.SearchKeyword +"\"," +
                            " \"user_agent\":\"" + trackingEvent.UserAgent +"\"," +
                            " \"ip_address\":\"" + trackingEvent.IPAddress +"\"," +
                            " \"browser\":\"" + trackingEvent.Browser +"\"," +
                            " \"results_count\":\"" + trackingEvent.ResultsCount.ToString() +"\"" +
                            "}" +
                        "}"
                    }
                };
        }

        private static long GetCurrentDateInUnixFormat()
        {
            return DateTimeOffset.Now.ToUnixTimeSeconds();
        }
    }
}
