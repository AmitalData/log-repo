using Simplog.Data.ShipmentsModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Logitude.BL.ShipmentsModel.EntityPMs;

namespace Logitude.BL.ShipmentsModel.EntityQueries
{
    public class OceanInsightsRequestsCountQuery
    {
        OceanInsightsRequestsCountRepository repository;

        public OceanInsightsRequestsCountQuery(int tenant)
        {
            repository = new OceanInsightsRequestsCountRepository(tenant);
        }

        public OceanInsightsRequestsCountQuery(OceanInsightsRequestsCountRepository repository)
        {
            this.repository = repository;
        }

        public OceanInsightsRequestsCountPM GetSinglePM(string id, int tenant)
        {
            OceanInsightsRequestsCountPM myResult = (from a in repository.Context.OceanInsightsRequestsCounts
                                               where a.Tenant == tenant && a.Id == id
                                               select new OceanInsightsRequestsCountPM()
                                                     {
                                                         Id = a.Id,
                                                         ContainerNumber = a.ContainerNumber,
                                                         CreateDate = a.CreateDate,
                                                         OceanInsigntId = a.OceanInsigntId,
                                                         ContainerSubscriptionId = a.ContainerSubscriptionId,
                                                         Tenant = a.Tenant, 
                                                         Type = a.Type,
                                                         BLNumber = a.BLNumber, 
                                                     }).FirstOrDefault();

              
            return myResult;
        }

        public OceanInsightsRequestsCountPM GetSinglePMByOceanInsightsIdTenant(string id, int tenant)
        {
            OceanInsightsRequestsCountPM myResult = (from a in repository.Context.OceanInsightsRequestsCounts
                                               where a.Tenant == tenant && a.OceanInsigntId == id
                                               select new OceanInsightsRequestsCountPM()
                                               {
                                                   Id = a.Id,
                                                   ContainerNumber = a.ContainerNumber,
                                                   CreateDate = a.CreateDate,
                                                   OceanInsigntId = a.OceanInsigntId,
                                                   ContainerSubscriptionId = a.ContainerSubscriptionId,
                                                   Tenant = a.Tenant,
                                                   Type = a.Type,
                                                   BLNumber = a.BLNumber,
                                               }).FirstOrDefault();


            return myResult;
        }

        public OceanInsightsRequestsCountPM GetSinglePMByOceanInsightsId(string id)
        {
            OceanInsightsRequestsCountPM myResult = (from a in repository.Context.OceanInsightsRequestsCounts
                                               where a.OceanInsigntId == id
                                               select new OceanInsightsRequestsCountPM()
                                               {
                                                   Id = a.Id,
                                                   ContainerNumber = a.ContainerNumber,
                                                   CreateDate = a.CreateDate,
                                                   OceanInsigntId = a.OceanInsigntId,
                                                   ContainerSubscriptionId = a.ContainerSubscriptionId,
                                                   Tenant = a.Tenant,
                                                   Type = a.Type,
                                                   BLNumber = a.BLNumber,
                                               }).FirstOrDefault();


            return myResult;
        }

        public List<OceanInsightsRequestsCountPM> GetAllByOceanInsightsId(string id)
        {
            List<OceanInsightsRequestsCountPM> myResult = (from a in repository.Context.OceanInsightsRequestsCounts
                                               where a.OceanInsigntId == id
                                               select new OceanInsightsRequestsCountPM()
                                               {
                                                   Id = a.Id,
                                                   ContainerNumber = a.ContainerNumber,
                                                   CreateDate = a.CreateDate,
                                                   OceanInsigntId = a.OceanInsigntId,
                                                   ContainerSubscriptionId = a.ContainerSubscriptionId,
                                                   Tenant = a.Tenant,
                                                   Type = a.Type,
                                                   BLNumber = a.BLNumber,
                                               }).ToList();


            return myResult;
        }

        public OceanInsightsRequestsCountPM GetSinglePMByOceanInsightsByContainerNoTenant(string ContainerNo, int Tenant)
        {
            OceanInsightsRequestsCountPM myResult = (from a in repository.Context.OceanInsightsRequestsCounts
                                                     where a.ContainerNumber == ContainerNo && a.Tenant == Tenant
                                                     select new OceanInsightsRequestsCountPM()
                                                     {
                                                         Id = a.Id,
                                                         ContainerNumber = a.ContainerNumber,
                                                         CreateDate = a.CreateDate,
                                                         OceanInsigntId = a.OceanInsigntId,
                                                         ContainerSubscriptionId = a.ContainerSubscriptionId,
                                                         Tenant = a.Tenant,
                                                         Type = a.Type,
                                                         BLNumber = a.BLNumber,
                                                     }).FirstOrDefault();


            return myResult;
        }

        public OceanInsightsRequestsCountPM GetSinglePMByOceanInsightsByBLNoTenant(string BLNumber, int Tenant)
        {
            OceanInsightsRequestsCountPM myResult = (from a in repository.Context.OceanInsightsRequestsCounts
                                                     where a.BLNumber == BLNumber && a.Tenant == Tenant
                                                     select new OceanInsightsRequestsCountPM()
                                                     {
                                                         Id = a.Id,
                                                         ContainerNumber = a.ContainerNumber,
                                                         CreateDate = a.CreateDate,
                                                         OceanInsigntId = a.OceanInsigntId,
                                                         ContainerSubscriptionId = a.ContainerSubscriptionId,
                                                         Tenant = a.Tenant,
                                                         Type = a.Type,
                                                         BLNumber = a.BLNumber,
                                                     }).FirstOrDefault();


            return myResult;
        }

        public List<OceanInsightsRequestsCountPM> GetOceanInsightsRequestsCountPMs(int tenant)
        {
            List<OceanInsightsRequestsCountPM> myResult = (from a in repository.Context.OceanInsightsRequestsCounts
                                                     where a.Tenant == tenant 
                                                     select new OceanInsightsRequestsCountPM()
                                                     {
                                                         Id = a.Id,
                                                         ContainerNumber = a.ContainerNumber,
                                                         CreateDate = a.CreateDate,
                                                         OceanInsigntId = a.OceanInsigntId,
                                                         ContainerSubscriptionId = a.ContainerSubscriptionId,
                                                         Tenant = a.Tenant,
                                                         Type = a.Type,
                                                         BLNumber = a.BLNumber,
                                                     }).ToList(); 
            return myResult;
        }

        public OceanInsightsRequestsCountPM GetSinglePMByOceanInsightsByContainerNoOceanInsigntId(string ContainerNo, string OceanInsigntId)
        {
            OceanInsightsRequestsCountPM myResult = (from a in repository.Context.OceanInsightsRequestsCounts
                                                     where a.ContainerNumber == ContainerNo && a.OceanInsigntId == OceanInsigntId
                                                     select new OceanInsightsRequestsCountPM()
                                                     {
                                                         Id = a.Id,
                                                         ContainerNumber = a.ContainerNumber,
                                                         CreateDate = a.CreateDate,
                                                         OceanInsigntId = a.OceanInsigntId,
                                                         ContainerSubscriptionId = a.ContainerSubscriptionId,
                                                         Tenant = a.Tenant,
                                                         Type = a.Type,
                                                         BLNumber = a.BLNumber,
                                                     }).FirstOrDefault();


            return myResult;
        }

    }
}