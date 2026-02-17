using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Global.Data.GlobalModel.Repositories;

namespace Logitude.BL.GlobalModel.EntityQueries
{
    public class PackageConnectedPackageQuery
    {
        PackageConnectedPackageRepository repository;

        public PackageConnectedPackageQuery()
        {
            repository = new PackageConnectedPackageRepository(); 
        }

        public PackageConnectedPackageQuery(PackageConnectedPackageRepository repository)
        {
            this.repository = repository;
        }

        public IQueryable<PackageConnectedPackagePM> GetConnectedPackagePMs()
        {
            return (from a in repository.context.PackageConnectedPackages.Include("ConnectedPackage")
                    select new PackageConnectedPackagePM()
                    {
                        Id = a.Id,
                        PackageCode = a.PackageCode,
                        ConnectedPackageCode = a.ConnectedPackageCode,
                        ConnectedPackageName = a.ConnectedPackage == null ? null : a.ConnectedPackage.Name,
                    });
        }

        public IQueryable<PackageConnectedPackagePM> GetConnectedPackageByPackageCode(string packageCode)
        {
            return (from a in repository.context.PackageConnectedPackages.Include("ConnectedPackage")
                    where a.PackageCode == packageCode
                    select new PackageConnectedPackagePM()
                    {
                        Id = a.Id,
                        PackageCode = a.PackageCode,
                        ConnectedPackageCode = a.ConnectedPackageCode,
                        ConnectedPackageName = a.ConnectedPackage == null ? null : a.ConnectedPackage.Name,
                    });
        }

        public PackageConnectedPackagePM GetSinglePM(string id)
        {
            var package = (from a in repository.context.PackageConnectedPackages.Include("ConnectedPackage")
                           where a.Id == id
                           select new PackageConnectedPackagePM()
                           {
                               Id = a.Id,
                               PackageCode = a.PackageCode,
                               ConnectedPackageCode = a.ConnectedPackageCode,
                               ConnectedPackageName = a.ConnectedPackage == null ? null : a.ConnectedPackage.Name,
                           }).FirstOrDefault();
            return package;
        }
    }
}
