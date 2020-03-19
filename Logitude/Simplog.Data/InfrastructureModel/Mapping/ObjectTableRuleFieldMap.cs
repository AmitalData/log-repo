using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Data.InfrastructureModel.EntityPOCOs;

namespace Simplog.Data.InfrastructureModel.Mapping
{
    public class ObjectTableRuleFieldMap : EntityTypeConfiguration<ObjectTableRuleField>
    {
        public ObjectTableRuleFieldMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.ObjectFieldId)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.ObjectTableRuleId)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.Expression)
                .HasMaxLength(250)
                .IsUnicode(false);

            this.Property(t => t.RuleNotificationTypeCode)
                .HasMaxLength(4)
                .IsUnicode(false);

            this.Property(t => t.ObjectFieldCode)
                .IsRequired()
                .HasMaxLength(200)
                .IsUnicode(false);

            // Table & Column Mappings
            this.ToTable("ObjectTableRuleFields");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.SystemLevel).HasColumnName("SystemLevel");
            this.Property(t => t.ObjectFieldId).HasColumnName("ObjectFieldId");
            this.Property(t => t.ObjectTableRuleId).HasColumnName("ObjectTableRuleId");
            this.Property(t => t.Expression).HasColumnName("Expression");
            this.Property(t => t.RuleNotificationTypeCode).HasColumnName("RuleNotificationTypeCode");
            this.Property(t => t.ObjectFieldCode).HasColumnName("ObjectFieldCode");

            // Relationships
            //this.HasRequired(t => t.ObjectField)
            //    .WithMany(t => t.ObjectTableRuleFields)
            //    .HasForeignKey(d => d.ObjectFieldId);
            //this.HasRequired(t => t.ObjectTableRule)
            //    .WithMany(t => t.ObjectTableRuleFields)
            //    .HasForeignKey(d => d.ObjectTableRuleId);
            //this.HasOptional(t => t.RuleNotificationType)
            //    .WithMany(t => t.ObjectTableRuleFields)
            //    .HasForeignKey(d => d.RuleNotificationTypeCode);

        }
    }
}
