using System;
using System.Linq;
using Simplog.Data.ShipmentsModel.Repositories;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityLists;
using Simplog.Data.ShipmentsModel.EntityPOCOs;

namespace Logitude.BL.ShipmentsModel.EntityQueries
{
    public class ContainerTrackingRequestQuery
    {
        ContainerTrackingRequestRepository repository;

        public ContainerTrackingRequestQuery(int tenant)
        {
            repository = new ContainerTrackingRequestRepository(tenant);
        }

        public ContainerTrackingRequestQuery(ContainerTrackingRequestRepository repository)
        {
            this.repository = repository;
        }

        public ContainerTrackingRequestPM GetSinglePM(string id,int tenant)
        {
            if (string.IsNullOrEmpty(id))
                return null;

            ContainerTrackingRequestPM entity = (from a in repository.context.ContainerTrackingRequests where a.Id == id && a.Tenant == tenant
                                                 select new ContainerTrackingRequestPM()
                                                  {
                                                      Id = a.Id,
                                                      RequestId = a.RequestId,
                                                      SearchFields = a.SearchFields,
                                                      ContainerNumber = a.ContainerNumber,
                                                      Provider = a.Provider,
                                                      Master = a.Master,
                                                      Tenant = a.Tenant
                                                  }).FirstOrDefault();

            return entity;


        }

        public IQueryable<ContainerTrackingRequestList> GetIQueryableEntityList(IQueryable<ContainerTrackingRequest> iQueryable)
        {
            IQueryable<ContainerTrackingRequestList> result = (from a in iQueryable
                                                                select new ContainerTrackingRequestList()
                                                                {
                                                                    Id = a.Id,
                                                                    RequestId = a.RequestId,
                                                                    SearchFields = a.SearchFields,
                                                                    ContainerNumber = a.ContainerNumber,
                                                                    Provider = a.Provider,
                                                                    Master = a.Master,
                                                                    Tenant = a.Tenant
                                                                });
            return result;
        }


    }
}