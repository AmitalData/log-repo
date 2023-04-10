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
using Logitude.BL.InfrastructureModel.Services;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.Server.Tools.QueueService;

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
        private string deploymentPackageExecutionLogId;

        public DeploymentPackageService(IWebFreightContext context, int tenant)
        {
            this.tenant = tenant;
            this.isChange = false;
            this.Context = context;
            this.entityRepository = new DeploymentPackageRepository(context);
            this.GetLoggedContact();
        }

        public void Create(DeploymentPackagePM deploymentPackagePM, bool fromWeb = true)
        {

            DeploymentPackageValidating.ValidateCode(deploymentPackagePM.Code, deploymentPackagePM.Tenant, entityRepository);
            if (deploymentPackagePM.DirectionId == "I" && fromWeb)
            {
                BuildImportQueueMessage(deploymentPackagePM);
                return;
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
            Context.SaveChanges();
            Poco.VersionId = new DeploymentPackageVersionInitializerService(deploymentPackagePM, Context).Create().Id;
            Context.SaveChanges();
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
            new DeploymentPackageVersionInitializerService(deploymentPackagePM, Context).Update();
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

        private void BuildImportQueueMessage(DeploymentPackagePM deploymentPackagePM)
        {
            DeploymentPackageExecutionLogService executionLogService = new DeploymentPackageExecutionLogService(Context, deploymentPackagePM.Tenant);
            DeploymentPackageExecutionLog deploymentPackageExecutionLog = executionLogService.GetNewInStanceFromDeploymentPackageExecutionLog(deploymentPackagePM);

            deploymentPackagePM.PackageExecutionLogId = deploymentPackageExecutionLog?.Id;

            IQueueService queueservice = new DbQueueService();
            queueservice.InitializeQueue("DeploymentPackageQueue", deploymentPackageExecutionLog.Tenant);
            queueservice.Send(new Dictionary<string, string>() { { "DeploymentPackageExecutionLogId", deploymentPackageExecutionLog.Id }, { "Tenant", deploymentPackageExecutionLog.Tenant.ToString() } }, deploymentPackageExecutionLog.Tenant);
        }
    }
}