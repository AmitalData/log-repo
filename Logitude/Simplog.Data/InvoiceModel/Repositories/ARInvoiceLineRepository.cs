using System.Collections.Generic;
using System.Linq;
using Simplog.Server.Infrastructure;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Server.Infrastructure.Helpers;

namespace Simplog.Data.InvoiceModel.Repositories
{
    public class ARInvoiceLineRepository: IRepository<ARInvoiceLine>
    {
        IInvoiceContext invoiceContext;
        public IInvoiceContext context
        {
            get { return this.invoiceContext; }
        }

        public ARInvoiceLineRepository()
        {
            invoiceContext = new InvoiceContext();
        }
        public ARInvoiceLineRepository(IInvoiceContext context)
        {
            invoiceContext = context;
        }
        public ARInvoiceLineRepository(int tenant)
        {
            invoiceContext = InvoiceContext.GetContext(tenant);
        }

        public IQueryable<ARInvoiceLine> GetInvoiceLines()
        {
            return invoiceContext.ARInvoiceLines;
        }

        public IQueryable<ARInvoiceLine> GetInvoiceLinesForMessaging(int tenant)
        {
            return (from a in invoiceContext.ARInvoiceLines.Include("ChargesType").Include("VatType")
                    where a.Tenant == tenant
                    select a);

        }

        public IQueryable<ARInvoiceLine> GetInvoiceLinesByInvoiceId(string invoiceId,int tenant)
        {
            return (from a in invoiceContext.ARInvoiceLines
                    where a.Tenant == tenant && a.ARInvoiceId == invoiceId
                    select a);

        }

        public IQueryable<ARInvoiceLine> GetInvoiceLinesByTenant(int tenant)
        {
            IQueryable<ARInvoiceLine> invoiceLines = from a in invoiceContext.ARInvoiceLines
                                                   where a.Tenant == tenant
                                                   select a;
            return invoiceLines; 
        }

       

        public ARInvoiceLine GetSingleInvoiceLine(string id)
        {
            return (from a in invoiceContext.ARInvoiceLines
                    where a.Id == id
                    select a).FirstOrDefault();
        }

        public int GetBiggestLineNumber(string invoiceId, int tenant)
        {
            int lineNumber = 0;

            IQueryable<ARInvoiceLine> lines = (from d in invoiceContext.ARInvoiceLines
                                               where d.ARInvoiceId == invoiceId && d.Tenant == tenant
                                               select d);

            if (lines.Count() > 0)
            {
                lineNumber = lines.Max(m => m.LineNumber);
            }

            return lineNumber;
        }

        public void Add(ARInvoiceLine entity)
        {
            invoiceContext.ARInvoiceLines.Add(entity);
        }

        public void Remove(ARInvoiceLine entity)
        {
            invoiceContext.ARInvoiceLines.Attach(entity);
            invoiceContext.ARInvoiceLines.Remove(entity);
        }

        public void Update(ARInvoiceLine entity)
        {            
            invoiceContext.ARInvoiceLines.Attach(entity);
            invoiceContext.SetAsModified(entity);
        }

        public List<ARInvoiceLine> All()
        {
            return invoiceContext.ARInvoiceLines.ToList();
        }

        public void SubmitChanges()
        {
            invoiceContext.SaveChanges();
        }



        public List<ARInvoiceLine> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public ARInvoiceLine GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

    }
}