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
var ClientMessagesService_1 = require("../../../Customs/Services/WebServices/ClientMessagesService");
var ClientSearchRequestParams_1 = require("../../../Customs/DataContract/RequestParams/ClientSearchRequestParams");
var ClientSearchByIDResponseData_1 = require("../../../Customs/DataContract/ResponseData/ClientSearchByIDResponseData");
var Validator_1 = require("../../../Infrastructure/Validators/Validator");
var TextCodeTranslator_1 = require("../../../Infrastructure/Utilities/TextCodeTranslator");
var Tools_1 = require("../../../Infrastructure/Tools");
var BaseRequestsSheetMassaging_1 = require("../../../CustomsModules/CustomsRequests/Components/BaseRequestsSheetMassaging");
var CustomMessageProgressComponent_1 = require("../../../CustomsModules/CustomsControls/Components/CustomMessageProgressComponent");
var ObservableCollection_1 = require("../../../Infrastructure/Utilities/ObservableCollection");
var LogitudeWindow_1 = require("../../../Controls/Windows/LogitudeWindow");
var ClientSearchByIDComponent = /** @class */ (function (_super) {
    __extends(ClientSearchByIDComponent, _super);
    function ClientSearchByIDComponent() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.ObjectTableName = "Customs.Client";
        _this._ClientMessagesService = new ClientMessagesService_1.ClientMessagesService();
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.SuperCustomMessageWrapperComponent = new CustomMessageWrapperComponent_1.CustomMessageWrapperComponent();
        _this.CustomerActivityList = new ObservableCollection_1.ObservableCollection([]);
        _this.AuthorizedList = new ObservableCollection_1.ObservableCollection([]);
        _this.AuthorizerList = new ObservableCollection_1.ObservableCollection([]);
        _this.ExportRequestList = new ObservableCollection_1.ObservableCollection([]);
        _this.IndicationPerClassificationList = new ObservableCollection_1.ObservableCollection([]);
        return _this;
    }
    ClientSearchByIDComponent.prototype.ngAfterViewInit = function () {
        if (this.SuperCustomMessageWrapperComponent == null) {
            console.warn("SuperCustomMessageWrapperComponent.ngAfterViewInit MyCustomMessageWrapperComponent == null");
        }
        else {
            console.log("SuperCustomMessageWrapperComponent.ngAfterViewInit MyCustomMessageWrapperComponent != null");
        }
        this.MyCustomMessageWrapperComponent = this.SuperCustomMessageWrapperComponent;
        this.subscribeWrapperComponent();
    };
    ClientSearchByIDComponent.prototype.OnRowLoaded = function (myRow) {
        if (myRow) {
            myRow.SetExpandaple(true);
        }
    };
    ClientSearchByIDComponent.prototype.OnMassageDisplayMethod = function () {
        if (this.RequestParams == null) {
            this.RequestParams = new ClientSearchRequestParams_1.ClientSearchRequestParams();
            this.UseExternalId();
        }
        else {
            if (!Tools_1.AppTool.IsNullOrEmpty(this.RequestParams.ExternalId)) {
                this.UseExternalId();
            }
            else {
                this.UsePassportRadio();
            }
        }
        if (this.ResponseData) {
            if (this.ResponseData.CustomerActivityList) {
                for (var _i = 0, _a = this.ResponseData.CustomerActivityList; _i < _a.length; _i++) {
                    var item = _a[_i];
                    item.CustomerIndicationList = new ObservableCollection_1.ObservableCollection(item.CustomerIndicationList);
                }
                this.CustomerActivityList.InsertCollection(this.ResponseData.CustomerActivityList);
            }
            if (this.ResponseData.AuthorizedList) {
                this.AuthorizedList.InsertCollection(this.ResponseData.AuthorizedList);
            }
            if (this.ResponseData.AuthorizerList) {
                this.AuthorizerList.InsertCollection(this.ResponseData.AuthorizerList);
            }
            if (this.ResponseData.ExportRequestList) {
                this.ExportRequestList.InsertCollection(this.ResponseData.ExportRequestList);
            }
            if (this.ResponseData.IndicationPerClassificationList) {
                this.IndicationPerClassificationList.InsertCollection(this.ResponseData.IndicationPerClassificationList);
            }
        }
    };
    Object.defineProperty(ClientSearchByIDComponent.prototype, "IsExternalId", {
        get: function () { return this.isExternalId; },
        set: function (value) {
            if (this.isExternalId != value) {
                this.isExternalId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ClientSearchByIDComponent.prototype, "IsPassport", {
        get: function () { return this.isPassport; },
        set: function (value) {
            if (this.isPassport != value) {
                this.isPassport = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ClientSearchByIDComponent.prototype, "ExternalId", {
        get: function () { return this.RequestParams ? this.RequestParams.ExternalId : null; },
        set: function (value) {
            if (this.RequestParams.ExternalId != value) {
                this.RequestParams.ExternalId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ClientSearchByIDComponent.prototype, "PassportNumber", {
        get: function () { return this.RequestParams ? this.RequestParams.PassportNumber : null; },
        set: function (value) {
            if (this.RequestParams.PassportNumber != value) {
                this.RequestParams.PassportNumber = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ClientSearchByIDComponent.prototype, "PassportTypeCode", {
        get: function () { return this.RequestParams ? this.RequestParams.PassportTypeCode : null; },
        set: function (value) {
            if (this.RequestParams.PassportTypeCode != value) {
                this.RequestParams.PassportTypeCode = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ClientSearchByIDComponent.prototype, "PassportCountryCode", {
        get: function () { return this.RequestParams ? this.RequestParams.PassportCountryCode : null; },
        set: function (value) {
            if (this.RequestParams.PassportCountryCode != value) {
                this.RequestParams.PassportCountryCode = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    //#endregion Properties
    //#region General Commands
    ClientSearchByIDComponent.prototype.UseExternalId = function () {
        this.PassportCountryCode = null;
        this.PassportTypeCode = null;
        this.PassportNumber = null;
        this.UIProperties.SetEnabled("ExternalId", "Customs.Client", true);
        this.UIProperties.SetEnabled("PassportCountryCode", "Customs.Client", false);
        this.UIProperties.SetEnabled("PassportTypeCode", "Customs.Client", false);
        this.UIProperties.SetEnabled("PassportNumber", "Customs.Client", false);
        this.IsExternalId = true;
        this.IsPassport = false;
    };
    ClientSearchByIDComponent.prototype.UsePassportRadio = function () {
        this.ExternalId = null;
        this.UIProperties.SetEnabled("ExternalId", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("PassportCountryCode", this.ObjectTableName, true);
        this.UIProperties.SetEnabled("PassportTypeCode", this.ObjectTableName, true);
        this.UIProperties.SetEnabled("PassportNumber", this.ObjectTableName, true);
        this.IsExternalId = false;
        this.IsPassport = true;
    };
    ClientSearchByIDComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    ClientSearchByIDComponent.prototype.EditCustomerIndicationCommand = function (item) {
        if (item.CustomerIndicationList == null || item.CustomerIndicationList.length == 0) {
            return;
        }
        var windowArgs = {};
        windowArgs.CustomerIndicationList = item.CustomerIndicationList;
        var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
        logitudeWindow.Width = 450;
        logitudeWindow.Height = 400;
        logitudeWindow.IsShowCloseButton = false;
        logitudeWindow.Title = "אינדיקציות ללקוח";
        logitudeWindow.WindowArgs = windowArgs;
        logitudeWindow.Show('./CustomsModules/CustomsGeneralRequests/Components/CustomerIndicationComponent');
    };
    ClientSearchByIDComponent.prototype.FillErrors = function () {
        var errors = [];
        Validator_1.Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        this.ValidationErrorsList = errors;
        if (this.IsExternalId && Tools_1.AppTool.IsNullOrEmpty(this.ExternalId)) {
            var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Client.O.CodeRequired");
            this.ValidationErrorsList.push(msg);
        }
        if (this.IsPassport && Tools_1.AppTool.IsNullOrEmpty(this.PassportNumber)) {
            var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Client.O.PassportRequired");
            this.ValidationErrorsList.push(msg);
        }
    };
    ClientSearchByIDComponent.prototype.OnCustomSendOptionsButtonClick = function (customSendOptionsArgs) {
        var _this = this;
        this.FillErrors();
        if (this.ValidationErrorsList.length > 0) {
            return;
        }
        this.ResponseData = new ClientSearchByIDResponseData_1.ClientSearchByIDResponseData();
        var currRequestParams = new ClientSearchRequestParams_1.ClientSearchRequestParams();
        currRequestParams.LoggingEnabled = true;
        currRequestParams.LoggingUserId = SessionLocator_1.SessionLocator.LoggedUserId;
        currRequestParams.RequestVIA = customSendOptionsArgs.RequestVIA;
        currRequestParams.ForcePersonalSign = customSendOptionsArgs.ForcePersonalSign;
        currRequestParams.Tenant = SessionLocator_1.SessionLocator.Tenant;
        currRequestParams.ExternalId = this.ExternalId;
        currRequestParams.PassportNumber = this.PassportNumber;
        currRequestParams.PassportTypeCode = this.PassportTypeCode;
        currRequestParams.PassportCountryCode = this.PassportCountryCode;
        CustomMessageProgressComponent_1.CustomMessageProgressComponent
            .ShowProgressBar(currRequestParams.PBId, "שליחת שאילתא לנתונים נוספים ליבואן", true)
            .then(function (res) {
            _this.ResponseData = res;
            _this.OnMassageDisplayMethod();
        }).catch(function (err) {
            _this.ValidationErrorsList.push(err);
        });
        this._ClientMessagesService.PostClientSearchByIDRequest(currRequestParams)
            .subscribe(function (myServiceResponse) {
        });
    };
    __decorate([
        core_1.ViewChild(CustomMessageWrapperComponent_1.CustomMessageWrapperComponent),
        __metadata("design:type", CustomMessageWrapperComponent_1.CustomMessageWrapperComponent)
    ], ClientSearchByIDComponent.prototype, "SuperCustomMessageWrapperComponent", void 0);
    ClientSearchByIDComponent = __decorate([
        core_1.Component({
            selector: 'ClientSearchByIDComponent',
            moduleId: module.id,
            templateUrl: './ClientSearchByIDComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], ClientSearchByIDComponent);
    return ClientSearchByIDComponent;
}(BaseRequestsSheetMassaging_1.BaseRequestsSheetMassaging));
exports.ClientSearchByIDComponent = ClientSearchByIDComponent;
//# sourceMappingURL=ClientSearchByIDComponent.js.map