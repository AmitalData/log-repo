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
    public class CustomerStatusQuery
    {
        CustomerStatusRepository repository;



        public CustomerStatusQuery(int tenant)
        {
            repository = new CustomerStatusRepository(tenant);
        }

        public CustomerStatusQuery(CustomerStatusRepository repository)
        {
            this.repository = repository;
        }

        public CustomerStatusPM GetSinglePM(string code)
        {
            string entityName = "CustomerStatusPM" + code;
            CustomerStatusPM entity;

            if (HttpContext.Current != null)
            {
                if (CacheManager.CacheWrapper.Get(entityName) == null)
                {
                    var entitystatuses = (from a in repository.context.CustomerStatus

                                          select new CustomerStatusPM()
                                          {
                                              Code = a.Code,
                                              Name = a.Name,
                                              SearchFields = a.SearchFields,

                                          });
                    foreach (var s in entitystatuses)
                    {
                        string name = "CustomerStatusPM" + s.Code;
                        if (CacheManager.CacheWrapper.Get(name) == null)
                        {
                            CacheManager.CacheWrapper.Insert(name, s, null, DateTime.UtcNow.AddHours(30), TimeSpan.Zero);
                        }
                    }
                    entity = (CustomerStatusPM)CacheManager.CacheWrapper.Get(entityName);
                }
                else
                {
                    entity = (CustomerStatusPM)CacheManager.CacheWrapper.Get(entityName);
                }
            }
            else
            {
                entity = (from a in repository.context.CustomerStatus
                          where a.Code == code
                          select new CustomerStatusPM()
                          {
                              Code = a.Code,
                              Name = a.Name,
                              SearchFields = a.SearchFields,

                          }).FirstOrDefault();
            }
            return entity;
        }

        public IQueryable<CustomerStatusPM> GetCustomerStatusPMs()
        {
            var query = from a in repository.context.CustomerStatus
                        select new CustomerStatusPM()
                        {
                            Code = a.Code,
                            Name = a.Name,
                            SearchFields = a.SearchFields,
                        };

            return query;
        }

        public IQueryable<CustomerStatusList> GetIQueryableEntityList(IQueryable<CustomerStatus> iQueryable)
        {
            IQueryable<CustomerStatusList> result = from entity in iQueryable
                                                 select new CustomerStatusList()
                                                 {
                                                     SearchFields = entity.SearchFields,
                                                     Name = entity.Name,
                                                     Code = entity.Code,
                                                 };
            return result;
        }
    }
}