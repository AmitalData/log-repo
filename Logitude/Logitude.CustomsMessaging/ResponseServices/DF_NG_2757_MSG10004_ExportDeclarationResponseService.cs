using Logitude.AmitalMessaging.Utils;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.Def.Messaging.Customs;
using Logitude.Customs.BL.Messaging.LogitudeClient.DeclarationErrorPointer;
using Logitude.Customs.BL.TraceEvents;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Data.EntityLists;
using Logitude.Customs.Data.Repsitories;
using Logitude.CustomsMessaging.Common.Gen;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.MessagingServices;
using Logitude.CustomsMessaging.ResponseServices.DeclarationErrorPointer;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnifreightIIG.Common.MessageLib.Collateral;
using Logitude.Customs.BL.Messaging.LogitudeClient.DeclarationErrorPointer.DBWCO;
using Logitude.Customs.BL.BL;
using Unifreight.BL.EntityQueryServices;
using Unifreight.Data.AmitalModel;
using UnifreightIIG.Common.ExportDeclarationServiceReference;
using Exception = System.Exception;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel;
using System.IO;
using System.Xml.Serialization;
using Logitude.BL.CommonDataModel.EntityQueries;

namespace Logitude.CustomsMessaging.ResponseServices
{

    public class DF_NG_2757_MSG10004_ExportDeclarationResponseService :
        ResponseServiceBase<INF_MSG_GenericResponseData, DF_NG_2757_MSG10004_ExportDeclarationResponse, GenericRequestParams>
    {

        DeclarationPM _MyDeclarationPM;
        private bool _FastDelete;
        DateTime _DateTime;

        public bool _IsSubmitDeclarationResponse { get; set; }
        public bool _IsRetrieveDeclarationResponse { get; set; }
        decimal? totGeneralTaxCalc = 0;
        decimal? totPurchaseCalc = 0;
        decimal? totVatCalc = 0;
        decimal? generalTax = 0;
        decimal? purchase = 0;
        decimal? vat = 0;

        DeclarationError _MyDeclarationError;
        decimal? _TotalBtlCoverageNISSum = 0;

        public override void OnRequestFail(DF_NG_2757_MSG10004_ExportDeclarationResponse customResponse, GenericRequestParams requestParams)
        {
            if (!String.IsNullOrWhiteSpace(requestParams.AppicationId))
            {
                CalculateDeclarationCourierStatus.UpdateCourierDeclarationStatusCode(requestParams.Tenant, requestParams.AppicationId);
            }
            base.OnRequestFail(customResponse, requestParams);
        }

        public override INF_MSG_GenericResponseData GetResponse(
            DF_NG_2757_MSG10004_ExportDeclarationResponse customResponse, GenericRequestParams requestParams)
        {

             return this.MyResponseData;
        }

        private void TestTrans(GenericRequestParams requestParams) /// itzik test 
        {
            DeclarationStatusRequestParams searchParams = new DeclarationStatusRequestParams()
            {
                LoggingEnabled = true,

                CustomFileNo = _MyDeclarationPM.CustomFileNo,
                DeclarationNumber = _MyDeclarationPM.DeclarationNumber,
                Tenant = requestParams.Tenant,
                RequestName = "Declaration Status Search",
                ResponseName = "Declaration Status Search",
                SuppressSplitWR = true
            };

            searchParams.RequestVIA = SendRequestVIA.WebServiceInteractive;
            var resData = Logitude.CustomsMessaging.MessagingServices.DF_NG_8250_Web01_DeclarationStatus_RequestMessagingService.SendInteractive(searchParams);
            if (!resData.Succeeded)
            {
                LogMessagingUtil.Instance.AppendLine("Sending Declaration Status Request Failed " + resData.CustomsRequestsSheetId + ", Message: " + resData.UserMessage);
                ///return;
            }
            else
            {
                LogMessagingUtil.Instance.AppendLine("Request Succeeded " + resData.CustomsRequestsSheetId);
            }

        }

        public override void Update(DF_NG_2757_MSG10004_ExportDeclarationResponse customResponse, GenericRequestParams requestParams)
        {
            var responseName = requestParams.ResponseName;
            var context = CustomContext.GetContext(requestParams.Tenant);
            var myQueryService = new DeclarationQueryService(context);
            var myDeclarationUpdateService = new DeclarationUpdateService(context, new Dictionary<string, IContext>(), requestParams.Tenant);
            var mySupplierInvoiceItemsTaxUpdateService = new SupplierInvoiceItemsTaxUpdateService(context, new Dictionary<string, IContext>(), requestParams.Tenant);
            var mySupplierInvoiceItemVehicleModUpdateService = new SupplierInvoiceItemVehicleModUpdateService(context, new Dictionary<string, IContext>(), requestParams.Tenant); // moran 20.10.15 - Task 17209 
            var mySupplierInvoiceItemModVehicleUpdateService = new SupplierInvoiceItemModVehicleUpdateService(context, new Dictionary<string, IContext>(), requestParams.Tenant); // moran 24.11.15 - Task 17424 
            var myDeclarationTaxUpdateService = new DeclarationTaxUpdateService(context, new Dictionary<string, IContext>(), requestParams.Tenant);
            var myDeclarationConstraintUpdateService = new DeclarationConstraintUpdateService(context, new Dictionary<string, IContext>(), requestParams.Tenant);
            var myDeclarationPaymentQueryService = new DeclarationPaymentQueryService(context);

            DeclarationErrorPointerService mydDclarationErrorPointerService = new DeclarationErrorPointerService();


            this.MyResponseData = new INF_MSG_GenericResponseData();

            if (string.IsNullOrWhiteSpace(requestParams.AppicationId))
            {
                //In cases the response cannot be connected to a request using the External Id , search by DeclarationNumber
                requestParams.AppicationId = myQueryService.GetIdByDeclarationNumber(customResponse.Response.Declaration.ID.Value, requestParams.Tenant);
                if (string.IsNullOrWhiteSpace(requestParams.AppicationId))
                {
                    //if still not found search by ExternalDeclarationNumber
                    requestParams.AppicationId = myQueryService.GetIdByExternalDeclarationNumber(customResponse.Response.Declaration.DMExtensions.ExternalDeclarationID.Value, requestParams.Tenant);
                }
            }
            _MyDeclarationPM = myQueryService.GetSingle(requestParams.AppicationId, true, false);
            var setting = CustomsSettingQueryService.GetSettingByTenant(requestParams.Tenant);
            if (_MyDeclarationPM != null)
            {
              
                if (_MyDeclarationPM.Direction == "E" && requestParams.RequestVIA == SendRequestVIA.WebServiceBatch && !setting.IsConnectedToUniFreight && customResponse.ResponseContentHeader?.Exception?.Length > 0 && customResponse.Response?.Declaration == null)
                {
                    DeclarationError declarationError = new DeclarationError();
                    declarationError.Entitites = new List<Entity>();
                    foreach (var item in customResponse.ResponseContentHeader?.Exception)
                    {
                        Entity entity = new Entity();
                        entity.FieldErrors = new List<field>();
                        entity.FieldErrors.Add(new field()
                        {
                            MessageError = item.ExeptionDescription,
                            Code = "Exception",
                            ListVersionID="1"

                        });
                        declarationError.Entitites.Add(entity);
                    }


                    var myDeclaretionErrorXml = XmlGenericUtil<DeclarationError>.SerializeObject(declarationError);
                    _MyDeclarationPM.ErrosXml = myDeclaretionErrorXml;
                    //using (var stringwriter = new System.IO.StringWriter())
                    //{
                    //    var serializer = new XmlSerializer(declarationError.GetType());
                    //    serializer.Serialize(stringwriter, declarationError);
                    //    
                    //}

                    this._MyDeclarationPM.ChangeSetOp = ChangeSetOperation.Update;

                }
                if(_MyDeclarationPM.Direction == "E" && requestParams.RequestVIA != SendRequestVIA.WebServiceBatch && !setting.IsConnectedToUniFreight && customResponse.ResponseContentHeader?.Exception?.Length > 0 && customResponse.Response?.Declaration == null)
                {
                    _MyDeclarationPM.ErrosXml = "";
                    this._MyDeclarationPM.ChangeSetOp = ChangeSetOperation.Update;
                }
                
                //myDeclarationUpdateService.Update(this._MyDe'clarationPM, true);
            }
            if (customResponse.ResponseContentHeader != null && customResponse.ResponseContentHeader.Exception != null && customResponse.ResponseContentHeader.Exception.Count() > 0)
            {
                if (!string.IsNullOrWhiteSpace(requestParams.AppicationId))
                {   if(_MyDeclarationPM==null)
                         _MyDeclarationPM = myQueryService.GetSingle(requestParams.AppicationId, true, false);
                    if (_MyDeclarationPM != null)
                    {
                        //_MyDeclarationPM.IsSubmitDeclaration = false;
                        //myDeclarationUpdateService.Update(_MyDeclarationPM, true);
                    }
                }

                this.MyResponseData.UserMessage = GetExceptionMsg(customResponse.ResponseContentHeader.Exception[0]);
                this.MyResponseData.ApplicationID = requestParams.AppicationId;
                this.MyResponseData.Succeeded = true;
                this.MyResponseData.HasException = true;

                return;

            }
            if (string.IsNullOrWhiteSpace(requestParams.AppicationId))
            {
                LogMessagingUtil.Instance.AppendLine("Can not find declaration- DeclarationNumber: " + customResponse.Response.Declaration.ID.Value + " ExternalDeclarationNumber: " + customResponse.Response.Declaration.DMExtensions.ExternalDeclarationID.Value);
                this.MyResponseData.ApplicationID = requestParams.AppicationId;
                this.MyResponseData.Succeeded = true;
                this.MyResponseData.UserMessage = "Can not find declaration- DeclarationNumber: " + customResponse.Response.Declaration.ID.Value + " ExternalDeclarationNumber: " + customResponse.Response.Declaration.DMExtensions.ExternalDeclarationID.Value;
				this.MyResponseData.HasException = true;

				return;
            }


            var swGetSingle = Stopwatch.StartNew();
            myQueryService.LoadSupplierInvoicesItemsParentsOnly = true;
            this._MyDeclarationPM = myQueryService.GetSingle(requestParams.AppicationId, true, false);
            LogMessagingUtil.Instance.AppendLine("myQueryService.GetSingle:Took:" + swGetSingle.ElapsedMilliseconds);

            if (this._MyDeclarationPM == null)
            {
                LogMessagingUtil.Instance.AppendLine("Can not find declaration" + requestParams.AppicationId);
                this.MyResponseData.ApplicationID = requestParams.AppicationId;
                this.MyResponseData.Succeeded = true;
                this.MyResponseData.UserMessage = "Can not find declaration" + requestParams.AppicationId;
				this.MyResponseData.HasException = true;

				return;
            }
			if ((requestParams.InterfaceTypeCode == "2751" || requestParams.InterfaceTypeCode == "2751T"))
            {
				if (customResponse?.Response?.Declaration != null) 
                {

					PC_NG_2280_MSG01_CertificateOfOriginRequestResponseService _pc_NG_2280_MSG01_CertificateOfOriginRequestResponseService = new PC_NG_2280_MSG01_CertificateOfOriginRequestResponseService();

					CertificateOfOriginUpdateService certificateOfOriginUpdateService = new CertificateOfOriginUpdateService(context, new Dictionary<string, IContext>(), requestParams.Tenant);
					CertificateOfOriginQueryService certificateOfOriginQueryService = new CertificateOfOriginQueryService(_MyDeclarationPM.Tenant);
					var certificateOfOrigins = certificateOfOriginQueryService.GetCertificateOfOriginsByDeclarationId(_MyDeclarationPM.Id, _MyDeclarationPM.AmendmentOriginalDeclartation, _MyDeclarationPM.Tenant);
					if (certificateOfOrigins != null && certificateOfOrigins.Count() == 1)
					{
						var certificateOfOrigin = certificateOfOriginQueryService.GetSingle(certificateOfOrigins[0].Id, true, false);

						bool IsUpdated = _pc_NG_2280_MSG01_CertificateOfOriginRequestResponseService.UpdateCooNumberInDeclaration(_MyDeclarationPM.Id, certificateOfOrigin, true);
						if (!IsUpdated)
						{
							certificateOfOrigin.UpdateDeclaration = "C";
							certificateOfOrigin.ChangeSetOp = ChangeSetOperation.Update;
							certificateOfOriginUpdateService.Update(certificateOfOrigin, true);
						}
					}
				}
			}
			setting = CustomsSettingQueryService.GetSettingByTenant(_MyDeclarationPM.Tenant);

            var lastStatus =_MyDeclarationPM.DeclarationStatusTypeCode;

            if (this._MyDeclarationPM.PaymentDate.HasValue)
            {
                if (customResponse.Response != null && customResponse.Response.Status != null && (customResponse.Response.Status[0].NameCode.Value == "13" || customResponse.Response.Status[0].NameCode.Value == "14"))
                {
                    // Clear Fields
                    _MyDeclarationPM.DeclarationStatusTypeCode = customResponse.Response.Status[0].NameCode.Value;
                    _MyDeclarationPM.PaymentDate = null;
                    _MyDeclarationPM.PaymentOrderNumber = null;
                    _MyDeclarationPM.PaymentStatusCode = null;
                    _MyDeclarationPM.CourierCustomStatusCode = null;
                    _MyDeclarationPM.CourierSuspentionCode = null;
                    _MyDeclarationPM.CourierSuspentionReasonCode = null;

                    // Delete Payment
                    var mydeclarationPaymentQueryService = new DeclarationPaymentQueryService(context);
                    var declarationPaymentPM = mydeclarationPaymentQueryService.GetSingle(_MyDeclarationPM.Id, true, false);
                    if (declarationPaymentPM != null)
                    {
                        declarationPaymentPM.ChangeSetOp = ChangeSetOperation.Delete;
                        if (declarationPaymentPM.DeclarationPaymentMethods.Any())
                        {
                            foreach (var item in declarationPaymentPM.DeclarationPaymentMethods)
                            {
                                item.ChangeSetOp = ChangeSetOperation.Delete;
                            }
                        }
                        DeclarationPaymentUpdateService declarationPaymentUpdateService = new DeclarationPaymentUpdateService(context, new Dictionary<string, IContext>(), _MyDeclarationPM.Tenant);
                        declarationPaymentUpdateService.Update(declarationPaymentPM, true);
                    }

                    // Delete Status
                    DeclarationUpdateService.DelDeclarationStatus(_MyDeclarationPM, "", "RSH");
                }
            }

            if (requestParams.GetType() == typeof(DeclarationRestoreRequestParams))// moran 10.1.16 Task 19724 // Mirit 24/01/16 19845 
            {

                if (this._MyDeclarationPM.Direction != "E")
                {
                    if (this._MyDeclarationPM.PaymentDate.HasValue && !_MyDeclarationPM.IsCourierDeclaration ) //If declaration was already paid 
                    {
                        //Task 44715 allow update of 1.0 if current <1.0 and it's a restore response
                        if (!(requestParams.GetType() == typeof(DeclarationRestoreRequestParams) && System.Convert.ToDouble(_MyDeclarationPM.VersionId) < 1.0 && System.Convert.ToDouble(customResponse.Response.Declaration.DMExtensions.VersionID.Value) == 1.0) //restored version 1.0 and current 0.x
                            && (_MyDeclarationPM.VersionId != customResponse.Response.Declaration.DMExtensions.VersionID.Value)) //Compare Declaration Version

                        {
                            string mess = "נתוני ההצהרה לא עודכנו " + " (" + _MyDeclarationPM.DeclarationNumber + ")" + " הצהרה כבר שולמה ויש שוני בין הגרסאות";
                            LogMessagingUtil.Instance.AppendLine(mess);
                            this.MyResponseData.ApplicationID = requestParams.AppicationId;
                            this.MyResponseData.Succeeded = true;
                            this.MyResponseData.UserMessage = mess;
                            this.MyResponseData.HasException = true;
                            return;
                        }
                        else
                        {
                            _MyDeclarationPM.PaymentDate = null;
                            _MyDeclarationPM.PaymentOrderNumber = "";
                            _MyDeclarationPM.PaymentStatusCode = "";
                        }
                    }
                }

                else
                {
                    if(customResponse?.Response?.Status[0]?.NameCode?.Value == "36" && _MyDeclarationPM?.DeclarationStatusTypeCode != "36")
                    {
                        SendDeclarationPrint(requestParams);
                    }

                    // if the declaration has been canceled, pass the status to unifreight
                    if (customResponse?.Response?.Status[0]?.NameCode?.Value == "1" && _MyDeclarationPM?.DeclarationStatusTypeCode != "1")
                    {
                        var amitalEventTracerModel = new Logitude.Customs.BL.TraceEvents.AmitalEventTracerModel()
                        {
                            Tenant = _MyDeclarationPM.Tenant,
                            objectTableName = "Customs.Declaration",
                            EventCode = "CAN",
                            notes = "הערות המכס לביטול: " + _MyDeclarationPM.CustomCancelRequestRemarks,
                            CommunicationLoggingEntityReference = _MyDeclarationPM.DeclarationNumber,
                            EntityId = _MyDeclarationPM.Id,
                            UserId = requestParams.LoggingUserId,
                            CommunicationSubject = "FU Status CAN from logitude ",
                            MyFUStatus = new AmitalEventTracerModel.FUStatus()
                            {
                                entname = "BFIFILE",
                                primary_number = _MyDeclarationPM.CustomFileNo,
                                status = "new",
                                xml_status = "new",
                                status_id = "CAN",
                                status_DateTime = DateTime.Now,
                                comments = ""
                            }
                        };
                        AmitalEventTracer.CreateTraceEvent(amitalEventTracerModel);
                    }
                }
            }

            //לא רלוונטי ליצוא
            /*if (this._MyDeclarationPM.IsConvertedDeclaration) // Mirit 24/01/16 19918
            {
                LogMessagingUtil.Instance.AppendLine("נתוני ההצהרה לא עודכנו מכיוון שמדובר בהצהרה מוסבת");
                this.MyResponseData.ApplicationID = requestParams.AppicationId;
                this.MyResponseData.Succeeded = true;
                this.MyResponseData.UserMessage = "נתוני ההצהרה לא עודכנו מכיוון שמדובר בהצהרה מוסבת";
                this.MyResponseData.HasException = false;

                return;
            }*/





            //<--- Yuval Chalup 28.05.2015 TASK-13252
            if (!string.IsNullOrWhiteSpace(this._MyDeclarationPM.Id))
            {
                if (this.MyRequestSheetParam == null)
                {
                    this.MyRequestSheetParam = new RequestSheetParam();
                }
                this.MyRequestSheetParam.CustomFileNo = this._MyDeclarationPM.CustomFileNo;
                this.MyRequestSheetParam.ObjectTableId1 = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
                this.MyRequestSheetParam.EntityId1 = this._MyDeclarationPM.Id;
                this.MyRequestSheetParam.RequestDescription = "משוב להצהרה  " + this._MyDeclarationPM.DeclarationNumber;
                if (customResponse.Response != null)
                {
                    if (customResponse.Response.Declaration != null)
                    {
                        if (customResponse.Response.Declaration.DMExtensions != null)
                        {
                            if (customResponse.Response.Declaration.DMExtensions.VersionID != null)
                            {
                                this.MyRequestSheetParam.RequestDescription = "משוב להצהרה  " + customResponse.Response.Declaration.ID.Value + " " + customResponse.Response.Declaration.DMExtensions.VersionID.Value;
                            }
                        }
                    }
                }
            }
            //move to OnUpdating


            //Yuval Chalup 28.05.2015 TASK-13252 --->

            float oldVersionId;
            float.TryParse(_MyDeclarationPM.VersionId, out oldVersionId);
            if (string.IsNullOrWhiteSpace(_MyDeclarationPM.VersionId))
            {
                oldVersionId = 0.1F;
            }
            var declarationPaymentsPM = myDeclarationPaymentQueryService.GetSingle(requestParams.AppicationId, true, false);

            if (requestParams.InterfaceTypeCode == "2755")
            {
                this._IsSubmitDeclarationResponse = true;
            }


           

            ICommonDataContext commondbContext = CommonDataContext.GetContext(_MyDeclarationPM.Tenant);
            UserRepository userRepository = new UserRepository(commondbContext);
            var user = userRepository.GetSingleUserByCode("MEHES", _MyDeclarationPM.Tenant, true);

            if (setting.IsConnectedToUniFreight || AmitalEventTracer.UseHybrid_When_NotIsConnectedToUniFreight)
            {

                if (this._MyDeclarationPM.Direction == "E" )
                {
                    if (customResponse.Response.Declaration.DMExtensions.VersionID.Value == "1.0")
                    {
                        _DateTime = new DateTime();
                        _DateTime = DateTime.Parse(customResponse.Response.Declaration.IssueDateTime);
                        RaiseEvent(this._MyDeclarationPM, user?.Id, status_id: "MRN", versionId: customResponse.Response.Declaration.DMExtensions.ExternalDeclarationID.Value, status_DateTime: _DateTime);
                    }


                    if (_MyDeclarationPM.DeclarationStatusTypeCode != customResponse.Response.Status[0].NameCode.Value)
                    {
                        _DateTime = new DateTime();
                        _DateTime = DateTime.Parse(customResponse.Response.Status[0].EffectiveDateTime);
                        switch (customResponse.Response.Status[0].NameCode.Value)
                        {
                            case "36":
                                {
                                    RaiseEvent(this._MyDeclarationPM, user?.Id, status_id: "CLS", versionId: customResponse.Response.Declaration.DMExtensions.ExternalDeclarationID.Value, status_DateTime: _DateTime);
                                }
                                break;
                            case "12":
                                {
                                    RaiseEvent(this._MyDeclarationPM, user?.Id, status_id: "FAI", versionId: customResponse.Response.Declaration.DMExtensions.ExternalDeclarationID.Value, status_DateTime: _DateTime);
                                }
                                break;
                            case "45":
                                {
                                    RaiseEvent(this._MyDeclarationPM, user?.Id, status_id: "H45", versionId: customResponse.Response.Declaration.DMExtensions.ExternalDeclarationID.Value, status_DateTime: _DateTime);
                                }
                                break;
                            case "5":
                                {
                                     RaiseEvent(this._MyDeclarationPM, user?.Id, status_id: "H05", versionId: customResponse.Response.Declaration.DMExtensions.ExternalDeclarationID.Value, status_DateTime: _DateTime);
                                }
                                break;
                            case "6":
                                {
                                    RaiseEvent(this._MyDeclarationPM, user?.Id, status_id: "RDH", versionId: customResponse.Response.Declaration.DMExtensions.ExternalDeclarationID.Value, status_DateTime: _DateTime);
                                    RaiseEvent(this._MyDeclarationPM, user?.Id, status_id: "H06", versionId: customResponse.Response.Declaration.DMExtensions.ExternalDeclarationID.Value, status_DateTime: _DateTime);
                                }
                                break;
                            case "3":
                                {
                                    RaiseEvent(this._MyDeclarationPM, user?.Id, status_id: "RDH", versionId: customResponse.Response.Declaration.DMExtensions.ExternalDeclarationID.Value, status_DateTime: _DateTime);
                                 }
                                break;
                            default:
                                break;
                        }
                        List<string> statusList = new List<string>()
                        {
                            "2","4","22","23","26","35","40","41"
                        };
                        if (statusList.Contains(customResponse.Response.Status[0].NameCode.Value))
                        {
                            RaiseEvent(this._MyDeclarationPM, user?.Id, status_id: "WAT", versionId: customResponse.Response.Declaration.DMExtensions.ExternalDeclarationID.Value, status_DateTime: _DateTime);
                        }

                        if (this._MyDeclarationPM.IsDiamondDeclaration && this._MyDeclarationPM.AutoSending)
                        {
                            // determine if the export diamonds feature is enabled to allow autosending
                            ICommonDataContext myContextCommon = CommonDataContext.GetContext(this._MyDeclarationPM.Tenant);
                            FeatureRepository myFeatureRepository = new FeatureRepository(myContextCommon);
                            FeatureQuery featureQuery = new FeatureQuery(myFeatureRepository);
                            var features = featureQuery.GetAllowedFeaturesForLoggedUser(AuthenticationUtil.ResolveUserId(this._MyDeclarationPM.Tenant), this._MyDeclarationPM.Tenant);
                            var featureExportDiamonds = features.Features.FirstOrDefault(x => x.Code == "ExportDiamonds");

                            if (featureExportDiamonds != null)
                            {
                                // get the declaration status label
                                DeclarationStatusTypeQueryService declarationStatusTypeQueryService = new DeclarationStatusTypeQueryService(_MyDeclarationPM.Tenant);
                                DeclarationStatusTypePM declarationStatusType = declarationStatusTypeQueryService.GetSingle(customResponse.Response.Status[0].NameCode.Value, false, true);
                                string declarationStatusLabel = declarationStatusType?.LocalName ?? "";
                                string declarationNumber = this._MyDeclarationPM.DeclarationNumber ?? "";

                                RaiseEvent(this._MyDeclarationPM, user?.Id, 
                                    status_id: "SOY", 
                                    versionId: customResponse.Response.Declaration.DMExtensions.ExternalDeclarationID.Value, 
                                    status_DateTime: _DateTime, 
                                    comments: $"CODE-{customResponse.Response.Status[0].NameCode.Value}-{declarationStatusLabel}-{declarationNumber}");
                            }
                        }
                    }
                }
            }
 
            if (_MyDeclarationPM.DepositionStatusCode == "R") _MyDeclarationPM.DepositionStatusCode = null;
            if (requestParams.ResponseName != "5117" && requestParams.ResponseName != "8237" && customResponse.ResponseContentHeader.Exception != null)
            {
                string userMessage = "";
                this._MyDeclarationPM.MarkAsChanged = false;
                var swErrosXml = Stopwatch.StartNew();

                foreach (UnifreightIIG.Common.ExportDeclarationServiceReference.Exception exception in customResponse.ResponseContentHeader.Exception)
                {
                    if (!string.IsNullOrWhiteSpace(userMessage))
                    {
                        userMessage = userMessage + @"
";
                    }
                    userMessage = userMessage + exception.ExeptionDescription;
                    this._MyDeclarationPM.ErrosXml = mydDclarationErrorPointerService.AddDeclarationExceptionExport(this._MyDeclarationPM.ErrosXml, "Buisness", exception, true);
                    LogMessagingUtil.Instance.AppendLine("ErrosXml:Took:" + swErrosXml.ElapsedMilliseconds);
                    switch (exception.ExeptionType)
                    {
                        case 2794:
                            {
                                this._MyDeclarationPM.DeclarationNumber = exception.ExceptionParms.FirstOrDefault();
                                if (!string.IsNullOrWhiteSpace(userMessage))
                                {
                                    userMessage = userMessage + @"
";
                                }
                                userMessage = userMessage + "עודכן מספר ההצהרה לפי רשומת הסוכן - יש לשדר את ההצהרה מחדש";
                                break;
                            }
                        case 1501:
                            {
                                UpdateUnifreightEvent("MPOA", requestParams.LoggingUserId);
                                userMessage = userMessage + "חסר יפוי כח";
                                break;
                            }
                        case 4589:
                            {
                                UpdateUnifreightEvent("MID", requestParams.LoggingUserId);
                                userMessage = userMessage + "חסר תצהיר יבואן";
                                if (string.IsNullOrWhiteSpace(_MyDeclarationPM.DepositionStatusCode)) _MyDeclarationPM.DepositionStatusCode = "R";
                                break;
                            }
                        case 2244:
                            {
                                UpdateUnifreightEvent("IDE", requestParams.LoggingUserId);
                                userMessage = userMessage + "תצהיר יבואן עומד לפוג";
                                break;
                            }
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

                if (this._MyDeclarationPM.IsCourierDeclaration)// due (customResponse.ResponseContentHeader.Exception != null)>> X
                {
 
                    var calculateDeclarationCourierStatus = new CalculateDeclarationCourierStatus(this._MyDeclarationPM);
                    calculateDeclarationCourierStatus.Update(
                        (currentDeclarationCourierStatusPM) =>
                        {

                            currentDeclarationCourierStatusPM.CourierDeclarationStatusCode = "X";
                        });
                }
               

                this.MyResponseData.ApplicationID = requestParams.AppicationId;
                this.MyResponseData.Succeeded = true;
                this.MyResponseData.UserMessage = userMessage;
                this.MyResponseData.HasException = true;

                return;
            }









            if (customResponse.Response == null)
            {
                if (customResponse.ResponseContentHeader.Exception == null)
                {
                    string text = null;
                    if (String.IsNullOrWhiteSpace(customResponse.ResponseContentHeader.Remark))
                    {
                        text = "No Declaration details in the Response " + requestParams.AppicationId;
                        this.MyResponseData.UserMessage = text;
                        LogMessagingUtil.Instance.AppendLine(text);
                    }
                    else
                    {
                        text = "No Declaration details in the Response " + customResponse.ResponseContentHeader.Remark + requestParams.AppicationId;
                        this.MyResponseData.UserMessage = text;
                        LogMessagingUtil.Instance.AppendLine(text);
                    }
                    this.MyResponseData.ApplicationID = requestParams.AppicationId; //Yuval Chalup 28.05.2015 TASK-13252+13509
                    return;
                }
            }

            LogMessagingUtil.Instance.AppendLine("Analyze declaration response" + requestParams.AppicationId);
            _FastDelete = true;
            var sw = Stopwatch.StartNew();
            if (_FastDelete)
            {

                var myDeclarationKeys = new DeclarationKeys { Id = _MyDeclarationPM.Id };
                myDeclarationTaxUpdateService.FastDeleteComposition(myDeclarationKeys);
                mySupplierInvoiceItemVehicleModUpdateService.FastDeleteComposition(myDeclarationKeys);
                mySupplierInvoiceItemsTaxUpdateService.FastDeleteComposition(myDeclarationKeys);
                mySupplierInvoiceItemModVehicleUpdateService.FastDeleteComposition(myDeclarationKeys);
                 (context as DbContextBase).SaveChanges();
                context = CustomContext.GetContext(requestParams.Tenant);
                myDeclarationUpdateService = new DeclarationUpdateService(context, new Dictionary<string, IContext>(), requestParams.Tenant);

            }
            else
            {
                DeleteDeclarationTaxes(myDeclarationTaxUpdateService);
                DeleteSupplierInvoiceItemsTaxes(mySupplierInvoiceItemsTaxUpdateService);
                DeleteSupplierInvoiceItemsVehicleMods(mySupplierInvoiceItemVehicleModUpdateService); // moran 20.10.15 - Task 17209
                DeleteSupplierInvoiceItemsModVehicles(mySupplierInvoiceItemModVehicleUpdateService); // moran 24.11.15 - Task 17424 
                                                                                                     //DeleteSupplierInvioceItemCertificates(mySupplierInvioceItemCertificatUpdateService);
            }
            LogMessagingUtil.Instance.AppendLine("IsFastDelete:" + _FastDelete.ToString() + ",Took :" + sw.ElapsedMilliseconds);

            if (requestParams.ResponseName != "5117" && requestParams.ResponseName != "8237")
            {
                if (String.IsNullOrWhiteSpace(_MyDeclarationPM.DeclarationNumber))
                {
                    _MyDeclarationPM.DeclarationNumber = customResponse.Response.Declaration.ID.Value;
                }
                else
                {
                    var customDeclarationNumber = customResponse.Response.Declaration.ID.Value;
                    if (customDeclarationNumber != _MyDeclarationPM.DeclarationNumber)
                    {
                         _MyDeclarationPM.DeclarationNumber = customDeclarationNumber; //yaron !!
                    }
                }
            }
            //Update Declaration 
            _MyDeclarationPM.VersionId = customResponse.Response.Declaration.DMExtensions.VersionID.Value;

            float version;
            float.TryParse(_MyDeclarationPM.VersionId, out version);
            if (version >= 1.0)
            {
                
                _MyDeclarationPM.IsSubmitDeclaration = true;
                if (_MyDeclarationPM.Direction == "E")
                {

                    var myAmitalEventTracerModel = new Logitude.Customs.BL.TraceEvents.AmitalEventTracerModel()
                    {

                        Tenant = _MyDeclarationPM.Tenant,
                        objectTableName = "Customs.Declaration",
                        EventCode = null,
                        notes = "",
                        CommunicationLoggingEntityReference = _MyDeclarationPM.DeclarationNumber,
                        EntityId = _MyDeclarationPM.Id,
                        UserId = _MyDeclarationPM.CreatedByUserId,

                        CommunicationSubject = "עדכון תיק מכס",

                    };
                    Logitude.AmitalMessaging.Infrastructure.FuStatus.LOGICUSTFILE logistictFile = AmitalInsertToQueueEzer.setLogistictFile(_MyDeclarationPM);
                    var amitalInsertToQueueService = new AmitalInsertToQueueService<Logitude.AmitalMessaging.Infrastructure.FuStatus.LOGICUSTFILE>(logistictFile);
                    amitalInsertToQueueService.InsertToQueue(myAmitalEventTracerModel, "UpdateExportCustomsFile");

                    if (!string.IsNullOrEmpty(customResponse.Response.Declaration.DMExtensions.ReferenceDateTime))
                        _MyDeclarationPM.TaxationDateTime = Convert.ToDateTime(customResponse.Response.Declaration.DMExtensions.ReferenceDateTime);
                }
                    
            }

            if (customResponse.Response.Declaration.DMExtensions.TransshipmentApprovalDateTime != null)
            {
                _MyDeclarationPM.TransshipmentApprovalDateTime = customResponse.Response.Declaration.DMExtensions.TransshipmentApprovalDateTime.Value;

            }
            _MyDeclarationPM.DeclarationStatusTypeCode = customResponse.Response.Status[0].NameCode.Value;

            if (requestParams.ResponseName == "8237" && customResponse.Response.Status[0].NameCode.Value == "36")
            {

                if (_MyDeclarationPM.Direction == "E")
                {
                    var myAmitalEventTracerModel = new Logitude.Customs.BL.TraceEvents.AmitalEventTracerModel()
                    {

                        Tenant = _MyDeclarationPM.Tenant,
                        objectTableName = "Customs.Declaration",
                        EventCode = null,
                        notes = "",
                        CommunicationLoggingEntityReference = _MyDeclarationPM.DeclarationNumber,
                        EntityId = _MyDeclarationPM.Id,
                        UserId = _MyDeclarationPM.CreatedByUserId,

                        CommunicationSubject = "עדכון תיק מכס",

                    };
                    Logitude.AmitalMessaging.Infrastructure.FuStatus.LOGICUSTFILE logistictFile = AmitalInsertToQueueEzer.setLogistictFile(_MyDeclarationPM);
                    var amitalInsertToQueueService = new AmitalInsertToQueueService<Logitude.AmitalMessaging.Infrastructure.FuStatus.LOGICUSTFILE>(logistictFile);
                    amitalInsertToQueueService.InsertToQueue(myAmitalEventTracerModel, "UpdateExportCustomsFile");

                }
                _MyDeclarationPM.IsExportClosed = true;
                _MyDeclarationPM.IsClose = true;
            }

            if ((requestParams.ResponseName == "9079" || requestParams.InterfaceTypeCode == "9079") && customResponse.Response.Status[0].NameCode.Value == "36")
            {
               _MyDeclarationPM.IsExportClosed = true;
                _MyDeclarationPM.IsClose = true;
            }

            if (customResponse.Response.Declaration.DMExtensions.ExpenseLoadingFactorDetails != null)
                _MyDeclarationPM.LoadingFactor = customResponse.Response.Declaration.DMExtensions.ExpenseLoadingFactorDetails.FirstOrDefault()?.ExpenseLoadingFactor.Value;

            if (customResponse.Response.Declaration.DMExtensions.CustomsValueComponent.TotalFOBNISAmount != null)
                _MyDeclarationPM.FOBValueNIS = Math.Round(customResponse.Response.Declaration.DMExtensions.CustomsValueComponent.TotalFOBNISAmount.Value, 2);
            if (customResponse.Response.Declaration.DMExtensions.CustomsValueComponent.TotalFOBUSDAmount != null)

                _MyDeclarationPM.FOBValueDollar = Math.Round(customResponse.Response.Declaration.DMExtensions.CustomsValueComponent.TotalFOBUSDAmount.Value, 2);
            var prev_TotalTax = _MyDeclarationPM.TotalTax;
            bool isSendVPE = false;
            _MyDeclarationPM.TotalTax = Math.Round(customResponse.Response.Declaration.DMExtensions.CustomsValueComponent.TaxAssessedAmount.Value, 2);
            if (requestParams.GetType() != typeof(DeclarationRestoreRequestParams))//Task 44715
            {
                if (_IsSubmitDeclarationResponse != true) _MyDeclarationPM.IsChanged = false;
            }

            decimal DealValueWithoutFactor = 0;
            if (customResponse.Response.Declaration.GoodsShipment != null)
            {
                foreach (var goodsShipment in customResponse.Response.Declaration.GoodsShipment)
                {
                    if (goodsShipment.GovernmentAgencyGoodsItem != null)
                    {
                        foreach (var governmentAgencyGoodsItem in goodsShipment.GovernmentAgencyGoodsItem)
                        {
                            if (governmentAgencyGoodsItem.DMExtensions != null)
                            {
                                if (governmentAgencyGoodsItem.DMExtensions.GoodsItemAmount != null)
                                {
                                    foreach (var goodsItemAmount in governmentAgencyGoodsItem.DMExtensions.GoodsItemAmount)
                                    {
                                        if (goodsItemAmount.AmountType.Value == "15" && goodsItemAmount.CustomsValueAmount.currencyIDSpecified && goodsItemAmount.CustomsValueAmount.currencyID.ToString() == "ILS")
                                        {
                                            DealValueWithoutFactor += goodsItemAmount.CustomsValueAmount.Value;
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (DealValueWithoutFactor != 0) _MyDeclarationPM.DealValueWithoutFactor = Math.Round(DealValueWithoutFactor, 2);

            if (requestParams.InterfaceTypeCode == "2751" || requestParams.InterfaceTypeCode == "2751T")
            {
                if (!string.IsNullOrWhiteSpace(RequestSheetContext.Current.GetContextOrDefault().SignByX509SubjectName))
                {
                    _MyDeclarationPM.IsSignedVersion = true;
                    _MyDeclarationPM.SignedByUserId = requestParams.LoggingUserId;
                    _MyDeclarationPM.SignerPersonalId = SignCertificateClass.GetPersonID(RequestSheetContext.Current.GetContextOrDefault().SignByX509SubjectName);
                }
                else
                {
                    _MyDeclarationPM.IsSignedVersion = false;
                    _MyDeclarationPM.SignedByUserId = null;
                    _MyDeclarationPM.SignerPersonalId = null;
                }
            }

            //Analyze the Errors section in the response XML 
            var swErrosXml1 = Stopwatch.StartNew();
            this._MyDeclarationPM.ErrosXml = mydDclarationErrorPointerService.AnalyzeErrorPionterExport(customResponse.Response.Error, _MyDeclarationPM, WCOTypeEnum.WCO_EX, !_IsSubmitDeclarationResponse);
            this._MyDeclarationError = mydDclarationErrorPointerService._declarationErrorPointer;
            LogMessagingUtil.Instance.AppendLine("ErrosXml:Took:" + swErrosXml1.ElapsedMilliseconds);


            var swTax = Stopwatch.StartNew();
            //Update Declaration Taxes
            _MyDeclarationPM.DeclarationTaxes = GetDeclarationTaxesPM(customResponse);

            bool tester = false;
            if (tester)
            {
                int count = 0;
                foreach (var si in _MyDeclarationPM.SupplierInvoices)
                {
                    foreach (var sii in si.SupplierInvoiceItems)
                    {
                        foreach (var siic in sii.SupplierInvioceItemCertificats)
                        {
                            count++;
                        }
                    }
                }
            }

            //Update Declaration Item Taxes
            LogMessagingUtil.Instance.LogActionTime(() =>
            {
                _MyDeclarationPM.SupplierInvoices = GetSupplierInvoicesPM(customResponse);
            }, "GetSupplierInvoicesPM");
            LogMessagingUtil.Instance.AppendLine("GetDeclarationTaxes+Item Tax:Took:" + swTax.ElapsedMilliseconds);

            if (tester)
            {
                int count = 0;
                foreach (var si in _MyDeclarationPM.SupplierInvoices)
                {
                    foreach (var sii in si.SupplierInvoiceItems)
                    {
                        foreach (var siic in sii.SupplierInvioceItemCertificats)
                        {
                            count++;
                        }
                    }
                }
            }

            //Build constraints
            if (this._IsSubmitDeclarationResponse != true)
            {
                DeleteDeclarationConstraints(myDeclarationConstraintUpdateService);
            }
            _MyDeclarationPM.DeclarationConstraints = BuildDeclarationConstraints(customResponse.Response.Error);

            //<--- Yuval Chalup 06.08.2015 TASK-15422
            _MyDeclarationPM.PaymentOrderNumber = null;
            _MyDeclarationPM.PaymentStatusCode = null;
            string paymentOrderNumber = null; //Yuval Chalup 10.08.2015 TASK-15472
            string paymentStatusCode = null; //Yuval Chalup 10.08.2015 TASK-15472
            if (customResponse.DeclarationPaymentDetails != null)
            {
                if (customResponse.DeclarationPaymentDetails.PaymentOrderNumber != null)
                {
                    _MyDeclarationPM.PaymentOrderNumber = customResponse.DeclarationPaymentDetails.PaymentOrderNumber.ToString();
                    paymentOrderNumber = customResponse.DeclarationPaymentDetails.PaymentOrderNumber.ToString(); //Yuval Chalup 10.08.2015 TASK-15472
                }
                if (customResponse.DeclarationPaymentDetails.PaymentOrderStatus != null)
                {
                    _MyDeclarationPM.PaymentStatusCode = customResponse.DeclarationPaymentDetails.PaymentOrderStatus.ToString();
                    paymentStatusCode = customResponse.DeclarationPaymentDetails.PaymentOrderStatus.ToString(); //Yuval Chalup 10.08.2015 TASK-15472

                }
            }
             if (!(!string.IsNullOrWhiteSpace(paymentOrderNumber) && paymentStatusCode != "5") && !(_IsSubmitDeclarationResponse == true && string.IsNullOrWhiteSpace(paymentOrderNumber) && _MyDeclarationPM.DeclarationStatusTypeCode == "5" && _MyDeclarationPM.TotalTax <= 5))
            {
                _MyDeclarationPM.CurrentContextTag = Logitude.Customs.BL.EntityUpdateServices.DeclarationUpdateService.UpdateUnifreightBillingConst;
                if (_MyDeclarationPM.IsCourierDeclaration && declarationPaymentsPM != null && declarationPaymentsPM.PaymentDate.HasValue && _IsSubmitDeclarationResponse == true && _MyDeclarationPM.DeclarationStatusTypeCode == "5")
                {
                    _MyDeclarationPM.PaymentDate = declarationPaymentsPM.PaymentDate;
                }

            }
            else
            {
                //Payment date update
                if (declarationPaymentsPM != null && declarationPaymentsPM.PaymentDate.HasValue)
                {
                    _MyDeclarationPM.PaymentDate = declarationPaymentsPM.PaymentDate;
                }
                 if (_MyDeclarationPM.CurrentContextTag != Logitude.Customs.BL.EntityUpdateServices.DeclarationUpdateService.UpdateUnifreightBillingConst)
                {
                    _MyDeclarationPM.CurrentContextTag = Logitude.Customs.BL.EntityUpdateServices.DeclarationUpdateService.CreateUnifreightPaymentConst;
                }
            }
            if (customResponse != null)
            {
                if (customResponse.Response != null)
                {
                    if (customResponse.Response.Status != null)
                    {
                        if (customResponse.Response.Status[0].NameCode != null)
                        {
                            if (customResponse.Response.Status[0].NameCode.Value == "14")
                            {
                                _MyDeclarationPM.PaymentDate = null;
                                LogMessagingUtil.Instance.AppendLine("Change Declaration Version From " + oldVersionId + "To " + _MyDeclarationPM.VersionId);
                                _MyDeclarationPM.CurrentContextTag = Logitude.Customs.BL.EntityUpdateServices.DeclarationUpdateService.UpdateUnifreightBillingConst;
                            }
                        }
                    }
                }
            }

            if (_TotalBtlCoverageNISSum > 0)
            {
                mydDclarationErrorPointerService = new DeclarationErrorPointerService();
                this._MyDeclarationPM.ErrosXml = mydDclarationErrorPointerService.AddErrorPionter(_MyDeclarationError, "", "", "", "", "", "", "", "A", @"לתיק זה קיימת הלוואת ביטוח לאומי ע""ס " + _TotalBtlCoverageNISSum.ToString() + @" ש""ח", "", "", "", "");
            }

            if (customResponse.CollateralRequestDetails != null)
            {
                if (customResponse.CollateralRequestDetails.Count() > 0)
                {
                    foreach (UnifreightIIG.Common.ExportDeclarationServiceReference.CollateralRequestDetails collateralRequestItem in customResponse.CollateralRequestDetails)
                    {
                        this._MyDeclarationPM.ErrosXml = mydDclarationErrorPointerService.AddErrorPionter(_MyDeclarationError, "", "", "", "", "", "", "", "A", "המשוב להצהרה כולל דרישה לבטוחה " + " - מספר בטוחה " + collateralRequestItem.collateralRequestNumber, "", "", "", "");
                     }
                }
            }

            UpdateDepositionStatusCode();

            if (!String.IsNullOrWhiteSpace("itzik and yaron move to herer from DeclarationWebService.asmx"))
            {
                if (requestParams.GetType() != typeof(DeclarationRestoreRequestParams))//Task 44715 (add condition to itzik and yaron...
                {
                    _MyDeclarationPM.MarkAsChanged = false;
                    _MyDeclarationPM.IsChanged = false;
                }
            }
            _MyDeclarationPM.CustomsRequestsSheetId = requestParams.CustomsRequestsSheetId;
            _MyDeclarationPM.ChangeSetOp = ChangeSetOperation.Update;
            myDeclarationUpdateService.IsFromCustomsFeedback = true;
            myDeclarationUpdateService.Update(_MyDeclarationPM, true);

            if (_MyDeclarationPM.IsCourierDeclaration)
            {
                DeclarationCourierStatusQueryService declarationCourierStatusQueryService = new DeclarationCourierStatusQueryService(context);
                DeclarationCourierStatusPM _MyDeclarationCourierStatusPM = new DeclarationCourierStatusPM();
                _MyDeclarationCourierStatusPM = declarationCourierStatusQueryService.GetSingle(_MyDeclarationPM.Id, true, false);
                if (_MyDeclarationCourierStatusPM != null)
                {
                    DeclarationPendingPM declarationPendingPM_900 = null;
                    DeclarationPendingPM declarationPendingPM_901 = null;
                    if (_MyDeclarationCourierStatusPM.DeclarationPendings != null && _MyDeclarationCourierStatusPM.DeclarationPendings.Count() > 0)
                    {
                        declarationPendingPM_900 = _MyDeclarationCourierStatusPM.DeclarationPendings.Where(r => r.DeclarationID == _MyDeclarationCourierStatusPM.DeclarationId && r.CourierPendingReasonCode == "900").FirstOrDefault();
                        declarationPendingPM_901 = _MyDeclarationCourierStatusPM.DeclarationPendings.Where(r => r.DeclarationID == _MyDeclarationCourierStatusPM.DeclarationId && r.CourierPendingReasonCode == "901").FirstOrDefault();
                    }
                    // Pending 901
                    Boolean isSetPendingTo901 = false;
                    if (customResponse.Response.Error != null)
                    {
                        foreach (var errorItem in customResponse.Response.Error)
                        {
                            if (errorItem.ValidationCode != null && errorItem.ValidationCode.Value == "2382")
                            {
                                CourierPendingReasonQueryService myCourierPendingReasonQueryService = new CourierPendingReasonQueryService(context);
                                CourierPendingReasonPM courierPendingReasonPM = myCourierPendingReasonQueryService.GetSingleCourierPendingReasonByCode("901", _MyDeclarationPM.Tenant);
                                if (courierPendingReasonPM == null)
                                {
                                    LogMessagingUtil.Instance.AppendLine("לא קיים קוד Pending - הצהרה פלסטינאית = 901 בטבלת סיבות Pending");
                                    break;
                                }
                                LogMessagingUtil.Instance.AppendLine("Pending - הצהרה פלסטינאית = 901");
                                isSetPendingTo901 = true;
                                if (declarationPendingPM_901 == null)
                                {
                                    CourierPendingReasonRepository courierPendingReasonRepositoryRepository = new CourierPendingReasonRepository(_MyDeclarationCourierStatusPM.Tenant);
                                    Boolean isActive = courierPendingReasonRepositoryRepository.IsActive("901", _MyDeclarationCourierStatusPM.Tenant);
                                    if (isActive)
                                    {
                                        declarationPendingPM_901 = new DeclarationPendingPM();
                                        declarationPendingPM_901.CourierPendingReasonCode = "901";
                                        declarationPendingPM_901.Status = "A";
                                        declarationPendingPM_901.ChangeSetOp = ChangeSetOperation.Insert;
                                        _MyDeclarationCourierStatusPM.DeclarationPendings.Add(declarationPendingPM_901);
                                    }
                                }
                                else if (declarationPendingPM_901.Status != "A")
                                {
                                    declarationPendingPM_901.ChangeSetOp = ChangeSetOperation.Update;
                                    declarationPendingPM_901.Status = "A";
                                }
                                if (declarationPendingPM_901.ChangeSetOp != ChangeSetOperation.None)
                                {
                                    LogMessagingUtil.Instance.AppendLine("Set Courier Pending Reason Code To 901");
                                    if (_MyDeclarationCourierStatusPM.ChangeSetOp != ChangeSetOperation.Update) _MyDeclarationCourierStatusPM.ChangeSetOp = ChangeSetOperation.Update;
                                }
                            }
                        }
                    }
                    if (!isSetPendingTo901)
                    {
                        if (declarationPendingPM_901 != null)
                        {
                            declarationPendingPM_901.ChangeSetOp = ChangeSetOperation.Update;
                            declarationPendingPM_901.Status = "S";
                            if (_MyDeclarationCourierStatusPM.ChangeSetOp != ChangeSetOperation.Update) _MyDeclarationCourierStatusPM.ChangeSetOp = ChangeSetOperation.Update;
                            LogMessagingUtil.Instance.AppendLine("Courier Pending Reason Code 901 Set as Solved");
                        }
                    }
                    // Pending 900
                    CourierMasterQueryService courierMasterService = new CourierMasterQueryService(requestParams.Tenant);
                    CourierMasterPM courierMaster = courierMasterService.GetSingle(_MyDeclarationPM.CourierMasterId, false, false);
                    if (courierMaster != null)
                    {
                        var myGDFDATAQueryService = new GDFDATAQueryService(AmitalContext.GetContext(requestParams.Tenant));
                        var def = myGDFDATAQueryService.GetSingle("ISRAEL", "CGO_ACT_COLLECT", "NON", courierMaster.IntegratorNumber, false, true);
                        bool isCollectActive = def.DEFDATA == "Y";
                        if (declarationPendingPM_900 == null)
                        {
                            CourierPendingReasonQueryService myCourierPendingReasonQueryService = new CourierPendingReasonQueryService(context);
                            CourierPendingReasonPM courierPendingReasonPM = myCourierPendingReasonQueryService.GetSingleCourierPendingReasonByCode("900", _MyDeclarationPM.Tenant);
                            if (courierPendingReasonPM == null)
                            {
                                LogMessagingUtil.Instance.AppendLine("לא קיים קוד תהליך גביה- במידה ומופעל בדיקה האם להגדיר גבייה = 900 בטבלת סיבות Pending");
                                isCollectActive = false;
                            }
                        }

                        if (isCollectActive)
                        {
                            bool isStatusVPA = false;
                            try
                            {
                                isStatusVPA = myDeclarationUpdateService.CheckFileStatus(_MyDeclarationPM, requestParams.LoggingUserId, "VPA");
                            }
                            catch (Exception e)
                            {
                                LogMessagingUtil.Instance.AppendLine("Exception was thrown while checking if VPA exist in the file " + _MyDeclarationPM.CustomFileNo + Environment.NewLine + e.Message);
                            }
                            if (isStatusVPA) isCollectActive = false;
                        }

                        if (isCollectActive)
                        {
                            def = myGDFDATAQueryService.GetSingle("ISRAEL", "CGO_COL_LOW_DIF", "NON", "NON", false, true);
                            string defValue = def.DEFDATA;
                            decimal defaultAmount = 0;
                            var boolvar = (decimal.TryParse(defValue, out defaultAmount));
                            decimal totalTax = _MyDeclarationPM.TotalTax > 0 ? _MyDeclarationPM.TotalTax.Value : 0;
                            decimal prevTotalTax = prev_TotalTax > 0 ? prev_TotalTax.Value : 0;
                            if (defaultAmount > 0 && defaultAmount >= totalTax - prevTotalTax)
                            {
                                isCollectActive = false;
                            }
                        }

                        if (isCollectActive && _MyDeclarationPM.TotalTax > 0 && _MyDeclarationPM.TotalTax != prev_TotalTax)
                        {
                            if (declarationPendingPM_900 != null && declarationPendingPM_900.Status != "S")
                            {
                                if (_MyDeclarationPM.SupplierInvoices != null && _MyDeclarationPM.SupplierInvoices.FirstOrDefault().IncotermCode != "DDP")
                                {
                                    isSendVPE = true;
                                }
                            }
                        }

                        if (isCollectActive)
                        {

                            LogMessagingUtil.Instance.AppendLine("תהליך גביה- במידה ומופעל בדיקה האם להגדיר גבייה = 900");
                            if (_MyDeclarationPM.SupplierInvoices != null && _MyDeclarationPM.SupplierInvoices.FirstOrDefault().IncotermCode != "DDP" && _MyDeclarationPM.TotalTax > 0)
                            {
                                if (declarationPendingPM_900 == null)
                                {

                                    declarationPendingPM_900 = new DeclarationPendingPM();
                                    declarationPendingPM_900.CourierPendingReasonCode = "900";
                                    declarationPendingPM_900.Status = "A";
                                    declarationPendingPM_900.ChangeSetOp = ChangeSetOperation.Insert;
                                    _MyDeclarationCourierStatusPM.DeclarationPendings.Add(declarationPendingPM_900);
                                }
                                else if (declarationPendingPM_900.Status != "A")
                                {
                                    declarationPendingPM_900.ChangeSetOp = ChangeSetOperation.Update;
                                    declarationPendingPM_900.Status = "A";
                                }
                                if (declarationPendingPM_900.ChangeSetOp != ChangeSetOperation.None)
                                {
                                    LogMessagingUtil.Instance.AppendLine("Set Courier Pending Reason Code 900");
                                    if (_MyDeclarationCourierStatusPM.ChangeSetOp != ChangeSetOperation.Update) _MyDeclarationCourierStatusPM.ChangeSetOp = ChangeSetOperation.Update;
                                }
                            }
                            else if (declarationPendingPM_900 != null)
                            {
                                declarationPendingPM_900.ChangeSetOp = ChangeSetOperation.Update;
                                declarationPendingPM_900.Status = "S";
                                if (_MyDeclarationCourierStatusPM.ChangeSetOp != ChangeSetOperation.Update) _MyDeclarationCourierStatusPM.ChangeSetOp = ChangeSetOperation.Update;
                                LogMessagingUtil.Instance.AppendLine("Courier Pending Reason Code 900 Set as Solved");
                            }
                        }
                    }

                }
                if (_MyDeclarationCourierStatusPM.ChangeSetOp == ChangeSetOperation.Update)
                {
                    DeclarationCourierStatusUpdateService declarationCourierStatusUpdateService = new DeclarationCourierStatusUpdateService(context, new Dictionary<string, IContext>(), _MyDeclarationPM.Tenant);
                    declarationCourierStatusUpdateService.Update(_MyDeclarationCourierStatusPM, true);
                }
             }

            if (_MyDeclarationPM.Direction != "E")
            {
                //<--- Yuval Chalup 09.11.2015 TASK-16498 - Update PaymentOrderNumber in Payment
                if (customResponse.DeclarationPaymentDetails != null)
                {
                    if (customResponse.DeclarationPaymentDetails.PaymentOrderNumber != null || (_IsSubmitDeclarationResponse == true && string.IsNullOrWhiteSpace(paymentOrderNumber) && _MyDeclarationPM.DeclarationStatusTypeCode == "5" && _MyDeclarationPM.TotalTax <= 5))
                    {
                        var unifreightDeclarationPaymentUpdateService = new UnifreightDeclarationPaymentUpdateService(declarationPaymentsPM, _MyDeclarationPM);
                        unifreightDeclarationPaymentUpdateService.Update();
                    }
                }
            }
            //Yuval Chalup 09.11.2015 TASK-16498 --->

            if (customResponse.CollateralRequestDetails != null) // Create Collateral
            {
                LogMessagingUtil.Instance.AppendLine("CollateralRequestDetails: Create Collateral");
                 var requestXml = XmlGenericUtil<UnifreightIIG.Common.ExportDeclarationServiceReference.CollateralRequestDetails[]>
                    .SerializeObject(customResponse.CollateralRequestDetails);
                var collateralArry = XmlGenericUtil<UnifreightIIG.Common.MessageLib.Collateral.CollateralRequestDetails[]>.DeSerializeObject(requestXml);

                COLT_NG_8211_MSG10040_CollateralRequestMsg myCOLT_NG_8211_MSG10040_CollateralRequestMsg = new COLT_NG_8211_MSG10040_CollateralRequestMsg();
                myCOLT_NG_8211_MSG10040_CollateralRequestMsg.ResponseContentHeader = null;
                myCOLT_NG_8211_MSG10040_CollateralRequestMsg.CollateralRequestDetails = collateralArry;
                var xml = XmlGenericUtil<UnifreightIIG.Common.MessageLib.Collateral.COLT_NG_8211_MSG10040_CollateralRequestMsg>
                    .SerializeObject(myCOLT_NG_8211_MSG10040_CollateralRequestMsg);

                var ser = XmlGenericUtil<UnifreightIIG.Common.MessageLib.Collateral.COLT_NG_8211_MSG10040_CollateralRequestMsg>.DeSerializeObject(xml);
                var DF_MSG10040_CollateralRequestMsgResponseService = new DF_8211_CollateralRequestMsgResponseService();
                DF_MSG10040_CollateralRequestMsgResponseService.Update(ser, requestParams);
            }

            this.MyResponseData.UserMessage = "בקשה נשלחה בהצלחה";
            string declarationStatusTypeName = _MyDeclarationPM.DeclarationStatusTypeCode;
            if (!string.IsNullOrWhiteSpace(_MyDeclarationPM.DeclarationStatusTypeCode))
            {
                DeclarationStatusTypeQueryService declarationStatusTypeQueryService = new DeclarationStatusTypeQueryService(_MyDeclarationPM.Tenant);
                DeclarationStatusTypePM declarationStatusType = declarationStatusTypeQueryService.GetSingle(_MyDeclarationPM.DeclarationStatusTypeCode, false, true);
                if (declarationStatusType != null)
                {
                    declarationStatusTypeName = declarationStatusType.LocalName;
                }
                this.MyResponseData.UserMessage = "בקשה נשלחה - סטטוס הטיוטה " + declarationStatusTypeName;
            }

            MyResponseData.ApplicationID = requestParams.AppicationId;
            MyResponseData.Succeeded = true;
            
            if (requestParams.InterfaceTypeCode == "9079")
            {
                myDeclarationUpdateService.SendDelayedDeclarationStatusRequest(_MyDeclarationPM);
            }
          
                if (isSendVPE)
            {
                string xml_status = "new";
                RaiseStatus(_MyDeclarationPM, "", "VPE", xml_status);
            }
            if (_MyDeclarationPM.Direction == "E" && _MyDeclarationPM.AutoSending && _MyDeclarationPM.IsDiamondDeclaration && customResponse?.Response?.Status[0]?.NameCode?.Value == "13" && lastStatus != "13")
            {
                CustomsRequestsSheetQueryService customsRequestsSheetQuery = new CustomsRequestsSheetQueryService(context);
                List<CustomsRequestsSheetPM> requestSheets = customsRequestsSheetQuery.GetRequestByInterfaceTypeCode(_MyDeclarationPM.Tenant, "2755E",
                   requestParams.LoggingObjectTableId, requestParams.LoggingEntityId,
                    _MyDeclarationPM.CustomFileNo);

                if (requestSheets == null || requestSheets.Count()==0) {
                    CreateDeclartionPayment(requestParams);
                    Send2755(requestParams);
                }
           
            }
        }

        private void CreateDeclartionPayment(GenericRequestParams requestParams)
        {
            DeclarationPaymentQueryService declarationPaymentQueryService = new DeclarationPaymentQueryService(requestParams.Tenant);

            DeclarationPaymentPM payment = declarationPaymentQueryService.GetSingle(_MyDeclarationPM.Id, true, false);
            if (payment == null)
            {
                 payment = new DeclarationPaymentPM()
                {
                    DeclarationId = _MyDeclarationPM.Id,
                    CreatedByUserId = _MyDeclarationPM.CreatedByUserId,
                    PaymentDate = DateTime.Now,
                    SignatoryIdentification = requestParams.SignByPersonalId,
                    Tenant = requestParams.Tenant,
                    ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert,
                };
            

            }
            else
            {
                payment.PaymentDate = DateTime.Now;
                payment.SignatoryIdentification = requestParams.SignByPersonalId;
                payment.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;

            }

            var context = CustomContext.GetContext(requestParams.Tenant);
            DeclarationPaymentUpdateService declarationPaymentUpdateService = new DeclarationPaymentUpdateService(context, new Dictionary<string, IContext>(), requestParams.Tenant);
            declarationPaymentUpdateService.Update(payment, true);


        }
        private void Send2755(GenericRequestParams requestParams)
        {
            GenericRequestParams submitRequestParams = new GenericRequestParams();
            submitRequestParams.AppicationId = requestParams.AppicationId;
            submitRequestParams.InterfaceTypeCode = "2755E";
        
            submitRequestParams.Tenant = requestParams.Tenant;
            submitRequestParams.RequestVIA = SendRequestVIA.WebServiceBatch;
            submitRequestParams.LoggingUserId = requestParams.LoggingUserId;
            submitRequestParams.ForcePersonalSign = false;
            submitRequestParams.ForceCompanySign = true;
            submitRequestParams.LoggingEntityId = requestParams.LoggingEntityId;
            submitRequestParams.LoggingEntityId2 = requestParams.LoggingEntityId2;
            submitRequestParams.LoggingObjectTableId = requestParams.LoggingObjectTableId;
            submitRequestParams.LoggingObjectTableId2 = requestParams.LoggingObjectTableId2;

            var messagingService = new
                DF_NG_2755_MSG12001_SubmitExportDeclarationMessagingService();
            INF_MSG_GenericResponseData submitResponseData = messagingService.Send(submitRequestParams);
            
        }
        private void UpdateDepositionStatusCode()
        {
            if (_MyDeclarationError != null && _MyDeclarationError.Entitites != null && _MyDeclarationError.Entitites.Count > 0)
            {
                //Go over all the 'Entity'
                foreach (var entity in _MyDeclarationError.Entitites)
                {
                    if (entity.FieldErrors != null)
                    {
                        //Go over all the 'FieldErrors'
                        foreach (var fieldErrors in entity.FieldErrors)
                        {
                            //Get all 'FieldErrors' for the 'FieldError'
                            List<field> fieldList = (from a in entity.FieldErrors
                                                     where (a.Code == "4589")
                                                     select a).ToList();
                            if (fieldList.Count > 0)
                            {
                                if (string.IsNullOrWhiteSpace(_MyDeclarationPM.DepositionStatusCode)) _MyDeclarationPM.DepositionStatusCode = "R";
                                return;
                            }
                        }
                    }
                }
            }
            if (_MyDeclarationPM.DepositionStatusCode == "R") _MyDeclarationPM.DepositionStatusCode = null;
        }

       

        //ITZIK+MIRT  private string GetErrosXmlFromResponseHeaderExeption()          {              return _ResponseHeaderExeption.ErrorDescription;         }

        private void DeleteDeclarationConstraints(DeclarationConstraintUpdateService myDeclarationConstraintUpdateService) // Delete old Constraints
        {
            if (this._MyDeclarationPM.DeclarationConstraints == null)
            {
                return;
            }

            foreach (var constraintItem in this._MyDeclarationPM.DeclarationConstraints)
            {
                constraintItem.ChangeSetOp = ChangeSetOperation.Delete;
                //myDeclarationConstraintUpdateService.Update(constraintItem, true);
                _MyDeclarationPM.DeletedDeclarationConstraints.Add(constraintItem);
            }
        }

        private List<DeclarationConstraintPM> BuildDeclarationConstraints(ResponseError[] responseError)
        {
            if (responseError == null)
            {
                return null;
            }
 
            var declarationConstraintPMList = new List<DeclarationConstraintPM>();
            foreach (var errorItem in responseError)
            {
                if (errorItem.DMExtensions != null)
                {
                    DeclarationConstraintPM declarationConstraintToCheckDistinct = null;
                    declarationConstraintToCheckDistinct = declarationConstraintPMList.Where(constraint => constraint.ConstraintNumber == errorItem.DMExtensions.ConstraintID.ToString()).FirstOrDefault();

                    if (declarationConstraintToCheckDistinct == null)
                    {
                        DeclarationConstraintPM declarationConstraint = null;
                        declarationConstraint = FindConstraintInList(errorItem.DMExtensions.ConstraintID.ToString(), _MyDeclarationPM.Tenant, _MyDeclarationPM.Id);

                        if (declarationConstraint == null)
                        {
                            declarationConstraint = new DeclarationConstraintPM();
                            declarationConstraint.ChangeSetOp = ChangeSetOperation.Insert;
                            declarationConstraint.ConstraintNumber = errorItem.DMExtensions.ConstraintID.ToString();
                            declarationConstraint.ConstraintTypeCode = errorItem.DMExtensions.ConstraintType.ToString();
                        }
                        else
                        {
                            declarationConstraint.ChangeSetOp = ChangeSetOperation.Update;
                            _MyDeclarationPM.DeletedDeclarationConstraints.Remove(declarationConstraint);
                        }

                        declarationConstraint.ConstraintStatusCode = errorItem.DMExtensions.ConstraintStatus.ToString();
                        declarationConstraintPMList.Add(declarationConstraint);
                    }
                }
            }
            return declarationConstraintPMList;
        }
        private List<DeclarationConstraintPM> BuildDeclarationConstraintsTEST(ResponseError[] responseError)
        {
            if (responseError == null)
            {
                return null;
            }

            var declarationConstraintPMList = new List<DeclarationConstraintPM>();

            try
            {
                responseError.Where(errorItem => errorItem.DMExtensions != null).ToList().ToDictionary(r => r.DMExtensions.ConstraintID.ToString());
            }
            catch (System.Exception)
            {

                throw new System.Exception("הגיעה מספר אילוץ כפול, נא לפנות למלמ");
            }


            var myhshSet = new HashSet<string>();
            foreach (var item in responseError.Where(errorItem => errorItem.DMExtensions != null))
            {

                myhshSet.Add(item.DMExtensions.ConstraintID.ToString());
            }
            foreach (var hshSetKey in myhshSet)
            {
                ResponseError errorItem = null;
                errorItem = responseError.LastOrDefault(r => r.DMExtensions != null && r.DMExtensions.ConstraintID.ToString() == hshSetKey);

                 {
                    DeclarationConstraintPM declarationConstraint = null;
                    declarationConstraint = FindConstraintInList(errorItem.DMExtensions.ConstraintID.ToString(), _MyDeclarationPM.Tenant, _MyDeclarationPM.Id);

                    if (declarationConstraint == null)
                    {
                        declarationConstraint = new DeclarationConstraintPM();
                        declarationConstraint.ChangeSetOp = ChangeSetOperation.Insert;
                        declarationConstraint.ConstraintNumber = errorItem.DMExtensions.ConstraintID.ToString();
                        declarationConstraint.ConstraintTypeCode = errorItem.DMExtensions.ConstraintType.ToString();
                    }
                    else
                    {
                        declarationConstraint.ChangeSetOp = ChangeSetOperation.Update;
                        _MyDeclarationPM.DeletedDeclarationConstraints.Remove(declarationConstraint);
                    }

                    declarationConstraint.ConstraintStatusCode = errorItem.DMExtensions.ConstraintStatus.ToString();
                    declarationConstraintPMList.Add(declarationConstraint);
                }
            }
            return declarationConstraintPMList;
        }

        private DeclarationConstraintPM FindConstraintInList(string constraintNumber, int tenant, string declarationID)
        {
            List<DeclarationConstraintPM> constraintList = (from a in _MyDeclarationPM.DeclarationConstraints
                                                            where a.ConstraintNumber == constraintNumber &&
                                                            a.Tenant == tenant && a.DeclarationID == declarationID
                                                            select a).ToList();

            if (constraintList.Count > 0)
            {
                return constraintList[0];
            }
            return null;
        }

        private void DeleteSupplierInvoiceItemsTaxes(SupplierInvoiceItemsTaxUpdateService mySupplierInvoiceItemsTaxUpdateService)
        {

            foreach (var si in _MyDeclarationPM.SupplierInvoices)
            {
                foreach (var supplierInvoiceItem in si.SupplierInvoiceItems)
                {
                    foreach (var item in supplierInvoiceItem.SupplierInvoiceItemTaxes)
                    {
                        item.ChangeSetOp = ChangeSetOperation.Delete;
                        mySupplierInvoiceItemsTaxUpdateService.Update(item, true);
                    }
                }
            }
        }

        private void DeleteDeclarationTaxes(DeclarationTaxUpdateService myDeclarationTaxUpdateService)
        {

            foreach (var item in _MyDeclarationPM.DeclarationTaxes)
            {
                item.ChangeSetOp = ChangeSetOperation.Delete;
                myDeclarationTaxUpdateService.Update(item, true);
            }
        }


        private void DeleteSupplierInvoiceItemsVehicleMods(SupplierInvoiceItemVehicleModUpdateService mySupplierInvoiceItemVehicleModUpdateService) // moran 20.10.15 - Task 17209
        {

            foreach (var si in _MyDeclarationPM.SupplierInvoices)
            {
                foreach (var supplierInvoiceItem in si.SupplierInvoiceItems)
                {
                    foreach (var vehicle in supplierInvoiceItem.SupplierInvoiceItemVehicles)
                    {
                        foreach (var vehicleMod in vehicle.SupplierInvoiceItemVehicleMods)
                        {
                            vehicleMod.ChangeSetOp = ChangeSetOperation.Delete;
                            mySupplierInvoiceItemVehicleModUpdateService.Update(vehicleMod, true);
                        }
                    }
                }
            }
        }


        private void DeleteSupplierInvoiceItemsModVehicles(SupplierInvoiceItemModVehicleUpdateService mySupplierInvoiceItemModVehicleUpdateService) // moran 24.11.15 - Task 17424
        {

            foreach (var si in _MyDeclarationPM.SupplierInvoices)
            {
                foreach (var supplierInvoiceItem in si.SupplierInvoiceItems)
                {
                    foreach (var modVehicle in supplierInvoiceItem.SupplierInvoiceItemModVehicles)
                    {
                        modVehicle.ChangeSetOp = ChangeSetOperation.Delete;
                        mySupplierInvoiceItemModVehicleUpdateService.Update(modVehicle, true);
                    }
                }
            }
        }

        private void DeleteSupplierInvioceItemCertificates(SupplierInvioceItemCertificatUpdateService mySupplierInvioceItemCertificatUpdateService)
        {
            foreach (var si in _MyDeclarationPM.SupplierInvoices)
            {
                foreach (var supplierInvoiceItem in si.SupplierInvoiceItems)
                {
                    foreach (var modCertificats in supplierInvoiceItem.SupplierInvioceItemCertificats)
                    {
                        modCertificats.ChangeSetOp = ChangeSetOperation.Delete;
                        mySupplierInvioceItemCertificatUpdateService.Update(modCertificats, true);
                    }
                }
            }
        }

        private List<SupplierInvoicePM> GetSupplierInvoicesPM(
            DF_NG_2757_MSG10004_ExportDeclarationResponse customResponse)
        {
            var supplierInvoicesPMList = new List<SupplierInvoicePM>();

            foreach (var goodsShipment in customResponse.Response.Declaration.GoodsShipment)
            {
                 var supplierInvoicePM = this._MyDeclarationPM.SupplierInvoices.FirstOrDefault(si => si.SequenceNumeric == goodsShipment.SequenceNumeric);

                if (supplierInvoicePM == null)
                {
                    throw new System.Exception(
                        "unable to find the supplierInvoicePM from goodsShipment.Invoice.ID.Value " + goodsShipment.Invoice.ID.Value);
                }
                var InvoiceCounterKey = supplierInvoicePM.InvoiceCounterKey;

                //Update supplier Valuation - Additional costs details from custom
                supplierInvoicePM.SupplierInvoiceModifications = GetSupplierInvoiceModifications(goodsShipment, ref supplierInvoicePM);
                //Update supplier items
                supplierInvoicePM.SupplierInvoiceItems = GetSupplierInvoiceItems(goodsShipment, ref supplierInvoicePM);

                if (goodsShipment.Invoice != null && goodsShipment.Invoice.DMExtensions != null && goodsShipment.Invoice.DMExtensions.RateNumeric != null)
                {
                    supplierInvoicePM.ExchangeRate = goodsShipment.Invoice.DMExtensions.RateNumeric.Value;
                }

                supplierInvoicePM.ChangeSetOp = ChangeSetOperation.Update;
                supplierInvoicesPMList.Add(supplierInvoicePM);
            }

            return supplierInvoicesPMList;
        }

        private List<SupplierInvoiceModificationPM> GetSupplierInvoiceModifications(DeclarationGoodsShipment goodsShipment, ref SupplierInvoicePM supplierInvoicePM)
        {
            // moran 26.5.15 - 13564 -->
            //var supplierInvoiceModificationPMList = new List<SupplierInvoiceModificationPM>();
            var supplierInvoiceModificationPMList = new List<SupplierInvoiceModificationPM>(supplierInvoicePM.SupplierInvoiceModifications);
            // moran 26.5.15 - 13564 <--
            //if (goodsShipment.CustomsValuation == null)
            //{
            //    return null;
            //}

            //Check if there is a DECLARED Fee (I01) in message
            //    DeclarationGoodsShipmentCustomsValuation declarationGoodsShipmentCustomsValuation_I01 = goodsShipment.CustomsValuation.FirstOrDefault(rec => rec.ChargesTypeCode.Value == "I01");

            //foreach (var valuationItem in goodsShipment.CustomsValuation)
            //{
            //    if (valuationItem.ChargesTypeCode.Value != "67" && valuationItem.ChargesTypeCode.Value != "144")
            //    {
            //        //If there is a DECLARED Fee (I01) in message:
            //        //1 - Do NOT get the CALCULATED Fee (I02) from message
            //        //2 - Delete the CALCULATED from DB
            //        if (declarationGoodsShipmentCustomsValuation_I01 != null && valuationItem.ChargesTypeCode.Value == "I02")
            //        {
            //            var supplierInvoiceModificationPM = supplierInvoicePM.SupplierInvoiceModifications.FirstOrDefault(si => si.TypeCode == valuationItem.ChargesTypeCode.Value);
            //            if (supplierInvoiceModificationPM != null)
            //            {
            //                //If exist delete
            //                supplierInvoiceModificationPM.ChangeSetOp = ChangeSetOperation.Delete;
            //                supplierInvoiceModificationPMList.Add(supplierInvoiceModificationPM);
            //            }
            //        }
            //        else
            //        {
            //            var supplierInvoiceModificationPM = supplierInvoicePM.SupplierInvoiceModifications.FirstOrDefault(si => si.TypeCode == valuationItem.ChargesTypeCode.Value);
            //            if (supplierInvoiceModificationPM != null)
            //            {
            //                //If exist update
            //                supplierInvoiceModificationPM.ChangeSetOp = ChangeSetOperation.Update;
            //            }
            //            else
            //            {
            //                //Else create new SupplierInvoiceModification record
            //                supplierInvoiceModificationPM = new SupplierInvoiceModificationPM();
            //                supplierInvoiceModificationPM.ChangeSetOp = ChangeSetOperation.Insert;
            //            }
            //            supplierInvoiceModificationPM.TypeCode = valuationItem.ChargesTypeCode.Value;
            //            supplierInvoiceModificationPM.CurrencyTypeCode = valuationItem.OtherChargeDeductionAmount.currencyID.ToString(); // TO CHECK? ENUM?
            //            supplierInvoiceModificationPM.Amount = valuationItem.OtherChargeDeductionAmount.Value;
            //            if (supplierInvoiceModificationPM.ChangeSetOp == ChangeSetOperation.Insert)
            //            {
            //                supplierInvoiceModificationPMList.Add(supplierInvoiceModificationPM);
            //            }
            //        }
            //    }
            //}

            return supplierInvoiceModificationPMList;
        }
#if refreshWSDL20141230
        private SupplierInvoiceItemsModificationPM GetSupplierInvoiceItemsTaxesModifications(
            DeclarationGoodsShipmentGovernmentAgencyGoodsItemCommodityDMExtensionsValuationDeductionAdjustment 
            declarationGoodsShipmentGovernmentAgencyGoodsItemCommodityDMExtensionsValuationDeductionAdjustment)
        {

            return null;
        }
#endif


        private List<SupplierInvoiceItemPM> GetSupplierInvoiceItems(
            DeclarationGoodsShipment goodsShipment,
            ref SupplierInvoicePM supplierInvoicePM)
        {
            var supplierInvoiceItemsPMList = new List<SupplierInvoiceItemPM>();
            if (supplierInvoicePM.IsAccumalated == true && supplierInvoicePM.SupplierInvoiceItems != null && supplierInvoicePM.SupplierInvoiceItems.Count > 0)
            {
                supplierInvoicePM.SupplierInvoiceItems.RemoveAll(rec => rec.IsParent != true);
            }
            foreach (var governmentAgencyGoodsItem in goodsShipment.GovernmentAgencyGoodsItem)
            {
                var supplierInvoiceItemPM = supplierInvoicePM.SupplierInvoiceItems.FirstOrDefault(si => si.SequenceNumeric == governmentAgencyGoodsItem.SequenceNumeric);
                //<--- Added by Yuval Chalup 26.05.2015 TASK-13473
                if (supplierInvoiceItemPM == null)
                {
                    if (governmentAgencyGoodsItem.Commodity != null)
                    {
                        if (governmentAgencyGoodsItem.Commodity.Classification != null)
                        {
                            if (governmentAgencyGoodsItem.Commodity.Classification.Count() > 0)
                            {
                                if (governmentAgencyGoodsItem.Commodity.Classification[0] != null)
                                {
                                    throw new System.Exception(
                                       "unable to find the supplierInvoiceItemPM from governmentAgencyGoodsItem.Commodity.Classification " + governmentAgencyGoodsItem.Commodity.Classification[0].ID.Value);
                                }
                            }
                        }
                    }
                    throw new System.Exception(
                       "unable to find the supplierInvoiceItemPM from governmentAgencyGoodsItem.SequenceNumeric " + governmentAgencyGoodsItem.SequenceNumeric);
                }


                //Added by Yuval Chalup 26.05.2015 TASK-13473 --->

                var supplierInvoiceItemsTaxPMList = new List<SupplierInvoiceItemsTaxPM>();
                //Update supplier item Valuation Adjustment - Commodity price adjustments
                //// supplierInvoiceItemPM.SupplierInvoiceItemsMods = GetSupplierInvoiceItemsModifications(governmentAgencyGoodsItem.ValuationAdjustment, supplierInvoiceItemPM);
                // moran 24.11.15 - Task 17424 -->
                //  supplierInvoiceItemPM.SupplierInvoiceItemModVehicles = GetSupplierInvoiceItemsModVehicles(governmentAgencyGoodsItem.DMExtensions.VehicleValuationAdjustment, supplierInvoiceItemPM);
                // moran 24.11.15 - Task 17424 <--

                    supplierInvoiceItemPM.SupplierInvioceItemCertificats = GetSupplierInvioceItemCertificats(supplierInvoicePM, supplierInvoiceItemPM);
               
                if (governmentAgencyGoodsItem.Commodity == null)
                {
                    continue;
                }
                //TODO:DDDD

                if (governmentAgencyGoodsItem.Commodity.DMExtensions != null)
                {
                    if (governmentAgencyGoodsItem.Commodity.DMExtensions.ItemFOBAmountForeign != null)
                        supplierInvoiceItemPM.ItemFOBAmountForeign = Math.Round(governmentAgencyGoodsItem.Commodity.DMExtensions.ItemFOBAmountForeign.Value, 2);
                    if (governmentAgencyGoodsItem.Commodity.DMExtensions.ItemFOBAmountNIS != null)
                        supplierInvoiceItemPM.ItemFOBAmountNIS = Math.Round(governmentAgencyGoodsItem.Commodity.DMExtensions.ItemFOBAmountNIS.Value, 2);

                }
                if (governmentAgencyGoodsItem.Commodity.DutyTaxFee != null)
                {



                    foreach (var dutyTaxFee in governmentAgencyGoodsItem.Commodity.DutyTaxFee)
                    {


                        var supplierInvoiceItemsTaxPM = new SupplierInvoiceItemsTaxPM();
                        supplierInvoiceItemsTaxPM.ChangeSetOp = ChangeSetOperation.Insert;
                        //supplierInvoiceItemsTaxPM.DeclarationId = this._MyDeclarationPM.Id; //Removed by Yuval Chalup 26.05.2015 TASK-13473 (Move to SupplierInvoiceItemsTaxUpdateService.OnUpdating)
                        supplierInvoiceItemsTaxPM.Tenant = this._MyDeclarationPM.Tenant;
                        //supplierInvoiceItemsTaxPM.InvoiceCounterKey = supplierInvoicePM.InvoiceCounterKey; //Removed by Yuval Chalup 26.05.2015 TASK-13473 (Move to SupplierInvoiceItemsTaxUpdateService.OnUpdating)
                        //supplierInvoiceItemsTaxPM.LineNumber = supplierInvoiceItemPM.LineNumber; //Removed by Yuval Chalup 26.05.2015 TASK-13473 (Move to SupplierInvoiceItemsTaxUpdateService.OnUpdating)
                        supplierInvoiceItemsTaxPM.TaxTypeCode = dutyTaxFee.TypeCode.Value;
                        //if (dutyTaxFee.DutyRegimeCode != null)
                        //{
                        //    supplierInvoiceItemsTaxPM.TradeAgreementTypeCode = dutyTaxFee.DutyRegimeCode.Value;
                        //}
                        supplierInvoiceItemsTaxPM.TaxRate = dutyTaxFee.TaxRate;
                        supplierInvoiceItemsTaxPM.TaxBaseAmount = dutyTaxFee.AdValoremTaxBaseAmount.Value;
                        supplierInvoiceItemsTaxPM.TaxAmount = dutyTaxFee.CalculatedTax.Amount.Value;
                        supplierInvoiceItemsTaxPM.DeferedTaxAmount = dutyTaxFee.CalculatedTax.DeferedTaxAmount.Value;
                        if (dutyTaxFee.CalculatedTax.DefinedPerUnitMethod != null)
                        {
                            supplierInvoiceItemsTaxPM.DefinedPerUnitMeasure = dutyTaxFee.CalculatedTax.DefinedPerUnitMethod.Value;
                        }
                        supplierInvoiceItemsTaxPM.AlternateRate = dutyTaxFee.CalculatedTax.AlternateRate.Value;
                        if (dutyTaxFee.CalculatedTax.AlternateDefinedPerUnitMeasure != null)
                        {
                            supplierInvoiceItemsTaxPM.AlternateDefinedPerUnitMeasure = dutyTaxFee.CalculatedTax.AlternateDefinedPerUnitMeasure.Value;
                        }
                        if (dutyTaxFee.CalculatedTax.DefinedPerUnitQuantity != null)
                        {
                            supplierInvoiceItemsTaxPM.DefinedPerUnitQuantity = dutyTaxFee.CalculatedTax.DefinedPerUnitQuantity.Value;
                        }
                        if (dutyTaxFee.CalculatedTax.AlternateDefinedPerUnitQuantity != null)
                        {
                            supplierInvoiceItemsTaxPM.AlternateDefinedPerUnitQuant = dutyTaxFee.CalculatedTax.AlternateDefinedPerUnitQuantity.Value;
                        }
                        if (dutyTaxFee.CalculatedTax.MeasurementUnitCode != null)
                        {
                            supplierInvoiceItemsTaxPM.MeasurementUnitCode = dutyTaxFee.CalculatedTax.MeasurementUnitCode.Value;
                        }
                        if (dutyTaxFee.CalculatedTax.AlternateMeasurementUnit != null)
                        {
                            supplierInvoiceItemsTaxPM.AlternateMeasurementUnitCode = dutyTaxFee.CalculatedTax.AlternateMeasurementUnit.Value;
                        }
                        if (dutyTaxFee.CalculatedTax.TradeLevyNumber != null)
                        {
                            supplierInvoiceItemsTaxPM.TradeLevyNumber = dutyTaxFee.CalculatedTax.TradeLevyNumber.Value;
                        }
                        //if (dutyTaxFee.CalculatedTax.TotalBtlCoverageNIS != null)
                        //{
                        //    supplierInvoiceItemsTaxPM.TotalBtlCoverageNIS = dutyTaxFee.CalculatedTax.TotalBtlCoverageNIS.Value;
                        //    _TotalBtlCoverageNISSum = _TotalBtlCoverageNISSum + dutyTaxFee.CalculatedTax.TotalBtlCoverageNIS.Value;
                        //}
                        //// moran 21.11.13 - Bug 2083 - change handle -->
                        //AddSupplierInvoiceItemsTaxesModificationPM(supplierInvoiceItemsTaxPM, governmentAgencyGoodsItem.Commodity);
                        //supplierInvoiceItemsTaxPM.SupplierInvoiceItemsTaxesModifications = GetSupplierInvoiceItemsTaxesModifications(governmentAgencyGoodsItem.Commodity.DMExtensions.ValuationDeductionAdjustment, supplierInvoiceItemsTaxPM);
                        // moran 21.11.13 - Bug 2083 - change handle <--
                        supplierInvoiceItemsTaxPMList.Add(supplierInvoiceItemsTaxPM);
                    }
                }

                supplierInvoiceItemPM.SupplierInvoiceItemTaxes = supplierInvoiceItemsTaxPMList;

                totGeneralTaxCalc = 0;
                totPurchaseCalc = 0;
                totVatCalc = 0;

                generalTax = 0;
                purchase = 0;
                vat = 0;

                foreach (var tax in supplierInvoiceItemPM.SupplierInvoiceItemTaxes)
                {
                    if (tax.TaxTypeCode == "1")
                    {
                        generalTax += tax.TaxAmount;
                    }
                    if (tax.TaxTypeCode == "16")
                    {
                        purchase += tax.TaxAmount;
                    }
                    if (tax.TaxTypeCode == "15")
                    {
                        vat += tax.TaxAmount;
                    }
                }

                // moran 6.10.15 - Task 17209 --> 
                supplierInvoiceItemPM.SupplierInvoiceItemVehicles = GetSupplierInvoiceItemsVehicles(governmentAgencyGoodsItem.DMExtensions.Vehicle, supplierInvoiceItemPM);
                // moran 6.10.15 - Task 17209 <--
                if (supplierInvoiceItemPM.SupplierInvoiceItemVehicles != null && supplierInvoiceItemPM.SupplierInvoiceItemVehicles.Count() > 0 && supplierInvoiceItemPM.SupplierInvoiceItemVehicles.LastOrDefault().SupplierInvoiceItemVehicleAdds != null && supplierInvoiceItemPM.SupplierInvoiceItemVehicles.LastOrDefault().SupplierInvoiceItemVehicleAdds.Count() > 0)
                {
                    decimal? diff = 0;
                    if (generalTax != totGeneralTaxCalc)
                    {
                        diff = generalTax - totGeneralTaxCalc;
                        supplierInvoiceItemPM.SupplierInvoiceItemVehicles.LastOrDefault().SupplierInvoiceItemVehicleAdds.LastOrDefault().ChassisTax += diff;
                    }
                    if (purchase != totPurchaseCalc)
                    {
                        diff = purchase - totPurchaseCalc;
                        supplierInvoiceItemPM.SupplierInvoiceItemVehicles.LastOrDefault().SupplierInvoiceItemVehicleAdds.LastOrDefault().ChassisPurchaseTax += diff;
                    }
                    if (vat != totVatCalc)
                    {
                        diff = vat - totVatCalc;
                        supplierInvoiceItemPM.SupplierInvoiceItemVehicles.LastOrDefault().SupplierInvoiceItemVehicleAdds.LastOrDefault().ChassisVat += diff;
                    }
                }
                supplierInvoiceItemPM.ChangeSetOp = ChangeSetOperation.Update;
                supplierInvoiceItemsPMList.Add(supplierInvoiceItemPM);
                var NewSupplierInvioceItemCertificats = supplierInvoiceItemPM.SupplierInvioceItemCertificats.Where(r => r.ChangeSetOp == ChangeSetOperation.Insert).ToList();
                if (supplierInvoicePM.IsAccumalated == true && supplierInvoiceItemPM.IsParent == true && NewSupplierInvioceItemCertificats != null && NewSupplierInvioceItemCertificats.Count > 0)
                {
                    AddNewCertificatesToChildItems(supplierInvoiceItemsPMList, supplierInvoiceItemPM, NewSupplierInvioceItemCertificats);
                }
            }

            return supplierInvoiceItemsPMList;
        }

        private void AddNewCertificatesToChildItems(List<SupplierInvoiceItemPM> supplierInvoiceItemsPMList, SupplierInvoiceItemPM supplierInvoiceItemParentPM, List<SupplierInvioceItemCertificatPM> newSupplierInvioceItemCertificats)
        {

            List<SupplierInvioceItemCertificatPM> newSupplierInvioceItemCertificatsForChild = newSupplierInvioceItemCertificats;
            var qs = new SupplierInvoiceItemQueryService(this._MyDeclarationPM.Tenant);
            SupplierInvioceItemCertificatQueryService supplierInvioceItemsCertificateQueryService = new SupplierInvioceItemCertificatQueryService(this._MyDeclarationPM.Tenant);
            List<SupplierInvoiceItemPM> supplierInvoiceItemsPMListforcert = qs.GetSupplierInvoiceItemsByParentFullPM(supplierInvoiceItemParentPM.DeclarationId, supplierInvoiceItemParentPM.CounterKey, supplierInvoiceItemParentPM.LineNumber, this._MyDeclarationPM.Tenant);
            foreach (var childSupplierInvoiceItemPM in supplierInvoiceItemsPMListforcert)
            {
                //newSupplierInvioceItemCertificatsForChild.ForEach(i => i.LineNumber = childSupplierInvoiceItemPM.LineNumber);
                childSupplierInvoiceItemPM.SupplierInvioceItemCertificats.AddRange(newSupplierInvioceItemCertificatsForChild);

                childSupplierInvoiceItemPM.ChangeSetOp = ChangeSetOperation.Update;
                supplierInvoiceItemsPMList.Add(childSupplierInvoiceItemPM);
            }
        }

        private List<SupplierInvioceItemCertificatPM> GetSupplierInvioceItemCertificats(SupplierInvoicePM supplierInvoicePM, SupplierInvoiceItemPM supplierInvoiceItemPM)
        {
            //if (supplierInvoiceItemPM.SupplierInvioceItemCertificats != null && supplierInvoiceItemPM.SupplierInvioceItemCertificats.Count() > 0)
            //{
            //    return supplierInvoiceItemPM.SupplierInvioceItemCertificats;
            //}

            //var supplierInvioceItemCertificatPMList = new List<SupplierInvioceItemCertificatPM>();
            var supplierInvioceItemCertificatPMList = supplierInvoiceItemPM.SupplierInvioceItemCertificats;


            List<string> certificateCodeListFromErrosXml = GetCertificateCodeListFromErrosXml("SupplierInvoice", supplierInvoicePM.SequenceNumeric.ToString(), "SupplierInvoiceItem", supplierInvoiceItemPM.SequenceNumeric.ToString());

            if (certificateCodeListFromErrosXml == null)
            {
                return supplierInvioceItemCertificatPMList;
                return null;
            }
            if (this._MyDeclarationPM.Direction == "E")
            {
                if (supplierInvioceItemCertificatPMList != null && supplierInvioceItemCertificatPMList.Count > 0)
                {
                    foreach (var supplierInvioceItemCertificatPM in supplierInvioceItemCertificatPMList)
                    {
                        if (supplierInvioceItemCertificatPM.AttachmentTypeCode == null && supplierInvioceItemCertificatPM.CertificateExemptionTypeCode == null && supplierInvioceItemCertificatPM.CertificateNumber == null&& !certificateCodeListFromErrosXml.Any(x=>x==supplierInvioceItemCertificatPM.ReqConfirmationTypeCode))
                        {
                            supplierInvioceItemCertificatPM.ChangeSetOp = ChangeSetOperation.Delete;
                        }

                    }

                }
            }
            foreach (var certificateCodeFromErrosXml in certificateCodeListFromErrosXml)
            {
                //Check if the code exists current SupplierInvioceItemCertificats
                List<string> entityList = (from a in supplierInvoiceItemPM.SupplierInvioceItemCertificats
                                           where (a.ReqConfirmationTypeCode == certificateCodeFromErrosXml&&a.ChangeSetOp!= ChangeSetOperation.Delete)
                                           select a.ReqConfirmationTypeCode).ToList();

                //If it does NOT exist - Add it to SupplierInvioceItemCertificat
                if (entityList.Count == 0)
                {
                    SupplierInvioceItemCertificatPM supplierInvioceItemCertificatPM = new SupplierInvioceItemCertificatPM();
                    supplierInvioceItemCertificatPM.ChangeSetOp = ChangeSetOperation.Insert;

                    supplierInvioceItemCertificatPM.Tenant = this._MyDeclarationPM.Tenant;
                    supplierInvioceItemCertificatPM.ReqConfirmationTypeCode = certificateCodeFromErrosXml;

                    supplierInvioceItemCertificatPMList.Add(supplierInvioceItemCertificatPM);
                }
            }

            return supplierInvioceItemCertificatPMList;
        }

        private List<string> GetCertificateCodeListFromErrosXml(string myChild1Type, string myChild1Sequence, string myChild2Type, string myChild2Sequence)
        {
            if (_MyDeclarationError == null)
            {
                return null;
            }
            if (_MyDeclarationError.Entitites.Count == 0)
            {
                return null;
            }

            List<string> certificateCodeListFromErrosXml = new List<string>();

            //Get all 'Entity' for the SupplierInvoiceItem
            List<Entity> entityList = (from a in _MyDeclarationError.Entitites
                                       where (a.Child1Type == myChild1Type && a.Child1Sequence == myChild1Sequence
                                       && a.Child2Type == myChild2Type && a.Child2Sequence == myChild2Sequence)
                                       select a).ToList();

            if (entityList.Count > 0) //Bug 23715: שליחת הצהרה- מתקבלת שגיאה שקשורה לאישורים
                                      //if (entityList.Count != null)
            {
                //Go over all the 'Entity'
                foreach (var entity in entityList)
                {
                    if (entity.FieldErrors != null)
                    {
                        //Go over all the 'FieldErrors'
                        foreach (var fieldErrors in entity.FieldErrors)
                        {
                            //Get all 'FieldErrors' for the 'FieldError'
                            List<field> fieldList = (from a in entity.FieldErrors
                                                     where (a.Code == "14026360" && a.Fieldcode == "ClassificationCode")
                                                     select a).ToList();
                            //if (fieldList.Count != null)
                            if (fieldList.Count > 0)// Bug 23715: שליחת הצהרה- מתקבלת שגיאה שקשורה לאישורים
                            {
                                //Go over all the 'FieldErrors'
                                foreach (var field in fieldList)
                                {
                                    var messageError = field.MessageError;
                                    messageError = messageError.Substring(messageError.IndexOf("#") + 1, messageError.LastIndexOf("#") - messageError.IndexOf("#") - 1);

                                    string[] codes = messageError.Split(new string[] { ";" }, StringSplitOptions.None);
                                    for (int i = 0; i < codes.Length; i++)
                                    {
                                        var code = codes[i];
                                        var charList = new List<char>(); // moran 19.10.16 - Bug 23715 - update handle -->
                                        charList.Add(' ');
                                        charList.Add(',');
                                        code = code.Trim(charList.ToArray());
                                        if (!string.IsNullOrWhiteSpace(code))
                                        {
                                            if (code.Length <= 4)//&& code.Length==3 Bug 23715: שליחת הצהרה- מתקבלת שגיאה שקשורה לאישורים
                                            {
                                                var confirmationType = new ConfirmationTypeRepository(_MyDeclarationPM.Tenant);
                                                var myConfirmationType = confirmationType.GetSingle(code);
                                                if (myConfirmationType != null)
                                                {
                                                    certificateCodeListFromErrosXml.Add(code);
                                                }
                                                else
                                                {
                                                    LogMessagingUtil.Instance.AppendLine("Certificate " + code + " does not exist in DB.");
                                                }
                                            }
                                            else
                                            {
                                                LogMessagingUtil.Instance.AppendLine("Certificate " + code + " is too large.");
                                            }
                                        } // moran 19.10.16 - Bug 23715 - update handle <--
                                    }
                                }
                            }
                        }
                    }
                }
                return certificateCodeListFromErrosXml;
            }
            else
            {
                return null;
            }
        }


        //private List<SupplierInvoiceItemModVehiclePM> GetSupplierInvoiceItemsModVehicles(DeclarationGoodsShipmentGovernmentAgencyGoodsItemDMExtensionsVehicleValuationAdjustment[] declarationGoodsShipmentGovernmentAgencyGoodsItemDMExtensionsVehicleValuationAdjustment, SupplierInvoiceItemPM supplierInvoiceItemPM)
        //{// moran 24.11.15 - Task 17424
        //    // moran 6.12.15 - Task 19037 -->
        //    //var supplierInvoiceItemModVehiclePMList = new List<SupplierInvoiceItemModVehiclePM>(supplierInvoiceItemPM.SupplierInvoiceItemModVehicles);
        //    var supplierInvoiceItemModVehiclePMList = new List<SupplierInvoiceItemModVehiclePM>();
        //    // moran 6.12.15 - Task 19037 <--
        //    if (declarationGoodsShipmentGovernmentAgencyGoodsItemDMExtensionsVehicleValuationAdjustment == null)
        //    {
        //        return null;
        //    }

        //    foreach (var valuationAdjustmentItem in declarationGoodsShipmentGovernmentAgencyGoodsItemDMExtensionsVehicleValuationAdjustment)
        //    {

        //        //var supplierInvoiceItemModVehiclePM = supplierInvoiceItemPM.SupplierInvoiceItemModVehicles.FirstOrDefault(si => si.AdjustmentTypeCode == valuationAdjustmentItem.AdjustmentType.Value);
        //        //if (supplierInvoiceItemModVehiclePM != null)
        //        //{
        //        //If exist update
        //        //  supplierInvoiceItemModVehiclePM.ChangeSetOp = ChangeSetOperation.Update;
        //        //}
        //        //else
        //        //{
        //        //Else create new SupplierInvoiceModification record
        //        // supplierInvoiceItemModVehiclePM = new SupplierInvoiceItemModVehiclePM();
        //        SupplierInvoiceItemModVehiclePM supplierInvoiceItemModVehiclePM = new SupplierInvoiceItemModVehiclePM();
        //        supplierInvoiceItemModVehiclePM.ChangeSetOp = ChangeSetOperation.Insert;
        //        //}
        //        supplierInvoiceItemModVehiclePM.Tenant = this._MyDeclarationPM.Tenant;
        //        supplierInvoiceItemModVehiclePM.AdjustmentTypeCode = valuationAdjustmentItem.AdjustmentType.Value;
        //        supplierInvoiceItemModVehiclePM.DeductAmount = valuationAdjustmentItem.DeductAmount.Value;

        //        supplierInvoiceItemModVehiclePMList.Add(supplierInvoiceItemModVehiclePM);
        //    }

        //    return supplierInvoiceItemModVehiclePMList;
        //}


#if refreshWSDL20141230
        // moran 21.11.13 - Bug 2083 - add GetSupplierInvoiceItemsTaxesModifications
        private List<SupplierInvoiceItemsTaxesModificationPM> GetSupplierInvoiceItemsTaxesModifications(DeclarationGoodsShipmentGovernmentAgencyGoodsItemCommodityDMExtensionsValuationDeductionAdjustment[] declarationGoodsShipmentGovernmentAgencyGoodsItemCommodityDMExtensionsValuationDeductionAdjustment, SupplierInvoiceItemsTaxPM supplierInvoiceItemsTaxPM)
        {
            var supplierInvoiceItemsTaxesModificationPMList = new List<SupplierInvoiceItemsTaxesModificationPM>();
            if (declarationGoodsShipmentGovernmentAgencyGoodsItemCommodityDMExtensionsValuationDeductionAdjustment == null)
            {
                return null;
            }

            foreach (var valuationDeductionAdjustment in declarationGoodsShipmentGovernmentAgencyGoodsItemCommodityDMExtensionsValuationDeductionAdjustment)
            {

                var supplierInvoiceItemsTaxesModificationPM = supplierInvoiceItemsTaxPM.SupplierInvoiceItemsTaxesModifications.FirstOrDefault(si => si.TypeCode == valuationDeductionAdjustment.ChargesTypeCode.Value);
                if (supplierInvoiceItemsTaxesModificationPM != null)
                {
                    //If exist update
                    supplierInvoiceItemsTaxesModificationPM.ChangeSetOp = ChangeSetOperation.Update;
                }
                else
                {
                    //Else create new SupplierInvoiceModification record
                    supplierInvoiceItemsTaxesModificationPM = new SupplierInvoiceItemsTaxesModificationPM();
                    supplierInvoiceItemsTaxesModificationPM.ChangeSetOp = ChangeSetOperation.Insert;
                }
                supplierInvoiceItemsTaxesModificationPM.TypeCode = valuationDeductionAdjustment.ChargesTypeCode.Value;
                supplierInvoiceItemsTaxesModificationPM.CurrencyTypeCode = valuationDeductionAdjustment.DeductAmount.currencyID.ToString();
                supplierInvoiceItemsTaxesModificationPM.Amount = valuationDeductionAdjustment.DeductAmount.Value;
                supplierInvoiceItemsTaxesModificationPM.LineNumber = supplierInvoiceItemsTaxPM.LineNumber;
                supplierInvoiceItemsTaxesModificationPM.TaxTypeCode = supplierInvoiceItemsTaxPM.TaxTypeCode;
                supplierInvoiceItemsTaxesModificationPM.Tenant = supplierInvoiceItemsTaxPM.Tenant;
                supplierInvoiceItemsTaxesModificationPM.DeclarationId = supplierInvoiceItemsTaxPM.DeclarationId;
                supplierInvoiceItemsTaxesModificationPMList.Add(supplierInvoiceItemsTaxesModificationPM);
            }

            return supplierInvoiceItemsTaxesModificationPMList;
        }

        
#endif

        // moran 6.10.15 - Task 17209 -->
        //private List<SupplierInvoiceItemVehiclePM> GetSupplierInvoiceItemsVehicles(DeclarationGoodsShipmentGovernmentAgencyGoodsItemDMExtensionsProductIdentification[] declarationGoodsShipmentGovernmentAgencyGoodsItemDMExtensionsProductIdentification, SupplierInvoiceItemPM supplierInvoiceItemPM)
        //{
        //    //var supplierInvoiceItemVehiclePMList = new List<SupplierInvoiceItemVehiclePM>(supplierInvoiceItemPM.SupplierInvoiceItemVehicles);
        //    var supplierInvoiceItemVehiclePMList = new List<SupplierInvoiceItemVehiclePM>();

        //    if (declarationGoodsShipmentGovernmentAgencyGoodsItemDMExtensionsProductIdentification == null || declarationGoodsShipmentGovernmentAgencyGoodsItemDMExtensionsProductIdentification.Count() < 1)
        //    {
        //        return null;
        //    }

        //    foreach (var vehicle in declarationGoodsShipmentGovernmentAgencyGoodsItemDMExtensionsProductIdentification)
        //    {

        //        var supplierInvoiceItemVehiclePM = supplierInvoiceItemPM.SupplierInvoiceItemVehicles.FirstOrDefault(si => si.VehicleTypeCode == vehicle.IDTypeCode.Value && (si.RichbitFileNumber == vehicle.ID.Value || si.VehicleChassisNumber == vehicle.ID.Value));
        //        if (supplierInvoiceItemVehiclePM == null)
        //        {

        //            throw new System.Exception(
        //               "unable to find the supplierInvoiceItemVehiclePM from vehicle ID " + vehicle.ID.Value + " and vehicle type " + vehicle.IDTypeCode.Value);
        //        }


        //        supplierInvoiceItemVehiclePM.ChangeSetOp = ChangeSetOperation.Update;

        //        supplierInvoiceItemVehiclePM.SupplierInvoiceItemVehicleMods = GetSupplierInvoiceItemsVehicleMods(vehicle.VehicleValuationAdjustment, supplierInvoiceItemVehiclePM);
        //        // moran 21.3.16 - Task 20132 -->
        //        supplierInvoiceItemVehiclePM.SupplierInvoiceItemVehicleAdds = GetSupplierInvoiceItemVehicleAdds(vehicle, supplierInvoiceItemVehiclePM, supplierInvoiceItemPM);
        //        // moran 21.3.16 - Task 20132 <--
        //        supplierInvoiceItemVehiclePMList.Add(supplierInvoiceItemVehiclePM); 
        //    }

        //    return supplierInvoiceItemVehiclePMList;
        //} // moran 6.10.15 - Task 17209 <--

        private List<SupplierInvoiceItemVehiclePM> GetSupplierInvoiceItemsVehicles(DeclarationGoodsShipmentGovernmentAgencyGoodsItemDMExtensionsProductIdentification[] declarationGoodsShipmentGovernmentAgencyGoodsItemDMExtensionsProductIdentification, SupplierInvoiceItemPM supplierInvoiceItemPM)
        {
            //var supplierInvoiceItemVehiclePMList = new List<SupplierInvoiceItemVehiclePM>(supplierInvoiceItemPM.SupplierInvoiceItemVehicles);
            var supplierInvoiceItemVehiclePMList = new List<SupplierInvoiceItemVehiclePM>();

            DeclarationGoodsShipmentGovernmentAgencyGoodsItemDMExtensionsProductIdentification vehicle = null;
            foreach (var supplierInvoiceItemVehiclePM in supplierInvoiceItemPM.SupplierInvoiceItemVehicles)
            {
                if (declarationGoodsShipmentGovernmentAgencyGoodsItemDMExtensionsProductIdentification == null || declarationGoodsShipmentGovernmentAgencyGoodsItemDMExtensionsProductIdentification.Count() < 1)
                {
                    vehicle = null;
                }
                else
                {
                    vehicle = declarationGoodsShipmentGovernmentAgencyGoodsItemDMExtensionsProductIdentification.FirstOrDefault(v => v.IDTypeCode.Value == supplierInvoiceItemVehiclePM.VehicleTypeCode && (v.ID.Value == supplierInvoiceItemVehiclePM.RichbitFileNumber || v.ID.Value == supplierInvoiceItemVehiclePM.VehicleChassisNumber));
                }
                supplierInvoiceItemVehiclePM.ChangeSetOp = ChangeSetOperation.Update;

                //if (vehicle == null)
                //{
                //    supplierInvoiceItemVehiclePM.SupplierInvoiceItemVehicleMods = GetSupplierInvoiceItemsVehicleMods(null, supplierInvoiceItemVehiclePM);
                //}
                //else
                //{
                //    supplierInvoiceItemVehiclePM.SupplierInvoiceItemVehicleMods = GetSupplierInvoiceItemsVehicleMods(vehicle.VehicleValuationAdjustment, supplierInvoiceItemVehiclePM);
                //}

                supplierInvoiceItemVehiclePMList.Add(supplierInvoiceItemVehiclePM);
            }


            vehicle = null;
            foreach (var supplierInvoiceItemVehiclePM in supplierInvoiceItemVehiclePMList)
            {
                if (declarationGoodsShipmentGovernmentAgencyGoodsItemDMExtensionsProductIdentification == null || declarationGoodsShipmentGovernmentAgencyGoodsItemDMExtensionsProductIdentification.Count() < 1)
                {
                    vehicle = null;
                }
                else
                {
                    vehicle = declarationGoodsShipmentGovernmentAgencyGoodsItemDMExtensionsProductIdentification.FirstOrDefault(v => v.IDTypeCode.Value == supplierInvoiceItemVehiclePM.VehicleTypeCode && (v.ID.Value == supplierInvoiceItemVehiclePM.RichbitFileNumber || v.ID.Value == supplierInvoiceItemVehiclePM.VehicleChassisNumber));
                }
                supplierInvoiceItemVehiclePM.ChangeSetOp = ChangeSetOperation.Update;

                supplierInvoiceItemVehiclePM.SupplierInvoiceItemVehicleAdds = GetSupplierInvoiceItemVehicleAdds(vehicle, supplierInvoiceItemVehiclePM, supplierInvoiceItemPM, supplierInvoiceItemVehiclePMList);

            }

            return supplierInvoiceItemVehiclePMList;
        }


        //private List<SupplierInvoiceItemVehicleModPM> GetSupplierInvoiceItemsVehicleMods(DeclarationGoodsShipmentGovernmentAgencyGoodsItemDMExtensionsProductIdentificationVehicleValuationAdjustment[] declarationGoodsShipmentGovernmentAgencyGoodsItemDMExtensionsProductIdentificationVehicleValuationAdjustment, SupplierInvoiceItemVehiclePM supplierInvoiceItemVehiclePM)
        //{ // moran 6.10.15 - Task 17209 -->
        //    //var supplierInvoiceItemVehicleModPMList = new List<SupplierInvoiceItemVehicleModPM>(supplierInvoiceItemVehiclePM.SupplierInvoiceItemVehicleMods);
        //    var supplierInvoiceItemVehicleModPMList = new List<SupplierInvoiceItemVehicleModPM>();

        //    if (declarationGoodsShipmentGovernmentAgencyGoodsItemDMExtensionsProductIdentificationVehicleValuationAdjustment == null || declarationGoodsShipmentGovernmentAgencyGoodsItemDMExtensionsProductIdentificationVehicleValuationAdjustment.Count() < 1)
        //    {
        //        return null;
        //    }

        //    foreach (var vehicleMod in declarationGoodsShipmentGovernmentAgencyGoodsItemDMExtensionsProductIdentificationVehicleValuationAdjustment)
        //    {

        //        //var supplierInvoiceItemVehicleModPM = supplierInvoiceItemVehiclePM.SupplierInvoiceItemVehicleMods.FirstOrDefault(si => si.AdjustmentTypeCode == vehicleMod.AdjustmentType.Value);

        //        //if (supplierInvoiceItemVehicleModPM != null)
        //        //{
        //        //If exist update
        //        //    supplierInvoiceItemVehicleModPM.ChangeSetOp = ChangeSetOperation.Update;
        //        //}
        //        //else
        //        //{
        //        //Else create new  record
        //        SupplierInvoiceItemVehicleModPM supplierInvoiceItemVehicleModPM = new SupplierInvoiceItemVehicleModPM();
        //        supplierInvoiceItemVehicleModPM.ChangeSetOp = ChangeSetOperation.Insert;
        //        //}

        //        supplierInvoiceItemVehicleModPM.Tenant = this._MyDeclarationPM.Tenant;
        //        supplierInvoiceItemVehicleModPM.AdjustmentTypeCode = vehicleMod.AdjustmentType.Value;
        //        supplierInvoiceItemVehicleModPM.DeductAmount = vehicleMod.DeductAmount.Value;

        //        supplierInvoiceItemVehicleModPMList.Add(supplierInvoiceItemVehicleModPM);
        //    }

        //    return supplierInvoiceItemVehicleModPMList;
        //} // moran 6.10.15 - Task 17209 <--

        private List<SupplierInvoiceItemVehicleAddPM> GetSupplierInvoiceItemVehicleAdds(DeclarationGoodsShipmentGovernmentAgencyGoodsItemDMExtensionsProductIdentification vehicle, SupplierInvoiceItemVehiclePM supplierInvoiceItemVehiclePM, SupplierInvoiceItemPM supplierInvoiceItemPM, List<SupplierInvoiceItemVehiclePM> supplierInvoiceItemVehiclePMList = null)
        { // moran 21.3.16 - Task 20132 

            var supplierInvoiceItemVehicleAddPM = supplierInvoiceItemVehiclePM.SupplierInvoiceItemVehicleAdds.FirstOrDefault(si => si.DeclarationId == supplierInvoiceItemVehiclePM.DeclarationId && si.InvoiceItemLineNumber == supplierInvoiceItemVehiclePM.InvoiceItemLineNumber && si.LineNumber == supplierInvoiceItemVehiclePM.LineNumber);
            if (supplierInvoiceItemVehicleAddPM == null)
            {
                return null; // moran 24.4.16 - Task 21126
                throw new System.Exception(
                   "unable to find the supplierInvoiceItemVehicleAddPM from Declaration Id " + supplierInvoiceItemVehiclePM.DeclarationId + " and Supplier Invoice Item Line " + supplierInvoiceItemVehiclePM.InvoiceItemLineNumber + " and Supplier Invoice Item Vehicle Line " + supplierInvoiceItemVehiclePM.LineNumber);
            }


            supplierInvoiceItemVehicleAddPM.ChangeSetOp = ChangeSetOperation.Update;

            var supplierInvoiceItemVehicleAddPMList = new List<SupplierInvoiceItemVehicleAddPM>();

            decimal? allDeduction = 0;
            decimal? chassisDeduction = 0;

            if (supplierInvoiceItemVehiclePMList != null && supplierInvoiceItemVehiclePMList.Count() > 0)
            {
                foreach (var vehicleMod in supplierInvoiceItemVehiclePMList)
                {
                    foreach (var Deduction in vehicleMod.SupplierInvoiceItemVehicleMods)
                    {
                        if ((!string.IsNullOrWhiteSpace(vehicleMod.RichbitFileNumber) && vehicleMod.RichbitFileNumber == supplierInvoiceItemVehiclePM.RichbitFileNumber) || (!string.IsNullOrWhiteSpace(vehicleMod.VehicleChassisNumber) && vehicleMod.VehicleChassisNumber == supplierInvoiceItemVehiclePM.VehicleChassisNumber))
                        {
                            chassisDeduction += Deduction.DeductAmount;
                        }

                        allDeduction += Deduction.DeductAmount;

                    }
                }
            }

            supplierInvoiceItemVehicleAddPM.ChassisPurchaseTax = ((allDeduction + purchase) / supplierInvoiceItemPM.ItemPrice) * supplierInvoiceItemVehicleAddPM.VehicleValue - chassisDeduction;
            supplierInvoiceItemVehicleAddPM.ChassisTax = (generalTax / supplierInvoiceItemPM.ItemPrice) * supplierInvoiceItemVehicleAddPM.VehicleValue;
            supplierInvoiceItemVehicleAddPM.ChassisVat = (vat / supplierInvoiceItemPM.ItemPrice) * supplierInvoiceItemVehicleAddPM.VehicleValue;

            totGeneralTaxCalc += supplierInvoiceItemVehicleAddPM.ChassisTax;
            totPurchaseCalc += supplierInvoiceItemVehicleAddPM.ChassisPurchaseTax;
            totVatCalc += supplierInvoiceItemVehicleAddPM.ChassisVat;

            supplierInvoiceItemVehicleAddPMList.Add(supplierInvoiceItemVehicleAddPM);

            return supplierInvoiceItemVehicleAddPMList;
        }

        //private List<SupplierInvoiceItemsModPM> GetSupplierInvoiceItemsModifications(DeclarationGoodsShipmentGovernmentAgencyGoodsItemValuationAdjustment[] declarationGoodsShipmentGovernmentAgencyGoodsItemValuationAdjustment, SupplierInvoiceItemPM supplierInvoiceItemsPM)
        //{
        //    // moran 26.5.15 - 13564 -->
        //    //var supplierInvoiceItemsModificationPMList = new List<SupplierInvoiceItemsModificationPM>();
        //    var supplierInvoiceItemsModificationPMList = new List<SupplierInvoiceItemsModPM>(supplierInvoiceItemsPM.SupplierInvoiceItemsMods);
        //    // moran 26.5.15 - 13564 <--
        //    if (declarationGoodsShipmentGovernmentAgencyGoodsItemValuationAdjustment == null)
        //    {
        //        return null;
        //    }

        //    foreach (var valuationAdjustmentItem in declarationGoodsShipmentGovernmentAgencyGoodsItemValuationAdjustment)
        //    {

        //        var supplierInvoiceItemsModificationPM = supplierInvoiceItemsPM.SupplierInvoiceItemsMods.FirstOrDefault(si => si.TypeCode == valuationAdjustmentItem.AdditionCode.Value);
        //        if (supplierInvoiceItemsModificationPM != null)
        //        {
        //            //If exist update
        //            supplierInvoiceItemsModificationPM.ChangeSetOp = ChangeSetOperation.Update;
        //        }
        //        else
        //        {
        //            //Else create new SupplierInvoiceModification record
        //            supplierInvoiceItemsModificationPM = new SupplierInvoiceItemsModPM();
        //            supplierInvoiceItemsModificationPM.ChangeSetOp = ChangeSetOperation.Insert;
        //        }
        //        supplierInvoiceItemsModificationPM.TypeCode = valuationAdjustmentItem.AdditionCode.Value;
        //        supplierInvoiceItemsModificationPM.CurrencyTypeCode = valuationAdjustmentItem.AmountAmount.currencyID.ToString();
        //        supplierInvoiceItemsModificationPM.Amount = valuationAdjustmentItem.AmountAmount.Value;

        //        supplierInvoiceItemsModificationPMList.Add(supplierInvoiceItemsModificationPM);
        //    }

        //    return supplierInvoiceItemsModificationPMList;
        //}


        private void AddSupplierInvoiceItemsTaxesModificationPM(
            SupplierInvoiceItemsTaxPM supplierInvoiceItemsTaxPM,
            DeclarationGoodsShipmentGovernmentAgencyGoodsItemCommodity declarationGoodsShipmentGovernmentAgencyGoodsItemCommodity)
        {

            // return;
            //var currPM = //supplierInvoiceItemsTax.SupplierInvoiceItemsTaxesModifications[0] ;
            //        new SupplierInvoiceItemsTaxesModPM()
            //        {

            //            DeclarationId = supplierInvoiceItemsTaxPM.DeclarationId,

            //            InvoiceCounterKey = supplierInvoiceItemsTaxPM.InvoiceCounterKey,
            //            ChangeSetOp = ChangeSetOperation.Insert,
            //            Tenant = supplierInvoiceItemsTaxPM.Tenant,

            //            TaxTypeCode = supplierInvoiceItemsTaxPM.TaxTypeCode,
            //            TypeCode = "1",
            //            Amount = 0,
            //            LineNumber = 1,
            //            CurrencyTypeCode = "1"

            //        };
#if refreshWSDL20141230
            var valuationDeductionAdjustment = declarationGoodsShipmentGovernmentAgencyGoodsItemCommodity.DMExtensions.ValuationDeductionAdjustment.FirstOrDefault();
            if (valuationDeductionAdjustment != null)
            {
                currPM.Amount= valuationDeductionAdjustment.DeductAmount.Value ;
                currPM.CurrencyTypeCode = valuationDeductionAdjustment.DeductAmount.currencyID.ToString() ;//??
                currPM.LineNumber = 1;//??

                currPM.TypeCode = valuationDeductionAdjustment.ChargesTypeCode.Value;


            }

            
#endif
            //this._SupplierInvoiceItemsTaxesModificationPMList.Add(currPM);

            //declarationGoodsShipmentGovernmentAgencyGoodsItemCommodity.DMExtensions.DutyRegimeCode 
            //declarationGoodsShipmentGovernmentAgencyGoodsItemCommodity.DMExtensions.ValuationDeductionAdjustment[0].
            //myList.

            ///throw new NotImplementedException();
        }


        private List<DeclarationTaxPM> GetDeclarationTaxesPM(DF_NG_2757_MSG10004_ExportDeclarationResponse customResponse)
        {
            var declarationTaxPMList = new List<DeclarationTaxPM>();

            if (customResponse.Response.Declaration.DutyTaxFee == null)
            {
                return declarationTaxPMList;
            }

            foreach (var dutyTaxFee in customResponse.Response.Declaration.DutyTaxFee)
            {
                var declarationTaxPM = new DeclarationTaxPM();
                declarationTaxPM.ChangeSetOp = ChangeSetOperation.Insert;
                declarationTaxPM.DeclarationId = this._MyDeclarationPM.Id;
                declarationTaxPM.Tenant = this._MyDeclarationPM.Tenant;
                declarationTaxPM.TaxTypeCode = dutyTaxFee.TypeCode.Value;
                declarationTaxPM.TotalAmount = dutyTaxFee.DMExtensions.CalculatedTax.Amount.Value;
                // declarationTaxPM.DeferredTaxAmount = dutyTaxFee.DMExtensions.CalculatedTax.DeferedTaxAmount.Value;
                declarationTaxPM.TaxBaseAmount = dutyTaxFee.AdValoremTaxBaseAmount.Value;

                declarationTaxPMList.Add(declarationTaxPM);
            }

            return declarationTaxPMList;
        }

        private void UpdateUnifreightEvent(string eventCode, string loggingUserId)
        {
            switch (eventCode)
            {
                case "MPOA":
                    RaiseUnifreightEvent("MPOA", "MPOA", "");
                    break;
                case "MID":
                    RaiseUnifreightEvent("MID", "MID", "");
                    break;
                case "IDE":
                    RaiseUnifreightEvent("IDE", "IDE", "");
                    break;
            }

        }

        private void RaiseUnifreightEvent(string eventCode, string unifrieghtEvent, string eventRemarks)
        {
            try
            {
                string loggingUserId = AuthenticationUtil.ResolveUserId(_MyDeclarationPM.Tenant);

                var myAmitalEventTracerModel = new Logitude.Customs.BL.TraceEvents.AmitalEventTracerModel()
                {

                    Tenant = _MyDeclarationPM.Tenant,
                    objectTableName = "Customs.Declaration",
                    EventCode = eventCode,
                    notes = eventRemarks,
                    CommunicationLoggingEntityReference = _MyDeclarationPM.DeclarationNumber,
                    EntityId = _MyDeclarationPM.Id,
                    UserId = loggingUserId,
                    CommunicationSubject = "Event from logitude",
                    MyUnifreightEventParam = new UnifreightEventParam()
                    {
                        Code = unifrieghtEvent,
                        Mode = UnifreightEventMode.@new,
                        EventDateTime = DateTime.Now,
                        Entname = "CFIFILEM",
                        PrimaryNum = _MyDeclarationPM.CustomFileNo,
                        EventRemarks = eventRemarks,
                    }
                };

                LogMessagingUtil.Instance.AppendLine("AmitalEventTracer.CreateTraceEvent: eventCode = " + eventCode + " CustomFileNo= " + _MyDeclarationPM.CustomFileNo + "   ");
                AmitalEventTracer.CreateTraceEvent(myAmitalEventTracerModel, true);

            }
            catch (System.Exception)
            {
                // TODO: BL Stop Execute or Cuntinue - Ask IHAB
                throw;
            }
        }

        public static void RaiseStatus(DeclarationPM dirtyDeclarationPM, string loggingUserId, string statusId, string xmlStatus)
        {

            try
            {
                if (string.IsNullOrWhiteSpace(loggingUserId)) loggingUserId = AuthenticationUtil.ResolveUserId(dirtyDeclarationPM.Tenant);
                var myAmitalEventTracerModel = new Logitude.Customs.BL.TraceEvents.AmitalEventTracerModel()
                {
                    Tenant = dirtyDeclarationPM.Tenant,
                    objectTableName = "Customs.Declaration",
                    EventCode = statusId,
                    notes = "DO_NOT_RAISE_EVENT",
                    CommunicationLoggingEntityReference = dirtyDeclarationPM.DeclarationNumber,
                    EntityId = dirtyDeclarationPM.Id,
                    UserId = loggingUserId,

                    CommunicationSubject = "FU Status " + statusId + " from Logitude",
                    MyFUStatus = new AmitalEventTracerModel.FUStatus()
                    {
                        entname = "CFIFILEM",
                        primary_number = dirtyDeclarationPM.CustomFileNo,
                        status = "new",
                        xml_status = xmlStatus,
                        status_id = statusId,
                        status_DateTime = DateTime.Now,
                        //status_place = "",
                        //status_save = "no_fail",
                        comments = "",
                    }
                };
                if (!dirtyDeclarationPM.IsConnectedToUnifreight) myAmitalEventTracerModel.NotConnectedToUniface = true;

                LogMessagingUtil.Instance.AppendLine("AmitalEventTracer.CreateTraceEvent Status " + statusId + "  CustomFileNo = " + dirtyDeclarationPM.CustomFileNo + "   ");
                AmitalEventTracer.CreateTraceEvent(myAmitalEventTracerModel);

            }
            catch (System.Exception)
            {
                // TODO: BL Stop Execute or Cuntinue - Ask IHAB
                throw;
            }
        }


        private string GetExceptionMsg(UnifreightIIG.Common.ExportDeclarationServiceReference.Exception ex)
        {
            List<string> fieldNames = new List<string>();
            WCOTypeEnum wCOTypeEnum = _MyDeclarationPM?.Direction == "E" ? WCOTypeEnum.WCO_EX : WCOTypeEnum.WCO;
            var fieldList = WCO.Instance.CreateDB(wCOTypeEnum).GetCopyList();
            WCOErrorPointerModel res;

            if (ex.ExceptionParms != null)
            {
                ex.ExceptionParms.ToList().ForEach(param =>
                {
                    res = GetErrorField(fieldList, param);

                    if (res != null)
                        fieldNames.Add(res.FieldNameHeb);
                });
            }

            string msg = fieldNames.Count > 0 ? "שגיאה בשדה: " + string.Join(",", fieldNames) : ex.ExeptionDescription;

            return msg;
        }

        private WCOErrorPointerModel GetErrorField(List<WCOErrorPointerModel> fieldList, string fullParam)
        {
            WCOErrorPointerModel res;

            if (fullParam.Contains(":"))
                return null;

            string param = fullParam.Substring(fullParam.LastIndexOf(".") + 1);

            res = fieldList.Find(x => x.XmlTag.EndsWith(param) && !string.IsNullOrEmpty(x.FieldNameHeb));
            if (res == null && param.StartsWith("Export"))
                res = fieldList.Find(x => x.XmlTag.EndsWith(param.Remove(0, 6)) && !string.IsNullOrEmpty(x.FieldNameHeb));

            if (res == null && fullParam.Contains("."))
            {
                fullParam = fullParam.Substring(0, fullParam.LastIndexOf("."));
                if (fullParam != null)
                    return GetErrorField(fieldList, fullParam);
            }

            return res;
        }

        private static void RaiseEvent(DeclarationPM dirtyDeclarationPM, string loggingUserId, string status_id, string versionId, DateTime? status_DateTime, string comments = null)
        {
            //primary_number = $"{dirtyDeclarationPM.CustomFileNo},{dirtyDeclarationPM.TransportModeId == "A" ? "EFIFILEM" : "MFIFILEM" }",
            string primary_number = $"{dirtyDeclarationPM.CustomFileNo},EFIFILEM";
            if (dirtyDeclarationPM.TransportModeId != "A")
            {
                primary_number = $"{dirtyDeclarationPM.CustomFileNo},MFIFILEM";
            }

            var myAmitalEventTracerModel = new Logitude.Customs.BL.TraceEvents.AmitalEventTracerModel()
            {
                Tenant = dirtyDeclarationPM.Tenant,
                objectTableName = "Customs.Declaration",
                EventCode = status_id,
                notes = "",
                CommunicationLoggingEntityReference = dirtyDeclarationPM.DeclarationNumber,
                EntityId = dirtyDeclarationPM.Id,
                UserId = loggingUserId,

                CommunicationSubject = "FU Status " + status_id + " from logitude",
                MyFUStatus = new AmitalEventTracerModel.FUStatus()
                {
                    entname = dirtyDeclarationPM.Direction == "E" ? "BFIFILE" : "CFIFILEM",
                    primary_number = primary_number,
                    status = "new",
                    xml_status = "new",
                    status_id = status_id,
                    status_DateTime = status_DateTime ?? DateTime.Now,
                    comments = !string.IsNullOrEmpty(comments) ? comments: dirtyDeclarationPM.DeclarationNumber+", גירסה" + versionId,
                }
            };

            AmitalEventTracer.CreateTraceEvent(myAmitalEventTracerModel, suppress_RAISE_EVENT: true, iscustomUser: true);


        }

        public bool SendDeclarationPrint(GenericRequestParams requestParams)
        {
            LogMessagingUtil.Instance.AppendLine("SendDeclarationPrint");
            string decNum =_MyDeclarationPM.DeclarationNumber;
            var decNumList = new List<string>();
            decNumList.Add(decNum);

            string LoggingUserId = requestParams.LoggingUserId;
            ICommonDataContext commonDbContext = CommonDataContext.GetContext(requestParams.Tenant);
            UserRepository userRepository = new UserRepository(commonDbContext);
            var user = userRepository.GetSingleUserByCode("MEHES", requestParams.Tenant, true);
            if (user != null)
            {
                LoggingUserId = user.Id;
            }

            DF_NG_8302_Web03_DeclarationPrintRequestParams searchParams = new DF_NG_8302_Web03_DeclarationPrintRequestParams()
            {
                LoggingEnabled = true,
                CustomFileNo = this._MyDeclarationPM.CustomFileNo,
                DeclarationNumber = decNumList, //declarationPM.DeclarationNumber,
                Tenant = this._MyDeclarationPM.Tenant,
                RequestName = "Declaration Print(8373)",
                ResponseName = "Declaration Print(8373)",
                LoggingEntityId =  _MyDeclarationPM.Id,
                RequestVIA = SendRequestVIA.WebServiceBatch,


                LoggingUserId = LoggingUserId //requestParams.LoggingUserId ,//HD CALL#298426
            };

            var myRequestMessagingService = new DF_NG_8302_Web03_DeclarationPrintMessagingService();
            var resData = myRequestMessagingService.Send(searchParams);
           
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
