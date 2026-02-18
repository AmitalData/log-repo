using System;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using Logitude.BL.Helpers;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityLists;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Data.InfrastructureModel.Repositories;
using Logitude.Server.Tools.QueueService;
using Logitude.Server.Tools.Helpers;

namespace Logitude.BL.CommonDataModel.EntityQueries
{
    public class HybridTenantStateQuery
    {
        HybridTenantStateRepository repository;

        public HybridTenantStateQuery()
        {
            repository = new HybridTenantStateRepository();
        }

        public HybridTenantStateQuery(int tenant)
        {
            repository = new HybridTenantStateRepository(tenant);
        }

        public HybridTenantStateQuery(HybridTenantStateRepository HybridTenantStateRepository)
        {
            repository = HybridTenantStateRepository;
        }






        public List<HybridTenantStateList> GetHybridTenantState(int tenant)
        {
            List<HybridTenantStateList> hybridTenantStateLists = (from a in repository.context.HybridTenantStates
                                                                  select new HybridTenantStateList()
                                                                  {
                                                                      Tenant = a.Tenant,
                                                                      WaitingQueue = a.WaitingQueue,
                                                                      LastUpdateDateTime = a.LastUpdateDateTime,
                                                                      FailedQueue = a.FailedQueue,
                                                                      FailedQueueTextColor = "#000000",
                                                                      LastUpdateDateTimeTextColor = "#000000",
                                                                      WaitingQueueTextColor = "#000000",
                                                                      LastQueueDateTime = a.LastQueueDateTime,
                                                                      VersionNumber = a.VersionNumber,
                                                                      VersionDate = a.VersionDate,
                                                                  }).ToList();

            List<int> statetenants = hybridTenantStateLists.Select(t => t.Tenant).ToList();

            List<Tenant> tenantsList = (from a in repository.context.Tenants
                                        where (statetenants.Contains(a.Id))
                                        select a).ToList();

            HybridTenantThresholdQuery hybridTenantThresholdQuery = new HybridTenantThresholdQuery(tenant);
            foreach (HybridTenantStateList item in hybridTenantStateLists)
            {
                HybridTenantThresholdList hybridTenantThresholdList = hybridTenantThresholdQuery.GetHybridTenantThresholdListByTenant(item.Tenant);

                if (hybridTenantThresholdList != null)
                {
                    if (item.FailedQueue > hybridTenantThresholdList.FailedThresold)
                    {
                        item.FailedQueueTextColor = "#ff0000";
                    }

                    if (item.WaitingQueue > hybridTenantThresholdList.WaitingThresold)
                    {
                        item.WaitingQueueTextColor = "#ff0000";
                    }

                    TimeSpan? date = DateTime.UtcNow - item.LastUpdateDateTime;

                    if (date.Value.Days > 3)
                    {
                        item.LastUpdateDateTimeTextColor = "#ff0000";
                    }
                }

                Tenant tenantList = tenantsList.Where(t => t.Id == item.Tenant).FirstOrDefault();
                if (tenantList != null)
                {
                    item.TenantName = tenantList.Company;
                }
                else item.TenantName = "NoTenantFount";

            }

            if (hybridTenantStateLists != null && hybridTenantStateLists.Count > 0)
            {
                hybridTenantStateLists = hybridTenantStateLists.Where(d => d.TenantName != "NoTenantFount").ToList();
            }

            AddWitingAndFailedHybridQueues(tenant, hybridTenantStateLists);

            return hybridTenantStateLists;
        }

        private void AddWitingAndFailedHybridQueues(int tenant, List<HybridTenantStateList> hybridTenantStateLists)
        {
            int workerStatus = 0;
            try
            {
                workerStatus = WorkerNameService.GetWorkerWaitingStatusForReceiving(tenant);
            }
            catch { }

            var queueMessages = new QueueMessageRepository(tenant).GetWaitingForHybridAndFailds(workerStatus)
                .Select(queueMessage => new
                {
                    CommunicationLogId = DictionaryJsonConverter.FromJsonToDictionary(queueMessage.MessageBody)["CommunicationLogId"].ToString(),
                    queueMessageId = queueMessage.Id,
                    queueMessage.Tenant,
                    queueMessage.Status
                }).ToList();

            // delete the queueMessages that have been deleted from the communicationLogs
            var clIds = queueMessages.Select(q => q.CommunicationLogId).ToList();
            (from cl in repository.context.CommunicationLogs
             where clIds.Contains(cl.Id) && cl.CommunicationStatusTypeCode == "D"
             select cl.Id)
                .ToList()
                .ForEach(CommunicationLogId => queueMessages.RemoveAll(x => x.CommunicationLogId == CommunicationLogId));

            var waitingAndFaildMsgList = queueMessages.GroupBy(c => c.Tenant)
                    .Select(g => new
                    {
                        Tenant = g.Key,
                        CountFailed = g.Count(c => c.Status == -1),
                        CountWaiting = g.Count(c => c.Status != -1)
                    })
                    .ToList();

            waitingAndFaildMsgList.ForEach(x =>
            {
                var hybridTenantState = hybridTenantStateLists.FirstOrDefault(h => h.Tenant == x.Tenant);
                if (hybridTenantState != null)
                {
                    hybridTenantState.HybridWaitingQueue = x.CountWaiting;
                    hybridTenantState.HybridFailedQueue = x.CountFailed;
                }
            });
        }
    }
}
