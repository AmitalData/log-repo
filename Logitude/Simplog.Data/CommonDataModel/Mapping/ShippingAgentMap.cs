using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Simplog.Data.CommonDataModel.Mapping
{
    public class ShippingAgentMap : EntityTypeConfiguration<ShippingAgent>
    {
        public ShippingAgentMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.ForwarderAccountNumber)
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.ForwarderCreditNumber)
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.LocalCustomsCode)
                .HasMaxLength(50)
                .IsUnicode(false);

            this.Property(t => t.PrimaryContactName).HasMaxLength(120).IsUnicode(true);
            this.Property(t => t.PrimaryContactEmail).HasMaxLength(70).IsUnicode(false);
            this.Property(t => t.PrimaryContactPhone).HasMaxLength(25).IsUnicode(false);

            // Table & Column Mappings
            this.ToTable("ShippingAgents");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.ForwarderAccountNumber).HasColumnName("ForwarderAccountNumber");
            this.Property(t => t.ForwarderCreditNumber).HasColumnName("ForwarderCreditNumber");
            this.Property(t => t.LocalCustomsCode).HasColumnName("LocalCustomsCode");
            this.Property(t => t.PrimaryContactName).HasColumnName("PrimaryContactName");
            this.Property(t => t.PrimaryContactEmail).HasColumnName("PrimaryContactEmail");
            this.Property(t => t.PrimaryContactPhone).HasColumnName("PrimaryContactPhone");

            // Relationships
            this.HasRequired(t => t.Card)
                .WithOptional(t => t.ShippingAgent);

        }
    }
}
