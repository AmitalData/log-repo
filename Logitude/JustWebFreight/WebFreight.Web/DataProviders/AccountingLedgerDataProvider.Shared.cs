using System;
using System.Collections.Generic;
using System.Linq;


namespace WebFreight.Web.DataProviders
{
    public class AccountingLedgerDataProvider: BaseDataProvider
    {
        public string CustomerName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string CustomerState { get; set; }
        public string ZIPCode { get; set; }
        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }
        public string Phone { get; set; }
        public string Name { get; set; }
        public DateTime? FromPeriod { get; set; }
        public DateTime? ToPeriod { get; set; }
        public string TenantName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Signature { get; set; }
        public string TenantPhone { get; set; }
        public string TenantFax { get; set; }
        public string DateType { get; set; }
        public byte[] Logo { get; set; }

        public List<AccountingLedger_Customer> AccountingLedgerList_Customer { get; set; }        
    }

    public class AccountingLedger_Customer
    {
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        public string CardCode { get; set; }
        public List<AccountingLedger> AccountingLedgerList { get; set; }
    }

    public class AccountingLedger
    {
        public DateTime? CreateDate { get; set; }
        public DateTime? DueDate { get; set; }
        public string ReferenceNumber { get; set; }
        public string ReferenceType { get; set; }
        public double? Debit { get; set; }
        public double? Credits { get; set; }
        public double? AccountBanalnce { get; set; }
        public string Currency { get; set; }
        public double? Total { get; set; }
        public double? opentotal { get; set; }
        public string ShipperReference1 { get; set; }
        public string ShipperReference2 { get; set; }
        public string ShipmentNumber { get; set; }
        public string Notes { get; set; }
        public DateTime? RegisterDate { get; set; }
        public DateTime? ValueDate { get; set; }
        public string PaymentMethod { get; set; }
        public string CustomerId { get; set; }
        public string BillToVendor { get; set; }
        public string HouseNumber { get; set; }
        public string MasterNumber { get; set; }
        public string BranchId { get; set; }
        public string BranchName { get; set; }
        public string Description { get; set; } 
        public double? DebitInLocalCurrency { get; set; }
        public double? CreditInLocalCurrency { get; set; }
        public double? AccountBalanceInLocalCurrency { get; set; }
    }
}
