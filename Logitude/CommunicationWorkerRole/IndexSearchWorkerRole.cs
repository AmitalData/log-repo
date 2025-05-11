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

namespace CommunicationWorkerRole
{
    public class IndexSearchWorkerRole : WorkerEntryPoint
    {
        private const string AzureSearchAISetKey = "AzureSearchAI";
        private static readonly DevLog logger = DevLog.Instance;
        private static readonly string runIndexerLockKey = "RUN_INDEXER";
        private static readonly string removeOldIndexDataLockKey = "REMOVE_OLD_INDEX_DATA";
        private static readonly string removeOldSearchDataLockKey = "REMOVE_OLD_SEARCH_DATA";
        private int mainTennat = 0;
        private double runIndexerInterval = 1;
        private double removeOldIndexDataInterval = 1;
        private double removeOldSearchDataInterval = 1;

        public override void Run()
        {
            logger.WriteTrace("IndexSearchWorkerRole Run");
        }
        public override bool OnStart()
        {
            logger.WriteTrace("IndexSearchWorkerRole OnStart");
            
            try
            {
                InitTenants();
                InitScheduler();
            }
            catch (Exception e)
            {
                logger.WriteFatal(e, "Error initializing IndexSearchWorkerRole");
                throw;
            }

            return base.OnStart();
        }

        private void InitTenants()
        {
            bool haveTenantDB = int.TryParse(ConfigurationManager.AppSettings["TenantDB"], out int TenantDB);

            logger.WriteTrace($"haveTenantDB: {haveTenantDB}, TenantDB: {TenantDB}");

            if (haveTenantDB)
                mainTennat = TenantDB;

            logger.WriteTrace($"TenantDB: {mainTennat}");
        }

        private void InitScheduler()
        {             
            if (double.TryParse(ConfigurationManager.AppSettings[nameof(runIndexerInterval)], out double configuredInterval))
                runIndexerInterval = configuredInterval;

            Scheduler(RunIndexer, runIndexerInterval, nameof(RunIndexer));
            logger.WriteTrace($"SchedulerRunIndexerFromDB interval: {runIndexerInterval}");
            
            if (double.TryParse(ConfigurationManager.AppSettings[nameof(removeOldIndexDataInterval)], out configuredInterval))
                removeOldIndexDataInterval = configuredInterval;

            Scheduler(RemoveOldIndexData, removeOldIndexDataInterval, nameof(RemoveOldIndexData));
            logger.WriteTrace($"removeOldnIndexeDataInterval interval: {removeOldIndexDataInterval}");

            
            if (double.TryParse(ConfigurationManager.AppSettings[nameof(removeOldSearchDataInterval)], out configuredInterval))
                removeOldSearchDataInterval = configuredInterval;

            Scheduler(RemoveOldSearchDataAsync, removeOldSearchDataInterval, nameof(RemoveOldSearchData));
            logger.WriteTrace($"SchedulerRemoveOldDSearchDataFromDB interval: {removeOldSearchDataInterval}");
        }

        public async Task RunIndexer()
        {
            SearchIndexQuery searchIndexQuery = new SearchIndexQuery(mainTennat);
            List<SearchIndex> allSearchIndexes = searchIndexQuery.GetAll();

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

                    bool success = await RunIndexer(index);
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

            Unlock(runIndexerLockKey);
        }

        public async Task RemoveOldIndexData()
        {
            SearchIndexQuery searchIndexQuery = new SearchIndexQuery(mainTennat);
            List<SearchIndex> allSearchIndexes = searchIndexQuery.GetAll();

            Lock(removeOldIndexDataLockKey);
            foreach (SearchIndex index in allSearchIndexes)
            {
                try
                {
                    if (index.LastRemove?.AddHours(removeOldIndexDataInterval) > DateTime.Now)
                    {
                        logger.WriteTrace($"it is not time for remove old data from index {index.Index}, last time: {index.LastRemove}, skipping index");
                        continue;
                    }

                    bool success = await RomoveFromIndex(index);
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

            Unlock(removeOldIndexDataLockKey);
        }

        private async Task RemoveOldSearchDataAsync() => await Task.Run(() => RemoveOldSearchData());
        
        public void RemoveOldSearchData()
        {
            SearchIndexTenantHistoryQuery searchIndexTenantHistoryQuery = new SearchIndexTenantHistoryQuery(mainTennat);
            SearchIndexEditHistoryQuery searchIndexEditHistoryQuery = new SearchIndexEditHistoryQuery(mainTennat);
            List<SearchIndexTenantHistory> allTenantHistories = searchIndexTenantHistoryQuery.GetAll();

            Lock(removeOldSearchDataLockKey);
            foreach (SearchIndexTenantHistory tenantHistory in allTenantHistories)
            {
                try
                {
                    if (tenantHistory.LastUpdate?.AddHours(removeOldSearchDataInterval) > DateTime.Now)
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

            Unlock(removeOldSearchDataLockKey);
        }

        private void Lock(string key)
        {
            ConcurrentKiller concurrentKiller = new ConcurrentKiller();
            concurrentKiller.FreeLockIfCreated15MinOld(key, mainTennat);
            concurrentKiller.LockOrCrashOnCommitDueUnique(key, mainTennat);
        }

        private void Unlock(string key) => new ConcurrentKiller().FreeLock(key, mainTennat);

        private Timer Scheduler(Func<Task> actionAsync, double time, string actionName = null)
        {
            Timer aTimer = new Timer();
            aTimer.Interval = time;
            aTimer.Elapsed += async (o, eea) =>
            {
                try
                {
                    await actionAsync();
                }
                catch (Exception e)
                {
                    logger.WriteFatal(e, "error on Schdule action " + actionName);
                }
                finally
                {
                    aTimer.Start();
                }
            };
            aTimer.AutoReset = false;
            aTimer.Enabled = true;

            return aTimer;
        }

        private async Task<bool> RunIndexer(SearchIndex index)
        {
            try
            {
                logger.WriteTrace($"Starting indexer for index {index.Index}...");

                FastSearchAzureSearchRepo fastSearchAzureSearchRepo = InitializeFastSearchAzureSearchRepo(index.Index);
                AzureSerchResponse response = await fastSearchAzureSearchRepo.RunIndexerAsync(index.Indexer);                                

                if (response.Status == (int)HttpStatusCode.Accepted)
                {
                    logger.WriteTrace($"Indexer '{index.Indexer}' has been successfully started.");
                    return true;
                }
                else
                {
                    logger.WriteError($"Failed to start indexer '{index.Indexer}'. Status: {response.Status}");
                    logger.WriteError("Error message: " + response.Content.ToString());
                    return false;
                }
            }
            catch (Exception ex)
            {
                logger.WriteFatal(ex, $"Error running indexer: {index.Indexer}");
                return false;
            }
        }

        private async Task<bool> RomoveFromIndex(SearchIndex index)
        {
            try
            {
                logger.WriteInfo($"Starting remove old data from index {index.Index}...");

                string filter = $"{index.TtlField} lt {DateTime.Now.AddMonths(-index.TtlMonth):O}";
                int size = 10;
                int rowDeleted = 0;
                FastSearchAzureSearchRepo fastSearchAzureSearchRepo = InitializeFastSearchAzureSearchRepo(index.Index);

                for (int i = 0; i < 10; i++)
                {
                    AzureSerchResponse response = await fastSearchAzureSearchRepo.DeleteAsync(filter, size);
                    if (response == null) break;

                    if (response.Status == (int)HttpStatusCode.OK)
                    {
                        logger.WriteTrace(message: $"Successfully removed old data from index {index.Index}, wait 1 seconds");
                        await Task.Delay(1000);
                    }
                    else
                    {
                        string content = response?.Content?.ToString();
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
            DefaultAndConfiguration_Ext ConnectionDetails = DefaultService.Instance.Get(0, AzureSearchAISetKey, "Customs");
            string searchServiceEndpoint = ConnectionDetails.Value1;
            string apiKey = ConnectionDetails.Value2;

            return new FastSearchAzureSearchRepo(searchServiceEndpoint, apiKey, indexName); ;
        }
    }
}