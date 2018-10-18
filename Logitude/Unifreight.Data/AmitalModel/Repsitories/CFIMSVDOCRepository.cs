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
    public class CFIMSVDOCRepository : IRepository<CFIMSVDOC>
    {
        private AmitalContext currentContext;
        public CFIMSVDOCRepository(int tenant)
        {
            currentContext = AmitalContext.GetContext(tenant);
        }

        public CFIMSVDOCRepository(AmitalContext context)
        {
            currentContext = context;
        }

        public CFIMSVDOC GetSingle(long FILENO, string COMID)
        {
            return (from a in context.CFIMSVDOCs
                    where a.FILENO == FILENO && a.COMID == COMID
                    select a).FirstOrDefault();
        }

        public IQueryable<CFIMSVDOC> GetAll()
        {
            return from a in context.CFIMSVDOCs
                   select a;
        }

        public void Add(CFIMSVDOC entity)
        {
            context.CFIMSVDOCs.Add(entity);
        }

        public void Remove(CFIMSVDOC entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.CFIMSVDOCs.Attach(entity);
            }
            context.CFIMSVDOCs.Remove(entity);
        }

        public void Update(CFIMSVDOC entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.CFIMSVDOCs.Attach(entity); context.SetAsModified(entity);
            }

        }

        public List<CFIMSVDOC> All()
        {
            return context.CFIMSVDOCs.ToList();
        }

        private AmitalContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<CFIMSVDOC> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public CFIMSVDOC GetSingle(EntityKeyFields entityKeys)
        {
            var keys = entityKeys as CFIMSVDOCKeys;
            return this.GetSingle(keys.FILENO, keys.COMID);
        }

    }
}
