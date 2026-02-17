using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Logitude.Customs.Data.DataContracts
{
    [DataContract]
    public class CertificateConnectedItems
    {
        [Key]
        [DataMember]
        public Guid Id { get; set; }
        [DataMember]
        public string DeclarationId { get; set; }
        [DataMember]
        public string InvoiceNumber { get; set; }
        [DataMember]
        public string ItemCode { get; set; }
        [DataMember]
        public string ClassificationCode { get; set; }
        [DataMember]
        public string TradeAgreementCode { get; set; }
        [DataMember]
        public string OriginCountryCode { get; set; }
        [DataMember]
        public string TradeAgreementName { get; set; }
        [DataMember]
        public string OriginCountryName { get; set; }
        [DataMember]
        public string CatalogNumber { get; set; }
        [DataMember]
        public int? SequenceNumeric { get; set; }
        [DataMember]
        public int InvoiceCounterKey { get; set; }
        [DataMember]
        public int LineNumber { get; set; }
        [DataMember]
        public int ItemCertificateCounterKey { get; set; }
        [DataMember]
        public string ReqConfirmationTypeCode { get; set; }
        [DataMember]
        public string CertificateNumber { get; set; }
        [DataMember]
        public string CertificateExemptionTypeCode { get; set; }
        [DataMember]
        public string ResConfirmationTypeCode { get; set; }
        [DataMember]
        public string AttachmentTypeCode { get; set; }
        [DataMember]
        public string CertificateTicketId { get; set; }



    }
}