using System.Collections.Generic;
using System.Linq;
using Simplog.Server.Infrastructure;
using Simplog.Data.InvoiceModel.EntityPOCOs;

namespace Simplog.Data.InvoiceModel.Repositories
{
    public class ARInvoiceTypeRepository : IRepository<ARInvoiceType>
    {

        IInvoiceContext invoiceContext;
        public ARInvoiceTypeRepository()
        {
            invoiceContext = new InvoiceContext();
        }

        public ARInvoiceTypeRepository(IInvoiceContext context)
        {
            invoiceContext = context;
        }
        public ARInvoiceTypeRepository(int tenant)
        {
            invoiceContext = InvoiceContext.GetContext(tenant);
        }
        public IQueryable<ARInvoiceType> GetARInvoiceTypes()
        {
            return context.ARInvoiceTypes;
        }
        public IQueryable<ARInvoiceType> GetAll()
        {
            return context.ARInvoiceTypes;
        }

        public ARInvoiceType GetSingleARInvoiceType(string code)
        {
            return (from a in context.ARInvoiceTypes
                    where a.Code == code
                    select a).FirstOrDefault();
        }

     

        public void Add(ARInvoiceType entity)
        {
            context.ARInvoiceTypes.Add(entity);
        }

        public void Remove(ARInvoiceType entity)
        {
            context.ARInvoiceTypes.Attach(entity);
            context.ARInvoiceTypes.Remove(entity);
        }

        public void Update(ARInvoiceType entity)
        {
            context.ARInvoiceTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ARInvoiceType> All()
        {
            return context.ARInvoiceTypes.ToList();
        }

        public IInvoiceContext context
        {
            get {return invoiceContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }





        public List<ARInvoiceType> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public ARInvoiceType GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}