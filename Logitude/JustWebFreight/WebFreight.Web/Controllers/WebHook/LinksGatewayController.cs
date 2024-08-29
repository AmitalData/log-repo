using Logitude.BL.GlobalModel.EntityQueries;
using Simplog.Global.Data.GlobalModel;
using Simplog.Global.Data.GlobalModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Xml;

namespace WebFreight.Web.Controllers.WebHook
{

        public class LinksGatewayController : ApiController
        {
            [HttpGet]
            public IHttpActionResult GetLink(string Menu, string SecurityKey="", string Tenant= "")
            {

            if (string.IsNullOrEmpty(Menu))
            {
                return Ok("Menu parameter is required");
            }

            var htmlVersion = GetHTMLVersion();
            string RedirectUrl = string.Empty;
            if (Menu == "PREQ" || Menu == "UID")
            {
                RedirectUrl = "Angular" + htmlVersion + "/index.html?Menu=" + Menu + "&SecurityKey=" + SecurityKey + "&Tenant=" + Tenant;
            
            }
            else if (Menu.ToUpper().StartsWith("URL_"))
            {
                Menu = Menu.Substring("URL_".Length) + "&";
                StringBuilder queryStringBuilder = GetQueryStringBuilder("Menu");
                RedirectUrl = "Angular" + htmlVersion + "/index.html?Menu=" + Menu + queryStringBuilder.ToString();
               
            }
            var rootUrl = Url.Content("~/");
            return Redirect(rootUrl + RedirectUrl);

            }

            private StringBuilder GetQueryStringBuilder(string excludeKey)
            {
                var queryStringBuilder = new StringBuilder();
                var queryParams = Request.GetQueryNameValuePairs();
                foreach (var keyValuePair in queryParams)
                {
                    if (!string.IsNullOrEmpty(keyValuePair.Key) && keyValuePair.Key.ToLower() != excludeKey.ToLower())
                    {
                        if (queryStringBuilder.Length > 0)
                        {
                            queryStringBuilder.Append("&");
                        }
                        queryStringBuilder.Append(HttpUtility.UrlEncode(keyValuePair.Key));
                        queryStringBuilder.Append("=");
                        queryStringBuilder.Append(HttpUtility.UrlEncode(keyValuePair.Value));
                    }
                }
                return queryStringBuilder;
            }

            private string GetHTMLVersion()
            {
                string htmlVersion = GetHTMLVersionFromFile();
                if (string.IsNullOrEmpty(htmlVersion)) htmlVersion = GetHtmlVersionFromSetting();
                return htmlVersion;
            }

            private string GetHtmlVersionFromSetting()
        {
            IGlobalContext objectContext = GlobalContext.GetContext();
            SettingRepository MySettingRepository = new SettingRepository(objectContext);
            SettingQuery MySettingQuery = new SettingQuery(MySettingRepository);
            var MySettings = MySettingQuery.GetSinglePM();
            return MySettings?.HtmlVersion;
        }

            private string GetHTMLVersionFromFile()
        {
            try
            {
                string htmlVersionFilePath = System.IO.Path.GetDirectoryName(new System.Uri(System.Reflection.Assembly.GetExecutingAssembly().CodeBase).LocalPath).Replace("bin", "") + "Version\\htmlVersion.xml";
                XmlDocument htmlVersionXmlDocument = new XmlDocument();
                htmlVersionXmlDocument.Load(htmlVersionFilePath);
                XmlNodeList htmlVersionNode = htmlVersionXmlDocument.GetElementsByTagName("HTMLversion");
                if (htmlVersionNode == null || htmlVersionNode.Count == 0) return null;
                return htmlVersionNode[0].InnerText;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

    }
}


