using System;
using System.Collections.Generic;
using System.Linq;

namespace WebFreight.Web.DataProviders
{
    public class APInvoiceDataProvider : BaseDataProvider
    {
        public DateTime? InvoiceDate { get; set; }
        public DateTime? DueDate { get; set; }
        public string VatNumber { get; set; }
        public string PaymentTerm { get; set; }
        public string Status { get; set; }
        public string InvoiceNumber { get; set; }
        public string InternalNumber { get; set; }
        public string ShipmentNumber { get; set; }
        public string VendorNumber { get; set; }
        public string AccountingNumber { get; set; }
        public string VendorAddress { get; set; } // ??
        public string VendorVatNumber { get; set; }
        public string ShipperName { get; set; }
        public string ShipperAddress { get; set; }
        public string ConsigneeName { get; set; }
        public string ConsigneeAddress { get; set; }
        public string MainCarriageCarrierLabel { get; set; }
        public string MainCarriageCarrierName { get; set; }
        public string MainCarriageVesselLabel { get; set; }
        public string MainCarriageVesselName { get; set; }
        public string FromLocation { get; set; }
        public string ToLocation { get; set; }
        public string FinalLocationLabel { get; set; }
        public string FinalLocation { get; set; }
        public string MainCarriageMAWBLabel { get; set; }
        public string MainCarriageMAWB { get; set; }
        public string HouseNumberLabel { get; set; }
        public string HouseNumber { get; set; }
        public string MainCarriageCarrierNumberLabel { get; set; }
        public string MainCarriageCarrierNumber { get; set; }
        public double? ChargeableWeight { get; set; }
        public string ChargeableWeightUnitCode { get; set; }
        public double? GrossWeight { get; set; }
        public string GrossWeightUnitCode { get; set; }
        public int? NumberofPackages { get; set; }
        public string ContainersLabel { get; set; }
        public string ContainersNumbersArray { get; set; }
        public string DescriptionOfGoods { get; set; }
        public string Notes { get; set; }
        public string BankDetails { get; set; }
        public string IssuedByUser { get; set; }
        public string Signature { get; set; }
        public string InvoiceSection1 { get; set; }
        public string InvoiceSection2 { get; set; }
        public string InvoiceCurrency { get; set; }
        public double? SubTotalInvoiceCurr { get; set; }
        public double? TotalInvoiceCurr { get; set; }
        public string ApprovedBy { get; set; }
        public string ApprovedDate { get; set; }
        public DateTime? ApprovedDateAsDateFormat { get; set; }
        public string CreatedByUser { get; set; }
        public string IRSPlace { get; set; }
        public string IRSNumber { get; set; }
      
        public string VendorBankName { get; set; }
        public string VendorBankAddress { get; set; }
        public string VendorSwift { get; set; }
        public string VendorBankAccountNumber { get; set; }
        public string VendoIBANNo { get; set; }
        public string VendorName { get; set; }

        public string ReleasingAgentName { get; set; }
        public string ReleasingAgentAddress { get; set; }
        public string BranchAddress { get; set; }

        public List<APReportInvoiceLine> APInvoiceLinesList { get; set; }
        public List<APTotalVat> APTotalVatList { get; set; }
        public List<APInvoiceMultipleEntity> APInvoiceMultipleEntityList { get; set; }
    }

    public class APReportInvoiceLine
    {
        public string ChargeTypeCode { get; set; }
        public string ChargeTypeName { get; set; }
        public string VatTypeName { get; set; }
        public double? VatTypePercentage { get; set; }
        public double? ExpectedAmount { get; set; }
        public double? OtherInvoicesAmount { get; set; }
        public double? ForeignAmount { get; set; }
        public string ForeignCurrency { get; set; }
        public double? InvoiceAmount { get; set; }
        public double? OpenAmount { get; set; }
    }

    public class APTotalVat
    {
        public string Type { get; set; }
        public double? Percentage { get; set; }
        public double? TotalVatAmountInInvoiceCurrency { get; set; }
    }

    public class APInvoiceMultipleEntity
    {
        public string MasterNumber { get; set; }
        public string HouseNumber { get; set; }
        public string ShipmentNumber { get; set; }
        public string PartnerName { get; set; }         
        public double? ExpectedAmount { get; set; }
        public double? OpenAmount { get; set; }
        public double? Total { get; set; }
        public double? TotalVAT { get; set; }
    }
}