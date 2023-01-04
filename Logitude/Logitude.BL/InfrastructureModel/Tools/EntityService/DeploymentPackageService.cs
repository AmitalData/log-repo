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
    public class DeploymentPackageService
    {
        bool isNewEntity;
        private int tenant;
        public DeploymentPackage Poco { get; set; }
        private bool isChange = false;
        private IWebFreightContext context;
        public IWebFreightContext Context
        {
            get { return context; }
            set { context = value; }
        }
        private DeploymentPackagePM entityPM;
        private DeploymentPackageRepository entityRepository;
        private Contact loggedContact;

        public DeploymentPackageService(IWebFreightContext context, int tenant)
        {
            this.tenant = tenant;
            this.isChange = false;
            this.Context = context;
            this.entityRepository = new DeploymentPackageRepository(context);
            this.GetLoggedContact();
        }

        public void Create(DeploymentPackagePM deploymentPackagePM)
        {

            if (!string.IsNullOrEmpty(deploymentPackagePM.Code) && entityRepository.CheckIfDeploymentPackageCodeExist(deploymentPackagePM.Code , deploymentPackagePM.Tenant))
            {
                throw new Exception("Another Deployment Package already exist with this Code ");
            }

            this.isNewEntity = true;
            this.entityPM = deploymentPackagePM;
            this.entityPM.Id = IdCounter.GetNumber("DeploymentPackage", tenant).ToString();
            this.entityPM.CreateDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
            this.entityPM.UpdateDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
            this.entityPM.UpdatedBy = this.loggedContact != null ? this.loggedContact.Id : this.entityPM.UpdatedBy;
            this.entityPM.CreatedBy = this.loggedContact != null ? this.loggedContact.Id : this.entityPM.CreatedBy;
            this.Poco = new DeploymentPackage();
            DeploymentPackageMapping.MapEntity(deploymentPackagePM, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();
        }

        public void Update(DeploymentPackagePM deploymentPackagePM)
        {
            this.isNewEntity = false;
            this.entityPM = deploymentPackagePM;
            this.entityPM.UpdateDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
            this.entityPM.UpdatedBy = this.loggedContact != null ? this.loggedContact.Id : this.entityPM.UpdatedBy;
            this.Poco = entityRepository.GetSingleDeploymentPackage(deploymentPackagePM.Id, deploymentPackagePM.Tenant);
            if (this.Poco == null) return;
            DeploymentPackageMapping.MapEntity(deploymentPackagePM, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
        }

        public void Delete(DeploymentPackagePM deploymentPackagePM)
        {
            this.Poco = entityRepository.GetSingleDeploymentPackage(deploymentPackagePM.Id, deploymentPackagePM.Tenant);
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