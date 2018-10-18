 
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
   public partial class WarehouseEntryPackagesReleaseRepository:IRepository<WarehouseEntryPackagesRelease>
   {
        
		public List<WarehouseEntryPackagesRelease> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }



        public List<WarehouseEntryPackagesRelease> GetWarehouseEntryPackagesReleaseByReleasePackageIds(List<string> warehouseReleasePackageids, int tenant)
        {
            return (from a in context.WarehouseEntryPackagesReleases
                    where  warehouseReleasePackageids.Contains(a.ReleasePackageId)  && a.Tenant == tenant
                    select a).ToList();
        }

    }

}
   