using System.Collections.Generic;
using System.Linq;
using Simplog.Server.Infrastructure;
using Simplog.Data.InvoiceModel.EntityPOCOs;

namespace Simplog.Data.InvoiceModel.Repositories
{
    public class APInvoiceTypeRepository: IRepository<APInvoiceType>
    {

        IInvoiceContext invoiceContext;
        public APInvoiceTypeRepository()
        {
            invoiceContext = new InvoiceContext();

        }

        public APInvoiceTypeRepository(IInvoiceContext context)
        {
            invoiceContext = context;
        }
        public APInvoiceTypeRepository(int tenant)
        {
            invoiceContext = InvoiceContext.GetContext(tenant);
        }
        public APInvoiceType GetSingleAPInvoiceType(string code)
        {
            return (from a in context.APInvoiceTypes
                    where a.Code == code
                    select a).FirstOrDefault();
        }

      
        public IQueryable<APInvoiceType> GetAPInvoiceTypes()
        {
            return (from a in context.APInvoiceTypes
                    
                    select a);
        }
        public IQueryable<APInvoiceType> GetAll()
        {
            return (from a in context.APInvoiceTypes

                    select a);
        }


        public void Add(APInvoiceType entity)
        {
            context.APInvoiceTypes.Add(entity);
        }

        public void Remove(APInvoiceType entity)
        {
            context.APInvoiceTypes.Attach(entity);
            context.APInvoiceTypes.Remove(entity);
        }

        public void Update(APInvoiceType entity)
        {
            context.APInvoiceTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<APInvoiceType> All()
        {
            return  context.APInvoiceTypes.ToList();
        }

        public IInvoiceContext context
        {
            get { return invoiceContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }




        public List<APInvoiceType> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public APInvoiceType GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}