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
    public class CTBRGOWNRepository : IRepository<CTBRGOWN>
    {
        private AmitalContext currentContext;
        public CTBRGOWNRepository(int tenant)
        {
            currentContext = AmitalContext.GetContext(tenant);
        }

        public CTBRGOWNRepository(AmitalContext context)
        {
            currentContext = context;
        }

        public CTBRGOWN GetSingle(short RIGHTID)
        {
            return (from a in context.CTBRGOWNs
                    where a.RIGHTID == RIGHTID
                    select a).FirstOrDefault();
        }

        public IQueryable<CTBRGOWN> GetAll()
        {
            return from a in context.CTBRGOWNs
                   select a;
        }

        public void Add(CTBRGOWN entity)
        {
            context.CTBRGOWNs.Add(entity);
        }

        public void Remove(CTBRGOWN entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.CTBRGOWNs.Attach(entity);
            }
            //context.AddToCTBRGOWNs 
            context.CTBRGOWNs.Remove(entity);
        }

        public void Update(CTBRGOWN entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.CTBRGOWNs.Attach(entity); context.SetAsModified(entity);
            }
        }

        public List<CTBRGOWN> All()
        {
            return context.CTBRGOWNs.ToList();
        }

        private AmitalContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<CTBRGOWN> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public CTBRGOWN GetSingle(EntityKeyFields entityKeys)
        {
            var keys = entityKeys as CTBRGOWNKeys;
            return this.GetSingle(keys.RIGHTID);
        }
    }
}
