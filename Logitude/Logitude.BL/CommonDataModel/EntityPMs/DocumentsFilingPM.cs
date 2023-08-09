using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Simplog.Server.Infrastructure;
using System.ComponentModel.DataAnnotations;
using System.ServiceModel.DomainServices.Server;

namespace Logitude.BL.CommonDataModel.EntityPMs
{
    public class DocumentsFilingPM
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string DocumentId { get; set; }
        public string Code { get; set; }
        public string DocumentTypeId { get; set; }
        public string DocumentTypeName { get; set; }
        public string DocumentTypeCode { get; set; }
        public string DoucmentTypeTemplateFormatCode { get; set; }
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
        public bool NoAddToTasksQueue { get; set; }
        // Dummy  prop
        public string FollowUpId { get; set; }
        public string FileExtension { get; set; }
        public bool HasFile { get; set; }
        public string CreatedByUserName { get; set; }
        public int FollowUpCount { get; set; }
        public bool IsFromUnifreightPodMobile { get; set; }
        
        public bool HasFollowUp { get; set; }
        public string OwnerName { get; set; }
        public double? FileSize { get; set; }
        public string FileName { get; set; }
        public string CalculatedFileName { get; set; }
        

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


        public DateTime? UpdateDate { get; set; }
        public string UpdatedByUserId { get; set; }
        public string ReceivedByUserName { get; set; }
        public string StatusCode { get; set; }

        public bool IsHybrid { get; set; }

        public string EntityReference { get; set; }
        public string ExternalEntityName { get; set; }
        public string ExternalEntityReference { get; set; }
        public string Folder { get; set; }

        public byte[] FileData { get; set; }

        public string EntityNumber { get; set; }
        public string ObjectTableName { get; set; }

        public bool IsAttachment { get; set; }
        public string BranchId { get; set; }
        public string DepartmentId { get; set; }

        public string CreatedByUserCode { get; set; }
        public string UpdatedByUserCode { get; set; }
        public string OwnerUserCode { get; set; }
        public string ReceivedByUserCode { get; set; }



        public bool IsDeleted { get; set; }
        public string DeletedByUserId { get; set; }
        public DateTime? DeleteDateTime { get; set; }
        public string FolderId { get; set; }

        public bool IsDigitallySigned { get; set; }
        public bool IsSharedWithForwarder { get; set; }
        public bool IsSharedWithCustomer { get; set; }
        public string ForwarderDocumentId { get; set; }
        public string CustomerDocumentId { get; set; }

        public string SignersList { get; set; }

        public string ChildEntityName { get; set; }
        public bool IsAttachSelect { get; set; }

        public string DivSelectBackground { get; set; }
        public int LastVersion { get; set; }// field for Itzik-----Mohammad.


        public bool IsUoloadedField { get; set; }// field for Itzik-----Mohammad.


        public DateTime? LastShareDate { get; set; }
        public bool IsSharedIn { get; set; }
        public bool IsSharedOut { get; set; }

        public bool IsUpdateSharedDocument { get; set; }
        private List<DocumentsFilingMetaDataValuePM> documentsFilingMetaDataValues;
        [Include]
        [Composition]
        [Association("DocumentsFilingPMDocumentsFilingMetaDataValuePM", "Id", "DocumentsFilingId")]
        public virtual List<DocumentsFilingMetaDataValuePM> DocumentsFilingMetaDataValues
        {
            get
            {
                if (documentsFilingMetaDataValues == null)
                {
                    documentsFilingMetaDataValues = new List<DocumentsFilingMetaDataValuePM>();
                }
                return documentsFilingMetaDataValues;
            }
            set { documentsFilingMetaDataValues = value; }
        }

        public string CustomsDocumentStatusCode { get; set; }

        public string ExternalAttachmentId { get; set; }

        public string DocumentCategoryCode { get; set; }

        public string DocumentCategoryName { get; set; }

        public bool DontAddToQueue { get; set; }

        public string CreateDateWords { get; set; }// Today, Last 7 Days, Older

        public string SecurityId { get; set; }

        public int? CustomerTenantNumber { get; set; }

        public string[] BlockIdsList { get; set; }
        public byte[] buffer { get; set; }
        public int BufferNumber { get; set; }
        public long SentSize { get; set; }
        public string FullFileName { get; set; }
        public bool IsRequested { get; set; }
        public string SignRequestByUserEmail { get; set; }
        public bool CancellSignRequest { get; set; }
        public string OrigionalDocumentId { get; set; }
        public DateTime? SignDueDate { get; set; }
        public bool DontDeleteRealFile { get; set; }
        public bool IsCustomReference { get; set; }
        public string CustomReference { get; set; }
        public bool IsDigitalSignRequired { get; set; }
        public bool BackedupExternally { get; set; }
        public string OcrStatusCode { get; set; }
        public decimal OcrScore { get; set; }
        public string OcrReference { get; set; }


    }
}
