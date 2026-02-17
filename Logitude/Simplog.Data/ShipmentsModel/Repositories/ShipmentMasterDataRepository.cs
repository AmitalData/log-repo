using System.Collections.Generic;
using System.Linq;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
namespace Simplog.Data.ShipmentsModel.Repositories
{
    public class ShipmentMasterDataRepository: IRepository<ShipmentMasterData>
    {
        IShipmentsContext shipmentContext;

        public ShipmentMasterDataRepository(IShipmentsContext context)
        {
            shipmentContext = context;
        }

        public ShipmentMasterDataRepository()
        {
            shipmentContext = new ShipmentsContext();
        }

        public ShipmentMasterDataRepository(int tenant)
        {
            shipmentContext = ShipmentsContext.GetContext(tenant);
        }

        public IQueryable<ShipmentMasterData> GetShipmentMasterDatasByTenant(int tenant)
        {
            return from a in context.ShipmentMasterDatas
                   where a.Tenant == tenant
                   select a;
        }

        public ShipmentMasterData GetSingleMasterData(string id)
        {
            return (from a in context.ShipmentMasterDatas
                    where a.Id == id
                    select a).FirstOrDefault();
        }

        public void Add(ShipmentMasterData entity)
        {
            context.ShipmentMasterDatas.Add(entity);
        }

        public void Remove(ShipmentMasterData entity)
        {
            try
            {
                context.ShipmentMasterDatas.Attach(entity);
            }
            catch { }
            context.ShipmentMasterDatas.Remove(entity);
        }

        public void Update(ShipmentMasterData entity)
        {
            try
            {
                context.ShipmentMasterDatas.Attach(entity);
            }
            catch { }
            context.SetAsModified(entity);
        }

        public List<ShipmentMasterData> All()
        {
            return context.ShipmentMasterDatas.ToList();
        }

        public IShipmentsContext context
        {
            get { return shipmentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<ShipmentMasterData> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public ShipmentMasterData GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}