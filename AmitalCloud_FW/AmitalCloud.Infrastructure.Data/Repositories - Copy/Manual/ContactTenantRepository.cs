using AmitalCloud.Infrastructure.Data.Context;
using AmitalCloud.Infrastructure.Data.Interfaces;
using AmitalCloud.Infrastructure.Data.EntityPOCOs;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

namespace AmitalCloud.Infrastructure.Data.Repositories
{
    public class ContactTenantRepository:IRepository<ContactTenant,string>
    {

        IAmitalCloudContext currentContext;
        public ContactTenantRepository()
        {
            currentContext = new AmitalCloudContext();
        }

        public ContactTenantRepository(IAmitalCloudContext context)
        {
            currentContext = context;
        }
        public ContactTenantRepository(int tenant)
        {
            currentContext = AmitalCloudContext.GetContext(tenant);
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

        public IAmitalCloudContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            this.context.SaveChanges();
        }


        public List<ContactTenant> GetMulti(IEntityKeyFields<ContactTenant,string> entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public ContactTenant GetSingle(IEntityKeyFields<ContactTenant, string> entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}