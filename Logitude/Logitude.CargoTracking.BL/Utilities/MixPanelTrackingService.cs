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
        const string TrackEventAPIURI = "https://api.mixpanel.com/track#live-event";

        public MixPanelTrackingService()
        {
        }

        public async Task TrackSearchActionAsync(MixPanelTrackingEvent trackingEvent)
        {
            var timeInUnix = DateTimeOffset.Now.ToUnixTimeSeconds();
            var values = new Dictionary<string, string>
                {
                    {
                    "data", 
                        "{ \"event\": \"Search\"," +
                        " \"properties\": {" +
                            " \"distinct_id\": \"13793\"," +
                            " \"token\": \"24322335b969b8bc7cc9e3fd6af39aa0\"," +
                            " \"time\":\""+ timeInUnix +"\"," +
                            " \"search_key\":\"" + trackingEvent.SearchKeyword +"\"," +
                            " \"user_agent\":\"" + trackingEvent.UserAgent +"\"," +
                            " \"ip_address\":\"" + trackingEvent.IPAddress +"\"," +
                            " \"browser\":\"" + trackingEvent.Browser +"\"," +
                            " \"results_count\":\"" + trackingEvent.ResultsCount.ToString() +"\"" +
                            "}" +


                        "}"
                         
                    }
                };
            try
            {
                var content = new FormUrlEncodedContent(values);

                var response = await client.PostAsync(TrackEventAPIURI, content);

                var responseString = await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {

                throw;
            }
            
        }

    }
}
