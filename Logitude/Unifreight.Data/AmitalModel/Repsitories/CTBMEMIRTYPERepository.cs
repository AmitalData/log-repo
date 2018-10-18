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
    public class CTBMEMIRTYPERepository : IRepository<CTBMEMIRTYPE>
    {
        private AmitalContext currentContext;
        public CTBMEMIRTYPERepository(int tenant)
        {
            currentContext = AmitalContext.GetContext(tenant);
        }

        public CTBMEMIRTYPERepository(AmitalContext context)
        {
            currentContext = context;
        }

        public CTBMEMIRTYPE GetSingle(int? MEMIRTYPE)
        {
            return (from a in context.CTBMEMIRTYPEs
                    where a.MEMIRTYPE == MEMIRTYPE
                    select a).FirstOrDefault();
        }

        public IQueryable<CTBMEMIRTYPE> GetAll()
        {
            return from a in context.CTBMEMIRTYPEs
                   select a;
        }

        public void Add(CTBMEMIRTYPE entity)
        {
            context.CTBMEMIRTYPEs.Add(entity);
        }

        public void Remove(CTBMEMIRTYPE entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.CTBMEMIRTYPEs.Attach(entity);
            }
            context.CTBMEMIRTYPEs.Remove(entity);
        }

        public void Update(CTBMEMIRTYPE entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.CTBMEMIRTYPEs.Attach(entity); context.SetAsModified(entity);
            }
        }

        public List<CTBMEMIRTYPE> All()
        {
            return context.CTBMEMIRTYPEs.ToList();
        }

        private AmitalContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<CTBMEMIRTYPE> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public CTBMEMIRTYPE GetSingle(EntityKeyFields entityKeys)
        {
            var keys = entityKeys as CTBMEMIRTYPEKeys;
            return this.GetSingle(keys.MEMIRTYPE);
        }
    }
}
