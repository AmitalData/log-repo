using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Mapping
{
    public class CardContactAdditionalServiceMap : EntityTypeConfiguration<CardContactAdditionalService>
    {
        public CardContactAdditionalServiceMap()
        {
            this.HasKey(t => t.Id);
            this.Property(t => t.Id).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.CardContactId).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.AdditionalServiceId).IsRequired().HasMaxLength(15).IsUnicode(false);

            // Table & Column Mappings
            this.ToTable("CardContactAdditionalServices");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.AdditionalServiceId).HasColumnName("AdditionalServiceId");
            this.Property(t => t.CardContactId).HasColumnName("CardContactId");

            // Relations
            this.HasRequired(t => t.CardContact).WithMany().HasForeignKey(d => d.CardContactId);
            this.HasRequired(t => t.AdditionalService).WithMany().HasForeignKey(d => d.AdditionalServiceId);
        }
    }
}
