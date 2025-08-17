using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class PackageTypeRepository:IRepository<PackageType>
    {
        ICommonDataContext commonDataContext;

        public PackageTypeRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }



        public PackageTypeRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public IQueryable<PackageType> GetPackageTypes(int tenant)
        {
            return (from record in context.PackageTypes.Include("Measurement") where record.Tenant == tenant select record);
        }

        public PackageType GetSinglePackageType(string id, int tenant)
        {
            return (from record in context.PackageTypes.Include("Measurement") where record.Id == id && record.Tenant == tenant select record).FirstOrDefault();
        }

        public static PackageType GetSinglePackageType(string id, int tenant, bool getFromCache)
        {
            string entityName = "PackageType" + id + tenant;
            PackageType entity;
            if (getFromCache)
            {
              
                    if (CacheManager.CacheWrapper.Get(entityName) == null)
                    {
                        ICommonDataContext context = CommonDataContext.GetContext(tenant);
                        var currencies = (from a in context.PackageTypes.Include("Measurement")
                                          where a.Tenant == tenant
                                          select a);

                        foreach (var c in currencies)
                        {
                            string name = "PackageType" + c.Id + tenant;
                            if (CacheManager.CacheWrapper.Get(name) == null)
                            {
                                CacheManager.CacheWrapper.Insert(name, c, null, DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                            }
                        }
                        entity = (PackageType)CacheManager.CacheWrapper.Get(entityName);
                    }
                    else
                    {
                        entity = (PackageType)CacheManager.CacheWrapper.Get(entityName);
                    }
                
             
            }
            else
            {
                ICommonDataContext context = CommonDataContext.GetContext(tenant);
                entity = (from record in context.PackageTypes.Include("Measurement") where record.Id == id && record.Tenant == tenant select record).FirstOrDefault();
            }
            return entity;
        }

        public  PackageType GetSinglePackageTypeByCode(string code, int tenant, bool getFromCache)
        {
            string entityName = "PackageType" + code + tenant;
            PackageType entity;
            if (getFromCache)
            {
             
                    if (CacheManager.CacheWrapper.Get(entityName) == null)
                    {
                        ICommonDataContext context = CommonDataContext.GetContext(tenant);
                        var currencies = (from a in context.PackageTypes.Include("Measurement")
                                          where a.Tenant == tenant
                                          select a);

                        foreach (var c in currencies)
                        {
                            string name = "PackageType" + c.Code + tenant;
                            if (CacheManager.CacheWrapper.Get(name) == null)
                            {
                                CacheManager.CacheWrapper.Insert(name, c, null, DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                            }
                        }
                        entity = (PackageType)CacheManager.CacheWrapper.Get(entityName);
                    }
                    else
                    {
                        entity = (PackageType)CacheManager.CacheWrapper.Get(entityName);
                    }
                
           
            }
            else
            {
                ICommonDataContext context = CommonDataContext.GetContext(tenant);
                entity = (from record in context.PackageTypes.Include("Measurement") where record.Code == code && record.Tenant == tenant select record).FirstOrDefault();
            }
            return entity;
        }

        public void Add(PackageType entity)
        {
            context.PackageTypes.Add(entity);
        }

        public void Remove(PackageType entity)
        {
            context.PackageTypes.Attach(entity);
            context.PackageTypes.Remove(entity);
        }

        public void Update(PackageType entity)
        {
            context.PackageTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<PackageType> All()
        {
            return context.PackageTypes.ToList();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<PackageType> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public PackageType GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }
    }
}