using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.ShipmentsModel.Repositories
{
    public class ContainerStatusRepository : IRepository<ContainerStatus>
    {

        IShipmentsContext shipmentContext;

        public ContainerStatusRepository(IShipmentsContext context)
        {
            shipmentContext = context;
        }

        public ContainerStatusRepository()
        {
            shipmentContext = new ShipmentsContext();
        }

        public ContainerStatusRepository(int tenant)
        {
            shipmentContext = ShipmentsContext.GetContext(tenant);
        }

        public void Add(ContainerStatus entity)
        {
            context.ContainerStatuses.Add(entity);
        }

        public List<ContainerStatus> GetAll()
        {
            return context.ContainerStatuses.ToList();
        }
        public IQueryable<ContainerStatus> GetContainerStatuses()
        {
            return context.ContainerStatuses;
        }

        public void Remove(ContainerStatus entity)
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

        public void Update(ContainerStatus entity)
        {
            context.ContainerStatuses.Attach(entity);
            context.SetAsModified(entity);
        }

        public ContainerStatus GetSingleContainerStatus(string Code)
        {
            return (from record in context.ContainerStatuses where record.Code == Code select record).FirstOrDefault();
        }

        public List<ContainerStatus> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public ContainerStatus GetSingle(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();

        }
        public List<ContainerStatus> All()
        {
            return context.ContainerStatuses.ToList();
        }
    }
}
