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
    public class CFIMSVPAGERepository : IRepository<CFIMSVPAGE>
    {
        private AmitalContext currentContext;
        public CFIMSVPAGERepository(int tenant)
        {
            currentContext = AmitalContext.GetContext(tenant);
        }

        public CFIMSVPAGERepository(AmitalContext context)
        {
            currentContext = context;
        }

        public CFIMSVPAGE GetSingle(long FILENO, string COMID, int PAGENUM, string QUETYPE)
        {
            return (from a in context.CFIMSVPAGEs
                    where a.FILENO == FILENO && a.COMID == COMID && a.PAGENUM == PAGENUM && a.QUETYPE == QUETYPE
                    select a).FirstOrDefault();
        }

        public IQueryable<CFIMSVPAGE> GetAll()
        {
            return from a in context.CFIMSVPAGEs
                   select a;
        }

        public void Add(CFIMSVPAGE entity)
        {
            context.CFIMSVPAGEs.Add(entity);
        }

        public void Remove(CFIMSVPAGE entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.CFIMSVPAGEs.Attach(entity);
            }
            context.CFIMSVPAGEs.Remove(entity);
        }

        public void Update(CFIMSVPAGE entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.CFIMSVPAGEs.Attach(entity); context.SetAsModified(entity);
            }

        }

        public List<CFIMSVPAGE> All()
        {
            return context.CFIMSVPAGEs.ToList();
        }

        private AmitalContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<CFIMSVPAGE> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public CFIMSVPAGE GetSingle(EntityKeyFields entityKeys)
        {
            var keys = entityKeys as CFIMSVPAGEKeys;
            return this.GetSingle(keys.FILENO, keys.COMID, keys.PAGENUM, keys.QUETYPE);
        }

    }
}
