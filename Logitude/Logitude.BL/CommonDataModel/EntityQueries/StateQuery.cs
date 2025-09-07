using System;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using Logitude.BL.Helpers;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityLists;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;

namespace Logitude.BL.CommonDataModel.EntityQueries
{
    public class StateQuery
    {
        StateRepository repository;



        public StateQuery(int tenant)
        {
            repository = new StateRepository(tenant);
        }

        public StateQuery(StateRepository repository)
        {
            this.repository = repository;
        }

        public StatePM GetSinglePM(string id, int tenant)
        {
            StatePM state = (from a in repository.context.States.Include("Country")
                             where a.Tenant == tenant && a.Id == id
                             select new StatePM()
                             {
                                 AddedManually = a.AddedManually,
                                 Code = a.Code,
                                 CountryId = a.CountryId,
                                 EnglishName = a.EnglishName,
                                 Id = a.Id,
                                 InActive = a.InActive,
                                 LocalName = a.LocalName,
                                 Notes = a.Notes,
                                 Tenant = a.Tenant,
                                 CountryCode = a.Country.Code,
                                 SearchFields = a.SearchFields,
                                 QBOTransactionLocationCode=a.QBOTransactionLocationCode,
                                 ComputedLocalName = string.IsNullOrEmpty(a.LocalName) ? a.EnglishName : a.LocalName,
                             }).FirstOrDefault();

            StatePM securedPm = new StatePM();
            SecuredMapping.GetMappedPM(state, securedPm, "State", tenant);

            return securedPm;
        }

        public StatePM GetSinglePMByCode(string code, int tenant)
        {
            StatePM state = (from a in repository.context.States.Include("Country")
                             where a.Tenant == tenant && a.Code == code
                             select new StatePM()
                             {
                                 AddedManually = a.AddedManually,
                                 Code = a.Code,
                                 CountryId = a.CountryId,
                                 EnglishName = a.EnglishName,
                                 Id = a.Id,
                                 InActive = a.InActive,
                                 LocalName = a.LocalName,
                                 Notes = a.Notes,
                                 Tenant = a.Tenant,
                                 CountryCode = a.Country.Code,
                                 SearchFields = a.SearchFields,
                                 QBOTransactionLocationCode = a.QBOTransactionLocationCode,
                                 ComputedLocalName = string.IsNullOrEmpty(a.LocalName) ? a.EnglishName : a.LocalName,
                             }).FirstOrDefault();

            StatePM securedPm = new StatePM();
            SecuredMapping.GetMappedPM(state, securedPm, "State", tenant);

            return securedPm;
        }

        public IQueryable<StatePM> GetStatePMsByTenant(int tenant)
        {
            IQueryable<StatePM> states = from a in repository.context.States.Include("Country")
                                         where a.Tenant == tenant
                                         select new StatePM()
                                         {
                                             AddedManually = a.AddedManually,
                                             Code = a.Code,
                                             CountryId = a.CountryId,
                                             EnglishName = a.EnglishName,
                                             Id = a.Id,
                                             InActive = a.InActive,
                                             LocalName = a.LocalName,
                                             Notes = a.Notes,
                                             Tenant = a.Tenant,
                                             CountryCode = a.Country.Code,
                                             SearchFields = a.SearchFields,
                                             QBOTransactionLocationCode = a.QBOTransactionLocationCode,
                                             ComputedLocalName = string.IsNullOrEmpty(a.LocalName) ? a.EnglishName : a.LocalName,
                                         };
            return states;
        }

        public IQueryable<StatePM> GetStatesByCodeOrName(string code, string name, int tenant)
        {
            string codeNew = "";
            codeNew = code;

            string nameNew = "";
            nameNew = name;

            var query = (from a in repository.context.States.Include("Country")
                         where a.Tenant == tenant
                         select new StatePM()
                         {
                             AddedManually = a.AddedManually,
                             Code = a.Code,
                             CountryId = a.CountryId,
                             EnglishName = a.EnglishName,
                             Id = a.Id,
                             InActive = a.InActive,
                             LocalName = a.LocalName,
                             Notes = a.Notes,
                             Tenant = a.Tenant,
                             CountryCode = a.Country.Code,
                             SearchFields = a.SearchFields,
                             QBOTransactionLocationCode = a.QBOTransactionLocationCode,
                             ComputedLocalName = string.IsNullOrEmpty(a.LocalName) ? a.EnglishName : a.LocalName,
                         }).AsQueryable();

            IQueryable<StatePM> query2 = null;
            if (!string.IsNullOrEmpty(code))
            {
                query2 = query.Where(d => d.Code.ToUpper().StartsWith(code.ToUpper()));
            }
            if (!string.IsNullOrEmpty(name))
            {
                if (query2 != null)
                {
                    if (query2.Count() == 0)
                    {
                        query2 = query.Where(d => d.EnglishName.ToUpper().StartsWith(name.ToUpper()));
                    }
                }
                else
                {
                    query2 = query.Where(d => d.EnglishName.ToUpper().StartsWith(name.ToUpper()));
                }
            }
            if (query2 != null)
            {
                return query2;
            }
            else
                return query;
        }

        public IQueryable<StateList> GetIQueryableEntityList(IQueryable<State> iQueryable)
        {
            IQueryable<StateList> result = from state in iQueryable.Include("Country")
                                           select new StateList()
                                           {
                                               Code = state.Code,
                                               EnglishName = state.EnglishName,
                                               LocalName = state.LocalName,
                                               InActive = state.InActive,
                                               Notes = state.Notes,
                                               CountryEnglishName = (state.Country != null ? (state.Country.EnglishName) : ""),
                                               Id = state.Id,
                                               Tenant = state.Tenant,
                                               CountryId = state.CountryId,
                                               SearchFields = state.SearchFields,
                                               QBOTransactionLocationCode = state.QBOTransactionLocationCode,
                                               AddedManually = state.AddedManually,
                                           };
            return result;
        }
    }
}