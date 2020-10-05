using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WarehouseData.Helper
{
    public class GeneralDataWarehouseService
    {


        public string ApplicationName = string.Empty;
        public string ApplicationMode = string.Empty;
        public long timeOut = 10000000000000000;
        ObjectFieldDataWarehouseService objectFieldDataWarehouseService;

        public GeneralDataWarehouseService(string applicationName = "WarehouseData", string applicationMode = "Debug")
        {
            this.ApplicationName = applicationName;
            this.ApplicationMode = applicationMode;
            objectFieldDataWarehouseService = new ObjectFieldDataWarehouseService();
        }

        public string GetDataWarehouseScriptByForderAndScriptName(string forderName, string scriptName)
        {
            string path = System.IO.Path.GetDirectoryName(new System.Uri(System.Reflection.Assembly.GetExecutingAssembly().CodeBase).LocalPath);
            if (ApplicationName != "Service")
            {
                if (path.Contains(@"\bin\" + ApplicationMode)) path = path.Replace(@"\bin\" + ApplicationMode, string.Empty);
            }
            string fileDirectory = Path.Combine(path, "WarehouseScript\\" + forderName, scriptName + ".sql");
            FileInfo file = new FileInfo(fileDirectory);
            string cmd = file.OpenText().ReadToEnd();
            return cmd;
        }

        public string CreateSqlDataWarehouseTable(TableClass table)
        {
            string sql = "If(OBJECT_ID('tempdb..#" + table.DWObjectTableCode + "Temp') Is Not Null) Begin  Drop Table #" + table.DWObjectTableCode + "Temp End \r\n";
            sql += " CREATE TABLE #" + table.DWObjectTableCode + "Temp ( \r\n";
            foreach (DWObjectFieldDB field in table.DWObjectFieldDBLists)
            {
                sql += field.FieldName + " ";
                sql += GetDataWarehouseSqlFieldType(field);
                sql += ",\r\n";
            }
            sql += ");";
            sql = sql.Replace(",);", ");");
            return sql;
        }

        private string GetDataWarehouseSqlFieldType(DWObjectFieldDB field)
        {
            string sqlFieldtype = string.Empty;

            if (field.DataTypeCode == "nText") sqlFieldtype += "nvarchar(" + field.MaxLength + ")";
            else if (field.DataTypeCode == "Text") sqlFieldtype += "varchar(" + field.MaxLength + ")";
            else if (field.DataTypeCode == "Boolean") sqlFieldtype += " bit";
            else if (field.DataTypeCode == "Decimal" || field.DataTypeCode == "Double") sqlFieldtype += " float";
            else if (field.DataTypeCode == "Integer") sqlFieldtype += " int";
            else if (field.DataTypeCode == "DateTime") sqlFieldtype += " dateTime";
            else if (field.DataTypeCode == "Date") sqlFieldtype += " date";
            else if (field.DataTypeCode == "SqlVariant") sqlFieldtype += " sql_variant";
            if (field.IsRequired) sqlFieldtype += " not null";
            if (field.IsPrimaryKey)
            {
                if (field.FieldName == "[Id_Number]") sqlFieldtype += " identity(1, 1)";
                sqlFieldtype += "  primary key";
            }

            return sqlFieldtype;
        }

        public string GetSqlCopyDataFromTempTableToActualTable(TableClass table)
        {
            string result = "If OBJECT_ID('New" + table.DWObjectTableCode + "','U')  IS NOT NULL Begin  Drop Table New" + table.DWObjectTableCode + " End \r\n";
            result += (" SELECT *  INTO New" + table.DWObjectTableCode + " FROM #" + table.DWObjectTableCode + "Temp \r\n");
            result += (" If(OBJECT_ID('tempdb..#" + table.DWObjectTableCode + "Temp') Is Not Null) Begin  Drop Table #" + table.DWObjectTableCode + "Temp End \r\n\r\n");

            return result;
        }

        public void ExecuteSql(string sqlString, string connectionString)
        {

            if (!string.IsNullOrEmpty(sqlString))
            {
                using (SqlConnection cn = new SqlConnection(connectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(sqlString, cn);
                    sqlCommand.CommandTimeout = (int)timeOut;
                    cn.Open();
                    sqlCommand.ExecuteNonQuery();
                    cn.Close();
                }
            }
        }



        public List<TableClass> FillDataWarehouseTable()
        {
            List<TableClass> tableNameLists = new List<TableClass>();
            //DW Table
            tableNameLists.Add(new TableClass() { TableName = "DWHSetting", DBTableName = "DWHSettings", Dw_TableName = "dw_DWHSettings", KeyName = "Tenant", HasConstraint = true, HasNotSpecifiedValue = true });
            tableNameLists.Add(new TableClass() { TableName = "Address", DBTableName = "Addresses", Dw_TableName = "dw_Addresses", KeyName = "Id", HasConstraint = true, HasNotSpecifiedValue = true });
            tableNameLists.Add(new TableClass() { TableName = "Country", DBTableName = "Countries", Dw_TableName = "dw_Countries", KeyName = "Id", HasNotSpecifiedValue = true });
            tableNameLists.Add(new TableClass() { TableName = "State", DBTableName = "States", Dw_TableName = "dw_States", KeyName = "Id", HasNotSpecifiedValue = true });
            tableNameLists.Add(new TableClass() { TableName = "PartnerType", IsCloseTable = true, DBTableName = "PartnerTypes", Dw_TableName = "dw_PartnerTypes", KeyName = "Id", HasNotSpecifiedValue = true });
            tableNameLists.Add(new TableClass() { TableName = "ObjectField", DBTableName = "ObjectFields", Dw_TableName = "dw_ObjectFields", KeyName = "Id", FieldsDBName = "FieldName,DataTypeCode,ObjectTableId,IsCustom" });
            tableNameLists.Add(new TableClass() { TableName = "Rank", DBTableName = "Ranks", Dw_TableName = "dw_Ranks", KeyName = "Id", });
            tableNameLists.Add(new TableClass() { TableName = "LeadSource", DBTableName = "LeadSources", Dw_TableName = "dw_LeadSources", KeyName = "Id", });

            tableNameLists.Add(new TableClass() { TableName = "Region", DBTableName = "Regions", Dw_TableName = "dw_Regions", KeyName = "Id" });
            tableNameLists.Add(new TableClass() { TableName = "CustomerSize", DBTableName = "CustomerSizes", Dw_TableName = "dw_CustomerSizes", KeyName = "Id" });
            tableNameLists.Add(new TableClass() { TableName = "Industry", DBTableName = "Industries", Dw_TableName = "dw_Industries", KeyName = "Id" });
            tableNameLists.Add(new TableClass() { TableName = "ShipmentMasterData", FieldsDBName = "MasterShipmentNumber,StatusDate,StatusId", DBTableName = "ShipmentMasterDatas", Dw_TableName = "dw_ShipmentMasterDatas", KeyName = "Id", HasNotSpecifiedValue = true, HasConstraint = true, DispayInScreen = true });
            tableNameLists.Add(new TableClass() { TableName = "ShipmentComputedFields", DBTableName = "ShipmentComputedFields", Dw_TableName = "dw_ShipmentComputedFields", KeyName = "Id", HasConstraint = true, DispayInScreen = true });
            tableNameLists.Add(new TableClass() { TableName = "ShipmentPayable", AdditionalIndexes = "ShipmentId" , ParentKeyName = "ShipmentId", HasConstraint = true, DBTableName = "ShipmentPayables", DispayInScreen = true, Dw_TableName = "dw_ShipmentPayables", KeyName = "Id", HasNotSpecifiedValue = true });
            tableNameLists.Add(new TableClass() { TableName = "APInvoiceLine", DBTableName = "APInvoiceLines", AdditionalIndexes = "EntityPayableId", DispayInScreen = true, Dw_TableName = "dw_APInvoiceLines", FieldsDBName = "EntityPayableId", KeyName = "APInvoiceId",  HasNotSpecifiedValue = true });
            tableNameLists.Add(new TableClass() { TableName = "APInvoice", DBTableName = "APInvoices", DispayInScreen = true, Dw_TableName = "dw_APInvoices", KeyName = "Id", HasNotSpecifiedValue = true });
            tableNameLists.Add(new TableClass() { TableName = "ShipmentReceivable", AdditionalIndexes = "ShipmentId" , ParentKeyName = "ShipmentId",  DispayInScreen = true, DBTableName = "ShipmentReceivables", Dw_TableName = "dw_ShipmentReceivables", KeyName = "Id", HasNotSpecifiedValue = true });
            tableNameLists.Add(new TableClass() { TableName = "ARInvoiceLine", FieldsDBName = "ARInvoiceId,ReceivableId",  DispayInScreen = true, AdditionalIndexes = "ReceivableId", DBTableName = "ARInvoiceLines", Dw_TableName = "dw_ARInvoiceLines", KeyName = "Id", HasNotSpecifiedValue = true });
            tableNameLists.Add(new TableClass() { TableName = "ARInvoice", HasConstraint = true, DispayInScreen = true, DBTableName = "ARInvoices", Dw_TableName = "dw_ARInvoices", KeyName = "Id", HasNotSpecifiedValue = true });
            tableNameLists.Add(new TableClass() { TableName = "Shipment", RelatedEntities = tableNameLists.Where(d => d.TableName == "ShipmentPayable" || d.TableName == "ShipmentReceivable").ToList(), FieldsDBName = (("ComputedStatusId,ComputedStatusDate,") +  GetCustomFieldAsDBFieldOnTable(40)), KeyName = "Id", DBTableName = "Shipments", Dw_TableName = "dw_Shipments", HasConstraint = true, DispayInScreen = true , });
            

            //Dimension  Table
            tableNameLists.Add(new TableClass() { TableName = "CustomPickList", DBTableName = "CustomPickLists", Dw_TableName = "dw_CustomPickLists", KeyName = "Id", HasDimensionTable = true, DWObjectTableCode = "DIM_CustomPickLists", BuildScriptName = "BuildCustomPickListDimensionsTable", IncrementalScriptName = "UpdateCustomPickListDimensionsTable", FieldsDBName = "Code,Value,IsMultipleChoice" });
            tableNameLists.Add(new TableClass() { IsCloseTable = true, TableName = "Direction", DBTableName = "Directions", Dw_TableName = "dw_Directions", KeyName = "Id", HasDimensionTable = true, DWObjectTableCode = "DIM_Directions", BuildScriptName = "BuildDirectionDimensionsTable", IncrementalScriptName = "UpdateDirectionDimensionTable" });
            tableNameLists.Add(new TableClass() { IsCloseTable = true, TableName = "TransportMode", DBTableName = "TransportModes", Dw_TableName = "dw_TransportModes", KeyName = "Id", HasDimensionTable = true, DWObjectTableCode = "DIM_TransportModes", BuildScriptName = "BuildTransportModeDimensionTable", IncrementalScriptName = "UpdateTransportModeDimensionTable" });
            tableNameLists.Add(new TableClass() { IsCloseTable = true, TableName = "ShipmentLevel", DBTableName = "ShipmentLevels", Dw_TableName = "dw_Levels", KeyName = "Code", HasDimensionTable = true, DWObjectTableCode = "DIM_Levels", BuildScriptName = "BuildShipmentLevelDimensionTable", IncrementalScriptName = "UpdateShipmentLevelDimensionTable" });
            tableNameLists.Add(new TableClass() { IsCloseTable = true, TableName = "OBLType", DBTableName = "OBLTypes", Dw_TableName = "dw_OBLTypes", KeyName = "Code", HasDimensionTable = true, DWObjectTableCode = "DIM_OBLTypes", BuildScriptName = "BuildOBLTypeDimensionTable", IncrementalScriptName = "UpdateOBLTypeDimensionTable" });
            tableNameLists.Add(new TableClass() { IsCloseTable = true, TableName = "ShipmentType", DBTableName = "ShipmentTypes", Dw_TableName = "dw_Types", KeyName = "Id", HasDimensionTable = true, DWObjectTableCode = "DIM_Types", HasNotSpecifiedValue = true, BuildScriptName = "BuildShipmentTypeDimensionTable", IncrementalScriptName = "UpdateShipmentTypeDimensionTable" });
            tableNameLists.Add(new TableClass() { TableName = "Branch", DBTableName = "Branches", Dw_TableName = "dw_Branches", KeyName = "Id", HasDimensionTable = true, DWObjectTableCode = "DIM_Branches", BuildScriptName = "BuildBrancheDimensionTable", IncrementalScriptName = "UpdateBrancheDimensionTable" });
            tableNameLists.Add(new TableClass() { TableName = "EntityStatus", DBTableName = "EntityStatus", FieldsDBName = "StatusWeight",  Dw_TableName = "dw_ShipmentStatuses", KeyName = "Id", HasDimensionTable = true, DWObjectTableCode = "DIM_ShipmentStatuses", BuildScriptName = "BuildEntityStatusDimensionTable", IncrementalScriptName = "UpdateEntityStatusDimensionTable" });
            tableNameLists.Add(new TableClass() { TableName = "Card", DBTableName = "Cards", Dw_TableName = "dw_Partners", KeyName = "Id", HasDimensionTable = true, DWObjectTableCode = "DIM_Partners", BuildScriptName = "BuildCardsDimensionTable", IncrementalScriptName = "UpdateCardDimensionTable", HasConstraint = true, DispayInScreen = true });
            tableNameLists.Add(new TableClass() { TableName = "Port", DBTableName = "Ports", Dw_TableName = "dw_Ports", KeyName = "Id", HasDimensionTable = true, DWObjectTableCode = "DIM_Ports", BuildScriptName = "BuildPortsDimensionTable", IncrementalScriptName = "UpdatePortsDimensionTable", HasConstraint = true, DispayInScreen = true });
            tableNameLists.Add(new TableClass() { TableName = "User", DBTableName = "Users", Dw_TableName = "dw_Users", KeyName = "Id", HasDimensionTable = true, DWObjectTableCode = "DIM_Users", BuildScriptName = "BuildUsersDimensionTable", IncrementalScriptName = "UpdateUsersDimensionTable", DispayInScreen = true });
            tableNameLists.Add(new TableClass() { TableName = "Contact", DBTableName = "Contacts", Dw_TableName = "dw_Contacts", KeyName = "Id", HasNotSpecifiedValue = true, DispayInScreen = true });
            tableNameLists.Add(new TableClass() { TableName = "Customer", DBTableName = "Customers", Dw_TableName = "dw_Customers", KeyName = "Id", DispayInScreen = true, HasConstraint = true });
            tableNameLists.Add(new TableClass() { TableName = "Tenant", DBTableName = "Tenants", Dw_TableName = "dw_Tenants", KeyName = "Id", HasDimensionTable = true, DWObjectTableCode = "DIM_Tenants", HasConstraint = true, BuildScriptName = "BuildTenantDimensionTable", IncrementalScriptName = "UpdateTenantDimensionTable", DispayInScreen = true });
            tableNameLists.Add(new TableClass() { TableName = "Department", DBTableName = "Departments", Dw_TableName = "dw_Departments", KeyName = "Id", HasDimensionTable = true, DWObjectTableCode = "DIM_Departments", BuildScriptName = "BuildDepartmentDimensionTable", IncrementalScriptName = "UpdateDepartmentDimensionTable", DispayInScreen = true });
            tableNameLists.Add(new TableClass() { TableName = "Incoterm", DBTableName = "Incoterms", Dw_TableName = "dw_Incoterms", KeyName = "Id", HasDimensionTable = true, DWObjectTableCode = "DIM_Incoterms", BuildScriptName = "BuildIncotermDimensionTable", IncrementalScriptName = "UpdateIncotermDimensionTable", DispayInScreen = true });
            tableNameLists.Add(new TableClass() { TableName = "Currency", DBTableName = "Currencies", Dw_TableName = "dw_Currencies", KeyName = "Id", HasDimensionTable = true, DWObjectTableCode = "DIM_Currencies", BuildScriptName = "BuildCurrencyDimensionTable", IncrementalScriptName = "UpdateCurrencyDimensionTable", DispayInScreen = true });
            tableNameLists.Add(new TableClass() { TableName = "MoveType", DBTableName = "MoveTypes", Dw_TableName = "dw_MoveTypes", KeyName = "Id", HasDimensionTable = true, DWObjectTableCode = "DIM_MoveTypes", BuildScriptName = "BuildMoveTypeDimensionTable", IncrementalScriptName = "UpdateMoveTypeDimensionTable", HasConstraint = true });
            tableNameLists.Add(new TableClass() { TableName = "Vessel", DBTableName = "Vessels", Dw_TableName = "dw_Vessels", KeyName = "Id", HasDimensionTable = true, DWObjectTableCode = "DIM_Vessels", BuildScriptName = "BuildVesselDimensionTable", IncrementalScriptName = "UpdateVesselDimensionTable", HasConstraint = true });
            tableNameLists.Add(new TableClass() { TableName = "SpecialServicesType", DBTableName = "SpecialServicesTypes", Dw_TableName = "dw_SpecialServicesTypes", KeyName = "Id", HasDimensionTable = true, DWObjectTableCode = "DIM_SpecialServicesTypes", BuildScriptName = "BuildSpecialServicesTypeDimensionTable", IncrementalScriptName = "UpdateSpecialServicesTypeDimensionTable", HasConstraint = true });
            tableNameLists.Add(new TableClass() { TableName = "ChargesType", DBTableName = "ChargesTypes", DispayInScreen = true, Dw_TableName = "dw_ChargesTypes", KeyName = "Id", HasDimensionTable = true, DWObjectTableCode = "DIM_ChargesTypes", BuildScriptName = "BuildChargesTypeDimensionTable", IncrementalScriptName = "UpdateChargesTypeDimensionTable" });
            tableNameLists.Add(new TableClass() { IsCloseTable = true, TableName = "ShipmentPayableStatus", DBTableName = "ShipmentPayableStatus", DispayInScreen = true, Dw_TableName = "dw_ShipmentPayableStatuses", KeyName = "Code", HasDimensionTable = true, DWObjectTableCode = "DIM_ShipmentPayableStatuses", BuildScriptName = "BuildShipmentPayableStatusDimensionTable", IncrementalScriptName = "UpdateShipmentPayableStatusDimensionTable" });
            tableNameLists.Add(new TableClass() { IsCloseTable = true, TableName = "ShipmentReceivableStatus", DBTableName = "ShipmentReceivableStatus", DispayInScreen = true, Dw_TableName = "dw_ShipmentReceivableStatuses", KeyName = "Code", HasDimensionTable = true, DWObjectTableCode = "DIM_ShipmentReceivableStatuses", BuildScriptName = "BuildShipmentReceivableStatusDimensionTable", IncrementalScriptName = "UpdateShipmentReceivableStatusDimensionTable" });

            //Fact Table
            tableNameLists.Add(new TableClass() { TableName = "Shipment", FieldIndexes = "Source Tenant,Parent Tenant,Id,DirectHouse", DWObjectTableCode = "Fact_Shipments", KeyName = "Id", DWTableKeyName = "Id", Dw_TableName = "dw_Shipments", HasFactTable = true, BuildScriptName = "BuildFactShipmentTable", IncrementalScriptName = "UpdateFactShipmentTable", HasCustomFields = true, CustomFieldsCount = 40, DispayInScreen = true });
            tableNameLists.Add(new TableClass() { TableName = "Shipment", FieldIndexes = "Source Tenant,Parent Tenant,Shipment Id,DirectHouse", DWObjectTableCode = "Fact_Charges", Dw_TableName = "dw_Shipments", KeyName = "[Shipment Id]", DWTableKeyName = "Id", HasFactTable = true, BuildScriptName = "BuildFactChargesTable", IncrementalScriptName = "UpdateFactChargesTable", DispayInScreen = true });


            //WaterMark
            tableNameLists.Add(new TableClass() { TableName = "WaterMark", DBTableName = "WaterMarks", Dw_TableName = "dw_WaterMarks", KeyName = "TableName", FieldsDBName = "TableName,LastUpdateDate" });


            return tableNameLists;

        }

        private string GetCustomFieldAsDBFieldOnTable(int customFieldsCount)
        {
            int i = 1;
            string customFieldsDBName = string.Empty;
            while (i <= customFieldsCount)
            {

                customFieldsDBName += ( i!=1 ? ",":"" )+   ("Field" + i);
                i += 1;
            }

            return customFieldsDBName;
        }

        public void RunSqlFunctions(string connectionString)
        {
            ExecuteScript("Others", "Day 06 [Abed]Add Function Date", connectionString);
            ExecuteScript("Others", "Day 18[AbedAddFuncationSplitString]", connectionString);
            ExecuteScript("Others", "Day 17[AbedAddFuncationResolveCustomFieldDateValue]", connectionString);
            ExecuteScript("Others", "Day 14 [Abed]AddFunctionResolveCustomFieldValue", connectionString);
        }

        public void ExecuteScript(string forderName, string scriptName, string connectionString)
        {
            string cmd = GetDataWarehouseScriptByForderAndScriptName(forderName, scriptName);

            ExecuteSql(cmd, connectionString);

        }



        public string BuildConnectionString(string catalog, string userName, string password, string server)
        {
            string result = "Data Source=" + server + ";Initial Catalog=" + catalog + ";Integrated Security=False;Persist Security Info=True;User ID=" + userName + ";Password= " + password + ";MultipleActiveResultSets=True;Connect Timeout=60";
            return result;
        }

        public string BuildConnectionString(string dbSourceConnection)
        {
            string result = string.Empty;

            string[] sourceConnectionArray = dbSourceConnection.Split(',');
            if (sourceConnectionArray.Length == 4)
            {
                result = BuildConnectionString(sourceConnectionArray[0], sourceConnectionArray[1], sourceConnectionArray[2], sourceConnectionArray[3]);
            }

            return result;
        }

        public string DeleteRowsFromDataWarehouse(DeleteRowsArgs deleteRowsArgs)
        {
            int rowsCount = 0;
            StringBuilder allDeletedRows = new StringBuilder();
            StringBuilder deletedRows = new StringBuilder();
            foreach (string id in deleteRowsArgs.IdsList)
            {
                rowsCount += 1;
                deletedRows.Append("'" + id + "'" + ",");
                if (rowsCount == 1000 || (deleteRowsArgs.IdsList.IndexOf(id) == deleteRowsArgs.IdsList.IndexOf(deleteRowsArgs.IdsList.Last())))
                {
                    if (deleteRowsArgs.ReturnDeleteIdsAsString) allDeletedRows.Append(deletedRows.ToString());
                    string cmd = "delete " + deleteRowsArgs.TableName + " where " + deleteRowsArgs.KeyName + " in " + ("(" + deletedRows.ToString() + ")").Replace(",)", ")");
                    ExecuteSql(cmd, deleteRowsArgs.ConnectionString);
                    rowsCount = 0;
                    deletedRows.Clear();
                }
            }

            return !string.IsNullOrEmpty(allDeletedRows.ToString()) ? ("(" + allDeletedRows.ToString() + ")").Replace(",)", ")") : null;

        }

        public int GetRecordDataCountByTableName(string tableName, string connectionString)
        {
            int count = 0;

            using (SqlConnection sourceConnection =
                       new SqlConnection(connectionString))
            {
                sourceConnection.Open();
                string sql = "SELECT COUNT(*) FROM dbo." + tableName + ";";
                SqlCommand commandRowCount = new SqlCommand(sql, sourceConnection);

                try
                {
                    count = (int)commandRowCount.ExecuteScalar();


                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message);
                }

            }
            return count;
        }

        public void CreateWaterMarksTable(string tableName, string connectionString, bool isPrivate = false)
        {
            string cmd = "IF OBJECT_ID ('" + tableName + "', 'U')  IS NOT NULL drop table " + tableName + " ; CREATE TABLE " + tableName + " (TableName varchar(100) not null,LastUpdateDate  datetime," + (isPrivate ? "PrivateTenant  int" : "") + ");CREATE NONCLUSTERED INDEX [IX_" + tableName + "_TableName] ON[dbo].[" + tableName + "]([TableName])";
            cmd += ";CREATE NONCLUSTERED INDEX [IX_" + tableName + "_LastUpdateDate] ON[dbo].[" + tableName + "]([LastUpdateDate])";
            cmd += isPrivate ? ";CREATE NONCLUSTERED INDEX [IX_PrivateWaterMarks_PrivateTenant] ON[dbo].[PrivateWaterMarks]([PrivateTenant])" : "";
            ExecuteSql(cmd, connectionString);

        }



    }


}

public class DeleteRowsArgs
{
    public string TableName { get; set; }
    public string KeyName { get; set; }
    public string ConnectionString { get; set; }
    public List<string> IdsList { get; set; }
    public bool ReturnDeleteIdsAsString { get; set; }

}
