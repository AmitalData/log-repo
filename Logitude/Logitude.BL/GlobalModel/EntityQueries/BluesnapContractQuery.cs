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

        public BluesnapContractPM GetSinglePM(string code, int tenant = 0)
        {
            BluesnapContractPM entity;
            entity = (from a in repository.context.BluesnapContracts
                      where a.Code == code
                      select new BluesnapContractPM()
                      {
                          Code = a.Code,
                          SearchFields = a.SearchFields,
                          Name = a.Name,
                          ContractId = a.ContractId,
                          InActive = a.InActive,
                          Tenant = tenant,
                      }).FirstOrDefault();

            return entity;
        }

        public BluesnapContractPM GetSingleBluesnapContractPM(string code, int tenant)
        {
            if (!string.IsNullOrEmpty(code))
            {
                string entityName = "BluesnapContractPM" + code + tenant;
                BluesnapContractPM entity;
                if (HttpContext.Current != null)
                {
                    if (CacheManager.CacheWrapper.Get(entityName) == null)
                    {
                        var entitystatuses = (from a in repository.context.BluesnapContracts

                                              select new BluesnapContractPM()
                                              {
                                                  Code = a.Code,
                                                  SearchFields = a.SearchFields,
                                                  Name = a.Name,
                                                  ContractId = a.ContractId,
                                                  InActive = a.InActive,
                                              });
                        foreach (var s in entitystatuses)
                        {
                            string name = "BluesnapContractPM" + s.Code + tenant;
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
                    entity = (from a in repository.context.BluesnapContracts
                              where a.Code == code
                              select new BluesnapContractPM()
                              {
                                  Code = a.Code,
                                  SearchFields = a.SearchFields,
                                  Name = a.Name,
                                  ContractId = a.ContractId,
                                  InActive = a.InActive,
                              }).FirstOrDefault();
                }

                return entity;
            }
            return null;
        }

        public IQueryable<BluesnapContractList> GetIQueryableEntityList(IQueryable<BluesnapContract> iQueryable)
        {
            IQueryable<BluesnapContractList> result = from a in iQueryable
                                                      select new BluesnapContractList()
                                                          {
                                                              Code = a.Code,
                                                              SearchFields = a.SearchFields,
                                                              Name = a.Name,
                                                              ContractId = a.ContractId,
                                                              InActive = a.InActive,
                                                          };
            return result;
        }
    }
}
