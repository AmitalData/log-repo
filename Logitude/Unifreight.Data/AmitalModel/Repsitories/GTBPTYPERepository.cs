
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
    public class GTBPTYPERepository : IRepository<GTBPTYPE>
    {
        private AmitalContext currentContext;
        public GTBPTYPERepository(int tenant)
        {
            currentContext = AmitalContext.GetContext(tenant);
        }

        public GTBPTYPERepository(AmitalContext context)
        {
            currentContext = context;
        }

        public GTBPTYPE GetSingle(string APPLICATION , string PRICETYPE )
        {
            return (from a in context.GTBPTYPEs
                    where a.APPLICATION == APPLICATION&& a.PRICETYPE == PRICETYPE
                    select a).FirstOrDefault();
        }

        private AmitalContext context
        {
            get { return currentContext; }
        }

        public List<GTBPTYPE> GetMulti(EntityKeyFields entityKeys)
        {
            throw new Exception();
        }

        public GTBPTYPE GetSingle(EntityKeyFields entityKeys)
        {
            var keys = entityKeys as GTBPTYPEKeys;
            return this.GetSingle(keys.APPLICATION, keys.APPLICATION);
        }

        public IQueryable<GTBPTYPE> GetAll()
        {
            return from a in context.GTBPTYPEs
                   select a;
        }


        public void Add(GTBPTYPE entity)
        {
            throw new NotImplementedException();
        }

        public void Remove(GTBPTYPE entity)
        {
            throw new NotImplementedException();
        }

        public void Update(GTBPTYPE entity)
        {
            throw new NotImplementedException();
        }

        public List<GTBPTYPE> All()
        {
            throw new NotImplementedException();
        }

        public void SubmitChanges()
        {
            throw new NotImplementedException();
        }
    }
}
