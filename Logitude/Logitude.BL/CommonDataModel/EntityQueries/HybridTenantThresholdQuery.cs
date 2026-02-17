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
    public class  HybridTenantThresholdQuery
    {
        HybridTenantThresholdRepository repository;

        public  HybridTenantThresholdQuery()
        {
               repository = new  HybridTenantThresholdRepository(); 
        }

        public  HybridTenantThresholdQuery(int tenant)
        {
            repository = new  HybridTenantThresholdRepository(tenant);
        }

        public  HybridTenantThresholdQuery( HybridTenantThresholdRepository  HybridTenantThresholdRepository)
        {
            repository =  HybridTenantThresholdRepository;
        }

        public  HybridTenantThresholdPM GetSinglePM(string id, int tenant)
        {

            var query = (from a in repository.context.HybridTenantThresholds
                         where a.Tenant == tenant
                         select new HybridTenantThresholdPM()
                         {
                              
                             Tenant = a.Tenant,
                             FailedThresold = a.FailedThresold,
                             WaitingThresold = a.WaitingThresold,
                         }).FirstOrDefault();

            return query;
        }


        public HybridTenantThresholdPM GetSinglePM(int tenant2, int tenant)
        {

            var query = (from a in repository.context.HybridTenantThresholds
                         where a.Tenant == tenant
                         select new HybridTenantThresholdPM()
                         {

                             Tenant = a.Tenant,
                             FailedThresold = a.FailedThresold,
                             WaitingThresold = a.WaitingThresold,
                         }).FirstOrDefault();

            return query;
        }





        public  HybridTenantThresholdPM GetHybridTenantThresholdByTenant( int tenant)
        {
            var query = (from a in repository.context.HybridTenantThresholds
                         where a.Tenant == tenant
                         select new  HybridTenantThresholdPM()
                         {
                              
                             Tenant = a.Tenant,
                             FailedThresold = a.FailedThresold,
                             WaitingThresold = a.WaitingThresold,
                         }).FirstOrDefault();

            return query;
        }



        public HybridTenantThresholdList GetHybridTenantThresholdListByTenant(int tenant)
        {
            var query = (from a in repository.context.HybridTenantThresholds
                         where a.Tenant == tenant
                         select new HybridTenantThresholdList()
                         {

                             Tenant = a.Tenant,
                             FailedThresold = a.FailedThresold,
                             WaitingThresold = a.WaitingThresold,
                         }).FirstOrDefault();

            return query;
        }

        public IQueryable<HybridTenantThresholdList> GetIQueryableEntityList(IQueryable<HybridTenantThreshold> pocos)
        {
            var query = (from a in pocos
                         select new HybridTenantThresholdList()
                         {

                             Tenant = a.Tenant,
                             FailedThresold = a.FailedThresold,
                             WaitingThresold = a.WaitingThresold,
                         });

            return query;

        }
    
    }
}
