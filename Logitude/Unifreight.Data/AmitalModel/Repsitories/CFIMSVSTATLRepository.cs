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
    public class CFIMSVSTATLRepository : IRepository<CFIMSVSTATL>
    {
        private AmitalContext currentContext;
        public CFIMSVSTATLRepository(int tenant)
        {
            currentContext = AmitalContext.GetContext(tenant);
        }

        public CFIMSVSTATLRepository(AmitalContext context)
        {
            currentContext = context;
        }

        public CFIMSVSTATL GetSingle(string GUID)
        {
            return (from a in context.CFIMSVSTATLs
                    where a.GUID == GUID
                    select a).FirstOrDefault();
        }

        public IQueryable<CFIMSVSTATL> GetAll()
        {
            return from a in context.CFIMSVSTATLs
                   select a;
        }

        public void Add(CFIMSVSTATL entity)
        {
            context.CFIMSVSTATLs.Add(entity);
        }

        public void Remove(CFIMSVSTATL entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.CFIMSVSTATLs.Attach(entity);
            }
            context.CFIMSVSTATLs.Remove(entity);
        }

        public void Update(CFIMSVSTATL entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.CFIMSVSTATLs.Attach(entity); context.SetAsModified(entity);
            }
        }

        public List<CFIMSVSTATL> All()
        {
            return context.CFIMSVSTATLs.ToList();
        }

        private AmitalContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<CFIMSVSTATL> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public CFIMSVSTATL GetSingle(EntityKeyFields entityKeys)
        {
            var keys = entityKeys as CFIMSVSTATLKeys;
            return this.GetSingle(keys.GUID);
        }
    }
}
