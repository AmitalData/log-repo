using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Mapping
{
    public class SharedLogisticsContactLastLoginMap : EntityTypeConfiguration<SharedLogisticsContactLastLogin>
    {
        public SharedLogisticsContactLastLoginMap()
        {
            // Primary Key
            this.HasKey(t => new { t.ContactId, t.CardId, t.PartnerTypeId, t.Via });

            // Properties
           
            this.Property(t => t.ContactId)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);


            this.Property(t => t.CardId)
               .HasMaxLength(15).IsRequired()
               .IsUnicode(false);

            this.Property(t => t.PartnerTypeId)
               .HasMaxLength(2).IsRequired()
               .IsUnicode(false);



            this.Property(t => t.Via)
            .HasMaxLength(20)
            .IsUnicode(false);

            // Table & Column Mappings
            this.ToTable("SharedLogisticsContactLastLogins");
            this.Property(t => t.CardId).HasColumnName("CardId");
            this.Property(t => t.LoginDateTime).HasColumnName("LoginDateTime");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.ContactId).HasColumnName("ContactId");
            this.Property(t => t.PartnerTypeId).HasColumnName("PartnerTypeId");
            this.Property(t => t.Via).HasColumnName("Via");
            // Relationships
            this.HasRequired(t => t.Contact)
               .WithMany()
               .HasForeignKey(d => d.ContactId);

        }
    }
}
