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
    public class YTBTABLERepository : IRepository<YTBTABLE>
    {
        private AmitalContext currentContext;
        public YTBTABLERepository(int tenant)
        {
            currentContext = AmitalContext.GetContext(tenant);
        }

        public YTBTABLERepository(AmitalContext context)
        {
            currentContext = context;
        }

        public YTBTABLE GetSingle(string CUSTTB, string TBCODE)
        {
            return (from a in context.YTBTABLEs
                    where a.CUSTTB == CUSTTB && a.TBCODE == TBCODE
                    select a).FirstOrDefault();
        }

        public IQueryable<YTBTABLE> GetAll()
        {
            return from a in context.YTBTABLEs
                   select a;
        }

        public void Add(YTBTABLE entity)
        {
            context.YTBTABLEs.Add(entity);
        }

        public void Remove(YTBTABLE entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.YTBTABLEs.Attach(entity);
            }
            context.YTBTABLEs.Remove(entity);
        }

        public void Update(YTBTABLE entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.YTBTABLEs.Attach(entity); context.SetAsModified(entity);
            }

        }

        public List<YTBTABLE> All()
        {
            return context.YTBTABLEs.ToList();
        }

        private AmitalContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<YTBTABLE> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public YTBTABLE GetSingle(EntityKeyFields entityKeys)
        {
            var keys = entityKeys as YTBTABLEKeys;
            return this.GetSingle(keys.TBCODE, keys.CUSTTB);
        }

    }
}
