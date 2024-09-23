using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.BL.Models;
using Logitude.Customs.Data;
using Logitude.CustomsMessaging.Helpers;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.Server.Tools.Helpers;
///using Logitude.CustomsMessaging.Utils;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.MessageLib.DeclarationDeal;
using Simplog.Data.InfrastructureModel.Repositories;
using Logitude.Server.Tools.Utils;
using System.Configuration;
using Logitude.AmitalMessaging.Utils;
using Logitude.Customs.BL.Messaging.Maman;
using Unifreight.BL.EntityQueryServices;
using Unifreight.Data.AmitalModel;
using Unifreight.BL.EntityPMs.UGenerated;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Logitude.Server.Tools.Contracts;
using Logitude.Server.Tools;
using Microsoft.Practices.Unity;
using Logitude.Customs.BL.TraceEvents;
using Simplog.Data.CommonDataModel;
using System.Data.Entity;
using Logitude.Customs.BL.NotificationBL;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.CustomsMessaging.MessagingServices;
using Logitude.Customs.BL.Messaging.Customs;
using Logitude.BL.CommonDataModel.EntityQueries;

namespace Logitude.CustomsMessaging.ResponseServices
{
    public class DF_NG_2470_DF_MSG16001_ReleaseGoodsMessageResponseService
        : ResponseServiceBase<ReleaseGoodsResponseData, DF_NG_2470_DF_MSG16001_ReleaseGoodsMessage, GenericRequestParams>
    {
        private bool _LockResponseService2470Feature;


        public override ReleaseGoodsResponseData GetResponse(DF_NG_2470_DF_MSG16001_ReleaseGoodsMessage customResponse, GenericRequestParams requestParams)
        {
            return this.MyResponseData;
        }

        public override void Update(DF_NG_2470_DF_MSG16001_ReleaseGoodsMessage customResponse, GenericRequestParams requestParams)
        {
            //Analyzing Message 2470 -Release Goods Message (Hatara)

            var declarationNumber = customResponse.GeneralData.declarationID;
            IDisposable disposableToken = null;
            try
            {
                _LockResponseService2470Feature = true;//ConfigurationManager.AppSettings["20180121.LockResponseService2470"] == "1";

                if (_LockResponseService2470Feature)
                {

                    string key = ProcessLockTableUtil.Instance.GetKey4Declaration(declarationNumber, requestParams.Tenant);
                    //using (disposableToken = ProcessLockUtil.Instance.InsertKey(key, "2470ResponseService.Update"))
                    disposableToken =
                        ///ProcessLockTableUtil.Instance.LockItAndGetReleaseToken(key, "2470ResponseService.Update");
                        ProcessLockTableUtil.Instance.GetProcessLockTableDisposable(requestParams.Tenant, true, key, "2470ResponseService.Update");
                }
                {

                    DateTime? hataraDate = null;
                    ICustomContext dbContext = CustomContext.GetContext(requestParams.Tenant);
                    DeclarationQueryService declarationQueryService = new DeclarationQueryService(requestParams.Tenant);
                    DeclarationUpdateService declarationUpdateService = new DeclarationUpdateService(dbContext, new Dictionary<string, IContext>(), requestParams.Tenant);


                    LogMessagingUtil.Instance.AppendLine("DeclarationNumber=" + declarationNumber);

                    this.MyRequestSheetParam = new RequestSheetParam();
                    MyRequestSheetParam.ObjectTableId1 = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
                    MyRequestSheetParam.RequestDescription = "התרה לתיק- מספר הצהרה: " + declarationNumber;

                    DeclarationPM declarationPM = declarationUpdateService.GetSertByConvertedDeclarationNumber(declarationNumber, requestParams.Tenant,true);
                    if (declarationPM == null || string.IsNullOrWhiteSpace(declarationPM.Id))
                    {
                        var errMess = "DeclarationPM not found: DeclarationNumber=" + declarationNumber;
                        this.MyResponseData = new ReleaseGoodsResponseData();
                        this.MyResponseData.Succeeded = true;
                        this.MyResponseData.HasException = true;
                        this.MyResponseData.UserMessage = errMess;
                        LogMessagingUtil.Instance.AppendLine(errMess);
                        return;
                    }

                    var myEventContextTagModel = new EventContextTagModel()
                    {
                        CallProccessID = EventContextTagModel.ProccessEnum.DF_NG_2470_DF_MSG16001_ReleaseGoodsMessageResponseServiceUpdate,
                    };

                    DateTime statusDateTime = customResponse.GeneralData.releaseDate.GetValueOrDefault();
                    if (statusDateTime == null)
                    {
                        statusDateTime = customResponse.RequestContentHeader.TransmitionDateTime;
                    }

                    if (declarationPM.Direction == "E") { 
                        declarationPM.ReleaseStatusTypeCode = customResponse.GeneralData?.ReleaseMessageCode.ToString();
                        if (declarationPM.IsSubmitDeclaration == false ||declarationPM.IsSubmitDeclaration==null)
                            declarationPM.IsSubmitDeclaration = true; 
                    }

                    var myCourierMasterQueryService = new CourierMasterQueryService(dbContext);
                    CourierMasterPM _CourierMasterPM = myCourierMasterQueryService.GetByDeclarationId(declarationPM.Id, requestParams.Tenant);
                    if (declarationPM.IsAmendment == true && declarationPM.AmendmentOriginalDeclartation != null && _CourierMasterPM == null)
                    {
                        _CourierMasterPM = myCourierMasterQueryService.GetByDeclarationId(declarationPM.AmendmentOriginalDeclartation, requestParams.Tenant);
                    }

                    if(declarationPM.IsCourierDeclaration && declarationPM.PaymentDate == null && customResponse.GeneralData.releaseDate != null)
                    {
                        declarationPM.PaymentDate = customResponse.GeneralData.releaseDate;
                    }

                    switch (customResponse.GeneralData.ReleaseMessageCode)
                    {
                        case 1: // released
                            LogMessagingUtil.Instance.AppendLine("released");
                            //hataraDate = customResponse.GeneralData.releaseDate;
                            declarationPM.HatraDate = customResponse.GeneralData.releaseDate.GetValueOrDefault(); //Yuval Chalup 17.01.2018 - Update date from response
                            myEventContextTagModel.EventCode = "RSG";
                            myEventContextTagModel.StatusDateTime = statusDateTime;
                            declarationPM.DeclarationStatusTypeCode = "7";

                            this.CloseCustomsCollateral(declarationPM);
                           
                            ICommonDataContext commonDbContext = CommonDataContext.GetContext(declarationPM.Tenant);
                            UserRepository userRepository = new UserRepository(commonDbContext);
                            var user = userRepository.GetSingleUserByCode("MEHES", declarationPM.Tenant, true);


                            var setting = CustomsSettingQueryService.GetSettingByTenant(declarationPM.Tenant);
                            
                            if (setting.IsConnectedToUniFreight || AmitalEventTracer.UseHybrid_When_NotIsConnectedToUniFreight)
                            {
                                if (declarationPM.Direction == "E")
                                {
                                    RaiseEvent(declarationPM, user?.Id, status_id: "HTR", status_DateTime: statusDateTime);
                                 
                                    SendSoyStatusToUnifreight(declarationPM, user?.Id);

                                }
                            }



                                if (declarationPM.IsCourierDeclaration)
                            {
                                // update NoOfCourierHawbwWithoutHatara
                                IUpdateOpenDeclarationInCourierMasterService myIUpdateOpenDeclarationInCourierMasterService = ContainerAccessor.Container.Resolve(typeof(IUpdateOpenDeclarationInCourierMasterService), "UpdateOpenDeclarationInCourierMasterService", new ParameterOverride("", declarationPM.Tenant)) as IUpdateOpenDeclarationInCourierMasterService;
                                myIUpdateOpenDeclarationInCourierMasterService.UpdateOpenDeclarationInCourierMaster(declarationPM.Tenant, _CourierMasterPM.Id, null);


                                declarationPM.CourierCustomStatusCode = "1";

                                string defValue = "";
                                if (_CourierMasterPM != null)
                                {
                                    Card myCard = null;
                                    var repository = new CardRepository(requestParams.Tenant);
                                    myCard = repository.GetSingleCard(_CourierMasterPM.IntegratorCode, requestParams.Tenant);
                                    if (!String.IsNullOrWhiteSpace(myCard.Code))
                                    {
                                        DefaultValueQueryService defaultValueQueryService = new DefaultValueQueryService(requestParams.Tenant);

                                        defValue = defaultValueQueryService.GetDefault("ISRAEL", "CGO_COURAWB_CLS", "NON", myCard.Code, requestParams.Tenant);
                                    }
                                }
                                if (defValue == "R" || String.IsNullOrWhiteSpace(defValue))
                                {

                                    LogMessagingUtil.Instance.AppendLine("Update DeclarationCourierStatusPM: IsClosedForFollowUp=true");
                                    DeclarationCourierStatusQueryService declarationCourierStatusQueryService = new DeclarationCourierStatusQueryService(requestParams.Tenant);
                                    DeclarationCourierStatusUpdateService declarationCourierStatusUpdateService = new DeclarationCourierStatusUpdateService(dbContext, new Dictionary<string, IContext>(), requestParams.Tenant);
                                    DeclarationCourierStatusPM declarationCourierStatusPM = declarationCourierStatusQueryService.GetSingle(declarationPM.Id, true, true);
                                    declarationCourierStatusPM.ChangeSetOp = ChangeSetOperation.Update;
                                    declarationCourierStatusPM.IsClosedForFollowUp = true;
                                    declarationCourierStatusUpdateService.Update(declarationCourierStatusPM, true);

                                }
                            }
                          if(declarationPM.Direction!="E")  declarationPM.IsClose = true;
                            MyRequestSheetParam.RequestDescription = "התרה לתיק. מספר הצהרה: " + declarationNumber;//eitan h 26/2/15 task 11525
                            break;
                        case 4 when declarationPM.Direction == "E":
                           
                                myEventContextTagModel.EventCode = "TAS";
                                myEventContextTagModel.StatusDateTime = statusDateTime;
                            break;


                        case 5: // released cancelled
                            LogMessagingUtil.Instance.AppendLine("released cancelled");
                            myEventContextTagModel.EventCode = "RSC";
                            myEventContextTagModel.StatusDateTime = statusDateTime;
                            declarationPM.DeclarationStatusTypeCode = "6";
                            declarationPM.HatraDate = null; //Yuval Chalup 17.01.2018 - Delete date
                            declarationPM.IsClose = false;
                            MyRequestSheetParam.RequestDescription = "ביטול התרה. תיק מספר: " + declarationPM.CustomFileNo;//eitan h 26/2/15 task 11525
                            break;
                        case 8 when declarationPM.Direction == "E":
                            
                                myEventContextTagModel.EventCode = "TAC";
                                myEventContextTagModel.StatusDateTime = statusDateTime;
                            break;

                        case 9: // Pre clearance
                            LogMessagingUtil.Instance.AppendLine("Pre clearence");
                            myEventContextTagModel.EventCode = "PRS";
                            myEventContextTagModel.StatusDateTime = statusDateTime;
                            //hataraDate = declarationPM.HatraDate; Yuval Chalup 17.01.2018 Remarked - Do NOT change date
                            MyRequestSheetParam.RequestDescription = "הודעה מוקדמת לסוכן מכס: " + declarationPM.CustomFileNo;
                            declarationPM.CourierCustomStatusCode = "1";
                            Send2470ToMaman(declarationPM, customResponse, requestParams);
                            break;
                        case 14: // Release When Arrived
                            LogMessagingUtil.Instance.AppendLine("Release When Arrived");
                            myEventContextTagModel.EventCode = "PRA";
                            myEventContextTagModel.StatusDateTime = statusDateTime;
                            MyRequestSheetParam.RequestDescription = "תיק מאושר להתרה לאחר הגשת טובין: " + declarationPM.CustomFileNo;
                            //hataraDate = declarationPM.HatraDate; Yuval Chalup 17.01.2018 Remarked - Do NOT change date
                            break;
                        default:
                            var errMess = "Undeveloped- ReleaseMessageCode=" + customResponse.GeneralData.ReleaseMessageCode;
                            this.MyResponseData = new ReleaseGoodsResponseData();
                            this.MyResponseData.Succeeded = true;
                            this.MyResponseData.HasException = true;
                            this.MyResponseData.UserMessage = errMess;
                            LogMessagingUtil.Instance.AppendLine(errMess);
                            return;                            
                    }


                    //declarationPM.HatraDate = hataraDate; - Yuval Chalup 17.01.2018 Remarked (Init in each case above)
                    LogMessagingUtil.Instance.AppendLine("declarationPM.HatraDate" + (declarationPM.HatraDate.HasValue ? declarationPM.HatraDate.Value.ToString() : "") + ",Time: " + DateTime.Now.ToString("hh:mm:ss.fff tt"));

                    declarationPM.CurrentContextTag = myEventContextTagModel;

                    declarationPM.ChangeSetOp = ChangeSetOperation.Update;
                    declarationUpdateService.Update(declarationPM, true);
                    LogMessagingUtil.Instance.AppendLine($"declarationUpdateService.Update(IsClose={declarationPM.IsClose},CourierCustomStatusCode ={declarationPM.CourierCustomStatusCode}),Time: "+ DateTime.Now.ToString("hh:mm:ss.fff tt"));

                    MyRequestSheetParam.EntityId1 = declarationPM.Id;
                    if (declarationPM.IsConvertedDeclaration)
                    {
                        MyRequestSheetParam.RequestDescription = string.Concat(MyRequestSheetParam.RequestDescription, "\n", declarationPM.UserNotes);
                    }
                    if (declarationPM.Direction == "E")
                    {
                        if (customResponse.GeneralData.ReleaseMessageCode == 4 || customResponse.GeneralData.ReleaseMessageCode == 8)
                        {
                            SendDeclarationStatusRequest(declarationPM);
                        }

                        
                    }
                    requestParams.LoggingEntityId = declarationPM.Id;
                    this.MyResponseData = new ReleaseGoodsResponseData()
                    {
                        Succeeded = true,
                        HasException = false,
                        DeclarationNumber = declarationPM.DeclarationNumber,
                        UserMessage = MyRequestSheetParam.RequestDescription,
                    };
                    GetResponseData(this.MyResponseData, customResponse, declarationPM);
                }
            }
            catch (ProcessLockException processLockException)
            {
                LogMessagingUtil.Instance.AppendLine("processLockException wait a minute!! ,the worker Role is proccesing anther response of the same Declaration  ");
                throw;
            }
            finally
            {
                if (disposableToken != null)
                {
                    disposableToken.Dispose();
                }
            }
        }

        private void SendSoyStatusToUnifreight(DeclarationPM declarationPM, string userId, DateTime? dateTime = null)
        {
            if (!declarationPM.AutoSending || !declarationPM.IsDiamondDeclaration) return;
            // determine if the export diamonds feature is enabled to allow autosending
            ICommonDataContext myContextCommon = CommonDataContext.GetContext(declarationPM.Tenant);
            FeatureRepository myFeatureRepository = new FeatureRepository(myContextCommon);
            FeatureQuery featureQuery = new FeatureQuery(myFeatureRepository);
            var features = featureQuery.GetAllowedFeaturesForLoggedUser(AuthenticationUtil.ResolveUserId(declarationPM.Tenant), declarationPM.Tenant);
            var featureExportDiamonds = features.Features.FirstOrDefault(x => x.Code == "ExportDiamonds");

            if (featureExportDiamonds != null)
            {
                string declarationStatus = "0";
                string declarationStatusLabel = "";
           
                    // get the declaration status label
                    DeclarationStatusTypeQueryService declarationStatusTypeQueryService = new DeclarationStatusTypeQueryService(declarationPM.Tenant);
                    DeclarationStatusTypePM declarationStatusType = declarationStatusTypeQueryService.GetSingle(declarationPM.DeclarationStatusTypeCode, false, true);
                    declarationStatusLabel = declarationStatusType?.LocalName ?? "לא ידוע";
                
                string statusSoyRemarks = $"CODE-{declarationStatus}-{declarationStatusLabel}-";

                if (!string.IsNullOrEmpty(declarationPM.DeclarationNumber))
                {
                    statusSoyRemarks += declarationPM.DeclarationNumber;
                }
              
 

                RaiseEvent(declarationPM, userId,
                    status_id: "SOY",
                    status_DateTime: declarationPM.HatraDate,
                    comments: statusSoyRemarks);
            }
        }


        private void Send2470ToMaman(DeclarationPM declarationPM, DF_NG_2470_DF_MSG16001_ReleaseGoodsMessage customResponse, GenericRequestParams requestParams)
        {
            LogMessagingUtil.Instance.AppendLine("הגדרת ברירת מחדל חדשה ביוניפרייט ברמת מערכת עמילות כפתור בלדרות: שליחה של מסר הודעה מוקדמת לממן עם אופציות .");

            var myGDFDATAQueryService = new GDFDATAQueryService(AmitalContext.GetContext(requestParams.Tenant));
            var def = myGDFDATAQueryService.GetSingle("ISRAEL", "CGO_2470", "NON", "NON", false, true);

            bool sendMaman2470 = def.DEFDATA /*DefaultValue*/ == "Y";


            LogMessagingUtil.Instance.AppendLine("default value CGO_2470 ==" + def.DEFDATA ?? "N");
            if (!sendMaman2470)
            {
                return;
            }
            var consignment1st = declarationPM.Consignments.FirstOrDefault() ?? new ConsignmentPM();
            LogMessagingUtil.Instance.AppendLine("2470 consignment1st.StorageSiteCode=" + consignment1st.StorageSiteCode ?? "none");
            if (consignment1st.StorageSiteCode != "ILMMN")
            {
                return;
            }
            LogMessagingUtil.Instance.AppendLine("send 2470 2 ILMMN");

            var customsResponseXml = XmlGenericUtil<DF_NG_2470_DF_MSG16001_ReleaseGoodsMessage>.SerializeObject(customResponse);
            var customsResponseBytes = System.Text.UTF8Encoding.UTF8.GetBytes(customsResponseXml);
            var myFTPOutMaman2470ReleaseGoodService = new FTPOutMaman2470ReleaseGoodService();
            myFTPOutMaman2470ReleaseGoodService
                .BuildCommunicationLog(customsResponseBytes, requestParams.Tenant, declarationPM.Id, requestParams.DCAFileName, false);

        }

        private void GetResponseData(ReleaseGoodsResponseData myResponseData, DF_NG_2470_DF_MSG16001_ReleaseGoodsMessage customResponse, DeclarationPM declarationPM)
        {
            if (declarationPM != null)
            {
                DeclarationQueryService declarationQueryService = new DeclarationQueryService(declarationPM.Tenant);
                DeclarationPM fullDeclarationPM = declarationQueryService.GetSingle(declarationPM.Id, true, false);
                MyResponseData.FileNumber = fullDeclarationPM.CustomFileNo;

                if (fullDeclarationPM.SupplierInvoices != null && fullDeclarationPM.SupplierInvoices.Count() > 0)
                {
                    MyResponseData.GoodsItemsList = new List<GoodsItems>();
                    foreach (var supplierInvoice in fullDeclarationPM.SupplierInvoices)
                    {
                        foreach (var supplierInvoiceItem in supplierInvoice.SupplierInvoiceItems.OrderBy(rec => rec.OrderByLineNo))
                        {
                            GoodsItems goodsItems = new GoodsItems();
                            goodsItems.SupplierInvoice = supplierInvoice.InvoiceNumber;
                            goodsItems.GoodsItemPath = supplierInvoiceItem.LineNumber.ToString();
                            goodsItems.CustomItemID = supplierInvoiceItem.ClassificationCode;
                            MyResponseData.GoodsItemsList.Add(goodsItems);
                        }
                        if (supplierInvoice.IsPrimarySupplierInvoice)
                        {
                            MyResponseData.CurrencyTypeCode = supplierInvoice.InvoiceCurrencyTypeCode;

                        }
                    }
                }
                if (declarationPM.TaxationDateTime.HasValue) MyResponseData.TaxationDate = declarationPM.TaxationDateTime.Value.Date.ToString("dd/MM/yyyy");
            }
            if (customResponse != null)
            {
                if (customResponse.GeneralData != null)
                {
                    MyResponseData.governmentProcedureType = customResponse.GeneralData.governmentProcedureType.ToString();
                    if (customResponse.GeneralData.releaseDate != null)
                    {
                        MyResponseData.releaseDate = customResponse.GeneralData.releaseDate.GetValueOrDefault().Date.ToString("dd/MM/yyyy");
                        if (customResponse.GeneralData.releaseDate.GetValueOrDefault().TimeOfDay.Hours != 0 || 
                            customResponse.GeneralData.releaseDate.GetValueOrDefault().TimeOfDay.Minutes != 0 ||
                            customResponse.GeneralData.releaseDate.GetValueOrDefault().TimeOfDay.Seconds != 0)
                        {
                            MyResponseData.releaseDate = customResponse.GeneralData.releaseDate.GetValueOrDefault()./*TimeOfDay.*/ToString("hh:mm") + "   " + MyResponseData.releaseDate;
                        }
                    }

                    if (customResponse.GeneralData.dealValueNISSpecified) MyResponseData.dealValueNIS = customResponse.GeneralData.dealValueNIS.ToString();
                    if (customResponse.GeneralData.CifValueNisSpecified) MyResponseData.CifValueNis = customResponse.GeneralData.CifValueNis.ToString();
                    if (customResponse.GeneralData.ExchangeRate > 0) MyResponseData.ExchangeRate = customResponse.GeneralData.ExchangeRate.ToString();
                }
                if (customResponse.Consignment != null && customResponse.Consignment[0] != null)
                {
                    MyResponseData.cargoDescription = customResponse.Consignment[0].cargoDescription;
                    MyResponseData.cargoIdentifierType = customResponse.Consignment[0].cargoIdentifier.cargoIdentifierType.ToString();
                    MyResponseData.cargoIdentifierKey1 = customResponse.Consignment[0].cargoIdentifier.cargoIdentifierKey1;
                    MyResponseData.cargoIdentifierKey2 = customResponse.Consignment[0].cargoIdentifier.cargoIdentifierKey2;
                }
                if (customResponse.Customers != null)
                {
                    if (customResponse.Customers.importerExpoterExternalIDSpecified) MyResponseData.importerExpoterExternalID = customResponse.Customers.importerExpoterExternalID.ToString();
                }
                if (customResponse.PackagesInDeliverySite != null && customResponse.PackagesInDeliverySite[0] != null)
                {
                    MyResponseData.packageType = customResponse.PackagesInDeliverySite[0].packageType;
                    MyResponseData.packageQuantity = customResponse.PackagesInDeliverySite[0].packageQuantity;
                    if (customResponse.PackagesInDeliverySite[0].packagesWeightSpecified) MyResponseData.packagesWeight = customResponse.PackagesInDeliverySite[0].packagesWeight.ToString();
                }
                if (customResponse.Sites != null)
                {
                    MyResponseData.loadingPort = customResponse.Sites.loadingSiteNumber;
                    MyResponseData.storageSiteNumber = customResponse.Sites.storingSiteNumber;
                    MyResponseData.unloadingSiteNumber = customResponse.Sites.unloadingSiteNumber;
                }

            }

        }
        private static void RaiseEvent(DeclarationPM dirtyDeclarationPM, string loggingUserId, string status_id, DateTime? status_DateTime, string  comments=null)
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
                    comments = !string.IsNullOrEmpty(comments)? comments:dirtyDeclarationPM.DeclarationNumber,





                }
            };

            AmitalEventTracer.CreateTraceEvent(myAmitalEventTracerModel, suppress_RAISE_EVENT: true, iscustomUser: true);



        }

        private void CloseCustomsCollateral(DeclarationPM dec)
        {
            ICustomContext dbContext = CustomContext.GetContext(dec.Tenant);
            var CustomsCollateralQueryService = new CustomsCollateralQueryService(dbContext);
            var DecList = new List<string>();
            DecList.Add(dec?.AmendmentOriginalDeclartation);
            DecList.Add(dec?.Id);
            var CollList = CustomsCollateralQueryService.GetDecCollListByOriginalDecId(DecList, dec.Tenant);
            var customsCollateralUpdateService = new CustomsCollateralUpdateService(dbContext, new Dictionary<string, IContext>(), dec.Tenant);

            if (CollList != null && CollList.Count > 0)
            {
                foreach(var Coll in CollList)
                {
                    if (!Coll.IsClosed)
                    {
                        Coll.IsClosed = true;
                        Coll.ChangeSetOp = ChangeSetOperation.Update;
                        customsCollateralUpdateService.Update(Coll,true);

                    }
                }
            }
        }

       

        void SendDeclarationStatusRequest(DeclarationPM myDeclarationPM)
        {
           

            var newSearchDeclarationStatusRequestParams = new DeclarationStatusRequestParams()
            {
                LoggingEnabled = true,
                CustomFileNo = myDeclarationPM.CustomFileNo,
                DeclarationNumber = myDeclarationPM.DeclarationNumber,
                Tenant = myDeclarationPM.Tenant,
                RequestName = "Declaration Status " + myDeclarationPM.DeclarationNumber,
                ResponseName = "Declaration Status " + myDeclarationPM.DeclarationNumber,
                RequestVIA = SendRequestVIA.WebServiceBatch,
                InterfaceTypeCode = "8250",
                LoggingEntityId = myDeclarationPM.Id,
                LoggingObjectTableId = ObjectTableRepository.GetObjectTableByName("Customs.Declaration"),
                LoggingUserId = AuthenticationUtil.ResolveUserId(myDeclarationPM.Tenant),
            };



            try
            {
                SBQMessageService.CreateSheetSBQMessage<Logitude.CustomsMessaging.Common.RequestParams.DeclarationStatusRequestParams>(newSearchDeclarationStatusRequestParams
                    , false
                    );
            }
            catch (CustomsRequestsSheetDomainModelServiceException myCustomsRequestsSheetServiceException)
            {
                if (myCustomsRequestsSheetServiceException.Where == CustomsRequestsSheetDomainModelServiceException.WhereEnum.SameRequestInProgress)
                {
                    Logitude.Server.Tools.Helpers.LogMessagingUtil.Instance.AppendLine("8250 RequestInProgress stop create a new one !! ");
                }
                // throw;
            }
        }

    }
}
