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
    public class FreightForwarderReferenceQuery
    {
        FreightForwarderReferenceRepository repository;

        public FreightForwarderReferenceQuery(int tenant)
        {
            repository = new FreightForwarderReferenceRepository(tenant);
        }

        public FreightForwarderReferenceQuery(FreightForwarderReferenceRepository repository)
        {
            this.repository = repository;
        }

        private List<FreightForwarderReferencePM> MapPocoToPM(IQueryable<FreightForwarderReference> iQueryable)
        {
            List<FreightForwarderReferencePM> myResult = (from a in iQueryable.Include("Shipment")
                                                   select new FreightForwarderReferencePM()
                                                   {
                                                       ShipmentId = a.ShipmentId,
                                                       Tenant = a.Tenant,
                                                       ForwarderShipmentNumber = a.ForwarderShipmentNumber,
                                                       ForwarderFileConnect = a.ForwarderFileConnect,
                                                   }).ToList();
            return myResult;
        }

        public FreightForwarderReferencePM GetSinglePM(int tenant, string id, int authTokenTenant)
        {
            FreightForwarderReferencePM myResult
                = (from a in repository.context.FreightForwarderReferences.Include("Shipment")
                   where a.ShipmentId == id && a.Tenant == tenant
                   select new FreightForwarderReferencePM()
                   {
                       ShipmentId = a.ShipmentId,
                       Tenant = a.Tenant,
                       ForwarderShipmentNumber = a.ForwarderShipmentNumber,
                       ForwarderFileConnect = a.ForwarderFileConnect,

                   }).FirstOrDefault();

            if (myResult != null)
            {
                new ChildEntitiesCustomFieldService().Set(new ChildEntitiesCustomFieldArgs()
                {
                    Tenant = tenant,
                    EntityId = myResult.ShipmentId,
                    ObjectTableName = "Shipment",
                    ChildObjectTableName = "FreightForwarderReference",
                    ChildEntityId = myResult?.ShipmentId,
                    ChildEntities = new List<object>() { myResult }.ToList(),
                });
            }
            return myResult;
        }

        public IQueryable<FreightForwarderReferenceList> GetIQueryableEntityList(IQueryable<FreightForwarderReference> iQueryable)
        {
            IQueryable<FreightForwarderReferenceList> result = (from a in iQueryable
                                                               select new FreightForwarderReferenceList()
                                                               {
                                                                   ShipmentId = a.ShipmentId,
                                                                   Tenant = a.Tenant
                                                               });
            return result;
        }

        public List<FreightForwarderReferencePM> GetFreightForwarderReferences(string shipmentId, int tenant)
        {
            List<FreightForwarderReferencePM> freightForwarderReferences
                = (from a in repository.context.FreightForwarderReferences
                   where a.ShipmentId == shipmentId && a.Tenant == tenant
                   select new FreightForwarderReferencePM()
                   {
                       ShipmentId = a.ShipmentId,
                       Tenant = a.Tenant,
                       ForwarderShipmentNumber = a.ForwarderShipmentNumber,
                       ForwarderFileConnect = a.ForwarderFileConnect,
                   }).ToList();

            return freightForwarderReferences;
        }
    }
}