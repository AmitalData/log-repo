using System;
using System.Linq;
using Simplog.Data.ShipmentsModel.Repositories;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityLists;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Logitude.BL.ShipmentsModel.APIDataContract;
using Logitude.BL.DataContracts;
using Logitude.BL.ShipmentsModel.CloseTables;

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

        public ContainerTrackingRequestPM GetSinglePM(string id, int tenant)
        {
            if (string.IsNullOrEmpty(id))
                return null;

            ContainerTrackingRequestPM entity = (from a in repository.context.ContainerTrackingRequests
                                                 where a.Id == id && a.Tenant == tenant
                                                 select new ContainerTrackingRequestPM()
                                                 {
                                                     Id = a.Id,
                                                     RequestId = a.RequestId,
                                                     SearchFields = a.SearchFields,
                                                     ContainerNumber = a.ContainerNumber,
                                                     Provider = a.Provider,
                                                     Master = a.Master,
                                                     Tenant = a.Tenant,
                                                     CarrierCode = a.CarrierCode,
                                                     ContainerId = a.ContainerId,
                                                     CreateDate = a.CreateDate,
                                                     ShipmentId = a.ShipmentId,
                                                     Status = a.Status,
                                                     IsSimulate = a.IsSimulate,
                                                 }).FirstOrDefault();

            return entity;


        }
        public ContainerTrackingRequestPM GetActiveRequest(GeneralContainerTrackingArgs unsubscribeArgs, int tenant)
        {
            var containerTrackingRequestQuery = repository.context.ContainerTrackingRequests.Where(e => e.Tenant == tenant
                && e.Status == ContainerTrackingRequestStatus.Active
                && e.IsSimulate == unsubscribeArgs.IsSimulator
                && e.Provider == unsubscribeArgs.SourceCode);
            if (unsubscribeArgs.IsFromContainer)
            {
                containerTrackingRequestQuery = containerTrackingRequestQuery.Where(e => e.ContainerId == unsubscribeArgs.ContainerId);
            }
            else
            {
                containerTrackingRequestQuery = containerTrackingRequestQuery.Where(e => e.ShipmentId == unsubscribeArgs.ShipmentId && e.ContainerNumber == null);
            }
            ContainerTrackingRequestPM entity = containerTrackingRequestQuery.Select(a => new ContainerTrackingRequestPM()
            {
                Id = a.Id,
                RequestId = a.RequestId,
                SearchFields = a.SearchFields,
                ContainerNumber = a.ContainerNumber,
                Provider = a.Provider,
                Master = a.Master,
                Tenant = a.Tenant,
                CarrierCode = a.CarrierCode,
                ContainerId = a.ContainerId,
                CreateDate = a.CreateDate,
                ShipmentId = a.ShipmentId,
                Status = a.Status,
                IsSimulate = a.IsSimulate,
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
                                                                   Tenant = a.Tenant,
                                                                   CarrierCode = a.CarrierCode,
                                                                   ContainerId = a.ContainerId,
                                                                   CreateDate = a.CreateDate,
                                                                   ShipmentId = a.ShipmentId,
                                                                   Status = a.Status,
                                                                   IsSimulate = a.IsSimulate,
                                                               });
            return result;
        }


    }
}