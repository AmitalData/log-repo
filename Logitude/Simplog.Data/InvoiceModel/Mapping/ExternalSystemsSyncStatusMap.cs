using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Data.InvoiceModel.EntityPOCOs;

namespace Simplog.Data.InvoiceModel.Mapping
{
    public class ExternalSystemsSyncStatusMap : EntityTypeConfiguration<ExternalSystemsSyncStatus>
    {

        public ExternalSystemsSyncStatusMap()
        {

            this.HasKey(d => d.Id);

            this.Property(d => d.Id)
                .HasMaxLength(15)
                .IsRequired()
                .IsUnicode(false);

            this.Property(d => d.ProgressDetails)
                .HasMaxLength(100)
                .IsUnicode(false);

            this.Property(d => d.Status)
                .HasMaxLength(20)
                .IsUnicode(false);

            this.Property(d => d.Subject)
                .HasMaxLength(40)
                .IsUnicode(false);

            this.ToTable("ExternalSystemsSyncStatuses");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.Subject).HasColumnName("Subject");
            this.Property(t => t.StatusDate).HasColumnName("StatusDate");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.ProgressDetails).HasColumnName("ProgressDetails");
             


        }
    }
}
