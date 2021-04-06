using CHAMP17;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.Tools.EntityService;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.XSD.Analyzers.CHAMPAnalyzer
{
    public partial class CHAMPAnalyzer
    {
        private ShipmentPM ShipmentPM;
        private void AnalyzeMessageQueue_FSA_Shipment(ShipmentPM entityPM)
        {
            this.ShipmentPM = entityPM;
            this.ShipmentPM.IsUpdatedByChampAnalyzer = true;
            this.ShipmentPM.IsFSRSent = false;

            if (this.myPortRepository == null)
            {
                this.myPortRepository = new PortRepository(iCommonContext);
            }

            this.AnalyzeMasterConsignment();
            this.AnalyzeOtherServiceInformation();
            this.AnalyzeStatus();

            string systemEmail = "system@tenant" + myTenant + ".com";
            ShipmentService service = new ShipmentService(this.iShipmentsContext, this.ShipmentPM, systemEmail);
            service.Update();
        }
        private void AnalyzeMasterConsignment()
        {
            if (mySplitConsignment.MasterAWBConsignmentDetail != null)
            {
                #region AWBOriginAndDestination
                if (mySplitConsignment.MasterAWBConsignmentDetail.AWBOriginAndDestination != null)
                {
                    string CodeOfOrigin = mySplitConsignment.MasterAWBConsignmentDetail.AWBOriginAndDestination.AirportCityCodeOfOrigin;
                    string CodeOfDestination = mySplitConsignment.MasterAWBConsignmentDetail.AWBOriginAndDestination.AirportCityCodeOfDestination;

                    if (this.ShipmentPM.MainCarriageFromPortCode == "---")
                    {
                        Port newFromPort = this.myPortRepository.GetPortsByNameOrCode(CodeOfOrigin, null, myTenant).Where(d => d.IsAir).FirstOrDefault();
                        if (newFromPort == null)
                        {
                            Port portZero = this.myPortRepository.GetPortsByNameOrCode(CodeOfOrigin, null, 0).Where(d => d.IsAir).FirstOrDefault();
                            if (portZero != null)
                            {
                                newFromPort = this.GetPortCopyToCurrentTenant(portZero.Id, myTenant);
                            }
                        }

                        if (newFromPort != null)
                        {
                            this.ShipmentPM.MainCarriageFromPortId = newFromPort.Id;
                            //entityPM.IsMappingXSDFields = false;
                        }
                    }

                    if (this.ShipmentPM.MainCarriageFinalDestinationPortCode == "---")
                    {
                        string OldPortId = this.ShipmentPM.MainCarriageFinalDestinationPortId;

                        Port newToPort = this.myPortRepository.GetPortsByNameOrCode(CodeOfDestination, null, myTenant).Where(d => d.IsAir).FirstOrDefault();
                        if (newToPort == null)
                        {
                            Port portZero = this.myPortRepository.GetPortsByNameOrCode(CodeOfDestination, null, 0).Where(d => d.IsAir).FirstOrDefault();
                            if (portZero != null)
                            {
                                newToPort = this.GetPortCopyToCurrentTenant(portZero.Id, myTenant);
                            }
                        }

                        if (newToPort != null)
                        {
                            this.ShipmentPM.MainCarriageFinalDestinationPortId = newToPort.Id;
                            //entityPM.IsMappingXSDFields = false;

                            if (OldPortId == this.ShipmentPM.Transshipment3ToPortId)
                            {
                                this.ShipmentPM.Transshipment3ToPortId = newToPort.Id;
                            }

                            else if (OldPortId == this.ShipmentPM.Transshipment2ToPortId)
                            {
                                this.ShipmentPM.Transshipment2ToPortId = newToPort.Id;
                            }

                            else if (OldPortId == this.ShipmentPM.Transshipment1ToPortId)
                            {
                                this.ShipmentPM.Transshipment1ToPortId = newToPort.Id;
                            }

                            else if (OldPortId == this.ShipmentPM.MainCarriageToPortId)
                            {
                                this.ShipmentPM.MainCarriageToPortId = newToPort.Id;
                            }
                        }
                    }
                }
                #endregion

                #region QuantityDetail
                if (mySplitConsignment.MasterAWBConsignmentDetail.QuantityDetail != null)
                {
                    if (this.ShipmentPM.NumberOfPackages == 0 || this.ShipmentPM.NumberOfPackages == null)
                    {
                        this.ShipmentPM.NumberOfPackages = mySplitConsignment.MasterAWBConsignmentDetail.QuantityDetail.NumberOfPieces;
                        //entityPM.IsMappingXSDFields = false;
                    }

                    if (this.ShipmentPM.GrossWeight == 0 || this.ShipmentPM.GrossWeight == null)
                    {
                        string weightCode = mySplitConsignment.MasterAWBConsignmentDetail.QuantityDetail.WeightCode;
                        this.ShipmentPM.GrossWeight = (double)mySplitConsignment.MasterAWBConsignmentDetail.QuantityDetail.Weight;
                        this.ShipmentPM.GrossWeightUnitCode = weightCode == "L" ? "LB" : "KG";

                        if (weightCode == "L")
                        {
                            this.ShipmentPM.GrossWeightInKG = this.ShipmentPM.GrossWeight / 2.20462262;
                        }

                        else
                        {
                            this.ShipmentPM.GrossWeightInKG = this.ShipmentPM.GrossWeight;
                        }

                        this.ShipmentPM.ChargeableWeight = MethodHelper.CalculateChargeableWeight(this.ShipmentPM.GrossWeight, this.ShipmentPM.VolumetricWeight, this.ShipmentPM.GrossWeightUnitCode, this.ShipmentPM.ChargeableWeightUnitCode, this.ShipmentPM.DirectionId, this.ShipmentPM.TransportModeId);
                        //entityPM.IsMappingXSDFields = false;
                    }
                }
                #endregion
            }
        }
        private void AnalyzeOtherServiceInformation()
        {
            if (mySplitConsignment.OtherServiceInformation != null)
            {
                string osi = mySplitConsignment.OtherServiceInformation.OSIDetailsFirstLine + Environment.NewLine + mySplitConsignment.OtherServiceInformation.OSIDetailsSecondLine;

                if (!string.IsNullOrEmpty(osi))
                {
                    this.CreateShipmentStatus(new StatusParams()
                    {
                        Tenant = myTenant,
                        Details = osi,                        
                        StatusCode = "OSI",
                        EntityId = this.ShipmentPM.Id,
                        LogDate = TenantServerConfigration.GetCurrentDateTime(myTenant),
                    });

                    if (string.IsNullOrEmpty(this.ShipmentPM.CarrierLastStatusCode))
                    {
                        this.ShipmentPM.CarrierLastStatusCode = "OSI";
                        this.ShipmentPM.CarrierLastStatusDate = TenantServerConfigration.GetCurrentDateTime(myTenant);
                    }
                }
            }
        }

        private void AnalyzeStatus()
        {
            if (mySplitConsignment.StatusDetail != null)
            {
                foreach (CHAMP17.StatusDetail item in mySplitConsignment.StatusDetail)
                {
                    if (!string.IsNullOrEmpty(item.StatusCode))
                    {
                        StatusParams iStatusArgs = new StatusParams()
                        {
                            Tenant = this.myTenant,
                            Details = item.StatusCode,
                            StatusCode = item.StatusCode,
                            EntityId = this.ShipmentPM.Id,
                            LogDate = TenantServerConfigration.GetCurrentDateTime(myTenant),
                        };

                        this.AnalyzeStatus_Movement(item.MovementDetail, iStatusArgs);
                        this.AnalyzeStatus_Quantity(item.QuantityDetail, iStatusArgs);
                        this.AnalyzeStatus_Departure(item.TimeOfDepartureInfo, iStatusArgs);
                        this.AnalyzeStatus_Arrival(item.TimeOfArrivalInfo, iStatusArgs);

                        this.UpdateShipment_LastStatus(iStatusArgs);
                        this.BuildTraceEvent_LastStatus(iStatusArgs);

                        switch (item.StatusCode)
                        {
                            case "DIS":
                                {
                                    iStatusArgs.Details = this.GetStatusDetails(item.StatusCode, item.DiscrepancyCode);
                                    break;
                                }

                            case "DEP":
                                {
                                    this.UpdateShipment_DepartureDates(iStatusArgs);
                                    break;
                                }

                            case "ARR":
                                {
                                    this.UpdateShipment_ArrivalDates(iStatusArgs);
                                    break;
                                }
                        }

                        this.CreateShipmentStatus(iStatusArgs);
                    }
                }
            }
        }
        private void AnalyzeStatus_Movement(CHAMP17.MovementDetail item, StatusParams iStatusArgs)
        {
            if (item != null)
            {
                iStatusArgs.FlightNumber = item.FlightNumber;
                iStatusArgs.LocationPortCode = this.GetMovementLocation(iStatusArgs.StatusCode, item);
                iStatusArgs.FromPortCode = item.AirportCityCodeOfDeparture;
                iStatusArgs.ToPortCode = item.AirportCityCodeOfArrival;

                if (iStatusArgs.StatusCode == "DEP")
                {
                    if (iStatusArgs.LocationPortCode == null)
                    {
                        iStatusArgs.LocationPortCode = item.AirportCityCodeOfArrival;
                    }
                }

                else if (iStatusArgs.StatusCode == "ARR")
                {
                    if (iStatusArgs.LocationPortCode == null)
                    {
                        iStatusArgs.LocationPortCode = item.AirportCityCodeOfDeparture;
                    }

                    if (iStatusArgs.ToPortCode == null)
                    {
                        iStatusArgs.ToPortCode = item.AirportCityCodeOfDeparture;
                    }
                }

                int day = item.Day;
                int month = !string.IsNullOrEmpty(item.Month) ? GetMonthInNumbers(item.Month) : 0;
                int hour = item.ActualTime / 100;
                int minute = item.ActualTime % 100;

                DateTime? currentDate = this.ShipmentPM.MAWBOBLDate != null ? this.ShipmentPM.MAWBOBLDate : this.ShipmentPM.CreateDateTime;
                int currentMonth = currentDate.Value.Month;
                int currentYear = currentDate.Value.Year;
                int difference = Math.Abs(currentMonth - month);

                int year = 0;
                if (difference > 6)
                {
                    if (month > currentMonth)
                    {
                        year = currentYear - 1;
                    }

                    else
                    {
                        year = currentYear + 1;
                    }
                }

                else
                {
                    year = currentYear;
                }

                if (day != 0 && month != 0)
                {
                    iStatusArgs.EventDate = new DateTime(year, month, day, hour, minute, 0);
                }

                if (iStatusArgs.StatusCode != null)
                {
                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = myTenant,
                        EventTypeCode = "FSAR",
                        UserId = null,
                        EntityId = this.ShipmentPM.Id,
                        ObjectTableName = "Shipment",
                        Notes = iStatusArgs.StatusCode + " status received" + Environment.NewLine + "Updated by FSA/FSU",
                    });
                }
            }
        }
        private void AnalyzeStatus_Quantity(CHAMP17.QuantityDetail item, StatusParams iStatusArgs)
        {
            if (item != null)
            {
                if (item.ShipmentDescriptionCode == "T")
                {
                    iStatusArgs.Partial = false;
                }

                iStatusArgs.Weight = item.Weight;
                iStatusArgs.Pieces = item.NumberOfPieces;
            }
        }
        private void AnalyzeStatus_Departure(CHAMP17.TimeOfDepartureInfo item, StatusParams iStatusArgs)
        {
            if (item != null)
            {
                int departureHour = item.Time / 100;
                int departureMinute = item.Time % 100;
                int dayIndicator = GetDayChangeIndicator(item.DayChangeIndicator);
                DateTime currentdate = iStatusArgs.EventDate != null ? iStatusArgs.EventDate.Value.AddDays(dayIndicator) : iStatusArgs.LogDate.AddDays(dayIndicator);

                iStatusArgs.DepartureDate = new DateTime(currentdate.Year, currentdate.Month, currentdate.Day, departureHour, departureMinute, 0);
                iStatusArgs.TimeOfDepartureInfo = item.TypeOfTimeIndicator;
            }
        }
        private void AnalyzeStatus_Arrival(CHAMP17.TimeOfArrivalInfo item, StatusParams iStatusArgs)
        {
            if (item != null)
            {
                int arrivalHour = item.Time / 100;
                int arrivalMinute = item.Time % 100;
                int dayIndicator = GetDayChangeIndicator(item.DayChangeIndicator);
                DateTime currentdate = iStatusArgs.EventDate != null ? iStatusArgs.EventDate.Value.AddDays(dayIndicator) : iStatusArgs.LogDate.AddDays(dayIndicator);

                iStatusArgs.ArrivalDate = new DateTime(currentdate.Year, currentdate.Month, currentdate.Day, arrivalHour, arrivalMinute, 0);
                iStatusArgs.TimeOfArrivalInfo = item.TypeOfTimeIndicator;
            }
        }

        private void UpdateShipment_LastStatus(StatusParams iStatusArgs)
        {
            string currentCarrierStatus = this.ShipmentPM.CarrierLastStatusCode;
            DateTime? CurrentCarrierLastStatusDate = this.ShipmentPM.CarrierLastStatusDate;

            DateTime carrierLastStatusDate = iStatusArgs.EventDate != null ? iStatusArgs.EventDate.Value : iStatusArgs.LogDate;

            if (CurrentCarrierLastStatusDate == null)
            {
                CurrentCarrierLastStatusDate = carrierLastStatusDate;
                currentCarrierStatus = iStatusArgs.StatusCode;
            }

            else
            {
                if (carrierLastStatusDate.Year > CurrentCarrierLastStatusDate.Value.Year)
                {
                    CurrentCarrierLastStatusDate = carrierLastStatusDate;
                    currentCarrierStatus = iStatusArgs.StatusCode;
                }
                else if (carrierLastStatusDate.Year == CurrentCarrierLastStatusDate.Value.Year)
                {
                    if (carrierLastStatusDate.Month > CurrentCarrierLastStatusDate.Value.Month)
                    {
                        CurrentCarrierLastStatusDate = carrierLastStatusDate;
                        currentCarrierStatus = iStatusArgs.StatusCode;
                    }
                    else if (carrierLastStatusDate.Month == CurrentCarrierLastStatusDate.Value.Month)
                    {
                        if (carrierLastStatusDate.Day > CurrentCarrierLastStatusDate.Value.Day)
                        {
                            CurrentCarrierLastStatusDate = carrierLastStatusDate;
                            currentCarrierStatus = iStatusArgs.StatusCode;
                        }
                        else if (carrierLastStatusDate.Day == CurrentCarrierLastStatusDate.Value.Day)
                        {
                            if (carrierLastStatusDate.Hour > CurrentCarrierLastStatusDate.Value.Hour)
                            {
                                CurrentCarrierLastStatusDate = carrierLastStatusDate;
                                currentCarrierStatus = iStatusArgs.StatusCode;
                            }
                            else if (carrierLastStatusDate.Hour == CurrentCarrierLastStatusDate.Value.Hour)
                            {
                                if (carrierLastStatusDate.Minute > CurrentCarrierLastStatusDate.Value.Minute)
                                {
                                    CurrentCarrierLastStatusDate = carrierLastStatusDate;
                                    currentCarrierStatus = iStatusArgs.StatusCode;
                                }
                            }
                        }
                    }
                }
            }

            this.ShipmentPM.CarrierLastStatusCode = currentCarrierStatus;
            this.ShipmentPM.CarrierLastStatusDate = CurrentCarrierLastStatusDate;
        }
        private void UpdateShipment_DepartureDates(StatusParams iStatusArgs)
        {
            if (iStatusArgs.DepartureDate != null)
            {
                if (!string.IsNullOrEmpty(iStatusArgs.FromPortCode))
                {
                    string myPortCode = iStatusArgs.FromPortCode;
                    DateTime? myEventDate = iStatusArgs.EventDate;
                    DateTime? myDateTime = iStatusArgs.DepartureDate;
                    string myTimeType = iStatusArgs.TimeOfDepartureInfo;

                    if (this.ShipmentPM.MainCarriageFromPortCode == myPortCode)
                    {
                        if (!string.IsNullOrEmpty(myTimeType))
                        {
                            switch (myTimeType)
                            {
                                case "A": { this.ShipmentPM.MainCarriageATD = myDateTime; break; }
                                case "E": { this.ShipmentPM.MainCarriageETD = myDateTime; this.ShipmentPM.MainCarriageSTD = myDateTime; break; }
                                case "S": { this.ShipmentPM.MainCarriageSTD = myDateTime; break; }
                            }
                        }

                        else
                        {
                            this.ShipmentPM.MainCarriageATD = myEventDate;
                        }
                    }

                    else if (this.ShipmentPM.Transshipment1FromPortCode == myPortCode)
                    {
                        if (!string.IsNullOrEmpty(myTimeType))
                        {
                            switch (myTimeType)
                            {
                                case "A": { this.ShipmentPM.Transshipment1ATD = myDateTime; break; }
                                case "E": { this.ShipmentPM.Transshipment1ETD = myDateTime; this.ShipmentPM.Transshipment1STD = myDateTime; break; }
                                case "S": { this.ShipmentPM.Transshipment1STD = myDateTime; break; }
                            }
                        }
                        else
                        {
                            this.ShipmentPM.Transshipment1ATD = myEventDate;
                        }

                    }

                    else if (this.ShipmentPM.Transshipment2FromPortCode == myPortCode)
                    {
                        if (!string.IsNullOrEmpty(myTimeType))
                        {
                            switch (myTimeType)
                            {
                                case "A": { this.ShipmentPM.Transshipment2ATD = myDateTime; break; }
                                case "E": { this.ShipmentPM.Transshipment2ETD = myDateTime; this.ShipmentPM.Transshipment2STD = myDateTime; break; }
                                case "S": { this.ShipmentPM.Transshipment2STD = myDateTime; break; }
                            }
                        }

                        else
                        {
                            this.ShipmentPM.Transshipment2ATD = myEventDate;
                        }
                    }

                    else if (this.ShipmentPM.Transshipment3FromPortCode == myPortCode)
                    {
                        if (!string.IsNullOrEmpty(myTimeType))
                        {
                            switch (myTimeType)
                            {
                                case "A": { this.ShipmentPM.Transshipment3ATD = myDateTime; break; }
                                case "E": { this.ShipmentPM.Transshipment3ETD = myDateTime; this.ShipmentPM.Transshipment3STD = myDateTime; break; }
                                case "S": { this.ShipmentPM.Transshipment3STD = myDateTime; break; }
                            }
                        }

                        else
                        {
                            this.ShipmentPM.Transshipment3ATD = myEventDate;
                        }
                    }

                    else if (this.ShipmentPM.PreCarriageFromPortCode == myPortCode)
                    {
                        if (!string.IsNullOrEmpty(myTimeType))
                        {
                            switch (myTimeType)
                            {
                                case "A": { this.ShipmentPM.PreCarriageATD = myDateTime; break; }
                                case "E": { this.ShipmentPM.PreCarriageETD = myDateTime; break; }
                            }
                        }

                        else
                        {
                            this.ShipmentPM.PreCarriageATD = myEventDate;
                        }
                    }

                    else if (this.ShipmentPM.OnCarriageFromPortCode == myPortCode)
                    {
                        if (!string.IsNullOrEmpty(myTimeType))
                        {
                            switch (myTimeType)
                            {
                                case "A": { this.ShipmentPM.OnCarriageATD = myDateTime; break; }
                                case "E": { this.ShipmentPM.OnCarriageETD = myDateTime; break; }
                            }
                        }

                        else
                        {
                            this.ShipmentPM.OnCarriageATD = myEventDate;
                        }
                    }
                }
            }
        }
        private void UpdateShipment_ArrivalDates(StatusParams iStatusArgs)
        {
            if (iStatusArgs.ArrivalDate != null)
            {
                if (!string.IsNullOrEmpty(iStatusArgs.ToPortCode))
                {
                    string myPortCode = iStatusArgs.ToPortCode;
                    DateTime? myEventDate = iStatusArgs.EventDate;
                    DateTime? myDateTime = iStatusArgs.ArrivalDate;
                    string myTimeType = iStatusArgs.TimeOfArrivalInfo;

                    if (this.ShipmentPM.MainCarriageToPortCode == myPortCode)
                    {
                        if (!string.IsNullOrEmpty(myTimeType))
                        {
                            switch (myTimeType)
                            {
                                case "A": { this.ShipmentPM.MainCarriageATA = myDateTime; break; }
                                case "E": { this.ShipmentPM.MainCarriageETA = myDateTime; this.ShipmentPM.MainCarriageSTA = myDateTime; break; }
                                case "S": { this.ShipmentPM.MainCarriageSTA = myDateTime; break; }
                            }
                        }

                        else
                        {
                            this.ShipmentPM.MainCarriageATA = myEventDate;
                        }
                    }

                    else if (this.ShipmentPM.Transshipment1ToPortCode == myPortCode)
                    {
                        if (!string.IsNullOrEmpty(myTimeType))
                        {
                            switch (myTimeType)
                            {
                                case "A": { this.ShipmentPM.Transshipment1ATA = myDateTime; break; }
                                case "E": { this.ShipmentPM.Transshipment1ETA = myDateTime; this.ShipmentPM.Transshipment1STA = myDateTime; break; }
                                case "S": { this.ShipmentPM.Transshipment1STA = myDateTime; break; }
                            }
                        }

                        else
                        {
                            this.ShipmentPM.Transshipment1ATA = myEventDate;
                        }
                    }

                    else if (this.ShipmentPM.Transshipment2ToPortCode == myPortCode)
                    {
                        if (!string.IsNullOrEmpty(myTimeType))
                        {
                            switch (myTimeType)
                            {
                                case "A": { this.ShipmentPM.Transshipment2ATA = myDateTime; break; }
                                case "E": { this.ShipmentPM.Transshipment2ETA = myDateTime; this.ShipmentPM.Transshipment2STA = myDateTime; break; }
                                case "S": { this.ShipmentPM.Transshipment2STA = myDateTime; break; }
                            }
                        }

                        else
                        {
                            this.ShipmentPM.Transshipment2ATA = myEventDate;
                        }
                    }

                    else if (this.ShipmentPM.Transshipment3ToPortCode == myPortCode)
                    {
                        if (!string.IsNullOrEmpty(myTimeType))
                        {
                            switch (myTimeType)
                            {
                                case "A": { this.ShipmentPM.Transshipment3ATA = myDateTime; break; }
                                case "E": { this.ShipmentPM.Transshipment3ETA = myDateTime; this.ShipmentPM.Transshipment3STA = myDateTime; break; }
                                case "S": { this.ShipmentPM.Transshipment3STA = myDateTime; break; }
                            }
                        }

                        else
                        {
                            this.ShipmentPM.Transshipment3ATA = myEventDate;
                        }
                    }

                    else if (this.ShipmentPM.PreCarriageToPortCode == myPortCode)
                    {
                        if (!string.IsNullOrEmpty(myTimeType))
                        {
                            switch (myTimeType)
                            {
                                case "A": { this.ShipmentPM.PreCarriageATA = myDateTime; break; }
                                case "E": { this.ShipmentPM.PreCarriageETA = myDateTime; break; }
                            }
                        }

                        else
                        {
                            this.ShipmentPM.PreCarriageATA = myEventDate;
                        }
                    }

                    else if (this.ShipmentPM.OnCarriageToPortCode == myPortCode)
                    {
                        if (!string.IsNullOrEmpty(myTimeType))
                        {
                            switch (myTimeType)
                            {
                                case "A": { this.ShipmentPM.OnCarriageATA = myDateTime; break; }
                                case "E": { this.ShipmentPM.OnCarriageETA = myDateTime; break; }
                            }
                        }

                        else
                        {
                            this.ShipmentPM.OnCarriageATA = myEventDate;
                        }
                    }
                }
            }
        }
        private void BuildTraceEvent_LastStatus(StatusParams iStatusArgs)
        {
            string notes = "";
            string myStatusCode = iStatusArgs.StatusCode.Trim();
            int NumberOfPieces = iStatusArgs.Pieces;
            decimal Weight = iStatusArgs.Weight;
            string FlightNumber = iStatusArgs.FlightNumber;
            string CodeOfLocation = iStatusArgs.LocationPortCode;
            string CodeOfDeparture = iStatusArgs.FromPortCode;
            string CodeOfArrival = iStatusArgs.ToPortCode;
            DateTime? EventDate = iStatusArgs.EventDate;

            switch (myStatusCode)
            {
                case "ARR":
                    {
                        notes = NumberOfPieces + " piece(s) " + Weight + " Kg arrived in " + CodeOfLocation + " from flight " + FlightNumber;
                        break;
                    }
                case "AWD":
                    {
                        notes = "Arrival Documents Delivered for flight " + FlightNumber;
                        break;
                    }
                case "AWR":
                    {
                        notes = "Arrival Documents Received from flight " + FlightNumber;
                        break;
                    }
                case "BKD":
                    {
                        notes = "Flight " + FlightNumber + " booked. " + NumberOfPieces + " piece(s) " + Weight + " Kg from " + CodeOfDeparture + " to " + CodeOfArrival;
                        break;
                    }
                case "CCD":
                    {
                        notes = "Flight " + FlightNumber + " cleared by customs";
                        break;
                    }
                case "CRC":
                    {
                        notes = "Flight " + FlightNumber + " reported by customs";
                        break;
                    }
                case "DDL":
                    {
                        notes = "Door-delivery to consignee for flight " + FlightNumber;
                        break;
                    }
                case "DEP":
                    {
                        notes = NumberOfPieces + " piece(s) " + Weight + " Kg departed on fligh " + FlightNumber + " from " + CodeOfDeparture + " to " + CodeOfArrival + " on " + EventDate;
                        break;
                    }
                case "DIS":
                    {
                        notes = "Discrepancy for flight " + FlightNumber;
                        break;
                    }
                case "DLV":
                    {
                        notes = NumberOfPieces + " piece(s) " + Weight + " Kg delivered in " + CodeOfLocation + " from flight " + FlightNumber;
                        break;
                    }
                case "DOC":
                    {
                        notes = "Documents Received for flight " + FlightNumber;
                        break;
                    }
                case "FOH":
                    {
                        notes = NumberOfPieces + " piece(s) " + Weight + " Kg On-Hand in " + CodeOfLocation + " for flight " + FlightNumber;
                        break;
                    }
                case "MAN":
                    {
                        notes = NumberOfPieces + " piece(s) " + Weight + " Kg manifested on flight " + FlightNumber + " from " + CodeOfDeparture + " to " + CodeOfArrival + " on " + EventDate;
                        break;
                    }
                case "NFD":
                    {
                        notes = "Notify about arrival for flight " + FlightNumber;
                        break;
                    }
                case "PRE":
                    {
                        notes = "Flight " + FlightNumber + " in preparation";
                        break;
                    }
                case "RCF":
                    {
                        notes = NumberOfPieces + " piece(s) " + Weight + " Kg received from flight in " + CodeOfLocation + " from flight " + FlightNumber;
                        break;
                    }
                case "RCS":
                    {
                        notes = NumberOfPieces + " piece(s) " + Weight + " Kg received from shipper in " + CodeOfLocation + " for flight " + FlightNumber;
                        break;
                    }
                case "RCT":
                    {
                        notes = NumberOfPieces + " piece(s) " + Weight + " Kg received from transfer in " + CodeOfLocation + " from flight " + FlightNumber;
                        break;
                    }
                case "TFD":
                    {
                        notes = NumberOfPieces + " piece(s) " + Weight + " Kg transfered in " + CodeOfLocation + " for flight " + FlightNumber;
                        break;
                    }
                case "TGC":
                    {
                        notes = "Consigement tranferred to customs/government control for flight " + FlightNumber;
                        break;
                    }
                case "TRM":
                    {
                        notes = NumberOfPieces + " piece(s) " + Weight + " Kg to be transfered from " + CodeOfLocation + " for flight " + FlightNumber;
                        break;
                    }
            }

            if (iStatusArgs.DepartureDate != null)
            {
                if (!string.IsNullOrEmpty(notes))
                {
                    notes = notes + Environment.NewLine;
                }

                notes = notes + "Scheduled time of flight Departure: " + iStatusArgs.DepartureDate.Value.ToShortTimeString();
            }

            if (iStatusArgs.ArrivalDate != null)
            {
                if (!string.IsNullOrEmpty(notes))
                {
                    notes = notes + Environment.NewLine;
                }

                notes = notes + "Scheduled time of flight Arrival: " + iStatusArgs.ArrivalDate.Value.ToShortTimeString();
            }

            EventTracerArgs myEventArgs = new EventTracerArgs()
            {
                Tenant = myTenant,
                EventTypeCode = myStatusCode + "E",
                UserId = null,
                EntityId = this.ShipmentPM.Id,
                ObjectTableName = "Shipment",
                Notes = notes,
            };

            EventTracer.CreateTraceEvent(myEventArgs);
        }

        private void CreateShipmentStatus(StatusParams iStatusArgs)
        {
            string iRecordData = this.BuildRecordData(iStatusArgs);

            if (!string.IsNullOrEmpty(iRecordData))
            {
                string iRecordHash = GetHashedData(iRecordData);

                ShipmentCarrierStatusRepository reposioty = new ShipmentCarrierStatusRepository(this.iShipmentsContext);

                if (!reposioty.DoesRecordExist(iRecordHash))
                {
                    ShipmentCarrierStatus status = new ShipmentCarrierStatus()
                    {
                        Id = IdCounter.GetNumber("ShipmentCarrierStatus", iStatusArgs.Tenant),
                        Tenant = iStatusArgs.Tenant,
                        ReceivingDate = iStatusArgs.LogDate,
                        ShipmentId = iStatusArgs.EntityId,
                        RecordHash = iRecordHash,
                        Status = iStatusArgs.StatusCode,
                        Details = iStatusArgs.Details,
                        EventDate = iStatusArgs.EventDate,
                        FlightNumber = iStatusArgs.FlightNumber,
                        AirlineName = iStatusArgs.AirlineName,
                        Pieces = iStatusArgs.Pieces,
                        Partial = iStatusArgs.Partial,
                        Weight = Convert.ToDouble(iStatusArgs.Weight),
                        DepartureDate = iStatusArgs.DepartureDate,
                        ArrivalDate = iStatusArgs.ArrivalDate,
                        TimeOfArrivalInfo = iStatusArgs.TimeOfArrivalInfo,
                        TimeOfDepartureInfo = iStatusArgs.TimeOfDepartureInfo,
                        FromPortId = iStatusArgs.FromPortId,
                        ToPortId = iStatusArgs.ToPortId,
                        Location = iStatusArgs.LocationPortId,
                    };


                    reposioty.Add(status);
                    reposioty.SubmitChanges();
                }
            }
        }
        private string BuildRecordData(StatusParams iStatusArgs)
        {
            string iResult = this.myTenant.ToString();

            if (!string.IsNullOrEmpty(iStatusArgs.FromPortCode))
            {
                Port iPort = this.myPortRepository.GetAirlinePortByCode(this.myTenant, iStatusArgs.FromPortCode, true);
                if (iPort == null)
                {
                    Port portZero = this.myPortRepository.GetPortsByNameOrCode(iStatusArgs.FromPortCode, null, 0).Where(a => a.IsAir).FirstOrDefault();
                    if (portZero != null)
                    {
                        iPort = this.GetPortCopyToCurrentTenant(portZero.Id, this.myTenant);
                    }
                }

                if (iPort != null)
                {
                    iResult += iPort.Id;
                    iStatusArgs.FromPortId = iPort.Id;
                }
            }

            if (!string.IsNullOrEmpty(iStatusArgs.ToPortCode))
            {
                Port iPort = this.myPortRepository.GetAirlinePortByCode(this.myTenant, iStatusArgs.ToPortCode, true);
                if (iPort == null)
                {
                    Port portZero = this.myPortRepository.GetPortsByNameOrCode(iStatusArgs.ToPortCode, null, 0).Where(a => a.IsAir).FirstOrDefault();
                    if (portZero != null)
                    {
                        iPort = this.GetPortCopyToCurrentTenant(portZero.Id, this.myTenant);
                    }
                }

                if (iPort != null)
                {
                    iResult += iPort.Id;
                    iStatusArgs.ToPortId = iPort.Id;
                }
            }

            if (!string.IsNullOrEmpty(iStatusArgs.LocationPortCode))
            {
                Port iPort = this.myPortRepository.GetAirlinePortByCode(this.myTenant, iStatusArgs.LocationPortCode, true);
                if (iPort == null)
                {
                    Port portZero = this.myPortRepository.GetPortsByNameOrCode(iStatusArgs.LocationPortCode, null, 0).Where(a => a.IsAir).FirstOrDefault();
                    if (portZero != null)
                    {
                        iPort = this.GetPortCopyToCurrentTenant(portZero.Id, this.myTenant);
                    }
                }

                if (iPort != null)
                {
                    iStatusArgs.LocationPortId = iPort.Id;
                }
            }

            if (!string.IsNullOrEmpty(iStatusArgs.AirlineName))
            {
                iResult += iStatusArgs.AirlineName;
            }

            if (!string.IsNullOrEmpty(iStatusArgs.Details))
            {
                iResult += iStatusArgs.Details;
            }

            if (!string.IsNullOrEmpty(iStatusArgs.StatusCode))
            {
                iResult += iStatusArgs.StatusCode;
            }

            if (!string.IsNullOrEmpty(iStatusArgs.FlightNumber))
            {
                iResult += iStatusArgs.FlightNumber;
            }
           
            iResult += iStatusArgs.Partial;
            iResult += iStatusArgs.Pieces;
            iResult += iStatusArgs.Weight;
            iResult += iStatusArgs.EntityId;

            return iResult;
        }
    }
}
