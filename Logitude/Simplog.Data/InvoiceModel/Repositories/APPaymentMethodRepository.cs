using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.InvoiceModel.Repositories
{
    public class APPaymentMethodRepository: IRepository<APPaymentMethod>
    {
        IInvoiceContext invoiceContext;

        public APPaymentMethodRepository(IInvoiceContext context)
        {
            invoiceContext = context;
        }

        public APPaymentMethodRepository()
        {
            invoiceContext = new InvoiceContext();
        }

        public APPaymentMethodRepository(int tenant)
        {
            invoiceContext = InvoiceContext.GetContext(tenant);
        }

        public APPaymentMethod GetSingleAPPaymentMethod(string id, int tenant)
        {
            return (from a in context.APPaymentMethods where a.Id == id && a.Tenant == tenant select a).FirstOrDefault();
        }

        public APPaymentMethod GetSingleAPPaymentMethodByCode(string code, int tenant)
        {
            return (from a in context.APPaymentMethods where a.Code == code && a.Tenant == tenant select a).FirstOrDefault();           
        }
             
        public IQueryable<APPaymentMethod> GetAPPaymentMethods(int tenant)
        {
            return (from a in context.APPaymentMethods where a.Tenant == tenant select a);
        }

        public void Add(APPaymentMethod entity)
        {
            context.APPaymentMethods.Add(entity);
        }

        public void Remove(APPaymentMethod entity)
        {
            context.APPaymentMethods.Attach(entity);
            context.APPaymentMethods.Remove(entity);
        }

        public void Update(APPaymentMethod entity)
        {
            try
            {
                context.APPaymentMethods.Attach(entity);
            }
            catch { }
            context.SetAsModified(entity);
        }

        public List<APPaymentMethod> All()
        {
            return context.APPaymentMethods.ToList();
        }

        public IInvoiceContext context
        {
            get {return invoiceContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<APPaymentMethod> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public APPaymentMethod GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }
    }
}