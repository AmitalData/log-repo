using System.Collections.Generic;
using System.Linq;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel;
using Simplog.Server.Infrastructure;

namespace Simplog.Global.Data.GlobalModel.Repositories
{
    public class AutoSignupEmailRepository:IRepository<AutoSignupEmail>
    {
        IGlobalContext globalContext;
        public AutoSignupEmailRepository()
        {
            globalContext = GlobalContext.GetContext();
        }

        public AutoSignupEmailRepository(IGlobalContext context)
        {
            globalContext = context;
        }


        public AutoSignupEmailRepository(int tenant)
        {
            globalContext = GlobalContext.GetContext();
        }

        public IQueryable<AutoSignupEmail> GetAutoSignupEmailsByStatus(string status)
        {
            return (from a in context.AutoSignupEmails
                    where a.Status.ToLower() == status.ToLower()
                    select a);
        }
         
        public AutoSignupEmail GetSingleAutoSignupEmail(string id)
        {
            return (from a in context.AutoSignupEmails
                    where a.Id == id
                    select a).FirstOrDefault();
        }
         
        public IQueryable<AutoSignupEmail> GetAllAutoSignupEmails()
        {
            return from a in context.AutoSignupEmails
                   select a;
        }

        public void Add(AutoSignupEmail entity)
        {
            context.AutoSignupEmails.Add(entity);
        }

        public void Remove(AutoSignupEmail entity)
        {
            context.AutoSignupEmails.Attach(entity);
            context.AutoSignupEmails.Remove(entity);
        }

        public void Update(AutoSignupEmail entity)
        {
            context.AutoSignupEmails.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<AutoSignupEmail> All()
        {
            return context.AutoSignupEmails.ToList();
        }

        public IGlobalContext context
        {
            get { return globalContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<AutoSignupEmail> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public AutoSignupEmail GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public IQueryable<AutoSignupEmail> GetAutoSignupEmailsByTenant(int tenant)
        {
            IQueryable<AutoSignupEmail> eventTypes = from a in context.AutoSignupEmails  select a;
            return eventTypes;
        }

     
    }
}