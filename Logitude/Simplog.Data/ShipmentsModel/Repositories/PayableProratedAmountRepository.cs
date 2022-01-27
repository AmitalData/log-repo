using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.ShipmentsModel.Repositories
{
    public class PayableProratedAmountRepository : IRepository<PayableProratedAmount>
    {
        IShipmentsContext shipmentsContext;

        public PayableProratedAmountRepository()
        {
            shipmentsContext = new ShipmentsContext();
        }

        public PayableProratedAmountRepository(int tenant)
        {
            shipmentsContext = ShipmentsContext.GetContext(tenant);
        }

        public PayableProratedAmountRepository(IShipmentsContext context)
        {
            shipmentsContext = context;
        }

        public PayableProratedAmount GetPayableProratedAmount(string id)
        {
            return (from a in context.PayableProratedAmounts
                    where a.Id == id
                    select a).FirstOrDefault();
        }

        public List<PayableProratedAmount>  GetPayableProratedAmountsByShipmentId(string shipmentId, int tenant)
        {
            return (from a in context.PayableProratedAmounts
                    where a.ShipmentId == shipmentId && a.Tenant == tenant
                    select a).ToList();
        }
        public List<PayableProratedAmount> GetPayableProratedAmountsByPayableId(string payableId, int tenant)
        {
            return (from a in context.PayableProratedAmounts
                    where a.PayableId == payableId && a.Tenant == tenant
                    select a).ToList();
        }

        public List<PayableProratedAmount> GetPayableProratedAmountsByPayablesIds(List<string> ids, int tenant)
        {
            List<PayableProratedAmount> myResult = new List<PayableProratedAmount>();

            if (ids.Count > 0)
            {
                myResult = (from a in context.PayableProratedAmounts
                            where a.Tenant == tenant && ids.Contains(a.PayableId)
                            select a).ToList();
            }

            return myResult;
        }
        
        public void Add(PayableProratedAmount entity)
        {
            context.PayableProratedAmounts.Add(entity);
        }

        public void Remove(PayableProratedAmount entity)
        {
            context.PayableProratedAmounts.Attach(entity);
            context.PayableProratedAmounts.Remove(entity);
        }

        public void Update(PayableProratedAmount entity)
        {
            try
            {
                context.PayableProratedAmounts.Attach(entity);
            }
            catch { }
            context.SetAsModified(entity);
        }

        public List<PayableProratedAmount> All()
        {
            return context.PayableProratedAmounts.ToList();
        }

        public IShipmentsContext context
        {
            get { return shipmentsContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<PayableProratedAmount> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public PayableProratedAmount GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}
