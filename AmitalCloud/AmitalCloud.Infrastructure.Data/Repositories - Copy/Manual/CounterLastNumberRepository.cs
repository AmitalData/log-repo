using AmitalCloud.Infrastructure.Data.Context;
using AmitalCloud.Infrastructure.Data.EntityPOCOs;
using AmitalCloud.Infrastructure.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AmitalCloud.Infrastructure.Data.Repositories
{
    public class CounterLastNumberRepository:IRepository<CounterLastNumber,string>
    {
          IAmitalCloudContext currentContext;
        public CounterLastNumberRepository()
        {
            currentContext = new AmitalCloudContext();
        }
        public CounterLastNumberRepository(IAmitalCloudContext context)
        {
            currentContext = context;

        }
        public CounterLastNumberRepository(int tenant)
        {
            currentContext = AmitalCloudContext.GetContext(tenant);
        }
        #region IRepository<CounterLastNumber> Members

        public void Add(CounterLastNumber entity)
        {
            //this.context.CounterLastNumbers.Add(entity);
        }

        public void Remove(CounterLastNumber entity)
        {
            context.CounterLastNumbers.Attach(entity);
            context.CounterLastNumbers.Remove(entity);
        }

        public void Update(CounterLastNumber entity)
        {
            // this.context.CounterLastNumbers.Attach(entity);
            //this.context.SetAsModified(entity);
        }

        public List<CounterLastNumber> All()
        {
            List<CounterLastNumber> xx = new List<CounterLastNumber>();
            return xx;
            //return this.context.CounterLastNumbers.ToList<Counter>();
        }

        public IAmitalCloudContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            this.context.SaveChanges();
        }

        #endregion
       
        #region IRepository<CounterLastNumber> Members




        #endregion
        public bool CheckIfExists(string counterId, int tenant)
        {
            bool exists = (from a in context.CounterStats
                           where a.CounterId == counterId && a.Tenant == tenant
                           select a).Any();
            return exists;
        }
        public List<CounterLastNumber> GetMulti(IEntityKeyFields<CounterLastNumber,string> entityKeys)
        {
            throw new System.NotImplementedException();
        }
        public CounterLastNumber GetSingle(IEntityKeyFields<CounterLastNumber,string> entityKeys)
        {
            throw new System.NotImplementedException();
        }
        public CounterLastNumber GetSingleByTableName(string tableName, int tenant)
        {
            CounterLastNumber a = context.CounterLastNumbers.Where(d => d.TableName == tableName && d.Tenant == tenant).FirstOrDefault();
            return a;
        }
    }
}