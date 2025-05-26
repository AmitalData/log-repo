using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Helpers;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.Server.Tools.Utils;
using NetCommonHelper.Logger;
using Simplog.Data.AzureSearch;
using Simplog.Data.AzureSearch.Repo;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Threading.Tasks;
using System.Timers;
using CancellationToken = System.Threading.CancellationToken;

namespace CommunicationWorkerRole
{
    public class IndexSearchWorkerRole : WorkerEntryPoint
    {
        private bool isFirstTime = true;
        private const string AzureSearchAISetKey = "AzureSearchAI";
        private const string AzureSearchAIAdditionalKey = "Customs";
        private static readonly DevLog logger = DevLog.Instance;
        private static readonly string runIndexerLockKey = "RUN_INDEXER";
        private static readonly string removeOldIndexDataLockKey = "REMOVE_OLD_INDEX_DATA";
        private static readonly string removeOldSearchDataLockKey = "REMOVE_OLD_SEARCH_DATA";
        private int mainTenant = 0;
        private double runIndexerIntervalMinutes = 1;
        private double removeOldIndexDataIntervalHours = 1;
        private double removeOldSearchDataIntervalHours = 1;

        public override void WorkOnce()
        {
            if (!isFirstTime) return;

            DevLog.Instance.WriteDebug("IndexSearchWorkerRole start run (WorkOnce)");

            try
            {
                isFirstTime = false;
                OnStart();
            }
            catch (Exception e)
            {
                logger.WriteFatal(e, "Error initializing IndexSearchWorkerRole");                
            }
        }

        public override void Run()
        {
            logger.WriteTrace("IndexSearchWorkerRole Run");
        }

        public override bool OnStart()
        {
            logger.WriteTrace("IndexSearchWorkerRole OnStart");
           
            InitTenants();
            InitScheduler();          

            return base.OnStart();
        }

        private void InitTenants()
        {
            bool haveTenantDB = int.TryParse(ConfigurationManager.AppSettings["TenantDB"], out int TenantDB);

            logger.WriteTrace($"haveTenantDB: {haveTenantDB}, TenantDB: {TenantDB}");

            if (haveTenantDB)
                mainTenant = TenantDB;

            logger.WriteTrace($"TenantDB: {mainTenant}");
        }

        private void InitScheduler()
        {
            runIndexerIntervalMinutes = GetValueFromConfig(nameof(runIndexerIntervalMinutes), runIndexerIntervalMinutes);
            Scheduler(() => RunIndexerAsync(CancellationToken.None), TimeSpan.FromMinutes(runIndexerIntervalMinutes).TotalMilliseconds, nameof(RunIndexerAsync));

            removeOldIndexDataIntervalHours = GetValueFromConfig(nameof(removeOldIndexDataIntervalHours), removeOldIndexDataIntervalHours);
            Scheduler(() => RemoveOldIndexDataAsync(CancellationToken.None), TimeSpan.FromHours(removeOldIndexDataIntervalHours).TotalMilliseconds, nameof(RemoveOldIndexDataAsync));

            removeOldSearchDataIntervalHours = GetValueFromConfig(nameof(removeOldSearchDataIntervalHours), removeOldSearchDataIntervalHours);
            Scheduler(() => RemoveOldSearchDataAsync(CancellationToken.None), TimeSpan.FromHours(removeOldSearchDataIntervalHours).TotalMilliseconds, nameof(RemoveOldSearchDataAsync));
        }

        private double GetValueFromConfig(string configKey, double defaultValue)
        {
            string value = ConfigurationManager.AppSettings[configKey];
            return double.TryParse(value, out double result) ? result : defaultValue;
        }

        public async Task RunIndexerAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            logger.WriteTrace("RunIndexerAsync started");

            SearchIndexQuery searchIndexQuery = new SearchIndexQuery(mainTenant);
            List<SearchIndex> allSearchIndexes = searchIndexQuery.GetAll();

            try
            {
                Lock(runIndexerLockKey);

                foreach (SearchIndex index in allSearchIndexes)
                {
                    try
                    {
                        if (index.LastUpdate?.AddMinutes(index.BuildIntervalMin) > DateTime.Now)
                        {
                            logger.WriteTrace($"it is not time for update index {index.Index}, last time: {index.LastUpdate}, skipping index");
                            continue;
                        }

                        bool success = await RunIndexerAsync(index, cancellationToken).ConfigureAwait(false);
                        if (success)
                        {
                            index.LastUpdate = DateTime.Now;
                            searchIndexQuery.Update(index);
                        }
                    }
                    catch (Exception e)
                    {
                        logger.WriteFatal(e, $"Error processing index {index.Index}");
                    }
                }
            }

            finally
            {
                Unlock(runIndexerLockKey);
            }

            logger.WriteTrace("RunIndexerAsync completed");
        }

        public async Task RemoveOldIndexDataAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            logger.WriteTrace("RemoveOldIndexDataAsync started");

            SearchIndexQuery searchIndexQuery = new SearchIndexQuery(mainTenant);
            List<SearchIndex> allSearchIndexes = searchIndexQuery.GetAll();

            try
            {
                Lock(removeOldIndexDataLockKey);

                foreach (SearchIndex index in allSearchIndexes)
                {
                    try
                    {
                        if (index.LastRemove?.AddHours(removeOldIndexDataIntervalHours) > DateTime.Now)
                        {
                            logger.WriteTrace($"it is not time for remove old data from index {index.Index}, last time: {index.LastRemove}, skipping index");
                            continue;
                        }

                        bool success = await RemoveFromIndexAsync(index, cancellationToken).ConfigureAwait(false);
                        if (success)
                        {
                            index.LastRemove = DateTime.Now;
                            searchIndexQuery.Update(index);
                        }
                    }
                    catch (Exception e)
                    {
                        logger.WriteFatal(e, $"Error when remove old data from index {index.Index}");
                    }
                }
            }
            finally
            {
                Unlock(removeOldIndexDataLockKey);
            }

            logger.WriteTrace("RemoveOldIndexDataAsync completed");
        }

        private async Task RemoveOldSearchDataAsync(CancellationToken cancellationToken = default(CancellationToken)) => await Task.Run(() => RemoveOldSearchData(), cancellationToken).ConfigureAwait(false);

        public void RemoveOldSearchData()
        {
            logger.WriteTrace("RemoveOldSearchData started");

            SearchIndexTenantHistoryQuery searchIndexTenantHistoryQuery = new SearchIndexTenantHistoryQuery(mainTenant);
            SearchIndexEditHistoryQuery searchIndexEditHistoryQuery = new SearchIndexEditHistoryQuery(mainTenant);
            List<SearchIndexTenantHistory> allTenantHistories = searchIndexTenantHistoryQuery.GetAll();

            try
            {
                Lock(removeOldSearchDataLockKey);

                foreach (SearchIndexTenantHistory tenantHistory in allTenantHistories)
                {
                    try
                    {
                        if (tenantHistory.LastUpdate?.AddHours(removeOldSearchDataIntervalHours) > DateTime.Now)
                        {
                            logger.WriteTrace($"it is not time for remove search data {tenantHistory.Screen}, last time: {tenantHistory.LastUpdate}, skipping index");
                            continue;
                        }

                        logger.WriteTrace($"Removing old search data for tenant {tenantHistory.Tenant}, from month {tenantHistory.TtlMonth}, for screen {tenantHistory.Screen}");

                        DateTime toDateTime = DateTime.Now.AddMonths(-tenantHistory.TtlMonth);
                        searchIndexEditHistoryQuery.RemoveOldSearchData(tenantHistory.Tenant, tenantHistory.Screen, toDateTime);

                        logger.WriteTrace($"Removing old search screen {tenantHistory.Screen}");

                        tenantHistory.LastUpdate = DateTime.Now;
                        searchIndexTenantHistoryQuery.Update(tenantHistory);
                    }
                    catch (Exception e)
                    {
                        logger.WriteFatal(e, $"Error processing index {tenantHistory.Screen}");
                    }
                }
            }
            finally
            {
                Unlock(removeOldSearchDataLockKey);
            }

            logger.WriteTrace("RemoveOldSearchData completed");
        }

        private void Lock(string key)
        {
            ConcurrentKiller concurrentKiller = new ConcurrentKiller();
            concurrentKiller.FreeLockIfCreated15MinOld(key, mainTenant);
            concurrentKiller.LockOrCrashOnCommitDueUnique(key, mainTenant);
        }

        private void Unlock(string key) => new ConcurrentKiller().FreeLock(key, mainTenant);

        private Timer Scheduler(Func<Task> actionAsync, double intervalMiliseconds, string actionName = null)
        {
            logger.WriteTrace(message: $"Scheduler {actionName} interval: {TimeSpan.FromMilliseconds(intervalMiliseconds).TotalMinutes} minutes");

            if (actionAsync == null)
                throw new ArgumentNullException(nameof(actionAsync), "actionAsync cannot be null");

            Timer aTimer = new Timer();
            aTimer.Interval = intervalMiliseconds;
            aTimer.Elapsed += async (timer, eea) =>
            {
                try
                {
                    logger.WriteTrace($"Starting scheduled action: {actionName} at {DateTime.Now}");
                    ((Timer)timer).Stop(); // Stop the timer to prevent re-entrancy
                    await actionAsync().ConfigureAwait(false);
                }
                catch (Exception e)
                {
                    logger.WriteFatal(e, "error on scheduled  action " + actionName);
                }
                finally
                {
                    ((Timer)timer).Start();
                }
            };
            aTimer.AutoReset = false;
            aTimer.Enabled = true;

            return aTimer;
        }

        private async Task<bool> RunIndexerAsync(SearchIndex index, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                if (index == null)
                    throw new ArgumentNullException(nameof(index), "index cannot be null");

                logger.WriteTrace($"Starting indexer for index {index.Index}...");

                FastSearchAzureSearchRepo fastSearchAzureSearchRepo = InitializeFastSearchAzureSearchRepo(index.Index);
                AzureSerchResponse response = await fastSearchAzureSearchRepo.RunIndexerAsync(index.Indexer, cancellationToken).ConfigureAwait(false);

                if (response.Status == (int)HttpStatusCode.Accepted)
                {
                    logger.WriteTrace($"Indexer '{index.Indexer}' has been successfully started.");
                    return true;
                }
                else
                {
                    logger.WriteError($"Failed to start indexer '{index.Indexer}'. Status: {response.Status}");
                    logger.WriteError("Error message: " + response.Content);
                    return false;
                }
            }
            catch (Exception ex)
            {
                logger.WriteFatal(ex, $"Error running indexer: {index.Indexer}");
                return false;
            }
        }

        private async Task<bool> RemoveFromIndexAsync(SearchIndex index, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (index == null)
                throw new ArgumentNullException(nameof(index), "index cannot be null");

            try
            {
                logger.WriteInfo($"Starting remove old data from index {index.Index}...");

                string filter = $"{index.TtlField} lt {DateTime.Now.AddMonths(-index.TtlMonth):O}";
                int size = 10;
                int rowDeleted = 0;
                FastSearchAzureSearchRepo fastSearchAzureSearchRepo = InitializeFastSearchAzureSearchRepo(index.Index);

                for (int i = 0; i < 10; i++)
                {
                    AzureSerchResponse response = await fastSearchAzureSearchRepo.DeleteAsync(filter, size, cancellationToken).ConfigureAwait(false);
                    if (response == null) break;

                    if (response.Status == (int)HttpStatusCode.OK)
                    {
                        logger.WriteTrace(message: $"Successfully removed old data from index {index.Index}, wait 1 seconds, deleted rows: {response.Content}");
                        rowDeleted += response.Count;
                        await Task.Delay(1000).ConfigureAwait(false);
                    }
                    else
                    {
                        string content = response?.Content;
                        logger.WriteError($"Failed to remove old data from index {index.Index}. Status: {response.Status}, response: {content}");
                        return false;
                    }
                }

                logger.WriteInfo($"Successfully removed {rowDeleted} rows old data from index {index.Index}.");

                return true;
            }
            catch (Exception ex)
            {
                logger.WriteFatal(ex, $"Error removing old data from index: {index.Index}");
                return false;
            }
        }

        private static FastSearchAzureSearchRepo InitializeFastSearchAzureSearchRepo(string indexName)
        {
            DefaultAndConfiguration_Ext ConnectionDetails = DefaultService.Instance.Get(0, AzureSearchAISetKey, AzureSearchAIAdditionalKey);
            string searchServiceEndpoint = ConnectionDetails.Value1;
            string apiKey = ConnectionDetails.Value2;

            return new FastSearchAzureSearchRepo(searchServiceEndpoint, apiKey, indexName); ;
        }
    }
}