using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Simplog.Data.CommonDataModel.Mapping
{
    public class AgentMap : EntityTypeConfiguration<Agent>
    {
        public AgentMap()
        {
            this.HasKey(t => t.Id);
            this.Property(t => t.Id).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.CASSCode).HasMaxLength(4).IsUnicode(false);
            this.Property(t => t.IATACode).HasMaxLength(7).IsUnicode(false);
            this.Property(t => t.RegulatedAgentCode).HasMaxLength(5).IsUnicode(false);
            this.Property(t => t.AgentSharedLogisticsKey).HasMaxLength(40).IsUnicode(false);
            this.Property(t => t.PrimaryContactName).HasMaxLength(120).IsUnicode(true);
            this.Property(t => t.PrimaryContactEmail).HasMaxLength(70).IsUnicode(false);
            this.Property(t => t.PrimaryContactPhone).HasMaxLength(25).IsUnicode(false);

            this.ToTable("Agents");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CASSCode).HasColumnName("CASSCode");
            this.Property(t => t.IATACode).HasColumnName("IATACode");
            this.Property(t => t.RegulatedAgentCode).HasColumnName("RegulatedAgentCode");
            this.Property(t => t.AgentSharedLogisticsKey).HasColumnName("AgentSharedLogisticsKey");
            this.Property(t => t.IsCreditLimitEnabled).HasColumnName("IsCreditLimitEnabled");
            this.Property(t => t.BlockNewInvoiceCreation).HasColumnName("BlockNewInvoiceCreation");
            this.Property(t => t.BlockNewShipmentCreation).HasColumnName("BlockNewShipmentCreation");
            this.Property(t => t.PrimaryContactName).HasColumnName("PrimaryContactName");
            this.Property(t => t.PrimaryContactEmail).HasColumnName("PrimaryContactEmail");
            this.Property(t => t.PrimaryContactPhone).HasColumnName("PrimaryContactPhone");

            this.HasRequired(t => t.Card).WithOptional(t => t.Agent);
        }
    }
}
