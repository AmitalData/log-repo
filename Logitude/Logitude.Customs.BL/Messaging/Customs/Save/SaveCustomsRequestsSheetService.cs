
using Logitude.AmitalMessaging.Utils;
using Logitude.Customs.BL.ClosedTable;
using Logitude.Customs.BL.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.Data;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.StorageService;
using Microsoft.Practices.Unity;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Xml;
using System.Xml.Serialization;

namespace Logitude.Customs.BL.Messaging.Customs.Save
{

    public class SaveCustomsRequestsSheetService :IDisposable 
    //<TRequestParams>        where TRequestParams : RequestParamsBase
    {
        ICommonDataContext _CommonContext;

        CommunicationLogRepository _CommunicationLogRepository;
        private CommunicationLogStepRepository _CommunicationLogStepRepository;
        private CustomsRequestsSheetQueryService _CustomsRequestsSheetQueryService;

        CustomsRequestsSheetPM _CustomsRequestsSheetPM;
        CommunicationLog _CommunicationLog;
        List<CommunicationLogStep> _CommunicationLogStepList;


        //TRequestParams
        RequestParamsBase _RequestParams;




        InterfaceManagementPM _MessageDefinition;

        

        int _Tenant;

        private DateTime _StartStepAt;
        private CustomsStepEnum _CustomsRequestStepEnum;
        private IMessageController _MessageController;
        private CustomsRequestsSheetUpdateService _CustomsRequestsSheetUpdateService;
        private ICustomContext _CustomContext;
        


        SaveCustomsRequestsSheetService()
        {

        }

        SaveCustomsRequestsSheetService(int tenant)//,string InterfaceTypeCode)
        {
            _Tenant = tenant;
            ///_InterfaceTypeCode=InterfaceTypeCode;
            _CommonContext = CommonDataContext.GetContext(_Tenant);
            _CustomContext=CustomContext.GetContext(_Tenant);
            _CommunicationLogRepository = new CommunicationLogRepository(_Tenant);
            _CommunicationLogStepRepository = new CommunicationLogStepRepository(_CommonContext);


            _CustomsRequestsSheetQueryService = new CustomsRequestsSheetQueryService(_CustomContext);
            _CustomsRequestsSheetUpdateService = new CustomsRequestsSheetUpdateService(_CustomContext, new Dictionary<string, IContext>(), this._Tenant);
            
        }
        
        SaveCustomsRequestsSheetService(RequestParamsBase requestParams)
            : this(requestParams.Tenant)//,requestParams.InterfaceTypeCode )
        {
            // TODO: Complete member initialization
            this.RequestParams = requestParams;
        }


        public static SaveCustomsRequestsSheetService
            CreateNew<TRequestParams>(TRequestParams requestParams)
            where TRequestParams : RequestParamsBase
        {

            try
            {
                CheckRequestParamsBase(requestParams);
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
                {
                    Logitude.Server.Tools.Helpers.LogMessagingUtil.Instance.AppendLine("CustomsRequestsSheetService CreateNew():interfaceTypeCode  " +
                    requestParams.InterfaceTypeCode);
                    var customsRequestsSheetService = new SaveCustomsRequestsSheetService//<TRequestParams>
                        (requestParams as RequestParamsBase);
                    customsRequestsSheetService.CreateNewComm();
                    customsRequestsSheetService.BuildSteps();
                    customsRequestsSheetService.CreateNewRequestSheet();
                    

                    //requestParams.CustomsRequestsSheetId = customsRequestsSheetService._CustomsRequestsSheetPM.Id;

                    customsRequestsSheetService.StartStep(CustomsStepEnum.StartRequestParams);
                    var mem = Serialize<TRequestParams>(requestParams);
                    customsRequestsSheetService.EndStepWithoutTransactionScope(mem, CommStatusEnum.D);
                    customsRequestsSheetService.StartCustomsRequestStepEnum = customsRequestsSheetService.GetCurrentProcessState(); 

                    scope.Complete(); 
                    return customsRequestsSheetService;
                }
            }
            catch (Exception e)
            {

                throw new
                    CustomsRequestsSheetServiceException(
                    CustomsRequestsSheetServiceException.WhereEnum.CustomsRequestsSheetServiceException, CustomsRequestsSheetServiceException.What2DoEnum.StopQueue,
                     "CreateNew<TRequestParams>(TRequestParams requestParams) Crash See inner Exception", e);
            }

        }

        private static void CheckRequestParamsBase(RequestParamsBase requestParams)
        {
            if (string.IsNullOrWhiteSpace(requestParams.InterfaceTypeCode))
            {
                throw new Exception("requestParams.InterfaceTypeCode is must");
            }
            if (requestParams.Tenant ==0)
            {
                throw new Exception("requestParams.Tenant ==0");
            } 
            
        }
        
        public static SaveCustomsRequestsSheetService
            Seed<TRequestParams>(string customsRequestsSheetId, int tenant, TRequestParams defaultRequestParamsFromCustomsResponse=null)
            /*

select * 
from Customs.CustomsRequestsSheets ,CommunicationLogs ,CommunicationLogSteps
where 
Customs.CustomsRequestsSheets.Id='1-8' and 
CommunicationLogs.Id=Customs.CustomsRequestsSheets.RequestComminicationId and 
CommunicationLogSteps.CommunicationLogId= CommunicationLogs.id

             */
            where TRequestParams : RequestParamsBase
        {
            try
            {

                Logitude.Server.Tools.Helpers.LogMessagingUtil.Instance.AppendLine("CustomsRequestsSheetService Retrive():customsRequestsSheetId  " + customsRequestsSheetId);

                var customsRequestsSheetService = new SaveCustomsRequestsSheetService//<TRequestParams>
                    (tenant);

                customsRequestsSheetService._CustomsRequestsSheetPM = customsRequestsSheetService._CustomsRequestsSheetQueryService
                    .GetSingle(customsRequestsSheetId, true, 
                    false //true
                    );
                customsRequestsSheetService.InitMessageDefinition();
                if (customsRequestsSheetService._CustomsRequestsSheetPM == null)
                {
                    if (defaultRequestParamsFromCustomsResponse != null)
                    {
                        customsRequestsSheetService.Dispose();
                        Logitude.Server.Tools.Helpers.LogMessagingUtil.Instance.AppendLine("DCA Recived - not callback  " );
                        customsRequestsSheetService = SaveCustomsRequestsSheetService.CreateNew<TRequestParams>(defaultRequestParamsFromCustomsResponse);
                        return customsRequestsSheetService;
                    }
                    var mess = "Not exist customsRequestsSheetId:" + customsRequestsSheetId;
                    Logitude.Server.Tools.Helpers.LogMessagingUtil.Instance.AppendLine(mess);
                    throw new Exception(mess);
                }
                if (defaultRequestParamsFromCustomsResponse != null)
                {
                    Logitude.Server.Tools.Helpers.LogMessagingUtil.Instance.AppendLine("DCA Return - Callback  ");
                }
                customsRequestsSheetService._CommunicationLog = customsRequestsSheetService._CommunicationLogRepository.GetSingleCommunicationLog(customsRequestsSheetService._CustomsRequestsSheetPM.RequestComminicationId, customsRequestsSheetService._CustomsRequestsSheetPM.Tenant);
                customsRequestsSheetService._CommunicationLogStepList = customsRequestsSheetService.
                    _CommunicationLogStepRepository.GetMultiCommunicationLog(customsRequestsSheetService._CommunicationLog.Id, customsRequestsSheetService._CommunicationLog.Tenant);

                
                //customsRequestsSheetService._RequestParams = reqParams;
                customsRequestsSheetService.DeSerializeRequestParams<TRequestParams>();
                customsRequestsSheetService.StartCustomsRequestStepEnum = customsRequestsSheetService.GetCurrentProcessState(); 
                Logitude.Server.Tools.Helpers.LogMessagingUtil.Instance.AppendLine("retieve success CustomsRequestsSheetService:" + customsRequestsSheetService._CustomsRequestsSheetPM.Id);

                return customsRequestsSheetService;
            }
            catch (Exception e)
            {

                throw new
                    CustomsRequestsSheetServiceException(
                    CustomsRequestsSheetServiceException.WhereEnum.CustomsRequestsSheetServiceException, CustomsRequestsSheetServiceException.What2DoEnum.StopQueue,
                     "Seed<TRequestParams>() Crash See inner Exception", e);
            }
        }
        Dictionary<CustomsStepEnum, byte[]> _BlobCach = new Dictionary<CustomsStepEnum, byte[]>();
        public byte[] GetBolb(CustomsStepEnum customsRequestStep)
        {
            if (_BlobCach.ContainsKey(customsRequestStep))
            {
                return _BlobCach[customsRequestStep];
            }
            byte[] ArryByte = null;
            var communicationLogStep = GetCommunicationLogStep(customsRequestStep);
            if (!GetBlob(communicationLogStep.Tenant, communicationLogStep.Document, out ArryByte))
            {
                throw new Exception("Bolb not found");
            }
            _BlobCach.Add(customsRequestStep, ArryByte);
            return ArryByte;
        }
        private static bool GetBlob(int tenant, Document document, out byte[] ArryByte)
        {
            ArryByte = null;
            var stopwatch = Stopwatch.StartNew();

            string filePath;
            filePath = document.GetBlobUrl(""); //GetBlobUrl(tenant, document, documentSufix);
            IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
            LogMessagingUtil.Instance.AppendLine("Try Read Bolb :" + filePath);
            Logitude.Server.Tools.BlobServiceReference.Response response = storageservice.Read(filePath);

            stopwatch.Stop();
            LogMessagingUtil.Instance.AppendLine("getBolb:" + filePath + "Took:" + stopwatch.Elapsed.ToString());
            if (response.Result == null)
            {
                LogMessagingUtil.Instance.AppendLine("Bolb is null! ");
                LogMessagingUtil.Instance.AppendLine(response.ErrorMessage);
                //throw new Exception("Bolb is null!  " + response.ErrorMessage);
                return false;
            }
            ArryByte = response.Result as byte[];
            return true;
        }
        
        public IMessageController MessageController
        {
            get
            {
                if (_MessageController == null)
                {
                    _MessageController = new DefaultMessageController(); 
                }
                return _MessageController;
            }
            set { _MessageController = value; }
        }

        

        public void StartStep(CustomsStepEnum customsRequestStep)
        {
            _CustomsRequestStepEnum = customsRequestStep;
            _StartStepAt = DateTime.Now;//DateTime.Now;//Server4 tIme 
        }
        public Exception FailStepRaiseCRSSExeption(Exception ee)
        {

            var communicationLogStep = GetCommunicationLogStep();
            if (MessageController.ToRetry(_CustomsRequestStepEnum, communicationLogStep.Retries))
            {
                EndStepWithoutTransactionScope(null, CommStatusEnum.W);
                return new
                   CustomsRequestsSheetServiceException(
                   CustomsRequestsSheetServiceException.WhereEnum.CustomsRequestsSheetServiceException, CustomsRequestsSheetServiceException.What2DoEnum.RetryQueue,
                    "MessageServiceException()Crash See inner Exception", ee);
            }
            else
            {
                EndStepWithoutTransactionScope(null, CommStatusEnum.F);
                
                return new
                   CustomsRequestsSheetServiceException(
                   CustomsRequestsSheetServiceException.WhereEnum.CustomsRequestsSheetServiceException, CustomsRequestsSheetServiceException.What2DoEnum.StopQueue,
                    "MessageServiceException()Crash See inner Exception", ee);

            }
            

            
        }
        public void SetCorrelationId(string _CorrelationId)
        {
            _CustomsRequestsSheetPM.CorrelationId = _CorrelationId;
        }
        public void UpdateConnectedEntitys(RequestParamsBase requestParams)
        {
            _CustomsRequestsSheetPM.ObjectTableId1 = requestParams.LoggingObjectTableId;
            _CustomsRequestsSheetPM.EntityId1 = requestParams.LoggingEntityId;
        }
        void EndStepWithoutTransactionScope(MemoryStream memstream, CommStatusEnum stepStatusEnum = CommStatusEnum.D)
        {
            try
            {

                var serverTime = DateTime.Now;//DateTime.Now;//Server4 tIme 
                CommunicationLogStep communicationLogStep = GetCommunicationLogStep();
                communicationLogStep.Retries++;
                communicationLogStep.Status = stepStatusEnum.ToString();
                communicationLogStep.StartDate = _StartStepAt;
                communicationLogStep.EndDate = serverTime;
                var sb = new StringBuilder(communicationLogStep.Log);
                sb.AppendLine(LogMessagingUtil.Instance.GetLastChars(8000));
                communicationLogStep.Log = sb.ToString().GetLast(8000);

                if (stepStatusEnum == CommStatusEnum.D)
                {
                    if (memstream == null)
                    {
                        throw new
               CustomsRequestsSheetServiceException(
               CustomsRequestsSheetServiceException.WhereEnum.CustomsRequestsSheetServiceException, CustomsRequestsSheetServiceException.What2DoEnum.StopQueue,
                "EndStep()CommStatusEnum.D but memstream == null", null);
                    }

                    SetBlob(memstream, communicationLogStep.Document);
                    var requestStatusCode = GetRequestSheetStatusCodeDone(
                        _CustomsRequestStepEnum
                        //(CustomsRequestStepEnum)communicationLogStep.StepNumber
                        );
                    _CustomsRequestsSheetPM.RequestStatusEnum = requestStatusCode;

                    if (_CustomsRequestStepEnum == CustomsStepEnum.AnalyzeResponseData)
                    {
                        _CustomsRequestsSheetPM.AnswerCreateDate = DateTime.Now;//Ask yaron 
                        _CommunicationLog.CommunicationStatusTypeCode = CommStatusEnum.D.ToString();
                    }
                }
                else if (stepStatusEnum == CommStatusEnum.F)
                {
                    var requestStatusCode = GetRequestSheetStatusCodeFailed(
                        _CustomsRequestStepEnum
                        //(CustomsRequestStepEnum)communicationLogStep.StepNumber
                        );
                    _CustomsRequestsSheetPM.RequestStatusEnum = requestStatusCode;
                    _CommunicationLog.CommunicationStatusTypeCode = CommStatusEnum.F.ToString();
                    
                }




                _CommunicationLog.LastStatusDate = serverTime;
                _CommunicationLog.Retries++;

                
                _CommunicationLogRepository.Update(_CommunicationLog);
                _CustomsRequestsSheetPM.ChangeSetOp = ChangeSetOperation.Update;
                _CustomsRequestsSheetUpdateService.Update(_CustomsRequestsSheetPM, true);
                _CommonContext.SaveChanges();
                LogMessagingUtil.Instance.Clear();

            }
            catch (Exception e)
            {

                throw new
                    CustomsRequestsSheetServiceException(
                    CustomsRequestsSheetServiceException.WhereEnum.CustomsRequestsSheetServiceException, CustomsRequestsSheetServiceException.What2DoEnum.StopQueue,
                     "Seed<TRequestParams>() Crash See inner Exception", e);
            }
        }
        public void EndStep(MemoryStream memstream, CommStatusEnum stepStatusEnum = CommStatusEnum.D)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
                {
                    EndStepWithoutTransactionScope(memstream, stepStatusEnum);
                    scope.Complete();
                }
            }
            catch (Exception e)
            {

                throw new
                    CustomsRequestsSheetServiceException(
                    CustomsRequestsSheetServiceException.WhereEnum.CustomsRequestsSheetServiceException, CustomsRequestsSheetServiceException.What2DoEnum.StopQueue,
                     "Seed<TRequestParams>() Crash See inner Exception", e);
            }
        }

        private CommunicationLogStep GetCommunicationLogStep(CustomsStepEnum? customsRequestStepEnum=null)
        {
            if (customsRequestStepEnum == null)
            {
                customsRequestStepEnum = _CustomsRequestStepEnum;
            }
            var communicationLogStep = _CommunicationLogStepList.FirstOrDefault(rec => rec.StepNumber == (int)customsRequestStepEnum);
            if (communicationLogStep == null)
            {
                throw new
                    CustomsRequestsSheetServiceException(
                    CustomsRequestsSheetServiceException.WhereEnum.CustomsRequestsSheetServiceException, CustomsRequestsSheetServiceException.What2DoEnum.StopQueue,
                     "No communicationLogStep type= " + customsRequestStepEnum.ToString(), null);
            }
            return communicationLogStep;
        }
        private SheetStatusEnum GetRequestSheetStatusCodeFailed(CustomsStepEnum step)
        {
            /*
1	Created
2	In Process
3	Send Failed
4	Analyze Failed
5	Analyzed
6	Cancelled              
             */

            switch (step)
            {
                case CustomsStepEnum.StartRequestParams:
                    return SheetStatusEnum.SendFailed;
                    break;
                case CustomsStepEnum.CustomRequest:
                    return SheetStatusEnum.SendFailed;
                    break;
                case CustomsStepEnum.CustomRequestSign:
                    return SheetStatusEnum.SendFailed;
                    break;
                case CustomsStepEnum.ReceivedCustomResponseCorrelation:
                    return SheetStatusEnum.SendFailed;
                    break;
                case CustomsStepEnum.DCAInProgressUploading:// Sent via DCA Get ServerJobId
                case CustomsStepEnum.DCAInProgressUploaded:
                    return SheetStatusEnum.SendFailed;
                    break;
                case CustomsStepEnum.AnalyzeResponseData:
                    return SheetStatusEnum.AnalyzeFailed;
                    break;
                
                default:
                    throw new Exception("GetRequestStatusCode return null");
                    break;
            }
        }

        public CustomsRequestsSheetServiceException FailSheet(Exception curException)
        {

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
            {
                var sb = new StringBuilder(_CommunicationLog.Logs);
                sb.AppendLine(LogMessagingUtil.Instance.ToString());
                sb.AppendLine(curException.ToString());
                _CommunicationLog.Logs = sb.ToString().GetLast((8000 - 1));
                _CommunicationLog.CommunicationStatusTypeCode = CommStatusEnum.F.ToString();
                _CommunicationLogRepository.Update(_CommunicationLog);
                _CommunicationLogRepository.SubmitChanges();

                var requestStatusCode = GetRequestSheetStatusCodeFailed(_CustomsRequestStepEnum);
                _CustomsRequestsSheetPM.RequestStatusEnum = requestStatusCode;
                 
                _CustomsRequestsSheetPM.ChangeSetOp = ChangeSetOperation.Update;
                _CustomsRequestsSheetUpdateService.Update(_CustomsRequestsSheetPM, true);
                scope.Complete();
            }
            


            return new CustomsRequestsSheetServiceException(
                CustomsRequestsSheetServiceException.WhereEnum.MessageServiceException,
                CustomsRequestsSheetServiceException.What2DoEnum.StopQueue
                    , "FailSheet" + curException.Message, curException);
        }

        private SheetStatusEnum GetRequestSheetStatusCodeDone(CustomsStepEnum step)
        {
            /*
1	Created
2	In Process
3	Send Failed
4	Analyze Failed
5	Analyzed
6	Cancelled              
             */

            switch (step)
            {
                case CustomsStepEnum.StartRequestParams:
                    return SheetStatusEnum.Created;
                    break;
                case CustomsStepEnum.CustomRequest:
                case CustomsStepEnum.CustomRequestSign:
                case CustomsStepEnum.DCAInProgressUploading :
                    return SheetStatusEnum.InProcess;
                    break;
                case CustomsStepEnum.DCAInProgressUploaded:
                case CustomsStepEnum.ReceivedCustomResponseCorrelation:
                    return SheetStatusEnum.Sent;
                    break;
                case CustomsStepEnum.AnalyzeResponseData:
                    return SheetStatusEnum.Analyzed;
                    break;
                default:
                    throw new Exception("GetRequestSheetStatusCodeDone return null");
                    break;
            }
        }
        private void SetBlob(MemoryStream memstream, Document document)
        {


            var stopwatch = Stopwatch.StartNew();
            //string filename = document.Id + documentSufix + "." + document.Extension;
            string filePath = "";// "tenant" + requestParams.Tenant + "/" + StorageAcountDetails.GetBlobNameByLocation(filename, document.Folder);
            filePath = document.GetBlobUrl(""); //GetBlobUrl(tenant, document, documentSufix);
            IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService"
                , new ParameterOverride("", 1)
                ) as IBlobService;
            Logitude.Server.Tools.BlobServiceReference.Response response = storageservice.Write(memstream.ToArray(), filePath);

            stopwatch.Stop();
            LogMessagingUtil.Instance.AppendLine("SetBolb:" + filePath + "Took:" + stopwatch.Elapsed.ToString());
        }
        private void BuildSteps()
        {
            if (_CommunicationLogStepList != null)
            {
                throw new Exception("BuildSteps() already done !!!");
            }
            _CommunicationLogStepList = new List<CommunicationLogStep>();

            //List<CustomsRequestStepEnum> arry = Enum.GetValues(typeof(CustomsRequestStepEnum)).OfType<CustomsRequestStepEnum>();

            var steps = this.MessageController.GetSteps(MessageDefinition, _RequestParams.RequestVIA );
            foreach (var step in steps)
            {


                DocumentRepository documentrepository = new DocumentRepository(_CommonContext);
                var document = new Document()
                {
                    CreateDate = DateTime.Now,
                    Extension = "xml",
                    FileSize = 999,
                    Tenant = Convert.ToInt32(_Tenant),
                    Id = IdCounter.GetNumber("Document", _Tenant),
                    HasFile = true,
                    Folder = "customs",
                };
                documentrepository.Add(document);

                var communicationLogStep = new CommunicationLogStep()
                {
                    CommunicationLogId = _CommunicationLog.Id,
                    StepNumber = (int)step,
                    Tenant = _CommunicationLog.Tenant,
                    Status = "W",
                    Name = step.ToString(),
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now,
                };
                communicationLogStep.DocumentId = document.Id;
                communicationLogStep.Document = document;
                _CommunicationLogStepRepository.Add(communicationLogStep);

                _CommunicationLogStepList.Add(communicationLogStep);
            }
        }
        private void CreateNewComm()
        {
            //int tenant = requestParams.Tenant;


            if (string.IsNullOrWhiteSpace(RequestParams.LoggingUserId))
            {
                RequestParams.LoggingUserId = AuthenticationUtil.ResolveUserId(this._Tenant);
            }




            string communicationLogId = "";
            if (String.IsNullOrEmpty(communicationLogId))
            {
                communicationLogId = IdCounter.GetNumber("CommunicationLog", this._Tenant);
            }
            _CommunicationLog = new CommunicationLog()
            {
                Id = communicationLogId,
                LastStatusDate = DateTime.Now,
                To = "Customs",
                InOut = "O",///TODO //requestParams.InOut,
                EntityId = RequestParams.LoggingEntityId,
                ObjectTableId = RequestParams.LoggingObjectTableId,
                Subject = RequestParams.RequestName,
                Tenant = this._Tenant,
                CommunicationLogTypeCode = "T",
                CreateDate = DateTime.Now,
                CommunicationStatusTypeCode = "W",
                CreatedByUserId = RequestParams.LoggingUserId,
                //DocumentId = MyDocument.Id,
                EntityReference = RequestParams.LoggingEntityReference,
                ///CorrelationID = correlationId,
                Logs = LogMessagingUtil.Instance.ToString(8000),
            };
            bool AddDummydueDocumentIdIsmust = true;//Message=Cannot insert the value NULL into column 'DocumentId', table 'Amital1_Main.dbo.CommunicationLogs'; column does not allow nulls. INSERT fails.
            if (AddDummydueDocumentIdIsmust)
            {
                DocumentRepository documentrepository = new DocumentRepository(_CommonContext);

                Document document = new Document()
                {
                    CreateDate = DateTime.Now,
                    Extension = "xml",
                    FileSize = 999,
                    Tenant = Convert.ToInt32(_Tenant),
                    Id = IdCounter.GetNumber("Document", _Tenant),
                    HasFile = true,
                    Folder = "customs",
                };
                document.HasFile = false;
                documentrepository.Add(document);
                documentrepository.SubmitChanges();
                _CommunicationLog.DocumentId = document.Id;
            }
            
            _CommunicationLogRepository.Add(_CommunicationLog);
            _CommunicationLogRepository.SubmitChanges();
            //communicationLogId = MyCommunicationLog.Id;
            LogMessagingUtil.Instance.AppendLine("LogRequest:communicationLogId:" + _CommunicationLog.Id);
        }

        private void CreateNewRequestSheet()
        {
            if (_CustomsRequestsSheetPM != null)
            {
                throw new Exception("customsRequestsSheetPm Already Exist ");
                throw new Exception("Please Insert customsRequestsSheetPm with override data ");
            }

            if (MessageDefinition == null)
            {
                throw new Exception("Please init _IIGMessagePM ");
            }

            //Logitude.Server.Tools.Helpers.LogMessagingUtil.Instance.AppendLine("selectedFile =" + selectedFile);
            //string externalId = "";// GetExternalId(selectedFile);


            _CustomsRequestsSheetPM = new CustomsRequestsSheetPM()
            {
                ChangeSetOp = ChangeSetOperation.Insert,
                Tenant = this._Tenant,
                RequestOwnerId = RequestParams.LoggingUserId,
                EntityId1 = RequestParams.LoggingEntityId,
                ObjectTableId1 = RequestParams.LoggingObjectTableId,
                EntityReference = RequestParams.LoggingEntityReference,
                InterfaceTypeCode = RequestParams.InterfaceTypeCode,

                RequestStatusEnum = SheetStatusEnum.Created,
                RequestComminicationId = _CommunicationLog.Id,
            };
            InitMessageDefinition();

            switch (_RequestParams.RequestVIA)
            {
                case SendRequestVIA.Default:
                    _CustomsRequestsSheetPM.IsDCA = (MessageDefinition.Interactive == InterfaceManagementPM.InteractiveMode.DCABatchIn ||
                MessageDefinition.Interactive == InterfaceManagementPM.InteractiveMode.DCABatchOutIn
                );
                    break;
                case SendRequestVIA.WebServiceInteractive:
                    break;
                case SendRequestVIA.WebServiceBatch:
                    break;
                case SendRequestVIA.DCABatch:
                    _CustomsRequestsSheetPM.IsDCA = true;
                    break;
                default:
                    break;
            }


            //var currentContext = CustomContext.GetContext(_CommunicationLog.Tenant);

            _CustomsRequestsSheetPM.ChangeSetOp = ChangeSetOperation.Insert;
            _CustomsRequestsSheetUpdateService.Update(_CustomsRequestsSheetPM, true);
            Logitude.Server.Tools.Helpers.LogMessagingUtil.Instance.AppendLine("Create   _CustomsRequestsSheetPM:" + _CustomsRequestsSheetPM.Id);
        }

       
        private void  InitMessageDefinition()
        {
            var interfaceTypeQueryService = new InterfaceManagementQueryService(_Tenant);
            if (_CustomsRequestsSheetPM.InterfaceTypeCode == null)
            {
                throw new Exception("InitInterfaceTypePM but _CustomsRequestsSheetPM.InterfaceTypeCode == null"); 
            }
            MessageDefinition = interfaceTypeQueryService.GetSingle(_CustomsRequestsSheetPM.InterfaceTypeCode, false, false);

            
        }
        


        

        public static MemoryStream Serialize<MyType>(MyType MyObject)
        {
            MemoryStream memstream = new MemoryStream();

            XmlSerializer ser = new XmlSerializer(typeof(MyType));

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
            ser.Serialize(writer, MyObject, ns);
            memstream.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(memstream);
            return memstream;
        }


        public CustomsRequestsSheetPM GetRequestSheet()
        {
            return _CustomsRequestsSheetPM;
        }

        void DeSerializeRequestParams<TRequestParams>()
            where TRequestParams : RequestParamsBase
        {

            //byte[] ArryByte = null;
            //ArryByte = customsRequestsSheetService.GetBolb(CustomsRequestStepEnum.StartRequestParams);
            //var xml = System.Text.Encoding.UTF8.GetString(ArryByte);
            //var reqParams = XmlGenericUtil<TRequestParams>.DeSerializeObject(xml);

            var myArry = this.GetBolb(CustomsStepEnum.StartRequestParams);
            string xml = Encoding.UTF8.GetString(myArry);
            var requestParams =XmlGenericUtil<TRequestParams>.DeSerializeObject(xml);
            
            RequestParams = requestParams;
        }

        public string GetCustomsRequestXml()
        {
            var myArry = this.GetBolb(CustomsStepEnum.CustomRequest);
            string xml = Encoding.UTF8.GetString(myArry);
            
            return xml;
        }


        public InterfaceManagementPM.InteractiveMode GetInteractiveMode()
        {
            if (MessageDefinition == null)
            {
                throw new
                    CustomsRequestsSheetServiceException(
                    CustomsRequestsSheetServiceException.WhereEnum.CustomsRequestsSheetServiceException, CustomsRequestsSheetServiceException.What2DoEnum.StopQueue,
                     "MessageHaveToSign _IIGMessagePM == null", null);
            }
            return MessageDefinition.Interactive ;
        }

        public CustomsStepEnum GetCurrentProcessState()
        {
            if (_CommunicationLogStepList == null)
            {
                throw new
                 CustomsRequestsSheetServiceException(
                 CustomsRequestsSheetServiceException.WhereEnum.CustomsRequestsSheetServiceException, CustomsRequestsSheetServiceException.What2DoEnum.StopQueue,
                  "GetCurrentProcessState _CommunicationLogStepList == null", null);
            }


            var currStep = _CommunicationLogStepList
                .Where(rec => rec.Status == CommStatusEnum.W.ToString())
                .OrderBy(rec => rec.StepNumber)
                .FirstOrDefault();
            if (currStep == null)
            {
                if (_CustomsRequestsSheetPM.RequestStatusEnum == SheetStatusEnum.Analyzed)
                {
                    return CustomsStepEnum.AnalyzeResponseData;
                }
                else
                {
                    throw new
                 CustomsRequestsSheetServiceException(
                 CustomsRequestsSheetServiceException.WhereEnum.CustomsRequestsSheetServiceException, CustomsRequestsSheetServiceException.What2DoEnum.StopQueue,
                  "Logic Error Step Must Be AnalyzeResponseData", null);
                }
            }

            return (CustomsStepEnum)currStep.StepNumber;
        }

        public byte[] GetCustomsRequestSign()
        {
            var myArry = this.GetBolb(CustomsStepEnum.CustomRequestSign);
            return myArry;
        }

        public string GetcustomsResponseXml()
        {
            var myArry = this.GetBolb(CustomsStepEnum.ReceivedCustomResponseCorrelation);
            string xml = Encoding.UTF8.GetString(myArry);
            return xml;
        }

        public CustomsMessaging.Common.DCAParams.DCAServerUploadResponse GetDCAServerUploadResponse()
        {
            var myArry = this.GetBolb(CustomsStepEnum.DCAInProgressUploading);
            string xml = Encoding.UTF8.GetString(myArry);
            LogMessagingUtil.Instance.AppendLine("<DCAServerUploadResponse>.DeserilazeObject");
            var dCAServerResponse = XmlGenericUtil<CustomsMessaging.Common.DCAParams.DCAServerUploadResponse>.DeSerializeObject(xml);
            return dCAServerResponse;
        }
        public CustomsMessaging.Common.DCAParams.DCAServerUploadStatus GetDCAServerUploadStatus()
        {
            var myArry = this.GetBolb(CustomsStepEnum.DCAInProgressUploaded );
            string xml = Encoding.UTF8.GetString(myArry);
            LogMessagingUtil.Instance.AppendLine("<DCAServerUploadStatus>.DeserilazeObject");
            var myDCAServerUploadStatus = XmlGenericUtil<CustomsMessaging.Common.DCAParams.DCAServerUploadStatus>.DeSerializeObject(xml);
            return myDCAServerUploadStatus;
        }

        private RequestParamsBase RequestParams
        {
             get { return _RequestParams; }
             set
            {
                if (_RequestParams == value) return;
                CheckRequestParamsBase(value);
                _RequestParams = value;
                //MessageDefinition = GetInterfaceTypePM(RequestParams.InterfaceTypeCode);
            }
        }
        public TRequestParams GetRequestParams<TRequestParams>()
            where TRequestParams : RequestParamsBase
        {

            return this.RequestParams as TRequestParams;
        }


        public void Dispose()
        {
            _CommonContext = null;

            _CommunicationLogRepository = null;
            _CommunicationLogStepRepository = null;
            _CustomsRequestsSheetQueryService = null;

            _CustomsRequestsSheetPM = null;
            _CommunicationLog = null;
            _CommunicationLogStepList = null;


            //TRequestParams
            _RequestParams = null;


            MessageDefinition = null;

            _MessageController = null;
            _CustomsRequestsSheetUpdateService = null;

        }

        public SendRequestVIA VIA()
        {
            var via = DefaultMessageController.Via(this.MessageDefinition, this._RequestParams.RequestVIA);
            return via;
        }



        public bool IsInteractive { get; set; }
        public CustomsStepEnum StartCustomsRequestStepEnum { get; private set; }
        public InterfaceManagementPM MessageDefinition

        {
            get
            {
                if (_MessageDefinition == null)
                {
                    throw new
                        CustomsRequestsSheetServiceException(
                        CustomsRequestsSheetServiceException.WhereEnum.CustomsRequestsSheetServiceException, CustomsRequestsSheetServiceException.What2DoEnum.StopQueue,
                         "MessageHaveToSign _IIGMessagePM == null", null);
                }
                return _MessageDefinition;
            }
            private set { _MessageDefinition = value; }
        }









        
    }


}
