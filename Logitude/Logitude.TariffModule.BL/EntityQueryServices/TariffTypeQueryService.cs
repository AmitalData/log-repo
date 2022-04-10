using Logitude.TariffModule.Data.EntityLists;
using Logitude.TariffModule.Data.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.TariffModule.BL.EntityQueryServices
{
    public partial  class TariffTypeQueryService
    {
        public IQueryable<TariffTypeList> GetIQueryableEntityList(IQueryable<TariffType> entities)
        {
            var myResult = (from entity in entities
                            select new TariffTypeList()
                            {
                                Code = entity.Code,
                                Name = entity.Name,
                                SearchFields = entity.SearchFields,
                                DirectionCode = entity.DirectionCode,
                                TransportModeCode = entity.TransportModeCode
                            });

            return myResult;
        }
    }
}
