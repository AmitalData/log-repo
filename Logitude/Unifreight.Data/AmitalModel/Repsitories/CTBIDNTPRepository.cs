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
    public class CTBIDNTPRepository : IRepository<CTBIDNTP>
    {
        private AmitalContext currentContext;
        public CTBIDNTPRepository(int tenant)
        {
            currentContext = AmitalContext.GetContext(tenant);
        }

        public CTBIDNTPRepository(AmitalContext context)
        {
            currentContext = context;
        }

        public CTBIDNTP GetSingle(string IDENTIFITYPE)
        {
            return (from a in context.CTBIDNTPs
                    where a.IDENTIFITYPE == IDENTIFITYPE
                    select a).FirstOrDefault();
        }

        public IQueryable<CTBIDNTP> GetAll()
        {
            return from a in context.CTBIDNTPs
                   select a;
        }

        public void Add(CTBIDNTP entity)
        {
            context.CTBIDNTPs.Add(entity);
        }

        public void Remove(CTBIDNTP entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.CTBIDNTPs.Attach(entity);
            }
            context.CTBIDNTPs.Remove(entity);
        }

        public void Update(CTBIDNTP entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.CTBIDNTPs.Attach(entity); context.SetAsModified(entity);
            }
        }

        public List<CTBIDNTP> All()
        {
            return context.CTBIDNTPs.ToList();
        }

        private AmitalContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<CTBIDNTP> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public CTBIDNTP GetSingle(EntityKeyFields entityKeys)
        {
            var keys = entityKeys as CTBIDNTPKeys;
            return this.GetSingle(keys.IDENTIFITYPE);
        }
    }
}
