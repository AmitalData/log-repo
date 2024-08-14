using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Simplog.Data.CommonDataModel.Mapping
{
    public class ShippingLineMap : EntityTypeConfiguration<ShippingLine>
    {
        public ShippingLineMap()
        {
            this.HasKey(t => t.Id);
            this.Property(t => t.OurCreditNumber).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.Id).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.ShippingAgentId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.SCACCode).IsRequired().HasMaxLength(4).IsUnicode(false);
            this.Property(t => t.INTTRARegistrationNotes).HasMaxLength(250).IsUnicode(true);
            this.Property(t => t.PrimaryContactName).HasMaxLength(60).IsUnicode(false);
            this.Property(t => t.PrimaryContactEmail).HasMaxLength(70).IsUnicode(false);
            this.Property(t => t.PrimaryContactPhone).HasMaxLength(25).IsUnicode(false);
            this.Property(t => t.CBSA).HasMaxLength(5).IsUnicode(false);
            this.Property(t => t.CAAT).HasMaxLength(4).IsUnicode(false);

            this.ToTable("ShippingLines");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.OurCreditNumber).HasColumnName("OurCreditNumber");
            this.Property(t => t.AddedManually).HasColumnName("AddedManually");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.ShippingAgentId).HasColumnName("ShippingAgentId");
            this.Property(t => t.SCACCode).HasColumnName("SCACCode");
            this.Property(t => t.IsINTTRARegistered).HasColumnName("IsINTTRARegistered");
            this.Property(t => t.INTTRARegistrationNotes).HasColumnName("INTTRARegistrationNotes");
            this.Property(t => t.PrimaryContactName).HasColumnName("PrimaryContactName");
            this.Property(t => t.PrimaryContactEmail).HasColumnName("PrimaryContactEmail");
            this.Property(t => t.PrimaryContactPhone).HasColumnName("PrimaryContactPhone");
            this.Property(t => t.CBSA).HasColumnName("CBSA");
            this.Property(t => t.CAAT).HasColumnName("CAAT");
            this.Property(t => t.INTTRAUpdatesShipment).HasColumnName("INTTRAUpdatesShipment");
            this.Property(t => t.IsSendingByContainer).HasColumnName("IsSendingByContainer");
            this.Property(t => t.IsSendingByBillOfLading).HasColumnName("IsSendingByBillOfLading");
            this.Property(t => t.IsAutomaticRequestsSent).HasColumnName("IsAutomaticRequestsSent");
            this.Property(t => t.IsSupportsContainerTracking).HasColumnName("IsSupportsContainerTracking");

            this.HasRequired(t => t.Card).WithOptional(t => t.ShippingLine);
            this.HasOptional(t => t.ShippingAgent).WithMany(t => t.ShippingLines).HasForeignKey(d => d.ShippingAgentId);
        }
    }
}
