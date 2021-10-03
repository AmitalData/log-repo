using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WarehouseData;
using WarehouseData.Helper;

namespace WarehouseData.Service
{
  public  class DWDataWarehouseService
    {
        public  List<string> CustomObjectFieldTableLists { get; set; }
        private GeneralDataWarehouseService generalDataWarehouseService;
        private WaterMarkDataWarehouseService waterMarkDataWarehouseService;
        public DWDataWarehouseService()
        {
            generalDataWarehouseService = new GeneralDataWarehouseService();
            waterMarkDataWarehouseService = new WaterMarkDataWarehouseService();
        }


        public void InitializationDWTable(TableClass table, string sourceConnectionString, string destinationConnectionString)
        {
            generalDataWarehouseService.ExecuteSql("IF OBJECT_ID ('" + table.Dw_TableName + "', 'U')  IS NOT NULL drop table " + table.Dw_TableName, destinationConnectionString);

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

            generalDataWarehouseService.ExecuteSql(CreateTABLE(table.Dw_TableName, dwObjectTable, table.ObjectFieldDBLists), destinationConnectionString);
            table.ObjectFieldDBLists = GetCopyToDwObjectFieldLists(dwObjectTable);

        }

        private List<DWObjectFieldDB> GetCopyToDwObjectFieldLists(DataTable dataTable)
        {
            List<DWObjectFieldDB> results = new List<DWObjectFieldDB>();
            for (int i = 0; i < dataTable.Columns.Count; i++)
            {
                var dWObjectFieldDB = new DWObjectFieldDB();
                dWObjectFieldDB.FieldName = "[" + dataTable.Columns[i].ColumnName + "]";
                dWObjectFieldDB.DataTypeCode = dataTable.Columns[i].DataType.ToString();
                dWObjectFieldDB.MaxLength = dataTable.Columns[i].MaxLength;
                dWObjectFieldDB.IsRequired = !dataTable.Columns[i].AllowDBNull;
                results.Add(dWObjectFieldDB);
            }
            return results;
        }

        private string CreateTABLE(string tableName, DataTable table, List<DWObjectFieldDB> copyToDwObjectFieldLists)
        {

            string sqlsc;
            sqlsc = "CREATE TABLE " + tableName + "(";
            for (int i = 0; i < table.Columns.Count; i++)
            {
                sqlsc += "\n [" + table.Columns[i].ColumnName + "] ";
                string columnType = table.Columns[i].DataType.ToString();
                if (table.Columns[i].MaxLength > 4000) table.Columns[i].MaxLength = -1;
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
                        string dataTypeCode = "varchar";
                        if (copyToDwObjectFieldLists != null)
                        {
                            var objectField = copyToDwObjectFieldLists.Where(d => d.FieldName == table.Columns[i].ColumnName).FirstOrDefault();
                            if (objectField != null && objectField.DataTypeCode == "nText") dataTypeCode = "nvarchar";

                        }
                        sqlsc += string.Format( " "+(dataTypeCode + " ({0}) "), table.Columns[i].MaxLength == -1 ? "max" : table.Columns[i].MaxLength.ToString());
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

        public void CreateIndex(TableClass table, string fieldName, string connectionString)
        {
            string cmd = "CREATE NONCLUSTERED INDEX [IX_" + table.Dw_TableName + "_" + fieldName + "] ON[dbo].[" + table.Dw_TableName + "]([" + fieldName + "])";

            if (table.DBTableName == "Shipments" && fieldName == "Id")
            {
                string customFieldindex = "";
                int i = 1;
                while (i <= 40)
                {
                    customFieldindex += "[Field" + i + "],";
                    i += 1;
                }

                cmd += "; CREATE NONCLUSTERED INDEX[dw_Shipments_AllColumnsIndexes]ON[dbo].[dw_Shipments]([AutomaticLastUpdateDate])INCLUDE([Id],[Tenant],[ShipmentNumber],[House],[BranchId],[IncotermId],[SalesmanUserId],[DepartmentId],[ShipmentTypeId],[ShipperId],[ConsigneeId],[TransportModeId],[DirectionId],[AgentId],[IsOperationalClosed],[ChargeableWeightInKG],[GrossWeightInKG],[VolumeInCBM],[NumberOfContainers],[NumberOfPackages],[IsDangerous],[ComputedStatusId],[IsAccountingClosed],[AccountedReceivablesInLocalCurrency],[ProfitInLocalCurrency],[CustomerId],[ProfitCurrencyId],[ProfitInProfitCurrency],[AccountedReceivablesInProfitCurrency],[MasterShipmentDataId],[FromPortId],[ToPortId],[ShipmentLevelCode],[AccountedPayablesInLocalCurrency],[AccountedPayablesInProfitCurrency],[FinalArrivalDate],[AccountManagerUserId],[StatusLocation],[CustomsClearanceDate],[FreightForwarderId] ,[CustomAgentExportId],[CustomAgentImportId],[ValueOfGoodsCurrencyId],[WarehouseLegWarehouseId] " +
                    ", [IsCancelled] , [StatusDate] , [CustomsDeclarationNumber] ,[FirstOperationalCloseDate] , [EstimatedFinalArrivalDate] , [ActualFinalArrivalDate],[Routing],[DescriptionOfGoods],[PreForwardingETD],[MoveTypeId], " + customFieldindex + "[SpecialServicesTypeId],[ConsolidatorId],[Notify1Id],[Notify2Id],[ColoaderId],[ShipperNotExporterId],[ReleasingAgentId] , [ConsigneeNotImporterId] , [IssuingCarrierAgentId], [OnForwardingTransportModeId],[FirstARInvoiceApprovalDate],[ShipperAddressId],[ShipperContactId],[ShipperNotExporterAddressId],[ShipperNotExporterContactId],[FreelancerAddressId],[FreelancerContactId],[ReleasingAgentAddressId],[ReleasingAgentContactId],[CustomerAddressId],[CustomerContactId],[ConsigneeAddressId],[ConsigneeContactId],[AgentAddressId],[AgentContactId],[CustomAgentExportAddressId],[CustomAgentExportContactId] " +
                     ", [CustomAgentImportAddressId] , [CustomAgentImportContactId] , [Notify1AddressId] ,[Notify1ContactId] , [Notify2AddressId] , [Notify2ContactId],[FreightForwarderAddressId],[FreightForwarderContactId],[ConsigneeNotImporterAddressId],[ConsigneeNotImporterContactId],[CustomClearancePointAddressId],[CustomClearancePointContactId],[ColoaderAddressId],[ColoaderContactId],[ConsolidatorAddressId],[ConsolidatorContactId])";
           
               
            }
            generalDataWarehouseService.ExecuteSql(cmd, connectionString);
        }


        public void CreateAdditionalIndexes(TableClass table, string destinationConnectionString)
        {

            foreach (string index in table.AdditionalIndexes.Split(','))
            {
                CreateIndex(table, index, destinationConnectionString);
            }

        }

        public void AddConstraint(TableClass table, string connectionString)
        {
            string cmd = string.Empty;
            switch (table.DBTableName)
            {


                case "Quotes":

                    cmd = " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "ShipperId DEFAULT '-1' FOR ShipperId"
                        + " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "ConsigneeId DEFAULT '-1' FOR ConsigneeId;"
                        + " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "AgentId DEFAULT '-1' FOR AgentId;"
                        + " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "CustomerId DEFAULT '-1' FOR CustomerId;"
                        + " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "IncotermId DEFAULT '-1' FOR IncotermId;"
                        + " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "SalesmanUserId DEFAULT '-1' FOR SalesmanUserId;"

                        + " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "ShipmentTypeId DEFAULT '-1' FOR ShipmentTypeId;"
                        + " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "DepartmentId DEFAULT '-1' FOR DepartmentId;"
                        + " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "BranchId DEFAULT '-1' FOR BranchId;"
                        + " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "SaleCurrencyId DEFAULT '-1' FOR SaleCurrencyId;"
                        + " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "DirectionId DEFAULT '1' FOR DirectionId;"
                        + " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "TransportModeId DEFAULT '1' FOR TransportModeId;"
                        + " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "StageId DEFAULT '-1' FOR StageId;"
                        + " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "QuoteClosingReasonId DEFAULT '-1' FOR QuoteClosingReasonId;"
                        + " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "FromPortId DEFAULT '-1' FOR FromPortId;"
                        + " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "ToPortId DEFAULT '-1' FOR ToPortId;"
                        ;

                    break;







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
                        + " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "DirectionId DEFAULT '1' FOR DirectionId;"
                        + " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "TransportModeId DEFAULT '1' FOR TransportModeId;"
                        + " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "ShipmentLevelCode DEFAULT '1' FOR ShipmentLevelCode;"
                        + " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "FreightForwarderId DEFAULT '-1' FOR FreightForwarderId;"
                        + " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "CustomAgentImportId DEFAULT '-1' FOR CustomAgentImportId;"
                        + " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "CustomAgentExportId DEFAULT '-1' FOR CustomAgentExportId;"
                        + " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "WarehouseLegWarehouseId DEFAULT '-1' FOR WarehouseLegWarehouseId"
                        + " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "CreatedByUserId DEFAULT '-1' FOR CreatedByUserId"
                        + " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "ValueOfGoodsCurrencyId DEFAULT '-1' FOR ValueOfGoodsCurrencyId"
                        + " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "MoveTypeId DEFAULT '-1' FOR MoveTypeId"
                        + " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "SpecialServicesTypeId DEFAULT '-1' FOR SpecialServicesTypeId"
                        + " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "AgentComputed DEFAULT '-1' FOR AgentComputed"
                        + " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "ConsolidatorId DEFAULT '-1' FOR ConsolidatorId"
                        + " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "Notify1Id DEFAULT '-1' FOR Notify1Id"
                        + " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "Notify2Id DEFAULT '-1' FOR Notify2Id"
                        + " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "ColoaderId DEFAULT '-1' FOR ColoaderId"
                        + " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "ShipperNotExporterId DEFAULT '-1' FOR ShipperNotExporterId"
                        + " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "ReleasingAgentId DEFAULT '-1' FOR ReleasingAgentId"
                        + " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "ConsigneeNotImporterId DEFAULT '-1' FOR ConsigneeNotImporterId"
                        + " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "IssuingCarrierAgentId DEFAULT '-1' FOR IssuingCarrierAgentId"
                        + " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "ComputedStatusId DEFAULT '-1' FOR ComputedStatusId;"
                        + " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "ShipmentPayableStatusCode DEFAULT '-1' FOR ShipmentPayableStatusCode;"
                        + " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "ShipmentReceivableStatusCode DEFAULT '-1' FOR ShipmentReceivableStatusCode;"

                        + " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "OnForwardingTransportModeId DEFAULT '1' FOR OnForwardingTransportModeId"
                        + " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "PreForwardingTransportModeId DEFAULT '1' FOR PreForwardingTransportModeId"
                        + " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "PreForwardingFromPortId DEFAULT '-1' FOR PreForwardingFromPortId"

                        + " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "PreForwardingToPortId DEFAULT '-1' FOR PreForwardingToPortId"
                        + " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "OnForwardingToPortId DEFAULT '-1' FOR OnForwardingToPortId"
                        + " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "OnForwardingFromPortId DEFAULT '-1' FOR OnForwardingFromPortId"
                        + " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "OnForwardingCarrierId DEFAULT '-1' FOR OnForwardingCarrierId"
                        + " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "PreForwardingCarrierId DEFAULT '-1' FOR PreForwardingCarrierId"
                        + " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "HandlerUserId DEFAULT '-1' FOR HandlerUserId;"




                       

                        + " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "ShipperAddressId DEFAULT '-1' FOR ShipperAddressId;"
                        + " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "ShipperContactId DEFAULT '-1' FOR ShipperContactId;"
                        + " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "ShipperNotExporterAddressId DEFAULT '-1' FOR ShipperNotExporterAddressId;"
                        + " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "ShipperNotExporterContactId DEFAULT '-1' FOR ShipperNotExporterContactId;"
                        + " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "FreelancerAddressId DEFAULT '-1' FOR FreelancerAddressId;"
                        + " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "FreelancerContactId DEFAULT '-1' FOR FreelancerContactId;"
                        + " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "ReleasingAgentAddressId DEFAULT '-1' FOR ReleasingAgentAddressId;"
                        + " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "ReleasingAgentContactId DEFAULT '-1' FOR ReleasingAgentContactId;"
                        + " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "CustomerAddressId DEFAULT '-1' FOR CustomerAddressId;"
                        + " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "CustomerContactId DEFAULT '-1' FOR CustomerContactId;"
                        + " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "ConsigneeAddressId DEFAULT '-1' FOR ConsigneeAddressId;"
                        + " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "ConsigneeContactId DEFAULT '-1' FOR ConsigneeContactId;"
                        + " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "AgentAddressId DEFAULT '-1' FOR AgentAddressId;"
                        + " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "AgentContactId DEFAULT '-1' FOR AgentContactId;"
                        + " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "CustomAgentExportAddressId DEFAULT '-1' FOR CustomAgentExportAddressId;"
                        + " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "CustomAgentExportContactId DEFAULT '-1' FOR CustomAgentExportContactId;"
                        +" ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "CustomAgentImportAddressId DEFAULT '-1' FOR CustomAgentImportAddressId;"
                        + " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "CustomAgentImportContactId DEFAULT '-1' FOR CustomAgentImportContactId;"
                        + " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "Notify1AddressId DEFAULT '-1' FOR Notify1AddressId;"
                        + " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "Notify1ContactId DEFAULT '-1' FOR Notify1ContactId;"
                        + " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "Notify2AddressId DEFAULT '-1' FOR Notify2AddressId;"
                        + " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "Notify2ContactId DEFAULT '-1' FOR Notify2ContactId;"
                        + " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "FreightForwarderAddressId DEFAULT '-1' FOR FreightForwarderAddressId;"
                        + " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "FreightForwarderContactId DEFAULT '-1' FOR FreightForwarderContactId;"
                        + " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "ConsigneeNotImporterAddressId DEFAULT '-1' FOR ConsigneeNotImporterAddressId;"
                        + " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "ConsigneeNotImporterContactId DEFAULT '-1' FOR ConsigneeNotImporterContactId;"
                        + " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "CustomClearancePointAddressId DEFAULT '-1' FOR CustomClearancePointAddressId;"
                        + " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "CustomClearancePointContactId DEFAULT '-1' FOR CustomClearancePointContactId;"
                        + " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "ColoaderAddressId DEFAULT '-1' FOR ColoaderAddressId;"
                        + " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "ColoaderContactId DEFAULT '-1' FOR ColoaderContactId;"
                        + " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "ConsolidatorAddressId DEFAULT '-1' FOR ConsolidatorAddressId;"
                        + " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "ConsolidatorContactId DEFAULT '-1' FOR ConsolidatorContactId;"
                        ;

                    break;
                case "Cards":

                    cmd = " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "SalesmanUserId DEFAULT '-1' FOR SalesmanUserId"
                        + " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "PrimaryContactId DEFAULT '-1' FOR PrimaryContactId;"
                       + " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "CountryId DEFAULT '-1' FOR CountryId;"
                        + " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "PartnerTypeId DEFAULT '-1' FOR PartnerTypeId;"
                    + " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "BillToId DEFAULT '-1' FOR BillToId;";

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
                    + " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "MainCarriageVesselId DEFAULT '-1' FOR MainCarriageVesselId;"
                    + " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "Transshipment1VesselId DEFAULT '-1' FOR Transshipment1VesselId"
                    + " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "Transshipment1CarrierId DEFAULT '-1' FOR Transshipment1CarrierId"
                    + " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "OBLTypeCode DEFAULT '-1' FOR OBLTypeCode;" 
                    + " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "Transshipment1FromPortId DEFAULT '-1' FOR Transshipment1FromPortId;"
                    + " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "Transshipment2FromPortId DEFAULT '-1' FOR Transshipment2FromPortId;"
                    + " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "Transshipment3FromPortId DEFAULT '-1' FOR Transshipment3FromPortId;"
                    + " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "Transshipment2CarrierId DEFAULT '-1' FOR Transshipment2CarrierId"
                    + " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "Transshipment3CarrierId DEFAULT '-1' FOR Transshipment3CarrierId"
                    + " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "OnCarriageTransportModeId DEFAULT '1' FOR OnCarriageTransportModeId"
                    + " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "PreCarriageTransportModeId DEFAULT '1' FOR PreCarriageTransportModeId"
                    + " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "PreCarriageFromPortId DEFAULT '-1' FOR PreCarriageFromPortId"

                    + " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "PreCarriageToPortId DEFAULT '-1' FOR PreCarriageToPortId"
                    + " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "OnCarriageToPortId DEFAULT '-1' FOR OnCarriageToPortId"
                    + " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "OnCarriageFromPortId DEFAULT '-1' FOR OnCarriageFromPortId"
                    + " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "OnCarriageCarrierId DEFAULT '-1' FOR OnCarriageCarrierId"
                    + " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "PreCarriageCarrierId DEFAULT '-1' FOR PreCarriageCarrierId"
                        ;

                    break;

                case "DWHSettings":

                    cmd = " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "ParentTenant DEFAULT -1 FOR ParentTenant";

                    break;

                case "ShipmentComputedFields":

                    cmd = " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "DeliveryToPortId DEFAULT '-1' FOR DeliveryToPortId;"
                   + " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "OperationallyClosedByUserId DEFAULT '-1' FOR OperationallyClosedByUserId;"
                    + " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "PickupTruckerId DEFAULT '-1' FOR PickupTruckerId;"
                    + " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "DeliveryTruckerId DEFAULT '-1' FOR DeliveryTruckerId;"
                    + " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "AccountingClosedByUserId DEFAULT '-1' FOR AccountingClosedByUserId;";
                    break;

                case "ARInvoices":

                    cmd = " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "BillToId DEFAULT -1 FOR BillToId"
                    + " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "SalesmanUserId DEFAULT -1 FOR SalesmanUserId"
                    + " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "PrintByUserId DEFAULT -1 FOR PrintByUserId"
                    + " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "ApprovedByUserId DEFAULT -1 FOR ApprovedByUserId"
                    + " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "PartnerId DEFAULT -1 FOR PartnerId"
                    + " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "PaymentTermId DEFAULT -1 FOR PaymentTermId"
                    + " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "CreatedByUserId DEFAULT '-1' FOR CreatedByUserId"
                    +" ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "LocalCurrencyId DEFAULT '-1' FOR LocalCurrencyId"
                    +" ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "InvoiceCurrencyId DEFAULT '-1' FOR InvoiceCurrencyId"
                    +" ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "MainEntityId DEFAULT '-1' FOR MainEntityId";

                    
                    break;

                case "APInvoices":

                    cmd = " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "ApprovedByUserId DEFAULT -1 FOR ApprovedByUserId"
                    +" ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "PaymentTermId DEFAULT -1 FOR PaymentTermId";
                    break;

                case "ARInvoiceLines":

                    cmd = " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "ForiegnCurrencyId DEFAULT -1 FOR ForiegnCurrencyId"
                    + " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "VatTypeId DEFAULT -1 FOR VatTypeId"
                    +" ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "ARInvoiceId DEFAULT -1 FOR ARInvoiceId";

                    
                    break;


                case "ShipmentPayables":

                    cmd = " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "VendorId DEFAULT -1 FOR VendorId";

                    break;

                case "ShipmentReceivableStatus":

                    cmd = " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "Code DEFAULT -1 FOR Code";

                    break;

                case "ShipmentPayableStatus":

                    cmd = " ALTER TABLE " + table.Dw_TableName + " ADD CONSTRAINT DF_" + table.DBTableName + "Code DEFAULT -1 FOR Code";

                    break;

                default:
                    cmd = string.Empty;
                    break;
            }

            generalDataWarehouseService.ExecuteSql(cmd, connectionString);

        }

        public void InSertNotSpecifiedValueToDW(TableClass table, string connectionString, int? privateTenant = null)
        {
            var sqlInsertNotSpecifiedRecordArgs = new SqlInsertNotSpecifiedRecordArgs()
            {
                ObjectFieldDBLists = table.ObjectFieldDBLists,
                Table = table,
                Tenant = privateTenant != null ? (int)privateTenant : 0,
                TableName = table.Dw_TableName
            };
            string sqlString = generalDataWarehouseService.GetSqlInsertNotSpecifiedRecorderToDB(sqlInsertNotSpecifiedRecordArgs);
            generalDataWarehouseService.ExecuteSql(sqlString, connectionString);
        }




        #region CopyDataBase
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
                    condition += ((!isPrivateDB ? " where " : " and") + GetCustomObjectFieldCondition());
                }
            
                string originTableName = table.TableName == "WaterMark" && isPrivateDB ? ("Private" + table.DBTableName) : table.DBTableName;



                SqlCommand commandSourceData = new SqlCommand(
           "SELECT " + fieldName +
           " FROM dbo." + originTableName + condition + " ;", sourceConnection);



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

                        bulkCopy.BulkCopyTimeout = (int)this.generalDataWarehouseService.timeOut;

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

        private string GetCustomObjectFieldCondition()
        {
            string condition  = " IsCustom = 1 and ObjectTableId in (select id from ObjectTables where Name in (";
            foreach (string customObjectField in CustomObjectFieldTableLists)
            {
                condition += "'" + customObjectField + "' ,";
            }
            condition = condition.Remove(condition.Length - 1) + "))";
            return condition;
        }

        private void OnSqlRowsCopied(
       object sender, SqlRowsCopiedEventArgs e)
        {
            if (Control != null && Table != null)
            {
                SetControlPropertyValue(Control, "Text", Table.DBTableName + "  " + e.RowsCopied.ToString());
            }

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




       



        public void UpdateDWDataBase(BuildDWArgs buildDWArgs)
        {
            TableClass table = buildDWArgs.table;
            string condition = !string.IsNullOrEmpty(buildDWArgs.Conition) ? buildDWArgs.Conition : GetUpdateDWDataBaseCondition(buildDWArgs);
            var columnNames =generalDataWarehouseService.GetColumnNamesAsString(table.Dw_TableName , buildDWArgs.DestinationConnectionString);
     
            using (SqlConnection sourceConnection =
                       new SqlConnection(buildDWArgs.SourceConnectionString))
            {
                sourceConnection.Open();

                SqlCommand commandSourceData = new SqlCommand(
               "SELECT  " + columnNames +
               " FROM dbo." + table.DBTableName + condition, sourceConnection);

                SqlDataReader reader = commandSourceData.ExecuteReader();
                if (reader.HasRows)
                {
                    var dataTable = new DataTable();
                    dataTable.Load(reader);


                    var columns = dataTable.Rows
                                     .Cast<DataRow>()
                                     .Select(r => (string)r[table.KeyName].ToString())
                                     .ToList();

                    table.UpdatedCount = columns != null ? columns.Count() : 0;
                    table.RefreshIds = generalDataWarehouseService.DeleteRowsFromDataWarehouse(new DeleteRowsArgs() { TableName = table.Dw_TableName, KeyName = table.KeyName, IdsList = columns, ConnectionString = buildDWArgs.DestinationConnectionString, ReturnDeleteIdsAsString = true });

                    if (!string.IsNullOrEmpty(table.RefreshIds))
                    {
                        DateTime automaticLastUpdateDate = DateTime.Now;
                        if (!buildDWArgs.IsChildentity)
                        {
                            automaticLastUpdateDate = (DateTime)dataTable.Rows
                            .Cast<DataRow>()
                            .Max(d => d["AutomaticLastUpdateDate"]);
                        }

                        using (SqlConnection destinationConnection =
                                   new SqlConnection(buildDWArgs.DestinationConnectionString))
                        {
                            destinationConnection.Open();

                            using (SqlBulkCopy bulkCopy =
                                       new SqlBulkCopy(destinationConnection))
                            {
                                bulkCopy.DestinationTableName =
                                    "dbo." + table.Dw_TableName;

                                bulkCopy.BulkCopyTimeout = (int)this.generalDataWarehouseService.timeOut;

                                try
                                {


                                    bulkCopy.EnableStreaming = true;
                                    bulkCopy.BatchSize = 100000;
                                    bulkCopy.WriteToServer(dataTable);
                                }

                                finally
                                {
                                    reader.Close();

                                    if (table.DBTableName != "WaterMarks" && !buildDWArgs.IsChildentity)
                                    {
                                        var lastUpdateDate = string.Empty;
                                        if (automaticLastUpdateDate != null) lastUpdateDate = automaticLastUpdateDate.ToString("MM/dd/yyyy hh:mm:ss.fff tt");
                                        else lastUpdateDate = DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff tt");
                                        this.waterMarkDataWarehouseService.UpdateWareMarkTable(table, lastUpdateDate, buildDWArgs.SourceConnectionString, buildDWArgs.PrivateTenant);

                                        table.IsUpdated = true;

                                    }

                                    UpdateDWRelatedEntities(buildDWArgs, table);

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

        private void UpdateDWRelatedEntities(BuildDWArgs buildDWArgs, TableClass table)
        {
            if (table.RelatedEntities != null)
            {
                foreach (TableClass relatedEntity in table.RelatedEntities)
                {
                    var condition = (" where " + relatedEntity.ParentKeyName + " in  " + table.RefreshIds);
                    using (SqlConnection sqlConnection = new SqlConnection(buildDWArgs.DestinationConnectionString))
                    {
                        sqlConnection.Open();
                        SqlCommand sqlCommand = new SqlCommand("delete FROM dbo." + relatedEntity.Dw_TableName + condition, sqlConnection);
                        if (sqlCommand.ExecuteNonQuery() > 0)
                        {
                            UpdateDWDataBase(new BuildDWArgs() { table = relatedEntity, Conition = condition, IsChildentity = true, SourceConnectionString = buildDWArgs.SourceConnectionString, DestinationConnectionString = buildDWArgs.DestinationConnectionString, PrivateTenant = buildDWArgs.PrivateTenant, RelatedTenants = buildDWArgs.RelatedTenants });

                        }
                        sqlConnection.Close();
                    }
                }
            }
        }

        private  string GetUpdateDWDataBaseCondition(BuildDWArgs buildDWArgs)
        {

            bool isPrivateDB = buildDWArgs.PrivateTenant != null ? true : false;

            string condition = " where AutomaticLastUpdateDate > ( select LastUpdateDate from WaterMarks where TableName = " + "'" + buildDWArgs.table.TableName + "')";

            if (isPrivateDB)
            {
                condition = " where AutomaticLastUpdateDate > ( select LastUpdateDate from PrivateWaterMarks where TableName = " + "'" + buildDWArgs.table.TableName + "'" + " and PrivateTenant = " + buildDWArgs.PrivateTenant + ") ";
                if (!buildDWArgs.table.IsCloseTable && buildDWArgs.table.FieldsDBName.Contains("Tenant")) condition += " and Tenant in " + buildDWArgs.RelatedTenants;
                else if (buildDWArgs.table.DBTableName == "Tenants") condition += " and Id in " + buildDWArgs.RelatedTenants;
            }

            if (buildDWArgs.table.TableName == "ObjectField")
            {
                condition += (" and" + GetCustomObjectFieldCondition());
            }

            return condition;
        }

        public void UpdateAutomaticLastUpdate(TableClass table, string sourceConnectionString, string destinationConnectionString, int? tenant = null)
        {
            if (table.DBTableName != "WaterMarks")
            {
                string lastUpdateDate = this.GetAutomaticLastUpdateDate(table.Dw_TableName, destinationConnectionString);
                if (string.IsNullOrEmpty(lastUpdateDate)) lastUpdateDate = DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff tt");
                this.waterMarkDataWarehouseService.AddWareMarkRecord(table, lastUpdateDate, sourceConnectionString, tenant);
            }

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
                com.CommandTimeout = (int)this.generalDataWarehouseService.timeOut;
                con.Open();
                using (SqlDataReader reader = com.ExecuteReader())
                {
                    reader.Read();
                    DateTime? datetime = null;
                    var value = reader["AutomaticLastUpdateDate"];
                    if (value != null)
                    {
                        if (!string.IsNullOrEmpty(value.ToString()))
                        {
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

   


    }
}


public class BuildDWArgs
{
    public TableClass table { get; set; }
    public string SourceConnectionString { get; set; }
    public string DestinationConnectionString { get; set; }
    public string RelatedTenants { get; set; }
    public int? PrivateTenant { get; set; }
    public string Conition { get; set; }
    public bool IsChildentity { get; set; }





}
