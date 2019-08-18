"use strict";
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
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var Tools_1 = require("../../../../Infrastructure/Tools");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var Validator_1 = require("../../../../Infrastructure/Validators/Validator");
var GenericRequestParams_1 = require("../../../../Customs/DataContract/RequestParams/GenericRequestParams");
var CustomMessageProgressComponent_1 = require("../../../../CustomsModules/CustomsControls/Components/CustomMessageProgressComponent");
var EntityPMService_1 = require("../../../../Infrastructure/Services/EntityPMService");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var PaymentOrderWebService_1 = require("../../../../Customs/Services/WebServices/PaymentOrderWebService");
var PaymentOrderPMService_1 = require("../../../../Customs/Services/StandardPMs/PaymentOrderPMService");
var SendPaymentOrderComponent = /** @class */ (function () {
    function SendPaymentOrderComponent(entityPMService) {
        this.entityPMService = entityPMService;
        this.ValidationErrors = [];
        this.ObjectTableName = "Customs.PaymentOrder";
        this.PaymentOrderPMService = new PaymentOrderPMService_1.PaymentOrderPMService();
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
    }
    SendPaymentOrderComponent.prototype.Run = function (args) {
        this.EntityPM = args.EntityPM;
        this.ObjectTable = args.ObjectTable;
        this.ValidationErrors = [];
        this.presendValidationsTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.O.PreSendValidations");
        this.Listen();
    };
    SendPaymentOrderComponent.prototype.Listen = function () {
        var _this = this;
        if (this.CurrentSession.CurrentEditComponent) {
            if (!this.SaveCompletedEvent) {
                this.SaveCompletedEvent = this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe(function (isSaveSuccess) {
                    if (isSaveSuccess) {
                        _this.EntityPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                    }
                });
            }
            if (!this.LoadCompletedEvent) {
                this.LoadCompletedEvent = this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe(function (isLoadSuccess) {
                    if (isLoadSuccess) {
                        _this.EntityPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                    }
                });
            }
        }
    };
    SendPaymentOrderComponent.prototype.OnCustomSendOptionsButtonClick = function (customSendOptionsArgs) {
        this.RequestVIA = customSendOptionsArgs.RequestVIA;
        this.Option = customSendOptionsArgs.Option;
        this.ForcePersonalSign = customSendOptionsArgs.ForcePersonalSign;
        Validator_1.Validator.TryValidateObject(this.EntityPM, "Customs.PaymentOrder", this.ValidationErrors);
        if (this.ValidationErrors.length == 0) {
            this.SaveEntityChanges(customSendOptionsArgs, false);
        }
        else {
            this.FillValidationErrors(this.presendValidationsTitle);
        }
    };
    SendPaymentOrderComponent.prototype.SaveEntityChanges = function (customSendOptionsArgs, isDelete) {
        var _this = this;
        this.EntityPM.Tenant = SessionLocator_1.SessionLocator.Tenant;
        this.CurrentSession.CurrentEditComponent.ValidationErrorsList = [];
        if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Id)) {
            return;
        }
        this.CurrentSession.StartBusyIndicator(TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.Saving"));
        this.PaymentOrderPMService.update(this.EntityPM).subscribe(function (myResponse) {
            _this.CurrentSession.StopBusyIndicator();
            if (myResponse.HasError) {
                _this.ValidationErrors = myResponse.ErrorsArray;
                _this.FillValidationErrors(_this.presendValidationsTitle);
            }
            else {
                _this.EntityPM = myResponse.Result;
                if (_this.CurrentSession.CurrentEditComponent) {
                    _this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                    _this.reloadEvent = _this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe(function (isLoadSuccess) {
                        _this.reloadEvent.unsubscribe();
                        if (isLoadSuccess) {
                            _this.EntityPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                            _this.CurrentSession.StartBusyIndicator("");
                            if (_this.PostSendPaymentOrderChecksAndPrecalculations() == true) {
                                _this.SendPaymentOrder();
                            }
                            else {
                                _this.CurrentSession.CurrentEditComponent.StopBusyIndicator();
                                _this.FillValidationErrors(_this.presendValidationsTitle);
                            }
                        }
                    });
                }
            }
        });
    };
    SendPaymentOrderComponent.prototype.PostSendPaymentOrderChecksAndPrecalculations = function () {
        if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.AccountingCustomFile)) {
            this.ValidationErrors.push(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.PaymentOrder.O.AccountingCustomFileMissing"));
            return false;
        }
        var sumOfPaymentMethods = 0;
        if (this.EntityPM.PaymentOrderMethods) {
            this.EntityPM.PaymentOrderMethods.forEach(function (itemLine) {
                sumOfPaymentMethods = sumOfPaymentMethods + itemLine.Amount;
            });
        }
        var deffirence = this.EntityPM.PaymentOrderLeftAmount - sumOfPaymentMethods;
        if (deffirence == 0) {
            return true;
        }
        else {
            this.ValidationErrors.push(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.PaymentOrder.O.PaymentOrderLeftAmountDifference"));
            return false;
        }
    };
    SendPaymentOrderComponent.prototype.FillValidationErrors = function (title) {
        var _this = this;
        var windowArgs = {};
        windowArgs.Errors = this.ValidationErrors;
        windowArgs.ComponentHeight = '328px';
        var windowTitle = title;
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Width = 600;
        logWindow.Height = 400;
        logWindow.Title = windowTitle;
        logWindow.ShowCloseButton = false;
        logWindow.WindowArgs = windowArgs;
        logWindow.WindowClosed.subscribe(function ($event) { return _this.OnAddEditWindowClosed($event); });
        logWindow.Show('./CustomsModules/CustomControls/Components/CustomsErrorsComponent');
    };
    SendPaymentOrderComponent.prototype.OnAddEditWindowClosed = function (event) {
        this.ValidationErrors = [];
    };
    SendPaymentOrderComponent.prototype.SendPaymentOrder = function () {
        var _this = this;
        var currRequestParams = new GenericRequestParams_1.GenericRequestParams();
        currRequestParams.LoggingEnabled = true;
        currRequestParams.LoggingUserId = SessionLocator_1.SessionLocator.LoggedUserId;
        currRequestParams.Tenant = SessionLocator_1.SessionLocator.Tenant;
        currRequestParams.AppicationId = this.EntityPM.Id;
        currRequestParams.RequestName = "send payment order request";
        currRequestParams.ResponseName = "send  payment order  response";
        currRequestParams.ForcePersonalSign = this.ForcePersonalSign;
        CustomMessageProgressComponent_1.CustomMessageProgressComponent
            .ShowProgressBar(currRequestParams.PBId, "שליחת בקשת תשלום הוראה", false)
            .then(function (res) {
            console.log(res);
            _this.ResponseData = res;
            _this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
        }).catch(function (err) {
            _this.ValidationErrors.push(err);
            _this.FillValidationErrors("Errors");
        });
        var myPaymentOrderWebService = new PaymentOrderWebService_1.PaymentOrderWebService();
        myPaymentOrderWebService.PostSendPaymentOrderRequest(currRequestParams)
            .subscribe(function (myServiceResponse) {
            //this.CurrentSession.StopBusyIndicator();
            //OnSendCompleted(); //to check refresh !!!! ????
            //this.BuildProtestsList();
            //RefreshProperties();
        });
    };
    SendPaymentOrderComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'SendPaymentOrderComponent',
            templateUrl: "SendPaymentOrderComponent.html",
        }),
        __metadata("design:paramtypes", [EntityPMService_1.EntityPMService])
    ], SendPaymentOrderComponent);
    return SendPaymentOrderComponent;
}());
exports.SendPaymentOrderComponent = SendPaymentOrderComponent;
//# sourceMappingURL=SendPaymentOrderComponent.js.map