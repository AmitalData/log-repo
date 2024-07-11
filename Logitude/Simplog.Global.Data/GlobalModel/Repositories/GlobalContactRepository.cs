using System.Collections.Generic;
using System.Linq;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Global.Data.GlobalModel;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure;

namespace Simplog.Global.Data.GlobalModel.Repositories
{
    public class GlobalContactRepository : IRepository<GlobalContact>
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
                                           where a.Email == email && a.GlobalTenantId == 0 && a.InActive == false
                                           select a).FirstOrDefault();
            if (globalContact == null)
            {
                globalContact = (from a in context.GlobalContacts.Include("GlobalTenant")
                                 where a.Email == email && a.GlobalTenantId == tenant
                                 select a).FirstOrDefault();
            }
            return globalContact;
        }
        public IQueryable<GlobalContact> GetGlobalContactByTenant(int tenant)
        {
            IQueryable<GlobalContact> contacts = from a in context.GlobalContacts.Include("GlobalTenant")
                                                 where a.GlobalTenantId == tenant
                                                 select a;
            return contacts;
        }

        public IQueryable<GlobalContact> GetContactByEmail(string email)
        {
            IQueryable<GlobalContact> contacts = from a in context.GlobalContacts.Include("GlobalTenant")
                                                 where a.Email == email && a.InActive == false
                                                 select a;
            return contacts;
        }

        public GlobalContact GetSingleGlobalContact(string id)
        {
            return (from a in context.GlobalContacts.Include("GlobalTenant")
                    where a.Id == id
                    select a).FirstOrDefault();
        }


        public List<GlobalContact> GetSystemGlobalContacts()
        {
            IQueryable<GlobalContact> contacts = from a in context.GlobalContacts.Include("GlobalTenant")
                                                 where a.Email.StartsWith("system@tenant")
                                                 select a;
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
            get { return globalContext; }
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
