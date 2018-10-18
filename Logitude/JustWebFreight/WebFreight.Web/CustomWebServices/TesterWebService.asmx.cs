using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Logitude.SystemLogs;

namespace WebFreight.Web.CustomWebServices
{
    /// <summary>
    /// Summary description for TesterWebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class TesterWebService : System.Web.Services.WebService
    {

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";

        }
        [WebMethod]
        public void Tester(byte[] data)
        {
            try
            {
                string messagedata = System.Text.Encoding.UTF8.GetString(data);
                switch (messagedata.ToUpper() )
                {
                    case "MIRIT":
                        Logitude.CustomsMessaging.Testers.Mirit.MiritStartUp.Doit();
                        break;
                    default:
                        break;
                }
               
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "web role", null,null);
            }
        }
    }
}
