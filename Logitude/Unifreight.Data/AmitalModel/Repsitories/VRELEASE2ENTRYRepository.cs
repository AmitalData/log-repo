using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unifreight.Data.AmitalModel.EntityKeys;
using Unifreight.Data.AmitalModel.EntityPOCOs;

namespace Unifreight.Data.AmitalModel.Repsitories
{
    public class VRELEASE2ENTRYRepository : IRepository<VRELEASE2ENTRY>
    {
        private AmitalContext currentContext;
        public VRELEASE2ENTRYRepository(int tenant)
        {
            currentContext = AmitalContext.GetContext(tenant);
        }

        public VRELEASE2ENTRYRepository(AmitalContext context)
        {
            currentContext = context;
        }

        public VRELEASE2ENTRY GetSingle(int CUFILENO)
        {
            return (from a in context.VRELEASE2ENTRIES
                    where a.CUFILENO == CUFILENO
                    select a).FirstOrDefault();
        }

        public VRELEASE2ENTRY GetSingleByCustomFile(string CUSTOMFILENO)
        {
            return (from a in context.VRELEASE2ENTRIES
                    where a.CUSTOMFILENO == CUSTOMFILENO
                    select a).FirstOrDefault();
        }

        public IQueryable<VRELEASE2ENTRY> GetAll()
        {
            return from a in context.VRELEASE2ENTRIES
                   select a;
        }

        public void Add(VRELEASE2ENTRY entity)
        {
            //context.VRELEASE2ENTRIES.Add(entity);
        }

        public void Remove(VRELEASE2ENTRY entity)
        {
            ////if (entity.EntityState == System.Data.EntityState.Unchanged)
            //{
            //    context.VRELEASE2ENTRIES.Attach(entity);
            //}
            //context.VRELEASE2ENTRIES.Remove(entity);
        }

        public void Update(VRELEASE2ENTRY entity)
        {
            ////if (entity.EntityState == System.Data.EntityState.Unchanged)
            //{
            //    context.VRELEASE2ENTRIES.Attach(entity); context.SetAsModified(entity);
            //}

        }

        public List<VRELEASE2ENTRY> All()
        {
            return context.VRELEASE2ENTRIES.ToList();
        }

        private AmitalContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<VRELEASE2ENTRY> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public VRELEASE2ENTRY GetSingle(EntityKeyFields entityKeys)
        {
            return null;
            //var keys = entityKeys as VRELEASE2ENTRYKeys;
            //return this.GetSingle(keys.BRANID);
        }
    }
}
