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
    public class CTBTRANSPRepository : IRepository<CTBTRANSP>
    {
        private AmitalContext currentContext;
        public CTBTRANSPRepository(int tenant)
        {
            currentContext = AmitalContext.GetContext(tenant);
        }

        public CTBTRANSPRepository(AmitalContext context)
        {
            currentContext = context;
        }

        public CTBTRANSP GetSingle(string TRANSPTYPE)
        {
            return (from a in context.CTBTRANSPs
                    where a.TRANSPTYPE == TRANSPTYPE
                    select a).FirstOrDefault();
        }

        public IQueryable<CTBTRANSP> GetAll()
        {
            return from a in context.CTBTRANSPs
                   select a;
        }

        public void Add(CTBTRANSP entity)
        {
            context.CTBTRANSPs.Add(entity);
        }

        public void Remove(CTBTRANSP entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.CTBTRANSPs.Attach(entity);
            }
            //context.AddToCTBTRANSPs 
            context.CTBTRANSPs.Remove(entity);
        }

        public void Update(CTBTRANSP entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.CTBTRANSPs.Attach(entity); context.SetAsModified(entity);
            }
            //context.SetAsModified(entity);
        }

        public List<CTBTRANSP> All()
        {
            return context.CTBTRANSPs.ToList();
        }

        private AmitalContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<CTBTRANSP> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public CTBTRANSP GetSingle(EntityKeyFields entityKeys)
        {
            var keys = entityKeys as CTBTRANSPKeys;
            return this.GetSingle(keys.TRANSPTYPE);
        }
    }
}
