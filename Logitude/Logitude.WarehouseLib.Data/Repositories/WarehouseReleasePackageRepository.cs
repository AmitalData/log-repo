 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.WarehouseLib.Data.EntityPOCOs;
using Logitude.WarehouseLib.Data.EntityKeys;
using Simplog.Server.Infrastructure;

namespace Logitude.WarehouseLib.Data.Repositories
{
   public partial class WarehouseReleasePackageRepository:IRepository<WarehouseReleasePackage>
   {
        
		public List<WarehouseReleasePackage> GetMulti(EntityKeyFields entityKeys)
        {
            WarehouseReleaseKeys myEntityKeys = entityKeys as WarehouseReleaseKeys;
            return (from a in context.WarehouseReleasePackages where a.WarehouseReleaseId == myEntityKeys.Id select a).ToList();
        }

        public List<WarehouseReleasePackage> GetWarehouseReleasesPackagesByReleaseId(string releaseId, int tenant)
        {
            List<WarehouseReleasePackage> myResult = (from a in context.WarehouseReleasePackages
                                                            where a.WarehouseReleaseId == releaseId && a.Tenant == tenant
                                                     select a).ToList();
            return myResult;
        }
    }

}
   