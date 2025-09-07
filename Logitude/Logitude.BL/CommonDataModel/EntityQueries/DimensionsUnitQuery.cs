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
    public class DimensionsUnitQuery
    {
        DimensionsUnitRepository repository;



        public DimensionsUnitQuery(int tenant)
        {
            repository = new DimensionsUnitRepository(tenant);
        }

        public DimensionsUnitQuery(DimensionsUnitRepository repository)
        {
            this.repository = repository;
        }

        public DimensionsUnitPM GetSinglePM(string code, int tenant = 0)
        {
            return (from a in repository.context.DimensionsUnits
                    where a.Code == code
                    select new DimensionsUnitPM()
                    {
                        Code = a.Code,
                        Name = a.Name,
                        SearchFields = a.SearchFields,
                        PrintAs = a.PrintAs
                    }).FirstOrDefault();
        }

        public IQueryable<DimensionsUnitPM> GetDimensionsUnitPMs()
        {
            return from a in repository.context.DimensionsUnits
                   select new DimensionsUnitPM()
                   {
                       Code = a.Code,
                       Name = a.Name,
                       SearchFields = a.SearchFields,
                       PrintAs = a.PrintAs
                   };
        }

        public IQueryable<DimensionsUnitList> GetIQueryableEntityList(IQueryable<DimensionsUnit> iQueryable)
        {
            IQueryable<DimensionsUnitList> result = from entity in iQueryable
                                                    select new DimensionsUnitList()
                                                    {
                                                        Name = entity.Name,
                                                        Code = entity.Code,
                                                        SearchFields = entity.SearchFields,
                                                        PrintAs = entity.PrintAs
                                                    };
            return result;
        }

        public DimensionsUnitPM GetSinglePMByCode(string code, int tenant = 0)
        {
            return (from a in repository.context.DimensionsUnits
                    where a.Code == code
                    select new DimensionsUnitPM()
                    {
                        Code = a.Code,
                        Name = a.Name,
                        SearchFields = a.SearchFields,
                        PrintAs = a.PrintAs
                    }).FirstOrDefault();
        }
    }
}