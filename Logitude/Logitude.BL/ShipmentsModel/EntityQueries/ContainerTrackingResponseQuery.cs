using System;
using System.Linq;
using Simplog.Data.ShipmentsModel.Repositories;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityLists;
using Simplog.Data.ShipmentsModel.EntityPOCOs;

namespace Logitude.BL.ShipmentsModel.EntityQueries
{
    public class ContainerTrackingResponseQuery
    {
        ContainerTrackingResponseRepository repository;

        public ContainerTrackingResponseQuery(int tenant)
        {
            repository = new ContainerTrackingResponseRepository(tenant);
        }

        public ContainerTrackingResponseQuery(ContainerTrackingResponseRepository repository)
        {
            this.repository = repository;
        }

        public ContainerTrackingResponsePM GetSinglePM(string id,int tenant)
        {
            if (string.IsNullOrEmpty(id))
                return null;

            ContainerTrackingResponsePM entity = (from a in repository.context.ContainerTrackingResponses where a.Id == id && a.Tenant == tenant
                                                  select new ContainerTrackingResponsePM()
                                                  {
                                                      Id = a.Id,
                                                      CommunicationLogId = a.CommunicationLogId,
                                                      Tenant = a.Tenant,
                                                      ContainerTrackingRequestId = a.ContainerTrackingRequestId,
                                                      CreateDate = a.CreateDate,
                                                      SearchFields = a.SearchFields
                                                  }).FirstOrDefault();

            return entity;


        }

        public IQueryable<ContainerTrackingResponseList> GetIQueryableEntityList(IQueryable<ContainerTrackingResponse> iQueryable)
        {
            IQueryable<ContainerTrackingResponseList> result = (from a in iQueryable
                                                                select new ContainerTrackingResponseList()
                                                                {
                                                                    Id = a.Id,
                                                                    CommunicationLogId = a.CommunicationLogId,
                                                                    Tenant = a.Tenant,
                                                                    ContainerTrackingRequestId = a.ContainerTrackingRequestId,
                                                                    CreateDate = a.CreateDate,
                                                                    SearchFields = a.SearchFields
                                                                });
            return result;
        }


    }
}