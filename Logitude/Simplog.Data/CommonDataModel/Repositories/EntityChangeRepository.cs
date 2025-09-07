using System.Collections.Generic;
using System.Linq;

using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class EntityChangeRepository : IRepository<EntityChange>
    {
        ICommonDataContext commonDataContext;



        public EntityChangeRepository(ICommonDataContext context)
        {
            commonDataContext = context;

        }

        public EntityChangeRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public EntityChange GetSingleEntityChange(string id, int tenant)
        {
            return (from a in this.context.EntityChanges
                    where a.Id == id
                    select a).FirstOrDefault();
        }


        public List<EntityChange> GetEntityChanges( int tenant)
        {
            return (from a in this.context.EntityChanges
                    where a.Tenant == tenant &&( a.ChangesAutomationFieldsXml!=null || a.AutomationConditionFieldsXml !=null)
                    select a).ToList();
        }



        public void Add(EntityChange entity)
        {
            this.context.EntityChanges.Add(entity);
        }

        public void Remove(EntityChange entity)
        {

            this.context.EntityChanges.Attach(entity);

            this.context.EntityChanges.Remove(entity);
        }

        public void Update(EntityChange entity)
        {
            this.context.EntityChanges.Attach(entity);

            this.context.SetAsModified(entity);
        }

        public List<EntityChange> All()
        {
            return this.context.EntityChanges.ToList();
        }

        public ICommonDataContext context
        {
            get { return this.commonDataContext; }
        }

        public void SubmitChanges()
        {
            this.context.SaveChanges();
        }


        public List<EntityChange> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public EntityChange GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}