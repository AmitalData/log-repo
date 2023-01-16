using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseData.Service
{
  public  class FeatureDataWarehouseService
    {

        private string globalDBConnetionstring = string.Empty;
        private string mainDBConnetionstring = string.Empty;
        string ddOnsPrivateDBCode = "PRIDB";


        public FeatureDataWarehouseService(string globalDBConnetionstring , string mainDBConnetionstring)
        {
            this.globalDBConnetionstring = globalDBConnetionstring;
            this.mainDBConnetionstring = mainDBConnetionstring;

        }


        public  bool CheckFeature(string featureCode, int tenant)
        {
            bool result = false;
            var mainPackageCode = RunSqlString(globalDBConnetionstring, "select PackageCode  from " + " dbo." + "TenantManagements where Id = " + tenant);
            if (mainPackageCode != "DVMT")
            {
                if (IsTenantHaveAddOnsPrivateDB(tenant))
                {
                    if (IsHavePrivateDBFeature(featureCode)) result = true;
                }
            }
            else result = true;


            return result;
        }


        private bool IsTenantHaveAddOnsPrivateDB(int tenant)
        {
            var tenantAddOnsPrivateDB = RunSqlString(globalDBConnetionstring, ("select PackageCode from TenantAddOns where Tenant = " + tenant + " and PackageCode = '" + ddOnsPrivateDBCode + "'"));
            return !string.IsNullOrEmpty(tenantAddOnsPrivateDB) ? true : false;
        }

        private bool IsHavePrivateDBFeature(string featureCode)
        {
            var privateDBFeature = RunSqlString(mainDBConnetionstring, ("select PackageConnectedPackages.PackageCode as PackageCode  from PackageFeatures inner join PackageConnectedPackages on PackageFeatures.PackageCode =PackageConnectedPackages.ConnectedPackageCode where FeatureId = (select Id from Features where Code = '" + featureCode + "') and PackageConnectedPackages.PackageCode = '"+ ddOnsPrivateDBCode + "'"));
            return !string.IsNullOrEmpty(privateDBFeature) ? true : false;
        }


        private string RunSqlString(string connectionString, string sql)
        {
            var dataTable = new DataTable();
            using (SqlConnection sourceConnection = new SqlConnection(connectionString))
            {
                sourceConnection.Open();
                SqlCommand commandSourceData = new SqlCommand(sql, sourceConnection);
                SqlDataReader reader = commandSourceData.ExecuteReader();
                dataTable.Load(reader);
                reader.Close();
                sourceConnection.Close();

            }

            return dataTable.Rows
                 .Cast<DataRow>()
                 .Select(r => (string)r["PackageCode"].ToString())
                 .FirstOrDefault();
        }

    }
}
