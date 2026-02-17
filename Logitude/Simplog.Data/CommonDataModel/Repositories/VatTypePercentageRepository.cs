using System;
using System.Collections.Generic;
using System.Linq;

using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Server.Infrastructure;
namespace Simplog.Data.CommonDataModel.Repositories
{
    public class VatTypePercentageRepository:IRepository<VatTypePercentage>
    {
        ICommonDataContext commonDataContext;

        public VatTypePercentageRepository()
        {
            commonDataContext = new CommonDataContext();
        }

        public VatTypePercentageRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public VatTypePercentageRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public VatTypePercentage GetSingleVatTypePercentage(string id)
        {
            return (from a in context.VatTypePercentages.Include("VatType")
                    where a.Id == id
                    select a).FirstOrDefault();
        }

        public  IQueryable<VatTypePercentage> GetVatTypePercentagesByTenant(int tenant)
        {
            return from a in context.VatTypePercentages.Include("VatType")
                   where a.Tenant == tenant
                   select a;
        }

        public VatTypePercentage GetVatTypePercentageByDate(string vatTypeId,int tenant, DateTime? date)
        {
            VatTypePercentage result = null;
            if (context.VatTypePercentages.Count() > 0)
            {
                result =
                    (from r in context.VatTypePercentages
                     where r.VatTypeId == vatTypeId
                     && r.Tenant == tenant
                     && System.Data.Entity.DbFunctions.TruncateTime(r.FromDate) <= date
                     select r).OrderByDescending(o => o.FromDate).FirstOrDefault();
            }

            return result;
        }

        public void Add(VatTypePercentage entity)
        {
            context.VatTypePercentages.Add(entity);
        }

        public void Remove(VatTypePercentage entity)
        {
            context.VatTypePercentages.Attach(entity);
            context.VatTypePercentages.Remove(entity);
        }

        public void Update(VatTypePercentage entity)
        {
            context.VatTypePercentages.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<VatTypePercentage> All()
        {
            return context.VatTypePercentages.ToList();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<VatTypePercentage> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public VatTypePercentage GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }
    }
}