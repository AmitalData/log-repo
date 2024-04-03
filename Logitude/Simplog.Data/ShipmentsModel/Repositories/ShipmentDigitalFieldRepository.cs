using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Server.Infrastructure;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;


namespace Simplog.Data.ShipmentsModel.Repositories
{
    public class ShipmentDigitalFieldRepository : IRepository<ShipmentDigitalField>
    {
        IShipmentsContext shipmentContext;

        public ShipmentDigitalFieldRepository(IShipmentsContext context)
        {
            shipmentContext = context;
        }

        public ShipmentDigitalFieldRepository()
        {
            shipmentContext = new ShipmentsContext();
        }

        public ShipmentDigitalFieldRepository(int tenant)
        {
            shipmentContext = ShipmentsContext.GetContext(tenant);
        }

        public IQueryable<ShipmentDigitalField> GetShipmentDigitalFields(int tenant)
        {
            return context.ShipmentDigitalFields.Where(s => s.Tenant == tenant);
        }
        public IQueryable<ShipmentDigitalField> GetShipmentDigitalFields()
        {
            return context.ShipmentDigitalFields;
        }

        public ShipmentDigitalField GetSingleShipmentDigitalFields(string Id, int tenant)
        {
            if (!string.IsNullOrEmpty(Id))
            {

                ShipmentDigitalField entity = (from a in context.ShipmentDigitalFields
                                                 where a.Id == Id && a.Tenant == tenant
                                                 select a).FirstOrDefault();

                return entity;
            }
            return null;
        }

        public void Add(ShipmentDigitalField entity)
        {
            context.ShipmentDigitalFields.Add(entity);
        }

        public void Remove(ShipmentDigitalField entity)
        {
            context.ShipmentDigitalFields.Attach(entity);
            context.ShipmentDigitalFields.Remove(entity);
        }

        public void Update(ShipmentDigitalField entity)
        {
            context.ShipmentDigitalFields.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ShipmentDigitalField> All()
        {
            return context.ShipmentDigitalFields.ToList();
        }

        public IShipmentsContext context
        {
            get { return shipmentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<ShipmentDigitalField> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public ShipmentDigitalField GetSingleShipmentDigitalFields(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {

                ShipmentDigitalField entity = (from a in context.ShipmentDigitalFields
                                                 where a.Id == id
                                                 select a).FirstOrDefault();

                return entity;
            }
            return null;
        }

        public ShipmentDigitalField GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }



        public IQueryable<ShipmentDigitalField> GetShipmentDigitalFieldsByIds(List<string> ids, int tenant)
        {
            return context.ShipmentDigitalFields.Where(s => s.Tenant == tenant && ids.Contains(s.Id));
        }
    }
}