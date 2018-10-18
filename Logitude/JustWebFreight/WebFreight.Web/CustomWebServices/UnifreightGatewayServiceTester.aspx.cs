using Logitude.CustomsMessaging.RequestServices;
using Logitude.CustomsMessaging.UnifreightGateway;
using Logitude.Server.Tools.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebFreight.Web.CustomWebServices
{
    public partial class UnifreightGatewayServiceTester : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string MoreParams = "";
                string MessageOut = "";
                var ListEntry = new Dictionary<string, string>();
                ListEntry.Add("tenant", "1");
                ListEntry.Add("UNIFREIGHT_USER_ID", "ITZIK");




                var s = new SivugUpsertDcaReceivedService();

                string AssemblyQualifiedName = "Logitude.CustomsMessaging.UnifreightGateway.SivugUpsertDcaReceivedService";
                string DataIn1 = s.GetExampleDataIn1();
                string DataIn2 = "";
                MoreParams = UnifreightListsUtil.Serialize(ListEntry);
                string DataOut1 = "";
                string DataOut2 = "";
                string SUCCESS = "";

                var gw = new UnifreightGatewayService();
                gw.ProccessRequest(
                    AssemblyQualifiedName,
            DataIn1,
            DataIn2,
            out DataOut1,
            out DataOut2,
            out SUCCESS,
            ref MoreParams,
            out MessageOut);

                return;
                var myMN_MSG1_MANIFESTRequestService = new MN_MSG1_MANIFESTRequestService();
                var customReq = myMN_MSG1_MANIFESTRequestService.GetRequest(
                    new Logitude.CustomsMessaging.Common.RequestParams.MANIFESTRequestRequestParams() { Tenant = 1 });
                //myMN_MSG1_MANIFESTRequestService.
                return;
                var unifreightGatewayService = new UnifreightGatewayService();
                var txt = unifreightGatewayService.GetState();
                Response.Write("Success");
                Response.Write(txt);
            }
            catch (Exception E)
            {
                Response.ClearContent();
                Response.Write("ERROR");
                Response.Write(E.ToString());
                //throw;
            }
        }
    }
}