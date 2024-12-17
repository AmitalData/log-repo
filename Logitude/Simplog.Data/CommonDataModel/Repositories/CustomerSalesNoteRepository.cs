using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class CustomerSalesNoteRepository : IRepository<CustomerSalesNote>
    {
        ICommonDataContext Context;
        public ICommonDataContext context
        {
            get { return Context; }
        }

        public CustomerSalesNoteRepository()
        {
            this.Context = new CommonDataContext();
        }

        public CustomerSalesNoteRepository(int tenant)
        {
            this.Context = CommonDataContext.GetContext(tenant);
        }

        public CustomerSalesNoteRepository(ICommonDataContext context)
        {
            this.Context = context;
        }

        public IQueryable<CustomerSalesNote> GetCustomerSalesNotes()
        {
            return context.CustomerSalesNotes;
        }

        public IQueryable<CustomerSalesNote> GetCustomerSalesNotesByTenant(int tenant)
        {
            return context.CustomerSalesNotes.Where(d => d.Tenant == tenant);
        }

        public IQueryable<CustomerSalesNote> GetCustomerSalesNotesByCustomer(string customerId, int tenant)
        {
            return context.CustomerSalesNotes.Where(d => d.CustomerId == customerId && d.Tenant == tenant);
        }

        public CustomerSalesNote GetSingleCustomerSalesNote(string id, int tenant)
        {
            return (from a in context.CustomerSalesNotes where a.Id == id && a.Tenant == tenant select a).FirstOrDefault();
        }

        public void Add(CustomerSalesNote entity)
        {
            context.CustomerSalesNotes.Add(entity);
        }

        public void Remove(CustomerSalesNote entity)
        {
            context.CustomerSalesNotes.Remove(entity);
        }

        public void Update(CustomerSalesNote entity)
        {
            context.CustomerSalesNotes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CustomerSalesNote> All()
        {
            return context.CustomerSalesNotes.ToList();
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<CustomerSalesNote> GetMulti(EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public CustomerSalesNote GetSingle(EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

    }
}
