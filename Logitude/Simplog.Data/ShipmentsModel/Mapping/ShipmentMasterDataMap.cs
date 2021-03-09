using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Data.ShipmentsModel.EntityPOCOs;

namespace Simplog.Data.ShipmentsModel.Mapping
{
    public class ShipmentMasterDataMap : EntityTypeConfiguration<ShipmentMasterData>
    {
        public ShipmentMasterDataMap()
        {
            this.HasKey(t => t.Id);
            this.Property(t => t.Id).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.MainCarriageFromPortId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.MainCarriageToPortId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.MainCarriageCarrierNumber).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.Master).HasMaxLength(20).IsUnicode(false);
            this.Property(t => t.Transshipment1FromPortId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.Transshipment1ToPortId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.Transshipment1CarrierNumber).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.Transshipment2FromPortId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.Transshipment2ToPortId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.Transshipment2CarrierNumber).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.Transshipment3FromPortId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.Transshipment3ToPortId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.Transshipment3CarrierNumber).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.BookingConfirmationNumber).HasMaxLength(25).IsUnicode(false);
            this.Property(t => t.BookingConfirmationNotes).HasMaxLength(250).IsUnicode(true);
            this.Property(t => t.BookingConfirmedBy).HasMaxLength(40).IsUnicode(false);
            this.Property(t => t.MainCarriageVesselId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.Transshipment1VesselId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.Transshipment2VesselId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.Transshipment3VesselId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.Transshipment1AdditionalMAWBOBLBL).HasMaxLength(20).IsUnicode(false);
            this.Property(t => t.Transshipment2AdditionalMAWBOBLBL).HasMaxLength(20).IsUnicode(false);
            this.Property(t => t.Transshipment3AdditionalMAWBOBLBL).HasMaxLength(20).IsUnicode(false);
            this.Property(t => t.MainCarriageCarrierId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.Transshipment1CarrierId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.Transshipment2CarrierId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.Transshipment3CarrierId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.MainCarriageFinalDestinationPortId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.StatusId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.StatusLocation).HasMaxLength(40).IsUnicode(true);
            this.Property(t => t.MasterShipmentNumber).HasMaxLength(20).IsUnicode(false);
            this.Property(t => t.FWBStatusCode).HasMaxLength(4).IsUnicode(false);
            this.Property(t => t.MainCarriageFromPartnerId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.MainCarriageToPartnerId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.MainCarriageFromAddressId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.MainCarriageToAddressId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.Driver).HasMaxLength(40).IsUnicode(true);
            this.Property(t => t.TruckNumber).HasMaxLength(15).IsUnicode(true);
            this.Property(t => t.TrailerNumber).HasMaxLength(15).IsUnicode(true);
            this.Property(t => t.ImportManifest).HasMaxLength(50).IsUnicode(false);
            this.Property(t => t.CarrierTransportDocumentNumber).HasMaxLength(50).IsUnicode(false);
            this.Property(t => t.CargonautFWBStatusCode).HasMaxLength(4).IsUnicode(false);
            this.Property(t => t.MainCarriageCarrierPrefix).IsFixedLength().HasMaxLength(2).IsUnicode(false);
            this.Property(t => t.Transshipment1CarrierPrefix).IsFixedLength().HasMaxLength(2).IsUnicode(false);
            this.Property(t => t.Transshipment2CarrierPrefix).IsFixedLength().HasMaxLength(2).IsUnicode(false);
            this.Property(t => t.Transshipment3CarrierPrefix).IsFixedLength().HasMaxLength(2).IsUnicode(false);                        
            this.Property(t => t.RegulatedAgentRANumber).HasMaxLength(5).IsUnicode(false);
            this.Property(t => t.ColoaderRANumber).HasMaxLength(5).IsUnicode(false);
            this.Property(t => t.AWBPrintingRANumber).HasMaxLength(5).IsUnicode(false);
            this.Property(t => t.KnownConsignorNumber).HasMaxLength(9).IsUnicode(false);
            this.Property(t => t.AWBPrintingSecurityStatusId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.AdditionalHandlingInfo).HasMaxLength(65).IsUnicode(false);
            this.Property(t => t.InterlineId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.AirlinePrefix).HasMaxLength(3).IsUnicode(false);
            this.Property(t => t.ManifestReason).HasMaxLength(500).IsUnicode(false);
            this.Property(t => t.ManifestStatusCode).HasMaxLength(4).IsUnicode(false);
            this.Property(t => t.OBLTypeCode).HasMaxLength(4).IsUnicode(false);

            this.Property(t => t.PreCarriageTransportModeId).IsFixedLength().HasMaxLength(1).IsUnicode(false);
            this.Property(t => t.PreCarriageFromPortId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.PreCarriageToPortId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.PreCarriageCarrierNumber).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.PreCarriageVesselId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.PreCarriageCarrierId).HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.OnCarriageTransportModeId).IsFixedLength().HasMaxLength(1).IsUnicode(false);
            this.Property(t => t.OnCarriageFromPortId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.OnCarriageToPortId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.OnCarriageCarrierNumber).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.OnCarriageVesselId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.OnCarriageCarrierId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.OnCarriageAdditionalTransportModeCode).HasMaxLength(4).IsUnicode(false);

            // Table & Column Mappings
            this.ToTable("ShipmentMasterDatas");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.MainCarriageFromPortId).HasColumnName("MainCarriageFromPortId");
            this.Property(t => t.MainCarriageToPortId).HasColumnName("MainCarriageToPortId");
            this.Property(t => t.MainCarriageATD).HasColumnName("MainCarriageATD");
            this.Property(t => t.MainCarriageATA).HasColumnName("MainCarriageATA");
            this.Property(t => t.MainCarriageETA).HasColumnName("MainCarriageETA");
            this.Property(t => t.MainCarriageETD).HasColumnName("MainCarriageETD");
            this.Property(t => t.MainCarriageCarrierNumber).HasColumnName("MainCarriageCarrierNumber");
            this.Property(t => t.Master).HasColumnName("Master");
            this.Property(t => t.Transshipment1FromPortId).HasColumnName("Transshipment1FromPortId");
            this.Property(t => t.Transshipment1ToPortId).HasColumnName("Transshipment1ToPortId");
            this.Property(t => t.Transshipment1ATD).HasColumnName("Transshipment1ATD");
            this.Property(t => t.Transshipment1ATA).HasColumnName("Transshipment1ATA");
            this.Property(t => t.Transshipment1ETD).HasColumnName("Transshipment1ETD");
            this.Property(t => t.Transshipment1ETA).HasColumnName("Transshipment1ETA");
            this.Property(t => t.Transshipment1CarrierNumber).HasColumnName("Transshipment1CarrierNumber");
            this.Property(t => t.Transshipment2FromPortId).HasColumnName("Transshipment2FromPortId");
            this.Property(t => t.Transshipment2ToPortId).HasColumnName("Transshipment2ToPortId");
            this.Property(t => t.Transshipment2ATD).HasColumnName("Transshipment2ATD");
            this.Property(t => t.Transshipment2ATA).HasColumnName("Transshipment2ATA");
            this.Property(t => t.Transshipment2ETD).HasColumnName("Transshipment2ETD");
            this.Property(t => t.Transshipment2ETA).HasColumnName("Transshipment2ETA");
            this.Property(t => t.Transshipment2CarrierNumber).HasColumnName("Transshipment2CarrierNumber");
            this.Property(t => t.Transshipment3FromPortId).HasColumnName("Transshipment3FromPortId");
            this.Property(t => t.Transshipment3ToPortId).HasColumnName("Transshipment3ToPortId");
            this.Property(t => t.Transshipment3ATD).HasColumnName("Transshipment3ATD");
            this.Property(t => t.Transshipment3ATA).HasColumnName("Transshipment3ATA");
            this.Property(t => t.Transshipment3ETD).HasColumnName("Transshipment3ETD");
            this.Property(t => t.Transshipment3ETA).HasColumnName("Transshipment3ETA");
            this.Property(t => t.Transshipment3CarrierNumber).HasColumnName("Transshipment3CarrierNumber");
            this.Property(t => t.MAWBOBLDate).HasColumnName("MAWBOBLDate");
            this.Property(t => t.BookingConfirmationNumber).HasColumnName("BookingConfirmationNumber");
            this.Property(t => t.BookingConfirmationNotes).HasColumnName("BookingConfirmationNotes");
            this.Property(t => t.BookingConfirmedBy).HasColumnName("BookingConfirmedBy");
            this.Property(t => t.MainCarriageVesselId).HasColumnName("MainCarriageVesselId");
            this.Property(t => t.Transshipment1VesselId).HasColumnName("Transshipment1VesselId");
            this.Property(t => t.Transshipment2VesselId).HasColumnName("Transshipment2VesselId");
            this.Property(t => t.Transshipment3VesselId).HasColumnName("Transshipment3VesselId");        
            this.Property(t => t.MainCarriageIsFromStack).HasColumnName("MainCarriageIsFromStack");
            this.Property(t => t.MainCarriageCarrierId).HasColumnName("MainCarriageCarrierId");
            this.Property(t => t.Transshipment1CarrierId).HasColumnName("Transshipment1CarrierId");
            this.Property(t => t.Transshipment2CarrierId).HasColumnName("Transshipment2CarrierId");
            this.Property(t => t.Transshipment3CarrierId).HasColumnName("Transshipment3CarrierId");            
            this.Property(t => t.StatusId).HasColumnName("StatusId");
            this.Property(t => t.MasterShipmentNumber).HasColumnName("MasterShipmentNumber");
            this.Property(t => t.FWBStatusCode).HasColumnName("FWBStatusCode");
            this.Property(t => t.FWBStatusDate).HasColumnName("FWBStatusDate");
            this.Property(t => t.MainCarriageFromPartnerId).HasColumnName("MainCarriageFromPartnerId");
            this.Property(t => t.MainCarriageToPartnerId).HasColumnName("MainCarriageToPartnerId");
            this.Property(t => t.MainCarriageFromAddressId).HasColumnName("MainCarriageFromAddressId");
            this.Property(t => t.MainCarriageToAddressId).HasColumnName("MainCarriageToAddressId");
            this.Property(t => t.Driver).HasColumnName("Driver");
            this.Property(t => t.TruckNumber).HasColumnName("TruckNumber");
            this.Property(t => t.TrailerNumber).HasColumnName("TrailerNumber");
            this.Property(t => t.MainCarriageSTA).HasColumnName("MainCarriageSTA");
            this.Property(t => t.MainCarriageSTD).HasColumnName("MainCarriageSTD");
            this.Property(t => t.Transshipment1STA).HasColumnName("Transshipment1STA");
            this.Property(t => t.Transshipment1STD).HasColumnName("Transshipment1STD");
            this.Property(t => t.Transshipment2STA).HasColumnName("Transshipment2STA");
            this.Property(t => t.Transshipment2STD).HasColumnName("Transshipment2STD");
            this.Property(t => t.Transshipment3STA).HasColumnName("Transshipment3STA");
            this.Property(t => t.Transshipment3STD).HasColumnName("Transshipment3STD");
            this.Property(t => t.ImportManifest).HasColumnName("ImportManifest");
            this.Property(t => t.CarrierTransportDocumentNumber).HasColumnName("CarrierTransportDocumentNumber");
            this.Property(t => t.StatusDate).HasColumnName("StatusDate");
            this.Property(t => t.CargonautFWBStatusCode).HasColumnName("CargonautFWBStatusCode");
            this.Property(t => t.CargonautFWBStatusDate).HasColumnName("CargonautFWBStatusDate");
            this.Property(t => t.MainCarriageCarrierPrefix).HasColumnName("MainCarriageCarrierPrefix");
            this.Property(t => t.Transshipment1CarrierPrefix).HasColumnName("Transshipment1CarrierPrefix");
            this.Property(t => t.Transshipment2CarrierPrefix).HasColumnName("Transshipment2CarrierPrefix");
            this.Property(t => t.Transshipment3CarrierPrefix).HasColumnName("Transshipment3CarrierPrefix");
            this.Property(t => t.IsKnownCargo).HasColumnName("IsKnownCargo");
            this.Property(t => t.RegulatedAgentRANumber).HasColumnName("RegulatedAgentRANumber");
            this.Property(t => t.KnownConsignorNumber).HasColumnName("KnownConsignorNumber");
            this.Property(t => t.ColoaderRANumber).HasColumnName("ColoaderRANumber");
            this.Property(t => t.AWBPrintingSecurityStatusId).HasColumnName("AWBPrintingSecurityStatusId");
            this.Property(t => t.AWBPrintingRANumber).HasColumnName("AWBPrintingRANumber");
            this.Property(t => t.AdditionalHandlingInfo).HasColumnName("AdditionalHandlingInfo");          
            this.Property(t => t.AWBPrintingRANumberEdited).HasColumnName("AWBPrintingRANumberEdited");
            this.Property(t => t.AdditionalHandlingInfoEdited).HasColumnName("AdditionalHandlingInfoEdited");
            this.Property(t => t.InterlineId).HasColumnName("InterlineId");
            this.Property(t => t.KCExpirationDate).HasColumnName("KCExpirationDate");
            this.Property(t => t.AirlinePrefix).HasColumnName("AirlinePrefix");
            this.Property(t => t.ManifestReason).HasColumnName("ManifestReason");
            this.Property(t => t.ManifestStatusCode).HasColumnName("ManifestStatusCode");
            this.Property(t => t.StatusLocation).HasColumnName("StatusLocation");
            this.Property(t => t.DepartureArrivalFromDate).HasColumnName("DepartureArrivalFromDate");
            this.Property(t => t.DepartureArrivalToDate).HasColumnName("DepartureArrivalToDate");
            this.Property(t => t.ProrateReceivables).HasColumnName("ProrateReceivables");
            this.Property(t => t.DocumentsClosingDate).HasColumnName("DocumentsClosingDate");
            this.Property(t => t.OBLTypeCode).HasColumnName("OBLTypeCode");
            this.Property(t => t.CutoffDate).HasColumnName("CutoffDate");
            this.Property(t => t.AutomaticLastUpdateDate).HasColumnName("AutomaticLastUpdateDate");

            this.Property(t => t.PreCarriageTransportModeId).HasColumnName("PreCarriageTransportModeId");
            this.Property(t => t.PreCarriageFromPortId).HasColumnName("PreCarriageFromPortId");
            this.Property(t => t.PreCarriageToPortId).HasColumnName("PreCarriageToPortId");
            this.Property(t => t.PreCarriageATD).HasColumnName("PreCarriageATD");
            this.Property(t => t.PreCarriageATA).HasColumnName("PreCarriageATA");
            this.Property(t => t.PreCarriageCarrierNumber).HasColumnName("PreCarriageCarrierNumber");
            this.Property(t => t.PreCarriageETA).HasColumnName("PreCarriageETA");
            this.Property(t => t.PreCarriageETD).HasColumnName("PreCarriageETD");
            this.Property(t => t.PreCarriageVesselId).HasColumnName("PreCarriageVesselId");
            this.Property(t => t.PreCarriageCarrierId).HasColumnName("PreCarriageCarrierId");

            this.Property(t => t.OnCarriageTransportModeId).HasColumnName("OnCarriageTransportModeId");
            this.Property(t => t.OnCarriageFromPortId).HasColumnName("OnCarriageFromPortId");
            this.Property(t => t.OnCarriageToPortId).HasColumnName("OnCarriageToPortId");
            this.Property(t => t.OnCarriageATD).HasColumnName("OnCarriageATD");
            this.Property(t => t.OnCarriageATA).HasColumnName("OnCarriageATA");
            this.Property(t => t.OnCarriageCarrierNumber).HasColumnName("OnCarriageCarrierNumber");
            this.Property(t => t.OnCarriageETD).HasColumnName("OnCarriageETD");
            this.Property(t => t.OnCarriageETA).HasColumnName("OnCarriageETA");
            this.Property(t => t.OnCarriageVesselId).HasColumnName("OnCarriageVesselId");
            this.Property(t => t.OnCarriageCarrierId).HasColumnName("OnCarriageCarrierId");
            this.Property(t => t.SplitOnCarriage).HasColumnName("SplitOnCarriage");
            this.Property(t => t.OnCarriageAdditionalTransportModeCode).HasColumnName("OnCarriageAdditionalTransportModeCode");

            //#if ORACLE_DB
            string dbms = System.Configuration.ConfigurationManager.AppSettings.Get("DBMS");
            if (dbms == "oracle")
            {
                this.Property(t => t.AWBPrintingSecurityStatusEdited).HasColumnName("AWBPrintSecurityStatusEdited");
                this.Property(t => t.Transshipment1AdditionalMAWBOBLBL).HasColumnName("Via1AdditionalMAWBOBLBL");
                this.Property(t => t.Transshipment2AdditionalMAWBOBLBL).HasColumnName("Via2AdditionalMAWBOBLBL");
                this.Property(t => t.Transshipment3AdditionalMAWBOBLBL).HasColumnName("Via3AdditionalMAWBOBLBL");
                this.Property(t => t.MainCarriageFinalDestinationPortId).HasColumnName("MainCarriageFinalPortId");
                this.Property(t => t.MainCarriageFinalDestinationETA).HasColumnName("MainCarriageFinalDestETA");
                this.Property(t => t.MainCarriageFinalDestinationATA).HasColumnName("MainCarriageFinalDestATA");
            }

//#else
            else
            {
                this.Property(t => t.AWBPrintingSecurityStatusEdited).HasColumnName("AWBPrintingSecurityStatusEdited");
                this.Property(t => t.Transshipment1AdditionalMAWBOBLBL).HasColumnName("Transshipment1AdditionalMAWBOBLBL");
                this.Property(t => t.Transshipment2AdditionalMAWBOBLBL).HasColumnName("Transshipment2AdditionalMAWBOBLBL");
                this.Property(t => t.Transshipment3AdditionalMAWBOBLBL).HasColumnName("Transshipment3AdditionalMAWBOBLBL");
                this.Property(t => t.MainCarriageFinalDestinationPortId).HasColumnName("MainCarriageFinalDestinationPortId");
                this.Property(t => t.MainCarriageFinalDestinationETA).HasColumnName("MainCarriageFinalDestinationETA");
                this.Property(t => t.MainCarriageFinalDestinationATA).HasColumnName("MainCarriageFinalDestinationATA");
            }
//#endif

            // Relationships
            this.HasOptional(t => t.FromPartnerAddress).WithMany().HasForeignKey(d => d.MainCarriageFromAddressId);
            this.HasOptional(t => t.ToPartnerAddress).WithMany().HasForeignKey(d => d.MainCarriageToAddressId);
            this.HasOptional(t => t.FromPartnerCard).WithMany().HasForeignKey(d => d.MainCarriageFromPartnerId);
            this.HasOptional(t => t.MainCarriageCarrierCard).WithMany().HasForeignKey(d => d.MainCarriageCarrierId);
            this.HasOptional(t => t.ToPartnerCard).WithMany().HasForeignKey(d => d.MainCarriageToPartnerId);
            this.HasOptional(t => t.Transshipment1CarrierCard).WithMany().HasForeignKey(d => d.Transshipment1CarrierId);
            this.HasOptional(t => t.Transshipment2CarrierCard).WithMany().HasForeignKey(d => d.Transshipment2CarrierId);
            this.HasOptional(t => t.Transshipment3CarrierCard).WithMany().HasForeignKey(d => d.Transshipment3CarrierId);
            this.HasOptional(t => t.FWBStatus).WithMany().HasForeignKey(d => d.FWBStatusCode);
            this.HasOptional(t => t.MainCarriageFinalDestinationPort).WithMany().HasForeignKey(d => d.MainCarriageFinalDestinationPortId);
            this.HasOptional(t => t.MainCarriageFromPort).WithMany().HasForeignKey(d => d.MainCarriageFromPortId);
            this.HasOptional(t => t.MainCarriageToPort).WithMany().HasForeignKey(d => d.MainCarriageToPortId);
            this.HasOptional(t => t.Transshipment1FromPort).WithMany().HasForeignKey(d => d.Transshipment1FromPortId);
            this.HasOptional(t => t.Transshipment1ToPort).WithMany().HasForeignKey(d => d.Transshipment1ToPortId);
            this.HasOptional(t => t.Transshipment2FromPort).WithMany().HasForeignKey(d => d.Transshipment2FromPortId);
            this.HasOptional(t => t.Transshipment2ToPort).WithMany().HasForeignKey(d => d.Transshipment2ToPortId);
            this.HasOptional(t => t.Transshipment3FromPort).WithMany().HasForeignKey(d => d.Transshipment3FromPortId);
            this.HasOptional(t => t.Transshipment3ToPort).WithMany().HasForeignKey(d => d.Transshipment3ToPortId);
            this.HasOptional(t => t.MainCarriageVessel).WithMany().HasForeignKey(d => d.MainCarriageVesselId);
            this.HasRequired(t => t.Shipment).WithOptional(t => t.ShipmentMasterData);
            this.HasOptional(t => t.Transshipment1Vessel).WithMany().HasForeignKey(d => d.Transshipment1VesselId);
            this.HasOptional(t => t.Transshipment2Vessel).WithMany().HasForeignKey(d => d.Transshipment2VesselId);
            this.HasOptional(t => t.Transshipment3Vessel).WithMany().HasForeignKey(d => d.Transshipment3VesselId);
            this.HasOptional(t => t.CargonautFWBStatus).WithMany().HasForeignKey(d => d.CargonautFWBStatusCode);
            this.HasOptional(t => t.AWBSpecialHandlingCode).WithMany().HasForeignKey(d => d.AWBPrintingSecurityStatusId);
            this.HasOptional(t => t.InterlineCard).WithMany().HasForeignKey(d => d.InterlineId);
            this.HasOptional(t => t.ManifestStatus).WithMany().HasForeignKey(d => d.ManifestStatusCode);
            this.HasOptional(t => t.OBLType).WithMany().HasForeignKey(d => d.OBLTypeCode);

            this.HasOptional(t => t.PreCarriageCarrierCard).WithMany().HasForeignKey(d => d.PreCarriageCarrierId);
            this.HasOptional(t => t.PreCarriageFromPort).WithMany().HasForeignKey(d => d.PreCarriageFromPortId);
            this.HasOptional(t => t.PreCarriageToPort).WithMany().HasForeignKey(d => d.PreCarriageToPortId);
            this.HasOptional(t => t.PreCarriageVessel).WithMany().HasForeignKey(d => d.PreCarriageVesselId);

            this.HasOptional(t => t.OnCarriageCarrierCard).WithMany().HasForeignKey(d => d.OnCarriageCarrierId);
            this.HasOptional(t => t.OnCarriageFromPort).WithMany().HasForeignKey(d => d.OnCarriageFromPortId);
            this.HasOptional(t => t.OnCarriageToPort).WithMany().HasForeignKey(d => d.OnCarriageToPortId);
            this.HasOptional(t => t.OnCarriageVessel).WithMany().HasForeignKey(d => d.OnCarriageVesselId);
            this.HasOptional(t => t.OnCarriageAdditionalTransportMode).WithMany().HasForeignKey(d => d.OnCarriageAdditionalTransportModeCode);
        }
    }
}
