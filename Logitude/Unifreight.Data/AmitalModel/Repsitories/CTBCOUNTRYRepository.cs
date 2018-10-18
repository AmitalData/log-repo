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
    public class CTBCOUNTRYRepository : IRepository<CTBCOUNTRY>
    {
        private AmitalContext currentContext;
        public CTBCOUNTRYRepository(int tenant)
        {
            currentContext = AmitalContext.GetContext(tenant);
        }

        public CTBCOUNTRYRepository(AmitalContext context)
        {
            currentContext = context;
        }

        public CTBCOUNTRY GetSingle(string COUNTRYID)
        {
            return (from a in context.CTBCOUNTRIES 
                    where a.COUNTRYID == COUNTRYID
                    select a).FirstOrDefault();
        }

        public IQueryable<CTBCOUNTRY> GetAll()
        {
            return from a in context.CTBCOUNTRIES
                   select a;
        }

        public void Add(CTBCOUNTRY entity)
        {
            context.CTBCOUNTRIES.Add(entity);
        }

        public void Remove(CTBCOUNTRY entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.CTBCOUNTRIES.Attach(entity);
            }
            context.CTBCOUNTRIES.Remove(entity);
        }

        public void Update(CTBCOUNTRY entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.CTBCOUNTRIES.Attach(entity); context.SetAsModified(entity);
            }
        }

        public List<CTBCOUNTRY> All()
        {
            return context.CTBCOUNTRIES.ToList();
        }

        private AmitalContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<CTBCOUNTRY> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public CTBCOUNTRY GetSingle(EntityKeyFields entityKeys)
        {
            var keys = entityKeys as CTBCOUNTRYKeys;
            return this.GetSingle(keys.COUNTRYID);
        }
    }
}
