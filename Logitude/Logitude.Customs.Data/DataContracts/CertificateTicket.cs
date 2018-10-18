using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel.DomainServices.Server;
using System.Web;

namespace Logitude.Customs.Data.DataContracts
{
    [DataContract]
    public class CertificateTicket
    {
        [Key]
        [DataMember]
        public string Id { get; set; }
        [DataMember]
        public string AttachmentTypeCode { get; set; }
        [DataMember]
        public string CertificateNumber { get; set; }
        [DataMember]
        public string ResConfirmationTypeCode { get; set; }
        [DataMember]
        public string CertificateExemptionTypeCode { get; set; }
        [DataMember]
        public string ReqConfirmationTypeCode { get; set; }
        [DataMember]
        public string ResConfirmationTypeName { get; set; }
        [DataMember]
        public string ReqConfirmationTypeName { get; set; }
        [DataMember]
        public string AttachmentTypeName { get; set; }
        [DataMember]
        public string CertificateExemptionTypeName { get; set; }
        [DataMember]
        public string CustomsAttachmentId { get; set; }
        [DataMember]
        public string ExternalCertificatCode { get; set; }
        [DataMember]
        public string InvoiceNumber { get; set; }
        [DataMember]
        public string DeclarationId { get; set; }
        [DataMember]
        public string oldAttachment { get; set; }
        [DataMember]
        public string oldCertificateNumber { get; set; }
        [DataMember]
        public string oldResConfirmation { get; set; }
        [DataMember]
        public string oldCertificateExempt { get; set; }

        private List<CertificateConnectedItems> selectedItems;

        [Include]
        [Association("certificateTicketConnections","Id", "CertificateTicketId")]
        [DataMember]
        public List<CertificateConnectedItems> SelectedItems
        {
            get {
                if (selectedItems == null)
                {
                    selectedItems = new List<CertificateConnectedItems>();
                }
                return selectedItems;
            }
            set {
                selectedItems = value;
            }
        }
        [DataMember]
        public bool IsAllSelected;

        [DataMember]
        public string ConnectedItemsKeys { get; set; }

        [DataMember]
        public string ExcludedItemsKeys { get; set; }

    }
}