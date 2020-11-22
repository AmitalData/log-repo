
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unifreight.Data.AmitalModel.EntityKeys;
using Unifreight.Data.AmitalModel.EntityPOCOs;

namespace Unifreight.Data.AmitalModel.Repsitories
{
    public class GDMLOCKRepository : IRepository<GDMLOCK>
    {
        private AmitalContext currentContext;
        public GDMLOCKRepository(int tenant)
        {
            currentContext = AmitalContext.GetContext(tenant);
        }

        public GDMLOCKRepository(AmitalContext context)
        {
            currentContext = context;
        }

        public GDMLOCK GetSingle(string COMID)
        {
            return (from a in context.GDMLOCKs
                    where a.COMID == COMID
                    select a).FirstOrDefault();
        }

        public IQueryable<GDMLOCK> GetAll()
        {
            return from a in context.GDMLOCKs
                   select a;
        }

        public void Add(GDMLOCK entity)
        {
            context.GDMLOCKs.Add(entity);
        }

        public void Remove(GDMLOCK entity)
        {

            AttachIfNot(entity); context.SetAsModified(entity); //context.GDMLOCKs.Attach(entity);
            //context.AddToGDMLOCKs 
            context.GDMLOCKs.Remove(entity);
        }

        public void Update(GDMLOCK entity)
        {
            AttachIfNot(entity); context.SetAsModified(entity);

            //context.SetAsModified(entity);
        }
        void AttachIfNot(GDMLOCK entity)
        {
            // if ((entity as EntityObject).EntityState == System.Data.EntityState.Unchanged)
            {
                context.GDMLOCKs.Attach(entity);
            }
        }

        public List<GDMLOCK> All()
        {
            return context.GDMLOCKs.ToList();
        }

        private AmitalContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<GDMLOCK> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public GDMLOCK GetSingle(EntityKeyFields entityKeys)
        {
            var keys = entityKeys as GDMLOCKKeys;
            return this.GetSingle(keys.COMID);
        }
    }
}
