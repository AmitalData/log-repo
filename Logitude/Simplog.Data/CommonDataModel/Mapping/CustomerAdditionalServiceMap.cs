using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Mapping
{
    public class CustomerAdditionalServiceMap : EntityTypeConfiguration<CustomerAdditionalService>
    {
        public CustomerAdditionalServiceMap()
        {
            this.HasKey(d => new { d.CustomerId, d.AdditionalServiceId });

            this.Property(t => t.CustomerId).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.AdditionalServiceId).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.Potential).IsRequired();
            this.Property(t => t.Notes).HasMaxLength(500).IsUnicode(true);

            this.ToTable("CustomerAdditionalServices");
            this.Property(t => t.CustomerId).HasColumnName("CustomerId");
            this.Property(t => t.AdditionalServiceId).HasColumnName("AdditionalServiceId");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.Potential).HasColumnName("Potential");
            this.Property(t => t.Notes).HasColumnName("Notes");
            this.Property(t => t.NotesRightToLeft).HasColumnName("NotesRightToLeft");

            this.HasRequired(t => t.Customer)
                .WithMany()
                .HasForeignKey(d => d.CustomerId);

            this.HasRequired(t => t.AdditionalService)
                .WithMany()
                .HasForeignKey(d => d.AdditionalServiceId);
        }
    }
}
