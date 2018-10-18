using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.DataMapping;
using Logitude.BL.CommonDataModel.Tools.Validating;
using Logitude.BL.Helpers;
using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.Tools.EntityService
{
    public class PackageService
    {
        bool isNewEntity;
        private int tenant;
        public Package Poco { get; set; }
        private PackagePM entityPM;
        private ICommonDataContext objectContext;
        private PackageRepository entityRepository;
        private PackageConnectedPackageRepository connectedPackageRepository;  
        public PackageService(ICommonDataContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.objectContext = objectContext;
            this.entityRepository = new PackageRepository(objectContext);
            this.connectedPackageRepository = new PackageConnectedPackageRepository(objectContext);
        }

        private List<PackageConnectedPackagePM> connectedPackageSet;
        public void SetChangeSet(List<PackageConnectedPackagePM> connectedPackageSet)
        {
            this.connectedPackageSet = connectedPackageSet;
        }

        public void Create(PackagePM entityPM)
        {
            this.isNewEntity = true;
            this.entityPM = entityPM;
            this.Poco = new Package();

            this.ValidatePackageExistence();

            PackageValidating.Validate(entityPM, isNewEntity);
            PackageMapping.MapEntity(entityPM, Poco, isNewEntity);

            this.UpdateConnectedPackages();

            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();
            TableLastUpdateClass.UpdateTableHistory(tenant, "Package");
        }

        public void Update(PackagePM entityPM, bool mapComposition = false)
        {
            this.isNewEntity = false;
            this.entityPM = entityPM;
            this.Poco = entityRepository.GetSinglePackage(entityPM.Code);

            if (mapComposition)
            {
                this.connectedPackageSet = this.entityPM.ConnectedPackages;
            }

            PackageValidating.Validate(entityPM, isNewEntity);
            PackageMapping.MapEntity(entityPM, Poco, isNewEntity);

            this.UpdateConnectedPackages();

            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
            TableLastUpdateClass.UpdateTableHistory(tenant, "Package");
        }

        private void ValidatePackageExistence()
        {
            if (isNewEntity)
            {
                bool isAlreadyExists = (from d in entityRepository.context.Packages
                                        where d.Code == this.entityPM.Code
                                        select d).Any();

                if (isAlreadyExists)
                {
                    throw new ApplicationException("Package with same code already exists");
                }
            }
        }

        private void UpdateConnectedPackages()
        {
            if (isNewEntity)
            {
                foreach (PackageConnectedPackagePM itemPM in entityPM.ConnectedPackages)
                {
                    this.CreateConnectedPackage(itemPM);
                }
            }

            else
            {
                if (connectedPackageSet != null)
                {
                    foreach (PackageConnectedPackagePM itemPM in connectedPackageSet)
                    {
                        switch (itemPM.ChangeSetOp)
                        {
                            case ChangeSetOperation.Insert:
                                {
                                    this.CreateConnectedPackage(itemPM);
                                    break;
                                }

                            case ChangeSetOperation.Update:
                                {
                                    this.UpdateConnectedPackage(itemPM);
                                    break;
                                }

                            case ChangeSetOperation.Delete:
                                {
                                    this.DeleteConnectedPackage(itemPM);
                                    break;
                                }

                            default: { break; }
                        }
                    }
                }
            }
        }

        private void CreateConnectedPackage(PackageConnectedPackagePM itemPM)
        {
            if (itemPM.Id == null)
            {
                itemPM.Id = IdCounter.GetNumber("PackageConnectedPackage", tenant).ToString();
                itemPM.PackageCode = entityPM.Code;

                PackageConnectedPackage itemPoco = new PackageConnectedPackage()
                {
                    Id = itemPM.Id,
                    PackageCode = itemPM.PackageCode,
                    ConnectedPackageCode = itemPM.ConnectedPackageCode,
                };

                connectedPackageRepository.Add(itemPoco);
            }
        }
        private void UpdateConnectedPackage(PackageConnectedPackagePM itemPM)
        {
            PackageConnectedPackage itemPoco = connectedPackageRepository.GetSinglePackageConnectedPackage(itemPM.Id);

            if (itemPoco != null)
            {
                itemPoco.PackageCode = entityPM.Code;
                itemPoco.ConnectedPackageCode = itemPM.ConnectedPackageCode;
                connectedPackageRepository.Update(itemPoco);
            }
        }
        private void DeleteConnectedPackage(PackageConnectedPackagePM itemPM)
        {
            PackageConnectedPackage itemPoco = connectedPackageRepository.GetSinglePackageConnectedPackage(itemPM.Id);

            if (itemPoco != null)
            {
                connectedPackageRepository.Remove(itemPoco);
            }
        }
    }
}
