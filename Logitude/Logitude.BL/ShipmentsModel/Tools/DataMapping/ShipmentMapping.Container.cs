using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.ShipmentsModel.Tools.DataMapping
{
    public partial class ShipmentMapping
    {
        public static void MapContainer(ContainerPM containerPM, Container container, bool isNewEntity)
        {
            if (isNewEntity)
            {
                container.Id = containerPM.Id;
                container.Tenant = containerPM.Tenant;
                container.CreateDate = containerPM.CreateDate;
                container.CreatedByUserId = containerPM.CreatedByUserId;
            }

            container.UpdateDate = containerPM.UpdateDate;
            container.UpdatedByUserId = containerPM.UpdatedByUserId;
            container.MainCarriageCarrierId = containerPM.MainCarriageCarrierId;
            container.MainCarriageCarrierNumber = containerPM.MainCarriageCarrierNumber;
            container.MainCarriageATA = containerPM.MainCarriageATA;
            container.MainCarriageATD = containerPM.MainCarriageATD;
            container.MainCarriageETA = containerPM.MainCarriageETA;
            container.MainCarriageETD = containerPM.MainCarriageETD;
            container.MainCarriageVesselId = containerPM.MainCarriageVesselId;
            container.DischargeDate = containerPM.DischargeDate;
            container.Master = containerPM.Master;
            container.ShipmentPackagesId = containerPM.ShipmentPackagesId;
            container.ContainerNumber = containerPM.ContainerNumber;
            container.ShipmentId = containerPM.ShipmentId;
            container.ActualEmptyPickupDate = containerPM.ActualEmptyPickupDate;
            container.EstimatedEmptyPickupDate = containerPM.EstimatedEmptyPickupDate;
            container.CurrentStatus = containerPM.CurrentStatus;
            container.CurrentStatusDate = containerPM.CurrentStatusDate;
            container.HasContainerException = containerPM.HasContainerException;
            container.CurrentLocation = containerPM.CurrentLocation;
            container.EmptyPickupLocation = containerPM.EmptyPickupLocation;
            container.DepartureLocation = containerPM.DepartureLocation;
            container.DestinationLocation = containerPM.DestinationLocation;
            container.ShipmentFirstPickupFrom = containerPM.ShipmentFirstPickupFrom;
            container.ShipmentFirstPickupTo = containerPM.ShipmentFirstPickupTo;
            container.ShipmentPreCarriageFromId = containerPM.ShipmentPreCarriageFromId;
            container.ShipmentPreCarriageToId = containerPM.ShipmentPreCarriageToId;
            container.ShipmentMainCarriageFromId = containerPM.ShipmentMainCarriageFromId;
            container.ShipmentMainCarriageToId = containerPM.ShipmentMainCarriageToId;
            container.ShipmentTransshipment1FromId = containerPM.ShipmentTransshipment1FromId;
            container.ShipmentTransshipment1ToId = containerPM.ShipmentTransshipment1ToId;
            container.ShipmentTransshipment2FromId = containerPM.ShipmentTransshipment2FromId;
            container.ShipmentTransshipment2ToId = containerPM.ShipmentTransshipment2ToId;
            container.ShipmentTransshipment3FromId = containerPM.ShipmentTransshipment3FromId;
            container.ShipmentTransshipment3ToId = containerPM.ShipmentTransshipment3ToId;
            container.ShipmentOnCarriageFromId = containerPM.ShipmentOnCarriageFromId;
            container.ShipmentOnCarriageToId = containerPM.ShipmentOnCarriageToId;
            container.ShipmentLastDeliveryFrom = containerPM.ShipmentLastDeliveryFrom;
            container.ShipmentLastDeliveryTo = containerPM.ShipmentLastDeliveryTo;
            container.OriginLocation = containerPM.OriginLocation;
            container.EstimatedOriginPickup = containerPM.EstimatedOriginPickup;
            container.ActualOriginPickup = containerPM.ActualOriginPickup;
            container.POLLocation = containerPM.POLLocation;
            container.EstimatedPOLArrival = containerPM.EstimatedPOLArrival;
            container.ActualPOLArrival = containerPM.ActualPOLArrival;
            container.EstimatedPOLLoaded = containerPM.EstimatedPOLLoaded;
            container.ActualPOLLoaded = containerPM.ActualPOLLoaded;
            container.EstimatedPOLVesselDeparture = containerPM.EstimatedPOLVesselDeparture;
            container.ActualPOLVesselDeparture = containerPM.ActualPOLVesselDeparture;
            container.TransshipmentCount = containerPM.TransshipmentCount;
            container.Transshipment1Location = containerPM.Transshipment1Location;
            container.EstimatedTrans1VesselArrival = containerPM.EstimatedTrans1VesselArrival;
            container.ActualTransshipment1VesselArrival = containerPM.ActualTransshipment1VesselArrival;
            container.EstimatedTransshipment1Discharge = containerPM.EstimatedTransshipment1Discharge;
            container.ActualTransshipment1Discharge = containerPM.ActualTransshipment1Discharge;
            container.EstimatedTransshipment1Loaded = containerPM.EstimatedTransshipment1Loaded;
            container.ActualTransshipment1Loaded = containerPM.ActualTransshipment1Loaded;
            container.EstimatedTrans1VesselDeparture = containerPM.EstimatedTrans1VesselDeparture;
            container.ActualTrans1VesselDeparture = containerPM.ActualTrans1VesselDeparture;
            container.Transshipment2Location = containerPM.Transshipment2Location;
            container.EstimatedTrans2VesselArrival = containerPM.EstimatedTrans2VesselArrival;
            container.ActualTransshipment2VesselArrival = containerPM.ActualTransshipment2VesselArrival;
            container.EstimatedTransshipment2Discharge = containerPM.EstimatedTransshipment2Discharge;
            container.ActualTransshipment2Discharge = containerPM.ActualTransshipment2Discharge;
            container.EstimatedTransshipment2Loaded = containerPM.EstimatedTransshipment2Loaded;
            container.ActualTransshipment2Loaded = containerPM.ActualTransshipment2Loaded;
            container.EstimatedTrans2VesselDeparture = containerPM.EstimatedTrans2VesselDeparture;
            container.ActualTrans2VesselDeparture = containerPM.ActualTrans2VesselDeparture;
            container.Transshipment3Location = containerPM.Transshipment3Location;
            container.EstimatedTrans3VesselArrival = containerPM.EstimatedTrans3VesselArrival;
            container.ActualTransshipment3VesselArrival = containerPM.ActualTransshipment3VesselArrival;
            container.EstimatedTransshipment3Discharge = containerPM.EstimatedTransshipment3Discharge;
            container.ActualTransshipment3Discharge = containerPM.ActualTransshipment3Discharge;
            container.EstimatedTransshipment3Loaded = containerPM.EstimatedTransshipment3Loaded;
            container.ActualTransshipment3Loaded = containerPM.ActualTransshipment3Loaded;
            container.EstimatedTrans3VesselDeparture = containerPM.EstimatedTrans3VesselDeparture;
            container.ActualTrans3VesselDeparture = containerPM.ActualTrans3VesselDeparture;
            container.Transshipment4Location = containerPM.Transshipment4Location;
            container.EstimatedTrans4VesselArrival = containerPM.EstimatedTrans4VesselArrival;
            container.ActualTransshipment4VesselArrival = containerPM.ActualTransshipment4VesselArrival;
            container.EstimatedTransshipment4Discharge = containerPM.EstimatedTransshipment4Discharge;
            container.ActualTransshipment4Discharge = containerPM.ActualTransshipment4Discharge;
            container.EstimatedTransshipment4Loaded = containerPM.EstimatedTransshipment4Loaded;
            container.ActualTransshipment4Loaded = containerPM.ActualTransshipment4Loaded;
            container.EstimatedTrans4VesselDeparture = containerPM.EstimatedTrans4VesselDeparture;
            container.ActualTrans4VesselDeparture = containerPM.ActualTrans4VesselDeparture;
            container.Leg1Vessel = containerPM.Leg1Vessel;
            container.Leg1Voyage = containerPM.Leg1Voyage;
            container.Leg2Vessel = containerPM.Leg2Vessel;
            container.Leg2Voyage = containerPM.Leg2Voyage;
            container.Leg3Vessel = containerPM.Leg3Vessel;
            container.Leg3Voyage = containerPM.Leg3Voyage;
            container.Leg4Vessel = containerPM.Leg4Vessel;
            container.Leg4Voyage = containerPM.Leg4Voyage;
            container.Leg5Vessel = containerPM.Leg5Vessel;
            container.Leg5Voyage = containerPM.Leg5Voyage;
            container.PODLocation = containerPM.PODLocation;
            container.EstimatedPODVesselArrival = containerPM.EstimatedPODVesselArrival;
            container.ActualPODVesselArrival = containerPM.ActualPODVesselArrival;
            container.EstimatedPODDischarge = containerPM.EstimatedPODDischarge;
            container.ActualPODDischarge = containerPM.ActualPODDischarge;
            container.EstimatedPODDeparture = containerPM.EstimatedPODDeparture;
            container.ActualPODDeparture = containerPM.ActualPODDeparture;
            container.DeliveryLocation = containerPM.DeliveryLocation;
            container.EstimatedDelivery = containerPM.EstimatedDelivery;
            container.ActualDelivery = containerPM.ActualDelivery;
            container.LIFLocation = containerPM.LIFLocation;
            container.EstimatedLIFArrival = containerPM.EstimatedLIFArrival;
            container.ActualLIFArrival = containerPM.ActualLIFArrival;
            container.EstimatedLIFDeparture = containerPM.EstimatedLIFDeparture;
            container.ActualLIFDeparture = containerPM.ActualLIFDeparture;
            container.GateIn = containerPM.GateIn;
            container.GateOut = containerPM.GateOut;
            container.EmptyReturnLocation = containerPM.EmptyReturnLocation;
            container.EstimatedEmptyReturn = containerPM.EstimatedEmptyReturn;
            container.ActualEmptyReturn = containerPM.ActualEmptyReturn;
            container.CustomsReleaseState = containerPM.CustomsReleaseState;
            container.CustomsReleaseDate = containerPM.CustomsReleaseDate;
            container.CarrierReleaseState = containerPM.CarrierReleaseState;
            container.CarrierReleaseDate = containerPM.CarrierReleaseDate;
            container.AvailablityDate = containerPM.AvailablityDate;
            container.AvailabilityLocation = containerPM.AvailabilityLocation;
            container.FreeDays = containerPM.FreeDays;
            container.LastFreeDayDate = containerPM.LastFreeDayDate;
            container.ShipmentStatusId = containerPM.ShipmentStatusId;
            BuildSearchField(containerPM, container);
        }

        public static void BuildSearchField(ContainerPM containerPM, Container container)
        {
            string mySearchFields = "";

            MethodHelper.AddToSearchFields(ref mySearchFields, containerPM.ContainerNumber);
            MethodHelper.AddToSearchFields(ref mySearchFields, containerPM.Master);
            MethodHelper.AddToSearchFields(ref mySearchFields, containerPM.MainCarriageCarrierNumber);
            MethodHelper.AddToSearchFields(ref mySearchFields, containerPM.CarrierName);

            containerPM.SearchFields = mySearchFields;
            container.SearchFields = mySearchFields;
        }
    }
}
