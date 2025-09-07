using System.Collections.Generic;
using System.Linq;
using Simplog.Server.Infrastructure;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure.Helpers;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class ContactTenantRepository:IRepository<ContactTenant>
    {

        ICommonDataContext commonDataContext;


        public ContactTenantRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }
        public ContactTenantRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }
        public IQueryable<ContactTenant> GetContactTenants(int tenant)
        {
            return (from record in context.ContactTenants where record.TenantId == tenant select record);
        }

        public IQueryable<ContactTenant> GetContactTenantForContact(string email)
        {
            var contactTenant = (from a in context.ContactTenants
                                 where a.Contact.Email == email.ToLower()
                                 select a);
            return contactTenant;
        }

        public ContactTenant GetContactTenantForContactId(string id,int tenant)
        {
            var contactTenant = (from a in context.ContactTenants
                                 where a.Contact.Id == id && a.TenantId == tenant
                                 select a).FirstOrDefault(); ;
            return contactTenant;
        }

       
        public ContactTenant GetSingleContactTenant(string id, int tenant)
        {
            return (from record in context.ContactTenants.Include("Contact") 
                    where record.Id == id && record.TenantId == tenant select record).FirstOrDefault();
        }

        public void Add(ContactTenant entity)
        {
            this.context.ContactTenants.Add(entity);
        }

        public void Remove(ContactTenant entity)
        {
            this.context.ContactTenants.Attach(entity);
            this.context.ContactTenants.Remove(entity);
        }

        public void Update(ContactTenant entity)
        {
            this.context.ContactTenants.Attach(entity);
            this.context.SetAsModified(entity);
        }

        public List<ContactTenant> All()
        {
            return this.context.ContactTenants.ToList<ContactTenant>();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            this.context.SaveChanges();
        }


        public List<ContactTenant> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public ContactTenant GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}