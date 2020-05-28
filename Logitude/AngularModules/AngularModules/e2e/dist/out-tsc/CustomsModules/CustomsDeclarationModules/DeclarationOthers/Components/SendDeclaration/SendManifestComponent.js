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
var DeclarationPMService_1 = require("../../../../../Customs/Services/StandardPMs/DeclarationPMService");
var CustomMessageProgressComponent_1 = require("../../../../CustomsControls/Components/CustomMessageProgressComponent");
var SupplierInvoicePMService_1 = require("../../../../../Customs/Services/StandardPMs/SupplierInvoicePMService");
var MANIFESTRequestRequestParams_1 = require("../../../../../Customs/DataContract/RequestParams/MANIFESTRequestRequestParams");
var EntityResourceService_1 = require("../../../../../Infrastructure/Services/EntityResourceService");
var SendManifestComponent = /** @class */ (function () {
    function SendManifestComponent() {
        this._SendManifestService = new SendManifestService();
        this.ButtonText = "שלח מצהר";
    }
    SendManifestComponent.prototype.Run = function (args) {
        this._SendManifestService.Run(args);
    };
    SendManifestComponent.prototype.OnCustomSendOptionsButtonClick = function (event) {
        this._SendManifestService.OnCustomSendOptionsButtonClick(event);
    };
    SendManifestComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'SendManifestComponent',
            templateUrl: "SendManifestComponent.html",
        }),
        __metadata("design:paramtypes", [])
    ], SendManifestComponent);
    return SendManifestComponent;
}());
exports.SendManifestComponent = SendManifestComponent;
var SendManifestService = /** @class */ (function () {
    function SendManifestService() {
        var _this = this;
        this.entityResourceService = new EntityResourceService_1.EntityResourceService();
        this.declarationPMService = new DeclarationPMService_1.DeclarationPMService();
        this.supplierInvoicePMService = new SupplierInvoicePMService_1.SupplierInvoicePMService();
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.CourierWorksheetmode = false;
        this.entityResourceService.getEntityResourceByTableName("Customs.CourierDeclaration").subscribe(function (response) {
            _this.entityResourceService.getEntityResourceByTableName("Customs.Declaration").subscribe(function (response) {
                _this.entityResourceService.getEntityResourceByTableName("Customs.CourierMaster").subscribe(function (response) {
                });
            });
        });
    }
    SendManifestService.prototype.Run = function (args) {
        this.CourierWorksheetmode = args.CourierWorksheetmode;
        this.EntityPM = args.EntityPM;
        this.ObjectTable = args.ObjectTable;
        this.ValidationErrors = [];
        this.presendValidationsTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.O.PreSendValidations");
        this.DeclarationService = new DeclarationWebService_1.DeclarationWebService();
        this.ButtonText = "שלח מצהר";
        this.Listen();
    };
    SendManifestService.prototype.Listen = function () {
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
    SendManifestService.prototype.OnCustomSendOptionsButtonClick = function (event) {
        var _this = this;
        this.CurrentSession.StartBusyIndicator("");
        this.RequestVIA = event.RequestVIA;
        this.Option = event.Option;
        this.ForcePersonalSign = event.ForcePersonalSign;
        Validator_1.Validator.TryValidateObject(this.EntityPM, "Customs.Declaration", this.ValidationErrors);
        if (this.ValidationErrors.length == 0) {
            // this.CurrentSession.CurrentEditComponent.SaveChanges("");
            var firstInvoice = this.EntityPM.SupplierInvoices.filter(function (d) { return d.SequenceNumeric == 1; })[0];
            //if (firstInvoice && firstInvoice.InsruancePercentage) {
            //    this.CalculateInsuranceAmount(firstInvoice);
            //}
            //else {
            if (this.CourierWorksheetmode) {
                this.PostSendDeclarationChecksAndPrecalculationsThenCheckRequiredFields();
                return;
            }
            this.declarationPMService.update(this.EntityPM).subscribe(function (myResponse) {
                if (myResponse.HasError) {
                    _this.StopMyBusyIndicator();
                    _this.ValidationErrors = myResponse.ErrorsArray;
                    _this.FillValidationErrors(_this.presendValidationsTitle);
                }
                else {
                    _this.EntityPM = myResponse.Result;
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
                                        _this.StopMyBusyIndicator();
                                        _this.ValidationErrors = response.Result.ErrorMessages;
                                        _this.FillValidationErrors(TextCodeTranslator_1.TextCodeTranslator.Translate(response.Result.ErrorsType));
                                    }
                                });
                            }
                        }
                    });
                }
            });
        }
        else {
            this.StopMyBusyIndicator();
            this.FillValidationErrors(this.presendValidationsTitle);
        }
    };
    SendManifestService.prototype.PostSendDeclarationChecksAndPrecalculationsThenCheckRequiredFields = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicator("");
        this.DeclarationService.PostSendDeclarationChecksAndPrecalculations(this.EntityPM.Id).subscribe(function (response) {
            if (!response.Result.HasError) {
                _this.CheckRequiredFields();
            }
            else {
                _this.StopMyBusyIndicator();
                _this.ValidationErrors = response.Result.ErrorMessages;
                _this.FillValidationErrors(TextCodeTranslator_1.TextCodeTranslator.Translate(response.Result.ErrorsType));
            }
        });
    };
    SendManifestService.prototype.CheckRequiredFields = function () {
        var _this = this;
        this.DeclarationService.GetRequiredFieldsForCourierDeclaration(this.EntityPM.Id).subscribe(function (response) {
            var errorsList = null;
            if (response.Result) {
                errorsList = response.Result.RequiredFields;
            }
            if (errorsList) {
                if (errorsList.length == 0) {
                    //this.SendDeclaration();
                    _this.CheckCourierMaster();
                }
                else {
                    _this.ValidationErrors = _this.GetRequiredErrorsList(errorsList);
                    _this.FillValidationErrors(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.O.RequiredFields"));
                }
            }
            else {
                //this.SendDeclaration();
                _this.CheckCourierMaster();
            }
        });
    };
    SendManifestService.prototype.CheckCourierMaster = function () {
        var _this = this;
        this.StartMyBusyIndicator(""); // avoid resend
        this.DeclarationService.GetMAWBCourierMasterByDeclaration(this.EntityPM.Id).subscribe(function (myResponse) {
            var result = myResponse.Result;
            if (Tools_1.AppTool.IsNullOrEmpty(result)) {
                _this.ValidationErrors.push("ההצרה אינה מקושרת לבלדר ראשי");
                _this.FillValidationErrors(_this.presendValidationsTitle);
            }
            else {
                _this.SendManifest();
            }
        });
    };
    //SendDeclaration() {
    SendManifestService.prototype.SendManifest = function () {
        var _this = this;
        var sendParams = new MANIFESTRequestRequestParams_1.MANIFESTRequestRequestParams();
        sendParams.Tenant = SessionLocator_1.SessionLocator.Tenant;
        sendParams.DeclarationId = this.EntityPM.Id;
        sendParams.LoggingEnabled = true;
        sendParams.LoggingEntityId = this.EntityPM.Id;
        sendParams.LoggingEntityReference = this.EntityPM.DeclarationNumber;
        sendParams.LoggingObjectTableId = this.ObjectTable.Id;
        sendParams.LoggingUserId = SessionLocator_1.SessionLocator.LoggedUserId;
        sendParams.RequestName = "Declaration Request";
        sendParams.ResponseName = "Declaration Response";
        sendParams.RequestVIA = this.RequestVIA;
        sendParams.ForcePersonalSign = this.ForcePersonalSign;
        //let myShowProgressBarParams = new ShowProgressBarParams();
        //myShowProgressBarParams.OnCloseCustomMessageProgressComponentMethod =
        //    (response: any) => {
        //        SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
        //            .then(cmpRef => {
        //                cmpRef.instance.ComponentRef = cmpRef;
        //                cmpRef.instance.Run({
        //                    SelectedTabCode: "DCCA",
        //                    EntityId: this.EntityPM.Id,
        //                    ObjectTableName: "Customs.Declaration",
        //                });
        //                cmpRef.instance.OnFirstTimeAfterSingleDataLoaded
        //                    .subscribe(myResult => {
        //                        var myDeclarationEditComponentController = cmpRef.instance.EditComponentController as DeclarationEditComponentController;
        //                        myDeclarationEditComponentController.CustomsAnswersShowManifest = true;
        //                        console.log("myDeclarationEditComponentController.CustomsAnswersShowManifest = true;");
        //                    });
        //            });
        //    };
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
                            _this.OnSuccessSendMethod(myResponseData);
                        }
                    }
                };
        }
        CustomMessageProgressComponent_1.CustomMessageProgressComponent
            .ShowProgressBar(sendParams.PBId, "שליחת מצהר", false, myShowProgressBarParams)
            .then(function (res) {
            _this.responseData = res;
            if (_this.CourierWorksheetmode) {
            }
            else {
                if (_this.responseData && _this.responseData.ContinueProcessInBackground) {
                    _this.CurrentSession.CurrentEditComponent.EditComponentController.IsInBatchRequest = true;
                }
                else if (_this.Option == 'WB' || _this.Option == 'D') { // work around itzik shall fix the undefined problem.
                    _this.CurrentSession.CurrentEditComponent.EditComponentController.IsInBatchRequest = true;
                }
                var myDeclarationEditComponentController = _this.CurrentSession.CurrentEditComponent.EditComponentController;
                myDeclarationEditComponentController.CustomsAnswersShowManifest = true;
                _this.CurrentSession.CurrentEditComponent.PreSelectedTabCode = "DCCA";
                _this.CurrentSession.CurrentEditComponent.SetSelectedTab();
                var myDeclarationEditComponentController = _this.CurrentSession.CurrentEditComponent.EditComponentController;
                myDeclarationEditComponentController.CustomsAnswersShowManifest = true;
                _this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
            }
        }).catch(function (err) {
            _this.StopMyBusyIndicator();
            _this.ValidationErrors.push(err);
            _this.FillValidationErrors("Errors");
        });
        this.DeclarationService.PostSendManifest(sendParams).subscribe(function (response) {
            //this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
            _this.StopMyBusyIndicator();
        });
        //SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
        //    .then(cmpRef => {
        //        cmpRef.instance.ComponentRef = cmpRef;
        //        cmpRef.instance.Run({
        //            SelectedTabCode: "DCCA",
        //            EntityId: this.EntityPM.Id,
        //            ObjectTableName: "Customs.Declaration",
        //        });
        //        cmpRef.instance.OnFirstTimeAfterSingleDataLoaded
        //            .subscribe(myResult => {
        //                var myDeclarationEditComponentController = cmpRef.instance.EditComponentController as DeclarationEditComponentController;
        //                myDeclarationEditComponentController.CustomsAnswersShowManifest = true;
        //                console.log("myDeclarationEditComponentController.CustomsAnswersShowManifest = true;");
        //            });
        //    });
    };
    SendManifestService.prototype.GetRequiredErrorsList = function (errorsList) {
        var errorsMessages = [];
        errorsList.forEach(function (error) {
            if (!Tools_1.AppTool.IsNullOrEmpty(error.CustomMessageError)) {
                errorsMessages.push(error.CustomMessageError);
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
    SendManifestService.prototype.OnAddEditWindowClosed = function (event) {
        this.ValidationErrors = [];
    };
    SendManifestService.prototype.FillValidationErrors = function (title) {
        var _this = this;
        this.StopMyBusyIndicator();
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
    SendManifestService.prototype.StopMyBusyIndicator = function () {
        if (this.CourierWorksheetmode) {
            this.CurrentSession.StopBusyIndicator();
        }
        else {
            this.CurrentSession.CurrentEditComponent.StopBusyIndicator();
        }
    };
    SendManifestService.prototype.StartMyBusyIndicator = function (mess) {
        ///this.StartMyBusyIndicator("");
        if (this.CourierWorksheetmode) {
            this.CurrentSession.StartBusyIndicator(mess);
        }
        else {
            this.CurrentSession.CurrentEditComponent.StartBusyIndicator(mess);
        }
    };
    return SendManifestService;
}());
exports.SendManifestService = SendManifestService;
//# sourceMappingURL=SendManifestComponent.js.map