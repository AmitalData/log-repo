using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebFreight.Web.WebServices;

namespace WebFreight.Web
{
    public partial class AmitalDefault : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            StartProgram();
            


           
        }

        public void StartProgram()
        {
            string token = Request.QueryString["token"];
            string tenant = Request.QueryString["tenant"];
            if (!string.IsNullOrEmpty(token) && !string.IsNullOrEmpty(tenant))
            {//AmitalBrowserInUse
                XapFilesStorageService xapservice = new XapFilesStorageService();

                CultureInfo en = new CultureInfo("en-US");

                string xapPhysicalPath = Server.MapPath(@"ClientBin/Simplog.Infrastructure.xap");
                DateTime lastWrite = System.IO.File.GetLastWriteTime(xapPhysicalPath);

                string xapsource = xapSource.Attributes["value"] + "?" + lastWrite.ToString(@"dd MMM yyyy HH':'mm':'ss", en.DateTimeFormat);
                xapSource.Attributes["value"] = xapsource;




                //string userName = Request.QueryString["fb_sig_user"];



                string xapfilesdata = xapservice.GetXapFilesDetails();

                p.InnerHtml = "<param name=\"initParams\" value=\"token=" + token + ",tenant=" + tenant + ",AmitalBrowserInUse=" + true + ",XapFilesDetails=" + xapfilesdata + ",LogoCode=" + LogitudeSettings.LogoCode + "\"  />";
                                                                                                                                                                                    

            }



        }

    }
}
