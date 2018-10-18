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
    public class GNDCARDRepository : IRepository<GNDCARD>
    {
        private AmitalContext currentContext;
        public GNDCARDRepository(int tenant)
        {
            currentContext = AmitalContext.GetContext(tenant);
        }

        public GNDCARDRepository(AmitalContext context)
        {
            currentContext = context;
        }

        public GNDCARD GetSingle(string CARDID)
        {
            return (from a in context.GNDCARDs
                    where a.CARDID == CARDID
                    select a).FirstOrDefault();
        }

        public IQueryable<GNDCARD> GetAll()
        {
            return from a in context.GNDCARDs
                   select a;
        }

        public void Add(GNDCARD entity)
        {
            context.GNDCARDs.Add(entity);
        }

        public void Remove(GNDCARD entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.GNDCARDs.Attach(entity);
            }
            //context.AddToGNDCARDs 
            context.GNDCARDs.Remove(entity);
        }

        public void Update(GNDCARD entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.GNDCARDs.Attach(entity); context.SetAsModified(entity);
            }
        }

        public List<GNDCARD> All()
        {
            return context.GNDCARDs.ToList();
        }

        private AmitalContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<GNDCARD> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public GNDCARD GetSingle(EntityKeyFields entityKeys)
        {
            var keys = entityKeys as GNDCARDKeys;
            return this.GetSingle(keys.CARDID);
        }
    }
}
	 