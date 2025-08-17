using System.Collections.Generic;
using System.Linq;
using Simplog.Server.Infrastructure;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class EntityDateRepository:IRepository<EntityDate>
    {
        ICommonDataContext commonDataContext;

        public EntityDateRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }
        public EntityDateRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }
        public IQueryable<EntityDate> GetEntityDates()
        {
            return context.EntityDates;
        }

        public EntityDate GetSingleEntityDate(string id)
        {
            return (from record in context.EntityDates where record.Id == id select record).FirstOrDefault();
        }

        //public EntityDatePM GetSinglePM(string id)
        //{
        //    EntityDate entity = this.GetSingleEntityDate(id);
        //    EntityDatePM PM = null;

        //    if (entity != null)
        //    {
        //        PM = new EntityDatePM()
        //        {

        //        };
        //    }

        //    return PM;
        //}

        public void Add(EntityDate entity)
        {
            context.EntityDates.Add(entity);
        }

        public void Remove(EntityDate entity)
        {
            context.EntityDates.Attach(entity);
            context.EntityDates.Remove(entity);
        }

        public void Update(EntityDate entity)
        {
            context.EntityDates.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<EntityDate> All()
        {
            return context.EntityDates.ToList();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<EntityDate> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public EntityDate GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}