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
using UnifreightIIG.Common.CargoQueryMessageServiceReference;
using Logitude.AmitalMessaging.Customs.CustomFile;
using Logitude.AmitalMessaging.Utils;
using Unifreight.BL.EntityPMs.UGenerated;
using Unifreight.BL.EntityQueryServices;
using System.Collections;
using Unifreight.Data.AmitalModel;
using System.Transactions;
using Simplog.Server.Infrastructure.Helpers;
using Unifreight.BL.EntityPMs;
using Unifreight.BL.EntityUpdateServices;
using Logitude.Server.Tools.Models;
using Logitude.AmitalMessaging.Infrastructure.Transmission;
using Logitude.Customs.Def.Messaging.Customs;

namespace Logitude.CustomsMessaging.ResponseServices
{

    public class MN_NG_8241_CargoResponseService : ResponseServiceBase<CargoQueryResponseData, MN_NG_8241_Cargo_Message, CargoQueryRequestParams>
    {
        public bool _IsSubmitDeclarationResponse { get; set; }
        DeclarationPM _MyDeclarationPM;
        private GTRTRANQueryService _GTRTRANQueryService;
        private Dictionary<string, IList> _MyLocalCache = new Dictionary<string, IList>();
        private AmitalContext _AmitalContext;

        public override void Update(MN_NG_8241_Cargo_Message customResponse, CargoQueryRequestParams requestParams)
        {
            //Analyze message 8241 - Cargo Message
            string xml = null;
            var responseName = requestParams.ResponseName;
            var context = CustomContext.GetContext(requestParams.Tenant);
            var myDeclarationQueryService = new DeclarationQueryService(context);
            var myDeclarationUpdateService = new DeclarationUpdateService(context, new Dictionary<string, IContext>(), requestParams.Tenant);
            DeclarationErrorPointerService mydDclarationErrorPointerService = new DeclarationErrorPointerService();

            this.MyResponseData = new CargoQueryResponseData();

            if (!string.IsNullOrWhiteSpace(requestParams.DeclarationId))
            {
                if (this.MyRequestSheetParam == null)
                {
                    this.MyRequestSheetParam = new RequestSheetParam();
                }
                this.MyRequestSheetParam.ObjectTableId1 = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
                this.MyRequestSheetParam.EntityId1 = requestParams.DeclarationId;
            }

            //Checking foe Exceptions
            if (customResponse.ResponseContentHeader.Exception != null)
            {
                this.MyResponseData.Succeeded = true;
                this.MyResponseData.HasException = true;
                this.MyResponseData.UserMessage = customResponse.ResponseContentHeader.Exception.FirstOrDefault().ExeptionDescription;
                this.MyResponseData.ResponseStatusXML = GetDummyXml(this.MyResponseData.UserMessage);
                LogMessagingUtil.Instance.AppendLine(customResponse.ResponseContentHeader.Exception.FirstOrDefault().ExeptionDescription);
                this.MyResponseData.IsShowUserMessage = true; //Yuval Chalup 07.03.2016 TASK-20176
                //return; //Yuval Chalup 12.09.2016 CA-271500 (Remark)
            }
            if (customResponse.Exception != null)
            {
                this.MyResponseData.Succeeded = true;
                this.MyResponseData.HasException = true;
                this.MyResponseData.UserMessage = customResponse.Exception.ExeptionDescription;
                this.MyResponseData.ResponseStatusXML = GetDummyXml(this.MyResponseData.UserMessage);
                LogMessagingUtil.Instance.AppendLine(customResponse.Exception.ExeptionDescription);
                this.MyResponseData.IsShowUserMessage = true; //Yuval Chalup 07.03.2016 TASK-20176
                //return; //Yuval Chalup 12.09.2016 CA-271500 (Remark)
            }

            LogMessagingUtil.Instance.AppendLine("Analyze Manifest Status Query response " + requestParams.DeclarationNumber);

            if (string.IsNullOrWhiteSpace(requestParams.DeclarationId))
            {
                LogMessagingUtil.Instance.AppendLine("Can not find declaration");
                this.MyResponseData.Succeeded = true;
                this.MyResponseData.HasException = true;
                this.MyResponseData.UserMessage = "Can not find declaration";
                this.MyResponseData.ResponseStatusXML = GetDummyXml("Can not find declaration", customResponse.Cargo);
                this.MyResponseData.IsShowUserMessage = true; //Yuval Chalup 07.03.2016 TASK-20176
                //return; //Yuval Chalup 12.09.2016 CA-271500 (Remark)
            }
            else
            {
                this._MyDeclarationPM = myDeclarationQueryService.GetSingle(requestParams.DeclarationId, true, false);
                if (this._MyDeclarationPM == null)
                {
                    var text = "Can not find declaration";
                    LogMessagingUtil.Instance.AppendLine(text);
                    this.MyResponseData.ApplicationID = requestParams.DeclarationId;
                    this.MyResponseData.Succeeded = true;
                    this.MyResponseData.HasException = true;
                    this.MyResponseData.UserMessage = text;
                    this.MyResponseData.ResponseStatusXML = GetDummyXml(text, customResponse.Cargo);
                    this.MyResponseData.IsShowUserMessage = true; //Yuval Chalup 07.03.2016 TASK-20176
                    //return; //Yuval Chalup 12.09.2016 CA-271500 (Remark)
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(this._MyDeclarationPM.Id))
                    {
                        if (this.MyRequestSheetParam == null)
                        {
                            this.MyRequestSheetParam = new RequestSheetParam();
                        }
                        this.MyRequestSheetParam.ObjectTableId1 = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
                        this.MyRequestSheetParam.EntityId1 = this._MyDeclarationPM.Id;
                    }

                    string text = "";
                    CustomsRequestsSheetQueryService customsRequestsSheetQueryService = new CustomsRequestsSheetQueryService(context);
                    List<CustomsRequestsSheetPM> customsRequestsSheetPMList = customsRequestsSheetQueryService.GetRequestInProgress(_MyDeclarationPM.Tenant, "2750", "", "", null, null, _MyDeclarationPM.CustomFileNo, true);
                    if (customsRequestsSheetPMList != null)
                    {
                        if (customsRequestsSheetPMList.Count > 0)
                        {
                            var RequestInProgressInterfaceTypeName = customsRequestsSheetPMList.First().InterfaceTypeName;
                            text = TranslateTextsClass.Translate("Customs.General.RequestInProgress", _MyDeclarationPM.Tenant,true);
                            text = String.Format(text, RequestInProgressInterfaceTypeName);
                        }
                    }

                    //Check if Declaration was already paid, constraint in progress or Future payment was done
                    var declarationValidator = new Logitude.Customs.BL.Validators.DeclarationValidator(_MyDeclarationPM);
                    declarationValidator.DeclarationViewDisplayOnlyChecks();
                    if (declarationValidator.ErrorCode.Count > 0)
                    {
                        //text = TranslateTextsClass.Translate(declarationValidator.ErrorCode[0], _MyDeclarationPM.Tenant);
                        text = TranslateTextsClass.Translate(declarationValidator.ErrorCode[0], _MyDeclarationPM.Tenant, true);
                    }

                    if (!string.IsNullOrWhiteSpace(text))
                    {
                        //LogMessagingUtil.Instance.AppendLine(TranslateTextsClass.Translate(declarationValidator.ErrorCode[0], _MyDeclarationPM.Tenant)); //Yuval Chalup 07.01.2016 TASK-19656
                        text = text + ". נתוני ההצהרה לא עודכנו";
                        //text = text + " (המכולות בתיק העמילות עודכנו)";
                        LogMessagingUtil.Instance.AppendLine(text);
                        this.MyResponseData.ApplicationID = requestParams.DeclarationId;
                        this.MyResponseData.Succeeded = true;
                        this.MyResponseData.HasException = false;
                        this.MyResponseData.UserMessage = text;
                        GetResponseDetails(customResponse);
                        //this.MyResponseData.ResponseStatusXML = GetDummyXml(text, customResponse.Cargo);
                        this.MyResponseData.IsShowUserMessage = true; //Yuval Chalup 07.03.2016 TASK-20176

                        //<--- Yuval Chalup 13.03.2016 TASK-20524
                        _MyDeclarationPM.ChangeSetOp = ChangeSetOperation.Update;
                        var _myCFIPACKS = GetCFIPACKSXML(customResponse);
                        var _FileAdditionalData = GetFileAdditionalDataXML(customResponse);
                        if (_myCFIPACKS != null)
                        {
                            text = text + " (המכולות בתיק העמילות עודכנו)";
                            this.MyResponseData.UserMessage = text;
                        }
                        if (requestParams.IsAngularClient != true)requestParams.AutoSend = true; // temp - AutoSend implemented only in angular
                        string _status = null;
                        DateTime? _statusDate = DateTime.Now;
                        if (customResponse.Cargo != null && 
                            customResponse.Cargo.CargoAdditionalData != null && customResponse.Cargo.CargoAdditionalData.Count() > 0)
                        {
                            if (customResponse.Cargo.totalNumberOfPackeges == customResponse.Cargo.CargoAdditionalData[0].totalRecordNumberOfPackeges && customResponse.Cargo.CargoAdditionalData[0].StorageDate != null)
                            {
                                if (_MyDeclarationPM.TransportModeId == "A")
                                {
                                    _status = "SMG";
                                }
                                else //if(_MyDeclarationPM.TransportModeId == "O")
                                {
                                    _status = "SST";
                                }
                                _statusDate = customResponse.Cargo.CargoAdditionalData[0].StorageDate;
                            }
                        }
                        CargoQueryContext _cargoContext = new CargoQueryContext
                        {
                            ResponseCFIPACKS = _myCFIPACKS,
                            RequestAutoSend = requestParams.AutoSend,
                            RaiseStatus = _status,
                            CargoData = "",
                            StatusDate = _statusDate,
                            FileAdditionalData = _FileAdditionalData
                    };
                        _MyDeclarationPM.CurrentContextTag = _cargoContext;

                        OpenUnifreighTask();
                        myDeclarationUpdateService.SuppressNewConcurrencyGUID = true;
                        myDeclarationUpdateService.Update(_MyDeclarationPM, true);
                        //Yuval Chalup 13.03.2016 TASK-20524 --->

                        return;
                    }
                    
                    LogMessagingUtil.Instance.AppendLine("Analyze Manifest response " + requestParams.DeclarationNumber);
                    customResponse.Cargo = customResponse.Cargo ?? new MN_NG_8241_Cargo_MessageCargo();
                    customResponse.Cargo.CargoAdditionalData = customResponse.Cargo.CargoAdditionalData ?? new MN_NG_8241_Cargo_MessageCargoCargoAdditionalData[] { new MN_NG_8241_Cargo_MessageCargoCargoAdditionalData() };

                    Boolean _IsRunOver = false;
                    if (_MyDeclarationPM.Consignments != null && _MyDeclarationPM.Consignments.Count() > 0)
                    {
                        string defValue = GetDefault("ISRAEL", "CGG_MAN_RUNOVR", "NON", "NON", _MyDeclarationPM.Tenant);
                        if (defValue == "Y")
                        {
                            _IsRunOver = true;
                        }
                        if (String.IsNullOrWhiteSpace(_MyDeclarationPM.Consignments[0].UnloadPortCode) || _IsRunOver)
                        {
                            _MyDeclarationPM.Consignments[0].UnloadPortCode = customResponse.Cargo.CargoAdditionalData.First().unloadingLocationID;
                            _MyDeclarationPM.Consignments[0].ChangeSetOp = ChangeSetOperation.Update; 
                        }
                        if (String.IsNullOrWhiteSpace(_MyDeclarationPM.Consignments[0].StorageSiteCode) || _IsRunOver)
                        {
                            _MyDeclarationPM.Consignments[0].StorageSiteCode = customResponse.Cargo.CargoAdditionalData.First().acceptedArrivalSiteID;
                            _MyDeclarationPM.Consignments[0].ChangeSetOp = ChangeSetOperation.Update; 
                        }
                        if ((String.IsNullOrWhiteSpace(_MyDeclarationPM.Consignments[0].LoadingPortCode) || _IsRunOver) && !String.IsNullOrWhiteSpace(customResponse.Cargo.CargoAdditionalData.First().LoadingSite))
                        {
                            _MyDeclarationPM.Consignments[0].LoadingPortCode = customResponse.Cargo.CargoAdditionalData.First().LoadingSite.Substring(0,5);
                            _MyDeclarationPM.Consignments[0].ChangeSetOp = ChangeSetOperation.Update;
                        }
                        if ((String.IsNullOrWhiteSpace(_MyDeclarationPM.Consignments[0].OriginCountryCode) || _IsRunOver) && !String.IsNullOrWhiteSpace(customResponse.Cargo.CargoAdditionalData.First().LoadingSite))
                        {
                            _MyDeclarationPM.Consignments[0].OriginCountryCode = customResponse.Cargo.CargoAdditionalData.First().LoadingSite.Substring(0, 2);
                            _MyDeclarationPM.Consignments[0].ChangeSetOp = ChangeSetOperation.Update;
                        }
                        //If there are NO packages OR If there is one DUMMY package (without wight, quantity and pack type)
                        if (_MyDeclarationPM.Consignments[0].ConsignmentPackages == null || _MyDeclarationPM.Consignments[0].ConsignmentPackages.Count() == 0 ||
                            (_MyDeclarationPM.Consignments[0].ConsignmentPackages.Count() == 1 &&
                            //(_MyDeclarationPM.Consignments[0].ConsignmentPackages.First().PackageTypeCode == null || _MyDeclarationPM.Consignments[0].ConsignmentPackages.First().PackageTypeCode == "") &&
                            (_MyDeclarationPM.Consignments[0].ConsignmentPackages.First().PackageQuantity == null || _MyDeclarationPM.Consignments[0].ConsignmentPackages.First().PackageQuantity == 0) &&
                            (_MyDeclarationPM.Consignments[0].ConsignmentPackages.First().GrossMassMeasure == null || _MyDeclarationPM.Consignments[0].ConsignmentPackages.First().GrossMassMeasure == 0)))
                        {
                            //If there is one DUMMY package (without wight, quantity and pack type) - Set first package as Update
                            if (_MyDeclarationPM.Consignments[0].ConsignmentPackages.Count() == 1 &&
                            //(_MyDeclarationPM.Consignments[0].ConsignmentPackages.First().PackageTypeCode == null || _MyDeclarationPM.Consignments[0].ConsignmentPackages.First().PackageTypeCode == "") &&
                            (_MyDeclarationPM.Consignments[0].ConsignmentPackages.First().PackageQuantity == null || _MyDeclarationPM.Consignments[0].ConsignmentPackages.First().PackageQuantity == 0) &&
                            (_MyDeclarationPM.Consignments[0].ConsignmentPackages.First().GrossMassMeasure == null || _MyDeclarationPM.Consignments[0].ConsignmentPackages.First().GrossMassMeasure == 0))
                            {
                                _MyDeclarationPM.Consignments[0].ConsignmentPackages[0].ChangeSetOp = ChangeSetOperation.Delete; 
                            }
                            //_MyDeclarationPM.Consignments[0].ConsignmentPackages = GetDeclarationConsignmentsPackagesPM(customResponse, _MyDeclarationPM.Consignments[0]);
                            foreach (var consignmentPackageDelete in _MyDeclarationPM.Consignments[0].ConsignmentPackages)
                            {
                                consignmentPackageDelete.ChangeSetOp = ChangeSetOperation.Delete; 
                            }
                            foreach (var consignmentPackageInsert in GetDeclarationConsignmentsPackagesPM(customResponse, _MyDeclarationPM.Consignments[0]))
                            {
                                _MyDeclarationPM.Consignments[0].ConsignmentPackages.Add(consignmentPackageInsert); 
                            }
                            _MyDeclarationPM.Consignments[0].ChangeSetOp = ChangeSetOperation.Update; 
                        }
                    }
                    if (_MyDeclarationPM.Consignments[0].ChangeSetOp == ChangeSetOperation.None)
                    {
                        myDeclarationUpdateService.SuppressNewConcurrencyGUID = true;
                    }
                    else
                    {
                        myDeclarationUpdateService.SuppressNewConcurrencyGUID = false;
                    }
                    _MyDeclarationPM.ChangeSetOp = ChangeSetOperation.Update;
                    //_MyDeclarationPM.CurrentContextTag = GetCFIPACKSXML(customResponse);
                    var myCFIPACKS = GetCFIPACKSXML(customResponse);
                    var myFileAdditionalData = GetFileAdditionalDataXML(customResponse);
                    if (requestParams.IsAngularClient != true) requestParams.AutoSend = true; // temp - AutoSend implemented only in angular
                    string status = null;
                    DateTime? statusDate = DateTime.Now;
                    if (customResponse.Cargo != null && customResponse.Cargo.CargoAdditionalData != null && customResponse.Cargo.CargoAdditionalData.Count() > 0 && customResponse.Cargo.totalNumberOfPackeges == customResponse.Cargo.CargoAdditionalData[0].totalRecordNumberOfPackeges && customResponse.Cargo.CargoAdditionalData[0].StorageDate != null) 
                    {
                        if (_MyDeclarationPM.TransportModeId == "A")
                        {
                            status = "SMG";
                        }
                        else //if(_MyDeclarationPM.TransportModeId == "O")
                        {
                            status = "SST";
                        }
                        statusDate = customResponse.Cargo.CargoAdditionalData[0].StorageDate;
                    }
                    CargoQueryContext cargoContext = new CargoQueryContext
                    {
                        ResponseCFIPACKS = myCFIPACKS,
                        RequestAutoSend = requestParams.AutoSend,
                        RaiseStatus = status,
                        CargoData = "",
                        StatusDate = statusDate,
                        FileAdditionalData = myFileAdditionalData
                    };
                    _MyDeclarationPM.CurrentContextTag = cargoContext;
                    myDeclarationUpdateService.Update(_MyDeclarationPM, true);
                }
            }

            //xml = XmlGenericUtil<MN_NG_8241_Cargo_MessageCargo>.SerializeObject(customResponse.Cargo);
            //MyResponseData.ResponseStatusXML = xml;
            MyResponseData.Succeeded = true;
            MyResponseData.HasException = false;
            GetResponseDetails(customResponse);
            MyResponseData.ApplicationID = requestParams.DeclarationId;
            MyResponseData.UserMessage = "שליחת מסר מצהר בוצעה בהצלחה. " + MyResponseData.UserMessage; //Yuval Chalup 12.09.2016 CA-271500 (Concat)
        }

        private string GetDefault(string DISTRID, string DEFID, string BRANCHID, string CARDID, int tenant)
        {
            AmitalContext amitalContext = AmitalContext.GetContext(tenant);
            var myGDFDATAQueryService = new GDFDATAQueryService(amitalContext);

            if (DISTRID == null || DEFID == null || BRANCHID == null || CARDID == null)
            {
                return ("");
            }

            GDFDATAPM myGDFDATAPM = myGDFDATAQueryService.GetSingle(DISTRID, DEFID, BRANCHID, CARDID, false, true);
            if (myGDFDATAPM == null)
            {
                return ("");
            }
            return (myGDFDATAPM.DEFDATA);
        }

        private void GetResponseDetails(MN_NG_8241_Cargo_Message customResponse)
        {
            //Manifest Details
            if (customResponse.Manifest != null)
            {
                MyResponseData.ManifestType = customResponse.Manifest.manifestType.ToString();
                MyResponseData.ManifestTypeName = customResponse.Manifest.manifestTypeName;
                MyResponseData.ManifestStatus = customResponse.Manifest.manifestStatus.ToString();
                MyResponseData.ManifestStatusName = customResponse.Manifest.manifestStatusName;
                MyResponseData.Manifestnumber = customResponse.Manifest.Manifestnumber;
                MyResponseData.IsSendingMorethenOneFlights = customResponse.Manifest.IsSendingMorethanOneFlight;
            }

            //Cargo Details
            if (customResponse.Cargo != null)
            {
                MyResponseData.CargoResultList = new CargoResult();
                MyResponseData.CargoResultList.CargoIdentifierKey1 = customResponse.Cargo.cargoIdentifierKey1;
                if (customResponse.Manifest != null && customResponse.Manifest.manifestType == 1 && !String.IsNullOrWhiteSpace(customResponse.Cargo.cargoIdentifierKey2))
                {
                    MyResponseData.CargoResultList.CargoIdentifierKey1 = customResponse.Cargo.cargoIdentifierKey2;
                }
                MyResponseData.CargoResultList.ParentCargoID = customResponse.Cargo.parentCargoID;
                MyResponseData.CargoResultList.TotalNumberOfPackeges = customResponse.Cargo.totalNumberOfPackeges.ToString("N2");
                MyResponseData.CargoResultList.GovernmentProcedureType = customResponse.Cargo.governmentProcedureType.ToString();
                MyResponseData.CargoResultList.GovernmentProcedureTypeName = customResponse.Cargo.governmentProcedureTypeName;
                MyResponseData.CargoResultList.TreatmentWayName = customResponse.Cargo.treatmentWayName;
                MyResponseData.CargoResultList.TotalWeight = customResponse.Cargo.totalWeight.ToString("N2");
                MyResponseData.CargoResultList.MasterBolNumber = customResponse.Cargo.MasterBolNumber;
                MyResponseData.CargoResultList.BillOfLadingNumber = customResponse.Cargo.BillOfLadingNumber;
                //Yuval Chalup 19.09.2016 T-23023 (Add the IF only - to avoid CargoAdditionalData.FirstOrDefault() NULL)
                if (customResponse.Cargo.CargoAdditionalData != null)
                {
                    if (customResponse.Cargo.CargoAdditionalData.Count() > 0)
                    {
                        MyResponseData.CargoResultList.CargoAdditionalDataList = new CargoAdditionalData();

                        MyResponseData.CargoResultList.CargoAdditionalDataList.UnloadingLocationName = customResponse.Cargo.CargoAdditionalData.FirstOrDefault().unloadingLocationName;
                        MyResponseData.CargoResultList.CargoAdditionalDataList.TotalRecordNumberOfPackeges = String.Format("{0:N2}", customResponse.Cargo.CargoAdditionalData.FirstOrDefault().totalRecordNumberOfPackeges);
                        MyResponseData.CargoResultList.CargoAdditionalDataList.GoodsReceiptPlaceSiteName = customResponse.Cargo.CargoAdditionalData.FirstOrDefault().acceptedArrivalSiteName;
                        MyResponseData.CargoResultList.CargoAdditionalDataList.TotalRecordWeight = String.Format("{0:N2}", customResponse.Cargo.CargoAdditionalData.FirstOrDefault().totalRecordWeight);
                        MyResponseData.CargoResultList.CargoAdditionalDataList.TransitDestinationLocationName = customResponse.Cargo.CargoAdditionalData.FirstOrDefault().transitDestinationLocationName;
                    }
                }
            }

            //DeliveryOrder Details
            if (customResponse.DeliveryOrder != null)
            {
                MyResponseData.DeliveryOrderResultList = new List<DeliveryOrderResult>();
                foreach (var deliveryOrderItem in customResponse.DeliveryOrder)
                {
                    DeliveryOrderResult deliveryOrderResult = new DeliveryOrderResult();
                    deliveryOrderResult.DeliveryOrderNumber = deliveryOrderItem.deliveryOrderNumber.ToString();
                    deliveryOrderResult.ProducerName = deliveryOrderItem.producerName;
                    deliveryOrderResult.ReceiverName = deliveryOrderItem.receiverName;
                    deliveryOrderResult.ReceiverCustomerActivityType = deliveryOrderItem.Receiver_CustomerActivityType.ToString("N2");
                    deliveryOrderResult.ReceiverCustomerActivityTypeName = deliveryOrderItem.Receiver_CustomerActivityTypeName;
                    deliveryOrderResult.DeliveryOrderDate = String.Format("{0:g}", deliveryOrderItem.DeliveryOrderDate);
                    deliveryOrderResult.DeliveryOrderStatus = deliveryOrderItem.deliveryOrderStatus.ToString("N2");
                    deliveryOrderResult.DeliveryOrderStatusName = deliveryOrderItem.deliveryOrderStatusName;
                    deliveryOrderResult.DeliverySiteId = deliveryOrderItem.deliverySiteId;
                    deliveryOrderResult.DeliverySiteName = deliveryOrderItem.deliverySiteName;

                    MyResponseData.DeliveryOrderResultList.Add(deliveryOrderResult);
                }
            }

            //CargosVersion Details
            if (customResponse.CargosVersion != null)
            {
                MyResponseData.CargosVersionResultList = new List<CargosVersionResult>();
                foreach (var cargosVersionItem in customResponse.CargosVersion)
                {
                    CargosVersionResult cargosVersionResult = new CargosVersionResult();
                    cargosVersionResult.Version = cargosVersionItem.Version.ToString();
                    cargosVersionResult.SubmiterName = cargosVersionItem.SubmiterName;
                    cargosVersionResult.CreateDate = String.Format("{0:g}", cargosVersionItem.createDate);
                    cargosVersionResult.ActionDate = String.Format("{0:g}", cargosVersionItem.actionDate);
                    cargosVersionResult.CargoStatus = cargosVersionItem.CargoStaus.ToString("N2"); ;
                    cargosVersionResult.CargoStausName = cargosVersionItem.CargoStausName;

                    MyResponseData.CargosVersionResultList.Add(cargosVersionResult);
                }
            }

            //CargoItem Details
            if (customResponse.CargoItem != null)
            {
                MyResponseData.CargoItemResultList = new List<CargoItemResult>();
                foreach (var cargosItem in customResponse.CargoItem)
                {
                    CargoItemResult cargoItemResult = new CargoItemResult();
                    cargoItemResult.RowNumber = cargosItem.rowNumber;
                    cargoItemResult.ParentCargoRowDetailsID = cargosItem.parentCargoRowDetailsID;
                    cargoItemResult.ContainerNumber = cargosItem.containerNumber;
                    cargoItemResult.CharacteristicCode = cargosItem.characteristicCode;
                    cargoItemResult.ContainerType = cargosItem.containerType;
                    cargoItemResult.Length = cargosItem.Length;
                    cargoItemResult.PackingType = cargosItem.PackingType;
                    cargoItemResult.Quantity = cargosItem.Quantity.ToString("N2");
                    cargoItemResult.GrossMassMeasureWeight = String.Format("{0:N2}", cargosItem.grossMassMeasureWeight);
                    cargoItemResult.RecordNumberOfPackeges = String.Format("{0:N2}", cargosItem.RecordNumberOfPackeges);
                    cargoItemResult.TotalRecordWeight = String.Format("{0:N2}", cargosItem.totalRecordWeight);
                    if (cargosItem.DangerousGoodsIndication == true)
                    {
                        cargoItemResult.DangerousGoodsIndication = "Visible";
                    }
                    else
                    {
                        cargoItemResult.DangerousGoodsIndication = "Collapsed";
                    }

                    //Seal Details
                    if (cargosItem.SealDetails != null)
                    {
                        int rowNumber = 1;
                        cargoItemResult.SealDetailsList = new List<SealDetails>();
                        foreach (var sealDetailsItem in cargosItem.SealDetails)
                        {
                            SealDetails sealDetails = new SealDetails();
                            sealDetails.RowNumber = rowNumber.ToString();
                            sealDetails.SealType = sealDetailsItem.sealType.ToString("N2");
                            sealDetails.SealTypeName = sealDetailsItem.sealTypeName;
                            sealDetails.SealNumber = sealDetailsItem.sealNumber;
                            rowNumber++;
                            cargoItemResult.SealDetailsList.Add(sealDetails);
                        }
                    }

                    //Movment Details
                    if (cargosItem.CargoMovement != null)
                    {
                        int rowNumber = 1;
                        cargoItemResult.CargoMovmentList = new List<CargoMovment>();
                        foreach (var cargoMovmentItem in cargosItem.CargoMovement)
                        {
                            CargoMovment cargoMovmentDetails = new CargoMovment();
                            cargoMovmentDetails.RowNumber = rowNumber.ToString();
                            cargoMovmentDetails.ExitReasonID = cargoMovmentItem.exitReasonID.ToString();
                            cargoMovmentDetails.ExitReasonName = cargoMovmentItem.exitReasonName;
                            cargoMovmentDetails.StatusID = cargoMovmentItem.statusID.ToString();
                            cargoMovmentDetails.StatusName = cargoMovmentItem.statusName;
                            cargoMovmentDetails.DocumentNumber = cargoMovmentItem.documentNumber;
                            cargoMovmentDetails.ReferenceTypeID = String.Format("{0:N2}", cargoMovmentItem.referenceTypeID);
                            cargoMovmentDetails.ReferenceTypeName = cargoMovmentItem.referenceTypeName;
                            cargoMovmentDetails.ReferenceNum = cargoMovmentItem.referenceNum;
                            cargoMovmentDetails.ExitSiteId = cargoMovmentItem.ExitSiteId;
                            cargoMovmentDetails.ExitSiteName = cargoMovmentItem.ExitSiteName;
                            cargoMovmentDetails.ExitDateTime = String.Format("{0:g}", cargoMovmentItem.ExitDateTime);
                            cargoMovmentDetails.EntrySiteId = cargoMovmentItem.entrySiteId;
                            cargoMovmentDetails.EntrySiteName = cargoMovmentItem.entrySiteName;
                            cargoMovmentDetails.EntryDateTime = String.Format("{0:g}", cargoMovmentItem.EntryDateTime);
                            rowNumber++;
                            cargoItemResult.CargoMovmentList.Add(cargoMovmentDetails);
                        }
                    }
                    MyResponseData.CargoItemResultList.Add(cargoItemResult);
                }
            }
        }

        private void OpenUnifreighTask()
        {
            TransactionScope scope = null;
            if (!DbContextBaseUtil.UnifreightDataIncludedInMain_FeatureOn)
            {
                scope = TransactionFactory.GetNewOracleReadCommittedTransaction();
            }
            try
            {
                using (_AmitalContext = AmitalContext.GetContext(_MyDeclarationPM.Tenant))
                {
                    var myCCUQUELOCKQueryService = new CCUQUELOCKQueryService(_AmitalContext);
                    var myCCUQUELOCKUpdateService = new CCUQUELOCKUpdateService(_AmitalContext);
                    var myGGGQUpdateService = new GGGQUpdateService(_AmitalContext);
                    var myYCULTASKUpdateService = new YCULTASKUpdateService(_AmitalContext);
                    var requestData = "";
                    //var unifreightUser = AuthenticationUtil.ResolveUnifreightUserId(_MyDeclarationPM.Tenant);
                    string unifreightUser = null;
                    if (RequestSheetContext.Current != null)
                    {
                        var loggingUserIdFromRS = RequestSheetContext.Current.GetContextOrDefault().GetUserFromRequestParam();
                        if (!string.IsNullOrWhiteSpace(loggingUserIdFromRS))
                        {
                            UserRepository userRep = new UserRepository(_MyDeclarationPM.Tenant);
                            User user = userRep.GetSingleUser(loggingUserIdFromRS, _MyDeclarationPM.Tenant, true);
                            if (user != null)
                            {
                                if (!String.IsNullOrWhiteSpace(user.Code))
                                {
                                    unifreightUser = user.Code;
                                }
                            }
                        }
                    }
                    if (String.IsNullOrWhiteSpace(unifreightUser))
                    {
                        unifreightUser = AuthenticationUtil.ResolveUnifreightUserId(_MyDeclarationPM.Tenant);
                    }
                    //long customFile;
                    if (string.IsNullOrWhiteSpace(_MyDeclarationPM.CustomFileNo)
                        //|| !long.TryParse(_MyDeclarationPM.CustomFileNo, out customFile)
                        )
                    {
                        return;
                    }
                    CCUQUELOCKPM myCCUQUELOCK = myCCUQUELOCKQueryService.GetSingle("CFIFILEM", _MyDeclarationPM.CustomFileNo, false);
                    if (myCCUQUELOCK == null)
                    {
                        var myCCUQUELOCKPM = new CCUQUELOCKPM()
                        {
                            ChangeSetOp = ChangeSetOperation.Insert,
                            ENTNAME = "CFIFILEM",
                            FILENO = _MyDeclarationPM.CustomFileNo,
                        };
                        myCCUQUELOCKUpdateService.DontAddTransaction = true;//we cant add a transaction with isolation level snap shot inside a read committed one so you have to assign this prop to true mohammad.
                        myCCUQUELOCKUpdateService.Update(myCCUQUELOCKPM, true);
                    }

                    if (_MyDeclarationPM.CurrentContextTag is CargoQueryContext)
                    {
                        var myCargoQueryContext = _MyDeclarationPM.CurrentContextTag as CargoQueryContext;
                        var myCFIPACKS = myCargoQueryContext.ResponseCFIPACKS as CFIPACKS;
                        string myCustomFileNo = "";
                        if (myCFIPACKS != null)
                        {
                            myCustomFileNo = myCFIPACKS.CFIPACKS_DATA[0].FILE_NO;
                            var xmlCFIPACKS = XmlGenericUtil<CFIPACKS>.SerializeObject(myCFIPACKS, true);
                            CCUQUELOCKPM myCCUQUELOCK_Packs = myCCUQUELOCKQueryService.GetSingle("CFIFILEM", myCFIPACKS.CFIPACKS_DATA[0].FILE_NO, false);
                            if (myCCUQUELOCK_Packs == null)
                            {
                                var myCCUQUELOCKPM = new CCUQUELOCKPM()
                                {
                                    ChangeSetOp = ChangeSetOperation.Insert,
                                    ENTNAME = "CFIFILEM",
                                    FILENO = myCFIPACKS.CFIPACKS_DATA[0].FILE_NO,
                                };
                                myCCUQUELOCKUpdateService.DontAddTransaction = true;//we cant add a transaction with isolation level snap shot inside a read committed one so you have to assign this prop to true mohammad.
                                myCCUQUELOCKUpdateService.Update(myCCUQUELOCKPM, true);
                            }

                            transmission mytransmission = GetTransmission(myCFIPACKS, "AMITAL", "Customs packs from logitude");
                            var xmltransmission = XmlGenericUtil<transmission>.SerializeObject(mytransmission, true);
                            requestData = xmltransmission;
                        }
                        if (myCargoQueryContext.RequestAutoSend)
                        {
                            requestData = requestData.Replace("</transmission>", string.Concat("<CARGOQUERYMODE>AUTOSEND</CARGOQUERYMODE>", "</transmission>"));
                        }
                        if (string.IsNullOrWhiteSpace(myCustomFileNo)) myCustomFileNo = _MyDeclarationPM.CustomFileNo;

                        if (!string.IsNullOrWhiteSpace(requestData))
                        {
                            var myYCULTASKPM_Packs = new YCULTASKPM()
                            {
                                ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert,
                                STATUS = "W",
                                REQUESTDATA = requestData,
                                ENTNAME = "CFIFILEM",
                                PRIMARYNUM = myCustomFileNo,
                                PRIORITY = YCULTASKPM.calcPriority("L2U"),
                                //PRIORITY = 1,
                                TYPE = "L2U",
                                USRCODE = unifreightUser,
                                ARCHIVE = "F",
                                //LOGTIME = (new DualQueryService(MainContext as AmitalContext)).GetServerDateTime() ?? DateTime.Now,
                            };
                            //myYCULTASKPM.TASKID = CommCounterUtil.GetUnique30(myYCULTASKPM.LOGTIME);
                            myYCULTASKUpdateService.DontAddTransaction = true;//we cant add a transaction with isolation level snap shot inside a read committed one so you have to assign this prop to true mohammad.
                            myYCULTASKUpdateService.Update(myYCULTASKPM_Packs, true);

                            var myGGGQPM_Packs = new GGGQPM()
                            {
                                ChangeSetOp = ChangeSetOperation.Insert,
                                ORIGINQUE = "LGT", //LugitudeRequest
                                STATUS = "1",
                                EXPTASKTIME = 5,
                                EXECDATE = (new DualQueryService(_AmitalContext as AmitalContext)).GetServerDateTime() ?? DateTime.Now.AddMinutes(-20), //-20 because of time differences between the server where the code runs in and the DB server
                                TRY = 9,
                                PRIORITY = 8,
                                ENTNAME = "CFIFILEM",
                                PRIMARYNUM = myCustomFileNo,
                                FORMID = "LGT_UPDATE_FCI",
                                DEBUG = "F",
                                DONEOPERATION = "A",
                                //GSTRING1 = myYCULTASKPM.TASKID,
                            };
                            myGGGQUpdateService.DontAddTransaction = true;//we cant add a transaction with isolation level snap shot inside a read committed one so you have to assign this prop to true mohammad.
                            myGGGQUpdateService.Update(myGGGQPM_Packs, true);
                        }
                    }
                }
            }
            finally
            {
                if (scope != null)
                {
                    scope.Dispose();
                }
            }
        }

        transmission GetTransmission<T>(T mySerilazeObject, string from, string Subject)
    where T : class
            //transmission GetTransmission(CFIPACKS mySerilazeObject, string from, string Subject)
        {
            var CommunicationsParamsSubject = Subject;
            var mytransmission = new transmission();
            var mytransmission_details = new List<transmission_details>();
            var mytransmission_detail1 = new transmission_details()
            {
                sender = new sender() { Value = from },
                subject = new subject() { Value = CommunicationsParamsSubject }
            };

            var xml = XmlGenericUtil<T>.SerializeObject(mySerilazeObject, true);

            var myListdata = new List<data>() { new data() { entity = xml } };

            mytransmission.data = myListdata.ToArray();// GetDataList().ToArray();
            if (mytransmission.data.Count() < 1)
            {
                throw new System.Exception("(mytransmission.data.Count < 1)");
            }

            mytransmission_details.Add(mytransmission_detail1);
            mytransmission.transmission_details = mytransmission_details.ToArray();
            return mytransmission;
        }

        private string GetDummyXml(string message)
        {
            var myDummyXml = new GeneralMessage() { Message = message };
            var xml = XmlGenericUtil<GeneralMessage>.SerializeObject(myDummyXml);
            return xml;
        }

        private string GetDummyXml(string message, MN_NG_8241_Cargo_MessageCargo response)
        {
            var myDummyXml = new GeneralMessage() { Message = message };
            if (response != null)
            {
                myDummyXml.Response = new MN_NG_8241_Cargo_MessageCargo();
                myDummyXml.Response = response;
            }
            var xml = XmlGenericUtil<GeneralMessage>.SerializeObject(myDummyXml);
            return xml;
        }

        private CFIPACKS GetCFIPACKSXML(MN_NG_8241_Cargo_Message customResponse)
        {
            //var setting = CustomsSettingQueryService.GetSettingByTenant(_MyDeclarationPM.Tenant);
            //if (setting != null)
            //{
            //if (!setting.IsConnectedToUniFreight)
            if (!_MyDeclarationPM.IsConnectedToUnifreight)
            {
                return null;
            }
            //}

            if (customResponse.CargoItem == null)
            {
                return null;
            }
            else
            {
                TransactionScope scope = null;
                if (!DbContextBaseUtil.UnifreightDataIncludedInMain_FeatureOn)
                {
                    scope = TransactionFactory.GetNewOracleReadCommittedTransaction();
                }
                try
                {
                    using (_AmitalContext = AmitalContext.GetContext(_MyDeclarationPM.Tenant))
                    {
                        var myCFIPACKS_DATAList = new List<CFIPACKS_DATA>();
                        int line_no = 1;
                        foreach (var cargoItem in customResponse.CargoItem)
                        {
                            //Containers ONLY
                            if (cargoItem.PackingType == "D5")
                            {
                                string seal = "";
                                string packingType = "";
                                line_no = line_no + 1;
                                if (cargoItem.SealDetails != null)
                                {
                                    seal = cargoItem.SealDetails[0].sealNumber;
                                }
                                packingType = GetTranslationP2L("IIGC", "ITBPCKTY", cargoItem.characteristicCode);

                                CFIPACKS_DATA myCFIPACKS_DATA = new CFIPACKS_DATA
                                {
                                    FILE_NO = _MyDeclarationPM.CustomFileNo,
                                    LINE_NO = line_no.ToString(),
                                    CONT_NO = cargoItem.containerNumber,
                                    CONT_TYPE_ID = packingType,
                                    WEIGHT = cargoItem.grossMassMeasureWeight.ToString(),
                                    SEAL = seal,
                                };
                                myCFIPACKS_DATAList.Add(myCFIPACKS_DATA);
                            }
                        }
                        if (myCFIPACKS_DATAList != null)
                        {
                            if (myCFIPACKS_DATAList.Count > 0)
                            {
                                CFIPACKS myCFIPACKS = new CFIPACKS();
                                myCFIPACKS.CFIPACKS_DATA = myCFIPACKS_DATAList.ToArray();
                                return myCFIPACKS;
                            }
                        }
                        return null;
                    }

                }

                finally
                {
                    if (scope != null)
                    {
                        scope.Dispose();
                    }
                }
            }
        }

        private FileAdditionalData GetFileAdditionalDataXML(MN_NG_8241_Cargo_Message customResponse)
        {
           
            if (!_MyDeclarationPM.IsConnectedToUnifreight)
            {
                return null;
            }
            
            if (customResponse.Cargo == null)
            {
                return null;
            }
            else
            {
                TransactionScope scope = null;
                if (!DbContextBaseUtil.UnifreightDataIncludedInMain_FeatureOn)
                {
                    scope = TransactionFactory.GetNewOracleReadCommittedTransaction();
                }
                try
                {
                    using (_AmitalContext = AmitalContext.GetContext(_MyDeclarationPM.Tenant))
                    {
                        if (_MyDeclarationPM.Consignments != null && (_MyDeclarationPM.Consignments[0].CargoTypeCode == "11" || _MyDeclarationPM.Consignments[0].CargoTypeCode == "20"))
                        {
                            if (!String.IsNullOrWhiteSpace(customResponse.Cargo.MasterBolNumber) || !String.IsNullOrWhiteSpace(customResponse.Cargo.BillOfLadingNumber))
                            {


                                FileAdditionalData myFileAdditionalData = new FileAdditionalData();
                                myFileAdditionalData.HAWB = customResponse.Cargo.BillOfLadingNumber;
                                myFileAdditionalData.MAWB = customResponse.Cargo.MasterBolNumber;
                                return myFileAdditionalData;
                            }
                        }
                        
                        return null;
                    }

                }

                finally
                {
                    if (scope != null)
                    {
                        scope.Dispose();
                    }
                }
            }
        }

        private string GetTranslationP2L(string partnerID, string tableID, string partnerCode)
        {
            if (partnerID == null || tableID == null || partnerCode == null)
            {
                return ("");
            }

            if (_GTRTRANQueryService == null)
            {
                _GTRTRANQueryService = new GTRTRANQueryService(_AmitalContext);
            }
            return GetTranslationP2LFromCache(partnerID, tableID, partnerCode);
        }

        private string GetTranslationP2LFromCache(string partnerID, string tableID, string partnerCode)
        {
            var key = partnerID + "'," + tableID;
            if (!_MyLocalCache.ContainsKey(key))
            {
                _MyLocalCache[key] = _GTRTRANQueryService.GetMulti(partnerID, tableID) ?? new List<GTRTRANPM>();
            }
            var myList = _MyLocalCache[key] as List<GTRTRANPM>;
            var recordTR = myList.FirstOrDefault(rec => rec.PARTNERID == partnerID && rec.TABLEID == tableID && rec.PARTNERCODE == partnerCode);
            if (recordTR == null)
            {
                return "";
            }
            return recordTR.LOCALCODE;
        }

        public override CargoQueryResponseData GetResponse(MN_NG_8241_Cargo_Message customResponse, CargoQueryRequestParams requestParams)
        {
            return this.MyResponseData;
        }

        private List<ConsignmentPackagePM> GetDeclarationConsignmentsPackagesPM(MN_NG_8241_Cargo_Message customResponse, ConsignmentPM Consignment)
        {
            var declarationConsignmentsPackagesPMList = new List<ConsignmentPackagePM>();

            if (customResponse.CargoItem == null)
            {
                return declarationConsignmentsPackagesPMList;
            }

            int count = 0;
            string packtype = null;
            decimal weight = 0;
            int quntity = 0;
            customResponse.CargoItem.OrderBy(ci => ci.PackingType);
            //Array.Sort(customResponse.CargoItem);
            foreach (var package in customResponse.CargoItem)
            {
                if (package.PackingType != packtype && packtype != null)
                {
                    count++;
                    var declarationConsignmentPackage = new ConsignmentPackagePM();
                    declarationConsignmentPackage.SequenceNumeric = count;
                    declarationConsignmentPackage.ChangeSetOp = ChangeSetOperation.Insert;
                    declarationConsignmentPackage.ConsignmentNumber = Consignment.ConsignmentNumber;
                    declarationConsignmentPackage.DeclarationId = Consignment.DeclarationId;
                    declarationConsignmentPackage.LineNumber = count;
                    declarationConsignmentPackage.Tenant = Consignment.Tenant;

                    declarationConsignmentPackage.PackageMeasureQualifierCode = "2";
                    declarationConsignmentPackage.PackageTypeCode = package.PackingType;
                    if (quntity > 0)
                    {
                        declarationConsignmentPackage.PackageQuantity = quntity;
                    }

                    if (weight > 0)
                    {
                        declarationConsignmentPackage.GrossMassMeasure = weight;
                    }
                    weight = 0;
                    quntity = 0;
                    declarationConsignmentsPackagesPMList.Add(declarationConsignmentPackage);
                }
                else
                {
                    if (package.grossMassMeasureWeight.HasValue)
                    {
                        weight = weight + package.grossMassMeasureWeight.Value;
                    }
                    quntity = quntity + package.Quantity;
                }
                packtype = package.PackingType;
            }
            var lastPackage = customResponse.CargoItem.Last();
            if (weight > 0 || quntity > 0)
            {
                count++;
                var declarationConsignmentPackage = new ConsignmentPackagePM();
                declarationConsignmentPackage.SequenceNumeric = count;
                declarationConsignmentPackage.ChangeSetOp = ChangeSetOperation.Insert;
                declarationConsignmentPackage.ConsignmentNumber = Consignment.ConsignmentNumber;
                declarationConsignmentPackage.DeclarationId = Consignment.DeclarationId;
                declarationConsignmentPackage.LineNumber = count;
                declarationConsignmentPackage.Tenant = Consignment.Tenant;

                declarationConsignmentPackage.PackageMeasureQualifierCode = "2";
                declarationConsignmentPackage.PackageTypeCode = lastPackage.PackingType;
                if (quntity > 0)
                {
                    declarationConsignmentPackage.PackageQuantity = quntity;
                }

                if (weight > 0)
                {
                    declarationConsignmentPackage.GrossMassMeasure = weight;
                }

                declarationConsignmentsPackagesPMList.Add(declarationConsignmentPackage);
            }

            return declarationConsignmentsPackagesPMList;
        }

        public class GeneralMessage
        {
            public string Message { get; set; }
            public MN_NG_8241_Cargo_MessageCargo Response { get; set; }
        }
    }

}




