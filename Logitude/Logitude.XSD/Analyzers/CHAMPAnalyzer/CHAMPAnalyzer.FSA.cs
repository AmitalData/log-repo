using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.Tools.EntityService;
using Logitude.BookingLib.BL.EntityPMs;
using Logitude.BookingLib.BL.EntityUpdateServices;
using Logitude.BookingLib.Data;
using Logitude.BookingLib.Data.EntityPOCOs;
using Logitude.BookingLib.Data.Repositories;
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
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.XSD.Analyzers.CHAMPAnalyzer
{
    public partial class CHAMPAnalyzer
    {
        private CHAMP17.StatusAnswer myFSA;
        private CHAMP17.SplitConsignment mySplitConsignment;
        private void AnalyzeBaseData_FSA()
        {          
            this.myFSA = (CHAMP17.StatusAnswer)myEnvelope.Item;
            this.mySplitConsignment = myFSA.SplitConsignment[0];            
            this.myPrefix = mySplitConsignment.MasterAWBConsignmentDetail.AWBIdentification.AirlinePrefix;
            this.myMaster = mySplitConsignment.MasterAWBConsignmentDetail.AWBIdentification.AWBSerialNumber;

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

            if (!string.IsNullOrEmpty(myPrefix) && !string.IsNullOrEmpty(myMaster))
            {
                myLongMaster = myPrefix + "-" + myMaster;
            }
        }

        private void AnalyzeMessageQueue_FSA(ShipmentPM entityPM, IShipmentsContext myContext)
        {
            entityPM.IsUpdatedByChampAnalyzer = true;

            PortRepository portRepository = new PortRepository(iCommonContext);
            //DocumentRepository documentrepository = new DocumentRepository(myCommonContext);

            string myFromPortCode = null;
            string myToPortCode = null;

            if (mySplitConsignment.MasterAWBConsignmentDetail != null)
            {
                #region AWBOriginAndDestination
                if (mySplitConsignment.MasterAWBConsignmentDetail.AWBOriginAndDestination != null)
                {
                    myFromPortCode = mySplitConsignment.MasterAWBConsignmentDetail.AWBOriginAndDestination.AirportCityCodeOfOrigin;
                    myToPortCode = mySplitConsignment.MasterAWBConsignmentDetail.AWBOriginAndDestination.AirportCityCodeOfDestination;

                    if (entityPM.MainCarriageFromPortCode == "---")
                    {
                        Port newFromPort = portRepository.GetPortsByNameOrCode(myFromPortCode, null, myTenant).Where(d => d.IsAir).FirstOrDefault();
                        if (newFromPort == null)
                        {
                            Port portZero = portRepository.GetPortsByNameOrCode(myFromPortCode, null, 0).Where(d => d.IsAir).FirstOrDefault();
                            if (portZero != null)
                            {
                                newFromPort = this.GetPortCopyToCurrentTenant(portZero.Id, myTenant);
                            }
                        }

                        entityPM.MainCarriageFromPortId = newFromPort != null ? newFromPort.Id : null;
                        //entityPM.IsMappingXSDFields = false;
                    }

                    if (entityPM.MainCarriageToPortCode == "---")
                    {
                        Port newToPort = portRepository.GetPortsByNameOrCode(myToPortCode, null, myTenant).Where(d => d.IsAir).FirstOrDefault();
                        if (newToPort == null)
                        {
                            Port portZero = portRepository.GetPortsByNameOrCode(myToPortCode, null, 0).Where(d => d.IsAir).FirstOrDefault();
                            if (portZero != null)
                            {
                                newToPort = this.GetPortCopyToCurrentTenant(portZero.Id, myTenant);
                            }
                        }
                        entityPM.MainCarriageToPortId = newToPort != null ? newToPort.Id : null;
                        //entityPM.IsMappingXSDFields = false;
                    }
                }
                #endregion

                #region QuantityDetail
                if (mySplitConsignment.MasterAWBConsignmentDetail.QuantityDetail != null)
                {
                    if (entityPM.NumberOfPackages == 0 || entityPM.NumberOfPackages == null)
                    {
                        entityPM.NumberOfPackages = mySplitConsignment.MasterAWBConsignmentDetail.QuantityDetail.NumberOfPieces;
                        //entityPM.IsMappingXSDFields = false;
                    }

                    if (entityPM.GrossWeight == 0 || entityPM.GrossWeight == null)
                    {
                        string weightCode = mySplitConsignment.MasterAWBConsignmentDetail.QuantityDetail.WeightCode;
                        entityPM.GrossWeight = (double)mySplitConsignment.MasterAWBConsignmentDetail.QuantityDetail.Weight;
                        entityPM.GrossWeightUnitCode = weightCode == "L" ? "LB" : "KG";
                        
                        if (weightCode == "L")
                        {
                            entityPM.GrossWeightInKG = entityPM.GrossWeight / 2.20462262;
                        }

                        else
                        {
                            entityPM.GrossWeightInKG = entityPM.GrossWeight;
                        }

                        entityPM.ChargeableWeight = MethodHelper.CalculateChargeableWeight(entityPM.GrossWeight, entityPM.VolumetricWeight, entityPM.GrossWeightUnitCode, entityPM.ChargeableWeightUnitCode, entityPM.DirectionId, entityPM.TransportModeId);
                        //entityPM.IsMappingXSDFields = false;
                    }
                }
                #endregion
            }

            #region OSI
            if (mySplitConsignment.OtherServiceInformation != null)
            {
                string osi = mySplitConsignment.OtherServiceInformation.OSIDetailsFirstLine + Environment.NewLine + mySplitConsignment.OtherServiceInformation.OSIDetailsSecondLine;

                if (!string.IsNullOrEmpty(osi))
                {
                    CreateCarrierStatus(new StatusParams()
                    {
                        Tenant = myTenant,
                        EntityId = entityPM.Id,
                        StatusCode = "OSI",
                        Details = osi,
                        FromPortCode = null,
                        ToPortCode = null,
                        FlightNumber = null,
                        Pieces = 0,
                        Partial = false,
                        Weight = 0,
                        EventDate = null,
                        LogDate = TenantServerConfigration.GetCurrentDateTime(myTenant),
                        Location = null,
                        AirlineName = null,
                        CommonContext = iCommonContext,
                        ShipmentContext = myContext
                    });

                    if (string.IsNullOrEmpty(entityPM.CarrierLastStatusCode))
                    {
                        entityPM.CarrierLastStatusCode = "OSI";
                        entityPM.CarrierLastStatusDate = TenantServerConfigration.GetCurrentDateTime(myTenant);
                    }
                }
            }
            #endregion

            #region Status Detail
            if (mySplitConsignment.StatusDetail != null && mySplitConsignment.StatusDetail.Count() > 0)
            {
                foreach (CHAMP17.StatusDetail item in mySplitConsignment.StatusDetail)
                {
                    StatusContext myStatusContext = new StatusContext()
                    {
                        LogDate = TenantServerConfigration.GetCurrentDateTime(myTenant),
                        StatusCode = item.StatusCode,
                        Details = item.StatusCode,
                    };

                    AnalyzeStatus(myStatusContext, item.MovementDetail, entityPM);
                    AnalyzeStatus(myStatusContext, item.QuantityDetail);
                    AnalyzeStatus(myStatusContext, item, "ARR");
                    AnalyzeStatus(myStatusContext, item, "DEP");
                    UpdateLastStatus(myStatusContext, entityPM);

                    switch (item.StatusCode)
                    {
                        case "DIS":
                            {
                                myStatusContext.Details = this.GetStatusDetails(item.StatusCode, item.DiscrepancyCode);
                                break;
                            }

                        case "ARR":
                            {
                                if (myStatusContext.ArrivalDate != null)
                                {
                                    UpdateArrivalDates(myStatusContext, myToPortCode, entityPM);
                                }

                                break;
                            }

                        case "DEP":
                            {
                                if (myStatusContext.DepartureDate != null)
                                {
                                    UpdateDepartureDates(myStatusContext, myFromPortCode, entityPM);
                                }

                                break;
                            }
                    }

                    if (!string.IsNullOrEmpty(myStatusContext.StatusCode))
                    {
                        CreateCarrierStatus(new StatusParams()
                        {
                            Tenant = myTenant,
                            AirlineName = entityPM.MainCarriageCarrierName,
                            CommonContext = iCommonContext,
                            ShipmentContext = myContext,
                            EntityId = entityPM.Id,
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

            entityPM.IsFSRSent = false;
            string systemEmail = "system@tenant" + myTenant + ".com";

            ShipmentService service = new ShipmentService(myContext, entityPM, systemEmail);
            service.Update();
        }
        private void AnalyzeMessageQueue_FSA(BookingPM entityPM, IBookingContext myContext, AnalyzeQueue analyzeQueue)
        {
            PortRepository portRepository = new PortRepository(iCommonContext);
            DocumentRepository documentrepository = new DocumentRepository(iCommonContext);
            BookingAnswerRepository bookingAnswerRepository = new BookingAnswerRepository(myContext);
            AirlineRepository airlineRepository = new AirlineRepository(iCommonContext);

            entityPM.FMAAcknowledgementReason = null;
            entityPM.FNAReason = null;
            entityPM.AnswerOtherServicesInformation = null;
            entityPM.FFRStatusDate = TenantServerConfigration.GetCurrentDateTime(myTenant);

            List<BookingAnswer> bookingAnswers = bookingAnswerRepository.GetBookingAnswersForBookingTenant(entityPM.Id, myTenant).ToList();
            foreach (BookingAnswer item in bookingAnswers)
            {
                bookingAnswerRepository.Remove(item);
            }

            entityPM.HasResponse = true;
            entityPM.HasErrors = false;
            entityPM.WaitingForResponse = false;
            entityPM.IsFSU = true; 

            if (myMessageIdentifier == "FSU")
            {
                entityPM.BookingStatusCode = "CNF";
                entityPM.FFRStatusCode = "BCN";
            }                       

            string myFromPortCode = null;
            string myToPortCode = null;

            if (mySplitConsignment.MasterAWBConsignmentDetail != null)
            {
                #region AWBOriginAndDestination
                if (mySplitConsignment.MasterAWBConsignmentDetail.AWBOriginAndDestination != null)
                {
                    myFromPortCode = mySplitConsignment.MasterAWBConsignmentDetail.AWBOriginAndDestination.AirportCityCodeOfOrigin;
                    myToPortCode = mySplitConsignment.MasterAWBConsignmentDetail.AWBOriginAndDestination.AirportCityCodeOfDestination;

                    if (entityPM.MainFromPortCode == "---")
                    {
                        Port newFromPort = portRepository.GetPortsByNameOrCode(myFromPortCode, null, myTenant).Where(d => d.IsAir).FirstOrDefault();
                        if (newFromPort == null)
                        {
                            Port portZero = portRepository.GetPortsByNameOrCode(myFromPortCode, null, 0).Where(d => d.IsAir).FirstOrDefault();
                            if (portZero != null)
                            {
                                newFromPort = this.GetPortCopyToCurrentTenant(portZero.Id, myTenant);
                            }
                        }
                        entityPM.MainCarriageFromPortId = newFromPort != null ? newFromPort.Id : null;
                    }

                    if (entityPM.MainToPortCode == "---")
                    {
                        Port newToPort = portRepository.GetPortsByNameOrCode(myToPortCode, null, myTenant).Where(d => d.IsAir).FirstOrDefault();
                        if (newToPort == null)
                        {
                            Port portZero = portRepository.GetPortsByNameOrCode(myToPortCode, null, 0).Where(d => d.IsAir).FirstOrDefault();
                            if (portZero != null)
                            {
                                newToPort = this.GetPortCopyToCurrentTenant(portZero.Id, myTenant);
                            }
                        }
                        entityPM.MainCarriageToPortId = newToPort != null ? newToPort.Id : null;
                    }
                }
                #endregion

                #region QuantityDetail
                if (mySplitConsignment.MasterAWBConsignmentDetail.QuantityDetail != null)
                {
                    if (entityPM.NumberOfPackages == 0 || entityPM.NumberOfPackages == null)
                    {
                        entityPM.NumberOfPackages = mySplitConsignment.MasterAWBConsignmentDetail.QuantityDetail.NumberOfPieces;
                    }

                    if (entityPM.GrossWeight == 0 || entityPM.GrossWeight == null)
                    {
                        string weightCode = mySplitConsignment.MasterAWBConsignmentDetail.QuantityDetail.WeightCode;
                        entityPM.GrossWeight = mySplitConsignment.MasterAWBConsignmentDetail.QuantityDetail.Weight;
                        entityPM.GrossWeightUnitCode = weightCode == "L" ? "LB" : "KG";

                        if (weightCode == "L")
                        {
                            entityPM.GrossWeightInKG = DoubleToDecimal((double)entityPM.GrossWeight / 2.20462262);
                        }

                        else
                        {
                            entityPM.GrossWeightInKG = entityPM.GrossWeight;
                        }

                        entityPM.ChargeableWeight = DoubleToDecimal(MethodHelper.CalculateChargeableWeight(entityPM.GrossWeight, entityPM.VolumetricWeight, entityPM.GrossWeightUnitCode, entityPM.ChargeableWeightUnitCode, entityPM.DirectionCode, entityPM.TransportModeCode));
                    }
                }
                #endregion
            }

            #region OSI
            if (mySplitConsignment.OtherServiceInformation != null)
            {
                if (!string.IsNullOrEmpty(mySplitConsignment.OtherServiceInformation.OSIDetailsFirstLine))
                {
                    entityPM.AnswerOtherServicesInformation = mySplitConsignment.OtherServiceInformation.OSIDetailsFirstLine;
                }

                if (!string.IsNullOrEmpty(mySplitConsignment.OtherServiceInformation.OSIDetailsSecondLine))
                {
                    entityPM.AnswerOtherServicesInformation = entityPM.AnswerOtherServicesInformation +  " " + mySplitConsignment.OtherServiceInformation.OSIDetailsSecondLine;
                }

                //string osi = mySplitConsignment.OtherServiceInformation.OSIDetailsFirstLine + Environment.NewLine + mySplitConsignment.OtherServiceInformation.OSIDetailsSecondLine;

                //if (!string.IsNullOrEmpty(osi))
                //{
                //    CreateCarrierStatus(new StatusParams()
                //    {
                //        Tenant = myTenant,
                //        EntityId = entityPM.Id,
                //        StatusCode = "OSI",
                //        Details = osi,
                //        FromPortCode = null,
                //        ToPortCode = null,
                //        FlightNumber = null,
                //        Pieces = 0,
                //        Partial = false,
                //        Weight = 0,
                //        EventDate = null,
                //        LogDate = TenantServerConfigration.GetCurrentDateTime(myTenant),
                //        Location = null,
                //        AirlineName = null,
                //        CommonContext = myCommonContext,
                //        ShipmentContext = null,
                //        BookingContext = myContext,
                //    });
                //}
            }
            #endregion

            #region Answer Detail
            if (mySplitConsignment.StatusDetail != null && mySplitConsignment.StatusDetail.Count() > 0)
            {
                foreach (CHAMP17.StatusDetail item in mySplitConsignment.StatusDetail)
                {
                    if (item.StatusCode == "BOK" || item.StatusCode == "BKD")
                    {
                        StatusContext myStatusContext = new StatusContext()
                        {
                            LogDate = TenantServerConfigration.GetCurrentDateTime(myTenant),
                            StatusCode = item.StatusCode,
                            Details = item.StatusCode,
                            //DepartureDate = item.TimeOfDepartureInfo.
                            FlightNumber = entityPM.MainCarriageCarrierPrefix + item.MovementDetail.FlightNumber,
                        };

                        Airline airline = airlineRepository.GetSingleAirlineByCode(item.MovementDetail.CarrierCode, myTenant);

                        BookingAnswer answer = new BookingAnswer()
                        {
                            Id = IdCounter.GetNumber("BookingAnswer", myTenant),
                            Tenant = myTenant,
                            BookingId = entityPM.Id,
                            ETD = entityPM.MainCarriageETD,
                            CreateDate = TenantServerConfigration.GetCurrentDateTime(myTenant),
                            StatusCode = "ACC",
                            Origin = myFromPortCode,
                            Destination = myToPortCode,
                            CommunicationLogId = analyzeQueue.CommunicationLogId,
                            FlightNumber = myStatusContext.FlightNumber,
                            BookingSpaceAllocationCode = "KK",
                            CarrierId = airline == null ? null : airline.Id,
                            OtherServicesInformation = item.OtherServiceInformation == null ? null : item.OtherServiceInformation.OSIDetailsFirstLine + item.OtherServiceInformation.OSIDetailsSecondLine,
                            //DescriptionOfGoods = item.QuantityDetail. ,
                            NumberOfPieces = item.QuantityDetail.NumberOfPieces,
                            Weight = item.QuantityDetail.Weight,
                            WeightUnitCode = item.QuantityDetail.WeightCode,
                        };

                        bookingAnswerRepository.Add(answer);
                    }
                }
            }
            #endregion

            string systemEmail = "system@tenant" + myTenant + ".com";

            entityPM.IsUpdatedByChampAnalyzer = true;
            entityPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
            BookingUpdateService service = new BookingUpdateService(myContext, new Dictionary<string, IContext>(), myTenant);
            service.Update(entityPM, true);
        }

        private void AnalyzeStatus(StatusContext myStatusContext, CHAMP17.StatusDetail item, string timeCode)
        {
            switch (timeCode)
            {
                case "ARR":
                    {
                        if (item.TimeOfArrivalInfo != null)
                        {
                            myStatusContext.TimeOfArrivalInfo = item.TimeOfArrivalInfo.TypeOfTimeIndicator;

                            int arrivalHour = item.TimeOfArrivalInfo.Time / 100;
                            int arrivalMinute = item.TimeOfArrivalInfo.Time % 100;
                            int dayIndicator = GetDayChangeIndicator(item.TimeOfArrivalInfo.DayChangeIndicator);
                            DateTime currentdate = myStatusContext.EventDate != null ? myStatusContext.EventDate.Value.AddDays(dayIndicator) : myStatusContext.LogDate.AddDays(dayIndicator);

                            myStatusContext.ArrivalDate = new DateTime(currentdate.Year, currentdate.Month, currentdate.Day, arrivalHour, arrivalMinute, 0);
                        }

                        break;
                    }

                case "DEP":
                    {
                        if (item.TimeOfDepartureInfo != null)
                        {
                            myStatusContext.TimeOfDepartureInfo = item.TimeOfDepartureInfo.TypeOfTimeIndicator;

                            int departureHour = item.TimeOfDepartureInfo.Time / 100;
                            int departureMinute = item.TimeOfDepartureInfo.Time % 100;
                            int dayIndicator = GetDayChangeIndicator(item.TimeOfDepartureInfo.DayChangeIndicator);
                            DateTime currentdate = myStatusContext.EventDate != null ? myStatusContext.EventDate.Value.AddDays(dayIndicator) : myStatusContext.LogDate.AddDays(dayIndicator);

                            myStatusContext.DepartureDate = new DateTime(currentdate.Year, currentdate.Month, currentdate.Day, departureHour, departureMinute, 0);
                        }

                        break;
                    }
            }
        }
        private void AnalyzeStatus(StatusContext myStatusContext, CHAMP17.MovementDetail myMovementDetail, ShipmentPM shipmentPM)
        {
            if (myMovementDetail != null)
            {
                int day = myMovementDetail.Day;
                int month = !string.IsNullOrEmpty(myMovementDetail.Month) ? GetMonthInNumbers(myMovementDetail.Month) : 0;
                int hour = myMovementDetail.ActualTime / 100;
                int minute = myMovementDetail.ActualTime % 100;

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
                    myStatusContext.EventDate = new DateTime(year, month, day, hour, minute, 0);
                }

                myStatusContext.FlightNumber = myMovementDetail.FlightNumber;
                myStatusContext.Location = this.GetMovementLocation(myStatusContext.StatusCode, myMovementDetail);
                myStatusContext.CodeOfArrival = myMovementDetail.AirportCityCodeOfArrival;
                myStatusContext.CodeOfDeparture = myMovementDetail.AirportCityCodeOfDeparture;

                if (myStatusContext.StatusCode != null)
                {
                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = myTenant,
                        EventTypeCode = "FSAR",
                        UserId = null,
                        EntityId = shipmentPM.Id,
                        ObjectTableName = "Shipment",
                        Notes = myStatusContext.StatusCode + " status received" + Environment.NewLine + "Updated by FSA/FSU",
                    });                    
                }
            }
        }
        private void AnalyzeStatus(StatusContext statusContext, CHAMP17.QuantityDetail myQuantityDetail)
        {
            if (myQuantityDetail != null)
            {
                if (myQuantityDetail.ShipmentDescriptionCode == "T")
                {
                    statusContext.IsPartial = false;
                }

                statusContext.Weight = myQuantityDetail.Weight;
                statusContext.NumberOfPieces = myQuantityDetail.NumberOfPieces;
            }
        }
        private void UpdateLastStatus(StatusContext myStatusContext, ShipmentPM shipmentPM)
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
        private void UpdateDepartureDates(StatusContext myStatusContext, string myPortCode, ShipmentPM shipmentPM)
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
        private void UpdateArrivalDates(StatusContext myStatusContext, string myPortCode, ShipmentPM shipmentPM)
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
            Port fromPort = portRepository.GetSinglePortByCode(statusParams.Tenant, statusParams.FromPortCode, true);

            if (fromPort == null)
            {
                Port portZero = portRepository.GetPortsByNameOrCode(statusParams.FromPortCode, null, 0).Where(a => a.IsAir).FirstOrDefault();
                if (portZero != null)
                {
                    fromPort = this.GetPortCopyToCurrentTenant(portZero.Id, statusParams.Tenant);
                }
            }

            Port toPort = portRepository.GetSinglePortByCode(statusParams.Tenant, statusParams.ToPortCode, true);
            if (toPort == null)
            {
                Port portZero = portRepository.GetPortsByNameOrCode(statusParams.ToPortCode, null, 0).Where(a => a.IsAir).FirstOrDefault();
                if (portZero != null)
                {
                    toPort = this.GetPortCopyToCurrentTenant(portZero.Id, statusParams.Tenant);
                }
            }

            Port locationPort = portRepository.GetSinglePortByCode(statusParams.Tenant, statusParams.Location, true);
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
                
        private string GetStatusDetails(string myStatusCode, string myDiscrepancyCode)
        {
            string myResult = myStatusCode;

            if (myStatusCode == "DIS")
            {
                switch (myDiscrepancyCode)
                {
                    case "FDAW":
                        {
                            myResult = "Found Air Waybill";
                            break;
                        }
                    case "FDCA":
                        {
                            myResult = "Found Cargo";
                            break;
                        }
                    case "MSAW":
                        {
                            myResult = "Missing Air Waybill";
                            break;
                        }
                    case "MSCA":
                        {
                            myResult = "Missing Cargo";
                            break;
                        }
                    case "FDAV":
                        {
                            myResult = "Found Mail Document";
                            break;
                        }
                    case "FDMB":
                        {
                            myResult = "Found Mailbag";
                            break;
                        }
                    case "MSAV":
                        {
                            myResult = "Missing Mail Document";
                            break;
                        }
                    case "MSMB":
                        {
                            myResult = "Missing Mailbag";
                            break;
                        }
                    case "DFLD":
                        {
                            myResult = "Definitely Loaded";
                            break;
                        }
                    case "OFLD":
                        {
                            myResult = "Offloaded";
                            break;
                        }
                    case "OVCD":
                        {
                            myResult = "Overcarried";
                            break;
                        }
                    case "SSPD":
                        {
                            myResult = "Shortshipped";
                            break;
                        }
                }
            }

            return myResult;
        }
        private string GetMovementLocation(string myStatusCode, CHAMP17.MovementDetail myMovementDetail)
        {
            string myLocation = myMovementDetail.AirportCityCodeOfDeparture;

            switch (myStatusCode.Trim())
            {
                case "DIS":
                    {
                        myLocation = myMovementDetail.AirportCityCodeOfDeparture != null ? myMovementDetail.AirportCityCodeOfDeparture : myMovementDetail.AirportCityCodeOfArrival;
                        break;
                    }
                case "AWD":
                    {
                        myLocation = myMovementDetail.AirportCityCodeOfDeparture != null ? myMovementDetail.AirportCityCodeOfDeparture : myMovementDetail.AirportCityCodeOfArrival;
                        break;
                    }
                case "AWR":
                    {
                        myLocation = myMovementDetail.AirportCityCodeOfArrival;
                        break;
                    }
                case "NFD":
                    {
                        myLocation = myMovementDetail.AirportCityCodeOfDeparture != null ? myMovementDetail.AirportCityCodeOfDeparture : myMovementDetail.AirportCityCodeOfArrival;
                        break;
                    }
                case "ARR":
                    {
                        myLocation = myMovementDetail.AirportCityCodeOfArrival;
                        break;
                    }
                case "BKD":
                    {
                        myLocation = myMovementDetail.AirportCityCodeOfDeparture;
                        break;
                    }
                case "CCD":
                    {
                        myLocation = myMovementDetail.AirportCityCodeOfDeparture != null ? myMovementDetail.AirportCityCodeOfDeparture : myMovementDetail.AirportCityCodeOfArrival;
                        break;
                    }
                case "TRM":
                    {
                        myLocation = myMovementDetail.AirportCityCodeOfDeparture != null ? myMovementDetail.AirportCityCodeOfDeparture : myMovementDetail.AirportCityCodeOfArrival;
                        break;
                    }
                case "MAN":
                    {
                        myLocation = myMovementDetail.AirportCityCodeOfDeparture;
                        break;
                    }
                case "DLV":
                    {
                        myLocation = myMovementDetail.AirportCityCodeOfDeparture != null ? myMovementDetail.AirportCityCodeOfDeparture : myMovementDetail.AirportCityCodeOfArrival;
                        break;
                    }
                case "DDL":
                    {
                        myLocation = myMovementDetail.AirportCityCodeOfDeparture != null ? myMovementDetail.AirportCityCodeOfDeparture : myMovementDetail.AirportCityCodeOfArrival;
                        break;
                    }
                case "RCF":
                    {
                        myLocation = myMovementDetail.AirportCityCodeOfDeparture;
                        break;
                    }
                case "RCS":
                    {
                        myLocation = myMovementDetail.AirportCityCodeOfDeparture != null ? myMovementDetail.AirportCityCodeOfDeparture : myMovementDetail.AirportCityCodeOfArrival;
                        break;
                    }
                case "RCT":
                    {
                        myLocation = myMovementDetail.AirportCityCodeOfDeparture != null ? myMovementDetail.AirportCityCodeOfDeparture : myMovementDetail.AirportCityCodeOfArrival;
                        break;
                    }
                case "TFD":
                    {
                        myLocation = myMovementDetail.AirportCityCodeOfDeparture != null ? myMovementDetail.AirportCityCodeOfDeparture : myMovementDetail.AirportCityCodeOfArrival;
                        break;
                    }
                case "PRE":
                    {
                        myLocation = myMovementDetail.AirportCityCodeOfDeparture;
                        break;
                    }
                case "CRC":
                    {
                        myLocation = myMovementDetail.AirportCityCodeOfDeparture != null ? myMovementDetail.AirportCityCodeOfDeparture : myMovementDetail.AirportCityCodeOfArrival;
                        break;
                    }
                case "TGC":
                    {
                        myLocation = myMovementDetail.AirportCityCodeOfDeparture != null ? myMovementDetail.AirportCityCodeOfDeparture : myMovementDetail.AirportCityCodeOfArrival;
                        break;
                    }
                case "DEP":
                    {
                        myLocation = myMovementDetail.AirportCityCodeOfDeparture;
                        break;
                    }
                case "FOH":
                    {
                        myLocation = myMovementDetail.AirportCityCodeOfDeparture != null ? myMovementDetail.AirportCityCodeOfDeparture : myMovementDetail.AirportCityCodeOfArrival;
                        break;
                    }
                case "DOC":
                    {
                        myLocation = myMovementDetail.AirportCityCodeOfDeparture;
                        break;
                    }

                default:
                    {
                        myLocation = myMovementDetail.AirportCityCodeOfDeparture;
                        break;
                    }
            }

            return myLocation;
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
        private int GetMonthInNumbers(string month)
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
        private int GetDayChangeIndicator(string indicator)
        {
            switch (indicator)
            {
                case "N": { return 1; }
                case "P": { return -1; }
                case "S": { return 2; }
                case "T": { return 3; }
                case "A": { return 4; }
                case "B": { return 5; }
                case "C": { return 6; }
                case "D": { return 7; }
                case "E": { return 8; }
                case "F": { return 9; }
                case "G": { return 10; }
                case "H": { return 11; }
                case "I": { return 12; }
                case "J": { return 13; }
                case "K": { return 14; }
                case "L": { return 15; }
                default: { return 0; }
            }
        }
        private static decimal? DoubleToDecimal(double? myValue)
        {
            decimal? myResult = null;

            if (myValue != null)
            {
                myResult = Convert.ToDecimal(myValue);
            }

            return myResult;
        }
        private Port GetPortCopyToCurrentTenant(string entityId, int tenant)
        {
            ICommonDataContext objectContext = CommonDataContext.GetContext(tenant);

            PortRepository portRepository = new PortRepository(objectContext);
            CountryRepository countryRepository = new CountryRepository(objectContext);
            GlobalZoneRepository globalZoneRepository = new GlobalZoneRepository(objectContext);

            Port newPort;
            Port port = portRepository.GetSinglePort(0, entityId);
            newPort = portRepository.GetSinglePortByCodeCountryCode(tenant, port.Code, port.Country.Code, false);
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
            }

            if (country == null)
            {
                country = countryRepository.GetSingleCountryByCode(port.Country.Code, tenant, false);
            }

            return newPort;
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
                        notes = myStatusContext.NumberOfPieces + " piece(s) " + myStatusContext.Weight + " Kg received from flight in "  + myStatusContext.Location + " from flight " + myStatusContext.FlightNumber;
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
