using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseDataViews.Service
{
  public  class FeaturePrivateDataWarehouseService
    {

        private string globalDBConnetionstring = string.Empty;
        private string mainDBConnetionstring = string.Empty;

        public FeaturePrivateDataWarehouseService(string globalDBConnetionstring, string mainDBConnetionstring)
        {
            this.globalDBConnetionstring = globalDBConnetionstring;
            this.mainDBConnetionstring = mainDBConnetionstring;

        }

        public bool CheckFeature(string featureCode, int tenant)
        {
            bool result = false;
            var mainPackageCode = RunSqlString(globalDBConnetionstring, "select PackageCode  from " + " dbo." + "TenantManagements where Id = " + tenant);
            if (mainPackageCode != "DVMT")
            {
                result = IsHavePackgeFeature(featureCode, mainPackageCode) ? true : false;
            }
            else result = true;
            return result;
        }

        private bool IsHavePackgeFeature(string featureCode, string packageCode)
        {
            var feature = RunSqlString(mainDBConnetionstring, ("select PackageConnectedPackages.PackageCode as PackageCode  from PackageFeatures inner join PackageConnectedPackages on PackageFeatures.PackageCode =PackageConnectedPackages.ConnectedPackageCode where FeatureUniqeCode = (select FeatureUniqeCode from Features where Code = '" + featureCode + "') and PackageConnectedPackages.PackageCode = '" + packageCode + "'"));
            return !string.IsNullOrEmpty(feature) ? true : false;
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
            }

            return dataTable.Rows
                 .Cast<DataRow>()
                 .Select(r => (string)r["PackageCode"].ToString())
                 .FirstOrDefault();
        }

    }
}
