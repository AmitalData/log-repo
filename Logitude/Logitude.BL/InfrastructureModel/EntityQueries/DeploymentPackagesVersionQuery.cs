using Logitude.BL.InfrastructureModel.EntityLists;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InfrastructureModel.EntityQueries
{
    public class DeploymentPackagesVersionQuery
    {
        private DeploymentPackagesVersionRepository repository;
        private int tenant;
        public DeploymentPackagesVersionQuery(int tenant)
        {
            repository = new DeploymentPackagesVersionRepository(tenant);
            this.tenant = tenant;
        }
        public DeploymentPackagesVersionQuery(DeploymentPackagesVersionRepository deploymentPackagesVersionRepository)
        {
            repository = deploymentPackagesVersionRepository;
        }
        public DeploymentPackagesVersionPM GetSinglePM(string id, int tenant)
        {
            DeploymentPackagesVersion deploymentPackagesVersion = repository.GetSingleDeploymentPackagesVersion(id, tenant);
            DeploymentPackagesVersionPM result = MapDeploymentPackagesVersionToDeploymentPackagesVersionPM(deploymentPackagesVersion, new DeploymentPackagesVersionPM());
            return result;
        }
        public IQueryable<DeploymentPackagesVersionList> GetIQueryableEntityList(IQueryable<DeploymentPackagesVersion> iQueryable)
        {

            IQueryable<DeploymentPackagesVersionList>
                result = from a in iQueryable.Include("CreatedByUser.Contact").Include("UpdatedByUser.Contact")
                         select new DeploymentPackagesVersionList()
                         {
                             Id = a.Id,
                             Tenant = a.Tenant,
                             CreateDate = a.CreateDate,
                             CreatedByUserId = a.CreatedByUserId,
                             UpdateDate = a.UpdateDate,
                             UpdatedByUserId = a.UpdatedByUserId,
                             DeploymentPackageID = a.DeploymentPackageID,
                             IsExported = a.IsExported,
                             DocumentId = a.DocumentId,
                             VersionName = a.VersionName,
                             VersionNumber = a.VersionNumber,         
                             CreatedByUserName = (a.CreatedByUser!=null && a.CreatedByUser.Contact!=null) ? a.CreatedByUser.Contact.EnglishName:null,
                             UpdatedByUserName = (a.UpdatedByUser != null && a.UpdatedByUser.Contact != null) ? a.UpdatedByUser.Contact.EnglishName : null,
                         };

            return result;
        }



        private DeploymentPackagesVersionPM MapDeploymentPackagesVersionToDeploymentPackagesVersionPM(DeploymentPackagesVersion deploymentPackagesVersion, DeploymentPackagesVersionPM deploymentPackageVersionPM)
        {
            deploymentPackageVersionPM.Id = deploymentPackagesVersion.Id;
            deploymentPackageVersionPM.Tenant = deploymentPackagesVersion.Tenant;
            deploymentPackageVersionPM.CreateDate = deploymentPackagesVersion.CreateDate;
            deploymentPackageVersionPM.CreatedByUserId = deploymentPackagesVersion.CreatedByUserId;
            deploymentPackageVersionPM.UpdateDate = deploymentPackagesVersion.UpdateDate;
            deploymentPackageVersionPM.UpdatedByUserId = deploymentPackagesVersion.UpdatedByUserId;
            deploymentPackageVersionPM.DeploymentPackageID = deploymentPackagesVersion.DeploymentPackageID;
            deploymentPackageVersionPM.IsExported = deploymentPackagesVersion.IsExported;
            deploymentPackageVersionPM.DocumentId = deploymentPackagesVersion.DocumentId;
            deploymentPackageVersionPM.VersionName = deploymentPackagesVersion.VersionName;
            deploymentPackageVersionPM.VersionNumber = deploymentPackagesVersion.VersionNumber;
            return deploymentPackageVersionPM;
        }
    
        public string GetDeploymentPackageVersionNameById(string id, int tenant)
        {
            return repository.context.DeploymentPackagesVersions.Where(v => v.Id == id && v.Tenant == tenant).Select(d=>d.VersionName).FirstOrDefault();
        }
    }
}
