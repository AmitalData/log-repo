using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseDataViews.Service
{
    public class FeaturePrivateDataWarehouseService
    {

        private string globalDBConnetionstring = string.Empty;
        private string mainDBConnetionstring = string.Empty;
        private int tenant;
        private List<string> tenantPackagesCodeLists = null;
        public FeaturePrivateDataWarehouseService(string globalDBConnetionstring, string mainDBConnetionstring , int tenant)
        {
            this.globalDBConnetionstring = globalDBConnetionstring;
            this.mainDBConnetionstring = mainDBConnetionstring;
            this.tenant = tenant;
            tenantPackagesCodeLists =GetTenantPackagesCodeLists();

        }

        public bool CheckFeature(string featureCode)
        {
            bool isHaveFeature = true;
            if (!IsTenantPackageDevelopment())
            {
                foreach (string packageCode in tenantPackagesCodeLists)
                {
                    isHaveFeature = IsHavePackgeFeature(featureCode, packageCode);
                    if (isHaveFeature) return isHaveFeature;
                }
            }
            return isHaveFeature;

        }

        private List<string> GetTenantPackagesCodeLists()
        {
            var packageLists = new List<string>();
            var dataTable = ExecuteSQL(globalDBConnetionstring, "select PackageCode,IsMultiPackage  from " + " dbo." + "TenantManagements where Id = " + tenant);
            DataRow dataRow = dataTable.AsEnumerable().FirstOrDefault();
            if (dataRow != null)
            {
                string packageCode = dataRow["PackageCode"] != null ? dataRow["PackageCode"].ToString() : "";
                bool isMultiPackage = dataRow["IsMultiPackage"] != null ? bool.Parse(dataRow["IsMultiPackage"].ToString()) : false;
                if (isMultiPackage) packageLists = GetTenantMultiPackagesLists();
                if (!packageLists.Contains(packageCode)) packageLists.Add(packageCode);
            }
            return packageLists;

        }

        private List<string> GetTenantMultiPackagesLists()
        {
            List<string> result = new List<string>();
            var tenantManagementLicenseDataTable = ExecuteSQL(globalDBConnetionstring, "select PackageCode  from " + " dbo." + "TenantManagementLicenses where Tenant = " + tenant);
            foreach (DataRow row in tenantManagementLicenseDataTable.AsEnumerable())
            {
                string factCode = row["PackageCode"] != null ? row["PackageCode"].ToString() : "";
                result.Add(factCode);
            }

            return result;
        }

        private bool IsTenantPackageDevelopment()
        {
            return tenantPackagesCodeLists.Contains("DVMT") ? true : false;
        }

        private bool IsHavePackgeFeature(string featureCode, string packageCode)
        {
            List<string> connectedPackages = GetContectPackages(packageCode);
            string connectedPackagesAsString = GetConnectedPackageAsString(connectedPackages);
            var existFeatureDataTable = ExecuteSQL(mainDBConnetionstring, "select top(1) FeatureUniqeCode from PackageFeatures where PackageCode in " + connectedPackagesAsString + " and FeatureUniqeCode = (select FeatureUniqeCode from Features where Code = " + "'" + featureCode + "')");
            return existFeatureDataTable.AsEnumerable().FirstOrDefault() !=null  ? true : false;
        }

        private static string GetConnectedPackageAsString(List<string> connectedPackages)
        {
            string connectedPackagesAsString = "(";
            foreach (string connectedPackage in connectedPackages)
            {
                connectedPackagesAsString += ("'" + connectedPackage + "'" + ",");
            }
            connectedPackagesAsString = connectedPackagesAsString.Remove(connectedPackagesAsString.Length - 1);
            connectedPackagesAsString += ")";
            return connectedPackagesAsString;
        }

        
        private List<string> GetContectPackages(string packageCode)
        {
            var contectPackages = new List<string>();
            var packageConnectedPackagesDataTable = ExecuteSQL(mainDBConnetionstring, "select ConnectedPackageCode from PackageConnectedPackages where PackageCode = " + "'" + packageCode + "'"   );
            foreach (DataRow row in packageConnectedPackagesDataTable.AsEnumerable())
            {
                string connectedPackageCode = row["ConnectedPackageCode"] != null ? row["ConnectedPackageCode"].ToString() : "";
                contectPackages.Add(connectedPackageCode);
            }
            return contectPackages;
        }

        private DataTable ExecuteSQL(string connectionString, string sql)
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

            return dataTable;
        }

    }
    public class TenantManagementDetails
    {
        public string PackageCode { get; set; }
        public bool IsMultiPackage { get; set; }
        public List<string> PackageCodeLists { get; set; }

    }

}
