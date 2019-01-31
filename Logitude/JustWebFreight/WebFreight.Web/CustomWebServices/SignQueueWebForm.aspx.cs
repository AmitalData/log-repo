using Logitude.Customs.BL.Messaging.Customs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebFreight.Web.CustomWebServices
{
    public partial class SignQueueWebForm : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
            try
            {
                bool forcePersonalSign = false;
                int tenant = int.Parse(Request.QueryString["tenant"].ToString());

                string SignByPersonalID = Request.QueryString["SignByPersonalID"];
                String CustomsRequestsSheetId = Request.QueryString["CustomsRequestsSheetId"];
                string InterfaceTypeCode = Request.QueryString["InterfaceTypeCode"];
                string SignStepName = Request.QueryString["SignStepName"];
                var SignQueueBy = SignQueue.GetSignQueueByTypeFromName(SignStepName);
                //please forcePersonalSign !!! in create URL !!!
                Logitude.Customs.BL.Messaging.Customs.SignQueue.Instance.Add(tenant, SignByPersonalID, CustomsRequestsSheetId, InterfaceTypeCode, SignQueueBy);
            }
                 
            finally
            {
                SignQueue.Instance.RefreshDb();
            }
            //CustomWebServices/SignQueueWebForm.aspx?tenant=208&PreferSignature=33&CustomsRequestsSheetId=222&InterfaceTypeCode=222;
            //created from Logitude.Customs.BL.Messaging.Customs.SignQueue.Instance.GetSignQueueWebFormUrl()

        }
    }
}