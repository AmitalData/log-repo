using System.Collections.Generic;
using System.Linq;
using Simplog.Server.Infrastructure;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Server.Infrastructure.Helpers;

namespace Simplog.Data.InvoiceModel.Repositories
{
    public class APInvoiceLineRepository: IRepository<APInvoiceLine>
    {
        IInvoiceContext invoiceContext;
        public APInvoiceLineRepository()
        {
            invoiceContext = new InvoiceContext();
        }

        public APInvoiceLineRepository(int tenant)
        {
            invoiceContext = InvoiceContext.GetContext(tenant);
        }

        public APInvoiceLineRepository(IInvoiceContext context)
        {
            invoiceContext = context;
        }

        public IQueryable<APInvoiceLine> GetInvoiceLinesForMessaging(int tenant)
        {
            return (from a in invoiceContext.APInvoiceLines.Include("ChargesType").Include("VatType")
                    where a.Tenant == tenant
                    select a);

        }

        public APInvoiceLine GetInvoiceByPayableId(string payableId, int tenant)
        {
            return (from a in context.APInvoiceLines where a.EntityPayableId == payableId && a.Tenant == tenant select a).FirstOrDefault();           
        }

        public APInvoiceLine GetSingleAPInvoiceLine(string invoiceId,int lineNumber, int tenant)
        {
            return (from a in context.APInvoiceLines
                    where a.APInvoiceId == invoiceId && a.LineNumber == lineNumber && a.Tenant == tenant
                    select a).FirstOrDefault();
        }
      
        public IQueryable<APInvoiceLine> GetAPInvoiceLinesByTenant(int tenant)
        {
            return (from a in context.APInvoiceLines
                    where a.Tenant == tenant
                    select a);
        }

        public IQueryable<APInvoiceLine> GetInvoiceLinesByEntityId(string invoiceId, string shipmentId, int tenant)
        {
            return (from a in context.APInvoiceLines
                    where a.Tenant == tenant
                    && a.APInvoiceId == invoiceId
                    && a.EntityId == shipmentId
                    select a);
        }

        public IQueryable<APInvoiceLine> GetOtherShipmentsLines(string invoiceId, string shipmentId, int tenant)
        {
            return (from a in context.APInvoiceLines
                    where a.Tenant == tenant
                    && a.APInvoiceId == invoiceId
                    && a.EntityId != shipmentId
                    select a);
        }

        public IQueryable<APInvoiceLine> GetInvoiceLinesByInvoiceId(string invoiceid, int tenant)
        {
            return (from a in context.APInvoiceLines
                    where a.APInvoiceId == invoiceid && a.Tenant == tenant
                    select a);
        }

        public bool IsPayableConnectedToInvoiceLines(string payableid, int tenant)
        {
            return (from a in context.APInvoiceLines
                    where a.EntityPayableId == payableid && a.Tenant == tenant
                    select a).Any();
            
        }

        public List<APInvoiceLine> GetPayablesInvoicesLines(List<string> allPayablesIds, int tenant)
        {
            List<APInvoiceLine> myResult = new List<APInvoiceLine>();

            if (allPayablesIds.Count > 0)
            {
                myResult = (from d in context.APInvoiceLines
                            where d.Tenant == tenant
                            && allPayablesIds.Contains(d.EntityPayableId)
                            select d).ToList();
            }

            return myResult;
        }

        public void Add(APInvoiceLine entity)
        {
            context.APInvoiceLines.Add(entity);
        }

        public void Remove(APInvoiceLine entity)
        {
            context.APInvoiceLines.Attach(entity);
            context.APInvoiceLines.Remove(entity);
        }

        public void Update(APInvoiceLine entity)
        {
            context.APInvoiceLines.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<APInvoiceLine> All()
        {
            return  context.APInvoiceLines.ToList();
        }

        public IInvoiceContext context
        {
            get { return invoiceContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<APInvoiceLine> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public APInvoiceLine GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public IQueryable<APInvoiceLine> GetExpenseInvoiceLinesByInvoiceId(string invoiceid, int tenant)
        {
            return (from a in context.APInvoiceLines.Include("ChargesType")
                    where a.APInvoiceId == invoiceid && a.Tenant == tenant
                    && a.ChargesType != null && a.ChargesType.IsExpense
                    select a);
        }
    }
}