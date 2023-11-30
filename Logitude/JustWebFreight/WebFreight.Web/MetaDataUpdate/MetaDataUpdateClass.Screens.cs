using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebFreight.Web.MetaDataUpdate.AddClasses;
using WebFreight.Web.MetaDataUpdate.DetailClasses;

namespace WebFreight.Web.MetaDataUpdate
{
    public partial class MetaDataUpdateClass
    {
        public void loadScreens()
        {
            ObjectContext = WebFreightContext.GetContext(0);
            ScreenFieldsRepository = new ScreenFieldsRepository(ObjectContext);
            ScreensRepository = new ScreensRepository(ObjectContext);
            Dictionary<string, Screen> tenantScreens = ScreensRepository.GetScreensByTenant(0).ToDictionary(d => d.Code + d.ObjectTableId, a => a);
            Dictionary<string, ScreenField> tenantScreenField = ScreenFieldsRepository.GetScreenFieldsByTenant(0).ToDictionary(d => d.ScreenId + d.ObjectFieldId);

            BuildShipmentScreens(tenantScreens, tenantScreenField);
            BuildMasterScreens(tenantScreens, tenantScreenField);
            BuildQuoteScreens(tenantScreens, tenantScreenField);
            BuildARInvoiceScreens(tenantScreens, tenantScreenField);
            BuildAPInvoiceScreens(tenantScreens, tenantScreenField);
            BuildARPaymentScreens(tenantScreens, tenantScreenField);
            BuildAPPaymentScreens(tenantScreens, tenantScreenField);
            BuildCustomerScreens(tenantScreens, tenantScreenField);
            BuildAgentScreens(tenantScreens, tenantScreenField);
            BuildCustomAgentScreens(tenantScreens, tenantScreenField);
            BuildShipppingAgentScreens(tenantScreens, tenantScreenField);
            BuildAirlineScreens(tenantScreens, tenantScreenField);
            BuildShippingLineScreens(tenantScreens, tenantScreenField);
            BuildTruckerScreens(tenantScreens, tenantScreenField);
            BuildVendorScreens(tenantScreens, tenantScreenField);
            BuildPortScreens(tenantScreens, tenantScreenField);
            BuildCountryScreens(tenantScreens, tenantScreenField);
            BuildGlobalZoneScreens(tenantScreens, tenantScreenField);
            BuildBranchScreens(tenantScreens, tenantScreenField);
            BuildDepartmentScreens(tenantScreens, tenantScreenField);
            BuildStatesScreens(tenantScreens, tenantScreenField);
            BuildCountryCityScreens(tenantScreens, tenantScreenField);
            BuildFollowUpTypeScreens(tenantScreens, tenantScreenField);
            BuildDocumentTypeScreens(tenantScreens, tenantScreenField);
            BuildDocumentsFilingScreens(tenantScreens, tenantScreenField);
            BuildEventTypeScreens(tenantScreens, tenantScreenField);
            BuildIncotermScreens(tenantScreens, tenantScreenField);
            BuildPaymentTermScreens(tenantScreens, tenantScreenField);
            BuildCurrencyScreens(tenantScreens, tenantScreenField);
            BuildVatTypeScreens(tenantScreens, tenantScreenField);
            BuildChargeTypeScreens(tenantScreens, tenantScreenField);
            BuildUserScreens(tenantScreens, tenantScreenField);
            BuildContactScreens(tenantScreens, tenantScreenField);
            BuildMainAddressForClientScreens(tenantScreens, tenantScreenField);
            BuildPackageTypeScreens(tenantScreens, tenantScreenField);
            BuildVesselScreens(tenantScreens, tenantScreenField);
            BuildWareHouseScreens(tenantScreens, tenantScreenField);
            BuildAccountScreens(tenantScreens, tenantScreenField);
            BuildCommunicationLogScreens(tenantScreens, tenantScreenField);
            BuildTenantManagementScreens(tenantScreens, tenantScreenField);
            BuildAnalyzeQueueScreens(tenantScreens, tenantScreenField);
            BuildCreditCardTypeScreens(tenantScreens, tenantScreenField);
            BuildLogitudeLeadScreens(tenantScreens, tenantScreenField);
            BuildErrorLogScreens(tenantScreens, tenantScreenField);
            BuildAccountingTransferScreens(tenantScreens, tenantScreenField);
            BuildMoveTypeScreens(tenantScreens, tenantScreenField);
            BuildReportScreens(tenantScreens, tenantScreenField);
            BuildLeadSourceScreens(tenantScreens, tenantScreenField);
            BuildCompetitorScreens(tenantScreens, tenantScreenField);
            BuildCommodityScreens(tenantScreens, tenantScreenField);
            BuildAdditionalServiceScreens(tenantScreens, tenantScreenField);
            BuildExternalSystemsTablesCodeScreens(tenantScreens, tenantScreenField);
            BuildIndustryScreens(tenantScreens, tenantScreenField);
            BuildProductTypeScreens(tenantScreens, tenantScreenField);
            BuildBusinessUnitScreens(tenantScreens, tenantScreenField);
            BuildSpecialServicesTypeScreens(tenantScreens, tenantScreenField);
            BuildRegionScreens(tenantScreens, tenantScreenField);
            BuildMessagingStockScreens(tenantScreens, tenantScreenField);
            BuildCustomerSizeScreens(tenantScreens, tenantScreenField);
            BuildQuoteStageScreens(tenantScreens, tenantScreenField);
            BuildMeasurementScreens(tenantScreens, tenantScreenField);
            BuildDistributorScreens(tenantScreens, tenantScreenField);
            BuildBluesnapContractScreens(tenantScreens, tenantScreenField);
            BuildDocumentFolderScreens(tenantScreens, tenantScreenField);
            BuildAccountingSystemScreens(tenantScreens, tenantScreenField);
            BuildAWBAdditionalHandlingInfoScreens(tenantScreens, tenantScreenField);
            BuildCustomerTenantAccessRequestScreens(tenantScreens, tenantScreenField);
            BuildAPILogsScreens(tenantScreens, tenantScreenField);
            BuildParticipantScreens(tenantScreens, tenantScreenField);
            BuildAirlineStatisticsScreens(tenantScreens, tenantScreenField);
            BuildCustomerTenantAccessScreens(tenantScreens, tenantScreenField);
            BuildLogitudeMessagesScreens(tenantScreens, tenantScreenField);
            BuildCustomerFieldsUpdateSettingScreens(tenantScreens, tenantScreenField);
            BuildChargesGroupScreens(tenantScreens, tenantScreenField);
            BuildBankAccountLiteScreens(tenantScreens, tenantScreenField);
            BuildAccountingPaymentMethodScreens(tenantScreens, tenantScreenField);
            BuildAPPaymentMethodScreens(tenantScreens, tenantScreenField);

            LoadObjectTableRulesANDFieldsValidations();
        }

        private void BuildAWBAdditionalHandlingInfoScreens(Dictionary<string, Screen> tenantScreens, Dictionary<string, ScreenField> tenantScreenField)
        {
            ObjectTable entityObject = ObjectContext.ObjectTables.Where(d => d.Name == "AWBAdditionalHandlingInfo" && d.Tenant == 0).FirstOrDefault();

            ObjectField objectField_Code = ObjectContext.ObjectFields.Where(d => d.FieldName == "Code" && d.ObjectTable.Id == entityObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField objectField_Name = ObjectContext.ObjectFields.Where(d => d.FieldName == "Name" && d.ObjectTable.Id == entityObject.Id && d.Tenant == 0).FirstOrDefault();

            Screen HeaderScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "AWBAdditionalHandlingInfo.HeaderScreen", Name = "Header Screen", ObjectTableId = entityObject.Id, NumberOfColumns = 1, NumberOfRows = 2, IsReadOnly = true }, ScreensRepository, tenantScreens);
            ScreenField Header_CodeScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = objectField_Code.Id, Row = 0, ScreenId = HeaderScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenField);
            ScreenField Header_NameScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = objectField_Name.Id, Row = 1, ScreenId = HeaderScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenField);
            entityObject.HeaderScreenId = HeaderScreen.Id;

            ObjectContext.SaveChanges();
        }

        private void BuildDistributorScreens(Dictionary<string, Screen> tenantScreens, Dictionary<string, ScreenField> tenantScreenFields)
        {
            ObjectTable distributorObject = ObjectContext.ObjectTables.Where(d => d.Name == "Distributor" && d.Tenant == 0).FirstOrDefault();

            ObjectField distributorCode = ObjectContext.ObjectFields.Where(d => d.FieldName == "Code" && d.ObjectTable.Id == distributorObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField distributorEnglishName = ObjectContext.ObjectFields.Where(d => d.FieldName == "EnglishName" && d.ObjectTable.Id == distributorObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField distributorLocalName = ObjectContext.ObjectFields.Where(d => d.FieldName == "LocalName" && d.ObjectTable.Id == distributorObject.Id && d.Tenant == 0).FirstOrDefault();

            //General Screen
            Screen generalTabScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "Distributor.GeneralTabScreen", Name = "General Tab Screen", ObjectTableId = distributorObject.Id, NumberOfColumns = 2, NumberOfRows = 4 }, ScreensRepository, tenantScreens);

            ScreenField distributorCodeScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = distributorCode.Id, Row = 0, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField distributorEnglishNameScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = distributorEnglishName.Id, Row = 1, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField distributorLocalNameScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = distributorLocalName.Id, Row = 2, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);

            ObjectContext.SaveChanges();
        }

        private void BuildMeasurementScreens(Dictionary<string, Screen> tenantScreens, Dictionary<string, ScreenField> tenantScreenFields)
        {
            ObjectTable entityObject = ObjectContext.ObjectTables.Where(d => d.Name == "Measurement" && d.Tenant == 0).FirstOrDefault();

            ObjectField objectField_Code = ObjectContext.ObjectFields.Where(d => d.FieldName == "Code" && d.ObjectTable.Id == entityObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField objectField_Name = ObjectContext.ObjectFields.Where(d => d.FieldName == "Name" && d.ObjectTable.Id == entityObject.Id && d.Tenant == 0).FirstOrDefault();

            Screen HeaderScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "Measurement.HeaderScreen", Name = "Header Screen", ObjectTableId = entityObject.Id, NumberOfColumns = 1, NumberOfRows = 2, IsReadOnly = true }, ScreensRepository, tenantScreens);
            ScreenField Header_CodeScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = objectField_Code.Id, Row = 0, ScreenId = HeaderScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField Header_NameScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = objectField_Name.Id, Row = 1, ScreenId = HeaderScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            entityObject.HeaderScreenId = HeaderScreen.Id;

            ObjectContext.SaveChanges();
        }

        private void BuildQuoteStageScreens(Dictionary<string, Screen> tenantScreens, Dictionary<string, ScreenField> tenantScreenFields)
        {
            ObjectTable entityObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "QuoteStage" && d.Tenant == 0).FirstOrDefault();

            ObjectField ObjectField_Code = ObjectContext.ObjectFields.Where(d => d.FieldName == "Code" && d.ObjectTable.Id == entityObjectTable.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField ObjectField_Name = ObjectContext.ObjectFields.Where(d => d.FieldName == "Name" && d.ObjectTable.Id == entityObjectTable.Id && d.Tenant == 0).FirstOrDefault();
            
            //Header Screen
            Screen HeaderScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "QuoteStage.HeaderScreen", Name = "Header Screen", ObjectTableId = entityObjectTable.Id, NumberOfColumns = 2, NumberOfRows = 1, IsReadOnly = true }, ScreensRepository, tenantScreens);
            ScreenField ScreenField_Code = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = ObjectField_Code.Id, Row = 0, ScreenId = HeaderScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField ScreenField_Name = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = ObjectField_Name.Id, Row = 0, ScreenId = HeaderScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            entityObjectTable.HeaderScreenId = HeaderScreen.Id;

            ObjectContext.SaveChanges();
        }

        private void BuildMessagingStockScreens(Dictionary<string, Screen> tenantScreens, Dictionary<string, ScreenField> tenantScreenFields)
        {
            ObjectTable entityTable = ObjectContext.ObjectTables.Where(d => d.Name == "MessagingStock" && d.Tenant == 0).FirstOrDefault();

            ObjectField objectField_01 = ObjectContext.ObjectFields.Where(d => d.FieldName == "StartDate" && d.ObjectTableId == entityTable.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField objectField_02 = ObjectContext.ObjectFields.Where(d => d.FieldName == "EndDate" && d.ObjectTableId == entityTable.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField objectField_03 = ObjectContext.ObjectFields.Where(d => d.FieldName == "Amount" && d.ObjectTableId == entityTable.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField objectField_04 = ObjectContext.ObjectFields.Where(d => d.FieldName == "Remaining" && d.ObjectTableId == entityTable.Id && d.Tenant == 0).FirstOrDefault();

            Screen headerScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "MessagingStock.HeaderScreen", Name = "Header Screen", ObjectTableId = entityTable.Id, NumberOfColumns = 2, NumberOfRows = 2, IsReadOnly = true, Tenant = 0 }, ScreensRepository, tenantScreens);
            ScreenField screenField_01 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 0, ObjectFieldId = objectField_01.Id, ScreenId = headerScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField screenField_02 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 1, ObjectFieldId = objectField_02.Id, ScreenId = headerScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField screenField_03 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, Row = 0, ObjectFieldId = objectField_03.Id, ScreenId = headerScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField screenField_04 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, Row = 1, ObjectFieldId = objectField_04.Id, ScreenId = headerScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);

            entityTable.HeaderScreenId = headerScreen.Id;

            ObjectContext.SaveChanges();
        }

        private void BuildBusinessUnitScreens(Dictionary<string, Screen> tenantScreens, Dictionary<string, ScreenField> tenantScreenFields)
        {
            ObjectTable entityTable = ObjectContext.ObjectTables.Where(d => d.Name == "BusinessUnit" && d.Tenant == 0).FirstOrDefault();

            ObjectField objectField_01 = ObjectContext.ObjectFields.Where(d => d.FieldName == "Name" && d.ObjectTableId == entityTable.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField objectField_02 = ObjectContext.ObjectFields.Where(d => d.FieldName == "ParentName" && d.ObjectTableId == entityTable.Id && d.Tenant == 0).FirstOrDefault();

            Screen headerScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "BusinessUnit.HeaderScreen", Name = "Header Screen", ObjectTableId = entityTable.Id, NumberOfColumns = 1, NumberOfRows = 2, IsReadOnly = true, Tenant = 0 }, ScreensRepository, tenantScreens);
            ScreenField screenField_01 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 0, ObjectFieldId = objectField_01.Id, ScreenId = headerScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField screenField_02 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 1, ObjectFieldId = objectField_02.Id, ScreenId = headerScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);

            entityTable.HeaderScreenId = headerScreen.Id;

            ObjectContext.SaveChanges();
        }

        public void BuildAccountingTransferScreens(Dictionary<string, Screen> tenantScreens, Dictionary<string, ScreenField> tenantScreenFields)
        {
            ObjectTable entityTableObject = ObjectContext.ObjectTables.Where(d => d.Name == "AccountingTransferHeader" && d.Tenant == 0).FirstOrDefault();

            ObjectField objectField1 = ObjectContext.ObjectFields.Where(d => d.FieldName == "TransferNumber" && d.ObjectTableId == entityTableObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField objectField2 = ObjectContext.ObjectFields.Where(d => d.FieldName == "TransferDate" && d.ObjectTableId == entityTableObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField objectField3 = ObjectContext.ObjectFields.Where(d => d.FieldName == "FileName" && d.ObjectTableId == entityTableObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField objectField4 = ObjectContext.ObjectFields.Where(d => d.FieldName == "UserName" && d.ObjectTableId == entityTableObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField objectField5 = ObjectContext.ObjectFields.Where(d => d.FieldName == "AccountingTransferTypeName" && d.ObjectTableId == entityTableObject.Id && d.Tenant == 0).FirstOrDefault();

            Screen headerScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "AccountingTransfer.HeaderScreen", Name = "Header Screen", ObjectTableId = entityTableObject.Id, NumberOfColumns = 3, NumberOfRows = 2, IsReadOnly = true }, ScreensRepository, tenantScreens);

            ScreenField screenField1 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 0, ObjectFieldId = objectField1.Id, ScreenId = headerScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField screenField2 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 1, ObjectFieldId = objectField2.Id, ScreenId = headerScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField screenField3 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, Row = 0, ObjectFieldId = objectField3.Id, ScreenId = headerScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField screenField4 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, Row = 1, ObjectFieldId = objectField4.Id, ScreenId = headerScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField screenField5 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 2, Row = 0, ObjectFieldId = objectField5.Id, ScreenId = headerScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);

            entityTableObject.HeaderScreenId = headerScreen.Id;

            ObjectContext.SaveChanges();
        }

        public void BuildShipmentScreens(Dictionary<string, Screen> tenantScreens, Dictionary<string, ScreenField> tenantScreenFields)
        {
            ObjectTable ShipmentObject = ObjectContext.ObjectTables.Where(d => d.Name == "Shipment" && d.Tenant == 0).FirstOrDefault();

            #region Shipment Header screen

            Screen shipmentHeaderScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "Shipment.HeaderScreen", Name = "Header Screen", ObjectTableId = ShipmentObject.Id, NumberOfColumns = 5, NumberOfRows = 2, IsReadOnly = true, Tenant = 0 }, ScreensRepository, tenantScreens);

            ObjectField shipmentTypeObject1 = ObjectContext.ObjectFields.Where(d => d.FieldName == "ShipmentTypeViewField" && d.ObjectTableId == ShipmentObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField ShipmentNumberObject1 = ObjectContext.ObjectFields.Where(d => d.FieldName == "ShipmentNumber" && d.ObjectTableId == ShipmentObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField IncotermObject1 = ObjectContext.ObjectFields.Where(d => d.FieldName == "IncotermCode" && d.ObjectTableId == ShipmentObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField HAWBObject1 = ObjectContext.ObjectFields.Where(d => d.FieldName == "House" && d.ObjectTableId == ShipmentObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField OpenDateObject1 = ObjectContext.ObjectFields.Where(d => d.FieldName == "CreateDateTime" && d.ObjectTableId == ShipmentObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField directionIdObject1 = ObjectContext.ObjectFields.Where(d => d.FieldName == "DirectionId" && d.ObjectTableId == ShipmentObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField ShipperIdObject1 = ObjectContext.ObjectFields.Where(d => d.FieldName == "ShipperName" && d.ObjectTableId == ShipmentObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField ConsigneeIdObject1 = ObjectContext.ObjectFields.Where(d => d.FieldName == "ConsigneeName" && d.ObjectTableId == ShipmentObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField entityStatusIdObject1 = ObjectContext.ObjectFields.Where(d => d.FieldName == "StatusName" && d.ObjectTableId == ShipmentObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField RoutingObject1 = ObjectContext.ObjectFields.Where(d => d.FieldName == "Routing" && d.ObjectTableId == ShipmentObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField transportModeIdObject1 = ObjectContext.ObjectFields.Where(d => d.FieldName == "TransportModeId" && d.ObjectTableId == ShipmentObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField MAWBOBLObjectfield = ObjectContext.ObjectFields.Where(d => d.FieldName == "LongMaster" && d.ObjectTableId == ShipmentObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField OperationalClosedObjectfield = ObjectContext.ObjectFields.Where(d => d.FieldName == "IsOperationalClosed" && d.ObjectTableId == ShipmentObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField AccountingClosedObjectfield = ObjectContext.ObjectFields.Where(d => d.FieldName == "IsAccountingClosed" && d.ObjectTableId == ShipmentObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField MasterTypeObject1 = ObjectContext.ObjectFields.Where(d => d.FieldName == "ShipmentTypeViewField" && d.ObjectTableId == ShipmentObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField PPCCObjectfield = ObjectContext.ObjectFields.Where(d => d.FieldName == "PPCC" && d.ObjectTableId == ShipmentObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField FlightDateObjectfield = ObjectContext.ObjectFields.Where(d => d.FieldName == "FlightDate" && d.ObjectTableId == ShipmentObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField CarrierObjectfield = ObjectContext.ObjectFields.Where(d => d.FieldName == "MainCarriageCarrierName" && d.ObjectTableId == ShipmentObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField AgentObjectfield = ObjectContext.ObjectFields.Where(d => d.FieldName == "AgentName" && d.ObjectTableId == ShipmentObject.Id && d.Tenant == 0).FirstOrDefault();

            ScreenField screenField1 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 0, ObjectFieldId = shipmentTypeObject1.Id, ScreenId = shipmentHeaderScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField screenField2 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 1, ObjectFieldId = IncotermObject1.Id, ScreenId = shipmentHeaderScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);

            ScreenField screenField3 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, Row = 0, ObjectFieldId = MAWBOBLObjectfield.Id, ScreenId = shipmentHeaderScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField screenField4 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, Row = 1, ObjectFieldId = HAWBObject1.Id, ScreenId = shipmentHeaderScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);

            ScreenField screenField5 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 2, Row = 0, ObjectFieldId = RoutingObject1.Id, ScreenId = shipmentHeaderScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField screenField6 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 2, Row = 1, ObjectFieldId = entityStatusIdObject1.Id, ScreenId = shipmentHeaderScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);

            ScreenField screenField7 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 3, Row = 0, ObjectFieldId = ShipperIdObject1.Id, ScreenId = shipmentHeaderScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField screenField8 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 3, Row = 1, ObjectFieldId = ConsigneeIdObject1.Id, ScreenId = shipmentHeaderScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);

            ScreenField screenField9 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 4, Row = 0, ObjectFieldId = OperationalClosedObjectfield.Id, ScreenId = shipmentHeaderScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField screenField10 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 4, Row = 1, ObjectFieldId = AccountingClosedObjectfield.Id, ScreenId = shipmentHeaderScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);

            ShipmentObject.HeaderScreenId = shipmentHeaderScreen.Id;
            #endregion

            #region Master Header Screen

            //Screen MasterHeaderScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "Master.HeaderScreen", Name = "Header Screen", ObjectTableId = ShipmentObject.Id, NumberOfColumns = 5, NumberOfRows = 2, IsReadOnly = true, Tenant = 0 }, ScreensRepository, tenantScreens);
            //ObjectField MasterNumberObject1 = ObjectContext.ObjectFields.Where(d => d.FieldName == "ShipmentNumber" && d.ObjectTableId == ShipmentObject.Id && d.Tenant == 0).FirstOrDefault();

            //ScreenField MasterTypefield1 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = MasterTypeObject1.Id, Row = 0, ScreenId = MasterHeaderScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            //ScreenField PPCCfield1 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = PPCCObjectfield.Id, Row = 1, ScreenId = MasterHeaderScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);

            //ScreenField MAWBOBLfield1 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = MAWBOBLObjectfield.Id, Row = 0, ScreenId = MasterHeaderScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            //ScreenField FlightDatefield1 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = FlightDateObjectfield.Id, Row = 1, ScreenId = MasterHeaderScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);

            //ScreenField Routingfield1 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 2, ObjectFieldId = RoutingObject1.Id, Row = 0, ScreenId = MasterHeaderScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            //ScreenField entityStatusIdfield1 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 2, ObjectFieldId = entityStatusIdObject1.Id, Row = 1, ScreenId = MasterHeaderScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);

            //ScreenField Carrierfield1 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 3, ObjectFieldId = CarrierObjectfield.Id, Row = 0, ScreenId = MasterHeaderScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            //ScreenField Agentfield1 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 3, ObjectFieldId = AgentObjectfield.Id, Row = 1, ScreenId = MasterHeaderScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);

            //ScreenField Operationalfield1 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 4, ObjectFieldId = OperationalClosedObjectfield.Id, Row = 0, ScreenId = MasterHeaderScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            //ScreenField Accountingfield1 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 4, ObjectFieldId = AccountingClosedObjectfield.Id, Row = 1, ScreenId = MasterHeaderScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);


            #endregion

            #region New Shipment Additional Fields
            Screen newShipmentScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "NewShipment", Name = "New Shipment", ObjectTableId = ShipmentObject.Id, NumberOfColumns = 2, NumberOfRows = 4, Tenant = 0 }, ScreensRepository, tenantScreens);
            ObjectField ObjectField1 = ObjectContext.ObjectFields.Where(d => d.FieldName == "AgentId" && d.ObjectTableId == ShipmentObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField ObjectField2 = ObjectContext.ObjectFields.Where(d => d.FieldName == "AgentReference1" && d.ObjectTableId == ShipmentObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField ObjectField3 = ObjectContext.ObjectFields.Where(d => d.FieldName == "AgentReference2" && d.ObjectTableId == ShipmentObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField ObjectField4 = ObjectContext.ObjectFields.Where(d => d.FieldName == "MainHarmonize" && d.ObjectTableId == ShipmentObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField ObjectField5 = ObjectContext.ObjectFields.Where(d => d.FieldName == "Notes" && d.ObjectTableId == ShipmentObject.Id && d.Tenant == 0).FirstOrDefault();

            ScreenField ScreenField1 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 0, ObjectFieldId = ObjectField1.Id, ScreenId = newShipmentScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField ScreenField2 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 1, ObjectFieldId = ObjectField2.Id, ScreenId = newShipmentScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField ScreenField3 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 2, ObjectFieldId = ObjectField3.Id, ScreenId = newShipmentScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField ScreenField4 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 3, ObjectFieldId = ObjectField4.Id, ScreenId = newShipmentScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField ScreenField5 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, Row = 0, ObjectFieldId = ObjectField5.Id, ScreenId = newShipmentScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);

            #endregion

            #region Customs Tab Additional Fields
            Screen aditionalFieldsScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "Shipment.CustomsAdditionalFields", Name = "Customs Additional Fields", ObjectTableId = ShipmentObject.Id, NumberOfColumns = 2, NumberOfRows = 1, Tenant = 0 }, ScreensRepository, tenantScreens);
            #endregion

            #region Agent Shared Manifest  Additional Fields
            Screen AgentSharedManifestAdditionalScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "SharedManifestAdditionalScreen", Name = "Shared Manifest Additional Fields", ObjectTableId = ShipmentObject.Id, NumberOfColumns = 2, NumberOfRows = 4, Tenant = 0 }, ScreensRepository, tenantScreens);


            #endregion

            #region Shipping Declaration  Additional Fields
            Screen ShippingDeclarationAdditionalScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "ShippingDeclarationAdditionalScreen", Name = "Shipping Declaration Additional Fields", ObjectTableId = ShipmentObject.Id, NumberOfColumns = 1, NumberOfRows = 4, Tenant = 0 }, ScreensRepository, tenantScreens);

            ObjectField ShippingDeclarationObjectField1 = ObjectContext.ObjectFields.Where(d => d.FieldName == "INTTRADocumentTypeCode" && d.ObjectTableId == ShipmentObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField ShippingDeclarationObjectField2 = ObjectContext.ObjectFields.Where(d => d.FieldName == "INTTRADocumentQTY" && d.ObjectTableId == ShipmentObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField ShippingDeclarationObjectField3 = ObjectContext.ObjectFields.Where(d => d.FieldName == "SIHasAttachList" && d.ObjectTableId == ShipmentObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField ShippingDeclarationObjectField4 = ObjectContext.ObjectFields.Where(d => d.FieldName == "INTTRAInstructions" && d.ObjectTableId == ShipmentObject.Id && d.Tenant == 0).FirstOrDefault();
        
            ScreenField ShippingDeclarationScreenField1 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 0, ObjectFieldId = ShippingDeclarationObjectField1.Id, ScreenId = ShippingDeclarationAdditionalScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField ShippingDeclarationScreenField2 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 1, ObjectFieldId = ShippingDeclarationObjectField2.Id, ScreenId = ShippingDeclarationAdditionalScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField ShippingDeclarationScreenField3 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 2, ObjectFieldId = ShippingDeclarationObjectField3.Id, ScreenId = ShippingDeclarationAdditionalScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField ShippingDeclarationScreenField4 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 3, ObjectFieldId = ShippingDeclarationObjectField4.Id, ScreenId = ShippingDeclarationAdditionalScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
          
            #endregion

            #region Genral Tab Screen
            ObjectField ShipmentTypeObject = ObjectContext.ObjectFields.Where(d => d.FieldName == "ShipmentTypeId" && d.ObjectTableId == ShipmentObject.Id).FirstOrDefault();
            ObjectField HAWBObject = ObjectContext.ObjectFields.Where(d => d.FieldName == "House" && d.ObjectTableId == ShipmentObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField HAWBDateObject = ObjectContext.ObjectFields.Where(d => d.FieldName == "HAWBDate" && d.ObjectTableId == ShipmentObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField IncotermsObject = ObjectContext.ObjectFields.Where(d => d.FieldName == "IncotermId" && d.ObjectTableId == ShipmentObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField FreightPrepaidCollectObject = ObjectContext.ObjectFields.Where(d => d.FieldName == "FreightPrepaidCollectId" && d.ObjectTableId == ShipmentObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField OtherPrepaidCollectObject = ObjectContext.ObjectFields.Where(d => d.FieldName == "OtherPrepaidCollectId" && d.ObjectTableId == ShipmentObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField SalesManUserObject = ObjectContext.ObjectFields.Where(d => d.FieldName == "SalesmanUserId" && d.ObjectTableId == ShipmentObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField OpenedByUserObject = ObjectContext.ObjectFields.Where(d => d.FieldName == "CreatedByUserId" && d.ObjectTableId == ShipmentObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField DepartmentObject = ObjectContext.ObjectFields.Where(d => d.FieldName == "DepartmentId" && d.ObjectTableId == ShipmentObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField QuoteTemplateObject = ObjectContext.ObjectFields.Where(d => d.FieldName == "QuoteTemplateId" && d.ObjectTableId == ShipmentObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField BranchObject = ObjectContext.ObjectFields.Where(d => d.FieldName == "BranchId" && d.ObjectTableId == ShipmentObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField MainHarmonizeObject = ObjectContext.ObjectFields.Where(d => d.FieldName == "MainHarmonize" && d.ObjectTableId == ShipmentObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField MoveTypeObject = ObjectContext.ObjectFields.Where(d => d.FieldName == "MoveTypeId" && d.ObjectTableId == ShipmentObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField AMSBLObject = ObjectContext.ObjectFields.Where(d => d.FieldName == "AMSBL" && d.ObjectTableId == ShipmentObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField SpecialServicesTypeObject = ObjectContext.ObjectFields.Where(d => d.FieldName == "SpecialServicesTypeId" && d.ObjectTableId == ShipmentObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField AccountManagerUserObject = ObjectContext.ObjectFields.Where(d => d.FieldName == "AccountManagerUserId" && d.ObjectTableId == ShipmentObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField ValueOfGoodsObject = ObjectContext.ObjectFields.Where(d => d.FieldName == "ValueOfGoods" && d.ObjectTableId == ShipmentObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField ValueOfGoodsCurrencyObject = ObjectContext.ObjectFields.Where(d => d.FieldName == "ValueOfGoodsCurrencyId" && d.ObjectTableId == ShipmentObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField ProjectNumberObject = ObjectContext.ObjectFields.Where(d => d.FieldName == "ProjectNumber" && d.ObjectTableId == ShipmentObject.Id && d.Tenant == 0).FirstOrDefault();

            Screen generalTabScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "Shipment.GeneralTabScreen", Name = "General Tab Screen", ObjectTableId = ShipmentObject.Id, NumberOfColumns = 2, NumberOfRows = 10, Tenant = 0 }, ScreensRepository, tenantScreens);
            /*Column[0]*/
            ScreenField HAWBfield = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 0, Column = 0, ObjectFieldId = HAWBObject.Id, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField HAWBDatefield = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 1, Column = 0, ObjectFieldId = HAWBDateObject.Id, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField AMSBLField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 2, Column = 0, ObjectFieldId = AMSBLObject.Id, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField Incotermsfield = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 3, Column = 0, ObjectFieldId = IncotermsObject.Id, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField MoveTypefield = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 4, Column = 0, ObjectFieldId = MoveTypeObject.Id, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField FreightPrepaidCollectfield = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 5, Column = 0, ObjectFieldId = FreightPrepaidCollectObject.Id, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField OtherPrepaidCollectfield = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 6, Column = 0, ObjectFieldId = OtherPrepaidCollectObject.Id, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField MainHarmonizefield = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 7, Column = 0, ObjectFieldId = MainHarmonizeObject.Id, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField ProjectNumberfield = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 8, Column = 0, ObjectFieldId = ProjectNumberObject.Id, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);

            /*Column[1]*/
            ScreenField OpenedByUserfield = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 0, Column = 1, ObjectFieldId = OpenedByUserObject.Id, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField SalesManUserfield = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 1, Column = 1, ObjectFieldId = SalesManUserObject.Id, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField AccountManagerUserfield = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 2, Column = 1, ObjectFieldId = AccountManagerUserObject.Id, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField Departmentfield = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 3, Column = 1, ObjectFieldId = DepartmentObject.Id, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField Branchfield = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 4, Column = 1, ObjectFieldId = BranchObject.Id, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField SpecialServicesTypefield = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 5, Column = 1, ObjectFieldId = SpecialServicesTypeObject.Id, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField ValueOfGoodsfield = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 6, Column = 1, ObjectFieldId = ValueOfGoodsObject.Id, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField ValueOfGoodsCurrencyfield = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 7, Column = 1, ObjectFieldId = ValueOfGoodsCurrencyObject.Id, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);

            #endregion

            #region Master Genral Tab Screen

            Screen masterGeneralTabScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "Master.GeneralTabScreen", Name = "General Tab Screen", ObjectTableId = ShipmentObject.Id, NumberOfColumns = 2, NumberOfRows = 9, Tenant = 0 }, ScreensRepository, tenantScreens);
            /*Column[0]*/
            ScreenField FreightPrepaidCollectfield2 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 0, Column = 0, ObjectFieldId = FreightPrepaidCollectObject.Id, ScreenId = masterGeneralTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField OtherPrepaidCollectfield2 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 1, Column = 0, ObjectFieldId = OtherPrepaidCollectObject.Id, ScreenId = masterGeneralTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField MoveTypefield2 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 2, Column = 0, ObjectFieldId = MoveTypeObject.Id, ScreenId = masterGeneralTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField AMSBLField2 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 3, Column = 0, ObjectFieldId = AMSBLObject.Id, ScreenId = masterGeneralTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField MasterValueOfGoodsfield = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 4, Column = 0, ObjectFieldId = ValueOfGoodsObject.Id, ScreenId = masterGeneralTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField MasterValueOfGoodsCurrencyfield = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 5, Column = 0, ObjectFieldId = ValueOfGoodsCurrencyObject.Id, ScreenId = masterGeneralTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField MasterProjectNumberfield = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 6, Column = 0, ObjectFieldId = ProjectNumberObject.Id, ScreenId = masterGeneralTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);

            /*Column[1]*/
            ScreenField OpenedByUserfield2 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 0, Column = 1, ObjectFieldId = OpenedByUserObject.Id, ScreenId = masterGeneralTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField Departmentfield2 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 1, Column = 1, ObjectFieldId = DepartmentObject.Id, ScreenId = masterGeneralTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField Branchfield2 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 2, Column = 1, ObjectFieldId = BranchObject.Id, ScreenId = masterGeneralTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            #endregion

            ObjectField MAWBOBLObject = ObjectContext.ObjectFields.Where(d => d.FieldName == "Master" && d.ObjectTableId == ShipmentObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField MainCarriageCarrierObject = ObjectContext.ObjectFields.Where(d => d.FieldName == "MainCarriageCarrierId" && d.ObjectTableId == ShipmentObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField MainCarriageCarrierNumberObject = ObjectContext.ObjectFields.Where(d => d.FieldName == "MainCarriageCarrierNumber" && d.ObjectTableId == ShipmentObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField MainCarriageETDObject = ObjectContext.ObjectFields.Where(d => d.FieldName == "MainCarriageETD" && d.ObjectTableId == ShipmentObject.Id && d.Tenant == 0).FirstOrDefault();

            Screen statusScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "Shipment.StatusScreen", Name = "Status Screen", ObjectTableId = ShipmentObject.Id, NumberOfColumns = 1, NumberOfRows = 1 }, ScreensRepository, tenantScreens);
            
            /* Client Add,Edit */
            ObjectField ShipperId = ObjectContext.ObjectFields.Where(d => d.FieldName == "ShipperId" && d.ObjectTableId == ShipmentObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField ShipperAddressId = ObjectContext.ObjectFields.Where(d => d.FieldName == "ShipperAddressId" && d.ObjectTableId == ShipmentObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField ShipperContactId = ObjectContext.ObjectFields.Where(d => d.FieldName == "ShipperContactId" && d.ObjectTableId == ShipmentObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField ShipperReference1 = ObjectContext.ObjectFields.Where(d => d.FieldName == "ShipperReference1" && d.ObjectTableId == ShipmentObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField ShipperReference2 = ObjectContext.ObjectFields.Where(d => d.FieldName == "ShipperReference2" && d.ObjectTableId == ShipmentObject.Id && d.Tenant == 0).FirstOrDefault();

            Screen ClientAddEditScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "Shipment.CustomerAddEditScreen", Name = "Customer Add Edit Screen", ObjectTableId = ShipmentObject.Id, NumberOfColumns = 2, NumberOfRows = 6 }, ScreensRepository, tenantScreens);
            ScreenField ShipperIdField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 0, Column = 0, ObjectFieldId = ShipperId.Id, ScreenId = ClientAddEditScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField ShipperReference1Field = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 1, Column = 0, ObjectFieldId = ShipperReference1.Id, ScreenId = ClientAddEditScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField ShipperAddressIdField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 2, Column = 0, ObjectFieldId = ShipperAddressId.Id, ScreenId = ClientAddEditScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField ShipperReference2Field = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 1, Column = 1, ObjectFieldId = ShipperReference2.Id, ScreenId = ClientAddEditScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField ShipperContactIdField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 2, Column = 1, ObjectFieldId = ShipperContactId.Id, ScreenId = ClientAddEditScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            
            if (Testing.General.IsTesting)
            {
                //============= Just For Testing ============= 
                Screen TestScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "Shipment.TestScreen", ObjectTableId = ShipmentObject.Id, NumberOfColumns = 2, NumberOfRows = 6 }, ScreensRepository, tenantScreens);
                ScreenField TestScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 0, Column = 0, ObjectFieldId = ShipperId.Id, ScreenId = ClientAddEditScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);

                //Update
                Screen TestUpdateHeaderScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "Shipment.HeaderScreen", Name = "Header Screen", ObjectTableId = ShipmentObject.Id, NumberOfColumns = 100, NumberOfRows = 3, IsReadOnly = true, Tenant = 0 }, ScreensRepository, tenantScreens);
                //============================================
            }
            ObjectContext.SaveChanges();
        }

        public void BuildMasterScreens(Dictionary<string, Screen> tenantScreens, Dictionary<string, ScreenField> tenantScreenFields)
        {
            ObjectTable MasterObject = ObjectContext.ObjectTables.Where(d => d.Name == "Master" && d.Tenant == 0).FirstOrDefault();

            //#region Header screen
            //Screen MasterHeaderScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "Master.HeaderScreen", Name = "Header Screen", ObjectTableId = MasterObject.Id, NumberOfColumns = 5, NumberOfRows = 2, IsReadOnly = true, Tenant = 0 }, ScreensRepository, tenantScreens);

            //ObjectField MasterNumberObject1 = ObjectContext.ObjectFields.Where(d => d.FieldName == "ShipmentNumber" && d.ObjectTableId == MasterObject.Id && d.Tenant == 0).FirstOrDefault();
            //ObjectField OpenDateObject1 = ObjectContext.ObjectFields.Where(d => d.FieldName == "CreateDateTime" && d.ObjectTableId == MasterObject.Id && d.Tenant == 0).FirstOrDefault();
            //ObjectField MasterTypeObject1 = ObjectContext.ObjectFields.Where(d => d.FieldName == "ShipmentTypeViewField" && d.ObjectTableId == MasterObject.Id && d.Tenant == 0).FirstOrDefault();
            //ObjectField directionIdObject1 = ObjectContext.ObjectFields.Where(d => d.FieldName == "DirectionId" && d.ObjectTableId == MasterObject.Id && d.Tenant == 0).FirstOrDefault();
            //ObjectField ShipperIdObject1 = ObjectContext.ObjectFields.Where(d => d.FieldName == "ShipperName" && d.ObjectTableId == MasterObject.Id && d.Tenant == 0).FirstOrDefault();
            //ObjectField entityStatusIdObject1 = ObjectContext.ObjectFields.Where(d => d.FieldName == "StatusName" && d.ObjectTableId == MasterObject.Id && d.Tenant == 0).FirstOrDefault();
            //ObjectField transportModeIdObject1 = ObjectContext.ObjectFields.Where(d => d.FieldName == "TransportModeId" && d.ObjectTableId == MasterObject.Id && d.Tenant == 0).FirstOrDefault();
            //ObjectField MAWBOBLObjectfield = ObjectContext.ObjectFields.Where(d => d.FieldName == "LongMaster" && d.ObjectTableId == MasterObject.Id && d.Tenant == 0).FirstOrDefault();
            //ObjectField PPCCObjectfield = ObjectContext.ObjectFields.Where(d => d.FieldName == "PPCC" && d.ObjectTableId == MasterObject.Id && d.Tenant == 0).FirstOrDefault();
            //ObjectField FlightDateObjectfield = ObjectContext.ObjectFields.Where(d => d.FieldName == "FlightDate" && d.ObjectTableId == MasterObject.Id && d.Tenant == 0).FirstOrDefault();
            //ObjectField RoutingObjectfield = ObjectContext.ObjectFields.Where(d => d.FieldName == "Routing" && d.ObjectTableId == MasterObject.Id && d.Tenant == 0).FirstOrDefault();
            //ObjectField CarrierObjectfield = ObjectContext.ObjectFields.Where(d => d.FieldName == "MainCarriageCarrierName" && d.ObjectTableId == MasterObject.Id && d.Tenant == 0).FirstOrDefault();
            //ObjectField AgentObjectfield = ObjectContext.ObjectFields.Where(d => d.FieldName == "AgentName" && d.ObjectTableId == MasterObject.Id && d.Tenant == 0).FirstOrDefault();
            //ObjectField OperationalClosedObjectfield = ObjectContext.ObjectFields.Where(d => d.FieldName == "IsOperationalClosed" && d.ObjectTableId == MasterObject.Id && d.Tenant == 0).FirstOrDefault();
            //ObjectField AccountingClosedObjectfield = ObjectContext.ObjectFields.Where(d => d.FieldName == "IsAccountingClosed" && d.ObjectTableId == MasterObject.Id && d.Tenant == 0).FirstOrDefault();

            //ScreenField MasterTypefield1 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = MasterTypeObject1.Id, Row = 0, ScreenId = MasterHeaderScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            //ScreenField PPCCfield1 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = PPCCObjectfield.Id, Row = 1, ScreenId = MasterHeaderScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);

            //ScreenField MAWBOBLfield1 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = MAWBOBLObjectfield.Id, Row = 0, ScreenId = MasterHeaderScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            //ScreenField FlightDatefield1 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = FlightDateObjectfield.Id, Row = 1, ScreenId = MasterHeaderScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);

            //ScreenField Routingfield1 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 2, ObjectFieldId = RoutingObjectfield.Id, Row = 0, ScreenId = MasterHeaderScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            //ScreenField entityStatusIdfield1 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 2, ObjectFieldId = entityStatusIdObject1.Id, Row = 1, ScreenId = MasterHeaderScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);

            //ScreenField Carrierfield1 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 3, ObjectFieldId = CarrierObjectfield.Id, Row = 0, ScreenId = MasterHeaderScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            //ScreenField Agentfield1 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 3, ObjectFieldId = AgentObjectfield.Id, Row = 1, ScreenId = MasterHeaderScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);

            //ScreenField Operationalfield1 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 4, ObjectFieldId = OperationalClosedObjectfield.Id, Row = 0, ScreenId = MasterHeaderScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            //ScreenField Accountingfield1 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 4, ObjectFieldId = AccountingClosedObjectfield.Id, Row = 1, ScreenId = MasterHeaderScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);

            //MasterObject.HeaderScreenId = MasterHeaderScreen.Id;
            //#endregion

            #region New Master Additional Fields
            Screen newMasterScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "NewMaster", Name = "New Master", ObjectTableId = MasterObject.Id, NumberOfColumns = 1, NumberOfRows = 3, Tenant = 0 }, ScreensRepository, tenantScreens);
            ObjectField ObjectField1 = ObjectContext.ObjectFields.Where(d => d.FieldName == "AgentId" && d.ObjectTableId == MasterObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField ObjectField2 = ObjectContext.ObjectFields.Where(d => d.FieldName == "AgentReference1" && d.ObjectTableId == MasterObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField ObjectField3 = ObjectContext.ObjectFields.Where(d => d.FieldName == "AgentReference2" && d.ObjectTableId == MasterObject.Id && d.Tenant == 0).FirstOrDefault();
            //ObjectField ObjectField4 = ObjectContext.ObjectFields.Where(d => d.FieldName == "MainHarmonize" && d.ObjectTableId == MasterObject.Id && d.Tenant == 0).FirstOrDefault();

            ScreenField ScreenField1 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 0, ObjectFieldId = ObjectField1.Id, ScreenId = newMasterScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField ScreenField2 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 1, ObjectFieldId = ObjectField2.Id, ScreenId = newMasterScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField ScreenField3 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 2, ObjectFieldId = ObjectField3.Id, ScreenId = newMasterScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            //ScreenField ScreenField4 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 3, ObjectFieldId = ObjectField4.Id, ScreenId = newMasterScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            #endregion

            //#region Genral Tab Screen
            //ObjectField MasterTypeObject = ObjectContext.ObjectFields.Where(d => d.FieldName == "ShipmentTypeId" && d.ObjectTableId == MasterObject.Id).FirstOrDefault();
            ////ObjectField HAWBObject = ObjectContext.ObjectFields.Where(d => d.FieldName == "House" && d.ObjectTableId == MasterObject.Id && d.Tenant == 0).FirstOrDefault();
            ////ObjectField HAWBDateObject = ObjectContext.ObjectFields.Where(d => d.FieldName == "HAWBDate" && d.ObjectTableId == MasterObject.Id && d.Tenant == 0).FirstOrDefault();
            ////ObjectField IncotermsObject = ObjectContext.ObjectFields.Where(d => d.FieldName == "IncotermId" && d.ObjectTableId == MasterObject.Id && d.Tenant == 0).FirstOrDefault();
            //ObjectField FreightPrepaidCollectObject = ObjectContext.ObjectFields.Where(d => d.FieldName == "FreightPrepaidCollectId" && d.ObjectTableId == MasterObject.Id && d.Tenant == 0).FirstOrDefault();
            //ObjectField OtherPrepaidCollectObject = ObjectContext.ObjectFields.Where(d => d.FieldName == "OtherPrepaidCollectId" && d.ObjectTableId == MasterObject.Id && d.Tenant == 0).FirstOrDefault();
            ////ObjectField SalesManUserObject = ObjectContext.ObjectFields.Where(d => d.FieldName == "SalesmanUserId" && d.ObjectTableId == MasterObject.Id && d.Tenant == 0).FirstOrDefault();
            //ObjectField OpenedByUserObject = ObjectContext.ObjectFields.Where(d => d.FieldName == "CreatedByUserId" && d.ObjectTableId == MasterObject.Id && d.Tenant == 0).FirstOrDefault();
            //ObjectField DepartmentObject = ObjectContext.ObjectFields.Where(d => d.FieldName == "DepartmentId" && d.ObjectTableId == MasterObject.Id && d.Tenant == 0).FirstOrDefault();
            //ObjectField BranchObject = ObjectContext.ObjectFields.Where(d => d.FieldName == "BranchId" && d.ObjectTableId == MasterObject.Id && d.Tenant == 0).FirstOrDefault();
            ////ObjectField MainHarmonizeObject = ObjectContext.ObjectFields.Where(d => d.FieldName == "MainHarmonize" && d.ObjectTableId == MasterObject.Id && d.Tenant == 0).FirstOrDefault();
            ////ObjectField NotesObject = ObjectContext.ObjectFields.Where(d => d.FieldName == "Notes" && d.ObjectTableId == MasterObject.Id && d.Tenant == 0).FirstOrDefault();
            ////ObjectField StatusField = ObjectContext.ObjectFields.Where(d => d.FieldName == "StatusId" && d.ObjectTableId == MasterObject.Id && d.Tenant == 0).FirstOrDefault();
            ////ObjectField LeadingCurrencyField = ObjectContext.ObjectFields.Where(d => d.FieldName == "LeadingCurrencyId" && d.ObjectTableId == MasterObject.Id).FirstOrDefault();

            //Screen generalTabScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "Master.GeneralTabScreen", Name = "General Tab Screen", ObjectTableId = MasterObject.Id, NumberOfColumns = 2, NumberOfRows = 9, Tenant = 0 }, ScreensRepository, tenantScreens);
            ///*Column[0]*/
            ////ScreenField MasterTypefield = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 0, Column = 0, ObjectFieldId = MasterTypeObject.Id, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ////ScreenField HAWBfield = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 1, Column = 0, ObjectFieldId = HAWBObject.Id, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ////ScreenField HAWBDatefield = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 2, Column = 0, ObjectFieldId = HAWBDateObject.Id, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ////ScreenField Statusfield = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 3, Column = 0, ObjectFieldId = StatusField.Id, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ////ScreenField Incotermsfield = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 1, Column = 0, ObjectFieldId = IncotermsObject.Id, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            //ScreenField FreightPrepaidCollectfield = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 0, Column = 0, ObjectFieldId = FreightPrepaidCollectObject.Id, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            //ScreenField OtherPrepaidCollectfield = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 1, Column = 0, ObjectFieldId = OtherPrepaidCollectObject.Id, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ////ScreenField SalesManUserfield = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 6, Column = 0, ObjectFieldId = SalesManUserObject.Id, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ////ScreenField MainHarmonizefield = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 7, Column = 0, ObjectFieldId = MainHarmonizeObject.Id, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);

            ///*Column[1]*/
            //ScreenField OpenedByUserfield = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 2, Column = 0, ObjectFieldId = OpenedByUserObject.Id, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            //ScreenField Departmentfield = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 3, Column = 0, ObjectFieldId = DepartmentObject.Id, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            //ScreenField Branchfield = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 4, Column = 0, ObjectFieldId = BranchObject.Id, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ////ScreenField LeadingCurrency = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 3, Column = 1, ObjectFieldId = LeadingCurrencyField.Id, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);           
            ////ScreenField Notesfield = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 0, Column = 1, ObjectFieldId = NotesObject.Id, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);

            ////ScreensRepository.Add(generalTabScreen);
            ////ScreenFieldsRepository.Add(MasterTypefield);
            ////ScreenFieldsRepository.Add(HAWBfield);
            ////ScreenFieldsRepository.Add(HAWBDatefield);
            ////ScreenFieldsRepository.Add(Incotermsfield);
            ////ScreenFieldsRepository.Add(FreightPrepaidCollectfield);
            ////ScreenFieldsRepository.Add(OtherPrepaidCollectfield); 
            ////ScreenFieldsRepository.Add(SalesManUserfield);
            ////ScreenFieldsRepository.Add(OpenedByUserfield);
            ////ScreenFieldsRepository.Add(Departmentfield);
            ////ScreenFieldsRepository.Add(Branchfield);
            ////ScreenFieldsRepository.Add(MainHarmonizefield);
            ////ScreenFieldsRepository.Add(Notesfield);
            ////ScreenFieldsRepository.Add(Statusfield);
            ////ScreenFieldsRepository.Add(LeadingCurrency);
            //#endregion

            //-------------------------------------------------------
            //Screen picDScreen = new Screen() { Code = "Master.PicDScreen", Id = IdCounter.GetNumber().ToString(), ObjectTableId = MasterObject.Id, NumberOfColumns = 2, NumberOfRows = 3 };
            //ObjectField PickUpFromAddressIdObject = ObjectContext.ObjectFields.Where(d => d.FieldName == "PickUpFromAddressId" && d.ObjectTableId == MasterObject.Id).FirstOrDefault();
            //ObjectField PickUpToPortIdObject = ObjectContext.ObjectFields.Where(d => d.FieldName == "PickUpToPortId" && d.ObjectTableId == MasterObject.Id).FirstOrDefault();
            //ObjectField PickUpATDObject = ObjectContext.ObjectFields.Where(d => d.FieldName == "PickUpATD" && d.ObjectTableId == MasterObject.Id).FirstOrDefault();
            //ObjectField PickUpATAObject = ObjectContext.ObjectFields.Where(d => d.FieldName == "PickUpATA" && d.ObjectTableId == MasterObject.Id).FirstOrDefault();
            //ObjectField PickUpCarrierIdObject = ObjectContext.ObjectFields.Where(d => d.FieldName == "PickUpCarrierId" && d.ObjectTableId == MasterObject.Id).FirstOrDefault();
            //ObjectField PickUpCarrierNumberObject = ObjectContext.ObjectFields.Where(d => d.FieldName == "PickUpCarrierNumber" && d.ObjectTableId == MasterObject.Id).FirstOrDefault();
            ////ObjectField PickUpFromPortIdObject = ObjectContext.ObjectFields.Where(d => d.FieldName == "PickUpFromPortId").FirstOrDefault();
            ////ObjectField PickUpToAddressIdObject = ObjectContext.ObjectFields.Where(d => d.FieldName == "PickUpToAddressId").FirstOrDefault();

            //ScreenField PickUpFromAddressField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = PickUpFromAddressIdObject.Id, Row = 0, ScreenId = picDScreen.Id, Tenant = 0, Id = IdCounter.GetNumber().ToString() };
            //ScreenField PickUpToPortIdField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = PickUpToPortIdObject.Id, Row = 0, ScreenId = picDScreen.Id, Tenant = 0, Id = IdCounter.GetNumber().ToString() };
            //ScreenField PickUpATDField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = PickUpATDObject.Id, Row = 1, ScreenId = picDScreen.Id, Tenant = 0, Id = IdCounter.GetNumber().ToString() };
            //ScreenField PickUpATAField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = PickUpATAObject.Id, Row = 1, ScreenId = picDScreen.Id, Tenant = 0, Id = IdCounter.GetNumber().ToString() };
            //ScreenField PickUpCarrierIdField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = PickUpCarrierIdObject.Id, Row = 2, ScreenId = picDScreen.Id, Tenant = 0, Id = IdCounter.GetNumber().ToString() };
            //ScreenField PickUpCarrierNumberField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = PickUpCarrierNumberObject.Id, Row = 2, ScreenId = picDScreen.Id, Tenant = 0, Id = IdCounter.GetNumber().ToString() };
            //ScreensRepository.Add(picDScreen);
            //ScreenFieldsRepository.Add(PickUpFromAddressField);
            //ScreenFieldsRepository.Add(PickUpToPortIdField);
            //ScreenFieldsRepository.Add(PickUpATDField);
            //ScreenFieldsRepository.Add(PickUpATAField);
            //ScreenFieldsRepository.Add(PickUpCarrierIdField);
            //ScreenFieldsRepository.Add(PickUpCarrierNumberField);

            //-------------------------------------------------------
            //Screen picAScreen = new Screen() { Code = "Master.PicAScreen", Id = IdCounter.GetNumber().ToString(), ObjectTableId = MasterObject.Id, NumberOfColumns = 2, NumberOfRows = 3 };
            //ScreenField PickUpFromAddressField2 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = PickUpFromAddressIdObject.Id, Row = 0, ScreenId = picAScreen.Id, Tenant = 0, Id = IdCounter.GetNumber().ToString() };
            //ScreenField PickUpToPortIdField2 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = PickUpToPortIdObject.Id, Row = 0, ScreenId = picAScreen.Id, Tenant = 0, Id = IdCounter.GetNumber().ToString() };
            //ScreenField PickUpATDField2 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = PickUpATDObject.Id, Row = 1, ScreenId = picAScreen.Id, Tenant = 0, Id = IdCounter.GetNumber().ToString() };
            //ScreenField PickUpATAField2 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = PickUpATAObject.Id, Row = 1, ScreenId = picAScreen.Id, Tenant = 0, Id = IdCounter.GetNumber().ToString() };
            //ScreenField PickUpCarrierIdField2 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = PickUpCarrierIdObject.Id, Row = 2, ScreenId = picAScreen.Id, Tenant = 0, Id = IdCounter.GetNumber().ToString() };
            //ScreenField PickUpCarrierNumberField2 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = PickUpCarrierNumberObject.Id, Row = 2, ScreenId = picAScreen.Id, Tenant = 0, Id = IdCounter.GetNumber().ToString() };
            //ScreensRepository.Add(picAScreen);
            //ScreenFieldsRepository.Add(PickUpFromAddressField2);
            //ScreenFieldsRepository.Add(PickUpToPortIdField2);
            //ScreenFieldsRepository.Add(PickUpATDField2);
            //ScreenFieldsRepository.Add(PickUpATAField2);
            //ScreenFieldsRepository.Add(PickUpCarrierIdField2);
            //ScreenFieldsRepository.Add(PickUpCarrierNumberField2);

            //-------------------------------------------------------

            ObjectField MAWBOBLObject = ObjectContext.ObjectFields.Where(d => d.FieldName == "Master" && d.ObjectTableId == MasterObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField MainCarriageCarrierObject = ObjectContext.ObjectFields.Where(d => d.FieldName == "MainCarriageCarrierId" && d.ObjectTableId == MasterObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField MainCarriageCarrierNumberObject = ObjectContext.ObjectFields.Where(d => d.FieldName == "MainCarriageCarrierNumber" && d.ObjectTableId == MasterObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField MainCarriageETDObject = ObjectContext.ObjectFields.Where(d => d.FieldName == "MainCarriageETD" && d.ObjectTableId == MasterObject.Id && d.Tenant == 0).FirstOrDefault();

            Screen statusScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "Master.StatusScreen", Name = "Status Screen", ObjectTableId = MasterObject.Id, NumberOfColumns = 1, NumberOfRows = 1 }, ScreensRepository, tenantScreens);

            Screen remarkScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "Master.RemarkScreen", Name = "Remark Screen", ObjectTableId = MasterObject.Id, NumberOfColumns = 1, NumberOfRows = 1 }, ScreensRepository, tenantScreens);


            Screen preAlertScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "Master.PreAlert", Name = "Pre Alert", ObjectTableId = MasterObject.Id, NumberOfColumns = 2, NumberOfRows = 3 }, ScreensRepository, tenantScreens);


            //ScreensRepository.Add(statusScreen);
            //ScreensRepository.Add(preAlertScreen);
            //ScreensRepository.Add(remarkScreen);

            //ScreenField HAWBField2 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = HAWBObject.Id, Row = 0, ScreenId = preAlertScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField MAWBOBLField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = MAWBOBLObject.Id, Row = 0, ScreenId = preAlertScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField MainCarriageCarrierField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = MainCarriageCarrierObject.Id, Row = 0, ScreenId = preAlertScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField MainCarriageCarrierNumberField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = MainCarriageCarrierNumberObject.Id, Row = 1, ScreenId = preAlertScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField MainCarriageETDField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = MainCarriageETDObject.Id, Row = 1, ScreenId = preAlertScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);

            //ScreenFieldsRepository.Add(HAWBField2);
            //ScreenFieldsRepository.Add(MAWBOBLField);
            //ScreenFieldsRepository.Add(MainCarriageCarrierField);
            //ScreenFieldsRepository.Add(MainCarriageCarrierNumberField);
            //ScreenFieldsRepository.Add(MainCarriageETDField);

            /* Booking Tab */
            //ObjectField BookingContainer20Object = ObjectContext.ObjectFields.Where(d => d.FieldName == "BookingContainer20" && d.ObjectTableId == MasterObject.Id).FirstOrDefault();
            //ObjectField BookingContainer40Object = ObjectContext.ObjectFields.Where(d => d.FieldName == "BookingContainer40" && d.ObjectTableId == MasterObject.Id).FirstOrDefault();
            //ObjectField BookingGrossWeightObject = ObjectContext.ObjectFields.Where(d => d.FieldName == "BookingGrossWeight" && d.ObjectTableId == MasterObject.Id).FirstOrDefault();
            //ObjectField BookingVolumeObject = ObjectContext.ObjectFields.Where(d => d.FieldName == "BookingVolume" && d.ObjectTableId == MasterObject.Id).FirstOrDefault();
            //ObjectField BookingNumberOfPackagesObject = ObjectContext.ObjectFields.Where(d => d.FieldName == "BookingNumberOfPackages" && d.ObjectTableId == MasterObject.Id).FirstOrDefault();
            //ObjectField BookingDangerousGoodsObject = ObjectContext.ObjectFields.Where(d => d.FieldName == "BookingDangerousGoods" && d.ObjectTableId == MasterObject.Id).FirstOrDefault();
            //ObjectField BookingDescriptionOfGoodsObject = ObjectContext.ObjectFields.Where(d => d.FieldName == "DescriptionOfGoods" && d.ObjectTableId == MasterObject.Id).FirstOrDefault();


            //Screen BookingScreen = new Screen() { Code = "Master.BookingTabScreen", Id = IdCounter.GetNumber().ToString(), ObjectTableId = MasterObject.Id, NumberOfColumns = 2, NumberOfRows = 6 };

            //ScreenField BookingNumberOfPackagesField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 0, Column = 0, ObjectFieldId = BookingNumberOfPackagesObject.Id, ScreenId = BookingScreen.Id, Tenant = 0, Id = IdCounter.GetNumber().ToString() };
            //ScreenField BookingContainer20Field = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 1, Column = 0, ObjectFieldId = BookingContainer20Object.Id, ScreenId = BookingScreen.Id, Tenant = 0, Id = IdCounter.GetNumber().ToString() };
            //ScreenField BookingContainer40Field = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 2, Column = 0, ObjectFieldId = BookingContainer40Object.Id, ScreenId = BookingScreen.Id, Tenant = 0, Id = IdCounter.GetNumber().ToString() };
            //ScreenField BookingGrossWeightField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 3, Column = 0, ObjectFieldId = BookingGrossWeightObject.Id, ScreenId = BookingScreen.Id, Tenant = 0, Id = IdCounter.GetNumber().ToString() };
            //ScreenField BookingVolumeField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 4, Column = 0, ObjectFieldId = BookingVolumeObject.Id, ScreenId = BookingScreen.Id, Tenant = 0, Id = IdCounter.GetNumber().ToString() };
            //ScreenField BookingDangerousGoodsField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 5, Column = 0, ObjectFieldId = BookingDangerousGoodsObject.Id, ScreenId = BookingScreen.Id, Tenant = 0, Id = IdCounter.GetNumber().ToString() };
            //ScreenField BookingDescriptionOfGoodsField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 0, Column = 1, ObjectFieldId = BookingDescriptionOfGoodsObject.Id, ScreenId = BookingScreen.Id, Tenant = 0, Id = IdCounter.GetNumber().ToString() };            


            //ScreensRepository.Add(BookingScreen);
            //ScreenFieldsRepository.Add(BookingNumberOfPackagesField);
            //ScreenFieldsRepository.Add(BookingContainer20Field);
            //ScreenFieldsRepository.Add(BookingContainer40Field);
            //ScreenFieldsRepository.Add(BookingGrossWeightField);
            //ScreenFieldsRepository.Add(BookingVolumeField);            
            //ScreenFieldsRepository.Add(BookingDangerousGoodsField);
            //ScreenFieldsRepository.Add(BookingDescriptionOfGoodsField);

            /* Client Add,Edit */
            ObjectField ShipperId = ObjectContext.ObjectFields.Where(d => d.FieldName == "ShipperId" && d.ObjectTableId == MasterObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField ShipperAddressId = ObjectContext.ObjectFields.Where(d => d.FieldName == "ShipperAddressId" && d.ObjectTableId == MasterObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField ShipperContactId = ObjectContext.ObjectFields.Where(d => d.FieldName == "ShipperContactId" && d.ObjectTableId == MasterObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField ShipperReference1 = ObjectContext.ObjectFields.Where(d => d.FieldName == "ShipperReference1" && d.ObjectTableId == MasterObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField ShipperReference2 = ObjectContext.ObjectFields.Where(d => d.FieldName == "ShipperReference2" && d.ObjectTableId == MasterObject.Id && d.Tenant == 0).FirstOrDefault();

            Screen ClientAddEditScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "Master.CustomerAddEditScreen", Name = "Customer Add Edit Screen", ObjectTableId = MasterObject.Id, NumberOfColumns = 2, NumberOfRows = 6 }, ScreensRepository, tenantScreens);
            ScreenField ShipperIdField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 0, Column = 0, ObjectFieldId = ShipperId.Id, ScreenId = ClientAddEditScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField ShipperReference1Field = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 1, Column = 0, ObjectFieldId = ShipperReference1.Id, ScreenId = ClientAddEditScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField ShipperAddressIdField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 2, Column = 0, ObjectFieldId = ShipperAddressId.Id, ScreenId = ClientAddEditScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField ShipperReference2Field = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 1, Column = 1, ObjectFieldId = ShipperReference2.Id, ScreenId = ClientAddEditScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField ShipperContactIdField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 2, Column = 1, ObjectFieldId = ShipperContactId.Id, ScreenId = ClientAddEditScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);

            //ScreensRepository.Add(ClientAddEditScreen);
            //ScreenFieldsRepository.Add(ShipperIdField);
            //ScreenFieldsRepository.Add(ShipperReference1Field);
            //ScreenFieldsRepository.Add(ShipperAddressIdField);
            //ScreenFieldsRepository.Add(ShipperReference2Field);
            //ScreenFieldsRepository.Add(ShipperContactIdField);
            if (Testing.General.IsTesting)
            {
                //============= Just For Testing ============= 
                Screen TestScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "Master.TestScreen", ObjectTableId = MasterObject.Id, NumberOfColumns = 2, NumberOfRows = 6 }, ScreensRepository, tenantScreens);
                ScreenField TestScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 0, Column = 0, ObjectFieldId = ShipperId.Id, ScreenId = ClientAddEditScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);

                //Update
                //Screen TestUpdateHeaderScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "Master.HeaderScreen", Name = "Header Screen", ObjectTableId = MasterObject.Id, NumberOfColumns = 100, NumberOfRows = 3, IsReadOnly = true, Tenant = 0 }, ScreensRepository, tenantScreens);
                //============================================
            }
            ObjectContext.SaveChanges();


        }

        public void BuildPortScreens(Dictionary<string, Screen> tenantScreens, Dictionary<string, ScreenField> tenantScreenFields)
        {
            ObjectTable PortObject = ObjectContext.ObjectTables.Where(d => d.Name == "Port" && d.Tenant == 0).FirstOrDefault();

            ObjectField PortCode = ObjectContext.ObjectFields.Where(d => d.FieldName == "Code" && d.ObjectTableId == PortObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField PortEnglishName = ObjectContext.ObjectFields.Where(d => d.FieldName == "EnglishName" && d.ObjectTableId == PortObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField PortLocalName = ObjectContext.ObjectFields.Where(d => d.FieldName == "LocalName" && d.ObjectTableId == PortObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField PortCountryId = ObjectContext.ObjectFields.Where(d => d.FieldName == "CountryId" && d.ObjectTableId == PortObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField StateCountryId = ObjectContext.ObjectFields.Where(d => d.FieldName == "StateId" && d.ObjectTableId == PortObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField PortAddedManually = ObjectContext.ObjectFields.Where(d => d.FieldName == "AddedManually" && d.ObjectTableId == PortObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField PortInActive = ObjectContext.ObjectFields.Where(d => d.FieldName == "InActive" && d.ObjectTableId == PortObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField PortIsOcean = ObjectContext.ObjectFields.Where(d => d.FieldName == "IsOcean" && d.ObjectTableId == PortObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField PortIsAir = ObjectContext.ObjectFields.Where(d => d.FieldName == "IsAir" && d.ObjectTableId == PortObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField PortIsGround = ObjectContext.ObjectFields.Where(d => d.FieldName == "IsInland" && d.ObjectTableId == PortObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField PortNotes = ObjectContext.ObjectFields.Where(d => d.FieldName == "Notes" && d.ObjectTableId == PortObject.Id && d.Tenant == 0).FirstOrDefault();

            // ** To be added later ** 
            //ObjectField PortRemark = ObjectContext.ObjectFields.Where(d => d.FieldName == "Remark" && d.ObjectTableId == PortObject.Id).FirstOrDefault();
            //ObjectField PortLatitude = ObjectContext.ObjectFields.Where(d => d.FieldName == "Latitude" && d.ObjectTableId == PortObject.Id).FirstOrDefault();
            //ObjectField PortLongtitude = ObjectContext.ObjectFields.Where(d => d.FieldName == "Longtitude" && d.ObjectTableId == PortObject.Id).FirstOrDefault();

            Screen generalTabScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "Port.GeneralTabScreen", Name = "General Tab Screen", ObjectTableId = PortObject.Id, NumberOfColumns = 2, NumberOfRows = 6 }, ScreensRepository, tenantScreens);

            ScreenField PortCodeScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = PortCode.Id, Row = 0, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField PortEnglishNameScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = PortEnglishName.Id, Row = 1, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField PortLocalNameScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = PortLocalName.Id, Row = 2, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField PortCountryIdScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = PortCountryId.Id, Row = 3, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField PortStateIdScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = StateCountryId.Id, Row = 4, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField NotesScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = PortNotes.Id, Row = 5, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);

            ScreenField PortInActiveScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = PortInActive.Id, Row = 1, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField PortIsOceanScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = PortIsOcean.Id, Row = 2, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField PortIsAirScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = PortIsAir.Id, Row = 3, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField PortIsGroundScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = PortIsGround.Id, Row = 4, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);

            //ScreensRepository.Add(generalTabScreen);
            //ScreenFieldsRepository.Add(PortCodeScreenField);
            //ScreenFieldsRepository.Add(PortEnglishNameScreenField);
            //ScreenFieldsRepository.Add(PortLocalNameScreenField);
            //ScreenFieldsRepository.Add(PortCountryIdScreenField);

            //ScreenFieldsRepository.Add(PortInActiveScreenField);
            //ScreenFieldsRepository.Add(PortIsOceanScreenField);
            //ScreenFieldsRepository.Add(PortIsAirScreenField);
            //ScreenFieldsRepository.Add(PortIsGroundScreenField);

            //==============Header screen===========================

            Screen HeaderScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "Port.HeaderScreen", Name = "Header Screen", ObjectTableId = PortObject.Id, NumberOfColumns = 1, NumberOfRows = 3, IsReadOnly = true }, ScreensRepository, tenantScreens);


            ScreenField Header_CodeScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = PortCode.Id, Row = 0, ScreenId = HeaderScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField Header_NameScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = PortEnglishName.Id, Row = 1, ScreenId = HeaderScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField Header_AddedManuallyScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = PortAddedManually.Id, Row = 2, ScreenId = HeaderScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            //ScreensRepository.Add(HeaderScreen);

            //ScreenFieldsRepository.Add(Header_CodeScreenField);
            //ScreenFieldsRepository.Add(Header_NameScreenField);
            //ScreenFieldsRepository.Add(Header_AddedManuallyScreenField);
            PortObject.HeaderScreenId = HeaderScreen.Id;

            ObjectContext.SaveChanges();
        }

        public void BuildCountryScreens(Dictionary<string, Screen> tenantScreens, Dictionary<string, ScreenField> tenantScreenFields)
        {
            ObjectTable CountryObject = ObjectContext.ObjectTables.Where(d => d.Name == "Country" && d.Tenant == 0).FirstOrDefault();

            #region General Screen
            Screen generalTabScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "Country.GeneralTabScreen", Name = "General Tab Screen", ObjectTableId = CountryObject.Id, NumberOfColumns = 2, NumberOfRows = 9 }, ScreensRepository, tenantScreens);

            ObjectField field01 = ObjectContext.ObjectFields.Where(d => d.FieldName == "Code" && d.ObjectTable.Id == CountryObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField field02 = ObjectContext.ObjectFields.Where(d => d.FieldName == "EnglishName" && d.ObjectTable.Id == CountryObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField field03 = ObjectContext.ObjectFields.Where(d => d.FieldName == "LocalName" && d.ObjectTable.Id == CountryObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField field04 = ObjectContext.ObjectFields.Where(d => d.FieldName == "GlobalZoneId" && d.ObjectTable.Id == CountryObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField field05 = ObjectContext.ObjectFields.Where(d => d.FieldName == "AddedManually" && d.ObjectTable.Id == CountryObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField field06 = ObjectContext.ObjectFields.Where(d => d.FieldName == "InActive" && d.ObjectTable.Id == CountryObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField field07 = ObjectContext.ObjectFields.Where(d => d.FieldName == "EC" && d.ObjectTable.Id == CountryObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField field08 = ObjectContext.ObjectFields.Where(d => d.FieldName == "IsStateRequired" && d.ObjectTable.Id == CountryObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField field09 = ObjectContext.ObjectFields.Where(d => d.FieldName == "HasCitiesList" && d.ObjectTable.Id == CountryObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField field10 = ObjectContext.ObjectFields.Where(d => d.FieldName == "Notes" && d.ObjectTable.Id == CountryObject.Id && d.Tenant == 0).FirstOrDefault();

            ScreenField screenField01 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = field01.Id, Row = 0, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField screenField02 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = field02.Id, Row = 1, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField screenField03 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = field03.Id, Row = 2, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField screenField04 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = field04.Id, Row = 3, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField screenField06 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = field06.Id, Row = 4, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField screenField07 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = field07.Id, Row = 5, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField screenField08 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = field08.Id, Row = 6, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField screenField09 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = field09.Id, Row = 7, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField screenField10 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = field10.Id, Row = 8, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            #endregion

            #region General Screen
            Screen headerScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "Country.HeaderScreen", Name = "Header Screen", ObjectTableId = CountryObject.Id, NumberOfColumns = 1, NumberOfRows = 3, IsReadOnly = true }, ScreensRepository, tenantScreens);

            ScreenField headerScreenField01 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = field01.Id, Row = 0, ScreenId = headerScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField headerScreenField02 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = field02.Id, Row = 1, ScreenId = headerScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField headerScreenField03 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = field05.Id, Row = 2, ScreenId = headerScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            CountryObject.HeaderScreenId = headerScreen.Id;
            #endregion

            ObjectContext.SaveChanges();
        }

        public void BuildGlobalZoneScreens(Dictionary<string, Screen> tenantScreens, Dictionary<string, ScreenField> tenantScreenFields)
        {
            ObjectTable GlobalZoneObject = ObjectContext.ObjectTables.Where(d => d.Name == "GlobalZone" && d.Tenant == 0).FirstOrDefault();

            // General Screen
            Screen generalTabScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "GlobalZone.GeneralTabScreen", Name = "General Tab Screen", ObjectTableId = GlobalZoneObject.Id, NumberOfColumns = 2, NumberOfRows = 4 }, ScreensRepository, tenantScreens);

            ObjectField GlobalZoneCode = ObjectContext.ObjectFields.Where(d => d.FieldName == "Code" && d.ObjectTable.Id == GlobalZoneObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField GlobalZoneName = ObjectContext.ObjectFields.Where(d => d.FieldName == "EnglishName" && d.ObjectTable.Id == GlobalZoneObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField GlobalZoneLocalName = ObjectContext.ObjectFields.Where(d => d.FieldName == "LocalName" && d.ObjectTable.Id == GlobalZoneObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField GlobalZoneInActive = ObjectContext.ObjectFields.Where(d => d.FieldName == "InActive" && d.ObjectTable.Id == GlobalZoneObject.Id && d.Tenant == 0).FirstOrDefault();


            ScreenField GlobalZoneCodeScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = GlobalZoneCode.Id, Row = 0, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField GlobalZoneNameScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = GlobalZoneName.Id, Row = 1, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField GlobalZoneLocalNameScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = GlobalZoneLocalName.Id, Row = 2, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField GlobalZoneInActiveScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = GlobalZoneInActive.Id, Row = 3, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);

            //ScreensRepository.Add(generalTabScreen);

            //ScreenFieldsRepository.Add(GlobalZoneCodeScreenField);
            //ScreenFieldsRepository.Add(GlobalZoneNameScreenField);
            //ScreenFieldsRepository.Add(GlobalZoneLocalNameScreenField);
            //ScreenFieldsRepository.Add(GlobalZoneInActiveScreenField);

            //==============Header screen===========================

            Screen HeaderScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "GlobalZone.HeaderScreen", Name = "Header Screen", ObjectTableId = GlobalZoneObject.Id, NumberOfColumns = 1, NumberOfRows = 2, IsReadOnly = true }, ScreensRepository, tenantScreens);


            ScreenField Header_CodeScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = GlobalZoneCode.Id, Row = 0, ScreenId = HeaderScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField Header_NameScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = GlobalZoneName.Id, Row = 1, ScreenId = HeaderScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);

            //ScreensRepository.Add(HeaderScreen);

            //ScreenFieldsRepository.Add(Header_CodeScreenField);
            //ScreenFieldsRepository.Add(Header_NameScreenField);

            GlobalZoneObject.HeaderScreenId = HeaderScreen.Id;

            ObjectContext.SaveChanges();

        }

        public void BuildBranchScreens(Dictionary<string, Screen> tenantScreens, Dictionary<string, ScreenField> tenantScreenFields)
        {
            ObjectTable BranchObject = ObjectContext.ObjectTables.Where(d => d.Name == "Branch" && d.Tenant == 0).FirstOrDefault();

            // General Screen
            Screen generalTabScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "Branch.GeneralTabScreen", Name = "General Tab Screen", ObjectTableId = BranchObject.Id, NumberOfColumns = 2, NumberOfRows = 3 }, ScreensRepository, tenantScreens);

            ObjectField BranchName = ObjectContext.ObjectFields.Where(d => d.FieldName == "EnglishName" && d.ObjectTableId == BranchObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField BranchLocalName = ObjectContext.ObjectFields.Where(d => d.FieldName == "LocalName" && d.ObjectTableId == BranchObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField BranchInActive = ObjectContext.ObjectFields.Where(d => d.FieldName == "InActive" && d.ObjectTableId == BranchObject.Id && d.Tenant == 0).FirstOrDefault();

            ScreenField BranchNameNameScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = BranchName.Id, Row = 0, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField BranchLocalNameLocalNameScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = BranchLocalName.Id, Row = 1, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField BranchLocalNameInActiveScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = BranchInActive.Id, Row = 2, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);

            //ScreensRepository.Add(generalTabScreen);

            //ScreenFieldsRepository.Add(BranchNameNameScreenField);
            //ScreenFieldsRepository.Add(BranchLocalNameLocalNameScreenField);
            //ScreenFieldsRepository.Add(BranchLocalNameInActiveScreenField);

            //==============Header screen===========================

            Screen HeaderScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "Branch.HeaderScreen", Name = "Header Screen", ObjectTableId = BranchObject.Id, NumberOfColumns = 1, NumberOfRows = 1, IsReadOnly = true }, ScreensRepository, tenantScreens);

            ScreenField Header_NameScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = BranchName.Id, Row = 0, ScreenId = HeaderScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);

            //ScreensRepository.Add(HeaderScreen);

            //ScreenFieldsRepository.Add(Header_NameScreenField);

            BranchObject.HeaderScreenId = HeaderScreen.Id;

            //-----------------------Billing Screen--------------------------

            ObjectContext.SaveChanges();

        }

        public void BuildDepartmentScreens(Dictionary<string, Screen> tenantScreens, Dictionary<string, ScreenField> tenantScreenFields)
        {
            ObjectTable DepartmentObject = ObjectContext.ObjectTables.Where(d => d.Name == "Department" && d.Tenant == 0).FirstOrDefault();

            // General Screen
            Screen generalTabScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "Department.GeneralTabScreen", Name = "General Tab Screen", ObjectTableId = DepartmentObject.Id, NumberOfColumns = 2, NumberOfRows = 4 }, ScreensRepository, tenantScreens);

            ObjectField DepartmentName = ObjectContext.ObjectFields.Where(d => d.FieldName == "EnglishName" && d.ObjectTableId == DepartmentObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField DepartmentLocalName = ObjectContext.ObjectFields.Where(d => d.FieldName == "LocalName" && d.ObjectTableId == DepartmentObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField DepartmentInActive = ObjectContext.ObjectFields.Where(d => d.FieldName == "InActive" && d.ObjectTableId == DepartmentObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField Notes = ObjectContext.ObjectFields.Where(d => d.FieldName == "Notes" && d.ObjectTableId == DepartmentObject.Id && d.Tenant == 0).FirstOrDefault();

            ScreenField DepartmentNameScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = DepartmentName.Id, Row = 0, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField DepartmentLocalNameScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = DepartmentLocalName.Id, Row = 1, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField DepartmentInActiveScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = DepartmentInActive.Id, Row = 2, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField NotesScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = Notes.Id, Row = 3, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            //ScreensRepository.Add(generalTabScreen);

            //ScreenFieldsRepository.Add(DepartmentNameScreenField);
            //ScreenFieldsRepository.Add(DepartmentLocalNameScreenField);
            //ScreenFieldsRepository.Add(DepartmentInActiveScreenField);

            //==============Header screen===========================

            Screen HeaderScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "Department.HeaderScreen", Name = "Header Screen", ObjectTableId = DepartmentObject.Id, NumberOfColumns = 1, NumberOfRows = 1, IsReadOnly = true }, ScreensRepository, tenantScreens);

            ScreenField Header_NameScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = DepartmentName.Id, Row = 0, ScreenId = HeaderScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);

            //ScreensRepository.Add(HeaderScreen);

            //ScreenFieldsRepository.Add(Header_NameScreenField);

            DepartmentObject.HeaderScreenId = HeaderScreen.Id;
            ObjectContext.SaveChanges();

        }

        public void BuildStatesScreens(Dictionary<string, Screen> tenantScreens, Dictionary<string, ScreenField> tenantScreenFields)
        {
            ObjectTable StateObject = ObjectContext.ObjectTables.Where(d => d.Name == "State" && d.Tenant == 0).FirstOrDefault();

            Screen generalTabScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "State.GeneralTabScreen", Name = "General Tab Screen", ObjectTableId = StateObject.Id, NumberOfColumns = 2, NumberOfRows = 6 }, ScreensRepository, tenantScreens);

            ObjectField StateCode = ObjectContext.ObjectFields.Where(d => d.FieldName == "Code" && d.ObjectTable.Id == StateObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField StateName = ObjectContext.ObjectFields.Where(d => d.FieldName == "EnglishName" && d.ObjectTable.Id == StateObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField StateLocalName = ObjectContext.ObjectFields.Where(d => d.FieldName == "LocalName" && d.ObjectTable.Id == StateObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField StateCountry = ObjectContext.ObjectFields.Where(d => d.FieldName == "CountryId" && d.ObjectTable.Id == StateObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField StateAddedManually = ObjectContext.ObjectFields.Where(d => d.FieldName == "AddedManually" && d.ObjectTable.Id == StateObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField StateInActive = ObjectContext.ObjectFields.Where(d => d.FieldName == "InActive" && d.ObjectTable.Id == StateObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField Notes = ObjectContext.ObjectFields.Where(d => d.ObjectTableId == StateObject.Id && d.FieldName == "Notes").FirstOrDefault();

            ScreenField StateCodeScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = StateCode.Id, Row = 0, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField StateNameScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = StateName.Id, Row = 1, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField StateLocalNameScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = StateLocalName.Id, Row = 2, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField StateCountryScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = StateCountry.Id, Row = 3, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField StateInActiveScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = StateInActive.Id, Row = 4, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField NotesScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = Notes.Id, Row = 5, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);


            Screen HeaderScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "State.HeaderScreen", Name = "Header Screen", ObjectTableId = StateObject.Id, NumberOfColumns = 1, NumberOfRows = 3, IsReadOnly = true }, ScreensRepository, tenantScreens);

            ScreenField Header_CodeScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = StateCode.Id, Row = 0, ScreenId = HeaderScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField Header_NameScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = StateName.Id, Row = 1, ScreenId = HeaderScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField Header_AddedManuallyScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = StateAddedManually.Id, Row = 2, ScreenId = HeaderScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            StateObject.HeaderScreenId = HeaderScreen.Id;

            ObjectContext.SaveChanges();
        }

        public void BuildCountryCityScreens(Dictionary<string, Screen> tenantScreens, Dictionary<string, ScreenField> tenantScreenFields)
        {
            ObjectTable entityObject = ObjectContext.ObjectTables.Where(d => d.Name == "CountryCity" && d.Tenant == 0).FirstOrDefault();

            Screen generalTabScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "CountryCity.GeneralTabScreen", Name = "General Tab Screen", ObjectTableId = entityObject.Id, NumberOfColumns = 2, NumberOfRows = 6 }, ScreensRepository, tenantScreens);

            ObjectField objectField_01 = ObjectContext.ObjectFields.Where(d => d.FieldName == "Code" && d.ObjectTable.Id == entityObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField objectField_02 = ObjectContext.ObjectFields.Where(d => d.FieldName == "EnglishName" && d.ObjectTable.Id == entityObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField objectField_03 = ObjectContext.ObjectFields.Where(d => d.FieldName == "LocalName" && d.ObjectTable.Id == entityObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField objectField_04 = ObjectContext.ObjectFields.Where(d => d.FieldName == "CountryId" && d.ObjectTable.Id == entityObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField objectField_05 = ObjectContext.ObjectFields.Where(d => d.FieldName == "InActive" && d.ObjectTable.Id == entityObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField objectField_06 = ObjectContext.ObjectFields.Where(d => d.FieldName == "Notes" && d.ObjectTableId == entityObject.Id).FirstOrDefault();
            ObjectField objectField_07 = ObjectContext.ObjectFields.Where(d => d.FieldName == "AddedManually" && d.ObjectTable.Id == entityObject.Id && d.Tenant == 0).FirstOrDefault();

            ScreenField screenField_01 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = objectField_01.Id, Row = 0, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField screenField_02 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = objectField_02.Id, Row = 1, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField screenField_03 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = objectField_03.Id, Row = 2, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField screenField_04 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = objectField_04.Id, Row = 3, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField screenField_05 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = objectField_05.Id, Row = 4, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField screenField_06 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = objectField_06.Id, Row = 5, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);

            Screen HeaderScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "CountryCity.HeaderScreen", Name = "Header Screen", ObjectTableId = entityObject.Id, NumberOfColumns = 1, NumberOfRows = 2, IsReadOnly = true }, ScreensRepository, tenantScreens);
            ScreenField Header_NameScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = objectField_02.Id, Row = 0, ScreenId = HeaderScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField Header_AddedManuallyScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = objectField_07.Id, Row = 1, ScreenId = HeaderScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            entityObject.HeaderScreenId = HeaderScreen.Id;

            ObjectContext.SaveChanges();
        }

        public void BuildFollowUpTypeScreens(Dictionary<string, Screen> tenantScreens, Dictionary<string, ScreenField> tenantScreenFields)
        {
            ObjectTable FollowUpObject = ObjectContext.ObjectTables.Where(d => d.Name == "FollowUpType" && d.Tenant == 0).FirstOrDefault();

            // General Screen
            Screen generalTabScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "FollowUp.GeneralTabScreen", Name = "General Tab Screen", ObjectTableId = FollowUpObject.Id, NumberOfColumns = 2, NumberOfRows = 3 }, ScreensRepository, tenantScreens);

            ObjectField FollowUpTypeCode = ObjectContext.ObjectFields.Where(d => d.FieldName == "Code" && d.ObjectTable.Id == FollowUpObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField FollowUpTypeName = ObjectContext.ObjectFields.Where(d => d.FieldName == "Name" && d.ObjectTable.Id == FollowUpObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField FollowUpDateType = ObjectContext.ObjectFields.Where(d => d.FieldName == "EntityDateId" && d.ObjectTable.Id == FollowUpObject.Id && d.Tenant == 0).FirstOrDefault();

            ScreenField FollowUpTypeCodeScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = FollowUpTypeCode.Id, Row = 0, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField FollowUpTypeNameScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = FollowUpTypeName.Id, Row = 1, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField FollowUpDateTypeScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = FollowUpDateType.Id, Row = 2, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);

            //ScreensRepository.Add(generalTabScreen);

            //ScreenFieldsRepository.Add(FollowUpTypeCodeScreenField);
            //ScreenFieldsRepository.Add(FollowUpTypeNameScreenField);
            //ScreenFieldsRepository.Add(FollowUpDateTypeScreenField);

            ObjectContext.SaveChanges();
        }

        public void BuildDocumentFolderScreens(Dictionary<string, Screen> tenantScreens, Dictionary<string, ScreenField> tenantScreenFields)
        {
            ObjectTable DocumentFolderObject = ObjectContext.ObjectTables.Where(d => d.Name == "DocumentFolder" && d.Tenant == 0).FirstOrDefault();

            ObjectField DocumentFolderCode = ObjectContext.ObjectFields.Where(d => d.FieldName == "Code" && d.ObjectTableId == DocumentFolderObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField DocumentFolderEnglishName = ObjectContext.ObjectFields.Where(d => d.FieldName == "EnglishName" && d.ObjectTableId == DocumentFolderObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField DocumentFolderLocalName = ObjectContext.ObjectFields.Where(d => d.FieldName == "LocalName" && d.ObjectTableId == DocumentFolderObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField DocumentFolderIsExternal = ObjectContext.ObjectFields.Where(d => d.FieldName == "IsExternalFolder" && d.ObjectTableId == DocumentFolderObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField DocumentFolderParentFolderId = ObjectContext.ObjectFields.Where(d => d.FieldName == "ParentFolderId" && d.ObjectTableId == DocumentFolderObject.Id && d.Tenant == 0).FirstOrDefault();

            Screen generalTabScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "DocumentFolder.GeneralTabScreen", Name = "General Tab Screen", ObjectTableId = DocumentFolderObject.Id, NumberOfColumns = 2, NumberOfRows = 8 }, ScreensRepository, tenantScreens);

            ScreenField DocumentTypeCodeScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = DocumentFolderCode.Id, Row = 0, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField DocumentTypeEnglishNameScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = DocumentFolderEnglishName.Id, Row = 1, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField DocumentTypeLocalNameScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = DocumentFolderLocalName.Id, Row = 2, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField DocumentTypeIsExternalScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = DocumentFolderParentFolderId.Id, Row = 3, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField DocumentTypeParentFolderIdScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = DocumentFolderIsExternal.Id, Row = 0, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);

            //==============Header screen===========================
            Screen HeaderScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "DocumentFolder.HeaderScreen", Name = "Header Screen", ObjectTableId = DocumentFolderObject.Id, NumberOfColumns = 1, NumberOfRows = 2, IsReadOnly = true }, ScreensRepository, tenantScreens);

            ScreenField Header_CodeScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = DocumentFolderCode.Id, Row = 0, ScreenId = HeaderScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField Header_NameScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = DocumentFolderEnglishName.Id, Row = 1, ScreenId = HeaderScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);

            DocumentFolderObject.HeaderScreenId = HeaderScreen.Id;

            ObjectContext.SaveChanges();
        }

        public void BuildDocumentTypeScreens(Dictionary<string, Screen> tenantScreens, Dictionary<string, ScreenField> tenantScreenFields)
        {
            ObjectTable DocumentTypeObject = ObjectContext.ObjectTables.Where(d => d.Name == "DocumentType" && d.Tenant == 0).FirstOrDefault();

            ObjectField DocumentTypeCode = ObjectContext.ObjectFields.Where(d => d.FieldName == "Code" && d.ObjectTableId == DocumentTypeObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField DocumentTypeName = ObjectContext.ObjectFields.Where(d => d.FieldName == "Name" && d.ObjectTableId == DocumentTypeObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField DocumentTypeFormat = ObjectContext.ObjectFields.Where(d => d.FieldName == "TemplateFormatCode" && d.ObjectTableId == DocumentTypeObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField DocumentTypeIsAir = ObjectContext.ObjectFields.Where(d => d.FieldName == "IsAir" && d.ObjectTableId == DocumentTypeObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField DocumentTypeIsOcean = ObjectContext.ObjectFields.Where(d => d.FieldName == "IsOcean" && d.ObjectTableId == DocumentTypeObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField DocumentTypeIsGround = ObjectContext.ObjectFields.Where(d => d.FieldName == "IsInland" && d.ObjectTableId == DocumentTypeObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField DocumentTypeIsDocIn = ObjectContext.ObjectFields.Where(d => d.FieldName == "IsDocIn" && d.ObjectTableId == DocumentTypeObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField DocumentTypeIsDocOut = ObjectContext.ObjectFields.Where(d => d.FieldName == "IsDocOut" && d.ObjectTableId == DocumentTypeObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField DocumentTypeIsMaster = ObjectContext.ObjectFields.Where(d => d.FieldName == "IsMaster" && d.ObjectTableId == DocumentTypeObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField DocumentTypeIsDirect = ObjectContext.ObjectFields.Where(d => d.FieldName == "IsDirect" && d.ObjectTableId == DocumentTypeObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField DocumentTypeIsHouse = ObjectContext.ObjectFields.Where(d => d.FieldName == "IsHouse" && d.ObjectTableId == DocumentTypeObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField DocumentTypeCategory = ObjectContext.ObjectFields.Where(d => d.FieldName == "DocumentTypeCategoryCode" && d.ObjectTableId == DocumentTypeObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField DocumentTypeObjectTableId = ObjectContext.ObjectFields.Where(d => d.FieldName == "ObjectTableId" && d.ObjectTableId == DocumentTypeObject.Id && d.Tenant == 0).FirstOrDefault();
            //ObjectField IsCustomerView = ObjectContext.ObjectFields.Where(d => d.FieldName == "IsCustomerView" && d.ObjectTableId == DocumentTypeObject.Id && d.Tenant == 0).FirstOrDefault();
            //ObjectField IsAgentView = ObjectContext.ObjectFields.Where(d => d.FieldName == "IsAgentView" && d.ObjectTableId == DocumentTypeObject.Id && d.Tenant == 0).FirstOrDefault();

            Screen generalTabScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "DocumentType.GeneralTabScreen", Name = "General Tab Screen", ObjectTableId = DocumentTypeObject.Id, NumberOfColumns = 2, NumberOfRows = 8 }, ScreensRepository, tenantScreens);

            ScreenField DocumentTypeCodeScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = DocumentTypeCode.Id, Row = 0, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField DocumentTypeNameScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = DocumentTypeName.Id, Row = 1, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField DocumentTypeObjectTableScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = DocumentTypeObjectTableId.Id, Row = 2, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField DocumentTypeFormatScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = DocumentTypeFormat.Id, Row = 3, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField DocumentTypeIsAirScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = DocumentTypeIsAir.Id, Row = 0, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField DocumentTypeIsOceanScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = DocumentTypeIsOcean.Id, Row = 1, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField DocumentTypeIsGroundScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = DocumentTypeIsGround.Id, Row = 2, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField DocumentTypeIsDocInScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = DocumentTypeIsDocIn.Id, Row = 4, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField DocumentTypeIsDocOutScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = DocumentTypeIsDocOut.Id, Row = 5, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField DocumentTypeIsMasterScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = DocumentTypeIsMaster.Id, Row = 3, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField DocumentTypeIsDirectScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = DocumentTypeIsDirect.Id, Row = 4, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField DocumentTypeIsHouseScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = DocumentTypeIsHouse.Id, Row = 5, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField DocumentTypeCategoryScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = DocumentTypeCategory.Id, Row = 6, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
         
            //ScreenField IsCustomerViewScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = IsCustomerView.Id, Row = 6, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            //ScreenField IsAgentViewScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = IsAgentView.Id, Row = 7, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);

            //==============Header screen===========================
            Screen HeaderScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "DocumentType.HeaderScreen", Name = "Header Screen", ObjectTableId = DocumentTypeObject.Id, NumberOfColumns = 1, NumberOfRows = 2, IsReadOnly = true }, ScreensRepository, tenantScreens);

            ScreenField Header_CodeScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = DocumentTypeCode.Id, Row = 0, ScreenId = HeaderScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField Header_NameScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = DocumentTypeName.Id, Row = 1, ScreenId = HeaderScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);

            DocumentTypeObject.HeaderScreenId = HeaderScreen.Id;

            ObjectContext.SaveChanges();
        }

        public void BuildDocumentsFilingScreens(Dictionary<string, Screen> tenantScreens, Dictionary<string, ScreenField> tenantScreenFields)
        {
            ObjectTable DocumentsFilingObject = ObjectContext.ObjectTables.Where(d => d.Name == "DocumentsFiling" && d.Tenant == 0).FirstOrDefault();

            ObjectField DocumentsFilingCode = ObjectContext.ObjectFields.Where(d => d.FieldName == "Code" && d.ObjectTableId == DocumentsFilingObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField DocumentsFilingDescription = ObjectContext.ObjectFields.Where(d => d.FieldName == "Description" && d.ObjectTableId == DocumentsFilingObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField DocumentsFilingCreateDate = ObjectContext.ObjectFields.Where(d => d.FieldName == "CreateDate" && d.ObjectTableId == DocumentsFilingObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField DocumentsFilingCreatedByUserName = ObjectContext.ObjectFields.Where(d => d.FieldName == "CreatedByUserName" && d.ObjectTableId == DocumentsFilingObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField DocumentsFilingOwnerName = ObjectContext.ObjectFields.Where(d => d.FieldName == "OwnerName" && d.ObjectTableId == DocumentsFilingObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField DocumentsFilingIsSharedWithForwarder = ObjectContext.ObjectFields.Where(d => d.FieldName == "IsSharedWithForwarder" && d.ObjectTableId == DocumentsFilingObject.Id && d.Tenant == 0).FirstOrDefault();

            //==============Header screen===========================
            Screen HeaderScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "DocumentsFiling.HeaderScreen", Name = "Header Screen", ObjectTableId = DocumentsFilingObject.Id, NumberOfColumns = 3, NumberOfRows = 2, IsReadOnly = true }, ScreensRepository, tenantScreens);

            ScreenField Header_CodeScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = DocumentsFilingCode.Id, Row = 0, ScreenId = HeaderScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField Header_DescriptionScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = DocumentsFilingDescription.Id, Row = 1, ScreenId = HeaderScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField Header_CreateDateScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = DocumentsFilingCreateDate.Id, Row = 0, ScreenId = HeaderScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField Header_CreatedByUserNameScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = DocumentsFilingCreatedByUserName.Id, Row = 1, ScreenId = HeaderScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField Header_OwnerNameScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 2, ObjectFieldId = DocumentsFilingOwnerName.Id, Row = 0, ScreenId = HeaderScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField Header_IsSharedWithForwarderScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 2, ObjectFieldId = DocumentsFilingIsSharedWithForwarder.Id, Row = 1, ScreenId = HeaderScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);

            DocumentsFilingObject.HeaderScreenId = HeaderScreen.Id;

            ObjectContext.SaveChanges();
        }
        

        public void BuildEventTypeScreens(Dictionary<string, Screen> tenantScreens, Dictionary<string, ScreenField> tenantScreenFields)
        {
            ObjectTable EventTypeObject = ObjectContext.ObjectTables.Where(d => d.Name == "EventType" && d.Tenant == 0).FirstOrDefault();

            // General Screen
            Screen generalTabScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "EventType.GeneralTabScreen", Name = "General Tab Screen", ObjectTableId = EventTypeObject.Id, NumberOfColumns = 2, NumberOfRows = 7 }, ScreensRepository, tenantScreens);

            ObjectField EventTypeCode = ObjectContext.ObjectFields.Where(d => d.FieldName == "Code" && d.ObjectTableId == EventTypeObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField EventTypeName = ObjectContext.ObjectFields.Where(d => d.FieldName == "EnglishName" && d.ObjectTableId == EventTypeObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField EventTypeLocalName = ObjectContext.ObjectFields.Where(d => d.FieldName == "LocalName" && d.ObjectTableId == EventTypeObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField EventTypeAddedManually = ObjectContext.ObjectFields.Where(d => d.FieldName == "AddedManually" && d.ObjectTableId == EventTypeObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField ObjectTableField = ObjectContext.ObjectFields.Where(d => d.FieldName == "ObjectTableId" && d.ObjectTableId == EventTypeObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField IsFollwoupfield = ObjectContext.ObjectFields.Where(d => d.FieldName == "IsFollowUp" && d.ObjectTableId == EventTypeObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField followEnglishname = ObjectContext.ObjectFields.Where(d => d.FieldName == "FollowUpEnglishName" && d.ObjectTableId == EventTypeObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField followlocalname = ObjectContext.ObjectFields.Where(d => d.FieldName == "FollowUpLocalName" && d.ObjectTableId == EventTypeObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField IsManualEntry = ObjectContext.ObjectFields.Where(d => d.FieldName == "IsManualEntry" && d.ObjectTableId == EventTypeObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField IsCustomerView = ObjectContext.ObjectFields.Where(d => d.FieldName == "IsCustomerView" && d.ObjectTableId == EventTypeObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField IsAgentView = ObjectContext.ObjectFields.Where(d => d.FieldName == "IsAgentView" && d.ObjectTableId == EventTypeObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField InActive = ObjectContext.ObjectFields.Where(d => d.FieldName == "InActive" && d.ObjectTableId == EventTypeObject.Id && d.Tenant == 0).FirstOrDefault();
            //ObjectField ManualActivatedFollowUp = ObjectContext.ObjectFields.Where(d => d.FieldName == "ManualActivatedFollowUp" && d.ObjectTableId == EventTypeObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField EntityStatusId = ObjectContext.ObjectFields.Where(d => d.FieldName == "EntityStatusId" && d.ObjectTableId == EventTypeObject.Id && d.Tenant == 0).FirstOrDefault();

            ScreenField ScreenField00 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = EventTypeCode.Id, Row = 0, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField ScreenField01 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = EventTypeName.Id, Row = 1, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField ScreenField02 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = EventTypeLocalName.Id, Row = 2, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField ScreenField03 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = ObjectTableField.Id, Row = 3, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField ScreenField04 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = EntityStatusId.Id, Row = 4, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField ScreenField05 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = InActive.Id, Row = 5, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);

            ScreenField ScreenField10 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = IsFollwoupfield.Id, Row = 0, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField ScreenField11 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = followEnglishname.Id, Row = 1, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField ScreenField12 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = followlocalname.Id, Row = 2, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            //ScreenField ScreenField13 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = ManualActivatedFollowUp.Id, Row = 3, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField ScreenField14 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = IsCustomerView.Id, Row = 3, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField ScreenField15 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = IsAgentView.Id, Row = 4, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            
            //==============Header screen===========================
            Screen HeaderScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "EventType.HeaderScreen", Name = "Header Screen", ObjectTableId = EventTypeObject.Id, NumberOfColumns = 1, NumberOfRows = 3, IsReadOnly = true }, ScreensRepository, tenantScreens);

            ScreenField Header_CodeScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = EventTypeCode.Id, Row = 0, ScreenId = HeaderScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField Header_NameScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = EventTypeName.Id, Row = 1, ScreenId = HeaderScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField Header_AddedManuallyScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = EventTypeAddedManually.Id, Row = 2, ScreenId = HeaderScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);

            EventTypeObject.HeaderScreenId = HeaderScreen.Id;

            ObjectContext.SaveChanges();
        }

        public void BuildIncotermScreens(Dictionary<string, Screen> tenantScreens, Dictionary<string, ScreenField> tenantScreenFields)
        {
            ObjectTable IncotermObject = ObjectContext.ObjectTables.Where(d => d.Name == "Incoterm" && d.Tenant == 0).FirstOrDefault();

            // General Screen
            Screen generalTabScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "Incoterm.GeneralTabScreen", Name = "General Tab Screen", ObjectTableId = IncotermObject.Id, NumberOfColumns = 2, NumberOfRows = 7 }, ScreensRepository, tenantScreens);

            ObjectField IncotermCode = ObjectContext.ObjectFields.Where(d => d.FieldName == "Code" && d.ObjectTableId == IncotermObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField IncotermName = ObjectContext.ObjectFields.Where(d => d.FieldName == "Name" && d.ObjectTableId == IncotermObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField IncotermLocalName = ObjectContext.ObjectFields.Where(d => d.FieldName == "LocalName" && d.ObjectTableId == IncotermObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField IncotermFreight = ObjectContext.ObjectFields.Where(d => d.FieldName == "Freight" && d.ObjectTableId == IncotermObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField IncotermAddedManually = ObjectContext.ObjectFields.Where(d => d.FieldName == "AddedManually" && d.ObjectTableId == IncotermObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField IncotermInActive = ObjectContext.ObjectFields.Where(d => d.FieldName == "InActive" && d.ObjectTableId == IncotermObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField IncotermOtherCharges = ObjectContext.ObjectFields.Where(d => d.FieldName == "OtherCharges" && d.ObjectTableId == IncotermObject.Id && d.Tenant == 0).FirstOrDefault();

            ScreenField IncotermCodeScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = IncotermCode.Id, Row = 0, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField IncotermNameScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = IncotermName.Id, Row = 1, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField IncotermLocalNameScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = IncotermLocalName.Id, Row = 2, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField IncotermFreightScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = IncotermFreight.Id, Row = 3, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);

            ScreenField IncotermInActiveScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = IncotermInActive.Id, Row = 4, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField IncotermOtherChargesScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = IncotermOtherCharges.Id, Row = 1, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);

            //==============Header screen===========================
            Screen HeaderScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "Incoterm.HeaderScreen", Name = "Header Screen", ObjectTableId = IncotermObject.Id, NumberOfColumns = 1, NumberOfRows = 3, IsReadOnly = true }, ScreensRepository, tenantScreens);

            ScreenField Header_CodeScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = IncotermCode.Id, Row = 0, ScreenId = HeaderScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField Header_NameScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = IncotermName.Id, Row = 1, ScreenId = HeaderScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField Header_AddedManuallyScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = IncotermAddedManually.Id, Row = 2, ScreenId = HeaderScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);

            IncotermObject.HeaderScreenId = HeaderScreen.Id;

            ObjectContext.SaveChanges();

        }

        public void BuildPaymentTermScreens(Dictionary<string, Screen> tenantScreens, Dictionary<string, ScreenField> tenantScreenFields)
        {
            ObjectTable PaymentTermObject = ObjectContext.ObjectTables.Where(d => d.Name == "PaymentTerm" && d.Tenant == 0).FirstOrDefault();

            #region General Screen
            Screen generalTabScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "PaymentTerm.GeneralTabScreen", Name = "General Tab Screen", ObjectTableId = PaymentTermObject.Id, NumberOfColumns = 2, NumberOfRows = 8 }, ScreensRepository, tenantScreens);

            ObjectField objectField_Name = ObjectContext.ObjectFields.Where(d => d.FieldName == "EnglishName" && d.ObjectTableId == PaymentTermObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField objectField_Local = ObjectContext.ObjectFields.Where(d => d.FieldName == "LocalName" && d.ObjectTableId == PaymentTermObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField objectField_CurrentMonth = ObjectContext.ObjectFields.Where(d => d.FieldName == "EndOfMonth" && d.ObjectTableId == PaymentTermObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField objectField_FromDateType = ObjectContext.ObjectFields.Where(d => d.FieldName == "FromDateTypeCode" && d.ObjectTableId == PaymentTermObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField objectField_Days = ObjectContext.ObjectFields.Where(d => d.FieldName == "Days" && d.ObjectTableId == PaymentTermObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField objectField_Inactive = ObjectContext.ObjectFields.Where(d => d.FieldName == "InActive" && d.ObjectTableId == PaymentTermObject.Id && d.Tenant == 0).FirstOrDefault();

            ScreenField screenField_Name = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = objectField_Name.Id, Row = 0, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField screenField_Local = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = objectField_Local.Id, Row = 1, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField screenField_CurrentMonth = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = objectField_CurrentMonth.Id, Row = 2, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField screenField_FromDateType = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = objectField_FromDateType.Id, Row = 3, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField screenField_Days = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = objectField_Days.Id, Row = 4, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField screenField_Inactive = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = objectField_Inactive.Id, Row = 5, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);

            ObjectField objectField10 = ObjectContext.ObjectFields.Where(d => d.FieldName == "Description" && d.ObjectTableId == PaymentTermObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField objectField11 = ObjectContext.ObjectFields.Where(d => d.FieldName == "LocalDescription" && d.ObjectTableId == PaymentTermObject.Id && d.Tenant == 0).FirstOrDefault();
            ScreenField screenField10 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = objectField10.Id, Row = 0, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField screenField11 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = objectField11.Id, Row = 1, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            #endregion

            #region Header screen
            ObjectField PaymentTermAddedManually = ObjectContext.ObjectFields.Where(d => d.FieldName == "AddedManually" && d.ObjectTableId == PaymentTermObject.Id && d.Tenant == 0).FirstOrDefault();

            Screen HeaderScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "PaymentTerm.HeaderScreen", Name = "Header Screen", ObjectTableId = PaymentTermObject.Id, NumberOfColumns = 1, NumberOfRows = 2, IsReadOnly = true }, ScreensRepository, tenantScreens);
            ScreenField Header_NameScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = objectField_Name.Id, Row = 0, ScreenId = HeaderScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField Header_AddedManuallyScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = PaymentTermAddedManually.Id, Row = 1, ScreenId = HeaderScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            PaymentTermObject.HeaderScreenId = HeaderScreen.Id;
            #endregion

            ObjectContext.SaveChanges();

        }

        public void BuildCurrencyScreens(Dictionary<string, Screen> tenantScreens, Dictionary<string, ScreenField> tenantScreenFields)
        {
            ObjectTable CurrencyObject = ObjectContext.ObjectTables.Where(d => d.Name == "Currency" && d.Tenant == 0).FirstOrDefault();

            // General Screen
            Screen generalTabScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "Currency.GeneralTabScreen", Name = "General Tab Screen", ObjectTableId = CurrencyObject.Id, NumberOfColumns = 2, NumberOfRows = 6 }, ScreensRepository, tenantScreens);

            ObjectField ObjectField_Code = ObjectContext.ObjectFields.Where(d => d.FieldName == "Code" && d.ObjectTableId == CurrencyObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField ObjectField_Name = ObjectContext.ObjectFields.Where(d => d.FieldName == "EnglishName" && d.ObjectTableId == CurrencyObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField ObjectField_Local = ObjectContext.ObjectFields.Where(d => d.FieldName == "LocalName" && d.ObjectTableId == CurrencyObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField ObjectField_Added = ObjectContext.ObjectFields.Where(d => d.FieldName == "AddedManually" && d.ObjectTableId == CurrencyObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField ObjectField_Inactive = ObjectContext.ObjectFields.Where(d => d.FieldName == "InActive" && d.ObjectTableId == CurrencyObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField ObjectField_Notes = ObjectContext.ObjectFields.Where(d => d.FieldName == "Notes" && d.ObjectTableId == CurrencyObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField ObjectField_Sign = ObjectContext.ObjectFields.Where(d => d.FieldName == "Sign" && d.ObjectTableId == CurrencyObject.Id && d.Tenant == 0).FirstOrDefault();

            ScreenField ScreenField_Code = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = ObjectField_Code.Id, Row = 0, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField ScreenField_Name = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = ObjectField_Name.Id, Row = 1, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField ScreenField_Local = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = ObjectField_Local.Id, Row = 2, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField ScreenField_Sign = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = ObjectField_Sign.Id, Row = 3, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField ScreenField_Inactive = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = ObjectField_Inactive.Id, Row = 4, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField ScreenField_Notes = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = ObjectField_Notes.Id, Row = 5, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);

            //==============Header screen===========================

            Screen ClientHeaderScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "Currency.HeaderScreen", Name = "Header Screen", ObjectTableId = CurrencyObject.Id, NumberOfColumns = 1, NumberOfRows = 3, IsReadOnly = true }, ScreensRepository, tenantScreens);
            ScreenField Header_CodeScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = ObjectField_Code.Id, Row = 0, ScreenId = ClientHeaderScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField Header_NameScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = ObjectField_Name.Id, Row = 1, ScreenId = ClientHeaderScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField Header_AddedManuallyScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = ObjectField_Added.Id, Row = 2, ScreenId = ClientHeaderScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            CurrencyObject.HeaderScreenId = ClientHeaderScreen.Id;
            ObjectContext.SaveChanges();
        }

        public void BuildVatTypeScreens(Dictionary<string, Screen> tenantScreens, Dictionary<string, ScreenField> tenantScreenFields)
        {
            ObjectTable ThisObject = ObjectContext.ObjectTables.Where(d => d.Name == "VatType" && d.Tenant == 0).FirstOrDefault();

            // General Screen
            Screen generalTabScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "VatType.GeneralTabScreen", Name = "General Tab Screen", ObjectTableId = ThisObject.Id, NumberOfColumns = 2, NumberOfRows = 6 }, ScreensRepository, tenantScreens);

            ObjectField Code = ObjectContext.ObjectFields.Where(d => d.FieldName == "Code" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField EnglishName = ObjectContext.ObjectFields.Where(d => d.FieldName == "EnglishName" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField LocalName = ObjectContext.ObjectFields.Where(d => d.FieldName == "LocalName" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();
            //ObjectField Percentage = ObjectContext.ObjectFields.Where(d => d.FieldName == "Percentage" && d.ObjectTableId == ThisObject.Id).FirstOrDefault();
            ObjectField AddedManually = ObjectContext.ObjectFields.Where(d => d.FieldName == "AddedManually" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField TermInActive = ObjectContext.ObjectFields.Where(d => d.FieldName == "InActive" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField description = ObjectContext.ObjectFields.Where(d => d.FieldName == "Description" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField localDescription = ObjectContext.ObjectFields.Where(d => d.FieldName == "LocalDescription" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();

            ScreenField CodeScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = Code.Id, Row = 0, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField EnglishNameScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = EnglishName.Id, Row = 1, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField LocalNameScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = LocalName.Id, Row = 2, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            //ScreenField PercentageScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = Percentage.Id, Row = 3, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);           
            ScreenField TermInActiveScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = TermInActive.Id, Row = 4, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField descriptionScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = description.Id, Row = 0, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField localDescriptionScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = localDescription.Id, Row = 1, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);

            //ScreensRepository.Add(generalTabScreen);

            //ScreenFieldsRepository.Add(CodeScreenField);
            //ScreenFieldsRepository.Add(EnglishNameScreenField);
            //ScreenFieldsRepository.Add(LocalNameScreenField);
            //ScreenFieldsRepository.Add(PercentageScreenField);

            //ScreenFieldsRepository.Add(TermInActiveScreenField);
            //ScreenFieldsRepository.Add(descriptionScreenField);
            //ScreenFieldsRepository.Add(localDescriptionScreenField);

            //==============Header screen===========================

            Screen HeaderScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "VatType.HeaderScreen", Name = "Header Screen", ObjectTableId = ThisObject.Id, NumberOfColumns = 1, NumberOfRows = 3, IsReadOnly = true }, ScreensRepository, tenantScreens);


            ScreenField Header_CodeScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = Code.Id, Row = 0, ScreenId = HeaderScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField Header_NameScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = EnglishName.Id, Row = 1, ScreenId = HeaderScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField Header_AddedManuallyScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = AddedManually.Id, Row = 2, ScreenId = HeaderScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ThisObject.HeaderScreenId = HeaderScreen.Id;

            ObjectContext.SaveChanges();

        }

        public void BuildChargeTypeScreens(Dictionary<string, Screen> tenantScreens, Dictionary<string, ScreenField> tenantScreenFields)
        {
            ObjectTable ThisObject = ObjectContext.ObjectTables.Where(d => d.Name == "ChargesType" && d.Tenant == 0).FirstOrDefault();

            #region General Screen
            Screen generalTabScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "ChargesType.GeneralTabScreen", Name = "General Tab Screen", ObjectTableId = ThisObject.Id, NumberOfColumns = 3, NumberOfRows = 11 }, ScreensRepository, tenantScreens);

            #endregion

            //==============Header screen===========================
            Screen HeaderScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "ChargesType.HeaderScreen", Name = "Header Screen", ObjectTableId = ThisObject.Id, NumberOfColumns = 3, NumberOfRows = 1, IsReadOnly = true }, ScreensRepository, tenantScreens);

            ObjectField Code = ObjectContext.ObjectFields.Where(d => d.FieldName == "Code" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField EnglishName = ObjectContext.ObjectFields.Where(d => d.FieldName == "EnglishName" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField AddedManually = ObjectContext.ObjectFields.Where(d => d.FieldName == "AddedManually" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();

            ScreenField Header_CodeScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = Code.Id, Row = 0, ScreenId = HeaderScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField Header_NameScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = EnglishName.Id, Row = 0, ScreenId = HeaderScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField Header_AddedManuallyScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 2, ObjectFieldId = AddedManually.Id, Row = 0, ScreenId = HeaderScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);

            ThisObject.HeaderScreenId = HeaderScreen.Id;

            ObjectContext.SaveChanges();
        }

        public void BuildCustomerScreens(Dictionary<string, Screen> tenantScreens, Dictionary<string, ScreenField> tenantScreenFields)
        {
            ObjectTable ThisObject = ObjectContext.ObjectTables.Where(d => d.Name == "Customer" && d.Tenant == 0).FirstOrDefault();

            ObjectField ObjectField01 = ObjectContext.ObjectFields.Where(d => d.ObjectTableId == ThisObject.Id && d.FieldName == "CodeMyCustomer").FirstOrDefault();
            ObjectField ObjectField02 = ObjectContext.ObjectFields.Where(d => d.ObjectTableId == ThisObject.Id && d.FieldName == "StartWorkingDate").FirstOrDefault();
            ObjectField ObjectField03 = ObjectContext.ObjectFields.Where(d => d.ObjectTableId == ThisObject.Id && d.FieldName == "PrimaryContactName").FirstOrDefault();
            ObjectField ObjectField04 = ObjectContext.ObjectFields.Where(d => d.ObjectTableId == ThisObject.Id && d.FieldName == "SalesmanUserEnglishName").FirstOrDefault();
            ObjectField ObjectField05 = ObjectContext.ObjectFields.Where(d => d.ObjectTableId == ThisObject.Id && d.FieldName == "CityWithCountry").FirstOrDefault();
            ObjectField ObjectField06 = ObjectContext.ObjectFields.Where(d => d.ObjectTableId == ThisObject.Id && d.FieldName == "LeadSourceName").FirstOrDefault();
            ObjectField ObjectField07 = ObjectContext.ObjectFields.Where(d => d.ObjectTableId == ThisObject.Id && d.FieldName == "PrimaryContactPhone").FirstOrDefault();
            ObjectField ObjectField08 = ObjectContext.ObjectFields.Where(d => d.ObjectTableId == ThisObject.Id && d.FieldName == "CustomerStatusName").FirstOrDefault();

            #region Header Screen
            Screen ClientHeaderScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "Customer.HeaderScreen", Name = "Header Screen", ObjectTableId = ThisObject.Id, NumberOfColumns = 4, NumberOfRows = 2, IsReadOnly = true }, ScreensRepository, tenantScreens);

            ScreenField screenField01 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = ObjectField01.Id, Row = 0, ScreenId = ClientHeaderScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField screenField02 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = ObjectField02.Id, Row = 0, ScreenId = ClientHeaderScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField screenField03 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 2, ObjectFieldId = ObjectField03.Id, Row = 0, ScreenId = ClientHeaderScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField screenField04 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 3, ObjectFieldId = ObjectField04.Id, Row = 0, ScreenId = ClientHeaderScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);

            ScreenField screenField05 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = ObjectField05.Id, Row = 1, ScreenId = ClientHeaderScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField screenField06 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = ObjectField06.Id, Row = 1, ScreenId = ClientHeaderScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField screenField07 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 2, ObjectFieldId = ObjectField07.Id, Row = 1, ScreenId = ClientHeaderScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField screenField08 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 3, ObjectFieldId = ObjectField08.Id, Row = 1, ScreenId = ClientHeaderScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);

            ThisObject.HeaderScreenId = ClientHeaderScreen.Id;
            #endregion

            #region GeneralTabScreen
            //Screen GeneralTabScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "Customer.GeneralTabScreen", Name = "General Tab Screen", ObjectTableId = ThisObject.Id, NumberOfColumns = 1, NumberOfRows = 8 }, ScreensRepository, tenantScreens);

            //ScreenField ScreenField01 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { ObjectFieldId = ObjectField01.Id, Column = 0, Row = 0, ScreenId = GeneralTabScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            //ScreenField ScreenField02 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { ObjectFieldId = ObjectField02.Id, Column = 0, Row = 1, ScreenId = GeneralTabScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            //ScreenField ScreenField03 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { ObjectFieldId = ObjectField03.Id, Column = 0, Row = 2, ScreenId = GeneralTabScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            //ScreenField ScreenField04 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { ObjectFieldId = objectField_VatNumber.Id, Column = 0, Row = 3, ScreenId = GeneralTabScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            //ScreenField ScreenField05 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { ObjectFieldId = objectField_PaymentTermId.Id, Column = 0, Row = 4, ScreenId = GeneralTabScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            //ScreenField screenField06 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { ObjectFieldId = ObjectField06.Id, Column = 0, Row = 5, ScreenId = GeneralTabScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            //ScreenField screenField07 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { ObjectFieldId = ObjectField07.Id, Column = 0, Row = 6, ScreenId = GeneralTabScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            #endregion

            #region BillingTabScreen
            Screen BillingTabScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "Customer.BillingTabScreen", Name = "Billing Tab Screen", ObjectTableId = ThisObject.Id, NumberOfColumns = 2, NumberOfRows = 7 }, ScreensRepository, tenantScreens);

            ObjectField billingObjectField_C1_0 = ObjectContext.ObjectFields.Where(d => d.FieldName == "InvoiceCurrencyId" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField billingObjectField_C1_1 = ObjectContext.ObjectFields.Where(d => d.FieldName == "PaymentTermId" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField billingObjectField_C1_2 = ObjectContext.ObjectFields.Where(d => d.FieldName == "VatTypeId" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();
            //ObjectField billingObjectField_C1_3 = ObjectContext.ObjectFields.Where(d => d.FieldName == "PaymentMethodCode" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField billingObjectField_C1_4 = ObjectContext.ObjectFields.Where(d => d.FieldName == "EnableConsolidationInvoices" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();

            ObjectField billingObjectField04 = ObjectContext.ObjectFields.Where(d => d.FieldName == "BankName" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField billingObjectField05 = ObjectContext.ObjectFields.Where(d => d.FieldName == "BankAddress" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField billingObjectField06 = ObjectContext.ObjectFields.Where(d => d.FieldName == "Swift" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField billingObjectField07 = ObjectContext.ObjectFields.Where(d => d.FieldName == "AccountNumber" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField billingObjectField08 = ObjectContext.ObjectFields.Where(d => d.FieldName == "IBANNumber" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();

            ScreenField billingScreenField_C0 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = billingObjectField_C1_0.Id, Row = 0, ScreenId = BillingTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField billingScreenField_C1 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = billingObjectField_C1_1.Id, Row = 1, ScreenId = BillingTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField billingScreenField_C2 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = billingObjectField_C1_2.Id, Row = 2, ScreenId = BillingTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            //ScreenField billingScreenField_C3 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = billingObjectField_C1_3.Id, Row = 3, ScreenId = BillingTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField billingScreenField_C4 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = billingObjectField_C1_4.Id, Row = 3, ScreenId = BillingTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField billingScreenField_C3 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = billingObjectField04.Id, Row = 4, ScreenId = BillingTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);

            ScreenField billingScreenField05 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = billingObjectField05.Id, Row = 0, ScreenId = BillingTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField billingScreenField06 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = billingObjectField06.Id, Row = 1, ScreenId = BillingTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField billingScreenField07 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = billingObjectField07.Id, Row = 2, ScreenId = BillingTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField billingScreenField08 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = billingObjectField08.Id, Row = 3, ScreenId = BillingTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            #endregion

            #region AccountingTabScreen
            Screen AccountingTabScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "Customer.AccountingTabScreen", Name = "Accounting Tab Screen", ObjectTableId = ThisObject.Id, NumberOfColumns = 2, NumberOfRows = 2 }, ScreensRepository, tenantScreens);
            ObjectField accountingObjectField00 = ObjectContext.ObjectFields.Where(d => d.FieldName == "ReceivablesAccountingCard" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField accountingObjectField01 = ObjectContext.ObjectFields.Where(d => d.FieldName == "PayablesAccountingCard" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();

            ScreenField accountingScreenField00 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = accountingObjectField00.Id, Row = 0, ScreenId = AccountingTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField accountingScreenField01 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = accountingObjectField01.Id, Row = 1, ScreenId = AccountingTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            #endregion

            #region Additional Fields
            Screen aditionalFieldsScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "Customer.AdditionalFields", Name = "Additional Fields", ObjectTableId = ThisObject.Id, NumberOfColumns = 1, NumberOfRows = 1, Tenant = 0 }, ScreensRepository, tenantScreens);
            #endregion

            ObjectContext.SaveChanges();
        }

        public void BuildAgentScreens(Dictionary<string, Screen> tenantScreens, Dictionary<string, ScreenField> tenantScreenFields)
        {
            ObjectTable ThisObject = ObjectContext.ObjectTables.Where(d => d.Name == "Agent" && d.Tenant == 0).FirstOrDefault();
            ObjectField ObjectField01 = ObjectContext.ObjectFields.Where(d => d.ObjectTableId == ThisObject.Id && d.FieldName == "Code").FirstOrDefault();
            ObjectField ObjectField02 = ObjectContext.ObjectFields.Where(d => d.ObjectTableId == ThisObject.Id && d.FieldName == "EnglishName").FirstOrDefault();

            #region HeaderScreen
            Screen HeaderScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "Agent.HeaderScreen", Name = "Header Screen", ObjectTableId = ThisObject.Id, NumberOfColumns = 1, NumberOfRows = 2, IsReadOnly = true }, ScreensRepository, tenantScreens);
            ScreenField Header_CodeScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = ObjectField01.Id, Row = 0, ScreenId = HeaderScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField Header_NameScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = ObjectField02.Id, Row = 1, ScreenId = HeaderScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ThisObject.HeaderScreenId = HeaderScreen.Id;
            #endregion

            #region BillingTabScreen
            Screen BillingTabScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "Agent.BillingTabScreen", Name = "Billing Tab Screen", ObjectTableId = ThisObject.Id, NumberOfColumns = 2, NumberOfRows =7 }, ScreensRepository, tenantScreens);

            ObjectField billingObjectField00 = ObjectContext.ObjectFields.Where(d => d.FieldName == "VatNumber" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField billingObjectField01 = ObjectContext.ObjectFields.Where(d => d.FieldName == "PaymentTermId" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField billingObjectField02 = ObjectContext.ObjectFields.Where(d => d.FieldName == "InvoiceCurrencyId" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField billingObjectField03 = ObjectContext.ObjectFields.Where(d => d.FieldName == "VatTypeId" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();
            //ObjectField billingObjectField04 = ObjectContext.ObjectFields.Where(d => d.FieldName == "PaymentMethodCode" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField billingObjectField33 = ObjectContext.ObjectFields.Where(d => d.FieldName == "EnableConsolidationInvoices" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField billingObjectField05 = ObjectContext.ObjectFields.Where(d => d.FieldName == "BankName" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField billingObjectField06 = ObjectContext.ObjectFields.Where(d => d.FieldName == "BankAddress" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField billingObjectField07 = ObjectContext.ObjectFields.Where(d => d.FieldName == "Swift" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField billingObjectField08 = ObjectContext.ObjectFields.Where(d => d.FieldName == "AccountNumber" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField billingObjectField09 = ObjectContext.ObjectFields.Where(d => d.FieldName == "IBANNumber" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();

            ScreenField billingScreenField00 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = billingObjectField00.Id, Row = 0, ScreenId = BillingTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField billingScreenField01 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = billingObjectField01.Id, Row = 1, ScreenId = BillingTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField billingScreenField02 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = billingObjectField02.Id, Row = 2, ScreenId = BillingTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField billingScreenField03 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = billingObjectField03.Id, Row = 3, ScreenId = BillingTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            //ScreenField billingScreenField04 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = billingObjectField04.Id, Row = 4, ScreenId = BillingTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField billingScreenField33 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = billingObjectField33.Id, Row = 5, ScreenId = BillingTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField billingScreenField05 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = billingObjectField05.Id, Row = 0, ScreenId = BillingTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField billingScreenField06 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = billingObjectField06.Id, Row = 1, ScreenId = BillingTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField billingScreenField07 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = billingObjectField07.Id, Row = 2, ScreenId = BillingTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField billingScreenField08 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = billingObjectField08.Id, Row = 3, ScreenId = BillingTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField billingScreenField09 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = billingObjectField09.Id, Row = 4, ScreenId = BillingTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            #endregion

            #region Additional Fields
            Screen aditionalFieldsScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "Agent.AdditionalFields", Name = "Additional Fields", ObjectTableId = ThisObject.Id, NumberOfColumns = 1, NumberOfRows = 1, Tenant = 0 }, ScreensRepository, tenantScreens);
            #endregion

            ObjectContext.SaveChanges();

        }

        public void BuildVendorScreens(Dictionary<string, Screen> tenantScreens, Dictionary<string, ScreenField> tenantScreenFields)
        {
            ObjectTable ThisObject = ObjectContext.ObjectTables.Where(d => d.Name == "Vendor" && d.Tenant == 0).FirstOrDefault();

            ObjectField ObjectField01 = ObjectContext.ObjectFields.Where(d => d.ObjectTableId == ThisObject.Id && d.FieldName == "Code").FirstOrDefault();
            ObjectField ObjectField02 = ObjectContext.ObjectFields.Where(d => d.ObjectTableId == ThisObject.Id && d.FieldName == "EnglishName").FirstOrDefault();
            ObjectField ObjectField03 = ObjectContext.ObjectFields.Where(d => d.ObjectTableId == ThisObject.Id && d.FieldName == "LocalName").FirstOrDefault();
            ObjectField ObjectField04 = ObjectContext.ObjectFields.Where(d => d.ObjectTableId == ThisObject.Id && d.FieldName == "Website").FirstOrDefault();
            ObjectField ObjectField05 = ObjectContext.ObjectFields.Where(d => d.ObjectTableId == ThisObject.Id && d.FieldName == "InActive").FirstOrDefault();
            ObjectField ObjectField06 = ObjectContext.ObjectFields.Where(d => d.ObjectTableId == ThisObject.Id && d.FieldName == "Notes").FirstOrDefault();

            #region HeaderScreen
            Screen HeaderScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "Vendor.HeaderScreen", Name = "Header Screen", ObjectTableId = ThisObject.Id, NumberOfColumns = 1, NumberOfRows = 2, IsReadOnly = true }, ScreensRepository, tenantScreens);
            ScreenField Header_CodeScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = ObjectField01.Id, Row = 0, ScreenId = HeaderScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField Header_NameScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = ObjectField02.Id, Row = 1, ScreenId = HeaderScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ThisObject.HeaderScreenId = HeaderScreen.Id;
            #endregion

            #region GeneralTabScreen
            Screen GeneralTabScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "Vendor.GeneralTabScreen", Name = "General Tab Screen", ObjectTableId = ThisObject.Id, NumberOfColumns = 1, NumberOfRows = 6 }, ScreensRepository, tenantScreens);
            ScreenField ScreenField01 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { ObjectFieldId = ObjectField01.Id, Column = 0, Row = 0, ScreenId = GeneralTabScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField ScreenField02 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { ObjectFieldId = ObjectField02.Id, Column = 0, Row = 1, ScreenId = GeneralTabScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField ScreenField03 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { ObjectFieldId = ObjectField03.Id, Column = 0, Row = 2, ScreenId = GeneralTabScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField ScreenField04 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { ObjectFieldId = ObjectField04.Id, Column = 0, Row = 3, ScreenId = GeneralTabScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField ScreenField05 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { ObjectFieldId = ObjectField05.Id, Column = 0, Row = 4, ScreenId = GeneralTabScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField ScreenField06 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { ObjectFieldId = ObjectField06.Id, Column = 0, Row = 5, ScreenId = GeneralTabScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            #endregion

            #region BillingTabScreen
            Screen BillingTabScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "Vendor.BillingTabScreen", Name = "Billing Tab Screen", ObjectTableId = ThisObject.Id, NumberOfColumns = 2, NumberOfRows = 6 }, ScreensRepository, tenantScreens);

            ObjectField billingObjectField00 = ObjectContext.ObjectFields.Where(d => d.FieldName == "VatNumber" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField billingObjectField01 = ObjectContext.ObjectFields.Where(d => d.FieldName == "PaymentTermId" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField billingObjectField02 = ObjectContext.ObjectFields.Where(d => d.FieldName == "InvoiceCurrencyId" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField billingObjectField03 = ObjectContext.ObjectFields.Where(d => d.FieldName == "VatTypeId" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();
            //ObjectField billingObjectField04 = ObjectContext.ObjectFields.Where(d => d.FieldName == "PaymentMethodCode" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField billingObjectField33 = ObjectContext.ObjectFields.Where(d => d.FieldName == "EnableConsolidationInvoices" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();

            ObjectField billingObjectField05 = ObjectContext.ObjectFields.Where(d => d.FieldName == "BankName" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField billingObjectField06 = ObjectContext.ObjectFields.Where(d => d.FieldName == "BankAddress" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField billingObjectField07= ObjectContext.ObjectFields.Where(d => d.FieldName == "Swift" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField billingObjectField08 = ObjectContext.ObjectFields.Where(d => d.FieldName == "AccountNumber" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField billingObjectField09 = ObjectContext.ObjectFields.Where(d => d.FieldName == "IBANNumber" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();

            ScreenField billingScreenField00 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = billingObjectField00.Id, Row = 0, ScreenId = BillingTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField billingScreenField01 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = billingObjectField01.Id, Row = 1, ScreenId = BillingTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField billingScreenField02 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = billingObjectField02.Id, Row = 2, ScreenId = BillingTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField billingScreenField03 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = billingObjectField03.Id, Row = 3, ScreenId = BillingTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            //ScreenField billingScreenField04 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = billingObjectField04.Id, Row = 4, ScreenId = BillingTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField billingScreenField33 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = billingObjectField33.Id, Row = 5, ScreenId = BillingTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);

            ScreenField billingScreenField05 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = billingObjectField05.Id, Row = 0, ScreenId = BillingTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField billingScreenField06 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = billingObjectField06.Id, Row = 1, ScreenId = BillingTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField billingScreenField07 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = billingObjectField07.Id, Row = 2, ScreenId = BillingTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField billingScreenField08 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = billingObjectField08.Id, Row = 3, ScreenId = BillingTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField billingScreenField09 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = billingObjectField09.Id, Row = 4, ScreenId = BillingTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            #endregion

            #region AccountingTabScreen
            Screen AccountingTabScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "Vendor.AccountingTabScreen", Name = "Accounting Tab Screen", ObjectTableId = ThisObject.Id, NumberOfColumns = 2, NumberOfRows = 2 }, ScreensRepository, tenantScreens);
            ObjectField accountingObjectField00 = ObjectContext.ObjectFields.Where(d => d.FieldName == "ReceivablesAccountingCard" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField accountingObjectField01 = ObjectContext.ObjectFields.Where(d => d.FieldName == "PayablesAccountingCard" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();

            ScreenField accountingScreenField00 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = accountingObjectField00.Id, Row = 0, ScreenId = AccountingTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField accountingScreenField01 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = accountingObjectField01.Id, Row = 1, ScreenId = AccountingTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            #endregion

            ObjectContext.SaveChanges();

        }

        public void BuildCustomAgentScreens(Dictionary<string, Screen> tenantScreens, Dictionary<string, ScreenField> tenantScreenFields)
        {
            ObjectTable ThisObject = ObjectContext.ObjectTables.Where(d => d.Name == "CustomAgent" && d.Tenant == 0).FirstOrDefault();
            ObjectField ObjectField01 = ObjectContext.ObjectFields.Where(d => d.ObjectTableId == ThisObject.Id && d.FieldName == "Code").FirstOrDefault();
            ObjectField ObjectField02 = ObjectContext.ObjectFields.Where(d => d.ObjectTableId == ThisObject.Id && d.FieldName == "EnglishName").FirstOrDefault();
            ObjectField ObjectField03 = ObjectContext.ObjectFields.Where(d => d.ObjectTableId == ThisObject.Id && d.FieldName == "LocalName").FirstOrDefault();
            ObjectField ObjectField04 = ObjectContext.ObjectFields.Where(d => d.ObjectTableId == ThisObject.Id && d.FieldName == "Website").FirstOrDefault();
            ObjectField ObjectField05 = ObjectContext.ObjectFields.Where(d => d.ObjectTableId == ThisObject.Id && d.FieldName == "InActive").FirstOrDefault();
            ObjectField ObjectField06 = ObjectContext.ObjectFields.Where(d => d.ObjectTableId == ThisObject.Id && d.FieldName == "Notes").FirstOrDefault();

            #region HeaderScreen
            Screen HeaderScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "CustomAgent.HeaderScreen", Name = "Header Screen", ObjectTableId = ThisObject.Id, NumberOfColumns = 1, NumberOfRows = 2, IsReadOnly = true }, ScreensRepository, tenantScreens);
            ScreenField Header_CodeScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = ObjectField01.Id, Row = 0, ScreenId = HeaderScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField Header_NameScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = ObjectField02.Id, Row = 1, ScreenId = HeaderScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ThisObject.HeaderScreenId = HeaderScreen.Id;
            #endregion

            #region GeneralTabScreen
            Screen GeneralTabScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "CustomAgent.GeneralTabScreen", Name = "General Tab Screen", ObjectTableId = ThisObject.Id, NumberOfColumns = 1, NumberOfRows = 6 }, ScreensRepository, tenantScreens);
            ScreenField ScreenField01 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { ObjectFieldId = ObjectField01.Id, Column = 0, Row = 0, ScreenId = GeneralTabScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField ScreenField02 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { ObjectFieldId = ObjectField02.Id, Column = 0, Row = 1, ScreenId = GeneralTabScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField ScreenField03 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { ObjectFieldId = ObjectField03.Id, Column = 0, Row = 2, ScreenId = GeneralTabScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField ScreenField04 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { ObjectFieldId = ObjectField04.Id, Column = 0, Row = 3, ScreenId = GeneralTabScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField ScreenField05 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { ObjectFieldId = ObjectField05.Id, Column = 0, Row = 4, ScreenId = GeneralTabScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField ScreenField06 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { ObjectFieldId = ObjectField06.Id, Column = 0, Row = 5, ScreenId = GeneralTabScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            #endregion

            #region BillingTabScreen
            Screen BillingTabScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "CustomAgent.BillingTabScreen", Name = "Billing Tab Screen", ObjectTableId = ThisObject.Id, NumberOfColumns = 2, NumberOfRows = 5 }, ScreensRepository, tenantScreens);

            ObjectField billingObjectField00 = ObjectContext.ObjectFields.Where(d => d.FieldName == "VatNumber" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField billingObjectField01 = ObjectContext.ObjectFields.Where(d => d.FieldName == "PaymentTermId" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField billingObjectField02 = ObjectContext.ObjectFields.Where(d => d.FieldName == "InvoiceCurrencyId" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField billingObjectField03 = ObjectContext.ObjectFields.Where(d => d.FieldName == "VatTypeId" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();
            //ObjectField billingObjectField04 = ObjectContext.ObjectFields.Where(d => d.FieldName == "PaymentMethodCode" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField billingObjectField33 = ObjectContext.ObjectFields.Where(d => d.FieldName == "EnableConsolidationInvoices" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();

            ObjectField billingObjectField05 = ObjectContext.ObjectFields.Where(d => d.FieldName == "BankName" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField billingObjectField06 = ObjectContext.ObjectFields.Where(d => d.FieldName == "BankAddress" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField billingObjectField07= ObjectContext.ObjectFields.Where(d => d.FieldName == "Swift" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField billingObjectField08 = ObjectContext.ObjectFields.Where(d => d.FieldName == "AccountNumber" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField billingObjectField09 = ObjectContext.ObjectFields.Where(d => d.FieldName == "IBANNumber" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();

            ScreenField billingScreenField00 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = billingObjectField00.Id, Row = 0, ScreenId = BillingTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField billingScreenField01 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = billingObjectField01.Id, Row = 1, ScreenId = BillingTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField billingScreenField02 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = billingObjectField02.Id, Row = 2, ScreenId = BillingTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField billingScreenField03 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = billingObjectField03.Id, Row = 3, ScreenId = BillingTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            //ScreenField billingScreenField04 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = billingObjectField04.Id, Row = 4, ScreenId = BillingTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField billingScreenField33 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = billingObjectField33.Id, Row = 5, ScreenId = BillingTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);

            ScreenField billingScreenField05 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = billingObjectField05.Id, Row = 0, ScreenId = BillingTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField billingScreenField06 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = billingObjectField06.Id, Row = 1, ScreenId = BillingTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField billingScreenField07 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = billingObjectField07.Id, Row = 2, ScreenId = BillingTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField billingScreenField08 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = billingObjectField08.Id, Row = 3, ScreenId = BillingTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField billingScreenField09 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = billingObjectField09.Id, Row = 4, ScreenId = BillingTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            #endregion

            #region AccountingTabScreen
            Screen AccountingTabScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "CustomAgent.AccountingTabScreen", Name = "Accounting Tab Screen", ObjectTableId = ThisObject.Id, NumberOfColumns = 2, NumberOfRows = 2 }, ScreensRepository, tenantScreens);
            ObjectField accountingObjectField00 = ObjectContext.ObjectFields.Where(d => d.FieldName == "ReceivablesAccountingCard" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField accountingObjectField01 = ObjectContext.ObjectFields.Where(d => d.FieldName == "PayablesAccountingCard" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();

            ScreenField accountingScreenField00 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = accountingObjectField00.Id, Row = 0, ScreenId = AccountingTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField accountingScreenField01 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = accountingObjectField01.Id, Row = 1, ScreenId = AccountingTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);

            #endregion

            ObjectContext.SaveChanges();
        }

        public void BuildShipppingAgentScreens(Dictionary<string, Screen> tenantScreens, Dictionary<string, ScreenField> tenantScreenFields)
        {
            ObjectTable ThisObject = ObjectContext.ObjectTables.Where(d => d.Name == "ShippingAgent" && d.Tenant == 0).FirstOrDefault();
            ObjectField ObjectField01 = ObjectContext.ObjectFields.Where(d => d.ObjectTableId == ThisObject.Id && d.FieldName == "Code").FirstOrDefault();
            ObjectField ObjectField02 = ObjectContext.ObjectFields.Where(d => d.ObjectTableId == ThisObject.Id && d.FieldName == "EnglishName").FirstOrDefault();
            ObjectField ObjectField03 = ObjectContext.ObjectFields.Where(d => d.ObjectTableId == ThisObject.Id && d.FieldName == "LocalName").FirstOrDefault();
            ObjectField ObjectField04 = ObjectContext.ObjectFields.Where(d => d.ObjectTableId == ThisObject.Id && d.FieldName == "Website").FirstOrDefault();
            ObjectField ObjectField05 = ObjectContext.ObjectFields.Where(d => d.ObjectTableId == ThisObject.Id && d.FieldName == "InActive").FirstOrDefault();
            ObjectField ObjectField06 = ObjectContext.ObjectFields.Where(d => d.ObjectTableId == ThisObject.Id && d.FieldName == "Notes").FirstOrDefault();
            ObjectField ObjectField07 = ObjectContext.ObjectFields.Where(d => d.ObjectTableId == ThisObject.Id && d.FieldName == "ForwarderAccountNumber").FirstOrDefault();
            ObjectField ObjectField08 = ObjectContext.ObjectFields.Where(d => d.ObjectTableId == ThisObject.Id && d.FieldName == "ForwarderCreditNumber").FirstOrDefault();
            ObjectField ObjectField09 = ObjectContext.ObjectFields.Where(d => d.ObjectTableId == ThisObject.Id && d.FieldName == "LocalCustomsCode").FirstOrDefault();

            #region HeaderScreen
            Screen HeaderScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "ShippingAgent.HeaderScreen", Name = "Header Screen", ObjectTableId = ThisObject.Id, NumberOfColumns = 1, NumberOfRows = 2, IsReadOnly = true }, ScreensRepository, tenantScreens);
            ScreenField Header_CodeScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = ObjectField01.Id, Row = 0, ScreenId = HeaderScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField Header_NameScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = ObjectField02.Id, Row = 1, ScreenId = HeaderScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ThisObject.HeaderScreenId = HeaderScreen.Id;
            #endregion

            #region GeneralTabScreen
            Screen GeneralTabScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "ShippingAgent.GeneralTabScreen", Name = "General Tab Screen", ObjectTableId = ThisObject.Id, NumberOfColumns = 2, NumberOfRows = 6 }, ScreensRepository, tenantScreens);
            ScreenField ScreenField01 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { ObjectFieldId = ObjectField01.Id, Column = 0, Row = 0, ScreenId = GeneralTabScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField ScreenField02 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { ObjectFieldId = ObjectField02.Id, Column = 0, Row = 1, ScreenId = GeneralTabScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField ScreenField03 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { ObjectFieldId = ObjectField03.Id, Column = 0, Row = 2, ScreenId = GeneralTabScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField ScreenField04 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { ObjectFieldId = ObjectField04.Id, Column = 0, Row = 3, ScreenId = GeneralTabScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField ScreenField05 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { ObjectFieldId = ObjectField05.Id, Column = 0, Row = 4, ScreenId = GeneralTabScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField ScreenField06 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { ObjectFieldId = ObjectField06.Id, Column = 0, Row = 5, ScreenId = GeneralTabScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);

            ScreenField ScreenField07 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { ObjectFieldId = ObjectField07.Id, Column = 1, Row = 0, ScreenId = GeneralTabScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField ScreenField08 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { ObjectFieldId = ObjectField08.Id, Column = 1, Row = 1, ScreenId = GeneralTabScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField ScreenField09 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { ObjectFieldId = ObjectField09.Id, Column = 1, Row = 2, ScreenId = GeneralTabScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);

            #endregion

            #region BillingTabScreen
            Screen BillingTabScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "ShippingAgent.BillingTabScreen", Name = "Billing Tab Screen", ObjectTableId = ThisObject.Id, NumberOfColumns = 2, NumberOfRows = 6 }, ScreensRepository, tenantScreens);

            ObjectField billingObjectField00 = ObjectContext.ObjectFields.Where(d => d.FieldName == "VatNumber" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField billingObjectField01 = ObjectContext.ObjectFields.Where(d => d.FieldName == "PaymentTermId" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField billingObjectField02 = ObjectContext.ObjectFields.Where(d => d.FieldName == "InvoiceCurrencyId" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField billingObjectField03 = ObjectContext.ObjectFields.Where(d => d.FieldName == "VatTypeId" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();
            //ObjectField billingObjectField04 = ObjectContext.ObjectFields.Where(d => d.FieldName == "PaymentMethodCode" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField billingObjectField33 = ObjectContext.ObjectFields.Where(d => d.FieldName == "EnableConsolidationInvoices" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();

            ObjectField billingObjectField05 = ObjectContext.ObjectFields.Where(d => d.FieldName == "BankName" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField billingObjectField06 = ObjectContext.ObjectFields.Where(d => d.FieldName == "BankAddress" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField billingObjectField07 = ObjectContext.ObjectFields.Where(d => d.FieldName == "Swift" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField billingObjectField08 = ObjectContext.ObjectFields.Where(d => d.FieldName == "AccountNumber" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField billingObjectField09 = ObjectContext.ObjectFields.Where(d => d.FieldName == "IBANNumber" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();

            ScreenField billingScreenField00 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = billingObjectField00.Id, Row = 0, ScreenId = BillingTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField billingScreenField01 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = billingObjectField01.Id, Row = 1, ScreenId = BillingTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField billingScreenField02 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = billingObjectField02.Id, Row = 2, ScreenId = BillingTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField billingScreenField03 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = billingObjectField03.Id, Row = 3, ScreenId = BillingTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            //ScreenField billingScreenField04 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = billingObjectField04.Id, Row = 4, ScreenId = BillingTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField billingScreenField33 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = billingObjectField33.Id, Row = 5, ScreenId = BillingTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);

            ScreenField billingScreenField05 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = billingObjectField05.Id, Row = 0, ScreenId = BillingTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField billingScreenField06 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = billingObjectField06.Id, Row = 1, ScreenId = BillingTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField billingScreenField07= AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = billingObjectField07.Id, Row = 2, ScreenId = BillingTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField billingScreenField08 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = billingObjectField08.Id, Row = 3, ScreenId = BillingTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField billingScreenField09 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = billingObjectField09.Id, Row = 4, ScreenId = BillingTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            #endregion

            #region AccountingTabScreen
            Screen AccountingTabScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "ShippingAgent.AccountingTabScreen", Name = "Accounting Tab Screen", ObjectTableId = ThisObject.Id, NumberOfColumns = 2, NumberOfRows = 2 }, ScreensRepository, tenantScreens);
            ObjectField accountingObjectField00 = ObjectContext.ObjectFields.Where(d => d.FieldName == "ReceivablesAccountingCard" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField accountingObjectField01 = ObjectContext.ObjectFields.Where(d => d.FieldName == "PayablesAccountingCard" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();

            ScreenField accountingScreenField00 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = accountingObjectField00.Id, Row = 0, ScreenId = AccountingTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField accountingScreenField01 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = accountingObjectField01.Id, Row = 1, ScreenId = AccountingTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);

            #endregion

            ObjectContext.SaveChanges();
        }

        public void BuildAirlineScreens(Dictionary<string, Screen> tenantScreens, Dictionary<string, ScreenField> tenantScreenFields)
        {
            ObjectTable ThisObject = ObjectContext.ObjectTables.Where(d => d.Name == "Airline" && d.Tenant == 0).FirstOrDefault();

            #region GeneralTabScreen
            Screen generalTabScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "Airline.GeneralTabScreen", Name = "General Tab Screen", ObjectTableId = ThisObject.Id, NumberOfColumns = 2, NumberOfRows = 7 }, ScreensRepository, tenantScreens);

            ObjectField Code = ObjectContext.ObjectFields.Where(d => d.FieldName == "Code" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField ICAO = ObjectContext.ObjectFields.Where(d => d.FieldName == "ICAO" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField EnglishName = ObjectContext.ObjectFields.Where(d => d.FieldName == "EnglishName" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField LocalName = ObjectContext.ObjectFields.Where(d => d.FieldName == "LocalName" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField Prefix = ObjectContext.ObjectFields.Where(d => d.FieldName == "Prefix" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField AddedManually = ObjectContext.ObjectFields.Where(d => d.FieldName == "AddedManually" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField TermInActive = ObjectContext.ObjectFields.Where(d => d.FieldName == "InActive" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField Remark = ObjectContext.ObjectFields.Where(d => d.FieldName == "Remark" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField Website = ObjectContext.ObjectFields.Where(d => d.FieldName == "Website" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField CheckDigit = ObjectContext.ObjectFields.Where(d => d.FieldName == "CheckDigit" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField LimitedLength = ObjectContext.ObjectFields.Where(d => d.FieldName == "LimitedLength" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField AccountNumberfield = ObjectContext.ObjectFields.Where(d => d.FieldName == "AccountNumber" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField BankAccountNumberfield = ObjectContext.ObjectFields.Where(d => d.FieldName == "BankAccountNumber" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();

            ScreenField CodeScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = Code.Id, Row = 0, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField ICAOScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = ICAO.Id, Row = 1, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField EnglishNameScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = EnglishName.Id, Row = 2, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField LocalNameScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = LocalName.Id, Row = 3, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField WebsiteScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = Website.Id, Row = 4, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField TermInActiveScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = TermInActive.Id, Row = 5, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField RemarkScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = Remark.Id, Row = 6, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);

            ScreenField PrefixScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = Prefix.Id, Row = 0, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField CheckDigitScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = CheckDigit.Id, Row = 1, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField LimitedLengthScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = LimitedLength.Id, Row = 2, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField AccountNumberScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = AccountNumberfield.Id, Row = 3, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);

            #endregion

            #region HeaderScreen
            Screen HeaderScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "Airline.HeaderScreen", Name = "Header Screen", ObjectTableId = ThisObject.Id, NumberOfColumns = 2, NumberOfRows = 2, IsReadOnly = true }, ScreensRepository, tenantScreens);
            ScreenField Header_CodeScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = Code.Id, Row = 0, ScreenId = HeaderScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField Header_NameScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = EnglishName.Id, Row = 1, ScreenId = HeaderScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField Header_AddedManuallyScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = AddedManually.Id, Row = 0, ScreenId = HeaderScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ThisObject.HeaderScreenId = HeaderScreen.Id;
            #endregion

            #region BillingTabScreen
            Screen BillingTabScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "Airline.BillingTabScreen", Name = "Billing Tab Screen", ObjectTableId = ThisObject.Id, NumberOfColumns = 2, NumberOfRows = 7 }, ScreensRepository, tenantScreens);

            ObjectField billingObjectField00 = ObjectContext.ObjectFields.Where(d => d.FieldName == "AWBAccount" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField billingObjectField01 = ObjectContext.ObjectFields.Where(d => d.FieldName == "VatNumber" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField billingObjectField02 = ObjectContext.ObjectFields.Where(d => d.FieldName == "InvoiceCurrencyId" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField billingObjectField03 = ObjectContext.ObjectFields.Where(d => d.FieldName == "PaymentTermId" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField billingObjectField04 = ObjectContext.ObjectFields.Where(d => d.FieldName == "VatTypeId" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();
            //ObjectField billingObjectField05 = ObjectContext.ObjectFields.Where(d => d.FieldName == "PaymentMethodCode" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField billingObjectField06 = ObjectContext.ObjectFields.Where(d => d.FieldName == "BankName" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField billingObjectField07 = ObjectContext.ObjectFields.Where(d => d.FieldName == "BankAddress" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField billingObjectField08 = ObjectContext.ObjectFields.Where(d => d.FieldName == "Swift" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField billingObjectField09 = ObjectContext.ObjectFields.Where(d => d.FieldName == "BankAccountNumber" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField billingObjectField10 = ObjectContext.ObjectFields.Where(d => d.FieldName == "IBANNumber" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField billingObjectField33 = ObjectContext.ObjectFields.Where(d => d.FieldName == "EnableConsolidationInvoices" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();

            ScreenField billingScreenField00 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = billingObjectField00.Id, Row = 0, ScreenId = BillingTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField billingScreenField01 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = billingObjectField01.Id, Row = 1, ScreenId = BillingTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField billingScreenField02 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = billingObjectField02.Id, Row = 2, ScreenId = BillingTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField billingScreenField03 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = billingObjectField03.Id, Row = 3, ScreenId = BillingTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField billingScreenField04 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = billingObjectField04.Id, Row = 4, ScreenId = BillingTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            //ScreenField billingScreenField05 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = billingObjectField05.Id, Row = 5, ScreenId = BillingTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField billingScreenField33 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = billingObjectField33.Id, Row = 6, ScreenId = BillingTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);

            ScreenField billingScreenField06 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = billingObjectField06.Id, Row = 0, ScreenId = BillingTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField billingScreenField07 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = billingObjectField07.Id, Row = 1, ScreenId = BillingTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField billingScreenField08 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = billingObjectField08.Id, Row = 2, ScreenId = BillingTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField billingScreenField09 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = billingObjectField09.Id, Row = 3, ScreenId = BillingTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField billingScreenField10 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = billingObjectField10.Id, Row = 4, ScreenId = BillingTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            #endregion

            #region AccountingTabScreen
            Screen AccountingTabScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "Airline.AccountingTabScreen", Name = "Accounting Tab Screen", ObjectTableId = ThisObject.Id, NumberOfColumns = 2, NumberOfRows = 2 }, ScreensRepository, tenantScreens);
            ObjectField accountingObjectField00 = ObjectContext.ObjectFields.Where(d => d.FieldName == "ReceivablesAccountingCard" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField accountingObjectField01 = ObjectContext.ObjectFields.Where(d => d.FieldName == "PayablesAccountingCard" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();

            ScreenField accountingScreenField00 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = accountingObjectField00.Id, Row = 0, ScreenId = AccountingTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField accountingScreenField01 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = accountingObjectField01.Id, Row = 1, ScreenId = AccountingTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);

            #endregion

            ObjectContext.SaveChanges();
        }

        public void BuildShippingLineScreens(Dictionary<string, Screen> tenantScreens, Dictionary<string, ScreenField> tenantScreenFields)
        {
            ObjectTable ThisObject = ObjectContext.ObjectTables.Where(d => d.Name == "ShippingLine" && d.Tenant == 0).FirstOrDefault();
            ObjectField ObjectField01 = ObjectContext.ObjectFields.Where(d => d.ObjectTableId == ThisObject.Id && d.FieldName == "Code").FirstOrDefault();
            ObjectField ObjectField02 = ObjectContext.ObjectFields.Where(d => d.ObjectTableId == ThisObject.Id && d.FieldName == "SCACCode").FirstOrDefault();
            ObjectField ObjectField03 = ObjectContext.ObjectFields.Where(d => d.ObjectTableId == ThisObject.Id && d.FieldName == "EnglishName").FirstOrDefault();
            ObjectField ObjectField04 = ObjectContext.ObjectFields.Where(d => d.ObjectTableId == ThisObject.Id && d.FieldName == "LocalName").FirstOrDefault();
            ObjectField ObjectField05 = ObjectContext.ObjectFields.Where(d => d.ObjectTableId == ThisObject.Id && d.FieldName == "ShippingAgentId").FirstOrDefault();
            ObjectField ObjectField06 = ObjectContext.ObjectFields.Where(d => d.ObjectTableId == ThisObject.Id && d.FieldName == "Website").FirstOrDefault();
            ObjectField ObjectField07 = ObjectContext.ObjectFields.Where(d => d.ObjectTableId == ThisObject.Id && d.FieldName == "InActive").FirstOrDefault();
            ObjectField ObjectField08 = ObjectContext.ObjectFields.Where(d => d.ObjectTableId == ThisObject.Id && d.FieldName == "Remark").FirstOrDefault();

            #region HeaderScreen
            Screen HeaderScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "ShippingLine.HeaderScreen", Name = "Header Screen", ObjectTableId = ThisObject.Id, NumberOfColumns = 1, NumberOfRows = 2, IsReadOnly = true }, ScreensRepository, tenantScreens);
            ScreenField Header_CodeScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = ObjectField01.Id, Row = 0, ScreenId = HeaderScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField Header_NameScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = ObjectField03.Id, Row = 1, ScreenId = HeaderScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ThisObject.HeaderScreenId = HeaderScreen.Id;
            #endregion

            #region GeneralTabScreen
            Screen GeneralTabScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "ShippingLine.GeneralTabScreen", Name = "General Tab Screen", ObjectTableId = ThisObject.Id, NumberOfColumns = 1, NumberOfRows = 8 }, ScreensRepository, tenantScreens);
            ScreenField ScreenField01 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { ObjectFieldId = ObjectField01.Id, Column = 0, Row = 0, ScreenId = GeneralTabScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField ScreenField02 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { ObjectFieldId = ObjectField02.Id, Column = 0, Row = 1, ScreenId = GeneralTabScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField ScreenField03 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { ObjectFieldId = ObjectField03.Id, Column = 0, Row = 2, ScreenId = GeneralTabScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField ScreenField04 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { ObjectFieldId = ObjectField04.Id, Column = 0, Row = 3, ScreenId = GeneralTabScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField ScreenField05 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { ObjectFieldId = ObjectField05.Id, Column = 0, Row = 4, ScreenId = GeneralTabScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField ScreenField06 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { ObjectFieldId = ObjectField06.Id, Column = 0, Row = 5, ScreenId = GeneralTabScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField ScreenField07 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { ObjectFieldId = ObjectField07.Id, Column = 0, Row = 6, ScreenId = GeneralTabScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField ScreenField08 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { ObjectFieldId = ObjectField08.Id, Column = 0, Row = 7, ScreenId = GeneralTabScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            #endregion

            #region BillingTabScreen
            Screen BillingTabScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "ShippingLine.BillingTabScreen", Name = "Billing Tab Screen", ObjectTableId = ThisObject.Id, NumberOfColumns = 2, NumberOfRows = 7 }, ScreensRepository, tenantScreens);

            ObjectField billingObjectField00 = ObjectContext.ObjectFields.Where(d => d.FieldName == "OurCreditNumber" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField billingObjectField01 = ObjectContext.ObjectFields.Where(d => d.FieldName == "VatNumber" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField billingObjectField02 = ObjectContext.ObjectFields.Where(d => d.FieldName == "InvoiceCurrencyId" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField billingObjectField03 = ObjectContext.ObjectFields.Where(d => d.FieldName == "PaymentTermId" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField billingObjectField04 = ObjectContext.ObjectFields.Where(d => d.FieldName == "VatTypeId" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();
            //ObjectField billingObjectField05 = ObjectContext.ObjectFields.Where(d => d.FieldName == "PaymentMethodCode" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();

            ObjectField billingObjectField06 = ObjectContext.ObjectFields.Where(d => d.FieldName == "BankName" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField billingObjectField07 = ObjectContext.ObjectFields.Where(d => d.FieldName == "BankAddress" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField billingObjectField08 = ObjectContext.ObjectFields.Where(d => d.FieldName == "Swift" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField billingObjectField09 = ObjectContext.ObjectFields.Where(d => d.FieldName == "AccountNumber" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField billingObjectField10 = ObjectContext.ObjectFields.Where(d => d.FieldName == "IBANNumber" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField billingObjectField33 = ObjectContext.ObjectFields.Where(d => d.FieldName == "EnableConsolidationInvoices" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();

            ScreenField billingScreenField00 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = billingObjectField00.Id, Row = 0, ScreenId = BillingTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField billingScreenField01 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = billingObjectField01.Id, Row = 1, ScreenId = BillingTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField billingScreenField02 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = billingObjectField02.Id, Row = 2, ScreenId = BillingTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField billingScreenField03 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = billingObjectField03.Id, Row = 3, ScreenId = BillingTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField billingScreenField04 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = billingObjectField04.Id, Row = 4, ScreenId = BillingTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            //ScreenField billingScreenField05 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = billingObjectField05.Id, Row = 5, ScreenId = BillingTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField billingScreenField33 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = billingObjectField33.Id, Row = 6, ScreenId = BillingTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);

            ScreenField billingScreenField06 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = billingObjectField06.Id, Row = 0, ScreenId = BillingTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField billingScreenField07 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = billingObjectField07.Id, Row = 1, ScreenId = BillingTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField billingScreenField08 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = billingObjectField08.Id, Row = 2, ScreenId = BillingTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField billingScreenField09 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = billingObjectField09.Id, Row = 3, ScreenId = BillingTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField billingScreenField010 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = billingObjectField10.Id, Row = 4, ScreenId = BillingTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            #endregion

            #region AccountingTabScreen
            Screen AccountingTabScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "ShippingLine.AccountingTabScreen", Name = "Accounting Tab Screen", ObjectTableId = ThisObject.Id, NumberOfColumns = 2, NumberOfRows = 2 }, ScreensRepository, tenantScreens);
            ObjectField accountingObjectField00 = ObjectContext.ObjectFields.Where(d => d.FieldName == "ReceivablesAccountingCard" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField accountingObjectField01 = ObjectContext.ObjectFields.Where(d => d.FieldName == "PayablesAccountingCard" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();

            ScreenField accountingScreenField00 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = accountingObjectField00.Id, Row = 0, ScreenId = AccountingTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField accountingScreenField01 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = accountingObjectField01.Id, Row = 1, ScreenId = AccountingTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);

            #endregion

            ObjectContext.SaveChanges();
        }

        public void BuildTruckerScreens(Dictionary<string, Screen> tenantScreens, Dictionary<string, ScreenField> tenantScreenFields)
        {
            ObjectTable ThisObject = ObjectContext.ObjectTables.Where(d => d.Name == "Trucker" && d.Tenant == 0).FirstOrDefault();
            ObjectField ObjectField01 = ObjectContext.ObjectFields.Where(d => d.ObjectTableId == ThisObject.Id && d.FieldName == "Code").FirstOrDefault();
            ObjectField ObjectField02 = ObjectContext.ObjectFields.Where(d => d.ObjectTableId == ThisObject.Id && d.FieldName == "EnglishName").FirstOrDefault();
            ObjectField ObjectField03 = ObjectContext.ObjectFields.Where(d => d.ObjectTableId == ThisObject.Id && d.FieldName == "LocalName").FirstOrDefault();
            ObjectField ObjectField04 = ObjectContext.ObjectFields.Where(d => d.ObjectTableId == ThisObject.Id && d.FieldName == "Website").FirstOrDefault();
            ObjectField ObjectField05 = ObjectContext.ObjectFields.Where(d => d.ObjectTableId == ThisObject.Id && d.FieldName == "InActive").FirstOrDefault();
            ObjectField ObjectField06 = ObjectContext.ObjectFields.Where(d => d.ObjectTableId == ThisObject.Id && d.FieldName == "Remark").FirstOrDefault();

            #region HeaderScreen
            Screen HeaderScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "Trucker.HeaderScreen", Name = "Header Screen", ObjectTableId = ThisObject.Id, NumberOfColumns = 1, NumberOfRows = 2, IsReadOnly = true }, ScreensRepository, tenantScreens);
            ScreenField Header_CodeScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = ObjectField01.Id, Row = 0, ScreenId = HeaderScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField Header_NameScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = ObjectField03.Id, Row = 1, ScreenId = HeaderScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ThisObject.HeaderScreenId = HeaderScreen.Id;
            #endregion

            #region GeneralTabScreen
            Screen GeneralTabScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "Trucker.GeneralTabScreen", Name = "General Tab Screen", ObjectTableId = ThisObject.Id, NumberOfColumns = 1, NumberOfRows = 6 }, ScreensRepository, tenantScreens);
            ScreenField ScreenField01 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { ObjectFieldId = ObjectField01.Id, Column = 0, Row = 0, ScreenId = GeneralTabScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField ScreenField02 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { ObjectFieldId = ObjectField02.Id, Column = 0, Row = 1, ScreenId = GeneralTabScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField ScreenField03 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { ObjectFieldId = ObjectField03.Id, Column = 0, Row = 2, ScreenId = GeneralTabScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField ScreenField04 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { ObjectFieldId = ObjectField04.Id, Column = 0, Row = 3, ScreenId = GeneralTabScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField ScreenField05 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { ObjectFieldId = ObjectField05.Id, Column = 0, Row = 4, ScreenId = GeneralTabScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField ScreenField06 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { ObjectFieldId = ObjectField06.Id, Column = 0, Row = 5, ScreenId = GeneralTabScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            #endregion

            #region BillingTabScreen
            Screen BillingTabScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "Trucker.BillingTabScreen", Name = "Billing Tab Screen", ObjectTableId = ThisObject.Id, NumberOfColumns = 2, NumberOfRows = 6 }, ScreensRepository, tenantScreens);

            ObjectField billingObjectField00 = ObjectContext.ObjectFields.Where(d => d.FieldName == "VatNumber" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField billingObjectField01 = ObjectContext.ObjectFields.Where(d => d.FieldName == "InvoiceCurrencyId" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField billingObjectField02 = ObjectContext.ObjectFields.Where(d => d.FieldName == "PaymentTermId" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField billingObjectField03 = ObjectContext.ObjectFields.Where(d => d.FieldName == "VatTypeId" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();
            //ObjectField billingObjectField04 = ObjectContext.ObjectFields.Where(d => d.FieldName == "PaymentMethodCode" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();

            ObjectField billingObjectField05 = ObjectContext.ObjectFields.Where(d => d.FieldName == "BankName" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField billingObjectField06 = ObjectContext.ObjectFields.Where(d => d.FieldName == "BankAddress" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField billingObjectField07 = ObjectContext.ObjectFields.Where(d => d.FieldName == "Swift" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField billingObjectField08 = ObjectContext.ObjectFields.Where(d => d.FieldName == "AccountNumber" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField billingObjectField09 = ObjectContext.ObjectFields.Where(d => d.FieldName == "IBANNumber" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField billingObjectField33 = ObjectContext.ObjectFields.Where(d => d.FieldName == "EnableConsolidationInvoices" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();

            ScreenField billingScreenField00 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = billingObjectField00.Id, Row = 0, ScreenId = BillingTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField billingScreenField01 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = billingObjectField01.Id, Row = 1, ScreenId = BillingTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField billingScreenField02 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = billingObjectField02.Id, Row = 2, ScreenId = BillingTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField billingScreenField03 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = billingObjectField03.Id, Row = 3, ScreenId = BillingTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            //ScreenField billingScreenField04 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = billingObjectField04.Id, Row = 4, ScreenId = BillingTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField billingScreenField33 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = billingObjectField33.Id, Row = 5, ScreenId = BillingTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);

            ScreenField billingScreenField05 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = billingObjectField05.Id, Row = 0, ScreenId = BillingTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField billingScreenField06 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = billingObjectField06.Id, Row = 1, ScreenId = BillingTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField billingScreenField07 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = billingObjectField07.Id, Row = 2, ScreenId = BillingTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField billingScreenField08 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = billingObjectField08.Id, Row = 3, ScreenId = BillingTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField billingScreenField09 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = billingObjectField09.Id, Row = 4, ScreenId = BillingTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            #endregion

            #region AccountingTabScreen
            Screen AccountingTabScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "Trucker.AccountingTabScreen", Name = "Accounting Tab Screen", ObjectTableId = ThisObject.Id, NumberOfColumns = 2, NumberOfRows = 2 }, ScreensRepository, tenantScreens);
            ObjectField accountingObjectField00 = ObjectContext.ObjectFields.Where(d => d.FieldName == "ReceivablesAccountingCard" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField accountingObjectField01 = ObjectContext.ObjectFields.Where(d => d.FieldName == "PayablesAccountingCard" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();

            ScreenField accountingScreenField00 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = accountingObjectField00.Id, Row = 0, ScreenId = AccountingTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField accountingScreenField01 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = accountingObjectField01.Id, Row = 1, ScreenId = AccountingTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);

            #endregion

            ObjectContext.SaveChanges();
        }

        public void BuildVesselScreens(Dictionary<string, Screen> tenantScreens, Dictionary<string, ScreenField> tenantScreenFields)
        {
            ObjectTable VesselObject = ObjectContext.ObjectTables.Where(d => d.Name == "Vessel" && d.Tenant == 0).FirstOrDefault();

            // General Screen
            //Screen generalTabScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "Vessel.GeneralTabScreen", Name = "General Tab Screen", ObjectTableId = VesselObject.Id, NumberOfColumns = 2, NumberOfRows = 5 }, ScreensRepository, tenantScreens);

            ObjectField VesselCode = ObjectContext.ObjectFields.Where(d => d.FieldName == "Code" && d.ObjectTableId == VesselObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField VesselName = ObjectContext.ObjectFields.Where(d => d.FieldName == "EnglishName" && d.ObjectTableId == VesselObject.Id && d.Tenant == 0).FirstOrDefault();
            //ObjectField VesselIMOCode = ObjectContext.ObjectFields.Where(d => d.FieldName == "IMOCode" && d.ObjectTableId == VesselObject.Id && d.Tenant == 0).FirstOrDefault();
            //ObjectField VesselLocalName = ObjectContext.ObjectFields.Where(d => d.FieldName == "LocalName" && d.ObjectTableId == VesselObject.Id && d.Tenant == 0).FirstOrDefault();
            //ObjectField VesselNotes = ObjectContext.ObjectFields.Where(d => d.FieldName == "Notes" && d.ObjectTableId == VesselObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField VesselAddedManually = ObjectContext.ObjectFields.Where(d => d.FieldName == "AddedManually" && d.ObjectTableId == VesselObject.Id && d.Tenant == 0).FirstOrDefault();
            //ObjectField VesselCountryId = ObjectContext.ObjectFields.Where(d => d.FieldName == "CountryId" && d.ObjectTableId == VesselObject.Id && d.Tenant == 0).FirstOrDefault();

            //ScreenField VesselNameScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = VesselName.Id, Row = 0, ScreenId = generalTabScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            //ScreenField VesselIMOCodeScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = VesselIMOCode.Id, Row = 1, ScreenId = generalTabScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            //ScreenField VesselLocalNameScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = VesselLocalName.Id, Row = 2, ScreenId = generalTabScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            //ScreenField VesselCountryIdScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = VesselCountryId.Id, Row = 3, ScreenId = generalTabScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            //ScreenField VesselNotesScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = VesselNotes.Id, Row = 0, ScreenId = generalTabScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            
            //==============Header screen===========================
            Screen HeaderScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "Vessel.HeaderScreen", Name = "Header Screen", ObjectTableId = VesselObject.Id, NumberOfColumns = 1, NumberOfRows = 3, IsReadOnly = true }, ScreensRepository, tenantScreens);
            
            ScreenField Header_CodeScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = VesselCode.Id, Row = 0, ScreenId = HeaderScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField Header_NameScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = VesselName.Id, Row = 1, ScreenId = HeaderScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField Header_AddedManuallyScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = VesselAddedManually.Id, Row = 2, ScreenId = HeaderScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            VesselObject.HeaderScreenId = HeaderScreen.Id;

            ObjectContext.SaveChanges();

        }

        public void BuildWareHouseScreens(Dictionary<string, Screen> tenantScreens, Dictionary<string, ScreenField> tenantScreenFields)
        {
            ObjectTable WareHouseObject = ObjectContext.ObjectTables.Where(d => d.Name == "Warehouse" && d.Tenant == 0).FirstOrDefault();

            ObjectField WareHouseObjectCode = ObjectContext.ObjectFields.Where(d => d.FieldName == "Code" && d.ObjectTableId == WareHouseObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField WareHouseObjectName = ObjectContext.ObjectFields.Where(d => d.FieldName == "EnglishName" && d.ObjectTableId == WareHouseObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField WareHouseObjectLocalName = ObjectContext.ObjectFields.Where(d => d.FieldName == "LocalName" && d.ObjectTableId == WareHouseObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField WareHouseObjectInActive = ObjectContext.ObjectFields.Where(d => d.FieldName == "InActive" && d.ObjectTableId == WareHouseObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField WareHouseObjectNotes = ObjectContext.ObjectFields.Where(d => d.FieldName == "Notes" && d.ObjectTableId == WareHouseObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField WareHouseObjectAddedManually = ObjectContext.ObjectFields.Where(d => d.FieldName == "AddedManually" && d.ObjectTableId == WareHouseObject.Id && d.Tenant == 0).FirstOrDefault();

            //#region General Screen
            //Screen generalTabScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "Warehouse.GeneralTabScreen", Name = "General Tab Screen", ObjectTableId = WareHouseObject.Id, NumberOfColumns = 2, NumberOfRows =5 }, ScreensRepository, tenantScreens);

            //ScreenField WareHouseCodeScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = WareHouseObjectCode.Id, Row = 0, ScreenId = generalTabScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            //ScreenField WareHouseNameScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = WareHouseObjectName.Id, Row = 1, ScreenId = generalTabScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            //ScreenField WareHouseLocalNameScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = WareHouseObjectLocalName.Id, Row = 2, ScreenId = generalTabScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            //ScreenField WareHouseInActiveScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = WareHouseObjectInActive.Id, Row = 3, ScreenId = generalTabScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            //ScreenField WareHouseNotesScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = WareHouseObjectNotes.Id, Row = 0, ScreenId = generalTabScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            //#endregion

            #region Header screen
            Screen HeaderScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "Warehouse.HeaderScreen", Name = "Header Screen", ObjectTableId = WareHouseObject.Id, NumberOfColumns = 1, NumberOfRows = 3, IsReadOnly = true }, ScreensRepository, tenantScreens);

            ScreenField Header_CodeScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = WareHouseObjectCode.Id, Row = 0, ScreenId = HeaderScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField Header_NameScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = WareHouseObjectName.Id, Row = 1, ScreenId = HeaderScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField Header_AddedManuallyScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = WareHouseObjectAddedManually.Id, Row = 2, ScreenId = HeaderScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);

            WareHouseObject.HeaderScreenId = HeaderScreen.Id;
            #endregion

            #region Billing Screen
            Screen BillingTabScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "Warehouse.BillingTabScreen", Name = "Billing Tab Screen", ObjectTableId = WareHouseObject.Id, NumberOfColumns = 2, NumberOfRows = 6 }, ScreensRepository, tenantScreens);

            ObjectField billingObjectField00 = ObjectContext.ObjectFields.Where(d => d.FieldName == "VatNumber" && d.ObjectTableId == WareHouseObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField billingObjectField01 = ObjectContext.ObjectFields.Where(d => d.FieldName == "InvoiceCurrencyId" && d.ObjectTableId == WareHouseObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField billingObjectField02 = ObjectContext.ObjectFields.Where(d => d.FieldName == "PaymentTermId" && d.ObjectTableId == WareHouseObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField billingObjectField03 = ObjectContext.ObjectFields.Where(d => d.FieldName == "VatTypeId" && d.ObjectTableId == WareHouseObject.Id && d.Tenant == 0).FirstOrDefault();
            //ObjectField billingObjectField04 = ObjectContext.ObjectFields.Where(d => d.FieldName == "PaymentMethodCode" && d.ObjectTableId == WareHouseObject.Id && d.Tenant == 0).FirstOrDefault();

            ObjectField billingObjectField05 = ObjectContext.ObjectFields.Where(d => d.FieldName == "BankName" && d.ObjectTableId == WareHouseObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField billingObjectField06 = ObjectContext.ObjectFields.Where(d => d.FieldName == "BankAddress" && d.ObjectTableId == WareHouseObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField billingObjectField07 = ObjectContext.ObjectFields.Where(d => d.FieldName == "Swift" && d.ObjectTableId == WareHouseObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField billingObjectField08 = ObjectContext.ObjectFields.Where(d => d.FieldName == "AccountNumber" && d.ObjectTableId == WareHouseObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField billingObjectField09 = ObjectContext.ObjectFields.Where(d => d.FieldName == "IBANNumber" && d.ObjectTableId == WareHouseObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField billingObjectField33 = ObjectContext.ObjectFields.Where(d => d.FieldName == "EnableConsolidationInvoices" && d.ObjectTableId == WareHouseObject.Id && d.Tenant == 0).FirstOrDefault();

            ScreenField billingScreenField00 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = billingObjectField00.Id, Row = 0, ScreenId = BillingTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField billingScreenField01 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = billingObjectField01.Id, Row = 1, ScreenId = BillingTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField billingScreenField02 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = billingObjectField02.Id, Row = 2, ScreenId = BillingTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField billingScreenField03 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = billingObjectField03.Id, Row = 3, ScreenId = BillingTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            //ScreenField billingScreenField04 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = billingObjectField04.Id, Row = 4, ScreenId = BillingTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField billingScreenField33 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = billingObjectField33.Id, Row = 5, ScreenId = BillingTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);

            ScreenField billingScreenField05 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = billingObjectField05.Id, Row = 0, ScreenId = BillingTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField billingScreenField06 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = billingObjectField06.Id, Row = 1, ScreenId = BillingTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField billingScreenField07 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = billingObjectField07.Id, Row = 2, ScreenId = BillingTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField billingScreenField08 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = billingObjectField08.Id, Row = 3, ScreenId = BillingTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField billingScreenField09 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = billingObjectField09.Id, Row = 4, ScreenId = BillingTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            #endregion

            #region AccountingTabScreen
            Screen AccountingTabScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "Warehouse.AccountingTabScreen", Name = "Accounting Tab Screen", ObjectTableId = WareHouseObject.Id, NumberOfColumns = 2, NumberOfRows = 2 }, ScreensRepository, tenantScreens);
            ObjectField accountingObjectField00 = ObjectContext.ObjectFields.Where(d => d.FieldName == "ReceivablesAccountingCard" && d.ObjectTableId == WareHouseObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField accountingObjectField01 = ObjectContext.ObjectFields.Where(d => d.FieldName == "PayablesAccountingCard" && d.ObjectTableId == WareHouseObject.Id && d.Tenant == 0).FirstOrDefault();

            ScreenField accountingScreenField00 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = accountingObjectField00.Id, Row = 0, ScreenId = AccountingTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField accountingScreenField01 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = accountingObjectField01.Id, Row = 1, ScreenId = AccountingTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);

            #endregion

            ObjectContext.SaveChanges();
        }

        public void BuildUserScreens(Dictionary<string, Screen> tenantScreens, Dictionary<string, ScreenField> tenantScreenFields)
        {
            ObjectTable ThisObject = ObjectContext.ObjectTables.Where(d => d.Name == "User" && d.Tenant == 0).FirstOrDefault();
            ObjectField ObjectField01 = ObjectContext.ObjectFields.Where(d => d.ObjectTableId == ThisObject.Id && d.FieldName == "Email").FirstOrDefault();

            #region Header Screen
            Screen HeaderScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "User.HeaderScreen", Name = "Header Screen", ObjectTableId = ThisObject.Id, NumberOfColumns = 1, NumberOfRows = 1, IsReadOnly = true }, ScreensRepository, tenantScreens);
            ScreenField Header_NameScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = ObjectField01.Id, Row = 0, ScreenId = HeaderScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ThisObject.HeaderScreenId = HeaderScreen.Id;
            #endregion

            ObjectContext.SaveChanges();

        }

        public void BuildContactScreens(Dictionary<string, Screen> tenantScreens, Dictionary<string, ScreenField> tenantScreenFields)
        {
            ObjectTable ThisObject = ObjectContext.ObjectTables.Where(d => d.Name == "Contact" && d.Tenant == 0).FirstOrDefault();

            ObjectField EnglishName = ObjectContext.ObjectFields.Where(d => d.FieldName == "EnglishName" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField IsUser = ObjectContext.ObjectFields.Where(d => d.FieldName == "IsUser" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();

            Screen HeaderScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "Contact.HeaderScreen", Name = "Header Screen", ObjectTableId = ThisObject.Id, NumberOfColumns = 2, NumberOfRows = 1, IsReadOnly = true }, ScreensRepository, tenantScreens);
            ScreenField Header_NameScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = EnglishName.Id, Row = 0, ScreenId = HeaderScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField Header_IsUserScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = IsUser.Id, Row = 0, ScreenId = HeaderScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);

            ThisObject.HeaderScreenId = HeaderScreen.Id;
            ObjectContext.SaveChanges();
        }

        public void BuildMainAddressForClientScreens(Dictionary<string, Screen> tenantScreens, Dictionary<string, ScreenField> tenantScreenFields)
        {
            //ObjectTable ThisObject = ObjectContext.ObjectTables.Where(d => d.Name == "Address" && d.Tenant == 0).FirstOrDefault();
            //// General Screen
            //Screen MainAddressTabScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "Customers.Addresses.MainAddress", Name = "Main Address", ObjectTableId = ThisObject.Id, NumberOfColumns = 2, NumberOfRows = 7 }, ScreensRepository, tenantScreens);

            //ObjectField Name = ObjectContext.ObjectFields.Where(d => d.FieldName == "Name" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();
            //ObjectField Address1 = ObjectContext.ObjectFields.Where(d => d.FieldName == "Address1" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();
            //ObjectField Address2 = ObjectContext.ObjectFields.Where(d => d.FieldName == "Address2" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();
            //ObjectField City = ObjectContext.ObjectFields.Where(d => d.FieldName == "City" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();
            //ObjectField Country = ObjectContext.ObjectFields.Where(d => d.FieldName == "CountryId" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();
            //ObjectField State = ObjectContext.ObjectFields.Where(d => d.FieldName == "StateId" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();
            //ObjectField ZipCode = ObjectContext.ObjectFields.Where(d => d.FieldName == "ZipCode" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();
            //ObjectField PhoneNumber = ObjectContext.ObjectFields.Where(d => d.FieldName == "PhoneNumber" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();
            //ObjectField FaxNumber = ObjectContext.ObjectFields.Where(d => d.FieldName == "FaxNumber" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();
            //ObjectField ATTN = ObjectContext.ObjectFields.Where(d => d.FieldName == "ATTN" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();

            //ScreenField NameScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = Name.Id, Row = 0, ScreenId = MainAddressTabScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            //ScreenField Address1ScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = Address1.Id, Row = 1, ScreenId = MainAddressTabScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            //ScreenField Address2NameScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = Address2.Id, Row = 2, ScreenId = MainAddressTabScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            //ScreenField CityScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = City.Id, Row = 3, ScreenId = MainAddressTabScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            //ScreenField CountryScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = Country.Id, Row = 4, ScreenId = MainAddressTabScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            //ScreenField StateScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = State.Id, Row = 5, ScreenId = MainAddressTabScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            //ScreenField ZipCodeScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = ZipCode.Id, Row = 6, ScreenId = MainAddressTabScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            //ScreenField PhoneNumberScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = PhoneNumber.Id, Row = 4, ScreenId = MainAddressTabScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            //ScreenField FaxNumberScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = FaxNumber.Id, Row = 5, ScreenId = MainAddressTabScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            //ScreenField ATTNScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = ATTN.Id, Row = 6, ScreenId = MainAddressTabScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);

            //ObjectContext.SaveChanges();
        }

        private void BuildARInvoiceScreens(Dictionary<string, Screen> tenantScreens, Dictionary<string, ScreenField> tenantScreenFields)
        {

            ObjectTable ThisObject = ObjectContext.ObjectTables.Where(d => d.Name == "ARInvoice" && d.Tenant == 0).FirstOrDefault();

            #region Header screen
            Screen HeaderScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "ARInvoice.HeaderScreen", Name = "Header Screen", ObjectTableId = ThisObject.Id, NumberOfColumns = 5, NumberOfRows = 1, IsReadOnly = true }, ScreensRepository, tenantScreens);
            ObjectField ObjectField1 = ObjectContext.ObjectFields.Where(d => d.FieldName == "ShipmentsNumbers" && d.ObjectTableId == ThisObject.Id).FirstOrDefault();
            ObjectField ObjectField2 = ObjectContext.ObjectFields.Where(d => d.FieldName == "StatusName" && d.ObjectTableId == ThisObject.Id).FirstOrDefault();
            ObjectField ObjectField3 = ObjectContext.ObjectFields.Where(d => d.FieldName == "AmountDue" && d.ObjectTableId == ThisObject.Id).FirstOrDefault();
            ObjectField ObjectField4 = ObjectContext.ObjectFields.Where(d => d.FieldName == "DueDate" && d.ObjectTableId == ThisObject.Id).FirstOrDefault();
            ObjectField ObjectField5 = ObjectContext.ObjectFields.Where(d => d.FieldName == "TransferStatusName" && d.ObjectTableId == ThisObject.Id).FirstOrDefault();
            //ObjectField ObjectField6 = ObjectContext.ObjectFields.Where(d => d.FieldName == "SATTransferStatusName" && d.ObjectTableId == ThisObject.Id).FirstOrDefault();


            ScreenField ScreenField1 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 0, ObjectFieldId = ObjectField1.Id, ScreenId = HeaderScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField ScreenField2 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, Row = 0, ObjectFieldId = ObjectField4.Id, ScreenId = HeaderScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField ScreenField3 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 2, Row = 0, ObjectFieldId = ObjectField3.Id, ScreenId = HeaderScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField ScreenField4 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 3, Row = 0, ObjectFieldId = ObjectField2.Id, ScreenId = HeaderScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField ScreenField5 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 4, Row = 0, ObjectFieldId = ObjectField5.Id, ScreenId = HeaderScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            //ScreenField ScreenField6 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 5, Row = 0, ObjectFieldId = ObjectField6.Id, ScreenId = HeaderScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);

            ThisObject.HeaderScreenId = HeaderScreen.Id;
            #endregion

            #region Header screen - Full Accounting
            Screen HeaderScreen4FullAccounting = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "ARInvoice.FullAccHeaderScreen", Name = "Header Screen", ObjectTableId = ThisObject.Id, NumberOfColumns = 5, NumberOfRows = 1, IsReadOnly = true }, ScreensRepository, tenantScreens);
            ObjectField FAObjectField1 = ObjectContext.ObjectFields.Where(d => d.FieldName == "ShipmentsNumbers" && d.ObjectTableId == ThisObject.Id).FirstOrDefault();
            ObjectField FAObjectField2 = ObjectContext.ObjectFields.Where(d => d.FieldName == "StatusName" && d.ObjectTableId == ThisObject.Id).FirstOrDefault();
            ObjectField FAObjectField3 = ObjectContext.ObjectFields.Where(d => d.FieldName == "AmountDue" && d.ObjectTableId == ThisObject.Id).FirstOrDefault();
            ObjectField FAObjectField4 = ObjectContext.ObjectFields.Where(d => d.FieldName == "DueDate" && d.ObjectTableId == ThisObject.Id).FirstOrDefault();
            ObjectField FAObjectField5 = ObjectContext.ObjectFields.Where(d => d.FieldName == "JournalNumber" && d.ObjectTableId == ThisObject.Id).FirstOrDefault();


            ScreenField FAScreenField1 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 0, ObjectFieldId = FAObjectField1.Id, ScreenId = HeaderScreen4FullAccounting.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField FAScreenField2 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, Row = 0, ObjectFieldId = FAObjectField4.Id, ScreenId = HeaderScreen4FullAccounting.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField FAScreenField3 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 2, Row = 0, ObjectFieldId = FAObjectField3.Id, ScreenId = HeaderScreen4FullAccounting.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField FAScreenField4 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 3, Row = 0, ObjectFieldId = FAObjectField2.Id, ScreenId = HeaderScreen4FullAccounting.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField FAScreenField5 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 4, Row = 0, ObjectFieldId = FAObjectField5.Id, ScreenId = HeaderScreen4FullAccounting.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);

            #endregion

            #region General Tab Screen
            Screen generalTabScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "ARInvoice.GeneralTabScreen", Name = "General Tab Screen", ObjectTableId = ThisObject.Id, NumberOfColumns = 2, NumberOfRows = 10 }, ScreensRepository, tenantScreens);
            ObjectField GObjectField01 = ObjectContext.ObjectFields.Where(d => d.ObjectTableId == ThisObject.Id && d.Tenant == 0 && d.FieldName == "UpdatedByUserId").FirstOrDefault();
            ObjectField GObjectField02 = ObjectContext.ObjectFields.Where(d => d.ObjectTableId == ThisObject.Id && d.Tenant == 0 && d.FieldName == "UpdateDate").FirstOrDefault();
            ObjectField GObjectField03 = ObjectContext.ObjectFields.Where(d => d.ObjectTableId == ThisObject.Id && d.Tenant == 0 && d.FieldName == "Sent").FirstOrDefault();
            ObjectField GObjectField04 = ObjectContext.ObjectFields.Where(d => d.ObjectTableId == ThisObject.Id && d.Tenant == 0 && d.FieldName == "HouseNumber").FirstOrDefault();
            ObjectField GObjectField05 = ObjectContext.ObjectFields.Where(d => d.ObjectTableId == ThisObject.Id && d.Tenant == 0 && d.FieldName == "MasterNumber").FirstOrDefault();
            ObjectField GObjectField06 = ObjectContext.ObjectFields.Where(d => d.ObjectTableId == ThisObject.Id && d.Tenant == 0 && d.FieldName == "BranchId").FirstOrDefault();
            ObjectField GObjectField07 = ObjectContext.ObjectFields.Where(d => d.ObjectTableId == ThisObject.Id && d.Tenant == 0 && d.FieldName == "CustomerRef").FirstOrDefault();
            //ObjectField GObjectField08 = ObjectContext.ObjectFields.Where(d => d.ObjectTableId == ThisObject.Id && d.Tenant == 0 && d.FieldName == "RequestedPaymentMethodCode").FirstOrDefault();
            ObjectField GObjectField09 = ObjectContext.ObjectFields.Where(d => d.ObjectTableId == ThisObject.Id && d.Tenant == 0 && d.FieldName == "SalesmanUserId").FirstOrDefault();
            ObjectField GObjectField10 = ObjectContext.ObjectFields.Where(d => d.ObjectTableId == ThisObject.Id && d.Tenant == 0 && d.FieldName == "BankAccountLiteId").FirstOrDefault();
            ObjectField GObjectField11 = ObjectContext.ObjectFields.Where(d => d.ObjectTableId == ThisObject.Id && d.Tenant == 0 && d.FieldName == "Intercompany").FirstOrDefault();

            ScreenField GScreenField01 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 0, Column = 0, ObjectFieldId = GObjectField01.Id, ScreenId = generalTabScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField GScreenField02 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 1, Column = 0, ObjectFieldId = GObjectField02.Id, ScreenId = generalTabScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField GScreenField03 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 2, Column = 0, ObjectFieldId = GObjectField03.Id, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField GScreenField04 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 3, Column = 0, ObjectFieldId = GObjectField04.Id, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField GScreenField05 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 4, Column = 0, ObjectFieldId = GObjectField05.Id, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField GScreenField06 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 5, Column = 0, ObjectFieldId = GObjectField06.Id, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField GScreenField07 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 6, Column = 0, ObjectFieldId = GObjectField07.Id, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            //ScreenField GScreenField08 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 7, Column = 0, ObjectFieldId = GObjectField08.Id, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField GScreenField09 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 7, Column = 0, ObjectFieldId = GObjectField09.Id, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField GScreenField10 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 8, Column = 0, ObjectFieldId = GObjectField10.Id, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField GScreenField11 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 9, Column = 0, ObjectFieldId = GObjectField11.Id, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);

            #endregion

            ObjectContext.SaveChanges();
        }

        public void BuildARPaymentScreens(Dictionary<string, Screen> tenantScreens, Dictionary<string, ScreenField> tenantScreenFields)
        {

            ObjectTable ARPaymentObject = ObjectContext.ObjectTables.Where(d => d.Name == "ARPayment" && d.Tenant == 0).FirstOrDefault();

            ObjectField ARPaymentNo = ObjectContext.ObjectFields.Where(d => d.FieldName == "PaymentNo" && d.ObjectTableId == ARPaymentObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField ARPaymentLocalCurrency = ObjectContext.ObjectFields.Where(d => d.FieldName == "LocalCurrencyId" && d.ObjectTableId == ARPaymentObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField ARPaymentBranch = ObjectContext.ObjectFields.Where(d => d.FieldName == "BranchId" && d.ObjectTableId == ARPaymentObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField ARPaymentBillTo = ObjectContext.ObjectFields.Where(d => d.FieldName == "BillToId" && d.ObjectTableId == ARPaymentObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField ARPaymentARAccount = ObjectContext.ObjectFields.Where(d => d.FieldName == "ARAccountId" && d.ObjectTableId == ARPaymentObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField ARPaymentStatus = ObjectContext.ObjectFields.Where(d => d.FieldName == "StatusCode" && d.ObjectTableId == ARPaymentObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField ARPaymentPaymentCurrency = ObjectContext.ObjectFields.Where(d => d.FieldName == "PaymentCurrencyId" && d.ObjectTableId == ARPaymentObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField ARPaymentInternalNotes = ObjectContext.ObjectFields.Where(d => d.FieldName == "InternalNotes" && d.ObjectTableId == ARPaymentObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField BankAccountLiteId = ObjectContext.ObjectFields.Where(d => d.FieldName == "BankAccountLiteId" && d.ObjectTableId == ARPaymentObject.Id && d.Tenant == 0).FirstOrDefault();

            #region Header Screen
            Screen HeaderScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "ARPayment.HeaderScreen", Name = "Header Screen", ObjectTableId = ARPaymentObject.Id, NumberOfColumns = 4, NumberOfRows = 1, IsReadOnly = true }, ScreensRepository, tenantScreens);
            ObjectField ObjectField1 = ObjectContext.ObjectFields.Where(d => d.FieldName == "PaymentNo" && d.ObjectTableId == ARPaymentObject.Id).FirstOrDefault();
            ObjectField ObjectField2 = ObjectContext.ObjectFields.Where(d => d.FieldName == "StatusName" && d.ObjectTableId == ARPaymentObject.Id).FirstOrDefault();
            ObjectField ObjectField3 = ObjectContext.ObjectFields.Where(d => d.FieldName == "OpenAmount" && d.ObjectTableId == ARPaymentObject.Id).FirstOrDefault();
            ObjectField ObjectField4 = ObjectContext.ObjectFields.Where(d => d.FieldName == "TransferStatusName" && d.ObjectTableId == ARPaymentObject.Id).FirstOrDefault();
            //ObjectField ObjectField5 = ObjectContext.ObjectFields.Where(d => d.FieldName == "SATTransferStatusName" && d.ObjectTableId == ARPaymentObject.Id).FirstOrDefault();

            ScreenField ScreenField1 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 0, ObjectFieldId = ObjectField1.Id, ScreenId = HeaderScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField ScreenField2 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, Row = 0, ObjectFieldId = ObjectField3.Id, ScreenId = HeaderScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField ScreenField3 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 2, Row = 0, ObjectFieldId = ObjectField2.Id, ScreenId = HeaderScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField ScreenField4 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 3, Row = 0, ObjectFieldId = ObjectField4.Id, ScreenId = HeaderScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            //ScreenField ScreenField5 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 4, Row = 0, ObjectFieldId = ObjectField5.Id, ScreenId = HeaderScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);

            ARPaymentObject.HeaderScreenId = HeaderScreen.Id;
            #endregion

            Screen generalTabScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "ARPayment.GeneralTabScreen", Name = "General Tab Screen", ObjectTableId = ARPaymentObject.Id, NumberOfColumns = 2, NumberOfRows = 6 }, ScreensRepository, tenantScreens);
            ObjectField GObjectField01 = ObjectContext.ObjectFields.Where(d => d.ObjectTableId == ARPaymentObject.Id && d.Tenant == 0 && d.FieldName == "UpdatedByUserId").FirstOrDefault();
            ObjectField GObjectField02 = ObjectContext.ObjectFields.Where(d => d.ObjectTableId == ARPaymentObject.Id && d.Tenant == 0 && d.FieldName == "UpdateDate").FirstOrDefault();
            //ObjectField GObjectField03 = ObjectContext.ObjectFields.Where(d => d.ObjectTableId == ARPaymentObject.Id && d.Tenant == 0 && d.FieldName == "SATPaymentMethodCode").FirstOrDefault();

            ScreenField GScreenField01 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 0, Column = 0, ObjectFieldId = GObjectField01.Id, ScreenId = generalTabScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField GScreenField02 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 1, Column = 0, ObjectFieldId = GObjectField02.Id, ScreenId = generalTabScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            //ScreenField GScreenField03 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 2, Column = 0, ObjectFieldId = GObjectField03.Id, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField GScreenField04 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 2, Column = 0, ObjectFieldId = BankAccountLiteId.Id, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ObjectContext.SaveChanges();
        }

        public void BuildAPPaymentScreens(Dictionary<string, Screen> tenantScreens, Dictionary<string, ScreenField> tenantScreenFields)
        {

            ObjectTable APPaymentObject = ObjectContext.ObjectTables.Where(d => d.Name == "APPayment" && d.Tenant == 0).FirstOrDefault();

            ObjectField APPaymentNo = ObjectContext.ObjectFields.Where(d => d.FieldName == "PaymentNo" && d.ObjectTableId == APPaymentObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField APPaymentLocalCurrency = ObjectContext.ObjectFields.Where(d => d.FieldName == "LocalCurrencyId" && d.ObjectTableId == APPaymentObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField APPaymentMethod = ObjectContext.ObjectFields.Where(d => d.FieldName == "AccountingPaymentMethodCode" && d.ObjectTableId == APPaymentObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField APPaymentVendor = ObjectContext.ObjectFields.Where(d => d.FieldName == "VendorId" && d.ObjectTableId == APPaymentObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField APPaymentStatus = ObjectContext.ObjectFields.Where(d => d.FieldName == "StatusCode" && d.ObjectTableId == APPaymentObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField APPaymentPaymentCurrency = ObjectContext.ObjectFields.Where(d => d.FieldName == "PaymentCurrencyId" && d.ObjectTableId == APPaymentObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField APPaymentInternalNotes = ObjectContext.ObjectFields.Where(d => d.FieldName == "InternalNotes" && d.ObjectTableId == APPaymentObject.Id && d.Tenant == 0).FirstOrDefault();

            #region Header Screen
            Screen HeaderScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "APPayment.HeaderScreen", Name = "Header Screen", ObjectTableId = APPaymentObject.Id, NumberOfColumns = 4, NumberOfRows = 1, IsReadOnly = true }, ScreensRepository, tenantScreens);
            ObjectField ObjectField1 = ObjectContext.ObjectFields.Where(d => d.FieldName == "PaymentNo" && d.ObjectTableId == APPaymentObject.Id).FirstOrDefault();
            ObjectField ObjectField2 = ObjectContext.ObjectFields.Where(d => d.FieldName == "StatusName" && d.ObjectTableId == APPaymentObject.Id).FirstOrDefault();
            ObjectField ObjectField3 = ObjectContext.ObjectFields.Where(d => d.FieldName == "OpenAmount" && d.ObjectTableId == APPaymentObject.Id).FirstOrDefault();
            ObjectField ObjectField4 = ObjectContext.ObjectFields.Where(d => d.FieldName == "TransferStatusName" && d.ObjectTableId == APPaymentObject.Id).FirstOrDefault();
            //ObjectField ObjectField4 = ObjectContext.ObjectFields.Where(d => d.FieldName == "IsClosed" && d.ObjectTableId == APPaymentObject.Id).FirstOrDefault();
            ScreenField ScreenField1 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 0, ObjectFieldId = ObjectField1.Id, ScreenId = HeaderScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField ScreenField2 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, Row = 0, ObjectFieldId = ObjectField3.Id, ScreenId = HeaderScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField ScreenField3 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 2, Row = 0, ObjectFieldId = ObjectField2.Id, ScreenId = HeaderScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField ScreenField4 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 3, Row = 0, ObjectFieldId = ObjectField4.Id, ScreenId = HeaderScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            //ScreenField ScreenField4 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 3, Row = 0, ObjectFieldId = ObjectField4.Id, ScreenId = HeaderScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            APPaymentObject.HeaderScreenId = HeaderScreen.Id;
            #endregion

            Screen generalTabScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "APPayment.GeneralTabScreen", Name = "General Tab Screen", ObjectTableId = APPaymentObject.Id, NumberOfColumns = 2, NumberOfRows = 6 }, ScreensRepository, tenantScreens);
            ScreenField APPaymentNumberScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = APPaymentNo.Id, Row = 0, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField APPaymentLocalCurerncyScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = APPaymentLocalCurrency.Id, Row = 0, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField APPaymentBranchScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = APPaymentMethod.Id, Row = 1, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField APPaymentBillToScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = APPaymentVendor.Id, Row = 2, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField APPaymentStatusScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = APPaymentStatus.Id, Row = 3, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField APPaymentPaymentCurrencyScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = APPaymentPaymentCurrency.Id, Row = 1, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField APPaymentInternalNotesScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = APPaymentInternalNotes.Id, Row = 2, ScreenId = generalTabScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);

            ObjectContext.SaveChanges();
        }

        private void BuildQuoteScreens(Dictionary<string, Screen> tenantScreens, Dictionary<string, ScreenField> tenantScreenFields)
        {
            ObjectTable ThisObject = ObjectContext.ObjectTables.Where(d => d.Name == "Quote" && d.Tenant == 0).FirstOrDefault();

            #region New Quote Screen
            Screen WizardScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "NewQuote", Name = "New Quote", ObjectTableId = ThisObject.Id, NumberOfColumns = 2, NumberOfRows = 3, Tenant = 0 }, ScreensRepository, tenantScreens);
            ObjectField ObjectField1 = ObjectContext.ObjectFields.Where(d => d.FieldName == "ShipperReference2" && d.ObjectTableId == ThisObject.Id).FirstOrDefault();
            ObjectField ObjectField2 = ObjectContext.ObjectFields.Where(d => d.FieldName == "ConsigneeReference2" && d.ObjectTableId == ThisObject.Id).FirstOrDefault();
            ObjectField ObjectField3 = ObjectContext.ObjectFields.Where(d => d.FieldName == "AgentId" && d.ObjectTableId == ThisObject.Id).FirstOrDefault();
            ObjectField ObjectField4 = ObjectContext.ObjectFields.Where(d => d.FieldName == "Notes" && d.ObjectTableId == ThisObject.Id).FirstOrDefault();

            ScreenField ScreenField1 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = ObjectField1.Id, Row = 0, ScreenId = WizardScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField ScreenField2 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = ObjectField2.Id, Row = 1, ScreenId = WizardScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField ScreenField3 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = ObjectField3.Id, Row = 2, ScreenId = WizardScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField ScreenField4 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = ObjectField4.Id, Row = 0, ScreenId = WizardScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);

            #endregion

            #region General Tab Screen
            Screen generalTabScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "Quote.GeneralTabScreen", Name = "General Tab Screen", ObjectTableId = ThisObject.Id, NumberOfColumns = 2, NumberOfRows = 9 }, ScreensRepository, tenantScreens);

            ObjectField SalesManUserObject = ObjectContext.ObjectFields.Where(d => d.FieldName == "SalesmanUserId" && d.ObjectTableId == ThisObject.Id).FirstOrDefault();
            ObjectField OpenedByUserObject = ObjectContext.ObjectFields.Where(d => d.FieldName == "CreatedByUserId" && d.ObjectTableId == ThisObject.Id).FirstOrDefault();
            ObjectField DepartmentObject = ObjectContext.ObjectFields.Where(d => d.FieldName == "DepartmentId" && d.ObjectTableId == ThisObject.Id).FirstOrDefault();
            ObjectField BranchObject = ObjectContext.ObjectFields.Where(d => d.FieldName == "BranchId" && d.ObjectTableId == ThisObject.Id).FirstOrDefault();
            ObjectField ValueOfGoodsObject = ObjectContext.ObjectFields.Where(d => d.FieldName == "ValueOfGoods" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField ValueOfGoodsCurrencyObject = ObjectContext.ObjectFields.Where(d => d.FieldName == "ValueOfGoodsCurrencyId" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();

            ScreenField SalesManUserfield = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 0, Column = 0, ObjectFieldId = SalesManUserObject.Id, ScreenId = generalTabScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField OpenedByUserfield = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 1, Column = 0, ObjectFieldId = OpenedByUserObject.Id, ScreenId = generalTabScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField Departmentfield = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 2, Column = 0, ObjectFieldId = DepartmentObject.Id, ScreenId = generalTabScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField Branchfield = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 3, Column = 0, ObjectFieldId = BranchObject.Id, ScreenId = generalTabScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField ValueOfGoodsfield = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 4, Column = 0, ObjectFieldId = ValueOfGoodsObject.Id, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField ValueOfGoodsCurrencyfield = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 5, Column = 0, ObjectFieldId = ValueOfGoodsCurrencyObject.Id, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);

            #endregion

            #region Header screen
            Screen HeaderScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "Quote.HeaderScreen", Name = "Header Screen", ObjectTableId = ThisObject.Id, NumberOfColumns = 4, NumberOfRows = 2, IsReadOnly = true }, ScreensRepository, tenantScreens);

            ObjectField objectField_00 = ObjectContext.ObjectFields.Where(d => d.FieldName == "QuoteTypeName" && d.ObjectTableId == ThisObject.Id).FirstOrDefault();
            ObjectField objectField_01 = ObjectContext.ObjectFields.Where(d => d.FieldName == "IncotermCode" && d.ObjectTableId == ThisObject.Id).FirstOrDefault();
            ObjectField objectField_10 = ObjectContext.ObjectFields.Where(d => d.FieldName == "Subject" && d.ObjectTableId == ThisObject.Id).FirstOrDefault();
            ObjectField objectField_11 = ObjectContext.ObjectFields.Where(d => d.FieldName == "Routing" && d.ObjectTableId == ThisObject.Id).FirstOrDefault();
            ObjectField objectField_20 = ObjectContext.ObjectFields.Where(d => d.FieldName == "LastUsageDate" && d.ObjectTableId == ThisObject.Id).FirstOrDefault();
            ObjectField objectField_21 = ObjectContext.ObjectFields.Where(d => d.FieldName == "ExpirationDate" && d.ObjectTableId == ThisObject.Id).FirstOrDefault();
            ObjectField objectField_30 = ObjectContext.ObjectFields.Where(d => d.FieldName == "SalesmanName" && d.ObjectTableId == ThisObject.Id).FirstOrDefault();
            ObjectField objectField_31 = ObjectContext.ObjectFields.Where(d => d.FieldName == "StageName" && d.ObjectTableId == ThisObject.Id).FirstOrDefault();

            ScreenField screenField_00 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = objectField_00.Id, Row = 0, ScreenId = HeaderScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField screenField_01 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = objectField_01.Id, Row = 1, ScreenId = HeaderScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField screenField_10 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = objectField_10.Id, Row = 0, ScreenId = HeaderScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField screenField_11 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = objectField_11.Id, Row = 1, ScreenId = HeaderScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField screenField_20 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 2, ObjectFieldId = objectField_20.Id, Row = 0, ScreenId = HeaderScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField screenField_21 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 2, ObjectFieldId = objectField_21.Id, Row = 1, ScreenId = HeaderScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField screenField_30 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 3, ObjectFieldId = objectField_30.Id, Row = 0, ScreenId = HeaderScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField screenField_31 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 3, ObjectFieldId = objectField_31.Id, Row = 1, ScreenId = HeaderScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);

            ThisObject.HeaderScreenId = HeaderScreen.Id;
            #endregion

            ObjectContext.SaveChanges();
        }

        private void BuildPackageTypeScreens(Dictionary<string, Screen> tenantScreens, Dictionary<string, ScreenField> tenantScreenFields)
        {

            ObjectTable PackageTypeObject = ObjectContext.ObjectTables.Where(d => d.Name == "PackageType" && d.Tenant == 0).FirstOrDefault();

            ObjectField objectField01 = ObjectContext.ObjectFields.Where(d => d.FieldName == "Code" && d.ObjectTableId == PackageTypeObject.Id).FirstOrDefault();
            ObjectField objectField02 = ObjectContext.ObjectFields.Where(d => d.FieldName == "EnglishName" && d.ObjectTableId == PackageTypeObject.Id).FirstOrDefault();
            ObjectField objectField03 = ObjectContext.ObjectFields.Where(d => d.FieldName == "LocalName" && d.ObjectTableId == PackageTypeObject.Id).FirstOrDefault();
            ObjectField objectField04 = ObjectContext.ObjectFields.Where(d => d.FieldName == "TEU" && d.ObjectTableId == PackageTypeObject.Id).FirstOrDefault();
            ObjectField objectField05 = ObjectContext.ObjectFields.Where(d => d.FieldName == "ContainerSize" && d.ObjectTableId == PackageTypeObject.Id).FirstOrDefault();
            ObjectField objectField06 = ObjectContext.ObjectFields.Where(d => d.FieldName == "Volume" && d.ObjectTableId == PackageTypeObject.Id).FirstOrDefault();
            ObjectField objectField07 = ObjectContext.ObjectFields.Where(d => d.FieldName == "PrintAs" && d.ObjectTableId == PackageTypeObject.Id).FirstOrDefault();
            ObjectField objectField08 = ObjectContext.ObjectFields.Where(d => d.FieldName == "Notes" && d.ObjectTableId == PackageTypeObject.Id).FirstOrDefault();
            ObjectField objectField09 = ObjectContext.ObjectFields.Where(d => d.FieldName == "IsContainer" && d.ObjectTableId == PackageTypeObject.Id).FirstOrDefault();
            ObjectField objectField10 = ObjectContext.ObjectFields.Where(d => d.FieldName == "IsAir" && d.ObjectTableId == PackageTypeObject.Id).FirstOrDefault();
            ObjectField objectField11 = ObjectContext.ObjectFields.Where(d => d.FieldName == "IsOcean" && d.ObjectTableId == PackageTypeObject.Id).FirstOrDefault();
            ObjectField objectField12 = ObjectContext.ObjectFields.Where(d => d.FieldName == "IsInland" && d.ObjectTableId == PackageTypeObject.Id).FirstOrDefault();
            ObjectField objectField13 = ObjectContext.ObjectFields.Where(d => d.FieldName == "InActive" && d.ObjectTableId == PackageTypeObject.Id).FirstOrDefault();
            ObjectField objectField_IsRefrigerated = ObjectContext.ObjectFields.Where(d => d.FieldName == "IsRefrigerated" && d.ObjectTableId == PackageTypeObject.Id).FirstOrDefault();

            Screen generalTabScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "PackageType.GeneralTabScreen", Name = "General Tab Screen", ObjectTableId = PackageTypeObject.Id, NumberOfColumns = 2, NumberOfRows = 9 }, ScreensRepository, tenantScreens);

            ScreenField screenField01 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { ObjectFieldId = objectField01.Id, Row = 0, Column = 0, ScreenId = generalTabScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField screenField02 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { ObjectFieldId = objectField02.Id, Row = 1, Column = 0, ScreenId = generalTabScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField screenField03 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { ObjectFieldId = objectField03.Id, Row = 2, Column = 0, ScreenId = generalTabScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField screenField04 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { ObjectFieldId = objectField04.Id, Row = 3, Column = 0, ScreenId = generalTabScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField screenField05 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { ObjectFieldId = objectField05.Id, Row = 4, Column = 0, ScreenId = generalTabScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField screenField06 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { ObjectFieldId = objectField06.Id, Row = 5, Column = 0, ScreenId = generalTabScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField screenField07 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { ObjectFieldId = objectField07.Id, Row = 6, Column = 0, ScreenId = generalTabScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField screenField08 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { ObjectFieldId = objectField08.Id, Row = 7, Column = 0, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);

            ScreenField screenField_1_0 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { ObjectFieldId = objectField09.Id, Row = 0, Column = 1, ScreenId = generalTabScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField screenField_1_1 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { ObjectFieldId = objectField_IsRefrigerated.Id, Row = 1, Column = 1, ScreenId = generalTabScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField screenField10 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { ObjectFieldId = objectField10.Id, Row = 2, Column = 1, ScreenId = generalTabScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField screenField11 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { ObjectFieldId = objectField11.Id, Row = 3, Column = 1, ScreenId = generalTabScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField screenField12 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { ObjectFieldId = objectField12.Id, Row = 4, Column = 1, ScreenId = generalTabScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField screenField13 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { ObjectFieldId = objectField13.Id, Row = 5, Column = 1, ScreenId = generalTabScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);

            //==============Header screen===========================
            ObjectField AddedManually = ObjectContext.ObjectFields.Where(d => d.FieldName == "AddedManually" && d.ObjectTableId == PackageTypeObject.Id).FirstOrDefault();

            Screen HeaderScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "PackageType.HeaderScreen", Name = "Header Screen", ObjectTableId = PackageTypeObject.Id, NumberOfColumns = 1, NumberOfRows = 2, IsReadOnly = true }, ScreensRepository, tenantScreens);

            ScreenField Header_CodeScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = objectField01.Id, Row = 0, ScreenId = HeaderScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField Header_NameScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = objectField02.Id, Row = 1, ScreenId = HeaderScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField Header_AddedManuallyScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { ObjectFieldId = AddedManually.Id, Row = 6, Column = 0, ScreenId = HeaderScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            PackageTypeObject.HeaderScreenId = HeaderScreen.Id;

            ObjectContext.SaveChanges();
        }

        public void BuildAccountScreens(Dictionary<string, Screen> tenantScreens, Dictionary<string, ScreenField> tenantScreenFields)
        {

            ObjectTable AccountObject = ObjectContext.ObjectTables.Where(d => d.Name == "Account" && d.Tenant == 0).FirstOrDefault();

            ObjectField AccountCode = ObjectContext.ObjectFields.Where(d => d.FieldName == "Code" && d.ObjectTableId == AccountObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField AccountName = ObjectContext.ObjectFields.Where(d => d.FieldName == "Name" && d.ObjectTableId == AccountObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField AccountAccountTypeCode = ObjectContext.ObjectFields.Where(d => d.FieldName == "AccountTypeCode" && d.ObjectTableId == AccountObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField AccountExternalCard = ObjectContext.ObjectFields.Where(d => d.FieldName == "ExternalAccountingCard" && d.ObjectTableId == AccountObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField AccountInActive = ObjectContext.ObjectFields.Where(d => d.FieldName == "InActive" && d.ObjectTableId == AccountObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField AccountAddedManually = ObjectContext.ObjectFields.Where(d => d.FieldName == "AddedManually" && d.ObjectTableId == AccountObject.Id && d.Tenant == 0).FirstOrDefault();

            Screen generalTabScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "Account.GeneralTabScreen", Name = "General Tab Screen", ObjectTableId = AccountObject.Id, NumberOfColumns = 2, NumberOfRows = 6 }, ScreensRepository, tenantScreens);

            ScreenField AccountCodeScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = AccountCode.Id, Row = 0, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField AccountNameScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = AccountName.Id, Row = 1, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField AccountTypeCodeScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = AccountAccountTypeCode.Id, Row = 2, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField AccountExternalCardScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = AccountExternalCard.Id, Row = 3, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField AccountInActiveScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = AccountInActive.Id, Row = 0, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField AccountAddedManuallyScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = AccountAddedManually.Id, Row = 1, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);

            //==============Header screen===========================
            Screen HeaderScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "Account.HeaderScreen", Name = "Header Screen", ObjectTableId = AccountObject.Id, NumberOfColumns = 2, NumberOfRows = 2, IsReadOnly = true }, ScreensRepository, tenantScreens);

            ScreenField Header_CodeScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = AccountCode.Id, Row = 0, ScreenId = HeaderScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField Header_NameScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = AccountName.Id, Row = 1, ScreenId = HeaderScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField Header_AccountTypeCodeScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = AccountAccountTypeCode.Id, Row = 0, ScreenId = HeaderScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField Header_AddedManuallyScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = AccountAddedManually.Id, Row = 1, ScreenId = HeaderScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);

            AccountObject.HeaderScreenId = HeaderScreen.Id;

            ObjectContext.SaveChanges();
        }

        public void BuildAPInvoiceScreens(Dictionary<string, Screen> tenantScreens, Dictionary<string, ScreenField> tenantScreenFields)
        {

            ObjectTable APInvoiceObject = ObjectContext.ObjectTables.Where(d => d.Name == "APInvoice" && d.Tenant == 0).FirstOrDefault();

            ObjectField APInvoiceNumber = ObjectContext.ObjectFields.Where(d => d.FieldName == "InvoiceNumber" && d.ObjectTableId == APInvoiceObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField APInvoiceCurrency = ObjectContext.ObjectFields.Where(d => d.FieldName == "InvoiceCurrencyId" && d.ObjectTableId == APInvoiceObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField APInvoiceProfitCurrency = ObjectContext.ObjectFields.Where(d => d.FieldName == "ProfitCurrencyId" && d.ObjectTableId == APInvoiceObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField APInvoiceTypeCode = ObjectContext.ObjectFields.Where(d => d.FieldName == "APInvoiceTypeCode" && d.ObjectTableId == APInvoiceObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField APInvoicePaymentTerm = ObjectContext.ObjectFields.Where(d => d.FieldName == "PaymentTermId" && d.ObjectTableId == APInvoiceObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField APInvoiceStatus = ObjectContext.ObjectFields.Where(d => d.FieldName == "StatusCode" && d.ObjectTableId == APInvoiceObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField APInvoiceDueDate = ObjectContext.ObjectFields.Where(d => d.FieldName == "DueDate" && d.ObjectTableId == APInvoiceObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField APInvoiceInternalNotes = ObjectContext.ObjectFields.Where(d => d.FieldName == "InternalNotes" && d.ObjectTableId == APInvoiceObject.Id && d.Tenant == 0).FirstOrDefault();

            ObjectField GObjectField1 = ObjectContext.ObjectFields.Where(d => d.ObjectTableId == APInvoiceObject.Id && d.Tenant == 0 && d.FieldName == "UpdatedByUserId").FirstOrDefault();
            ObjectField GObjectField2 = ObjectContext.ObjectFields.Where(d => d.ObjectTableId == APInvoiceObject.Id && d.Tenant == 0 && d.FieldName == "UpdateDate").FirstOrDefault();
            ObjectField GObjectField3 = ObjectContext.ObjectFields.Where(d => d.ObjectTableId == APInvoiceObject.Id && d.Tenant == 0 && d.FieldName == "BranchId").FirstOrDefault();
            ObjectField GObjectField4 = ObjectContext.ObjectFields.Where(d => d.ObjectTableId == APInvoiceObject.Id && d.Tenant == 0 && d.FieldName == "MasterNumber").FirstOrDefault();
            ObjectField GObjectField5 = ObjectContext.ObjectFields.Where(d => d.ObjectTableId == APInvoiceObject.Id && d.Tenant == 0 && d.FieldName == "HouseNumber").FirstOrDefault();
            ObjectField GObjectField6 = ObjectContext.ObjectFields.Where(d => d.ObjectTableId == APInvoiceObject.Id && d.Tenant == 0 && d.FieldName == "AccountingDate").FirstOrDefault();

            #region Header Screen
            Screen HeaderScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "APInvoice.HeaderScreen", Name = "Header Screen", ObjectTableId = APInvoiceObject.Id, NumberOfColumns = 5, NumberOfRows = 1, IsReadOnly = true }, ScreensRepository, tenantScreens);
            ObjectField ObjectField1 = ObjectContext.ObjectFields.Where(d => d.FieldName == "ShipmentsNumbers" && d.ObjectTableId == APInvoiceObject.Id).FirstOrDefault();
            ObjectField ObjectField2 = ObjectContext.ObjectFields.Where(d => d.FieldName == "StatusName" && d.ObjectTableId == APInvoiceObject.Id).FirstOrDefault();
            ObjectField ObjectField3 = ObjectContext.ObjectFields.Where(d => d.FieldName == "AmountDue" && d.ObjectTableId == APInvoiceObject.Id).FirstOrDefault();
            ObjectField ObjectField4 = ObjectContext.ObjectFields.Where(d => d.FieldName == "DueDate" && d.ObjectTableId == APInvoiceObject.Id).FirstOrDefault();
            ObjectField ObjectField5 = ObjectContext.ObjectFields.Where(d => d.FieldName == "TransferStatusName" && d.ObjectTableId == APInvoiceObject.Id).FirstOrDefault();

            ScreenField ScreenField1 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 0, ObjectFieldId = ObjectField1.Id, ScreenId = HeaderScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField ScreenField2 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, Row = 0, ObjectFieldId = ObjectField4.Id, ScreenId = HeaderScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField ScreenField3 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 2, Row = 0, ObjectFieldId = ObjectField3.Id, ScreenId = HeaderScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField ScreenField4 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 3, Row = 0, ObjectFieldId = ObjectField2.Id, ScreenId = HeaderScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField ScreenField5 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 4, Row = 0, ObjectFieldId = ObjectField5.Id, ScreenId = HeaderScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);

            APInvoiceObject.HeaderScreenId = HeaderScreen.Id;
            #endregion

            Screen generalTabScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "APInvoice.GeneralTabScreen", Name = "General Tab Screen", ObjectTableId = APInvoiceObject.Id, NumberOfColumns = 1, NumberOfRows = 6 }, ScreensRepository, tenantScreens);
            ScreenField GScreenField1 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 0, Column = 0, ObjectFieldId = GObjectField1.Id, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField GScreenField2 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 1, Column = 0, ObjectFieldId = GObjectField2.Id, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField GScreenField3 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 2, Column = 0, ObjectFieldId = GObjectField3.Id, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField GScreenField4 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 3, Column = 0, ObjectFieldId = GObjectField4.Id, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField GScreenField5 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 4, Column = 0, ObjectFieldId = GObjectField5.Id, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField GScreenField6 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 5, Column = 0, ObjectFieldId = GObjectField6.Id, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);

            ObjectContext.SaveChanges();
        }

        public void BuildCommunicationLogScreens(Dictionary<string, Screen> tenantScreens, Dictionary<string, ScreenField> tenantScreenFields)
        {
            ObjectTable LogObject = ObjectContext.ObjectTables.Where(d => d.Name == "CommunicationLog" && d.Tenant == 0).FirstOrDefault();

            ObjectField IdField = ObjectContext.ObjectFields.Where(d => d.FieldName == "Id" && d.ObjectTableId == LogObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField FromField = ObjectContext.ObjectFields.Where(d => d.FieldName == "From" && d.ObjectTableId == LogObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField ToField = ObjectContext.ObjectFields.Where(d => d.FieldName == "To" && d.ObjectTableId == LogObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField CCField = ObjectContext.ObjectFields.Where(d => d.FieldName == "CC" && d.ObjectTableId == LogObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField BCCField = ObjectContext.ObjectFields.Where(d => d.FieldName == "BCC" && d.ObjectTableId == LogObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField SubjectField = ObjectContext.ObjectFields.Where(d => d.FieldName == "Subject" && d.ObjectTableId == LogObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField ObjectTableIdField = ObjectContext.ObjectFields.Where(d => d.FieldName == "ObjectTableId" && d.ObjectTableId == LogObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField CommunicationStatusTypeCodeField = ObjectContext.ObjectFields.Where(d => d.FieldName == "CommunicationStatusTypeCode" && d.ObjectTableId == LogObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField CommunicationLogTypeCodeField = ObjectContext.ObjectFields.Where(d => d.FieldName == "CommunicationLogTypeCode" && d.ObjectTableId == LogObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField RetriesField = ObjectContext.ObjectFields.Where(d => d.FieldName == "Retries" && d.ObjectTableId == LogObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField InOutField = ObjectContext.ObjectFields.Where(d => d.FieldName == "InOut" && d.ObjectTableId == LogObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField TenantNameField = ObjectContext.ObjectFields.Where(d => d.FieldName == "TenantName" && d.ObjectTableId == LogObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField AWBNumberField = ObjectContext.ObjectFields.Where(d => d.FieldName == "AWBNumber" && d.ObjectTableId == LogObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField LogSettingsField = ObjectContext.ObjectFields.Where(d => d.FieldName == "LogSettings" && d.ObjectTableId == LogObject.Id && d.Tenant == 0).FirstOrDefault();

            #region Header Screen
            Screen HeaderScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "CommunicationLog.HeaderScreen", Name = "Header Screen", ObjectTableId = LogObject.Id, NumberOfColumns = 4, NumberOfRows = 1, IsReadOnly = true }, ScreensRepository, tenantScreens);

            ScreenField ScreenField1 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 0, ObjectFieldId = FromField.Id, ScreenId = HeaderScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField ScreenField2 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, Row = 0, ObjectFieldId = ToField.Id, ScreenId = HeaderScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField ScreenField3 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 2, Row = 0, ObjectFieldId = CommunicationStatusTypeCodeField.Id, ScreenId = HeaderScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField ScreenField4 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 3, Row = 0, ObjectFieldId = CommunicationLogTypeCodeField.Id, ScreenId = HeaderScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);

            LogObject.HeaderScreenId = HeaderScreen.Id;
            #endregion

            #region General Screen
            Screen generalTabScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "CommunicationLog.GeneralTabScreen", Name = "General Tab Screen", ObjectTableId = LogObject.Id, NumberOfColumns = 2, NumberOfRows = 10 }, ScreensRepository, tenantScreens);

            ScreenField GScreenField00 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 0, Column = 0, ObjectFieldId = IdField.Id, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField GScreenField01 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 1, Column = 0, ObjectFieldId = FromField.Id, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField GScreenField02 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 2, Column = 0, ObjectFieldId = ToField.Id, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField GScreenField03 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 3, Column = 0, ObjectFieldId = CCField.Id, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField GScreenField04 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 4, Column = 0, ObjectFieldId = BCCField.Id, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField GScreenField05 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 5, Column = 0, ObjectFieldId = SubjectField.Id, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField GScreenField06 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 6, Column = 0, ObjectFieldId = RetriesField.Id, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField GScreenField07 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 7, Column = 0, ObjectFieldId = InOutField.Id, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);

            ScreenField GScreenField08 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 0, Column = 1, ObjectFieldId = ObjectTableIdField.Id, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField GScreenField09 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 1, Column = 1, ObjectFieldId = CommunicationStatusTypeCodeField.Id, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField GScreenField10 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 2, Column = 1, ObjectFieldId = CommunicationLogTypeCodeField.Id, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField GScreenField11 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 3, Column = 1, ObjectFieldId = TenantNameField.Id, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField GScreenField12 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 4, Column = 1, ObjectFieldId = AWBNumberField.Id, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField GScreenField13 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 5, Column = 1, ObjectFieldId = LogSettingsField.Id, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            #endregion

            ObjectContext.SaveChanges();
        }

        public void BuildAPILogsScreens(Dictionary<string, Screen> tenantScreens, Dictionary<string, ScreenField> tenantScreenFields)
        {
            ObjectTable LogObject = ObjectContext.ObjectTables.Where(d => d.Name == "APILogs" && d.Tenant == 0).FirstOrDefault();

            ObjectField IdField = ObjectContext.ObjectFields.Where(d => d.FieldName == "Id" && d.ObjectTableId == LogObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField Direction = ObjectContext.ObjectFields.Where(d => d.FieldName == "Direction" && d.ObjectTableId == LogObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField Status = ObjectContext.ObjectFields.Where(d => d.FieldName == "StatusName" && d.ObjectTableId == LogObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField NumberOfRetries = ObjectContext.ObjectFields.Where(d => d.FieldName == "NumberOfRetries" && d.ObjectTableId == LogObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField ExpirationDate = ObjectContext.ObjectFields.Where(d => d.FieldName == "ExpirationDate" && d.ObjectTableId == LogObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField Subject = ObjectContext.ObjectFields.Where(d => d.FieldName == "Subject" && d.ObjectTableId == LogObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField ObjectTableId = ObjectContext.ObjectFields.Where(d => d.FieldName == "ObjectTableId" && d.ObjectTableId == LogObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField PartnerName = ObjectContext.ObjectFields.Where(d => d.FieldName == "PartnerName" && d.ObjectTableId == LogObject.Id && d.Tenant == 0).FirstOrDefault(); 
            ObjectField LastExceptionMessage = ObjectContext.ObjectFields.Where(d => d.FieldName == "LastExceptionMessage" && d.ObjectTableId == LogObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField LastUpdateDate = ObjectContext.ObjectFields.Where(d => d.FieldName == "LastUpdateDate" && d.ObjectTableId == LogObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField ObjectTableName = ObjectContext.ObjectFields.Where(d => d.FieldName == "ObjectTableName" && d.ObjectTableId == LogObject.Id && d.Tenant == 0).FirstOrDefault();
            
            
            #region Header Screen
            Screen HeaderScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "APILogs.HeaderScreen", Name = "Header Screen", ObjectTableId = LogObject.Id, NumberOfColumns = 2, NumberOfRows = 2, IsReadOnly = true }, ScreensRepository, tenantScreens);

            ScreenField ScreenField1 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 0, ObjectFieldId = Subject.Id, ScreenId = HeaderScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField ScreenField2 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 1, ObjectFieldId = Status.Id, ScreenId = HeaderScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField ScreenField3 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, Row = 1, ObjectFieldId = Direction.Id, ScreenId = HeaderScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField ScreenField4 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, Row = 0, ObjectFieldId = ExpirationDate.Id, ScreenId = HeaderScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);

            LogObject.HeaderScreenId = HeaderScreen.Id;
            #endregion

            #region General Screen
            Screen generalTabScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "APILogs.GeneralTabScreen", Name = "General Tab Screen", ObjectTableId = LogObject.Id, NumberOfColumns = 2, NumberOfRows = 10 }, ScreensRepository, tenantScreens);

            ScreenField GScreenField00 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 0, Column = 0, ObjectFieldId = IdField.Id, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField GScreenField01 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 1, Column = 0, ObjectFieldId = Direction.Id, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField GScreenField02 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 2, Column = 0, ObjectFieldId = Status.Id, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField GScreenField03 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 3, Column = 0, ObjectFieldId = NumberOfRetries.Id, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField GScreenField04 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 4, Column = 0, ObjectFieldId = ExpirationDate.Id, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField GScreenField05 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 5, Column = 0, ObjectFieldId = Subject.Id, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField GScreenField06 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 6, Column = 0, ObjectFieldId = ObjectTableName.Id, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField GScreenField07 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 7, Column = 0, ObjectFieldId = PartnerName.Id, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);

            ScreenField GScreenField08 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 0, Column = 1, ObjectFieldId = LastExceptionMessage.Id, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            #endregion

            ObjectContext.SaveChanges();
        }

        public void BuildTenantScreens(Dictionary<string, Screen> tenantScreens, Dictionary<string, ScreenField> tenantScreenFields)
        {

            ObjectTable TenantObject = ObjectContext.ObjectTables.Where(d => d.Name == "Tenant" && d.Tenant == 0).FirstOrDefault();

            ObjectField IdField = ObjectContext.ObjectFields.Where(d => d.FieldName == "Id" && d.ObjectTableId == TenantObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField CompanyField = ObjectContext.ObjectFields.Where(d => d.FieldName == "Company" && d.ObjectTableId == TenantObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField LanguageField = ObjectContext.ObjectFields.Where(d => d.FieldName == "Language" && d.ObjectTableId == TenantObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField SignatureField = ObjectContext.ObjectFields.Where(d => d.FieldName == "Signature" && d.ObjectTableId == TenantObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField VatNumberField = ObjectContext.ObjectFields.Where(d => d.FieldName == "VatNumber" && d.ObjectTableId == TenantObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField VersionField = ObjectContext.ObjectFields.Where(d => d.FieldName == "Version" && d.ObjectTableId == TenantObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField TimeZoneOffsetField = ObjectContext.ObjectFields.Where(d => d.FieldName == "TimeZoneOffset" && d.ObjectTableId == TenantObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField IsActiveField = ObjectContext.ObjectFields.Where(d => d.FieldName == "IsActive" && d.ObjectTableId == TenantObject.Id && d.Tenant == 0).FirstOrDefault();

            ObjectField AccCurrencyIdField = ObjectContext.ObjectFields.Where(d => d.FieldName == "CurrencyId" && d.ObjectTableId == TenantObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField FreightCurrencyIdField = ObjectContext.ObjectFields.Where(d => d.FieldName == "FreightCurrencyId" && d.ObjectTableId == TenantObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField OtherChargesCurrencyIdField = ObjectContext.ObjectFields.Where(d => d.FieldName == "OtherChargesCurrencyId" && d.ObjectTableId == TenantObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField QuoteSaleCurrencyIdField = ObjectContext.ObjectFields.Where(d => d.FieldName == "QuoteSaleCurrencyId" && d.ObjectTableId == TenantObject.Id && d.Tenant == 0).FirstOrDefault();

            ObjectField AgentIdField = ObjectContext.ObjectFields.Where(d => d.FieldName == "AgentId" && d.ObjectTableId == TenantObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField PaymentTermIdField = ObjectContext.ObjectFields.Where(d => d.FieldName == "PaymentTermId" && d.ObjectTableId == TenantObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField PasswordPolicyField = ObjectContext.ObjectFields.Where(d => d.FieldName == "PasswordPolicyCode" && d.ObjectTableId == TenantObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField PackageCodeField = ObjectContext.ObjectFields.Where(d => d.FieldName == "PackageCode" && d.ObjectTableId == TenantObject.Id && d.Tenant == 0).FirstOrDefault();

            #region Header Screen
            Screen HeaderScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "Tenant.HeaderScreen", Name = "Header Screen", ObjectTableId = TenantObject.Id, NumberOfColumns = 4, NumberOfRows = 1, IsReadOnly = true }, ScreensRepository, tenantScreens);

            ScreenField ScreenField1 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 0, ObjectFieldId = IdField.Id, ScreenId = HeaderScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField ScreenField2 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, Row = 0, ObjectFieldId = CompanyField.Id, ScreenId = HeaderScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField ScreenField3 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 2, Row = 0, ObjectFieldId = VersionField.Id, ScreenId = HeaderScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField ScreenField4 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 3, Row = 0, ObjectFieldId = TimeZoneOffsetField.Id, ScreenId = HeaderScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);

            TenantObject.HeaderScreenId = HeaderScreen.Id;
            #endregion

            #region General Screen
            Screen generalTabScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "Tenant.GeneralTabScreen", Name = "General Tab Screen", ObjectTableId = TenantObject.Id, NumberOfColumns = 2, NumberOfRows = 10 }, ScreensRepository, tenantScreens);

            ScreenField GScreenField2 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 0, Column = 0, ObjectFieldId = CompanyField.Id, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField GScreenField4 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 1, Column = 0, ObjectFieldId = LanguageField.Id, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField GScreenField5 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 2, Column = 0, ObjectFieldId = SignatureField.Id, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField GScreenField6 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 3, Column = 0, ObjectFieldId = VatNumberField.Id, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField GScreenField7 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 4, Column = 0, ObjectFieldId = VersionField.Id, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField GScreenField8 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 5, Column = 0, ObjectFieldId = TimeZoneOffsetField.Id, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField GScreenField9 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 6, Column = 0, ObjectFieldId = IsActiveField.Id, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);

            ScreenField GScreenField10 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 0, Column = 1, ObjectFieldId = AccCurrencyIdField.Id, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField GScreenField11 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 1, Column = 1, ObjectFieldId = FreightCurrencyIdField.Id, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField GScreenField12 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 2, Column = 1, ObjectFieldId = OtherChargesCurrencyIdField.Id, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField GScreenField13 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 3, Column = 1, ObjectFieldId = QuoteSaleCurrencyIdField.Id, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField GScreenField14 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 4, Column = 1, ObjectFieldId = AgentIdField.Id, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField GScreenField15 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 5, Column = 1, ObjectFieldId = PaymentTermIdField.Id, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField GScreenField16 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 6, Column = 1, ObjectFieldId = PasswordPolicyField.Id, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField GScreenField17 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 7, Column = 1, ObjectFieldId = PackageCodeField.Id, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);

            #endregion

            ObjectContext.SaveChanges();
        }

        private void BuildTenantManagementScreens(Dictionary<string, Screen> tenantScreens, Dictionary<string, ScreenField> tenantScreenFields)
        {
            ObjectTable TenantMngmntObject = ObjectContext.ObjectTables.Where(d => d.Name == "TenantManagement" && d.Tenant == 0).FirstOrDefault();

            ObjectField IdField = ObjectContext.ObjectFields.Where(d => d.FieldName == "Id" && d.ObjectTableId == TenantMngmntObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField NameField = ObjectContext.ObjectFields.Where(d => d.FieldName == "Name" && d.ObjectTableId == TenantMngmntObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField TimeZoneField = ObjectContext.ObjectFields.Where(d => d.FieldName == "TimeZone" && d.ObjectTableId == TenantMngmntObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField GlobalDBField = ObjectContext.ObjectFields.Where(d => d.FieldName == "GlobalDBId" && d.ObjectTableId == TenantMngmntObject.Id && d.Tenant == 0).FirstOrDefault();

            #region Header Screen
            Screen HeaderScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "TenantManagement.HeaderScreen", Name = "Header Screen", ObjectTableId = TenantMngmntObject.Id, NumberOfColumns = 4, NumberOfRows = 1, IsReadOnly = true }, ScreensRepository, tenantScreens);

            ScreenField ScreenField1 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 0, ObjectFieldId = IdField.Id, ScreenId = HeaderScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField ScreenField2 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, Row = 0, ObjectFieldId = NameField.Id, ScreenId = HeaderScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField ScreenField3 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 2, Row = 0, ObjectFieldId = TimeZoneField.Id, ScreenId = HeaderScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField ScreenField4 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 3, Row = 0, ObjectFieldId = GlobalDBField.Id, ScreenId = HeaderScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);

            TenantMngmntObject.HeaderScreenId = HeaderScreen.Id;
            #endregion

            ObjectContext.SaveChanges();
        }

        private void BuildAnalyzeQueueScreens(Dictionary<string, Screen> tenantScreens, Dictionary<string, ScreenField> tenantScreenFields)
        {
            ObjectTable AnalyzeQueueObject = ObjectContext.ObjectTables.Where(d => d.Name == "AnalyzeQueue" && d.Tenant == 0).FirstOrDefault();

            ObjectField IdField = ObjectContext.ObjectFields.Where(d => d.FieldName == "Id" && d.ObjectTableId == AnalyzeQueueObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField FromField = ObjectContext.ObjectFields.Where(d => d.FieldName == "From" && d.ObjectTableId == AnalyzeQueueObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField CreateDateField = ObjectContext.ObjectFields.Where(d => d.FieldName == "CreateDate" && d.ObjectTableId == AnalyzeQueueObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField FileSizeField = ObjectContext.ObjectFields.Where(d => d.FieldName == "FileSize" && d.ObjectTableId == AnalyzeQueueObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField StatusField = ObjectContext.ObjectFields.Where(d => d.FieldName == "Status" && d.ObjectTableId == AnalyzeQueueObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField CommunicationLogField = ObjectContext.ObjectFields.Where(d => d.FieldName == "CommunicationLogId" && d.ObjectTableId == AnalyzeQueueObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField SubjectField = ObjectContext.ObjectFields.Where(d => d.FieldName == "Subject" && d.ObjectTableId == AnalyzeQueueObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField RetriesField = ObjectContext.ObjectFields.Where(d => d.FieldName == "Retries" && d.ObjectTableId == AnalyzeQueueObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField TenantNameField = ObjectContext.ObjectFields.Where(d => d.FieldName == "TenantName" && d.ObjectTableId == AnalyzeQueueObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField AWBNumberField = ObjectContext.ObjectFields.Where(d => d.FieldName == "AWBNumber" && d.ObjectTableId == AnalyzeQueueObject.Id && d.Tenant == 0).FirstOrDefault();

            ObjectField ConnectedToTenantField = ObjectContext.ObjectFields.Where(d => d.FieldName == "ConnectedToTenant" && d.ObjectTableId == AnalyzeQueueObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField ConnectedToEntityField = ObjectContext.ObjectFields.Where(d => d.FieldName == "ConnectedToEntity" && d.ObjectTableId == AnalyzeQueueObject.Id && d.Tenant == 0).FirstOrDefault();

            #region Header Screen
            Screen HeaderScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "AnalyzeQueue.HeaderScreen", Name = "Header Screen", ObjectTableId = AnalyzeQueueObject.Id, NumberOfColumns = 4, NumberOfRows = 1, IsReadOnly = true }, ScreensRepository, tenantScreens);

            ScreenField ScreenField1 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 0, ObjectFieldId = IdField.Id, ScreenId = HeaderScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField ScreenField2 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, Row = 0, ObjectFieldId = FromField.Id, ScreenId = HeaderScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField ScreenField3 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 2, Row = 0, ObjectFieldId = CreateDateField.Id, ScreenId = HeaderScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField ScreenField4 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 3, Row = 0, ObjectFieldId = SubjectField.Id, ScreenId = HeaderScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);

            AnalyzeQueueObject.HeaderScreenId = HeaderScreen.Id;
            #endregion

            #region General Screen
            Screen generalTabScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "AnalyzeQueue.GeneralTabScreen", Name = "General Tab Screen", ObjectTableId = AnalyzeQueueObject.Id, NumberOfColumns = 2, NumberOfRows = 8 }, ScreensRepository, tenantScreens);

            ScreenField GScreenField0 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 0, Column = 0, ObjectFieldId = FromField.Id, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField GScreenField1 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 1, Column = 0, ObjectFieldId = CreateDateField.Id, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField GScreenField2 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 2, Column = 0, ObjectFieldId = FileSizeField.Id, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField GScreenField3 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 3, Column = 0, ObjectFieldId = StatusField.Id, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField GScreenField4 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 4, Column = 0, ObjectFieldId = SubjectField.Id, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField GScreenField5 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 5, Column = 0, ObjectFieldId = RetriesField.Id, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField GScreenField6 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 6, Column = 0, ObjectFieldId = TenantNameField.Id, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField GScreenField7 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 7, Column = 0, ObjectFieldId = AWBNumberField.Id, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);

            ScreenField GScreenField8 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 0, Column = 1, ObjectFieldId = ConnectedToTenantField.Id, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField GScreenField9 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 1, Column = 1, ObjectFieldId = ConnectedToEntityField.Id, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            #endregion

            ObjectContext.SaveChanges();
        }

        public void BuildCreditCardTypeScreens(Dictionary<string, Screen> tenantScreens, Dictionary<string, ScreenField> tenantScreenFields)
        {
            ObjectTable creditCardTypeObject = ObjectContext.ObjectTables.Where(d => d.Name == "CreditCardType" && d.Tenant == 0).FirstOrDefault();

            ObjectField CodeField = ObjectContext.ObjectFields.Where(d => d.FieldName == "Code" && d.ObjectTableId == creditCardTypeObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField NameField = ObjectContext.ObjectFields.Where(d => d.FieldName == "Name" && d.ObjectTableId == creditCardTypeObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField inactiveField = ObjectContext.ObjectFields.Where(d => d.FieldName == "InActive" && d.ObjectTableId == creditCardTypeObject.Id && d.Tenant == 0).FirstOrDefault();

            #region Header Screen
            Screen HeaderScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "CreditCardType.HeaderScreen", Name = "Header Screen", ObjectTableId = creditCardTypeObject.Id, NumberOfColumns = 2, NumberOfRows = 1, IsReadOnly = true }, ScreensRepository, tenantScreens);

            ScreenField ScreenField1 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 0, ObjectFieldId = CodeField.Id, ScreenId = HeaderScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField ScreenField2 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, Row = 0, ObjectFieldId = NameField.Id, ScreenId = HeaderScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);

            creditCardTypeObject.HeaderScreenId = HeaderScreen.Id;
            #endregion

            #region General Screen
            Screen generalTabScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "CreditCardType.GeneralTabScreen", Name = "General Tab Screen", ObjectTableId = creditCardTypeObject.Id, NumberOfColumns = 1, NumberOfRows = 4 }, ScreensRepository, tenantScreens);

            ScreenField GScreenField7 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 0, Column = 0, ObjectFieldId = CodeField.Id, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField GScreenField8 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 1, Column = 0, ObjectFieldId = NameField.Id, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField GScreenField9 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 2, Column = 0, ObjectFieldId = inactiveField.Id, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);

            #endregion

            ObjectContext.SaveChanges();
        }

        public void BuildMoveTypeScreens(Dictionary<string, Screen> tenantScreens, Dictionary<string, ScreenField> tenantScreenFields)
        {
            ObjectTable moveTypeObject = ObjectContext.ObjectTables.Where(d => d.Name == "MoveType" && d.Tenant == 0).FirstOrDefault();

            ObjectField CodeField = ObjectContext.ObjectFields.Where(d => d.FieldName == "Code" && d.ObjectTableId == moveTypeObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField EnglishNameField = ObjectContext.ObjectFields.Where(d => d.FieldName == "MoveTypeEnglishName" && d.ObjectTableId == moveTypeObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField TransportModeField = ObjectContext.ObjectFields.Where(d => d.FieldName == "TransportModeId" && d.ObjectTableId == moveTypeObject.Id && d.Tenant == 0).FirstOrDefault();

            #region Header Screen
            Screen HeaderScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "MoveType.HeaderScreen", Name = "Header Screen", ObjectTableId = moveTypeObject.Id, NumberOfColumns = 3, NumberOfRows = 1, IsReadOnly = true }, ScreensRepository, tenantScreens);

            ScreenField ScreenField1 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 0, ObjectFieldId = CodeField.Id, ScreenId = HeaderScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField ScreenField2 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, Row = 0, ObjectFieldId = EnglishNameField.Id, ScreenId = HeaderScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField ScreenField3 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 2, Row = 0, ObjectFieldId = TransportModeField.Id, ScreenId = HeaderScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);

            moveTypeObject.HeaderScreenId = HeaderScreen.Id;
            #endregion

            ObjectContext.SaveChanges();
        }

        public void BuildLogitudeLeadScreens(Dictionary<string, Screen> tenantScreens, Dictionary<string, ScreenField> tenantScreenFields)
        {
            ObjectTable LogitudeLeadObject = ObjectContext.ObjectTables.Where(d => d.Name == "LogitudeLead" && d.Tenant == 0).FirstOrDefault();

            ObjectField PhoneNumber = ObjectContext.ObjectFields.Where(d => d.FieldName == "PhoneNumber" && d.ObjectTableId == LogitudeLeadObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField CompanyName = ObjectContext.ObjectFields.Where(d => d.FieldName == "CompanyName" && d.ObjectTableId == LogitudeLeadObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField ContactName = ObjectContext.ObjectFields.Where(d => d.FieldName == "ContactName" && d.ObjectTableId == LogitudeLeadObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField NumberOfBranches = ObjectContext.ObjectFields.Where(d => d.FieldName == "NumberOfBranches" && d.ObjectTableId == LogitudeLeadObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField Email = ObjectContext.ObjectFields.Where(d => d.FieldName == "Email" && d.ObjectTableId == LogitudeLeadObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField NumberOfUsers = ObjectContext.ObjectFields.Where(d => d.FieldName == "NumberOfUsers" && d.ObjectTableId == LogitudeLeadObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField Comments = ObjectContext.ObjectFields.Where(d => d.FieldName == "Comments" && d.ObjectTableId == LogitudeLeadObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField IsEmailVerified = ObjectContext.ObjectFields.Where(d => d.FieldName == "IsEmailVerified" && d.ObjectTableId == LogitudeLeadObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField IsSentToCustomer = ObjectContext.ObjectFields.Where(d => d.FieldName == "IsSentToCustomer" && d.ObjectTableId == LogitudeLeadObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField RequestType = ObjectContext.ObjectFields.Where(d => d.FieldName == "RequestType" && d.ObjectTableId == LogitudeLeadObject.Id && d.Tenant == 0).FirstOrDefault();

            #region Header Screen

            Screen HeaderScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "LogitudeLead.HeaderScreen", Name = "Header Screen", ObjectTableId = LogitudeLeadObject.Id, NumberOfColumns = 5, NumberOfRows = 2, IsReadOnly = true }, ScreensRepository, tenantScreens);

            ScreenField ScreenField1 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 0, ObjectFieldId = PhoneNumber.Id, ScreenId = HeaderScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField ScreenField2 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, Row = 0, ObjectFieldId = CompanyName.Id, ScreenId = HeaderScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField ScreenField3 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 2, Row = 0, ObjectFieldId = ContactName.Id, ScreenId = HeaderScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);

            ScreenField ScreenField4 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 3, Row = 0, ObjectFieldId = NumberOfBranches.Id, ScreenId = HeaderScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField ScreenField5 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 4, Row = 0, ObjectFieldId = Email.Id, ScreenId = HeaderScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField ScreenField6 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 1, ObjectFieldId = NumberOfUsers.Id, ScreenId = HeaderScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);

            ScreenField ScreenField7 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, Row = 1, ObjectFieldId = Comments.Id, ScreenId = HeaderScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField ScreenField8 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 2, Row = 1, ObjectFieldId = IsEmailVerified.Id, ScreenId = HeaderScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField ScreenField9 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 3, Row = 1, ObjectFieldId = IsSentToCustomer.Id, ScreenId = HeaderScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);

            ScreenField ScreenField10 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 4, Row = 1, ObjectFieldId = RequestType.Id, ScreenId = HeaderScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            LogitudeLeadObject.HeaderScreenId = HeaderScreen.Id;
            #endregion


            #region General Screen

            Screen generalTabScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "LogitudeLead.GeneralTabScreen", Name = "General Tab Screen", IsReadOnly = false, ObjectTableId = LogitudeLeadObject.Id, NumberOfColumns = 1, NumberOfRows = 10 }, ScreensRepository, tenantScreens);
            ScreenField GScreenField1 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 0, Column = 0, ObjectFieldId = PhoneNumber.Id, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField GScreenField2 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 1, Column = 0, ObjectFieldId = CompanyName.Id, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField GScreenField3 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 2, Column = 0, ObjectFieldId = ContactName.Id, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField GScreenField4 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 3, Column = 0, ObjectFieldId = NumberOfBranches.Id, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);



            ScreenField GScreenField5 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 4, Column = 0, ObjectFieldId = Email.Id, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField GScreenField6 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 5, Column = 0, ObjectFieldId = NumberOfUsers.Id, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField GScreenField7 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 6, Column = 0, ObjectFieldId = Comments.Id, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField GScreenField8 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 7, Column = 0, ObjectFieldId = IsEmailVerified.Id, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);


            ScreenField GScreenField9 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 8, Column = 0, ObjectFieldId = IsSentToCustomer.Id, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField GScreenField10 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 9, Column = 0, ObjectFieldId = RequestType.Id, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);

            #endregion


            ObjectContext.SaveChanges();
        }

        private void BuildErrorLogScreens(Dictionary<string, Screen> tenantScreens, Dictionary<string, ScreenField> tenantScreenFields)
        {
            ObjectTable ErrorLogObject = ObjectContext.ObjectTables.Where(d => d.Name == "ErrorLog" && d.Tenant == 0).FirstOrDefault();

            ObjectField UserNameField = ObjectContext.ObjectFields.Where(d => d.FieldName == "UserName" && d.ObjectTableId == ErrorLogObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField LogDateField = ObjectContext.ObjectFields.Where(d => d.FieldName == "LogDate" && d.ObjectTableId == ErrorLogObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField TierField = ObjectContext.ObjectFields.Where(d => d.FieldName == "Tier" && d.ObjectTableId == ErrorLogObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField ExceptionField = ObjectContext.ObjectFields.Where(d => d.FieldName == "Exception" && d.ObjectTableId == ErrorLogObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField StackTraceField = ObjectContext.ObjectFields.Where(d => d.FieldName == "StackTrace" && d.ObjectTableId == ErrorLogObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField ClientDateField = ObjectContext.ObjectFields.Where(d => d.FieldName == "ClientDate" && d.ObjectTableId == ErrorLogObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField IPField = ObjectContext.ObjectFields.Where(d => d.FieldName == "IP" && d.ObjectTableId == ErrorLogObject.Id && d.Tenant == 0).FirstOrDefault();

            #region Header Screen
            Screen HeaderScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "ErrorLog.HeaderScreen", Name = "Header Screen", ObjectTableId = ErrorLogObject.Id, NumberOfColumns = 4, NumberOfRows = 1, IsReadOnly = true }, ScreensRepository, tenantScreens);

            ScreenField ScreenField1 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 0, ObjectFieldId = UserNameField.Id, ScreenId = HeaderScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField ScreenField2 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, Row = 0, ObjectFieldId = LogDateField.Id, ScreenId = HeaderScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField ScreenField3 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 2, Row = 0, ObjectFieldId = TierField.Id, ScreenId = HeaderScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField ScreenField4 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 3, Row = 0, ObjectFieldId = ClientDateField.Id, ScreenId = HeaderScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);

            ErrorLogObject.HeaderScreenId = HeaderScreen.Id;
            #endregion

            #region General Screen
            Screen generalTabScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "ErrorLog.GeneralTabScreen", Name = "General Tab Screen", ObjectTableId = ErrorLogObject.Id, NumberOfColumns = 1, NumberOfRows = 3 }, ScreensRepository, tenantScreens);

            ScreenField GScreenField1 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 0, Column = 0, ObjectFieldId = ClientDateField.Id, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);

            ScreenField GScreenField2 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 1, Column = 0, ObjectFieldId = IPField.Id, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);

            #endregion

            ObjectContext.SaveChanges();
        }

        private void BuildReportScreens(Dictionary<string, Screen> tenantScreens, Dictionary<string, ScreenField> tenantScreenFields)
        {
            ObjectTable ReportObject = ObjectContext.ObjectTables.Where(d => d.Name == "Report" && d.Tenant == 0).FirstOrDefault();

            ObjectField CodeField = ObjectContext.ObjectFields.Where(d => d.FieldName == "Code" && d.ObjectTableId == ReportObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField NameField = ObjectContext.ObjectFields.Where(d => d.FieldName == "Name" && d.ObjectTableId == ReportObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField DescriptionField = ObjectContext.ObjectFields.Where(d => d.FieldName == "Description" && d.ObjectTableId == ReportObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField FilterControlNameField = ObjectContext.ObjectFields.Where(d => d.FieldName == "FilterControlName" && d.ObjectTableId == ReportObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField InActiveField = ObjectContext.ObjectFields.Where(d => d.FieldName == "InActive" && d.ObjectTableId == ReportObject.Id && d.Tenant == 0).FirstOrDefault();

            #region Header Screen
            Screen HeaderScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "Report.HeaderScreen", Name = "Header Screen", ObjectTableId = ReportObject.Id, NumberOfColumns = 2, NumberOfRows = 1, IsReadOnly = true }, ScreensRepository, tenantScreens);

            ScreenField ScreenField1 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 0, ObjectFieldId = NameField.Id, ScreenId = HeaderScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField ScreenField2 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, Row = 0, ObjectFieldId = FilterControlNameField.Id, ScreenId = HeaderScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);

            ReportObject.HeaderScreenId = HeaderScreen.Id;
            #endregion

            #region General Screen
            Screen generalTabScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "Report.GeneralTabScreen", Name = "General Tab Screen", ObjectTableId = ReportObject.Id, NumberOfColumns = 1, NumberOfRows = 5, }, ScreensRepository, tenantScreens);

            ScreenField GScreenField1 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 0, Column = 0, ObjectFieldId = CodeField.Id, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField GScreenField2 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 1, Column = 0, ObjectFieldId = NameField.Id, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField GScreenField3 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 2, Column = 0, ObjectFieldId = DescriptionField.Id, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField GScreenField4 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 3, Column = 0, ObjectFieldId = FilterControlNameField.Id, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField GScreenField5 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 4, Column = 0, ObjectFieldId = InActiveField.Id, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);

            #endregion

            ObjectContext.SaveChanges();
        }

        private void BuildCommodityScreens(Dictionary<string, Screen> tenantScreens, Dictionary<string, ScreenField> tenantScreenFields)
        {
            ObjectTable entityObject = ObjectContext.ObjectTables.Where(d => d.Name == "Commodity" && d.Tenant == 0).FirstOrDefault();

            ObjectField CodeField = ObjectContext.ObjectFields.Where(d => d.FieldName == "Code" && d.ObjectTableId == entityObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField NameField = ObjectContext.ObjectFields.Where(d => d.FieldName == "Name" && d.ObjectTableId == entityObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField InActiveField = ObjectContext.ObjectFields.Where(d => d.FieldName == "InActive" && d.ObjectTableId == entityObject.Id && d.Tenant == 0).FirstOrDefault();

            #region Header Screen
            Screen HeaderScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "Commodity.HeaderScreen", Name = "Header Screen", ObjectTableId = entityObject.Id, NumberOfColumns = 1, NumberOfRows = 1, IsReadOnly = true }, ScreensRepository, tenantScreens);

            ScreenField ScreenField1 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 0, ObjectFieldId = CodeField.Id, ScreenId = HeaderScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);

            entityObject.HeaderScreenId = HeaderScreen.Id;
            #endregion

            #region General Screen
            Screen generalTabScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "Commodity.GeneralTabScreen", Name = "General Tab Screen", ObjectTableId = entityObject.Id, NumberOfColumns = 1, NumberOfRows = 3, }, ScreensRepository, tenantScreens);

            ScreenField GScreenField1 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 0, Column = 0, ObjectFieldId = CodeField.Id, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField GScreenField2 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 1, Column = 0, ObjectFieldId = NameField.Id, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField GScreenField3 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 2, Column = 0, ObjectFieldId = InActiveField.Id, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            #endregion

            ObjectContext.SaveChanges();
        }

        public void BuildLeadSourceScreens(Dictionary<string, Screen> tenantScreens, Dictionary<string, ScreenField> tenantScreenFields)
        {
            ObjectTable leadSourceObject = ObjectContext.ObjectTables.Where(d => d.Name == "LeadSource" && d.Tenant == 0).FirstOrDefault();


            ObjectField leadSourceName = ObjectContext.ObjectFields.Where(d => d.FieldName == "Name" && d.ObjectTable.Id == leadSourceObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField leadSourceInActive = ObjectContext.ObjectFields.Where(d => d.FieldName == "InActive" && d.ObjectTable.Id == leadSourceObject.Id && d.Tenant == 0).FirstOrDefault();

            //Header Screen
            Screen HeaderScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "LeadSource.HeaderScreen", Name = "Header Screen", ObjectTableId = leadSourceObject.Id, NumberOfColumns = 1, NumberOfRows = 1, IsReadOnly = true }, ScreensRepository, tenantScreens);
            ScreenField Header_NameScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = leadSourceName.Id, Row = 0, ScreenId = HeaderScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            leadSourceObject.HeaderScreenId = HeaderScreen.Id;

            //General Screen
            Screen generalTabScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "LeadSource.GeneralTabScreen", Name = "General Tab Screen", ObjectTableId = leadSourceObject.Id, NumberOfColumns = 2, NumberOfRows = 2 }, ScreensRepository, tenantScreens);
            ScreenField leadSourceNameScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = leadSourceName.Id, Row = 0, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField leadSourceInActiveScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = leadSourceInActive.Id, Row = 1, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);

            ObjectContext.SaveChanges();
        }

        public void BuildIndustryScreens(Dictionary<string, Screen> tenantScreens, Dictionary<string, ScreenField> tenantScreenFields)
        {
            ObjectTable IndustryObject = ObjectContext.ObjectTables.Where(d => d.Name == "Industry" && d.Tenant == 0).FirstOrDefault();

            ObjectField IndustryCode = ObjectContext.ObjectFields.Where(d => d.FieldName == "Code" && d.ObjectTable.Id == IndustryObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField IndustryName = ObjectContext.ObjectFields.Where(d => d.FieldName == "Name" && d.ObjectTable.Id == IndustryObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField IndustryInActive = ObjectContext.ObjectFields.Where(d => d.FieldName == "InActive" && d.ObjectTable.Id == IndustryObject.Id && d.Tenant == 0).FirstOrDefault();

            //Header Screen
            Screen HeaderScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "Industry.HeaderScreen", Name = "Header Screen", ObjectTableId = IndustryObject.Id, NumberOfColumns = 1, NumberOfRows = 2, IsReadOnly = true }, ScreensRepository, tenantScreens);

            ScreenField Header_CodeScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = IndustryCode.Id, Row = 0, ScreenId = HeaderScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField Header_NameScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = IndustryName.Id, Row = 1, ScreenId = HeaderScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);

            IndustryObject.HeaderScreenId = HeaderScreen.Id;

            //General Screen
            Screen generalTabScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "Industry.GeneralTabScreen", Name = "General Tab Screen", ObjectTableId = IndustryObject.Id, NumberOfColumns = 2, NumberOfRows = 4 }, ScreensRepository, tenantScreens);

            ScreenField IndustryCodeScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = IndustryCode.Id, Row = 0, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField IndustryNameScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = IndustryName.Id, Row = 1, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField IndustryInActiveScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = IndustryInActive.Id, Row = 2, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);

            ObjectContext.SaveChanges();
        }

        public void BuildCompetitorScreens(Dictionary<string, Screen> tenantScreens, Dictionary<string, ScreenField> tenantScreenFields)
        {
            ObjectTable competitorObject = ObjectContext.ObjectTables.Where(d => d.Name == "Competitor" && d.Tenant == 0).FirstOrDefault();

            ObjectField competitorName = ObjectContext.ObjectFields.Where(d => d.FieldName == "Name" && d.ObjectTable.Id == competitorObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField competitorWebsite = ObjectContext.ObjectFields.Where(d => d.FieldName == "Website" && d.ObjectTable.Id == competitorObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField competitorInActive = ObjectContext.ObjectFields.Where(d => d.FieldName == "InActive" && d.ObjectTable.Id == competitorObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField competitorAddress1 = ObjectContext.ObjectFields.Where(d => d.FieldName == "Address1" && d.ObjectTable.Id == competitorObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField competitorAddress2 = ObjectContext.ObjectFields.Where(d => d.FieldName == "Address2" && d.ObjectTable.Id == competitorObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField competitorZipCode = ObjectContext.ObjectFields.Where(d => d.FieldName == "ZipCode" && d.ObjectTable.Id == competitorObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField competitorCity = ObjectContext.ObjectFields.Where(d => d.FieldName == "City" && d.ObjectTable.Id == competitorObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField competitorCountryId = ObjectContext.ObjectFields.Where(d => d.FieldName == "CountryId" && d.ObjectTable.Id == competitorObject.Id && d.Tenant == 0).FirstOrDefault();

            ObjectField competitorOpportunity = ObjectContext.ObjectFields.Where(d => d.FieldName == "Opportunity" && d.ObjectTable.Id == competitorObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField competitorStrengths = ObjectContext.ObjectFields.Where(d => d.FieldName == "Strengths" && d.ObjectTable.Id == competitorObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField competitorWeaknesses = ObjectContext.ObjectFields.Where(d => d.FieldName == "Weaknesses" && d.ObjectTable.Id == competitorObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField competitorThreat = ObjectContext.ObjectFields.Where(d => d.FieldName == "Threat" && d.ObjectTable.Id == competitorObject.Id && d.Tenant == 0).FirstOrDefault();

            //Header Screen
            Screen HeaderScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "Competitor.HeaderScreen", Name = "Header Screen", ObjectTableId = competitorObject.Id, NumberOfColumns = 1, NumberOfRows = 2, IsReadOnly = true }, ScreensRepository, tenantScreens);

            ScreenField Header_NameScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = competitorName.Id, Row = 0, ScreenId = HeaderScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField Header_WebsiteScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = competitorWebsite.Id, Row = 1, ScreenId = HeaderScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);

            competitorObject.HeaderScreenId = HeaderScreen.Id;

            //General Screen
            Screen generalTabScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "Competitor.GeneralTabScreen", Name = "General Tab Screen", ObjectTableId = competitorObject.Id, NumberOfColumns = 2, NumberOfRows = 7 }, ScreensRepository, tenantScreens);

            ScreenField competitorNameScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = competitorName.Id, Row = 0, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField competitorWebsiteScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = competitorWebsite.Id, Row = 1, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField competitorAddress1ScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = competitorAddress1.Id, Row = 2, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField competitorAddress2ScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = competitorAddress2.Id, Row = 3, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField competitorZipCodeScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = competitorZipCode.Id, Row = 0, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField competitorCityScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = competitorCity.Id, Row = 1, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField competitorCountryScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = competitorCountryId.Id, Row = 2, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            
            ScreenField competitorStrengthsScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = competitorStrengths.Id, Row = 4, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField competitorOpportunityScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = competitorOpportunity.Id, Row = 5, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField competitorWeaknessesScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = competitorWeaknesses.Id, Row = 4, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField competitorThreatScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = competitorThreat.Id, Row = 5, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField competitorInActiveScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = competitorInActive.Id, Row = 6, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);

            ObjectContext.SaveChanges();
        }

        public void BuildAdditionalServiceScreens(Dictionary<string, Screen> tenantScreens, Dictionary<string, ScreenField> tenantScreenFields)
        {
            ObjectTable AdditionalServiceObject = ObjectContext.ObjectTables.Where(d => d.Name == "AdditionalService" && d.Tenant == 0).FirstOrDefault();

            ObjectField AdditionalServiceName = ObjectContext.ObjectFields.Where(d => d.FieldName == "Name" && d.ObjectTable.Id == AdditionalServiceObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField AdditionalServiceInActive = ObjectContext.ObjectFields.Where(d => d.FieldName == "InActive" && d.ObjectTableId == AdditionalServiceObject.Id && d.Tenant == 0).FirstOrDefault();

            //Header Screen
            Screen HeaderScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "AdditionalService.HeaderScreen", Name = "Header Screen", ObjectTableId = AdditionalServiceObject.Id, NumberOfColumns = 1, NumberOfRows = 1, IsReadOnly = true }, ScreensRepository, tenantScreens);
            ScreenField Header_NameScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = AdditionalServiceName.Id, Row = 0, ScreenId = HeaderScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);

            AdditionalServiceObject.HeaderScreenId = HeaderScreen.Id;

            //General Screen
            Screen generalTabScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "AdditionalService.GeneralTabScreen", Name = "General Tab Screen", ObjectTableId = AdditionalServiceObject.Id, NumberOfColumns = 2, NumberOfRows = 2 }, ScreensRepository, tenantScreens);

            ScreenField AdditionalServiceNameScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = AdditionalServiceName.Id, Row = 0, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField AdditionalServiceInActiveScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = AdditionalServiceInActive.Id, Row = 1, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);

            ObjectContext.SaveChanges();
        }

        public void BuildExternalSystemsTablesCodeScreens(Dictionary<string, Screen> tenantScreens, Dictionary<string, ScreenField> tenantScreenFields)
        {
            ObjectTable ExternalSystemsTablesCodeObject = ObjectContext.ObjectTables.Where(d => d.Name == "ExternalSystemsTablesCode" && d.Tenant == 0).FirstOrDefault();

            ObjectField LogitudeTable = ObjectContext.ObjectFields.Where(d => d.FieldName == "LogitudeTable" && d.ObjectTable.Id == ExternalSystemsTablesCodeObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField Code = ObjectContext.ObjectFields.Where(d => d.FieldName == "Code" && d.ObjectTable.Id == ExternalSystemsTablesCodeObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField Name = ObjectContext.ObjectFields.Where(d => d.FieldName == "Name" && d.ObjectTable.Id == ExternalSystemsTablesCodeObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField CreatedDate = ObjectContext.ObjectFields.Where(d => d.FieldName == "CreatedDate" && d.ObjectTable.Id == ExternalSystemsTablesCodeObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField UpdatedDate = ObjectContext.ObjectFields.Where(d => d.FieldName == "UpdatedDate" && d.ObjectTable.Id == ExternalSystemsTablesCodeObject.Id && d.Tenant == 0).FirstOrDefault();

            //Header Screen
            Screen HeaderScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "ExternalSystemsTablesCode.HeaderScreen", Name = "Header Screen", ObjectTableId = ExternalSystemsTablesCodeObject.Id, NumberOfColumns = 3, NumberOfRows = 1, IsReadOnly = true }, ScreensRepository, tenantScreens);

            ScreenField Header_CodeScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = Code.Id, Row = 0, ScreenId = HeaderScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField Header_NameScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = Name.Id, Row = 0, ScreenId = HeaderScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField Header_CreatedDateScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 2, ObjectFieldId = CreatedDate.Id, Row = 0, ScreenId = HeaderScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);

            ExternalSystemsTablesCodeObject.HeaderScreenId = HeaderScreen.Id;

            //General Screen
            Screen generalTabScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "ExternalSystemsTablesCode.GeneralTabScreen", Name = "General Tab Screen", ObjectTableId = ExternalSystemsTablesCodeObject.Id, NumberOfColumns = 1, NumberOfRows = 5 }, ScreensRepository, tenantScreens);

            ScreenField CodeScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = Code.Id, Row = 0, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField NameScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = Name.Id, Row = 1, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField LogitudeTableScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = LogitudeTable.Id, Row = 2, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField CreatedDateScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = CreatedDate.Id, Row = 3, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField UpdatedDateScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = UpdatedDate.Id, Row = 4, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);

            ObjectContext.SaveChanges();
        }

        public void BuildProductTypeScreens(Dictionary<string, Screen> tenantScreens, Dictionary<string, ScreenField> tenantScreenFields)
        {
            ObjectTable ProductTypeObject = ObjectContext.ObjectTables.Where(d => d.Name == "ProductType" && d.Tenant == 0).FirstOrDefault();

            ObjectField ProductTypeCode = ObjectContext.ObjectFields.Where(d => d.FieldName == "Code" && d.ObjectTable.Id == ProductTypeObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField ProductTypeName = ObjectContext.ObjectFields.Where(d => d.FieldName == "Name" && d.ObjectTable.Id == ProductTypeObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField ProductTypeInActive = ObjectContext.ObjectFields.Where(d => d.FieldName == "InActive" && d.ObjectTable.Id == ProductTypeObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField ProductTypeQuotationDefaultTemplateId = ObjectContext.ObjectFields.Where(d => d.FieldName == "QuotationDefaultTemplateId" && d.ObjectTable.Id == ProductTypeObject.Id && d.Tenant == 0).FirstOrDefault();
            
            //Header Screen
            Screen HeaderScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "ProductType.HeaderScreen", Name = "Header Screen", ObjectTableId = ProductTypeObject.Id, NumberOfColumns = 2, NumberOfRows = 1, IsReadOnly = true }, ScreensRepository, tenantScreens);

            ScreenField Header_CodeScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = ProductTypeCode.Id, Row = 0, ScreenId = HeaderScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField Header_NameScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = ProductTypeName.Id, Row = 0, ScreenId = HeaderScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);

            ProductTypeObject.HeaderScreenId = HeaderScreen.Id;

            //General Screen
            Screen generalTabScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "ProductType.GeneralTabScreen", Name = "General Tab Screen", ObjectTableId = ProductTypeObject.Id, NumberOfColumns = 2, NumberOfRows = 4 }, ScreensRepository, tenantScreens);

            ScreenField ProductTypeCodeScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = ProductTypeCode.Id, Row = 0, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField ProductTypeNameScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = ProductTypeName.Id, Row = 1, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField ProductTypeInActiveScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = ProductTypeInActive.Id, Row = 2, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField ProductTypeQuotationDefaultTemplateIdScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = ProductTypeQuotationDefaultTemplateId.Id, Row = 3, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ObjectContext.SaveChanges();
        }

        public void BuildSpecialServicesTypeScreens(Dictionary<string, Screen> tenantScreens, Dictionary<string, ScreenField> tenantScreenFields)
        {
            ObjectTable SpecialServicesTypeObject = ObjectContext.ObjectTables.Where(d => d.Name == "SpecialServicesType" && d.Tenant == 0).FirstOrDefault();

            ObjectField SpecialServicesTypeCode = ObjectContext.ObjectFields.Where(d => d.FieldName == "Code" && d.ObjectTable.Id == SpecialServicesTypeObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField SpecialServicesTypeEnglishName = ObjectContext.ObjectFields.Where(d => d.FieldName == "EnglishName" && d.ObjectTable.Id == SpecialServicesTypeObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField SpecialServicesTypeLocalName = ObjectContext.ObjectFields.Where(d => d.FieldName == "LocalName" && d.ObjectTable.Id == SpecialServicesTypeObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField SpecialServicesTypeInActive = ObjectContext.ObjectFields.Where(d => d.FieldName == "InActive" && d.ObjectTable.Id == SpecialServicesTypeObject.Id && d.Tenant == 0).FirstOrDefault();

            //Header Screen
            Screen HeaderScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "SpecialServciesType.HeaderScreen", Name = "Header Screen", ObjectTableId = SpecialServicesTypeObject.Id, NumberOfColumns = 2, NumberOfRows = 1, IsReadOnly = true }, ScreensRepository, tenantScreens);

            ScreenField Header_CodeScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = SpecialServicesTypeCode.Id, Row = 0, ScreenId = HeaderScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField Header_NameScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = SpecialServicesTypeEnglishName.Id, Row = 0, ScreenId = HeaderScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);

            //InActive
            SpecialServicesTypeObject.HeaderScreenId = HeaderScreen.Id;

            //General Screen
            Screen generalTabScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "SpecialServicesType.GeneralTabScreen", Name = "General Tab Screen", ObjectTableId = SpecialServicesTypeObject.Id, NumberOfColumns = 2, NumberOfRows = 4 }, ScreensRepository, tenantScreens);

            ScreenField SpecialServicesTypeCodeScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = SpecialServicesTypeCode.Id, Row = 0, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField SpecialServicesTypeEnglishNameScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = SpecialServicesTypeEnglishName.Id, Row = 1, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField SpecialServicesTypeLocalScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = SpecialServicesTypeLocalName.Id, Row = 2, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField SpecialServicesTypeInActiveScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = SpecialServicesTypeInActive.Id, Row = 3, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);

            ObjectContext.SaveChanges();
        }

        public void BuildRegionScreens(Dictionary<string, Screen> tenantScreens, Dictionary<string, ScreenField> tenantScreenFields)
        {
            ObjectTable RegionObject = ObjectContext.ObjectTables.Where(d => d.Name == "Region" && d.Tenant == 0).FirstOrDefault();

            ObjectField RegionEnglishName = ObjectContext.ObjectFields.Where(d => d.FieldName == "Name" && d.ObjectTable.Id == RegionObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField RegionLocalName = ObjectContext.ObjectFields.Where(d => d.FieldName == "LocalName" && d.ObjectTable.Id == RegionObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField RegionInActive = ObjectContext.ObjectFields.Where(d => d.FieldName == "InActive" && d.ObjectTable.Id == RegionObject.Id && d.Tenant == 0).FirstOrDefault();

            //Header Screen
            Screen HeaderScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "Region.HeaderScreen", Name = "Header Screen", ObjectTableId = RegionObject.Id, NumberOfColumns = 2, NumberOfRows = 1, IsReadOnly = true }, ScreensRepository, tenantScreens);

            ScreenField Header_EnglishNameScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = RegionEnglishName.Id, Row = 0, ScreenId = HeaderScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);

            RegionObject.HeaderScreenId = HeaderScreen.Id;

            //General Screen
            Screen generalTabScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "Region.GeneralTabScreen", Name = "General Tab Screen", ObjectTableId = RegionObject.Id, NumberOfColumns = 1, NumberOfRows = 3 }, ScreensRepository, tenantScreens);

            ScreenField RegionEnglishNameScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = RegionEnglishName.Id, Row = 0, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField RegionLocalScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = RegionLocalName.Id, Row = 1, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField RegionInActiveScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = RegionInActive.Id, Row = 2, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);

            ObjectContext.SaveChanges();
        }

        public void BuildCustomerSizeScreens(Dictionary<string, Screen> tenantScreens, Dictionary<string, ScreenField> tenantScreenFields)
        {
            ObjectTable CustomerSizeObject = ObjectContext.ObjectTables.Where(d => d.Name == "CustomerSize" && d.Tenant == 0).FirstOrDefault();

            ObjectField NameField = ObjectContext.ObjectFields.Where(d => d.FieldName == "Name" && d.ObjectTable.Id == CustomerSizeObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField OrderField = ObjectContext.ObjectFields.Where(d => d.FieldName == "Order" && d.ObjectTable.Id == CustomerSizeObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField InActiveField = ObjectContext.ObjectFields.Where(d => d.FieldName == "InActive" && d.ObjectTable.Id == CustomerSizeObject.Id && d.Tenant == 0).FirstOrDefault();

            //Header Screen
            Screen HeaderScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "CustomerSize.HeaderScreen", Name = "Header Screen", ObjectTableId = CustomerSizeObject.Id, NumberOfColumns = 2, NumberOfRows = 1, IsReadOnly = true }, ScreensRepository, tenantScreens);
            ScreenField Header_NameScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = NameField.Id, Row = 0, ScreenId = HeaderScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField Header_RankScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = OrderField.Id, Row = 0, ScreenId = HeaderScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            CustomerSizeObject.HeaderScreenId = HeaderScreen.Id;

            //General Screen
            Screen generalTabScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "CustomerSize.GeneralTabScreen", Name = "General Tab Screen", ObjectTableId = CustomerSizeObject.Id, NumberOfColumns = 2, NumberOfRows = 4 }, ScreensRepository, tenantScreens);
            ScreenField NameScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = NameField.Id, Row = 0, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField RankScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = OrderField.Id, Row = 1, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField InActiveScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = InActiveField.Id, Row = 2, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);

            ObjectContext.SaveChanges();
        }

        public void BuildBluesnapContractScreens(Dictionary<string, Screen> tenantScreens, Dictionary<string, ScreenField> tenantScreenFields)
        {
            ObjectTable BluesnapContractObject = ObjectContext.ObjectTables.Where(d => d.Name == "BluesnapContract" && d.Tenant == 0).FirstOrDefault();

            ObjectField BluesnapContractCode = ObjectContext.ObjectFields.Where(d => d.FieldName == "Code" && d.ObjectTable.Id == BluesnapContractObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField BluesnapContractName = ObjectContext.ObjectFields.Where(d => d.FieldName == "Name" && d.ObjectTable.Id == BluesnapContractObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField BluesnapContractContractId = ObjectContext.ObjectFields.Where(d => d.FieldName == "ContractId" && d.ObjectTable.Id == BluesnapContractObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField BluesnapContractInActive = ObjectContext.ObjectFields.Where(d => d.FieldName == "InActive" && d.ObjectTable.Id == BluesnapContractObject.Id && d.Tenant == 0).FirstOrDefault();

            //Header Screen
            Screen HeaderScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "BluesnapContract.HeaderScreen", Name = "Header Screen", ObjectTableId = BluesnapContractObject.Id, NumberOfColumns = 2, NumberOfRows = 1, IsReadOnly = true }, ScreensRepository, tenantScreens);
            ScreenField Header_CodeScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = BluesnapContractCode.Id, Row = 0, ScreenId = HeaderScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField Header_NameScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = BluesnapContractName.Id, Row = 0, ScreenId = HeaderScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            BluesnapContractObject.HeaderScreenId = HeaderScreen.Id;

            //General Screen
            Screen generalTabScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "BluesnapContract.GeneralTabScreen", Name = "General Tab Screen", ObjectTableId = BluesnapContractObject.Id, NumberOfColumns = 1, NumberOfRows = 4 }, ScreensRepository, tenantScreens);
            ScreenField BluesnapContractCodeScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = BluesnapContractCode.Id, Row = 0, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField BluesnapContractNameScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = BluesnapContractName.Id, Row = 1, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField BluesnapContractContractIdScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = BluesnapContractContractId.Id, Row = 2, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField BluesnapContractInActiveScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = BluesnapContractInActive.Id, Row = 3, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);

            ObjectContext.SaveChanges();
        }

        public void BuildAccountingSystemScreens(Dictionary<string, Screen> tenantScreens, Dictionary<string, ScreenField> tenantScreenFields)
        {
            ObjectTable AccountingSystemObject = ObjectContext.ObjectTables.Where(d => d.Name == "AccountingSystem" && d.Tenant == 0).FirstOrDefault();

            ObjectField AccountingSystemField01 = ObjectContext.ObjectFields.Where(d => d.FieldName == "Code" && d.ObjectTable.Id == AccountingSystemObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField AccountingSystemField02 = ObjectContext.ObjectFields.Where(d => d.FieldName == "Name" && d.ObjectTable.Id == AccountingSystemObject.Id && d.Tenant == 0).FirstOrDefault();

            //Header Screen
            Screen HeaderScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "AccountingSystem.HeaderScreen", Name = "Header Screen", ObjectTableId = AccountingSystemObject.Id, NumberOfColumns = 2, NumberOfRows = 1, IsReadOnly = true }, ScreensRepository, tenantScreens);
            ScreenField Header_CodeScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = AccountingSystemField01.Id, Row = 0, ScreenId = HeaderScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField Header_NameScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = AccountingSystemField02.Id, Row = 0, ScreenId = HeaderScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            AccountingSystemObject.HeaderScreenId = HeaderScreen.Id;

            ObjectContext.SaveChanges();
        }

        public void BuildCustomerTenantAccessScreens(Dictionary<string, Screen> tenantScreens, Dictionary<string, ScreenField> tenantScreenFields)
        {
            ObjectTable CustomerTenantAccessObject = ObjectContext.ObjectTables.Where(d => d.Name == "CustomerTenantAccess" && d.Tenant == 0).FirstOrDefault();

            ObjectField Field01 = ObjectContext.ObjectFields.Where(d => d.FieldName == "CompanyName" && d.ObjectTable.Id == CustomerTenantAccessObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField Field02 = ObjectContext.ObjectFields.Where(d => d.FieldName == "ContactName" && d.ObjectTable.Id == CustomerTenantAccessObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField Field03 = ObjectContext.ObjectFields.Where(d => d.FieldName == "ContactPhone" && d.ObjectTable.Id == CustomerTenantAccessObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField Field04 = ObjectContext.ObjectFields.Where(d => d.FieldName == "StatusName" && d.ObjectTable.Id == CustomerTenantAccessObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField Field05 = ObjectContext.ObjectFields.Where(d => d.FieldName == "CompanyVat" && d.ObjectTable.Id == CustomerTenantAccessObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField Field06 = ObjectContext.ObjectFields.Where(d => d.FieldName == "ContactMobile" && d.ObjectTable.Id == CustomerTenantAccessObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField Field07 = ObjectContext.ObjectFields.Where(d => d.FieldName == "RequestDateTime" && d.ObjectTable.Id == CustomerTenantAccessObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField Field08 = ObjectContext.ObjectFields.Where(d => d.FieldName == "CompanyEmail" && d.ObjectTable.Id == CustomerTenantAccessObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField Field09 = ObjectContext.ObjectFields.Where(d => d.FieldName == "CustomCompanyName" && d.ObjectTable.Id == CustomerTenantAccessObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField Field10 = ObjectContext.ObjectFields.Where(d => d.FieldName == "CustomerTenant" && d.ObjectTable.Id == CustomerTenantAccessObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField Field11 = ObjectContext.ObjectFields.Where(d => d.FieldName == "StockTypeCode" && d.ObjectTable.Id == CustomerTenantAccessObject.Id && d.Tenant == 0).FirstOrDefault();

            //==============Header screen===========================

            Screen HeaderScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "CustomerTenantAccess.HeaderScreen", Name = "Header Screen", ObjectTableId = CustomerTenantAccessObject.Id, NumberOfColumns = 5, NumberOfRows = 2, IsReadOnly = true }, ScreensRepository, tenantScreens);


            ScreenField Header_CompanyNameScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = Field09.Id, Row = 0, ScreenId = HeaderScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField Header_ContactNameScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = Field02.Id, Row = 0, ScreenId = HeaderScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField Header_ContactPhoneScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 2, ObjectFieldId = Field03.Id, Row = 0, ScreenId = HeaderScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField Header_StatusNameScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 3, ObjectFieldId = Field04.Id, Row = 0, ScreenId = HeaderScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField Header_CompanyVatScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = Field05.Id, Row = 1, ScreenId = HeaderScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField Header_ContactMobileScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 2, ObjectFieldId = Field06.Id, Row = 1, ScreenId = HeaderScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField Header_RequestDateTimeScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 3, ObjectFieldId = Field07.Id, Row = 1, ScreenId = HeaderScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField Header_CompanyEmailScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = Field08.Id, Row = 1, ScreenId = HeaderScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField Header_CustomerTenantScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 4, ObjectFieldId = Field10.Id, Row = 0, ScreenId = HeaderScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField Header_StockTypeCodeScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 4, ObjectFieldId = Field11.Id, Row = 1, ScreenId = HeaderScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);

            CustomerTenantAccessObject.HeaderScreenId = HeaderScreen.Id;

            ObjectContext.SaveChanges();

        }

        public void BuildCustomerTenantAccessRequestScreens(Dictionary<string, Screen> tenantScreens, Dictionary<string, ScreenField> tenantScreenFields)
        {
            ObjectTable CustomerTenantAccessRequestObject = ObjectContext.ObjectTables.Where(d => d.Name == "CustomerTenantAccessRequest" && d.Tenant == 0).FirstOrDefault();

            // General Screen
            Screen generalTabScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "CustomerTenantAccessRequest.GeneralTabScreen", Name = "General Tab Screen", ObjectTableId = CustomerTenantAccessRequestObject.Id, NumberOfColumns =1, NumberOfRows = 1 }, ScreensRepository, tenantScreens);

            ObjectField Forworder = ObjectContext.ObjectFields.Where(d => d.FieldName == "ForwarderId" && d.ObjectTableId == CustomerTenantAccessRequestObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField Status = ObjectContext.ObjectFields.Where(d => d.FieldName == "RequestStatus" && d.ObjectTableId == CustomerTenantAccessRequestObject.Id && d.Tenant == 0).FirstOrDefault();
           

            ScreenField ForworderScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = Forworder.Id, Row = 0, ScreenId = generalTabScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
      

            //==============Header screen===========================

            //Screen HeaderScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "CustomerTenantAccessRequest.HeaderScreen", Name = "Header Screen", ObjectTableId = CustomerTenantAccessRequestObject.Id, NumberOfColumns = 2, NumberOfRows = 1, IsReadOnly = true }, ScreensRepository, tenantScreens);


            //ScreenField Header_ForworderScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = Forworder.Id, Row = 0, ScreenId = HeaderScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            //ScreenField Header_StatusScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = Status.Id, Row = 0, ScreenId = HeaderScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
           
            //VesselObject.HeaderScreenId = HeaderScreen.Id;

            ObjectContext.SaveChanges();

        }

        public void BuildParticipantScreens(Dictionary<string, Screen> tenantScreens, Dictionary<string, ScreenField> tenantScreenFields)
        {
            ObjectTable ThisObject = ObjectContext.ObjectTables.Where(d => d.Name == "Participant" && d.Tenant == 0).FirstOrDefault();
           
            #region HeaderScreen
            Screen HeaderScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "Participant.HeaderScreen", Name = "Header Screen", ObjectTableId = ThisObject.Id, NumberOfColumns = 2, NumberOfRows = 2, IsReadOnly = true }, ScreensRepository, tenantScreens);

            ObjectField headerObjectField1 = ObjectContext.ObjectFields.Where(d => d.ObjectTableId == ThisObject.Id && d.FieldName == "Code").FirstOrDefault();
            ObjectField headerObjectField2 = ObjectContext.ObjectFields.Where(d => d.ObjectTableId == ThisObject.Id && d.FieldName == "EnglishName").FirstOrDefault();
            ObjectField headerObjectField3 = ObjectContext.ObjectFields.Where(d => d.ObjectTableId == ThisObject.Id && d.FieldName == "ForwarderTenant").FirstOrDefault();
            ObjectField headerObjectField4 = ObjectContext.ObjectFields.Where(d => d.ObjectTableId == ThisObject.Id && d.FieldName == "TTY").FirstOrDefault();

            ScreenField headerScreenField1 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = headerObjectField1.Id, Row = 0, ScreenId = HeaderScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField headerScreenField2 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = headerObjectField2.Id, Row = 1, ScreenId = HeaderScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField headerScreenField3 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = headerObjectField3.Id, Row = 0, ScreenId = HeaderScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField headerScreenField4 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = headerObjectField4.Id, Row = 1, ScreenId = HeaderScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ThisObject.HeaderScreenId = HeaderScreen.Id;
            #endregion
            
            #region BillingTabScreen
            Screen BillingTabScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "Participant.BillingTabScreen", Name = "Billing Tab Screen", ObjectTableId = ThisObject.Id, NumberOfColumns = 2, NumberOfRows = 6 }, ScreensRepository, tenantScreens);

            ObjectField billingObjectField01 = ObjectContext.ObjectFields.Where(d => d.FieldName == "VatNumber" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField billingObjectField02 = ObjectContext.ObjectFields.Where(d => d.FieldName == "PaymentTermId" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField billingObjectField03 = ObjectContext.ObjectFields.Where(d => d.FieldName == "InvoiceCurrencyId" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField billingObjectField04 = ObjectContext.ObjectFields.Where(d => d.FieldName == "VatTypeId" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField billingObjectField05 = ObjectContext.ObjectFields.Where(d => d.FieldName == "BankName" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField billingObjectField06 = ObjectContext.ObjectFields.Where(d => d.FieldName == "BankAddress" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField billingObjectField07 = ObjectContext.ObjectFields.Where(d => d.FieldName == "Swift" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField billingObjectField08 = ObjectContext.ObjectFields.Where(d => d.FieldName == "AccountNumber" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField billingObjectField09 = ObjectContext.ObjectFields.Where(d => d.FieldName == "IBANNumber" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();

            ScreenField billingScreenField01 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = billingObjectField01.Id, Row = 0, ScreenId = BillingTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField billingScreenField02 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = billingObjectField02.Id, Row = 1, ScreenId = BillingTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField billingScreenField03 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = billingObjectField03.Id, Row = 2, ScreenId = BillingTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField billingScreenField04 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = billingObjectField04.Id, Row = 3, ScreenId = BillingTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField billingScreenField05 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = billingObjectField05.Id, Row = 0, ScreenId = BillingTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField billingScreenField06 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = billingObjectField06.Id, Row = 1, ScreenId = BillingTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField billingScreenField07 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = billingObjectField07.Id, Row = 2, ScreenId = BillingTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField billingScreenField08 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = billingObjectField08.Id, Row = 3, ScreenId = BillingTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField billingScreenField09 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = billingObjectField09.Id, Row = 4, ScreenId = BillingTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            #endregion

            #region Additional Fields
            Screen aditionalFieldsScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "Participant.AdditionalFields", Name = "Additional Fields", ObjectTableId = ThisObject.Id, NumberOfColumns = 1, NumberOfRows = 4, Tenant = 0 }, ScreensRepository, tenantScreens);
            #endregion

            ObjectContext.SaveChanges();

        }

        public void BuildAirlineStatisticsScreens(Dictionary<string, Screen> tenantScreens, Dictionary<string, ScreenField> tenantScreenFields)
        {
            ObjectTable ThisObject = ObjectContext.ObjectTables.Where(d => d.Name == "AirlineStatistics" && d.Tenant == 0).FirstOrDefault();

            #region HeaderScreen
            Screen HeaderScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "AirlineStatistics.HeaderScreen", Name = "Header Screen", ObjectTableId = ThisObject.Id, NumberOfColumns = 2, NumberOfRows = 2, IsReadOnly = true }, ScreensRepository, tenantScreens);

            ObjectField headerObjectField1 = ObjectContext.ObjectFields.Where(d => d.ObjectTableId == ThisObject.Id && d.FieldName == "SourceTenantName").FirstOrDefault();
            ObjectField headerObjectField2 = ObjectContext.ObjectFields.Where(d => d.ObjectTableId == ThisObject.Id && d.FieldName == "EntityCreatedByUserName").FirstOrDefault();
            ObjectField headerObjectField3 = ObjectContext.ObjectFields.Where(d => d.ObjectTableId == ThisObject.Id && d.FieldName == "EntityReference").FirstOrDefault();
            ObjectField headerObjectField4 = ObjectContext.ObjectFields.Where(d => d.ObjectTableId == ThisObject.Id && d.FieldName == "AWBNumber").FirstOrDefault();

            ScreenField headerScreenField1 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = headerObjectField1.Id, Row = 0, ScreenId = HeaderScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField headerScreenField2 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = headerObjectField2.Id, Row = 1, ScreenId = HeaderScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField headerScreenField3 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = headerObjectField3.Id, Row = 0, ScreenId = HeaderScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField headerScreenField4 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = headerObjectField4.Id, Row = 1, ScreenId = HeaderScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ThisObject.HeaderScreenId = HeaderScreen.Id;
            #endregion

            #region GeneralTabScreen
            Screen generalTabScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "AirlineStatistics.GeneralTabScreen", Name = "General Tab Screen", ObjectTableId = ThisObject.Id, NumberOfColumns = 2, NumberOfRows = 7 }, ScreensRepository, tenantScreens);

            ObjectField generalObjectField01 = ObjectContext.ObjectFields.Where(d => d.FieldName == "AirlineCode" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField generalObjectField02 = ObjectContext.ObjectFields.Where(d => d.FieldName == "MessageType" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField generalObjectField03 = ObjectContext.ObjectFields.Where(d => d.FieldName == "LastSentDate" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField generalObjectField04 = ObjectContext.ObjectFields.Where(d => d.FieldName == "OriginCode" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField generalObjectField05 = ObjectContext.ObjectFields.Where(d => d.FieldName == "DestinationCode" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField generalObjectField06 = ObjectContext.ObjectFields.Where(d => d.FieldName == "ShipperName" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField generalObjectField07 = ObjectContext.ObjectFields.Where(d => d.FieldName == "ConsigneeName" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField generalObjectField08 = ObjectContext.ObjectFields.Where(d => d.FieldName == "GrossWeight" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField generalObjectField09 = ObjectContext.ObjectFields.Where(d => d.FieldName == "ChargeableWeight" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField generalObjectField10 = ObjectContext.ObjectFields.Where(d => d.FieldName == "NumberOfPackages" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();

            ScreenField generalScreenField01 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = generalObjectField01.Id, Row = 0, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField generalScreenField02 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = generalObjectField02.Id, Row = 1, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField generalScreenField03 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = generalObjectField03.Id, Row = 2, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField generalScreenField04 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = generalObjectField04.Id, Row = 3, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField generalScreenField05 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = generalObjectField05.Id, Row = 4, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField generalScreenField06 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = generalObjectField06.Id, Row = 5, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField generalScreenField07 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = generalObjectField07.Id, Row = 6, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField generalScreenField08 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = generalObjectField08.Id, Row = 0, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField generalScreenField09 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = generalObjectField09.Id, Row = 1, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField generalScreenField10 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = generalObjectField10.Id, Row = 2, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);

            #endregion

            ObjectContext.SaveChanges();

        }

        private void BuildLogitudeMessagesScreens(Dictionary<string, Screen> tenantScreens, Dictionary<string, ScreenField> tenantScreenFields)
        {
            ObjectTable ThisObject = ObjectContext.ObjectTables.Where(d => d.Name == "LogitudeMessagesTransmissionLog" && d.Tenant == 0).FirstOrDefault();

            #region HeaderScreen
            Screen HeaderScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "LogitudeMessagesTransmissionLog.HeaderScreen", Name = "Header Screen", ObjectTableId = ThisObject.Id, NumberOfColumns = 2, NumberOfRows = 2, IsReadOnly = true }, ScreensRepository, tenantScreens);

            ObjectField headerObjectField1 = ObjectContext.ObjectFields.Where(d => d.ObjectTableId == ThisObject.Id && d.FieldName == "AirlineCode").FirstOrDefault();
            ObjectField headerObjectField2 = ObjectContext.ObjectFields.Where(d => d.ObjectTableId == ThisObject.Id && d.FieldName == "MessageTypeCode").FirstOrDefault();
            ObjectField headerObjectField3 = ObjectContext.ObjectFields.Where(d => d.ObjectTableId == ThisObject.Id && d.FieldName == "AWBNumber").FirstOrDefault();
            ObjectField headerObjectField4 = ObjectContext.ObjectFields.Where(d => d.ObjectTableId == ThisObject.Id && d.FieldName == "SentDate").FirstOrDefault();

            ScreenField headerScreenField1 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = headerObjectField1.Id, Row = 0, ScreenId = HeaderScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField headerScreenField2 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = headerObjectField2.Id, Row = 1, ScreenId = HeaderScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField headerScreenField3 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = headerObjectField3.Id, Row = 0, ScreenId = HeaderScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField headerScreenField4 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = headerObjectField4.Id, Row = 1, ScreenId = HeaderScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ThisObject.HeaderScreenId = HeaderScreen.Id;
            #endregion

            #region GeneralTabScreen
            Screen generalTabScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "LogitudeMessagesTransmissionLog.GeneralTabScreen", Name = "General Tab Screen", ObjectTableId = ThisObject.Id, NumberOfColumns = 2, NumberOfRows = 12 }, ScreensRepository, tenantScreens);

            ObjectField generalObjectField01 = ObjectContext.ObjectFields.Where(d => d.FieldName == "CCS" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField generalObjectField02 = ObjectContext.ObjectFields.Where(d => d.FieldName == "MessageTypeCode" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField generalObjectField03 = ObjectContext.ObjectFields.Where(d => d.FieldName == "AirlineCode" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField generalObjectField04 = ObjectContext.ObjectFields.Where(d => d.FieldName == "AWBNumber" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField generalObjectField05 = ObjectContext.ObjectFields.Where(d => d.FieldName == "SentDate" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField generalObjectField06 = ObjectContext.ObjectFields.Where(d => d.FieldName == "UserName" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField generalObjectField07 = ObjectContext.ObjectFields.Where(d => d.FieldName == "UserEmail" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField generalObjectField08 = ObjectContext.ObjectFields.Where(d => d.FieldName == "Participant" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField generalObjectField09 = ObjectContext.ObjectFields.Where(d => d.FieldName == "Origin" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField generalObjectField10 = ObjectContext.ObjectFields.Where(d => d.FieldName == "Destination" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField generalObjectField11 = ObjectContext.ObjectFields.Where(d => d.FieldName == "Pieces" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField generalObjectField12 = ObjectContext.ObjectFields.Where(d => d.FieldName == "GrossWeight" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField generalObjectField13 = ObjectContext.ObjectFields.Where(d => d.FieldName == "ChargeableWeight" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField generalObjectField14 = ObjectContext.ObjectFields.Where(d => d.FieldName == "Volume" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField generalObjectField15 = ObjectContext.ObjectFields.Where(d => d.FieldName == "DescriptionOfGoods" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField generalObjectField16 = ObjectContext.ObjectFields.Where(d => d.FieldName == "DirectParticipant" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField generalObjectField17 = ObjectContext.ObjectFields.Where(d => d.FieldName == "IsUpdatedinAirlineTenant" && d.ObjectTableId == ThisObject.Id && d.Tenant == 0).FirstOrDefault();

            ScreenField generalScreenField01 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = generalObjectField01.Id, Row = 0, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField generalScreenField02 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = generalObjectField02.Id, Row = 1, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField generalScreenField03 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = generalObjectField03.Id, Row = 2, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField generalScreenField04 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = generalObjectField04.Id, Row = 3, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField generalScreenField05 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = generalObjectField05.Id, Row = 4, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField generalScreenField06 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = generalObjectField06.Id, Row = 5, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField generalScreenField07 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = generalObjectField07.Id, Row = 6, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField generalScreenField08 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = generalObjectField16.Id, Row = 7, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField generalScreenField09 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = generalObjectField17.Id, Row = 8, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField generalScreenField10 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = generalObjectField08.Id, Row = 0, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField generalScreenField11 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = generalObjectField09.Id, Row = 1, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField generalScreenField12 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = generalObjectField10.Id, Row = 2, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField generalScreenField13 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = generalObjectField11.Id, Row = 3, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField generalScreenField14 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = generalObjectField12.Id, Row = 4, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField generalScreenField15 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = generalObjectField13.Id, Row = 5, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField generalScreenField16 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = generalObjectField14.Id, Row = 6, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField generalScreenField17 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = generalObjectField15.Id, Row = 7, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);

            #endregion

            ObjectContext.SaveChanges();
        }

        private void BuildCustomerFieldsUpdateSettingScreens(Dictionary<string, Screen> tenantScreens, Dictionary<string, ScreenField> tenantScreenFields)
        {
            ObjectTable ThisObject = ObjectContext.ObjectTables.Where(d => d.Name == "CustomerFieldsUpdateSetting" && d.Tenant == 0).FirstOrDefault();

            #region HeaderScreen
            Screen HeaderScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "CustomerFieldsUpdateSetting.HeaderScreen", Name = "Header Screen", ObjectTableId = ThisObject.Id, NumberOfColumns = 2, NumberOfRows =1, IsReadOnly = true }, ScreensRepository, tenantScreens);

            ObjectField headerObjectField1 = ObjectContext.ObjectFields.Where(d => d.ObjectTableId == ThisObject.Id && d.FieldName == "ObjectFieldName").FirstOrDefault();
            ObjectField headerObjectField2 = ObjectContext.ObjectFields.Where(d => d.ObjectTableId == ThisObject.Id && d.FieldName == "UpdateDirection").FirstOrDefault();

            ScreenField headerScreenField1 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = headerObjectField1.Id, Row = 0, ScreenId = HeaderScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField headerScreenField2 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = headerObjectField2.Id, Row = 0, ScreenId = HeaderScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
           
            ThisObject.HeaderScreenId = HeaderScreen.Id;
            #endregion



            ObjectContext.SaveChanges();
        }

        private void BuildChargesGroupScreens(Dictionary<string, Screen> tenantScreens, Dictionary<string, ScreenField> tenantScreenFields)
        {
            ObjectTable ChargesGroupObject = ObjectContext.ObjectTables.Where(d => d.Name == "ChargesGroup" && d.Tenant == 0).FirstOrDefault();

            ObjectField CodeField = ObjectContext.ObjectFields.Where(d => d.FieldName == "Code" && d.ObjectTableId == ChargesGroupObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField NameField = ObjectContext.ObjectFields.Where(d => d.FieldName == "Name" && d.ObjectTableId == ChargesGroupObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField LocalNameField = ObjectContext.ObjectFields.Where(d => d.FieldName == "LocalName" && d.ObjectTableId == ChargesGroupObject.Id && d.Tenant == 0).FirstOrDefault();

            #region Header Screen
            Screen HeaderScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "ChargesGroup.HeaderScreen", Name = "Header Screen", ObjectTableId = ChargesGroupObject.Id, NumberOfColumns =3, NumberOfRows = 1, IsReadOnly = true }, ScreensRepository, tenantScreens);

            ScreenField ScreenField1 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 0, ObjectFieldId = CodeField.Id, ScreenId = HeaderScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField ScreenField2 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, Row = 0, ObjectFieldId = NameField.Id, ScreenId = HeaderScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField ScreenField3 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 2, Row = 0, ObjectFieldId = LocalNameField.Id, ScreenId = HeaderScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            ChargesGroupObject.HeaderScreenId = HeaderScreen.Id;
            #endregion

            #region General Screen
            Screen generalTabScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "ChargesGroup.GeneralTabScreen", Name = "General Tab Screen", ObjectTableId = ChargesGroupObject.Id, NumberOfColumns = 1, NumberOfRows =3 }, ScreensRepository, tenantScreens);

            ScreenField GScreenField1 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 0, Column = 0, ObjectFieldId = CodeField.Id, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField GScreenField2 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 1, Column = 0, ObjectFieldId = NameField.Id, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField GScreenField3 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 2, Column = 0, ObjectFieldId = LocalNameField.Id, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);

            #endregion

            ObjectContext.SaveChanges();
        }

        public void BuildBankAccountLiteScreens(Dictionary<string, Screen> tenantScreens, Dictionary<string, ScreenField> tenantScreenFields)
        {
            ObjectTable bankAccountObject = ObjectContext.ObjectTables.Where(d => d.Name == "BankAccountLite" && d.Tenant == 0).FirstOrDefault();

            ObjectField bankAccountBankCode = ObjectContext.ObjectFields.Where(d => d.FieldName == "BankCode" && d.ObjectTable.Id == bankAccountObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField bankAccountBranchNumber = ObjectContext.ObjectFields.Where(d => d.FieldName == "BranchNumber" && d.ObjectTable.Id == bankAccountObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField bankAccountAccountNumber = ObjectContext.ObjectFields.Where(d => d.FieldName == "AccountNumber" && d.ObjectTable.Id == bankAccountObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField bankAccountCurrencyId = ObjectContext.ObjectFields.Where(d => d.FieldName == "CurrencyId" && d.ObjectTable.Id == bankAccountObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField bankAccountEnglishName = ObjectContext.ObjectFields.Where(d => d.FieldName == "EnglishName" && d.ObjectTable.Id == bankAccountObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField bankAccountLocalName = ObjectContext.ObjectFields.Where(d => d.FieldName == "LocalName" && d.ObjectTable.Id == bankAccountObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField bankAccountIBAN = ObjectContext.ObjectFields.Where(d => d.FieldName == "IBAN" && d.ObjectTable.Id == bankAccountObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField bankAccountSwiftCode = ObjectContext.ObjectFields.Where(d => d.FieldName == "SwiftCode" && d.ObjectTable.Id == bankAccountObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField bankAccountBranchAddress = ObjectContext.ObjectFields.Where(d => d.FieldName == "BranchAddress" && d.ObjectTable.Id == bankAccountObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField bankAccountInactive = ObjectContext.ObjectFields.Where(d => d.FieldName == "Inactive" && d.ObjectTable.Id == bankAccountObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField bankAccountVatNumber = ObjectContext.ObjectFields.Where(d => d.FieldName == "VatNumber" && d.ObjectTable.Id == bankAccountObject.Id && d.Tenant == 0).FirstOrDefault();


            #region Header Screen
            Screen headerScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "BankAccountLite.HeaderScreen", Name = "Header Screen", ObjectTableId = bankAccountObject.Id, NumberOfColumns = 3, NumberOfRows = 1, IsReadOnly = true }, ScreensRepository, tenantScreens);

            ScreenField screenField1 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 0, ObjectFieldId = bankAccountAccountNumber.Id, ScreenId = headerScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField screenField2 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, Row = 0, ObjectFieldId = bankAccountBankCode.Id, ScreenId = headerScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField screenField3 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 2, Row = 0, ObjectFieldId = bankAccountBranchNumber.Id, ScreenId = headerScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);

            bankAccountObject.HeaderScreenId = headerScreen.Id;
            #endregion

            //General Screen
            Screen generalTabScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "BankAccountLite.GeneralTabScreen", Name = "General Tab Screen", ObjectTableId = bankAccountObject.Id, NumberOfColumns = 3, NumberOfRows = 6 }, ScreensRepository, tenantScreens);
            ScreenField bankAccountBankCodeScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = bankAccountBankCode.Id, Row = 0, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField bankAccountBranchNumberScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = bankAccountBranchNumber.Id, Row = 1, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField bankAccountAccountNumberScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = bankAccountAccountNumber.Id, Row = 2, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField bankAccountCurrencyIdScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = bankAccountCurrencyId.Id, Row = 3, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField bankAccountEnglishNameScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = bankAccountEnglishName.Id, Row = 4, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField bankAccountLocalNameScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = bankAccountLocalName.Id, Row = 5, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField bankAccountIBANScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = bankAccountIBAN.Id, Row = 0, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField bankAccountSwiftCodeScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = bankAccountSwiftCode.Id, Row = 1, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);

            ScreenField bankAccountVatNumberScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = bankAccountVatNumber.Id, Row = 2, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField bankAccountBranchAddressScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = bankAccountBranchAddress.Id, Row = 3, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField bankAccountInactiveScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = bankAccountInactive.Id, Row = 4, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ObjectContext.SaveChanges();
        }

        public void BuildAccountingPaymentMethodScreens(Dictionary<string, Screen> tenantScreens, Dictionary<string, ScreenField> tenantScreenFields)
        {
            ObjectTable AccountingPaymentMethodObject = ObjectContext.ObjectTables.Where(d => d.Name == "AccountingPaymentMethod" && d.Tenant == 0).FirstOrDefault();

            ObjectField AccountingPaymentMethodCode = ObjectContext.ObjectFields.Where(d => d.FieldName == "Code" && d.ObjectTable.Id == AccountingPaymentMethodObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField AccoutningPaymentMethodName = ObjectContext.ObjectFields.Where(d => d.FieldName == "Name" && d.ObjectTable.Id == AccountingPaymentMethodObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField AccoutningPaymentMethodInActive = ObjectContext.ObjectFields.Where(d => d.FieldName == "Inactive" && d.ObjectTable.Id == AccountingPaymentMethodObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField AccoutningPaymentMethodIsAR = ObjectContext.ObjectFields.Where(d => d.FieldName == "IsAR" && d.ObjectTable.Id == AccountingPaymentMethodObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField AccoutningPaymentMethodIsAP = ObjectContext.ObjectFields.Where(d => d.FieldName == "IsAP" && d.ObjectTable.Id == AccountingPaymentMethodObject.Id && d.Tenant == 0).FirstOrDefault();

            //Header Screen
            Screen HeaderScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "AccountingPaymentMethod.HeaderScreen", Name = "Header Screen", ObjectTableId = AccountingPaymentMethodObject.Id, NumberOfColumns = 2, NumberOfRows = 1, IsReadOnly = true }, ScreensRepository, tenantScreens);
            ScreenField Header_CodeScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = AccountingPaymentMethodCode.Id, Row = 0, ScreenId = HeaderScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField Header_NameScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = AccoutningPaymentMethodName.Id, Row = 0, ScreenId = HeaderScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            AccountingPaymentMethodObject.HeaderScreenId = HeaderScreen.Id;

            //General Screen
            Screen generalTabScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "AccountingPaymentMethod.GeneralTabScreen", Name = "General Tab Screen", ObjectTableId = AccountingPaymentMethodObject.Id, NumberOfColumns = 1, NumberOfRows = 5 }, ScreensRepository, tenantScreens);
            ScreenField AccountingPaymentMethodCodeScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = AccountingPaymentMethodCode.Id, Row = 0, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField AccountingPaymentMethodNameScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = AccoutningPaymentMethodName.Id, Row = 1, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField AccountingPaymentMethodInActiveScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = AccoutningPaymentMethodInActive.Id, Row = 2, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField AccountingPaymentMethodIsARScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = AccoutningPaymentMethodIsAR.Id, Row = 3, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField AccountingPaymentMethodIsAPScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = AccoutningPaymentMethodIsAP.Id, Row = 4, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);

            ObjectContext.SaveChanges();
        }

        public void BuildAPPaymentMethodScreens(Dictionary<string, Screen> tenantScreens, Dictionary<string, ScreenField> tenantScreenFields)
        {
            ObjectTable APPaymentMethodObject = ObjectContext.ObjectTables.Where(d => d.Name == "APPaymentMethod" && d.Tenant == 0).FirstOrDefault();

            ObjectField APPaymentMethodCode = ObjectContext.ObjectFields.Where(d => d.FieldName == "Code" && d.ObjectTable.Id == APPaymentMethodObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField APPaymentMethodName = ObjectContext.ObjectFields.Where(d => d.FieldName == "Name" && d.ObjectTable.Id == APPaymentMethodObject.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField APPaymentMethodInActive = ObjectContext.ObjectFields.Where(d => d.FieldName == "Inactive" && d.ObjectTable.Id == APPaymentMethodObject.Id && d.Tenant == 0).FirstOrDefault();

            //Header Screen
            Screen HeaderScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "APPaymentMethod.HeaderScreen", Name = "Header Screen", ObjectTableId = APPaymentMethodObject.Id, NumberOfColumns = 2, NumberOfRows = 1, IsReadOnly = true }, ScreensRepository, tenantScreens);
            ScreenField Header_CodeScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = APPaymentMethodCode.Id, Row = 0, ScreenId = HeaderScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField Header_NameScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, ObjectFieldId = APPaymentMethodName.Id, Row = 0, ScreenId = HeaderScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            APPaymentMethodObject.HeaderScreenId = HeaderScreen.Id;

            //General Screen
            Screen generalTabScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "APPaymentMethod.GeneralTabScreen", Name = "General Tab Screen", ObjectTableId = APPaymentMethodObject.Id, NumberOfColumns = 1, NumberOfRows = 4 }, ScreensRepository, tenantScreens);
            ScreenField APPaymentMethodCodeScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = APPaymentMethodCode.Id, Row = 0, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField APPaymentMethodNameScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = APPaymentMethodName.Id, Row = 1, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
            ScreenField APPaymentMethodInActiveScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, ObjectFieldId = APPaymentMethodInActive.Id, Row = 2, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);

            ObjectContext.SaveChanges();
        }
    }
}