using Logitude.BL.InfrastructureModel.EntityLists;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.Tools.EntityService;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Logitude.BL.InfrastructureModel.EntityQueries
{
    public class DeploymentPackageQuery
    {
        private DeploymentPackageRepository repository;
        private int tenant;
        public DeploymentPackageQuery(int tenant)
        {
            repository = new DeploymentPackageRepository(tenant);
            this.tenant = tenant;
        }
        public DeploymentPackageQuery(DeploymentPackageRepository deploymentPackageRepository)
        {
            repository = deploymentPackageRepository;
        }
        public DeploymentPackagePM GetSinglePM(string id, int tenant)
        {
            DeploymentPackage deploymentPackage = repository.GetSingleDeploymentPackage(id, tenant);
            DeploymentPackagePM result = MapDeploymentPackageToDeploymentPackagePM(deploymentPackage, new DeploymentPackagePM());
            return result;
        }
        public IQueryable<DeploymentPackageList> GetIQueryableEntityList(IQueryable<DeploymentPackage> iQueryable)
        {

            IQueryable<DeploymentPackageList>
                result = from a in iQueryable.Include("CreatedByUser.Contact").Include("UpdatedByUser.Contact")
                         select new DeploymentPackageList()
                         {
                             Id = a.Id,
                             Tenant = a.Tenant,
                             InActive = a.InActive,
                             DirectionId = a.DirectionId,
                             Description = a.Description,
                             Name = a.Name,
                             Code = a.Code,
                             CreatedBy = a.CreatedBy,
                             CreateDate = a.CreateDate,
                             UpdatedBy = a.UpdatedBy,
                             UpdateDate = a.UpdateDate,
                             SearchFields = a.SearchFields,
                             CreatedByUserName = (a.CreatedByUser!=null && a.CreatedByUser.Contact!=null) ? a.CreatedByUser.Contact.EnglishName:null,
                             UpdatedByUserName = (a.UpdatedByUser != null && a.UpdatedByUser.Contact != null) ? a.UpdatedByUser.Contact.EnglishName : null,

                         };

            return result;
        }



        private DeploymentPackagePM MapDeploymentPackageToDeploymentPackagePM(DeploymentPackage deploymentPackage, DeploymentPackagePM deploymentPackagePM)
        {
            deploymentPackagePM.Id = deploymentPackage.Id;
            deploymentPackagePM.Tenant = deploymentPackage.Tenant;
            deploymentPackagePM.CreateDate = deploymentPackage.CreateDate;
            deploymentPackagePM.CreatedBy = deploymentPackage.CreatedBy;
            deploymentPackagePM.UpdatedBy = deploymentPackage.UpdatedBy;
            deploymentPackagePM.UpdateDate = deploymentPackage.UpdateDate;
            deploymentPackagePM.SearchFields = deploymentPackage.SearchFields;
            deploymentPackagePM.Name = deploymentPackage.Name;
            deploymentPackagePM.Code = deploymentPackage.Code;
            deploymentPackagePM.InActive = deploymentPackage.InActive;
            deploymentPackagePM.Description = deploymentPackage.Description;
            deploymentPackagePM.DirectionId = deploymentPackage.DirectionId;
            deploymentPackagePM.CreatedByUserName = (deploymentPackage.CreatedByUser != null && deploymentPackage.CreatedByUser.Contact != null) ? deploymentPackage.CreatedByUser.Contact.EnglishName : null ;
            deploymentPackagePM.UpdatedByUserName = (deploymentPackage.UpdatedByUser != null && deploymentPackage.UpdatedByUser.Contact != null) ? deploymentPackage.UpdatedByUser.Contact.EnglishName : null;
            deploymentPackagePM.VersionId = deploymentPackage.VersionId;
            deploymentPackagePM.DocumentId = deploymentPackage.DeploymentPackagesVersion != null ? deploymentPackage.DeploymentPackagesVersion.DocumentId : null;
            deploymentPackagePM.IsExported = deploymentPackage.DeploymentPackagesVersion != null ? deploymentPackage.DeploymentPackagesVersion.IsExported : false;
            MapDeploymentPackageDetails(deploymentPackagePM);
            return deploymentPackagePM;
        }

        public void MapDeploymentPackageDetails(DeploymentPackagePM deploymentPackagePM)
        {
            if (deploymentPackagePM.DocumentId == null) return;
            StorageDataArgs storageDataArgs = GetStorageDataArgs(deploymentPackagePM, tenant);
            byte[] deploymentPackageZipFileDetailsBytes = StorageDataService.ReadFileFromStorage(storageDataArgs);
            deploymentPackagePM.DeploymentPackageDetails = new DeploymentPackageExtractDetailsService().Extract(deploymentPackageZipFileDetailsBytes);
        }

        private StorageDataArgs GetStorageDataArgs(DeploymentPackagePM deploymentPackagePM, int tenant)
        {
            Document document = new DocumentRepository(deploymentPackagePM.Tenant).GetSingleDocument(deploymentPackagePM.Tenant, deploymentPackagePM.DocumentId);
            return new StorageDataArgs()
            {
                FileName = document.Id,
                FolderName = document.Folder,
                Tenant = document.Tenant,
                Extension = document.Extension
            };
        }
    }
}
