
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using WarehouseData;

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
        public DataTable GetDataTableFromSql(string connectionString, string sqlString)
        {
            var result = new DataTable();
            using (SqlConnection sourceConnection = new SqlConnection(connectionString))
            {
                sourceConnection.Open();
                SqlCommand commandSourceData = new SqlCommand(sqlString, sourceConnection);
                SqlDataReader reader = commandSourceData.ExecuteReader();
                result.Load(reader);
                reader.Close();

            }
            return result;
        }

        public List<TableClass> FillDataWarehouseTable()
        {
            List<TableClass> tableNameLists = new List<TableClass>();
            tableNameLists.Add(new TableClass() { TableName = "DWHSetting", DBTableName = "DWHSettings", Dw_TableName = "dw_DWHSettings", KeyName = "Tenant", HasConstraint = true, HasNotSpecifiedValue = true, RelatedFactTables = GetAllFactTableLists() });
            tableNameLists.Add(new TableClass() { TableName = "Address", DBTableName = "Addresses", Dw_TableName = "dw_Addresses", KeyName = "Id", HasConstraint = true, HasNotSpecifiedValue = true, RelatedFactTables = GetAllFactTableLists() });
            tableNameLists.Add(new TableClass() { TableName = "Country", DBTableName = "Countries", DispayInScreen = true, Dw_TableName = "dw_Countries", KeyName = "Id", HasNotSpecifiedValue = true, HasDimensionTable = true, DWObjectTableCode = "DIM_Countries", BuildScriptName = "BuildCountriesDimensionTable", IncrementalScriptName = "UpdateCountriesDimensionTable" , RelatedFactTables = GetAllFactTableLists() });
            tableNameLists.Add(new TableClass() { TableName = "State", DBTableName = "States", Dw_TableName = "dw_States", KeyName = "Id", HasNotSpecifiedValue = true, RelatedFactTables = GetAllFactTableLists() });
            tableNameLists.Add(new TableClass() { TableName = "PartnerType", IsCloseTable = true, DBTableName = "PartnerTypes", Dw_TableName = "dw_PartnerTypes", KeyName = "Id", HasNotSpecifiedValue = true, RelatedFactTables = GetAllFactTableLists() });
            tableNameLists.Add(new TableClass() { TableName = "ObjectField", DBTableName = "ObjectFields", Dw_TableName = "dw_ObjectFields", KeyName = "Id", FieldsDBName = "FieldName,DataTypeCode,ObjectTableId,IsCustom", RelatedFactTables = GetAllFactTableLists() });
            tableNameLists.Add(new TableClass() { TableName = "Rank", DBTableName = "Ranks", Dw_TableName = "dw_Ranks", KeyName = "Id",  RelatedFactTables = GetAllFactTableLists() });
            tableNameLists.Add(new TableClass() { TableName = "LeadSource", DBTableName = "LeadSources", Dw_TableName = "dw_LeadSources", KeyName = "Id",  RelatedFactTables = GetAllFactTableLists() });
            tableNameLists.Add(new TableClass() { TableName = "Region", DBTableName = "Regions", Dw_TableName = "dw_Regions", KeyName = "Id", RelatedFactTables = GetAllFactTableLists() });
            tableNameLists.Add(new TableClass() { TableName = "CustomerSize", DBTableName = "CustomerSizes", Dw_TableName = "dw_CustomerSizes", KeyName = "Id", RelatedFactTables = GetAllFactTableLists() });
            tableNameLists.Add(new TableClass() { TableName = "Industry", DBTableName = "Industries", Dw_TableName = "dw_Industries", KeyName = "Id", RelatedFactTables = GetAllFactTableLists() });
            
            tableNameLists.Add(new TableClass() { TableName = "ShipmentMasterData", FieldsDBName = "MasterShipmentNumber,StatusDate,StatusId", DBTableName = "ShipmentMasterDatas", Dw_TableName = "dw_ShipmentMasterDatas", KeyName = "Id", HasConstraint = true, DispayInScreen = true, HasNotSpecifiedValue = true, RelatedFactTables = new List<string> { "Fact_Charges", "Fact_Shipments" } });
            tableNameLists.Add(new TableClass() { TableName = "ShipmentComputedFields", DBTableName = "ShipmentComputedFields", Dw_TableName = "dw_ShipmentComputedFields", KeyName = "Id", HasConstraint = true, DispayInScreen = true, RelatedFactTables = new List<string> { "Fact_Charges", "Fact_Shipments" } });
           
            tableNameLists.Add(new TableClass() { TableName = "ShipmentPayable", AdditionalIndexes = "ShipmentId", ParentKeyName = "ShipmentId", HasConstraint = true, DBTableName = "ShipmentPayables", DispayInScreen = true, Dw_TableName = "dw_ShipmentPayables", KeyName = "Id", RelatedFactTables = new List<string> { "Fact_Charges" } });
            tableNameLists.Add(new TableClass() { TableName = "APInvoiceLine", DBTableName = "APInvoiceLines", AdditionalIndexes = "EntityPayableId", DispayInScreen = true, Dw_TableName = "dw_APInvoiceLines", FieldsDBName = "EntityPayableId", KeyName = "APInvoiceId", RelatedFactTables = new List<string> { "Fact_Charges" } });
            tableNameLists.Add(new TableClass() { TableName = "APInvoice", HasConstraint = true, DBTableName = "APInvoices", DispayInScreen = true, Dw_TableName = "dw_APInvoices", KeyName = "Id", RelatedFactTables = new List<string> { "Fact_Charges", "Fact_Invoices" } });
            tableNameLists.Add(new TableClass() { TableName = "ShipmentReceivable", AdditionalIndexes = "ShipmentId", ParentKeyName = "ShipmentId", DispayInScreen = true, DBTableName = "ShipmentReceivables", Dw_TableName = "dw_ShipmentReceivables", KeyName = "Id", RelatedFactTables = new List<string> { "Fact_Charges" } });
            tableNameLists.Add(new TableClass() { TableName = "ARInvoiceLine", FieldsDBName = "ARInvoiceId,ReceivableId", DispayInScreen = true, AdditionalIndexes = "ReceivableId", DBTableName = "ARInvoiceLines", Dw_TableName = "dw_ARInvoiceLines", KeyName = "Id", RelatedFactTables = new List<string> { "Fact_Charges" } });
            tableNameLists.Add(new TableClass() { TableName = "ARInvoice", HasConstraint = true, DispayInScreen = true, DBTableName = "ARInvoices", Dw_TableName = "dw_ARInvoices", KeyName = "Id", RelatedFactTables = new List<string> { "Fact_Charges", "Fact_Invoices" } });
            tableNameLists.Add(new TableClass() { TableName = "Shipment", RelatedEntities = tableNameLists.Where(d => d.TableName == "ShipmentPayable" || d.TableName == "ShipmentReceivable").ToList(), KeyName = "Id", DBTableName = "Shipments", Dw_TableName = "dw_Shipments", HasConstraint = true, DispayInScreen = true, RelatedFactTables = new List<string> { "Fact_Charges" , "Fact_Shipments" }, HasNotSpecifiedValue =true});
            
            tableNameLists.Add(new TableClass() { TableName = "Quote", KeyName = "Id", DBTableName = "Quotes", Dw_TableName = "dw_Quotes", HasConstraint = true, DispayInScreen = true ,RelatedFactTables = new List<string> { "Fact_Quotes" } });
            tableNameLists.Add(new TableClass() { TableName = "QuoteComputedField", DBTableName = "QuoteComputedFields", Dw_TableName = "dw_QuoteComputedFields", KeyName = "Id", HasConstraint = true, DispayInScreen = true, RelatedFactTables = new List<string> { "Fact_Quotes" } });


            //Dimension  Table
            tableNameLists.Add(new TableClass() { TableName = "CustomPickList", DBTableName = "CustomPickLists", Dw_TableName = "dw_CustomPickLists", KeyName = "Id", HasDimensionTable = true, DWObjectTableCode = "DIM_CustomPickLists", BuildScriptName = "BuildCustomPickListDimensionsTable", IncrementalScriptName = "UpdateCustomPickListDimensionsTable", FieldsDBName = "Code,Value,IsMultipleChoice", RelatedFactTables = GetAllFactTableLists() });
            tableNameLists.Add(new TableClass() { IsCloseTable = true, TableName = "Direction", DBTableName = "Directions", Dw_TableName = "dw_Directions", KeyName = "Id", HasDimensionTable = true, DWObjectTableCode = "DIM_Directions", BuildScriptName = "BuildDirectionDimensionsTable", IncrementalScriptName = "UpdateDirectionDimensionTable", RelatedFactTables = GetAllFactTableLists() });
            tableNameLists.Add(new TableClass() { IsCloseTable = true, TableName = "TransportMode", DBTableName = "TransportModes", Dw_TableName = "dw_TransportModes", KeyName = "Id", HasDimensionTable = true, DWObjectTableCode = "DIM_TransportModes", BuildScriptName = "BuildTransportModeDimensionTable", IncrementalScriptName = "UpdateTransportModeDimensionTable", RelatedFactTables = GetAllFactTableLists() });
            tableNameLists.Add(new TableClass() { IsCloseTable = true, TableName = "ShipmentLevel", DBTableName = "ShipmentLevels", Dw_TableName = "dw_Levels", KeyName = "Code", HasDimensionTable = true, DWObjectTableCode = "DIM_Levels", BuildScriptName = "BuildShipmentLevelDimensionTable", IncrementalScriptName = "UpdateShipmentLevelDimensionTable", RelatedFactTables = GetAllFactTableLists() });
            tableNameLists.Add(new TableClass() { IsCloseTable = true, TableName = "OBLType", DBTableName = "OBLTypes", Dw_TableName = "dw_OBLTypes", KeyName = "Code", HasDimensionTable = true, DWObjectTableCode = "DIM_OBLTypes", BuildScriptName = "BuildOBLTypeDimensionTable", IncrementalScriptName = "UpdateOBLTypeDimensionTable", RelatedFactTables = new List<string> { "Fact_Shipments" } });
            tableNameLists.Add(new TableClass() { IsCloseTable = true, TableName = "ShipmentType", DBTableName = "ShipmentTypes", Dw_TableName = "dw_Types", KeyName = "Id", HasDimensionTable = true, DWObjectTableCode = "DIM_Types", BuildScriptName = "BuildShipmentTypeDimensionTable", IncrementalScriptName = "UpdateShipmentTypeDimensionTable", HasNotSpecifiedValue = true, RelatedFactTables = GetAllFactTableLists() });
            tableNameLists.Add(new TableClass() { TableName = "Branch", DBTableName = "Branches", Dw_TableName = "dw_Branches", KeyName = "Id", HasDimensionTable = true, DWObjectTableCode = "DIM_Branches", BuildScriptName = "BuildBrancheDimensionTable", IncrementalScriptName = "UpdateBrancheDimensionTable", RelatedFactTables = GetAllFactTableLists() });
            tableNameLists.Add(new TableClass() { TableName = "EntityStatus", DBTableName = "EntityStatus", FieldsDBName = "StatusWeight", Dw_TableName = "dw_ShipmentStatuses", KeyName = "Id", HasDimensionTable = true, DWObjectTableCode = "DIM_ShipmentStatuses", BuildScriptName = "BuildEntityStatusDimensionTable", IncrementalScriptName = "UpdateEntityStatusDimensionTable", RelatedFactTables = new List<string> { "Fact_Charges", "Fact_Shipments" } });
            tableNameLists.Add(new TableClass() { TableName = "Card", DBTableName = "Cards", Dw_TableName = "dw_Partners", KeyName = "Id", HasDimensionTable = true, DWObjectTableCode = "DIM_Partners", BuildScriptName = "BuildCardsDimensionTable", IncrementalScriptName = "UpdateCardDimensionTable", HasConstraint = true, DispayInScreen = true, RelatedFactTables = GetAllFactTableLists() });
            tableNameLists.Add(new TableClass() { TableName = "Port", DBTableName = "Ports", Dw_TableName = "dw_Ports", KeyName = "Id", HasDimensionTable = true, DWObjectTableCode = "DIM_Ports", BuildScriptName = "BuildPortsDimensionTable", IncrementalScriptName = "UpdatePortsDimensionTable", HasConstraint = true, DispayInScreen = true, RelatedFactTables = GetAllFactTableLists() , HasNotSpecifiedValue = true });
            tableNameLists.Add(new TableClass() { TableName = "User", DBTableName = "Users", Dw_TableName = "dw_Users", KeyName = "Id", HasDimensionTable = true, DWObjectTableCode = "DIM_Users", BuildScriptName = "BuildUsersDimensionTable", IncrementalScriptName = "UpdateUsersDimensionTable", DispayInScreen = true, RelatedFactTables = GetAllFactTableLists() });
            tableNameLists.Add(new TableClass() { TableName = "Contact", DBTableName = "Contacts", Dw_TableName = "dw_Contacts", KeyName = "Id", DispayInScreen = true, HasNotSpecifiedValue = true, RelatedFactTables = GetAllFactTableLists() });
            tableNameLists.Add(new TableClass() { TableName = "Customer", DBTableName = "Customers", Dw_TableName = "dw_Customers", KeyName = "Id", DispayInScreen = true, HasConstraint = true, RelatedFactTables = GetAllFactTableLists() });
            tableNameLists.Add(new TableClass() { TableName = "Tenant", DBTableName = "Tenants", Dw_TableName = "dw_Tenants", KeyName = "Id", HasDimensionTable = true, DWObjectTableCode = "DIM_Tenants", HasConstraint = true, BuildScriptName = "BuildTenantDimensionTable", IncrementalScriptName = "UpdateTenantDimensionTable", DispayInScreen = true, RelatedFactTables = GetAllFactTableLists() });
            tableNameLists.Add(new TableClass() { TableName = "Department", DBTableName = "Departments", Dw_TableName = "dw_Departments", KeyName = "Id", HasDimensionTable = true, DWObjectTableCode = "DIM_Departments", BuildScriptName = "BuildDepartmentDimensionTable", IncrementalScriptName = "UpdateDepartmentDimensionTable", DispayInScreen = true, RelatedFactTables = GetAllFactTableLists() });
            tableNameLists.Add(new TableClass() { TableName = "Incoterm", DBTableName = "Incoterms", Dw_TableName = "dw_Incoterms", KeyName = "Id", HasDimensionTable = true, DWObjectTableCode = "DIM_Incoterms", BuildScriptName = "BuildIncotermDimensionTable", IncrementalScriptName = "UpdateIncotermDimensionTable", DispayInScreen = true, RelatedFactTables = GetAllFactTableLists() });
            tableNameLists.Add(new TableClass() { TableName = "Currency", DBTableName = "Currencies", Dw_TableName = "dw_Currencies", KeyName = "Id", HasDimensionTable = true, DWObjectTableCode = "DIM_Currencies", BuildScriptName = "BuildCurrencyDimensionTable", IncrementalScriptName = "UpdateCurrencyDimensionTable", DispayInScreen = true, RelatedFactTables = GetAllFactTableLists() });
            tableNameLists.Add(new TableClass() { TableName = "MoveType", DBTableName = "MoveTypes", Dw_TableName = "dw_MoveTypes", KeyName = "Id", HasDimensionTable = true, DWObjectTableCode = "DIM_MoveTypes", BuildScriptName = "BuildMoveTypeDimensionTable", IncrementalScriptName = "UpdateMoveTypeDimensionTable", HasConstraint = true, RelatedFactTables = new List<string> { "Fact_Shipments" } });
            tableNameLists.Add(new TableClass() { TableName = "Vessel", DBTableName = "Vessels", Dw_TableName = "dw_Vessels", KeyName = "Id", HasDimensionTable = true, DWObjectTableCode = "DIM_Vessels", BuildScriptName = "BuildVesselDimensionTable", IncrementalScriptName = "UpdateVesselDimensionTable", HasConstraint = true, RelatedFactTables = new List<string> {  "Fact_Shipments" } });
            tableNameLists.Add(new TableClass() { TableName = "SpecialServicesType", DBTableName = "SpecialServicesTypes", Dw_TableName = "dw_SpecialServicesTypes", KeyName = "Id", HasDimensionTable = true, DWObjectTableCode = "DIM_SpecialServicesTypes", BuildScriptName = "BuildSpecialServicesTypeDimensionTable", IncrementalScriptName = "UpdateSpecialServicesTypeDimensionTable", HasConstraint = true, RelatedFactTables = new List<string> { "Fact_Charges", "Fact_Shipments" } });
            tableNameLists.Add(new TableClass() { TableName = "ChargesType", DBTableName = "ChargesTypes", DispayInScreen = true, Dw_TableName = "dw_ChargesTypes", KeyName = "Id", HasDimensionTable = true, DWObjectTableCode = "DIM_ChargesTypes", BuildScriptName = "BuildChargesTypeDimensionTable", IncrementalScriptName = "UpdateChargesTypeDimensionTable", RelatedFactTables = new List<string> { "Fact_Charges"} });
            tableNameLists.Add(new TableClass() { IsCloseTable = true, TableName = "ShipmentPayableStatus", DBTableName = "ShipmentPayableStatus", DispayInScreen = true, Dw_TableName = "dw_ShipmentPayableStatuses", KeyName = "Code", HasDimensionTable = true, DWObjectTableCode = "DIM_ShipmentPayableStatuses", BuildScriptName = "BuildShipmentPayableStatusDimensionTable", IncrementalScriptName = "UpdateShipmentPayableStatusDimensionTable", RelatedFactTables = GetAllFactTableLists() });
            tableNameLists.Add(new TableClass() { IsCloseTable = true, TableName = "ShipmentReceivableStatus", DBTableName = "ShipmentReceivableStatus", DispayInScreen = true, Dw_TableName = "dw_ShipmentReceivableStatuses", KeyName = "Code", HasDimensionTable = true, DWObjectTableCode = "DIM_ShipmentReceivableStatuses", BuildScriptName = "BuildShipmentReceivableStatusDimensionTable", IncrementalScriptName = "UpdateShipmentReceivableStatusDimensionTable", RelatedFactTables = GetAllFactTableLists() });
            tableNameLists.Add(new TableClass() { TableName = "QuoteStage", DBTableName = "QuoteStages", DispayInScreen = true, Dw_TableName = "dw_QuoteStages", KeyName = "Id", HasDimensionTable = true, DWObjectTableCode = "DIM_QuoteStages", BuildScriptName = "BuildQuoteStagesDimensionTable", IncrementalScriptName = "UpdateQuoteStagesDimensionTable", RelatedFactTables = new List<string>() { "Fact_Quotes" } });
            tableNameLists.Add(new TableClass() { TableName = "QuoteClosingReason", DBTableName = "QuoteClosingReasons", DispayInScreen = true, Dw_TableName = "dw_QuoteClosingReasons", KeyName = "Id", HasDimensionTable = true, DWObjectTableCode = "DIM_QuoteClosingReasons", BuildScriptName = "BuildQuoteClosingReasonsDimensionTable", IncrementalScriptName = "UpdateQuoteClosingReasonsDimensionTable", RelatedFactTables = new List<string>() { "Fact_Quotes" } });
            tableNameLists.Add(new TableClass() { TableName = "PaymentTerm", DBTableName = "PaymentTerms", DispayInScreen = true, Dw_TableName = "dw_PaymentTerms", KeyName = "Id", HasDimensionTable = true, DWObjectTableCode = "DIM_PaymentTerms", BuildScriptName = "BuildPaymentTermsDimensionTable", IncrementalScriptName = "UpdatePaymentTermsDimensionTable" , RelatedFactTables = GetAllFactTableLists() });
            tableNameLists.Add(new TableClass() { IsCloseTable = true, TableName = "APInvoiceType", DBTableName = "APInvoiceTypes", Dw_TableName = "dw_APInvoiceTypes", KeyName = "Name", DispayInScreen = true, HasConstraint = true, RelatedFactTables = GetAllFactTableLists() });
            tableNameLists.Add(new TableClass() { IsCloseTable = true, TableName = "ARInvoiceType", DBTableName = "ARInvoiceTypes", Dw_TableName = "dw_ARInvoiceTypes", KeyName = "Name", HasDimensionTable = true, HasConstraint = true,  DispayInScreen = true,  DWObjectTableCode = "DIM_InvoiceTypes", BuildScriptName = "BuildInvoiceTypesDimensionTable", IncrementalScriptName = "UpdateInvoiceTypesDimensionTable", RelatedFactTables = GetAllFactTableLists() });

            tableNameLists.Add(new TableClass() { IsCloseTable = true, TableName = "APInvoiceStatus", DBTableName = "APInvoiceStatus", Dw_TableName = "dw_APInvoiceStatus", KeyName = "Name", DispayInScreen = true, HasConstraint = true, RelatedFactTables = GetAllFactTableLists() });
            tableNameLists.Add(new TableClass() { IsCloseTable = true, TableName = "ARInvoiceStatus", DBTableName = "ARInvoiceStatus", Dw_TableName = "dw_ARInvoiceStatus", KeyName = "Name", HasDimensionTable = true, HasConstraint = true, DispayInScreen = true, DWObjectTableCode = "DIM_InvoiceStatus", BuildScriptName = "BuildInvoiceStatusDimensionTable", IncrementalScriptName = "UpdateInvoiceStatusDimensionTable", RelatedFactTables = GetAllFactTableLists() });

            tableNameLists.Add(new TableClass() { TableName = "VatType", DBTableName = "VatTypes", DispayInScreen = true, Dw_TableName = "dw_VatTypes", KeyName = "Id", HasDimensionTable = true, DWObjectTableCode = "DIM_VatTypes", BuildScriptName = "BuildVatTypeDimensionTable", IncrementalScriptName = "UpdateVatTypeDimensionTable", RelatedFactTables = GetAllFactTableLists() });

            //Fact Table
            tableNameLists.Add(new TableClass() { TableName = "Shipment", FieldIndexes = "Source Tenant,Parent Tenant,Id,DirectHouse", DWObjectTableCode = "Fact_Shipments", KeyName = "Id", DWTableKeyName = "Id", Dw_TableName = "dw_Shipments", HasFactTable = true, BuildScriptName = "BuildFactShipmentTable", IncrementalScriptName = "UpdateFactShipmentTable", DispayInScreen = true, RelatedFactTables = new List<string>() { "Fact_Shipments" } });
            tableNameLists.Add(new TableClass() { TableName = "Shipment", FieldIndexes = "Source Tenant,Parent Tenant,Shipment Id,DirectHouse", DWObjectTableCode = "Fact_Charges", Dw_TableName = "dw_Shipments", KeyName = "[Shipment Id]", DWTableKeyName = "Id", HasFactTable = true, BuildScriptName = "BuildFactChargesTable", IncrementalScriptName = "UpdateFactChargesTable", DispayInScreen = true, RelatedFactTables = new List<string>() { "Fact_Charges" } });
            tableNameLists.Add(new TableClass() { TableName = "Quote", DWObjectTableCode = "Fact_Quotes", Dw_TableName = "dw_Quotes", KeyName = "Id", DWTableKeyName = "Id", HasFactTable = true, BuildScriptName = "BuildFactQuotesTable", IncrementalScriptName = "UpdateFactQuoteTable", DispayInScreen = true, RelatedFactTables = new List<string> { "Fact_Quotes" } });
            tableNameLists.Add(new TableClass() { HasMultipleDWTables = true, FieldIndexes = "Main Entity Id",  MultipleDW_TablesNames = new List<string> { "dw_ARInvoices", "dw_APInvoices" }, MultipleTablesNames = new List<string> { "ARInvoice", "APInvoice" }, TableName = "Invoice", DWObjectTableCode = "Fact_Invoices", KeyName = "Id", DWTableKeyName = "Id", HasFactTable = true, BuildScriptName = "BuildFactInvoicesTable", IncrementalScriptName = "UpdateFactInvoicesTable", DispayInScreen = true, RelatedFactTables = new List<string> { "Fact_Invoices" } });
 
            tableNameLists.Add(new TableClass() { TableName = "ARInvoice", DWObjectTableCode = "Fact_ARInvoices", Dw_TableName = "dw_ARInvoices", KeyName = "Id", DWTableKeyName = "Id", HasFactTable = true, BuildScriptName = "BuildFactARInvoicesTable", IncrementalScriptName = "UpdateFactARInvoicesTable", DispayInScreen = true, RelatedFactTables = new List<string> { "Fact_ARInvoices" } });

      

            //WaterMark
            tableNameLists.Add(new TableClass() { TableName = "WaterMark", DBTableName = "WaterMarks", Dw_TableName = "dw_WaterMarks", KeyName = "TableName", FieldsDBName = "TableName,LastUpdateDate", RelatedFactTables = GetAllFactTableLists() });
            
            
            return tableNameLists;

        }

        public List<string>   GetEnvironmentFactTables(string sourceConnection)
        {
            List<string> factTables = new List<string>();
            DataTable dataTable = GetDataTableFromSql(sourceConnection, "select FactCodes from DWHEnvironmentSettings");
            if (dataTable != null && dataTable.Rows != null)
            {
                factTables = dataTable.Rows[0]["FactCodes"].ToString().Split(',').ToList();
            }
            return factTables;
        }

        private List<string> GetAllFactTableLists()
        {
            List<string> factTables = new List<string>() { "Fact_Shipments", "Fact_Charges", "Fact_Quotes", "Fact_Invoices", "Fact_ARInvoices" };
            return factTables;
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

        public string GetSqlInsertNotSpecifiedRecorderToDB(SqlInsertNotSpecifiedRecordArgs args)
        {
            string result = "insert into " + args.TableName;
            StringBuilder fieldNamesBuilder = new StringBuilder(" ( ");
            StringBuilder fieldValuesBuilder = new StringBuilder(" values ( ");
            foreach (DWObjectFieldDB field in args.ObjectFieldDBLists)
            {
                string fieldValue = string.Empty; ;
                if (field.FieldName != "[Id_Number]")
                {
                    fieldNamesBuilder.Append(field.FieldName + ((args.ObjectFieldDBLists.Last() != field) ? "," : ")"));
                    fieldValuesBuilder.Append(GetNotSpecifiedFieldValue(args.Table, field , args.Tenant) + ((args.ObjectFieldDBLists.Last() != field) ? "," : ")"));
                }
            }
            result = result + fieldNamesBuilder.ToString() + " " + fieldValuesBuilder.ToString();

            return result;
        }

        private static string GetNotSpecifiedFieldValue(TableClass table, DWObjectFieldDB field , int tenant)
        {
            string fieldValue = string.Empty;
            string dataTypeCode = field.DataTypeCode;
            if (dataTypeCode.Contains("Text") || field.DataTypeCode == "System.String")
            {
                fieldValue = "'Not Specified'";
                if (field.FieldName.Contains("Id") || (field.FieldName.Contains("Code") && table.TableName != "Card")) fieldValue = field.MaxLength > 1 ? "'-1'" : "'1'";
                if (fieldValue.Length > field.MaxLength + 2) fieldValue = "null";

            }
            else if (dataTypeCode.Contains("Boolean") || dataTypeCode.Contains("Decimal") || dataTypeCode.Contains("Double") || field.DataTypeCode == "Integer" || field.DataTypeCode == "System.Int32")
            {
                if (field.FieldName == "[Tenant]") fieldValue = tenant.ToString();
                else fieldValue = field.FieldName == "[Tenant Number]" ? "-1" : "0";
                if (table.Dw_TableName == "dw_DWHSettings" && (field.FieldName == "[ParentTenant]" || field.FieldName == "[Tenant]")) fieldValue = "-1";

            }
            else if (dataTypeCode.Contains("DateTime"))
            {
               
                    fieldValue = field.FieldName == "[AutomaticLastUpdateDate]" || field.IsRequired ? " GETDATE()" : "null";
          
            }

            return fieldValue;
        }


        public string GetColumnNamesAsString(string tableName, string connectionString)
        {
            string columnNames = string.Empty;
            var dataTable = GetDataTableFromSql(connectionString, ("select top(1) * from " + tableName));
            string[] columnNameLists = dataTable.Columns.Cast<DataColumn>().Select(x => x.ColumnName).ToArray();
            foreach (string column in columnNameLists)
            {
                columnNames += column + ",";
            }
            if (!string.IsNullOrEmpty(columnNames))
            {
                columnNames = columnNames.Remove(columnNames.Length - 1);
            }

            return columnNames;
        }



        public List<TableClass> GetEnvironmentFactDataWarehouseTables(List<TableClass> dataWarehouseTables,string connectionString)
        {
            List<string> environmentFactTableCodes = GetEnvironmentFactTables(connectionString);   
            return  dataWarehouseTables.Where(dwTable => environmentFactTableCodes.Any(environmentFactCode => dwTable.RelatedFactTables.Any(factCode => factCode == environmentFactCode))).ToList();

        }

       
        public List<IndexItem> GetDWObjectFieldIndexes(string xml)
        {
            List<IndexItem> result = DeserializeObject<List<IndexItem>>(xml);
            return result;
        }


        public static T DeserializeObject<T>(string xmlstring)
        {
            XmlSerializer serilaizer = new XmlSerializer(typeof(T));
            using (var sr = new StringReader(xmlstring))
            {
                return (T)serilaizer.Deserialize(sr);
            }

        }


        public static  string GetFullExceptionMessageFromException(Exception exception)
        {
            var exceptionMessage = string.Empty;

            if (exception != null)
            {
                exceptionMessage = exception.Message;
                if (exception.InnerException != null)
                {
                    exceptionMessage = exceptionMessage + Environment.NewLine + exception.InnerException;
                }
                if (exception.StackTrace != null)
                {
                    exceptionMessage = exceptionMessage + Environment.NewLine + "Stack trace: " + exception.StackTrace;
                }
            }
            return exceptionMessage;
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

public class SqlInsertNotSpecifiedRecordArgs
{
    public string TableName { get; set; }
    public TableClass Table { get; set; }
    public int Tenant { get; set; }
    public List<DWObjectFieldDB> ObjectFieldDBLists { get; set; }


}


