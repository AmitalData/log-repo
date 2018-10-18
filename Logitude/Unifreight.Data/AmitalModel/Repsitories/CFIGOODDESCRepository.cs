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
    public class CFIGOODDESCRepository : IRepository<CFIGOODDESC>
    {
        private AmitalContext currentContext;
        public CFIGOODDESCRepository(int tenant)
        {
            currentContext = AmitalContext.GetContext(tenant);
        }

        public CFIGOODDESCRepository(AmitalContext context)
        {
            currentContext = context;
        }

        public CFIGOODDESC GetSingle(long FILENO)
        {
            return (from a in context.CFIGOODDESCs
                    where a.FILENO == FILENO
                    select a).FirstOrDefault();
        }

        public IQueryable<CFIGOODDESC> GetAll()
        {
            return from a in context.CFIGOODDESCs
                   select a;
        }

        public void Add(CFIGOODDESC entity)
        {
            context.CFIGOODDESCs.Add(entity);
        }

        public void Remove(CFIGOODDESC entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.CFIGOODDESCs.Attach(entity);
            }
            context.CFIGOODDESCs.Remove(entity);
        }

        public void Update(CFIGOODDESC entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.CFIGOODDESCs.Attach(entity); context.SetAsModified(entity);
            }

        }

        public List<CFIGOODDESC> All()
        {
            return context.CFIGOODDESCs.ToList();
        }

        private AmitalContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<CFIGOODDESC> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public CFIGOODDESC GetSingle(EntityKeyFields entityKeys)
        {
            var keys = entityKeys as CFIGOODDESCKeys;
            return this.GetSingle(keys.FILENO);
        }
    }
}
