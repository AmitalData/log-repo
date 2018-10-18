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
    public class CTBPKDTRepository : IRepository<CTBPKDT>
    {
        private AmitalContext currentContext;
        public CTBPKDTRepository(int tenant)
        {
            currentContext = AmitalContext.GetContext(tenant);
        }

        public CTBPKDTRepository(AmitalContext context)
        {
            currentContext = context;
        }

        public CTBPKDT GetSingle(string PACKDETAIL)
        {
            return (from a in context.CTBPKDTs
                    where a.PACKDETAIL == PACKDETAIL
                    select a).FirstOrDefault();
        }

        public IQueryable<CTBPKDT> GetAll()
        {
            return from a in context.CTBPKDTs
                   select a;
        }

        public void Add(CTBPKDT entity)
        {
            context.CTBPKDTs.Add(entity);
        }

        public void Remove(CTBPKDT entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.CTBPKDTs.Attach(entity);
            }
            context.CTBPKDTs.Remove(entity);
        }

        public void Update(CTBPKDT entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.CTBPKDTs.Attach(entity); context.SetAsModified(entity);
            }
        }

        public List<CTBPKDT> All()
        {
            return context.CTBPKDTs.ToList();
        }

        private AmitalContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<CTBPKDT> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public CTBPKDT GetSingle(EntityKeyFields entityKeys)
        {
            var keys = entityKeys as CTBPKDTKeys;
            return this.GetSingle(keys.PACKDETAIL);
        }
    }
}
