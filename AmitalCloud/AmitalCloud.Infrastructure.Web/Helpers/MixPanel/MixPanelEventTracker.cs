using AmitalCloud.Infrastructure.Application.EntityQueryServices;
using AmitalCloud.Infrastructure.Data.Helpers;
using AmitalCloud.Infrastructure.Domain.EntityPMs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net.Http;
using System.Web;
using System.Web.Configuration;

namespace AmitalCloud.Infrastructure.Web.Helpers.MixPanel
{
    public class MixPanelEventTracker
    {
        private static readonly HttpClient client = new HttpClient();
        private const string trackEventAPIURI = "https://api.mixpanel.com/track#live-event";
        string userEmail;
        string projectToken;
        private int tenant;
        private TenantManagementPM tenantManagement;

        public MixPanelEventTracker(string projectToken, string email, int tenant)
        {
            this.projectToken = projectToken;
            this.userEmail = email;
            this.tenant = tenant;
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
            if (userEmail != null)
                mixPanelEvent.Properties.Add(new TrackingEventProperty("distinct_id", userEmail));
            mixPanelEvent.Properties.Add(new TrackingEventProperty("token", projectToken));
            mixPanelEvent.Properties.Add(new TrackingEventProperty("time", DateTimeOffset.Now.ToUnixTimeSeconds().ToString()));
            mixPanelEvent.Properties.Add(new TrackingEventProperty("browser", GetBrowserName()));
            mixPanelEvent.Properties.Add(new TrackingEventProperty("ip_address", GetIPAddress()));
            AddTenantProperties(mixPanelEvent);
        }

        private void AddTenantProperties(MixPanelEvent mixPanelEvent)
        {
            GetTenantManagement(tenant);
            mixPanelEvent.Properties.Add(new TrackingEventProperty("tenant-number", tenant.ToString()));
            mixPanelEvent.Properties.Add(new TrackingEventProperty("tenant-name", tenantManagement.Name));
            mixPanelEvent.Properties.Add(new TrackingEventProperty("tenant-url", tenantManagement.CustomerURL));
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
        private static string GetIPAddress() => AuthenticationUtil.GetIP4Address();

        private void GetTenantManagement(int tenant)
        {
            TenantManagementQueryService tenantManagementQuery = new TenantManagementQueryService(tenant);
            tenantManagement = tenantManagementQuery.GetSingle(tenant, false, false);
        }
    }
}
