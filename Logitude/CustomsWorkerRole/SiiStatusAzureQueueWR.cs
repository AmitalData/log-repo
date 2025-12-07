using Azure;
using Azure.Messaging.ServiceBus;
using Logitude.BL.Helpers;
using Logitude.Customs.BL.BL.SIIRequest;
using Logitude.Customs.BL.CloseTables;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data.DataContracts.SIIRequest;
using Logitude.Server.Tools.RestRequestExecutor;
using Newtonsoft.Json;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using WebFreight.Web.Helpers.WorkerRoleHelpers;
using Logitude.Customs.Def.EntityPMs;

namespace CustomsWorkerRole
{
    public class SiiStatusAzureQueueWR : CustomsWorkerEntryPoint
    {
        private readonly int _seedDefaultTenant;
        private List<CustomsSettingPM> _allCustomsSettings;
        private readonly List<ServiceBusProcessor> _processors = new List<ServiceBusProcessor>();
        private readonly HashSet<int> _tenantsMissingConfig = new HashSet<int>();
        bool _onStartDone = false;

        public SiiStatusAzureQueueWR()
        {
            _seedDefaultTenant = SettingUtil.GetCurrentTenant();
            if (_seedDefaultTenant == -1)
            {
                _seedDefaultTenant = 0;
            }
        }

        public override bool OnStart()
        {
            ThreadId = Guid.NewGuid().ToString();
            DoneItemsInRange = new Dictionary<DateTime, int>();
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

        public override void WorkOnce()
        {
            try
            {
                if (_onStartDone) return;
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
            var customsSettingQueryService = new CustomsSettingQueryService(_seedDefaultTenant);
            _allCustomsSettings = customsSettingQueryService.GetAll();

            foreach (var setting in _allCustomsSettings)
            {
                StartTenantProcessor(setting.Tenant);
            }

            if (_tenantsMissingConfig.Count > 0)
            {
                var list = string.Join(", ", _tenantsMissingConfig);
                NetCommonHelper.Logger.DevLog.Instance.WriteInfo(
                    $"SII Status WR: tenants without SII status queue config or CustomsAgentId: {list}");
            }
            else
            {
                NetCommonHelper.Logger.DevLog.Instance.WriteInfo(
                    "SII Status WR: all tenants have SII status queue config.");
            }

            return null;
        }

        private void StartTenantProcessor(int tenant)
        {
            try
            {
                var connectionString = GetMandatoryDefault(tenant, "SIIStatusQueueConn");
                var queuePrefix = GetMandatoryDefault(tenant, "SIIStatusQueuePrefix");

                var setting = CustomsSettingQueryService.GetSettingByTenant(tenant);
                var consumerId = setting?.CustomsAgentId;

                if (string.IsNullOrWhiteSpace(consumerId))
                {
                    _tenantsMissingConfig.Add(tenant);
                    NetCommonHelper.Logger.DevLog.Instance.WriteDebug(
                        $"SII Status WR: skipping tenant {tenant} – CustomsAgentId is missing or empty.");
                    return;
                }

                var queueName = queuePrefix + consumerId;

                NetCommonHelper.Logger.DevLog.Instance.WriteInfo(
                    $"SII Status WR: starting processor. Tenant={tenant}, Queue={queueName}");

                var client = new ServiceBusClient(connectionString);

                var processor = client.CreateProcessor(
                    queueName,
                    new ServiceBusProcessorOptions
                    {
                        AutoCompleteMessages = false,
                        MaxConcurrentCalls = 1,
                        ReceiveMode = ServiceBusReceiveMode.PeekLock,
                        MaxAutoLockRenewalDuration = TimeSpan.FromMinutes(3),
                    });

                processor.ProcessMessageAsync += args => ProcessMessageForTenantAsync(args, tenant);
                processor.ProcessErrorAsync += args => ProcessErrorForTenantAsync(args, tenant, processor);

                processor.StartProcessingAsync().GetAwaiter().GetResult();
                _processors.Add(processor);
            }
            catch (ConfigurationErrorsException)
            {
                _tenantsMissingConfig.Add(tenant);
                NetCommonHelper.Logger.DevLog.Instance.WriteDebug(
                    $"SII Status WR: skipping tenant {tenant} – SIIStatusQueueConn or SIIStatusQueuePrefix not configured.");
            }
            catch (Exception ex)
            {
                NetCommonHelper.Logger.DevLog.Instance.WriteError(
                    $"SII Status WR: failed to start processor for tenant {tenant}. Error={ex}");
            }
        }

        private async Task ProcessMessageForTenantAsync(ProcessMessageEventArgs args, int tenant)
        {
            var stopwatch = Stopwatch.StartNew();
            string rawBody = string.Empty;
            string siiMessageJson = string.Empty;
            string declarationId = null;

            try
            {
                rawBody = args.Message.Body.ToString();

                var queueMsg = JsonConvert.DeserializeObject<AzureQueueMessageApi>(rawBody);
                if (queueMsg == null)
                {
                    NetCommonHelper.Logger.DevLog.Instance.WriteError(
                        $"SII Status: cannot deserialize AzureQueueMessageApi. Body={rawBody}");

                    await LogSiiStatusCommunicationAsync(tenant, null, rawBody, false, "Cannot deserialize AzureQueueMessageApi");
                    await args.DeadLetterMessageAsync(
                        args.Message,
                        "BadMessage",
                        "Cannot deserialize AzureQueueMessageApi");
                    return;
                }

                if (queueMsg.Params == null ||
                    !queueMsg.Params.TryGetValue("message", out siiMessageJson) ||
                    string.IsNullOrWhiteSpace(siiMessageJson))
                {
                    NetCommonHelper.Logger.DevLog.Instance.WriteError(
                        $"SII Status: missing 'message' param. Body={rawBody}");
                    await args.DeadLetterMessageAsync(
                         args.Message,
                         "BadMessage",
                         "Missing 'message' param in AzureQueueMessageApi.Params");

                    await LogSiiStatusCommunicationAsync(tenant, null, rawBody, false, "Missing 'message' param");
                    return;
                }

                var siiEnvelope = JsonConvert.DeserializeObject<SiiStatusEnvelope>(siiMessageJson);
                if (siiEnvelope == null || siiEnvelope.message == null)
                {
                    NetCommonHelper.Logger.DevLog.Instance.WriteError(
                        $"SII Status: invalid envelope or inner message. Body={siiMessageJson}");

                    await LogSiiStatusCommunicationAsync(tenant, null, siiMessageJson, false, "Invalid envelope or inner message");
                    await args.DeadLetterMessageAsync(
                        args.Message,
                        "BadMessage",
                        "Invalid SiiStatusEnvelope or missing inner 'message'");
                    return;
                }

                var updateService = new SiiStatusUpdateService(tenant);
                declarationId = updateService.UpdateStatus(siiEnvelope.message);

                await args.CompleteMessageAsync(args.Message);

                await LogSiiStatusCommunicationAsync(tenant, declarationId, siiMessageJson, true, "SII status processed successfully");

                NetCommonHelper.Logger.DevLog.Instance.WriteInfo(
                    $"SII Status processed successfully. Request={siiEnvelope.message.requestNumber}, Tenant={tenant}, Time={stopwatch.Elapsed}");
            }
            catch (Exception e)
            {
                if (IsAuthError(e))
                {
                    NetCommonHelper.Logger.DevLog.Instance.WriteError(
                        $"SII Status FATAL auth error while processing message. Tenant={tenant}. Error={e} Body={rawBody}.");

                    try
                    {
                        await args.AbandonMessageAsync(args.Message);
                    }
                    catch (Exception abandonEx)
                    {
                        NetCommonHelper.Logger.DevLog.Instance.WriteError(
                            $"SII Status error abandoning message after auth failure. Tenant={tenant}. Error={abandonEx}");
                    }

                    await LogSiiStatusCommunicationAsync(tenant, declarationId, rawBody, false, $"Auth error: {e}");

                    return;
                }

                NetCommonHelper.Logger.DevLog.Instance.WriteError(
                    $"SII Status processing error. Tenant={tenant}. Error={e} Body={rawBody}");

                await LogSiiStatusCommunicationAsync(tenant, declarationId, rawBody, false, $"Processing error: {e}");
            }
            finally
            {
                stopwatch.Stop();
            }
        }

        private async Task ProcessErrorForTenantAsync(ProcessErrorEventArgs args, int tenant, ServiceBusProcessor processor)
        {
            var ex = args.Exception;

            try
            {
                if (IsAuthError(ex))
                {
                    NetCommonHelper.Logger.DevLog.Instance.WriteError(
                        $"SII Status queue FATAL auth error. Tenant={tenant}. ErrorSource={args.ErrorSource}, Exception={ex}. Stopping processor.");

                    try
                    {
                        await processor.StopProcessingAsync();
                    }
                    catch (Exception stopEx)
                    {
                        NetCommonHelper.Logger.DevLog.Instance.WriteError(
                            $"SII Status queue error while stopping after auth failure. Tenant={tenant}. Error={stopEx}");
                    }

                    return;
                }

                NetCommonHelper.Logger.DevLog.Instance.WriteError(
                    $"SII Status queue error. Tenant={tenant}. ErrorSource={args.ErrorSource}, Exception={ex}");
            }
            catch (Exception logEx)
            {
                NetCommonHelper.Logger.DevLog.Instance.WriteError(
                    $"SII Status ProcessErrorAsync failed while logging. Tenant={tenant}. Error={logEx}");
            }
        }

        public void DebugStep()
        {
            ExecuteQueue();
        }

        private async Task LogSiiStatusCommunicationAsync(
            int tenant,
            string declarationId,
            string payload,
            bool success,
            string remark)
        {
            if (string.IsNullOrWhiteSpace(declarationId))
            {
                NetCommonHelper.Logger.DevLog.Instance.WriteInfo(
                    $"SII Status comm log skipped (no declaration). Tenant={tenant}. {remark}\r\n{payload}");
                return;
            }

            try
            {
                var status = success ? StatusTypeCommunication.Done : StatusTypeCommunication.Failed;

                var factory = new SIIRequestApiRequestFactory(tenant);

                var comm = factory.BuildCommunicationsDto(
                    CustomsPartnerFtpDetails.InterfaceName_SIIRequestStatus,
                    CustomsPartnerFtpDetails.PartnerCode_SII,
                    declarationId);

                var logger = new ApiCommunicationLog();

                var payloadWithRemark = string.IsNullOrWhiteSpace(remark)
                    ? payload
                    : $"{remark}\r\n{payload}";

                await logger.AddCommunicationLogAsync(
                    requestPayload: string.Empty,
                    responsePayload: payloadWithRemark,
                    tenant: tenant,
                    statusTypeCode: status,
                    comm: comm);
            }
            catch (Exception ex)
            {
                NetCommonHelper.Logger.DevLog.Instance.WriteError(
                    $"SII Status: failed to write communication log. Tenant={tenant}. Error={ex}");
            }
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
