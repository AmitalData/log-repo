using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Helpers;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.QueueService;
using Logitude.Server.Tools.StorageService;
using Logitude.SystemLogs;
using Microsoft.Practices.Unity;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.InvoiceModel;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.Repositories;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;
using System.Xml.Serialization;

namespace Logitude.BL.InvoiceModel.EntityOtherServices
{
    public class ARInvoiceMessageHelper
    {
        private int tenant;
        private string filename;
        private List<ARInvoice> invoices;
        private string myVATableTempCard;
        private string myVATExemptTempCard;
        private string myAccountingSystemCode;
        private IInvoiceContext invoiceCotnext;
        private ICommonDataContext commonContext;
        private IShipmentsContext shipmentsContext;
        private MessageTransferHelper Helper;
        private bool isDropBox = false;
        private bool UsingFTP = false;
        private string FTPDetailId;
        public ARInvoiceMessageHelper(List<ARInvoice> invoices, string filename, int tenant,  bool isDropBox = false, bool isFTP = false)
        {
            this.tenant = tenant;
            this.filename = filename;
            this.invoices = invoices;
            this.invoiceCotnext = InvoiceContext.GetContext(tenant);
            this.commonContext = CommonDataContext.GetContext(tenant);
            this.shipmentsContext = ShipmentsContext.GetContext(tenant);
            this.Helper = new MessageTransferHelper(tenant, this.invoiceCotnext, this.commonContext, this.shipmentsContext);
            this.isDropBox = isDropBox;
            this.UsingFTP = isFTP;

            AccountingSettingRepository accountingSettingRepository = new AccountingSettingRepository(commonContext);
            AccountingSetting accountingSetting = accountingSettingRepository.GetSingleAccountSetting(tenant);
            if (accountingSetting != null)
            {
                myVATableTempCard = accountingSetting.ReceivableVATableTempCard;
                myVATExemptTempCard = accountingSetting.ReceivableVATExemptTempCard;
                myAccountingSystemCode = accountingSetting.AccountingSystemCode;
                FTPDetailId = accountingSetting.TransferFTPDetailId;
            }
        }
        public ARInvoiceMessageHelper(int tenant)
        {
            this.tenant = tenant;
            this.invoiceCotnext = InvoiceContext.GetContext(tenant);
            this.commonContext = CommonDataContext.GetContext(tenant);
            this.shipmentsContext = ShipmentsContext.GetContext(tenant);
            this.Helper = new MessageTransferHelper(tenant, this.invoiceCotnext, this.commonContext, this.shipmentsContext);

            AccountingSettingRepository accountingSettingRepository = new AccountingSettingRepository(commonContext);
            AccountingSetting accountingSetting = accountingSettingRepository.GetSingleAccountSetting(tenant);
            if (accountingSetting != null)
            {
                myVATableTempCard = accountingSetting.ReceivableVATableTempCard;
                myVATExemptTempCard = accountingSetting.ReceivableVATExemptTempCard;
                myAccountingSystemCode = accountingSetting.AccountingSystemCode;
            }
        }

        public void Transfer()
        {
            if (myAccountingSystemCode == "GI" || myAccountingSystemCode == "AI")
            {
                this.GetGenericInterfaceData();
            }

            else
            {
                this.GetOriginalTranferData();
            }
        }
        public void RebuildFile()
        {
            if (filename.ToLower().EndsWith(".xml"))
            {
                this.GetGenericInterfaceData();
            }

            else
            {
                this.GetOriginalTranferData();
            }
        }

        private void GetGenericInterfaceData()
        {
            ARInvoiceRoot log = new ARInvoiceRoot();
            log.Invoices = new List<ARInvoiceElement>();

            DateTime todayDate = TenantServerConfigration.GetCurrentDateTime(tenant);

            List<string> allInvoiceIds = invoices.Select(s => s.Id).ToList();
            List<string> allCardsIds = invoices.Select(s => s.BillToId).ToList();

            List<Card> allCards = (from d in commonContext.Cards
                                   where d.Tenant == tenant
                                   && allCardsIds.Contains(d.Id)
                                   select d).ToList();

            List<ARInvoiceLine> allInvoicesLines = (from d in invoiceCotnext.ARInvoiceLines.Include("ChargesType").Include("VatType")
                                                    where d.Tenant == tenant
                                                    && allInvoiceIds.Contains(d.ARInvoiceId)
                                                    select d).ToList();

            List<string> allChargesTypesIds = (from d in allInvoicesLines
                                               group d by d.ChargesTypeId into g
                                               select g.Key).ToList();

            List<ChargesExternalAccountsByProduct> allChargesExternalByProducts
                = (from f in commonContext.ChargesExternalAccountsByProducts
                   where f.Tenant == tenant
                   && allChargesTypesIds.Contains(f.ChargesTypeId)
                   select f).ToList();

            List<ARInvoiceTotalVAT> allInvoicesTotalVATs = (from d in invoiceCotnext.ARInvoiceTotalVATs.Include("VatType")
                                                    where d.Tenant == tenant
                                                    && allInvoiceIds.Contains(d.ARInvoiceId)
                                                    select d).ToList();

            List<SATPaymentMethod> allPaymentMethods = (from d in invoiceCotnext.SATPaymentMethods select d).ToList();

            CardExternalAccountsByProductRepository myCardExternalAccountsByProductRepository = new CardExternalAccountsByProductRepository(commonContext);
            MeasurementRepository measurementRepository = new MeasurementRepository(this.commonContext);
            AddressQuery addressQuery = new AddressQuery(new AddressRepository(this.commonContext));
            ShipmentQuery shipmentQuery = new ShipmentQuery(new ShipmentRepository(this.shipmentsContext));
            ShipmentReceivableRepository shipmentReceivableRepository = new ShipmentReceivableRepository(this.shipmentsContext);
            PrepaidCollectRepository prepaidCollectRepository = new PrepaidCollectRepository(tenant);
            VatTypePercentageRepository vatTypePercentageRepository = new VatTypePercentageRepository(commonContext);

            foreach (ARInvoice item in invoices)
            {
                ARInvoiceElement invoiceElement = new ARInvoiceElement();

                List<ARInvoiceTotalVAT> myTotalVATs = allInvoicesTotalVATs.Where(d => d.ARInvoiceId == item.Id).ToList();

                ShipmentPM shipment = null;
                if (!string.IsNullOrEmpty(item.MainEntityId))
                {
                    shipment = shipmentQuery.GetSinglePMWithoutComposition(item.MainEntityId, tenant);
                }

                #region Invoice
                invoiceElement.DocumentType = "ARINV";
                invoiceElement.InvoiceType = item.ARInvoiceTypeCode == "CD" ? "Credit" : "Debit";
                invoiceElement.InvoiceNumber = item.InvoiceNumber;
                invoiceElement.InvoiceDate = item.InvoiceDate;
                invoiceElement.InvoiceNotes = item.PrintNotes;
                invoiceElement.InvoiceCurrency = item.InvoiceCurrency.Code;
                invoiceElement.ShipmentNumber = item.MainEntityReference;
                invoiceElement.MasterReference = item.MasterNumber;
                invoiceElement.HouseReference = item.HouseNumber;
                invoiceElement.CustomerReference = item.CustomerRef;
                invoiceElement.DueDate = item.DueDate;
                invoiceElement.InvoiceTotalLineAmount = item.SubTotalInInvoiceCurrency == null ? 0 : (decimal)item.SubTotalInInvoiceCurrency;
                invoiceElement.TotalInvoiceInInvoiceCurrency = item.AmountInInvoiceCurrency == null ? 0 : (decimal)item.AmountInInvoiceCurrency;
                invoiceElement.RateInvoiceCurrency = item.InvoiceCurrencyExchangeRate == null ? 0 : (decimal)item.InvoiceCurrencyExchangeRate;
                invoiceElement.VATNumber = item.VatNumber;
                invoiceElement.PaymentTermExternalId = item.PaymentTermExternalId;
                invoiceElement.SATPaymentMethodCode = item.SATPaymentMethodCode;
                invoiceElement.SATPaymentMethodName = allPaymentMethods.Where(a => a.Code == item.SATPaymentMethodCode).Select(a => a.Name).FirstOrDefault();
                invoiceElement.Tenant = tenant;
                invoiceElement.Intercompany = item.Intercompany ? "True" : "False";
                invoiceElement.TotalTaxAmountInInvoiceCurrency = (decimal)myTotalVATs.Sum(s => s.InvoiceCurrencyVATAmount);

                if (item.IsConsolidationInvoice)
                {
                    invoiceElement.ShipmentNumber = "Consolidation invoice";
                }
                #endregion

                #region Card

                invoiceElement.Card = new CardElement();

                Card myPartnerCard = allCards.Where(d => d.Id == item.BillToId).FirstOrDefault();

                if (myPartnerCard != null)
                {
                    string myAccountingNumber = this.GetDebitAccount(item);

                    invoiceElement.Card.Code = myPartnerCard.Code;
                    invoiceElement.Card.AccountingCard = myAccountingNumber;
                    invoiceElement.Card.IntercompanyCode = myPartnerCard.ExternalId2;
                    invoiceElement.Card.Name = myPartnerCard.EnglishName;
                    invoiceElement.Card.VATNumber = myPartnerCard.VatNumber;

                    AddressPM address = addressQuery.GetSingleAddressPM(item.BillToAddressId, tenant, true);
                    if (address != null)
                    {
                        invoiceElement.Card.Address1 = address.Address1;
                        invoiceElement.Card.Address2 = address.Address2;
                        invoiceElement.Card.City = address.City;
                        invoiceElement.Card.ZipCode = address.ZipCode;
                        invoiceElement.Card.Country = address.CountryEnglishName;
                        invoiceElement.Card.CountryCode = address.CountryCode;
                        invoiceElement.Card.State = address.StateEnglishName;
                    }

                    invoiceElement.Card.Advanced = new CardAdvancedElement();
                    invoiceElement.Card.Advanced.BusinessArea = "";
                    invoiceElement.Card.Advanced.GLAccount = "";
                    invoiceElement.Card.Advanced.CostCenter = "";

                    if (myPartnerCard.ExternalAccountingBusinessArea != null)
                    {
                        invoiceElement.Card.Advanced.BusinessArea = myPartnerCard.ExternalAccountingBusinessArea;
                    }

                    if (shipment != null)
                    {
                        string myShipmentProductCode = shipment.ProductCode;
                        if (!string.IsNullOrEmpty(myShipmentProductCode))
                        {
                            CardExternalAccountsByProduct myCardExternal = myCardExternalAccountsByProductRepository.GetSingle(myPartnerCard.Id, myShipmentProductCode, tenant);
                            if (myCardExternal != null)
                            {
                                if (myCardExternal.GLAccount != null)
                                {
                                    invoiceElement.Card.Advanced.GLAccount = myCardExternal.GLAccount;
                                }

                                if (myCardExternal.CostCenter != null)
                                {
                                    invoiceElement.Card.Advanced.CostCenter = myCardExternal.CostCenter;
                                }
                            }
                        }
                    }
                }
                #endregion

                #region Lines
                invoiceElement.InvoiceLines = new List<ARInvoiceLineElement>();
                List<ARInvoiceLine> dueVatLines = new List<ARInvoiceLine>();
                List<ARInvoiceLine> lines = allInvoicesLines.Where(d => d.ARInvoiceId == item.Id).OrderBy(o => o.ChargesType.ViewOrder).ToList();              
                List<string> allInvoiceLinesVATTypesIds = lines.Select(s => s.VatTypeId).Distinct().ToList();
                
                List<VATTypesGroup> allVATTypesGroups = (from d in commonContext.VATTypesGroups
                                                        where d.Tenant == tenant
                                                        && allInvoiceLinesVATTypesIds.Contains(d.GroupVATTypeId)
                                                        select d).ToList();

                int count = 1;
                foreach (ARInvoiceLine myline in lines)
                {
                    #region
                    ARInvoiceLineElement lineElement = new ARInvoiceLineElement()
                    {
                        LineNumber = count,
                        Description = myline.Description,
                        CreditAccount = this.GetGenericCreditAccount(myline),
                        InvoiceLineNote = myline.Notes,
                        AmountInOriginalCurrency = myline.ForiegnCurrencyAmount == null ? 0 : (decimal)myline.ForiegnCurrencyAmount,
                        AmountInInvoiceCurrency = myline.InvoiceCurrencyAmount == null ? 0 : (decimal)myline.InvoiceCurrencyAmount,
                        AmountInLocalCurrency = myline.LocalCurrencyAmount == null ? 0 : (decimal)myline.LocalCurrencyAmount,
                        ChargeTypeCode = myline.ChargesType == null ? "" : myline.ChargesType.Code,
                        TaxCode = myline.VatType == null ? "" : myline.VatType.Code,
                        UnitPrice = myline.UnitPrice == null ? 0 : (decimal)myline.UnitPrice,
                        Quantity = myline.Quantity == null ? 0 : (decimal)myline.Quantity,
                        IsMultiTAX = myline.VatType == null ? false : myline.VatType.IsMultiPercentage,
                        AppliesRegionalTax = myline.IsRegionalTax,
                    };
                    
                    if (string.IsNullOrEmpty(lineElement.OriginalCurrency))
                    {
                        Currency currency = CurrencyRepository.GetSingleCurrency(myline.ForiegnCurrencyId, tenant, true);
                        if (currency != null)
                        {
                            lineElement.OriginalCurrency = currency.Code;
                        }
                    }

                    if (string.IsNullOrEmpty(lineElement.ChargeTypeCode))
                    {
                        ChargesType chargesType = ChargesTypeRepository.GetSingleChargesType(myline.ChargesTypeId, tenant, true);
                        if (chargesType != null)
                        {
                            lineElement.ChargeTypeCode = chargesType.Code;
                        }
                    }

                    if (string.IsNullOrEmpty(lineElement.MeasurementCode))
                    {
                        Measurement myMeasurement = measurementRepository.GetSingleMeasurement(myline.MeasurementId, tenant);
                        if (myMeasurement != null)
                        {
                            lineElement.MeasurementCode = myMeasurement.Code;
                        }
                    }
                    
                    if (!string.IsNullOrEmpty(myline.PrepaidCollectId))
                    {
                        PrepaidCollect prepaidCollect = prepaidCollectRepository.GetSinglePrepaidCollect(myline.PrepaidCollectId);
                        if (prepaidCollect != null)
                        {
                            lineElement.PrepaidCollect = prepaidCollect.Name;
                        }
                    }

                    lineElement.Advanced = new LineAdvancedElement();
                    lineElement.Advanced.GLAccount = "";
                    lineElement.Advanced.CostCenter = "";
                    if (shipment != null)
                    {
                        string myShipmentProductCode = shipment.ProductCode;

                        if (!string.IsNullOrEmpty(myShipmentProductCode))
                        {
                            if (!string.IsNullOrEmpty(myline.ChargesTypeId))
                            {
                                ChargesExternalAccountsByProduct myExternal = allChargesExternalByProducts.Where(d => d.ChargesTypeId == myline.ChargesTypeId && d.ProductTypeCode == myShipmentProductCode).FirstOrDefault();
                                if (myExternal != null)
                                {
                                    if (myExternal.ReceivablesGLAccount != null)
                                    {
                                        lineElement.Advanced.GLAccount = myExternal.ReceivablesGLAccount;
                                    }

                                    if (myExternal.ReceivablesCostCenter != null)
                                    {
                                        lineElement.Advanced.CostCenter = myExternal.ReceivablesCostCenter;
                                    }
                                }
                            }
                        }
                    }

                    lineElement.TaxDetails = new List<LineTaxDetailsElement>();
                    VatType lineVatType = VatTypeRepository.GetSingleVatType(myline.VatTypeId, item.Tenant, true);

                    if (lineVatType != null)
                    {
                        if (string.IsNullOrEmpty(lineElement.TaxCode))
                        {
                            lineElement.TaxCode = lineVatType.Code;
                        }

                        if (!lineVatType.IsMultiPercentage)
                        {
                            ARInvoiceTotalVAT totalVAT = myTotalVATs.Where(d => d.VatTypeId == lineVatType.Id).FirstOrDefault();
                            lineElement.TaxPercentage = totalVAT.VatPercent == null ? 0 : (decimal)totalVAT.VatPercent;
                            lineElement.VATExternalId = totalVAT.ExternalVATCard;

                            lineElement.TaxDetails.Add(new LineTaxDetailsElement()
                            {
                                TaxCode = lineVatType.Code,
                                TaxPercentage = totalVAT.VatPercent == null ? 0 : (decimal)totalVAT.VatPercent,
                                VATExternalId = totalVAT.ExternalVATCard
                            });
                        }

                        else
                        {
                            double multiVatPercentage = 0;
                            List<VATTypesGroup> myVATTypesGroups = allVATTypesGroups.Where(d => d.GroupVATTypeId == lineVatType.Id).ToList();

                            foreach (VATTypesGroup vATGroupItem in myVATTypesGroups)
                            {
                                ARInvoiceTotalVAT totalVAT = myTotalVATs.Where(d => d.VatTypeId == vATGroupItem.SingleVATTypeId).FirstOrDefault();
                                VatType lineSingleVatType = VatTypeRepository.GetSingleVatType(vATGroupItem.SingleVATTypeId, item.Tenant, true);

                                if (totalVAT.VatPercent != null)
                                {
                                    multiVatPercentage += totalVAT.VatPercent.Value;
                                }

                                lineElement.TaxDetails.Add(new LineTaxDetailsElement()
                                {
                                    TaxCode = lineSingleVatType.Code,
                                    TaxPercentage = totalVAT.VatPercent == null ? 0 : (decimal)totalVAT.VatPercent,
                                    VATExternalId = totalVAT.ExternalVATCard
                                });
                            }

                            lineElement.TaxPercentage = (decimal)multiVatPercentage;
                        }                       
                    }
                    
                    invoiceElement.InvoiceLines.Add(lineElement);
                    count++;

                    if (myline.VatPercentage != 0 && myline.VatPercentage != null)
                    {
                        dueVatLines.Add(myline);
                    }

                    #endregion                    
                }
                
                //if (dueVatLines.Count != 0)
                //{
                //    foreach (ARInvoiceLine myline in dueVatLines)
                //    {
                //        invoiceElement.TotalTaxAmountInInvoiceCurrency += (decimal)((myline.InvoiceCurrencyAmount != null ? myline.InvoiceCurrencyAmount : 0) * (myline.VatPercentage != null ? (myline.VatPercentage / 100) : 0)).Value;
                //    }
                //}

                //else
                //{
                //    invoiceElement.TotalTaxAmountInInvoiceCurrency = 0;
                //}
                #endregion

                #region Shipment
                if (shipment != null)
                {
                    invoiceElement.ETA = shipment.MainCarriageETA;
                    invoiceElement.ETD = shipment.MainCarriageETD;
                    invoiceElement.ATA = shipment.MainCarriageATA;
                    invoiceElement.ATD = shipment.MainCarriageATD;

                    #region ShipmentDetails: Advanced GI
                    if (myAccountingSystemCode == "AI")
                    {
                        ShipmentDeliveryQuery shipmentDeliveryQuery = new ShipmentDeliveryQuery(new ShipmentPickUpDeliveryRepository(this.shipmentsContext));
                        ShipmentPickUpQuery shipmentPickupQuery = new ShipmentPickUpQuery(new ShipmentPickUpDeliveryRepository(this.shipmentsContext));
                        ShipmentPackageQuery shipmentPackageQuery = new ShipmentPackageQuery(new ShipmentPackageRepository(this.shipmentsContext));

                        invoiceElement.ShipmentDetails = new ARShipmentDetailsElement();

                        #region General
                        invoiceElement.ShipmentDetails.Level = shipment.ShipmentLevelName;
                        invoiceElement.ShipmentDetails.TransportMode = shipment.TransportModeName;
                        invoiceElement.ShipmentDetails.Direction = shipment.DirectionName;
                        invoiceElement.ShipmentDetails.ShipmentNumber = shipment.ShipmentNumber;
                        invoiceElement.ShipmentDetails.MasterNumber = shipment.LongMaster;
                        invoiceElement.ShipmentDetails.HouseNumber = shipment.House;
                        invoiceElement.ShipmentDetails.AMSBL = shipment.AMSBL;
                        invoiceElement.ShipmentDetails.BookingNumber = shipment.BookingConfirmationNumber;
                        invoiceElement.ShipmentDetails.SalesMan = shipment.SalesmanUserName;
                        invoiceElement.ShipmentDetails.Branch = shipment.BranchName;
                        invoiceElement.ShipmentDetails.MoveTypeCode = shipment.MoveTypeCode;
                        invoiceElement.ShipmentDetails.MoveTypeName = shipment.MoveTypeName;
                        invoiceElement.ShipmentDetails.IncotermCode = shipment.IncotermCode;
                        invoiceElement.ShipmentDetails.IncotermName = shipment.IncotermName;
                        invoiceElement.ShipmentDetails.DescriptionOfGoods = shipment.DescriptionOfGoods;
                        #endregion

                        #region Partners

                        #region Shipper
                        if (!string.IsNullOrEmpty(shipment.ShipperId))
                        {
                            Card shipper = CardRepository.GetSingleCard(shipment.ShipperId, tenant, true);
                            AddressPM shipperAddress = addressQuery.GetSingleAddressPM(shipment.ShipperAddressId, tenant, true);

                            Contact shipperContact = null;
                            if (!string.IsNullOrEmpty(shipment.ShipperContactId))
                            {
                                shipperContact = ContactRepository.GetSingleContact(shipment.ShipperContactId, tenant, true);
                            }

                            if (shipper != null)
                            {
                                invoiceElement.ShipmentDetails.ShipperName = shipper.EnglishName;
                                invoiceElement.ShipmentDetails.ShipperRef1 = shipment.ShipperReference1;
                                invoiceElement.ShipmentDetails.ShipperRef2 = shipment.ShipperReference2;

                                if (shipperAddress != null)
                                {
                                    invoiceElement.ShipmentDetails.ShipperFullAddress = this.Helper.GetAddress(shipperAddress);
                                    invoiceElement.ShipmentDetails.ShipperAddress1 = shipperAddress.Address1;
                                    invoiceElement.ShipmentDetails.ShipperAddress2 = shipperAddress.Address2;
                                    invoiceElement.ShipmentDetails.ShipperCity = shipperAddress.City;
                                    invoiceElement.ShipmentDetails.ShipperCountryCode = shipperAddress.CountryCode;
                                    invoiceElement.ShipmentDetails.ShipperCountryName = shipperAddress.CountryEnglishName;
                                    invoiceElement.ShipmentDetails.ShipperStateCode = shipperAddress.StateCode;
                                    invoiceElement.ShipmentDetails.ShipperStateName = shipperAddress.StateEnglishName;
                                    invoiceElement.ShipmentDetails.ShipperZipCode = shipperAddress.ZipCode;
                                    invoiceElement.ShipmentDetails.ShipperTelephone = shipperAddress.PhoneNumber;
                                    invoiceElement.ShipmentDetails.ShipperFax = shipperAddress.FaxNumber;
                                }

                                if (shipperContact != null)
                                {
                                    invoiceElement.ShipmentDetails.ShipperContactName = shipperContact.EnglishName;
                                    invoiceElement.ShipmentDetails.ShipperContactEmail = shipperContact.Email;
                                    invoiceElement.ShipmentDetails.ShipperContactPhone = shipperContact.BusinessPhone;
                                    invoiceElement.ShipmentDetails.ShipperContactFax = shipperContact.Fax;
                                    invoiceElement.ShipmentDetails.ShipperContactPosition = shipperContact.Position;
                                }
                            }
                        }
                        #endregion

                        #region Consignee
                        if (!string.IsNullOrEmpty(shipment.ConsigneeId))
                        {
                            Card consignee = CardRepository.GetSingleCard(shipment.ConsigneeId, tenant, true);
                            AddressPM consigneeAddress = addressQuery.GetSingleAddressPM(shipment.ConsigneeAddressId, tenant, true);

                            Contact consigneeContact = null;
                            if (!string.IsNullOrEmpty(shipment.ConsigneeContactId))
                            {
                                consigneeContact = ContactRepository.GetSingleContact(shipment.ConsigneeContactId, tenant, true);
                            }

                            if (consignee != null)
                            {
                                invoiceElement.ShipmentDetails.ConsigneeName = consignee.EnglishName;
                                invoiceElement.ShipmentDetails.ConsigneeRef1 = shipment.ConsigneeReference1;
                                invoiceElement.ShipmentDetails.ConsigneeRef2 = shipment.ConsigneeReference2;

                                if (consigneeAddress != null)
                                {
                                    invoiceElement.ShipmentDetails.ConsigneeFullAddress = this.Helper.GetAddress(consigneeAddress);
                                    invoiceElement.ShipmentDetails.ConsigneeAddress1 = consigneeAddress.Address1;
                                    invoiceElement.ShipmentDetails.ConsigneeAddress2 = consigneeAddress.Address2;
                                    invoiceElement.ShipmentDetails.ConsigneeCity = consigneeAddress.City;
                                    invoiceElement.ShipmentDetails.ConsigneeCountryCode = consigneeAddress.CountryCode;
                                    invoiceElement.ShipmentDetails.ConsigneeCountryName = consigneeAddress.CountryEnglishName;
                                    invoiceElement.ShipmentDetails.ConsigneeStateCode = consigneeAddress.StateCode;
                                    invoiceElement.ShipmentDetails.ConsigneeStateName = consigneeAddress.StateEnglishName;
                                    invoiceElement.ShipmentDetails.ConsigneeZipCode = consigneeAddress.ZipCode;
                                    invoiceElement.ShipmentDetails.ConsigneeTelephone = consigneeAddress.PhoneNumber;
                                    invoiceElement.ShipmentDetails.ConsigneeFax = consigneeAddress.FaxNumber;
                                }

                                if (consigneeContact != null)
                                {
                                    invoiceElement.ShipmentDetails.ConsigneeContactName = consigneeContact.EnglishName;
                                    invoiceElement.ShipmentDetails.ConsigneeContactEmail = consigneeContact.Email;
                                    invoiceElement.ShipmentDetails.ConsigneeContactPhone = consigneeContact.BusinessPhone;
                                    invoiceElement.ShipmentDetails.ConsigneeContactFax = consigneeContact.Fax;
                                    invoiceElement.ShipmentDetails.ConsigneeContactPosition = consigneeContact.Position;
                                }
                            }
                        }
                        #endregion

                        #region Agent
                        string agentId = shipment.AgentId;
                        if (string.IsNullOrEmpty(agentId))
                        {
                            if (!string.IsNullOrEmpty(shipment.MasterShipmentDataId) && shipment.ShipmentLevelCode == "H")
                            {
                                ShipmentPM master = shipmentQuery.GetSinglePM(shipment.MasterShipmentDataId, tenant);
                                agentId = master.AgentId;
                            }
                        }

                        if (!string.IsNullOrEmpty(agentId))
                        {
                            Card agent = CardRepository.GetSingleCard(agentId, tenant, true);
                            AddressPM agentAddress = addressQuery.GetSingleAddressPM(shipment.AgentAddressId, tenant, true);

                            Contact agentContact = null;
                            if (!string.IsNullOrEmpty(shipment.AgentContactId))
                            {
                                agentContact = ContactRepository.GetSingleContact(shipment.AgentContactId, tenant, true);
                            }

                            if (agent != null)
                            {
                                invoiceElement.ShipmentDetails.AgentName = agent.EnglishName;
                                invoiceElement.ShipmentDetails.AgentRef1 = shipment.AgentReference1;
                                invoiceElement.ShipmentDetails.AgentRef2 = shipment.AgentReference2;

                                if (agentAddress != null)
                                {
                                    invoiceElement.ShipmentDetails.AgentFullAddress = this.Helper.GetAddress(agentAddress);
                                    invoiceElement.ShipmentDetails.AgentAddress1 = agentAddress.Address1;
                                    invoiceElement.ShipmentDetails.AgentAddress2 = agentAddress.Address2;
                                    invoiceElement.ShipmentDetails.AgentCity = agentAddress.City;
                                    invoiceElement.ShipmentDetails.AgentCountryCode = agentAddress.CountryCode;
                                    invoiceElement.ShipmentDetails.AgentCountryName = agentAddress.CountryEnglishName;
                                    invoiceElement.ShipmentDetails.AgentStateCode = agentAddress.StateCode;
                                    invoiceElement.ShipmentDetails.AgentStateName = agentAddress.StateEnglishName;
                                    invoiceElement.ShipmentDetails.AgentZipCode = agentAddress.ZipCode;
                                    invoiceElement.ShipmentDetails.AgentTelephone = agentAddress.PhoneNumber;
                                    invoiceElement.ShipmentDetails.AgentFax = agentAddress.FaxNumber;
                                }

                                if (agentContact != null)
                                {
                                    invoiceElement.ShipmentDetails.AgentContactName = agentContact.EnglishName;
                                    invoiceElement.ShipmentDetails.AgentContactEmail = agentContact.Email;
                                    invoiceElement.ShipmentDetails.AgentContactPhone = agentContact.BusinessPhone;
                                    invoiceElement.ShipmentDetails.AgentContactFax = agentContact.Fax;
                                    invoiceElement.ShipmentDetails.AgentContactPosition = agentContact.Position;
                                }
                            }
                        }
                        #endregion

                        #region Notify 1
                        if (!string.IsNullOrEmpty(shipment.Notify1Id))
                        {
                            Card notify1 = CardRepository.GetSingleCard(shipment.Notify1Id, tenant, true);
                            AddressPM notify1Address = addressQuery.GetSingleAddressPM(shipment.Notify1AddressId, tenant, true);

                            Contact notify1Contact = null;
                            if (!string.IsNullOrEmpty(shipment.Notify1ContactId))
                            {
                                notify1Contact = ContactRepository.GetSingleContact(shipment.Notify1ContactId, tenant, true);
                            }

                            if (notify1 != null)
                            {
                                invoiceElement.ShipmentDetails.Notify1Name = notify1.EnglishName;

                                if (notify1Address != null)
                                {
                                    invoiceElement.ShipmentDetails.Notify1FullAddress = this.Helper.GetAddress(notify1Address);
                                    invoiceElement.ShipmentDetails.Notify1Address1 = notify1Address.Address1;
                                    invoiceElement.ShipmentDetails.Notify1Address2 = notify1Address.Address2;
                                    invoiceElement.ShipmentDetails.Notify1City = notify1Address.City;
                                    invoiceElement.ShipmentDetails.Notify1CountryCode = notify1Address.CountryCode;
                                    invoiceElement.ShipmentDetails.Notify1CountryName = notify1Address.CountryEnglishName;
                                    invoiceElement.ShipmentDetails.Notify1StateCode = notify1Address.StateCode;
                                    invoiceElement.ShipmentDetails.Notify1StateName = notify1Address.StateEnglishName;
                                    invoiceElement.ShipmentDetails.Notify1ZipCode = notify1Address.ZipCode;
                                    invoiceElement.ShipmentDetails.Notify1Telephone = notify1Address.PhoneNumber;
                                    invoiceElement.ShipmentDetails.Notify1Fax = notify1Address.FaxNumber;
                                }

                                if (notify1Contact != null)
                                {
                                    invoiceElement.ShipmentDetails.Notify1ContactName = notify1Contact.EnglishName;
                                    invoiceElement.ShipmentDetails.Notify1ContactEmail = notify1Contact.Email;
                                    invoiceElement.ShipmentDetails.Notify1ContactPhone = notify1Contact.BusinessPhone;
                                    invoiceElement.ShipmentDetails.Notify1ContactFax = notify1Contact.Fax;
                                    invoiceElement.ShipmentDetails.Notify1ContactPosition = notify1Contact.Position;
                                }
                            }
                        }
                        #endregion

                        #region Notify 2
                        if (!string.IsNullOrEmpty(shipment.Notify2Id))
                        {
                            Card notify2 = CardRepository.GetSingleCard(shipment.Notify2Id, tenant, true);
                            AddressPM notify2Address = addressQuery.GetSingleAddressPM(shipment.Notify2AddressId, tenant, true);

                            Contact notify2Contact = null;
                            if (!string.IsNullOrEmpty(shipment.Notify2ContactId))
                            {
                                notify2Contact = ContactRepository.GetSingleContact(shipment.Notify2ContactId, tenant, true);
                            }

                            if (notify2 != null)
                            {
                                invoiceElement.ShipmentDetails.Notify2Name = notify2.EnglishName;

                                if (notify2Address != null)
                                {
                                    invoiceElement.ShipmentDetails.Notify2FullAddress = this.Helper.GetAddress(notify2Address);
                                    invoiceElement.ShipmentDetails.Notify2Address1 = notify2Address.Address1;
                                    invoiceElement.ShipmentDetails.Notify2Address2 = notify2Address.Address2;
                                    invoiceElement.ShipmentDetails.Notify2City = notify2Address.City;
                                    invoiceElement.ShipmentDetails.Notify2CountryCode = notify2Address.CountryCode;
                                    invoiceElement.ShipmentDetails.Notify2CountryName = notify2Address.CountryEnglishName;
                                    invoiceElement.ShipmentDetails.Notify2StateCode = notify2Address.StateCode;
                                    invoiceElement.ShipmentDetails.Notify2StateName = notify2Address.StateEnglishName;
                                    invoiceElement.ShipmentDetails.Notify2ZipCode = notify2Address.ZipCode;
                                    invoiceElement.ShipmentDetails.Notify2Telephone = notify2Address.PhoneNumber;
                                    invoiceElement.ShipmentDetails.Notify2Fax = notify2Address.FaxNumber;
                                }

                                if (notify2Contact != null)
                                {
                                    invoiceElement.ShipmentDetails.Notify2ContactName = notify2Contact.EnglishName;
                                    invoiceElement.ShipmentDetails.Notify2ContactEmail = notify2Contact.Email;
                                    invoiceElement.ShipmentDetails.Notify2ContactPhone = notify2Contact.BusinessPhone;
                                    invoiceElement.ShipmentDetails.Notify2ContactFax = notify2Contact.Fax;
                                    invoiceElement.ShipmentDetails.Notify2ContactPosition = notify2Contact.Position;
                                }
                            }
                        }
                        #endregion

                        #endregion

                        #region Routing

                        bool isInlandDomesticShipment = (shipment.DirectionId == "D" && shipment.TransportModeId == "I");

                        ShipmentPickUpPM myPickup = shipmentPickupQuery.GetShipmentPickUpPMsByTenantAndShipment(shipment.Id, shipment.Tenant).Where(a => a.PickUpDeliveryNumber == shipment.ShipmentNumber + "/" + shipment.ShipmentPickUpIndex).FirstOrDefault();
                        ShipmentDeliveryPM myDelivery = shipmentDeliveryQuery.GetShipmentDeliveryPMsByTenantAndShipment(shipment.Id, shipment.Tenant).Where(a => a.PickUpDeliveryNumber == shipment.ShipmentNumber + "/" + shipment.ShipmentDeliveryIndex).FirstOrDefault();

                        //PickUpAddress
                        if (myPickup == null)
                        {
                            if (!string.IsNullOrEmpty(shipment.ShipperNotExporterAddressId))
                            {
                                AddressPM myAddress = addressQuery.GetSingleAddressPM(shipment.ShipperNotExporterAddressId, tenant, true);
                                if (myAddress != null)
                                {
                                    invoiceElement.ShipmentDetails.PickUpAddress = myAddress.City != null ? myAddress.City : "";
                                }
                            }
                            else if (!string.IsNullOrEmpty(shipment.PreCarriageFromPortName))
                            {
                                invoiceElement.ShipmentDetails.PickUpAddress = shipment.PreCarriageFromPortName;
                            }
                        }
                        else
                        {
                            invoiceElement.ShipmentDetails.PickUpAddress = this.Helper.GetPickUpDeliveryFromCityOrPortName(myPickup);
                            invoiceElement.ShipmentDetails.PickUpATD = myPickup.ATD;

                            //DeliveryAddress
                            if (myPickup.ToAddressId != null)
                            {
                                AddressPM toAddress = addressQuery.GetSingleAddressPM(myPickup.ToAddressId, tenant, true);

                                if (toAddress != null)
                                {
                                    invoiceElement.ShipmentDetails.DeliveryAddress = this.Helper.GetAddress(toAddress);
                                }
                            }
                            else
                            {
                                invoiceElement.ShipmentDetails.DeliveryAddress = myPickup.ToAddress != null ? myPickup.ToAddress : "";
                            }
                        }

                        if (myDelivery != null)
                        {
                            invoiceElement.ShipmentDetails.DeliveryATA = myDelivery.ATA;
                        }

                        #region PreCarriage
                        invoiceElement.ShipmentDetails.PreCarriageTransportMode = this.Helper.GetTransportModes(shipment.PreCarriageTransportModeId);
                        invoiceElement.ShipmentDetails.PreCarriageFromPortCode = shipment.PreCarriageFromPortCode;
                        invoiceElement.ShipmentDetails.PreCarriageFromPortName = shipment.PreCarriageFromPortName;
                        invoiceElement.ShipmentDetails.PreCarriageFromPortCountryCode = shipment.PreCarriageFromPortCountryCode;
                        invoiceElement.ShipmentDetails.PreCarriageFromPortCountryName = shipment.PreCarriageFromPortCountryName;
                        invoiceElement.ShipmentDetails.PreCarriageToPortCode = shipment.PreCarriageToPortCode;
                        invoiceElement.ShipmentDetails.PreCarriageToPortName = shipment.PreCarriageToPortName;
                        invoiceElement.ShipmentDetails.PreCarriageToPortCountryCode = shipment.PreCarriageToPortCountryCode;
                        invoiceElement.ShipmentDetails.PreCarriageToPortCountryName = shipment.PreCarriageToPortCountryName;
                        invoiceElement.ShipmentDetails.PreCarriageCarrierName = shipment.PreCarriageCarrierName;
                        invoiceElement.ShipmentDetails.PreCarriageCarrierNumber = shipment.PreCarriageCarrierNumber;
                        invoiceElement.ShipmentDetails.PreCarriageETD = shipment.PreCarriageETD;
                        invoiceElement.ShipmentDetails.PreCarriageETA = shipment.PreCarriageETA;
                        invoiceElement.ShipmentDetails.PreCarriageATD = shipment.PreCarriageATD;
                        invoiceElement.ShipmentDetails.PreCarriageATA = shipment.PreCarriageATA;
                        #endregion

                        #region OnCarriage
                        invoiceElement.ShipmentDetails.OnCarriageTransportMode = this.Helper.GetTransportModes(shipment.OnCarriageTransportModeId);
                        invoiceElement.ShipmentDetails.OnCarriageFromPortCode = shipment.OnCarriageFromPortCode;
                        invoiceElement.ShipmentDetails.OnCarriageFromPortName = shipment.OnCarriageFromPortName;
                        invoiceElement.ShipmentDetails.OnCarriageFromPortCountryCode = shipment.OnCarriageFromPortCountryCode;
                        invoiceElement.ShipmentDetails.OnCarriageFromPortCountryName = shipment.OnCarriageFromPortCountryName;
                        invoiceElement.ShipmentDetails.OnCarriageToPortCode = shipment.OnCarriageToPortCode;
                        invoiceElement.ShipmentDetails.OnCarriageToPortName = shipment.OnCarriageToPortName;
                        invoiceElement.ShipmentDetails.OnCarriageToPortCountryCode = shipment.OnCarriageToPortCountryCode;
                        invoiceElement.ShipmentDetails.OnCarriageToPortCountryName = shipment.OnCarriageToPortCountryName;
                        invoiceElement.ShipmentDetails.OnCarriageCarrierName = shipment.OnCarriageCarrierName;
                        invoiceElement.ShipmentDetails.OnCarriageCarrierNumber = shipment.OnCarriageCarrierNumber;
                        invoiceElement.ShipmentDetails.OnCarriageETD = shipment.OnCarriageETD;
                        invoiceElement.ShipmentDetails.OnCarriageETA = shipment.OnCarriageETA;
                        invoiceElement.ShipmentDetails.OnCarriageATD = shipment.OnCarriageATD;
                        invoiceElement.ShipmentDetails.OnCarriageATA = shipment.OnCarriageATA;
                        #endregion

                        #region PreForwarding
                        invoiceElement.ShipmentDetails.PreForwardingTransportMode = this.Helper.GetTransportModes(shipment.PreForwardingTransportModeId);
                        invoiceElement.ShipmentDetails.PreForwardingFromPortCode = shipment.PreForwardingFromPortCode;
                        invoiceElement.ShipmentDetails.PreForwardingFromPortName = shipment.PreForwardingFromPortName;
                        invoiceElement.ShipmentDetails.PreForwardingFromPortCountryCode = shipment.PreForwardingFromPortCountryCode;
                        invoiceElement.ShipmentDetails.PreForwardingFromPortCountryName = shipment.PreForwardingFromPortCountryName;
                        invoiceElement.ShipmentDetails.PreForwardingToPortCode = shipment.PreForwardingToPortCode;
                        invoiceElement.ShipmentDetails.PreForwardingToPortName = shipment.PreForwardingToPortName;
                        invoiceElement.ShipmentDetails.PreForwardingToPortCountryCode = shipment.PreForwardingToPortCountryCode;
                        invoiceElement.ShipmentDetails.PreForwardingToPortCountryName = shipment.PreForwardingToPortCountryName;
                        invoiceElement.ShipmentDetails.PreForwardingCarrierName = shipment.PreForwardingCarrierName;
                        invoiceElement.ShipmentDetails.PreForwardingCarrierNumber = shipment.PreForwardingCarrierNumber;
                        invoiceElement.ShipmentDetails.PreForwardingETD = shipment.PreForwardingETD;
                        invoiceElement.ShipmentDetails.PreForwardingETA = shipment.PreForwardingETA;
                        invoiceElement.ShipmentDetails.PreForwardingATD = shipment.PreForwardingATD;
                        invoiceElement.ShipmentDetails.PreForwardingATA = shipment.PreForwardingATA;
                        #endregion

                        #region OnForwarding
                        invoiceElement.ShipmentDetails.OnForwardingTransportMode = this.Helper.GetTransportModes(shipment.OnForwardingTransportModeId);
                        invoiceElement.ShipmentDetails.OnForwardingFromPortCode = shipment.OnForwardingFromPortCode;
                        invoiceElement.ShipmentDetails.OnForwardingFromPortName = shipment.OnForwardingFromPortName;
                        invoiceElement.ShipmentDetails.OnForwardingFromPortCountryCode = shipment.OnForwardingFromPortCountryCode;
                        invoiceElement.ShipmentDetails.OnForwardingFromPortCountryName = shipment.OnForwardingFromPortCountryName;
                        invoiceElement.ShipmentDetails.OnForwardingToPortCode = shipment.OnForwardingToPortCode;
                        invoiceElement.ShipmentDetails.OnForwardingToPortName = shipment.OnForwardingToPortName;
                        invoiceElement.ShipmentDetails.OnForwardingToPortCountryCode = shipment.OnForwardingToPortCountryCode;
                        invoiceElement.ShipmentDetails.OnForwardingToPortCountryName = shipment.OnForwardingToPortCountryName;
                        invoiceElement.ShipmentDetails.OnForwardingCarrierName = shipment.OnForwardingCarrierName;
                        invoiceElement.ShipmentDetails.OnForwardingCarrierNumber = shipment.OnForwardingCarrierNumber;
                        invoiceElement.ShipmentDetails.OnForwardingETD = shipment.OnForwardingETD;
                        invoiceElement.ShipmentDetails.OnForwardingETA = shipment.OnForwardingETA;
                        invoiceElement.ShipmentDetails.OnForwardingATD = shipment.OnForwardingATD;
                        invoiceElement.ShipmentDetails.OnForwardingATA = shipment.OnForwardingATA;
                        #endregion

                        #region MainCarriage
                        invoiceElement.ShipmentDetails.MainCarriageCarrierCode = shipment.MainCarriageCarrierCode;
                        invoiceElement.ShipmentDetails.MainCarriageCarrierName = shipment.MainCarriageCarrierName;
                        invoiceElement.ShipmentDetails.MainCarriageAirlinePrefix = shipment.MainCarriageCarrierPrefix;
                        invoiceElement.ShipmentDetails.MainCarriageCarrierNumber = shipment.MainCarriageCarrierNumber;
                        invoiceElement.ShipmentDetails.MainCarriageETD = shipment.MainCarriageETD;
                        invoiceElement.ShipmentDetails.MainCarriageATD = shipment.MainCarriageATD;
                        invoiceElement.ShipmentDetails.MainCarriageETA = shipment.MainCarriageETA;
                        invoiceElement.ShipmentDetails.MainCarriageATA = shipment.MainCarriageATA;
                        invoiceElement.ShipmentDetails.MainCarriageVessel = shipment.MainCarriageVesselName;
                        #endregion

                        #region Via1
                        invoiceElement.ShipmentDetails.Via1CarrierCode = shipment.Transshipment1CarrierCode;
                        invoiceElement.ShipmentDetails.Via1CarrierName = shipment.Transshipment1CarrierName;
                        invoiceElement.ShipmentDetails.Via1AirlinePrefix = shipment.Transshipment1CarrierPrefix;
                        invoiceElement.ShipmentDetails.Via1CarrierNumber = shipment.Transshipment1CarrierNumber;
                        invoiceElement.ShipmentDetails.Via1ETD = shipment.Transshipment1ETD;
                        invoiceElement.ShipmentDetails.Via1ATD = shipment.Transshipment1ATD;
                        invoiceElement.ShipmentDetails.Via1ETA = shipment.Transshipment1ETA;
                        invoiceElement.ShipmentDetails.Via1ATA = shipment.Transshipment1ATA;
                        invoiceElement.ShipmentDetails.Via1Vessel = shipment.Transshipment1VesselName;
                        #endregion

                        #region Via2
                        invoiceElement.ShipmentDetails.Via2CarrierCode = shipment.Transshipment2CarrierCode;
                        invoiceElement.ShipmentDetails.Via2CarrierName = shipment.Transshipment2CarrierName;
                        invoiceElement.ShipmentDetails.Via2AirlinePrefix = shipment.Transshipment2CarrierPrefix;
                        invoiceElement.ShipmentDetails.Via2CarrierNumber = shipment.Transshipment2CarrierNumber;
                        invoiceElement.ShipmentDetails.Via2ETD = shipment.Transshipment2ETD;
                        invoiceElement.ShipmentDetails.Via2ATD = shipment.Transshipment2ATD;
                        invoiceElement.ShipmentDetails.Via2ETA = shipment.Transshipment2ETA;
                        invoiceElement.ShipmentDetails.Via2ATA = shipment.Transshipment2ATA;
                        invoiceElement.ShipmentDetails.Via2Vessel = shipment.Transshipment2VesselName;
                        #endregion

                        #region Via3
                        invoiceElement.ShipmentDetails.Via3CarrierCode = shipment.Transshipment3CarrierCode;
                        invoiceElement.ShipmentDetails.Via3CarrierName = shipment.Transshipment3CarrierName;
                        invoiceElement.ShipmentDetails.Via3AirlinePrefix = shipment.Transshipment3CarrierPrefix;
                        invoiceElement.ShipmentDetails.Via3CarrierNumber = shipment.Transshipment3CarrierNumber;
                        invoiceElement.ShipmentDetails.Via3ETD = shipment.Transshipment3ETD;
                        invoiceElement.ShipmentDetails.Via3ATD = shipment.Transshipment3ATD;
                        invoiceElement.ShipmentDetails.Via3ETA = shipment.Transshipment3ETA;
                        invoiceElement.ShipmentDetails.Via3ATA = shipment.Transshipment3ATA;
                        invoiceElement.ShipmentDetails.Via3Vessel = shipment.Transshipment3VesselName;
                        #endregion

                        #region FromLocation
                        invoiceElement.ShipmentDetails.FromPortCode = shipment.FromPort;
                        invoiceElement.ShipmentDetails.FromPortName = shipment.FromPortName;
                        invoiceElement.ShipmentDetails.FromCountryCode = shipment.FromPortCountry;
                        invoiceElement.ShipmentDetails.FromCountryName = shipment.FromPortCountryName;

                        if (isInlandDomesticShipment)
                        {
                            AddressPM fromAddress = addressQuery.GetSingleAddressPM(shipment.MainCarriageFromAddressId, tenant, true);
                            invoiceElement.ShipmentDetails.FromAddress = this.Helper.GetAddress(fromAddress);
                        }
                        #endregion

                        #region ToLocation
                        invoiceElement.ShipmentDetails.ToPortCode = shipment.ToPort;
                        invoiceElement.ShipmentDetails.ToPortName = shipment.ToPortName;
                        invoiceElement.ShipmentDetails.ToCountryCode = shipment.ToPortCountry;
                        invoiceElement.ShipmentDetails.ToCountryName = shipment.ToPortCountryName;

                        if (isInlandDomesticShipment)
                        {
                            AddressPM toAddress = addressQuery.GetSingleAddressPM(shipment.MainCarriageToAddressId, tenant, true);
                            invoiceElement.ShipmentDetails.ToAddress = this.Helper.GetAddress(toAddress);
                        }
                        #endregion

                        #region FinalLocation
                        invoiceElement.ShipmentDetails.FinalPortCode = shipment.MainCarriageFinalDestinationPortCode;
                        invoiceElement.ShipmentDetails.FinalPortName = shipment.MainCarriageFinalDestinationPortName;
                        invoiceElement.ShipmentDetails.FinalCountryCode = shipment.MainCarriageFinalDestinationPortCountryCode;
                        invoiceElement.ShipmentDetails.FinalCountryName = shipment.MainCarriageFinalDestinationPortCountryName;

                        if (isInlandDomesticShipment)
                        {
                            AddressPM finalAddress = addressQuery.GetSingleAddressPM(shipment.MainCarriageToAddressId, tenant, true);
                            invoiceElement.ShipmentDetails.FinalAddress = this.Helper.GetAddress(finalAddress);
                        }
                        #endregion

                        #endregion

                        #region PackageDetails

                        invoiceElement.ShipmentDetails.PackageLines = new List<PackageLineElement>();

                        List<ShipmentPackagePM> shipmentPackagesList = shipmentPackageQuery.GetShipmentPackages(shipment.Id, shipment.ShipmentNumber, tenant);

                        invoiceElement.ShipmentDetails.TotalGrossWeight = shipment.GrossWeight;
                        invoiceElement.ShipmentDetails.GrossWeightUnitCode = shipment.GrossWeightUnitCode;
                        invoiceElement.ShipmentDetails.TotalChargeableWeight = shipment.ChargeableWeight;
                        invoiceElement.ShipmentDetails.ChargeableWeightUnitCode = shipment.ChargeableWeightUnitCode;
                        invoiceElement.ShipmentDetails.TotalVolume = shipment.Volume;
                        invoiceElement.ShipmentDetails.VolumeUnitCode = shipment.VolumeUnitCode;
                        invoiceElement.ShipmentDetails.TotalNumberOfPackage = shipment.NumberOfPackages;
                        invoiceElement.ShipmentDetails.TotalNumberOfContainers = shipment.NumberOfContainers;


                        StringBuilder str2 = new StringBuilder();
                        foreach (ShipmentPackagePM package in shipmentPackagesList)
                        {
                            PackageLineElement PackageElement = new PackageLineElement();

                            invoiceElement.ShipmentDetails.TotalContainers += (package.Quantity != null ? package.Quantity.ToString() : "") + ", ";

                            if (!string.IsNullOrEmpty(package.PackageTypeCode))
                            {
                                string alphaFormat = @"[^A-Za-z]*";
                                string numericFormat = @"[^0-9]*";
                                string alpha = Regex.Replace(package.PackageTypeCode, alphaFormat, string.Empty, RegexOptions.Compiled);
                                string num = Regex.Replace(package.PackageTypeCode, numericFormat, string.Empty, RegexOptions.Compiled);

                                invoiceElement.ShipmentDetails.TotalContainers += (package.Quantity != null ? package.Quantity.ToString() : "") + " x " + (num + "'" + alpha) + ", ";
                            }

                            string containerNo = package.ContainerNumber;
                            string type = !string.IsNullOrEmpty(package.PrintAs) ? package.PrintAs : package.PackageTypeCode;
                            str2.Append(containerNo);
                            str2.Append(' ');
                            str2.Append(type);
                            str2.Append(',');

                            PackageElement.Width = package.Width;
                            PackageElement.Height = package.Height;
                            PackageElement.Length = package.Length;
                            PackageElement.Dimensions = package.PrintAs + "_" + package.TEU.ToString();

                            if (package.MarksAndNumbers == null)
                            {
                                if (package.IsContainer)
                                {
                                    if (package.ContainerNumber != null)
                                    {
                                        if (package.ContainerNumber.Length > 10)
                                        {
                                            PackageElement.PackageMarksAndNumbers = !string.IsNullOrEmpty(package.ContainerNumber) ? package.ContainerNumber.Substring(0, 4) + " " + package.ContainerNumber.Substring(4, 6) + "/" + package.ContainerNumber.Substring(10, 1) + Environment.NewLine : "";
                                        }
                                        else
                                        {
                                            PackageElement.PackageMarksAndNumbers = package.ContainerNumber;
                                        }
                                    }

                                    if (!string.IsNullOrEmpty(package.ShipperSeal))
                                    {
                                        PackageElement.PackageMarksAndNumbers = PackageElement.PackageMarksAndNumbers + "SEAL:" + package.ShipperSeal + Environment.NewLine;
                                    }

                                    if (package.Tare != null)
                                    {
                                        PackageElement.PackageGrossWeight = PackageElement.PackageGrossWeight + Environment.NewLine + "TARE:" + package.Tare + (shipment.GrossWeightUnitCode != null ? shipment.GrossWeightUnitCode : "");
                                    }
                                }
                            }
                            else
                            {
                                PackageElement.PackageMarksAndNumbers = package.MarksAndNumbers;
                            }

                            if (shipment.ShipmentTypeId == "FCLD")
                            {
                                if (package.InsideShipmentPackages.Count() != 0)
                                {
                                    PackageElement.PackageQuantity = package.InsideShipmentPackages.Sum(d => d.Quantity);
                                }
                                else
                                {
                                    PackageElement.PackageQuantity = package.Quantity;
                                }
                            }
                            else
                            {
                                PackageElement.PackageQuantity = package.Quantity;
                            }

                            if (package.IsContainer)
                            {
                                PackageElement.PackageType = package.PrintAs;
                            }
                            else
                            {
                                PackageElement.PackageType = package.PackageTypeName;
                            }

                            if (!string.IsNullOrEmpty(package.Harmonize))
                            {
                                if (!string.IsNullOrEmpty(PackageElement.PackageDescriptionOfGoods))
                                {
                                    PackageElement.PackageDescriptionOfGoods += Environment.NewLine;
                                }
                                PackageElement.PackageDescriptionOfGoods += "HS Code: " + package.Harmonize;
                            }

                            PackageElement.PackageVolume = package.Volume;
                            PackageElement.ContainerNumber = package.ContainerNumber;
                            PackageElement.PackageTare = package.Tare;
                            PackageElement.PackageQuantityAndType = PackageElement.PackageQuantity + "x" + package.PrintAs;
                            PackageElement.IsDangerous = package.IsDangerous;
                            //PackageElement.InsidePackagesCount = package.Width;
                            //PackageElement.InsidePackageList = package.Width;
                            PackageElement.SealNumber = package.ShipperSeal;
                            PackageElement.ContainerSize = package.ContainerSize;

                            invoiceElement.ShipmentDetails.PackageLines.Add(PackageElement);
                        }

                        string str2_String = str2.ToString();
                        if (!string.IsNullOrEmpty(str2_String))
                        {
                            str2_String = str2_String.TrimEnd(',');
                        }

                        invoiceElement.ShipmentDetails.ContainersNumbersAndTypesArray = str2_String;

                        #endregion

                        #region Custom Fields

                        this.SetCustomFields("Shipment", tenant, shipment, invoiceElement.ShipmentDetails);
                        this.SetCustomFields("ARInvoice", tenant, item, invoiceElement.ShipmentDetails);

                        if (myPartnerCard != null)
                        {
                            if (myPartnerCard.Customer != null)
                            {
                                this.SetCustomFields("Customer", tenant, myPartnerCard.Customer, invoiceElement.ShipmentDetails);
                            }
                        }

                        #endregion
                    }
                    #endregion
                }
                #endregion

                #region TAX
                invoiceElement.TaxTotalsInInvoiceCurrency = new List<InvoiceTaxElement>();
                
                foreach (ARInvoiceTotalVAT myline in myTotalVATs)
                {
                    InvoiceTaxElement invoiceTaxElement = new InvoiceTaxElement()
                    {
                        TaxCode = myline.VatType == null ? null : myline.VatType.Code,
                        TaxPercentage = myline.VatPercent == null ? 0 : (decimal)myline.VatPercent.Value,
                        TaxAmount = myline.InvoiceCurrencyVATAmount == null ? 0 : (decimal)myline.InvoiceCurrencyVATAmount.Value,
                        IsRegionalTax = myline.IsRegionalTax,
                    };

                    invoiceElement.TaxTotalsInInvoiceCurrency.Add(invoiceTaxElement);
                }
                #endregion

                log.Invoices.Add(invoiceElement);
            }

            this.BuildXMLFile(log, tenant, filename);
        }
        private void GetOriginalTranferData()
        {
            string allInvoicesMessage = "";
            ARInvoiceLineRepository invoiceLineRepository = new ARInvoiceLineRepository(tenant);

            List<string> allInvoiceIds = invoices.Select(s => s.Id).ToList();
            IQueryable<ARInvoiceLine> iQueryableLines = invoiceLineRepository.GetInvoiceLinesForMessaging(tenant);
            List<ARInvoiceLine> allInvoicesLines = (from d in iQueryableLines where allInvoiceIds.Contains(d.ARInvoiceId) select d).ToList();

            foreach (ARInvoice invoice in invoices)
            {
                List<ARInvoiceLine> lines = allInvoicesLines.Where(d => d.ARInvoiceId == invoice.Id).OrderBy(o => o.ChargesType.ViewOrder).ToList();

                string invoiceMessage = this.Compute(invoice, lines);
                allInvoicesMessage += invoiceMessage;
            }

            if (!string.IsNullOrEmpty(allInvoicesMessage))
            {
                StringBuilder stringbuilder = new StringBuilder();
                Encoding encoding = new UTF8Encoding();
                stringbuilder.AppendLine(allInvoicesMessage);
                byte[] errordata = encoding.GetBytes(stringbuilder.ToString());
                string[] fileProps = filename.Split('.');

                Logitude.Server.Tools.BlobFileInfo fileInfo = new Logitude.Server.Tools.BlobFileInfo()
                {
                    FileName = fileProps[0],
                    Extension = fileProps.Length > 1 ? fileProps[1] : null,
                    Tenant = tenant,
                    FileSize = errordata.Length,
                    HasExternalContainer = true,
                    ExternalContainerName = "tenant" + tenant
                };

                Logitude.Server.Tools.StorageService.IBlobService storageservice = Logitude.Server.Tools.ContainerAccessor.Container.Resolve(typeof(Logitude.Server.Tools.StorageService.IBlobService), "StorageService", new ParameterOverride("", 1)) as Logitude.Server.Tools.StorageService.IBlobService;
                storageservice.Write(errordata, fileInfo);
            }
        }

        private string Compute(ARInvoice invoice, List<ARInvoiceLine> lines)
        {
            List<ARInvoiceLine> noVatLines = new List<ARInvoiceLine>();
            List<ARInvoiceLine> dueVatLines = new List<ARInvoiceLine>();
            
            foreach (ARInvoiceLine invoiceline in lines)
            {
                if (invoiceline.VatPercentage == 0 || invoiceline.VatPercentage == null)
                {
                    noVatLines.Add(invoiceline);
                }
                else
                {
                    dueVatLines.Add(invoiceline);
                }
            }

            Message dueVATLine = null;
            if (dueVatLines.Count != 0)
            {
                dueVATLine = this.Step_1_VAT(invoice, dueVatLines);
            }

            Message noVatLine = null;
            if (myAccountingSystemCode != "RH")
            {
                if (noVatLines.Count != 0)
                {
                    noVatLine = this.Step_1_noVAT(invoice, noVatLines);
                }
            }

            List<Message> lines_VAT = this.Step_2_VAT(invoice, dueVatLines);
            List<Message> lines_NOVAT = this.Step_2_NOVAT(invoice, noVatLines);

            List<Message> finalList = new List<Message>();

            if (dueVATLine != null)
            {
                finalList.Add(dueVATLine);
            }

            if (noVatLine != null)
            {
                finalList.Add(noVatLine);
            }

            for (int i = 0; i < lines_VAT.Count; i++)
            {
                if (lines_VAT[i] != null)
                {
                    finalList.Add(lines_VAT[i]);
                }
            }

            for (int i = 0; i < lines_NOVAT.Count; i++)
            {
                if (lines_NOVAT[i] != null)
                {
                    finalList.Add(lines_NOVAT[i]);
                }
            }

            StringBuilder main = new StringBuilder();

            for (int i = 0; i < finalList.Count; i++)
            {
                main.AppendLine(BuildFile(finalList[i], invoice));
            }

            return main.ToString();
        }
        private string BuildFile(Message message, ARInvoice invoice)
        {
            StringBuilder str = new StringBuilder(450);

            if (!string.IsNullOrEmpty(message.TransactionTypeCode)) { str.Append(message.TransactionTypeCode.PadRight(4)); }
            else { str.Append(' ', 4); }

            if (message.Reference1.Length < 9) { str.Append(message.Reference1.PadLeft(8)).Append(' '); }
            else { str.Append(message.Reference1.Substring(0, 8)).Append(' '); }

            if (message.Reference2 != null)
            {
                if (message.Reference2.Length < 8)
                {
                    str.Append(message.Reference2.PadLeft(7)).Append(' ');
                }
                else
                {
                    str.Append(message.Reference2.Substring(0, 7)).Append(' ');
                }
            }
            else { str.Append(' ', 8); }

            str.Append(String.Format("{0:dd/MM/yy}", message.TransactionDate)).EnsureCapacity(10);

            str.Append(String.Format("{0:dd/MM/yy}", message.ValueDate)).EnsureCapacity(10);

            str.Append(' ', 5); //message.costcode

            str.Append(message.CurrencyCode.PadRight(5));

            if (message.Details != null)
            {
                if (message.Details.Length < 50)
                {
                    str.Append(message.Details.PadRight(49)).Append(' ');
                }
                else
                {
                    str.Append(message.Details.Substring(0, 49)).Append(' ');
                }
            }
            else { str.Append(' ', 50); }

            #region debit/ credit Accounts
            if (message.DebitAccount1 != null)
            {
                if (message.DebitAccount1.Length < 16)
                {
                    str.Append(message.DebitAccount1.PadLeft(15));
                }
                else
                {
                    str.Append(message.DebitAccount1.Substring(0, 15));
                }
            }
            else { str.Append(' ', 15); }

            str.Append(' ', 15); //message.DebitAccount2          

            if (message.CreditAccount1 != null)
            {
                if (message.CreditAccount1.Length < 16)
                {
                    str.Append(message.CreditAccount1.PadLeft(15));
                }
                else
                {
                    str.Append(message.CreditAccount1.Substring(0, 15));
                }
            }
            else { str.Append(' ', 15); }

            if (message.CreditAccount2 != null)
            {
                if (message.CreditAccount2.Length < 16)
                {
                    str.Append(message.CreditAccount2.PadLeft(15));
                }
                else
                {
                    str.Append(message.CreditAccount2.Substring(0, 15));
                }
            }
            else { str.Append(' ', 15); }
            #endregion

            #region debit/credit Amounts
            if (invoice.ARInvoiceTypeCode == "CD")
            {
                if (message.DebitAmount1InLocalCurrency.ToString().StartsWith("-"))
                {
                    if (message.DebitAmount1InLocalCurrency != null) { str.Append(String.Format("{0:-000000000.00}", Math.Abs(message.DebitAmount1InLocalCurrency.Value)).Replace(".", "")); }
                    else { str.Append("-00000000000"); }

                    str.Append("-00000000000"); //message.DebitAmount2InLocalCurrency

                    if (message.CreditAmount1InLocalCurrency != null) { str.Append(String.Format("{0:-000000000.00}", Math.Abs(message.CreditAmount1InLocalCurrency.Value)).Replace(".", "")); }
                    else { str.Append("-00000000000"); }

                    if (message.CreditAmount2InLocalCurrency != null) { str.Append(String.Format("{0:-000000000.00}", Math.Abs(message.CreditAmount2InLocalCurrency.Value)).Replace(".", "")); }
                    else { str.Append("-00000000000"); }

                    if (message.DebitAmount1InInvoiceCurrency != null) { str.Append(String.Format("{0:-000000000.00}", Math.Abs(message.DebitAmount1InInvoiceCurrency.Value)).Replace(".", "")); }
                    else { str.Append("-00000000000"); }

                    str.Append("-00000000000");  //message.DebitAmount2InInvoiceCurrency

                    if (message.CreditAmount1InInvoiceCurrency != null) { str.Append(String.Format("{0:-000000000.00}", Math.Abs(message.CreditAmount1InInvoiceCurrency.Value)).Replace(".", "")); }
                    else { str.Append("-00000000000"); }

                    if (message.CreditAmount2InInvoiceCurrency != null) { str.Append(String.Format("{0:-000000000.00}", Math.Abs(message.CreditAmount2InInvoiceCurrency.Value)).Replace(".", "")); }
                    else { str.Append("-00000000000"); }
                }
                else
                {
                    if (message.DebitAmount1InLocalCurrency != null) { str.Append(String.Format("{0:+000000000.00}", Math.Abs(message.DebitAmount1InLocalCurrency.Value)).Replace(".", "")); }
                    else { str.Append("+00000000000"); }

                    str.Append("+00000000000"); //message.DebitAmount2InLocalCurrency

                    if (message.CreditAmount1InLocalCurrency != null) { str.Append(String.Format("{0:+000000000.00}", Math.Abs(message.CreditAmount1InLocalCurrency.Value)).Replace(".", "")); }
                    else { str.Append("+00000000000"); }

                    if (message.CreditAmount2InLocalCurrency != null) { str.Append(String.Format("{0:+000000000.00}", Math.Abs(message.CreditAmount2InLocalCurrency.Value)).Replace(".", "")); }
                    else { str.Append("+00000000000"); }

                    if (message.DebitAmount1InInvoiceCurrency != null) { str.Append(String.Format("{0:+000000000.00}", Math.Abs(message.DebitAmount1InInvoiceCurrency.Value)).Replace(".", "")); }
                    else { str.Append("+00000000000"); }

                    str.Append("+00000000000");  //message.DebitAmount2InInvoiceCurrency

                    if (message.CreditAmount1InInvoiceCurrency != null) { str.Append(String.Format("{0:+000000000.00}", Math.Abs(message.CreditAmount1InInvoiceCurrency.Value)).Replace(".", "")); }
                    else { str.Append("+00000000000"); }

                    if (message.CreditAmount2InInvoiceCurrency != null) { str.Append(String.Format("{0:+000000000.00}", Math.Abs(message.CreditAmount2InInvoiceCurrency.Value)).Replace(".", "")); }
                    else { str.Append("+00000000000"); }
                }
            }
            else
            {
                if (message.DebitAmount1InLocalCurrency.ToString().StartsWith("-"))
                {
                    if (message.DebitAmount1InLocalCurrency != null) { str.Append(String.Format("{0:-000000000.00}", Math.Abs(message.DebitAmount1InLocalCurrency.Value)).Replace(".", "")); }
                    else { str.Append("-00000000000"); }

                    str.Append("+00000000000"); //message.DebitAmount2InLocalCurrency

                    if (message.CreditAmount1InLocalCurrency != null) { str.Append(String.Format("{0:-000000000.00}", Math.Abs(message.CreditAmount1InLocalCurrency.Value)).Replace(".", "")); }
                    else { str.Append("-00000000000"); }

                    if (message.CreditAmount2InLocalCurrency != null) { str.Append(String.Format("{0:-000000000.00}", Math.Abs(message.CreditAmount2InLocalCurrency.Value)).Replace(".", "")); }
                    else { str.Append("-00000000000"); }

                    if (message.DebitAmount1InInvoiceCurrency != null) { str.Append(String.Format("{0:-000000000.00}", Math.Abs(message.DebitAmount1InInvoiceCurrency.Value)).Replace(".", "")); }
                    else { str.Append("-00000000000"); }

                    str.Append("+00000000000"); //DebitAmount2InInvoiceCurrency

                    if (message.CreditAmount1InInvoiceCurrency != null) { str.Append(String.Format("{0:-000000000.00}", Math.Abs(message.CreditAmount1InInvoiceCurrency.Value)).Replace(".", "")); }
                    else { str.Append("-00000000000"); }

                    if (message.CreditAmount2InInvoiceCurrency != null) { str.Append(String.Format("{0:-000000000.00}", Math.Abs(message.CreditAmount2InInvoiceCurrency.Value)).Replace(".", "")); }
                    else { str.Append("-00000000000"); }
                }
                else
                {
                    if (message.DebitAmount1InLocalCurrency != null) { str.Append(String.Format("{0:+000000000.00}", message.DebitAmount1InLocalCurrency.Value).Replace(".", "")); }
                    else { str.Append("+00000000000"); }

                    str.Append("+00000000000"); //message.DebitAmount2InLocalCurrency

                    if (message.CreditAmount1InLocalCurrency != null) { str.Append(String.Format("{0:+000000000.00}", message.CreditAmount1InLocalCurrency.Value).Replace(".", "")); }
                    else { str.Append("+00000000000"); }

                    if (message.CreditAmount2InLocalCurrency != null) { str.Append(String.Format("{0:+000000000.00}", message.CreditAmount2InLocalCurrency.Value).Replace(".", "")); }
                    else { str.Append("+00000000000"); }

                    if (message.DebitAmount1InInvoiceCurrency != null) { str.Append(String.Format("{0:+000000000.00}", message.DebitAmount1InInvoiceCurrency.Value).Replace(".", "")); }
                    else { str.Append("+00000000000"); }

                    str.Append("+00000000000"); //DebitAmount2InInvoiceCurrency

                    if (message.CreditAmount1InInvoiceCurrency != null) { str.Append(String.Format("{0:+000000000.00}", message.CreditAmount1InInvoiceCurrency.Value).Replace(".", "")); }
                    else { str.Append("+00000000000"); }

                    if (message.CreditAmount2InInvoiceCurrency != null) { str.Append(String.Format("{0:+000000000.00}", message.CreditAmount2InInvoiceCurrency.Value).Replace(".", "")); }
                    else { str.Append("+00000000000"); }
                }
            }
            #endregion

            str.Append(' ', 10); //message.ThirdDate           

            str.Append(' ', 9); //message.Reference3

            str.Append(' ', 9); //message.Quantity

            str.Append(message.File.PadRight(50)); // message.File

            str.Append(' ', 50); //message.Remarks

            str.Append(' ', 50); //message.AdditionalRemarks           

            str.Append(' ', 10); //message.Branch         

            if (message.VATNumber != null)
            {
                if (message.VATNumber.Length < 10)
                {
                    str.Append(message.VATNumber.PadLeft(9));
                }
                else
                {
                    str.Append(message.VATNumber.Substring(0, 9));
                }
            }
            else { str.Append(' ', 9); }

            string s = str.ToString();
            return s;
        }

        #region Stpes

        private Message Step_1_VAT(ARInvoice invoice, List<ARInvoiceLine> dueVatLines)
        {
            string shipmentNumber = invoice.MainEntityReference == null ? "" : invoice.MainEntityReference;
            
            ARInvoiceTotalVATRepository totalVATRep = new ARInvoiceTotalVATRepository(tenant);                       
            List<ARInvoiceTotalVAT> totalVATs = totalVATRep.GetInvoiceTotalVatsForInvoice(invoice.Id, tenant).ToList();

            Message message = new Message();

            message.TransactionTypeCode = myAccountingSystemCode == "HV" ? "" : "100";

            if (!string.IsNullOrEmpty(invoice.InvoiceNumber))
            {
                char[] characters = invoice.InvoiceNumber.ToCharArray();
                foreach (char c in characters)
                {
                    if (c == '0' || c == '1' || c == '2' || c == '3' || c == '4' || c == '5' || c == '6' || c == '7' || c == '8' || c == '9')
                    {
                        message.Reference1 += c.ToString();
                    }
                }
            }

            if (!string.IsNullOrEmpty(shipmentNumber))
            {
                char[] characters = shipmentNumber.ToCharArray();
                foreach (char c in characters)
                {
                    if (c == '0' || c == '1' || c == '2' || c == '3' || c == '4' || c == '5' || c == '6' || c == '7' || c == '8' || c == '9')
                    {
                        message.Reference2 += c.ToString();
                    }
                }
            }

            message.TransactionDate = invoice.InvoiceDate;
            message.ValueDate = invoice.DueDate;
            message.CurrencyCode = invoice.AccountingExternalCode != null ? invoice.AccountingExternalCode : "";

            //details
            if (myAccountingSystemCode == "HV")
            {
                char[] characters = invoice.InvoiceNumber.ToCharArray();
                foreach (char c in characters)
                {
                    if (c == '0' || c == '1' || c == '2' || c == '3' || c == '4' || c == '5' || c == '6' || c == '7' || c == '8' || c == '9')
                    {
                        message.Details += c.ToString();
                    }
                }
                message.Details += ",";

                char[] characters1 = shipmentNumber.ToCharArray();
                foreach (char c in characters1)
                {
                    if (c == '0' || c == '1' || c == '2' || c == '3' || c == '4' || c == '5' || c == '6' || c == '7' || c == '8' || c == '9')
                    {
                        message.Details += c.ToString();
                    }
                }
                message.Details += ",";
                message.Details += invoice.BillTo != null ? invoice.BillTo.EnglishName : "";
            }

            else
            {
                message.Details = (invoice.InvoiceNumber) + "," + (shipmentNumber) + "," + (invoice.BillTo != null ? invoice.BillTo.EnglishName : "");
            }

            double? localAmount = 0;
            double? invoiceAmount = 0;

            double? localVatAmount = 0;
            double? invoiceVatAmount = 0;

            double? localDueVatAmount = 0;
            double? invoiceDueVatAmount = 0;
            
            foreach (ARInvoiceLine line in dueVatLines)
            {
                localAmount += line.LocalCurrencyAmount + ((line.LocalCurrencyAmount != null ? line.LocalCurrencyAmount : 0) * (line.VatPercentage != null ? (line.VatPercentage / 100) : 0)).Value;
                invoiceAmount += line.InvoiceCurrencyAmount + ((line.InvoiceCurrencyAmount != null ? line.InvoiceCurrencyAmount : 0) * (line.VatPercentage != null ? (line.VatPercentage / 100) : 0)).Value;

                localVatAmount += ((line.LocalCurrencyAmount != null ? line.LocalCurrencyAmount : 0) * (line.VatPercentage != null ? (line.VatPercentage / 100) : 0)).Value;
                invoiceVatAmount += ((line.InvoiceCurrencyAmount != null ? line.InvoiceCurrencyAmount : 0) * (line.VatPercentage != null ? (line.VatPercentage / 100) : 0)).Value;

                localDueVatAmount += line.LocalCurrencyAmount;
                invoiceDueVatAmount += line.InvoiceCurrencyAmount;
            }

            message.DebitAccount1 = this.GetDebitAccount(invoice);
            message.CreditAccount1 = myVATableTempCard;

            message.CreditAccount2 = "";
            if (totalVATs != null)
            {
                ARInvoiceTotalVAT item = totalVATs.Where(d => d.VatPercent > 0).FirstOrDefault();
                if (item != null)
                {
                    message.CreditAccount2 = item.ExternalVATCard;
                }
            }

            message.DebitAmount1InLocalCurrency = localAmount;
            message.DebitAmount1InInvoiceCurrency = invoiceAmount;

            message.CreditAmount1InLocalCurrency = localDueVatAmount;
            message.CreditAmount1InInvoiceCurrency = invoiceDueVatAmount;

            message.CreditAmount2InLocalCurrency = localVatAmount;
            message.CreditAmount2InInvoiceCurrency = invoiceVatAmount;

            message.File = "0";

            if (!string.IsNullOrEmpty(invoice.VatNumber))
            {
                char[] characters = invoice.VatNumber.ToCharArray();
                foreach (char c in characters)
                {
                    if (c == '0' || c == '1' || c == '2' || c == '3' || c == '4' || c == '5' || c == '6' || c == '7' || c == '8' || c == '9')
                    {
                        message.VATNumber += c.ToString();
                    }
                }
            }
            else
            {
                message.VATNumber = "";
            }

            return message;
        }

        private Message Step_1_noVAT(ARInvoice invoice, List<ARInvoiceLine> noVatLines)
        {
            string shipmentNumber = invoice.MainEntityReference == null ? "" : invoice.MainEntityReference;

            Message message = new Message();

            message.TransactionTypeCode = myAccountingSystemCode == "HV" ? "" : "150";

            if (!string.IsNullOrEmpty(invoice.InvoiceNumber))
            {
                char[] characters = invoice.InvoiceNumber.ToCharArray();
                foreach (char c in characters)
                {
                    if (c == '0' || c == '1' || c == '2' || c == '3' || c == '4' || c == '5' || c == '6' || c == '7' || c == '8' || c == '9')
                    {
                        message.Reference1 += c.ToString();
                    }
                }
            }

            if (!string.IsNullOrEmpty(shipmentNumber))
            {
                char[] characters = shipmentNumber.ToCharArray();
                foreach (char c in characters)
                {
                    if (c == '0' || c == '1' || c == '2' || c == '3' || c == '4' || c == '5' || c == '6' || c == '7' || c == '8' || c == '9')
                    {
                        message.Reference2 += c.ToString();
                    }
                }
            }

            message.TransactionDate = invoice.InvoiceDate;
            message.ValueDate = invoice.DueDate;
            message.CurrencyCode = invoice.AccountingExternalCode != null ? invoice.AccountingExternalCode : "";

            //details

            if (myAccountingSystemCode == "HV")
            {
                char[] characters = invoice.InvoiceNumber.ToCharArray();
                foreach (char c in characters)
                {
                    if (c == '0' || c == '1' || c == '2' || c == '3' || c == '4' || c == '5' || c == '6' || c == '7' || c == '8' || c == '9')
                    {
                        message.Details += c.ToString();
                    }
                }
                message.Details += ",";

                char[] characters1 = shipmentNumber.ToCharArray();
                foreach (char c in characters1)
                {
                    if (c == '0' || c == '1' || c == '2' || c == '3' || c == '4' || c == '5' || c == '6' || c == '7' || c == '8' || c == '9')
                    {
                        message.Details += c.ToString();
                    }
                }
                message.Details += ",";
                message.Details += invoice.BillTo != null ? invoice.BillTo.EnglishName : "";
            }

            else
            {
                message.Details = (invoice.InvoiceNumber) + "," + (shipmentNumber) + "," + (invoice.BillTo != null ? invoice.BillTo.EnglishName : "");
            }

            double? localAmount = 0;
            double? invoiceAmount = 0;

            double? localVatAmount = 0;
            double? invoiceVatAmount = 0;

            double? localDueVatAmount = 0;
            double? invoiceDueVatAmount = 0;

            ICommonDataContext commonContext = CommonDataContext.GetContext(invoice.Tenant);
            foreach (ARInvoiceLine line in noVatLines)
            {
                VatType vattype = (from vat in commonContext.VatTypes
                                   where vat.Id == line.VatTypeId
                                   select vat).FirstOrDefault();

                if (vattype != null)
                {
                    List<VatTypePercentage> percentageList = (from percentage in commonContext.VatTypePercentages
                                                              where percentage.VatTypeId == vattype.Id
                                                              orderby percentage.FromDate descending
                                                              select percentage).ToList();

                    if (percentageList.Count > 0)
                    {
                        localAmount += line.LocalCurrencyAmount + ((line.LocalCurrencyAmount != null ? line.LocalCurrencyAmount : 0) * (percentageList[0].Percentage != null ? (percentageList[0].Percentage / 100) : 0)).Value;
                        invoiceAmount += line.InvoiceCurrencyAmount + ((line.InvoiceCurrencyAmount != null ? line.InvoiceCurrencyAmount : 0) * (percentageList[0].Percentage != null ? (percentageList[0].Percentage / 100) : 0)).Value;

                        localVatAmount += ((line.LocalCurrencyAmount != null ? line.LocalCurrencyAmount : 0) * (percentageList[0].Percentage != null ? (percentageList[0].Percentage / 100) : 0)).Value;
                        invoiceVatAmount += ((line.InvoiceCurrencyAmount != null ? line.InvoiceCurrencyAmount : 0) * (percentageList[0].Percentage != null ? (percentageList[0].Percentage / 100) : 0)).Value;

                        localDueVatAmount += line.LocalCurrencyAmount;
                        invoiceDueVatAmount += line.InvoiceCurrencyAmount;
                    }
                }
            }

            message.DebitAccount1 = this.GetDebitAccount(invoice);
            message.CreditAccount1 = myVATExemptTempCard;

            message.DebitAmount1InLocalCurrency = localAmount;
            message.DebitAmount1InInvoiceCurrency = invoiceAmount;

            message.CreditAmount1InLocalCurrency = localDueVatAmount;
            message.CreditAmount1InInvoiceCurrency = invoiceDueVatAmount;

            message.CreditAmount2InLocalCurrency = localVatAmount;
            message.CreditAmount2InInvoiceCurrency = invoiceVatAmount;

            message.File = "0";

            if (!string.IsNullOrEmpty(invoice.VatNumber))
            {
                char[] characters = invoice.VatNumber.ToCharArray();
                foreach (char c in characters)
                {
                    if (c == '0' || c == '1' || c == '2' || c == '3' || c == '4' || c == '5' || c == '6' || c == '7' || c == '8' || c == '9')
                    {
                        message.VATNumber += c.ToString();
                    }
                }
            }
            else
            {
                message.VATNumber = "";
            }

            return message;
        }

        private List<Message> Step_2_VAT(ARInvoice invoice, List<ARInvoiceLine> lines)
        {
            List<Message> incomeList = new List<Message>();
            ChargeTypeAccountingRepository chargeRep = new ChargeTypeAccountingRepository(tenant);
            ChargesTypeRepository chargeTypeRep = new ChargesTypeRepository(tenant);

            foreach (ARInvoiceLine line in lines)
            {
                Message income = new Message();

                ChargesType charge = chargeTypeRep.GetSingleChargesType(line.ChargesTypeId, tenant);

                if (myAccountingSystemCode == "HV")
                {
                    income.TransactionTypeCode = "";
                }

                else if (myAccountingSystemCode == "RH")
                {
                    income.TransactionTypeCode = "3";
                }

                if (!string.IsNullOrEmpty(invoice.InvoiceNumber))
                {
                    char[] characters = invoice.InvoiceNumber.ToCharArray();
                    foreach (char c in characters)
                    {
                        if (c == '0' || c == '1' || c == '2' || c == '3' || c == '4' || c == '5' || c == '6' || c == '7' || c == '8' || c == '9')
                        {
                            income.Reference1 += c.ToString();
                        }
                    }
                }

                if (!string.IsNullOrEmpty(invoice.MainEntityReference))
                {
                    char[] characters = invoice.MainEntityReference.ToCharArray();
                    foreach (char c in characters)
                    {
                        if (c == '0' || c == '1' || c == '2' || c == '3' || c == '4' || c == '5' || c == '6' || c == '7' || c == '8' || c == '9')
                        {
                            income.Reference2 += c.ToString();
                        }
                    }
                }

                income.TransactionDate = invoice.InvoiceDate;
                income.ValueDate = invoice.DueDate;
                income.CurrencyCode = invoice.AccountingExternalCode != null ? invoice.AccountingExternalCode : "";
                income.Details = line.Description != null ? line.Description : "";

                if (myAccountingSystemCode == "RH")
                {
                    income.DebitAccount1 = myVATableTempCard;                  
                }

                else
                {
                    income.DebitAccount1 = myVATableTempCard;                    
                }

                income.CreditAccount1 = line.CreditAccount != null ? line.CreditAccount : "";

                if (string.IsNullOrEmpty(income.CreditAccount1))
                {
                    if (charge.AccountingVATSplit)
                    {
                        IQueryable<ChargeTypeAccounting> iData = chargeRep.GetChargeTypeAccountingsForChargeType(line.ChargesTypeId, tenant);
                        ChargeTypeAccounting myLineData = iData.Where(d => d.VatTypeId == line.VatTypeId).FirstOrDefault();
                        if (myLineData != null)
                        {
                            income.CreditAccount1 = myLineData.ReceivableCreditAccount;
                        }
                    }

                    else
                    {
                        income.CreditAccount1 = charge.ReceivableCreditAccount;
                    }
                }

                if (string.IsNullOrEmpty(income.CreditAccount1))
                {
                    throw new ApplicationException("Error: Missing CreditAccount");
                }

                income.CreditAmount1InLocalCurrency = line.LocalCurrencyAmount;
                income.CreditAmount1InInvoiceCurrency = line.InvoiceCurrencyAmount;

                income.DebitAmount1InLocalCurrency = line.LocalCurrencyAmount;
                income.DebitAmount1InInvoiceCurrency = line.InvoiceCurrencyAmount;

                income.File = "0";

                if (!string.IsNullOrEmpty(invoice.VatNumber))
                {
                    char[] characters = invoice.VatNumber.ToCharArray();
                    foreach (char c in characters)
                    {
                        if (c == '0' || c == '1' || c == '2' || c == '3' || c == '4' || c == '5' || c == '6' || c == '7' || c == '8' || c == '9')
                        {
                            income.VATNumber += c.ToString();
                        }
                    }
                }
                else
                {
                    income.VATNumber = "";
                }

                incomeList.Add(income);
            }
            return incomeList;
        }

        private List<Message> Step_2_NOVAT(ARInvoice invoice, List<ARInvoiceLine> lines)
        {
            List<Message> incomeList = new List<Message>();
            ChargeTypeAccountingRepository chargeRep = new ChargeTypeAccountingRepository(tenant);
            ChargesTypeRepository chargeTypeRep = new ChargesTypeRepository(tenant);

            foreach (ARInvoiceLine line in lines)
            {
                Message income = new Message();

                ChargesType charge = chargeTypeRep.GetSingleChargesType(line.ChargesTypeId, tenant);

                if (myAccountingSystemCode == "HV")
                {
                    income.TransactionTypeCode = "";
                }

                else if (myAccountingSystemCode == "RH")
                {
                    if (charge != null)
                    {
                        if (charge.ChargesGroupCode == "CUSCH")
                        {
                            income.TransactionTypeCode = "3";
                        }
                        else
                        {
                            income.TransactionTypeCode = "150";
                        }
                    }
                    else
                    {
                        income.TransactionTypeCode = "3";
                    }
                }

                if (!string.IsNullOrEmpty(invoice.InvoiceNumber))
                {
                    char[] characters = invoice.InvoiceNumber.ToCharArray();
                    foreach (char c in characters)
                    {
                        if (c == '0' || c == '1' || c == '2' || c == '3' || c == '4' || c == '5' || c == '6' || c == '7' || c == '8' || c == '9')
                        {
                            income.Reference1 += c.ToString();
                        }
                    }
                }

                if (!string.IsNullOrEmpty(invoice.MainEntityReference))
                {
                    char[] characters = invoice.MainEntityReference.ToCharArray();
                    foreach (char c in characters)
                    {
                        if (c == '0' || c == '1' || c == '2' || c == '3' || c == '4' || c == '5' || c == '6' || c == '7' || c == '8' || c == '9')
                        {
                            income.Reference2 += c.ToString();
                        }
                    }
                }

                income.TransactionDate = invoice.InvoiceDate;
                income.ValueDate = invoice.DueDate;
                income.CurrencyCode = invoice.AccountingExternalCode != null ? invoice.AccountingExternalCode : "";
                income.Details = line.Description != null ? line.Description : "";

                if (myAccountingSystemCode == "RH")
                {
                    income.DebitAccount1 = GetDebitAccount(invoice);
                    
                }
                else
                {
                    income.DebitAccount1 = myVATExemptTempCard;                    
                }

                income.CreditAccount1 = line.CreditAccount != null ? line.CreditAccount : "";

                if (string.IsNullOrEmpty(income.CreditAccount1))
                {
                    if (charge.AccountingVATSplit)
                    {
                        IQueryable<ChargeTypeAccounting> iData = chargeRep.GetChargeTypeAccountingsForChargeType(line.ChargesTypeId, tenant);
                        ChargeTypeAccounting myLineData = iData.Where(d => d.VatTypeId == line.VatTypeId).FirstOrDefault();
                        if (myLineData != null)
                        {
                            income.CreditAccount1 = myLineData.ReceivableCreditAccount;
                        }
                    }

                    else
                    {
                        income.CreditAccount1 = charge.ReceivableCreditAccount;
                    }
                }

                if (string.IsNullOrEmpty(income.CreditAccount1))
                {
                    throw new ApplicationException("Error: Missing CreditAccount");
                }

                income.CreditAmount1InLocalCurrency = line.LocalCurrencyAmount;
                income.CreditAmount1InInvoiceCurrency = line.InvoiceCurrencyAmount;

                income.DebitAmount1InLocalCurrency = line.LocalCurrencyAmount;
                income.DebitAmount1InInvoiceCurrency = line.InvoiceCurrencyAmount;

                income.File = "0";

                if (!string.IsNullOrEmpty(invoice.VatNumber))
                {
                    char[] characters = invoice.VatNumber.ToCharArray();
                    foreach (char c in characters)
                    {
                        if (c == '0' || c == '1' || c == '2' || c == '3' || c == '4' || c == '5' || c == '6' || c == '7' || c == '8' || c == '9')
                        {
                            income.VATNumber += c.ToString();
                        }
                    }
                }
                else
                {
                    income.VATNumber = "";
                }

                incomeList.Add(income);
            }
            return incomeList;
        }

        #endregion

        #region Build XML
        private void BuildXMLFile(ARInvoiceRoot myClass, int tenant, string fileName)
        {
            MemoryStream memstream = new MemoryStream();
            XmlSerializer ser = new XmlSerializer(typeof(ARInvoiceRoot));
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();

            //ns.Add("", "http://www.champ.aero/GCCS/CargoXML");

            XmlWriterSettings settings = new XmlWriterSettings()
            {
                Indent = true,
                IndentChars = "",
                OmitXmlDeclaration = true,
                NewLineChars = "",
                NewLineHandling = NewLineHandling.Replace
            };

            XmlWriter writer = XmlTextWriter.Create(memstream, settings);

            writer.WriteRaw("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>\r\n");

            ser.Serialize(writer, myClass, ns);
            memstream.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(memstream);
            string content = reader.ReadToEnd();

            content = content.Replace(" />", "/>");

            byte[] bytearray = Encoding.UTF8.GetBytes(content);

            if (this.isDropBox)
            {
                this.BulidDropBoxXMLLFile(bytearray);
            }

            else if(this.UsingFTP && !string.IsNullOrEmpty(this.FTPDetailId))
            {
                this.BuildFile_ViaFTP(bytearray);
            }

            else
            {
                if (bytearray != null)
                {
                    string[] fileProps = fileName.Split('.');
                    Logitude.Server.Tools.BlobFileInfo fileInfo = new Logitude.Server.Tools.BlobFileInfo()
                    {
                        FileName = fileProps[0],
                        Extension = fileProps.Length > 1 ? fileProps[1] : "xml",
                        Tenant = tenant,
                        FileSize = bytearray.Length,
                        HasExternalContainer = true,
                        ExternalContainerName = "tenant" + tenant
                    };
                    Logitude.Server.Tools.StorageService.IBlobService storageservice = Logitude.Server.Tools.ContainerAccessor.Container.Resolve(typeof(Logitude.Server.Tools.StorageService.IBlobService), "StorageService", new ParameterOverride("", 1)) as Logitude.Server.Tools.StorageService.IBlobService;
                    storageservice.Write(bytearray, fileInfo);
                }
            }
        }

        private void BulidDropBoxXMLLFile(byte[] bytearray)
        {
            DropBoxActionsHelper helper = new DropBoxActionsHelper(tenant);
            ObjectTableRepository repo = new ObjectTableRepository(tenant);
            var objectTableId = repo.GetObjectTableIdByName("ARInvoice");
            var invoice = this.invoices.FirstOrDefault();
            var entityId = "";
            if (invoice != null)
            {
                entityId = invoice.Id;
            }
            var commLog = helper.CreateDropBoxCommunicationLog(tenant, objectTableId, bytearray, this.filename, "ARInvoices", "AR Invoice", entityId);
        }

        private string myDocumentId;
        private string myDocumentFolder;
        private string myDocumentExtension;
        private string myCommunicationLogId;
        private void BuildFile_ViaFTP(byte[] myByteArray)
        {
            ObjectTableRepository repo = new ObjectTableRepository(tenant);
            string  objectTableId = repo.GetObjectTableIdByName("ARInvoice");
            string FTPFileName = "";

            AccountingTranferViaFTPHelper helper = new AccountingTranferViaFTPHelper(tenant, objectTableId, FTPDetailId);           
            var invoice = this.invoices.FirstOrDefault();
            var entityId = "";
            if (invoice != null)
            {
                entityId = invoice.Id;
                FTPFileName = ("ARInvoice_" + invoice.InvoiceNumber).ToLower();
            }
            
            CommunicationLog commLog = helper.CreateCommunicationLog(myByteArray, FTPFileName, entityId, myAccountingSystemCode);

            this.myDocumentId = helper.DocumentId;
            this.myDocumentFolder = helper.DocumentFolder;
            this.myDocumentExtension = helper.DocumentExtension;
            this.myCommunicationLogId = commLog.Id;

            BlobFileInfo fileInfo = new BlobFileInfo()
            {
                FileName = myDocumentId,
                FolderName = myDocumentFolder,
                Extension = myDocumentExtension,
                Tenant = tenant,
                FileSize = myByteArray.Length,
            };

            IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
            storageservice.Write(myByteArray, fileInfo);

            try
            {
                IQueueService queueservice = new DbQueueService();
                queueservice.InitializeQueue(commLog.QueueName, 0);
                queueservice.Send(new Dictionary<string, string>() { { "CommunicationLogId", commLog.Id }, { "Tenant", tenant.ToString() } }, tenant);
            }

            catch (Exception ex)
            {
                string ip = "";

                if (HttpContext.Current != null && HttpContext.Current.Request != null)
                {
                    ip = HttpContext.Current.Request.UserHostAddress;
                }

                ExceptionHandler.HandleException(ex, System.DateTime.Now, 0, null, "AR Invoice Transfer", null, ip);
            }
        }
        #endregion

        public string GetMessageData(string invoiceId, int tenant)
        {
            ARInvoiceRepository invoiceRepository = new ARInvoiceRepository(this.invoiceCotnext);

            ARInvoice invoice = invoiceRepository.GetSingleInvoice(invoiceId);
            List<ARInvoiceLine> invoiceLines = invoiceCotnext.ARInvoiceLines.Where(d => d.ARInvoiceId == invoiceId && d.Tenant == tenant).OrderBy(o => o.ChargesType.ViewOrder).ToList();

            string myResult = this.Compute(invoice, invoiceLines);
            return myResult;
        }
        private string GetDebitAccount(ARInvoice invoice)
        {
            string myResult = invoice.DebitAccount == null ? null : invoice.DebitAccount.Trim();

            if (string.IsNullOrEmpty(myResult))
            {
                Card myCard = CardRepository.GetSingleCard(invoice.BillToId, invoice.Tenant, true);
                if (myCard != null)
                {
                    AccountingSystemHelper accountingSystemHelper = new AccountingSystemHelper();
                    myResult = accountingSystemHelper.GetGenericCreditAccount(myCard.Id, invoice.InvoiceCurrencyId, tenant, false);
                }                
            }

            if (string.IsNullOrEmpty(myResult))
            {
                throw new ApplicationException("Error: Missing Debit Account 1");
            }

            return myResult;
        }
        private string GetGenericCreditAccount(ARInvoiceLine myline)
        {
            string myResult = myline.CreditAccount == null ? null : myline.CreditAccount.Trim();

            if (string.IsNullOrEmpty(myResult))
            {
                ChargeTypeAccountingRepository chargeRep = new ChargeTypeAccountingRepository(tenant);
                ChargesTypeRepository chargeTypeRep = new ChargesTypeRepository(tenant);
                ChargesType charge = chargeTypeRep.GetSingleChargesType(myline.ChargesTypeId, tenant);

                if (charge.AccountingVATSplit)
                {
                    IQueryable<ChargeTypeAccounting> iData = chargeRep.GetChargeTypeAccountingsForChargeType(myline.ChargesTypeId, tenant);
                    ChargeTypeAccounting myLineData = iData.Where(d => d.VatTypeId == myline.VatTypeId).FirstOrDefault();
                    if (myLineData != null)
                    {
                        myResult = myLineData.ReceivableCreditAccount;
                    }
                }
                else
                {
                    myResult = charge.ReceivableCreditAccount;
                }
            }

            if (string.IsNullOrEmpty(myResult))
            {
                throw new ApplicationException("Error: Missing Credit Account");
            }

            return myResult;
        }
        private void SetCustomFields(string objectTableName, int tenant, Object entity, ARShipmentDetailsElement element)
        {
            TextCodeRepository textCodeRepository = new TextCodeRepository(this.tenant);
            CustomFieldResolver customFieldResolver = new CustomFieldResolver();
            List<ObjectField> customFields = ObjectFieldRepository.GetCustomObjectFieldsByObjectTableName(objectTableName, tenant).ToList();

            foreach (ObjectField field in customFields)
            {
                if (entity != null)
                {
                    string fieldValue = customFieldResolver.GetFieldValue(entity, field, tenant);
                    PropertyInfo valuePropInfo = element.GetType().GetProperty(objectTableName + field.FieldName + "Value");
                    valuePropInfo.SetValue(element, fieldValue, null);

                    if (!string.IsNullOrEmpty(fieldValue))
                    {
                        string fieldName = textCodeRepository.GetSingleTextCodeByTenant(field.FullNameTextCodeCode, tenant).DefaultText;
                        PropertyInfo namePropInfo = element.GetType().GetProperty(objectTableName + field.FieldName + "Name");
                        namePropInfo.SetValue(element, fieldName, null);
                    }
                }
            }
        }
    }
}