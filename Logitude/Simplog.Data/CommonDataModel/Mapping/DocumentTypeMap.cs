using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System.Configuration;

namespace Simplog.Data.CommonDataModel.Mapping
{
    public class DocumentTypeMap : EntityTypeConfiguration<DocumentType>
    {
        public DocumentTypeMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(40)
                .IsUnicode(true);

            this.Property(t => t.Code)
                .IsRequired()
                .HasMaxLength(7)
                .IsUnicode(false);

            this.Property(t => t.Notes)
                .HasMaxLength(250)
                .IsUnicode(true);

            this.Property(t => t.ObjectTableId)
           
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.Subject)
                .HasMaxLength(500)
                .IsUnicode(true);

            this.Property(t => t.DocumentTypeDefaultReportTemplateId)
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.DocumentTypeDefaultHTMLTemplateId)
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.TemplateFormatCode)
                
                .HasMaxLength(4)
                .IsUnicode(false);

            this.Property(t => t.DocumentTypeDefaultEditorTool)
                .HasMaxLength(1)
                .IsUnicode(false);

            this.Property(t => t.SearchFields)
                .HasMaxLength(1000)
                .IsUnicode(true);

            this.Property(t => t.CustomControl)
                .HasMaxLength(250)
                .IsUnicode(false);

            this.Property(t => t.CustomerRoleId)
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.AgentRoleId)
                .HasMaxLength(15)
                .IsUnicode(false);
            this.Ignore(d => d.FollowUpTypeId);

            this.Property(t => t.IsAgentView)
                .IsRequired();

            this.Property(t => t.IsCustomerView)
                .IsRequired();

            this.Property(t => t.IsReadOnly)
                .IsRequired();

            this.Property(t => t.LimitedPrintCopyId)
            .HasMaxLength(15)
            .IsUnicode(false);
            this.Property(t => t.DocumentsDataProviderCode)
       
                 .HasMaxLength(10)
                .IsUnicode(false);
 
            this.Property(t => t.CountryCode).IsFixedLength().IsUnicode(false).HasMaxLength(2);

            this.Property(t => t.InActive)
           .IsRequired();
            
            this.Property(t => t.IsCopiedAtSignup)
                .IsRequired();

            this.Property(t => t.IsEnabledForCustomers)
                .IsRequired();

            this.Property(t => t.DocumentTypeCategoryCode)
                  .IsRequired()
                  .HasMaxLength(1)
                  .IsUnicode(true);


            this.Property(t => t.FileName)
                .HasMaxLength(200)
                .IsUnicode(true);




            this.Property(t => t.SharedDocumentTypeCopyId)
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.IsAirDigitalSignRequired)
                .IsRequired();

            this.Property(t => t.IsOceanDigitalSignRequired)
                .IsRequired();

            this.Property(t => t.IsInlandDigitalSignRequired)
                .IsRequired();



            this.Property(t => t.PrintingFieldsScreenCode)
                .HasMaxLength(60)
                .IsUnicode(false);





            this.Property(t => t.OnSendPopulateDateFieldName)
                .HasMaxLength(200)
                .IsUnicode(true);



            this.Property(t => t.OnUploadPopulateDateFieldName)
                .HasMaxLength(200)
                .IsUnicode(true);


            this.Property(t => t.OnPrintPopulateDateFieldName)
                .HasMaxLength(200)
                .IsUnicode(true);



  






            
            // Table & Column Mappings
            this.ToTable("DocumentTypes");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.OrderBy).HasColumnName("OrderBy");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.Notes).HasColumnName("Notes");
            this.Property(t => t.IsAir).HasColumnName("IsAir");
            this.Property(t => t.IsOcean).HasColumnName("IsOcean");
            this.Property(t => t.IsInland).HasColumnName("IsInland");
            this.Property(t => t.IsDocIn).HasColumnName("IsDocIn");
            this.Property(t => t.IsDocOut).HasColumnName("IsDocOut");
            this.Property(t => t.ObjectTableId).HasColumnName("ObjectTableId");
            this.Property(t => t.Subject).HasColumnName("Subject");
           
            this.Property(t => t.TemplateFormatCode).HasColumnName("TemplateFormatCode");
            this.Property(t => t.DocumentTypeDefaultEditorTool).HasColumnName("DocumentTypeDefaultEditorTool");

            this.Property(t => t.IsMaster).HasColumnName("IsMaster");
            this.Property(t => t.IsHouse).HasColumnName("IsHouse");
            this.Property(t => t.IsDirect).HasColumnName("IsDirect");
            this.Property(t => t.SearchFields).HasColumnName("SearchFields");
            this.Property(t => t.CustomControl).HasColumnName("CustomControl");
            this.Property(t => t.CustomerRoleId).HasColumnName("CustomerRoleId");
            this.Property(t => t.AgentRoleId).HasColumnName("AgentRoleId");
            this.Property(t => t.IsAgentView).HasColumnName("IsAgentView");
            this.Property(t => t.IsCustomerView).HasColumnName("IsCustomerView");
            this.Property(t => t.IsCustomerUploadPermission).HasColumnName("IsCustomerUploadPermission");
            this.Property(t => t.IsReadOnly).HasColumnName("IsReadOnly");
            this.Property(t => t.InActive).HasColumnName("InActive");
            this.Property(t => t.IsCopiedAtSignup).HasColumnName("IsCopiedAtSignup");
            this.Property(t => t.IsEnabledForCustomers).HasColumnName("IsEnabledForCustomers");

            this.Property(t => t.DocumentsDataProviderCode).HasColumnName("DocumentsDataProviderCode");
            this.Property(t => t.DocumentTypeCategoryCode).HasColumnName("DocumentTypeCategoryCode");
            this.Property(t => t.FileName).HasColumnName("FileName");
            this.Property(t => t.SharedDocumentTypeCopyId).HasColumnName("SharedDocumentTypeCopyId");
            this.Property(t => t.IsAirDigitalSignRequired).HasColumnName("IsAirDigitalSignRequired");
            this.Property(t => t.IsOceanDigitalSignRequired).HasColumnName("IsOceanDigitalSignRequired");
            this.Property(t => t.IsInlandDigitalSignRequired).HasColumnName("IsInlandDigitalSignRequired");
            this.Property(t => t.AddedManually).HasColumnName("AddedManually");

            this.Property(t => t.OnSendPopulateDateFieldName).HasColumnName("OnSendPopulateDateFieldName");
            this.Property(t => t.OnUploadPopulateDateFieldName).HasColumnName("OnUploadPopulateDateFieldName");

            this.Property(t => t.OnPrintPopulateDateFieldName).HasColumnName("OnPrintPopulateDateFieldName");


            

            this.Property(t => t.PrintingFieldsScreenCode).HasColumnName("PrintingFieldsScreenCode");
           

            //#if ORACLE_DB
            string dbms = System.Configuration.ConfigurationManager.AppSettings.Get("DBMS");
            if (dbms == "oracle")
            {
                this.Property(t => t.DocumentTypeDefaultReportTemplateId).HasColumnName("DocTypeDefaultReportTempId");
                this.Property(t => t.DocumentTypeDefaultHTMLTemplateId).HasColumnName("DocTypeDefaultHTMLTempId");
                this.Property(t => t.IsSystemAdditionalPrintingFields).HasColumnName("IsSysAdditionalPrintingFields");
            }
            else
            {
                //#else
                this.Property(t => t.DocumentTypeDefaultReportTemplateId).HasColumnName("DocumentTypeDefaultReportTemplateId");
                this.Property(t => t.DocumentTypeDefaultHTMLTemplateId).HasColumnName("DocumentTypeDefaultHTMLTemplateId");
                this.Property(t => t.IsSystemAdditionalPrintingFields).HasColumnName("IsSystemAdditionalPrintingFields");
            }
           
//#endif

            this.Property(t => t.CountryCode).HasColumnName("CountryCode");
            // Relationships
            this.HasOptional(t => t.AgentRole)
                .WithMany()
                .HasForeignKey(d => d.AgentRoleId);
            this.HasOptional(t => t.CustomerRole)
                .WithMany()
                .HasForeignKey(d => d.CustomerRoleId);
            //this.HasRequired(t => t.ObjectTable)
            //    .WithMany(t => t.DocumentTypes)
            //    .HasForeignKey(d => d.ObjectTableId);
            this.HasOptional(t => t.TemplateFormat)
                .WithMany()
                .HasForeignKey(d => d.TemplateFormatCode);


            this.HasRequired(t => t.DocumentTypeCategory)
               .WithMany()
               .HasForeignKey(d => d.DocumentTypeCategoryCode);


            
            this.HasOptional(t => t.DocumentsDataProvider)
           .WithMany()
           .HasForeignKey(d => d.DocumentsDataProviderCode);

        }
    }
}
