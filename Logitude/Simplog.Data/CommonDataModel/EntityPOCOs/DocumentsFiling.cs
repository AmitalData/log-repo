using Simplog.Data.InfrastructureModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.EntityPOCOs
{
    public class DocumentsFiling
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
        public bool Received { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string UpdatedByUserId { get; set; }

        public string ReceivedByUserId { get; set; }
        public DateTime? ReceivedDate { get; set; }
        public string StatusCode { get; set; }
        //public string ExternalCode { get; set; }

        

        public string EntityReference { get; set; }
        public string ExternalEntityName { get; set; }
        public string ExternalEntityReference { get; set; }

        public string FolderId  { get; set; }

        public bool IsDeleted { get; set; }
        public string DeletedByUserId { get; set; }
        public DateTime? DeleteDateTime { get; set; }

        public bool IsDigitallySigned { get; set; }
        public string SignersList { get; set; }

        public int LastVersion { get; set; }// field for Itzik-----Mohammad.


        public DateTime? LastShareDate { get; set; }
        public bool IsSharedIn { get; set; }
        public bool IsSharedOut { get; set; }

        //[ForeignKey("FolderId")]
        public virtual DocumentFolder Folder { get; set; }
    

        [ForeignKey("StatusCode")]
        public virtual DocumentStatus DocumentStatus { get; set; }

        [ForeignKey("DocumentId")]
        public virtual Document Document { get; set; }

        [ForeignKey("ObjectTableId")]
        public virtual ObjectTable ObjectTable { get; set; }

        [ForeignKey("ChildObjectTableId")]
        public virtual ObjectTable ChildObjectTable { get; set; }

        [ForeignKey("CreatedByUserId")]
        public virtual User CreatedByUser { get; set; }

        [ForeignKey("UpdatedByUserId")]
        public virtual User UpdatedByUser { get; set; }

        [ForeignKey("OwnerId")]
        public virtual User Owner { get; set; }

        [ForeignKey("DocumentTypeId")]
        public virtual DocumentType DocumentType { get; set; }

        [ForeignKey("ReceivedByUserId")]
        public virtual User ReceivedByUser { get; set; }


        public virtual DocumentOut DocumentOut { get; set; }


        public string BranchId { get; set; }
        [ForeignKey("BranchId")]
        public virtual Branch Branch { get; set; }

        public string DepartmentId { get; set; }
        [ForeignKey("DepartmentId")]
        public virtual Department Department { get; set; }


        [ForeignKey("DeletedByUserId")]
        public virtual User DeletedByUser { get; set; }

        public bool IsSharedWithForwarder { get; set; }
        public bool IsSharedWithCustomer { get; set; }
        public string ForwarderDocumentId { get; set; }
        public string CustomerDocumentId { get; set; }

        public string SecurityId  { get; set; }

        public int? CustomerTenantNumber { get; set; }

     
       
        public bool IsRequested { get; set; }

        public string SignRequestByUserEmail { get; set; }

        public bool CancellSignRequest { get; set; }

        public string OrigionalDocumentId { get; set; }

        public string ComputedForwarderDocumentId { get; set; }

        public DateTime? SignDueDate { get; set; }

        public string EntityNumber { get; set; }

        public bool IsDigitalSignRequired { get; set; }
        public bool BackedupExternally { get; set; }
        public DateTime? LastBackupDate { get; set; }


    }
}
