using System;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using Logitude.BL.Helpers;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityLists;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;

namespace Logitude.BL.CommonDataModel.EntityQueries
{
    public class ContactTenantQuery
    {
        ContactTenantRepository repository;


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
            NetCommonHelper.Logger.DevLog.Instance.WriteInfo("GetContactTenantForUser: contactId:" + contactId + "tenant: " + tenant.ToString());
            NetCommonHelper.Logger.DevLog.Instance.WriteInfo("GetContactTenantForUser before repository.context.GetConnection().Database" + repository.context.GetConnection()?.Database);

            var contactTenant = (from a in repository.context.ContactTenants
                                 where a.ContactId == contactId && a.TenantId == tenant
                                 select new ContactTenantPM()
                                 {
                                     ContactId = a.ContactId,
                                     Id = a.Id,
                                     TenantId = a.TenantId,
                                 }).FirstOrDefault();
            NetCommonHelper.Logger.DevLog.Instance.WriteInfo("GetContactTenantForUser vafter repository.context.GetConnection().Database" + repository.context.GetConnection()?.Database);

            NetCommonHelper.Logger.DevLog.Instance.WriteInfo("GetContactTenantForUser contactTenant" + contactTenant?.Id);

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
