using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class DistributorRepository : IRepository<Distributor>
    {
        ICommonDataContext commonDataContext;

        public DistributorRepository()
        {
            commonDataContext = new CommonDataContext();
        }

        public DistributorRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public DistributorRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public IQueryable<Distributor> GetDistributors()
        {
            return context.Distributors;
        }

        public Distributor GetSingleDistributor(string code, int tenant = 0)
        {
            Distributor entity = (from record in context.Distributors where record.Code == code select record).FirstOrDefault();
            return entity;

        }

        public void Add(Distributor entity)
        {
            context.Distributors.Add(entity);
        }

        public void Remove(Distributor entity)
        {
            try
            {
                context.Distributors.Attach(entity);
            }
            catch { }
            context.Distributors.Remove(entity);
        }

        public void Update(Distributor entity)
        {
            try
            {
                context.Distributors.Attach(entity);
            }
            catch { }
            context.SetAsModified(entity);
        }

        public List<Distributor> All()
        {
            return context.Distributors.ToList();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<Distributor> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public Distributor GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }
    }
}
