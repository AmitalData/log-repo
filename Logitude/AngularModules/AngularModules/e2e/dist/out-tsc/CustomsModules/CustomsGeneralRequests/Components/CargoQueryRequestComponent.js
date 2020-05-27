"use strict";
var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    }
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
Object.defineProperty(exports, "__esModule", { value: true });
var core_1 = require("@angular/core");
var CustomMessageWrapperComponent_1 = require("../../../CustomsModules/CustomsControls/Components/CustomMessageWrapperComponent");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var DeclarationExtendedListService_1 = require("../../../Customs/Services/ExtendedLists/DeclarationExtendedListService");
var IIGGeneralMessagesService_1 = require("../../../Customs/Services/WebServices/IIGGeneralMessagesService");
var CargoQueryRequestParams_1 = require("../../../Customs/DataContract/RequestParams/CargoQueryRequestParams");
var CargoQueryResponseData_1 = require("../../../Customs/DataContract/ResponseData/CargoQueryResponseData");
var Validator_1 = require("../../../Infrastructure/Validators/Validator");
var TextCodeTranslator_1 = require("../../../Infrastructure/Utilities/TextCodeTranslator");
var Tools_1 = require("../../../Infrastructure/Tools");
var BaseRequestsSheetMassaging_1 = require("../../../CustomsModules/CustomsRequests/Components/BaseRequestsSheetMassaging");
var RequestParamsBase_1 = require("../../../Customs/DataContract/RequestParams/RequestParamsBase");
var CustomMessageProgressComponent_1 = require("../../../CustomsModules/CustomsControls/Components/CustomMessageProgressComponent");
var ObservableCollection_1 = require("../../../Infrastructure/Utilities/ObservableCollection");
var DeclarationDisplayOnlyChecks_1 = require("../../../Customs/Utilities/DeclarationDisplayOnlyChecks");
var MessageWindow_1 = require("../../../Controls/Windows/MessageWindow");
var EntityResourceService_1 = require("../../../Infrastructure/Services/EntityResourceService");
var CargoQueryRequestComponent = /** @class */ (function (_super) {
    __extends(CargoQueryRequestComponent, _super);
    function CargoQueryRequestComponent(EntityResourceService) {
        var _this = _super.call(this) || this;
        _this.EntityResourceService = EntityResourceService;
        _this.DataContext = _this;
        _this.ObjectTableName = "Customs.Declaration";
        _this._DeclarationExtendedListService = new DeclarationExtendedListService_1.DeclarationExtendedListService();
        _this._IIGGeneralMessagesService = new IIGGeneralMessagesService_1.IIGGeneralMessagesService();
        _this._MyResponseObjectToShow = null;
        _this._UserMessagehidden = true;
        //DeclarationId: string ;
        _this._IsReady = false;
        _this._IsFromDeclaration = false;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.SuperCustomMessageWrapperComponent = new CustomMessageWrapperComponent_1.CustomMessageWrapperComponent();
        _this.DeliveryOrderResultList = new ObservableCollection_1.ObservableCollection([]);
        _this.CargosVersionResultList = new ObservableCollection_1.ObservableCollection([]);
        _this.CargoItemResultList = new ObservableCollection_1.ObservableCollection([]);
        _this.EntityResourceService.getEntityResourceByTableName("Customs.Consignment").subscribe(function (response) {
            _this.EntityResourceService.getEntityResourceByTableName("Customs.Declaration").subscribe(function (response) {
                _this._IsReady = true;
            });
        });
        return _this;
        //this.ValidationErrorsList 
        //this.UIProperties.SetRequired("DeclarationNumber", this.ObjectTableName, true);
    }
    CargoQueryRequestComponent.prototype.ngAfterViewInit = function () {
        if (this.SuperCustomMessageWrapperComponent == null) {
            console.warn("SuperCustomMessageWrapperComponent.ngAfterViewInit MyCustomMessageWrapperComponent == null");
        }
        else {
            console.log("SuperCustomMessageWrapperComponent.ngAfterViewInit MyCustomMessageWrapperComponent != null");
        }
        this.MyCustomMessageWrapperComponent = this.SuperCustomMessageWrapperComponent;
        this.subscribeWrapperComponent();
    };
    CargoQueryRequestComponent.prototype.SetMenuArg = function (MenuArg) {
        if (MenuArg != null && MenuArg.Mode == "SendCargoQueryRequestFromDeclaration") {
            this._IsFromDeclaration = true;
            this.SendCargoQueryRequestFromDeclaration(MenuArg);
        }
        else {
            this.RequestParams = MenuArg;
            this.OnMassageDisplayMethod();
        }
    };
    CargoQueryRequestComponent.prototype.SendCargoQueryRequestFromDeclaration = function (MenuArg) {
        this.OnMassageDisplayMethod();
        this.CargoTypeCode = MenuArg.CargoTypeCode;
        this.ManifestNumber = MenuArg.ManifestNumber;
        this.SecondCargoID = MenuArg.SecondCargoID;
        this.DeclarationId = MenuArg.DeclarationId;
        this.UIProperties.SetEnabled("CustomFileNo", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("CargoTypeCode", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("ManifestNumber", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("SecondCargoID", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("ThirdCargoID", this.ObjectTableName, false);
        //this.CustomSendOptionsButtonIsDisable = true;
        var customSendOptionsArgs = new RequestParamsBase_1.CustomSendOptionsArgs();
        customSendOptionsArgs.ForcePersonalSign = false;
        customSendOptionsArgs.RequestVIA = RequestParamsBase_1.SendRequestVIA.WebServiceInteractive;
        this.OnCustomSendOptionsButtonClick(customSendOptionsArgs);
    };
    CargoQueryRequestComponent.prototype.DueChangeClearChildField = function (sourceIsCostomFile) {
        this.UIProperties.SetValidity("CustomFileNo", this.ObjectTableName, true, "");
        this.UIProperties.SetValidity("DeclarationNumber", this.ObjectTableName, true, "");
        if (sourceIsCostomFile) {
            this.DeclarationNumber = "";
        }
        else {
            this.CustomFileNo = "";
        }
        this.DeclarationId = "";
        this.ResponseData = null;
        this._LastFetchConsignmentPMList = null;
        this.ValidationErrorsList = [];
    };
    CargoQueryRequestComponent.prototype.CustomFileNoTextChanged = function (searchtext) {
        var _this = this;
        if (Tools_1.AppTool.IsNullOrEmpty(this.CustomFileNo)) {
            this.NoConnectedConsignmentEnableField();
        }
        else {
            this.CurrentSession.StartBusyIndicator("");
            this._DeclarationExtendedListService.GetSingleDeclarationByCustomFileNo(this.CustomFileNo)
                .subscribe(function (myDeclarationResponse) {
                _this.CurrentSession.StopBusyIndicator();
                _this._LastFetchDeclarationList = myDeclarationResponse.Result;
                if (Tools_1.AppTool.IsNullOrEmpty(_this._LastFetchDeclarationList)) {
                    _this.NoConnectedConsignmentEnableField();
                }
                else {
                    _this.CurrentSession.StartBusyIndicator("");
                    _this._DeclarationExtendedListService.GetConsignmentListPMByCustomFileNo(_this.CustomFileNo)
                        .subscribe(function (myResponse) {
                        _this.CurrentSession.StopBusyIndicator();
                        _this.FetchConsignment(myResponse, false);
                    });
                }
            });
        }
    };
    CargoQueryRequestComponent.prototype.NoConnectedConsignmentEnableField = function () {
        this.DeclarationId = "";
        this.DeclarationNumber = "";
        this.CargoTypeCode = "";
        this.ManifestNumber = "";
        this.SecondCargoID = "";
        this.ThirdCargoID = "";
        //this.ResponseStatusXML = "";
        this.UIProperties.SetEnabled("ManifestNumber", this.ObjectTableName, true);
        this.UIProperties.SetEnabled("CargoTypeCode", this.ObjectTableName, true);
        this.UIProperties.SetEnabled("SecondCargoID", this.ObjectTableName, true);
        this.UIProperties.SetEnabled("ThirdCargoID", this.ObjectTableName, true);
    };
    CargoQueryRequestComponent.prototype.FetchConsignment = function (myResponse, sourceIsCostomFile) {
        this._LastFetchConsignmentPMList = myResponse.Result;
        if (this._LastFetchConsignmentPMList != null
        //&& this._LastFetchConsignmentPMList.length > 0
        ) {
            var pm = this._LastFetchConsignmentPMList[0];
            this.DeclarationId = pm.DeclarationId;
            this.CargoTypeCode = pm.CargoTypeCode;
            this.ManifestNumber = pm.ManifestNumber;
            this.SecondCargoID = pm.SecondCargoID;
            this.ThirdCargoID = pm.ThirdCargoID;
            this.UIProperties.SetEnabled("ManifestNumber", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("CargoTypeCode", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("SecondCargoID", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("ThirdCargoID", this.ObjectTableName, false);
            this.DeclarationNumber = this._LastFetchDeclarationList.DeclarationNumber;
            //this.CurrentSession.StartBusyIndicator("")
            //this._DeclarationExtendedListService.GetSingleDeclarationByNumber(this.CustomFileNo)
            //    .subscribe((myResponse: ServiceResponse) => {
            //        this.CurrentSession.StopBusyIndicator();
            //        this.FetchConsignment(myResponse, false);
            //    });
        }
        else {
            this.NoConnectedConsignmentEnableField();
        }
    };
    CargoQueryRequestComponent.prototype.CargoTypeCodeValueChanged = function (paramValueChanged) {
        console.log(paramValueChanged);
        this.GetCustomFileNo();
    };
    CargoQueryRequestComponent.prototype.ManifestNumberTextChanged = function (param) {
        this.GetCustomFileNo();
    };
    CargoQueryRequestComponent.prototype.SecondCargoIDTextChanged = function (param) {
        if (Tools_1.AppTool.IsNullOrEmpty(this.SecondCargoID)) {
            return;
        }
        if (this.SecondCargoID.length == 9) // moran 10.5.15 -  Bug 9749
         {
            if (this.SecondCargoID.substring(0, 1) != "I") {
                this.SecondCargoID = "I" + this.SecondCargoID;
            }
        }
        this.GetCustomFileNo();
    };
    CargoQueryRequestComponent.prototype.GetCustomFileNo = function () {
        var _this = this;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.ManifestNumber) && !Tools_1.AppTool.IsNullOrEmpty(this.SecondCargoID)) {
            if (!Tools_1.AppTool.IsNullOrEmpty(this.CargoTypeCode)) {
                //LoadOperation op2 = this.context.Load(this.context.GetSingleDeclarationPMByCargoIdentifiersQuery(this.CargoTypeCode, this.ManifestNumber, this.SecondCargoID, TenantContext.Current.Id), LoadBehavior.RefreshCurrent, true);
                //op2.Completed += CargoTypeCodeLostFocus_Completed;
                this._DeclarationExtendedListService.GetSingleDeclarationPMByCargoIdentifiers(this.CargoTypeCode, this.ManifestNumber, this.SecondCargoID, SessionLocator_1.SessionLocator.Tenant)
                    .subscribe(function (myResponse) {
                    var myDeclarationPMList = myResponse.Result;
                    if (myDeclarationPMList) {
                        if (myDeclarationPMList[0]) {
                            _this.CustomFileNo = myDeclarationPMList[0].CustomFileNo;
                            _this.DeclarationNumber = myDeclarationPMList[0].DeclarationNumber;
                            _this.DeclarationId = myDeclarationPMList[0].Id;
                        }
                    }
                });
            }
        }
    };
    Object.defineProperty(CargoQueryRequestComponent.prototype, "CustomFileNo", {
        get: function () { return this.RequestParams ? this.RequestParams.CustomsFile : null; },
        set: function (value) {
            if (this.RequestParams.CustomsFile != value) {
                //this.UIProperties.SetValidity("CustomFileNo", "Customs.Declaration", true, TextCodeTranslator.Translate("Customs.Declaration.O.Didntfindcustomfile"));
                this.RequestParams.CustomsFile = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CargoQueryRequestComponent.prototype, "DeclarationNumber", {
        get: function () { return this.RequestParams ? this.RequestParams.DeclarationNumber : null; },
        set: function (value) {
            if (this.RequestParams.DeclarationNumber != value) {
                this.RequestParams.DeclarationNumber = value;
                if (value) {
                    //     this.UIProperties.SetRequired("DeclarationNumber", this.ObjectTableName, false);
                }
            }
            else {
                //this.UIProperties.SetRequired("DeclarationNumber", this.ObjectTableName, true);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CargoQueryRequestComponent.prototype, "DeclarationId", {
        get: function () { return this.RequestParams ? this.RequestParams.DeclarationId : null; },
        set: function (value) {
            if (this.RequestParams.DeclarationId != value) {
                this.RequestParams.DeclarationId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CargoQueryRequestComponent.prototype, "CargoTypeCode", {
        get: function () { return this.RequestParams ? this.RequestParams.CargoTypeCode : null; },
        set: function (value) {
            {
                if (this.RequestParams.CargoTypeCode != value) {
                    this.RequestParams.CargoTypeCode = value;
                    //FirePropertyChanged("CargoTypeCode");
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CargoQueryRequestComponent.prototype, "ManifestNumber", {
        get: function () { return this.RequestParams ? this.RequestParams.ManifestNumber : null; },
        set: function (value) {
            if (this.RequestParams.ManifestNumber != value) {
                this.RequestParams.ManifestNumber = value;
                //FirePropertyChanged("ManifestNumber");
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CargoQueryRequestComponent.prototype, "SecondCargoID", {
        get: function () { return this.RequestParams ? this.RequestParams.SecondCargoID : null; },
        set: function (value) {
            this.RequestParams.SecondCargoID = value;
            //FirePropertyChanged("SecondCargoID");
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CargoQueryRequestComponent.prototype, "ThirdCargoID", {
        get: function () { return this.RequestParams ? this.RequestParams.ThirdCargoID : null; },
        set: function (value) {
            if (this.RequestParams.ThirdCargoID != value) {
                this.RequestParams.ThirdCargoID = value;
                //FirePropertyChanged("ThirdCargoID");
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CargoQueryRequestComponent.prototype, "IsShowUserMessage", {
        get: function () {
            return this.ResponseData ? this.ResponseData.IsShowUserMessage : false;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CargoQueryRequestComponent.prototype, "UserMessage", {
        get: function () {
            return this.ResponseData ? this.ResponseData.UserMessage : null;
        },
        set: function (value) { },
        enumerable: true,
        configurable: true
    });
    CargoQueryRequestComponent.prototype.RefreshScreen = function () {
        this._MyResponseObjectToShow = null;
        this._UserMessagehidden = true;
        if (this.ResponseData == null) {
            return;
        }
        this._UserMessagehidden = !this.ResponseData.IsShowUserMessage;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.ResponseData.ResponseStatusXML)) {
            this.UIProperties.SetRequired("DeclarationNumber", this.ObjectTableName, false);
            try {
                this._MyResponseObjectToShow = JSON.parse(this.ResponseData.ResponseStatusXML);
            }
            catch (err) {
                console.log(err);
            }
        }
    };
    CargoQueryRequestComponent.prototype.OnMassageDisplayMethod = function () {
        if (this.RequestParams == null) {
            this.RequestParams = new CargoQueryRequestParams_1.CargoQueryRequestParams();
        }
        if (this.ResponseData == null) {
            this.ResponseData = new CargoQueryResponseData_1.CargoQueryResponseData();
        }
        else {
            if (this.ResponseData.DeliveryOrderResultList) {
                this.DeliveryOrderResultList.InsertCollection(this.ResponseData.DeliveryOrderResultList);
            }
            if (this.ResponseData.CargosVersionResultList) {
                this.CargosVersionResultList.InsertCollection(this.ResponseData.CargosVersionResultList);
            }
            if (this.ResponseData.CargoItemResultList) {
                this.CargoItemResultList.InsertCollection(this.ResponseData.CargoItemResultList);
            }
        }
        this.RefreshScreen();
    };
    CargoQueryRequestComponent.prototype.FillError = function () {
        var errors = [];
        Validator_1.Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        if (Tools_1.AppTool.IsNullOrEmpty(this.CargoTypeCode) || Tools_1.AppTool.IsNullOrEmpty(this.ManifestNumber)) //eitan h 4/3/15 task 11484
         {
            errors.push(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.CargoDataMissing"));
        }
        this.ValidationErrorsList = errors;
    };
    CargoQueryRequestComponent.prototype.OnCustomSendOptionsButtonClick = function (customSendOptionsArgs) {
        var _this = this;
        this.FillError();
        if (this.ValidationErrorsList.length > 0) {
            return;
        }
        if (this.ValidationErrorsList.length > 0) {
            return;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.CustomFileNo)) {
            var declarationDisplayOnlyChecks = new DeclarationDisplayOnlyChecks_1.DeclarationDisplayOnlyChecks();
            declarationDisplayOnlyChecks.CheckIfRequestInProgress("2715", this.CustomFileNo, SessionLocator_1.SessionLocator.Tenant)
                .subscribe(function (response) {
                if (!response.HasError) {
                    var requestSheets = response.Result;
                    var haveRS2715 = false;
                    if ((requestSheets == null || requestSheets.length == 0)
                        || (requestSheets != null && requestSheets.length == 1 && requestSheets[0].InterfaceTypeCode == null)) {
                        haveRS2715 = false;
                    }
                    else {
                        haveRS2715 = true;
                    }
                    if (haveRS2715) {
                        var messageWindow = new MessageWindow_1.MessageWindow();
                        messageWindow.Width = 400;
                        messageWindow.Height = 150;
                        messageWindow.Title = "שיחזור מספר הצהרה";
                        messageWindow.Show("לא ניתן לשחזר מספר הצהרה ,קיימת בקשה מסוג הצהרה בתהליך ");
                        return;
                    }
                    declarationDisplayOnlyChecks.CheckIfRequestInProgress("2755", _this.CustomFileNo, SessionLocator_1.SessionLocator.Tenant)
                        .subscribe(function (response) {
                        if (!response.HasError) {
                            var requestSheets = response.Result;
                            var haveRS2755 = false;
                            if ((requestSheets == null || requestSheets.length == 0)
                                || (requestSheets != null && requestSheets.length == 1 && requestSheets[0].InterfaceTypeCode == null)) {
                                haveRS2755 = false;
                            }
                            else {
                                haveRS2755 = true;
                            }
                            if (haveRS2755) {
                                var messageWindow = new MessageWindow_1.MessageWindow();
                                messageWindow.Width = 400;
                                messageWindow.Height = 150;
                                messageWindow.Title = "שיחזור מספר הצהרה";
                                messageWindow.Show("לא ניתן לשחזר מספר הצהרה ,קיימת בקשה מסוג הגשת תשלום ");
                                return;
                            }
                            _this.SendCargoQueryRequest(customSendOptionsArgs);
                        }
                    });
                }
            });
        }
        else {
            this.SendCargoQueryRequest(customSendOptionsArgs);
        }
    };
    CargoQueryRequestComponent.prototype.SendCargoQueryRequest = function (customSendOptionsArgs) {
        var _this = this;
        var currRequestParams = new CargoQueryRequestParams_1.CargoQueryRequestParams(); ///Force new GUID On Each Send !!
        currRequestParams.LoggingEnabled = true;
        currRequestParams.LoggingUserId = SessionLocator_1.SessionLocator.LoggedUserId;
        currRequestParams.Tenant = SessionLocator_1.SessionLocator.Tenant;
        currRequestParams.RequestVIA = customSendOptionsArgs.RequestVIA;
        currRequestParams.ForcePersonalSign = customSendOptionsArgs.ForcePersonalSign;
        currRequestParams.AppicationId = this.RequestParams.DeclarationId;
        currRequestParams.DeclarationId = this.RequestParams.DeclarationId;
        currRequestParams.DeclarationNumber = this.RequestParams.DeclarationNumber;
        currRequestParams.CustomsFile = this.RequestParams.CustomsFile;
        currRequestParams.CargoTypeCode = this.RequestParams.CargoTypeCode;
        currRequestParams.ManifestNumber = this.RequestParams.ManifestNumber;
        currRequestParams.SecondCargoID = this.RequestParams.SecondCargoID;
        currRequestParams.ThirdCargoID = this.RequestParams.ThirdCargoID;
        currRequestParams.RequestName = "Manifest Status Query";
        currRequestParams.ResponseName = "Manifest Status Query";
        CustomMessageProgressComponent_1.CustomMessageProgressComponent
            .ShowProgressBar(currRequestParams.PBId, "שליחת שאילתא למצהר", true)
            .then(function (res) {
            _this.ResponseData = res;
            if (_this._IsFromDeclaration && _this.ResponseData.HasException == false) {
                if (_this.CurrentSession.CurrentEditComponent != null) {
                    _this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                }
            }
            _this.OnMassageDisplayMethod();
        }).catch(function (err) {
            _this.ValidationErrorsList.push(err);
        });
        this._IIGGeneralMessagesService.PostCargoQueryRequestParams(currRequestParams)
            .subscribe(function (myServiceResponse) {
        });
    };
    Object.defineProperty(CargoQueryRequestComponent.prototype, "CargoQueryResponseData", {
        get: function () {
            if (this.ResponseData) {
                var my = this.ResponseData;
                if (my) {
                }
                else {
                    my = new CargoQueryResponseData_1.CargoQueryResponseData();
                }
                if (Tools_1.AppTool.IsNullOrEmpty(my.CargoResultList)) {
                    my.CargoResultList = new CargoQueryResponseData_1.CargoResult();
                }
                if (Tools_1.AppTool.IsNullOrEmpty(my.CargoResultList.CargoAdditionalDataList)) {
                    my.CargoResultList.CargoAdditionalDataList = new CargoQueryResponseData_1.CargoAdditionalData();
                }
                return my;
            }
            return null;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CargoQueryRequestComponent.prototype, "ManifestTypeName", {
        get: function () { return this.CargoQueryResponseData ? this.CargoQueryResponseData.ManifestTypeName : null; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CargoQueryRequestComponent.prototype, "ManifestStatusName", {
        get: function () { return this.CargoQueryResponseData ? this.CargoQueryResponseData.ManifestStatusName : null; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CargoQueryRequestComponent.prototype, "ResponseManifestNumber", {
        get: function () { return this.CargoQueryResponseData ? this.CargoQueryResponseData.Manifestnumber : null; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CargoQueryRequestComponent.prototype, "CargoIdentifierKey1", {
        get: function () { return this.CargoQueryResponseData ? this.CargoQueryResponseData.CargoResultList.CargoIdentifierKey1 : null; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CargoQueryRequestComponent.prototype, "ParentCargoID", {
        get: function () { return this.CargoQueryResponseData ? this.CargoQueryResponseData.CargoResultList.ParentCargoID : null; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CargoQueryRequestComponent.prototype, "UnloadingLocationName", {
        get: function () { return this.CargoQueryResponseData ? this.CargoQueryResponseData.CargoResultList.CargoAdditionalDataList.UnloadingLocationName : null; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CargoQueryRequestComponent.prototype, "TotalNumberOfPackeges", {
        get: function () { return this.CargoQueryResponseData ? this.CargoQueryResponseData.CargoResultList.TotalNumberOfPackeges : null; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CargoQueryRequestComponent.prototype, "TotalRecordNumberOfPackeges", {
        get: function () { return this.CargoQueryResponseData ? this.CargoQueryResponseData.CargoResultList.TotalNumberOfPackeges : null; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CargoQueryRequestComponent.prototype, "GovernmentProcedureTypeName", {
        get: function () { return this.CargoQueryResponseData ? this.CargoQueryResponseData.CargoResultList.GovernmentProcedureTypeName : null; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CargoQueryRequestComponent.prototype, "TreatmentWayName", {
        get: function () { return this.CargoQueryResponseData ? this.CargoQueryResponseData.CargoResultList.TreatmentWayName : null; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CargoQueryRequestComponent.prototype, "GoodsReceiptPlaceSiteName", {
        get: function () { return this.CargoQueryResponseData ? this.CargoQueryResponseData.CargoResultList.CargoAdditionalDataList.GoodsReceiptPlaceSiteName : null; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CargoQueryRequestComponent.prototype, "TotalWeight", {
        get: function () { return this.CargoQueryResponseData ? this.CargoQueryResponseData.CargoResultList.TotalWeight : null; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CargoQueryRequestComponent.prototype, "TotalRecordWeight", {
        get: function () { return this.CargoQueryResponseData ? this.CargoQueryResponseData.CargoResultList.CargoAdditionalDataList.TotalRecordWeight : null; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CargoQueryRequestComponent.prototype, "MasterBolNumber", {
        get: function () { return this.CargoQueryResponseData ? this.CargoQueryResponseData.CargoResultList.MasterBolNumber : null; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CargoQueryRequestComponent.prototype, "BillOfLadingNumber", {
        get: function () { return this.CargoQueryResponseData ? this.CargoQueryResponseData.CargoResultList.BillOfLadingNumber : null; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CargoQueryRequestComponent.prototype, "TransitDestinationLocationName", {
        get: function () { return this.CargoQueryResponseData ? this.CargoQueryResponseData.CargoResultList.CargoAdditionalDataList.TransitDestinationLocationName : null; },
        enumerable: true,
        configurable: true
    });
    CargoQueryRequestComponent.prototype.OnRowLoaded = function (Row) {
        var isExpandaple = false;
        if (Row) {
            var item = Row.rowData;
            if (!Tools_1.AppTool.IsNullOrEmpty(item.SealDetailsList) && item.SealDetailsList.length > 0) {
                item.SealDetailsObservableCollection = new ObservableCollection_1.ObservableCollection([]);
                item.SealDetailsObservableCollection.InsertCollection(item.SealDetailsList);
                isExpandaple = true;
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(item.CargoMovmentList) && item.CargoMovmentList.length > 0) {
                item.CargoMovmentObservableCollection = new ObservableCollection_1.ObservableCollection([]);
                item.CargoMovmentObservableCollection.InsertCollection(item.CargoMovmentList);
                isExpandaple = true;
            }
            if (isExpandaple) {
                item.CargoMovmentObservableCollection = item.CargoMovmentObservableCollection || new ObservableCollection_1.ObservableCollection([]);
                item.SealDetailsObservableCollection = item.SealDetailsObservableCollection || new ObservableCollection_1.ObservableCollection([]);
            }
            Row.SetExpandaple(isExpandaple);
        }
    };
    CargoQueryRequestComponent.prototype.CancelButtonClicked = function () {
        if (this._IsFromDeclaration) {
            if (this.ValidationErrorsList.length) {
                this.CurrentSession.CloseCurrentWindowEmit("");
            }
            else {
                this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                this.CurrentSession.CloseCurrentWindowEmit("ReloadEntity");
            }
        }
        else {
            this.CurrentSession.CloseCurrentWindow();
        }
    };
    __decorate([
        core_1.ViewChild(CustomMessageWrapperComponent_1.CustomMessageWrapperComponent),
        __metadata("design:type", CustomMessageWrapperComponent_1.CustomMessageWrapperComponent)
    ], CargoQueryRequestComponent.prototype, "SuperCustomMessageWrapperComponent", void 0);
    CargoQueryRequestComponent = __decorate([
        core_1.Component({
            selector: 'CargoQueryRequestComponent',
            moduleId: module.id,
            templateUrl: './CargoQueryRequestComponent.html',
        }),
        __metadata("design:paramtypes", [EntityResourceService_1.EntityResourceService])
    ], CargoQueryRequestComponent);
    return CargoQueryRequestComponent;
}(BaseRequestsSheetMassaging_1.BaseRequestsSheetMassaging));
exports.CargoQueryRequestComponent = CargoQueryRequestComponent;
//# sourceMappingURL=CargoQueryRequestComponent.js.map