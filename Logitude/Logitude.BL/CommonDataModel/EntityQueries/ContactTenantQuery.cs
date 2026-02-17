using System;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using Logitude.BL.Helpers;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityLists;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;

namespace Logitude.BL.CommonDataModel.EntityQueries
{
    public class ContactTenantQuery
    {
        ContactTenantRepository repository;
        public ContactTenantQuery()
        {
            repository = new ContactTenantRepository(); 
        }

        public ContactTenantQuery(int tenant)
        {
            repository = new ContactTenantRepository(tenant);
        }

        public ContactTenantQuery(ContactTenantRepository contactTenantRepository)
        {
            repository = contactTenantRepository;
        }

        public ContactTenantPM GetContactTenantForUser(string contactId, int tenant)
        {
            var contactTenant = (from a in repository.context.ContactTenants
                                 where a.ContactId == contactId && a.TenantId == tenant
                                 select new ContactTenantPM()
                                 {
                                     ContactId = a.ContactId,
                                     Id = a.Id,
                                     TenantId = a.TenantId,
                                 }).FirstOrDefault();

         
            return contactTenant;
        }

        public ContactTenantPM GetSinglePM(string id, int tenant)
        {
            var contactTenant = (from a in repository.context.ContactTenants
                                 where a.Id == id && a.TenantId == tenant
                                 select new ContactTenantPM()
                                 {
                                     ContactId = a.ContactId,
                                     Id = a.Id,
                                     TenantId = a.TenantId,
                                 }).FirstOrDefault();
            return contactTenant;
        }

        public IQueryable<ContactTenantPM> GetContactTenantPMsByTenant(int tenant)
        {
            IQueryable<ContactTenantPM> contactTenants = from a in repository.context.ContactTenants
                                                         where a.TenantId == tenant
                                                         select new ContactTenantPM()
                                                         {
                                                             ContactId = a.ContactId,
                                                             Id = a.Id,
                                                             TenantId = a.TenantId,
                                                         };
            return contactTenants;
        }
    }
}
