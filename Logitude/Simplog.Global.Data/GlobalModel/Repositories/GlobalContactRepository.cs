using System.Collections.Generic;
using System.Linq;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Global.Data.GlobalModel;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;

namespace Simplog.Global.Data.GlobalModel.Repositories
{
    public class GlobalContactRepository:IRepository<GlobalContact>
    {
        IGlobalContext globalContext;

        public GlobalContactRepository(IGlobalContext context)
        {
            globalContext = context;
        }

        public GlobalContactRepository()
        {
            globalContext = GlobalContext.GetContext();
        }
        public GlobalContact GetGlobalContactByEmailAndTenant(string email, int tenant)
        {
            GlobalContact globalContact = (from a in context.GlobalContacts.Include("GlobalTenant")
                    where a.Email == email && a.GlobalTenantId == 0 && a.InActive==false
                    select a).FirstOrDefault();
            if (globalContact == null)
            {
                globalContact = (from a in context.GlobalContacts.Include("GlobalTenant")
                                 where a.Email == email && a.GlobalTenantId == tenant
                                 select a).FirstOrDefault();
            }

            string entityName = "GlobalContact" + email + tenant;
            if (CacheManager.CacheWrapper != null)
            {
                if (CacheManager.CacheWrapper.Get(entityName) == null && globalContact != null)
                {
                    CacheManager.CacheWrapper.Insert(entityName, globalContact, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                }
                else
                {
                    globalContact = (GlobalContact)CacheManager.CacheWrapper.Get(entityName);
                }
            }
            return globalContact;
        }
        public IQueryable<GlobalContact> GetGlobalContactByTenant(int tenant)
        {
            IQueryable<GlobalContact> contacts = from a in context.GlobalContacts.Include("GlobalTenant")
                                                 where a.GlobalTenantId == tenant
                                                 select a;

            string entityName = "GlobalContacts" + tenant;
            if (CacheManager.CacheWrapper != null)
            {
                if (CacheManager.CacheWrapper.Get(entityName) == null && contacts != null)
                {
                    CacheManager.CacheWrapper.Insert(entityName, contacts, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                }
                else
                {
                    contacts = (IQueryable<GlobalContact>)CacheManager.CacheWrapper.Get(entityName);
                }
            }
            return contacts;
        }

        public IQueryable<GlobalContact> GetContactByEmail(string email)
        {
            IQueryable<GlobalContact> contacts = from a in context.GlobalContacts.Include("GlobalTenant")
                                                 where a.Email == email && a.InActive == false
                                                 select a;

            string entityName = "GlobalContacts" + email;
            if (CacheManager.CacheWrapper != null)
            {
                if (CacheManager.CacheWrapper.Get(entityName) == null && contacts != null)
                {
                    CacheManager.CacheWrapper.Insert(entityName, contacts, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                }
                else
                {
                    contacts = (IQueryable<GlobalContact>)CacheManager.CacheWrapper.Get(entityName);
                }
            }
            return contacts;
        }

        public GlobalContact GetSingleGlobalContact(string id)
        {
            GlobalContact globalContact = (from a in context.GlobalContacts.Include("GlobalTenant")
                   where a.Id == id
                   select a).FirstOrDefault();

            string entityName = "GlobalContactById" + id;
            if (CacheManager.CacheWrapper != null)
            {
                if (CacheManager.CacheWrapper.Get(entityName) == null && globalContact != null)
                {
                    CacheManager.CacheWrapper.Insert(entityName, globalContact, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                }
                else
                {
                    globalContact = (GlobalContact)CacheManager.CacheWrapper.Get(entityName);
                }
            }
            return globalContact;
        }


        public List<GlobalContact> GetSystemGlobalContacts()
        {
            IQueryable<GlobalContact> contacts = from a in context.GlobalContacts.Include("GlobalTenant")
                                                 where a.Email.StartsWith("system@tenant")
                                                 select a;
            string entityName = "GlobalContactsList";
            if (CacheManager.CacheWrapper != null)
            {
                if (CacheManager.CacheWrapper.Get(entityName) == null && contacts != null)
                {
                    CacheManager.CacheWrapper.Insert(entityName, contacts, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                }
                else
                {
                    contacts = (IQueryable<GlobalContact>)CacheManager.CacheWrapper.Get(entityName);
                }
            }
            return contacts.ToList();
        }


        public void Add(GlobalContact entity)
        {
            context.GlobalContacts.Add(entity);
        }

        public void Remove(GlobalContact entity)
        {
            context.GlobalContacts.Attach(entity);
            context.GlobalContacts.Remove(entity);
        }

        public void Update(GlobalContact entity)
        {
            context.GlobalContacts.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<GlobalContact> All()
        {
            return context.GlobalContacts.ToList();
        }

        public IGlobalContext context
        {
            get {return globalContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<GlobalContact> GetMulti(EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public GlobalContact GetSingle(EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}
