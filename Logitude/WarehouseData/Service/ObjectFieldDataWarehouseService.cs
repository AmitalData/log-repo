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

                var fields = (from rowfield in copyToDwObjectFieldLists.AsEnumerable()
                              where rowfield.Field<string>("ObjectTableId") == tableId
                              select rowfield.Field<string>("FieldName")).ToList();

                TableClass tableClass = tableNameLists.Where(d => d.TableName == tableName).FirstOrDefault();
                if (tableClass != null) tableClass.ObjectTableId = tableId;
                tableClass.FieldsDBName = GetDWObjectFieldsDBName(fields, tableClass);
            }

            foreach (TableClass tableClass in tableNameLists)
            {
                if (tableClass.TableName != "WaterMark") tableClass.FieldsDBName = tableClass.FieldsDBName + GetAdditionalDWObjectFieldsDBName(tableClass);

            }

            return tableNameLists;
        }

        private static string GetAdditionalDWObjectFieldsDBName(TableClass tableClass)
        {
            string fieldsDBName = (","+tableClass.KeyName)   + (!tableClass.IsCloseTable && tableClass.KeyName != "Tenant" && tableClass.TableName != "Tenant" ? ",Tenant" : "");
            fieldsDBName += ",AutomaticLastUpdateDate";
            return fieldsDBName;
        }

        private static string GetDWObjectFieldsDBName(List<string> fields, TableClass tableClass)
        {
            StringBuilder dwFieldsDBNameBuilder = new StringBuilder(tableClass.FieldsDBName);
            if (fields.Count > 0)
            {
                dwFieldsDBNameBuilder.Append(!string.IsNullOrEmpty(dwFieldsDBNameBuilder.ToString()) ? "," : "");
                foreach (string fieldName in fields)
                {
                    dwFieldsDBNameBuilder.Append((fieldName + (fields.Last() != fieldName ? "," : "")));

                }
            }
            return dwFieldsDBNameBuilder.ToString();
        }

        private DataTable GetCopyToDWObjectFields(string connectionString)
        {
            DataTable objectFieldsTable = new DataTable();
            using (SqlConnection sourceConnection = new SqlConnection(connectionString))
            {
                sourceConnection.Open();
                SqlCommand commandSourceData = new SqlCommand("SELECT  FieldName,ObjectTableId from ObjectFields where CopyToDW = 1", sourceConnection);
                SqlDataReader reader = commandSourceData.ExecuteReader();
                objectFieldsTable.Load(reader);
                reader.Close();
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
            }
            return objectTablesLists;
        }

        private static string GetDWTablesNamesAsString(List<TableClass> tableNameLists)
        {
            StringBuilder tableNamesBuilder = new StringBuilder("(");
            foreach (TableClass table in tableNameLists)
            {
                string name = table.TableName == "ShipmentMasterData" ? "Master" : table.TableName;
                tableNamesBuilder.Append(("'" + name + (tableNameLists.Last() != table ? "'," : "')")));
            }
            return tableNamesBuilder.ToString() ;
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
            foreach (TableClass tableClass in tableNameLists.Where(d =>d.HasFactTable).ToList())
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

            }
            return dWObjectFieldsMetaData;
        }

        private List<DWObjectFieldDB> GetDWObjectFieldDBLists(List<DataRow> rowList)
        {
            var result = new List<DWObjectFieldDB>();
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
                result.Add(fieldDB);
            }

            return result;
        }

        #endregion
    }
}
