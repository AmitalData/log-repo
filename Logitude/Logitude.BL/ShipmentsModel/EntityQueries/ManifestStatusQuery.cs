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
    public class ManifestStatusQuery
    {
        ManifestStatusRepository repository;
        public ManifestStatusQuery(int tenant)
        {
            repository = new ManifestStatusRepository(tenant);
        }

        public ManifestStatusQuery(ManifestStatusRepository repository)
        {
            this.repository = repository;
        }

        public IQueryable<ManifestStatusList> GetIQueryableEntityList(IQueryable<ManifestStatus> iQueryable)
        {
            IQueryable<ManifestStatusList> result = from entity in iQueryable
                                                    select new ManifestStatusList()
                                                           {
                                                               Code = entity.Code,
                                                               Name = entity.Name,
                                                               SearchFields = entity.SearchFields,
                                                           };
            return result;
        }

    }
}
