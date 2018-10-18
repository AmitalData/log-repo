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
    public class CTBPACKTYPERepository : IRepository<CTBPACKTYPE>
    {
        private AmitalContext currentContext;
        public CTBPACKTYPERepository(int tenant)
        {
            currentContext = AmitalContext.GetContext(tenant);
        }

        public CTBPACKTYPERepository(AmitalContext context)
        {
            currentContext = context;
        }

        public CTBPACKTYPE GetSingle(string PACKTYPEID)
        {
            return (from a in context.CTBPACKTYPEs
                    where a.PACKTYPEID == PACKTYPEID
                    select a).FirstOrDefault();
        }

        public IQueryable<CTBPACKTYPE> GetAll()
        {
            return from a in context.CTBPACKTYPEs
                   select a;
        }

        public void Add(CTBPACKTYPE entity)
        {
            context.CTBPACKTYPEs.Add(entity);
        }

        public void Remove(CTBPACKTYPE entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.CTBPACKTYPEs.Attach(entity);
            }
            context.CTBPACKTYPEs.Remove(entity);
        }

        public void Update(CTBPACKTYPE entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.CTBPACKTYPEs.Attach(entity); context.SetAsModified(entity);
            }
        }

        public List<CTBPACKTYPE> All()
        {
            return context.CTBPACKTYPEs.ToList();
        }

        private AmitalContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<CTBPACKTYPE> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public CTBPACKTYPE GetSingle(EntityKeyFields entityKeys)
        {
            var keys = entityKeys as CTBPACKTYPEKeys;
            return this.GetSingle(keys.PACKTYPEID);
        }
    }
}
