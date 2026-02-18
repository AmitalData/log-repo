using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.ShipmentsModel.Repositories
{
    public class ShipmentPayableAmountTypeRepository: IRepository<ShipmentPayableAmountType>
    {
        IShipmentsContext shipmentContext;

        public ShipmentPayableAmountTypeRepository()
        {
            shipmentContext = new ShipmentsContext();
        }

        public ShipmentPayableAmountTypeRepository(IShipmentsContext context)
        {
            shipmentContext = context;
        }

        public ShipmentPayableAmountTypeRepository(int tenant)
        {
            shipmentContext = ShipmentsContext.GetContext(tenant);
        }

        public IQueryable<ShipmentPayableAmountType> GetShipmentPayableAmountTypes()
        {
            return context.ShipmentPayableAmountTypes;
        }

        public ShipmentPayableAmountType GetSingleShipmentPayableAmountType(string code)
        {
            return (from a in context.ShipmentPayableAmountTypes
                    where a.Code == code
                    select a).FirstOrDefault();
        }

        public void Add(ShipmentPayableAmountType entity)
        {
            context.ShipmentPayableAmountTypes.Add(entity);
        }

        public void Remove(ShipmentPayableAmountType entity)
        {
            context.ShipmentPayableAmountTypes.Attach(entity);
            context.ShipmentPayableAmountTypes.Remove(entity);
        }

        public void Update(ShipmentPayableAmountType entity)
        {
            context.ShipmentPayableAmountTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ShipmentPayableAmountType> All()
        {
            return context.ShipmentPayableAmountTypes.ToList();
        }

        public IShipmentsContext context
        {
            get { return shipmentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<ShipmentPayableAmountType> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public ShipmentPayableAmountType GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }
    }
}