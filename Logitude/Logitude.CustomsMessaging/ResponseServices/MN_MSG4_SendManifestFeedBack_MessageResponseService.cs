using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.BL.Messaging.LogitudeClient.DeclarationErrorPointer;
using Logitude.Customs.BL.NotificationBL;
using Logitude.Customs.BL.TraceEvents;
using Logitude.Customs.Data;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.ResponseServices.DeclarationErrorPointer;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.MessageLib.Storage;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using UnifreightIIG.Common.MessageLib.Docs;
using Logitude.AmitalMessaging.Customs.CustomFile;
using Logitude.AmitalMessaging.Utils;
using Unifreight.BL.EntityPMs.UGenerated;
using Unifreight.BL.EntityQueryServices;
using System.Collections;
using Unifreight.Data.AmitalModel;
using UnifreightIIG.Common.MANIFESTRequestServiceReference;
using UnifreightIIG.Common.CommonIIGInterface;
using UnifreightIIG.Common.FaultProceduralDetailsServiceReference;
using Logitude.Customs.BL.Models;
using Logitude.Customs.BL.Messaging.LogitudeClient.DeclarationErrorPointer.DBWCO;
using Logitude.Customs.BL.BL;
using Logitude.Customs.BL.Messaging.Customs;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.Server.Tools.Utils;

namespace Logitude.CustomsMessaging.ResponseServices
{

    public class MN_MSG4_SendManifestFeedBack_MessageResponseService : ResponseServiceBase<
        MANIFESTRequestResponseData, MN_MSG4_SendManifestFeedBack_Message, MANIFESTRequestRequestParams>
    {
        public UnifreightIIG.Common.CommonIIGInterface.IResponseHeaderOrFault _ResponseHeaderExeption;
        public bool _IsSubmitDeclarationResponse { get; set; }
        DeclarationPM _MyDeclarationPM;
        ConsignmentPM _MyConsignmentPM;
        private GTRTRANQueryService _GTRTRANQueryService;
        private Dictionary<string, IList> _MyLocalCache = new Dictionary<string, IList>();
        private AmitalContext _AmitalContext;


        public override void OnRequestFail(MN_MSG4_SendManifestFeedBack_Message customResponse, MANIFESTRequestRequestParams requestParams)
        {
            if (!String.IsNullOrWhiteSpace(requestParams.DeclarationId))
            {
                CalculateDeclarationCourierStatus.UpdateCourierManifestStatusCode(requestParams.Tenant, requestParams.DeclarationId);
            }
            base.OnRequestFail(customResponse, requestParams);
        }

      

        public override void Update(MN_MSG4_SendManifestFeedBack_Message customResponse, MANIFESTRequestRequestParams requestParams)
        {
            var responseName = requestParams.ResponseName;
            var context = CustomContext.GetContext(requestParams.Tenant);
            var myDeclarationQueryService = new DeclarationQueryService(context);
            var myDeclarationUpdateService = new DeclarationUpdateService(context, new Dictionary<string, IContext>(), requestParams.Tenant);
            DeclarationErrorPointerService mydDclarationErrorPointerService = new DeclarationErrorPointerService();
            EventContextTagModel myEventContextTagModel = new EventContextTagModel();

            this.MyResponseData = new MANIFESTRequestResponseData();


            if (string.IsNullOrWhiteSpace(requestParams.DeclarationId))
            {
                var errMess = "No DeclarationId in requestParams";
                LogMessagingUtil.Instance.AppendLine(errMess);
                this.MyResponseData.UserMessage = errMess;
                this.MyResponseData.HasException = true;
                return;
            }

            _MyDeclarationPM = myDeclarationQueryService.GetSingle(requestParams.DeclarationId, true, false);

            if (this._MyDeclarationPM == null)
            {
                var errMess = "Can not find Declaration for Id " + requestParams.DeclarationId;
                LogMessagingUtil.Instance.AppendLine(errMess);
                this.MyResponseData.UserMessage = errMess;
                this.MyResponseData.HasException = true;
                return;
            }
            bool lockit = false;
            string key = "";
            if (_MyDeclarationPM.IsCourierDeclaration)
            {
                lockit = true;
                key = ProcessLockTableUtil.Instance.GetKey4UpdateDeclarationCourier_DocumentStatusCode(_MyDeclarationPM.Id, requestParams.Tenant);
            }
            using (var processLockTableDisposable = ProcessLockTableUtil.Instance.GetProcessLockTableDisposable(requestParams.Tenant, lockit, key, "CRS:2715/UDLT"))

            { 
                //Checking foe Exceptions
                if (customResponse.ResponseContentHeader.Exception != null || _ResponseHeaderExeption != null)
            {
                if (customResponse.ResponseContentHeader.Exception != null)
                {
                    this.MyResponseData.UserMessage = customResponse.ResponseContentHeader.Exception.FirstOrDefault().ExeptionDescription;
                }
                else
                {
                    this.MyResponseData.UserMessage = _ResponseHeaderExeption.ErrorDescription;
                }
                //this.MyResponseData.HasException = true;
                //return;
            }

            if (customResponse.ResponseContentHeader != null)
            {
                if (customResponse.ResponseContentHeader.ApplicationID == 0)
                {
                    if (customResponse.ResponseContentHeader.Exception != null)
                    {
                        if (String.IsNullOrWhiteSpace(customResponse.ResponseContentHeader.Exception.FirstOrDefault().ExeptionDescription))
                        {
                            var errMess = "No Declaration details in the Response " + requestParams.DeclarationId;
                            LogMessagingUtil.Instance.AppendLine(errMess);
                            this.MyResponseData.UserMessage = errMess;
                        }
                        else
                        {
                            var errMess = "No Declaration details in the Response " + customResponse.ResponseContentHeader.Exception.FirstOrDefault().ExeptionDescription + ". " + requestParams.DeclarationId;
                            LogMessagingUtil.Instance.AppendLine(errMess);
                            this.MyResponseData.UserMessage = errMess;
                        }
                    }
                    //return;
                }
            }

            //if (customResponse.Response == null)
            //{
            //    if (customResponse.ResponseContentHeader.Exception != null)
            //    {
            //        if (String.IsNullOrWhiteSpace(customResponse.ResponseContentHeader.Exception.FirstOrDefault().ExeptionDescription))
            //        {
            //            var errMess = "No Declaration details in the Response " + requestParams.DeclarationId;
            //            LogMessagingUtil.Instance.AppendLine(errMess);
            //            this.MyResponseData.UserMessage = errMess;
            //        }
            //        else
            //        {
            //            var errMess = "No Declaration details in the Response " + customResponse.ResponseContentHeader.Exception.FirstOrDefault().ExeptionDescription + ". " + requestParams.DeclarationId;
            //            LogMessagingUtil.Instance.AppendLine(errMess);
            //            this.MyResponseData.UserMessage = errMess;
            //        }
            //    }
            //    //return;
            //}

            
            if (!(!string.IsNullOrWhiteSpace(this.MyResponseData.UserMessage) && customResponse.Response == null))
            {
                /*
                 if(customResponse.ResponseContentHeader.Exception != null)
            {
                string userMessage = "";
                this._MyDeclarationPM.MarkAsChanged = false;
                var swErrosXml = Stopwatch.StartNew();

                foreach (UnifreightIIG.Common.ImportDeclarationServiceReference.Exception exception in customResponse.ResponseContentHeader.Exception)
                {
                    if (!string.IsNullOrWhiteSpace(userMessage))
                    {
                        userMessage = userMessage + @"
";
                    }
                    userMessage = userMessage + exception.ExeptionDescription;
                    this._MyDeclarationPM.ErrosXml = mydDclarationErrorPointerService.AddDeclarationException(this._MyDeclarationPM.ErrosXml, "Buisness", exception, true);
                    LogMessagingUtil.Instance.AppendLine("ErrosXml:Took:" + swErrosXml.ElapsedMilliseconds);
                    if (exception.ExeptionType == 2794)
                    {
                        this._MyDeclarationPM.DeclarationNumber = exception.ExceptionParms.FirstOrDefault();
                        if (!string.IsNullOrWhiteSpace(userMessage))
                        {
                            userMessage = userMessage + @"
";
                        }
                        userMessage = userMessage + "עודכן מספר ההצהרה לפי רשומת הסוכן - יש לשדר את ההצהרה מחדש";
                    }
                }
                if (_IsSubmitDeclarationResponse != true) // only for sending declaration (2750)
                {
                    _MyDeclarationPM.IsChanged = true;
                }

                if (_MyDeclarationPM.UserNotes == "LoadTestOnProgress")
                {
                    _MyDeclarationPM.UserNotes = "LoadTest";
                }

                this._MyDeclarationPM.ChangeSetOp = ChangeSetOperation.Update;
                this._MyDeclarationPM.CurrentContextTag = Logitude.Customs.BL.EntityUpdateServices.DeclarationUpdateService.UpdateIIGExcptionConst;
                myDeclarationUpdateService.Update(this._MyDeclarationPM, true);

                this.MyResponseData.ApplicationID = requestParams.AppicationId;
                this.MyResponseData.Succeeded = true;
                this.MyResponseData.UserMessage = userMessage;
                this.MyResponseData.HasException = true;

                return;
                }

                var swErrosXml1 = Stopwatch.StartNew();
            this._MyDeclarationPM.ErrosXml = mydDclarationErrorPointerService.AnalyzeErrorPionter(customResponse.Response.Error, customResponse.Response.Declaration);
            this._MyDeclarationError = mydDclarationErrorPointerService._declarationErrorPointer;
            LogMessagingUtil.Instance.AppendLine("ErrosXml:Took:" + swErrosXml1.ElapsedMilliseconds);


            
                 */
                _MyDeclarationPM.ManifestErrorXml = "";
                if (customResponse.Response.Error != null && customResponse.Response.Error.Count() > 0)
                {
                    //In order to use mydDclarationErrorPointerService.AnalyzeErrorPionter:
                    //Transfer the MANIFEST.ResponseError and MANIFEST.Declaration into ImportDeclaration.ResponseError and ImportDeclaration.Declaration
                    var errorXml = XmlGenericUtil<UnifreightIIG.Common.MANIFESTRequestServiceReference.ResponseError[]>.SerializeObject(customResponse.Response.Error);
                    UnifreightIIG.Common.ImportDeclarationServiceReference.ResponseError[] responseError = XmlGenericUtil<UnifreightIIG.Common.ImportDeclarationServiceReference.ResponseError[]>.DeSerializeObject(errorXml);

                    //var declarationXml = XmlGenericUtil<UnifreightIIG.Common.MANIFESTRequestServiceReference.Declaration>.SerializeObject(customResponse.Response.Declaration);
                    //UnifreightIIG.Common.ImportDeclarationServiceReference.Declaration declaration = XmlGenericUtil<UnifreightIIG.Common.ImportDeclarationServiceReference.Declaration>.DeSerializeObject(declarationXml);
                    UnifreightIIG.Common.ImportDeclarationServiceReference.Declaration declaration = new UnifreightIIG.Common.ImportDeclarationServiceReference.Declaration();

                    _MyDeclarationPM.ManifestErrorXml = mydDclarationErrorPointerService.AnalyzeErrorPionter(responseError, _MyDeclarationPM, WCOTypeEnum.Manifest, false);



                    //Translate 'listVersionID' of the errors:
                    //1       ==>   4
                    //2 or 3  ==>   1
                    DeclarationError myDeclarationError = XmlGenericUtil<DeclarationError>.DeSerializeObject(_MyDeclarationPM.ManifestErrorXml);
                    foreach (var entitite in myDeclarationError.Entitites)
                    {
                        entitite.FieldErrors.Where(c => c.ListVersionID == "1").ToList().ForEach(n => n.ListVersionID = "4");
                        entitite.EntityErrors.Where(c => c.ListVersionID == "1").ToList().ForEach(n => n.ListVersionID = "4");

                        entitite.FieldErrors.Where(c => c.ListVersionID == "2" || c.ListVersionID == "3").ToList().ForEach(n => n.ListVersionID = "1");
                        entitite.EntityErrors.Where(c => c.ListVersionID == "2" || c.ListVersionID == "3").ToList().ForEach(n => n.ListVersionID = "1");
                    }
                    _MyDeclarationPM.ManifestErrorXml = XmlGenericUtil<DeclarationError>.SerializeObject(myDeclarationError);



                    var errors = customResponse.Response.Error.Where(r => r.ValidationCode.listVersionID == "3").ToList();
                    if (errors != null && errors.Count() > 0)
                    {
                        foreach (var error in errors)
                        {
                            this.MyResponseData.UserMessage = this.MyResponseData.UserMessage + @"
" + error.ValidationCode.name;
                        }
                        
                        _MyDeclarationPM.ManifestCargoStatusCode = "3";
                    }
                    else
                    {
                        errors = customResponse.Response.Error.Where(r => r.ValidationCode.listVersionID == "2").ToList();
                        if (errors != null && errors.Count() > 0)
                        {
                            foreach (var error in errors)
                            {
                                this.MyResponseData.UserMessage = this.MyResponseData.UserMessage + @"
" + error.ValidationCode.name;
                            }
                            _MyDeclarationPM.ManifestCargoStatusCode = "2";
                        }
                        else
                        {
                            _MyDeclarationPM.ManifestCargoStatusCode = "1";
                        }
                    }
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(this.MyResponseData.UserMessage))
                    {
                        _MyDeclarationPM.ManifestCargoStatusCode = "3";
                    }
                    else
                    {
                        _MyDeclarationPM.ManifestCargoStatusCode = "1";
                    }
                }


                if (customResponse.ResponseContentHeader.Exception != null)
                {
                    string userMessage = "";

                    foreach (UnifreightIIG.Common.MANIFESTRequestServiceReference.Exception exception in customResponse.ResponseContentHeader.Exception)
                    {
                        if (!string.IsNullOrWhiteSpace(userMessage))
                        {
                            userMessage = userMessage + @"
";
                        }
                        userMessage = userMessage + exception.ExeptionDescription;
                        this._MyDeclarationPM.ManifestErrorXml = mydDclarationErrorPointerService.AddManifestException(this._MyDeclarationPM.ManifestErrorXml, "Buisness", exception, true);

                    }
                    if (!string.IsNullOrWhiteSpace(userMessage))
                    {
                        this.MyResponseData.UserMessage = this.MyResponseData.UserMessage + @"
" + userMessage;
                    }
                }

                if (!string.IsNullOrWhiteSpace(customResponse.ResponseContentHeader.Remark))
                {
                    var manifestException = new UnifreightIIG.Common.MANIFESTRequestServiceReference.Exception();
                    manifestException.ExeptionDescription = customResponse.ResponseContentHeader.Remark;
                    this._MyDeclarationPM.ManifestErrorXml = mydDclarationErrorPointerService.AddManifestException(this._MyDeclarationPM.ManifestErrorXml, "Warning", manifestException);
                    this.MyResponseData.UserMessage = this.MyResponseData.UserMessage + @"
" + customResponse.ResponseContentHeader.Remark;
                }
                
                   
            }

            
                if (_MyDeclarationPM.ManifestCargoStatusCode=="1" || _MyDeclarationPM.ManifestCargoStatusCode=="2" && !_MyDeclarationPM.EffectiveFlight)//MNC its success !
                {
                    DeclarationCourierStatusQueryService declarationCourierStatusQueryService = new DeclarationCourierStatusQueryService(context);
                    DeclarationCourierStatusPM declarationCourierStatusPM = declarationCourierStatusQueryService.GetSingle(requestParams.DeclarationId, true, false);



                    FeatureQuery featureQuery = new FeatureQuery();

                    var features = featureQuery.GetAllowedFeaturesForLoggedUser(requestParams.LoggingUserId, requestParams.Tenant);

                    var feature = features.Features.FirstOrDefault(x => x.Code == "SendDeclaration");
                   

                        if ((declarationCourierStatusPM != null && declarationCourierStatusPM.CourierDeclarationStatusCode == "R" && feature != null) || (declarationCourierStatusPM != null && declarationCourierStatusPM.CourierDeclarationStatusCode == "V" && declarationCourierStatusPM.CourierPaymentStatusCode != "P" && declarationCourierStatusPM.CourierPaymentStatusCode != "O"))
                    {
                        LogMessagingUtil.Instance.AppendLine($"  if ((declarationCourierStatusPM != null && declarationCourierStatusPM.CourierDeclarationStatusCode == 'R' && feature != null) || (declarationCourierStatusPM != null && declarationCourierStatusPM.CourierDeclarationStatusCode == 'V' && declarationCourierStatusPM.CourierPaymentStatusCode != 'P' && declarationCourierStatusPM.CourierPaymentStatusCode != 'O'))");
                        if (_MyDeclarationPM.TaxationDateTime < DateTime.Now.Date)
                        {
                            _MyDeclarationPM.TaxationDateTime = DateTime.Now.Date;//לפני השליחה יש לעדכן את תאריך חישוב המיסים לתאריך נוכחי על מנת להמנע מטיוטה שגויה
                        }
                        using (var trans = TransactionFactory.GetNewTransaction())// I PREFERRED WITHOUT TRANS BUT  (TO 1345- 1415). .
                        {
                            OnSucceededSendDeclarationDelay1Min(requestParams);
                            trans.Complete();
                        }
                    }

                   
                

                
            }

            _MyDeclarationPM.ChangeSetOp = ChangeSetOperation.Update;
            myDeclarationUpdateService.Update(_MyDeclarationPM, true);
            

    
            MyResponseData.ApplicationID = requestParams.ImportManifest;
            MyResponseData.Succeeded = true;


            }
        }

        private void OnSucceededSendDeclarationDelay1Min(MANIFESTRequestRequestParams requestParams)
        {

            try
            {
                var objectTableId = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");

                var requestParams2750 = new GenericRequestParams()
                {
                    Tenant = requestParams.Tenant,
                    //IsFakeResponse = true,
                    //RequestName = requestName,
                    //ResponseName = responseName,
                    LoggingEnabled = true,
                    LoggingObjectTableId = objectTableId,
                    LoggingEntityId = _MyDeclarationPM.Id,
                    //LoggingObjectTableId2 = requestParams.LoggingObjectTableId,
                    //LoggingEntityId2 = _CourierDeclarationPM,
                    AppicationId = _MyDeclarationPM.Id,
                    InterfaceTypeCode = "2750",

                    //LoggingEntityReference = declarationNumber,
                    LoggingUserId = requestParams.LoggingUserId,
                    RequestVIA = SendRequestVIA.WebServiceBatch,
                   
                };

                SBQMessageService.CreateSheetSBQMessage<GenericRequestParams>(requestParams2750, false,DateTime.Now.AddMinutes(2));
                LogMessagingUtil.Instance.AppendLine($" OnSucceededSendDeclarationDelay1Min SheetSBQ ({requestParams2750.PBId})");
                

            }
            catch (System.Exception ee1)
            {

                LogMessagingUtil.Instance.AppendLine($"Exception!!!OnSucceededSendDeclarationDelay1Min({_MyDeclarationPM.Id}) : {ee1.Message}");
                
            }
        }

        private void RaiseEvent(DeclarationPM dirtyDeclarationPM, string loggingUserId, EventContextTagModel myEventContextTagModel)
        {
            try
            {
                var myAmitalEventTracerModel = new Logitude.Customs.BL.TraceEvents.AmitalEventTracerModel()
                {
                    Tenant = dirtyDeclarationPM.Tenant,
                    objectTableName = "Customs.Declaration",
                    EventCode = myEventContextTagModel.EventCode,
                    notes = myEventContextTagModel.EventRemarks,
                    CommunicationLoggingEntityReference = dirtyDeclarationPM.DeclarationNumber,
                    EntityId = dirtyDeclarationPM.Id,
                    UserId = loggingUserId,

                    CommunicationSubject = "FU Status " + myEventContextTagModel.EventCode + " from logitude ",
                    MyFUStatus = new AmitalEventTracerModel.FUStatus()
                    {
                        entname = "CFIFILEM",
                        primary_number = dirtyDeclarationPM.CustomFileNo,
                        status = "new",
                        xml_status = "new",
                        status_id = myEventContextTagModel.EventCode,
                        status_DateTime = DateTime.Now,
                        status_save = "no_fail",
                        comments = myEventContextTagModel.EventRemarks,
                    }
                };


                LogMessagingUtil.Instance.AppendLine("AmitalEventTracer.CreateTraceEvent  eventCode =" + myEventContextTagModel.EventCode + "  CustomFileNo= " + dirtyDeclarationPM.CustomFileNo + "   ");
                AmitalEventTracer.CreateTraceEvent(myAmitalEventTracerModel);

            }
            catch (System.Exception ex)
            {
                // TODO: BL Stop Execute or Cuntinue - Ask IHAB
                throw;
            }

        }


        public override MANIFESTRequestResponseData GetResponse(MN_MSG4_SendManifestFeedBack_Message customResponse, MANIFESTRequestRequestParams requestParams)
        {
            return this.MyResponseData;
        }

        private string GetErrosXmlFromResponseHeaderExeption()
        {
            return _ResponseHeaderExeption.ErrorDescription;
        }
    }
}




