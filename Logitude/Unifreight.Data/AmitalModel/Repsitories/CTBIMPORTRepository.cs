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
    public class CTBIMPORTRepository : IRepository<CTBIMPORT>
    {
        private AmitalContext currentContext;
        public CTBIMPORTRepository(int tenant)
        {
            currentContext = AmitalContext.GetContext(tenant);
        }

        public CTBIMPORTRepository(AmitalContext context)
        {
            currentContext = context;
        }

        public CTBIMPORT GetSingle(string IMPORTERID)
        {
            return (from a in context.CTBIMPORTs
                    where a.IMPORTERID == IMPORTERID
                    select a).FirstOrDefault();
        }

        public IQueryable<CTBIMPORT> GetAll()
        {
            return from a in context.CTBIMPORTs
                   select a;
        }

        public void Add(CTBIMPORT entity)
        {
            context.CTBIMPORTs.Add(entity);
        }

        public void Remove(CTBIMPORT entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.CTBIMPORTs.Attach(entity);
            }
            //context.AddToCTBIMPORTs 
            context.CTBIMPORTs.Remove(entity);
        }

        public void Update(CTBIMPORT entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.CTBIMPORTs.Attach(entity); context.SetAsModified(entity);
            }
        }

        public List<CTBIMPORT> All()
        {
            return context.CTBIMPORTs.ToList();
        }

        private AmitalContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<CTBIMPORT> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public CTBIMPORT GetSingle(EntityKeyFields entityKeys)
        {
            var keys = entityKeys as CTBIMPORTKeys;
            return this.GetSingle(keys.IMPORTERID);
        }
    }
}
