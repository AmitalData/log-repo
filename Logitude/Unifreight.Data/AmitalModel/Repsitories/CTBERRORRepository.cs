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
    public class CTBERRORRepository : IRepository<CTBERROR>
    {
        private AmitalContext currentContext;
        public CTBERRORRepository(int tenant)
        {
            currentContext = AmitalContext.GetContext(tenant);
        }

        public CTBERRORRepository(AmitalContext context)
        {
            currentContext = context;
        }

        public CTBERROR GetSingle(string ERRORCODE)
        {
            return (from a in context.CTBERRORs
                    where a.ERRORCODE == ERRORCODE
                    select a).FirstOrDefault();
        }

        public IQueryable<CTBERROR> GetAll()
        {
            return from a in context.CTBERRORs
                   select a;
        }

        public void Add(CTBERROR entity)
        {
            context.CTBERRORs.Add(entity);
        }

        public void Remove(CTBERROR entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.CTBERRORs.Attach(entity);
            }
            context.CTBERRORs.Remove(entity);
        }

        public void Update(CTBERROR entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.CTBERRORs.Attach(entity); context.SetAsModified(entity);
            }
        }

        public List<CTBERROR> All()
        {
            return context.CTBERRORs.ToList();
        }

        private AmitalContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<CTBERROR> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public CTBERROR GetSingle(EntityKeyFields entityKeys)
        {
            var keys = entityKeys as CTBERRORKeys;
            return this.GetSingle(keys.ERRORCODE);
        }
    }
}
