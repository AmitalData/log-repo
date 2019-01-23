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

            this.HasRequired(t => t.Shipment);
        }
    }
}
