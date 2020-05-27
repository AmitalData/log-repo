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
var CLAIM_2340_ClaimRequestRequestParams_1 = require("../../../../Customs/DataContract/RequestParams/CLAIM_2340_ClaimRequestRequestParams");
var CustomMessageProgressComponent_1 = require("../../../../CustomsModules/CustomsControls/Components/CustomMessageProgressComponent");
var EntityPMService_1 = require("../../../../Infrastructure/Services/EntityPMService");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var ConfirmWindow_1 = require("../../../../Controls/Windows/ConfirmWindow");
var ClaimWebService_1 = require("../../../../Customs/Services/WebServices/ClaimWebService");
var ClaimPMService_1 = require("../../../../Customs/Services/StandardPMs/ClaimPMService");
var SendClaimComponent = /** @class */ (function () {
    function SendClaimComponent(entityPMService) {
        this.entityPMService = entityPMService;
        this.ValidationErrors = [];
        this.ObjectTableName = "Customs.Claim";
        this.sendClaimsRelatedEntitiesList = [];
        this.ClaimWebService = new ClaimWebService_1.ClaimWebService();
        this.ClaimPMService = new ClaimPMService_1.ClaimPMService();
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
    }
    SendClaimComponent.prototype.Run = function (args) {
        this.EntityPM = args.EntityPM;
        this.ObjectTable = args.ObjectTable;
        this.ValidationErrors = [];
        this.presendValidationsTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.O.PreSendValidations");
        this.Listen();
    };
    SendClaimComponent.prototype.Listen = function () {
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
    SendClaimComponent.prototype.OnCustomSendOptionsButtonClick = function (customSendOptionsArgs) {
        this.RequestVIA = customSendOptionsArgs.RequestVIA;
        this.Option = customSendOptionsArgs.Option;
        this.ForcePersonalSign = customSendOptionsArgs.ForcePersonalSign;
        Validator_1.Validator.TryValidateObject(this.EntityPM, "Customs.Claim", this.ValidationErrors);
        if (this.ValidationErrors.length == 0) {
            this.SaveEntityChanges(customSendOptionsArgs, false);
        }
        else {
            this.FillValidationErrors(this.presendValidationsTitle);
        }
    };
    SendClaimComponent.prototype.SaveEntityChanges = function (customSendOptionsArgs, isDelete) {
        var _this = this;
        this.EntityPM.Tenant = SessionLocator_1.SessionLocator.Tenant;
        this.CurrentSession.CurrentEditComponent.ValidationErrorsList = [];
        if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Id)) {
            return;
        }
        this.CurrentSession.StartBusyIndicator(TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.Saving"));
        this.ClaimPMService.update(this.EntityPM).subscribe(function (myResponse) {
            if (myResponse.HasError) {
                _this.CurrentSession.CurrentEditComponent.StopBusyIndicator();
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
                                _this.CheckRequiredFields();
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
    SendClaimComponent.prototype.PostSendPaymentOrderChecksAndPrecalculations = function () {
        return true;
        //if (this.EntityPM.PaymentOrderSelectedLabel == "" && AppTool.IsNullOrEmpty(this.EntityPM.AccountingCustomFile)) {
        //    this.ValidationErrors.push(TextCodeTranslator.Translate("Customs.PaymentOrder.O.AccountingCustomFileMissing"));
        //    return false;
        //}
        //var sumOfPaymentMethods = 0;
        //if (this.EntityPM.PaymentOrderMethods) {
        //    this.EntityPM.PaymentOrderMethods.forEach((itemLine) => {
        //        sumOfPaymentMethods = sumOfPaymentMethods + itemLine.Amount;
        //    });
        //}
        //var deffirence = this.EntityPM.PaymentOrderLeftAmount - sumOfPaymentMethods;
        //if (deffirence == 0) {
        //    return true;
        //}
        //else {
        //    this.ValidationErrors.push(TextCodeTranslator.Translate("Customs.PaymentOrder.O.PaymentOrderLeftAmountDifference"));
        //    return false;
        //}
    };
    SendClaimComponent.prototype.CheckRequiredFields = function () {
        var _this = this;
        this.ClaimWebService.GetRequiredFieldsForClaim(this.EntityPM.Id).subscribe(function (response) {
            var errorsList = response.Result.RequiredFields;
            if (errorsList.length == 0) {
                _this.InstructionSendToMehes();
                return;
            }
            else {
                _this.CurrentSession.CurrentEditComponent.StopBusyIndicator();
                _this.ValidationErrors = _this.GetRequiredErrorsList(errorsList);
                _this.FillValidationErrors(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.O.RequiredFields"));
            }
        });
    };
    SendClaimComponent.prototype.GetRequiredErrorsList = function (errorsList) {
        var errorsMessages = [];
        errorsList.forEach(function (error) {
            if (!Tools_1.AppTool.IsNullOrEmpty(error.CustomMessageError)) {
                if (error.CustomMessageError.indexOf("specialerror") > -1) {
                    var ErrorMessage = "";
                    var errorArr = error.CustomMessageError.split(',');
                    ErrorMessage = errorArr[1] + TextCodeTranslator_1.TextCodeTranslator.Translate(errorArr[2]);
                    errorsMessages.push(ErrorMessage);
                }
                else {
                    errorsMessages.push(TextCodeTranslator_1.TextCodeTranslator.Translate(error.CustomMessageError));
                }
            }
            else {
                var table = window.ObjectTables.filter(function (d) { return d.Name === error.TableName; })[0];
                var field = window.ObjectFields.filter(function (d) { return d.FieldName == error.FieldName && d.ObjectTableId == table.Id; })[0];
                var message = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.O.FieldForTableIsRequired");
                var fieldName = TextCodeTranslator_1.TextCodeTranslator.Translate(field.FullNameTextCodeCode);
                var tableName = TextCodeTranslator_1.TextCodeTranslator.Translate(error.TableName);
                message = message.replace('%FieldName', fieldName);
                message = message.replace('%TableName', tableName);
                message = message.replace('%EntityReference', error.EntityReference);
                errorsMessages.push(message);
            }
        });
        return errorsMessages;
    };
    SendClaimComponent.prototype.InstructionSendToMehes = function () {
        var counter = 0;
        if (this.EntityPM.ClaimsRelatedEntities != null && this.EntityPM.ClaimsRelatedEntities.length > 0) {
            for (var _i = 0, _a = this.EntityPM.ClaimsRelatedEntities; _i < _a.length; _i++) {
                var claimsRelatedEntityItem = _a[_i];
                if (claimsRelatedEntityItem.IsSendClaimsRelatedEntity == true) {
                    this.sendClaimsRelatedEntitiesList.push(claimsRelatedEntityItem.EntityCounterKey.toString());
                }
                if (!Tools_1.AppTool.IsNullOrEmpty(claimsRelatedEntityItem.TapagNumber)) {
                    counter++;
                }
            }
            if (this.sendClaimsRelatedEntitiesList == null || (this.sendClaimsRelatedEntitiesList != null && this.sendClaimsRelatedEntitiesList.length == 0)) {
                var errorMessage = "חובה לבחור לפחות ישות תביעה אחת לשליחה";
                if (this.EntityPM.ClaimsRelatedEntities.length == counter) {
                    errorMessage = "בתביעה זו לא ניתן לבצע שליחה, כיוון שכל תיקי התביעה עודכנו וקיבלו תיק תביעה במכס";
                }
                this.CurrentSession.CurrentEditComponent.StopBusyIndicator();
                var winConfirmation = new ConfirmWindow_1.ConfirmWindow();
                winConfirmation.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Claim.G.RelatedEntitiesCheck");
                winConfirmation.Width = 250;
                winConfirmation.Height = 150;
                winConfirmation.ShowNoButton = false;
                winConfirmation.YesButtonText = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.B.OK");
                //winConfirmation.NoButtonText = TextCodeTranslator.Translate("Customs.General.B.Cancel");
                winConfirmation.ShowCancelButton = false;
                winConfirmation.Show(errorMessage);
                return;
            }
        }
        else {
            var winConfirmation = new ConfirmWindow_1.ConfirmWindow();
            winConfirmation.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Claim.G.RelatedEntitiesCheck");
            winConfirmation.Width = 250;
            winConfirmation.Height = 150;
            winConfirmation.ShowNoButton = false;
            winConfirmation.YesButtonText = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.B.OK");
            winConfirmation.ShowCancelButton = false;
            winConfirmation.Show("חובה לבחור לפחות ישות תביעה אחת לשליחה");
            return;
        }
        this.PreClaimSendChecks();
    };
    SendClaimComponent.prototype.PreClaimSendChecks = function () {
        //var ClaimValidator = new ClaimValidator(entityPM);
        //ClaimValidator.PreClaimSendChecks();
        //if (ClaimValidator.ErrorCode.Count == 0) {
        //    //בעתיד לבדוק צרופות
        //    //LoadOperation customsDocument = this.context.Load(this.context.GetClaimDocumentListQuery(entityPM.Id, "Claim", TenantContext.Current.Id), LoadBehavior.RefreshCurrent, true);
        //    //customsDocument.Completed += customsDocument_Completed;
        //    SendClaim();
        //    return;
        //}
        this.SendClaim();
    };
    SendClaimComponent.prototype.FillValidationErrors = function (title) {
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
    SendClaimComponent.prototype.OnAddEditWindowClosed = function (event) {
        this.ValidationErrors = [];
    };
    SendClaimComponent.prototype.SendClaim = function () {
        var _this = this;
        var currRequestParams = new CLAIM_2340_ClaimRequestRequestParams_1.CLAIM_2340_ClaimRequestRequestParams();
        currRequestParams.LoggingEnabled = true;
        currRequestParams.LoggingUserId = SessionLocator_1.SessionLocator.LoggedUserId;
        currRequestParams.Tenant = SessionLocator_1.SessionLocator.Tenant;
        currRequestParams.AppicationId = this.EntityPM.Id;
        currRequestParams.ClaimsRelatedEntitiesList = this.sendClaimsRelatedEntitiesList;
        currRequestParams.RequestName = "Claim Request";
        currRequestParams.ResponseName = "Claim Response";
        currRequestParams.ForcePersonalSign = this.ForcePersonalSign;
        CustomMessageProgressComponent_1.CustomMessageProgressComponent
            .ShowProgressBar(currRequestParams.PBId, "שליחת תביעה", false)
            .then(function (res) {
            console.log(res);
            _this.ResponseData = res;
            _this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
        }).catch(function (err) {
            _this.ValidationErrors.push(err);
            _this.FillValidationErrors("Errors");
        });
        var myClaimWebService = new ClaimWebService_1.ClaimWebService();
        myClaimWebService.PostSendClaimRequest(currRequestParams)
            .subscribe(function (myServiceResponse) {
        });
    };
    SendClaimComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'SendClaimComponent',
            templateUrl: "SendClaimComponent.html",
        }),
        __metadata("design:paramtypes", [EntityPMService_1.EntityPMService])
    ], SendClaimComponent);
    return SendClaimComponent;
}());
exports.SendClaimComponent = SendClaimComponent;
//# sourceMappingURL=SendClaimComponent.js.map