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
    public class SchedulerProcedureQuery
    {
        SchedulerProcedureRepository repository;
        public SchedulerProcedureQuery()
        {
            repository = new SchedulerProcedureRepository();
        }

        public SchedulerProcedureQuery(int tenant)
        {
            repository = new SchedulerProcedureRepository(tenant);
        }

        public SchedulerProcedureQuery(SchedulerProcedureRepository SchedulerProcedureRepository)
        {
            repository = SchedulerProcedureRepository;
        }


        public SchedulerProcedurePM GetSingleSchedulerProcedurePM(string Code)
        {
            return (from a in repository.context.SchedulerProcedures
                    where a.Code == Code
                    select new SchedulerProcedurePM()
                    {
                        Code = a.Code,
                        Name = a.Name,
                        SearchFields = a.SearchFields,
                        Description = a.Description,
                        IsInternallyDefined = a.IsInternallyDefined
                    }).FirstOrDefault();
        }



        public List<SchedulerProcedurePM> GetSchedulerProcedurePMs(string Code)
        {
            return (from a in repository.context.SchedulerProcedures
                    where a.Code == Code
                    select new SchedulerProcedurePM()
                    {
                        Code = a.Code,
                        Name = a.Name,
                        SearchFields = a.SearchFields,
                        Description = a.Description,
                        IsInternallyDefined = a.IsInternallyDefined

        }).ToList();
        }

   

        public IQueryable<SchedulerProcedureList> GetIQueryableEntityList(IQueryable<SchedulerProcedure> iQueryable) {
            IQueryable<SchedulerProcedureList> result = from a in iQueryable
                                                        where a.IsInternallyDefined == false
                                                        select new SchedulerProcedureList()
                                                        {
                                                            Code = a.Code,
                                                            Name = a.Name,
                                                            SearchFields = a.SearchFields,
                                                            Description = a.Description,
                                                            IsInternallyDefined = a.IsInternallyDefined
                                                          };
            return result;
        }

    }
}