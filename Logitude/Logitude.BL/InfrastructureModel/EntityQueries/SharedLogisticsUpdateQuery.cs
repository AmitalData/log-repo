using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;

using Logitude.BL.InfrastructureModel.EntityLists;
using Logitude.BL.InfrastructureModel.EntityPMs;

namespace Logitude.BL.InfrastructureModel.EntityQueries
{
    public class SharedLogisticsUpdateQuery
    {
        SharedLogisticsUpdateRepository repository;
        public SharedLogisticsUpdateQuery()
        {
            repository = new SharedLogisticsUpdateRepository(); 
        }

        public SharedLogisticsUpdateQuery(int tenant)
        {
            repository = new SharedLogisticsUpdateRepository(tenant);
        }

        public SharedLogisticsUpdateQuery(SharedLogisticsUpdateRepository sharedLogisticsUpdateRepository)
        {
            repository = sharedLogisticsUpdateRepository;
        }


        public SharedLogisticsUpdatePM GetSingleSharedLogisticUpdatePM(string id)
        {
            return (from a in repository.context.SharedLogisticsUpdates
                    where a.Id == id
                    select new SharedLogisticsUpdatePM()
                    {
                        Id = a.Id,
                        EntityId = a.EntityId,
                        DocumentId = a.DocumentId,
                        HandledByUserId = a.HandledByUserId,
                        HandledDate = a.HandledDate,
                        ObjectTableId = a.ObjectTableId,
                        Read = a.Read,
                        ReceivedDate = a.ReceivedDate,
                        ReceivedFrom = a.ReceivedFrom,
                        Status = a.Status,
                        Subject = a.Subject,
                        Tenant = a.Tenant,
                    }).FirstOrDefault();
        }

        public List<SharedLogisticsUpdatePM> GetSharedLogisticsUpdatePMsForEntity(string entityid, int tenant)
        {
            return (from a in repository.context.SharedLogisticsUpdates
                    where a.EntityId == entityid
                    select new SharedLogisticsUpdatePM()
                    {
                        Id = a.Id,
                        EntityId = a.EntityId,
                        DocumentId = a.DocumentId,
                        HandledByUserId = a.HandledByUserId,
                        HandledDate = a.HandledDate,
                        ObjectTableId = a.ObjectTableId,
                        Read = a.Read,
                        ReceivedDate = a.ReceivedDate,
                        ReceivedFrom = a.ReceivedFrom,
                        Status = a.Status,
                        Subject = a.Subject,
                        Tenant = a.Tenant,
                    }).ToList();
        }


        public List<SharedLogisticsUpdatePM> GetSharedLogisticsUpdatePMs()
        {
            return (from a in repository.context.SharedLogisticsUpdates
                    select new SharedLogisticsUpdatePM()
                    {
                        Id = a.Id,
                        EntityId = a.EntityId,
                        DocumentId = a.DocumentId,
                        HandledByUserId = a.HandledByUserId,
                        HandledDate = a.HandledDate,
                        ObjectTableId = a.ObjectTableId,
                        Read = a.Read,
                        ReceivedDate = a.ReceivedDate,
                        ReceivedFrom = a.ReceivedFrom,
                        Status = a.Status,
                        Subject = a.Subject,
                        Tenant = a.Tenant,
                    }).ToList();
        }


        public IQueryable<SharedLogisticsUpdateList> GetIQueryableEntityList(IQueryable<SharedLogisticsUpdate> iQueryable)
        {
            IQueryable<SharedLogisticsUpdateList> result = from entity in iQueryable
                                                           select new SharedLogisticsUpdateList()
                                                           {
                                                               Id = entity.Id,
                                                               EntityId = entity.EntityId,
                                                               DocumentId = entity.DocumentId,
                                                               HandledByUserId = entity.HandledByUserId,
                                                               HandledDate = entity.HandledDate,
                                                               ObjectTableId = entity.ObjectTableId,
                                                               Read = entity.Read,
                                                               ReceivedDate = entity.ReceivedDate,
                                                               ReceivedFrom = entity.ReceivedFrom,
                                                               Status = entity.Status,
                                                               Subject = entity.Subject,
                                                               Tenant = entity.Tenant,
                                                           };
            return result;
        }

    }
}