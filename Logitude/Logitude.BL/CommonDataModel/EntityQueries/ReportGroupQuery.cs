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
    public class ReportGroupQuery
    {
        ReportGroupRepository repository;



        public ReportGroupQuery(int tenant)
        {
            repository = new ReportGroupRepository(tenant);
        }

        public ReportGroupQuery(ReportGroupRepository ReportGroupRepository)
        {
            repository = ReportGroupRepository;
        }
        public ReportGroupPM GetSinglePM(string id, int tenant)
        {
            if (!string.IsNullOrEmpty(id))
            {
                string entityName = "ReportGroupPM" + id + tenant;
                ReportGroupPM entity;
                if (HttpContext.Current != null)
                {
                    if (CacheManager.CacheWrapper.Get(entityName) == null)
                    {

                        var ReportGroups = (from a in repository.context.ReportGroups
                                            where a.Tenant == tenant
                                            select new ReportGroupPM()
                                            {
                                                Id = a.Id,
                                                Tenant = a.Tenant,
                                                LocalName = a.LocalName,
                                                EnglishName = a.EnglishName,
                                                Code = a.Code,
                                                OrderNumber = a.OrderNumber,
                                            });

                        foreach (var c in ReportGroups)
                        {
                            string cname = "ReportGroupPM" + c.Id + c.Tenant;

                            if (CacheManager.CacheWrapper.Get(cname) == null)
                            {
                                CacheManager.CacheWrapper.Insert(cname, c, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                            }
                        }
                        entity = (ReportGroupPM)CacheManager.CacheWrapper.Get(entityName);

                    }
                    else
                    {
                        entity = (ReportGroupPM)CacheManager.CacheWrapper.Get(entityName);
                    }
                }

                else
                {
                    entity = (from a in repository.context.ReportGroups
                              where a.Tenant == tenant && a.Id == id
                              select new ReportGroupPM()
                              {
                                  Id = a.Id,
                                  Tenant = a.Tenant,
                                  LocalName = a.LocalName,
                                  EnglishName = a.EnglishName,
                                  Code = a.Code,
                                  OrderNumber = a.OrderNumber,
                              }).FirstOrDefault();
                }
                ReportGroupPM securedPm = new ReportGroupPM();
                SecuredMapping.GetMappedPM(entity, securedPm, "ReportGroup", tenant);

                return securedPm;
            }
            return null;
        }

        public IQueryable<ReportGroupPM> GetReportGroupPMsByTenant(int tenant)
        {
            IQueryable<ReportGroupPM> ReportGroups = from a in repository.context.ReportGroups
                                                     where a.Tenant == tenant
                                                     select new ReportGroupPM()
                                                      {
                                                          Id = a.Id,
                                                          Tenant = a.Tenant,
                                                          LocalName = a.LocalName,
                                                          EnglishName = a.EnglishName,
                                                          Code = a.Code,
                                                          OrderNumber = a.OrderNumber,
                                                      };
            return ReportGroups;
        }

        public ReportGroupPM GetReportGroupByName(string name, int tenant)
        {
            string nameNew = "";
            nameNew = name;

            var query = (from a in repository.context.ReportGroups
                         where a.Tenant == tenant && a.EnglishName == name
                         select new ReportGroupPM()
                         {
                             Id = a.Id,
                             Tenant = a.Tenant,
                             LocalName = a.LocalName,
                             EnglishName = a.EnglishName,
                             Code = a.Code,
                             OrderNumber = a.OrderNumber,
                         }).FirstOrDefault();

            return query;
        }

        public IQueryable<ReportGroupList> GetIQueryableEntityList(IQueryable<ReportGroup> iQueryable)
        {
            IQueryable<ReportGroupList> result = from reportGroup in iQueryable
                                                 select new ReportGroupList()
                                                 {
                                                     Id = reportGroup.Id,
                                                     Tenant = reportGroup.Tenant,
                                                     Code = reportGroup.Code,
                                                     EnglishName = reportGroup.EnglishName,
                                                     LocalName = reportGroup.LocalName,
                                                     OrderNumber = reportGroup.OrderNumber
                                                 };
            return result;
        }

        public ReportGroup GetFirstReportGroupForTenant(int tenant)
        {
            return (from a in repository.context.ReportGroups
                    where a.Tenant == tenant
                    select a).FirstOrDefault();
        }
    }
}
