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
    public class ETBVENDRepository : IRepository<ETBVEND>
    {
        private AmitalContext currentContext;
        public ETBVENDRepository(int tenant)
        {
            currentContext = AmitalContext.GetContext(tenant);
        }

        public ETBVENDRepository(AmitalContext context)
        {
            currentContext = context;
        }

        public ETBVEND GetSingle(string VENDORID)
        {
            return (from a in context.ETBVENDs
                    where a.VENDORID == VENDORID
                    select a).FirstOrDefault();
        }

        public IQueryable<ETBVEND> GetAll()
        {
            return from a in context.ETBVENDs
                   select a;
        }

        public void Add(ETBVEND entity)
        {
            context.ETBVENDs.Add(entity);
        }

        public void Remove(ETBVEND entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.ETBVENDs.Attach(entity);
            }
            context.ETBVENDs.Remove(entity);
        }

        public void Update(ETBVEND entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.ETBVENDs.Attach(entity); context.SetAsModified(entity);
            }
        }

        public List<ETBVEND> All()
        {
            return context.ETBVENDs.ToList();
        }

        private AmitalContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<ETBVEND> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public ETBVEND GetSingle(EntityKeyFields entityKeys)
        {
            var keys = entityKeys as ETBVENDKeys;
            return this.GetSingle(keys.VENDORID);
        }
    }
}
