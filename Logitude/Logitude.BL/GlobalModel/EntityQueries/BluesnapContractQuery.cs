using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.BL.GlobalModel.EntityLists;
using Logitude.BL.GlobalModel.EntityPMs;

namespace Logitude.BL.GlobalModel.EntityQueries
{
    public class BluesnapContractQuery
    {
        BluesnapContractRepository repository;

        public BluesnapContractQuery()
        {
               repository = new BluesnapContractRepository(); 
        }
        
        public BluesnapContractQuery(BluesnapContractRepository bluesnapContractRepository)
        {
            repository = bluesnapContractRepository;
        }

        public BluesnapContractPM GetSinglePM(string Id, int tenant = 0)
        {
            BluesnapContractPM entity;
            entity = (from a in repository.context.BluesnapContracts.Include("BluesnapContractType")
                      where a.Id == Id
                      select new BluesnapContractPM()
                      {
                          Id=a.Id,
                          Code = a.Code,
                          SearchFields = a.SearchFields,
                          Name = a.Name,
                          ContractId = a.ContractId,
                          InActive = a.InActive,
                          Tenant = tenant,
                          BluesnapContractTypeCode=a.BluesnapContractTypeCode,
                          BluesnapContractTypeName=a.BluesnapContractType!=null?a.BluesnapContractType.Name:"",
                      }).FirstOrDefault();

            return entity;
        }

        public BluesnapContractPM GetSingleBluesnapContractPM(string Id, int tenant)
        {
            if (!string.IsNullOrEmpty(Id))
            {
                string entityName = "BluesnapContractPM" + Id + tenant;
                BluesnapContractPM entity;
                if (HttpContext.Current != null)
                {
                    if (CacheManager.CacheWrapper.Get(entityName) == null)
                    {
                        var entitystatuses = (from a in repository.context.BluesnapContracts.Include("BluesnapContractType")

                                              select new BluesnapContractPM()
                                              {
                                                  Id=a.Id,
                                                  Code = a.Code,
                                                  SearchFields = a.SearchFields,
                                                  Name = a.Name,
                                                  ContractId = a.ContractId,
                                                  InActive = a.InActive,
                                                  BluesnapContractTypeCode = a.BluesnapContractTypeCode,
                                                  BluesnapContractTypeName = a.BluesnapContractType != null ? a.BluesnapContractType.Name : "",
                                              });
                        foreach (var s in entitystatuses)
                        {
                            string name = "BluesnapContractPM" + s.Id + tenant;
                            if (CacheManager.CacheWrapper.Get(name) == null)
                            {
                                CacheManager.CacheWrapper.Insert(name, s, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                            }
                        }
                        entity = (BluesnapContractPM)CacheManager.CacheWrapper.Get(entityName);
                    }
                    else
                    {
                        entity = (BluesnapContractPM)CacheManager.CacheWrapper.Get(entityName);
                    }
                }
                else
                {
                    entity = (from a in repository.context.BluesnapContracts.Include("BluesnapContractType")
                              where a.Id == Id
                              select new BluesnapContractPM()
                              {
                                  Code = a.Code,
                                  SearchFields = a.SearchFields,
                                  Name = a.Name,
                                  ContractId = a.ContractId,
                                  InActive = a.InActive,
                                  BluesnapContractTypeCode = a.BluesnapContractTypeCode,
                                  BluesnapContractTypeName = a.BluesnapContractType != null ? a.BluesnapContractType.Name : "",
                              }).FirstOrDefault();
                }

                return entity;
            }
            return null;
        }

        public IQueryable<BluesnapContractList> GetIQueryableEntityList(IQueryable<BluesnapContract> iQueryable)
        {
            IQueryable<BluesnapContractList> result = from a in iQueryable.Include("BluesnapContractType")
                                                      select new BluesnapContractList()
                                                          {
                                                          Id=a.Id,
                                                              Code = a.Code,
                                                              SearchFields = a.SearchFields,
                                                              Name = a.Name,
                                                              ContractId = a.ContractId,
                                                              InActive = a.InActive,
                                                          BluesnapContractTypeCode = a.BluesnapContractTypeCode,
                                                          BluesnapContractTypeName = a.BluesnapContractType != null ? a.BluesnapContractType.Name : "",
                                                      };
            return result;
        }
    }
}
