
using System.Collections.Generic;
using System.Linq;

using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel;
using Simplog.Server.Infrastructure;

namespace Simplog.Global.Data.GlobalModel.Repositories
{
    public class CaptchaKeyRepository : IRepository<CaptchaKey>
    {
        IGlobalContext globalContext;
        public CaptchaKeyRepository()
        {
            globalContext = GlobalContext.GetContext();
        }

        public CaptchaKeyRepository(IGlobalContext context)
        {
            globalContext = context;
        }

        public CaptchaKey GetSingleCaptchaKey(string id)
        {
            CaptchaKey item = context.CaptchaKeys.Where(d => d.Id == id && !d.IsUsed).FirstOrDefault();
            return item;
        }

        public IQueryable<CaptchaKey> GetAllCaptchaKeys()
        {
            return from a in context.CaptchaKeys
                   select a;
        }


        public void Add(CaptchaKey entity)
        {
            context.CaptchaKeys.Add(entity);
        }

        public void Remove(CaptchaKey entity)
        {
            context.CaptchaKeys.Attach(entity);
            context.CaptchaKeys.Remove(entity);
        }

        public void Update(CaptchaKey entity)
        {
            context.CaptchaKeys.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CaptchaKey> All()
        {
            return context.CaptchaKeys.ToList();
        }

        public IGlobalContext context
        {
            get { return globalContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<CaptchaKey> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public CaptchaKey GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

    }
}