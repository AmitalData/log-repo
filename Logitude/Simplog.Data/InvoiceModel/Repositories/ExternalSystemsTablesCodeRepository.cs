using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.InvoiceModel.Repositories
{
   public class ExternalSystemsTablesCodeRepository : IRepository<ExternalSystemsTablesCode>
    {

         IInvoiceContext invoiceContext;
        public ExternalSystemsTablesCodeRepository()
        {
            invoiceContext = new InvoiceContext();
        }
        public ExternalSystemsTablesCodeRepository(int tenant)
        {
            invoiceContext = InvoiceContext.GetContext(tenant);
        }
        public ExternalSystemsTablesCodeRepository(IInvoiceContext context)
        {
            invoiceContext = context;
        }

        public IQueryable<ExternalSystemsTablesCode> GetExternalSystemsTablesCodes()
        {
            return context.ExternalSystemsTablesCodes;
        }

        public IQueryable<ExternalSystemsTablesCode> GetExternalSystemsTablesCodes(int tenant)
        {
            return (from record in context.ExternalSystemsTablesCodes where record.Tenant == tenant select record);
        }

        //public IQueryable<ExternalSystemsTablesCode> GetExternalSystemsTablesCodesByTenant(int tenant)
        //{
        //    return (from record in context.ExternalSystemsTablesCodes where record.Tenant == tenant select record);
        //}

     

        public ExternalSystemsTablesCode GetSingleExternalSystemsTablesCode(string id, int tenant)
        {
            return (from record in context.ExternalSystemsTablesCodes where record.Id == id && record.Tenant == tenant select record).FirstOrDefault();          
        }

        public ExternalSystemsTablesCode GetSingleExternalSystemsTablesCodeByCode(string code, int tenant)
        {
            return (from record in context.ExternalSystemsTablesCodes where record.Code == code && record.Tenant == tenant select record).FirstOrDefault();
        }

      
        public void Add(ExternalSystemsTablesCode entity)
        {
            context.ExternalSystemsTablesCodes.Add(entity);
        }

        public void Remove(ExternalSystemsTablesCode entity)
        {
            context.ExternalSystemsTablesCodes.Attach(entity);
            context.ExternalSystemsTablesCodes.Remove(entity);
        }

        public void Update(ExternalSystemsTablesCode entity)
        {
            context.ExternalSystemsTablesCodes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ExternalSystemsTablesCode> All()
        {
            return context.ExternalSystemsTablesCodes.ToList();
        }

        public IInvoiceContext context
        {
            get {return invoiceContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }




        public List<ExternalSystemsTablesCode> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public ExternalSystemsTablesCode GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }


     
    }
}
