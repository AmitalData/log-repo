using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.ShipmentsModel.Repositories
{
    public class CustomsTransferLineRepository : IRepository<CustomsTransferLine>
    {
        IShipmentsContext shipmentContext;
        
        public CustomsTransferLineRepository(int tenant)
        {
            shipmentContext = ShipmentsContext.GetContext(tenant);
        }

        public CustomsTransferLineRepository(IShipmentsContext context)
        {
            shipmentContext = context;
        }

        public CustomsTransferLine GetSingleEntity(string id)
        {
            return (from a in context.CustomsTransferLines where a.Id == id select a).FirstOrDefault();
        }

        public IQueryable<CustomsTransferLine> GetShipmentTransferLines(string shipmentId, int tenant)
        {
            return (from a in context.CustomsTransferLines where a.ShipmentId == shipmentId && a.Tenant == tenant select a);
        }
        
        public List<string> GetShipmentsIdsList(string transferHeaderId, int tenant)
        {
            List<string> myResult = (from a in context.CustomsTransferLines
                                     where a.Tenant == tenant
                                     && a.CustomsTransferHeaderId == transferHeaderId
                                     select a.ShipmentId).ToList();

            return myResult;
        }

        public void Add(CustomsTransferLine entity)
        {
            context.CustomsTransferLines.Add(entity);
        }

        public void Remove(CustomsTransferLine entity)
        {
            context.CustomsTransferLines.Attach(entity);
            context.CustomsTransferLines.Remove(entity);
        }

        public void Update(CustomsTransferLine entity)
        {
            context.CustomsTransferLines.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CustomsTransferLine> All()
        {
            return context.CustomsTransferLines.ToList();
        }

        public IShipmentsContext context
        {
            get { return shipmentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<CustomsTransferLine> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public CustomsTransferLine GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }
    }
}
