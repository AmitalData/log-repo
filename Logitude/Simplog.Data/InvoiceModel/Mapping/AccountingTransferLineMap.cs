using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Data.InvoiceModel.EntityPOCOs;

namespace Simplog.Data.InvoiceModel.Mapping
{
    public class AccountingTransferLineMap : EntityTypeConfiguration<AccountingTransferLine>
    {
        public AccountingTransferLineMap()
        {
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant)
                .IsRequired();

            this.Property(t => t.AccountingTransferHeaderId)
             .IsRequired()
             .HasMaxLength(15)
             .IsUnicode(false);

            this.Property(t => t.EntityId)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.EntityReference)
                .IsOptional()
                .HasMaxLength(20)
                .IsUnicode(false);

            this.Property(t => t.SearchFields)
                .HasMaxLength(1000)
                .IsOptional().IsUnicode(true);


            // Table & Column Mappings
            this.ToTable("AccountingTransferLines");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.AccountingTransferHeaderId).HasColumnName("AccountingTransferHeaderId");
            this.Property(t => t.EntityId).HasColumnName("EntityId");
            this.Property(t => t.EntityReference).HasColumnName("EntityReference");
            this.Property(t => t.SearchFields).HasColumnName("SearchFields");

            // Relationships
            this.HasRequired(t => t.AccountingTransferHeader)
                .WithMany()
                .HasForeignKey(d => d.AccountingTransferHeaderId);

        }
    }
}
