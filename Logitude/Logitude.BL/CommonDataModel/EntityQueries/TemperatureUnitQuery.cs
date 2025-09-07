using Logitude.BL.CommonDataModel.EntityLists;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.EntityQueries
{
    public class TemperatureUnitQuery
    {
        TemperatureUnitRepository repository;


        public TemperatureUnitQuery(int tenant)
        {
            repository = new TemperatureUnitRepository(tenant);
        }

        public TemperatureUnitQuery(TemperatureUnitRepository repository)
        {
            this.repository = repository;
        }

        public IQueryable<TemperatureUnitList> GetIQueryableEntityList(IQueryable<TemperatureUnit> iQueryable)
        {
            IQueryable<TemperatureUnitList> result = from entity in iQueryable
                                                     select new TemperatureUnitList()
                                                     {
                                                         Name = entity.Name,
                                                         Code = entity.Code,
                                                         SearchFields = entity.SearchFields,
                                                     };
            return result;
        }

    }
}
