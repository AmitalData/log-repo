using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Simplog.Data.InfrastructureModel.Repositories;

using Logitude.BL.InfrastructureModel.EntityLists;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Logitude.BL.InfrastructureModel.EntityPMs;

namespace Logitude.BL.InfrastructureModel.EntityQueries
{
    public class IATACodeQuery
    {
        IATACodeRepository repository;
        public IATACodeQuery()
        {
            repository = new IATACodeRepository(); 
        }

        public IATACodeQuery(int tenant)
        {
            repository = new IATACodeRepository(tenant);
        }

        public IATACodeQuery(IATACodeRepository iataCodeRepository)
        {
            repository = iataCodeRepository;
        }

        public IATACodePM GetSinglePM(string id)
        {
            return (from a in repository.context.IATACodes
                    where a.Id == id
                    select new IATACodePM()
                    {
                        Id = a.Id,
                        Code = a.Code,
                        Name = a.Name,
                        MeasurementCode = a.MeasurementCode,
                        DueTypeCode = a.DueTypeCode,
                        SearchFields = a.SearchFields,
                        IsIATA = a.IsIATA,
                        InActive = a.InActive,
                        AirlineId = a.AirlineId,
                    }).FirstOrDefault();
        }

        public IATACodePM GetSingleIATACodePM(string id)
        {
            return (from a in repository.context.IATACodes
                    where a.Id == id
                    select new IATACodePM()
                    {
                        Id = a.Id,
                        Code = a.Code,
                        Name = a.Name,
                        MeasurementCode = a.MeasurementCode,
                        DueTypeCode = a.DueTypeCode,
                        SearchFields = a.SearchFields,
                        IsIATA = a.IsIATA,
                        InActive = a.InActive,
                        AirlineId = a.AirlineId,
                    }).FirstOrDefault();
        }

        public IQueryable<IATACodePM> GetIATACodePMs()
        {
            return from a in repository.context.IATACodes
                   select new IATACodePM()
                   {
                       Id = a.Id,
                       Code = a.Code,
                       Name = a.Name,
                       MeasurementCode = a.MeasurementCode,
                       DueTypeCode = a.DueTypeCode,
                       SearchFields = a.SearchFields,
                       IsIATA = a.IsIATA,
                       InActive = a.InActive,
                       AirlineId = a.AirlineId,
                   };
        }


        public IQueryable<IATACodeList> GetIQueryableEntityList(IQueryable<IATACode> iQueryable)
        {
            IQueryable<IATACodeList> result = from a in iQueryable
                                              select new IATACodeList()
                                              {
                                                  Id = a.Id,
                                                  Code = a.Code,
                                                  Name = a.Name,
                                                  MeasurementCode = a.MeasurementCode,
                                                  DueTypeCode = a.DueTypeCode,
                                                  SearchFields = a.SearchFields,
                                                  IsIATA = a.IsIATA,
                                                  InActive = a.InActive,
                                                  AirlineId = a.AirlineId,
                                              };
            return result;
        }
    }
}