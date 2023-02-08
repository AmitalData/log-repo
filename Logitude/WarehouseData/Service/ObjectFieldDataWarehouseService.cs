using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseData.Helper
{
    public class ObjectFieldDataWarehouseService
    {


        public ObjectFieldDataWarehouseService()
        {

        }

        #region ObjectField

        public List<TableClass> BuildWarehouseObjectFieldOnTables(List<TableClass> tableLists, string connectionString)
        {
            List<TableClass> tableNameLists = tableLists;
            var copyToDwObjectFieldLists = GetCopyToDWObjectFields(connectionString);
            var objectTableLists = GetDWObjectTableLists(tableNameLists, connectionString);

            foreach (DataRow row in objectTableLists.Rows)
            {
                string tableName = row["Name"].ToString() == "Master" ? "ShipmentMasterData" : row["Name"].ToString();
                string tableId = row["Id"].ToString();
                TableClass tableClass = tableNameLists.Where(d => d.TableName == tableName && !d.HasFactTable).FirstOrDefault();
                if (tableClass != null) tableClass.ObjectTableId = tableId;

                tableClass.FieldsDBNameLists = (from rowfield in copyToDwObjectFieldLists.AsEnumerable()
                              where rowfield.Field<string>("ObjectTableId") == tableId
                              select rowfield.Field<string>("FieldName")).ToList();

                tableClass.FieldsDBName = GetDWObjectFieldsDBName(tableClass.FieldsDBNameLists, tableClass);
            }

            foreach (TableClass tableClass in tableNameLists.Where(d => !d.HasFactTable))
            {
                var rowLists = copyToDwObjectFieldLists.AsEnumerable().Where(row => row["ObjectTableId"].ToString() == tableClass.ObjectTableId).ToList();
                tableClass.ObjectFieldDBLists = GetDWObjectFieldDBLists(rowLists);

                if (tableClass.TableName != "WaterMark") tableClass.FieldsDBName = tableClass.FieldsDBName + GetAdditionalDWObjectFieldsDBName(tableClass);

            }

            return tableNameLists;
        }

        public List<TableClass> SetCustomObjectFieldMetaData(List<TableClass> dataWarehouseMetaDataTables, string connectionString)
        {
            List<TableClass> dataWarehouseMetaDataTableLists = dataWarehouseMetaDataTables;
            var dwObjectDataTables = new GeneralDataWarehouseService().GetDataTableFromSql(connectionString, "select Code ,HasCustomFields ,MaxNumberOfCustomFields ,ObjectTableName from DWObjectTables where  HasCustomFields = 1");
            foreach (DataRow row in dwObjectDataTables.AsEnumerable())
            {
                int maxNumberOfCustomFields = row["MaxNumberOfCustomFields"] != null && !string.IsNullOrEmpty(row["MaxNumberOfCustomFields"].ToString()) ? int.Parse(row["MaxNumberOfCustomFields"].ToString()) : 0;
                string ObjectTableName = row["ObjectTableName"] != null ? row["ObjectTableName"].ToString() : "";

                TableClass factMetaDataTable = dataWarehouseMetaDataTableLists.Where(d => d.DWObjectTableCode == row["Code"].ToString()).FirstOrDefault();
                if (factMetaDataTable != null)
                {
                    TableClass dwMetaDataTable = dataWarehouseMetaDataTableLists.Where(d => d.TableName == factMetaDataTable.TableName && !d.HasFactTable).FirstOrDefault();
                    factMetaDataTable.HasCustomFields = dwMetaDataTable.HasCustomFields = true;
                    factMetaDataTable.MaxNumberOfCustomFields = dwMetaDataTable.MaxNumberOfCustomFields = maxNumberOfCustomFields;
                    factMetaDataTable.ObjectTableName = dwMetaDataTable.ObjectTableName = ObjectTableName;
                }
            }
            return dataWarehouseMetaDataTableLists;

        }

        private  string GetAdditionalDWObjectFieldsDBName(TableClass tableClass)
        {
            string fieldsDBName = string.Empty;
             if (tableClass.FieldsDBNameLists!=null && !tableClass.FieldsDBNameLists.Contains(tableClass.KeyName)) fieldsDBName = "," + tableClass.KeyName;
             fieldsDBName += (!tableClass.IsCloseTable && tableClass.KeyName != "Tenant" && tableClass.TableName != "Tenant" ? ",Tenant" : "");
             fieldsDBName += ",AutomaticLastUpdateDate";
            fieldsDBName +=  (tableClass.HasCustomFields ? ("," + GetCustomFieldDBNames(tableClass.MaxNumberOfCustomFields) ): "");

            return fieldsDBName;
        }


        private string GetCustomFieldDBNames(int maxNumberOfCustomFields)
        {
            int i = 1;
            string customFieldsDBName = string.Empty;
            while (i <= maxNumberOfCustomFields)
            {
                customFieldsDBName += (i != 1 ? "," : "") + ("Field" + i);
                i += 1;
            }
            return customFieldsDBName;
        }


        private static string GetDWObjectFieldsDBName(List<string> fields, TableClass tableClass)
        {
            StringBuilder dwFieldsDBNameBuilder = new StringBuilder(tableClass.FieldsDBName);
            if (fields.Count > 0)
            {
                dwFieldsDBNameBuilder.Append(!string.IsNullOrEmpty(dwFieldsDBNameBuilder.ToString()) ? "," : "");
                foreach (string fieldName in fields)
                {
                    if (!dwFieldsDBNameBuilder.ToString().Split(',').Contains(fieldName))
                    {
                        dwFieldsDBNameBuilder.Append((fieldName + (fields.Last() != fieldName ? "," : "")));
                    }
                }
                dwFieldsDBNameBuilder.Append("^");
            }
            return dwFieldsDBNameBuilder.ToString().Replace(",^", "").Replace("^", "");
        }

        private DataTable GetCopyToDWObjectFields(string connectionString)
        {
            DataTable objectFieldsTable = new DataTable();
            using (SqlConnection sourceConnection = new SqlConnection(connectionString))
            {
                sourceConnection.Open();
                SqlCommand commandSourceData = new SqlCommand("SELECT  FieldName,ObjectTableId,DataTypeCode from ObjectFields where CopyToDW = 1", sourceConnection);
                SqlDataReader reader = commandSourceData.ExecuteReader();
                objectFieldsTable.Load(reader);
                reader.Close();
                sourceConnection.Close();

            }

            return objectFieldsTable;
        }

        private DataTable GetDWObjectTableLists(List<TableClass> tableNameLists, string connectionString)
        {
            var objectTablesLists = new DataTable();
            using (SqlConnection sourceConnection = new SqlConnection(connectionString))
            {
                sourceConnection.Open();
                string sql = "SELECT  Id,Name from  ObjectTables where Name in " + GetDWTablesNamesAsString(tableNameLists);
                SqlCommand commandSourceData = new SqlCommand(sql, sourceConnection);
                SqlDataReader reader = commandSourceData.ExecuteReader();
                objectTablesLists.Load(reader);
                reader.Close();
                sourceConnection.Close();

            }
            return objectTablesLists;
        }

        private static string GetDWTablesNamesAsString(List<TableClass> tableNameLists)
        {
            StringBuilder tableNamesBuilder = new StringBuilder("(");
            foreach (TableClass table in tableNameLists.Where(d=>!d.HasFactTable))
            {
                string name = table.TableName == "ShipmentMasterData" ? "Master" : table.TableName;
                tableNamesBuilder.Append(("'" + name + (tableNameLists.Last() != table ? "'," : "')")));
            }
            return tableNamesBuilder.ToString();
        }
        #endregion

        #region DW ObjectField

        public List<TableClass> BuildDWObjectFieldDB(List<TableClass> tableLists, string connectionString)
        {
            List<TableClass> tableNameLists = tableLists;
            var dWObjectFieldsMetaData = GetDWObjectFieldsMetaData(connectionString);
            foreach (TableClass tableClass in tableNameLists.Where(d => d.HasDimensionTable || d.HasFactTable).ToList())
            {
                var rowList = dWObjectFieldsMetaData.AsEnumerable().Where(row => row["DWObjectTableCode"].ToString() == tableClass.DWObjectTableCode).ToList();
                tableClass.DWObjectFieldDBLists = GetDWObjectFieldDBLists(rowList);
            }

            MapDimensionFieldsPropertyOnFactTables(tableNameLists);

            return tableNameLists;
        }

        private static void MapDimensionFieldsPropertyOnFactTables(List<TableClass> tableNameLists)
        {
            foreach (TableClass tableClass in tableNameLists.Where(d => d.HasFactTable).ToList())
            {
                if (tableClass.DWObjectFieldDBLists != null)
                {
                    foreach (DWObjectFieldDB field in tableClass.DWObjectFieldDBLists.Where(d => d.DataTypeCode == "Dimension").ToList())
                    {
                        if(field.DimensionTableCode== "DIM_Dates")
                        {
                            field.DataTypeCode = "DateTime";
                        }
                        else if (field.DimensionTableCode == "DIM_InvoiceMainTypes")
                        {
                            field.DataTypeCode = "Text";
                            field.MaxLength = 15;
                        }
                        else 
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
                    }
                }
            }
        }

        private static DataTable GetDWObjectFieldsMetaData(string connectionString)
        {
            var dWObjectFieldsMetaData = new DataTable();
            using (SqlConnection sourceConnection = new SqlConnection(connectionString))
            {
                sourceConnection.Open();
                SqlCommand commandSourceData = new SqlCommand("SELECT  Code, DWObjectTableCode,DataTypeCode,MaxLength,MinLength,IsRequired ,IsPrimaryKey,DimensionTableCode from DWObjectFields", sourceConnection);
                SqlDataReader reader = commandSourceData.ExecuteReader();
                dWObjectFieldsMetaData.Load(reader);
                reader.Close();
                sourceConnection.Close();


            }
            return dWObjectFieldsMetaData;
        }

        private List<DWObjectFieldDB> GetDWObjectFieldDBLists(List<DataRow> rowList)
        {
            var result = new List<DWObjectFieldDB>();
            foreach (DataRow row in rowList)
            {
                var fieldDB = new DWObjectFieldDB();
                fieldDB.FieldName = row.Table.Columns.Contains("Code") ? row["Code"].ToString(): row.Table.Columns.Contains("FieldName") ? row["FieldName"].ToString() : "";
                fieldDB.DataTypeCode = row.Table.Columns.Contains("DataTypeCode") ? row["DataTypeCode"].ToString():"";
                fieldDB.MaxLength = row.Table.Columns.Contains("MaxLength") ?  Int32.Parse(row["MaxLength"].ToString()):100;
                fieldDB.MinLength = row.Table.Columns.Contains("MinLength") ? Int32.Parse(row["MinLength"].ToString()):0;
                fieldDB.IsRequired = row.Table.Columns.Contains("IsRequired") ? bool.Parse(row["IsRequired"].ToString()):false;
                fieldDB.IsPrimaryKey = row.Table.Columns.Contains("IsPrimaryKey") ? bool.Parse(row["IsPrimaryKey"].ToString()):false;
                fieldDB.DimensionTableCode = row.Table.Columns.Contains("DimensionTableCode") ? row["DimensionTableCode"].ToString():"";

                result.Add(fieldDB);
            }

            return result;
        }

        #endregion
    }
}
