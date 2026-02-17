using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InfrastructureModel.CustomFilters
{
    public class EntityStatusCustomFilter
    {
        private int tenant;
        public EntityStatusCustomFilter(int tenant)
        {
            this.tenant = tenant;
        }

        public IQueryable<EntityStatus> GetFilteredQuery(QueryOperations operations, IQueryable<EntityStatus> queryableData)
        {
            return queryableData;
        }
    }
}
