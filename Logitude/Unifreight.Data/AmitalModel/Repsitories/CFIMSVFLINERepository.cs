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
    public class CFIMSVFLINERepository : IRepository<CFIMSVFLINE>
    {
        private AmitalContext currentContext;
        public CFIMSVFLINERepository(int tenant)
        {
            currentContext = AmitalContext.GetContext(tenant);
        }

        public CFIMSVFLINERepository(AmitalContext context)
        {
            currentContext = context;
        }

        public CFIMSVFLINE GetSingle(long FILENO, string COMID, int LINENUM)
        {
            return (from a in context.CFIMSVFLINEs
                    where a.FILENO == FILENO && a.COMID == COMID && a.LINENUM == LINENUM
                    select a).FirstOrDefault();
        }

        public IQueryable<CFIMSVFLINE> GetAll()
        {
            return from a in context.CFIMSVFLINEs
                   select a;
        }

        public void Add(CFIMSVFLINE entity)
        {
            context.CFIMSVFLINEs.Add(entity);
        }

        public void Remove(CFIMSVFLINE entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.CFIMSVFLINEs.Attach(entity);
            }
            context.CFIMSVFLINEs.Remove(entity);
        }

        public void Update(CFIMSVFLINE entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.CFIMSVFLINEs.Attach(entity); context.SetAsModified(entity);
            }

        }

        public List<CFIMSVFLINE> All()
        {
            return context.CFIMSVFLINEs.ToList();
        }

        private AmitalContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<CFIMSVFLINE> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public CFIMSVFLINE GetSingle(EntityKeyFields entityKeys)
        {
            var keys = entityKeys as CFIMSVFLINEKeys;
            return this.GetSingle(keys.FILENO, keys.COMID, keys.LINENUM);
        }

    }
}
