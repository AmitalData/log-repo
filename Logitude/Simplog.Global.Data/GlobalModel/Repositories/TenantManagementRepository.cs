using System.Collections.Generic;
using System.Linq;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Server.Infrastructure;
using System.Security.Cryptography;
using System;

namespace Simplog.Global.Data.GlobalModel.Repositories
{
    public class TenantManagementRepository : IRepository<TenantManagement>
    {
        IGlobalContext globalContext;

        public TenantManagementRepository(IGlobalContext context)
        {
            globalContext = context;
        }

        public TenantManagementRepository()
        {
            globalContext = GlobalContext.GetContext();
        }

        //public IQueryable<TenantManagement> GetAWBStockPrepaidTenants()
        //{
        //    return this.context.TenantManagements.Where(d => d.IsAWBStockPrepaid == true);
        //}

        public IQueryable<TenantManagement> GetTenants()
        {
            return this.context.TenantManagements.Include("GlobalTenant");
        }

        public IQueryable<TenantManagement> GetTenantManagements()
        {
            return this.context.TenantManagements.Include("GlobalTenant");
        }                      

        public IQueryable<TenantManagement> GetAllTenants()
        {
            return this.context.TenantManagements.Include("GlobalTenant");
        }

        public TenantManagement GetSingleTenantManagement(int id)
        {
            string entityName = "TenantManagement" + id;
            TenantManagement entity = (from a in context.TenantManagements.Include("GlobalTenant") where a.Id == id select a).FirstOrDefault(); ;

            if (CacheManager.CacheWrapper != null)
            {
                if (CacheManager.CacheWrapper.Get(entityName) == null && entity != null)
                {
                    CacheManager.CacheWrapper.Insert(entityName, entity, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                }
                else
                {
                    entity = (TenantManagement)CacheManager.CacheWrapper.Get(entityName);
                }
            }
            return entity;

            // return (from a in context.TenantManagements.Include("GlobalTenant") where a.Id == id select a).FirstOrDefault();
        }

        public TenantManagement GetSingleTenantManagementByBluesnapAccountId(string bluesnapaccountId)
        {
            string entityName = "TenantManagement" + bluesnapaccountId;
            IQueryable<TenantManagement> allTenantManagements = (from a in context.TenantManagements.Include("GlobalTenant")
                                                                 where a.BluesnapAccount == bluesnapaccountId
                                                                 select a);
            if (CacheManager.CacheWrapper != null)
            {
                if (CacheManager.CacheWrapper.Get(entityName) == null && allTenantManagements != null)
                {
                    CacheManager.CacheWrapper.Insert(entityName, allTenantManagements, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);                
                }
                else
                {
                    allTenantManagements = (IQueryable<TenantManagement>)CacheManager.CacheWrapper.Get(entityName);
                }
            }

            TenantManagement tenantManagement = null;
            //IQueryable<TenantManagement> allTenantManagements = (from a in context.TenantManagements.Include("GlobalTenant")
            //                            where a.BluesnapAccount == bluesnapaccountId
            //                            select a);
            var parentTenant = allTenantManagements.Where(a => a.IsParentTenant && a.NoPaymentForChildTenants).FirstOrDefault();

            if (parentTenant != null)
            {
                tenantManagement = parentTenant;
            }
            else
            {
                tenantManagement = (from a in allTenantManagements.Include("GlobalTenant")
                                    where a.BluesnapAccount == bluesnapaccountId && a.GlobalTenant.IsActive
                                    select a).FirstOrDefault();
                if (tenantManagement == null)
                {
                    tenantManagement = (from a in allTenantManagements.Include("GlobalTenant")
                                        where a.BluesnapAccount == bluesnapaccountId && !a.GlobalTenant.IsActive
                                        select a).FirstOrDefault();
                }
            }

            return tenantManagement;
        }

        public List<TenantManagement> GetTenantManagementsForPackage(string packageCode)
        {
            string entityName = "TenantManagement" + packageCode;
            List<TenantManagement> entity = (from a in context.TenantManagements
                                            where a.PackageCode == packageCode
                                            select a).ToList();
            if (CacheManager.CacheWrapper != null)
            {
                if (CacheManager.CacheWrapper.Get(entityName) == null && entity != null)
                {
                    CacheManager.CacheWrapper.Insert(entityName, entity, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                }
                else
                {
                    entity = (List<TenantManagement>)CacheManager.CacheWrapper.Get(entityName);
                }
            }
            
            return entity;
            //return (from a in context.TenantManagements
            //        where a.PackageCode == packageCode
            //        select a).ToList();
        }

        public List<string> GetTenantManagementsIdsForPackage(string packageCode)
        {
            string entityName = "TenantManagement" + packageCode;
            List<string> entity = (from a in context.TenantManagements
                                   where a.PackageCode == packageCode
                                   select a.Id.ToString()).ToList(); 

            if (CacheManager.CacheWrapper != null)
            {
                if (CacheManager.CacheWrapper.Get(entityName) == null && entity != null)
                {
                    CacheManager.CacheWrapper.Insert(entityName, entity, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                }
                else
                {
                    entity = (List<string>)CacheManager.CacheWrapper.Get(entityName);
                }
            }
            return entity;
            //return  (from a in context.TenantManagements
            //        where a.PackageCode == packageCode
            //        select a.Id.ToString()).ToList();
        }


        public bool CheckDistributor(string distributorCode,int tenant)
        {
            bool isDistributor = (from a in context.TenantManagements
                                  where a.Id == tenant && a.DistributorCode == distributorCode
                                  select a).Any();
            return isDistributor;
        }

        public void Add(TenantManagement entity)
        {
            context.TenantManagements.Add(entity);
        }

        public void Remove(TenantManagement entity)
        {
            context.TenantManagements.Attach(entity);
            context.TenantManagements.Remove(entity);
        }

        public void Update(TenantManagement entity)
        {
            context.TenantManagements.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<TenantManagement> All()
        {
            return context.TenantManagements.ToList();
        }

        public IGlobalContext context
        {
            get {return globalContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<TenantManagement> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public TenantManagement GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public TenantManagement GetTenantByPIMA(string myPIMA)
        {
            return (from a in context.TenantManagements where a.PIMA == myPIMA select a).FirstOrDefault();
        }

        public TenantManagement GetTenantByTTY(string myTTY)
        {
            return (from a in context.TenantManagements where a.TTY == myTTY select a).FirstOrDefault();
        }

        public List<int> GetActiveTenantsIdsByDistributor(string distributorCode)
        {
            return (from a in context.TenantManagements.Include("GlobalTenant")
                    where a.GlobalTenant.IsActive && a.GlobalTenant.Version != -1 && a.DistributorCode == distributorCode
                    select a.Id).ToList();
        }

        public TenantManagement GetTenantManagementByConnectedArline(string airlineCode)
        {
            TenantManagement tenantManagement = (from a in context.TenantManagements.Include("GlobalTenant")
                    where a.TenantTypeCode == "AIR" && a.TenantConnectedToAirlineCode == airlineCode
                    select a).FirstOrDefault();

            return tenantManagement;
        }

        public IQueryable<TenantManagement> GetAirlineTenants()
        {
            return (from a in context.TenantManagements where a.TenantTypeCode == "AIR" select a);
        }

        public TenantManagement GetSingleTenantManagementPMBySupportEmail(string supportEmail)
        {
            return (from a in context.TenantManagements.Include("GlobalTenant")
                    where a.SupportEmail == supportEmail && a.SupportActivated == true
                    select a).FirstOrDefault();
        }

        public bool CheckSupportEmailTenantManagement(string supportDomain, int tenant)
        {
           bool isExist = false; 
           TenantManagement myTenant = (from a in context.TenantManagements.Include("GlobalTenant")
                                        where a.SupportDomain == supportDomain && a.Id != tenant && a.SupportActivated == true
                                        select a).FirstOrDefault();
           if (myTenant != null)
           {
               isExist = true;
           }

           return isExist ;
        }

        public Tuple<bool, int?> CheckSubDomainTenantManagement(string subDomain, int tenant)
        {
            bool isExist = false;
            TenantManagement myTenant = context.TenantManagements
                                               .FirstOrDefault(a => a.CustomerURL == subDomain 
                                                                    && a.Id != tenant);
            if (myTenant != null)
            {
                isExist = true;
            }

            return Tuple.Create(isExist, myTenant?.Id);
        }

        public TenantManagement GetSingleTenantManagementPMByListOfEmails(List<string> emails)
        {
            TenantManagement myTenant = new TenantManagement();
            List<TenantManagement> tenants = new List<TenantManagement>();
            if (emails.Count > 0)
            {
                tenants = (from a in context.TenantManagements.Include("GlobalTenant")
                            where  a.SupportActivated == true
                            select a).ToList();

                emails = emails.Select(a=>a.Split('@')[1].Trim()).ToList();
                myTenant = tenants.Where(a=>a.SupportDomain != null && emails.Contains(a.SupportDomain)).FirstOrDefault();
            }

            return myTenant;
        }

        public IQueryable<TenantManagement> GetParentTenantManagements()
        {
            return this.context.TenantManagements.Where(d => d.IsParentTenant);
        }

        public IQueryable<TenantManagement> GetChildTenantManagements(int parentTenantId)
        {
            return this.context.TenantManagements.Where(d => d.ParentTenantId == parentTenantId);
        }


        public IQueryable<TenantManagement> GetTenantManagementsByIds(List<int>Ids)
        {
            return this.context.TenantManagements.Include("GlobalTenant").Where(d => Ids.Contains(d.Id));
        }
         
        public int GetScheduledTasksLimitPerReport(int id)
        {
            return (from a in context.TenantManagements where a.Id == id select a.ScheduledTasksLimitPerReport).FirstOrDefault();
        }
		public TenantManagement GetTenantManagementByExportTenant(int exportTenant)
		{
			string entityName = "TenantManagementExportTenant" + exportTenant;
            TenantManagement entity = null;
            if (CacheManager.CacheWrapper != null)
            {
                if (CacheManager.CacheWrapper.Get(entityName) == null)
                {
                    entity = (from a in context.TenantManagements where a.ExportTenant == exportTenant select a).FirstOrDefault();
                    CacheManager.CacheWrapper.Insert(entityName, entity, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                }
                else
                {
                    entity = (TenantManagement)CacheManager.CacheWrapper.Get(entityName);
                }
            }
            else
            {
				entity = (from a in context.TenantManagements where a.ExportTenant == exportTenant select a).FirstOrDefault();
			}

			return entity;
		}		

	}
}