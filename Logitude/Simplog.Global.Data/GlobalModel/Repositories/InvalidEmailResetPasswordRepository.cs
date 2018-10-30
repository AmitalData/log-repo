
using System.Collections.Generic;
using System.Linq;

using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel;
using Simplog.Server.Infrastructure;

namespace Simplog.Global.Data.GlobalModel.Repositories
{
    public class InvalidEmailResetPasswordRepository : IRepository<InvalidEmailResetPassword>
    {
        IGlobalContext globalContext;
        public InvalidEmailResetPasswordRepository()
        {
            globalContext = GlobalContext.GetContext();
        }

        public InvalidEmailResetPasswordRepository(IGlobalContext context)
        {
            globalContext = context;
        }

        public InvalidEmailResetPassword GetSingleInvalidEmailResetPassword(string id)
        {
            InvalidEmailResetPassword item = context.InvalidEmailResetPasswords.Where(d => d.Id == id).FirstOrDefault();
            return item;
        }

        public IQueryable<InvalidEmailResetPassword> GetAllInvalidEmailResetPasswords()
        {
            return from a in context.InvalidEmailResetPasswords
                   select a;
        }


        public void Add(InvalidEmailResetPassword entity)
        {
            context.InvalidEmailResetPasswords.Add(entity);
        }

        public void Remove(InvalidEmailResetPassword entity)
        {
            context.InvalidEmailResetPasswords.Attach(entity);
            context.InvalidEmailResetPasswords.Remove(entity);
        }

        public void Update(InvalidEmailResetPassword entity)
        {
            context.InvalidEmailResetPasswords.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<InvalidEmailResetPassword> All()
        {
            return context.InvalidEmailResetPasswords.ToList();
        }

        public IGlobalContext context
        {
            get { return globalContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<InvalidEmailResetPassword> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public InvalidEmailResetPassword GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

    }
}