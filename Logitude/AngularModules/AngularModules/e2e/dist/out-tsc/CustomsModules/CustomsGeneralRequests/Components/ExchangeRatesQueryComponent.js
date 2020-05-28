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
var ExchangeRatesQueryRequestParams_1 = require("../../../Customs/DataContract/RequestParams/ExchangeRatesQueryRequestParams");
var Validator_1 = require("../../../Infrastructure/Validators/Validator");
var TextCodeTranslator_1 = require("../../../Infrastructure/Utilities/TextCodeTranslator");
var Tools_1 = require("../../../Infrastructure/Tools");
var BaseRequestsSheetMassaging_1 = require("../../../CustomsModules/CustomsRequests/Components/BaseRequestsSheetMassaging");
var ObservableCollection_1 = require("../../../Infrastructure/Utilities/ObservableCollection");
var CustomMessageProgressComponent_1 = require("../../../CustomsModules/CustomsControls/Components/CustomMessageProgressComponent");
var ExchangeRatesQueryComponent = /** @class */ (function (_super) {
    __extends(ExchangeRatesQueryComponent, _super);
    function ExchangeRatesQueryComponent() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.ObjectTableName = "Customs.Declaration"; //TODO
        _this._IIGGeneralMessagesService = new IIGGeneralMessagesService_1.IIGGeneralMessagesService();
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.SuperCustomMessageWrapperComponent = new CustomMessageWrapperComponent_1.CustomMessageWrapperComponent();
        _this.ExchangeRatesQueryObservableList = new ObservableCollection_1.ObservableCollection([]);
        return _this;
    }
    ExchangeRatesQueryComponent.prototype.ngAfterViewInit = function () {
        if (this.SuperCustomMessageWrapperComponent == null) {
            console.warn("SuperCustomMessageWrapperComponent.ngAfterViewInit MyCustomMessageWrapperComponent == null");
        }
        else {
            console.log("SuperCustomMessageWrapperComponent.ngAfterViewInit MyCustomMessageWrapperComponent != null");
        }
        this.MyCustomMessageWrapperComponent = this.SuperCustomMessageWrapperComponent;
        this.subscribeWrapperComponent();
    };
    ExchangeRatesQueryComponent.prototype.OnMassageDisplayMethod = function () {
        if (this.RequestParams == null) {
            this.RequestParams = new ExchangeRatesQueryRequestParams_1.ExchangeRatesQueryRequestParams();
            this.UIProperties.SetRequired("FromDate", this.ObjectTableName, true);
            this.UIProperties.SetRequired("ToDate", this.ObjectTableName, true);
        }
        if (this.ResponseData && this.ResponseData.CurrencyRateList) {
            this.ExchangeRatesQueryObservableList.InsertCollection(this.ResponseData.CurrencyRateList);
        }
    };
    Object.defineProperty(ExchangeRatesQueryComponent.prototype, "FromDate", {
        get: function () { return this.RequestParams.FromDate; },
        set: function (value) {
            if (this.RequestParams.FromDate != value) {
                this.RequestParams.FromDate = value;
                if (value) {
                    this.UIProperties.SetRequired("FromDate", this.ObjectTableName, false);
                }
                else {
                    this.UIProperties.SetRequired("FromDate", this.ObjectTableName, true);
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ExchangeRatesQueryComponent.prototype, "ToDate", {
        get: function () { return this.RequestParams.ToDate; },
        set: function (value) {
            if (this.RequestParams.ToDate != value) {
                this.RequestParams.ToDate = value;
                if (value) {
                    this.UIProperties.SetRequired("ToDate", this.ObjectTableName, false);
                }
                else {
                    this.UIProperties.SetRequired("ToDate", this.ObjectTableName, true);
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ExchangeRatesQueryComponent.prototype, "CurrencyTypeCode", {
        get: function () { return this.RequestParams.CurrencyTypeCode; },
        set: function (value) {
            if (this.RequestParams.CurrencyTypeCode != value) {
                this.RequestParams.CurrencyTypeCode = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ExchangeRatesQueryComponent.prototype, "CurrencyTypeName", {
        get: function () { return this.RequestParams.CurrencyTypeName; },
        set: function (value) {
            if (this.RequestParams.CurrencyTypeName != value) {
                this.RequestParams.CurrencyTypeName = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ExchangeRatesQueryComponent.prototype, "CustomsCurrencyRate", {
        get: function () { return this.RequestParams.CustomsCurrencyRate; },
        set: function (value) {
            if (this.RequestParams.CustomsCurrencyRate != value) {
                this.RequestParams.CustomsCurrencyRate = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ExchangeRatesQueryComponent.prototype, "StartDate", {
        get: function () { return this.RequestParams.StartDate; },
        set: function (value) {
            if (this.RequestParams.StartDate != value) {
                this.RequestParams.StartDate = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    ExchangeRatesQueryComponent.prototype.FillErrors = function () {
        var errors = [];
        Validator_1.Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        this.ValidationErrorsList = errors;
        if (Tools_1.AppTool.IsNullOrEmpty(this.FromDate)) {
            var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.ExchangeRate.O.FromDateMandatory");
            this.ValidationErrorsList.push(msg);
        }
        if (Tools_1.AppTool.IsNullOrEmpty(this.ToDate)) {
            var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.ExchangeRate.O.ToDateMandatory");
            this.ValidationErrorsList.push(msg);
        }
    };
    ExchangeRatesQueryComponent.prototype.OnCustomSendOptionsButtonClick = function (customSendOptionsArgs) {
        var _this = this;
        //alert(customSendOptionsArgs.Option);
        this.FillErrors();
        if (this.ValidationErrorsList.length > 0) {
            return;
        }
        this.ExchangeRatesQueryObservableList.Clear();
        var currRequestParams = new ExchangeRatesQueryRequestParams_1.ExchangeRatesQueryRequestParams(); ///Force new GUID On Each Send !!
        currRequestParams.LoggingEnabled = true;
        currRequestParams.LoggingUserId = SessionLocator_1.SessionLocator.LoggedUserId;
        currRequestParams.Tenant = SessionLocator_1.SessionLocator.Tenant;
        currRequestParams.FromDate = this.FromDate;
        currRequestParams.ToDate = this.ToDate;
        currRequestParams.CurrencyTypeId = this.CurrencyTypeCode;
        currRequestParams.RequestVIA = customSendOptionsArgs.RequestVIA;
        currRequestParams.ForcePersonalSign = customSendOptionsArgs.ForcePersonalSign;
        CustomMessageProgressComponent_1.CustomMessageProgressComponent
            .ShowProgressBar(currRequestParams.PBId, "שליחת שאילתא לשערי מטבע", true)
            .then(function (res) {
            _this.ResponseData = res;
            _this.OnMassageDisplayMethod();
        }).catch(function (err) {
            _this.ValidationErrorsList.push(err);
        });
        this._IIGGeneralMessagesService.PostExchangeRatesQuery(currRequestParams)
            .subscribe(function (myServiceResponse) {
            //this.CurrentSession.StopBusyIndicator();
            //console.log(myServiceResponse);
            //this.ResponseData = myServiceResponse.Result;
            //this.OnMassageDisplayMethod();
        });
    };
    __decorate([
        core_1.ViewChild(CustomMessageWrapperComponent_1.CustomMessageWrapperComponent),
        __metadata("design:type", CustomMessageWrapperComponent_1.CustomMessageWrapperComponent)
    ], ExchangeRatesQueryComponent.prototype, "SuperCustomMessageWrapperComponent", void 0);
    ExchangeRatesQueryComponent = __decorate([
        core_1.Component({
            selector: 'ExchangeRatesQueryComponent',
            moduleId: module.id,
            templateUrl: './ExchangeRatesQueryComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], ExchangeRatesQueryComponent);
    return ExchangeRatesQueryComponent;
}(BaseRequestsSheetMassaging_1.BaseRequestsSheetMassaging));
exports.ExchangeRatesQueryComponent = ExchangeRatesQueryComponent;
//# sourceMappingURL=ExchangeRatesQueryComponent.js.map