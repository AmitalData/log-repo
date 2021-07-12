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

namespace Logitude.BL.CommonDataModel.Tools.MixPanelTracker
{
    public class MixPanelEventTracker
    {
        private static readonly HttpClient client = new HttpClient();
        private const string trackEventAPIURI = "https://api.mixpanel.com/track#live-event";
        string masterUserId;
        string projectToken;

        public MixPanelEventTracker(string projectToken)
        {
            this.projectToken = projectToken;
        }
        public MixPanelEventTracker(string projectToken,string masterUserId)
        {
            this.projectToken = projectToken;
            this.masterUserId = masterUserId;
        }

        public async void TrackEvent(MixPanelEvent mixPanelEvent)
        {
            AddPrimaryEventFields(mixPanelEvent);

            FormUrlEncodedContent requestParameters = BuildHttpRequestParameters(mixPanelEvent);

            var trackEventResponse = await client.PostAsync(trackEventAPIURI, requestParameters);
            await trackEventResponse.Content.ReadAsStringAsync();
        }

        private void AddPrimaryEventFields(MixPanelEvent mixPanelEvent)
        {
            if(masterUserId != null)
                mixPanelEvent.Properties.Add(new TrackingEventProperty("distinct_id", masterUserId));
            mixPanelEvent.Properties.Add(new TrackingEventProperty("token", projectToken));
            mixPanelEvent.Properties.Add(new TrackingEventProperty("time", DateTimeOffset.Now.ToUnixTimeSeconds().ToString()));
            mixPanelEvent.Properties.Add(new TrackingEventProperty("browser", GetBrowserName()));
            mixPanelEvent.Properties.Add(new TrackingEventProperty("ip_address", GetIPAddress()));

        }

        private static FormUrlEncodedContent BuildHttpRequestParameters(MixPanelEvent mixPanelEvent)
        {
            var parametersDictionary = new Dictionary<string, string> { { "data", mixPanelEvent.ToString() } };
            return new FormUrlEncodedContent(parametersDictionary);

        }

        private static string GetBrowserName()
        {
            if (HttpContext.Current == null)
                return null;

            var userAgent = HttpContext.Current.Request.UserAgent;
            var userBrowser = new HttpBrowserCapabilities { Capabilities = new Hashtable { { string.Empty, userAgent } } };
            var factory = new BrowserCapabilitiesFactory();
            factory.ConfigureBrowserCapabilities(new NameValueCollection(), userBrowser);
            return userBrowser.Browser;
        }
        private static string GetIPAddress() => HttpContext.Current?.Request?.UserHostAddress;
        

    }
}
