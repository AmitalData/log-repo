using Logitude.BL.GlobalModel.EntityQueries;
using Simplog.Global.Data.GlobalModel;
using Simplog.Global.Data.GlobalModel.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace WebFreight.Web
{
    public partial class LinksGateway : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            bool isFromLocal = Request.Url.Host.ToLower().Contains("localhost");
            //http://localhost:9996/LinksGateway.aspx?Menu=PREQ&SecurityKey=d5e6d15f4cb24f12a8ac9c5e8c54a06d
            var Menu = Request.QueryString["Menu"];
            if (Menu == null) { return; }
            if (Menu == "PREQ" || Menu == "UID")
            {
                var Tenant = Request.QueryString["Tenant"];
                var SecurityKey = Request.QueryString["SecurityKey"];
                var htmlVersion = GetHTMLVersion();
                string RedirectUrl = "";
                if (isFromLocal)
                {
                    RedirectUrl = "http://localhost:4200/index.html?Menu=" + Menu + "&SecurityKey=" + SecurityKey ;
                }
                else
                {
                    RedirectUrl = "~/Angular" + htmlVersion + "/index.html?Menu=" + Menu + "&SecurityKey=" + SecurityKey;
                }
                if(!string.IsNullOrWhiteSpace(Tenant)){
                    RedirectUrl += "&Tenant=" + Tenant;
                } 
                Response.Redirect(RedirectUrl);
            }
            else if (Menu.ToUpper().StartsWith("URL_"))
            {
                var htmlVersion = GetHTMLVersion();
                Menu = Menu.Substring("URL_".Length) + "&";
                StringBuilder queryStringBuilder = getQueryStringBuilder("Menu");
                string RedirectUrl = "Angular" + htmlVersion + "/index.html?Menu=" + Menu + queryStringBuilder.ToString();
                Response.Redirect("~/" + RedirectUrl);

            }


        }

        private StringBuilder getQueryStringBuilder(string excludeKey)
        {
            StringBuilder queryStringBuilder = new StringBuilder();
            foreach (string key in Request.QueryString.AllKeys)
            {
                if (!string.IsNullOrEmpty(key) && key.ToLower() != excludeKey.ToLower())
                {
                    if (queryStringBuilder.Length > 0)
                    {
                        queryStringBuilder.Append("&");
                    }
                    queryStringBuilder.Append(HttpUtility.UrlEncode(key));
                    queryStringBuilder.Append("=");
                    queryStringBuilder.Append(HttpUtility.UrlEncode(Request.QueryString[key]));
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