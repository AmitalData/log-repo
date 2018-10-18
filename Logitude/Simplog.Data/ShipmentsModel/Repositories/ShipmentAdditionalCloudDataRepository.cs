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
    public class ShipmentAdditionalCloudDataRepository : IRepository<ShipmentAdditionalCloudData>
    {
        IShipmentsContext shipmentContext;

        public ShipmentAdditionalCloudDataRepository(IShipmentsContext context)
        {
            shipmentContext = context;
        }

        public ShipmentAdditionalCloudDataRepository()
        {
            shipmentContext = new ShipmentsContext();
        }

        public ShipmentAdditionalCloudDataRepository(int tenant)
        {
            shipmentContext = ShipmentsContext.GetContext(tenant);
        }

        public IQueryable<ShipmentAdditionalCloudData> GetShipmentAdditionalCloudData(int tenant)
        {
            return context.ShipmentAdditionalCloudDatas.Where(s => s.Tenant == tenant);
        }

        public ShipmentAdditionalCloudData GetSingleShipmentAdditionalCloudData(string Id,int tenant)
        {
            if (!string.IsNullOrEmpty(Id))
            {

                ShipmentAdditionalCloudData entity = (from a in context.ShipmentAdditionalCloudDatas
                                                      where a.Id==Id && a.Tenant == tenant
                                                select a).FirstOrDefault();
               
                return entity;
            }
            return null;
        }

        public void Add(ShipmentAdditionalCloudData entity)
        {
            context.ShipmentAdditionalCloudDatas.Add(entity);
        }

        public void Remove(ShipmentAdditionalCloudData entity)
        {
            context.ShipmentAdditionalCloudDatas.Attach(entity);
            context.ShipmentAdditionalCloudDatas.Remove(entity);
        }

        public void Update(ShipmentAdditionalCloudData entity)
        {
            context.ShipmentAdditionalCloudDatas.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ShipmentAdditionalCloudData> All()
        {
            return context.ShipmentAdditionalCloudDatas.ToList();
        }

        public IShipmentsContext context
        {
            get {return shipmentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<ShipmentAdditionalCloudData> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public ShipmentAdditionalCloudData GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }
    }
}