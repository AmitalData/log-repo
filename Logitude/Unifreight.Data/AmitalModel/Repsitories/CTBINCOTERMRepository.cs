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
    public class CTBINCOTERMRepository : IRepository<CTBINCOTERM>
    {
        private AmitalContext currentContext;
        public CTBINCOTERMRepository(int tenant)
        {
            currentContext = AmitalContext.GetContext(tenant);
        }

        public CTBINCOTERMRepository(AmitalContext context)
        {
            currentContext = context;
        }

        public CTBINCOTERM GetSingle(string PTERMID)
        {
            return (from a in context.CTBINCOTERMs
                    where a.PTERMID == PTERMID
                    select a).FirstOrDefault();
        }

        public IQueryable<CTBINCOTERM> GetAll()
        {
            return from a in context.CTBINCOTERMs
                   select a;
        }

        public void Add(CTBINCOTERM entity)
        {
            context.CTBINCOTERMs.Add(entity);
        }

        public void Remove(CTBINCOTERM entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.CTBINCOTERMs.Attach(entity);
            }
            context.CTBINCOTERMs.Remove(entity);
        }

        public void Update(CTBINCOTERM entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.CTBINCOTERMs.Attach(entity); context.SetAsModified(entity);
            }
        }

        public List<CTBINCOTERM> All()
        {
            return context.CTBINCOTERMs.ToList();
        }

        private AmitalContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<CTBINCOTERM> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public CTBINCOTERM GetSingle(EntityKeyFields entityKeys)
        {
            var keys = entityKeys as CTBINCOTERMKeys;
            return this.GetSingle(keys.PTERMID);
        }
    }
}
