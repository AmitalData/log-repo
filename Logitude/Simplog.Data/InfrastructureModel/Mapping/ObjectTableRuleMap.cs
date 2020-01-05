using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Data.InfrastructureModel.EntityPOCOs;

namespace Simplog.Data.InfrastructureModel.Mapping
{
    public class ObjectTableRuleMap : EntityTypeConfiguration<ObjectTableRule>
    {
        public ObjectTableRuleMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.Condition)
                .HasMaxLength(250)
                .IsUnicode(false);

            this.Property(t => t.RuleCode)
                .IsRequired()
                .HasMaxLength(40)
                .IsUnicode(false);

            this.Property(t => t.OutputMessage)
                .HasMaxLength(250)
                .IsUnicode(false);

            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(40)
                .IsUnicode(false);

            this.Property(t => t.RuleTypeCode)
                .IsRequired()
                .HasMaxLength(4)
                .IsUnicode(false);

            this.Property(t => t.ObjectTableId)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.TriggerFieldId)
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.TriggerTypeCode)
                .IsRequired()
                .HasMaxLength(4)
                .IsUnicode(false);

            this.Property(t => t.RuleNotificationTypeCode)
                .IsRequired()
                .HasMaxLength(4)
                .IsUnicode(false);

            this.Property(t => t.TriggerFieldCode)
                .HasMaxLength(200)
                .IsUnicode(false);

            // Table & Column Mappings
            this.ToTable("ObjectTableRules");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.Condition).HasColumnName("Condition");
            this.Property(t => t.SystemLevel).HasColumnName("SystemLevel");
            this.Property(t => t.RuleCode).HasColumnName("RuleCode");
            this.Property(t => t.OutputMessage).HasColumnName("OutputMessage");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.InActive).HasColumnName("InActive");
            this.Property(t => t.RuleTypeCode).HasColumnName("RuleTypeCode");
            this.Property(t => t.ObjectTableId).HasColumnName("ObjectTableId");
            this.Property(t => t.TriggerFieldId).HasColumnName("TriggerFieldId");
            this.Property(t => t.ActiveForNew).HasColumnName("ActiveForNew");
            this.Property(t => t.ActiveForUpdate).HasColumnName("ActiveForUpdate");
            this.Property(t => t.TriggerTypeCode).HasColumnName("TriggerTypeCode");
            this.Property(t => t.RuleNotificationTypeCode).HasColumnName("RuleNotificationTypeCode");
            this.Property(t => t.Internal).HasColumnName("Internal");
            this.Property(t => t.AdvancedCondition).HasColumnName("AdvancedCondition");
            this.Property(t => t.TriggerFieldCode).HasColumnName("TriggerFieldCode");

            // Relationships
            //this.HasOptional(t => t.ObjectField)
            //    .WithMany(t => t.ObjectTableRules)
            //    .HasForeignKey(d => d.TriggerFieldId);
            //this.HasRequired(t => t.ObjectTable)
            //    .WithMany(t => t.ObjectTableRules)
            //    .HasForeignKey(d => d.ObjectTableId);

            this.HasRequired(t => t.RuleNotificationType).WithMany().HasForeignKey(d => d.RuleNotificationTypeCode);

            //this.HasRequired(t => t.RuleType)
            //    .WithMany(t => t.ObjectTableRules)
            //    .HasForeignKey(d => d.RuleTypeCode);
            //this.HasRequired(t => t.TriggerType)
            //    .WithMany(t => t.ObjectTableRules)
            //    .HasForeignKey(d => d.TriggerTypeCode);

        }
    }
}
