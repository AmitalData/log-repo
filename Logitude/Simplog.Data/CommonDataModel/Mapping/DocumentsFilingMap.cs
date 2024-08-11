using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Mapping
{
    public class DocumentsFilingMap : EntityTypeConfiguration<DocumentsFiling>
    {
        public DocumentsFilingMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(40)
                .IsUnicode(false);

            this.Property(t => t.DocumentId)
              .HasMaxLength(15)
              .IsUnicode(false);

            this.Property(t => t.Code)
              .HasMaxLength(30)
              .IsRequired()
              .IsUnicode(false);

            this.Property(t => t.DocumentTypeId)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.DirectionCode)
               .IsRequired()
               .HasMaxLength(1)
               .IsUnicode(false);

            this.Property(t => t.EntityId)
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.ObjectTableId)
               .HasMaxLength(15)
               .IsUnicode(false);

            this.Property(t => t.ChildEntityId)
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.ChildEntityReference)
                .HasMaxLength(40)
                .IsUnicode(false);

            this.Property(t => t.ChildObjectTableId)
             .HasMaxLength(15)
             .IsUnicode(false);


            this.Property(t => t.Notes)
                .HasMaxLength(250)
                .IsUnicode(true);

            this.Property(t => t.CreatedByUserId)
               .IsRequired()
               .HasMaxLength(15)
               .IsUnicode(false);

            this.Property(t => t.OwnerId)

               .HasMaxLength(15)
               .IsUnicode(false);

            this.Property(t => t.UpdatedByUserId)
                .IsRequired()
               .HasMaxLength(15)
               .IsUnicode(false);

            this.Property(t => t.Description)
               .HasMaxLength(256)
               .IsUnicode(true);

            this.Property(t => t.SearchFields)
                .HasMaxLength(1000)
                .IsUnicode(true);


            this.Property(t => t.ReceivedByUserId)
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.ReceivedByByContactId)
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.StatusCode)
               .HasMaxLength(4)
               .IsUnicode(false);

            //this.Property(t => t.ExternalCode)
            //  .HasMaxLength(40)
            //  .IsUnicode(false);


            this.Property(t => t.EntityReference)
              .HasMaxLength(40)
              .IsUnicode(false);

            this.Property(t => t.ExternalEntityName)
              .HasMaxLength(24)
              .IsUnicode(true);

            this.Property(t => t.ExternalEntityReference)
            .HasMaxLength(35)
            .IsUnicode(false);

            this.Property(t => t.DepartmentId)

             .HasMaxLength(15)
             .IsUnicode(false);

            this.Property(t => t.BranchId)

                .HasMaxLength(15)
                .IsUnicode(false);


            this.Property(t => t.FolderId)

          .HasMaxLength(15)
          .IsUnicode(false);


            this.Property(t => t.DeletedByUserId)
              .HasMaxLength(15)
              .IsUnicode(false);


            this.Property(t => t.SignersList)
             .HasMaxLength(256)
             .IsUnicode(true);


            this.Property(t => t.ForwarderDocumentId)
           .HasMaxLength(40)
           .IsUnicode(false);


            this.Property(t => t.CustomerDocumentId)
           .HasMaxLength(40)
           .IsUnicode(false);

            this.Property(t => t.ComputedCustomerDocumentId)
           .HasMaxLength(40)
           .IsUnicode(false);

            this.Property(t => t.SecurityId)
                .HasMaxLength(60)
                .IsUnicode(false);

            this.Property(t => t.SignRequestByUserEmail)
                     .HasMaxLength(256)
                     .IsUnicode(true);

            this.Property(t => t.OrigionalDocumentId)
              .HasMaxLength(15)
              .IsUnicode(false);

            this.Property(t => t.ComputedForwarderDocumentId).HasMaxLength(40).IsUnicode(false);

            this.Property(t => t.EntityNumber)
               .HasMaxLength(20)
               .IsUnicode(false);

            this.Property(t => t.IsDigitalSignRequired)
                .IsRequired();

            this.Property(t => t.ReceivedByPartner)
           .HasMaxLength(25)
           .IsUnicode(false);

            this.Property(t => t.BillToId)
          .HasMaxLength(60)
          .IsUnicode(false);

            // Table & Column Mappings
            this.ToTable("DocumentsFilings");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.CustomerTenantNumber).HasColumnName("CustomerTenantNumber");
            this.Property(t => t.DocumentId).HasColumnName("DocumentId");
            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.DocumentTypeId).HasColumnName("DocumentTypeId");
            this.Property(t => t.DirectionCode).HasColumnName("DirectionCode");
            this.Property(t => t.EntityId).HasColumnName("EntityId");
            this.Property(t => t.ObjectTableId).HasColumnName("ObjectTableId");
            this.Property(t => t.ChildEntityId).HasColumnName("ChildEntityId");
            this.Property(t => t.ChildEntityReference).HasColumnName("ChildEntityReference");
            this.Property(t => t.ChildObjectTableId).HasColumnName("ChildObjectTableId");
            this.Property(t => t.Notes).HasColumnName("Notes");
            this.Property(t => t.CreatedByUserId).HasColumnName("CreatedByUserId");
            this.Property(t => t.CreateDate).HasColumnName("CreateDate");
            this.Property(t => t.OwnerId).HasColumnName("OwnerId");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.SearchFields).HasColumnName("SearchFields");
            this.Property(t => t.HasCopies).HasColumnName("HasCopies");
            this.Property(t => t.CancellSignRequest).HasColumnName("CancellSignRequest");
            this.Property(t => t.Received).HasColumnName("Received");
            this.Property(t => t.UpdatedByUserId).HasColumnName("UpdatedByUserId");
            this.Property(t => t.UpdateDate).HasColumnName("UpdateDate");
            this.Property(t => t.SignDueDate).HasColumnName("SignDueDate");

            this.Property(t => t.ReceivedDate).HasColumnName("ReceivedDate");
            this.Property(t => t.ReceivedByUserId).HasColumnName("ReceivedByUserId");
            this.Property(t => t.ReceivedByByContactId).HasColumnName("ReceivedByByContactId");
            this.Property(t => t.StatusCode).HasColumnName("StatusCode");



            this.Property(t => t.EntityReference).HasColumnName("EntityReference");
            this.Property(t => t.ExternalEntityName).HasColumnName("ExternalEntityName");
            this.Property(t => t.ExternalEntityReference).HasColumnName("ExternalEntityReference");
            this.Property(t => t.DepartmentId).HasColumnName("DepartmentId");
            this.Property(t => t.BranchId).HasColumnName("BranchId");
            this.Property(t => t.FolderId).HasColumnName("FolderId");

            this.Property(t => t.IsDeleted).HasColumnName("IsDeleted");
            this.Property(t => t.DeletedByUserId).HasColumnName("DeletedByUserId");
            this.Property(t => t.DeleteDateTime).HasColumnName("DeleteDateTime");

            this.Property(t => t.IsDigitallySigned).HasColumnName("IsDigitallySigned");
            this.Property(t => t.IsSharedWithForwarder).HasColumnName("IsSharedWithForwarder");
            this.Property(t => t.IsSharedWithCustomer).HasColumnName("IsSharedWithCustomer");
            this.Property(t => t.SignersList).HasColumnName("SignersList");
            this.Property(t => t.SecurityId).HasColumnName("SecurityId");
            this.Property(t => t.LastVersion).HasColumnName("LastVersion");

            this.Property(t => t.IsRequested).HasColumnName("IsRequested");
            this.Property(t => t.SignRequestByUserEmail).HasColumnName("SignRequestByUserEmail");
            this.Property(t => t.OrigionalDocumentId).HasColumnName("OrigionalDocumentId");

            this.Property(t => t.LastShareDate).HasColumnName("LastShareDate");
            this.Property(t => t.IsSharedIn).HasColumnName("IsSharedIn");
            this.Property(t => t.IsSharedOut).HasColumnName("IsSharedOut");
            this.Property(t => t.EntityNumber).HasColumnName("EntityNumber");

            this.Property(t => t.IsDigitalSignRequired).HasColumnName("IsDigitalSignRequired");
            this.Property(t => t.BackedupExternally).HasColumnName("BackedupExternally");
            this.Property(t => t.LastBackupDate).HasColumnName("LastBackupDate");
            this.Property(t => t.IsTransferdToQBO).HasColumnName("IsTransferdToQBO");
            this.Property(t => t.ReceivedByPartner).HasColumnName("ReceivedByPartner");

            this.HasOptional(t => t.Document)
                .WithMany()
                .HasForeignKey(d => d.DocumentId);

            this.HasRequired(t => t.DocumentType)
                .WithMany()
                .HasForeignKey(d => d.DocumentTypeId)
                .WillCascadeOnDelete(false);

            this.HasRequired(t => t.CreatedByUser)
                .WithMany()
                .HasForeignKey(d => d.CreatedByUserId);

            this.HasRequired(t => t.Owner)
               .WithMany()
               .HasForeignKey(d => d.OwnerId);

            this.HasOptional(t => t.ObjectTable)
               .WithMany()
               .HasForeignKey(d => d.ObjectTableId);

            this.HasOptional(t => t.ChildObjectTable)
             .WithMany()
             .HasForeignKey(d => d.ChildObjectTableId);

            this.HasRequired(t => t.UpdatedByUser)
               .WithMany()
               .HasForeignKey(d => d.UpdatedByUserId);

            this.HasOptional(t => t.ReceivedByUser)
              .WithMany()
              .HasForeignKey(d => d.ReceivedByUserId);

            this.HasOptional(t => t.ReceivedByByContact)
               .WithMany()
               .HasForeignKey(d => d.ReceivedByByContactId);

            this.HasOptional(t => t.DocumentStatus)
             .WithMany()
             .HasForeignKey(d => d.StatusCode);

            this.HasOptional(t => t.Branch).WithMany().HasForeignKey(d => d.BranchId);

            this.HasOptional(t => t.Department).WithMany().HasForeignKey(d => d.DepartmentId);

            this.HasOptional(t => t.Folder)
              .WithMany()
              .HasForeignKey(d => d.FolderId);


            this.HasOptional(t => t.DeletedByUser)
              .WithMany()
              .HasForeignKey(d => d.DeletedByUserId);

        }
    }
}

