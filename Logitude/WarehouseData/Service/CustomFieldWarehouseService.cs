using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseData.Helper
{
   public class CustomFieldWarehouseService
    {
        private  int customFieldsCount = 0;
        GeneralDataWarehouseService generalDataWarehouseService;
        List<TableClass> tableLists;
        public CustomFieldWarehouseService()
        {
            this.generalDataWarehouseService = new GeneralDataWarehouseService() ;
        }


        public string BuildCustomFields(string sqlString , int customFieldsCount)
        {
            string result = sqlString;
            this.customFieldsCount = customFieldsCount;
            if (!string.IsNullOrEmpty(result))
            {
                result = ResolveDeclareCustomFieldsVariable(result);
                result = ResolveCustomFieldNamesVariable(result);
                result = ResolveCustomFieldValuesVariable(result);
                result = ResolveShipmentsCustomFieldsVariable(result);
                result = ResolveCursorCustomFieldsVariable(result);

            }
            return result;
        }

        private string ResolveCursorCustomFieldsVariable(string sql)
        {
            int i = 1;
            string result = string.Empty;
            if (sql.Contains("@CursorCustomFieldsVariable"))
            {
                i = 1;
                result = string.Empty;
                while (i <= customFieldsCount)
                {
                    result += "@Field" + i + ",";
                    i += 1;
                }
                result += "@CustomFields,";
                result += "^";
                result = result.Replace(",^", "");
                sql = sql.Replace("@CursorCustomFieldsVariable", result);
            }
            return sql;
        }

        private string ResolveShipmentsCustomFieldsVariable(string sql)
        {
            int i = 1;
            string result = string.Empty;
            if (sql.Contains("@dw_Shipments.CustomFieldsVariable"))
            {
                i = 1;
                result = string.Empty;
                while (i <= customFieldsCount)
                {
                    result += "dw_Shipments.Field" + i + ",";
                    i += 1;
                }
                result += "dw_CustomObjectFields.CustomFields,";

                result += "^";
                result = result.Replace(",^", "");
                sql = sql.Replace("@dw_Shipments.CustomFieldsVariable", result);
            }
            return sql;
        }

        private string ResolveCustomFieldValuesVariable(string sql)
        {
            int i = 1;
            string result = string.Empty;

            if (sql.Contains("[CustomFieldValuesVariable]"))
            {
                i = 1;
                result = string.Empty;
                while (i <= customFieldsCount)
                {
                    result += "dbo.ResolveCustomFieldValue(@Field" + i + ",'Field" + i + "' ,@CustomFields)" + (i < customFieldsCount ? "," : "");

                    i += 1;
                }

                sql = sql.Replace("[CustomFieldValuesVariable]", result);
            }
            return sql;
        }

        private string ResolveCustomFieldNamesVariable(string sql)
        {
            int i = 1;
            string result = string.Empty;
            if (sql.Contains("[CustomFieldNamesVariable]"))
            {
                i = 1;
                result = string.Empty;
                while (i <= customFieldsCount)
                {
                    result += "[Field" + i + "]" + (i < customFieldsCount ? "," : "");
                    i += 1;
                }

                sql = sql.Replace("[CustomFieldNamesVariable]", result);
            }

            return sql;
        }

        private string ResolveDeclareCustomFieldsVariable(string sql)
        {
            int i = 1;
            string result = string.Empty;

            if (sql.Contains("--@[DeclareCustomFieldsVariable]"))
            {
                while (i <= customFieldsCount)
                {
                    result += "   declare @Field" + i + " as varchar(2000) \r\n";

                    i += 1;
                }
                result += "   declare @CustomFields as varchar(4000) \r\n";

                //DataTypeCode
                sql = sql.Replace("--@[DeclareCustomFieldsVariable]", result);
            }

            return sql;
        }



        public void BuildCustomObjectFieldsTable(string desconnectionString, List<TableClass> tableLists, bool isIncrementDataWarehouse = false)
        {
            this.tableLists = tableLists;
            string tenantUpdated = isIncrementDataWarehouse ? GetCustomFieldTenantsUpdated(desconnectionString) : null;
            if (isIncrementDataWarehouse && string.IsNullOrEmpty(tenantUpdated)) return;

            TableClass objectFieldTable = tableLists.Where(d => d.TableName == "ObjectField").FirstOrDefault();
            TableClass tenantTable = tableLists.Where(d => d.TableName == "Tenant").FirstOrDefault();
            string customFieldSql = !isIncrementDataWarehouse ? CreateCustomObjectFieldsTempTable(desconnectionString) + "\n" : null;
            DataTable customObjectFields = GetCustomObjectFields(desconnectionString, tenantUpdated, objectFieldTable);

            var tenantLists = new List<int>();
            if (!isIncrementDataWarehouse)
            {
                tenantLists = GetCustomFieldTenantLists(desconnectionString);
            }
            else
            {
                DeleteRecordFromCustomObjectField(desconnectionString, tenantUpdated);
                foreach (string id in tenantUpdated.Split(','))
                {
                    if (!string.IsNullOrEmpty(id)) tenantLists.Add(Int32.Parse(id));
                }

            }

            foreach (int tenant in tenantLists)
            {
                customFieldSql += FillCustomObjectFieldsTempTable(tenant, customObjectFields, isIncrementDataWarehouse);
            }

            if (!isIncrementDataWarehouse)
            {
                string tableName = "dw_CustomObjectFields";
                customFieldSql += "\n" + "If OBJECT_ID('" + tableName + "','U')  IS NOT NULL Begin  Drop Table " + tableName + " End \r\n";
                customFieldSql += (" SELECT *  INTO " + tableName + " FROM #" + tableName + "Temp \r\n");
                customFieldSql += (" If(OBJECT_ID('tempdb..#" + tableName + "Temp') Is Not Null) Begin  Drop Table #" + tableName + "Temp End \r\n\r\n");
                customFieldSql += "  CREATE NONCLUSTERED INDEX [IX_" + tableName + "_" + "Tenant" + "] ON[dbo].[" + tableName + "]([" + "Tenant" + "])";
                customFieldSql += "  CREATE NONCLUSTERED INDEX [IX_" + tableName + "_" + "ObjectTableName" + "] ON[dbo].[" + tableName + "]([" + "ObjectTableName" + "])";

            }

            generalDataWarehouseService.ExecuteSql(customFieldSql, desconnectionString);
        }

        private void DeleteRecordFromCustomObjectField(string desconnectionString, string tenantUpdated)
        {
            using (SqlConnection sourceConnection = new SqlConnection(desconnectionString))
            {
                sourceConnection.Open();
                string sql = "delete dw_CustomObjectFields  where tenant in  (" + tenantUpdated + ")";
                SqlCommand commandSourceData = new SqlCommand(sql, sourceConnection);
                SqlDataReader reader = commandSourceData.ExecuteReader();
                reader.Close();
            }
        }


        private string GetCustomFieldTenantsUpdated(string desconnectionString )
        {
            string result = string.Empty;
            TableClass objectFieldTable = tableLists.Where(d => d.TableName == "ObjectField").FirstOrDefault();
            TableClass tenantTable = tableLists.Where(d => d.TableName == "Tenant").FirstOrDefault();
            if (string.IsNullOrEmpty(objectFieldTable.RefreshIds) && string.IsNullOrEmpty(tenantTable.RefreshIds)) return result;
            List<string> tenantNumbersLists = !string.IsNullOrEmpty(tenantTable.RefreshIds) ? tenantTable.RefreshIds.Replace("(", "").Replace(")", "").Replace("'", "").Split(',').ToList() : new List<string>();
            if (!string.IsNullOrEmpty(objectFieldTable.RefreshIds))
            {
                var customObjectFields = new DataTable();

                using (SqlConnection sourceConnection = new SqlConnection(desconnectionString))
                {
                    sourceConnection.Open();
                    string sql = "SELECT  Tenant from " + objectFieldTable.Dw_TableName + " where id in " + objectFieldTable.RefreshIds;
                    SqlCommand commandSourceData = new SqlCommand(sql, sourceConnection);
                    SqlDataReader reader = commandSourceData.ExecuteReader();
                    customObjectFields.Load(reader);
                    reader.Close();
                }

                var tenantLists = customObjectFields.AsEnumerable().GroupBy(row => row.Field<Int32>("Tenant").ToString()).Select(d => d.First().Field<Int32>("Tenant").ToString());
                if (tenantLists.Count() > 0)
                {

                    foreach (var tenant in tenantLists)
                    {
                        if (!tenantNumbersLists.Contains(tenant)) tenantNumbersLists.Add(tenant);

                    }

                }

            }

            if (tenantNumbersLists.Count() > 0)
            {
                foreach (string item in tenantNumbersLists)
                {
                    result += item + ",";
                }

                result += "@";
                result = result.Replace(",@", "");

            }

            return result;
        }

        private List<int> GetCustomFieldTenantLists(string desconnectionString)
        {
            List<int> tenantLists;
            var tenants = new DataTable();
            using (SqlConnection sourceConnection = new SqlConnection(desconnectionString))
            {
                sourceConnection.Open();
                SqlCommand commandSourceData = new SqlCommand("SELECT  Id from dw_Tenants", sourceConnection);
                SqlDataReader reader = commandSourceData.ExecuteReader();
                tenants.Load(reader);
                reader.Close();
            }
            tenantLists = (from rowfield in tenants.AsEnumerable()
                           select Int32.Parse(rowfield["Id"].ToString())).ToList();
            return tenantLists;
        }

        private DataTable GetCustomObjectFields(string desconnectionString, string tenantUpdated, TableClass objectFieldTable)
        {
            var customObjectFields = new DataTable();
            using (SqlConnection sourceConnection = new SqlConnection(desconnectionString))
            {
                sourceConnection.Open();
                string sql = "SELECT  FieldName,DataTypeCode,ObjectTableId,Tenant from " + objectFieldTable.Dw_TableName + " where IsCustom = 1";

                if (!string.IsNullOrEmpty(tenantUpdated))
                {
                    sql += " and tenant in (" + tenantUpdated + ")";
                }
                SqlCommand commandSourceData = new SqlCommand(sql, sourceConnection);
                SqlDataReader reader = commandSourceData.ExecuteReader();
                customObjectFields.Load(reader);
                reader.Close();
            }

            return customObjectFields;
        }

        private string CreateCustomObjectFieldsTempTable(string connectionString)
        {
            string tableName = "dw_CustomObjectFields";
            string cmd = "If(OBJECT_ID('tempdb..#" + tableName + "Temp') Is Not Null) Begin  Drop Table #" + tableName + "Temp End ; CREATE TABLE #" + tableName + "Temp (Tenant int not null,ObjectTableName varchar(50),CustomFields  varchar(4000));";

            return cmd;
        }
        private string FillCustomObjectFieldsTempTable(int tenant, DataTable customObjectFieldsTable, bool isIncrementDataWarehouse)
        {
            string result = "";
            string customFields = "";
            string customObjectFieldTableName = isIncrementDataWarehouse ? "dw_CustomObjectFields" : "#dw_CustomObjectFieldsTemp";
            List<string> tablesAddedCustomField = new List<string>();

            var customObjectFields = from rowfield in customObjectFieldsTable.AsEnumerable()
                                     where rowfield.Field<Int32>("Tenant") == tenant
                                     select rowfield;

            var customObjectFieldsGroups = customObjectFields.AsEnumerable().GroupBy(row => row.Field<string>("ObjectTableId"));

            foreach (var customObjectFieldsGroup in customObjectFieldsGroups)
            {


                List<DataRow> fields = customObjectFieldsGroup.ToList();
                string objectTableName = string.Empty;
                foreach (DataRow row in fields)
                {
                    string fieldName = row["FieldName"].ToString();
                    string dataTypeCode = row["DataTypeCode"].ToString();
                    string objectTableId = row["ObjectTableId"].ToString();


                    var table = tableLists.Where(d => d.ObjectTableId == objectTableId).FirstOrDefault();
                    if (table != null)
                    {
                        objectTableName = table.TableName;
                        tablesAddedCustomField.Add(objectTableName);
                    }
                    customFields += (fieldName + ":" + dataTypeCode + ",");
                }

                result += "insert into " + customObjectFieldTableName + " (Tenant, ObjectTableName , CustomFields) Values (" + tenant.ToString() + ", '" + objectTableName + "' , '" + customFields + "') \n";
            }

            foreach (string tableName in tableLists.Where(d=>d.HasCustomFields).Select(d=>d.TableName).ToList())
            {
                var table = tablesAddedCustomField.Where(d => d == tableName).FirstOrDefault();
                if (table == null)
                {
                    result += "insert into " + customObjectFieldTableName + " (Tenant, ObjectTableName , CustomFields) Values (" + tenant.ToString() + ", '" + tableName + "' , null) \n";

                }
            }

            return result;

        }


    }
}
