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
    public class CTBMISHGURRepository : IRepository<CTBMISHGUR>
    {
        private AmitalContext currentContext;
        public CTBMISHGURRepository(int tenant)
        {
            currentContext = AmitalContext.GetContext(tenant);
        }

        public CTBMISHGURRepository(AmitalContext context)
        {
            currentContext = context;
        }

        public CTBMISHGUR GetSingle(string SUGMISHGUR)
        {
            return (from a in context.CTBMISHGURs
                    where a.SUGMISHGUR == SUGMISHGUR
                    select a).FirstOrDefault();
        }

        public IQueryable<CTBMISHGUR> GetAll()
        {
            return from a in context.CTBMISHGURs
                   select a;
        }

        public void Add(CTBMISHGUR entity)
        {
            context.CTBMISHGURs.Add(entity);
        }

        public void Remove(CTBMISHGUR entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.CTBMISHGURs.Attach(entity);
            }
            context.CTBMISHGURs.Remove(entity);
        }

        public void Update(CTBMISHGUR entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.CTBMISHGURs.Attach(entity); context.SetAsModified(entity);
            }
        }

        public List<CTBMISHGUR> All()
        {
            return context.CTBMISHGURs.ToList();
        }

        private AmitalContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<CTBMISHGUR> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public CTBMISHGUR GetSingle(EntityKeyFields entityKeys)
        {
            var keys = entityKeys as CTBMISHGURKeys;
            return this.GetSingle(keys.SUGMISHGUR);
        }
    }
}
