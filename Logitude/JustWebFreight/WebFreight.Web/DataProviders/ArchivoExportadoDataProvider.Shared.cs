using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace WebFreight.Web.DataProviders
{
    public class ArchivoExportadoDataProvider : BaseDataProvider
    {
        [Key]
        public int Id { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string CurrencyCode { get; set; }
        public string IncludeDraftInvoices { get; set; }
        public string IncludeEstimations { get; set; }
        public List<ArchivoExportadoShipmentItem> Shipments { get; set; }
    }

    public class ArchivoExportadoShipmentItem
    {
        public string ShipmentNumber { get; set; }
        public string LineTypeCode { get; set; }
        public double? Payables { get; set; }
        public double? Receivables { get; set; }
        public string LongMaster { get; set; }
        public string DirectionPartner { get; set; }
        public string DescriptionOfGoods { get; set; }
        public string CustomerExternalID { get; set; }
        public string Customer { get; set; }
        public string Shipper { get; set; }
        public string ShipperNotExporter { get; set; }
        public string Consignee { get; set; }
        public string ConsigneeNotImporter { get; set; }
        public string InvoiceNumber { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public string InvoiceCurrencyCode { get; set; }
        public double? InvoiceCurrencyRate { get; set; }
        public string CardCode { get; set; }
        public string CardName { get; set; }
        public string PartnerName { get; set; }
        public string CardExternal { get; set; }
        public string CreatedByUser { get; set; }
        public string BranchCode { get; set; }
        public string BranchName { get; set; }
        public string BranchLocalName { get; set; }
        public string BranchExternalId { get; set; }
        public string ChargeTypeCode { get; set; }
        public string ChargeTypeName { get; set; }
        public string ChargeTypeLocalName { get; set; }
        public string Salesman { get; set; }
        public string VendorName { get; set; }
        public string BillToName { get; set; }
        public double? OpenPayables { get; set; }
        public double? OpenReceivables { get; set; }
        public double? AccountedPayables { get; set; }
        public string AccountedPayablesCurrencyCode { get; set; }
        public double? AccountedPayablesCurrencyRate { get; set; }
        public double? AccountedReceivables { get; set; }
        public string AccountedReceivablesCurrencyCode { get; set; }
        public double? AccountedReceivablesCurrencyRate { get; set; }
        public string Direction { get; set; }
        public string ShipperConsigneeExternalID { get; set; }
        public DateTime? OperationalDate { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public double? ChargeableWeight { get; set; }
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
        public double? ExpectedPayables { get; set; }
        public double? Profit { get; set; }
        public DateTime? ETD { get; set; }
        public string CustomerRef1 { get; set; }
        public string CustomerRef2 { get; set; }
        public string OriginCode { get; set; }
        public string DestinationCode { get; set; }
        public string CountryOfOrigin { get; set; }
        public string CountryOfDestination { get; set; }
        public string IncotermCode { get; set; }
        public string IncotermName { get; set; }
        public string House { get; set; }
        public string ContainersNumbers { get; set; }
        public string AccountManagerName { get; set; }
        public string ShipmentStatus { get; set; }
        public bool AccountingClosed { get; set; }
        public DateTime? ShipmentCreateDate { get; set; }
        public string ShipmentNotes { get; set; }
        public string ShipmentOpenedBy { get; set; }
        public double? InvoiceAmountDueInLocalCurrency { get; set; }
        public double? InvoiceAmountDueInInvoiceCurrency { get; set; }        
        public double? AccountedReceivablesInInvoiceCurrency { get; set; }
        public double? AccountedPayablesInInvoiceCurrency { get; set; }
    }
}