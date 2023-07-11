using Logitude.Customs.BL.Messaging.Customs;
using Logitude.Customs.Def.Messaging.Customs;
using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.Server.Tools.Helpers;
using System.IO;
using Logitude.Server.Tools.Models;
using System.Transactions;
using Logitude.Customs.Def.ClosedTable;
using Logitude.Server.Tools.ExternalServices;
using System.Xml.Linq;
using Simplog.Data.Helpers;
using Logitude.CustomsMessaging.Helpers;
using Logitude.Customs.BL.Messaging.Customs.SignQueueBL;

namespace Logitude.CustomsMessaging.MessagingServices
{
   public abstract partial class MessagingServiceBase<TRequestParams, TResponseData, TCustomsRequest, TCustomsResponse, TRequestService, TResponseService, TRequestHeader>
: Logitude.CustomsMessaging.MessagingServices.IMessagingServiceBase<TRequestParams, TResponseData, TCustomsRequest, TCustomsResponse, TRequestHeader>
    {
        
        

        public (byte[], RequestParamsBase) PasiveSignGetBytesToSign(int tenant, string CustomsRequestsSheetId)
        {

            TRequestParams defaultRequestParamsFromCustomsResponse = null;
            defaultRequestParamsFromCustomsResponse = new TRequestParams();
            CustomsRequestsSheetDomainModelService<TRequestParams>.Seed(CustomsRequestsSheetId, tenant, defaultRequestParamsFromCustomsResponse, out _CustomsRequestsSheetService);
            if (_CustomsRequestsSheetService.InterfaceTenantDefinitionManagement.InterfaceManagement.SignatureBy != SignQueueByType.None)
            {
            }
            if (_CustomsRequestsSheetService.StartCustomsRequestStepEnum != CustomsStepEnum.CustomRequestSign)
            {
                //throw "Not in the right step";
            }
            var dRequestParams = _CustomsRequestsSheetService.GetRequestParams<TRequestParams>() as RequestParamsBase;
            var xmlSerilazeObject = _CustomsRequestsSheetService.GetCustomsRequestXml();
            var bytesSerilazeObject = UTF8Encoding.UTF8.GetBytes(xmlSerilazeObject);
            return (bytesSerilazeObject, dRequestParams);
        }

        
        private bool CustomsCommandSign(TCustomsRequest customsRequest)
        {
            LogMessagingUtil.Instance.AppendLine("CustomsCommandSign");

            var toContinueNextCommand = true;
            var requestParams = _CustomsRequestsSheetService.GetRequestParams<TRequestParams>();
        
            switch (_CustomsRequestsSheetService.CalcSignByFromStep(null))
            {
             
                case SignQueueByType.SignQueueByCustomsAgentId:
                    break;
                case SignQueueByType.SignQueueByPersonId:
                    break;
             
                default:
                    //if (!requestParams.ForcePersonalSign)
                    {
                        return toContinueNextCommand;
                    }
                    break;
            }
            
           
            LogMessagingUtil.Instance.AppendLine("Sign...");
            byte[] customRequestSignedByteArry = null;
            if (_CustomsRequestsSheetService.StartCustomsRequestStepEnum > CustomsStepEnum.CustomRequestSign)
            {
                LogMessagingUtil.Instance.AppendLine("if (_CustomsRequestsSheetService.StartCustomsRequestStepEnum > CustomsStepEnum.CustomRequestSign):" + _CustomsRequestsSheetService.StartCustomsRequestStepEnum);

                customRequestSignedByteArry = _CustomsRequestsSheetService.GetCustomsRequestSign();
                if (customRequestSignedByteArry == null)
                {
                    throw new Exception("GetBlob TCustomsRequestSign Failed : " + _CustomsRequestsSheetService.MyCustomsRequestsSheetPM.Id);
                }


                if (customRequestSignedByteArry != null)
                {
                    LogMessagingUtil.Instance.AppendLine("CustomRequestSignedByteArry revive");

                    SetRequestSheetContextCurrentX509Certificate(customRequestSignedByteArry);
                    //_CustomRequestSignedByteArry = customRequestSignedByteArry;
                    
                    return toContinueNextCommand;
                }
            }


            string availableSignServer = "";
            string noAvailableSignServerErrorText = "";
            
            bool throwNoAvailableSignServer=false;


            string personId = ""; SignQueueByType  SignatureBy =  SignQueueByType.None;

            DateTime startAt = TenantServerConfigration.GetCurrentDateTime(requestParams.Tenant);//DateTime.Now;20150909
            if (_SignRecievedModel == null)
            {

                if (SignQueue.Instance.IsPasiveSignMode() && !IsIneractiveHSM())
                {

                    availableSignServer = _CustomsRequestsSheetService.GetAvailableSignServer(out personId, out SignatureBy, out noAvailableSignServerErrorText);

                    if (!string.IsNullOrWhiteSpace(availableSignServer))
                    {

                        Nullable<CustomsCommandEnum> curComm = null;
                        if (_CustomsStateMachineProcess != null)
                        {
                            curComm = _CustomsStateMachineProcess.CurrentCommand;
                        }
                        _CustomsRequestsSheetService.StartStep(CustomsStepEnum.CustomRequestSign, curComm);

                        var pmCustomsSetting = Customs.BL.EntityQueryServices.CustomsSettingQueryService.GetSettingByTenant(requestParams.Tenant);
                        
                        if (string.IsNullOrEmpty(requestParams.SignMethodByQueue))
                        {
                            throw new Exception("RequestParams.SignType is must !!");
                        }
                        SignMethodByQueueEnum signMethodBy = SignMethodByQueueEnum.None;
                        if (!Enum.TryParse<SignMethodByQueueEnum>(requestParams.SignMethodByQueue, out signMethodBy))
                        {
                            throw new Exception("RequestParams.SignType is must !!");
                        }
                        switch (signMethodBy)
                        {

                            case SignMethodByQueueEnum.HybridDbSignQueue:
#if false
                                if (!pmCustomsSetting.IsConnectedToUniFreight)
                                {
                                    _CustomsRequestsSheetService.AddExportDBSignQueue(requestParams, personId, SignatureBy, pmCustomsSetting.CustomsAgentId);
                                }
#endif
                                var signQueueHybridExportDBService = new CreateSignQueueHybridExportDBService();
                                signQueueHybridExportDBService.CreateQueue(requestParams, personId, SignatureBy, pmCustomsSetting.CustomsAgentId);

                                break;
                            case SignMethodByQueueEnum.HSMSignQueue:
                                var signQueueHSMDBService = new CreateSignQueueHSMDBService();
                                signQueueHSMDBService.CreateQueue(requestParams, personId, SignatureBy, pmCustomsSetting.CustomsAgentId);
                                break;

                            case SignMethodByQueueEnum.None:
                            case SignMethodByQueueEnum.MemorySignQueue:
                            default:
                                {
                                    SignQueue.Instance.Add(requestParams.Tenant, personId, requestParams.CustomsRequestsSheetId, requestParams.InterfaceTypeCode, SignatureBy);
                                }
                                break;
                        }                                             
                        var businessErrorException = new BusinessErrorException("Add SignQueue ");

                        businessErrorException.CurrentContextTag = new TResponseData()
                        {
                            //ContinuePasiveSignInBackground = true,
                            HasException = false,
                            ContinueProcessInBackground = true,
                            Succeeded = true
                        };
                        throw businessErrorException;
                    }
                    else
                    {
                        throwNoAvailableSignServer = true;
                    }
                }
            }
            else if (_SignRecievedModel != null)
            {

                Logitude.Server.Tools.Helpers.LogMessagingUtil.Instance
     .AppendLine("else if (_SignRecievedModel != null):" + _SignRecievedModel.CustomsRequestsSheetId);

 

                DateTime startAtD = DateTime.MinValue;
                SignQueue.Instance.GetStartAt(requestParams.Tenant, requestParams.CustomsRequestsSheetId, out startAtD);
                if (startAtD != DateTime.MinValue)
                {
                    startAt = startAtD;
                }
                if (this._CustomsRequestsSheetService.GetCurrentStepStartAt().HasValue)
                {
                    startAt = this._CustomsRequestsSheetService.GetCurrentStepStartAt().GetValueOrDefault();
                }
            ;
                if (!string.IsNullOrWhiteSpace(_SignRecievedModel?.ExportTaskQueueId))
                {
                    LogMessagingUtil.Instance.AppendLine("if (!string.IsNullOrWhiteSpace(_SignRecievedModel?.ExportTaskQueueId))");

                    var queueservice = new Server.Tools.QueueService.DbQueueService("How Care ", requestParams.Tenant);
                    if (!string.IsNullOrWhiteSpace(_SignRecievedModel?.ExportTaskMarkAsFailedMessage))
                    {
                        queueservice.CompleteAsFailedParam(_SignRecievedModel.ExportTaskQueueId);
                    }
                    else
                    {
                        queueservice.Complete(_SignRecievedModel.ExportTaskQueueId);
                    }
                    
                }


            }

            var stepRequest = new StepRequest()
            {
                TimeOutInMin = 1,
                currentCustomsStep = CustomsStepEnum.CustomRequestSign,
                StartAt = startAt  
            };
            //object ContextObjectTag = null;
            toContinueNextCommand = DoStep(stepRequest, () =>
            {
                if (!string.IsNullOrWhiteSpace(_SignRecievedModel?.ExportTaskMarkAsFailedMessage))
                {
                    throw new Exception(_SignRecievedModel.ExportTaskMarkAsFailedMessage);
                }
                if (throwNoAvailableSignServer)
                {
                    throw new Exception(noAvailableSignServerErrorText);
                }
                var res = CustomsCommandSignCore(requestParams, customsRequest);
                ///ContextObjectTag = res.ContextObjectTag;
                return res
                ;
            },
            null,null);
            ///customsRequest = ContextObjectTag as TCustomsRequest;
            ;
            if (!string.IsNullOrWhiteSpace(_SignRecievedModel?.ExportTaskQueueId))
            {
                toContinueNextCommand = false;//use batch - not IIS !!!
            }
                
            return toContinueNextCommand;

        }

        private bool IsIneractiveHSM()
        {
            
            if (RequestParams.SignMethodByQueue != SignMethodByQueueEnum.HSMSignQueue.ToString())
            {
                return false;
            }
            if (RequestParams.RequestVIA != SendRequestVIA.WebServiceInteractive)
            {
                return false;
            }
            return true;

        }

        private void SetRequestSheetContextCurrentX509Certificate(byte[] customRequestSignedByteArry)
        {
            if (false)
            {
                File.WriteAllBytes(@"C:\Users\itzik\Desktop\zevel\tst.xml", customRequestSignedByteArry);
                var xml = Encoding.UTF8.GetString(customRequestSignedByteArry);
                File.WriteAllText(@"C:\Users\itzik\Desktop\zevel\tst_utf8.xml", xml);
                var xDoc1 = XDocument.Parse(xml);
            }
            
            //var xDoc = XDocument.Parse(Encoding.UTF8.GetString(customRequestSignedByteArry));
            string xml1 = Encoding.UTF8.GetString(customRequestSignedByteArry) ?? "";
            if (xml1[0] != '<')//why START AT 2022/8/29 == bom ?? 
            {
                xml1 = xml1.Substring(1);
            }
            var xDoc = XDocument.Parse(xml1);
            var eleX509SubjectName = xDoc.Descendants().FirstOrDefault(ele => ele.Name.LocalName.Contains("X509SubjectName"));
            if (eleX509SubjectName != null)
            {
                RequestSheetContext.Current.GetContextOrDefault().SignByX509SubjectName= eleX509SubjectName.Value;
            }

        }
        private StepResult CustomsCommandSignCore(TRequestParams requestParams, TCustomsRequest customsRequest)
        {
            byte[] customRequestSignedByteArry = null;

            var stepResult = new StepResult();
            DateTime StartAt = TenantServerConfigration.GetCurrentDateTime(requestParams.Tenant);//DateTime.Now;20150909;
            if (_SignRecievedModel != null)
            {
                LogMessagingUtil.Instance.AppendLine("CompleteResponseSignBytes ...");
                LogMessagingUtil.Instance.AppendLine("CurrentSignCertificateName =" + _SignRecievedModel.CurrentSignCertificateName);

                SignQueue.Instance.GetStartAt(requestParams.Tenant, requestParams.CustomsRequestsSheetId, out StartAt);

                customRequestSignedByteArry = _SignRecievedModel.CustomRequestSignedByteArryPasiveSign;
                if (Transaction.Current != null)
                {
                    Transaction.Current.TransactionCompleted += (senderO, e) =>
                    {
                        if (e.Transaction.TransactionInformation.Status == TransactionStatus.Committed)
                        {
                            LogMessagingUtil.Instance.AppendLine("if (e.Transaction.TransactionInformation.Status == TransactionStatus.Committed)");

                            SignQueue.Instance.Remove(requestParams.Tenant, requestParams.CustomsRequestsSheetId);
                        }
                    };
                }
                else
                {
                    LogMessagingUtil.Instance.AppendLine("Transaction.Current == null ??? can not remove  SignQueue.Instance.Remove !!!");

                    throw new Exception("Transaction.Current == null ??? can not remove  SignQueue.Instance.Remove !!!");
                }
            }
            else
            {

                if (IsIneractiveHSM())
                {
                    customRequestSignedByteArry = TaskSignItHSM(requestParams.Tenant, customsRequest); //no catch exeption -rethrow
                }
                else
                {
                    LogMessagingUtil.Instance.AppendLine("TaskSignIt");

                    customRequestSignedByteArry = TaskSignIt(requestParams.Tenant, customsRequest); //no catch exeption -rethrow
                }
                
            }
            SetRequestSheetContextCurrentX509Certificate(customRequestSignedByteArry);
            var memSign = new MemoryStream(customRequestSignedByteArry);
            
            stepResult.commStatusEnum = CommStatusEnum.D;
            stepResult.memstream = memSign;
            //stepResult.ContextObjectTag = CustomsRequest as object;
            CRSShrinkBlobStepsUtil.Try2ShrinkCustomRequestBlobFile<TCustomsRequest>(
                                customsRequest,
                                (this._RequestService = _RequestService ?? new TRequestService())
                                .GetActionShrinkCustomRequest(),
                                _CustomsRequestsSheetService);

            return stepResult;

        }
        

    }
}
