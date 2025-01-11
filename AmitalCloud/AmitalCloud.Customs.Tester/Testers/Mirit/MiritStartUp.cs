using Logitude.CustomsMessaging.Helpers;
using Logitude.CustomsMessaging.Testers.Mirit.VendorRepository;
using System;

namespace Logitude.CustomsMessaging.Testers.Mirit
{
    public class MiritStartUp
    {
        public static void Doit()
        {
            try
            {


                //////miri !!!
                var systemTables = new SystemTables();
                var ListofSYSTBL_NG_9001_MSG_SystemTablesResponseTableData = systemTables.GetTableData("2011", 208);
                return;
                UpdateVendor.GetTSTFromCustomAndUpdateIt();
            }
            catch (Exception e)
            {

                NetCommonHelper.Logger.DevLog.Instance.WriteFatal(e);
            }
        }
    }
}
