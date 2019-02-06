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
    public class CFIMSVREMRepository : IRepository<CFIMSVREM>
    {
        private AmitalContext currentContext;
        public CFIMSVREMRepository(int tenant)
        {
            currentContext = AmitalContext.GetContext(tenant);
        }

        public CFIMSVREMRepository(AmitalContext context)
        {
            currentContext = context;
        }

        public CFIMSVREM GetSingle(long FILENO, string COMID, int PAGENUM, int TOP, int LEFT, int HEIGHT, int WIDTH)
        {
            return (from a in context.CFIMSVREMs
                    where a.FILENO == FILENO && a.COMID == COMID && a.PAGENUM == PAGENUM && a.TOP == TOP && a.LEFT == LEFT && a.HEIGHT == HEIGHT && a.WIDTH == WIDTH
                    select a).FirstOrDefault();
        }

        public IQueryable<CFIMSVREM> GetAll()
        {
            return from a in context.CFIMSVREMs
                   select a;
        }

        public void Add(CFIMSVREM entity)
        {
            context.CFIMSVREMs.Add(entity);
        }

        public void Remove(CFIMSVREM entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.CFIMSVREMs.Attach(entity);
            }
            context.CFIMSVREMs.Remove(entity);
        }

        public void Update(CFIMSVREM entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.CFIMSVREMs.Attach(entity); context.SetAsModified(entity);
            }

        }

        public List<CFIMSVREM> All()
        {
            return context.CFIMSVREMs.ToList();
        }

        private AmitalContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<CFIMSVREM> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public CFIMSVREM GetSingle(EntityKeyFields entityKeys)
        {
            var keys = entityKeys as CFIMSVREMKeys;
            return this.GetSingle(keys.FILENO, keys.COMID, keys.PAGENUM, keys.TOP, keys.LEFT, keys.HEIGHT, keys.WIDTH);
        }

    }
}
