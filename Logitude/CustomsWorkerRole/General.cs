using System;
using System.Configuration;
using System.Linq;
using Logitude.SystemLogs;
using Simplog.Global.Data.GlobalModel;





namespace CustomsWorkerRole
{
    static class General
    {
        public static bool IsUpdating()
        {
            try
            {



                IGlobalContext globalcontext = GlobalContext.GetContext();
                bool isUpgrading = (from a in globalcontext.GlobalDBs
                                    select a).FirstOrDefault().IsUpgrading;

                return isUpgrading;
            }
            catch (Exception e)
            {
                ExceptionHandler.HandleException(e,DateTime.Now, 0, "", "WorkerRole", "General : IsUpdating() Method in customs worker role",null);
                return false;
            }
        }
        public static int GetTenantDB()
        {
			string tenantValue = ConfigurationManager.AppSettings["TenantDB"];
			int tenant = string.IsNullOrEmpty(tenantValue) ? 0 : Convert.ToInt32(tenantValue);
            return tenant;
		}
    }
}
