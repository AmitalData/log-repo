using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BookingLib.Data.EntityPOCOs;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.EntityChanges;
using Logitude.Server.Tools.Helpers;
using Microsoft.ServiceBus.Messaging;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Logitude.XSD
{
    public class MessagesTransmissionHelper
    {
        private int tenant;
        private string CCS;
        private string MessageTypeCode; //"FHL" : "FWB", ...
        private string FromPortId;
        private string FromPortCode;
        private string ToPortId;
        private string ToPortCode;
        private string AirlineId;
        private string AirlineCode;
        private string AirlinePrefix;
        private string UserId;
        private string UserName;
        private string UserEmail;       
        private int? ForwarderTenant;
        private string ParticipantId;
        private bool IsDirectParticipant;
        private ICommonDataContext myContext;
        private LogitudeMessagesTransmissionLog TransmissionLog;
        private LogitudeMessagesTransmissionLogRepository logRepository;
        public bool IsDEXXCargonaut { get; set; }
        public MessagesTransmissionHelper(int tenant, string messageTypeCode)
        {
            this.tenant = tenant;
            this.MessageTypeCode = messageTypeCode;
            this.myContext = CommonDataContext.GetContext(tenant);           
            this.logRepository = new LogitudeMessagesTransmissionLogRepository(myContext);
        }

        public void Build(ShipmentPM shipment)
        {
            this.AirlineId = shipment.MainCarriageCarrierId;
            this.FromPortId = shipment.MainCarriageFromPortId;
            this.ToPortId = shipment.MainCarriageFinalDestinationPortId;
            this.UserId = shipment.LastSentByUserId;

            this.Initialize();
            this.BuildTransmission();

            DateTime? mySentDate = null;
            switch (this.MessageTypeCode)
            {
                case "FHL":
                    {
                        if (this.IsDEXXCargonaut)
                        {
                            mySentDate = shipment.CargonautFHLStatusDate;
                        }

                        else
                        {
                            mySentDate = shipment.FHLStatusDate;
                        }
                        
                        break;
                    }

                case "FWB":
                    {
                        if (this.IsDEXXCargonaut)
                        {
                            mySentDate = shipment.CargonautFWBStatusDate;
                        }

                        else
                        {
                            mySentDate = shipment.FWBStatusDate;
                        }
                        
                        break;
                    }

                case "FSR":
                    {
                        mySentDate = shipment.LastFSRStatusRequestDate;
                        break;
                    }
            }

            decimal? myVolume = null;
            decimal? myGrossWeight = null;
            decimal? myChargeableWeight = null;

            if (shipment.Volume != null)
            {
                myVolume = (decimal)shipment.Volume;
            }

            if (shipment.GrossWeight != null)
            {
                myGrossWeight = (decimal)shipment.GrossWeight;
            }

            if (shipment.ChargeableWeight != null)
            {
                myChargeableWeight = (decimal)shipment.ChargeableWeight;
            }

            this.TransmissionLog.SentDate = mySentDate;
            this.TransmissionLog.Prefix = shipment.AirlinePrefix;
            this.TransmissionLog.AWBNumber = shipment.AirlinePrefix + "-" + shipment.Master;
            this.TransmissionLog.IATACode = shipment.IssuingCarrierIATACode;
            this.TransmissionLog.CASSCode = shipment.CASSCode;
            this.TransmissionLog.Pieces = shipment.NumberOfPackages;
            this.TransmissionLog.Volume = myVolume;
            this.TransmissionLog.GrossWeight = myGrossWeight;
            this.TransmissionLog.ChargeableWeight = myChargeableWeight;
            this.TransmissionLog.VolumeUnitCode = shipment.VolumeUnitCode;
            this.TransmissionLog.GrossWeightUnitCode = shipment.GrossWeightUnitCode;
            this.TransmissionLog.ChargeableWeightUnitCode = shipment.ChargeableWeightUnitCode;
            this.TransmissionLog.DescriptionOfGoods = shipment.DescriptionOfGoods;
            this.TransmissionLog.SearchFields = this.TransmissionLog.CCS + "," + this.TransmissionLog.MessageTypeCode + "," + this.TransmissionLog.Prefix + "," + shipment.Master + "," + this.TransmissionLog.AWBNumber;

            this.SubmitTransmission();
        }
        public void Build(Shipment shipment, ShipmentMasterData MasterData)
        {
            this.AirlineId = MasterData.MainCarriageCarrierId;
            this.FromPortId = MasterData.MainCarriageFromPortId;
            this.ToPortId = MasterData.MainCarriageFinalDestinationPortId;
            this.UserId = shipment.LastSentByUserId;

            this.Initialize();
            this.BuildTransmission();

            DateTime? mySentDate = null;
            switch (this.MessageTypeCode)
            {
                case "FHL":
                    {
                        if (this.IsDEXXCargonaut)
                        {
                            mySentDate = shipment.CargonautFHLStatusDate;
                        }

                        else
                        {
                            mySentDate = shipment.FHLStatusDate;
                        }

                        break;
                    }

                case "FWB":
                    {
                        if (this.IsDEXXCargonaut)
                        {
                            mySentDate = MasterData.CargonautFWBStatusDate;
                        }

                        else
                        {
                            mySentDate = MasterData.FWBStatusDate;
                        }

                        break;
                    }

                case "FSR":
                    {
                        mySentDate = shipment.LastFSRStatusRequestDate;
                        break;
                    }
            }

            decimal? myVolume = null;
            decimal? myGrossWeight = null;
            decimal? myChargeableWeight = null;

            if (shipment.Volume != null)
            {
                myVolume = (decimal)shipment.Volume;
            }

            if (shipment.GrossWeight != null)
            {
                myGrossWeight = (decimal)shipment.GrossWeight;
            }

            if (shipment.ChargeableWeight != null)
            {
                myChargeableWeight = (decimal)shipment.ChargeableWeight;
            }

            this.TransmissionLog.SentDate = mySentDate;
            this.TransmissionLog.Prefix = MasterData.AirlinePrefix;
            this.TransmissionLog.AWBNumber = MasterData.AirlinePrefix + "-" + MasterData.Master;
            this.TransmissionLog.IATACode = shipment.IssuingCarrierIATACode;
            this.TransmissionLog.CASSCode = shipment.CASSCode;
            this.TransmissionLog.Pieces = shipment.NumberOfPackages;
            this.TransmissionLog.Volume = myVolume;
            this.TransmissionLog.GrossWeight = myGrossWeight;
            this.TransmissionLog.ChargeableWeight = myChargeableWeight;
            this.TransmissionLog.VolumeUnitCode = shipment.VolumeUnitCode;
            this.TransmissionLog.GrossWeightUnitCode = shipment.GrossWeightUnitCode;
            this.TransmissionLog.ChargeableWeightUnitCode = shipment.ChargeableWeightUnitCode;
            this.TransmissionLog.DescriptionOfGoods = shipment.DescriptionOfGoods;
            this.TransmissionLog.SearchFields = this.TransmissionLog.CCS + "," + this.TransmissionLog.MessageTypeCode + "," + this.TransmissionLog.Prefix + "," + MasterData.Master + "," + this.TransmissionLog.AWBNumber;

            this.SubmitTransmission();
        }
        public void Build(BookingLib.Data.EntityPOCOs.Booking booking)
        {
            this.AirlineId = booking.MainCarriageCarrierId;
            this.FromPortId = booking.MainCarriageFromPortId;
            this.ToPortId = booking.MainCarriageFinalDestinationPortId;
            this.UserId = booking.LastSentByUserId;

            this.Initialize();
            this.BuildTransmission();

            DateTime? mySentDate = null;
            switch (this.MessageTypeCode)
            {
                case "FFR": { mySentDate = booking.FFRStatusDate; break; }
                case "FSR": { mySentDate = booking.LastFSRStatusRequestDate; break; }
            }

            decimal? myVolume = null;
            decimal? myGrossWeight = null;
            decimal? myChargeableWeight = null;

            if (booking.Volume != null)
            {
                myVolume = (decimal)booking.Volume;
            }

            if (booking.GrossWeight != null)
            {
                myGrossWeight = (decimal)booking.GrossWeight;
            }

            if (booking.ChargeableWeight != null)
            {
                myChargeableWeight = (decimal)booking.ChargeableWeight;
            }

            this.TransmissionLog.SentDate = mySentDate;
            this.TransmissionLog.Prefix = booking.AirlinePrefix;
            this.TransmissionLog.AWBNumber = booking.AirlinePrefix + "-" + booking.Master;
            this.TransmissionLog.IATACode = booking.IssuingCarrierIATACode;
            this.TransmissionLog.CASSCode = booking.CASSCode;
            this.TransmissionLog.Pieces = booking.NumberOfPackages;
            this.TransmissionLog.Volume = myVolume;
            this.TransmissionLog.GrossWeight = myGrossWeight;
            this.TransmissionLog.ChargeableWeight = myChargeableWeight;
            this.TransmissionLog.VolumeUnitCode = booking.VolumeUnitCode;
            this.TransmissionLog.GrossWeightUnitCode = booking.GrossWeightUnitCode;
            this.TransmissionLog.ChargeableWeightUnitCode = booking.ChargeableWeightUnitCode;
            this.TransmissionLog.DescriptionOfGoods = booking.DescriptionOfGoods;
            this.TransmissionLog.SearchFields = this.TransmissionLog.CCS + "," + this.TransmissionLog.MessageTypeCode + "," + this.TransmissionLog.Prefix + "," + booking.Master + this.TransmissionLog.AWBNumber;

            this.SubmitTransmission();
        }
        public void Build(FlightsSchedulesRequest flightRequest)
        {
            this.AirlineId = flightRequest.AirlineId;
            this.FromPortId = flightRequest.FromPortId;
            this.ToPortId = flightRequest.ToPortId;
            this.UserId = flightRequest.CreatedByUserId;

            this.Initialize();
            this.BuildTransmission();

            this.TransmissionLog.SentDate = flightRequest.CreateDate;
            this.TransmissionLog.Prefix = this.AirlinePrefix;
            this.TransmissionLog.GrossWeight = flightRequest.GrossWeight;
            this.TransmissionLog.GrossWeightUnitCode = flightRequest.GrossWeightUnitCode;
            this.TransmissionLog.Volume = flightRequest.Volume;
            this.TransmissionLog.VolumeUnitCode = flightRequest.VolumeUnitCode;
            this.TransmissionLog.SearchFields = this.TransmissionLog.CCS + "," + this.TransmissionLog.MessageTypeCode + "," + this.TransmissionLog.AirlineCode + "," + this.TransmissionLog.Prefix;

            this.SubmitTransmission();
        }

        private void Initialize()
        {
            PortRepository portRepository = new PortRepository(myContext);
            CardRepository cardRepository = new CardRepository(myContext);
            ContactRepository contactRpeository = new ContactRepository(myContext);

            TenantManagement myTenantManagement = null;
            TenantManagement airlineTenantManagement = null;
            if (!string.IsNullOrEmpty(this.AirlineId))
            {
                Card myCard = cardRepository.GetSingleCard(this.AirlineId, this.tenant);

                if (myCard != null)
                {
                    this.AirlineCode = myCard.Code;

                    if(myCard.Airline != null)
                    {
                        this.AirlinePrefix = myCard.Airline.Prefix;
                    }

                    using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                    {
                        TenantManagementRepository tenantManagementRepository = new TenantManagementRepository();

                        myTenantManagement = tenantManagementRepository.GetSingleTenantManagement(tenant);
                        airlineTenantManagement = tenantManagementRepository.GetTenantManagementByConnectedArline(this.AirlineCode);

                        scope.Complete();
                    }
                }
            }

            if (myTenantManagement != null)
            {
                this.CCS = myTenantManagement.AWBMessagesCCSTypeCode;
            }

            if (airlineTenantManagement != null)
            {
                ParticipantRepository participantRepository = new ParticipantRepository(myContext);
                Participant myParticipant = participantRepository.GetSingleParticipantByForwarderandAirlineTenant(tenant, airlineTenantManagement.Id);

                if (myParticipant != null)
                {
                    this.ParticipantId = myParticipant.Id;
                    this.ForwarderTenant = myParticipant.ForwarderTenant;
                    this.IsDirectParticipant = myParticipant.IsDirect;
                }
            }

            if (!string.IsNullOrEmpty(this.FromPortId))
            {
                Port fromPort = portRepository.GetSinglePort(this.FromPortId, tenant);
                if(fromPort != null)
                {
                    this.FromPortCode = fromPort.Code;
                }
            }

            if (!string.IsNullOrEmpty(this.ToPortId))
            {
                Port toPort = portRepository.GetSinglePort(this.ToPortId, tenant);
                if (toPort != null)
                {
                    this.ToPortCode = toPort.Code;
                }
            }

            if (!string.IsNullOrEmpty(this.UserId))
            {
                Simplog.Data.CommonDataModel.EntityPOCOs.Contact myUser = contactRpeository.GetSingleContact(this.UserId, tenant);
                if(myUser != null)
                {
                    this.UserName = myUser.EnglishName;
                    this.UserEmail = myUser.Email;
                }
            }
        }
        private void BuildTransmission()
        {
            this.TransmissionLog = new LogitudeMessagesTransmissionLog()
            {
                Id = IdCounter.GetNumber("LogitudeMessagesTransmissionLog", tenant),
                Tenant = tenant,
                CCS = this.CCS,
                AirlineCode = this.AirlineCode,
                MessageTypeCode = this.MessageTypeCode,
                DirectParticipant = this.IsDirectParticipant,
                UserName = this.UserName,
                UserEmail = this.UserEmail,
                Origin = this.FromPortCode,
                Destination = this.ToPortCode,
                IsUpdatedinAirlineTenant = false,
                ParticipantId = this.ParticipantId,
                SourceTenant = this.ForwarderTenant,
            };
        }
        private void SubmitTransmission()
        {
            logRepository.Add(TransmissionLog);
            logRepository.SubmitChanges();

            this.AddEntityChange();
            this.SendTransmissionToQueue();
        }
        private void AddEntityChange()
        {
            LogitudeMessagesTransmissionLogQuery query = new LogitudeMessagesTransmissionLogQuery(logRepository);
            LogitudeMessagesTransmissionLogPM myLog = query.GetSinglePM(this.TransmissionLog.Id, tenant);
            if (myLog != null)
            {
                var mainEntityChangeService = new MainEntityChangeService(new EntityChangeArgs() { EntityPM = myLog,  ProcessType = "OnCreate",  ObjectTableName = "LogitudeMessagesTransmissionLog", EntityId = myLog.Id, Tenant = tenant});
                mainEntityChangeService.AddEntityChange();


            }
        }
        private void SendTransmissionToQueue()
        {
            using (TransactionScope scope = TransactionFactory.GetNewSerializableTransaction())
            {
                QueueClient client = ServiceBusQueueHelper.CreateMessagesTransmissionLogQueue(tenant);
                BrokeredMessage message = new BrokeredMessage();
                message.Properties["Tenant"] = tenant;
                message.Properties["TransmissionLogId"] = this.TransmissionLog.Id;
                client.Send(message);
                scope.Complete();
            }
        }
    }
}
