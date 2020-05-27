"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var TextCodeTranslator_1 = require("../../../Infrastructure/Utilities/TextCodeTranslator");
var LogitudeWindow_1 = require("../../../Controls/Windows/LogitudeWindow");
var ConfirmWindow_1 = require("../../../Controls/Windows/ConfirmWindow");
var TaxReportExtendedPMService_1 = require("../../Services/ExtendedPMs/TaxReportExtendedPMService");
var TaxReportMenuButtonsHandler = /** @class */ (function () {
    function TaxReportMenuButtonsHandler() {
        this.ObjectTableName = "TaxReport";
        this._TaxReportExtendedPMService = new TaxReportExtendedPMService_1.TaxReportExtendedPMService();
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
    }
    TaxReportMenuButtonsHandler.prototype.SetEntityPM = function (entityArgs) {
        this.TenantPM = SessionLocator_1.SessionLocator.TenantPM;
        this.entityArgs = entityArgs;
        this.EntityPM = entityArgs.EntityPM;
    };
    TaxReportMenuButtonsHandler.prototype.CheckButtonState = function (menuButtons) {
        if (this.EntityPM != null) {
            if (this.entityArgs.EditComponent != null) {
                var table = window.ObjectTables.filter(function (d) { return d.Name === 'TaxReport'; })[0];
                for (var i = 0; i < menuButtons.length; i++) {
                    var button = menuButtons[i];
                    switch (button.EventCode) {
                        case "TRCN":
                            {
                                if (this.EntityPM.IsCancelled) {
                                    button.IsDisabled = true;
                                }
                                else {
                                    button.IsDisabled = false;
                                }
                                break;
                            }
                    }
                }
            }
        }
        return menuButtons;
    };
    TaxReportMenuButtonsHandler.prototype.MenuButtonClick = function (menuButton) {
        var _this = this;
        switch (menuButton.EventCode) {
            case "TRCN": // Cancel
                {
                    var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
                    var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.General.O.WantToCancelTaxReport");
                    confirmWindow.Show(msg);
                    confirmWindow.WindowClosed.subscribe(function (event) {
                        if (confirmWindow.Yes) {
                            _this.EntityPM.IsCancelled = true;
                            _this.entityArgs.EditComponent.SaveChanges();
                            _this.entityArgs.EditComponent.SaveCompleted.subscribe(function (isSaveSuccess) {
                                if (isSaveSuccess) {
                                    _this.entityArgs.EditComponent.ReloadEntityPM();
                                }
                                else {
                                    _this.EntityPM.IsCancelled = false;
                                }
                            });
                        }
                    });
                    break;
                }
            case "TRDL": // Download
                {
                    var windowTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("TaxReport.B.Download");
                    var windowArgs = {};
                    windowArgs.ObjectTableName = "TaxReport";
                    windowArgs.EntityPM = this.EntityPM;
                    windowArgs.StartDirectly = false; // start service after show window
                    windowArgs.TimerInterval = 1000; // wait time between requests
                    var logWindow = new LogitudeWindow_1.LogitudeWindow();
                    logWindow.Width = 350;
                    logWindow.Height = 150;
                    logWindow.Title = windowTitle;
                    logWindow.ShowCloseButton = true;
                    logWindow.WindowArgs = windowArgs;
                    logWindow.WindowClosed.subscribe(function ($event) {
                        _this.entityArgs.EditComponent.ReloadEntityPM();
                    });
                    logWindow.Show('./Accounting/Components/Others/AccountingFlatFileDownloadComponent');
                    //this.CurrentSession.StartBusyIndicatorLoading();
                    //this._TaxReportExtendedPMService.DownloadPNC874File(this.EntityPM).subscribe(myResult => {
                    //  var mm: ServiceResponse = myResult;
                    //  var entity = mm.Result;
                    //    var docFilingPM = entity;
                    //    DownloadManager.DownloadPage(docFilingPM.DocumentId);
                    //  this.CurrentSession.StopBusyIndicator();
                    //});
                    break;
                }
        }
    };
    TaxReportMenuButtonsHandler.prototype.StartBusyIndicator = function (message) {
        this.CurrentSession.StartBusyIndicator(message);
    };
    TaxReportMenuButtonsHandler.prototype.StopBusyIndicator = function () {
        this.CurrentSession.StopBusyIndicator();
    };
    return TaxReportMenuButtonsHandler;
}());
exports.TaxReportMenuButtonsHandler = TaxReportMenuButtonsHandler;
//# sourceMappingURL=TaxReportMenuButtonsHandler.js.map