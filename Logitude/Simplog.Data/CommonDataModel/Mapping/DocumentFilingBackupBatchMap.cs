using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Mapping
{
    public class DocumentFilingBackupBatchMap : EntityTypeConfiguration<DocumentFilingBackupBatch>
    {
        public DocumentFilingBackupBatchMap()
        {
            this.HasKey(t => new { t.Id });


            this.Property(t => t.Id)
            .IsRequired()
            .HasMaxLength(15)
            .IsUnicode(false);

            this.Property(t => t.BatchNumber).IsRequired()
            .HasMaxLength(15)
            .IsRequired()
            .IsUnicode(false);

            this.Property(t => t.Status)
            .HasMaxLength(15)
            .IsRequired()
            .IsUnicode(false);


            this.Property(t => t.CreateDateTime)
            .IsRequired();


            // Table & Column Mappings
            //string dbms = System.Configuration.ConfigurationManager.AppSettings.Get("DBMS");
            //if (dbms == "oracle")
            //{
            //    this.ToTable("DocumentFilingBackupBatches");
            //}
            //else
            //{
            //    this.ToTable("DocumentFilingBackupBatches");
            //}
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.BatchNumber).HasColumnName("BatchNumber");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.DoneDate).HasColumnName("DoneDate");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.CreateDateTime).HasColumnName("CreateDateTime");
            this.Property(t => t.FromDatetime).HasColumnName("FromDatetime");
            this.Property(t => t.ToDatetime).HasColumnName("ToDatetime");
            this.Property(t => t.TotalDocuments).HasColumnName("TotalDocuments");
            this.Property(t => t.TotalSucceeded).HasColumnName("TotalSucceeded");
            this.Property(t => t.TotalFailed).HasColumnName("TotalFailed");
            this.Property(t => t.IncludeBackedUp).HasColumnName("IncludeBackedUp");


        }
    }
}
