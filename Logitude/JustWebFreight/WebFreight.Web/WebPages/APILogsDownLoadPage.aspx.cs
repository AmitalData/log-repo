using Logitude.BL.InfrastructureModel.EntityQueries;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Global.Data.GlobalModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebFreight.Web.Helpers;

namespace WebFreight.Web.WebPages
{
    public partial class APILogsDownLoadPage : System.Web.UI.Page
    {
        public bool CheckAvailablityTenantsForEmail(string email, int tenant)
        {
            UserRepository userRep = new UserRepository(0);
            Simplog.Data.CommonDataModel.EntityPOCOs.User user = userRep.GetSingleUserByEmail(email, 0, false);

            bool available = true;
            if (user != null)
            {
                TenantManagementRepository tenantManagementRep = new TenantManagementRepository();
                bool isDistributorToCurrentTenant = tenantManagementRep.CheckDistributor(user.DistributorCode, tenant);
                if (user.IsDistributor)
                {
                    if (isDistributorToCurrentTenant)
                    {
                        available = true;
                    }
                    else
                    {
                        available = false;
                    }
                }
                else
                {
                    available = true;
                }
            }
            else
            {
                ContactRepository contactRep = new ContactRepository(tenant);
                available = contactRep.CheckEmailAvailabilityForTenant(email, tenant);
            }
            return available;
        }


        protected void Page_Load(object sender, EventArgs e)
        {


            int? tenant = null;
            string token = Request["tempId"] ?? "";


            SecurityDocumentResult securityDocumentResult = SecurityDocumentHelper.ValidationDocumentToken(token);
            bool isValid = securityDocumentResult.IsValid;
            string email = securityDocumentResult.Email;
            string exceptionMessage = securityDocumentResult.ExceptionResult;
            tenant = securityDocumentResult.Tenant;


            if (isValid)
            {
                if (!CheckAvailablityTenantsForEmail(email, (int)tenant)) isValid = false;
            }

            if (isValid)
            {
                string HeaderData = Request["header"] ?? "";
                var Data = HeaderData.Split('_');
                var APILogQuery = new APILogsDataQuery((int)tenant);
                var APILog = APILogQuery.GetSinglePM(Data[0], (int)tenant);
                string message = "";
                if (Data[1] == "Req")
                {
                    message = APILog.RequestData;
                }
                else
                {
                    message = APILog.ResponseData;
                }

                byte[] bytes = new byte[message.Length * sizeof(char)];
                //System.Buffer.BlockCopy(message.ToCharArray(), 0, bytes, 0, bytes.Length);
                bytes = Encoding.UTF8.GetBytes(message);
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.AddHeader("Content-Length", bytes.Length.ToString());
                HttpContext.Current.Response.AddHeader("Content-Disposition", "inline;filename=Body.xml");
                HttpContext.Current.Response.ContentType = "application/xml";
                HttpContext.Current.Response.BinaryWrite(bytes);
                if (HttpContext.Current.Response.IsClientConnected)
                {
                    HttpContext.Current.Response.Flush();
                    HttpContext.Current.Response.Close();
                    HttpContext.Current.ApplicationInstance.CompleteRequest();

                }

            }
            else
            {
                var message = exceptionMessage;
                if (string.IsNullOrEmpty(exceptionMessage)) message = "Sorry you’re not authenticated to view this excel.";
                Response.Output.Write(message);
 
            }



        }
    }
}