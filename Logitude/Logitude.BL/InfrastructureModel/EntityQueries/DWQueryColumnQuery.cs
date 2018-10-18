

using System.Linq;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Logitude.BL.InfrastructureModel.EntityLists;
using Logitude.BL.InfrastructureModel.EntityPMs;
using System.Collections.Generic;

namespace Logitude.BL.InfrastructureModel.EntityQueries
{
    public class DWQueryColumnQuery
    {
        DWQueryColumnRepository repository;

        public DWQueryColumnQuery()
        {
            repository = new DWQueryColumnRepository();
        }

        public DWQueryColumnQuery(int tenant)
        {
            repository = new DWQueryColumnRepository(tenant);
        }

        public DWQueryColumnQuery(DWQueryColumnRepository DWQueryColumnRepository)
        {
            repository = DWQueryColumnRepository;
        }

        public DWQueryColumnPM GetSingleDWQueryColumnPM(string id, int tenant)
        {
            return (from a in repository.webFreightContext.DWQueryColumns
                    where a.Id == id && a.Tenant == tenant
                    select new DWQueryColumnPM()
                    {
                        Id = a.Id,
                        Tenant = a.Tenant,
                        ColumnWidth = a.ColumnWidth,
                        DWObjectFieldId = a.DWObjectFieldId,
                        DWQueryId = a.DWQueryId,
                        IndexOrder = a.IndexOrder,
                        UserId = a.UserId,
                    }).FirstOrDefault();
        }


        public IQueryable<DWQueryColumnPM> GetDWQueryColumnPMsByTenant(int tenant)
        {
            return (from a in repository.webFreightContext.DWQueryColumns
                    where a.Tenant == tenant
                    select new DWQueryColumnPM()
                    {

                        Id = a.Id,
                        Tenant = a.Tenant,
                        ColumnWidth = a.ColumnWidth,
                        DWObjectFieldId = a.DWObjectFieldId,
                        DWQueryId = a.DWQueryId,
                        IndexOrder = a.IndexOrder,
                        UserId = a.UserId,
                    }
                  );
        }

        public DWQueryColumnPM GetSinglePM(string id, int tenant)
        {
            return (from a in repository.webFreightContext.DWQueryColumns
                    where a.Id == id && a.Tenant == tenant
                    select new DWQueryColumnPM()
                    {
                        Id = a.Id,
                        Tenant = a.Tenant,
                        ColumnWidth = a.ColumnWidth,
                        DWObjectFieldId = a.DWObjectFieldId,
                        DWQueryId = a.DWQueryId,
                        IndexOrder = a.IndexOrder,
                        UserId = a.UserId,
                    }).FirstOrDefault();
        }

        public IQueryable<DWQueryColumnPM> GetDWQueryColumnPMs(int tenant)
        {
            return (from a in repository.webFreightContext.DWQueryColumns
                    where a.Tenant == tenant
                    select new DWQueryColumnPM()
                    {
                        Id = a.Id,
                        Tenant = a.Tenant,
                        ColumnWidth = a.ColumnWidth,
                        DWObjectFieldId = a.DWObjectFieldId,
                        DWQueryId = a.DWQueryId,
                        IndexOrder = a.IndexOrder,
                        UserId = a.UserId,
                    });
        }

        public IQueryable<DWQueryColumnList> GetIQueryableEntityList(IQueryable<DWQueryColumn> iQueryable)
        {
            IQueryable<DWQueryColumnList> result = from a in iQueryable
                                                   select new DWQueryColumnList()
                                                   {
                                                       Id = a.Id,
                                                       Tenant = a.Tenant,
                                                       ColumnWidth = a.ColumnWidth,
                                                       DWObjectFieldId = a.DWObjectFieldId,
                                                       DWQueryId = a.DWQueryId,
                                                       IndexOrder = a.IndexOrder,
                                                       UserId = a.UserId,
                                                   };

            return result;
        }




        public IQueryable<DWQueryColumnPM> GetDWQueryColumnsBydWQueryIdAndUserAngular(int tenant, string userId, string dwQueryId)
        {
            IQueryable<DWQueryColumnPM> queries = null;
           
            queries = from a in repository.webFreightContext.DWQueryColumns
                      where a.Tenant == tenant && a.UserId == userId && a.DWQueryId == dwQueryId
                      select new DWQueryColumnPM()
                      {
                          Id = a.Id,
                          Tenant = a.Tenant,
                          ColumnWidth = a.ColumnWidth,
                          DWObjectFieldId = a.DWObjectFieldId,
                          DWQueryId = a.DWQueryId,
                          IndexOrder = a.IndexOrder,
                          UserId = a.UserId,

                      };
            List<DWQueryColumnPM> Cols = new List<DWQueryColumnPM>();
            foreach (var item in queries)
            {
                if (!Cols.Contains(item))
                {
                    Cols.Add(item);
                }
            }
            return Cols.AsQueryable();
        }


    }
}
