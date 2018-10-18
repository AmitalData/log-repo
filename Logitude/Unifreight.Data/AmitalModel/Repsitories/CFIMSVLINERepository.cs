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
    public class CFIMSVLINERepository : IRepository<CFIMSVLINE>
    {
        private AmitalContext currentContext;
        public CFIMSVLINERepository(int tenant)
        {
            currentContext = AmitalContext.GetContext(tenant);
        }

        public CFIMSVLINERepository(AmitalContext context)
        {
            currentContext = context;
        }

        public CFIMSVLINE GetSingle(long FILENO, string COMID, int PAGENUM, int LINENUM)
        {
            return (from a in context.CFIMSVLINEs
                    where a.FILENO == FILENO && a.COMID == COMID && a.PAGENUM == PAGENUM && a.LINENUM == LINENUM
                    select a).FirstOrDefault();
        }

        public IQueryable<CFIMSVLINE> GetAll()
        {
            return from a in context.CFIMSVLINEs
                   select a;
        }

        public void Add(CFIMSVLINE entity)
        {
            context.CFIMSVLINEs.Add(entity);
        }

        public void Remove(CFIMSVLINE entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.CFIMSVLINEs.Attach(entity);
            }
            context.CFIMSVLINEs.Remove(entity);
        }

        public void Update(CFIMSVLINE entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.CFIMSVLINEs.Attach(entity); context.SetAsModified(entity);
            }

        }

        public List<CFIMSVLINE> All()
        {
            return context.CFIMSVLINEs.ToList();
        }

        private AmitalContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<CFIMSVLINE> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public CFIMSVLINE GetSingle(EntityKeyFields entityKeys)
        {
            var keys = entityKeys as CFIMSVLINEKeys;
            return this.GetSingle(keys.FILENO, keys.COMID, keys.PAGENUM, keys.LINENUM);
        }

    }
}
