using AmitalCloud.Infrastructure.Data.Context;
using AmitalCloud.Infrastructure.Domain.EntityPMs;
using AmitalCloud.Infrastructure.Domain.Interfaces;
using AmitalCloud.Infrastructure.Data.Repositories;
using System.Linq;
using AmitalCloud.Infrastructure.Domain.EntityPOCOs;

namespace AmitalCloud.Infrastructure.Data.Queries
{
    public class ContactTenantQuery
    {
        IRepository<ContactTenant> repository;
        public ContactTenantQuery() : this(0)   
        {
        }
        public ContactTenantQuery(int tenant)
        {
            repository = new Repository<ContactTenant>(AmitalCloudContext.GetContext(tenant));
        }
        public ContactTenantQuery(IRepository< ContactTenant> contactTenantRepository)
        {
            repository = contactTenantRepository;
        }
        public ContactTenantPM GetContactTenantForUser(string contactId, int tenant)
        {
            return (from a in repository.GetMulti(a => a.ContactId == contactId && a.TenantId == tenant)   
                                 select new ContactTenantPM()
                                 {
                                     ContactId = a.ContactId,
                                     Id = a.Id,
                                     TenantId = a.TenantId,
                                 }).FirstOrDefault();

        }
        public ContactTenantPM GetSinglePM(string id, int tenant)
        {
            return (from a in repository.GetMulti(a => a.Id == id && a.TenantId == tenant)
                                 select new ContactTenantPM()
                                 {
                                     ContactId = a.ContactId,
                                     Id = a.Id,
                                     TenantId = a.TenantId,
                                 }).FirstOrDefault();
        }
     }
}
