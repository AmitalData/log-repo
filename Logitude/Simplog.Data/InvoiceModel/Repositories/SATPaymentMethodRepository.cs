using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.InvoiceModel.Repositories
{
    public class SATPaymentMethodRepository : IRepository<SATPaymentMethod>
    {
        IInvoiceContext invoiceContext;

        public SATPaymentMethodRepository(IInvoiceContext context)
        {
            invoiceContext = context;
        }

        public SATPaymentMethodRepository()
        {
            invoiceContext = new InvoiceContext();
        }

        public SATPaymentMethodRepository(int tenant)
        {
            invoiceContext = InvoiceContext.GetContext(tenant);
        }

        public SATPaymentMethod GetSingleSATPaymentMethod(string code)
        {
            return (from a in context.SATPaymentMethods where a.Code == code select a).FirstOrDefault();
        }
        public IQueryable<SATPaymentMethod> GetAll()
        {
            return (from a in context.SATPaymentMethods select a);
        }
        public IQueryable<SATPaymentMethod> GetSATPaymentMethods()
        {
            return (from a in context.SATPaymentMethods select a);
        }

        public void Add(SATPaymentMethod entity)
        {
            context.SATPaymentMethods.Add(entity);
        }

        public void Remove(SATPaymentMethod entity)
        {
            context.SATPaymentMethods.Attach(entity);
            context.SATPaymentMethods.Remove(entity);
        }

        public void Update(SATPaymentMethod entity)
        {
            try
            {
                context.SATPaymentMethods.Attach(entity);
            }
            catch { }
            context.SetAsModified(entity);
        }

        public List<SATPaymentMethod> All()
        {
            return context.SATPaymentMethods.ToList();
        }

        public IInvoiceContext context
        {
            get { return invoiceContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<SATPaymentMethod> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public SATPaymentMethod GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }
    }
}