

using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.Tools.DataMapping;
using Logitude.Server.Tools.Counters;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Logitude.Server.Tools;

namespace Logitude.BL.InfrastructureModel.Tools.EntityService
{
    public class DeploymentPackageExecutionLogService
    {
        bool isNewEntity;
        private int tenant;
        public DeploymentPackageExecutionLog Poco { get; set; }

        public IWebFreightContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private DeploymentPackageExecutionLogPM entityPm;
        private IWebFreightContext objectContext;
        private DeploymentPackageExecutionLogRepository entityRepository;


        private Contact loggedContact;
        public DeploymentPackageExecutionLogService(IWebFreightContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new DeploymentPackageExecutionLogRepository(objectContext);

        }



        public void Create(DeploymentPackageExecutionLogPM entityPM)
        {
            this.isNewEntity = true;
            this.entityPm = entityPM;
            this.entityPm.Id = IdCounter.GetNumber("DeploymentPackageExecutionLog", tenant).ToString();
            this.Poco = new DeploymentPackageExecutionLog();

            DeploymentPackageExecutionLogMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();
        }

        public void Update(DeploymentPackageExecutionLogPM entityPM)
        {
            this.isNewEntity = false;
            this.entityPm = entityPM;

            this.Poco = entityRepository.GetSingleDeploymentPackageExecutionLog(entityPM.Id, entityPm.Tenant);
            DeploymentPackageExecutionLogMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();


        }

        public DeploymentPackageExecutionLog GetNewInStanceFromDeploymentPackageExecutionLog(DeploymentPackagePM deploymentPackagePM)
        {
            DeploymentPackageExecutionLogRepository deploymentPackageExecutionLogRepository = new DeploymentPackageExecutionLogRepository(deploymentPackagePM.Tenant);
            DeploymentPackageExecutionLog deploymentPackageExecutionLog = new DeploymentPackageExecutionLog()
            {
                Id = IdCounter.GetNumber("DeploymentPackageExecutionLog", deploymentPackagePM.Tenant).ToString(),
                Tenant = deploymentPackagePM.Tenant,
                CreateDate = DateTime.Now,
                CreatedByUserId = deploymentPackagePM.CreatedBy,
                RequestXML = LogitudeXmlSerializer.SerializeObjectToXmlString(deploymentPackagePM),
                StatusCode = "W",
                Subject = deploymentPackagePM.Name,
            };
            deploymentPackageExecutionLogRepository.Add(deploymentPackageExecutionLog);
            deploymentPackageExecutionLogRepository.SubmitChanges();

            return deploymentPackageExecutionLog;
        }



    }
}
