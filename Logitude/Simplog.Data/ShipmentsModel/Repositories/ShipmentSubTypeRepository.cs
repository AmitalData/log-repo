using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.ShipmentsModel.Repositories
{
    public class ShipmentSubTypeRepository : IRepository<ShipmentSubType>
    {
        IShipmentsContext shipmentsContext;

        public ShipmentSubTypeRepository()
        {
            shipmentsContext = new ShipmentsContext();
        }

        public ShipmentSubTypeRepository(IShipmentsContext context)
        {
            shipmentsContext = context;
        }

        public ShipmentSubTypeRepository(int tenant)
        {
            shipmentsContext = ShipmentsContext.GetContext(tenant);
        }

        public ShipmentSubType GetSingleShipmentSubType(string id, int tenant)
        {
            return (from a in context.ShipmentSubTypes.Include("ShipmentType").Include("CreatedByUser").Include("CreatedByUser.Contact").Include("UpdatedByUser").Include("UpdatedByUser.Contact")
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public ShipmentSubType GetSingleShipmentSubTypeByCode(string code, int tenant)
        {
            return (from a in context.ShipmentSubTypes
                    where a.Code == code && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<ShipmentSubType> GetAll()
        {
            return context.ShipmentSubTypes;
        }
        public IQueryable<ShipmentSubType> GetShipmentSubTypes(int tenant)
        {
            return (from a in context.ShipmentSubTypes.Include("ShipmentType").Include("CreatedByUser").Include("CreatedByUser.Contact").Include("UpdatedByUser").Include("UpdatedByUser.Contact")
                    where a.Tenant == tenant
                    select a);
        }

        public IQueryable<ShipmentSubType> GetShipmentSubTypesWithoutIncludes(int tenant)
        {
            return (from a in context.ShipmentSubTypes
                    where a.Tenant == tenant
                    select a);
        }

        public void Add(ShipmentSubType entity)
        {
            context.ShipmentSubTypes.Add(entity);
        }

        public void Remove(ShipmentSubType entity)
        {
            context.ShipmentSubTypes.Attach(entity);
            context.ShipmentSubTypes.Remove(entity);
        }

        public void Update(ShipmentSubType entity)
        {
            context.ShipmentSubTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ShipmentSubType> All()
        {
            return context.ShipmentSubTypes.ToList();
        }

        public IShipmentsContext context
        {
            get { return shipmentsContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<ShipmentSubType> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public ShipmentSubType GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}