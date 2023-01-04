using Logitude.Server.Tools.Helpers;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;

namespace Logitude.Server.Tools.QueueService
{
    public class WorkerNameService
    {
        public static int GetWorkerWaitingStatusForSending(int tenant)
        {
            int waitingStatus = 0;
            string workerName = GetCurrentWorkerName();
            if (FeatureToggleHelper.HasFeatureToggle("NWR", tenant))
            {
                 waitingStatus = -1030;
                if (!string.IsNullOrEmpty(workerName))
                {
                    if (workerName.ToLower() == "staging")
                    {
                        workerName = "stagingnew";
                    }
                    else if (workerName.ToLower() == "production")
                    {
                        workerName = "productionnew";
                    }
                }
            }
            if (!string.IsNullOrEmpty(workerName))
            { 
                waitingStatus = GetWatingStatusByWorkerRoleName(tenant, workerName);
            }
            
            return waitingStatus;
        }

        public static int GetWorkerWaitingStatusForReceiving(int tenant)
        {
            string workerName = GetCurrentWorkerName();
            if (!string.IsNullOrEmpty(workerName))
            {
                return GetWatingStatusByWorkerRoleName(tenant, workerName);
            }
            else
            {
                throw new Exception("Worker role name is not set!");
            }
            
        }

        private static int GetWatingStatusByWorkerRoleName(int tenant, string workerName)
        {
            if (workerName.ToLower() == "development")
                return -10;

            if (workerName.ToLower() == "production")
                return 0;
            if (workerName.ToLower() == "staging")
                return -100;
            
            return GetWaitingSatusForWorkerName(tenant, workerName);
        }

        private static string GetCurrentWorkerName()
        {
            string workerName = null;
            if (HttpContext.Current != null)
            {
                if (HttpContext.Current.Items.Contains("workerrolename"))
                {
                    workerName = HttpContext.Current.Items["workerrolename"].ToString();

                }
            }
            else
            {
                workerName = LogitudeSettings.WorkerRoleName;
            }

            return workerName;
        }

        private static int GetWaitingSatusForWorkerName(int tenant, string workerName)
        {
            string entityName = "WorkerRoleNamePM" + workerName;

            WorkerRoleName entity = GetWorkerRoleNameFromDB(workerName, tenant);
            if (entity == null)
            {
                entity = CreateNewWorkerRoleName(tenant, workerName, entityName);
            }

            return entity.WaitingStatus;
        }

        private static WorkerRoleName CreateNewWorkerRoleName(int tenant, string workerName, string entityName)
        {
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                int newWatingStatus = -1000;
                WorkerRoleNameRepository workerRoleNameRepository = new WorkerRoleNameRepository(tenant);
                WorkerRoleName lastAdded = workerRoleNameRepository.GetLastAddedWorkerRoleName();
                if (lastAdded == null)
                    newWatingStatus = -1000;
                else
                    newWatingStatus = lastAdded.WaitingStatus - 1;

                WorkerRoleName entity = new WorkerRoleName()
                {
                    CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                    Name = workerName,
                    WaitingStatus = newWatingStatus,
                };
                workerRoleNameRepository.Add(entity);
                workerRoleNameRepository.SubmitChanges();
                if (HttpContext.Current != null)
                {
                    if (CacheManager.CacheWrapper.Get(entityName) == null)
                    {
                        CacheManager.CacheWrapper.Insert(entityName, entity, null, DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                    }
                }

                scope.Complete();

                return entity;
            }
        }

        private static WorkerRoleName GetWorkerRoleNameFromDB(string workerName, int tenant)
        {
            string entityName = "WorkerRoleNamePM" + workerName;
            WorkerRoleName entity;
            if (HttpContext.Current != null)
            {
                if (CacheManager.CacheWrapper.Get(entityName) == null)
                {
                    WorkerRoleNameRepository workerRoleNameRepository = new WorkerRoleNameRepository(tenant);
                    entity = workerRoleNameRepository.GetSingleWorkerRoleName(workerName);
                    if (entity != null)
                    {
                        if (CacheManager.CacheWrapper.Get(entityName) == null)
                        {
                            CacheManager.CacheWrapper.Insert(entityName, entity, null, DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                        }
                    }
                }
                else
                {
                    entity = (WorkerRoleName)CacheManager.CacheWrapper.Get(entityName);
                }
            }
            else
            {
                WorkerRoleNameRepository workerRoleNameRepository = new WorkerRoleNameRepository(tenant);
                entity = workerRoleNameRepository.GetSingleWorkerRoleName(workerName);
            }

            return entity;
        }
    }
}
