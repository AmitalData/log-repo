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
var CustomsBookInRequestParams_1 = require("../../../Customs/DataContract/RequestParams/CustomsBookInRequestParams");
var TextCodeTranslator_1 = require("../../../Infrastructure/Utilities/TextCodeTranslator");
var Tools_1 = require("../../../Infrastructure/Tools");
var BaseRequestsSheetMassaging_1 = require("../../../CustomsModules/CustomsRequests/Components/BaseRequestsSheetMassaging");
var CustomMessageProgressComponent_1 = require("../../../CustomsModules/CustomsControls/Components/CustomMessageProgressComponent");
var CachedDataManager_1 = require("../../../Infrastructure/Utilities/CachedDataManager");
var CustomsBookListService_1 = require("../../../Customs/Services/StandardLists/CustomsBookListService");
var CustomsBookQueryComponent = /** @class */ (function (_super) {
    __extends(CustomsBookQueryComponent, _super);
    function CustomsBookQueryComponent() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.ObjectTableName = "Customs.Declaration"; //TODO
        _this._IIGGeneralMessagesService = new IIGGeneralMessagesService_1.IIGGeneralMessagesService();
        _this._CustomsBookListService = new CustomsBookListService_1.CustomsBookListService();
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.SuperCustomMessageWrapperComponent = new CustomMessageWrapperComponent_1.CustomMessageWrapperComponent();
        _this._MyResponseObjectToShow = null;
        return _this;
    }
    CustomsBookQueryComponent.prototype.ngAfterViewInit = function () {
        if (this.SuperCustomMessageWrapperComponent == null) {
            console.warn("SuperCustomMessageWrapperComponent.ngAfterViewInit MyCustomMessageWrapperComponent == null");
        }
        else {
            console.log("SuperCustomMessageWrapperComponent.ngAfterViewInit MyCustomMessageWrapperComponent != null");
        }
        this.MyCustomMessageWrapperComponent = this.SuperCustomMessageWrapperComponent;
        this.subscribeWrapperComponent();
    };
    CustomsBookQueryComponent.prototype.OnMassageDisplayMethod = function () {
        var _this = this;
        if (this._CustomsBookListService == null) {
            this._CustomsBookListService = new CustomsBookListService_1.CustomsBookListService();
        }
        this._CustomsBookListService.getAll().subscribe(function (serviceResponse) {
            if (_this.RequestParams == null) {
                _this.RequestParams = new CustomsBookInRequestParams_1.CustomsBookInRequestParams();
            }
            var allCustomsBookList = serviceResponse.Result;
            var tenantCustomsBookList = allCustomsBookList.filter(function (rec) { return rec.Tenant == SessionLocator_1.SessionLocator.Tenant; })[0];
            _this.UIProperties.SetRequired("FromDate", _this.ObjectTableName, true);
            _this.UIProperties.SetRequired("ToDate", _this.ObjectTableName, true);
            if (Tools_1.AppTool.IsNullOrEmpty(tenantCustomsBookList)) {
                _this.FromDate = Tools_1.DateTool.GetDateByDay(-30);
                _this.ToDate = Tools_1.DateTool.GetDateByDay(+0);
            }
            else {
                _this.FromDate = new Date(tenantCustomsBookList.LastUpdateDate.valueOf());
                _this.ToDate = Tools_1.DateTool.AddDays(new Date(tenantCustomsBookList.LastUpdateDate.valueOf()), 29);
            }
            _this.UIProperties.SetEnabled("FromDate", _this.ObjectTableName, false);
        });
        if (this.ResponseData
        //&& this.ResponseData.CurrencyRateList
        ) {
            //this.ExchangeRatesQueryObservableList.InsertCollection(this.ResponseData.CurrencyRateList);
            try {
                this._MyResponseObjectToShow = JSON.parse(this.ResponseData.ResponseStatusXML);
            }
            catch (err) {
                console.log(err);
            }
        }
    };
    Object.defineProperty(CustomsBookQueryComponent.prototype, "FromDate", {
        get: function () { return this.RequestParams != null ? this.RequestParams.fromDate : null; },
        set: function (value) {
            if (this.RequestParams.fromDate != value) {
                this.RequestParams.fromDate = value;
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
    Object.defineProperty(CustomsBookQueryComponent.prototype, "ToDate", {
        get: function () { return this.RequestParams != null ? this.RequestParams.toDate : null; },
        set: function (value) {
            if (this.RequestParams.toDate != value) {
                this.RequestParams.toDate = value;
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
    Object.defineProperty(CustomsBookQueryComponent.prototype, "IsGetHistoricalData", {
        get: function () {
            return this.RequestParams.isGetHistoricalData;
        },
        set: function (value) {
            this.RequestParams.isGetHistoricalData = value;
        },
        enumerable: true,
        configurable: true
    });
    CustomsBookQueryComponent.prototype.FillErrors = function () {
        //var errors: string[] = [];
        //Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        //this.ValidationErrorsList = errors;
        this.ValidationErrorsList = [];
        if (Tools_1.AppTool.IsNullOrEmpty(this.FromDate)) {
            var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.ExchangeRate.O.FromDateMandatory");
            this.ValidationErrorsList.push(msg);
        }
        if (Tools_1.AppTool.IsNullOrEmpty(this.ToDate)) {
            var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.ExchangeRate.O.ToDateMandatory");
            this.ValidationErrorsList.push(msg);
        }
    };
    CustomsBookQueryComponent.prototype.OnCustomSendOptionsButtonClick = function (customSendOptionsArgs) {
        var _this = this;
        //alert(customSendOptionsArgs.Option);
        this.FillErrors();
        if (this.ValidationErrorsList.length > 0) {
            return;
        }
        var currRequestParams = new CustomsBookInRequestParams_1.CustomsBookInRequestParams(); ///Force new GUID On Each Send !!
        currRequestParams.LoggingEnabled = true;
        currRequestParams.LoggingUserId = SessionLocator_1.SessionLocator.LoggedUserId;
        currRequestParams.Tenant = SessionLocator_1.SessionLocator.Tenant;
        currRequestParams.fromDate = this.FromDate;
        currRequestParams.toDate = this.ToDate;
        currRequestParams.fromDateSpecified = true;
        currRequestParams.toDateSpecified = true;
        currRequestParams.isGetHistoricalData = this.IsGetHistoricalData;
        currRequestParams.RequestVIA = customSendOptionsArgs.RequestVIA;
        currRequestParams.ForcePersonalSign = customSendOptionsArgs.ForcePersonalSign;
        CustomMessageProgressComponent_1.CustomMessageProgressComponent
            .ShowProgressBar(currRequestParams.PBId, "עדכון ספר סיווג", true)
            .then(function (res) {
            //this.ResponseData = res;
            //this.OnMassageDisplayMethod();
        }).catch(function (err) {
            _this.ValidationErrorsList.push(err);
        });
        this._IIGGeneralMessagesService.PostCustomsBookInRequestParams(currRequestParams)
            .subscribe(function (myServiceResponse) {
            //this.CurrentSession.StopBusyIndicator();
            //console.log(myServiceResponse);
            _this.ResponseData = myServiceResponse.Result;
            if (_this.ResponseData) {
                if (!_this.ResponseData.HasException && _this.ResponseData.Succeeded) {
                    CachedDataManager_1.CachedDataManager.RefreshTableData("Customs.CustomsItem", true);
                    //return responseData.ResponseStatusXML;
                    //return "ספר סיווג עודכן בהצלחה";;
                }
            }
            _this.OnMassageDisplayMethod();
        });
    };
    __decorate([
        core_1.ViewChild(CustomMessageWrapperComponent_1.CustomMessageWrapperComponent),
        __metadata("design:type", CustomMessageWrapperComponent_1.CustomMessageWrapperComponent)
    ], CustomsBookQueryComponent.prototype, "SuperCustomMessageWrapperComponent", void 0);
    CustomsBookQueryComponent = __decorate([
        core_1.Component({
            selector: 'CustomsBookQueryComponent',
            moduleId: module.id,
            templateUrl: './CustomsBookQueryComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], CustomsBookQueryComponent);
    return CustomsBookQueryComponent;
}(BaseRequestsSheetMassaging_1.BaseRequestsSheetMassaging));
exports.CustomsBookQueryComponent = CustomsBookQueryComponent;
//# sourceMappingURL=CustomsBookQueryComponent.js.map