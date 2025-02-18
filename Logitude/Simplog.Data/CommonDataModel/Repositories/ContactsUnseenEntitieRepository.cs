using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Repositories
{
  public  class ContactsUnseenEntitieRepository : IRepository<ContactsUnseenEntitie>
    {

      ICommonDataContext Context;
        public ICommonDataContext context
        {
            get { return Context; }
        }

        public ContactsUnseenEntitieRepository()
        {
            this.Context = new CommonDataContext();
        }

        public ContactsUnseenEntitieRepository(int tenant)
        {
            this.Context = CommonDataContext.GetContext(tenant);
        }

        public ContactsUnseenEntitieRepository(ICommonDataContext context)
        {
            this.Context = context;
        }

        public int GetContactsUnseenEntitiesCountByContactIdAndTenant(string contactId, int tenant)
        {
            return context.ContactsUnseenEntities.Where(a => a.ContactId == contactId && a.Tenant == tenant).Count();

        }




        public ContactsUnseenEntitie GetSingleContactsUnseenEntitie(string id, int tenant)
        {
            return (from a in context.ContactsUnseenEntities where a.Id == id && a.Tenant == tenant select a).FirstOrDefault();
        }

        public IQueryable<ContactsUnseenEntitie> GetContactsUnseenEntities(int tenant)
        {
            return context.ContactsUnseenEntities.Where(d => d.Tenant == tenant);
        }

        public List<string> GetAllIds(int tenant)
        {
            List<string> myResult = context.ContactsUnseenEntities.Where(d => d.Tenant == tenant).Select(s => s.Id).ToList();
            return myResult;
        }


        public IQueryable<string> GetContactsUnseenEntitiesByContactAndObjectTable(string ContactId, string objecttableId, int tenant, List<string> trackedIds)
        {

            IQueryable<string> contactsUnseenLists = (from d in context.ContactsUnseenEntities
                                                                  where trackedIds.Contains(d.EntityId) && d.ContactId == ContactId && d.ObjectTableId == objecttableId && d.Tenant == tenant
                                                                  select d.EntityId);
            // 

            return contactsUnseenLists;// context.ContactsUnseenEntities.Where(d => d.ContactId == ContactId && d.ObjectTableId == objecttableId && d.Tenant == tenant).ToList();
        }
      



        public void Add(ContactsUnseenEntitie entity)
        {
            context.ContactsUnseenEntities.Add(entity);
        }

        public void Remove(ContactsUnseenEntitie entity)
        {
            context.ContactsUnseenEntities.Remove(entity);
        }

        public void Update(ContactsUnseenEntitie entity)
        {
            context.ContactsUnseenEntities.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ContactsUnseenEntitie> All()
        {
            return context.ContactsUnseenEntities.ToList();
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<ContactsUnseenEntitie> GetMulti(EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public ContactsUnseenEntitie GetSingle(EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }


        
    }
}
