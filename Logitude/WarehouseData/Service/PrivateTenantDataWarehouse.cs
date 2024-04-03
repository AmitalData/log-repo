using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseData.Service
{
  public class PrivateTenantDataWarehouse
    {

        public PrivateTenantDataWarehouse()
        {

        }


        public DataTable GetPrivateTenant(string connectionString)
        {
            var dataTable = new DataTable();

            using (SqlConnection sourceConnection = new SqlConnection(connectionString))
            {
                sourceConnection.Open();
                SqlCommand commandSourceData = new SqlCommand(
               "SELECT  * from  DWHSettings where Catalog is not null", sourceConnection);

                SqlDataReader reader = commandSourceData.ExecuteReader();

                dataTable.Load(reader);

                reader.Close();
                sourceConnection.Close();

            }

            return dataTable;
        }

        public List<int> GetPrivateRelatedTenants(string connectionString, int tenant)
        {
            List<int> result = new List<int>();
            using (SqlConnection sourceConnection = new SqlConnection(connectionString))
            {
                sourceConnection.Open();
                SqlCommand commandSourceData = new SqlCommand(
               "SELECT  Tenant from  DWHSettings where ParentTenant = " + tenant, sourceConnection);
                SqlDataReader reader = commandSourceData.ExecuteReader();
                var dataTable = new DataTable();
                dataTable.Load(reader);

                result = dataTable.Rows
                                 .Cast<DataRow>()
                                 .Select(r => (int)r["Tenant"])
                                 .ToList();
                reader.Close();
                sourceConnection.Close();

            }

            return result;
        }

        public string ConvertIntgerListToString(List<int> relatedTenants)
        {
            string tenants = string.Empty;
            if (relatedTenants != null && relatedTenants.Count > 0)
            {
                tenants = relatedTenants.Select(i => i.ToString()).Aggregate((s1, s2) => s1 + ", " + s2);
                tenants = "(" + tenants + ")";
            }
            return tenants;
        }
    }
}
