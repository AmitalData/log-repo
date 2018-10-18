using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.InvoiceModel.Repositories
{
    public class AccountingPaymentMethodRepository : IRepository<AccountingPaymentMethod>
    {
        IInvoiceContext invoiceContext;

        public AccountingPaymentMethodRepository(IInvoiceContext context)
        {
            invoiceContext = context;
        }

        public AccountingPaymentMethodRepository()
        {
            invoiceContext = new InvoiceContext();
        }

        public AccountingPaymentMethodRepository(int tenant)
        {
            invoiceContext = InvoiceContext.GetContext(tenant);
        }

        public AccountingPaymentMethod GetSingleAccountingPaymentMethod(string id, int tenant)
        {
            return (from a in context.AccountingPaymentMethods where a.Id == id && a.Tenant == tenant select a).FirstOrDefault();
        }

        public AccountingPaymentMethod GetSingleAccountingPaymentMethodByCode(string code, int tenant)
        {
            return (from a in context.AccountingPaymentMethods where a.Code == code && a.Tenant == tenant select a).FirstOrDefault();
        }

        public IQueryable<AccountingPaymentMethod> GetAccountingPaymentMethods(int tenant)
        {
            return (from a in context.AccountingPaymentMethods where a.Tenant == tenant select a);
        }

        public void Add(AccountingPaymentMethod entity)
        {
            context.AccountingPaymentMethods.Add(entity);
        }

        public void Remove(AccountingPaymentMethod entity)
        {
            context.AccountingPaymentMethods.Attach(entity);
            context.AccountingPaymentMethods.Remove(entity);
        }

        public void Update(AccountingPaymentMethod entity)
        {
            try
            {
                context.AccountingPaymentMethods.Attach(entity);
            }
            catch { }
            context.SetAsModified(entity);
        }

        public List<AccountingPaymentMethod> All()
        {
            return context.AccountingPaymentMethods.ToList();
        }

        public IInvoiceContext context
        {
            get {return invoiceContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<AccountingPaymentMethod> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public AccountingPaymentMethod GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }
    }
}