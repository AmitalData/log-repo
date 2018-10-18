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
    public class CTBLOADRepository : IRepository<CTBLOAD>
    {
        private AmitalContext currentContext;
        public CTBLOADRepository(int tenant)
        {
            currentContext = AmitalContext.GetContext(tenant);
        }

        public CTBLOADRepository(AmitalContext context)
        {
            currentContext = context;
        }

        public CTBLOAD GetSingle(string LPORTID)
        {
            return (from a in context.CTBLOADs
                    where a.LPORTID == LPORTID
                    select a).FirstOrDefault();
        }

        public IQueryable<CTBLOAD> GetAll()
        {
            return from a in context.CTBLOADs
                   select a;
        }

        public void Add(CTBLOAD entity)
        {
            context.CTBLOADs.Add(entity);
        }

        public void Remove(CTBLOAD entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.CTBLOADs.Attach(entity);
            }
            context.CTBLOADs.Remove(entity);
        }

        public void Update(CTBLOAD entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.CTBLOADs.Attach(entity); context.SetAsModified(entity);
            }
        }

        public List<CTBLOAD> All()
        {
            return context.CTBLOADs.ToList();
        }

        private AmitalContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<CTBLOAD> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public CTBLOAD GetSingle(EntityKeyFields entityKeys)
        {
            var keys = entityKeys as CTBLOADKeys;
            return this.GetSingle(keys.LPORTID);
        }
    }
}
