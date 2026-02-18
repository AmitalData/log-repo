using System.Collections.Generic;
using System.Linq;
using Simplog.Server.Infrastructure.Helpers;

using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System.Transactions;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class AirlineRepository:IRepository<Airline>
    {
        ICommonDataContext commonDataContext;

        public AirlineRepository()
        {
            commonDataContext = new CommonDataContext();
        }

        public AirlineRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public AirlineRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }
      
        public Airline GetSingleAirlineByPrefix(string prefix,int tenant)
        {
            return (from a in context.Airlines.Include("Card").Include("Card.PaymentTerm")
                    where a.Prefix == prefix && a.Tenant==tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<Airline> GetAllAirlinesByPrefix(string prefix, int tenant)
        {
            return (from a in context.Airlines where a.Prefix == prefix && a.Tenant == tenant select a);
        }

        public Airline GetSingleAirlineByCode(string code, int tenant)
        {
            return (from a in context.Airlines.Include("Card").Include("Card.PaymentTerm")
                    where a.Card.Code == code && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public int GetAirlinesCount(int tenant)
        {
            return (from record in context.Airlines.Include("Card").Include("Card.PaymentTerm") where record.Tenant == tenant select record).Count();
        }

        public IQueryable<Airline> GetAirlines(int tenant)
        {
            return (from d in context.Airlines.Include("Card").Include("Card.PaymentTerm") where d.Tenant == tenant select d);
        }

        public IQueryable<Airline> GetChampNeedsRegistrationAirlines()
        {
            return (from d in context.Airlines.Include("Card").Include("Card.PaymentTerm") where d.Tenant == 0 && d.ChampNeedsRegistration == true select d);
        }

        public IQueryable<Airline> GetGLSHKNeedsRegistrationAirlines()
        {
            return (from d in context.Airlines.Include("Card").Include("Card.PaymentTerm") where d.Tenant == 0 && d.GLSHKNeedsRegistration == true select d);
        }

        public List<Airline> GetAllAllowedAirlinesInRestriction(int tenant)
        {
            return (from d in context.Airlines.Include("Card") where d.Tenant == tenant && d.IsAllowedInAirlinesRestriction select d).ToList();
        }

        public Airline GetSingleAirline(string id, int tenant)
        {
            return (from record in context.Airlines.Include("Card").Include("Card.PaymentTerm") where record.Id == id && record.Tenant == tenant select record).FirstOrDefault();
        }

        public string GetAllowedAirlineId(int tenant)
        {
            string myResult = null;

            bool isRestrictedByAirline = false;

            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                TenantManagementRepository tenantManagementRepository = new TenantManagementRepository();
                TenantManagement tenantManagement = tenantManagementRepository.GetSingleTenantManagement(tenant);
                if (tenantManagement != null)
                {
                    isRestrictedByAirline = tenantManagement.IsRestrictedByAirline;
                }

                scope.Complete();
            }

            if (isRestrictedByAirline)
            {
                List<Airline> allowedAirlines = (from d in context.Airlines where d.Tenant == tenant && d.IsAllowedInAirlinesRestriction select d).ToList();
                if (allowedAirlines.Count == 1)
                {
                    myResult = allowedAirlines.FirstOrDefault().Id;
                }
            }

            return myResult;
        }

        public void Add(Airline entity)
        {
            context.Airlines.Add(entity);
        }

        public void Remove(Airline entity)
        {
            context.Airlines.Attach(entity);
            context.Airlines.Remove(entity);
        }

        public void Update(Airline entity)
        {
            context.Airlines.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<Airline> All()
        {
            return context.Airlines.ToList();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<Airline> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public Airline GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}