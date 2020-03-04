using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class ChargesTypeRepository:IRepository<ChargesType>
    {
        ICommonDataContext commonDataContext;

        public ChargesTypeRepository()
        {
            commonDataContext = new CommonDataContext();
        }

        public ChargesTypeRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public IQueryable<ChargesType> GetChargesTypes(int tenant)
        {
            return (from record in context.ChargesTypes.Include("Measurement").Include("ContainerMeasurement").Include("ReceivableAccount").Include("PayableAccount").Include("PayablesDefaultCurrency").Include("ReceivablesDefaultCurrency") where record.Tenant == tenant select record);
        }

        public IQueryable<ChargesType> GetQuoteDefaultChargesTypes(int tenant)
        {
            return (from d in context.ChargesTypes.Include("Measurement").Include("ContainerMeasurement").Include("ReceivableAccount").Include("PayableAccount")
                    where d.Tenant == tenant && d.IsAutoDisplayInQuote
                    select d);
        }

        public ChargesTypeRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public static ChargesType GetSingleChargesType(string id, int tenant, bool getFromCache)
        {
            string entityName = "ChargesType" + id + tenant;
            ChargesType entity;
            if (getFromCache)
            {
               
                    if (CacheManager.CacheWrapper.Get(entityName) == null)
                    {
                        ICommonDataContext context = CommonDataContext.GetContext(tenant);
                        var currencies = (from a in context.ChargesTypes.Include("Measurement").Include("ContainerMeasurement").Include("ReceivableAccount").Include("PayableAccount")
                                          where a.Tenant == tenant
                                          select a);

                        foreach (var c in currencies)
                        {
                            string name = "ChargesType" + c.Id + tenant;
                            if (CacheManager.CacheWrapper.Get(name) == null)
                            {
                                CacheManager.CacheWrapper.Insert(name, c, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                            }
                        }
                        entity = (ChargesType)CacheManager.CacheWrapper.Get(entityName);
                    }
                    else
                    {
                        entity = (ChargesType)CacheManager.CacheWrapper.Get(entityName);
                    }
                
             
            }
            else
            {
                ICommonDataContext context = CommonDataContext.GetContext(tenant);
                entity = (from record in context.ChargesTypes.Include("Measurement").Include("ContainerMeasurement").Include("ReceivableAccount").Include("PayableAccount") where record.Id == id && record.Tenant == tenant select record).FirstOrDefault();
            }
            return entity;           
        }
        
        public ChargesType GetSingleChargesType(string id, int tenant)
        {

            ChargesType entity = (from a in context.ChargesTypes.Include("Measurement").Include("ContainerMeasurement").Include("ReceivableAccount").Include("PayableAccount").Include("ChargesGroup")
                                  where a.Tenant == tenant && a.Id == id
                                          select a).FirstOrDefault();
            return entity;
        }

        public ChargesType GetSingleChargesTypeByCode(string code, int tenant)
        {

            ChargesType entity = (from a in context.ChargesTypes.Include("Measurement").Include("ContainerMeasurement").Include("ReceivableAccount").Include("PayableAccount")
                                  where a.Tenant == tenant && a.Code == code
                                  select a).FirstOrDefault();
            return entity;
        }
        
        public void Add(ChargesType entity)
        {
            context.ChargesTypes.Add(entity);
        }

        public void Remove(ChargesType entity)
        {
            context.ChargesTypes.Attach(entity);
            context.ChargesTypes.Remove(entity);
        }

        public void Update(ChargesType entity)
        {
            context.ChargesTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ChargesType> All()
        {
            return context.ChargesTypes.ToList();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<ChargesType> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public ChargesType GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }
    }
}
