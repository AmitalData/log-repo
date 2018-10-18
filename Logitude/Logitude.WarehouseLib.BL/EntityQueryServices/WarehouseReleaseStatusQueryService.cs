using Logitude.WarehouseLib.Data.EntityLists;
using Logitude.WarehouseLib.Data.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.WarehouseLib.BL.EntityQueryServices
{
    public partial class WarehouseReleaseStatusQueryService
    {
        public IQueryable<WarehouseReleaseStatusList> GetIQueryableEntityList(IQueryable<WarehouseReleaseStatus> entities)
        {
            var myResult = (from entity in entities
                            select new WarehouseReleaseStatusList()
                            {
                                Code = entity.Code,
                                Name = entity.Name,
                                SearchFields = entity.SearchFields,
                            });

            return myResult;
        }
    }
}
