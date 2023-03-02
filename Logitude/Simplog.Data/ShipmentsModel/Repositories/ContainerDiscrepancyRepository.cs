using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;


namespace Simplog.Data.ShipmentsModel.Repositories
{
    public class ContainerDiscrepancyRepository : IRepository<ContainerDiscrepancy>
    {
        public IShipmentsContext shipmentContext;
        public ContainerDiscrepancyRepository(int tenant)
        {
            shipmentContext = ShipmentsContext.GetContext(tenant);
        }

        public ContainerDiscrepancyRepository(IShipmentsContext context)
        {
            shipmentContext = context;
        }

        public IShipmentsContext context
        {
            get { return shipmentContext; }
        }
        public void Add(ContainerDiscrepancy entity)
        {
            context.ContainerDiscrepancies.Add(entity);
        }

        public ContainerDiscrepancy GetSingleContainerDiscrepancy(string id, int tenant)
        {
            throw new NotImplementedException();
        }

        public List<ContainerDiscrepancy> All()
        {
            throw new NotImplementedException();
        }

        public List<ContainerDiscrepancy> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public ContainerDiscrepancy GetSingle(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public void Remove(ContainerDiscrepancy entity)
        {
            throw new NotImplementedException();
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public void Update(ContainerDiscrepancy entity)
        {
            try
            {
                context.ContainerDiscrepancies.Attach(entity);
            }
            catch { }
            context.SetAsModified(entity);
        }
        public ContainerDiscrepancy GetContainerDiscrepancyByContainerIdAndDiscrepancyReason(int tenant, string containerId, string shipmentId, string discrepancyReason)
        {
            return (from containerDiscrepancy in context.ContainerDiscrepancies
                    where containerDiscrepancy.Tenant == tenant &&
                    containerDiscrepancy.ContainerId == containerId &&  
                    containerDiscrepancy.ShipmentId == shipmentId &&
                    containerDiscrepancy.Discrepancy == discrepancyReason
                    select containerDiscrepancy).FirstOrDefault();
        }
    }
}
