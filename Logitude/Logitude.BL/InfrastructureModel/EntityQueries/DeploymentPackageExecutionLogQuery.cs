

using System;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using Logitude.BL.Helpers;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityLists;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;


namespace Logitude.BL.InfrastructureModel.EntityQueries
{
    public class DeploymentPackageExecutionLogQuery
    {
        DeploymentPackageExecutionLogRepository repository;

        public DeploymentPackageExecutionLogQuery()
        {
            repository = new DeploymentPackageExecutionLogRepository();
        }

        public DeploymentPackageExecutionLogQuery(int tenant)
        {
            repository = new DeploymentPackageExecutionLogRepository(tenant);
        }

        public DeploymentPackageExecutionLogQuery(DeploymentPackageExecutionLogRepository repository)
        {
            this.repository = repository;
        }

        public IQueryable<DeploymentPackageExecutionLogList> GetIQueryableEntityList(IQueryable<DeploymentPackageExecutionLog> iQueryable)
        {
            IQueryable<DeploymentPackageExecutionLogList> result = from a in iQueryable
                                                         select new DeploymentPackageExecutionLogList()
                                                         {

                                                             Tenant = a.Tenant,
                                                             Id = a.Id,
                                                             CreatedByUserId = a.CreatedByUserId,
                                                             CreateDate = a.CreateDate,
                                                             StartDate = a.StartDate,
                                                             Subject = a.Subject,
                                                             StatusCode = a.StatusCode,
                                                             DoneDate = a.DoneDate,
                                                             ExceptionMessage = a.ExceptionMessage,
                                                             Logs = a.Logs,
                                                             RequestXML = a.RequestXML,
                                                             RetryNumber = a.RetryNumber,
                                                             ExecutedByServerName = a.ExecutedByServerName,

                                                         };


            return result;
        }


        public DeploymentPackageExecutionLogPM GetSinglePM(string id, int tenant)
        {
            DeploymentPackageExecutionLogPM entity = (from a in repository.context.DeploymentPackageExecutionLogs
                                                      where a.Tenant == tenant
                                            && a.Id == id
                                            select new DeploymentPackageExecutionLogPM()
                                            {
                                                Tenant = a.Tenant,
                                                Id = a.Id,
                                                CreatedByUserId = a.CreatedByUserId,
                                                CreateDate = a.CreateDate,
                                                StartDate = a.StartDate,
                                                Subject = a.Subject,
                                                StatusCode = a.StatusCode,
                                                DoneDate = a.DoneDate,
                                                ExceptionMessage = a.ExceptionMessage,
                                                Logs = a.Logs,
                                                RequestXML = a.RequestXML,
                                                RetryNumber = a.RetryNumber,
                                                ExecutedByServerName = a.ExecutedByServerName,

                                            }).FirstOrDefault();
            return entity;
        }

        public IQueryable<DeploymentPackageExecutionLogPM> GetDeploymentPackageExecutionLogPMsByTenant(int tenant)
        {
            IQueryable<DeploymentPackageExecutionLogPM> DeploymentPackageExecutionLogPMs = from a in repository.context.DeploymentPackageExecutionLogs
                                                                                   where a.Tenant == tenant
                                                                       select new DeploymentPackageExecutionLogPM()
                                                                       {
                                                                           Tenant = a.Tenant,
                                                                           Id = a.Id,
                                                                           CreatedByUserId = a.CreatedByUserId,
                                                                           CreateDate = a.CreateDate,
                                                                           StartDate = a.StartDate,
                                                                           Subject = a.Subject,
                                                                           StatusCode = a.StatusCode,
                                                                           DoneDate = a.DoneDate,
                                                                           ExceptionMessage = a.ExceptionMessage,
                                                                           Logs = a.Logs,
                                                                           RequestXML = a.RequestXML,
                                                                           RetryNumber = a.RetryNumber,
                                                                           ExecutedByServerName = a.ExecutedByServerName,

                                                                       };
            return DeploymentPackageExecutionLogPMs;
        }

        public IQueryable<DeploymentPackageExecutionLogList> GetDeploymentPackageExecutionLogListsByTenant(int tenant)
        {
            IQueryable<DeploymentPackageExecutionLogList> DeploymentPackageExecutionLogLists = from a in repository.context.DeploymentPackageExecutionLogs
                                                                                       where a.Tenant == tenant
                                                                           select new DeploymentPackageExecutionLogList()
                                                                           {
                                                                               Tenant = a.Tenant,
                                                                               Id = a.Id,
                                                                               CreatedByUserId = a.CreatedByUserId,
                                                                               CreateDate = a.CreateDate,
                                                                               StartDate = a.StartDate,
                                                                               Subject = a.Subject,
                                                                               StatusCode = a.StatusCode,
                                                                               DoneDate = a.DoneDate,
                                                                               ExceptionMessage = a.ExceptionMessage,
                                                                               Logs = a.Logs,
                                                                               RequestXML = a.RequestXML,
                                                                               RetryNumber = a.RetryNumber,
                                                                               ExecutedByServerName = a.ExecutedByServerName,


                                                                           };
            return DeploymentPackageExecutionLogLists;
        }

        public DeploymentPackageExecutionLogList GetDeploymentPackageExecutionLogList(string id, int tenant)
        {
            DeploymentPackageExecutionLogList deploymentPackageExecutionLogList = (from a in repository.context.DeploymentPackageExecutionLogs
                                                                           where a.Tenant == tenant && a.Id == id
                                              select new DeploymentPackageExecutionLogList()
                                              {
                                                  Tenant = a.Tenant,
                                                  Id = a.Id,
                                                  Subject = a.Subject,
                                                  StatusCode = a.StatusCode,
                                                  ExceptionMessage = a.ExceptionMessage,
                                                  CreateDate = a.CreateDate,
                                                  ExecutedByServerName = a.ExecutedByServerName,

                                              }).FirstOrDefault();

            if (deploymentPackageExecutionLogList != null) deploymentPackageExecutionLogList.ExceptionMessage = GetUnderStandableMessageFromMessageException(deploymentPackageExecutionLogList.ExceptionMessage);

            return deploymentPackageExecutionLogList;
        }


        private string GetUnderStandableMessageFromMessageException(string exceptionMessage)
        {
            string result = string.Empty;
            if (!string.IsNullOrEmpty(exceptionMessage))
            {
                string[] lines = exceptionMessage.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                result = lines[0];
            }
            return result;
        }
    }
}