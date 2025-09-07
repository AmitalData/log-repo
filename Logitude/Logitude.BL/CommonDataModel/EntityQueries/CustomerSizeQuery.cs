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
    public class CustomerSizeQuery
    {
        CustomerSizeRepository repository;



        public CustomerSizeQuery(int tenant)
        {
            repository = new CustomerSizeRepository(tenant);
        }

        public CustomerSizeQuery(CustomerSizeRepository CustomerSizeRepository)
        {
            repository = CustomerSizeRepository;
        }

        public CustomerSizePM GetSinglePM(string id, int tenant)
        {
            if (!string.IsNullOrEmpty(id))
            {
                string entityName = "CustomerSizePM" + id + tenant;
                CustomerSizePM entity;
                if (HttpContext.Current != null)
                {
                    if (CacheManager.CacheWrapper.Get(entityName) == null)
                    {
                        var CustomerSizes = (from a in repository.context.CustomerSizes
                                             where a.Tenant == tenant
                                             select new CustomerSizePM()
                                             {
                                                 Name = a.Name,
                                                 Id = a.Id,
                                                 Tenant = a.Tenant,
                                                 SearchFields = a.SearchFields,
                                                 Order = a.OrderNumber,
                                                 InActive = a.InActive,
                                                 Code = a.Code,
                                             });

                        foreach (var c in CustomerSizes)
                        {
                            string cname = "CustomerSizePM" + c.Id + c.Tenant;

                            if (CacheManager.CacheWrapper.Get(cname) == null)
                            {
                                CacheManager.CacheWrapper.Insert(cname, c, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                            }
                        }
                        entity = (CustomerSizePM)CacheManager.CacheWrapper.Get(entityName);

                    }
                    else
                    {
                        entity = (CustomerSizePM)CacheManager.CacheWrapper.Get(entityName);
                    }
                }
                else
                {
                    entity = (from a in repository.context.CustomerSizes
                              where a.Tenant == tenant && a.Id == id
                              select new CustomerSizePM()
                              {
                                  Name = a.Name,
                                  Id = a.Id,
                                  Tenant = a.Tenant,
                                  SearchFields = a.SearchFields,
                                  Order = a.OrderNumber,
                                  InActive = a.InActive,
                                  Code = a.Code,
                              }).FirstOrDefault();
                }

                return entity;
            }
            return null;
        }

        public CustomerSizePM GetSinglePMByCode(string code, int tenant)
        {
            return (from a in repository.context.CustomerSizes
                    where a.Tenant == tenant
                    && a.Code == code
                    select new CustomerSizePM()
                    {
                        Name = a.Name,
                        Id = a.Id,
                        Tenant = a.Tenant,
                        SearchFields = a.SearchFields,
                        Order = a.OrderNumber,
                        InActive = a.InActive,
                        Code = a.Code,
                    }).FirstOrDefault();
        }

        public IQueryable<CustomerSizePM> GetCustomerSizePMsByTenant(int tenant)
        {
            IQueryable<CustomerSizePM> CustomerSizes = from a in repository.context.CustomerSizes
                                                       where a.Tenant == tenant
                                                       select new CustomerSizePM()
                                                       {
                                                           Name = a.Name,
                                                           Id = a.Id,
                                                           Tenant = a.Tenant,
                                                           SearchFields = a.SearchFields,
                                                           Order = a.OrderNumber,
                                                           InActive = a.InActive,
                                                           Code = a.Code,
                                                       };
            return CustomerSizes;
        }

        public IQueryable<CustomerSizeList> GetIQueryableEntityList(IQueryable<CustomerSize> iQueryable)
        {
            IQueryable<CustomerSizeList> result = from a in iQueryable
                                                  select new CustomerSizeList()
                                                  {
                                                      Name = a.Name,
                                                      Id = a.Id,
                                                      Tenant = a.Tenant,
                                                      SearchFields = a.SearchFields,
                                                      Order = a.OrderNumber,
                                                      InActive = a.InActive,
                                                      Code = a.Code,
                                                  };
            return result;
        }

        public CustomerSize GetFirstCustomerSizeForTenant(int tenant)
        {
            return (from a in repository.context.CustomerSizes
                    where a.Tenant == tenant
                    select a).FirstOrDefault();
        }
    }
}
