using Simplog.Data.ShipmentsModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Logitude.BL.ShipmentsModel.EntityPMs;

namespace Logitude.BL.ShipmentsModel.EntityQueries
{
    public class OceanInsightsRequestQuery
    {
        OceanInsightsRequestRepository repository;

        public OceanInsightsRequestQuery(int tenant)
        {
            repository = new OceanInsightsRequestRepository(tenant);
        }

        public OceanInsightsRequestQuery(OceanInsightsRequestRepository repository)
        {
            this.repository = repository;
        }

        public OceanInsightsRequestPM GetSinglePM(string id, int tenant)
        {
            OceanInsightsRequestPM myResult = (from a in repository.Context.OceanInsightsRequests
                                               where a.Tenant == tenant && a.Id == id
                                               select new OceanInsightsRequestPM()
                                                     {
                                                         Id = a.Id,
                                                         ContainerNumber = a.ContainerNumber,
                                                         CreateDate = a.CreateDate,
                                                         OceanInsigntId = a.OceanInsigntId,
                                                         SCACCode = a.SCACCode,
                                                         Tenant = a.Tenant,
                                                         UpdateDate = a.UpdateDate,
                                                         Type = a.Type,
                                                         BLNumber = a.BLNumber,
                                                         FromPushPage = a.FromPushPage
                                                     }).FirstOrDefault();

              
            return myResult;
        }

        public OceanInsightsRequestPM GetSinglePMByOceanInsightsIdTenant(string id, int tenant)
        {
            OceanInsightsRequestPM myResult = (from a in repository.Context.OceanInsightsRequests
                                               where a.Tenant == tenant && a.OceanInsigntId == id
                                               select new OceanInsightsRequestPM()
                                               {
                                                   Id = a.Id,
                                                   ContainerNumber = a.ContainerNumber,
                                                   CreateDate = a.CreateDate,
                                                   OceanInsigntId = a.OceanInsigntId,
                                                   SCACCode = a.SCACCode,
                                                   Tenant = a.Tenant,
                                                   UpdateDate = a.UpdateDate,
                                                   Type = a.Type,
                                                   BLNumber = a.BLNumber,
                                                   FromPushPage = a.FromPushPage
                                               }).FirstOrDefault();


            return myResult;
        }

        public OceanInsightsRequestPM GetSinglePMByOceanInsightsId(string id)
        {
            OceanInsightsRequestPM myResult = (from a in repository.Context.OceanInsightsRequests
                                               where a.OceanInsigntId == id
                                               select new OceanInsightsRequestPM()
                                               {
                                                   Id = a.Id,
                                                   ContainerNumber = a.ContainerNumber,
                                                   CreateDate = a.CreateDate,
                                                   OceanInsigntId = a.OceanInsigntId,
                                                   SCACCode = a.SCACCode,
                                                   Tenant = a.Tenant,
                                                   UpdateDate = a.UpdateDate,
                                                   Type = a.Type,
                                                   BLNumber = a.BLNumber,
                                                   FromPushPage = a.FromPushPage
                                               }).FirstOrDefault();


            return myResult;
        }

        public List<OceanInsightsRequestPM> GetAllByOceanInsightsId(string id)
        {
            List<OceanInsightsRequestPM> myResult = (from a in repository.Context.OceanInsightsRequests
                                               where a.OceanInsigntId == id
                                               select new OceanInsightsRequestPM()
                                               {
                                                   Id = a.Id,
                                                   ContainerNumber = a.ContainerNumber,
                                                   CreateDate = a.CreateDate,
                                                   OceanInsigntId = a.OceanInsigntId,
                                                   SCACCode = a.SCACCode,
                                                   Tenant = a.Tenant,
                                                   UpdateDate = a.UpdateDate,
                                                   Type = a.Type,
                                                   BLNumber = a.BLNumber,
                                                   FromPushPage = a.FromPushPage
                                               }).ToList();


            return myResult;
        }

        public OceanInsightsRequestPM GetSinglePMByOceanInsightsByScacCodeContainerNoTenant(string ScacCode, string ContainerNo, int Tenant)
        {
            OceanInsightsRequestPM myResult = (from a in repository.Context.OceanInsightsRequests
                                               where a.ContainerNumber == ContainerNo && a.SCACCode == ScacCode && a.Tenant == Tenant
                                               select new OceanInsightsRequestPM()
                                               {
                                                   Id = a.Id,
                                                   ContainerNumber = a.ContainerNumber,
                                                   CreateDate = a.CreateDate,
                                                   OceanInsigntId = a.OceanInsigntId,
                                                   SCACCode = a.SCACCode,
                                                   Tenant = a.Tenant,
                                                   UpdateDate = a.UpdateDate,
                                                   Type = a.Type,
                                                   BLNumber = a.BLNumber,
                                                   FromPushPage = a.FromPushPage
                                               }).FirstOrDefault();


            return myResult;
        }

        public OceanInsightsRequestPM GetSinglePMByOceanInsightsByCareierScacBLNoTenant(string CarrierScac, string BLNumber, int Tenant)
        {
            OceanInsightsRequestPM myResult = (from a in repository.Context.OceanInsightsRequests
                                               where a.BLNumber == BLNumber && a.SCACCode == CarrierScac && a.Tenant == Tenant
                                               select new OceanInsightsRequestPM()
                                               {
                                                   Id = a.Id,
                                                   ContainerNumber = a.ContainerNumber,
                                                   CreateDate = a.CreateDate,
                                                   OceanInsigntId = a.OceanInsigntId,
                                                   SCACCode = a.SCACCode,
                                                   Tenant = a.Tenant,
                                                   UpdateDate = a.UpdateDate,
                                                   Type = a.Type,
                                                   BLNumber = a.BLNumber,
                                                   FromPushPage = a.FromPushPage
                                               }).FirstOrDefault();


            return myResult;
        }

        public List<OceanInsightsRequestPM> GetOceanInsightsRequestPMs(int tenant)
        {
            List<OceanInsightsRequestPM> myResult = (from a in repository.Context.OceanInsightsRequests
                                                     where a.Tenant == tenant 
                                                     select new OceanInsightsRequestPM()
                                                     {
                                                         Id = a.Id,
                                                         ContainerNumber = a.ContainerNumber,
                                                         CreateDate = a.CreateDate,
                                                         OceanInsigntId = a.OceanInsigntId,
                                                         SCACCode = a.SCACCode,
                                                         Tenant = a.Tenant,
                                                         UpdateDate = a.UpdateDate,
                                                         Type = a.Type,
                                                         BLNumber = a.BLNumber,
                                                         FromPushPage = a.FromPushPage
                                                     }).ToList(); 
            return myResult;
        }

    }
}