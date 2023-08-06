using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.DomainServices.Server;
using System.Web;

using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class TenantRepository:IRepository<Tenant>
    {
        ICommonDataContext commonDataContext;

        public TenantRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public TenantRepository()
        {
            commonDataContext = new CommonDataContext();
        }

        public TenantRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public IQueryable<Tenant> GetTenants()
        {
            return this.context.Tenants.Include("Address").Include("PaymentTerm").Include("OtherChargesCurrency").Include("QuoteSaleCurrency").Include("AgentCard").Include("Currency").Include("ProfitCurrency").Include("FreightCurrency").Include("PasswordPolicy").Include("Address.Country");
        }

        public List<int> GetAccountingActivatedTenants()
        {
            return this.context.Tenants.Where(r => r.AccountingActivated).Select(r=>r.Id).ToList();
        }

        public static Tenant GetSingleTenant(int id,bool getFromCache)
        {
            string entityName = "Tenant" + id ;
            Tenant entity;
            if (getFromCache)
            {
             
                    if (CacheManager.CacheWrapper.Get(entityName) == null)
                    {
                        ICommonDataContext context = CommonDataContext.GetContext(id);
                        entity = (from a in context.Tenants.Include("Address").Include("PaymentTerm").Include("OtherChargesCurrency").Include("QuoteSaleCurrency").Include("AgentCard").Include("Currency").Include("ProfitCurrency").Include("FreightCurrency").Include("PasswordPolicy").Include("Address.Country") where a.Id == id select a).FirstOrDefault();

                        if (CacheManager.CacheWrapper.Get(entityName) == null)
                        {
                            if (entity != null)
                            {
                                CacheManager.CacheWrapper.Insert(entityName, entity, null, DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                            }
                        }
                    }
                    else
                    {
                        entity = (Tenant)CacheManager.CacheWrapper.Get(entityName);
                    }
                
          
            }
            else 
            {
                ICommonDataContext context = CommonDataContext.GetContext(id);
                entity = (from a in context.Tenants.Include("Address").Include("PaymentTerm").Include("OtherChargesCurrency").Include("QuoteSaleCurrency").Include("AgentCard").Include("Currency").Include("ProfitCurrency").Include("FreightCurrency").Include("PasswordPolicy").Include("Address.Country") where a.Id == id select a).FirstOrDefault();
            }
            return entity;
        }


        public int GetTenantEmailSendingQuota(int id)
        {
            return (from record in context.Tenants where record.Id == id  select record.TenantEmailSendingQuota).FirstOrDefault();
        }
        public bool GetTenantAccountingActivated(int id)
        {
            return (from record in context.Tenants where record.Id == id select record.AccountingActivated).FirstOrDefault();
        }
        public  Tenant GetSingleTenantByIdAndTenant(int id, bool getFromCache)
        {
            string entityName = "Tenant" + id;
            Tenant entity;
            if (getFromCache)
            {
               
                    if (CacheManager.CacheWrapper.Get(entityName) == null)
                    {
                     
                        entity = (from a in context.Tenants.Include("Address").Include("PaymentTerm").Include("OtherChargesCurrency").Include("QuoteSaleCurrency").Include("AgentCard").Include("Currency").Include("ProfitCurrency").Include("FreightCurrency").Include("PasswordPolicy").Include("Address.Country") where a.Id == id select a).FirstOrDefault();

                        if (CacheManager.CacheWrapper.Get(entityName) == null)
                        {
                            if (entity != null)
                            {
                                CacheManager.CacheWrapper.Insert(entityName, entity, null, DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                            }
                        }
                    }
                    else
                    {
                        entity = (Tenant)CacheManager.CacheWrapper.Get(entityName);
                    }
                
           
            }
            else
            {
             
                entity = (from a in context.Tenants.Include("Address").Include("PaymentTerm").Include("OtherChargesCurrency").Include("QuoteSaleCurrency").Include("AgentCard").Include("Currency").Include("ProfitCurrency").Include("FreightCurrency").Include("PasswordPolicy").Include("Address.Country") where a.Id == id select a).FirstOrDefault();
            }
            return entity;
        }

        public  Tenant GetSingleTenant(int id)
        {
            Tenant entity =  context.Tenants
                                    .Include("PaymentTerm")
                                    .Include("OtherChargesCurrency")
                                    .Include("QuoteSaleCurrency")
                                    .Include("AgentCard")
                                    .Include("Currency")
                                    .Include("ProfitCurrency")
                                    .Include("FreightCurrency")
                                    .Include("PasswordPolicy")
                                    .Include("Address.Country")
                                    .Include("Address.State")
                                    .Include("CustomerCard")
                                    .FirstOrDefault(a => a.Id == id);
            return entity;
        }


        public Tenant GetSingleTenantWithOutIncluded(int id)
        {
            Tenant entity = context.Tenants
                                   .Include("Address.Country")
                                   .Include("Address.State")
                                   .FirstOrDefault(a => a.Id == id);
            return entity;
        }



        public Tenant GetSingleTenantOnly(int id)
        {
            Tenant entity = (from a in context.Tenants where a.Id == id select a).FirstOrDefault();
            return entity;
        }

        public Tenant GetSingleByTenant(int id)
        {
            Tenant entity = (from a in context.Tenants.Include("CustomerCard") where a.Id == id select a).FirstOrDefault();
            return entity;
        }

        [Invoke]
        public int GetTenantsCount()
        {
            return this.context.Tenants.Count();
        }

        public bool CheckIfDatabaseBackupBuilt(int id)
        {
            return (from a in context.Tenants
                    where a.Id == id && a.IsDataBackupBuilt
                    select a).Any();
        }
         
        public void Add(Tenant entity)
        {
            this.context.Tenants.Add(entity);
        }

        public void Remove(Tenant entity)
        {
            try
            {
                this.context.Tenants.Attach(entity);
            }
            catch { }
            this.context.Tenants.Remove(entity);
        }

        public void Update(Tenant entity)
        {
            try
            {
                this.context.Tenants.Attach(entity);
            }
            catch { }
            this.context.SetAsModified(entity);
        }

        public List<Tenant> All()
        {
            return this.context.Tenants.ToList<Tenant>();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            this.context.SaveChanges();
        }


        public List<Tenant> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public Tenant GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public List<Tenant>  GetTenantListByListIds(List<string> ids)
        {

            return (from d in this.context.Tenants
                    where ids.Contains(d.Id.ToString())
                    select d).ToList();
      
        }

        public string GetTenantVatNumberOnly(int id)
        {
            return (from a in context.Tenants where a.Id == id select a.VatNumber).FirstOrDefault();
        }

        public bool TenantExist(int id)
        {
            return context.Tenants.Any(a => a.Id == id);
        }

        public string GetLocalCurrencyFromTenant(int id)
        {
            return (from a in context.Tenants where a.Id == id select a.CurrencyId).FirstOrDefault();
        }
    }
}
