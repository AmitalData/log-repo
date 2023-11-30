using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class BranchRepository:IRepository<Branch>
    {
        ICommonDataContext commonDataContext;

        public BranchRepository()
        {
            commonDataContext = new CommonDataContext();
        }

        public BranchRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public BranchRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public IQueryable<Branch> GetBranches(int tenant)
        {
            return (from record in context.Branches where record.Tenant == tenant select record);
        }
        public Branch GetSingleBranch(string id, int tenant)
        {
                string key = $"GetSingleBranch({id}, {tenant})";
                return Simplog.Server.Infrastructure.Helpers.CacheManager.GetOrInsertNewObject<Branch>(key, () =>
                {
                    return GetSingleBranchReal(id, tenant);
                });


        }
        private Branch GetSingleBranchReal(string id, int tenant)
        {
            return (from record in context.Branches where record.Id == id && record.Tenant == tenant select record).FirstOrDefault();
        }

        public Branch GetSingleBranchByCode(string code, int tenant)
        {
            return (from record in context.Branches where record.Code == code && record.Tenant == tenant select record).FirstOrDefault();
        }
        
        public void Add(Branch entity)
        {
            this.context.Branches.Add(entity);
        }

        public void Remove(Branch entity)
        {
            try
            {
                this.context.Branches.Attach(entity);
            }
            catch { }
            this.context.Branches.Remove(entity);

        }

        public void Update(Branch entity)
        {
            try
            {
                this.context.Branches.Attach(entity);
            }
            catch { }
            this.context.SetAsModified(entity);

        }

        public List<Branch> All()
        {
            return this.context.Branches.ToList<Branch>();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            this.context.SaveChanges();
        }

     

        public Branch GetBranchByName(string name, int tenant)
        {
            string nameNew = "";
            nameNew = name;

            var query = (from a in context.Branches
                         where a.Tenant == tenant && a.EnglishName == name
                         select  a).FirstOrDefault();
            return query;
            //IQueryable<BranchPM> query2 = null;
            //if (!string.IsNullOrEmpty(name))
            //{
            //    query2 = query.Where(d => d.EnglishName.ToUpper().StartsWith(name.ToUpper()) && d.Tenant == tenant);
            //}
            //if (query2 != null)
            //{
            //    return query2;
            //    //query = query.Where(d => d.EnglishName.ToUpper().StartsWith(name.ToUpper()) && d.Tenant == tenant);
            //}
            //else
            //    return query;
        }

        public static Branch GetSingleBranch(string id, int tenant, bool getFromCache)
        {
            string entityName = "Branch" + id + tenant;
            Branch entity;
            if (getFromCache)
            {
               
                    if (CacheManager.CacheWrapper.Get(entityName) == null)
                    {
                        ICommonDataContext context = CommonDataContext.GetContext(tenant);
                        var currencies = (from a in context.Branches
                                          where a.Tenant == tenant
                                          select a);

                        foreach (var c in currencies)
                        {
                            string name = "Branch" + c.Id + tenant;
                            if (CacheManager.CacheWrapper.Get(name) == null)
                            {
                                CacheManager.CacheWrapper.Insert(name, c, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                            }
                        }
                        entity = (Branch)CacheManager.CacheWrapper.Get(entityName);
                    }
                    else
                    {
                        entity = (Branch)CacheManager.CacheWrapper.Get(entityName);
                    }
                
             
            }
            else
            {
                ICommonDataContext context = CommonDataContext.GetContext(tenant);
                entity = (from record in context.Branches where record.Id == id && record.Tenant == tenant select record).FirstOrDefault();
            }
            return entity;
        }



        public List<Branch> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public Branch GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public Branch GetBranchByCounterCode(string counterCode, int tenant)
        {
            return (from branch in context.Branches where branch.CounterCode == counterCode && branch.Tenant == tenant select branch)?.FirstOrDefault();
        }
    }
}
