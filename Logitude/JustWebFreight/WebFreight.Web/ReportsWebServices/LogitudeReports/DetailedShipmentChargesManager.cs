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
        private bool SplitByCharges = false;
        private bool IncludeCancelledShipments = false;
        private string SelectedDateType = null;
        private IInvoiceContext myInvoiceContext;
        private ICommonDataContext myCommonContext;
        private IShipmentsContext myShipmentsContext;
        private CustomFieldResolver customFieldResolver;

        public DetailedShipmentChargesManager(byte[] xmlFilters, int tenant)
        {
            this.tenant = tenant;

            myInvoiceContext = InvoiceContext.GetContext(tenant);
            myCommonContext = CommonDataContext.GetContext(tenant);
            myShipmentsContext = ShipmentsContext.GetContext(tenant);
            customFieldResolver = new CustomFieldResolver();

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
            QueryFilterItem filterItem_SplitByCharges = myQueryOperations.QueryFilterItems.Where(d => d.FieldName == "SplitByCharges").FirstOrDefault();
            QueryFilterItem filterItem_IncludeCancelledShipments = myQueryOperations.QueryFilterItems.Where(d => d.FieldName == "IncludeCancelledShipments").FirstOrDefault();
            
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

            if (filterItem_SplitByCharges != null)
            {
                if (filterItem_SplitByCharges.FieldValue != null)
                {
                    SplitByCharges = (bool)filterItem_SplitByCharges.FieldValue;
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
        }

        public byte[] GetData()
        {
            ArchivoExportadoDataProvider myDataProvider = new ArchivoExportadoDataProvider();

            this.SplitByCharges = true;

            if (this.SplitByCharges)
            {
                myDataProvider = this.LoadDataProvider_SplitByCharges();
            }

            else
            {
                myDataProvider = this.LoadDataProvider();
            }

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ArchivoExportadoDataProvider));
            MemoryStream memoryStream = new MemoryStream();
            xmlSerializer.Serialize(memoryStream, myDataProvider);
            memoryStream.Seek(0, SeekOrigin.Begin);

            StreamReader streamReader = new StreamReader(memoryStream);
            string content = streamReader.ReadToEnd();
            byte[] bytearray = memoryStream.ToArray();
            return bytearray;
        }

        private ArchivoExportadoDataProvider LoadDataProvider()
        {
            ArchivoExportadoDataProvider myDataProvider = new ArchivoExportadoDataProvider();
            myDataProvider.Id = tenant;
            myDataProvider.CurrencyCode = this.SelectedCurrencyCode;
            myDataProvider.IncludeDraftInvoices = this.IncludeDraftInvoices ? "Yes" : "No";
            myDataProvider.IncludeEstimations = this.IncludeEstimations ? "Yes" : "No";
            myDataProvider.From = this.GetDateString(this.FromDate);
            myDataProvider.To = this.GetDateString(this.ToDate);
            myDataProvider.Shipments = new List<ArchivoExportadoShipmentItem>();
            IQueryable<ShipmentDataView> iQueryable_Shipments = this.GetIQueryableShipments();

            if (this.IncludeCancelledShipments)
            {

            }
            else
            {
                iQueryable_Shipments = iQueryable_Shipments.Where(d => !d.IsCancelled);
            }

            List<ShipmentDataView> allShipments = iQueryable_Shipments.ToList();

            if (allShipments.Count > 0)
            {
                #region Data
                List<string> allShipmentsIds = allShipments.Select(s => s.Id).ToList();

                IQueryable<APInvoice> iQueryable_APInvoice = (from d in myInvoiceContext.APInvoiceEntities.Include("APInvoice")
                                                              where d.Tenant == tenant
                                                              && allShipmentsIds.Contains(d.EntityId)
                                                              select d.APInvoice);

                IQueryable<ARInvoice> iQueryable_ARInvoice = (from d in myInvoiceContext.ARInvoiceEntities.Include("ARInvoice")
                                                              where d.Tenant == tenant
                                                              && allShipmentsIds.Contains(d.EntityId)
                                                              select d.ARInvoice);

                iQueryable_APInvoice = iQueryable_APInvoice.Where(d => d.StatusCode != "VD" && d.StatusCode != "WA");
                iQueryable_ARInvoice = iQueryable_ARInvoice.Where(d => d.StatusCode != "VD" && d.StatusCode != "LL");

                if (!this.IncludeDraftInvoices)
                {
                    iQueryable_ARInvoice = iQueryable_ARInvoice.Where(d => d.StatusCode != "DR");
                }

                List<APInvoice> allAPInvoices = iQueryable_APInvoice.ToList();
                List<ARInvoice> allARInvoices = iQueryable_ARInvoice.ToList();

                List<Card> allCards = new List<Card>();
                List<Contact> allContacts = new List<Contact>();
                List<Branch> allBranchs = new List<Branch>();
                List<Currency> allCurrencies = new List<Currency>();
                List<Incoterm> allIncoterms = new List<Incoterm>();

                if (allAPInvoices.Count > 0 || allARInvoices.Count > 0 || allShipments.Count > 0)
                {
                    allCards = this.GetCards(allAPInvoices, allARInvoices, new List<ChargeTypeGroupClass>());
                    allContacts = this.GetContacts(allAPInvoices, allARInvoices);
                    allBranchs = (from d in myCommonContext.Branches where d.Tenant == tenant select d).ToList();
                    allCurrencies = (from d in myCommonContext.Currencies where d.Tenant == tenant select d).ToList();
                    allIncoterms = (from d in myCommonContext.Incoterms where d.Tenant == tenant select d).ToList();
                }
                #endregion

                foreach (ShipmentDataView myShipment in allShipments)
                {
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
                            ArchivoExportadoShipmentItem myRecord = new ArchivoExportadoShipmentItem();
                            myRecord.ShipmentNumber = myShipment.ShipmentNumber;
                            myRecord.OriginCode = myShipment.MainCarriageFromPortCode;
                            myRecord.DestinationCode = myShipment.MainCarriageFinalDestinationPortCode;
                            myRecord.CountryOfOrigin = myShipment.MainCarriageFromPortCountryName;
                            myRecord.CountryOfDestination = myShipment.MainCarriageFinalDestinationCountryName;
                            

                            myRecord.LineTypeCode = "EFC";
                            myRecord.Payables = this.IsLocalCurrency ? myShipment.OpenPayablesInLocalCurrency : myShipment.OpenPayablesInProfitCurrency;
                            myRecord.LongMaster = longMaster;
                            myRecord.DirectionPartner = myDirectionPartner;
                            myRecord.DescriptionOfGoods = myShipment.DescriptionOfGoods;
                            myRecord.Salesman = myShipment.SalesmanUserName;
                            myRecord.CustomerExternalID = customerExternalID;
                            myRecord.Shipper = myShipment.ShipperName;
                            myRecord.ShipperNotExporter = myShipment.ShipperNotExporterName;
                            myRecord.Consignee = myShipment.ConsigneeName;
                            myRecord.ConsigneeNotImporter = myShipment.ConsigneeNotImporterName;
                            myRecord.Direction = myShipment.DirectionName;

                            if (!string.IsNullOrEmpty(myShipment.BranchId))
                            {
                                Branch myBranch = allBranchs.Where(d => d.Id == myShipment.BranchId).FirstOrDefault();
                                if (myBranch != null)
                                {
                                    myRecord.BranchCode = myBranch.Code;
                                    myRecord.BranchName = myBranch.EnglishName;
                                    myRecord.BranchLocalName = myBranch.LocalName;
                                    myRecord.BranchExternalId = myBranch.ExternalId;
                                }
                            }

                            if (!string.IsNullOrEmpty(myShipment.IncotermId))
                            {
                                Incoterm myIncoterm = allIncoterms.Where(d => d.Id == myShipment.IncotermId).FirstOrDefault();
                                if (myIncoterm != null)
                                {
                                    myRecord.IncotermCode = myIncoterm.Code;
                                    myRecord.IncotermName = myIncoterm.Name;
                                }
                            }

                            myDataProvider.Shipments.Add(myRecord);
                        }
                    }

                    foreach (APInvoice invoice in allAPInvoices.Where(d => d.MainEntityId == myShipment.Id))
                    {
                        #region AP Invoices
                        ArchivoExportadoShipmentItem myRecord = new ArchivoExportadoShipmentItem();
                        myRecord.LineTypeCode = "FC";
                        myRecord.ShipmentNumber = myShipment.ShipmentNumber;
                        myRecord.OriginCode = myShipment.MainCarriageFromPortCode;
                        myRecord.DestinationCode = myShipment.MainCarriageFinalDestinationPortCode;
                        myRecord.LongMaster = longMaster;
                        myRecord.DirectionPartner = myDirectionPartner;
                        myRecord.DescriptionOfGoods = myShipment.DescriptionOfGoods;
                        myRecord.Payables = this.IsLocalCurrency ? invoice.AmountInLocalCurrency : invoice.AmountInProfitCurrency;
                        myRecord.Salesman = myShipment.SalesmanUserName;
                        myRecord.CustomerExternalID = customerExternalID;
                        myRecord.InvoiceNumber = invoice.InvoiceNumber;
                        myRecord.InvoiceDate = invoice.InvoiceDate;
                        myRecord.InvoiceCurrencyRate = invoice.InvoiceCurrencyExchangeRate;
                        myRecord.Shipper = myShipment.ShipperName;
                        myRecord.ShipperNotExporter = myShipment.ShipperNotExporterName;
                        myRecord.Consignee = myShipment.ConsigneeName;
                        myRecord.ConsigneeNotImporter = myShipment.ConsigneeNotImporterName;
                        myRecord.Direction = myShipment.DirectionName;
                        myRecord.CountryOfOrigin = myShipment.MainCarriageFromPortCountryName;
                        myRecord.CountryOfDestination = myShipment.MainCarriageFinalDestinationCountryName;

                        Currency myCurrency = allCurrencies.Where(d => d.Id == invoice.InvoiceCurrencyId).FirstOrDefault();
                        if (myCurrency != null)
                        {
                            myRecord.InvoiceCurrencyCode = myCurrency.Code;
                        }

                        Card myCard = allCards.Where(d => d.Id == invoice.VendorId).FirstOrDefault();
                        if (myCard != null)
                        {
                            myRecord.CardCode = myCard.Code;
                            myRecord.CardName = myCard.EnglishName;
                            myRecord.CardExternal = myCard.PayablesAccountingCard;
                            myRecord.VendorName = myCard.EnglishName;
                        }

                        Contact myContact = allContacts.Where(d => d.Id == invoice.CreatedByUserId).FirstOrDefault();
                        if (myContact != null)
                        {
                            myRecord.CreatedByUser = myContact.EnglishName;
                        }

                        Branch myBranch = allBranchs.Where(d => d.Id == invoice.BranchId).FirstOrDefault();
                        if (invoice.IsMultipleEntities)
                        {
                            myBranch = allBranchs.Where(d => d.Id == myShipment.BranchId).FirstOrDefault();
                        }

                        if (myBranch != null)
                        {
                            myRecord.BranchCode = myBranch.Code;
                            myRecord.BranchName = myBranch.EnglishName;
                            myRecord.BranchLocalName = myBranch.LocalName;
                            myRecord.BranchExternalId = myBranch.ExternalId;
                        }

                        if (!string.IsNullOrEmpty(myShipment.IncotermId))
                        {
                            Incoterm myIncoterm = allIncoterms.Where(d => d.Id == myShipment.IncotermId).FirstOrDefault();
                            if (myIncoterm != null)
                            {
                                myRecord.IncotermCode = myIncoterm.Code;
                                myRecord.IncotermName = myIncoterm.Name;
                            }
                        }

                        myDataProvider.Shipments.Add(myRecord);
                        #endregion
                    }

                    foreach (ARInvoice invoice in allARInvoices.Where(d => d.MainEntityId == myShipment.Id))
                    {
                        #region AR Invoices
                        ArchivoExportadoShipmentItem myRecord = new ArchivoExportadoShipmentItem();
                        myRecord.LineTypeCode = invoice.StatusCode == "DR" ? "FX" : "FC";
                        myRecord.ShipmentNumber = myShipment.ShipmentNumber;
                        myRecord.OriginCode = myShipment.MainCarriageFromPortCode;
                        myRecord.DestinationCode = myShipment.MainCarriageFinalDestinationPortCode;
                        myRecord.LongMaster = longMaster;
                        myRecord.DirectionPartner = myDirectionPartner;
                        myRecord.DescriptionOfGoods = myShipment.DescriptionOfGoods;
                        myRecord.Receivables = this.IsLocalCurrency ? invoice.AmountInLocalCurrency : invoice.AmountInProfitCurrency;
                        myRecord.Salesman = myShipment.SalesmanUserName;
                        myRecord.CustomerExternalID = customerExternalID;
                        myRecord.InvoiceNumber = invoice.InvoiceNumber;
                        myRecord.InvoiceDate = invoice.InvoiceDate;
                        myRecord.InvoiceCurrencyRate = invoice.InvoiceCurrencyExchangeRate;
                        myRecord.Shipper = myShipment.ShipperName;
                        myRecord.ShipperNotExporter = myShipment.ShipperNotExporterName;
                        myRecord.Consignee = myShipment.ConsigneeName;
                        myRecord.ConsigneeNotImporter = myShipment.ConsigneeNotImporterName;
                        myRecord.Direction = myShipment.DirectionName;
                        myRecord.CountryOfOrigin = myShipment.MainCarriageFromPortCountryName;
                        myRecord.CountryOfDestination = myShipment.MainCarriageFinalDestinationCountryName;

                        Currency myCurrency = allCurrencies.Where(d => d.Id == invoice.InvoiceCurrencyId).FirstOrDefault();
                        if (myCurrency != null)
                        {
                            myRecord.InvoiceCurrencyCode = myCurrency.Code;
                        }

                        Card myCard = allCards.Where(d => d.Id == invoice.BillToId).FirstOrDefault();
                        if (myCard != null)
                        {
                            myRecord.CardCode = myCard.Code;
                            myRecord.CardName = myCard.EnglishName;
                            myRecord.CardExternal = myCard.ReceivablesAccountingCard;
                            myRecord.BillToName = myCard.EnglishName;
                        }

                        Contact myContact = allContacts.Where(d => d.Id == invoice.CreatedByUserId).FirstOrDefault();
                        if (myContact != null)
                        {
                            myRecord.CreatedByUser = myContact.EnglishName;
                        }

                        if (!string.IsNullOrEmpty(invoice.BranchId))
                        {
                            Branch myBranch = allBranchs.Where(d => d.Id == invoice.BranchId).FirstOrDefault();
                            if (myBranch != null)
                            {
                                myRecord.BranchCode = myBranch.Code;
                                myRecord.BranchName = myBranch.EnglishName;
                                myRecord.BranchLocalName = myBranch.LocalName;
                                myRecord.BranchExternalId = myBranch.ExternalId;
                            }
                        }

                        if (!string.IsNullOrEmpty(myShipment.IncotermId))
                        {
                            Incoterm myIncoterm = allIncoterms.Where(d => d.Id == myShipment.IncotermId).FirstOrDefault();
                            if (myIncoterm != null)
                            {
                                myRecord.IncotermCode = myIncoterm.Code;
                                myRecord.IncotermName = myIncoterm.Name;
                            }
                        }

                        myDataProvider.Shipments.Add(myRecord);
                        #endregion
                    }
                    #endregion
                }
            }

            return myDataProvider;
        }
        private ArchivoExportadoDataProvider LoadDataProvider_SplitByCharges()
        {
            ArchivoExportadoDataProvider myDataProvider = new ArchivoExportadoDataProvider();
            myDataProvider.Id = tenant;
            myDataProvider.CurrencyCode = this.SelectedCurrencyCode;
            myDataProvider.IncludeDraftInvoices = this.IncludeDraftInvoices ? "Yes" : "No";
            myDataProvider.IncludeEstimations = this.IncludeEstimations ? "Yes" : "No";
            myDataProvider.From = this.GetDateString(this.FromDate);
            myDataProvider.To = this.GetDateString(this.ToDate);
            myDataProvider.Shipments = new List<ArchivoExportadoShipmentItem>();

            IQueryable<ShipmentDataView> iQueryable_Shipments = this.GetIQueryableShipments();

            if (this.IncludeCancelledShipments)
            {

            }
            else
            {
                iQueryable_Shipments = iQueryable_Shipments.Where(d => !d.IsCancelled);
            }
            
            List<ShipmentDataView> allShipments = iQueryable_Shipments.ToList();

            if (allShipments.Count > 0)
            {
                #region Data
                List<string> allShipmentsIds = allShipments.Select(s => s.Id).ToList();

                IQueryable<APInvoice> iQueryable_APInvoices = (from d in myInvoiceContext.APInvoiceEntities.Include("APInvoice")
                                                               where d.Tenant == tenant
                                                               && allShipmentsIds.Contains(d.EntityId)
                                                               select d.APInvoice);

                IQueryable<ARInvoice> iQueryable_ARInvoices = (from d in myInvoiceContext.ARInvoiceEntities.Include("ARInvoice")
                                                               where d.Tenant == tenant
                                                               && allShipmentsIds.Contains(d.EntityId)
                                                               select d.ARInvoice);


                List<ShipmentInvoice> allShipmentsAPInvoices = (from d in myInvoiceContext.APInvoiceEntities
                                                                where d.Tenant == tenant
                                                                && allShipmentsIds.Contains(d.EntityId)
                                                                group d by new { d.APInvoiceId, d.EntityId } into g
                                                                select new ShipmentInvoice()
                                                                {
                                                                    InvoiceId = g.Key.APInvoiceId,
                                                                    ShipmentId = g.Key.EntityId
                                                                }).ToList();

                List<ShipmentInvoice> allShipmentsARInvoices = (from d in myInvoiceContext.ARInvoiceEntities
                                                                where d.Tenant == tenant
                                                                && allShipmentsIds.Contains(d.EntityId)
                                                                group d by new { d.ARInvoiceId, d.EntityId } into g
                                                                select new ShipmentInvoice()
                                                                {
                                                                    InvoiceId = g.Key.ARInvoiceId,
                                                                    ShipmentId = g.Key.EntityId
                                                                }).ToList();

                List<ChargeTypeGroupClass> allPayablesData = new List<ChargeTypeGroupClass>();
                List<ChargeTypeGroupClass> allReceivablesData = new List<ChargeTypeGroupClass>();
                if (this.IncludeEstimations)
                {
                    allPayablesData
                        = (from d in myShipmentsContext.ShipmentPayables
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

                    allReceivablesData
                        = (from d in myShipmentsContext.ShipmentReceivables
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

                iQueryable_APInvoices = iQueryable_APInvoices.Where(d => d.StatusCode != "VD");
                iQueryable_ARInvoices = iQueryable_ARInvoices.Where(d => d.StatusCode != "VD" && d.StatusCode != "LL");

                if (!this.IncludeDraftInvoices)
                {
                    iQueryable_ARInvoices = iQueryable_ARInvoices.Where(d => d.StatusCode != "DR");
                }

                List<APInvoice> allAPInvoices = iQueryable_APInvoices.ToList();
                List<ARInvoice> allARInvoices = iQueryable_ARInvoices.ToList();

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
                    allCards = this.GetCards(allAPInvoices, allARInvoices, allPayablesData);
                    allContacts = this.GetContacts(allAPInvoices, allARInvoices);
                    allBranchs = (from d in myCommonContext.Branches where d.Tenant == tenant select d).ToList();
                    allCurrencies = (from d in myCommonContext.Currencies where d.Tenant == tenant select d).ToList();
                    allIncoterms = (from d in myCommonContext.Incoterms where d.Tenant == tenant select d).ToList();

                    if (allAPInvoices.Count > 0)
                    {
                        List<string> allAPInvoicesIds = allAPInvoices.Select(s => s.Id).ToList();

                        allAPInvoiceLinesData
                            = (from d in myInvoiceContext.APInvoiceLines
                               where d.Tenant == tenant
                               && allAPInvoicesIds.Contains(d.APInvoiceId)
                               group d by new { d.APInvoiceId, d.EntityId, d.ChargesTypeId } into g
                               select new ChargeTypeGroupClass()
                               {
                                   InvoiceId = g.Key.APInvoiceId,
                                   ShipmentId = g.Key.EntityId,
                                   ChargesTypeId = g.Key.ChargesTypeId,
                                   AmountInLocal = g.Sum(s => s.LocalCurrencyAmount),
                                   AmountInProfit = g.Sum(s => s.ProfitCurrencyAmount),
                               }).ToList();
                    }

                    if (allARInvoices.Count > 0)
                    {
                        List<string> allARInvoicesIds = allARInvoices.Select(s => s.Id).ToList();

                        allARInvoiceLinesData
                            = (from d in myInvoiceContext.ARInvoiceLines
                               where d.Tenant == tenant
                               && allARInvoicesIds.Contains(d.ARInvoiceId)
                               group d by new { d.ARInvoiceId, d.EntityId, d.ChargesTypeId } into g
                               select new ChargeTypeGroupClass()
                               {
                                   InvoiceId = g.Key.ARInvoiceId,
                                   ShipmentId = g.Key.EntityId,
                                   ChargesTypeId = g.Key.ChargesTypeId,
                                   AmountInLocal = g.Sum(s => s.LocalCurrencyAmount),
                                   AmountInProfit = g.Sum(s => s.ProfitCurrencyAmount)
                               }).ToList();
                    }
                }
                #endregion

                Currency SelectedCurrency = allCurrencies.Where(d => d.Code == this.SelectedCurrencyCode).FirstOrDefault();
                if (SelectedCurrency != null)
                {
                    this.SelectedCurrencyId = SelectedCurrency.Id;
                }

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
                                    myRecord.CountryOfDestination = myShipment.MainCarriageFinalDestinationCountryName;

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
                                    myRecord.CountryOfDestination = myShipment.MainCarriageFinalDestinationCountryName;

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
                    
                    List<string> myAPInvoicesIds = allShipmentsAPInvoices.Where(d => d.ShipmentId == myShipment.Id).Select(s => s.InvoiceId).ToList();
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
                            
                            List<ChargeTypeGroupClass> lines_Grouped = allAPInvoiceLinesData.Where(d => d.InvoiceId == invoice.Id && d.ShipmentId == myShipment.Id).ToList();

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
                                myRecord.Payables = this.IsLocalCurrency ? item.AmountInLocal : item.AmountInProfit;
                                myRecord.InvoiceNumber = invoice.InvoiceNumber;
                                myRecord.InvoiceDate = invoice.InvoiceDate;
                                myRecord.InvoiceCurrencyRate = invoice.InvoiceCurrencyExchangeRate;
                                myRecord.CustomerExternalID = customerExternalID;
                                myRecord.Shipper = myShipment.ShipperName;
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
                                myRecord.CountryOfDestination = myShipment.MainCarriageFinalDestinationCountryName;

                                if (this.tenant == 1255)
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

                    List<string> myARInvoicesIds = allShipmentsARInvoices.Where(d => d.ShipmentId == myShipment.Id).Select(s => s.InvoiceId).ToList();
                    foreach (string id in myARInvoicesIds)
                    {
                        ARInvoice invoice = allARInvoices.Where(d => d.Id == id).FirstOrDefault();
                        if (invoice != null)
                        {
                            #region AR Invoices
                            Card myCard = allCards.Where(d => d.Id == invoice.BillToId).FirstOrDefault();
                            Contact myContact = allContacts.Where(d => d.Id == invoice.CreatedByUserId).FirstOrDefault();
                            Currency myCurrency = allCurrencies.Where(d => d.Id == invoice.InvoiceCurrencyId).FirstOrDefault();

                            List<ChargeTypeGroupClass> lines_Grouped = allARInvoiceLinesData.Where(d => d.InvoiceId == invoice.Id && d.ShipmentId == myShipment.Id).ToList();
                            
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
                                myRecord.Receivables = this.IsLocalCurrency ? item.AmountInLocal : item.AmountInProfit;
                                myRecord.InvoiceNumber = invoice.InvoiceNumber;
                                myRecord.InvoiceDate = invoice.InvoiceDate;
                                myRecord.InvoiceCurrencyRate = invoice.InvoiceCurrencyExchangeRate;
                                myRecord.CustomerExternalID = customerExternalID;
                                myRecord.Shipper = myShipment.ShipperName;
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
                                myRecord.CountryOfDestination = myShipment.MainCarriageFinalDestinationCountryName;

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

            return myDataProvider;
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

            return iQueryable_Shipments;
        }

        private List<Card> GetCards(List<APInvoice> allAPInvoices, List<ARInvoice> allARInvoices, List<ChargeTypeGroupClass> allPayablesData)
        {
            List<Card> myResult = new List<Card>();

            List<string> allIds = new List<string>();
            List<string> allIds_APInvoice = allAPInvoices.Where(d => d.VendorId != null).Select(s => s.VendorId).ToList();
            List<string> allIds_ARInvoice = allARInvoices.Where(d => d.BillToId != null).Select(s => s.BillToId).ToList();
            List<string> allIds_Payables = allPayablesData.Where(d => d.CardId != null).Select(s => s.CardId).ToList();

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

            myResult = (from d in myCommonContext.Cards
                        where d.Tenant == tenant
                        && allIds.Contains(d.Id)
                        select d).ToList();

            return myResult;
        }
        private List<Contact> GetContacts(List<APInvoice> allAPInvoices, List<ARInvoice> allARInvoices)
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
    }
}