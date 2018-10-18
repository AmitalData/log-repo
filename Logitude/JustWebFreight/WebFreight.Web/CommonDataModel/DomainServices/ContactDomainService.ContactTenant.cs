using System.Collections.Generic;
using System.Linq;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using WebFreight.Web.Security;

namespace WebFreight.Web.CommonDataModel.DomainServices
{
    public partial class ContactDomainService
    {
        public IQueryable<ContactTenant> GetContactTenants(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            contactTenantRepository = new ContactTenantRepository(tenant);
            return contactTenantRepository.GetContactTenants(tenant);
        }

        public List<TenantPM> GetTenantsForContactsByEmail(string email, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(tenant);
            }
            contactTenantRepository = new ContactTenantRepository(objectContext);
            contactRepository = new ContactRepository(objectContext);
         
            List<Contact> contacts = contactRepository.GetActiveContacts(tenant).Where(d => d.Email == email).ToList<Contact>();
            List<ContactTenant> contactTenants = new List<ContactTenant>();
            List<Tenant> tenants = new List<Tenant>();

            foreach (Contact contact in contacts)
            {
                foreach (ContactTenant contactTenant in this.GetContactTenantsForContact(contact.Id, tenant))
                {
                    tenants.Add(contactTenant.Tenant);
                }
            }
            return (from a in tenants
                    select new TenantPM()
                    {
                        AddressId = a.AddressId,
                        Company = a.Company,
                        CurrencyId = a.CurrencyId,
                        Direction = a.Direction,
                        Email = a.Email,
                        Format = a.Format,
                        Id = a.Id,
                        Language = a.Language,
                        Website = a.Website,
                    }).ToList();
        }

        public IQueryable<ContactTenant> GetContactTenantsForContact(string contactId, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            contactTenantRepository = new ContactTenantRepository(tenant);
            return contactTenantRepository.GetContactTenants(tenant).Where(ct => ct.ContactId == contactId);
        }

        //public void MapContactTenantContactTenantPM(ContactTenantPM contactTenantPm, ContactTenant contactTenant)
        //{
        //    contactTenant.ContactId = contactTenantPm.ContactId;
        //    contactTenant.Id = contactTenantPm.Id;
        //    contactTenant.TenantId = contactTenantPm.TenantId;
        //}

        public void InsertContactTenant(ContactTenantPM contactTenant)
        {
            ContactTenantService service = new ContactTenantService(objectContext, contactTenant.TenantId);
            service.Create(contactTenant);

            //contactTenantRepository = new ContactTenantRepository(contactTenant.TenantId);
            //ContactTenant newContactTenant = new ContactTenant();
            //newContactTenant.ContactTenantRoles.Add(new ContactTenantRole() { ContactTenant = newContactTenant });
            //contactTenant.ContactId = newContactTenant.ContactId;
            //MapContactTenantContactTenantPM(contactTenant, newContactTenant);
            //contactTenantRepository.Add(newContactTenant);
        }

        public void UpdateContactTenant(ContactTenantPM currentContactTenant)
        {

            ContactTenantService service = new ContactTenantService(objectContext, currentContactTenant.TenantId);
            service.Update(currentContactTenant);

            //contactTenantRepository = new ContactTenantRepository(currentContactTenant.TenantId);
            //ContactTenant entity = contactTenantRepository.GetSingleContactTenant(currentContactTenant.Id, currentContactTenant.TenantId);
            //MapContactTenantContactTenantPM(currentContactTenant, entity);
            //contactTenantRepository.Update(entity);
        }

        public void DeleteContactTenant(ContactTenant contactTenant)
        {
            contactTenantRepository = new ContactTenantRepository(contactTenant.TenantId);
            ContactTenant entity = contactTenantRepository.GetSingleContactTenant(contactTenant.Id, contactTenant.TenantId);
            contactTenantRepository.Remove(entity);
        }
    }
}