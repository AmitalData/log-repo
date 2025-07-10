using AmitalCloud.Infrastructure.Data.Context;
using AmitalCloud.Infrastructure.Data.Helpers;
using AmitalCloud.Infrastructure.Data.Repositories;
using AmitalCloud.Infrastructure.Domain.EntityLists;
using AmitalCloud.Infrastructure.Domain.EntityPMs;
using AmitalCloud.Infrastructure.Domain.EntityClasses;
using AmitalCloud.Infrastructure.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Transactions;
using System.Web;
namespace AmitalCloud.Infrastructure.Data.Queries
{
    public class ContactQuery
    {
        IRepository<Contact> repository;
        IAmitalCloudContext context;
 
        public ContactQuery(int tenant)
        {
            context = AmitalCloudContext.GetContext(tenant);
            repository = new Repository<Contact>(context);
        }
        #region GetSingle ContactPM
        private ContactPM GetContactPMFromCache(int tenant, string cacheKey, Expression<Func<ContactPM, bool>> predicate)
        {
            ContactPM entity;
            if (HttpContext.Current != null)
            {
                entity = (ContactPM)CacheManager.CacheWrapper.Get(cacheKey);
                if (entity == null)
                {
                    entity = GetEntityPMWithPassword(tenant, predicate);
                    if (entity != null)
                    {
                        if (CacheManager.CacheWrapper.Get(cacheKey) == null)
                        {
                            CacheManager.CacheWrapper.Insert(cacheKey, entity, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                        }
                    }
                }
            }
            else
            {
                entity = GetEntityPMWithPassword(tenant, predicate);
            }

            return entity;
        }
        private IQueryable<ContactPM> GetContactPMQuery()
        => (from a in context.Contacts
                    where a.UserType == "R"
                    let contact = new ContactPM(a)
                    {
                        //ComputedLocalName = string.IsNullOrEmpty(a.LocalName) ? a.EnglishName : a.LocalName,
                        //DontShowLocal = a.DontShowLocalLabels,
                        ContactDoneMethodCode = a.ContactDoneMethod != null ? a.ContactDoneMethod.Code : null,
                        //ContactDoneMethodName = a.ContactDoneMethod != null ? a.ContactDoneMethod.Name : null,
                    }
                    select contact);
        private void GetContactPassword(ContactPM instance)
        {
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                IGlobalContext globalContext = GlobalContext.GetContext();
                ContactPassword contactPassword = globalContext.ContactPasswords.Where(cn => cn.Email == instance.Email.ToLower()).FirstOrDefault();
                GlobalContact globalContact = globalContext.GlobalContacts.Where(cn => cn.Email == instance.Email.ToLower() && cn.GlobalTenantId == instance.Tenant).FirstOrDefault();
                if (globalContact != null)
                {
                    //instance.IsUser = globalContact.IsUser;
                }
                if (contactPassword != null)
                {
                    //instance.Password = contactPassword.Password;
                    //instance.IsLocked = contactPassword.IsLocked;
                    //instance.MustChangePassword = contactPassword.MustChangePassword;
                    //instance.NumberOfRetries = contactPassword.NumberOfRetries;
                }
                scope.Complete();
            }
        }
        private ContactPM GetEntityPMWithPassword(int tenant, Expression<Func<ContactPM, bool>> predicate)
        {
            ContactPM entity = GetContactPMQuery().Where(predicate).FirstOrDefault();
            if (entity == null)
            {
                tenant = 0;
                entity = GetContactPMQuery().Where(predicate).FirstOrDefault();
            }
            if (entity != null)
            {
                GetContactPassword(entity);
            }

            return entity;
        }
        public ContactPM GetSingleContact(string email, int tenant) => GetEntityPMWithPassword(tenant, a => a.Email == email.ToLower() && a.Tenant == tenant);
        public ContactPM GetContactByEmailOnly(string email, int tenant) => GetContactPMFromCache(tenant, $"ContactPM_({email}_{tenant})".ToLower(), a => a.Email == email && a.InActive == false && a.Tenant == tenant);
        public ContactPM GetSingleByEmail(string email, int tenant) => GetSingleContact(email, tenant);
        #endregion GetSingle ContactPM  
        public IQueryable<ContactList> GetContactListsByListIds(List<string> contactIds, int tenant)
        => (from a in context.Contacts where contactIds.Contains(a.Id) && a.Tenant == tenant
                                                    select new ContactList(a));

    }
}