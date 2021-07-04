using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.BL.ShipmentsModel.Tools.EntityService;
using Logitude.Server.Tools.Counters;
using Logitude.XSD.INTTRA_Status;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.XSD.Analyzers.INTTRAAnalyzer
{
    public partial class INTTRAAnalyzer
    {
        private string ContainerNumber = null;
        private ShipmentPackagePM iContainer = null;
        private void Analyze_Status()
        {
            if (this.iMessage_Status != null)
            {
                INTTRA_Status.MessageBodyType iMessageBody = iMessage_Status.MessageBody;

                if (iMessageBody != null)
                {
                    this.shipmentPM.IsUpdatedByINTTRAAnalyzer = true;
                                        
                    bool isContainerExists_Message = false;
                    bool isContainerExists_Shipment = false;

                    INTTRA_Status.MessageDetailsType iMessageDetails = iMessageBody.MessageDetails;
                    if (iMessageDetails != null)
                    {
                        INTTRA_Status.EquipmentDetailsType equipmentDetails = iMessageDetails.EquipmentDetails;
                        if (equipmentDetails != null)
                        {
                            if (equipmentDetails.EquipmentIdentifier != null)
                            {
                                if (!string.IsNullOrEmpty(equipmentDetails.EquipmentIdentifier.Value))
                                {
                                    isContainerExists_Message = true;
                                    this.ContainerNumber = equipmentDetails.EquipmentIdentifier.Value;

                                    if (this.ContainerNumber != null)
                                    {
                                        this.ContainerNumber = this.ContainerNumber.Trim();

                                        iContainer = this.shipmentPM.ShipmentPackages.Where(d => d.ContainerNumber != null && d.ContainerNumber.ToUpper() == this.ContainerNumber.ToUpper()).FirstOrDefault();

                                        if (iContainer != null)
                                        {
                                            isContainerExists_Shipment = true;
                                        }
                                    }
                                }
                            }
                        }
                    }

                    if (!isContainerExists_Message)
                    {
                        throw new Exception("Unknown message Container No.");
                    }

                    else if (!isContainerExists_Shipment)
                    {
                        throw new Exception("There is no Container " + this.ContainerNumber + " for this Shipment");
                    }

                    else
                    {                        
                        INTTRA_Status.MessagePropertiesType iMessageProperties = iMessageBody.MessageProperties;

                        if (iMessageProperties != null)
                        {
                            if (iMessageProperties.EventLocation != null)
                            {
                                if (iMessageProperties.EventLocation.Location != null)
                                {
                                    this.ReadShippingLine(iMessageProperties);
                                    this.ReadEventLocation(iMessageProperties);
                                    this.ReadRoutingLocations(iMessageProperties);
                                    this.BuildStatus();
                                }
                            }
                        }
                    }
                }
            }
        }

        private void BuildStatus()
        {
            string systemEmail = "system@tenant" + this.Tenant + ".com";
            string iHash = GetHashedData(this.shipmentPM.Id, this.ShipmentNumber);

            if (!iShipmentContainerStatusRepository.DoesRecordExist(iHash))
            {
                DateTime iLogDate = TenantServerConfigration.GetCurrentDateTime(this.Tenant);

                if (this.isUpdatingShipmentDates)
                {
                    this.UpdateShipmentRoutings();
                }

                this.UpdateContainerFields();
                this.UpdateLastStatus(this.EventLocationCode, this.EventLocationeDate, iLogDate, iContainer);
                shipmentPM.INTTRALastStatusDate = iLogDate;

                ShipmentService service = new ShipmentService(myShipmentContext, shipmentPM, systemEmail);
                service.Update(true);

                ShipmentContainerStatus iStatus = new ShipmentContainerStatus()
                {
                    Id = IdCounter.GetNumber("ShipmentContainerStatus", this.Tenant),
                    ShipmentId = this.ShipmentId,
                    ContainerId = iContainer.Id,
                    RecordHash = iHash,
                    Tenant = this.Tenant,
                    StatusCode = this.EventLocationCode,
                    EventDate = EventLocationeDate,
                    FromPortId = DeparturePortId,
                    ToPortId = ArrivalPortId,
                    DepartureDate = DepartureDate,
                    ArrivalDate = ArrivalDate,
                    Details = this.EventLocationCode,
                    ReceivingDate = iLogDate,
                    VoyageNumber = this.VoyageNumber,
                    VesselName = this.VesselName,
                    ShippingLineName = ShippingLineName,
                    ContainerNumber = ContainerNumber,
                    Location = this.EventLocationPortId,
                    TimeOfArrivalInfo = ArrivalDateIndicator,
                    TimeOfDepartureInfo = DepartureDateIndicator,
                };

                iShipmentContainerStatusRepository.Add(iStatus);

                if (this.shipmentPM.ShipmentLevelCode == "C")
                {
                    if (this.shipmentPM.ShipmentConsoleShipments.Count > 0)
                    {
                        foreach (ConsoleShipmentPM item in shipmentPM.ShipmentConsoleShipments)
                        {
                            #region

                            ShipmentPM iHouse = iShipmentQuery.GetSinglePM(item.Id, this.Tenant);

                            ShipmentPackagePM iHouseContainer = iHouse.ShipmentPackages.Where(d => d.ContainerNumber == ContainerNumber && d.OriginalShipmentPackageId == iContainer.Id).FirstOrDefault();
                            if (iHouseContainer == null)
                            {
                                iHouseContainer = iHouse.ShipmentPackages.Where(d => d.ContainerNumber == ContainerNumber).FirstOrDefault();
                            }

                            if (iHouseContainer != null)
                            {
                                this.UpdateLastStatus(this.EventLocationCode, this.EventLocationeDate, iLogDate, iHouseContainer);
                                iHouse.INTTRALastStatusDate = iLogDate;
                                
                                ShipmentService iHouseService = new ShipmentService(myShipmentContext, iHouse, systemEmail);
                                iHouseService.Update(true);

                                string iHouseHash = GetHashedData(iHouse.Id, iHouse.ShipmentNumber);

                                if (!iShipmentContainerStatusRepository.DoesRecordExist(iHouseHash))
                                {
                                    ShipmentContainerStatus iHouseStatus = new ShipmentContainerStatus()
                                    {
                                        Id = IdCounter.GetNumber("ShipmentContainerStatus", this.Tenant),
                                        ShipmentId = iHouse.Id,
                                        ContainerId = iHouseContainer.Id,
                                        RecordHash = iHouseHash,
                                        Tenant = this.Tenant,
                                        StatusCode = this.EventLocationCode,
                                        EventDate = EventLocationeDate,
                                        FromPortId = DeparturePortId,
                                        ToPortId = ArrivalPortId,
                                        DepartureDate = DepartureDate,
                                        ArrivalDate = ArrivalDate,
                                        Details = this.EventLocationCode,
                                        ReceivingDate = iLogDate,
                                        VoyageNumber = this.VoyageNumber,
                                        VesselName = this.VesselName,
                                        ShippingLineName = ShippingLineName,
                                        ContainerNumber = ContainerNumber,
                                        Location = EventLocationPortId,
                                        TimeOfArrivalInfo = ArrivalDateIndicator,
                                        TimeOfDepartureInfo = DepartureDateIndicator,
                                    };

                                    iShipmentContainerStatusRepository.Add(iHouseStatus);
                                }
                            }
                            #endregion
                        }
                    }
                }

                iShipmentContainerStatusRepository.SubmitChanges();
            }
        }

        private string VesselName = null;
        private string VoyageNumber = null;
        private string ShippingLineName = null;
        private bool isUpdatingShipmentDates = false;
        private void ReadShippingLine(MessagePropertiesType iMessageProperties)
        {
            this.ShippingLineName = shipmentPM.MainCarriageCarrierName;

            if (iMessageProperties.TransportationDetails != null)
            {
                if (iMessageProperties.TransportationDetails.ConveyanceInformation != null)
                {
                    this.VoyageNumber = iMessageProperties.TransportationDetails.ConveyanceInformation.VoyageTripNumber;
                    this.VesselName = iMessageProperties.TransportationDetails.ConveyanceInformation.ConveyanceName;

                    string d = iMessageProperties.TransportationDetails.ConveyanceInformation.CarrierSCAC;
                    string f = iMessageProperties.TransportationDetails.ConveyanceInformation.TransportIdentification.Value;
                }
            }

            if (shipmentPM.MainCarriageCarrierId != null)
            {
                ShippingLineRepository iShippingLineRepository = new ShippingLineRepository(myCommonContext);
                ShippingLine iShippingLine = iShippingLineRepository.GetSingleShippingLine(shipmentPM.MainCarriageCarrierId, this.Tenant);
                if (iShippingLine != null)
                {
                    this.isUpdatingShipmentDates = iShippingLine.INTTRAUpdatesShipment;
                }
            }
        }

        private string EventLocationCode = null;
        private string EventLocationDirection = null;
        private DateTime? EventLocationeDate = null;
        private string EventLocationDateString = "";
        private string EventLocationPortId = null;
        private string EventLocationPortCode = null;
        private void ReadEventLocation(INTTRA_Status.MessagePropertiesType iMessageProperties)
        {
            INTTRA_Status.LocationType location = iMessageProperties.EventLocation.Location;

            if (location != null)
            {
                this.EventLocationCode = iMessageProperties.EventCode;
                this.EventLocationDirection = this.GetEventDirection(this.EventLocationCode);
                this.EventLocationeDate = this.GetDateFromString(location);

                if (EventLocationeDate != null)
                {
                    this.EventLocationDateString = EventLocationeDate.ToString();
                }

                string EventCountryCode = location.LocationCountry;
                string EventCombinedCode = location.LocationCode.Value;
                string EventPortCode = EventCombinedCode.Substring(EventCountryCode.Length);

                Port iEventPort = iPortRepository.GetOceanPortByCodeAndCountryCode(this.Tenant, EventPortCode, EventCountryCode, true);
                if (iEventPort == null)
                {
                    iEventPort = iPortRepository.GetOceanPortByCodeAndCountryCode(0, EventPortCode, EventCountryCode, true);
                    if (iEventPort != null)
                    {
                        iEventPort = this.GetPortCopyToCurrentTenant(iEventPort.Id, this.Tenant);
                    }
                }

                if (iEventPort != null)
                {
                    this.EventLocationPortId = iEventPort.Id;
                }

                this.EventLocationPortCode = EventPortCode;
            }
        }

        private string DeparturePortId = null;
        private string DeparturePortCode = null;
        private DateTime? DepartureDate = null;
        private string DepartureDateIndicator = null;
        private string ArrivalPortId = null;
        private string ArrivalPortCode = null;
        private DateTime? ArrivalDate = null;
        private string ArrivalDateIndicator = null;
        private bool HasIntermediatePort = false;
        private string ShipmentRoutingLegCode = "Main";
        private INTTRA_Status.LocationType1 location_From = null;
        private INTTRA_Status.LocationType1 location_To = null;
        private List<INTTRA_Status.LocationType1> RoutingLocations = new List<LocationType1>();
        private void ReadRoutingLocations(MessagePropertiesType iMessageProperties)
        {
            if (iMessageProperties.TransportationDetails != null)
            {
                if (iMessageProperties.TransportationDetails.Location != null)
                {
                    this.RoutingLocations = iMessageProperties.TransportationDetails.Location.ToList();

                    if (this.RoutingLocations.Count > 0)
                    {
                        if (this.shipmentPM.Transshipment1FromPortId != null && this.shipmentPM.Transshipment1ToPortId != null)
                        {
                            this.HasIntermediatePort = true;
                        }

                        if (this.HasIntermediatePort == false)
                        {
                            this.ShipmentRoutingLegCode = "Main";
                            location_From = this.RoutingLocations.Where(d => d.LocationType == INTTRA_Status.LocationType1LocationType.PortOfLoading).FirstOrDefault();
                            location_To = this.RoutingLocations.Where(d => d.LocationType == INTTRA_Status.LocationType1LocationType.PortOfDischarge).FirstOrDefault();

                            if (location_From != null)
                            {
                                string CountryCode = location_From.LocationCountry;
                                string CombinedCode = location_From.LocationCode.Value;
                                string PortCode = CombinedCode.Substring(CountryCode.Length);

                                if (PortCode != this.shipmentPM.MainCarriageFromPortCode)
                                {
                                    throw new Exception("Invalid Port Of Loading");
                                }
                            }

                            if (location_To != null)
                            {
                                string CountryCode = location_To.LocationCountry;
                                string CombinedCode = location_To.LocationCode.Value;
                                string PortCode = CombinedCode.Substring(CountryCode.Length);

                                if (PortCode != this.shipmentPM.MainCarriageToPortCode)
                                {
                                    throw new Exception("Invalid Port Of Discharge");
                                }
                            }
                        }

                        else
                        {
                            #region                           
                            if (this.EventLocationDirection == "From")
                            {
                                if (EventLocationPortId == this.shipmentPM.MainCarriageFromPortId)
                                {
                                    this.ShipmentRoutingLegCode = "Main";
                                    this.GetRoutingLocations(this.shipmentPM.MainCarriageFromPortCode, this.shipmentPM.MainCarriageToPortCode);
                                }

                                else if (EventLocationPortId == this.shipmentPM.Transshipment1FromPortId)
                                {
                                    this.ShipmentRoutingLegCode = "TR1";
                                    this.GetRoutingLocations(this.shipmentPM.Transshipment1FromPortCode, this.shipmentPM.Transshipment1ToPortCode);
                                }

                                else if (EventLocationPortId == this.shipmentPM.Transshipment2FromPortId)
                                {
                                    this.ShipmentRoutingLegCode = "TR2";
                                    this.GetRoutingLocations(this.shipmentPM.Transshipment2FromPortCode, this.shipmentPM.Transshipment2ToPortCode);
                                }

                                else if (EventLocationPortId == this.shipmentPM.Transshipment3FromPortId)
                                {
                                    this.ShipmentRoutingLegCode = "TR3";
                                    this.GetRoutingLocations(this.shipmentPM.Transshipment3FromPortCode, this.shipmentPM.Transshipment3ToPortCode);
                                }
                            }

                            else
                            {
                                if (EventLocationPortId == this.shipmentPM.MainCarriageToPortId)
                                {
                                    this.ShipmentRoutingLegCode = "Main";
                                    this.GetRoutingLocations(this.shipmentPM.MainCarriageFromPortCode, this.shipmentPM.MainCarriageToPortCode);
                                }

                                else if (EventLocationPortId == this.shipmentPM.Transshipment1ToPortId)
                                {
                                    this.ShipmentRoutingLegCode = "TR1";
                                    this.GetRoutingLocations(this.shipmentPM.Transshipment1FromPortCode, this.shipmentPM.Transshipment1ToPortCode);
                                }

                                else if (EventLocationPortId == this.shipmentPM.Transshipment2ToPortId)
                                {
                                    this.ShipmentRoutingLegCode = "TR2";
                                    this.GetRoutingLocations(this.shipmentPM.Transshipment2FromPortCode, this.shipmentPM.Transshipment2ToPortCode);
                                }

                                else if (EventLocationPortId == this.shipmentPM.Transshipment3ToPortId)
                                {
                                    this.ShipmentRoutingLegCode = "TR3";
                                    this.GetRoutingLocations(this.shipmentPM.Transshipment3FromPortCode, this.shipmentPM.Transshipment3ToPortCode);
                                }
                            }

                            #endregion
                        }

                        if (location_From != null)
                        {
                            #region
                            INTTRA_Status.LocationType1 iLocation = location_From;
                            string CountryCode = iLocation.LocationCountry;
                            string CombinedCode = iLocation.LocationCode.Value;
                            DateTime? LocationsDate = this.GetDateFromString(iLocation);
                            string PortCode = CombinedCode.Substring(CountryCode.Length);

                            Port iPort = iPortRepository.GetOceanPortByCodeAndCountryCode(this.Tenant, PortCode, CountryCode, true);
                            if (iPort == null)
                            {
                                iPort = iPortRepository.GetOceanPortByCodeAndCountryCode(0, PortCode, CountryCode, true);
                                if (iPort != null)
                                {
                                    iPort = this.GetPortCopyToCurrentTenant(iPort.Id, this.Tenant);
                                }
                            }

                            if (iPort != null)
                            {
                                DeparturePortId = iPort.Id;
                            }

                            DeparturePortCode = PortCode;

                            if (location_From.DateTime != null)
                            {
                                if (location_From.DateTime.DateType == INTTRA_Status.DateTimeType2DateType.DepartureActual)
                                {
                                    DepartureDate = LocationsDate;
                                    DepartureDateIndicator = "A";
                                }

                                else if (location_From.DateTime.DateType == INTTRA_Status.DateTimeType2DateType.DepartureEstimated)
                                {
                                    DepartureDate = LocationsDate;
                                    DepartureDateIndicator = "E";
                                }
                            }
                            #endregion
                        }

                        if (location_To != null)
                        {
                            #region
                            INTTRA_Status.LocationType1 iLocation = location_To;
                            string CountryCode = iLocation.LocationCountry;
                            string CombinedCode = iLocation.LocationCode.Value;
                            DateTime? LocationsDate = this.GetDateFromString(iLocation);
                            string PortCode = CombinedCode.Substring(CountryCode.Length);

                            Port iPort = iPortRepository.GetOceanPortByCodeAndCountryCode(this.Tenant, PortCode, CountryCode, true);
                            if (iPort == null)
                            {
                                iPort = iPortRepository.GetOceanPortByCodeAndCountryCode(0, PortCode, CountryCode, true);
                                if (iPort != null)
                                {
                                    iPort = this.GetPortCopyToCurrentTenant(iPort.Id, this.Tenant);
                                }
                            }

                            if (iPort != null)
                            {
                                ArrivalPortId = iPort.Id;
                            }

                            ArrivalPortCode = PortCode;

                            if (location_To.DateTime != null)
                            {
                                if (location_To.DateTime.DateType == INTTRA_Status.DateTimeType2DateType.ArrivalActual)
                                {
                                    ArrivalDate = LocationsDate;
                                    ArrivalDateIndicator = "A";
                                }

                                else if (location_To.DateTime.DateType == INTTRA_Status.DateTimeType2DateType.ArrivalEstimated)
                                {
                                    ArrivalDate = LocationsDate;
                                    ArrivalDateIndicator = "E";
                                }
                            }
                            #endregion
                        }
                    }                   
                }
            }

        }
        private void GetRoutingLocations(string FromPortCode, string ToPortCode)
        {
            if (!string.IsNullOrEmpty(FromPortCode))
            {
                if (this.location_From == null)
                {
                    INTTRA_Status.LocationType1 ilocation = this.RoutingLocations.Where(d => d.LocationType == INTTRA_Status.LocationType1LocationType.PortOfLoading).FirstOrDefault();
                    if (ilocation != null)
                    {
                        string CountryCode = ilocation.LocationCountry;
                        string CombinedCode = ilocation.LocationCode.Value;
                        string iPortCode = CombinedCode.Substring(CountryCode.Length);

                        if (iPortCode == FromPortCode)
                        {
                            this.location_From = ilocation;
                        }
                    }
                }

                if (this.location_From == null)
                {
                    INTTRA_Status.LocationType1 ilocation = this.RoutingLocations.Where(d => d.LocationType == INTTRA_Status.LocationType1LocationType.IntermediatePort).FirstOrDefault();
                    if (ilocation != null)
                    {
                        string CountryCode = ilocation.LocationCountry;
                        string CombinedCode = ilocation.LocationCode.Value;
                        string iPortCode = CombinedCode.Substring(CountryCode.Length);

                        if (iPortCode == FromPortCode)
                        {
                            this.location_From = ilocation;
                        }
                    }
                }
            }

            if (!string.IsNullOrEmpty(ToPortCode))
            {
                if (this.location_To == null)
                {
                    INTTRA_Status.LocationType1 ilocation = this.RoutingLocations.Where(d => d.LocationType == INTTRA_Status.LocationType1LocationType.IntermediatePort).FirstOrDefault();
                    if (ilocation != null)
                    {
                        string CountryCode = ilocation.LocationCountry;
                        string CombinedCode = ilocation.LocationCode.Value;
                        string iPortCode = CombinedCode.Substring(CountryCode.Length);

                        if (iPortCode == ToPortCode)
                        {
                            this.location_To = ilocation;
                        }
                    }
                }

                if (this.location_To == null)
                {
                    INTTRA_Status.LocationType1 ilocation = this.RoutingLocations.Where(d => d.LocationType == INTTRA_Status.LocationType1LocationType.PortOfDischarge).FirstOrDefault();
                    if (ilocation != null)
                    {
                        string CountryCode = ilocation.LocationCountry;
                        string CombinedCode = ilocation.LocationCode.Value;
                        string iPortCode = CombinedCode.Substring(CountryCode.Length);

                        if (iPortCode == ToPortCode)
                        {
                            this.location_To = ilocation;
                        }
                    }
                }
            }               
        }

        private void UpdateShipmentRoutings()
        {
            switch (this.ShipmentRoutingLegCode)
            {
                case "TR1":
                    {
                        if (this.location_From != null)
                        {
                            if (this.location_From.DateTime != null)
                            {
                                switch (this.location_From.DateTime.DateType)
                                {
                                    case INTTRA_Status.DateTimeType2DateType.DepartureEstimated:
                                        {
                                            if (this.shipmentPM.Transshipment1ETD == null && this.shipmentPM.Transshipment1ATD == null)
                                            {
                                                this.shipmentPM.Transshipment1ETD = this.DepartureDate;
                                            }

                                            break;
                                        }

                                    case INTTRA_Status.DateTimeType2DateType.DepartureActual:
                                        {
                                            if (this.EventLocationCode == "VD")
                                            {
                                                if (this.shipmentPM.Transshipment1ATD == null)
                                                {
                                                    this.shipmentPM.Transshipment1ATD = EventLocationeDate;
                                                }
                                            }

                                            break;
                                        }
                                }
                            }
                        }

                        if (this.location_To != null)
                        {
                            if (this.location_To.DateTime != null)
                            {
                                switch (location_To.DateTime.DateType)
                                {
                                    case INTTRA_Status.DateTimeType2DateType.ArrivalEstimated:
                                        {
                                            if (this.shipmentPM.Transshipment1ETA == null && this.shipmentPM.Transshipment1ATA == null)
                                            {
                                                this.shipmentPM.Transshipment1ETA = this.ArrivalDate;
                                            }

                                            break;
                                        }

                                    case INTTRA_Status.DateTimeType2DateType.ArrivalActual:
                                        {
                                            if (this.EventLocationCode == "VA")
                                            {
                                                if (this.shipmentPM.Transshipment1ATA == null)
                                                {
                                                    this.shipmentPM.Transshipment1ATA = EventLocationeDate;
                                                }
                                            }

                                            break;
                                        }
                                }
                            }
                        }

                        break;
                    }

                case "TR2":
                    {
                        break;
                    }

                case "TR3":
                    {
                        break;
                    }

                default:
                    {
                        if (this.location_From != null)
                        {
                            if (this.location_From.DateTime != null)
                            {
                                switch (this.location_From.DateTime.DateType)
                                {
                                    case INTTRA_Status.DateTimeType2DateType.DepartureEstimated:
                                        {
                                            if (this.shipmentPM.MainCarriageETD == null && this.shipmentPM.MainCarriageATD == null)
                                            {
                                                this.shipmentPM.MainCarriageETD = this.DepartureDate;
                                            }

                                            break;
                                        }

                                    case INTTRA_Status.DateTimeType2DateType.DepartureActual:
                                        {
                                            if (this.EventLocationCode == "VD")
                                            {
                                                if (this.shipmentPM.MainCarriageATD == null)
                                                {
                                                    this.shipmentPM.MainCarriageATD = EventLocationeDate;
                                                }
                                            }

                                            break;
                                        }
                                }
                            }
                        }

                        if (this.location_To != null)
                        {
                            if (this.location_To.DateTime != null)
                            {
                                switch (location_To.DateTime.DateType)
                                {
                                    case INTTRA_Status.DateTimeType2DateType.ArrivalEstimated:
                                        {
                                            if (this.shipmentPM.MainCarriageETA == null && this.shipmentPM.MainCarriageATA == null)
                                            {
                                                this.shipmentPM.MainCarriageETA = this.ArrivalDate;
                                            }

                                            break;
                                        }

                                    case INTTRA_Status.DateTimeType2DateType.ArrivalActual:
                                        {
                                            if (this.EventLocationCode == "VA")
                                            {
                                                if (this.shipmentPM.MainCarriageATA == null)
                                                {
                                                    this.shipmentPM.MainCarriageATA = EventLocationeDate;
                                                }
                                            }

                                            break;
                                        }
                                }
                            }
                        }

                        break;
                    }
            }
        }
        private void UpdateLastStatus(string lastStatusCode, DateTime? lastStatusDate, DateTime iLogDate, ShipmentPackagePM iContainer)
        {
            string inttraSource = "INT";
            if (lastStatusDate == null)
            {
                lastStatusDate = iLogDate;
            }

            if (iContainer.LastStatusDate == null)
            {
                iContainer.LastStatusCode = lastStatusCode;
                iContainer.LastStatusDate = lastStatusDate;
                iContainer.ContainerStatusSourceCode = inttraSource;
                iContainer.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
            }

            else if (lastStatusDate > iContainer.LastStatusDate)
            {
                iContainer.LastStatusCode = lastStatusCode;
                iContainer.LastStatusDate = lastStatusDate;
                iContainer.ContainerStatusSourceCode = inttraSource;
                iContainer.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
            }
        }
        private void UpdateContainerFields()
        {
            if (this.DepartureDateIndicator == "E")
            {
                iContainer.ETD = this.DepartureDate;
            }

            if (this.ArrivalDateIndicator == "E")
            {
                iContainer.ETA = this.ArrivalDate;
            }

            iContainer.VoyageTripNumber = this.VoyageNumber;

            if (this.DeparturePortId != null && this.ArrivalPortId != null)
            {
                iContainer.RoutingIds = this.DeparturePortId + "," + this.ArrivalPortId;
            }

            if (this.DeparturePortCode != null && this.ArrivalPortCode != null)
            {
                iContainer.Routing = this.DeparturePortCode + " > " + this.ArrivalPortCode;
            }

            List<ShipmentPackagePM> AllContainers = this.shipmentPM.ShipmentPackages.ToList();
            if (AllContainers.Count > 1)
            {
                #region
                var AllVoyageNumber = (from a in AllContainers
                                       where a.VoyageTripNumber != null && a.Routing != null
                                       group a by new { a.VoyageTripNumber, a.Routing } into g
                                       select g.Key).ToList();

                if (AllVoyageNumber.Count > 1)
                {
                    string iRouting_Main = this.shipmentPM.MainCarriageFromPortId + "," + this.shipmentPM.MainCarriageToPortId;
                    string iRouting_Trs1 = this.shipmentPM.Transshipment1FromPortId + "," + this.shipmentPM.Transshipment1ToPortId;
                    string iRouting_Trs2 = this.shipmentPM.Transshipment2FromPortId + "," + this.shipmentPM.Transshipment2ToPortId;
                    string iRouting_Trs3 = this.shipmentPM.Transshipment3FromPortId + "," + this.shipmentPM.Transshipment3ToPortId;

                    foreach (ShipmentPackagePM item in AllContainers.Where(d => d.VoyageTripNumber != null && d.Routing != null))
                    {
                        bool iHasContainerException = false;

                        if (item.RoutingIds == iRouting_Main)
                        {
                            if (item.ETD != shipmentPM.MainCarriageETD || item.ETA != shipmentPM.MainCarriageETA)
                            {
                                iHasContainerException = true;
                            }
                        }

                        else if (item.RoutingIds == iRouting_Trs1)
                        {
                            if (item.ETD != shipmentPM.Transshipment1ETD || item.ETA != shipmentPM.Transshipment1ETA)
                            {
                                iHasContainerException = true;
                            }
                        }

                        else if (item.RoutingIds == iRouting_Trs2)
                        {
                            if (item.ETD != shipmentPM.Transshipment2ETD || item.ETA != shipmentPM.Transshipment2ETA)
                            {
                                iHasContainerException = true;
                            }
                        }

                        else if (item.RoutingIds == iRouting_Trs3)
                        {
                            if (item.ETD != shipmentPM.Transshipment3ETD || item.ETA != shipmentPM.Transshipment3ETA)
                            {
                                iHasContainerException = true;
                            }
                        }

                        if (item.HasContainerException != iHasContainerException)
                        {
                            item.HasContainerException = iHasContainerException;
                            item.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
                        }
                    }
                }

                else
                {
                    foreach (ShipmentPackagePM item in AllContainers.Where(d => d.VoyageTripNumber != null && d.Routing != null))
                    {
                        item.HasContainerException = false;
                        item.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
                    }
                }

                #endregion
            }

            if (AllContainers.Where(d => d.HasContainerException == true).Any())
            {
                shipmentPM.HasContainerException = true;
            }

            else
            {
                shipmentPM.HasContainerException = false;

                if (this.isUpdatingShipmentDates)
                {
                    var AllVoyageNumber = (from a in AllContainers
                                           where
                                           a.VoyageTripNumber != null
                                           && a.RoutingIds != null
                                           && a.ETD != null
                                           && a.ETA != null

                                           group a by new
                                           {
                                               a.VoyageTripNumber,
                                               a.RoutingIds,
                                               a.ETD,
                                               a.ETA
                                           } into g

                                           select g.Key).ToList();

                    if (AllVoyageNumber.Count == 1)
                    {
                        string iRouting_Main = this.shipmentPM.MainCarriageFromPortId + "," + this.shipmentPM.MainCarriageToPortId;
                        string iRouting_Trs1 = this.shipmentPM.Transshipment1FromPortId + "," + this.shipmentPM.Transshipment1ToPortId;
                        string iRouting_Trs2 = this.shipmentPM.Transshipment2FromPortId + "," + this.shipmentPM.Transshipment2ToPortId;
                        string iRouting_Trs3 = this.shipmentPM.Transshipment3FromPortId + "," + this.shipmentPM.Transshipment3ToPortId;

                        string iRoutingIds = AllVoyageNumber.FirstOrDefault().RoutingIds;
                        DateTime? iETD = AllVoyageNumber.FirstOrDefault().ETD;
                        DateTime? iETA = AllVoyageNumber.FirstOrDefault().ETA;

                        if (iRoutingIds == iRouting_Main)
                        {
                            this.shipmentPM.MainCarriageETD = iETD;
                            this.shipmentPM.MainCarriageETA = iETA;
                        }

                        else if (iRoutingIds == iRouting_Trs1)
                        {
                            this.shipmentPM.Transshipment1ETD = iETD;
                            this.shipmentPM.Transshipment1ETA = iETA;
                        }

                        else if (iRoutingIds == iRouting_Trs2)
                        {
                            this.shipmentPM.Transshipment2ETD = iETD;
                            this.shipmentPM.Transshipment2ETA = iETA;
                        }

                        else if (iRoutingIds == iRouting_Trs3)
                        {
                            this.shipmentPM.Transshipment3ETD = iETD;
                            this.shipmentPM.Transshipment3ETA = iETA;
                        }
                    }
                }
            }
        }
        private DateTime? GetDateFromString(INTTRA_Status.LocationType iLocation)
        {
            // 201804241920
            // 2018 04 24 19 20
            //0
            //4
            //6
            //8

            DateTime? myResult = null;

            if (iLocation != null)
            {
                if (iLocation.DateTime != null)
                {
                    string dateString = iLocation.DateTime.Value.ToString();

                    if (!string.IsNullOrEmpty(dateString))
                    {
                        if (dateString != "0")
                        {
                            int Year = Int32.Parse(dateString.Substring(0, 4));
                            int Month = Int32.Parse(dateString.Substring(4, 2));
                            int Day = Int32.Parse(dateString.Substring(6, 2));
                            int Hour = Int32.Parse(dateString.Substring(8, 2));
                            int Minut = Int32.Parse(dateString.Substring(10, 2));
                            myResult = new DateTime(Year, Month, Day, Hour, Minut, 0);
                        }
                    }
                }
            }

            return myResult;
        }
        private DateTime? GetDateFromString(INTTRA_Status.LocationType1 iLocation)
        {
            // 201804241920
            // 2018 04 24 19 20
            //0
            //4
            //6
            //8

            DateTime? myResult = null;

            if (iLocation != null)
            {
                if (iLocation.DateTime != null)
                {
                    string dateString = iLocation.DateTime.Value.ToString();

                    if (!string.IsNullOrEmpty(dateString))
                    {
                        if (dateString != "0")
                        {
                            int Year = Int32.Parse(dateString.Substring(0, 4));
                            int Month = Int32.Parse(dateString.Substring(4, 2));
                            int Day = Int32.Parse(dateString.Substring(6, 2));
                            int Hour = Int32.Parse(dateString.Substring(8, 2));
                            int Minut = Int32.Parse(dateString.Substring(10, 2));
                            myResult = new DateTime(Year, Month, Day, Hour, Minut, 0);
                        }
                    }
                }
            }

            return myResult;
        }
        private Port GetPortCopyToCurrentTenant(string entityId, int tenant)
        {
            Port port = iPortRepository.GetSinglePort(0, entityId);
            Port newPort = iPortRepository.GetSinglePortByCodeCountryCode(tenant, port.Code, port.Country.Code, false);

            if (newPort == null)
            {
                Country country = iCountryRepository.GetSingleCountryByCode(port.Country.Code, tenant, false);

                if (country == null)
                {
                    GlobalZone globalzone = iGlobalZoneRepository.GetSingleGlobalZoneByCode(port.Country.GlobalZone.Code, tenant);

                    if (globalzone == null)
                    {
                        GlobalZone oldZone = iGlobalZoneRepository.GetSingleGlobalZone(port.Country.GlobalZoneId, 0);
                        globalzone = new GlobalZone()
                        {
                            Id = IdCounter.GetNumber("GlobalZone", tenant).ToString(),
                            Code = oldZone.Code,
                            EnglishName = oldZone.EnglishName,
                            LocalName = oldZone.LocalName,
                            Notes = oldZone.Notes,
                            SearchFields = oldZone.SearchFields,
                            Tenant = tenant,
                        };

                        iGlobalZoneRepository.Add(globalzone);
                        iGlobalZoneRepository.SubmitChanges();
                    }

                    Country oldCountry = CountryRepository.GetSingleCountry(port.CountryId, 0, false);
                    country = new Country()
                    {
                        Id = IdCounter.GetNumber("Country", tenant).ToString(),
                        Tenant = tenant,
                        GlobalZoneId = oldCountry.GlobalZoneId,
                        EC = oldCountry.EC,
                        EnglishName = oldCountry.EnglishName,
                        Code = oldCountry.Code,
                        InActive = oldCountry.InActive,
                        Notes = oldCountry.Notes,
                        LocalName = oldCountry.LocalName,
                        SearchFields = oldCountry.SearchFields,
                    };

                    iCountryRepository.Add(country);
                    iCountryRepository.SubmitChanges();
                }

                newPort = new Port()
                {
                    Id = IdCounter.GetNumber("Port", tenant).ToString(),
                    Code = port.Code,
                    EnglishName = port.EnglishName,
                    LocalName = port.LocalName,
                    Tenant = tenant,
                    AddedManually = false,
                    InActive = false,
                    CountryId = country.Id,
                    IsAir = port.IsAir,
                    IsInland = port.IsInland,
                    IsOcean = port.IsOcean,
                    Latitude = port.Latitude,
                    Longtitude = port.Longtitude,
                    SearchFields = port.SearchFields,
                    Notes = port.Notes,
                };

                iPortRepository.Add(newPort);
                iPortRepository.SubmitChanges();
            }

            return newPort;
        }
        private string GetHashedData(string iShipmentId, string iShipmentNumber)
        {
            string information = iShipmentId + iShipmentNumber + this.Tenant.ToString() + this.DeparturePortId + this.ArrivalPortId + this.ContainerNumber + this.EventLocationCode + this.EventLocationDateString;
            byte[] byteRepresentation = UnicodeEncoding.UTF8.GetBytes(information);
            byte[] hashedTextInBytes = null;
            MD5CryptoServiceProvider myMd5 = new MD5CryptoServiceProvider();
            hashedTextInBytes = myMd5.ComputeHash(byteRepresentation);
            string hashedText = Convert.ToBase64String(hashedTextInBytes);

            return hashedText;
        }
        private string GetEventDirection(string StatusCode)
        {
            string myResult = null;

            switch (StatusCode)
            {
                case "2":
                case "3":
                case "AA":
                case "AC":
                case "AE":
                case "AF":
                case "AI":
                case "AW":
                case "B":
                case "BE":
                case "BF":
                case "BR":
                case "C":
                case "CA":
                case "CD":
                case "CO":
                case "CS":
                case "EE":
                case "EP":
                case "GI":
                case "I":
                case "VD":
                case "X3":
                case "X4":
                case "X7":
                case "X8":
                case "XA":
                    {
                        myResult = "From";
                        //myResult = "Origin";
                        break;
                    }

                case "A":
                case "A1":
                case "A2":
                case "A3":
                case "A4":
                case "AD":
                case "AG":
                case "AH":
                case "AJ":
                case "AL":
                case "AM":
                case "AN":
                case "AR":
                case "AV":
                case "BD":
                case "CR":
                case "CT":
                case "D":
                case "DN":
                case "E":
                case "ED":
                case "FT":
                case "OA":
                case "RD":
                case "UV":
                case "VA":
                case "X1":
                case "X2":
                case "X5":
                case "X6":
                case "X9":
                    {
                        myResult = "To";
                        //myResult = "Destination";
                        break;
                    }

                default:
                    {
                        myResult = "To";
                        //myResult = "Destination";
                        break;
                    }
            }

            return myResult;
        }
    }
}
