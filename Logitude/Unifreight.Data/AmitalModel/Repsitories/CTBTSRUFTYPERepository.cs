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
    public class CTBTSRUFTYPERepository : IRepository<CTBTSRUFTYPE>
    {
        private AmitalContext currentContext;
        public CTBTSRUFTYPERepository(int tenant)
        {
            currentContext = AmitalContext.GetContext(tenant);
        }

        public CTBTSRUFTYPERepository(AmitalContext context)
        {
            currentContext = context;
        }

        public CTBTSRUFTYPE GetSingle(string TSRUFAID)
        {
            return (from a in context.CTBTSRUFTYPEs
                    where a.TSRUFAID == TSRUFAID
                    select a).FirstOrDefault();
        }

        public IQueryable<CTBTSRUFTYPE> GetAll()
        {
            return from a in context.CTBTSRUFTYPEs
                   select a;
        }

        public void Add(CTBTSRUFTYPE entity)
        {
            context.CTBTSRUFTYPEs.Add(entity);
        }

        public void Remove(CTBTSRUFTYPE entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.CTBTSRUFTYPEs.Attach(entity);
            }
            context.CTBTSRUFTYPEs.Remove(entity);
        }

        public void Update(CTBTSRUFTYPE entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.CTBTSRUFTYPEs.Attach(entity); context.SetAsModified(entity);
            }
        }

        public List<CTBTSRUFTYPE> All()
        {
            return context.CTBTSRUFTYPEs.ToList();
        }

        private AmitalContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<CTBTSRUFTYPE> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public CTBTSRUFTYPE GetSingle(EntityKeyFields entityKeys)
        {
            var keys = entityKeys as CTBTSRUFTYPEKeys;
            return this.GetSingle(keys.TSRUFAID);
        }
    }
}
