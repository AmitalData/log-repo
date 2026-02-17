using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InvoiceModel;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.Repositories;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.ShipmentsModel.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Xml.Serialization;
using WebFreight.Web.DataProviders;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using System.Text.RegularExpressions;
using Simplog.Data.Helpers;
using Logitude.Server.Tools.Helpers;
using System.Reflection;
using Logitude.BL.DataContracts;
using Logitude.BL.InvoiceModel.EntityQueries;
using Logitude.BL.InvoiceModel.EntityPMs;

namespace WebFreight.Web.ReportsWebServices
{
    /// <summary>
    /// Summary description for APInvoiceWebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class APInvoiceWebService : System.Web.Services.WebService
    {
        IInvoiceContext invoiceCotnext;
        ICommonDataContext commonContext;
        IShipmentsContext shipmentsContext;
        APInvoiceRepository invoiceRepository;
        APInvoiceLineRepository invoiceLineRepository;
        APInvoiceTotalVATRepository invoiceTotalVATRepository;
        ShipmentRepository shipmentRepository;
        AddressRepository addressRepository;
        ShipmentQuery shipmentQuery;
        ShipmentPayableRepository payableRepository;
        CardRepository cardRepository;
        APInvoiceQuery invoiceQuery;
        int currentTenant;

        [WebMethod]
        public byte[] GetAPInvoiceData(string invoiceId, int tenant)
        {
            APInvoiceDataProvider invoicedataprovider = GetAPInvoiceDataProvider(invoiceId, tenant);

            XmlSerializer serializer = new XmlSerializer(typeof(APInvoiceDataProvider));
            MemoryStream memstream = new MemoryStream();
            serializer.Serialize(memstream, invoicedataprovider);
            memstream.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(memstream);
            string content = reader.ReadToEnd();
            byte[] bytearray = memstream.ToArray();
            return bytearray;
        }

        public APInvoiceDataProvider GetAPInvoiceDataProvider(string invoiceId, int tenant)
        {
            APInvoiceDataProvider invoiceDataProvider = new APInvoiceDataProvider();

            currentTenant = tenant;

            invoiceCotnext = InvoiceContext.GetContext(tenant);
            commonContext = CommonDataContext.GetContext(tenant);
            shipmentsContext = ShipmentsContext.GetContext(tenant);

            invoiceRepository = new APInvoiceRepository(invoiceCotnext);
            invoiceLineRepository = new APInvoiceLineRepository(invoiceCotnext);
            invoiceTotalVATRepository = new APInvoiceTotalVATRepository(invoiceCotnext);
            shipmentRepository = new ShipmentRepository(shipmentsContext);
            shipmentQuery = new ShipmentQuery(shipmentRepository);
            payableRepository = new ShipmentPayableRepository(shipmentsContext);
            addressRepository = new AddressRepository(commonContext);
            cardRepository = new CardRepository(commonContext);
            invoiceQuery = new APInvoiceQuery(invoiceRepository);

            APInvoicePM myAPInvoice = invoiceQuery.GetSinglePM(invoiceId, tenant);

            if (myAPInvoice != null)
            {
                if (myAPInvoice.IsMultipleEntities)
                {
                    invoiceDataProvider = GetMultipleAPInvoiceDataProvider(myAPInvoice);
                }

                else
                {
                    invoiceDataProvider = GetNormalAPInvoiceDataProvider(myAPInvoice);
                }
            }

            return invoiceDataProvider;
        }

        private APInvoiceDataProvider GetNormalAPInvoiceDataProvider(APInvoicePM invoice)
        {
            APInvoiceDataProvider invoiceDataProvider = new APInvoiceDataProvider();

            if (invoice != null)
            {
                Tenant myTenant = (from t in commonContext.Tenants where t.Id == currentTenant select t).FirstOrDefault();
                Card vendorCard = cardRepository.GetSingleCard(invoice.VendorId, currentTenant);
                ShipmentPM shipment = shipmentQuery.GetSinglePM(invoice.MainEntityId, currentTenant);
                
                #region Invoice

                invoiceDataProvider.InvoiceDate = invoice.InvoiceDate;
                invoiceDataProvider.DueDate = invoice.DueDate;
                invoiceDataProvider.Status = invoice.StatusName;
                invoiceDataProvider.InvoiceNumber = invoice.InvoiceNumber;
                invoiceDataProvider.InternalNumber = invoice.InternalNumber;
                invoiceDataProvider.ShipmentNumber = invoice.MainEntityReference;
                invoiceDataProvider.AccountingNumber = invoice.CreditAccount;
                invoiceDataProvider.HouseNumber = invoice.HouseNumber;
                invoiceDataProvider.Notes = invoice.InternalNotes;
                invoiceDataProvider.PaymentTerm = invoice.PaymentTermName;
                invoiceDataProvider.InvoiceCurrency = invoice.InvoiceCurrencyCode;

                double? invoiceSubTotals = invoice.SubTotalInInvoiceCurrency;
                double? invoiceAmount = invoice.AmountInInvoiceCurrency;

                if (invoiceSubTotals < 0)
                {
                    invoiceSubTotals = invoiceSubTotals * -1;
                }

                if (invoiceAmount < 0)
                {
                    invoiceAmount = invoiceAmount * -1;
                }

                invoiceDataProvider.SubTotalInvoiceCurr = invoiceSubTotals != null ? invoiceSubTotals.Value : 0;
                invoiceDataProvider.TotalInvoiceCurr = invoiceAmount != null ? invoiceAmount.Value : 0;

                invoiceDataProvider.ApprovedDate = invoice.ApprovedDate != null ? String.Format("{0:dd.MMM.yyyy}", invoice.ApprovedDate) : "";
                invoiceDataProvider.ApprovedDateAsDateFormat = invoice.ApprovedDate;
                invoiceDataProvider.ApprovedBy = invoice.ApprovedByUserName;
                invoiceDataProvider.CreatedByUser = invoice.CreatedByUserName;

                #endregion

                #region Tenant
                if (myTenant != null)
                {
                    invoiceDataProvider.VatNumber = myTenant.VatNumber;
                    invoiceDataProvider.BankDetails = myTenant.BankDetails;
                    invoiceDataProvider.Signature = myTenant.Signature;
                    invoiceDataProvider.Logo = DataProviders.General.GetLogo(myTenant.Id);
                    invoiceDataProvider.InvoiceSection1 = myTenant.InvoiceSection1;
                    invoiceDataProvider.InvoiceSection2 = myTenant.InvoiceSection2;
                }
                #endregion

                #region Vendor
                if (vendorCard != null)
                {
                    invoiceDataProvider.VendorNumber = vendorCard.Code;
                    invoiceDataProvider.VendorVatNumber = vendorCard.VatNumber;
                    invoiceDataProvider.IRSPlace = vendorCard.IRSPlace;
                    invoiceDataProvider.IRSNumber = vendorCard.IRSNumber;
                    invoiceDataProvider.VendorBankName = vendorCard.BankName;
                    invoiceDataProvider.VendorBankAddress = vendorCard.BankAddress;
                    invoiceDataProvider.VendorSwift = vendorCard.Swift;
                    invoiceDataProvider.VendorBankAccountNumber = vendorCard.AccountNumber;
                    invoiceDataProvider.VendoIBANNo = vendorCard.IBANNumber;
                    invoiceDataProvider.VendorName = vendorCard.EnglishName;

                    Address vendorAddress = addressRepository.GetMainAddressByCardId(invoice.VendorId, currentTenant);

                    if (vendorAddress != null)
                    {
                        string vendorLocalname = vendorCard.LocalName;
                        string vendorEnglishName = vendorCard.EnglishName;

                        if (vendorAddress.IsLocalLanguage && !string.IsNullOrEmpty(vendorLocalname))
                        {
                            invoiceDataProvider.VendorAddress = vendorLocalname + Environment.NewLine + DataProviders.General.GetAddress(vendorAddress);
                        }

                        else
                        {
                            invoiceDataProvider.VendorAddress = vendorEnglishName + Environment.NewLine + DataProviders.General.GetAddress(vendorAddress);
                        }
                    }
                }
                #endregion 

                #region IssuedByuser
                //User issuedByuser = (from user in commonContext.Users.Include("Contact") where user.Id == invoice.IssuedByUserId select user).FirstOrDefault();
                //if (issuedByuser != null)
                //{
                //    Contact contact = issuedByuser.Contact;
                //    if (contact != null)
                //    {
                //        invoiceDataProvider.IssuedByUser = contact.EnglishName != null ? contact.EnglishName : "";
                //    }

                //    invoice.PrintByUserId = issuedByuser.Id;
                //    invoice.PrintDate = TenantServerConfigration.GetCurrentDateTime(currentTenant);

                //    if (invoice.StatusCode == "AD")
                //    {
                //        invoice.IsPrinted = true;
                //    }

                //    invoiceRepository.Update(invoice);
                //    invoiceRepository.SubmitChanges();
                //}
                #endregion

                #region Shipment
                if (shipment != null)
                {
                    if (!string.IsNullOrEmpty(shipment.BranchId))
                    {
                        BranchRepository branchRepository = new BranchRepository(currentTenant);
                        Branch branch = branchRepository.GetSingleBranch(shipment.BranchId, currentTenant);

                        if (branch != null)
                        {
                            if (!string.IsNullOrEmpty(branch.AddressId))
                            {
                                Address branchAddress = addressRepository.GetSingleAddress(branch.AddressId, currentTenant);
                                invoiceDataProvider.BranchAddress = DataProviders.General.GetAddress(branchAddress);
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(shipment.ShipperId))
                    {
                        Card iCard = CardRepository.GetSingleCard(shipment.ShipperId, currentTenant, true);
                        if (iCard != null)
                        {
                            invoiceDataProvider.ShipperName = iCard.EnglishName;

                            if (!string.IsNullOrEmpty(shipment.ShipperAddressId))
                            {
                                Address iAddress = addressRepository.GetSingleAddress(shipment.ShipperAddressId, currentTenant);
                                if (iAddress != null)
                                {
                                    invoiceDataProvider.ShipperAddress = DataProviders.General.GetAddress(iAddress);
                                }
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(shipment.ConsigneeId))
                    {
                        Card iCard = CardRepository.GetSingleCard(shipment.ConsigneeId, currentTenant, true);
                        if (iCard != null)
                        {
                            invoiceDataProvider.ConsigneeName = iCard.EnglishName;

                            if (!string.IsNullOrEmpty(shipment.ConsigneeAddressId))
                            {
                                Address iAddress = addressRepository.GetSingleAddress(shipment.ConsigneeAddressId, currentTenant);
                                if (iAddress != null)
                                {
                                    invoiceDataProvider.ConsigneeAddress = DataProviders.General.GetAddress(iAddress);
                                }
                            }
                        }
                    }

                    invoiceDataProvider.MainCarriageCarrierName = shipment.MainCarriageCarrierName;
                    invoiceDataProvider.GrossWeight = shipment.GrossWeight;
                    invoiceDataProvider.ChargeableWeight = shipment.ChargeableWeight;
                    invoiceDataProvider.GrossWeightUnitCode = shipment.GrossWeightUnitCode;
                    invoiceDataProvider.ChargeableWeightUnitCode = shipment.ChargeableWeightUnitCode;
                    invoiceDataProvider.DescriptionOfGoods = shipment.DescriptionOfGoods;
                    invoiceDataProvider.MainCarriageMAWB = shipment.Master;

                    string carrierPrefix = null;
                    Card maincarriagecarrier = (from mc in commonContext.Cards.Include("Airline")
                                                where mc.Id == shipment.MainCarriageCarrierId
                                                select mc).FirstOrDefault();

                    if (maincarriagecarrier != null)
                    {
                        carrierPrefix = shipment.AirlinePrefix != null ? shipment.AirlinePrefix : "";
                    }

                    if (!string.IsNullOrEmpty(carrierPrefix) && !string.IsNullOrEmpty(invoiceDataProvider.MainCarriageMAWB))
                    {
                        invoiceDataProvider.MainCarriageMAWB = carrierPrefix + "-" + invoiceDataProvider.MainCarriageMAWB;
                    }

                    if (shipment.DirectionId == "D" && shipment.TransportModeId == "I")
                    {
                        Address fromAddress = addressRepository.GetSingleAddress(shipment.MainCarriageFromAddressId, currentTenant);
                        Address toAddress = addressRepository.GetSingleAddress(shipment.MainCarriageToAddressId, currentTenant);

                        if (fromAddress != null)
                        {
                            invoiceDataProvider.FromLocation = fromAddress.City + " " + (fromAddress.Country != null ? fromAddress.Country.Code : "");
                        }

                        if (toAddress != null)
                        {
                            invoiceDataProvider.ToLocation = toAddress.City + " " + (toAddress.Country != null ? toAddress.Country.Code : "");
                            invoiceDataProvider.FinalLocation = toAddress.City + " " + (toAddress.Country != null ? toAddress.Country.Code : "");
                        }
                    }
                    else
                    {
                        Port mainCarriageFromPort = (from a in commonContext.Ports.Include("Country")
                                                     where a.Id == shipment.MainCarriageFromPortId

                                                     select a).FirstOrDefault();
                        Port mainCarriageToPort = (from a in commonContext.Ports.Include("Country")
                                                   where a.Id == shipment.MainCarriageToPortId
                                                   select a).FirstOrDefault();

                        Port finalDistinationPort = (from a in commonContext.Ports.Include("Country")
                                                     where a.Id == shipment.MainCarriageFinalDestinationPortId
                                                     select a).FirstOrDefault();

                        if (mainCarriageFromPort != null)
                        {
                            invoiceDataProvider.FromLocation = mainCarriageFromPort.Code + " " + mainCarriageFromPort.EnglishName;
                        }

                        if (mainCarriageToPort != null)
                        {
                            invoiceDataProvider.ToLocation = mainCarriageToPort.Code + " " + mainCarriageToPort.EnglishName;
                        }

                        if (finalDistinationPort != null)
                        {
                            invoiceDataProvider.FinalLocation = finalDistinationPort != null ? (finalDistinationPort.Code + " " + finalDistinationPort.EnglishName) : "";
                        }
                    }

                    if (shipment.TransportModeId == "A")
                    {
                        invoiceDataProvider.MainCarriageCarrierNumber = shipment.MainCarriageCarrierCode + shipment.MainCarriageCarrierNumber;
                    }
                    else
                    {
                        invoiceDataProvider.MainCarriageCarrierNumber = shipment.MainCarriageCarrierNumber;
                    }

                    if (shipment.TransportModeId == "A")
                    {
                        invoiceDataProvider.MainCarriageMAWBLabel = "M.A.W.B";
                        invoiceDataProvider.MainCarriageCarrierNumberLabel = "Flight No.";
                        invoiceDataProvider.MainCarriageCarrierLabel = "Airline";
                        invoiceDataProvider.FinalLocationLabel = "Final Destination";
                        invoiceDataProvider.MainCarriageVesselLabel = "";
                        invoiceDataProvider.HouseNumberLabel = "HAWB";
                        invoiceDataProvider.ContainersLabel = "";
                    }

                    else if (shipment.TransportModeId == "O")
                    {
                        invoiceDataProvider.MainCarriageMAWBLabel = "MBL";
                        invoiceDataProvider.MainCarriageCarrierNumberLabel = "Voyage No.";
                        invoiceDataProvider.MainCarriageCarrierLabel = "Shipping line";
                        invoiceDataProvider.FinalLocationLabel = "Discharge Port";
                        invoiceDataProvider.MainCarriageVesselLabel = "Vessel";
                        invoiceDataProvider.HouseNumberLabel = "FBL";
                        invoiceDataProvider.ContainersLabel = "Containers";

                        Vessel maincarriagevessel = (from v in commonContext.Vessels where v.Id == shipment.MainCarriageVesselId select v).FirstOrDefault();

                        if (maincarriagevessel != null)
                        {
                            invoiceDataProvider.MainCarriageVesselName = maincarriagevessel.EnglishName;
                        }
                    }

                    else if (shipment.TransportModeId == "I")
                    {
                        invoiceDataProvider.MainCarriageMAWBLabel = "CMR";
                        invoiceDataProvider.MainCarriageCarrierNumberLabel = "Trucker No.";
                        invoiceDataProvider.MainCarriageCarrierLabel = "Trucker";
                        invoiceDataProvider.FinalLocationLabel = "Final Destination";
                        invoiceDataProvider.MainCarriageVesselLabel = "";
                        invoiceDataProvider.HouseNumberLabel = "House#";
                        invoiceDataProvider.ContainersLabel = "Trailers";
                    }

                    string alpha = "";
                    string num = "";
                    string alphaFormat = @"[^A-Za-z]*";
                    string numericFormat = @"[^0-9]*";
                    List<ShipmentPackage> shipmentPackagesList = shipmentsContext.ShipmentPackages.Where(sh => sh.ShipmentId == shipment.Id).ToList();
                    if (shipmentPackagesList.Count > 0)
                    {
                        string myTotalContainers = "";
                        int? myNumberofPackages = shipmentPackagesList.Sum(s => s.Quantity);

                        var grouped = (from a in shipmentPackagesList
                                       where a.PackageTypeId != null
                                       group a by a.PackageTypeId into g
                                       select new
                                       {
                                           PackageTypeId = g.Key,
                                           Quantity = g.Sum(s => s.Quantity)
                                       });

                        foreach (var item in grouped)
                        {
                            PackageType myPackageType = (from pa in commonContext.PackageTypes
                                                         where pa.Id == item.PackageTypeId
                                                         select pa).FirstOrDefault();

                            if (myPackageType != null)
                            {
                                if (myPackageType.IsContainer)
                                {
                                    alpha = Regex.Replace(myPackageType.Code, alphaFormat, string.Empty, RegexOptions.Compiled);
                                    num = Regex.Replace(myPackageType.Code, numericFormat, string.Empty, RegexOptions.Compiled);

                                    string itemText = item.Quantity.ToString() + " x " + num + "'" + alpha;
                                    myTotalContainers = string.IsNullOrEmpty(myTotalContainers) ? itemText : myTotalContainers + ", " + itemText;
                                }
                            }
                        }

                        string myContainersNumbersText = "";

                        foreach (ShipmentPackage item in shipmentPackagesList)
                        {
                            if (!string.IsNullOrEmpty(item.ContainerNumber))
                            {
                                PackageType myPackageType = (from pa in commonContext.PackageTypes where pa.Id == item.PackageTypeId select pa).FirstOrDefault();
                                if (myPackageType != null)
                                {
                                    if (myPackageType.IsContainer)
                                    {
                                        string type = !string.IsNullOrEmpty(myPackageType.PrintAs) ? myPackageType.PrintAs : myPackageType.Code;
                                        string itemText = item.ContainerNumber + " " + type;

                                        myContainersNumbersText = string.IsNullOrEmpty(myContainersNumbersText) ? item.ContainerNumber : myContainersNumbersText + "," + item.ContainerNumber;
                                    }
                                }
                            }
                        }

                        invoiceDataProvider.ContainersNumbersArray = myContainersNumbersText;
                        invoiceDataProvider.NumberofPackages = myNumberofPackages;
                    }

                }
                #endregion

                #region ReleasingAgent
                string myReleasingAgentId = shipment.ReleasingAgentId;
                string myReleasingAgentAddressId = shipment.ReleasingAgentAddressId;
                if (!string.IsNullOrEmpty(myReleasingAgentId))
                {
                    Card myPartnerCard = CardRepository.GetSingleCard(myReleasingAgentId, currentTenant, true);

                    if (myPartnerCard != null)
                    {
                        invoiceDataProvider.ReleasingAgentAddress = myPartnerCard.EnglishName != null ? myPartnerCard.EnglishName + Environment.NewLine : "";
                        invoiceDataProvider.ReleasingAgentName = myPartnerCard.EnglishName;
                        if (!string.IsNullOrEmpty(myReleasingAgentAddressId))
                        {
                            Address myPartnerAddress = addressRepository.GetSingleAddress(myReleasingAgentAddressId, currentTenant);

                            if (myPartnerAddress != null)
                            {
                                if (myPartnerAddress.IsLocalLanguage && !string.IsNullOrEmpty(myPartnerCard.LocalName))
                                {
                                    invoiceDataProvider.ReleasingAgentAddress = myPartnerCard.LocalName + Environment.NewLine;
                                }

                                invoiceDataProvider.ReleasingAgentAddress = invoiceDataProvider.ReleasingAgentAddress + DataProviders.General.GetAddress(myPartnerAddress);

                                if (myPartnerAddress.PhoneNumber != null || myPartnerAddress.FaxNumber != null)
                                {
                                    invoiceDataProvider.ReleasingAgentAddress = invoiceDataProvider.ReleasingAgentAddress + Environment.NewLine + (myPartnerAddress.PhoneNumber != null ? "Tel: " + myPartnerAddress.PhoneNumber + " " : "") + (myPartnerAddress.FaxNumber != null ? "Fax: " + myPartnerAddress.FaxNumber + " " : "");
                                }
                            }
                        }
                    }
                }
                #endregion 

                #region InvoiceLines

                invoiceDataProvider.APInvoiceLinesList = new List<APReportInvoiceLine>();
                List<APInvoiceLine> invoiceLines = invoiceLineRepository.GetInvoiceLinesByInvoiceId(invoice.Id, currentTenant).OrderBy(d => d.ChargesType.ViewOrder).ToList();

                foreach (APInvoiceLine invoiceline in invoiceLines)
                {
                    APReportInvoiceLine reportinvoiceline = new APReportInvoiceLine();

                    Currency foreigncurrency = (from f in commonContext.Currencies where f.Id == invoiceline.ForiegnCurrencyId select f).FirstOrDefault();
                    ChargesType chargesType = (from c in commonContext.ChargesTypes where c.Id == invoiceline.ChargesTypeId select c).FirstOrDefault();
                    VatType vatType = (from v in commonContext.VatTypes where v.Id == invoiceline.VatTypeId select v).FirstOrDefault();
                    ShipmentPayable payable = payableRepository.GetSingleShipmentPayable(invoiceline.EntityPayableId);

                    reportinvoiceline.ChargeTypeCode = chargesType == null ? "" : chargesType.Code;
                    reportinvoiceline.ChargeTypeName = invoiceline.Description != null ? invoiceline.Description : "";
                    reportinvoiceline.VatTypeName = vatType == null ? "" : vatType.EnglishName;
                    reportinvoiceline.VatTypePercentage = invoiceline.VatPercentage;
                    reportinvoiceline.ForeignCurrency = foreigncurrency != null ? foreigncurrency.Code : "";

                    reportinvoiceline.ExpectedAmount = payable == null ? 0 : payable.ExpectedAmount;
                    reportinvoiceline.OtherInvoicesAmount = payable == null ? 0 : (payable.AccountedAmount - invoiceline.ForiegnCurrencyAmount);
                    reportinvoiceline.ForeignAmount = MethodHelper.Round(invoiceline.ForiegnCurrencyAmount, 2);
                    reportinvoiceline.InvoiceAmount = MethodHelper.Round(invoiceline.InvoiceCurrencyAmount, 2);
                    reportinvoiceline.OpenAmount = payable == null ? 0 : payable.OpenAmount;

                    invoiceDataProvider.APInvoiceLinesList.Add(reportinvoiceline);
                }
                #endregion

                #region Total VAT

                invoiceDataProvider.APTotalVatList = new List<APTotalVat>();
                List<APInvoiceTotalVAT> totalVats = invoiceTotalVATRepository.GetInvoiceTotalVatsByInvoiceId(invoice.Id, currentTenant).ToList();

                foreach (APInvoiceTotalVAT item in totalVats)
                {
                    APTotalVat reportTotalVAT = new APTotalVat();

                    VatType vatType = (from vat in commonContext.VatTypes where vat.Id == item.VatTypeId select vat).FirstOrDefault();                    
                    List<VatTypePercentage> vattypepercentageList = (from percentage in commonContext.VatTypePercentages where percentage.VatTypeId == vatType.Id orderby percentage.FromDate descending select percentage).ToList();

                    reportTotalVAT.Type = vatType != null ? vatType.EnglishName : "";
                    reportTotalVAT.TotalVatAmountInInvoiceCurrency = item.InvoiceCurrencyVATAmount;

                    if (vattypepercentageList.Count > 0)
                    {
                        reportTotalVAT.Percentage = vattypepercentageList[0].Percentage;
                    }

                    invoiceDataProvider.APTotalVatList.Add(reportTotalVAT);
                }
                #endregion

                #region InvoiceDataProviderType

                Type invoiceDataProviderType = invoiceDataProvider.GetType();
                PropertyInfo[] properties = invoiceDataProviderType.GetProperties();
                foreach (PropertyInfo item in properties)
                {
                    try
                    {
                        if (item.Name != "APInvoiceLinesList")
                        {
                            if (item.GetValue(invoiceDataProvider, null) == null || item.GetValue(invoiceDataProvider, null).ToString() == "0" || item.GetValue(invoiceDataProvider, null).ToString() == "00.00")
                            {
                                item.SetValue(invoiceDataProvider, "", null);
                            }
                        }
                    }

                    catch
                    {

                    }
                }
                #endregion
            }

            return invoiceDataProvider;
        }

        private APInvoiceDataProvider GetMultipleAPInvoiceDataProvider(APInvoicePM invoice)
        {
            APInvoiceDataProvider invoiceDataProvider = new APInvoiceDataProvider();

            if (invoice != null)
            {
                Tenant myTenant = (from t in commonContext.Tenants where t.Id == currentTenant select t).FirstOrDefault();
                Card vendorCard = cardRepository.GetSingleCard(invoice.VendorId, currentTenant);
               
                #region Invoice

                invoiceDataProvider.InvoiceDate = invoice.InvoiceDate;
                invoiceDataProvider.DueDate = invoice.DueDate;
                invoiceDataProvider.Status = invoice.StatusName;
                invoiceDataProvider.InvoiceNumber = invoice.InvoiceNumber;
                invoiceDataProvider.InternalNumber = invoice.InternalNumber;
                invoiceDataProvider.ShipmentNumber = invoice.MainEntityReference;
                invoiceDataProvider.AccountingNumber = invoice.CreditAccount;
                invoiceDataProvider.HouseNumber = invoice.HouseNumber;
                invoiceDataProvider.Notes = invoice.InternalNotes;
                invoiceDataProvider.InvoiceCurrency = invoice.InvoiceCurrencyCode;
                invoiceDataProvider.PaymentTerm = invoice.PaymentTermName;

                double? invoiceSubTotals = invoice.SubTotalInInvoiceCurrency;
                double? invoiceAmount = invoice.AmountInInvoiceCurrency;

                if (invoiceSubTotals < 0)
                {
                    invoiceSubTotals = invoiceSubTotals * -1;
                }

                if (invoiceAmount < 0)
                {
                    invoiceAmount = invoiceAmount * -1;
                }

                invoiceDataProvider.SubTotalInvoiceCurr = invoiceSubTotals != null ? invoiceSubTotals.Value : 0;
                invoiceDataProvider.TotalInvoiceCurr = invoiceAmount != null ? invoiceAmount.Value : 0;

                invoiceDataProvider.ApprovedDate = invoice.ApprovedDate != null ? String.Format("{0:dd.MMM.yyyy}", invoice.ApprovedDate) : "";
                invoiceDataProvider.ApprovedDateAsDateFormat = invoice.ApprovedDate;
                invoiceDataProvider.ApprovedBy = invoice.ApprovedByUserName;
                invoiceDataProvider.CreatedByUser = invoice.CreatedByUserName;

                #endregion

                #region Tenant
                if (myTenant != null)
                {
                    invoiceDataProvider.VatNumber = myTenant.VatNumber;
                    invoiceDataProvider.BankDetails = myTenant.BankDetails;
                    invoiceDataProvider.Signature = myTenant.Signature;
                    invoiceDataProvider.Logo = DataProviders.General.GetLogo(myTenant.Id);
                    invoiceDataProvider.InvoiceSection1 = myTenant.InvoiceSection1;
                    invoiceDataProvider.InvoiceSection2 = myTenant.InvoiceSection2;
                }
                #endregion

                #region Vendor
                if (vendorCard != null)
                {
                    invoiceDataProvider.VendorNumber = vendorCard.Code;
                    invoiceDataProvider.VendorVatNumber = vendorCard.VatNumber;

                    Address vendorAddress = addressRepository.GetMainAddressByCardId(invoice.VendorId, currentTenant);

                    if (vendorAddress != null)
                    {
                        string vendorLocalname = vendorCard.LocalName;
                        string vendorEnglishName = vendorCard.EnglishName;

                        if (vendorAddress.IsLocalLanguage && !string.IsNullOrEmpty(vendorLocalname))
                        {
                            invoiceDataProvider.VendorAddress = vendorLocalname + Environment.NewLine + DataProviders.General.GetAddress(vendorAddress);
                        }

                        else
                        {
                            invoiceDataProvider.VendorAddress = vendorEnglishName + Environment.NewLine + DataProviders.General.GetAddress(vendorAddress);
                        }
                    }
                }
                #endregion 

                #region IssuedByuser
                //User issuedByuser = (from user in commonContext.Users.Include("Contact") where user.Id == invoice.IssuedByUserId select user).FirstOrDefault();
                //if (issuedByuser != null)
                //{
                //    Contact contact = issuedByuser.Contact;
                //    if (contact != null)
                //    {
                //        invoiceDataProvider.IssuedByUser = contact.EnglishName != null ? contact.EnglishName : "";
                //    }

                //    invoice.PrintByUserId = issuedByuser.Id;
                //    invoice.PrintDate = TenantServerConfigration.GetCurrentDateTime(currentTenant);

                //    if (invoice.StatusCode == "AD")
                //    {
                //        invoice.IsPrinted = true;
                //    }

                //    invoiceRepository.Update(invoice);
                //    invoiceRepository.SubmitChanges();
                //}
                #endregion

                #region Multiple Entities

                invoiceDataProvider.APInvoiceMultipleEntityList = new List<APInvoiceMultipleEntity>();

                if (invoice.InvoiceMultipleShipments.Count > 0)
                {
                    foreach (APInvoiceMultipleShipmentPM item in invoice.InvoiceMultipleShipments)
                    {
                        APInvoiceMultipleEntity singleRecord = new APInvoiceMultipleEntity()
                            {
                                MasterNumber = item.Master,
                                HouseNumber = item.House,
                                ShipmentNumber = item.ShipmentNumber,
                                PartnerName = item.PartnerName,
                                ExpectedAmount = item.ExpectedAmount,
                                OpenAmount = item.OpenAmount,
                                Total = item.SubTotalInInvoiceCurrency,
                                TotalVAT = item.TotalVATAmount,
                            };

                        invoiceDataProvider.APInvoiceMultipleEntityList.Add(singleRecord);
                    }
                }

                #endregion

                #region InvoiceDataProviderType

                Type invoiceDataProviderType = invoiceDataProvider.GetType();
                PropertyInfo[] properties = invoiceDataProviderType.GetProperties();
                foreach (PropertyInfo item in properties)
                {
                    try
                    {
                        if (item.Name != "APInvoiceLinesList")
                        {
                            if (item.GetValue(invoiceDataProvider, null) == null || item.GetValue(invoiceDataProvider, null).ToString() == "0" || item.GetValue(invoiceDataProvider, null).ToString() == "00.00")
                            {
                                item.SetValue(invoiceDataProvider, "", null);
                            }
                        }
                    }

                    catch
                    {

                    }
                }
                #endregion
            }

            return invoiceDataProvider;
        }
    }
}
