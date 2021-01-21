using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.Helpers
{
    public class ObjectTablePropertyGetter : IObjectTablePropertyGetter
    {
        public string GetKeyPropertyPath(string objectTableName,int tenant)
        {
            ObjectTableRepository objectTableRepository = new ObjectTableRepository(tenant);
            ObjectTable objectTable = objectTableRepository.GetObjectTableByName(objectTableName, tenant, true);

            return objectTable.KeyPropertyPath;
        }
    }
}
