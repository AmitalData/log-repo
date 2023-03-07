using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using Simplog.Server.Infrastructure.Helpers;
using System.Text;

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
            return (from container in context.Containers.Include("CarrierCard").Include("VesselCard").Include("ShipmentOnCarriageToPort").Include("ShipmentOnCarriageFromPort")
                    .Include("ShipmentTransshipment3ToPort").Include("ShipmentTransshipment3FromPort").Include("ShipmentTransshipment2ToPort").Include("ShipmentTransshipment2FromPort")
                    .Include("ShipmentTransshipment1ToPort").Include("ShipmentTransshipment1FromPort").Include("ShipmentMainCarriageToPort").Include("ShipmentMainCarriageFromPort")
                    .Include("ShipmentPreCarriageToPort").Include("ShipmentPreCarriageFromPort").Include("ShipmentEntityStatus").Include("TerminalCard").Include("TerminalCardAddress")
                    .Include("TruckerCard").Include("ShipmentOriginAgent").Include("ShipmentDestinationAgent").Include("CustomerCard").Include("Handler").Include("Handler.Contact")
                    .Include("Shipment").Include("EntityStatus").Include("ContainerType").Include("EmptyPickupLocationPort").Include("PreCarriageLocationPort").Include("PODLocationPort")
                    .Include("POLLocationPort").Include("OnCarriageLocationPort").Include("Transshipment1LocationPort").Include("Transshipment2LocationPort").Include("Transshipment3LocationPort")
                    .Include("Transshipment4LocationPort").Include("EmptyReturnLocationPort").Include("ShipmentPreCarriageFromPort").Include("ShipmentMainCarriageFromPort")
                    .Include("ShipmentMainCarriageToPort").Include("ShipmentOnCarriageToPort").Include("ShipmentTransshipment1FromPort").Include("ShipmentTransshipment2FromPort")
                    .Include("ShipmentTransshipment3FromPort").Include("ShipmentDepartment")
                    where container.Id == id && container.Tenant == tenant
                    select container).FirstOrDefault();
        }

        public IQueryable<Container> GetContainers(int tenant)
        {
            return (from container in context.Containers
                    where  container.Tenant == tenant
                    select container);
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
                    where container.Tenant == tenant && container.ShipmentPackagesId == shipmentPackageId && !container.IsCancelled
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

        public Container GetCancelledContainerByShipmentId(string shipmentId,string containerNumber, int tenant)
        {
            return (from container in context.Containers 
                    where container.ContainerNumber == containerNumber && container.Tenant == tenant && container.ShipmentId == shipmentId && container.IsCancelled
                    select container).FirstOrDefault();
        }

        public string GetConcurrencyGUIDByShipmentId(string shipmentId, int tenant)
        {
            return (from shipment in context.Shipments
                    where shipment.Id == shipmentId && shipment.Tenant == tenant 
                    select shipment.ConcurrencyGUID).FirstOrDefault();
        }

        public string GetConcurrencyGUIDByContainerId(string containerId, int tenant)
        {
            return (from container in context.Containers
                    where container.Id == containerId && container.Tenant == tenant
                    select container.ConcurrencyGUID).FirstOrDefault();
        }

        public List<Container> GetContainersFromIds(List<string> containerIds, int tenant)
        {
            List<Container> containers = new List<Container>();
            if (containerIds.Count() == 0) return containers;

            IShipmentsContext shipmentsContext = ShipmentsContext.GetContext(tenant);
            StringBuilder values = new StringBuilder();
            values.AppendFormat("{0}", "'" + containerIds[0] + "'");
            for (int i = 1; i < containerIds.Count; i++)
                values.AppendFormat(", {0}", "'" + containerIds[i] + "'");

            string sql = string.Format("SELECT * FROM Containers WHERE ID IN ({0})", values);
            containers = shipmentsContext.GetActiveDbContext().Database.SqlQuery<Container>(sql).ToList();

            return containers;
        }
    }
}
