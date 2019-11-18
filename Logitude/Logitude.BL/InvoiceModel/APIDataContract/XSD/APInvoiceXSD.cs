using Logitude.BL.CommonDataModel.APIDataContract.ApiV1;
using Logitude.BL.InvoiceModel.APIDataContract.ApiV1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Logitude.BL.InvoiceModel.APIDataContract.XSD
{
    [XmlRoot("APInvoice")]
    public class APInvoice
    {
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Id { get; set; }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int Tenant { get; set; }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string InternalNumber { get; set; }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string VATNumber { get; set; }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string InvoiceNumber { get; set; }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string InternalNotes { get; set; }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string HouseNumber { get; set; }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string MasterNumber { get; set; }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Description { get; set; }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string AccountingExternalCode { get; set; }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string CreditAccount { get; set; }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string PaymentTermExternalId { get; set; }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string ExternalAccountingEntityId { get; set; }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string EntityReference { get; set; }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string EntityType { get; set; }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string ComputingPartnerCode { get; set; }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public double InvoiceCurrencyExchangeRate { get; set; }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public double SubTotalInLocalCurrency { get; set; }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public double SubTotalInInvoiceCurrency { get; set; }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public double AmountInLocalCurrency { get; set; }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public double ProfitCurrencyExchangeRate { get; set; }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public double AmountInProfitCurrency { get; set; }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public double AmountDue { get; set; }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public double AmountDueInLocalCurrency { get; set; }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public double AmountDueInProfitCurrency { get; set; }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public double RefundAmount { get; set; }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public double AmountInInvoiceCurrency { get; set; }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public double InvoiceExpectedAmount { get; set; }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool IsExternalEntity { get; set; }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool IsGeneralInvoice { get; set; }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public DateTime InvoiceDate { get; set; }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public DateTime AccountingDate { get; set; }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public DateTime DueDate { get; set; }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public DateTime ExchangeRateDate { get; set; }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public DateTime UpdateDate { get; set; }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public DateTime ApprovedDate { get; set; }

        [System.Xml.Serialization.XmlElementAttribute(IsNullable = false)]
        public Vendor Vendor { get; set; }

        [System.Xml.Serialization.XmlElementAttribute(IsNullable = false)]
        public Currency InvoiceCurrency { get; set; }

        [System.Xml.Serialization.XmlElementAttribute(IsNullable = false)]
        public PaymentTerm PaymentTerm { get; set; }

        [System.Xml.Serialization.XmlElementAttribute(IsNullable = false)]
        public Currency LocalCurrency { get; set; }

        [System.Xml.Serialization.XmlElementAttribute(IsNullable = false)]
        public APInvoiceStatus Status { get; set; }

        [System.Xml.Serialization.XmlArray("InvoiceLines")]
        [System.Xml.Serialization.XmlArrayItem("APInvoiceLine")]
        //[System.Xml.Serialization.XmlElementAttribute(IsNullable = false)]
        public List<APInvoiceLine> InvoiceLines { get; set; }

        public Currency ProfitCurrency { get; set; }
        public User UpdatedByUser { get; set; }
        public Branch Branch { get; set; }
        public APInvoiceTransferStatus TransferStatus { get; set; }
        public User ApprovedByUser { get; set; }
    }
}
