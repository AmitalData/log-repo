using Azure;
using Azure.Search.Documents;
using Azure.Search.Documents.Indexes;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Helpers;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.Server.Tools.Utils;
using NetCommonHelper.Logger;
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
    class IndexSearchWorkerRole : WorkerEntryPoint
    {
        private const string AzureSearchAISetKey = "AzureSearchAI";
        private static readonly DevLog logger = DevLog.Instance;
        private static readonly string runIndexerLockKey = "RUN_INDEXER";
        private static readonly string removeOldIndexDataLockKey = "REMOVE_OLD_INDEX_DATA";
        private static readonly string removeOldSearchDataLockKey = "REMOVE_OLD_SEARCH_DATA";
        private int mainTennat = 0;
        private double runIndexerInterval = TimeSpan.FromMinutes(1).TotalMilliseconds;
        private double removeOldIndexDataInterval = TimeSpan.FromHours(1).TotalMilliseconds;
        private double removeOldSearchDataInterval = TimeSpan.FromHours(1).TotalMilliseconds;
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

            Scheduler(RemoveOldnIndexData, removeOldIndexDataInterval, nameof(RemoveOldnIndexData));
            logger.WriteTrace($"removeOldnIndexeDataInterval interval: {removeOldIndexDataInterval}");

            
            if (double.TryParse(ConfigurationManager.AppSettings[nameof(removeOldSearchDataInterval)], out configuredInterval))
                removeOldSearchDataInterval = configuredInterval;

            Scheduler(RemoveOldDSearchDataAsync, removeOldSearchDataInterval, nameof(RemoveOldDSearchData));
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
                    if (index.LastUpdate.AddMinutes(index.BuildIntervalMin) > DateTime.Now)
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

        private async Task RemoveOldnIndexData()
        {
            SearchIndexQuery searchIndexQuery = new SearchIndexQuery(mainTennat);
            List<SearchIndex> allSearchIndexes = searchIndexQuery.GetAll();

            Lock(removeOldIndexDataLockKey);
            foreach (SearchIndex index in allSearchIndexes)
            {
                try
                {
                    if (index.LastRemove.AddHours(removeOldIndexDataInterval) > DateTime.Now)
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

        private async Task RemoveOldDSearchDataAsync() => await Task.Run(() => RemoveOldDSearchData());
        
        private void RemoveOldDSearchData()
        {
            SearchIndexTenantHistoryQuery searchIndexTenantHistoryQuery = new SearchIndexTenantHistoryQuery(mainTennat);
            SearchIndexEditHistoryQuery searchIndexEditHistoryQuery = new SearchIndexEditHistoryQuery(mainTennat);
            List<SearchIndexTenantHistory> allTenantHistories = searchIndexTenantHistoryQuery.GetAll();

            Lock(removeOldSearchDataLockKey);
            foreach (SearchIndexTenantHistory tenantHistory in allTenantHistories)
            {
                try
                {
                    if (tenantHistory.LastUpdate.AddHours(removeOldSearchDataInterval) > DateTime.Now)
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

                SearchIndexerClient indexerClient = InitializeIndexerClient(index.ObjectName);
                Response response = await indexerClient.RunIndexerAsync(index.Indexer);

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
                logger.WriteTrace($"Starting remove old data from index {index.Index}...");

                DefaultAndConfiguration_Ext ConnectionDetails = DefaultService.Instance.Get(0, AzureSearchAISetKey, index.Index);
                string searchServiceEndpoint = ConnectionDetails.Value1;
                string apiKey = ConnectionDetails.Value2;
                AzureSearchRepoBase<object> azureSearchRepo = new AzureSearchRepoBase<object>(searchServiceEndpoint, apiKey, index.Index, new string[] { });

                List<object> results = await azureSearchRepo.SearchAsync("*", new SearchOptions
                {
                    Filter = $"{index.TtlField} lt {DateTime.Now.AddMonths(-index.TtlMonth):O}",
                    Select = { "id" }
                });

                Response response = (await azureSearchRepo.DeleteAsync(results)).GetRawResponse();

                if (response.Status == (int)HttpStatusCode.OK)
                {
                    logger.WriteTrace(message: $"Successfully removed old data from index {index.Index}.");
                    return true;
                }
                else
                {
                    string content = response?.Content?.ToString();
                    logger.WriteError($"Failed to remove old data from index {index.Index}. Status: {response.Status}, response: {content}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                logger.WriteFatal(ex, $"Error removing old data from index: {index.Index}");
                return false;
            }
        }

        private static SearchIndexerClient InitializeIndexerClient(string indexName)
        {
            DefaultAndConfiguration_Ext ConnectionDetails = DefaultService.Instance.Get(0, AzureSearchAISetKey, indexName);
            string searchServiceEndpoint = ConnectionDetails.Value1;
            string apiKey = ConnectionDetails.Value2;
            AzureKeyCredential credential = new AzureKeyCredential(apiKey);
            SearchIndexerClient indexerClient = new SearchIndexerClient(new Uri(searchServiceEndpoint), credential);
            return indexerClient;
        }
    }
}