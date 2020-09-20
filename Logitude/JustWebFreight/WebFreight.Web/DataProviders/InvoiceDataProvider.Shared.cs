using Simplog.Data.InvoiceModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace WebFreight.Web.DataProviders
{
    public class InvoiceDataProvider : BaseDataProvider
    {
        public string ProjectNumber { get; set; }
        public string TenantStateCode { get; set; }
        public string BranchSignature { get; set; }
        public string PackageDetails { get; set; }
        public string Type { get; set; }
        public string ShipmentCreateDate { get; set; }
        public DateTime ShipmentCreateDateAsDateFormat { get; set; }
        public string InvoiceSection1 { get; set; }
        public string InvoiceSection2 { get; set; }
        public string BankDetails { get; set; }
        public string BillToBankName { get; set; }
        public string BillToBankAddress { get; set; }
        public string BillToSwift { get; set; }
        public string BillToAccountNumber { get; set; }
        public string BillToIBANNumber { get; set; }
        public string CustomerRef { get; set; }
        public string InvoiceNumber { get; set; }
        public string BillTo { get; set; }
        public string BillTo_LocalName { get; set; }
        public string BillToAddress { get; set; }
        public string BillToAddress_NoName { get; set; }
        public string BillToAddress_OneLine { get; set; }
        public string BillToAddressDescription { get; set; }
        public string BillToVatNumber { get; set; }
        public string BillToCustomerCode { get; set; }
        public string BillToTelephone { get; set; }
        public string BillToStateCode { get; set; }
        public string ReceivablesExternalID { get; set; }
        public string DebitAccount { get; set; }
        public string AccountingNumber { get; set; }
        public string InvoiceDate { get; set; }
        public DateTime? InvoiceDateAsDateFormat { get; set; }
        public string DueDate { get; set; }
        public DateTime? DueDateAsDateFormat { get; set; }
        public string Status { get; set; }
        public string WaterMark { get; set; }
        public string IssuedByUser { get; set; }
        public string IssuedByUser_LocalName { get; set; }
        public string MainCarriageLastDestination { get; set; }
        public string MainCarriageLastDestination_label { get; set; }
        public string MainCarriageLastDestinationPortCode { get; set; }
        public string MainCarriageMAWBOBLBL { get; set; }
        public string MainCarriageMAWBOBLBL_label { get; set; }
        public string GrossWeight { get; set; }
        public string ChargeableWeight { get; set; }
        public string WeightUnitCode { get; set; }
        public string MainCarriageFromPortName { get; set; }
        public string MainCarriageFromPort_LocalName { get; set; }
        public string MainCarriageToPortName { get; set; }
        public string MainCarriageToPort_LocalName { get; set; }
        public string MainCarriageFromPortCode { get; set; }
        public string MainCarriageToPortCode { get; set; }
        public string MainCarriageCarrier { get; set; }
        public string MainCarriageCarrier_label { get; set; }
        public string MainCarriageCarrierNumber { get; set; }
        public string MainCarriageCarrierPrefix { get; set; }
        public string MainCarriageCarrierType_label { get; set; }
        public string MainCarriageExpectedDate { get; set; }
        public string InvoiceType_label { get; set; }
        public string InvoiceType_labelHebrew { get; set; }
        public string InvoiceType_label_Spanish { get; set; }
        public string Draft_labelHebrew { get; set; }
        public string Draft_label { get; set; }
        public string MainCarriageCarrierDate { get; set; }
        public string ShipmentNumber { get; set; }
        public string ShipmentType { get; set; }
        public string HouseNumber { get; set; }
        public string ClientNumber { get; set; }
        public string Shipper { get; set; }
        public string Shipper_LocalName { get; set; }
        public string ShipperAddress { get; set; }
        public string Consignee { get; set; }
        public string Consignee_LocalName { get; set; }
        public string ConsigneeAddress { get; set; }
        public string Volume { get; set; }
        public string VolumetricWeight { get; set; }
        public string Quantity { get; set; }
        public string ContainersNumbersArray { get; set; }
        public string ContainersNumbersAndTypesArray { get; set; }
        public string Notes { get; set; }
        public string DescriptionOfGoods { get; set; }
        public string MainCarriageVesselName { get; set; }
        public string MainCarriageVessel_LocalName { get; set; }
        public string PaymentTerm { get; set; }
        public string PaymentTerm_Local { get; set; }
        public string PaymentTermDescription { get; set; }
        public string PaymentTermLocalDescription { get; set; }
        public string VatTypesDescription { get; set; }
        public string VatTypesLocalDescription { get; set; }
        public string FinalDestinationPortName { get; set; }
        public string FinalDestinationPortCountryLocalName { get; set; }
        public string FinalDestinationPort_LocalName { get; set; }
        public string FinalDestinationPortCode { get; set; }
        public string NumberofPackages { get; set; }
        public DateTime FromPeriod { get; set; }
        public DateTime ToPeriod { get; set; }
        public string Name { get; set; }
        public string Currency { get; set; }
        public double? TotalSub { get; set; }
        public double? TotalVats { get; set; }
        public double? TotalsGrands { get; set; }

        public double? TotalVats_1 { get; set; }
        public double? TotalVats_2 { get; set; }
        public double? TotalVats_3 { get; set; }
        public double? TotalVats_4 { get; set; }

        public string VAT1Header { get; set; }
        public string VAT2Header { get; set; }
        public string VAT3Header { get; set; }
        public string VAT4Header { get; set; }

        public string VAT1Code { get; set; }
        public string VAT2Code { get; set; }
        public string VAT3Code { get; set; }
        public string VAT4Code { get; set; }

        public string ApprovedBy { get; set; }
        public string ApprovedDate { get; set; }
        public DateTime? ApprovedDateAsDateFormat { get; set; }
        public string HouseNumber_Label { get; set; }
        public string Containers_Label { get; set; }
        public string MainCarriageVessel_Label { get; set; }
        public string ETD { get; set; } //take the departure from the main carriage from, and the arrivals from the final destination to
        public string ETA { get; set; }
        public string ATD { get; set; }
        public string ATA { get; set; }
        public string TotalContainers { get; set; } //(ex : 2 x 40’OT, 1 x 20’GP) it is calculated for the messages
        public string PackagesInDetails { get; set; }

        public DateTime? ETDAsDateFormat { get; set; }
        public DateTime? ETAAsDateFormat { get; set; }
        public DateTime? ATDAsDateFormat { get; set; }
        public DateTime? ATAAsDateFormat { get; set; }

        public string TenantName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Signature { get; set; }
        public string TenantPhone { get; set; }
        public string TenantFax { get; set; }

        public string Via1 { get; set; }
        public string Via2 { get; set; }
        public string Via3 { get; set; }
        public string ViaList { get; set; }
        public string InsidePackagesDetails { get; set; }
        public bool IsAir { get; set; }
        public bool IsOcean { get; set; }
        public bool IsInland { get; set; }

        //invoice lines
        public List<ReportInvoiceLine> InvoiceLinesList { get; set; }
        public List<InvoicesReport> InvoicesReportList { get; set; }
        public List<InvoiceTotals> InvoiceTotalsList { get; set; }
        public List<InvoicesReport> InvoicesReportList_NotSorted { get; set; }
        //invoice totlas
        public string SubTotalLocalCurr { get; set; }
        public string SubTotalInvoiceCurr { get; set; }
        public string TotalLocalCurr { get; set; }
        public double TotalInvoiceCurr { get; set; }
        public string Tax { get; set; }
        public string LocalCurrency { get; set; }
        public string InvoiceCurrency { get; set; }
        public string InvoicecurrencyLocalName { get; set; }
        public string LocalCurrencySign { get; set; }
        public string InvoiceCurrencySign { get; set; }
        public string VatNumber { get; set; }
        public List<TotalVat> TotalVatList { get; set; }
        public string VATAmounts { get; set; }
        public string VatableAmounts { get; set; }
        public List<TotalVat> TotalVatListNonZero { get; set; }
        public string VATAmountsNonZero { get; set; }
        public string VatableAmountsNonZero { get; set; }
        public string ClientRef1 { get; set; }
        public string ClientRef2 { get; set; }
        public string CopyNameNoDraft { get; set; }
        public string CopyName { get; set; }
        public string CopyName_hebrew { get; set; }
        public string PickUpATD { get; set; }
        public string DeliveryATA { get; set; }
        public string maincarriagecarrierLocalName { get; set; }
        public string ConsigneeRef1 { get; set; }
        public string ConsigneeRef2 { get; set; }
        public string FromLocation { get; set; }
        public string FromLocation_PortName { get; set; }
        public string FromLocation_CountryName { get; set; }
        public string ToLocation { get; set; }
        public string ToLocation_PortName { get; set; }
        public string ToLocation_CountryName { get; set; }
        public string FinalLocation { get; set; }
        public string FinalLocation_PortName { get; set; }
        public string FinalLocation_CountryName { get; set; }
        public string OriginalInvoiceNumber { get; set; }
        public string OriginalInvoiceNumber_label { get; set; }
        public string LocalCurrencyExchangeRates { get; set; }
        public string InvoiceCurrencyExchangeRates { get; set; }
        public string CustomerField1 { get; set; }
        public string CustomerField2 { get; set; }
        public string CustomerField3 { get; set; }
        public string CustomerField4 { get; set; }
        public string CustomerField5 { get; set; }
        public string CustomerField6 { get; set; }
        public string CustomerField7 { get; set; }
        public string CustomerField8 { get; set; }
        public string CustomerField9 { get; set; }
        public string CustomerField10 { get; set; }
        public string ShipmentField1 { get; set; }
        public string ShipmentField2 { get; set; }
        public string ShipmentField3 { get; set; }
        public string ShipmentField4 { get; set; }
        public string ShipmentField5 { get; set; }
        public string ShipmentField6 { get; set; }
        public string ShipmentField7 { get; set; }
        public string ShipmentField8 { get; set; }
        public string ShipmentField9 { get; set; }
        public string ShipmentField10 { get; set; }
        public string ShipmentField11 { get; set; }
        public string ShipmentField12 { get; set; }
        public string ShipmentField13 { get; set; }
        public string ShipmentField14 { get; set; }
        public string ShipmentField15 { get; set; }
        public string ShipmentField16 { get; set; }
        public string ShipmentField17 { get; set; }
        public string ShipmentField18 { get; set; }
        public string ShipmentField19 { get; set; }
        public string ShipmentField20 { get; set; }
        public string ShipmentField21 { get; set; }
        public string ShipmentField22 { get; set; }
        public string ShipmentField23 { get; set; }
        public string ShipmentField24 { get; set; }
        public string ShipmentField25 { get; set; }
        public string ShipmentField26 { get; set; }
        public string ShipmentField27 { get; set; }
        public string ShipmentField28 { get; set; }
        public string ShipmentField29 { get; set; }
        public string ShipmentField30 { get; set; }
        public string ShipmentField31 { get; set; }
        public string ShipmentField32 { get; set; }
        public string ShipmentField33 { get; set; }
        public string ShipmentField34 { get; set; }
        public string ShipmentField35 { get; set; }
        public string ShipmentField36 { get; set; }
        public string ShipmentField37 { get; set; }
        public string ShipmentField38 { get; set; }
        public string ShipmentField39 { get; set; }
        public string ShipmentField40 { get; set; }

        public string ARInvoiceField1 { get; set; }
        public string ARInvoiceField2 { get; set; }
        public string ARInvoiceField3 { get; set; }
        public string ARInvoiceField4 { get; set; }
        public string ARInvoiceField5 { get; set; }
        public string ARInvoiceField6 { get; set; }
        public string ARInvoiceField7 { get; set; }
        public string ARInvoiceField8 { get; set; }
        public string ARInvoiceField9 { get; set; }
        public string ARInvoiceField10 { get; set; }
        public string MasterNumber { get; set; }
        public string SalesMan { get; set; }
        public string CustomsDeclarationNumber { get; set; }
        public string Origin { get; set; }
        public string FinalDestination { get; set; }
        public string LoadingPlace { get; set; }
        public string LoadingPlaceShipper { get; set; }
        public string PlaceOfDelivery { get; set; }
        public string MasterInternalNumber { get; set; }
        public string PONumber { get; set; }
        public string ShipperRefNo { get; set; }
        public string ConstituentInvoices { get; set; }
        public string DeliveryFrom { get; set; }
        public string DeliveryTo { get; set; }
        public string Incoterm { get; set; } //(Code ) 
        public int? DeliveryPackagesQuantity { get; set; }
        public double? DeliveryPackagesWeight { get; set; }
        public string DeliveryTrailerNo { get; set; }
        public string DeliveryDriverName { get; set; }
        public string FianlDestinationInclOnCarriagePortName { get; set; }
        public string FianlDestinationInclOnCarriagePortCode { get; set; }
        public string VoyageNo { get; set; }
        public string ContactPersonName { get; set; }
        public string ContactPersonEmail { get; set; }
        public string MainCarriageMAWBOBLBL_label_new { get; set; }
        public string HouseNumber_HBL { get; set; }
        public string IRSPlace { get; set; }
        public string IRSNumber { get; set; }
        public string CustomerReferences { get; set; }
        public string BookingConfirmationNumber { get; set; }
        //public string RequestedPaymentMethodCode { get; set; }
        //public string RequestedPaymentMethodName { get; set; }

        public string ReleasingAgentName { get; set; }
        public string ReleasingAgentAddress { get; set; }

        public string AgentName { get; set; }
        public string AgentAddress { get; set; }

        public double? VatableAmount_ZERO { get; set; }
        public double? VatableAmount_REIM { get; set; }
        public double? VatableAmount_12 { get; set; }
        
        public string FreightPC { get; set; }
        public double? USD_MXN_ExchangeRate { get; set; }
        public double? InvoiceToAccountingExchangeRate { get; set; }
        public string ShipmentLevel { get; set; }
        public string BranchName { get; set; }
        public string BranchAddress { get; set; }

        public List<ReportInvoiceLine> ExpenseInvoiceLinesList { get; set; }
        public List<ReportInvoiceLine> NoExpenseInvoiceLinesList { get; set; }
        public List<TotalVat> ExpenseTotalVatList { get; set; }
        public List<TotalVat> NoExpenseTotalVatList { get; set; }

        //invoice totlas
        public double? ExpenseSubTotalLocalCurr { get; set; }
        public double? ExpenseSubTotalInvoiceCurr { get; set; }
        public double? NoExpenseSubTotalLocalCurr { get; set; }
        public double? NoExpenseSubTotalInvoiceCurr { get; set; }

        public double? ExpenseTotalLocalCurr { get; set; }
        public double? ExpenseTotalInvoiceCurr { get; set; }
        public string AmountInWordsExpenseTotalInvoiceCurrFrench { get; set; }
        public string AmountInWordsExpenseTotalInvoiceCurrSpanish { get; set; }

        public double? NoExpenseTotalLocalCurr { get; set; }
        public double? NoExpenseTotalInvoiceCurr { get; set; }
        public string AmountInWordsNoExpenseTotalInvoiceCurrFrench { get; set; }
        public string AmountInWordsNoExpenseTotalInvoiceCurrSpanish { get; set; }

        public double? AmountDueInLocalCurrency { get; set; }
        public double? AmountDueInInvoiceCurrency { get; set; }

        public string Shipper2 { get; set; }
        public string Shipper3 { get; set; }
        public string Shipper4 { get; set; }
        public string Shipper5 { get; set; }
        public string HAWB2 { get; set; }
        public string HAWB3 { get; set; }
        public string HAWB4 { get; set; }
        public string HAWB5 { get; set; }

        public string BillToPrimaryContactMobile { get; set; }
        public string BillToPrimaryContactBusinessPhone { get; set; }

        public List<ReportConstituentInvoiceLine> ConstituentInvoicesList { get; set; }

        private SAT sAT;
        public SAT SAT
        {
            get
            {
                if (this.sAT == null)
                {
                    this.sAT = new SAT();
                }

                return sAT;
            }

            set
            {

                sAT = value;
            }
        }

        public decimal AmountInWords { get; set; }
         
        public string DeclarationNumber { get; set; }
        public DateTime? DeclarationDate { get; set; }
        public DateTime? CustomsClearanceDate { get; set; }

        public string AmountInWordsFrench { get; set; }
        public string AmountInWordsFrenchWithFR { get; set; }
        public string AmountInWordsFrenchNoFR { get; set; }

        public string AmountInWordsSpanish { get; set; }
        public string AmountInWordsSpanishWithZero { get; set; }
        public string AmountInWordsEnglish { get; set; }
        public string AmountInWordsEnglishNoFR { get; set; }
        public string AmountInWordsRussian { get; set; }

        public string AmountsInEnglishWithZero { get; set; }

        public DateTime? FreightRelease { get; set; }
        public DateTime? TerminalAvailable { get; set; }
        public string ISFNumber { get; set; }
        public DateTime? ISFDate { get; set; }
        public string ITNumber { get; set; }
        public DateTime? ITDate { get; set; }
        public string FMCNumber { get; set; }
        public string ENSNumber { get; set; }
        public DateTime? ENSDate { get; set; }

        public string OBLType { get; set; }
        public DateTime? DocumentsClosingDate { get; set; }

        public string TotalVATableAmountInInvoiceCurrency { get; set; }
        public string TotalNONVATableAmountInInvoiceCurrency { get; set; }

        public string CompleteShipmentType { get; set; }

        public double TotalProfitCurr { get; set; }
        
        public string Notify1Address { get; set; }
        public string Notify1VATNumber { get; set; }

        public string Transshipment1MasterNumber { get; set; }
        public string Transshipment1FromPortName { get; set; }
        public string Transshipment1CarrierName { get; set; }
        public string CustomsClearancePointName { get; set; }
        public double? ValueOfGoods { get; set; }
        public string ValueOfGoodsCurrency { get; set; }
        public class InvoicesReport
        {
            public string InvoiceType { get; set; }
            public string InvoiceNumber { get; set; }
            public string InternalNumber { get; set; }
            public string BillTo { get; set; }
            public DateTime InvoiceDate { get; set; }
            public string OurRefNumber { get; set; }
            public string InvoiceStatus { get; set; }
            public double? SubTotallocal { get; set; }
            public double? VATlocal { get; set; }
            public double? GrandTotallocal { get; set; }
            public string Currency { get; set; }
            public double? vatInLocal { get; set; }
            public double? subInLocal { get; set; }
            public string LocalCurrency { get; set; }
            public DateTime? CreateDate { get; set; }
            public DateTime? DueDate { get; set; }
            
            public double? VAT1Amount { get; set; }
            public double? VAT2Amount { get; set; }
            public double? VAT3Amount { get; set; }
            public double? VAT4Amount { get; set; }

            public string ARInvoiceField1 { get; set; }
            public string ARInvoiceField2 { get; set; }
            public string ARInvoiceField3 { get; set; }
            public string ARInvoiceField4 { get; set; }
            public string ARInvoiceField5 { get; set; }
            public string ARInvoiceField6 { get; set; }
            public string ARInvoiceField7 { get; set; }
            public string ARInvoiceField8 { get; set; }
            public string ARInvoiceField9 { get; set; }
            public string ARInvoiceField10 { get; set; }
            public string Salesman { get; set; }
            public double? ExpenseCharges { get; set; }
            public double? SubTotalInLocalCurrency { get; set; }
            public double? VATInLocalCurrency { get; set; }
            public double? GrandTotalInLocalCurrency { get; set; }
            public double? ExpenseChargesInLocalCurrency { get; set; }
            public string UUID { get; set; }
            public string BillToCode { get; set; }
        }

        public class InvoiceTotals
        {
            public int? TotalInvoices { get; set; }
            public double? TotalSubTotals { get; set; }
            public double? TotalVats { get; set; }
            public double? TotalGrands { get; set; }
            public string Currency { get; set; }
            public double? totalGrandTotal { get; set; }
            
            public double? VAT1Amount { get; set; }
            public double? VAT2Amount { get; set; }
            public double? VAT3Amount { get; set; }
            public double? VAT4Amount { get; set; }
        }

        public List<ShipmentAssemblyLine> Assemblies { get; set; }

        // Warehouse Fields 
        public string WarehouseLegTerminalName { get; set; }
        public string WarehouseLegAddress { get; set; }
        public string WarehouseLegTerminalCode { get; set; }
        public DateTime? WarehouseLegExpectedEntryDate { get; set; }
        public DateTime? WarehouseLegActualEntryDate { get; set; }
        public DateTime? WarehouseLegExpectedReleaseDate { get; set; }
        public DateTime? WarehouseLegActualReleaseDate { get; set; }
        public DateTime? WarehouseLegLastFreeDate { get; set; }
        public string WarehouseLegRemarks { get; set; }
        public string WarehouseLegReference { get; set; }
        public DateTime? WarehouseLegEntryDate { get; set; }
        public DateTime? WarehouseLegReleaseDate { get; set; }

        public List<PickUpDeliveryLine> PickUpsLines { get; set; }
        public List<PickUpDeliveryLine> DeliveriesLines { get; set; }

        public DateTime? PickupETD { get; set; }
        public DateTime? DeliveryETD { get; set; }
        public string PickupAddress { get; set; }
        public string DeliveryAddress { get; set; }
        public string PickupShortAddress { get; set; }

        public string DepositBankEnglishName { get; set; }
        public string DepositBankLocalName { get; set; }
        public string DepositBankSwiftCode { get; set; }
        public string DepositBankIBAN { get; set; }

        public string LCLContainersNumbersArray { get; set; }
        public string AMSBL { get; set; }
        public string MainHarmonize { get; set; }

        public string AgentReference1 { get; set; }
        public string AgentReference2 { get; set; }

        public string TenantCBSA { get; set; }
        public string TenantCAAT { get; set; }
        public string CarrierCBSA { get; set; }
        public string CarrierCAAT { get; set; }
        public string AccountDisplayNumber { get; set; }
        
        public string ShipperNotExporter { get; set; }
        public string ShipperNotExporterAddress { get; set; }
        public string ConsigneeNotImporter { get; set; }
        public string ConsigneeNotImporterAddress { get; set; }
        public string SalesmanEmail { get; set; }
        public string MoveType { get; set; }
        public string MainCarriageLastdestinationPortName { get; set; }
        public string MainCarriageLastdestinationPortCode { get; set; }
        public string BillToBillingAddress { get; set; }
        public string ShipmentSubTypeName { get; set; }
        public string TrailerNumber { get; set; }
        public int WarehouseFreeDays { get; set; }
        public string PreCarriageVessel { get; set; }

        public int? StorageFreeDays { get; set; }
        public int? StorageDays { get; set; }
        public List<StoragePricing> ShipmentStoragePricings { get; set; }
    }

    public class SAT
    {
        public string SelloSAT { get; set; }
        public string SelloCFD { get; set; }
        public string UUID { get; set; }
        public string QR { get; set; }
        public DateTime FechaTimbardo { get; set; }
        public string NoCertificadoSAT { get; set; }
        public string NoCertificado { get; set; }
        public string PaymentMethodCode { get; set; }
        public string PaymentMethodName { get; set; }
        public string PaymentMethodLocalName { get; set; }
        public string FormadePago { get; set; }
        public string RegimenFiscal { get; set; }
        public string TipoDeComprobante { get; set; }
        public string CadenaOriginal { get; set; }

        public string LugardeExpedicion { get; set; }
        public string BillToZipCode { get; set; }
        public string MetodoPago { get; set; }
        public string usoCFDI { get; set; }
        public string TipoRelacion { get; set; }
        public string CFDIRelacionado { get; set; }
        
        public Image QRImage { get; set; }
    }

    public class ReportConstituentInvoiceLine
    {
        public string InvoiceId { get; set; }
        public string InvoiceNumber { get; set; }
        public string CustomerRef { get; set; }
        public string InvoiceDate { get; set; }
        public string BillToName { get; set; }
        public string InvoiceCurrencyCode { get; set; }
        public string HouseNumber { get; set; }
        public string MasterNumber { get; set; }
        public string MainEntityReference { get; set; }
        public string Shipper { get; set; }
        public string Consignee { get; set; }
        public string Carrier { get; set; }
        public string CarrierNumber { get; set; }
        public string DescriptionOfGoods { get; set; }
        public string FromLocation { get; set; }
        public string ToLocation { get; set; }
        public string FinalDestination { get; set; }
        public string Volume { get; set; }
        public string GrossWeight { get; set; }
        public string ChargeableWeight { get; set; }
        public string PackagesQuantity { get; set; }
        public string SubTotalInInvoiceCurrency { get; set; }
        public string SubTotalInLocalCurrency { get; set; }
        public string AmountInInvoiceCurrency { get; set; }
        public string AmountInLocalCurrency { get; set; }
        public string AmountInProfitCurrency { get; set; }
        public string TotalVAT { get; set; }
        public string TotalVATLocal { get; set; }
        public string VatableAmount { get; set; }
        public string VatableAmountLocal { get; set; }
        public string LoadingPlace { get; set; }
        public string PlaceOfDelivery { get; set; }
        public string PackagesType { get; set; }
        public string DeliveryFrom { get; set; }
        public string DeliveryTo { get; set; }
        public string Incoterm { get; set; } //(Code ) 
        public int? DeliveryPackagesQuantity { get; set; }
        public double? DeliveryPackagesWeight { get; set; }
        public string DeliveryTrailerNo { get; set; }
        public string DeliveryDriverName { get; set; }
        public DateTime? MainCarriageATD { get; set; }
        public string ShipmentNumber { get; set; }
        public string ShipmentRouting { get; set; }
    }

    public class ReportInvoiceLine
    {
        public string Description { get; set; }
        public string Quantity { get; set; }
        public string Measurement { get; set; }
        public string UnitPrice { get; set; }
        public double UnitPriceDouble { get; set; }
        public string LocalAmount { get; set; }
        public string ForeignAmount { get; set; }
        public string InvoiceAmount { get; set; }
        public string ForeignCurrency { get; set; }
        public string ForeignCurrencySign { get; set; }
        public string VatType { get; set; }
        public string VatTypeLocalName { get; set; }
        public string VatTypePercentage { get; set; }
        public string ChargeType { get; set; }
        public string ChargeTypeLocalName { get; set; }
        public string LocalDescription { get; set; }
        public string VatAmountInLocalCurrency { get; set; }
        public string VatAmountInInvoiceCurrency { get; set; }
        public string VatAmountIncludeMultiInInvoiceCurrency { get; set; }

        public string VatIndication { get; set; }
        public string ChrageTypeCode { get; set; }
        public string DebitInLocalAmount { get; set; }
        public string DebitInInvoiceAmount { get; set; }
        public string DebitInForeignAmount { get; set; }
        public string CreditInLocalAmount { get; set; }
        public string CreditInInvoiceAmount { get; set; }
        public string CreditInForeignAmount { get; set; }
        public string Notes { get; set; }
        public string DescriptionAndNotes { get; set; }
        public string UOMPercentage { get; set; }
        public bool IsExpense { get; set; }

        public double? LocalAmount_Double { get; set; }
        public double? InvoiceAmount_Double { get; set; }
        public double? CalculatedUnitPrice { get; set; }
        public double? ForeignAmount_Double { get; set; }

        public string ClaveUnidad { get; set; }
        public string ClaveProdServ { get; set; }

        public string VATableAmountInInvoiceCurrency { get; set; }
        public string NONVATableAmountInInvoiceCurrency { get; set; }

        public double? VATableAmountInInvoiceCurrency_double { get; set; }
        public double? NONVATableAmountInInvoiceCurrency_double { get; set; }

        public string VatAmountInForeignCurrency { get; set; }
        public string ForeignToInvoiceExchangeRate { get; set; }

        public double? LocalAmountWithVAT { get; set; }
        public string VATDescription { get; set; }
        public string VATLocalDescription { get; set; }
        public bool IsRegionalTax { get; set; }
        public string ChargeTypeDescription { get; set; }
    }

    public class TotalVat
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public string TypeLocalName { get; set; }
        public string LocalVatAmout { get; set; }
        public string InvoiceCurrencyVatAmount { get; set; }
        public string Percentage { get; set; }
        public string Description { get; set; }
        public string LocalDescription { get; set; }
        public string TotalVatAmountInLocalCurrency { get; set; }
        public string TotalVatAmountInInvoiceCurrency { get; set; }
        public double VATAmount { get; set; }
        public double VatableAmount { get; set; }

        public double? TotalVatAmountInLocalCurrency_Double { get; set; }
        public double? TotalVatAmountInInvoiceCurrency_Double { get; set; }
    }

    public class StoragePricing
    {
        public string Id { get; set; }
        public string WarehouseName { get; set; }
        public int StepFrom { get; set; }
        public int? StepTo { get; set; }
        public int? Days { get; set; }
        public decimal? SalePrice { get; set; }
        public decimal? Amount { get; set; }
        public int LineNumber { get; set; }
    }
}
