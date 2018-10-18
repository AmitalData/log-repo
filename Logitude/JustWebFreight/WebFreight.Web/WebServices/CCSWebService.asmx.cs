using Logitude.XSD;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Services;

namespace WebFreight.Web.WebServices
{
    /// <summary>
    /// Summary description for CCSWebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class CCSWebService : System.Web.Services.WebService
    {
        [WebMethod]
        public CCSResult Send(string myShipmentId, int myTenant, string myRecipient, bool isCargonautSending, bool isDEXXSending)
        {
            CCSHelper myCCSHelper = new CCSHelper(myShipmentId, myTenant, myRecipient, isCargonautSending, isDEXXSending);

            myCCSHelper.Run();

            return myCCSHelper.Result;
        }
    }
}
