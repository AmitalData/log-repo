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
    public class CTBAPPROVTYPERepository : IRepository<CTBAPPROVTYPE>
    {
        private AmitalContext currentContext;
        public CTBAPPROVTYPERepository(int tenant)
        {
            currentContext = AmitalContext.GetContext(tenant);
        }

        public CTBAPPROVTYPERepository(AmitalContext context)
        {
            currentContext = context;
        }

        public CTBAPPROVTYPE GetSingle(string APPROVTYPEID)
        {
            return (from a in context.CTBAPPROVTYPEs
                    where a.APPROVTYPEID == APPROVTYPEID
                    select a).FirstOrDefault();
        }

        public IQueryable<CTBAPPROVTYPE> GetAll()
        {
            return from a in context.CTBAPPROVTYPEs
                   select a;
        }

        public void Add(CTBAPPROVTYPE entity)
        {
            context.CTBAPPROVTYPEs.Add(entity);
        }

        public void Remove(CTBAPPROVTYPE entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.CTBAPPROVTYPEs.Attach(entity);
            }
            context.CTBAPPROVTYPEs.Remove(entity);
        }

        public void Update(CTBAPPROVTYPE entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.CTBAPPROVTYPEs.Attach(entity); context.SetAsModified(entity);
            }
        }

        public List<CTBAPPROVTYPE> All()
        {
            return context.CTBAPPROVTYPEs.ToList();
        }

        private AmitalContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<CTBAPPROVTYPE> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public CTBAPPROVTYPE GetSingle(EntityKeyFields entityKeys)
        {
            var keys = entityKeys as CTBAPPROVTYPEKeys;
            return this.GetSingle(keys.APPROVTYPEID);
        }
    }
}
