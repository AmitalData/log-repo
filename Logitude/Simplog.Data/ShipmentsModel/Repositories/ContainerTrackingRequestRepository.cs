using System.Collections.Generic;
using System.Linq;

using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.ShipmentsModel.Repositories
{
    public class ContainerTrackingRequestRepository : IRepository<ContainerTrackingRequest>
    {
        IShipmentsContext shipmentContext;

        public ContainerTrackingRequestRepository()
        {
            shipmentContext = new ShipmentsContext();
        }

        public ContainerTrackingRequestRepository(IShipmentsContext context)
        {
            shipmentContext = context;
        }

        public ContainerTrackingRequestRepository(int tenant)
        {
            shipmentContext = ShipmentsContext.GetContext(tenant);
        }

        public ContainerTrackingRequest GetSingleContainerTrackingRequest(string id,int tenant)
        {
            return (from a in context.ContainerTrackingRequests
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public void Add(ContainerTrackingRequest entity)
        {
            context.ContainerTrackingRequests.Add(entity);
        }

        public void Remove(ContainerTrackingRequest entity)
        {
            context.ContainerTrackingRequests.Attach(entity);
            context.ContainerTrackingRequests.Remove(entity);
        }

        public void Update(ContainerTrackingRequest entity)
        {
            context.ContainerTrackingRequests.Attach(entity);
            context.SetAsModified(entity);
        }

        public IQueryable<ContainerTrackingRequest> GetContainerTrackingRequests(int tenant)
        {
            return context.ContainerTrackingRequests.Where(e=>e.Tenant == tenant);
        }

        public IShipmentsContext context
        {
            get { return shipmentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<ContainerTrackingRequest> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public ContainerTrackingRequest GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public List<ContainerTrackingRequest> All()
        {
            return context.ContainerTrackingRequests.ToList();
        }
    }
}