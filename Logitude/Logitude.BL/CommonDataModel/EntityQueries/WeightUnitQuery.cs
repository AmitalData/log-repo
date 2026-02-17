using System;
using System.Linq;
using System.Collections.Generic;
using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.BL.CommonDataModel.EntityLists;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;

namespace Logitude.BL.CommonDataModel.EntityQueries
{
    public class WeightUnitQuery
    {
        WeightUnitRepository repository;

        public WeightUnitQuery()
        {
            repository = new WeightUnitRepository();
        }

        public WeightUnitQuery(int tenant)
        {
            repository = new WeightUnitRepository(tenant);
        }

        public WeightUnitQuery(WeightUnitRepository repository)
        {
            this.repository = repository;
        }

        public WeightUnitPM GetSinglePM(string code, int tenant = 0)
        {
            return (from a in repository.context.WeightUnits
                    where a.Code == code
                    select new WeightUnitPM()
                    {
                        Code = a.Code,
                        Name = a.Name,
                        PrintAs = a.PrintAs,
                        SearchFields = a.SearchFields,
                    }).FirstOrDefault();
        }

        public WeightUnitPM GetSingleWeightUnitPM(string code)
        {
            var wUnit = (from a in repository.context.WeightUnits
                         where a.Code == code
                         select new WeightUnitPM()
                         {
                             Code = a.Code,
                             Name = a.Name,
                             PrintAs = a.PrintAs,
                             SearchFields = a.SearchFields,
                         }).FirstOrDefault();
            return wUnit;
        }

        public IQueryable<WeightUnitPM> GetWeightUnitPMs()
        {
            return (from a in repository.context.WeightUnits
                    select new WeightUnitPM()
                    {
                        Code = a.Code,
                        Name = a.Name,
                        PrintAs = a.PrintAs,
                        SearchFields = a.SearchFields,
                    });
        }

        public IQueryable<WeightUnitList> GetIQueryableEntityList(IQueryable<WeightUnit> iQueryable)
        {
            IQueryable<WeightUnitList> result = from entity in iQueryable
                                                select new WeightUnitList()
                                                {
                                                    Name = entity.Name,
                                                    Code = entity.Code,
                                                    PrintAs = entity.PrintAs,
                                                    SearchFields = entity.SearchFields,
                                                };
            return result;
        }

    }
}