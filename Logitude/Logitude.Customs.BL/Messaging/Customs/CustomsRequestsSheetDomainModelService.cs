using Logitude.AmitalMessaging.Utils;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.Customs.BL.CloseTables;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.BL.Messaging.Customs.PerformanceLogger;
using Logitude.Customs.BL.Messaging.Customs.SignQueueBL;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityMapping;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Def.ClosedTable;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Def.Messaging.Customs;
using Logitude.CustomsMessaging.Common.Gen;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.ExternalServices;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.StorageService;
using Logitude.Server.Tools.Utils;
using Logitude.SystemLogs;
using Microsoft.Practices.Unity;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using Unifreight.BL.EntityQueryServices;



//using Simplog.Infrastructure.SimplogUtilities;

namespace Logitude.Customs.BL.Messaging.Customs
{


    public class CustomsRequestsSheetDomainModelService<TRequestParams> : IDisposable, IUpdateBolb
        where TRequestParams : RequestParamsBase
        //<TRequestParams>        where TRequestParams : RequestParamsBase
    {
        ICommonDataContext _CommonContext;

        CommunicationLogRepository _CommunicationLogRepository;
        private CommunicationLogStepRepository _CommunicationLogStepRepository;
        private CustomsRequestsSheetQueryService _CustomsRequestsSheetQueryService;

        CustomsRequestsSheetPM _MyCustomsRequestsSheetPM;


        CommunicationLog _CommunicationLog;
        List<CommunicationLogStep> _CommunicationLogStepList;


        //TRequestParams
        TRequestParams _RequestParams;




        InterfaceTenantDefinitionManagementPM _InterfaceTenantDefinitionManagement;
        InterfaceManagementPM _OutMessageDefinition;


        Dictionary<CustomsStepEnum, byte[]> _BlobCach = new Dictionary<CustomsStepEnum, byte[]>();


        readonly int _Tenant;

        private DateTime _StartStepAt;
        private CustomsStepEnum _CurrentCustomsRequestStepEnum;
        private IMessageController _MessageController;
        private CustomsRequestsSheetUpdateService _CustomsRequestsSheetUpdateService;
        private ICustomContext _CustomContext;

        private Nullable<CustomsCommandEnum> _StartCustomsCommand;

        //private ClientProgressBarIndicatorService _ClientProgressBarIndicatorService;


        CustomsRequestsSheetDomainModelService()
        {

        }

        CustomsRequestsSheetDomainModelService(int tenant, OverrideControllerModel overrideControllerModel = null)//,string InterfaceTypeCode)
        {
            _Tenant = tenant;
            MyOverrideControllerModel = overrideControllerModel;
            ///_InterfaceTypeCode=InterfaceTypeCode;
            _CommonContext = CommonDataContext.GetContext(_Tenant);
            _CustomContext = CustomContext.GetContext(_Tenant);
            _CommunicationLogRepository = new CommunicationLogRepository(_CommonContext);
            _CommunicationLogStepRepository = new CommunicationLogStepRepository(_CommonContext);


            _CustomsRequestsSheetQueryService = new CustomsRequestsSheetQueryService(_CustomContext);
            _CustomsRequestsSheetUpdateService = new CustomsRequestsSheetUpdateService(_CustomContext, new Dictionary<string, IContext>(), this._Tenant);

        }

        CustomsRequestsSheetDomainModelService(TRequestParams requestParams, RequestSheetParam reqSheetDetails, bool isInteractive)
            : this(requestParams.Tenant)//,requestParams.InterfaceTypeCode )
        {
			DateTime stopLogAt = DateTime.MinValue;

			string UntilDateyyyyMMdd = Environment.GetEnvironmentVariable("20240205T155633.LogUntilDateyyyyMMdd");

			if (!string.IsNullOrWhiteSpace(UntilDateyyyyMMdd))
			{
				stopLogAt = DateTime.ParseExact(UntilDateyyyyMMdd,
													"yyyyMMdd",
													CultureInfo.InvariantCulture,
													DateTimeStyles.None);
			}
			// TODO: Complete member initialization
			var sw = Stopwatch.StartNew();
            try
            {
                this.IsInteractive = isInteractive;
                CheckRequestParamsBase(requestParams);
                SuppressSendIIGMessages(requestParams);

                CheckMessageInContainer(requestParams.InterfaceTypeCode, requestParams.MainInterfaceCode);
                ConcurrentKiller(requestParams, reqSheetDetails);//Leave the campground cleaner than the way you found it.” found it.

                this.RequestParams = requestParams;            
                InitMessageDefinition();
                ThrowIfInterfaceNotActiveOrBelongOurCompanyType();

                SendRequestVIA requestVIA = _RequestParams.RequestVIA;

                bool avoidSign = false;
                bool courierForceSign = Server.Tools.Helpers.FeatureToggleHelper.HasFeatureToggle("CFS", requestParams.Tenant);//'Courier Force Sign
                if (CustomsSettingQueryService.GetSettingByTenant(requestParams.Tenant).CompanyType == "B"//Courier 
                    && courierForceSign
                    )
                {
                    var courierForceSignService = new CourierForceSignService();
                    courierForceSignService.ApplyForceSign(ref requestParams);
                }
                else
                {

                    
                    avoidSign = AvoidSign(_RequestParams);
                    if (avoidSign)
                    {
                        _RequestParams.AvoidSign = true;
                        if (_RequestParams.ForcePersonalSign)
                        {
                            _RequestParams.ForcePersonalSign = false;
                        }
                    }

                }

                if (_RequestParams.TestCase != null && !String.IsNullOrWhiteSpace(_RequestParams.TestCase.Code))
                {
                    var detail = (new SincroTestCaseDetails()).GetAllSincroTestCaseDetails()
                        .First(r => r.Code == _RequestParams.TestCase.Code);
                    if (detail.AvoidSign)
                    {
                        avoidSign = true;
                    }


                }

                MessageController
                    .BuildRealSteps(InterfaceTenantDefinitionManagement, ref requestVIA, _RequestParams.ForcePersonalSign, avoidSign, requestParams.ForceCompanySign);
                RequestParams.RequestVIA = requestVIA;


                ThrowIfNoAvailablePersonalSignServer();
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {

                    Logitude.Server.Tools.Helpers.LogMessagingUtil.Instance.AppendLine("CustomsRequestsSheetService CreateNew():interfaceTypeCode  " +
                    requestParams.InterfaceTypeCode);


                    this.CreateNewComm();
                    this.CreateNewRequestSheet(requestParams);
                    this.BuildSteps();

                    requestParams.CustomsRequestsSheetId = this._MyCustomsRequestsSheetPM.Id;

                    this.StartStep(CustomsStepEnum.StartRequestParams, null);
                    OnCreateSetDefault();

                    UpdateConnectedEntitys(reqSheetDetails);
                    var mem =
                        XmlGenericUtil<TRequestParams>.MemoryStreamSerialize(requestParams);
                    //this.Serialize<TRequestParams>(requestParams);
                    LogMessagingUtil.Instance.AppendLine("CustomsRequestsSheetDomainModelService.CreateNew(WithoutEndStepWithoutTransactionScope):took:" + sw.ElapsedMilliseconds);
                    this.EndStepWithoutTransactionScope(mem, CommStatusEnum.D);
                    this.StartCustomsRequestStepEnum = this.GetCurrentProcessState();

                    scope.Complete();


                }
                RequestSheetContext.Current.SetRSContext(requestParams);

            }

            catch (CustomsRequestsSheetDomainModelServiceException)
            {
                throw;
            }
            catch (CourierForceSignException e)
            {
				NoteClientNoRequestSheet4U(requestParams, e.Message);
                throw new
                    CustomsRequestsSheetDomainModelServiceException(
                    CustomsRequestsSheetDomainModelServiceException.WhereEnum.CourierForceSignException, CustomsRequestsSheetDomainModelServiceException.What2DoEnum.StopQueue,
                     e.Message, e);
            }
            catch (Exception e)
            {

				NoteClientNoRequestSheet4U(requestParams, e.ToString());
                throw new
                    CustomsRequestsSheetDomainModelServiceException(
                    CustomsRequestsSheetDomainModelServiceException.WhereEnum.CustomsRequestsSheetServiceException, CustomsRequestsSheetDomainModelServiceException.What2DoEnum.StopQueue,
                     "CreateNew<TRequestParams>(TRequestParams requestParams) Crash See inner Exception", e);
            }
            finally
            {
                LogMessagingUtil.Instance.AppendLine("CustomsRequestsSheetDomainModelService.CreateNew():took(total):" + sw.ElapsedMilliseconds);

            }
        }

        private void ThrowIfInterfaceNotActiveOrBelongOurCompanyType()
        {
            bool throwIt = false;
            string errorText = "";
            if (!_InterfaceTenantDefinitionManagement.OverrideActive)
            {
                errorText = "!_InterfaceTenantDefinitionManagement.Active";
                throwIt = true;
            }
            if (!_InterfaceTenantDefinitionManagement.InterfaceManagement.Active)
            {
                errorText = "!_InterfaceTenantDefinitionManagement.InterfaceManagement.Active";
                throwIt = true;
            }

            if (!String.IsNullOrWhiteSpace(_InterfaceTenantDefinitionManagement.InterfaceManagement.InterfaceType))
            {



                var customsSettingQueryService = new CustomsSettingQueryService(_Tenant);
                var customsSettingPM = customsSettingQueryService.GetSingle(_Tenant.ToString(), false, true);
                if (customsSettingPM.CompanyType != _InterfaceTenantDefinitionManagement.InterfaceManagement.InterfaceType)
                {
                    throwIt = true;
                    errorText = "customsSettingPM.CompanyType != _InterfaceTenantDefinitionManagement.InterfaceManagement.InterfaceType";
                }
            }
            if (!throwIt)
            {
                return;
            }
            NoteClientNoRequestSheet4U(RequestParams, errorText);
            var ex = new CustomsRequestsSheetDomainModelServiceException(
               CustomsRequestsSheetDomainModelServiceException.WhereEnum.InterfaceNotActiveOrBelongOurCompanyType, CustomsRequestsSheetDomainModelServiceException.What2DoEnum.StopQueue,
                   errorText, null);
            ex.SuppressExceptionTostring = true;
            throw ex;
        }

        private void ConcurrentKiller(TRequestParams requestParams, RequestSheetParam reqSheetDetails)
        {
            if (reqSheetDetails != null)
            {
                ///Task 40564: טיפול בשליחת בקשות בו זמנית CALL#311773
                bool tryConcurrentKiller = true; //ConfigurationManager.AppSettings["20180718.ConcurrentKiller"] == "1";
                if (tryConcurrentKiller)
                {
                    if (CustomsRequestsSheetQueryService.GetintrefaceTypeListDisplayOnly().ToList().Contains(requestParams.InterfaceTypeCode))
                    {
                        if (!String.IsNullOrWhiteSpace(requestParams.LoggingObjectTableId) &&
                            !String.IsNullOrWhiteSpace(requestParams.LoggingEntityId)

                              &&
                        // eitan: בקשת מכס אחת פר ישות בו זמנית -לא תתור במקביל 
                        // itzik : CourierMaster מלבד בישות 
                        // בשלב ראשון ב CUSTOMS יעבור ל PROD בהמשך 
                        "Customs.CourierMaster" != ObjectTableRepository.GetSingleObjectTableById(requestParams.LoggingObjectTableId, requestParams.Tenant).Name
                            )
                        {
                            string CRSKey = CustomsRequestsSheetDomainModelUtil.GetCRSVirtualKey(requestParams);
                            LogMessagingUtil.Instance.AppendLine("lock in CustomsRequestsSheetDomainModelService.ConcurrentKiller():299 key: " + CRSKey);
                            var concurrentKiller = new ConcurrentKiller();
                            concurrentKiller.LockOrCrashOnCommitDueUnique(CRSKey, requestParams.Tenant);
                            bool ReleaseConcurrentKeyOn1stStep = true;
                            if (!ReleaseConcurrentKeyOn1stStep)
                            {
                                Transaction.Current.TransactionCompleted +=
                                    (sender, e) =>
                                    {
                                        if (e.Transaction.TransactionInformation.Status == TransactionStatus.Committed)
                                        {
                                            CustomsRequestsSheetDomainModelUtil.ReleaseConcurrentVirtualKey(requestParams);
                                        }
                                    };
                            }

                            string sAvoidInProgressSameInterfaceCodePerEntity = ConfigurationManager.AppSettings["20200805HD353811.AvoidInProgressSameInterfaceCodePerEntity"];
                            if (!String.IsNullOrWhiteSpace(sAvoidInProgressSameInterfaceCodePerEntity))
                            {
                                AvoidInProgressSameInterfaceCodePerEntity(requestParams, reqSheetDetails);
                            }

                        }
                    }
                }
                var listRequestInProgress = _CustomsRequestsSheetQueryService.GetRequestInProgress(
                    requestParams.Tenant, requestParams.InterfaceTypeCode,
                reqSheetDetails.ObjectTableId1, reqSheetDetails.EntityId1,
                reqSheetDetails.ObjectTableId2, reqSheetDetails.EntityId2,
                reqSheetDetails.CustomFileNo);
                if (listRequestInProgress != null)
                {
                    if (requestParams.SplitterModeLetCreateMyType)
                    {
                        listRequestInProgress = listRequestInProgress.Where(r => r.InterfaceTypeCode != requestParams.InterfaceTypeCode).ToList();
                    }

                    if (listRequestInProgress.Count > 0)
                    {
                        if(listRequestInProgress.Any(r => r.InterfaceTypeCode == "DCAOCR") && requestParams.FutureSendDateTime.HasValue)
                        {
                            return;
                        }
                        var RequestInProgressInterfaceTypeName = listRequestInProgress.First().InterfaceTypeName;
                        var RequestInProgressInterfaceId = string.Join(",", listRequestInProgress.Select(request => request.Id.ToString()));

                        ThrowRequestInProgress(requestParams, RequestInProgressInterfaceTypeName, RequestInProgressInterfaceId);
                        return;
                    }
                }
            }
        }

        private void AvoidInProgressSameInterfaceCodePerEntity(TRequestParams requestParams, RequestSheetParam reqSheetDetails)
        {
            var listSameInterfaceCodePerEntity_InProgress = _CustomsRequestsSheetQueryService.SameInterfaceCodePerEntity_InProgress(
                                    requestParams.Tenant, requestParams.InterfaceTypeCode,
                                reqSheetDetails.ObjectTableId1, reqSheetDetails.EntityId1,
                                reqSheetDetails.CustomFileNo);
            if (listSameInterfaceCodePerEntity_InProgress.Count > 0)
            {
                var RequestInProgressInterfaceTypeName = listSameInterfaceCodePerEntity_InProgress.First().InterfaceTypeName;
                var RequestInProgressInterfaceId = listSameInterfaceCodePerEntity_InProgress.All(a => a.Id != null).ToString();

                ThrowRequestInProgress(requestParams, RequestInProgressInterfaceTypeName, RequestInProgressInterfaceId);
            }
        }

        private void ThrowRequestInProgress(TRequestParams requestParams, string RequestInProgressInterfaceTypeName, string RequestInProgressInterfaceId="")
        {
            var text = //TranslateTextsClass.GetTranslation("Customs.General.RequestInProgress", "", null, null, this._Tenant);
                TranslateTextsClass.Translate("Customs.General.RequestInProgress", this._Tenant);
            text = String.Format(text, RequestInProgressInterfaceTypeName);
            NoteClientNoRequestSheet4U(requestParams, text);
             var ex = new CustomsRequestsSheetDomainModelServiceException(
            CustomsRequestsSheetDomainModelServiceException.WhereEnum.SameRequestInProgress, CustomsRequestsSheetDomainModelServiceException.What2DoEnum.StopQueue,
                text,
                null);
            ex.SuppressExceptionTostring = true;
            ex.CustomsRequestsSheetId = RequestInProgressInterfaceId;
            throw ex;
        }

        private bool AvoidSign(TRequestParams requestParams)
        {
            try
            {
                //INSERT INTO "TOGGLES" (CODE, NAME, SEARCHFIELDS) VALUES ('FSN', 'Force Sign', 'Force Sign')
                //INSERT INTO "FEATURETOGGLES"(ID, TENANT, CREATEDATE, CREATEDBYUSERID, UPDATEDATE, UPDATEDBYUSERID, SEARCHFIELDS, TENANTNUMBER, INACTIVE, TOGGLECODE)
                //VALUES('FSN_3', '3', TO_TIMESTAMP('2022-06-01 14:19:28.729000000', 'YYYY-MM-DD HH24:MI:SS.FF'), '1-9', TO_TIMESTAMP('2022-06-01 14:19:46.456000000', 'YYYY-MM-DD HH24:MI:SS.FF'), '1-9', 'FSN', '3', '0', 'FSN')
                //if (Server.Tools.Helpers.FeatureToggleHelper.HasFeatureToggle("FSN", requestParams.Tenant))
                //{
                //    return false;
                //}
                FeatureQuery featureQuery = new FeatureQuery(requestParams.Tenant);


                //SHOULD BE - 
                //var sw = Stopwatch.StartNew();
                //bool exist = ProxyUtil.SecurityUtilityCheckFeature("Customs.Declaration", "EscapeSign", requestParams.Tenant);
                //Debug.WriteLine($"SecurityUtilityCheckFeature({sw.Elapsed})");
                //if (exist)
                //{
                //    return true;
                //}

                //var features = featureQuery.GetAllowedFeaturesForLoggedUser(AuthenticationUtil.ResolveUserId(requestParams.Tenant), requestParams.Tenant);
                //var feature = features.Features.FirstOrDefault(x => x.Code == "EscapeSign");
                //if (feature != null)
                //{
                //    return true;
                //}
                
                if (requestParams.MainInterfaceCode == "2715" //D_NG_2715_MSG22002_AddAGlobalScannedAttachmentToEntityMessagingService
                &&
                String.IsNullOrWhiteSpace(requestParams.LoggingEntityId) & string.IsNullOrWhiteSpace(requestParams.LoggingObjectTableId) &&
                CustomsSettingQueryService.GetSettingByTenant(requestParams.Tenant).CompanyType == "B")//Courier
                {
                    return true;//in courier CompanyType -AvoidSign
                }
                if (!String.IsNullOrWhiteSpace(requestParams.LoggingEntityId) & !string.IsNullOrWhiteSpace(requestParams.LoggingObjectTableId))
                {
                    DefaultValueQueryService defaultValueQueryService = new DefaultValueQueryService(_Tenant);

                    string defValue = defaultValueQueryService.GetDefault("ISRAEL", "CGO_HIGH_VALUE", "NON", "NON", _Tenant);
                    decimal defaultAmount = 0;
                    var boolvar = (decimal.TryParse(defValue, out defaultAmount));
                    if (requestParams.LoggingObjectTableId == ObjectTableRepository.GetObjectTableByName("Customs.Declaration"))
                    {

                   
                        {
                            var declarationQueryService = new DeclarationQueryService(_Tenant);
                            var declaration = declarationQueryService.GetSingle(RequestParams.LoggingEntityId, false, false);

                            if (declaration != null && declaration.IsCourierDeclaration)
                            {
                                DeclarationCourierStatusQueryService declarationCourierStatusQueryService = new DeclarationCourierStatusQueryService(_Tenant);
                                DeclarationCourierStatusPM myDeclarationCourierStatusPM = declarationCourierStatusQueryService.GetSingle(declaration.Id, true, false);
                                if (myDeclarationCourierStatusPM.TotalInvoiceAmountInUSD > defaultAmount)
                                {
                                 }
                                else
                                {
                                     LogMessagingUtil.Instance.AppendLine($"{defaultAmount} בלדרות ביטול חתימה במסרים - סך חשבון בהצהרה בדולרים   {myDeclarationCourierStatusPM.TotalInvoiceAmountInUSD.GetValueOrDefault()} קטן מהגדרת המינימום");
                                    return true;
                                }
                            }



                        }

                        return false;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
                return false;
            }
            catch (Exception)
            {

                return false;
            }
        }

        private static void NoteClientNoRequestSheet4U(TRequestParams requestParams, string text)
        {
            Simplog.Server.Infrastructure.Helpers.CacheManager.CacheWrapper
                .Insert("Customs.General.RequestInProgressNoteClient," + requestParams.PBId, text);
        }
      
        public string GetAvailableSignServer(out string personId, out SignQueueByType SignatureBy, out string noAvailableSignServerErrorText,
            string OverrideSignStepName = null)
        {
            string availableSignServer = null;
            noAvailableSignServerErrorText = personId = "";

            SignatureBy = //SignQueue.GetSignatureBy(_RequestParams.InterfaceTypeCode, _RequestParams.ForcePersonalSign);
                    this.CalcSignByFromStep(OverrideSignStepName);
            personId = SignQueue.Instance.GetUserPersonID(_RequestParams.LoggingUserId, _RequestParams.Tenant);

            SignMethodByQueueEnum signMethodByQueueEnum = SignMethodByQueueEnum.None;
            string customsAgentId = SignQueue.GetCustomsAgentIdFromTenant(_RequestParams.Tenant);
            var dbSignQueueService = new SignQueueHybridDbService();

            var isExport = SignQueueHybridDbService.IsCloudExport(_RequestParams.Tenant);
            var signQueueHSMService = new SignQueueHSMService();
            
            if (string.IsNullOrWhiteSpace(availableSignServer) &&
                 (isExport ||
                signQueueHSMService.IsHSMSign_IsOn(_RequestParams.Tenant)) )
 
            {
                (availableSignServer, signMethodByQueueEnum) = dbSignQueueService
                    .GetAvailableSignServer(_RequestParams.Tenant, SignatureBy, personId, isExport);
                if (availableSignServer != null)
                {
                    if (signMethodByQueueEnum == SignMethodByQueueEnum.HybridDbSignQueue)
                    {
                        RequestParams.RequestVIA = SendRequestVIA.WebServiceBatch;
                        RequestParams.RequestVIAChangeDue = ("בקשה מחוייבת חתימה ולכן תשודר ברקע");
                    }
                    RequestParams.SignMethodByQueue = signMethodByQueueEnum.ToString();//"HybridDbSignQueue";
                    RequestParams.SignByPersonalId = SignCertificateClass.GetPersonID(availableSignServer);
                    RequestParams.SignQueueByCompanyOrPersonal = SignatureBy.ToString();
                }



            }
            if (string.IsNullOrWhiteSpace(availableSignServer))
            {
                availableSignServer = SignQueue.Instance.
                    GetAvailableSignServer(_RequestParams.Tenant, SignatureBy, personId);

                RequestParams.SignMethodByQueue = SignMethodByQueueEnum.MemorySignQueue.ToString();//"MemorySignQueue";

            }
            if (string.IsNullOrWhiteSpace(availableSignServer))
            {
                noAvailableSignServerErrorText = GetNoAvailableSignServerErrorText(personId, SignatureBy, RequestParams.InterfaceTypeCode);
            }
            return availableSignServer;
        }

        public SignQueueByType CalcSignByFromStep(string OverrideSignStepName)
        {

            var signStepName = OverrideSignStepName;
            if (string.IsNullOrWhiteSpace(signStepName))
            {
                signStepName = GetSignStepName(); ;
            }
            return SignQueue.GetSignQueueByTypeFromName(signStepName);
        }

        string GetSignStepName(bool signStepNameIsMust = false)
        {
            int stepNumber = (int)CustomsStepEnum.CustomRequestSign;

            var signStep = this._CommunicationLogStepList.FirstOrDefault(rec => rec.StepNumber == stepNumber);
            if (signStep == null)
            {
                if (signStepNameIsMust)
                {
                    throw new Exception("CalcSignByFromStep() not found rec.StepNumber == (int)CustomsStepEnum.CustomRequestSign");
                }
                return null;
            }
            return signStep.Name;
        }
        void ThrowIfNoAvailablePersonalSignServer()
        {
            var customsSettingsM = CustomsSettingQueryService.GetSettingByTenant(_RequestParams.Tenant);

            if (!SignQueue.Instance.IsPasiveSignMode())
            {
                return;
            }
            if (string.IsNullOrWhiteSpace(MessageController.SignStepName))
            {
                return;
            }
            if (RequestParams.SignMethodByQueue == SignMethodByQueueEnum.HSMSignQueue.ToString()
                &&
                !string.IsNullOrWhiteSpace(RequestParams.SignByPersonalId)
                )
            {
                return;//already checked !
            }

            string personId = ""; string noAvailableSignServerErrorText = "";
            SignQueueByType signatureBy = SignQueueByType.None;


            var availableSignServer = GetAvailableSignServer(out personId, out signatureBy, out noAvailableSignServerErrorText,
                MessageController.SignStepName);

            switch (signatureBy)
            {

                case SignQueueByType.SignQueueByPersonId:
                    if (RequestParams.RequestVIA != SendRequestVIA.WebServiceInteractive && customsSettingsM?.CompanyType=="B")
                    {
                        //no need to check if have 
                        return;
                    }
                    break;
                case SignQueueByType.None:
                case SignQueueByType.SignQueueByCustomsAgentId:
                default:
                    //if (RequestParams.MainInterfaceCode=="2715")//קלוט צרופה
                    if (RequestParams.RequestVIA != SendRequestVIA.WebServiceInteractive)
                    {
                        //no need to check if have 
                        return;
                    }
                    break;
            }
            if (!string.IsNullOrWhiteSpace(availableSignServer))
            {
                return;
            }

            //if (Debugger.IsAttached)
            //{
            //    var doNotThrow = true;
            //    if (doNotThrow)
            //    {
            //        AmitalDebuggerUtil.Break(AmitalDebuggerLevel.Verbose);
            //        return;
            //    }
            //}
            NoteClientNoRequestSheet4U(RequestParams, noAvailableSignServerErrorText);
            var ex = new CustomsRequestsSheetDomainModelServiceException(
                           CustomsRequestsSheetDomainModelServiceException.WhereEnum.NoAvailableSignServer, CustomsRequestsSheetDomainModelServiceException.What2DoEnum.StopQueue,
                               noAvailableSignServerErrorText, null);
            ex.SuppressExceptionTostring = true;
            throw ex;
        }

        string GetNoAvailableSignServerErrorText(string personId, SignQueueByType SignatureBy, string InterfaceManagementCode)
        {
            switch (SignatureBy)
            {

                case SignQueueByType.SignQueueByCustomsAgentId:
                    return (
                        //"No Available Company Sign Server Intarface =" + InterfaceManagementCode
                        string.Format(
                        "לא נמצא כרטיס חתימה חברתי (מסר {0})", InterfaceManagementCode
                        )
                        );
                    break;
                case SignQueueByType.SignQueueByPersonId:
                    return (
                        //"No Available Personal Sign Server Intarface =" + InterfaceManagementCode + " Person =" + personId
                        string.Format(
                        @"לא נמצא כרטיס חתימה מתאים לת""ז {0} (מסר {1})", personId, InterfaceManagementCode

                        )
                        );
                    break;
                default:
                    throw new Exception(" GetNoAvailableSignServerErrorText() error logic = not expected !!!");
                    break;
            }


        }
        private bool ToCreateQ()
        {
            if (!this.IsInteractive)
            {
                return false;
            }

            switch (this.GetRequestParams<TRequestParams>().RequestVIA)
            {
                case Logitude.CustomsMessaging.Common.RequestParams.SendRequestVIA.WebServiceInteractive:
                    return false;
                    break;
                case Logitude.CustomsMessaging.Common.RequestParams.SendRequestVIA.DCABatch:
                case Logitude.CustomsMessaging.Common.RequestParams.SendRequestVIA.WebServiceBatch:
                    //CreateSBQMessage(); 
                    return true;
                    break;


                case Logitude.CustomsMessaging.Common.RequestParams.SendRequestVIA.Default:
                default:
                    //if (_CustomsRequestsSheetService.GetInteractiveMode() == Logitude.Customs.BL.EntityPMs.IIGMessagePM.InteractiveMode.DCA)
                    switch (this.GetInteractiveMode())
                    {

                        case InteractiveMode.WebServiceInteractive:
                            return false;
                            break;
                        case InteractiveMode.WebServiceBatch:
                            return true;
                            break;
                        case InteractiveMode.DCABatchOutIn:
                            return true;
                            break;
                        case InteractiveMode.DCABatchIn:
                            break;
                            throw new Exception("Bad definition could not be InterfaceTypePM.InteractiveMode.DCABatchIn");
                        case InteractiveMode.none:
                        default:
                            throw new Exception("Bad definition _CustomsRequestsSheetService.GetInteractiveMode ");
                            break;
                    }

                    break;
            }
            return false;
        }
        private void CheckMessageInContainer(string InterfaceTypeCode, string MainInterfaceCode)
        {
            var curr = MainInterfaceCode;
            if (string.IsNullOrWhiteSpace(curr))
            {
                curr = InterfaceTypeCode;
            }
            if (!ContainerAccessor.Container.IsRegistered<IMessagingServiceInterfaceType>(curr))
            {
                throw new Exception("המסר לא מוגדר כראוי נא לפנות לעמיטל");
                //message.DeadLetter();
            }
        }

        //private void InitClientProgressBarIndicatorService()
        //{
        //    if (this._ClientProgressBarIndicatorService != null) return;
        //    this._ClientProgressBarIndicatorService = new ClientProgressBarIndicatorService(this._RequestParams);
        //    if (this.MessageController.CalcRequestVIA == SendRequestVIA.WebServiceInteractive)
        //    {
        //        this._ClientProgressBarIndicatorService.StartBroadcast(//this._CommunicationLogStepList
        //            "Creating Request Sheet"
        //            );

        //    }


        //}





        private void OnCreateSetDefault()
        {
            //if (String.IsNullOrWhiteSpace(_CommunicationLog.Subject))
            //{
            //    _CommunicationLog.Subject = this.InterfaceTenantDefinitionManagement.InterfaceManagement.Description;
            //}
            //this.CustomsRequestsSheet.RequestDescription = _CommunicationLog.Subject;
            if (InterfaceTenantDefinitionManagement.Interactive == Def.ClosedTable.InteractiveMode.DCABatchIn)
            {
                _CommunicationLog.InOut = "I";
            }
        }




        CustomsRequestsSheetDomainModelService(string customsRequestsSheetId, int tenant, OverrideControllerModel debugModel = null,string parentId=null)
            : this(tenant, debugModel)//,requestParams.InterfaceTypeCode )
        {
            Logitude.Server.Tools.Helpers.LogMessagingUtil.Instance.AppendLine("Seed1  ");

            try
            {

                Logitude.Server.Tools.Helpers.LogMessagingUtil.Instance.AppendLine("CustomsRequestsSheetService Retrive():customsRequestsSheetId  " + customsRequestsSheetId);


                MyCustomsRequestsSheetPM = _CustomsRequestsSheetQueryService
                    .GetSingle(customsRequestsSheetId, true,
                    false //true
                    );
                if (MyCustomsRequestsSheetPM == null)
                {
                    throw new Exception("Not found customsRequestsSheetId =" + customsRequestsSheetId);
                }
                else
                {
                    CustomsRequestsSheet currCustomsRequestsSheet = null;
                  
                        var customsRequestsSheetQueryService = new CustomsRequestsSheetQueryService(tenant);
                        currCustomsRequestsSheet = customsRequestsSheetQueryService.GetTenantPriorityByEntityID(parentId, tenant);
                    if (currCustomsRequestsSheet != null)
                    {
                        MyCustomsRequestsSheetPM.TenantPriority = currCustomsRequestsSheet.TenantPriority;
                    }
                    
                }
                if (MyCustomsRequestsSheetPM.RequestStatusEnum == SheetStatusEnum.Analyzed)
                {
                    //if (!debugModel)
                    if (debugModel == null)
                    {
                        throw new Exception("The CustomsRequestsSheet already Analyzed ");
                    }
                }
                ////InitMessageDefinition();
                _CommunicationLog = _CommunicationLogRepository.GetSingleCommunicationLog(this.MyCustomsRequestsSheetPM.RequestComminicationId, this.MyCustomsRequestsSheetPM.Tenant);
                if (_CommunicationLog.CommunicationStatusTypeCode
                    != CommStatusEnum.W.ToString())
                {
                    ///throw new Exception("The CommunicationStatus isn't wait ");
                    Logitude.Server.Tools.Helpers.LogMessagingUtil.Instance.AppendLine("Warning The CommunicationStatus is " + _CommunicationLog.CommunicationStatusTypeCode + " <> expected waiting  (אפשר שהשליחה נכשלה רק לכאורה )");
                }

                this._CommunicationLogStepList = _CommunicationLogStepRepository.GetMultiCommunicationLog(this._CommunicationLog.Id, this._CommunicationLog.Tenant)
                    .OrderBy(rec => rec.StepNumber).ToList();


                //this._RequestParams = reqParams;
                this.DeSerializeRequestParams//<TRequestParams>
                    ();
                InitMessageDefinition();// 
                this.StartCustomsRequestStepEnum = this.GetCurrentProcessState();
                Logitude.Server.Tools.Helpers.LogMessagingUtil.Instance.AppendLine("retieve success CustomsRequestsSheetService:" + this.MyCustomsRequestsSheetPM.Id);
                RequestSheetContext.Current.SetRSContext(RequestParams);

            }
            catch (CustomsRequestsSheetDomainModelServiceException)
            {
                throw;
            }
            catch (Exception e)
            {

                var MyCustomsRequestsSheetPMId = "";
                try
                {
                    MyCustomsRequestsSheetPMId = this.MyCustomsRequestsSheetPM?.Id;
                    MyCustomsRequestsSheetPMId = MyCustomsRequestsSheetPMId ?? RequestSheetContext.Current.GetContextOrDefault().CustomsRequestsSheetId;
                }
                catch (Exception)
                {

                    throw;
                }



                try //if Blob service failed try to update step !!
                {
                    EndStepWithoutTransactionScope(null, CommStatusEnum.F, e.ToString());
                }
                catch (Exception)
                {


                }


                throw new
                    CustomsRequestsSheetDomainModelServiceException(
                    CustomsRequestsSheetDomainModelServiceException.WhereEnum.CustomsRequestsSheetServiceException, CustomsRequestsSheetDomainModelServiceException.What2DoEnum.StopQueue,
                     "Seed<TRequestParams>() Crash See inner Exception:MyCustomsRequestsSheetPMId=" + MyCustomsRequestsSheetPMId, e);
            }
        }


        public static void
           CreateNew(TRequestParams requestParams, RequestSheetParam reqSheetDetails, bool isInteractive, out CustomsRequestsSheetDomainModelService<TRequestParams> customsRequestsSheetService)
        {

            customsRequestsSheetService = new CustomsRequestsSheetDomainModelService<TRequestParams>(requestParams, reqSheetDetails, isInteractive);
        }

        public static void
            Seed(string customsRequestsSheetId, int tenant, TRequestParams defaultRequestParamsFromCustomsResponse,
            out CustomsRequestsSheetDomainModelService<TRequestParams> customsRequestsSheetService,
            OverrideControllerModel debugModel = null,
            bool DcaReceivedCustomResponseCorrelation = false,
            RequestSheetParam reqSheetDetails = null, string parentId = null)
        /*

select * 
from Customs.CustomsRequestsSheets ,CommunicationLogs ,CommunicationLogSteps
where 
Customs.CustomsRequestsSheets.Id='1-8' and 
CommunicationLogs.Id=Customs.CustomsRequestsSheets.RequestComminicationId and 
CommunicationLogSteps.CommunicationLogId= CommunicationLogs.id

         */
        //   where TRequestParams : RequestParamsBase
        {
            customsRequestsSheetService = null;
            try
            {
                Logitude.Server.Tools.Helpers.LogMessagingUtil.Instance.AppendLine("Seed2");

                Logitude.Server.Tools.Helpers.LogMessagingUtil.Instance.AppendLine("CustomsRequestsSheetService Retrive():customsRequestsSheetId  " + customsRequestsSheetId);

                customsRequestsSheetService = new CustomsRequestsSheetDomainModelService<TRequestParams>(tenant);
                customsRequestsSheetService.MyOverrideControllerModel = debugModel;
                if (customsRequestsSheetService != null && customsRequestsSheetService.MyOverrideControllerModel != null)
                {
                    Logitude.Server.Tools.Helpers.LogMessagingUtil.Instance.Append("customsRequestsSheetService.DebugMode= " + customsRequestsSheetService.MyOverrideControllerModel.ToString());
                }
                customsRequestsSheetService.MyCustomsRequestsSheetPM = customsRequestsSheetService._CustomsRequestsSheetQueryService
                    .GetSingle(customsRequestsSheetId, true,
                    false //true
                    );
                if (customsRequestsSheetService.MyCustomsRequestsSheetPM != null)
                {                   
                    Logitude.Server.Tools.Helpers.LogMessagingUtil.Instance.Append("Is DCA Callback ???found customsRequestsSheetService.MyCustomsRequestsSheetPM");
                    if (customsRequestsSheetService.MyCustomsRequestsSheetPM.Tenant != tenant)
                    {
                        Logitude.Server.Tools.Helpers.LogMessagingUtil.Instance.Append("MyCustomsRequestsSheetPM.Tenant=")
                            .Append(customsRequestsSheetService.MyCustomsRequestsSheetPM.Tenant)
                            .Append("  in difrent tenant=").Append(tenant)
                            .AppendLine(" Due that Create new Sheet !!!");
                        customsRequestsSheetService.MyCustomsRequestsSheetPM = null;
                    }
                    else
                    {

                        switch (customsRequestsSheetService.MyCustomsRequestsSheetPM.RequestStatusEnum)
                        {
                            case SheetStatusEnum.Analyzed:
                            case SheetStatusEnum.Cancelled:
                                Logitude.Server.Tools.Helpers.LogMessagingUtil.Instance.Append("MyCustomsRequestsSheetPM.RequestStatusEnum=")
                            .Append(customsRequestsSheetService.MyCustomsRequestsSheetPM.RequestStatusEnum.ToString());

                                if (//customsRequestsSheetService.MyCustomsRequestsSheetPM.RequestStatusEnum== SheetStatusEnum.Analyzed  &&
                                    debugModel != null)
                                {
                                    Logitude.Server.Tools.Helpers.LogMessagingUtil.Instance
                                        .AppendLine("debugMode .. ");

                                    if (debugModel.SelectedDCAFileDebugCreateNew)
                                    {
                                        Logitude.Server.Tools.Helpers.LogMessagingUtil.Instance
                                        .AppendLine(" Due that Create new Sheet !!!"); ;
                                        customsRequestsSheetService.MyCustomsRequestsSheetPM = null;
                                    }
                                }
                                else if (DcaReceivedCustomResponseCorrelation)
                                {
                                    Logitude.Server.Tools.Helpers.LogMessagingUtil.Instance.Append(@"Stage DcaReceivedCustomResponseCorrelation allowed Only 2 Set Response Sorry -
After that Remove file  from DCA  .. ");

                                    var ex = new CustomsRequestsSheetDomainModelServiceException(CustomsRequestsSheetDomainModelServiceException.WhereEnum.RequestCancelled, CustomsRequestsSheetDomainModelServiceException.What2DoEnum.StopQueueAddLog, "", null);
                                    ex.CustomsRequestsSheetId = customsRequestsSheetId;
                                    ex.Tenant = tenant;
                                    throw ex;
                                }
                                else
                                {
                                    var sts = customsRequestsSheetService.MyCustomsRequestsSheetPM.RequestStatusEnum.ToString();
                                    customsRequestsSheetService.MyCustomsRequestsSheetPM = null;
                                    throw new Exception("End end due Analyzed/Cancelled" + sts);
                                }

                                break;
                            default:
                                break;
                        }
                    }

                }
                if (customsRequestsSheetService.MyCustomsRequestsSheetPM == null)
                {
                    if (defaultRequestParamsFromCustomsResponse != null)
                    {
                        defaultRequestParamsFromCustomsResponse.ParentId = parentId;
                        customsRequestsSheetService.Dispose();
                        Logitude.Server.Tools.Helpers.LogMessagingUtil.Instance.AppendLine("DCA Recived - not callback  ");
                        //var reqSheetDetials = CustomsRequestsSheetService<TRequestParams>.GetSheetDetailsFromRequestParam(defaultRequestParamsFromCustomsResponse);
                        CreateNew(defaultRequestParamsFromCustomsResponse, reqSheetDetails, false, out customsRequestsSheetService);
                        return;
                    }
                    var mess = "Not exist customsRequestsSheetId:" + customsRequestsSheetId;
                    Logitude.Server.Tools.Helpers.LogMessagingUtil.Instance.AppendLine(mess);
                    throw new Exception(mess);
                }
                if (defaultRequestParamsFromCustomsResponse != null)
                {
                    Logitude.Server.Tools.Helpers.LogMessagingUtil.Instance.AppendLine("DCA Return - Callback  ");
                }
                customsRequestsSheetService = new CustomsRequestsSheetDomainModelService<TRequestParams>(customsRequestsSheetId, tenant, debugModel,parentId);
                Logitude.Server.Tools.Helpers.LogMessagingUtil.Instance.AppendLine("retieve success CustomsRequestsSheetService:" + customsRequestsSheetService.MyCustomsRequestsSheetPM.Id);
                if(parentId != null&& customsRequestsSheetService?.MyCustomsRequestsSheetPM?.InterfaceTypeCode=="2715"&& customsRequestsSheetService?.MyCustomsRequestsSheetPM?.TenantPriority>0)
                {
                    ICustomContext _CustomContext = CustomContext.GetContext(tenant);

                    CustomsRequestsSheetUpdateService _CustomsRequestsSheetUpdateService= new CustomsRequestsSheetUpdateService(_CustomContext, new Dictionary<string, IContext>(), tenant);
                    customsRequestsSheetService.MyCustomsRequestsSheetPM.ChangeSetOp = ChangeSetOperation.Update;
                    _CustomsRequestsSheetUpdateService.Update(customsRequestsSheetService.MyCustomsRequestsSheetPM, true);
                }
              
              

            }
            catch (CustomsRequestsSheetDomainModelServiceException)
            {

                throw;
            }
            catch (Exception e)
            {
                var exMessage = "customsRequestsSheetId= " + customsRequestsSheetId + " tenant= " + tenant;
                ExceptionHandler.HandleException(e, DateTime.Now, 0, "", "CustomsRequestsSheetDomainModelService", "Seed(" + exMessage + ") Method", null);

                throw new
                    CustomsRequestsSheetDomainModelServiceException(
                    CustomsRequestsSheetDomainModelServiceException.WhereEnum.CustomsRequestsSheetServiceException, CustomsRequestsSheetDomainModelServiceException.What2DoEnum.StopQueue,
                     "Seed<TRequestParams>() Crash See inner Exception", e);
            }

        }

        private static void CheckRequestParamsBase(RequestParamsBase requestParams)
        {
            if (string.IsNullOrWhiteSpace(requestParams.PBId))
            {
                throw new Exception("requestParams.PBId is must");
            }

            if (string.IsNullOrWhiteSpace(requestParams.InterfaceTypeCode))
            {
                throw new Exception("requestParams.InterfaceTypeCode is must");
            }
            if (requestParams.Tenant == 0)
            {
                throw new Exception("requestParams.Tenant ==0");
            }
            if (string.IsNullOrWhiteSpace(requestParams.LoggingUserId))
            {
                requestParams.LoggingUserId = AuthenticationUtil.ResolveUserId(requestParams.Tenant);
            }
            if (string.IsNullOrWhiteSpace(requestParams.LoggingUserId))
            {
                throw new Exception("LoggingUserId is must");
            }         
            if (!String.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["AvoidCreateCustomsRequestSheet"]))
            {
                var code = requestParams.MainInterfaceCode ?? "";
                var InterfaceTypeCode = requestParams.InterfaceTypeCode ?? "";
                if (code.StartsWith("U"))
                {
                    LogMessagingUtil.Instance.AppendLine("Unifreight Message is " + requestParams.MainInterfaceCode + " Suppress AvoidCreateCustomsRequestSheet");
                }
                else
                {

                    string val = ConfigurationManager.AppSettings["AvoidCreateCustomsRequestSheet"].ToString();
                    if (val == "2")
                    {
                        switch (InterfaceTypeCode)
                        {

                            case "2715":
                            case "2750":
                                {
                                    switch (requestParams.RequestVIA)
                                    {
                                        case SendRequestVIA.DCABatch:
                                            throw new Exception("מסרים בכספת למכס מושבתים עד לסיום תהליך ההסבה");
                                            break;
                                        default:
                                            break;
                                    }
                                    LogMessagingUtil.Instance.AppendLine("Unifreight Message is " + requestParams.MainInterfaceCode + " (2715,2750 ) Suppress AvoidCreateCustomsRequestSheet");
                                }
                                break;
                            default:
                                throw new Exception("רק מסר הצהרה ושליחת מסמך ניתנים לשליחה למכס  - שאר המסרים מושבתים עד לסיום תהליך ההסבה");
                                break;
                        }

                    }
                    else
                    {
                        throw new Exception("המסרים למכס מושבתים עד לסיום תהליך ההסבה");
                    }

                }

                //switch (requestParams.MainInterfaceCode)
                //{
                //    case "US2L01":
                //    case "US2L01I":
                //    case "UCTZIP":

                //        {
                //            LogMessagingUtil.Instance.AppendLine("Message is " + requestParams.MainInterfaceCode + " Suppress AvoidCreateCustomsRequestSheet");
                //            break;
                //        }

                //    default:
                //        throw new Exception("המסרים למכס מושבתים עד לסיום תהליך ההסבה");
                //        break;
                //}

            }
        }

        private static void SuppressSendIIGMessages(RequestParamsBase requestParams)
        {
            if (                
                requestParams.RequestVIA == SendRequestVIA.DCABatch &&        
                !String.IsNullOrEmpty(requestParams.DCAFileName)        
                )
            {
                return;
            }
            var customsSettingsM = CustomsSettingQueryService.GetSettingByTenant(requestParams.Tenant);
            if (
                customsSettingsM.SuppressIIGMessageFromDate.HasValue &&
                customsSettingsM.SuppressIIGMessageToDate.HasValue
                )
            {
                if (
                    customsSettingsM.SuppressIIGMessageFromDate <= DateTime.Now && 
                    DateTime.Now < customsSettingsM.SuppressIIGMessageToDate
                    )
                {


                    if (customsSettingsM.CompanyType == "B")
                    { //Courier
                        if (requestParams.FutureSendDateTime.HasValue)
                        {
                            if (requestParams.FutureSendDateTime <= customsSettingsM.SuppressIIGMessageToDate)
                            {
                                requestParams.FutureSendDateTime = customsSettingsM.SuppressIIGMessageToDate;
                            }
                        }
                        else
                        {
                            requestParams.FutureSendDateTime = customsSettingsM.SuppressIIGMessageToDate;
                        }
                        switch (requestParams.RequestVIA)
                        {
                            case SendRequestVIA.Default:
                            case SendRequestVIA.WebServiceInteractive:
                                requestParams.RequestVIA = SendRequestVIA.WebServiceBatch;
                                break;
                            case SendRequestVIA.WebServiceBatch:
                                break;
                            case SendRequestVIA.DCABatch:
                                break;
                            default:
                                break;
                        }
                        requestParams.RequestVIAChangeDue = requestParams.RequestVIAChangeDue ?? "";
                        requestParams.RequestVIAChangeDue += $"{requestParams.FutureSendDateTime} המסרים למכס מושבתים-המסר נדחה עד לסיום תהליך ההסבה";

                    }
                    else
                    {//customs
                        throw new Exception($"{customsSettingsM.SuppressIIGMessageToDate} המסרים למכס מושבתים עד לסיום תהליך ההסבה");
                    }

                }
            }
        }

        public byte[] GetBolb(CustomsStepEnum customsRequestStep)
        {
            LogMessagingUtilWR.Instance.AppendLine("GetBolb:S");
            try
            {
                if (_BlobCach.ContainsKey(customsRequestStep))
                {
                    return _BlobCach[customsRequestStep];
                }
                byte[] ArryByte = null;
                var communicationLogStep = GetCommunicationLogStep(customsRequestStep);
                if (!GetBlob(communicationLogStep.Tenant, communicationLogStep.Document, out ArryByte))
                {
                    throw new Exception("GetBlob(" + communicationLogStep.Document.GetBlobUrl("") + ") not found");
                }
                _BlobCach.Add(customsRequestStep, ArryByte);
                return ArryByte;
            }
            finally
            {
                LogMessagingUtilWR.Instance.AppendLine("GetBolb:E");
            }
        }
        public void UpdateBolb(CustomsStepEnum customsRequestStep, Func<MemoryStream, MemoryStream> funcManupliateMemoryStream)
        {

            byte[] ArryByte = null;
            var communicationLogStep = GetCommunicationLogStep(customsRequestStep);
            if (!GetBlob(communicationLogStep.Tenant, communicationLogStep.Document, out ArryByte))
            {
                throw new Exception("GetBlob(" + communicationLogStep.Document.GetBlobUrl("") + ") not found");
            }
            var updatedMemoryStream = funcManupliateMemoryStream(new MemoryStream(ArryByte));
            SetBlob(updatedMemoryStream, communicationLogStep.Document);
            updatedMemoryStream.Position = 0;
            if (_BlobCach.ContainsKey(customsRequestStep))
            {
                _BlobCach[customsRequestStep] = updatedMemoryStream.ToArray();
            }
            else
            {
                _BlobCach.Add(customsRequestStep, updatedMemoryStream.ToArray());
            }


        }
        public static bool GetBlob(int tenant, Document document, out byte[] ArryByte)
        {
            ArryByte = null;
            var stopwatch = Stopwatch.StartNew();

            string filePath;
            filePath = document.GetBlobUrl(""); //GetBlobUrl(tenant, document, documentSufix);
            IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
            LogMessagingUtil.Instance.AppendLine("Try Read Bolb :" + filePath);

            BlobFileInfo fileInfo = new BlobFileInfo()
            {
                FileName = document.Id,
                FolderName = document.Folder,
                Extension = document.Extension,
                Tenant = tenant,
                FileSize = document.FileSize,

            };
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
        public CustomsRequestsSheetPM MyCustomsRequestsSheetPM
        {
            get { return _MyCustomsRequestsSheetPM; }
            private set { _MyCustomsRequestsSheetPM = value; }
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



        public void StartStep(CustomsStepEnum customsRequestStep, Nullable<CustomsCommandEnum> customsCommand, DateTime? StartAt = null)
        {


            if (_MyCustomsRequestsSheetPM.RequestStatusEnum == SheetStatusEnum.Cancelled ||
                       GetAccurateRequestStatusEnum() == SheetStatusEnum.Cancelled
                ) //  RequestStatusCode
            {
                throw new CustomsRequestsSheetDomainModelServiceException(
                        CustomsRequestsSheetDomainModelServiceException.WhereEnum.RequestCancelled,
                        CustomsRequestsSheetDomainModelServiceException.What2DoEnum.StopQueue,
                        @"Cancelled Cancelled !! can not StartStep()_CurrentCustomsCommandWR.Value != " + customsCommand.ToString(),
                        null);
            }

            if (CurrentWR.HasValue && customsCommand.HasValue && CurrentWR.Value != customsCommand)
            {

                if (!RequestParams.SuppressSplitWR)
                {
                    new CustomsRequestsSheetDomainModelServiceException(
                        CustomsRequestsSheetDomainModelServiceException.WhereEnum.MessageServiceException,
                        CustomsRequestsSheetDomainModelServiceException.What2DoEnum.StopQueue,
                        @"can not StartStep()_CurrentCustomsCommandWR.Value != " + customsCommand.ToString(),
                        null);
                }
                else
                {
                    //LogMessagingUtil.Instance.AppendLine("Not InBackgroundSplitWR but CurrentWR.HasValue  ???");
                    LogMessagingUtil.Instance.AppendLine("SuppressSplitWR but CurrentWR.HasValue  ???");

                    //if (Debugger.IsAttached) Debugger.Break();
                }
            }
            LogMessagingUtil.Instance.AppendLine(ProcessDetails());
            _StartCustomsCommand = customsCommand;
            _CurrentCustomsRequestStepEnum = customsRequestStep;
            //if (_ClientProgressBarIndicatorService != null)
            {
                var mess = "";
                //switch (customsRequestStep)
                //{
                //    case CustomsStepEnum.CustomRequest:
                //        mess = "מכין בקשה";
                //        break;
                //    case CustomsStepEnum.CustomRequestSign:
                //        mess = "חותם על הבקשה";
                //        break;
                //    case CustomsStepEnum.DCAInProgressUploading:
                //        break;
                //    case CustomsStepEnum.DCAInProgressUploaded:

                //        break;
                //    case CustomsStepEnum.ReceivedCustomResponseCorrelation:
                //        mess =
                //            "ממתין לתשובת המכס";
                //            //"שולח בקשה";
                //        break;
                //    case CustomsStepEnum.AnalyzeResponseData:
                //        mess = "מנתח תשובה";
                //        break;
                //    case CustomsStepEnum.StartRequestParams:
                //    default:
                //        mess = "בונה בקשה";
                //        break;
                //}
                mess = ResponseDataBase.GetCustomsRequestStepText(customsRequestStep);
                UpsertClientProgressBarIndicatorCurrentStage(mess);
                //_ClientProgressBarIndicatorService.StartStep(mess);
            }

            if (StartAt != null)
            {
                _StartStepAt = StartAt.Value;
            }
            else
            {
                _StartStepAt = TenantServerConfigration.GetCurrentDateTime(_Tenant);//DateTime.Now;20150909
            }
            UpdateCurrentStep2InProgress();

        }


        public DateTime? GetCurrentStepStartAt()
        {
            if (!InProgressFeatureIsOn)
            {
                return null;
            }
            CommunicationLogStep communicationLogStep = GetCommunicationLogStep();
            if (communicationLogStep != null)
            {
                return communicationLogStep.StartDate;
            }
            return null;

        }
        public void UpdateCurrentStep2InProgress()
        {
            if (!InProgressFeatureIsOn)
            {
                return;
            }

            CommunicationLogStep communicationLogStep = GetCommunicationLogStep();
            using (TransactionScope scope = TransactionFactory.GetTransaction())
            {

                var myCommonContext = CommonDataContext.GetContext(_Tenant);
                var myCommunicationLogStepRepository = new CommunicationLogStepRepository(myCommonContext);
                var myCommunicationLogStep = myCommunicationLogStepRepository.CommunicationLogStep(communicationLogStep.CommunicationLogId, communicationLogStep.StepNumber, communicationLogStep.Tenant);

                if (myCommunicationLogStep != null && myCommunicationLogStep.Status == CommStatusEnum.P.ToString())
                {
                    var mess = "another thread is handling , try later ";
                    LogMessagingUtil.Instance.AppendLine(mess);
                    var featureAvoidConcurency = false;
                    if (featureAvoidConcurency)
                    {
                        throw new Exception(mess);
                    }
                }
                //communicationLogStep.Status = stepStatusEnum.ToString();
                communicationLogStep.StartDate = _StartStepAt;

                communicationLogStep.Status = CommStatusEnum.P.ToString();

                _CommonContext.SaveChanges();

                scope.Complete();
            }


        }

        private string ProcessDetails()
        {
            var wr = "";
            if (CurrentWR.HasValue)
            {
                wr = CurrentWR.ToString();
            }
            else
            {
                wr = "";
            }
            return "Machine:" + Environment.MachineName + ",CurrentWR:" + wr + "," + Environment.CommandLine;
        }
        public Exception FailStepRaiseCRSSExeption(Exception ee, string defaultMessage, RequestSheetParam myRequestSheetParam, MemoryStream memstream = null
            , Action OnFailAction = null)
        {
             if (_MyCustomsRequestsSheetPM.RequestStatusEnum == SheetStatusEnum.Cancelled ||
                    GetAccurateRequestStatusEnum() == SheetStatusEnum.Cancelled
                    ) //  RequestStatusCode
            {
                throw new CustomsRequestsSheetDomainModelServiceException(
                        CustomsRequestsSheetDomainModelServiceException.WhereEnum.RequestCancelled,
                        CustomsRequestsSheetDomainModelServiceException.What2DoEnum.StopQueue,
                        @"Cancelled Cancelled Cancelled Cancelled ",
                        null);
            }
            LogMessagingUtil.Instance.AppendLine(defaultMessage + ":exceptionMessage:" + ee.ToString());
            if (ee.InnerException != null)
            {
                LogMessagingUtil.Instance.AppendLine("InnerException.Message:" + ee.InnerException.Message);
            }


            var communicationLogStep = GetCommunicationLogStep();
            bool onlyOneChanceToSend = //20180718.ConcurrentKiller
                (_CurrentCustomsRequestStepEnum == CustomsStepEnum.ReceivedCustomResponseCorrelation &&
                CustomsRequestsSheetQueryService.GetintrefaceTypeListDisplayOnly().ToList()
                .Contains(_RequestParams.InterfaceTypeCode) )||( _CurrentCustomsRequestStepEnum ==CustomsStepEnum.CustomRequest && _RequestParams.InterfaceTypeCode =="2755") ;

            if (!onlyOneChanceToSend && !this.IsInteractive && MessageController.ToRetry(_CurrentCustomsRequestStepEnum, communicationLogStep.Retries))
            {
                EndStepWithoutTransactionScope(null, CommStatusEnum.W);
                return new
                   CustomsRequestsSheetDomainModelServiceException(
                   CustomsRequestsSheetDomainModelServiceException.WhereEnum.BLException, CustomsRequestsSheetDomainModelServiceException.What2DoEnum.RetryQueue,
                    "MessageServiceException()Crash See inner Exception", ee);
            }
            else
            {
                //if (OnFailAction != null)
                //{
                //    OnFailAction();
                //}

                OnFailAction?.Invoke();
                if (myRequestSheetParam != null)//only when total fail we try to change the ReqSheet 
                {
                    this.UpdateConnectedEntitys(myRequestSheetParam);
                }
                EndStepWithoutTransactionScope(memstream, CommStatusEnum.F, defaultMessage);

                return new
                   CustomsRequestsSheetDomainModelServiceException(
                   CustomsRequestsSheetDomainModelServiceException.WhereEnum.BLException, CustomsRequestsSheetDomainModelServiceException.What2DoEnum.StopQueue,
                    "MessageServiceException()Crash See inner Exception", ee);

            }



        }
        public void SetTenantPriority(int? _TenantPriority)
        {
            MyCustomsRequestsSheetPM.TenantPriority = _TenantPriority;

        }
        public void SetCorrelationId(string _CorrelationId)
        {
            MyCustomsRequestsSheetPM.CorrelationId = _CorrelationId;

        }
        public void SetDcaAnalyzeAggregateKey(string dcaAnalyzeAggregateKey)
        {
            MyCustomsRequestsSheetPM.AnalyzeDcaAggregateKey = dcaAnalyzeAggregateKey;
        }
        public void UpdateConnectedEntitys(RequestSheetParam reqSheetDetails)
        {

            if (reqSheetDetails == null)
            {
                reqSheetDetails = new RequestSheetParam();
                //return;
            }
            List<String> myDef = new List<string>()
            {
                "3670",  //VE_MSG010_VendorInsertUpdateDeleteMessageRequestService
                "2715",  //D_NG_2715_MSG22002_AddAGlobalScannedAttachmentToEntityRequestService
                "US2L01I" // Unifreight_L2US01_US2L01_SivugMessagingService - moran 6.7.16 - Bug 21904 
            };

            var defDesc = "";
            if (this._InterfaceTenantDefinitionManagement != null && this._InterfaceTenantDefinitionManagement.InterfaceManagement != null)
            {
                if (myDef.Contains(this.RequestParams.InterfaceTypeCode) && !String.IsNullOrWhiteSpace(RequestParams.ResponseName))
                {
                    defDesc = _RequestParams.ResponseName;
                }
                else
                {
                    defDesc = this._InterfaceTenantDefinitionManagement.InterfaceManagement.Description;
                }
            }
            else
            {
               NetCommonHelper.Logger.DevLog.Instance.WriteDebug("this._InterfaceTenantDefinitionManagement  not init ???");
            }

            var currRequestDescriptionIsNullOrDef = false;
            if (String.IsNullOrWhiteSpace(MyCustomsRequestsSheetPM.RequestDescription))
            {
                currRequestDescriptionIsNullOrDef = true;
            }
            else if (defDesc == MyCustomsRequestsSheetPM.RequestDescription)
            {
                currRequestDescriptionIsNullOrDef = true;
            }
            if (!String.IsNullOrWhiteSpace(reqSheetDetails.EntityReference))
            {
                MyCustomsRequestsSheetPM.EntityReference = reqSheetDetails.EntityReference;
            }
            if (currRequestDescriptionIsNullOrDef)
            {

                var updateDesc = "";
                if (!String.IsNullOrWhiteSpace(reqSheetDetails.RequestDescription))
                {
                    updateDesc = reqSheetDetails.RequestDescription;
                    LogMessagingUtil.Instance.AppendLine("Set RequestDescription from  RequestSheetParam :" + reqSheetDetails.RequestDescription);
                }
                else
                {
                    LogMessagingUtil.Instance.AppendLine("Set InterfaceManagement.Description :" + defDesc);
                    updateDesc = defDesc;
                }
                updateDesc = updateDesc ?? "";
                if (updateDesc.Length > 120)
                {
                    LogMessagingUtil.Instance.AppendLine("RequestDescription.Substring(0, 119)!!!!!!!!!!!!!!");
                    updateDesc = updateDesc.Substring(0, 119);
                }
                _CommunicationLog.Subject = MyCustomsRequestsSheetPM.RequestDescription = updateDesc;
            }
            //if (String.IsNullOrWhiteSpace(CustomsRequestsSheet.RequestDescription))
            //{
            //    if (!String.IsNullOrWhiteSpace(reqSheetDetails.RequestDescription))
            //    {
            //        _CommunicationLog.Subject = CustomsRequestsSheet.RequestDescription = reqSheetDetails.RequestDescription;
            //    }
            //}
            if (!string.IsNullOrWhiteSpace(reqSheetDetails.CustomFileNo))
            {
                MyCustomsRequestsSheetPM.CustomFileNo = reqSheetDetails.CustomFileNo;
            }

            if (!String.IsNullOrWhiteSpace(reqSheetDetails.EntityId1) & !string.IsNullOrWhiteSpace(reqSheetDetails.ObjectTableId1))
            {
                _CommunicationLog.ObjectTableId = MyCustomsRequestsSheetPM.ObjectTableId1 = reqSheetDetails.ObjectTableId1;
                _CommunicationLog.EntityId = MyCustomsRequestsSheetPM.EntityId1 = reqSheetDetails.EntityId1;
                if (string.IsNullOrWhiteSpace(MyCustomsRequestsSheetPM.CustomFileNo))
                {
                    MyCustomsRequestsSheetPM.CustomFileNo = CustomsRequestsSheetPostUpdateService.GetCustomFileNo(MyCustomsRequestsSheetPM.Tenant, MyCustomsRequestsSheetPM.EntityId1, MyCustomsRequestsSheetPM.ObjectTableId1);
                }

            }

            if (!String.IsNullOrWhiteSpace(reqSheetDetails.EntityId2) & !string.IsNullOrWhiteSpace(reqSheetDetails.ObjectTableId2))
            {
                MyCustomsRequestsSheetPM.ObjectTableId2 = reqSheetDetails.ObjectTableId2;
                MyCustomsRequestsSheetPM.EntityId2 = reqSheetDetails.EntityId2;
                if (string.IsNullOrWhiteSpace(MyCustomsRequestsSheetPM.CustomFileNo))
                {
                    MyCustomsRequestsSheetPM.CustomFileNo = CustomsRequestsSheetPostUpdateService.GetCustomFileNo(MyCustomsRequestsSheetPM.Tenant, MyCustomsRequestsSheetPM.EntityId2, MyCustomsRequestsSheetPM.ObjectTableId2);
                }
            }

            //var requestSheetParam = new RequestSheetParam();
            //requestSheetParam.ObjectTableId1 = CustomsRequestsSheet.ObjectTableId1;
            //requestSheetParam.EntityId1 = CustomsRequestsSheet.EntityId1;
            //requestSheetParam.ObjectTableId2=   CustomsRequestsSheet.ObjectTableId2 ;
            //requestSheetParam.EntityId2=CustomsRequestsSheet.EntityId2 ;
            //requestSheetParam.RequestDescription = CustomsRequestsSheet.RequestDescription;

        }
        bool EndStepWithoutTransactionScope(MemoryStream memstream, CommStatusEnum stepStatusEnum = CommStatusEnum.D, string ExceptionMessage = null)
        {
            bool explictStopAndWrite = false;
            try
            {
                LogMessagingUtil.Instance.AppendLine("EndStepWithoutTransactionScope");

                var serverTime = TenantServerConfigration.GetCurrentDateTime(_Tenant);//DateTime.Now;20150909
                CommunicationLogStep communicationLogStep = GetCommunicationLogStep();
                communicationLogStep.Retries++;
                //communicationLogStep.Status = stepStatusEnum.ToString();
                communicationLogStep.StartDate = _StartStepAt;
                if (communicationLogStep.StartDate == DateTime.MinValue)
                {
                    communicationLogStep.StartDate = serverTime;
                }
                communicationLogStep.EndDate = serverTime;
                PerformanceM.LastInstance.RequestStartDate = communicationLogStep.StartDate;
                PerformanceM.LastInstance.RequestEndDate = communicationLogStep.EndDate;

                if (memstream != null)
                {

                    try
                    {
                        SetBlob(memstream, communicationLogStep.Document);
                        var test = false;
                        if (test)
                        {
                            var Serverbytes = GetBolb((CustomsStepEnum)communicationLogStep.StepNumber);
                            var serverXml = System.Text.Encoding.UTF8.GetString(Serverbytes.ToArray());
                        }
                    }
                    catch (Exception e)
                    {

                        LogMessagingUtil.Instance.AppendLine(e.ToString());
                        stepStatusEnum = CommStatusEnum.F;
                        explictStopAndWrite = true;

                    }

                }
                communicationLogStep.Status = stepStatusEnum.ToString();

                var sb = new StringBuilder(communicationLogStep.LogNormalized());
                sb.AppendLine(LogMessagingUtil.Instance.GetLastChars(8000));

                communicationLogStep.SetASCompressLog(sb.ToString().GetLast(8000));



                if (stepStatusEnum == CommStatusEnum.D)
                {
                    if (memstream == null)
                    {
                        throw new
               CustomsRequestsSheetDomainModelServiceException(
               CustomsRequestsSheetDomainModelServiceException.WhereEnum.CustomsRequestsSheetServiceException, CustomsRequestsSheetDomainModelServiceException.What2DoEnum.StopQueue,
                "EndStep()CommStatusEnum.D but memstream == null", null);
                    }

                    ///SetBlob(memstream, communicationLogStep.Document);
                    var requestStatusCode = GetRequestSheetStatusCodeDone(
                        _CurrentCustomsRequestStepEnum
                        //(CustomsRequestStepEnum)communicationLogStep.StepNumber
                        );
                    MyCustomsRequestsSheetPM.RequestStatusEnum = requestStatusCode;
                    if (this.DualResponseHeaderStatusReturnAckSentResponseOnDCA)
                    {
                        MyCustomsRequestsSheetPM.IsDCA = true;
                        communicationLogStep.Status = CommStatusEnum.W.ToString();
                        _CommunicationLog.CommunicationStatusTypeCode = CommStatusEnum.W.ToString();
                    }

                    if (_CurrentCustomsRequestStepEnum == CustomsStepEnum.ReceivedCustomResponseCorrelation)
                    {
                        MyCustomsRequestsSheetPM.AnswerCreateDate = TenantServerConfigration.GetCurrentDateTime(_Tenant);//DateTime.Now;20150909
                    }
                }
                else if (stepStatusEnum == CommStatusEnum.F)
                {
                    var requestStatusCode = GetRequestSheetStatusCodeFailed(
                        _CurrentCustomsRequestStepEnum
                        //(CustomsRequestStepEnum)communicationLogStep.StepNumber
                        );
                    //CustomsRequestsSheet.RequestStatusCode = ((int)requestStatusCode).ToString();       
                    MyCustomsRequestsSheetPM.RequestStatusEnum = requestStatusCode;

                }




                _CommunicationLog.LastStatusDate = serverTime;
                _CommunicationLog.Retries++;

                if (stepStatusEnum == CommStatusEnum.F)
                {
                    if (!String.IsNullOrWhiteSpace(ExceptionMessage))
                    {
                        _CommunicationLog.ExceptionMessage = ExceptionMessage;
                    }
                    _CommunicationLog.CommunicationStatusTypeCode = CommStatusEnum.F.ToString();
                    ///CustomsRequestsSheet.RequestStatusCode = CommStatusEnum.F.ToString();

                }


                if (MyCustomsRequestsSheetPM.RequestStatusEnum == SheetStatusEnum.Analyzed)
                {
                    if (_CommunicationLog.CommunicationStatusTypeCode == CommStatusEnum.D.ToString())
                    {
                        if (this.MyOverrideControllerModel == null)
                        {
                            throw new Exception("The CommunicationStatus already Done ");
                        }
                    }
                    _CommunicationLog.CommunicationStatusTypeCode = CommStatusEnum.D.ToString();
                    _CommunicationLog.DoneDate = serverTime;
                }
                if (!String.IsNullOrWhiteSpace(OnEndStepAppendLogToCommunicationLog))
                {
                    _CommunicationLog.Append2Log(OnEndStepAppendLogToCommunicationLog);
                    OnEndStepAppendLogToCommunicationLog = null;
                }
                _CommunicationLogRepository.Update(_CommunicationLog);
                MyCustomsRequestsSheetPM.ChangeSetOp = ChangeSetOperation.Update;
                _CustomsRequestsSheetUpdateService.Update(MyCustomsRequestsSheetPM, true);
                _CommonContext.SaveChanges();
                //if (_ClientProgressBarIndicatorService != null)
                //{
                //    _ClientProgressBarIndicatorService.EndStep(communicationLogStep, stepStatusEnum, communicationLogStep.Log);
                //}


                LogMessagingUtil.Instance.Clear();


            }
            catch (CustomsRequestsSheetDomainModelServiceException)
            {
                throw;
            }
            catch (Exception e)
            {

                throw new
                    CustomsRequestsSheetDomainModelServiceException(
                    CustomsRequestsSheetDomainModelServiceException.WhereEnum.CustomsRequestsSheetServiceException, CustomsRequestsSheetDomainModelServiceException.What2DoEnum.StopQueue,
                     "Seed<TRequestParams>() Crash See inner Exception", e);
            }
            return explictStopAndWrite;
        }
        public bool EndStep(MemoryStream memstream, RequestSheetParam requestSheetParam, CommStatusEnum stepStatusEnum = CommStatusEnum.D)
        {
            bool explictStopAndWrite = false;

            try
            {
                if (_MyCustomsRequestsSheetPM.RequestStatusEnum == SheetStatusEnum.Cancelled ||
                    GetAccurateRequestStatusEnum() == SheetStatusEnum.Cancelled
                    ) //  RequestStatusCode
                {
                    throw new CustomsRequestsSheetDomainModelServiceException(
                            CustomsRequestsSheetDomainModelServiceException.WhereEnum.RequestCancelled,
                            CustomsRequestsSheetDomainModelServiceException.What2DoEnum.StopQueue,
                            @"Cancelled Cancelled Cancelled Cancelled ",
                            null);
                }

                CustomsCommandEnum nxtCustomsCommandEnum = CustomsCommandEnum.CustomsCommandAnalyzeResponseWR;
                bool toContinueNextCommand = false;
                bool raiseDifferentWR = false;
                Logitude.Server.Tools.Helpers.LogMessagingUtil.Instance
.AppendLine("before : if (!this.DualResponseHeaderStatusReturnAckSentResponseOnDCA) ");
                if (!this.DualResponseHeaderStatusReturnAckSentResponseOnDCA)
                {
                    toContinueNextCommand = EndStepToContinueNextCommand(out raiseDifferentWR);
                    Logitude.Server.Tools.Helpers.LogMessagingUtil.Instance
.AppendLine("into : if (!this.DualResponseHeaderStatusReturnAckSentResponseOnDCA) ");
                    if (!toContinueNextCommand && !raiseDifferentWR)
                    {
                        nxtCustomsCommandEnum = CalcNextCommandSQ();
                        Logitude.Server.Tools.Helpers.LogMessagingUtil.Instance
                .AppendLine("Due !RequestParams.SuppressSplitWR a New Queue will Create =" + nxtCustomsCommandEnum.ToString());

                    }
                }

                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {
                    if (requestSheetParam != null)
                    {
                        LogMessagingUtil.Instance.AppendLine(" (requestSheetParam != null)");

                        this.UpdateConnectedEntitys(requestSheetParam);
                    }
                    explictStopAndWrite = EndStepWithoutTransactionScope(memstream, stepStatusEnum);
                    scope.Complete();
                }
                if (this.DualResponseHeaderStatusReturnAckSentResponseOnDCA)
                {
                    var SentResponseOnDCAexc = new CustomsRequestsSheetDomainModelServiceException(
                CustomsRequestsSheetDomainModelServiceException.WhereEnum.ReuestSheet,
                CustomsRequestsSheetDomainModelServiceException.What2DoEnum.StopQueue, "explictStopAndWrite:DualResponseHeaderStatusReturnAckSentResponseOnDCA", null);
                    throw SentResponseOnDCAexc;
                }
                if (explictStopAndWrite)
                {
                    var raiseDifferentWRexc1 = new CustomsRequestsSheetDomainModelServiceException(
                CustomsRequestsSheetDomainModelServiceException.WhereEnum.ReuestSheet,
                CustomsRequestsSheetDomainModelServiceException.What2DoEnum.StopQueue, "explictStopAndWrite", null);
                    throw raiseDifferentWRexc1;
                }
                //return EndStepToContinue(false);
                if (toContinueNextCommand)
                {
                    LogMessagingUtil.Instance.AppendLine("if (toContinueNextCommand)2");

                    return true;
                }
                if (raiseDifferentWR)
                {
                    LogMessagingUtil.Instance.AppendLine("if (raiseDifferentWR)2");

                    var raiseDifferentWRexc = new CustomsRequestsSheetDomainModelServiceException(
                CustomsRequestsSheetDomainModelServiceException.WhereEnum.ReuestSheet,
                CustomsRequestsSheetDomainModelServiceException.What2DoEnum.StopQueue, "EndStep():RequestParams.!SuppressSplitWR but CurrentWR.Value != _StartCustomsCommand ", null);
                    throw raiseDifferentWRexc;
                }
                var createSBQMessage = true;
                if (nxtCustomsCommandEnum == CustomsCommandEnum.CustomsCommandSignRequestWR)
                {
                    if (SignQueue.Instance.IsPasiveSignMode())
                    {
                        var signStepName = this.GetSignStepName(true);
                        var personId = SignQueue.Instance.GetUserPersonID(this.MyCustomsRequestsSheetPM.RequestOwnerId, this.MyCustomsRequestsSheetPM.Tenant);
                        var pmCustomsSetting = CustomsSettingQueryService.GetSettingByTenant(this.MyCustomsRequestsSheetPM.Tenant);
                        //if (string.IsNullOrEmpty(RequestParams.SignMethodByQueue))
                        //{
                        //    throw new Exception("RequestParams.SignType is must !!");
                        //}
                        SignMethodByQueueEnum signMethodBy = SignMethodByQueueEnum.MemorySignQueue;
                        if (!Enum.TryParse<SignMethodByQueueEnum>(RequestParams.SignMethodByQueue, out signMethodBy))
                        {
                            //throw new Exception("RequestParams.SignType is must !!");
                        }
                        switch (signMethodBy)
                        {
                        
                            case SignMethodByQueueEnum.HybridDbSignQueue:
 
                                {
                                    var signQueueHybridExportDBService = new CreateSignQueueHybridExportDBService();
                                    signQueueHybridExportDBService.CreateQueue(RequestParams, personId, CalcSignByFromStep(null), pmCustomsSetting.CustomsAgentId);
                                }
                                break;
                            case SignMethodByQueueEnum.HSMSignQueue:
                                {
                                    LogMessagingUtil.Instance.AppendLine("case SignMethodByQueueEnum.MemorySignQueue:" + signMethodBy);

                                    var signQueueHSMDBService = new CreateSignQueueHSMDBService();
                                    signQueueHSMDBService.CreateQueue(RequestParams, personId, CalcSignByFromStep(null), pmCustomsSetting.CustomsAgentId);
                                }

                                break;
                        
                            case SignMethodByQueueEnum.None:
                            case SignMethodByQueueEnum.MemorySignQueue:
                            default:
                                {
                                    LogMessagingUtil.Instance.AppendLine("case SignMethodByQueueEnum.MemorySignQueue:" + signMethodBy);

                                    var signQueueWebFormUrl = SignQueue.Instance
                                        .GetSignQueueWebFormUrl(
                                        _MyCustomsRequestsSheetPM.Tenant,
                                        personId,
                                        _MyCustomsRequestsSheetPM.Id, _MyCustomsRequestsSheetPM.InterfaceTypeCode, signStepName);
                                    try
                                    {

                                        Task.Run(
                                            () =>
                                            {
                                                Thread.Sleep(5000);
                                                var uri = new Uri(signQueueWebFormUrl);
                                                var client = new WebClient();
                                                client.DownloadStringCompleted += (sender, e1) =>
                                                {
                                            /// var res = e1.Result;
                                                };
                                                client.DownloadStringAsync(uri);
                                            });
                                    }
                                    catch (Exception)
                                    {

                                        //throw;
                                    }
                                }
                                break;
                        }
                        LogMessagingUtil.Instance.AppendLine("end create sign step...");

                        createSBQMessage = false;
                     }
                }
                if (createSBQMessage)
                {
                    LogMessagingUtil.Instance.AppendLine("if (createSBQMessage)");

                    SBQMessageService.CreateBasic<CustomsCommandEnum>(
                            nxtCustomsCommandEnum,
                            this.MyCustomsRequestsSheetPM.Tenant,
                            this.MyCustomsRequestsSheetPM.InterfaceTypeCode,
                            this.MyCustomsRequestsSheetPM.Id);
                }



                var NewQueueCreatedException = new CustomsRequestsSheetDomainModelServiceException(
                CustomsRequestsSheetDomainModelServiceException.WhereEnum.ReuestSheet,
                CustomsRequestsSheetDomainModelServiceException.What2DoEnum.NewQueueCreated, "", null);
                throw NewQueueCreatedException;

            }
            catch (CustomsRequestsSheetDomainModelServiceException)
            {
                throw;
            }
            catch (Exception e)
            {

                throw new
                    CustomsRequestsSheetDomainModelServiceException(
                    CustomsRequestsSheetDomainModelServiceException.WhereEnum.CustomsRequestsSheetServiceException, CustomsRequestsSheetDomainModelServiceException.What2DoEnum.StopQueue,
                     "Seed<TRequestParams>() Crash See inner Exception", e);
            }
        }

        private SheetStatusEnum GetAccurateRequestStatusEnum()
        {
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                var myCustomContext = CustomContext.GetContext(_Tenant);



                var myCustomsRequestsSheetQueryService = new CustomsRequestsSheetQueryService(myCustomContext);
                var accurateRequest = myCustomsRequestsSheetQueryService
                        .GetSingle(_MyCustomsRequestsSheetPM.Id, false, false);
                if (accurateRequest == null) return SheetStatusEnum.Created;
                return accurateRequest.RequestStatusEnum;
            }
        }


        public bool IsOnlIne()
        {
            var toContinueNextCommand = true;
            if (!CurrentWR.HasValue) return toContinueNextCommand;
            if (!_StartCustomsCommand.HasValue) return toContinueNextCommand;
            return false;
        }
        private bool EndStepToContinueNextCommand(out bool raiseDifferentWR)
        {

            raiseDifferentWR = false;
            var toContinueNextCommand = true;
            Logitude.Server.Tools.Helpers.LogMessagingUtil.Instance
                .AppendLine("if (!CurrentWR.HasValue): " + !CurrentWR.HasValue);
            if (!CurrentWR.HasValue) return toContinueNextCommand;
            Logitude.Server.Tools.Helpers.LogMessagingUtil.Instance
              .AppendLine("if (!_StartCustomsCommand.HasValue):" + _StartCustomsCommand.HasValue);
            if (!_StartCustomsCommand.HasValue) return toContinueNextCommand;



            var curVal = CurrentWR.GetValueOrDefault();
            if (!RequestParams.SuppressSplitWR)//the eblity to continue work 1 proccess without Split
            {
                Logitude.Server.Tools.Helpers.LogMessagingUtil.Instance
              .AppendLine("!RequestParams.SuppressSplitWR");

                if (CurrentWR.Value != _StartCustomsCommand)
                {
                    Logitude.Server.Tools.Helpers.LogMessagingUtil.Instance
           .AppendLine("if (CurrentWR.Value != _StartCustomsCommand) : CurrentWR.Value + "  + CurrentWR.Value + "_StartCustomsCommand:" + _StartCustomsCommand);
                    raiseDifferentWR = true;
                    //throw new BusinessErrorException("");
                    return false;

                }
                toContinueNextCommand = false;
                if (curVal == CustomsCommandEnum.CustomsCommandAnalyzeResponseWR)//End Step ,No more Steps
                {
                    Logitude.Server.Tools.Helpers.LogMessagingUtil.Instance
.AppendLine("if (curVal == CustomsCommandEnum.CustomsCommandAnalyzeResponseWR):" + curVal);

                    toContinueNextCommand = true;
                }
                else if (curVal == CustomsCommandEnum.CustomsCommandSendDCAUploadStatusWR)//end Step - Wait to dca  In
                {
                    Logitude.Server.Tools.Helpers.LogMessagingUtil.Instance
.AppendLine("else if (curVal == CustomsCommandEnum.CustomsCommandSendDCAUploadStatusWR):" + curVal);
                    toContinueNextCommand = true;
                }
            }
            return toContinueNextCommand;
        }

        private CustomsCommandEnum CalcNextCommandSQ()
        {
            var ele = GetCommunicationLogStep(_CurrentCustomsRequestStepEnum);
            var idx = this._CommunicationLogStepList.IndexOf(ele);

            idx++;
            var nxtStep = this._CommunicationLogStepList.ElementAt(idx);
            CustomsStepEnum nxtEnum = (CustomsStepEnum)nxtStep.StepNumber;

            CustomsCommandEnum nxtCustomsCommandEnum = this.MessageController.ConvertToWR(nxtEnum);

            return nxtCustomsCommandEnum;
        }

        public CustomsStepEnum CalcLastCustomsStep()
        {
            var ele = GetCommunicationLogStep(_CurrentCustomsRequestStepEnum);
            var idx = this._CommunicationLogStepList.IndexOf(ele);

            idx--;
            var nxtStep = this._CommunicationLogStepList.ElementAt(idx);
            CustomsStepEnum lastCustomsStep = (CustomsStepEnum)nxtStep.StepNumber;


            return lastCustomsStep;
        }

        private CommunicationLogStep GetCommunicationLogStep(CustomsStepEnum? customsRequestStepEnum = null)
        {
            if (customsRequestStepEnum == null)
            {
                customsRequestStepEnum = _CurrentCustomsRequestStepEnum;
            }
            var communicationLogStep = _CommunicationLogStepList.FirstOrDefault(rec => rec.StepNumber == (int)customsRequestStepEnum);
            if (communicationLogStep == null)
            {
                throw new
                    CustomsRequestsSheetDomainModelServiceException(
                    CustomsRequestsSheetDomainModelServiceException.WhereEnum.CustomsRequestsSheetServiceException, CustomsRequestsSheetDomainModelServiceException.What2DoEnum.StopQueue,
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
                    //case CustomsStepEnum.CustomRequestSignPersonal:
                    return SheetStatusEnum.SendFailed;
                    break;
                case CustomsStepEnum.ReceivedCustomResponseCorrelation:

                    if (MyCustomsRequestsSheetPM != null && MyCustomsRequestsSheetPM.IsDCA)
                    {
                        return SheetStatusEnum.ReceivedFailed;
                    }
                    else
                    {
                        return SheetStatusEnum.SendFailed;
                    }
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

        public CustomsRequestsSheetDomainModelServiceException FailSheet(Exception curException, bool cancelImmediately = false)
        {

            using (TransactionScope scope = TransactionFactory.GetTransaction())
            {
                var sb = new StringBuilder(_CommunicationLog.Logs);
                sb.AppendLine(LogMessagingUtil.Instance.ToString());
                sb.AppendLine(curException.ToString());
                _CommunicationLog.Logs = sb.ToString().GetLast((8000 - 1));
                _CommunicationLog.CommunicationStatusTypeCode = CommStatusEnum.F.ToString();
                _CommunicationLogRepository.Update(_CommunicationLog);
                _CommunicationLogRepository.SubmitChanges();
                if (cancelImmediately)
                {
                    //var requestStatusCode = GetRequestSheetStatusCodeFailed(_CurrentCustomsRequestStepEnum);
                    MyCustomsRequestsSheetPM.RequestStatusEnum = SheetStatusEnum.Cancelled;
                }
                else
                {
                    var requestStatusCode = GetRequestSheetStatusCodeFailed(_CurrentCustomsRequestStepEnum);
                    MyCustomsRequestsSheetPM.RequestStatusEnum = requestStatusCode;
                }


                MyCustomsRequestsSheetPM.ChangeSetOp = ChangeSetOperation.Update;
                _CustomsRequestsSheetUpdateService.CommLogStepCanCancelledAction = CommLogStepCanCancelled;
                _CustomsRequestsSheetUpdateService.Update(MyCustomsRequestsSheetPM, true);
                scope.Complete();
            }



            return new CustomsRequestsSheetDomainModelServiceException(
                CustomsRequestsSheetDomainModelServiceException.WhereEnum.MessageServiceException,
                CustomsRequestsSheetDomainModelServiceException.What2DoEnum.StopQueue
                    , "FailSheet" + curException.Message, curException);
        }
        public static void CommLogStepCanCancelled(CustomsRequestsSheet entityPOCO,
          CustomsRequestsSheetPM entityPM, DateTime? nowIs
          )
        {
            LogMessagingUtil.Instance.AppendLine("CustomsRequestsSheetDomainModelService:CommLogStepCanCancelled :do nothing");
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
                    if (!_RequestParams.AvoidSign)
                    {
                        //if (this._CommunicationLogStepList.Exists(rec => rec.StepNumber == (int)CustomsStepEnum.CustomRequestSign))
                        if (InterfaceTenantDefinitionManagement.InterfaceManagement.SignatureBy == SignQueueByType.SignQueueByCustomsAgentId
                            || InterfaceTenantDefinitionManagement.InterfaceManagement.SignatureBy == SignQueueByType.SignQueueByPersonId
                            || _RequestParams.ForcePersonalSign)
                        {
                            return SheetStatusEnum.WaitingForSigning;
                            break;
                        }
                    }
                    return SheetStatusEnum.InProcess;
                    break;
                //case CustomsStepEnum.CustomRequestSign:

                case CustomsStepEnum.CustomRequestSign:
                    return SheetStatusEnum.InProcess;
                    break;

                case CustomsStepEnum.DCAInProgressUploading:
                case CustomsStepEnum.DCAInProgressUploaded:
                    return SheetStatusEnum.Sent;
                case CustomsStepEnum.ReceivedCustomResponseCorrelation:
                    if (this.DualResponseHeaderStatusReturnAckSentResponseOnDCA)
                    {
                        return SheetStatusEnum.SentResponseOnDCA;
                    }
                    return SheetStatusEnum.Received;
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

            BlobFileInfo fileInfo = new BlobFileInfo()
            {
                FileName = document.Id,
                FolderName = document.Folder,
                Extension = document.Extension,
                Tenant = document.Tenant,
                FileSize = memstream.ToArray().Length,

            };
            storageservice.Write(memstream.ToArray(), fileInfo);


            stopwatch.Stop();
            LogMessagingUtil.Instance.AppendLine("SetBolb:" + filePath + "Took:" + stopwatch.Elapsed.ToString());

            //if (response.HasError)
            //{
            //    throw new Exception("BlobServiceReference.Response SetBolb: HasError =" + response.ErrorMessage);
            //}
        }
        private void BuildSteps()
        {
            if (_CommunicationLogStepList != null)
            {
                throw new Exception("BuildSteps() already done !!!");
            }
            _CommunicationLogStepList = new List<CommunicationLogStep>();

            //List<CustomsRequestStepEnum> arry = Enum.GetValues(typeof(CustomsRequestStepEnum)).OfType<CustomsRequestStepEnum>();
            //if (false)
            //{
            //    SendRequestVIA requestVIA = _RequestParams.RequestVIA;
            //    var steps = this.MessageController.BuildRealSteps(InterfaceTenantDefinitionManagement, ref requestVIA, _RequestParams.ForcePersonalSign);
            //}



            foreach (var step in MessageController.RealSteps)
            {

                var stepName = step.ToString();
                if (step == CustomsStepEnum.CustomRequestSign)
                {
                    stepName = MessageController.SignStepName;
                }
                DocumentRepository documentrepository = new DocumentRepository(_CommonContext);
                var document = new Document()
                {
                    CreateDate = TenantServerConfigration.GetCurrentDateTime(_Tenant),//DateTime.Now;20150909
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
                    Name = stepName,
                    StartDate = TenantServerConfigration.GetCurrentDateTime(_Tenant),//DateTime.Now;20150909
                    EndDate = TenantServerConfigration.GetCurrentDateTime(_Tenant),//DateTime.Now;20150909
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

            string communicationLogId = "";
            if (String.IsNullOrEmpty(communicationLogId))
            {
                communicationLogId = IdCounter.GetNumber("CommunicationLog", this._Tenant);
            }
            _CommunicationLog = new CommunicationLog()
            {
                Id = communicationLogId,
                LastStatusDate = TenantServerConfigration.GetCurrentDateTime(_Tenant),//DateTime.Now;20150909
                To = "Customs",
                InOut = "O",///TODO //requestParams.InOut,
                EntityId = RequestParams.LoggingEntityId,
                ObjectTableId = RequestParams.LoggingObjectTableId,
                Subject = GetSubjectRequestDescription(RequestParams),
                Tenant = this._Tenant,
                CommunicationLogTypeCode = "T",
                CreateDate = TenantServerConfigration.GetCurrentDateTime(_Tenant),//DateTime.Now;20150909
                CommunicationStatusTypeCode = "W",
                CreatedByUserId = RequestParams.LoggingUserId,
                //DocumentId = MyDocument.Id,
                EntityReference = RequestParams.LoggingEntityReference,
                ///CorrelationID = correlationId,
                Logs = LogMessagingUtil.Instance.ToString(8000),
                CreateDateUTC = DateTime.UtcNow,
                LastStatusDateUTC = DateTime.UtcNow,

            };

            bool AddDummydueDocumentIdIsmust = true;//Message=Cannot insert the value NULL into column 'DocumentId', table 'Amital1_Main.dbo.CommunicationLogs'; column does not allow nulls. INSERT fails.
            if (AddDummydueDocumentIdIsmust)
            {
                DocumentRepository documentrepository = new DocumentRepository(_CommonContext);

                Document document = new Document()
                {
                    CreateDate = TenantServerConfigration.GetCurrentDateTime(_Tenant),//DateTime.Now;20150909
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

        public static string GetSubjectRequestDescription(TRequestParams requestParams)
        {
            return "";
            requestParams.RequestName = requestParams.RequestName ?? "";
            requestParams.LoggingEntityId = requestParams.LoggingEntityId ?? "";
            var subject = new StringBuilder();

            subject.Append(requestParams.LoggingEntityReference);

            if (subject.Length + requestParams.LoggingEntityId.Length < 118)
            {
                subject
                    .Append(" ")
                    .Append(requestParams.LoggingEntityId);
            }

            if (subject.Length + requestParams.RequestName.Length < 118)
            {
                subject
                    .Append(" ")
                    .Append(requestParams.RequestName);
            }

            return subject.ToString();
        }

        private void CreateNewRequestSheet(TRequestParams requestParams)
        {
            if (MyCustomsRequestsSheetPM != null)
            {
                throw new Exception("customsRequestsSheetPm Already Exist ");
                throw new Exception("Please Insert customsRequestsSheetPm with override data ");
            }



            //Logitude.Server.Tools.Helpers.LogMessagingUtil.Instance.AppendLine("selectedFile =" + selectedFile);
            //string externalId = "";// GetExternalId(selectedFile);

            int? tenantPriority = null;
            CustomsRequestsSheet currCustomsRequestsSheet = null;
            if (requestParams != null)
            {
                var customsRequestsSheetQueryService = new CustomsRequestsSheetQueryService(this._Tenant);
                currCustomsRequestsSheet = customsRequestsSheetQueryService.GetTenantPriorityByEntityID(requestParams.ParentId, this._Tenant);
                tenantPriority = currCustomsRequestsSheet?.TenantPriority;
            }
            if(requestParams.TenantPriority > 0)
            {
                tenantPriority = requestParams.TenantPriority;
            }
            if (tenantPriority == null)
            {
                InterfaceTenantDefinitionQueryService interfaceTenantDefinitionQuery = new InterfaceTenantDefinitionQueryService(_CustomContext);
                InterfaceTenantDefinitionPM interfaceTenantDefinitionPM = interfaceTenantDefinitionQuery.GetInterfaceDefWithPriorityFromCacheByTenatCode(this._Tenant, requestParams.InterfaceTypeCode);
                tenantPriority = interfaceTenantDefinitionPM?.TenantPriority;
            }
            MyCustomsRequestsSheetPM = new CustomsRequestsSheetPM()
            {
                ChangeSetOp = ChangeSetOperation.Insert,
                Tenant = this._Tenant,
                RequestOwnerId = RequestParams.LoggingUserId,
                EntityId1 = RequestParams.LoggingEntityId,
                ObjectTableId1 = RequestParams.LoggingObjectTableId,
                EntityReference = RequestParams.LoggingEntityReference,
                InterfaceTypeCode = RequestParams.InterfaceTypeCode,
                RequestCreateDate = TenantServerConfigration.GetCurrentDateTime(_Tenant),//DateTime.Now;20150909
                RequestStatusEnum = SheetStatusEnum.Created,
                RequestComminicationId = _CommunicationLog.Id,
                //CustomFileNo = GetCustomFileNo(RequestParams)
                TenantPriority = tenantPriority,
                IsHSM = requestParams.SignMethodByQueue == SignMethodByQueueEnum.HSMSignQueue.ToString() ? true:false

            };

            MyCustomsRequestsSheetPM.Id = RequestParams.PBId;//GUID 
            ///InitMessageDefinition();

            if (string.IsNullOrWhiteSpace(MyCustomsRequestsSheetPM.RequestOwnerId))
            {
                MyCustomsRequestsSheetPM.RequestOwnerId = AuthenticationUtil.ResolveUnifreightUserId(this._Tenant);
            }

            MyCustomsRequestsSheetPM.IsDCA = IsDCA();
            if (MyCustomsRequestsSheetPM.IsDCA)
            {
                if (RequestParams.TransmitionDateTime.HasValue && RequestParams.TransmitionDateTime > DateTime.MinValue)
                {
                    MyCustomsRequestsSheetPM.RequestCreateDate = RequestParams.TransmitionDateTime;
                }
                if (String.IsNullOrWhiteSpace(RequestParams.DCAFileName)) ///out 
                {
                    ///throw new Exception("is dca but  RequestParams.DCAFileName is null ??!!!!");
                }
                else // IN
                {
                    var messageRestor =
                //GetSYSTBL_
                "MSG9010_9011"
                    //_MessageRestoreRequest_Out.IL941079089.2015-07-06_12-50-08-790.1eea0e31-f6ca-4fbb-a3a2-55cb8f01ed97.MR.PLT
                    ;
                    if (!RequestParams.DCAFileName.ToUpper().Contains(messageRestor.ToUpper()))
                    {
                        MyCustomsRequestsSheetPM.IsRestored = RequestParams.DCAFileName.ToUpper().Contains("." + DCAUtil.SufixRestored + ".");
                    }
                }

            }

            //var currentContext = CustomContext.GetContext(_CommunicationLog.Tenant);

            MyCustomsRequestsSheetPM.ChangeSetOp = ChangeSetOperation.Insert;
            _CustomsRequestsSheetUpdateService.Update(MyCustomsRequestsSheetPM, true);
            Logitude.Server.Tools.Helpers.LogMessagingUtil.Instance.AppendLine("Create   CustomsRequestsSheetPM:" + MyCustomsRequestsSheetPM.Id);
        }

        private bool IsDCA()
        {
            var isDCA = false;
            switch (_RequestParams.RequestVIA)
            {
                case SendRequestVIA.Default:
                    isDCA = (InterfaceTenantDefinitionManagement.Interactive == InteractiveMode.DCABatchIn ||
                InterfaceTenantDefinitionManagement.Interactive == InteractiveMode.DCABatchOutIn
                );
                    break;
                case SendRequestVIA.WebServiceInteractive:
                    break;
                case SendRequestVIA.WebServiceBatch:
                    break;
                case SendRequestVIA.DCABatch:
                    isDCA = true;
                    break;
                default:
                    break;
            }
            return isDCA;
        }

        public static string GetCustomFileNo(TRequestParams RequestParams)
        {

            return CustomsRequestsSheetPostUpdateService.GetCustomFileNo(RequestParams.Tenant, RequestParams.LoggingEntityId, RequestParams.LoggingObjectTableId);

        }



        private void InitMessageDefinition()
        {
            var interfaceTenantDefinitionQueryService = new InterfaceTenantDefinitionQueryService(_Tenant);
            string interfaceTypeCode =
                //MyCustomsRequestsSheetPM.InterfaceTypeCode;
                RequestParams.InterfaceTypeCode;
            if (interfaceTypeCode == null)
            {
                throw new Exception("InitInterfaceTypePM but CustomsRequestsSheetPM.InterfaceTypeCode == null");
            }
            _InterfaceTenantDefinitionManagement = interfaceTenantDefinitionQueryService.GetWithInterfaceManagementDefinition(_Tenant,
                interfaceTypeCode
                ).FirstOrDefault();
            //= interfaceTenantDefinitionQueryService.GetWithInterfaceManagementDefinition(_Tenant, CustomsRequestsSheet.InterfaceTypeCode);
            if (InterfaceTenantDefinitionManagement == null)
            {
                throw new Exception("Please init _IIGMessagePM ");
            }

            if (InterfaceTenantDefinitionManagement.InterfaceManagement.INOUT == InOutType.In)
            {
                var interfaceManagementQueryService = new InterfaceManagementQueryService(_Tenant);
                _OutMessageDefinition = interfaceManagementQueryService.GetOutInterfaceType(InterfaceTenantDefinitionManagement.Code);
            }

        }









        void DeSerializeRequestParams//<TRequestParams>
            ()
        //where TRequestParams : RequestParamsBase
        {

            //byte[] ArryByte = null;
            //ArryByte = customsRequestsSheetService.GetBolb(CustomsRequestStepEnum.StartRequestParams);
            //var xml = System.Text.Encoding.UTF8.GetString(ArryByte);
            //var reqParams = XmlGenericUtil<TRequestParams>.DeSerializeObject(xml);

            var myArry = this.GetBolb(CustomsStepEnum.StartRequestParams);
            string xml = Encoding.UTF8.GetString(myArry);
            if (string.IsNullOrWhiteSpace(xml))
            {
                throw new Exception("why the hell the DeSerializeRequestParams XML IsNullOrWhiteSpace ");
            }
            var requestParams = XmlGenericUtil<TRequestParams>.DeSerializeObject(xml);

            RequestParams = requestParams;
        }

        public string GetCustomsRequestXml()
        {

            var myArry = this.GetBolb(CustomsStepEnum.CustomRequest);
            string xml = Encoding.UTF8.GetString(myArry);

            return xml;
        }


        public InteractiveMode GetInteractiveMode()
        {
            if (InterfaceTenantDefinitionManagement == null)
            {
                throw new
                    CustomsRequestsSheetDomainModelServiceException(
                    CustomsRequestsSheetDomainModelServiceException.WhereEnum.CustomsRequestsSheetServiceException, CustomsRequestsSheetDomainModelServiceException.What2DoEnum.StopQueue,
                     "MessageHaveToSign _IIGMessagePM == null", null);
            }
            return InterfaceTenantDefinitionManagement.Interactive;
        }

        public CustomsStepEnum GetCurrentProcessState()
        {
            if (_CommunicationLogStepList == null)
            {
                throw new
                 CustomsRequestsSheetDomainModelServiceException(
                 CustomsRequestsSheetDomainModelServiceException.WhereEnum.CustomsRequestsSheetServiceException, CustomsRequestsSheetDomainModelServiceException.What2DoEnum.StopQueue,
                  "GetCurrentProcessState _CommunicationLogStepList == null", null);
            }

            if (MyCustomsRequestsSheetPM.RequestStatusEnum == SheetStatusEnum.SentResponseOnDCA)
            {
                return CustomsStepEnum.ReceivedCustomResponseCorrelation;
            }
            var currStep = _CommunicationLogStepList
                .Where(rec => rec.Status == CommStatusEnum.W.ToString())
                .OrderBy(rec => rec.StepNumber)
                .FirstOrDefault();
            if (InProgressFeatureIsOn)
            {
                currStep = _CommunicationLogStepList
                .Where(rec => rec.Status == CommStatusEnum.W.ToString() || rec.Status == CommStatusEnum.P.ToString())
                .OrderBy(rec => rec.StepNumber)
                .FirstOrDefault();
            }
            if (currStep == null)
            {
                if (MyCustomsRequestsSheetPM.RequestStatusEnum == SheetStatusEnum.Analyzed)
                {
                    var aggregateDCAKey = "";// _MyCustomsRequestsSheetPM.AggregateDCAKey
                    if (_MyCustomsRequestsSheetPM.IsDCA
                        && !String.IsNullOrWhiteSpace(aggregateDCAKey)
                        && !MyOverrideControllerModel.IsAggregateDCAAnalyzer)
                    {
                        throw new
                 CustomsRequestsSheetDomainModelServiceException(
                 CustomsRequestsSheetDomainModelServiceException.WhereEnum.AggregateDCAAnalyzerIsMust, CustomsRequestsSheetDomainModelServiceException.What2DoEnum.RetryQueue,
                  @"There is AggregateDCAKey ,so Controller Must be in IsAggregateDCAAnalyzer mode ", null);
                    }
                    return CustomsStepEnum.AnalyzeResponseData;
                }

                else
                {
                    if (this.MyOverrideControllerModel != null)
                    {

                        return CustomsStepEnum.AnalyzeResponseData;
                    }
                    var more = String.Format("CustomFileNo={0},MyCustomsRequestsSheetPM.Id={1}  ", MyCustomsRequestsSheetPM.CustomFileNo, MyCustomsRequestsSheetPM.Id);
                    throw new
                 CustomsRequestsSheetDomainModelServiceException(
                 CustomsRequestsSheetDomainModelServiceException.WhereEnum.CustomsRequestsSheetServiceException, CustomsRequestsSheetDomainModelServiceException.What2DoEnum.StopQueue,
                  @"Logic Error Step Must Be AnalyzeResponseData
" + more, null);
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
            var myArry = this.GetBolb(CustomsStepEnum.DCAInProgressUploaded);
            string xml = Encoding.UTF8.GetString(myArry);
            LogMessagingUtil.Instance.AppendLine("<DCAServerUploadStatus>.DeserilazeObject");
            var myDCAServerUploadStatus = XmlGenericUtil<CustomsMessaging.Common.DCAParams.DCAServerUploadStatus>.DeSerializeObject(xml);
            return myDCAServerUploadStatus;
        }

        private TRequestParams RequestParams
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
            if ((_CommonContext as DbContextBase) != null)
            {
                (_CommonContext as DbContextBase).Dispose();
            }
            _CommonContext = null;

            _CommunicationLogRepository = null;
            _CommunicationLogStepRepository = null;
            _CustomsRequestsSheetQueryService = null;

            MyCustomsRequestsSheetPM = null;
            _CommunicationLog = null;
            _CommunicationLogStepList = null;


            //TRequestParams
            _RequestParams = null;


            _InterfaceTenantDefinitionManagement = null;

            _MessageController = null;
            _CustomsRequestsSheetUpdateService = null;

        }

        //public SendRequestVIA VIA()
        //{
        //    var via = DefaultMessageController.Via(this.InterfaceTenantDefinitionManagement, this._RequestParams.RequestVIA);
        //    return via;
        //}



        public bool IsInteractive { get; private set; }
        public CustomsStepEnum StartCustomsRequestStepEnum { get; private set; }
        public InterfaceTenantDefinitionManagementPM InterfaceTenantDefinitionManagement
        {
            get
            {
                if (_InterfaceTenantDefinitionManagement == null)
                {
                    throw new
                        CustomsRequestsSheetDomainModelServiceException(
                        CustomsRequestsSheetDomainModelServiceException.WhereEnum.CustomsRequestsSheetServiceException, CustomsRequestsSheetDomainModelServiceException.What2DoEnum.StopQueue,
                         "MessageHaveToSign _IIGMessagePM == null", null);
                }
                return _InterfaceTenantDefinitionManagement;
            }
            //private set { _MessageDefinition = value; }
        }

        public InterfaceManagementPM MainMessageDefinition
        {
            get { return _OutMessageDefinition; }

        }











        public static RequestSheetParam GetSheetDetailsFromRequestParam(TRequestParams requestParams)
        {
            var my = new Logitude.CustomsMessaging.Common.RequestParams.RequestSheetParam();
            my.ObjectTableId1 = requestParams.LoggingObjectTableId;
            my.EntityId1 = requestParams.LoggingEntityId;
            my.ObjectTableId2 = requestParams.LoggingObjectTableId2;
            my.EntityId2 = requestParams.LoggingEntityId2;
            my.RequestDescription = CustomsRequestsSheetDomainModelService<TRequestParams>.GetSubjectRequestDescription(requestParams);
            my.CustomFileNo = CustomsRequestsSheetDomainModelService<TRequestParams>.GetCustomFileNo(requestParams);
            return my;
        }

        public Nullable<CustomsCommandEnum> CurrentWR { get; set; }





        public static void UpdateMainEntity(int Tenant, string customsRequestsSheetId, string objectTableId, string entityId)
        {

            if (string.IsNullOrWhiteSpace(customsRequestsSheetId) ||
                string.IsNullOrWhiteSpace(objectTableId) ||
                string.IsNullOrWhiteSpace(entityId))
            {
                return;
            }

            ICustomContext dbContext = CustomContext.GetContext(Tenant);
            var updateService = new CustomsRequestsSheetUpdateService(dbContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), Tenant);
            var queryService = new CustomsRequestsSheetQueryService(dbContext);
            var customsRequestsSheet = queryService.GetSingle(customsRequestsSheetId, false, false);
            if (customsRequestsSheet == null)
            {
                return;
            }

        }





        public void UpsertClientProgressBarIndicatorCurrentStage(string mess, Func<string> GetMess = null)
        {
            if (CurrentWR.HasValue) return;
            if (this.RequestParams == null) return;
            if (GetMess != null) mess = GetMess();
            var PBId = this.RequestParams.PBId;
            ClientProgressBarIndicatorService.UpsertClientProgressBarIndicatorCurrentStage(PBId, mess);
        }


        public bool InBatchModeToCreateQ()
        {


            switch (GetRequestParams<TRequestParams>().RequestVIA)
            {
                case Logitude.CustomsMessaging.Common.RequestParams.SendRequestVIA.WebServiceInteractive:
                    return false;
                    break;
                case Logitude.CustomsMessaging.Common.RequestParams.SendRequestVIA.DCABatch:
                case Logitude.CustomsMessaging.Common.RequestParams.SendRequestVIA.WebServiceBatch:
                    //CreateSBQMessage(); 
                    return true;
                    break;


                case Logitude.CustomsMessaging.Common.RequestParams.SendRequestVIA.Default:
                default:
                    //if (_CustomsRequestsSheetService.GetInteractiveMode() == Logitude.Customs.BL.EntityPMs.IIGMessagePM.InteractiveMode.DCA)
                    switch (GetInteractiveMode())
                    {

                        case InteractiveMode.WebServiceInteractive:
                            return false;
                            break;
                        case InteractiveMode.WebServiceBatch:
                            return true;
                            break;
                        case InteractiveMode.DCABatchOutIn:
                            return true;
                            break;
                        case InteractiveMode.DCABatchIn:
                            break;
                            throw new Exception("Bad definition could not be InterfaceTypePM.InteractiveMode.DCABatchIn");
                        case InteractiveMode.none:
                        default:
                            throw new Exception("Bad definition _CustomsRequestsSheetService.GetInteractiveMode ");
                            break;
                    }

                    break;
            }
            return false;
        }
        public OverrideControllerModel MyOverrideControllerModel { get; set; }

        public void SetIsInteractive()
        {
            IsInteractive = true;
        }

        public bool DualResponseHeaderStatusReturnAckSentResponseOnDCA { get; set; }



        public string OnEndStepAppendLogToCommunicationLog { get; set; }
        public const bool InProgressFeatureIsOn = true;

        public void ReAnalyzeStatusReceivedCreateQ(DateTime? futureSendDateTime = null)
        {

            CommunicationLogStep communicationLogStep = GetCommunicationLogStep(CustomsStepEnum.AnalyzeResponseData);
            communicationLogStep.Status = CommStatusEnum.W.ToString();
            _CommunicationLogStepRepository.Update(communicationLogStep);
            _CommunicationLog.CommunicationStatusTypeCode = CommStatusEnum.W.ToString();
            _CommunicationLog.CommunicationStatusTypeCode = CommStatusEnum.W.ToString();
            _CommunicationLogRepository.Update(_CommunicationLog);
            _CommunicationLogRepository.SubmitChanges();

            MyCustomsRequestsSheetPM.ChangeSetOp = ChangeSetOperation.Update;
            MyCustomsRequestsSheetPM.RequestStatusEnum = SheetStatusEnum.Received;
            _CustomsRequestsSheetUpdateService.Update(MyCustomsRequestsSheetPM, true);
            _CommonContext.SaveChanges();

            CustomsCommandEnum nxtCustomsCommandEnum = CustomsCommandEnum.CustomsCommandAnalyzeResponseWR;
            SBQMessageService.CreateBasic<CustomsCommandEnum>(
                            nxtCustomsCommandEnum,
                            this.MyCustomsRequestsSheetPM.Tenant,
                            this.MyCustomsRequestsSheetPM.InterfaceTypeCode,
                            this.MyCustomsRequestsSheetPM.Id, futureSendDateTime);
        }


        public void ReCreateNow()
        {

            
            CustomsCommandEnum nxtCustomsCommandEnum = CustomsCommandEnum.CustomsCommandGetCustomRequestWR;
            SBQMessageService.CreateBasic<CustomsCommandEnum>(
                            nxtCustomsCommandEnum,
                            this.MyCustomsRequestsSheetPM.Tenant,
                            this.MyCustomsRequestsSheetPM.InterfaceTypeCode,
                            this.MyCustomsRequestsSheetPM.Id);
        }
    }


    public class DCAUtil
    {
        public const string SufixRestored = "MR";
        public const string SufixDebugCreateNew = "DBGN";

        public static string GetExternalId(string selectedFile)
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
            if (extension.Equals(
                Logitude.Customs.BL.Messaging.Customs.DCAUtil.SufixRestored //"MR"
                , StringComparison.OrdinalIgnoreCase) ||
                extension.Equals(
                Logitude.Customs.BL.Messaging.Customs.DCAUtil.SufixDebugCreateNew//"DBGN"
                , StringComparison.OrdinalIgnoreCase))
            {
                fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileNameWithoutExtension);
                extension = Path.GetExtension(fileNameWithoutExtension);

                extension = extension.Substring(1);//remove dot 
            }
            return extension;
        }
    }

    public interface IUpdateBolb
    {
        void UpdateBolb(CustomsStepEnum customsRequestStep, Func<MemoryStream, MemoryStream> funcManupliateMemoryStream);
    }
    public class OverrideControllerModel
    {
        public bool DebugMode { get; set; }

        public bool IsAggregateDCAAnalyzer { get; set; }

        public bool SelectedDCAFileDebugCreateNew { get; set; }

        public CustomsCommandEnum? CurrentCustomsCommandWR { get; set; }

        public string AggregateDCAAnalyzerLogger { get; set; }
        public bool IsCustomsMessagingSheetWR { get; set; }
    }


}
