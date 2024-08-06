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
            this.Property(t => t.ContainersNumbersAndTypesArray).HasMaxLength(2000).IsUnicode(true);
            this.Property(t => t.Commodity).HasMaxLength(15).IsUnicode(true);
            this.Property(t => t.FirstPickupLocation).HasMaxLength(100).IsUnicode(true);

            this.Property(t => t.OperationallyClosedByUserId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.DeliveryToPortId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.DeliveryFrom).HasMaxLength(40).IsUnicode(false);
            this.Property(t => t.DeliveryTo).HasMaxLength(40).IsUnicode(false);
            this.Property(t => t.PickupFrom).HasMaxLength(40).IsUnicode(false);
            this.Property(t => t.PickupTo).HasMaxLength(40).IsUnicode(false);
            this.Property(t => t.OperationallyClosedByUserName).HasMaxLength(200).IsUnicode(true);

            this.Property(t => t.DeliveryTruckerId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.PickupTruckerId).HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.DeliveryTruckerNumber).HasMaxLength(30).IsUnicode(false);
            this.Property(t => t.PickupTruckerNumber).HasMaxLength(30).IsUnicode(false);

            this.Property(t => t.DeliveryDriver).HasMaxLength(40).IsUnicode(false);
            this.Property(t => t.PickupDriver).HasMaxLength(40).IsUnicode(false);

            this.Property(t => t.DeliveryTrailerNumber).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.PickupTrailerNumber).HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.DeliveryNotes).HasMaxLength(2000).IsUnicode(true);
            this.Property(t => t.PickupNotes).HasMaxLength(2000).IsUnicode(true);

            this.Property(t => t.DeliveryDate).IsOptional();
            this.Property(t => t.OnHandDate).IsOptional();
            this.Property(t => t.PODDate).IsOptional();

            this.Property(t => t.AccountingClosedByUserId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.MainCarriageETA).IsOptional();
            this.Property(t => t.MainCarriageETD).IsOptional();
            this.Property(t => t.MainCarriageATA).IsOptional();
            this.Property(t => t.MainCarriageATD).IsOptional();
            this.Property(t => t.PackagesQuantityAndType).HasMaxLength(2000).IsUnicode(false);

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
            this.Property(t => t.CreatedFromDigital).HasColumnName("CreatedFromDigital");



            this.Property(t => t.DeliveryTruckerId).HasColumnName("DeliveryTruckerId");
            this.Property(t => t.PickupTruckerId).HasColumnName("PickupTruckerId");
            this.Property(t => t.DeliveryTruckerNumber).HasColumnName("DeliveryTruckerNumber");
            this.Property(t => t.PickupTruckerNumber).HasColumnName("PickupTruckerNumber");
            this.Property(t => t.DeliveryDriver).HasColumnName("DeliveryDriver");
            this.Property(t => t.PickupDriver).HasColumnName("PickupDriver");
            this.Property(t => t.DeliveryTrailerNumber).HasColumnName("DeliveryTrailerNumber");
            this.Property(t => t.PickupTrailerNumber).HasColumnName("PickupTrailerNumber");
            this.Property(t => t.DeliveryNotes).HasColumnName("DeliveryNotes");
            this.Property(t => t.PickupNotes).HasColumnName("PickupNotes");
            this.Property(t => t.BookingConfirmationSent).HasColumnName("BookingConfirmationSent");
            this.Property(t => t.PreAlertSent).HasColumnName("PreAlertSent");
            this.Property(t => t.DeliveryNoticeSent).HasColumnName("DeliveryNoticeSent");
            this.Property(t => t.ExpectedArrivalNoticeSent).HasColumnName("ExpectedArrivalNoticeSent");
            this.Property(t => t.ArrivalNoticeSent).HasColumnName("ArrivalNoticeSent");
            this.Property(t => t.T1Received).HasColumnName("T1Received");
            this.Property(t => t.IsDocumentsNeedApprove).HasColumnName("IsDocumentsNeedApprove");







            string dbms = System.Configuration.ConfigurationManager.AppSettings.Get("DBMS");
            if (dbms == "oracle")
            {
                this.Property(t => t.ImporterDepositionRequestDetails).HasColumnName("ImporterDepositionReqDetails");
            }
            else
            {
                this.Property(t => t.ImporterDepositionRequestDetails).HasColumnName("ImporterDepositionRequestDetails");
            }

            this.Property(t => t.FirstPickupLocation).HasColumnName("FirstPickupLocation");
            this.Property(t => t.Commodity).HasColumnName("Commodity");
            this.Property(t => t.ContainersNumbers).HasColumnName("ContainersNumbers");
            this.Property(t => t.ContainersNumbersAndTypesArray).HasColumnName("ContainersNumbersAndTypesArray");
            this.Property(t => t.FirstPickupATD).HasColumnName("FirstPickupATD");
            this.Property(t => t.FirstPickupATA).HasColumnName("FirstPickupATA");
            this.Property(t => t.FinalDeliveryETD).HasColumnName("FinalDeliveryETD");
            this.Property(t => t.FinalDeliveryETA).HasColumnName("FinalDeliveryETA");
            this.Property(t => t.FinalDeliveryATD).HasColumnName("FinalDeliveryATD");
            this.Property(t => t.FinalDeliveryATA).HasColumnName("FinalDeliveryATA");

            this.Property(t => t.OperationallyClosedByUserId).HasColumnName("OperationallyClosedByUserId");
            this.Property(t => t.NumberOfDeliveries).HasColumnName("NumberOfDeliveries");
            this.Property(t => t.LastPickupATA).HasColumnName("LastPickupATA");
            this.Property(t => t.LastPickupATD).HasColumnName("LastPickupATD");
            this.Property(t => t.LastPickupETA).HasColumnName("LastPickupETA");
            this.Property(t => t.LastPickupETD).HasColumnName("LastPickupETD");
            this.Property(t => t.DeliveryToPortId).HasColumnName("DeliveryToPortId");
            this.Property(t => t.DeliveryFrom).HasColumnName("DeliveryFrom");
            this.Property(t => t.DeliveryTo).HasColumnName("DeliveryTo");
            this.Property(t => t.PickupFrom).HasColumnName("PickupFrom");
            this.Property(t => t.PickupTo).HasColumnName("PickupTo");
            this.Property(t => t.OperationallyClosedByUserName).HasColumnName("OperationallyClosedByUserName");
            this.Property(t => t.AccountingClosedByUserId).HasColumnName("AccountingClosedByUserId");

            this.Property(t => t.DeliveryDate).HasColumnName("DeliveryDate");
            this.Property(t => t.OnHandDate).HasColumnName("OnHandDate");
            this.Property(t => t.PODDate).HasColumnName("PODDate");

            this.HasRequired(t => t.Shipment);
            this.HasOptional(t => t.OperationallyClosedByUser).WithMany().HasForeignKey(d => d.OperationallyClosedByUserId);
            this.HasOptional(t => t.DeliveryToPort).WithMany().HasForeignKey(d => d.DeliveryToPortId);

            this.HasOptional(t => t.DeliveryTrucker).WithMany().HasForeignKey(d => d.DeliveryTruckerId);
            this.HasOptional(t => t.PickupTrucker).WithMany().HasForeignKey(d => d.PickupTruckerId);
            this.HasOptional(t => t.AccountingClosedByUserUser).WithMany().HasForeignKey(d => d.AccountingClosedByUserId);

            this.Property(t => t.MainCarriageETA).HasColumnName("MainCarriageETA");
            this.Property(t => t.MainCarriageETD).HasColumnName("MainCarriageETD");
            this.Property(t => t.MainCarriageATA).HasColumnName("MainCarriageATA");
            this.Property(t => t.MainCarriageATD).HasColumnName("MainCarriageATD");
            this.Property(t => t.PackagesQuantityAndType).HasColumnName("PackagesQuantityAndType");

        }
    }
}
