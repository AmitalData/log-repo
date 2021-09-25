using Simplog.Data.ShipmentsModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.ShipmentsModel.Mapping
{
    public class ContainerMap : EntityTypeConfiguration<Container>
    {
        public ContainerMap()
        {
            this.HasKey(t => t.Id);
            this.Property(t => t.Id).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.ShipmentPackagesId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.MainCarriageCarrierId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.MainCarriageVesselId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.SearchFields).IsMaxLength().IsUnicode(true);
            this.Property(t => t.ContainerNumber).HasMaxLength(20).IsUnicode(false);
            this.Property(t => t.MainCarriageCarrierNumber).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.Master).HasMaxLength(20).IsUnicode(false);
            this.Property(t => t.ShipmentId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.ShipmentPreCarriageFromId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.ShipmentPreCarriageToId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.ShipmentMainCarriageFromId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.ShipmentMainCarriageToId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.ShipmentTransshipment1FromId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.ShipmentTransshipment1ToId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.ShipmentTransshipment2FromId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.ShipmentTransshipment3FromId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.ShipmentTransshipment2ToId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.ShipmentTransshipment3ToId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.ShipmentOnCarriageFromId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.ShipmentOnCarriageToId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.AvailabilityLocation).HasMaxLength(10).IsUnicode(false);
            this.Property(t => t.ShipmentStatusId).HasMaxLength(15).IsUnicode(false);

            this.ToTable("Containers");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.ShipmentPackagesId).HasColumnName("Code");
            this.Property(t => t.CreateDate).HasColumnName("CreateDate");
            this.Property(t => t.UpdateDate).HasColumnName("UpdateDate");
            this.Property(t => t.CreatedByUserId).HasColumnName("CreatedByUserId");
            this.Property(t => t.UpdatedByUserId).HasColumnName("UpdatedByUserId");
            this.Property(t => t.ContainerNumber).HasColumnName("ContainerNumber");
            this.Property(t => t.MainCarriageCarrierId).HasColumnName("MainCarriageCarrierId");
            this.Property(t => t.MainCarriageCarrierNumber).HasColumnName("MainCarriageCarrierNumber");
            this.Property(t => t.MainCarriageATA).HasColumnName("MainCarriageATA");
            this.Property(t => t.MainCarriageATD).HasColumnName("MainCarriageATD");
            this.Property(t => t.MainCarriageETA).HasColumnName("MainCarriageETA");
            this.Property(t => t.MainCarriageETD).HasColumnName("MainCarriageETD");
            this.Property(t => t.DischargeDate).HasColumnName("DischargeDate");
            this.Property(t => t.Master).HasColumnName("Master");
            this.Property(t => t.SearchFields).HasColumnName("SearchFields");
            this.Property(t => t.ShipmentId).HasColumnName("ShipmentId");
            this.Property(t => t.EstimatedEmptyPickupDate).HasColumnName("EstimatedEmptyPickupDate");
            this.Property(t => t.ActualEmptyPickupDate).HasColumnName("ActualEmptyPickupDate");
            this.Property(t => t.CurrentStatus).HasColumnName("CurrentStatus");
            this.Property(t => t.CurrentStatusDate).HasColumnName("CurrentStatusDate");
            this.Property(t => t.HasContainerException).HasColumnName("HasContainerException");
            this.Property(t => t.EmptyPickupLocation).HasColumnName("EmptyPickupLocation");
            this.Property(t => t.CurrentLocation).HasColumnName("CurrentLocation");
            this.Property(t => t.DestinationLocation).HasColumnName("DestinationLocation");
            this.Property(t => t.DepartureLocation).HasColumnName("DepartureLocation");

            this.Property(t => t.ShipmentFirstPickupFrom).HasColumnName("ShipmentFirstPickupFrom");
            this.Property(t => t.ShipmentFirstPickupTo).HasColumnName("ShipmentFirstPickupTo");
            this.Property(t => t.ShipmentPreCarriageFromId).HasColumnName("ShipmentPreCarriageFromId");
            this.Property(t => t.ShipmentPreCarriageToId).HasColumnName("ShipmentPreCarriageToId");
            this.Property(t => t.ShipmentMainCarriageFromId).HasColumnName("ShipmentMainCarriageFromId");
            this.Property(t => t.ShipmentMainCarriageToId).HasColumnName("ShipmentMainCarriageToId");
            this.Property(t => t.ShipmentTransshipment1FromId).HasColumnName("ShipmentTransshipment1FromId");
            this.Property(t => t.ShipmentTransshipment1ToId).HasColumnName("ShipmentTransshipment1ToId");
            this.Property(t => t.ShipmentTransshipment2FromId).HasColumnName("ShipmentTransshipment2FromId");
            this.Property(t => t.ShipmentTransshipment2ToId).HasColumnName("ShipmentTransshipment2ToId");
            this.Property(t => t.ShipmentTransshipment3FromId).HasColumnName("ShipmentTransshipment3FromId");            
            this.Property(t => t.ShipmentTransshipment3ToId).HasColumnName("ShipmentTransshipment3ToId");
            this.Property(t => t.ShipmentOnCarriageFromId).HasColumnName("ShipmentOnCarriageFromId");
            this.Property(t => t.ShipmentOnCarriageToId).HasColumnName("ShipmentOnCarriageToId");
            this.Property(t => t.ShipmentLastDeliveryFrom).HasColumnName("ShipmentLastDeliveryFrom");
            this.Property(t => t.ShipmentLastDeliveryTo).HasColumnName("ShipmentLastDeliveryTo");
            this.Property(t => t.OriginLocation).HasColumnName("OriginLocation");
            this.Property(t => t.EstimatedOriginPickup).HasColumnName("EstimatedOriginPickup");
            this.Property(t => t.ActualOriginPickup).HasColumnName("ActualOriginPickup");//
            this.Property(t => t.POLLocation).HasColumnName("POLLocation");
            this.Property(t => t.EstimatedPOLArrival).HasColumnName("EstimatedPOLArrival");
            this.Property(t => t.ActualPOLArrival).HasColumnName("ActualPOLArrival");
            this.Property(t => t.EstimatedPOLLoaded).HasColumnName("EstimatedPOLLoaded");
            this.Property(t => t.ActualPOLLoaded).HasColumnName("ActualPOLLoaded");
            this.Property(t => t.EstimatedPOLVesselDeparture).HasColumnName("EstimatedPOLVesselDeparture");
            this.Property(t => t.ActualPOLVesselDeparture).HasColumnName("ActualPOLVesselDeparture");

            this.Property(t => t.TransshipmentCount).HasColumnName("TransshipmentCount");
            this.Property(t => t.Transshipment1Location).HasColumnName("Transshipment1Location");//
            this.Property(t => t.EstimatedTrans1VesselArrival).HasColumnName("EstimatedTrans1VesselArrival");
            this.Property(t => t.ActualTransshipment1VesselArrival).HasColumnName("ActualTransshipment1VesselArrival");
            this.Property(t => t.EstimatedTransshipment1Discharge).HasColumnName("EstimatedTransshipment1Discharge");
            this.Property(t => t.ActualTransshipment1Discharge).HasColumnName("ActualTransshipment1Discharge");
            this.Property(t => t.EstimatedTransshipment1Loaded).HasColumnName("EstimatedTransshipment1Loaded");
            this.Property(t => t.ActualTransshipment1Loaded).HasColumnName("ActualTransshipment1Loaded");
            this.Property(t => t.EstimatedTrans1VesselDeparture).HasColumnName("EstimatedTransshipment1VesselDeparture");
            this.Property(t => t.ActualTrans1VesselDeparture).HasColumnName("ActualTransshipment1VesselDeparture");

            this.Property(t => t.Transshipment2Location).HasColumnName("Transshipment2Location");//
            this.Property(t => t.EstimatedTrans2VesselArrival).HasColumnName("EstimatedTrans2VesselArrival");
            this.Property(t => t.ActualTransshipment2VesselArrival).HasColumnName("ActualTransshipment2VesselArrival");
            this.Property(t => t.EstimatedTransshipment2Discharge).HasColumnName("EstimatedTransshipment2Discharge");
            this.Property(t => t.ActualTransshipment2Discharge).HasColumnName("ActualTransshipment2Discharge");
            this.Property(t => t.EstimatedTransshipment2Loaded).HasColumnName("EstimatedTransshipment2Loaded");
            this.Property(t => t.ActualTransshipment2Loaded).HasColumnName("ActualTransshipment2Loaded");
            this.Property(t => t.EstimatedTrans2VesselDeparture).HasColumnName("EstimatedTransshipment2VesselDeparture");
            this.Property(t => t.ActualTrans2VesselDeparture).HasColumnName("ActualTransshipment1Vesse2Departure");

            this.Property(t => t.Transshipment3Location).HasColumnName("Transshipment3Location");//
            this.Property(t => t.EstimatedTrans3VesselArrival).HasColumnName("EstimatedTrans3VesselArrival");
            this.Property(t => t.ActualTransshipment3VesselArrival).HasColumnName("ActualTransshipment3VesselArrival");
            this.Property(t => t.EstimatedTransshipment3Discharge).HasColumnName("EstimatedTransshipment3Discharge");
            this.Property(t => t.ActualTransshipment3Discharge).HasColumnName("ActualTransshipment3Discharge");
            this.Property(t => t.EstimatedTransshipment3Loaded).HasColumnName("EstimatedTransshipment3Loaded");
            this.Property(t => t.ActualTransshipment3Loaded).HasColumnName("ActualTransshipment3Loaded");
            this.Property(t => t.EstimatedTrans3VesselDeparture).HasColumnName("EstimatedTransshipment3VesselDeparture");
            this.Property(t => t.ActualTrans3VesselDeparture).HasColumnName("ActualTransshipment3VesselDeparture");

            this.Property(t => t.Transshipment4Location).HasColumnName("Transshipment4Location");//
            this.Property(t => t.EstimatedTrans4VesselArrival).HasColumnName("EstimatedTrans4VesselArrival");
            this.Property(t => t.ActualTransshipment4VesselArrival).HasColumnName("ActualTransshipment4VesselArrival");
            this.Property(t => t.EstimatedTransshipment4Discharge).HasColumnName("EstimatedTransshipment4Discharge");
            this.Property(t => t.ActualTransshipment4Discharge).HasColumnName("ActualTransshipment4Discharge");
            this.Property(t => t.EstimatedTransshipment4Loaded).HasColumnName("EstimatedTransshipment4Loaded");
            this.Property(t => t.ActualTransshipment4Loaded).HasColumnName("ActualTransshipment4Loaded");
            this.Property(t => t.EstimatedTrans4VesselDeparture).HasColumnName("EstimatedTransshipment4VesselDeparture");
            this.Property(t => t.ActualTrans4VesselDeparture).HasColumnName("ActualTransshipment4VesselDeparture");

            this.Property(t => t.Leg1Vessel).HasColumnName("Leg1Vessel");
            this.Property(t => t.Leg1Voyage).HasColumnName("Leg1Voyage");
            this.Property(t => t.Leg2Vessel).HasColumnName("Leg2Vessel");
            this.Property(t => t.Leg2Voyage).HasColumnName("Leg2Voyage");
            this.Property(t => t.Leg3Vessel).HasColumnName("Leg3Vessel");
            this.Property(t => t.Leg3Voyage).HasColumnName("Leg3Voyage");
            this.Property(t => t.Leg4Vessel).HasColumnName("Leg4Vessel");
            this.Property(t => t.Leg4Voyage).HasColumnName("Leg4Voyage");
            this.Property(t => t.Leg5Vessel).HasColumnName("Leg5Vessel");
            this.Property(t => t.Leg5Voyage).HasColumnName("Leg5Voyage");

            this.Property(t => t.PODLocation).HasColumnName("PODLocation");
            this.Property(t => t.EstimatedPODVesselArrival).HasColumnName("EstimatedPODVesselArrival");
            this.Property(t => t.ActualPODVesselArrival).HasColumnName("ActualPODVesselArrival");
            this.Property(t => t.EstimatedPODDischarge).HasColumnName("EstimatedPODDischarge");
            this.Property(t => t.ActualPODDischarge).HasColumnName("ActualPODDischarge");
            this.Property(t => t.EstimatedPODDeparture).HasColumnName("EstimatedPODDeparture");
            this.Property(t => t.ActualPODDeparture).HasColumnName("ActualPODDeparture");
            this.Property(t => t.DeliveryLocation).HasColumnName("DeliveryLocation");
            this.Property(t => t.EstimatedDelivery).HasColumnName("EstimatedDelivery");
            this.Property(t => t.ActualDelivery).HasColumnName("ActualDelivery");
            this.Property(t => t.LIFLocation).HasColumnName("LIFLocation");
            this.Property(t => t.EstimatedLIFArrival).HasColumnName("EstimatedLIFArrival");
            this.Property(t => t.ActualLIFArrival).HasColumnName("ActualLIFArrival");
            this.Property(t => t.EstimatedLIFDeparture).HasColumnName("EstimatedLIFDeparture");
            this.Property(t => t.ActualLIFDeparture).HasColumnName("ActualLIFDeparture");
            this.Property(t => t.GateIn).HasColumnName("GateIn");
            this.Property(t => t.GateOut).HasColumnName("GateOut");
            this.Property(t => t.EmptyReturnLocation).HasColumnName("EmptyReturnLocation");
            this.Property(t => t.EstimatedEmptyReturn).HasColumnName("EstimatedEmptyReturn");
            this.Property(t => t.ActualEmptyReturn).HasColumnName("ActualEmptyReturn");
            this.Property(t => t.CustomsReleaseState).HasColumnName("CustomsReleaseState");
            this.Property(t => t.CustomsReleaseDate).HasColumnName("CustomsReleaseDate");
            this.Property(t => t.CarrierReleaseState).HasColumnName("CarrierReleaseState");
            this.Property(t => t.CarrierReleaseDate).HasColumnName("CarrierReleaseDate");
            this.Property(t => t.AvailablityDate).HasColumnName("AvailablityDate");
            this.Property(t => t.AvailabilityLocation).HasColumnName("AvailabilityLocation");
            this.Property(t => t.FreeDays).HasColumnName("FreeDays");
            this.Property(t => t.ShipmentStatusId).HasColumnName("ShipmentStatusId");
            this.Property(t => t.TerminalPhone).HasColumnName("TerminalPhone");
            this.Property(t => t.TerminalAddress).HasColumnName("TerminalAddress");
            this.Property(t => t.TerminalId).HasColumnName("TerminalId");
            this.Property(t => t.LastFreeDayDate).HasColumnName("LastFreeDayDate");


            this.HasOptional(t => t.CarrierCard).WithMany().HasForeignKey(d => d.MainCarriageCarrierId).WillCascadeOnDelete(false); 
            this.HasOptional(t => t.ShipmentPackage).WithMany().HasForeignKey(d => d.ShipmentPackagesId).WillCascadeOnDelete(false);
            this.HasOptional(t => t.VesselCard).WithMany().HasForeignKey(d => d.MainCarriageVesselId).WillCascadeOnDelete(false); 

            this.HasOptional(t => t.ShipmentPreCarriageFromPort).WithMany().HasForeignKey(d => d.ShipmentPreCarriageFromId); 
            this.HasOptional(t => t.ShipmentPreCarriageToPort).WithMany().HasForeignKey(d => d.ShipmentPreCarriageToId);
            this.HasOptional(t => t.ShipmentMainCarriageFromPort).WithMany().HasForeignKey(d => d.ShipmentMainCarriageFromId); 
            this.HasOptional(t => t.ShipmentMainCarriageToPort).WithMany().HasForeignKey(d => d.ShipmentMainCarriageToId); 
            this.HasOptional(t => t.ShipmentTransshipment1FromPort).WithMany().HasForeignKey(d => d.ShipmentTransshipment1FromId);
            this.HasOptional(t => t.ShipmentTransshipment1ToPort).WithMany().HasForeignKey(d => d.ShipmentTransshipment1ToId); 
            this.HasOptional(t => t.ShipmentTransshipment2FromPort).WithMany().HasForeignKey(d => d.ShipmentTransshipment2FromId); 
            this.HasOptional(t => t.ShipmentTransshipment2ToPort).WithMany().HasForeignKey(d => d.ShipmentTransshipment2ToId);
            this.HasOptional(t => t.ShipmentTransshipment3FromPort).WithMany().HasForeignKey(d => d.ShipmentTransshipment3FromId); 
            this.HasOptional(t => t.ShipmentTransshipment3ToPort).WithMany().HasForeignKey(d => d.ShipmentTransshipment3ToId); 
            this.HasOptional(t => t.ShipmentOnCarriageFromPort).WithMany().HasForeignKey(d => d.ShipmentOnCarriageFromId);
            this.HasOptional(t => t.ShipmentOnCarriageToPort).WithMany().HasForeignKey(d => d.ShipmentOnCarriageToId);
            this.HasOptional(t => t.ShipmentEntityStatus).WithMany().HasForeignKey(d => d.ShipmentStatusId);
            this.HasOptional(t => t.TerminalCard).WithMany().HasForeignKey(d => d.TerminalId);

        }
    }
}
