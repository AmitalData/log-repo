using Simplog.Data.InvoiceModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.InvoiceModel.Mapping
{
    public class ARInvoicesSignedStatusMap : EntityTypeConfiguration<ARInvoicesSignedStatus>
    {
        public ARInvoicesSignedStatusMap()
        {
            // Primary Key
            this.HasKey(t => t.Code);

            // Properties
            this.Property(t => t.Code)
                .IsRequired()
                .HasMaxLength(1)
                .IsUnicode(false);

            this.Property(t => t.LocalName)
                .IsRequired()
                .HasMaxLength(30)
                .IsUnicode(false);

            this.Property(t => t.EnglishName)
                .HasMaxLength(60)
                .IsUnicode(true);

            this.Property(t => t.SearchFields)
            .HasMaxLength(4000)
            .IsUnicode(true);


            // Table & Column Mappings
            this.ToTable("ARInvoicesSignedStatuses");
            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.LocalName).HasColumnName("LocalName");
            this.Property(t => t.EnglishName).HasColumnName("EnglishName");
            this.Property(t => t.SearchFields).HasColumnName("SearchFields");

        }
    }
}
