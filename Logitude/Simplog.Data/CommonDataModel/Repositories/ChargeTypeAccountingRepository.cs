using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class ChargeTypeAccountingRepository:IRepository<ChargeTypeAccounting>
    {
        ICommonDataContext commonDataContext;

        public ChargeTypeAccountingRepository()
        {
            commonDataContext = new CommonDataContext();
        }

        public ChargeTypeAccountingRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public ChargeTypeAccountingRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public ChargeTypeAccounting GetSingleChargeTypeAccountings(string id,int tenant)
        {
            return (from d in context.ChargeTypeAccountings.Include("VatType").Include("ChargeType") where d.Id == id && d.Tenant == tenant select d).FirstOrDefault();
        }

        public IQueryable<ChargeTypeAccounting> GetChargeTypeAccountings(int tenant)
        {
            return (from d in context.ChargeTypeAccountings.Include("VatType").Include("ChargeType") where d.Tenant == tenant select d);
        }

        public IQueryable<ChargeTypeAccounting> GetChargeTypeAccountingsForChargeType(string chargeTypeId,int tenant)
        {
            return (from d in context.ChargeTypeAccountings.Include("VatType").Include("ChargeType") where d.ChargeTypeId == chargeTypeId && d.Tenant == tenant select d);
        }

        public void Add(ChargeTypeAccounting entity)
        {
            context.ChargeTypeAccountings.Add(entity);
        }

        public void Remove(ChargeTypeAccounting entity)
        {
            context.ChargeTypeAccountings.Attach(entity);
            context.ChargeTypeAccountings.Remove(entity);
        }

        public void Update(ChargeTypeAccounting entity)
        {
            context.ChargeTypeAccountings.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ChargeTypeAccounting> All()
        {
            return context.ChargeTypeAccountings.ToList();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }



        public List<ChargeTypeAccounting> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public ChargeTypeAccounting GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }
    }
}
