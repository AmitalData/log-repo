using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.DataContracts;
using Logitude.BL.ShipmentsModel.Tools.EntityService;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.XSD.Analyzers.GLSHKAnalyzer
{
    public partial class GLSHKAnalyzer
    {
        private GLSHK.FSU myFSU;
        private GLSHK.FSUAWB myFSUAWB;
        private void AnalyzeBaseData_FSU()
        {
            this.myFSU = (GLSHK.FSU)myMessage.Item;
            this.myFSUAWB = (GLSHK.FSUAWB)myFSU.FSUAWB[0];
            this.myPrefix = myFSUAWB.AWB.Prefix;
            this.myMaster = myFSUAWB.AWB.SerialNum;

            if (myPrefix.Length < 3)
            {
                int rem = 3 - myPrefix.Length;
                for (int i = 0; i < rem; i++)
                {
                    myPrefix = "0" + myPrefix;
                }
            }

            if (myMaster.Length < 8)
            {
                int rem = 8 - myMaster.Length;
                for (int i = 0; i < rem; i++)
                {
                    myMaster = "0" + myMaster;
                }
            }
        }

        private void AnalyzeMessageQueue_FSU()
        {
            PortRepository portRepository = new PortRepository(myCommonContext);
            DocumentRepository documentrepository = new DocumentRepository(myCommonContext);

            string myFromPortCode = null;
            string myToPortCode = null;
           
            #region AWBOD
            if (myFSUAWB.AWBOD != null)
            {
                myFromPortCode = myFSUAWB.AWBOD.Origin;
                myToPortCode = myFSUAWB.AWBOD.Destination;

                if (shipmentPM.MainCarriageFromPortCode == "---")
                {
                    Port newFromPort = portRepository.GetPortsByNameOrCode(myFSUAWB.AWBOD.Origin, null, shipmentPM.Tenant).Where(d => d.IsAir).FirstOrDefault();
                    if (newFromPort == null)
                    {
                        Port portZero = portRepository.GetPortsByNameOrCode(myFSUAWB.AWBOD.Origin, null, 0).Where(d => d.IsAir).FirstOrDefault();
                        if (portZero != null)
                        {
                            newFromPort = this.GetPortCopyToCurrentTenant(portZero.Id, shipmentPM.Tenant);
                        }
                    }
                    shipmentPM.MainCarriageFromPortId = newFromPort != null ? newFromPort.Id : null;
                }

                if (shipmentPM.MainCarriageToPortCode == "---")
                {
                    Port newToPort = portRepository.GetPortsByNameOrCode(myFSUAWB.AWBOD.Destination, null, shipmentPM.Tenant).Where(d => d.IsAir).FirstOrDefault();
                    if (newToPort == null)
                    {
                        Port portZero = portRepository.GetPortsByNameOrCode(myFSUAWB.AWBOD.Destination, null, 0).Where(d => d.IsAir).FirstOrDefault();
                        if (portZero != null)
                        {
                            newToPort = this.GetPortCopyToCurrentTenant(portZero.Id, shipmentPM.Tenant);
                        }
                    }
                    shipmentPM.MainCarriageToPortId = newToPort != null ? newToPort.Id : null;
                }
            }

            if (myFSUAWB.Quantity != null)
            {
                if (shipmentPM.NumberOfPackages == 0 || shipmentPM.NumberOfPackages == null)
                {
                    int myPieces = 0;
                    bool isParsed = Int32.TryParse(myFSUAWB.Quantity.Pieces, out myPieces);
                    if (isParsed)
                    {
                        shipmentPM.NumberOfPackages = myPieces;
                    }
                }

                if (shipmentPM.GrossWeight == 0 || shipmentPM.GrossWeight == null)
                {
                    string weightCode = myFSUAWB.Quantity.WeightCode.ToString();
                    shipmentPM.GrossWeight = (double)myFSUAWB.Quantity.Weight;
                    shipmentPM.GrossWeightUnitCode = weightCode == "L" ? "LB" : "KG";

                    if (weightCode == "L")
                    {
                        shipmentPM.GrossWeightInKG = shipmentPM.GrossWeight / 2.20462262;
                    }

                    else
                    {
                        shipmentPM.GrossWeightInKG = shipmentPM.GrossWeight;
                    }

                    shipmentPM.ChargeableWeight = CalculateChargeableWeight(shipmentPM.GrossWeight, shipmentPM.VolumetricWeight, shipmentPM.GrossWeightUnitCode, shipmentPM.ChargeableWeightUnitCode, shipmentPM.DirectionId, shipmentPM.TransportModeId);
                }
            }
            #endregion

            #region OSI
            //if (answer.SplitConsignment[0].OtherServiceInformation != null)
            //{
            //    string osi = answer.SplitConsignment[0].OtherServiceInformation.OSIDetailsFirstLine + Environment.NewLine + answer.SplitConsignment[0].OtherServiceInformation.OSIDetailsSecondLine;

            //    if (!string.IsNullOrEmpty(osi))
            //    {
            //        CreateCarrierStatus(new CreateStatusParams() { Tenant = tenant, EntityId = shipmentPM.Id, StatusCode = "OSI", Details = osi, FromPortCode = null, ToPortCode = null, FlightNumber = null, Pieces = 0, Partial = false, Weight = 0, EventDate = null, LogDate = TenantServerConfigration.GetCurrentDateTime(tenant), Location = null, AirlineName = null, CommonContext = commonContext, ShipmentContext = shipmentContext });
            //        if (string.IsNullOrEmpty(shipmentPM.CarrierLastStatusCode))
            //        {
            //            shipmentPM.CarrierLastStatusCode = "OSI";
            //            shipmentPM.CarrierLastStatusDate = TenantServerConfigration.GetCurrentDateTime(tenant);
            //        }
            //    }
            //}
            #endregion

            #region Status Detail
            if (myFSUAWB.FSUStatusDetail != null && myFSUAWB.FSUStatusDetail.Count() > 0)
            {
                foreach (GLSHK.FSUStatusDetail item in myFSUAWB.FSUStatusDetail)
                {
                    StatusContext myStatusContext = new StatusContext()
                    {
                        LogDate = TenantServerConfigration.GetCurrentDateTime(myTenant),
                    };

                    if (item.RCSDetail != null)
                    {
                        GLSHK.RCSDetail myStatusObject = item.RCSDetail;

                        string myCode = "RCS";
                        myStatusContext.Details = myCode;
                        myStatusContext.StatusCode = myCode;
                        AnalyzeStatus(myStatusContext, myStatusObject.Movement);
                        AnalyzeStatus(myStatusContext, myStatusObject.Quantity);
                        UpdateLastStatus(myStatusContext);
                    }

                    else if (item.RCTDetail != null)
                    {
                        GLSHK.RCTDetail myStatusObject = item.RCTDetail;

                        string myCode = "RCT";
                        myStatusContext.Details = myCode;
                        myStatusContext.StatusCode = myCode;
                        AnalyzeStatus(myStatusContext, myStatusObject.Movement);
                        AnalyzeStatus(myStatusContext, myStatusObject.Quantity);
                        UpdateLastStatus(myStatusContext);
                    }

                    else if (item.RCFDetail != null)
                    {
                        GLSHK.RCFDetail myStatusObject = item.RCFDetail;

                        string myCode = "RCF";
                        myStatusContext.Details = myCode;
                        myStatusContext.StatusCode = myCode;
                        AnalyzeStatus(myStatusContext, myStatusObject.Movement);
                        AnalyzeStatus(myStatusContext, myStatusObject.Quantity);
                        AnalyzeStatus(myStatusContext, myStatusObject.ARRTime, "ARR");
                        AnalyzeStatus(myStatusContext, myStatusObject.DEPTime, "DEP");
                        UpdateLastStatus(myStatusContext);
                    }

                    else if (item.BKDDetail != null)
                    {
                        GLSHK.BKDDetail myStatusObject = item.BKDDetail;

                        string myCode = "BKD";
                        myStatusContext.Details = myCode;
                        myStatusContext.StatusCode = myCode;
                        myStatusContext.CodeOfDeparture = myStatusObject.Movement.DEPAirport;
                        myStatusContext.CodeOfArrival = myStatusObject.Movement.ARRAirport;
                        AnalyzeStatus(myStatusContext, myStatusObject.Movement); // New
                        AnalyzeStatus(myStatusContext, myStatusObject.Quantity);
                        AnalyzeStatus(myStatusContext, myStatusObject.ARRTime, "ARR");
                        AnalyzeStatus(myStatusContext, myStatusObject.DEPTime, "DEP");
                        UpdateLastStatus(myStatusContext);
                    }

                    else if (item.MANDetail != null)
                    {
                        GLSHK.MANDetail myStatusObject = item.MANDetail;

                        string myCode = "MAN";
                        myStatusContext.Details = myCode;
                        myStatusContext.StatusCode = myCode;
                        myStatusContext.CodeOfDeparture = myStatusObject.Movement.DEPAirport;
                        myStatusContext.CodeOfArrival = myStatusObject.Movement.ARRAirport;
                        AnalyzeStatus(myStatusContext, myStatusObject.Movement);
                        AnalyzeStatus(myStatusContext, myStatusObject.Quantity);
                        AnalyzeStatus(myStatusContext, myStatusObject.ARRTime, "ARR");
                        AnalyzeStatus(myStatusContext, myStatusObject.DEPTime, "DEP");
                        UpdateLastStatus(myStatusContext);
                    }

                    else if (item.PREDetail != null)
                    {
                        GLSHK.PREDetail myStatusObject = item.PREDetail;

                        string myCode = "PRE";
                        myStatusContext.Details = myCode;
                        myStatusContext.StatusCode = myCode;
                        AnalyzeStatus(myStatusContext, myStatusObject.Movement);
                        AnalyzeStatus(myStatusContext, myStatusObject.Quantity);
                        AnalyzeStatus(myStatusContext, myStatusObject.ARRTime, "ARR");
                        AnalyzeStatus(myStatusContext, myStatusObject.DEPTime, "DEP");
                        UpdateLastStatus(myStatusContext);
                    }

                    else if (item.TRMDetail != null)
                    {
                        GLSHK.TRMDetail myStatusObject = item.TRMDetail;

                        string myCode = "TRM";
                        myStatusContext.Details = myCode;
                        myStatusContext.StatusCode = myCode;
                        AnalyzeStatus(myStatusContext, myStatusObject.Movement);
                        AnalyzeStatus(myStatusContext, myStatusObject.Quantity);
                        UpdateLastStatus(myStatusContext);
                    }

                    else if (item.TFDDetail != null)
                    {
                        GLSHK.TFDDetail myStatusObject = item.TFDDetail;

                        string myCode = "TFD";
                        myStatusContext.Details = myCode;
                        myStatusContext.StatusCode = myCode;
                        AnalyzeStatus(myStatusContext, myStatusObject.Movement);
                        AnalyzeStatus(myStatusContext, myStatusObject.Quantity);
                        UpdateLastStatus(myStatusContext);
                    }

                    else if (item.NFDDetail != null)
                    {
                        GLSHK.NFDDetail myStatusObject = item.NFDDetail;

                        string myCode = "NFD";
                        myStatusContext.Details = myCode;
                        myStatusContext.StatusCode = myCode;
                        AnalyzeStatus(myStatusContext, myStatusObject.Movement);
                        AnalyzeStatus(myStatusContext, myStatusObject.Quantity);
                        UpdateLastStatus(myStatusContext);
                    }

                    else if (item.AWDDetail != null)
                    {
                        GLSHK.AWDDetail myStatusObject = item.AWDDetail;

                        string myCode = "AWD";
                        myStatusContext.Details = myCode;
                        myStatusContext.StatusCode = myCode;
                        AnalyzeStatus(myStatusContext, myStatusObject.Movement);
                        AnalyzeStatus(myStatusContext, myStatusObject.Quantity);
                        UpdateLastStatus(myStatusContext);
                    }

                    else if (item.CCDDetail != null)
                    {
                        GLSHK.CCDDetail myStatusObject = item.CCDDetail;

                        string myCode = "CCD";
                        myStatusContext.Details = myCode;
                        myStatusContext.StatusCode = myCode;
                        AnalyzeStatus(myStatusContext, myStatusObject.Movement);
                        AnalyzeStatus(myStatusContext, myStatusObject.Quantity);
                        UpdateLastStatus(myStatusContext);
                    }

                    else if (item.DLVDetail != null)
                    {
                        GLSHK.DLVDetail myStatusObject = item.DLVDetail;

                        string myCode = "DLV";
                        myStatusContext.Details = myCode;
                        myStatusContext.StatusCode = myCode;
                        AnalyzeStatus(myStatusContext, myStatusObject.Movement);
                        AnalyzeStatus(myStatusContext, myStatusObject.Quantity);
                        UpdateLastStatus(myStatusContext);
                    }

                    else if (item.DISDetail != null)
                    {
                        GLSHK.DISDetail myStatusObject = item.DISDetail;

                        string myCode = "DIS";
                        myStatusContext.Details = myCode;
                        myStatusContext.StatusCode = myCode;
                        AnalyzeStatus(myStatusContext, myStatusObject.Movement);
                        AnalyzeStatus(myStatusContext, myStatusObject.Quantity);
                        UpdateLastStatus(myStatusContext);
                    }

                    else if (item.CRCDetail != null)
                    {
                        GLSHK.CRCDetail myStatusObject = item.CRCDetail;

                        string myCode = "CRC";
                        myStatusContext.Details = myCode;
                        myStatusContext.StatusCode = myCode;
                        AnalyzeStatus(myStatusContext, myStatusObject.Movement);
                        AnalyzeStatus(myStatusContext, myStatusObject.Quantity);
                        UpdateLastStatus(myStatusContext);
                    }

                    else if (item.DDLDetail != null)
                    {
                        GLSHK.DDLDetail myStatusObject = item.DDLDetail;

                        string myCode = "DDL";
                        myStatusContext.Details = myCode;
                        myStatusContext.StatusCode = myCode;
                        AnalyzeStatus(myStatusContext, myStatusObject.Movement);
                        AnalyzeStatus(myStatusContext, myStatusObject.Quantity);
                        UpdateLastStatus(myStatusContext);
                    }

                    else if (item.TGCDetail != null)
                    {
                        GLSHK.TGCDetail myStatusObject = item.TGCDetail;

                        string myCode = "TGC";
                        myStatusContext.Details = myCode;
                        myStatusContext.StatusCode = myCode;
                        AnalyzeStatus(myStatusContext, myStatusObject.Movement);
                        AnalyzeStatus(myStatusContext, myStatusObject.Quantity);
                        UpdateLastStatus(myStatusContext);
                    }

                    else if (item.DEPDetail != null)
                    {
                        GLSHK.DEPDetail myStatusObject = item.DEPDetail;

                        string myCode = "DEP";
                        myStatusContext.Details = myCode;
                        myStatusContext.StatusCode = myCode;
                        myStatusContext.CodeOfDeparture = myStatusObject.Movement.DEPAirport;
                        myStatusContext.CodeOfArrival = myStatusObject.Movement.ARRAirport;
                        AnalyzeStatus(myStatusContext, myStatusObject.Movement);
                        AnalyzeStatus(myStatusContext, myStatusObject.Quantity);
                        AnalyzeStatus(myStatusContext, myStatusObject.ARRTime, "ARR");
                        AnalyzeStatus(myStatusContext, myStatusObject.DEPTime, "DEP");
                        UpdateLastStatus(myStatusContext);

                        if (myStatusContext.DepartureDate != null)
                        {
                            UpdateDepartureDates(myStatusContext, myFromPortCode);
                        }
                    }

                    else if (item.ARRDetail != null)
                    {
                        GLSHK.ARRDetail myStatusObject = item.ARRDetail;

                        string myCode = "ARR";
                        myStatusContext.Details = myCode;
                        myStatusContext.StatusCode = myCode;
                        AnalyzeStatus(myStatusContext, myStatusObject.Movement);
                        AnalyzeStatus(myStatusContext, myStatusObject.Quantity);
                        AnalyzeStatus(myStatusContext, myStatusObject.ARRTime, "ARR");
                        AnalyzeStatus(myStatusContext, myStatusObject.DEPTime, "DEP");
                        UpdateLastStatus(myStatusContext);

                        if (myStatusContext.ArrivalDate != null)
                        {
                            UpdateArrivalDates(myStatusContext, myToPortCode);
                        }
                    }

                    else if (item.AWRDetail != null)
                    {
                        GLSHK.AWRDetail myStatusObject = item.AWRDetail;

                        string myCode = "AWR";
                        myStatusContext.Details = myCode;
                        myStatusContext.StatusCode = myCode;
                        AnalyzeStatus(myStatusContext, myStatusObject.Movement);
                        AnalyzeStatus(myStatusContext, myStatusObject.Quantity);
                        UpdateLastStatus(myStatusContext);
                    }

                    else if (item.FOHDetail != null)
                    {
                        GLSHK.FOHDetail myStatusObject = item.FOHDetail;

                        string myCode = "FOH";
                        myStatusContext.Details = myCode;
                        myStatusContext.StatusCode = myCode;
                        AnalyzeStatus(myStatusContext, myStatusObject.Movement);
                        AnalyzeStatus(myStatusContext, myStatusObject.Quantity);
                        UpdateLastStatus(myStatusContext);
                    }

                    if (!string.IsNullOrEmpty(myStatusContext.StatusCode))
                    {
                        CreateCarrierStatus(new StatusParams()
                        {
                            Tenant = myTenant,
                            AirlineName = shipmentPM.MainCarriageCarrierName,
                            CommonContext = myCommonContext,
                            ShipmentContext = myShipmentContext,
                            EntityId = shipmentPM.Id,
                            EventDate = myStatusContext.EventDate,
                            FlightNumber = myStatusContext.FlightNumber,
                            Location = myStatusContext.Location,
                            LogDate = myStatusContext.LogDate,
                            Partial = myStatusContext.IsPartial,
                            Pieces = myStatusContext.NumberOfPieces,
                            Weight = myStatusContext.Weight,
                            StatusCode = myStatusContext.StatusCode,
                            ArrivalDate = myStatusContext.ArrivalDate,
                            DepartureDate = myStatusContext.DepartureDate,
                            TimeOfArrivalInfo = myStatusContext.TimeOfArrivalInfo,
                            TimeOfDepartureInfo = myStatusContext.TimeOfDepartureInfo,
                            Details = myStatusContext.Details,
                            FromPortCode = myFromPortCode,
                            ToPortCode = myToPortCode,
                        });
                    }
                }
            }
            #endregion

            shipmentPM.IsFSRSent = false;
            string systemEmail = "system@tenant" + myTenant + ".com";

            ShipmentService service = new ShipmentService(myShipmentContext, shipmentPM, systemEmail);
            service.Update();
        }

        private void AnalyzeStatus(StatusContext statusContext, GLSHK.MovementDetail myMovementDetail)
        {
            if (myMovementDetail != null)
            {
                statusContext.Location = myMovementDetail.AirportCode;
                statusContext.FlightNumber = myMovementDetail.CarrierCode + myMovementDetail.FlightNum;

                int day = ConvertStringToInteger(myMovementDetail.Day);
                int month = !string.IsNullOrEmpty(myMovementDetail.Month) ? GetMonthInNumbers(myMovementDetail.Month) : 0;
                int hour = ConvertStringToInteger(myMovementDetail.ActualTime) / 100;
                int minute = ConvertStringToInteger(myMovementDetail.ActualTime) % 100;

                DateTime? currentDate = shipmentPM.MAWBOBLDate != null ? shipmentPM.MAWBOBLDate : shipmentPM.CreateDateTime;
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
                    statusContext.EventDate = new DateTime(year, month, day, hour, minute, 0);
                }

                if (statusContext.StatusCode != null)
                {
                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = myTenant,
                        EventTypeCode = "FSAR",
                        UserId = null,
                        EntityId = shipmentPM.Id,
                        ObjectTableName = "Shipment",
                        Notes = statusContext.StatusCode + " status received",
                    });
                }
            }
        }
        private void AnalyzeStatus(StatusContext statusContext, GLSHK.MovementDetail_DEPARR myMovementDetail)
        {
            if (myMovementDetail != null)
            {
                statusContext.Location = myMovementDetail.ARRAirport;
                statusContext.FlightNumber = myMovementDetail.FlightNum;

                int day = ConvertStringToInteger(myMovementDetail.Day);
                int month = !string.IsNullOrEmpty(myMovementDetail.Month) ? GetMonthInNumbers(myMovementDetail.Month) : 0;
                int hour = ConvertStringToInteger(myMovementDetail.ActualTime) / 100;
                int minute = ConvertStringToInteger(myMovementDetail.ActualTime) % 100;

                DateTime? currentDate = shipmentPM.MAWBOBLDate != null ? shipmentPM.MAWBOBLDate : shipmentPM.CreateDateTime;
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
                    statusContext.EventDate = new DateTime(year, month, day, hour, minute, 0);
                }

                if (statusContext.StatusCode != null)
                {
                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = myTenant,
                        EventTypeCode = "FSAR",
                        UserId = null,
                        EntityId = shipmentPM.Id,
                        ObjectTableName = "Shipment",
                        Notes = statusContext.StatusCode + " status received",
                    });
                }
            }
        }
        private void AnalyzeStatus(StatusContext statusContext, GLSHK.MovementDetail_TRM myMovementDetail)
        {
            //if (myMovementDetail != null)
            //{
            //    statusContext.Location = myMovementDetail.AirportCode;
            //    //statusContext.FlightNumber = myMovementDetail.FlightNum;

            //    //int day = ConvertStringToInteger(myMovementDetail.Day);
            //    //int month = !string.IsNullOrEmpty(myMovementDetail.Month) ? GetMonthInNumbers(myMovementDetail.Month) : 0;
            //    //int hour = ConvertStringToInteger(myMovementDetail.ActualTime) / 100;
            //    //int minute = ConvertStringToInteger(myMovementDetail.ActualTime) % 100;

            //    DateTime? currentDate = shipmentPM.MAWBOBLDate != null ? shipmentPM.MAWBOBLDate : shipmentPM.CreateDateTime;
            //    int currentMonth = currentDate.Value.Month;
            //    int currentYear = currentDate.Value.Year;
            //    int difference = Math.Abs(currentMonth - month);

            //    int year = 0;
            //    if (difference > 6)
            //    {
            //        if (month > currentMonth)
            //        {
            //            year = currentYear - 1;
            //        }

            //        else
            //        {
            //            year = currentYear + 1;
            //        }
            //    }

            //    else
            //    {
            //        year = currentYear;
            //    }

            //    if (day != 0 && month != 0)
            //    {
            //        statusContext.EventDate = new DateTime(year, month, day, hour, minute, 0);
            //    }

            //    if (statusContext.StatusCode != null)
            //    {
            //        string eventTypeCode = "FSAR";
            //        EventTracer.CreateTraceEvent(new TraceEvent(), eventTypeCode, myTenant, null, shipmentPM.Id, statusContext.StatusCode + " status received", objectTableName, null, null, false);
            //    }
            //}
        }
        private void AnalyzeStatus(StatusContext statusContext, GLSHK.QTYDetail myQuantityDetail)
        {
            if (myQuantityDetail != null)
            {
                if (myQuantityDetail.DescCode == GLSHK.QTYDetailDescCode.T)
                {
                    statusContext.IsPartial = false;
                }

                statusContext.Weight = myQuantityDetail.Weight;
                statusContext.NumberOfPieces = ConvertStringToInteger(myQuantityDetail.Pieces);
            }
        }
        private void AnalyzeStatus(StatusContext myStatusContext, GLSHK.FlightTime flightTime, string timeCode)
        {
            if (flightTime != null)
            {
                switch (timeCode)
                {
                    case "ARR":
                        {
                            myStatusContext.TimeOfArrivalInfo = flightTime.TimeType;

                            if (flightTime.TimeType != null)
                            {
                                int hours = ConvertStringToInteger(flightTime.Time) / 100;
                                int minutes = ConvertStringToInteger(flightTime.Time) % 100;
                                int dayIndicator = ConvertStringToInteger(flightTime.DayChangeIndicator);
                                DateTime myCurrentDate = myStatusContext.EventDate != null ? myStatusContext.EventDate.Value.AddDays(dayIndicator) : myStatusContext.LogDate.AddDays(dayIndicator);
                                myStatusContext.ArrivalDate = new DateTime(myCurrentDate.Year, myCurrentDate.Month, myCurrentDate.Day, hours, minutes, 0);
                            }

                            break;
                        }

                    case "DEP":
                        {
                            myStatusContext.TimeOfDepartureInfo = flightTime.TimeType;

                            if (flightTime.TimeType != null)
                            {
                                int hours = ConvertStringToInteger(flightTime.Time) / 100;
                                int minutes = ConvertStringToInteger(flightTime.Time) % 100;
                                int dayIndicator = ConvertStringToInteger(flightTime.DayChangeIndicator);
                                DateTime myCurrentDate = myStatusContext.EventDate != null ? myStatusContext.EventDate.Value.AddDays(dayIndicator) : myStatusContext.LogDate.AddDays(dayIndicator);
                                myStatusContext.DepartureDate = new DateTime(myCurrentDate.Year, myCurrentDate.Month, myCurrentDate.Day, hours, minutes, 0);
                            }

                            break;
                        }
                }
            }
        }
        private void UpdateLastStatus(StatusContext myStatusContext)
        {
            string currentCarrierStatus = shipmentPM.CarrierLastStatusCode;
            DateTime? CurrentCarrierLastStatusDate = shipmentPM.CarrierLastStatusDate;

            DateTime carrierLastStatusDate = myStatusContext.EventDate != null ? myStatusContext.EventDate.Value : myStatusContext.LogDate;

            if (CurrentCarrierLastStatusDate == null)
            {
                CurrentCarrierLastStatusDate = carrierLastStatusDate;
                currentCarrierStatus = myStatusContext.StatusCode;
            }

            else
            {
                if (carrierLastStatusDate.Year > CurrentCarrierLastStatusDate.Value.Year)
                {
                    CurrentCarrierLastStatusDate = carrierLastStatusDate;
                    currentCarrierStatus = myStatusContext.StatusCode;
                }
                else if (carrierLastStatusDate.Year == CurrentCarrierLastStatusDate.Value.Year)
                {
                    if (carrierLastStatusDate.Month > CurrentCarrierLastStatusDate.Value.Month)
                    {
                        CurrentCarrierLastStatusDate = carrierLastStatusDate;
                        currentCarrierStatus = myStatusContext.StatusCode;
                    }
                    else if (carrierLastStatusDate.Month == CurrentCarrierLastStatusDate.Value.Month)
                    {
                        if (carrierLastStatusDate.Day > CurrentCarrierLastStatusDate.Value.Day)
                        {
                            CurrentCarrierLastStatusDate = carrierLastStatusDate;
                            currentCarrierStatus = myStatusContext.StatusCode;
                        }
                        else if (carrierLastStatusDate.Day == CurrentCarrierLastStatusDate.Value.Day)
                        {
                            if (carrierLastStatusDate.Hour > CurrentCarrierLastStatusDate.Value.Hour)
                            {
                                CurrentCarrierLastStatusDate = carrierLastStatusDate;
                                currentCarrierStatus = myStatusContext.StatusCode;
                            }
                            else if (carrierLastStatusDate.Hour == CurrentCarrierLastStatusDate.Value.Hour)
                            {
                                if (carrierLastStatusDate.Minute > CurrentCarrierLastStatusDate.Value.Minute)
                                {
                                    CurrentCarrierLastStatusDate = carrierLastStatusDate;
                                    currentCarrierStatus = myStatusContext.StatusCode;
                                }
                            }
                        }
                    }
                }
            }

            shipmentPM.CarrierLastStatusCode = currentCarrierStatus;
            shipmentPM.CarrierLastStatusDate = CurrentCarrierLastStatusDate;

            this.TraceEachStatus(myStatusContext, shipmentPM.Id);
        }
        private void UpdateDepartureDates(StatusContext myStatusContext, string myPortCode)
        {
            if (!string.IsNullOrEmpty(myPortCode))
            {
                DateTime? myEventDate = myStatusContext.EventDate;
                DateTime? myDateTime = myStatusContext.DepartureDate;
                string myTimeType = myStatusContext.TimeOfDepartureInfo;

                if (shipmentPM.MainCarriageFromPortCode == myPortCode)
                {
                    if (!string.IsNullOrEmpty(myTimeType))
                    {
                        switch (myTimeType)
                        {
                            case "A": { shipmentPM.MainCarriageATD = myDateTime; break; }
                            case "E": { shipmentPM.MainCarriageETD = myDateTime; shipmentPM.MainCarriageSTD = myDateTime; break; }
                            case "S": { shipmentPM.MainCarriageSTD = myDateTime; break; }
                        }
                    }

                    else
                    {
                        shipmentPM.MainCarriageATD = myEventDate;
                    }
                }

                else if (shipmentPM.Transshipment1FromPortCode == myPortCode)
                {
                    if (!string.IsNullOrEmpty(myTimeType))
                    {
                        switch (myTimeType)
                        {
                            case "A": { shipmentPM.Transshipment1ATD = myDateTime; break; }
                            case "E": { shipmentPM.Transshipment1ETD = myDateTime; shipmentPM.Transshipment1STD = myDateTime; break; }
                            case "S": { shipmentPM.Transshipment1STD = myDateTime; break; }
                        }
                    }
                    else
                    {
                        shipmentPM.Transshipment1ATD = myEventDate;
                    }

                }

                else if (shipmentPM.Transshipment2FromPortCode == myPortCode)
                {
                    if (!string.IsNullOrEmpty(myTimeType))
                    {
                        switch (myTimeType)
                        {
                            case "A": { shipmentPM.Transshipment2ATD = myDateTime; break; }
                            case "E": { shipmentPM.Transshipment2ETD = myDateTime; shipmentPM.Transshipment2STD = myDateTime; break; }
                            case "S": { shipmentPM.Transshipment2STD = myDateTime; break; }
                        }
                    }

                    else
                    {
                        shipmentPM.Transshipment2ATD = myEventDate;
                    }
                }

                else if (shipmentPM.Transshipment3FromPortCode == myPortCode)
                {
                    if (!string.IsNullOrEmpty(myTimeType))
                    {
                        switch (myTimeType)
                        {
                            case "A": { shipmentPM.Transshipment3ATD = myDateTime; break; }
                            case "E": { shipmentPM.Transshipment3ETD = myDateTime; shipmentPM.Transshipment3STD = myDateTime; break; }
                            case "S": { shipmentPM.Transshipment3STD = myDateTime; break; }
                        }
                    }

                    else
                    {
                        shipmentPM.Transshipment3ATD = myEventDate;
                    }
                }

                else if (shipmentPM.PreCarriageFromPortCode == myPortCode)
                {
                    if (!string.IsNullOrEmpty(myTimeType))
                    {
                        switch (myTimeType)
                        {
                            case "A": { shipmentPM.PreCarriageATD = myDateTime; break; }
                            case "E": { shipmentPM.PreCarriageETD = myDateTime; break; }
                        }
                    }

                    else
                    {
                        shipmentPM.PreCarriageATD = myEventDate;
                    }
                }

                else if (shipmentPM.OnCarriageFromPortCode == myPortCode)
                {
                    if (!string.IsNullOrEmpty(myTimeType))
                    {
                        switch (myTimeType)
                        {
                            case "A": { shipmentPM.OnCarriageATD = myDateTime; break; }
                            case "E": { shipmentPM.OnCarriageETD = myDateTime; break; }
                        }
                    }

                    else
                    {
                        shipmentPM.OnCarriageATD = myEventDate;
                    }
                }
            }
        }
        private void UpdateArrivalDates(StatusContext myStatusContext, string myPortCode)
        {
            if (!string.IsNullOrEmpty(myPortCode))
            {
                DateTime? myEventDate = myStatusContext.EventDate;
                DateTime? myDateTime = myStatusContext.ArrivalDate;
                string myTimeType = myStatusContext.TimeOfArrivalInfo;

                if (shipmentPM.MainCarriageToPortCode == myPortCode)
                {
                    if (!string.IsNullOrEmpty(myTimeType))
                    {
                        switch (myTimeType)
                        {
                            case "A": { shipmentPM.MainCarriageATA = myDateTime; break; }
                            case "E": { shipmentPM.MainCarriageETA = myDateTime; shipmentPM.MainCarriageSTA = myDateTime; break; }
                            case "S": { shipmentPM.MainCarriageSTA = myDateTime; break; }
                        }
                    }

                    else
                    {
                        shipmentPM.MainCarriageATA = myEventDate;
                    }
                }

                else if (shipmentPM.Transshipment1ToPortCode == myPortCode)
                {
                    if (!string.IsNullOrEmpty(myTimeType))
                    {
                        switch (myTimeType)
                        {
                            case "A": { shipmentPM.Transshipment1ATA = myDateTime; break; }
                            case "E": { shipmentPM.Transshipment1ETA = myDateTime; shipmentPM.Transshipment1STA = myDateTime; break; }
                            case "S": { shipmentPM.Transshipment1STA = myDateTime; break; }
                        }
                    }

                    else
                    {
                        shipmentPM.Transshipment1ATA = myEventDate;
                    }
                }

                else if (shipmentPM.Transshipment2ToPortCode == myPortCode)
                {
                    if (!string.IsNullOrEmpty(myTimeType))
                    {
                        switch (myTimeType)
                        {
                            case "A": { shipmentPM.Transshipment2ATA = myDateTime; break; }
                            case "E": { shipmentPM.Transshipment2ETA = myDateTime; shipmentPM.Transshipment2STA = myDateTime; break; }
                            case "S": { shipmentPM.Transshipment2STA = myDateTime; break; }
                        }
                    }

                    else
                    {
                        shipmentPM.Transshipment2ATA = myEventDate;
                    }
                }

                else if (shipmentPM.Transshipment3ToPortCode == myPortCode)
                {
                    if (!string.IsNullOrEmpty(myTimeType))
                    {
                        switch (myTimeType)
                        {
                            case "A": { shipmentPM.Transshipment3ATA = myDateTime; break; }
                            case "E": { shipmentPM.Transshipment3ETA = myDateTime; shipmentPM.Transshipment3STA = myDateTime; break; }
                            case "S": { shipmentPM.Transshipment3STA = myDateTime; break; }
                        }
                    }

                    else
                    {
                        shipmentPM.Transshipment3ATA = myEventDate;
                    }
                }

                else if (shipmentPM.PreCarriageToPortCode == myPortCode)
                {
                    if (!string.IsNullOrEmpty(myTimeType))
                    {
                        switch (myTimeType)
                        {
                            case "A": { shipmentPM.PreCarriageATA = myDateTime; break; }
                            case "E": { shipmentPM.PreCarriageETA = myDateTime; break; }
                        }
                    }

                    else
                    {
                        shipmentPM.PreCarriageATA = myEventDate;
                    }
                }

                else if (shipmentPM.OnCarriageToPortCode == myPortCode)
                {
                    if (!string.IsNullOrEmpty(myTimeType))
                    {
                        switch (myTimeType)
                        {
                            case "A": { shipmentPM.OnCarriageATA = myDateTime; break; }
                            case "E": { shipmentPM.OnCarriageETA = myDateTime; break; }
                        }
                    }

                    else
                    {
                        shipmentPM.OnCarriageATA = myEventDate;
                    }
                }
            }
        }

        private void CreateCarrierStatus(StatusParams statusParams)
        {
            PortRepository portRepository = new PortRepository(statusParams.CommonContext);
            Port fromPort = portRepository.GetAirlinePortByCode(statusParams.Tenant, statusParams.FromPortCode, true);

            if (fromPort == null)
            {
                Port portZero = portRepository.GetPortsByNameOrCode(statusParams.FromPortCode, null, 0).Where(a => a.IsAir).FirstOrDefault();
                if (portZero != null)
                {
                    fromPort = this.GetPortCopyToCurrentTenant(portZero.Id, statusParams.Tenant);
                }
            }

            Port toPort = portRepository.GetAirlinePortByCode(statusParams.Tenant, statusParams.ToPortCode, true);
            if (toPort == null)
            {
                Port portZero = portRepository.GetPortsByNameOrCode(statusParams.ToPortCode, null, 0).Where(a => a.IsAir).FirstOrDefault();
                if (portZero != null)
                {
                    toPort = this.GetPortCopyToCurrentTenant(portZero.Id, statusParams.Tenant);
                }
            }

            Port locationPort = portRepository.GetAirlinePortByCode(statusParams.Tenant, statusParams.Location, true);
            ShipmentCarrierStatusRepository reposioty = new ShipmentCarrierStatusRepository(statusParams.Tenant);

            string recordInfo = statusParams.Tenant.ToString() + (fromPort != null ? fromPort.Id : null) + (toPort != null ? toPort.Id : null) + statusParams.AirlineName + statusParams.Details + statusParams.StatusCode + statusParams.FlightNumber + statusParams.Partial + statusParams.Pieces + statusParams.Weight + statusParams.EntityId;
            string recordHash = GetHashedData(recordInfo);
            double d;
            d = Convert.ToDouble(statusParams.Weight);
            if (!reposioty.DoesRecordExist(recordHash))
            {
                ShipmentCarrierStatus status = new ShipmentCarrierStatus()
                {
                    Id = IdCounter.GetNumber("ShipmentCarrierStatus", statusParams.Tenant),
                    Status = statusParams.StatusCode,
                    Details = statusParams.Details,
                    EventDate = statusParams.EventDate,
                    FlightNumber = statusParams.FlightNumber,
                    FromPortId = fromPort != null ? fromPort.Id : null,
                    ToPortId = toPort != null ? toPort.Id : null,
                    Partial = statusParams.Partial,
                    Pieces = statusParams.Pieces,
                    Weight = d,
                    ReceivingDate = statusParams.LogDate,
                    ShipmentId = statusParams.EntityId,
                    Tenant = statusParams.Tenant,
                    RecordHash = recordHash,
                    Location = locationPort != null ? locationPort.Id : null,
                    AirlineName = statusParams.AirlineName,
                    DepartureDate = statusParams.DepartureDate,
                    ArrivalDate = statusParams.ArrivalDate,
                    TimeOfArrivalInfo = statusParams.TimeOfArrivalInfo,
                    TimeOfDepartureInfo = statusParams.TimeOfDepartureInfo,
                };

                reposioty.Add(status);
                reposioty.SubmitChanges();
            }
        }

        public Port GetPortCopyToCurrentTenant(string entityId, int tenant)
        {

            ICommonDataContext objectContext = CommonDataContext.GetContext(tenant);

            PortRepository portRepository = new PortRepository(objectContext);
            CountryRepository countryRepository = new CountryRepository(objectContext);
            GlobalZoneRepository globalZoneRepository = new GlobalZoneRepository(objectContext);

            Port newPort;
            Port port = portRepository.GetSinglePort(0, entityId);
            newPort = portRepository.GetAirlineSinglePortByCodeCountryCode(tenant, port.Code, port.Country.Code, false);
            Country country = null;

            if (newPort == null)
            {
                country = countryRepository.GetSingleCountryByCode(port.Country.Code, tenant, false);
                
                if (country == null)
                {
                    GlobalZone globalzone = globalZoneRepository.GetSingleGlobalZoneByCode(port.Country.GlobalZone.Code, tenant);

                    if (globalzone == null)
                    {
                        GlobalZone oldZone = globalZoneRepository.GetSingleGlobalZone(port.Country.GlobalZoneId, 0);
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

                        globalZoneRepository.Add(globalzone);
                        globalZoneRepository.SubmitChanges();
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

                    countryRepository.Add(country);
                    countryRepository.SubmitChanges();
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

                portRepository.Add(newPort);
                portRepository.SubmitChanges();
                RunStoredProcedureClass.UpdatePortSearcsFields(newPort.Id, newPort.Tenant);

            }

            if (country == null)
            {
                country = countryRepository.GetSingleCountryByCode(port.Country.Code, tenant, false);
            }


            return newPort;
        }
        public static double? CalculateChargeableWeight(double? grossWeight, double? volumetricWeight, string grossWeightUnitCode, string chargeableWeightUnitCode, string directionId, string transportModeId)
        {
            if (grossWeight == null && volumetricWeight == null)
            {
                return null;
            }

            double? result = null;
            double? GrossWeightInKG = null;
            double? grossWeightInCh = null;
            if (grossWeight != null)
            {
                GrossWeightInKG = ConvertToWeightUnit(grossWeightUnitCode, "Set", grossWeight);
                grossWeightInCh = ConvertToWeightUnit(chargeableWeightUnitCode, "Get", GrossWeightInKG);
            }

            if (grossWeightInCh != null && volumetricWeight == null)
            {
                result = grossWeightInCh;
            }

            if (grossWeightInCh == null && volumetricWeight != null)
            {
                result = volumetricWeight;
            }

            if (grossWeightInCh != null && volumetricWeight != null)
            {
                result = grossWeightInCh > volumetricWeight ? grossWeightInCh : volumetricWeight;
            }

            return Round(result, chargeableWeightUnitCode, directionId, transportModeId);
        }
        public static double? Round(double? args, string chargeableWeightUnitCode, string directionId, string transportModeId)
        {
            double? result = args;

            if (chargeableWeightUnitCode != "MT")
            {
                if (directionId == "E" && transportModeId == "A")
                {
                    if (result != null)
                    {
                        string toString = result.ToString();
                        string[] r = toString.Split('.');

                        if (r.Length > 1)
                        {
                            string strDigits = "0." + r[1];
                            double? digits = Convert.ToDouble(strDigits);
                            double? integer = Convert.ToDouble(r[0]);

                            if (digits < 0.5)
                            {
                                result = integer + 0.5;
                            }

                            else
                            {
                                result = integer + 1;
                            }
                        }
                    }
                }
            }

            return result;
        }
        private static double? ConvertToWeightUnit(string weightUnitCode, string operation, double? value)
        {
            double? result = 0;
            if (value == null || value == 0)
            {
                return value;
            }

            else
            {
                if (weightUnitCode.ToUpper() == "KG")
                {
                    result = value;
                }

                else if (weightUnitCode.ToUpper() == "LB")
                {
                    if (operation == "Get")
                    {
                        result = (value * 2.20458);
                    }

                    else if (operation == "Set")
                    {
                        result = value / 2.20458;
                    }
                }

                else if (weightUnitCode.ToUpper() == "MT")
                {
                    if (operation == "Get")
                    {
                        result = (value / 1000);
                    }

                    else if (operation == "Set")
                    {
                        result = value * 1000;
                    }
                }
            }

            string str = String.Format("{0:0.000}", result);
            result = Convert.ToDouble(str);

            decimal decimalResult = Convert.ToDecimal(result);
            decimal round = (decimal)MethodHelper.Round(decimalResult, 3);
            return Convert.ToDouble(round);
        }
        public int GetMonthInNumbers(string month)
        {
            int monthInNumber = 1;
            switch (month)
            {
                case "JAN":
                    {
                        monthInNumber = 1;
                        break;
                    }
                case "FEB":
                    {
                        monthInNumber = 2;
                        break;
                    }
                case "MAR":
                    {
                        monthInNumber = 3;
                        break;
                    }
                case "APR":
                    {
                        monthInNumber = 4;
                        break;
                    }
                case "MAY":
                    {
                        monthInNumber = 5;
                        break;
                    }
                case "JUN":
                    {
                        monthInNumber = 6;
                        break;
                    }
                case "JUL":
                    {
                        monthInNumber = 7;
                        break;
                    }
                case "AUG":
                    {
                        monthInNumber = 8;
                        break;
                    }
                case "SEP":
                    {
                        monthInNumber = 9;
                        break;
                    }
                case "OCT":
                    {
                        monthInNumber = 10;
                        break;
                    }
                case "NOV":
                    {
                        monthInNumber = 11;
                        break;
                    }
                case "DEC":
                    {
                        monthInNumber = 12;
                        break;
                    }
            }
            return monthInNumber;
        }
        private int ConvertStringToInteger(string myString)
        {
            int myResult = 0;

            if (!string.IsNullOrEmpty(myString))
            {
                Int32.TryParse(myString, out myResult);
            }

            return myResult;
        }
        public string GetHashedData(string information)
        {
            byte[] byteRepresentation = UnicodeEncoding.UTF8.GetBytes(information);
            byte[] hashedTextInBytes = null;
            MD5CryptoServiceProvider myMd5 = new MD5CryptoServiceProvider();
            hashedTextInBytes = myMd5.ComputeHash(byteRepresentation);
            string hashedText = Convert.ToBase64String(hashedTextInBytes);

            return hashedText;

        }

        private void TraceEachStatus(StatusContext myStatusContext, string entityId)
        {
            string myStatusCode = myStatusContext.StatusCode.Trim();
            string notes = "";

            switch (myStatusCode)
            {
                case "ARR":
                    {
                        notes = myStatusContext.NumberOfPieces + " piece(s) " + myStatusContext.Weight + " Kg arrived in " + myStatusContext.Location + " from flight " + myStatusContext.FlightNumber;
                        break;
                    }
                case "AWD":
                    {
                        notes = "Arrival Documents Delivered for flight " + myStatusContext.FlightNumber;
                        break;
                    }
                case "AWR":
                    {
                        notes = "Arrival Documents Received from flight " + myStatusContext.FlightNumber;
                        break;
                    }
                case "BKD":
                    {
                        notes = "Flight " + myStatusContext.FlightNumber + " booked. " + myStatusContext.NumberOfPieces + " piece(s) " + myStatusContext.Weight + " Kg from " + myStatusContext.CodeOfDeparture + " to " + myStatusContext.CodeOfArrival;
                        break;
                    }
                case "CCD":
                    {
                        notes = "Flight " + myStatusContext.FlightNumber + " cleared by customs";
                        break;
                    }
                case "CRC":
                    {
                        notes = "Flight " + myStatusContext.FlightNumber + " reported by customs";
                        break;
                    }
                case "DDL":
                    {
                        notes = "Door-delivery to consignee for flight " + myStatusContext.FlightNumber;
                        break;
                    }
                case "DEP":
                    {
                        notes = myStatusContext.NumberOfPieces + " piece(s) " + myStatusContext.Weight + " Kg departed on fligh " + myStatusContext.FlightNumber + " from " + myStatusContext.CodeOfDeparture + " to " + myStatusContext.CodeOfArrival + " on " + myStatusContext.EventDate;
                        break;
                    }
                case "DIS":
                    {
                        notes = "Discrepancy for flight " + myStatusContext.FlightNumber;
                        break;
                    }
                case "DLV":
                    {
                        notes = myStatusContext.NumberOfPieces + " piece(s) " + myStatusContext.Weight + " Kg delivered in " + myStatusContext.Location + " from flight " + myStatusContext.FlightNumber;
                        break;
                    }
                case "DOC":
                    {
                        notes = "Documents Received for flight " + myStatusContext.FlightNumber;
                        break;
                    }
                case "FOH":
                    {
                        notes = myStatusContext.NumberOfPieces + " piece(s) " + myStatusContext.Weight + " Kg On-Hand in " + myStatusContext.Location + " for flight " + myStatusContext.FlightNumber;
                        break;
                    }
                case "MAN":
                    {
                        notes = myStatusContext.NumberOfPieces + " piece(s) " + myStatusContext.Weight + " Kg manifested on flight " + myStatusContext.FlightNumber + " from " + myStatusContext.CodeOfDeparture + " to " + myStatusContext.CodeOfArrival + " on " + myStatusContext.EventDate;
                        break;
                    }
                case "NFD":
                    {
                        notes = "Notify about arrival for flight " + myStatusContext.FlightNumber;
                        break;
                    }
                case "PRE":
                    {
                        notes = "Flight " + myStatusContext.FlightNumber + " in preparation";
                        break;
                    }
                case "RCF":
                    {
                        notes = myStatusContext.NumberOfPieces + " piece(s) " + myStatusContext.Weight + " Kg received from flight in " + myStatusContext.Location + " from flight " + myStatusContext.FlightNumber;
                        break;
                    }
                case "RCS":
                    {
                        notes = myStatusContext.NumberOfPieces + " piece(s) " + myStatusContext.Weight + " Kg received from shipper in " + myStatusContext.Location + " for flight " + myStatusContext.FlightNumber;
                        break;
                    }
                case "RCT":
                    {
                        notes = myStatusContext.NumberOfPieces + " piece(s) " + myStatusContext.Weight + " Kg received from transfer in " + myStatusContext.Location + " from flight " + myStatusContext.FlightNumber;
                        break;
                    }
                case "TFD":
                    {
                        notes = myStatusContext.NumberOfPieces + " piece(s) " + myStatusContext.Weight + " Kg transfered in " + myStatusContext.Location + " for flight " + myStatusContext.FlightNumber;
                        break;
                    }
                case "TGC":
                    {
                        notes = "Consigement tranferred to customs/government control for flight " + myStatusContext.FlightNumber;
                        break;
                    }
                case "TRM":
                    {
                        notes = myStatusContext.NumberOfPieces + " piece(s) " + myStatusContext.Weight + " Kg to be transfered from " + myStatusContext.Location + " for flight " + myStatusContext.FlightNumber;
                        break;
                    }
            }

            if (myStatusContext.DepartureDate != null)
            {
                if (!string.IsNullOrEmpty(notes))
                {
                    notes = notes + Environment.NewLine;
                }

                notes = notes + "Scheduled time of flight Departure: " + myStatusContext.DepartureDate.Value.ToShortTimeString();
            }

            if (myStatusContext.ArrivalDate != null)
            {
                if (!string.IsNullOrEmpty(notes))
                {
                    notes = notes + Environment.NewLine;
                }

                notes = notes + "Scheduled time of flight Arrival: " + myStatusContext.ArrivalDate.Value.ToShortTimeString();
            }

            EventTracerArgs myEventArgs = new EventTracerArgs()
            {
                Tenant = myTenant,
                EventTypeCode = myStatusCode + "E",
                UserId = null,
                EntityId = entityId,
                ObjectTableName = "Shipment",
                Notes = notes,
            };

            EventTracer.CreateTraceEvent(myEventArgs);
        }
    }
}
