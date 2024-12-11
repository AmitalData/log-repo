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
    public class WarehouseWeightRoundingQuery
    {
        WarehouseWeightRoundingRepository repository;

        public WarehouseWeightRoundingQuery()
        {
            repository = new WarehouseWeightRoundingRepository();
        }

        public WarehouseWeightRoundingQuery(int tenant)
        {
            repository = new WarehouseWeightRoundingRepository(tenant);
        }

        public WarehouseWeightRoundingQuery(WarehouseWeightRoundingRepository communicationLogTypeRepository)
        {
            repository = communicationLogTypeRepository;
        }
        
        public IQueryable<WarehouseWeightRoundingList> GetIQueryableEntityList(IQueryable<WarehouseWeightRounding> iQueryable)
        {
            IQueryable<WarehouseWeightRoundingList> result = from entity in iQueryable
                                                   select new WarehouseWeightRoundingList()
                                                   {
                                                       Name = entity.Name,
                                                       Code = entity.Code,
                                                       SearchFields = entity.SearchFields,
                                                       Display = entity.Display,
                                                   };
            return result;
        }
    }
}
