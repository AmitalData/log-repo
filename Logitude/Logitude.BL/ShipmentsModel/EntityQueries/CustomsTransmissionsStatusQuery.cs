using Logitude.BL.ShipmentsModel.EntityLists;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.ShipmentsModel.EntityQueries
{
    public class CustomsTransmissionsStatusQuery
    {
        private CustomsTransmissionsStatusRepository repository;

        public CustomsTransmissionsStatusQuery(int tenant)
        {
            repository = new CustomsTransmissionsStatusRepository(tenant);
        }

        public CustomsTransmissionsStatusQuery(CustomsTransmissionsStatusRepository repository)
        {
            this.repository = repository;
        }

        public IQueryable<CustomsTransmissionsStatusList> GetIQueryableEntityList(IQueryable<CustomsTransmissionsStatus> iQueryable)
        {
            IQueryable<CustomsTransmissionsStatusList> result = from entity in iQueryable
                                                    select new CustomsTransmissionsStatusList()
                                                    {
                                                        Code = entity.Code,
                                                        Name = entity.Name,
                                                        SearchFields = entity.SearchFields,
                                                    };
            return result;
        }
    }
}
