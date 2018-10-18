using Logitude.WarehouseLib.Data.EntityLists;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.WarehouseLib.BL.EntityQueryServices
{
    public partial class WarehouseEntryPackagesReleaseQueryService
    {
        public List<WarehouseEntryPackagesReleaseList> GetWarehouseEntryPackagePMListsByCustomerIdIdAndWarehouseId(List<string> warehouseEntryPackageIds, int tenant)
        {

            List<WarehouseEntryPackagesReleaseList> myResult = (from a in context.WarehouseEntryPackagesReleases
                                                                where a.Tenant == tenant && warehouseEntryPackageIds.Contains(a.EntryPackageId)
                                                                select new WarehouseEntryPackagesReleaseList()
                                                                {
                                                                    ReleasePackageId = a.ReleasePackageId,
                                                                    EntryPackageId = a.EntryPackageId,
                                                                }).ToList();
            return myResult;
        }
    }
}

