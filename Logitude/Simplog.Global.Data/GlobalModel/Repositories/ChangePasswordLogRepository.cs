using System.Collections.Generic;
using System.Linq;

using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel;
using Simplog.Server.Infrastructure;

namespace Simplog.Global.Data.GlobalModel.Repositories
{
    public class ChangePasswordlogRepository : IRepository<ChangePasswordLog>
    {
        IGlobalContext globalContext;
        public ChangePasswordlogRepository()
        {
            globalContext = GlobalContext.GetContext();
        }

        public ChangePasswordlogRepository(IGlobalContext context)
        {
            globalContext = context;
        }



        public ChangePasswordLog GetSingleChangePasswordlog(string email)
        {
            ChangePasswordLog item = context.ChangePasswordLogs.Where(d => d.Email.ToLower() == email.ToLower()).FirstOrDefault();
            return item;
        }

        public IQueryable<ChangePasswordLog> GetAllChangePasswordlog()
        {
            return from a in context.ChangePasswordLogs
                   select a;
        }


        public void Add(ChangePasswordLog entity)
        {
            context.ChangePasswordLogs.Add(entity);
        }

        public void Remove(ChangePasswordLog entity)
        {
            context.ChangePasswordLogs.Attach(entity);
            context.ChangePasswordLogs.Remove(entity);
        }

        public void Update(ChangePasswordLog entity)
        {
            context.ChangePasswordLogs.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ChangePasswordLog> All()
        {
            return context.ChangePasswordLogs.ToList();
        }

        public IGlobalContext context
        {
            get { return globalContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<ChangePasswordLog> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public ChangePasswordLog GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }


    }
}