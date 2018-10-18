using System.Collections.Generic;
using System.Linq;

using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.InfrastructureModel.Repositories
{
    public class ObjectTableHelperControlRepository:IRepository<ObjectTableHelperControl>
    {
        IWebFreightContext webFreightContext;
        public ObjectTableHelperControlRepository(IWebFreightContext context)
        {
            webFreightContext = context;

        }

        public ObjectTableHelperControlRepository()
        {
            webFreightContext = new WebFreightContext();
        }
        public ObjectTableHelperControlRepository(int tenant)
        {
            webFreightContext = WebFreightContext.GetContext(tenant);
        }
        public IQueryable<ObjectTableHelperControl> GetObjectTableHelperControlsByTenant(int tenant)
        {
            var helpercontrols = from a in context.ObjectTableHelperControls
                                 where a.Tenant == tenant
                                 select a;
            return helpercontrols;
        }
        public ObjectTableHelperControl GetSingleObjectTableHelperControl(string id)
        {
            return (from a in context.ObjectTableHelperControls
                    where a.Id == id
                    select a).FirstOrDefault();
        }
       
        public void Add(ObjectTableHelperControl entity)
        {
            context.ObjectTableHelperControls.Add(entity);
        }

        public void Remove(ObjectTableHelperControl entity)
        {
            context.ObjectTableHelperControls.Attach(entity);
            context.ObjectTableHelperControls.Remove(entity);
        }

        public void Update(ObjectTableHelperControl entity)
        {
            try
            {
                context.ObjectTableHelperControls.Attach(entity);
            }
            catch { }
            context.SetAsModified(entity);
        }

        public List<ObjectTableHelperControl> All()
        {
            return context.ObjectTableHelperControls.ToList();
        }

        public IWebFreightContext context
        {
            get {return webFreightContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<ObjectTableHelperControl> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public ObjectTableHelperControl GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}