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
    public class BranchQuery
    {
        BranchRepository repository;
        public BranchQuery()
        {
               repository = new BranchRepository(); 
        }
        public BranchQuery(int tenant)
        {
            repository = new BranchRepository(tenant);
        }
        public BranchQuery(BranchRepository branchRepository)
        {
            repository = branchRepository;
        }
      
        public BranchPM GetSinglePM(string id, int tenant,bool getFromCache = true)
        {
            if (!string.IsNullOrEmpty(id))
            {
                string entityName = "BranchPM" + id + tenant;
                BranchPM entity;
                if (getFromCache)
                {
                    if (HttpContext.Current != null)
                    {
                        if (CacheManager.CacheWrapper.Get(entityName) == null)
                        {
							var branches = (from a in repository.context.Branches
											where a.Tenant == tenant
											select new BranchPM()
											{
												EnglishName = a.EnglishName,
												Id = a.Id,
												InActive = a.InActive,
												LocalName = a.LocalName,
												Notes = a.Notes,
												Tenant = a.Tenant,
												SearchFields = a.SearchFields,
												ComputedLocalName = string.IsNullOrEmpty(a.LocalName) ? a.EnglishName : a.LocalName,
												ExternalId = a.ExternalId,
												AddressId = a.AddressId,
												Signature = a.Signature,
												Code = a.Code,
												INTTRAId = a.INTTRAId,
												INTTRAAlias = a.INTTRAAlias,
												INTTRAContactId = a.INTTRAContactId,
												CounterCode = a.CounterCode,
											});

                            foreach (var c in branches)
                            {
                                string cname = "BranchPM" + c.Id + c.Tenant;

                                if (CacheManager.CacheWrapper.Get(cname) == null)
                                {
                                    CacheManager.CacheWrapper.Insert(cname, c, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                                }
                            }
                            entity = (BranchPM)CacheManager.CacheWrapper.Get(entityName);

                        }
                        else
                        {
                            entity = (BranchPM)CacheManager.CacheWrapper.Get(entityName);
                        }
                    }
                    else
                    {
                        entity = (from a in repository.context.Branches
                                  where a.Tenant == tenant && a.Id == id
                                  select new BranchPM()
                                  {
                                      EnglishName = a.EnglishName,
                                      Id = a.Id,
                                      InActive = a.InActive,
                                      LocalName = a.LocalName,
                                      Notes = a.Notes,
                                      Tenant = a.Tenant,
                                      SearchFields = a.SearchFields,
                                      ComputedLocalName = string.IsNullOrEmpty(a.LocalName) ? a.EnglishName : a.LocalName,
                                      ExternalId = a.ExternalId,
                                      AddressId = a.AddressId,
                                      Signature = a.Signature,
                                      Code = a.Code,
                                      INTTRAId = a.INTTRAId,
                                      INTTRAAlias = a.INTTRAAlias,
                                      INTTRAContactId = a.INTTRAContactId,
									  CounterCode = a.CounterCode,
								  }).FirstOrDefault();
                    }
                }
                else
                {
                    entity = (from a in repository.context.Branches
                              where a.Tenant == tenant && a.Id == id
                              select new BranchPM()
                              {
                                  EnglishName = a.EnglishName,
                                  Id = a.Id,
                                  InActive = a.InActive,
                                  LocalName = a.LocalName,
                                  Notes = a.Notes,
                                  Tenant = a.Tenant,
                                  SearchFields = a.SearchFields,
                                  ComputedLocalName = string.IsNullOrEmpty(a.LocalName) ? a.EnglishName : a.LocalName,
                                  ExternalId = a.ExternalId,
                                  AddressId = a.AddressId,
                                  Signature = a.Signature,
                                  Code = a.Code,
                                  INTTRAId = a.INTTRAId,
                                  INTTRAAlias = a.INTTRAAlias,
                                  INTTRAContactId = a.INTTRAContactId,
								  CounterCode = a.CounterCode,
							  }).FirstOrDefault();
                }

                BranchPM securedPm = new BranchPM();
                SecuredMapping.GetMappedPM(entity, securedPm, "Branch", tenant);

                return securedPm;
            }
            return null;
        }
        public BranchPM GetSinglePMByCode(string Code, int tenant, bool getFromCache = true)
        {
            if (!string.IsNullOrEmpty(Code))
            {
                string entityName = "BranchPM" + Code + tenant;
                BranchPM entity;
                if (getFromCache)
                {
                    if (HttpContext.Current != null)
                    {
                        if (CacheManager.CacheWrapper.Get(entityName) == null)
                        {
                            var branches = (from a in repository.context.Branches
                                            where a.Tenant == tenant
                                            && a.Code == Code
                                            select new BranchPM()
                                            {
                                                EnglishName = a.EnglishName,
                                                Id = a.Id,
                                                InActive = a.InActive,
                                                LocalName = a.LocalName,
                                                Notes = a.Notes,
                                                Tenant = a.Tenant,
                                                SearchFields = a.SearchFields,
                                                ComputedLocalName = string.IsNullOrEmpty(a.LocalName) ? a.EnglishName : a.LocalName,
                                                ExternalId = a.ExternalId,
                                                AddressId = a.AddressId,
                                                Signature = a.Signature,
                                                Code = a.Code,
                                                INTTRAId = a.INTTRAId,
                                                INTTRAAlias = a.INTTRAAlias,
                                                INTTRAContactId = a.INTTRAContactId,
												CounterCode = a.CounterCode,
											});

                            foreach (var c in branches)
                            {
                                string cname = "BranchPM" + c.Code + c.Tenant;

                                if (CacheManager.CacheWrapper.Get(cname) == null)
                                {
                                    CacheManager.CacheWrapper.Insert(cname, c, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                                }
                            }
                            entity = (BranchPM)CacheManager.CacheWrapper.Get(entityName);

                        }
                        else
                        {
                            entity = (BranchPM)CacheManager.CacheWrapper.Get(entityName);
                        }
                    }
                    else
                    {
                        entity = (from a in repository.context.Branches
                                  where a.Tenant == tenant && a.Code == Code
                                  select new BranchPM()
                                  {
                                      EnglishName = a.EnglishName,
                                      Id = a.Id,
                                      InActive = a.InActive,
                                      LocalName = a.LocalName,
                                      Notes = a.Notes,
                                      Tenant = a.Tenant,
                                      SearchFields = a.SearchFields,
                                      ComputedLocalName = string.IsNullOrEmpty(a.LocalName) ? a.EnglishName : a.LocalName,
                                      ExternalId = a.ExternalId,
                                      AddressId = a.AddressId,
                                      Signature = a.Signature,
                                      Code = a.Code,
                                      INTTRAId = a.INTTRAId,
                                      INTTRAAlias = a.INTTRAAlias,
                                      INTTRAContactId = a.INTTRAContactId,
									  CounterCode = a.CounterCode,
								  }).FirstOrDefault();
                    }
                }
                else
                {
                    entity = (from a in repository.context.Branches
                              where a.Tenant == tenant && a.Code == Code
                              select new BranchPM()
                              {
                                  EnglishName = a.EnglishName,
                                  Id = a.Id,
                                  InActive = a.InActive,
                                  LocalName = a.LocalName,
                                  Notes = a.Notes,
                                  Tenant = a.Tenant,
                                  SearchFields = a.SearchFields,
                                  ComputedLocalName = string.IsNullOrEmpty(a.LocalName) ? a.EnglishName : a.LocalName,
                                  ExternalId = a.ExternalId,
                                  AddressId = a.AddressId,
                                  Signature = a.Signature,
                                  Code = a.Code,
                                  INTTRAId = a.INTTRAId,
                                  INTTRAAlias = a.INTTRAAlias,
                                  INTTRAContactId = a.INTTRAContactId,
								  CounterCode = a.CounterCode,
							  }).FirstOrDefault();
                }
                BranchPM securedPm = new BranchPM();
                SecuredMapping.GetMappedPM(entity, securedPm, "Branch", tenant);

                return securedPm;
            }
            return null;
        }       
        public IQueryable<BranchPM> GetBranchPMsByTenant(int tenant)
        {
            IQueryable<BranchPM> branches = from a in repository.context.Branches
                                            where a.Tenant == tenant
                                            select new BranchPM()
                                            {
                                                EnglishName = a.EnglishName,
                                                Id = a.Id,
                                                InActive = a.InActive,
                                                LocalName = a.LocalName,
                                                Notes = a.Notes,
                                                Tenant = a.Tenant,
                                                SearchFields = a.SearchFields,
                                                ComputedLocalName = string.IsNullOrEmpty(a.LocalName) ? a.EnglishName : a.LocalName,
                                                ExternalId = a.ExternalId,
                                                AddressId = a.AddressId,
                                                Signature = a.Signature,
                                                Code = a.Code,
                                                INTTRAId = a.INTTRAId,
                                                INTTRAAlias = a.INTTRAAlias,
                                                INTTRAContactId = a.INTTRAContactId,
												CounterCode = a.CounterCode,
											};
            return branches;
        }
        public BranchPM GetBranchByName(string name, int tenant)
        {
            string nameNew = "";
            nameNew = name;

            var query = (from a in repository.context.Branches
                         where a.Tenant == tenant && a.EnglishName == name
                         select new BranchPM()
                         {
                             EnglishName = a.EnglishName,
                             Id = a.Id,
                             InActive = a.InActive,
                             LocalName = a.LocalName,
                             Notes = a.Notes,
                             Tenant = a.Tenant,
                             SearchFields = a.SearchFields,
                             ComputedLocalName = string.IsNullOrEmpty(a.LocalName) ? a.EnglishName : a.LocalName,
                             ExternalId = a.ExternalId,
                             AddressId = a.AddressId,
                             Signature = a.Signature,
                             Code = a.Code,
                             INTTRAId = a.INTTRAId,
                             INTTRAAlias = a.INTTRAAlias,
                             INTTRAContactId = a.INTTRAContactId,
							 CounterCode = a.CounterCode,
						 }).FirstOrDefault();
            return query;
        }
        public IQueryable<BranchList> GetIQueryableEntityList(IQueryable<Branch> iQueryable)
        {
            IQueryable<BranchList> result = from branch in iQueryable
                                            select new BranchList()
                                            {
                                                EnglishName = branch.EnglishName,
                                                LocalName = branch.LocalName,
                                                Notes = branch.Notes,
                                                InActive = branch.InActive,
                                                Id = branch.Id,
                                                Tenant = branch.Tenant,
                                                SearchFields = branch.SearchFields,
                                                ExternalId = branch.ExternalId,
                                                Signature = branch.Signature,
                                                Code = branch.Code,
                                                INTTRAId = branch.INTTRAId,
                                                INTTRAAlias = branch.INTTRAAlias,
                                                INTTRAContactId = branch.INTTRAContactId,
                                            };
            return result;
        }
        public Branch GetFirstBranchForTenant(int tenant)
        {
            return (from a in repository.context.Branches
                    where a.Tenant == tenant
                    select a).FirstOrDefault();
        }
    }
}
