using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
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
        public void LoadEventTypes()
        {
            ObjectContext = WebFreightContext.GetContext(0);
            EntityStatusRepository = new EntityStatusRepository(ObjectContext);
            EventTypeRepository = new EventTypeRepository(ObjectContext);
            List<EntityStatus> tenantEntityStatus = EntityStatusRepository.GetEntityStatusByTenant(0).ToList();
            Dictionary<string, EventType> tenantEventTypes = EventTypeRepository.GetEventTypesByTenant(0).ToDictionary(d => d.Code + d.ObjectTableId, a => a);

            #region objecrtTables
            ObjectTablePM shipmentObject = ObjectTableQuery.GetObjectTableByCode("Shipment", 0);
            ObjectTablePM masterObject = ObjectTableQuery.GetObjectTableByCode("Master", 0);
            ObjectTablePM quoteObject = ObjectTableQuery.GetObjectTableByCode("Quote", 0);
            ObjectTablePM customerObject = ObjectTableQuery.GetObjectTableByCode("Customer", 0);
            ObjectTablePM agentObject = ObjectTableQuery.GetObjectTableByCode("Agent", 0);
            ObjectTablePM customAgentObject = ObjectTableQuery.GetObjectTableByCode("CustomAgent", 0);
            ObjectTablePM shippingAgentObject = ObjectTableQuery.GetObjectTableByCode("ShippingAgent", 0);
            ObjectTablePM airlineObject = ObjectTableQuery.GetObjectTableByCode("Airline", 0);
            ObjectTablePM shippingLineObject = ObjectTableQuery.GetObjectTableByCode("ShippingLine", 0);
            ObjectTablePM truckerObject = ObjectTableQuery.GetObjectTableByCode("Trucker", 0);
            ObjectTablePM incotermObject = ObjectTableQuery.GetObjectTableByCode("Incoterm", 0);
            ObjectTablePM paymentTermObject = ObjectTableQuery.GetObjectTableByCode("PaymentTerm", 0);
            ObjectTablePM currencyObject = ObjectTableQuery.GetObjectTableByCode("Currency", 0);
            ObjectTablePM vatTypeObject = ObjectTableQuery.GetObjectTableByCode("VatType", 0);
            ObjectTablePM chargeTypeObject = ObjectTableQuery.GetObjectTableByCode("ChargesType", 0);
            ObjectTablePM portObject = ObjectTableQuery.GetObjectTableByCode("Port", 0);
            ObjectTablePM countryObject = ObjectTableQuery.GetObjectTableByCode("Country", 0);
            ObjectTablePM globalZoneObject = ObjectTableQuery.GetObjectTableByCode("GlobalZone", 0);
            ObjectTablePM branchObject = ObjectTableQuery.GetObjectTableByCode("Branch", 0);
            ObjectTablePM departmentObject = ObjectTableQuery.GetObjectTableByCode("Department", 0);
            ObjectTablePM contactObject = ObjectTableQuery.GetObjectTableByCode("Contact", 0);
            ObjectTablePM userObject = ObjectTableQuery.GetObjectTableByCode("User", 0);
            ObjectTablePM stateObject = ObjectTableQuery.GetObjectTableByCode("State", 0);
            ObjectTablePM documentTypeObject = ObjectTableQuery.GetObjectTableByCode("DocumentType", 0);
            ObjectTablePM eventTypeObject = ObjectTableQuery.GetObjectTableByCode("EventType", 0);
            ObjectTablePM packageTypeObject = ObjectTableQuery.GetObjectTableByCode("PackageType", 0);
            ObjectTablePM vesselObject = ObjectTableQuery.GetObjectTableByCode("Vessel", 0);
            ObjectTablePM warehouseObject = ObjectTableQuery.GetObjectTableByCode("Warehouse", 0);
            ObjectTablePM accountObject = ObjectTableQuery.GetObjectTableByCode("Account", 0);
            ObjectTablePM vendorObject = ObjectTableQuery.GetObjectTableByCode("Vendor", 0);
            ObjectTablePM arInvoiceObject = ObjectTableQuery.GetObjectTableByCode("ARInvoice", 0);
            ObjectTablePM apInvoiceObject = ObjectTableQuery.GetObjectTableByCode("APInvoice", 0);
            ObjectTablePM arPaymentObject = ObjectTableQuery.GetObjectTableByCode("ARPayment", 0);
            ObjectTablePM apPaymentObject = ObjectTableQuery.GetObjectTableByCode("APPayment", 0);
            ObjectTablePM commLogObject = ObjectTableQuery.GetObjectTableByCode("CommunicationLog", 0);
            ObjectTablePM tenantMngmntObject = ObjectTableQuery.GetObjectTableByCode("TenantManagement", 0);
            ObjectTablePM analyzeQueueObject = ObjectTableQuery.GetObjectTableByCode("AnalyzeQueue", 0);
            ObjectTablePM CountryCityObject = ObjectTableQuery.GetObjectTableByCode("CountryCity", 0);
            ObjectTablePM MeasurementObject = ObjectTableQuery.GetObjectTableByCode("Measurement", 0);
            ObjectTablePM BluesnapContractObject = ObjectTableQuery.GetObjectTableByCode("BluesnapContract", 0);
            ObjectTablePM CreditCardTypeObject = ObjectTableQuery.GetObjectTableByCode("CreditCardType", 0);
            ObjectTablePM MoveTypeObject = ObjectTableQuery.GetObjectTableByCode("MoveType", 0);
            ObjectTablePM ReportObject = ObjectTableQuery.GetObjectTableByCode("Report", 0);
            ObjectTablePM RegionObject = ObjectTableQuery.GetObjectTableByCode("Region", 0);
            ObjectTablePM CommodityObject = ObjectTableQuery.GetObjectTableByCode("Commodity", 0);
            ObjectTablePM SpecialServicesTypeObject = ObjectTableQuery.GetObjectTableByCode("SpecialServicesType", 0);
            ObjectTablePM BusinessUnitObject = ObjectTableQuery.GetObjectTableByCode("BusinessUnit", 0);
            ObjectTablePM CustomerSizeObject = ObjectTableQuery.GetObjectTableByCode("CustomerSize", 0);
            ObjectTablePM LeadSourceObject = ObjectTableQuery.GetObjectTableByCode("LeadSource", 0);
            ObjectTablePM AdditionalServiceObject = ObjectTableQuery.GetObjectTableByCode("AdditionalService", 0);
            ObjectTablePM IndustryObject = ObjectTableQuery.GetObjectTableByCode("Industry", 0);
            ObjectTablePM CompetitorObject = ObjectTableQuery.GetObjectTableByCode("Competitor", 0);
            ObjectTablePM ProductTypeObject = ObjectTableQuery.GetObjectTableByCode("ProductType", 0);
            ObjectTablePM QuoteStageObject = ObjectTableQuery.GetObjectTableByCode("QuoteStage", 0);
            ObjectTablePM MessagingStockTable = ObjectTableQuery.GetObjectTableByCode("MessagingStock", 0);
            ObjectTablePM participantObject = ObjectTableQuery.GetObjectTableByCode("Participant", 0);
            ObjectTablePM automationObject = ObjectTableQuery.GetObjectTableByCode("Automation", 0);
            ObjectTablePM ARPaymentMethodObject = ObjectTableQuery.GetObjectTableByCode("ARPaymentMethod", 0);
            ObjectTablePM APPaymentMethodObject = ObjectTableQuery.GetObjectTableByCode("APPaymentMethod", 0);
            ObjectTablePM AccountingTransferHeaderObject = ObjectTableQuery.GetObjectTableByCode("AccountingTransferHeader", 0);
            #endregion

            #region ============= Just For Testing =============
            if (Testing.General.IsTesting)
            {
                AddEventTypes.AddEventType(new EventTypeDetails()
                {
                    Code = "TTTT",
                    EnglishName = "TESET",
                    Tenant = 0,
                    AddedManually = false,
                    LocalName = "test",
                    ObjectTableId = quoteObject.Id,
                    ShortView = false,

                }, EventTypeRepository, tenantEventTypes);

                //Update
                AddEventTypes.AddEventType(new EventTypeDetails()
                {
                    Code = "UPQT",
                    EnglishName = "UpdateTest_UpdateQuote",
                    Tenant = 0,
                    AddedManually = false,
                    LocalName = "UpdateQuote",
                    ObjectTableId = quoteObject.Id,
                    ShortView = false,
                }, EventTypeRepository, tenantEventTypes);
            }
            #endregion

            #region Master eventtypes
            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "REMF",
                EnglishName = "Reminder",
                Tenant = 0,
                AddedManually = false,
                IsManualEntry = true,
                LocalName = "Reminder",
                ObjectTableId = masterObject.Id,
                ShortView = true,
                FollowUpLocalName = "Reminder",
                FollowUpEnglishName = "Reminder",
                IsFollowUp = true,
                ManualActivatedFollowUp = true,
                EventTypeCategoryCode = "LOG",
            }, EventTypeRepository, tenantEventTypes);
            #endregion

            #region Quote eventTypes

            #region Ticket Events 
            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "QTCN",
                EnglishName = "Ticket Connected",
                Tenant = 0,
                AddedManually = false,
                LocalName = "Ticket Connected",
                ObjectTableId = quoteObject.Id,
                ShortView = false,
                EventTypeCategoryCode = "OPE",
            }, EventTypeRepository, tenantEventTypes);
            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "QTDC",
                EnglishName = "Ticket Disconnected",
                Tenant = 0,
                AddedManually = false,
                LocalName = "Ticket Disconnected",
                ObjectTableId = quoteObject.Id,
                ShortView = false,
                EventTypeCategoryCode = "OPE",
            }, EventTypeRepository, tenantEventTypes);
            #endregion 

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "UPQT",
                EnglishName = "Quote Updated",
                Tenant = 0,
                AddedManually = false,
                LocalName = "Quote Updated",
                ObjectTableId = quoteObject.Id,
                ShortView = false,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "CRQT",
                EnglishName = "Created",
                Tenant = 0,
                AddedManually = false,
                IsManualEntry = false,
                LocalName = "Created",
                ObjectTableId = quoteObject.Id,
                ShortView = true,
                InActive = true,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "REMF",
                EnglishName = "Reminder",
                Tenant = 0,
                AddedManually = false,
                IsManualEntry = true,
                LocalName = "Reminder",
                ObjectTableId = quoteObject.Id,
                ShortView = true,
                FollowUpLocalName = "Reminder",
                FollowUpEnglishName = "Reminder",
                IsFollowUp = true,
                ManualActivatedFollowUp = true,
                EventTypeCategoryCode = "LOG",
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "TRQR",
                EnglishName = "Transportation Quote Received",
                Tenant = 0,
                AddedManually = false,
                IsManualEntry = true,
                LocalName = "Transportation Quote Received",
                ObjectTableId = quoteObject.Id,
                ShortView = true,
                FollowUpLocalName = "Transportation Quote",
                FollowUpEnglishName = "Transportation Quote",
                IsFollowUp = true,
                ManualActivatedFollowUp = true,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "EXPD",
                EnglishName = "Quote Expired",
                Tenant = 0,
                AddedManually = false,
                IsManualEntry = true,
                LocalName = "Quote Expired",
                ObjectTableId = quoteObject.Id,
                IsFollowUp = true,
                FollowUpLocalName = "Quote Expired",
                FollowUpEnglishName = "Quote Expired",
                ShortView = true,
                ManualActivatedFollowUp = true,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "SASC",
                EnglishName = "Quote Sent",
                Tenant = 0,
                AddedManually = false,
                LocalName = "Quote Sent",
                ObjectTableId = quoteObject.Id,
                ShortView = true,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "CLQT",
                EnglishName = "Cancel Quote",
                Tenant = 0,
                AddedManually = false,
                LocalName = "Cancel Quote",
                ObjectTableId = quoteObject.Id,
                ShortView = true,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "RAQT",
                EnglishName = "Reactivate Quote",
                Tenant = 0,
                AddedManually = false,
                LocalName = "Reactivate Quote",
                ObjectTableId = quoteObject.Id,
                ShortView = true,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "RQTD",
                EnglishName = "Return To Draft",
                Tenant = 0,
                AddedManually = false,
                LocalName = "Return To Draft",
                ObjectTableId = quoteObject.Id,
                ShortView = true,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "CFAQ",
                EnglishName = "Copied from another Quote",
                Tenant = 0,
                AddedManually = false,
                IsManualEntry = false,
                LocalName = "Copied from another Quote",
                ObjectTableId = quoteObject.Id,
                IsFollowUp = false,
                ShortView = true,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "QTVI",
                EnglishName = "Quote Viewed",
                Tenant = 0,
                AddedManually = false,
                LocalName = "Quote Viewed",
                ObjectTableId = quoteObject.Id,
                ShortView = true,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "QTDS",
                EnglishName = "Quote In Discussion",
                Tenant = 0,
                AddedManually = false,
                LocalName = "Quote In Discussion",
                ObjectTableId = quoteObject.Id,
                ShortView = true,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "QTCP",
                EnglishName = "Quote Accepted",
                Tenant = 0,
                AddedManually = false,
                LocalName = "Quote Accepted",
                ObjectTableId = quoteObject.Id,
                IsFollowUp = true,
                FollowUpLocalName = "Customer Accepted",
                FollowUpEnglishName = "Customer Accepted",
                ShortView = true,
                ManualActivatedFollowUp = true,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "QTDL",
                EnglishName = "Quote Declined",
                Tenant = 0,
                AddedManually = false,
                LocalName = "Quote Declined",
                ObjectTableId = quoteObject.Id,
                ShortView = true,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "QEMO",
                EnglishName = "Quote email out sent",
                Tenant = 0,
                AddedManually = false,
                LocalName = "Email out sent",
                ObjectTableId = quoteObject.Id,
                ShortView = true,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "QACR",
                EnglishName = "Activity Created",
                Tenant = 0,
                LocalName = "Activity Created",
                ObjectTableId = quoteObject.Id,
                ShortView = true,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "QACM",
                EnglishName = "Activity Completed",
                Tenant = 0,
                LocalName = "Activity Completed",
                ObjectTableId = quoteObject.Id,
                ShortView = true,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "QARP",
                EnglishName = "Activity Reopened",
                Tenant = 0,
                LocalName = "Activity Reopened",
                ObjectTableId = quoteObject.Id,
                ShortView = true,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "QUSG",
                EnglishName = "Stage Due Date updated",
                Tenant = 0,
                LocalName = "Stage Due Date updated",
                ObjectTableId = quoteObject.Id,
                ShortView = true,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "DCQT",
                EnglishName = "Quote Disconnected",
                Tenant = 0,
                AddedManually = false,
                LocalName = "Quote Disconnected",
                ObjectTableId = quoteObject.Id,
                ShortView = true,
            }, EventTypeRepository, tenantEventTypes);

            #endregion

            #region Shipment

            #region Ticket Events 
            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "STCN",
                EnglishName = "Ticket Connected",
                Tenant = 0,
                AddedManually = false,
                LocalName = "Ticket Connected",
                ObjectTableId = shipmentObject.Id,
                ShortView = false,
                EventTypeCategoryCode = "OPE",
            }, EventTypeRepository, tenantEventTypes);
            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "STDC",
                EnglishName = "Ticket Disconnected",
                Tenant = 0,
                AddedManually = false,
                LocalName = "Ticket Disconnected",
                ObjectTableId = shipmentObject.Id,
                ShortView = false,
                EventTypeCategoryCode = "OPE",
            }, EventTypeRepository, tenantEventTypes);

            #endregion 

            #region With Status
            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "ORDR",
                EnglishName = "Created",
                Tenant = 0,
                LocalName = "Created",
                ObjectTableId = shipmentObject.Id,
                EntityStatusId = tenantEntityStatus.Where(d => d.Tenant == 0 && d.Code == "SHOR").FirstOrDefault().Id,
                ShortView = true,
                EventTypeCategoryCode = "OPE",
                //StatusWeight = 0:Order,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "PICD",
                EnglishName = "Picked Up",
                Tenant = 0,
                LocalName = "Picked Up",
                ObjectTableId = shipmentObject.Id,
                EntityStatusId = tenantEntityStatus.Where(d => d.Tenant == 0 && d.Code == "SHPK").FirstOrDefault().Id,
                ShortView = true,
                FollowUpEnglishName = "Pick Up",
                FollowUpLocalName = "Pick Up",
                IsFollowUp = true,
                EventTypeCategoryCode = "LEG",
                IsCustomerView = true,
                IsSharedLogisticsEnabled = true,
                //StatusWeight = 1:Pick Up: ATD,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "PCAR",
                EnglishName = "Pick up arranged",
                Tenant = 0,
                LocalName = "Pick up arranged",
                IsFollowUp = true,
                FollowUpEnglishName = "Pick up Arrangement",
                FollowUpLocalName = "Pick up Arrangement",
                ManualActivatedFollowUp = true,
                AllowedInAutomation  =true,
                ObjectTableId = shipmentObject.Id,
                EntityStatusId = tenantEntityStatus.Where(d => d.Tenant == 0 && d.Code == "SHP2").FirstOrDefault().Id,
                EventTypeCategoryCode = "LEG",
                //StatusWeight = 1:Pick Up,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "RCS",
                EnglishName = "On Hand",
                Tenant = 0,
                IsManualEntry = true,
                LocalName = "On Hand",
                ObjectTableId = shipmentObject.Id,
                EntityStatusId = tenantEntityStatus.Where(d => d.Tenant == 0 && d.Code == "SHON").FirstOrDefault().Id,
                IsFollowUp = true,
                FollowUpLocalName = "On Hand",
                FollowUpEnglishName = "On Hand",
                EventTypeCategoryCode = "LEG",
                ShortView = true,
                IsCustomerView = true,
                IsAgentView = true,
                IsSharedLogisticsEnabled = true,
                //StatusWeight = 2:On Hand: ATA,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "PRCD",
                EnglishName = "Pre Carriage Departed",
                Tenant = 0,
                LocalName = "Pre Carriage Departed",
                IsFollowUp = true,
                FollowUpEnglishName = "Pre carriage Departure",
                FollowUpLocalName = "Pre carriage Departure",
                ObjectTableId = shipmentObject.Id,
                EntityStatusId = tenantEntityStatus.Where(d => d.Tenant == 0 && d.Code == "SDE5").FirstOrDefault().Id,
                ShortView = true,
                EventTypeCategoryCode = "LEG",
                IsCustomerView = true,
                IsSharedLogisticsEnabled = true,
                //StatusWeight = 3:Departed: PreCarriageATD,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "FRRL",
                EnglishName = "Freight Release",
                Tenant = 0,
                LocalName = "Freight Release",
                IsFollowUp = true,
                FollowUpEnglishName = "Freight Release",
                FollowUpLocalName = "Freight Release",
                ObjectTableId = shipmentObject.Id,
                ShortView = true,
                EventTypeCategoryCode = "LEG",
                IsCustomerView = true,
                IsSharedLogisticsEnabled = true,
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "TRAV",
                EnglishName = "Terminal Available",
                Tenant = 0,
                LocalName = "Terminal Available",
                IsFollowUp = true,
                FollowUpEnglishName = "Terminal Availability",
                FollowUpLocalName = "Terminal Availability",
                ObjectTableId = shipmentObject.Id,
                ShortView = true,
                EventTypeCategoryCode = "LEG",
                IsCustomerView = true,
                IsSharedLogisticsEnabled = true,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "PRCA",
                EnglishName = "Pre Carriage Arrived",
                Tenant = 0,
                LocalName = "Pre Carriage Arrived",
                IsFollowUp = true,
                FollowUpEnglishName = "Pre carriage Arrival",
                FollowUpLocalName = "Pre carriage Arrival",
                ObjectTableId = shipmentObject.Id,
                EntityStatusId = tenantEntityStatus.Where(d => d.Tenant == 0 && d.Code == "PRCA").FirstOrDefault().Id,
                ShortView = true,
                EventTypeCategoryCode = "LEG",
                IsCustomerView = true,
                IsSharedLogisticsEnabled = true,
                //StatusWeight = 4:Arrived: PreCarriageATA,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "DEP",
                EnglishName = "Departed",
                Tenant = 0,
                LocalName = "Departed",
                IsFollowUp = true,
                FollowUpEnglishName = "Main Departure",
                FollowUpLocalName = "Main Departure",
                ObjectTableId = shipmentObject.Id,
                EntityStatusId = tenantEntityStatus.Where(d => d.Tenant == 0 && d.Code == "SDEP").FirstOrDefault().Id,
                ShortView = true,
                EventTypeCategoryCode = "LEG",
                IsCustomerView = true,
                IsAgentView = true,
                IsSharedLogisticsEnabled = true,
                //StatusWeight = 5:Departed: MainCarriageATD,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "ARR",
                EnglishName = "Arrived at Destination",
                Tenant = 0,
                LocalName = "Arrived at Destination",
                IsFollowUp = true,
                FollowUpLocalName = "Main Arrival",
                FollowUpEnglishName = "Main Arrival",
                ObjectTableId = shipmentObject.Id,
                EntityStatusId = tenantEntityStatus.Where(d => d.Tenant == 0 && d.Code == "SARR").FirstOrDefault().Id,
                ShortView = true,
                EventTypeCategoryCode = "LEG",
                IsCustomerView = true,
                IsAgentView = true,
                IsSharedLogisticsEnabled = true,
                //StatusWeight = 6:Arrived:MainCarriageATA,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "T1DP",
                EnglishName = "Leg1 Departed",
                Tenant = 0,
                LocalName = "Leg1 Departed",
                IsFollowUp = true,
                FollowUpEnglishName = "Leg1 Departure",
                FollowUpLocalName = "Leg1 Departure",
                ObjectTableId = shipmentObject.Id,
                EntityStatusId = tenantEntityStatus.Where(d => d.Tenant == 0 && d.Code == "SDE2").FirstOrDefault().Id,
                ShortView = true,
                EventTypeCategoryCode = "LEG",
                IsCustomerView = true,
                IsAgentView = true,
                IsSharedLogisticsEnabled = true,
                //StatusWeight = 7:Departed:Transshipment1ATD,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "T1AR",
                EnglishName = "Leg1 Arrived at Destination",
                Tenant = 0,
                LocalName = "Leg1 Arrived at Destination",
                IsFollowUp = true,
                FollowUpLocalName = "Leg1 Arrival",
                FollowUpEnglishName = "Leg1 Arrival",
                ObjectTableId = shipmentObject.Id,
                EntityStatusId = tenantEntityStatus.Where(d => d.Tenant == 0 && d.Code == "SAR2").FirstOrDefault().Id,
                ShortView = true,
                EventTypeCategoryCode = "LEG",
                IsCustomerView = true,
                IsAgentView = true,
                IsSharedLogisticsEnabled = true,
                //StatusWeight = 8:Arrived:Transshipment1ATA,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "T2DP",
                EnglishName = "Leg2 Departed",
                Tenant = 0,
                LocalName = "Leg2 Departed",
                IsFollowUp = true,
                FollowUpEnglishName = "Leg2 Departure",
                FollowUpLocalName = "Leg2 Departure",
                ObjectTableId = shipmentObject.Id,
                EntityStatusId = tenantEntityStatus.Where(d => d.Tenant == 0 && d.Code == "SDE3").FirstOrDefault().Id,
                ShortView = true,
                EventTypeCategoryCode = "LEG",
                IsCustomerView = true,
                IsAgentView = true,
                IsSharedLogisticsEnabled = true,
                //StatusWeight = 9:Departed:Transshipment2ATD,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "T2AR",
                EnglishName = "Leg2 Arrived at Destination",
                Tenant = 0,
                LocalName = "Leg2 Arrived at Destination",
                IsFollowUp = true,
                FollowUpLocalName = "Leg2 Arrival",
                FollowUpEnglishName = "Leg2 Arrival",
                ObjectTableId = shipmentObject.Id,
                EntityStatusId = tenantEntityStatus.Where(d => d.Tenant == 0 && d.Code == "SAR3").FirstOrDefault().Id,
                ShortView = true,
                EventTypeCategoryCode = "LEG",
                IsCustomerView = true,
                IsAgentView = true,
                IsSharedLogisticsEnabled = true,
                //StatusWeight = 10:Arrived:Transshipment2ATA,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "T3DP",
                EnglishName = "Leg3 Departed",
                Tenant = 0,
                LocalName = "Leg3 Departed",
                IsFollowUp = true,
                FollowUpEnglishName = "Leg3 Departed",
                FollowUpLocalName = "Leg3 Departed",
                ObjectTableId = shipmentObject.Id,
                EntityStatusId = tenantEntityStatus.Where(d => d.Tenant == 0 && d.Code == "SDE4").FirstOrDefault().Id,
                ShortView = true,
                EventTypeCategoryCode = "LEG",
                IsCustomerView = true,
                IsAgentView = true,
                IsSharedLogisticsEnabled = true,
                //StatusWeight = 11:Departed:Transshipment3ATD,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "T3AR",
                EnglishName = "Leg3 Arrived at Destination",
                Tenant = 0,
                LocalName = "Leg3 Arrived at Destination",
                IsFollowUp = true,
                FollowUpLocalName = "Leg3 Arrival",
                FollowUpEnglishName = "Leg3 Arrival",
                ObjectTableId = shipmentObject.Id,
                EntityStatusId = tenantEntityStatus.Where(d => d.Tenant == 0 && d.Code == "SAR4").FirstOrDefault().Id,
                ShortView = true,
                EventTypeCategoryCode = "LEG",
                IsCustomerView = true,
                IsAgentView = true,
                IsSharedLogisticsEnabled = true,
                //StatusWeight = 12:Arrived:Transshipment3ATA,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "ONCD",
                EnglishName = "On Carriage Departed",
                Tenant = 0,
                LocalName = "On Carriage Departed",
                IsFollowUp = true,
                FollowUpEnglishName = "On carriage Departure",
                FollowUpLocalName = "On carriage Departure",
                ObjectTableId = shipmentObject.Id,
                EntityStatusId = tenantEntityStatus.Where(d => d.Tenant == 0 && d.Code == "ONCD").FirstOrDefault().Id,
                ShortView = true,
                EventTypeCategoryCode = "LEG",
                IsCustomerView = true,
                IsAgentView = true,
                IsSharedLogisticsEnabled = true,
                //StatusWeight = 13:Departed:OnCarriageATD,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "ONCA",
                EnglishName = "On Carriage Arrived",
                Tenant = 0,
                LocalName = "On Carriage Arrived",
                IsFollowUp = true,
                FollowUpEnglishName = "On carriage Arrival",
                FollowUpLocalName = "On carriage Arrival",
                ObjectTableId = shipmentObject.Id,
                EntityStatusId = tenantEntityStatus.Where(d => d.Tenant == 0 && d.Code == "SAR5").FirstOrDefault().Id,
                ShortView = true,
                EventTypeCategoryCode = "LEG",
                IsCustomerView = true,
                IsAgentView = true,
                IsSharedLogisticsEnabled = true,
                //StatusWeight = 14:Arrived:OnCarriageATA,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "CERT",
                EnglishName = "Pending Import Formalities",
                LocalName = "Pending Import Formalities",
                Tenant = 0,
                IsManualEntry = true,
                ObjectTableId = shipmentObject.Id,
                EntityStatusId = tenantEntityStatus.Where(d => d.Tenant == 0 && d.Code == "CERT").FirstOrDefault().Id,
                
                ShortView = true,
                EventTypeCategoryCode = "OPE",
                //StatusWeight = 15,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "ECCL",
                EnglishName = "Export Custom Clearance",
                LocalName = "Export Custom Clearance",
                Tenant = 0,
                ObjectTableId = shipmentObject.Id,
                EntityStatusId = tenantEntityStatus.Where(d => d.Tenant == 0 && d.Code == "ECCL").FirstOrDefault().Id,
                IsManualEntry=true,
                IsCustomerView = true,
                IsAgentView = true,
                ShortView = true,               
                EventTypeCategoryCode = "OPE",
                //StatusWeight = 3,
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "ICCL",
                EnglishName = "Import Custom Clearance",
                LocalName = "Import Custom Clearance",
                Tenant = 0,
                ObjectTableId = shipmentObject.Id,
                EntityStatusId = tenantEntityStatus.Where(d => d.Tenant == 0 && d.Code == "ICCL").FirstOrDefault().Id,
                IsManualEntry = true,
                IsCustomerView = true,
                IsAgentView = true,
                ShortView = true,
                EventTypeCategoryCode = "OPE",
                //StatusWeight = 17,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "CCD",
                EnglishName = "Custom Cleared",
                Tenant = 0,
                IsManualEntry = true,
                LocalName = "Custom Cleared",
                IsFollowUp = true,
                FollowUpEnglishName = "Custom Clearing",
                FollowUpLocalName = "Custom Clearing",
                ObjectTableId = shipmentObject.Id,
                EntityStatusId = tenantEntityStatus.Where(d => d.Tenant == 0 && d.Code == "SHCL").FirstOrDefault().Id,
                ShortView = true,
                ManualActivatedFollowUp = true,
                AllowedInAutomation = true,
                IsCustomerView = true,
                IsAgentView = true,
                EventTypeCategoryCode = "OPE",
                IsSharedLogisticsEnabled = true,
                //StatusWeight = 16,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "DELD",
                EnglishName = "Delivery Departed",
                Tenant = 0,
                LocalName = "Delivery Departed",
                ObjectTableId = shipmentObject.Id,
                EntityStatusId = tenantEntityStatus.Where(d => d.Tenant == 0 && d.Code == "SDLY").FirstOrDefault().Id,
                ShortView = true,
                FollowUpLocalName = "Delivery Departed",
                FollowUpEnglishName = "Delivery Departed",
                IsFollowUp = true,
                EventTypeCategoryCode = "LEG",
                IsCustomerView = true,
                IsAgentView = true,
                IsSharedLogisticsEnabled = true,
                //StatusWeight = 17,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "DLAR",
                EnglishName = "Delivery Arranged",
                Tenant = 0,
                IsManualEntry = true,
                LocalName = "Delivery Arranged",
                IsFollowUp = true,
                FollowUpEnglishName = "Delivery Arrangement",
                FollowUpLocalName = "Delivery Arrangement",
                ObjectTableId = shipmentObject.Id,
                EntityStatusId = tenantEntityStatus.Where(d => d.Tenant == 0 && d.Code == "SDL2").FirstOrDefault().Id,
                ShortView = true,
                EventTypeCategoryCode = "LEG",
                //StatusWeight = 17,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "DDCU",
                EnglishName = "Documents delivered to custom agent",
                Tenant = 0,
                IsManualEntry = true,
                LocalName = "Documents delivered to custom agent",
                IsFollowUp = true,
                FollowUpEnglishName = "Deliver Docs",
                FollowUpLocalName = "Deliver Docs",
                ObjectTableId = shipmentObject.Id,
                EntityStatusId = tenantEntityStatus.Where(d => d.Tenant == 0 && d.Code == "DTCA").FirstOrDefault().Id,
                ShortView = true,
                IsCustomerView = true,
                IsAgentView = true,
                EventTypeCategoryCode = "LEG",
                //StatusWeight = 18,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "PIOD",
                EnglishName = "POD",
                Tenant = 0,
                IsManualEntry = true,
                LocalName = "POD",
                ObjectTableId = shipmentObject.Id,
                EntityStatusId = tenantEntityStatus.Where(d => d.Tenant == 0 && d.Code == "SDLD").FirstOrDefault().Id,
                ShortView = true,
                FollowUpLocalName = "POD",
                FollowUpEnglishName = "POD",
                IsFollowUp = true,
                EventTypeCategoryCode = "LEG",
                IsCustomerView = true,
                IsSharedLogisticsEnabled = true,
                //StatusWeight = 19,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "AFD",
                EnglishName = "Available for Delivery",
                Tenant = 0,
                IsManualEntry = true,
                LocalName = "Available for Delivery",
                IsFollowUp = true,
                FollowUpEnglishName = "Available for Delivery",
                FollowUpLocalName = "Available for Delivery",
                ObjectTableId = shipmentObject.Id,
                EntityStatusId = tenantEntityStatus.Where(d => d.Tenant == 0 && d.Code == "SAFD").FirstOrDefault().Id,
                ShortView = true,
                EventTypeCategoryCode = "LEG",
                IsCustomerView = true,
                IsAgentView = true,
                IsSharedLogisticsEnabled = true,
                //StatusWeight = 19,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "WHED",
                EnglishName = "Warehouse Entry",
                Tenant = 0,
                IsManualEntry = true,
                LocalName = "Warehouse Entry",
                IsFollowUp = true,
                FollowUpEnglishName = "Warehouse Entry",
                FollowUpLocalName = "Warehouse Entry",
                ObjectTableId = shipmentObject.Id,
                EntityStatusId = tenantEntityStatus.Where(d => d.Tenant == 0 && d.Code == "INWH").FirstOrDefault().Id,
                ShortView = true,
                EventTypeCategoryCode = "LEG",
                IsCustomerView = true,
                IsAgentView = true,
                IsSharedLogisticsEnabled = true,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "WHRD",
                EnglishName = "Warehouse Release",
                Tenant = 0,
                IsManualEntry = true,
                LocalName = "Warehouse Release",
                IsFollowUp = true,
                FollowUpEnglishName = "Warehouse Release",
                FollowUpLocalName = "Warehouse Release",
                EntityStatusId = tenantEntityStatus.Where(d => d.Tenant == 0 && d.Code == "SRIM").FirstOrDefault().Id,
                ObjectTableId = shipmentObject.Id,
                ShortView = true,
                EventTypeCategoryCode = "LEG",
                IsCustomerView = true,
                IsAgentView = true,
                IsSharedLogisticsEnabled = true,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "ETD",
                EnglishName = "ETD",
                Tenant = 0,
                LocalName = "ETD",
                ObjectTableId = shipmentObject.Id,
                //EntityStatusId = tenantEntityStatus.Where(d => d.Tenant == 0 && d.Code == "ETD").FirstOrDefault().Id,
                ShortView = true,
                EventTypeCategoryCode = "LEG",
                IsSharedLogisticsEnabled = true,
                AllowedInAutomation = true,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "ETA",
                EnglishName = "ETA",
                Tenant = 0,
                LocalName = "ETA",
                ObjectTableId = shipmentObject.Id,
                //EntityStatusId = tenantEntityStatus.Where(d => d.Tenant == 0 && d.Code == "ETA").FirstOrDefault().Id,
                ShortView = true,
                EventTypeCategoryCode = "LEG",
                IsSharedLogisticsEnabled = true,
                AllowedInAutomation = true,
            }, EventTypeRepository, tenantEventTypes);

            #endregion

            #region AWB Events
            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "ARRE",
                EnglishName = "Arrived",
                Tenant = 0,
                AddedManually = false,
                LocalName = "Arrived",
                ObjectTableId = shipmentObject.Id,
                ShortView = false,
                EventTypeCategoryCode = "LOG",
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "AWDE",
                EnglishName = "Arrival documents delivery",
                Tenant = 0,
                AddedManually = false,
                LocalName = "Arrival documents delivery",
                ObjectTableId = shipmentObject.Id,
                ShortView = true,
                EventTypeCategoryCode = "LOG",
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "AWRE",
                EnglishName = "Arrival documents received ",
                Tenant = 0,
                AddedManually = false,
                LocalName = "Arrival documents received ",
                ObjectTableId = shipmentObject.Id,
                ShortView = false,
                EventTypeCategoryCode = "LOG",
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "BKDE",
                EnglishName = "Booked",
                Tenant = 0,
                AddedManually = false,
                LocalName = "Booked",
                ObjectTableId = shipmentObject.Id,
                ShortView = false,
                EventTypeCategoryCode = "LOG",
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "CCDE",
                EnglishName = "Cleared by Customs",
                Tenant = 0,
                AddedManually = false,
                LocalName = "Cleared by Customs",
                ObjectTableId = shipmentObject.Id,
                ShortView = false,
                EventTypeCategoryCode = "LOG",
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "CRCE",
                EnglishName = "Reported by Customs",
                Tenant = 0,
                AddedManually = false,
                LocalName = "Reported by Customs",
                ObjectTableId = shipmentObject.Id,
                ShortView = false,
                EventTypeCategoryCode = "LOG",
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "DDLE",
                EnglishName = "Door-delivery to consignee",
                Tenant = 0,
                AddedManually = false,
                LocalName = "Door-delivery to consignee",
                ObjectTableId = shipmentObject.Id,
                ShortView = false,
                EventTypeCategoryCode = "LOG",
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "DEPE",
                EnglishName = "Departed",
                Tenant = 0,
                AddedManually = false,
                LocalName = "Departed",
                ObjectTableId = shipmentObject.Id,
                ShortView = false,
                EventTypeCategoryCode = "LOG",
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "DISE",
                EnglishName = "Discrepancy",
                Tenant = 0,
                AddedManually = false,
                LocalName = "Discrepancy",
                ObjectTableId = shipmentObject.Id,
                ShortView = false,
                EventTypeCategoryCode = "LOG",
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "DLVE",
                EnglishName = "Delivered",
                Tenant = 0,
                AddedManually = false,
                LocalName = "Delivered",
                ObjectTableId = shipmentObject.Id,
                ShortView = false,
                EventTypeCategoryCode = "LOG",
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "DOCE",
                EnglishName = "Documents Received",
                Tenant = 0,
                AddedManually = false,
                LocalName = "Documents Received",
                ObjectTableId = shipmentObject.Id,
                ShortView = false,
                EventTypeCategoryCode = "LOG",
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "FOHE",
                EnglishName = "On-Hand",
                Tenant = 0,
                AddedManually = false,
                LocalName = "On-Hand",
                ObjectTableId = shipmentObject.Id,
                ShortView = false,
                EventTypeCategoryCode = "LOG",
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "MANE",
                EnglishName = "Manifested",
                Tenant = 0,
                AddedManually = false,
                LocalName = "Manifested",
                ObjectTableId = shipmentObject.Id,
                ShortView = false,
                EventTypeCategoryCode = "LOG",
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "NFDE",
                EnglishName = "Notify about arrival",
                Tenant = 0,
                AddedManually = false,
                LocalName = "Notify about arrival",
                ObjectTableId = shipmentObject.Id,
                ShortView = false,
                EventTypeCategoryCode = "LOG",
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "PREE",
                EnglishName = "In Preparation",
                Tenant = 0,
                AddedManually = false,
                LocalName = "In Preparation",
                ObjectTableId = shipmentObject.Id,
                ShortView = false,
                EventTypeCategoryCode = "LOG",
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "RCFE",
                EnglishName = "Received from Flight",
                Tenant = 0,
                AddedManually = false,
                LocalName = "Received from Flight",
                ObjectTableId = shipmentObject.Id,
                ShortView = false,
                EventTypeCategoryCode = "LOG",
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "RCSE",
                EnglishName = "Received from Shipper",
                Tenant = 0,
                AddedManually = false,
                LocalName = "Received from Shipper",
                ObjectTableId = shipmentObject.Id,
                ShortView = false,
                EventTypeCategoryCode = "LOG",
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "RCTE",
                EnglishName = "Received from Transfer",
                Tenant = 0,
                AddedManually = false,
                LocalName = "Received from Transfer",
                ObjectTableId = shipmentObject.Id,
                ShortView = false,
                EventTypeCategoryCode = "LOG",
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "TFDE",
                EnglishName = "Transferred",
                Tenant = 0,
                AddedManually = false,
                LocalName = "Transferred",
                ObjectTableId = shipmentObject.Id,
                ShortView = false,
                EventTypeCategoryCode = "LOG",
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "TGCE",
                EnglishName = "Consigement tranferred to customs",
                Tenant = 0,
                AddedManually = false,
                LocalName = "Consigement tranferred to customs",
                ObjectTableId = shipmentObject.Id,
                ShortView = false,
                EventTypeCategoryCode = "LOG",
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "TRME",
                EnglishName = "To be Transferred",
                Tenant = 0,
                AddedManually = false,
                LocalName = "To be Transferred",
                ObjectTableId = shipmentObject.Id,
                ShortView = false,
                EventTypeCategoryCode = "LOG",
            }, EventTypeRepository, tenantEventTypes);

            #endregion

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "USHI",
                EnglishName = "Shipment Updated",
                Tenant = 0,
                AddedManually = false,
                LocalName = "Shipment Updated",
                ObjectTableId = shipmentObject.Id,
                ShortView = false,
                EventTypeCategoryCode = "LOG",
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "DELY",
                EnglishName = "Delay",
                Tenant = 0,
                AddedManually = false,
                IsManualEntry = true,
                LocalName = "Delay",
                ObjectTableId = shipmentObject.Id,
                ShortView = true,
                IsCustomerView = true,
                IsAgentView = true,
                EventTypeCategoryCode = "LEG",
                IsSharedLogisticsEnabled = true,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "DOCI",
                EnglishName = "Document Received",
                Tenant = 0,
                AddedManually = false,
                IsManualEntry = false,
                LocalName = "Document Received",
                ObjectTableId = shipmentObject.Id,
                ShortView = false,
                FollowUpLocalName = "Waiting for Documentation",
                FollowUpEnglishName = "Waiting for Documentation",
                IsFollowUp = true,
                ManualActivatedFollowUp = false,
                EventTypeCategoryCode = "DOC",
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "DOCO",
                EnglishName = "Document Printed",
                Tenant = 0,
                AddedManually = false,
                IsManualEntry = false,
                LocalName = "Document Printed",
                ObjectTableId = shipmentObject.Id,
                ShortView = false,
                FollowUpLocalName = "Print Documents",
                FollowUpEnglishName = "Print Documents",
                IsFollowUp = true,
                ManualActivatedFollowUp = false,
                EventTypeCategoryCode = "DOC",
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "REMF",
                EnglishName = "Reminder",
                Tenant = 0,
                AddedManually = false,
                IsManualEntry = true,
                LocalName = "Reminder",
                ObjectTableId = shipmentObject.Id,
                ShortView = true,
                FollowUpLocalName = "Reminder",
                FollowUpEnglishName = "Reminder",
                IsFollowUp = true,
                ManualActivatedFollowUp = true,
                AllowedInAutomation = true,
                EventTypeCategoryCode = "LOG",
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "BKD",
                EnglishName = "Booked",
                Tenant = 0,
                AddedManually = false,
                IsManualEntry = false,
                LocalName = "Booked",
                IsFollowUp = true,
                FollowUpEnglishName = "Booking Arrangement",
                FollowUpLocalName = "Booking Arrangement",
                ManualActivatedFollowUp = true,
                AllowedInAutomation = true,
                ObjectTableId = shipmentObject.Id,
                ShortView = true,
                IsCustomerView = true,
                IsAgentView = true,
                EventTypeCategoryCode = "OPE",
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "BKCF",
                EnglishName = "Booking Confirmation",
                Tenant = 0,
                AddedManually = false,
                IsManualEntry = false,
                LocalName = "Booking Confirmation",
                IsFollowUp = false,
                ObjectTableId = shipmentObject.Id,
                ShortView = true,
                IsCustomerView = true,
                IsAgentView = true,
                EventTypeCategoryCode = "OPE",
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "INPR",
                EnglishName = "Invoice Prepared",
                Tenant = 0,
                AddedManually = false,
                IsManualEntry = false,
                LocalName = "Invoice Prepared",
                IsFollowUp = true,
                FollowUpEnglishName = "Invoicing",
                FollowUpLocalName = "Invoicing",
                ObjectTableId = shipmentObject.Id,
                ShortView = false,
                ManualActivatedFollowUp = true,
                AllowedInAutomation = true,
                EventTypeCategoryCode = "OPE",
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "DAMG",
                EnglishName = "Damaged",
                Tenant = 0,
                AddedManually = false,
                IsManualEntry = true,
                LocalName = "Damaged",
                ObjectTableId = shipmentObject.Id,
                ShortView = true,
                ManualActivatedFollowUp = false,
                IsCustomerView = true,
                IsAgentView = true,
                EventTypeCategoryCode = "OPE",
                IsSharedLogisticsEnabled = true,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "CUDL",
                EnglishName = "Custom Delay",
                Tenant = 0,
                AddedManually = false,
                IsManualEntry = true,
                LocalName = "Custom Delay",
                ObjectTableId = shipmentObject.Id,
                ShortView = true,
                ManualActivatedFollowUp = false,
                IsCustomerView = true,
                IsAgentView = true,
                EventTypeCategoryCode = "LEG",
                IsSharedLogisticsEnabled = true,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "CUIN",
                EnglishName = "Custom Inspection",
                Tenant = 0,
                AddedManually = false,
                IsManualEntry = true,
                LocalName = "Custom Inspection",
                ObjectTableId = shipmentObject.Id,
                ShortView = true,
                ManualActivatedFollowUp = false,
                IsCustomerView = true,
                IsAgentView = true,
                EventTypeCategoryCode = "LEG",
                IsSharedLogisticsEnabled = true,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "OPCL",
                EnglishName = "Operational Closed",
                Tenant = 0,
                AddedManually = false,
                LocalName = "Operational Closed",
                ObjectTableId = shipmentObject.Id,
                ShortView = true,
                EventTypeCategoryCode = "OPE",
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "OPOP",
                EnglishName = "Operational Opened",
                Tenant = 0,
                AddedManually = false,
                LocalName = "Operational Opened",
                ObjectTableId = shipmentObject.Id,
                ShortView = true,
                EventTypeCategoryCode = "OPE",
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "ACCL",
                EnglishName = "Accounting Closed",
                Tenant = 0,
                AddedManually = false,
                LocalName = "Accounting Closed",
                ObjectTableId = shipmentObject.Id,
                ShortView = true,
                EventTypeCategoryCode = "OPE",
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "ACOP",
                EnglishName = "Accounting Opened",
                Tenant = 0,
                AddedManually = false,
                LocalName = "Accounting Opened",
                ObjectTableId = shipmentObject.Id,
                ShortView = true,
                EventTypeCategoryCode = "OPE",
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "SCNL",
                EnglishName = "Cancelled",
                Tenant = 0,
                AddedManually = false,
                LocalName = "Cancelled",
                ObjectTableId = shipmentObject.Id,
                ShortView = true,
                EventTypeCategoryCode = "OPE",
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "SRAC",
                EnglishName = "Reactivated",
                Tenant = 0,
                AddedManually = false,
                LocalName = "Reactivated",
                ObjectTableId = shipmentObject.Id,
                ShortView = true,
                EventTypeCategoryCode = "OPE",
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "OFQT",
                EnglishName = "Opened From Quote",
                Tenant = 0,
                AddedManually = false,
                IsManualEntry = false,
                LocalName = "Opened From Quote",
                ObjectTableId = shipmentObject.Id,
                IsFollowUp = false,
                ShortView = true,
                IsCustomerView = true,
                EventTypeCategoryCode = "OPE",
                IsSharedLogisticsEnabled = true,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "CTQT",
                EnglishName = "Connected To Quote",
                Tenant = 0,
                LocalName = "Connected To Quote",
                ObjectTableId = shipmentObject.Id,
                ShortView = true,
                IsCustomerView = true,
                EventTypeCategoryCode = "OPE",
                IsSharedLogisticsEnabled = true,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "SLCN",
                EnglishName = "Salesman Changed",
                Tenant = 0,
                LocalName = "Salesman Changed",
                ObjectTableId = shipmentObject.Id,
                ShortView = true,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "AWBX",
                EnglishName = "AWB Retuned to Stack",
                Tenant = 0,
                AddedManually = false,
                IsManualEntry = false,
                LocalName = "AWB Retuned to Stack",
                ObjectTableId = shipmentObject.Id,
                ShortView = true,
                EventTypeCategoryCode = "LOG",
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "AWBS",
                EnglishName = "Got AWB number from stack",
                Tenant = 0,
                AddedManually = false,
                IsManualEntry = false,
                LocalName = "Got AWB number from stack",
                ObjectTableId = shipmentObject.Id,
                ShortView = true,
                EventTypeCategoryCode = "LOG",
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "CSHD",
                EnglishName = "Convert Shipment From House To Direct",
                Tenant = 0,
                AddedManually = false,
                LocalName = "Convert Shipment From House To Direct",
                ObjectTableId = shipmentObject.Id,
                ShortView = true,
                EventTypeCategoryCode = "LOG",
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "CSDH",
                EnglishName = "Convert Shipment From Direct To House",
                Tenant = 0,
                AddedManually = false,
                LocalName = "Convert Shipment From Direct To House",
                ObjectTableId = shipmentObject.Id,
                ShortView = true,
                EventTypeCategoryCode = "LOG",
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "CFAS",
                EnglishName = "Copied from another Shipment",
                Tenant = 0,
                AddedManually = false,
                IsManualEntry = false,
                LocalName = "Copied from another Shipment",
                ObjectTableId = shipmentObject.Id,
                IsFollowUp = false,
                ShortView = true,
                EventTypeCategoryCode = "LOG",
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "FWBS",
                EnglishName = "FWB Message Sent",
                Tenant = 0,
                AddedManually = false,
                IsManualEntry = false,
                LocalName = "FWB Message Sent",
                ObjectTableId = shipmentObject.Id,
                IsFollowUp = false,
                ShortView = true,
                EventTypeCategoryCode = "LOG",
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "FHLS",
                EnglishName = "FHL Message Sent",
                Tenant = 0,
                AddedManually = false,
                IsManualEntry = false,
                LocalName = "FHL Message Sent",
                ObjectTableId = shipmentObject.Id,
                IsFollowUp = false,
                ShortView = true,
                EventTypeCategoryCode = "LOG",
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "FNAR",
                EnglishName = "FNA Message Received",
                Tenant = 0,
                AddedManually = false,
                IsManualEntry = false,
                LocalName = "FNA Message Received",
                ObjectTableId = shipmentObject.Id,
                IsFollowUp = false,
                ShortView = true,
                EventTypeCategoryCode = "LOG",
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "FMAR",
                EnglishName = "FMA Message Received",
                Tenant = 0,
                AddedManually = false,
                IsManualEntry = false,
                LocalName = "FMA Message Received",
                ObjectTableId = shipmentObject.Id,
                IsFollowUp = false,
                ShortView = true,
                EventTypeCategoryCode = "LOG",
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "FSRS",
                EnglishName = "Status Request Sent",
                Tenant = 0,
                AddedManually = false,
                IsManualEntry = false,
                LocalName = "Status Request Sent",
                ObjectTableId = shipmentObject.Id,
                IsFollowUp = false,
                ShortView = true,
                EventTypeCategoryCode = "LOG",
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "FSAR",
                EnglishName = "Status Received",
                Tenant = 0,
                AddedManually = false,
                IsManualEntry = false,
                LocalName = "Status Received",
                ObjectTableId = shipmentObject.Id,
                IsFollowUp = false,
                ShortView = true,
                EventTypeCategoryCode = "LOG",
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "EXCE",
                EnglishName = "Exception",
                Tenant = 0,
                AddedManually = false,
                IsManualEntry = true,
                LocalName = "Exception",
                ObjectTableId = shipmentObject.Id,
                IsFollowUp = false,
                ShortView = true,
                EventTypeCategoryCode = "LOG",
            }, EventTypeRepository, tenantEventTypes);

             AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "EXRE",
                EnglishName = "Exception Resolved",
                Tenant = 0,
                AddedManually = false,
                IsManualEntry = true,
                LocalName = "Exception Resolved",
                ObjectTableId = shipmentObject.Id,
                IsFollowUp = false,
                ShortView = true,
                EventTypeCategoryCode = "LOG",
            }, EventTypeRepository, tenantEventTypes);
            
             AddEventTypes.AddEventType(new EventTypeDetails()
             {
                 Code = "AUCR",
                 EnglishName = "Automation Created",
                 Tenant = 0,
                 AddedManually = false,
                 IsManualEntry = true,
                 LocalName = "Automation Created",
                 ObjectTableId = automationObject.Id,
                 IsFollowUp = false,
                 ShortView = true,
                 EventTypeCategoryCode = "LOG",
             }, EventTypeRepository, tenantEventTypes);

             AddEventTypes.AddEventType(new EventTypeDetails()
             {
                 Code = "AUUP",
                 EnglishName = "Automation Updated",
                 Tenant = 0,
                 AddedManually = false,
                 IsManualEntry = true,
                 LocalName = "Automation Updated",
                 ObjectTableId = automationObject.Id,
                 IsFollowUp = false,
                 ShortView = true,
                 EventTypeCategoryCode = "LOG",
             }, EventTypeRepository, tenantEventTypes);
            
             AddEventTypes.AddEventType(new EventTypeDetails()
             {
                 Code = "AUSI",
                 EnglishName = "Set Automation as Inactive",
                 Tenant = 0,
                 AddedManually = false,
                 IsManualEntry = true,
                 LocalName = "Set Automation as Inactive",
                 ObjectTableId = automationObject.Id,
                 IsFollowUp = false,
                 ShortView = true,
                 EventTypeCategoryCode = "LOG",
             }, EventTypeRepository, tenantEventTypes); 
            
             AddEventTypes.AddEventType(new EventTypeDetails()
             {
                 Code = "AURE",
                 EnglishName = "Automation Reactivated",
                 Tenant = 0,
                 AddedManually = false,
                 IsManualEntry = true,
                 LocalName = "Automation Reactivated",
                 ObjectTableId = automationObject.Id,
                 IsFollowUp = false,
                 ShortView = true,
                 EventTypeCategoryCode = "LOG",
             }, EventTypeRepository, tenantEventTypes);
            
            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "CFSM",
                EnglishName = "Created From Shared Manifest",
                Tenant = 0,
                AddedManually = false,
                IsManualEntry = true,
                LocalName = "Created From Shared Manifest",
                ObjectTableId = shipmentObject.Id,
                IsFollowUp = false,
                ShortView = true,
                EventTypeCategoryCode = "LOG",
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "CUST",
                EnglishName = "Sent to Customs",
                Tenant = 0,
                AddedManually = false,
                IsManualEntry = false,
                LocalName = "Sent to Customs",
                ObjectTableId = shipmentObject.Id,
                IsFollowUp = false,
                ShortView = true,
                EventTypeCategoryCode = "LOG",
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "RPCP",
                EnglishName = "Receivables/Payables copied",
                Tenant = 0,
                AddedManually = false,
                LocalName = "Receivables/Payables copied",
                ObjectTableId = shipmentObject.Id,
                ShortView = true,
                EventTypeCategoryCode = "OPE",
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "ICUC",
                EnglishName = "Import Customs Clearance",
                Tenant = 0,
                AddedManually = false,
                LocalName = "Import Customs Clearance",
                ObjectTableId = shipmentObject.Id,
                ShortView = true,
                IsFollowUp=true,
                FollowUpEnglishName= "Import Customs Clearance",
                FollowUpLocalName = "Import Customs Clearance",
                EventTypeCategoryCode = "OPE",
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "ECUC",
                EnglishName = "Export Customs Clearance",
                Tenant = 0,
                AddedManually = false,
                LocalName = "Export Customs Clearance",
                ObjectTableId = shipmentObject.Id,
                ShortView = true,
                IsFollowUp = true,
                FollowUpEnglishName = "Export Customs Clearance",
                FollowUpLocalName = "Export Customs Clearance",
                EventTypeCategoryCode = "OPE",
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "CUCD",
                EnglishName = "Custom Cleared",
                Tenant = 0,
                AddedManually = false,
                LocalName = "Custom Cleared",
                ObjectTableId = shipmentObject.Id,
                ShortView = true,
                EventTypeCategoryCode = "OPE",
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "CUTO",
                EnglishName = "Cargo Cut-Off Updated",
                Tenant = 0,
                AddedManually = false,
                LocalName = "Cargo Cut-Off Updated",
                ObjectTableId = shipmentObject.Id,
                ShortView = true,
                IsFollowUp = true,
                FollowUpEnglishName = "Cargo Cut-Off",
                FollowUpLocalName = "Cargo Cut-Off",
                EventTypeCategoryCode = "OPE",
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "OBLD",
                EnglishName = "OBL Date Updated",
                Tenant = 0,
                AddedManually = false,
                LocalName = "OBL Date Updated",
                ObjectTableId = shipmentObject.Id,
                ShortView = true,
                IsFollowUp = true,
                FollowUpEnglishName = "OBL Date",
                FollowUpLocalName = "OBL Date",
                EventTypeCategoryCode = "OPE",
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "SPLT",
                EnglishName = "Split Shipment",               
                LocalName = "Split Shipment",
                ObjectTableId = shipmentObject.Id,
                Tenant = 0,
                ShortView = true,
                EventTypeCategoryCode = "OPE",
            }, EventTypeRepository, tenantEventTypes);

            #endregion

            #region Airline EventTypes

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "AWBA",
                EnglishName = "AWB Stack Added",
                Tenant = 0,
                AddedManually = false,
                IsManualEntry = false,
                LocalName = "AWB Stack Added",
                ObjectTableId = airlineObject.Id,
                ShortView = true,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "AWBR",
                EnglishName = "AWB Stack Removed",
                Tenant = 0,
                AddedManually = false,
                IsManualEntry = false,
                LocalName = "AWB Stack Removed",
                ObjectTableId = airlineObject.Id,
                ShortView = true,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "AWBD",
                EnglishName = "AWB Stack Removed By Date",
                Tenant = 0,
                AddedManually = false,
                IsManualEntry = false,
                LocalName = "AWB Stack Removed By Date",
                ObjectTableId = airlineObject.Id,
                ShortView = true,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "AWBF",
                EnglishName = "AWB Stack Returned By Shipment",
                Tenant = 0,
                AddedManually = false,
                IsManualEntry = false,
                LocalName = "AWB Stack Returned By Shipment",
                ObjectTableId = airlineObject.Id,
                ShortView = true,
            }, EventTypeRepository, tenantEventTypes);

            #endregion

            #region Customer
            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "CRCU",
                EnglishName = "Created",
                Tenant = 0,
                LocalName = "Created",
                ObjectTableId = customerObject.Id,
                EntityStatusId = tenantEntityStatus.Where(d => d.Tenant == 0 && d.Code == "CSCR").FirstOrDefault().Id,
                ShortView = true,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "POTC",
                EnglishName = "Created as Potential",
                Tenant = 0,
                LocalName = "Created as Potential",
                ObjectTableId = customerObject.Id,
                EntityStatusId = tenantEntityStatus.Where(d => d.Tenant == 0 && d.Code == "CSCR").FirstOrDefault().Id,
                ShortView = true,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "UPCU",
                EnglishName = "Customer Updated",
                Tenant = 0,
                LocalName = "Customer Updated",
                ObjectTableId = customerObject.Id,
                ShortView = true,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "CSWA",
                EnglishName = "To be Activated",
                Tenant = 0,
                LocalName = "To be Activated",
                ObjectTableId = customerObject.Id,
                ShortView = true,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "CSAV",
                EnglishName = "Customer Activated",
                Tenant = 0,
                LocalName = "Customer Activated",
                ObjectTableId = customerObject.Id,
                ShortView = true,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "CSIN",
                EnglishName = "Customer Deactivated",
                Tenant = 0,
                LocalName = "Customer Deactivated",
                ObjectTableId = customerObject.Id,
                ShortView = true,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "CSRA",
                EnglishName = "Customer Reactivated",
                Tenant = 0,
                LocalName = "Customer Reactivated",
                ObjectTableId = customerObject.Id,
                ShortView = true,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "CSMC",
                EnglishName = "Set as My Customer",
                Tenant = 0,
                LocalName = "Set as My Customer",
                ObjectTableId = customerObject.Id,
                ShortView = true,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "CSNC",
                EnglishName = "Set as Not My Customer",
                Tenant = 0,
                LocalName = "Set as Not My Customer",
                ObjectTableId = customerObject.Id,
                ShortView = true,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "CEMO",
                EnglishName = "Customer email out sent",
                Tenant = 0,
                LocalName = "Email out sent",
                ObjectTableId = customerObject.Id,
                ShortView = true,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "SLCH",
                EnglishName = "Salesman Changed",
                Tenant = 0,
                LocalName = "Salesman Changed",
                ObjectTableId = customerObject.Id,
                ShortView = true,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "PRUP",
                EnglishName = "Products Updated",
                Tenant = 0,
                LocalName = "Products Updated",
                ObjectTableId = customerObject.Id,
                ShortView = true,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "CMUP",
                EnglishName = "Competitors Updated",
                Tenant = 0,
                LocalName = "Competitors Updated",
                ObjectTableId = customerObject.Id,
                ShortView = true,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "ADUP",
                EnglishName = "Additional Services Updated",
                Tenant = 0,
                LocalName = "Additional Services Updated",
                ObjectTableId = customerObject.Id,
                ShortView = true,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "ARUP",
                EnglishName = "Addresses Updated",
                Tenant = 0,
                LocalName = "Addresses Updated",
                ObjectTableId = customerObject.Id,
                ShortView = true,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "CNUP",
                EnglishName = "Contacts Updated",
                Tenant = 0,
                LocalName = "Contacts Updated",
                ObjectTableId = customerObject.Id,
                ShortView = true,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "CACR",
                EnglishName = "Activity Created",
                Tenant = 0,
                LocalName = "Activity Created",
                ObjectTableId = customerObject.Id,
                ShortView = true,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "CACM",
                EnglishName = "Activity Completed",
                Tenant = 0,
                LocalName = "Activity Completed",
                ObjectTableId = customerObject.Id,
                ShortView = true,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "CARP",
                EnglishName = "Activity Reopened",
                Tenant = 0,
                LocalName = "Activity Reopened",
                ObjectTableId = customerObject.Id,
                ShortView = true,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "PWEN",
                EnglishName = "Activity Watch Enabled",
                Tenant = 0,
                LocalName = "Activity Watch Enabled",
                ObjectTableId = customerObject.Id,
                ShortView = true,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "PWDS",
                EnglishName = "Activity Watch Disabled",
                Tenant = 0,
                LocalName = "Activity Watch Disabled",
                ObjectTableId = customerObject.Id,
                ShortView = true,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "CRDL",
                EnglishName = "Credit limit details updated",                
                LocalName = "Credit limit details updated",
                Tenant = 0,
                ObjectTableId = customerObject.Id,
                ShortView = true,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "SPOT",
                EnglishName = "Set as Potential",
                LocalName = "Set as Potential",
                Tenant = 0,
                ObjectTableId = customerObject.Id,
                ShortView = true,
            }, EventTypeRepository, tenantEventTypes);
            #endregion

            #region Agent
            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "UPAG",
                EnglishName = "Agent Updated",
                Tenant = 0,
                AddedManually = false,
                LocalName = "Agent Updated",
                ObjectTableId = agentObject.Id,
                ShortView = false,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "CRAG",
                EnglishName = "Created",
                Tenant = 0,
                AddedManually = false,
                IsManualEntry = false,
                LocalName = "Created",
                ObjectTableId = agentObject.Id,
                ShortView = true,
            }, EventTypeRepository, tenantEventTypes);
            #endregion

            #region CustomAgent
            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "UPCA",
                EnglishName = "Custom Agent Updated",
                Tenant = 0,
                AddedManually = false,
                LocalName = "Custom Agent Updated",
                ObjectTableId = customAgentObject.Id,
                ShortView = false,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "CRCA",
                EnglishName = "Created",
                Tenant = 0,
                AddedManually = false,
                IsManualEntry = false,
                LocalName = "Created",
                ObjectTableId = customAgentObject.Id,
                ShortView = true,
            }, EventTypeRepository, tenantEventTypes);
            #endregion

            #region ShippingAgent
            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "UPSA",
                EnglishName = "Shipping Agent Updated",
                Tenant = 0,
                AddedManually = false,
                LocalName = "Shipping Agent Updated",
                ObjectTableId = shippingAgentObject.Id,
                ShortView = false,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "CRSA",
                EnglishName = "Created",
                Tenant = 0,
                AddedManually = false,
                IsManualEntry = false,
                LocalName = "Created",
                ObjectTableId = shippingAgentObject.Id,
                ShortView = true,
            }, EventTypeRepository, tenantEventTypes);
            #endregion

            #region Airline
            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "UPAL",
                EnglishName = "Airline Updated",
                Tenant = 0,
                AddedManually = false,
                LocalName = "Airline Updated",
                ObjectTableId = airlineObject.Id,
                ShortView = false,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "CRAL",
                EnglishName = "Created",
                Tenant = 0,
                AddedManually = false,
                IsManualEntry = false,
                LocalName = "Created",
                ObjectTableId = airlineObject.Id,
                ShortView = true,
            }, EventTypeRepository, tenantEventTypes);
            #endregion

            #region ShippingLine
            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "UPSL",
                EnglishName = "Shipping Line Updated",
                Tenant = 0,
                AddedManually = false,
                LocalName = "Shipping Line Updated",
                ObjectTableId = shippingLineObject.Id,
                ShortView = false,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "CRSL",
                EnglishName = "Created",
                Tenant = 0,
                AddedManually = false,
                IsManualEntry = false,
                LocalName = "Created",
                ObjectTableId = shippingLineObject.Id,
                ShortView = true,
            }, EventTypeRepository, tenantEventTypes);
            #endregion

            #region Trucker
            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "UPTR",
                EnglishName = "Trucker Updated",
                Tenant = 0,
                AddedManually = false,
                LocalName = "Trucker Updated",
                ObjectTableId = truckerObject.Id,
                ShortView = false,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "CRTR",
                EnglishName = "Created",
                Tenant = 0,
                AddedManually = false,
                IsManualEntry = false,
                LocalName = "Created",
                ObjectTableId = truckerObject.Id,
                ShortView = true,
            }, EventTypeRepository, tenantEventTypes);
            #endregion

            #region Incoterm
            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "UPIT",
                EnglishName = "Incoterm Updated",
                Tenant = 0,
                AddedManually = false,
                LocalName = "Incoterm Updated",
                ObjectTableId = incotermObject.Id,
                ShortView = false,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "CRIT",
                EnglishName = "Created",
                Tenant = 0,
                AddedManually = false,
                IsManualEntry = false,
                LocalName = "Created",
                ObjectTableId = incotermObject.Id,
                ShortView = true,
            }, EventTypeRepository, tenantEventTypes);
            #endregion

            #region PaymentTerm
            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "UPPT",
                EnglishName = "Payment Term Updated",
                Tenant = 0,
                AddedManually = false,
                LocalName = "Payment Term Updated",
                ObjectTableId = paymentTermObject.Id,
                ShortView = false,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "CRPT",
                EnglishName = "Created",
                Tenant = 0,
                AddedManually = false,
                IsManualEntry = false,
                LocalName = "Created",
                ObjectTableId = paymentTermObject.Id,
                ShortView = true,
            }, EventTypeRepository, tenantEventTypes);
            #endregion

            #region Currency
            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "UPCR",
                EnglishName = "Currency Updated",
                Tenant = 0,
                AddedManually = false,
                LocalName = "Currency Updated",
                ObjectTableId = currencyObject.Id,
                ShortView = false,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "CRCR",
                EnglishName = "Created",
                Tenant = 0,
                AddedManually = false,
                IsManualEntry = false,
                LocalName = "Created",
                ObjectTableId = currencyObject.Id,
                ShortView = true,
            }, EventTypeRepository, tenantEventTypes);
            #endregion

            #region VatType
            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "UPVT",
                EnglishName = "Vat Type Updated",
                Tenant = 0,
                AddedManually = false,
                LocalName = "Vat Type Updated",
                ObjectTableId = vatTypeObject.Id,
                ShortView = false,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "CRVT",
                EnglishName = "Created",
                Tenant = 0,
                AddedManually = false,
                IsManualEntry = false,
                LocalName = "Created",
                ObjectTableId = vatTypeObject.Id,
                ShortView = true,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "VTPD",
                Tenant = 0,
                EnglishName = "VAT Percentage edited",
                LocalName = "VAT Percentage edited",
                ObjectTableId = vatTypeObject.Id,
                ShortView = true,
            }, EventTypeRepository, tenantEventTypes);
            #endregion

            #region ChargeType
            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "UPCT",
                EnglishName = "Charges Type Updated",
                Tenant = 0,
                AddedManually = false,
                LocalName = "Charges Type Updated",
                ObjectTableId = chargeTypeObject.Id,
                ShortView = false,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "CRCT",
                EnglishName = "Created",
                Tenant = 0,
                AddedManually = false,
                IsManualEntry = false,
                LocalName = "Created",
                ObjectTableId = chargeTypeObject.Id,
                ShortView = true,
            }, EventTypeRepository, tenantEventTypes);
            #endregion

            #region Port
            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "UPPO",
                EnglishName = "Port Updated",
                Tenant = 0,
                AddedManually = false,
                LocalName = "Port Updated",
                ObjectTableId = portObject.Id,
                ShortView = false,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "CRPO",
                EnglishName = "Created",
                Tenant = 0,
                AddedManually = false,
                IsManualEntry = false,
                LocalName = "Created",
                ObjectTableId = portObject.Id,
                ShortView = true,
            }, EventTypeRepository, tenantEventTypes);
            #endregion

            #region Country
            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "UPCN",
                EnglishName = "Country Updated",
                Tenant = 0,
                AddedManually = false,
                LocalName = "Country Updated",
                ObjectTableId = countryObject.Id,
                ShortView = false,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "CRCN",
                EnglishName = "Created",
                Tenant = 0,
                AddedManually = false,
                IsManualEntry = false,
                LocalName = "Created",
                ObjectTableId = countryObject.Id,
                ShortView = true,
            }, EventTypeRepository, tenantEventTypes);
            #endregion

            #region GlobalZone
            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "UPGZ",
                EnglishName = "Global Zone Updated",
                Tenant = 0,
                AddedManually = false,
                LocalName = "Global Zone Updated",
                ObjectTableId = globalZoneObject.Id,
                ShortView = false,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "CRGZ",
                EnglishName = "Created",
                Tenant = 0,
                AddedManually = false,
                IsManualEntry = false,
                LocalName = "Created",
                ObjectTableId = globalZoneObject.Id,
                ShortView = true,
            }, EventTypeRepository, tenantEventTypes);
            #endregion

            #region Branch
            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "UPBR",
                EnglishName = "Branch Updated",
                Tenant = 0,
                AddedManually = false,
                LocalName = "Branch Updated",
                ObjectTableId = branchObject.Id,
                ShortView = false,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "CRBR",
                EnglishName = "Created",
                Tenant = 0,
                AddedManually = false,
                IsManualEntry = false,
                LocalName = "Created",
                ObjectTableId = branchObject.Id,
                ShortView = true,
            }, EventTypeRepository, tenantEventTypes);
            #endregion

            #region Department
            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "UPDP",
                EnglishName = "Department Updated",
                Tenant = 0,
                AddedManually = false,
                LocalName = "Department Updated",
                ObjectTableId = departmentObject.Id,
                ShortView = false,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "CRDP",
                EnglishName = "Created",
                Tenant = 0,
                AddedManually = false,
                IsManualEntry = false,
                LocalName = "Created",
                ObjectTableId = departmentObject.Id,
                ShortView = true,
            }, EventTypeRepository, tenantEventTypes);
            #endregion

            #region Contact
            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "UPCO",
                EnglishName = "Contact Updated",
                Tenant = 0,
                AddedManually = false,
                LocalName = "Contact Updated",
                ObjectTableId = contactObject.Id,
                ShortView = false,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "CRCO",
                EnglishName = "Created",
                Tenant = 0,
                AddedManually = false,
                IsManualEntry = false,
                LocalName = "Created",
                ObjectTableId = contactObject.Id,
                ShortView = true,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "HPCO",
                EnglishName = "Contact Felicitated",
                Tenant = 0,
                AddedManually = false,
                IsManualEntry = false,
                LocalName = "Contact Felicitated",
                ObjectTableId = contactObject.Id,
                ShortView = true,
            }, EventTypeRepository, tenantEventTypes);
            #endregion

            #region User
            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "UPUS",
                EnglishName = "User Updated",
                Tenant = 0,
                AddedManually = false,
                LocalName = "User Updated",
                ObjectTableId = userObject.Id,
                ShortView = false,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "CRUS",
                EnglishName = "Created",
                Tenant = 0,
                AddedManually = false,
                IsManualEntry = false,
                LocalName = "Created",
                ObjectTableId = userObject.Id,
                ShortView = true,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "EXUP",
                EnglishName = "Expiration Date Updated",
                Tenant = 0,
                AddedManually = false,
                LocalName = "Expiration Date Updated",
                ObjectTableId = userObject.Id,
                ShortView = true,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "RCUS",
                EnglishName = "Role Changed",
                Tenant = 0,
                AddedManually = false,
                LocalName = "Role Changed",
                ObjectTableId = userObject.Id,
                ShortView = true,
            }, EventTypeRepository, tenantEventTypes);
            #endregion

            #region State
            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "UPST",
                EnglishName = "State Updated",
                Tenant = 0,
                AddedManually = false,
                LocalName = "State Updated",
                ObjectTableId = stateObject.Id,
                ShortView = false,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "CRST",
                EnglishName = "Created",
                Tenant = 0,
                AddedManually = false,
                IsManualEntry = false,
                LocalName = "Created",
                ObjectTableId = stateObject.Id,
                ShortView = true,
            }, EventTypeRepository, tenantEventTypes);
            #endregion

            #region DocumentType
            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "UPDT",
                EnglishName = "Document Type Updated",
                Tenant = 0,
                AddedManually = false,
                LocalName = "Document Type Updated",
                ObjectTableId = documentTypeObject.Id,
                ShortView = false,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "CRDT",
                EnglishName = "Created",
                Tenant = 0,
                AddedManually = false,
                IsManualEntry = false,
                LocalName = "Created",
                ObjectTableId = documentTypeObject.Id,
                ShortView = true,
            }, EventTypeRepository, tenantEventTypes);
            #endregion

            #region EventType
            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "UPET",
                EnglishName = "Event Type Updated",
                Tenant = 0,
                AddedManually = false,
                LocalName = "Event Type Updated",
                ObjectTableId = eventTypeObject.Id,
                ShortView = false,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "CRET",
                EnglishName = "Created",
                Tenant = 0,
                AddedManually = false,
                IsManualEntry = false,
                LocalName = "Created",
                ObjectTableId = eventTypeObject.Id,
                ShortView = true,
            }, EventTypeRepository, tenantEventTypes);
            #endregion

            #region PackageType
            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "UPPK",
                EnglishName = "Package Type Updated",
                Tenant = 0,
                AddedManually = false,
                LocalName = "Package Type Updated",
                ObjectTableId = packageTypeObject.Id,
                ShortView = false,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "CRPK",
                EnglishName = "Created",
                Tenant = 0,
                AddedManually = false,
                IsManualEntry = false,
                LocalName = "Created",
                ObjectTableId = packageTypeObject.Id,
                ShortView = true,
            }, EventTypeRepository, tenantEventTypes);
            #endregion

            #region Vessel
            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "UPVS",
                EnglishName = "Vessel Updated",
                Tenant = 0,
                AddedManually = false,
                LocalName = "Vessel Updated",
                ObjectTableId = vesselObject.Id,
                ShortView = false,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "CRVS",
                EnglishName = "Created",
                Tenant = 0,
                AddedManually = false,
                IsManualEntry = false,
                LocalName = "Created",
                ObjectTableId = vesselObject.Id,
                ShortView = true,
            }, EventTypeRepository, tenantEventTypes);
            #endregion

            #region Warehouse
            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "UPWH",
                EnglishName = "Warehouse Updated",
                Tenant = 0,
                AddedManually = false,
                LocalName = "Warehouse Updated",
                ObjectTableId = warehouseObject.Id,
                ShortView = false,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "CRWH",
                EnglishName = "Created",
                Tenant = 0,
                AddedManually = false,
                IsManualEntry = false,
                LocalName = "Created",
                ObjectTableId = warehouseObject.Id,
                ShortView = true,
            }, EventTypeRepository, tenantEventTypes);
            #endregion

            #region Account
            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "UPAC",
                EnglishName = "Account Updated",
                Tenant = 0,
                AddedManually = false,
                LocalName = "Account Updated",
                ObjectTableId = accountObject.Id,
                ShortView = false,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "CRAC",
                EnglishName = "Created",
                Tenant = 0,
                AddedManually = false,
                IsManualEntry = false,
                LocalName = "Created",
                ObjectTableId = accountObject.Id,
                ShortView = true,
            }, EventTypeRepository, tenantEventTypes);
            #endregion

            #region Vendor
            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "UPVD",
                EnglishName = "Vendor Updated",
                Tenant = 0,
                AddedManually = false,
                LocalName = "Vendor Updated",
                ObjectTableId = vendorObject.Id,
                ShortView = false,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "CRVD",
                EnglishName = "Created",
                Tenant = 0,
                AddedManually = false,
                IsManualEntry = false,
                LocalName = "Created",
                ObjectTableId = vendorObject.Id,
                ShortView = true,
            }, EventTypeRepository, tenantEventTypes);
            #endregion

            #region ARInvoice

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "INTS",
                EnglishName = "Transferred to SAT",
                Tenant = 0,
                LocalName = "Transferred to SAT",
                ObjectTableId = arInvoiceObject.Id,
                ShortView = true,
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "INAS",
                EnglishName = "Approved by SAT",
                Tenant = 0,
                LocalName = "Approved by SAT",
                ObjectTableId = arInvoiceObject.Id,
                ShortView = true,
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "ISLC",
                EnglishName = "Salesman Changed",
                Tenant = 0,
                LocalName = "Salesman Changed",
                ObjectTableId = arInvoiceObject.Id,
                ShortView = true,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "REMF",
                EnglishName = "Reminder",
                Tenant = 0,
                IsManualEntry = true,
                LocalName = "Reminder",
                ObjectTableId = arInvoiceObject.Id,
                ShortView = true,
                FollowUpLocalName = "Reminder",
                FollowUpEnglishName = "Reminder",
                IsFollowUp = true,
                ManualActivatedFollowUp = true,
                EventTypeCategoryCode = "LOG",
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "UPIN",
                EnglishName = "Invoice Updated",
                Tenant = 0,
                LocalName = "Invoice Updated",
                ObjectTableId = arInvoiceObject.Id,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "CRIN",
                EnglishName = "Created",
                Tenant = 0,
                LocalName = "Created",
                ObjectTableId = arInvoiceObject.Id,
                ShortView = true,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails() { Code = "INAP", EnglishName = "Approved", LocalName = "Approved", Tenant = 0, ObjectTableId = arInvoiceObject.Id, ShortView = true, }, EventTypeRepository, tenantEventTypes);
            AddEventTypes.AddEventType(new EventTypeDetails() { Code = "INCA", EnglishName = "Canceled", LocalName = "Canceled", Tenant = 0, ObjectTableId = arInvoiceObject.Id, ShortView = true, }, EventTypeRepository, tenantEventTypes);
            AddEventTypes.AddEventType(new EventTypeDetails() { Code = "INVO", EnglishName = "Voided", LocalName = "Voided", Tenant = 0, ObjectTableId = arInvoiceObject.Id, ShortView = true, }, EventTypeRepository, tenantEventTypes);
            AddEventTypes.AddEventType(new EventTypeDetails() { Code = "INCN", EnglishName = "Constituent Connected", LocalName = "Constituent Connected", Tenant = 0, ObjectTableId = arInvoiceObject.Id, ShortView = true, }, EventTypeRepository, tenantEventTypes);
            AddEventTypes.AddEventType(new EventTypeDetails() { Code = "INDS", EnglishName = "Constituent Disconnected", LocalName = "Constituent Disconnected", Tenant = 0, ObjectTableId = arInvoiceObject.Id, ShortView = true, }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "INSE",
                EnglishName = "Invoice Sent",
                LocalName = "Invoice Sent",
                Tenant = 0,
                AddedManually = false,
                IsManualEntry = false,
                ObjectTableId = arInvoiceObject.Id,
                ShortView = true,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "INNS",
                EnglishName = "Return Invoice to Not Sent",
                LocalName = "Return Invoice to Not Sent",
                Tenant = 0,
                AddedManually = false,
                IsManualEntry = false,
                ObjectTableId = arInvoiceObject.Id,
                ShortView = true,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "INPD",
                EnglishName = "Invoice Paid",
                LocalName = "Invoice Paid",
                Tenant = 0,
                AddedManually = false,
                IsManualEntry = false,
                ObjectTableId = arInvoiceObject.Id,
                ShortView = true,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "INNP",
                EnglishName = "Return Invoice to Not Paid",
                LocalName = "Return Invoice to Not Paid",
                Tenant = 0,
                AddedManually = false,
                IsManualEntry = false,
                ObjectTableId = arInvoiceObject.Id,
                ShortView = true,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "COAR",
                EnglishName = "Payment Connected",
                Tenant = 0,
                LocalName = "Payment Connected",
                ObjectTableId = arInvoiceObject.Id,
                ShortView = true,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "ARID",
                EnglishName = "Payment Disconnected",
                Tenant = 0,
                LocalName = "Payment Disconnected",
                ObjectTableId = arInvoiceObject.Id,
                ShortView = true,
            }, EventTypeRepository, tenantEventTypes);

            #endregion

            #region APInvoice
            AddEventTypes.AddEventType(new EventTypeDetails() { Code = "APIA", EnglishName = "Approved", LocalName = "Approved", Tenant = 0, ObjectTableId = apInvoiceObject.Id, ShortView = true, }, EventTypeRepository, tenantEventTypes);
            AddEventTypes.AddEventType(new EventTypeDetails() { Code = "APIC", EnglishName = "Canceled", LocalName = "Canceled", Tenant = 0, ObjectTableId = apInvoiceObject.Id, ShortView = true, }, EventTypeRepository, tenantEventTypes);
            AddEventTypes.AddEventType(new EventTypeDetails() { Code = "APIV", EnglishName = "Voided", LocalName = "Voided", Tenant = 0, ObjectTableId = apInvoiceObject.Id, ShortView = true, }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "UPPI",
                EnglishName = "Invoice Updated",
                Tenant = 0,
                LocalName = "Invoice Updated",
                ObjectTableId = apInvoiceObject.Id,
                ShortView = false,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "CRPI",
                EnglishName = "Created",
                Tenant = 0,
                LocalName = "Created",
                ObjectTableId = apInvoiceObject.Id,
                ShortView = true,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "COIN",
                EnglishName = "Payment Connected",
                Tenant = 0,
                LocalName = "Payment Connected",
                ObjectTableId = apInvoiceObject.Id,
                ShortView = true,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "APID",
                EnglishName = "Payment Disconnected",
                Tenant = 0,
                LocalName = "Payment Disconnected",
                ObjectTableId = apInvoiceObject.Id,
                ShortView = true,
            }, EventTypeRepository, tenantEventTypes);

            #endregion

            #region ARPayment

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "PATS",
                EnglishName = "Transferred to SAT",
                Tenant = 0,
                LocalName = "Transferred to SAT",
                ObjectTableId = arInvoiceObject.Id,
                ShortView = true,
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "PAAS",
                EnglishName = "Approved by SAT",
                Tenant = 0,
                LocalName = "Approved by SAT",
                ObjectTableId = arInvoiceObject.Id,
                ShortView = true,
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails() { Code = "ARPA", EnglishName = "Approved", LocalName = "Approved", Tenant = 0, ObjectTableId = arPaymentObject.Id, ShortView = true, }, EventTypeRepository, tenantEventTypes);
            AddEventTypes.AddEventType(new EventTypeDetails() { Code = "ARPC", EnglishName = "Canceled", LocalName = "Canceled", Tenant = 0, ObjectTableId = arPaymentObject.Id, ShortView = true, }, EventTypeRepository, tenantEventTypes);
            AddEventTypes.AddEventType(new EventTypeDetails() { Code = "ARPV", EnglishName = "Voided", LocalName = "Voided", Tenant = 0, ObjectTableId = arPaymentObject.Id, ShortView = true, }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "UPPY",
                EnglishName = "Payment Updated",
                Tenant = 0,
                LocalName = "Payment Updated",
                ObjectTableId = arPaymentObject.Id,
                ShortView = false,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "CRPY",
                EnglishName = "Created",
                Tenant = 0,
                LocalName = "Created",
                ObjectTableId = arPaymentObject.Id,
                ShortView = true,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "CNAR",
                EnglishName = "Connected",
                Tenant = 0,
                LocalName = "Connected",
                ObjectTableId = arPaymentObject.Id,
                ShortView = true,
            }, EventTypeRepository, tenantEventTypes);

			AddEventTypes.AddEventType(new EventTypeDetails()
			{
				Code = "R2CB",
				EnglishName = "Returned to Cashbook",
				Tenant = 0,
				LocalName = "המחאה הוצאה מהפקדה",
				ObjectTableId = arPaymentObject.Id,
				ShortView = true,
			}, EventTypeRepository, tenantEventTypes);


			AddEventTypes.AddEventType(new EventTypeDetails()
			{
				Code = "R2CS",
				EnglishName = "Returned to Customer",
				Tenant = 0,
				LocalName = "המחאה הוצאה מהפקדה והוחזרה ללקוח",
				ObjectTableId = arPaymentObject.Id,
				ShortView = true,
			}, EventTypeRepository, tenantEventTypes);
			#endregion

			//#region BankAccountLite
			//AddEventTypes.AddEventType(new EventTypeDetails()
   //         {
   //             Code = "UPBA",
   //             EnglishName = "Updated",
   //             Tenant = 0,
   //             LocalName = "Updated",
   //             ObjectTableId = BankAccountLiteObject.Id,
   //             ShortView = false,
   //         }, EventTypeRepository, tenantEventTypes);

   //         AddEventTypes.AddEventType(new EventTypeDetails()
   //         {
   //             Code = "CRBA",
   //             EnglishName = "Created",
   //             Tenant = 0,
   //             LocalName = "Created",
   //             ObjectTableId = BankAccountLiteObject.Id,
   //             ShortView = true,
   //         }, EventTypeRepository, tenantEventTypes);
   //         #endregion 

            #region APPayment
            AddEventTypes.AddEventType(new EventTypeDetails() { Code = "APPA", EnglishName = "Approved", LocalName = "Approved", Tenant = 0, ObjectTableId = apPaymentObject.Id, ShortView = true, }, EventTypeRepository, tenantEventTypes);
            AddEventTypes.AddEventType(new EventTypeDetails() { Code = "APPC", EnglishName = "Canceled", LocalName = "Canceled", Tenant = 0, ObjectTableId = apPaymentObject.Id, ShortView = true, }, EventTypeRepository, tenantEventTypes);
            AddEventTypes.AddEventType(new EventTypeDetails() { Code = "APPV", EnglishName = "Voided", LocalName = "Voided", Tenant = 0, ObjectTableId = apPaymentObject.Id, ShortView = true, }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "UPAP",
                EnglishName = "Payment Updated",
                Tenant = 0,
                AddedManually = false,
                LocalName = "Payment Updated",
                ObjectTableId = apPaymentObject.Id,
                ShortView = false,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "CRAP",
                EnglishName = "Created",
                Tenant = 0,
                AddedManually = false,
                IsManualEntry = false,
                LocalName = "Created",
                ObjectTableId = apPaymentObject.Id,
                ShortView = true,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "CNPY",
                EnglishName = "Connected",
                Tenant = 0,
                AddedManually = false,
                IsManualEntry = false,
                LocalName = "Connected",
                ObjectTableId = apPaymentObject.Id,
                ShortView = true,
            }, EventTypeRepository, tenantEventTypes);
            #endregion

            #region CommunicationLog
            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "CRLG",
                EnglishName = "Created",
                Tenant = 0,
                AddedManually = false,
                IsManualEntry = false,
                LocalName = "Created",
                ObjectTableId = commLogObject.Id,
                ShortView = true,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "UPLG",
                EnglishName = "Log Updated",
                Tenant = 0,
                AddedManually = false,
                LocalName = "Log Updated",
                ObjectTableId = commLogObject.Id,
                ShortView = false,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "RSLG",
                EnglishName = "Resend",
                Tenant = 0,
                AddedManually = false,
                IsManualEntry = false,
                LocalName = "Resend",
                ObjectTableId = commLogObject.Id,
                ShortView = true,
            }, EventTypeRepository, tenantEventTypes);
            #endregion

            #region TenantMngmnt
            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "UPMG",
                EnglishName = "Tenant Management Updated",
                Tenant = 0,
                AddedManually = false,
                LocalName = "Tenant Management Updated",
                ObjectTableId = tenantMngmntObject.Id,
                ShortView = false,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "CRMG",
                EnglishName = "Tenant Management Created",
                Tenant = 0,
                AddedManually = false,
                LocalName = "Tenant Management Created",
                ObjectTableId = tenantMngmntObject.Id,
                ShortView = false,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "PFMG",
                EnglishName = "Payment Failure Updated",
                Tenant = 0,
                AddedManually = false,
                LocalName = "Payment Failure Updated",
                ObjectTableId = tenantMngmntObject.Id,
                ShortView = false,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "PCMG",
                EnglishName = "Package Changed",
                Tenant = 0,
                AddedManually = false,
                LocalName = "Package Changed",
                ObjectTableId = tenantMngmntObject.Id,
                ShortView = false,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "INMG",
                EnglishName = "Inactivated",
                Tenant = 0,
                AddedManually = false,
                LocalName = "Inactivated",
                ObjectTableId = tenantMngmntObject.Id,
                ShortView = false,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "TSMG",
                EnglishName = "Trial Start Date Changed",
                Tenant = 0,
                AddedManually = false,
                LocalName = "Trial Start Date Changed",
                ObjectTableId = tenantMngmntObject.Id,
                ShortView = false,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "TEMG",
                EnglishName = "Trial End Date Changed",
                Tenant = 0,
                AddedManually = false,
                LocalName = "Trial End Date Changed",
                ObjectTableId = tenantMngmntObject.Id,
                ShortView = false,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "USMG",
                EnglishName = "Users Updated",
                Tenant = 0,
                AddedManually = false,
                LocalName = "Users Updated",
                ObjectTableId = tenantMngmntObject.Id,
                ShortView = false,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "ACMG",
                EnglishName = "Activated",
                Tenant = 0,
                AddedManually = false,
                LocalName = "Activated",
                ObjectTableId = tenantMngmntObject.Id,
                ShortView = false,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "TMTP",
                EnglishName = "TTY/PIMA Changed",
                Tenant = 0,
                AddedManually = false,
                LocalName = "TTY/PIMA Changed",
                ObjectTableId = tenantMngmntObject.Id,
                ShortView = false,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "ARGR",
                EnglishName = "Airline Registration Requested",
                Tenant = 0,
                AddedManually = false,
                LocalName = "Airline Registration Requested",
                ObjectTableId = tenantMngmntObject.Id,
                ShortView = false,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "ARRG",
                EnglishName = "Airline Registered",
                Tenant = 0,
                AddedManually = false,
                LocalName = "Airline Registered",
                ObjectTableId = tenantMngmntObject.Id,
                ShortView = false,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "ARDE",
                EnglishName = "Airline Registration Declined",
                Tenant = 0,
                AddedManually = false,
                LocalName = "Airline Registration Declined",
                ObjectTableId = tenantMngmntObject.Id,
                ShortView = false,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "ASDI",
                EnglishName = "Airline set as Direct",
                Tenant = 0,
                AddedManually = false,
                LocalName = "Airline set as Direct",
                ObjectTableId = tenantMngmntObject.Id,
                ShortView = false,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "BREN",
                EnglishName = "Branding enabled",
                Tenant = 0,
                AddedManually = false,
                LocalName = "Branding enabled",
                ObjectTableId = tenantMngmntObject.Id,
                ShortView = false,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "BRDI",
                EnglishName = "Branding disabled",
                Tenant = 0,
                AddedManually = false,
                LocalName = "Branding disabled",
                ObjectTableId = tenantMngmntObject.Id,
                ShortView = false,
            }, EventTypeRepository, tenantEventTypes);
 


            #endregion

            #region AnalyzeQueue
            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "UPAQ",
                EnglishName = "Analyze Queue Updated",
                Tenant = 0,
                AddedManually = false,
                LocalName = "Analyze Queue Updated",
                ObjectTableId = analyzeQueueObject.Id,
                ShortView = false,
            }, EventTypeRepository, tenantEventTypes);
            #endregion

            #region Country City
            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "UPCC",
                EnglishName = "City Updated",
                Tenant = 0,
                LocalName = "City Updated",
                ObjectTableId = CountryCityObject.Id,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "CRCC",
                EnglishName = "Created",
                Tenant = 0,
                LocalName = "Created",
                ObjectTableId = CountryCityObject.Id,
                ShortView = true,
            }, EventTypeRepository, tenantEventTypes);
            #endregion

            #region Measurement
            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "UPMM",
                EnglishName = "Measurement Updated",
                Tenant = 0,
                LocalName = "Measurement Updated",
                ObjectTableId = MeasurementObject.Id,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "CRMM",
                EnglishName = "Created",
                Tenant = 0,
                LocalName = "Created",
                ObjectTableId = MeasurementObject.Id,
                ShortView = true,
            }, EventTypeRepository, tenantEventTypes);
            #endregion

            #region BluesnapContract
            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "UPBC",
                EnglishName = "Bluesnap Contract Updated",
                Tenant = 0,
                AddedManually = false,
                LocalName = "Bluesnap Contract Updated",
                ObjectTableId = BluesnapContractObject.Id,
                ShortView = false,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "CRBC",
                EnglishName = "Created",
                Tenant = 0,
                AddedManually = false,
                IsManualEntry = false,
                LocalName = "Created",
                ObjectTableId = BluesnapContractObject.Id,
                ShortView = true,
            }, EventTypeRepository, tenantEventTypes);
            #endregion

            #region CreditCardType
            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "UPCD",
                EnglishName = "Credit Card Type Updated",
                Tenant = 0,
                AddedManually = false,
                LocalName = "Credit Card Type Updated",
                ObjectTableId = CreditCardTypeObject.Id,
                ShortView = false,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "CRCD",
                EnglishName = "Created",
                Tenant = 0,
                AddedManually = false,
                IsManualEntry = false,
                LocalName = "Created",
                ObjectTableId = CreditCardTypeObject.Id,
                ShortView = true,
            }, EventTypeRepository, tenantEventTypes);
            #endregion

            #region MoveType
            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "UPMT",
                EnglishName = "Move Type Updated",
                Tenant = 0,
                AddedManually = false,
                LocalName = "Move Type Updated",
                ObjectTableId = MoveTypeObject.Id,
                ShortView = false,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "CRMT",
                EnglishName = "Created",
                Tenant = 0,
                AddedManually = false,
                IsManualEntry = false,
                LocalName = "Created",
                ObjectTableId = MoveTypeObject.Id,
                ShortView = true,
            }, EventTypeRepository, tenantEventTypes);
            #endregion

            #region Report
            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "UPRP",
                EnglishName = "Report Updated",
                Tenant = 0,
                AddedManually = false,
                LocalName = "Report Updated",
                ObjectTableId = ReportObject.Id,
                ShortView = false,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "CRRP",
                EnglishName = "Created",
                Tenant = 0,
                AddedManually = false,
                IsManualEntry = false,
                LocalName = "Created",
                ObjectTableId = ReportObject.Id,
                ShortView = true,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "TEPU",
                EnglishName = "New Template Uploaded",
                Tenant = 0,
                AddedManually = false,
                IsManualEntry = false,
                LocalName = "New Template Uploaded",
                ObjectTableId = ReportObject.Id,
                ShortView = true,
            }, EventTypeRepository, tenantEventTypes);
            #endregion

            #region Region
            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "UPRG",
                EnglishName = "Region Updated",
                Tenant = 0,
                AddedManually = false,
                LocalName = "Region Updated",
                ObjectTableId = RegionObject.Id,
                ShortView = false,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "CRRG",
                EnglishName = "Created",
                Tenant = 0,
                AddedManually = false,
                IsManualEntry = false,
                LocalName = "Created",
                ObjectTableId = RegionObject.Id,
                ShortView = true,
            }, EventTypeRepository, tenantEventTypes);
            #endregion

            #region Commodity
            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "UPCM",
                EnglishName = "Commodity Updated",
                Tenant = 0,
                AddedManually = false,
                LocalName = "Commodity Updated",
                ObjectTableId = CommodityObject.Id,
                ShortView = false,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "CRCM",
                EnglishName = "Created",
                Tenant = 0,
                AddedManually = false,
                IsManualEntry = false,
                LocalName = "Created",
                ObjectTableId = CommodityObject.Id,
                ShortView = true,
            }, EventTypeRepository, tenantEventTypes);
            #endregion

            #region SpecialServicesType
            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "UPSS",
                EnglishName = "Special Services Type Updated",
                Tenant = 0,
                AddedManually = false,
                LocalName = "Special Services Type Updated",
                ObjectTableId = SpecialServicesTypeObject.Id,
                ShortView = false,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "CRSS",
                EnglishName = "Created",
                Tenant = 0,
                AddedManually = false,
                IsManualEntry = false,
                LocalName = "Created",
                ObjectTableId = SpecialServicesTypeObject.Id,
                ShortView = true,
            }, EventTypeRepository, tenantEventTypes);
            #endregion

            #region BusinessUnit
            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "UPBU",
                EnglishName = "Business Unit Updated",
                Tenant = 0,
                AddedManually = false,
                LocalName = "Business Unit Updated",
                ObjectTableId = BusinessUnitObject.Id,
                ShortView = false,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "CRBU",
                EnglishName = "Created",
                Tenant = 0,
                AddedManually = false,
                IsManualEntry = false,
                LocalName = "Created",
                ObjectTableId = BusinessUnitObject.Id,
                ShortView = true,
            }, EventTypeRepository, tenantEventTypes);
            #endregion

            #region CustomerSize
            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "UPCZ",
                EnglishName = "Customer Size Updated",
                Tenant = 0,
                AddedManually = false,
                LocalName = "Customer Size Updated",
                ObjectTableId = CustomerSizeObject.Id,
                ShortView = false,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "CRCZ",
                EnglishName = "Created",
                Tenant = 0,
                AddedManually = false,
                IsManualEntry = false,
                LocalName = "Created",
                ObjectTableId = CustomerSizeObject.Id,
                ShortView = true,
            }, EventTypeRepository, tenantEventTypes);
            #endregion

            #region LeadSource
            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "UPLS",
                EnglishName = "Lead Source Updated",
                Tenant = 0,
                AddedManually = false,
                LocalName = "Lead Source Updated",
                ObjectTableId = LeadSourceObject.Id,
                ShortView = false,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "CRLS",
                EnglishName = "Created",
                Tenant = 0,
                AddedManually = false,
                IsManualEntry = false,
                LocalName = "Created",
                ObjectTableId = LeadSourceObject.Id,
                ShortView = true,
            }, EventTypeRepository, tenantEventTypes);
            #endregion

            #region AdditionalService
            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "UPAS",
                EnglishName = "Additional Service Updated",
                Tenant = 0,
                AddedManually = false,
                LocalName = "Additional Service Updated",
                ObjectTableId = AdditionalServiceObject.Id,
                ShortView = false,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "CRAS",
                EnglishName = "Created",
                Tenant = 0,
                AddedManually = false,
                IsManualEntry = false,
                LocalName = "Created",
                ObjectTableId = AdditionalServiceObject.Id,
                ShortView = true,
            }, EventTypeRepository, tenantEventTypes);
            #endregion

            #region Industry
            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "UPID",
                EnglishName = "Industry Updated",
                Tenant = 0,
                AddedManually = false,
                LocalName = "Industry Updated",
                ObjectTableId = IndustryObject.Id,
                ShortView = false,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "CRID",
                EnglishName = "Created",
                Tenant = 0,
                AddedManually = false,
                IsManualEntry = false,
                LocalName = "Created",
                ObjectTableId = IndustryObject.Id,
                ShortView = true,
            }, EventTypeRepository, tenantEventTypes);
            #endregion

            #region Competitor
            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "UPCP",
                EnglishName = "Competitor Updated",
                Tenant = 0,
                AddedManually = false,
                LocalName = "Competitor Updated",
                ObjectTableId = CompetitorObject.Id,
                ShortView = false,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "CRCP",
                EnglishName = "Created",
                Tenant = 0,
                AddedManually = false,
                IsManualEntry = false,
                LocalName = "Created",
                ObjectTableId = CompetitorObject.Id,
                ShortView = true,
            }, EventTypeRepository, tenantEventTypes);
            #endregion

            #region ProductType
            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "UPPR",
                EnglishName = "Product Type Updated",
                Tenant = 0,
                AddedManually = false,
                LocalName = "Product Type Updated",
                ObjectTableId = ProductTypeObject.Id,
                ShortView = false,
            }, EventTypeRepository, tenantEventTypes);
            #endregion

            #region QuoteStage
            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "UPQS",
                EnglishName = "Quote Stage Updated",
                Tenant = 0,
                AddedManually = false,
                LocalName = "Quote Stage Updated",
                ObjectTableId = QuoteStageObject.Id,
                ShortView = false,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "CRQS",
                EnglishName = "Created",
                Tenant = 0,
                AddedManually = false,
                IsManualEntry = false,
                LocalName = "Created",
                ObjectTableId = QuoteStageObject.Id,
                ShortView = true,
            }, EventTypeRepository, tenantEventTypes);
            #endregion

            #region MessagingStock
            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "UPMS",
                EnglishName = "Updated",
                Tenant = 0,
                AddedManually = false,
                LocalName = "Updated",
                ObjectTableId = MessagingStockTable.Id,
                ShortView = false,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "CRMS",
                EnglishName = "Created",
                Tenant = 0,
                AddedManually = false,
                IsManualEntry = false,
                LocalName = "Created",
                ObjectTableId = MessagingStockTable.Id,
                ShortView = true,
            }, EventTypeRepository, tenantEventTypes);
            #endregion

            #region Participant
            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "UPPC",
                EnglishName = "Participant Updated",
                Tenant = 0,
                AddedManually = false,
                LocalName = "Participant Updated",
                ObjectTableId = participantObject.Id,
                ShortView = false,
            }, EventTypeRepository, tenantEventTypes);

            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "CRPC",
                EnglishName = "Created",
                Tenant = 0,
                AddedManually = false,
                IsManualEntry = false,
                LocalName = "Created",
                ObjectTableId = participantObject.Id,
                ShortView = true,
            }, EventTypeRepository, tenantEventTypes);
            #endregion
            
            #region AccountingTransferHeader            
            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "CAAH",
                EnglishName = "Created",
                Tenant = 0,
                AddedManually = false,
                IsManualEntry = false,
                LocalName = "Created",
                ObjectTableId = AccountingTransferHeaderObject.Id,
                ShortView = true,
            }, EventTypeRepository, tenantEventTypes);
            #endregion

            EventTypeRepository.SubmitChanges();
        }
    }
}