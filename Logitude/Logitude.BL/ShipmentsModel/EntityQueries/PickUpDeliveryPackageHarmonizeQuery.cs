using Logitude.BL.ShipmentsModel.EntityPMs;
using Simplog.Data.ShipmentsModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.ShipmentsModel.EntityQueries
{
    public class PickUpDeliveryPackageHarmonizeQuery
    {
        PickUpDeliveryPackageHarmonizeRepository repository;
        public PickUpDeliveryPackageHarmonizeQuery(int tenant)
        {
            repository = new PickUpDeliveryPackageHarmonizeRepository(tenant);
        }

        public PickUpDeliveryPackageHarmonizeQuery(PickUpDeliveryPackageHarmonizeRepository myRepository)
        {
            repository = myRepository;
        }

        public List<PickUpDeliveryPackageHarmonizePM> GetPickUpDeliveryPackageHarmonizes(string packageId, int tenant)
        {
            List<PickUpDeliveryPackageHarmonizePM> myResult = (from a in repository.context.PickUpDeliveryPackageHarmonizes
                                                               where a.Tenant == tenant && a.PackageId == packageId
                                                         select new PickUpDeliveryPackageHarmonizePM()
                                                         {
                                                             Id = a.Id,
                                                             Tenant = a.Tenant,
                                                             PackageId = a.PackageId,
                                                             Harmonize = a.Harmonize,
                                                         }).OrderBy(a => a.Id).ToList();

            return myResult;
        }
    }
}
