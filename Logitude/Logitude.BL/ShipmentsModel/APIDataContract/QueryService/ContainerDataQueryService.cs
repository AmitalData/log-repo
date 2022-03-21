using Logitude.BL.CommonDataModel.APIDataContract.ApiV1;
using Logitude.BL.ShipmentsModel.APIDataContract.ApiV1;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Simplog.Data.ShipmentsModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.ShipmentsModel.APIDataContract.ApiV1
{
    public partial class ContainerDataQueryService
    {
        public ContainerData GetContainerDataByContainerNumberAndShipmentNumber(string containerNumber, string shipmentNumber, int tenant)
        {
            try
            {
                ContainerPM containerPM = query.GetContainerByNumberAndShipmentNumber(containerNumber, shipmentNumber, tenant);
                if (containerPM == null)
                {
                    throw new ApplicationException("Container with number " + containerNumber + " and shipment number " + shipmentNumber + " doesn't exist");
                }

                return ContainerDataDataMapping(containerPM, tenant, null);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

		public ContainerPM ContainerDataCustomDataMapping(ContainerData containerData, int tenant, string ComputingPartnerName = "", bool IsUpdate = false)
		{
			try
			{
				PortQueryService portService = new PortQueryService(tenant);
				UserQueryService userService = new UserQueryService(tenant);
				CardQueryService cardService = new CardQueryService(tenant);
				VesselQueryService vesselService = new VesselQueryService(tenant);

				ContainerPM containerPM = new ContainerPM();

				if (!string.IsNullOrEmpty(containerData.ContainerNumber) && !string.IsNullOrEmpty(containerData.ShipmentNumber))
				{
					containerPM = query.GetContainerByNumberAndShipmentNumber(containerData.ContainerNumber, containerData.ShipmentNumber, tenant);
				}

				if (containerPM == null)
				{
					throw new ApplicationException("Container with number " + containerData.ContainerNumber + " and shipment number " + containerData.ShipmentNumber +  " doesn't exist");
				}

				containerPM.DischargeDate = containerData.DischargeDate;
				containerPM.EstimatedEmptyPickupDate = containerData.EstimatedEmptyPickupDate;
				containerPM.ActualEmptyPickupDate = containerData.ActualEmptyPickupDate;
				containerPM.EmptyPickupLocation = containerData.EmptyPickupLocation;
				containerPM.DepartureLocation = containerData.DepartureLocation;
				containerPM.DestinationLocation = containerData.DestinationLocation;
				containerPM.PreCarriageETD = containerData.PreCarriageETD;
				containerPM.PreCarriageATD = containerData.PreCarriageATD;
				containerPM.POLLocation = containerData.POLLocation;
				containerPM.EstimatedPOLArrival = containerData.EstimatedPOLArrival;
				containerPM.ActualPOLArrival = containerData.ActualPOLArrival;
				containerPM.EstimatedPOLLoaded = containerData.EstimatedPOLLoaded;
				containerPM.ActualPOLLoaded = containerData.ActualPOLLoaded;
				containerPM.EstimatedPOLVesselDeparture = containerData.EstimatedPOLVesselDeparture;
				containerPM.ActualPOLVesselDeparture = containerData.ActualPOLVesselDeparture;
				containerPM.Transshipment1Location = containerData.Transshipment1Location;
				containerPM.EstimatedTrans1VesselArrival = containerData.EstimatedTrans1VesselArrival;
				containerPM.ActualTransshipment1VesselArrival = containerData.ActualTransshipment1VesselArrival;
				containerPM.EstimatedTransshipment1Discharge = containerData.EstimatedTransshipment1Discharge;
				containerPM.ActualTransshipment1Discharge = containerData.ActualTransshipment1Discharge;
				containerPM.EstimatedTransshipment1Loaded = containerData.EstimatedTransshipment1Loaded;
				containerPM.ActualTransshipment1Loaded = containerData.ActualTransshipment1Loaded;
				containerPM.EstimatedTrans1VesselDeparture = containerData.EstimatedTrans1VesselDeparture;
				containerPM.ActualTrans1VesselDeparture = containerData.ActualTrans1VesselDeparture;
				containerPM.Transshipment2Location = containerData.Transshipment2Location;
				containerPM.EstimatedTrans2VesselArrival = containerData.EstimatedTrans2VesselArrival;
				containerPM.ActualTransshipment2VesselArrival = containerData.ActualTransshipment2VesselArrival;
				containerPM.EstimatedTransshipment2Discharge = containerData.EstimatedTransshipment2Discharge;
				containerPM.ActualTransshipment2Discharge = containerData.ActualTransshipment2Discharge;
				containerPM.EstimatedTransshipment2Loaded = containerData.EstimatedTransshipment2Loaded;
				containerPM.ActualTransshipment2Loaded = containerData.ActualTransshipment2Loaded;
				containerPM.EstimatedTrans2VesselDeparture = containerData.EstimatedTrans2VesselDeparture;
				containerPM.ActualTrans2VesselDeparture = containerData.ActualTrans2VesselDeparture;
				containerPM.Transshipment3Location = containerData.Transshipment3Location;
				containerPM.EstimatedTrans3VesselArrival = containerData.EstimatedTrans3VesselArrival;
				containerPM.ActualTransshipment3VesselArrival = containerData.ActualTransshipment3VesselArrival;
				containerPM.EstimatedTransshipment3Discharge = containerData.EstimatedTransshipment3Discharge;
				containerPM.ActualTransshipment3Discharge = containerData.ActualTransshipment3Discharge;
				containerPM.EstimatedTransshipment3Loaded = containerData.EstimatedTransshipment3Loaded;
				containerPM.ActualTransshipment3Loaded = containerData.ActualTransshipment3Loaded;
				containerPM.EstimatedTrans3VesselDeparture = containerData.EstimatedTrans3VesselDeparture;
				containerPM.ActualTrans3VesselDeparture = containerData.ActualTrans3VesselDeparture;
				containerPM.Transshipment4Location = containerData.Transshipment4Location;
				containerPM.EstimatedTrans4VesselArrival = containerData.EstimatedTrans4VesselArrival;
				containerPM.ActualTransshipment4VesselArrival = containerData.ActualTransshipment4VesselArrival;
				containerPM.EstimatedTransshipment4Discharge = containerData.EstimatedTransshipment4Discharge;
				containerPM.ActualTransshipment4Discharge = containerData.ActualTransshipment4Discharge;
				containerPM.EstimatedTransshipment4Loaded = containerData.EstimatedTransshipment4Loaded;
				containerPM.ActualTransshipment4Loaded = containerData.ActualTransshipment4Loaded;
				containerPM.EstimatedTrans4VesselDeparture = containerData.EstimatedTrans4VesselDeparture;
				containerPM.ActualTrans4VesselDeparture = containerData.ActualTrans4VesselDeparture;
				containerPM.Leg1Voyage = containerData.Leg1Voyage;
				containerPM.Leg2Voyage = containerData.Leg2Voyage;
				containerPM.Leg3Voyage = containerData.Leg3Voyage;
				containerPM.Leg4Voyage = containerData.Leg4Voyage;
				containerPM.Leg5Voyage = containerData.Leg5Voyage;
				containerPM.PODLocation = containerData.PODLocation;
				containerPM.EstimatedPODVesselArrival = containerData.EstimatedPODVesselArrival;
				containerPM.ActualPODVesselArrival = containerData.ActualPODVesselArrival;
				containerPM.EstimatedPODDischarge = containerData.EstimatedPODDischarge;
				containerPM.ActualPODDischarge = containerData.ActualPODDischarge;
				containerPM.EstimatedPODDeparture = containerData.EstimatedPODDeparture;
				containerPM.ActualPODDeparture = containerData.ActualPODDeparture;
				containerPM.OnCarriageLocation = containerData.OnCarriageLocation;
				containerPM.OnCarriageETD = containerData.OnCarriageETD;
				containerPM.OnCarriageATD = containerData.OnCarriageATD;
				containerPM.LIFLocation = containerData.LIFLocation;
				containerPM.EstimatedLIFArrival = containerData.EstimatedLIFArrival;
				containerPM.ActualLIFArrival = containerData.ActualLIFArrival;
				containerPM.EstimatedOnCarriageDeparture = containerData.EstimatedOnCarriageDeparture;
				containerPM.ActualOnCarriageDeparture = containerData.ActualOnCarriageDeparture;
				containerPM.GateIn = containerData.GateIn;
				containerPM.GateOut = containerData.GateOut;
				containerPM.EmptyReturnLocation = containerData.EmptyReturnLocation;
				containerPM.EstimatedEmptyReturn = containerData.EstimatedEmptyReturn;
				containerPM.ActualEmptyReturn = containerData.ActualEmptyReturn;
				containerPM.CustomsReleaseState = containerData.CustomsReleaseState;
				containerPM.CustomsReleaseDate = containerData.CustomsReleaseDate;
				containerPM.CarrierReleaseState = containerData.CarrierReleaseState;
				containerPM.CarrierReleaseDate = containerData.CarrierReleaseDate;
				containerPM.AvailablityDate = containerData.AvailablityDate;
				containerPM.AvailabilityLocation = containerData.AvailabilityLocation;
				containerPM.LastFreeDayDate = containerData.LastFreeDayDate;
				containerPM.FreeDays = containerData.FreeDays;
				containerPM.TerminalPhone = containerData.TerminalPhone;
				containerPM.IsClosed = containerData.IsClosed;
				containerPM.ClosedDate = containerData.ClosedDate;
				containerPM.OnCarriageGateOut = containerData.OnCarriageGateOut;
				containerPM.PreCarriageGateIn = containerData.PreCarriageGateIn;

				if (!IsUpdate)
				{
					containerPM.ContainerNumber = containerData.ContainerNumber;
					containerPM.PreCarriageLocation = containerData.PreCarriageLocation;
					containerPM.IsCancelled = containerData.IsCancelled;
					containerPM.CancelledDate = containerData.CancelledDate;
					containerPM.ShipmentNumber = containerData.ShipmentNumber;
				}				

				if (containerData.UpdatedByUser != null)
				{
					var myUpdatedByUserPM = userService.UserDataMappingAndValidatin(containerData.UpdatedByUser, tenant, ComputingPartnerName, IsUpdate);
					if (myUpdatedByUserPM != null)
					{
						containerPM.UpdatedByUserId = myUpdatedByUserPM.Id;
					}

					else
                    {
						containerPM.UpdatedByUserId = null;
					}
				}
				else
				{
					containerPM.UpdatedByUserId = null;
				}

				if (containerData.EmptyPickupLocationPort != null)
				{
					var myEmptyPickupLocationPortPM = portService.PortDataMappingAndValidatin(containerData.EmptyPickupLocationPort, tenant, ComputingPartnerName, IsUpdate);
					if (myEmptyPickupLocationPortPM != null)
					{
						containerPM.EmptyPickupLocationPortId = myEmptyPickupLocationPortPM.Id;
					}
				}

				if (containerData.OnCarriageLocationPort != null)
				{
					var myOnCarriageLocationPortPM = portService.PortDataMappingAndValidatin(containerData.OnCarriageLocationPort, tenant, ComputingPartnerName, IsUpdate);
					if (myOnCarriageLocationPortPM != null)
					{
						containerPM.OnCarriageLocationPortId = myOnCarriageLocationPortPM.Id;
					}
				}

				if (containerData.EmptyReturnLocationPort != null)
				{
					var myEmptyReturnLocationPortPM = portService.PortDataMappingAndValidatin(containerData.EmptyReturnLocationPort, tenant, ComputingPartnerName, IsUpdate);
					if (myEmptyReturnLocationPortPM != null)
					{
						containerPM.EmptyReturnLocationPortId = myEmptyReturnLocationPortPM.Id;
					}
				}

				if (containerData.AvailabilityLocationPort != null)
				{
					var myAvailabilityLocationPortPM = portService.PortDataMappingAndValidatin(containerData.AvailabilityLocationPort, tenant, ComputingPartnerName, IsUpdate);
					if (myAvailabilityLocationPortPM != null)
					{
						containerPM.AvailabilityLocationPortId = myAvailabilityLocationPortPM.Id;
					}
				}

				if (containerData.PreCarriageLocationPort != null)
				{
					var myPreCarriageLocationPortPM = portService.PortDataMappingAndValidatin(containerData.PreCarriageLocationPort, tenant, ComputingPartnerName, IsUpdate);
					if (myPreCarriageLocationPortPM != null)
					{
						containerPM.PreCarriageLocationPortId = myPreCarriageLocationPortPM.Id;
					}
				}

				if (containerData.LIFLocationPort != null)
				{
					var myLIFLocationPortPM = portService.PortDataMappingAndValidatin(containerData.LIFLocationPort, tenant, ComputingPartnerName, IsUpdate);
					if (myLIFLocationPortPM != null)
					{
						containerPM.LIFLocationPortId = myLIFLocationPortPM.Id;
					}
				}

				if (containerData.POLLocationPort != null)
				{
					var myPOLLocationPortPM = portService.PortDataMappingAndValidatin(containerData.POLLocationPort, tenant, ComputingPartnerName, IsUpdate);
					if (myPOLLocationPortPM != null)
					{
						containerPM.POLLocationPortId = myPOLLocationPortPM.Id;
					}
				}

				if (containerData.PODLocationPort != null)
				{
					var myPODLocationPortPM = portService.PortDataMappingAndValidatin(containerData.PODLocationPort, tenant, ComputingPartnerName, IsUpdate);
					if (myPODLocationPortPM != null)
					{
						containerPM.PODLocationPortId = myPODLocationPortPM.Id;
					}
				}

				if (containerData.Transshipment1LocationPort != null)
				{
					var myTransshipment1LocationPortPM = portService.PortDataMappingAndValidatin(containerData.Transshipment1LocationPort, tenant, ComputingPartnerName, IsUpdate);
					if (myTransshipment1LocationPortPM != null)
					{
						containerPM.Transshipment1LocationPortId = myTransshipment1LocationPortPM.Id;
					}
				}

				if (containerData.Transshipment2LocationPort != null)
				{
					var myTransshipment2LocationPortPM = portService.PortDataMappingAndValidatin(containerData.Transshipment2LocationPort, tenant, ComputingPartnerName, IsUpdate);
					if (myTransshipment2LocationPortPM != null)
					{
						containerPM.Transshipment2LocationPortId = myTransshipment2LocationPortPM.Id;
					}
				}

				if (containerData.Transshipment3LocationPort != null)
				{
					var myTransshipment3LocationPortPM = portService.PortDataMappingAndValidatin(containerData.Transshipment3LocationPort, tenant, ComputingPartnerName, IsUpdate);
					if (myTransshipment3LocationPortPM != null)
					{
						containerPM.Transshipment3LocationPortId = myTransshipment3LocationPortPM.Id;
					}
				}

				if (containerData.Transshipment4LocationPort != null)
				{
					var myTransshipment4LocationPortPM = portService.PortDataMappingAndValidatin(containerData.Transshipment4LocationPort, tenant, ComputingPartnerName, IsUpdate);
					if (myTransshipment4LocationPortPM != null)
					{
						containerPM.Transshipment4LocationPortId = myTransshipment4LocationPortPM.Id;
					}
				}
				
				if (containerData.Terminal != null)
				{
					var myTerminalPM = cardService.CardDataMappingAndValidatin(containerData.Terminal, tenant, ComputingPartnerName, IsUpdate);
					if (myTerminalPM != null)
					{
						containerPM.TerminalId = myTerminalPM.Id;
					}
				}
				
				if (containerData.Leg1Vessel != null)
				{
					var myLeg1VesselPM = vesselService.VesselDataMappingAndValidatin(containerData.Leg1Vessel, tenant, ComputingPartnerName, IsUpdate);
					if (myLeg1VesselPM != null)
					{
						containerPM.Leg1VesselId = myLeg1VesselPM.Id;
					}
				}

				if (containerData.Leg2Vessel != null)
				{
					var myLeg2VesselPM = vesselService.VesselDataMappingAndValidatin(containerData.Leg2Vessel, tenant, ComputingPartnerName, IsUpdate);
					if (myLeg2VesselPM != null)
					{
						containerPM.Leg2VesselId = myLeg2VesselPM.Id;
					}
				}

				if (containerData.Leg3Vessel != null)
				{
					var myLeg3VesselPM = vesselService.VesselDataMappingAndValidatin(containerData.Leg3Vessel, tenant, ComputingPartnerName, IsUpdate);
					if (myLeg3VesselPM != null)
					{
						containerPM.Leg3VesselId = myLeg3VesselPM.Id;
					}
				}

				if (containerData.Leg4Vessel != null)
				{
					var myLeg4VesselPM = vesselService.VesselDataMappingAndValidatin(containerData.Leg4Vessel, tenant, ComputingPartnerName, IsUpdate);
					if (myLeg4VesselPM != null)
					{
						containerPM.Leg4VesselId = myLeg4VesselPM.Id;
					}
				}

				if (containerData.Leg5Vessel != null)
				{
					var myLeg5VesselPM = vesselService.VesselDataMappingAndValidatin(containerData.Leg5Vessel, tenant, ComputingPartnerName, IsUpdate);
					if (myLeg5VesselPM != null)
					{
						containerPM.Leg5VesselId = myLeg5VesselPM.Id;
					}
				}

				return containerPM;
			}

			catch (Exception ex)
			{
				throw ex;
			}
		}
	}
}
