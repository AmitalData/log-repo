using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Mapping
{
    public class DocumentsFilingsViewMap : EntityTypeConfiguration<DocumentsFilingsView>
    {
        public DocumentsFilingsViewMap()
        {
            this.HasKey(t => new { t.Id });

            this.Property(t => t.Id).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.Tenant).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("DocumentsFilingsView");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.ChildEntityId).HasColumnName("ChildEntityId");
            this.Property(t => t.ChildEntityReference).HasColumnName("ChildEntityReference");
            this.Property(t => t.ChildObjectTableId).HasColumnName("ChildObjectTableId");
            this.Property(t => t.CustomerDocumentId).HasColumnName("CustomerDocumentId");
            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.CustomerTenantNumber).HasColumnName("CustomerTenantNumber");
            this.Property(t => t.CustomsDocId).HasColumnName("CustomsDocId");
            this.Property(t => t.CreateDate).HasColumnName("CreateDate");
            this.Property(t => t.CreatedByUserId).HasColumnName("CreatedByUserId");
            this.Property(t => t.CreatedByUserName).HasColumnName("CreatedByUserName");
            //this.Property(t => t.CustomsDocumentTypeCode).HasColumnName("CustomsDocumentTypeCode");
            //this.Property(t => t.CustomsDocumentTypeName).HasColumnName("CustomsDocumentTypeName");
            this.Property(t => t.DeleteDateTime).HasColumnName("DeleteDateTime");
            this.Property(t => t.DeletedByUserId).HasColumnName("DeletedByUserId");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.DirectionCode).HasColumnName("DirectionCode");
            this.Property(t => t.DocumentId).HasColumnName("DocumentId");
            this.Property(t => t.DocumentTypeCode).HasColumnName("DocumentTypeCode");
            this.Property(t => t.DocumentTypeId).HasColumnName("DocumentTypeId");
            this.Property(t => t.DocumentTypeName).HasColumnName("DocumentTypeName");
            this.Property(t => t.DoucmentTypeTemplateFormatCode).HasColumnName("DoucmentTypeTemplateFormatCode");
            this.Property(t => t.EntityId).HasColumnName("EntityId");
            this.Property(t => t.EntityReference).HasColumnName("EntityReference");
            this.Property(t => t.Extension).HasColumnName("Extension");
            //this.Property(t => t.ExternalCode).HasColumnName("ExternalCode");
            this.Property(t => t.ExternalEntityName).HasColumnName("ExternalEntityName");
            this.Property(t => t.ExternalEntityReference).HasColumnName("ExternalEntityReference");
            this.Property(t => t.FileName).HasColumnName("FileName");
            this.Property(t => t.FileSize).HasColumnName("FileSize");
            this.Property(t => t.Folder).HasColumnName("Folder");
            this.Property(t => t.FolderId).HasColumnName("FolderId");
            //this.Property(t => t.FollowUpCount).HasColumnName("FollowUpCount");
            //this.Property(t => t.FollowUpId).HasColumnName("FollowUpId");
            this.Property(t => t.ForwarderDocumentId).HasColumnName("ForwarderDocumentId");
            this.Property(t => t.HasCopies).HasColumnName("HasCopies");
            this.Property(t => t.HasFile).HasColumnName("HasFile");
            //this.Property(t => t.HasFollowUp).HasColumnName("HasFollowUp");
            this.Property(t => t.IsAgentView).HasColumnName("IsAgentView");
            this.Property(t => t.IsCustomerView).HasColumnName("IsCustomerView");
            this.Property(t => t.IsDeleted).HasColumnName("IsDeleted");
            this.Property(t => t.IsDigitallySigned).HasColumnName("IsDigitallySigned");
            //this.Property(t => t.IsMetaDataReady).HasColumnName("IsMetaDataReady");
            this.Property(t => t.IsSharedWithCustomer).HasColumnName("IsSharedWithCustomer");
            this.Property(t => t.IsSharedWithForwarder).HasColumnName("IsSharedWithForwarder");
            //this.Property(t => t.LastVersion).HasColumnName("LastVersion");
            //this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.Notes).HasColumnName("Notes");
            this.Property(t => t.ObjectTableId).HasColumnName("ObjectTableId");
            this.Property(t => t.OwnerId).HasColumnName("OwnerId");
            this.Property(t => t.OwnerName).HasColumnName("OwnerName");
            this.Property(t => t.Received).HasColumnName("Received");
            this.Property(t => t.SearchFields).HasColumnName("SearchFields");
            this.Property(t => t.ReceivedByUserId).HasColumnName("ReceivedByUserId");
            this.Property(t => t.ReceivedDate).HasColumnName("ReceivedDate");
            this.Property(t => t.SecurityId).HasColumnName("SecurityId");
            this.Property(t => t.SignersList).HasColumnName("SignersList");
            this.Property(t => t.UpdateDate).HasColumnName("UpdateDate");
            this.Property(t => t.UpdatedByUserId).HasColumnName("UpdatedByUserId");
            this.Property(t => t.StatusCode).HasColumnName("StatusCode");
        }
    }
}
