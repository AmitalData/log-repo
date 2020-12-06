using Logitude.AmitalMessaging.Utils;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.BL.Models;
using Logitude.Customs.Data;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.MessagingServices;
using Logitude.CustomsMessaging.ResponseServices.DeclarationErrorPointer;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.Utils;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.ImportDeclarationServiceReference;
using UnifreightIIG.Common.MessageLib.Fault;
using UnifreightIIG.Common.MessageLib.ID;
using UnifreightIIG.Common.MessageLib.Collateral;
using Logitude.Customs.BL.Messaging.LogitudeClient.DeclarationErrorPointer;
using System.Reflection;
using System.Xml.Serialization;
using Logitude.Customs.BL.TraceEvents;
using Simplog.Data.CommonDataModel.Repositories;
using Declaration = UnifreightIIG.Common.MessageLib.ID.Declaration;
using System.Diagnostics;
using Response = UnifreightIIG.Common.MessageLib.ID.Response;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.BL.Messaging.LogitudeClient.DeclarationErrorPointer.DBWCO;
using Unifreight.Data.AmitalModel;
using Unifreight.BL.EntityQueryServices;
using ResponseError = UnifreightIIG.Common.MessageLib.ID.ResponseError;
using DeclarationGoodsShipment = UnifreightIIG.Common.MessageLib.ID.DeclarationGoodsShipment;
using DeclarationGoodsShipmentCustomsValuation = UnifreightIIG.Common.MessageLib.ID.DeclarationGoodsShipmentCustomsValuation;
using UnifreightIIG.Common.MessageLib.Ransom;
using Logitude.BL.CommonDataModel.EntityQueries;

namespace Logitude.CustomsMessaging.ResponseServices
{
    public class DF_NG_5117_ImportDeclerationAmendmentReplyResponseService : ResponseServiceBase<INF_MSG_GenericResponseData, DF_NG_5117_MSG14003_ImportDeclarationAmendmentReplyMsg, GenericRequestParams>
    {
        DeclarationPM _MyDeclarationPM;
        DeclarationPM _MyDeclarationPMOrg;

        private DeclarationPrintResponseData _SendDeclarationPrintResponse;
 

        public INF_MSG_GenericResponseData Update5117(DF_NG_5117_MSG14003_ImportDeclarationAmendmentReplyMsg customResponse, GenericRequestParams requestParams  )
        {
             Update( customResponse , requestParams);
            return this.MyResponseData;
        }
        public override void Update(DF_NG_5117_MSG14003_ImportDeclarationAmendmentReplyMsg customResponse, GenericRequestParams requestParams)
        {
            var context = CustomContext.GetContext(requestParams.Tenant);
            var myDeclarationQueryService = new DeclarationQueryService(context);
            var myDeclarationUpdateService = new DeclarationUpdateService(context, new Dictionary<string, IContext>(), requestParams.Tenant);
            this.MyResponseData = new INF_MSG_GenericResponseData();
            DeclarationCorrectionsPointerService myDeclarationCorrectionsPointerService = new DeclarationCorrectionsPointerService();
            string error = "";
            DF_NG_2754_MSG10004_ImportAmendmentDeclarationResponseService dF_NG_2754_MSG10004_ImportFixedDeclarationResponseService = new DF_NG_2754_MSG10004_ImportAmendmentDeclarationResponseService();
 


            FeatureQuery featureQuery = new FeatureQuery();

            var features = featureQuery.GetAllowedFeaturesForLoggedUser(requestParams.LoggingUserId, requestParams.Tenant);

            var feature = features.Features.FirstOrDefault(x => x.Code == "DECLARATIONAMENDMENT");
            if (feature != null)
            {
                var dec = new UnifreightIIG.Common.ImportDeclarationServiceReference.Declaration();

                bool fromMehes = false;

                if(customResponse.ResponseContentHeader != null && customResponse.ResponseContentHeader.Exception!= null && customResponse.ResponseContentHeader.Exception.Count()>0)
                {
                     this.MyResponseData.ApplicationID = requestParams.AppicationId;
                    this.MyResponseData.Succeeded = true;
                    this.MyResponseData.UserMessage = customResponse.ResponseContentHeader.Exception[0].ExeptionDescription;
                    this.MyResponseData.HasException = false;

                    return;

                }
             
                 string functionalReferenceID = "";
                if (customResponse.Response.FunctionalReferenceID != null)
                    functionalReferenceID = customResponse.Response.FunctionalReferenceID.Value;
                else
                    functionalReferenceID = "";

                var declaration = myDeclarationQueryService.GetDeclarationByfunctionalReferenceID(functionalReferenceID, requestParams.Tenant);



                if (declaration != null)
                {
                    _MyDeclarationPM = declaration;
                    if (customResponse.Response.Declaration != null)
                        _MyDeclarationPM = dF_NG_2754_MSG10004_ImportFixedDeclarationResponseService.MapResponseToDeclaration(CastDeclaration(customResponse.Response.Declaration), requestParams.Tenant, false, _MyDeclarationPM.Id, out error, false, isUpdateAfterAccept: true);

                }
                else
                {

                    if (customResponse.Response.Declaration != null && customResponse.Response.Declaration.ID != null && customResponse.Response.Declaration.ID.Value != null && customResponse.Response.Declaration.ID.Value.Substring(2, 2) == "99")
                    {
                        _MyDeclarationPM = myDeclarationUpdateService.GetSertByConvertedDeclarationNumber(customResponse.Response.Declaration.ID.Value, requestParams.Tenant);

                        _MyDeclarationPM = dF_NG_2754_MSG10004_ImportFixedDeclarationResponseService.MapResponseToDeclaration(CastDeclaration(customResponse.Response.Declaration), requestParams.Tenant, false, _MyDeclarationPM.Id, out error, true);
 
                    }

                    else if(customResponse.Response.Declaration!= null)
                    {

                        string id = myDeclarationQueryService.GetIdByDeclarationNumber(customResponse.Response.Declaration.ID.Value, requestParams.Tenant);

                        _MyDeclarationPM = dF_NG_2754_MSG10004_ImportFixedDeclarationResponseService.MapResponseToDeclaration(CastDeclaration(customResponse.Response.Declaration), requestParams.Tenant, false, id, out error, false);
                        fromMehes = true;
                    }


                }



                if (this._MyDeclarationPM == null)
                {
                    LogMessagingUtil.Instance.AppendLine("Can not find declaration" + requestParams.AppicationId);
                    this.MyResponseData.ApplicationID = requestParams.AppicationId;
                    this.MyResponseData.Succeeded = false;
                    this.MyResponseData.UserMessage = "Can not find declaration" + requestParams.AppicationId;
                     return;
                }

           


                if (customResponse.ProceduralFaults != null)
                {
                    var ProceduralFaultDetailsXml_5117 = XmlGenericUtil<UnifreightIIG.Common.MessageLib.ID.ProceduralFaultDetails[]>
                       .SerializeObject(customResponse.ProceduralFaults);

                    var customResponse_8218 = new EV_NG_8218_MSG14100_ProceduralFaultMsg() { };
                    customResponse_8218.ProceduralFaultDetails = XmlGenericUtil<UnifreightIIG.Common.MessageLib.Fault.ProceduralFaultDetails[]>
                        .DeSerializeObject(ProceduralFaultDetailsXml_5117);

                    var ResponseService_8218 = new EV_NG_8218_MSG14100_ProceduralFaultMsgResponseService();
                    ResponseService_8218.Update(customResponse_8218, requestParams);


                    //update id original after create faults
                    var proceduralFaultQueryService = new ProceduralFaultQueryService(requestParams.Tenant);
                    var proceduralFaultUpdateService = new ProceduralFaultUpdateService(context, new Dictionary<string, IContext>(), requestParams.Tenant);
                    foreach (var proceduralFaultItem in customResponse_8218.ProceduralFaultDetails)
                    {
                        string faultId = proceduralFaultQueryService.GetFaultIdByFaultNumber(proceduralFaultItem.proceduralFaultNumber.ToString(), requestParams.Tenant);
                      var  proceduralFaultPM = new ProceduralFaultPM();
 
                        EventContextTagModel myInsertEventContextTagModel = new EventContextTagModel();
                        myInsertEventContextTagModel.MyNotificationPM = new NotificationPM();

                        if (!string.IsNullOrWhiteSpace(faultId))
                        {
                            proceduralFaultPM = proceduralFaultQueryService.GetSingle(faultId, true, false);
                            proceduralFaultPM.ChangeSetOp = ChangeSetOperation.Update;
                            proceduralFaultPM.DeclarationId = _MyDeclarationPM.Id;
                            proceduralFaultUpdateService.Update(proceduralFaultPM, true);
                         }
                    }
                      
                }



                var declarationQueryService = new DeclarationQueryService(_MyDeclarationPM.Tenant);
                _MyDeclarationPMOrg = declarationQueryService.GetSingle(_MyDeclarationPM.AmendmentOriginalDeclartation, true, false);


                string loggingUserId = "";
                UserRepository userRepository = new UserRepository(_MyDeclarationPM.Tenant);
                var user = userRepository.GetSingleUserByCode("MEHES", _MyDeclarationPM.Tenant, true);
                if (user != null)
                {
                    loggingUserId = user.Id;
                }

                EventContextTagModel myUpdateEventContextTagModel = null;

                string key = ProcessLockTableUtil.Instance.GetKey4Declaration(_MyDeclarationPM.Id, requestParams.Tenant);
                using (var disposableToken =
                ProcessLockTableUtil.Instance.GetProcessLockTableDisposable(requestParams.Tenant, true, key, "5117ResponseService.Update")
                    )
                {
 
                foreach (var additionalInformation in customResponse.Response.AdditionalInformation)
                    {
                        switch (additionalInformation.StatementTypeCode.Value)
                        {
                            case "16":
                                {
                                    if (additionalInformation.Content != null)
                                    {
                                      //  additionalInformation.Content.Value = "11";
                                         var paymentOrderQueryService = new PaymentOrderQueryService(context);
                                        var paymentOrderId = paymentOrderQueryService.GetIdByPaymentNumber(additionalInformation.Content.Value, requestParams.Tenant);
                                        if(!string.IsNullOrEmpty(paymentOrderId))
                                        {
                                            PaymentOrderPM paymentOrder = paymentOrderQueryService.GetSingle(paymentOrderId, false, false);

                                            paymentOrder.FirstEntityID =  _MyDeclarationPM.Id;
                                            paymentOrder.ChangeSetOp = ChangeSetOperation.Update;
                                            PaymentOrderUpdateService pOUpdateservice = new PaymentOrderUpdateService(context, new Dictionary<string, IContext>(), requestParams.Tenant);
                                            pOUpdateservice.Update(paymentOrder, true);
                                        }

                                    }
                                    break;
                                }
                            case "29":
                                {
                                    if (additionalInformation.Content != null)
                                        _MyDeclarationPM.AmendmentRemarks += '\n' + additionalInformation.Content.Value;
                                    break;
                                }
                            case "27":
                                {
                                    if (additionalInformation.Content != null)
                                        _MyDeclarationPM.AmendmentRejectionReason =  additionalInformation.Content.Value;
                                    break;

                                }

                            case "32":
                                {
                                    if (additionalInformation.Content != null)
                                    {
                                        switch (additionalInformation.Content.Value)
                                        {
                                            case "1":

                                                var declarationParent = myDeclarationQueryService.GetAcceptDeclarationAmendment(_MyDeclarationPM.AmendmentOriginalDeclartation, requestParams.Tenant);

                                                _MyDeclarationPM.AmendmentDontDisplayInList = false;
                                                _MyDeclarationPM.AmendmentStatus = "3";
                                                UpdateReplacingDeclaration(requestParams, myDeclarationQueryService, myDeclarationUpdateService);
                                                UpdateParentDec(myDeclarationUpdateService, declarationParent);
                                                var amitalEventTracerModel = new Logitude.Customs.BL.TraceEvents.AmitalEventTracerModel()
                                                {
                                                    Tenant = _MyDeclarationPM.Tenant,
                                                    objectTableName = "Customs.Declaration",
                                                    EventCode = "DMA",
                                                    notes = "- תיקון הצהרה אושר" + (_MyDeclarationPMOrg != null ? _MyDeclarationPMOrg.DeclarationNumber : _MyDeclarationPM.DeclarationNumber) + " מספר בקשה - " + _MyDeclarationPM.AmendmentRequestNumber,
                                                    CommunicationLoggingEntityReference = _MyDeclarationPMOrg != null ? _MyDeclarationPMOrg.DeclarationNumber : _MyDeclarationPM.DeclarationNumber,
                                                    EntityId = _MyDeclarationPMOrg != null ? _MyDeclarationPMOrg.Id : _MyDeclarationPM.Id,
                                                    UserId = loggingUserId,

                                                    CommunicationSubject = "FU Status DMA from logitude ",
                                                    MyFUStatus = new AmitalEventTracerModel.FUStatus()
                                                    {
                                                        entname = "CFIFILEM",
                                                        primary_number = _MyDeclarationPM.CustomFileNo,
                                                        status = "new",
                                                        xml_status = "new",
                                                        status_id = "DMA",
                                                        status_DateTime = DateTime.Now,
                                                        comments = "- תיקון הצהרה אושר" + (_MyDeclarationPMOrg != null ? _MyDeclarationPMOrg.DeclarationNumber : _MyDeclarationPM.DeclarationNumber) + " מספר בקשה - " + _MyDeclarationPM.AmendmentRequestNumber,
                                                    }
                                                };
                                                AmitalEventTracer.CreateTraceEvent(amitalEventTracerModel, iscustomUser: true);

                                                myUpdateEventContextTagModel = new EventContextTagModel()
                                                {
                                                    CallProccessID = EventContextTagModel.ProccessEnum.DF_NG_5117_ImportDeclerationAmendmentReplyResponseService,
                                                    EventCode = "DMA",
                                                    EventRemarks = "Declaration Amendment Approved",
                                                    FUStatusRemarks = "- תיקון הצהרה אושר" + (_MyDeclarationPMOrg != null ? _MyDeclarationPMOrg.DeclarationNumber : _MyDeclarationPM.DeclarationNumber) + " מספר בקשה - " + _MyDeclarationPM.AmendmentRequestNumber,
                                                };

                                                break;

                                            case "4":
                                                _MyDeclarationPM.AmendmentStatus = "4";


                                                var myAmitalEventTracerModel2 = new Logitude.Customs.BL.TraceEvents.AmitalEventTracerModel()
                                                {
                                                    Tenant = _MyDeclarationPM.Tenant,
                                                    objectTableName = "Customs.Declaration",
                                                    EventCode = "DMD",
                                                    notes = "תיקון הצהרה נדחה - " + (_MyDeclarationPMOrg != null ? _MyDeclarationPMOrg.DeclarationNumber : _MyDeclarationPM.DeclarationNumber) + " מספר בקשה - " + _MyDeclarationPM.AmendmentRequestNumber,
                                                    CommunicationLoggingEntityReference = _MyDeclarationPMOrg != null ? _MyDeclarationPMOrg.DeclarationNumber : _MyDeclarationPM.DeclarationNumber,
                                                    EntityId = _MyDeclarationPMOrg != null ? _MyDeclarationPMOrg.Id : _MyDeclarationPM.Id,
                                                    UserId = loggingUserId,

                                                    CommunicationSubject = "FU Status DMD from logitude ",
                                                    MyFUStatus = new AmitalEventTracerModel.FUStatus()
                                                    {
                                                        entname = "CFIFILEM",
                                                        primary_number = _MyDeclarationPM.CustomFileNo,
                                                        status = "new",
                                                        xml_status = "new",
                                                        status_id = "DMD",
                                                        status_DateTime = DateTime.Now,
                                                        comments = "תיקון הצהרה נדחה - " + (_MyDeclarationPMOrg != null ? _MyDeclarationPMOrg.DeclarationNumber : _MyDeclarationPM.DeclarationNumber) + " מספר בקשה - " + _MyDeclarationPM.AmendmentRequestNumber,
                                                    }
                                                };

                                                AmitalEventTracer.CreateTraceEvent(myAmitalEventTracerModel2, iscustomUser: true);
                                                  myUpdateEventContextTagModel = new EventContextTagModel()
                                                {
                                                    CallProccessID = EventContextTagModel.ProccessEnum.DF_NG_5117_ImportDeclerationAmendmentReplyResponseService,
                                                    EventCode = "DMD",
                                                    EventRemarks = "Declaration Amendment Denial",
                                                    FUStatusRemarks = "תיקון הצהרה נדחה - " + (_MyDeclarationPMOrg != null ? _MyDeclarationPMOrg.DeclarationNumber : _MyDeclarationPM.DeclarationNumber) + " מספר בקשה - " + _MyDeclarationPM.AmendmentRequestNumber,
                                                };



                                                List<string> currentXmlVersionId = myDeclarationCorrectionsPointerService.GetVersionIdFromCorrectionXML(this._MyDeclarationPM.CorrectionsXml);
                                           
                                                    var customResponseResponseXml = XmlGenericUtil<UnifreightIIG.Common.MessageLib.ID.Response>
                                                        .SerializeObject(customResponse.Response);

                                                    var importDeclarationServiceReferenceResponse = XmlGenericUtil<UnifreightIIG.Common.MessageLib.ID.Response>
                                                        .DeSerializeObject(customResponseResponseXml);
                                                
                                                    List<error> systemMessagesList = new List<error>();
                                                
                                                    this._MyDeclarationPM.CorrectionsXml = myDeclarationCorrectionsPointerService.AnalyzeCorrectionsPointer(this._MyDeclarationPM.CorrectionsXml, importDeclarationServiceReferenceResponse, systemMessagesList, requestParams.Tenant);
                                            

                                                break;
  


                                            case "2":
                                                _MyDeclarationPM.AmendmentStatus = "6";
                                                _MyDeclarationPM.AmendmentDontDisplayInList = false;

                                                 declarationParent = myDeclarationQueryService.GetAcceptDeclarationAmendment(_MyDeclarationPM.AmendmentOriginalDeclartation, requestParams.Tenant);

                                                UpdateParentDec(myDeclarationUpdateService, declarationParent);
                                                UpdateReplacingDeclaration(requestParams, myDeclarationQueryService, myDeclarationUpdateService);


                                                var myAmitalEventTracerModel4 = new Logitude.Customs.BL.TraceEvents.AmitalEventTracerModel()
                                                {
                                                    Tenant = _MyDeclarationPM.Tenant,
                                                    objectTableName = "Customs.Declaration",
                                                    EventCode = "DMP",
                                                    notes = "תיקון הצהרה אושר חלקית - " + ( _MyDeclarationPMOrg != null ? _MyDeclarationPMOrg.DeclarationNumber : _MyDeclarationPM.DeclarationNumber )+ "מספר בקשה - " + _MyDeclarationPM.AmendmentRequestNumber,
                                                    CommunicationLoggingEntityReference = _MyDeclarationPMOrg != null ? _MyDeclarationPMOrg.DeclarationNumber : _MyDeclarationPM.DeclarationNumber,
                                                    EntityId = _MyDeclarationPMOrg != null ? _MyDeclarationPMOrg.Id : _MyDeclarationPM.Id,
                                                    UserId = loggingUserId,

                                                    CommunicationSubject = "FU Status DMP from logitude ",
                                                    MyFUStatus = new AmitalEventTracerModel.FUStatus()
                                                    {
                                                        entname = "CFIFILEM",
                                                        primary_number = _MyDeclarationPM.CustomFileNo,
                                                        status = "new",
                                                        xml_status = "new",
                                                        status_id = "DMP",
                                                        status_DateTime = DateTime.Now,
                                                        comments = "תיקון הצהרה אושר חלקית - " +( _MyDeclarationPMOrg != null ? _MyDeclarationPMOrg.DeclarationNumber : _MyDeclarationPM.DeclarationNumber) + "מספר בקשה - " + _MyDeclarationPM.AmendmentRequestNumber,
                                                    }
                                                };

                                                AmitalEventTracer.CreateTraceEvent(myAmitalEventTracerModel4, iscustomUser: true);
                                                myUpdateEventContextTagModel = new EventContextTagModel()
                                                {
                                                    CallProccessID = EventContextTagModel.ProccessEnum.DF_NG_5117_ImportDeclerationAmendmentReplyResponseService,
                                                    EventCode = "DMP",
                                                    EventRemarks = "Declaration Amendment Partial Approval",
                                                    FUStatusRemarks = "תיקון הצהרה אושר חלקית - " + (_MyDeclarationPMOrg != null ? _MyDeclarationPMOrg.DeclarationNumber : _MyDeclarationPM.DeclarationNumber) + "מספר בקשה - " + _MyDeclarationPM.AmendmentRequestNumber,
                                                };

                                                break;


                                            case "6":
                                                _MyDeclarationPM.AmendmentStatus = "1";
                                                var myAmitalEventTracerModel5 = new Logitude.Customs.BL.TraceEvents.AmitalEventTracerModel()
                                                {
                                                    Tenant = _MyDeclarationPM.Tenant,
                                                    objectTableName = "Customs.Declaration",
                                                    EventCode = "DWR",
                                                    notes = "תיקון הצהרה ממתין לטיפול המכס - " +( _MyDeclarationPMOrg != null ? _MyDeclarationPMOrg.DeclarationNumber : _MyDeclarationPM.DeclarationNumber )+ " מספר בקשה - " + _MyDeclarationPM.AmendmentRequestNumber,
                                                    CommunicationLoggingEntityReference = _MyDeclarationPMOrg != null ? _MyDeclarationPMOrg.DeclarationNumber : _MyDeclarationPM.DeclarationNumber,
                                                    EntityId = _MyDeclarationPMOrg != null ? _MyDeclarationPMOrg.Id : _MyDeclarationPM.Id,
                                                    UserId = loggingUserId,

                                                    CommunicationSubject = "FU Status DWR from logitude ",
                                                    MyFUStatus = new AmitalEventTracerModel.FUStatus()
                                                    {
                                                        entname = "CFIFILEM",
                                                        primary_number = _MyDeclarationPM.CustomFileNo,
                                                        status = "new",
                                                        xml_status = "new",
                                                        status_id = "DWR",
                                                        status_DateTime = DateTime.Now,
                                                        comments = "תיקון הצהרה ממתין לטיפול המכס - " +( _MyDeclarationPMOrg != null ? _MyDeclarationPMOrg.DeclarationNumber : _MyDeclarationPM.DeclarationNumber) + " מספר בקשה - " + _MyDeclarationPM.AmendmentRequestNumber,
                                                    }
                                                };

                                                AmitalEventTracer.CreateTraceEvent(myAmitalEventTracerModel5, iscustomUser: true);
                                                myUpdateEventContextTagModel = new EventContextTagModel()
                                                {
                                                    CallProccessID = EventContextTagModel.ProccessEnum.DF_NG_5117_ImportDeclerationAmendmentReplyResponseService,
                                                    EventCode = "DWR",
                                                    EventRemarks = "Declaration Amendment Waiting for customs response",
                                                    FUStatusRemarks = "תיקון הצהרה ממתין לטיפול המכס - " + (_MyDeclarationPMOrg != null ? _MyDeclarationPMOrg.DeclarationNumber : _MyDeclarationPM.DeclarationNumber) + " מספר בקשה - " + _MyDeclarationPM.AmendmentRequestNumber,
                                                };

                                                UpdateReplacingDeclaration(requestParams, myDeclarationQueryService, myDeclarationUpdateService);

                                                break;
                                        };

                                    }


                                    break;
                                }
                        }
                   }
 
                    if (customResponse.Response.Error != null && (!new string[]{ "3","6"}.Contains( _MyDeclarationPM.AmendmentStatus)))
                    {
                        DeclarationErrorPointerService mydDclarationErrorPointerService = new DeclarationErrorPointerService();

                        this._MyDeclarationPM.AmendmentErrorXml = mydDclarationErrorPointerService.AnalyzeErrorPionter(CastError(customResponse.Response.Error), _MyDeclarationPM, WCOTypeEnum.WCO);

                    }


                    if (myUpdateEventContextTagModel != null)
                        this._MyDeclarationPM.CurrentContextTag = myUpdateEventContextTagModel;
                    if (fromMehes)
                    {
                        _MyDeclarationPM.AmendmentCorrectedByUserId = loggingUserId;
                        _MyDeclarationPM.AmendmentissueDate = DateTime.ParseExact(customResponse.Response.Declaration.IssueDateTime, "yyyy-MM-ddTHH:mm:ss", null);

 
                    }
                    if(_MyDeclarationPM.AmendmentStatus == "3"  || _MyDeclarationPM.AmendmentStatus == "6")
                    {
                        _MyDeclarationPM.DeclarationStatusTypeCode = customResponse.Response.Status.NameCode.Value;
                        _MyDeclarationPM.PaymentDate = _MyDeclarationPMOrg.PaymentDate;
                        if (fromMehes)
                        {
                            var myAmitalEventTracerModel = new Logitude.Customs.BL.TraceEvents.AmitalEventTracerModel()
                            {
                                Tenant = _MyDeclarationPM.Tenant,
                                objectTableName = "Customs.Declaration",
                                EventCode = "DCH",
                                notes = "-  בוצע תיקון הצהרה" + (_MyDeclarationPMOrg != null ? _MyDeclarationPMOrg.DeclarationNumber : _MyDeclarationPM.DeclarationNumber),
                                CommunicationLoggingEntityReference = _MyDeclarationPMOrg != null ? _MyDeclarationPMOrg.DeclarationNumber : _MyDeclarationPM.DeclarationNumber,
                                EntityId = _MyDeclarationPMOrg != null ? _MyDeclarationPMOrg.Id : _MyDeclarationPM.Id,
                                UserId = loggingUserId,

                                CommunicationSubject = "FU Status DCH from logitude ",
                                MyFUStatus = new AmitalEventTracerModel.FUStatus()
                                {
                                    entname = "CFIFILEM",
                                    primary_number = _MyDeclarationPM.CustomFileNo,
                                    status = "new",
                                    xml_status = "new",
                                    status_id = "DCH",
                                    status_DateTime = DateTime.Now,
                                    comments = "-  בוצע תיקון הצהרה" + (_MyDeclarationPMOrg != null ? _MyDeclarationPMOrg.DeclarationNumber : _MyDeclarationPM.DeclarationNumber),
                                }
                            };
                            AmitalEventTracer.CreateTraceEvent(myAmitalEventTracerModel, iscustomUser: true);


                            myUpdateEventContextTagModel = new EventContextTagModel()
                            {
                                CallProccessID = EventContextTagModel.ProccessEnum.DF_NG_5117_ImportDeclerationAmendmentReplyResponseService,
                                EventCode = "DCH",
                                EventRemarks = "Declaration Changed By Customs",
                                FUStatusRemarks = "בוצע תיקון הצהרה" + (_MyDeclarationPMOrg != null ? _MyDeclarationPMOrg.DeclarationNumber : _MyDeclarationPM.DeclarationNumber),
                            };

                        }


                        List<string> currentXmlVersionId = myDeclarationCorrectionsPointerService.GetVersionIdFromCorrectionXML(this._MyDeclarationPM.CorrectionsXml);
                        if ((customResponse.Response.Declaration != null) && (currentXmlVersionId == null || !currentXmlVersionId.Contains(customResponse.Response.Declaration.DMExtensions.VersionID.Value)))
                        {

                            var customResponseResponseXml = XmlGenericUtil<UnifreightIIG.Common.MessageLib.ID.Response>
                                .SerializeObject(customResponse.Response);

                            var importDeclarationServiceReferenceResponse = XmlGenericUtil<UnifreightIIG.Common.MessageLib.ID.Response>
                                .DeSerializeObject(customResponseResponseXml);
                            ////5117 5117 5117 5117 5117
                            List<error> systemMessagesList = new List<error>();
                            if (customResponse.CollateralRequests != null && customResponse.CollateralRequests.Count() > 0) // Update Declaration Correction Pointer
                            {
                                foreach (var collateralRequestItem in customResponse.CollateralRequests)
                                {
                                    var myError = new error();
                                    myError.ListVersionID = "A";
                                    myError.MessageError = "המשוב להצהרה כולל דרישה לבטוחה " + " - מספר בטוחה " + collateralRequestItem.collateralRequestNumber;
                                    systemMessagesList.Add(myError);
                                }
                            }
                            this._MyDeclarationPM.CorrectionsXml = myDeclarationCorrectionsPointerService.AnalyzeCorrectionsPointer(this._MyDeclarationPM.CorrectionsXml, importDeclarationServiceReferenceResponse, systemMessagesList, requestParams.Tenant);
                        }
                        if (_MyDeclarationPM.UserNotes == "LoadTestOnProgress")
                        {
                            _MyDeclarationPM.UserNotes = "LoadTest";
                        }

 
                        this._MyDeclarationPM.ChangeSetOp = ChangeSetOperation.Update;
                        myDeclarationUpdateService.Update(this._MyDeclarationPM, true);

             
                   



                        bool sendDeclarationPrintSync = false;
                        if (sendDeclarationPrintSync)
                        {
                            SendDeclarationPrintSync(requestParams);
                        }
                        else
                        {
                            SendDeclarationPrint(requestParams);
                        }

                        if (customResponse.CollateralRequests != null)
                        {
                            LogMessagingUtil.Instance.AppendLine("ImportDeclarationAmendmentReplyMsg: Create Collateral");
                            var requestXml = XmlGenericUtil<UnifreightIIG.Common.MessageLib.ID.CollateralRequestDetails[]>.SerializeObject(customResponse.CollateralRequests);
                            var collateralArry = XmlGenericUtil<UnifreightIIG.Common.MessageLib.Collateral.CollateralRequestDetails[]>.DeSerializeObject(requestXml);

                            COLT_NG_8211_MSG10040_CollateralRequestMsg myCOLT_NG_8211_MSG10040_CollateralRequestMsg = new COLT_NG_8211_MSG10040_CollateralRequestMsg();
                            var responseContentHeader = customResponse.GetResponseContentHeader();
                            myCOLT_NG_8211_MSG10040_CollateralRequestMsg.ResponseContentHeader = new UnifreightIIG.Common.MessageLib.Collateral.ResponseContentHeader();
                            if (responseContentHeader != null)
                            {
                                myCOLT_NG_8211_MSG10040_CollateralRequestMsg.ResponseContentHeader.ApplicationID = responseContentHeader.ApplicationID;
                                myCOLT_NG_8211_MSG10040_CollateralRequestMsg.ResponseContentHeader.Remark = responseContentHeader.Remark;
                                myCOLT_NG_8211_MSG10040_CollateralRequestMsg.ResponseContentHeader.TransmitionDateTime = responseContentHeader.TransmitionDateTime;
                            }
                            myCOLT_NG_8211_MSG10040_CollateralRequestMsg.CollateralRequestDetails = collateralArry;
                            var xml = XmlGenericUtil<UnifreightIIG.Common.MessageLib.Collateral.COLT_NG_8211_MSG10040_CollateralRequestMsg>
                                .SerializeObject(myCOLT_NG_8211_MSG10040_CollateralRequestMsg);

                            var ser = XmlGenericUtil<UnifreightIIG.Common.MessageLib.Collateral.COLT_NG_8211_MSG10040_CollateralRequestMsg>.DeSerializeObject(xml);
                            var DF_MSG10040_CollateralRequestMsgResponseService = new DF_8211_CollateralRequestMsgResponseService();
                            DF_MSG10040_CollateralRequestMsgResponseService.Update(ser, requestParams);
                        }

                    }

                    else
                    {

                        this._MyDeclarationPM.ChangeSetOp = ChangeSetOperation.Update;
                        myDeclarationUpdateService.Update(this._MyDeclarationPM, true);

                    }
                    if (customResponse.Response.Declaration != null)
                    {
                        DF_NG_2754_MSG10004_ImportDeclarationResponseService dF_NG_2754_MSG10004_ImportDeclarationResponseService = new DF_NG_2754_MSG10004_ImportDeclarationResponseService();

                        UnifreightIIG.Common.ImportDeclarationServiceReference.DF_NG_2754_MSG10004_ImportDeclarationResponse dec_2754 = new UnifreightIIG.Common.ImportDeclarationServiceReference.DF_NG_2754_MSG10004_ImportDeclarationResponse();
                        dec_2754.Response = new UnifreightIIG.Common.ImportDeclarationServiceReference.Response();
                        dec_2754.Response.Declaration = CastDeclaration(customResponse.Response.Declaration);
                        dec_2754.Response.Status = CastStatus(customResponse.Response.Status);


                        if (customResponse.Response.Error != null)
                            dec_2754.Response.Error = CastError(customResponse.Response.Error);


                        GenericRequestParams requestParams_2754 = new GenericRequestParams
                        {
                            Tenant = _MyDeclarationPM.Tenant,
                            AppicationId = _MyDeclarationPM.Id,
                            ResponseName = "5117"
                        };

                        dF_NG_2754_MSG10004_ImportDeclarationResponseService.Update(dec_2754, requestParams_2754);

                    }

                    //}

                    if (customResponse.AmendmentDocumentDetails != null) // Create Document 
                    {
                        LogMessagingUtil.Instance.AppendLine("ConstraintApprovalDecision: Create Document");

                        foreach (var documentItem in customResponse.AmendmentDocumentDetails)
                        {

                            //Get Document Detail
                            var documentXml = new UnifreightIIG.Common.MessageLib.Ransom.RequiredDocumentDetails();
                            documentXml.documentID = documentItem.RequiredDocumentDetails.documentID;
                            documentXml.remarks = documentItem.RequiredDocumentDetails.remarks;
                            documentXml.requiredDocumentMessageType = documentItem.RequiredDocumentDetails.requiredDocumentMessageType;
                            documentXml.typeID = documentItem.RequiredDocumentDetails.typeID.ToString();
                            //Get Entity Details
                            RequiredDocumentRequestParams documentRequestParams = new RequiredDocumentRequestParams();
                            documentRequestParams.Tenant = requestParams.Tenant;
                            documentRequestParams.ParentEntityCode = "Declaration";
                            documentRequestParams.ParentEntityId = _MyDeclarationPM.Id;

                            var listConnectedEntity = new List<UnifreightIIG.Common.MessageLib.Ransom.ConnectedEntity>();
                            var entityXml = new UnifreightIIG.Common.MessageLib.Ransom.ConnectedEntity();
                            entityXml.entityType = 1055;
                            //    entityXml.entityIdKey1 = customResponse..LeadDocumentIDNum;
                            listConnectedEntity.Add(entityXml);

                            VAL_NG_8227_MSG_520_RequiredDocumentMessage myVAL_NG_8227_MSG_520_RequiredDocumentMessage = new VAL_NG_8227_MSG_520_RequiredDocumentMessage();
                            myVAL_NG_8227_MSG_520_RequiredDocumentMessage.RequiredDocumentDetails = documentXml;

                            myVAL_NG_8227_MSG_520_RequiredDocumentMessage.RelatedEntity = listConnectedEntity.ToArray();
                            var xml = XmlGenericUtil<UnifreightIIG.Common.MessageLib.Ransom.VAL_NG_8227_MSG_520_RequiredDocumentMessage>
                               .SerializeObject(myVAL_NG_8227_MSG_520_RequiredDocumentMessage);

                            var ser = XmlGenericUtil<UnifreightIIG.Common.MessageLib.Ransom.VAL_NG_8227_MSG_520_RequiredDocumentMessage>.DeSerializeObject(xml);
                            var myVAL_NG_8227_MSG_520_RequiredDocumentMessageResponseService = new VAL_NG_8227_MSG_520_RequiredDocumentMessageResponseService();
                            myVAL_NG_8227_MSG_520_RequiredDocumentMessageResponseService.Update(ser, documentRequestParams);
                        }
                    }


                }


                this.MyResponseData.ApplicationID = requestParams.AppicationId;
                this.MyResponseData.Succeeded = true;
                this.MyResponseData.HasException = false;
                this.MyResponseData.UserMessage = "מענה לתיקון הצהרה  " + this._MyDeclarationPM.DeclarationNumber + " נקלט בהצלחה";

                this.MyRequestSheetParam = new RequestSheetParam();
                this.MyRequestSheetParam.CustomFileNo = this._MyDeclarationPM.CustomFileNo;
                this.MyRequestSheetParam.ObjectTableId1 = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
                this.MyRequestSheetParam.EntityId1 = this._MyDeclarationPM.Id;
                this.MyRequestSheetParam.RequestDescription = "מענה לתיקון הצהרה  " + this._MyDeclarationPM.DeclarationNumber;

                requestParams.AppicationId = _MyDeclarationPM.Id;// myDeclarationQueryService.GetIdByDeclarationNumber(_MyDeclarationPM.Id, requestParams.Tenant);
                //if (string.IsNullOrWhiteSpace(requestParams.AppicationId))
                //{
                //     requestParams.AppicationId = myDeclarationQueryService.GetIdByExternalDeclarationNumber(customResponse.Response.Declaration.DMExtensions.ExternalDeclarationID.Value, requestParams.Tenant);
                //}


            }



            else
            {

                requestParams.AppicationId = myDeclarationQueryService.GetIdByDeclarationNumber(customResponse.Response.Declaration.ID.Value, requestParams.Tenant);
                if (string.IsNullOrWhiteSpace(requestParams.AppicationId))
                {
                     requestParams.AppicationId = myDeclarationQueryService.GetIdByExternalDeclarationNumber(customResponse.Response.Declaration.DMExtensions.ExternalDeclarationID.Value, requestParams.Tenant);
                }

                var declarationNumber = "";
                if (customResponse.Response.Declaration != null && customResponse.Response.Declaration.ID != null && customResponse.Response.Declaration.ID.Value != null && customResponse.Response.Declaration.ID.Value.Substring(2, 2) == "99")
                {
                    _MyDeclarationPM = myDeclarationUpdateService.GetSertByConvertedDeclarationNumber(customResponse.Response.Declaration.ID.Value, requestParams.Tenant);
                 }
                else
                {
                    if (string.IsNullOrWhiteSpace(requestParams.AppicationId))
                    {
                        LogMessagingUtil.Instance.AppendLine("Can not find declaration- DeclarationNumber: " + customResponse.Response.Declaration.ID.Value + " ExternalDeclarationNumber: " + customResponse.Response.Declaration.DMExtensions.ExternalDeclarationID.Value);
                        this.MyResponseData.ApplicationID = requestParams.AppicationId;
                        this.MyResponseData.Succeeded = false;
                        this.MyResponseData.UserMessage = "Can not find declaration- DeclarationNumber: " + customResponse.Response.Declaration.ID.Value + " ExternalDeclarationNumber: " + customResponse.Response.Declaration.DMExtensions.ExternalDeclarationID.Value;
                        return;
                    }
                 }
                declarationNumber = customResponse.Response.Declaration.ID.Value;

                 string key = ProcessLockTableUtil.Instance.GetKey4Declaration(declarationNumber, requestParams.Tenant);
                using (var disposableToken =
                     ProcessLockTableUtil.Instance.GetProcessLockTableDisposable(requestParams.Tenant, true, key, "5117ResponseService.Update")
                    )
                {
                    if (!(customResponse.Response.Declaration != null && customResponse.Response.Declaration.ID != null && customResponse.Response.Declaration.ID.Value != null && customResponse.Response.Declaration.ID.Value.Substring(2, 2) == "99"))
                    {
                        this._MyDeclarationPM = myDeclarationQueryService.GetSingle(requestParams.AppicationId, true, false);
                    }

                    if (this._MyDeclarationPM == null)
                    {
                        LogMessagingUtil.Instance.AppendLine("Can not find declaration" + requestParams.AppicationId);
                        this.MyResponseData.ApplicationID = requestParams.AppicationId;
                        this.MyResponseData.Succeeded = false;
                        this.MyResponseData.UserMessage = "Can not find declaration" + requestParams.AppicationId;
                        return;
                    }

                    var myUpdateEventContextTagModel = new EventContextTagModel()
                    {
                        CallProccessID = EventContextTagModel.ProccessEnum.DF_NG_5117_ImportDeclerationAmendmentReplyResponseService,
                        EventCode = "DCH",
                        EventRemarks = "Declaration Changed By Customs",
                        FUStatusRemarks = "בוצע תיקון הצהרה" + ( _MyDeclarationPMOrg != null ? _MyDeclarationPMOrg.DeclarationNumber : _MyDeclarationPM.DeclarationNumber),
                    };

                    this._MyDeclarationPM.CurrentContextTag = myUpdateEventContextTagModel;
                    List<string> currentXmlVersionId = myDeclarationCorrectionsPointerService.GetVersionIdFromCorrectionXML(this._MyDeclarationPM.CorrectionsXml);
                    if (currentXmlVersionId == null || !currentXmlVersionId.Contains(customResponse.Response.Declaration.DMExtensions.VersionID.Value))
                    {

                        var customResponseResponseXml = XmlGenericUtil<UnifreightIIG.Common.MessageLib.ID.Response>
                            .SerializeObject(customResponse.Response);

                        var importDeclarationServiceReferenceResponse = XmlGenericUtil<UnifreightIIG.Common.MessageLib.ID.Response>
                            .DeSerializeObject(customResponseResponseXml);
                        ////5117 5117 5117 5117 5117
                        List<error> systemMessagesList = new List<error>();
                        if (customResponse.CollateralRequests != null && customResponse.CollateralRequests.Count() > 0) // Update Declaration Correction Pointer
                        {
                            foreach (var collateralRequestItem in customResponse.CollateralRequests)
                            {
                                var myError = new error();
                                myError.ListVersionID = "A";
                                myError.MessageError = "המשוב להצהרה כולל דרישה לבטוחה " + " - מספר בטוחה " + collateralRequestItem.collateralRequestNumber;
                                systemMessagesList.Add(myError);
                            }
                        }
                        this._MyDeclarationPM.CorrectionsXml = myDeclarationCorrectionsPointerService.AnalyzeCorrectionsPointer(this._MyDeclarationPM.CorrectionsXml, importDeclarationServiceReferenceResponse, systemMessagesList, requestParams.Tenant);
                    }
                    if (_MyDeclarationPM.UserNotes == "LoadTestOnProgress")
                    {
                        _MyDeclarationPM.UserNotes = "LoadTest";
                    }

                    //<--- Yuval Chalup 18.10.2016 TASK-23113
                    if (customResponse.ProceduralFaults != null)
                    {
                        //Transfer ProceduralFaultDetails of 5117 to ProceduralFaultDetails of 8218
                        var ProceduralFaultDetailsXml_5117 = XmlGenericUtil<UnifreightIIG.Common.MessageLib.ID.ProceduralFaultDetails[]>
                            .SerializeObject(customResponse.ProceduralFaults);

                        var customResponse_8218 = new EV_NG_8218_MSG14100_ProceduralFaultMsg() { };
                        customResponse_8218.ProceduralFaultDetails = XmlGenericUtil<UnifreightIIG.Common.MessageLib.Fault.ProceduralFaultDetails[]>
                            .DeSerializeObject(ProceduralFaultDetailsXml_5117);

                        var ResponseService_8218 = new EV_NG_8218_MSG14100_ProceduralFaultMsgResponseService();
                        ResponseService_8218.Update(customResponse_8218, requestParams);
                    }
                    //Yuval Chalup 18.10.2016 TASK-23113 --->

                    this._MyDeclarationPM.ChangeSetOp = ChangeSetOperation.Update;
                    myDeclarationUpdateService.Update(this._MyDeclarationPM, true);

                    this.MyResponseData.ApplicationID = requestParams.AppicationId;
                    this.MyResponseData.Succeeded = true;
                    this.MyResponseData.HasException = false;
                    this.MyResponseData.UserMessage = "מענה לתיקון הצהרה  " + this._MyDeclarationPM.DeclarationNumber + " נקלט בהצלחה";

                    this.MyRequestSheetParam = new RequestSheetParam();
                    this.MyRequestSheetParam.CustomFileNo = this._MyDeclarationPM.CustomFileNo;
                    this.MyRequestSheetParam.ObjectTableId1 = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
                    this.MyRequestSheetParam.EntityId1 = this._MyDeclarationPM.Id;
                    this.MyRequestSheetParam.RequestDescription = "מענה לתיקון הצהרה  " + this._MyDeclarationPM.DeclarationNumber;

                    bool sendDeclarationPrintSync = false;// ConfigurationManager.AppSettings["20180307.5117SendDeclarationPrintSync"] =="1";
                    if (sendDeclarationPrintSync)
                    {
                        SendDeclarationPrintSync(requestParams);
                    }
                    else
                    {
                        SendDeclarationPrint(requestParams);
                    }

                    if (customResponse.CollateralRequests != null) // Create Collateral
                    {
                        LogMessagingUtil.Instance.AppendLine("ImportDeclarationAmendmentReplyMsg: Create Collateral");
                        var requestXml = XmlGenericUtil<UnifreightIIG.Common.MessageLib.ID.CollateralRequestDetails[]>.SerializeObject(customResponse.CollateralRequests);
                        var collateralArry = XmlGenericUtil<UnifreightIIG.Common.MessageLib.Collateral.CollateralRequestDetails[]>.DeSerializeObject(requestXml);

                        COLT_NG_8211_MSG10040_CollateralRequestMsg myCOLT_NG_8211_MSG10040_CollateralRequestMsg = new COLT_NG_8211_MSG10040_CollateralRequestMsg();
                        var responseContentHeader = customResponse.GetResponseContentHeader();
                        myCOLT_NG_8211_MSG10040_CollateralRequestMsg.ResponseContentHeader = new UnifreightIIG.Common.MessageLib.Collateral.ResponseContentHeader();
                        if (responseContentHeader != null)
                        {
                            myCOLT_NG_8211_MSG10040_CollateralRequestMsg.ResponseContentHeader.ApplicationID = responseContentHeader.ApplicationID;
                            myCOLT_NG_8211_MSG10040_CollateralRequestMsg.ResponseContentHeader.Remark = responseContentHeader.Remark;
                            myCOLT_NG_8211_MSG10040_CollateralRequestMsg.ResponseContentHeader.TransmitionDateTime = responseContentHeader.TransmitionDateTime;
                        }
                        myCOLT_NG_8211_MSG10040_CollateralRequestMsg.CollateralRequestDetails = collateralArry;
                        var xml = XmlGenericUtil<UnifreightIIG.Common.MessageLib.Collateral.COLT_NG_8211_MSG10040_CollateralRequestMsg>
                            .SerializeObject(myCOLT_NG_8211_MSG10040_CollateralRequestMsg);

                        var ser = XmlGenericUtil<UnifreightIIG.Common.MessageLib.Collateral.COLT_NG_8211_MSG10040_CollateralRequestMsg>.DeSerializeObject(xml);
                        var DF_MSG10040_CollateralRequestMsgResponseService = new DF_8211_CollateralRequestMsgResponseService();
                        DF_MSG10040_CollateralRequestMsgResponseService.Update(ser, requestParams);
                    }

                }

            }
        }

        private void UpdateParentDec(DeclarationUpdateService myDeclarationUpdateService, DeclarationPM declarationParent)
        {
            if (declarationParent != null)
            {
                _MyDeclarationPM.DeclarationNumber = declarationParent.DeclarationNumber;
                declarationParent.AmendmentDontDisplayInList = true;
                declarationParent.DeclarationNumber = null;


                declarationParent.ChangeSetOp = ChangeSetOperation.Update;
                myDeclarationUpdateService.Update(declarationParent, true);



            }
        }

        private void UpdateReplacingDeclaration(GenericRequestParams requestParams, DeclarationQueryService myDeclarationQueryService, DeclarationUpdateService myDeclarationUpdateService)
        {
            if (!string.IsNullOrEmpty(_MyDeclarationPM.ReplacingRepairRequest))
            {
                var declarationReplacing = myDeclarationQueryService.GetDeclarationAmendmentByIdAndAmendmentNo(requestParams.Tenant, _MyDeclarationPM.AmendmentOriginalDeclartation, _MyDeclarationPM.ReplacingRepairRequest);
                declarationReplacing.AmendmentStatus = "7";
                declarationReplacing.ChangeSetOp = ChangeSetOperation.Update;
                myDeclarationUpdateService.Update(declarationReplacing, true);

            }
        }


        //        private void UpdateDeclaration(Response response)
        //        {
        //            Declaration declaration = response.Declaration;
        //            var context = CustomContext.GetContext(_MyDeclarationPM.Tenant);
        //            var myQueryService = new DeclarationQueryService(context);
        //            var myDeclarationUpdateService = new DeclarationUpdateService(context, new Dictionary<string, IContext>(), _MyDeclarationPM.Tenant);
        //            var mySupplierInvoiceItemsTaxUpdateService = new SupplierInvoiceItemsTaxUpdateService(context, new Dictionary<string, IContext>(), _MyDeclarationPM.Tenant);
        //            var mySupplierInvoiceItemVehicleModUpdateService = new SupplierInvoiceItemVehicleModUpdateService(context, new Dictionary<string, IContext>(), _MyDeclarationPM.Tenant); // moran 20.10.15 - Task 17209 
        //            var mySupplierInvoiceItemModVehicleUpdateService = new SupplierInvoiceItemModVehicleUpdateService(context, new Dictionary<string, IContext>(), _MyDeclarationPM.Tenant); // moran 24.11.15 - Task 17424 
        //            //var mySupplierInvoiceItemsTaxesModificationUpdateService = new SupplierInvoiceItemsTaxesModUpdateService(context, new Dictionary<string, IContext>(), requestParams.Tenant);
        //            var myDeclarationTaxUpdateService = new DeclarationTaxUpdateService(context, new Dictionary<string, IContext>(), _MyDeclarationPM.Tenant);
        //            var myDeclarationConstraintUpdateService = new DeclarationConstraintUpdateService(context, new Dictionary<string, IContext>(), _MyDeclarationPM.Tenant);
        //            var myDeclarationPaymentQueryService = new DeclarationPaymentQueryService(context);

        //            DeclarationErrorPointerService mydDclarationErrorPointerService = new DeclarationErrorPointerService();



        //            var swGetSingle = Stopwatch.StartNew();
        //            myQueryService.LoadSupplierInvoicesItemsParentsOnly = true;
        //            LogMessagingUtil.Instance.AppendLine("myQueryService.GetSingle:Took:" + swGetSingle.ElapsedMilliseconds);



        //            if (_MyDeclarationPM.IsCourierDeclaration && this._MyDeclarationPM.PaymentDate.HasValue)
        //            {
        //                if (response.Status.NameCode.Value == "13")
        //                {
        //                    // Clear Fields
        //                    _MyDeclarationPM.DeclarationStatusTypeCode = response.Status.NameCode.Value;
        //                    _MyDeclarationPM.PaymentDate = null;
        //                    _MyDeclarationPM.PaymentOrderNumber = null;
        //                    _MyDeclarationPM.PaymentStatusCode = null;
        //                    _MyDeclarationPM.CourierCustomStatusCode = null;
        //                    _MyDeclarationPM.CourierSuspentionCode = null;
        //                    _MyDeclarationPM.CourierSuspentionReasonCode = null;

        //                    // Delete Payment
        //                    var mydeclarationPaymentQueryService = new DeclarationPaymentQueryService(context);
        //                    var declarationPaymentPM = mydeclarationPaymentQueryService.GetSingle(_MyDeclarationPM.Id, true, false);
        //                    if (declarationPaymentPM != null)
        //                    {
        //                        declarationPaymentPM.ChangeSetOp = ChangeSetOperation.Delete;
        //                        if (declarationPaymentPM.DeclarationPaymentMethods.Any())
        //                        {
        //                            foreach (var item in declarationPaymentPM.DeclarationPaymentMethods)
        //                            {
        //                                item.ChangeSetOp = ChangeSetOperation.Delete;
        //                            }
        //                        }
        //                        DeclarationPaymentUpdateService declarationPaymentUpdateService = new DeclarationPaymentUpdateService(context, new Dictionary<string, IContext>(), _MyDeclarationPM.Tenant);
        //                        declarationPaymentUpdateService.Update(declarationPaymentPM, true);
        //                    }

        //                    // Delete Status
        //                    DeclarationUpdateService.DelDeclarationStatus(_MyDeclarationPM, "", "RSH");
        //                }
        //            }


        //            if (this._MyDeclarationPM.IsConvertedDeclaration) // Mirit 24/01/16 19918
        //            {


        //                return;
        //            }


        //            float oldVersionId;
        //            float.TryParse(_MyDeclarationPM.VersionId, out oldVersionId);
        //            if (string.IsNullOrWhiteSpace(_MyDeclarationPM.VersionId))
        //            {
        //                oldVersionId = 0.1F;
        //            }
        //            var declarationPaymentsPM = myDeclarationPaymentQueryService.GetSingle(_MyDeclarationPM.Id, true, false);


        //            if (_MyDeclarationPM.DepositionStatusCode == "R") _MyDeclarationPM.DepositionStatusCode = null;
        ////            if (response.Error != null)
        ////            {
        ////                string userMessage = "";
        ////                this._MyDeclarationPM.MarkAsChanged = false;
        ////                var swErrosXml = Stopwatch.StartNew();

        ////                foreach (UnifreightIIG.Common.ImportDeclarationServiceReference.Exception exception in response.Error)
        ////                {
        ////                    if (!string.IsNullOrWhiteSpace(userMessage))
        ////                    {
        ////                        userMessage = userMessage + @"
        ////";
        ////                    }
        ////                    userMessage = userMessage + exception.ExeptionDescription;
        ////                    this._MyDeclarationPM.ErrosXml = mydDclarationErrorPointerService.AddDeclarationException(this._MyDeclarationPM.ErrosXml, "Buisness", exception, true);
        ////                    LogMessagingUtil.Instance.AppendLine("ErrosXml:Took:" + swErrosXml.ElapsedMilliseconds);
        ////                    switch (exception.ExeptionType)
        ////                    {
        ////                        case 2794:
        ////                            {
        ////                                this._MyDeclarationPM.DeclarationNumber = exception.ExceptionParms.FirstOrDefault();
        ////                                if (!string.IsNullOrWhiteSpace(userMessage))
        ////                                {
        ////                                    userMessage = userMessage + @"
        ////";
        ////                                }
        ////                                userMessage = userMessage + "עודכן מספר ההצהרה לפי רשומת הסוכן - יש לשדר את ההצהרה מחדש";
        ////                                break;
        ////                            }
        ////                        case 1501:
        ////                            {
        ////                                UpdateUnifreightEvent("MPOA", requestParams.LoggingUserId);
        ////                                userMessage = userMessage + "חסר יפוי כח";
        ////                                break;
        ////                            }
        ////                        case 4589:
        ////                            {
        ////                                UpdateUnifreightEvent("MID", requestParams.LoggingUserId);
        ////                                userMessage = userMessage + "חסר תצהיר יבואן";
        ////                                if (string.IsNullOrWhiteSpace(_MyDeclarationPM.DepositionStatusCode)) _MyDeclarationPM.DepositionStatusCode = "R";
        ////                                break;
        ////                            }
        ////                        case 2244:
        ////                            {
        ////                                UpdateUnifreightEvent("IDE", requestParams.LoggingUserId);
        ////                                userMessage = userMessage + "תצהיר יבואן עומד לפוג";
        ////                                break;
        ////                            }
        ////                    }
        ////                }

        ////            }


        //                if (_MyDeclarationPM.UserNotes == "LoadTestOnProgress")
        //                {
        //                    _MyDeclarationPM.UserNotes = "LoadTest";
        //                }

        //                this._MyDeclarationPM.ChangeSetOp = ChangeSetOperation.Update;
        //                this._MyDeclarationPM.CurrentContextTag = Logitude.Customs.BL.EntityUpdateServices.DeclarationUpdateService.UpdateIIGExcptionConst;
        //                myDeclarationUpdateService.Update(this._MyDeclarationPM, true);

        //                if (this._MyDeclarationPM.IsCourierDeclaration)
        //                {

        //                    var calculateDeclarationCourierStatus = new CalculateDeclarationCourierStatus(this._MyDeclarationPM);
        //                    calculateDeclarationCourierStatus.Update(
        //                        (currentDeclarationCourierStatusPM) =>
        //                        {

        //                            currentDeclarationCourierStatusPM.CourierDeclarationStatusCode = "X";
        //                        });
        //                }




        //            //if (response == null)
        //            //{
        //            //    if (customResponse.ResponseContentHeader.Exception == null)
        //            //    {
        //            //        string text = null;
        //            //        if (String.IsNullOrWhiteSpace(customResponse.ResponseContentHeader.Remark))
        //            //        {
        //            //            text = "No Declaration details in the Response " + requestParams.AppicationId;
        //            //            this.MyResponseData.UserMessage = text;
        //            //            LogMessagingUtil.Instance.AppendLine(text);
        //            //        }
        //            //        else
        //            //        {
        //            //            text = "No Declaration details in the Response " + customResponse.ResponseContentHeader.Remark + requestParams.AppicationId;
        //            //            this.MyResponseData.UserMessage = text;
        //            //            LogMessagingUtil.Instance.AppendLine(text);
        //            //        }
        //            //        this.MyResponseData.ApplicationID = requestParams.AppicationId; //Yuval Chalup 28.05.2015 TASK-13252+13509
        //            //        return;
        //            //    }
        //            //}

        //            LogMessagingUtil.Instance.AppendLine("Analyze declaration response" + _MyDeclarationPM.Id);
        //           var _FastDelete = true;
        //            var sw = Stopwatch.StartNew();
        //            if (_FastDelete)
        //            {

        //                var myDeclarationKeys = new DeclarationKeys { Id = _MyDeclarationPM.Id };
        //                myDeclarationTaxUpdateService.FastDeleteComposition(myDeclarationKeys);
        //                mySupplierInvoiceItemVehicleModUpdateService.FastDeleteComposition(myDeclarationKeys);
        //                mySupplierInvoiceItemsTaxUpdateService.FastDeleteComposition(myDeclarationKeys);
        //                mySupplierInvoiceItemModVehicleUpdateService.FastDeleteComposition(myDeclarationKeys);
        //                 (context as DbContextBase).SaveChanges();
        //                context = CustomContext.GetContext(_MyDeclarationPM.Tenant);
        //                myDeclarationUpdateService = new DeclarationUpdateService(context, new Dictionary<string, IContext>(), _MyDeclarationPM.Tenant);

        //            }
        //            else
        //            {
        //                DeleteDeclarationTaxes(myDeclarationTaxUpdateService);
        //                DeleteSupplierInvoiceItemsTaxes(mySupplierInvoiceItemsTaxUpdateService);
        //                DeleteSupplierInvoiceItemsVehicleMods(mySupplierInvoiceItemVehicleModUpdateService);  
        //                DeleteSupplierInvoiceItemsModVehicles(mySupplierInvoiceItemModVehicleUpdateService);  

        //            }
        //            LogMessagingUtil.Instance.AppendLine("IsFastDelete:" + _FastDelete.ToString() + ",Took :" + sw.ElapsedMilliseconds);

        //            if (String.IsNullOrWhiteSpace(_MyDeclarationPM.DeclarationNumber))
        //            {
        //                _MyDeclarationPM.DeclarationNumber = response.Declaration.ID.Value;
        //            }
        //            else
        //            {
        //                var customDeclarationNumber = response.Declaration.ID.Value;
        //                if (customDeclarationNumber != _MyDeclarationPM.DeclarationNumber)
        //                {
        //                     _MyDeclarationPM.DeclarationNumber = customDeclarationNumber;  
        //                }
        //            }

        //            //Update Declaration 
        //            _MyDeclarationPM.VersionId = response.Declaration.DMExtensions.VersionID.Value;
        //            _MyDeclarationPM.DeclarationStatusTypeCode = response.Status.NameCode.Value;
        //            _MyDeclarationPM.LoadingFactor = response.Declaration.DMExtensions.ExpenseLoadingFactor.Value;
        //             _MyDeclarationPM.DealValue = Math.Round(response.Declaration.DMExtensions.CustomsValueComponent.TotalDealValueAmountNIS.Value, 2);
        //             _MyDeclarationPM.CIFValue = Math.Round(response.Declaration.DMExtensions.CustomsValueComponent.CifValueNIS.Value, 2);
        //             _MyDeclarationPM.TotalTax = Math.Round(response.Declaration.DMExtensions.CustomsValueComponent.TaxAssessedAmount.Value, 2);
        //            _MyDeclarationPM.DealValueWithFactor = Math.Round(response.Declaration.DMExtensions.CustomsValueComponent.TotalMADDealValueAmountNIS.Value, 2);
        //            _MyDeclarationPM.TaxationDateTime = Convert.ToDateTime(response.Declaration.DMExtensions.TaxationDateTime);
        //             decimal DealValueWithoutFactor = 0;

        //            if (response.Declaration.GoodsShipment != null)
        //            {
        //                foreach (var goodsShipment in response.Declaration.GoodsShipment)
        //                {
        //                    if (goodsShipment.GovernmentAgencyGoodsItem != null)
        //                    {
        //                        foreach (var governmentAgencyGoodsItem in goodsShipment.GovernmentAgencyGoodsItem)
        //                        {
        //                            if (governmentAgencyGoodsItem.DMExtensions != null)
        //                            {
        //                                if (governmentAgencyGoodsItem.DMExtensions.GoodsItemAmount != null)
        //                                {
        //                                    foreach (var goodsItemAmount in governmentAgencyGoodsItem.DMExtensions.GoodsItemAmount)
        //                                    {
        //                                        if (goodsItemAmount.AmountType.Value == "15" && goodsItemAmount.CustomsValueAmount.currencyIDSpecified && goodsItemAmount.CustomsValueAmount.currencyID.ToString() == "ILS")
        //                                        {
        //                                            DealValueWithoutFactor += goodsItemAmount.CustomsValueAmount.Value;
        //                                            break;
        //                                        }
        //                                    }
        //                                }
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //            if (DealValueWithoutFactor != 0) _MyDeclarationPM.DealValueWithoutFactor = Math.Round(DealValueWithoutFactor, 2);

        //            //if (requestParams.InterfaceTypeCode == "2750")
        //            //{
        //            //    if (!string.IsNullOrWhiteSpace(RequestSheetContext.Current.GetContextOrDefault().SignByX509SubjectName))
        //            //    {
        //            //        _MyDeclarationPM.IsSignedVersion = true;
        //            //        _MyDeclarationPM.SignedByUserId = requestParams.LoggingUserId;
        //            //        _MyDeclarationPM.SignerPersonalId = SignCertificateClass.GetPersonID(RequestSheetContext.Current.GetContextOrDefault().SignByX509SubjectName);
        //            //    }
        //            //    else
        //            //    {
        //            //        _MyDeclarationPM.IsSignedVersion = false;
        //            //        _MyDeclarationPM.SignedByUserId = null;
        //            //        _MyDeclarationPM.SignerPersonalId = null;
        //            //    }
        //            //}

        //            //Analyze the Errors section in the response XML 
        //            var swErrosXml1 = Stopwatch.StartNew();
        //            this._MyDeclarationPM.ErrosXml = mydDclarationErrorPointerService.AnalyzeErrorPionter(response.Error, _MyDeclarationPM, WCOTypeEnum.WCO, !_IsSubmitDeclarationResponse);
        //             var error = mydDclarationErrorPointerService._declarationErrorPointer;
        //            LogMessagingUtil.Instance.AppendLine("ErrosXml:Took:" + swErrosXml1.ElapsedMilliseconds);


        //            var swTax = Stopwatch.StartNew();
        //            //Update Declaration Taxes
        //            _MyDeclarationPM.DeclarationTaxes = GetDeclarationTaxesPM(response);

        //            bool tester = false;
        //            if (tester)
        //            {
        //                int count = 0;
        //                foreach (var si in _MyDeclarationPM.SupplierInvoices)
        //                {
        //                    foreach (var sii in si.SupplierInvoiceItems)
        //                    {
        //                        foreach (var siic in sii.SupplierInvioceItemCertificats)
        //                        {
        //                            count++;
        //                        }
        //                    }
        //                }
        //            }

        //            //Update Declaration Item Taxes
        //            LogMessagingUtil.Instance.LogActionTime(() =>
        //            {
        //                _MyDeclarationPM.SupplierInvoices = GetSupplierInvoicesPM(response);
        //            }, "GetSupplierInvoicesPM");
        //            LogMessagingUtil.Instance.AppendLine("GetDeclarationTaxes+Item Tax:Took:" + swTax.ElapsedMilliseconds);

        //            if (tester)
        //            {
        //                int count = 0;
        //                foreach (var si in _MyDeclarationPM.SupplierInvoices)
        //                {
        //                    foreach (var sii in si.SupplierInvoiceItems)
        //                    {
        //                        foreach (var siic in sii.SupplierInvioceItemCertificats)
        //                        {
        //                            count++;
        //                        }
        //                    }
        //                }
        //            }

        //            //Build constraints
        //            if (this._IsSubmitDeclarationResponse != true)
        //            {
        //                DeleteDeclarationConstraints(myDeclarationConstraintUpdateService);
        //            }
        //            _MyDeclarationPM.DeclarationConstraints = BuildDeclarationConstraints(response.Error);

        //            //<--- Yuval Chalup 06.08.2015 TASK-15422
        //            _MyDeclarationPM.PaymentOrderNumber = null;
        //            _MyDeclarationPM.PaymentStatusCode = null;
        //            //string paymentOrderNumber = null; //Yuval Chalup 10.08.2015 TASK-15472
        //            //string paymentStatusCode = null; //Yuval Chalup 10.08.2015 TASK-15472
        //            //if (customResponse.DeclarationPaymentDetails != null)
        //            //{
        //            //    if (customResponse.DeclarationPaymentDetails.PaymentOrderNumber != null)
        //            //    {
        //            //        _MyDeclarationPM.PaymentOrderNumber = customResponse.DeclarationPaymentDetails.PaymentOrderNumber.ToString();
        //            //        paymentOrderNumber = customResponse.DeclarationPaymentDetails.PaymentOrderNumber.ToString(); //Yuval Chalup 10.08.2015 TASK-15472
        //            //    }
        //            //    if (customResponse.DeclarationPaymentDetails.PaymentOrderStatus != null)
        //            //    {
        //            //        _MyDeclarationPM.PaymentStatusCode = customResponse.DeclarationPaymentDetails.PaymentOrderStatus.ToString();
        //            //        paymentStatusCode = customResponse.DeclarationPaymentDetails.PaymentOrderStatus.ToString(); //Yuval Chalup 10.08.2015 TASK-15472

        //            //    }
        //            //}

        //            // if (!(!string.IsNullOrWhiteSpace(paymentOrderNumber) && paymentStatusCode != "5") && !(_IsSubmitDeclarationResponse == true && string.IsNullOrWhiteSpace(paymentOrderNumber) && _MyDeclarationPM.DeclarationStatusTypeCode == "5" && _MyDeclarationPM.TotalTax <= 5))
        //            //{
        //            //    _MyDeclarationPM.CurrentContextTag = Logitude.Customs.BL.EntityUpdateServices.DeclarationUpdateService.UpdateUnifreightBillingConst;
        //            //    if (_MyDeclarationPM.IsCourierDeclaration && declarationPaymentsPM != null && declarationPaymentsPM.PaymentDate.HasValue && _IsSubmitDeclarationResponse == true && _MyDeclarationPM.DeclarationStatusTypeCode == "5")
        //            //    {
        //            //        _MyDeclarationPM.PaymentDate = declarationPaymentsPM.PaymentDate;
        //            //    }

        //            //}
        //            //else
        //            //{
        //            //    //Payment date update
        //            //    if (declarationPaymentsPM != null && declarationPaymentsPM.PaymentDate.HasValue)
        //            //    {
        //            //        _MyDeclarationPM.PaymentDate = declarationPaymentsPM.PaymentDate;
        //            //    }
        //            //    //if (!string.IsNullOrWhiteSpace(customResponse.Response.Status.EffectiveDateTime))
        //            //    //{
        //            //    //    _MyDeclarationPM.PaymentDate = DateTime.Parse(customResponse.Response.Status.EffectiveDateTime);
        //            //    //}
        //            //    if (_MyDeclarationPM.CurrentContextTag != Logitude.Customs.BL.EntityUpdateServices.DeclarationUpdateService.UpdateUnifreightBillingConst)
        //            //    {
        //            //        _MyDeclarationPM.CurrentContextTag = Logitude.Customs.BL.EntityUpdateServices.DeclarationUpdateService.CreateUnifreightPaymentConst;
        //            //    }
        //            //}
        //                             if (response.Status.NameCode.Value == "14")
        //                            {
        //                                _MyDeclarationPM.PaymentDate = null;
        //                                LogMessagingUtil.Instance.AppendLine("Change Declaration Version From " + oldVersionId + "To " + _MyDeclarationPM.VersionId);
        //                                _MyDeclarationPM.CurrentContextTag = Logitude.Customs.BL.EntityUpdateServices.DeclarationUpdateService.UpdateUnifreightBillingConst;
        //                            }



        //            if (_TotalBtlCoverageNISSum > 0)
        //            {
        //                mydDclarationErrorPointerService = new DeclarationErrorPointerService();
        //                this._MyDeclarationPM.ErrosXml = mydDclarationErrorPointerService.AddErrorPionter(error, "", "", "", "", "", "", "", "A", @"לתיק זה קיימת הלוואת ביטוח לאומי ע""ס " + _TotalBtlCoverageNISSum.ToString() + @" ש""ח", "", "", "", "");
        //            }

        //            //if (response.Declaration.col != null)
        //            //{
        //            //    if (customResponse.CollateralRequestDetails.Count() > 0)
        //            //    {
        //            //        foreach (UnifreightIIG.Common.ImportDeclarationServiceReference.CollateralRequestDetails collateralRequestItem in customResponse.CollateralRequestDetails)
        //            //        {
        //            //            this._MyDeclarationPM.ErrosXml = mydDclarationErrorPointerService.AddErrorPionter(_MyDeclarationError, "", "", "", "", "", "", "", "A", "המשוב להצהרה כולל דרישה לבטוחה " + " - מספר בטוחה " + collateralRequestItem.collateralRequestNumber, "", "", "", "");
        //            //         }
        //            //    }
        //            //}

        //            UpdateDepositionStatusCode(error);

        //            //if (!String.IsNullOrWhiteSpace("itzik and yaron move to herer from DeclarationWebService.asmx"))
        //            //{
        //            //    if (requestParams.GetType() != typeof(DeclarationRestoreRequestParams))//Task 44715 (add condition to itzik and yaron...
        //            //    {
        //            //        _MyDeclarationPM.MarkAsChanged = false;
        //            //        _MyDeclarationPM.IsChanged = false;
        //            //    }
        //            //}
        //        //    _MyDeclarationPM.CustomsRequestsSheetId = requestParams.CustomsRequestsSheetId;
        //            _MyDeclarationPM.ChangeSetOp = ChangeSetOperation.Update;
        //            myDeclarationUpdateService.IsFromCustomsFeedback = true;
        //            myDeclarationUpdateService.Update(_MyDeclarationPM, true);

        //            if (_MyDeclarationPM.IsCourierDeclaration)
        //            {
        //                DeclarationCourierStatusQueryService declarationCourierStatusQueryService = new DeclarationCourierStatusQueryService(context);
        //                DeclarationCourierStatusPM _MyDeclarationCourierStatusPM = new DeclarationCourierStatusPM();
        //                _MyDeclarationCourierStatusPM = declarationCourierStatusQueryService.GetSingle(_MyDeclarationPM.Id, true, false);
        //                if (_MyDeclarationCourierStatusPM != null)
        //                {
        //                    DeclarationPendingPM declarationPendingPM_900 = null;
        //                    DeclarationPendingPM declarationPendingPM_901 = null;
        //                    if (_MyDeclarationCourierStatusPM.DeclarationPendings != null && _MyDeclarationCourierStatusPM.DeclarationPendings.Count() > 0)
        //                    {
        //                        declarationPendingPM_900 = _MyDeclarationCourierStatusPM.DeclarationPendings.Where(r => r.DeclarationID == _MyDeclarationCourierStatusPM.DeclarationId && r.CourierPendingReasonCode == "900").FirstOrDefault();
        //                        declarationPendingPM_901 = _MyDeclarationCourierStatusPM.DeclarationPendings.Where(r => r.DeclarationID == _MyDeclarationCourierStatusPM.DeclarationId && r.CourierPendingReasonCode == "901").FirstOrDefault();
        //                    }
        //                    // Pending 901
        //                    Boolean isSetPendingTo901 = false;
        //                    if (response.Error != null)
        //                    {
        //                        foreach (var errorItem in response.Error)
        //                        {
        //                            if (errorItem.ValidationCode != null && errorItem.ValidationCode.Value == "2382")
        //                            {
        //                                CourierPendingReasonQueryService myCourierPendingReasonQueryService = new CourierPendingReasonQueryService(context);
        //                                CourierPendingReasonPM courierPendingReasonPM = myCourierPendingReasonQueryService.GetSingle("901", false, false);
        //                                if (courierPendingReasonPM == null)
        //                                {
        //                                    LogMessagingUtil.Instance.AppendLine("לא קיים קוד Pending - הצהרה פלסטינאית = 901 בטבלת סיבות Pending");
        //                                    break;
        //                                }
        //                                LogMessagingUtil.Instance.AppendLine("Pending - הצהרה פלסטינאית = 901");
        //                                isSetPendingTo901 = true;
        //                                if (declarationPendingPM_901 == null)
        //                                {
        //                                    declarationPendingPM_901 = new DeclarationPendingPM();
        //                                    declarationPendingPM_901.CourierPendingReasonCode = "901";
        //                                    declarationPendingPM_901.Status = "A";
        //                                    declarationPendingPM_901.ChangeSetOp = ChangeSetOperation.Insert;
        //                                    _MyDeclarationCourierStatusPM.DeclarationPendings.Add(declarationPendingPM_901);
        //                                }
        //                                else if (declarationPendingPM_901.Status != "A")
        //                                {
        //                                    declarationPendingPM_901.ChangeSetOp = ChangeSetOperation.Update;
        //                                    declarationPendingPM_901.Status = "A";
        //                                }
        //                                if (declarationPendingPM_901.ChangeSetOp != ChangeSetOperation.None)
        //                                {
        //                                    LogMessagingUtil.Instance.AppendLine("Set Courier Pending Reason Code To 901");
        //                                    if (_MyDeclarationCourierStatusPM.ChangeSetOp != ChangeSetOperation.Update) _MyDeclarationCourierStatusPM.ChangeSetOp = ChangeSetOperation.Update;
        //                                }
        //                            }
        //                        }
        //                    }
        //                    if (!isSetPendingTo901)
        //                    {
        //                        if (declarationPendingPM_901 != null)
        //                        {
        //                            declarationPendingPM_901.ChangeSetOp = ChangeSetOperation.Update;
        //                            declarationPendingPM_901.Status = "S";
        //                            if (_MyDeclarationCourierStatusPM.ChangeSetOp != ChangeSetOperation.Update) _MyDeclarationCourierStatusPM.ChangeSetOp = ChangeSetOperation.Update;
        //                            LogMessagingUtil.Instance.AppendLine("Courier Pending Reason Code 901 Set as Solved");
        //                        }
        //                    }
        //                    // Pending 900
        //                    CourierMasterQueryService courierMasterService = new CourierMasterQueryService(_MyDeclarationPM.Tenant);
        //                    CourierMasterPM courierMaster = courierMasterService.GetSingle(_MyDeclarationPM.CourierMasterId, false, false);
        //                    if (courierMaster != null)
        //                    {
        //                        var myGDFDATAQueryService = new GDFDATAQueryService(AmitalContext.GetContext(_MyDeclarationPM.Tenant));
        //                        var def = myGDFDATAQueryService.GetSingle("ISRAEL", "CGO_ACT_COLLECT", "NON", courierMaster.IntegratorNumber, false, true);
        //                        bool isCollectActive = def.DEFDATA == "Y";
        //                        if (declarationPendingPM_900 == null)
        //                        {
        //                            CourierPendingReasonQueryService myCourierPendingReasonQueryService = new CourierPendingReasonQueryService(context);
        //                            CourierPendingReasonPM courierPendingReasonPM = myCourierPendingReasonQueryService.GetSingle("900", false, false);
        //                            if (courierPendingReasonPM == null)
        //                            {
        //                                LogMessagingUtil.Instance.AppendLine("לא קיים קוד תהליך גביה- במידה ומופעל בדיקה האם להגדיר גבייה = 900 בטבלת סיבות Pending");
        //                                isCollectActive = false;
        //                            }
        //                        }
        //                        if (isCollectActive)
        //                        {

        //                            LogMessagingUtil.Instance.AppendLine("תהליך גביה- במידה ומופעל בדיקה האם להגדיר גבייה = 900");
        //                            if (_MyDeclarationPM.SupplierInvoices != null && _MyDeclarationPM.SupplierInvoices.FirstOrDefault().IncotermCode != "DDP" && _MyDeclarationPM.TotalTax > 0)
        //                            {
        //                                if (declarationPendingPM_900 == null)
        //                                {
        //                                    declarationPendingPM_900 = new DeclarationPendingPM();
        //                                    declarationPendingPM_900.CourierPendingReasonCode = "900";
        //                                    declarationPendingPM_900.Status = "A";
        //                                    declarationPendingPM_900.ChangeSetOp = ChangeSetOperation.Insert;
        //                                    _MyDeclarationCourierStatusPM.DeclarationPendings.Add(declarationPendingPM_900);
        //                                }
        //                                else if (declarationPendingPM_900.Status != "A")
        //                                {
        //                                    declarationPendingPM_900.ChangeSetOp = ChangeSetOperation.Update;
        //                                    declarationPendingPM_900.Status = "A";
        //                                }
        //                                if (declarationPendingPM_900.ChangeSetOp != ChangeSetOperation.None)
        //                                {
        //                                    LogMessagingUtil.Instance.AppendLine("Set Courier Pending Reason Code 900");
        //                                    if (_MyDeclarationCourierStatusPM.ChangeSetOp != ChangeSetOperation.Update) _MyDeclarationCourierStatusPM.ChangeSetOp = ChangeSetOperation.Update;
        //                                }
        //                            }
        //                            else if (declarationPendingPM_900 != null)
        //                            {
        //                                declarationPendingPM_900.ChangeSetOp = ChangeSetOperation.Update;
        //                                declarationPendingPM_900.Status = "S";
        //                                if (_MyDeclarationCourierStatusPM.ChangeSetOp != ChangeSetOperation.Update) _MyDeclarationCourierStatusPM.ChangeSetOp = ChangeSetOperation.Update;
        //                                LogMessagingUtil.Instance.AppendLine("Courier Pending Reason Code 900 Set as Solved");
        //                            }
        //                        }
        //                    }

        //                }
        //                if (_MyDeclarationCourierStatusPM.ChangeSetOp == ChangeSetOperation.Update)
        //                {
        //                    DeclarationCourierStatusUpdateService declarationCourierStatusUpdateService = new DeclarationCourierStatusUpdateService(context, new Dictionary<string, IContext>(), _MyDeclarationPM.Tenant);
        //                    declarationCourierStatusUpdateService.Update(_MyDeclarationCourierStatusPM, true);
        //                }
        //             }

        //            // if (_MyDeclarationPM.IsConnectedToUnifreight)
        //            //{
        //            //    //<--- Yuval Chalup 09.11.2015 TASK-16498 - Update PaymentOrderNumber in Payment
        //            //    if (customResponse.DeclarationPaymentDetails != null)
        //            //    {
        //            //        if (customResponse.DeclarationPaymentDetails.PaymentOrderNumber != null || (_IsSubmitDeclarationResponse == true && string.IsNullOrWhiteSpace(paymentOrderNumber) && _MyDeclarationPM.DeclarationStatusTypeCode == "5" && _MyDeclarationPM.TotalTax <= 5))
        //            //        {
        //            //            var unifreightDeclarationPaymentUpdateService = new UnifreightDeclarationPaymentUpdateService(declarationPaymentsPM, _MyDeclarationPM);
        //            //            unifreightDeclarationPaymentUpdateService.Update();
        //            //        }
        //            //    }
        //            //}

        //            //if (customResponse.CollateralRequestDetails != null) // Create Collateral
        //            //{
        //            //    LogMessagingUtil.Instance.AppendLine("CollateralRequestDetails: Create Collateral");
        //            //    //var headerXml = XmlGenericUtil<UnifreightIIG.Common.ImportDeclarationServiceReference.RequestContentHeader>
        //            //    //    .SerializeObject(customResponse.);
        //            //    //var header = XmlGenericUtil<UnifreightIIG.Common.MessageLib.Collateral.RequestContentHeader>.DeSerializeObject(headerXml);
        //            //    var requestXml = XmlGenericUtil<UnifreightIIG.Common.ImportDeclarationServiceReference.CollateralRequestDetails[]>
        //            //        .SerializeObject(customResponse.CollateralRequestDetails);
        //            //    var collateralArry = XmlGenericUtil<UnifreightIIG.Common.MessageLib.Collateral.CollateralRequestDetails[]>.DeSerializeObject(requestXml);

        //            //    COLT_NG_8211_MSG10040_CollateralRequestMsg myCOLT_NG_8211_MSG10040_CollateralRequestMsg = new COLT_NG_8211_MSG10040_CollateralRequestMsg();
        //            //    myCOLT_NG_8211_MSG10040_CollateralRequestMsg.ResponseContentHeader = null;
        //            //    myCOLT_NG_8211_MSG10040_CollateralRequestMsg.CollateralRequestDetails = collateralArry;
        //            //    var xml = XmlGenericUtil<UnifreightIIG.Common.MessageLib.Collateral.COLT_NG_8211_MSG10040_CollateralRequestMsg>
        //            //        .SerializeObject(myCOLT_NG_8211_MSG10040_CollateralRequestMsg);

        //            //    var ser = XmlGenericUtil<UnifreightIIG.Common.MessageLib.Collateral.COLT_NG_8211_MSG10040_CollateralRequestMsg>.DeSerializeObject(xml);
        //            //    var DF_MSG10040_CollateralRequestMsgResponseService = new DF_8211_CollateralRequestMsgResponseService();
        //            //    DF_MSG10040_CollateralRequestMsgResponseService.Update(ser, requestParams);
        //            //}

        //            // string declarationStatusTypeName = _MyDeclarationPM.DeclarationStatusTypeCode;
        //            //if (!string.IsNullOrWhiteSpace(_MyDeclarationPM.DeclarationStatusTypeCode))
        //            //{
        //            //    DeclarationStatusTypeQueryService declarationStatusTypeQueryService = new DeclarationStatusTypeQueryService(_MyDeclarationPM.Tenant);
        //            //    DeclarationStatusTypePM declarationStatusType = declarationStatusTypeQueryService.GetSingle(_MyDeclarationPM.DeclarationStatusTypeCode, false, true);
        //            //    if (declarationStatusType != null)
        //            //    {
        //            //        declarationStatusTypeName = declarationStatusType.LocalName;
        //            //    }
        //            // }




        //        }


        //        private List<SupplierInvoicePM> GetSupplierInvoicesPM(Response response)

        //        {
        //            var supplierInvoicesPMList = new List<SupplierInvoicePM>();

        //            foreach (var goodsShipment in response.Declaration.GoodsShipment)
        //            {
        //                //var supplierInvoiceItemsTaxPM = new SupplierInvoiceItemsTaxPM();
        //                //var supplierInvoicePM = this._MyDeclarationPM.SupplierInvoices.FirstOrDefault(si => si.InvoiceNumber == goodsShipment.Invoice.ID.Value);
        //                var supplierInvoicePM = this._MyDeclarationPM.SupplierInvoices.FirstOrDefault(si => si.SequenceNumeric == goodsShipment.SequenceNumeric);

        //                if (supplierInvoicePM == null)
        //                {
        //                    throw new System.Exception(
        //                        "unable to find the supplierInvoicePM from goodsShipment.Invoice.ID.Value " + goodsShipment.Invoice.ID.Value);
        //                }
        //                var InvoiceCounterKey = supplierInvoicePM.InvoiceCounterKey;

        //                //Update supplier Valuation - Additional costs details from custom
        //                supplierInvoicePM.SupplierInvoiceModifications = GetSupplierInvoiceModifications(goodsShipment, ref supplierInvoicePM);
        //                //Update supplier items
        //                supplierInvoicePM.SupplierInvoiceItems = GetSupplierInvoiceItems(goodsShipment, ref supplierInvoicePM);

        //                if (goodsShipment.Invoice != null && goodsShipment.Invoice.DMExtensions != null && goodsShipment.Invoice.DMExtensions.RateNumeric != null)
        //                {
        //                    supplierInvoicePM.ExchangeRate = goodsShipment.Invoice.DMExtensions.RateNumeric.Value;
        //                }

        //                supplierInvoicePM.ChangeSetOp = ChangeSetOperation.Update;
        //                supplierInvoicesPMList.Add(supplierInvoicePM);
        //            }

        //            return supplierInvoicesPMList;
        //        }


        //        private List<SupplierInvoiceItemPM> GetSupplierInvoiceItems(
        //    DeclarationGoodsShipment goodsShipment,
        //    ref SupplierInvoicePM supplierInvoicePM)
        //        {
        //            var supplierInvoiceItemsPMList = new List<SupplierInvoiceItemPM>();
        //            if (supplierInvoicePM.IsAccumalated == true && supplierInvoicePM.SupplierInvoiceItems != null && supplierInvoicePM.SupplierInvoiceItems.Count > 0)
        //            {
        //                supplierInvoicePM.SupplierInvoiceItems.RemoveAll(rec => rec.IsParent != true);
        //            }
        //            foreach (var governmentAgencyGoodsItem in goodsShipment.GovernmentAgencyGoodsItem)
        //            {
        //                var supplierInvoiceItemPM = supplierInvoicePM.SupplierInvoiceItems.FirstOrDefault(si => si.SequenceNumeric == governmentAgencyGoodsItem.SequenceNumeric);
        //                //<--- Added by Yuval Chalup 26.05.2015 TASK-13473
        //                if (supplierInvoiceItemPM == null)
        //                {
        //                    if (governmentAgencyGoodsItem.Commodity != null)
        //                    {
        //                        if (governmentAgencyGoodsItem.Commodity.Classification != null)
        //                        {
        //                            if (governmentAgencyGoodsItem.Commodity.Classification.Count() > 0)
        //                            {
        //                                if (governmentAgencyGoodsItem.Commodity.Classification[0] != null)
        //                                {
        //                                    throw new System.Exception(
        //                                       "unable to find the supplierInvoiceItemPM from governmentAgencyGoodsItem.Commodity.Classification " + governmentAgencyGoodsItem.Commodity.Classification[0].ID.Value);
        //                                }
        //                            }
        //                        }
        //                    }
        //                    throw new System.Exception(
        //                       "unable to find the supplierInvoiceItemPM from governmentAgencyGoodsItem.SequenceNumeric " + governmentAgencyGoodsItem.SequenceNumeric);
        //                }
        //                //Added by Yuval Chalup 26.05.2015 TASK-13473 --->

        //                var supplierInvoiceItemsTaxPMList = new List<SupplierInvoiceItemsTaxPM>();
        //                //Update supplier item Valuation Adjustment - Commodity price adjustments
        //                supplierInvoiceItemPM.SupplierInvoiceItemsMods = GetSupplierInvoiceItemsModifications(governmentAgencyGoodsItem.ValuationAdjustment, supplierInvoiceItemPM);
        //                // moran 24.11.15 - Task 17424 -->
        //                supplierInvoiceItemPM.SupplierInvoiceItemModVehicles = GetSupplierInvoiceItemsModVehicles(governmentAgencyGoodsItem.DMExtensions.VehicleValuationAdjustment, supplierInvoiceItemPM);
        //                // moran 24.11.15 - Task 17424 <--

        //                supplierInvoiceItemPM.SupplierInvioceItemCertificats = GetSupplierInvioceItemCertificats(supplierInvoicePM, supplierInvoiceItemPM);

        //                if (governmentAgencyGoodsItem.Commodity == null)
        //                {
        //                    continue;
        //                }
        //                //TODO:DDDD

        //                if (governmentAgencyGoodsItem.Commodity.DutyTaxFee != null)
        //                {
        //                    foreach (var dutyTaxFee in governmentAgencyGoodsItem.Commodity.DutyTaxFee)
        //                    {
        //                        var supplierInvoiceItemsTaxPM = new SupplierInvoiceItemsTaxPM();
        //                        supplierInvoiceItemsTaxPM.ChangeSetOp = ChangeSetOperation.Insert;
        //                        //supplierInvoiceItemsTaxPM.DeclarationId = this._MyDeclarationPM.Id; //Removed by Yuval Chalup 26.05.2015 TASK-13473 (Move to SupplierInvoiceItemsTaxUpdateService.OnUpdating)
        //                        supplierInvoiceItemsTaxPM.Tenant = this._MyDeclarationPM.Tenant;
        //                        //supplierInvoiceItemsTaxPM.InvoiceCounterKey = supplierInvoicePM.InvoiceCounterKey; //Removed by Yuval Chalup 26.05.2015 TASK-13473 (Move to SupplierInvoiceItemsTaxUpdateService.OnUpdating)
        //                        //supplierInvoiceItemsTaxPM.LineNumber = supplierInvoiceItemPM.LineNumber; //Removed by Yuval Chalup 26.05.2015 TASK-13473 (Move to SupplierInvoiceItemsTaxUpdateService.OnUpdating)
        //                        supplierInvoiceItemsTaxPM.TaxTypeCode = dutyTaxFee.TypeCode.Value;
        //                        if (dutyTaxFee.DutyRegimeCode != null)
        //                        {
        //                            supplierInvoiceItemsTaxPM.TradeAgreementTypeCode = dutyTaxFee.DutyRegimeCode.Value;
        //                        }
        //                        supplierInvoiceItemsTaxPM.TaxRate = dutyTaxFee.TaxRate;
        //                        supplierInvoiceItemsTaxPM.TaxBaseAmount = dutyTaxFee.AdValoremTaxBaseAmount.Value;
        //                        supplierInvoiceItemsTaxPM.TaxAmount = dutyTaxFee.DMExtensions.CalculatedTax.Amount.Value;
        //                        supplierInvoiceItemsTaxPM.DeferedTaxAmount = dutyTaxFee.DMExtensions.CalculatedTax.DeferedTaxAmount.Value;
        //                        if (dutyTaxFee.DMExtensions.CalculatedTax.DefinedPerUnitMethod != null)
        //                        {
        //                            supplierInvoiceItemsTaxPM.DefinedPerUnitMeasure = dutyTaxFee.DMExtensions.CalculatedTax.DefinedPerUnitMethod.Value;
        //                        }
        //                        supplierInvoiceItemsTaxPM.AlternateRate = dutyTaxFee.DMExtensions.CalculatedTax.AlternateRate.Value;
        //                        if (dutyTaxFee.DMExtensions.CalculatedTax.AlternateDefinedPerUnitMeasure != null)
        //                        {
        //                            supplierInvoiceItemsTaxPM.AlternateDefinedPerUnitMeasure = dutyTaxFee.DMExtensions.CalculatedTax.AlternateDefinedPerUnitMeasure.Value;
        //                        }
        //                        if (dutyTaxFee.DMExtensions.CalculatedTax.DefinedPerUnitQuantity != null)
        //                        {
        //                            supplierInvoiceItemsTaxPM.DefinedPerUnitQuantity = dutyTaxFee.DMExtensions.CalculatedTax.DefinedPerUnitQuantity.Value;
        //                        }
        //                        if (dutyTaxFee.DMExtensions.CalculatedTax.AlternateDefinedPerUnitQuantity != null)
        //                        {
        //                            supplierInvoiceItemsTaxPM.AlternateDefinedPerUnitQuant = dutyTaxFee.DMExtensions.CalculatedTax.AlternateDefinedPerUnitQuantity.Value;
        //                        }
        //                        if (dutyTaxFee.DMExtensions.CalculatedTax.MeasurementUnitCode != null)
        //                        {
        //                            supplierInvoiceItemsTaxPM.MeasurementUnitCode = dutyTaxFee.DMExtensions.CalculatedTax.MeasurementUnitCode.Value;
        //                        }
        //                        if (dutyTaxFee.DMExtensions.CalculatedTax.AlternateMeasurementUnit != null)
        //                        {
        //                            supplierInvoiceItemsTaxPM.AlternateMeasurementUnitCode = dutyTaxFee.DMExtensions.CalculatedTax.AlternateMeasurementUnit.Value;
        //                        }
        //                        if (dutyTaxFee.DMExtensions.CalculatedTax.TradeLevyNumber != null)
        //                        {
        //                            supplierInvoiceItemsTaxPM.TradeLevyNumber = dutyTaxFee.DMExtensions.CalculatedTax.TradeLevyNumber.Value;
        //                        }
        //                        if (dutyTaxFee.DMExtensions.CalculatedTax.TotalBtlCoverageNIS != null)
        //                        {
        //                            supplierInvoiceItemsTaxPM.TotalBtlCoverageNIS = dutyTaxFee.DMExtensions.CalculatedTax.TotalBtlCoverageNIS.Value;
        //                            _TotalBtlCoverageNISSum = _TotalBtlCoverageNISSum + dutyTaxFee.DMExtensions.CalculatedTax.TotalBtlCoverageNIS.Value;
        //                        }
        //                        // moran 21.11.13 - Bug 2083 - change handle -->
        //                        //AddSupplierInvoiceItemsTaxesModificationPM(supplierInvoiceItemsTaxPM, governmentAgencyGoodsItem.Commodity);
        //                        //supplierInvoiceItemsTaxPM.SupplierInvoiceItemsTaxesModifications = GetSupplierInvoiceItemsTaxesModifications(governmentAgencyGoodsItem.Commodity.DMExtensions.ValuationDeductionAdjustment, supplierInvoiceItemsTaxPM);
        //                        // moran 21.11.13 - Bug 2083 - change handle <--
        //                        supplierInvoiceItemsTaxPMList.Add(supplierInvoiceItemsTaxPM);
        //                    }
        //                }

        //                supplierInvoiceItemPM.SupplierInvoiceItemTaxes = supplierInvoiceItemsTaxPMList;

        //                totGeneralTaxCalc = 0;
        //                totPurchaseCalc = 0;
        //                totVatCalc = 0;

        //                generalTax = 0;
        //                purchase = 0;
        //                vat = 0;

        //                foreach (var tax in supplierInvoiceItemPM.SupplierInvoiceItemTaxes)
        //                {
        //                    if (tax.TaxTypeCode == "1")
        //                    {
        //                        generalTax += tax.TaxAmount;
        //                    }
        //                    if (tax.TaxTypeCode == "16")
        //                    {
        //                        purchase += tax.TaxAmount;
        //                    }
        //                    if (tax.TaxTypeCode == "15")
        //                    {
        //                        vat += tax.TaxAmount;
        //                    }
        //                }

        //                // moran 6.10.15 - Task 17209 --> 
        //                supplierInvoiceItemPM.SupplierInvoiceItemVehicles = GetSupplierInvoiceItemsVehicles(governmentAgencyGoodsItem.DMExtensions.Vehicle, supplierInvoiceItemPM);
        //                // moran 6.10.15 - Task 17209 <--
        //                if (supplierInvoiceItemPM.SupplierInvoiceItemVehicles != null && supplierInvoiceItemPM.SupplierInvoiceItemVehicles.Count() > 0 && supplierInvoiceItemPM.SupplierInvoiceItemVehicles.LastOrDefault().SupplierInvoiceItemVehicleAdds != null && supplierInvoiceItemPM.SupplierInvoiceItemVehicles.LastOrDefault().SupplierInvoiceItemVehicleAdds.Count() > 0)
        //                {
        //                    decimal? diff = 0;
        //                    if (generalTax != totGeneralTaxCalc)
        //                    {
        //                        diff = generalTax - totGeneralTaxCalc;
        //                        supplierInvoiceItemPM.SupplierInvoiceItemVehicles.LastOrDefault().SupplierInvoiceItemVehicleAdds.LastOrDefault().ChassisTax += diff;
        //                    }
        //                    if (purchase != totPurchaseCalc)
        //                    {
        //                        diff = purchase - totPurchaseCalc;
        //                        supplierInvoiceItemPM.SupplierInvoiceItemVehicles.LastOrDefault().SupplierInvoiceItemVehicleAdds.LastOrDefault().ChassisPurchaseTax += diff;
        //                    }
        //                    if (vat != totVatCalc)
        //                    {
        //                        diff = vat - totVatCalc;
        //                        supplierInvoiceItemPM.SupplierInvoiceItemVehicles.LastOrDefault().SupplierInvoiceItemVehicleAdds.LastOrDefault().ChassisVat += diff;
        //                    }
        //                }
        //                supplierInvoiceItemPM.ChangeSetOp = ChangeSetOperation.Update;
        //                supplierInvoiceItemsPMList.Add(supplierInvoiceItemPM);
        //                var NewSupplierInvioceItemCertificats = supplierInvoiceItemPM.SupplierInvioceItemCertificats.Where(r => r.ChangeSetOp == ChangeSetOperation.Insert).ToList();
        //                if (supplierInvoicePM.IsAccumalated == true && supplierInvoiceItemPM.IsParent == true && NewSupplierInvioceItemCertificats != null && NewSupplierInvioceItemCertificats.Count > 0)
        //                {
        //                    AddNewCertificatesToChildItems(supplierInvoiceItemsPMList, supplierInvoiceItemPM, NewSupplierInvioceItemCertificats);
        //                }
        //            }

        //            return supplierInvoiceItemsPMList;
        //        }


        //        private List<SupplierInvoiceModificationPM> GetSupplierInvoiceModifications(DeclarationGoodsShipment goodsShipment, ref SupplierInvoicePM supplierInvoicePM)
        //        {
        //            // moran 26.5.15 - 13564 -->
        //            //var supplierInvoiceModificationPMList = new List<SupplierInvoiceModificationPM>();
        //            var supplierInvoiceModificationPMList = new List<SupplierInvoiceModificationPM>(supplierInvoicePM.SupplierInvoiceModifications);
        //            // moran 26.5.15 - 13564 <--
        //            if (goodsShipment.CustomsValuation == null)
        //            {
        //                return null;
        //            }

        //            //Check if there is a DECLARED Fee (I01) in message
        //            DeclarationGoodsShipmentCustomsValuation declarationGoodsShipmentCustomsValuation_I01 = goodsShipment.CustomsValuation.FirstOrDefault(rec => rec.ChargesTypeCode.Value == "I01");

        //            foreach (var valuationItem in goodsShipment.CustomsValuation)
        //            {
        //                if (valuationItem.ChargesTypeCode.Value != "67" && valuationItem.ChargesTypeCode.Value != "144")
        //                {
        //                    //If there is a DECLARED Fee (I01) in message:
        //                    //1 - Do NOT get the CALCULATED Fee (I02) from message
        //                    //2 - Delete the CALCULATED from DB
        //                    if (declarationGoodsShipmentCustomsValuation_I01 != null && valuationItem.ChargesTypeCode.Value == "I02")
        //                    {
        //                        var supplierInvoiceModificationPM = supplierInvoicePM.SupplierInvoiceModifications.FirstOrDefault(si => si.TypeCode == valuationItem.ChargesTypeCode.Value);
        //                        if (supplierInvoiceModificationPM != null)
        //                        {
        //                            //If exist delete
        //                            supplierInvoiceModificationPM.ChangeSetOp = ChangeSetOperation.Delete;
        //                            supplierInvoiceModificationPMList.Add(supplierInvoiceModificationPM);
        //                        }
        //                    }
        //                    else
        //                    {
        //                        var supplierInvoiceModificationPM = supplierInvoicePM.SupplierInvoiceModifications.FirstOrDefault(si => si.TypeCode == valuationItem.ChargesTypeCode.Value);
        //                        if (supplierInvoiceModificationPM != null)
        //                        {
        //                            //If exist update
        //                            supplierInvoiceModificationPM.ChangeSetOp = ChangeSetOperation.Update;
        //                        }
        //                        else
        //                        {
        //                            //Else create new SupplierInvoiceModification record
        //                            supplierInvoiceModificationPM = new SupplierInvoiceModificationPM();
        //                            supplierInvoiceModificationPM.ChangeSetOp = ChangeSetOperation.Insert;
        //                        }
        //                        supplierInvoiceModificationPM.TypeCode = valuationItem.ChargesTypeCode.Value;
        //                        supplierInvoiceModificationPM.CurrencyTypeCode = valuationItem.OtherChargeDeductionAmount.currencyID.ToString(); // TO CHECK? ENUM?
        //                        supplierInvoiceModificationPM.Amount = valuationItem.OtherChargeDeductionAmount.Value;
        //                        if (supplierInvoiceModificationPM.ChangeSetOp == ChangeSetOperation.Insert)
        //                        {
        //                            supplierInvoiceModificationPMList.Add(supplierInvoiceModificationPM);
        //                        }
        //                    }
        //                }
        //            }

        //            return supplierInvoiceModificationPMList;
        //        }


        //        private void DeleteDeclarationConstraints(DeclarationConstraintUpdateService myDeclarationConstraintUpdateService) // Delete old Constraints
        //        {
        //            if (this._MyDeclarationPM.DeclarationConstraints == null)
        //            {
        //                return;
        //            }

        //            foreach (var constraintItem in this._MyDeclarationPM.DeclarationConstraints)
        //            {
        //                constraintItem.ChangeSetOp = ChangeSetOperation.Delete;
        //                 _MyDeclarationPM.DeletedDeclarationConstraints.Add(constraintItem);
        //            }
        //        }

        //        private List<DeclarationConstraintPM> BuildDeclarationConstraints(ResponseError[] responseError)
        //        {
        //            if (responseError == null)
        //            {
        //                return null;
        //            }

        //            var declarationConstraintPMList = new List<DeclarationConstraintPM>();
        //            foreach (var errorItem in responseError)
        //            {
        //                if (errorItem.DMExtensions != null)
        //                {
        //                    DeclarationConstraintPM declarationConstraintToCheckDistinct = null;
        //                    declarationConstraintToCheckDistinct = declarationConstraintPMList.Where(constraint => constraint.ConstraintNumber == errorItem.DMExtensions.ConstraintID.ToString()).FirstOrDefault();

        //                    if (declarationConstraintToCheckDistinct == null)
        //                    {
        //                        DeclarationConstraintPM declarationConstraint = null;
        //                        declarationConstraint = FindConstraintInList(errorItem.DMExtensions.ConstraintID.ToString(), _MyDeclarationPM.Tenant, _MyDeclarationPM.Id);

        //                        if (declarationConstraint == null)
        //                        {
        //                            declarationConstraint = new DeclarationConstraintPM();
        //                            declarationConstraint.ChangeSetOp = ChangeSetOperation.Insert;
        //                            declarationConstraint.ConstraintNumber = errorItem.DMExtensions.ConstraintID.ToString();
        //                            declarationConstraint.ConstraintTypeCode = errorItem.DMExtensions.ConstraintType.ToString();
        //                        }
        //                        else
        //                        {
        //                            declarationConstraint.ChangeSetOp = ChangeSetOperation.Update;
        //                            _MyDeclarationPM.DeletedDeclarationConstraints.Remove(declarationConstraint);
        //                        }

        //                        declarationConstraint.ConstraintStatusCode = errorItem.DMExtensions.ConstraintStatus.ToString();
        //                        declarationConstraintPMList.Add(declarationConstraint);
        //                    }
        //                }
        //            }
        //            return declarationConstraintPMList;
        //        }


        //        private DeclarationConstraintPM FindConstraintInList(string constraintNumber, int tenant, string declarationID)
        //        {
        //            List<DeclarationConstraintPM> constraintList = (from a in _MyDeclarationPM.DeclarationConstraints
        //                                                            where a.ConstraintNumber == constraintNumber &&
        //                                                            a.Tenant == tenant && a.DeclarationID == declarationID
        //                                                            select a).ToList();

        //            if (constraintList.Count > 0)
        //            {
        //                return constraintList[0];
        //            }
        //            return null;
        //        }
        //        private void UpdateDepositionStatusCode(DeclarationError declarationError)
        //        {
        //            if (declarationError != null && declarationError.Entitites != null && declarationError.Entitites.Count > 0)
        //            {
        //                //Go over all the 'Entity'
        //                foreach (var entity in declarationError.Entitites)
        //                {
        //                    if (entity.FieldErrors != null)
        //                    {
        //                        //Go over all the 'FieldErrors'
        //                        foreach (var fieldErrors in entity.FieldErrors)
        //                        {
        //                            //Get all 'FieldErrors' for the 'FieldError'
        //                            List<field> fieldList = (from a in entity.FieldErrors
        //                                                     where (a.Code == "4589")
        //                                                     select a).ToList();
        //                            if (fieldList.Count > 0)
        //                            {
        //                                if (string.IsNullOrWhiteSpace(_MyDeclarationPM.DepositionStatusCode)) _MyDeclarationPM.DepositionStatusCode = "R";
        //                                return;
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //            if (_MyDeclarationPM.DepositionStatusCode == "R") _MyDeclarationPM.DepositionStatusCode = null;
        //        }
        public UnifreightIIG.Common.ImportDeclarationServiceReference.ResponseStatus CastStatus(UnifreightIIG.Common.MessageLib.ID.ResponseStatus declaration)
        {


            string DeclarationString;
            using (var stringwriter = new System.IO.StringWriter())
            {
                var serializer = new XmlSerializer(declaration.GetType());
                serializer.Serialize(stringwriter, declaration);
                DeclarationString = stringwriter.ToString();
            }



            using (var stringReader = new System.IO.StringReader(DeclarationString))
            {
                var serializer = new XmlSerializer(typeof(UnifreightIIG.Common.ImportDeclarationServiceReference.ResponseStatus));
                return serializer.Deserialize(stringReader) as UnifreightIIG.Common.ImportDeclarationServiceReference.ResponseStatus;
            }
        }

        public UnifreightIIG.Common.ImportDeclarationServiceReference.Declaration CastDeclaration(UnifreightIIG.Common.MessageLib.ID.Declaration declaration )
        {


            string DeclarationString;
            using (var stringwriter = new System.IO.StringWriter())
            {
                var serializer = new XmlSerializer(declaration.GetType());
                serializer.Serialize(stringwriter, declaration);
                DeclarationString = stringwriter.ToString();
            }



            using (var stringReader = new System.IO.StringReader(DeclarationString))
            {
                var serializer = new XmlSerializer(typeof(UnifreightIIG.Common.ImportDeclarationServiceReference.Declaration));
                return serializer.Deserialize(stringReader) as UnifreightIIG.Common.ImportDeclarationServiceReference.Declaration;
            }
        }


        public UnifreightIIG.Common.ImportDeclarationServiceReference.ResponseError[] CastError(UnifreightIIG.Common.MessageLib.ID.ResponseError[] responseError)
        {


            string ErrorString;
            using (var stringwriter = new System.IO.StringWriter())
            {
                var serializer = new XmlSerializer(responseError.GetType());
                serializer.Serialize(stringwriter, responseError);
                ErrorString = stringwriter.ToString();
            }



            using (var stringReader = new System.IO.StringReader(ErrorString))
            {
                var serializer = new XmlSerializer(typeof(UnifreightIIG.Common.ImportDeclarationServiceReference.ResponseError[]));
                return serializer.Deserialize(stringReader) as UnifreightIIG.Common.ImportDeclarationServiceReference.ResponseError[];
            }
        }

        bool SendDeclarationPrintSync(GenericRequestParams requestParams)
        {

            bool SendDeclarationPrintDone = false;
            // Use this line to throw UnauthorizedAccessException, which we handle.
            Task<bool> task1 = Task<bool>.Factory.StartNew(() => SendDeclarationPrint(requestParams));

            // Use this line to throw an exception that is not handled. 
            //  Task task1 = Task.Factory.StartNew(() => { throw new IndexOutOfRangeException(); } ); 
            try
            {
                task1.Wait();
                SendDeclarationPrintDone = task1.Result;
            }
            catch (AggregateException ae)
            {
                if (true)
                {
                    throw ae.Flatten();
                }

                ae.Handle((x) =>
                {
                    if (x is UnauthorizedAccessException) // This we know how to handle.
                    {
                        Console.WriteLine("You do not have permission to access all folders in this path.");
                        Console.WriteLine("See your network administrator or try another path.");
                        return true;
                    }
                    return false; // Let anything else stop the application.
                });

            }

            Console.WriteLine("task1 has completed.");
            return SendDeclarationPrintDone;
        }

        public override INF_MSG_GenericResponseData GetResponse(DF_NG_5117_MSG14003_ImportDeclarationAmendmentReplyMsg customResponse, GenericRequestParams requestParams)
        {
            return this.MyResponseData;
        }
        
        public bool SendDeclarationPrint(GenericRequestParams requestParams)
        {
            LogMessagingUtil.Instance.AppendLine("SendDeclarationPrint");
            string decNum = this._MyDeclarationPMOrg != null ? this._MyDeclarationPMOrg.DeclarationNumber : _MyDeclarationPM.DeclarationNumber;
            var decNumList = new List<string>();
            decNumList.Add(decNum);
            DF_NG_8302_Web03_DeclarationPrintRequestParams searchParams = new DF_NG_8302_Web03_DeclarationPrintRequestParams()
            {
                LoggingEnabled = true,
                CustomFileNo = this._MyDeclarationPM.CustomFileNo,
                DeclarationNumber = decNumList, //declarationPM.DeclarationNumber,
                Tenant = this._MyDeclarationPM.Tenant,
                RequestName = "Declaration Print(5117)",
                ResponseName = "Declaration Print(5117)",
                LoggingEntityId = this._MyDeclarationPMOrg != null? this._MyDeclarationPMOrg.Id: _MyDeclarationPM.Id,
                RequestVIA = SendRequestVIA.WebServiceBatch,


                LoggingUserId = requestParams.LoggingUserId ,//HD CALL#298426
            }; 

             var myRequestMessagingService = new DF_NG_8302_Web03_DeclarationPrintMessagingService();
            var resData = myRequestMessagingService.Send(searchParams);
            _SendDeclarationPrintResponse = resData;
            if (!resData.Succeeded)
            {
                LogMessagingUtil.Instance.AppendLine("Declaration Print Request Failed " + resData.CustomsRequestsSheetId + ", Message: " + resData.UserMessage);
                return false;
            }
            LogMessagingUtil.Instance.AppendLine("Declaration Print Request Succeeded " + resData.CustomsRequestsSheetId);
            return true;
        }
    }
}
