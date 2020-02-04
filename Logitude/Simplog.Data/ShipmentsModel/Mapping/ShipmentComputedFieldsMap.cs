using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Data.ShipmentsModel.EntityPOCOs;

namespace Simplog.Data.ShipmentsModel.Mapping
{
    public class ShipmentComputedFieldsMap : EntityTypeConfiguration<ShipmentComputedFields>
    {
        public ShipmentComputedFieldsMap()
        {
            this.HasKey(t => t.Id);
            this.Property(t => t.Id).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.IsMissingDocuments).IsRequired();
            this.Property(t => t.DocumentsSearchFields).IsMaxLength().IsUnicode(true);
            this.Property(t => t.LastDocumentDateTime).IsOptional();
            this.Property(t => t.MissingDocumentsCount);
            this.Property(t => t.MissingDocumentsNames).IsMaxLength();
            this.Property(t => t.IsDigitalSignRequired).IsRequired();
            this.Property(t => t.ImporterDepositionRequestDetails).HasMaxLength(100).IsUnicode(false);
            this.Property(t => t.FirstPickupATD).IsOptional();
            this.Property(t => t.FirstPickupATA).IsOptional();
            this.Property(t => t.FinalDeliveryETD).IsOptional();
            this.Property(t => t.FinalDeliveryETA).IsOptional();
            this.Property(t => t.FinalDeliveryATD).IsOptional();
            this.Property(t => t.FinalDeliveryATA).IsOptional();
            this.Property(t => t.ContainersNumbers).HasMaxLength(1000).IsUnicode(true);
            this.Property(t => t.Commodity).HasMaxLength(15).IsUnicode(true);
            this.Property(t => t.FirstPickupLocation).HasMaxLength(100).IsUnicode(true);

            this.Property(t => t.OperationallyClosedByUserId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.ImportDeclarationNumber).HasMaxLength(40).IsUnicode(false);
            this.Property(t => t.DeliveryToCity).HasMaxLength(25).IsUnicode(false);
            this.Property(t => t.DeliveryToPortId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.DeliveryFrom).HasMaxLength(40).IsUnicode(false);
            this.Property(t => t.DeliveryTo).HasMaxLength(40).IsUnicode(false);
            this.Property(t => t.PickupFrom).HasMaxLength(40).IsUnicode(false);
            this.Property(t => t.PickupTo).HasMaxLength(40).IsUnicode(false);
            this.Property(t => t.OperationallyClosedByUserName).HasMaxLength(100).IsUnicode(false);

            // Table & Column Mappings
            this.ToTable("ShipmentComputedFields");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.IsMissingDocuments).HasColumnName("IsMissingDocuments");
            this.Property(t => t.DocumentsSearchFields).HasColumnName("DocumentsSearchFields");
            this.Property(t => t.LastDocumentDateTime).HasColumnName("LastDocumentDateTime");
            this.Property(t => t.MissingDocumentsCount).HasColumnName("MissingDocumentsCount");
            this.Property(t => t.MissingDocumentsNames).HasColumnName("MissingDocumentsNames");
            this.Property(t => t.IsRequestedDocuments).HasColumnName("IsRequestedDocuments");
            this.Property(t => t.RequestedDocumentsCount).HasColumnName("RequestedDocumentsCount");
            this.Property(t => t.NumberOfHouses).HasColumnName("NumberOfHouses");
            this.Property(t => t.IsDigitalSignRequired).HasColumnName("IsDigitalSignRequired");
            this.Property(t => t.IsDepositionRequired).HasColumnName("IsDepositionRequired");
            this.Property(t => t.ImporterDepositionRequestDetails).HasColumnName("ImporterDepositionRequestDetails");
            this.Property(t => t.FirstPickupLocation).HasColumnName("FirstPickupLocation");
            this.Property(t => t.Commodity).HasColumnName("Commodity");
            this.Property(t => t.ContainersNumbers).HasColumnName("ContainersNumbers");
            this.Property(t => t.FirstPickupATD).HasColumnName("FirstPickupATD");
            this.Property(t => t.FirstPickupATA).HasColumnName("FirstPickupATA");
            this.Property(t => t.FinalDeliveryETD).HasColumnName("FinalDeliveryETD");
            this.Property(t => t.FinalDeliveryETA).HasColumnName("FinalDeliveryETA");
            this.Property(t => t.FinalDeliveryATD).HasColumnName("FinalDeliveryATD");
            this.Property(t => t.FinalDeliveryATA).HasColumnName("FinalDeliveryATA");

            this.Property(t => t.OperationallyClosedByUserId).HasColumnName("OperationallyClosedByUserId");
            this.Property(t => t.NumberOfDeliveries).HasColumnName("NumberOfDeliveries");
            this.Property(t => t.ImportDeclarationDate).HasColumnName("ImportDeclarationDate");
            this.Property(t => t.ImportDeclarationNumber).HasColumnName("ImportDeclarationNumber");
            this.Property(t => t.LastPickupATA).HasColumnName("LastPickupATA");
            this.Property(t => t.LastPickupATD).HasColumnName("LastPickupATD");
            this.Property(t => t.LastPickupETA).HasColumnName("LastPickupETA");
            this.Property(t => t.LastPickupETD).HasColumnName("LastPickupETD");
            this.Property(t => t.DeliveryToCity).HasColumnName("DeliveryToCity");
            this.Property(t => t.DeliveryToPortId).HasColumnName("DeliveryToPortId");
            this.Property(t => t.ContainsDangerousGoods).HasColumnName("ContainsDangerousGoods");
            this.Property(t => t.DeliveryFrom).HasColumnName("DeliveryFrom");
            this.Property(t => t.DeliveryTo).HasColumnName("DeliveryTo");
            this.Property(t => t.PickupFrom).HasColumnName("PickupFrom");
            this.Property(t => t.PickupTo).HasColumnName("PickupTo");
            this.Property(t => t.OperationallyClosedByUserName).HasColumnName("OperationallyClosedByUserName");

            this.HasRequired(t => t.Shipment);
            this.HasOptional(t => t.OperationallyClosedByUser).WithMany().HasForeignKey(d => d.OperationallyClosedByUserId);
            this.HasOptional(t => t.DeliveryToPort).WithMany().HasForeignKey(d => d.DeliveryToPortId);
        }
    }
}
