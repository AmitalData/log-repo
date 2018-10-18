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
    public class CTBPARTRepository : IRepository<CTBPART>
    {
        private AmitalContext currentContext;
        public CTBPARTRepository(int tenant)
        {
            currentContext = AmitalContext.GetContext(tenant);
        }

        public CTBPARTRepository(AmitalContext context)
        {
            currentContext = context;
        }

        public CTBPART GetSingle(string PARTIALITYID)
        {
            return (from a in context.CTBPARTs
                    where a.PARTIALITYID == PARTIALITYID
                    select a).FirstOrDefault();
        }

        public IQueryable<CTBPART> GetAll()
        {
            return from a in context.CTBPARTs
                   select a;
        }

        public void Add(CTBPART entity)
        {
            context.CTBPARTs.Add(entity);
        }

        public void Remove(CTBPART entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.CTBPARTs.Attach(entity);
            }
            context.CTBPARTs.Remove(entity);
        }

        public void Update(CTBPART entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.CTBPARTs.Attach(entity); context.SetAsModified(entity);
            }
        }

        public List<CTBPART> All()
        {
            return context.CTBPARTs.ToList();
        }

        private AmitalContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<CTBPART> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public CTBPART GetSingle(EntityKeyFields entityKeys)
        {
            var keys = entityKeys as CTBPARTKeys;
            return this.GetSingle(keys.PARTIALITYID);
        }
    }
}
