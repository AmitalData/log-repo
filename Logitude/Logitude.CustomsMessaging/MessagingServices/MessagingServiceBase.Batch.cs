#if false


using Logitude.Server.Tools;
//using Logitude.AmitalMessaging.Utils;
using Logitude.Server.Tools.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Practices.Unity;
using System.Transactions;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System.IO;
using Microsoft.ServiceBus.Messaging;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Azure;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.Repositories;
using Logitude.Customs.BL.Messaging.Customs;
using Logitude.Server.Tools.QueueService;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.AmitalMessaging.Utils;


namespace Logitude.CustomsMessaging.MessagingServices
{

    //old def move to  MessagingServiceBase20131016
    public abstract partial class MessagingServiceBase<TRequestParams, TResponseData, TCustomsRequest, TCustomsResponse, TRequestService, TResponseService, TRequestHeader>
        : Logitude.CustomsMessaging.MessagingServices.IMessagingServiceBase<TRequestParams, TResponseData, TCustomsRequest, TCustomsResponse, TRequestHeader>
    {



        



        public void SendBatchStateMachine(int tenant, string mySBQCorrelationId, ref CustomsStepEnum processState) ///,CancellationToken ct)
        {
            string requestParamsXML = "";//, IMessagingBatchService messagingBatchService


            TCustomsRequest customsRequest = default(TCustomsRequest);
            TCustomsResponse customsResponse = default(TCustomsResponse);
            LogMessagingUtil.Instance.Clear();
            Stopwatch totalStopwatch = null;
            string exceptionMessage = "";
            byte[] customRequestSignedByteArry = null;
            CustomsStepEnum currentProcessState = CustomsStepEnum.StartRequestParams;
            CustomsStepEnum newProcessState = CustomsStepEnum.StartRequestParams;
            CommunicationLog mainComm = null;
            ICommonDataContext commonContext = null;
            CommunicationLogRepository communicationLogRepository = null;
            string mainCommSts = "W";
            try
            {
                //if (Debugger.IsAttached) Debugger.Break();
                currentProcessState = processState;
                totalStopwatch = Stopwatch.StartNew();
                commonContext = CommonDataContext.GetContext(tenant);
                communicationLogRepository = new CommunicationLogRepository(commonContext);


                mainComm = communicationLogRepository.GetSingleCommunicationLog(mySBQCorrelationId, tenant);
                if (mainComm == null)
                {
                    throw new Exception("GetSingleCommunicationLog Failed : " + mySBQCorrelationId);
                }

                if (!GetBlob(tenant, mainComm.Document, ".TRequestParams", out  requestParamsXML))
                {
                    throw new Exception("GetBlob TRequestParams Failed : " + mySBQCorrelationId);
                }



                var requestParams = XmlGenericUtil<TRequestParams>.DeSerializeObject(requestParamsXML);
                requestParams.LoggingEnabled = true;//all the time add log 
                //_MessagingBatchService = messagingBatchService;
                ///this.CustomsSetting = CustomsSettingQueryService.GetSettingByTenant(requestParams.Tenant);

                var transitions = InitTransition();

                var CommandList = new List<CustomsRCmmand>()
                {

                new CustomsRCmmand(CustomsCommandEnum.GetCustomRequest, (o)=>{
                 
                    LogMessagingUtil.Instance.AppendLine("GetCustomRequest...");
                    var documentSufix="";
                    if (currentProcessState > CustomsStepEnum.StartRequestParams)   
                    {    
                        var customsRequestXml="";
                        if (!GetBlob(tenant, mainComm.Document, documentSufix, out  customsRequestXml))
                        {
                            throw new Exception("GetBlob TCustomsRequest Failed : " + mySBQCorrelationId);
                        }
                        //_MessagingBatchService.CustomsRequestXml = customsRequestXml;
                        if (!String.IsNullOrWhiteSpace(customsRequestXml))
                        {
                            LogMessagingUtil.Instance.AppendLine("<TCustomsRequest>.DeserilazeObject");
                            customsRequest = XmlGenericUtil<TCustomsRequest>.DeSerializeObject(customsRequestXml);
                            newProcessState = CustomsStepEnum.CustomRequest;
                            return;                    
                        }
                    }
                    customsRequest = TaskCustomRequest(requestParams); //no catch exeption
                    ///_MessagingBatchService.CustomsRequestXml = XmlGenericUtil<TCustomsRequest>.SerializeObject(customsRequest);
                    MemoryStream memstream =
                        XmlGenericUtil<TCustomsRequest>.MemoryStreamSerializeWithDefaultNamespace(customsRequest);
                        //Serialize<TCustomsRequest>(customsRequest);
                    SetBlob(requestParams, mainComm.Document, memstream, documentSufix);
                    newProcessState = CustomsStepEnum.CustomRequest;
                }), 

                new CustomsRCmmand(CustomsCommandEnum.Sign, (o)=>
                    {
                        LogMessagingUtil.Instance.AppendLine("Sign...");
                        //_MessageDef = GetMessageDef();
                        if (true)//  !_MessageDef.ToSign)
                        {
                            newProcessState = CustomsStepEnum.CustomRequestSign;
                            return ;
                        }
                        
                        if (currentProcessState > CustomsStepEnum.CustomRequestSign )
                        {
                            if (!GetBlob(tenant, mainComm.Document, ".TCustomsRequestSign", out  customRequestSignedByteArry ))
                            {
                                throw new Exception("GetBlob TCustomsRequestSign Failed : " + mySBQCorrelationId);
                            }
                            //_MessagingBatchService.CustomRequestSignedByteArry   = customRequestSignedByteArry ;
                        }
                        if (customRequestSignedByteArry != null)
                        {
                            LogMessagingUtil.Instance.AppendLine("CustomRequestSignedByteArry revive");
                            customRequestSignedByteArry = customRequestSignedByteArry;
                            newProcessState = CustomsStepEnum.CustomRequestSign;
                            return;
                        }
                        customRequestSignedByteArry = TaskSignIt(requestParams.Tenant, customsRequest); //no catch exeption -rethrow
                        var memSign=new MemoryStream(customRequestSignedByteArry);
                        SetBlob(requestParams , mainComm.Document, memSign, ".TCustomsRequestSign");
                        newProcessState = CustomsStepEnum.CustomRequestSign ;
                        //_MessagingBatchService.CustomRequestSignedByteArry = _CustomRequestSignedByteArry;
                        
                    }), 
              
                    new CustomsRCmmand(CustomsCommandEnum.SendWS, (o)=>
                {
                    try
                    {
                        string customsResponseXml = null;
                        if (currentProcessState > CustomsStepEnum.ReceivedCustomResponseCorrelation)
                        {
                            
                            if (!GetBlob(tenant, mainComm.Document, ".TCustomsResponse", out  customsResponseXml))
                            {
                                throw new Exception("GetBlob CustomsResponseXml Failed : " + mySBQCorrelationId);
                            }
                            
                        }
                        if (!String.IsNullOrWhiteSpace(customsResponseXml))
                        {
                            LogMessagingUtil.Instance.AppendLine("<TCustomsResponse>.DeserilazeObject");
                            customsResponse = XmlGenericUtil<TCustomsResponse>.DeSerializeObject(customsResponseXml);
                            newProcessState = CustomsStepEnum.ReceivedCustomResponseCorrelation;
                            return;
                        }
                        if (false) ///_MessageDef.ToSign)
                        {
                            LogMessagingUtil.Instance.AppendLine("DoCallWSSigned...");
                            customsResponse = TaskCallWSSigned(requestParams, customsRequest, customRequestSignedByteArry);
                        }
                        else
                        {
                            LogMessagingUtil.Instance.AppendLine("SendWS...");
                            customsResponse = TaskCallWS(requestParams, customsRequest);////no catch exeption
                        }

                        var memCustomsResponse =
                            XmlGenericUtil<TCustomsResponse>.MemoryStreamSerializeWithDefaultNamespace(customsResponse);
                            //Serialize<TCustomsResponse>(customsResponse);
                        SetBlob(requestParams, mainComm.Document, memCustomsResponse, ".TCustomsResponse");
                        newProcessState = CustomsStepEnum.ReceivedCustomResponseCorrelation ;
                        //LogMessagingUtil.Instance.AppendLine("LogResponse... w");
                        //this.LogResponse(customsResponse, requestParams, "W"); /// Needed in dca only ??
                    }
                    catch (Exception ex)
                    {

                        var defaultMessage = "Sending request to IIG Server Failed ";
                        exceptionMessage = UnifreightIIG.Common.Utils.ErrorHandlerUtil.CreateNew().ToFormattedMessage(ex);
                        if (String.IsNullOrWhiteSpace(exceptionMessage))
                        {
                            exceptionMessage = defaultMessage;
                        }
                        LogMessagingUtil.Instance.AppendLine("exceptionMessage:" + exceptionMessage);
                        LogMessagingUtil.Instance.AppendLine("exceptionMessage:" + ex.ToString());
                        throw;
                    }
                    
                    
                }),
                new CustomsRCmmand(CustomsCommandEnum.SendDCA, (o)=>{ throw new Exception("TO DO SendDCA");  }),

                new CustomsRCmmand(CustomsCommandEnum.AnalyzeResponse, (o)=>
                {
                    bool newQueue=false;
                    //if (newQueue)
                    //{
                    //    LogMessagingUtil.Instance.AppendLine("LogResponse... W");
                    //    this.LogResponse(customsResponse, requestParams, "W");
                    //    //openNewQueue
                    //    return;
                    //}
                    
                     var responseData = TaskAnalyzeCompleteTrans(requestParams, customsResponse);//DB

                     if (!responseData.Succeeded )
                    {
                        throw new Exception("Try Again !!!" + responseData.ExceptionMessage);
                    }

                    //var ResponseData = _ResponseService.GetResponse(customsResponse, requestParams);
                    var memResponseData =
                        XmlGenericUtil<TResponseData>.MemoryStreamSerialize(responseData);
                        //Serialize<TResponseData>(responseData);
                    SetBlob(requestParams, mainComm.Document, memResponseData, ".TResponseData");
                    newProcessState = CustomsStepEnum.AnalyzeResponseData;
                }),
                };

                var p = new CustomsStateMachineProcess(CommandList, transitions);
                LogMessagingUtil.Instance.AppendLine("Current State = " + p.CurrentState);
                Stopwatch stopwatch = null;

                stopwatch = Stopwatch.StartNew();
                LogMessagingUtil.Instance.AppendLine("Current State = " + p.MoveNext(CustomsCommandEnum.GetCustomRequest, GetCustomRequestError));
                LogMessagingUtil.Instance.AppendLine("SendWS:Took:" + stopwatch.Elapsed.ToString());

                if (totalStopwatch.Elapsed > TimeSpan.FromMinutes(_TimeOut))
                {
                    throw new Exception(_MessageTimeOut);
                }
                stopwatch = Stopwatch.StartNew();
                LogMessagingUtil.Instance.AppendLine("Current State = " + p.MoveNext(CustomsCommandEnum.Sign));
                LogMessagingUtil.Instance.AppendLine("Sign:took:" + stopwatch.Elapsed.ToString());
                if (totalStopwatch.Elapsed > TimeSpan.FromMinutes(_TimeOut))
                {
                    throw new Exception(_MessageTimeOut);
                }

                if (false) //_MessageDef.Interactive == Customs.BL.EntityPMs.IIGMessagePM.InteractiveMode.DCA)
                {
                    stopwatch = Stopwatch.StartNew();
                    LogMessagingUtil.Instance.AppendLine("Current State = " + p.MoveNext(CustomsCommandEnum.SendDCA));
                    LogMessagingUtil.Instance.AppendLine("SendDCA:took:" + stopwatch.Elapsed.ToString());
                }
                else
                {
                    stopwatch = Stopwatch.StartNew();
                    LogMessagingUtil.Instance.AppendLine("Current State = " + p.MoveNext(CustomsCommandEnum.SendWS));
                    LogMessagingUtil.Instance.AppendLine("SendWS:took:" + stopwatch.Elapsed.ToString());
                }

                if (totalStopwatch.Elapsed > TimeSpan.FromMinutes(_TimeOut))
                {
                    throw new Exception(_MessageTimeOut);
                }
                mainCommSts = "D";
                LogMessagingUtil.Instance.AppendLine("Current State = " + p.MoveNext(CustomsCommandEnum.AnalyzeResponse));
                LogMessagingUtil.Instance.AppendLine("SendBatch- Done !!!");


            }

            catch (Exception curException)
            {
                mainCommSts = "F";
                //success = false;
                if (curException.Message == _MessageTimeOut)
                {
                    LogMessagingUtil.Instance.AppendLine(_MessageTimeOut + ":" + _TimeOut.ToString());
                    return;
                }
                LogMessagingUtil.Instance.AppendLine("Exception " + curException.ToString());
            }
            finally
            {
                try
                {
                    using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, TimeSpan.FromMinutes(2)))
                    {
                        mainComm.Logs = LogMessagingUtil.Instance.ToString(8000);
                        mainComm.CommunicationStatusTypeCode = mainCommSts;
                        communicationLogRepository.Update(mainComm);
                        communicationLogRepository.SubmitChanges();
                        scope.Complete();
                    }
                }
                catch (Exception)
                {

                    throw;
                }


                //_MessagingBatchService.Save(success);
                processState = newProcessState;
            }
        }

       
        public string CreateSBQMessage(TRequestParams requestParams)
        {
            var correlationId = "";
            var memstream1 =
                XmlGenericUtil<TRequestParams>.MemoryStreamSerialize(requestParams);
            //Serialize<TRequestParams>(requestParams);

            var subj = requestParams.RequestName + "(" + typeof(TCustomsRequest).Name + ")";
            //var mySBQMessage = new SBQMessageService();
            correlationId = SBQMessageService.CreateSBQMessage(
                requestParams.Tenant, requestParams.LoggingEntityId, requestParams.LoggingObjectTableId, "",
                requestParams.LoggingUserId, requestParams.LoggingEntityReference,
                subj, this.MainInterfaceCode, memstream1);
            return correlationId;
        }
        private void GetCustomRequestError(object obj)
        {
            //throw new NotImplementedException();
        }




    }
   
}



#endif