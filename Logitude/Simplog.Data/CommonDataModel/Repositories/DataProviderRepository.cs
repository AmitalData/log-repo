using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class DataProviderRepository : IRepository<DataProvider>
    {
        ICommonDataContext commonDataContext;

        public DataProviderRepository()
        {
            commonDataContext = new CommonDataContext();
        }

        public DataProviderRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public DataProviderRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public IQueryable<DataProvider> GetDataProviders()
        {
            return context.DataProviders;
        }

        public DataProvider GetSingleDataProvider(string code)
        {
            return (from record in context.DataProviders where record.Code == code select record).FirstOrDefault();
        }

        public string GetSingleHasDataProvider(string code)
        {
            return (from record in context.DataProviders where record.Code == code select record.Code).FirstOrDefault();
        }


        public void Add(DataProvider entity)
        {
            context.DataProviders.Add(entity);
        }

        public void Remove(DataProvider entity)
        {
            context.DataProviders.Attach(entity);
            context.DataProviders.Remove(entity);
        }

        public void Update(DataProvider entity)
        {
            context.DataProviders.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<DataProvider> All()
        {
            return context.DataProviders.ToList();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<DataProvider> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public DataProvider GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}
