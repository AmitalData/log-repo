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
    public class GNDADRRepository : IRepository<GNDADR>
    {
        private AmitalContext currentContext;
        public GNDADRRepository(int tenant)
        {
            currentContext = AmitalContext.GetContext(tenant);
        }

        public GNDADRRepository(AmitalContext context)
        {
            currentContext = context;
        }

        public GNDADR GetSingle(string CARDID, int LINE)
        {
            return (from a in context.GNDADRs
                    where a.CARDID == CARDID && a.LINE == LINE
                    select a).FirstOrDefault();
        }

        private AmitalContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<GNDADR> GetMulti(EntityKeyFields entityKeys)
        {
            var keys = entityKeys as GNDCARDKeys;

            return (from a in context.GNDADRs
                    where a.CARDID == keys.CARDID
                    select a).ToList();
        }

        public GNDADR GetSingle(EntityKeyFields entityKeys)
        {
            var keys = entityKeys as GNDADRKeys;
            return this.GetSingle(keys);
        }

        public IQueryable<GNDADR> GetAll()
        {
            return from a in context.GNDADRs
                   select a;
        }

        public void Add(GNDADR entity)
        {
            context.GNDADRs.Add(entity);
        }

        public void Remove(GNDADR entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.GNDADRs.Attach(entity);
            }
            context.GNDADRs.Remove(entity);
        }
        public void Update(GNDADR entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.GNDADRs.Attach(entity); context.SetAsModified(entity);
            }
        }

        public List<GNDADR> All()
        {
            return context.GNDADRs.ToList();
        }

    }
}
	 
