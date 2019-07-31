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
    public class ShipmentComputedFieldsRepository: IRepository<ShipmentComputedFields>
    {
        IShipmentsContext shipmentContext;

        public ShipmentComputedFieldsRepository(IShipmentsContext context)
        {
            shipmentContext = context;
        }

        public ShipmentComputedFieldsRepository()
        {
            shipmentContext = new ShipmentsContext();
        }

        public ShipmentComputedFieldsRepository(int tenant)
        {
            shipmentContext = ShipmentsContext.GetContext(tenant);
        }

        public IQueryable<ShipmentComputedFields> GetShipmentComputedFields(int tenant)
        {
            return context.ShipmentComputedFields.Where(s => s.Tenant == tenant);
        }

        public ShipmentComputedFields GetSingleShipmentComputedFields(string Id,int tenant)
        {
            if (!string.IsNullOrEmpty(Id))
            {

                ShipmentComputedFields entity = (from a in context.ShipmentComputedFields
                                                where a.Id==Id && a.Tenant == tenant
                                                select a).FirstOrDefault();
               
                return entity;
            }
            return null;
        }

        public void Add(ShipmentComputedFields entity)
        {
            context.ShipmentComputedFields.Add(entity);
        }

        public void Remove(ShipmentComputedFields entity)
        {
            context.ShipmentComputedFields.Attach(entity);
            context.ShipmentComputedFields.Remove(entity);
        }

        public void Update(ShipmentComputedFields entity)
        {
            context.ShipmentComputedFields.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ShipmentComputedFields> All()
        {
            return context.ShipmentComputedFields.ToList();
        }

        public IShipmentsContext context
        {
            get {return shipmentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<ShipmentComputedFields> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public ShipmentComputedFields GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }



        public IQueryable<ShipmentComputedFields> GetShipmentComputedFieldsByIds( List<string> ids ,int tenant)
        {
            return context.ShipmentComputedFields.Where(s => s.Tenant == tenant && ids.Contains(s.Id));
        }

    }
}