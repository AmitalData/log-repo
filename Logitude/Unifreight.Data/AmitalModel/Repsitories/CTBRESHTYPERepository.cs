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
    public class CTBRESHTYPERepository : IRepository<CTBRESHTYPE>
    {
        private AmitalContext currentContext;
        public CTBRESHTYPERepository(int tenant)
        {
            currentContext = AmitalContext.GetContext(tenant);
        }

        public CTBRESHTYPERepository(AmitalContext context)
        {
            currentContext = context;
        }

        public CTBRESHTYPE GetSingle(string RESHIMONTYPE)
        {
            return (from a in context.CTBRESHTYPEs
                    where a.RESHIMONTYPE == RESHIMONTYPE
                    select a).FirstOrDefault();
        }

        public IQueryable<CTBRESHTYPE> GetAll()
        {
            return from a in context.CTBRESHTYPEs
                   select a;
        }

        public void Add(CTBRESHTYPE entity)
        {
            context.CTBRESHTYPEs.Add(entity);
        }

        public void Remove(CTBRESHTYPE entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.CTBRESHTYPEs.Attach(entity);
            }
            //context.AddToCTBRESHTYPEs 
            context.CTBRESHTYPEs.Remove(entity);
        }

        public void Update(CTBRESHTYPE entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.CTBRESHTYPEs.Attach(entity); context.SetAsModified(entity);
            }
        }

        public List<CTBRESHTYPE> All()
        {
            return context.CTBRESHTYPEs.ToList();
        }

        private AmitalContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<CTBRESHTYPE> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public CTBRESHTYPE GetSingle(EntityKeyFields entityKeys)
        {
            var keys = entityKeys as CTBRESHTYPEKeys;
            return this.GetSingle(keys.RESHIMONTYPE);
        }
    }
}
	 