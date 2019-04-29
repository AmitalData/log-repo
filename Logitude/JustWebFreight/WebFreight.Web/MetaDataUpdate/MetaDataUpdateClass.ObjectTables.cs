using Simplog.Data.InfrastructureModel.EntityPOCOs;
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
        #region Objects
        ObjectTable ShipmentObject;
        ObjectTable ShipmentReceivablesObject;
        ObjectTable ShipmentPayablesObject;
        ObjectTable ShipmentPayableLineStatusObject;
        ObjectTable ShipmentPackagesObject;
        ObjectTable InsideShipmentPackagesObject;
        ObjectTable PickUpDeliveryFromToTypeObject;
        ObjectTable ShipmentPickUpDeliveryObject;
        ObjectTable ShipmentOrderPackageObject;
        ObjectTable ShipmentCustomerTypeObject;
        ObjectTable ShipmentPickUpDeliveryPackagesObject;
        ObjectTable ShipmentAWBPrintOnlyObject;
        ObjectTable FixedAmountObject;
        ObjectTable QuoteObject;
        ObjectTable QuoteTypeObject;
        ObjectTable MarkUpTypeObject;
        ObjectTable QuoteChargeObject;
        ObjectTable QuoteSaleChargesObject;
        ObjectTable QuoteCostChargesObject;
        ObjectTable QuotePriceStepObject;
        ObjectTable QuoteCustomerTypeObject;
        ObjectTable QuoteSalesTotalObject;
        ObjectTable CardsObject;
        ObjectTable CarriersObject;
        ObjectTable PartnerTypesObject;
        ObjectTable CustomersObject;
        ObjectTable AgentObject;
        ObjectTable CustomAgentsObject;
        ObjectTable ShippingAgentsObject;
        ObjectTable AirlinesObject;
        ObjectTable TruckersObject;
        ObjectTable ShippingLinesObject;
        ObjectTable BranchesObject;
        ObjectTable ChargesTypesObject;
        ObjectTable ChargeTypeAccountingObject;
        ObjectTable AddressObject;
        ObjectTable ContactsObject;
        ObjectTable CountriesObject;
        ObjectTable CurrenciesObject;
        ObjectTable DepartmentsObject;
        ObjectTable DirectionsObject;
        ObjectTable ExternalDocumentsObject;
        ObjectTable DocumentsFilingObject;
        ObjectTable InternalDocumentsObject;
        ObjectTable FollowUpsObject;
        ObjectTable GlobalZonesObject;
        ObjectTable IncotermsObject;
        ObjectTable MasterObject;
        ObjectTable PaymentTermsObject;
        ObjectTable PortsObject;
        ObjectTable SpecialServicesObject;
        ObjectTable StatesObject;
        ObjectTable CountryCityObject;
        ObjectTable UsersObject;
        ObjectTable InvoiceObject;
        ObjectTable InvoiceLinesObject;
        ObjectTable InvoiceTypesObject;
        ObjectTable InvoiceStatusObject;
        ObjectTable ARInvoiceTransferStatusObject;
        ObjectTable APInvoiceTransferStatusObject;
        ObjectTable ARPaymentTransferStatusObject;
        ObjectTable VatTypesObject;
        ObjectTable VatTypePercentageObject;
        ObjectTable ShipmentReceivableLineStatusObject;
        ObjectTable FollowUpTypesObject;
        ObjectTable EventTypesObject;
        ObjectTable PackageTypesObject;
        ObjectTable DocumentTypesObject;
        ObjectTable EntityDateObject;
        ObjectTable BasketTypeObject;
        ObjectTable WeightUnitObject;
        ObjectTable RateClassObject;
        ObjectTable DimensionsUnitObject;
        ObjectTable AccountingSystemObject;
        ObjectTable AccountingSettingObject;
        ObjectTable VolumeUnitObject;
        ObjectTable ContainerTypesObject;
        ObjectTable MeasurementsObject;
        ObjectTable ShipmentTypesObject;
        ObjectTable PrepaidCollectObject;
        ObjectTable DueTypeObject;
        ObjectTable RatesTableObject;
        ObjectTable TransportModesObject;
        ObjectTable ObjectTableObject;
        ObjectTable ObjectFieldObject;
        ObjectTable EntityStatusObject;
        ObjectTable TraceEventObject;
        ObjectTable ChargesGroupObject;
        ObjectTable IATACodeObject;
        ObjectTable VesselObject;
        ObjectTable WareHouseObject;
        ObjectTable TemplateFormatObject;
        ObjectTable DocumentTypeTemplateObject;
        ObjectTable RankObject;
        ObjectTable DocumentTypeCustomFieldObject;
        ObjectTable SystemDataObject;
        ObjectTable SharedLogisticsObject;
        ObjectTable TenantObject;
        ObjectTable ShipmentReceivableStatusObject;
        ObjectTable ShipmentPayableStatusObject;
        ObjectTable DescriptionOfGoodsObject;
        ObjectTable CounterDefinitionObject;
        ObjectTable GeneralObject;
        ObjectTable TarrifChargesObject;
        ObjectTable TarrifFromToObject;
        ObjectTable TarrifFromToTypeObject;
        ObjectTable TarrifHeaderObject;
        ObjectTable TarrifStepObject;
        ObjectTable TarrifTypeObject;
        ObjectTable TextCodeObject;
        ObjectTable MAWBStackObject;
        ObjectTable RestrictionObject;
        ObjectTable RoleObject;
        ObjectTable ShipmentLevelObject;
        ObjectTable AccountObject;
        ObjectTable AccountTypeObject;
        ObjectTable ARInvoicePaymentObject;
        ObjectTable ARPaymentObject;
        ObjectTable PaymentMethodObject;
        ObjectTable AccountingPaymentMethodObject;
        ObjectTable ARPaymentStatusObject;
        ObjectTable APInvoiceObject;
        ObjectTable APInvoiceLineObject;
        ObjectTable APInvoiceStatusObject;
        ObjectTable ARInvoiceTotalVatObject;
        ObjectTable APInvoiceTotalVatObject;
        ObjectTable APInvoiceTypesObject;
        ObjectTable APPaymentObject;
        ObjectTable APPaymentMethodObject;
        ObjectTable APPaymentStatusObject;
        ObjectTable APInvoicePaymentObject;
        ObjectTable BankAccountLiteObject;
        ObjectTable VendorObject;
        ObjectTable AWBChargesCodeObject;
        ObjectTable AWBSpecialHandlingCodeObject;
        ObjectTable CommunicationLogsObject;
        ObjectTable CommunicationLogTypesObject;
        ObjectTable WarehouseTypesObject;
        ObjectTable CommunicationStatusTypesObject;
        ObjectTable PasswordPoliciesObject;
        ObjectTable PackagesObject;
        ObjectTable TenantManagementObject;
        ObjectTable RecurringPeriodObject;
        ObjectTable PaymentChannelObject;
        ObjectTable TermsofUseSignatureObject;
        ObjectTable AnalyzeQueueObject;
        ObjectTable CreditCardTypeObject;
        ObjectTable EventTypeCategoryObject;
        ObjectTable ErrorLogObject;
        ObjectTable AccountingTransferHeaderObject;
        ObjectTable AccountingTransferLineObject;
        ObjectTable AccountingTransferTypeObject;
        ObjectTable MoveTypeObject;
        ObjectTable ReportObject;
        ObjectTable SharedLogisticsInvitationStatusObject;
        ObjectTable DocumentTypeCopy;
        ObjectTable AWBOCIObject;
        ObjectTable AWBCustomsInfoObject;
        ObjectTable AWBInformationObject;
        ObjectTable ShipmentPackageItemObject;
        ObjectTable LeadSourceObject;
        ObjectTable IndustryObject;
        ObjectTable ProductTypeObject;
        ObjectTable ProductPeriodObject;
        ObjectTable CustomerProductObject;
        ObjectTable CustomerProductActualDataObject;
        ObjectTable CustomerProductLocationObject;
        ObjectTable CustomerProductLocationActualDataObject;
        ObjectTable CompetitorObject;
        ObjectTable CustomerAdditionalServiceObject;
        ObjectTable CommodityObject;
        ObjectTable ContactDoneMethodObject;
        ObjectTable AdditionalServiceObject;
        ObjectTable QuotePackageObject;
        ObjectTable QuoteTemplateObject;
        ObjectTable QuoteTemplateSectionObject;
        ObjectTable QuoteTemplateSectionTypeObject;
        ObjectTable BorderTypeObject;
        ObjectTable QuoteTemplateSettingObject;
        ObjectTable QuoteTemplateTextCodeObject;
        ObjectTable QuoteTemplateTableDesignObject;
        ObjectTable QuoteTemplateTextDesignObject;
        ObjectTable ExternalSystemsTablesCodeObject;
        ObjectTable VatUniqueTypeObject;
        ObjectTable VatMandatoryTypeObject;
        ObjectTable CustomerSalesNoteObject;
        ObjectTable QuoteClosingReasonObject;
        ObjectTable AccountingSystemsSettingObject;
        ObjectTable AccountingSystemsSyncStatusObject;
        ObjectTable CustomerStatusObject;
        ObjectTable ShipmentCommodityObject;
        ObjectTable CommodityPackageObject;
        ObjectTable BusinessUnitObject;
        ObjectTable EmailAlertsObject;
        ObjectTable FeatureAccessLevelObject;
        ObjectTable SpecialServicesTypeObject;
        ObjectTable RegionObject;
        ObjectTable PaymentCurrencyObject;
        ObjectTable CustomPickListObject;
        ObjectTable LogitudeLeadObject;
        ObjectTable MessagingStockObject;
        ObjectTable MessagingStockUsageHistoryObject;
        ObjectTable CustomerSizeObject;
        ObjectTable QuoteStageObject;
        ObjectTable QuoteRatingObject;
        ObjectTable DistributorObject;
        ObjectTable AutomationObject;
        ObjectTable AccountingInformationIdentifierObject;
        ObjectTable CommunicationLogStepObject;
        ObjectTable ComputingPartnerObjectTable;
        ObjectTable ComputingPartnerCodeObjectTable;
        ObjectTable ComputingPartnerTableObjectTable;
        ObjectTable ComputingPartnerTranslationObjectTable;
        ObjectTable BluesnapContractObject;
        ObjectTable AWBMessagesCCSTypeObject;
        ObjectTable DocumentFoldersObject;
        ObjectTable ManifestStatusObject;
        ObjectTable AWBAdditionalHandlingInfoObject;
        ObjectTable InboundEmailObject;
        ObjectTable InboundEmailLineObject;
        ObjectTable DocumentTypeCategoryObject;
        ObjectTable CustomerTenantAccessObject;
        ObjectTable CustomerTenantAccessStatusTypeObject;
        ObjectTable CustomerTenantAccessCardObject;
        ObjectTable CustomerTenantAccessRequestObject;
        ObjectTable HybridPartnerObject;
        ObjectTable BusinessHourObject;
        ObjectTable BusinessHoursHolidayObject;
        ObjectTable MappedShipmentDirectionsObject;
        ObjectTable APILogsObject;
        ObjectTable APILogsDataObject;
        ObjectTable BatchServicesLogObject;
        ObjectTable BatchServicesDefinitionsObject;
        ObjectTable HybridTenantStateObject;
        ObjectTable TenantTypeObject;
        ObjectTable QueueMessageMoreDetailsObject;
        ObjectTable CustomerTenantAccessCardsBatchObject;
        ObjectTable ParticipantObject;
        ObjectTable AirlineStatisticsObject;
        ObjectTable ApiCredintialsObject;
        ObjectTable AWBDescriptionOfGoodsObject;
        ObjectTable VatFormatTypeObject;
        ObjectTable OceanInsightsStatusesObject;
        ObjectTable OceanInsightsRequestObject;
        ObjectTable HelpResourceObject;
        ObjectTable AutomationsObject;
        ObjectTable AutomationConditionObject;
        ObjectTable AutomationResultEmailRecipientObject;
        ObjectTable LogitudeMessagesTransmissionLogObject;
        ObjectTable FeaturePackageTypeObject;
        ObjectTable TenantManagementLicenseObject;
        ObjectTable PackageConnectedPackageObject;
        ObjectTable UserLicenseObject;
        ObjectTable TenantAddOnObject;
        ObjectTable AirlineMessagingRuleObject;
        ObjectTable TasksSchedulerObject;
        ObjectTable TaskSchedulerHistoryObject;
        ObjectTable PaymentTermDateTypeObject;
        ObjectTable OtherParticipantIdObject;
        ObjectTable ARInvoiceLineActionObject;
        ObjectTable TenantManagmentPrivateLabelsObject;
        ObjectTable CustomerFieldsUpdateSettingObject;
        ObjectTable CreditLimitSettingObject;
        ObjectTable AgentSharedManifestObject;
        ObjectTable SharedManifestsStatusObject;
        ObjectTable CustomerAccountManagerByProduct;
        ObjectTable CardExternalAccountsByProductObject;
        ObjectTable QuoteTotalVatObject;
        ObjectTable QuoteVATsTotalObject;
        ObjectTable CustomsInterfaceObject;
        ObjectTable CustomsInterfaceSettingObject;
        ObjectTable FTPDetailObject;
        ObjectTable SATInterfaceObject;
        ObjectTable SATInterfaceSettingObject;
        ObjectTable VATTypesGroupObject;
        ObjectTable CustomsTransmissionsStatusObject;
        ObjectTable CardExternalCodeByCurrencyObject;
        ObjectTable FBLStockObject;
        ObjectTable SATPaymentMethodObject;
        ObjectTable ContainerFollowUpObject;
        ObjectTable LoginPolicyObject;
        ObjectTable TenantLoginPolicyObject;
        ObjectTable TwoFactorAuthenticationDeviceObject;
        ObjectTable OBLTypesObject;
        ObjectTable AgentSharedDocumentObject;
        ObjectTable ShipmentAssemblyObject;
        ObjectTable MetodoPagoObject;
        ObjectTable ChargesExternalAccountsByProductObject;
        ObjectTable RegistryDateTypesObject;
        ObjectTable UsoCFDIObject;
        ObjectTable ReportsTemplateObject;
        ObjectTable ReportsTemplatesVersionObject;
        ObjectTable ShipmentCustomsMessageTypesObject;
        ObjectTable ShipmentCustomsTransmissionsObject;
        ObjectTable APPaymentTransferStatusObject;
        ObjectTable SATTransferStatusObject;
        ObjectTable SATInvoiceStatusObject;
        ObjectTable FilingInboxObject;
        ObjectTable FilingInboxAttachmentObject;
        ObjectTable FilingInboxAttachmentLogObject;
        ObjectTable QuoteSettingObject;
        ObjectTable INTTRASettingObject;
        ObjectTable INTTRASettingModeObject;
        ObjectTable INTTRABranchRegisteredCarrierObject;
        ObjectTable DocumentFilingBackupBatchObject;
        ObjectTable DocumentFilingBackupSettingObject;
        ObjectTable TemperatureUnitObject;
        ObjectTable PickUpDeliveryTransportModeObject;
        ObjectTable INTTRADocumentTypeObject;
        ObjectTable DWHSettingObject;
        ObjectTable DWObjectTableObject;
        ObjectTable DWObjectFieldObject;
        ObjectTable ShipmentPackageHarmonizeObject;
        ObjectTable PickUpDeliveryPackageHarmonizeObject;
        ObjectTable DWSubQueryObject;
        #endregion

        #region Create All Object Tables
        ObjectTable TestObjectTable;
        ObjectTable TestObjectTable2;
        private void CreateAllObjectsTables(Dictionary<string, ObjectTable> objectTables, Dictionary<string, TextCode> textCodes)
        {
            if (Testing.General.IsTesting)
            {
                TestObjectTable = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
                {
                    DefaultText = "ObjTbl_Test",
                    ObjectTableName = "ObjTbl_Test",
                    ObjectTablePlural = "ObjTbl_Tests",
                    ObjectTableSingular = "ObjTbl_Test",
                    Tenant = 0,
                    KeyPropertyPath = "Id",
                    IsMain = true,
                    IsClosed = true,
                    NewWizardControlName = "Simplog.ShipmentLib.NewShipmentCommand",
                   // NewWizardCompoTenantManagementnentPath = "./Shipment/Components/NewShipment/NewShipmentComponent",
                    HasCounter = true,
                    IsRestrictable = true,
                }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            }

            #region AccountingSystemTenantManagement
            AccountingSystemObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Accounting System",
                ObjectTableName = "AccountingSystem",
                ObjectTablePlural = "Accounting Systems",
                ObjectTableSingular = "Accounting System",
                DBTableName = "AccountingSystems",
                LookUp1 = "Name",
                Tenant = 0,
                KeyPropertyPath = "Code",
                IsClosed = true,
                CacheOnClient = true,
                IsAutoComplete = true,
                IsMain = true,
                ObjectTableTypeCode = "MD",
                ClientModuleName = "Common",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region AccountingSetting
            AccountingSettingObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Accounting Setting",
                ObjectTableName = "AccountingSetting",
                ObjectTablePlural = "Accounting Settings",
                ObjectTableSingular = "Accounting Setting",
                DBTableName = "AccountingSettings",
                ClientModuleName = "Common",
                Tenant = 0,
                KeyPropertyPath = "Id",
                CacheOnClient = true,
                IsMain = true,
                ObjectTableTypeCode = "MD",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region General

            GeneralObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "General",
                ObjectTableName = "General",
                ObjectTablePlural = "Generals",
                ObjectTableSingular = "General",
                Tenant = 0,
                KeyPropertyPath = "Id",
                IsMain = true,
                ObjectTableTypeCode = "MD",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);

            #endregion

            #region CounterDefinition

            CounterDefinitionObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Counter Definition",
                ObjectTableName = "CounterDefinition",
                ObjectTablePlural = "Counter Definitions",
                ObjectTableSingular = "Counter Definition",
                DBTableName = "CounterDefinitions",
                Tenant = 0,
                KeyPropertyPath = "Id",
                IsMain = true,
                ObjectTableTypeCode = "MD",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);

            #endregion

            #region TenantObject
            TenantObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Tenant",
                ObjectTableName = "Tenant",
                ObjectTablePlural = "Tenants",
                ObjectTableSingular = "Tenant",
                DBTableName = "Tenants",
                Tenant = 0,
                IsMain = true,
                ObjectTableTypeCode = "MD",
                ClientModuleName = "Common",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region TenantManagementObject
            TenantManagementObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Tenant Management",
                ObjectTableName = "TenantManagement",
                ObjectTablePlural = "Tenant Managements",
                ObjectTableSingular = "Tenant Management",
                DBTableName = "TenantManagements",
                Tenant = 0,      
                IsMain = true,
                ObjectTableTypeCode = "MD",
                EnableSecurity = true,
                ClientModuleName = "Infrastructure",
                HasMenuButtons = true,
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region Shipment
            ShipmentObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Shipment",
                ObjectTableName = "Shipment",
                ObjectTablePlural = "Shipments",
                ObjectTableSingular = "Shipment",
                DBTableName = "Shipments",
                Tenant = 0,
                IsNewWizard = true,
                NewWizardControlName = "Simplog.ShipmentLib.NewShipmentCommand",
                //SpotlightDataTemplate = "ShipmentSpotlightDataTemplate",
                KeyPropertyPath = "Id",
                HasCounter = true,
                IsMain = true,
                IsRestrictable = true,
                EnableSecurity = true,
                ObjectTableTypeCode = "BR",
                AllowCustomFields = true,
                MaxNumberOfCustomFields = 40,
                HasDynamicHeader = true,                                
                HasDocuments = true,
                ClientModuleName = "Shipment",
                NewWizardComponentPath = "./Shipment/Components/NewEntity/NewShipmentComponent",
                HasHelper = true,
                HasShortTitle = true,
                HasMenuButtons = true,
                HasFiltersMenu=true,
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region ShipmentPackage
            ShipmentPackagesObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Shipment Package",
                ObjectTableName = "ShipmentPackage",
                ObjectTablePlural = "Shipment Packages",
                ObjectTableSingular = "Shipment Package",
                DBTableName = "ShipmentPackages",
                Tenant = 0,
                KeyPropertyPath = "Id",
                IsMain = false,
                IsComposition = true,
                EnableSecurity = true,
                ObjectTableTypeCode = "BR",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region ShipmentOrderPackageObject
            ShipmentOrderPackageObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Shipment Order Package",
                ObjectTableName = "ShipmentOrderPackage",
                ObjectTablePlural = "Shipment Order Packages",
                ObjectTableSingular = "Shipment Order Package",
                DBTableName = "ShipmentOrderPackages",
                Tenant = 0,
                KeyPropertyPath = "Id",
                IsMain = false,
                IsComposition = true,
                EnableSecurity = true,
                ObjectTableTypeCode = "BR",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region PickUpDeliveryFromToTypeObject
            PickUpDeliveryFromToTypeObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "PickUp Delivery From To Type",
                ObjectTableName = "PickUpDeliveryFromToType",
                ObjectTablePlural = "Pick Up Delivery From To Types",
                ObjectTableSingular = "Pick Up Delivery From To Type",
                DBTableName = "PickUpDeliveryFromToTypes",
                Tenant = 0,
                KeyPropertyPath = "Code",
                IsMain = true,
                ObjectTableTypeCode = "MD",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region ShipmentPickUpDelivery
            ShipmentPickUpDeliveryObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Shipment PickUp Delivery",
                ObjectTableName = "ShipmentPickUpDelivery",
                ObjectTablePlural = "Shipment Pick Up Deliveries",
                ObjectTableSingular = "Shipment Pick Up Delivery",
                DBTableName = "ShipmentPickUpDeliveries",
                Tenant = 0,
                KeyPropertyPath = "Id",
                IsMain = false,
                IsComposition = true,
                EnableSecurity = true,
                ObjectTableTypeCode = "BR",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region ShipmentPickUpDeliveryPackages
            ShipmentPickUpDeliveryPackagesObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Shipment PickUp Delivery Package",
                ObjectTableName = "ShipmentPickUpDeliveryPackage",
                ObjectTablePlural = "Shipment Pick Up Delivery Packages",
                ObjectTableSingular = "Shipment Pick Up Delivery Package",
                DBTableName = "ShipmentPickUpDeliveryPackages",
                Tenant = 0,
                KeyPropertyPath = "Id",
                IsMain = false,
                IsComposition = true,
                EnableSecurity = true,
                ObjectTableTypeCode = "BR",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region InsideShipmentPackage
            InsideShipmentPackagesObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Inside Shipment Package",
                ObjectTableName = "InsideShipmentPackage",
                ObjectTablePlural = "Inside Shipment Packages",
                ObjectTableSingular = "Inside Shipment Package",
                DBTableName = "InsideShipmentPackages",
                Tenant = 0,
                KeyPropertyPath = "Id",
                IsMain = false,
                IsComposition = true,
                EnableSecurity = true,
                ObjectTableTypeCode = "BR",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region ShipmentReceivable
            ShipmentReceivablesObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Shipment Receivable",
                ObjectTableName = "ShipmentReceivable",
                ObjectTablePlural = "Shipment Receivables",
                ObjectTableSingular = "Shipment Receivable",
                DBTableName = "ShipmentReceivables",
                LookUp1 = "Code",
                LookUp2 = "EnglishName",
                Tenant = 0,
                KeyPropertyPath = "Id",
                IsAutoComplete = true,
                IsMain = false,
                IsComposition = true,
                EnableSecurity = true,
                ObjectTableTypeCode = "BR",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region ShipmentReceivableStatusObject
            ShipmentReceivableStatusObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Shipment Receivable Status",
                ObjectTableName = "ShipmentReceivableStatus",
                ObjectTablePlural = "Shipment Receivable Status",
                ObjectTableSingular = "Shipment Receivable Status",
                DBTableName = "ShipmentReceivableStatus",
                Tenant = 0,
                KeyPropertyPath = "Code",
                LookUp1 = "Name",
                CacheOnClient = true,
                IsClosed = true,
                IsAutoComplete = true,
                IsMain = true,
                ObjectTableTypeCode = "MD",
                ClientModuleName = "Shipment",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region ShipmentReceivableLineStatusObject
            ShipmentReceivableLineStatusObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Shipment Receivable Line Status",
                ObjectTableName = "ShipmentReceivableLineStatus",
                ObjectTablePlural = "Shipment Receivable Line Status",
                ObjectTableSingular = "Shipment Receivable Line Status",
                DBTableName = "ShipmentReceivableLineStatus",
                Tenant = 0,
                KeyPropertyPath = "Code",
                CacheOnClient = false,
                IsMain = true,
                IsClosed = true,
                ObjectTableTypeCode = "MD",
                ClientModuleName = "Shipment",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region ShipmentPayable
            ShipmentPayablesObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Shipment Payable",
                ObjectTableName = "ShipmentPayable",
                ObjectTablePlural = "Shipment Payables",
                ObjectTableSingular = "Shipment Payable",
                DBTableName = "ShipmentPayables",
                Tenant = 0,
                KeyPropertyPath = "Id",
                IsMain = false,
                IsComposition = true,
                EnableSecurity = true,
                ObjectTableTypeCode = "BR",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region ShipmentPayableStatusObject
            ShipmentPayableStatusObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Shipment Payable Status",
                ObjectTableName = "ShipmentPayableStatus",
                ObjectTablePlural = "Shipment Payable Status",
                ObjectTableSingular = "Shipment Payable Status",
                DBTableName = "ShipmentPayableStatus",
                Tenant = 0,
                KeyPropertyPath = "Code",
                LookUp1 = "Name",
                CacheOnClient = true,
                IsClosed = true,
                IsAutoComplete = true,
                IsMain = true,
                ObjectTableTypeCode = "MD",
                ClientModuleName = "Shipment",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region ShipmentPayableLineStatusObject
            ShipmentPayableLineStatusObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Shipment Payable Line Status",
                ObjectTableName = "ShipmentPayableLineStatus",
                ObjectTablePlural = "Shipment Payable Line Status",
                ObjectTableSingular = "Shipment Payable Line Status",
                DBTableName = "ShipmentPayableLineStatus",
                Tenant = 0,
                KeyPropertyPath = "Code",
                IsMain = true,
                IsClosed = true,
                ObjectTableTypeCode = "MD",
                ClientModuleName = "Shipment",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region ShipmentCustomerTypeObject
            ShipmentCustomerTypeObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Shipment Customer Type",
                ObjectTableName = "ShipmentCustomerType",
                ObjectTablePlural = "Shipment Customer Types",
                ObjectTableSingular = "Shipment Customer Type",
                DBTableName = "ShipmentCustomerTypes",
                Tenant = 0,
                KeyPropertyPath = "Code",
                LookUp1 = "Name",
                IsClosed = true,
                CacheOnClient = true,
                IsMain = true,
                IsAutoComplete = true,
                ObjectTableTypeCode = "MD",
                ClientModuleName = "Shipment",
                DependencyFilter1 = "ShowInLOV",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region ShipmentAWBPrintOnlyObject
            ShipmentAWBPrintOnlyObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Shipment AWB Print Only",
                ObjectTableName = "ShipmentAWBPrintOnly",
                ObjectTablePlural = "Shipment AWB Print Onlies",
                ObjectTableSingular = "Shipment AWB Print Only",
                DBTableName = "ShipmentAWBPrintOnlies",
                Tenant = 0,
                KeyPropertyPath = "Id",
                IsMain = true,
                IsComposition = true,
                EnableSecurity = true,
                ObjectTableTypeCode = "BR",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region FixedAmount
            FixedAmountObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Fixed Amount",
                ObjectTableName = "FixedAmount",
                ObjectTablePlural = "Fixed Amounts",
                ObjectTableSingular = "Fixed Amount",
                Tenant = 0,
                KeyPropertyPath = "Code",
                LookUp1 = "Name",
                IsClosed = true,
                CacheOnClient = true,
                IsMain = true,
                IsAutoComplete = true,
                ObjectTableTypeCode = "MD",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region Quote
            QuoteObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Quote",
                ObjectTableName = "Quote",
                ObjectTablePlural = "Quotes",
                ObjectTableSingular = "Quote",
                DBTableName = "Quotes",
                Tenant = 0,
                HasCounter = true,
                IsNewWizard = true,
                NewWizardControlName = "Simplog.QuoteLib.NewQuoteCommand",
                KeyPropertyPath = "Id",
                IsMain = true,
                IsRestrictable = true,
                EnableSecurity = true,
                ObjectTableTypeCode = "BR",
                AllowCustomFields = true,
                MaxNumberOfCustomFields = 10,
                HasDocuments = true,
                ClientModuleName = "Quote",
                NewWizardComponentPath = "./Quote/Components/NewEntity/NewQuoteComponent",
                HasCustomFilter = true,
                HasCustomValidator = true,
                HasFiltersMenu = true,
                HasHelper = true,
                HasMenuButtons = true,
                HasShortTitle = true,
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region QuoteType
            QuoteTypeObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Quote Type",
                ObjectTableName = "QuoteType",
                ObjectTablePlural = "Quote Types",
                ObjectTableSingular = "Quote Type",
                DBTableName = "QuoteTypes",
                Tenant = 0,
                KeyPropertyPath = "Code",
                LookUp1 = "Name",
                IsClosed = true,
                CacheOnClient = true,
                IsMain = true,
                IsAutoComplete = true,
                ObjectTableTypeCode = "MD",
                ClientModuleName = "Quote",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region MarkUpType
            MarkUpTypeObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Mark Up Type",
                ObjectTableName = "MarkUpType",
                ObjectTablePlural = "Mark Up Types",
                ObjectTableSingular = "Mark Up Type",
                DBTableName = "MarkUpTypes",
                Tenant = 0,
                KeyPropertyPath = "Code",
                LookUp1 = "Name",
                IsClosed = true,
                CacheOnClient = true,
                IsMain = true,
                IsAutoComplete = true,
                ObjectTableTypeCode = "MD",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region QuoteCharge
            QuoteChargeObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Quote Charge",
                ObjectTableName = "QuoteCharge",
                ObjectTablePlural = "Quote Charges",
                ObjectTableSingular = "Quote Charge",
                DBTableName = "QuoteCharges",
                Tenant = 0,
                KeyPropertyPath = "Id",
                IsComposition = true,
                EnableSecurity = true,
                ObjectTableTypeCode = "BR",
                ClientModuleName = "Quote",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region QuoteSaleCharge
            QuoteSaleChargesObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Quote Sale Charge",
                ObjectTableName = "QuoteSaleCharge",
                ObjectTablePlural = "Quote Sale Charges",
                ObjectTableSingular = "Quote Sale Charge",
                Tenant = 0,
                KeyPropertyPath = "Id",
                ObjectTableTypeCode = "BR",
                ClientModuleName = "Quote",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region QuoteCostCharge
            QuoteCostChargesObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Quote Cost Charge",
                ObjectTableName = "QuoteCostCharge",
                ObjectTablePlural = "Quote Cost Charges",
                ObjectTableSingular = "Quote Cost Charge",
                Tenant = 0,
                KeyPropertyPath = "Id",
                ObjectTableTypeCode = "BR",
                ClientModuleName = "Quote",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region QuotePriceStep
            QuotePriceStepObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Quote Price Steps",
                ObjectTableName = "QuotePriceSteps",
                ObjectTablePlural = "Quotes Price Steps",
                ObjectTableSingular = "Quote Price Steps",
                DBTableName = "QuotePriceSteps",
                Tenant = 0,
                KeyPropertyPath = "Id",
                IsMain = false,
                IsComposition = true,
                EnableSecurity = true,
                ObjectTableTypeCode = "BR",
                ClientModuleName = "Quote",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region QuoteCustomerTypeObject
            QuoteCustomerTypeObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Quote Customer Type",
                ObjectTableName = "QuoteCustomerType",
                ObjectTablePlural = "Quote Customer Types",
                ObjectTableSingular = "Quote Customer Type",
                DBTableName = "QuoteCustomerTypes",
                Tenant = 0,
                KeyPropertyPath = "Code",
                LookUp1 = "Name",
                IsClosed = true,
                CacheOnClient = true,
                IsMain = true,
                IsAutoComplete = true,
                ObjectTableTypeCode = "MD",
                ClientModuleName = "Quote",
                DependencyFilter1 = "ShowInLOV",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region QuoteSalesTotalObject
            QuoteSalesTotalObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Quote Sales Total",
                ObjectTableName = "QuoteSalesTotal",
                ObjectTablePlural = "Quote Sales Total",
                ObjectTableSingular = "Quote Sales Total",
                Tenant = 0,
                KeyPropertyPath = "Code",
                ObjectTableTypeCode = "BR",
                ClientModuleName = "Quote",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region DocumentTypeCustomField
            DocumentTypeCustomFieldObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Document Type Custom Field",
                ObjectTableName = "DocumentTypeCustomField",
                ObjectTablePlural = "Document Type Custom Fields",
                ObjectTableSingular = "Document Type Custom Field",
                DBTableName = "DocumentTypeCustomFields1",
                Tenant = 0,
                IsMain = false,
                IsComposition = true,
                ObjectTableTypeCode = "MD",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region TemplateFormat
            TemplateFormatObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Template Format",
                ObjectTableName = "TemplateFormat",
                ObjectTablePlural = "Template Formats",
                ObjectTableSingular = "Template Format",
                DBTableName = "TemplateFormats",
                IsClosed = true,
                KeyPropertyPath = "Code",
                LookUp1 = "Code",
                LookUp2 = "Name",
                Tenant = 0,
                CacheOnClient = true,
                IsAutoComplete = true,
                IsMain = true,
                IsComposition = true,
                ObjectTableTypeCode = "MD",
                ClientModuleName = "Common",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region ObjectTable
            ObjectTableObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Object Table",
                ObjectTableName = "ObjectTable",
                ObjectTablePlural = "Object Tables",
                ObjectTableSingular = "Object Table",
                DBTableName = "ObjectTables",
                Tenant = 0,
                KeyPropertyPath = "Id",
                ClientModuleName = "Infrastructure",
                IsMain = true,
                LookUp1 = "Name",
                IsAutoComplete = true,

                DependencyFilter1 = "HasDocuments",
                DependencyFilter2 = "AllowedForComputingPartners",
                DependencyFilter3  = "AllowedInQueues",

                ObjectTableTypeCode = "MD",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region EntityStatus
            EntityStatusObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Entity Status",
                ObjectTableName = "EntityStatus",
                ObjectTablePlural = "Entity Status",
                ObjectTableSingular = "Entity Status",
                DBTableName = "EntityStatus",
                Tenant = 0,
                KeyPropertyPath = "Id",
                LookUp1 = "DisplayName",
                CacheOnClient = true,
                IsClosed = false,
                EditableFromAutoCompleteWindow = false,
                DependencyFilter1 = "ObjectTableName",
                IsAutoComplete = true,
                IsMain = true,
                SortingByObjectField = "StatusWeight",
                ObjectTableTypeCode = "MD",
                ClientModuleName = "Infrastructure",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region TraceEvent
            TraceEventObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Trace Event",
                ObjectTableName = "TraceEvent",
                ObjectTablePlural = "Trace Events",
                ObjectTableSingular = "Trace Event",
                DBTableName = "TraceEvents",
                Tenant = 0,
                KeyPropertyPath = "Id",
                IsMain = false,
                ObjectTableTypeCode = "MD",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region RatesTable
            RatesTableObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Rates Table",
                ObjectTableName = "RatesTable",
                ObjectTablePlural = "Rates Tables",
                ObjectTableSingular = "Rates Table",
                DBTableName = "RatesTables",
                Tenant = 0,
                KeyPropertyPath = "Id",
                CacheOnClient = true,
                IsAutoComplete = true,
                IsMain = true,
                AutoCompleteSearchWindow = true,
                ObjectTableTypeCode = "MD",
                ClientModuleName = "Infrastructure",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region ObjectFieldObject
            ObjectFieldObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Object Field",
                ObjectTableName = "ObjectField",
                ObjectTablePlural = "Object Fields",
                ObjectTableSingular = "Object Field",
                DBTableName = "ObjectFields",
                Tenant = 0,
                KeyPropertyPath = "Id",
                HasCounter = true,
                IsMain = true,
                IsRestrictable = true,
                ObjectTableTypeCode = "MD",
                LookUp1 = "FullNameTextCodeDefaultText",
                ClientModuleName = "Infrastructure",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region Address
            AddressObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Address",
                ObjectTableName = "Address",
                ObjectTablePlural = "Addresses",
                ObjectTableSingular = "Address",
                DBTableName = "Addresses",
                Tenant = 0,
                LookUp1 = "Description",
                DependencyFilter1 = "CardId",
                KeyPropertyPath = "Id",
                CacheOnClient = false,
                EditableFromAutoCompleteWindow = false,
                IsAutoComplete = true,
                IsMain = false,
                AutoCompleteSearchWindow = false,
                IsComposition = true,
                ObjectTableTypeCode = "MD",
                ClientModuleName = "Common",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region Agent
            AgentObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Agent",
                ObjectTableName = "Agent",
                ObjectTablePlural = "Agents",
                ObjectTableSingular = "Agent",
                DBTableName = "Agents",
                Tenant = 0,
                KeyPropertyPath = "Id",
                EditableFromAutoCompleteWindow = true,
                IsNewWizard = true,
                NewWizardControlName = "Simplog.FreightLib.NewAgentCommand",
                IsMain = true,
                EnableSecurity = true,
                ObjectTableTypeCode = "MD",
                ClientModuleName = "Common",
                NewWizardComponentPath = "./CommonModules/CommonAgent/Components/NewEntity/NewAgentComponent",
                AllowedForComputingPartners = true,
                CodeField="Code",
                NameField= "EnglishName",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region Airline
            AirlinesObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Airline",
                ObjectTableName = "Airline",
                ObjectTablePlural = "Airlines",
                ObjectTableSingular = "Airline",
                DBTableName = "Airlines",
                Tenant = 0,
                KeyPropertyPath = "Id",
                EditableFromAutoCompleteWindow = true,
                CacheOnClient = true,
                IsMain = true,
                NewWizardControlName = "Simplog.FreightLib.NewAirlineCommand",
                IsNewWizard = true,
                EnableSecurity = true,
                ObjectTableTypeCode = "MD",
                ClientModuleName = "Common",
                NewWizardComponentPath = "./CommonModules/CommonAirline/Components/NewEntity/NewAirlineComponent",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region Branch
            BranchesObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Branch",
                ObjectTableName = "Branch",
                ObjectTablePlural = "Branches",
                ObjectTableSingular = "Branch",
                DBTableName = "Branches",
                Tenant = 0,
                LookUp1 = "EnglishName",
                KeyPropertyPath = "Id",
                CacheOnClient = true,
                EditableFromAutoCompleteWindow = true,
                EnableAddFromLOV = true,
                EnableEditFromLOV = true,
                IsAutoComplete = true,
                IsMain = true,
                AutoCompleteSearchWindow = false,
                EnableSecurity = true,
                ObjectTableTypeCode = "MD",
                ClientModuleName = "Common",
                AllowedForComputingPartners = true,
                CodeField="Code",
                NameField = "EnglishName",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region Card
            CardsObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Card",
                ObjectTableName = "Card",
                ObjectTablePlural = "Cards",
                ObjectTableSingular = "Card",
                DBTableName = "Cards",
                Tenant = 0,
                LookUp1 = "Code",
                LookUp2 = "EnglishName",
                DependencyFilter1 = "PartnerTypeId",
                DependencyFilter2 = "IsCustomer",
                KeyPropertyPath = "Id",
                CacheOnClient = false,
                EditableFromAutoCompleteWindow = true,
                IsAutoComplete = false,
                IsMain = true,
                AutoCompleteSearchWindow = true,
                EnableAddFromLOV = true,
                EnableEditFromLOV = true,
                ObjectTableTypeCode = "MD",
                ClientModuleName = "Common",
                NameField = "EnglishName",
                CodeField = "Code",
                AllowedForComputingPartners = true,
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region Carrier
            CarriersObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Carrier",
                ObjectTableName = "Carrier",
                ObjectTablePlural = "Carriers",
                ObjectTableSingular = "Carrier",
                LookUp1 = "Code",
                LookUp2 = "EnglishName",
                Tenant = 0,
                DependencyFilter1 = "PartnerTypeId",
                KeyPropertyPath = "Id",
                CacheOnClient = true,
                EditableFromAutoCompleteWindow = true,
                EnableAddFromLOV = true,
                EnableEditFromLOV = true,
                IsAutoComplete = false,
                IsMain = true,
                AutoCompleteSearchWindow = true,
                SortingByObjectField = "Code",
                ObjectTableTypeCode = "MD",
                ClientModuleName = "Common",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region ChargesType
            ChargesTypesObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Charges Type",
                ObjectTableName = "ChargesType",
                ObjectTablePlural = "Charges Types",
                ObjectTableSingular = "Charges Type",
                DBTableName = "ChargesTypes",
                KeyPropertyPath = "Id",
                LookUp1 = "Code",
                LookUp2 = "EnglishName",
                Tenant = 0,
                CacheOnClient = true,
                EditableFromAutoCompleteWindow = true,
                EnableAddFromLOV = true,
                EnableEditFromLOV = true,
                IsMain = true,
                IsAutoComplete = true,
                IsNewWizard = true,
                NewWizardControlName = "Simplog.FreightLib.Views.ChargesTypes.ChargesTypeWizard.NewChargesTypeControlCommand",
                AutoCompleteSearchWindow = true,
                EnableSecurity = true,
                ObjectTableTypeCode = "MD",
                NewWizardComponentPath = "./Common/Components/Maintenance/ChargesType/NewChargesTypeComponent",
                ClientModuleName = "Common",
                AllowedForComputingPartners = true,
                CodeField="Code",
                NameField = "EnglishName",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region ChargeTypeAccounting
            ChargeTypeAccountingObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Charge Type Accounting",
                ObjectTableName = "ChargeTypeAccounting",
                ObjectTablePlural = "Charge Type Accountings",
                ObjectTableSingular = "Charge Type Accounting",
                DBTableName = "ChargeTypeAccountings",
                Tenant = 0,
                KeyPropertyPath = "Id",
                CacheOnClient = true,
                IsMain = true,
                IsComposition = true,
                ObjectTableTypeCode = "MD",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region ChargesGroup
            ChargesGroupObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Charges Group",
                ObjectTableName = "ChargesGroup",
                ObjectTablePlural = "Charges Groups",
                ObjectTableSingular = "Charges Group",
                DBTableName = "ChargesGroups",
                KeyPropertyPath = "Id",
                LookUp1 = "Code",
                LookUp2 = "Name",
                EnableSecurity = true,
                Tenant = 0,
                CacheOnClient = true,
                IsMain = true,
                IsAutoComplete = true,
                AutoCompleteSearchWindow = true,
                ObjectTableTypeCode = "MD",
                ClientModuleName = "Infrastructure",
                AllowedForComputingPartners = true,
                CodeField="Code",
                NameField = "Name",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region IATACode
            IATACodeObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "IATA Code",
                ObjectTableName = "IATACode",
                ObjectTablePlural = "IATA Codes",
                ObjectTableSingular = "IATA Code",
                DBTableName = "IATACodes",
                LookUp1 = "Code",
                KeyPropertyPath = "Id",
                Tenant = 0,
                IsClosed = false,
                CacheOnClient = true,
                IsAutoComplete = true,
                IsMain = true,
                ObjectTableTypeCode = "MD",
                DependencyFilter1 = "AirlineId",
                ClientModuleName = "Infrastructure",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region Customer
            CustomersObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Customer",
                ObjectTableName = "Customer",
                ObjectTablePlural = "Customers",
                ObjectTableSingular = "Customer",
                DBTableName = "Customers",
                Tenant = 0,
                KeyPropertyPath = "Id",
                NewWizardControlName = "Simplog.FreightLib.NewCustomerCommand",
                IsNewWizard = true,
                LookUp1 = "Code",
                LookUp2 = "EnglishName",
                IsMain = true,
                EnableSecurity = true,
                ObjectTableTypeCode = "MD",
                AllowCustomFields = true,
                MaxNumberOfCustomFields = 10,
                HasDocuments = true,
                ClientModuleName = "Common",
                NewWizardComponentPath = "./CommonModules/CommonCustomer/Components/NewEntity/NewCustomerComponent",
                HasHelper = true,
                HasShortTitle = true,
                HasMenuButtons = true,
                AllowedForComputingPartners = true,
                CodeField="Code",
                NameField = "EnglishName",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region CustomerAccountManagerByProduct
            CustomerAccountManagerByProduct = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Customer Account Manager By Product",
                ObjectTableName = "CustomerAccountManagerByProduct",
                ObjectTablePlural = "CustomerAccountManagerByProducts",
                ObjectTableSingular = "CustomerAccountManagerByProduct",
                DBTableName = "CustomerAccountManagerByProducts",
                Tenant = 0,
                KeyPropertyPath = "Id",
                LookUp1 = "ProductTypeCode",
                LookUp2 = "EnglishName",
                IsMain = true,
                EnableSecurity = true,
                ObjectTableTypeCode = "MD",
                AllowCustomFields = true,
                MaxNumberOfCustomFields = 10,
                HasDocuments = true,
                ClientModuleName = "Common",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region Contact
            ContactsObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Contact",
                ObjectTableName = "Contact",
                ObjectTablePlural = "Contacts",
                ObjectTableSingular = "Contact",
                DBTableName = "Contacts",
                Tenant = 0,
                LookUp1 = "EnglishName",
                KeyPropertyPath = "Id",
                CacheOnClient = false,
                IsNewWizard = true,
                NewWizardControlName = "Simplog.FreightLib.NewContactCommand",
                NewWizardComponentPath = "./CommonModules/CommonPartners/Components/NewEntity/NewContactComponent",
                EditableFromAutoCompleteWindow = false,
                DependencyFilter1 = "CardId",
                DependencyFilter2 = "ContactIdCustomFilter",
                IsClosed = false,
                IsMain = true,
                AutoCompleteSearchWindow = false,
                IsAutoComplete = true,
                EnableAddFromLOV = false,
                EnableEditFromLOV = false,
                EnableSecurity = true,
                ObjectTableTypeCode = "MD",
                ClientModuleName = "Common",

                HasMenuButtons = true,
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region Country
            CountriesObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Country",
                ObjectTableName = "Country",
                ObjectTablePlural = "Countries",
                ObjectTableSingular = "Country",
                DBTableName = "Countries",
                LocalDefaultText = "מדינה",
                Tenant = 0,
                LookUp1 = "Code",
                LookUp2 = "EnglishName",
                KeyPropertyPath = "Id",
                CacheOnClient = true,
                EditableFromAutoCompleteWindow = true,
                EnableAddFromLOV = true,
                EnableEditFromLOV = true,
                IsMain = true,
                IsAutoComplete = true,
                AutoCompleteSearchWindow = true,
                EnableSecurity = true,
                ObjectTableTypeCode = "MD",
                ClientModuleName = "Common",
                AllowedForComputingPartners = true,
                CodeField = "Code",
                NameField = "EnglishName",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region Currency
            CurrenciesObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Currency",
                ObjectTableName = "Currency",
                ObjectTablePlural = "Currencies",
                ObjectTableSingular = "Currency",
                DBTableName = "Currencies",
                LookUp1 = "Code",
                Tenant = 0,
                KeyPropertyPath = "Id",
                CacheOnClient = true,
                EditableFromAutoCompleteWindow = true,
                EnableAddFromLOV = true,
                EnableEditFromLOV = true,
                IsMain = true,
                IsAutoComplete = true,
                AutoCompleteSearchWindow = false,
                EnableSecurity = true,
                ObjectTableTypeCode = "MD",
                IsNewWizard = true,
                NewWizardControlName = "Simplog.FreightLib.NewCurrencyCommand",
                NewWizardComponentPath = "./Common/Components/Maintenance/Currency/NewCurrencyComponent",
                ClientModuleName = "Common",
                AllowedForComputingPartners = true,
                CodeField="Code",
                NameField = "EnglishName",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region CustomAgent
            CustomAgentsObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Customs Agent",
                ObjectTableName = "CustomAgent",
                ObjectTablePlural = "Customs Agents",
                ObjectTableSingular = "Customs Agent",
                DBTableName = "CustomAgents",
                Tenant = 0,
                KeyPropertyPath = "Id",
                EditableFromAutoCompleteWindow = true,
                IsNewWizard = true,
                NewWizardControlName = "Simplog.FreightLib.NewCustomAgentCommand",
                IsMain = true,
                EnableSecurity = true,
                ObjectTableTypeCode = "MD",
                ClientModuleName = "Common",
                NewWizardComponentPath = "./CommonModules/CommonPartners/Components/NewEntity/NewCustomAgentComponent",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region Department
            DepartmentsObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Department",
                ObjectTableName = "Department",
                ObjectTablePlural = "Departments",
                ObjectTableSingular = "Department",
                DBTableName = "Departments",
                Tenant = 0,
                LookUp1 = "EnglishName",
                KeyPropertyPath = "Id",
                CacheOnClient = true,
                EditableFromAutoCompleteWindow = true,
                EnableAddFromLOV = true,
                EnableEditFromLOV = true,
                IsMain = true,
                IsAutoComplete = true,
                AutoCompleteSearchWindow = false,
                EnableSecurity = true,
                ObjectTableTypeCode = "MD",
                ClientModuleName = "Common",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region Direction
            DirectionsObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Direction",
                ObjectTableName = "Direction",
                ObjectTablePlural = "Directions",
                ObjectTableSingular = "Direction",
                DBTableName = "Directions",
                LookUp1 = "Name",
                Tenant = 0,
                KeyPropertyPath = "Id",
                IsClosed = true,
                CacheOnClient = true,
                IsMain = true,
                IsAutoComplete = true,
                ObjectTableTypeCode = "MD",
                ClientModuleName = "Infrastructure",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region ExternalDocument
            ExternalDocumentsObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Docs In",
                ObjectTableName = "DocsIn",
                ObjectTablePlural = "Docs Ins",
                ObjectTableSingular = "Docs In",
                DBTableName = "DocumentIns",
                Tenant = 0,
                KeyPropertyPath = "Id",
                IsMain = true,
                IsComposition = true,
                EnableSecurity = true,
                ObjectTableTypeCode = "MD",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);



            #endregion

            #region ExternalDocument
            DocumentsFilingObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Document Filing",
                ObjectTableName = "DocumentsFiling",
                ObjectTablePlural = "Documents Filings",
                ObjectTableSingular = "Document Filing",
                DBTableName = "DocumentsFiling",
                Tenant = 0,
                KeyPropertyPath = "Id",
                IsMain = true,
                HasCounter = true,
                EnableSecurity = true,
                HasShortTitle = true,
                ObjectTableTypeCode = "MD",
                ClientModuleName = "Common",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region InternalDocument
            InternalDocumentsObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Docs Out",
                ObjectTableName = "DocsOut",
                ObjectTablePlural = "Docs Outs",
                ObjectTableSingular = "Docs Out",
                DBTableName = "DocumentOuts",
                Tenant = 0,
                KeyPropertyPath = "Id",
                IsMain = true,
                IsComposition = true,
                EnableSecurity = true,
                ObjectTableTypeCode = "MD",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region FollowUp
            FollowUpsObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Follow Up",
                ObjectTableName = "FollowUp",
                ObjectTablePlural = "Follow Ups",
                ObjectTableSingular = "Follow Up",
                DBTableName = "FollowUps",
                Tenant = 0,
                KeyPropertyPath = "Id",
                IsMain = false,
                IsComposition = true,
                EnableSecurity = true,
                ObjectTableTypeCode = "MD",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region GlobalZone
            GlobalZonesObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Global Zone",
                ObjectTableName = "GlobalZone",
                ObjectTablePlural = "Global Zones",
                ObjectTableSingular = "Global Zone",
                DBTableName = "GlobalZones",
                LookUp1 = "EnglishName",
                Tenant = 0,
                KeyPropertyPath = "Id",
                CacheOnClient = true,
                EditableFromAutoCompleteWindow = true,
                EnableAddFromLOV = true,
                EnableEditFromLOV = true,
                IsMain = true,
                IsAutoComplete = true,
                AutoCompleteSearchWindow = true,
                EnableSecurity = true,
                ObjectTableTypeCode = "MD",
                ClientModuleName = "Common",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region Master
            MasterObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Master",
                ObjectTableName = "Master",
                ObjectTablePlural = "Masters",
                ObjectTableSingular = "Master",
                DBTableName = "Masters",
                Tenant = 0,
                IsNewWizard = true,
                NewWizardControlName = "Simplog.ShipmentLib.NewMasterCommand",
                KeyPropertyPath = "Id",
                HasCounter = true,
                IsMain = true,
                IsRestrictable = true,
                EnableSecurity = true,
                ObjectTableTypeCode = "BR",
                AllowCustomFields = true,
                MaxNumberOfCustomFields = 10,
                HasDocuments = true,
                ClientModuleName = "Shipment",
                NewWizardComponentPath = "./Shipment/Components/NewEntity/NewMasterComponent",
                HasHelper = true,
                HasShortTitle = true,
                HasMenuButtons = true,
                HasFiltersMenu = true,
               
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);

            #endregion

            #region PaymentTerm
            PaymentTermsObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Payment Term",
                ObjectTableName = "PaymentTerm",
                ObjectTablePlural = "Payment Terms",
                ObjectTableSingular = "Payment Term",
                DBTableName = "PaymentTerms",
                LookUp1 = "EnglishName",
                Tenant = 0,
                KeyPropertyPath = "Id",
                CacheOnClient = true,
                EditableFromAutoCompleteWindow = true,
                DependencyFilter1 = "DisplayInLOV",

                EnableAddFromLOV = true,
                EnableEditFromLOV = true,
                IsMain = true,
                IsAutoComplete = true,
                AutoCompleteSearchWindow = false,
                EnableSecurity = true,
                ObjectTableTypeCode = "MD",
                ClientModuleName = "Common",
                AllowedForComputingPartners = true,
                CodeField="Code",
                NameField = "EnglishName",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region Port
            PortsObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Port",
                ObjectTableName = "Port",
                ObjectTablePlural = "Ports",
                ObjectTableSingular = "Port",
                DBTableName = "Ports",
                LookUp1 = "Code",
                LookUp2 = "EnglishName",
                Tenant = 0,
                KeyPropertyPath = "Id",
                CacheOnClient = true,
                EditableFromAutoCompleteWindow = true,
                EnableAddFromLOV = true,
                EnableEditFromLOV = true,
                IsMain = true,
                DependencyFilter1 = "TransportModeId",
                AutoCompleteSearchWindow = true,
                SortingByObjectField = "Code",
                EnableSecurity = true,
                ObjectTableTypeCode = "MD",
                ClientModuleName = "Common",
                AllowedForComputingPartners=true,
                CodeField="CombinedCode",
                NameField = "EnglishName",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region ShippingAgent
            ShippingAgentsObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Shipping Agent",
                ObjectTableName = "ShippingAgent",
                ObjectTablePlural = "Shipping Agents",
                ObjectTableSingular = "Shipping Agent",
                DBTableName = "ShippingAgents",
                Tenant = 0,
                KeyPropertyPath = "Id",
                EditableFromAutoCompleteWindow = true,
                IsNewWizard = true,
                NewWizardControlName = "Simplog.FreightLib.NewShippingAgentCommand",
                IsMain = true,
                EnableSecurity = true,
                ObjectTableTypeCode = "MD",
                ClientModuleName = "Common",
                NewWizardComponentPath = "./CommonModules/CommonPartners/Components/NewEntity/NewShippingAgentComponent",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region ShippingLine
            ShippingLinesObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Shipping Line",
                ObjectTableName = "ShippingLine",
                ObjectTablePlural = "Shipping Lines",
                ObjectTableSingular = "Shipping Line",
                DBTableName = "ShippingLines",
                Tenant = 0,
                KeyPropertyPath = "Id",
                EditableFromAutoCompleteWindow = true,
                CacheOnClient = true,
                IsMain = true,
                NewWizardControlName = "Simplog.FreightLib.NewShippingLineCommand",
                IsNewWizard = true,
                EnableSecurity = true,
                ObjectTableTypeCode = "MD",
                NewWizardComponentPath = "./CommonModules/CommonPartners/Components/NewEntity/NewShippingLineComponent",
                ClientModuleName = "Common",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region SpecialService
            SpecialServicesObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Special Service",
                ObjectTableName = "SpecialService",
                ObjectTablePlural = "Special Services",
                ObjectTableSingular = "Special Service",
                DBTableName = "SpecialServices",
                Tenant = 0,
                KeyPropertyPath = "Id",
                IsMain = true,
                EnableSecurity = false,
                ObjectTableTypeCode = "MD",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region State
            StatesObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "State",
                ObjectTableName = "State",
                ObjectTablePlural = "States",
                ObjectTableSingular = "State",
                DBTableName = "States",
                Tenant = 0,
                LookUp1 = "Code",
                LookUp2 = "EnglishName",
                KeyPropertyPath = "Id",
                CacheOnClient = true,
                EditableFromAutoCompleteWindow = true,
                DependencyFilter1 = "CountryId",
                EnableAddFromLOV = true,
                EnableEditFromLOV = true,
                IsMain = true,
                IsAutoComplete = true,
                AutoCompleteSearchWindow = true,
                EnableSecurity = true,
                ObjectTableTypeCode = "MD",
                ClientModuleName = "Common",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region City
            CountryCityObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "City",
                ObjectTableName = "CountryCity",
                ObjectTablePlural = "Cities",
                ObjectTableSingular = "City",
                DBTableName = "CountryCities",
                Tenant = 0,
                LocalDefaultText = "עיר",
                LookUp1 = "EnglishName",
                KeyPropertyPath = "Id",
                CacheOnClient = true,
                EditableFromAutoCompleteWindow = true,
                DependencyFilter1 = "CountryId",
                EnableAddFromLOV = true,
                EnableEditFromLOV = true,
                IsMain = true,
                IsAutoComplete = true,
                AutoCompleteSearchWindow = true,
                EnableSecurity = true,
                ObjectTableTypeCode = "MD",
                ClientModuleName = "Common",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region TransportMode
            TransportModesObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Transport Mode",
                ObjectTableName = "TransportMode",
                ObjectTablePlural = "Transport Modes",
                ObjectTableSingular = "Transport Mode",
                DBTableName = "TransportModes",
                LookUp1 = "Name",
                Tenant = 0,
                IsClosed = true,
                KeyPropertyPath = "Id",
                CacheOnClient = true,
                IsMain = true,
                IsAutoComplete = true,
                ObjectTableTypeCode = "MD",
                ClientModuleName = "Infrastructure",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region Trucker
            TruckersObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Trucker",
                ObjectTableName = "Trucker",
                ObjectTablePlural = "Truckers",
                ObjectTableSingular = "Trucker",
                DBTableName = "Truckers",
                Tenant = 0,
                KeyPropertyPath = "Id",
                EditableFromAutoCompleteWindow = true,
                CacheOnClient = true,
                IsMain = true,
                IsNewWizard = true,
                NewWizardControlName = "Simplog.FreightLib.NewTruckerCommand",
                EnableSecurity = true,
                ObjectTableTypeCode = "MD",
                ClientModuleName = "Common",
                NewWizardComponentPath = "./CommonModules/CommonPartners/Components/NewEntity/NewTruckerComponent",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region Measurement
            MeasurementsObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Measurement",
                ObjectTableName = "Measurement",
                ObjectTablePlural = "Measurements",
                ObjectTableSingular = "Measurement",
                DBTableName = "Measurements",
                LookUp1 = "Code",
                LookUp2 = "Name",
                Tenant = 0,
                KeyPropertyPath = "Id",
                DependencyFilter1 = "IsContainerMeasurement",
                DependencyFilter2 = "IsContainer",
                CacheOnClient = true,
                EditableFromAutoCompleteWindow = true,
                IsMain = true,
                IsAutoComplete = true,
                EnableSecurity = true,
                ObjectTableTypeCode = "MD",
                ClientModuleName = "Common",
                AllowedForComputingPartners = true,
                CodeField="Code",
                NameField = "Name",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region User
            UsersObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "User",
                ObjectTableName = "User",
                ObjectTablePlural = "Users",
                ObjectTableSingular = "User",
                DBTableName = "Users",
                LookUp1 = "EnglishName",
                Tenant = 0,
                KeyPropertyPath = "Id",
                CacheOnClient = true,
                EditableFromAutoCompleteWindow = true,
                EnableAddFromLOV = true,
                EnableEditFromLOV = true,
                IsMain = true,
                IsAutoComplete = true,
                AutoCompleteSearchWindow = false,
                IsNewWizard = true,
                NewWizardControlName = "Simplog.Infrastructure.NewUserCommand",
                NewWizardComponentPath = "./InfrastructureModules/InfrastructureUser/Components/NewUserComponent",
                EnableSecurity = true,
                ObjectTableTypeCode = "MD",
                DependencyFilter1 = "BusinessUnitId",
                DependencyFilter2 = "EmployeeGroupCustomFilter",
                DependencyFilter3 = "IsSalesman",
                ClientModuleName = "Common",
                HasMenuButtons = true,
                AllowedForComputingPartners=true,
                CodeField= "Email",
                NameField = "EnglishName",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region PartnerType
            PartnerTypesObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Partner Type",
                ObjectTableName = "PartnerType",
                ObjectTablePlural = "Partner Types",
                ObjectTableSingular = "Partner Type",
                DBTableName = "PartnerTypes",
                ClientModuleName = "Common",
                Tenant = 0,
                KeyPropertyPath = "Id",
                CacheOnClient = true,
                LookUp1 = "Id",
                LookUp2 = "Name",
                AutoCompleteSearchWindow = true,
                IsClosed = true,
                IsMain = true,
                IsAutoComplete = true,
                ObjectTableTypeCode = "MD",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region ShipmentType
            ShipmentTypesObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Shipment Type",
                ObjectTableName = "ShipmentType",
                ObjectTablePlural = "Shipment Types",
                ObjectTableSingular = "Shipment Type",
                DBTableName = "ShipmentTypes",
                Tenant = 0,
                LookUp1 = "Name",
                KeyPropertyPath = "Id",
                IsClosed = true,
                CacheOnClient = true,
                DependencyFilter1 = "TransportModeId",
                IsMain = true,
                IsAutoComplete = true,
                ObjectTableTypeCode = "MD",
                ClientModuleName = "Shipment",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region VatType
            VatTypesObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "VAT Type",
                ObjectTableName = "VatType",
                ObjectTablePlural = "VAT Types",
                ObjectTableSingular = "VAT Type",
                DBTableName = "VatTypes",
                LookUp1 = "Code",
                LookUp2 = "EnglishName",
                Tenant = 0,
                KeyPropertyPath = "Id",
                CacheOnClient = true,
                EditableFromAutoCompleteWindow = true,
                EnableAddFromLOV = true,
                EnableEditFromLOV = true,
                IsMain = true,
                IsAutoComplete = true,
                AutoCompleteSearchWindow = false,
                EnableSecurity = true,
                ObjectTableTypeCode = "MD",
                ClientModuleName = "Common",
                DependencyFilter1 = "IsMultiPercentage",
                AllowedForComputingPartners = true,
                CodeField="Code",
                NameField = "EnglishName",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region VatTypePercentage
            VatTypePercentageObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Vat Type Percentage",
                ObjectTableName = "VatTypePercentage",
                ObjectTablePlural = "VAT Type Percentages",
                ObjectTableSingular = "VAT Type Percentage",
                DBTableName = "VatTypePercentages",
                Tenant = 0,
                KeyPropertyPath = "Id",
                CacheOnClient = true,
                IsMain = true,
                IsComposition = true,
                ObjectTableTypeCode = "MD",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region FollowUpType
            FollowUpTypesObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Follow Up Type",
                ObjectTableName = "FollowUpType",
                ObjectTablePlural = "Follow Up Types",
                ObjectTableSingular = "Follow Up Type",
                Tenant = 0,
                KeyPropertyPath = "Id",
                LookUp1 = "Name",
                CacheOnClient = true,
                EditableFromAutoCompleteWindow = true,
                IsAutoComplete = true,
                AutoCompleteSearchWindow = true,
                ObjectTableTypeCode = "MD",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region EventType
            EventTypesObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Event Type",
                ObjectTableName = "EventType",
                ObjectTablePlural = "Event Types",
                ObjectTableSingular = "Event Type",
                Tenant = 0,
                KeyPropertyPath = "Id",
                LookUp1 = "Code",
                LookUp2 = "EnglishName",
                CacheOnClient = true,
                DependencyFilter1 = "IsManualEntry",
                DependencyFilter2 = "ObjectTableId",
                EditableFromAutoCompleteWindow = true,
                IsMain = true,
                IsAutoComplete = true,
                AutoCompleteSearchWindow = true,
                ObjectTableTypeCode = "MD",
                EnableSecurity = true,
                ClientModuleName = "Infrastructure",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region ContainerType
            ContainerTypesObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Container Type",
                ObjectTableName = "ContainerType",
                ObjectTablePlural = "Container Types",
                ObjectTableSingular = "Container Type",
                LookUp1 = "Code",
                LookUp2 = "EnglishName",
                Tenant = 0,
                KeyPropertyPath = "Id",
                IsClosed = true,
                CacheOnClient = true,
                IsMain = true,
                ObjectTableTypeCode = "MD",
                IsAutoComplete = true,
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region PackageType
            PackageTypesObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Package Type",
                ObjectTableName = "PackageType",
                ObjectTablePlural = "Package Types",
                ObjectTableSingular = "Package Type",
                DBTableName = "PackageTypes",
                DependencyFilter1 = "TransportModeId",
                DependencyFilter2 = "IsContainer",
                LookUp1 = "Code",
                LookUp2 = "EnglishName",
                Tenant = 0,
                KeyPropertyPath = "Id",
                CacheOnClient = true,
                EditableFromAutoCompleteWindow = true,
                IsClosed = false,
                EnableEditFromLOV = true,
                EnableAddFromLOV = true,
                IsMain = true,
                //AutoCompleteSearchWindow = true,
                EnableSecurity = true,
                ObjectTableTypeCode = "MD",
                ClientModuleName = "Common",
                AllowedForComputingPartners = true,
                CodeField="Code",
                NameField = "EnglishName",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region DocumentType
            DocumentTypesObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Document Type",
                ObjectTableName = "DocumentType",
                ObjectTablePlural = "Document Types",
                ObjectTableSingular = "Document Type",
                DBTableName = "DocumentTypes",
                Tenant = 0,
                KeyPropertyPath = "Id",
                CacheOnClient = true,
                EditableFromAutoCompleteWindow = true,
                IsMain = true,
                ObjectTableTypeCode = "MD",
                EnableSecurity = true,
                LookUp1 = "Code",
                LookUp2 = "Name",
                IsNewWizard = true,
                NewWizardControlName = "Simplog.Infrastructure.Views.Documents.DocumentTypes.NewDocumentTypeCommand",
                NewWizardComponentPath = "./InfrastructureModules/InfrastructureDocuments/Components/DocumentType/NewDocumentTypeComponent",
                DependencyFilter1 = "ObjectTableId",
                DependencyFilter2 = "IsDocIn",
                ClientModuleName = "Common",

            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region DocumentTypeTemplate

            DocumentTypeTemplateObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Document Type Template",
                ObjectTableName = "DocumentTypeTemplate",
                ObjectTablePlural = "Document Type Templates",
                ObjectTableSingular = "Document Type Template",
                DBTableName = "DocumentTypeTemplates",
                Tenant = 0,
                KeyPropertyPath = "Id",
                CacheOnClient = false,
                EditableFromAutoCompleteWindow = true,
                IsMain = true,
                IsAutoComplete = true,
                AutoCompleteSearchWindow = true,
                EnableSecurity = true,

                ObjectTableTypeCode = "MD",
                ClientModuleName = "Common",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region HybridTenantThreshold

            DocumentTypeTemplateObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Hybrid Tenant Threshold",
                ObjectTableName = "HybridTenantThreshold",
                ObjectTablePlural = "Hybrid Tenant Threshold ",
                ObjectTableSingular = "Hybrid Tenant Threshold",
                DBTableName = "HybridTenantThresholds",
                Tenant = 0,
                KeyPropertyPath = "Id",
                CacheOnClient = false,
                EditableFromAutoCompleteWindow = true,
                IsMain = true,
                IsAutoComplete = true,
                AutoCompleteSearchWindow = true,
                EnableSecurity = true,

                ObjectTableTypeCode = "MD",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);





            #endregion

            #region EntityDate
            EntityDateObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Entity Date",
                ObjectTableName = "EntityDate",
                ObjectTablePlural = "Entity Dates",
                ObjectTableSingular = "Entity Date",
                DBTableName = "EntityDates",
                Tenant = 0,
                KeyPropertyPath = "Id",
                IsMain = true,
                IsClosed = true,
                ObjectTableTypeCode = "MD",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region PrepaidCollect
            PrepaidCollectObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Freight",
                ObjectTableName = "PrepaidCollect",
                ObjectTablePlural = "Prepaid Collects",
                ObjectTableSingular = "Prepaid Collect",
                DBTableName = "PrepaidCollects",
                LookUp1 = "Name",
                Tenant = 0,
                KeyPropertyPath = "Id",
                IsClosed = true,
                CacheOnClient = true,
                IsAutoComplete = true,
                DependencyFilter1 = "DisplayInLOV",
                IsMain = true,
                ObjectTableTypeCode = "MD",
                ClientModuleName = "Infrastructure",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region WeightUnit
            WeightUnitObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Weight Unit",
                ObjectTableName = "WeightUnit",
                ObjectTablePlural = "Weight Units",
                ObjectTableSingular = "Weight Unit",
                DBTableName = "WeightUnits",
                LookUp1 = "Name",
                Tenant = 0,
                KeyPropertyPath = "Code",
                IsClosed = true,
                CacheOnClient = true,
                IsAutoComplete = true,
                IsMain = true,
                ObjectTableTypeCode = "MD",
                ClientModuleName = "Common",
                AllowedForComputingPartners = true,
                CodeField="Code",
                NameField = "Name",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region DueType
            DueTypeObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Due Type",
                ObjectTableName = "DueType",
                ObjectTablePlural = "Due Types",
                ObjectTableSingular = "Due Type",
                DBTableName = "DueTypes",
                LookUp1 = "Name",
                Tenant = 0,
                KeyPropertyPath = "Code",
                IsClosed = true,
                CacheOnClient = true,
                IsAutoComplete = true,
                IsMain = true,
                ObjectTableTypeCode = "MD",
                ClientModuleName = "Common",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region RateClass
            RateClassObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Rate Class",
                ObjectTableName = "RateClass",
                ObjectTablePlural = "Rate Classes",
                ObjectTableSingular = "Rate Class",
                DBTableName = "RateClasses",
                LookUp1 = "Code",
                LookUp2 = "Name",
                Tenant = 0,
                KeyPropertyPath = "Code",
                CacheOnClient = true,
                IsClosed = true,
                EditableFromAutoCompleteWindow = true,
                IsAutoComplete = true,
                IsMain = true,
                ObjectTableTypeCode = "MD",
                ClientModuleName = "Common",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region DimensionsUnit
            DimensionsUnitObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Dimensions Unit",
                ObjectTableName = "DimensionsUnit",
                ObjectTablePlural = "Dimensions Units",
                ObjectTableSingular = "Dimensions Unit",
                DBTableName = "DimensionsUnits",
                LookUp1 = "Name",
                Tenant = 0,
                KeyPropertyPath = "Code",
                IsClosed = true,
                CacheOnClient = true,
                IsAutoComplete = true,
                IsMain = true,
                ObjectTableTypeCode = "MD",
                ClientModuleName = "Common",
                DependencyFilter1 = "Code",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region AWBChargesCode
            AWBChargesCodeObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "AWB Charges Code",
                ObjectTableName = "AWBChargesCode",
                ObjectTablePlural = "AWB Charges Codes",
                ObjectTableSingular = "AWB Charges Code",
                DBTableName = "AWBChargesCodes",
                LookUp1 = "Code",
                Tenant = 0,
                IsClosed = true,
                KeyPropertyPath = "Code",
                IsAutoComplete = true,
                AutoCompleteSearchWindow = true,
                CacheOnClient = true,
                IsMain = true,
                ObjectTableTypeCode = "MD",
                ClientModuleName = "Shipment",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region AWBSpecialHandlingCode
            AWBSpecialHandlingCodeObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "AWB Special Handling Code",
                ObjectTableName = "AWBSpecialHandlingCode",
                ObjectTablePlural = "AWB Special Handling Codes",
                ObjectTableSingular = "AWB Special Handling Code",
                DBTableName = "AWBSpecialHandlingCodes",
                LookUp1 = "Code",
                Tenant = 0,
                IsClosed = false,
                KeyPropertyPath = "Id",
                IsAutoComplete = true,
                AutoCompleteSearchWindow = true,
                CacheOnClient = true,
                IsMain = true,
                ObjectTableTypeCode = "MD",
                DependencyFilter1 = "AirlineId",
                ClientModuleName = "Shipment",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region VolumeUnit
            VolumeUnitObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Volume Unit",
                ObjectTableName = "VolumeUnit",
                ObjectTablePlural = "Volume Units",
                ObjectTableSingular = "Volume Unit",
                DBTableName = "VolumeUnits",
                LookUp1 = "Name",
                Tenant = 0,
                IsClosed = true,
                KeyPropertyPath = "Code",
                CacheOnClient = true,
                IsAutoComplete = true,
                IsMain = true,
                ObjectTableTypeCode = "MD",
                ClientModuleName = "Infrastructure",
                AllowedForComputingPartners = true,
                CodeField="Code",
                NameField = "Name",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region Vessel
            VesselObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Vessel",
                ObjectTableName = "Vessel",
                ObjectTablePlural = "Vessels",
                ObjectTableSingular = "Vessel",
                DBTableName = "Vessels",
                LookUp1 = "Code",
                LookUp2 = "EnglishName",
                Tenant = 0,
                KeyPropertyPath = "Id",
                CacheOnClient = true,
                EditableFromAutoCompleteWindow = true,
                EnableAddFromLOV = true,
                EnableEditFromLOV = true,
                IsMain = true,
                IsAutoComplete = true,
                AutoCompleteSearchWindow = true,
                EnableSecurity = true,
                ObjectTableTypeCode = "MD",
                ClientModuleName = "Common",
                AllowedForComputingPartners = true,
                CodeField="Vessel",
                NameField = "EnglishName",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region CustomerTenantAccess
            CustomerTenantAccessObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Customer Tenant Access",
                ObjectTableName = "CustomerTenantAccess",
                ObjectTablePlural = "Customer Tenant Accesses",
                ObjectTableSingular = "Customer Tenant Access",
                DBTableName = "CustomerTenantAccesses",
                Tenant = 0,
                KeyPropertyPath = "Id",
                IsMain = true,
                EnableSecurity = true,
                ObjectTableTypeCode = "MD",
                IsSaveButtonVisible = false,
                ClientModuleName = "Common",
                HasMenuButtons = true,
                HasFiltersMenu = true,
                HasCustomFilter = true,
                //IsNewWizard = true,
                // NewWizardControlName = "Simplog.Infrastructure.Vie.DocumentTypes.NewDocumentTypeCommand",

            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region CustomerTenantAccessStatusType
            CustomerTenantAccessStatusTypeObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Customer Tenant Access Status Type",
                ObjectTableName = "CustomerTenantAccessStatusType",
                ObjectTablePlural = "Customer Tenant Access Status Types",
                ObjectTableSingular = "Customer Tenant Access Status Type",
                DBTableName = "CustomerTenantAccessStatusTypes",
                LookUp1 = "Code",
                LookUp2 = "EnglishName",
                Tenant = 0,
                KeyPropertyPath = "Code",
                CacheOnClient = true,
                IsClosed = true,
                EditableFromAutoCompleteWindow = true,
                IsAutoComplete = true,               
                ObjectTableTypeCode = "MD",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region CustomerTenantAccessCard
            CustomerTenantAccessCardObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Customer Tenant Access Card",
                ObjectTableName = "CustomerTenantAccessCard",
                ObjectTablePlural = "Customer Tenant Access Cards",
                ObjectTableSingular = "Customer Tenant Access Card",
                DBTableName = "CustomerTenantAccessCards",            
                Tenant = 0,
                KeyPropertyPath = "Id",             
                IsMain = false,
                IsComposition = true,
                EnableSecurity = true,
                ObjectTableTypeCode = "BR",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region CustomerTenantAccessRequest
            CustomerTenantAccessRequestObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Customer Tenant Access Request",
                ObjectTableName = "CustomerTenantAccessRequest",
                ObjectTablePlural = "Request data from Agents",
                ObjectTableSingular = "Customer Tenant Access Request",
                DBTableName = "CustomerTenantAccessRequests",
                Tenant = 0,
                KeyPropertyPath = "Id",
                IsMain=true,            
                EnableSecurity = true,
                ObjectTableTypeCode = "MD",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region HybridPartnerObject
            HybridPartnerObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Hybrid Partner",
                ObjectTableName = "HybridPartner",
                ObjectTablePlural = "Hybrid Partners",
                ObjectTableSingular = "Hybrid Partner",
                DBTableName = "HybridPartners",
                Tenant = 0,
                KeyPropertyPath = "Id",
                LookUp1 = "Name",
                LookUp2="LocalName",
                IsMain = true,           
                EnableSecurity = true,
                ObjectTableTypeCode = "MD",
                CacheOnClient = true,
                IsAutoComplete = true,
                AutoCompleteSearchWindow = false,
                IsNewWizard = true,
                NewWizardControlName = "Simplog.Infrastructure.Views.HybridPartner.NewHybridPartnerCommand",
                NewWizardComponentPath = "./InfrastructureModules/InfrastructureHybrid/Components/HypridPartner/NewHybridPartnerComponent",
                ClientModuleName = "Common",

            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region WareHouse
            WareHouseObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Warehouse",
                ObjectTableName = "Warehouse",
                ObjectTablePlural = "Warehouses",
                ObjectTableSingular = "Warehouse",
                DBTableName = "Warehouses",
                LookUp1 = "Code",
                LookUp2 = "EnglishName",
                Tenant = 0,
                KeyPropertyPath = "Id",
                CacheOnClient = true,
                IsNewWizard = true,
                NewWizardControlName = "Simplog.FreightLib.NewWarehouseCommand",
                EditableFromAutoCompleteWindow = true,
                EnableAddFromLOV = true,
                EnableEditFromLOV = true,
                IsMain = true,
                IsAutoComplete = true,
                AutoCompleteSearchWindow = true,
                EnableSecurity = true,
                ObjectTableTypeCode = "MD",
                ClientModuleName = "Common",
                NewWizardComponentPath = "./CommonModules/CommonPartners/Components/NewEntity/NewWarehouseComponent",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region Rank
            RankObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Rank",
                ObjectTableName = "Rank",
                ObjectTablePlural = "Ranks",
                ObjectTableSingular = "Rank",
                DBTableName = "Ranks",
                LookUp1 = "Name",
                Tenant = 0,
                KeyPropertyPath = "Id",
                CacheOnClient = true,
                EditableFromAutoCompleteWindow = false,
                IsMain = true,
                IsAutoComplete = true,
                AutoCompleteSearchWindow = false,
                EnableSecurity = false,
                ObjectTableTypeCode = "MD",
                ClientModuleName = "Common"
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region SystemData
            SystemDataObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "System Data",
                ObjectTableName = "SystemData",
                ObjectTablePlural = "System Data",
                ObjectTableSingular = "System Data",
                Tenant = 0,
                KeyPropertyPath = "Id",
                IsMain = true,
                ObjectTableTypeCode = "MD",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);


            #endregion

            #region  Shared logistics
            SharedLogisticsObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "SharedLogistics",
                ObjectTableName = "SharedLogistics",
                ObjectTablePlural = "Shared Logistics",
                ObjectTableSingular = "Shared Logistics",
                Tenant = 0,
                KeyPropertyPath = "Id",
                IsMain = true,
                ObjectTableTypeCode = "MD",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);



            #endregion

            #region DescriptionOfGoods
            DescriptionOfGoodsObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Description Of Good",
                ObjectTableName = "DescriptionOfGood",
                ObjectTablePlural = "Descriptions Of Goods",
                ObjectTableSingular = "Description Of Goods",
                DBTableName = "DescriptionOfGoods",
                Tenant = 0,
                LookUp1 = "Name",
                LookUp2 = "DescriptionOfGood",
                KeyPropertyPath = "Id",
                IsClosed = true,
                CacheOnClient = true,
                IsAutoComplete = true,
                IsMain = true,
                ObjectTableTypeCode = "MD",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region TarrifTypeObject
            TarrifTypeObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Tarrif Type",
                ObjectTableName = "TarrifType",
                ObjectTablePlural = "Tarrif Types",
                ObjectTableSingular = "Tarrif Type",
                DBTableName = "TarrifTypes",
                Tenant = 0,
                KeyPropertyPath = "Code",
                IsClosed = true,
                IsMain = true,
                ObjectTableTypeCode = "MD",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region TarrifFromToTypeObject
            TarrifFromToTypeObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Tarrif From To Type",
                ObjectTableName = "TarrifFromToType",
                ObjectTablePlural = "Tarrif From To Types",
                ObjectTableSingular = "Tarrif From To Type",
                DBTableName = "TarrifFromToTypes",
                Tenant = 0,
                KeyPropertyPath = "Code",
                IsClosed = true,
                IsMain = true,
                ObjectTableTypeCode = "MD",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region TarrifChargesObject
            TarrifChargesObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Tarrif Charge",
                ObjectTableName = "TarrifCharge",
                ObjectTablePlural = "Tarrif Charges",
                ObjectTableSingular = "Tarrif Charge",
                DBTableName = "TarrifCharges",
                Tenant = 0,
                KeyPropertyPath = "Id",
                IsClosed = false,
                IsMain = true,
                ObjectTableTypeCode = "MD",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region TarrifFromToObject
            TarrifFromToObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Tarrif From To",
                ObjectTableName = "TarrifFromTo",
                ObjectTablePlural = "Tarrifs From To",
                ObjectTableSingular = "Tarrif From To",
                DBTableName = "TarrifFromToes",
                Tenant = 0,
                KeyPropertyPath = "Id",
                IsClosed = false,
                IsMain = true,
                ObjectTableTypeCode = "MD",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region TarrifHeaderObject
            TarrifHeaderObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Tarrif Header",
                ObjectTableName = "TarrifHeader",
                ObjectTablePlural = "Tarrif Headers",
                ObjectTableSingular = "Tarrif Header",
                DBTableName = "TarrifHeaders",
                Tenant = 0,
                KeyPropertyPath = "Id",
                IsClosed = false,
                IsMain = true,
                ObjectTableTypeCode = "MD",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region TarrifStepObject
            TarrifStepObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Tarrif Step",
                ObjectTableName = "TarrifStep",
                ObjectTablePlural = "Tarrif Steps",
                ObjectTableSingular = "Tarrif Step",
                DBTableName = "TarrifSteps",
                Tenant = 0,
                KeyPropertyPath = "Id",
                IsClosed = false,
                IsMain = true,
                ObjectTableTypeCode = "MD",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region TextCodeObject
            TextCodeObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Text Code",
                ObjectTableName = "TextCode",
                ObjectTablePlural = "Text Codes",
                ObjectTableSingular = "Text Code",
                DBTableName = "TextCodes",
                Tenant = 0,
                KeyPropertyPath = "Id",
                IsClosed = false,
                IsMain = true,
                ObjectTableTypeCode = "MD",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region MAWBStackObject
            MAWBStackObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "MAWB Stack",
                ObjectTableName = "MAWBStack",
                ObjectTablePlural = "MAWB Stacks",
                ObjectTableSingular = "MAWB Stack",
                DBTableName = "MAWBStacks",
                Tenant = 0,
                KeyPropertyPath = "Id",
                IsClosed = false,
                IsMain = true,
                ObjectTableTypeCode = "MD",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region RestrictionObject
            RestrictionObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Restriction",
                ObjectTableName = "Restriction",
                ObjectTablePlural = "Restrictions",
                ObjectTableSingular = "Restriction",
                DBTableName = "Restrictions",
                Tenant = 0,
                KeyPropertyPath = "Id",
                IsClosed = false,
                IsMain = true,
                ObjectTableTypeCode = "MD",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region RoleObject
            RoleObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Role",
                ObjectTableName = "Role",
                ObjectTablePlural = "Roles",
                ObjectTableSingular = "Role",
                DBTableName = "Roles",
                Tenant = 0,
                KeyPropertyPath = "Id",
                IsClosed = false,
                IsMain = true,
                ObjectTableTypeCode = "MD",
                IsAutoComplete = true,
                LookUp1 = "Name",
                DependencyFilter1 = "IsCustomRole",
                DependencyFilter2 = "RoleCodeTenantFilter",
                ClientModuleName = "Common",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region ShipmentLevel
            ShipmentLevelObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Shipment Level",
                ObjectTableName = "ShipmentLevel",
                ObjectTablePlural = "Shipment Levels",
                ObjectTableSingular = "Shipment Level",
                DBTableName = "ShipmentLevels",
                Tenant = 0,
                KeyPropertyPath = "Code",
                IsClosed = true,
                IsMain = true,
                IsAutoComplete = true,
                CacheOnClient = true,
                LookUp1 = "Name",
                ObjectTableTypeCode = "MD",
                ClientModuleName = "Shipment",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region Account
            AccountObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Account",
                ObjectTableName = "Account",
                ObjectTablePlural = "Accounts",
                ObjectTableSingular = "Account",
                DBTableName = "Accounts",
                LookUp1 = "Code",
                LookUp2 = "Name",
                Tenant = 0,
                KeyPropertyPath = "Id",
                CacheOnClient = true,
                IsMain = true,
                IsAutoComplete = true,
                DependencyFilter1 = "AccountTypeCode",
                AutoCompleteSearchWindow = true,
                ObjectTableTypeCode = "MD",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region AccountType
            AccountTypeObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Account Type",
                ObjectTableName = "AccountType",
                ObjectTablePlural = "Account Types",
                ObjectTableSingular = "Account Type",
                DBTableName = "AccountTypes",
                LookUp1 = "Code",
                LookUp2 = "Name",
                Tenant = 0,
                KeyPropertyPath = "Code",
                IsAutoComplete = true,
                IsClosed = true,
                ObjectTableTypeCode = "MD",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region ARInvoicePayment
            ARInvoicePaymentObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Invoice Payment",
                ObjectTableName = "ARInvoicePayment",
                ObjectTablePlural = "Invoice Payments",
                ObjectTableSingular = "Invoice Payment",
                DBTableName = "ARInvoicePayments",
                LookUp1 = "Code",
                LookUp2 = "Name",
                Tenant = 0,
                KeyPropertyPath = "Id",
                CacheOnClient = false,
                EditableFromAutoCompleteWindow = true,
                EnableAddFromLOV = true,
                EnableEditFromLOV = true,
                IsMain = true,
                AutoCompleteSearchWindow = true,
                SortingByObjectField = "Code",
                IsComposition = true,
                ObjectTableTypeCode = "BR",
                ClientModuleName = "Invoice",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region ARInvoice
            InvoiceObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "A/R Invoice",
                ObjectTableName = "ARInvoice",
                ObjectTablePlural = "A/R Invoices",
                ObjectTableSingular = "A/R Invoice",
                DBTableName = "ARInvoices",
                Tenant = 0,
                KeyPropertyPath = "Id",
                HasCounter = true,
                IsMain = true,
                IsSaveButtonVisible = false,
                EnableSecurity = true,
                ObjectTableTypeCode = "BR",
                AllowCustomFields = true,
                MaxNumberOfCustomFields = 10,
                HasDocuments = true,
                ClientModuleName = "Invoice",
                HasHelper = true,
                HasShortTitle = true,
                HasMenuButtons = true,
                LocalDefaultText = "חשבונית לקוח",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region ARInvoiceLine
            InvoiceLinesObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "A/R Invoice Line",
                ObjectTableName = "ARInvoiceLine",
                ObjectTablePlural = "A/R Invoice Lines",
                ObjectTableSingular = "A/R Invoice Line",
                DBTableName = "ARInvoiceLines",
                Tenant = 0,
                KeyPropertyPath = "Id",
                IsMain = false,
                IsComposition = true,
                EnableSecurity = true,
                ObjectTableTypeCode = "BR",
                ClientModuleName = "Invoice",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region ARInvoiceType
            InvoiceTypesObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "A/R Invoice Type",
                ObjectTableName = "ARInvoiceType",
                ObjectTablePlural = "A/R Invoice Types",
                ObjectTableSingular = "A/R Invoice Type",
                DBTableName = "ARInvoiceTypes",
                Tenant = 0,
                KeyPropertyPath = "Code",
                CacheOnClient = true,
                LookUp1 = "Code",
                LookUp2 = "Name",
                AutoCompleteSearchWindow = true,
                EditableFromAutoCompleteWindow = true,
                IsClosed = true,
                IsMain = true,
                IsAutoComplete = true,
                ObjectTableTypeCode = "MD",
                ClientModuleName = "Invoice",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region ARInvoiceStatus
            InvoiceStatusObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "A/R Invoice Status",
                ObjectTableName = "ARInvoiceStatus",
                ObjectTablePlural = "A/R Invoice Status",
                ObjectTableSingular = "A/R Invoice Status",
                DBTableName = "ARInvoiceStatus",
                Tenant = 0,
                KeyPropertyPath = "Code",
                LookUp1 = "Code",
                LookUp2 = "Name",
                CacheOnClient = true,
                EditableFromAutoCompleteWindow = true,
                IsClosed = true,
                IsMain = true,
                IsAutoComplete = true,
                ObjectTableTypeCode = "MD",
                ClientModuleName = "Invoice",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region ARInvoiceTransferStatus
            ARInvoiceTransferStatusObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "A/R Invoice Transfer Status",
                ObjectTableName = "ARInvoiceTransferStatus",
                ObjectTablePlural = "A/R Invoice Transfer Status",
                ObjectTableSingular = "A/R Invoice Transfer Status",
                DBTableName = "ARInvoiceTransferStatus",
                Tenant = 0,
                KeyPropertyPath = "Code",
                LookUp1 = "Code",
                LookUp2 = "Name",
                CacheOnClient = true,
                IsClosed = true,
                IsMain = true,
                IsAutoComplete = true,
                EditableFromAutoCompleteWindow = true,
                ObjectTableTypeCode = "MD",
                ClientModuleName = "Invoice",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region APInvoiceTransferStatus
            APInvoiceTransferStatusObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "A/P Invoice Transfer Status",
                ObjectTableName = "APInvoiceTransferStatus",
                ObjectTablePlural = "A/P Invoice Transfer Status",
                ObjectTableSingular = "A/P Invoice Transfer Status",
                DBTableName = "APInvoiceTransferStatus",
                Tenant = 0,
                KeyPropertyPath = "Code",
                LookUp1 = "Code",
                LookUp2 = "Name",
                CacheOnClient = true,
                IsClosed = true,
                IsMain = true,
                IsAutoComplete = true,
                EditableFromAutoCompleteWindow = true,
                ObjectTableTypeCode = "MD",
                ClientModuleName = "Invoice",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region ARPaymentTransferStatus
            ARPaymentTransferStatusObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "A/R Payment Transfer Status",
                ObjectTableName = "ARPaymentTransferStatus",
                ObjectTablePlural = "A/R Payment Transfer Status",
                ObjectTableSingular = "A/R Payment Transfer Status",
                DBTableName = "ARPaymentTransferStatus",
                Tenant = 0,
                KeyPropertyPath = "Code",
                LookUp1 = "Code",
                LookUp2 = "Name",
                CacheOnClient = true,
                IsClosed = true,
                IsMain = true,
                IsAutoComplete = true,
                EditableFromAutoCompleteWindow = true,
                ObjectTableTypeCode = "MD",
                ClientModuleName = "Invoice",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region APPaymentTransferStatus
            APPaymentTransferStatusObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "A/P Payment Transfer Status",
                ObjectTableName = "APPaymentTransferStatus",
                ObjectTablePlural = "A/P Payment Transfer Status",
                ObjectTableSingular = "A/P Payment Transfer Status",
                DBTableName = "APPaymentTransferStatus",
                Tenant = 0,
                KeyPropertyPath = "Code",
                LookUp1 = "Code",
                LookUp2 = "Name",
                CacheOnClient = true,
                IsClosed = true,
                IsMain = true,
                IsAutoComplete = true,
                EditableFromAutoCompleteWindow = true,
                ObjectTableTypeCode = "MD",
                ClientModuleName = "Invoice",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region ARPayment
            ARPaymentObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "A/R Payment",
                ObjectTableName = "ARPayment",
                ObjectTablePlural = "A/R Payments",
                ObjectTableSingular = "A/R Payment",
                DBTableName = "ARPayments",
                Tenant = 0,
                KeyPropertyPath = "Id",
                HasCounter = true,
                IsMain = true,
                IsNewWizard = true,
                NewWizardControlName = "Simplog.InvoiceLib.NewARPaymentCommand",
                EnableSecurity = true,
                ObjectTableTypeCode = "BR",
                HasDocuments = true,
                ClientModuleName = "Invoice",
                HasHelper = true,
                HasShortTitle = true,
                HasMenuButtons = true,
                LocalDefaultText = "קבלת לקוח",
                NewWizardComponentPath = "./InvoiceModules/ARPayment/Components/NewEntity/NewARPaymentComponent",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region PaymentMethod
            AccountingPaymentMethodObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Accounting Payment Method",
                ObjectTableName = "AccountingPaymentMethod",
                ObjectTablePlural = "Accounting Payment Methods",
                ObjectTableSingular = "Accounting Payment Method",
                DBTableName = "AccountingPaymentMethods",
                Tenant = 0,
                KeyPropertyPath = "Id",
                IsClosed = false,
                IsMain = true,
                IsAutoComplete = true,
                CacheOnClient = true,
                LookUp1 = "Name",
                ObjectTableTypeCode = "MD",
                ClientModuleName = "Invoice",
                DependencyFilter1="IsAR",
                DependencyFilter2="IsAP",
                EnableSecurity = true,
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region ARPaymentStatus
            ARPaymentStatusObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "A/R Payment Status",
                ObjectTableName = "ARPaymentStatus",
                ObjectTablePlural = "A/R Payment Status",
                ObjectTableSingular = "A/R Payment Status",
                DBTableName = "ARPaymentStatus",
                Tenant = 0,
                KeyPropertyPath = "Code",
                IsClosed = true,
                IsMain = false,
                IsAutoComplete = true,
                CacheOnClient = true,
                LookUp1 = "Name",
                ObjectTableTypeCode = "MD",
                ClientModuleName = "Invoice",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region APInvoiceType
            APInvoiceTypesObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "A/P Invoice Type",
                ObjectTableName = "APInvoiceType",
                ObjectTablePlural = "A/P Invoice Types",
                ObjectTableSingular = "A/P Invoice Type",
                DBTableName = "APInvoiceTypes",
                Tenant = 0,
                KeyPropertyPath = "Code",
                CacheOnClient = true,
                LookUp1 = "Code",
                LookUp2 = "Name",
                AutoCompleteSearchWindow = true,
                EditableFromAutoCompleteWindow = true,
                IsClosed = true,
                IsMain = true,
                IsAutoComplete = true,
                ObjectTableTypeCode = "MD",
                ClientModuleName = "Invoice",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region APInvoiceLine
            APInvoiceLineObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "A/P Invoice Line",
                ObjectTableName = "APInvoiceLine",
                ObjectTablePlural = "A/P Invoice Lines",
                ObjectTableSingular = "A/P Invoice Line",
                DBTableName = "APInvoiceLines",
                Tenant = 0,
                IsMain = false,
                IsComposition = true,
                EnableSecurity = true,
                ObjectTableTypeCode = "BR",
                ClientModuleName = "Invoice",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region APInvoiceStatus
            APInvoiceStatusObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "A/P Invoice Status",
                ObjectTableName = "APInvoiceStatus",
                ObjectTablePlural = "A/P Invoice Status",
                ObjectTableSingular = "A/P Invoice Status",
                DBTableName = "APInvoiceStatus",
                Tenant = 0,
                KeyPropertyPath = "Code",
                LookUp1 = "Code",
                LookUp2 = "Name",
                CacheOnClient = true,
                EditableFromAutoCompleteWindow = true,
                IsClosed = true,
                IsMain = true,
                IsAutoComplete = true,
                ObjectTableTypeCode = "MD",
                ClientModuleName = "Invoice",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region APInvoice
            APInvoiceObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "A/P Invoice",
                ObjectTableName = "APInvoice",
                ObjectTablePlural = "A/P Invoices",
                ObjectTableSingular = "A/P Invoice",
                DBTableName = "APInvoices",
                Tenant = 0,
                KeyPropertyPath = "Id",
                HasCounter = true,
                IsMain = true,
                IsSaveButtonVisible = false,
                EnableSecurity = true,
                ObjectTableTypeCode = "BR",
                HasDocuments = true,
                ClientModuleName = "Invoice",
                HasHelper = true,
                HasShortTitle = true,
                HasMenuButtons = true,
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region ARInvoiceTotalVat
            ARInvoiceTotalVatObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "A/R Invoice Total VAT",
                ObjectTableName = "ARInvoiceTotalVAT",
                ObjectTablePlural = "A/R Invoice Total VATs",
                ObjectTableSingular = "A/R Invoice Total VAT",
                DBTableName = "ARInvoiceTotalVATs",
                Tenant = 0,
                KeyPropertyPath = "Id",
                IsMain = false,
                EnableSecurity = true,
                IsComposition = true,
                ObjectTableTypeCode = "BR",
                ClientModuleName = "Invoice",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region APInvoiceTotalVat
            APInvoiceTotalVatObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "A/P Invoice Total VAT",
                ObjectTableName = "APInvoiceTotalVAT",
                ObjectTablePlural = "A/P Invoice Total VATs",
                ObjectTableSingular = "A/P Invoice Total VAT",
                DBTableName = "APInvoiceTotalVATs",
                Tenant = 0,
                KeyPropertyPath = "Id",
                IsMain = false,
                EnableSecurity = true,
                IsComposition = true,
                ObjectTableTypeCode = "BR",
                ClientModuleName = "Invoice",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region APPayment
            APPaymentObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "A/P Payment",
                ObjectTableName = "APPayment",
                ObjectTablePlural = "A/P Payments",
                ObjectTableSingular = "A/P Payment",
                DBTableName = "APPayments",
                Tenant = 0,
                KeyPropertyPath = "Id",
                HasCounter = true,
                IsMain = true,
                EnableSecurity = true,
                ObjectTableTypeCode = "BR",
                HasDocuments = true,
                ClientModuleName = "Invoice",
                HasHelper = true,
                HasShortTitle = true,
                HasMenuButtons = true,
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region APPaymentMethod
            APPaymentMethodObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "A/P Payment Method",
                ObjectTableName = "APPaymentMethod",
                ObjectTablePlural = "A/P Payment Methods",
                ObjectTableSingular = "A/P Payment Method",
                DBTableName = "APPaymentMethods",
                Tenant = 0,
                KeyPropertyPath = "Id",
                IsClosed = false,
                IsMain = true,
                IsAutoComplete = true,
                CacheOnClient = true,
                LookUp1 = "Name",
                ObjectTableTypeCode = "MD",
                ClientModuleName = "Invoice",
                EnableSecurity = true,
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region APPaymentStatus
            APPaymentStatusObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "A/P Payment Status",
                ObjectTableName = "APPaymentStatus",
                ObjectTablePlural = "A/P Payment Status",
                ObjectTableSingular = "A/P Payment Status",
                DBTableName = "APPaymentStatus",
                Tenant = 0,
                KeyPropertyPath = "Code",
                IsClosed = true,
                IsMain = false,
                IsAutoComplete = true,
                CacheOnClient = true,
                ObjectTableTypeCode = "MD",
                LookUp1 = "Name",
                ClientModuleName = "Invoice",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region APInvoicePayment
            APInvoicePaymentObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "A/P Invoice Payment",
                ObjectTableName = "APInvoicePayment",
                ObjectTablePlural = "A/P Invoice Payments",
                ObjectTableSingular = "A/P Invoice Payment",
                DBTableName = "APInvoicePayment",
                Tenant = 0,
                KeyPropertyPath = "Id",
                IsMain = false,
                IsComposition = true,
                ObjectTableTypeCode = "BR",
                ClientModuleName = "Invoice",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region BankAccountLite
            BankAccountLiteObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Bank Account",
                ObjectTableName = "BankAccountLite",
                ObjectTablePlural = "Bank Accounts",
                ObjectTableSingular = "Bank Account Lite",
                DBTableName = "BankAccountLites",
                DescriptionDefaultText = "Manage your business Bank Accounts",
                LookUp1 = "EnglishName",
                LookUp2 = "AccountNumber",
                Tenant = 0,
                KeyPropertyPath = "Id",
                IsAutoComplete = true,
                EnableSecurity = true,
                IsMain = true,
                ObjectTableTypeCode = "BR",
                IsNewWizard = true,
                NewWizardControlName = "Simplog.InvoiceLib.NewBankAccountLiteCommand",
                NewWizardComponentPath = "./Invoice/Components/NewEntity/NewBankAccountLiteComponent",
                ClientModuleName = "Invoice",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region Vendor
            VendorObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Vendor",
                ObjectTableName = "Vendor",
                ObjectTablePlural = "Vendors",
                ObjectTableSingular = "Vendor",
                DBTableName = "Vendors",
                Tenant = 0,
                KeyPropertyPath = "Id",
                EditableFromAutoCompleteWindow = true,
                IsNewWizard = true,
                NewWizardControlName = "Simplog.FreightLib.NewVendorCommand",
                IsMain = true,
                EnableSecurity = true,
                ObjectTableTypeCode = "MD",
                ClientModuleName = "Common",
                NewWizardComponentPath = "./CommonModules/CommonPartners/Components/NewEntity/NewVendorComponent",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region CommunicationLogs
            CommunicationLogsObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Communication Log",
                ObjectTableName = "CommunicationLog",
                ObjectTablePlural = "Communication Logs",
                ObjectTableSingular = "Communication Log",
                DBTableName = "CommunicationLogs",
                Tenant = 0,
                KeyPropertyPath = "Id",
                IsMain = true,
                EnableSecurity = true,
                ObjectTableTypeCode = "MD",
                ClientModuleName = "Common",
                HasMenuButtons = true,
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region CommunicationLogTypes
            CommunicationLogTypesObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Communication Log Type",
                ObjectTableName = "CommunicationLogType",
                ObjectTablePlural = "Communication Log Types",
                ObjectTableSingular = "Communication Log Type",
                DBTableName = "CommunicationLogTypes",
                Tenant = 0,
                KeyPropertyPath = "Code",
                IsClosed = true,
                IsMain = false,
                IsAutoComplete = true,
                CacheOnClient = true,
                ObjectTableTypeCode = "MD",
                LookUp1 = "Name",
                ClientModuleName = "Common",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region WarehouseTypes
            WarehouseTypesObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Warehouse Type",
                ObjectTableName = "WarehouseType",
                ObjectTablePlural = "Warehouse Types",
                ObjectTableSingular = "Warehouse Type",
                DBTableName = "WarehouseTypes",
                Tenant = 0,
                KeyPropertyPath = "Code",
                IsClosed = true,
                IsMain = false,
                IsAutoComplete = true,
                CacheOnClient = true,
                ObjectTableTypeCode = "MD",
                LookUp1 = "Name",
                ClientModuleName = "Common",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region CommunicationStatusTypes
            CommunicationStatusTypesObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Communication Status Type",
                ObjectTableName = "CommunicationStatusType",
                ObjectTablePlural = "Communication Status Types",
                ObjectTableSingular = "Communication Status Type",
                DBTableName = "CommunicationStatusTypes",
                Tenant = 0,
                KeyPropertyPath = "Code",
                IsClosed = true,
                IsMain = false,
                IsAutoComplete = true,
                CacheOnClient = true,
                ObjectTableTypeCode = "MD",
                LookUp1 = "Name",
                ClientModuleName = "Common",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region PasswordPolicies
            PasswordPoliciesObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Passwoed Policy",
                ObjectTableName = "PasswordPolicy",
                ObjectTablePlural = "Password Policies",
                ObjectTableSingular = "Password Policy",
                DBTableName = "PasswordPolicies",
                Tenant = 0,
                KeyPropertyPath = "Code",
                IsClosed = true,
                IsMain = false,
                IsAutoComplete = true,
                CacheOnClient = true,
                ObjectTableTypeCode = "MD",
                LookUp1 = "PasswordStrength",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region Packages
            PackagesObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Package",
                ObjectTableName = "Package",
                ObjectTablePlural = "Packages",
                ObjectTableSingular = "Package",
                DBTableName = "Packages",
                Tenant = 0,
                KeyPropertyPath = "Code",
                IsClosed = false,
                IsMain = false,
                IsAutoComplete = true,
                ClientModuleName = "Common",
                CacheOnClient = true,
                ObjectTableTypeCode = "MD",
                LookUp1 = "Name",
                DependencyFilter1 = "FeaturePackageTypeCode",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region TermsofUseSignatures
            TermsofUseSignatureObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Terms of Use Signature",
                ObjectTableName = "TermsofUseSignature",
                ObjectTablePlural = "TermsofUseSignatures",
                ObjectTableSingular = "TermsofUseSignature",
                DBTableName = "TermsofUseSignatures",
                Tenant = 0,
                KeyPropertyPath = "Id",
                IsClosed = false,
                IsMain = false,
                IsAutoComplete = false,
                CacheOnClient = false,
                ObjectTableTypeCode = "MD",

            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region AnalyzeQueueObject
            AnalyzeQueueObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Analyze Queue",
                ObjectTableName = "AnalyzeQueue",
                ObjectTablePlural = "AnalyzeQueues",
                ObjectTableSingular = "Analyze Queue",
                DBTableName = "AnalyzeQueues",
                Tenant = 0,
                IsMain = true,
                ObjectTableTypeCode = "MD",
                EnableSecurity = true,
                ClientModuleName = "Infrastructure",
                DisableSearchBox = true,
                HasMenuButtons = true,
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region CreditCardTypeObject
            CreditCardTypeObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Credit Card Type",
                ObjectTableName = "CreditCardType",
                ObjectTablePlural = "Credit Card Types",
                ObjectTableSingular = "Credit Card Type",
                DBTableName = "CreditCardTypes",
                Tenant = 0,
                KeyPropertyPath = "Id",
                LookUp1 = "Code",
                LookUp2 = "Name",
                CacheOnClient = true,
                EditableFromAutoCompleteWindow = true,
                IsMain = true,
                IsAutoComplete = true,
                AutoCompleteSearchWindow = true,
                ObjectTableTypeCode = "MD",
                EnableEditFromLOV = true,
                EnableAddFromLOV = true,
                EnableSecurity = true,
                ClientModuleName = "Invoice",
                AllowedForComputingPartners = true,
                CodeField="Code",
                NameField = "Name",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region RecurringPeriod
            RecurringPeriodObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Recurring Period",
                ObjectTableName = "RecurringPeriod",
                ObjectTablePlural = "Recurring Periods",
                ObjectTableSingular = "Recurring Period",
                DBTableName = "RecurringPeriods",
                Tenant = 0,
                KeyPropertyPath = "Code",
                IsClosed = true,
                IsMain = false,
                IsAutoComplete = true,
                CacheOnClient = true,
                ObjectTableTypeCode = "MD",
                LookUp1 = "Name",
                ClientModuleName = "Infrastructure",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region PaymentMethodObject
            PaymentMethodObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Payment Method",
                ObjectTableName = "PaymentMethod",
                ObjectTablePlural = "Payment Methods",
                ObjectTableSingular = "Payment Method",
                DBTableName = "PaymentMethods",
                Tenant = 0,
                KeyPropertyPath = "Code",
                IsClosed = true,
                IsMain = false,
                IsAutoComplete = true,
                CacheOnClient = true,
                ObjectTableTypeCode = "MD",
                LookUp1 = "Name",
                ClientModuleName = "Infrastructure",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region PaymentChannelObject
            PaymentChannelObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Payment Channel",
                ObjectTableName = "PaymentChannel",
                ObjectTablePlural = "Payment Channels",
                ObjectTableSingular = "Payment Channel",
                DBTableName = "PaymentChannels",
                ClientModuleName = "Infrastructure",
                Tenant = 0,
                KeyPropertyPath = "Code",
                IsClosed = true,
                IsMain = false,
                IsAutoComplete = true,
                CacheOnClient = true,
                ObjectTableTypeCode = "MD",
                LookUp1 = "Name",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region EventTypeCategoryObject
            EventTypeCategoryObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Event Type Category",
                ObjectTableName = "EventTypeCategory",
                ObjectTablePlural = "Event Type Categories",
                ObjectTableSingular = "Event Type Category",
                DBTableName = "EventTypeCategories",
                Tenant = 0,
                KeyPropertyPath = "Code",
                IsClosed = true,
                IsMain = false,
                IsAutoComplete = true,
                CacheOnClient = true,
                ObjectTableTypeCode = "MD",
                LookUp1 = "Name",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region AccountingTransferHeader
            AccountingTransferHeaderObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Accounting Transfer",
                ObjectTableName = "AccountingTransferHeader",
                ObjectTablePlural = "Accounting Transfers",
                ObjectTableSingular = "Accounting Transfer",
                DBTableName = "AccountingTransferHeaders",
                Tenant = 0,
                KeyPropertyPath = "Id",
                HasCounter = true,
                IsMain = true,
                EnableSecurity = true,
                ObjectTableTypeCode = "BR",
                IsNewWizard = true,
                NewWizardControlName = "Simplog.InvoiceLib.NewAccountingTransferCommand",
                ClientModuleName = "Invoice",
                HasHelper = true,
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region AccountingTransferLine
            AccountingTransferLineObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Accounting Transfer Line",
                ObjectTableName = "AccountingTransferLine",
                ObjectTablePlural = "Accounting Transfer Lines",
                ObjectTableSingular = "Accounting Transfer Line",
                DBTableName = "AccountingTransferLines",
                Tenant = 0,
                KeyPropertyPath = "Id",
                IsMain = false,
                IsComposition = true,
                EnableSecurity = true,
                ObjectTableTypeCode = "BR",
                ClientModuleName = "Invoice",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region AccountingTransferType
            AccountingTransferTypeObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Accounting Transfer Type",
                ObjectTableName = "AccountingTransferType",
                ObjectTablePlural = "Accounting Transfer Types",
                ObjectTableSingular = "Accounting Transfer Type",
                DBTableName = "AccountingTransferTypes",
                Tenant = 0,
                KeyPropertyPath = "Id",
                IsMain = true,
                IsClosed = true,
                CacheOnClient = true,
                ObjectTableTypeCode = "MD",
                LookUp1 = "Name",
                IsAutoComplete = true,
                ClientModuleName = "Invoice",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region Incoterm
            IncotermsObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Incoterm",
                ObjectTableName = "Incoterm",
                ObjectTablePlural = "Incoterms",
                ObjectTableSingular = "Incoterm",
                DBTableName = "Incoterms",
                Tenant = 0,
                LookUp1 = "Code",
                LookUp2 = "Name",
                KeyPropertyPath = "Id",
                CacheOnClient = true,
                EditableFromAutoCompleteWindow = true,
                EnableAddFromLOV = true,
                EnableEditFromLOV = true,
                IsMain = true,
                IsAutoComplete = true,
                AutoCompleteSearchWindow = false,
                EnableSecurity = true,
                ObjectTableTypeCode = "MD",
                ClientModuleName = "Common",
                AllowedForComputingPartners = true,
                CodeField="Code",
                NameField = "Name",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region MoveTypeObject
            MoveTypeObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Move Type",
                ObjectTableName = "MoveType",
                ObjectTablePlural = "Move Types",
                ObjectTableSingular = "Move Type",
                DBTableName = "MoveTypes",
                Tenant = 0,
                KeyPropertyPath = "Id",
                LookUp1 = "Code",
                LookUp2 = "MoveTypeEnglishName",
                CacheOnClient = true,
                EditableFromAutoCompleteWindow = true,
                IsMain = true,
                IsAutoComplete = true,
                AutoCompleteSearchWindow = false,
                ObjectTableTypeCode = "MD",
                EnableEditFromLOV = true,
                EnableAddFromLOV = true,
                IsNewWizard = true,
                NewWizardControlName = "Simplog.Infrastructure.NewMoveTypeCommand",     
                NewWizardComponentPath = "./InfrastructureModules/InfrastructureOthers/Components/MoveType/NewMoveTypeComponent",

                DependencyFilter1 = "TransportModeId",
                EnableSecurity = true,
                ClientModuleName = "Infrastructure",
                AllowedForComputingPartners = true,
                CodeField="Code",
                NameField = "Name",

            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region ErrorLog
            ErrorLogObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Error Log",
                ObjectTableName = "ErrorLog",
                ObjectTablePlural = "Error Log",
                ObjectTableSingular = "ErrorLog",
                DBTableName = "ErrorLogs",
                Tenant = 0,
                IsMain = true,
                ObjectTableTypeCode = "MD",
                EnableSecurity = true,
                ClientModuleName = "Infrastructure",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region Report
            ReportObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Report",
                ObjectTableName = "Report",
                ObjectTablePlural = "Reports",
                ObjectTableSingular = "Report",
                DBTableName = "Reports",
                Tenant = 0,
                KeyPropertyPath = "Id",
                HasCounter = true,
                IsMain = true,
                IsRestrictable = true,
                EnableSecurity = true,
                ObjectTableTypeCode = "MD",
                AllowCustomFields = true,
                MaxNumberOfCustomFields = 10,
                HasDynamicHeader = true,
                ClientModuleName = "Common",

            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region SharedLogisticsInvitationStatus
            SharedLogisticsInvitationStatusObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Shared Logistics Invitation Status",
                ObjectTableName = "SharedLogisticsInvitationStatus",
                ObjectTablePlural = "Shared Logistics Invitation Status",
                ObjectTableSingular = "Shared Logistics Invitation Status",
                DBTableName = "SharedLogisticsInvitationStatus",
                Tenant = 0,
                KeyPropertyPath = "Code",
                IsClosed = true,
                IsMain = false,
                IsAutoComplete = true,
                CacheOnClient = true,
                ObjectTableTypeCode = "MD",
                LookUp1 = "Name",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region DocumentTypeCopy
            DocumentTypeCopy = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Document Type Copy",
                ObjectTableName = "DocumentTypeCopy",
                ObjectTablePlural = "Document Type Copy",
                ObjectTableSingular = "Document Type Copy",
                DBTableName = "DocumentTypeCopies",

                Tenant = 0,
                IsMain = false,
                IsComposition = true,
                ObjectTableTypeCode = "MD",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region AWBOCI
            AWBOCIObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "AWB OCI",
                ObjectTableName = "AWBOCI",
                ObjectTablePlural = "AWB OCIs",
                ObjectTableSingular = "AWB OCI",
                DBTableName = "AWBOCIs",
                Tenant = 0,
                KeyPropertyPath = "Id",
                IsMain = true,
                EnableSecurity = false,
                IsComposition = true,
                ObjectTableTypeCode = "BR",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region AWBCustomsInfo
            AWBCustomsInfoObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "AWB Customs Information",
                ObjectTableName = "AWBCustomsInformation",
                ObjectTablePlural = "AWB Customs Informations",
                ObjectTableSingular = "AWB Customs Information",
                DBTableName = "AWBCustomsInformations",
                LookUp1 = "Code",
                KeyPropertyPath = "Code",
                Tenant = 0,
                IsClosed = true,
                CacheOnClient = true,
                IsAutoComplete = true,
                IsMain = true,
                ObjectTableTypeCode = "MD",
                ClientModuleName = "Shipment",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region AWBInformation
            AWBInformationObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "AWB Information",
                ObjectTableName = "AWBInformation",
                ObjectTablePlural = "AWB Informations",
                ObjectTableSingular = "AWB Information",
                DBTableName = "AWBInformations",
                LookUp1 = "Code",
                KeyPropertyPath = "Code",
                Tenant = 0,
                IsClosed = true,
                CacheOnClient = true,
                IsAutoComplete = true,
                IsMain = true,
                ObjectTableTypeCode = "MD",
                ClientModuleName = "Shipment",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region ShipmentPackageItem
            ShipmentPackageItemObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Shipment Package Item",
                ObjectTableName = "ShipmentPackageItem",
                ObjectTablePlural = "Shipment Package Items",
                ObjectTableSingular = "Shipment Package Item",
                DBTableName = "ShipmentPackageItems",
                Tenant = 0,
                IsMain = true,
                EnableSecurity = true,
                IsComposition = true,
                ObjectTableTypeCode = "BR",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region LeadSource
            LeadSourceObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                ObjectTableName = "LeadSource",
                DBTableName = "LeadSources",
                ObjectTableSingular = "Lead Source",
                ObjectTablePlural = "Lead Sources",
                IsNewWizard = false,
                LookUp1 = "Name",
                KeyPropertyPath = "Id",
                AutoCompleteSearchWindow = false,
                IsClosed = false,
                CacheOnClient = true,
                EditableFromAutoCompleteWindow = false,
                HasCounter = false,
                EnableAddFromLOV = true,
                IsRestrictable = false,
                IsMain = true,
                IsAutoComplete = true,
                EnableEditFromLOV = true,
                SortingByObjectField = "Name",
                InActive = false,
                IsSaveButtonVisible = true,
                IsComposition = false,
                EnableSecurity = true,
                AllowCustomFields = false,
                HasDynamicHeader = false,
                ObjectTableTypeCode = "MD",
                MaxNumberOfCustomFields = 0,
                DefaultText = "Lead Source",
                ClientModuleName = "Common",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region Industry
            IndustryObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                ObjectTableName = "Industry",
                DBTableName = "Industries",
                ObjectTableSingular = "Industry",
                ObjectTablePlural = "Industries",
                IsNewWizard = false,
                LookUp1 = "Name",
                KeyPropertyPath = "Id",
                AutoCompleteSearchWindow = false,
                IsClosed = false,
                CacheOnClient = true,
                EditableFromAutoCompleteWindow = false,
                HasCounter = false,
                EnableAddFromLOV = true,
                IsRestrictable = false,
                IsMain = true,
                IsAutoComplete = true,
                EnableEditFromLOV = true,
                SortingByObjectField = "Code",
                InActive = false,
                IsSaveButtonVisible = true,
                IsComposition = false,
                EnableSecurity = true,
                AllowCustomFields = false,
                HasDynamicHeader = false,
                ObjectTableTypeCode = "MD",
                MaxNumberOfCustomFields = 0,
                DefaultText = "Industry",
                ClientModuleName = "Common"
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region ProductTypeObject
            ProductTypeObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Product Type",
                ObjectTableName = "ProductType",
                ObjectTablePlural = "Product Types",
                ObjectTableSingular = "Product Type",
                DBTableName = "ProductTypes",
                Tenant = 0,
                KeyPropertyPath = "Code",
                IsClosed = false,
                IsMain = true,
                IsAutoComplete = true,
                CacheOnClient = true,
                ObjectTableTypeCode = "MD",
                LookUp1 = "Name",
                EnableSecurity = true,
                ClientModuleName = "Common"
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region ProductPeriodObject
            ProductPeriodObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Product Period",
                ObjectTableName = "ProductPeriod",
                ObjectTablePlural = "Product Periods",
                ObjectTableSingular = "Product Period",
                DBTableName = "ProductPeriods",
                Tenant = 0,
                KeyPropertyPath = "Code",
                IsClosed = true,
                IsMain = false,
                IsAutoComplete = true,
                CacheOnClient = true,
                ObjectTableTypeCode = "MD",
                LookUp1 = "Name",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region CustomerProductObject
            CustomerProductObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                ObjectTableName = "CustomerProduct",
                DBTableName = "CustomerProducts",
                ObjectTableSingular = "Customer Product",
                ObjectTablePlural = "Customer Products",
                KeyPropertyPath = "CustomerId",
                IsMain = true,
                SortingByObjectField = "CustomerId",
                ObjectTableTypeCode = "MD",
                DefaultText = "Customer Product",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region CustomerProductActualDataObject
            CustomerProductActualDataObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                ObjectTableName = "CustomerProductActualData",
                DBTableName = "CustomerProductActualDatas",
                ObjectTableSingular = "Customer Product Actual Data",
                ObjectTablePlural = "Customer Product Actual Datas",
                LookUp1 = "CustomerId",
                LookUp2 = "ProductTypeCode",
                KeyPropertyPath = "CustomerId",
                IsMain = true,
                SortingByObjectField = "CustomerId",
                ObjectTableTypeCode = "MD",
                DefaultText = "Customer Product Actual Data",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region CustomerProductLocationObject
            CustomerProductLocationObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                ObjectTableName = "CustomerProductLocation",
                DBTableName = "CustomerProductLocations",
                ObjectTableSingular = "Customer Product Location",
                ObjectTablePlural = "Customer Product Locations",
                KeyPropertyPath = "CustomerId",
                IsMain = true,
                SortingByObjectField = "CustomerId",
                ObjectTableTypeCode = "MD",
                DefaultText = "Customer Product Location",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region CustomerProductLocationActualDataObject
            CustomerProductLocationActualDataObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                ObjectTableName = "CustomerProductActualDataLocation",
                DBTableName = "CustomerProductLocationActualDatas",
                ObjectTableSingular = "Customer Product Location Actual Data",
                ObjectTablePlural = "Customer Product Location Actual Datas",
                KeyPropertyPath = "CustomerId",
                IsMain = true,
                SortingByObjectField = "CustomerId",
                ObjectTableTypeCode = "MD",
                DefaultText = "Customer Product Location Actual Data",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region CompetitorObject
            CompetitorObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                ObjectTableName = "Competitor",
                DBTableName = "Competitors",
                ObjectTableSingular = "Competitor",
                ObjectTablePlural = "Competitors",
                IsNewWizard = true,
                LookUp1 = "Name",
                KeyPropertyPath = "Id",
                IsMain = true,
                SortingByObjectField = "Name",
                IsSaveButtonVisible = true,
                EnableSecurity = true,
                ObjectTableTypeCode = "MD",
                NewWizardControlName = "Logitude.CRM.NewCompetitorCommand",
                DefaultText = "Competitor",
                ClientModuleName= "Common",
                NewWizardComponentPath= "./Common/Components/Maintenance/CompetitorComponent",

            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region CustomerAdditionalServiceObject
            CustomerAdditionalServiceObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                ObjectTableName = "CustomerAdditionalService",
                DBTableName = "CustomerAdditionalServices",
                ObjectTableSingular = "Customer Additional Service",
                ObjectTablePlural = "Customer Additional Services",
                KeyPropertyPath = "CustomerId",
                IsMain = true,
                SortingByObjectField = "CustomerId",
                ObjectTableTypeCode = "MD",
                DefaultText = "Customer Additional Service",
                ClientModuleName="Common",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region Commodity
            CommodityObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Commodity",
                ObjectTableName = "Commodity",
                ObjectTablePlural = "Commodities",
                ObjectTableSingular = "Commodity",
                DBTableName = "Commodities",
                Tenant = 0,
                KeyPropertyPath = "Id",
                IsMain = true,
                EnableSecurity = true,
                ObjectTableTypeCode = "MD",
                ClientModuleName = "Common",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region ContactDoneMethod
            ContactDoneMethodObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Contact Done Method",
                ObjectTableName = "ContactDoneMethod",
                ObjectTablePlural = "DContact Done Methods",
                ObjectTableSingular = "Contact Done Method",
                DBTableName = "ContactDoneMethods",
                LookUp1 = "Name",
                Tenant = 0,
                KeyPropertyPath = "Code",
                IsClosed = true,
                CacheOnClient = true,
                IsAutoComplete = true,
                IsMain = true,
                ObjectTableTypeCode = "MD",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region AdditionalService
            AdditionalServiceObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                ObjectTableName = "AdditionalService",
                DBTableName = "AdditionalServices",
                ObjectTableSingular = "Additional Service",
                ObjectTablePlural = "Additional Services",
                LookUp1 = "Name",
                KeyPropertyPath = "Id",
                CacheOnClient = true,
                IsMain = true,
                IsAutoComplete = true,
                SortingByObjectField = "Name",
                IsSaveButtonVisible = true,
                EnableSecurity = true,
                ObjectTableTypeCode = "MD",
                MaxNumberOfCustomFields = 0,
                DefaultText = "Additional Service",
                ClientModuleName ="Common",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region QuotePackage
            QuotePackageObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "QuotePackage",
                ObjectTableName = "QuotePackage",
                ObjectTablePlural = "Quote Packages",
                ObjectTableSingular = "Quote Package",
                DBTableName = "QuotePackages",
                Tenant = 0,
                KeyPropertyPath = "Id",
                IsMain = false,
                IsComposition = true,
                EnableSecurity = true,
                ObjectTableTypeCode = "BR",
                ClientModuleName = "Quote",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region QuoteTemplate

            QuoteTemplateObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                ObjectTableName = "QuoteTemplate",
                DBTableName = "QuoteTemplates",
                ObjectTableSingular = "Quote Template",
                ObjectTablePlural = "Quote Templates",
                Tenant = 0,
                LookUp1 = "Name",
                KeyPropertyPath = "Id",
                CacheOnClient = false,
                EditableFromAutoCompleteWindow = true,
                NewWizardControlName = "Simplog.QuoteLib.NewQuoteTemplateCommand",
                NewWizardComponentPath = "./QuoteModules/QuoteTemplates/Components/NewQuoteTemplateComponent",
                IsMain = true,
                IsNewWizard = true,
                IsAutoComplete = true,
                AutoCompleteSearchWindow = false,
                EnableSecurity = true,
                DefaultText = "Quote Template",
                ObjectTableTypeCode = "MD",
                ClientModuleName = "Quote",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region LogitudeLead
            LogitudeLeadObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                ObjectTableName = "LogitudeLead",
                DBTableName = "LogitudeLeads",
                ObjectTableSingular = "Logitude Lead",
                ObjectTablePlural = "Logitude Leads",
                Tenant = 0,
                LookUp1 = "Name",
                KeyPropertyPath = "Id",
                CacheOnClient = false,
                EditableFromAutoCompleteWindow = true,
                EnableSecurity = true,
                IsMain = true,
                IsNewWizard = true,
                IsAutoComplete = true,
                AutoCompleteSearchWindow = true,
                IsClosed = false,
                IsComposition = false,
                DefaultText = "Logitude Leads",
                ObjectTableTypeCode = "MD",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);

            #endregion

            #region CustomPickList
            CustomPickListObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                ObjectTableName = "CustomPickList",
                DBTableName = "CustomPickLists",
                ObjectTableSingular = "Custom Pick List",
                ObjectTablePlural = "Custom Pick Lists",
                Tenant = 0,
                LookUp1 = "Code",
                LookUp2 = "Value",
                KeyPropertyPath = "Id",
                CacheOnClient = true,
                EditableFromAutoCompleteWindow = true,
                EnableSecurity = false,
                IsMain = true,
                IsNewWizard = false,
                IsAutoComplete = true,
                AutoCompleteSearchWindow = true,
                IsClosed = false,
                IsComposition = false,
                DefaultText = "Custom Pick Lists",
                ObjectTableTypeCode = "MD",
                ClientModuleName= "Infrastructure",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);

            #endregion

            #region QuoteTemplateSection

            QuoteTemplateSectionObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                ObjectTableName = "QuoteTemplateSection",
                DBTableName = "QuoteTemplateSections",
                ObjectTableSingular = "Quote Template Sections",
                ObjectTablePlural = "Quote Template Sections",
                Tenant = 0,
                KeyPropertyPath = "Id",
                CacheOnClient = false,
                EditableFromAutoCompleteWindow = true,
                IsMain = true,
                IsAutoComplete = true,
                AutoCompleteSearchWindow = true,
                EnableSecurity = false,
                DefaultText = "Quote Template Sections",
                ObjectTableTypeCode = "MD",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region QuoteTemplateSectionType

            QuoteTemplateSectionTypeObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                ObjectTableName = "QuoteTemplateSectionType",
                DBTableName = "QuoteTemplateSectionTypes",
                ObjectTableSingular = "Quote Template Section Type",
                ObjectTablePlural = "Quote Template Section Types",
                Tenant = 0,
                KeyPropertyPath = "Code",
                CacheOnClient = false,
                EditableFromAutoCompleteWindow = false,
                ObjectTableTypeCode = "MD",
                IsMain = true,
                IsAutoComplete = true,
                AutoCompleteSearchWindow = false,
                EnableSecurity = false,
                DefaultText = "Quote Template Section Type",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region BorderType

            BorderTypeObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                ObjectTableName = "BorderType",
                DBTableName = "BorderTypes",
                ObjectTableSingular = "Border Type",
                ObjectTablePlural = "Border Types",
                Tenant = 0,
                KeyPropertyPath = "Code",
                CacheOnClient = false,
                EditableFromAutoCompleteWindow = false,
                ObjectTableTypeCode = "MD",
                IsMain = true,
                IsAutoComplete = true,
                AutoCompleteSearchWindow = false,
                EnableSecurity = false,
                DefaultText = "Border Type",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region QuoteTemplateSetting

            QuoteTemplateSettingObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                ObjectTableName = "QuoteTemplateSetting",
                DBTableName = "QuoteTemplateSettings",
                ObjectTableSingular = "Quote Template Setting",
                ObjectTablePlural = "Quote TemplateS Settings",
                Tenant = 0,
                KeyPropertyPath = "Id",
                CacheOnClient = false,
                EditableFromAutoCompleteWindow = false,
                ObjectTableTypeCode = "MD",
                IsMain = true,
                IsAutoComplete = true,
                AutoCompleteSearchWindow = false,
                EnableSecurity = false,
                DefaultText = "Quote TemplateS Setting",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region QuoteTemplateTextCode

            QuoteTemplateTextCodeObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                ObjectTableName = "QuoteTemplateTextCode",
                DBTableName = "QuoteTemplateTextCodes",
                ObjectTableSingular = "Quote Template Text Code",
                ObjectTablePlural = "Quote Template Text Codes",
                Tenant = 0,
                KeyPropertyPath = "Id",
                CacheOnClient = false,
                EditableFromAutoCompleteWindow = false,
                ObjectTableTypeCode = "MD",
                IsMain = true,
                IsAutoComplete = true,
                AutoCompleteSearchWindow = false,
                EnableSecurity = false,
                DefaultText = "Quote Template Text Code",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);


            #endregion

            #region QuoteTemplateTableDesign

            QuoteTemplateTableDesignObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                ObjectTableName = "QuoteTemplateTableDesign",
                DBTableName = "QuoteTemplateTableDesigns",
                ObjectTableSingular = "Quote Template Table Design",
                ObjectTablePlural = "Quote Template Table Designs",
                Tenant = 0,
                KeyPropertyPath = "Id",
                CacheOnClient = false,
                EditableFromAutoCompleteWindow = false,
                ObjectTableTypeCode = "MD",
                IsMain = true,
                IsAutoComplete = true,
                AutoCompleteSearchWindow = false,
                EnableSecurity = false,
                DefaultText = "Quote Template Table Design",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);

            //QuoteTemplateTableDesignObject


            #endregion

            #region QuoteTemplateTextDesign


            QuoteTemplateTextDesignObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                ObjectTableName = "QuoteTemplateTextDesign",
                DBTableName = "QuoteTemplateTextDesigns",
                ObjectTableSingular = "Quote Template Text Design",
                ObjectTablePlural = "Quote Template Text Designs",
                Tenant = 0,
                KeyPropertyPath = "Id",
                CacheOnClient = false,
                EditableFromAutoCompleteWindow = false,
                ObjectTableTypeCode = "MD",
                IsMain = true,
                IsAutoComplete = true,
                AutoCompleteSearchWindow = false,
                EnableSecurity = false,
                DefaultText = "Quote Template Text Design",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);


            #endregion

            #region ExternalSystemsTablesCode

            ExternalSystemsTablesCodeObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                ObjectTableName = "ExternalSystemsTablesCode",
                DBTableName = "ExternalSystemsTablesCodes",
                ObjectTableSingular = "External Systems Tables Code",
                ObjectTablePlural = "External Systems Tables Codes",
                Tenant = 0,
                KeyPropertyPath = "Id",
                CacheOnClient = false,
                EditableFromAutoCompleteWindow = false,
                ObjectTableTypeCode = "MD",
                IsMain = true,
                EnableSecurity = false,
                IsAutoComplete = true,
                LookUp1 = "Name",
                AutoCompleteSearchWindow = true,
                SortingByObjectField = "Name",
                DependencyFilter1 = "LogitudeTable",
                DefaultText = "External Systems Tables Code",
                ClientModuleName = "Invoice",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region VatUniqueTypeObject
            VatUniqueTypeObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "VAT Unique Type",
                ObjectTableName = "VatUniqueType",
                ObjectTablePlural = "VAT Unique Types",
                ObjectTableSingular = "VAT Unique Type",
                DBTableName = "VatUniqueTypes",
                Tenant = 0,
                KeyPropertyPath = "Code",
                IsClosed = true,
                IsMain = false,
                IsAutoComplete = true,
                CacheOnClient = true,
                ObjectTableTypeCode = "MD",
                LookUp1 = "Name",
                ClientModuleName = "Common",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region VatMandatoryTypeObject
            VatMandatoryTypeObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "VAT Mandatory Type",
                ObjectTableName = "VatMandatoryType",
                ObjectTablePlural = "VAT Mandatory Types",
                ObjectTableSingular = "VAT Mandatory Type",
                DBTableName = "VatMandatoryTypes",
                Tenant = 0,
                KeyPropertyPath = "Code",
                IsClosed = true,
                IsMain = false,
                IsAutoComplete = true,
                CacheOnClient = true,
                ObjectTableTypeCode = "MD",
                LookUp1 = "Name",
                ClientModuleName = "Common",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region QuoteClosingReason
            QuoteClosingReasonObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Quote Closing Reason",
                ObjectTableName = "QuoteClosingReason",
                ObjectTablePlural = "Quote Closing Reasons",
                ObjectTableSingular = "Quote Closing Reason",
                DBTableName = "QuoteClosingReasons",
                Tenant = 0,
                KeyPropertyPath = "Code",
                LookUp1 = "Name",
                IsClosed = true,
                CacheOnClient = true,
                IsMain = true,
                IsAutoComplete = true,
                ObjectTableTypeCode = "MD",
                ClientModuleName = "Quote",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region CustomerSalesNoteObject
            CustomerSalesNoteObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Customer Sales Note",
                ObjectTableName = "CustomerSalesNote",
                ObjectTablePlural = "Customer Sales Notes",
                ObjectTableSingular = "Customer Sales Note",
                DBTableName = "CustomerSalesNotes",
                Tenant = 0,
                KeyPropertyPath = "Id",
                IsMain = false,
                IsComposition = true,
                EnableSecurity = true,
                ObjectTableTypeCode = "BR",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region AccountingSystemsSetting
            AccountingSystemsSettingObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                ObjectTableName = "AccountingSystemsSetting",
                DBTableName = "AccountingSystemsSettings",
                ObjectTableSingular = "Accounting Systems Setting",
                ObjectTablePlural = "Accounting Systems Settings",
                Tenant = 0,
                KeyPropertyPath = "Id",
                CacheOnClient = false,
                ObjectTableTypeCode = "MD",
                IsMain = true,
                EnableSecurity = false,
                DefaultText = "Accounting Systems Setting",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region AccountingSystemsSyncStatus

            AccountingSystemsSyncStatusObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                ObjectTableName = "AccountingSystemsSyncStatus",
                DBTableName = "AccountingSystemsSyncStatuses",
                ObjectTableSingular = "Accounting Systems Sync Status",
                ObjectTablePlural = "Accounting Systems Sync Statuses",
                Tenant = 0,
                KeyPropertyPath = "Id",
                CacheOnClient = false,
                ObjectTableTypeCode = "MD",
                IsMain = true,
                EnableSecurity = false,
                DefaultText = "Accounting Systems Sync Status",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);

            #endregion

            #region CustomerStatus
            CustomerStatusObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Customer Status",
                ObjectTableName = "CustomerStatus",
                ObjectTablePlural = "Customer Status",
                ObjectTableSingular = "Customer Status",
                DBTableName = "CustomerStatus",
                Tenant = 0,
                KeyPropertyPath = "Code",
                CacheOnClient = true,
                LookUp1 = "Code",
                LookUp2 = "Name",
                AutoCompleteSearchWindow = true,
                IsClosed = true,
                IsMain = true,
                IsAutoComplete = true,
                ObjectTableTypeCode = "MD",
                ClientModuleName = "Common"
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region Shipment Commodity
            ShipmentCommodityObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Shipment Commodity",
                ObjectTableName = "ShipmentCommodity",
                ObjectTablePlural = "Shipment Commodities",
                ObjectTableSingular = "Shipment Commodity",
                DBTableName = "ShipmentCommodities",
                Tenant = 0,
                KeyPropertyPath = "Id",
                IsMain = true,
                EnableSecurity = true,
                IsComposition = true,
                ObjectTableTypeCode = "BR",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region CommodityPackage
            CommodityPackageObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Commodity Package",
                ObjectTableName = "CommodityPackage",
                ObjectTablePlural = "CommodityPackages",
                ObjectTableSingular = "Commodity Package",
                DBTableName = "CommodityPackages",
                Tenant = 0,
                KeyPropertyPath = "Id",
                IsMain = true,
                EnableSecurity = true,
                IsComposition = true,
                ObjectTableTypeCode = "BR",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region BusinessUnit
            BusinessUnitObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Business Unit",
                ObjectTableName = "BusinessUnit",
                ObjectTablePlural = "Business Units",
                ObjectTableSingular = "Business Unit",
                DBTableName = "BusinessUnits",
                Tenant = 0,
                KeyPropertyPath = "Id",
                IsMain = true,
                EnableSecurity = true,
                ObjectTableTypeCode = "MD",
                LookUp1 = "Name",
                IsAutoComplete = true,
                IsNewWizard = true,
                ClientModuleName = "Common",
                NewWizardControlName = "Simplog.Infrastructure.NewBusinessUnitCommand",
                NewWizardComponentPath = "./Common/Components/Maintenance/BusinessUnit/NewBusinessUnitComponent",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region FeatureAccessLevel
            FeatureAccessLevelObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Feature Access Level",
                ObjectTableName = "FeatureAccessLevel",
                ObjectTablePlural = "Feature Access Levels",
                ObjectTableSingular = "Feature Access Level",
                DBTableName = "FeatureAccessLevels",
                Tenant = 0,
                KeyPropertyPath = "Code",
                ObjectTableTypeCode = "MD",
                IsClosed = true,
                CacheOnClient = true,
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region SpecialServicesType
            SpecialServicesTypeObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Special Services Type",
                ObjectTableName = "SpecialServicesType",
                ObjectTablePlural = "Special Services Types",
                ObjectTableSingular = "Special Services Type",
                DBTableName = "SpecialServicesTypes",
                Tenant = 0,
                KeyPropertyPath = "Id",
                ObjectTableTypeCode = "MD",
                IsMain = true,
                EnableSecurity = true,
                LookUp1 = "Code",
                LookUp2 = "EnglishName",
                ClientModuleName = "Shipment",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region EmailAlertSetting
            EmailAlertsObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Email Alert Settings",
                ObjectTableName = "EmailAlertSetting",
                ObjectTablePlural = "Email Alert Settings",
                ObjectTableSingular = "Email Alert Setting",
                DBTableName = "EmailAlertSettings",
                Tenant = 0,
                KeyPropertyPath = "Id",
                IsMain = true,
                EnableSecurity = true,
                ObjectTableTypeCode = "MD",

            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region Region
            RegionObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Region",
                ObjectTableName = "Region",
                ObjectTablePlural = "Regions",
                ObjectTableSingular = "Region",
                DBTableName = "Regions",
                Tenant = 0,
                KeyPropertyPath = "Id",
                CacheOnClient = true,
                EditableFromAutoCompleteWindow = false,
                ObjectTableTypeCode = "MD",
                IsMain = true,
                IsClosed = false,
                EnableSecurity = true,
                IsAutoComplete = true,
                LookUp1 = "Name",
                AutoCompleteSearchWindow = false,
                SortingByObjectField = "Name",
                ClientModuleName="Common",


            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region PaymentCurrency
            PaymentCurrencyObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Payment Currency",
                ObjectTableName = "PaymentCurrency",
                ObjectTablePlural = "Payment Currencies",
                ObjectTableSingular = "Payment Currency",
                DBTableName = "PaymentCurrencies",
                Tenant = 0,
                KeyPropertyPath = "Code",
                IsClosed = true,
                IsMain = false,
                IsAutoComplete = true,
                CacheOnClient = true,
                ObjectTableTypeCode = "MD",
                LookUp1 = "Name",
                ClientModuleName = "Infrastructure",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region MessagingStock
            MessagingStockObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Messaging Stock",
                ObjectTableName = "MessagingStock",
                ObjectTablePlural = "Messaging Stocks",
                ObjectTableSingular = "Messaging Stock",
                DBTableName = "MessagingStocks",
                Tenant = 0,
                KeyPropertyPath = "Id",
                IsMain = true,
                EnableSecurity = true,
                ObjectTableTypeCode = "MD",
                IsNewWizard = true,
                NewWizardControlName = "Simplog.ShipmentLib.NewMessagingStockCommand",
                NewWizardComponentPath = "./ShipmentModules/ShipmentStock/Components/MessagingStock/StockNewWizardComponent",
                ClientModuleName = "Shipment",
                HasMenuButtons = true,
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region StockUsageHistory
            MessagingStockUsageHistoryObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Messaging Stock Usage History",
                ObjectTableName = "MessagingStockUsageHistory",
                ObjectTablePlural = "Messaging Stock Usage Histories",
                ObjectTableSingular = "Messaging Stock Usage History",
                DBTableName = "MessagingStockUsageHistories",
                Tenant = 0,
                KeyPropertyPath = "Id",
                IsMain = true,
                EnableSecurity = true,
                IsComposition = true,
                ObjectTableTypeCode = "BR",
                SortingByObjectField = "TenantNumber",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region CustomerSize
            CustomerSizeObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                ObjectTableName = "CustomerSize",
                DBTableName = "CustomerSizes",
                ObjectTableSingular = "Customer Size",
                ObjectTablePlural = "Customer Sizes",
                IsNewWizard = false,
                LookUp1 = "Name",
                KeyPropertyPath = "Id",
                IsClosed = false,
                CacheOnClient = true,
                EnableAddFromLOV = true,
                IsMain = true,
                IsAutoComplete = true,
                EnableEditFromLOV = true,
                SortingByObjectField = "Order",
                InActive = false,
                IsSaveButtonVisible = true,
                EnableSecurity = true,
                ObjectTableTypeCode = "MD",
                MaxNumberOfCustomFields = 0,
                DefaultText = "Customer Size",
                ClientModuleName = "Common"
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region QuoteStage
            QuoteStageObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Quote Stage",
                ObjectTableName = "QuoteStage",
                ObjectTablePlural = "Quote Stages",
                ObjectTableSingular = "Quote Stage",
                DBTableName = "QuoteStages",
                Tenant = 0,
                KeyPropertyPath = "Id",
                CacheOnClient = true,
                IsAutoComplete = true,
                LookUp1 = "Name",
                IsMain = true,
                EnableSecurity = true,
                ObjectTableTypeCode = "MD",
                ClientModuleName = "Quote",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region QuoteRating
            QuoteRatingObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Quote Rating",
                ObjectTableName = "QuoteRating",
                ObjectTablePlural = "Quote Ratings",
                ObjectTableSingular = "Quote Rating",
                DBTableName = "QuoteRatings",
                Tenant = 0,
                KeyPropertyPath = "Code",
                IsMain = false,
                IsClosed = true,
                CacheOnClient = true,
                IsAutoComplete = true,
                LookUp1 = "Name",
                ObjectTableTypeCode = "MD",
                ClientModuleName = "Quote",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region DistributorTable
            DistributorObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Distributor",
                ObjectTableName = "Distributor",
                ObjectTablePlural = "Distributors",
                ObjectTableSingular = "Distributor",
                DBTableName = "Distributors",
                Tenant = 0,
                KeyPropertyPath = "Code",
                IsMain = true,
                IsClosed = false,
                CacheOnClient = false,
                IsAutoComplete = true,
                LookUp1 = "EnglishName",
                ObjectTableTypeCode = "MD",
                EnableSecurity = true,
                ClientModuleName = "Common",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region AutomationTable
            AutomationObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Automation",
                ObjectTableName = "Automation",
                ObjectTablePlural = "Automations",
                ObjectTableSingular = "Automation",
                DBTableName = "Automations",
                Tenant = 0,
                KeyPropertyPath = "Id",
                IsMain = true,
                IsClosed = false,
                CacheOnClient = false,
                IsAutoComplete = true,
                LookUp1 = "Name",
                ObjectTableTypeCode = "MD",
                EnableSecurity = true,
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion
           
            #region AccountingInformationIdentifier
            AccountingInformationIdentifierObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Accounting Information Identifier",
                ObjectTableName = "AccountingInformationIdentifier",
                ObjectTablePlural = "Accounting Information Identifiers",
                ObjectTableSingular = "Accounting Information Identifier",
                DBTableName = "AccountingInformationIdentifiers",
                Tenant = 0,
                KeyPropertyPath = "Code",
                LookUp1 = "Code",
                IsAutoComplete = true,
                AutoCompleteSearchWindow = false,
                IsClosed = true,
                CacheOnClient = true,
                IsMain = true,
                ObjectTableTypeCode = "MD",
                ClientModuleName = "Shipment",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region ComputingPartner
            ComputingPartnerObjectTable = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Computing Partner",
                ObjectTableName = "ComputingPartner",
                ObjectTablePlural = "Computing Partners",
                ObjectTableSingular = "Computing Partner",
                DBTableName = "ComputingPartners",
                KeyPropertyPath = "Id",
                Tenant = 0,
                IsMain = true,
                EnableSecurity = true,
                ObjectTableTypeCode = "MD",
                IsNewWizard = true,
                NewWizardControlName = "Simplog.Infrastructure.NewComputingPartnerCommand",
                NewWizardComponentPath = "./InfrastructureModules/InfrastructureComputingPartner/Components/NewComputingPartnerConmponent",
                ClientModuleName = "Common",
                LookUp1 = "Code",
                LookUp2 = "Name",

            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region ComputingPartnerCode
            ComputingPartnerCodeObjectTable = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Computing Partner Code",
                ObjectTableName = "ComputingPartnerCode",
                ObjectTablePlural = "Computing Partner Codes",
                ObjectTableSingular = "Computing Partner Code",
                DBTableName = "ComputingPartnerCodes",
                Tenant = 0,
                KeyPropertyPath = "Id",
                IsMain = true,
                EnableSecurity = false,
                IsComposition = false,
                ObjectTableTypeCode = "BR",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region ComputingPartnerTranslation
            ComputingPartnerTranslationObjectTable = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Computing Partner Translation",
                ObjectTableName = "ComputingPartnerTranslation",
                ObjectTablePlural = "Computing Partner Translations",
                ObjectTableSingular = "Computing Partner Translation",
                DBTableName = "ComputingPartnerTranslations",
                Tenant = 0,
                KeyPropertyPath = "Id",
                IsMain = true,
                EnableSecurity = false,
                IsComposition = false,
                ObjectTableTypeCode = "BR",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region ComputingPartnerTable
            ComputingPartnerTableObjectTable = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Computing Partner Table",
                ObjectTableName = "ComputingPartnerTable",
                ObjectTablePlural = "Computing Partner Tables",
                ObjectTableSingular = "Computing Partner Table",
                DBTableName = "ComputingPartnerTables",
                Tenant = 0,
                IsMain = true,
                EnableSecurity = false,
                IsComposition = false,
                ObjectTableTypeCode = "BR",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region AWBMessagesCCSType
            AWBMessagesCCSTypeObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                ObjectTableName = "AWBMessagesCCSType",
                DBTableName = "AWBMessagesCCSTypes",
                ObjectTableSingular = "AWB Messages CCS Type",
                ObjectTablePlural = "AWB Messages CCS Types",
                LookUp1 = "Name",
                KeyPropertyPath = "Code",
                Tenant = 0,
                IsMain = true,
                IsClosed = true,
                CacheOnClient = true,
                IsAutoComplete = true,
                SortingByObjectField = "Name",
                ObjectTableTypeCode = "MD",
                DefaultText = "AWB Messages CCS Type",
                ClientModuleName = "Infrastructure",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region DocumentFolder
            DocumentFoldersObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Document Folder",
                ObjectTableName = "DocumentFolder",
                ObjectTablePlural = "Document Folders",
                ObjectTableSingular = "Document Folder",
                DBTableName = "DocumentFolders",
                Tenant = 0,
                KeyPropertyPath = "Id",
                // CacheOnClient = true,
                EditableFromAutoCompleteWindow = true,
                IsMain = true,
                ObjectTableTypeCode = "MD",
                EnableSecurity = true,
                LookUp1 = "Code",
                LookUp2 = "EnglishName",
                IsNewWizard = true,
                NewWizardControlName = "Simplog.Infrastructure.Views.Documents.DocumentFolders.NewDocumentFolderCommand",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region ManifestStatus
            ManifestStatusObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Manifest Status",
                ObjectTableName = "ManifestStatus",
                DBTableName = "ManifestStatus",
                ObjectTableSingular = "Manifest Status",
                ObjectTablePlural = "Manifest Status",
                LookUp1 = "Name",
                KeyPropertyPath = "Code",
                Tenant = 0,
                IsMain = true,
                IsClosed = true,
                CacheOnClient = true,
                IsAutoComplete = true,
                SortingByObjectField = "Name",
                ObjectTableTypeCode = "MD",
                ClientModuleName = "Shipment",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region ManifestStatus
            CustomsTransmissionsStatusObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Customs Transmissions Status",
                ObjectTableName = "CustomsTransmissionsStatus",
                DBTableName = "CustomsTransmissionsStatus",
                ObjectTableSingular = "Customs Transmissions Status",
                ObjectTablePlural = "Customs Transmissions Status",
                LookUp1 = "Name",
                KeyPropertyPath = "Code",
                Tenant = 0,
                IsClosed = true,
                CacheOnClient = true,
                IsAutoComplete = true,
                SortingByObjectField = "Name",
                ObjectTableTypeCode = "MD",
                ClientModuleName = "Shipment",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region AWBAdditionalHandlingInfoObject
            AWBAdditionalHandlingInfoObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "AWB Additional Handling Info",
                ObjectTableName = "AWBAdditionalHandlingInfo",
                DBTableName = "AWBAdditionalHandlingInfos",
                ObjectTableSingular = "AWB Additional Handling Info",
                ObjectTablePlural = "AWB Additional Handling Infos",
                LookUp1 = "Code",
                KeyPropertyPath = "Code",
                Tenant = 0,
                IsMain = true,
                IsClosed = false,
                CacheOnClient = true,
                IsAutoComplete = true,
                EnableSecurity = true,
                SortingByObjectField = "Code",
                ObjectTableTypeCode = "MD",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region DocumentTypeCategory
            DocumentTypeCategoryObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Document Type Category",
                ObjectTableName = "DocumentTypeCategory",
                ObjectTablePlural = "DocumentTypeCategories",
                ObjectTableSingular = "DocumentTypeCategory",
                DBTableName = "DocumentTypeCategories",
                LookUp1 = "Code",
                LookUp2 = "Name",
                ClientModuleName = "Common",
                Tenant = 0,
                KeyPropertyPath = "Code",
                CacheOnClient = true,
                IsClosed = true,
                EditableFromAutoCompleteWindow = true,
                IsAutoComplete = true,
                IsMain = true,
                ObjectTableTypeCode = "MD",
               
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region InboundEmail
            InboundEmailObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Inbound Email",
                ObjectTableName = "InboundEmail",
                ObjectTablePlural = "InboundEmails",
                ObjectTableSingular = "InboundEmail",
                DBTableName = "InboundEmails",
                KeyPropertyPath = "Id",
                EditableFromAutoCompleteWindow = true,
                IsNewWizard = true,
                NewWizardControlName = "Logitude.CRM.Views.NewEntity.NewInboundEmailCommand",
                Tenant = 0,
                IsMain = true,
                EnableSecurity = true,
                ObjectTableTypeCode = "MD",
                ClientModuleName = "Infrastructure",
                NewWizardComponentPath = "./CRM/Components/NewEntity/InboundEmail/NewInboundEmailComponent",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region InboundEmailLine
            InboundEmailLineObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Inbound Email Line",
                ObjectTableName = "InboundEmailLine",
                ObjectTablePlural = "InboundEmailLines",
                ObjectTableSingular = "InboundEmailLine",
                DBTableName = "InboundEmailLines",
                Tenant = 0,
                KeyPropertyPath = "Id",
                IsMain = false,
                IsComposition = true,
                EnableSecurity = true,
                ObjectTableTypeCode = "MD",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region BusinessHourObject
            BusinessHourObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Business Hour",
                ObjectTableName = "BusinessHour",
                ObjectTablePlural = "Business Hours",
                ObjectTableSingular = "Business Hour",
                DBTableName = "BusinessHours",
                KeyPropertyPath = "Id",
                Tenant = 0,
                IsMain = true,
                EnableSecurity = true,
                ObjectTableTypeCode = "MD",
                IsAutoComplete = true,
                LookUp1 = "Name",
                ClientModuleName = "Infrastructure",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion 

            #region BusinessHoursHolidayObject
            BusinessHoursHolidayObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Business Hours Holiday",
                ObjectTableName = "BusinessHoursHoliday",
                ObjectTablePlural = "BusinessHoursHolidays",
                ObjectTableSingular = "BusinessHoursHoliday",
                DBTableName = "BusinessHoursHolidays",
                KeyPropertyPath = "Id",
                Tenant = 0,
                IsMain = true,
                EnableSecurity = true,
                ObjectTableTypeCode = "MD",
                ClientModuleName = "Infrastructure",
                IsComposition = true,
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);


            #endregion
            
            #region MappedShipmentDirections
            MappedShipmentDirectionsObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Mapped Shipment Directions",
                ObjectTableName = "MappedShipmentDirections",
                ObjectTablePlural = "MappedShipmentDirections",
                ObjectTableSingular = "MappedShipmentDirection",
                DBTableName = "MappedShipmentDirections",
                KeyPropertyPath = "ShipmentDirectionId",
                EditableFromAutoCompleteWindow = true,
                Tenant = 0,
                IsMain = true,
                EnableSecurity = true,
                ObjectTableTypeCode = "MD",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);


            #endregion
         
            #region APILogsObject
            APILogsObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "API Logs",
                ObjectTableName = "APILogs",
                ObjectTablePlural = "API Logs", 
                ObjectTableSingular = "API Log",
                DBTableName = "APILogs",
                KeyPropertyPath = "Id",
                EditableFromAutoCompleteWindow = true,
                Tenant = 0,
                IsMain = true,
                EnableSecurity = true,
                ObjectTableTypeCode = "MD",
                ClientModuleName = "Infrastructure",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);


            #endregion
         
            #region APILogsDataObject
            APILogsDataObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "API Logs Data",
                ObjectTableName = "APILogsData",
                ObjectTablePlural = "APILogsData",
                ObjectTableSingular = "APILogsData",
                DBTableName = "APILogsData",
                KeyPropertyPath = "Id",
                EditableFromAutoCompleteWindow = true,
                Tenant = 0,
                IsMain = true,
                EnableSecurity = true,
                ObjectTableTypeCode = "MD",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);


            #endregion

            #region BatchServicesLog
            BatchServicesLogObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Batch Services Log",
                ObjectTableName = "BatchServicesLog",
                ObjectTablePlural = "Batch Services Log",
                ObjectTableSingular = "BatchServicesLog",
                DBTableName = "BatchServicesLog",
                Tenant = 0,
                IsMain = true,
                ObjectTableTypeCode = "MD",
                EnableSecurity = true,
                ClientModuleName = "Infrastructure",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion
         
            #region BatchServicesDefinitions
            BatchServicesDefinitionsObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Batch Services Definitions",
                ObjectTableName = "BatchServicesDefinition",
                ObjectTablePlural = "Batch Services Definitions",
                ObjectTableSingular = "Batch Services Definition",
                DBTableName = "BatchServicesDefinitions",
                Tenant = 0,
                IsMain = true,
                ObjectTableTypeCode = "MD",
                EnableSecurity = true,
                ClientModuleName = "Infrastructure",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region TenantTypeObject
            TenantTypeObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Tenant Type",
                ObjectTableName = "TenantType",
                ObjectTablePlural = "Tenant Types",
                ObjectTableSingular = "Tenant Type",
                DBTableName = "TenantTypes",
                Tenant = 0,
                KeyPropertyPath = "Code",
                IsClosed = true,
                IsMain = false,
                IsAutoComplete = true,
                CacheOnClient = true,
                ObjectTableTypeCode = "MD",
                LookUp1 = "Name",
                ClientModuleName = "Infrastructure",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region QueueMessageMoreDetailsObject
            QueueMessageMoreDetailsObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Queue Message More Details",
                ObjectTableName = "QueueMessageMoreDetails",
                ObjectTablePlural = "Queue Messages More Details",
                ObjectTableSingular = "QueueMessageMoreDetails",
                DBTableName = "QueueMessageMoreDetails",
                Tenant = 0,
                IsMain = true,
                ObjectTableTypeCode = "MD",
                EnableSecurity = true,
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region Participant
            ParticipantObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Participant",
                ObjectTableName = "Participant",
                ObjectTablePlural = "Participants",
                ObjectTableSingular = "Participant",
                DBTableName = "Participants",
                Tenant = 0,
                KeyPropertyPath = "Id",
                EditableFromAutoCompleteWindow = true,
                IsNewWizard = true,
                NewWizardControlName = "Simplog.FreightLib.NewParticipantCommand",
                IsMain = true,
                EnableSecurity = true,
                ObjectTableTypeCode = "MD",
                ClientModuleName="Common",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region CustomerTenantAccessCardsBatchObject
            CustomerTenantAccessCardsBatchObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Customer Tenant Access Cards Batch",
                ObjectTableName = "CustomerTenantAccessCardsBatch",
                ObjectTablePlural = "Customer Tenant Access Cards Batches",
                ObjectTableSingular = "Customer Tenant Access Cards Batch",
                DBTableName = "CustomerTenantAccessCardsBatches",
                Tenant = 0,
                IsMain = true,
                ObjectTableTypeCode = "MD",
                EnableSecurity = true,
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region AirlineStatistics
            AirlineStatisticsObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Airline Statistics",
                ObjectTableName = "AirlineStatistics",
                ObjectTablePlural = "Airline Statistics",
                ObjectTableSingular = "Airline Statistics",
                DBTableName = "AirlineStatistics",
                Tenant = 0,
                KeyPropertyPath = "Id",
                IsMain = true,
                EnableSecurity = true,
                ObjectTableTypeCode = "MD",
                ClientModuleName="Common",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region ApiCredintialsObject
            ApiCredintialsObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "API Credentials",
                ObjectTableName = "ApiCredintials",
                ObjectTablePlural = "API Credentials",
                ObjectTableSingular = "API Credentials",
                DBTableName = "ApiCredintials",
                Tenant = 0,
                IsMain = true,
                ObjectTableTypeCode = "MD",
                EnableSecurity = true,
                IsNewWizard = true,
                NewWizardControlName = "Simplog.Infrastructure.Views.ApiCredintials.AddEditApiCredintialsControl",
                ClientModuleName = "Infrastructure",
                NewWizardComponentPath = "./InfrastructureModules/InfrastructureOthers/Components/ApiCredintials/ApiCredintialsComponent",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region AWBDescriptionOfGoods
            AWBDescriptionOfGoodsObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                ObjectTableName = "AWBDescriptionOfGoods",
                DBTableName = "AWBDescriptionOfGoods",
                ObjectTableSingular = "AWB Description of Goods",
                ObjectTablePlural = "AWB Description of Goods",
                LookUp1 = "Name",
                DependencyFilter1 = "AirlineCode",
                KeyPropertyPath = "Id",
                IsClosed = true,
                CacheOnClient = true,
                IsAutoComplete = true,
                SortingByObjectField = "Name",
                ObjectTableTypeCode = "MD",
                MaxNumberOfCustomFields = 0,
                DefaultText = "AWB Description of Goods",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region HybridTenantState
            HybridTenantStateObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Hybrid Tenant State",
                ObjectTableName = "HybridTenantState",
                ObjectTablePlural = "Hybrid Tenant State",
                ObjectTableSingular = "HybridTenantState",
                DBTableName = "HybridTenantState",
                Tenant = 0,
                IsMain = true,
                ObjectTableTypeCode = "MD",
                EnableSecurity = true,
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region OceanInsightsStatusesObject
            OceanInsightsStatusesObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Ocean Insights Statuses",
                ObjectTableName = "OceanInsightsStatuses",
                ObjectTablePlural = "Ocean Insights Statuses",
                ObjectTableSingular = "Ocean Insights Statuses",
                DBTableName = "OceanInsightsStatuses",
                Tenant = 0,
                IsMain = true,
                ObjectTableTypeCode = "MD",
                EnableSecurity = true,
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region OceanInsightsRequestObject
            OceanInsightsRequestObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Ocean Insights Request",
                ObjectTableName = "OceanInsightsRequest",
                ObjectTablePlural = "Ocean Insights Requests",
                ObjectTableSingular = "Ocean Insights Request",
                DBTableName = "OceanInsightsRequests",
                Tenant = 0,
                IsMain = true,
                ObjectTableTypeCode = "MD",
                EnableSecurity = true,
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region VatFormatTypeObject
            VatFormatTypeObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "VAT Format Type",
                ObjectTableName = "VatFormatType",
                ObjectTablePlural = "VAT Format Types",
                ObjectTableSingular = "VAT Format Type",
                DBTableName = "VatFormatTypes",
                Tenant = 0,
                KeyPropertyPath = "Code",
                IsClosed = true,
                IsMain = false,
                IsAutoComplete = true,
                CacheOnClient = true,
                ObjectTableTypeCode = "MD",
                LookUp1 = "Name",
                ClientModuleName = "Common",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region HelpResource
            HelpResourceObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Help Center",
                ObjectTableName = "HelpResource",
                ObjectTablePlural = "Help Center",
                ObjectTableSingular = "Help Center",
                DBTableName = "HelpResources",
                Tenant = 0,
                KeyPropertyPath = "Code",
                HasCounter = true,
                IsMain = true,
                IsRestrictable = true,
                EnableSecurity = true,
                ObjectTableTypeCode = "MD",
                AllowCustomFields = true,
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region AutomationsObject
            //AutomationsObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            //{
            //    ObjectTableName = "Automation",
            //    DBTableName = "Automations",
            //    ObjectTableSingular = "Automation",
            //    ObjectTablePlural = "Automations",
            //    Tenant = 0,
            //    LookUp1 = "Name",
            //    KeyPropertyPath = "Id",
            //    CacheOnClient = false,
            //    EditableFromAutoCompleteWindow = true,
            //    EnableSecurity = true,
            //    IsMain = true,

            //    IsAutoComplete = true,
            //    AutoCompleteSearchWindow = true,
            //    IsClosed = false,
            //    IsComposition = false,
            //    DefaultText = "Automation",
            //    ObjectTableTypeCode = "MD",
            //}, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);

            #endregion

            #region Automation Condition Object
            AutomationConditionObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Automation Condition",
                ObjectTableName = "AutomationCondition",
                ObjectTablePlural = "AutomationConditions",
                ObjectTableSingular = "Automation Condition",
                DBTableName = "AutomationConditions",
                Tenant = 0,
                KeyPropertyPath = "Code",
                HasCounter = true,
                IsMain = true,
              
                EnableSecurity = true,
                ObjectTableTypeCode = "MD",
                
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region AutomationResultEmailRecipientObject
            AutomationResultEmailRecipientObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Automation Result Email Recipient",
                ObjectTableName = "AutomationResultEmailRecipient",
                ObjectTablePlural = "AutomationResultEmailRecipients",
                ObjectTableSingular = "Automation Result Email Recipient",
                DBTableName = "AutomationResultEmailRecipients",
                Tenant = 0,
                KeyPropertyPath = "Code",
                HasCounter = true,
                IsMain = true,
                EnableSecurity = true,
                ObjectTableTypeCode = "MD",
           
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region LogitudeMessagesTransmissionLog
            LogitudeMessagesTransmissionLogObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Logitude Messages Transmission Log",
                ObjectTableName = "LogitudeMessagesTransmissionLog",
                ObjectTablePlural = "Logitude Messages Transmission Logs",
                ObjectTableSingular = "Logitude Messages Transmission Log",
                DBTableName = "LogitudeMessagesTransmissionLogs",
                Tenant = 0,
                KeyPropertyPath = "Id",
                IsMain = true,
                EnableSecurity = true,
                ObjectTableTypeCode = "MD",
                HasDocuments = true,
                ClientModuleName= "Common",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region FeaturePackageType
            FeaturePackageTypeObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Feature Package Type",
                ObjectTableName = "FeaturePackageType",
                ObjectTablePlural = "Feature Package Types",
                ObjectTableSingular = "Feature Package Type",
                DBTableName = "FeaturePackageTypes",
                LookUp1 = "Name",
                Tenant = 0,
                IsClosed = true,
                KeyPropertyPath = "Code",
                CacheOnClient = true,
                IsAutoComplete = true,
                IsMain = true,
                ObjectTableTypeCode = "MD",
                ClientModuleName = "Common",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region TenantManagementLicenseObject
            TenantManagementLicenseObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Tenant Management License",
                ObjectTableName = "TenantManagementLicense",
                ObjectTablePlural = "Tenant Management Licenses",
                ObjectTableSingular = "Tenant Management License",
                DBTableName = "TenantManagementLicenses",                
                Tenant = 0,                
                KeyPropertyPath = "Id",
                IsMain = true,
                ObjectTableTypeCode = "MD",
                ClientModuleName = "Infrastructure",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region PackageConnectedPackage
            PackageConnectedPackageObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Package Connected Package",
                ObjectTableName = "PackageConnectedPackage",
                ObjectTablePlural = "Package Connected Packages",
                ObjectTableSingular = "Package Connected Package",
                DBTableName = "PackageConnectedPackages",
                Tenant = 0,
                KeyPropertyPath = "Id",
                IsMain = false,
                IsComposition = true,
                ObjectTableTypeCode = "MD",
                ClientModuleName = "Infrastructure",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region UserLicense
            UserLicenseObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "User License",
                ObjectTableName = "UserLicense",
                ObjectTablePlural = "User Licenses",
                ObjectTableSingular = "User License",
                DBTableName = "UserLicenses",
                Tenant = 0,
                KeyPropertyPath = "Id",
                IsMain = true,
                ObjectTableTypeCode = "MD",
                ClientModuleName = "Infrastructure",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region TenantAddOn
            TenantAddOnObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Tenant Add On",
                ObjectTableName = "TenantAddOn",
                ObjectTablePlural = "Tenant Add Ons",
                ObjectTableSingular = "Tenant Add On",
                DBTableName = "TenantAddOns",
                Tenant = 0,
                KeyPropertyPath = "Id",
                IsMain = true,
                ObjectTableTypeCode = "MD",
                ClientModuleName = "Infrastructure",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region AirlineMessagingRule
            AirlineMessagingRuleObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Airline Messaging Rule",
                ObjectTableName = "AirlineMessagingRule",
                ObjectTablePlural = "Airline Messaging Rules",
                ObjectTableSingular = "Airline Messaging Rule",
                DBTableName = "AirlineMessagingRules",
                Tenant = 0,
                KeyPropertyPath = "Id",
                IsMain = true,
                EnableSecurity = true,
                ObjectTableTypeCode = "MD",
                ClientModuleName = "Common",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region TasksScheduler
            TasksSchedulerObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Tasks Scheduler",
                ObjectTableName = "TasksScheduler",
                ObjectTablePlural = "Tasks Schedulers",
                ObjectTableSingular = "Tasks Scheduler",
                DBTableName = "TasksScheduler",
                Tenant = 0,
                KeyPropertyPath = "Id",
                IsMain = true,
                EnableSecurity = true,
                ObjectTableTypeCode = "MD",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region TaskSchedulerHistory
            TaskSchedulerHistoryObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Task Scheduler History",
                ObjectTableName = "TaskSchedulerHistory",
                ObjectTablePlural = "Task Scheduler Histories",
                ObjectTableSingular = "Task Scheduler History",
                DBTableName = "TaskSchedulerHistory",
                Tenant = 0,
                KeyPropertyPath = "Id",
                IsMain = true,
                EnableSecurity = true,
                ObjectTableTypeCode = "MD",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion
            
            #region WeightUnit
            PaymentTermDateTypeObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Payment Term Date Type",
                ObjectTableName = "PaymentTermDateType",
                ObjectTablePlural = "Payment Term Date Types",
                ObjectTableSingular = "Payment Term Date Type",
                DBTableName = "PaymentTermDateTypes",
                LookUp1 = "Name",
                Tenant = 0,
                KeyPropertyPath = "Code",
                IsClosed = true,
                CacheOnClient = true,
                IsAutoComplete = true,
                IsMain = true,
                ObjectTableTypeCode = "MD",
                ClientModuleName = "Common",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region OtherParticipantIds
            OtherParticipantIdObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Other Participant Id",
                ObjectTableName = "OtherParticipantId",
                ObjectTablePlural = "Other Participant Ids",
                ObjectTableSingular = "Other Participant Id",
                DBTableName = "OtherParticipantIds",
                LookUp1 = "Code",
                Tenant = 0,
                KeyPropertyPath = "Code",
                IsClosed = true,
                CacheOnClient = true,
                IsAutoComplete = true,
                IsMain = true,
                ObjectTableTypeCode = "MD",
                ClientModuleName = "Shipment",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region ARInvoiceLineActionObject
            ARInvoiceLineActionObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "ARInvoice Line Action",
                ObjectTableName = "ARInvoiceLineAction",
                ObjectTablePlural = "ARInvoice Line Actions",
                ObjectTableSingular = "ARInvoice Line Action",
                DBTableName = "ARInvoiceLineActions",
                Tenant = 0,
                KeyPropertyPath = "Code",
                IsClosed = true,
                IsMain = true,
                IsAutoComplete = true,
                CacheOnClient = true,
                ObjectTableTypeCode = "MD",
                LookUp1 = "Name",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region TenantManagmentPrivateLabelsObject
            TenantManagmentPrivateLabelsObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Tenant Managment Private Labels",
                ObjectTableName = "TenantManagmentPrivateLabels",
                ObjectTablePlural = "Tenant Managment Private Labels",
                ObjectTableSingular = "Tenant Managment Private Label",
                DBTableName = "TenantManagmentPrivateLabels",
                Tenant = 0,
                LookUp1 = "Id",
                LookUp2 = "PrivateLabelName",
                IsAutoComplete = true,
                KeyPropertyPath = "Id",
                IsMain = true,
                EnableSecurity = true,
                ObjectTableTypeCode = "MD",
                CacheOnClient = false,
                IsNewWizard = true,
                NewWizardControlName = "Simplog.Infrastructure.Views.TenantManagmentPrivateLabels.AddEditPrivateLabelsControl",
                NewWizardComponentPath = "./InfrastructureModules/InfrastructureTenantManagement/Components/TenantManagement/AddEditPrivateLabelsComponent",
                ClientModuleName = "Infrastructure",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion
            
            #region Customer Fields Update Setting Object
            CustomerFieldsUpdateSettingObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Customer Fields Update Setting",
                ObjectTableName = "CustomerFieldsUpdateSetting",
                ObjectTablePlural = "Customer Fields Update Setting",
                ObjectTableSingular = "Customer Fields Update Setting",
                DBTableName = "CustomerFieldsUpdateSettings",
                Tenant = 0,
                KeyPropertyPath = "Id",
                HasCounter = true,
                IsMain = true,
                EnableSecurity = true,
                ObjectTableTypeCode = "MD",
                IsNewWizard = true,
                NewWizardControlName = "Simplog.Infrastructure.NewCustomerFieldsUpdateSettingCommand",
                ClientModuleName = "Common",
                NewWizardComponentPath = "./Common/Components/Maintenance/CustomerFieldsUpdateSetting/AddEditCustomerFieldsUpdateSettingComponent",

            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);

            #endregion
            
            #region CreditLimitSetting
            CreditLimitSettingObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Credit Limit Setting",
                ObjectTableName = "CreditLimitSetting",
                ObjectTablePlural = "Credit Limit Settings",
                ObjectTableSingular = "Credit Limit Setting",
                DBTableName = "CreditLimitSettings",
                ClientModuleName = "Common",
                Tenant = 0,
                KeyPropertyPath = "Id",                
                IsMain = true,
                ObjectTableTypeCode = "MD",
                EnableSecurity = true,
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion
            
            #region AgentSharedManifest

            AgentSharedManifestObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Agent Shared Manifest",
                ObjectTableName = "AgentSharedManifest",
                ObjectTablePlural = "Agent Shared Manifests",
                ObjectTableSingular = "Agent Shared Manifest",
                DBTableName = "AgentSharedManifests",
                Tenant = 0,
                KeyPropertyPath = "Id",
                CacheOnClient = false,
                EditableFromAutoCompleteWindow = true,
                IsMain = true,
                IsAutoComplete = true,
                AutoCompleteSearchWindow = true,
                EnableSecurity = true,
                ObjectTableTypeCode = "MD",
                ClientModuleName = "Common",
                SortingByObjectField = "CreateDate",
             

            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion
            
            #region SharedManifestsStatus
            SharedManifestsStatusObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Shared Manifests Status",
                ObjectTableName = "SharedManifestsStatus",
                ObjectTablePlural = "Shared Manifests Status",
                ObjectTableSingular = "Shared Manifests Status",
                DBTableName = "SharedManifestsStatus",
                LookUp1 = "StatusName",
                Tenant = 0,
                KeyPropertyPath = "StatusCode",
                IsClosed = true,
                IsMain = true,
                ObjectTableTypeCode = "MD",
                ClientModuleName = "Common",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion
            
            #region CardExternalAccountsByProduct
            CardExternalAccountsByProductObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Card External Accounts By Product",
                ObjectTableName = "CardExternalAccountsByProduct",
                ObjectTablePlural = "Card External Accounts By Products",
                ObjectTableSingular = "Card External Accounts By Product",
                DBTableName = "CardExternalAccountsByProducts",
                ClientModuleName = "Common",
                Tenant = 0,
                KeyPropertyPath = "Id",
                IsMain = true,
                ObjectTableTypeCode = "MD",
                EnableSecurity = true,
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion
            
            #region QuoteTotalVatObject
            QuoteTotalVatObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Quote Total VAT",
                ObjectTableName = "QuoteTotalVAT",
                ObjectTablePlural = "Quote Total VATs",
                ObjectTableSingular = "Quote Total VAT",
                DBTableName = "QuoteTotalVATs",
                Tenant = 0,
                KeyPropertyPath = "Id",
                IsMain = false,
                EnableSecurity = true,
                IsComposition = true,
                ObjectTableTypeCode = "BR",
                ClientModuleName = "Quote",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion
            
            #region QuoteVATsTotalObject
            QuoteVATsTotalObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Quote VATs Total",
                ObjectTableName = "QuoteVATsTotal",
                ObjectTablePlural = "Quote VATs Total",
                ObjectTableSingular = "Quote VATs Total",
                Tenant = 0,
                KeyPropertyPath = "Id",
                ObjectTableTypeCode = "BR",
                ClientModuleName = "Quote",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion
            
            #region CustomsInterface
            CustomsInterfaceObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Customs Interface",
                ObjectTableName = "CustomsInterface",
                ObjectTablePlural = "Customs Interfaces",
                ObjectTableSingular = "Customs Interface",
                DBTableName = "CustomsInterfaces",
                LookUp1 = "Name",
                Tenant = 0,
                KeyPropertyPath = "Code",
                IsClosed = true,
                CacheOnClient = true,
                IsAutoComplete = true,
                IsMain = true,
                ObjectTableTypeCode = "MD",
                ClientModuleName = "Common",
                DependencyFilter1 = "InterfaceType",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion
            
            #region CustomsInterfaceSetting
            CustomsInterfaceSettingObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Customs Interface Setting",
                ObjectTableName = "CustomsInterfaceSetting",
                ObjectTablePlural = "Customs Interface Settings",
                ObjectTableSingular = "Customs Interface Setting",
                DBTableName = "CustomsInterfaceSettings",
                ClientModuleName = "Common",
                Tenant = 0,
                KeyPropertyPath = "Tenant",
                CacheOnClient = false,
                IsMain = true,
                ObjectTableTypeCode = "MD",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region FTP Detail
            FTPDetailObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "FTP Detail",
                ObjectTableName = "FTPDetail",
                ObjectTablePlural = "FTP Details",
                ObjectTableSingular = "FTP Detail",
                DBTableName = "FTPDetails",
                Tenant = 0,
                KeyPropertyPath = "Id",
                CacheOnClient = false,
                IsMain = true,
                IsComposition = false,
                ObjectTableTypeCode = "MD",
                ClientModuleName = "Common",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region VATTypesGroupObject
            VATTypesGroupObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "VAT Types Group",
                ObjectTableName = "VATTypesGroup",
                ObjectTablePlural = "VAT Types Groups",
                ObjectTableSingular = "VAT Types Group",
                DBTableName = "VATTypesGroups",
                Tenant = 0,
                KeyPropertyPath = "Id",
                IsComposition = true,
                ObjectTableTypeCode = "BR",
                ClientModuleName = "Common",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region SATInterface
            SATInterfaceObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "SAT Interface",
                ObjectTableName = "SATInterface",
                ObjectTablePlural = "SAT Interfaces",
                ObjectTableSingular = "SAT Interface",
                DBTableName = "SATInterfaces",
                LookUp1 = "Name",
                Tenant = 0,
                KeyPropertyPath = "Code",
                IsClosed = true,
                CacheOnClient = true,
                IsAutoComplete = true,
                IsMain = true,
                ObjectTableTypeCode = "MD",
                ClientModuleName = "Invoice",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region SATInterfaceSetting
            SATInterfaceSettingObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "SAT Interface Setting",
                ObjectTableName = "SATInterfaceSetting",
                ObjectTablePlural = "SAT Interface Settings",
                ObjectTableSingular = "SAT Interface Setting",
                DBTableName = "SATInterfaceSettings",
                ClientModuleName = "Invoice",
                Tenant = 0,
                KeyPropertyPath = "Tenant",
                CacheOnClient = false,
                IsMain = true,
                ObjectTableTypeCode = "MD",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region CardExternalCodeByCurrencyObject

            CardExternalCodeByCurrencyObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Card External Code By Currency",
                ObjectTableName = "CardExternalCodeByCurrency",
                ObjectTablePlural = "Card External Code By Currencies",
                ObjectTableSingular = "Card External Code By Currency",
                DBTableName = "CardExternalCodeByCurrencies",
                ClientModuleName = "Common",
                Tenant = 0,
                KeyPropertyPath = "Id",
                IsMain = true,
                ObjectTableTypeCode = "MD",
                EnableSecurity = true,
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);

            #endregion

            #region FBLStockObject
            FBLStockObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "FBL Stock",
                ObjectTableName = "FBLStock",
                ObjectTablePlural = "FBL Stocks",
                ObjectTableSingular = "FBL Stock",
                DBTableName = "FBLStocks",
                Tenant = 0,
                KeyPropertyPath = "Id",
                IsClosed = false,
                IsMain = true,
                ObjectTableTypeCode = "MD",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region SATPaymentMethod
            SATPaymentMethodObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "SAT Payment Method",
                ObjectTableName = "SATPaymentMethod",
                ObjectTablePlural = "SAT Payment Methods",
                ObjectTableSingular = "SAT Payment Method",
                DBTableName = "SATPaymentMethods",
                Tenant = 0,
                KeyPropertyPath = "Code",
                IsClosed = true,
                IsMain = false,
                IsAutoComplete = true,
                CacheOnClient = true,
                LookUp1 = "LocalName",
                ObjectTableTypeCode = "MD",
                ClientModuleName = "Invoice",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region ContainerFollowUpObject
            ContainerFollowUpObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Container Follow Up",
                ObjectTableName = "ContainerFollowUp",
                ObjectTablePlural = "Container Follow Ups",
                ObjectTableSingular = "Container Follow Up",
                DBTableName = "ContainerFollowUps",
                Tenant = 0,
                KeyPropertyPath = "Id",
                IsMain = true,
                EnableSecurity = true,
                ObjectTableTypeCode = "MD",
                ClientModuleName = "Shipment",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region LoginPolicy
            LoginPolicyObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Login Policy",
                ObjectTableName = "LoginPolicy",
                ObjectTablePlural = "Login Policies",
                ObjectTableSingular = "Login Policy",
                DBTableName = "LoginPolicies",
                Tenant = 0,
                KeyPropertyPath = "Code",
                IsClosed = true,
                IsMain = false,
                IsAutoComplete = true,
                CacheOnClient = true,
                LookUp1 = "Name",
                ObjectTableTypeCode = "MD",
                ClientModuleName = "Common",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region TenantLoginPolicy

            TenantLoginPolicyObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Login Policy",
                ObjectTableName = "TenantLoginPolicy",
                ObjectTablePlural = "Login Policies",
                ObjectTableSingular = "Login Policy",
                DBTableName = "TenantLoginPolicies",
                Tenant = 0,
                KeyPropertyPath = "Tenant",
                CacheOnClient = false,
                EditableFromAutoCompleteWindow = true,
                IsMain = true,
                IsAutoComplete = true,
                AutoCompleteSearchWindow = true,
                EnableSecurity = true,
                ObjectTableTypeCode = "MD",
                ClientModuleName = "Common",
               
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region AgentSharedDocument

            AgentSharedDocumentObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Agent Shared Document",
                ObjectTableName = "AgentSharedDocument",
                ObjectTablePlural = "Agent Shared Documents",
                ObjectTableSingular = "Agent Shared Document",
                DBTableName = "AgentSharedDocuments",
                Tenant = 0,
                KeyPropertyPath = "Id",
                CacheOnClient = false,
                EditableFromAutoCompleteWindow = true,
                IsMain = true,
                IsAutoComplete = true,
                AutoCompleteSearchWindow = true,
                EnableSecurity = true,
                ObjectTableTypeCode = "MD",
                ClientModuleName = "Common",
                SortingByObjectField = "CreateDate",


            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region TwoFactorAuthenticationDeviceObject

            TwoFactorAuthenticationDeviceObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Two Factor Authentication Device",
                ObjectTableName = "TwoFactorAuthenticationDevice",
                ObjectTablePlural = "Two Factor Authentication Devices",
                ObjectTableSingular = "Two Factor Authentication Device",
                DBTableName = "TwoFactorAuthenticationDevices",
                Tenant = 0,
                KeyPropertyPath = "TwoFactorkey",
                CacheOnClient = false,
                EditableFromAutoCompleteWindow = true,
                IsMain = true,
                IsAutoComplete = true,
                AutoCompleteSearchWindow = true,
                EnableSecurity = true,
                ObjectTableTypeCode = "MD",
                ClientModuleName = "Common",
                SortingByObjectField = "CreateDate",

            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region OBLTypesObject
            OBLTypesObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "OBL Type",
                ObjectTableName = "OBLType",
                ObjectTablePlural = "OBL Types",
                ObjectTableSingular = "OBL Type",
                DBTableName = "OBLTypes",
                LookUp1 = "Name",
                Tenant = 0,
                KeyPropertyPath = "Code",
                IsClosed = true,
                CacheOnClient = true,
                IsAutoComplete = true,
                IsMain = true,
                ObjectTableTypeCode = "MD",
                ClientModuleName = "Shipment",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region ShipmentAssembly
            ShipmentAssemblyObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Shipment Assembly",
                ObjectTableName = "ShipmentAssembly",
                ObjectTablePlural = "Shipment Assemblies",
                ObjectTableSingular = "Shipment Assembly",
                DBTableName = "ShipmentAssemblies",
                Tenant = 0,
                KeyPropertyPath = "Id",
                IsMain = false,
                IsComposition = true,
                EnableSecurity = true,
                ObjectTableTypeCode = "BR",
                ClientModuleName = "Shipment",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region MetodoPago
            MetodoPagoObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Metodo Pago",
                ObjectTableName = "MetodoPago",
                ObjectTablePlural = "Metodo Pagos",
                ObjectTableSingular = "Metodo Pago",
                DBTableName = "MetodoPagos",
                Tenant = 0,
                KeyPropertyPath = "Code",
                IsClosed = true,
                IsMain = false,
                IsAutoComplete = true,
                CacheOnClient = true,
                LookUp1 = "Name",
                ObjectTableTypeCode = "MD",
                ClientModuleName = "Common",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region ChargesExternalAccountsByProduct
            ChargesExternalAccountsByProductObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Charges External Accounts By Product",
                ObjectTableName = "ChargesExternalAccountsByProduct",
                ObjectTablePlural = "Charges External Accounts By Products",
                ObjectTableSingular = "Charges External Accounts By Product",
                DBTableName = "ChargesExternalAccountsByProducts",
                ClientModuleName = "Common",
                Tenant = 0,
                KeyPropertyPath = "Id",
                IsMain = true,
                ObjectTableTypeCode = "MD",
                EnableSecurity = true,
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region RegistryDateTypesObject
            RegistryDateTypesObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Registry Date Type",
                ObjectTableName = "RegistryDateType",
                ObjectTablePlural = "Registry Date Types",
                ObjectTableSingular = "Registry Date Type",
                DBTableName = "RegistryDateTypes",
                LookUp1 = "Name",
                Tenant = 0,
                KeyPropertyPath = "Code",
                IsClosed = true,
                CacheOnClient = true,
                IsAutoComplete = true,
                IsMain = true,
                ObjectTableTypeCode = "MD",
                ClientModuleName = "Common",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region UsoCFDI
            UsoCFDIObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Uso CFDI",
                ObjectTableName = "UsoCFDI",
                ObjectTablePlural = "Uso CFDIs",
                ObjectTableSingular = "Uso CFDI",
                DBTableName = "UsoCFDIs",
                Tenant = 0,
                KeyPropertyPath = "Code",
                IsClosed = true,
                IsMain = false,
                IsAutoComplete = true,
                CacheOnClient = true,
                LookUp1 = "Name",
                ObjectTableTypeCode = "MD",
                ClientModuleName = "Common",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region Reports Template Object

            ReportsTemplateObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Reports Template",
                ObjectTableName = "ReportsTemplate",
                ObjectTablePlural = "ReportsTemplates",
                ObjectTableSingular = "ReportsTemplate",
                DBTableName = "ReportsTemplates",
                Tenant = 0,
                KeyPropertyPath = "Tenant",
                IsMain = true,
                IsAutoComplete = true,
                AutoCompleteSearchWindow = true,
                EnableSecurity = true,
                ObjectTableTypeCode = "MD",
                ClientModuleName = "Common",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);






            #endregion

            #region Reports Templates Version Object;

            ReportsTemplatesVersionObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Reports Templates Version",
                ObjectTableName = "ReportsTemplatesVersion",
                ObjectTablePlural = "ReportsTemplatesVersions",
                ObjectTableSingular = "ReportsTemplatesVersion",
                DBTableName = "ReportsTemplatesVersions",
                Tenant = 0,
                KeyPropertyPath = "Tenant",
                IsMain = true,
                IsAutoComplete = true,
                AutoCompleteSearchWindow = true,
                EnableSecurity = true,
                ObjectTableTypeCode = "MD",
                ClientModuleName = "Common",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);






            #endregion

            #region ShipmentCustomsMessageTypesObject
            ShipmentCustomsMessageTypesObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Shipment Customs Message Type",
                ObjectTableName = "ShipmentCustomsMessageType",
                ObjectTablePlural = "Shipment Customs Message Types",
                ObjectTableSingular = "Shipment Customs Message Type",
                DBTableName = "ShipmentCustomsMessageTypes",
                LookUp1 = "Name",
                Tenant = 0,
                KeyPropertyPath = "Code",
                IsClosed = true,
                CacheOnClient = true,
                IsAutoComplete = true,
                IsMain = true,
                ObjectTableTypeCode = "MD",
                ClientModuleName = "Shipment",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region ShipmentCustomsTransmission
            ShipmentCustomsTransmissionsObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Shipment Customs Transmission",
                ObjectTableName = "ShipmentCustomsTransmission",
                ObjectTablePlural = "Shipment Customs Transmissions",
                ObjectTableSingular = "Shipment Customs Transmission",
                DBTableName = "ShipmentCustomsTransmissions",
                Tenant = 0,
                KeyPropertyPath = "Id",
                IsMain = true,
                EnableSecurity = true,
                ObjectTableTypeCode = "MD",
                ClientModuleName = "Shipment",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region SATTransferStatus
            SATTransferStatusObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "SAT Transfer Status",
                ObjectTableName = "SATTransferStatus",
                ObjectTablePlural = "SAT Transfer Status",
                ObjectTableSingular = "SAT Transfer Status",
                DBTableName = "SATTransferStatus",
                Tenant = 0,
                KeyPropertyPath = "Code",
                IsClosed = true,
                IsMain = false,
                IsAutoComplete = true,
                CacheOnClient = true,
                LookUp1 = "Name",
                ObjectTableTypeCode = "MD",
                ClientModuleName = "Invoice",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region SATTransferStatus
            SATInvoiceStatusObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "SAT Invoice Status",
                ObjectTableName = "SATInvoiceStatus",
                ObjectTablePlural = "SAT Invoice Status",
                ObjectTableSingular = "SAT Invoice Status",
                DBTableName = "SATInvoiceStatus",
                Tenant = 0,
                KeyPropertyPath = "Code",
                IsClosed = true,
                IsMain = false,
                IsAutoComplete = true,
                CacheOnClient = true,
                LookUp1 = "Name",
                ObjectTableTypeCode = "MD",
                ClientModuleName = "Invoice",
                ServerModuleName = "InvoiceModel",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region FilingInbox
            FilingInboxObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Filing Inbox",
                ObjectTableName = "FilingInbox",
                ObjectTablePlural = "Filing Inboxes",
                ObjectTableSingular = "Filing Inbox",
                DBTableName = "FilingInboxes",
                Tenant = 0,
                KeyPropertyPath = "Id",
                ObjectTableTypeCode = "MD",
                ClientModuleName = "Common",
                IsMain = true,
                EnableSecurity = true,
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region FilingInboxAttachment
            FilingInboxAttachmentObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Filing Inbox Attachment",
                ObjectTableName = "FilingInboxAttachment",
                ObjectTablePlural = "Filing Inbox Attachments",
                ObjectTableSingular = "Filing Inbox Attachment",
                DBTableName = "FilingInboxAttachments",
                Tenant = 0,
                KeyPropertyPath = "Id",
                ObjectTableTypeCode = "MD",
                ClientModuleName = "Common",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region FilingInboxAttachmentLog
            FilingInboxAttachmentLogObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Filing Inbox Attachment Log",
                ObjectTableName = "FilingInboxAttachmentLog",
                ObjectTablePlural = "Filing Inbox Attachment Logs",
                ObjectTableSingular = "Filing Inbox Attachment Log",
                DBTableName = "FilingInboxAttachmentLogs",
                Tenant = 0,
                KeyPropertyPath = "Id",
                ObjectTableTypeCode = "MD",
                ClientModuleName = "Common",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region QuoteSetting
            QuoteSettingObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Quote Setting",
                ObjectTableName = "QuoteSetting",
                ObjectTablePlural = "Quote Settings",
                ObjectTableSingular = "Quote Setting",
                DBTableName = "QuoteSettings",
                Tenant = 0,
                KeyPropertyPath = "Id",
                ObjectTableTypeCode = "MD",
                ClientModuleName = "Quote",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region INTTRASetting
            INTTRASettingObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "INTTRA Setting",
                ObjectTableName = "INTTRASetting",
                ObjectTablePlural = "INTTRA Settings",
                ObjectTableSingular = "INTTRA Setting",
                DBTableName = "INTTRASettings",
                Tenant = 0,
                KeyPropertyPath = "Id",
                ObjectTableTypeCode = "MD",
                ClientModuleName = "Common",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region INTTRABranchRegisteredCarrier
            INTTRABranchRegisteredCarrierObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "INTTRA Branch Registered Carrier",
                ObjectTableName = "INTTRABranchRegisteredCarrier",
                ObjectTablePlural = "INTTRA Branch Registered Carriers",
                ObjectTableSingular = "INTTRA Branch Registered Carrier",
                DBTableName = "INTTRABranchRegisteredCarriers",
                Tenant = 0,
                KeyPropertyPath = "Id",
                ObjectTableTypeCode = "MD",
                ClientModuleName = "Common",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region INTTRASettingMode
            INTTRASettingModeObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "INTTRA Setting Mode",
                ObjectTableName = "INTTRASettingMode",
                ObjectTablePlural = "INTTRA Setting Modes",
                ObjectTableSingular = "INTTRA Setting Mode",
                DBTableName = "INTTRASettingModes",
                LookUp1 = "Name",
                Tenant = 0,
                KeyPropertyPath = "Code",
                IsClosed = true,
                CacheOnClient = true,
                IsAutoComplete = true,
                IsMain = true,
                ObjectTableTypeCode = "MD",
                ClientModuleName = "Common",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region DocumentFilingBackupBatchObject
            DocumentFilingBackupBatchObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Document Filing Backup Batch",
                ObjectTableName = "DocumentFilingBackupBatch",
                ObjectTablePlural = "Document Filing Backup Batches",
                ObjectTableSingular = "Document Filing Backup Batch",
                DBTableName = "DocumentFilingBackupBatches",
                Tenant = 0,
                IsMain = true,
                ObjectTableTypeCode = "MD",
                EnableSecurity = true,
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion
      
            #region DocumentFilingBackupSetting
            DocumentFilingBackupSettingObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Document Filing Backup Setting",
                ObjectTableName = "DocumentFilingBackupSetting",
                ObjectTablePlural = "Document Filing Backup Settings",
                ObjectTableSingular = "Document Filing Backup Setting",
                DBTableName = "DocumentFilingBackupSettings",
                Tenant = 0,
                KeyPropertyPath = "Tenant",
                IsMain = true,
                ObjectTableTypeCode = "MD",
                ClientModuleName = "Common",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region TemperatureUnit
            TemperatureUnitObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Temperature Unit",
                ObjectTableName = "TemperatureUnit",
                ObjectTablePlural = "Temperature Units",
                ObjectTableSingular = "Temperature Unit",
                DBTableName = "TemperatureUnits",
                LookUp1 = "Name",
                Tenant = 0,
                KeyPropertyPath = "Code",
                IsClosed = true,
                CacheOnClient = true,
                IsAutoComplete = true,
                IsMain = true,
                ObjectTableTypeCode = "MD",
                ClientModuleName = "Common",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region PickUpDeliveryTransportMode
            PickUpDeliveryTransportModeObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "PickUp Delivery Transport Mode",
                ObjectTableName = "PickUpDeliveryTransportMode",
                ObjectTablePlural = "PickUp Delivery Transport Modes",
                ObjectTableSingular = "PickUp Delivery Transport Mode",
                DBTableName = "PickUpDeliveryTransportModes",
                LookUp1 = "Name",
                Tenant = 0,
                KeyPropertyPath = "Code",
                IsClosed = true,
                CacheOnClient = true,
                IsAutoComplete = true,
                IsMain = true,
                ObjectTableTypeCode = "MD",
                ClientModuleName = "Shipment",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region INTTRADocumentTypeObject
            INTTRADocumentTypeObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "INTTRA Document Type",
                ObjectTableName = "INTTRADocumentType",
                ObjectTablePlural = "INTTRA Document Types",
                ObjectTableSingular = "INTTRA Document Type",
                DBTableName = "INTTRADocumentTypes",
                LookUp1 = "Name",
                Tenant = 0,
                KeyPropertyPath = "Code",
                IsClosed = true,
                CacheOnClient = true,
                IsAutoComplete = true,
                IsMain = true,
                ObjectTableTypeCode = "MD",
                ClientModuleName = "Shipment",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region DWHSettingObject
            DWHSettingObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "DWHSetting",
                ObjectTableName = "DWHSetting",
                ObjectTablePlural = "DWHSettings",
                ObjectTableSingular = "DWHSetting",
                DBTableName = "DWHSettings",
                Tenant = 0,
                KeyPropertyPath = "Tenant",
                IsMain = true,
                IsAutoComplete = true,
                AutoCompleteSearchWindow = true,
                EnableSecurity = true,
                ObjectTableTypeCode = "MD",
                ClientModuleName = "Common",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);

            #endregion

            #region DWObjectTable
            DWObjectTableObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "DW Object Table",
                ObjectTableName = "DWObjectTable",
                ObjectTablePlural = "DW Object Tables",
                ObjectTableSingular = "DW Object Table",
                DBTableName = "DWObjectTables",
                Tenant = 0,
                KeyPropertyPath = "Id",
                IsMain = true,
                IsAutoComplete = true,
                AutoCompleteSearchWindow = true,
                EnableSecurity = true,
                ObjectTableTypeCode = "MD",
                ClientModuleName = "Infrastructure",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);

            #endregion

            #region DWSubQueryObject
            DWSubQueryObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "DWSubQuery",
                ObjectTableName = "DWSubQuery",
                ObjectTablePlural = "DWSubQuerys",
                ObjectTableSingular = "DWSubQuery",
                DBTableName = "DWSubQueries",
                Tenant = 0,
                KeyPropertyPath = "Id",
                IsMain = true,
                IsAutoComplete = true,
                AutoCompleteSearchWindow = true,
                EnableSecurity = true,
                ObjectTableTypeCode = "MD",
                ClientModuleName = "Infrastructure",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);

            #endregion

            

            #region DWObjectTable
            DWObjectFieldObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "DW Object Field",
                ObjectTableName = "DWObjectField",
                ObjectTablePlural = "DW Object Fields",
                ObjectTableSingular = "DW Object Field",
                DBTableName = "DWObjectFields",
                Tenant = 0,
                KeyPropertyPath = "Id",
                IsMain = true,
                IsAutoComplete = true,
                AutoCompleteSearchWindow = true,
                EnableSecurity = true,
                ObjectTableTypeCode = "MD",
                ClientModuleName = "Infrastructure",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);

            #endregion

            #region ShipmentPackageHarmonize
            ShipmentPackageHarmonizeObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Shipment Package Harmonize",
                ObjectTableName = "ShipmentPackageHarmonize",
                ObjectTablePlural = "Shipment Package Harmonizes",
                ObjectTableSingular = "Shipment Package Harmonize",
                DBTableName = "ShipmentPackageHarmonizes",
                Tenant = 0,
                KeyPropertyPath = "Id",
                IsMain = false,
                IsComposition = true,
                EnableSecurity = true,
                ObjectTableTypeCode = "BR",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            #region PickUpDeliveryPackageHarmonizeObject
            PickUpDeliveryPackageHarmonizeObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "PickUp Delivery Package Harmonize",
                ObjectTableName = "PickUpDeliveryPackageHarmonize",
                ObjectTablePlural = "PickUp Delivery Package Harmonizes",
                ObjectTableSingular = "PickUp Delivery Package Harmonize",
                DBTableName = "PickUpDeliveryPackageHarmonizes",
                Tenant = 0,
                KeyPropertyPath = "Id",
                IsMain = false,
                IsComposition = true,
                EnableSecurity = true,
                ObjectTableTypeCode = "BR",
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
            #endregion

            this.ObjectContext.SaveChanges();
        }
        #endregion      
    }
}