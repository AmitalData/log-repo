using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
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
            ShipmentMapping.MapContainerConcurrencyFields(containerPM, container, isNewEntity);
            BuildSearchField(containerPM, container);
        }
        public static void MapContainerFields(ContainerPM containerPM, Container container, bool isNewEntity)
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
            container.DischargeDate = containerPM.DischargeDate;
            container.ContainerNumber = containerPM.ContainerNumber;
            container.ActualEmptyPickupDate = containerPM.ActualEmptyPickupDate;
            container.EstimatedEmptyPickupDate = containerPM.EstimatedEmptyPickupDate;
            container.CurrentStatus = containerPM.CurrentStatus;
            container.CurrentStatusDate = containerPM.CurrentStatusDate;
            container.HasContainerException = containerPM.HasContainerException;
            container.CurrentLocation = containerPM.CurrentLocation;
            container.EmptyPickupLocation = containerPM.EmptyPickupLocation;
            container.DepartureLocation = containerPM.DepartureLocation;
            container.DestinationLocation = containerPM.DestinationLocation;
            container.OnCarriageLocation = containerPM.OnCarriageLocation;
            container.OnCarriageETD = containerPM.OnCarriageETD;
            container.OnCarriageATD = containerPM.OnCarriageATD;
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
            container.PreCarriageLocation = containerPM.PreCarriageLocation;
            container.PreCarriageATD = containerPM.PreCarriageATD;
            container.PreCarriageETD = containerPM.PreCarriageETD;
            container.LIFLocation = containerPM.LIFLocation;
            container.EstimatedLIFArrival = containerPM.EstimatedLIFArrival;
            container.ActualLIFArrival = containerPM.ActualLIFArrival;
            container.EstimatedOnCarriageDeparture = containerPM.EstimatedOnCarriageDeparture;
            container.ActualOnCarriageDeparture = containerPM.ActualOnCarriageDeparture;
            container.GateIn = containerPM.GateIn;
            container.GateOut = containerPM.GateOut;
            container.EmptyReturnLocation = containerPM.EmptyReturnLocation;
            container.EstimatedEmptyReturn = containerPM.EstimatedEmptyReturn;
            container.ActualEmptyReturn = containerPM.ActualEmptyReturn;
            container.CustomsReleaseState = containerPM.CustomsReleaseState;
            container.CarrierReleaseState = containerPM.CarrierReleaseState;
            container.AvailablityDate = containerPM.AvailablityDate;
            container.AvailabilityLocation = containerPM.AvailabilityLocation;
            container.FreeDays = containerPM.FreeDays;
            container.LastFreeDayDate = containerPM.LastFreeDayDate;
            container.EmptyPickupLocationPortId = containerPM.EmptyPickupLocationPortId;
            container.PreCarriageLocationPortId = containerPM.PreCarriageLocationPortId;
            container.EmptyReturnLocationPortId = containerPM.EmptyReturnLocationPortId;
            container.AvailabilityLocationPortId = containerPM.AvailabilityLocationPortId;
            container.OnCarriageLocationPortId = containerPM.OnCarriageLocationPortId;
            container.LIFLocationPortId = containerPM.LIFLocationPortId;
            container.POLLocationPortId = containerPM.POLLocationPortId;
            container.PODLocationPortId = containerPM.PODLocationPortId;
            container.Transshipment1LocationPortId = containerPM.Transshipment1LocationPortId;
            container.Transshipment2LocationPortId = containerPM.Transshipment2LocationPortId;
            container.Transshipment3LocationPortId = containerPM.Transshipment3LocationPortId;
            container.Transshipment4LocationPortId = containerPM.Transshipment4LocationPortId;
            container.Field1 = containerPM.Field1 != null ? containerPM.Field1.Value : null;
            container.Field2 = containerPM.Field2 != null ? containerPM.Field2.Value : null;
            container.Field3 = containerPM.Field3 != null ? containerPM.Field3.Value : null;
            container.Field4 = containerPM.Field4 != null ? containerPM.Field4.Value : null;
            container.Field5 = containerPM.Field5 != null ? containerPM.Field5.Value : null;
            container.Field6 = containerPM.Field6 != null ? containerPM.Field6.Value : null;
            container.Field7 = containerPM.Field7 != null ? containerPM.Field7.Value : null;
            container.Field8 = containerPM.Field8 != null ? containerPM.Field8.Value : null;
            container.Field9 = containerPM.Field9 != null ? containerPM.Field9.Value : null;
            container.Field10 = containerPM.Field10 != null ? containerPM.Field10.Value : null;
            container.TerminalAddress = containerPM.TerminalAddress;
            container.TerminalPhone = containerPM.TerminalPhone;
            container.TerminalAddressId = containerPM.TerminalAddressId;
            container.IsAutomaticUpdates = containerPM.IsAutomaticUpdates;
            container.IsClosed = containerPM.IsClosed;
            container.ClosedDate = containerPM.ClosedDate;
            container.StatusId = containerPM.StatusId;
            container.IsCancelled = containerPM.IsCancelled;
            container.CancelledDate = containerPM.CancelledDate;
            container.Leg1VesselId = containerPM.Leg1VesselId;
            container.Leg2VesselId = containerPM.Leg2VesselId;
            container.Leg3VesselId = containerPM.Leg3VesselId;
            container.Leg4VesselId = containerPM.Leg4VesselId;
            container.Leg5VesselId = containerPM.Leg5VesselId;
            container.ExceptionDate = containerPM.ExceptionDate;
            container.ExceptionDescription = containerPM.ExceptionDescription;
            container.ExceptionResolvedDescription = containerPM.ExceptionResolvedDescription;
            container.HasException = containerPM.HasException;
            container.LastExceptionDescription = containerPM.LastExceptionDescription;
            container.IsExceptionResolved = containerPM.IsExceptionResolved;
            container.PreCarriageGateIn = containerPM.PreCarriageGateIn;
            container.OnCarriageGateOut = containerPM.OnCarriageGateOut;
            container.UpdatedByPartner = SetUpdatedByPartner(containerPM);
            container.ContainerTypeId = containerPM.ContainerTypeId;
            container.Volume = containerPM.Volume;
            container.VolumeUnitCode = containerPM.VolumeUnitCode;
            container.GrossWeight = containerPM.GrossWeight;
            container.GrossWeightUnitCode = containerPM.GrossWeightUnitCode;
            container.AdditionalReference1 = containerPM.AdditionalReference1;
            container.AdditionalReference2 = containerPM.AdditionalReference2;
            container.AdditionalReference3 = containerPM.AdditionalReference3;
            container.AdditionalReference4 = containerPM.AdditionalReference4;
            container.HasTransshipments = containerPM.HasTransshipments;

            container.Field11 = containerPM.Field11 != null ? containerPM.Field11.Value : null;
            container.Field12 = containerPM.Field12 != null ? containerPM.Field12.Value : null;
            container.Field13 = containerPM.Field13 != null ? containerPM.Field13.Value : null;
            container.Field14 = containerPM.Field14 != null ? containerPM.Field14.Value : null;
            container.Field15 = containerPM.Field15 != null ? containerPM.Field15.Value : null;
            container.Field16 = containerPM.Field16 != null ? containerPM.Field16.Value : null;
            container.Field17 = containerPM.Field17 != null ? containerPM.Field17.Value : null;
            container.Field18 = containerPM.Field18 != null ? containerPM.Field18.Value : null;
            container.Field19 = containerPM.Field19 != null ? containerPM.Field19.Value : null;
            container.Field20 = containerPM.Field20 != null ? containerPM.Field20.Value : null;

            container.Field21 = containerPM.Field21 != null ? containerPM.Field21.Value : null;
            container.Field22 = containerPM.Field22 != null ? containerPM.Field22.Value : null;
            container.Field23 = containerPM.Field23 != null ? containerPM.Field23.Value : null;
            container.Field24 = containerPM.Field24 != null ? containerPM.Field24.Value : null;
            container.Field25 = containerPM.Field25 != null ? containerPM.Field25.Value : null;
            container.Field26 = containerPM.Field26 != null ? containerPM.Field26.Value : null;
            container.Field27 = containerPM.Field27 != null ? containerPM.Field27.Value : null;
            container.Field28 = containerPM.Field28 != null ? containerPM.Field28.Value : null;
            container.Field29 = containerPM.Field29 != null ? containerPM.Field29.Value : null;
            container.Field30 = containerPM.Field30 != null ? containerPM.Field30.Value : null;

            container.Field31 = containerPM.Field31 != null ? containerPM.Field31.Value : null;
            container.Field32 = containerPM.Field32 != null ? containerPM.Field32.Value : null;
            container.Field33 = containerPM.Field33 != null ? containerPM.Field33.Value : null;
            container.Field34 = containerPM.Field34 != null ? containerPM.Field34.Value : null;
            container.Field35 = containerPM.Field35 != null ? containerPM.Field35.Value : null;
            container.Field36 = containerPM.Field36 != null ? containerPM.Field36.Value : null;
            container.Field37 = containerPM.Field37 != null ? containerPM.Field37.Value : null;
            container.Field38 = containerPM.Field38 != null ? containerPM.Field38.Value : null;
            container.Field39 = containerPM.Field39 != null ? containerPM.Field39.Value : null;
            container.Field40 = containerPM.Field40 != null ? containerPM.Field40.Value : null;
        }
        private static string SetUpdatedByPartner(ContainerPM entityPM)
        {
            string updatedByPartner = null;
            if (entityPM.IsUpdatedOceanInsightsAnalyzer)
            {
                updatedByPartner = "Ocean Insights";
            }else if (entityPM.IsUpdatedVizionAnalyzer)
            {
                updatedByPartner = "Vizion";
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

        public static void MapContainerShipmentFields(ContainerPM containerPM, Container container)
        {
            container.ShipmentId = containerPM.ShipmentId;
            container.CustomsReleaseDate = containerPM.CustomsReleaseDate;
            container.CarrierReleaseDate = containerPM.CarrierReleaseDate;
            container.Master = containerPM.Master;
            container.VesselName = containerPM.VesselName;
            container.MainCarriageCarrierId = containerPM.MainCarriageCarrierId;
            container.MainCarriageCarrierNumber = containerPM.MainCarriageCarrierNumber;
            container.MainCarriageATA = containerPM.MainCarriageATA;
            container.MainCarriageATD = containerPM.MainCarriageATD;
            container.MainCarriageETA = containerPM.MainCarriageETA;
            container.MainCarriageETD = containerPM.MainCarriageETD;
            container.MainCarriageVesselId = containerPM.MainCarriageVesselId;
            container.ShipmentPackagesId = containerPM.ShipmentPackagesId;
            container.ShipmentStatusId = containerPM.ShipmentStatusId;
            container.ShipmentPickupETA = containerPM.ShipmentPickupETA;
            container.ShipmentPickupETD = containerPM.ShipmentPickupETD;
            container.ShipmentPickupATA = containerPM.ShipmentPickupATA;
            container.ShipmentPickupATD = containerPM.ShipmentPickupATD;
            container.ShipmentPreCarriageETA = containerPM.ShipmentPreCarriageETA;
            container.ShipmentPreCarriageETD = containerPM.ShipmentPreCarriageETD;
            container.ShipmentPreCarriageATA = containerPM.ShipmentPreCarriageATA;
            container.ShipmentPreCarriageATD = containerPM.ShipmentPreCarriageATD;
            container.ShipmentMainCarriageETA = containerPM.ShipmentMainCarriageETA;
            container.ShipmentMainCarriageETD = containerPM.ShipmentMainCarriageETD;
            container.ShipmentMainCarriageATA = containerPM.ShipmentMainCarriageATA;
            container.ShipmentMainCarriageATD = containerPM.ShipmentMainCarriageATD;
            container.ShipmentTransshipment1ETA = containerPM.ShipmentTransshipment1ETA;
            container.ShipmentTransshipment1ETD = containerPM.ShipmentTransshipment1ETD;
            container.ShipmentTransshipment1ATA = containerPM.ShipmentTransshipment1ATA;
            container.ShipmentTransshipment1ATD = containerPM.ShipmentTransshipment1ATD;
            container.ShipmentTransshipment2ETA = containerPM.ShipmentTransshipment2ETA;
            container.ShipmentTransshipment2ETD = containerPM.ShipmentTransshipment2ETD;
            container.ShipmentTransshipment2ATA = containerPM.ShipmentTransshipment2ATA;
            container.ShipmentTransshipment2ATD = containerPM.ShipmentTransshipment2ATD;
            container.ShipmentTransshipment3ETA = containerPM.ShipmentTransshipment3ETA;
            container.ShipmentTransshipment3ETD = containerPM.ShipmentTransshipment3ETD;
            container.ShipmentTransshipment3ATA = containerPM.ShipmentTransshipment3ATA;
            container.ShipmentTransshipment3ATD = containerPM.ShipmentTransshipment3ATD;
            container.ShipmentOnCarriageETA = containerPM.ShipmentOnCarriageETA;
            container.ShipmentOnCarriageETD = containerPM.ShipmentOnCarriageETD;
            container.ShipmentOnCarriageATA = containerPM.ShipmentOnCarriageATA;
            container.ShipmentOnCarriageATD = containerPM.ShipmentOnCarriageATD;
            container.ShipmentDeliveryETA = containerPM.ShipmentDeliveryETA;
            container.ShipmentDeliveryETD = containerPM.ShipmentDeliveryETD;
            container.ShipmentDeliveryATA = containerPM.ShipmentDeliveryATA;
            container.ShipmentDeliveryATD = containerPM.ShipmentDeliveryATD;
            container.ShipmentOriginAgentId = containerPM.ShipmentOriginAgentId;
            container.ShipmentDestinationAgentId = containerPM.ShipmentDestinationAgentId;
            container.ShipmentNumber = containerPM.ShipmentNumber;
            container.ShipmentTypeId = containerPM.ShipmentTypeId;
            container.ShipmentCreateDate = containerPM.ShipmentCreateDate;
            container.ShipmentDeliveryTruckerId = containerPM.ShipmentDeliveryTruckerId;
            container.ShipmentPickupFrom = containerPM.ShipmentPickupFrom;
            container.ShipmentPickupTo = containerPM.ShipmentPickupTo;
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
            container.ShipmentDeliveryFrom = containerPM.ShipmentDeliveryFrom;
            container.ShipmentDeliveryTo = containerPM.ShipmentDeliveryTo;
            container.ContainersCount = containerPM.ContainersCount;
            container.HandlerId = containerPM.HandlerId;
            container.CustomerId = containerPM.CustomerId;
            container.OPClosed = containerPM.OPClosed;
            container.TerminalId = containerPM.TerminalId;
            container.PODReceivedOnDate = containerPM.PODReceivedOnDate;
            container.EmptyContainerReturnETA = containerPM.EmptyContainerReturnETA;
            container.EmptyContainerReturnATA = containerPM.EmptyContainerReturnATA;
            container.EmptyContainerReturnETD = containerPM.EmptyContainerReturnETD;
            container.EmptyContainerReturnATD = containerPM.EmptyContainerReturnATD;
            container.EmptyContainerReturnFrom = containerPM.EmptyContainerReturnFrom;
            container.EmptyContainerReturnTo = containerPM.EmptyContainerReturnTo;
        }
        public static void BuildSearchField(ContainerPM containerPM, Container container)
        {
            string mySearchFields = "";

            MethodHelper.AddToSearchFields(ref mySearchFields, containerPM.ContainerNumber);
            MethodHelper.AddToSearchFields(ref mySearchFields, containerPM.Master);
            MethodHelper.AddToSearchFields(ref mySearchFields, containerPM.MainCarriageCarrierNumber);
            MethodHelper.AddToSearchFields(ref mySearchFields, containerPM.CarrierName);
            MethodHelper.AddToSearchFields(ref mySearchFields, containerPM.ShipmentNumber);

            containerPM.SearchFields = mySearchFields;
            container.SearchFields = mySearchFields;
        }
    }
}
