using System.Collections.Generic;
using System.Linq;
using Simplog.Server.Infrastructure.Helpers;

using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class TruckerRepository:IRepository<Trucker>
    {
        ICommonDataContext commonDataContext;



        public TruckerRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public TruckerRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public IQueryable<Trucker> GetTruckers(int tenant)
        {
            return (from record in context.Truckers.Include("Card").Include("Card.PaymentTerm") where record.Tenant == tenant select record);
        }

        public Trucker GetSingleTrucker(string id, int tenant)
        {
            return (from record in context.Truckers.Include("Card").Include("Card.PaymentTerm") where record.Id == id && record.Tenant == tenant select record).FirstOrDefault();
        }

        public Trucker GetSingleTruckerByCode(string code, int tenant)
        {
            return (from record in context.Truckers.Include("Card").Include("Card.PaymentTerm") where record.Card.Code == code && record.Tenant == tenant select record).FirstOrDefault();
        }

        public void Add(Trucker entity)
        {
            context.Truckers.Add(entity);
        }

        public void Remove(Trucker entity)
        {
            context.Truckers.Remove(entity);
        }

        public void Update(Trucker entity)
        {
            context.Truckers.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<Trucker> All()
        {
            return context.Truckers.ToList();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<Trucker> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public Trucker GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}