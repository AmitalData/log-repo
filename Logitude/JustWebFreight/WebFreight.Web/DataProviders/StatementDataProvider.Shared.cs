using System;
using System.Collections.Generic;
using System.Linq;


namespace WebFreight.Web.DataProviders
{
    public class StatementDataProvider : BaseDataProvider
    {
        public string CustomerName { get; set; }
        public string Currency { get; set; }

        public double? CurrentDue { get; set; }
        public double? DaysPastDue1_30 { get; set; }
        public double? DaysPastDue31_60 { get; set; }
        public double? DaysPastDue61_90 { get; set; }
        public double? Over90DaysPastDue { get; set; }
        public double? DaysPastDue1_15 { get; set; }
        public double? DaysPastDue16_30 { get; set; }
        public double? DaysPastDue91_120 { get; set; }
        public double? DaysPastDue31_45 { get; set; }
        public double? DaysPastDue46_60 { get; set; }
        public double? DaysPastDue61_75 { get; set; }
        public double? DaysPastDue76_90 { get; set; }
        public double? Over120DaysPastDue { get; set; }

        public string Phone { get; set; }
        public string Fax { get; set; }
        public string BankDetails { get; set; }

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
        public string Name { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string AROrAPFilter { get; set; }
        public string PaymentOrInvoiceFilter { get; set; }
        public List<StatementGroup> StatementGroupList { get; set; } 
        public List<StatementRecord> StatementRecordList { get; set; }
        public List<StatmentAging> StatementAgingSummaryRecordList { get; set; }  
    }

    public class StatementGroup
    {
        public string Currency { get; set; }
        public List<StatementRecord> StatementRecordList { get; set; }
        public List<StatmentAging> StatementAgingSummaryRecordList { get; set; }
    }

    public class StatementRecord
    {
        public DateTime Date { get; set; }
        public string Type { get; set; }
        public DateTime DueDate { get; set; }
        public string YourRefrence { get; set; }
        public string OurRefrence { get; set; }
        public string MasterNumber { get; set; }
        public string HouseNumber { get; set; }
        public string Desicription { get; set; }
        public double Balance { get; set; }
        public double? Debit { get; set; }
        public double CreditWithZero { get; set; }
        public double? Credit { get; set; }
        public double DebitWithZero { get; set; }
        public string Currency { get; set; }
        public double? TotalAmount { get; set; }

        public double? currentDue { get; set; }
        public double? Due1_30 { get; set; }
        public double? Due31_60 { get; set; }
        public double? Due61_90 { get; set; }
        public double? Due90 { get; set; }
        public double? Due1_15 { get; set; }
        public double? Due16_30 { get; set; }
        public double? Due91_120 { get; set; }
        public double? Due31_45 { get; set; }
        public double? Due46_60 { get; set; }
        public double? Due61_75 { get; set; }
        public double? Due76_90 { get; set; }
        public double? Due120 { get; set; }
        public double? total { get; set; }

        public string SupplierName { get; set; }
        public string SupplierRefrence { get; set; }
        public string DescriptionOfGoods { get; set; }
        public string ShipmentId { get; set; }
        public string CurrencyId { get; set; }

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

        public string Notes { get; set; }
        public DateTime? RegisterDate { get; set; }
        public DateTime? ValueDate { get; set; }
        public string PaymentMethod { get; set; }

        public string Shipper { get; set; }
        public string Consignee { get; set; }

        public string BillToVendorId { get; set; }
        public string BillToVendor { get; set; }

        public string InvoiceStatus { get; set; }
        public double? InvoiceAmount { get; set; }
        public double? AmountPaid { get; set; }
        public string BranchId { get; set; }
        public string BranchName { get; set; }

        public double? InvoiceAmountInInvoiceCurrency { get; set; }
        public double? AmountPaidInInvoiceCurrency { get; set; }
        
        public string PaymentStatus { get; set; }
        public double? OriginalAmount { get; set; }
        public string ShipmentNumber { get; set; }
        public string ShipmentDirection { get; set; }
    }

    public class StatmentAging
    {
        public double? currentDue { get; set; }
        public double? Due1_30 { get; set; }
        public double? Due31_60 { get; set; }
        public double? Due61_90 { get; set; }
        public double? Due90 { get; set; }
        public double? Due1_15 { get; set; }
        public double? Due16_30 { get; set; }
        public double? Due91_120 { get; set; }
        public double? Due120 { get; set; }

        public double? Due31_45 { get; set; }
        public double? Due46_60 { get; set; }
        public double? Due61_75 { get; set; }
        public double? Due76_90 { get; set; }

        public string Currency { get; set; }
    }
}