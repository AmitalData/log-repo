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
    public class ContainerTrackingProviderRepository : IRepository<ContainerTrackingProvider>
    {
        IShipmentsContext shipmentContext;

        public ContainerTrackingProviderRepository(IShipmentsContext context)
        {
            shipmentContext = context;
        }

        public ContainerTrackingProviderRepository()
        {
            shipmentContext = new ShipmentsContext();
        }

        public ContainerTrackingProviderRepository(int tenant)
        {
            shipmentContext = ShipmentsContext.GetContext(tenant);
        }

        public ContainerTrackingProvider GetSingleContainerTrackingProvider(string id, int tenant)
        {
            return (from a in context.ContainerTrackingProviders
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public void Add(ContainerTrackingProvider entity)
        {
            context.ContainerTrackingProviders.Add(entity);
        }

        public void Remove(ContainerTrackingProvider entity)
        {
            context.ContainerTrackingProviders.Attach(entity);
            context.ContainerTrackingProviders.Remove(entity);
        }

        public void Update(ContainerTrackingProvider entity)
        {
            context.ContainerTrackingProviders.Attach(entity);
            context.SetAsModified(entity);
        }

        public IQueryable<ContainerTrackingProvider> GetContainerTrackingProviders(int tenant)
        {
            return context.ContainerTrackingProviders.Where(e => e.Tenant == tenant || e.Tenant == 0);
        }

        public IShipmentsContext context
        {
            get { return shipmentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<ContainerTrackingProvider> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public ContainerTrackingProvider GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
        public ContainerTrackingProvider GetBySourceCode(string sourceCode)
        {
            if (sourceCode.Equals("2", StringComparison.InvariantCultureIgnoreCase))
            {
                sourceCode = "VZN";
            }
            return context.ContainerTrackingProviders.Where(e => e.SourceCode == sourceCode).FirstOrDefault();
        }

        public List<ContainerTrackingProvider> All()
        {
            return context.ContainerTrackingProviders.ToList();
        }
    }
}