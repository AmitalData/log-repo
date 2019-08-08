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
var IIGGeneralMessagesService_1 = require("../../../Customs/Services/WebServices/IIGGeneralMessagesService");
var CreditQueryRequestParams_1 = require("../../../Customs/DataContract/RequestParams/CreditQueryRequestParams");
var Tools_1 = require("../../../Infrastructure/Tools");
var BaseRequestsSheetMassaging_1 = require("../../../CustomsModules/CustomsRequests/Components/BaseRequestsSheetMassaging");
var CustomMessageProgressComponent_1 = require("../../../CustomsModules/CustomsControls/Components/CustomMessageProgressComponent");
var ObservableCollection_1 = require("../../../Infrastructure/Utilities/ObservableCollection");
var CustomsSettingListService_1 = require("../../../Customs/Services/StandardLists/CustomsSettingListService");
var CreditLimitQueryComponent = /** @class */ (function (_super) {
    __extends(CreditLimitQueryComponent, _super);
    function CreditLimitQueryComponent() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.ObjectTableName = "Customs.Declaration";
        _this.IsAgentNoDisplayOnly = true;
        _this._IIGGeneralMessagesService = new IIGGeneralMessagesService_1.IIGGeneralMessagesService();
        _this._CustomsSettingListService = new CustomsSettingListService_1.CustomsSettingListService();
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.SuperCustomMessageWrapperComponent = new CustomMessageWrapperComponent_1.CustomMessageWrapperComponent();
        _this.BalanceDetailsResultList = new ObservableCollection_1.ObservableCollection([]);
        _this.BankAccountsResultList = new ObservableCollection_1.ObservableCollection([]);
        return _this;
    }
    CreditLimitQueryComponent.prototype.ngAfterViewInit = function () {
        if (this.SuperCustomMessageWrapperComponent == null) {
            console.warn("SuperCustomMessageWrapperComponent.ngAfterViewInit MyCustomMessageWrapperComponent == null");
        }
        else {
            console.log("SuperCustomMessageWrapperComponent.ngAfterViewInit MyCustomMessageWrapperComponent != null");
        }
        this.MyCustomMessageWrapperComponent = this.SuperCustomMessageWrapperComponent;
        this.subscribeWrapperComponent();
    };
    CreditLimitQueryComponent.prototype.OnMassageDisplayMethod = function () {
        var _this = this;
        if (this.RequestParams == null) {
            this.RequestParams = new CreditQueryRequestParams_1.CreditQueryRequestParams();
            this.StartDate = new Date();
            this.EndDate = new Date();
            if (this._CustomsSettingListService == null) {
                this._CustomsSettingListService = new CustomsSettingListService_1.CustomsSettingListService();
            }
            this._CustomsSettingListService.getSingleFromCache(SessionLocator_1.SessionLocator.Tenant.toString())
                .subscribe(function (customsSettingList) {
                if (customsSettingList) {
                    _this.AgentCode = customsSettingList.Result ? customsSettingList.Result.CustomsAgentId : null;
                }
            });
        }
        if (this.ResponseData) {
            if (this.ResponseData.BalanceDetailsList) {
                this.BalanceDetailsResultList.InsertCollection(this.ResponseData.BalanceDetailsList);
            }
            if (this.ResponseData.BankAccountsList) {
                this.BankAccountsResultList.InsertCollection(this.ResponseData.BankAccountsList);
            }
        }
    };
    Object.defineProperty(CreditLimitQueryComponent.prototype, "AgentCode", {
        //#region Properties
        get: function () { return this.RequestParams ? this.RequestParams.AgentExternalId : null; },
        set: function (value) {
            if (this.RequestParams.AgentExternalId != value) {
                this.RequestParams.AgentExternalId = value;
            }
            if (value) {
                this.UIProperties.SetEnabled("AgentNo", null, false);
            }
            else {
                this.UIProperties.SetEnabled("AgentNo", null, true);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CreditLimitQueryComponent.prototype, "AgentNo", {
        get: function () { return this.RequestParams ? this.RequestParams.AgentID : null; },
        set: function (value) {
            if (this.RequestParams.AgentID != value) {
                this.RequestParams.AgentID = value;
            }
            if (value) {
                this.UIProperties.SetEnabled("AgentCode", null, false);
            }
            else {
                this.UIProperties.SetEnabled("AgentCode", null, true);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CreditLimitQueryComponent.prototype, "StartDate", {
        get: function () { return this.RequestParams ? this.RequestParams.DateFrom : null; },
        set: function (value) {
            if (this.RequestParams.DateFrom != value) {
                this.RequestParams.DateFrom = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CreditLimitQueryComponent.prototype, "EndDate", {
        get: function () { return this.RequestParams ? this.RequestParams.DateTo : null; },
        set: function (value) {
            if (this.RequestParams.DateTo != value) {
                this.RequestParams.DateTo = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CreditLimitQueryComponent.prototype, "ImporterCode", {
        get: function () { return this.RequestParams ? this.RequestParams.ExtertnalID : null; },
        set: function (value) {
            if (this.RequestParams.ExtertnalID != value) {
                this.RequestParams.ExtertnalID = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CreditLimitQueryComponent.prototype, "ImporterName", {
        get: function () { return this._ImporterName; },
        set: function (value) {
            if (this._ImporterName != value) {
                this._ImporterName = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    //#endregion Properties
    //#region Importer Commands
    CreditLimitQueryComponent.prototype.ImporterClicked = function (client) {
        this.ImporterCode = client.Code;
        this.ImporterName = Tools_1.AppTool.IsNullOrEmpty(client) ? "" : client.FullName;
    };
    CreditLimitQueryComponent.prototype.ImporterTextChanged = function (item) {
        if (item == "") {
            this.ImporterCode = null;
            this.ImporterName = "";
        }
    };
    //#endregion
    //#region General Commands
    CreditLimitQueryComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    CreditLimitQueryComponent.prototype.OnCustomSendOptionsButtonClick = function (customSendOptionsArgs) {
        var _this = this;
        var currRequestParams = new CreditQueryRequestParams_1.CreditQueryRequestParams();
        currRequestParams.LoggingEnabled = true;
        currRequestParams.LoggingUserId = SessionLocator_1.SessionLocator.LoggedUserId;
        currRequestParams.RequestVIA = customSendOptionsArgs.RequestVIA;
        currRequestParams.ForcePersonalSign = customSendOptionsArgs.ForcePersonalSign;
        currRequestParams.Tenant = SessionLocator_1.SessionLocator.Tenant;
        currRequestParams.DateFrom = this.StartDate;
        currRequestParams.DateTo = this.EndDate;
        currRequestParams.AgentID = this.AgentNo;
        currRequestParams.AgentExternalId = this.AgentCode;
        currRequestParams.ExtertnalID = this.ImporterCode;
        CustomMessageProgressComponent_1.CustomMessageProgressComponent
            .ShowProgressBar(currRequestParams.PBId, "שליחת שאילתא לתקרת אשראי", true)
            .then(function (res) {
            _this.ResponseData = res;
            _this.OnMassageDisplayMethod();
        }).catch(function (err) {
            _this.ValidationErrorsList.push(err);
        });
        this._IIGGeneralMessagesService.PostCreditQueryRequest(currRequestParams)
            .subscribe(function (myServiceResponse) {
        });
    };
    __decorate([
        core_1.ViewChild(CustomMessageWrapperComponent_1.CustomMessageWrapperComponent),
        __metadata("design:type", CustomMessageWrapperComponent_1.CustomMessageWrapperComponent)
    ], CreditLimitQueryComponent.prototype, "SuperCustomMessageWrapperComponent", void 0);
    CreditLimitQueryComponent = __decorate([
        core_1.Component({
            selector: 'CreditLimitQueryComponent',
            moduleId: module.id,
            templateUrl: './CreditLimitQueryComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], CreditLimitQueryComponent);
    return CreditLimitQueryComponent;
}(BaseRequestsSheetMassaging_1.BaseRequestsSheetMassaging));
exports.CreditLimitQueryComponent = CreditLimitQueryComponent;
//# sourceMappingURL=CreditLimitQueryComponent.js.map