using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Simplog.Global.Data.GlobalModel.Repositories;

using WebFreight.Web.GlobalModelDB;
using System.Transactions;
using WebFreight.Web.Testing;
using WebFreight.Web.CommonDataModel;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using WebFreight.Web.GlobalModel;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Logitude.SystemLogs;

namespace WebFreight.Web.Monitoring
{
    public partial class DbUp : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Clear();
            Response.ContentType = "text/xml";
            //Response.Write("<?xml version='1.0' encoding='UTF-8'?>");
            Response.Write("<pingdom_http_custom_check>");

            if (IsSqlDbUp())
            {
                Response.Write("<status>OK</status>");
            }
            else
            {
                Response.Write("<status>Fail</status>");
            }
            int ResponseTime_Millisecond = HttpContext.Current.Timestamp.Millisecond;
            String ResponseTime = "<response_time>" + ResponseTime_Millisecond + "</response_time>";
            Response.Write(ResponseTime);
            Response.Write("</pingdom_http_custom_check>");
            Response.End();
        }

        private bool IsSqlDbUp()
        {
            List<GlobalDB> GlobalDatabases;
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())//TransactionFactory.GetNewTransaction())
            {
                GlobalDBRepository globaldbRep = new GlobalDBRepository();

                try
                {
                    GlobalDatabases = globaldbRep.All();
                    scope.Complete();
                }
                catch (Exception errorInfo)
                {
                    ExceptionHandler.HandleException(errorInfo, DateTime.Now, 0, "", "StorageUp", "Bug in IsSqlDbUp Method : globaldbRep.All()",null);
                    scope.Complete();
                    return false;
                }

            }

            foreach (GlobalDB db in GlobalDatabases)
            {

                CommonDataContext Context = CommonDataContext.GetContextByDBId(db.Id);

                try
                {
                    Branch branch = (from a in Context.Branches
                                     select a).FirstOrDefault();
                }

                catch (Exception errorInfo)
                {
                    ExceptionHandler.HandleException(errorInfo, DateTime.Now, 0, "", "StorageUp", "Bug in IsSqlDbUp Method : Branch branch = (from a in Context.Branches select a).FirstOrDefault();",null);
                    return false;
                }

            }

            return true;
        }
    }
}