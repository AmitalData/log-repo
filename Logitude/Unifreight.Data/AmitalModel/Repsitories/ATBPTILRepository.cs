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
    public class ATBPTILRepository : IRepository<ATBPTIL>
    {
        private AmitalContext currentContext;
        public ATBPTILRepository(int tenant)
        {
            currentContext = AmitalContext.GetContext(tenant);
        }

        public ATBPTILRepository(AmitalContext context)
        {
            currentContext = context;
        }

        public ATBPTIL GetSingle(string BRANID)
        {
            return (from a in context.ATBPTILs
                    where a.BRANID == BRANID
                    select a).FirstOrDefault();
        }

        public IQueryable<ATBPTIL> GetAll()
        {
            return from a in context.ATBPTILs
                   select a;
        }

        public void Add(ATBPTIL entity)
        {
            context.ATBPTILs.Add(entity);
        }

        public void Remove(ATBPTIL entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.ATBPTILs.Attach(entity);
            }
            context.ATBPTILs.Remove(entity);
        }

        public void Update(ATBPTIL entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.ATBPTILs.Attach(entity); context.SetAsModified(entity);
            }
            
        }

        public List<ATBPTIL> All()
        {
            return context.ATBPTILs.ToList();
        }

        private AmitalContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<ATBPTIL> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public ATBPTIL GetSingle(EntityKeyFields entityKeys)
        {
            var keys = entityKeys as ATBPTILKeys;
            return this.GetSingle(keys.BRANID);
        }
    }
}
