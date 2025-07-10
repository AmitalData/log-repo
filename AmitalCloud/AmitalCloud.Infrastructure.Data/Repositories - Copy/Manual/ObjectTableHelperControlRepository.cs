using System.Collections.Generic;
using System.Linq;
using AmitalCloud.Infrastructure.Data.Context;
using AmitalCloud.Infrastructure.Data.EntityPOCOs;
using AmitalCloud.Infrastructure.Data.Interfaces;

namespace AmitalCloud.Infrastructure.Data.Repositories
{
    public class ObjectTableHelperControlRepository:IRepository<ObjectTableHelperControl, string>
    {
        IAmitalCloudContext amitalCloudContext;
        public ObjectTableHelperControlRepository(IAmitalCloudContext context)
        {
            amitalCloudContext = context;

        }

        public ObjectTableHelperControlRepository()
        {
            amitalCloudContext = new AmitalCloudContext();
        }
        public ObjectTableHelperControlRepository(int tenant)
        {
            amitalCloudContext = AmitalCloudContext.GetContext(tenant);
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

        public IAmitalCloudContext context
        {
            get {return amitalCloudContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<ObjectTableHelperControl> GetMulti(IEntityKeyFields<ObjectTableHelperControl,string> entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public ObjectTableHelperControl GetSingle(IEntityKeyFields<ObjectTableHelperControl,string> entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}