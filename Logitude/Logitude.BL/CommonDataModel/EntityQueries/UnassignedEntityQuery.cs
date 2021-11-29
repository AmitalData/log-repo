using System;
using System.Linq;
using System.Collections.Generic;
using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.BL.CommonDataModel.EntityLists;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Logitude.BL.CommonDataModel.EntityQueries
{
    public class UnassignedEntityQuery
    {
        UnassignedEntityRepository repository;

        public UnassignedEntityQuery()
        {
            repository = new UnassignedEntityRepository();
        }

        public UnassignedEntityQuery(int tenant)
        {
            repository = new UnassignedEntityRepository(tenant);
        }

        public UnassignedEntityQuery(UnassignedEntityRepository repository)
        {
            this.repository = repository;
        }

        public UnassignedEntityPM GetSinglePM(string id, int tenant)
        {
            return (from a in repository.context.UnassignedEntitys.Include("ObjectTable")
                    where a.Id == id && a.Tenant == tenant
                    select new UnassignedEntityPM()
                    {
                        Id = a.Id,
                        Tenant = a.Tenant,
                        ObjectTableId = a.ObjectTableId,
                        UnassignedCode = a.UnassignedCode
                    }).FirstOrDefault();
        }

        public List<UnassignedEntityPM> GetWeightUnitPMs()
        {
            List<UnassignedEntityPM> unassignedEntitys
                 = (from a in repository.context.UnassignedEntitys.Include("ObjectTable")
                    select new UnassignedEntityPM()
                    {
                        Id = a.Id,
                        Tenant = a.Tenant,
                        ObjectTableId = a.ObjectTableId,
                        UnassignedCode = a.UnassignedCode
                    }).ToList();

            return unassignedEntitys;
        }

        public UnassignedEntityPM GetSinglePMByObjectTableId(string objectTableId, int tenant)
        {
            return (from a in repository.context.UnassignedEntitys
                    where a.ObjectTableId == objectTableId && a.Tenant ==tenant
                    select new UnassignedEntityPM()
                    {
                        Id = a.Id,
                        Tenant = a.Tenant,
                        ObjectTableId = a.ObjectTableId,
                        UnassignedCode = a.UnassignedCode
                    }).FirstOrDefault();
        }

        public IQueryable<UnassignedEntityList> GetIQueryableEntityList(IQueryable<UnassignedEntity> iQueryable)
        {
            IQueryable<UnassignedEntityList> result = (from a in iQueryable
                                              select new UnassignedEntityList()
                                              {
                                                  Id = a.Id,
                                                  Tenant = a.Tenant,
                                                  ObjectTableId = a.ObjectTableId,
                                                  UnassignedCode = a.UnassignedCode
                                              });
            return result;
        }
    }
}
