using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Simplog.Data.ShipmentsModel.Repositories;
using Logitude.BL.ShipmentsModel.EntityLists;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Server.Infrastructure.Helpers;

namespace Logitude.BL.ShipmentsModel.EntityQueries
{
    public class ShipmentCustomsTransmissionQuery
    {
        ShipmentCustomsTransmissionRepository repository;

        public ShipmentCustomsTransmissionQuery(int tenant)
        {
            repository = new ShipmentCustomsTransmissionRepository(tenant);
        }

        public ShipmentCustomsTransmissionQuery(ShipmentCustomsTransmissionRepository repository)
        {
            this.repository = repository;
        }

        public ShipmentCustomsTransmissionPM GetSinglePM(string id, int tenant)
        {
            return (from a in repository.Context.ShipmentCustomsTransmissions
                    where a.Id == id && a.Tenant == tenant
                    select new ShipmentCustomsTransmissionPM()
                    {
                        Id = a.Id,
                        Tenant = a.Tenant,
                        ShipmentId = a.ShipmentId,
                        LastSendDate = a.LastSendDate,
                        SentByUserId = a.SentByUserId,
                        CommunicationLogId = a.CommunicationLogId,
                        Status = a.Status,
                        Error = a.Error,
                        MessageCode = a.MessageCode,
                    }).FirstOrDefault();
        }

        public IQueryable<ShipmentCustomsTransmissionPM> GetShipmentCustomsTransmissionPMsByShipmentId(string shipmentId, int tenant)
        {
            return (from a in repository.Context.ShipmentCustomsTransmissions.Include("CustomsTransmissionsStatus").Include("SendByUser.Contact")
                    where a.ShipmentId == shipmentId && a.Tenant == tenant
                    select new ShipmentCustomsTransmissionPM()
                    {
                        Id = a.Id,
                        Tenant = a.Tenant,
                        ShipmentId = a.ShipmentId,
                        LastSendDate = a.LastSendDate,
                        SentByUserId = a.SentByUserId,
                        CommunicationLogId = a.CommunicationLogId,
                        Status = a.Status,
                        Error = a.Error,
                        MessageCode = a.MessageCode,
                        StatusName = a.CustomsTransmissionsStatus != null ? a.CustomsTransmissionsStatus.Name : "",
                        ByUserName = a.SendByUser != null ? a.SendByUser.Contact != null ? a.SendByUser.Contact.EnglishName : "" : "",
                    });
        }

        public IQueryable<ShipmentCustomsTransmissionList> GetIQueryableEntityList(IQueryable<ShipmentCustomsTransmission> iQueryable)
        {
            IQueryable<ShipmentCustomsTransmissionList> result =
                from a in iQueryable
                select new ShipmentCustomsTransmissionList()
                {
                    Id = a.Id,
                    Tenant = a.Tenant,
                    ShipmentId = a.ShipmentId,
                    LastSendDate = a.LastSendDate,
                    SentByUserId = a.SentByUserId,
                    CommunicationLogId = a.CommunicationLogId,
                    Status = a.Status,
                    Error = a.Error,
                    MessageCode = a.MessageCode,
                };
            return result;
        }

    }
}