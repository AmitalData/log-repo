using System;
using System.Collections.Generic;
using System.Linq;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InvoiceModel.Repositories;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.Server.Tools.CustomFields;
using Logitude.BL.ShipmentsModel.EntityLists;

namespace Logitude.BL.ShipmentsModel.EntityQueries
{
    public class ShipmentReferanceQuery
    {
        ShipmentReferanceRepository repository;

        public ShipmentReferanceQuery(int tenant)
        {
            repository = new ShipmentReferanceRepository(tenant);
        }

        public ShipmentReferanceQuery(ShipmentReferanceRepository repository)
        {
            this.repository = repository;
        }

        /*
        public List<ShipmentReferancePM> GetShipmentReferancePMsByShipmentId(string shipmentId, int tenant)
        {
            IQueryable<ShipmentReferance> iQueryable = (from a in repository.context.ShipmentReferances
                                                        where a.ShipmentId == shipmentId && a.Tenant == tenant
                                                         select a);

            List<ShipmentReferancePM> shipmentReferances = this.MapPocoToPM(iQueryable);

            foreach (ShipmentReferancePM item in shipmentReferances)
            {
                IQueryable<ShipmentReferance> iQueryableChilds = (from a in repository.context.ShipmentReferances
                                                                  where a.ShipmentReceivableParentId == item.Id && a.Tenant == tenant
                                                                     select a);

                item.ChildShipmentReferances = this.MapPocoToPM(iQueryableChilds);
            }


            new ChildEntitiesCustomFieldService().Set(new ChildEntitiesCustomFieldArgs()
            {
                Tenant = tenant,
                EntityId = shipmentId,
                ObjectTableName = "Shipment",
                ChildObjectTableName = "ShipmentReferance",
                ChildEntities = shipmentReferances.Cast<object>().ToList()
            });


            return shipmentReferances.OrderBy(d => d.Id).ToList();
        }*/

        private List<ShipmentReferancePM> MapPocoToPM(IQueryable<ShipmentReferance> iQueryable)
        {
            List<ShipmentReferancePM> myResult = (from a in iQueryable.Include("Card").Include("ReferenceType").Include("Shipment")
                                                   select new ShipmentReferancePM()
                                                   {
                                                       ShipmentId = a.ShipmentId,
                                                       Tenant = a.Tenant,
                                                       LineNumber = a.LineNumber,
                                                       ReferenceType = a.ReferenceType == null ? null : a.ReferenceTypeCode.Code,
                                                       PartnerId = a.Card == null ? null : a.Card.Code,
                                                       ReferenceValue = a.ReferenceValue,
                                                   }).ToList();
            return myResult;
        }

        public ShipmentReferancePM GetSinglePM(string id, int tenant)
        {
            ShipmentReferancePM myResult
                = (from a in repository.context.ShipmentReferances.Include("Card").Include("ReferenceType").Include("Shipment")
                   where a.ShipmentId == id && a.Tenant == tenant
                   select new ShipmentReferancePM()
                   {
                       ShipmentId = a.ShipmentId,
                       Tenant = a.Tenant,
                       LineNumber = a.LineNumber,
                       ReferenceType = a.ReferenceType == null ? null : a.ReferenceTypeCode.Code,
                       PartnerId = a.Card == null ? null : a.Card.Code,
                       ReferenceValue = a.ReferenceValue,

                   }).FirstOrDefault();

            if (myResult != null)
            {
                new ChildEntitiesCustomFieldService().Set(new ChildEntitiesCustomFieldArgs()
                {
                    Tenant = tenant,
                    EntityId = myResult.ShipmentId,
                    ObjectTableName = "Shipment",
                    ChildObjectTableName = "ShipmentReferance",
                    ChildEntityId = myResult?.ShipmentId,
                    ChildEntities = new List<object>() { myResult }.ToList(),
                });
            }
            return myResult;
        }

        public IQueryable<ShipmentReferanceList> GetIQueryableEntityList(IQueryable<ShipmentReferance> iQueryable)
        {
            IQueryable<ShipmentReferanceList> result = (from a in iQueryable
                                                               select new ShipmentReferanceList()
                                                               {
                                                                   ShipmentId = a.ShipmentId,
                                                                   Tenant = a.Tenant
                                                               });
            return result;
        }

        public List<ShipmentReferancePM> GetShipmentReferances(string shipmentId, int tenant)
        {
            List<ShipmentReferancePM> shipmentReferances
                = (from a in repository.context.ShipmentReferances.Include("Card").Include("ReferenceType")
                   where a.ShipmentId == shipmentId && a.Tenant == tenant
                   select new ShipmentReferancePM()
                   {
                       ShipmentId = a.ShipmentId,
                       Tenant = a.Tenant,
                       LineNumber = a.LineNumber,
                       ReferenceType = a.ReferenceType == null ? null : a.ReferenceTypeCode.Code,
                       PartnerId = a.Card == null ? null : a.Card.Code,
                       ReferenceValue = a.ReferenceValue,
                   }).ToList();

            return shipmentReferances;
        }
    }
}