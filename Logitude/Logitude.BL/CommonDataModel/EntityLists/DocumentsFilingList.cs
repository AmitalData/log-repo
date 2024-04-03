using System;
using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.CommonDataModel.EntityLists
{
    public class DocumentsFilingList
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string DocumentId { get; set; }
        public string Code { get; set; }
        public string DocumentTypeId { get; set; }
        public string DirectionCode { get; set; }
        public string EntityId { get; set; }
        public string ObjectTableId { get; set; }
        public string ChildEntityId { get; set; }
        public string ChildEntityReference { get; set; }
        public string ChildObjectTableId { get; set; }
        public string Notes { get; set; }
        public string CreatedByUserId { get; set; }
        public string OwnerId { get; set; }
        public string Description { get; set; }
        public string SearchFields { get; set; }
        public DateTime CreateDate { get; set; }
        public bool HasCopies { get; set; }
        public string StatusCode { get; set; }

        public string CalculatedFileName { get; set; }

        public string FollowUpId { get; set; }
        public string Extension { get; set; }
        public bool HasFile { get; set; }
        public string CreatedByUserName { get; set; }
        public int FollowUpCount { get; set; }
        public string DocumentTypeName { get; set; }
        public string DocumentTypeCode { get; set; }
        public string DoucmentTypeTemplateFormatCode { get; set; }
        public bool HasFollowUp { get; set; }

        public double? FileSize { get; set; }
        public string FileName { get; set; }
        public string Name { get; set; }
        // dummy
        public bool IsAgentView { get; set; }
        public bool IsCustomerView { get; set; }

        public string CustomsDocumentTypeName { get; set; }

        public string CustomsDocumentTypeCode { get; set; }
        public bool IsMetaDataReady { get; set; }

        public bool Received { get; set; }

        public string ReceivedByUserId { get; set; }
        public DateTime? ReceivedDate { get; set; }

        public string Folder { get; set; }

        public DateTime? UpdateDate { get; set; }
        public string UpdatedByUserId { get; set; }

        public string OwnerName { get; set; }
        public string ExternalCode { get; set; }
        public string EntityReference { get; set; }
        public string ExternalEntityName { get; set; }
        public string ExternalEntityReference { get; set; }

        public bool IsDeleted { get; set; }
        public string DeletedByUserId { get; set; }
        public DateTime? DeleteDateTime { get; set; }
        public string FolderId { get; set; }
        public bool IsDigitallySigned { get; set; }
        public string SignersList { get; set; }
        public bool IsSharedWithForwarder { get; set; }
        public bool IsSharedWithCustomer { get; set; }
        public string ForwarderDocumentId { get; set; }
        public string CustomerDocumentId { get; set; }
        public string SecurityId { get; set; }
        public int? CustomerTenantNumber { get; set; }
        public string CustomsDocId { get; set; }
        public string SignRequestByUserEmail { get; set; }
        public bool CancellSignRequest { get; set; }
        public string OrigionalDocumentId { get; set; }
        public bool IsRequested { get; set; }

        public DateTime? LastShareDate { get; set; }
        public bool IsSharedIn { get; set; }
        public bool IsSharedOut { get; set; }
        public DateTime? SignDueDate { get; set; }
        public bool IsDigitalSignRequired { get; set; }
        public bool BackedupExternally { get; set; }
        public string ReceivedByByContactId { get; set; }
        public string ReceivedByPartner { get; set; }
         public string BillToId { get; set; }

    }
}
