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
    public class SharedLogisticsInvitationStatusQuery
    {
        SharedLogisticsInvitationStatusRepository repository;

        public SharedLogisticsInvitationStatusQuery()
        {
            repository = new SharedLogisticsInvitationStatusRepository(); 
        }

        public SharedLogisticsInvitationStatusQuery(int tenant)
        {
            repository = new SharedLogisticsInvitationStatusRepository(tenant);
        }

        public SharedLogisticsInvitationStatusQuery(SharedLogisticsInvitationStatusRepository repository)
        {
            this.repository = repository;
        }

        public SharedLogisticsInvitationStatusPM GetSingleSharedLogisticsInvitationStatusPM(int code)
        {
            return (from a in repository.context.SharedLogisticsInvitationStatus
                    where a.Code == code
                    select new SharedLogisticsInvitationStatusPM()
                    {
                        Code = a.Code,
                        Name = a.Name,
                        SearchFields = a.SearchFields,
                    }).FirstOrDefault();

        }

        public SharedLogisticsInvitationStatusPM GetSinglePM(int code, int tenant)
        {
            return (from a in repository.context.SharedLogisticsInvitationStatus
                    where a.Code == code
                    select new SharedLogisticsInvitationStatusPM()
                    {
                        Code = a.Code,
                        Name = a.Name,
                        SearchFields = a.SearchFields,
                    }).FirstOrDefault();

        }

        public IQueryable<SharedLogisticsInvitationStatusPM> GetSharedLogisticsInvitationStatusPMs()
        {
            return from a in repository.context.SharedLogisticsInvitationStatus
                   select new SharedLogisticsInvitationStatusPM()
                   {
                       Code = a.Code,
                       Name = a.Name,
                       SearchFields = a.SearchFields,
                   };
        }

        public IQueryable<SharedLogisticsInvitationStatusList> GetIQueryableEntityList(IQueryable<SharedLogisticsInvitationStatus> iQueryable)
        {
            IQueryable<SharedLogisticsInvitationStatusList> result = from entity in iQueryable
                                                       select new SharedLogisticsInvitationStatusList()
                                                {
                                                    Name = entity.Name,
                                                    Code = entity.Code,
                                                    SearchFields = entity.SearchFields,
                                                };
            return result;
        }
    }
}