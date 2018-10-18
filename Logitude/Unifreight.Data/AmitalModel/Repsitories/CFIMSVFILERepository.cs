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
    public class CFIMSVFILERepository : IRepository<CFIMSVFILE>
    {
        private AmitalContext currentContext;
        public CFIMSVFILERepository(int tenant)
        {
            currentContext = AmitalContext.GetContext(tenant);
        }

        public CFIMSVFILERepository(AmitalContext context)
        {
            currentContext = context;
        }

        public CFIMSVFILE GetSingle(long FILENO)
        {
            return (from a in context.CFIMSVFILEs
                    where a.FILENO == FILENO
                    select a).FirstOrDefault();
        }

        public IQueryable<CFIMSVFILE> GetAll()
        {
            return from a in context.CFIMSVFILEs
                   select a;
        }

        public void Add(CFIMSVFILE entity)
        {
            context.CFIMSVFILEs.Add(entity);
        }

        public void Remove(CFIMSVFILE entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.CFIMSVFILEs.Attach(entity);
            }
            context.CFIMSVFILEs.Remove(entity);
        }

        public void Update(CFIMSVFILE entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.CFIMSVFILEs.Attach(entity); context.SetAsModified(entity);
            }

        }

        public List<CFIMSVFILE> All()
        {
            return context.CFIMSVFILEs.ToList();
        }

        private AmitalContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<CFIMSVFILE> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public CFIMSVFILE GetSingle(EntityKeyFields entityKeys)
        {
            var keys = entityKeys as CFIMSVFILEKeys;
            return this.GetSingle(keys.FILENO);
        }

    }
}
