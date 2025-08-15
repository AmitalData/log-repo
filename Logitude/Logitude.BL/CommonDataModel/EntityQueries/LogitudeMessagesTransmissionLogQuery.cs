using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.DataContracts;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Server.Infrastructure.Helpers;

namespace Logitude.BL.CommonDataModel.EntityQueries
{
    public class LogitudeMessagesTransmissionLogQuery
    {
        LogitudeMessagesTransmissionLogRepository repository;

 

        public LogitudeMessagesTransmissionLogQuery(int tenant)
        {
            repository = new LogitudeMessagesTransmissionLogRepository(tenant);
        }

        public LogitudeMessagesTransmissionLogQuery(LogitudeMessagesTransmissionLogRepository repository)
        {
            this.repository = repository;
        }

        public LogitudeMessagesTransmissionLogPM GetSinglePM(string id, int tenant)
        {
            LogitudeMessagesTransmissionLogPM entityPM = (from a in repository.context.LogitudeMessagesTransmissionLogs
                                            where a.Id == id
                                            select new LogitudeMessagesTransmissionLogPM()
                                            {
                                                Id = a.Id,
                                                Tenant = a.Tenant,
                                                CCS = a.CCS,
                                                AirlineCode = a.AirlineCode,
                                                MessageTypeCode = a.MessageTypeCode,
                                                Prefix = a.Prefix,
                                                AWBNumber = a.AWBNumber,
                                                HAWB = a.HAWB,
                                                SentDate = a.SentDate,
                                                Participant = a.Participant,
                                                IATACode = a.IATACode,
                                                CASSCode = a.CASSCode,
                                                UserName = a.UserName,
                                                UserEmail = a.UserEmail,
                                                Origin = a.Origin,
                                                Destination = a.Destination,
                                                Pieces = a.Pieces,
                                                GrossWeight = a.GrossWeight,
                                                GrossWeightUnitCode = a.GrossWeightUnitCode,
                                                ChargeableWeight = a.ChargeableWeight,
                                                ChargeableWeightUnitCode = a.ChargeableWeightUnitCode,
                                                Volume = a.Volume,
                                                VolumeUnitCode = a.VolumeUnitCode,
                                                DescriptionOfGoods = a.DescriptionOfGoods,
                                                DirectParticipant = a.DirectParticipant,
                                                IsUpdatedinAirlineTenant = a.IsUpdatedinAirlineTenant,
                                                SearchFields = a.SearchFields,
                                                ParticipantId = a.ParticipantId,
                                                SourceTenant = a.SourceTenant,
                                            }).FirstOrDefault();

            if (!string.IsNullOrEmpty(entityPM.ParticipantId))
            {
                ParticipantRepository participantRepository = new ParticipantRepository(tenant);
                Participant participant = participantRepository.GetSingleParticipant(entityPM.ParticipantId, tenant);

                if (participant != null)
                {
                    entityPM.FWBNotifyContacts = participant.FWBNotifyContacts;
                    entityPM.FHLNotifyContacts = participant.FHLNotifyContacts;
                    entityPM.FFRNotifyContacts = participant.FFRNotifyContacts;
                }
            }

            return entityPM;
        }

        public IQueryable<LogitudeMessagesTransmissionLogPM> GetLogitudeMessagesTransmissionLogPMsByTenant(int tenant)
        {
            return (from a in repository.context.LogitudeMessagesTransmissionLogs
                    where a.Tenant == tenant
                    select new LogitudeMessagesTransmissionLogPM()
                    {
                        Id = a.Id,
                        Tenant = a.Tenant,
                        CCS = a.CCS,
                        AirlineCode = a.AirlineCode,
                        MessageTypeCode = a.MessageTypeCode,
                        Prefix = a.Prefix,
                        AWBNumber = a.AWBNumber,
                        HAWB = a.HAWB,
                        SentDate = a.SentDate,
                        Participant = a.Participant,
                        IATACode = a.IATACode,
                        CASSCode = a.CASSCode,
                        UserName = a.UserName,
                        UserEmail = a.UserEmail,
                        Origin = a.Origin,
                        Destination = a.Destination,
                        Pieces = a.Pieces,
                        GrossWeight = a.GrossWeight,
                        GrossWeightUnitCode = a.GrossWeightUnitCode,
                        ChargeableWeight = a.ChargeableWeight,
                        ChargeableWeightUnitCode = a.ChargeableWeightUnitCode,
                        Volume = a.Volume,
                        VolumeUnitCode = a.VolumeUnitCode,
                        DescriptionOfGoods = a.DescriptionOfGoods,
                        DirectParticipant = a.DirectParticipant,
                        IsUpdatedinAirlineTenant = a.IsUpdatedinAirlineTenant,
                        SearchFields = a.SearchFields,
                    });
        }

        public IQueryable<LogitudeMessagesTransmissionLogList> GetIQueryableEntityList(IQueryable<LogitudeMessagesTransmissionLog> iQueryable)
        {
            IQueryable<LogitudeMessagesTransmissionLogList> result = from a in iQueryable
                                                      select new LogitudeMessagesTransmissionLogList()
                                                      {
                                                          Id = a.Id,
                                                          Tenant = a.Tenant,
                                                          CCS = a.CCS,
                                                          AirlineCode = a.AirlineCode,
                                                          MessageTypeCode = a.MessageTypeCode,
                                                          Prefix = a.Prefix,
                                                          AWBNumber = a.AWBNumber,
                                                          HAWB = a.HAWB,
                                                          SentDate = a.SentDate,
                                                          Participant = a.Participant,
                                                          IATACode = a.IATACode,
                                                          CASSCode = a.CASSCode,
                                                          UserName = a.UserName,
                                                          UserEmail = a.UserEmail,
                                                          Origin = a.Origin,
                                                          Destination = a.Destination,
                                                          Pieces = a.Pieces,
                                                          GrossWeight = a.GrossWeight,
                                                          GrossWeightUnitCode = a.GrossWeightUnitCode,
                                                          ChargeableWeight = a.ChargeableWeight,
                                                          ChargeableWeightUnitCode = a.ChargeableWeightUnitCode,
                                                          Volume = a.Volume,
                                                          VolumeUnitCode = a.VolumeUnitCode,
                                                          DescriptionOfGoods = a.DescriptionOfGoods,
                                                          DirectParticipant = a.DirectParticipant,
                                                          IsUpdatedinAirlineTenant = a.IsUpdatedinAirlineTenant,
                                                          SearchFields = a.SearchFields,
                                                      };
            return result;
        }

        public List<ChartingDataClass> GetActivityStatusByMessagesLogs(int lastDays, string messageType, int tenant)
        {
            DateTime? todayDate = TenantServerConfigration.GetCurrentDateTime(tenant).Date;
        //    todayDate= todayDate.Value.AddDays((todayDate.Value.Day * -1));
            DateTime? date1 = todayDate.Value.AddDays(lastDays);
            date1 = date1.Value.AddDays((date1.Value.Day * -1)+1);

            List<ChartingDataClass> myResult = (from d in repository.context.LogitudeMessagesTransmissionLogs
                                                where d.Tenant == tenant
                                                && d.MessageTypeCode == messageType
                                                && d.SentDate != null
                                                && System.Data.Entity.DbFunctions.TruncateTime(d.SentDate) >= date1
                                                group d by new
                                                {
                                                     d.SentDate.Value.Month,
                                                     d.SentDate.Value.Year,
                                                } into g
                                                orderby g.Key.Year, g.Key.Month
                                                select new ChartingDataClass()
                                                {
                                                    Id = (g.Key.Month + g.Key.Year).ToString(),
                                                    LabelProperty = g.Key.Month + "-" + g.Key.Year,
                                                    Code = messageType,
                                                    Month = g.Key.Month,
                                                    Year = g.Key.Year,
                                                    IntegerProperty = g.Count()
                                                }).ToList();

            return myResult;
        }
    }
}
