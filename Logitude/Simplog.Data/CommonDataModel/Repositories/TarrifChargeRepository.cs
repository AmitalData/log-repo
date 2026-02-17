using System.Collections.Generic;
using System.Linq;
using Simplog.Server.Infrastructure.Helpers;

using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class TarrifChargeRepository:IRepository<TarrifCharge>
    {
        ICommonDataContext commonDataContext;

        public TarrifChargeRepository()
        {
            commonDataContext = new CommonDataContext();
        }

        public TarrifChargeRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public TarrifChargeRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public TarrifCharge GetSingleTarrifCharge(string id,int tenant = 0)
        {
            return (from a in context.TarrifCharges.Include("ChargesType").Include("Currency").Include("Measurement")
                    where a.Id == id
                    select a).FirstOrDefault();
        }

        public IQueryable<TarrifCharge> GetTarrifChargesByTenant(int tenant)
        {
            return from a in context.TarrifCharges.Include("ChargesType").Include("Currency").Include("Measurement")
                   where a.Tenant == tenant
                   select a;
        }


        public IQueryable<TarrifCharge> GetTarrifCharges(int tenant)
        {
            return from a in context.TarrifCharges.Include("ChargesType").Include("Currency").Include("Measurement")
                   where a.Tenant == tenant
                   select a;
        }
        

        public void Add(TarrifCharge entity)
        {
            context.TarrifCharges.Add(entity);
        }

        public void Remove(TarrifCharge entity)
        {
            context.TarrifCharges.Attach(entity);
            context.TarrifCharges.Remove(entity);
        }

        public void Update(TarrifCharge entity)
        {
            context.TarrifCharges.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<TarrifCharge> All()
        {
            return context.TarrifCharges.ToList();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext ; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<TarrifCharge> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public TarrifCharge GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}