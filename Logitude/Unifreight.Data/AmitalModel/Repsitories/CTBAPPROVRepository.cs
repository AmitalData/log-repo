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
    public class CTBAPPROVRepository : IRepository<CTBAPPROV>
    {
        private AmitalContext currentContext;
        public CTBAPPROVRepository(int tenant)
        {
            currentContext = AmitalContext.GetContext(tenant);
        }

        public CTBAPPROVRepository(AmitalContext context)
        {
            currentContext = context;
        }

        public CTBAPPROV GetSingle(string APPROVCODEID)
        {
            return (from a in context.CTBAPPROVs
                    where a.APPROVCODEID == APPROVCODEID
                    select a).FirstOrDefault();
        }

        public IQueryable<CTBAPPROV> GetAll()
        {
            return from a in context.CTBAPPROVs
                   select a;
        }

        public void Add(CTBAPPROV entity)
        {
            context.CTBAPPROVs.Add(entity);
        }

        public void Remove(CTBAPPROV entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.CTBAPPROVs.Attach(entity);
            }
            context.CTBAPPROVs.Remove(entity);
        }

        public void Update(CTBAPPROV entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.CTBAPPROVs.Attach(entity); context.SetAsModified(entity);
            }
        }

        public List<CTBAPPROV> All()
        {
            return context.CTBAPPROVs.ToList();
        }

        private AmitalContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<CTBAPPROV> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public CTBAPPROV GetSingle(EntityKeyFields entityKeys)
        {
            var keys = entityKeys as CTBAPPROVKeys;
            return this.GetSingle(keys.APPROVCODEID);
        }
    }
}
