using Logitude.BL.ShipmentsModel.EntityPMs;
using Simplog.Data.ShipmentsModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.ShipmentsModel.EntityQueries
{
    public class ShipmentPackageHarmonizeQuery
    {
        ShipmentPackageHarmonizeRepository repository;
        public ShipmentPackageHarmonizeQuery(int tenant)
        {
            repository = new ShipmentPackageHarmonizeRepository(tenant);
        }
        public ShipmentPackageHarmonizeQuery(ShipmentPackageHarmonizeRepository myRepository)
        {
            repository = myRepository;
        }

        public List<ShipmentPackageHarmonizePM> GetShipmentPackageHarmonizes(string shipmentPackageId, int tenant)
        {
            List<ShipmentPackageHarmonizePM> myResult = (from a in repository.context.ShipmentPackageHarmonizes
                                                         where a.Tenant == tenant && a.PackageId == shipmentPackageId && string.IsNullOrEmpty(a.InsidePackageId)
                                                         select new ShipmentPackageHarmonizePM()
                                                         {
                                                             Id = a.Id,
                                                             Tenant = a.Tenant,
                                                             PackageId = a.PackageId,
                                                             Harmonize = a.Harmonize,
                                                             InsidePackageId = a.InsidePackageId,
                                                         }).OrderBy(a => a.Id).ToList();

            return myResult;
        }

        public List<ShipmentPackageHarmonizePM> GetInsideShipmentPackageHarmonizes(string insidePackageId, int tenant)
        {
            List<ShipmentPackageHarmonizePM> myResult = (from a in repository.context.ShipmentPackageHarmonizes
                                                         where a.Tenant == tenant && a.InsidePackageId == insidePackageId
                                                         select new ShipmentPackageHarmonizePM()
                                                         {
                                                             Id = a.Id,
                                                             Tenant = a.Tenant,
                                                             PackageId = a.PackageId,
                                                             Harmonize = a.Harmonize,
                                                             InsidePackageId = a.InsidePackageId,
                                                         }).OrderBy(a => a.Id).ToList();

            return myResult;
        }
    }
}
