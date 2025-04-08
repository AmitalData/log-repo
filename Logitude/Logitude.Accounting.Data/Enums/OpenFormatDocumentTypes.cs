using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.Data.Enums
{
    public struct OpenFormatDocumentTypes
    {
        public const string Order = "100";
        public const string DeliveryNote = "200";
        public const string AgentDeliveryNote = "205";
        public const string ReturnNote = "210";

        public const string Invoice = "300";
        public const string TaxInvoice = "305";
        public const string ConsolidateInvoice = "310";
        public const string TaxInvoiceReceipt = "320";
        public const string CreditTaxInvoice = "330";
        public const string ReservationInvoice = "340";

        public const string AgentInvoice = "345";
        public const string Receipt = "400";
        public const string DonationReceipt = "405";
        public const string CashOut = "410";
        public const string BankDeposit = "420";

        public const string PurchaseOrder = "500";
        public const string PurchaseDeliveryNote = "600";

        public const string PurchaseReturn = "610";
        public const string PurchaseTaxInvoice = "700";
        public const string PurchaseCredit = "710";
        public const string OpeningBalance = "800";
        public const string GeneralInventoryEntry = "810";
        public const string GeneralInventoryIssue = "820";
        public const string TransferBetweenWarehouses = "830";
        public const string UpdateFollowingCount = "840";

        public const string ProductionInputReport = "900";
        public const string ProductionOutputReport = "910";
        public const string Fallback = "910";
    }
}
