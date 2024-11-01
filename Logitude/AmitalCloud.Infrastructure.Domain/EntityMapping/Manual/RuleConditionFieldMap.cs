using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using AmitalCloud.Infrastructure.Domain.EntityPOCOs;

namespace AmitalCloud.Infrastructure.Domain.EntityMapping
{
    public class RuleConditionFieldMap : EntityTypeConfiguration<RuleConditionField>
    {
        public RuleConditionFieldMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.Value)
                .HasMaxLength(100)
                .IsUnicode(false);

            this.Property(t => t.Operator)
                .HasMaxLength(40)
                .IsUnicode(false);

            this.Property(t => t.ObjectTableRuleId)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.ObjectFieldId)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            //this.Property(t => t.ObjectFieldCode)
            //    .IsRequired()
            //    .HasMaxLength(200)
            //    .IsUnicode(false);

            this.Property(t => t.ObjectFieldCode)
               .HasMaxLength(200)
               .IsUnicode(false);

            // Table & Column Mappings
            this.ToTable("RuleConditionFields");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.Value).HasColumnName("Value");
            this.Property(t => t.Operator).HasColumnName("Operator");
            this.Property(t => t.ObjectTableRuleId).HasColumnName("ObjectTableRuleId");
            this.Property(t => t.ObjectFieldId).HasColumnName("ObjectFieldId");
            this.Property(t => t.ObjectFieldCode).HasColumnName("ObjectFieldCode");

            // Relationships
            //this.HasRequired(t => t.ObjectField)
            //    .WithMany(t => t.RuleConditionFields)
            //    .HasForeignKey(d => d.ObjectFieldId);
            //this.HasRequired(t => t.ObjectTableRule)
            //    .WithMany(t => t.RuleConditionFields)
            //    .HasForeignKey(d => d.ObjectTableRuleId);

        }
    }
}
