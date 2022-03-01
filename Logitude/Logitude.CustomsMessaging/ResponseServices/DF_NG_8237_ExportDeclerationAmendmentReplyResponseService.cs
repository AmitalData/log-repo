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
using UnifreightIIG.Common.MessageLib.Fault;
 using UnifreightIIG.Common.MessageLib.Collateral;
using Logitude.Customs.BL.Messaging.LogitudeClient.DeclarationErrorPointer;
using System.Xml.Serialization;
using Logitude.Customs.BL.TraceEvents;
using Simplog.Data.CommonDataModel.Repositories;
using Logitude.Customs.BL.Messaging.LogitudeClient.DeclarationErrorPointer.DBWCO;
 using UnifreightIIG.Common.MessageLib.Ransom;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.Customs.BL.Messaging.Maman;
using Logitude.Customs.BL.Messaging.Customs;
using UnifreightIIG.Common.ExportDeclarationAmendmentRequestMsgRequestServiceReference;

namespace Logitude.CustomsMessaging.ResponseServices
{
    public class DF_NG_8237_ExportDeclerationAmendmentReplyResponseService : ResponseServiceBase<INF_MSG_GenericResponseData, DF_NG_8237_MSG14003_ExportDeclarationAmendmentReplyMsg, AmendmentRequestParams>
    {
        DeclarationPM _MyDeclarationPM;
        DeclarationPM _MyDeclarationPMOrg;
        private bool isExportClose=false;

        private DeclarationPrintResponseData _SendDeclarationPrintResponse;


        public INF_MSG_GenericResponseData Update8237(DF_NG_8237_MSG14003_ExportDeclarationAmendmentReplyMsg customResponse, AmendmentRequestParams requestParams)
        {
            Update(customResponse, requestParams);
            return this.MyResponseData;
        }
        public override void Update(DF_NG_8237_MSG14003_ExportDeclarationAmendmentReplyMsg customResponse, AmendmentRequestParams requestParams)
        {
            var context = CustomContext.GetContext(requestParams.Tenant);
            var myDeclarationQueryService = new DeclarationQueryService(context);
            var myDeclarationUpdateService = new DeclarationUpdateService(context, new Dictionary<string, IContext>(), requestParams.Tenant);
            this.MyResponseData = new INF_MSG_GenericResponseData();
            DeclarationCorrectionsPointerService myDeclarationCorrectionsPointerService = new DeclarationCorrectionsPointerService();
            string error = "";
            DF_NG_2757_MSG10004_ExportAmendmentDeclarationResponseService dF_NG_2757_MSG10004_ExportFixedDeclarationResponseService = new DF_NG_2757_MSG10004_ExportAmendmentDeclarationResponseService();
 


            FeatureQuery featureQuery = new FeatureQuery();

         
                //var dec = new UnifreightIIG.Common.ImportDeclarationServiceReference.Declaration();

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


            _MyDeclarationPM = declaration;

            if (customResponse.Response !=null  && customResponse.Response.AdditionalInformation != null && customResponse.Response.AdditionalInformation.FirstOrDefault(x=>x.StatementTypeCode.Value=="28") != null )
            {
                isExportClose = true;
            }


            if(!isExportClose)
            {
                if (declaration != null)
                {
                    var AdditionalInformation = customResponse.Response.AdditionalInformation;
                    string status = "";
                    if (AdditionalInformation != null)
                    {
                        status = AdditionalInformation.FirstOrDefault(x => x.Content != null && x.StatementTypeCode.Value == "32").Content.Value;
                    }
                    if (customResponse.Response.Declaration != null && (status == "2" || status == "1"))
                        _MyDeclarationPM = dF_NG_2757_MSG10004_ExportFixedDeclarationResponseService.MapResponseToDeclaration(CastDeclaration(customResponse.Response.Declaration), requestParams.Tenant, false, _MyDeclarationPM.Id, out error, false, isUpdateAfterAccept: true);

                }
                else
                {

                    if (customResponse.Response.Declaration != null && customResponse.Response.Declaration.ID != null && customResponse.Response.Declaration.ID.Value != null && customResponse.Response.Declaration.ID.Value.Substring(2, 2) == "99")
                    {
                        _MyDeclarationPM = myDeclarationUpdateService.GetSertByConvertedDeclarationNumber(customResponse.Response.Declaration.ID.Value, requestParams.Tenant);

                        _MyDeclarationPM = dF_NG_2757_MSG10004_ExportFixedDeclarationResponseService.MapResponseToDeclaration(CastDeclaration(customResponse.Response.Declaration), requestParams.Tenant, false, _MyDeclarationPM.Id, out error, true);

                    }

                    else if (customResponse.Response.Declaration != null)
                    {
                        _MyDeclarationPM = myDeclarationQueryService.GetSingleByDecNoAndVersion(customResponse.Response.Declaration.ID.Value, customResponse.Response.Declaration.DMExtensions.VersionID.Value, requestParams.Tenant);


                        if (_MyDeclarationPM != null)
                        {
                            _MyDeclarationPM = dF_NG_2757_MSG10004_ExportFixedDeclarationResponseService.MapResponseToDeclaration(CastDeclaration(customResponse.Response.Declaration), requestParams.Tenant, false, _MyDeclarationPM.Id, out error, false, isUpdateAfterAccept: true);

                        }

                        else
                        {
                            string id = myDeclarationQueryService.GetIdByDeclarationNumber(customResponse.Response.Declaration.ID.Value, requestParams.Tenant);

                            _MyDeclarationPM = dF_NG_2757_MSG10004_ExportFixedDeclarationResponseService.MapResponseToDeclaration(CastDeclaration(customResponse.Response.Declaration), requestParams.Tenant, false, id, out error, false);

                        }
                        fromMehes = true;
                    }


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
                    var ProceduralFaultDetailsXml_8237 = XmlGenericUtil<UnifreightIIG.Common.ExportDeclarationAmendmentRequestMsgRequestServiceReference.ProceduralFaultDetails[]>
                       .SerializeObject(customResponse.ProceduralFaults);

                    var customResponse_8218 = new EV_NG_8218_MSG14100_ProceduralFaultMsg() { };
                    customResponse_8218.ProceduralFaultDetails = XmlGenericUtil<UnifreightIIG.Common.MessageLib.Fault.ProceduralFaultDetails[]>
                        .DeSerializeObject(ProceduralFaultDetailsXml_8237);

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


            DeclarationPM declarationParent = null;
                var declarationQueryService = new DeclarationQueryService(_MyDeclarationPM.Tenant);
                _MyDeclarationPMOrg =isExportClose? _MyDeclarationPM: declarationQueryService.GetSingle(_MyDeclarationPM.AmendmentOriginalDeclartation, true, false);


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
                ProcessLockTableUtil.Instance.GetProcessLockTableDisposable(requestParams.Tenant, true, key, "8237ResponseService.Update")
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
                                            PaymentOrderPM paymentOrder = paymentOrderQueryService.GetSingle(paymentOrderId, true, false);
                                            PaymentOrderConnectionTablePM paymentOrderConnectionTablePM= null;
                                        if (   paymentOrder.PaymentOrderConnectionTables!= null && paymentOrder.PaymentOrderConnectionTables.Count>0)
                                            {
                                                foreach (var item in paymentOrder.PaymentOrderConnectionTables)
                                                {
                                                    if(item.ConnectedEntityId == _MyDeclarationPMOrg.Id && item.ConnectedEntityCode=="D" )
                                                    {
                                                        item.ChangeSetOp = ChangeSetOperation.Delete;

                                                          paymentOrderConnectionTablePM = new PaymentOrderConnectionTablePM()
                                                        {
                                                            ConnectedEntityCode = "D",
                                                            ChangeSetOp = ChangeSetOperation.Insert,
                                                            PaymentOrderId = paymentOrderId,
                                                            ConnectedEntityId   = _MyDeclarationPM.Id,
                                                            Tenant = _MyDeclarationPM.Tenant

                                                        };




                                                    }
                                             
                                                }
                                          
                                            }
                                            if (paymentOrderConnectionTablePM != null)
                                            {
                                                paymentOrder.PaymentOrderConnectionTables.Add(paymentOrderConnectionTablePM);


                                                //  paymentOrder.FirstEntityID =  _MyDeclarationPM.Id;
                                                paymentOrder.ChangeSetOp = ChangeSetOperation.Update;
                                                PaymentOrderUpdateService pOUpdateservice = new PaymentOrderUpdateService(context, new Dictionary<string, IContext>(), requestParams.Tenant);
                                                pOUpdateservice.Update(paymentOrder, false);

                                                //_MyDeclarationPM = declarationQueryService.GetSingle(_MyDeclarationPM.Id, true, false);

                                            }
                                        }

                                    }
                                    break;
                                }
                            case "29":
                                {
                                    if (additionalInformation.Content != null)
                                        _MyDeclarationPM.AmendmentRemarks += '\n' + additionalInformation.Content.Value;
                                    if(!string.IsNullOrEmpty(_MyDeclarationPM.AmendmentRemarks)&&  _MyDeclarationPM.AmendmentRemarks.Length>=511)
                                    {
                                        _MyDeclarationPM.AmendmentRemarks = _MyDeclarationPM.AmendmentRemarks.Substring(0, 511);
                                    }
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
                                    _MyDeclarationPM.AmendmentStatus = additionalInformation.Content.Value;
                                    switch (additionalInformation.Content.Value)
                                        {
                                            case "1":
                                             if (!isExportClose)
                                            {
                                                  declarationParent = myDeclarationQueryService.GetAcceptDeclarationAmendment(_MyDeclarationPM.AmendmentOriginalDeclartation, requestParams.Tenant);
 

                                            _MyDeclarationPM.AmendmentDontDisplayInList = false;
                                                UpdateReplacingDeclaration(requestParams, myDeclarationQueryService, myDeclarationUpdateService);
                                                if (_MyDeclarationPM.AmendmentOriginalDeclartation != declarationParent.AmendmentOriginalDeclartation)
                                                {
                                                    UpdateParentDec(myDeclarationUpdateService, declarationParent);

                                                }
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

                                            }

                                            else
                                            {
                                                _MyDeclarationPM.IsExportClosed = true;
                                            }

                                            break;

                                            case "4":
                                              //  _MyDeclarationPM.AmendmentStatus = "4";

                                            if (!isExportClose)
                                            {
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

                                            }

                                        



                                                List<string> currentXmlVersionId = myDeclarationCorrectionsPointerService.GetVersionIdFromCorrectionXML(this._MyDeclarationPM.CorrectionsXml);
                                           
                                                    var customResponseResponseXml = XmlGenericUtil<Response>
                                                        .SerializeObject(customResponse.Response);

                                                    var importDeclarationServiceReferenceResponse = XmlGenericUtil<Response>
                                                        .DeSerializeObject(customResponseResponseXml);
                                                
                                                    List<error> systemMessagesList = new List<error>();
                                                
                                                    this._MyDeclarationPM.CorrectionsXml = myDeclarationCorrectionsPointerService.AnalyzeCorrectionsPointerExport(this._MyDeclarationPM.CorrectionsXml, importDeclarationServiceReferenceResponse, systemMessagesList, requestParams.Tenant,customResponse.ReferencesListMsg);
                                            

                                                break;
  


                                            case "2":
 
                                                 //_MyDeclarationPM.AmendmentStatus = "6";
 
                                            if (!isExportClose)
                                            {
                                                _MyDeclarationPM.AmendmentDontDisplayInList = false;

                                                declarationParent = myDeclarationQueryService.GetAcceptDeclarationAmendment(_MyDeclarationPM.AmendmentOriginalDeclartation, requestParams.Tenant);
                                                if (_MyDeclarationPM.AmendmentOriginalDeclartation != declarationParent.AmendmentOriginalDeclartation)
                                                {
                                                    UpdateParentDec(myDeclarationUpdateService, declarationParent);

                                                }
                                                UpdateReplacingDeclaration(requestParams, myDeclarationQueryService, myDeclarationUpdateService);


                                                var myAmitalEventTracerModel4 = new Logitude.Customs.BL.TraceEvents.AmitalEventTracerModel()
                                                {
                                                    Tenant = _MyDeclarationPM.Tenant,
                                                    objectTableName = "Customs.Declaration",
                                                    EventCode = "DMP",
                                                    notes = "תיקון הצהרה אושר חלקית - " + (_MyDeclarationPMOrg != null ? _MyDeclarationPMOrg.DeclarationNumber : _MyDeclarationPM.DeclarationNumber) + "מספר בקשה - " + _MyDeclarationPM.AmendmentRequestNumber,
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
                                                        comments = "תיקון הצהרה אושר חלקית - " + (_MyDeclarationPMOrg != null ? _MyDeclarationPMOrg.DeclarationNumber : _MyDeclarationPM.DeclarationNumber) + "מספר בקשה - " + _MyDeclarationPM.AmendmentRequestNumber,
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

                                            }

                                            break;


                                            case "6":

                                                //_MyDeclarationPM.AmendmentStatus = "1";

                                            if (!isExportClose)
                                            {
                                                var myAmitalEventTracerModel5 = new Logitude.Customs.BL.TraceEvents.AmitalEventTracerModel()
                                                {
                                                    Tenant = _MyDeclarationPM.Tenant,
                                                    objectTableName = "Customs.Declaration",
                                                    EventCode = "DWR",
                                                    notes = "תיקון הצהרה ממתין לטיפול המכס - " + (_MyDeclarationPMOrg != null ? _MyDeclarationPMOrg.DeclarationNumber : _MyDeclarationPM.DeclarationNumber) + " מספר בקשה - " + _MyDeclarationPM.AmendmentRequestNumber,
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
                                                        comments = "תיקון הצהרה ממתין לטיפול המכס - " + (_MyDeclarationPMOrg != null ? _MyDeclarationPMOrg.DeclarationNumber : _MyDeclarationPM.DeclarationNumber) + " מספר בקשה - " + _MyDeclarationPM.AmendmentRequestNumber,
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

                                            }

                                            break;
                                        };

                                    }


                                    break;
                                }
                        }
                   }
 
                    if (customResponse.Response.Error != null && (!new string[]{ "1","2"}.Contains( _MyDeclarationPM.AmendmentStatus)))
                    {
                        DeclarationErrorPointerService mydDclarationErrorPointerService = new DeclarationErrorPointerService();
                    if(!isExportClose)
                        this._MyDeclarationPM.AmendmentErrorXml = mydDclarationErrorPointerService.AnalyzeErrorPionterExport(CastError(customResponse.Response.Error), _MyDeclarationPM, WCOTypeEnum.WCO);
                    else
                        this._MyDeclarationPM.ExportClosedErrorXML = mydDclarationErrorPointerService.AnalyzeErrorPionterExport(CastError(customResponse.Response.Error), _MyDeclarationPM, WCOTypeEnum.WCO);

                }


                if (myUpdateEventContextTagModel != null)
                        this._MyDeclarationPM.CurrentContextTag = myUpdateEventContextTagModel;
                    if (fromMehes)
                    {
                        _MyDeclarationPM.AmendmentCorrectedByUserId = loggingUserId;
                        _MyDeclarationPM.AmendmentissueDate = DateTime.ParseExact(customResponse.Response.Declaration.IssueDateTime, "yyyy-MM-ddTHH:mm:ss", null);

 
                    }
 
                    if(_MyDeclarationPM.AmendmentStatus == "1"  || _MyDeclarationPM.AmendmentStatus == "2")
                 {
                    _MyDeclarationPM.DeclarationStatusTypeCode = customResponse.Response.Status[0].NameCode.Value;
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

                    if (!isExportClose)
                    {
                        List<string> currentXmlVersionId = myDeclarationCorrectionsPointerService.GetVersionIdFromCorrectionXML(this._MyDeclarationPM.CorrectionsXml);
                    if ((customResponse.Response.Declaration != null) && (currentXmlVersionId == null || !currentXmlVersionId.Contains(customResponse.Response.Declaration.DMExtensions.VersionID.Value)))
                    {

                        var customResponseResponseXml = XmlGenericUtil<Response>
                            .SerializeObject(customResponse.Response);

                        var importDeclarationServiceReferenceResponse = XmlGenericUtil<Response>
                            .DeSerializeObject(customResponseResponseXml);
                        ////8237 
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
                        this._MyDeclarationPM.CorrectionsXml = myDeclarationCorrectionsPointerService.AnalyzeCorrectionsPointerExport(this._MyDeclarationPM.CorrectionsXml, importDeclarationServiceReferenceResponse, systemMessagesList, requestParams.Tenant, customResponse.ReferencesListMsg);
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
                            var requestXml = XmlGenericUtil<UnifreightIIG.Common.ExportDeclarationAmendmentRequestMsgRequestServiceReference.CollateralRequestDetails[]>.SerializeObject(customResponse.CollateralRequests);
                            var collateralArry = XmlGenericUtil<UnifreightIIG.Common.MessageLib.Collateral.CollateralRequestDetails[]>.DeSerializeObject(requestXml);

                            COLT_NG_8211_MSG10040_CollateralRequestMsg myCOLT_NG_8211_MSG10040_CollateralRequestMsg = new COLT_NG_8211_MSG10040_CollateralRequestMsg();
                            var responseContentHeader = customResponse.ResponseContentHeader;
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

                    else
                    {

                        this._MyDeclarationPM.ChangeSetOp = ChangeSetOperation.Update;
                        myDeclarationUpdateService.Update(this._MyDeclarationPM, true);

                    }
                    if (customResponse.Response.Declaration != null  && !isExportClose)
                    {
                        DF_NG_2757_MSG10004_ExportDeclarationResponseService dF_NG_2757_MSG10004_ExportDeclarationResponseService = new DF_NG_2757_MSG10004_ExportDeclarationResponseService();

                        UnifreightIIG.Common.ExportDeclarationServiceReference.DF_NG_2757_MSG10004_ExportDeclarationResponse dec_2757 = new UnifreightIIG.Common.ExportDeclarationServiceReference.DF_NG_2757_MSG10004_ExportDeclarationResponse();
                        dec_2757.Response = new UnifreightIIG.Common.ExportDeclarationServiceReference.Response();
                        dec_2757.Response.Declaration = CastDeclaration(customResponse.Response.Declaration);
                        dec_2757.Response.Status = new UnifreightIIG.Common.ExportDeclarationServiceReference.ResponseStatus[] {  CastStatus(customResponse.Response.Status[0]) };


                        if (customResponse.Response.Error != null)
                            dec_2757.Response.Error = CastError(customResponse.Response.Error);


                        GenericRequestParams requestParams_2757 = new GenericRequestParams
                        {
                            Tenant = _MyDeclarationPM.Tenant,
                            AppicationId = _MyDeclarationPM.Id,
                            ResponseName = "8237"
                        };

                        dF_NG_2757_MSG10004_ExportDeclarationResponseService.Update(dec_2757, requestParams_2757);

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


                if(_MyDeclarationPM.IsCourierDeclaration &&( _MyDeclarationPM.AmendmentStatus =="1" || _MyDeclarationPM.AmendmentStatus == "2" ) && _MyDeclarationPM.HatraDate ==null)

                {
                    var mySend2MasofIfNeededService = new Send2MasofIfNeededService();
                    mySend2MasofIfNeededService.Send2Masof(_MyDeclarationPM, false, _MyDeclarationPM, true);


                    SendManifest(_MyDeclarationPM, requestParams); 

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


 


        public void SendManifest(DeclarationPM declarationPM, GenericRequestParams requestParams)  
        {

            var objectTableId = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
            var objectTableIdCourierMaster = ObjectTableRepository.GetObjectTableByName("Customs.CourierMaster");

            var requestParams1170 = new MANIFESTRequestRequestParams()
            {
                Tenant = requestParams.Tenant,
                LoggingEnabled = true,
                LoggingObjectTableId = objectTableId,
                LoggingEntityId = declarationPM.Id,
                LoggingObjectTableId2 = objectTableIdCourierMaster,
                LoggingEntityId2 = declarationPM.CourierMasterId,
                InterfaceTypeCode = "1170",
                LoggingUserId = requestParams.LoggingUserId,
                RequestVIA = SendRequestVIA.WebServiceBatch,
                DeclarationId = declarationPM.Id,
                LoggingEntityReference = declarationPM.Id,

            };
            SBQMessageService.CreateSheetSBQMessage<MANIFESTRequestRequestParams>(requestParams1170, false);
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
                declarationReplacing.AmendmentStatus = "9";
                declarationReplacing.ChangeSetOp = ChangeSetOperation.Update;
                myDeclarationUpdateService.Update(declarationReplacing, true);

            }
        }


        public UnifreightIIG.Common.ExportDeclarationServiceReference.ResponseStatus CastStatus(ResponseStatus declaration)
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
                var serializer = new XmlSerializer(typeof(UnifreightIIG.Common.ExportDeclarationServiceReference.ResponseStatus));
                return serializer.Deserialize(stringReader) as UnifreightIIG.Common.ExportDeclarationServiceReference.ResponseStatus;
            }
        }

        public UnifreightIIG.Common.ExportDeclarationServiceReference.Declaration CastDeclaration(Declaration declaration )
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
                var serializer = new XmlSerializer(typeof(UnifreightIIG.Common.ExportDeclarationServiceReference.Declaration));
                return serializer.Deserialize(stringReader) as UnifreightIIG.Common.ExportDeclarationServiceReference.Declaration;
            }
        }


        public UnifreightIIG.Common.ExportDeclarationServiceReference.ResponseError[] CastError(ResponseError[] responseError)
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
                var serializer = new XmlSerializer(typeof(UnifreightIIG.Common.ExportDeclarationServiceReference.ResponseError[]));
                return serializer.Deserialize(stringReader) as UnifreightIIG.Common.ExportDeclarationServiceReference.ResponseError[];
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

        public override INF_MSG_GenericResponseData GetResponse(DF_NG_8237_MSG14003_ExportDeclarationAmendmentReplyMsg customResponse, AmendmentRequestParams requestParams)
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
                RequestName = "Declaration Print(8237)",
                ResponseName = "Declaration Print(8237)",
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
