using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.BL.Messaging.Customs;
using Logitude.Customs.Data;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.Helpers;
using Logitude.CustomsMessaging.MessagingServices;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.GlobalScannedAttachmentToEntityServiceReference;
using Logitude.Server.Tools.Utils;
using Logitude.Customs.BL.BL;
using Logitude.Customs.Def.EntityQueryServicesExt;
using Logitude.Server.Tools;
using Microsoft.Practices.Unity;
using Simplog.Data.CommonDataModel.Repositories;
using Logitude.Customs.BL.Messaging.Customs.SignQueueBL;

namespace Logitude.CustomsMessaging.ResponseServices
{
    public class D_NG_2716_MSG22001_AddAttachmentResponseService :
        ResponseServiceBase<AddAttachmentResponseData, D_NG_2716_MSG22001_AddAttachmentResponse, D_NG_2715_MSG22002_AddAGlobalScannedAttachmentToEntityRequestParam>
    {
        CustomsDocumentPM _MyCustomsDocumentPM;
       
        public override AddAttachmentResponseData GetResponse(D_NG_2716_MSG22001_AddAttachmentResponse customResponse, D_NG_2715_MSG22002_AddAGlobalScannedAttachmentToEntityRequestParam requestParams)
        {

            return this.MyResponseData;
        }
        public override void OnRequestFail(D_NG_2716_MSG22001_AddAttachmentResponse customResponse, D_NG_2715_MSG22002_AddAGlobalScannedAttachmentToEntityRequestParam requestParams)
        {

            var context = CustomContext.GetContext(requestParams.Tenant);
            var myQueryService = new CustomsDocumentQueryService(context);
            var myCustomsDocumentUpdateService = new CustomsDocumentUpdateService(context, new Dictionary<string, IContext>(), requestParams.Tenant);

            var myCustomsDocumentPM = myQueryService.GetSingle(requestParams.DocumentsFilingId, true, false);

            var featureDocumentStatusCodeShouldNOTChange = ConfigurationManager.AppSettings["20180624.DocumentStatusCodeShouldNOTChange"] == "1";

            if (///myCustomsDocumentPM.DocumentStatusCode == "1" || 
                featureDocumentStatusCodeShouldNOTChange &&
                !String.IsNullOrWhiteSpace(myCustomsDocumentPM.CustomsDocId))
            {
                LogMessagingUtil.Instance.AppendLine(" D_NG_2716_MSG22001_AddAttachmentResponseService.OnRequestFail stop !!!  sorry  due String.IsNullOrWhiteSpace(myCustomsDocumentPM.CustomsDocId))");
                return;
            }
            myCustomsDocumentPM.DocumentStatusCode = "2";
            myCustomsDocumentPM.ChangeSetOp = ChangeSetOperation.Update;
            myCustomsDocumentUpdateService.Update(myCustomsDocumentPM, true);
            UpdateDeclarationCourierStatus(context, myCustomsDocumentPM, requestParams.DeclaretionId);
            base.OnRequestFail(customResponse, requestParams);
        }
        public override void Update(D_NG_2716_MSG22001_AddAttachmentResponse customResponse, D_NG_2715_MSG22002_AddAGlobalScannedAttachmentToEntityRequestParam requestParams)
        {
            bool lockit = !string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings.Get("Singleton.CRS:2715/UDLT"));
            string key = ProcessLockTableUtil.Instance.GetKey4DocumentsFilingId(requestParams.DocumentsFilingId, requestParams.Tenant);
            bool SyncUpdateDeclarationCourier_DocumentStatusCode = true;//In ECOMMERCE(DSV) 2 docment per dec - force Sync UpdateDeclarationCourierStatus
            if (SyncUpdateDeclarationCourier_DocumentStatusCode && !String.IsNullOrWhiteSpace(requestParams.DeclaretionId))
            {
                var declarationQueryService = new Logitude.Customs.BL.EntityQueryServices.DeclarationQueryService(requestParams.Tenant);
                var connectedDeclarationPM = declarationQueryService.GetSingle(requestParams.DeclaretionId, false, false);
                if (connectedDeclarationPM != null && connectedDeclarationPM.IsCourierDeclaration)
                {
                    lockit = true;
                    key = ProcessLockTableUtil.Instance.GetKey4UpdateDeclarationCourier_DocumentStatusCode(connectedDeclarationPM.Id, requestParams.Tenant);
                }

            }

            using (var processLockTableDisposable = ProcessLockTableUtil.Instance.GetProcessLockTableDisposable(requestParams.Tenant, lockit, key, "CRS:2715/UDLT"))
            {
                RealUpdate(customResponse, requestParams);
            }
        }

        

        private void RealUpdate(D_NG_2716_MSG22001_AddAttachmentResponse customResponse, D_NG_2715_MSG22002_AddAGlobalScannedAttachmentToEntityRequestParam requestParams)
        {
            this.MyResponseData = new AddAttachmentResponseData()
            {
                Succeeded = true,
                HasException = false,
            };

            /*if (customResponse.ResponseContentHeader.Exception != null)
            {
                if (customResponse.ResponseContentHeader.Exception.FirstOrDefault() != null)
                {
                    if (customResponse.ResponseContentHeader.Exception.FirstOrDefault().ExeptionDescription != null)
                    {
                        if (!string.IsNullOrWhiteSpace(customResponse.ResponseContentHeader.Exception.FirstOrDefault().ExeptionDescription.ToString()))
                        {
                            string errorMessage = customResponse.ResponseContentHeader.Exception.FirstOrDefault().ExeptionDescription.ToString();
                            this.MyResponseData.Succeeded = true;
                            this.MyResponseData.HasException = true;
                            this.MyResponseData.UserMessage = errorMessage;
                            LogMessagingUtil.Instance.AppendLine(errorMessage);

                            return;
                        }
                    }
                }
            }*/

            if (String.IsNullOrWhiteSpace(customResponse.externalAttachmentID))
            {
                if (String.IsNullOrWhiteSpace(customResponse.ResponseContentHeader.Remark))
                {
                    LogMessagingUtil.Instance.AppendLine("No Customs Document details in the Response " + requestParams.DocumentsFilingId);
                }
                else
                {
                    LogMessagingUtil.Instance.AppendLine("No Customs Document details in the Response " + customResponse.ResponseContentHeader.Remark + requestParams.DocumentsFilingId);
                }
                //return;
            }

            var context = CustomContext.GetContext(requestParams.Tenant);
            var myQueryService = new CustomsDocumentQueryService(context);
            var myCustomsDocumentUpdateService = new CustomsDocumentUpdateService(context, new Dictionary<string, IContext>(), requestParams.Tenant);

            this._MyCustomsDocumentPM = myQueryService.GetSingle(requestParams.DocumentsFilingId, true, false);
            if (this._MyCustomsDocumentPM == null)
            {
                LogMessagingUtil.Instance.AppendLine("Can not found Customs Document" + requestParams.DocumentsFilingId);
                this.MyResponseData.HasException = true;
                this.MyResponseData.ErrorRemarks = "Can not found Customs Document" + requestParams.DocumentsFilingId;
                return;
            }

            var declarationQueryService = new Logitude.Customs.BL.EntityQueryServices.DeclarationQueryService(requestParams.Tenant);
            var declarationPM = declarationQueryService.GetSingle(requestParams.DeclaretionId, false, false);

            LogMessagingUtil.Instance.AppendLine("Analyze Customs Document response" + requestParams.DocumentsFilingId);
            _MyCustomsDocumentPM.ChangeSetOp = ChangeSetOperation.Update;
            //NO ApplicationID - ERROR
            if (string.IsNullOrWhiteSpace(customResponse.ResponseContentHeader.ApplicationID.ToString()))
            {
                _MyCustomsDocumentPM.DocumentStatusCode = "2";
            }
            else
            {
                //ApplicationID is '0' - ERROR
                if (customResponse.ResponseContentHeader.ApplicationID == 0)
                {
                    _MyCustomsDocumentPM.DocumentStatusCode = "2";
                }
                //ApplicationID is  the Customs Document ID - SUCCESS
                else
                {
                    //CustomsDocumentsTicketQueryService myCustomsDocumentsTicketQueryService = new CustomsDocumentsTicketQueryService(context);
                    //List<CustomsDocumentsTicketPM> customsDocumentsTicketPMList = myCustomsDocumentsTicketQueryService.GetCustomsDocumentsTicketsByDocumentsFilingId(requestParams.DocumentsFilingId, requestParams.Tenant);
                    //if (customsDocumentsTicketPMList != null) // && customsDocumentsTicketPMList.FirstOrDefault() != null && !string.IsNullOrWhiteSpace(customsDocumentsTicketPMList.FirstOrDefault().RequestedCustomsDocId))
                    //{
                    //    //_MyCustomsDocumentPM.DocumentStatusCode = "8"; // Verification Progress
                    //    var myCustomsDocumentsTicketUpdateService = new CustomsDocumentsTicketUpdateService(context, new Dictionary<string, IContext>(), requestParams.Tenant);
                    //    foreach (var customsDocumentsTicket in customsDocumentsTicketPMList)
                    //    {
                    //        if (!string.IsNullOrWhiteSpace(customsDocumentsTicket.RequestedCustomsDocId))
                    //        {
                    //            customsDocumentsTicket.VerificationStatusTypeCode = "8";
                    //            customsDocumentsTicket.ChangeSetOp = ChangeSetOperation.Update;
                    //            myCustomsDocumentsTicketUpdateService.Update(customsDocumentsTicket, true);
                    //        }
                    //        else
                    //        {
                    //            customsDocumentsTicket.ChangeSetOp = ChangeSetOperation.Update;
                    //            myCustomsDocumentsTicketUpdateService.Update(customsDocumentsTicket, true);
                    //        }
                     //    }
                  //  }

                    //else
                    {
                        _MyCustomsDocumentPM.DocumentStatusCode = "1"; // Sent
                    }
                    _MyCustomsDocumentPM.CustomsDocId = customResponse.ResponseContentHeader.ApplicationID.ToString();

                    if (requestParams.IsFromAutoClosing)
                    {
						
                        if (declarationPM?.TransportModeId == "O") {

                           var declarations = declarationQueryService.GetDeclarationsByExportFile(declarationPM.Tenant, declarationPM.ExportFile);
                            
                           foreach ( var declaration in declarations)
                           {
						    	ICustomsAutoDecClosing CustomsAutoDecClosing = ContainerAccessor.Container.Resolve(typeof(ICustomsAutoDecClosing), "CustomsAutoDecClosing", new ParameterOverride("", 1)) as ICustomsAutoDecClosing;
						    	CustomsAutoDecClosing.Send8235(declaration, requestParams.LoggingUserId);
						   }
						}
                        else
                        {
							ICustomsAutoDecClosing CustomsAutoDecClosing = ContainerAccessor.Container.Resolve(typeof(ICustomsAutoDecClosing), "CustomsAutoDecClosing", new ParameterOverride("", 1)) as ICustomsAutoDecClosing;
							CustomsAutoDecClosing.Send8235(declarationPM, requestParams.LoggingUserId);
						}

					}
                }
            }


            if(customResponse.ResponseContentHeader!= null && customResponse.ResponseContentHeader.Exception!= null 
                && customResponse.ResponseContentHeader.Exception[0].ExceptionParms[0]== _MyCustomsDocumentPM.ExternalAttachmentId && !string.IsNullOrEmpty(_MyCustomsDocumentPM.CustomsDocId))
            {
                _MyCustomsDocumentPM.DocumentStatusCode = "1";
            }
            
            //if there is an error - Log the error text
            if (_MyCustomsDocumentPM.DocumentStatusCode == "2")
            {
                if (customResponse.ResponseContentHeader.Exception != null)
                {
                    string exeptionDescription = "";
                    string exeptionType = "";
                    foreach (var error in customResponse.ResponseContentHeader.Exception)
                    {
                        if (!string.IsNullOrWhiteSpace(exeptionDescription) && !string.IsNullOrWhiteSpace(exeptionType))
                        {
                            exeptionType = exeptionType + " , ";
                            exeptionDescription = exeptionDescription + "\n";
                        }
                        exeptionType = exeptionType + error.ExeptionType;
                        exeptionDescription = exeptionDescription + error.ExeptionDescription;
                    }
                    this.MyResponseData.HasException = true;
                    this.MyResponseData.UserMessage = exeptionDescription;
                    this.MyResponseData.ErrorCode = exeptionType;
                    this.MyResponseData.ErrorRemarks = exeptionDescription;
                    LogMessagingUtil.Instance.AppendLine(exeptionDescription);
                }
            }

            _MyCustomsDocumentPM.CustomRecievedDate = DateTime.Now;
            _MyCustomsDocumentPM.CurrentContextTag = Logitude.Customs.BL.EntityUpdateServices.CustomsDocumentUpdateService.AvoidSendToCustoms;
            if (!string.IsNullOrWhiteSpace(_MyCustomsDocumentPM.DocumentRemarks) &&
                _MyCustomsDocumentPM.DocumentRemarks.Contains(CustomsDocumentUpdateService.LoadTestSendMessageToQueue))
            {
                LogMessagingUtil.Instance.AppendLine("LoadTestSendMessageToQueue  >>> LoadTest");
                _MyCustomsDocumentPM.DocumentRemarks = "LoadTest";
            }

            LogMessagingUtil.Instance.AppendLine("_MyCustomsDocumentPM.CustomsDocId == " + _MyCustomsDocumentPM.CustomsDocId);
            LogMessagingUtil.Instance.AppendLine("_MyCustomsDocumentPM.DocumentStatusCode == " + _MyCustomsDocumentPM.DocumentStatusCode);
            EnshureIsPartOfDeclaration(context, requestParams.DeclaretionId);
            myCustomsDocumentUpdateService.Update(_MyCustomsDocumentPM, true);

            CustomsDocumentsTicketQueryService myCustomsDocumentsTicketQueryService = new CustomsDocumentsTicketQueryService(context);
            List<CustomsDocumentsTicketPM> customsDocumentsTicketPMList = myCustomsDocumentsTicketQueryService.GetCustomsDocumentsTicketsByDocumentsFilingId(requestParams.DocumentsFilingId, requestParams.Tenant);
            if (customsDocumentsTicketPMList != null) // && customsDocumentsTicketPMList.FirstOrDefault() != null && !string.IsNullOrWhiteSpace(customsDocumentsTicketPMList.FirstOrDefault().RequestedCustomsDocId))
            {
                //_MyCustomsDocumentPM.DocumentStatusCode = "8"; // Verification Progress
                var myCustomsDocumentsTicketUpdateService = new CustomsDocumentsTicketUpdateService(context, new Dictionary<string, IContext>(), requestParams.Tenant);
                foreach (var customsDocumentsTicket in customsDocumentsTicketPMList)
                {
                    if (!string.IsNullOrWhiteSpace(customsDocumentsTicket.RequestedCustomsDocId) && _MyCustomsDocumentPM.DocumentStatusCode  != "2")
                    {
                        if (_MyCustomsDocumentPM.DocumentStatusCode == "1" && declarationPM != null)
                        {
                            DeclarationUpdateService declarationUpdateService = new DeclarationUpdateService(context, new Dictionary<string, IContext>(), requestParams.Tenant);

                            var requestedCustomsDocId = myCustomsDocumentsTicketQueryService.CheckRequestedCustomsDocIdsByEntityIdAndChilds(requestParams.DeclaretionId, requestParams.Tenant, "", customsDocumentsTicket.RequestedCustomsDocId);

                            if (requestedCustomsDocId != declarationPM.RequestedCustomsDocId)
                                declarationPM.ChangeSetOp = ChangeSetOperation.Update;

                            declarationUpdateService.Update(declarationPM, true);
                        }

                        customsDocumentsTicket.VerificationStatusTypeCode = "8";
                        customsDocumentsTicket.ChangeSetOp = ChangeSetOperation.Update;
                        myCustomsDocumentsTicketUpdateService.Update(customsDocumentsTicket, true);
                    }
                    else
                    {
                        customsDocumentsTicket.ChangeSetOp = ChangeSetOperation.Update;
                        myCustomsDocumentsTicketUpdateService.Update(customsDocumentsTicket, true);
                    }
                }
            }

            UpdateDeclarationCourierStatus(context, _MyCustomsDocumentPM, requestParams.DeclaretionId);
            this.MyResponseData.ApplicationID = _MyCustomsDocumentPM.CustomsDocId;
            this.MyResponseData.DocumentNumber = _MyCustomsDocumentPM.ExternalAttachmentId;
            this.MyResponseData.CustomDocument = _MyCustomsDocumentPM.CustomsDocId;
            this.MyResponseData.CustomRecievedDate = _MyCustomsDocumentPM.CustomRecievedDate.Value.Date.ToString("dd/MM/yyyy");
            this.MyResponseData.Remarks = _MyCustomsDocumentPM.DocumentRemarks;

            //this.MyRequestSheetParam = D_NG_2715_MSG22002_AddAGlobalScannedAttachmentToEntityMessagingService.GetReqSheetParam(requestParams.DocumentsFilingId, requestParams.DeclaretionId, null);

            //Open Task in Unifreight
            LogMessagingUtil.Instance.AppendLine("Open Task in Unifreight by using DocumentsFilingService");
            DocumentsFilingQuery documentsFilingQuery = new DocumentsFilingQuery(requestParams.Tenant);
            DocumentsFilingPM documentIn = documentsFilingQuery.GetSinglePM(_MyCustomsDocumentPM.DocumentsFilingId, requestParams.Tenant);
            if (documentIn != null)
            {
                ICommonDataContext dataContext = CommonDataContext.GetContext(requestParams.Tenant);
                var documentsFilingService = new UnifreightDocumentsFilingService(dataContext, requestParams.Tenant, new CustomDocumentsFilingParams() { MainInterfaceCode = "2715" });
                documentsFilingService.Update(documentIn, null, requestParams.LoggingUserId, false);
            }

            if (!string.IsNullOrWhiteSpace(_MyCustomsDocumentPM.DocumentRemarks) &&
               _MyCustomsDocumentPM.DocumentRemarks.Contains(CustomsDocumentUpdateService.WhileAnalayzeCostomResponseSendDEC))
            {
                LogMessagingUtil.Instance.AppendLine("WhileAnalayzeCostomResponseSendDEC  >>> LoadTest");

                //SBQMessageService.CreateSheetSBQMessage<Logitude.CustomsMessaging.Common.RequestParams.>(requestParams
                //    , false
                //    );


                var genericRequestParams = new GenericRequestParams()
                {
                    Tenant = requestParams.Tenant,
                    AppicationId = requestParams.DeclaretionId,
                    RequestVIA = SendRequestVIA.WebServiceBatch,
                    LoggingEnabled = true,
                    InterfaceTypeCode = "2750",
                    MainInterfaceCode = "2750",
                    LoggingEntityId = requestParams.DeclaretionId,
                    //LoggingEntityReference = this._DeclarationPM.DeclarationNumber;
                    LoggingObjectTableId = ObjectTableRepository.GetObjectTableByName("Customs.Declaration"),
                    LoggingUserId = requestParams.LoggingUserId,
                };
                //var messService = new Logitude.CustomsMessaging.MessagingServices.DF_MSG10000_ImportDeclarationMessagingService();
                //var responseData = messService.SendSheet(genericRequestParams);


                using (var trans = TransactionFactory.GetNewTransaction())
                {
                    try
                    {
                        SBQMessageService.CreateSheetSBQMessage<Logitude.CustomsMessaging.Common.RequestParams.GenericRequestParams>(genericRequestParams
                            , false
                            );
                        trans.Complete();
                    }
                    catch (CustomsRequestsSheetDomainModelServiceException myCustomsRequestsSheetServiceException)
                    {
                        if (myCustomsRequestsSheetServiceException.Where == CustomsRequestsSheetDomainModelServiceException.WhereEnum.SameRequestInProgress)
                        {
                            Logitude.Server.Tools.Helpers.LogMessagingUtil.Instance.AppendLine(" load test SameRequestInProgress!! " + myCustomsRequestsSheetServiceException.Message);

                        }
                        else if (myCustomsRequestsSheetServiceException.Where == CustomsRequestsSheetDomainModelServiceException.WhereEnum.NoAvailableSignServer)
                        {
                            Logitude.Server.Tools.Helpers.LogMessagingUtil.Instance.AppendLine("load test SameRequestInProgress!!  " + myCustomsRequestsSheetServiceException.Message);
                        }
                        //throw;
                    }
                }
            }

            // for the Diamonds declaration in export, send it automatically to the mehes
            if (declarationPM != null) {
                if (declarationPM.IsDiamondDeclaration && declarationPM.Direction == "E" && declarationPM.AutoSending)
                {
                    try
                    {
                        LogMessagingUtil.Instance.AppendLine("Check to send diamonds declaration to mehes");
                        SendAutomaticReadyDeclaration(declarationPM);
                    }
                    catch (System.Exception e)
                    {
                        LogMessagingUtil.Instance.AppendLine("Failed to send automatic diamonds declarations: " + e.ToString());
                    }
                }
                else
                {
                    LogMessagingUtil.Instance.AppendLine("No check for diamonds process");
                }
            }
           
        }

        private void SendAutomaticReadyDeclaration(DeclarationPM declaration)
        {
            // determine if the export diamonds feature is enabled to allow autosending
            ICommonDataContext myContextCommon = CommonDataContext.GetContext(declaration.Tenant);
            FeatureRepository myFeatureRepository = new FeatureRepository(myContextCommon);
            FeatureQuery featureQuery = new FeatureQuery(myFeatureRepository);
            var features = featureQuery.GetAllowedFeaturesForLoggedUser(AuthenticationUtil.ResolveUserId(declaration.Tenant), declaration.Tenant);
            var featureExportDiamonds = features.Features.FirstOrDefault(x => x.Code == "ExportDiamonds");

            if (featureExportDiamonds != null)
            {
                // check the declaration is ready to be sent the mehes. if then send it
                var declarationQueryService = new DeclarationQueryService(declaration.Tenant);
                var signQueueHSMService = new SignQueueHSMService();
                 bool declarationReadyForSending = declarationQueryService.CheckDiamondsDeclarationReadyForSending(declaration);
                var loggedUserId = string.Empty;

                var objecttableId = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
                if (signQueueHSMService.IsHSMSign_IsOn(declaration.Tenant))
                {
                    loggedUserId = AuthenticationUtil.ResolveUserId(declaration.Tenant);
                }
                else
                {
                    if (!string.IsNullOrEmpty(declaration.SignedByUserId))
                    {
                        loggedUserId = declaration.SignedByUserId;
                    }
                    else
                    {
                        loggedUserId = declarationQueryService.GetSignedByUserIdByCustomFileNo(declaration.Tenant, declaration.CustomFileNo);
                    }
                }
                if (declarationReadyForSending)
                {
                    GenericRequestParams requestParamsData = new GenericRequestParams()
                    {
                        AppicationId = declaration.Id,
                        Tenant = declaration.Tenant,
                        RequestVIA = SendRequestVIA.WebServiceBatch,
                        ForcePersonalSign = true,
                        LoggingEnabled = true,
                        LoggingEntityId = declaration.Id,
                        LoggingEntityReference = declaration.DeclarationNumber,
                        LoggingUserId = loggedUserId,
                        RequestName = "Declaration Request",
                        ResponseName = "Declaration Response",
                        ForceCompanySign = false,
                        LoggingObjectTableId = ObjectTableRepository.GetObjectTableByName("Customs.Declaration"),
                        FutureSendDateTime= DateTime.Now.AddMinutes(5),
                        
                    };

                    LogMessagingUtil.Instance.AppendLine("Send declaration to mehes");
                    INF_MSG_GenericResponseData responseData;
                    var messagingService = new DF_NG_2751_MSG10000_ExportDeclarationMessagingService();
                    responseData = messagingService.Send(requestParamsData);
                }
            }
        }


        private void EnshureIsPartOfDeclaration(ICustomContext context, string declaretionId)
        {
            if (String.IsNullOrWhiteSpace(declaretionId)) return;
            if (_MyCustomsDocumentPM.IsPartOfDeclaration) return;
            LogMessagingUtil.Instance.AppendLine("EnshureIsPartOfDeclaration");
            var theKey = _MyCustomsDocumentPM.DocumentsFilingId;
            //thekey  CUSTOMSDOCUMENTSTICKETS.id 
            //CUSTOMSDOCUMENTSTICKETS.DOCUMENTSFILINGID  =>DOCUMENTSFILINGS.id

            //var customsDocumentsTicketQueryService = new CustomsDocumentsTicketQueryService(context);
            //var connectedTickets=customsDocumentsTicketQueryService.GetCustomsDocumentsTicketsByDocumentsFilingId(_MyCustomsDocumentPM.DocumentsFilingId, _MyCustomsDocumentPM.Tenant);


            //thekey  CUSTOMSDOCUMENTPOINTERS.id 
            //CUSTOMSDOCUMENTPOINTERS.CUSTOMSDOCUMENTSTICKETID==    CUSTOMSDOCUMENTSTICKETS.id 
            //CUSTOMSDOCUMENTPOINTERS.PARENTENTITYCODE =  'Declaration' &&  CUSTOMSDOCUMENTPOINTERS.PARENTENTITYID== declaretionId!!!

            var customsDocumentPointerQueryService = new CustomsDocumentPointerQueryService(context);
            var connectdCustomDocumentPointers = customsDocumentPointerQueryService.GetCustomDocumentPointersForCustomDocumentId(_MyCustomsDocumentPM.DocumentsFilingId, _MyCustomsDocumentPM.Tenant);
            var connectdPointer = connectdCustomDocumentPointers.Where(rec => rec.ParentEntityCode == "Declaration" && rec.ParentEntityId == declaretionId).FirstOrDefault();
            if (connectdPointer != null)
            {
                LogMessagingUtil.Instance.AppendLine("_MyCustomsDocumentPM.IsPartOfDeclaration = true;");
                _MyCustomsDocumentPM.IsPartOfDeclaration = true;
            }

        }

        private void UpdateDeclarationCourierStatus(ICustomContext context, CustomsDocumentPM entityPM, string declaretionId)
        {

            string status = null;
            if (String.IsNullOrWhiteSpace(declaretionId)) return;

            DeclarationPM connectedDeclarationPM = null;
            var declarationQueryService = new Logitude.Customs.BL.EntityQueryServices.DeclarationQueryService(entityPM.Tenant);
            connectedDeclarationPM = declarationQueryService.GetSingle(declaretionId, false, false);
            if (connectedDeclarationPM != null && connectedDeclarationPM.IsCourierDeclaration)
            {
                DeclarationCourierStatusUpdateService declarationCourierStatusUpdateService = new DeclarationCourierStatusUpdateService(context, new Dictionary<string, IContext>(), connectedDeclarationPM.Tenant);
                DeclarationCourierStatusQueryService declarationCourierStatusQueryService = new DeclarationCourierStatusQueryService(context);
                DeclarationCourierStatusPM currentDeclarationCourierStatusPM = declarationCourierStatusQueryService.GetSingle(connectedDeclarationPM.Id, true, false);

                if (currentDeclarationCourierStatusPM != null)
                {
                    CalculateDeclarationCourierStatus calculateDeclarationCourierStatus = new CalculateDeclarationCourierStatus(connectedDeclarationPM, connectedDeclarationPM.Id, connectedDeclarationPM.Tenant);
                    string prevVal = null;
                    string currvVal = null;

                    prevVal = currentDeclarationCourierStatusPM.CourierDeclarationStatusCode;
                   // calculateDeclarationCourierStatus.CalcDocumentStatusCode(currentDeclarationCourierStatusPM);
                    calculateDeclarationCourierStatus.CalcCourierDeclarationStatusCode(currentDeclarationCourierStatusPM);
                    currvVal = currentDeclarationCourierStatusPM.CourierDeclarationStatusCode;

                    //if (prevVal != currvVal)
                    //{
                        LogMessagingUtil.Instance.AppendLine("currentDeclarationCourierStatusPM.CourierDeclarationStatusCode: " + currentDeclarationCourierStatusPM.CourierDeclarationStatusCode);
                        currentDeclarationCourierStatusPM.ChangeSetOp = ChangeSetOperation.Update;
                        LogMessagingUtil.Instance.AppendLine($"D_NG_2716_MSG22001_AddAttachmentResponseService UpdateDeclarationCourierStatus currentDeclarationCourierStatusPM.DocumentStatusCode = {currentDeclarationCourierStatusPM.DocumentStatusCode}");
                        declarationCourierStatusUpdateService.Update(currentDeclarationCourierStatusPM, true);
                    //}
                }

            }
        }

        //private void UpdateDeclarationCourierStatus(ICustomContext context, CustomsDocumentPM entityPM, string declaretionId)
        //{

        //    string status = null;
        //    if (String.IsNullOrWhiteSpace(declaretionId)) return;

        //    DeclarationPM connectedDeclarationPM = null;
        //    var declarationQueryService = new Logitude.Customs.BL.EntityQueryServices.DeclarationQueryService(entityPM.Tenant);
        //    connectedDeclarationPM = declarationQueryService.GetSingle(declaretionId, false, false);
        //    if (connectedDeclarationPM != null && connectedDeclarationPM.IsCourierDeclaration)
        //    {

        //        if (entityPM.DocumentStatusCode == "2")
        //        {
        //            status = "X";
        //        }
        //        else
        //        {

        //            var myQueryService = new CustomsDocumentQueryService(context);
        //            var customsDocumentPMList = myQueryService.GetDeclarationDocumentList(declaretionId, "Declaration", entityPM.Tenant);
        //            if (customsDocumentPMList != null && customsDocumentPMList.Where(r => r.DocumentStatusCode == "2").Count() > 0)
        //            {
        //                status = "X";
        //            }
        //            else if (entityPM.DocumentStatusCode == "1" && (customsDocumentPMList == null || customsDocumentPMList != null && customsDocumentPMList.Where(r => r.DocumentStatusCode != "1").Count() < 1))
        //            {
        //                status = "V";
        //            }
        //        }

        //        DeclarationCourierStatusUpdateService declarationCourierStatusUpdateService = new DeclarationCourierStatusUpdateService(context, new Dictionary<string, IContext>(), connectedDeclarationPM.Tenant);
        //        DeclarationCourierStatusQueryService declarationCourierStatusQueryService = new DeclarationCourierStatusQueryService(context);
        //        DeclarationCourierStatusPM currentDeclarationCourierStatusPM = declarationCourierStatusQueryService.GetSingle(connectedDeclarationPM.Id, true, false);

        //        string CourierDeclarationstatus = null;
        //        if (currentDeclarationCourierStatusPM != null)
        //        {
        //            CalculateDeclarationCourierStatus calculateDeclarationCourierStatus = new CalculateDeclarationCourierStatus(connectedDeclarationPM, connectedDeclarationPM.Id, connectedDeclarationPM.Tenant);
        //            string prevVal = null;
        //            string currvVal = null;

        //            prevVal = currentDeclarationCourierStatusPM.CourierDeclarationStatusCode;
        //            calculateDeclarationCourierStatus.CalcCourierDeclarationStatusCode(currentDeclarationCourierStatusPM);
        //            currvVal = currentDeclarationCourierStatusPM.CourierDeclarationStatusCode;

        //            if (prevVal != currvVal)
        //            {
        //                CourierDeclarationstatus = currvVal;
        //            }
        //            LogMessagingUtil.Instance.AppendLine("currentDeclarationCourierStatusPM.CourierDeclarationStatusCode: " + currentDeclarationCourierStatusPM.CourierDeclarationStatusCode);
        //        }

        //        if (!string.IsNullOrWhiteSpace(status) || !string.IsNullOrWhiteSpace(CourierDeclarationstatus))
        //        {

        //            if (currentDeclarationCourierStatusPM == null)
        //            {
        //                currentDeclarationCourierStatusPM = new DeclarationCourierStatusPM()
        //                {
        //                    DeclarationId = connectedDeclarationPM.Id,
        //                    Tenant = connectedDeclarationPM.Tenant,
        //                    IsClosedForFollowUp = false,
        //                    IsCourierMissingClassification = false,
        //                };
        //                currentDeclarationCourierStatusPM.ChangeSetOp = ChangeSetOperation.Insert;
        //            }
        //            else
        //            {
        //                currentDeclarationCourierStatusPM.ChangeSetOp = ChangeSetOperation.Update;
        //            }
        //            LogMessagingUtil.Instance.AppendLine($"D_NG_2716_MSG22001_AddAttachmentResponseService UpdateDeclarationCourierStatus currentDeclarationCourierStatusPM.DocumentStatusCode = {status}");
        //            if(!string.IsNullOrWhiteSpace(status))currentDeclarationCourierStatusPM.DocumentStatusCode = status;
        //            if(!string.IsNullOrWhiteSpace(CourierDeclarationstatus))currentDeclarationCourierStatusPM.CourierDeclarationStatusCode = CourierDeclarationstatus;
        //            declarationCourierStatusUpdateService.Update(currentDeclarationCourierStatusPM, true);
        //        }
        //    }
        //}
    }
}
