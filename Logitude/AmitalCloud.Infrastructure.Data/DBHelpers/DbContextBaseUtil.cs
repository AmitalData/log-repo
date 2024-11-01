using AmitalCloud.Infrastructure.Domain.Enums;
using Devart.Data.Oracle;
using System;
using System.Linq;

namespace AmitalCloud.Infrastructure.Data.DBHelpers
{
    public static class DbContextBaseUtil
    {
        readonly static bool _UnifreightDataIncludedInMain_FeatureOn;
        public static bool UnifreightDataIncludedInMain_FeatureOn { get { return _UnifreightDataIncludedInMain_FeatureOn; } }
        static DbContextBaseUtil()
        {
            _UnifreightDataIncludedInMain_FeatureOn = true;
            return;
        }
        public static string GetConnectionStringWithAmitalNetRole(string currentConnectionString)
        {
            return currentConnectionString;
        }
        public static bool? ToLog { get; set; }
        public static DateTime? MaxPoolSizeWasReachedWhileSave { get; set; }
        public static string GetStoredProcedureName(string StoredProcedure, AmitalCloudDBSchema amitalCloudDBSchema, string OracleConnectionStr)
        {
            if (DBHelpers.DbContextBaseUtil.UnifreightDataIncludedInMain_FeatureOn)
            {
                var oraCSB = new OracleConnectionStringBuilder(OracleConnectionStr);
                return oraCSB.UserId.ToString() + "." + StoredProcedure;
            }
            return StoredProcedure;
        }
        public static OracleConnectionStringBuilder GetOracleConStrBuilder(string dbConnectionInfo)
        {
            if (String.IsNullOrWhiteSpace(dbConnectionInfo))
            {
                throw new Exception("DevartAmitalDirect.ConnectionString is missing 'server,port,sid,userId,password'");
            }
            string[] information = dbConnectionInfo.Split(',');
            if (information.Count() != 5)
            {
                throw new Exception("DevartAmitalDirect.ConnectionString must be 'server,port,sid,userId,password'");
            }
            string server = information[0];
            string port = information[1];
            string sid = information[2];
            string userId = information[3];
            string password = information[4];
            int iPort = 1521;
            if (!int.TryParse(port, out iPort))
            {
                throw new Exception("DevartAmitalDirect.ConnectionString must be 'server,port,sid,userId,password'");
            }
            OracleConnectionStringBuilder oraCSB = new OracleConnectionStringBuilder();
            oraCSB.Direct = true;
            oraCSB.Server = server; 
            oraCSB.Port = iPort;
            oraCSB.Sid = sid;
            oraCSB.UserId = userId;
            oraCSB.Password = password;
            return oraCSB;
        }

        public static string GetSchemaAMITAL_DB(int tenantSeed = 1)
        {
            Devart.Data.Oracle.OracleConnectionStringBuilder csb = null;
            string dbConnectionInfo = "";
            if (AmitalCloudSettings.GetLogitudeCustomsSettingsMInject != null)
            {
                dbConnectionInfo = AmitalCloudSettings.GetLogitudeCustomsSettingsMInject(tenantSeed).UnfConnectionString;
            }
            else
            {
                dbConnectionInfo = AmitalCloudSettings.GetUnfDBConnectionInfoFromTenantInject(tenantSeed);
            }
            csb = GetOracleConStrBuilder(dbConnectionInfo);
            return csb.UserId.ToUpper();
        }
    }

}
