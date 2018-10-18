using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using System.Web;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.InvoiceModel.Repositories
{
    public class CreditCardTypeRepository : IRepository<CreditCardType>
    {
        IInvoiceContext invoiceContext;

        public CreditCardTypeRepository(IInvoiceContext context)
        {
            invoiceContext = context;
        }

        public CreditCardTypeRepository()
        {
            invoiceContext = new InvoiceContext();
        }

        public CreditCardTypeRepository(int tenant)
        {
            invoiceContext = InvoiceContext.GetContext(tenant);
        }

        public CreditCardType GetSingleCreditCardType(string id, int tenant)
        {
            return (from a in context.CreditCardTypes where a.Id == id && a.Tenant == tenant select a).FirstOrDefault();
        }

        public IQueryable<CreditCardType> GetCreditCardTypes(int tenant)
        {
            return (from a in context.CreditCardTypes where a.Tenant == tenant select a);
        }

        public void Add(CreditCardType entity)
        {
            context.CreditCardTypes.Add(entity);
        }

        public void Remove(CreditCardType entity)
        {
            context.CreditCardTypes.Attach(entity);
            context.CreditCardTypes.Remove(entity);
        }

        public void Update(CreditCardType entity)
        {
            try
            {
                context.CreditCardTypes.Attach(entity);
            }
            catch { }
            context.SetAsModified(entity);
        }

        public List<CreditCardType> All()
        {
            return context.CreditCardTypes.ToList();
        }

        public IInvoiceContext context
        {
            get { return invoiceContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<CreditCardType> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public CreditCardType GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }
    }
}
