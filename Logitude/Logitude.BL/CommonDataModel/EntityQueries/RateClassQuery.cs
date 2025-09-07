using System;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using Logitude.BL.Helpers;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityLists;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;

namespace Logitude.BL.CommonDataModel.EntityQueries
{
    public class RateClassQuery
    {
        RateClassRepository repository;



        public RateClassQuery(int tenant)
        {
            repository = new RateClassRepository(tenant);
        }

        public RateClassQuery(RateClassRepository repository)
        {
            this.repository = repository;
        }

        public RateClassPM GetSinglePM(string code)
        {
            return (from a in repository.context.RateClasses
                    where a.Code == code
                    select new RateClassPM()
                    {
                        Code = a.Code,
                        Name = a.Name,
                        SearchFields = a.SearchFields,
                    }).FirstOrDefault();
        }

        public IQueryable<RateClassPM> GetRateClassPMs()
        {
            return from a in repository.context.RateClasses
                   select new RateClassPM()
                   {
                       Code = a.Code,
                       Name = a.Name,
                       SearchFields = a.SearchFields,
                   };
        }

        public IQueryable<RateClassList> GetIQueryableEntityList(IQueryable<RateClass> iQueryable)
        {
            IQueryable<RateClassList> result = from entity in iQueryable
                                               select new RateClassList()
                                               {
                                                   Name = entity.Name,
                                                   Code = entity.Code,
                                                   SearchFields = entity.SearchFields,
                                               };
            return result;
        }
    }
}