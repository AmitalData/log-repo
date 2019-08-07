"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var BankDepositExtendedPMService_1 = require("./../../Services/ExtendedPMs/BankDepositExtendedPMService");
var FeatureLocator_1 = require("../../../Infrastructure/Utilities/FeatureLocator");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var TextCodeTranslator_1 = require("../../../Infrastructure/Utilities/TextCodeTranslator");
var Tools_1 = require("../../../Infrastructure/Tools");
var DocumentTypePMExtendedService_1 = require("../../../Common/Services/ExtendedPMs/DocumentTypePMExtendedService");
var ExportDocumentService_1 = require("../../../Common/Services/DocumentServices/ExportDocumentService");
var DocumentOutPMService_1 = require("../../../Common/Services/ExtendedPMs/DocumentOutPMService");
var GeneralPrintHelper_1 = require("../../../Infrastructure/Helpers/GeneralPrintHelper");
var ServiceLocator_1 = require("../../../Infrastructure/Locators/ServiceLocator");
var BankDepositMenuButtonsHandler = /** @class */ (function () {
    function BankDepositMenuButtonsHandler() {
        this.ObjectTableName = "BankDeposit";
        this._documentOutPMService = new DocumentOutPMService_1.DocumentOutPMService();
        this._documentTypePMService = new DocumentTypePMExtendedService_1.DocumentTypePMExtendedService();
        this._exportDocumentService = new ExportDocumentService_1.ExportDocumentService();
        this._BankDepositExtendedPMService = new BankDepositExtendedPMService_1.BankDepositExtendedPMService();
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.LoadCompletedEvent = null;
    }
    BankDepositMenuButtonsHandler.prototype.SetEntityPM = function (entityArgs) {
        this.TenantPM = SessionLocator_1.SessionLocator.TenantPM;
        this.entityArgs = entityArgs;
        this.EntityPM = entityArgs.EntityPM;
        this.Listen();
    };
    BankDepositMenuButtonsHandler.prototype.Listen = function () {
        var _this = this;
        if (this.CurrentSession.CurrentEditComponent != null) {
            this.CurrentSession.CurrentEditComponent.ComponentId;
            if (this.LoadCompletedEvent == null) {
                this.LoadCompletedEvent = this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe(function (isLoadSuccess) {
                    if (isLoadSuccess) {
                        _this.EntityPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                        console.log("Entity Reloaded");
                    }
                });
            }
        }
    };
    BankDepositMenuButtonsHandler.prototype.CheckButtonState = function (menuButtons) {
        if (this.EntityPM != null) {
            if (this.entityArgs.EditComponent != null) {
                var table = window.ObjectTables.filter(function (d) { return d.Name === 'BankDeposit'; })[0];
                var buttonEnabled = true;
                var eventsTabFeature = FeatureLocator_1.FeatureLocator.Features.filter(function (f) { return (f.Code == "UPDATE") && f.ObjectTableId == table.Id; })[0];
                if (!eventsTabFeature) {
                    buttonEnabled = false;
                }
                for (var i = 0; i < menuButtons.length; i++) {
                    var button = menuButtons[i];
                    switch (button.EventCode) {
                        case "BankDepositApprove":
                            {
                                if (this.EntityPM.Id) {
                                    button.IsDisabled = true;
                                }
                                else {
                                    button.IsDisabled = false;
                                }
                                break;
                            }
                        case "BankDepositPrint":
                            {
                                if (!this.EntityPM.Id)
                                    button.IsDisabled = true;
                                else
                                    button.IsDisabled = false;
                                break;
                            }
                        case "CancelDeposit":
                            {
                                if (!this.EntityPM.Id)
                                    button.IsDisabled = true;
                                else if (this.EntityPM.JournalQueueId == null)
                                    button.IsDisabled = true;
                                else
                                    button.IsDisabled = false;
                                if (this.EntityPM.IsCanceled)
                                    button.IsDisabled = true;
                                break;
                            }
                    }
                }
            }
        }
        return menuButtons;
    };
    BankDepositMenuButtonsHandler.prototype.MenuButtonClick = function (menuButton) {
        var _this = this;
        if (!this.EntityPM.CreateDate) {
            // will override in server, its required even on client!!
            this.EntityPM.CreateDate = new Date();
            this.EntityPM.CreatedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
            this.EntityPM.UpdateDate = new Date();
            this.EntityPM.UpdatedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
        }
        switch (menuButton.EventCode) {
            case "BankDepositApprove":
                {
                    this.entityArgs.EditComponent.ValidationErrorsList = [];
                    if (this.EntityPM.IsCashDeposit) {
                        if (this.EntityPM.LocalDepositAmount == 0) {
                            var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.General.O.ZeroDeposit");
                            this.entityArgs.EditComponent.ValidationErrorsList = [];
                            this.entityArgs.EditComponent.ValidationErrorsList.push(msg);
                            return;
                        }
                        else if (this.EntityPM.LocalDepositAmount < 0) {
                            var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.O.minusDepositNotAllowed");
                            this.entityArgs.EditComponent.ValidationErrorsList = [];
                            this.entityArgs.EditComponent.ValidationErrorsList.push(msg);
                            return;
                        }
                    }
                    else {
                        if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.BankDepositLines)) {
                            this.entityArgs.EditComponent.ValidationErrorsList = [];
                            this.entityArgs.EditComponent.ValidationErrorsList.push(TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.General.O.selectAtLeast1Linetodeposit"));
                            return;
                        }
                        else {
                            if (this.EntityPM.BankDepositLines.length == 0) {
                                this.entityArgs.EditComponent.ValidationErrorsList = [];
                                this.entityArgs.EditComponent.ValidationErrorsList.push(TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.General.O.selectAtLeast1Linetodeposit"));
                                return;
                            }
                        }
                    }
                    break;
                }
            case "CancelDeposit":
                {
                    ///// save in server
                    this.CurrentSession.StartBusyIndicatorLoading();
                    this._BankDepositExtendedPMService.cancelDeposit(this.EntityPM.Id).subscribe(function (myResult) {
                        _this.CurrentSession.StopBusyIndicator();
                        var mm = myResult;
                        if (!mm.HasError) {
                            _this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                        }
                        else {
                            _this.CurrentSession.CurrentEditComponent.ValidationErrorsList = mm.ErrorsArray;
                        }
                    });
                    ///// old save pattern: update in client then submitchanges
                    // this.EntityPM.IsCanceled = true;
                    // this.entityArgs.EditComponent.SaveChanges();
                    // this.entityArgs.EditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                    //     if (isSaveSuccess) {
                    //     } else {
                    //         this.EntityPM.IsCanceled = false;
                    //     }
                    // });
                    return;
                }
            case "BankDepositPrint":
                {
                    this.PrintDeposit();
                    return; // no save on print
                }
        }
        if (this.entityArgs.EditComponent.ValidationErrorsList.length == 0) {
            this.entityArgs.EditComponent.SaveChanges();
        }
    };
    BankDepositMenuButtonsHandler.prototype.StartBusyIndicator = function (message) {
        this.CurrentSession.StartBusyIndicator(message);
    };
    BankDepositMenuButtonsHandler.prototype.StopBusyIndicator = function () {
        this.CurrentSession.StopBusyIndicator();
    };
    BankDepositMenuButtonsHandler.prototype.PrintDeposit = function () {
        var myPrintHelper = new GeneralPrintHelper_1.GeneralPrintHelper(this.ObjectTableName, "BDPR", this.EntityPM.Id, null, this.EntityPM.BankAccountNumber, null);
        if (myPrintHelper.IsLoadPrintControl) {
            ServiceLocator_1.ServiceLocator.SendTotangoUserActivity(this.ObjectTableName, "BankDepositPrint");
            myPrintHelper.ShowPrintControl();
        }
    };
    return BankDepositMenuButtonsHandler;
}());
exports.BankDepositMenuButtonsHandler = BankDepositMenuButtonsHandler;
//# sourceMappingURL=BankDepositMenuButtonsHandler.js.map