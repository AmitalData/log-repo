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
    public class DepartmentQuery
    {
        DepartmentRepository repository;
        public DepartmentQuery()
        {
            repository = new DepartmentRepository(); 
        }

        public DepartmentQuery(int tenant)
        {
            repository = new DepartmentRepository(tenant);
        }

        public DepartmentQuery(DepartmentRepository departmentRepository)
        {
            repository = departmentRepository;
        }

     
        public DepartmentPM GetSinglePM(string id, int tenant)
        {
            if (!string.IsNullOrEmpty(id))
            {
                string entityName = "DepartmentPM" + id + tenant;
                DepartmentPM entity;
                if (HttpContext.Current != null)
                {
                    if (CacheManager.CacheWrapper.Get(entityName) == null)
                    {

                        var departments = (from a in repository.context.Departments
                                           where a.Tenant == tenant
                                           select new DepartmentPM()
                                           {

                                               EnglishName = a.EnglishName,
                                               Id = a.Id,
                                               InActive = a.InActive,
                                               LocalName = a.LocalName,
                                               Notes = a.Notes,
                                               DirectionId = a.DirectionId,
                                               Tenant = a.Tenant,
                                               SearchFields = a.SearchFields,
                                               ComputedLocalName = string.IsNullOrEmpty(a.LocalName) ? a.EnglishName : a.LocalName,
                                               Code = a.Code,
                                           });

                        foreach (var c in departments)
                        {
                            string cname = "DepartmentPM" + c.Id + c.Tenant;

                            if (CacheManager.CacheWrapper.Get(cname) == null)
                            {
                                CacheManager.CacheWrapper.Insert(cname, c, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                            }
                        }
                        entity = (DepartmentPM)CacheManager.CacheWrapper.Get(entityName);

                    }
                    else
                    {
                        entity = (DepartmentPM)CacheManager.CacheWrapper.Get(entityName);
                    }
                }
                else
                {
                    entity = (from a in repository.context.Departments
                              where a.Tenant == tenant && a.Id == id
                              select new DepartmentPM()
                              {

                                  EnglishName = a.EnglishName,
                                  Id = a.Id,
                                  InActive = a.InActive,
                                  LocalName = a.LocalName,
                                  Notes = a.Notes,
                                  DirectionId = a.DirectionId,
                                  Tenant = a.Tenant,
                                  SearchFields = a.SearchFields,
                                  ComputedLocalName = string.IsNullOrEmpty(a.LocalName) ? a.EnglishName : a.LocalName,
                                  Code = a.Code,
                              }).FirstOrDefault();
                }
                DepartmentPM securedPm = new DepartmentPM();
                SecuredMapping.GetMappedPM(entity, securedPm, "Department", tenant);

                return securedPm;
            }
            return null;
        }

        public IQueryable<DepartmentPM> GetDepartmentPMsByTenant(int tenant)
        {
            IQueryable<DepartmentPM> departments = from a in repository.context.Departments
                                                   where a.Tenant == tenant
                                                   select new DepartmentPM()
                                                   {
                                                       EnglishName = a.EnglishName,
                                                       Id = a.Id,
                                                       InActive = a.InActive,
                                                       LocalName = a.LocalName,
                                                       Notes = a.Notes,
                                                       DirectionId = a.DirectionId,
                                                       Tenant = a.Tenant,
                                                       SearchFields = a.SearchFields,
                                                       ComputedLocalName = string.IsNullOrEmpty(a.LocalName) ? a.EnglishName : a.LocalName,
                                                       Code = a.Code,
                                                   };
            return departments;
        }

        public DepartmentPM GetDepartmentByName(string name, int tenant)
        {
            string nameNew = "";
            nameNew = name;

            var query = (from a in repository.context.Departments
                         where a.Tenant == tenant
                         select new DepartmentPM()
                         {
                             EnglishName = a.EnglishName,
                             Id = a.Id,
                             InActive = a.InActive,
                             LocalName = a.LocalName,
                             Notes = a.Notes,
                             DirectionId = a.DirectionId,
                             Tenant = a.Tenant,
                             SearchFields = a.SearchFields,
                             ComputedLocalName = string.IsNullOrEmpty(a.LocalName) ? a.EnglishName : a.LocalName,
                             Code = a.Code,
                         }).FirstOrDefault();
            return query;
        }

        public IQueryable<DepartmentList> GetIQueryableEntityList(IQueryable<Department> iQueryable)
        {
            IQueryable<DepartmentList> result = from department in iQueryable
                                                select new DepartmentList()
                                                {
                                                    EnglishName = department.EnglishName,
                                                    LocalName = department.LocalName,
                                                    Notes = department.Notes,
                                                    DirectionId = department.DirectionId,
                                                    InActive = department.InActive,
                                                    Id = department.Id,
                                                    Tenant = department.Tenant,
                                                    SearchFields = department.SearchFields,
                                                    Code = department.Code,
                                                };
            return result;
        }
        public Department GetFirstDepartmentForTenant(int tenant)
        {
            return (from a in repository.context.Departments
                    where a.Tenant == tenant
                    select a).FirstOrDefault();
        }
        
        public DepartmentPM GetSinglePMByCode(string Code, int tenant, bool getFromCache = true)
        {
            if (!string.IsNullOrEmpty(Code))
            {
                string entityName = "DepartmentPM" + Code + tenant;
                DepartmentPM entity;
                if (getFromCache)
                {
                    if (HttpContext.Current != null)
                    {
                        if (CacheManager.CacheWrapper.Get(entityName) == null)
                        {
                            var departments = (from a in repository.context.Departments
                                            where a.Tenant == tenant
                                            && a.Code == Code
                                            select new DepartmentPM()
                                            {
                                                EnglishName = a.EnglishName,
                                                Id = a.Id,
                                                InActive = a.InActive,
                                                LocalName = a.LocalName,
                                                Notes = a.Notes,
                                                DirectionId = a.DirectionId,
                                                Tenant = a.Tenant,
                                                SearchFields = a.SearchFields,
                                                ComputedLocalName = string.IsNullOrEmpty(a.LocalName) ? a.EnglishName : a.LocalName,
                                                Code = a.Code,
                                            });

                            foreach (var c in departments)
                            {
                                string cname = "DepartmentPM" + c.Code + c.Tenant;

                                if (CacheManager.CacheWrapper.Get(cname) == null)
                                {
                                    CacheManager.CacheWrapper.Insert(cname, c, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                                }
                            }
                            entity = (DepartmentPM)CacheManager.CacheWrapper.Get(entityName);

                        }
                        else
                        {
                            entity = (DepartmentPM)CacheManager.CacheWrapper.Get(entityName);
                        }
                    }
                    else
                    {
                        entity = (from a in repository.context.Departments
                                  where a.Tenant == tenant && a.Code == Code
                                  select new DepartmentPM()
                                  {
                                      EnglishName = a.EnglishName,
                                      Id = a.Id,
                                      InActive = a.InActive,
                                      LocalName = a.LocalName,
                                      Notes = a.Notes,
                                      DirectionId = a.DirectionId,
                                      Tenant = a.Tenant,
                                      SearchFields = a.SearchFields,
                                      ComputedLocalName = string.IsNullOrEmpty(a.LocalName) ? a.EnglishName : a.LocalName,
                                      Code = a.Code,
                                  }).FirstOrDefault();
                    }
                }
                else
                {
                    entity = (from a in repository.context.Departments
                              where a.Tenant == tenant && a.Code == Code
                              select new DepartmentPM()
                              {
                                  EnglishName = a.EnglishName,
                                  Id = a.Id,
                                  InActive = a.InActive,
                                  LocalName = a.LocalName,
                                  DirectionId = a.DirectionId,
                                  Notes = a.Notes,
                                  Tenant = a.Tenant,
                                  SearchFields = a.SearchFields,
                                  ComputedLocalName = string.IsNullOrEmpty(a.LocalName) ? a.EnglishName : a.LocalName,
                                  Code = a.Code,
                              }).FirstOrDefault();
                }

                if (entity != null)
                {
                    DepartmentPM securedPm = new DepartmentPM();
                    SecuredMapping.GetMappedPM(entity, securedPm, "Department", tenant);
                    return securedPm;
                }
                else
                {
                    return null;
                }
            }
            return null;
        }
    }
}
