using System.Collections.Generic;
using System.Linq;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Server.Infrastructure;
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
            return (from a in context.TenantManagements.Include("GlobalTenant") where a.Id == id select a).FirstOrDefault();
        }

        public TenantManagement GetSingleTenantManagementByBluesnapAccountId(string bluesnapaccountId)
        {
            var tenantManagement = (from a in context.TenantManagements.Include("GlobalTenant")
                                    where a.BluesnapAccount == bluesnapaccountId  && a.GlobalTenant.IsActive
                                    select a).FirstOrDefault();

            if(tenantManagement == null)
            {
                tenantManagement = (from a in context.TenantManagements.Include("GlobalTenant")
                                    where a.BluesnapAccount == bluesnapaccountId && !a.GlobalTenant.IsActive
                                    select a).FirstOrDefault();
            }

            return tenantManagement;
        }

        public List<TenantManagement> GetTenantManagementsForPackage(string packageCode)
        {
            return (from a in context.TenantManagements
                    where a.PackageCode == packageCode
                    select a).ToList();
        }

        public List<string> GetTenantManagementsIdsForPackage(string packageCode)
        {
            return  (from a in context.TenantManagements
                    where a.PackageCode == packageCode
                    select a.Id.ToString()).ToList();
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




    }
}