using Logitude.CustomsMessaging.Helpers;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.RequestServices;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.ResponseServices;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Server.Infrastructure.Azure;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Xml;
using System.Xml.Serialization;
using UnifreightIIG.Common.ClientSdk;
using UnifreightIIG.Common.CommonIIGInterface;
using UnifreightIIG.Common.Faults;
using Logitude.Server.Tools;
using Logitude.Server.Tools.StorageService;
using Microsoft.Practices.Unity;
using Logitude.Customs.BL.EntityQueryServices;
using System.Reflection;
using System.ServiceModel;
using Logitude.Server.Tools.ExternalServices;
using Logitude.Customs.BL.Messaging.Customs;
using Logitude.AmitalMessaging.Utils;
using Logitude.Customs.Def.ClosedTable;
using Logitude.Server.Tools.Models;
using System.ComponentModel;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.Customs.BL.Messaging.Customs.SignQueueBL;
using Logitude.BL.InfrastructureModel.APIDataContract.ApiV1;

namespace Logitude.CustomsMessaging.MessagingServices
{
    //old def move to  MessagingServiceBase20131016
    public abstract partial class MessagingServiceBase<TRequestParams, TResponseData, TCustomsRequest, TCustomsResponse, TRequestService, TResponseService, TRequestHeader>
        : Logitude.CustomsMessaging.MessagingServices.IMessagingServiceBase<TRequestParams, TResponseData, TCustomsRequest, TCustomsResponse, TRequestHeader>
        where TCustomsRequest : class ,new()
        where TCustomsResponse : class ,IINF_MSG_Generic, new()
        where TRequestService : RequestServiceBase<TCustomsRequest, TRequestParams>, new()
        where TRequestParams : RequestParamsBase, new()
        where TResponseService : ResponseServiceBase<TResponseData, TCustomsResponse, TRequestParams>, new()
        where TResponseData : ResponseDataBase, new()
        where TRequestHeader : IRequestHeader, new()
    {

        //static readonly Dictionary<string, string> _DictionaryTranslate;
        static MessagingServiceBase()
        {
            //MessagingServiceFactoryHelper.InitContainer();
            //    _DictionaryTranslate = new Dictionary<string, string>();
            //    _DictionaryTranslate.Add( "CH_NG_190_MSG1_NoticeToClient", 
            //        (new DCAInCH_NG_190_MSG1_NoticeToClientMessagingService()).MainInterfaceCode );
            //    _DictionaryTranslate.Add("CH_NG_196_MSG7_CargoExitFromCheckSite",
            //        (new DCAInCH_NG_196_MSG7_CargoExitFromCheckSiteMassageService()).MainInterfaceCode);

            //    _DictionaryTranslate.Add("DE_NG_5107_MSG10_AcceptanceOrRejectionMessage",
            //        (new DCAInDE_NG_5107_MSG10_AcceptanceOrRejectionMessagingService()).MainInterfaceCode);
            //    _DictionaryTranslate.Add("DF_NG_2470_DF_MSG16001_ReleaseGoodsMessage",
            //        (new DCAInDF_NG_2470_DF_MSG16001_ReleaseGoodsMessagingService()).MainInterfaceCode);
            //    _DictionaryTranslate.Add("VAL_NG_8227_MSG_520_RequiredDocumentMessage",
            //        (new DCAInVAL_NG_8227_MSG_520_RequiredDocumentMassagingService()).MainInterfaceCode);
            //
        }


        protected TRequestParams RequestParams;
        protected IResponseHeaderOrFault _ResponseHeader;
        private string _CorrelationId;
        Logitude.Customs.Def.EntityPMs.CustomsSettingPM _CustomsSetting;
        

        abstract protected TCustomsResponse CallWS(TCustomsRequest customRequest, TRequestParams requestParams, out string exceptionMessage);
        protected TResponseService _ResponseService;
        private Customs.Def.EntityPMs.InterfaceManagementPM _MainMessageDefinition;
        //IMessagingBatchService _MessagingBatchService;
        //private Customs.BL.EntityPMs.IIGMessagePM _MessageDef;


        public MessagingServiceBase()
        {
            string cmd= Environment.CommandLine ?? "";
            if (!cmd.Contains("AmitalCustomsWindowsService"))
            //if (CurrentCustomsCommandWR == null)
            {
                LogMessagingUtil.Instance.Clear();
            }
            _swMessagingServiceBase = Stopwatch.StartNew(); 
            MessagingServiceFactoryHelper.InitContainer();
            var interfaceCode = this.MainInterfaceCode;//may raise NotImplementedException
            _ResponseService = new TResponseService();
            


        }

        virtual protected TCustomsResponse CallWSTest(TCustomsRequest customRequest, TRequestParams requestParams)
        {
            var exceptionMessage = "No test is implemented please make sure that you implemented the CallWSTest and that you don't call the overridden base method";
            throw new NotImplementedException("CallWSSigned Not Implemented, Type is " + typeof(TCustomsRequest).Name);
        }

        virtual protected void PreCallWS(TCustomsRequest customRequest, TRequestParams requestParams)
        {

        }
        


        virtual protected TCustomsResponse CallWSSigned(byte[] customRequestSignedByteArry, TRequestParams requestParams, out string exceptionMessage)
        {
            throw new NotImplementedException("CallWSSigned Not Implemented, Type is " + typeof(TCustomsRequest).Name);
        }
        virtual protected void BuildRequestContentHeaderB4Sign(TCustomsRequest customRequest)
        {
            try
            {
                AddDefaultIRequestContentHeader(customRequest);

            }
            catch (Exception)
            {

                throw new NotImplementedException("InitRequestContentHeader Not Implemented, Type is " + typeof(TCustomsRequest).Name);
            }




        }
        public void AddDefaultIRequestContentHeader(TCustomsRequest customRequest)
        {
            try
            {

                if (customRequest == null)
                {
                }

                dynamic customRequestD = customRequest;
                if (customRequestD.RequestContentHeader != null)
                {
                    return;
                }
                var prop = (customRequestD as object).GetType().GetProperties().ToList().FirstOrDefault(rec => rec.Name == "RequestContentHeader");
                var tRequestContentHeader = prop.PropertyType;

                var newTObject = Activator.CreateInstance(tRequestContentHeader);
                prop.SetValue(customRequest, newTObject);
                var myRequestContentHeader = customRequestD.RequestContentHeader as IRequestContentHeader;

                myRequestContentHeader.TransmitionDateTime = DateTime.Now; 
                myRequestContentHeader.SenderID = 1;
                myRequestContentHeader.RecieverID = new int[] { 1 };
            }
            catch (Exception)
            {

                throw;
            }




        }

        public TResponseData Send(TRequestParams requestParams, TCustomsRequest customsRequestCalc = null)
        {
            
            _RequestService = new TRequestService();
            LogMessagingUtil.Instance.AppendLine("B4_ManipulateRequestParams:Took:" + _swMessagingServiceBase.ElapsedMilliseconds);
            _swMessagingServiceBase.Restart(); 
            _RequestService.ManipulateRequestParams(requestParams);
            LogMessagingUtil.Instance.AppendLine("ManipulateRequestParams::Took:" + _swMessagingServiceBase.ElapsedMilliseconds);
            string requestSatus = "F";
            string responseStatus = "F";
            TCustomsRequest customsRequest = default(TCustomsRequest);
            TCustomsResponse customsResponse = default(TCustomsResponse);
            TResponseData errorResponseData = null;
            
            Stopwatch totalStopwatch = null;

            InitDef();
            this.RequestParams = requestParams;
            if (customsRequestCalc == null)
            {
                if (true
                    //||AuthenticationUtil.ResolveUserIdentityName(requestParams.Tenant).Equals("itzik@amital.co.il", StringComparison.OrdinalIgnoreCase)
                    )
                {
                    if (_MainMessageDefinition != null)
                    {
                        return SendSheet(requestParams);
                    }
                }
            }

            try
            {
                requestParams.LoggingEnabled = true;//all the time add log 
                totalStopwatch = Stopwatch.StartNew();




                if (customsRequestCalc == null)
                {

                    customsRequest = TaskCustomRequest(requestParams);
                }
                else
                {
                    customsRequest = customsRequestCalc;
                    LogMessagingUtil.Instance.AppendLine("customsRequest Already  Calculated !!!");
                }


                if (requestParams.TestCase != null && requestParams.TestCase.Type == "customservice" && requestParams.TestCase.Code != "Real Logic")
                {
                    customsResponse = CallWSTest(customsRequest, requestParams);


                }
                else
                {
                    errorResponseData = SendCustomRequest(requestParams, ref customsRequest, ref customsResponse);//DB CommID
                    if (errorResponseData != null)
                    {
                        return errorResponseData;
                    }
                    requestSatus = "D";
                }

                bool toContinueNextCommand = true;
                var responseData = TaskAnalyzeCompleteTrans(requestParams, customsResponse, out toContinueNextCommand);//DB
                if (responseData.Succeeded)
                {
                    responseStatus = "D";
                }


                return responseData;
            }
            catch (Exception exception1)
            {
                LogMessagingUtil.Instance.AppendLine(exception1.ToString());
                return new TResponseData() { UserMessage = "Unexpected Exception " + exception1.Message + " (For Detailed Error ,Look in communication)", HasException = true };
            }
            finally
            {
                LogMessagingUtil.Instance.AppendLine("totalStopwatch(Not inclode Logs) " + totalStopwatch.Elapsed.ToString());
                if (!String.IsNullOrEmpty(this.RequestsSheetExternalId))
                {
                    LogRequest(customsRequest, requestParams, requestSatus);
                    LogResponse(customsResponse, requestParams, responseStatus);
                }

            }
        }

        private void InitDef()
        {
            if (_MainMessageDefinition != null)
            {
                return;
            }
            var interfaceCode = this.MainInterfaceCode;//may raise NotImplementedException
            int tenant = SettingUtil.GetCurrentTenant();
            var interfaceTypeQueryService = new InterfaceManagementQueryService(tenant);
            _MainMessageDefinition = interfaceTypeQueryService.GetSingle(interfaceCode, false, true);
        }

        


        private void DoPreCallWSCompleteTrans(TRequestParams requestParams, TCustomsRequest customsRequest)
        {
            using (TransactionScope scope = TransactionFactory.GetTransaction())
            {
                ///CreateRequestCommunicationLogId(requestParams);

                var stopwatch = Stopwatch.StartNew();
                LogMessagingUtil.Instance.AppendLine("PreCallWS ..");
                PreCallWS(customsRequest, requestParams);
                if (this._CustomsRequestsSheetService != null)
                {
                    this.RequestsSheetExternalId = this._CustomsRequestsSheetService.MyCustomsRequestsSheetPM.Id;
                }
                else
                {
                    LogMessagingUtil.Instance.AppendLine("CreateRequestCommunicationLogId ..");
                    this.RequestsSheetExternalId = IdCounter.GetNumber("CommunicationLog", requestParams.Tenant);
                }
                _IIGGatewayMoreParams = new UnifreightIIG.Common.TheGateway.MoreParams() { MyOption = UnifreightIIG.Common.TheGateway.MoreParams.Options.None };
                stopwatch.Stop();
                scope.Complete();
                LogMessagingUtil.Instance.AppendLine("MessagingServiceBase:PreCallWS:Took:" + stopwatch.Elapsed.ToString());
                LogMessagingUtil.Instance.AppendLine("RequestsSheetExternalId =" + this.RequestsSheetExternalId);
            }
        }

        private TResponseData TaskAnalyzeCompleteTrans(TRequestParams requestParams, TCustomsResponse customsResponse, out bool toContinue,
            Func<TRequestParams, TResponseData, CommStatusEnum, bool> actionB4TransactionScopeComplete = null)
        {
            TResponseData responseData = null;
            toContinue = true;
            string exceptionMessage = "";
            
            try
            {
                var timeout = TimeSpan.FromMinutes(1);
                var transactionScopeOption = TransactionScopeOption.Required;
                if (this.MainInterfaceCode == "9000" || this.MainInterfaceCode == "8302" || this.MainInterfaceCode == "2715")
                {
                    LogMessagingUtil.Instance.AppendLine("GetTransaction(timeout)=TimeSpan.FromMinutes(5)");
                    timeout = TimeSpan.FromMinutes(5);
                    //transactionScopeOption = TransactionScopeOption.Suppress;
                }
                using (TransactionScope scope = TransactionFactory.GetTransaction(timeout))//new TransactionScope(transactionScopeOption, timeout))
                {
                    var res = AnalyzeCore(requestParams, customsResponse,true);

                    
                    CommStatusEnum commStatusEnum =res.commStatusEnum ;
                    responseData = res.ContextObjectTag as TResponseData;

                    if (actionB4TransactionScopeComplete != null)
                    {
                        toContinue = actionB4TransactionScopeComplete(requestParams, responseData, commStatusEnum);//inner trans require (Open trans if does not have !!)
                    }
                    scope.Complete();
                }


                return responseData;
            }
            catch (Exception ex)
            {

                exceptionMessage = ex.Message;
                LogMessagingUtil.Instance.AppendLine("MessagingServiceBase:UpdateCompleteTrans:Exception " + ex.ToString());
                if (ex.InnerException != null)
                {
                    var dbEntityValidationException = ex.InnerException as System.Data.Entity.Validation.DbEntityValidationException;
                    if (dbEntityValidationException != null)
                    {
                        var formatedEx = ExceptionFormatUtil.GetFormated(dbEntityValidationException);
                        exceptionMessage = formatedEx.Message;
                        LogMessagingUtil.Instance.AppendLine("DbEntityValidationException " + formatedEx.ToString());
                    }
                    else
                    {
                        LogMessagingUtil.Instance.AppendLine("ex.InnerException " + ex.InnerException.Message);
                    }
                }
                responseData = new TResponseData() { UserMessage = exceptionMessage, HasException = true, Succeeded = false };
                if (actionB4TransactionScopeComplete != null)
                {
                    if (this._CustomsRequestsSheetService.IsInteractive)
                    {
                        using (TransactionScope scope = TransactionFactory.GetTransaction())
                        {
                            var commStatusEnumF = CommStatusEnum.F;
                            actionB4TransactionScopeComplete(requestParams, responseData, commStatusEnumF);
                            scope.Complete();
                        }

                    }
                    else
                    {
                        throw;
                    }
                }
                return responseData;
                //exceptionMessage = ex.Message;
                //LogMessagingUtil.Instance.AppendLine("MessagingServiceBase:UpdateCompleteTrans:Exception " + ex.ToString());
                //return new TResponseData() { ExceptionMessage = exceptionMessage, HasException = true, Succeeded = false };
            }
        }

        private StepResult AnalyzeCore(TRequestParams requestParams, TCustomsResponse customsResponse, bool toGetResponseDataAfterAnalyze)
        {

            var stepResult = new StepResult();
            TResponseData responseData = null;
            //string exceptionMessage;
            stepResult.commStatusEnum = CommStatusEnum.F;
            if (this.MyOverrideControllerModel != null && !string.IsNullOrWhiteSpace(this.MyOverrideControllerModel.AggregateDCAAnalyzerLogger))
            {
                LogMessagingUtil.Instance.AppendLine(this.MyOverrideControllerModel.AggregateDCAAnalyzerLogger);
            }
            Stopwatch stopwatch = Stopwatch.StartNew();
            //if (_CustomsRequestsSheetService.DualResponseHeaderStatusReturnAckSentResponseOnDCA)
            //{
            //    LogMessagingUtil.Instance.AppendLine("DualResponseHeaderStatusReturnAckWillArriveFromDCA");

            //    responseData = new TResponseData() { UserMessage = "עקב עומס במכס  , המשוב יתקבל בכספת", HasException = false, Succeeded = true };

            //}
            //else
            {
            responseData = GetIIGBLExceptionFromReponseHeader(customsResponse);
            }
            ///_ResponseService = new TResponseService();
            
            if (responseData != null)
            {

                if (responseData.Succeeded)
                {
                    stepResult.commStatusEnum = CommStatusEnum.D;
                    LogMessagingUtil.Instance.AppendLine("הניתוח הצליח אבל ...");
                }
                else
                {
                    stepResult.commStatusEnum = CommStatusEnum.F;
                    LogMessagingUtil.Instance.AppendLine("IIG Exception ,No Analyze/Update ");
                }
                _ResponseService.MyResponseData = responseData;
                stepResult.ContextObjectTag = responseData;
                var memResponseData =
           XmlGenericUtil<TResponseData>.MemoryStreamSerialize(responseData);
                stepResult.memstream = memResponseData;
                stepResult.SuppressFuncAfterCommit = true;
            }
            else
            {
                try
                {
                    LogMessagingUtil.Instance.AppendLine("MessagingServiceBase:Update:Start");
                    //throw new Exception("tst");
                    _ResponseService.Update(customsResponse, requestParams);
				
					stopwatch.Stop();
                    LogMessagingUtil.Instance.AppendLine("MessagingServiceBase:Update:" + stopwatch.Elapsed.ToString());
                }
                catch (DbEntityValidationException ex)
                {

                    var formated = ExceptionFormatUtil.GetFormated(ex);
                    LogMessagingUtil.Instance.AppendLine("MessagingServiceBase:UpdateCompleteTrans:DbEntityValidationException  " + formated.ToString());
                    throw formated;
                }
                catch (Exception ex)
                {

                    //exceptionMessage = ex.Message;
                    LogMessagingUtil.Instance.AppendLine("MessagingServiceBase:UpdateCompleteTrans:Exception " + ex.ToString());
                    if (ex.InnerException != null)
                    {
                        var dbEntityValidationException = ex.InnerException as System.Data.Entity.Validation.DbEntityValidationException;
                        if (dbEntityValidationException != null)
                        {
                            var formatedEx = ExceptionFormatUtil.GetFormated(dbEntityValidationException);
                            //exceptionMessage = formatedEx.Message;
                            LogMessagingUtil.Instance.AppendLine("DbEntityValidationException " + formatedEx.ToString());
                            throw formatedEx;
                        }
                        
                    }
                    
                    
                    //LogMessagingUtil.Instance.AppendLine("ex.InnerException " + ex.InnerException.Message);
                    throw ex;
                    
                }
                if (toGetResponseDataAfterAnalyze)
                {
                    stepResult = GetResponseDataAfterAnalyze(requestParams, customsResponse);
                }

                CRSShrinkBlobStepsUtil.Try2ShrinkCustomResponseBlobFile<TCustomsResponse>(customsResponse,
                    
                    
                                (this._ResponseService = _ResponseService ?? new TResponseService())
                                .GetActionShrinkCustomResponse(),
                                _CustomsRequestsSheetService);
            }
            return stepResult;
        }

        private StepResult  GetResponseDataAfterAnalyze(
            TRequestParams requestParams, TCustomsResponse customsResponse)
        {
            var stepResult = new StepResult();
            TResponseData responseData = null;
            Stopwatch stopwatch = Stopwatch.StartNew();
            responseData = _ResponseService.GetResponse(customsResponse, requestParams);
            LogMessagingUtil.Instance.AppendLine("MessagingServiceBase:GetResponse:" + stopwatch.Elapsed.ToString());
            if (responseData == null)
            {
                responseData = _ResponseService.MyResponseData;
            }
            if (responseData == null)
            {
                throw new Exception(@"_ResponseService.GetResponse(customsResponse, requestParams)  return null 
Please instance and set MyResponseData ");
            }
            var memResponseData =
            XmlGenericUtil<TResponseData>.MemoryStreamSerialize(responseData);
            stepResult.commStatusEnum = CommStatusEnum.D;
            stepResult.memstream = memResponseData;
            stepResult.ContextObjectTag = responseData as TResponseData;
            return stepResult;
        }



        private TResponseData SendCustomRequest(TRequestParams requestParams, ref TCustomsRequest customsRequest, ref TCustomsResponse customsResponse)
        {

            Stopwatch stopwatch = null;
            string exceptionMessage = "";
            try
            {



                stopwatch = Stopwatch.StartNew();
#if false
                _MessageDef = GetMessageDef();
                switch (_MessageDef.Interactive)
                {

                    case Logitude.Customs.BL.EntityPMs.IIGMessagePM.InteractiveMode.DCA:
                        return SendDCA(requestParams, customsRequest);

                    case Logitude.Customs.BL.EntityPMs.IIGMessagePM.InteractiveMode.none:
                    case Logitude.Customs.BL.EntityPMs.IIGMessagePM.InteractiveMode.Interactive:
                    default:
                        customsResponse = TaskCallWS(requestParams, customsRequest);
                        break;
                }       
#endif
                customsResponse = TaskCallWS(requestParams, customsRequest);





                return null;


            }
            catch (Exception ex)
            {

                var defaultMessage = "Sending request to IIG Server Failed ";
                string correlationId = "";
                exceptionMessage = UnifreightIIG.Common.Utils.ErrorHandlerUtil.CreateNew().ToFormattedMessage(ex, out correlationId);
                _CorrelationId = correlationId;

                if (String.IsNullOrWhiteSpace(exceptionMessage))
                {
                    exceptionMessage = defaultMessage;
                }
                LogMessagingUtil.Instance.AppendLine("exceptionMessage:" + exceptionMessage);
                LogMessagingUtil.Instance.AppendLine("exceptionMessage:" + ex.ToString());
                if (ex.InnerException != null)
                {
                    LogMessagingUtil.Instance.AppendLine("InnerException.Message:" + ex.InnerException.Message);
                }

                //UpdateCommunicationLog(requestParams, requestCommunicationLogId, exceptionMessage);

                return new TResponseData() { UserMessage = exceptionMessage, HasException = true };
            }
            finally
            {
                stopwatch.Stop();
                LogMessagingUtil.Instance.AppendLine("customsResponse(CallWS):Took:" + stopwatch.Elapsed.ToString());
            }
        }

        private TCustomsResponse TaskCallWSSigned(TRequestParams requestParams, TCustomsRequest customsRequest, byte[] customRequestSignedByteArry)
        {
            //CreateRequestCommunicationLogId(requestParams);
            //customsResponse = CallWSSigned(_CustomRequestSignedByteArry, requestParams, out exceptionMessage);
            var exceptionMessage = "";
            DoPreCallWSCompleteTrans(requestParams, customsRequest);//DB

            
            var sw = Stopwatch.StartNew();
            var customsResponse = CallWSSigned(customRequestSignedByteArry, requestParams, out exceptionMessage);
            LogMessagingUtil.Instance.AppendLine("CallWSSigned:Took:" + sw.Elapsed.ToString());


            SetCorrelationID();


            //GetIIGBLExceptionFromReponseHeader(customsResponse);
            return customsResponse;
        }

        private void SetCorrelationID()
        {
            if (_ResponseHeader == null)
            {
                throw new Logitude.Server.Tools.Models.BusinessErrorException("The Response Must include _ResponseHeader ");
            }
            this._CorrelationId = _ResponseHeader.CorrelationId;
            LogMessagingUtil.Instance.AppendLine("responseHeader{CorrelationId =" + _ResponseHeader.CorrelationId);
            LogMessagingUtil.Instance.Append(",ErrorCode =" + _ResponseHeader.ErrorCode);
            LogMessagingUtil.Instance.Append(",Status =" + _ResponseHeader.Status);
            LogMessagingUtil.Instance.Append(",ExternalId =" + _ResponseHeader.ExternalId);
            LogMessagingUtil.Instance.AppendLine(",ErrorDescription =" + _ResponseHeader.ErrorDescription + "}");

            if (_ResponseHeader.Status.Equals("Ack", StringComparison.OrdinalIgnoreCase))//Status =Ack
            {
                
                _CustomsRequestsSheetService.DualResponseHeaderStatusReturnAckSentResponseOnDCA = true;
                //GetRequestParams<TRequestParams>().SuppressSplitWR
            }
        }

        private TCustomsResponse TaskCallWS(TRequestParams requestParams, TCustomsRequest customsRequest)
        {
            var exceptionMessage = "";

            DoPreCallWSCompleteTrans(requestParams, customsRequest);//DB
            
            var sw = Stopwatch.StartNew();
            var customsResponse = CallWS(customsRequest, requestParams, out exceptionMessage);
            LogMessagingUtil.Instance.AppendLine("CallWS:Took:" + sw.Elapsed.ToString());

            SetCorrelationID();
            ///GetIIGBLExceptionFromReponseHeader(customsResponse);
            return customsResponse;
        }
        /// <summary>
        /// Raise UnifreightIIGFault
        /// </summary>
        /// <param name="customsResponse"></param>
        protected virtual TResponseData GetIIGBLExceptionFromReponseHeader(TCustomsResponse customsResponse)
        {

            string exceptionMessage = "";
            try
            {
                var responseContentHeader = customsResponse.GetResponseContentHeader() as IResponseContentHeader;
                //UnifreightIIGFault.
                ThrowIIGBLException(_ResponseHeader, responseContentHeader);
                return null;

            }
            catch (System.ServiceModel.FaultException<UnifreightIIGFault> myUnifreightIIGFault)
            {
                var resData = DefaultException(myUnifreightIIGFault);
                if (myUnifreightIIGFault.Detail.PlaceFault == UnifreightIIGFault.PlaceFaultEnum.IIGBusinessError)
                {
                    //הניתוח הצליח אבל החולה מת...
                    resData.Succeeded = true;
                }
                return resData;

            }
            catch (Exception ex)
            {

                return DefaultException(ex);
            }

        }

        private static TResponseData DefaultException(Exception ex)
        {
            string exceptionMessage = "";
            var defaultMessage = "Sending request to IIG Server Failed ";
            exceptionMessage = UnifreightIIG.Common.Utils.ErrorHandlerUtil.CreateNew().ToFormattedMessage(ex);
            if (String.IsNullOrWhiteSpace(exceptionMessage))
            {
                exceptionMessage = defaultMessage;
            }
            LogMessagingUtil.Instance.AppendLine("exceptionMessage:" + exceptionMessage);
            LogMessagingUtil.Instance.AppendLine("exceptionMessage:" + ex.ToString());

            //UpdateCommunicationLog(requestParams, requestCommunicationLogId, exceptionMessage);

            return new TResponseData() { UserMessage =  exceptionMessage, HasException = true };
        }
        const string TechnicalError = "TechnicalError";
        public static void ThrowIIGBLException(IResponseHeaderOrFault responseHeader, IResponseContentHeader responseContentHeader)
        {
            string errMessage;
            if (responseHeader != null)
            {
                if (responseHeader.Status == "Success")
                {
                    return;
                }//BusinessError




                if (responseHeader.Status.Equals(//"TechnicalError"
                    TechnicalError
                    , StringComparison.OrdinalIgnoreCase))
                {
                    //CorrelationId: "a4066472-3678-4e88-8da0-999aa2d1ad7f"
                    //    ErrorCode: "ProviderTechnicalError"
                    //    ErrorDescription: "Error in Service Provider, Please Contact ESB Administrator."
                    //    ExternalId: "7ab164e2-2850-451a-8f45-2b7d24fb4368"
                    //    Status: "TechnicalError"
                    errMessage = "TechnicalError:" + responseHeader.ErrorDescription + ": " + responseHeader.ErrorCode;
                    throw new FaultException<UnifreightIIGFault>(
                        new UnifreightIIGFault(UnifreightIIGFault.PlaceFaultEnum.IIGTechnicalError, errMessage));
                }


            }


            if (responseContentHeader == null)
            {
                return;
            }

            if (responseHeader != null)
            {
                errMessage = responseHeader.Status;
            }
            else
            {
                errMessage = "IIGBLError:"; //responseContentHeader:"From DCAE
            }

            var exceptionList = responseContentHeader.GetException();
            if (exceptionList == null)
            {
                return;//??
            }
            if (exceptionList.Count() < 1)
            {
                return;
            }
            errMessage = "תשובת המכס  :"; //+Environment.NewLine;
            foreach (var curException in exceptionList)
            {
                errMessage += Environment.NewLine + curException.ExeptionDescription;//In Hebrew  

                if (curException.ExeptionDescription.Contains("Please Contact ESB Administrator"))
                {
                    errMessage += "\n יש לפנות למוקד מלמ - שער עולמי טלפון 03-5312222 שלוחה 1";
                }
            }
            throw new FaultException<UnifreightIIGFault>(
                    new UnifreightIIGFault(UnifreightIIGFault.PlaceFaultEnum.IIGBusinessError, errMessage));
        }
        private TResponseData SendDCASign(byte[] customRequestSignedByteArry, TRequestParams requestParams)
        {
            throw new NotImplementedException();
        }
#if false
        private static Customs.BL.EntityPMs.IIGMessagePM GetMessageDef()
        {
            var messageQueryService = new IIGMessageQueryService();
            var MessageDef = messageQueryService.GetAll()
                .FirstOrDefault(rec =>
                    rec.InOut == Customs.BL.EntityPMs.IIGMessagePM.InOutType.Out
                    &&
                    (
                    rec.MalamClass.Equals(typeof(TCustomsRequest).Name, StringComparison.OrdinalIgnoreCase)
                    ||
                    rec.PrefixFileName.ToLower().Contains(typeof(TCustomsRequest).Name)
                    )
                    );
            MessageDef = MessageDef ?? new Logitude.Customs.BL.EntityPMs.IIGMessagePM();
            return MessageDef;
        }
#endif




        private TResponseData SendDCA(TRequestParams requestParams, TCustomsRequest customsRequest)
        {
            //var my=_MessageDef.Interactive;
            TResponseData responseData = new TResponseData();
            responseData.Succeeded = true;
            return responseData;

        }

        private byte[] TaskSignIt(int tenant, TCustomsRequest customsRequest)
        {
            var sw = Stopwatch.StartNew();
            byte[] bytesSignedSerilazeObject = null;
            try
            {
                LogMessagingUtil.Instance.AppendLine("Must Sign It");
                BuildRequestContentHeaderB4Sign(customsRequest);
                //ValidateCustomsRequestB4Send(customsRequest);
                var xmlSerilazeObject = XmlGenericUtil<TCustomsRequest>.SerializeObject(customsRequest);
                var bytesSerilazeObject = UTF8Encoding.UTF8.GetBytes(xmlSerilazeObject);
                var signManager = new SignManager();
                bytesSignedSerilazeObject = signManager.SignBytes(tenant, bytesSerilazeObject);

                //if (Environment.MachineName.StartsWith("itzik", StringComparison.OrdinalIgnoreCase))
                //{
                //    File.WriteAllBytes(@"c:\1.signed", bytesSignedSerilazeObject);
                //}

            }
            catch (Exception ex)
            {
                var msg = "Sign service failed " + Environment.NewLine + ex.Message;
                ex.GetType().GetField("_message", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(ex, msg);
                throw ex;

                //throw new Exception("Sign service failed ", e);
            }
            return bytesSignedSerilazeObject;

        }

        private byte[] TaskSignItHSM(int tenant, TCustomsRequest customsRequest)
        {
            var sw = Stopwatch.StartNew();
            byte[] bytesSignedSerilazeObject = null;
            try
            {
                LogMessagingUtil.Instance.AppendLine("Must HSM Sign It");
                //BuildRequestContentHeaderB4Sign(customsRequest);
                
                var xmlSerilazeObject = XmlGenericUtil<TCustomsRequest>.SerializeObject(customsRequest);
                var bytesSerilazeObject = UTF8Encoding.UTF8.GetBytes(xmlSerilazeObject);

                CustomsSettingQueryService settingService = new CustomsSettingQueryService(RequestParams.Tenant);
                var setting = settingService.GetSettingByTenantN(RequestParams.Tenant);

                var hSMSignFileService = new HSMSignFileService();

                SignQueueByType signQueueByType = SignQueueByType.None;
                string companypersonal = "";
                Enum.TryParse<SignQueueByType>(RequestParams.SignQueueByCompanyOrPersonal, out signQueueByType);
                switch (signQueueByType)
                {

                    case SignQueueByType.SignQueueByPersonId:
                        companypersonal = "P";
                        break;
                    case SignQueueByType.SignQueueByCustomsAgentId:
                    default:
                        companypersonal = "C";
                        break;
                }
                LogMessagingUtil.Instance.AppendLine(
                    $"hSMSignFile({RequestParams.SignByPersonalId}, {companypersonal})");
                bytesSignedSerilazeObject  = hSMSignFileService
                    .SignCustomsRequest(
                    RequestParams.Tenant, RequestParams.PBId,
                    RequestParams.SignByPersonalId, companypersonal,
                    setting.CustomsAgentId, bytesSerilazeObject, RequestParams?.HsmStationContext);



            }
            catch (Exception ex)
            {
                var msg = "HSM Sign service failed " + Environment.NewLine + ex.Message;
                ex.GetType().GetField("_message", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(ex, msg);
                throw ex;

                //throw new Exception("Sign service failed ", e);
            }
            return bytesSignedSerilazeObject;

        }

        private TCustomsRequest TaskCustomRequest(TRequestParams requestParams)
        {
            var stopwatch = Stopwatch.StartNew();
            //this.CustomsSetting = CustomsSettingQueryService.GetSettingByTenant(requestParams.Tenant);
            LogMessagingUtil.Instance.AppendLine("CustomRequest:");
            if (_RequestService == null)
            {
                _RequestService = new TRequestService();
            }
            if (requestParams.LoggingEntityId != null && requestParams.LoggingObjectTableId != null) 
            { 
			    GeneralLockQueryService generalLockQueryService = new GeneralLockQueryService(requestParams.Tenant);
			    var generalLock = generalLockQueryService.CheckIsLocked(requestParams.Tenant, "MessageInteractive", requestParams.LoggingUserId,requestParams.LoggingEntityId,requestParams.LoggingObjectTableId,true);

			    if (generalLock != null)
                {
                    string message = $"The entity {generalLock.EntityId1} object {generalLock.ObjectTable1} is locked by {generalLock.UserName}";
			    	LogMessagingUtil.Instance.AppendLine(message);
			    	throw new Exception(message);
			    }
			}
			TCustomsRequest customsRequest = _RequestService.GetRequest(requestParams);
            if (customsRequest == null) //itzik
            {
                LogMessagingUtil.Instance.AppendLine("_RequestService.GetRequest(requestParams) return null ???!!!  ");
                throw new Exception("_RequestService.GetRequest(requestParams) return null ???!!!  ");
            }
            BuildRequestContentHeaderB4Sign(customsRequest);

            LogMessagingUtil.Instance.AppendLine("MessagingServiceBase:requestService.GetRequest:Elapsed:" + stopwatch.Elapsed.ToString());


            //if (!requestParams.LoggingEnabled)
            //{
            var tempXml = XmlGenericUtil<TCustomsRequest>.SerializeObject(customsRequest);
            LogMessagingUtil.Instance.AppendLine("customsRequest:").AppendLine(tempXml);
            //}
            this._HugeFile = (tempXml.Length //* sizeof(char) 
                > this.HugeFileSize);
            if (!this._HugeFile)
            {
                ValidateCustomsRequestB4Send(customsRequest);
            }
            else
            {
                LogMessagingUtil.Instance.AppendLine("_HugeFile:suppress>ValidateCustomsRequestB4Send");
            }
            LogMessagingUtil.Instance.AppendLine("MessagingServiceBase:requestService.GetRequest:ValidateCustomsRequestB4Send:Elapsed:" + stopwatch.Elapsed.ToString());
            LogMessagingUtil.Instance.AppendLine("Done");

            return customsRequest;
        }

        private void ValidateCustomsRequestB4Send(TCustomsRequest customsRequest)
        {
            if (_CustomsRequestsSheetService == null)
            {
                return;
            }
            if (this._HugeFile) return;
            var featureSuppressValidateCustomsRequestB4Send = true;//ConfigurationManager.AppSettings["20180201.SuppressValidateCustomsRequestB4Send"] == "1";
            if (featureSuppressValidateCustomsRequestB4Send)
            {
                LogMessagingUtil.Instance.AppendLine("20180201.SuppressValidateCustomsRequestB4Send");
                return;
            }
            

            //ValidateCustomsRequestB4Send = true;//better  now  then in the IIG SERVER -
            if (ToValidateCustomsRequestB4Send() || _CustomsRequestsSheetService.MyCustomsRequestsSheetPM.IsDCA)
            {
                LogMessagingUtil.Instance.AppendLine("ValidateCustomsRequestB4Send better  now  then in the IIG SERVER ");
                try
                {
                    var reqService = new TRequestHeader();
                    var serviceName = reqService.ServiceName;
                    var myValidateDocumentUtil = new UnifreightIIG.Common.Utils.ValidateDocumentUtil();
                    myValidateDocumentUtil.ValidateObjectAgainstWSDL<TCustomsRequest>(customsRequest, serviceName);
                }
                catch (FaultException<UnifreightIIGFault> fault)
                {
                    var FormattedMessage = UnifreightIIG.Common.Utils.ErrorHandlerUtil.CreateNew().ToFormattedMessage(fault);
                    throw new BusinessErrorException(FormattedMessage);
                    //throw new CustomsRequestsSheetServiceException(
                    //    CustomsRequestsSheetServiceException.WhereEnum.BLException, CustomsRequestsSheetServiceException.What2DoEnum.StopQueue
                    //,"Validate CustomsRequest Before Send Failed :" +FormattedMessage, null);;

                }

            }
        }


        private void UpdateCommunicationLog(TRequestParams requestParams, string requestCommunicationLogId, string exceptionMessage)
        {
            if (!requestParams.LoggingEnabled) return;
            if (string.IsNullOrWhiteSpace(requestCommunicationLogId)) return;



            using (TransactionScope scope = TransactionFactory.GetTransaction())
            {

                int tenant = requestParams.Tenant;
                ICommonDataContext commonContext = CommonDataContext.GetContext(tenant);
                CommunicationLogRepository communicationLogRepository = new CommunicationLogRepository(commonContext);
                CommunicationLog commLog = communicationLogRepository.GetSingleCommunicationLog(requestCommunicationLogId, tenant);
                commLog.ExceptionMessage = exceptionMessage;
                commLog.CorrelationID = this._CorrelationId;
                commLog.Logs = LogMessagingUtil.Instance.ToString(8000);

                commLog.CommunicationStatusTypeCode = "F";
                commLog.LastStatusDate = TenantServerConfigration.GetCurrentDateTime(commLog.Tenant);
                communicationLogRepository.Update(commLog);
                communicationLogRepository.SubmitChanges();
                scope.Complete();
            }


        }



        private void LogRequest(TCustomsRequest customRequest, TRequestParams requestParams, string CommunicationStatusTypeCode)
        {

            return;
            if (!requestParams.LoggingEnabled)
            {
                return;
            }
            if (customRequest == null)
            {
                return;
            }
            using (TransactionScope scope = TransactionFactory.GetTransaction())
            {
                string communicationLogId = this.RequestsSheetExternalId;

                //TCustomsRequest customsRequest = default(TCustomsRequest);


                Document document = LogMessage(requestParams, requestParams.RequestName + "(" + typeof(TCustomsRequest).Name + ")", "O", CommunicationStatusTypeCode, ref communicationLogId, "");
                LogMessagingUtil.Instance.AppendLine("LogRequest:communicationLogId:" + communicationLogId);
                //SerializeAndBolbIt(customRequest, requestParams, document);
                MemoryStream memstream =
                    XmlGenericUtil<TCustomsRequest>.MemoryStreamSerializeWithDefaultNamespace(customRequest);
                //Serialize<TCustomsRequest>(customRequest);
                SetBlob(requestParams, document, memstream);
                scope.Complete();

            }

        }

        private void LogResponse(TCustomsResponse customResponse, TRequestParams requestParams, string CommunicationStatusTypeCode)
        {
            return;
            if (!requestParams.LoggingEnabled)
            {
                return;
            }
            if (customResponse == null)
            {
                return;
            }
            using (TransactionScope scope = TransactionFactory.GetTransaction())
            {
                var communicationLogId = "";
                Document document = LogMessage(requestParams, requestParams.ResponseName + " (" + typeof(TCustomsResponse).Name + ")", "I", CommunicationStatusTypeCode, ref communicationLogId);
                //SerializeCustomResponse(customResponse, requestParams, document);
                MemoryStream memstream =
                    XmlGenericUtil<TCustomsResponse>.MemoryStreamSerializeWithDefaultNamespace(customResponse);
                //Serialize<TCustomsResponse>(customResponse);
                SetBlob(requestParams, document, memstream);

                scope.Complete();
            }

        }

        private static Document LogMessage(TRequestParams requestParams, string subject, string InOut, string CommunicationStatusTypeCode, ref string communicationLogId, string correlationId = "")
        {
            int tenant = requestParams.Tenant;
            ICommonDataContext commonContext = CommonDataContext.GetContext(tenant);
            CommunicationLogRepository communicationLogRepository = new CommunicationLogRepository(commonContext);
            DocumentRepository documentrepository = new DocumentRepository(commonContext);

            Document document = new Document()
            {
                CreateDate = DateTime.Now,
                Extension = "xml",
                FileSize = 999,
                Tenant = Convert.ToInt32(tenant),
                Id = IdCounter.GetNumber("Document", tenant),
                HasFile = true,
                Folder = "customs",
            };
            documentrepository.Add(document);
            documentrepository.SubmitChanges();
            if (String.IsNullOrEmpty(communicationLogId))
            {
                communicationLogId = IdCounter.GetNumber("CommunicationLog", tenant);
            }
            CommunicationLog commLog = new CommunicationLog()
            {
                Id = communicationLogId,
                LastStatusDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                To = "Customs",
                InOut = InOut,
                EntityId = requestParams.LoggingEntityId,
                ObjectTableId = requestParams.LoggingObjectTableId,
                Subject = subject,
                Tenant = tenant,
                CommunicationLogTypeCode = "T",
                CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                CommunicationStatusTypeCode = CommunicationStatusTypeCode,
                CreatedByUserId = requestParams.LoggingUserId,
                DocumentId = document.Id,
                EntityReference = requestParams.LoggingEntityReference,
                CorrelationID = correlationId,
                Logs = LogMessagingUtil.Instance.ToString(8000),
                LastStatusDateUTC = DateTime.UtcNow,
                CreateDateUTC = DateTime.UtcNow,
            };
            communicationLogRepository.Add(commLog);
            communicationLogRepository.SubmitChanges();
            communicationLogId = commLog.Id;
            return document;
        }

        private void SerializeAndBolbIt<MyType>(MyType myObject, TRequestParams requestParams, Document document)
            where MyType : class
        {
            MemoryStream memstream =
                XmlGenericUtil<MyType>.MemoryStreamSerialize(myObject);
            //Serialize<MyType>(myObject);
            SetBlob(requestParams, document, memstream);
        }

        private static void SetBlob(TRequestParams requestParams, Document document, MemoryStream memstream, string documentSufix = "")
        {


            var stopwatch = Stopwatch.StartNew();
            //string filename = document.Id + documentSufix + "." + document.Extension;
            string filePath = "";// "tenant" + requestParams.Tenant + "/" + StorageAcountDetails.GetBlobNameByLocation(filename, document.Folder);
            filePath = document.GetBlobUrl(documentSufix); ///GetBlobUrl(requestParams.Tenant, document, documentSufix);
            Logitude.Server.Tools.BlobFileInfo fileInfo = new Logitude.Server.Tools.BlobFileInfo()
            {
                FileName = document.Id,
                FolderName = document.Folder,
                Extension = document.Extension,
                Tenant = document.Tenant,
                FileSize = document.FileSize,
            };
            IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
            storageservice.Write(memstream.ToArray(), fileInfo);

            stopwatch.Stop();
            LogMessagingUtil.Instance.AppendLine("SetBolb:" + filePath + "Took:" + stopwatch.Elapsed.ToString());
        }
        private static bool GetBlob(int tenant, Document document, string documentSufix, out string xmlfile)
        {
            byte[] ArryByte = null;
            xmlfile = null;
            if (!GetBlob(tenant, document, documentSufix, out ArryByte))
            {
                return false;
            }
            Encoding encoding = Encoding.UTF8;
            xmlfile = encoding.GetString(ArryByte);

            return true;
        }
        private static bool GetBlob(int tenant, Document document, string documentSufix, out byte[] ArryByte)
        {
            ArryByte = null;
            var stopwatch = Stopwatch.StartNew();

            string filePath;
            filePath = document.GetBlobUrl(documentSufix); //GetBlobUrl(tenant, document, documentSufix);
             
            LogMessagingUtil.Instance.AppendLine("Try Read Bolb :" + filePath);
           
            Logitude.Server.Tools.BlobFileInfo fileInfo = new Logitude.Server.Tools.BlobFileInfo()
            {
                FileName = document.Id,
                FolderName = document.Folder,
                Extension = document.Extension,
                Tenant = document.Tenant,
                FileSize = document.FileSize,
            };
            Logitude.Server.Tools.StorageService.IBlobService storageservice = Logitude.Server.Tools.ContainerAccessor.Container.Resolve(typeof(Logitude.Server.Tools.StorageService.IBlobService), "StorageService", new ParameterOverride("", 1)) as Logitude.Server.Tools.StorageService.IBlobService;
            ArryByte = storageservice.Read(fileInfo);


                  

            stopwatch.Stop();
            LogMessagingUtil.Instance.AppendLine("getBolb:" + filePath + "Took:" + stopwatch.Elapsed.ToString());
            if (ArryByte == null)
            {
                LogMessagingUtil.Instance.AppendLine("Bolb is null! ");
                //LogMessagingUtil.Instance.AppendLine(response.ErrorMessage);
                //throw new Exception("Bolb is null!  " + response.ErrorMessage);
                return false;
            }
            //ArryByte = response.Result as byte[];
            return true;
        }







        private void SerializeCustomResponse(TCustomsResponse customResponse, TRequestParams requestParams, Document document)
        {
            MemoryStream memstream = new MemoryStream();

            XmlSerializer ser = new XmlSerializer(typeof(TCustomsResponse));

            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("", "http://www.champ.aero/GCCS/CargoXML");
            XmlWriterSettings settings = new XmlWriterSettings()
            {
                Indent = true,
                IndentChars = "",
                OmitXmlDeclaration = true,
                NewLineChars = "",
                NewLineHandling = NewLineHandling.Replace,


            };

            XmlWriter writer = XmlTextWriter.Create(memstream, settings);
            writer.WriteRaw("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>\r\n");
            ser.Serialize(writer, customResponse, ns);
            memstream.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(memstream);
            string content = reader.ReadToEnd();
            content = content.Replace(" />", "/>");
            byte[] bytearray = Encoding.UTF8.GetBytes(content);
            string filename = document.Id + "." + document.Extension;

            Logitude.Server.Tools.BlobFileInfo fileInfo = new Logitude.Server.Tools.BlobFileInfo()
            {
                FileName = document.Id,
                FolderName = document.Folder,
                Extension = document.Extension,
                Tenant = document.Tenant,
                FileSize = memstream.Length,
            };
            Logitude.Server.Tools.StorageService.IBlobService storageservice = Logitude.Server.Tools.ContainerAccessor.Container.Resolve(typeof(Logitude.Server.Tools.StorageService.IBlobService), "StorageService", new ParameterOverride("", 1)) as Logitude.Server.Tools.StorageService.IBlobService;
            storageservice.Write(memstream.ToArray(), fileInfo);

            //var blobContainer = StorageAcountDetails.GetCurrentContainer(requestParams.Tenant);
            //var blobfile = blobContainer.GetBlockBlobReference(StorageAcountDetails.GetBlobNameByLocation(filename, document.Folder));
            //using (Stream blobstream = blobfile.OpenWrite())
            //{
            //    blobstream.Write(memstream.ToArray(), 0, (int)memstream.Length);
            //}
        }

        public virtual string TestSendXml(string customXmlRequest, out string exceptionMessage)
        {
            exceptionMessage = "";
            var customXmlResponseXmlOut = "";
            //var  myRequest = 
            TCustomsRequest customsRequest = default(TCustomsRequest);

            try
            {
                customsRequest = XmlGenericUtil<TCustomsRequest>.DeSerializeObject(customXmlRequest);
            }
            catch (Exception)
            {
                string text = "";
                customsRequest = new TCustomsRequest();
                text = XmlGenericUtil<TCustomsRequest>.SerializeObject(customsRequest);
                throw new Exception("Bad customXmlRequest!! Try this :" + Environment.NewLine + text);
            }


            TRequestParams requestParams = default(TRequestParams);

            TCustomsResponse customsResponse = default(TCustomsResponse);
            TResponseData responseData = default(TResponseData);
            try
            {
                responseData = this.Send(null, customsRequest);
                //customsResponse = this.CallWS(customsRequest, requestParams, out exceptionMessage);
            }
            catch (Exception ex)
            {

                string correlationId="";
                exceptionMessage = UnifreightIIG.Common.Utils.ErrorHandlerUtil.CreateNew().ToFormattedMessage(ex, out correlationId);
                _CorrelationId = correlationId;
                return "";

            }
            customXmlResponseXmlOut = XmlGenericUtil<TResponseData>.SerializeObject(responseData);

            return customXmlResponseXmlOut;


        }










        public string RequestsSheetExternalId { get; private set; }

        

        protected Logitude.Customs.Def.EntityPMs.CustomsSettingPM CustomsSetting
        {
            get
            {

                int tenant;
                if (RequestParams != null)
                {
                    tenant = RequestParams.Tenant;
                }
                else
                {
                    tenant = this._CustomsRequestsSheetService.MyCustomsRequestsSheetPM.Tenant;
                }
                if (_CustomsSetting == null)
                {
                    _CustomsSetting = CustomsSettingQueryService.GetSettingByTenant(tenant);
                }
                return _CustomsSetting;
            }
#if false
            set
            {

                _CustomsSetting = value;
                if (_CustomsSetting == null)
                {
                    throw new Logitude.Server.Tools.Models.BusinessErrorException("Please Note the CustomsSetting is null");
                }

            }   
#endif

        }

        public TRequestService _RequestService { get; set; }



        
    }

    public class DefaultResponseHeaderOrFault : IResponseHeaderOrFault
    {
        public string CorrelationId { get; set; }
        public string ErrorCode { get; set; }
        public string ErrorDescription { get; set; }
        public string ExternalId { get; set; }
        public string Status { get; set; }

        


        event PropertyChangedEventHandler IResponseHeaderOrFault.PropertyChanged
        {
            add { throw new NotImplementedException(); }
            remove { throw new NotImplementedException(); }
        }
    }
}
