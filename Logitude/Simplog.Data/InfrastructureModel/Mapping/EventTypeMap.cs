using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Data.InfrastructureModel.EntityPOCOs;

namespace Simplog.Data.InfrastructureModel.Mapping
{
    public class EventTypeMap : EntityTypeConfiguration<EventType>
    {
        public EventTypeMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.Code).IsRequired().HasMaxLength(4).IsUnicode(false);
            this.Property(t => t.EnglishName).IsRequired().HasMaxLength(100).IsUnicode(false);
            this.Property(t => t.LocalName).HasMaxLength(100).IsUnicode(true);
            this.Property(t => t.EntityStatusId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.ObjectTableId).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.FollowUpEnglishName).HasMaxLength(40).IsUnicode(false);
            this.Property(t => t.FollowUpLocalName).HasMaxLength(40).IsUnicode(true);
            this.Property(t => t.SearchFields).HasMaxLength(1000).IsUnicode(true);
            this.Property(t => t.CustomerRoleId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.AgentRoleId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.IsAgentView).IsRequired();
            this.Property(t => t.IsCustomerView).IsRequired();
            this.Property(t => t.IsSharedLogisticsEnabled).IsRequired();
            this.Property(t => t.Code).IsRequired().HasMaxLength(200).IsUnicode(false);
            this.Property(t => t.EventTrigger).HasMaxLength(1000).IsUnicode(false);

            // Table & Column Mappings
            this.ToTable("EventTypes");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.EnglishName).HasColumnName("EnglishName");
            this.Property(t => t.AddedManually).HasColumnName("AddedManually");
            this.Property(t => t.IsManualEntry).HasColumnName("IsManualEntry");
            this.Property(t => t.LocalName).HasColumnName("LocalName");
            this.Property(t => t.EntityStatusId).HasColumnName("EntityStatusId");
            this.Property(t => t.ObjectTableId).HasColumnName("ObjectTableId");
            this.Property(t => t.IsFollowUp).HasColumnName("IsFollowUp");
            this.Property(t => t.FollowUpEnglishName).HasColumnName("FollowUpEnglishName");
            this.Property(t => t.FollowUpLocalName).HasColumnName("FollowUpLocalName");
            this.Property(t => t.ShortView).HasColumnName("ShortView");
            this.Property(t => t.ManualActivatedFollowUp).HasColumnName("ManualActivatedFollowUp");
            this.Property(t => t.InActive).HasColumnName("InActive");
            this.Property(t => t.SearchFields).HasColumnName("SearchFields");
            this.Property(t => t.CustomerRoleId).HasColumnName("CustomerRoleId");
            this.Property(t => t.AgentRoleId).HasColumnName("AgentRoleId");
            this.Property(t => t.IsAgentView).HasColumnName("IsAgentView");
            this.Property(t => t.IsCustomerView).HasColumnName("IsCustomerView");
            this.Property(t => t.IsSharedLogisticsEnabled).HasColumnName("IsSharedLogisticsEnabled");
            this.Property(t => t.AllowedInAutomation).HasColumnName("AllowedInAutomation");
            this.Property(t => t.CustomField).HasColumnName("CustomField");
            this.Property(t => t.EventTrigger).HasColumnName("EventTrigger");

            this.HasOptional(t => t.AgentRole).WithMany().HasForeignKey(d => d.AgentRoleId);
            this.HasOptional(t => t.CustomerRole).WithMany().HasForeignKey(d => d.CustomerRoleId);
        }
    }
}
