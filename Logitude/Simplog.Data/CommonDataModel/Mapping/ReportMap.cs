using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Simplog.Data.CommonDataModel.Mapping
{
    public class ReportMap : EntityTypeConfiguration<Report>
    {

        public ReportMap()
        {
            this.HasKey(t => t.Id);
            this.Property(t => t.Id).IsRequired().IsUnicode(false).HasMaxLength(15);
            this.Property(t => t.Code).IsUnicode(false).HasMaxLength(4);
            this.Property(t => t.Name).IsUnicode(false).HasMaxLength(40);
            this.Property(t => t.LocalName).IsUnicode(true).HasMaxLength(60);
            this.Property(t => t.Description).IsUnicode(true).HasMaxLength(250);
            this.Property(t => t.FilterControlName).IsUnicode(false).HasMaxLength(100);
            this.Property(t => t.SearchFields).IsUnicode(true).HasMaxLength(1000);
            this.Property(t => t.ReportGroupId).IsUnicode(false).HasMaxLength(15);
            this.Property(t => t.FeatureId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.ReportDocumentId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.FilterHtmlComponentUrl).HasMaxLength(256).IsUnicode(false);
            this.Property(t => t.DefaultTemplateId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.DefaultMessageTemplateId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.DefaultExcelTemplateId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.FeatureUniqeCode).HasMaxLength(120).IsUnicode(false);


            this.ToTable("Reports");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.LocalName).HasColumnName("LocalName");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.SearchFields).HasColumnName("SearchFields");
            this.Property(t => t.FilterControlName).HasColumnName("FilterControlName");
            this.Property(t => t.ReportGroupId).HasColumnName("ReportGroupId");
            this.Property(t => t.FeatureId).HasColumnName("FeatureId");
            this.Property(t => t.ReportDocumentId).HasColumnName("ReportDocumentId");
            this.Property(t => t.InActive).HasColumnName("InActive");
            this.Property(t => t.FilterHtmlComponentUrl).HasColumnName("FilterHtmlComponentUrl");
            this.Property(t => t.DefaultMessageTemplateId).HasColumnName("DefaultMessageTemplateId");
            this.Property(t => t.DefaultExcelTemplateId).HasColumnName("DefaultExcelTemplateId");
            this.Property(t => t.DefaultTemplateId).HasColumnName("DefaultTemplateId");
            this.Property(t => t.FeatureUniqeCode).HasColumnName("FeatureUniqeCode");
            this.Property(t => t.AvailableForScheduling).HasColumnName("AvailableForScheduling");
            this.Property(t => t.DisablePreview).HasColumnName("DisablePreview");
            


            //this.HasOptional(t => t.Feature).WithMany().HasForeignKey(d => d.FeatureId);
            this.HasOptional(d => d.ReportGroup).WithMany().HasForeignKey(d => d.ReportGroupId);
            //this.HasOptional(t => t.Feature).WithMany().HasForeignKey(d => d.FeatureId);
            this.HasOptional(t => t.ReportsTemplate).WithMany().HasForeignKey(d => d.DefaultTemplateId);
            this.HasOptional(t => t.ReportsTemplateDefaultMessage).WithMany().HasForeignKey(d => d.DefaultMessageTemplateId);
            this.HasOptional(t => t.ReportsTemplateDefaultExcel).WithMany().HasForeignKey(d => d.DefaultExcelTemplateId);

        }
    }
}
