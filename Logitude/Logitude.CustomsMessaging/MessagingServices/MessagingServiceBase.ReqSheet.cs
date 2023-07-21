using Logitude.Customs.BL.Messaging.Customs;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.Repositories;
using System.Diagnostics;
using System;
using Logitude.AmitalMessaging.Utils;
using System.Collections.Generic;
using System.IO;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Logitude.Server.Tools.Counters;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Server.Tools.ExternalServices;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data.EntityKeys;
using Logitude.CustomsMessaging.Common.DCAParams;
using System.Threading;
using Logitude.Customs.Def.ClosedTable;
using Logitude.CustomsMessaging.Common.RequestParams;
using System.Text;

using Logitude.Server.Tools.Models;
using System.Transactions;
using Logitude.Customs.BL.Utils;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.CustomsMessaging.Dca;
using Logitude.Server.Tools.QueueService;
using Logitude.CustomsMessaging.Helpers;
using Logitude.CustomsMessaging.Common.ResponseData;
using Newtonsoft.Json;
using Logitude.Customs.Data;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools.Utils;
using Logitude.Customs.Def.Messaging.Customs;
using System.Linq;
using System.Configuration;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.CustomsMessaging.Testers.Messages;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.BL.Exceptions;

namespace Logitude.CustomsMessaging.MessagingServices
{
    public abstract partial class MessagingServiceBase<TRequestParams, TResponseData, TCustomsRequest, TCustomsResponse, TRequestService, TResponseService, TRequestHeader>
    : IMessagingServiceBase<TRequestParams, TResponseData, TCustomsRequest, TCustomsResponse, TRequestHeader>
    {
        private CustomsRequestsSheetDomainModelService<TRequestParams> _CustomsRequestsSheetService;
        //private InterfaceManagementPM _MainMessageDefinition;


        const int _TimeOut = 3;
        const string _MessageTimeOut = "MessageTimeOut";
        private CustomsStateMachineProcess _CustomsStateMachineProcess;
        private bool _HugeFile;
        protected UnifreightIIG.Common.TheGateway.MoreParams _IIGGatewayMoreParams;
        private Stopwatch _swMessagingServiceBase;




        virtual protected TRequestParams CreateDefaultRequestParamsFromCustomsResponse(TCustomsResponse customsResponse)
        {
            return new TRequestParams(); //DUE DUAL 
            throw new NotImplementedException("virtual partial class MessagingServiceBase:TRequestParams CreateDefaultRequestParamsFromCustomsResponse(TCustomsResponse customsResponse):" + this.GetType().FullName);
        }


        virtual protected bool? IsOurEnvironment(TCustomsResponse customsResponse, TRequestParams RequestParams)
        {
            return null;
        }
        virtual protected DcaReceivedController GetDcaReceivedController(TCustomsResponse customsResponse, TRequestParams RequestParams)
        {
            return null;
        }

        virtual protected Logitude.CustomsMessaging.Common.RequestParams.RequestSheetParam GetSheetDetailsFromRequestParam(TRequestParams requestParams)
        {
            return CustomsRequestsSheetDomainModelService<TRequestParams>.GetSheetDetailsFromRequestParam(requestParams);

        }
        abstract public string MainInterfaceCode { get; }


        virtual protected bool ToValidateCustomsRequestB4Send()
        {
            return true;

        }


        public TResponseData SendSheet(TRequestParams requestParams, TCustomsRequest customsRequestCalc = null)
        {
            TResponseData responseData = null;
            ///var u = this.UnityId;
            //CustomsRequestsSheetService<TRequestParams> customsRequestsSheetService;
            bool toCommit = true;
            ///using (var sendScope = TransactionFactory.GetTransaction())
            TransactionScope sendScope = null;

            try
            {
                sendScope = TransactionFactory.GetTransaction();
                requestParams.InterfaceTypeCode = this.MainInterfaceCode;
                RequestSheetParam reqSheetDetails = this.GetSheetDetailsFromRequestParam(requestParams);
                requestParams.LoggingEnabled = true;
                if (Environment.MachineName.ToLower().Equals("itzik-7-new"))
                {
                    //requestParams.SuppressSplitWR = true;
                }
                try
                {

                    var isInteractive = true;
                    CustomsRequestsSheetDomainModelService<TRequestParams>.CreateNew(requestParams, reqSheetDetails, isInteractive, out _CustomsRequestsSheetService);
                }
                catch (CustomsRequestsSheetDomainModelServiceException customsRequestsSheetServiceException)
                {
                    toCommit = false;
                    responseData = new TResponseData();
                    responseData.Succeeded = false;
                    responseData.UserMessage = customsRequestsSheetServiceException.Message;
                    if (!customsRequestsSheetServiceException.SuppressExceptionTostring)
                    {
                        responseData.UserMessage += Environment.NewLine + customsRequestsSheetServiceException.ToString();
                    }
                    responseData.HasException = true;
                    return responseData;
                }
                catch (Exception ex)
                {
                    toCommit = false;
                    responseData = new TResponseData();
                    responseData.Succeeded = false;
                    responseData.UserMessage = ex.Message + Environment.NewLine + ex.ToString();
                    responseData.HasException = true;
                    return responseData;
                }



                

                //_CustomsRequestsSheetService.IsInteractive = true;
                if (!_CustomsRequestsSheetService.InBatchModeToCreateQ())
                {
                    if (sendScope != null)
                    {
                        if (toCommit)
                        {
                            sendScope.Complete();
                        }
                        sendScope.Dispose(); sendScope = null;
                    }
                    // from now on 'not in trans - due Flush to client + Show steps !!
                    return SendSheet();
                }
                responseData = new TResponseData();
                try
                {

                    if (!requestParams.SuppressSplitWR)
                    {
                        SBQMessageService.CreateBasic<CustomsCommandEnum>(
                             CustomsCommandEnum.CustomsCommandGetCustomRequestWR,
                            _CustomsRequestsSheetService.MyCustomsRequestsSheetPM.Tenant,
                            _CustomsRequestsSheetService.MyCustomsRequestsSheetPM.InterfaceTypeCode,
                            _CustomsRequestsSheetService.MyCustomsRequestsSheetPM.Id,
                            this.RequestParams.FutureSendDateTime
                            );

                        responseData.Succeeded = true;
                        responseData.ContinueProcessInBackground = true;
                        responseData.CustomsRequestsSheetId = _CustomsRequestsSheetService.MyCustomsRequestsSheetPM.Id;
                        return responseData;
                    }
                    else
                    {
                        SBQMessageService.CreateBasic(
                        SBQueueNames.CustomsMessagingSheetBQ,
                        _CustomsRequestsSheetService.MyCustomsRequestsSheetPM.Tenant,
                        _CustomsRequestsSheetService.MyCustomsRequestsSheetPM.InterfaceTypeCode,
                        _CustomsRequestsSheetService.MyCustomsRequestsSheetPM.Id);

                        responseData.Succeeded = true;
                        responseData.ContinueProcessInBackground = true;
                        responseData.CustomsRequestsSheetId = _CustomsRequestsSheetService.MyCustomsRequestsSheetPM.Id;
                        return responseData;
                    }
                }
                catch (Exception eex)
                {
                    responseData = new TResponseData();
                    responseData.Succeeded = false;
                    responseData.UserMessage = "Exception  While build Queue" + eex.Message + Environment.NewLine + eex.ToString();
                    responseData.HasException = true;
                    toCommit = false;
                    return responseData;
                }
                finally
                {
                    if (false)
                    {

                        if (_CustomsRequestsSheetService != null)
                        {

                            _CustomsRequestsSheetService.UpsertClientProgressBarIndicatorCurrentStage(null,
                                () =>
                                {
                                    var xml = XmlGenericUtil<TResponseData>.SerializeObject(responseData);
                                    return xml;
                                });
                        }
                    }
                    else if (responseData != null && !toCommit)
                    {
                        var xml = XmlGenericUtil<TResponseData>.SerializeObject(responseData);
                        Simplog.Server.Infrastructure.Helpers.CacheManager.CacheWrapper
            .Insert("Customs.General.RequestStopedNoteClient," + requestParams.PBId, xml);
                    }


                }

            }
            finally
            {
                if (sendScope != null)// if batch= BuildQueue
                {
                    if (toCommit) // only if success to BuildQueue
                    {
                        sendScope.Complete();
                    }
                    sendScope.Dispose();
                }
                if (_CustomsRequestsSheetService != null)
                {
                    _CustomsRequestsSheetService.Dispose();
                }
                RequestSheetContext.Current.Dispose();
            }

        }
        public object ReQueue(int tenant, string customsRequestsSheetId,string parentId=null)
        {

            using (var scope = TransactionFactory.GetTransaction())
            {

                this.MyOverrideControllerModel = new OverrideControllerModel();
                CreateCustomsRequestsSheetService(tenant, customsRequestsSheetId,parentId);
                switch (_CustomsRequestsSheetService.MyCustomsRequestsSheetPM.RequestStatusEnum)
                {
                    case SheetStatusEnum.Received:
                    case SheetStatusEnum.AnalyzeFailed:
                        _CustomsRequestsSheetService.ReAnalyzeStatusReceivedCreateQ();

                        break;

                    case SheetStatusEnum.Created:
                        {
                            _CustomsRequestsSheetService.ReCreateNow();
                        }
                        break;
                    default:
                        throw new Exception("SheetStatus  not Valid");
                        break;
                }

                scope.Complete();
            }
            return null;

        }


        public object SendSheet(int tenant, string customsRequestsSheetId,
            SendSheetSignModel mySendSheetSignModel = null)
        {

            Logitude.Server.Tools.Helpers.LogMessagingUtil.Instance.AppendLine("SendSheet::" + Environment.StackTrace);

            //CustomsRequestsSheetService<TRequestParams> customsRequestsSheetService;
            try
            {

                try
                {

                    _SignRecievedModel = mySendSheetSignModel;
                    CreateCustomsRequestsSheetService(tenant, customsRequestsSheetId);
                    if (_SignRecievedModel != null)
                    {
                        if (_CustomsRequestsSheetService.InBatchModeToCreateQ())
                        {
                            CurrentCustomsCommandWR = CustomsCommandEnum.CustomsCommandSignRequestWR;
                        }
                        else
                        {
                            _CustomsRequestsSheetService.SetIsInteractive();
                        }
                    }
                    _CustomsRequestsSheetService.CurrentWR = CurrentCustomsCommandWR;
                }
                catch (CustomsRequestsSheetDomainModelServiceException customsRequestsSheetServiceException)
                {
                    var responseData = new TResponseData();
                    responseData.Succeeded = false;
                    responseData.UserMessage = customsRequestsSheetServiceException.Message + Environment.NewLine + customsRequestsSheetServiceException.ToString();
                    responseData.HasException = true;
                    return responseData;
                }
                catch (Exception ex)
                {

                    var responseData = new TResponseData();
                    responseData.Succeeded = false;
                    responseData.UserMessage = ex.Message + Environment.NewLine + ex.ToString();
                    responseData.HasException = true;
                    return responseData;
                }

                //_CustomsRequestsSheetService = customsRequestsSheetService;
                TResponseData responseData1 = SendSheet();
                var obj = responseData1 as object;
                return obj;


            }
            finally
            {
                if (_CustomsRequestsSheetService != null)
                {
                    _CustomsRequestsSheetService.Dispose();
                }
                RequestSheetContext.Current.Dispose();
            }
        }

        private void CreateCustomsRequestsSheetService(int tenant, string customsRequestsSheetId, string parentId = null)
        {
            CustomsRequestsSheetDomainModelService<TRequestParams>.Seed(customsRequestsSheetId, tenant, null, out _CustomsRequestsSheetService, this.MyOverrideControllerModel,false,null, parentId);
        }

        private TResponseData SendSheet()
        {
            Logitude.Server.Tools.Helpers.LogMessagingUtil.Instance.AppendLine("SendSheet22::" + Environment.StackTrace);

            if (_CustomsRequestsSheetService == null)
            {
                throw new Exception("SendSheetStateMachine():(_CustomsRequestsSheetService == null)");
            }


            TCustomsRequest customsRequest = default(TCustomsRequest);
            TCustomsResponse customsResponse = default(TCustomsResponse);
            TResponseData responseData = default(TResponseData);
            DCAServerUploadResponse dCAServerUploadResponse = null;
            DCAServerUploadStatus dCAServerUploadStatus = null;
            //LogMessagingUtil.Instance.Clear();
            Stopwatch totalStopwatch = null;




            try
            {
                AmitalDebuggerUtil.Break();

                totalStopwatch = Stopwatch.StartNew();





                var transitions = InitTransition();


                var CommandList = new List<CustomsRCmmand>()
                {
                    new CustomsRCmmand(CustomsCommandEnum.CustomsCommandGetCustomRequestWR, (o) =>{ return CustomsCommandGetCustomRequest(out customsRequest);   }) ,
                    new CustomsRCmmand(CustomsCommandEnum.CustomsCommandSignRequestWR, (o) =>{return CustomsCommandSign(customsRequest);}),


                    new CustomsRCmmand(CustomsCommandEnum.CustomsCommandSendWSReceiveCorrelationWR, (o) =>{return CustomsCommandSendWS(customsRequest, ref customsResponse,ref responseData);}),

                    new CustomsRCmmand(CustomsCommandEnum.CustomsCommandSendDCAWR, (o)=>{ return CustomsCommandSendDCA(customsRequest, out  dCAServerUploadResponse);  }),
                    new CustomsRCmmand(CustomsCommandEnum.CustomsCommandSendDCAUploadStatusWR , (o)=>{ return CustomsCommandDCAUploadStatus(dCAServerUploadResponse, out  dCAServerUploadStatus);  }),
                    ///new CustomsRCmmand(CustomsCommandEnum.CustomsCommandDownloadDcaReceiveCorrelationWR , (o) => this.DcaReceivedCustomResponseCorrelation( 
                    new CustomsRCmmand(CustomsCommandEnum.CustomsCommandAnalyzeResponseWR, (o) =>{ return CustomsCommandAnalyzeResponse(customsResponse, ref responseData);})
                };


                _CustomsStateMachineProcess = new CustomsStateMachineProcess(CommandList, transitions);
                var currentWR = "WebRole";
                if (_CustomsRequestsSheetService.CurrentWR.HasValue)
                {
                    currentWR = _CustomsRequestsSheetService.CurrentWR.Value.ToString();
                }
                LogMessagingUtil.Instance.AppendLine("currentWR =" + currentWR + " StartCustomsRequestStep  = " + _CustomsRequestsSheetService.StartCustomsRequestStepEnum);

                Stopwatch stopwatch = null;



                if (GotoAnalyzeDueDCAIn())
                {
                    TRequestParams requestParams;
                    if (!GetSheetCustomResponse(ref customsResponse, out requestParams))
                    {
                        throw new Exception("GetSheetCustomResponse() failed !!!");
                    }
                    if (!CustomsCommandSign(customsRequest))
                    {
                    }
                }
                else
                {



                    //stopwatch = Stopwatch.StartNew();
                    //LogMessagingUtil.Instance.AppendLine("Current State = " + _CustomsStateMachineProcess.MoveNext(CustomsCommandEnum.CustomsCommandGetCustomRequestWR, GetCustomRequestError));
                    //LogMessagingUtil.Instance.AppendLine("SendWS:Took:" + stopwatch.Elapsed.ToString());
                    _CustomsStateMachineProcess.MoveNext(CustomsCommandEnum.CustomsCommandGetCustomRequestWR);

                    RaiseTimeout(totalStopwatch);
                    stopwatch = Stopwatch.StartNew();
                    LogMessagingUtil.Instance.AppendLine("Current State = " + _CustomsStateMachineProcess.MoveNext(CustomsCommandEnum.CustomsCommandSignRequestWR));
                    LogMessagingUtil.Instance.AppendLine("Sign:took:" + stopwatch.Elapsed.ToString());
                    RaiseTimeout(totalStopwatch);

                    if (
                        //_CustomsRequestsSheetService.VIA() 
                        _CustomsRequestsSheetService.GetRequestParams<TRequestParams>().RequestVIA == Common.RequestParams.SendRequestVIA.DCABatch
                    //_CustomsRequestsSheetService.GetRequestParams<TRequestParams>().RequestVIA== Common.RequestParams.SendRequestVIA.DCABatch 
                    //||
                    //_CustomsRequestsSheetService.GetInteractiveMode() == Customs.BL.EntityPMs.IIGMessagePM.InteractiveMode.DCA
                    )
                    {
                        stopwatch = Stopwatch.StartNew();
                        LogMessagingUtil.Instance.AppendLine("Current State = " + _CustomsStateMachineProcess.MoveNext(CustomsCommandEnum.CustomsCommandSendDCAWR));
                        LogMessagingUtil.Instance.AppendLine("SendDCA:took:" + stopwatch.Elapsed.ToString());


                        stopwatch = Stopwatch.StartNew();
                        LogMessagingUtil.Instance.AppendLine("Current State = " + _CustomsStateMachineProcess.MoveNext(CustomsCommandEnum.CustomsCommandSendDCAUploadStatusWR));
                        LogMessagingUtil.Instance.AppendLine("DCAUploadStatus:took:" + stopwatch.Elapsed.ToString());

                        return new TResponseData()
                        {
                            Succeeded = true,
                            CustomsRequestsSheetId = _CustomsRequestsSheetService.MyCustomsRequestsSheetPM.Id,
                            ContinueProcessInBackground = true,
                            UserMessage = "WAIT TO REPLY from dca "

                        };
                    }
                    else
                    {
                        stopwatch = Stopwatch.StartNew();
                        LogMessagingUtil.Instance.AppendLine("Current State = " + _CustomsStateMachineProcess.MoveNext(CustomsCommandEnum.CustomsCommandSendWSReceiveCorrelationWR));
                        LogMessagingUtil.Instance.AppendLine("SendWS:took:" + stopwatch.Elapsed.ToString());
                    }
                }//GotoAnalyzeDueDCAIn())
                RaiseTimeout(totalStopwatch);
                LogMessagingUtil.Instance.AppendLine("Current State = " + _CustomsStateMachineProcess.MoveNext(CustomsCommandEnum.CustomsCommandAnalyzeResponseWR));
                LogMessagingUtil.Instance.AppendLine("SendBatch- Done !!!");



            }
            catch (CustomsRequestsSheetDomainModelServiceException cex)
            {
                if (cex.What2Do == CustomsRequestsSheetDomainModelServiceException.What2DoEnum.NewQueueCreated)
                {
                    if (responseData == null)
                    {
                        responseData = new TResponseData(); //{ Succeeded = true,  ContinueProcessInBackground= true , HasException = false };
                    }
                    responseData.Succeeded = true;
                    responseData.ContinueProcessInBackground = true;
                    responseData.HasException = false;
                }
                else if (cex.What2Do == CustomsRequestsSheetDomainModelServiceException.What2DoEnum.CancelRequest)
                {
                    LogMessagingUtil.Instance.AppendLine("SendSheet():What2DoEnum.CancelRequest " + cex.Message);
                    var resData = new TResponseData();
                    responseData = new TResponseData() { Succeeded = false, UserMessage = cex.Message, HasException = true };


                    //CustomsRequestsSheetDomainModelServiceException customsRequestsSheetServiceException =
                    _CustomsRequestsSheetService.FailSheet(cex, true);
                    //   throw customsRequestsSheetServiceException;




                }
                else
                {
                    if (!_CustomsRequestsSheetService.IsInteractive)
                    {
                        throw;
                    }
                    else
                    {
                        var mess = cex.Message;
                        if (cex.InnerException != null)
                        {
                            mess += Environment.NewLine;
                            mess += cex.InnerException.Message;
                        }
                        if (responseData == null)
                        {
                            responseData = new TResponseData() { Succeeded = false, UserMessage = mess + "Please Look at Request Sheet for more details ", HasException = true };

                        }
                    }
                }
            }

            catch (BusinessErrorException businessErrorException)
            {
                LogMessagingUtil.Instance.AppendLine("SendSheet():BusinessErrorException " + businessErrorException.Message);
                var resData = businessErrorException.CurrentContextTag as TResponseData;
                if (resData == null)
                {

                    if (!_CustomsRequestsSheetService.IsInteractive)
                    {
                        CustomsRequestsSheetDomainModelServiceException customsRequestsSheetServiceException =
                        _CustomsRequestsSheetService.FailSheet(businessErrorException);
                        throw customsRequestsSheetServiceException;
                    }
                    else
                    {
                        if (responseData == null)
                        {
                            responseData = new TResponseData() { Succeeded = false, UserMessage = businessErrorException.Message, HasException = true };
                        }
                    }
                }
                else
                {
                    responseData = resData;
                }
            }
            catch (AnotherThreadHandlingException) { }
            catch (Exception curException)
            {
                //success = false;

                LogMessagingUtil.Instance.AppendLine("SendSheet():Exception " + curException.ToString());
                CustomsRequestsSheetDomainModelServiceException customsRequestsSheetServiceException =
                    _CustomsRequestsSheetService.FailSheet(curException);

                if (!_CustomsRequestsSheetService.IsInteractive)
                {
                    throw customsRequestsSheetServiceException;
                }
                else
                {
                    if (responseData == null)
                    {
                        responseData = new TResponseData() { Succeeded = false, UserMessage = "Please Look at Request Sheet for more details", HasException = true };

                    }
                }

            }
            finally
            {
                if (responseData == null)
                {
                    responseData = new TResponseData();

                }
                responseData.CustomsRequestsSheetId = _CustomsRequestsSheetService.MyCustomsRequestsSheetPM.Id;
                if (string.IsNullOrWhiteSpace(_CorrelationId))
                {
                    responseData.CorrelationId = _CorrelationId;
                }


                if (_CustomsRequestsSheetService != null)
                {

                    _CustomsRequestsSheetService.UpsertClientProgressBarIndicatorCurrentStage(null,
                        () =>
                        {
                            var xml = XmlGenericUtil<TResponseData>.SerializeObject(responseData);
                            return xml;
                        });
                }
                //_MessagingBatchService.LogMessaging = LogMessagingUtil.Instance.ToString();
                //_MessagingBatchService.Save(success);
                //processState = newProcessState;

            }
            return responseData;
        }



        private void GetCustomRequestError(object obj)
        {
            throw new NotImplementedException();
        }

        private bool GotoAnalyzeDueDCAIn()
        {
            //dca IN
            //{ new CustomsStateMachineProcess.StateTransition(CustomsRequestStepEnum.StartRequestParams, CustomsCommandEnum.AnalyzeResponse ), CustomsRequestStepEnum.AnalyzeResponseData} ,
            if
                (
                !_CustomsRequestsSheetService.IsInteractive &&
                _CustomsRequestsSheetService.MyCustomsRequestsSheetPM.IsDCA &&
                _CustomsRequestsSheetService.StartCustomsRequestStepEnum == CustomsStepEnum.AnalyzeResponseData
                )
            {
                LogMessagingUtil.Instance.AppendLine("Go to Analyze no need to rivive all");
                return true;
            }
            return false;
        }

        private bool CustomsCommandDCAUploadStatus(DCAServerUploadResponse dCAServerUploadResponse, out DCAServerUploadStatus dCAServerUploadStatus, DCAServerUploadStatus dCAServerUploadStatusFromWebForm = null)
        {
            var toContinueNextCommand = true;
            string MessageOut = "";
            try
            {
                if (_CustomsRequestsSheetService.StartCustomsRequestStepEnum > CustomsStepEnum.DCAInProgressUploaded)
                {
                    if (_CustomsRequestsSheetService.MyCustomsRequestsSheetPM.RequestStatusEnum == SheetStatusEnum.SendFailed)
                    {
                        //LogMessagingUtil.Instance.AppendLine("CustomsRequestsSheet SheetStatusEnum =SendFailed, long time no see u = welcome back");

                    }
                    else
                    {


                        dCAServerUploadStatus = _CustomsRequestsSheetService.GetDCAServerUploadStatus();
                        if (dCAServerUploadStatus == null)
                        {
                            throw new Exception("_CustomsRequestsSheetService.GetDCAServerUploadStatus()==null : " + _CustomsRequestsSheetService.MyCustomsRequestsSheetPM.Id);
                        }

                        return toContinueNextCommand;
                    }
                }

                Nullable<CustomsCommandEnum> curComm = null;
                if (_CustomsStateMachineProcess != null)
                {
                    curComm = _CustomsStateMachineProcess.CurrentCommand;
                }
                _CustomsRequestsSheetService.StartStep(CustomsStepEnum.DCAInProgressUploaded, curComm);



                string UnifreightQueueOutStatus = "";
                //string MessageOut="";
                if (dCAServerUploadStatusFromWebForm == null)
                {


                    var dcaManager = new DcaManager(this.CustomsSetting.DCAServiceAddress //  @"http://itzik7:5050/Unifreight/DCAService/basic"
                         , this.CustomsSetting.DCAPartnerVault  ///"IIG"
                        , _CustomsRequestsSheetService.MyCustomsRequestsSheetPM.Tenant);

                    dcaManager.GetOutgoingQueueStatus(
                        _CustomsRequestsSheetService.MyCustomsRequestsSheetPM.Id, dCAServerUploadResponse.FileName,
                        out UnifreightQueueOutStatus,
                        out MessageOut);

                    //                All UnifreightQueueOutStatusEnum are   NOT_FOUND,FAILD,IN_PROGRESS,SENT
                    //UnifreightQueueOutStatus  =SENT

                    LogMessagingUtil.Instance.AppendLine("UnifreightQueueOutStatus =" + UnifreightQueueOutStatus);
                    if (String.IsNullOrWhiteSpace(UnifreightQueueOutStatus))
                    {
                        LogMessagingUtil.Instance.AppendLine("UnifreightQueueOutStatus==null ; error " + MessageOut);
                        throw new Exception("UnifreightQueueOutStatus==null ; error  " + MessageOut);
                    }

                    dCAServerUploadStatus = new DCAServerUploadStatus();
                    dCAServerUploadStatus.TheDCAServerUploadResponse = dCAServerUploadResponse;
                    dCAServerUploadStatus.DcaMessage = MessageOut;
                    dCAServerUploadStatus.UnifreightQueueOutStatus = UnifreightQueueOutStatus;
                }
                else
                {
                    dCAServerUploadStatus = dCAServerUploadStatusFromWebForm;
                }

                var myStatus = dCAServerUploadStatus.GetStatusEum();
                LogMessagingUtil.Instance.AppendLine("myStatus:" + myStatus.ToString());




                switch (myStatus)
                {
                    case Logitude.CustomsMessaging.Common.DCAParams.DCAServerUploadStatus.StatusEum.IN_PROGRESS:
                        throw new Exception("The current file is IN_PROGRESS try again (If 2 time IN_PROGRESS - Check  if  cyberark is down ) ");
                        break;
                    case Logitude.CustomsMessaging.Common.DCAParams.DCAServerUploadStatus.StatusEum.SENT:
                        var memdCAServerUploadStatus =
                            XmlGenericUtil<DCAServerUploadStatus>.MemoryStreamSerialize(dCAServerUploadStatus);
                        //Serialize<DCAServerUploadStatus>(dCAServerUploadStatus);
                        LogMessagingUtil.Instance.AppendLine("Send Via DCA now we need to wait for reply from mehes dca !!");
                        toContinueNextCommand = _CustomsRequestsSheetService.EndStep(memdCAServerUploadStatus, null);
                        return toContinueNextCommand;
                        break;
                    case Logitude.CustomsMessaging.Common.DCAParams.DCAServerUploadStatus.StatusEum.LogitudeDefault:
                    case Logitude.CustomsMessaging.Common.DCAParams.DCAServerUploadStatus.StatusEum.NOT_FOUND:
                    case Logitude.CustomsMessaging.Common.DCAParams.DCAServerUploadStatus.StatusEum.FAILD:
                    default:
                        _CustomsRequestsSheetService.EndStep(null, null, CommStatusEnum.F);
                        throw new CustomsRequestsSheetDomainModelServiceException(
                    CustomsRequestsSheetDomainModelServiceException.WhereEnum.MessageServiceException,
                    CustomsRequestsSheetDomainModelServiceException.What2DoEnum.StopQueue, "Nothing to do !! GetOutgoingQueueStatus =" + myStatus.ToString(), null);
                        break;
                }


                return toContinueNextCommand;


            }
            catch (CustomsRequestsSheetDomainModelServiceException)
            {
                throw;
            }
            catch (Exception ex)
            {

                throw _CustomsRequestsSheetService.FailStepRaiseCRSSExeption(ex, "Sending request to DCA Server Failed ", null);

            }
        }

        private bool CustomsCommandSendDCA(TCustomsRequest customsRequest, out DCAServerUploadResponse dCAServerResponse)
        {
            //throw new Exception("TO DO SendDCA");  throw new NotImplementedException();

            string fileContentsBASE64 = "";
            string MessageOut = "";
            var toContinueNextCommand = true;
            try
            {


                if (_CustomsRequestsSheetService.StartCustomsRequestStepEnum > CustomsStepEnum.DCAInProgressUploading)
                {
                    dCAServerResponse = _CustomsRequestsSheetService.GetDCAServerUploadResponse();
                    if (dCAServerResponse == null)
                    {
                        throw new Exception("_CustomsRequestsSheetService.GetDCAServerResponse() : " + _CustomsRequestsSheetService.MyCustomsRequestsSheetPM.Id);
                    }
                    return toContinueNextCommand;
                }


                _CustomsRequestsSheetService.StartStep(CustomsStepEnum.DCAInProgressUploading, _CustomsStateMachineProcess.CurrentCommand);
                var sendSignVersion = (_CustomsRequestsSheetService.InterfaceTenantDefinitionManagement.InterfaceManagement.SignatureBy != SignQueueByType.None);
                if (!sendSignVersion)
                {
                    
                    sendSignVersion = _CustomsRequestsSheetService.GetRequestParams<TRequestParams>().ForcePersonalSign;
                }

                if (sendSignVersion)
                {
                    LogMessagingUtil.Instance.AppendLine("Send DCA Signed...");
                    //fileContentsBASE64 = Convert.ToBase64String(_CustomsRequestsSheetService.GetCustomsRequestSign());
                    LogMessagingUtil.Instance.AppendLine("ESBRequestSigned allways instance  from UnifreightIIG.Common.WarehouseBlockBalanceServiceReference.ESBRequestSigned !!! bad bad ");
                    var myESBRequestSigned = new UnifreightIIG.Common.WarehouseBlockBalanceServiceReference.ESBRequestSigned();
                    myESBRequestSigned.SignedByteArry = _CustomsRequestsSheetService.GetCustomsRequestSign(); ;
                    var xmlESBRequestSigned = XmlGenericUtil<UnifreightIIG.Common.WarehouseBlockBalanceServiceReference.ESBRequestSigned>.SerializeObject(myESBRequestSigned);
                    var bytesESBRequestSigned = System.Text.Encoding.UTF8.GetBytes(xmlESBRequestSigned);
                    fileContentsBASE64 = Convert.ToBase64String(bytesESBRequestSigned);
                    //customsResponse = TaskCallWSSigned(requestParams, customsRequest);
                }
                else
                {
                    LogMessagingUtil.Instance.AppendLine("Send DCA ...");
                    var bytes = System.Text.Encoding.UTF8.GetBytes(_CustomsRequestsSheetService.GetCustomsRequestXml());
                    fileContentsBASE64 = Convert.ToBase64String(bytes);
                    //customsResponse = TaskCallWS(requestParams, customsRequest);////no catch exeption
                }


                //ValidateCustomsRequestB4Send(customsRequest);

                //var myName = typeof(TCustomsRequest).Name;
                var reqService = new TRequestHeader();
                var serviceName = reqService.ServiceName;
                if (string.IsNullOrWhiteSpace(serviceName))
                {
                    throw new Exception("Can not resolve serviceName");
                }
                var setting = CustomsSettingQueryService.GetSettingByTenant(_CustomsRequestsSheetService.MyCustomsRequestsSheetPM.Tenant);
                bool isVualtTaskyam = !setting.TehilaDca;
                var sufix = CustomsSettingUtil.GetSufix(_CustomsRequestsSheetService.MyCustomsRequestsSheetPM.Tenant);

                var myParams = new DCAParams()
                {
                    consumerId = this.CustomsSetting.CustomsAgentId,
                    mehesCustomerId = "941079089",
                    Sufix = sufix
                }; //941079089

                var dCAOutFileName = DcaManager.GetDCAOutFileName(
                          serviceName,//_CustomsRequestsSheetService.CurrentMessageDefinition.DcaPrefixName,
                        _CustomsRequestsSheetService.MyCustomsRequestsSheetPM.Id, isVualtTaskyam,
                        myParams
                        );
                var dcaManager = new DcaManager(this.CustomsSetting.DCAServiceAddress //  @"http://itzik7:5050/Unifreight/DCAService/basic"
                     , this.CustomsSetting.DCAPartnerVault  ///"IIG"
                    , _CustomsRequestsSheetService.MyCustomsRequestsSheetPM.Tenant);

                var serverJobID = dcaManager.SubmitFileOutgoingQueue(
                    _CustomsRequestsSheetService.MyCustomsRequestsSheetPM.Id, fileContentsBASE64, dCAOutFileName,
                    out MessageOut);
                LogMessagingUtil.Instance.AppendLine("serverJobID =" + serverJobID);
                if (String.IsNullOrWhiteSpace(serverJobID))
                {
                    //MessageOut

                    LogMessagingUtil.Instance.AppendLine("error " + MessageOut);
                    throw new Exception("DCA FAILED must return serverJobID :" + MessageOut);
                }
                //ServerJobID
                dCAServerResponse = new DCAServerUploadResponse();
                dCAServerResponse.ServerJobID = serverJobID;
                dCAServerResponse.DcaMessage = MessageOut;
                dCAServerResponse.ComAmitalID = _CustomsRequestsSheetService.MyCustomsRequestsSheetPM.Id;
                //dCAServerResponse.DCAParams = myParams;
                dCAServerResponse.FileName = dCAOutFileName;
                var memdCAServerResponse =
                    XmlGenericUtil<DCAServerUploadResponse>.MemoryStreamSerialize(dCAServerResponse);
                //Serialize<DCAServerUploadResponse>(dCAServerResponse);


                //SetBlob(requestParams, comm.Document, memCustomsResponse, ".TCustomsResponse");
                //_CustomsRequestsSheetService.SetCorrelationId(_CorrelationId);
                toContinueNextCommand = _CustomsRequestsSheetService.EndStep(memdCAServerResponse, null);
                LogMessagingUtil.Instance.AppendLine("wait 1 sec hope the file will upload ");
                Thread.Sleep(TimeSpan.FromSeconds(1));//
                return toContinueNextCommand;
            }
            catch (CustomsRequestsSheetDomainModelServiceException)
            {
                throw;
            }
            catch (Exception ex)
            {

                TRequestParams myRequestParams = _CustomsRequestsSheetService.GetRequestParams<TRequestParams>(); ;
                Action myAction = null;

                myAction = () =>
                {
                    LogMessagingUtil.Instance.AppendLine("ResponseService.OnRequestFail()");
                    _ResponseService.OnRequestFail(null, myRequestParams);
                };
                throw _CustomsRequestsSheetService.FailStepRaiseCRSSExeption(ex, "Sending request to DCA Server Failed ", null, null, myAction);

            }
        }

        private bool CustomsCommandAnalyzeResponseEndStep(TRequestParams requestParams, TResponseData responseData, CommStatusEnum stepStatusEnum)
        {
            var memResponseData =
                XmlGenericUtil<TResponseData>.MemoryStreamSerialize(responseData);
            //Serialize<TResponseData>(responseData);


            //if (_ResponseService.MyRequestSheetParam != null)
            //{
            //    _CustomsRequestsSheetService.UpdateConnectedEntitys(_ResponseService.MyRequestSheetParam);
            //}
            var toContinueNextCommand = _CustomsRequestsSheetService.EndStep(memResponseData, _ResponseService.MyRequestSheetParam, stepStatusEnum); //Commit or  scope.Complete(); !!!
            return toContinueNextCommand;
        }
        private bool CustomsCommandAnalyzeResponse(TCustomsResponse customsResponse, ref TResponseData responseData)
        {
            var stepRequest = new StepRequest()
            {
                TimeOutInMin = 1,
                currentCustomsStep = CustomsStepEnum.AnalyzeResponseData,
                //ResponseData = new TResponseData() { HasException = true, Succeeded = false, UserMessage = "Look at Request Sheet for more details !" }  

            };
            if (this.MainInterfaceCode == "8302")// due delay in UROUTER !!
            {
                stepRequest.TimeOutInMin = 5;
            }

            if (this.MainInterfaceCode == "9000")
            {
                stepRequest.TimeOutInMin = 5;
                //transactionScopeOption = TransactionScopeOption.Suppress;
            }
            if (this.MainInterfaceCode == "2750")
            {
                stepRequest.TimeOutInMin = 5;
                //transactionScopeOption = TransactionScopeOption.Suppress;
            }
            if (this.MainInterfaceCode == "2751")
            {
                stepRequest.TimeOutInMin = 5;
                //transactionScopeOption = TransactionScopeOption.Suppress;
            }
            if (this.MainInterfaceCode == "2715")
            {
                stepRequest.TimeOutInMin = 5;
                //transactionScopeOption = TransactionScopeOption.Suppress;
            }

            if (this.MainInterfaceCode == "US2L01")
            {
                stepRequest.TimeOutInMin = 9;
                //transactionScopeOption = TransactionScopeOption.Suppress;
            }

            var requestParams = _CustomsRequestsSheetService.GetRequestParams<TRequestParams>();
            if (requestParams.RequestVIA > SendRequestVIA.WebServiceInteractive)
            {
                stepRequest.TimeOutInMin = 40;//IHAB SAID TO INCREASE - HOW MATCH ? - HE NOT MANTIONED !  //IHAB
            }
            object ContextObjectTag = null;
            bool toContinueNextCommand = DoStep(stepRequest, () =>
            {
                LogMessagingUtilWR.Instance.AppendLine("AnalyzeCore:b4");
                var res = AnalyzeCore(requestParams, customsResponse, false);
                LogMessagingUtilWR.Instance.AppendLine("AnalyzeCore:after");
                bool tryConcurrentKiller = true;//ConfigurationManager.AppSettings["20180718.ConcurrentKiller"] == "1";
                if (tryConcurrentKiller)
                {
                    CustomsRequestsSheetDomainModelUtil.ReleaseConcurrentKey(requestParams);
                }
                ContextObjectTag = res.ContextObjectTag;
                return res
                ;
            },
            () =>
            {
                LogMessagingUtilWR.Instance.AppendLine("GetResponseDataAfterAnalyze:b4");
                var res = GetResponseDataAfterAnalyze(requestParams, customsResponse);
                LogMessagingUtilWR.Instance.AppendLine("GetResponseDataAfterAnalyze:after");
                ContextObjectTag = res.ContextObjectTag;
                return res;
            },
            () => 
            {
                LogMessagingUtil.Instance.AppendLine("ResponseService.OnUpdateFail()");
                _ResponseService.OnRequestFail(customsResponse, requestParams);
            }
            );
            responseData = ContextObjectTag as TResponseData
            ;
            return toContinueNextCommand;
        }


        private bool CustomsCommandSendWS(TCustomsRequest customsRequest, ref TCustomsResponse customsResponse, ref TResponseData responseData)
        {
            var toContinueNextCommand = true;
            TRequestParams requestParams;
            if (GetSheetCustomResponse(ref customsResponse, out requestParams))
            {
                return toContinueNextCommand;
            }

            try
            {
                //var communicationLogStep = _CustomsRequestsSheetService.GetCommunicationLogStep();
                //communicationLogStep.Retries>0

                

                //string key = ProcessLockTableUtil.Instance.GetKey4InProggressCustomsRequestsSheet(requestParams.CustomsRequestsSheetId);
                //using (var disposableToken = ProcessLockTableUtil.Instance.LockItAndGetReleaseToken(key, "CustomsCommandSendWS"))
                {
                    _CustomsRequestsSheetService.StartStep(CustomsStepEnum.ReceivedCustomResponseCorrelation, _CustomsStateMachineProcess.CurrentCommand);
                    var test = false;
                    if (test)
                    {
                        Thread.Sleep(TimeSpan.FromMinutes(2));
                    }
                    DoConcurrentKiller(requestParams);
                    if (!requestParams.AvoidSign && requestParams.TestCase == null && (_CustomsRequestsSheetService.InterfaceTenantDefinitionManagement.InterfaceManagement.SignatureBy != SignQueueByType.None || requestParams.ForcePersonalSign))

                    //if (!requestParams.AvoidSign  && (_CustomsRequestsSheetService.InterfaceTenantDefinitionManagement.InterfaceManagement.SignatureBy != SignQueueByType.None || requestParams.ForcePersonalSign))

                    {
                        LogMessagingUtil.Instance.AppendLine("DoCallWSSigned...");
                        customsResponse = TaskCallWSSigned(requestParams, customsRequest, _CustomsRequestsSheetService.GetCustomsRequestSign());
                    }
                    else
                    {
                        LogMessagingUtil.Instance.AppendLine("SendWS...");
                        customsResponse = TaskCallWS(requestParams, customsRequest);////no catch exeption
                    }
                    LogMessagingUtil.Instance.AppendLine("IIGGatewayNoteMessage=" + _IIGGatewayMoreParams.NoteMessage);
                    _CustomsRequestsSheetService.OnEndStepAppendLogToCommunicationLog = "IIGGatewayNoteMessage=" + _IIGGatewayMoreParams.NoteMessage;
                    var memCustomsResponse =
                        XmlGenericUtil<TCustomsResponse>.MemoryStreamSerializeWithDefaultNamespace(customsResponse);
                    //Serialize<TCustomsResponse>(customsResponse);
                    //SetBlob(requestParams, comm.Document, memCustomsResponse, ".TCustomsResponse");
                    //_CustomsRequestsSheetService.SetCorrelationId(_CorrelationId); TryShrinkBlobFiles(customsRequest);                
                    SetCorrelationId(_CorrelationId, customsRequest);

                    toContinueNextCommand = _CustomsRequestsSheetService.EndStep(memCustomsResponse, null);
                }
                return toContinueNextCommand;

            }
            catch (AnotherThreadHandlingException)
            {
                throw;
            }
            catch (CustomsRequestsSheetDomainModelServiceException)
            {
                throw;
            }
            catch (System.ServiceModel.FaultException<UnifreightIIG.Common.SystemTableServiceReference.ResponseFault> fault)
            {
                customsResponse = new TCustomsResponse();
                var hdear = customsResponse.GetResponseContentHeader();
                //hdear.GetException();  
                var util = new UnifreightIIG.Common.Utils.ErrorHandlerUtil();
                string correlationId = "";
                var formattedMessage = util.ToFormattedMessage(fault, out correlationId);
                _CorrelationId = correlationId;
                if (String.IsNullOrWhiteSpace(_CorrelationId) || !this._CustomsRequestsSheetService.IsOnlIne())
                {


                    throw HandlExceptionSendWS(ref responseData, fault);

                }
                _ResponseHeader = new DefaultResponseHeaderOrFault()
                {
                    CorrelationId = _CorrelationId,
                    Status = // "TechnicalError"
                    TechnicalError
                    ,
                    ErrorDescription = formattedMessage
                };

                var memCustomsResponse =
                    XmlGenericUtil<DefaultResponseHeaderOrFault>.MemoryStreamSerialize(_ResponseHeader as DefaultResponseHeaderOrFault);
                //new MemoryStream(Encoding.UTF8.GetBytes("<err>" + + "<err>"));
                //Serialize<TCustomsResponse>(customsResponse);
                //SetBlob(requestParams, comm.Document, memCustomsResponse, ".TCustomsResponse");
                //_CustomsRequestsSheetService.SetCorrelationId(_CorrelationId);TryShrinkBlobFiles(customsRequest);                
                SetCorrelationId(_CorrelationId, customsRequest);

                toContinueNextCommand = _CustomsRequestsSheetService.EndStep(memCustomsResponse, null);
                return true;




            }
            catch (Exception ex)
            {
                throw HandlExceptionSendWS(ref responseData, ex , requestParams);

            }
        }

        private static void DoConcurrentKiller(TRequestParams requestParams)
        {
            bool tryConcurrentKiller = true;//ConfigurationManager.AppSettings["20180718.ConcurrentKiller"] == "1";
            if (tryConcurrentKiller)
            {
                if (CustomsRequestsSheetQueryService.GetintrefaceTypeListDisplayOnly().ToList().Contains(requestParams.InterfaceTypeCode))
                {
                    
                    string CRSKey = CustomsRequestsSheetDomainModelUtil.GetCRSKey(requestParams.CustomsRequestsSheetId);
                    LogMessagingUtil.Instance.AppendLine(" ** try lock,MessagingServiceBase.ReqSheet.SendSheet.DoConcurrentKiller():1082, key: " + CRSKey);

                    using (var scope = TransactionFactory.GetNewTransaction())// why GetNewTransaction() => b4 send Started (test constraint b4 not after SEND !!!)
                    {
                        var concurrentKiller = new ConcurrentKiller();
                        bool SupressFreeLockIfCreated15MinOld = ConfigurationManager.AppSettings["20210201.SupressFreeLockIfCreated15MinOld"] == "1";
                        if (!SupressFreeLockIfCreated15MinOld)
                        {
                            concurrentKiller.FreeLockIfCreated15MinOld(CRSKey, requestParams.Tenant);
                        }
                        concurrentKiller.LockOrCrashOnCommitDueUnique(CRSKey, requestParams.Tenant);
                        scope.Complete();
                    }

                }
            }
        }
        void SetTenantPriority(string ParentId,int tenant )
        {
            CustomsRequestsSheet currCustomsRequestsSheet = null;
            if (ParentId != null)
            {
                var customsRequestsSheetQueryService = new CustomsRequestsSheetQueryService(tenant);
                currCustomsRequestsSheet = customsRequestsSheetQueryService.GetTenantPriorityByEntityID(ParentId, tenant);
                if (currCustomsRequestsSheet != null) {
                 _CustomsRequestsSheetService.SetTenantPriority(currCustomsRequestsSheet.TenantPriority);
                }
            }
        }
        void SetCorrelationId(string CorrelationId, TCustomsRequest customsRequest)
        {
            _CustomsRequestsSheetService.SetCorrelationId(_CorrelationId);
            TryShrinkBlobFiles(customsRequest);
        }
        private void TryShrinkBlobFiles(TCustomsRequest customsRequest)
        {
            var lastStep = _CustomsRequestsSheetService.CalcLastCustomsStep();
            switch (lastStep)
            {
                case CustomsStepEnum.CustomRequest:
                    {
                        CRSShrinkBlobStepsUtil.Try2ShrinkCustomRequestBlobFile<TCustomsRequest>(
                            customsRequest,
                            (this._RequestService = _RequestService ?? new TRequestService()).GetActionShrinkCustomRequest(),
                            _CustomsRequestsSheetService);
                    }
                    break;
                case CustomsStepEnum.CustomRequestSign:
                    //case CustomsStepEnum.CustomRequestSignPersonal:
                    if (this.MainInterfaceCode == "2715")
                    {
                        CRSShrinkBlobStepsUtil.Try2ShrinkCustomRequestSignBlobFile(_CustomsRequestsSheetService);
                    }

                    break;
                case CustomsStepEnum.DCAInProgressUploading:
                case CustomsStepEnum.DCAInProgressUploaded:
                case CustomsStepEnum.ReceivedCustomResponseCorrelation:
                case CustomsStepEnum.AnalyzeResponseData:
                case CustomsStepEnum.StartRequestParams:
                default:
                    break;
            }
        }

        private Exception HandlExceptionSendWS(ref TResponseData responseData, Exception currentEx, TRequestParams requestParams = null)
        {
            string exceptionMessage = null;
            var defaultMessage = "Sending request to IIG Server Failed ";
            string correlationId;

            exceptionMessage = UnifreightIIG.Common.Utils.ErrorHandlerUtil.CreateNew().ToFormattedMessage(currentEx, out correlationId);
            _CorrelationId = correlationId;
            if (!string.IsNullOrWhiteSpace(_CorrelationId))
            {
                //_CustomsRequestsSheetService.SetCorrelationId(_CorrelationId);                 
                SetCorrelationId(_CorrelationId, null);

            }
            if (String.IsNullOrWhiteSpace(exceptionMessage))
            {
                exceptionMessage = defaultMessage;
            }
            responseData = new TResponseData() { Succeeded = false, HasException = true, UserMessage = "SendWS failed:" + exceptionMessage };

            MemoryStream memTResponseData = null;
            ///XmlGenericUtil<TResponseData>.MemoryStreamSerialize(responseData);

            TRequestParams myRequestParams = requestParams ?? this.RequestParams;
            Action myAction = null;

            myAction = () =>
              {
                  LogMessagingUtil.Instance.AppendLine("ResponseService.OnRequestFail()");
                  _ResponseService.OnRequestFail(null, myRequestParams);
              };
            return _CustomsRequestsSheetService.FailStepRaiseCRSSExeption(currentEx, exceptionMessage, null, memTResponseData, myAction);
        }

        private bool GetSheetCustomResponse(ref TCustomsResponse customsResponse, out TRequestParams requestParams)
        {
            string customsResponseXml = null;
            requestParams = _CustomsRequestsSheetService.GetRequestParams<TRequestParams>();
            if (_CustomsRequestsSheetService.StartCustomsRequestStepEnum > CustomsStepEnum.ReceivedCustomResponseCorrelation)
            {
                customsResponseXml = _CustomsRequestsSheetService.GetcustomsResponseXml();
                if (string.IsNullOrWhiteSpace(customsResponseXml))
                {
                    throw new Exception("GetBlob CustomsResponseXml Failed : " + _CustomsRequestsSheetService.MyCustomsRequestsSheetPM.Id);
                }



                LogMessagingUtil.Instance.AppendLine("<TCustomsResponse>.DeserilazeObject");
                customsResponse = XmlGenericUtil<TCustomsResponse>.DeSerializeObject(customsResponseXml);

                return true;


            }
            return false;
        }

        private bool CustomsCommandGetCustomRequest(out TCustomsRequest customsRequest)
        {

            customsRequest = null;
            var requestParams = _CustomsRequestsSheetService.GetRequestParams<TRequestParams>();
            LogMessagingUtil.Instance.AppendLine("GetCustomRequest...");


            var customsRequestXml = "";
            if (_CustomsRequestsSheetService.StartCustomsRequestStepEnum > CustomsStepEnum.CustomRequest)
            {
                customsRequestXml = _CustomsRequestsSheetService.GetCustomsRequestXml();
                if (!String.IsNullOrWhiteSpace(customsRequestXml))
                {
                    this._HugeFile = (customsRequestXml.Length
                        ///* sizeof(Char)
                        //System.Text.ASCIIEncoding.Unicode.GetByteCount(yourString);
                        > this.HugeFileSize);
                    LogMessagingUtil.Instance.AppendLine("<TCustomsRequest>.DeserilazeObject");
                    customsRequest = XmlGenericUtil<TCustomsRequest>.DeSerializeObject(customsRequestXml);
                    return true;
                }
            }

            bool tryConcurrentKiller = true;//ConfigurationManager.AppSettings["20180718.ConcurrentKiller"] == "1";
            if (tryConcurrentKiller)
            {
                if (!String.IsNullOrWhiteSpace(requestParams.LoggingObjectTableId) &&
                          !String.IsNullOrWhiteSpace(requestParams.LoggingEntityId))
                {
                    //if (CustomsRequestsSheetQueryService.GetintrefaceTypeListDisplayOnly().ToList().Contains(requestParams.InterfaceTypeCode))
                    {
                        CustomsRequestsSheetDomainModelUtil.ReleaseConcurrentVirtualKey(requestParams);
                    }
                }
            }
            var stepRequest = new StepRequest()
            {
                TimeOutInMin = 1,
                currentCustomsStep = CustomsStepEnum.CustomRequest
            };
            if (requestParams.RequestVIA > SendRequestVIA.WebServiceInteractive)
            {
                stepRequest.TimeOutInMin = 20;//IHAB
            }
            if (requestParams.RequestVIAChangeDue ==
                 ("הצהרה זו מכילה מעל 998 פרטי מכס ולכן תשלח לכספת"))
            {
                stepRequest.TimeOutInMin = 40;//ITZIK
            }
            object ContextObjectTag = null;
            bool toContinueNextCommand = DoStep(stepRequest, () =>
            {
                var res = CustomsCommandGetCustomRequestCore(requestParams);
                ContextObjectTag = res.ContextObjectTag;
                return res
                ;
            },
            null, () =>
            {
                LogMessagingUtil.Instance.AppendLine("CustomsCommandGetCustomRequest.OnUpdateFail()");
                _RequestService.OnRequestFail(requestParams);
            });
            customsRequest = ContextObjectTag as TCustomsRequest;
            ;
            return toContinueNextCommand;
        }



        private StepResult CustomsCommandGetCustomRequestCore(TRequestParams requestParams)
        {
            var stepResult = new StepResult();
            stepResult.commStatusEnum = CommStatusEnum.F;
            MemoryStream memstream = null;
            TCustomsRequest CustomsRequest = TaskCustomRequest(requestParams); //no catch exeption
            ///_MessagingBatchService.CustomsRequestXml = XmlGenericUtil<TCustomsRequest>.SerializeObject(customsRequest);
            ///



            memstream = XmlGenericUtil<TCustomsRequest>.MemoryStreamSerializeWithDefaultNamespace(CustomsRequest);
            var customsRequestLength = memstream.Length;
            LogMessagingUtil.Instance.AppendLine("customsRequestLength  = " + customsRequestLength.ToString());

            if (customsRequestLength > (HugeFileSize * 1.2))
            {
                this._HugeFile = true;
            }

            if (requestParams.RequestVIA != SendRequestVIA.DCABatch && this._HugeFile)
            {
                CreateNewDcaRequestDue9MBcustomsRequestLength();
            }

            var stopwatch = Stopwatch.StartNew();
            LogMessagingUtil.Instance.AppendLine("DoPost GetCustomRequest..");
            _RequestService.PostGetRequest(CustomsRequest, requestParams);

            stopwatch.Stop();
            LogMessagingUtil.Instance.AppendLine("MessagingServiceBase:DoPostGetCustomRequest:Took:" + stopwatch.Elapsed.ToString());
            stepResult.commStatusEnum = CommStatusEnum.D;
            stepResult.memstream = memstream;
            stepResult.ContextObjectTag = CustomsRequest as object;
            return stepResult;
        }
        public int HugeFileSize
        {
            get
            {
                return CustomsDocumentUpdateService.HugeFileSizeSendToDCA;
                
                var my9mb = 9000000;
                return my9mb;
            }
        }

        private bool DoStep(StepRequest stepRequest, Func<StepResult> funcStepCore, Func<StepResult> funcAfterStepCoreOutOfBLTRans , Action OnUpdateFailAction)
        {
            bool toContinueNextCommand = false;
            MemoryStream memstream = null;
            StepResult stepResult = null;
            RequestSheetParam myRequestSheetParam = null;
            if (stepRequest.TimeOutInMin < 1)
            {
                stepRequest.TimeOutInMin = 1;
            }
            var timeout = TimeSpan.FromMinutes(stepRequest.TimeOutInMin);
            TransactionScope scopeAdmin = null;
            //using (scopeAdmin = TransactionFactory.GetTransaction(timeout))//new TransactionScope(
            //{


            _CustomsRequestsSheetService.StartStep(stepRequest.currentCustomsStep, _CustomsStateMachineProcess.CurrentCommand, stepRequest.StartAt);
            try
            {
                bool takeContextLog = false;
                DbContextBase.IDbContextLogger myLogger = null;
                try
                {
                    
                    if (takeContextLog)
                    {
                        //myLogger = (CustomContext.GetContext(this._CustomsRequestsSheetService.MyCustomsRequestsSheetPM.Tenant) as DbContextBase).CreateLogger();
                        DbContextBaseUtil.ToLog = true;
                    }
                    using (var scopeMainBL = TransactionFactory.GetNewTransaction(timeout)) //new TransactionScope(TransactionScopeOption.RequiresNew,
                    {

                        stepResult = funcStepCore();
                        memstream = stepResult.memstream;

                        scopeMainBL.Complete();

                    }//using (var scopeMainBL = TransactionFactory.GetNewTransaction())
                }
                finally
                {
                    if (takeContextLog)
                    {
                        //if (myLogger!=null)
                        //{
                        //    myLogger.Dispose();
                        //}
                        DbContextBaseUtil.ToLog = false;
                    }
                }
                scopeAdmin = TransactionFactory.GetTransaction();
                if (funcAfterStepCoreOutOfBLTRans != null)
                {
                    if (!stepResult.SuppressFuncAfterCommit)
                    {
                        stepResult = funcAfterStepCoreOutOfBLTRans();
                        memstream = stepResult.memstream;
                    }
                }
                myRequestSheetParam = GetSheetParam(stepRequest);
                try
                {
                    toContinueNextCommand = _CustomsRequestsSheetService.EndStep(memstream, myRequestSheetParam, stepResult.commStatusEnum);//Inner TransactionScope(TransactionScopeOption.Required)

                }
                catch (CustomsRequestsSheetDomainModelServiceException myCustomsRequestsSheetDomainModelServiceException)
                {

                    if (stepRequest.currentCustomsStep == CustomsStepEnum.CustomRequest)
                    {
                        if (myCustomsRequestsSheetDomainModelServiceException.What2Do ==
                        CustomsRequestsSheetDomainModelServiceException.What2DoEnum.NewQueueCreated
                        && _RequestService.ToCancelSheetAfterGetRequest)
                        {
                            if (_RequestService.ToCancelSheetAfterGetRequest)
                            {
                                var customsRequestsSheetDomainModelServiceException = new CustomsRequestsSheetDomainModelServiceException(
                               Customs.BL.Messaging.Customs.CustomsRequestsSheetDomainModelServiceException.WhereEnum.MessageServiceException,
                               Customs.BL.Messaging.Customs.CustomsRequestsSheetDomainModelServiceException.What2DoEnum.CancelRequest,
                               "ToCancelSheetAfterGetRequest", null);
                                throw customsRequestsSheetDomainModelServiceException;
                            }
                        }
                    }
                    throw;
                }

                return toContinueNextCommand;

            }
            catch (CustomsRequestsSheetDomainModelServiceException myCustomsRequestsSheetDomainModelServiceException)
            {

                throw;///
            }
            catch (Exception ee)
            {
                scopeAdmin = scopeAdmin ?? TransactionFactory.GetTransaction();
                myRequestSheetParam = GetSheetParam(stepRequest);
                MemoryStream memstream1 = null;

                if (stepRequest.currentCustomsStep == CustomsStepEnum.AnalyzeResponseData)
                {
                    try
                    {
                        var tResponseData = new TResponseData()
                        {
                            UserMessage = @"Analyze Failed (" + ee.Message + @")
                                Look at Request Sheet for more details ",
                            HasException = true,
                            Succeeded = false
                        };
                        memstream1 = XmlGenericUtil<TResponseData>.MemoryStreamSerialize(tResponseData);
                    }
                    catch (Exception)
                    {


                    }
                }
                throw _CustomsRequestsSheetService.FailStepRaiseCRSSExeption(ee, stepRequest.currentCustomsStep.ToString() + @" Failed
Exception:" + ee.Message
                    , myRequestSheetParam, memstream1, OnUpdateFailAction);

            }
            finally
            {
                if (scopeAdmin != null)
                {
                    scopeAdmin.Complete();
                    scopeAdmin.Dispose();
                }
            }

        }

        private RequestSheetParam GetSheetParam(StepRequest stepRequest)
        {
            RequestSheetParam myRequestSheetParam = null;
            switch (stepRequest.currentCustomsStep)
            {
                case CustomsStepEnum.StartRequestParams:
                    break;
                case CustomsStepEnum.CustomRequest:
                    if (_RequestService != null)
                    {
                        myRequestSheetParam = _RequestService.MyRequestSheetParam;
                    }
                    break;
                case CustomsStepEnum.CustomRequestSign:
                    break;
                case CustomsStepEnum.DCAInProgressUploading:
                    break;
                case CustomsStepEnum.DCAInProgressUploaded:
                    break;
                case CustomsStepEnum.ReceivedCustomResponseCorrelation:
                    break;
                case CustomsStepEnum.AnalyzeResponseData:
                    if (_ResponseService != null)
                    {
                        myRequestSheetParam = _ResponseService.MyRequestSheetParam;
                    }
                    break;
                default:
                    break;
            }
            return myRequestSheetParam;
        }

        //private static void CompleteTransAndRaiseCancelRequest(TransactionScope scopeMainBL, bool toCancelSheetAfterGetRequest)
        //{
        //    scopeMainBL.Complete();
        //    if (toCancelSheetAfterGetRequest)
        //    {
        //        var customsRequestsSheetDomainModelServiceException = new CustomsRequestsSheetDomainModelServiceException(
        //            Customs.BL.Messaging.Customs.CustomsRequestsSheetDomainModelServiceException.WhereEnum.MessageServiceException,
        //            Customs.BL.Messaging.Customs.CustomsRequestsSheetDomainModelServiceException.What2DoEnum.CancelRequest,
        //            "ToCancelSheetAfterGetRequest", null);

        //        throw customsRequestsSheetDomainModelServiceException;
        //    }
        //    //finally
        //    //{
        //    //    scopeAdmin.Complete();

        //}



        private static void RaiseCancelRequest(bool toCancelSheetAfterGetRequest)
        {

            if (toCancelSheetAfterGetRequest)
            {
                var customsRequestsSheetDomainModelServiceException = new CustomsRequestsSheetDomainModelServiceException(
                    Customs.BL.Messaging.Customs.CustomsRequestsSheetDomainModelServiceException.WhereEnum.MessageServiceException,
                    Customs.BL.Messaging.Customs.CustomsRequestsSheetDomainModelServiceException.What2DoEnum.CancelRequest,
                    "ToCancelSheetAfterGetRequest", null);

                throw customsRequestsSheetDomainModelServiceException;
            }
        }

        private void CreateNewDcaRequestDue9MBcustomsRequestLength()
        {
            ///throw new NotImplementedException();
        }





        private void RaiseTimeout(Stopwatch totalStopwatch)
        {
            if (_CustomsRequestsSheetService.IsInteractive)
            {
                return;
            }
            if (totalStopwatch.Elapsed > TimeSpan.FromMinutes(_TimeOut))
            {
                LogMessagingUtil.Instance.AppendLine(_MessageTimeOut + ":" + _TimeOut.ToString());
                throw new CustomsRequestsSheetDomainModelServiceException(CustomsRequestsSheetDomainModelServiceException.WhereEnum.MessageServiceException, CustomsRequestsSheetDomainModelServiceException.What2DoEnum.RetryQueue
                    , _MessageTimeOut, null);
            }
        }




        private string JsonValidate(string value)
        {
            value = value ?? "";
            var sb = new StringBuilder(value);
            sb
                .Replace(":", "p")
                .Replace('{', '<')
                .Replace('[', '<')
                .Replace('}', '>')
                .Replace(']', '>')
                ;
            return sb.ToString();
        }

        public string CreateFakeDCA(GenericRequestParams requestParamsData)
        {
            string uniComm = null;
            string fileName = null;
            var transmitionDateTime = DateTime.Now;
            string xmlESBResponseXmlClass = null;



            TCustomsResponse customsResponse = GetFakeCustomsResponse(requestParamsData);

            var body = XmlGenericUtil<TCustomsResponse>.SerializeObject(customsResponse);
            //
            body = body.Substring(body.IndexOf(Environment.NewLine));
            var myESBResponseXmlClass = new ESBResponseXmlClass();
            var extrenalId = "62833ff7-1cd3-4faa-85a6-a4312ae4797a";
            extrenalId = uniComm ?? Guid.NewGuid().ToString();
            xmlESBResponseXmlClass = myESBResponseXmlClass.Get(Guid.NewGuid().ToString(), extrenalId, body);
            var transTime = "2016-04-19_13-35-13-481";

            transTime = transmitionDateTime.ToString("s").Replace("T", "_").Replace(":", "-");
            transTime += "-";
            transTime += transmitionDateTime.Millisecond.ToString();

            fileName = "DcaPrefixName.IL941079089FAKEFAKEFAKE." + transTime + "." + extrenalId + ".PLT.xml";

            var ourRef = "";
            using (var trans = TransactionFactory.GetNewTransaction())
            {
                try
                {


                    String theInterfaceCode = requestParamsData.InterfaceTypeCode ?? this.MainInterfaceCode;
                    var InterfaceManagementQS = new InterfaceManagementQueryService(requestParamsData.Tenant);
                    var InterfaceManagementPM = InterfaceManagementQS.GetSingleInterfaceManagementwithDefinition(
                        theInterfaceCode /*this.MainInterfaceCode*/, requestParamsData.Tenant);
                    fileName = fileName.Replace("DcaPrefixName.", InterfaceManagementPM.DcaPrefixName);
                    ourRef = this.DcaReceivedCustomResponseCorrelation(InterfaceManagementPM, requestParamsData.Tenant, new Customs.BL.Utils.DCAFileModel()
                    {
                        SelectedFileDownload = fileName,
                        TimStamp = transmitionDateTime

                    }, xmlESBResponseXmlClass);



                    trans.Complete();
                    return "המסר נבנה בהצלחה וישלח בתהליך רקע";
                }
                catch (CustomsRequestsSheetDomainModelServiceException myCustomsRequestsSheetServiceException)
                {
                    if (myCustomsRequestsSheetServiceException.Where == CustomsRequestsSheetDomainModelServiceException.WhereEnum.SameRequestInProgress)
                    {
                        Logitude.Server.Tools.Helpers.LogMessagingUtil.Instance.AppendLine(" UCB1170 SameRequestInProgress!! " + myCustomsRequestsSheetServiceException.Message);


                    }
                    else if (myCustomsRequestsSheetServiceException.Where == CustomsRequestsSheetDomainModelServiceException.WhereEnum.NoAvailableSignServer)
                    {
                        Logitude.Server.Tools.Helpers.LogMessagingUtil.Instance.AppendLine("UCB1170 SameRequestInProgress!!  " + myCustomsRequestsSheetServiceException.Message);
                    }
                    return "קיים מסר זהה בתהליך";
                    //throw;
                }
            }

        }
        
        virtual protected TCustomsResponse GetFakeCustomsResponse(GenericRequestParams requestParamsData)
        {
            
            throw new NotImplementedException("TCustomsResponse GetFakeCustomsResponse(GenericRequestParams requestParamsData):" + this.GetType().FullName);
        }

        public string DcaReceivedCustomResponseCorrelation(
            Logitude.Customs.Def.EntityPMs.InterfaceManagementPM currentDCAInInterfaceManagementPM, int tenant,
            //string selectedFile
            DCAFileModel selectedDCAFile, string fileContents //byte[] messageBytes
            , bool pseudo = false ,DateTime? futureSendDateTime = null)
        {
            string customsRequestsSheetPMId = "";
            CommStatusEnum stepStatusEnum = CommStatusEnum.W;
            TRequestParams defaultRequestParamsFromCustomsResponse = null;
            try
            {
                int tenantSave = tenant;
                var sb = new StringBuilder();
                sb
                    .AppendLine("currentDCAInInterfaceTypePM.Code=" + currentDCAInInterfaceManagementPM.Code)
                    .AppendLine("this.UnityId=" + this.MainInterfaceCode)
                    .AppendLine("DCAFileName=" + selectedDCAFile.SelectedFileDownload + "/tenant=" + tenant.ToString());
                if (!String.IsNullOrWhiteSpace(selectedDCAFile.DownloadLog))
                {
                    sb.AppendLine("DownloadLog=" + selectedDCAFile.DownloadLog);
                }
                if (selectedDCAFile.DebugCreateNew)
                {
                    sb.AppendLine("In Debug mode CreateNew");
                }



                LogMessagingUtil.Instance.AppendLine(sb.ToString());




                var dcaReceivedService = new DcaReceivedService<TCustomsResponse>(selectedDCAFile.SelectedFileDownload, fileContents);
                dcaReceivedService.ProccessIt();
                _CorrelationId = dcaReceivedService.CorrelationId;
                var customsResponse = dcaReceivedService.CustomsResponse ?? new TCustomsResponse();
                
                var CreateDefaultRequestParamsFromCustomsResponseFailed = true;
                try
                {
                    defaultRequestParamsFromCustomsResponse = CreateDefaultRequestParamsFromCustomsResponse(customsResponse);
                    CreateDefaultRequestParamsFromCustomsResponseFailed = false;
                }
                catch //(Exception)
                {

                    defaultRequestParamsFromCustomsResponse = new TRequestParams();
                }

                if (currentDCAInInterfaceManagementPM.InOut == "I")//Can be  DCAIn =No callback!!!
                {
                    defaultRequestParamsFromCustomsResponse.RequestVIA = Common.RequestParams.SendRequestVIA.DCABatch;
                    defaultRequestParamsFromCustomsResponse.Tenant = tenant;
                    defaultRequestParamsFromCustomsResponse.InterfaceTypeCode =
                        currentDCAInInterfaceManagementPM.Code
                            //this.MainInterfaceCode
                            ;

                    defaultRequestParamsFromCustomsResponse.MainInterfaceCode = this.MainInterfaceCode;
                    if (futureSendDateTime != null)
                    {
                        defaultRequestParamsFromCustomsResponse.FutureSendDateTime = futureSendDateTime;

                    }
                    defaultRequestParamsFromCustomsResponse.LoggingEnabled = true;

                    defaultRequestParamsFromCustomsResponse.TransmitionDateTime = selectedDCAFile.TimStamp;

                    if (customsResponse.GetResponseContentHeader() == null)
                    {
                        LogMessagingUtil.Instance.AppendLine("BLException ??? TransmitionDateTime = Get Default due customsResponse.GetResponseContentHeader() == null ");
                    }
                    else
                    {
                        if (defaultRequestParamsFromCustomsResponse.TransmitionDateTime != selectedDCAFile.TimStamp)
                        {
                            LogMessagingUtil.Instance.AppendLine("customsResponse.GetResponseContentHeader().TransmitionDateTime:" + customsResponse.GetResponseContentHeader().TransmitionDateTime + "  !=selectedDCAFile.TimStamp ");
                        }
                        //LogMessagingUtil.Instance.AppendLine("TransmitionDateTime = customsResponse.GetResponseContentHeader().TransmitionDateTime");
                        //defaultRequestParamsFromCustomsResponse.TransmitionDateTime = customsResponse.GetResponseContentHeader().TransmitionDateTime;
                    }
                }
                //defaultRequestParamsFromCustomsResponse.DCAFileName = selectedFile;
                defaultRequestParamsFromCustomsResponse.DCAFileName = selectedDCAFile.SelectedFileDownload; // itzik revive it ?!?!
                //defaultRequestParamsFromCustomsResponse.LoggingUserId = //system@tenant1.co.il
                //defaultRequestParamsFromCustomsResponse.RequestName= messageDCA.ClassName 

                var dm = new OverrideControllerModel()
                {

                    CurrentCustomsCommandWR = CurrentCustomsCommandWR,
                    SelectedDCAFileDebugCreateNew = selectedDCAFile.DebugCreateNew
                };

                RequestSheetParam reqSheetDetails = this.GetSheetDetailsFromRequestParam(defaultRequestParamsFromCustomsResponse);//itzik for Sivug Batch
                //var debug = selectedDCAFile.DebugCreateNew;
                CustomsRequestsSheetDomainModelService<TRequestParams>.Seed(dcaReceivedService.ExternalId, tenant, defaultRequestParamsFromCustomsResponse, out _CustomsRequestsSheetService, dm, true, reqSheetDetails);
         
                var queueSendModel = new QueueSendModel()
                {
                    Tenant = tenant,
                    InterfaceTypeCode = this.MainInterfaceCode,
                };
                if (futureSendDateTime.HasValue)
                {
                    TimeSpan timeSpan = futureSendDateTime.Value.Subtract(DateTime.Now);
                    queueSendModel.Delay = timeSpan;
                }
                queueSendModel.EntityCode = "CustomsRequestsSheet".ToLower();//"CustomsRequestsSheet";
                queueSendModel.EntityId = _CustomsRequestsSheetService?.MyCustomsRequestsSheetPM?.Id;

                //_CustomsRequestsSheetService = customsRequestsSheetService;
                bool explictStop = false;
                if (_CustomsRequestsSheetService.StartCustomsRequestStepEnum == CustomsStepEnum.DCAInProgressUploaded)
                {
                    _CustomsRequestsSheetService.StartStep(CustomsStepEnum.DCAInProgressUploaded,
                        //_CustomsStateMachineProcess.CurrentCommand
                        CustomsCommandEnum.CustomsCommandDownloadDcaReceiveCorrelationWR
                        );
                    var dCAServerUploadStatus = new DCAServerUploadStatus();
                    //dCAServerUploadStatus.TheDCAServerUploadResponse = dCAServerUploadResponse;
                    dCAServerUploadStatus.DcaMessage = "Stage  DCAInProgressUploaded is close due the response arrived!!!";
                    dCAServerUploadStatus.UnifreightQueueOutStatus = "SENT";
                    LogMessagingUtil.Instance.AppendLine(dCAServerUploadStatus.DcaMessage);

                    var memdCAServerUploadStatus = XmlGenericUtil<DCAServerUploadStatus>.MemoryStreamSerialize(dCAServerUploadStatus);
                    LogMessagingUtil.Instance.AppendLine("Send Via DCA now we need to wait for reply from mehes dca !!");
                    explictStop = _CustomsRequestsSheetService.EndStep(memdCAServerUploadStatus, null);
                    if (explictStop)
                    {
                        return _CustomsRequestsSheetService.MyCustomsRequestsSheetPM.Id;
                    }
                }

                _CustomsRequestsSheetService.StartStep(CustomsStepEnum.ReceivedCustomResponseCorrelation, CustomsCommandEnum.CustomsCommandDownloadDcaReceiveCorrelationWR);

                if (currentDCAInInterfaceManagementPM.Code != this.MainInterfaceCode)
                {
                    LogMessagingUtil.Instance.AppendLine("Please note this is return message !!! DCA Callback !!!");
                }
                else
                {
                    LogMessagingUtil.Instance.AppendLine("Please note this is absoloutly -DCA In !!!!");
                }
                var DCACallback = true;
                //defaultRequestParamsFromCustomsResponse.DCAFileName = selectedDCAFile.SelectedFileDownload
                if (_CustomsRequestsSheetService.GetRequestParams<TRequestParams>().DCAFileName == selectedDCAFile.SelectedFileDownload)
                {
                    DCACallback = false;
                    LogMessagingUtil.Instance.AppendLine("Please note this Not DCACallback !!!");
                }

                var memCustomsResponse = //XmlGenericUtil<TCustomsResponse>.MemoryStreamSerializeWithDefaultNamespace(customsResponse);
                    dcaReceivedService.GetMemoryStreamCustomsResponse();

                LogMessagingUtil.Instance.AppendLine(sb.ToString()); //DCAFileNAme: אנא בדוק שאתה שומר את שם הקובץ בכספת , בלוג

                ///TASK 25449
                if (
                    //currentDCAInInterfaceManagementPM.OurEnvironmentCheckFake  // Fake 2 check !
                    !DCACallback //if callback 
                                 /*
             4. במקרה שהמסר הנבדק הוא מסר משוב ולא מסר נדחף, יש לבדוק האם הוא שייך לסביבה הפעילה כבר בשלב התשתיתי (לפי קורלציה) ולהתנהג בהתאם.
             רק עבור מסרים נדחפים יש לפנות לתוכניות אפליקטיביות
                                  */
                    )
                {
                    if (_CustomsRequestsSheetService.InterfaceTenantDefinitionManagement.DcaRenameFileEnable)
                    {
                        LogMessagingUtil.Instance.AppendLine("InterfaceTenantDefinitionManagement.DcaRenameFileEnable !!");
                        if (String.IsNullOrWhiteSpace(_CustomsRequestsSheetService.InterfaceTenantDefinitionManagement.DcaRenameFilePrefix))
                        {
                            LogMessagingUtil.Instance.AppendLine("בדיקת קובץ יזום מוגדר אבל אין DcaRenameFilePrefix ,אי לכך ממשיכים להטעין את הקובץ (סוכם עם איתן ) !!!");

                        }
                        else
                        {
                            var isOurEnvironment = this.IsOurEnvironment(dcaReceivedService.CustomsResponse, _CustomsRequestsSheetService.GetRequestParams<TRequestParams>());
                            if (!isOurEnvironment.HasValue)
                            {
                                LogMessagingUtil.Instance.AppendLine("בדיקת קובץ יזום מוגדר אבל אין תוכנית בדיקה אפיקטיבית ,אי לכך ממשיכים להטעין את הקובץ (סוכם עם איתן ) !!!");
                            }
                            else
                            {
                                if (isOurEnvironment.GetValueOrDefault())
                                {
                                    LogMessagingUtil.Instance.AppendLine("Good this message belong to our Environment !!");
                                }
                                else
                                {

                                    LogMessagingUtil.Instance.AppendLine("DcaMessageNotBelongOurEnvironment !!");
                                    var ex = new CustomsRequestsSheetDomainModelServiceException(
                                        CustomsRequestsSheetDomainModelServiceException.WhereEnum.DcaMessageNotBelongOurEnvironment,
                                        CustomsRequestsSheetDomainModelServiceException.What2DoEnum.Default, "", null);
                                    //ex.CustomsRequestsSheetId = customsRequestsSheetId;
                                    ex.Tenant = tenant;
                                    throw ex;
                                }
                            }
                        }
                    }
                }

                this._ResponseService = this._ResponseService ?? new TResponseService();
                //_CustomsRequestsSheetService.SetCorrelationId(_CorrelationId); TryShrinkBlobFiles(null);                
                SetCorrelationId(_CorrelationId, null);
                SetTenantPriority(selectedDCAFile?.ParentId,tenant);
              var dcaReceivedController = this.GetDcaReceivedController(dcaReceivedService.CustomsResponse, _CustomsRequestsSheetService.GetRequestParams<TRequestParams>());
                if (dcaReceivedController != null && !String.IsNullOrWhiteSpace(dcaReceivedController.DcaAnalyzeAggregateKey))
                {
                    dcaReceivedController.DcaAnalyzeAggregateKey = JsonValidate(dcaReceivedController.DcaAnalyzeAggregateKey);
                    LogMessagingUtil.Instance.AppendLine("dcaReceivedController.DcaAnalyzeAggregateKey =" + dcaReceivedController.DcaAnalyzeAggregateKey + " Delay 30Sec !!!!");
                    _CustomsRequestsSheetService.SetDcaAnalyzeAggregateKey(dcaReceivedController.DcaAnalyzeAggregateKey);
                    queueSendModel.DcaAnalyzeAggregateKey = dcaReceivedController.DcaAnalyzeAggregateKey;
                    queueSendModel.Delay = TimeSpan.FromSeconds(32);


                }
                stepStatusEnum = CommStatusEnum.D;
                if (tenantSave != _CustomsRequestsSheetService.MyCustomsRequestsSheetPM.Tenant)
                {
                    stepStatusEnum = FailedAndThrow(pseudo,
                    "tenantSave != _CustomsRequestsSheetService.MyCustomsRequestsSheetPM.Tenant"
                        , stepStatusEnum);
                }
                else if (!String.IsNullOrWhiteSpace(dcaReceivedService.ErrorMessage))
                {
                    stepStatusEnum = FailedAndThrow(pseudo,
                        "The DcaReceivedService notice that Message  is not valid  !!!  " + dcaReceivedService.ErrorMessage
                        , stepStatusEnum);

                }
                else
                {
                    if (CreateDefaultRequestParamsFromCustomsResponseFailed)
                    {
                        if (pseudo)
                        {
                            throw new Exception("Create DefaultRequestParamsFromCustomsResponse Failed !!!  מכאן אין תמיכה DCA בסוג המסר הנל   ");
                        }
                        LogMessagingUtil.Instance.AppendLine("Create DefaultRequestParamsFromCustomsResponse Failed !!!  מכאן אין תמיכה DCA בסוג המסר הנל  ");
                        stepStatusEnum = CommStatusEnum.F;
                    }
                }
                if (!_CustomsRequestsSheetService.MyCustomsRequestsSheetPM.IsDCA)
                {
                    LogMessagingUtil.Instance.AppendLine("Change to DCA Due From DcaReceivedCustomResponseCorrelation Method!!");
                    _CustomsRequestsSheetService.MyCustomsRequestsSheetPM.IsDCA = true;

                }
                explictStop = _CustomsRequestsSheetService.EndStep(memCustomsResponse, null, stepStatusEnum);
                if (explictStop)
                {
                    bool inDcaReceivedCustomResponseCorrelationIWillCreateNewQueue = true;
                    if (!inDcaReceivedCustomResponseCorrelationIWillCreateNewQueue)
                    {
                        return _CustomsRequestsSheetService.MyCustomsRequestsSheetPM.Id;
                    }
                }


                if (!pseudo && stepStatusEnum == CommStatusEnum.D)
                {
                    if (_CustomsRequestsSheetService.MyCustomsRequestsSheetPM.IsRestored)
                    {
                        LogMessagingUtil.Instance.AppendLine("IsRestored No  SBQUEUEU   4  U !!! ");
                    }
                    else
                    {
                        LogMessagingUtil.Instance.AppendLine("Now we have to creat SBQUEUEU");
                        if (!_CustomsRequestsSheetService.GetRequestParams<TRequestParams>().SuppressSplitWR)
                        {
                            SBQMessageService.CreateBasic<CustomsCommandEnum>(
                                 CustomsCommandEnum.CustomsCommandAnalyzeResponseWR,
                                _CustomsRequestsSheetService.MyCustomsRequestsSheetPM.Id, queueSendModel);
                        }
                        else
                        {
                            SBQMessageService.CreateBasic(
                                                SBQueueNames.CustomsMessagingSheetBQ,
                                                ///not the receive _CustomsRequestsSheetService.CustomsRequestsSheet.InterfaceTypeCode ,
                                                _CustomsRequestsSheetService.MyCustomsRequestsSheetPM.Id, queueSendModel);
                        }
                    }
                }
            }
            catch (CustomsRequestsSheetDomainModelServiceException myCustomsRequestsSheetDomainModelServiceException)
            {
                if (myCustomsRequestsSheetDomainModelServiceException.What2Do == CustomsRequestsSheetDomainModelServiceException.What2DoEnum.StopQueueAddLog)
                {
                    var sb = new StringBuilder();
                    sb.Append("CorrelationId =" + _CorrelationId)
                        .AppendLine("selectedFile=" + selectedDCAFile.SelectedFileDownload)
                        .AppendLine("myCommunicationLog.ExceptionMessage = ExceptionMessage ")
                        .AppendLine(fileContents.Substring(0, Math.Min(fileContents.Length, 4000)));

                    CustomsRequestsSheetDomainModelUtil.SetExceptionMessage(tenant, myCustomsRequestsSheetDomainModelServiceException.CustomsRequestsSheetId, sb.ToString());
                    return null;
                    //myCustomsRequestsSheetDomainModelServiceException.CustomsRequestsSheetId
                    //_CorrelationId 
                    //fileContents
                    //    selectedFile
                }
                else
                {
                    throw;
                }

            }
            finally
            {


                bool tryConcurrentKiller = true;//ConfigurationManager.AppSettings["20180718.ConcurrentKiller"] == "1";
                if (tryConcurrentKiller)
                {
                    if (_CustomsRequestsSheetService != null)
                    {
                        var requestParams = _CustomsRequestsSheetService.GetRequestParams<TRequestParams>() ?? defaultRequestParamsFromCustomsResponse;
                        if (requestParams != null)
                        {
                            //throw new Exception("tryConcurrentKiller()--(requestParams==null)");

                            var intrefaceTypeListDisplayOnly = CustomsRequestsSheetQueryService.GetintrefaceTypeListDisplayOnly().ToList();
                            if (intrefaceTypeListDisplayOnly == null)
                            {
                                throw new Exception("tryConcurrentKiller()--(intrefaceTypeListDisplayOnly==null)");
                            }
                            if (intrefaceTypeListDisplayOnly/*CustomsRequestsSheetQueryService.GetintrefaceTypeListDisplayOnly().ToList()*/.Contains(requestParams.InterfaceTypeCode))
                            {

                                CustomsRequestsSheetDomainModelUtil.ReleaseConcurrentVirtualKey(requestParams, false);
                            }
                        }
                    }
                }
                if (stepStatusEnum == CommStatusEnum.F)
                {
                    try// Not neccery -fast develop
                    {
                        this._ResponseService.OnRequestFail(null, _CustomsRequestsSheetService.GetRequestParams<TRequestParams>());
                    }
                    catch (Exception ee) {}
                }
                if (_CustomsRequestsSheetService != null)
                {
                    if (_CustomsRequestsSheetService.MyCustomsRequestsSheetPM != null)
                    {
                        customsRequestsSheetPMId = _CustomsRequestsSheetService.MyCustomsRequestsSheetPM.Id;
                    }
                    _CustomsRequestsSheetService.Dispose();
                }
                RequestSheetContext.Current.Dispose();
            }

            return customsRequestsSheetPMId;
        }

        public string DCAServerUploadStatus(
            int tenant,
            DCAFileModel selectedDCAFile,
            Logitude.CustomsMessaging.Common.DCAParams.DCAServerUploadStatus MyDCAServerUploadStatus)
        {
            try
            {
                CustomsRequestsSheetDomainModelService<TRequestParams>.Seed(selectedDCAFile.OurRefExtrenalId, tenant, null,
                    out _CustomsRequestsSheetService, null, true);
                //_CustomsRequestsSheetService = customsRequestsSheetService;
                bool explictStop = false;
                if (_CustomsRequestsSheetService.StartCustomsRequestStepEnum != CustomsStepEnum.DCAInProgressUploaded)
                {
                    if (_CustomsRequestsSheetService.MyCustomsRequestsSheetPM.RequestStatusEnum == SheetStatusEnum.SendFailed)
                    {
                        LogMessagingUtil.Instance.AppendLine("CustomsRequestsSheet SheetStatusEnum =SendFailed, long time no see u = welcome back");
                        ///_CustomsRequestsSheetService.StartCustomsRequestStepEnum = CustomsStepEnum.DCAInProgressUploaded;
                    }
                    else
                    {
                        throw new Exception("Not in correct Status");
                    }


                }
                MyDCAServerUploadStatus.TheDCAServerUploadResponse = MyDCAServerUploadStatus.TheDCAServerUploadResponse ?? new DCAServerUploadResponse();
                var dCAServerUploadResponseDmy = MyDCAServerUploadStatus.TheDCAServerUploadResponse;

                DCAServerUploadStatus dCAServerUploadStatus;

                this.CustomsCommandDCAUploadStatus(
                    MyDCAServerUploadStatus.TheDCAServerUploadResponse,
                    out dCAServerUploadStatus,
                    MyDCAServerUploadStatus);

            }
            catch (Exception)
            {

                throw;
            }
            return "";
        }


        private static CommStatusEnum FailedAndThrow(bool pseudo, string errMessage, CommStatusEnum stepStatusEnum)
        {
            LogMessagingUtil.Instance.AppendLine(errMessage);

            stepStatusEnum = CommStatusEnum.F;
            if (pseudo)
            {
                throw new Exception(errMessage);
            }
            return stepStatusEnum;
        }


        private string GetExternalId(string selectedFile)
        {
            /*
        In 

        Customs Push
        \\GK-UNISVC1\CyberArk_DCA\amital_shipping\Download\ranar\UDCAServerHistory\SendMN_MSG1171_SendManifestFeedBack_Message_Out.IL941079089.2014-06-15_12-46-40-871.a60c718f-3d68-4d15-9b9a-6043dabb7574.PRD.xml.zip


        Return after our Req
        "\\GK-UNISVC1\CyberArk_DCA\amital_shipping\Download\ranar\UDCAServerHistory\SaveMN_MSG1170_1171_MANIFESTRequest_Out.IL941079089.2014-06-15_09-22-58-890.20140615083441612924803021008.PRD.xml.zip"

         */


            //\\dev2008\CyberArk_DCA\dev64bit_amitestm53\Download\IIG\GetSYSTBL_MSG9000_9001_SystemTableRequest_Out.IL941079089.2014-06-15_17-51-26-314.653bc69d-31e4-4e47-b973-28bd2cd8fe60.TST.xml
            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(selectedFile);//GetSYSTBL_MSG9000_9001_SystemTableRequest_Out.IL941079089.2014-06-15_17-51-26-314.653bc69d-31e4-4e47-b973-28bd2cd8fe60.TST.xml

            fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileNameWithoutExtension);///GetSYSTBL_MSG9000_9001_SystemTableRequest_Out.IL941079089.2014-06-15_17-51-26-314.653bc69d-31e4-4e47-b973-28bd2cd8fe60.TST
                                                                                                  ///GetSYSTBL_MSG9000_9001_SystemTableRequest_Out.IL941079089.2014-06-15_17-51-26-314.653bc69d-31e4-4e47-b973-28bd2cd8fe60
            var extension = Path.GetExtension(fileNameWithoutExtension);
            //.653bc69d-31e4-4e47-b973-28bd2cd8fe60
            extension = extension.Substring(1);//remove dot 
            return extension;
        }


        private static Dictionary<CustomsStateMachineProcess.StateTransition, CustomsStepEnum> InitTransition()
        {
            var transitions = new Dictionary<CustomsStateMachineProcess.StateTransition, CustomsStepEnum>()
            {
                { new CustomsStateMachineProcess.StateTransition(CustomsStepEnum.StartRequestParams, CustomsCommandEnum.CustomsCommandGetCustomRequestWR),CustomsStepEnum.CustomRequest},

                { new CustomsStateMachineProcess.StateTransition(CustomsStepEnum.CustomRequest, CustomsCommandEnum.CustomsCommandSignRequestWR ), CustomsStepEnum.CustomRequestSign},
                { new CustomsStateMachineProcess.StateTransition(CustomsStepEnum.CustomRequestSign, CustomsCommandEnum.CustomsCommandSendWSReceiveCorrelationWR), CustomsStepEnum.ReceivedCustomResponseCorrelation},

                { new CustomsStateMachineProcess.StateTransition(CustomsStepEnum.CustomRequestSign, CustomsCommandEnum.CustomsCommandSendDCAWR), CustomsStepEnum.DCAInProgressUploading },
                { new CustomsStateMachineProcess.StateTransition(CustomsStepEnum.DCAInProgressUploading, CustomsCommandEnum.CustomsCommandSendDCAUploadStatusWR), CustomsStepEnum.DCAInProgressUploaded},

                { new CustomsStateMachineProcess.StateTransition(CustomsStepEnum.DCAInProgressUploaded, CustomsCommandEnum.CustomsCommandDownloadDcaReceiveCorrelationWR), CustomsStepEnum.ReceivedCustomResponseCorrelation},

                //dca IN
                { new CustomsStateMachineProcess.StateTransition(CustomsStepEnum.StartRequestParams, CustomsCommandEnum.CustomsCommandAnalyzeResponseWR ), CustomsStepEnum.AnalyzeResponseData} ,

                { new CustomsStateMachineProcess.StateTransition(CustomsStepEnum.ReceivedCustomResponseCorrelation, CustomsCommandEnum.CustomsCommandAnalyzeResponseWR ), CustomsStepEnum.AnalyzeResponseData}
            };
            return transitions;
        }

        public string ConvertStepDataToJSON(int StepNumber, string DocumentDataXml)
        {
            switch (StepNumber)
            {
                case 0:
                    {
                        var req = XmlGenericUtil<TRequestParams>.DeSerializeObject(DocumentDataXml);
                        DocumentDataXml = JsonConvert.SerializeObject(req);
                        break;
                    }
                case 20:
                    {
                        var customsResponse = XmlGenericUtil<TCustomsResponse>.DeSerializeObject(DocumentDataXml);
                        DocumentDataXml = JsonConvert.SerializeObject(customsResponse);
                        break;
                    }

                case 30:
                    {
                        var res = XmlGenericUtil<TResponseData>.DeSerializeObject(DocumentDataXml);
                        DocumentDataXml = JsonConvert.SerializeObject(res);
                        break;
                    }
                default:
                    break;
            }
            return DocumentDataXml;
        }

        public OverrideControllerModel MyOverrideControllerModel { get; set; }

        public SendSheetSignModel _SignRecievedModel { get; set; }
    }
    public class StepRequest
    {

        public int TimeOutInMin { get; set; }
        public CustomsStepEnum currentCustomsStep { get; set; }



        public DateTime? StartAt { get; set; }


    }
    public class StepResult
    {
        public CommStatusEnum commStatusEnum { get; set; }


        public MemoryStream memstream { get; set; }

        public object ContextObjectTag { get; set; }

        public bool SuppressFuncAfterCommit { get; set; }
    }
    public class DcaReceivedController
    {
        public string DcaAnalyzeAggregateKey { get; set; }
    }
}
