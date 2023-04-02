using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.Infrastructure.Data.Models.AuditLog;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using System.Collections.Generic;

namespace Logitude.BL.ShipmentsModel.Tools.DataMapping
{
    public partial class ShipmentMapping
    {
        public static void MapContainer(ContainerPM containerPM, Container container, bool isNewEntity, List<FieldChange> fieldChanges)
        {
            ShipmentMapping.MapContainerConcurrencyFields(containerPM, container, isNewEntity, fieldChanges);
            BuildSearchField(containerPM, container, fieldChanges);
        }

        public static void MapContainerFields(ContainerPM containerPM, Container container, bool isNewEntity, List<FieldChange> fieldChanges)
        {
            if (isNewEntity)
            {
                FieldChange.Add(container.Id, containerPM.Id, nameof(containerPM.Id), fieldChanges);
                container.Id = containerPM.Id;
                
                FieldChange.Add(container.Tenant, containerPM.Tenant, nameof(containerPM.Tenant), fieldChanges);
                container.Tenant = containerPM.Tenant;
                
                FieldChange.Add(container.CreateDate, containerPM.CreateDate, nameof(containerPM.CreateDate), fieldChanges);
                container.CreateDate = containerPM.CreateDate;
                
                FieldChange.Add(container.CreatedByUserId, containerPM.CreatedByUserId, nameof(containerPM.CreatedByUserId), fieldChanges);
                container.CreatedByUserId = containerPM.CreatedByUserId;
            }
            
            FieldChange.Add(container.UpdateDate, containerPM.UpdateDate, nameof(containerPM.UpdateDate), fieldChanges);
            container.UpdateDate = containerPM.UpdateDate;
            
            FieldChange.Add(container.UpdatedByUserId, containerPM.UpdatedByUserId, nameof(containerPM.UpdatedByUserId), fieldChanges);
            container.UpdatedByUserId = containerPM.UpdatedByUserId;
            
            FieldChange.Add(container.DischargeDate, containerPM.DischargeDate, nameof(containerPM.DischargeDate), fieldChanges);
            container.DischargeDate = containerPM.DischargeDate;
            
            FieldChange.Add(container.ContainerNumber, containerPM.ContainerNumber, nameof(containerPM.ContainerNumber), fieldChanges);
            container.ContainerNumber = containerPM.ContainerNumber;
            
            FieldChange.Add(container.ActualEmptyPickupDate, containerPM.ActualEmptyPickupDate, nameof(containerPM.ActualEmptyPickupDate), fieldChanges);
            container.ActualEmptyPickupDate = containerPM.ActualEmptyPickupDate;
            
            FieldChange.Add(container.EstimatedEmptyPickupDate, containerPM.EstimatedEmptyPickupDate, nameof(containerPM.EstimatedEmptyPickupDate), fieldChanges);
            container.EstimatedEmptyPickupDate = containerPM.EstimatedEmptyPickupDate;
            
            FieldChange.Add(container.CurrentStatus, containerPM.CurrentStatus, nameof(containerPM.CurrentStatus), fieldChanges);
            container.CurrentStatus = containerPM.CurrentStatus;
            
            FieldChange.Add(container.CurrentStatusDate, containerPM.CurrentStatusDate, nameof(containerPM.CurrentStatusDate), fieldChanges);
            container.CurrentStatusDate = containerPM.CurrentStatusDate;
            
            FieldChange.Add(container.HasContainerException, containerPM.HasContainerException, nameof(containerPM.HasContainerException), fieldChanges);
            container.HasContainerException = containerPM.HasContainerException;
            
            FieldChange.Add(container.CurrentLocation, containerPM.CurrentLocation, nameof(containerPM.CurrentLocation), fieldChanges);
            container.CurrentLocation = containerPM.CurrentLocation;
            
            FieldChange.Add(container.EmptyPickupLocation, containerPM.EmptyPickupLocation, nameof(containerPM.EmptyPickupLocation), fieldChanges);
            container.EmptyPickupLocation = containerPM.EmptyPickupLocation;
            
            FieldChange.Add(container.DepartureLocation, containerPM.DepartureLocation, nameof(containerPM.DepartureLocation), fieldChanges);
            container.DepartureLocation = containerPM.DepartureLocation;
            
            FieldChange.Add(container.DestinationLocation, containerPM.DestinationLocation, nameof(containerPM.DestinationLocation), fieldChanges);
            container.DestinationLocation = containerPM.DestinationLocation;
            
            FieldChange.Add(container.OnCarriageLocation, containerPM.OnCarriageLocation, nameof(containerPM.OnCarriageLocation), fieldChanges);
            container.OnCarriageLocation = containerPM.OnCarriageLocation;
            
            FieldChange.Add(container.OnCarriageETD, containerPM.OnCarriageETD, nameof(containerPM.OnCarriageETD), fieldChanges);
            container.OnCarriageETD = containerPM.OnCarriageETD;
            
            FieldChange.Add(container.OnCarriageATD, containerPM.OnCarriageATD, nameof(containerPM.OnCarriageATD), fieldChanges);
            container.OnCarriageATD = containerPM.OnCarriageATD;
            
            FieldChange.Add(container.POLLocation, containerPM.POLLocation, nameof(containerPM.POLLocation), fieldChanges);
            container.POLLocation = containerPM.POLLocation;
            
            FieldChange.Add(container.EstimatedPOLArrival, containerPM.EstimatedPOLArrival, nameof(containerPM.EstimatedPOLArrival), fieldChanges);
            container.EstimatedPOLArrival = containerPM.EstimatedPOLArrival;
            
            FieldChange.Add(container.ActualPOLArrival, containerPM.ActualPOLArrival, nameof(containerPM.ActualPOLArrival), fieldChanges);
            container.ActualPOLArrival = containerPM.ActualPOLArrival;
            
            FieldChange.Add(container.EstimatedPOLLoaded, containerPM.EstimatedPOLLoaded, nameof(containerPM.EstimatedPOLLoaded), fieldChanges);
            container.EstimatedPOLLoaded = containerPM.EstimatedPOLLoaded;
            
            FieldChange.Add(container.ActualPOLLoaded, containerPM.ActualPOLLoaded, nameof(containerPM.ActualPOLLoaded), fieldChanges);
            container.ActualPOLLoaded = containerPM.ActualPOLLoaded;
            
            FieldChange.Add(container.EstimatedPOLVesselDeparture, containerPM.EstimatedPOLVesselDeparture, nameof(containerPM.EstimatedPOLVesselDeparture), fieldChanges);
            container.EstimatedPOLVesselDeparture = containerPM.EstimatedPOLVesselDeparture;
            
            FieldChange.Add(container.ActualPOLVesselDeparture, containerPM.ActualPOLVesselDeparture, nameof(containerPM.ActualPOLVesselDeparture), fieldChanges);
            container.ActualPOLVesselDeparture = containerPM.ActualPOLVesselDeparture;
            
            FieldChange.Add(container.TransshipmentCount, containerPM.TransshipmentCount, nameof(containerPM.TransshipmentCount), fieldChanges);
            container.TransshipmentCount = containerPM.TransshipmentCount;
            
            FieldChange.Add(container.Transshipment1Location, containerPM.Transshipment1Location, nameof(containerPM.Transshipment1Location), fieldChanges);
            container.Transshipment1Location = containerPM.Transshipment1Location;
            
            FieldChange.Add(container.EstimatedTrans1VesselArrival, containerPM.EstimatedTrans1VesselArrival, nameof(containerPM.EstimatedTrans1VesselArrival), fieldChanges);
            container.EstimatedTrans1VesselArrival = containerPM.EstimatedTrans1VesselArrival;
            
            FieldChange.Add(container.ActualTransshipment1VesselArrival, containerPM.ActualTransshipment1VesselArrival, nameof(containerPM.ActualTransshipment1VesselArrival), fieldChanges);
            container.ActualTransshipment1VesselArrival = containerPM.ActualTransshipment1VesselArrival;
            
            FieldChange.Add(container.EstimatedTransshipment1Discharge, containerPM.EstimatedTransshipment1Discharge, nameof(containerPM.EstimatedTransshipment1Discharge), fieldChanges);
            container.EstimatedTransshipment1Discharge = containerPM.EstimatedTransshipment1Discharge;
            
            FieldChange.Add(container.ActualTransshipment1Discharge, containerPM.ActualTransshipment1Discharge, nameof(containerPM.ActualTransshipment1Discharge), fieldChanges);
            container.ActualTransshipment1Discharge = containerPM.ActualTransshipment1Discharge;
            
            FieldChange.Add(container.EstimatedTransshipment1Loaded, containerPM.EstimatedTransshipment1Loaded, nameof(containerPM.EstimatedTransshipment1Loaded), fieldChanges);
            container.EstimatedTransshipment1Loaded = containerPM.EstimatedTransshipment1Loaded;
            
            FieldChange.Add(container.ActualTransshipment1Loaded, containerPM.ActualTransshipment1Loaded, nameof(containerPM.ActualTransshipment1Loaded), fieldChanges);
            container.ActualTransshipment1Loaded = containerPM.ActualTransshipment1Loaded;
            
            FieldChange.Add(container.EstimatedTrans1VesselDeparture, containerPM.EstimatedTrans1VesselDeparture, nameof(containerPM.EstimatedTrans1VesselDeparture), fieldChanges);
            container.EstimatedTrans1VesselDeparture = containerPM.EstimatedTrans1VesselDeparture;
            
            FieldChange.Add(container.ActualTrans1VesselDeparture, containerPM.ActualTrans1VesselDeparture, nameof(containerPM.ActualTrans1VesselDeparture), fieldChanges);
            container.ActualTrans1VesselDeparture = containerPM.ActualTrans1VesselDeparture;
            
            FieldChange.Add(container.Transshipment2Location, containerPM.Transshipment2Location, nameof(containerPM.Transshipment2Location), fieldChanges);
            container.Transshipment2Location = containerPM.Transshipment2Location;
            
            FieldChange.Add(container.EstimatedTrans2VesselArrival, containerPM.EstimatedTrans2VesselArrival, nameof(containerPM.EstimatedTrans2VesselArrival), fieldChanges);
            container.EstimatedTrans2VesselArrival = containerPM.EstimatedTrans2VesselArrival;
            
            FieldChange.Add(container.ActualTransshipment2VesselArrival, containerPM.ActualTransshipment2VesselArrival, nameof(containerPM.ActualTransshipment2VesselArrival), fieldChanges);
            container.ActualTransshipment2VesselArrival = containerPM.ActualTransshipment2VesselArrival;
            
            FieldChange.Add(container.EstimatedTransshipment2Discharge, containerPM.EstimatedTransshipment2Discharge, nameof(containerPM.EstimatedTransshipment2Discharge), fieldChanges);
            container.EstimatedTransshipment2Discharge = containerPM.EstimatedTransshipment2Discharge;
            
            FieldChange.Add(container.ActualTransshipment2Discharge, containerPM.ActualTransshipment2Discharge, nameof(containerPM.ActualTransshipment2Discharge), fieldChanges);
            container.ActualTransshipment2Discharge = containerPM.ActualTransshipment2Discharge;
            
            FieldChange.Add(container.EstimatedTransshipment2Loaded, containerPM.EstimatedTransshipment2Loaded, nameof(containerPM.EstimatedTransshipment2Loaded), fieldChanges);
            container.EstimatedTransshipment2Loaded = containerPM.EstimatedTransshipment2Loaded;
            
            FieldChange.Add(container.ActualTransshipment2Loaded, containerPM.ActualTransshipment2Loaded, nameof(containerPM.ActualTransshipment2Loaded), fieldChanges);
            container.ActualTransshipment2Loaded = containerPM.ActualTransshipment2Loaded;
            
            FieldChange.Add(container.EstimatedTrans2VesselDeparture, containerPM.EstimatedTrans2VesselDeparture, nameof(containerPM.EstimatedTrans2VesselDeparture), fieldChanges);
            container.EstimatedTrans2VesselDeparture = containerPM.EstimatedTrans2VesselDeparture;
            
            FieldChange.Add(container.ActualTrans2VesselDeparture, containerPM.ActualTrans2VesselDeparture, nameof(containerPM.ActualTrans2VesselDeparture), fieldChanges);
            container.ActualTrans2VesselDeparture = containerPM.ActualTrans2VesselDeparture;
            
            FieldChange.Add(container.Transshipment3Location, containerPM.Transshipment3Location, nameof(containerPM.Transshipment3Location), fieldChanges);
            container.Transshipment3Location = containerPM.Transshipment3Location;
            
            FieldChange.Add(container.EstimatedTrans3VesselArrival, containerPM.EstimatedTrans3VesselArrival, nameof(containerPM.EstimatedTrans3VesselArrival), fieldChanges);
            container.EstimatedTrans3VesselArrival = containerPM.EstimatedTrans3VesselArrival;
            
            FieldChange.Add(container.ActualTransshipment3VesselArrival, containerPM.ActualTransshipment3VesselArrival, nameof(containerPM.ActualTransshipment3VesselArrival), fieldChanges);
            container.ActualTransshipment3VesselArrival = containerPM.ActualTransshipment3VesselArrival;
            
            FieldChange.Add(container.EstimatedTransshipment3Discharge, containerPM.EstimatedTransshipment3Discharge, nameof(containerPM.EstimatedTransshipment3Discharge), fieldChanges);
            container.EstimatedTransshipment3Discharge = containerPM.EstimatedTransshipment3Discharge;
            
            FieldChange.Add(container.ActualTransshipment3Discharge, containerPM.ActualTransshipment3Discharge, nameof(containerPM.ActualTransshipment3Discharge), fieldChanges);
            container.ActualTransshipment3Discharge = containerPM.ActualTransshipment3Discharge;
            
            FieldChange.Add(container.EstimatedTransshipment3Loaded, containerPM.EstimatedTransshipment3Loaded, nameof(containerPM.EstimatedTransshipment3Loaded), fieldChanges);
            container.EstimatedTransshipment3Loaded = containerPM.EstimatedTransshipment3Loaded;
            
            FieldChange.Add(container.ActualTransshipment3Loaded, containerPM.ActualTransshipment3Loaded, nameof(containerPM.ActualTransshipment3Loaded), fieldChanges);
            container.ActualTransshipment3Loaded = containerPM.ActualTransshipment3Loaded;
            
            FieldChange.Add(container.EstimatedTrans3VesselDeparture, containerPM.EstimatedTrans3VesselDeparture, nameof(containerPM.EstimatedTrans3VesselDeparture), fieldChanges);
            container.EstimatedTrans3VesselDeparture = containerPM.EstimatedTrans3VesselDeparture;
            
            FieldChange.Add(container.ActualTrans3VesselDeparture, containerPM.ActualTrans3VesselDeparture, nameof(containerPM.ActualTrans3VesselDeparture), fieldChanges);
            container.ActualTrans3VesselDeparture = containerPM.ActualTrans3VesselDeparture;
            
            FieldChange.Add(container.Transshipment4Location, containerPM.Transshipment4Location, nameof(containerPM.Transshipment4Location), fieldChanges);
            container.Transshipment4Location = containerPM.Transshipment4Location;
            
            FieldChange.Add(container.EstimatedTrans4VesselArrival, containerPM.EstimatedTrans4VesselArrival, nameof(containerPM.EstimatedTrans4VesselArrival), fieldChanges);
            container.EstimatedTrans4VesselArrival = containerPM.EstimatedTrans4VesselArrival;
            
            FieldChange.Add(container.ActualTransshipment4VesselArrival, containerPM.ActualTransshipment4VesselArrival, nameof(containerPM.ActualTransshipment4VesselArrival), fieldChanges);
            container.ActualTransshipment4VesselArrival = containerPM.ActualTransshipment4VesselArrival;
            
            FieldChange.Add(container.EstimatedTransshipment4Discharge, containerPM.EstimatedTransshipment4Discharge, nameof(containerPM.EstimatedTransshipment4Discharge), fieldChanges);
            container.EstimatedTransshipment4Discharge = containerPM.EstimatedTransshipment4Discharge;
            
            FieldChange.Add(container.ActualTransshipment4Discharge, containerPM.ActualTransshipment4Discharge, nameof(containerPM.ActualTransshipment4Discharge), fieldChanges);
            container.ActualTransshipment4Discharge = containerPM.ActualTransshipment4Discharge;
            
            FieldChange.Add(container.EstimatedTransshipment4Loaded, containerPM.EstimatedTransshipment4Loaded, nameof(containerPM.EstimatedTransshipment4Loaded), fieldChanges);
            container.EstimatedTransshipment4Loaded = containerPM.EstimatedTransshipment4Loaded;
            
            FieldChange.Add(container.ActualTransshipment4Loaded, containerPM.ActualTransshipment4Loaded, nameof(containerPM.ActualTransshipment4Loaded), fieldChanges);
            container.ActualTransshipment4Loaded = containerPM.ActualTransshipment4Loaded;
            
            FieldChange.Add(container.EstimatedTrans4VesselDeparture, containerPM.EstimatedTrans4VesselDeparture, nameof(containerPM.EstimatedTrans4VesselDeparture), fieldChanges);
            container.EstimatedTrans4VesselDeparture = containerPM.EstimatedTrans4VesselDeparture;
            
            FieldChange.Add(container.ActualTrans4VesselDeparture, containerPM.ActualTrans4VesselDeparture, nameof(containerPM.ActualTrans4VesselDeparture), fieldChanges);
            container.ActualTrans4VesselDeparture = containerPM.ActualTrans4VesselDeparture;
            
            FieldChange.Add(container.Leg1Vessel, containerPM.Leg1Vessel, nameof(containerPM.Leg1Vessel), fieldChanges);
            container.Leg1Vessel = containerPM.Leg1Vessel;
            
            FieldChange.Add(container.Leg1Voyage, containerPM.Leg1Voyage, nameof(containerPM.Leg1Voyage), fieldChanges);
            container.Leg1Voyage = containerPM.Leg1Voyage;
            
            FieldChange.Add(container.Leg2Vessel, containerPM.Leg2Vessel, nameof(containerPM.Leg2Vessel), fieldChanges);
            container.Leg2Vessel = containerPM.Leg2Vessel;
            
            FieldChange.Add(container.Leg2Voyage, containerPM.Leg2Voyage, nameof(containerPM.Leg2Voyage), fieldChanges);
            container.Leg2Voyage = containerPM.Leg2Voyage;
            
            FieldChange.Add(container.Leg3Vessel, containerPM.Leg3Vessel, nameof(containerPM.Leg3Vessel), fieldChanges);
            container.Leg3Vessel = containerPM.Leg3Vessel;
            
            FieldChange.Add(container.Leg3Voyage, containerPM.Leg3Voyage, nameof(containerPM.Leg3Voyage), fieldChanges);
            container.Leg3Voyage = containerPM.Leg3Voyage;
            
            FieldChange.Add(container.Leg4Vessel, containerPM.Leg4Vessel, nameof(containerPM.Leg4Vessel), fieldChanges);
            container.Leg4Vessel = containerPM.Leg4Vessel;
            
            FieldChange.Add(container.Leg4Voyage, containerPM.Leg4Voyage, nameof(containerPM.Leg4Voyage), fieldChanges);
            container.Leg4Voyage = containerPM.Leg4Voyage;
            
            FieldChange.Add(container.Leg5Vessel, containerPM.Leg5Vessel, nameof(containerPM.Leg5Vessel), fieldChanges);
            container.Leg5Vessel = containerPM.Leg5Vessel;
            
            FieldChange.Add(container.Leg5Voyage, containerPM.Leg5Voyage, nameof(containerPM.Leg5Voyage), fieldChanges);
            container.Leg5Voyage = containerPM.Leg5Voyage;
            
            FieldChange.Add(container.PODLocation, containerPM.PODLocation, nameof(containerPM.PODLocation), fieldChanges);
            container.PODLocation = containerPM.PODLocation;
            
            FieldChange.Add(container.EstimatedPODVesselArrival, containerPM.EstimatedPODVesselArrival, nameof(containerPM.EstimatedPODVesselArrival), fieldChanges);
            container.EstimatedPODVesselArrival = containerPM.EstimatedPODVesselArrival;
            
            FieldChange.Add(container.ActualPODVesselArrival, containerPM.ActualPODVesselArrival, nameof(containerPM.ActualPODVesselArrival), fieldChanges);
            container.ActualPODVesselArrival = containerPM.ActualPODVesselArrival;
            
            FieldChange.Add(container.EstimatedPODDischarge, containerPM.EstimatedPODDischarge, nameof(containerPM.EstimatedPODDischarge), fieldChanges);
            container.EstimatedPODDischarge = containerPM.EstimatedPODDischarge;
            
            FieldChange.Add(container.ActualPODDischarge, containerPM.ActualPODDischarge, nameof(containerPM.ActualPODDischarge), fieldChanges);
            container.ActualPODDischarge = containerPM.ActualPODDischarge;
            
            FieldChange.Add(container.EstimatedPODDeparture, containerPM.EstimatedPODDeparture, nameof(containerPM.EstimatedPODDeparture), fieldChanges);
            container.EstimatedPODDeparture = containerPM.EstimatedPODDeparture;
            
            FieldChange.Add(container.ActualPODDeparture, containerPM.ActualPODDeparture, nameof(containerPM.ActualPODDeparture), fieldChanges);
            container.ActualPODDeparture = containerPM.ActualPODDeparture;
            
            FieldChange.Add(container.PreCarriageLocation, containerPM.PreCarriageLocation, nameof(containerPM.PreCarriageLocation), fieldChanges);
            container.PreCarriageLocation = containerPM.PreCarriageLocation;
            
            FieldChange.Add(container.PreCarriageATD, containerPM.PreCarriageATD, nameof(containerPM.PreCarriageATD), fieldChanges);
            container.PreCarriageATD = containerPM.PreCarriageATD;
            
            FieldChange.Add(container.PreCarriageETD, containerPM.PreCarriageETD, nameof(containerPM.PreCarriageETD), fieldChanges);
            container.PreCarriageETD = containerPM.PreCarriageETD;
            
            FieldChange.Add(container.LIFLocation, containerPM.LIFLocation, nameof(containerPM.LIFLocation), fieldChanges);
            container.LIFLocation = containerPM.LIFLocation;
            
            FieldChange.Add(container.EstimatedLIFArrival, containerPM.EstimatedLIFArrival, nameof(containerPM.EstimatedLIFArrival), fieldChanges);
            container.EstimatedLIFArrival = containerPM.EstimatedLIFArrival;
            
            FieldChange.Add(container.ActualLIFArrival, containerPM.ActualLIFArrival, nameof(containerPM.ActualLIFArrival), fieldChanges);
            container.ActualLIFArrival = containerPM.ActualLIFArrival;
            
            FieldChange.Add(container.EstimatedOnCarriageDeparture, containerPM.EstimatedOnCarriageDeparture, nameof(containerPM.EstimatedOnCarriageDeparture), fieldChanges);
            container.EstimatedOnCarriageDeparture = containerPM.EstimatedOnCarriageDeparture;
            
            FieldChange.Add(container.ActualOnCarriageDeparture, containerPM.ActualOnCarriageDeparture, nameof(containerPM.ActualOnCarriageDeparture), fieldChanges);
            container.ActualOnCarriageDeparture = containerPM.ActualOnCarriageDeparture;
            
            FieldChange.Add(container.GateIn, containerPM.GateIn, nameof(containerPM.GateIn), fieldChanges);
            container.GateIn = containerPM.GateIn;
            
            FieldChange.Add(container.GateOut, containerPM.GateOut, nameof(containerPM.GateOut), fieldChanges);
            container.GateOut = containerPM.GateOut;
            
            FieldChange.Add(container.EmptyReturnLocation, containerPM.EmptyReturnLocation, nameof(containerPM.EmptyReturnLocation), fieldChanges);
            container.EmptyReturnLocation = containerPM.EmptyReturnLocation;
            
            FieldChange.Add(container.EstimatedEmptyReturn, containerPM.EstimatedEmptyReturn, nameof(containerPM.EstimatedEmptyReturn), fieldChanges);
            container.EstimatedEmptyReturn = containerPM.EstimatedEmptyReturn;
            
            FieldChange.Add(container.ActualEmptyReturn, containerPM.ActualEmptyReturn, nameof(containerPM.ActualEmptyReturn), fieldChanges);
            container.ActualEmptyReturn = containerPM.ActualEmptyReturn;
            
            FieldChange.Add(container.CustomsReleaseState, containerPM.CustomsReleaseState, nameof(containerPM.CustomsReleaseState), fieldChanges);
            container.CustomsReleaseState = containerPM.CustomsReleaseState;
            
            FieldChange.Add(container.CarrierReleaseState, containerPM.CarrierReleaseState, nameof(containerPM.CarrierReleaseState), fieldChanges);
            container.CarrierReleaseState = containerPM.CarrierReleaseState;
            
            FieldChange.Add(container.AvailablityDate, containerPM.AvailablityDate, nameof(containerPM.AvailablityDate), fieldChanges);
            container.AvailablityDate = containerPM.AvailablityDate;
            
            FieldChange.Add(container.AvailabilityLocation, containerPM.AvailabilityLocation, nameof(containerPM.AvailabilityLocation), fieldChanges);
            container.AvailabilityLocation = containerPM.AvailabilityLocation;
            
            FieldChange.Add(container.FreeDays, containerPM.FreeDays, nameof(containerPM.FreeDays), fieldChanges);
            container.FreeDays = containerPM.FreeDays;
            
            FieldChange.Add(container.LastFreeDayDate, containerPM.LastFreeDayDate, nameof(containerPM.LastFreeDayDate), fieldChanges);
            container.LastFreeDayDate = containerPM.LastFreeDayDate;
            
            FieldChange.Add(container.EmptyPickupLocationPortId, containerPM.EmptyPickupLocationPortId, nameof(containerPM.EmptyPickupLocationPortId), fieldChanges);
            container.EmptyPickupLocationPortId = containerPM.EmptyPickupLocationPortId;
            
            FieldChange.Add(container.PreCarriageLocationPortId, containerPM.PreCarriageLocationPortId, nameof(containerPM.PreCarriageLocationPortId), fieldChanges);
            container.PreCarriageLocationPortId = containerPM.PreCarriageLocationPortId;
            
            FieldChange.Add(container.EmptyReturnLocationPortId, containerPM.EmptyReturnLocationPortId, nameof(containerPM.EmptyReturnLocationPortId), fieldChanges);
            container.EmptyReturnLocationPortId = containerPM.EmptyReturnLocationPortId;
            
            FieldChange.Add(container.AvailabilityLocationPortId, containerPM.AvailabilityLocationPortId, nameof(containerPM.AvailabilityLocationPortId), fieldChanges);
            container.AvailabilityLocationPortId = containerPM.AvailabilityLocationPortId;
            
            FieldChange.Add(container.OnCarriageLocationPortId, containerPM.OnCarriageLocationPortId, nameof(containerPM.OnCarriageLocationPortId), fieldChanges);
            container.OnCarriageLocationPortId = containerPM.OnCarriageLocationPortId;
            
            FieldChange.Add(container.LIFLocationPortId, containerPM.LIFLocationPortId, nameof(containerPM.LIFLocationPortId), fieldChanges);
            container.LIFLocationPortId = containerPM.LIFLocationPortId;
            
            FieldChange.Add(container.POLLocationPortId, containerPM.POLLocationPortId, nameof(containerPM.POLLocationPortId), fieldChanges);
            container.POLLocationPortId = containerPM.POLLocationPortId;
            
            FieldChange.Add(container.PODLocationPortId, containerPM.PODLocationPortId, nameof(containerPM.PODLocationPortId), fieldChanges);
            container.PODLocationPortId = containerPM.PODLocationPortId;
            
            FieldChange.Add(container.Transshipment1LocationPortId, containerPM.Transshipment1LocationPortId, nameof(containerPM.Transshipment1LocationPortId), fieldChanges);
            container.Transshipment1LocationPortId = containerPM.Transshipment1LocationPortId;
            
            FieldChange.Add(container.Transshipment2LocationPortId, containerPM.Transshipment2LocationPortId, nameof(containerPM.Transshipment2LocationPortId), fieldChanges);
            container.Transshipment2LocationPortId = containerPM.Transshipment2LocationPortId;
            
            FieldChange.Add(container.Transshipment3LocationPortId, containerPM.Transshipment3LocationPortId, nameof(containerPM.Transshipment3LocationPortId), fieldChanges);
            container.Transshipment3LocationPortId = containerPM.Transshipment3LocationPortId;
            
            FieldChange.Add(container.Transshipment4LocationPortId, containerPM.Transshipment4LocationPortId, nameof(containerPM.Transshipment4LocationPortId), fieldChanges);
            container.Transshipment4LocationPortId = containerPM.Transshipment4LocationPortId;
            
            FieldChange.Add(container.Field1, containerPM.Field1 != null ? containerPM.Field1.Value : null, nameof(containerPM.Field1), fieldChanges);
            container.Field1 = containerPM.Field1 != null ? containerPM.Field1.Value : null;

            FieldChange.Add(container.Field2, containerPM.Field2 != null ? containerPM.Field2.Value : null, nameof(containerPM.Field2), fieldChanges);
            container.Field2 = containerPM.Field2 != null ? containerPM.Field2.Value : null;

            FieldChange.Add(container.Field3, containerPM.Field3 != null ? containerPM.Field3.Value : null, nameof(containerPM.Field3), fieldChanges);
            container.Field3 = containerPM.Field3 != null ? containerPM.Field3.Value : null;

            FieldChange.Add(container.Field4, containerPM.Field4 != null ? containerPM.Field4.Value : null, nameof(containerPM.Field4), fieldChanges);
            container.Field4 = containerPM.Field4 != null ? containerPM.Field4.Value : null;

            FieldChange.Add(container.Field5, containerPM.Field5 != null ? containerPM.Field5.Value : null, nameof(containerPM.Field5), fieldChanges);
            container.Field5 = containerPM.Field5 != null ? containerPM.Field5.Value : null;

            FieldChange.Add(container.Field6, containerPM.Field6 != null ? containerPM.Field6.Value : null, nameof(containerPM.Field6), fieldChanges);
            container.Field6 = containerPM.Field6 != null ? containerPM.Field6.Value : null;

            FieldChange.Add(container.Field7, containerPM.Field7 != null ? containerPM.Field7.Value : null, nameof(containerPM.Field7), fieldChanges);
            container.Field7 = containerPM.Field7 != null ? containerPM.Field7.Value : null;

            FieldChange.Add(container.Field8, containerPM.Field8 != null ? containerPM.Field8.Value : null, nameof(containerPM.Field8), fieldChanges);
            container.Field8 = containerPM.Field8 != null ? containerPM.Field8.Value : null;

            FieldChange.Add(container.Field9, containerPM.Field9 != null ? containerPM.Field9.Value : null, nameof(containerPM.Field9), fieldChanges);
            container.Field9 = containerPM.Field9 != null ? containerPM.Field9.Value : null;

            FieldChange.Add(container.Field10, containerPM.Field10 != null ? containerPM.Field10.Value : null, nameof(containerPM.Field10), fieldChanges);
            container.Field10 = containerPM.Field10 != null ? containerPM.Field10.Value : null;
            
            FieldChange.Add(container.TerminalAddress, containerPM.TerminalAddress, nameof(containerPM.TerminalAddress), fieldChanges);
            container.TerminalAddress = containerPM.TerminalAddress;
            
            FieldChange.Add(container.TerminalPhone, containerPM.TerminalPhone, nameof(containerPM.TerminalPhone), fieldChanges);
            container.TerminalPhone = containerPM.TerminalPhone;
            
            FieldChange.Add(container.TerminalAddressId, containerPM.TerminalAddressId, nameof(containerPM.TerminalAddressId), fieldChanges);
            container.TerminalAddressId = containerPM.TerminalAddressId;
            
            FieldChange.Add(container.IsAutomaticUpdates, containerPM.IsAutomaticUpdates, nameof(containerPM.IsAutomaticUpdates), fieldChanges);
            container.IsAutomaticUpdates = containerPM.IsAutomaticUpdates;
            
            FieldChange.Add(container.IsClosed, containerPM.IsClosed, nameof(containerPM.IsClosed), fieldChanges);
            container.IsClosed = containerPM.IsClosed;
            
            FieldChange.Add(container.ClosedDate, containerPM.ClosedDate, nameof(containerPM.ClosedDate), fieldChanges);
            container.ClosedDate = containerPM.ClosedDate;
            
            FieldChange.Add(container.StatusId, containerPM.StatusId, nameof(containerPM.StatusId), fieldChanges);
            container.StatusId = containerPM.StatusId;
            
            FieldChange.Add(container.IsCancelled, containerPM.IsCancelled, nameof(containerPM.IsCancelled), fieldChanges);
            container.IsCancelled = containerPM.IsCancelled;
            
            FieldChange.Add(container.CancelledDate, containerPM.CancelledDate, nameof(containerPM.CancelledDate), fieldChanges);
            container.CancelledDate = containerPM.CancelledDate;
            
            FieldChange.Add(container.Leg1VesselId, containerPM.Leg1VesselId, nameof(containerPM.Leg1VesselId), fieldChanges);
            container.Leg1VesselId = containerPM.Leg1VesselId;
            
            FieldChange.Add(container.Leg2VesselId, containerPM.Leg2VesselId, nameof(containerPM.Leg2VesselId), fieldChanges);
            container.Leg2VesselId = containerPM.Leg2VesselId;
            
            FieldChange.Add(container.Leg3VesselId, containerPM.Leg3VesselId, nameof(containerPM.Leg3VesselId), fieldChanges);
            container.Leg3VesselId = containerPM.Leg3VesselId;
            
            FieldChange.Add(container.Leg4VesselId, containerPM.Leg4VesselId, nameof(containerPM.Leg4VesselId), fieldChanges);
            container.Leg4VesselId = containerPM.Leg4VesselId;
            
            FieldChange.Add(container.Leg5VesselId, containerPM.Leg5VesselId, nameof(containerPM.Leg5VesselId), fieldChanges);
            container.Leg5VesselId = containerPM.Leg5VesselId;
            
            FieldChange.Add(container.ExceptionDate, containerPM.ExceptionDate, nameof(containerPM.ExceptionDate), fieldChanges);
            container.ExceptionDate = containerPM.ExceptionDate;
            
            FieldChange.Add(container.ExceptionDescription, containerPM.ExceptionDescription, nameof(containerPM.ExceptionDescription), fieldChanges);
            container.ExceptionDescription = containerPM.ExceptionDescription;
            
            FieldChange.Add(container.ExceptionResolvedDescription, containerPM.ExceptionResolvedDescription, nameof(containerPM.ExceptionResolvedDescription), fieldChanges);
            container.ExceptionResolvedDescription = containerPM.ExceptionResolvedDescription;
            
            FieldChange.Add(container.HasException, containerPM.HasException, nameof(containerPM.HasException), fieldChanges);
            container.HasException = containerPM.HasException;
            
            FieldChange.Add(container.LastExceptionDescription, containerPM.LastExceptionDescription, nameof(containerPM.LastExceptionDescription), fieldChanges);
            container.LastExceptionDescription = containerPM.LastExceptionDescription;
            
            FieldChange.Add(container.IsExceptionResolved, containerPM.IsExceptionResolved, nameof(containerPM.IsExceptionResolved), fieldChanges);
            container.IsExceptionResolved = containerPM.IsExceptionResolved;
            
            FieldChange.Add(container.PreCarriageGateIn, containerPM.PreCarriageGateIn, nameof(containerPM.PreCarriageGateIn), fieldChanges);
            container.PreCarriageGateIn = containerPM.PreCarriageGateIn;
            
            FieldChange.Add(container.OnCarriageGateOut, containerPM.OnCarriageGateOut, nameof(containerPM.OnCarriageGateOut), fieldChanges);
            container.OnCarriageGateOut = containerPM.OnCarriageGateOut;

            var updatedByPartner = SetUpdatedByPartner(containerPM);
            FieldChange.Add(container.UpdatedByPartner, updatedByPartner, nameof(containerPM.UpdatedByPartner), fieldChanges);
            container.UpdatedByPartner = updatedByPartner;
            
            FieldChange.Add(container.ContainerTypeId, containerPM.ContainerTypeId, nameof(containerPM.ContainerTypeId), fieldChanges);
            container.ContainerTypeId = containerPM.ContainerTypeId;
            
            FieldChange.Add(container.Volume, containerPM.Volume, nameof(containerPM.Volume), fieldChanges);
            container.Volume = containerPM.Volume;
            
            FieldChange.Add(container.VolumeUnitCode, containerPM.VolumeUnitCode, nameof(containerPM.VolumeUnitCode), fieldChanges);
            container.VolumeUnitCode = containerPM.VolumeUnitCode;
            
            FieldChange.Add(container.GrossWeight, containerPM.GrossWeight, nameof(containerPM.GrossWeight), fieldChanges);
            container.GrossWeight = containerPM.GrossWeight;
            
            FieldChange.Add(container.GrossWeightUnitCode, containerPM.GrossWeightUnitCode, nameof(containerPM.GrossWeightUnitCode), fieldChanges);
            container.GrossWeightUnitCode = containerPM.GrossWeightUnitCode;
            
            FieldChange.Add(container.AdditionalReference1, containerPM.AdditionalReference1, nameof(containerPM.AdditionalReference1), fieldChanges);
            container.AdditionalReference1 = containerPM.AdditionalReference1;
            
            FieldChange.Add(container.AdditionalReference2, containerPM.AdditionalReference2, nameof(containerPM.AdditionalReference2), fieldChanges);
            container.AdditionalReference2 = containerPM.AdditionalReference2;
            
            FieldChange.Add(container.AdditionalReference3, containerPM.AdditionalReference3, nameof(containerPM.AdditionalReference3), fieldChanges);
            container.AdditionalReference3 = containerPM.AdditionalReference3;
            
            FieldChange.Add(container.AdditionalReference4, containerPM.AdditionalReference4, nameof(containerPM.AdditionalReference4), fieldChanges);
            container.AdditionalReference4 = containerPM.AdditionalReference4;
            
            FieldChange.Add(container.HasTransshipments, containerPM.HasTransshipments, nameof(containerPM.HasTransshipments), fieldChanges);
            container.HasTransshipments = containerPM.HasTransshipments;

            FieldChange.Add(container.Field11, containerPM.Field11 != null ? containerPM.Field11.Value : null, nameof(containerPM.Field11), fieldChanges);
            container.Field11 = containerPM.Field11 != null ? containerPM.Field11.Value : null;

            FieldChange.Add(container.Field12, containerPM.Field12 != null ? containerPM.Field12.Value : null, nameof(containerPM.Field12), fieldChanges);
            container.Field12 = containerPM.Field12 != null ? containerPM.Field12.Value : null;

            FieldChange.Add(container.Field13, containerPM.Field13 != null ? containerPM.Field13.Value : null, nameof(containerPM.Field13), fieldChanges);
            container.Field13 = containerPM.Field13 != null ? containerPM.Field13.Value : null;

            FieldChange.Add(container.Field14, containerPM.Field14 != null ? containerPM.Field14.Value : null, nameof(containerPM.Field14), fieldChanges);
            container.Field14 = containerPM.Field14 != null ? containerPM.Field14.Value : null;

            FieldChange.Add(container.Field15, containerPM.Field15 != null ? containerPM.Field15.Value : null, nameof(containerPM.Field15), fieldChanges);
            container.Field15 = containerPM.Field15 != null ? containerPM.Field15.Value : null;

            FieldChange.Add(container.Field16, containerPM.Field16 != null ? containerPM.Field16.Value : null, nameof(containerPM.Field16), fieldChanges);
            container.Field16 = containerPM.Field16 != null ? containerPM.Field16.Value : null;

            FieldChange.Add(container.Field17, containerPM.Field17 != null ? containerPM.Field17.Value : null, nameof(containerPM.Field17), fieldChanges);
            container.Field17 = containerPM.Field17 != null ? containerPM.Field17.Value : null;

            FieldChange.Add(container.Field18, containerPM.Field18 != null ? containerPM.Field18.Value : null, nameof(containerPM.Field18), fieldChanges);
            container.Field18 = containerPM.Field18 != null ? containerPM.Field18.Value : null;

            FieldChange.Add(container.Field19, containerPM.Field19 != null ? containerPM.Field19.Value : null, nameof(containerPM.Field19), fieldChanges);
            container.Field19 = containerPM.Field19 != null ? containerPM.Field19.Value : null;

            FieldChange.Add(container.Field20, containerPM.Field20 != null ? containerPM.Field20.Value : null, nameof(containerPM.Field20), fieldChanges);
            container.Field20 = containerPM.Field20 != null ? containerPM.Field20.Value : null;

            FieldChange.Add(container.Field21, containerPM.Field21 != null ? containerPM.Field21.Value : null, nameof(containerPM.Field21), fieldChanges);
            container.Field21 = containerPM.Field21 != null ? containerPM.Field21.Value : null;

            FieldChange.Add(container.Field22, containerPM.Field22 != null ? containerPM.Field22.Value : null, nameof(containerPM.Field22), fieldChanges);
            container.Field22 = containerPM.Field22 != null ? containerPM.Field22.Value : null;

            FieldChange.Add(container.Field23, containerPM.Field23 != null ? containerPM.Field23.Value : null, nameof(containerPM.Field23), fieldChanges);
            container.Field23 = containerPM.Field23 != null ? containerPM.Field23.Value : null;

            FieldChange.Add(container.Field24, containerPM.Field24 != null ? containerPM.Field24.Value : null, nameof(containerPM.Field24), fieldChanges);
            container.Field24 = containerPM.Field24 != null ? containerPM.Field24.Value : null;

            FieldChange.Add(container.Field25, containerPM.Field25 != null ? containerPM.Field25.Value : null, nameof(containerPM.Field25), fieldChanges);
            container.Field25 = containerPM.Field25 != null ? containerPM.Field25.Value : null;

            FieldChange.Add(container.Field26, containerPM.Field26 != null ? containerPM.Field26.Value : null, nameof(containerPM.Field26), fieldChanges);
            container.Field26 = containerPM.Field26 != null ? containerPM.Field26.Value : null;

            FieldChange.Add(container.Field27, containerPM.Field27 != null ? containerPM.Field27.Value : null, nameof(containerPM.Field27), fieldChanges);
            container.Field27 = containerPM.Field27 != null ? containerPM.Field27.Value : null;

            FieldChange.Add(container.Field28, containerPM.Field28 != null ? containerPM.Field28.Value : null, nameof(containerPM.Field28), fieldChanges);
            container.Field28 = containerPM.Field28 != null ? containerPM.Field28.Value : null;

            FieldChange.Add(container.Field29, containerPM.Field29 != null ? containerPM.Field29.Value : null, nameof(containerPM.Field29), fieldChanges);
            container.Field29 = containerPM.Field29 != null ? containerPM.Field29.Value : null;

            FieldChange.Add(container.Field30, containerPM.Field30 != null ? containerPM.Field30.Value : null, nameof(containerPM.Field30), fieldChanges);
            container.Field30 = containerPM.Field30 != null ? containerPM.Field30.Value : null;

            FieldChange.Add(container.Field31, containerPM.Field31 != null ? containerPM.Field31.Value : null, nameof(containerPM.Field31), fieldChanges);
            container.Field31 = containerPM.Field31 != null ? containerPM.Field31.Value : null;

            FieldChange.Add(container.Field32, containerPM.Field32 != null ? containerPM.Field32.Value : null, nameof(containerPM.Field32), fieldChanges);
            container.Field32 = containerPM.Field32 != null ? containerPM.Field32.Value : null;

            FieldChange.Add(container.Field33, containerPM.Field33 != null ? containerPM.Field33.Value : null, nameof(containerPM.Field33), fieldChanges);
            container.Field33 = containerPM.Field33 != null ? containerPM.Field33.Value : null;

            FieldChange.Add(container.Field34, containerPM.Field34 != null ? containerPM.Field34.Value : null, nameof(containerPM.Field34), fieldChanges);
            container.Field34 = containerPM.Field34 != null ? containerPM.Field34.Value : null;

            FieldChange.Add(container.Field35, containerPM.Field35 != null ? containerPM.Field35.Value : null, nameof(containerPM.Field35), fieldChanges);
            container.Field35 = containerPM.Field35 != null ? containerPM.Field35.Value : null;

            FieldChange.Add(container.Field36, containerPM.Field36 != null ? containerPM.Field36.Value : null, nameof(containerPM.Field36), fieldChanges);
            container.Field36 = containerPM.Field36 != null ? containerPM.Field36.Value : null;

            FieldChange.Add(container.Field37, containerPM.Field37 != null ? containerPM.Field37.Value : null, nameof(containerPM.Field37), fieldChanges);
            container.Field37 = containerPM.Field37 != null ? containerPM.Field37.Value : null;

            FieldChange.Add(container.Field38, containerPM.Field38 != null ? containerPM.Field38.Value : null, nameof(containerPM.Field38), fieldChanges);
            container.Field38 = containerPM.Field38 != null ? containerPM.Field38.Value : null;

            FieldChange.Add(container.Field39, containerPM.Field39 != null ? containerPM.Field39.Value : null, nameof(containerPM.Field39), fieldChanges);
            container.Field39 = containerPM.Field39 != null ? containerPM.Field39.Value : null;

            FieldChange.Add(container.Field40, containerPM.Field40 != null ? containerPM.Field40.Value : null, nameof(containerPM.Field40), fieldChanges);
            container.Field40 = containerPM.Field40 != null ? containerPM.Field40.Value : null;

            FieldChange.Add(container.OnCarriageETA, containerPM.OnCarriageETA, nameof(containerPM.OnCarriageETA), fieldChanges);
            container.OnCarriageETA = containerPM.OnCarriageETA;

            FieldChange.Add(container.OnCarriageATA, containerPM.OnCarriageATA, nameof(containerPM.OnCarriageATA), fieldChanges);
            container.OnCarriageATA = containerPM.OnCarriageATA;

            FieldChange.Add(container.RequestDate, containerPM.RequestDate, nameof(containerPM.RequestDate), fieldChanges);
            container.RequestDate = containerPM.RequestDate;

            FieldChange.Add(container.RecentResponseDate, containerPM.RecentResponseDate, nameof(containerPM.RecentResponseDate), fieldChanges);
            container.RecentResponseDate = containerPM.RecentResponseDate;
        }

        private static string SetUpdatedByPartner(ContainerPM entityPM)
        {
            string updatedByPartner = null;
            if (entityPM.IsUpdatedOceanInsightsAnalyzer)
            {
                updatedByPartner = "Ocean Insights";
            }else if (entityPM.IsUpdatedVizionAnalyzer)
            {
                updatedByPartner = "Container Tracker";
            }
            else
            {
                updatedByPartner = GetUserName(entityPM.UpdatedByUserId, entityPM.Tenant);
            }
            return updatedByPartner;
        }

        private static string GetUserName(string UpdatedByUserId, int tenant)
        {
            string userName = null;
            ICommonDataContext commonContext = CommonDataContext.GetContext(tenant);
            ContactRepository contactRep = new ContactRepository(commonContext);
            string email = "system@tenant" + tenant + ".com";

            if (AuthenticationUtil.IsAuthenticatedUserExists())
            {
                email = AuthenticationUtil.GetAuthenticatedUser();
            }

            Contact contact = contactRep.GetSingleContactByEmail(email, tenant);
            if (contact != null)
            {
                userName = contact.EnglishName;
            }

            return userName;
        }

        public static void MapContainerShipmentFields(ContainerPM containerPM, Container container, List<FieldChange> fieldChanges)
        {
            if (!containerPM.IsShipmentBatchUpdate && !isNew && containerPM.ShipmentStatusId == container.ShipmentStatusId) return;            

            FieldChange.Add(container.ShipmentId, containerPM.ShipmentId, nameof(containerPM.ShipmentId), fieldChanges);
            container.ShipmentId = containerPM.ShipmentId;
            
            FieldChange.Add(container.CustomsReleaseDate, containerPM.CustomsReleaseDate, nameof(containerPM.CustomsReleaseDate), fieldChanges);
            container.CustomsReleaseDate = containerPM.CustomsReleaseDate;
            
            FieldChange.Add(container.CarrierReleaseDate, containerPM.CarrierReleaseDate, nameof(containerPM.CarrierReleaseDate), fieldChanges);
            container.CarrierReleaseDate = containerPM.CarrierReleaseDate;
            
            FieldChange.Add(container.Master, containerPM.Master, nameof(containerPM.Master), fieldChanges);
            container.Master = containerPM.Master;
            
            FieldChange.Add(container.VesselName, containerPM.VesselName, nameof(containerPM.VesselName), fieldChanges);
            container.VesselName = containerPM.VesselName;
            
            FieldChange.Add(container.MainCarriageCarrierId, containerPM.MainCarriageCarrierId, nameof(containerPM.MainCarriageCarrierId), fieldChanges);
            container.MainCarriageCarrierId = containerPM.MainCarriageCarrierId;
            
            FieldChange.Add(container.MainCarriageCarrierNumber, containerPM.MainCarriageCarrierNumber, nameof(containerPM.MainCarriageCarrierNumber), fieldChanges);
            container.MainCarriageCarrierNumber = containerPM.MainCarriageCarrierNumber;
            
            FieldChange.Add(container.MainCarriageATA, containerPM.MainCarriageATA, nameof(containerPM.MainCarriageATA), fieldChanges);
            container.MainCarriageATA = containerPM.MainCarriageATA;
            
            FieldChange.Add(container.MainCarriageATD, containerPM.MainCarriageATD, nameof(containerPM.MainCarriageATD), fieldChanges);
            container.MainCarriageATD = containerPM.MainCarriageATD;
            
            FieldChange.Add(container.MainCarriageETA, containerPM.MainCarriageETA, nameof(containerPM.MainCarriageETA), fieldChanges);
            container.MainCarriageETA = containerPM.MainCarriageETA;
            
            FieldChange.Add(container.MainCarriageETD, containerPM.MainCarriageETD, nameof(containerPM.MainCarriageETD), fieldChanges);
            container.MainCarriageETD = containerPM.MainCarriageETD;
            
            FieldChange.Add(container.MainCarriageVesselId, containerPM.MainCarriageVesselId, nameof(containerPM.MainCarriageVesselId), fieldChanges);
            container.MainCarriageVesselId = containerPM.MainCarriageVesselId;

            FieldChange.Add(container.ShipmentLastLegETA, containerPM.ShipmentLastLegETA, nameof(containerPM.ShipmentLastLegETA), fieldChanges);
            container.ShipmentLastLegETA = containerPM.ShipmentLastLegETA;

            FieldChange.Add(container.ShipmentLastLegATA, containerPM.ShipmentLastLegATA, nameof(containerPM.ShipmentLastLegATA), fieldChanges);
            container.ShipmentLastLegATA = containerPM.ShipmentLastLegATA;

            FieldChange.Add(container.ShipmentPackagesId, containerPM.ShipmentPackagesId, nameof(containerPM.ShipmentPackagesId), fieldChanges);
            container.ShipmentPackagesId = containerPM.ShipmentPackagesId;
            
            FieldChange.Add(container.ShipmentStatusId, containerPM.ShipmentStatusId, nameof(containerPM.ShipmentStatusId), fieldChanges);
            container.ShipmentStatusId = containerPM.ShipmentStatusId;
            
            FieldChange.Add(container.ShipmentPickupETA, containerPM.ShipmentPickupETA, nameof(containerPM.ShipmentPickupETA), fieldChanges);
            container.ShipmentPickupETA = containerPM.ShipmentPickupETA;
            
            FieldChange.Add(container.ShipmentPickupETD, containerPM.ShipmentPickupETD, nameof(containerPM.ShipmentPickupETD), fieldChanges);
            container.ShipmentPickupETD = containerPM.ShipmentPickupETD;
            
            FieldChange.Add(container.ShipmentPickupATA, containerPM.ShipmentPickupATA, nameof(containerPM.ShipmentPickupATA), fieldChanges);
            container.ShipmentPickupATA = containerPM.ShipmentPickupATA;
            
            FieldChange.Add(container.ShipmentPickupATD, containerPM.ShipmentPickupATD, nameof(containerPM.ShipmentPickupATD), fieldChanges);
            container.ShipmentPickupATD = containerPM.ShipmentPickupATD;
            
            FieldChange.Add(container.ShipmentPreCarriageETA, containerPM.ShipmentPreCarriageETA, nameof(containerPM.ShipmentPreCarriageETA), fieldChanges);
            container.ShipmentPreCarriageETA = containerPM.ShipmentPreCarriageETA;
            
            FieldChange.Add(container.ShipmentPreCarriageETD, containerPM.ShipmentPreCarriageETD, nameof(containerPM.ShipmentPreCarriageETD), fieldChanges);
            container.ShipmentPreCarriageETD = containerPM.ShipmentPreCarriageETD;
            
            FieldChange.Add(container.ShipmentPreCarriageATA, containerPM.ShipmentPreCarriageATA, nameof(containerPM.ShipmentPreCarriageATA), fieldChanges);
            container.ShipmentPreCarriageATA = containerPM.ShipmentPreCarriageATA;
            
            FieldChange.Add(container.ShipmentPreCarriageATD, containerPM.ShipmentPreCarriageATD, nameof(containerPM.ShipmentPreCarriageATD), fieldChanges);
            container.ShipmentPreCarriageATD = containerPM.ShipmentPreCarriageATD;
            
            FieldChange.Add(container.ShipmentMainCarriageETA, containerPM.ShipmentMainCarriageETA, nameof(containerPM.ShipmentMainCarriageETA), fieldChanges);
            container.ShipmentMainCarriageETA = containerPM.ShipmentMainCarriageETA;
            
            FieldChange.Add(container.ShipmentMainCarriageETD, containerPM.ShipmentMainCarriageETD, nameof(containerPM.ShipmentMainCarriageETD), fieldChanges);
            container.ShipmentMainCarriageETD = containerPM.ShipmentMainCarriageETD;
            
            FieldChange.Add(container.ShipmentMainCarriageATA, containerPM.ShipmentMainCarriageATA, nameof(containerPM.ShipmentMainCarriageATA), fieldChanges);
            container.ShipmentMainCarriageATA = containerPM.ShipmentMainCarriageATA;
            
            FieldChange.Add(container.ShipmentMainCarriageATD, containerPM.ShipmentMainCarriageATD, nameof(containerPM.ShipmentMainCarriageATD), fieldChanges);
            container.ShipmentMainCarriageATD = containerPM.ShipmentMainCarriageATD;
            
            FieldChange.Add(container.ShipmentTransshipment1ETA, containerPM.ShipmentTransshipment1ETA, nameof(containerPM.ShipmentTransshipment1ETA), fieldChanges);
            container.ShipmentTransshipment1ETA = containerPM.ShipmentTransshipment1ETA;
            
            FieldChange.Add(container.ShipmentTransshipment1ETD, containerPM.ShipmentTransshipment1ETD, nameof(containerPM.ShipmentTransshipment1ETD), fieldChanges);
            container.ShipmentTransshipment1ETD = containerPM.ShipmentTransshipment1ETD;
            
            FieldChange.Add(container.ShipmentTransshipment1ATA, containerPM.ShipmentTransshipment1ATA, nameof(containerPM.ShipmentTransshipment1ATA), fieldChanges);
            container.ShipmentTransshipment1ATA = containerPM.ShipmentTransshipment1ATA;
            
            FieldChange.Add(container.ShipmentTransshipment1ATD, containerPM.ShipmentTransshipment1ATD, nameof(containerPM.ShipmentTransshipment1ATD), fieldChanges);
            container.ShipmentTransshipment1ATD = containerPM.ShipmentTransshipment1ATD;
            
            FieldChange.Add(container.ShipmentTransshipment2ETA, containerPM.ShipmentTransshipment2ETA, nameof(containerPM.ShipmentTransshipment2ETA), fieldChanges);
            container.ShipmentTransshipment2ETA = containerPM.ShipmentTransshipment2ETA;
            
            FieldChange.Add(container.ShipmentTransshipment2ETD, containerPM.ShipmentTransshipment2ETD, nameof(containerPM.ShipmentTransshipment2ETD), fieldChanges);
            container.ShipmentTransshipment2ETD = containerPM.ShipmentTransshipment2ETD;
            
            FieldChange.Add(container.ShipmentTransshipment2ATA, containerPM.ShipmentTransshipment2ATA, nameof(containerPM.ShipmentTransshipment2ATA), fieldChanges);
            container.ShipmentTransshipment2ATA = containerPM.ShipmentTransshipment2ATA;
            
            FieldChange.Add(container.ShipmentTransshipment2ATD, containerPM.ShipmentTransshipment2ATD, nameof(containerPM.ShipmentTransshipment2ATD), fieldChanges);
            container.ShipmentTransshipment2ATD = containerPM.ShipmentTransshipment2ATD;
            
            FieldChange.Add(container.ShipmentTransshipment3ETA, containerPM.ShipmentTransshipment3ETA, nameof(containerPM.ShipmentTransshipment3ETA), fieldChanges);
            container.ShipmentTransshipment3ETA = containerPM.ShipmentTransshipment3ETA;
            
            FieldChange.Add(container.ShipmentTransshipment3ETD, containerPM.ShipmentTransshipment3ETD, nameof(containerPM.ShipmentTransshipment3ETD), fieldChanges);
            container.ShipmentTransshipment3ETD = containerPM.ShipmentTransshipment3ETD;
            
            FieldChange.Add(container.ShipmentTransshipment3ATA, containerPM.ShipmentTransshipment3ATA, nameof(containerPM.ShipmentTransshipment3ATA), fieldChanges);
            container.ShipmentTransshipment3ATA = containerPM.ShipmentTransshipment3ATA;
            
            FieldChange.Add(container.ShipmentTransshipment3ATD, containerPM.ShipmentTransshipment3ATD, nameof(containerPM.ShipmentTransshipment3ATD), fieldChanges);
            container.ShipmentTransshipment3ATD = containerPM.ShipmentTransshipment3ATD;
            
            FieldChange.Add(container.ShipmentOnCarriageETA, containerPM.ShipmentOnCarriageETA, nameof(containerPM.ShipmentOnCarriageETA), fieldChanges);
            container.ShipmentOnCarriageETA = containerPM.ShipmentOnCarriageETA;
            
            FieldChange.Add(container.ShipmentOnCarriageETD, containerPM.ShipmentOnCarriageETD, nameof(containerPM.ShipmentOnCarriageETD), fieldChanges);
            container.ShipmentOnCarriageETD = containerPM.ShipmentOnCarriageETD;
            
            FieldChange.Add(container.ShipmentOnCarriageATA, containerPM.ShipmentOnCarriageATA, nameof(containerPM.ShipmentOnCarriageATA), fieldChanges);
            container.ShipmentOnCarriageATA = containerPM.ShipmentOnCarriageATA;
            
            FieldChange.Add(container.ShipmentOnCarriageATD, containerPM.ShipmentOnCarriageATD, nameof(containerPM.ShipmentOnCarriageATD), fieldChanges);
            container.ShipmentOnCarriageATD = containerPM.ShipmentOnCarriageATD;
            
            FieldChange.Add(container.ShipmentDeliveryETA, containerPM.ShipmentDeliveryETA, nameof(containerPM.ShipmentDeliveryETA), fieldChanges);
            container.ShipmentDeliveryETA = containerPM.ShipmentDeliveryETA;
            
            FieldChange.Add(container.ShipmentDeliveryETD, containerPM.ShipmentDeliveryETD, nameof(containerPM.ShipmentDeliveryETD), fieldChanges);
            container.ShipmentDeliveryETD = containerPM.ShipmentDeliveryETD;
            
            FieldChange.Add(container.ShipmentDeliveryATA, containerPM.ShipmentDeliveryATA, nameof(containerPM.ShipmentDeliveryATA), fieldChanges);
            container.ShipmentDeliveryATA = containerPM.ShipmentDeliveryATA;
            
            FieldChange.Add(container.ShipmentDeliveryATD, containerPM.ShipmentDeliveryATD, nameof(containerPM.ShipmentDeliveryATD), fieldChanges);
            container.ShipmentDeliveryATD = containerPM.ShipmentDeliveryATD;
            
            FieldChange.Add(container.ShipmentOriginAgentId, containerPM.ShipmentOriginAgentId, nameof(containerPM.ShipmentOriginAgentId), fieldChanges);
            container.ShipmentOriginAgentId = containerPM.ShipmentOriginAgentId;
            
            FieldChange.Add(container.ShipmentDestinationAgentId, containerPM.ShipmentDestinationAgentId, nameof(containerPM.ShipmentDestinationAgentId), fieldChanges);
            container.ShipmentDestinationAgentId = containerPM.ShipmentDestinationAgentId;
            
            FieldChange.Add(container.ShipmentNumber, containerPM.ShipmentNumber, nameof(containerPM.ShipmentNumber), fieldChanges);
            container.ShipmentNumber = containerPM.ShipmentNumber;
            
            FieldChange.Add(container.ShipmentTypeId, containerPM.ShipmentTypeId, nameof(containerPM.ShipmentTypeId), fieldChanges);
            container.ShipmentTypeId = containerPM.ShipmentTypeId;
            
            FieldChange.Add(container.ShipmentCreateDate, containerPM.ShipmentCreateDate, nameof(containerPM.ShipmentCreateDate), fieldChanges);
            container.ShipmentCreateDate = containerPM.ShipmentCreateDate;
            
            FieldChange.Add(container.ShipmentDeliveryTruckerId, containerPM.ShipmentDeliveryTruckerId, nameof(containerPM.ShipmentDeliveryTruckerId), fieldChanges);
            container.ShipmentDeliveryTruckerId = containerPM.ShipmentDeliveryTruckerId;
            
            FieldChange.Add(container.ShipmentPickupFrom, containerPM.ShipmentPickupFrom, nameof(containerPM.ShipmentPickupFrom), fieldChanges);
            container.ShipmentPickupFrom = containerPM.ShipmentPickupFrom;
            
            FieldChange.Add(container.ShipmentPickupTo, containerPM.ShipmentPickupTo, nameof(containerPM.ShipmentPickupTo), fieldChanges);
            container.ShipmentPickupTo = containerPM.ShipmentPickupTo;
            
            FieldChange.Add(container.ShipmentPreCarriageFromId, containerPM.ShipmentPreCarriageFromId, nameof(containerPM.ShipmentPreCarriageFromId), fieldChanges);
            container.ShipmentPreCarriageFromId = containerPM.ShipmentPreCarriageFromId;
            
            FieldChange.Add(container.ShipmentPreCarriageToId, containerPM.ShipmentPreCarriageToId, nameof(containerPM.ShipmentPreCarriageToId), fieldChanges);
            container.ShipmentPreCarriageToId = containerPM.ShipmentPreCarriageToId;
            
            FieldChange.Add(container.ShipmentMainCarriageFromId, containerPM.ShipmentMainCarriageFromId, nameof(containerPM.ShipmentMainCarriageFromId), fieldChanges);
            container.ShipmentMainCarriageFromId = containerPM.ShipmentMainCarriageFromId;
            
            FieldChange.Add(container.ShipmentMainCarriageToId, containerPM.ShipmentMainCarriageToId, nameof(containerPM.ShipmentMainCarriageToId), fieldChanges);
            container.ShipmentMainCarriageToId = containerPM.ShipmentMainCarriageToId;
            
            FieldChange.Add(container.ShipmentTransshipment1FromId, containerPM.ShipmentTransshipment1FromId, nameof(containerPM.ShipmentTransshipment1FromId), fieldChanges);
            container.ShipmentTransshipment1FromId = containerPM.ShipmentTransshipment1FromId;
            
            FieldChange.Add(container.ShipmentTransshipment1ToId, containerPM.ShipmentTransshipment1ToId, nameof(containerPM.ShipmentTransshipment1ToId), fieldChanges);
            container.ShipmentTransshipment1ToId = containerPM.ShipmentTransshipment1ToId;
            
            FieldChange.Add(container.ShipmentTransshipment2FromId, containerPM.ShipmentTransshipment2FromId, nameof(containerPM.ShipmentTransshipment2FromId), fieldChanges);
            container.ShipmentTransshipment2FromId = containerPM.ShipmentTransshipment2FromId;
            
            FieldChange.Add(container.ShipmentTransshipment2ToId, containerPM.ShipmentTransshipment2ToId, nameof(containerPM.ShipmentTransshipment2ToId), fieldChanges);
            container.ShipmentTransshipment2ToId = containerPM.ShipmentTransshipment2ToId;
            
            FieldChange.Add(container.ShipmentTransshipment3FromId, containerPM.ShipmentTransshipment3FromId, nameof(containerPM.ShipmentTransshipment3FromId), fieldChanges);
            container.ShipmentTransshipment3FromId = containerPM.ShipmentTransshipment3FromId;
            
            FieldChange.Add(container.ShipmentTransshipment3ToId, containerPM.ShipmentTransshipment3ToId, nameof(containerPM.ShipmentTransshipment3ToId), fieldChanges);
            container.ShipmentTransshipment3ToId = containerPM.ShipmentTransshipment3ToId;
            
            FieldChange.Add(container.ShipmentOnCarriageFromId, containerPM.ShipmentOnCarriageFromId, nameof(containerPM.ShipmentOnCarriageFromId), fieldChanges);
            container.ShipmentOnCarriageFromId = containerPM.ShipmentOnCarriageFromId;
            
            FieldChange.Add(container.ShipmentOnCarriageToId, containerPM.ShipmentOnCarriageToId, nameof(containerPM.ShipmentOnCarriageToId), fieldChanges);
            container.ShipmentOnCarriageToId = containerPM.ShipmentOnCarriageToId;
            
            FieldChange.Add(container.ShipmentDeliveryFrom, containerPM.ShipmentDeliveryFrom, nameof(containerPM.ShipmentDeliveryFrom), fieldChanges);
            container.ShipmentDeliveryFrom = containerPM.ShipmentDeliveryFrom;
            
            FieldChange.Add(container.ShipmentDeliveryTo, containerPM.ShipmentDeliveryTo, nameof(containerPM.ShipmentDeliveryTo), fieldChanges);
            container.ShipmentDeliveryTo = containerPM.ShipmentDeliveryTo;
            
            FieldChange.Add(container.ContainersCount, containerPM.ContainersCount, nameof(containerPM.ContainersCount), fieldChanges);
            container.ContainersCount = containerPM.ContainersCount;
            
            FieldChange.Add(container.HandlerId, containerPM.HandlerId, nameof(containerPM.HandlerId), fieldChanges);
            container.HandlerId = containerPM.HandlerId;
            
            FieldChange.Add(container.CustomerId, containerPM.CustomerId, nameof(containerPM.CustomerId), fieldChanges);
            container.CustomerId = containerPM.CustomerId;
            
            FieldChange.Add(container.OPClosed, containerPM.OPClosed, nameof(containerPM.OPClosed), fieldChanges);
            container.OPClosed = containerPM.OPClosed;
            
            FieldChange.Add(container.TerminalId, containerPM.TerminalId, nameof(containerPM.TerminalId), fieldChanges);
            container.TerminalId = containerPM.TerminalId;
            
            FieldChange.Add(container.PODReceivedOnDate, containerPM.PODReceivedOnDate, nameof(containerPM.PODReceivedOnDate), fieldChanges);
            container.PODReceivedOnDate = containerPM.PODReceivedOnDate;
            
            FieldChange.Add(container.EmptyContainerReturnETA, containerPM.EmptyContainerReturnETA, nameof(containerPM.EmptyContainerReturnETA), fieldChanges);
            container.EmptyContainerReturnETA = containerPM.EmptyContainerReturnETA;
            
            FieldChange.Add(container.EmptyContainerReturnATA, containerPM.EmptyContainerReturnATA, nameof(containerPM.EmptyContainerReturnATA), fieldChanges);
            container.EmptyContainerReturnATA = containerPM.EmptyContainerReturnATA;
            
            FieldChange.Add(container.EmptyContainerReturnETD, containerPM.EmptyContainerReturnETD, nameof(containerPM.EmptyContainerReturnETD), fieldChanges);
            container.EmptyContainerReturnETD = containerPM.EmptyContainerReturnETD;
            
            FieldChange.Add(container.EmptyContainerReturnATD, containerPM.EmptyContainerReturnATD, nameof(containerPM.EmptyContainerReturnATD), fieldChanges);
            container.EmptyContainerReturnATD = containerPM.EmptyContainerReturnATD;
            
            FieldChange.Add(container.EmptyContainerReturnFrom, containerPM.EmptyContainerReturnFrom, nameof(containerPM.EmptyContainerReturnFrom), fieldChanges);
            container.EmptyContainerReturnFrom = containerPM.EmptyContainerReturnFrom;
            
            FieldChange.Add(container.EmptyContainerReturnTo, containerPM.EmptyContainerReturnTo, nameof(containerPM.EmptyContainerReturnTo), fieldChanges);
            container.EmptyContainerReturnTo = containerPM.EmptyContainerReturnTo;

            FieldChange.Add(container.ShipmentDepartmentId, containerPM.ShipmentDepartmentId, nameof(containerPM.ShipmentDepartmentId), fieldChanges);
            container.ShipmentDepartmentId = containerPM.ShipmentDepartmentId;
        }

        public static void BuildSearchField(ContainerPM containerPM, Container container, List<FieldChange> fieldChanges)
        {
            string mySearchFields = "";

            MethodHelper.AddToSearchFields(ref mySearchFields, containerPM.ContainerNumber);
            MethodHelper.AddToSearchFields(ref mySearchFields, containerPM.Master);
            MethodHelper.AddToSearchFields(ref mySearchFields, containerPM.MainCarriageCarrierNumber);
            MethodHelper.AddToSearchFields(ref mySearchFields, containerPM.CarrierName);
            MethodHelper.AddToSearchFields(ref mySearchFields, containerPM.ShipmentNumber);

            containerPM.SearchFields = mySearchFields;
            
            FieldChange.Add(container.SearchFields, containerPM.SearchFields, nameof(containerPM.SearchFields), fieldChanges);
            container.SearchFields = mySearchFields;
        }
    }
}
