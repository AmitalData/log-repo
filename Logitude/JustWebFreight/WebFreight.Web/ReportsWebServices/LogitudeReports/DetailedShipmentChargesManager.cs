using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.InvoiceModel;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using WebFreight.Web.DataProviders;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.ShipmentsModel.Repositories;
using Logitude.BL.Helpers;
using Simplog.Data.CommonDataModel.Repositories;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.EntityPMs;
using WebFreight.Web.WebServices;
using Logitude.BL.DataContracts;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.Repositories;
using Logitude.BL.InfrastructureModel.EntityQueries;
using WebFreight.Web.Services;

namespace WebFreight.Web.ReportsWebServices.LogitudeReports
{
    public class DetailedShipmentChargesManager
    {
        private int tenant;
        private DateTime? FromDate = null;
        private DateTime? ToDate = null;
        private bool IsLocalCurrency = false;
        private string SelectedCurrencyId = null;
        private string SelectedCurrencyCode = null;
        private bool IncludeDraftInvoices = false;
        private bool IncludeEstimations = false;
        private bool IncludeCancelledShipments = false;
        private string SelectedDateType = null;
        private bool housesAndDirectOnly = false;
        private IInvoiceContext myInvoiceContext;
        private ICommonDataContext myCommonContext;
        private IShipmentsContext myShipmentsContext;
        private CustomFieldResolver customFieldResolver;
        private AddressRepository addressRepository;
        private ShipmentPackageRepository shipmentPackageRepository;
        private WebServiceHelper servicHelper;
        private string tenantLocalCurrencyId;
        private IWebFreightContext webFreightContext;
        private RatesTableRepository ratesTablesRepository;
        private RatesTableQuery ratesTableQuery;
        private List<APInvoice> allAPInvoices;
        private List<ARInvoice> allARInvoices;
        private List<ShipmentInvoice> allShipmentsAPInvoices;
        private List<ShipmentInvoice> allShipmentsARInvoices;        
        private ArchivoExportadoDataProvider myDataProvider;

        public DetailedShipmentChargesManager(byte[] xmlFilters, int tenant)
        {
            this.tenant = tenant;
            servicHelper = new WebServiceHelper(this.tenant);
            myInvoiceContext = InvoiceContext.GetContext(tenant);
            myCommonContext = CommonDataContext.GetContext(tenant);
            myShipmentsContext = ShipmentsContext.GetContext(tenant);
            customFieldResolver = new CustomFieldResolver(tenant);
            addressRepository = new AddressRepository(myCommonContext);
            shipmentPackageRepository = new ShipmentPackageRepository(myShipmentsContext);
            webFreightContext = WebFreightContext.GetContext(tenant);
            ratesTablesRepository = new RatesTableRepository(webFreightContext);
            ratesTableQuery = new RatesTableQuery(ratesTablesRepository);            

            Tenant myTenant = (from d in myCommonContext.Tenants where d.Id == tenant select d).FirstOrDefault();
            tenantLocalCurrencyId = myTenant?.CurrencyId;

            MemoryStream memoryStream = new MemoryStream(xmlFilters);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations myQueryOperations = (QueryOperations)xmlSerializer.Deserialize(memoryStream);

            QueryFilterItem filterItem_SelectedDateType = myQueryOperations.QueryFilterItems.Where(d => d.FieldName == "SelectedDateType").FirstOrDefault();
            QueryFilterItem filterItem_FromDate = myQueryOperations.QueryFilterItems.Where(d => d.FieldName == "FromDate").FirstOrDefault();
            QueryFilterItem filterItem_ToDate = myQueryOperations.QueryFilterItems.Where(d => d.FieldName == "ToDate").FirstOrDefault();
            QueryFilterItem filterItem_IsLocalCurrency = myQueryOperations.QueryFilterItems.Where(d => d.FieldName == "IsLocalCurrency").FirstOrDefault();
            QueryFilterItem filterItem_SelectedCurrencyCode = myQueryOperations.QueryFilterItems.Where(d => d.FieldName == "SelectedCurrencyCode").FirstOrDefault();
            QueryFilterItem filterItem_IncludeDraftInvoices = myQueryOperations.QueryFilterItems.Where(d => d.FieldName == "IncludeDraftInvoices").FirstOrDefault();
            QueryFilterItem filterItem_IncludeEstimations = myQueryOperations.QueryFilterItems.Where(d => d.FieldName == "IncludeEstimations").FirstOrDefault();
            QueryFilterItem filterItem_IncludeCancelledShipments = myQueryOperations.QueryFilterItems.Where(d => d.FieldName == "IncludeCancelledShipments").FirstOrDefault();
            QueryFilterItem filterItem_HousesAndDirectOnly = myQueryOperations.QueryFilterItems.Where(d => d.FieldName == "HousesAndDirectOnly").FirstOrDefault();

            if (filterItem_FromDate != null)
            {
                if (filterItem_FromDate.FieldValue != null)
                {
                    DateTime date;
                    bool isValid = DateTime.TryParse(filterItem_FromDate.FieldValue.ToString(), out date);
                    if (isValid)
                    {
                        this.FromDate = date;
                    }
                }
            }

            if (filterItem_ToDate != null)
            {
                if (filterItem_ToDate.FieldValue != null)
                {
                    DateTime date;
                    bool isValid = DateTime.TryParse(filterItem_ToDate.FieldValue.ToString(), out date);
                    if (isValid)
                    {
                        this.ToDate = date;
                    }
                }
            }

            if (filterItem_IsLocalCurrency != null)
            {
                if (filterItem_IsLocalCurrency.FieldValue != null)
                {
                    IsLocalCurrency = (bool)filterItem_IsLocalCurrency.FieldValue;
                }
            }

            if (filterItem_SelectedCurrencyCode != null)
            {
                if (filterItem_SelectedCurrencyCode.FieldValue != null)
                {
                    SelectedCurrencyCode = filterItem_SelectedCurrencyCode.FieldValue.ToString();
                }
            }

            if (filterItem_IncludeDraftInvoices != null)
            {
                if (filterItem_IncludeDraftInvoices.FieldValue != null)
                {
                    IncludeDraftInvoices = (bool)filterItem_IncludeDraftInvoices.FieldValue;
                }
            }

            if (filterItem_IncludeEstimations != null)
            {
                if (filterItem_IncludeEstimations.FieldValue != null)
                {
                    IncludeEstimations = (bool)filterItem_IncludeEstimations.FieldValue;
                }
            }

            if (filterItem_IncludeCancelledShipments != null)
            {
                if (filterItem_IncludeCancelledShipments.FieldValue != null)
                {
                    IncludeCancelledShipments = (bool)filterItem_IncludeCancelledShipments.FieldValue;
                }
            }

            if (filterItem_SelectedDateType != null)
            {
                if (filterItem_SelectedDateType.FieldValue != null)
                {
                    SelectedDateType = filterItem_SelectedDateType.FieldValue.ToString();
                }
            }

            if (filterItem_HousesAndDirectOnly != null)
            {
                if (filterItem_HousesAndDirectOnly.FieldValue != null)
                {
                    housesAndDirectOnly = (bool)filterItem_HousesAndDirectOnly.FieldValue;
                }
            }
        }

        public byte[] GetData()
        {
            this.InitializeDataProvider();
            this.LoadDataProvider();
            return new ReportMemoryStreamService().Convert(myDataProvider, typeof(ArchivoExportadoDataProvider), tenant);
        }
        private void InitializeDataProvider()
        {
            myDataProvider = new ArchivoExportadoDataProvider();
            myDataProvider.Id = tenant;
            myDataProvider.CurrencyCode = this.SelectedCurrencyCode;
            myDataProvider.IncludeDraftInvoices = this.IncludeDraftInvoices ? "Yes" : "No";
            myDataProvider.IncludeEstimations = this.IncludeEstimations ? "Yes" : "No";
            myDataProvider.From = this.GetDateString(this.FromDate);
            myDataProvider.To = this.GetDateString(this.ToDate);
            myDataProvider.Shipments = new List<ArchivoExportadoShipmentItem>();
        }
        private void LoadDataProvider()
        {
            IQueryable<ShipmentDataView> iQueryable_Shipments = this.GetIQueryableShipments();            
            List<ShipmentDataView> allShipments = iQueryable_Shipments.ToList();

            if (allShipments.Count > 0)
            {
                #region Data
                List<string> allShipmentsIds = allShipments.Select(s => s.Id).ToList();

                if (housesAndDirectOnly)
                {
                    IQueryable<ShipmentDataView> houses = iQueryable_Shipments.Where(d => d.ShipmentLevelCode == "H" && !string.IsNullOrEmpty(d.MasterShipmentDataId));
                    List<string> housesMastersIds = houses.Select(s => s.MasterShipmentDataId).Distinct().ToList();
                    allShipmentsIds = allShipmentsIds.Concat(housesMastersIds).ToList();
                }

                this.BuildInvoicesLists(allShipmentsIds);
                this.BuildInvoicesShipmentsLists(allShipmentsIds);
                List<ShipmentPickUpDelivery> shipmentPickUpDeliveriesLists = (from d in myShipmentsContext.ShipmentPickUpDeliveries where allShipmentsIds.Contains(d.ShipmentId) select d).Include("ToAddressCountry").ToList();

                List<ChargeTypeGroupClass> allPayablesData = new List<ChargeTypeGroupClass>();
                List<ChargeTypeGroupClass> allReceivablesData = new List<ChargeTypeGroupClass>();

                if (this.IncludeEstimations)
                {
                    allPayablesData = this.GetPayablesData(allShipmentsIds);
                    allReceivablesData = this.GetReceivablesData(allShipmentsIds); 
                }

                List<Card> allCards = new List<Card>();
                List<Contact> allContacts = new List<Contact>();
                List<Branch> allBranchs = new List<Branch>();
                List<Currency> allCurrencies = new List<Currency>();
                List<Incoterm> allIncoterms = new List<Incoterm>();
                List<ChargesType> allChargesTypes = (from d in myCommonContext.ChargesTypes where d.Tenant == tenant select d).ToList();

                List<ChargeTypeGroupClass> allAPInvoiceLinesData = new List<ChargeTypeGroupClass>();
                List<ChargeTypeGroupClass> allARInvoiceLinesData = new List<ChargeTypeGroupClass>();
                if (allAPInvoices.Count > 0 || allARInvoices.Count > 0 || allShipments.Count > 0)
                {
                    allCards = this.GetCards(allPayablesData);
                    allContacts = this.GetContacts();
                    allBranchs = (from d in myCommonContext.Branches where d.Tenant == tenant select d).ToList();
                    allCurrencies = (from d in myCommonContext.Currencies where d.Tenant == tenant select d).ToList();
                    allIncoterms = (from d in myCommonContext.Incoterms where d.Tenant == tenant select d).ToList();

                    if (allAPInvoices.Count > 0)
                    {
                        List<string> allAPInvoicesIds = allAPInvoices.Select(s => s.Id).ToList();
                        allAPInvoiceLinesData = this.GetAPInvoiceLinesData(allAPInvoicesIds);
                    }

                    if (allARInvoices.Count > 0)
                    {
                        List<string> allARInvoicesIds = allARInvoices.Select(s => s.Id).ToList();
                        allARInvoiceLinesData = this.GetARInvoiceLinesData(allARInvoicesIds);                        
                    }
                }
                #endregion

                #region Currency
                Currency SelectedCurrency = allCurrencies.Where(d => d.Code == this.SelectedCurrencyCode).FirstOrDefault();
                if (SelectedCurrency != null)
                {
                    this.SelectedCurrencyId = SelectedCurrency.Id;
                }
                #endregion

                foreach (ShipmentDataView myShipment in allShipments)
                {
                    Branch myBranch = null;
                    Incoterm myIncoterm = null;

                    if (!string.IsNullOrEmpty(myShipment.BranchId))
                    {
                        myBranch = allBranchs.Where(d => d.Id == myShipment.BranchId).FirstOrDefault();
                    }

                    if (!string.IsNullOrEmpty(myShipment.IncotermId))
                    {
                        myIncoterm = allIncoterms.Where(d => d.Id == myShipment.IncotermId).FirstOrDefault();
                    }

                    #region
                    string longMaster = this.GetLongMaster(myShipment);
                    string myDirectionPartner = myShipment.DirectionId == "I" ? myShipment.ConsigneeName : myShipment.ShipperName;

                    string customerExternalID = null;
                    if (!string.IsNullOrEmpty(myShipment.CustomerId))
                    {
                        Card customer = myCommonContext.Cards.Where(d => d.Id == myShipment.CustomerId).FirstOrDefault();
                        if (customer != null)
                        {
                            customerExternalID = customer.ReceivablesAccountingCard;
                        }
                    }

                    if (this.IncludeEstimations)
                    {
                        if (this.HasAmount(myShipment.OpenPayablesInLocalCurrency))
                        {
                            #region Payables
                            List<ChargeTypeGroupClass> lines_Grouped = allPayablesData.Where(d => d.ShipmentId == myShipment.Id).ToList();
                            foreach (ChargeTypeGroupClass item in lines_Grouped)
                            {
                                if (this.HasAmount(item.AmountInLocal))
                                {
                                    ArchivoExportadoShipmentItem myRecord = new ArchivoExportadoShipmentItem();
                                    myRecord.ShipmentNumber = myShipment.ShipmentNumber;
                                    myRecord.OriginCode = myShipment.MainCarriageFromPortCode;
                                    myRecord.DestinationCode = myShipment.MainCarriageFinalDestinationPortCode;
                                    myRecord.LineTypeCode = "EFC";
                                    myRecord.LongMaster = longMaster;
                                    myRecord.DirectionPartner = myDirectionPartner;
                                    myRecord.DescriptionOfGoods = myShipment.DescriptionOfGoods;
                                    myRecord.Payables = this.IsLocalCurrency ? item.AmountInLocal : item.AmountInProfit;
                                    myRecord.Salesman = myShipment.SalesmanUserName;
                                    myRecord.OpenPayables = myRecord.Payables;
                                    myRecord.CustomerExternalID = customerExternalID;
                                    myRecord.Shipper = myShipment.ShipperName;
                                    myRecord.ShipmentStatus = myShipment.ShipmentStatusName;
                                    myRecord.AccountingClosed = myShipment.IsAccountingClosed;
                                    myRecord.ShipperNotExporter = myShipment.ShipperNotExporterName;
                                    myRecord.Consignee = myShipment.ConsigneeName;
                                    myRecord.ConsigneeNotImporter = myShipment.ConsigneeNotImporterName;
                                    myRecord.Direction = myShipment.DirectionName;
                                    myRecord.OperationalDate = myShipment.OperationalDate;
                                    myRecord.Origin = myShipment.Origin;
                                    myRecord.Destination = myShipment.LastFinalDestination;
                                    myRecord.ChargeableWeight = myShipment.ChargeableWeight;
                                    myRecord.Profit = this.IsLocalCurrency ? myShipment.ProfitInLocalCurrency : myShipment.ProfitInProfitCurrency;
                                    myRecord.ExpectedPayables = this.IsLocalCurrency ? item.ExpectedAmountInLocal : item.ExpectedAmountInProfit;
                                    myRecord.ETD = myShipment.MainCarriageETD;
                                    myRecord.CustomerRef1 = myShipment.CustomerReference1;
                                    myRecord.CustomerRef2 = myShipment.CustomerReference2;
                                    myRecord.CountryOfOrigin = myShipment.MainCarriageFromPortCountryName;
                                    myRecord.House = myShipment.House;
                                    myRecord.ContainersNumbers = shipmentPackageRepository.GetContainersNumbersByShipmentIdAndTenant(myShipment.Id, myShipment.Tenant);
                                    myRecord.ShipmentCreateDate = myShipment.CreateDateTime;
                                    myRecord.ShipmentNotes = myShipment.Notes;
                                    myRecord.ShipmentOpenedBy = myShipment.CreatedByUserName;

                                    ShipmentPickUpDelivery myLastDelivery = shipmentPickUpDeliveriesLists.Where(d => d.ShipmentId == myShipment.Id && d.PickUpDeliveryTypeCode == "DELV").OrderByDescending(s => s.PickUpDeliveryNumber).FirstOrDefault();
                                    myRecord.CountryOfDestination = this.ComputeCountryOfDistination(myShipment, myLastDelivery);

                                    customFieldResolver.SetDataProviderCustomFieldsValues("Shipment", tenant, myShipment, myRecord);

                                    if (myBranch != null)
                                    {
                                        myRecord.BranchCode = myBranch.Code;
                                        myRecord.BranchName = myBranch.EnglishName;
                                        myRecord.BranchLocalName = myBranch.LocalName;
                                        myRecord.BranchExternalId = myBranch.ExternalId;
                                    }

                                    if (myIncoterm != null)
                                    {
                                        myRecord.IncotermCode = myIncoterm.Code;
                                        myRecord.IncotermName = myIncoterm.Name;
                                    }

                                    if (item.ChargesTypeId != null)
                                    {
                                        ChargesType myChargesType = allChargesTypes.Where(d => d.Id == item.ChargesTypeId).FirstOrDefault();
                                        if (myChargesType != null)
                                        {
                                            myRecord.ChargeTypeCode = myChargesType.Code;
                                            myRecord.ChargeTypeName = myChargesType.EnglishName;
                                            myRecord.ChargeTypeLocalName = myChargesType.LocalName;
                                        }
                                    }

                                    if (item.CardId != null)
                                    {
                                        Card myCard = allCards.Where(d => d.Id == item.CardId).FirstOrDefault();
                                        if (myCard != null)
                                        {
                                            myRecord.CardCode = myCard.Code;
                                            myRecord.CardName = myCard.EnglishName;
                                            myRecord.CardExternal = myCard.PayablesAccountingCard;
                                            myRecord.VendorName = myCard.EnglishName;
                                        }
                                    }

                                    myDataProvider.Shipments.Add(myRecord);
                                }
                            }
                            #endregion
                        }

                        if (this.HasAmount(myShipment.OpenReceivablesInLocalCurrency))
                        {
                            #region Receivables
                            List<ChargeTypeGroupClass> lines_Grouped = allReceivablesData.Where(d => d.ShipmentId == myShipment.Id).ToList();
                            foreach (ChargeTypeGroupClass item in lines_Grouped)
                            {
                                if (this.HasAmount(item.AmountInLocal))
                                {
                                    ArchivoExportadoShipmentItem myRecord = new ArchivoExportadoShipmentItem();
                                    myRecord.ShipmentNumber = myShipment.ShipmentNumber;
                                    myRecord.OriginCode = myShipment.MainCarriageFromPortCode;
                                    myRecord.DestinationCode = myShipment.MainCarriageFinalDestinationPortCode;
                                    myRecord.LineTypeCode = "EFC";
                                    myRecord.LongMaster = longMaster;
                                    myRecord.DirectionPartner = myDirectionPartner;
                                    myRecord.DescriptionOfGoods = myShipment.DescriptionOfGoods;
                                    myRecord.Receivables = this.IsLocalCurrency ? item.AmountInLocal : item.AmountInProfit;
                                    myRecord.Salesman = myShipment.SalesmanUserName;
                                    myRecord.OpenReceivables = myRecord.Receivables;
                                    myRecord.CustomerExternalID = customerExternalID;
                                    myRecord.Shipper = myShipment.ShipperName;
                                    myRecord.ShipmentStatus = myShipment.ShipmentStatusName;
                                    myRecord.AccountingClosed = myShipment.IsAccountingClosed;
                                    myRecord.ShipperNotExporter = myShipment.ShipperNotExporterName;
                                    myRecord.Consignee = myShipment.ConsigneeName;
                                    myRecord.ConsigneeNotImporter = myShipment.ConsigneeNotImporterName;
                                    myRecord.Direction = myShipment.DirectionName;
                                    myRecord.OperationalDate = myShipment.OperationalDate;
                                    myRecord.Origin = myShipment.Origin;
                                    myRecord.Destination = myShipment.LastFinalDestination;
                                    myRecord.ChargeableWeight = myShipment.ChargeableWeight;
                                    myRecord.Profit = this.IsLocalCurrency ? myShipment.ProfitInLocalCurrency : myShipment.ProfitInProfitCurrency;
                                    myRecord.ETD = myShipment.MainCarriageETD;
                                    myRecord.CustomerRef1 = myShipment.CustomerReference1;
                                    myRecord.CustomerRef2 = myShipment.CustomerReference2;
                                    myRecord.CountryOfOrigin = myShipment.MainCarriageFromPortCountryName;
                                    myRecord.House = myShipment.House;
                                    myRecord.ContainersNumbers = shipmentPackageRepository.GetContainersNumbersByShipmentIdAndTenant(myShipment.Id, myShipment.Tenant);
                                    myRecord.ShipmentCreateDate = myShipment.CreateDateTime;
                                    myRecord.ShipmentNotes = myShipment.Notes;
                                    myRecord.ShipmentOpenedBy = myShipment.CreatedByUserName;

                                    ShipmentPickUpDelivery myLastDelivery = shipmentPickUpDeliveriesLists.Where(d => d.ShipmentId == myShipment.Id && d.PickUpDeliveryTypeCode == "DELV").OrderByDescending(s => s.PickUpDeliveryNumber).FirstOrDefault();
                                    myRecord.CountryOfDestination = this.ComputeCountryOfDistination(myShipment, myLastDelivery);

                                    customFieldResolver.SetDataProviderCustomFieldsValues("Shipment", tenant, myShipment, myRecord);

                                    if (myBranch != null)
                                    {
                                        myRecord.BranchCode = myBranch.Code;
                                        myRecord.BranchName = myBranch.EnglishName;
                                        myRecord.BranchLocalName = myBranch.LocalName;
                                        myRecord.BranchExternalId = myBranch.ExternalId;
                                    }

                                    if (myIncoterm != null)
                                    {
                                        myRecord.IncotermCode = myIncoterm.Code;
                                        myRecord.IncotermName = myIncoterm.Name;
                                    }

                                    if (item.ChargesTypeId != null)
                                    {
                                        ChargesType myChargesType = allChargesTypes.Where(d => d.Id == item.ChargesTypeId).FirstOrDefault();
                                        if (myChargesType != null)
                                        {
                                            myRecord.ChargeTypeCode = myChargesType.Code;
                                            myRecord.ChargeTypeName = myChargesType.EnglishName;
                                            myRecord.ChargeTypeLocalName = myChargesType.LocalName;
                                        }
                                    }

                                    myDataProvider.Shipments.Add(myRecord);
                                }
                            }
                            #endregion
                        }
                    }

                    List<string> myAPInvoicesIds = new List<string>();
                    if (housesAndDirectOnly)
                    {
                        myAPInvoicesIds = allShipmentsAPInvoices.Where(d => d.ShipmentId == myShipment.Id || d.ShipmentId == myShipment.MasterShipmentDataId).Select(s => s.InvoiceId).ToList();
                    }

                    else
                    {
                        myAPInvoicesIds = allShipmentsAPInvoices.Where(d => d.ShipmentId == myShipment.Id).Select(s => s.InvoiceId).ToList();
                    }

                    foreach (string id in myAPInvoicesIds)
                    {
                        APInvoice invoice = allAPInvoices.Where(d => d.Id == id).FirstOrDefault();
                        if (invoice != null)
                        {
                            #region AP Invoices
                            Card myCard = allCards.Where(d => d.Id == invoice.VendorId).FirstOrDefault();
                            Contact myContact = allContacts.Where(d => d.Id == invoice.CreatedByUserId).FirstOrDefault();
                            Currency myCurrency = allCurrencies.Where(d => d.Id == invoice.InvoiceCurrencyId).FirstOrDefault();

                            if (invoice.IsMultipleEntities)
                            {
                                myBranch = allBranchs.Where(d => d.Id == myShipment.BranchId).FirstOrDefault();
                                myIncoterm = allIncoterms.Where(d => d.Id == myShipment.IncotermId).FirstOrDefault();
                            }

                            List<ChargeTypeGroupClass> lines_Grouped = new List<ChargeTypeGroupClass>();

                            if (housesAndDirectOnly)
                            {
                                lines_Grouped = allAPInvoiceLinesData.Where(d => d.InvoiceId == invoice.Id && (d.ShipmentId == myShipment.Id || d.ShipmentId == myShipment.MasterShipmentDataId)).ToList();
                            }

                            else
                            {
                                lines_Grouped = allAPInvoiceLinesData.Where(d => d.InvoiceId == invoice.Id && d.ShipmentId == myShipment.Id).ToList();
                            }

                            foreach (ChargeTypeGroupClass item in lines_Grouped)
                            {
                                ArchivoExportadoShipmentItem myRecord = new ArchivoExportadoShipmentItem();
                                myRecord.LineTypeCode = "FC";
                                myRecord.ShipmentNumber = myShipment.ShipmentNumber;
                                myRecord.OriginCode = myShipment.MainCarriageFromPortCode;
                                myRecord.DestinationCode = myShipment.MainCarriageFinalDestinationPortCode;
                                myRecord.LongMaster = longMaster;
                                myRecord.DirectionPartner = myDirectionPartner;
                                myRecord.DescriptionOfGoods = myShipment.DescriptionOfGoods;
                                myRecord.Salesman = myShipment.SalesmanUserName;
                                myRecord.Payables = this.ComputePayables(item, invoice, myShipment);
                                myRecord.InvoiceNumber = invoice.InvoiceNumber;
                                myRecord.InvoiceDate = invoice.InvoiceDate;
                                myRecord.InvoiceCurrencyRate = invoice.InvoiceCurrencyExchangeRate;
                                myRecord.CustomerExternalID = customerExternalID;
                                myRecord.Shipper = myShipment.ShipperName;
                                myRecord.ShipmentStatus = myShipment.ShipmentStatusName;
                                myRecord.AccountingClosed = myShipment.IsAccountingClosed;
                                myRecord.ShipperNotExporter = myShipment.ShipperNotExporterName;
                                myRecord.Consignee = myShipment.ConsigneeName;
                                myRecord.ConsigneeNotImporter = myShipment.ConsigneeNotImporterName;
                                myRecord.Direction = myShipment.DirectionName;
                                myRecord.OperationalDate = myShipment.OperationalDate;
                                myRecord.Origin = myShipment.Origin;
                                myRecord.Destination = myShipment.LastFinalDestination;
                                myRecord.ChargeableWeight = myShipment.ChargeableWeight;
                                myRecord.Profit = this.IsLocalCurrency ? myShipment.ProfitInLocalCurrency : myShipment.ProfitInProfitCurrency;
                                myRecord.ETD = myShipment.MainCarriageETD;
                                myRecord.CustomerRef1 = myShipment.CustomerReference1;
                                myRecord.CustomerRef2 = myShipment.CustomerReference2;
                                myRecord.CountryOfOrigin = myShipment.MainCarriageFromPortCountryName;
                                myRecord.House = myShipment.House;
                                myRecord.ContainersNumbers = shipmentPackageRepository.GetContainersNumbersByShipmentIdAndTenant(myShipment.Id, myShipment.Tenant);
                                myRecord.ShipmentCreateDate = myShipment.CreateDateTime;
                                myRecord.ShipmentNotes = myShipment.Notes;
                                myRecord.ShipmentOpenedBy = myShipment.CreatedByUserName;
                                myRecord.InvoiceAmountDueInLocalCurrency = invoice.AmountDueInLocalCurrency;
                                myRecord.InvoiceAmountDueInInvoiceCurrency = invoice.AmountDue;
                                myRecord.AccountedReceivablesInInvoiceCurrency = this.ComputeAccountedReceivablesInInvoiceCurrency(invoice.InvoiceCurrencyId, myShipment, invoice.InvoiceDate);
                                myRecord.AccountedPayablesInInvoiceCurrency = this.ComputeAccountedPayablesInInvoiceCurrency(invoice.Id, invoice.InvoiceCurrencyId, myShipment, invoice.InvoiceDate, item.PayableId);
                                myRecord.PaidDate = invoice.PaidDate;
                                
                                ShipmentPickUpDelivery myLastDelivery = shipmentPickUpDeliveriesLists.Where(d => d.ShipmentId == myShipment.Id && d.PickUpDeliveryTypeCode == "DELV").OrderByDescending(s => s.PickUpDeliveryNumber).FirstOrDefault();
                                myRecord.CountryOfDestination = this.ComputeCountryOfDistination(myShipment, myLastDelivery);

                                if (this.tenant == 1255 || this.tenant == 2512)
                                {
                                    if (this.IsLocalCurrency)
                                    {
                                        myRecord.ExpectedPayables = (from d in myShipmentsContext.ShipmentPayables
                                                                     where d.Tenant == tenant
                                                                     && d.ShipmentId == item.ShipmentId
                                                                     && d.ChargesTypeId == item.ChargesTypeId
                                                                     select d.ExpectedAmountLocal).Sum();
                                    }

                                    else
                                    {
                                        myRecord.ExpectedPayables = (from d in myShipmentsContext.ShipmentPayables
                                                                     where d.Tenant == tenant
                                                                     && d.ShipmentId == item.ShipmentId
                                                                     && d.ChargesTypeId == item.ChargesTypeId
                                                                     select d.ExpectedAmountInProfitCurrency).Sum();
                                    }       
                                }

                                customFieldResolver.SetDataProviderCustomFieldsValues("Shipment", tenant, myShipment, myRecord);

                                if (myCurrency != null)
                                {
                                    myRecord.InvoiceCurrencyCode = myCurrency.Code;
                                }

                                myRecord.AccountedPayables = myRecord.Payables;
                                myRecord.AccountedPayablesCurrencyCode = myRecord.InvoiceCurrencyCode;

                                if (this.SelectedCurrencyId == invoice.InvoiceCurrencyId)
                                {
                                    myRecord.AccountedPayablesCurrencyRate = 1;
                                }

                                else if (this.IsLocalCurrency)
                                {
                                    myRecord.AccountedPayablesCurrencyRate = myRecord.InvoiceCurrencyRate;
                                }

                                else
                                {
                                    myRecord.AccountedPayablesCurrencyRate = (1 / invoice.ProfitCurrencyExchangeRate) * myRecord.InvoiceCurrencyRate;
                                    myRecord.AccountedPayablesCurrencyRate = MethodHelper.Round(myRecord.AccountedPayablesCurrencyRate, 2);
                                }

                                if (myCard != null)
                                {
                                    myRecord.CardCode = myCard.Code;
                                    myRecord.CardName = myCard.EnglishName;
                                    myRecord.CardExternal = myCard.PayablesAccountingCard;
                                    myRecord.VendorName = myCard.EnglishName;
                                }

                                if (myContact != null)
                                {
                                    myRecord.CreatedByUser = myContact.EnglishName;
                                }

                                if (myBranch != null)
                                {
                                    myRecord.BranchCode = myBranch.Code;
                                    myRecord.BranchName = myBranch.EnglishName;
                                    myRecord.BranchLocalName = myBranch.LocalName;
                                    myRecord.BranchExternalId = myBranch.ExternalId;
                                }

                                if (myIncoterm != null)
                                {
                                    myRecord.IncotermCode = myIncoterm.Code;
                                    myRecord.IncotermName = myIncoterm.Name;
                                }

                                ChargesType myChargesType = allChargesTypes.Where(d => d.Id == item.ChargesTypeId).FirstOrDefault();
                                if (myChargesType != null)
                                {
                                    myRecord.ChargeTypeCode = myChargesType.Code;
                                    myRecord.ChargeTypeName = myChargesType.EnglishName;
                                    myRecord.ChargeTypeLocalName = myChargesType.LocalName;
                                }

                                myDataProvider.Shipments.Add(myRecord);
                            }
                            #endregion
                        }
                    }

                    List<string> myARInvoicesIds = new List<string>();
                    if (housesAndDirectOnly)
                    {
                        myARInvoicesIds = allShipmentsARInvoices.Where(d => d.ShipmentId == myShipment.Id || d.ShipmentId == myShipment.MasterShipmentDataId).Select(s => s.InvoiceId).ToList();
                    }

                    else
                    {
                        myARInvoicesIds = allShipmentsARInvoices.Where(d => d.ShipmentId == myShipment.Id).Select(s => s.InvoiceId).ToList();
                    }
                    foreach (string id in myARInvoicesIds)
                    {
                        ARInvoice invoice = allARInvoices.Where(d => d.Id == id).FirstOrDefault();
                        if (invoice != null)
                        {
                            #region AR Invoices
                            Card myCard = allCards.Where(d => d.Id == invoice.BillToId).FirstOrDefault();
                            Card partner = allCards.Where(d => d.Id == invoice.PartnerId).FirstOrDefault();
                            Contact myContact = allContacts.Where(d => d.Id == invoice.CreatedByUserId).FirstOrDefault();
                            Currency myCurrency = allCurrencies.Where(d => d.Id == invoice.InvoiceCurrencyId).FirstOrDefault();

                            List<ChargeTypeGroupClass> lines_Grouped = new List<ChargeTypeGroupClass>();
                            if (housesAndDirectOnly)
                            {
                                lines_Grouped = allARInvoiceLinesData.Where(d => d.InvoiceId == invoice.Id && (d.ShipmentId == myShipment.Id || d.ShipmentId == myShipment.MasterShipmentDataId)).ToList();
                            }

                            else
                            {
                                lines_Grouped = allARInvoiceLinesData.Where(d => d.InvoiceId == invoice.Id && d.ShipmentId == myShipment.Id).ToList();
                            }

                            foreach (ChargeTypeGroupClass item in lines_Grouped)
                            {
                                ArchivoExportadoShipmentItem myRecord = new ArchivoExportadoShipmentItem();
                                myRecord.LineTypeCode = invoice.StatusCode == "DR" ? "FX" : "FC";
                                myRecord.ShipmentNumber = myShipment.ShipmentNumber;
                                myRecord.OriginCode = myShipment.MainCarriageFromPortCode;
                                myRecord.DestinationCode = myShipment.MainCarriageFinalDestinationPortCode;
                                myRecord.LongMaster = longMaster;
                                myRecord.DirectionPartner = myDirectionPartner;
                                myRecord.DescriptionOfGoods = myShipment.DescriptionOfGoods;
                                myRecord.Salesman = myShipment.SalesmanUserName;
                                myRecord.Receivables = this.ComputeReceivables(item, invoice, myShipment);
                                myRecord.InvoiceNumber = invoice.InvoiceNumber;
                                myRecord.InvoiceDate = invoice.InvoiceDate;
                                myRecord.InvoiceCurrencyRate = invoice.InvoiceCurrencyExchangeRate;
                                myRecord.CustomerExternalID = customerExternalID;
                                myRecord.Shipper = myShipment.ShipperName;
                                myRecord.ShipmentStatus = myShipment.ShipmentStatusName;
                                myRecord.AccountingClosed = myShipment.IsAccountingClosed;
                                myRecord.ShipperNotExporter = myShipment.ShipperNotExporterName;
                                myRecord.Consignee = myShipment.ConsigneeName;
                                myRecord.ConsigneeNotImporter = myShipment.ConsigneeNotImporterName;
                                myRecord.Direction = myShipment.DirectionName;
                                myRecord.OperationalDate = myShipment.OperationalDate;
                                myRecord.Origin = myShipment.Origin;
                                myRecord.Destination = myShipment.LastFinalDestination;
                                myRecord.ChargeableWeight = myShipment.ChargeableWeight;
                                myRecord.Profit = this.IsLocalCurrency ? myShipment.ProfitInLocalCurrency : myShipment.ProfitInProfitCurrency;
                                myRecord.ETD = myShipment.MainCarriageETD;
                                myRecord.CustomerRef1 = myShipment.CustomerReference1;
                                myRecord.CustomerRef2 = myShipment.CustomerReference2;
                                myRecord.CountryOfOrigin = myShipment.MainCarriageFromPortCountryName;
                                myRecord.House = myShipment.House;
                                myRecord.ContainersNumbers = shipmentPackageRepository.GetContainersNumbersByShipmentIdAndTenant(myShipment.Id, myShipment.Tenant);
                                myRecord.ShipmentCreateDate = myShipment.CreateDateTime;
                                myRecord.ShipmentNotes = myShipment.Notes;
                                myRecord.ShipmentOpenedBy = myShipment.CreatedByUserName;
                                myRecord.InvoiceAmountDueInLocalCurrency = invoice.AmountDueInLocalCurrency;
                                myRecord.InvoiceAmountDueInInvoiceCurrency = invoice.AmountDue;
                                myRecord.AccountedReceivablesInInvoiceCurrency = this.ComputeAccountedReceivablesInInvoiceCurrency(invoice.InvoiceCurrencyId, myShipment, invoice.InvoiceDate);
                                myRecord.AccountedPayablesInInvoiceCurrency = this.ComputeAccountedPayablesInInvoiceCurrency(invoice.Id, invoice.InvoiceCurrencyId, myShipment, invoice.InvoiceDate);
                                myRecord.PaidDate = invoice.PaidDate;

                                ShipmentPickUpDelivery myLastDelivery = shipmentPickUpDeliveriesLists.Where(d => d.ShipmentId == myShipment.Id && d.PickUpDeliveryTypeCode == "DELV").OrderByDescending(s => s.PickUpDeliveryNumber).FirstOrDefault();
                                myRecord.CountryOfDestination = this.ComputeCountryOfDistination(myShipment, myLastDelivery);

                                customFieldResolver.SetDataProviderCustomFieldsValues("Shipment", tenant, myShipment, myRecord);
                                customFieldResolver.SetDataProviderCustomFieldsValues("ARInvoice", tenant, invoice, myRecord);

                                if (myCurrency != null)
                                {
                                    myRecord.InvoiceCurrencyCode = myCurrency.Code;
                                }

                                myRecord.AccountedReceivables = myRecord.Receivables;
                                myRecord.AccountedReceivablesCurrencyCode = myRecord.InvoiceCurrencyCode;

                                if (this.SelectedCurrencyId == invoice.InvoiceCurrencyId)
                                {
                                    myRecord.AccountedReceivablesCurrencyRate = 1;
                                }

                                else if (this.IsLocalCurrency)
                                {
                                    myRecord.AccountedReceivablesCurrencyRate = myRecord.InvoiceCurrencyRate;
                                }

                                else
                                {
                                    myRecord.AccountedReceivablesCurrencyRate = (1 / invoice.ProfitCurrencyExchangeRate) * myRecord.InvoiceCurrencyRate;
                                    myRecord.AccountedReceivablesCurrencyRate = MethodHelper.Round(myRecord.AccountedReceivablesCurrencyRate, 2);
                                }

                                if (myCard != null)
                                {
                                    myRecord.CardCode = myCard.Code;
                                    myRecord.CardName = myCard.EnglishName;
                                    myRecord.CardExternal = myCard.ReceivablesAccountingCard;
                                    myRecord.BillToName = myCard.EnglishName;
                                }
                                if (partner != null)
                                {                  
                                    myRecord.PartnerName = partner.EnglishName;
                                }

                                if (myContact != null)
                                {
                                    myRecord.CreatedByUser = myContact.EnglishName;
                                }

                                if (myBranch != null)
                                {
                                    myRecord.BranchCode = myBranch.Code;
                                    myRecord.BranchName = myBranch.EnglishName;
                                    myRecord.BranchLocalName = myBranch.LocalName;
                                    myRecord.BranchExternalId = myBranch.ExternalId;
                                }

                                if (myIncoterm != null)
                                {
                                    myRecord.IncotermCode = myIncoterm.Code;
                                    myRecord.IncotermName = myIncoterm.Name;
                                }

                                ChargesType myChargesType = allChargesTypes.Where(d => d.Id == item.ChargesTypeId).FirstOrDefault();
                                if (myChargesType != null)
                                {
                                    myRecord.ChargeTypeCode = myChargesType.Code;
                                    myRecord.ChargeTypeName = myChargesType.EnglishName;
                                    myRecord.ChargeTypeLocalName = myChargesType.LocalName;
                                }

                                myDataProvider.Shipments.Add(myRecord);
                            }
                            #endregion
                        }
                    }
                    #endregion
                }
            }
        }

        private IQueryable<ShipmentDataView> GetIQueryableShipments()
        {
            ShipmentRepository myShipmentRepository = new ShipmentRepository(myShipmentsContext);
            IQueryable<ShipmentDataView> iQueryable_Shipments = myShipmentRepository.GetShipmentViewsByTenant(tenant);

            iQueryable_Shipments = (from d in iQueryable_Shipments
                                    where
                                    (
                                    (d.OpenPayablesInLocalCurrency != null && d.OpenPayablesInLocalCurrency != 0)
                                    ||
                                    (d.AccountedPayablesInLocalCurrency != null && d.AccountedPayablesInLocalCurrency != 0)
                                    ||
                                    (d.OpenReceivablesInLocalCurrency != null && d.OpenReceivablesInLocalCurrency != 0)
                                    ||
                                    (d.AccountedReceivablesInLocalCurrency != null && d.AccountedReceivablesInLocalCurrency != 0)
                                    )
                                    select d);

            if (this.SelectedDateType == "CRT")
            {
                iQueryable_Shipments = iQueryable_Shipments.Where(d => d.CreateDateTime != null);

                if (this.FromDate != null)
                {
                    iQueryable_Shipments = iQueryable_Shipments.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.CreateDateTime) >= System.Data.Entity.DbFunctions.TruncateTime(this.FromDate));
                }

                if (this.ToDate != null)
                {
                    iQueryable_Shipments = iQueryable_Shipments.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.CreateDateTime) <= System.Data.Entity.DbFunctions.TruncateTime(this.ToDate));
                }
            }

            else if (this.SelectedDateType == "REG")
            {
                iQueryable_Shipments = iQueryable_Shipments.Where(d => d.RegistryDate != null);

                if (this.FromDate != null)
                {
                    iQueryable_Shipments = iQueryable_Shipments.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.RegistryDate) >= System.Data.Entity.DbFunctions.TruncateTime(this.FromDate));
                }

                if (this.ToDate != null)
                {
                    iQueryable_Shipments = iQueryable_Shipments.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.RegistryDate) <= System.Data.Entity.DbFunctions.TruncateTime(this.ToDate));
                }
            }

            else if (this.SelectedDateType == "OPE")
            {
                iQueryable_Shipments = iQueryable_Shipments.Where(d => d.OperationalDate != null);

                if (this.FromDate != null)
                {
                    iQueryable_Shipments = iQueryable_Shipments.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.OperationalDate) >= System.Data.Entity.DbFunctions.TruncateTime(this.FromDate));
                }

                if (this.ToDate != null)
                {
                    iQueryable_Shipments = iQueryable_Shipments.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.OperationalDate) <= System.Data.Entity.DbFunctions.TruncateTime(this.ToDate));
                }
            }

            else if (this.SelectedDateType == "OPC")
            {
                iQueryable_Shipments = iQueryable_Shipments.Where(d => d.OperationalCloseDate != null);

                if (this.FromDate != null)
                {
                    iQueryable_Shipments = iQueryable_Shipments.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.OperationalCloseDate) >= System.Data.Entity.DbFunctions.TruncateTime(this.FromDate));
                }

                if (this.ToDate != null)
                {
                    iQueryable_Shipments = iQueryable_Shipments.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.OperationalCloseDate) <= System.Data.Entity.DbFunctions.TruncateTime(this.ToDate));
                }
            }

            else if (this.SelectedDateType == "ACD") iQueryable_Shipments = FilterShipmentsByAccountingCloseDate(iQueryable_Shipments);

            if (!this.IncludeCancelledShipments)
            {
                iQueryable_Shipments = iQueryable_Shipments.Where(d => !d.IsCancelled);
            }

            if (housesAndDirectOnly)
            {
                iQueryable_Shipments = iQueryable_Shipments.Where(d => d.ShipmentLevelCode != "C");
            }

            return iQueryable_Shipments;
        }

        private IQueryable<ShipmentDataView> FilterShipmentsByAccountingCloseDate(IQueryable<ShipmentDataView> iQueryable_Shipments)
        {
            iQueryable_Shipments = iQueryable_Shipments.Where(d => d.AccountingCloseDate != null);
            if (this.FromDate != null) iQueryable_Shipments = iQueryable_Shipments.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.AccountingCloseDate) >= System.Data.Entity.DbFunctions.TruncateTime(this.FromDate));
            if (this.ToDate != null) iQueryable_Shipments = iQueryable_Shipments.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.AccountingCloseDate) <= System.Data.Entity.DbFunctions.TruncateTime(this.ToDate));
            return iQueryable_Shipments;
        }

        private void BuildInvoicesLists(List<string> allShipmentsIds)
        {
            IQueryable<APInvoice> iQueryable_APInvoices = (from d in myInvoiceContext.APInvoiceEntities.Include("APInvoice")
                                                           where d.Tenant == tenant
                                                           && allShipmentsIds.Contains(d.EntityId)
                                                           select d.APInvoice);

            IQueryable<ARInvoice> iQueryable_ARInvoices = (from d in myInvoiceContext.ARInvoiceEntities.Include("ARInvoice")
                                                           where d.Tenant == tenant
                                                           && allShipmentsIds.Contains(d.EntityId)
                                                           select d.ARInvoice);            

            iQueryable_APInvoices = iQueryable_APInvoices.Where(d => d.StatusCode != "VD");
            iQueryable_ARInvoices = iQueryable_ARInvoices.Where(d => d.StatusCode != "VD" && d.StatusCode != "LL");

            if (!this.IncludeDraftInvoices)
            {
                iQueryable_ARInvoices = iQueryable_ARInvoices.Where(d => d.StatusCode != "DR");
            }

            this.allAPInvoices = iQueryable_APInvoices.ToList();
            this.allARInvoices = iQueryable_ARInvoices.ToList();
        }
        private void BuildInvoicesShipmentsLists(List<string> allShipmentsIds)
        {
            allShipmentsAPInvoices = (from d in myInvoiceContext.APInvoiceEntities
                                      where d.Tenant == tenant
                                      && allShipmentsIds.Contains(d.EntityId)
                                      group d by new { d.APInvoiceId, d.EntityId } into g
                                      select new ShipmentInvoice()
                                      {
                                          InvoiceId = g.Key.APInvoiceId,
                                          ShipmentId = g.Key.EntityId
                                      }).ToList();

            allShipmentsARInvoices = (from d in myInvoiceContext.ARInvoiceEntities
                                      where d.Tenant == tenant
                                      && allShipmentsIds.Contains(d.EntityId)
                                      group d by new { d.ARInvoiceId, d.EntityId } into g
                                      select new ShipmentInvoice()
                                      {
                                          InvoiceId = g.Key.ARInvoiceId,
                                          ShipmentId = g.Key.EntityId
                                      }).ToList();
        }
        private List<ChargeTypeGroupClass> GetPayablesData(List<string> allShipmentsIds)
        {
            return (from d in myShipmentsContext.ShipmentPayables
                          where d.Tenant == tenant
                          && d.OpenAmount != null
                          && d.OpenAmount != 0
                          && allShipmentsIds.Contains(d.ShipmentId)
                          group d by new { d.ShipmentId, d.ChargesTypeId, d.VendorId } into g
                          select new ChargeTypeGroupClass()
                          {
                              CardId = g.Key.VendorId,
                              ShipmentId = g.Key.ShipmentId,
                              ChargesTypeId = g.Key.ChargesTypeId,
                              AmountInLocal = g.Sum(s => s.OpenAmountInLocalCurrency),
                              AmountInProfit = g.Sum(s => s.OpenAmountInProfitCurrency),
                              ExpectedAmountInLocal = g.Sum(s => s.ExpectedAmountLocal),
                              ExpectedAmountInProfit = g.Sum(s => s.ExpectedAmountInProfitCurrency),
                          }).ToList();
        }
        private List<ChargeTypeGroupClass> GetReceivablesData(List<string> allShipmentsIds)
        {
            return (from d in myShipmentsContext.ShipmentReceivables
                           where d.Tenant == tenant
                           && d.ShipmentReceivableLineStatusCode == "OAMT"
                           && allShipmentsIds.Contains(d.ShipmentId)
                           group d by new { d.ShipmentId, d.ChargesTypeId } into g
                           select new ChargeTypeGroupClass()
                           {
                               ShipmentId = g.Key.ShipmentId,
                               ChargesTypeId = g.Key.ChargesTypeId,
                               AmountInLocal = g.Sum(s => s.TotalAmountLocal),
                               AmountInProfit = g.Sum(s => s.AmountInProfitCurrency)
                           }).ToList();
        }
        private List<ChargeTypeGroupClass> GetARInvoiceLinesData(List<string> allARInvoicesIds)
        {
            return (from d in myInvoiceContext.ARInvoiceLines
                    where d.Tenant == tenant
                    && allARInvoicesIds.Contains(d.ARInvoiceId)
                    group d by new { d.ARInvoiceId, d.EntityId, d.ChargesTypeId, d.ReceivableId } into g
                    select new ChargeTypeGroupClass()
                    {
                        InvoiceId = g.Key.ARInvoiceId,
                        ShipmentId = g.Key.EntityId,
                        ChargesTypeId = g.Key.ChargesTypeId,
                        ReceivableId = g.Key.ReceivableId,
                        AmountInLocal = g.Sum(s => s.LocalCurrencyAmount),
                        AmountInProfit = g.Sum(s => s.ProfitCurrencyAmount)
                    }).ToList();
        }
        private List<ChargeTypeGroupClass> GetAPInvoiceLinesData(List<string> allAPInvoicesIds)
        {
            return (from d in myInvoiceContext.APInvoiceLines
                    where d.Tenant == tenant
                    && allAPInvoicesIds.Contains(d.APInvoiceId)
                    group d by new { d.APInvoiceId, d.EntityId, d.ChargesTypeId, d.EntityPayableId } into g
                    select new ChargeTypeGroupClass()
                    {
                        InvoiceId = g.Key.APInvoiceId,
                        ShipmentId = g.Key.EntityId,
                        ChargesTypeId = g.Key.ChargesTypeId,
                        PayableId = g.Key.EntityPayableId,
                        AmountInLocal = g.Sum(s => s.LocalCurrencyAmount),
                        AmountInProfit = g.Sum(s => s.ProfitCurrencyAmount),
                    }).ToList();
        }
        private List<Card> GetCards(List<ChargeTypeGroupClass> allPayablesData)
        {
            List<Card> myResult = new List<Card>();

            List<string> allIds = new List<string>();
            List<string> allIds_APInvoice = allAPInvoices.Where(d => d.VendorId != null).Select(s => s.VendorId).ToList();
            List<string> allIds_ARInvoice = allARInvoices.Where(d => d.BillToId != null).Select(s => s.BillToId).ToList();
            List<string> allIds_Payables = allPayablesData.Where(d => d.CardId != null).Select(s => s.CardId).ToList();
            List<string> partnersIds = allARInvoices.Where(d => d.PartnerId != null).Select(d => d.PartnerId).ToList();

            foreach (string id in allIds_APInvoice)
            {
                if (!allIds.Contains(id))
                {
                    allIds.Add(id);
                }
            }
            foreach (string id in allIds_ARInvoice)
            {
                if (!allIds.Contains(id))
                {
                    allIds.Add(id);
                }
            }
            foreach (string id in allIds_Payables)
            {
                if (!allIds.Contains(id))
                {
                    allIds.Add(id);
                }
            }
            foreach (string id in partnersIds)
            {
                if (!allIds.Contains(id))
                {
                    allIds.Add(id);
                }
            }

            myResult = (from d in myCommonContext.Cards
                        where d.Tenant == tenant
                        && allIds.Contains(d.Id)
                        select d).ToList();

            return myResult;
        }
        private List<Contact> GetContacts()
        {
            List<Contact> myResult = new List<Contact>();

            List<string> allIds = new List<string>();
            List<string> allIds_APInvoice = allAPInvoices.Select(s => s.CreatedByUserId).ToList();
            List<string> allIds_ARInvoice = allARInvoices.Select(s => s.CreatedByUserId).ToList();

            foreach (string id in allIds_APInvoice)
            {
                if (!allIds.Contains(id))
                {
                    allIds.Add(id);
                }
            }
            foreach (string id in allIds_ARInvoice)
            {
                if (!allIds.Contains(id))
                {
                    allIds.Add(id);
                }
            }

            myResult = (from d in myCommonContext.Contacts
                        where d.Tenant == tenant
                        && allIds.Contains(d.Id)
                        select d).ToList();

            return myResult;
        }
        private bool HasAmount(double? value)
        {
            bool myResult = false;

            if (value != null && value != 0)
            {
                myResult = true;
            }

            return myResult;
        }
        private string GetLongMaster(ShipmentDataView myShipment)
        {
            string longMaster = "";
            if (myShipment.TransportModeId == "A")
            {
                if (!string.IsNullOrEmpty(myShipment.AirlinePrefix) && !string.IsNullOrEmpty(myShipment.Master))
                {
                    longMaster = myShipment.AirlinePrefix + "-" + myShipment.Master;
                }
            }

            else
            {
                longMaster = myShipment.Master;
            }

            return longMaster;
        }
        private string GetDateString(DateTime? date)
        {
            string myResult = null;

            if (date != null)
            {
                //myResult = date.Value.Day.ToString() + @"/" + date.Value.Month.ToString() + @"/" + date.Value.Year.ToString();
                myResult = String.Format("{0:dd/MM/yyyy}", date);
            }

            return myResult;
        }
        private string ComputeCountryOfDistination(ShipmentDataView myShipment, ShipmentPickUpDelivery myLastDelivery)
        {
            string countryName = null;

            if (myShipment.DirectionId == "D" && myShipment.TransportModeId == "I")
            {
                InlandDomesticArgs args = new InlandDomesticArgs()
                {
                    InlandDomesticFromTypeCode = myShipment.InlandDomesticFromTypeCode,
                    MainCarriageFromAddressId = myShipment.MainCarriageFromAddressId,
                    MainCarriageFromPortId = myShipment.MainCarriageFromPortId,
                    InlandDomesticFromCity = myShipment.InlandDomesticFromCity,
                    InlandDomesticFromCountryId = myShipment.InlandDomesticFromCountryId,
                    InlandDomesticToTypeCode = myShipment.InlandDomesticToTypeCode,
                    MainCarriageToAddressId = myShipment.MainCarriageToAddressId,
                    InlandDomesticToCity = myShipment.InlandDomesticToCity,
                    InlandDomesticToCountryId = myShipment.InlandDomesticToCountryId,
                    MainCarriageToPortId = myShipment.MainCarriageToPortId,
                    MainCarriageFromPortCountryCode = myShipment.MainCarriageFromPortCountryCode,
                    MainCarriageToPortCountryCode = myShipment.MainCarriageToPortCountryCode
                };
                countryName = servicHelper.GetInlandDomesticToCountryName(args);
            }

            else
            {
                if (myLastDelivery != null)
                {
                    switch (myLastDelivery.PickUpDeliveryToTypeCode)
                    {
                        case "PART":
                            {
                                if (!string.IsNullOrEmpty(myLastDelivery.ToAddressId))
                                {
                                    Address myPartnerAddress = addressRepository.GetSingleAddress(myLastDelivery.ToAddressId, tenant);
                                    if (myPartnerAddress != null)
                                    {
                                        countryName = myPartnerAddress.Country != null ? myPartnerAddress.Country.EnglishName : null;
                                    }
                                }

                                break;
                            }

                        case "PORT":
                            {
                                if (!string.IsNullOrEmpty(myLastDelivery.ToPortId))
                                {
                                    PortPM myPort = PortQuery.GetSinglePort(tenant, myLastDelivery.ToPortId, true);
                                    if (myPort != null)
                                    {
                                        countryName = myPort.CountryName;
                                    }
                                }

                                break;
                            }

                        case "CASL":
                            {
                                string myCountry = myLastDelivery.ToAddressCountry != null ? myLastDelivery.ToAddressCountry.EnglishName : null;
                                if (!string.IsNullOrEmpty(myCountry))
                                {
                                    countryName = myCountry;
                                }

                                break;
                            }
                    }
                }

                else if (myShipment.DirectionId == "I" && !string.IsNullOrEmpty(myShipment.WarehouseLegWarehouseId))
                {
                    Card warehouse = CardRepository.GetSingleCard(myShipment.WarehouseLegWarehouseId, tenant, true);
                    if (warehouse != null)
                    {
                        countryName = warehouse.CountryName;
                    }
                }

                else if (!string.IsNullOrEmpty(myShipment.OnForwardingToPortId))
                {
                    PortPM onForwardingToPort = PortQuery.GetSinglePort(tenant, myShipment.OnForwardingToPortId, true);
                    if (onForwardingToPort != null)
                    {
                        countryName = onForwardingToPort.CountryName;
                    }
                }

                else if (!string.IsNullOrEmpty(myShipment.OnCarriageToPortId))
                {
                    PortPM onCarriageToPort = PortQuery.GetSinglePort(tenant, myShipment.OnCarriageToPortId, true);
                    if (onCarriageToPort != null)
                    {
                        countryName = onCarriageToPort.CountryName;
                    }
                }

                else
                {
                    if (!string.IsNullOrEmpty(myShipment.Transshipment3ToPortId))
                    {
                        PortPM transshipment3ToPort = PortQuery.GetSinglePort(tenant, myShipment.Transshipment3ToPortId, true);
                        if (transshipment3ToPort != null)
                        {
                            countryName = transshipment3ToPort.CountryName;
                        }
                    }

                    else if (!string.IsNullOrEmpty(myShipment.Transshipment2ToPortId))
                    {
                        PortPM transshipment2ToPort = PortQuery.GetSinglePort(tenant, myShipment.Transshipment2ToPortId, true);
                        if (transshipment2ToPort != null)
                        {
                            countryName = transshipment2ToPort.CountryName;

                        }
                    }

                    else if (!string.IsNullOrEmpty(myShipment.Transshipment1ToPortId))
                    {
                        PortPM transshipment1ToPort = PortQuery.GetSinglePort(tenant, myShipment.Transshipment1ToPortId, true);
                        if (transshipment1ToPort != null)
                        {
                            countryName = transshipment1ToPort.CountryName;

                        }
                    }

                    else if (!string.IsNullOrEmpty(myShipment.MainCarriageToPortId))
                    {
                        PortPM mainCarriageToPort = PortQuery.GetSinglePort(tenant, myShipment.MainCarriageToPortId, true);
                        if (mainCarriageToPort != null)
                        {
                            countryName = mainCarriageToPort.CountryName;
                        }
                    }
                    else if (!string.IsNullOrEmpty(myShipment.ToPortId))
                    {
                        PortPM mainCarriageToPort = PortQuery.GetSinglePort(tenant, myShipment.ToPortId, true);
                        if (mainCarriageToPort != null)
                        {
                            countryName = mainCarriageToPort.CountryName;
                        }
                    }
                }
            }

            return countryName;
        }
        private double? ComputeAccountedReceivablesInInvoiceCurrency(string invoiceCurrencyId, ShipmentDataView myShipment, DateTime? invoiceDate)
        {
            double? myResult = null;

            if (invoiceCurrencyId == myShipment.ProfitCurrencyId)
            {
                myResult = myShipment.AccountedReceivablesInProfitCurrency;
            }

            else if (invoiceCurrencyId == tenantLocalCurrencyId)
            {
                myResult = myShipment.AccountedReceivablesInLocalCurrency;
            }

            else
            {
                double? rate = this.GetCurrencysExchangeRate(invoiceCurrencyId, invoiceDate);
                if (rate != null && rate != 0)
                {
                    myResult = myShipment.AccountedReceivablesInLocalCurrency / rate;
                }
            }

            return myResult;
        }
        private double? ComputeAccountedPayablesInInvoiceCurrency(string invoiceId, string invoiceCurrencyId, ShipmentDataView myShipment, DateTime? invoiceDate, string payableId = null)
        {
            double? myResult = null;

            if (housesAndDirectOnly && myShipment.ShipmentLevelCode == "H" && !string.IsNullOrEmpty(myShipment.MasterShipmentDataId))
            {
                myResult = this.ComputeAccountedPayables_ConnectedHouse(invoiceId, invoiceCurrencyId, myShipment, invoiceDate, payableId);
            }

            else
            {
                if (invoiceCurrencyId == myShipment.ProfitCurrencyId)
                {
                    myResult = myShipment.AccountedPayablesInProfitCurrency;
                }

                else if (invoiceCurrencyId == tenantLocalCurrencyId)
                {
                    myResult = myShipment.AccountedPayablesInLocalCurrency;
                }

                else
                {
                    double? rate = this.GetCurrencysExchangeRate(invoiceCurrencyId, invoiceDate);
                    if (rate != null && rate != 0)
                    {
                        myResult = myShipment.AccountedPayablesInLocalCurrency / rate;
                    }
                }
            }

            return myResult;
        }
        private double? ComputeAccountedPayables_ConnectedHouse(string invoiceId, string invoiceCurrencyId, ShipmentDataView myShipment, DateTime? invoiceDate, string payableId)
        {
            double? myResult = null;
            ShipmentPayable housePayable = (from d in myShipmentsContext.ShipmentPayables
                                            where d.Tenant == tenant
                                            && d.ShipmentPayableParentId == payableId
                                            && d.ShipmentId == myShipment.Id
                                            select d).FirstOrDefault();

            if (housePayable != null)
            {
                PayableProratedAmount payableProratedAmounts = (from d in myShipmentsContext.PayableProratedAmounts
                                                                      where d.Tenant == tenant
                                                                      && d.PayableId == housePayable.Id
                                                                      && d.InvoiceId == invoiceId
                                                                      && d.ShipmentId == myShipment.Id
                                                                      select d).FirstOrDefault();
                if (payableProratedAmounts != null)
                {
                    if (invoiceCurrencyId == myShipment.ProfitCurrencyId)
                    {
                        myResult = payableProratedAmounts.ProratedAmountInProfitCurrency;
                    }

                    else if (invoiceCurrencyId == tenantLocalCurrencyId)
                    {
                        myResult = payableProratedAmounts.ProratedAmountInLocalCurrency;
                    }

                    else
                    {
                        double? rate = this.GetCurrencysExchangeRate(invoiceCurrencyId, invoiceDate);
                        if (rate != null && rate != 0)
                        {
                            myResult = payableProratedAmounts.ProratedAmountInLocalCurrency / rate;
                        }
                    }
                }
            }

            else
            {
                if (invoiceCurrencyId == myShipment.ProfitCurrencyId)
                {
                    myResult = myShipment.AccountedPayablesInProfitCurrency;
                }

                else if (invoiceCurrencyId == tenantLocalCurrencyId)
                {
                    myResult = myShipment.AccountedPayablesInLocalCurrency;
                }

                else
                {
                    double? rate = this.GetCurrencysExchangeRate(invoiceCurrencyId, invoiceDate);
                    if (rate != null && rate != 0)
                    {
                        myResult = myShipment.AccountedPayablesInLocalCurrency / rate;
                    }
                }
            }

            return myResult;
        }
        private double? ComputePayables(ChargeTypeGroupClass item, APInvoice invoice, ShipmentDataView myShipment)
        {
            double? myResult = this.IsLocalCurrency ? item.AmountInLocal : item.AmountInProfit;

            if (housesAndDirectOnly && myShipment.ShipmentLevelCode == "H" && !string.IsNullOrEmpty(myShipment.MasterShipmentDataId))
            {
                ShipmentPayable housePayable = (from d in myShipmentsContext.ShipmentPayables
                                                where d.Tenant == tenant
                                                && d.ShipmentPayableParentId == item.PayableId
                                                && d.ShipmentId == myShipment.Id
                                                select d).FirstOrDefault();

                if (housePayable != null)
                {
                    PayableProratedAmount payableProratedAmounts = (from d in myShipmentsContext.PayableProratedAmounts
                                                                    where d.Tenant == tenant
                                                                    && d.PayableId == housePayable.Id
                                                                    && d.InvoiceId == invoice.Id
                                                                    && d.ShipmentId == myShipment.Id
                                                                    select d).FirstOrDefault();
                    if (payableProratedAmounts != null)
                    {
                        myResult = this.IsLocalCurrency ? payableProratedAmounts.ProratedAmountInLocalCurrency : payableProratedAmounts.ProratedAmountInProfitCurrency;
                    }
                }

                else
                {
                    myResult = this.IsLocalCurrency ? item.AmountInLocal : item.AmountInProfit;
                }
            }

            return myResult;
        }
        private double? ComputeReceivables(ChargeTypeGroupClass item, ARInvoice invoice, ShipmentDataView myShipment)
        {
            double? myResult = this.IsLocalCurrency ? item.AmountInLocal : item.AmountInProfit;

            if (housesAndDirectOnly && myShipment.ShipmentLevelCode == "H" && !string.IsNullOrEmpty(myShipment.MasterShipmentDataId))
            {
                ShipmentReceivable houseReceivable = (from d in myShipmentsContext.ShipmentReceivables
                                                where d.Tenant == tenant
                                                && d.ShipmentReceivableParentId == item.ReceivableId
                                                && d.ShipmentId == myShipment.Id
                                                select d).FirstOrDefault();

                if (houseReceivable != null)
                {
                    myResult = this.IsLocalCurrency ? houseReceivable.TotalAmountLocal : houseReceivable.AmountInProfitCurrency;
                }
            }

            return myResult;
        }
        private double? GetCurrencysExchangeRate(string foreignCurrencyId, DateTime? rateDate)
        {
            double? rate = null;

            LastRate lastRate = ratesTableQuery.GetLastRecordByValueDate(tenant, foreignCurrencyId, tenantLocalCurrencyId, rateDate);
            if (lastRate != null)
            {
                rate = lastRate.Rate;
            }

            return rate;
        }
    }
}