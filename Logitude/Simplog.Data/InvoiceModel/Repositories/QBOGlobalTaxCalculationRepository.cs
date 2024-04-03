using System;
using System.Collections.Generic;
using System.Linq;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Server.Infrastructure;


namespace Simplog.Data.InvoiceModel.Repositories
{
    public class QBOGlobalTaxCalculationRepository : IRepository<QBOGlobalTaxCalculation>
    {
        IInvoiceContext invoiceContext;

        public QBOGlobalTaxCalculationRepository(IInvoiceContext context)
        {
            invoiceContext = context;
        }

        public QBOGlobalTaxCalculationRepository()
        {
            invoiceContext = new InvoiceContext();
        }

        public QBOGlobalTaxCalculationRepository(int tenant)
        {
            invoiceContext = InvoiceContext.GetContext(tenant);
        }

        public QBOGlobalTaxCalculation GetSingleQBOGlobalTaxCalculation(string code)
        {
            return (from a in context.QBOGlobalTaxCalculations where a.Code == code select a).FirstOrDefault();
        }

        public IQueryable<QBOGlobalTaxCalculation> GetQBOGlobalTaxCalculations()
        {
            return (from a in context.QBOGlobalTaxCalculations select a);
        }
        public IQueryable<QBOGlobalTaxCalculation> GetAll()
        {
            return (from a in context.QBOGlobalTaxCalculations select a);
        }

        public void Add(QBOGlobalTaxCalculation entity)
        {
            context.QBOGlobalTaxCalculations.Add(entity);
        }

        public void Remove(QBOGlobalTaxCalculation entity)
        {
            context.QBOGlobalTaxCalculations.Attach(entity);
            context.QBOGlobalTaxCalculations.Remove(entity);
        }

        public void Update(QBOGlobalTaxCalculation entity)
        {
            try
            {
                context.QBOGlobalTaxCalculations.Attach(entity);
            }
            catch { }
            context.SetAsModified(entity);
        }

        public List<QBOGlobalTaxCalculation> All()
        {
            return context.QBOGlobalTaxCalculations.ToList();
        }

        public IInvoiceContext context
        {
            get { return invoiceContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<QBOGlobalTaxCalculation> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public QBOGlobalTaxCalculation GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }
    }
}