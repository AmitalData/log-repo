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
var CustomMessageWrapperComponent_1 = require("../../../../CustomsModules/CustomsControls/Components/CustomMessageWrapperComponent");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var DeclarationMessagesService_1 = require("../../../../Customs/Services/WebServices/DeclarationMessagesService");
var ExportDeclarationDataRequestParams_1 = require("../../../../Customs/DataContract/RequestParams/ExportDeclarationDataRequestParams");
var ExportDeclarationDataResponseData_1 = require("../../../../Customs/DataContract/ResponseData/ExportDeclarationDataResponseData");
var Validator_1 = require("../../../../Infrastructure/Validators/Validator");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var Tools_1 = require("../../../../Infrastructure/Tools");
var BaseRequestsSheetMassaging_1 = require("../../../../CustomsModules/CustomsRequests/Components/BaseRequestsSheetMassaging");
var CustomMessageProgressComponent_1 = require("../../../../CustomsModules/CustomsControls/Components/CustomMessageProgressComponent");
var ObservableCollection_1 = require("../../../../Infrastructure/Utilities/ObservableCollection");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var ExportDeclarationDataComponent = /** @class */ (function (_super) {
    __extends(ExportDeclarationDataComponent, _super);
    function ExportDeclarationDataComponent() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.ObjectTableName = "Customs.Declaration";
        _this._DeclarationMessagesService = new DeclarationMessagesService_1.DeclarationMessagesService();
        _this.GovernmentProcedureList = [];
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.SuperCustomMessageWrapperComponent = new CustomMessageWrapperComponent_1.CustomMessageWrapperComponent();
        _this.InvoiceList = new ObservableCollection_1.ObservableCollection([]);
        _this.RequestList = new ObservableCollection_1.ObservableCollection([]);
        return _this;
    }
    ExportDeclarationDataComponent.prototype.ngAfterViewInit = function () {
        if (this.SuperCustomMessageWrapperComponent == null) {
            console.warn("SuperCustomMessageWrapperComponent.ngAfterViewInit MyCustomMessageWrapperComponent == null");
        }
        else {
            console.log("SuperCustomMessageWrapperComponent.ngAfterViewInit MyCustomMessageWrapperComponent != null");
        }
        this.MyCustomMessageWrapperComponent = this.SuperCustomMessageWrapperComponent;
        this.subscribeWrapperComponent();
    };
    ExportDeclarationDataComponent.prototype.OnMassageDisplayMethod = function () {
        var _this = this;
        if (this.RequestParams == null) {
            this.RequestParams = new ExportDeclarationDataRequestParams_1.ExportDeclarationDataRequestParams();
        }
        if (this.ResponseData) {
            if (this.ResponseData.InvoiceList) {
                this.InvoiceList.InsertCollection(this.ResponseData.InvoiceList);
            }
            if (this.ResponseData.RequestList) {
                this.RequestList.InsertCollection(this.ResponseData.RequestList);
            }
            if (this.ResponseData.RequestList) {
                for (var _i = 0, _a = this.ResponseData.RequestList; _i < _a.length; _i++) {
                    var item = _a[_i];
                    item.GovernmentProcedureList.forEach(function (itemLine) {
                        _this.GovernmentProcedureList.push(itemLine.ItemGovernmentProcedureType);
                    });
                    item.VehicleList = new ObservableCollection_1.ObservableCollection(item.VehicleList);
                }
                this.RequestList.InsertCollection(this.ResponseData.RequestList);
            }
            if (this.ResponseData.RequestList) {
                for (var _b = 0, _c = this.ResponseData.RequestList; _b < _c.length; _b++) {
                    var item = _c[_b];
                    item.GovernmentProcedureList.forEach(function (itemLine) {
                        _this.GovernmentProcedureList.push(itemLine.ItemGovernmentProcedureType);
                    });
                    item.VehicleList = new ObservableCollection_1.ObservableCollection(item.VehicleList);
                }
                this.RequestList.InsertCollection(this.ResponseData.RequestList);
            }
        }
        else {
            this.ResponseData = new ExportDeclarationDataResponseData_1.ExportDeclarationDataResponseData();
        }
    };
    Object.defineProperty(ExportDeclarationDataComponent.prototype, "ReshimonNubmer", {
        //#region Properties
        get: function () { return this.RequestParams ? this.RequestParams.ReshimonNubmer : null; },
        set: function (value) {
            if (this.RequestParams.ReshimonNubmer != value) {
                this.RequestParams.ReshimonNubmer = value;
                if (Tools_1.AppTool.IsNullOrEmpty(this.RequestParams.ReshimonNubmer)) {
                    this.UIProperties.SetRequired("ReshimonNubmer", this.ObjectTableName, true);
                }
                else {
                    this.UIProperties.SetRequired("ReshimonNubmer", this.ObjectTableName, false);
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ExportDeclarationDataComponent.prototype, "Title", {
        get: function () { return this.ResponseData ? this.ResponseData.Title : null; },
        set: function (value) {
            if (this.ResponseData.Title != value) {
                this.ResponseData.Title = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ExportDeclarationDataComponent.prototype, "FOBNetoNISAmount", {
        get: function () { return this.ResponseData ? this.ResponseData.FOBNetoNISAmount : null; },
        set: function (value) {
            if (this.ResponseData.FOBNetoNISAmount != value) {
                this.ResponseData.FOBNetoNISAmount = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ExportDeclarationDataComponent.prototype, "AgentCustomerExternalID", {
        get: function () { return this.ResponseData ? this.ResponseData.AgentCustomerExternalID : null; },
        set: function (value) {
            if (this.ResponseData.AgentCustomerExternalID != value) {
                this.ResponseData.AgentCustomerExternalID = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ExportDeclarationDataComponent.prototype, "FOBNISAmount", {
        get: function () { return this.ResponseData ? this.ResponseData.FOBNISAmount : null; },
        set: function (value) {
            if (this.ResponseData.FOBNISAmount != value) {
                this.ResponseData.FOBNISAmount = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ExportDeclarationDataComponent.prototype, "CalculationDate", {
        get: function () { return this.ResponseData ? this.ResponseData.CalculationDate : null; },
        set: function (value) {
            if (this.ResponseData.CalculationDate != value) {
                this.ResponseData.CalculationDate = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ExportDeclarationDataComponent.prototype, "LoadingDate", {
        get: function () { return this.ResponseData ? this.ResponseData.LoadingDate : null; },
        set: function (value) {
            if (this.ResponseData.LoadingDate != value) {
                this.ResponseData.LoadingDate = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    //#region General Commands
    ExportDeclarationDataComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    ExportDeclarationDataComponent.prototype.FillErrors = function () {
        var errors = [];
        Validator_1.Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        this.ValidationErrorsList = errors;
        if (Tools_1.AppTool.IsNullOrEmpty(this.ReshimonNubmer)) {
            this.ValidationErrorsList.push(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.ExportDeclarationDataQuery.O.ReshimonNubmerMandatory"));
        }
    };
    ExportDeclarationDataComponent.prototype.OnCustomSendOptionsButtonClick = function (customSendOptionsArgs) {
        var _this = this;
        this.FillErrors();
        if (this.ValidationErrorsList.length > 0) {
            return;
        }
        var currRequestParams = new ExportDeclarationDataRequestParams_1.ExportDeclarationDataRequestParams();
        currRequestParams.LoggingEnabled = true;
        currRequestParams.LoggingUserId = SessionLocator_1.SessionLocator.LoggedUserId;
        currRequestParams.RequestVIA = customSendOptionsArgs.RequestVIA;
        currRequestParams.ForcePersonalSign = customSendOptionsArgs.ForcePersonalSign;
        currRequestParams.Tenant = SessionLocator_1.SessionLocator.Tenant;
        currRequestParams.ReshimonNubmer = this.ReshimonNubmer;
        CustomMessageProgressComponent_1.CustomMessageProgressComponent
            .ShowProgressBar(currRequestParams.PBId, "שליחת שאילתא להצהרה יצוא", true)
            .then(function (res) {
            _this.ResponseData = res;
            _this.OnMassageDisplayMethod();
        }).catch(function (err) {
            _this.ValidationErrorsList.push(err);
        });
        this._DeclarationMessagesService.PostExportDeclarationDataRequest(currRequestParams)
            .subscribe(function (myServiceResponse) {
        });
    };
    ExportDeclarationDataComponent.prototype.EditButtonClicked = function (item) {
        if (this.GovernmentProcedureList != null && this.GovernmentProcedureList.length > 0) {
            var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
            logitudeWindow.Width = 300;
            logitudeWindow.Height = 380;
            logitudeWindow.IsShowCloseButton = true;
            logitudeWindow.Title = "תהליכים לסחורה";
            logitudeWindow.WindowArgs = this.GovernmentProcedureList;
            //logitudeWindow.WindowClosed.subscribe(($event: any) => this.OnCustomFilesScreenWindowClosed($event));
            logitudeWindow.Show('./CustomsModules/CustomsPaymentOrder/Components/EditTabs/General/AccountingCustomFilesComponent');
        }
    };
    ExportDeclarationDataComponent.prototype.VehicleButtonClicked = function (item) {
        if (item.VehicleList != null && item.VehicleList.Collection.length > 0) {
            var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
            logitudeWindow.Width = 380;
            logitudeWindow.Height = 380;
            logitudeWindow.IsShowCloseButton = true;
            logitudeWindow.Title = "רכבים לסחורה";
            logitudeWindow.WindowArgs = item.VehicleList;
            //logitudeWindow.Show('./Customs/Components/CustomsRequests/DeclarationRequests/VehicleForGoodsItemComponent');
            logitudeWindow.Show('./CustomsModules/CustomsRequests/Components/DeclarationRequests/VehicleForGoodsItemComponent');
        }
    };
    __decorate([
        core_1.ViewChild(CustomMessageWrapperComponent_1.CustomMessageWrapperComponent),
        __metadata("design:type", CustomMessageWrapperComponent_1.CustomMessageWrapperComponent)
    ], ExportDeclarationDataComponent.prototype, "SuperCustomMessageWrapperComponent", void 0);
    ExportDeclarationDataComponent = __decorate([
        core_1.Component({
            selector: 'ExportDeclarationDataComponent',
            moduleId: module.id,
            templateUrl: './ExportDeclarationDataComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], ExportDeclarationDataComponent);
    return ExportDeclarationDataComponent;
}(BaseRequestsSheetMassaging_1.BaseRequestsSheetMassaging));
exports.ExportDeclarationDataComponent = ExportDeclarationDataComponent;
//# sourceMappingURL=ExportDeclarationDataComponent.js.map