 
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
   public partial class WarehouseEntryPackageRepository:IRepository<WarehouseEntryPackage>
   {
        
		public List<WarehouseEntryPackage> GetMulti(EntityKeyFields entityKeys)
        {

            WarehouseEntryKeys myEntityKeys = entityKeys as WarehouseEntryKeys;
            return (from a in context.WarehouseEntryPackages where a.WarehouseEntryId == myEntityKeys.Id select a).ToList();
        }

        public List<WarehouseEntryPackage>  GetWarehouseEntryPackageByIds(List<string> warehouseEntryPackageids, int tenant)
        {
            return (from a in context.WarehouseEntryPackages
                    where warehouseEntryPackageids.Contains(a.Id) && a.Tenant == tenant
                    select a).ToList();
        }




    }

}
   