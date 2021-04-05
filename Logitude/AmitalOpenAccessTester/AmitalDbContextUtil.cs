using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unifreight.Data.AmitalModel;

namespace AmitalOpenAccessTester
{
    public class AmitalDbContextUtil
    {


        public static AmitalContext GetContext()
        {
            if (ConfigurationManager.ConnectionStrings["DevartAmitalDirect"] == null)
            {
                throw new Exception("DevartAmitalDirect.ConnectionString is missing 'server,port,sid,userId,password'");
            }
            LogitudeSettings.DatabaseManagementSystem = "oracle";
            var dbConnectionInfo = ConfigurationManager.ConnectionStrings["DevartAmitalDirect"].ConnectionString; ;
            var config = Devart.Data.Oracle.Entity.Configuration.OracleEntityProviderConfig.Instance;
            //config.DatabaseScript.Schema.DeleteDatabaseBehaviour =  Devart.Data.Oracle.Entity.Configuration.DeleteDatabaseBehaviour.Schema;
            //config.Workarounds.IgnoreSchemaName = true;
            //dbConnectionInfo = "10.10.10.67,1521,amital,amitestm,amitestm";
            LogitudeSettings.GetUnfDBConnectionInfoFromTenantInject = GetUnfDBConnectionInfo;
            return AmitalContext.GetContextByDBInfo(dbConnectionInfo, 1);
            //return AmitalContext.GetContext(1);
        }
        public static string GetUnfDBConnectionInfo(int tenant)
        {
            //CustomsSettingPM customsSettingPM = CustomsSettingQueryService.GetSettingByTenant(tenant);

            //var customsSettingQueryService = new CustomsSettingQueryService(tenant);
            //customsSettingPM = customsSettingQueryService.GetSettingByTenantN(tenant);

            var config = Devart.Data.Oracle.Entity.Configuration.OracleEntityProviderConfig.Instance;
            config.Workarounds.DisableQuoting = false;///ORA-01747 ? ??????
                                                      ///
            var dbConnectionInfo = ConfigurationManager.ConnectionStrings["DevartAmitalDirect"].ConnectionString; ;
            return dbConnectionInfo;
        }
    }
}
