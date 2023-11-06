using Logitude.BL.GlobalModel.EntityQueries;
using Simplog.Global.Data.GlobalModel;
using Simplog.Global.Data.GlobalModel.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

            //http://localhost:9996/LinksGateway.aspx?Menu=PREQ&SecurityKey=cfeb18b0f2044ccab43bb1d2cc67048e&Tenant=203
            var Menu = Request.QueryString["Menu"];
            if (Menu == "PREQ" || Menu == "UID")
            {
                var Tenant = Request.QueryString["Tenant"];
                var SecurityKey = Request.QueryString["SecurityKey"];
                var htmlVersion = GetHTMLVersion();

                string RedirectUrl = "Angular" + htmlVersion + "/index.html?Menu=" + Menu + "&SecurityKey=" + SecurityKey + "&Tenant=" + Tenant;
                Response.Redirect("~/" + RedirectUrl);
            }

     
        }


        private string GetHTMLVersion()
        {
            string htmlVersion = GetHTMLVersionFromFile();
            if(string.IsNullOrEmpty(htmlVersion)) htmlVersion = GetHtmlVersionFromSetting();
            return htmlVersion;
        }

        
        private  string GetHtmlVersionFromSetting()
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