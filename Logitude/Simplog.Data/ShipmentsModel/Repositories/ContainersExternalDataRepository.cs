using System;
using System.Collections.Generic;
using System.Linq;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.ShipmentsModel.Repositories
{
    public class ContainersExternalDataRepository : IRepository<ContainersExternalData>
    {
        IShipmentsContext shipmentContext;

        public ContainersExternalDataRepository(IShipmentsContext context)
        {
            shipmentContext = context;
        }

        public ContainersExternalDataRepository()
        {
            shipmentContext = new ShipmentsContext();
        }

        public ContainersExternalDataRepository(int tenant)
        {
            shipmentContext = ShipmentsContext.GetContext(tenant);
        }

        public IQueryable<ContainersExternalData> GetContainersExternalData(int tenant)
        {
            return context.ContainersExternalDatas.Where(s => s.Tenant == tenant);
        }
        public IQueryable<ContainersExternalData> GetContainersExternalData()
        {
            return context.ContainersExternalDatas;
        }

        public ContainersExternalData GetSingleContainersExternalData(string Id, int tenant)
        {
            if (!string.IsNullOrEmpty(Id))
            {

                ContainersExternalData entity = (from a in context.ContainersExternalDatas
                                                 where a.Id == Id && a.Tenant == tenant
                                                 select a).FirstOrDefault();

                return entity;
            }
            return null;
        }

        public void Add(ContainersExternalData entity)
        {
            context.ContainersExternalDatas.Add(entity);
        }

        public void Remove(ContainersExternalData entity)
        {
            context.ContainersExternalDatas.Attach(entity);
            context.ContainersExternalDatas.Remove(entity);
        }

        public void Update(ContainersExternalData entity)
        {
            context.ContainersExternalDatas.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ContainersExternalData> All()
        {
            return context.ContainersExternalDatas.ToList();
        }

        public IShipmentsContext context
        {
            get { return shipmentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<ContainersExternalData> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public ContainersExternalData GetSingleContainersExternalData(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {

                ContainersExternalData entity = (from a in context.ContainersExternalDatas
                                                 where a.Id == id
                                                 select a).FirstOrDefault();

                return entity;
            }
            return null;
        }

        public ContainersExternalData GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public IQueryable<ContainersExternalData> GetContainersExternalDataByIds(List<string> ids, int tenant)
        {
            return context.ContainersExternalDatas.Where(s => s.Tenant == tenant && ids.Contains(s.Id));
        }

    }
}
