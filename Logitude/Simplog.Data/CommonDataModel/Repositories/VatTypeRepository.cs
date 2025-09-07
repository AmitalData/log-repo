using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class VatTypeRepository:IRepository<VatType>
    {
        ICommonDataContext commonDataContext;

        public VatTypeRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }
        public VatTypeRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public IQueryable<VatType> GetVatTypes(int tenant)
        {
            return (from record in context.VatTypes where record.Tenant == tenant select record);
        }

        public VatType GetSingleVatType(string id, int tenant)
        {
            return (from record in context.VatTypes where record.Id == id && record.Tenant == tenant select record).FirstOrDefault(); 
        }

        public VatType GetSingleVatTypeByCode(string code, int tenant)
        {
            return (from record in context.VatTypes where record.Code == code && record.Tenant == tenant select record).FirstOrDefault();
        }

        public static VatType GetSingleVatType(string id, int tenant, bool getFromCache)
        {
            string entityName = "VatType" + id + tenant;
            VatType entity;
            if (getFromCache)
            {
             
                    if (CacheManager.CacheWrapper.Get(entityName) == null)
                    {
                        ICommonDataContext context = CommonDataContext.GetContext(tenant);
                        var vatTypes = (from a in context.VatTypes
                                          where a.Tenant == tenant
                                          select a);

                        foreach (var c in vatTypes)
                        {
                            string name = "VatType" + c.Id + tenant;
                            if (CacheManager.CacheWrapper.Get(name) == null)
                            {
                                CacheManager.CacheWrapper.Insert(name, c, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                            }
                        }
                        entity = (VatType)CacheManager.CacheWrapper.Get(entityName);                    
                    }
                    else
                    {
                        entity = (VatType)CacheManager.CacheWrapper.Get(entityName);
                    }
                
            
            }
            else
            {
                ICommonDataContext context = CommonDataContext.GetContext(tenant);
                entity = (from record in context.VatTypes where record.Id == id && record.Tenant == tenant select record).FirstOrDefault();
            }
            return entity;            
        }

        public void Add(VatType entity)
        {
            context.VatTypes.Add(entity);
        }

        public void Remove(VatType entity)
        {
            context.VatTypes.Attach(entity);
            context.VatTypes.Remove(entity);
        }

        public void Update(VatType entity)
        {
            context.VatTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<VatType> All()
        {
            return context.VatTypes.ToList();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<VatType> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public VatType GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }
    }
}
