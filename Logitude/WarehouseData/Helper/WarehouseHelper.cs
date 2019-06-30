using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WarehouseData.Helper
{
    public class WarehouseHelper
    {

        #region General 
        long timeOut = 10000000000000000;
        string AppName = string.Empty;
        string Mode = string.Empty;
        int CustomFieldsCount = 40;
        List<TableClass> tableLists = new List<TableClass>();
        public WarehouseHelper(string appName = "WarehouseData", string mode = "Debug")
        {
            this.AppName = appName;
            this.Mode = mode;
        }

        public List<TableClass> FillTable()
        {
            List<TableClass> tableNameLists = new List<TableClass>();
            tableNameLists.Add(new TableClass() { TableName = "DWHSetting", DBTableName = "DWHSettings", Dw_TableName = "dw_DWHSettings", KeyName = "Tenant", HasConstraint = true, HasNotSpecifiedValue = true });
            tableNameLists.Add(new TableClass() { TableName = "Address", DBTableName = "Addresses", Dw_TableName = "dw_Addresses", KeyName = "Id", HasConstraint = true, HasNotSpecifiedValue = true });
            tableNameLists.Add(new TableClass() { TableName = "Country", DBTableName = "Countries", Dw_TableName = "dw_Countries", KeyName = "Id", HasNotSpecifiedValue = true });
            tableNameLists.Add(new TableClass() { TableName = "State", DBTableName = "States", Dw_TableName = "dw_States", KeyName = "Id", HasNotSpecifiedValue = true });
            tableNameLists.Add(new TableClass() { TableName = "PartnerType", DBTableName = "PartnerTypes", Dw_TableName = "dw_PartnerTypes", KeyName = "Id", HasNotSpecifiedValue = true });
            tableNameLists.Add(new TableClass() { TableName = "ObjectField", DBTableName = "ObjectFields", Dw_TableName = "dw_ObjectFields", KeyName = "Id" , FieldsDBName = "FieldName,DataTypeCode" });

            tableNameLists.Add(new TableClass() { IsCloseTable = true, TableName = "Direction", DBTableName = "Directions", Dw_TableName = "dw_Directions", KeyName = "Id", HasDimensionTable = true, DWObjectTableCode = "DIM_Directions", BuildScriptName = "BuildDirectionDimensionsTable", IncrementalScriptName = "UpdateDirectionDimensionTable" });
            tableNameLists.Add(new TableClass() { IsCloseTable = true, TableName = "TransportMode", DBTableName = "TransportModes", Dw_TableName = "dw_TransportModes", KeyName = "Id", HasDimensionTable = true, DWObjectTableCode = "DIM_TransportModes", BuildScriptName = "BuildTransportModeDimensionTable", IncrementalScriptName = "UpdateTransportModeDimensionTable" });
            tableNameLists.Add(new TableClass() { IsCloseTable = true, TableName = "ShipmentLevel", DBTableName = "ShipmentLevels", Dw_TableName = "dw_Levels", KeyName = "Code", HasDimensionTable = true, DWObjectTableCode = "DIM_Levels", BuildScriptName = "BuildShipmentLevelDimensionTable", IncrementalScriptName = "UpdateShipmentLevelDimensionTable" });
            tableNameLists.Add(new TableClass() { IsCloseTable = true, TableName = "ShipmentType", DBTableName = "ShipmentTypes", Dw_TableName = "dw_Types", KeyName = "Id", HasDimensionTable = true, DWObjectTableCode = "DIM_Types", HasNotSpecifiedValue = true, BuildScriptName = "BuildShipmentTypeDimensionTable", IncrementalScriptName = "UpdateShipmentTypeDimensionTable" });
            tableNameLists.Add(new TableClass() { TableName = "Branch", DBTableName = "Branches", Dw_TableName = "dw_Branches", KeyName = "Id", HasDimensionTable = true, DWObjectTableCode = "DIM_Branches", BuildScriptName = "BuildBrancheDimensionTable", IncrementalScriptName = "UpdateBrancheDimensionTable" });
            tableNameLists.Add(new TableClass() { TableName = "EntityStatus", DBTableName = "EntityStatus", Dw_TableName = "dw_ShipmentStatuses", KeyName = "Id", HasDimensionTable = true, DWObjectTableCode = "DIM_ShipmentStatuses", BuildScriptName = "BuildEntityStatusDimensionTable", IncrementalScriptName = "UpdateEntityStatusDimensionTable" });
            tableNameLists.Add(new TableClass() { TableName = "Rank", DBTableName = "Ranks", Dw_TableName = "dw_Ranks", KeyName = "Id", });
            tableNameLists.Add(new TableClass() { TableName = "Shipment", FieldIndexes = "Source Tenant,Parent Tenant", DWObjectTableCode = "Fact_Shipments", FieldsDBName = "ToPortId,FromPortId", KeyName = "Id", DBTableName = "Shipments", Dw_TableName = "dw_Shipments", HasConstraint = true, HasFactTable = true, BuildScriptName = "BuildFactShipmentTable", IncrementalScriptName = "UpdateFactShipmentTable", DispayInScreen = true });
            tableNameLists.Add(new TableClass() { TableName = "ShipmentMasterData", FieldsDBName = "MasterShipmentNumber", DBTableName = "ShipmentMasterDatas", Dw_TableName = "dw_ShipmentMasterDatas", KeyName = "Id", HasNotSpecifiedValue = true, HasConstraint = true, DispayInScreen = true });
            tableNameLists.Add(new TableClass() { TableName = "Card", DBTableName = "Cards", Dw_TableName = "dw_Partners", KeyName = "Id", HasDimensionTable = true, DWObjectTableCode = "DIM_Partners", BuildScriptName = "BuildCardsDimensionTable", IncrementalScriptName = "UpdateCardDimensionTable", HasConstraint = true, DispayInScreen = true });
            tableNameLists.Add(new TableClass() { TableName = "Port", DBTableName = "Ports", Dw_TableName = "dw_Ports", KeyName = "Id", HasDimensionTable = true, DWObjectTableCode = "DIM_Ports", BuildScriptName = "BuildPortsDimensionTable", IncrementalScriptName = "UpdatePortsDimensionTable", HasConstraint = true, DispayInScreen = true });
            tableNameLists.Add(new TableClass() { TableName = "User", DBTableName = "Users", Dw_TableName = "dw_Users", KeyName = "Id", HasDimensionTable = true, DWObjectTableCode = "DIM_Users", BuildScriptName = "BuildUsersDimensionTable", IncrementalScriptName = "UpdateUsersDimensionTable", DispayInScreen = true });
            tableNameLists.Add(new TableClass() { TableName = "Contact", DBTableName = "Contacts", Dw_TableName = "dw_Contacts", KeyName = "Id", HasNotSpecifiedValue = true, DispayInScreen = true });
            tableNameLists.Add(new TableClass() { TableName = "Customer", DBTableName = "Customers", Dw_TableName = "dw_Customers", KeyName = "Id", DispayInScreen = true });
            tableNameLists.Add(new TableClass() { TableName = "Tenant", DBTableName = "Tenants", Dw_TableName = "dw_Tenants", KeyName = "Id", HasDimensionTable = true, DWObjectTableCode = "DIM_Tenants", HasConstraint = true, BuildScriptName = "BuildTenantDimensionTable", IncrementalScriptName = "UpdateTenantDimensionTable", DispayInScreen = true });
            tableNameLists.Add(new TableClass() { TableName = "Department", DBTableName = "Departments", Dw_TableName = "dw_Departments", KeyName = "Id", HasDimensionTable = true, DWObjectTableCode = "DIM_Departments", BuildScriptName = "BuildDepartmentDimensionTable", IncrementalScriptName = "UpdateDepartmentDimensionTable", DispayInScreen = true });
            tableNameLists.Add(new TableClass() { TableName = "Incoterm", DBTableName = "Incoterms", Dw_TableName = "dw_Incoterms", KeyName = "Id", HasDimensionTable = true, DWObjectTableCode = "DIM_Incoterms", BuildScriptName = "BuildIncotermDimensionTable", IncrementalScriptName = "UpdateIncotermDimensionTable", DispayInScreen = true });
            tableNameLists.Add(new TableClass() { TableName = "Currency", DBTableName = "Currencies", Dw_TableName = "dw_Currencies", KeyName = "Id", HasDimensionTable = true, DWObjectTableCode = "DIM_Currencies", BuildScriptName = "BuildCurrencyDimensionTable", IncrementalScriptName = "UpdateCurrencyDimensionTable", DispayInScreen = true });
            tableNameLists.Add(new TableClass() { TableName = "MoveType", DBTableName = "MoveTypes", Dw_TableName = "dw_MoveTypes", KeyName = "Id", HasDimensionTable = true, DWObjectTableCode = "DIM_MoveTypes", BuildScriptName = "BuildMoveTypeDimensionTable", IncrementalScriptName = "UpdateMoveTypeDimensionTable", HasConstraint = true  });
            tableNameLists.Add(new TableClass() { TableName = "Vessel", DBTableName = "Vessels", Dw_TableName = "dw_Vessels", KeyName = "Id", HasDimensionTable = true, DWObjectTableCode = "DIM_Vessels", BuildScriptName = "BuildVesselDimensionTable", IncrementalScriptName = "UpdateVesselDimensionTable", HasConstraint = true });
            tableNameLists.Add(new TableClass() { TableName = "SpecialServicesType", DBTableName = "SpecialServicesTypes", Dw_TableName = "dw_SpecialServicesTypes", KeyName = "Id", HasDimensionTable = true, DWObjectTableCode = "DIM_SpecialServicesTypes", BuildScriptName = "BuildSpecialServicesTypeDimensionTable", IncrementalScriptName = "UpdateSpecialServicesTypeDimensionTable", HasConstraint = true });
            tableNameLists.Add(new TableClass() { TableName = "WaterMark", DBTableName = "WaterMarks", Dw_TableName = "dw_WaterMarks", KeyName = "TableName", FieldsDBName = "TableName,LastUpdateDate" });
            FullShipmentCustomFields(tableNameLists.Where(d => d.TableName == "Shipment").FirstOrDefault());
            tableLists = tableNameLists;
            return tableNameLists;

        }

        private void FullShipmentCustomFields(TableClass tableClass)
        {
            int i = 1;
            while (i <= CustomFieldsCount)
            {
                tableClass.FieldsDBName += (",Field" + i);
                i += 1;
            }
        }

        public void BuildDWObjectFieldDB(List<TableClass> tableNameLists, string connectionString)
        {

            var dWObjectFieldDB = new DataTable();

            using (SqlConnection sourceConnection = new SqlConnection(connectionString))
            {
                sourceConnection.Open();
                SqlCommand commandSourceData = new SqlCommand("SELECT  Code, DWObjectTableCode,DataTypeCode,MaxLength,MinLength,IsRequired ,IsPrimaryKey,DimensionTableCode from DWObjectFields", sourceConnection);
                SqlDataReader reader = commandSourceData.ExecuteReader();
                dWObjectFieldDB.Load(reader);
                reader.Close();

            }

            foreach (TableClass tableClass in tableNameLists.Where(d => d.HasDimensionTable || d.HasFactTable).ToList())
            {
                var rowList = dWObjectFieldDB.AsEnumerable().Where(row => row["DWObjectTableCode"].ToString() == tableClass.DWObjectTableCode).ToList();
                tableClass.DWObjectFieldDBLists = new List<DWObjectFieldDB>();
                foreach (DataRow row in rowList)
                {
                    var fieldDB = new DWObjectFieldDB();
                    fieldDB.FieldName = row["Code"].ToString();
                    fieldDB.DataTypeCode = row["DataTypeCode"].ToString();
                    fieldDB.MaxLength = Int32.Parse(row["MaxLength"].ToString());
                    fieldDB.MinLength = Int32.Parse(row["MinLength"].ToString());
                    fieldDB.IsRequired = bool.Parse(row["IsRequired"].ToString());
                    fieldDB.IsPrimaryKey = bool.Parse(row["IsPrimaryKey"].ToString());
                    fieldDB.DimensionTableCode = row["DimensionTableCode"].ToString();
                    tableClass.DWObjectFieldDBLists.Add(fieldDB);
                }
            }

            foreach (TableClass tableClass in tableNameLists.Where(d => d.HasDimensionTable || d.HasFactTable).ToList())
            {
                if (tableClass.DWObjectFieldDBLists != null)
                {
                    foreach (DWObjectFieldDB field in tableClass.DWObjectFieldDBLists.Where(d => d.DataTypeCode == "Dimension").ToList())
                    {
                        if (field.DimensionTableCode != "DIM_Dates")
                        {
                            var dimensionTable = tableNameLists.Where(d => d.DWObjectTableCode == field.DimensionTableCode).FirstOrDefault();
                            if (dimensionTable != null)
                            {
                                DWObjectFieldDB orginalField = dimensionTable.DWObjectFieldDBLists.Where(d => d.IsPrimaryKey).FirstOrDefault();
                                if (orginalField != null)
                                {
                                    field.DataTypeCode = orginalField.DataTypeCode;
                                    field.MaxLength = orginalField.MaxLength;
                                    field.MinLength = orginalField.MinLength;
                                    field.IsRequired = orginalField.IsRequired;
                                    field.IsPrimaryKey = false;
                                }
                            }
                        }
                        else field.DataTypeCode = "Integer";

                    }
                }
            }

        }

        public void RunSqlFunctions(string connectionString)
        {
            ExecuteScript("Others", connectionString, "Day 06 [Abed]Add Function Date");
            ExecuteScript("Others", connectionString, "Day 17[AbedAddFuncationResolveCustomFieldDateValue]");
            ExecuteScript("Others", connectionString, "Day 14 [Abed]AddFunctionResolveCustomFieldValue");
        }

        public void BuildWarehouseObjectField(List<TableClass> tableNameLists, string connectionString)
        {
            var objectFieldsTable = new DataTable();
            using (SqlConnection sourceConnection = new SqlConnection(connectionString))
            {
                sourceConnection.Open();
                SqlCommand commandSourceData = new SqlCommand("SELECT  FieldName,ObjectTableId from ObjectFields where CopyToDW = 1", sourceConnection);
                SqlDataReader reader = commandSourceData.ExecuteReader();
                objectFieldsTable.Load(reader);
                reader.Close();
            }

            using (SqlConnection sourceConnection = new SqlConnection(connectionString))
            {
                sourceConnection.Open();
                string sql = "SELECT  Id,Name from  ObjectTables where Name in (";
                foreach (TableClass table in tableNameLists)
                {
                    string name = table.TableName == "ShipmentMasterData" ? "Master" : table.TableName;
                    sql += ("'" + name + "',");
                }
                sql += ")";
                sql = sql.Replace(",)", ")");

                SqlCommand commandSourceData = new SqlCommand(sql, sourceConnection);
                SqlDataReader reader = commandSourceData.ExecuteReader();
                var objectTables = new DataTable();
                objectTables.Load(reader);

                reader.Close();

                foreach (DataRow row in objectTables.Rows)
                {
                    string tableName = row["Name"].ToString() == "Master" ? "ShipmentMasterData" : row["Name"].ToString();
                    string tableId = row["Id"].ToString();

                    var fields = from rowfield in objectFieldsTable.AsEnumerable()
                                 where rowfield.Field<string>("ObjectTableId") == tableId
                                 select rowfield;

                    var result = fields
                             .Cast<DataRow>()
                             .Select(r => (string)r["FieldName"].ToString())
                             .ToList();

                    TableClass tableClass = tableNameLists.Where(d => d.TableName == tableName).FirstOrDefault();
                    if (result.Count > 0)
                    {
                        int count = 0;
                        foreach (string fieldName in result)
                        {
                            count += 1;
                            if (count == 1)
                            {
                                if (string.IsNullOrEmpty(tableClass.FieldsDBName)) tableClass.FieldsDBName += fieldName;
                                else tableClass.FieldsDBName += ("," + fieldName);
                            }
                            else tableClass.FieldsDBName += ("," + fieldName);
                        }
                    }



                }

            }

            foreach (TableClass tableClass in tableNameLists)
            {
                if (tableClass.TableName != "WaterMark")
                {
                    if (!tableClass.IsCloseTable)
                    {
                        if (tableClass.TableName != "DWHSetting")
                        {
                            if (tableClass.TableName != "PartnerType" && tableClass.TableName != "Tenant") tableClass.FieldsDBName = "Id,Tenant," + tableClass.FieldsDBName;
                            else tableClass.FieldsDBName = "Id," + tableClass.FieldsDBName;
                        }
                        else tableClass.FieldsDBName = tableClass.FieldsDBName;

                    }
                    else
                    {
                        if (tableClass.KeyName == "Id") tableClass.FieldsDBName = "Id," + tableClass.FieldsDBName;
                    }

                    tableClass.FieldsDBName += ",AutomaticLastUpdateDate";
                }
            }



        }

        public void BuildAndExecuteDataWarehouseScript(string forderName, string connectionString, TableClass table)
        {
            string scriptName =  forderName == "BuildWarehouse" ? table.BuildScriptName : table.IncrementalScriptName;

            string path = System.IO.Path.GetDirectoryName(new System.Uri(System.Reflection.Assembly.GetExecutingAssembly().CodeBase).LocalPath);
            if (AppName != "Service")
            {
                if (path.Contains(@"\bin\" + Mode)) path = path.Replace(@"\bin\" + Mode, string.Empty);
            }

            string fileDirectory = Path.Combine(path, "WarehouseScript\\" + forderName, scriptName + ".sql");
            FileInfo file = new FileInfo(fileDirectory);
            string cmd = file.OpenText().ReadToEnd();

            if (forderName == "BuildWarehouse" && table != null)
            {
                cmd = (CreateDimensionsFactTable(connectionString, table) + cmd);
                cmd += (" " + TransferTempDataToNewDimensionsFactTable(connectionString, table));
                if (table.HasFactTable && table.TableName == "Shipment")
                {
                    cmd += (" " + RenameALLTables());
                    cmd += (" " + AddRelationsBetweenFactAndDim(table));
                    cmd += AddIndexs(table);
                }
            }

            if (table.HasFactTable && table.TableName == "Shipment") cmd = BuildCustomFields(cmd);
      
            ExecuteSql(cmd, connectionString);

        }

        private string AddIndexs(TableClass table)
        {
            string sql = string.Empty;
            if (!string.IsNullOrEmpty(table.FieldIndexes))
            {
                string[] fieldNames = table.FieldIndexes.Split(',');
                if (fieldNames.Length > 0)
                {
                    foreach (string fieldName in fieldNames)
                    {
                        sql += (" CREATE NONCLUSTERED INDEX [IX_" + table.DWObjectTableCode + "_" + fieldName + "] ON [dbo].[" + table.DWObjectTableCode + "]([" + fieldName + "]) \r\n ");
                    }
                }

            }
            return sql;
        }

        private string BuildCustomFields(string sql)
        {
            if (!string.IsNullOrEmpty(sql))
            {
                sql = ResolveDeclareCustomFieldsVariable(sql);
                sql = ResolveCustomFieldDataTypeCodeVariable(sql);
                sql = ResolveCustomFieldNamesVariable(sql);
                sql = ResolveCustomFieldValuesVariable(sql);
                sql = ResolveShipmentsCustomFieldsVariable(sql);
                sql = ResolveCursorCustomFieldsVariable(sql);

            }

            return sql;

        }

        private string ResolveCustomFieldDataTypeCodeVariable(string sql)
        {
            int i = 1;
            string result = string.Empty;

            if (sql.Contains("--@[ResolveCustomFieldDataTypeCodeVariable]"))
            {
                result = string.Empty;
                while (i <= CustomFieldsCount)
                {
                    result += "   set @Field" + i + "DataTypeCode =( select DataTypeCode from #TempObjectFields where FieldName = 'Field" + i + "' and Tenant =@SourceTenant ) \r\n";
                    i += 1;
                }

                sql = sql.Replace("--@[ResolveCustomFieldDataTypeCodeVariable]", result);
            }
            return sql;
        }


        private string ResolveCursorCustomFieldsVariable(string sql)
        {
            int i = 1;
            string result = string.Empty;
            if (sql.Contains("@CursorCustomFieldsVariable"))
            {
                i = 1;
                result = string.Empty;
                while (i <= CustomFieldsCount)
                {
                    result += "@Field" + i + ",";
                    i += 1;
                }
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
                while (i <= CustomFieldsCount)
                {
                    result += "dw_Shipments.Field" + i + ",";
                    i += 1;
                }
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
                while (i <= CustomFieldsCount)
                {
                    result += "dbo.ResolveCustomFieldValue(@Field" + i + ",@Field" + i + "DataTypeCode)" + (i < CustomFieldsCount ? "," : ""); ;
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
                while (i <= CustomFieldsCount)
                {
                    result += "[Field" + i + "]" + (i < CustomFieldsCount ? "," : "");
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
                while (i <= CustomFieldsCount)
                {
                    result += "   declare @Field" + i + " as varchar(2000) \r\n";
                    result += "   declare @Field" + i + "DataTypeCode as varchar(100) \r\n";
                    i += 1;
                }
                //DataTypeCode
                sql = sql.Replace("--@[DeclareCustomFieldsVariable]", result);
            }

            return sql;
        }

        public void ExecuteScript(string forderName, string connectionString, string scriptName )
        {
            string path = System.IO.Path.GetDirectoryName(new System.Uri(System.Reflection.Assembly.GetExecutingAssembly().CodeBase).LocalPath);
            if (AppName != "Service")
            {
                if (path.Contains(@"\bin\" + Mode)) path = path.Replace(@"\bin\" + Mode, string.Empty);
            }

            string fileDirectory = Path.Combine(path, "WarehouseScript\\" + forderName, scriptName + ".sql");
            FileInfo file = new FileInfo(fileDirectory);
            string cmd = file.OpenText().ReadToEnd();

            ExecuteSql(cmd, connectionString);

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


        private string CreateDimensionsFactTable(string connectionString, TableClass table)
        {
            string sql = "If(OBJECT_ID('tempdb..#" + table.DWObjectTableCode + "Temp') Is Not Null) Begin  Drop Table #" + table.DWObjectTableCode + "Temp End \r\n";
            sql += " CREATE TABLE #" + table.DWObjectTableCode + "Temp ( \r\n";
            string insertNullValue = "insert into #" + table.DWObjectTableCode + "Temp (";
            foreach (DWObjectFieldDB field in table.DWObjectFieldDBLists)
            {
                sql += field.FieldName + " ";

                if (!table.HasFactTable)
                {
                    if (field.FieldName != "[Id_Number]") insertNullValue += field.FieldName + ",";
                }

                if (field.DataTypeCode == "Text" || field.DataTypeCode == "nText")
                {
                    if (field.DataTypeCode == "nText") sql += "n";
                    sql += "varchar(" + field.MaxLength + ")";
                }
                else if (field.DataTypeCode == "Boolean") sql += " bit";
                else if (field.DataTypeCode == "Decimal" || field.DataTypeCode == "Double") sql += " float";
                else if (field.DataTypeCode == "Integer") sql += " int";
                else if (field.DataTypeCode == "DateTime") sql += " dateTime";
                else if (field.DataTypeCode == "Date") sql += " date";
                else if (field.DataTypeCode == "SqlVariant") sql += " sql_variant";
                if (field.IsRequired) sql += " not null";

                if (field.IsPrimaryKey)
                {
                    if (field.FieldName == "[Id_Number]") sql += " identity(1, 1)";
                    sql += "  primary key";
                }

                sql += ",\r\n";
            }

            if (!table.HasFactTable)
            {
                insertNullValue += ")";
                insertNullValue = insertNullValue.Replace(",)", ")") + " values(";
            }
            if (!table.HasFactTable)
            {
                foreach (DWObjectFieldDB field in table.DWObjectFieldDBLists)
                {
                    if (field.FieldName != "[Id_Number]")
                    {
                        string values = null;
                        if (field.DataTypeCode == "Text" || field.DataTypeCode == "nText")
                        {
                            if (field.FieldName == "[Id]" || (field.FieldName == "[Code]" && table.TableName != "Card")) values = field.MaxLength > 1 ? "'-1'" : "'1'";
                            else values = "'Not Specified'";

                            if (field.MaxLength + 2 < values.Length) values = "null";
                        }
                        else if (field.DataTypeCode == "Boolean" || field.DataTypeCode == "Decimal" || field.DataTypeCode == "Integer")
                        {
                            values = field.FieldName == "[Tenant Number]" ? "-1" : "0";
                        }

                         insertNullValue += (values + ",");
                    }
                }
            }

            if (!table.HasFactTable)
            {
                insertNullValue += ")";
                insertNullValue = insertNullValue.Replace(",)", ")");
            }

            if (table.DBTableName == "ShipmentTypes")
            {
                var x = insertNullValue.Replace("-1", "Air").Replace("Not Specified", "Air") + ";";
                insertNullValue += x;
            }

            sql += ");";
            sql = sql.Replace(",);", ");");
            if (!table.HasFactTable) sql += (" " + insertNullValue);

            return sql;


        }

        private string TransferTempDataToNewDimensionsFactTable(string connectionString, TableClass table)
        {
            string sql = "If OBJECT_ID('New" + table.DWObjectTableCode + "','U')  IS NOT NULL Begin  Drop Table New" + table.DWObjectTableCode + " End \r\n";
            sql += (" SELECT *  INTO New" + table.DWObjectTableCode + " FROM #" + table.DWObjectTableCode + "Temp \r\n");
            sql += (" If(OBJECT_ID('tempdb..#" + table.DWObjectTableCode + "Temp') Is Not Null) Begin  Drop Table #" + table.DWObjectTableCode + "Temp End \r\n\r\n");

            if (!table.HasFactTable)
            {
                var field = table.DWObjectFieldDBLists.Where(d => d.IsPrimaryKey).FirstOrDefault();
                if (field != null)
                {
                    string keyName = field.FieldName.Replace("[", "").Replace("]", "");
                    sql += (" ALTER TABLE New" + table.DWObjectTableCode + " ADD CONSTRAINT PK_New" + table.DWObjectTableCode + "_" + keyName.Replace(" ", "") + " PRIMARY KEY CLUSTERED([" + keyName + "]) \r\n");

                }

                var secondField = table.DWObjectFieldDBLists.Where(d => d.FieldName == "[Id]").FirstOrDefault();
                if (secondField == null) secondField = table.DWObjectFieldDBLists.Where(d => d.FieldName == "[Code]").FirstOrDefault();
                if (secondField != null)
                {
                    string secondKeyName = secondField.FieldName.Replace("[", "").Replace("]", "");
                    sql += (" CREATE NONCLUSTERED INDEX [IX_" + table.DWObjectTableCode + "_" + secondKeyName.Replace(" ", "") + "] ON [dbo].[New" + table.DWObjectTableCode + "]([" + secondKeyName + "]) \r\n");
                }



            }


            return sql;


        }


        private string RenameTable(TableClass table)
        {
            string sql = "IF OBJECT_ID('" + table.DWObjectTableCode + "', 'U')  IS NOT NULL and OBJECT_ID('New" + table.DWObjectTableCode + "', 'U')  IS NOT NULL begin EXEC sp_rename '" + table.DWObjectTableCode + "', 'Old" + table.DWObjectTableCode + "' end \r\n";
            sql += "IF OBJECT_ID('New" + table.DWObjectTableCode + "', 'U')  IS NOT NULL begin EXEC sp_rename 'New" + table.DWObjectTableCode + "', '" + table.DWObjectTableCode + "' end \r\n";
            sql += "IF OBJECT_ID('Old" + table.DWObjectTableCode + "', 'U')  IS NOT NULL begin drop table Old" + table.DWObjectTableCode + " end \r\n";

            if (table.HasFactTable) sql += "\r\n";

            return sql;
        }
        private string RenameALLTables()
        {
            string sql = RenameTable(tableLists.Where(d => d.HasFactTable).FirstOrDefault());


            foreach (TableClass table in tableLists.Where(d => d.HasDimensionTable))
            {
                sql += RenameTable(table);

                if (!table.HasFactTable)
                {
                    string fieldName = GetPrimaryKey(table);
                    if (!string.IsNullOrWhiteSpace(fieldName))
                    {
                        sql += "IF  EXISTS (SELECT * FROM sys.key_constraints WHERE [type] = 'PK' and   [parent_object_id] = Object_id('dbo." + table.DWObjectTableCode + "') and name = 'PK_New" + table.DWObjectTableCode + "_" + fieldName.Replace(" ", "") + "')  begin ALTER TABLE " + table.DWObjectTableCode + " DROP CONSTRAINT PK_New" + table.DWObjectTableCode + "_" + fieldName.Replace(" ", "") + " end \r\n";
                        sql += ("ALTER TABLE " + table.DWObjectTableCode + " ADD CONSTRAINT PK_" + table.DWObjectTableCode + "_" + fieldName.Replace(" ", "") + " PRIMARY KEY CLUSTERED([" + fieldName + "]) \r\n\r\n");

                    }



                }
            }

            return sql;

        }

        private string AddRelationsBetweenFactAndDim(TableClass table)
        {
            string sql = "IF OBJECT_ID ('" + table.DWObjectTableCode + "', 'U')  IS NOT NULL \r\n begin \r\n";
            string primaryfield = GetPrimaryKey(table);
            if (!string.IsNullOrWhiteSpace(primaryfield))
            {
                sql += (" ALTER TABLE " + table.DWObjectTableCode + " ADD CONSTRAINT PK_" + table.DWObjectTableCode + "_" + primaryfield.Replace(" ", "") + " PRIMARY KEY CLUSTERED([" + primaryfield + "]) \r\n");
            }

            foreach (DWObjectFieldDB objectFieldDB in table.DWObjectFieldDBLists.Where(d => !string.IsNullOrWhiteSpace(d.DimensionTableCode) &&  d.DimensionTableCode!= "DIM_Dates"))
            {
                string field = objectFieldDB.FieldName.Replace("[", "").Replace("]", "");
                TableClass orginalTable = tableLists.Where(d => d.DWObjectTableCode == objectFieldDB.DimensionTableCode).FirstOrDefault();
                if (orginalTable != null)
                {
                    string orginalfield = GetPrimaryKey(orginalTable);
                    sql += (" ALTER TABLE " + table.DWObjectTableCode + " ADD CONSTRAINT FK_" + table.DWObjectTableCode + "_" + objectFieldDB.DimensionTableCode + "_" + field.Replace(" ", "") + " FOREIGN KEY ([" + field + "]) REFERENCES " + objectFieldDB.DimensionTableCode + "([" + orginalfield + "])\r\n");
                }
                else
                {
                    if (objectFieldDB.DimensionTableCode == "DIM_Dates")
                    {
                        sql += (" ALTER TABLE " + table.DWObjectTableCode + " ADD CONSTRAINT FK_" + table.DWObjectTableCode + "_DIM_Dates_" + field.Replace(" ", "") + " FOREIGN KEY ([" + field + "]) REFERENCES " + objectFieldDB.DimensionTableCode + "([Date Key])\r\n");
                    }
                }

            }

            sql += (" CREATE NONCLUSTERED INDEX [IX_" + table.DWObjectTableCode + "_Id" + "] ON [dbo].[" + table.DWObjectTableCode + "]([Id]) \r\n end");



            return sql;
        }


        private string GetPrimaryKey(TableClass table)
        {
            string result = "";
            var field = table.DWObjectFieldDBLists.Where(d => d.IsPrimaryKey).FirstOrDefault();
            if (field != null)
            {
                result = field.FieldName.Replace("[", "").Replace("]", "");
            }

            return result;
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


        public int GetCount(string tableName, string connectionString)
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

        delegate void SetControlValueCallback(Control oControl, string propName, object propValue);
        public void SetControlPropertyValue(Control oControl, string propName, object propValue)
        {
            if (oControl.InvokeRequired)
            {
                SetControlValueCallback d = new SetControlValueCallback(SetControlPropertyValue);
                oControl.Invoke(d, new object[] { oControl, propName, propValue });
            }
            else
            {
                Type t = oControl.GetType();
                PropertyInfo[] props = t.GetProperties();
                foreach (PropertyInfo p in props)
                {
                    if (p.Name.ToUpper() == propName.ToUpper())
                    {
                        p.SetValue(oControl, propValue, null);
                    }
                }
            }
        }


        #endregion

        #region Build Data Base


        public void BuildDataBase(string sourceConnectionString, string destinationConnectionString, int? privateTenant = null, string relatedTenants = null)
        {
            bool isPrivateDB = privateTenant != null ? true : false;

            if (!isPrivateDB) CreateWaterMarksTable("WaterMarks", sourceConnectionString);

            List<TableClass> tableNameLists = FillTable();



            BuildWarehouseObjectField(tableNameLists, sourceConnectionString);
            BuildDWObjectFieldDB(tableNameLists, sourceConnectionString);
            foreach (TableClass table in tableNameLists)
            {
                if (table.TableName != "WaterMark")
                {
                    InitializationDWTable(table, sourceConnectionString, destinationConnectionString);
                    CreateIndex(table, table.KeyName, destinationConnectionString);
                    CreateIndex(table, "AutomaticLastUpdateDate", destinationConnectionString);
                    if (table.HasConstraint) AddConstraint(table, destinationConnectionString);
                    if (table.HasNotSpecifiedValue) InSertNotSpecifiedValueToDW(table, destinationConnectionString, privateTenant);
                }
                else
                {
                    if (isPrivateDB) CreateWaterMarksTable("dw_WaterMarks", destinationConnectionString);
                    else
                    {
                        InitializationDWTable(table, sourceConnectionString, destinationConnectionString);
                        CreateIndex(table, table.KeyName, destinationConnectionString);
                        CreateIndex(table, "LastUpdateDate", destinationConnectionString);
                    }
                }


                CopyDataBase(null, table, sourceConnectionString, destinationConnectionString, privateTenant, relatedTenants);
                UpdateAutomaticLastUpdate(table, sourceConnectionString, destinationConnectionString, privateTenant);
            }

            ExecuteScript("BuildWarehouse", destinationConnectionString, "BuildDateDimensionsTable");
            RunSqlFunctions(destinationConnectionString);

            foreach (TableClass table in tableNameLists.Where(d => d.HasDimensionTable).ToList())
            {
                BuildAndExecuteDataWarehouseScript("BuildWarehouse", destinationConnectionString, table);
            }

            foreach (TableClass table in tableNameLists.Where(d => d.HasFactTable).ToList())
            {
                BuildAndExecuteDataWarehouseScript("BuildWarehouse", destinationConnectionString, table);
            }

        }

        Control Control = null;
        TableClass Table = null;




        public void CopyDataBase(Control control, TableClass table, string sourceConnectionString, string destinationConnectionString, int? privateTenant = null, string relatedTenants = null)
        {
            Control = control;
            Table = table;

            bool isPrivateDB = privateTenant != null ? true : false;

            using (SqlConnection sourceConnection =
                       new SqlConnection(sourceConnectionString))
            {
                sourceConnection.Open();

                string fieldName = !string.IsNullOrEmpty(table.FieldsDBName) ? table.FieldsDBName : "*";
                string condition = string.Empty;

                if (isPrivateDB)
                {
                    if (table.TableName == "WaterMark") condition = " where PrivateTenant = " + privateTenant;
                    if (!table.IsCloseTable && table.FieldsDBName.Contains("Tenant")) condition = " where Tenant in " + relatedTenants;
                    else if (table.DBTableName == "Tenants") condition = " where Id in " + relatedTenants;
                }

                if (table.TableName == "ObjectField")
                {
                    condition += !isPrivateDB ? " where " : " and";
                    condition += " IsCustom = 1 and ObjectTableId =(select id from ObjectTables where Name = 'Shipment')";

                }

                string tableName = table.DBTableName;
                if (table.TableName == "WaterMark" && isPrivateDB) tableName = "Private" + tableName;



                SqlCommand commandSourceData = new SqlCommand(
           "SELECT " + fieldName +
           " FROM dbo." + tableName + condition + " ;", sourceConnection);



                SqlDataReader reader = commandSourceData.ExecuteReader();

                using (SqlConnection destinationConnection =
                           new SqlConnection(destinationConnectionString))
                {
                    destinationConnection.Open();

                    using (SqlBulkCopy bulkCopy =
                               new SqlBulkCopy(destinationConnection))
                    {
                        bulkCopy.DestinationTableName =
                            "dbo." + table.Dw_TableName;

                        bulkCopy.BulkCopyTimeout = (int)this.timeOut;

                        try
                        {

                            bulkCopy.EnableStreaming = true;
                            bulkCopy.BatchSize = 100000;
                            if (Control != null && Table != null)
                            {
                                bulkCopy.NotifyAfter = 100000;
                                bulkCopy.SqlRowsCopied += new SqlRowsCopiedEventHandler(OnSqlRowsCopied);
                            }

                            bulkCopy.WriteToServer(reader);

                        }

                        finally
                        {
                            reader.Close();


                        }
                    }

                }
            }


        }


        private void OnSqlRowsCopied(
        object sender, SqlRowsCopiedEventArgs e)
        {
            if (Control != null && Table != null)
            {
                SetControlPropertyValue(Control, "Text", Table.DBTableName + "  " + e.RowsCopied.ToString());
            }

        }

        public string CreateTABLE(string tableName, DataTable table)
        {

            string sqlsc;
            sqlsc = "CREATE TABLE " + tableName + "(";
            for (int i = 0; i < table.Columns.Count; i++)
            {
                sqlsc += "\n [" + table.Columns[i].ColumnName + "] ";
                string columnType = table.Columns[i].DataType.ToString();
                switch (columnType)
                {
                    case "System.Int32":
                        sqlsc += " int ";
                        break;
                    case "System.Int64":
                        sqlsc += " bigint ";
                        break;
                    case "System.Int16":
                        sqlsc += " smallint";
                        break;
                    case "System.Byte":
                        sqlsc += " tinyint";
                        break;
                    case "System.Decimal":
                        sqlsc += " decimal ";
                        break;
                    case "System.DateTime":
                        sqlsc += " datetime ";
                        break;

                    case "System.Boolean":
                        sqlsc += " bit ";
                        break;

                    case "System.Double":
                        sqlsc += " float ";
                        break;
                    case "System.String":
                        sqlsc += string.Format(" varchar({0}) ", table.Columns[i].MaxLength == -1 ? "max" : table.Columns[i].MaxLength.ToString());
                        break;

                    default:
                        sqlsc += string.Format(" varchar({0}) ", table.Columns[i].MaxLength == -1 ? "max" : table.Columns[i].MaxLength.ToString());
                        break;
                }
                if (table.Columns[i].AutoIncrement)
                {
                    sqlsc += " IDENTITY(" + table.Columns[i].AutoIncrementSeed.ToString() + "," + table.Columns[i].AutoIncrementStep.ToString() + ") ";
                }
                if (!table.Columns[i].AllowDBNull)
                    sqlsc += " NOT NULL ";
                sqlsc += ",";
            }
            return sqlsc.Substring(0, sqlsc.Length - 1) + "\n)";
        }

        public void UpdateAutomaticLastUpdate(TableClass table, string sourceConnectionString, string destinationConnectionString, int? tenant = null)
        {
            if (table.DBTableName != "WaterMarks")
            {
                string lastUpdateDate = this.GetAutomaticLastUpdateDate(table.Dw_TableName, destinationConnectionString);
                if (string.IsNullOrEmpty(lastUpdateDate)) lastUpdateDate = DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff tt");
                this.AddWareMarkRecord(table, lastUpdateDate, sourceConnectionString, tenant);
            }

        }

        public void InitializationDWTable(TableClass table, string sourceConnectionString, string destinationConnectionString)
        {
            ExecuteSql("IF OBJECT_ID ('" + table.Dw_TableName + "', 'U')  IS NOT NULL drop table " + table.Dw_TableName, destinationConnectionString);
     
            var dwObjectTable = new DataTable();
            using (SqlConnection sourceConnection = new SqlConnection(sourceConnectionString))
            {
                sourceConnection.Open();
                string fieldName = !string.IsNullOrEmpty(table.FieldsDBName) ? table.FieldsDBName : "*";
                SqlCommand commandSourceData = new SqlCommand("select top(1) " + fieldName + " from " + " dbo." + table.DBTableName, sourceConnection);

                SqlDataReader reader = commandSourceData.ExecuteReader();

                dwObjectTable.Load(reader);
                reader.Close();

            }
            ExecuteSql(CreateTABLE(table.Dw_TableName, dwObjectTable), destinationConnectionString);
        }

        public void CreateIndex(TableClass table, string fieldName, string connectionString)
        {
            string cmd = "CREATE NONCLUSTERED INDEX [IX_" + table.Dw_TableName + "_" + fieldName + "] ON[dbo].[" + table.Dw_TableName + "]([" + fieldName + "])";

           if (table.DBTableName == "Shipments" && fieldName == "Id")
            {
                string customFieldindex = "";
                int i = 1;
                while (i <= 6)
                {
                    customFieldindex += "[Field" + i + "],";
                    i += 1;
                }

                cmd += "; CREATE NONCLUSTERED INDEX[dw_Shipments_AllColumnsIndexes]ON[dbo].[dw_Shipments]([AutomaticLastUpdateDate])INCLUDE([Id],[Tenant],[ShipmentNumber],[House],[BranchId],[IncotermId],[SalesmanUserId],[DepartmentId],[ShipmentTypeId],[ShipperId],[ConsigneeId],[TransportModeId],[DirectionId],[AgentId],[IsOperationalClosed],[ChargeableWeightInKG],[GrossWeightInKG],[VolumeInCBM],[NumberOfContainers],[NumberOfPackages],[StatusId],[IsAccountingClosed],[AccountedReceivablesInLocalCurrency],[ProfitInLocalCurrency],[CustomerId],[ProfitCurrencyId],[ProfitInProfitCurrency],[AccountedReceivablesInProfitCurrency],[MasterShipmentDataId],[FromPortId],[ToPortId],[ShipmentLevelCode],[AccountedPayablesInLocalCurrency],[AccountedPayablesInProfitCurrency],[FinalArrivalDate],[AccountManagerUserId],[StatusLocation],[CustomsClearanceDate],[ForwarderPartnerId] ,[CustomAgentExportId],[CustomAgentImportId],[ValueOfGoodsCurrencyId],[WarehouseLegWarehouseId] " +
                    ", [IsCancelled] , [StatusDate] , [CustomsDeclarationNumber] ,[FirstOperationalCloseDate] , [EstimatedFinalArrivalDate] , [ActualFinalArrivalDate],[Routing],[DescriptionOfGoods],[PreCarriageETD],[MoveTypeId], "  + customFieldindex + "[SpecialServicesTypeId])";
            }
            ExecuteSql(cmd, connectionString);
        }

        public void AddConstraint(TableClass table, string connectionString)
        {
            string cmd = string.Empty;
            switch (table.DBTableName)
            {
                case "Shipments":

                    cmd = " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "ShipperId DEFAULT '-1' FOR ShipperId"
                        + " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "ConsigneeId DEFAULT '-1' FOR ConsigneeId;"
                        + " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "AgentId DEFAULT '-1' FOR AgentId;"
                        + " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "CustomerId DEFAULT '-1' FOR CustomerId;"
                        + " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "IncotermId DEFAULT '-1' FOR IncotermId;"
                        + " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "SalesmanUserId DEFAULT '-1' FOR SalesmanUserId;"
                        + " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "AccountManagerUserId DEFAULT '-1' FOR AccountManagerUserId;"
                        + " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "FromPortId DEFAULT '-1' FOR FromPortId;"
                        + " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "ToPortId DEFAULT '-1' FOR ToPortId;"
                        + " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "MasterShipmentDataId DEFAULT '-1' FOR MasterShipmentDataId;"
                        + " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "ShipmentTypeId DEFAULT '-1' FOR ShipmentTypeId;"
                        + " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "DepartmentId DEFAULT '-1' FOR DepartmentId;"
                        + " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "BranchId DEFAULT '-1' FOR BranchId;"
                        + " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "ProfitCurrencyId DEFAULT '-1' FOR ProfitCurrencyId;"
                        + " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "StatusId DEFAULT '-1' FOR StatusId;"
                        + " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "DirectionId DEFAULT '1' FOR DirectionId;"
                        + " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "TransportModeId DEFAULT '1' FOR TransportModeId;"
                        + " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "ShipmentLevelCode DEFAULT '1' FOR ShipmentLevelCode;"
                        + " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "ForwarderPartnerId DEFAULT '-1' FOR ForwarderPartnerId;"
                        + " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "CustomAgentImportId DEFAULT '-1' FOR CustomAgentImportId;"
                        + " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "CustomAgentExportId DEFAULT '-1' FOR CustomAgentExportId;"
                        + " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "WarehouseLegWarehouseId DEFAULT '-1' FOR WarehouseLegWarehouseId"
                        + " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "CreatedByUserId DEFAULT '-1' FOR CreatedByUserId"
                        + " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "ValueOfGoodsCurrencyId DEFAULT '-1' FOR ValueOfGoodsCurrencyId"
                        + " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "MoveTypeId DEFAULT '-1' FOR MoveTypeId"
                        + " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "SpecialServicesTypeId DEFAULT '-1' FOR SpecialServicesTypeId"
                        +" ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "AgentComputed DEFAULT '-1' FOR AgentComputed";
                    

                    break;
                case "Cards":

                    cmd = " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "SalesmanUserId DEFAULT '-1' FOR SalesmanUserId"
                        + " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "PrimaryContactId DEFAULT '-1' FOR PrimaryContactId;"
                       + " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "CountryId DEFAULT '-1' FOR CountryId;"
                        + " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "PartnerTypeId DEFAULT '-1' FOR PartnerTypeId;";
                    break;
                case "Tenants":

                    cmd = " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "AddressId DEFAULT '-1' FOR AddressId"
                        + " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "CurrencyId DEFAULT '-1' FOR CurrencyId;";

                    break;
                case "Ports":
                case "Addresses":

                    cmd = " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "StateId DEFAULT '-1' FOR StateId"
                          + " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "CountryId DEFAULT '-1' FOR CountryId;";

                    break;
                case "ShipmentMasterDatas":

                    cmd = " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "MainCarriageToPortId DEFAULT '-1' FOR MainCarriageToPortId"
                    + " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "Transshipment1ToPortId DEFAULT '-1' FOR Transshipment1ToPortId;"
                    + " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "Transshipment2ToPortId DEFAULT '-1' FOR Transshipment2ToPortId;"
                    + " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "Transshipment3ToPortId DEFAULT '-1' FOR Transshipment3ToPortId;"
                    + " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "MainCarriageCarrierId DEFAULT '-1' FOR MainCarriageCarrierId;"
                    + " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "MainCarriageVesselId DEFAULT '-1' FOR MainCarriageVesselId;";

                    break;

                case "DWHSettings":

                    cmd = " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "ParentTenant DEFAULT -1 FOR ParentTenant";

                    break;

                default:
                    cmd = string.Empty;
                    break;
            }

            ExecuteSql(cmd, connectionString);

        }

        public void InSertNotSpecifiedValueToDW(TableClass table, string connectionString, int? privateTenant = null)
        {
            int tenant = privateTenant != null ? (int)privateTenant : 0;

            string cmd = "";

            if (table.DBTableName == "States")
            {
                cmd = "INSERT INTO " + table.Dw_TableName + " (Id,Tenant,EnglishName,AutomaticLastUpdateDate)values('-1'," + tenant + ",'' , GETDATE());";
            }
            else if (table.DBTableName == "Countries")
            {
                cmd = "INSERT INTO " + table.Dw_TableName + " (Id,Tenant,EnglishName,Code,AutomaticLastUpdateDate)values('-1'," + tenant + ",'' ,'', GETDATE());";
            }
            else if (table.DBTableName == "Addresses") cmd = "INSERT INTO " + table.Dw_TableName + " (Id,Tenant,CountryId,StateId ,AutomaticLastUpdateDate )values('-1'," + tenant + ",'-1' ,'-1' , GETDATE());";

            else if (table.DBTableName == "Contacts")
            {
                cmd = "INSERT INTO " + table.Dw_TableName + " (Id,Tenant,EnglishName,LocalName, Email,AutomaticLastUpdateDate)values('-1'," + tenant + ",'','','' , GETDATE());";
            }

            else if (table.DBTableName == "ShipmentTypes" || table.DBTableName == "PartnerTypes") cmd = "INSERT INTO " + table.Dw_TableName + " (Id,Name, AutomaticLastUpdateDate)values('-1','' ,GETDATE());";
            else if (table.DBTableName == "ShipmentMasterDatas")
            {
                cmd = "INSERT INTO " + table.Dw_TableName + " (Id,Tenant,Master,MainCarriageATD, MainCarriageToPortId,Transshipment1ToPortId, Transshipment2ToPortId , Transshipment3ToPortId,AutomaticLastUpdateDate)values('-1'," + tenant + ",'',null,'-1' , '-1','-1','-1', GETDATE())";
            }
            else if (table.DBTableName == "DWHSettings")
            {
                cmd = "INSERT INTO " + table.Dw_TableName + " (Tenant,ParentTenant, AutomaticLastUpdateDate)values(-1 ,-1, GETDATE());";
            }

            ExecuteSql(cmd, connectionString);



        }

        #region WaterMark
        public void CreateWaterMarksTable(string tableName, string connectionString)
        {
            string cmd = "IF OBJECT_ID ('" + tableName + "', 'U')  IS NOT NULL drop table " + tableName + " ; CREATE TABLE " + tableName + " (TableName varchar(100) not null,LastUpdateDate  datetime,);CREATE NONCLUSTERED INDEX [IX_" + tableName + "_TableName] ON[dbo].[" + tableName + "]([TableName])";
            cmd += ";CREATE NONCLUSTERED INDEX [IX_" + tableName + "_LastUpdateDate] ON[dbo].[" + tableName + "]([LastUpdateDate])";
            ExecuteSql(cmd, connectionString);

        }

        public void CreatePrivateWaterMarksTable(string connectionString)
        {
            string cmd = "IF OBJECT_ID ('PrivateWaterMarks', 'U')  IS NOT NULL drop table PrivateWaterMarks ; CREATE TABLE PrivateWaterMarks (TableName varchar(100) not null, LastUpdateDate  datetime, PrivateTenant  int);CREATE NONCLUSTERED INDEX [IX_PrivateWaterMarks_TableName] ON[dbo].[PrivateWaterMarks]([TableName])";
            cmd += ";CREATE NONCLUSTERED INDEX [IX_PrivateWaterMarks_LastUpdateDate] ON[dbo].[PrivateWaterMarks]([LastUpdateDate])";
            cmd += ";CREATE NONCLUSTERED INDEX [IX_PrivateWaterMarks_PrivateTenant] ON[dbo].[PrivateWaterMarks]([PrivateTenant])";

            ExecuteSql(cmd, connectionString);
        }

        public void AddWareMarkRecord(TableClass table, string date, string connectionString, int? tenant)
        {
            string cmd = tenant == null ? "insert into WaterMarks  values('" + table.TableName + "' , '" + date + "')" : "insert into PrivateWaterMarks  values('" + table.TableName + "' , '" + date + "' ," + tenant + " )";
            ExecuteSql(cmd, connectionString);

        }

        public string GetAutomaticLastUpdateDate(string tableName, string connectionString)
        {
            string result = null;

            SqlConnection con = new SqlConnection(connectionString);

            SqlCommand com = new SqlCommand(
"select MAX(AutomaticLastUpdateDate) AutomaticLastUpdateDate " +
"FROM dbo." + tableName + " ;", con);

            try
            {
                con.Open();

                using (SqlDataReader reader = com.ExecuteReader())
                {
                    reader.Read();
                    DateTime? datetime = null;
                    var value = reader["AutomaticLastUpdateDate"];
                    if (value != null)
                    {
                        if (!string.IsNullOrEmpty(value.ToString())) {
                            datetime = (DateTime?)(value);
                            if (datetime != null) result = datetime.Value.ToString("MM/dd/yyyy hh:mm:ss.fff tt");
                        }
             
                    }
             
                    
               

                }
            }
            finally
            {
                con.Close();
            }
            return result;
        }
        #endregion



        #endregion
         
        #region Incremental Data Base

        public void UpdateWarehouseData(string sourceConnectionString, string destinationConnectionString, int? privateTenant = null, string relatedTenants = null)
        {
            List<TableClass> tableNameLists = FillTable();
            BuildWarehouseObjectField(tableNameLists, sourceConnectionString);

            foreach (TableClass table in tableNameLists)
            {
                if (table.DBTableName != "WaterMarks")
                {
                    UpdateDWDataBase(table, sourceConnectionString, destinationConnectionString, privateTenant, relatedTenants);

                }
            }

            #region Update Dimensions Table
            foreach (TableClass table in tableNameLists.Where(d => d.HasDimensionTable).ToList())
            {
                BuildAndExecuteDataWarehouseScript("IncrementalWarehouse", destinationConnectionString, table);
            }
            #endregion

            #region Update Fact Table

            foreach (TableClass table in tableNameLists.Where(d => d.HasFactTable).ToList())
            {
                RemoveDataFromFactShipment(table, destinationConnectionString);
                BuildAndExecuteDataWarehouseScript("IncrementalWarehouse", destinationConnectionString, table);
            }

            #endregion

        }

        public void UpdateDWDataBase(TableClass table, string sourceConnectionString, string destinationConnectionString, int? privateTenant = null, string relatedTenants = null)
        {
            string fieldName = !string.IsNullOrEmpty(table.FieldsDBName) ? table.FieldsDBName : "*";
            bool isPrivateDB = privateTenant != null ? true : false;
            string condition = " where AutomaticLastUpdateDate > ( select LastUpdateDate from WaterMarks where TableName = " + "'" + table.TableName + "')";

            if (isPrivateDB)
            {
                condition = " where AutomaticLastUpdateDate > ( select LastUpdateDate from PrivateWaterMarks where TableName = " + "'" + table.TableName + "'" + " and PrivateTenant = " + privateTenant + ") ";
                if (!table.IsCloseTable && table.FieldsDBName.Contains("Tenant")) condition += " and Tenant in " + relatedTenants;
                else if (table.DBTableName == "Tenants") condition += " and Id in " + relatedTenants;
            }

            if (table.TableName == "ObjectField")
            {
                condition += "and IsCustom = 1 and ObjectTableId =(select id from ObjectTables where Name = 'Shipment')";

            }

            using (SqlConnection sourceConnection =
                       new SqlConnection(sourceConnectionString))
            {
                sourceConnection.Open();

                SqlCommand commandSourceData = new SqlCommand(
               "SELECT  " + fieldName +
               " FROM dbo." + table.DBTableName + condition, sourceConnection);

                SqlDataReader reader = commandSourceData.ExecuteReader();

                var dataTable = new DataTable();
                dataTable.Load(reader);

                var columns = dataTable.Rows
                                 .Cast<DataRow>()
                                 .Select(r => (string)r[table.KeyName].ToString())
                                 .ToList();

                table.UpdatedCount = columns != null ? columns.Count() : 0;

                string ids = String.Empty;
                foreach (string id in columns)
                {
                    ids += "'" + id + "'" + ",";
                }

                if (!string.IsNullOrEmpty(ids))
                {
                    DateTime automaticLastUpdateDate = (DateTime)dataTable.Rows
                                  .Cast<DataRow>()
                                  .Max(d => d["AutomaticLastUpdateDate"]);


                    if (!string.IsNullOrEmpty(ids))
                    {
                        ids = "(" + ids + ")";
                        ids = ids.Replace(",)", ")");

                    }

                    RemoveDataBase(table, ids, destinationConnectionString);

                    using (SqlConnection destinationConnection =
                               new SqlConnection(destinationConnectionString))
                    {
                        destinationConnection.Open();

                        using (SqlBulkCopy bulkCopy =
                                   new SqlBulkCopy(destinationConnection))
                        {
                            bulkCopy.DestinationTableName =
                                "dbo." + table.Dw_TableName;

                            bulkCopy.BulkCopyTimeout = (int)this.timeOut;

                            try
                            {


                                bulkCopy.EnableStreaming = true;
                                bulkCopy.BatchSize = 100000;
                                bulkCopy.WriteToServer(dataTable);
                            }

                            finally
                            {
                                reader.Close();

                                if (table.DBTableName != "WaterMarks")
                                {
                                    var lastUpdateDate = string.Empty;
                                    if (automaticLastUpdateDate != null) lastUpdateDate = automaticLastUpdateDate.ToString("MM/dd/yyyy hh:mm:ss.fff tt");
                                    else lastUpdateDate = DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff tt");
                                    this.UpdateWareMarkTable(table, lastUpdateDate, sourceConnectionString);

                                    table.IsUpdated = true;
                                    //table.RefreshIds = ids;

                                }

                            }
                        }

                    }



                }

                else
                {
                    reader.Close();
                }


            }



        }

        private void RemoveDataBase(TableClass table, string ids, string connectionString)
        {
            if (!string.IsNullOrEmpty(ids))
            {
                string cmd = "delete " + table.Dw_TableName + " where " + table.KeyName + " in " + ids;
                ExecuteSql(cmd, connectionString);

            }

        }

        public string RemoveDataFromFactShipment(TableClass table, string connectionString)
        {
            string ids = String.Empty;


            string factTableName = "Fact_" + table.DBTableName;

            using (SqlConnection sourceConnection =
                       new SqlConnection(connectionString))
            {
                sourceConnection.Open();

                SqlCommand commandSourceData = new SqlCommand(
               "SELECT " + table.KeyName +
               " FROM dbo." + table.Dw_TableName + " where AutomaticLastUpdateDate > ( select LastUpdateDate from dw_WaterMarks where TableName = " + "'" + table.TableName + "');", sourceConnection);

                SqlDataReader reader =
                    commandSourceData.ExecuteReader();

                while (reader.Read())
                {
                    ids += "'" + ((reader["Id"].ToString())) + "'" + ",";
                }

                if (!string.IsNullOrEmpty(ids))
                {
                    ids = "(" + ids + ")";
                    ids = ids.Replace(",)", ")");

                }

                reader.Close();
            }


            if (!string.IsNullOrEmpty(ids))
            {
                string cmd = "delete " + factTableName + " where " + table.KeyName + " in " + ids;
                ExecuteSql(cmd, connectionString);

            }

            return ids;
        }

        private void UpdateWareMarkTable(TableClass table, string date, string connectionString, int? privateTenant = null)
        {
            string cmd = "update  WaterMarks set LastUpdateDate = '" + date + "' where tableName = '" + table.TableName + "'";
            if (privateTenant != null)
            {
                cmd = cmd.Replace("WaterMarks", "PrivateWaterMarks");
                cmd += (" and PrivateTenant = " + privateTenant);
            }
            ExecuteSql(cmd, connectionString);

        }


        #endregion

        #region Private Tenant
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

        public void BuildOrUpdatePrivateDBData(string dbsourceConnection, string dbDestinationConnection, string type)
        {
            if (!string.IsNullOrEmpty(dbsourceConnection) && !string.IsNullOrEmpty(dbDestinationConnection))
            {
                string[] sourceConnectionArray = dbsourceConnection.Split(',');
                string[] destinationConnectionArray = dbDestinationConnection.Split(',');

                if (sourceConnectionArray.Length != 4 || destinationConnectionArray.Length != 4)
                {
                    if (AppName != "Service")
                    {
                        MessageBox.Show("connection not valid");
                    }
                    return;
                }

                string sourceConnectionString = BuildConnectionString(sourceConnectionArray[0], sourceConnectionArray[1], sourceConnectionArray[2], sourceConnectionArray[3]);


                var dWHSettingsTable = GetPrivateTenant(sourceConnectionString);

                if (type == "Build") CreatePrivateWaterMarksTable(sourceConnectionString);

                foreach (DataRow row in dWHSettingsTable.Rows)
                {
                    int tenant = Int32.Parse(row["Tenant"].ToString());
                    string catalog = row["Catalog"].ToString();
                    string destinationConnectionString = BuildConnectionString(catalog, destinationConnectionArray[1], destinationConnectionArray[2], destinationConnectionArray[3]);
                    List<int> relatedTenants = GetPrivateRelatedTenants(sourceConnectionString, tenant);

                    if (!relatedTenants.Contains(tenant)) relatedTenants.Add(tenant);

                    string tenants = ConvertIntgerListToString(relatedTenants);
                    if (type == "Build") BuildDataBase(sourceConnectionString, destinationConnectionString, tenant, tenants);
                    else UpdateWarehouseData(sourceConnectionString, destinationConnectionString, tenant, tenants);
                }
            }
            else if (AppName != "Service")
            {
                MessageBox.Show("Connection Problem");
            }
        }

        #endregion


    }
}
