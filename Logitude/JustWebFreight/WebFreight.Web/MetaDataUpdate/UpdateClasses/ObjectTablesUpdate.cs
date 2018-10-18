using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;

using WebFreight.Web.InfrastructureModel;
using WebFreight.Web.MetaDataUpdate.AddClasses;
using WebFreight.Web.MetaDataUpdate.DetailClasses;

namespace WebFreight.Web.MetaDataUpdate.UpdateClasses
{
    public class ObjectTablesUpdate
    {

        public static void UpdateObjectTables(Dictionary<string, ObjectTable> objectTables, Dictionary<string, TextCode> textCodes, TextCodeRepository textCodeRepository, ObjectTableRepository objectTableRepository,IWebFreightContext objectContext)
        {
            if (Testing.General.IsTesting)
            {
                AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
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

                    HasCounter = true,

                    IsRestrictable = true,
                }, objectTableRepository, textCodeRepository, objectTables, textCodes);
            }

            #region AccountingSystem
            ObjectTable accountingSystemObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "AccountingSystem",
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
            }, objectTableRepository, textCodeRepository, objectTables, textCodes);
            #endregion

            #region General

            ObjectTable generalObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "General",
                ObjectTableName = "General",
                ObjectTablePlural = "Generals",
                ObjectTableSingular = "General",
                Tenant = 0,
                KeyPropertyPath = "Id",
                IsMain = true,
                ObjectTableTypeCode = "MD",
            }, objectTableRepository, textCodeRepository, objectTables, textCodes);

            #endregion

            #region CounterDefinition

            ObjectTable counterDefinitionObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "CounterDefinition",
                ObjectTableName = "CounterDefinition",
                ObjectTablePlural = "Counter Definitions",
                ObjectTableSingular = "Counter Definition",
                DBTableName = "CounterDefinitions",
                Tenant = 0,
                KeyPropertyPath = "Id",
                IsMain = true,
                ObjectTableTypeCode = "MD",
            }, objectTableRepository, textCodeRepository, objectTables, textCodes);

            #endregion

            #region TenantObject
            ObjectTable tenantObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Tenant",
                ObjectTableName = "Tenant",
                ObjectTablePlural = "Tenants",
                ObjectTableSingular = "Tenant",
                DBTableName = "Tenants",
                Tenant = 0,
                IsMain = true,
                ObjectTableTypeCode = "MD",
            }, objectTableRepository, textCodeRepository, objectTables, textCodes);
            #endregion

            #region TenantManagementObject
            ObjectTable tenantManagementObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Tenant Management",
                ObjectTableName = "TenantManagement",
                ObjectTablePlural = "Tenant Managements",
                ObjectTableSingular = "Tenant Management",
                DBTableName = "TenantManagements",
                Tenant = 0,
                IsMain = true,
                ObjectTableTypeCode = "MD",
            }, objectTableRepository, textCodeRepository, objectTables, textCodes);
            #endregion

            #region Shipment
            ObjectTable ShipmentObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
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
            }, objectTableRepository, textCodeRepository, objectTables, textCodes);
            #endregion

            #region ShipmentPackage
            ObjectTable ShipmentPackagesObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "ShipmentPackage",
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
            }, objectTableRepository, textCodeRepository, objectTables, textCodes);
            #endregion

            #region ShipmentOrderPackageObject
            ObjectTable ShipmentOrderPackageObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "ShipmentOrderPackage",
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
            }, objectTableRepository, textCodeRepository, objectTables, textCodes);
            #endregion

            #region PickUpDeliveryFromToTypeObject
            ObjectTable PickUpDeliveryFromToTypeObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "PickUpDeliveryFromToType",
                ObjectTableName = "PickUpDeliveryFromToType",
                ObjectTablePlural = "Pick Up Delivery From To Types",
                ObjectTableSingular = "Pick Up Delivery From To Type",
                DBTableName = "PickUpDeliveryFromToTypes",
                Tenant = 0,
                KeyPropertyPath = "Code",
                IsMain = true,
                ObjectTableTypeCode = "MD",
            }, objectTableRepository, textCodeRepository, objectTables, textCodes);
            #endregion

            #region ShipmentPickUpDelivery
            ObjectTable ShipmentPickUpDeliveryObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "ShipmentPickUpDelivery",
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
            }, objectTableRepository, textCodeRepository, objectTables, textCodes);
            #endregion

            #region ShipmentPickUpDeliveryPackages
            ObjectTable ShipmentPickUpDeliveryPackagesObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "ShipmentPickUpDeliveryPackage",
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
            }, objectTableRepository, textCodeRepository, objectTables, textCodes);
            #endregion

            #region InsideShipmentPackage
            ObjectTable InsideShipmentPackagesObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "InsideShipmentPackage",
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
            }, objectTableRepository, textCodeRepository, objectTables, textCodes);
            #endregion

            #region ShipmentReceivable
            ObjectTable ShipmentReceivablesObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "ShipmentReceivable",
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
            }, objectTableRepository, textCodeRepository, objectTables, textCodes);
            #endregion

            #region ShipmentReceivableStatusObject
            ObjectTable ShipmentReceivableStatusObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "ShipmentReceivableStatus",
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
            }, objectTableRepository, textCodeRepository, objectTables, textCodes);
            #endregion

            #region ShipmentReceivableLineStatusObject
            ObjectTable ShipmentReceivableLineStatusObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "ShipmentReceivableLineStatus",
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
            }, objectTableRepository, textCodeRepository, objectTables, textCodes);
            #endregion

            #region ShipmentPayable
            ObjectTable ShipmentPayablesObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "ShipmentPayable",
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
            }, objectTableRepository, textCodeRepository, objectTables, textCodes);
            #endregion

            #region ShipmentPayableStatusObject
            ObjectTable ShipmentPayableStatusObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "ShipmentPayableStatus",
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
            }, objectTableRepository, textCodeRepository, objectTables, textCodes);
            #endregion

            #region ShipmentPayableLineStatusObject
            ObjectTable ShipmentPayableLineStatusObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "ShipmentPayableLineStatus",
                ObjectTableName = "ShipmentPayableLineStatus",
                ObjectTablePlural = "Shipment Payable Line Status",
                ObjectTableSingular = "Shipment Payable Line Status",
                DBTableName = "ShipmentPayableLineStatus",
                Tenant = 0,
                KeyPropertyPath = "Code",
                IsMain = true,
                IsClosed = true,
                ObjectTableTypeCode = "MD",
            }, objectTableRepository, textCodeRepository, objectTables, textCodes);
            #endregion

            #region ShipmentCustomerTypeObject
            ObjectTable ShipmentCustomerTypeObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "ShipmentCustomerType",
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
            }, objectTableRepository, textCodeRepository, objectTables, textCodes);
            #endregion

            #region ShipmentAWBPrintOnlyObject
            ObjectTable ShipmentAWBPrintOnlyObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "ShipmentAWBPrintOnly",
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
            }, objectTableRepository, textCodeRepository, objectTables, textCodes);
            #endregion

            #region FixedAmount
            ObjectTable FixedAmountObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "FixedAmount",
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
            }, objectTableRepository, textCodeRepository, objectTables, textCodes);
            #endregion

            #region Quote
            ObjectTable QuoteObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
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
                HasShortTitle = true,
                HasMenuButtons = true,
                HasCustomFilter = true,
                HasHelper = true,
                HasFiltersMenu = true,
                HasCustomValidator = true,
            }, objectTableRepository, textCodeRepository, objectTables, textCodes);
            #endregion

            #region QuoteType
            ObjectTable QuoteTypeObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "QuoteType",
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
            }, objectTableRepository, textCodeRepository, objectTables, textCodes);
            #endregion

            #region MarkUpType
            ObjectTable MarkUpTypeObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "MarkUpType",
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
            }, objectTableRepository, textCodeRepository, objectTables, textCodes);
            #endregion

            #region QuoteCharge
            ObjectTable QuoteChargeObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "QuoteCharge",
                ObjectTableName = "QuoteCharge",
                ObjectTablePlural = "Quote Charges",
                ObjectTableSingular = "Quote Charge",
                DBTableName = "QuoteCharges",
                Tenant = 0,
                KeyPropertyPath = "Id",
                IsComposition = true,
                EnableSecurity = true,
                ObjectTableTypeCode = "BR",
            }, objectTableRepository, textCodeRepository, objectTables, textCodes);
            #endregion

            #region QuoteSaleCharge
            ObjectTable QuoteSaleChargesObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "QuoteSaleCharge",
                ObjectTableName = "QuoteSaleCharge",
                ObjectTablePlural = "Quote Sale Charges",
                ObjectTableSingular = "Quote Sale Charge",
                Tenant = 0,
                KeyPropertyPath = "Id",
                ObjectTableTypeCode = "BR",
            }, objectTableRepository, textCodeRepository, objectTables, textCodes);
            #endregion

            #region QuoteCostCharge
            ObjectTable QuoteCostChargesObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "QuoteCostCharge",
                ObjectTableName = "QuoteCostCharge",
                ObjectTablePlural = "Quote Cost Charges",
                ObjectTableSingular = "Quote Cost Charge",
                Tenant = 0,
                KeyPropertyPath = "Id",
                ObjectTableTypeCode = "BR",
            }, objectTableRepository, textCodeRepository, objectTables, textCodes);
            #endregion

            #region QuotePriceStep
            ObjectTable QuotePriceStepObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "QuotePriceSteps",
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
            }, objectTableRepository, textCodeRepository, objectTables, textCodes);
            #endregion

            #region QuoteCustomerTypeObject
            ObjectTable QuoteCustomerTypeObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "QuoteCustomerType",
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
            }, objectTableRepository, textCodeRepository, objectTables, textCodes);
            #endregion

            #region PotentialCustomerObject
            ObjectTable PotentialCustomerObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "PotentialCustomer",
                ObjectTableName = "PotentialCustomer",
                ObjectTablePlural = "Potential Customers",
                ObjectTableSingular = "Potential Customer",
                DBTableName = "PotentialCustomers",
                Tenant = 0,
                LookUp1 = "EnglishName",
                KeyPropertyPath = "Id",
                CacheOnClient = false,
                IsMain = true,
                IsNewWizard = true,
                EnableSecurity = true,
                NewWizardControlName = "Simplog.FreightLib.NewPotentialCustomerCommand",
                ObjectTableTypeCode = "MD",
            }, objectTableRepository, textCodeRepository, objectTables, textCodes);
            #endregion

            #region DocumentTypeCustomField
            ObjectTable DocumentTypeCustomFieldObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
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
            }, objectTableRepository, textCodeRepository, objectTables, textCodes);
            #endregion

            #region DocumentTypeTemplate
            ObjectTable DocumentTypeTemplateObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Document Type Template",
                ObjectTableName = "DocumentTypeTemplate",
                ObjectTablePlural = "Document Type Templates",
                ObjectTableSingular = "Document Type Template",
                DBTableName = "DocumentTypeTemplates",

                Tenant = 0,
                IsMain = false,
                IsComposition = true,
                ObjectTableTypeCode = "MD",
            }, objectTableRepository, textCodeRepository, objectTables, textCodes);
            #endregion

            #region TemplateFormat
            ObjectTable TemplateFormatObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
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
            }, objectTableRepository, textCodeRepository, objectTables, textCodes);
            #endregion

            #region ObjectTable
            ObjectTable ObjectTableObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "ObjectTable",
                ObjectTableName = "ObjectTable",
                ObjectTablePlural = "Object Tables",
                ObjectTableSingular = "Object Table",
                DBTableName = "ObjectTables",
                Tenant = 0,
                KeyPropertyPath = "Id",
                CacheOnClient = true,
                IsMain = true,
                LookUp1 = "Name",
                IsAutoComplete = true,
                AutoCompleteSearchWindow = true,
                ObjectTableTypeCode = "MD",
            }, objectTableRepository, textCodeRepository, objectTables, textCodes);
            #endregion

            #region EntityStatus
            ObjectTable EntityStatusObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "EntityStatus",
                ObjectTableName = "EntityStatus",
                ObjectTablePlural = "Entity Status",
                ObjectTableSingular = "Entity Status",
                DBTableName = "EntityStatus",
                Tenant = 0,
                KeyPropertyPath = "Id",
                LookUp1 = "Name",
                CacheOnClient = true,
                IsClosed = true,
                EditableFromAutoCompleteWindow = false,
                DependencyFilter1 = "ObjectTableName",
                IsAutoComplete = true,
                IsMain = true,
                SortingByObjectField = "StatusWeight",
                ObjectTableTypeCode = "MD",
            }, objectTableRepository, textCodeRepository, objectTables, textCodes);
            #endregion

            #region TraceEvent
            ObjectTable TraceEventObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "TraceEvent",
                ObjectTableName = "TraceEvent",
                ObjectTablePlural = "Trace Events",
                ObjectTableSingular = "Trace Event",
                DBTableName = "TraceEvents",
                Tenant = 0,
                KeyPropertyPath = "Id",
                IsMain = false,
                ObjectTableTypeCode = "MD",
            }, objectTableRepository, textCodeRepository, objectTables, textCodes);
            #endregion

            #region RatesTable
            ObjectTable RatesTableObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "RatesTable",
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
            }, objectTableRepository, textCodeRepository, objectTables, textCodes);
            #endregion

            #region ObjectFieldObject
            ObjectTable ObjectFieldObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "ObjectField",
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
            }, objectTableRepository, textCodeRepository, objectTables, textCodes);
            #endregion

            #region Address
            ObjectTable AddressObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
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
            }, objectTableRepository, textCodeRepository, objectTables, textCodes);
            #endregion

            #region Agent
            ObjectTable AgentObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
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
            }
             , objectTableRepository, textCodeRepository, objectTables, textCodes);
            #endregion

            #region Airline
            ObjectTable AirlinesObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
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
            }, objectTableRepository, textCodeRepository, objectTables, textCodes);
            #endregion

            #region Branch
            ObjectTable BranchesObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
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
            }, objectTableRepository, textCodeRepository, objectTables, textCodes);
            #endregion

            #region Card
            ObjectTable CardsObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
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
                KeyPropertyPath = "Id",
                CacheOnClient = false,
                EditableFromAutoCompleteWindow = true,
                IsAutoComplete = false,
                IsMain = true,
                AutoCompleteSearchWindow = true,
                EnableAddFromLOV = true,
                EnableEditFromLOV = true,
                ObjectTableTypeCode = "MD",
            }, objectTableRepository, textCodeRepository, objectTables, textCodes);
            #endregion

            #region Carrier
            ObjectTable CarriersObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
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
            }, objectTableRepository, textCodeRepository, objectTables, textCodes);
            #endregion

            #region ChargesType
            ObjectTable ChargesTypesObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "ChargesType",
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
            }, objectTableRepository, textCodeRepository, objectTables, textCodes);
            #endregion

            #region ChargesGroup
            ObjectTable ChargesGroupObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Charges Group",
                ObjectTableName = "ChargesGroup",
                ObjectTablePlural = "Charges Groups",
                ObjectTableSingular = "Charges Group",
                DBTableName = "ChargesGroups",
                KeyPropertyPath = "Code",
                LookUp1 = "Code",
                LookUp2 = "Name",
                Tenant = 0,
                CacheOnClient = true,
                EnableAddFromLOV = true,
                EnableEditFromLOV = true,
                IsMain = true,
                IsAutoComplete = true,
                AutoCompleteSearchWindow = true,
                ObjectTableTypeCode = "MD",
            }, objectTableRepository, textCodeRepository, objectTables, textCodes);
            #endregion

            #region IATACode
            ObjectTable IATACodeObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "IATA Code",
                ObjectTableName = "IATACode",
                ObjectTablePlural = "IATA Codes",
                ObjectTableSingular = "IATA Code",
                DBTableName = "IATACodes",
                KeyPropertyPath = "Code",
                LookUp1 = "Code",
                LookUp2 = "Name",
                Tenant = 0,
                CacheOnClient = true,
                EnableAddFromLOV = true,
                EnableEditFromLOV = true,
                IsMain = true,
                IsAutoComplete = true,
                AutoCompleteSearchWindow = true,
                EnableSecurity = false,
                ObjectTableTypeCode = "MD",
            }, objectTableRepository, textCodeRepository, objectTables, textCodes);
            #endregion

            #region Customer
            ObjectTable CustomersObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
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
            }, objectTableRepository, textCodeRepository, objectTables, textCodes);
            #endregion

            #region Contact
            ObjectTable ContactsObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
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
                EditableFromAutoCompleteWindow = false,
                DependencyFilter1 = "CardId",
                IsClosed = false,
                IsMain = true,
                AutoCompleteSearchWindow = false,
                IsAutoComplete = true,
                EnableAddFromLOV = false,
                EnableEditFromLOV = false,
                EnableSecurity = true,
                ObjectTableTypeCode = "MD",
            }, objectTableRepository, textCodeRepository, objectTables, textCodes);
            #endregion

            #region Country
            ObjectTable CountriesObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Country",
                ObjectTableName = "Country",
                ObjectTablePlural = "Countries",
                ObjectTableSingular = "Country",
                DBTableName = "Countries",
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
            }, objectTableRepository, textCodeRepository, objectTables, textCodes);
            #endregion

            #region Currency
            ObjectTable CurrenciesObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Currency",
                ObjectTableName = "Currency",
                ObjectTablePlural = "Currencies",
                ObjectTableSingular = "Currency",
                DBTableName = "Currencies",
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
            }, objectTableRepository, textCodeRepository, objectTables, textCodes);
            #endregion

            #region CustomAgent
            ObjectTable CustomAgentsObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "CustomAgent",
                ObjectTableName = "CustomAgent",
                ObjectTablePlural = "Custom Agents",
                ObjectTableSingular = "Custom Agent",
                DBTableName = "CustomAgents",
                Tenant = 0,
                KeyPropertyPath = "Id",
                EditableFromAutoCompleteWindow = true,
                IsNewWizard = true,
                NewWizardControlName = "Simplog.FreightLib.NewCustomAgentCommand",
                IsMain = true,
                EnableSecurity = true,
                ObjectTableTypeCode = "MD",
            }, objectTableRepository, textCodeRepository, objectTables, textCodes);
            #endregion

            #region Department
            ObjectTable DepartmentsObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
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
            }, objectTableRepository, textCodeRepository, objectTables, textCodes);
            #endregion

            #region Direction
            ObjectTable DirectionsObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
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
            }, objectTableRepository, textCodeRepository, objectTables, textCodes);
            #endregion

            #region ExternalDocument
            ObjectTable ExternalDocumentsObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
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
            }, objectTableRepository, textCodeRepository, objectTables, textCodes);
            #endregion

            #region InternalDocument
            ObjectTable InternalDocumentsObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
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
            }, objectTableRepository, textCodeRepository, objectTables, textCodes);
            #endregion

            #region FollowUp
            ObjectTable FollowUpsObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
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
            }, objectTableRepository, textCodeRepository, objectTables, textCodes);
            #endregion

            #region GlobalZone
            ObjectTable GlobalZonesObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "GlobalZone",
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
            }, objectTableRepository, textCodeRepository, objectTables, textCodes);
            #endregion

            #region Incoterm
            ObjectTable IncotermsObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
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
            }, objectTableRepository, textCodeRepository, objectTables, textCodes);
            #endregion

            #region Master
            ObjectTable MasterObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
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
            }, objectTableRepository, textCodeRepository, objectTables, textCodes);

            #endregion

            #region PaymentTerm
            ObjectTable PaymentTermsObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "PaymentTerm",
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
            }, objectTableRepository, textCodeRepository, objectTables, textCodes);
            #endregion

            #region Port
            ObjectTable PortsObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
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
            }, objectTableRepository, textCodeRepository, objectTables, textCodes);
            #endregion

            #region ShippingAgent
            ObjectTable ShippingAgentsObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "ShippingAgent",
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
            }, objectTableRepository, textCodeRepository, objectTables, textCodes);
            #endregion

            #region ShippingLine
            ObjectTable ShippingLinesObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "ShippingLine",
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
            }, objectTableRepository, textCodeRepository, objectTables, textCodes);
            #endregion

            #region SpecialService
            ObjectTable SpecialServicesObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "SpecialService",
                ObjectTableName = "SpecialService",
                ObjectTablePlural = "Special Services",
                ObjectTableSingular = "Special Service",
                DBTableName = "SpecialServices",
                Tenant = 0,
                KeyPropertyPath = "Id",
                IsMain = true,
                EnableSecurity = false,
                ObjectTableTypeCode = "MD",
            }, objectTableRepository, textCodeRepository, objectTables, textCodes);
            #endregion

            #region State
            ObjectTable StatesObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
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
            }, objectTableRepository, textCodeRepository, objectTables, textCodes);
            #endregion

            #region TransportMode
            ObjectTable TransportModesObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "TransportMode",
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
            }, objectTableRepository, textCodeRepository, objectTables, textCodes);
            #endregion

            #region Trucker
            ObjectTable TruckersObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
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
            }, objectTableRepository, textCodeRepository, objectTables, textCodes);
            #endregion

            #region Measurement
            ObjectTable MeasurementsObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
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
                IsClosed = false,
                EditableFromAutoCompleteWindow = true,
                EnableAddFromLOV = false,
                EnableEditFromLOV = false,
                IsMain = true,
                IsAutoComplete = true,
                AutoCompleteSearchWindow = false,
                EnableSecurity = false,
                ObjectTableTypeCode = "MD",
            }, objectTableRepository, textCodeRepository, objectTables, textCodes);
            #endregion

            #region User
            ObjectTable UsersObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
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
                //EnableSecurity = true,
                ObjectTableTypeCode = "MD",
            }, objectTableRepository, textCodeRepository, objectTables, textCodes);
            #endregion

            #region PartnerType
            ObjectTable PartnerTypesObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "PartnerType",
                ObjectTableName = "PartnerType",
                ObjectTablePlural = "Partner Types",
                ObjectTableSingular = "Partner Type",
                DBTableName = "PartnerTypes",
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
            }, objectTableRepository, textCodeRepository, objectTables, textCodes);
            #endregion

            #region ShipmentType
            ObjectTable ShipmentTypesObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "ShipmentType",
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
            }, objectTableRepository, textCodeRepository, objectTables, textCodes);
            #endregion

            #region VatType
            ObjectTable VatTypesObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "VatType",
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
            }, objectTableRepository, textCodeRepository, objectTables, textCodes);
            #endregion

            #region VatTypePercentage
            ObjectTable VatTypePercentageObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "VatTypePercentage",
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
            }, objectTableRepository, textCodeRepository, objectTables, textCodes);
            #endregion

            #region FollowUpType
            ObjectTable FollowUpTypesObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "FollowUpType",
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
            }, objectTableRepository, textCodeRepository, objectTables, textCodes);
            #endregion

            #region EventType
            ObjectTable EventTypesObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "EventType",
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
            }, objectTableRepository, textCodeRepository, objectTables, textCodes);
            #endregion

            #region ContainerType
            ObjectTable ContainerTypesObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "ContainerType",
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
            }, objectTableRepository, textCodeRepository, objectTables, textCodes);
            #endregion

            #region PackageType
            ObjectTable PackageTypesObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "PackageType",
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
                AutoCompleteSearchWindow = true,
                EnableSecurity = true,
                ObjectTableTypeCode = "MD",
            }, objectTableRepository, textCodeRepository, objectTables, textCodes);
            #endregion

            #region DocumentType
            ObjectTable DocumentTypesObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "DocumentType",
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
            }, objectTableRepository, textCodeRepository, objectTables, textCodes);
            #endregion

            #region EntityDate
            ObjectTable EntityDateObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "EntityDate",
                ObjectTableName = "EntityDate",
                ObjectTablePlural = "Entity Dates",
                ObjectTableSingular = "Entity Date",
                DBTableName = "EntityDates",
                Tenant = 0,
                KeyPropertyPath = "Id",
                IsMain = true,
                IsClosed = true,
                ObjectTableTypeCode = "MD",
            }, objectTableRepository, textCodeRepository, objectTables, textCodes);
            #endregion

            #region PrepaidCollect
            ObjectTable PrepaidCollectObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
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
            }, objectTableRepository, textCodeRepository, objectTables, textCodes);
            #endregion

            #region WeightUnit
            ObjectTable WeightUnitObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "WeightUnit",
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
            }, objectTableRepository, textCodeRepository, objectTables, textCodes);
            #endregion

            #region DueType
            ObjectTable DueTypeObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "DueType",
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
            }, objectTableRepository, textCodeRepository, objectTables, textCodes);
            #endregion

            #region RateClass
            ObjectTable RateClassObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "RateClass",
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
            }, objectTableRepository, textCodeRepository, objectTables, textCodes);
            #endregion

            #region DimensionsUnit
            ObjectTable DimensionsUnitObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "DimensionsUnit",
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
            }, objectTableRepository, textCodeRepository, objectTables, textCodes);
            #endregion

            #region AWBPrintSpecification
            ObjectTable AWBPrintSpecificationObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "AWBPrintSpecification",
                ObjectTableName = "AWBPrintSpecification",
                ObjectTablePlural = "AWB Print Specifications",
                ObjectTableSingular = "AWB Print Specification",
                DBTableName = "AWBPrintSpecifications",
                LookUp1 = "Name",
                Tenant = 0,
                IsClosed = true,
                KeyPropertyPath = "Code",
                IsAutoComplete = true,
                CacheOnClient = true,
                IsMain = true,
                ObjectTableTypeCode = "MD",
            }, objectTableRepository, textCodeRepository, objectTables, textCodes);
            #endregion

            #region AWBChargesCode
            ObjectTable AWBChargesCodeObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "AWBChargesCode",
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
            }, objectTableRepository, textCodeRepository, objectTables, textCodes);
            #endregion

            #region AWBSpecialHandlingCode
            ObjectTable AWBSpecialHandlingCodeObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "AWBSpecialHandlingCode",
                ObjectTableName = "AWBSpecialHandlingCode",
                ObjectTablePlural = "AWB Special Handling Codes",
                ObjectTableSingular = "AWB Special Handling Code",
                DBTableName = "AWBSpecialHandlingCodes",
                LookUp1 = "Code",
                Tenant = 0,
                IsClosed = true,
                KeyPropertyPath = "Code",
                IsAutoComplete = true,
                AutoCompleteSearchWindow = true,
                CacheOnClient = true,
                IsMain = true,
                ObjectTableTypeCode = "MD",
            }, objectTableRepository, textCodeRepository, objectTables, textCodes);
            #endregion

            #region VolumeUnit
            ObjectTable VolumeUnitObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "VolumeUnit",
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
            }, objectTableRepository, textCodeRepository, objectTables, textCodes);
            #endregion

            #region Vessel
            ObjectTable VesselObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
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
            }, objectTableRepository, textCodeRepository, objectTables, textCodes);
            #endregion

            #region CustomerTenantAccess
            ObjectTable CustomerTenantAccessObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "CustomerTenantAccess",
                ObjectTableName = "CustomerTenantAccess",
                ObjectTablePlural = "CustomerTenantAccesses",
                ObjectTableSingular = "CustomerTenantAccess",
                DBTableName = "CustomerTenantAccesses",                   
                Tenant = 0,
                KeyPropertyPath = "Id",
                IsMain = true,          
                EnableSecurity = true,
                ObjectTableTypeCode = "MD",
            }, objectTableRepository, textCodeRepository, objectTables, textCodes);
            #endregion

            #region CustomerTenantAccessRequest
            ObjectTable CustomerTenantAccessRequestObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "CustomerTenantAccessRequest",
                ObjectTableName = "CustomerTenantAccessRequest",
                ObjectTablePlural = "CustomerTenantAccessRequests",
                ObjectTableSingular = "CustomerTenantAccessRequest",
                DBTableName = "CustomerTenantAccessRequests",
                Tenant = 0,
                KeyPropertyPath = "Id",
                IsMain = true,
                EnableSecurity = true,
                ObjectTableTypeCode = "MD",
            }, objectTableRepository, textCodeRepository, objectTables, textCodes);
            #endregion

            #region CustomerTenantAccessCard
            ObjectTable CustomerTenantAccessCardsObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "CustomerTenantAccessCard",
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
            }, objectTableRepository, textCodeRepository, objectTables, textCodes);
            #endregion

            #region CustomerTenantAccessStatusType
            ObjectTable CustomerTenantAccessStatusTypeObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
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
            }, objectTableRepository, textCodeRepository, objectTables, textCodes);

            #endregion

            #region HybridPartnerObject
            ObjectTable HybridPartnerObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Hybrid Partner",
                ObjectTableName = "HybridPartner",
                ObjectTablePlural = "Hybrid Partners",
                ObjectTableSingular = "Hybrid Partner",
                DBTableName = "HybridPartners",
                Tenant = 0,
                KeyPropertyPath = "Id",
                IsMain = true,
                EnableSecurity = true,
                ObjectTableTypeCode = "MD",
                AutoCompleteSearchWindow = true,      
            }, objectTableRepository, textCodeRepository, objectTables, textCodes);
            #endregion


            #region WareHouse
            ObjectTable WareHouseObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
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
            }, objectTableRepository, textCodeRepository, objectTables, textCodes);
            #endregion

            #region Rank
            ObjectTable RankObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
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
            }, objectTableRepository, textCodeRepository, objectTables, textCodes);
            #endregion

            #region SystemData
            ObjectTable SystemDataObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "SystemData",
                ObjectTableName = "SystemData",
                ObjectTablePlural = "System Data",
                ObjectTableSingular = "System Data",
                Tenant = 0,
                KeyPropertyPath = "Id",
                IsMain = true,
                ObjectTableTypeCode = "MD",
            }, objectTableRepository, textCodeRepository, objectTables, textCodes);

            #endregion

            #region DescriptionOfGoods
            ObjectTable DescriptionOfGoodsObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "DescriptionOfGood",
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
            }, objectTableRepository, textCodeRepository, objectTables, textCodes);
            #endregion

            #region TarrifTypeObject
            ObjectTable TarrifTypeObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "TarrifType",
                ObjectTableName = "TarrifType",
                ObjectTablePlural = "Tarrif Types",
                ObjectTableSingular = "Tarrif Type",
                DBTableName = "TarrifTypes",
                Tenant = 0,
                KeyPropertyPath = "Code",
                IsClosed = true,
                IsMain = true,
                ObjectTableTypeCode = "MD",
            }, objectTableRepository, textCodeRepository, objectTables, textCodes);
            #endregion

            #region TarrifFromToTypeObject
            ObjectTable TarrifFromToTypeObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "TarrifFromToType",
                ObjectTableName = "TarrifFromToType",
                ObjectTablePlural = "Tarrif From To Types",
                ObjectTableSingular = "Tarrif From To Type",
                DBTableName = "TarrifFromToTypes",
                Tenant = 0,
                KeyPropertyPath = "Code",
                IsClosed = true,
                IsMain = true,
                ObjectTableTypeCode = "MD",
            }, objectTableRepository, textCodeRepository, objectTables, textCodes);
            #endregion

            #region TarrifChargesObject
            ObjectTable TarrifChargesObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "TarrifCharge",
                ObjectTableName = "TarrifCharge",
                ObjectTablePlural = "Tarrif Charges",
                ObjectTableSingular = "Tarrif Charge",
                DBTableName = "TarrifCharges",
                Tenant = 0,
                KeyPropertyPath = "Id",
                IsClosed = false,
                IsMain = true,
                ObjectTableTypeCode = "MD",
            }, objectTableRepository, textCodeRepository, objectTables, textCodes);
            #endregion

            #region TarrifFromToObject
            ObjectTable TarrifFromToObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "TarrifFromTo",
                ObjectTableName = "TarrifFromTo",
                ObjectTablePlural = "Tarrifs From To",
                ObjectTableSingular = "Tarrif From To",
                DBTableName = "TarrifFromToes",
                Tenant = 0,
                KeyPropertyPath = "Id",
                IsClosed = false,
                IsMain = true,
                ObjectTableTypeCode = "MD",
            }, objectTableRepository, textCodeRepository, objectTables, textCodes);
            #endregion

            #region TarrifHeaderObject
            ObjectTable TarrifHeaderObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "TarrifHeader",
                ObjectTableName = "TarrifHeader",
                ObjectTablePlural = "Tarrif Headers",
                ObjectTableSingular = "Tarrif Header",
                DBTableName = "TarrifHeaders",
                Tenant = 0,
                KeyPropertyPath = "Id",
                IsClosed = false,
                IsMain = true,
                ObjectTableTypeCode = "MD",
            }, objectTableRepository, textCodeRepository, objectTables, textCodes);
            #endregion

            #region TarrifStepObject
            ObjectTable TarrifStepObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "TarrifStep",
                ObjectTableName = "TarrifStep",
                ObjectTablePlural = "Tarrif Steps",
                ObjectTableSingular = "Tarrif Step",
                DBTableName = "TarrifSteps",
                Tenant = 0,
                KeyPropertyPath = "Id",
                IsClosed = false,
                IsMain = true,
                ObjectTableTypeCode = "MD",
            }, objectTableRepository, textCodeRepository, objectTables, textCodes);
            #endregion

            #region TextCodeObject
            ObjectTable TextCodeObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "TextCode",
                ObjectTableName = "TextCode",
                ObjectTablePlural = "Text Codes",
                ObjectTableSingular = "Text Code",
                DBTableName = "TextCodes",
                Tenant = 0,
                KeyPropertyPath = "Id",
                IsClosed = false,
                IsMain = true,
                ObjectTableTypeCode = "MD",
            }, objectTableRepository, textCodeRepository, objectTables, textCodes);
            #endregion

            #region MAWBStackObject
            ObjectTable MAWBStackObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "MAWBStack",
                ObjectTableName = "MAWBStack",
                ObjectTablePlural = "MAWB Stacks",
                ObjectTableSingular = "MAWB Stack",
                DBTableName = "MAWBStacks",
                Tenant = 0,
                KeyPropertyPath = "Id",
                IsClosed = false,
                IsMain = true,
                ObjectTableTypeCode = "MD",
            }, objectTableRepository, textCodeRepository, objectTables, textCodes);
            #endregion

            #region RestrictionObject
            ObjectTable RestrictionObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
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
            }, objectTableRepository, textCodeRepository, objectTables, textCodes);
            #endregion

            #region RoleObject
            ObjectTable RoleObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
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
            }, objectTableRepository, textCodeRepository, objectTables, textCodes);
            #endregion

            #region ShipmentLevel
            ObjectTable ShipmentLevelObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
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
            }, objectTableRepository, textCodeRepository, objectTables, textCodes);
            #endregion

            #region Account
            ObjectTable AccountObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
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
            }, objectTableRepository, textCodeRepository, objectTables, textCodes);
            #endregion

            #region AccountType
            ObjectTable AccountTypeObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
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
            }, objectTableRepository, textCodeRepository, objectTables, textCodes);
            #endregion

            #region ARInvoicePayment
            ObjectTable ARInvoicePaymentObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
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
                CacheOnClient = true,
                EditableFromAutoCompleteWindow = true,
                EnableAddFromLOV = true,
                EnableEditFromLOV = true,
                IsMain = true,
                AutoCompleteSearchWindow = true,
                SortingByObjectField = "Code",
                IsComposition = true,
                ObjectTableTypeCode = "BR",
            }, objectTableRepository, textCodeRepository, objectTables, textCodes);
            #endregion

            #region ARInvoice
            ObjectTable InvoiceObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
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
            }, objectTableRepository, textCodeRepository, objectTables, textCodes);
            #endregion

            #region ARInvoiceLine
            ObjectTable InvoiceLinesObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
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
            }, objectTableRepository, textCodeRepository, objectTables, textCodes);
            #endregion

            #region ARInvoiceType
            ObjectTable InvoiceTypesObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
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
            }, objectTableRepository, textCodeRepository, objectTables, textCodes);
            #endregion

            #region ARInvoiceStatus
            ObjectTable InvoiceStatusObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
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
            }, objectTableRepository, textCodeRepository, objectTables, textCodes);
            #endregion

            #region ARPayment
            ObjectTable ARPaymentObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
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
            }, objectTableRepository, textCodeRepository, objectTables, textCodes);
            #endregion

            #region PaymentMethod
            ObjectTable ARPaymentMethodObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Payment Method",
                ObjectTableName = "PaymentMethod",
                ObjectTablePlural = "Payment Methods",
                ObjectTableSingular = "Payment Method",
                DBTableName = "PaymentMethods",
                Tenant = 0,
                KeyPropertyPath = "Code",
                IsClosed = false,
                IsMain = true,
                IsAutoComplete = true,
                CacheOnClient = true,
                LookUp1 = "Name",
                ObjectTableTypeCode = "MD",
                EnableSecurity=true,
            }, objectTableRepository, textCodeRepository, objectTables, textCodes);
            #endregion

            #region ARPaymentStatus
            ObjectTable ARPaymentStatusObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
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
            }, objectTableRepository, textCodeRepository, objectTables, textCodes);
            #endregion

  
            #region APInvoiceType
            ObjectTable APInvoiceTypesObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
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
            }, objectTableRepository, textCodeRepository, objectTables, textCodes);
            #endregion

            #region APInvoiceLine
            ObjectTable APInvoiceLineObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
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
            }, objectTableRepository, textCodeRepository, objectTables, textCodes);
            #endregion

            #region APInvoiceStatus
            ObjectTable APInvoiceStatusObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
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
            }, objectTableRepository, textCodeRepository, objectTables, textCodes);
            #endregion

            #region APInvoice
            ObjectTable APInvoiceObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
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
            }, objectTableRepository, textCodeRepository, objectTables, textCodes);
            #endregion

            #region APInvoiceTotalVat
            ObjectTable APInvoiceTotalVatObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "A/P Invoice Total Vat",
                ObjectTableName = "APInvoiceTotalVat",
                ObjectTablePlural = "A/P Invoice Total VATs",
                ObjectTableSingular = "A/P Invoice Total VAT",
                DBTableName = "APInvoiceTotalVats",
                Tenant = 0,
                KeyPropertyPath = "Id",
                HasCounter = true,
                IsMain = true,
                IsSaveButtonVisible = false,
                ObjectTableTypeCode = "BR",
            }, objectTableRepository, textCodeRepository, objectTables, textCodes);
            #endregion

            #region APPayment
            ObjectTable APPaymentObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
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
            }, objectTableRepository, textCodeRepository, objectTables, textCodes);
            #endregion

            #region APPaymentMethod
            ObjectTable APPaymentMethodObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "A/P Payment Method",
                ObjectTableName = "APPaymentMethod",
                ObjectTablePlural = "A/P Payment Methods",
                ObjectTableSingular = "A/P Payment Method",
                DBTableName = "APPaymentMethods",
                Tenant = 0,
                KeyPropertyPath = "Code",
                IsClosed = true,
                IsMain = false,
                IsAutoComplete = true,
                CacheOnClient = true,
                LookUp1 = "Name",
                ObjectTableTypeCode = "MD",
            }, objectTableRepository, textCodeRepository, objectTables, textCodes);
            #endregion

            #region APPaymentStatus
            ObjectTable APPaymentStatusObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
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
            }, objectTableRepository, textCodeRepository, objectTables, textCodes);
            #endregion

            #region APInvoicePayment
            ObjectTable APInvoicePaymentObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
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
            }, objectTableRepository, textCodeRepository, objectTables, textCodes);
            #endregion

            #region Vendor
            ObjectTable VendorObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
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
            }
             , objectTableRepository, textCodeRepository, objectTables, textCodes);
            #endregion

            #region CommunicationLogs
            ObjectTable CommunicationLogsObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
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
            }, objectTableRepository, textCodeRepository, objectTables, textCodes);
            #endregion

            #region CommunicationLogTypes
            ObjectTable CommunicationLogTypesObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
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
            }, objectTableRepository, textCodeRepository, objectTables, textCodes);
            #endregion

            #region CommunicationStatusTypes
            ObjectTable CommunicationStatusTypesObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Communication Status Type",
                ObjectTableName = "CommunicationStatusTypes",
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
            }, objectTableRepository, textCodeRepository, objectTables, textCodes);
            #endregion

            #region PasswordPolicies
            ObjectTable PasswordPoliciesObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
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
            }, objectTableRepository, textCodeRepository, objectTables, textCodes);
            #endregion

            #region Packages
            ObjectTable PackagesObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
                DefaultText = "Package",
                ObjectTableName = "Package",
                ObjectTablePlural = "Packages",
                ObjectTableSingular = "Package",
                DBTableName = "Packages",
                Tenant = 0,
                KeyPropertyPath = "Code",
                IsClosed = true,
                IsMain = false,
                IsAutoComplete = true,
                CacheOnClient = true,
                ObjectTableTypeCode = "MD",
                LookUp1 = "Name",
            }, objectTableRepository, textCodeRepository, objectTables, textCodes);
            #endregion


            #region TermsofUseSignatures
            ObjectTable TermsofUseSignatureObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
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

            }, objectTableRepository, textCodeRepository, objectTables, textCodes);
            #endregion

            objectContext.SaveChanges();
        }
    }


}