using System.Collections.Generic;
using System.Linq;

using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.InvoiceModel.Repositories
{
    public class APInvoiceTotalVATRepository: IRepository<APInvoiceTotalVAT>
    {
        IInvoiceContext invoiceContext;

        public APInvoiceTotalVATRepository()
        {
            invoiceContext = new InvoiceContext();

        }

        public APInvoiceTotalVATRepository(IInvoiceContext context)
        {
            invoiceContext = context;
        }
        
        public APInvoiceTotalVATRepository(int tenant)
        {
            invoiceContext = InvoiceContext.GetContext(tenant);
        }
     
        public IQueryable<APInvoiceTotalVAT> GetInvoiceTotalVatsByInvoiceId(string invoiceId, int tenant)
        {
            return from a in context.APInvoiceTotalVATs
                   where a.Tenant == tenant 
                   && a.APInvoiceId == invoiceId
                   select a;
        }

        public APInvoiceTotalVAT GetSingleAPInvoiceTotalVAT(string id, int tenant)
        {
            return (from a in context.APInvoiceTotalVATs
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<APInvoiceTotalVAT> GetAPInvoiceTotalVats(int tenant)
        {
            return (from a in context.APInvoiceTotalVATs
                    where a.Tenant == tenant
                    select a);
        }
    
        public void Add(APInvoiceTotalVAT entity)
        {
            context.APInvoiceTotalVATs.Add(entity);
        }

        public void Remove(APInvoiceTotalVAT entity)
        {
            context.APInvoiceTotalVATs.Attach(entity);
            context.APInvoiceTotalVATs.Remove(entity);
        }

        public void Update(APInvoiceTotalVAT entity)
        {
            context.APInvoiceTotalVATs.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<APInvoiceTotalVAT> All()
        {
            return  context.APInvoiceTotalVATs.ToList();
        }

        public IInvoiceContext context
        {
            get { return invoiceContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<APInvoiceTotalVAT> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public APInvoiceTotalVAT GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public List<APInvoiceTotalVAT> GetInvoiceTotalVatsForInvoiceWithoutZeroVATPercent(string invoiceId, int tenant)
        {
            return (from a in context.APInvoiceTotalVATs
                    where a.Tenant == tenant && a.APInvoiceId == invoiceId && a.VatPercent != 0
                    select a).ToList();
        }
    }
}