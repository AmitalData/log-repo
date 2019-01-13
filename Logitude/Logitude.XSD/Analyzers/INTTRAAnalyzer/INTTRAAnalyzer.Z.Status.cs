using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.BL.ShipmentsModel.Tools.EntityService;
using Logitude.Server.Tools.Counters;
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
        private void Analyze_Status()
        {
            if (this.iMessage_Status != null)
            {
                INTTRA_Status.MessageBodyType iMessageBody = iMessage_Status.MessageBody;

                if (iMessageBody != null)
                {
                    this.shipmentPM.IsUpdatedByINTTRAAnalyzer = true;
                    string systemEmail = "system@tenant" + this.Tenant + ".com";

                    string ContainerNumber = null;
                    ShipmentPackagePM iContainer = null;
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
                                    ContainerNumber = equipmentDetails.EquipmentIdentifier.Value;

                                    if (ContainerNumber != null)
                                    {
                                        ContainerNumber = ContainerNumber.Trim();
                                    }

                                    iContainer = this.shipmentPM.ShipmentPackages.Where(d => d.ContainerNumber == ContainerNumber).FirstOrDefault();

                                    if (iContainer != null)
                                    {
                                        isContainerExists_Shipment = true;
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
                        throw new Exception("There is no Container " + ContainerNumber + " for this Shipment");
                    }

                    else
                    {                        
                        INTTRA_Status.MessagePropertiesType iMessageProperties = iMessageBody.MessageProperties;

                        if (iMessageProperties != null)
                        {
                            string EventCode = iMessageProperties.EventCode;

                            #region EventLocation
                            string EventLocationPortId = null;
                            string EventLocationPortCode = null;
                            DateTime? EventLocationeDate = null;
                            string EventLocationDateString = "";
                            if (iMessageProperties.EventLocation != null)
                            {
                                if (iMessageProperties.EventLocation.Location != null)
                                {
                                    INTTRA_Status.LocationType location = iMessageProperties.EventLocation.Location;

                                    EventLocationeDate = this.GetDateFromString(location);

                                    if(EventLocationeDate != null)
                                    {
                                        EventLocationDateString = EventLocationeDate.ToString();
                                    }

                                    string CountryCode = location.LocationCountry;
                                    string CombinedCode = location.LocationCode.Value;
                                    string PortCode = CombinedCode.Substring(CountryCode.Length);

                                    Port iPort = iPortRepository.GetSinglePortByCode(this.Tenant, PortCode, true);
                                    if (iPort == null)
                                    {
                                        iPort = iPortRepository.GetPortsByNameOrCode(PortCode, null, 0).Where(a => a.IsOcean).FirstOrDefault();
                                        if (iPort != null)
                                        {
                                            iPort = this.GetPortCopyToCurrentTenant(iPort.Id, this.Tenant);
                                        }
                                    }

                                    if (iPort != null)
                                    {
                                        EventLocationPortId = iPort.Id;
                                    }

                                    EventLocationPortCode = PortCode;
                                }
                            }
                            #endregion

                            #region Ports
                            string DeparturePortId = null;
                            string DeparturePortCode = null;
                            DateTime? DepartureDate = null;
                            string DepartureDateIndicator = null;
                            string ArrivalPortId = null;
                            string ArrivalPortCode = null;
                            DateTime? ArrivalDate = null;
                            string ArrivalDateIndicator = null;
                            List<INTTRA_Status.LocationType1> locations = new List<INTTRA_Status.LocationType1>();
                            if (iMessageProperties.TransportationDetails != null)
                            {
                                if (iMessageProperties.TransportationDetails.Location != null)
                                {
                                    locations = iMessageProperties.TransportationDetails.Location.ToList();
                                    INTTRA_Status.LocationType1 location_From = locations.Where(d => d.LocationType == INTTRA_Status.LocationType1LocationType.PortOfLoading).FirstOrDefault();
                                    INTTRA_Status.LocationType1 location_To = locations.Where(d => d.LocationType == INTTRA_Status.LocationType1LocationType.PortOfDischarge).FirstOrDefault();

                                    if (location_From != null)
                                    {
                                        INTTRA_Status.LocationType1 iLocation = location_From;
                                        string CountryCode = iLocation.LocationCountry;
                                        string CombinedCode = iLocation.LocationCode.Value;
                                        DateTime? LocationsDate = this.GetDateFromString(iLocation);
                                        string PortCode = CombinedCode.Substring(CountryCode.Length);

                                        Port iPort = iPortRepository.GetSinglePortByCode(this.Tenant, PortCode, true);
                                        if (iPort == null)
                                        {
                                            iPort = iPortRepository.GetPortsByNameOrCode(PortCode, null, 0).Where(a => a.IsOcean).FirstOrDefault();
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
                                        DepartureDate = LocationsDate;

                                        if (location_From.DateTime.DateType == INTTRA_Status.DateTimeType2DateType.DepartureActual)
                                        {
                                            DepartureDateIndicator = "A";
                                        }

                                        else if (location_From.DateTime.DateType == INTTRA_Status.DateTimeType2DateType.DepartureEstimated)
                                        {
                                            DepartureDateIndicator = "E";
                                        }
                                    }

                                    if (location_To != null)
                                    {
                                        INTTRA_Status.LocationType1 iLocation = location_To;
                                        string CountryCode = iLocation.LocationCountry;
                                        string CombinedCode = iLocation.LocationCode.Value;
                                        DateTime? LocationsDate = this.GetDateFromString(iLocation);
                                        string PortCode = CombinedCode.Substring(CountryCode.Length);

                                        Port iPort = iPortRepository.GetSinglePortByCode(this.Tenant, PortCode, true);
                                        if (iPort == null)
                                        {
                                            iPort = iPortRepository.GetPortsByNameOrCode(PortCode, null, 0).Where(a => a.IsOcean).FirstOrDefault();
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
                                        ArrivalDate = LocationsDate;

                                        if (location_To.DateTime.DateType == INTTRA_Status.DateTimeType2DateType.ArrivalActual)
                                        {
                                            ArrivalDateIndicator = "A";
                                        }

                                        else if (location_To.DateTime.DateType == INTTRA_Status.DateTimeType2DateType.ArrivalEstimated)
                                        {
                                            ArrivalDateIndicator = "E";
                                        }
                                    }
                                }
                            }
                            #endregion

                            #region ShippingLine
                            string iVoyageNumber = null;
                            string ShippingLineName = shipmentPM.MainCarriageCarrierName;

                            if (iMessageProperties.TransportationDetails != null)
                            {
                                if (iMessageProperties.TransportationDetails.ConveyanceInformation != null)
                                {
                                    iVoyageNumber = iMessageProperties.TransportationDetails.ConveyanceInformation.VoyageTripNumber;
                                    string d = iMessageProperties.TransportationDetails.ConveyanceInformation.CarrierSCAC;
                                    string b = iMessageProperties.TransportationDetails.ConveyanceInformation.ConveyanceName;
                                    string f = iMessageProperties.TransportationDetails.ConveyanceInformation.TransportIdentification.Value;
                                }
                            }
                            #endregion

                            #region Build Status
                            string iDetails = EventCode;
                            DateTime iLogDate = TenantServerConfigration.GetCurrentDateTime(this.Tenant);

                            string iInfo = this.shipmentPM.Id + this.ShipmentNumber + this.Tenant.ToString() + DeparturePortId + ArrivalPortId + ContainerNumber + EventCode + EventLocationDateString;
                            string iHash = GetHashedData(iInfo);
                            if (!iShipmentContainerStatusRepository.DoesRecordExist(iHash))
                            {
                                this.UpdateShipmentRoutings(locations, EventCode, EventLocationeDate);
                                this.UpdateContainerFields(iContainer, iVoyageNumber, DeparturePortCode, ArrivalPortCode, DepartureDate, ArrivalDate, DepartureDateIndicator, ArrivalDateIndicator);

                                shipmentPM.INTTRALastStatusDate = iLogDate;
                                this.UpdateLastStatus(EventCode, EventLocationeDate, iLogDate, iContainer);
                                ShipmentService service = new ShipmentService(myShipmentContext, shipmentPM, systemEmail);
                                service.Update(true);

                                ShipmentContainerStatus iStatus = new ShipmentContainerStatus()
                                {
                                    Id = IdCounter.GetNumber("ShipmentContainerStatus", this.Tenant),
                                    ShipmentId = this.ShipmentId,
                                    ContainerId = iContainer.Id,
                                    RecordHash = iHash,
                                    Tenant = this.Tenant,
                                    StatusCode = EventCode,
                                    EventDate = EventLocationeDate,
                                    FromPortId = DeparturePortId,
                                    ToPortId = ArrivalPortId,
                                    DepartureDate = DepartureDate,
                                    ArrivalDate = ArrivalDate,
                                    Details = iDetails,
                                    ReceivingDate = iLogDate,
                                    VoyageNumber = iVoyageNumber,
                                    ShippingLineName = ShippingLineName,
                                    ContainerNumber = ContainerNumber,
                                    Location = EventLocationPortId,

                                    //Partial = statusParams.Partial,
                                    //Pieces = statusParams.Pieces,
                                    //Weight = d,
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
                                                iHouse.INTTRALastStatusDate = iLogDate;
                                                this.UpdateLastStatus(EventCode, EventLocationeDate, iLogDate, iHouseContainer);
                                                ShipmentService iHouseService = new ShipmentService(myShipmentContext, iHouse, systemEmail);
                                                iHouseService.Update(true);

                                                string iHouseInfo = iHouse.Id + iHouse.ShipmentNumber + this.Tenant.ToString() + DeparturePortId + ArrivalPortId + ContainerNumber + EventCode + EventLocationDateString;
                                                string iHouseHash = GetHashedData(iHouseInfo);
                                                if (!iShipmentContainerStatusRepository.DoesRecordExist(iHouseHash))
                                                {
                                                    ShipmentContainerStatus iHouseStatus = new ShipmentContainerStatus()
                                                    {
                                                        Id = IdCounter.GetNumber("ShipmentContainerStatus", this.Tenant),
                                                        ShipmentId = iHouse.Id,
                                                        ContainerId = iHouseContainer.Id,
                                                        RecordHash = iHouseHash,
                                                        Tenant = this.Tenant,
                                                        StatusCode = EventCode,
                                                        EventDate = EventLocationeDate,
                                                        FromPortId = DeparturePortId,
                                                        ToPortId = ArrivalPortId,
                                                        DepartureDate = DepartureDate,
                                                        ArrivalDate = ArrivalDate,
                                                        Details = iDetails,
                                                        ReceivingDate = iLogDate,
                                                        VoyageNumber = iVoyageNumber,
                                                        ShippingLineName = ShippingLineName,
                                                        ContainerNumber = ContainerNumber,
                                                        Location = EventLocationPortId,

                                                        //Partial = statusParams.Partial,
                                                        //Pieces = statusParams.Pieces,
                                                        //Weight = d,

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
                            #endregion
                        }
                    }
                }
            }
        }



        private void UpdateShipmentRoutings(List<INTTRA_Status.LocationType1> locations, string EventCode, DateTime? EventLocationeDate)
        {
            foreach (INTTRA_Status.LocationType1 iLocation in locations)
            {
                if (iLocation.DateTime != null)
                {
                    string CountryCode = iLocation.LocationCountry;
                    string CombinedCode = iLocation.LocationCode.Value;
                    string PortCode = CombinedCode.Substring(CountryCode.Length);
                    DateTime? LocationsDate = this.GetDateFromString(iLocation);

                    Port iPort = iPortRepository.GetSinglePortByCode(this.Tenant, PortCode, true);
                    if (iPort != null)
                    {
                        string PortId = iPort.Id;

                        switch (iLocation.LocationType)
                        {
                            case INTTRA_Status.LocationType1LocationType.PortOfLoading:
                                {
                                    if (PortId == this.shipmentPM.MainCarriageFromPortId)
                                    {
                                        switch (iLocation.DateTime.DateType)
                                        {
                                            case INTTRA_Status.DateTimeType2DateType.DepartureEstimated:
                                                {
                                                    if (this.shipmentPM.MainCarriageETD == null && this.shipmentPM.MainCarriageATD == null)
                                                    {
                                                        this.shipmentPM.MainCarriageETD = LocationsDate;
                                                    }

                                                    break;
                                                }

                                            case INTTRA_Status.DateTimeType2DateType.DepartureActual:
                                                {
                                                    //this.shipmentPM.MainCarriageATD = LocationsDate;

                                                    if (EventCode == "VD")
                                                    {
                                                        this.shipmentPM.MainCarriageATD = EventLocationeDate;
                                                    }

                                                    break;
                                                }
                                        }
                                    }

                                    break;
                                }

                            case INTTRA_Status.LocationType1LocationType.PortOfDischarge:
                                {
                                    if (PortId == this.shipmentPM.MainCarriageFinalDestinationPortId)
                                    {
                                        switch (iLocation.DateTime.DateType)
                                        {
                                            case INTTRA_Status.DateTimeType2DateType.ArrivalEstimated:
                                                {
                                                    if (this.shipmentPM.MainCarriageETA == null && this.shipmentPM.MainCarriageATA == null)
                                                    {
                                                        this.shipmentPM.MainCarriageETA = LocationsDate;
                                                    }

                                                    break;
                                                }

                                            case INTTRA_Status.DateTimeType2DateType.ArrivalActual:
                                                {
                                                    //this.shipmentPM.MainCarriageATA = LocationsDate;

                                                    if (EventCode == "VA")
                                                    {
                                                        this.shipmentPM.MainCarriageATA = EventLocationeDate;
                                                    }

                                                    break;
                                                }
                                        }
                                    }

                                    break;
                                }

                            case INTTRA_Status.LocationType1LocationType.PlaceOfReceipt:
                                {
                                    if (this.shipmentPM.PreCarriageFromPortId != null && this.shipmentPM.PreCarriageToPortId != null)
                                    {
                                        if (PortId == this.shipmentPM.PreCarriageFromPortId)
                                        {
                                            switch (iLocation.DateTime.DateType)
                                            {
                                                case INTTRA_Status.DateTimeType2DateType.DepartureEstimated:
                                                    {
                                                        if (this.shipmentPM.PreCarriageETD == null && this.shipmentPM.PreCarriageATD == null)
                                                        {
                                                            this.shipmentPM.PreCarriageETD = LocationsDate;
                                                        }

                                                        break;
                                                    }

                                                case INTTRA_Status.DateTimeType2DateType.DepartureActual:
                                                    {
                                                        //this.shipmentPM.PreCarriageATD = LocationsDate;

                                                        if (EventCode == "VD")
                                                        {
                                                            this.shipmentPM.PreCarriageATD = EventLocationeDate;
                                                        }

                                                        break;
                                                    }
                                            }
                                        }
                                    }

                                    break;
                                }

                            case INTTRA_Status.LocationType1LocationType.PlaceOfDelivery:
                                {
                                    if (this.shipmentPM.OnCarriageFromPortId != null && this.shipmentPM.OnCarriageToPortId != null)
                                    {
                                        if (PortId == this.shipmentPM.PreCarriageToPortId)
                                        {
                                            switch (iLocation.DateTime.DateType)
                                            {
                                                case INTTRA_Status.DateTimeType2DateType.ArrivalEstimated:
                                                    {
                                                        if (this.shipmentPM.OnCarriageETA == null && this.shipmentPM.OnCarriageATA == null)
                                                        {
                                                            this.shipmentPM.OnCarriageETA = LocationsDate;
                                                        }

                                                        break;
                                                    }

                                                case INTTRA_Status.DateTimeType2DateType.ArrivalActual:
                                                    {
                                                        //this.shipmentPM.OnCarriageATA = LocationsDate;

                                                        if (EventCode == "VA")
                                                        {
                                                            this.shipmentPM.OnCarriageATA = EventLocationeDate;
                                                        }

                                                        break;
                                                    }
                                            }
                                        }
                                    }

                                    break;
                                }

                            case INTTRA_Status.LocationType1LocationType.IntermediatePort:
                                {
                                    switch (iLocation.DateTime.DateType)
                                    {
                                        case INTTRA_Status.DateTimeType2DateType.DepartureActual:
                                        case INTTRA_Status.DateTimeType2DateType.DepartureEstimated:
                                            {
                                                if (this.shipmentPM.PreCarriageFromPortId != null && this.shipmentPM.PreCarriageToPortId != null)
                                                {
                                                    if (PortId == this.shipmentPM.PreCarriageFromPortId)
                                                    {
                                                        switch (iLocation.DateTime.DateType)
                                                        {
                                                            case INTTRA_Status.DateTimeType2DateType.DepartureEstimated:
                                                                {
                                                                    if (this.shipmentPM.PreCarriageETD == null && this.shipmentPM.PreCarriageATD == null)
                                                                    {
                                                                        this.shipmentPM.PreCarriageETD = LocationsDate;
                                                                    }

                                                                    break;
                                                                }

                                                            case INTTRA_Status.DateTimeType2DateType.DepartureActual:
                                                                {
                                                                    //this.shipmentPM.PreCarriageATD = LocationsDate;

                                                                    if (EventCode == "VD")
                                                                    {
                                                                        this.shipmentPM.PreCarriageATD = EventLocationeDate;
                                                                    }

                                                                    break;
                                                                }
                                                        }
                                                    }
                                                }

                                                if (PortId == this.shipmentPM.MainCarriageFromPortId)
                                                {
                                                    switch (iLocation.DateTime.DateType)
                                                    {
                                                        case INTTRA_Status.DateTimeType2DateType.DepartureEstimated:
                                                            {
                                                                if (this.shipmentPM.MainCarriageETD == null && this.shipmentPM.MainCarriageATD == null)
                                                                {
                                                                    this.shipmentPM.MainCarriageETD = LocationsDate;
                                                                }

                                                                break;
                                                            }

                                                        case INTTRA_Status.DateTimeType2DateType.DepartureActual:
                                                            {
                                                                //this.shipmentPM.MainCarriageATD = LocationsDate;

                                                                if (EventCode == "VD")
                                                                {
                                                                    this.shipmentPM.MainCarriageATD = EventLocationeDate;
                                                                }

                                                                break;
                                                            }  
                                                    }
                                                }

                                                break;
                                            }

                                        case INTTRA_Status.DateTimeType2DateType.ArrivalActual:
                                        case INTTRA_Status.DateTimeType2DateType.ArrivalEstimated:
                                            {
                                                if (this.shipmentPM.OnCarriageFromPortId != null && this.shipmentPM.OnCarriageToPortId != null)
                                                {
                                                    if (PortId == this.shipmentPM.PreCarriageToPortId)
                                                    {
                                                        switch (iLocation.DateTime.DateType)
                                                        {
                                                            case INTTRA_Status.DateTimeType2DateType.ArrivalEstimated:
                                                                {
                                                                    if (this.shipmentPM.OnCarriageETA == null && this.shipmentPM.OnCarriageATA == null)
                                                                    {
                                                                        this.shipmentPM.OnCarriageETA = LocationsDate;
                                                                    }

                                                                    break;
                                                                }

                                                            case INTTRA_Status.DateTimeType2DateType.ArrivalActual:
                                                                {
                                                                    //this.shipmentPM.OnCarriageATA = LocationsDate;

                                                                    if (EventCode == "VA")
                                                                    {
                                                                        this.shipmentPM.OnCarriageATA = EventLocationeDate;
                                                                    }

                                                                    break;
                                                                }          
                                                        }
                                                    }
                                                }

                                                if (PortId == this.shipmentPM.MainCarriageFinalDestinationPortId)
                                                {
                                                    switch (iLocation.DateTime.DateType)
                                                    {
                                                        case INTTRA_Status.DateTimeType2DateType.ArrivalEstimated:
                                                            {
                                                                if (this.shipmentPM.MainCarriageETA == null && this.shipmentPM.MainCarriageATA == null)
                                                                {
                                                                    this.shipmentPM.MainCarriageETA = LocationsDate;
                                                                }

                                                                break;
                                                            }

                                                        case INTTRA_Status.DateTimeType2DateType.ArrivalActual:
                                                            {
                                                                //this.shipmentPM.MainCarriageATA = LocationsDate;

                                                                if (EventCode == "VA")
                                                                {
                                                                    this.shipmentPM.MainCarriageATA = EventLocationeDate;
                                                                }

                                                                break;
                                                            }                                                 
                                                    }
                                                }

                                                break;
                                            }
                                    }

                                    break;
                                }
                        }
                    }
                }
            }
        }
        private void UpdateLastStatus(string lastStatusCode, DateTime? lastStatusDate, DateTime iLogDate, ShipmentPackagePM iContainer)
        {
            if (lastStatusDate == null)
            {
                lastStatusDate = iLogDate;
            }

            if (iContainer.LastStatusDate == null)
            {
                iContainer.LastStatusCode = lastStatusCode;
                iContainer.LastStatusDate = lastStatusDate;
                iContainer.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
            }

            else if (lastStatusDate > iContainer.LastStatusDate)
            {
                iContainer.LastStatusCode = lastStatusCode;
                iContainer.LastStatusDate = lastStatusDate;
                iContainer.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
            }
        }
        private void UpdateContainerFields(ShipmentPackagePM iContainer, string iVoyageNumber, string departurePortCode, string arrivalPortCode, DateTime? departureDate, DateTime? arrivalDate, string departureDateIndicator, string arrivalDateIndicator)
        {
            iContainer.VoyageTripNumber = iVoyageNumber;

            if (departurePortCode != null)
            {
                departurePortCode = "";
            }
            if (arrivalPortCode != null)
            {
                arrivalPortCode = "";
            }

            iContainer.Routing = departurePortCode + " > " + arrivalPortCode;

            if (departureDateIndicator == "E")
            {
                iContainer.ETD = departureDate;
            }

            if (arrivalDateIndicator == "E")
            {
                iContainer.ETA = arrivalDate;
            }

            iContainer.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
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
        private string GetHashedData(string information)
        {
            byte[] byteRepresentation = UnicodeEncoding.UTF8.GetBytes(information);
            byte[] hashedTextInBytes = null;
            MD5CryptoServiceProvider myMd5 = new MD5CryptoServiceProvider();
            hashedTextInBytes = myMd5.ComputeHash(byteRepresentation);
            string hashedText = Convert.ToBase64String(hashedTextInBytes);

            return hashedText;
        }
    }
}
