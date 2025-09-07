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
    public class WarehouseWeightMeasurementQuery
    {
        WarehouseWeightMeasurementRepository repository;

   

        public WarehouseWeightMeasurementQuery(int tenant)
        {
            repository = new WarehouseWeightMeasurementRepository(tenant);
        }

        public WarehouseWeightMeasurementQuery(WarehouseWeightMeasurementRepository communicationLogTypeRepository)
        {
            repository = communicationLogTypeRepository;
        }
        
        public IQueryable<WarehouseWeightMeasurementList> GetIQueryableEntityList(IQueryable<WarehouseWeightMeasurement> iQueryable)
        {
            IQueryable<WarehouseWeightMeasurementList> result = from entity in iQueryable
                                                   select new WarehouseWeightMeasurementList()
                                                   {
                                                       Name = entity.Name,
                                                       Code = entity.Code,
                                                       SearchFields = entity.SearchFields,
                                                   };
            return result;
        }
    }
}
