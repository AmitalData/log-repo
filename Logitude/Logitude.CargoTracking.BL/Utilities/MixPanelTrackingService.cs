using Logitude.CargoTracking.BL.DataContracts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;

namespace Logitude.CargoTracking.BL.Utilities
{
    public class MixPanelTrackingService
    {
        private static readonly HttpClient client = new HttpClient();
        private const string trackEventAPIURI = "https://api.mixpanel.com/track#live-event";

        public MixPanelTrackingService()
        {
        }

        public MixPanelTrackingEvent CreateMixPanelEvent(string searchkey, int count, string eventName)
        {
              MixPanelTrackingEvent trackingEvent = new MixPanelTrackingEvent()
                {
                  EventName= eventName,
                    SearchKeyword = searchkey,
                    UserAgent = HttpContext.Current.Request.UserAgent,
                    Browser = GetBrowserName(),
                    ResultsCount = count,
                    IPAddress = HttpContext.Current.Request.UserHostAddress
                };
            return trackingEvent;
        }

        public async Task SendTrackingEventPostRequest(MixPanelTrackingEvent trackingEvent)
        {
            Dictionary<string, string> requestParameters = BuildHttpRequestParameters(trackingEvent);
            var trackEventResponse = await client.PostAsync(trackEventAPIURI, new FormUrlEncodedContent(requestParameters));
            var trackEventResponseMessage = await trackEventResponse.Content.ReadAsStringAsync();
        }

        private static Dictionary<string, string> BuildHttpRequestParameters(MixPanelTrackingEvent trackingEvent)
        {
            return new Dictionary<string, string>
                {
                    {
                    "data",
                        "{ \"event\": \"" + trackingEvent.EventName +"\"," +
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
        private static string GetBrowserName()
        {
            var userAgent = HttpContext.Current.Request.UserAgent;
            var userBrowser = new HttpBrowserCapabilities { Capabilities = new Hashtable { { string.Empty, userAgent } } };
            var factory = new BrowserCapabilitiesFactory();
            factory.ConfigureBrowserCapabilities(new NameValueCollection(), userBrowser);
            var browser = userBrowser.Browser;
            return browser;
        }
    }
}
