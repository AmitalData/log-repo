using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace WebFreight.Web.DataProviders
{
    public class ArchivoExportadoDataProvider: BaseDataProvider
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

        public string InvoiceNumber { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public string InvoiceCurrencyCode { get; set; }
        public double? InvoiceCurrencyRate { get; set; }
        public string CardCode { get; set; }
        public string CardName { get; set; }
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
    }
}