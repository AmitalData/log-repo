using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using Simplog.Server.Infrastructure.Helpers;

namespace Simplog.Data.ShipmentsModel.Repositories
{
    public class ContainerRepository : IRepository<Container>
    {

        public IShipmentsContext shipmentContext;
        public ContainerRepository(int tenant)
        {
            shipmentContext = ShipmentsContext.GetContext(tenant);
        }

        public ContainerRepository(IShipmentsContext context)
        {
            shipmentContext = context;
        }

        public IShipmentsContext context
        {
            get { return shipmentContext; }
        }

        public void Add(Container entity)
        {
            context.Containers.Add(entity);
        }

        public List<Container> All()
        {
            return context.Containers.ToList();
        }

        public Container GetSingleContainer(string id, int tenant)
        {
            return (from container in context.Containers.Include("CarrierCard").Include("VesselCard")
                    where container.Id == id && container.Tenant == tenant
                    select container).FirstOrDefault();
        }

        public IQueryable<Container> GetContainers(int tenant)
        {
            return context.Containers;
        }

        public void Remove(Container entity)
        {
           context.Containers.Attach(entity);
           context.Containers.Remove(entity);
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public void Update(Container entity)
        {
            try
            {
                context.Containers.Attach(entity);
            }
            catch { }
            context.SetAsModified(entity);
        }
        public List<Container> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public Container GetSingle(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public Container GetContainerByShipmentPackagesId(string shipmentPackageId, int tenant)
        {
            return (from container in context.Containers
                    where container.Tenant == tenant && container.ShipmentPackagesId == shipmentPackageId
                    select container).FirstOrDefault();
        }

        public Container GetContainerByContainerNumberAndTenant(string containerNumber, int tenant)
        {
            return (from container in context.Containers
                    where container.Tenant == tenant && container.ContainerNumber == containerNumber
                    select container).FirstOrDefault();
        }

        public IQueryable<Container> GetContainesrByShipmentId(string shipmentId, int tenant)
        {
            return from container in context.Containers
                   where container.Tenant == tenant && container.ShipmentId == shipmentId
                   select container;
        }
    }
}
