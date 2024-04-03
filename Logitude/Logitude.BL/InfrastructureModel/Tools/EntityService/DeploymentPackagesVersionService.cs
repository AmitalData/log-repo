using System;
using System.Web;
using System.Linq;
using System.Collections.Generic;
using Simplog.Data.Helpers;
using Logitude.Server.Tools.Helpers;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools.Counters;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.Tools.Validating;
using Logitude.BL.InfrastructureModel.Tools.TraceEvents;
using Logitude.BL.InfrastructureModel.Tools.DataMapping;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Logitude.BL.InfrastructureModel.Tools.EntityService
{
    public class DeploymentPackagesVersionService
    {
        bool isNewEntity;
        private int tenant;
        public DeploymentPackagesVersion Poco { get; set; }
        private bool isChange = false;
        private IWebFreightContext context;
        public IWebFreightContext Context
        {
            get { return context; }
            set { context = value; }
        }
        private DeploymentPackagesVersionPM entityPM;
        private DeploymentPackagesVersionRepository entityRepository;
        private Contact loggedContact;
        private string versionKey="1";

        public DeploymentPackagesVersionService(IWebFreightContext context, int tenant)
        {
            this.tenant = tenant;
            this.isChange = false;
            this.Context = context;
            this.entityRepository = new DeploymentPackagesVersionRepository(context);
            this.GetLoggedContact();
        }

        public void Create(DeploymentPackagesVersionPM deploymentPackagesVersionPM, bool submitChanges = true)
        {
            this.isNewEntity = true;
            this.entityPM = deploymentPackagesVersionPM;
            this.entityPM.Id = IdCounter.GetNumber("DeploymentPackagesVersion", tenant).ToString();
            this.entityPM.CreateDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
            this.entityPM.UpdateDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
            this.entityPM.UpdatedByUserId = this.loggedContact != null ? this.loggedContact.Id : this.entityPM.UpdatedByUserId;
            this.entityPM.CreatedByUserId = this.loggedContact != null ? this.loggedContact.Id : this.entityPM.CreatedByUserId;
            this.entityPM.VersionNumber = GetLastVersionByDeploymentPackageId(deploymentPackagesVersionPM);
            this.entityPM.VersionName = BuildVersionName(this.entityPM.VersionNumber);
            this.Poco = new DeploymentPackagesVersion();
            DeploymentPackagesVersionMapping.MapEntity(deploymentPackagesVersionPM, Poco, isNewEntity);
            entityRepository.Add(Poco);
            if (!submitChanges) return;
            entityRepository.SubmitChanges();
        }
        private int GetLastVersionByDeploymentPackageId(DeploymentPackagesVersionPM deploymentPackagesVersionPM)
        {
            DeploymentPackagesVersionRepository deploymentPackagesVersionRepository = new DeploymentPackagesVersionRepository(deploymentPackagesVersionPM.Tenant);
            int lastVersionNumber = deploymentPackagesVersionRepository.GetLastDeploymentPackagesVersionByDeploymentPackageId(deploymentPackagesVersionPM.DeploymentPackageID, deploymentPackagesVersionPM.Tenant);
            return lastVersionNumber == 0 ? 0 : lastVersionNumber + 1;
        }
        private string BuildVersionName(int versionNumber)
        {
            return versionKey +"." + versionNumber;
        }
        public void Update(DeploymentPackagesVersionPM deploymentPackagesVersionPM)
        {
            this.isNewEntity = false;
            this.entityPM = deploymentPackagesVersionPM;
            this.entityPM.UpdateDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
            this.entityPM.UpdatedByUserId = this.loggedContact != null ? this.loggedContact.Id : this.entityPM.UpdatedByUserId;
            this.Poco = entityRepository.GetSingleDeploymentPackagesVersion(deploymentPackagesVersionPM.Id, deploymentPackagesVersionPM.Tenant);
            if (this.Poco == null) return;
            DeploymentPackagesVersionMapping.MapEntity(deploymentPackagesVersionPM, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
        }

        public void Delete(DeploymentPackagesVersionPM deploymentPackagesVersionPM)
        {
            this.Poco = entityRepository.GetSingleDeploymentPackagesVersion(deploymentPackagesVersionPM.Id, deploymentPackagesVersionPM.Tenant);
            if (this.Poco == null) return;
            entityRepository.Remove(Poco);
            entityRepository.SubmitChanges();
        }

        private void GetLoggedContact()
        {
            if (HttpContext.Current != null && HttpContext.Current.User != null && HttpContext.Current.User.Identity != null && !string.IsNullOrEmpty(HttpContext.Current.User.Identity.Name))
            {
                this.loggedContact = new ContactRepository(tenant).GetSingleContactByEmail((HttpContext.Current.User.Identity.Name), tenant , true);
                return;
            }
            this.loggedContact = new ContactRepository(tenant).GetSingleContactByEmail(("system@tenant" + tenant.ToString() + ".com"), tenant,true);
        }
    }
}