using System.Collections.Generic;
using System.Linq;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
namespace Simplog.Data.InfrastructureModel.Repositories
{
    public class CounterLastNumberRepository:IRepository<CounterLastNumber>
    {
          IWebFreightContext webFreightContext;
        public CounterLastNumberRepository()
        {
            webFreightContext = new WebFreightContext();
        }
        public CounterLastNumberRepository(IWebFreightContext context)
        {
            webFreightContext = context;

        }
        public CounterLastNumberRepository(int tenant)
        {
            webFreightContext = WebFreightContext.GetContext(tenant);
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

        public IWebFreightContext context
        {
            get { return webFreightContext; }
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

        public List<CounterLastNumber> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public CounterLastNumber GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
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