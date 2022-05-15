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

        public IQueryable<ContainerTrackingProvider> GetContainerTrackingProviders(int tenant)
        {
            TenantRepository tenantRepository = new TenantRepository(tenant);
            Tenant tenantEntity = tenantRepository.GetSingleTenant(tenant); ////
            if (tenantEntity.IsHybrid)
            {
                return context.ContainerTrackingProviders;
            }
            else
            {
                return context.ContainerTrackingProviders.Where(s => s.Code != "A");
            }            
        }

        public IQueryable<ContainerTrackingProvider> GetAll()
        {
            return context.ContainerTrackingProviders;
        }

        public IQueryable<ContainerTrackingProvider> GetContainerTrackingProviders()
        {
            return context.ContainerTrackingProviders;
        }

        public ContainerTrackingProvider GetSingleContainerTrackingProvider(string code)
        {
            return (from a in context.ContainerTrackingProviders where a.Code == code select a).FirstOrDefault();
        }


        public string GetSingleContainerTrackingProviderNameByCode(string code)
        {
            return (from a in context.ContainerTrackingProviders where a.Code == code select a.Name).FirstOrDefault();
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

        public List<ContainerTrackingProvider> All()
        {
            return context.ContainerTrackingProviders.ToList();
        }

        public IShipmentsContext context
        {
            get {return shipmentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<ContainerTrackingProvider> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public ContainerTrackingProvider GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }
    }
}