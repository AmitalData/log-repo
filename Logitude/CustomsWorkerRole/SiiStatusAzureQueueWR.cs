using Azure;
using Azure.Messaging.ServiceBus;
using Logitude.BL.Helpers;
using Logitude.Customs.BL.BL.SIIRequest;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data.DataContracts.SIIRequest;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Server.Tools;
using Newtonsoft.Json;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using WebFreight.Web.Helpers.WorkerRoleHelpers;
using WebFreight.Web.Security;

namespace CustomsWorkerRole
{
    public class SiiStatusAzureQueueWR : CustomsWorkerEntryPoint
    {
        static string _connectionString;
        static string _queueName;
        static string _queuePrefix;

        int _tenant = 0;
        bool _onStartDone = false;
        bool _hasConfig = false;

        public SiiStatusAzureQueueWR()
        {
        }

        public override bool OnStart()
        {
            ThreadId = Guid.NewGuid().ToString();
            DoneItemsInRange = new Dictionary<DateTime, int>();
            ConnectClient();
            return base.OnStart();
        }

        public override void Run()
        {
            while (true)
            {
                if (!General.IsUpdating())
                {
                    try
                    {
                        WorkOnce();
                        Thread.Sleep(TimeSpan.FromMinutes(1));
                    }
                    catch (Exception exception)
                    {
                        Thread.Sleep(TimeSpan.FromMinutes(1));
                        NetCommonHelper.Logger.DevLog.Instance.WriteError(exception.ToString());
                    }
                }
                else
                {
                    Thread.Sleep(TimeSpan.FromMinutes(1));
                }
            }
        }

        private void ConnectClient()
        {
            try
            {
                int tenantConfig = SettingUtil.GetTenantDBFromConfig();
                _tenant = tenantConfig;

                _connectionString = GetMandatoryDefault(_tenant, "SIIStatusQueueConn");
                _queuePrefix = GetMandatoryDefault(_tenant, "SIIStatusQueuePrefix");

                var setting = CustomsSettingQueryService.GetSettingByTenant(_tenant);
                var consumerId = setting?.CustomsAgentId;
                if (string.IsNullOrWhiteSpace(consumerId))
                {
                    throw new ConfigurationErrorsException(
                        string.Format("CustomsAgentId is missing or empty in Customs settings for tenant {0}.", _tenant));
                }

                _queueName = _queuePrefix + consumerId;

                NetCommonHelper.Logger.DevLog.Instance.WriteInfo(
                    string.Format("SII Status WR: Tenant={0}, Queue={1}, ConsumerId={2}",
                        _tenant, _queueName, consumerId));

                _hasConfig = true;
            }
            catch (ConfigurationErrorsException ex)
            {
                _hasConfig = false;
                NetCommonHelper.Logger.DevLog.Instance.WriteInfo(
                    string.Format("SII Status WR: configuration missing. {0}. Worker will not start.", ex.Message));
            }
            catch (Exception ex)
            {
                _hasConfig = false;
                NetCommonHelper.Logger.DevLog.Instance.WriteError(ex.ToString());
            }
        }

        public override void WorkOnce()
        {
            try
            {
                if (_onStartDone) return;
                if (!_hasConfig) return;

                _onStartDone = true;
                OnStart();
                ExecuteQueue();
            }
            catch (Exception exception)
            {
                Thread.Sleep(TimeSpan.FromMinutes(1));
                _onStartDone = false;
                NetCommonHelper.Logger.DevLog.Instance.WriteError(exception.ToString());
            }
        }

        public ServiceBusProcessor ExecuteQueue()
        {
            if (!_hasConfig ||
                string.IsNullOrWhiteSpace(_connectionString) ||
                string.IsNullOrWhiteSpace(_queueName))
            {
                NetCommonHelper.Logger.DevLog.Instance.WriteInfo(
                    "SII Status WR: configuration incomplete. ConnectionString or QueueName is empty. Processor will not start.");
                return null;
            }

            var processor = new ServiceBusClient(_connectionString).CreateProcessor(
                _queueName,
                new ServiceBusProcessorOptions
                {
                    AutoCompleteMessages = false,
                    MaxConcurrentCalls = 1,
                    ReceiveMode = ServiceBusReceiveMode.PeekLock,
                    MaxAutoLockRenewalDuration = TimeSpan.FromMinutes(3),
                });

            processor.ProcessMessageAsync += async (args) =>
            {
                var stopwatch = Stopwatch.StartNew();
                string rawBody = string.Empty;

                try
                {
                    rawBody = args.Message.Body.ToString();

                    var queueMsg = JsonConvert.DeserializeObject<AzureQueueMessageApi>(rawBody);
                    if (queueMsg == null)
                    {
                        NetCommonHelper.Logger.DevLog.Instance.WriteError(
                            string.Format("SII Status: cannot deserialize AzureQueueMessageApi. Body={0}", rawBody));
                        return;
                    }

                    string siiMessageJson;
                    if (queueMsg.Params == null ||
                        !queueMsg.Params.TryGetValue("message", out siiMessageJson) ||
                        string.IsNullOrWhiteSpace(siiMessageJson))
                    {
                        NetCommonHelper.Logger.DevLog.Instance.WriteError(
                            string.Format("SII Status: missing 'message' param. Body={0}", rawBody));
                        return;
                    }

                    var siiEnvelope = JsonConvert.DeserializeObject<SiiStatusEnvelope>(siiMessageJson);
                    if (siiEnvelope == null || siiEnvelope.message == null)
                    {
                        NetCommonHelper.Logger.DevLog.Instance.WriteError(
                            string.Format("SII Status: invalid envelope or inner message. Body={0}", siiMessageJson));
                        return;
                    }

                    var updateService = new SiiStatusUpdateService(_tenant);
                    updateService.UpdateStatus(siiEnvelope.message);

                    await args.CompleteMessageAsync(args.Message);

                    NetCommonHelper.Logger.DevLog.Instance.WriteInfo(
                        string.Format("SII Status processed successfully. Request={0}, Tenant={1}, Time={2}",
                            siiEnvelope.message.requestNumber, _tenant, stopwatch.Elapsed));
                }
                catch (Exception e)
                {
                    if (IsAuthError(e))
                    {
                        NetCommonHelper.Logger.DevLog.Instance.WriteError(
                            string.Format(
                                "SII Status FATAL auth error while processing message. Error={0} Body={1}. Disabling worker until restart.",
                                e, rawBody));

                        try
                        {
                            await args.AbandonMessageAsync(args.Message);
                        }
                        catch (Exception abandonEx)
                        {
                            NetCommonHelper.Logger.DevLog.Instance.WriteError(
                                string.Format("SII Status error abandoning message after auth failure. Error={0}", abandonEx));
                        }

                        _hasConfig = false;
                        return;
                    }

                    NetCommonHelper.Logger.DevLog.Instance.WriteError(
                        string.Format("SII Status processing error. Error={0} Body={1}", e, rawBody));
                }
                finally
                {
                    stopwatch.Stop();
                }
            };

            processor.ProcessErrorAsync += async (args) =>
            {
                var ex = args.Exception;

                if (IsAuthError(ex))
                {
                    NetCommonHelper.Logger.DevLog.Instance.WriteError(
                        string.Format(
                            "SII Status queue FATAL auth error. ErrorSource={0}, Exception={1}. Stopping processor and disabling worker until restart.",
                            args.ErrorSource, ex));

                    try
                    {
                        await processor.StopProcessingAsync();
                    }
                    catch (Exception stopEx)
                    {
                        NetCommonHelper.Logger.DevLog.Instance.WriteError(
                            string.Format(
                                "SII Status queue error while stopping after auth failure: {0}",
                                stopEx));
                    }

                    _hasConfig = false;

                    return;
                }

                NetCommonHelper.Logger.DevLog.Instance.WriteError(
                    string.Format("SII Status queue error. ErrorSource={0}, Exception={1}",
                        args.ErrorSource, ex));
            };

            processor.StartProcessingAsync().GetAwaiter().GetResult();
            return processor;
        }

        public void DebugStep()
        {
            ConnectClient();
            ExecuteQueue();
        }

        private static bool IsAuthError(Exception ex)
        {
            if (ex is UnauthorizedAccessException)
                return true;

            if (ex is RequestFailedException rfe && rfe.Status == 401)
                return true;

            if (ex.InnerException != null)
                return IsAuthError(ex.InnerException);

            return false;
        }

        private static string GetMandatoryDefault(int tenant, string key)
        {
            var def = DefaultService.Instance.Get(tenant, key, key);
            if (def == null || string.IsNullOrWhiteSpace(def.Value1))
            {
                throw new ConfigurationErrorsException(
                    string.Format("Default key '{0}' is missing or empty for tenant {1}.", key, tenant));
            }

            return def.Value1;
        }
    }
}
