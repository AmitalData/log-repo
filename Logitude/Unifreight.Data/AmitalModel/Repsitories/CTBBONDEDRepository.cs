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
    public class CTBBONDEDRepository : IRepository<CTBBONDED>
    {
        private AmitalContext currentContext;
        public CTBBONDEDRepository(int tenant)
        {
            currentContext = AmitalContext.GetContext(tenant);
        }

        public CTBBONDEDRepository(AmitalContext context)
        {
            currentContext = context;
        }

        public CTBBONDED GetSingle(string WAREHOUSEID)
        {
            return (from a in context.CTBBONDEDs
                    where a.WAREHOUSEID == WAREHOUSEID
                    select a).FirstOrDefault();
        }

        public IQueryable<CTBBONDED> GetAll()
        {
            return from a in context.CTBBONDEDs
                   select a;
        }

        public void Add(CTBBONDED entity)
        {
            context.CTBBONDEDs.Add(entity);
        }

        public void Remove(CTBBONDED entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.CTBBONDEDs.Attach(entity);
            }
            context.CTBBONDEDs.Remove(entity);
        }

        public void Update(CTBBONDED entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.CTBBONDEDs.Attach(entity); context.SetAsModified(entity);
            }
        }

        public List<CTBBONDED> All()
        {
            return context.CTBBONDEDs.ToList();
        }

        private AmitalContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<CTBBONDED> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public CTBBONDED GetSingle(EntityKeyFields entityKeys)
        {
            var keys = entityKeys as CTBBONDEDKeys;
            return this.GetSingle(keys.WAREHOUSEID);
        }
    }
}
