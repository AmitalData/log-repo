using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.ShipmentsModel.Repositories
{
    public class ContainerStatusSourceRepository : IRepository<ContainerStatusSource>
    {
        IShipmentsContext shipmentContext;

        public ContainerStatusSourceRepository(IShipmentsContext context)
        {
            shipmentContext = context;
        }

        public ContainerStatusSourceRepository()
        {
            shipmentContext = new ShipmentsContext();
        }

        public ContainerStatusSourceRepository(int tenant)
        {
            shipmentContext = ShipmentsContext.GetContext(tenant);
        }

        public void Add(ContainerStatusSource entity)
        {
            context.ContainerStatusSources.Add(entity);
        }

        public List<ContainerStatusSource> GetAll()
        {
            return context.ContainerStatusSources.ToList();
        }
        public IQueryable<ContainerStatusSource> GetContainerStatusSources()
        {
            return context.ContainerStatusSources;
        }

        public void Remove(ContainerStatusSource entity)
        {
            throw new NotImplementedException();
        }
        public IShipmentsContext context
        {
            get { return shipmentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public void Update(ContainerStatusSource entity)
        {
            context.ContainerStatusSources.Attach(entity);
            context.SetAsModified(entity);
        }

        public ContainerStatusSource GetSingleContainerStatus(string Code)
        {
            return (from record in context.ContainerStatusSources where record.Code == Code select record).FirstOrDefault();
        }

        public List<ContainerStatusSource> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public ContainerStatusSource GetSingle(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();

        }
        public List<ContainerStatusSource> All()
        {
            return context.ContainerStatusSources.ToList();
        }
    }
}
