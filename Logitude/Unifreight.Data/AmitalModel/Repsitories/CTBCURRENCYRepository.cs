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
    public class CTBCURRENCYRepository : IRepository<CTBCURRENCY>
    {
        private AmitalContext currentContext;
        public CTBCURRENCYRepository(int tenant)
        {
            currentContext = AmitalContext.GetContext(tenant);
        }

        public CTBCURRENCYRepository(AmitalContext context)
        {
            currentContext = context;
        }

        public CTBCURRENCY GetSingle(string CURRENCYID)
        {
            return (from a in context.CTBCURRENCIES
                    where a.CURRENCYID == CURRENCYID
                    select a).FirstOrDefault();
        }

        public IQueryable<CTBCURRENCY> GetAll()
        {
            return from a in context.CTBCURRENCIES
                   select a;
        }

        public void Add(CTBCURRENCY entity)
        {
            context.CTBCURRENCIES.Add(entity);
        }

        public void Remove(CTBCURRENCY entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.CTBCURRENCIES.Attach(entity);
            }
            context.CTBCURRENCIES.Remove(entity);
        }

        public void Update(CTBCURRENCY entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.CTBCURRENCIES.Attach(entity); context.SetAsModified(entity);
            }
        }

        public List<CTBCURRENCY> All()
        {
            return context.CTBCURRENCIES.ToList();
        }

        private AmitalContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<CTBCURRENCY> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public CTBCURRENCY GetSingle(EntityKeyFields entityKeys)
        {
            var keys = entityKeys as CTBCURRENCYKeys;
            return this.GetSingle(keys.CURRENCYID);
        }
    }
}
