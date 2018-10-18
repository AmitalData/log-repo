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
    public class CTBUNLOADRepository : IRepository<CTBUNLOAD>
    {
        private AmitalContext currentContext;
        public CTBUNLOADRepository(int tenant)
        {
            currentContext = AmitalContext.GetContext(tenant);
        }

        public CTBUNLOADRepository(AmitalContext context)
        {
            currentContext = context;
        }

        public CTBUNLOAD GetSingle(string ULPORTID)
        {
            return (from a in context.CTBUNLOADs
                    where a.ULPORTID == ULPORTID
                    select a).FirstOrDefault();
        }

        public IQueryable<CTBUNLOAD> GetAll()
        {
            return from a in context.CTBUNLOADs
                   select a;
        }

        public void Add(CTBUNLOAD entity)
        {
            context.CTBUNLOADs.Add(entity);
        }

        public void Remove(CTBUNLOAD entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.CTBUNLOADs.Attach(entity);
            }
            context.CTBUNLOADs.Remove(entity);
        }

        public void Update(CTBUNLOAD entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.CTBUNLOADs.Attach(entity); context.SetAsModified(entity);
            }
        }

        public List<CTBUNLOAD> All()
        {
            return context.CTBUNLOADs.ToList();
        }

        private AmitalContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<CTBUNLOAD> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public CTBUNLOAD GetSingle(EntityKeyFields entityKeys)
        {
            var keys = entityKeys as CTBUNLOADKeys;
            return this.GetSingle(keys.ULPORTID);
        }
    }
}
