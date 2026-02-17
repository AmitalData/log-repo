using Simplog.Data.ShipmentsModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Logitude.BL.ShipmentsModel.EntityPMs;

namespace Logitude.BL.ShipmentsModel.EntityQueries
{
    public class OceanInsightsStatusesQuery
    {
        OceanInsightsStatusesRepository repository;

        public OceanInsightsStatusesQuery(int tenant)
        {
            repository = new OceanInsightsStatusesRepository(tenant);
        }

        public OceanInsightsStatusesQuery(OceanInsightsStatusesRepository repository)
        {
            this.repository = repository;
        }

        public OceanInsightsStatusesPM GetSinglePM(string id, int tenant)
        {
            OceanInsightsStatusesPM myResult = (from a in repository.Context.OceanInsightsStatuses
                                            where a.Tenant == tenant && a.Id == id
                                            select new OceanInsightsStatusesPM()
                                                  {
                                                      Id = a.Id,
                                                      CommunicationLogId = a.CommunicationLogId,
                                                      ContentDocumentId = a.ContentDocumentId,
                                                      CreateDate = a.CreateDate,
                                                      OceanInsightsRequestId = a.OceanInsightsRequestId,
                                                      Tenant = a.Tenant
                                                  }).FirstOrDefault();

            
            return myResult;
        }

        public List<OceanInsightsStatusesPM> GetOceanInsightsStatusesByRequestId(string RequestId, int tenant)
        {
            List<OceanInsightsStatusesPM> myResult = (from a in repository.Context.OceanInsightsStatuses
                                                  where a.Tenant == tenant && a.OceanInsightsRequestId == RequestId
                                                  select new OceanInsightsStatusesPM()
                                                  {
                                                      Id = a.Id,
                                                      CommunicationLogId = a.CommunicationLogId,
                                                      ContentDocumentId = a.ContentDocumentId,
                                                      CreateDate = a.CreateDate,
                                                      OceanInsightsRequestId = a.OceanInsightsRequestId,
                                                      Tenant = a.Tenant
                                                  }).ToList(); 
            return myResult;
        }

    }
}