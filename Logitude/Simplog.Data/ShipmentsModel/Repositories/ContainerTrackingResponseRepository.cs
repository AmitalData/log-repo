using System.Collections.Generic;
using System.Linq;

using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.ShipmentsModel.Repositories
{
    public class ContainerTrackingResponseRepository : IRepository<ContainerTrackingResponse>
    {
        IShipmentsContext shipmentContext;

        public ContainerTrackingResponseRepository()
        {
            shipmentContext = new ShipmentsContext();
        }

        public ContainerTrackingResponseRepository(IShipmentsContext context)
        {
            shipmentContext = context;
        }

        public ContainerTrackingResponseRepository(int tenant)
        {
            shipmentContext = ShipmentsContext.GetContext(tenant);
        }

        public ContainerTrackingResponse GetSingleContainerTrackingResponse(string id,int tenant)
        {
            return (from a in context.ContainerTrackingResponses
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public void Add(ContainerTrackingResponse entity)
        {
            context.ContainerTrackingResponses.Add(entity);
        }

        public void Remove(ContainerTrackingResponse entity)
        {
            context.ContainerTrackingResponses.Attach(entity);
            context.ContainerTrackingResponses.Remove(entity);
        }

        public void Update(ContainerTrackingResponse entity)
        {
            context.ContainerTrackingResponses.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ContainerTrackingResponse> All()
        {
            return context.ContainerTrackingResponses.ToList();
        }
        public IQueryable<ContainerTrackingResponse> GetContainerTrackingResponses(int tenant)
        {
            return context.ContainerTrackingResponses.Where(e => e.Tenant == tenant);
        }
        
        public IShipmentsContext context
        {
            get { return shipmentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<ContainerTrackingResponse> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public ContainerTrackingResponse GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}