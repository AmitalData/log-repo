using System.Collections.Generic;
using System.Linq;

using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel;
using Simplog.Server.Infrastructure;

namespace Simplog.Global.Data.GlobalModel.Repositories
{
    public class OneTimePasswordRepository : IRepository<OneTimePassword>
    {
        IGlobalContext globalContext;
        public OneTimePasswordRepository()
        {
            globalContext = GlobalContext.GetContext();
        }

        public OneTimePasswordRepository(IGlobalContext context)
        {
            globalContext = context;
        }



        public OneTimePassword GetSingleOneTimePassword(string id)
        {
            OneTimePassword item = context.OneTimePasswords.Where(d => d.Id== id).FirstOrDefault();
            return item;
        }

        public IQueryable<OneTimePassword> GetAllOneTimePasswords()
        {
            return from a in context.OneTimePasswords
                   select a;
        }


        public void Add(OneTimePassword entity)
        {
            context.OneTimePasswords.Add(entity);
        }

        public void Remove(OneTimePassword entity)
        {
            context.OneTimePasswords.Attach(entity);
            context.OneTimePasswords.Remove(entity);
        }

        public void Update(OneTimePassword entity)
        {
            context.OneTimePasswords.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<OneTimePassword> All()
        {
            return context.OneTimePasswords.ToList();
        }

        public IGlobalContext context
        {
            get { return globalContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<OneTimePassword> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public OneTimePassword GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

    }
}