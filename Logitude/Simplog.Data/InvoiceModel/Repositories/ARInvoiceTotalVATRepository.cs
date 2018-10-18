using System.Collections.Generic;
using System.Linq;

using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.InvoiceModel.Repositories
{
    public class ARInvoiceTotalVATRepository: IRepository<ARInvoiceTotalVAT>
    {
        IInvoiceContext invoiceContext;

        public ARInvoiceTotalVATRepository()
        {
            invoiceContext = new InvoiceContext();
        }

        public ARInvoiceTotalVATRepository(IInvoiceContext context)
        {
            invoiceContext = context;
        }

        public ARInvoiceTotalVATRepository(int tenant)
        {
            invoiceContext = InvoiceContext.GetContext(tenant);
        }

        public ARInvoiceTotalVAT GetSingleInvoiceTotalVAT(string id)
        {
            return (from a in context.ARInvoiceTotalVATs
                    where a.Id == id
                    select a).FirstOrDefault();
        }

        public IQueryable<ARInvoiceTotalVAT> GetInvoiceTotalVATsByTenant(int tenant)
        {
            return from a in context.ARInvoiceTotalVATs
                   where a.Tenant == tenant
                   select a;
 
        }
     
        public IQueryable<ARInvoiceTotalVAT> GetInvoiceTotalVatsForInvoice(string invoiceId, int tenant)
        {
            return from a in context.ARInvoiceTotalVATs
                   where a.Tenant == tenant && a.ARInvoiceId == invoiceId
                   select a;
        }

        public List<ARInvoiceTotalVAT> GetTotalVATsFromInvoiceIdList(List<string> ids, int tenant)
        {
            List<ARInvoiceTotalVAT> myResult = new List<ARInvoiceTotalVAT>();

            if (ids.Count > 0)
            {
                myResult = (from a in context.ARInvoiceTotalVATs
                             where a.Tenant == tenant && ids.Contains(a.ARInvoiceId)
                             select a).ToList();
            }

            return myResult;
        }

        public void Add(ARInvoiceTotalVAT entity)
        {
            context.ARInvoiceTotalVATs.Add(entity);
        }

        public void Remove(ARInvoiceTotalVAT entity)
        {
            context.ARInvoiceTotalVATs.Attach(entity);
            context.ARInvoiceTotalVATs.Remove(entity);
        }

        public void Update(ARInvoiceTotalVAT entity)
        {
            context.ARInvoiceTotalVATs.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ARInvoiceTotalVAT> All()
        {
            return context.ARInvoiceTotalVATs.ToList();
        }

        public IInvoiceContext context
        {
            get { return invoiceContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }




        public List<ARInvoiceTotalVAT> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public ARInvoiceTotalVAT GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public List<ARInvoiceTotalVAT> GetInvoiceTotalVatsForInvoiceWithoutZeroVATPercent(string invoiceId, int tenant)
        {
            return (from a in context.ARInvoiceTotalVATs
                   where a.Tenant == tenant && a.ARInvoiceId == invoiceId && a.VatPercent != 0
                   select a).ToList();
        }
    }
}