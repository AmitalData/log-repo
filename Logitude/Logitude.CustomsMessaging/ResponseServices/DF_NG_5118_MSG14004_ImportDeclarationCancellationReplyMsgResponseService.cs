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
using UnifreightIIG.Common.MessageLib.ID;
using Logitude.BL.CommonDataModel.EntityQueries;
using UnifreightIIG.Common.MessageLib.Ransom;

namespace Logitude.CustomsMessaging.ResponseServices
{
    public class DF_NG_5118_MSG14004_ImportDeclarationCancellationReplyMsgResponseService : 
        ResponseServiceBase<INF_MSG_GenericResponseData, DF_NG_5118_MSG14004_DeclarationCancellationReplyMsg, GenericRequestParams>
    {
        DeclarationPM _MyDeclarationPM;
 
        private DeclarationPrintResponseData _SendDeclarationPrintResponse;
 

      
        public override void Update(DF_NG_5118_MSG14004_DeclarationCancellationReplyMsg customResponse, GenericRequestParams requestParams)
        {
            var context = CustomContext.GetContext(requestParams.Tenant);
            var myDeclarationQueryService = new DeclarationQueryService(context);
            var myDeclarationUpdateService = new DeclarationUpdateService(context, new Dictionary<string, IContext>(), requestParams.Tenant);
            this.MyResponseData = new INF_MSG_GenericResponseData();
            DeclarationCorrectionsPointerService myDeclarationCorrectionsPointerService = new DeclarationCorrectionsPointerService();
            string error = "";
            this.MyResponseData = new INF_MSG_GenericResponseData();


          

            if (customResponse.CancellationResponse== null  )
            {
                this.MyResponseData.ApplicationID = requestParams.AppicationId;
                this.MyResponseData.Succeeded = true;
                this.MyResponseData.UserMessage = "CancellationResponse  is null";
                this.MyResponseData.HasException = false;

                return;
            }
            FeatureQuery featureQuery = new FeatureQuery();
            bool bFromMehes = false;
            var features = featureQuery.GetAllowedFeaturesForLoggedUser(requestParams.LoggingUserId, requestParams.Tenant);

            var feature = features.Features.FirstOrDefault(x => x.Code == "DeclarationCancellation");
            if (feature != null)
            {
                _MyDeclarationPM = myDeclarationQueryService.GetSingleDeclarationByNumber(customResponse.CancellationResponse.DeclarationID, requestParams.Tenant);
                if (_MyDeclarationPM == null)
                {
                    this.MyResponseData.ApplicationID = requestParams.AppicationId;
                    this.MyResponseData.Succeeded = true;
                    this.MyResponseData.UserMessage = "לא נמצאה הצהרה מתאימה.";
                    this.MyResponseData.HasException = false;

                    return;
                }

                string loggingUserId = "";
                UserRepository userRepository = new UserRepository(_MyDeclarationPM.Tenant);
                var user = userRepository.GetSingleUserByCode("MEHES", _MyDeclarationPM.Tenant, true);
                if (user != null)
                {
                    loggingUserId = user.Id;
                }
                if (customResponse.CancellationResponse.FunctionalReferenceID ==null)
                {
                    bFromMehes = true;
                }


                _MyDeclarationPM.DeclarationStatusTypeCode = customResponse.CancellationResponse.DeclarationStatusID.ToString();

                if (customResponse.AdditionalInformation!= null && customResponse.AdditionalInformation.Count()>0)
                {
                    foreach (var item in customResponse.AdditionalInformation)
                    {
                        switch(item.StatementTypeCode)
                        {
                            case  30:
                                {
                                    if(bFromMehes)
                                    _MyDeclarationPM.CancelRequestReasonCode = item.Content;
                                    break;
                                }

                            case 33:
                                {
                                    _MyDeclarationPM.CancelRequestStatusCode= item.Content;

                              
                                    break;
                                }


                            case 22:
                                {
                                    _MyDeclarationPM.CustomCancelRequestRemarks = item.Content;
                                    break;
                                }


                            case 36:
                                {
                                    _MyDeclarationPM.CancelRequestRejectionReason = item.Content;
                                    break;
                                }


                            case 37:
                                {
                                     _MyDeclarationPM.CancelRequestApproveDate = DateTime.ParseExact(item.Content, "dd/MM/yyyy HH:mm:ss", null);
                                    break;
                                }

                            case 31:
                                {
                                    _MyDeclarationPM.IsClaimable = Convert.ToBoolean( item.Content);
                                    break;
                                }
                        }
                    }
                }


                switch (_MyDeclarationPM.CancelRequestStatusCode)
                {
                    case "6":
                        {
                            var amitalEventTracerModel = new Logitude.Customs.BL.TraceEvents.AmitalEventTracerModel()
                            {
                                Tenant = _MyDeclarationPM.Tenant,
                                objectTableName = "Customs.Declaration",
                                EventCode = "CRJ",
                                notes = "סיבת הדחיה: " + _MyDeclarationPM.CancelRequestRejectionReason
                                    + +'\n' + "הערות המכס לביטול: " + _MyDeclarationPM.CustomCancelRequestRemarks,
                                CommunicationLoggingEntityReference = _MyDeclarationPM.DeclarationNumber,
                                EntityId = _MyDeclarationPM.Id,
                                UserId = requestParams.LoggingUserId,

                                CommunicationSubject = "FU Status CRJ from logitude ",
                                MyFUStatus = new AmitalEventTracerModel.FUStatus()
                                {
                                    entname = "CFIFILEM",
                                    primary_number = _MyDeclarationPM.CustomFileNo,
                                    status = "new",
                                    xml_status = "new",
                                    status_id = "CRJ",
                                    status_DateTime = Convert.ToDateTime(_MyDeclarationPM.CancelRequestApproveDate),
                                    comments = "סיבת הדחיה: " + _MyDeclarationPM.CancelRequestRejectionReason
                                    + +'\n' + "הערות המכס לביטול: " + _MyDeclarationPM.CustomCancelRequestRemarks
 
 
                                }
                            };
                            AmitalEventTracer.CreateTraceEvent(amitalEventTracerModel);

                            break;
                        }
                    case "5":
                        {

                            var amitalEventTracerModel = new Logitude.Customs.BL.TraceEvents.AmitalEventTracerModel()
                            {
                                Tenant = _MyDeclarationPM.Tenant,
                                objectTableName = "Customs.Declaration",
                                EventCode = "CAP",
                                notes = "הערות המכס לביטול: " + _MyDeclarationPM.CustomCancelRequestRemarks,
                                CommunicationLoggingEntityReference = _MyDeclarationPM.DeclarationNumber,
                                EntityId = _MyDeclarationPM.Id,
                                UserId =bFromMehes? loggingUserId : requestParams.LoggingUserId,

                                CommunicationSubject = "FU Status CAP from logitude ",
                                MyFUStatus = new AmitalEventTracerModel.FUStatus()
                                {
                                    entname = "CFIFILEM",
                                    primary_number = _MyDeclarationPM.CustomFileNo,
                                    status = "new",
                                    xml_status = "new",
                                    status_id = "CAP",
                                    status_DateTime = Convert.ToDateTime(_MyDeclarationPM.CancelRequestApproveDate),
                                    comments ="הערות המכס לביטול: " + _MyDeclarationPM.CustomCancelRequestRemarks


                                }
                            };
                            AmitalEventTracer.CreateTraceEvent(amitalEventTracerModel);

                            break;
                        }
                }


                if (customResponse.ProceduralFaultMsg != null)
                {
                    var ProceduralFaultDetailsXml_5118 = XmlGenericUtil<UnifreightIIG.Common.MessageLib.ID.ProceduralFaultDetails[]>
                       .SerializeObject(customResponse.ProceduralFaultMsg);

                    var customResponse_8218 = new EV_NG_8218_MSG14100_ProceduralFaultMsg() { };
                    customResponse_8218.ProceduralFaultDetails = XmlGenericUtil<UnifreightIIG.Common.MessageLib.Fault.ProceduralFaultDetails[]>
                        .DeSerializeObject(ProceduralFaultDetailsXml_5118);

                    var ResponseService_8218 = new EV_NG_8218_MSG14100_ProceduralFaultMsgResponseService();
                    ResponseService_8218.Update(customResponse_8218, requestParams);
                }


                if (customResponse.CanceledDocumentDetails != null) // Create Document 
                {
                    LogMessagingUtil.Instance.AppendLine("ConstraintApprovalDecision: Create Document");

                    foreach (var documentItem in customResponse.CanceledDocumentDetails)
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
                this._MyDeclarationPM.ChangeSetOp = ChangeSetOperation.Update;
                myDeclarationUpdateService.Update(this._MyDeclarationPM, true);

                this.MyResponseData.ApplicationID = requestParams.AppicationId;
                this.MyResponseData.Succeeded = true;
                this.MyResponseData.HasException = false;
                this.MyResponseData.UserMessage = "מענה לביטול הצהרה  " + this._MyDeclarationPM.DeclarationNumber + " נקלט בהצלחה";

                this.MyRequestSheetParam = new RequestSheetParam();
                this.MyRequestSheetParam.CustomFileNo = this._MyDeclarationPM.CustomFileNo;
                this.MyRequestSheetParam.ObjectTableId1 = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
                this.MyRequestSheetParam.EntityId1 = this._MyDeclarationPM.Id;
                this.MyRequestSheetParam.RequestDescription = "מענה לביטול הצהרה  " + this._MyDeclarationPM.DeclarationNumber;



            }

            else
            {
                this.MyResponseData.ApplicationID = requestParams.AppicationId;
                this.MyResponseData.Succeeded = true;
                this.MyResponseData.UserMessage = "אין הרשאה לביטול הצהרה.";
                this.MyResponseData.HasException = false;

                return;
            }



         }
       
 
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

 
        public override INF_MSG_GenericResponseData GetResponse(DF_NG_5118_MSG14004_DeclarationCancellationReplyMsg customResponse, GenericRequestParams requestParams)
        {
            return this.MyResponseData;
        }
        
     }
}
