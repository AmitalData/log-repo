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
var BaseComponent_1 = require("../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var DeclarationExtendedListService_1 = require("../../../Customs/Services/ExtendedLists/DeclarationExtendedListService");
var IIGGeneralMessagesService_1 = require("../../../Customs/Services/WebServices/IIGGeneralMessagesService");
var SpecialActivityRequestParams_1 = require("../../../Customs/DataContract/RequestParams/SpecialActivityRequestParams");
var Validator_1 = require("../../../Infrastructure/Validators/Validator");
var TextCodeTranslator_1 = require("../../../Infrastructure/Utilities/TextCodeTranslator");
var Tools_1 = require("../../../Infrastructure/Tools");
var BaseRequestsSheetMassaging_1 = require("../../../CustomsModules/CustomsRequests/Components/BaseRequestsSheetMassaging");
var CustomMessageProgressComponent_1 = require("../../../CustomsModules/CustomsControls/Components/CustomMessageProgressComponent");
var ObservableCollection_1 = require("../../../Infrastructure/Utilities/ObservableCollection");
var CustomsSettingListService_1 = require("../../../Customs/Services/StandardLists/CustomsSettingListService");
var ConfirmWindow_1 = require("../../../Controls/Windows/ConfirmWindow");
var LuhnAlgorithm_1 = require("../../../Customs/Utilities/LuhnAlgorithm");
var ApiQueryFilters_1 = require("../../../Infrastructure/DataContracts/ApiQueryFilters");
var EntityResourceService_1 = require("../../../Infrastructure/Services/EntityResourceService");
var SpecialActivityRequestComponent = /** @class */ (function (_super) {
    __extends(SpecialActivityRequestComponent, _super);
    function SpecialActivityRequestComponent(EntityResourceService) {
        var _this = _super.call(this) || this;
        _this.EntityResourceService = EntityResourceService;
        _this.DataContext = _this;
        _this.ObjectTableName = "Customs.Declaration";
        _this._IsGoodsDetailsData = true;
        _this._IsOtherActivity = false;
        _this._IsRePackingApproval = false;
        _this._IsSampleRequest = false;
        _this._IsResponseMessageVisibility = false;
        _this._IsLoadResponseData = true;
        _this._DeclarationExtendedListService = new DeclarationExtendedListService_1.DeclarationExtendedListService();
        _this._IIGGeneralMessagesService = new IIGGeneralMessagesService_1.IIGGeneralMessagesService();
        _this._CustomsSettingListService = new CustomsSettingListService_1.CustomsSettingListService();
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.SuperCustomMessageWrapperComponent = new CustomMessageWrapperComponent_1.CustomMessageWrapperComponent();
        _this.RepresentativeList = new ObservableCollection_1.ObservableCollection([]);
        _this.RepackingCurrentList = new ObservableCollection_1.ObservableCollection([]);
        _this.RepackingDesiredList = new ObservableCollection_1.ObservableCollection([]);
        _this.SampleRequestList = new ObservableCollection_1.ObservableCollection([]);
        _this.EntityResourceService.getEntityResourceByTableName("Customs.Claim").subscribe(function (response) {
        });
        return _this;
    }
    SpecialActivityRequestComponent.prototype.ngAfterViewInit = function () {
        if (this.SuperCustomMessageWrapperComponent == null) {
            console.warn("SuperCustomMessageWrapperComponent.ngAfterViewInit MyCustomMessageWrapperComponent == null");
        }
        else {
            console.log("SuperCustomMessageWrapperComponent.ngAfterViewInit MyCustomMessageWrapperComponent != null");
        }
        this.MyCustomMessageWrapperComponent = this.SuperCustomMessageWrapperComponent;
        this.subscribeWrapperComponent();
    };
    SpecialActivityRequestComponent.prototype.OnMassageDisplayMethod = function () {
        var _this = this;
        this.StorageFilterItems = new ApiQueryFilters_1.ApiQueryFilters();
        this.StorageFilterItems.addAdditionalFilter("Code", "1,2,3,4,8,9,10,11,12", null, null, "Exclude", false, false, false, "string", false, true);
        this._TodayDate = Tools_1.DateTool.GetCurrentDateTimeAsUtc();
        if (this.RequestParams == null) {
            this.RequestParams = new SpecialActivityRequestParams_1.SpecialActivityRequestParams();
            this.RequestParams.GeneralDetailsData = new SpecialActivityRequestParams_1.GeneralDetails();
            this.RequestParams.GeneralDetailsData.CargoIdentifier = new SpecialActivityRequestParams_1.CargoIdentifier();
            this.RequestParams.GoodsDetailsData = new SpecialActivityRequestParams_1.GoodsDetails();
            this.RequestParams.RePackingApprovalDetailsData = new SpecialActivityRequestParams_1.RePackingApprovalDetails();
            this.SpecialActivityType = "6";
            if (this._CustomsSettingListService == null) {
                this._CustomsSettingListService = new CustomsSettingListService_1.CustomsSettingListService();
            }
            this._CustomsSettingListService.getSingleFromCache(SessionLocator_1.SessionLocator.Tenant.toString())
                .subscribe(function (customsSettingList) {
                if (customsSettingList != null) {
                    _this.ApplicantAgentNumber = customsSettingList.Result ? customsSettingList.Result.CustomsAgentId : null;
                    _this.UIProperties.SetEnabled("ApplicantAgentNumber", null, false);
                }
            });
            this.UIProperties.SetRequired("ActivityRequestStartDate", null, true);
            this.UIProperties.SetRequired("ActivityRequestEndDate", null, true);
            this.UIProperties.SetRequired("SiteNumber", null, true);
            this.UIProperties.SetRequired("CargoIdentifierType", null, true);
            this.UIProperties.SetRequired("CargoIdentifierKey1", null, true);
        }
        if (this.ResponseData) {
            this.IsResponseMessageVisibility = true;
            if (this._IsLoadResponseData) {
                this.LoadResponseData();
            }
        }
    };
    SpecialActivityRequestComponent.prototype.SetMenuArg = function (MenuArg) {
        if (MenuArg) {
            this.OnMassageDisplayMethod();
            this.CustomFileNo = MenuArg.CustomFileNo;
            this.DeclarationId = MenuArg.DeclarationId;
            this.CustomFileNoTextChanged("");
        }
    };
    SpecialActivityRequestComponent.prototype.LoadResponseData = function () {
        var _this = this;
        if (this.ResponseData) {
            this.IsResponseMessageVisibility = true;
            if (this.RequestParams.GeneralDetailsData != null && this.RequestParams.GeneralDetailsData.CargoIdentifier != null) {
                this.CargoIdentifierType = this.RequestParams.GeneralDetailsData.CargoIdentifier.CargoIdentifierType.toString();
            }
            switch (this.RequestParams.GeneralDetailsData.SpecialActivityType) {
                case "5": //RePackingApproval
                case 5:
                    this.SpecialActivityType = "5";
                    this.SetIsRePackingApproval(true);
                    if (this.RequestParams.CurrentPackingDetailsDataList != null && this.RequestParams.CurrentPackingDetailsDataList != null) {
                        this.RequestParams.CurrentPackingDetailsDataList.forEach(function (item) {
                            _this.RepackingCurrentList.Insert(new RepackingCurrentRequestDetailsComponent(item));
                        });
                    }
                    if (this.RequestParams.DesiredPackingDetailsDataList != null && this.RequestParams.DesiredPackingDetailsDataList != null) {
                        this.RequestParams.DesiredPackingDetailsDataList.forEach(function (item) {
                            _this.RepackingDesiredList.Insert(new RepackingDesiredRequestDetailsComponent(item));
                        });
                    }
                    break;
                case "6": //GoodsDetails 
                case 6:
                    this.SpecialActivityType = "6";
                    this.SetIsGoodsDetailsData(true);
                    if (this.RequestParams.GoodsDetailsData != null) {
                        if (this.RequestParams.GoodsDetailsData.SpecialActionsCode != null) {
                            this.SpecialActionsCode = this.RequestParams.GoodsDetailsData.SpecialActionsCode.toString();
                        }
                        if (this.RequestParams.GoodsDetailsData.RepresentativeList != null) {
                            this.RequestParams.GoodsDetailsData.RepresentativeList.forEach(function (item) {
                                _this.RepresentativeList.Insert(new RepresentativeComponent(item));
                            });
                        }
                    }
                    break;
                case "7": //SampleRequest 
                case 7:
                    this.SpecialActivityType = "7";
                    this.SetIsSampleRequest(true);
                    if (this.RequestParams.SampleRequestDetailsDataList != null && this.RequestParams.SampleRequestDetailsDataList != null) {
                        this.RequestParams.SampleRequestDetailsDataList.forEach(function (item) {
                            _this.SampleRequestList.Insert(new SampleRequestDetailsComponent(item));
                        });
                    }
                    break;
                case "13": //Other
                case 13:
                    this.SpecialActivityType = "13";
                    this.SetIsOtherActivity(true);
                    break;
                default:
                    break;
            }
        }
    };
    Object.defineProperty(SpecialActivityRequestComponent.prototype, "SpecialActivityType", {
        get: function () { return this._SpecialActivityType; },
        set: function (value) {
            if (this._SpecialActivityType != value) {
                this._SpecialActivityType = value;
            }
            switch (value) {
                case "5": //RePackingApproval
                    this.SetIsRePackingApproval(true);
                    break;
                case "6": //GoodsDetails 
                    this.SetIsGoodsDetailsData(true);
                    break;
                case "7": //SampleRequest 
                    this.SetIsSampleRequest(true);
                    break;
                case "13": //Other
                    this.SetIsOtherActivity(true);
                    break;
                default:
                    break;
            }
            this.SetWarning(value);
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SpecialActivityRequestComponent.prototype, "ApplicantAgentNumber", {
        get: function () { return this.RequestParams.GeneralDetailsData.ApplicantAgentNumber; },
        set: function (value) {
            if (this.RequestParams.GeneralDetailsData.ApplicantAgentNumber != value) {
                this.RequestParams.GeneralDetailsData.ApplicantAgentNumber = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SpecialActivityRequestComponent.prototype, "SpecialActivityRequestNumber", {
        get: function () { return this.RequestParams.GeneralDetailsData.SpecialActivityRequestNumber; },
        set: function (value) {
            if (this.RequestParams.GeneralDetailsData.SpecialActivityRequestNumber != value) {
                this.RequestParams.GeneralDetailsData.SpecialActivityRequestNumber = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SpecialActivityRequestComponent.prototype, "ActivityRequestStartDate", {
        get: function () { return this.RequestParams.GeneralDetailsData.ActivityRequestStartDate; },
        set: function (value) {
            if (this.RequestParams.GeneralDetailsData.ActivityRequestStartDate != value) {
                this.RequestParams.GeneralDetailsData.ActivityRequestStartDate = value;
                this.ActivityRequestStartDateLostFocusMethod(value);
            }
            if (value && !Tools_1.AppTool.IsNullOrEmpty(this.ActivityRequestStartTime)) {
                this.UIProperties.SetRequired("ActivityRequestStartDate", null, false);
            }
            else {
                this.UIProperties.SetRequired("ActivityRequestStartDate", null, true);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SpecialActivityRequestComponent.prototype, "ActivityRequestStartTime", {
        get: function () { return this.RequestParams.GeneralDetailsData.ActivityRequestStartTime ? this.RequestParams.GeneralDetailsData.ActivityRequestStartTime : null; },
        set: function (value) {
            if (this.RequestParams.GeneralDetailsData.ActivityRequestStartTime != value) {
                this.RequestParams.GeneralDetailsData.ActivityRequestStartTime = value;
            }
            if (value && !Tools_1.AppTool.IsNullOrEmpty(this.ActivityRequestStartDate)) {
                this.UIProperties.SetRequired("ActivityRequestStartDate", null, false);
            }
            else {
                this.UIProperties.SetRequired("ActivityRequestStartDate", null, true);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SpecialActivityRequestComponent.prototype, "ActivityRequestEndDate", {
        get: function () { return this.RequestParams.GeneralDetailsData.ActivityRequestEndDate; },
        set: function (value) {
            if (this.RequestParams.GeneralDetailsData.ActivityRequestEndDate != value) {
                this.RequestParams.GeneralDetailsData.ActivityRequestEndDate = value;
                this.ActivityRequestEndDateLostFocusMethod(value);
            }
            if (value && !Tools_1.AppTool.IsNullOrEmpty(this.ActivityRequestEndTime)) {
                this.UIProperties.SetRequired("ActivityRequestEndDate", null, false);
            }
            else {
                this.UIProperties.SetRequired("ActivityRequestEndDate", null, true);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SpecialActivityRequestComponent.prototype, "ActivityRequestEndTime", {
        get: function () { return this.RequestParams.GeneralDetailsData.ActivityRequestEndTime; },
        set: function (value) {
            if (this.RequestParams.GeneralDetailsData.ActivityRequestEndTime != value) {
                this.RequestParams.GeneralDetailsData.ActivityRequestEndTime = value;
            }
            if (value && !Tools_1.AppTool.IsNullOrEmpty(this.ActivityRequestEndDate)) {
                this.UIProperties.SetRequired("ActivityRequestEndDate", null, false);
            }
            else {
                this.UIProperties.SetRequired("ActivityRequestEndDate", null, true);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SpecialActivityRequestComponent.prototype, "CustomFileNo", {
        get: function () { return this.RequestParams.GeneralDetailsData.CustomFileNo; },
        set: function (value) {
            if (this.RequestParams.GeneralDetailsData.CustomFileNo != value) {
                this.RequestParams.GeneralDetailsData.CustomFileNo = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SpecialActivityRequestComponent.prototype, "DeclarationId", {
        get: function () { return this.RequestParams.GeneralDetailsData.DeclarationId; },
        set: function (value) {
            if (this.RequestParams.GeneralDetailsData.DeclarationId != value) {
                this.RequestParams.GeneralDetailsData.DeclarationId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SpecialActivityRequestComponent.prototype, "SiteNumber", {
        get: function () { return this.RequestParams.GeneralDetailsData.SiteNumber; },
        set: function (value) {
            if (this.RequestParams.GeneralDetailsData.SiteNumber != value) {
                this.RequestParams.GeneralDetailsData.SiteNumber = value;
            }
            if (value) {
                this.UIProperties.SetRequired("SiteNumber", null, false);
            }
            else {
                this.UIProperties.SetRequired("SiteNumber", null, true);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SpecialActivityRequestComponent.prototype, "WarehouseBlockNumber", {
        get: function () { return this.RequestParams.GeneralDetailsData.WarehouseBlockNumber; },
        set: function (value) {
            if (this.RequestParams.GeneralDetailsData.WarehouseBlockNumber != value) {
                this.RequestParams.GeneralDetailsData.WarehouseBlockNumber = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SpecialActivityRequestComponent.prototype, "CargoIdentifierType", {
        get: function () { return this._CargoIdentifierType; },
        set: function (value) {
            if (this._CargoIdentifierType != value) {
                this._CargoIdentifierType = value;
            }
            if (value) {
                this.UIProperties.SetRequired("CargoIdentifierType", null, false);
            }
            else {
                this.UIProperties.SetRequired("CargoIdentifierType", null, true);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SpecialActivityRequestComponent.prototype, "CargoIdentifierKey1", {
        get: function () { return this.RequestParams.GeneralDetailsData.CargoIdentifier.CargoIdentifierKey1; },
        set: function (value) {
            if (this.RequestParams.GeneralDetailsData.CargoIdentifier.CargoIdentifierKey1 != value) {
                this.RequestParams.GeneralDetailsData.CargoIdentifier.CargoIdentifierKey1 = value;
            }
            if (value) {
                this.UIProperties.SetRequired("CargoIdentifierKey1", null, false);
            }
            else {
                this.UIProperties.SetRequired("CargoIdentifierKey1", null, true);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SpecialActivityRequestComponent.prototype, "CargoIdentifierKey2", {
        get: function () { return this.RequestParams.GeneralDetailsData.CargoIdentifier.CargoIdentifierKey2; },
        set: function (value) {
            if (this.RequestParams.GeneralDetailsData.CargoIdentifier.CargoIdentifierKey2 != value) {
                this.RequestParams.GeneralDetailsData.CargoIdentifier.CargoIdentifierKey2 = value;
            }
            if (this.IsRePackingApproval || this.IsSampleRequest) {
                if (value) {
                    this.UIProperties.SetWarning("CargoIdentifierKey2", null, false);
                }
                else {
                    this.UIProperties.SetWarning("CargoIdentifierKey2", null, true);
                }
            }
            else {
                this.UIProperties.SetWarning("CargoIdentifierKey2", null, false);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SpecialActivityRequestComponent.prototype, "CargoIdentifierKey3", {
        get: function () { return this.RequestParams.GeneralDetailsData.CargoIdentifier.CargoIdentifierKey3; },
        set: function (value) {
            if (this.RequestParams.GeneralDetailsData.CargoIdentifier.CargoIdentifierKey3 != value) {
                this.RequestParams.GeneralDetailsData.CargoIdentifier.CargoIdentifierKey3 = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SpecialActivityRequestComponent.prototype, "CargoRowNumber", {
        get: function () { return this.RequestParams.GeneralDetailsData.CargoRowNumber; },
        set: function (value) {
            if (this.RequestParams.GeneralDetailsData.CargoRowNumber != value) {
                this.RequestParams.GeneralDetailsData.CargoRowNumber = value;
            }
            if (value || this.IsGoodsDetailsData == false) {
                this.UIProperties.SetWarning("CargoRowNumber", null, false);
            }
            else {
                this.UIProperties.SetWarning("CargoRowNumber", null, true);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SpecialActivityRequestComponent.prototype, "AuthorityCode", {
        get: function () { return this.RequestParams.GeneralDetailsData.AuthorityCode; },
        set: function (value) {
            if (this.RequestParams.GeneralDetailsData.AuthorityCode != value) {
                this.RequestParams.GeneralDetailsData.AuthorityCode = value;
            }
            if (value || this.IsSampleRequest == false) {
                this.UIProperties.SetWarning("AuthorityCode", null, false);
            }
            else {
                this.UIProperties.SetWarning("AuthorityCode", null, true);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SpecialActivityRequestComponent.prototype, "ImporterNumber", {
        get: function () { return this.RequestParams.GeneralDetailsData.ImporterNumber; },
        set: function (value) {
            if (this.RequestParams.GeneralDetailsData.ImporterNumber != value) {
                this.RequestParams.GeneralDetailsData.ImporterNumber = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    SpecialActivityRequestComponent.prototype.ImporterNumberTextChanged = function (code) {
        this.ImporterNumber = code;
        if (this.IsRePackingApproval || this.IsSampleRequest) {
            if (!Tools_1.AppTool.IsNullOrEmpty(this.ImporterNumber)) {
                this.UIProperties.SetWarning("ImporterNumber", "Customs.Client", false);
            }
            else {
                this.UIProperties.SetWarning("ImporterNumber", "Customs.Client", true);
            }
        }
    };
    Object.defineProperty(SpecialActivityRequestComponent.prototype, "CheckSite", {
        get: function () { return this.RequestParams.GeneralDetailsData.CheckSite; },
        set: function (value) {
            if (this.RequestParams.GeneralDetailsData.CheckSite != value) {
                this.RequestParams.GeneralDetailsData.CheckSite = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SpecialActivityRequestComponent.prototype, "SpecialActivityTypeEssence", {
        get: function () { return this.RequestParams.GeneralDetailsData.SpecialActivityTypeEssence; },
        set: function (value) {
            if (this.RequestParams.GeneralDetailsData.SpecialActivityTypeEssence != value) {
                this.RequestParams.GeneralDetailsData.SpecialActivityTypeEssence = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SpecialActivityRequestComponent.prototype, "ResponseMessage", {
        get: function () { return this.ResponseData ? this.ResponseData.UserMessage : null; },
        set: function (value) {
            if (this.ResponseData.UserMessage != value) {
                this.ResponseData.UserMessage = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SpecialActivityRequestComponent.prototype, "IsResponseMessageVisibility", {
        get: function () { return this._IsResponseMessageVisibility; },
        set: function (newValue) {
            if (this._IsResponseMessageVisibility != newValue) {
                this._IsResponseMessageVisibility = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    //#endregion Properties
    //#region GoodsDetailsData
    SpecialActivityRequestComponent.prototype.SetIsGoodsDetailsData = function (newValue) {
        this.IsGoodsDetailsData = newValue;
        //this.UIProperties.SetRequired("DeclarationNumber", null, true);
    };
    Object.defineProperty(SpecialActivityRequestComponent.prototype, "IsGoodsDetailsData", {
        get: function () { return this._IsGoodsDetailsData; },
        set: function (newValue) {
            if (this._IsGoodsDetailsData != newValue) {
                this._IsGoodsDetailsData = newValue;
                if (newValue == true) {
                    this.SetIsOtherActivity(false);
                    this.SetIsRePackingApproval(false);
                    this.SetIsSampleRequest(false);
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SpecialActivityRequestComponent.prototype, "IdemanderType", {
        get: function () { return this.RequestParams.GoodsDetailsData != null ? this.RequestParams.GoodsDetailsData.IdemanderType : null; },
        set: function (value) {
            if (this.RequestParams.GoodsDetailsData.IdemanderType != value) {
                this.RequestParams.GoodsDetailsData.IdemanderType = value;
            }
            if (value || this.IsGoodsDetailsData == false) {
                this.UIProperties.SetWarning("IdemanderType", null, false);
            }
            else {
                this.UIProperties.SetWarning("IdemanderType", null, true);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SpecialActivityRequestComponent.prototype, "SpecialActionsCode", {
        get: function () { return this._SpecialActionsCode; },
        set: function (value) {
            if (this._SpecialActionsCode != value) {
                this._SpecialActionsCode = value;
            }
            if (value || this.IsGoodsDetailsData == false) {
                this.UIProperties.SetWarning("SpecialActionsCode", null, false);
            }
            else {
                this.UIProperties.SetWarning("SpecialActionsCode", null, true);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SpecialActivityRequestComponent.prototype, "OtherDescription", {
        get: function () { return this.RequestParams.GoodsDetailsData != null ? this.RequestParams.GoodsDetailsData.OtherDescription : null; },
        set: function (value) {
            if (this.RequestParams.GoodsDetailsData.OtherDescription != value) {
                this.RequestParams.GoodsDetailsData.OtherDescription = value;
            }
            if (value || this.IsGoodsDetailsData == false) {
                this.UIProperties.SetWarning("OtherDescription", null, false);
            }
            else {
                this.UIProperties.SetWarning("OtherDescription", null, true);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SpecialActivityRequestComponent.prototype, "GoodsDescription", {
        get: function () { return this.RequestParams.GoodsDetailsData != null ? this.RequestParams.GoodsDetailsData.GoodsDescription : null; },
        set: function (value) {
            if (this.RequestParams.GoodsDetailsData.GoodsDescription != value) {
                this.RequestParams.GoodsDetailsData.GoodsDescription = value;
            }
            if (value || this.IsGoodsDetailsData == false) {
                this.UIProperties.SetWarning("GoodsDescription", null, false);
            }
            else {
                this.UIProperties.SetWarning("GoodsDescription", null, true);
            }
        },
        enumerable: true,
        configurable: true
    });
    SpecialActivityRequestComponent.prototype.AddRepresentativeItemCommand = function () {
        this.RepresentativeList.Insert(new RepresentativeComponent(new SpecialActivityRequestParams_1.RepresentativeDetails()));
        this.SetRepresentativeRowNumber();
    };
    SpecialActivityRequestComponent.prototype.DeleteRepresentativeDetailsCommand = function (item) {
        this.RepresentativeList.Remove(item);
        this.SetRepresentativeRowNumber();
    };
    SpecialActivityRequestComponent.prototype.SetRepresentativeRowNumber = function () {
        var representativeRowNumber = 0;
        for (var _i = 0, _a = this.RepresentativeList.Collection; _i < _a.length; _i++) {
            var item = _a[_i];
            representativeRowNumber = representativeRowNumber + 1;
            item.RepresentativeNumber = representativeRowNumber;
        }
    };
    //#endregion GoodsDetailsData
    //#region OtherActivity Properties
    SpecialActivityRequestComponent.prototype.SetIsOtherActivity = function (newValue) {
        this.IsOtherActivity = newValue;
    };
    Object.defineProperty(SpecialActivityRequestComponent.prototype, "IsOtherActivity", {
        get: function () { return this._IsOtherActivity; },
        set: function (newValue) {
            if (this._IsOtherActivity != newValue) {
                this._IsOtherActivity = newValue;
                if (newValue == true) {
                    this.SetIsGoodsDetailsData(false);
                    this.SetIsRePackingApproval(false);
                    this.SetIsSampleRequest(false);
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    //#endregion OtherActivity Properties
    //#region RePackingApproval
    SpecialActivityRequestComponent.prototype.SetIsRePackingApproval = function (newValue) {
        this.IsRePackingApproval = newValue;
    };
    Object.defineProperty(SpecialActivityRequestComponent.prototype, "IsRePackingApproval", {
        get: function () { return this._IsRePackingApproval; },
        set: function (newValue) {
            if (this._IsRePackingApproval != newValue) {
                this._IsRePackingApproval = newValue;
                if (newValue == true) {
                    this.SetIsGoodsDetailsData(false);
                    this.SetIsOtherActivity(false);
                    this.SetIsSampleRequest(false);
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SpecialActivityRequestComponent.prototype, "RepackingSiteNumber", {
        get: function () { return this.RequestParams.RePackingApprovalDetailsData.SiteNumber; },
        set: function (value) {
            if (this.RequestParams.RePackingApprovalDetailsData.SiteNumber != value) {
                this.RequestParams.RePackingApprovalDetailsData.SiteNumber = value;
            }
            if (value || this.IsRePackingApproval == false) {
                this.UIProperties.SetWarning("RepackingSiteNumber", null, false);
            }
            else {
                this.UIProperties.SetWarning("RepackingSiteNumber", null, true);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SpecialActivityRequestComponent.prototype, "ApprovalDate", {
        get: function () { return this.RequestParams.RePackingApprovalDetailsData.ApprovalDate; },
        set: function (value) {
            if (this.RequestParams.RePackingApprovalDetailsData.ApprovalDate != value) {
                this.RequestParams.RePackingApprovalDetailsData.ApprovalDate = value;
            }
            if (value || this.IsRePackingApproval == false) {
                this.UIProperties.SetWarning("ApprovalDate", null, false);
            }
            else {
                this.UIProperties.SetWarning("ApprovalDate", null, true);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SpecialActivityRequestComponent.prototype, "ApprovalName", {
        get: function () { return this.RequestParams.RePackingApprovalDetailsData.ApprovalName; },
        set: function (value) {
            if (this.RequestParams.RePackingApprovalDetailsData.ApprovalName != value) {
                this.RequestParams.RePackingApprovalDetailsData.ApprovalName = value;
            }
            if (value || this.IsRePackingApproval == false) {
                this.UIProperties.SetWarning("ApprovalName", null, false);
            }
            else {
                this.UIProperties.SetWarning("ApprovalName", null, true);
            }
        },
        enumerable: true,
        configurable: true
    });
    SpecialActivityRequestComponent.prototype.AddRepackingCurrentItemCommand = function () {
        this.RepackingCurrentList.Insert(new RepackingCurrentRequestDetailsComponent(new SpecialActivityRequestParams_1.CurrentPackingDetails()));
        this.SetRepackingCurrentRowNumber();
    };
    SpecialActivityRequestComponent.prototype.DeleteRepackingCurrentItemCommand = function (item) {
        this.RepackingCurrentList.Remove(item);
        this.SetRepackingCurrentRowNumber();
    };
    SpecialActivityRequestComponent.prototype.SetRepackingCurrentRowNumber = function () {
        var rowNumber = 0;
        for (var _i = 0, _a = this.RepackingCurrentList.Collection; _i < _a.length; _i++) {
            var item = _a[_i];
            rowNumber = rowNumber + 1;
            item.CurrentRePackingOldLineNumber = rowNumber;
        }
    };
    SpecialActivityRequestComponent.prototype.AddRepackingDesiredItemCommand = function () {
        this.RepackingDesiredList.Insert(new RepackingDesiredRequestDetailsComponent(new SpecialActivityRequestParams_1.DesiredPackingDetails()));
        this.SetRepackingDesiredRowNumber();
    };
    SpecialActivityRequestComponent.prototype.DeleteRepackingDesiredRequestDetailsCommand = function (item) {
        this.RepackingDesiredList.Remove(item);
        this.SetRepackingDesiredRowNumber();
    };
    SpecialActivityRequestComponent.prototype.SetRepackingDesiredRowNumber = function () {
        var rowNumber = 0;
        for (var _i = 0, _a = this.RepackingDesiredList.Collection; _i < _a.length; _i++) {
            var item = _a[_i];
            rowNumber = rowNumber + 1;
            item.DesiredRePackingNewLineNumber = rowNumber;
        }
    };
    //#endregion RePackingApproval
    //#region SampleRequest
    SpecialActivityRequestComponent.prototype.SetIsSampleRequest = function (newValue) {
        this.IsSampleRequest = newValue;
    };
    Object.defineProperty(SpecialActivityRequestComponent.prototype, "IsSampleRequest", {
        get: function () { return this._IsSampleRequest; },
        set: function (newValue) {
            if (this._IsSampleRequest != newValue) {
                this._IsSampleRequest = newValue;
                if (newValue == true) {
                    this.SetIsOtherActivity(false);
                    this.SetIsRePackingApproval(false);
                    this.SetIsGoodsDetailsData(false);
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    SpecialActivityRequestComponent.prototype.AddSampleRequestItemCommand = function () {
        this.SampleRequestList.Insert(new SampleRequestDetailsComponent(new SpecialActivityRequestParams_1.SampleRequestDetails()));
        this.SetSampleRequestRowNumber();
    };
    SpecialActivityRequestComponent.prototype.DeleteSampleRequestDetailsCommand = function (item) {
        this.SampleRequestList.Remove(item);
        this.SetSampleRequestRowNumber();
    };
    SpecialActivityRequestComponent.prototype.SetSampleRequestRowNumber = function () {
        var rowNumber = 0;
        for (var _i = 0, _a = this.SampleRequestList.Collection; _i < _a.length; _i++) {
            var item = _a[_i];
            rowNumber = rowNumber + 1;
            item.SampleRowNumber = rowNumber;
        }
    };
    //#endregion GoodsDetailsData
    //#region Properties Commands
    SpecialActivityRequestComponent.prototype.SetWarning = function (specialActivityType) {
        this.UIProperties.SetWarning("CargoIdentifierKey2", null, false);
        this.UIProperties.SetWarning("ImporterNumber", null, false);
        this.UIProperties.SetWarning("CargoRowNumber", null, false);
        this.UIProperties.SetWarning("AuthorityCode", null, false);
        switch (specialActivityType) {
            case "5": //RePackingApproval
                this.UIProperties.SetWarning("CargoIdentifierKey2", null, true);
                this.UIProperties.SetWarning("ImporterNumber", null, true);
                this.UIProperties.SetWarning("ApprovalDate", null, true);
                this.UIProperties.SetWarning("RepackingSiteNumber", null, true);
                this.UIProperties.SetWarning("ApprovalName", null, true);
                break;
            case "6": //GoodsDetails 
                this.UIProperties.SetWarning("CargoRowNumber", null, true);
                this.UIProperties.SetWarning("IdemanderType", null, true);
                this.UIProperties.SetWarning("SpecialActionsCode", null, true);
                this.UIProperties.SetWarning("OtherDescription", null, true);
                this.UIProperties.SetWarning("GoodsDescription", null, true);
                break;
            case "7": //SampleRequest 
                this.UIProperties.SetWarning("ImporterNumber", null, true);
                this.UIProperties.SetWarning("CargoIdentifierKey2", null, true);
                this.UIProperties.SetWarning("AuthorityCode", null, true);
                break;
            default:
                break;
        }
    };
    SpecialActivityRequestComponent.prototype.ActivityRequestStartDateLostFocusMethod = function (startDateItem) {
        this.ValidationErrorsList = [];
        this.ActivityRequestStartTime = null;
        if (Tools_1.AppTool.IsNullOrEmpty(startDateItem)) {
            return;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(startDateItem) && startDateItem > this.ActivityRequestEndDate) {
            var msg = "תאריך תחילת הפעולה לא יכול להיות אחרי תאריך סיום הפעולה";
            this.ValidationErrorsList.push(msg);
        }
        if (startDateItem.getDate() == this._TodayDate.getDate()) {
            var date = new Date();
            date.setMinutes(0);
            this.ActivityRequestStartTime = Tools_1.DateTool.AddHour(date, 3);
        }
    };
    SpecialActivityRequestComponent.prototype.ActivityRequestEndDateLostFocusMethod = function (endDateItem) {
        this.ValidationErrorsList = [];
        this.ActivityRequestEndTime = null;
        if (Tools_1.AppTool.IsNullOrEmpty(endDateItem)) {
            return;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.ActivityRequestStartDate) && this.ActivityRequestStartDate > endDateItem) {
            var msg = "תאריך תחילת הפעולה לא יכול להיות אחרי תאריך סיום הפעולה";
            this.ValidationErrorsList.push(msg);
        }
        if (endDateItem.getDate() == this._TodayDate.getDate()) {
            var date = new Date();
            date.setMinutes(0);
            this.ActivityRequestEndTime = Tools_1.DateTool.AddHour(date, 3);
        }
    };
    //#endregion Properties Commands
    //#region Declaration Commands
    SpecialActivityRequestComponent.prototype.DueChangeClearChildField = function (sourceIsCostomFile) {
        this.UIProperties.SetValidity("CustomFileNo", this.ObjectTableName, true, "");
        if (sourceIsCostomFile) {
            this.DeclarationId = "";
            this.CargoIdentifierType = "";
            this.CargoIdentifierKey1 = "";
            this.CargoIdentifierKey2 = "";
            this.CargoIdentifierKey3 = "";
            this.SiteNumber = "";
            this.ImporterNumber = "";
        }
        else {
            this.CustomFileNo = "";
        }
    };
    SpecialActivityRequestComponent.prototype.CustomFileNoTextChanged = function (searchtext) {
        var _this = this;
        if (Tools_1.AppTool.IsNullOrEmpty(this.CustomFileNo)) {
            this.DeclarationId = "";
            this.CargoIdentifierType = "";
            this.CargoIdentifierKey1 = "";
            this.CargoIdentifierKey2 = "";
            this.CargoIdentifierKey3 = "";
            this.ImporterNumber = "";
            this.SiteNumber = "";
            //this.UIProperties.SetEnabled("SiteNumber", "Customs.SiteLookup", true);
            //this.UIProperties.SetEnabled("CargoIdentifierType", "Customs.CargoIdentifireType", true);
            //this.UIProperties.SetEnabled("CargoIdentifierKey1", null, true);
            //this.UIProperties.SetEnabled("CargoIdentifierKey2", null, true);
            //this.UIProperties.SetEnabled("CargoIdentifierKey3", null, true);
            //this.UIProperties.SetEnabled("ImporterNumber", "Customs.Client", true);
            return;
        }
        this.DueChangeClearChildField(true);
        this.CurrentSession.StartBusyIndicator("");
        this._DeclarationExtendedListService.GetConsignmentListPMByCustomFileNo(this.CustomFileNo)
            .subscribe(function (myResponse) {
            _this.CurrentSession.StopBusyIndicator();
            _this.FetchDeclarationConsignment(myResponse, true);
        });
    };
    SpecialActivityRequestComponent.prototype.FetchDeclarationConsignment = function (myResponse, sourceIsCostomFile) {
        var _this = this;
        var lastFetchDeclarationList = myResponse.Result;
        if (lastFetchDeclarationList != null) {
            lastFetchDeclarationList = lastFetchDeclarationList[0];
            this.DeclarationId = lastFetchDeclarationList.DeclarationId;
            this.CargoIdentifierType = lastFetchDeclarationList.CargoTypeCode;
            this.CargoIdentifierKey1 = lastFetchDeclarationList.ManifestNumber;
            this.CargoIdentifierKey2 = lastFetchDeclarationList.SecondCargoID;
            this.CargoIdentifierKey3 = lastFetchDeclarationList.ThirdCargoID;
            this.SiteNumber = lastFetchDeclarationList.StorageSiteCode;
            //this.UIProperties.SetValidity("CustomFileNo", this.ObjectTableName, true, "");
            //this.UIProperties.SetEnabled("SiteNumber", "Customs.SiteLookup", false);
            //this.UIProperties.SetEnabled("CargoIdentifierType", "Customs.CargoIdentifireType", false);
            //this.UIProperties.SetEnabled("CargoIdentifierKey1", null, false);
            //this.UIProperties.SetEnabled("CargoIdentifierKey2", null, false);
            //this.UIProperties.SetEnabled("CargoIdentifierKey3", null, false);
            //this.UIProperties.SetEnabled("ImporterNumber", this.ObjectTableName, false);
            this.CurrentSession.StartBusyIndicator("");
            this._DeclarationExtendedListService.GetSingleDeclarationByCustomFileNo(this.CustomFileNo)
                .subscribe(function (myResponse) {
                _this.CurrentSession.StopBusyIndicator();
                _this.FetchDeclaration(myResponse, true);
            });
        }
        else {
            if (sourceIsCostomFile) {
                this.SetValidityCustomFileNo();
            }
            else {
                //this.SetValidityDeclarationNumber();
            }
        }
    };
    SpecialActivityRequestComponent.prototype.FetchDeclaration = function (myResponse, sourceIsCostomFile) {
        var lastFetchDeclarationList = myResponse.Result;
        if (lastFetchDeclarationList != null) {
            this.ImporterNumber = lastFetchDeclarationList.ImporterCode;
        }
    };
    SpecialActivityRequestComponent.prototype.SetValidityCustomFileNo = function () {
        var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.Didntfindcustomfile");
        this.ValidationErrorsList.push(msg);
        this.UIProperties.SetValidity("CustomFileNo", this.ObjectTableName, false, msg);
    };
    //#endregion Declaration Commands
    //#region General Commands
    SpecialActivityRequestComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    SpecialActivityRequestComponent.prototype.FillErrors = function () {
        var _this = this;
        var errors = [];
        Validator_1.Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        this.ValidationErrorsList = errors;
        if (Tools_1.AppTool.IsNullOrEmpty(this.ActivityRequestStartDate) || Tools_1.AppTool.IsNullOrEmpty(this.ActivityRequestEndDate)
            || Tools_1.AppTool.IsNullOrEmpty(this.ActivityRequestStartTime) || Tools_1.AppTool.IsNullOrEmpty(this.ActivityRequestEndTime)) {
            var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.SpecialActivityRequest.F.ActivityRequestStartEndDateMandatory");
            if (Tools_1.AppTool.IsNullOrEmpty(this.ActivityRequestStartTime) || Tools_1.AppTool.IsNullOrEmpty(this.ActivityRequestEndTime)) {
                msg = msg + " (כולל שעות)";
            }
            this.ValidationErrorsList.push(msg);
        }
        if (Tools_1.AppTool.IsNullOrEmpty(this.SiteNumber)) {
            var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.SpecialActivityRequest.F.SiteNumberMandatory");
            this.ValidationErrorsList.push(msg);
        }
        if (Tools_1.AppTool.IsNullOrEmpty(this.CargoIdentifierType)) {
            var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.SpecialActivityRequest.F.CargoIdentifierTypMandatory");
            this.ValidationErrorsList.push(msg);
        }
        if (Tools_1.AppTool.IsNullOrEmpty(this.CargoIdentifierKey1)) {
            var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.SpecialActivityRequest.F.CargoIdentifierKey1Mandatory");
            this.ValidationErrorsList.push(msg);
        }
        switch (this.SpecialActivityType) {
            case "5": //RePackingApproval
                if (this.RepackingCurrentList.Length == 0) {
                    this.ValidationErrorsList.push(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.SpecialActivityRequest.F.RepackingCurrentItemsItemsMandatory"));
                }
                if (this.RepackingDesiredList.Length == 0) {
                    this.ValidationErrorsList.push(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.SpecialActivityRequest.F.RepackingDesiredItemsItemsMandatory"));
                }
                break;
            case "6": //GoodsDetails 
                if (this.RepresentativeList.Length == 0) {
                    this.ValidationErrorsList.push(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.SpecialActivityRequest.F.RepresentativeItemsItemsMandatory"));
                }
                break;
            case "7": //SampleRequest 
                if (this.SampleRequestList.Length == 0) {
                    this.ValidationErrorsList.push(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.SpecialActivityRequest.F.SampleItemsMandatory"));
                }
                else {
                    this.SampleRequestList.Collection.forEach(function (item) {
                        if (Tools_1.AppTool.IsNullOrEmpty(item.SampleReturnDate)) {
                            _this.ValidationErrorsList.push(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.SpecialActivityRequest.F.SampleReturnDateMandatory"));
                        }
                        if (Tools_1.AppTool.IsNullOrEmpty(item.SampleValue)) {
                            _this.ValidationErrorsList.push(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.SpecialActivityRequest.F.SampleValueMandatory"));
                        }
                        if (Tools_1.AppTool.IsNullOrEmpty(item.CurrencyTypeCode)) {
                            _this.ValidationErrorsList.push(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.SpecialActivityRequest.F.CurrencyTypeCodeMandatory"));
                        }
                        if (Tools_1.AppTool.IsNullOrEmpty(item.SampleDescription)) {
                            _this.ValidationErrorsList.push(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.SpecialActivityRequest.F.SampleDescriptionMandatory"));
                        }
                    });
                }
                break;
            default:
                break;
        }
    };
    SpecialActivityRequestComponent.prototype.CheckBeforeSend = function () {
        var _this = this;
        var isMissingWarningField = false;
        var userMessage = "לא הוזנו כל השדות המומלצים (מסומנים בצהוב), האם ברצונך לשלוח בכל זאת?";
        switch (this.SpecialActivityType) {
            case "5": //RePackingApproval
                if (Tools_1.AppTool.IsNullOrEmpty(this.CargoIdentifierKey2) || Tools_1.AppTool.IsNullOrEmpty(this.RepackingSiteNumber) || Tools_1.AppTool.IsNullOrEmpty(this.ApprovalDate)
                    || Tools_1.AppTool.IsNullOrEmpty(this.ApprovalName) || Tools_1.AppTool.IsNullOrEmpty(this.ImporterNumber)) {
                    isMissingWarningField = true;
                }
                break;
            case "6": //GoodsDetails
                if (Tools_1.AppTool.IsNullOrEmpty(this.CargoRowNumber) || Tools_1.AppTool.IsNullOrEmpty(this.IdemanderType) || Tools_1.AppTool.IsNullOrEmpty(this.OtherDescription)
                    || Tools_1.AppTool.IsNullOrEmpty(this.SpecialActionsCode) || Tools_1.AppTool.IsNullOrEmpty(this.GoodsDescription)) {
                    isMissingWarningField = true;
                }
                if (this.RepackingCurrentList.Length > 0) {
                    this.RepackingCurrentList.Collection.forEach(function (item) {
                        if (Tools_1.AppTool.IsNullOrEmpty(item.RepresentativeName) || Tools_1.AppTool.IsNullOrEmpty(item.RepresentativeID)) {
                            isMissingWarningField = true;
                        }
                    });
                }
                break;
            case "7": //SampleRequest 
                if (Tools_1.AppTool.IsNullOrEmpty(this.CargoIdentifierKey2) || Tools_1.AppTool.IsNullOrEmpty(this.AuthorityCode) || Tools_1.AppTool.IsNullOrEmpty(this.ImporterNumber)) {
                    isMissingWarningField = true;
                }
                break;
            default:
                break;
        }
        if (isMissingWarningField) {
            var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
            confirmWindow.Width = 250;
            confirmWindow.Height = 150;
            confirmWindow.YesButtonText = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.B.OK");
            confirmWindow.NoButtonText = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.B.Cancel");
            confirmWindow.ShowCancelButton = false;
            confirmWindow.Show(userMessage);
            confirmWindow.WindowClosed.subscribe(function (event) {
                if (confirmWindow.Yes) {
                    _this.SendSpecialActivityRequestMethod();
                }
            });
        }
        else {
            this.SendSpecialActivityRequestMethod();
        }
    };
    SpecialActivityRequestComponent.prototype.OnCustomSendOptionsButtonClick = function (customSendOptionsArgs) {
        this.FillErrors();
        if (this.ValidationErrorsList.length > 0) {
            return;
        }
        this._CustomSendOptionsArgs = customSendOptionsArgs;
        this.CheckBeforeSend();
    };
    SpecialActivityRequestComponent.prototype.SendSpecialActivityRequestMethod = function () {
        var _this = this;
        var currRequestParams = new SpecialActivityRequestParams_1.SpecialActivityRequestParams();
        currRequestParams.LoggingEnabled = true;
        currRequestParams.LoggingUserId = SessionLocator_1.SessionLocator.LoggedUserId;
        currRequestParams.RequestVIA = this._CustomSendOptionsArgs.RequestVIA;
        currRequestParams.ForcePersonalSign = this._CustomSendOptionsArgs.ForcePersonalSign;
        currRequestParams.Tenant = SessionLocator_1.SessionLocator.Tenant;
        //GeneralDetails
        currRequestParams.GeneralDetailsData = new SpecialActivityRequestParams_1.GeneralDetails();
        currRequestParams.GeneralDetailsData.SpecialActivityRequestNumber = this.SpecialActivityRequestNumber;
        currRequestParams.GeneralDetailsData.ActivityRequestStartDate = this.ActivityRequestStartDate;
        if (this.ActivityRequestStartTime != null) {
            var startDate = this.ActivityRequestStartDate;
            startDate.setHours(this.ActivityRequestStartTime.getHours());
            startDate.setMinutes(this.ActivityRequestStartTime.getMinutes());
            currRequestParams.GeneralDetailsData.ActivityRequestStartDate = startDate;
            currRequestParams.GeneralDetailsData.ActivityRequestStartTime = this.ActivityRequestStartTime;
        }
        currRequestParams.GeneralDetailsData.ActivityRequestEndDate = this.ActivityRequestEndDate;
        if (this.ActivityRequestEndTime != null) {
            var endDate = this.ActivityRequestEndDate;
            endDate.setHours(this.ActivityRequestEndTime.getHours());
            endDate.setMinutes(this.ActivityRequestEndTime.getMinutes());
            currRequestParams.GeneralDetailsData.ActivityRequestEndDate = endDate;
            currRequestParams.GeneralDetailsData.ActivityRequestEndTime = this.ActivityRequestEndTime;
        }
        currRequestParams.GeneralDetailsData.CustomFileNo = this.CustomFileNo;
        currRequestParams.GeneralDetailsData.DeclarationId = this.DeclarationId;
        currRequestParams.GeneralDetailsData.SiteNumber = this.SiteNumber;
        currRequestParams.GeneralDetailsData.WarehouseBlockNumber = this.WarehouseBlockNumber;
        currRequestParams.GeneralDetailsData.CargoRowNumber = this.CargoRowNumber;
        currRequestParams.GeneralDetailsData.CargoRowNumberSpecified = this.CargoRowNumber == null ? false : true;
        currRequestParams.GeneralDetailsData.AuthorityCode = this.AuthorityCode;
        currRequestParams.GeneralDetailsData.AuthorityCodeSpecified = this.AuthorityCode == null ? false : true;
        currRequestParams.GeneralDetailsData.SpecialActivityType = Number(this.SpecialActivityType);
        currRequestParams.GeneralDetailsData.ImporterNumber = this.ImporterNumber;
        currRequestParams.GeneralDetailsData.ImporterNumberSpecified = this.ImporterNumber == null ? false : true;
        currRequestParams.GeneralDetailsData.ApplicantAgentNumber = this.ApplicantAgentNumber;
        currRequestParams.GeneralDetailsData.CheckSite = this.CheckSite;
        currRequestParams.GeneralDetailsData.SpecialActivityTypeEssence = this.SpecialActivityTypeEssence;
        //CargoIdentifier
        currRequestParams.GeneralDetailsData.CargoIdentifier = new SpecialActivityRequestParams_1.CargoIdentifier();
        var cargoIdentifierType = this.CargoIdentifierType;
        currRequestParams.GeneralDetailsData.CargoIdentifier.CargoIdentifierType = cargoIdentifierType;
        currRequestParams.GeneralDetailsData.CargoIdentifier.CargoIdentifierKey1 = this.CargoIdentifierKey1;
        currRequestParams.GeneralDetailsData.CargoIdentifier.CargoIdentifierKey2 = this.CargoIdentifierKey2;
        currRequestParams.GeneralDetailsData.CargoIdentifier.CargoIdentifierKey3 = this.CargoIdentifierKey3;
        switch (this.SpecialActivityType) {
            case "5": //RePackingApproval
                {
                    currRequestParams.RePackingApprovalDetailsData = new SpecialActivityRequestParams_1.RePackingApprovalDetails();
                    currRequestParams.RePackingApprovalDetailsData.ApprovalName = this.ApprovalName;
                    currRequestParams.RePackingApprovalDetailsData.ApprovalDateSpecified = this.ApprovalDate == null ? false : true;
                    currRequestParams.RePackingApprovalDetailsData.ApprovalDate = this.ApprovalDate;
                    currRequestParams.RePackingApprovalDetailsData.SiteNumber = this.SiteNumber;
                    if (this.RepackingCurrentList != null && this.RepackingCurrentList.Length > 0) {
                        currRequestParams.CurrentPackingDetailsDataList = [];
                        this.RepackingCurrentList.Collection.forEach(function (repackingCurrentItem) {
                            var currentPackingDetails = new SpecialActivityRequestParams_1.CurrentPackingDetails();
                            currentPackingDetails.RePackingOldLineNumber = repackingCurrentItem.CurrentRePackingOldLineNumber;
                            currentPackingDetails.PresentPackingStateContent = repackingCurrentItem.CurrentPresentPackingStateContent;
                            currentPackingDetails.RePackingOldLineNumberSpecified = repackingCurrentItem.CurrentRePackingOldLineNumber == null ? false : true;
                            var packingDetails = new SpecialActivityRequestParams_1.PackingDetails();
                            packingDetails.PackageId = repackingCurrentItem.RepackingCurrentPackageId;
                            packingDetails.PackageType = repackingCurrentItem.RepackingCurrentPackageType;
                            packingDetails.PackageTypeName = repackingCurrentItem.RepackingCurrentPackageName;
                            packingDetails.Quantity = repackingCurrentItem.RepackingCurrentQuantity;
                            packingDetails.Weight = repackingCurrentItem.RepackingCurrentWeight;
                            packingDetails.WeightSpecified = repackingCurrentItem.RepackingCurrentWeight == null ? false : true;
                            currentPackingDetails.PackingDetails = packingDetails;
                            currRequestParams.CurrentPackingDetailsDataList.push(currentPackingDetails);
                        });
                    }
                    if (this.RepackingDesiredList != null && this.RepackingDesiredList.Length > 0) {
                        currRequestParams.DesiredPackingDetailsDataList = [];
                        this.RepackingDesiredList.Collection.forEach(function (repackingDesiredItem) {
                            var desiredPackingDetails = new SpecialActivityRequestParams_1.DesiredPackingDetails();
                            desiredPackingDetails.RePackingOldLineNumber = repackingDesiredItem.DesiredRePackingOldLineNumber;
                            desiredPackingDetails.RePackingNewLineNumber = repackingDesiredItem.DesiredRePackingNewLineNumber;
                            desiredPackingDetails.RePackingOldLineNumberSpecified = repackingDesiredItem.DesiredRePackingOldLineNumber == null ? false : true;
                            desiredPackingDetails.RePackingNewLineNumberSpecified = repackingDesiredItem.DesiredRePackingNewLineNumber == null ? false : true;
                            var packingDetails = new SpecialActivityRequestParams_1.PackingDetails();
                            packingDetails.PackageId = repackingDesiredItem.RepackingDesiredPackageId;
                            packingDetails.PackageType = repackingDesiredItem.RepackingDesiredPackageType;
                            packingDetails.PackageTypeName = repackingDesiredItem.RepackingDesiredPackageName;
                            packingDetails.Quantity = repackingDesiredItem.RepackingDesiredQuantity;
                            packingDetails.Weight = repackingDesiredItem.RepackingDesiredWeight;
                            packingDetails.WeightSpecified = repackingDesiredItem.RepackingDesiredWeight == null ? false : true;
                            desiredPackingDetails.PackingDetails = packingDetails;
                            currRequestParams.DesiredPackingDetailsDataList.push(desiredPackingDetails);
                        });
                    }
                    currRequestParams.RequestName = "Special Activity Request - Repacking";
                    currRequestParams.ResponseName = "Special Activity Request - Repacking";
                    break;
                }
            case "6": //GoodsDetails 
                {
                    currRequestParams.GoodsDetailsData = new SpecialActivityRequestParams_1.GoodsDetails();
                    currRequestParams.GoodsDetailsData.IdemanderType = this.IdemanderType;
                    currRequestParams.GoodsDetailsData.IdemanderTypeSpecified = this.IdemanderType == null ? false : true;
                    var specialActionsCode = this.SpecialActionsCode;
                    currRequestParams.GoodsDetailsData.SpecialActionsCode = specialActionsCode;
                    currRequestParams.GoodsDetailsData.SpecialActionsCodeSpecified = specialActionsCode > 0 ? true : false;
                    currRequestParams.GoodsDetailsData.GoodsDescription = this.GoodsDescription;
                    currRequestParams.GoodsDetailsData.OtherDescription = this.OtherDescription;
                    if (this.RepresentativeList != null && this.RepresentativeList.Length > 0) {
                        currRequestParams.GoodsDetailsData.RepresentativeList = [];
                        this.RepresentativeList.Collection.forEach(function (representativeItem) {
                            var representativeDetails = new SpecialActivityRequestParams_1.RepresentativeDetails();
                            representativeDetails.RepresentativeNumber = representativeItem.RepresentativeNumber;
                            representativeDetails.RepresentativeName = representativeItem.RepresentativeName;
                            representativeDetails.RepresentativeID = representativeItem.RepresentativeID;
                            representativeDetails.RepresentativeNumberSpecified = representativeItem.RepresentativeNumber == null ? false : true;
                            representativeDetails.RepresentativeIDSpecified = representativeItem.RepresentativeID == null ? false : true;
                            currRequestParams.GoodsDetailsData.RepresentativeList.push(representativeDetails);
                        });
                    }
                    currRequestParams.RequestName = "Special Activity Request - Goods Details";
                    currRequestParams.ResponseName = "Special Activity Request - Goods Details";
                    break;
                }
            case "7": //SampleRequest 
                {
                    if (this.SampleRequestList != null && this.SampleRequestList.Length > 0) {
                        currRequestParams.SampleRequestDetailsDataList = [];
                        this.SampleRequestList.Collection.forEach(function (sampleItem) {
                            var sampleRequestDetails = new SpecialActivityRequestParams_1.SampleRequestDetails();
                            sampleRequestDetails.SampleRowNumber = sampleItem.SampleRowNumber;
                            sampleRequestDetails.SampleReturnDate = sampleItem.SampleReturnDate;
                            sampleRequestDetails.CustomsItem = sampleItem.CustomsItem;
                            sampleRequestDetails.CustomsItemQuantity = sampleItem.CustomsItemQuantity;
                            sampleRequestDetails.SampleValue = sampleItem.SampleValue;
                            sampleRequestDetails.CurrencyTypeCode = sampleItem.CurrencyTypeCode;
                            sampleRequestDetails.CurrencyTypeName = sampleItem.CurrencyTypeName;
                            sampleRequestDetails.SampleDescription = sampleItem.SampleDescription;
                            sampleRequestDetails.SampleRowNumberSpecified = sampleItem.SampleRowNumber == null ? false : true;
                            sampleRequestDetails.CustomsItemQuantitySpecified = sampleItem.CustomsItemQuantity == null ? false : true;
                            var packingDetails = new SpecialActivityRequestParams_1.PackingDetails();
                            packingDetails.PackageId = sampleItem.SamplePackageId;
                            packingDetails.PackageType = sampleItem.SamplePackageType;
                            packingDetails.PackageTypeName = sampleItem.SamplePackageName;
                            packingDetails.Quantity = sampleItem.SampleQuantity;
                            packingDetails.Weight = sampleItem.SampleWeight;
                            packingDetails.WeightSpecified = sampleItem.SampleWeight == null ? false : true;
                            sampleRequestDetails.SamplePackingDetails = packingDetails;
                            currRequestParams.SampleRequestDetailsDataList.push(sampleRequestDetails);
                        });
                    }
                    currRequestParams.RequestName = "Special Activity Request - Sample request";
                    currRequestParams.ResponseName = "Special Activity Request - Sample request";
                    break;
                }
            case "13": //Other
                break;
        }
        CustomMessageProgressComponent_1.CustomMessageProgressComponent
            .ShowProgressBar(currRequestParams.PBId, "שליחת בקשה לפעולות מיוחדות", true)
            .then(function (res) {
            _this.ResponseData = res;
            _this._IsLoadResponseData = false;
            _this.OnMassageDisplayMethod();
        }).catch(function (err) {
            _this.ValidationErrorsList.push(err);
        });
        this._IIGGeneralMessagesService.PostSpecialActivityRequest(currRequestParams)
            .subscribe(function (myServiceResponse) {
        });
    };
    __decorate([
        core_1.ViewChild(CustomMessageWrapperComponent_1.CustomMessageWrapperComponent),
        __metadata("design:type", CustomMessageWrapperComponent_1.CustomMessageWrapperComponent)
    ], SpecialActivityRequestComponent.prototype, "SuperCustomMessageWrapperComponent", void 0);
    SpecialActivityRequestComponent = __decorate([
        core_1.Component({
            selector: 'SpecialActivityRequestComponent',
            moduleId: module.id,
            templateUrl: './SpecialActivityRequestComponent.html',
        }),
        __metadata("design:paramtypes", [EntityResourceService_1.EntityResourceService])
    ], SpecialActivityRequestComponent);
    return SpecialActivityRequestComponent;
}(BaseRequestsSheetMassaging_1.BaseRequestsSheetMassaging));
exports.SpecialActivityRequestComponent = SpecialActivityRequestComponent;
var RepresentativeComponent = /** @class */ (function (_super) {
    __extends(RepresentativeComponent, _super);
    function RepresentativeComponent(representativeDetails) {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.RepresentativeNumber = representativeDetails.RepresentativeNumber;
        _this.RepresentativeName = representativeDetails.RepresentativeName;
        _this.RepresentativeID = representativeDetails.RepresentativeID;
        return _this;
    }
    Object.defineProperty(RepresentativeComponent.prototype, "RepresentativeNumber", {
        get: function () { return this.representativeNumber; },
        set: function (newValue) { this.representativeNumber = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(RepresentativeComponent.prototype, "RepresentativeName", {
        get: function () { return this.representativeName; },
        set: function (newValue) { this.representativeName = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(RepresentativeComponent.prototype, "RepresentativeID", {
        get: function () { return this.representativeID; },
        set: function (newValue) { this.representativeID = newValue; },
        enumerable: true,
        configurable: true
    });
    return RepresentativeComponent;
}(BaseComponent_1.BaseComponent));
exports.RepresentativeComponent = RepresentativeComponent;
var RepackingCurrentRequestDetailsComponent = /** @class */ (function (_super) {
    __extends(RepackingCurrentRequestDetailsComponent, _super);
    function RepackingCurrentRequestDetailsComponent(currentPackingDetails) {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.CurrentRePackingOldLineNumber = currentPackingDetails.RePackingOldLineNumber;
        _this.CurrentPresentPackingStateContent = currentPackingDetails.PresentPackingStateContent;
        if (currentPackingDetails.PackingDetails != null) {
            _this.RepackingCurrentPackageId = currentPackingDetails.PackingDetails.PackageId;
            _this.RepackingCurrentPackageType = currentPackingDetails.PackingDetails.PackageType;
            _this.RepackingCurrentPackageName = currentPackingDetails.PackingDetails.PackageTypeName;
            _this.RepackingCurrentQuantity = currentPackingDetails.PackingDetails.Quantity;
            _this.RepackingCurrentWeight = currentPackingDetails.PackingDetails.Weight;
        }
        return _this;
    }
    Object.defineProperty(RepackingCurrentRequestDetailsComponent.prototype, "CurrentRePackingOldLineNumber", {
        get: function () { return this.currentRePackingOldLineNumber; },
        set: function (newValue) { this.currentRePackingOldLineNumber = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(RepackingCurrentRequestDetailsComponent.prototype, "RepackingCurrentPackageName", {
        get: function () { return this.repackingCurrentPackageName; },
        set: function (newValue) { this.repackingCurrentPackageName = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(RepackingCurrentRequestDetailsComponent.prototype, "RepackingCurrentPackageType", {
        get: function () { return this.repackingCurrentPackageType; },
        set: function (newValue) { this.repackingCurrentPackageType = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(RepackingCurrentRequestDetailsComponent.prototype, "RepackingCurrentPackageId", {
        get: function () { return this.repackingCurrentPackageId; },
        set: function (newValue) { this.repackingCurrentPackageId = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(RepackingCurrentRequestDetailsComponent.prototype, "RepackingCurrentQuantity", {
        get: function () { return this.repackingCurrentQuantity; },
        set: function (newValue) { this.repackingCurrentQuantity = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(RepackingCurrentRequestDetailsComponent.prototype, "RepackingCurrentWeight", {
        get: function () { return this.repackingCurrentWeight; },
        set: function (newValue) { this.repackingCurrentWeight = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(RepackingCurrentRequestDetailsComponent.prototype, "CurrentPresentPackingStateContent", {
        get: function () { return this.currentPresentPackingStateContent; },
        set: function (newValue) { this.currentPresentPackingStateContent = newValue; },
        enumerable: true,
        configurable: true
    });
    RepackingCurrentRequestDetailsComponent.prototype.SetLocalName = function (entity, fieldName) {
        if (!Tools_1.AppTool.IsNullOrEmpty(entity)) {
            this[fieldName] = entity.LocalName;
        }
        else {
            this[fieldName] = null;
        }
    };
    return RepackingCurrentRequestDetailsComponent;
}(BaseComponent_1.BaseComponent));
exports.RepackingCurrentRequestDetailsComponent = RepackingCurrentRequestDetailsComponent;
var RepackingDesiredRequestDetailsComponent = /** @class */ (function (_super) {
    __extends(RepackingDesiredRequestDetailsComponent, _super);
    function RepackingDesiredRequestDetailsComponent(desiredPackingDetails) {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.DesiredRePackingNewLineNumber = desiredPackingDetails.RePackingNewLineNumber;
        _this.DesiredRePackingOldLineNumber = desiredPackingDetails.RePackingOldLineNumber;
        if (desiredPackingDetails.PackingDetails != null) {
            _this.RepackingDesiredPackageId = desiredPackingDetails.PackingDetails.PackageId;
            _this.RepackingDesiredPackageType = desiredPackingDetails.PackingDetails.PackageType;
            _this.RepackingDesiredPackageName = desiredPackingDetails.PackingDetails.PackageTypeName;
            _this.RepackingDesiredQuantity = desiredPackingDetails.PackingDetails.Quantity;
            _this.RepackingDesiredWeight = desiredPackingDetails.PackingDetails.Weight;
        }
        return _this;
    }
    Object.defineProperty(RepackingDesiredRequestDetailsComponent.prototype, "DesiredRePackingNewLineNumber", {
        get: function () { return this.desiredRePackingNewLineNumber; },
        set: function (newValue) { this.desiredRePackingNewLineNumber = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(RepackingDesiredRequestDetailsComponent.prototype, "RepackingDesiredPackageType", {
        get: function () { return this.repackingDesiredPackageType; },
        set: function (newValue) { this.repackingDesiredPackageType = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(RepackingDesiredRequestDetailsComponent.prototype, "RepackingDesiredPackageName", {
        get: function () { return this.repackingDesiredPackageName; },
        set: function (newValue) { this.repackingDesiredPackageName = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(RepackingDesiredRequestDetailsComponent.prototype, "RepackingDesiredPackageId", {
        get: function () { return this.repackingDesiredPackageId; },
        set: function (newValue) { this.repackingDesiredPackageId = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(RepackingDesiredRequestDetailsComponent.prototype, "RepackingDesiredQuantity", {
        get: function () { return this.repackingDesiredQuantity; },
        set: function (newValue) { this.repackingDesiredQuantity = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(RepackingDesiredRequestDetailsComponent.prototype, "RepackingDesiredWeight", {
        get: function () { return this.repackingDesiredWeight; },
        set: function (newValue) { this.repackingDesiredWeight = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(RepackingDesiredRequestDetailsComponent.prototype, "DesiredRePackingOldLineNumber", {
        get: function () { return this.desiredRePackingOldLineNumber; },
        set: function (newValue) { this.desiredRePackingOldLineNumber = newValue; },
        enumerable: true,
        configurable: true
    });
    RepackingDesiredRequestDetailsComponent.prototype.SetLocalName = function (entity, fieldName) {
        if (!Tools_1.AppTool.IsNullOrEmpty(entity)) {
            this[fieldName] = entity.LocalName;
        }
        else {
            this[fieldName] = null;
        }
    };
    return RepackingDesiredRequestDetailsComponent;
}(BaseComponent_1.BaseComponent));
exports.RepackingDesiredRequestDetailsComponent = RepackingDesiredRequestDetailsComponent;
var SampleRequestDetailsComponent = /** @class */ (function (_super) {
    __extends(SampleRequestDetailsComponent, _super);
    function SampleRequestDetailsComponent(sampleRequestDetails) {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.SampleRowNumber = sampleRequestDetails.SampleRowNumber;
        _this.SampleReturnDate = sampleRequestDetails.SampleReturnDate;
        _this.CustomsItemQuantity = sampleRequestDetails.CustomsItemQuantity;
        _this.CurrencyTypeCode = sampleRequestDetails.CurrencyTypeCode;
        _this.CurrencyTypeName = sampleRequestDetails.CurrencyTypeName;
        _this.SampleValue = sampleRequestDetails.SampleValue;
        _this.SampleDescription = sampleRequestDetails.SampleDescription;
        _this.CustomsItem = sampleRequestDetails.CustomsItem;
        if (sampleRequestDetails.SamplePackingDetails != null) {
            _this.SamplePackageId = sampleRequestDetails.SamplePackingDetails.PackageId;
            _this.SamplePackageType = sampleRequestDetails.SamplePackingDetails.PackageType;
            _this.SamplePackageName = sampleRequestDetails.SamplePackingDetails.PackageTypeName;
            _this.SampleQuantity = sampleRequestDetails.SamplePackingDetails.Quantity;
            _this.SampleWeight = sampleRequestDetails.SamplePackingDetails.Weight;
        }
        return _this;
    }
    Object.defineProperty(SampleRequestDetailsComponent.prototype, "SampleRowNumber", {
        get: function () { return this.sampleRowNumber; },
        set: function (newValue) { this.sampleRowNumber = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SampleRequestDetailsComponent.prototype, "SampleReturnDate", {
        get: function () { return this.sampleReturnDate; },
        set: function (newValue) { this.sampleReturnDate = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SampleRequestDetailsComponent.prototype, "CustomsItem", {
        get: function () { return this.customsItem; },
        set: function (newValue) { this.customsItem = newValue; },
        enumerable: true,
        configurable: true
    });
    SampleRequestDetailsComponent.prototype.OnCustomsItemLostFocus = function (logCellTemplate, customsItemTextBox) {
        //var newValue = this.CustomsItem;
        var newValue = customsItemTextBox.textValue;
        var valid = true;
        this.UIProperties.SetValidity("CustomsItem", "Customs.SupplierInvoiceItem", true, "");
        if (Tools_1.AppTool.IsNullOrEmpty(newValue)) {
            return;
        }
        if (newValue.toString().length > 11) {
            valid = false;
            this.UIProperties.SetValidity("CustomsItem", "Customs.SupplierInvoiceItem", false, TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.CodeLong"));
        }
        else if (newValue.toString().length < 8) {
            valid = false;
            this.UIProperties.SetValidity("CustomsItem", "Customs.SupplierInvoiceItem", false, TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.CodeShort"));
        }
        else if (newValue.toString().length == 8) {
            newValue = newValue + "00";
            var checkDigit = LuhnAlgorithm_1.LuhnAlgorithm.CalculateLuhnAlgorithm(newValue);
            newValue = newValue + checkDigit;
            valid = true;
            ;
        }
        else if (newValue.toString().length == 9) {
            var digit = newValue.toString().substring(8);
            newValue = newValue.toString().substring(0, 8) + "00" + newValue.toString().substring(8);
            var checkDigit = LuhnAlgorithm_1.LuhnAlgorithm.CalculateLuhnAlgorithm(newValue.substring(0, 10));
            if (digit != checkDigit.toString()) {
                valid = false;
                this.UIProperties.SetValidity("CustomsItem", "Customs.SupplierInvoiceItem", false, TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.CorrectDigit") + checkDigit.toString());
            }
            else {
                valid = true;
            }
        }
        else if (newValue.toString().length == 10) {
            var checkDigit = LuhnAlgorithm_1.LuhnAlgorithm.CalculateLuhnAlgorithm(newValue);
            newValue = newValue + "" + checkDigit;
            valid = true;
        }
        else if (newValue.toString().length == 11) {
            var digit = newValue.toString().substring(10);
            var checkDigit = LuhnAlgorithm_1.LuhnAlgorithm.CalculateLuhnAlgorithm(newValue.toString().substring(0, 10));
            if (digit != checkDigit.toString()) {
                valid = false;
                this.UIProperties.SetValidity("CustomsItem", null, false, TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.CorrectDigit") + checkDigit.toString());
            }
            else {
                valid = true;
            }
        }
        else {
            valid = true;
            this.UIProperties.SetValidity("CustomsItem", null, true, "");
        }
        this.CustomsItem = newValue;
        if (valid) {
            SessionLocator_1.SessionLocator.SustainFocusOnCell = false;
        }
        else {
            SessionLocator_1.SessionLocator.SustainFocusOnCell = true;
            this.CurrentSession.SessionEvent.emit({ FocusNow: true, OuterDivId: logCellTemplate.OuterDivId, LogTextBoxId: customsItemTextBox.InputId });
        }
    };
    SampleRequestDetailsComponent.prototype.OnSamplePackageIdLostFocus = function (logCellTemplate, samplePackageIdTextBox) {
        var newValue = samplePackageIdTextBox.textValue;
        var valid = true;
        this.UIProperties.SetValidity("SamplePackageId", "Customs.Declaration", true, "");
        if (Tools_1.AppTool.IsNullOrEmpty(newValue)) {
            return;
        }
        if (newValue.toString().length > 32) {
            valid = false;
            this.UIProperties.SetValidity("SamplePackageId", "Customs.Declaration", false, "השדה זהוי אריזה חייב להיות קטן מ 32 תווים");
        }
        else {
            valid = true;
            this.UIProperties.SetValidity("SamplePackageId", "Customs.Declaration", true, "");
        }
        this.SamplePackageId = newValue;
        if (valid) {
            SessionLocator_1.SessionLocator.SustainFocusOnCell = false;
        }
        else {
            SessionLocator_1.SessionLocator.SustainFocusOnCell = true;
            this.CurrentSession.SessionEvent.emit({ FocusNow: true, OuterDivId: logCellTemplate.OuterDivId, LogTextBoxId: samplePackageIdTextBox.InputId });
        }
    };
    Object.defineProperty(SampleRequestDetailsComponent.prototype, "CustomsItemQuantity", {
        get: function () { return this.customsItemQuantity; },
        set: function (newValue) { this.customsItemQuantity = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SampleRequestDetailsComponent.prototype, "CurrencyTypeCode", {
        get: function () { return this.currencyTypeCode; },
        set: function (newValue) { this.currencyTypeCode = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SampleRequestDetailsComponent.prototype, "CurrencyTypeName", {
        get: function () { return this.currencyTypeName; },
        set: function (newValue) { this.currencyTypeName = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SampleRequestDetailsComponent.prototype, "SampleValue", {
        get: function () { return this.sampleValue; },
        set: function (newValue) { this.sampleValue = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SampleRequestDetailsComponent.prototype, "SampleDescription", {
        get: function () { return this.sampleDescription; },
        set: function (newValue) { this.sampleDescription = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SampleRequestDetailsComponent.prototype, "SamplePackageId", {
        get: function () { return this.samplePackageId; },
        set: function (newValue) { this.samplePackageId = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SampleRequestDetailsComponent.prototype, "SamplePackageType", {
        get: function () { return this.samplePackageType; },
        set: function (newValue) { this.samplePackageType = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SampleRequestDetailsComponent.prototype, "SamplePackageName", {
        get: function () { return this.samplePackageName; },
        set: function (newValue) { this.samplePackageName = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SampleRequestDetailsComponent.prototype, "SampleWeight", {
        get: function () { return this.sampleWeight; },
        set: function (newValue) { this.sampleWeight = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SampleRequestDetailsComponent.prototype, "SampleQuantity", {
        get: function () { return this.sampleQuantity; },
        set: function (newValue) { this.sampleQuantity = newValue; },
        enumerable: true,
        configurable: true
    });
    SampleRequestDetailsComponent.prototype.SetLocalName = function (entity, fieldName) {
        if (!Tools_1.AppTool.IsNullOrEmpty(entity)) {
            this[fieldName] = entity.LocalName;
        }
        else {
            this[fieldName] = null;
        }
    };
    return SampleRequestDetailsComponent;
}(BaseComponent_1.BaseComponent));
exports.SampleRequestDetailsComponent = SampleRequestDetailsComponent;
//# sourceMappingURL=SpecialActivityRequestComponent.js.map