using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Mapping
{
    public class AirlineMessagingRuleMap : EntityTypeConfiguration<AirlineMessagingRule>
    {
        public AirlineMessagingRuleMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);
            
            // Properties
            this.Property(t => t.Id)
               .IsRequired()
               .HasMaxLength(15)
               .IsUnicode(false);

            this.Property(t => t.AirlineId)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.MessageTypeCode)
                .IsRequired()
                .HasMaxLength(3)
                .IsUnicode(false);

            this.Property(t => t.RuleFieldId)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.CreatedByUserId)
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.UpdatedByUserId)
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.RuleFieldCode)
                .IsRequired()
                .HasMaxLength(200)
                .IsUnicode(false);

            // Table & Column Mappings
            this.ToTable("AirlineMessagingRules");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.AirlineId).HasColumnName("AirlineId");
            this.Property(t => t.MessageTypeCode).HasColumnName("MessageTypeCode");
            this.Property(t => t.RuleFieldId).HasColumnName("RuleFieldId");
            this.Property(t => t.IsMandatoryForSending).HasColumnName("IsMandatoryForSending");
            this.Property(t => t.MaxSize).HasColumnName("MaxSize");
            this.Property(t => t.InActive).HasColumnName("InActive");
            this.Property(t => t.CreatedByUserId).HasColumnName("CreatedByUserId");
            this.Property(t => t.CreateDate).HasColumnName("CreateDate");
            this.Property(t => t.UpdatedByUserId).HasColumnName("UpdatedByUserId");
            this.Property(t => t.UpdateDate).HasColumnName("UpdateDate");
            this.Property(t => t.RuleFieldCode).HasColumnName("RuleFieldCode");

            // Relationships
            this.HasRequired(t => t.RuleField).WithMany().HasForeignKey(d => d.RuleFieldId);
            this.HasRequired(t => t.Airline).WithMany().HasForeignKey(d => d.AirlineId);
            this.HasOptional(t => t.CreatedByUser).WithMany().HasForeignKey(d => d.CreatedByUserId);
            this.HasOptional(t => t.UpdatedByUser).WithMany().HasForeignKey(d => d.UpdatedByUserId);
        }
    }
}
