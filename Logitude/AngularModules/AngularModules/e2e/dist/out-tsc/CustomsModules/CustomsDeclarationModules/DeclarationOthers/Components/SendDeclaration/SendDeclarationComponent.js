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
var SessionLocator_1 = require("../../../../../Infrastructure/Utilities/SessionLocator");
var Tools_1 = require("../../../../../Infrastructure/Tools");
var DeclarationWebService_1 = require("../../../../../Customs/Services/WebServices/DeclarationWebService");
var TextCodeTranslator_1 = require("../../../../../Infrastructure/Utilities/TextCodeTranslator");
var LogitudeWindow_1 = require("../../../../../Controls/Windows/LogitudeWindow");
var Validator_1 = require("../../../../../Infrastructure/Validators/Validator");
var CustomsExchangeRateExtendedPMService_1 = require("../../../../../Customs/Services/ExtendedPMs/CustomsExchangeRateExtendedPMService");
var DeclarationValidator_1 = require("../../../../../Customs/Validators/DeclarationValidator");
var DeclarationPMService_1 = require("../../../../../Customs/Services/StandardPMs/DeclarationPMService");
var GenericRequestParams_1 = require("../../../../../Customs/DataContract/RequestParams/GenericRequestParams");
var CustomMessageProgressComponent_1 = require("../../../../CustomsControls/Components/CustomMessageProgressComponent");
var SupplierInvoicePMService_1 = require("../../../../../Customs/Services/StandardPMs/SupplierInvoicePMService");
var SupplierInvoiceExtendedPMService_1 = require("../../../../../Customs/Services/ExtendedPMs/SupplierInvoiceExtendedPMService");
var AmitalGatewayUtil_1 = require("../../../../../Infrastructure/Utilities/AmitalGatewayUtil");
var UnifreightController_1 = require("../../../../../Customs/Controller/UnifreightController");
var FeatureLocator_1 = require("../../../../../Infrastructure/Utilities/FeatureLocator");
var AddEditSupplierInvoiceComponent_1 = require("../../../DeclarationSupplierInvoice/Components/SupplierInvoices/AddEditSupplierInvoiceComponent");
var SendDeclarationComponent = /** @class */ (function () {
    function SendDeclarationComponent() {
        this.customsExchangeRateExtendedPMService = new CustomsExchangeRateExtendedPMService_1.CustomsExchangeRateExtendedPMService();
        this.supplierInvoicePMService = new SupplierInvoicePMService_1.SupplierInvoicePMService();
        this._SupplierInvoiceExtendedPMService = new SupplierInvoiceExtendedPMService_1.SupplierInvoiceExtendedPMService();
        this.declarationPMService = new DeclarationPMService_1.DeclarationPMService();
        this._SendDeclarationService = new SendDeclarationService();
        this._WorkWithService = true;
        //------------------------------------------------------//
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
    }
    SendDeclarationComponent.prototype.Run = function (args) {
        this.EntityPM = args.EntityPM;
        if (!this.EntityPM.IsCourierDeclaration) {
            this.ButtonText = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.Send");
        }
        else {
            this.ButtonText = "שלח הצהרה"; // TextCodeTranslator.Translate("Customs.Declaration.O.SendDeclaration");
        }
        if (this._WorkWithService) {
            this._SendDeclarationService.Run(args);
            return;
        }
    };
    SendDeclarationComponent.prototype.Listen = function () {
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
    SendDeclarationComponent.prototype.OnCustomSendOptionsButtonClick = function (event) {
        if (this._WorkWithService) {
            this._SendDeclarationService.OnCustomSendOptionsButtonClick(event);
            return;
        }
    };
    SendDeclarationComponent.prototype.ngOnDestroy = function () {
        if (this.SaveCompletedEvent) {
            this.SaveCompletedEvent.unsubscribe();
            this.SaveCompletedEvent = null;
        }
        if (this.LoadCompletedEvent) {
            this.LoadCompletedEvent.unsubscribe();
            this.LoadCompletedEvent = null;
        }
        if (this._WorkWithService) {
            this._SendDeclarationService.ngOnDestroy();
            return;
        }
    };
    SendDeclarationComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'SendDeclarationComponent',
            templateUrl: "SendDeclarationComponent.html",
        }),
        __metadata("design:paramtypes", [])
    ], SendDeclarationComponent);
    return SendDeclarationComponent;
}());
exports.SendDeclarationComponent = SendDeclarationComponent;
var SendDeclarationService = /** @class */ (function () {
    function SendDeclarationService() {
        this.customsExchangeRateExtendedPMService = new CustomsExchangeRateExtendedPMService_1.CustomsExchangeRateExtendedPMService();
        this.supplierInvoicePMService = new SupplierInvoicePMService_1.SupplierInvoicePMService();
        this._SupplierInvoiceExtendedPMService = new SupplierInvoiceExtendedPMService_1.SupplierInvoiceExtendedPMService();
        this.declarationPMService = new DeclarationPMService_1.DeclarationPMService();
        //------------------------------------------------------//
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.CourierWorksheetmode = false;
    }
    SendDeclarationService.prototype.Run = function (args) {
        this.EntityPM = args.EntityPM;
        this.ObjectTable = args.ObjectTable;
        this.CourierWorksheetmode = args.CourierWorksheetmode;
        this.ValidationErrors = [];
        this.presendValidationsTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.O.PreSendValidations");
        this.DeclarationService = new DeclarationWebService_1.DeclarationWebService();
        if (!this.EntityPM.IsCourierDeclaration) {
            this.ButtonText = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.Send");
        }
        else {
            this.ButtonText = "שלח הצהרה"; // TextCodeTranslator.Translate("Customs.Declaration.O.SendDeclaration");
        }
        this.Listen();
    };
    SendDeclarationService.prototype.Listen = function () {
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
    SendDeclarationService.prototype.OnCustomSendOptionsButtonClick = function (event) {
        var _this = this;
        this.CurrentSession.StartBusyIndicator("");
        this.RequestVIA = event.RequestVIA;
        this.Option = event.Option;
        this.ForcePersonalSign = event.ForcePersonalSign;
        Validator_1.Validator.TryValidateObject(this.EntityPM, "Customs.Declaration", this.ValidationErrors);
        if (this.ValidationErrors.length == 0) {
            if (this.CourierWorksheetmode) {
                this.PostSendDeclarationChecksAndPrecalculationsThenCheckRequiredFields();
                return;
            }
            // this.CurrentSession.CurrentEditComponent.SaveChanges("");
            //var firstInvoice: SupplierInvoicePM = this.EntityPM.SupplierInvoices.filter(d => d.SequenceNumeric == 1)[0];
            //if (firstInvoice && firstInvoice.InsruancePercentage && this.EntityPM.TaxationDateTime) {
            //    this.CalculateInsuranceAmount(firstInvoice);
            //}
            //else {
            {
                this.declarationPMService.update(this.EntityPM).subscribe(function (myResponse) {
                    if (myResponse.HasError) {
                        _this.ValidationErrors = myResponse.ErrorsArray;
                        _this.FillValidationErrors(_this.presendValidationsTitle);
                        _this.StopMyBusyIndicator(); ///this.CurrentSession.CurrentEditComponent.StopBusyIndicator();
                    }
                    else {
                        _this.EntityPM = myResponse.Result;
                        if (_this.CurrentSession.CurrentEditComponent) {
                            _this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                            _this.reloadEvent = _this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe(function (isLoadSuccess) {
                                _this.reloadEvent.unsubscribe();
                                if (isLoadSuccess) {
                                    _this.EntityPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                                    var asREUSEService = true;
                                    if (asREUSEService) {
                                        _this.PostSendDeclarationChecksAndPrecalculationsThenCheckRequiredFields();
                                    }
                                    else {
                                        _this.CurrentSession.StartBusyIndicator("");
                                        _this.DeclarationService.PostSendDeclarationChecksAndPrecalculations(_this.EntityPM.Id).subscribe(function (response) {
                                            if (!response.Result.HasError) {
                                                _this.CheckRequiredFields();
                                            }
                                            else {
                                                _this.StopMyBusyIndicator(); ///this.CurrentSession.CurrentEditComponent.StopBusyIndicator();
                                                _this.ValidationErrors = response.Result.ErrorMessages;
                                                _this.FillValidationErrors(TextCodeTranslator_1.TextCodeTranslator.Translate(response.Result.ErrorsType));
                                            }
                                        });
                                    }
                                }
                            });
                        }
                    }
                });
            }
        }
        else {
            this.StopMyBusyIndicator(); ///this.CurrentSession.CurrentEditComponent.StopBusyIndicator();
            this.FillValidationErrors(this.presendValidationsTitle);
        }
    };
    SendDeclarationService.prototype.PostSendDeclarationChecksAndPrecalculationsThenCheckRequiredFields = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicator("");
        this.DeclarationService.PostSendDeclarationChecksAndPrecalculations(this.EntityPM.Id).subscribe(function (response) {
            if (!response.Result.HasError) {
                _this.CheckRequiredFields();
            }
            else {
                _this.StopMyBusyIndicator(); ///this.CurrentSession.CurrentEditComponent.StopBusyIndicator();
                _this.ValidationErrors = response.Result.ErrorMessages;
                _this.FillValidationErrors(TextCodeTranslator_1.TextCodeTranslator.Translate(response.Result.ErrorsType));
            }
        });
    };
    SendDeclarationService.prototype.CheckRequiredFields = function () {
        var _this = this;
        this.DeclarationService.GetRequiredFieldsForDeclaration(this.EntityPM.Id).subscribe(function (response) {
            //List < CustomsRequiredFieldsErrorItem > errorsList = requiredFieldsErrors.RequiredFields;
            var errorsList = response.Result.RequiredFields;
            if (errorsList.length == 0) {
                if (Tools_1.AppTool.IsNullOrEmpty(_this.EntityPM.CustomFileNo) || true) { //|| !ScriptableGatewayUtil.AmitalBrowserInUse) { i put true temporarly--MM
                    _this.InstructionSendToMehes(); //this.ConfirmB4TaxationDateTimeCheck();
                    return;
                }
            }
            else {
                _this.ValidationErrors = _this.GetRequiredErrorsList(errorsList);
                _this.FillValidationErrors(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.O.RequiredFields"));
            }
        });
    };
    SendDeclarationService.prototype.InstructionSendToMehes = function () {
        var _this = this;
        if (AmitalGatewayUtil_1.AmitalGatewayUtil.Instance.IsDeclarationInUse(this.EntityPM.CustomFileNo, this.EntityPM.IsConvertedDeclaration, this.EntityPM.IsConnectedToUnifreight)) { //if (!AppTool.IsNullOrEmpty(this.EntityPM.CustomFileNo) && AmitalGatewayUtil.Instance.AmitalBrowserInUse) {
            this.StartMyBusyIndicator(""); ///this.CurrentSession.CurrentEditComponent.StartBusyIndicator("");
            var myUnifreightPrintStimulController = new UnifreightController_1.UnifreightController(this.EntityPM, "Logitude.Customs.MenuButtonHandlers.DeclarationMenuButtonsHandler.MyUnifreightPrintStimulController");
            myUnifreightPrintStimulController.SendRequestInstructionToUnifreightAsync("SENDTOMEHES");
            myUnifreightPrintStimulController.GetPromise().
                then(function (e) {
                var UnifreightResponseStatus = e.UnifreightResponseStatus;
                var UnifreightMessage = e.UnifreightMessage;
                if (UnifreightResponseStatus) {
                    //busyIndicatorStartEvent.Publish(new BusyIndicatorStartEventArgs() { Start = true, Message = TextCodeTranslator.Translate("Customs.General.O.Sending") });
                    //var IFritz_feature = FeatureLocator.Features.filter(d => d.Code == "IFRITZ")[0];
                    if (FeatureLocator_1.FeatureLocator.IsFeatureGrantedByCode("IFRITZ")) { //    o        לאחר שמירה ובדיקת שדות לשליחה, יש לבדוק Feature כפי שבודקים במסך חשבון ספק
                        console.log("FritzFeatureIsON .. ");
                        _this.UnifreightRequestExpenseFreight();
                    }
                    else {
                        _this.ConfirmB4TaxationDateTimeCheck();
                    }
                }
                else {
                    _this.StopMyBusyIndicator(); ///this.CurrentSession.CurrentEditComponent.StopBusyIndicator();
                }
            });
        }
        else {
            this.ConfirmB4TaxationDateTimeCheck();
        }
    };
    SendDeclarationService.prototype.UnifreightRequestExpenseFreight = function () {
        var _this = this;
        console.log("UnifreightRequestExpenseFreight .. ");
        this.CurrentSession.StartBusyIndicator("Check Insurance ...");
        var sub = AmitalGatewayUtil_1.AmitalGatewayUtil.Instance.UnifaceRequestArrived
            .subscribe(function (unifreightMessageM) {
            var IsMatchUnifreightCallbackCommand = (unifreightMessageM.UnifreightEntityNumber == _this.EntityPM.CustomFileNo &&
                unifreightMessageM.LogitudeViewModel == "SendDeclarationService");
            if (IsMatchUnifreightCallbackCommand) {
                sub.unsubscribe();
                console.log("UnifreightRequestExpenseFreight .. UnifaceRequestArrived ");
                var supplierInvoice_1 = null;
                supplierInvoice_1 = _this.EntityPM.SupplierInvoices[0];
                _this.CurrentSession.StartBusyIndicator(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.O.Loading"));
                _this._SupplierInvoiceExtendedPMService
                    .GetSingleSupplierInvoicePMWithLimitedItems(_this.EntityPM.Id, supplierInvoice_1.InvoiceCounterKey, 0, 0, "").subscribe(function (response) {
                    supplierInvoice_1 = response.Result;
                    var service = new AddEditSupplierInvoiceComponent_1.AnalyzeUnifreightInsuranceService();
                    service.Open(unifreightMessageM, _this.EntityPM, supplierInvoice_1);
                    if (_this.EntityPM.IsChanged) {
                        _this.UpdateReloadAndConfirmB4TaxationDateTimeCheck(supplierInvoice_1);
                    }
                    else {
                        console.log("UnifaceRequestArrived But noting change ");
                        _this.ConfirmB4TaxationDateTimeCheck();
                    }
                });
            }
        });
        AmitalGatewayUtil_1.AmitalGatewayUtil.Instance.DeclarationMessaging.RaiseCheckInsuranseReturnIsNeededAmount(this.EntityPM.CustomFileNo, this.EntityPM.Id, "SendDeclarationService", "OPEN");
    };
    SendDeclarationService.prototype.UpdateReloadAndConfirmB4TaxationDateTimeCheck = function (supplierInvoice) {
        var _this = this;
        console.log("UpdateReloadAndConfirmB4TaxationDateTimeCheck 1.update");
        //o	יש לבצע שמירה מחדש של ההצהרה (וחשבון ספק)
        this.supplierInvoicePMService.update(supplierInvoice)
            //this.declarationPMService.update(this.EntityPM)
            .subscribe(function (myResponse) {
            if (myResponse.HasError) {
                console.log("UpdateReloadAndConfirmB4TaxationDateTimeCheck 2.1 update failed StopBusyIndicator");
                _this.StopMyBusyIndicator(); ///this.CurrentSession.CurrentEditComponent.StopBusyIndicator();
            }
            else {
                _this.EntityPM = myResponse.Result;
                console.log("UpdateReloadAndConfirmB4TaxationDateTimeCheck 2.2.1 update Success");
                if (_this.CurrentSession.CurrentEditComponent) {
                    //o	יש לבצע רענון לנתוני client
                    console.log("UpdateReloadAndConfirmB4TaxationDateTimeCheck 2.2.2 Reload");
                    _this.reloadEvent = _this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe(function (isLoadSuccess) {
                        _this.reloadEvent.unsubscribe();
                        console.log("UpdateReloadAndConfirmB4TaxationDateTimeCheck 2.2.3 Continue to this.ConfirmB4TaxationDateTimeCheck();");
                        //o	לאחר מכן להמשיך בתהליך השליחה למכס
                        _this.ConfirmB4TaxationDateTimeCheck();
                    });
                    _this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                }
            }
        });
    };
    SendDeclarationService.prototype.ConfirmB4TaxationDateTimeCheck = function () {
        var _this = this;
        var declarationValidator = new DeclarationValidator_1.DeclarationValidator();
        declarationValidator.SetEntityPM(this.EntityPM);
        var taxationDateTimeMessage = declarationValidator.TaxationDateTimeCheck();
        if (Tools_1.AppTool.IsNullOrEmpty(taxationDateTimeMessage)) {
            this.CheckCertificateStatus();
        }
        else {
            this.ValidationErrors.push(TextCodeTranslator_1.TextCodeTranslator.Translate(taxationDateTimeMessage));
            var windowArgs = {};
            windowArgs.Errors = this.ValidationErrors;
            windowArgs.NoButtonVisibility = true;
            windowArgs.CancelButtonVisibility = true;
            windowArgs.NoButtonText = "לא";
            windowArgs.SaveButtonText = "עדכן";
            windowArgs.CancelButtonText = "בטל";
            windowArgs.ComponentHeight = '328px';
            var windowTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.O.TaxationDateTimeCheck");
            var logWindow = new LogitudeWindow_1.LogitudeWindow();
            logWindow.Width = 600;
            logWindow.Height = 400;
            logWindow.Title = windowTitle;
            logWindow.ShowCloseButton = false;
            logWindow.WindowArgs = windowArgs;
            logWindow.WindowClosed.subscribe(function ($event) { return _this.TaxationWindowClosed($event); });
            logWindow.Show('./CustomsModules/CustomControls/Components/CustomsErrorsComponent');
            this.StopMyBusyIndicator(); ///this.CurrentSession.CurrentEditComponent.StopBusyIndicator();
            // this.FillValidationErrors(TextCodeTranslator.Translate("Customs.General.O.TaxationDateTimeCheck"));
        }
    };
    SendDeclarationService.prototype.TaxationWindowClosed = function (event) {
        var _this = this;
        this.ValidationErrors = [];
        switch (event) {
            case "ok": {
                this.StartMyBusyIndicator(""); ///this.CurrentSession.CurrentEditComponent.StartBusyIndicator("");
                this.EntityPM.TaxationDateTime = Tools_1.DateTool.GetCurrentDateAsUtc();
                var declarationPMService = new DeclarationPMService_1.DeclarationPMService();
                declarationPMService.update(this.EntityPM).subscribe(function (myResponse) {
                    if (myResponse.HasError) {
                        _this.ValidationErrors = myResponse.ErrorsArray;
                        _this.FillValidationErrors(_this.presendValidationsTitle);
                    }
                    else {
                        _this.EntityPM = myResponse.Result;
                        _this.CheckCertificateStatus();
                    }
                });
                break;
            }
            case "no": {
                this.StartMyBusyIndicator(""); ///this.CurrentSession.CurrentEditComponent.StartBusyIndicator("");
                this.CheckCertificateStatus();
                break;
            }
            case "cancel": {
                break;
            }
        }
    };
    SendDeclarationService.prototype.CheckCertificateStatus = function () {
        var _this = this;
        this.StartMyBusyIndicator(""); ///this.CurrentSession.CurrentEditComponent.StartBusyIndicator("");// avoid resend
        this.DeclarationService.CheckCertificateStatus(this.EntityPM.Id).subscribe(function (myResponse) {
            //List < CustomsRequiredFieldsErrorItem > items = requiredFieldsErrors.RequiredFields;
            var items = myResponse.Result.RequiredFields;
            var warningsList = [];
            items.forEach(function (item) {
                warningsList.push("חשבון ספק " + item.EntityReference + " - שורה  " + item.EntityReference2 + " מסך אישורים - לא הוזנו שדות החובה שנדרשים לאישור הנ”ל ");
            });
            if (warningsList.length > 0) {
                _this.ValidationErrors = warningsList;
                var windowArgs = {};
                windowArgs.Errors = _this.ValidationErrors;
                windowArgs.NoButtonVisibility = false;
                windowArgs.CancelButtonVisibility = true;
                windowArgs.SaveButtonText = "שלח נוכחי";
                windowArgs.CancelButtonText = "בטל שליחה";
                windowArgs.ComponentHeight = '328px';
                var windowTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.O.PreSendValidations");
                var logWindow = new LogitudeWindow_1.LogitudeWindow();
                logWindow.Width = 600;
                logWindow.Height = 400;
                logWindow.Title = windowTitle;
                logWindow.ShowCloseButton = false;
                logWindow.WindowArgs = windowArgs;
                logWindow.WindowClosed.subscribe(function ($event) { return _this.CheckCertificateStatusClosed($event); });
                logWindow.Show('./CustomsModules/CustomControls/Components/CustomsErrorsComponent');
                _this.StopMyBusyIndicator(); ///this.CurrentSession.CurrentEditComponent.StopBusyIndicator();
            }
            else {
                _this.DeclarationSendChecks();
            }
        });
    };
    SendDeclarationService.prototype.DeclarationSendChecks = function () {
        var _this = this;
        this.StartMyBusyIndicator(""); ///this.CurrentSession.CurrentEditComponent.StartBusyIndicator("");//Avoid ReSend
        var declarationValidator = new DeclarationValidator_1.DeclarationValidator();
        declarationValidator.SetEntityPM(this.EntityPM);
        declarationValidator.PreDeclarationSendChecks();
        if (declarationValidator.ValidationErrorMessageCodes.length == 0) {
            this.DeclarationService.GetDeclarationDocumentList(this.EntityPM.Id, "Declaration").subscribe(function (myResponse) {
                var myCustomsDocumentPMList = myResponse.Result;
                if (!myCustomsDocumentPMList) {
                    _this.CheckMandatoryTickets();
                }
                else if (myCustomsDocumentPMList.length == 0) {
                    _this.CheckMandatoryTickets();
                }
                else {
                    var notSendList = myCustomsDocumentPMList.filter(function (r) { return Tools_1.AppTool.IsNullOrEmpty(r.CustomsDocId); });
                    if (notSendList.length == 0) {
                        _this.CheckMandatoryTickets();
                        return;
                    }
                    var errorMessage = "";
                    // this.StopMyBusyIndicator();///this.CurrentSession.CurrentEditComponent.StopBusyIndicator();
                    myCustomsDocumentPMList.forEach(function (customsDocumentPM) {
                        if (Tools_1.AppTool.IsNullOrEmpty(customsDocumentPM.CustomsDocId)) {
                            errorMessage = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.O.DocumetsUploaded");
                        }
                    });
                    if (Tools_1.AppTool.IsNullOrEmpty(errorMessage)) {
                        //this.SendDeclaration();
                        _this.CheckMandatoryTickets();
                    }
                    else {
                        _this.ValidationErrors.push(errorMessage);
                        var windowArgs = {};
                        windowArgs.Errors = _this.ValidationErrors;
                        windowArgs.NoButtonVisibility = false;
                        windowArgs.CancelButtonVisibility = true;
                        windowArgs.ComponentHeight = '328px';
                        var windowTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.O.DocumetsUploadedCheck");
                        var logWindow = new LogitudeWindow_1.LogitudeWindow();
                        logWindow.Width = 600;
                        logWindow.Height = 400;
                        logWindow.Title = windowTitle;
                        logWindow.ShowCloseButton = false;
                        logWindow.WindowArgs = windowArgs;
                        logWindow.WindowClosed.subscribe(function ($event) { return _this.DocumetsUploadedCheckClosed($event); });
                        logWindow.Show('./CustomsModules/CustomControls/Components/CustomsErrorsComponent');
                        _this.StopMyBusyIndicator(); ///this.CurrentSession.CurrentEditComponent.StopBusyIndicator();
                    }
                }
            });
        }
        else {
            this.StopMyBusyIndicator(); ///this.CurrentSession.CurrentEditComponent.StopBusyIndicator();
            this.ValidationErrors = declarationValidator.ValidationErrorMessageCodes;
            this.FillValidationErrors(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.O.PreSendValidations"));
        }
    };
    SendDeclarationService.prototype.CheckMandatoryTickets = function () {
        //this.StartMyBusyIndicator("");///this.CurrentSession.CurrentEditComponent.StartBusyIndicator("");//Avoid ReSend
        var _this = this;
        this.DeclarationService.GetDeclarationMandatoryTicketList(this.EntityPM.Id, "Declaration").subscribe(function (myResponse) {
            var myCustomsDocumentPMList = myResponse.Result;
            if (!myCustomsDocumentPMList) {
                _this.CheckFreightByIncoterm();
            }
            else if (myCustomsDocumentPMList.length == 0) {
                _this.CheckFreightByIncoterm();
            }
            else {
                var errorMessage = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.O.DocumetsMandatoryTicket");
                if (Tools_1.AppTool.IsNullOrEmpty(errorMessage)) {
                    _this.CheckFreightByIncoterm();
                }
                else {
                    _this.ValidationErrors.push(errorMessage);
                    var windowArgs = {};
                    windowArgs.Errors = _this.ValidationErrors;
                    windowArgs.NoButtonVisibility = false;
                    windowArgs.CancelButtonVisibility = true;
                    windowArgs.ComponentHeight = '328px';
                    var windowTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.O.DocumetsMandatoryTicket");
                    var logWindow = new LogitudeWindow_1.LogitudeWindow();
                    logWindow.Width = 600;
                    logWindow.Height = 400;
                    logWindow.Title = windowTitle;
                    logWindow.ShowCloseButton = false;
                    logWindow.WindowArgs = windowArgs;
                    logWindow.WindowClosed.subscribe(function ($event) { return _this.DocumetsTicketUploadedCheckClosed($event); });
                    logWindow.Show('./CustomsModules/CustomControls/Components/CustomsErrorsComponent');
                    _this.StopMyBusyIndicator(); ///this.CurrentSession.CurrentEditComponent.StopBusyIndicator();
                }
            }
        });
    };
    SendDeclarationService.prototype.CheckFreightByIncoterm = function () {
        //this.StartMyBusyIndicator("");///this.CurrentSession.CurrentEditComponent.StartBusyIndicator("");//Avoid ReSend
        var _this = this;
        this.DeclarationService.CheckFreightAmountsByIncoterm(this.EntityPM.Id).subscribe(function (myResponse) {
            var isFreightAmount = myResponse.Result;
            if (!isFreightAmount) {
                _this.SendDeclaration();
            }
            else {
                //var errorMessage = TextCodeTranslator.Translate("Customs.General.O.FreightIncotermMandatory");
                var errorMessage = "קיימים נתוני ערך הובלה אך תנאי המכר בתיק אינם דורשים זאת , להמשיך ? ";
                if (Tools_1.AppTool.IsNullOrEmpty(errorMessage)) {
                    _this.SendDeclaration();
                }
                else {
                    _this.ValidationErrors.push(errorMessage);
                    var windowArgs = {};
                    windowArgs.Errors = _this.ValidationErrors;
                    windowArgs.NoButtonVisibility = false;
                    windowArgs.CancelButtonVisibility = true;
                    windowArgs.ComponentHeight = '328px';
                    var windowTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.O.FreightIncotermMandatory");
                    var logWindow = new LogitudeWindow_1.LogitudeWindow();
                    logWindow.Width = 600;
                    logWindow.Height = 400;
                    logWindow.Title = windowTitle;
                    logWindow.ShowCloseButton = false;
                    logWindow.WindowArgs = windowArgs;
                    logWindow.WindowClosed.subscribe(function ($event) { return _this.FreightByIncotermUploadedCheckClosed($event); });
                    logWindow.Show('./CustomsModules/CustomControls/Components/CustomsErrorsComponent');
                    _this.StopMyBusyIndicator(); ///this.CurrentSession.CurrentEditComponent.StopBusyIndicator();
                }
            }
        });
    };
    SendDeclarationService.prototype.DocumetsUploadedCheckClosed = function (event) {
        this.StartMyBusyIndicator(""); ///this.CurrentSession.CurrentEditComponent.StartBusyIndicator("");
        this.ValidationErrors = [];
        switch (event) {
            case "ok": {
                this.CheckMandatoryTickets();
                break;
            }
            case "cancel": {
                this.StopMyBusyIndicator(); ///this.CurrentSession.CurrentEditComponent.StopBusyIndicator();
                break;
            }
        }
    };
    SendDeclarationService.prototype.DocumetsTicketUploadedCheckClosed = function (event) {
        this.StartMyBusyIndicator(""); ///this.CurrentSession.CurrentEditComponent.StartBusyIndicator("");
        this.ValidationErrors = [];
        switch (event) {
            case "ok": {
                this.CheckFreightByIncoterm();
                break;
            }
            case "cancel": {
                this.StopMyBusyIndicator(); ///this.CurrentSession.CurrentEditComponent.StopBusyIndicator();
                break;
            }
        }
    };
    SendDeclarationService.prototype.FreightByIncotermUploadedCheckClosed = function (event) {
        this.StartMyBusyIndicator(""); ///this.CurrentSession.CurrentEditComponent.StartBusyIndicator("");
        this.ValidationErrors = [];
        switch (event) {
            case "ok": {
                this.SendDeclaration();
                break;
            }
            case "cancel": {
                this.StopMyBusyIndicator(); ///this.CurrentSession.CurrentEditComponent.StopBusyIndicator();
                break;
            }
        }
    };
    SendDeclarationService.prototype.CheckCertificateStatusClosed = function (event) {
        this.StartMyBusyIndicator(""); ///this.CurrentSession.CurrentEditComponent.StartBusyIndicator("");
        this.ValidationErrors = [];
        switch (event) {
            case "ok": {
                this.DeclarationSendChecks();
                break;
            }
            case "cancel": {
                this.StopMyBusyIndicator(); ///this.CurrentSession.CurrentEditComponent.StopBusyIndicator();
                break;
            }
        }
    };
    SendDeclarationService.prototype.SendDeclaration = function () {
        var _this = this;
        this.StartMyBusyIndicator(""); ///this.CurrentSession.CurrentEditComponent.StartBusyIndicator("");//Avoid ReSend
        var searchParams = new GenericRequestParams_1.GenericRequestParams();
        searchParams.Tenant = SessionLocator_1.SessionLocator.Tenant;
        searchParams.AppicationId = this.EntityPM.Id;
        searchParams.LoggingEnabled = true;
        searchParams.LoggingEntityId = this.EntityPM.Id;
        searchParams.LoggingEntityReference = this.EntityPM.DeclarationNumber;
        searchParams.LoggingObjectTableId = this.ObjectTable.Id;
        searchParams.LoggingUserId = SessionLocator_1.SessionLocator.LoggedUserId;
        searchParams.RequestName = "Declaration Request";
        searchParams.ResponseName = "Declaration Response";
        searchParams.RequestVIA = this.RequestVIA;
        searchParams.ForcePersonalSign = this.ForcePersonalSign;
        var myShowProgressBarParams = null;
        if (this.CourierWorksheetmode) {
            myShowProgressBarParams = new CustomMessageProgressComponent_1.ShowProgressBarParams();
            myShowProgressBarParams.OnCloseCustomMessageProgressComponentMethod =
                function (response) {
                    var myResponseData = response;
                    if (myResponseData) {
                        if (myResponseData.HasException || !myResponseData.Succeeded) {
                            //do not close Win !!
                        }
                        else {
                            //if OK then  close Win !!
                            _this.OnSuccessSendMethod(_this.ResponseData);
                        }
                    }
                };
        }
        CustomMessageProgressComponent_1.CustomMessageProgressComponent
            .ShowProgressBar(searchParams.PBId, "שליחת הצהרת יבוא", false, myShowProgressBarParams)
            .then(function (res) {
            _this.ResponseData = res;
            if (_this.CourierWorksheetmode) {
            }
            else {
                if (_this.ResponseData && _this.ResponseData.ContinueProcessInBackground) {
                    _this.CurrentSession.CurrentEditComponent.EditComponentController.IsInBatchRequest = true;
                }
                else if (_this.Option == 'WB' || _this.Option == 'D') { // work around itzik shall fix the undefined problem.
                    _this.CurrentSession.CurrentEditComponent.EditComponentController.IsInBatchRequest = true;
                }
                var myDeclarationEditComponentController = _this.CurrentSession.CurrentEditComponent.EditComponentController;
                myDeclarationEditComponentController.CustomsAnswersShowManifest = false;
                _this.CurrentSession.CurrentEditComponent.PreSelectedTabCode = "DCCA";
                _this.CurrentSession.CurrentEditComponent.SetSelectedTab();
                _this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
            }
        }).catch(function (err) {
            _this.StopMyBusyIndicator(); ///this.CurrentSession.CurrentEditComponent.StopBusyIndicator();
            _this.ValidationErrors.push(err);
            _this.FillValidationErrors("Errors");
        });
        this.DeclarationService.PostSendDeclaration(searchParams).subscribe(function (response) {
            //this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
        });
    };
    SendDeclarationService.prototype.GetRequiredErrorsList = function (errorsList) {
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
                if (error.TableName == "Customs.SupplierInvoiceItem" && !Tools_1.AppTool.IsNullOrEmpty(error.EntityReference2)) {
                    error.EntityReference = error.EntityReference + " (חשבון " + error.EntityReference2 + " )";
                }
                var message = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.O.FieldForTableIsRequired");
                var fieldName = error.FieldName;
                if (field) {
                    fieldName = TextCodeTranslator_1.TextCodeTranslator.Translate(field.FullNameTextCodeCode);
                }
                var tableName = TextCodeTranslator_1.TextCodeTranslator.Translate(error.TableName);
                message = message.replace('%FieldName', fieldName);
                message = message.replace('%TableName', tableName);
                message = message.replace('%EntityReference', error.EntityReference);
                errorsMessages.push(message);
            }
        });
        return errorsMessages;
    };
    SendDeclarationService.prototype.OnAddEditWindowClosed = function (event) {
        this.ValidationErrors = [];
    };
    SendDeclarationService.prototype.FillValidationErrors = function (title) {
        var _this = this;
        this.StopMyBusyIndicator(); ///this.CurrentSession.CurrentEditComponent.StopBusyIndicator();
        var windowArgs = {};
        windowArgs.Errors = this.ValidationErrors;
        windowArgs.ComponentHeight = '328px'; // بدك تقيم 72 
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
    SendDeclarationService.prototype.StopMyBusyIndicator = function () {
        if (this.CourierWorksheetmode) {
            this.CurrentSession.StopBusyIndicator();
        }
        else {
            this.CurrentSession.CurrentEditComponent.StopBusyIndicator();
        }
    };
    SendDeclarationService.prototype.StartMyBusyIndicator = function (mess) {
        ///this.CurrentSession.CurrentEditComponent.StartBusyIndicator("");
        if (this.CourierWorksheetmode) {
            this.CurrentSession.StartBusyIndicator(mess);
        }
        else {
            this.CurrentSession.CurrentEditComponent.StartBusyIndicator(mess);
        }
    };
    SendDeclarationService.prototype.ngOnDestroy = function () {
        if (this.SaveCompletedEvent) {
            this.SaveCompletedEvent.unsubscribe();
            this.SaveCompletedEvent = null;
        }
        if (this.LoadCompletedEvent) {
            this.LoadCompletedEvent.unsubscribe();
            this.LoadCompletedEvent = null;
        }
    };
    return SendDeclarationService;
}());
exports.SendDeclarationService = SendDeclarationService;
//# sourceMappingURL=SendDeclarationComponent.js.map