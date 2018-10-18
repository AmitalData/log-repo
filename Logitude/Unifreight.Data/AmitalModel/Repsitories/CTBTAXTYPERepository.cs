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
    public class CTBTAXTYPERepository : IRepository<CTBTAXTYPE>
    {
        private AmitalContext currentContext;
        public CTBTAXTYPERepository(int tenant)
        {
            currentContext = AmitalContext.GetContext(tenant);
        }

        public CTBTAXTYPERepository(AmitalContext context)
        {
            currentContext = context;
        }

        public CTBTAXTYPE GetSingle(string TAXTYPE)
        {
            return (from a in context.CTBTAXTYPEs
                    where a.TAXTYPE == TAXTYPE
                    select a).FirstOrDefault();
        }

        public IQueryable<CTBTAXTYPE> GetAll()
        {
            return from a in context.CTBTAXTYPEs
                   select a;
        }

        public void Add(CTBTAXTYPE entity)
        {
            context.CTBTAXTYPEs.Add(entity);
        }

        public void Remove(CTBTAXTYPE entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.CTBTAXTYPEs.Attach(entity);
            }
            context.CTBTAXTYPEs.Remove(entity);
        }

        public void Update(CTBTAXTYPE entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.CTBTAXTYPEs.Attach(entity); context.SetAsModified(entity);
            }
        }

        public List<CTBTAXTYPE> All()
        {
            return context.CTBTAXTYPEs.ToList();
        }

        private AmitalContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<CTBTAXTYPE> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public CTBTAXTYPE GetSingle(EntityKeyFields entityKeys)
        {
            var keys = entityKeys as CTBTAXTYPEKeys;
            return this.GetSingle(keys.TAXTYPE);
        }
    }
}
