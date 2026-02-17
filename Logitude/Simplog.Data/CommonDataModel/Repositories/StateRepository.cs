using System.Collections.Generic;
using System.Linq;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System.Web;
using System;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class StateRepository : IRepository<State>
    {
        ICommonDataContext commonDataContext;

        public StateRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public StateRepository()
        {
            commonDataContext = new CommonDataContext();
        }

        public StateRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public IQueryable<State> GetStates(int tenant)
        {
            return (from record in context.States.Include("Country") where record.Tenant == tenant select record);
        }

        public State GetSingleState(string id, int tenant)
        {
            return (from record in context.States.Include("Country") where record.Id == id && record.Tenant == tenant select record).FirstOrDefault();
        }

        public State GetSingleStateByCode(string code, int tenant)
        {
            return (from record in context.States.Include("Country") where record.Code == code && record.Tenant == tenant select record).FirstOrDefault();
        }

        public bool CheckStateAlreadyExists(string code, string id, int tenant)
        {
            return (from a in context.States where a.Tenant == tenant && a.Code == code && a.Id != id select a).Any();
        }

        public bool IsCountryHasStates(string countryId, int tenant)
        {
            return (from a in context.States where a.CountryId == countryId && a.Tenant == tenant select a).Any();
        }



        public string GetStateIdByCode(string code, int tenant)
        {
            return (from record in context.States where record.Code == code && record.Tenant == tenant select record.Id).FirstOrDefault();
        }

        public void Add(State entity)
        {
            context.States.Add(entity);
        }

        public void Remove(State entity)
        {
            context.States.Attach(entity);
            context.States.Remove(entity);
        }

        public void Update(State entity)
        {
            context.States.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<State> All()
        {
            return context.States.ToList();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<State> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public State GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public State GetSingleState(string id, int tenant, bool getFromCache)
        {
            return (from record in context.States.Include("Country") where record.Id == id && record.Tenant == tenant select record).FirstOrDefault();
        }
    }
}