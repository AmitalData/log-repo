using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel.DomainServices.Server;
using System.Web;
using System.Xml.Serialization;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.BL.Helpers;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using System.Collections.ObjectModel;

namespace WebFreight.Web.CommonDataModel.DomainServices
{
    public partial class CommonDataDomainService
    {
   

        HybridTenantThresholdRepository hybridTenantThresholdRepository;
        HybridTenantThresholdQuery hybridTenantThresholdQuery;



        public HybridTenantThresholdPM GetSingleHybridTenantThreshold(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("HybridTenantThreshold", "READ", tenant);

             hybridTenantThresholdQuery = new HybridTenantThresholdQuery(tenant);
            return hybridTenantThresholdQuery.GetSinglePM(id, tenant);
        }

        public HybridTenantThresholdPM GetHybridTenantThresholdByIdTenant( int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("HybridTenantThreshold", "READ", tenant);

            hybridTenantThresholdQuery = new HybridTenantThresholdQuery(tenant);
            return hybridTenantThresholdQuery.GetHybridTenantThresholdByTenant(tenant);
        }








        //public HybridTenantThresholdList GetSingleHybridTenantThresholdList(string id, int tenant)
        //{
        //    SecurityUtility.AuthenticationOnTenant(tenant);
        //    SecurityUtility.CheckContactFeature("HybridTenantThreshold", "READ", tenant);

        //    HybridTenantThresholdRepository = new HybridTenantThresholdRepository(tenant);
        //    HybridTenantThresholdQuery = new HybridTenantThresholdQuery(HybridTenantThresholdRepository);
        //    HybridTenantThresholdList HybridTenantThresholdList = null;
        //    HybridTenantThreshold HybridTenantThreshold = HybridTenantThresholdRepository.GetSingleHybridTenantThreshold(id, tenant);

        //    if (HybridTenantThreshold != null)
        //    {
        //        List<HybridTenantThreshold> singleEntityList = new List<HybridTenantThreshold>();
        //        singleEntityList.Add(HybridTenantThreshold);

        //        IQueryable<HybridTenantThreshold> iQueryable = singleEntityList.AsQueryable();
        //        IQueryable<HybridTenantThresholdList> iQueryableEntityList = HybridTenantThresholdQuery.GetIQueryableEntityList(iQueryable);
        //        HybridTenantThresholdList = iQueryableEntityList.FirstOrDefault();
        //    }
        //    return HybridTenantThresholdList;
        //}

        //public IQueryable<HybridTenantThresholdList> GetHybridTenantThresholdLists(int tenant)
        //{
        //    SecurityUtility.AuthenticationOnTenant(tenant);
        //    SecurityUtility.CheckContactFeature("HybridTenantThreshold", "READ", tenant);

        //    HybridTenantThresholdRepository = new HybridTenantThresholdRepository(tenant);
        //    HybridTenantThresholdQuery = new HybridTenantThresholdQuery(HybridTenantThresholdRepository);
        //    IQueryable<HybridTenantThreshold> HybridTenantThresholdes = HybridTenantThresholdRepository.GetHybridTenantThresholds(tenant);
        //    IQueryable<HybridTenantThresholdList> query2 = HybridTenantThresholdQuery.GetIQueryableEntityList(HybridTenantThresholdes);
        //    return query2;
        //}

     


        public void InsertHybridTenantThreshold(HybridTenantThresholdPM HybridTenantThreshold)
        {
            SecurityUtility.CheckContactFeature("HybridTenantThreshold", "NEW", HybridTenantThreshold.Tenant);

            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(HybridTenantThreshold.Tenant);
            }
            HybridTenantThresholdService service = new HybridTenantThresholdService(objectContext, HybridTenantThreshold.Tenant);
            service.Create(HybridTenantThreshold);

            TableLastUpdateClass.UpdateTableHistory(HybridTenantThreshold.Tenant, "HybridTenantThreshold");
        }

        public void UpdateHybridTenantThreshold(HybridTenantThresholdPM currentHybridTenantThreshold)
        {
            SecurityUtility.CheckContactFeature("HybridTenantThreshold", "UPDATE", currentHybridTenantThreshold.Tenant);

            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(currentHybridTenantThreshold.Tenant);
            }

            hybridTenantThresholdRepository = new HybridTenantThresholdRepository(objectContext);

            string entityName = "HybridTenantThreshold" + currentHybridTenantThreshold.Tenant;
            string entityPmName = "HybridTenantThresholdPM" +currentHybridTenantThreshold.Tenant;

            if (CacheManager.CacheWrapper.Get(entityName) != null)
            {
                CacheManager.CacheWrapper.Invalidate(entityName);
            }
            if (CacheManager.CacheWrapper.Get(entityPmName) != null)
            {
                CacheManager.CacheWrapper.Invalidate(entityPmName);
            }
            HybridTenantThresholdService service = new HybridTenantThresholdService(objectContext, currentHybridTenantThreshold.Tenant);
            service.Update(currentHybridTenantThreshold);
            TableLastUpdateClass.UpdateTableHistory(currentHybridTenantThreshold.Tenant, "HybridTenantThreshold");
        }

        public void DeleteHybridTenantThreshold(HybridTenantThresholdPM HybridTenantThreshold)
        {
            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(HybridTenantThreshold.Tenant);
            }
            hybridTenantThresholdRepository = new HybridTenantThresholdRepository(objectContext);


            HybridTenantThreshold entity = hybridTenantThresholdRepository.GetSingleHybridTenantThreshold(HybridTenantThreshold.Tenant);
            hybridTenantThresholdRepository.Remove(entity);
        }
    }
}