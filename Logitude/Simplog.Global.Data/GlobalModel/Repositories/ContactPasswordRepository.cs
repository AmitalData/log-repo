using System.Collections.Generic;
using System.Linq;

using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel;
using Simplog.Server.Infrastructure;

namespace Simplog.Global.Data.GlobalModel.Repositories
{
    public class ContactPasswordRepository : IRepository<ContactPassword>
    {
        IGlobalContext globalContext;
        public ContactPasswordRepository()
        {
            globalContext = GlobalContext.GetContext();
        }

        public ContactPasswordRepository(IGlobalContext context)
        {
            globalContext = context;
        }



        public ContactPassword GetSingleContactPassword(string email)
        {
            ContactPassword item = context.ContactPasswords.Where(d => d.Email.ToLower() == email.ToLower()).FirstOrDefault();
            return item;
        }

        public IQueryable<ContactPassword> GetAllContactPasswords()
        {
            return from a in context.ContactPasswords
                   select a;
        }


        public void Add(ContactPassword entity)
        {
            context.ContactPasswords.Add(entity);
        }

        public void Remove(ContactPassword entity)
        {
            context.ContactPasswords.Attach(entity);
            context.ContactPasswords.Remove(entity);
        }

        public void Update(ContactPassword entity)
        {
            context.ContactPasswords.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ContactPassword> All()
        {
            return context.ContactPasswords.ToList();
        }

        public IGlobalContext context
        {
            get { return globalContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<ContactPassword> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public ContactPassword GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }



        public string GetOldPasswordByEmail(string email)
        {
          return  context.ContactPasswords.Where(d => d.Email.ToLower() == email.ToLower()).Select(d=>d.Password).FirstOrDefault();
        }
    }
}