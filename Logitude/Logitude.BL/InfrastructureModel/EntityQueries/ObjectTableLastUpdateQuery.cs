using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Simplog.Server.Infrastructure.Helpers;

using Simplog.Data.InfrastructureModel.Repositories;

using Logitude.BL.InfrastructureModel.EntityPMs;

namespace Logitude.BL.InfrastructureModel.EntityQueries
{
    public class ObjectTableLastUpdateQuery
    {
        ObjectTableLastUpdateRepository repository;
        public ObjectTableLastUpdateQuery()
        {
            repository = new ObjectTableLastUpdateRepository();
        }

        public ObjectTableLastUpdateQuery(int tenant)
        {
            repository = new ObjectTableLastUpdateRepository(tenant);
        }

        public ObjectTableLastUpdateQuery(ObjectTableLastUpdateRepository entityLastAccessRepository)
        {
            repository = entityLastAccessRepository;
        }

        public ObjectTableLastUpdatePM GetObjectTableLastUpdatePM(string objectTableId, string entityId, string userId, int tenant)
        {
            ObjectTableLastUpdatePM entitylast = (from a in repository.context.ObjectTableLastUpdates.Include("ObjectTable")
                                                  where a.ObjectTableId == objectTableId && a.Tenant == tenant
                                                  select new ObjectTableLastUpdatePM()
                                                  {

                                                      Id = a.Id,
                                                      ObjectTableId = a.ObjectTableId,
                                                      Tenant = a.Tenant,
                                                      UpdatedByUserId = a.UpdatedByUserId,
                                                      LastUpdateDate = a.LastUpdateDate,
                                                      ObjectTableName = a.ObjectTable.Name,
                                                  }).FirstOrDefault();
            return entitylast;
        }

        public List<ObjectTableLastUpdatePM> GetLastUpdatedTables(int tenant, DateTime sinceDate)
        {
            return (from a in repository.context.ObjectTableLastUpdates.Include("ObjectTable")
                    where a.LastUpdateDate > sinceDate && a.Tenant == tenant && a.ObjectTable.CacheOnClient // (a.ObjectTable.IsClosed && 
                    select new ObjectTableLastUpdatePM()
                    {
                        Id = a.Id,
                        ObjectTableId = a.ObjectTableId,
                        Tenant = a.Tenant,
                        UpdatedByUserId = a.UpdatedByUserId,
                        LastUpdateDate = a.LastUpdateDate,
                        ObjectTableName = a.ObjectTable.Name,
                    }).OrderByDescending(d => d.LastUpdateDate).ToList();
        }

        public List<ObjectTableLastUpdatePM> GetLastUpdatedTablesForTenant(int tenant, DateTime sinceDate)
        {
			return (from a in repository.context.ObjectTableLastUpdates.Include("ObjectTable")
					where a.LastUpdateDate > sinceDate && (a.Tenant == tenant || a.Tenant == 0) && a.ObjectTable.CacheOnClient &&  !a.ObjectTable.IsClosed // (a.ObjectTable.IsClosed && 
					select new ObjectTableLastUpdatePM()
					{
						Id = a.Id,
						ObjectTableId = a.ObjectTableId,
						Tenant = a.Tenant,
						UpdatedByUserId = a.UpdatedByUserId,
						LastUpdateDate = a.LastUpdateDate,
						ObjectTableName = a.ObjectTable.Name,
					}).OrderByDescending(d => d.LastUpdateDate).ToList();
        }


        public ObjectTableLastUpdatePM GetLastUpdatedTable(int tenant)
        {
            return (from a in repository.context.ObjectTableLastUpdates.Include("ObjectTable")
                    where a.Tenant == tenant
                    select new ObjectTableLastUpdatePM()
                    {
                        Id = a.Id,
                        ObjectTableId = a.ObjectTableId,
                        Tenant = a.Tenant,
                        UpdatedByUserId = a.UpdatedByUserId,
                        LastUpdateDate = a.LastUpdateDate,
                        ObjectTableName = a.ObjectTable.Name,
                    }).OrderByDescending(d => d.LastUpdateDate).FirstOrDefault();
        }

    }
}