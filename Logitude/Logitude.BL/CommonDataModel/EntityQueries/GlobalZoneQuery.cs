using System;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using Logitude.BL.Helpers;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityLists;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;

namespace Logitude.BL.CommonDataModel.EntityQueries
{
    public class GlobalZoneQuery
    {
        GlobalZoneRepository repository;

        public GlobalZoneQuery()
        {
            repository = new GlobalZoneRepository(); 
        }

        public GlobalZoneQuery(int tenant)
        {
            repository = new GlobalZoneRepository(tenant);
        }

        public GlobalZoneQuery(GlobalZoneRepository repository)
        {
            this.repository = repository;
        }

        public GlobalZonePM GetSinglePM(string id, int tenant)
        {
            GlobalZonePM instance = (from a in repository.context.GlobalZones
                                     where a.Tenant == tenant
                                     && a.Id == id
                                     select new GlobalZonePM()
                                     {
                                         Code = a.Code,
                                         EnglishName = a.EnglishName,
                                         Id = a.Id,
                                         InActive = a.InActive,
                                         LocalName = a.LocalName,
                                         Notes = a.Notes,
                                         Tenant = a.Tenant,
                                         SearchFields = a.SearchFields,
                                         ComputedLocalName = string.IsNullOrEmpty(a.LocalName) ? a.EnglishName : a.LocalName,
                                     }).FirstOrDefault();

            GlobalZonePM securedPm = new GlobalZonePM();
            SecuredMapping.GetMappedPM(instance, securedPm, "GlobalZone", tenant);

            return securedPm;
        }

        public IQueryable<GlobalZonePM> GetGlobalZonePMsByTenant(int tenant)
        {
            IQueryable<GlobalZonePM> query = from a in repository.context.GlobalZones
                                             where a.Tenant == tenant
                                             select new GlobalZonePM()
                                             {
                                                 Code = a.Code,
                                                 EnglishName = a.EnglishName,
                                                 Id = a.Id,
                                                 InActive = a.InActive,
                                                 LocalName = a.LocalName,
                                                 Notes = a.Notes,
                                                 Tenant = a.Tenant,
                                                 SearchFields = a.SearchFields,
                                                 ComputedLocalName = string.IsNullOrEmpty(a.LocalName) ? a.EnglishName : a.LocalName,
                                             };
            return query;
        }

        public IQueryable<GlobalZonePM> GetGlobalZonesByCodeOrName(string code, string name, int tenant)
        {
            string codeNew = "";
            codeNew = code;

            string nameNew = "";
            nameNew = name;

            var query = (from a in repository.context.GlobalZones
                         where a.Tenant == tenant
                         select new GlobalZonePM()
                         {
                             Code = a.Code,
                             EnglishName = a.EnglishName,
                             Id = a.Id,
                             InActive = a.InActive,
                             LocalName = a.LocalName,
                             Notes = a.Notes,
                             Tenant = a.Tenant,
                             SearchFields = a.SearchFields,
                             ComputedLocalName = string.IsNullOrEmpty(a.LocalName) ? a.EnglishName : a.LocalName,
                         }).AsQueryable();

            IQueryable<GlobalZonePM> query2 = null;
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

        public IQueryable<GlobalZoneList> GetIQueryableEntityList(IQueryable<GlobalZone> iQueryable)
        {
            IQueryable<GlobalZoneList> result = from a in iQueryable
                                                select new GlobalZoneList()
                                                {
                                                    Code = a.Code,
                                                    EnglishName = a.EnglishName,
                                                    Id = a.Id,
                                                    InActive = a.InActive,
                                                    LocalName = a.LocalName,
                                                    Notes = a.Notes,
                                                    Tenant = a.Tenant,
                                                    SearchFields = a.SearchFields,
                                                };
            return result;
        }
    }
}