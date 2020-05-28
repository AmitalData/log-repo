"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var DocumentsFilingViewsExtService_1 = require("../../../Common/Services/ExtendedLists/DocumentsFilingViewsExtService");
var DownloadManager_1 = require("../../../Infrastructure/Utilities/DownloadManager");
var DocumentTypePMExtendedService_1 = require("../../../Common/Services/ExtendedPMs/DocumentTypePMExtendedService");
var DocumentsFilingExtendedPMService_1 = require("../../../Common/Services/ExtendedPMs/DocumentsFilingExtendedPMService");
var ServiceLocator_1 = require("../../../Infrastructure/Locators/ServiceLocator");
var GeneralPrintHelper_1 = require("../../../Infrastructure/Helpers/GeneralPrintHelper");
var OpenFormatReportMenuButtonsHandler = /** @class */ (function () {
    function OpenFormatReportMenuButtonsHandler() {
        this.ObjectTableName = "OpenFormatReport";
        this._DocumentsFilingViewsExtService = new DocumentsFilingViewsExtService_1.DocumentsFilingViewsExtService();
        this.DocumentTypePMExtendedService = new DocumentTypePMExtendedService_1.DocumentTypePMExtendedService();
        this.DocumentsFilingExtendedPMService = new DocumentsFilingExtendedPMService_1.DocumentsFilingExtendedPMService();
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.try = true;
    }
    OpenFormatReportMenuButtonsHandler.prototype.SetEntityPM = function (entityArgs) {
        this.TenantPM = SessionLocator_1.SessionLocator.TenantPM;
        this.entityArgs = entityArgs;
        this.EntityPM = entityArgs.EntityPM;
    };
    OpenFormatReportMenuButtonsHandler.prototype.CheckButtonState = function (menuButtons) {
        if (this.EntityPM != null) {
            if (this.entityArgs.EditComponent != null) {
                var table = window.ObjectTables.filter(function (d) { return d.Name === 'OpenFormatReport'; })[0];
                for (var i = 0; i < menuButtons.length; i++) {
                    var button = menuButtons[i];
                    switch (button.EventCode) {
                        case "OFMR":
                            {
                                button.IsDisabled = false;
                                break;
                            }
                        case "OPDL":
                            {
                                if (this.EntityPM.StatusTypeCode != "3") {
                                    button.IsDisabled = true;
                                }
                                else {
                                    button.IsDisabled = false;
                                }
                                break;
                            }
                        //case "INIDL":
                        //    {
                        //        if (this.EntityPM.StatusTypeCode != "3") {
                        //            button.IsDisabled = true;
                        //        }
                        //        else {
                        //            button.IsDisabled = false;
                        //        }
                        //        break;
                        //    }
                        case "PDFD":
                            {
                                if (this.EntityPM.StatusTypeCode != "3") {
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
    OpenFormatReportMenuButtonsHandler.prototype.MenuButtonClick = function (menuButton) {
        var _this = this;
        switch (menuButton.EventCode) {
            case "OPDL": // Download
                {
                    this.try = true;
                    this.DocumentTypePMExtendedService.GetDocumentTypeByCode("BKMV", this.TenantPM.Id).subscribe(function (myResult) {
                        console.log("[GetLastDocumentsFilingPM]", myResult);
                        var mm = myResult;
                        if (!mm.HasError) {
                            _this.BMKDocumentType = mm.Result;
                            if (_this.BMKDocumentType) {
                                _this.GetDocumentType("INI");
                            }
                        }
                    });
                    break;
                }
            //case "INIDL": // Download
            //    {
            //        this.DocumentTypePMExtendedService.GetDocumentTypeByCode("INI", this.TenantPM.Id).subscribe(myResult => {
            //            console.log("[GetLastDocumentsFilingPM]", myResult);
            //            var mm: ServiceResponse = myResult;
            //            if (!mm.HasError) {
            //                this.documentType = mm.Result;
            //                if (this.documentType) {
            //                    this.GetDocument();
            //                }
            //            }
            //        });
            //        break;
            //    }
            case "PDFD":
                {
                    var myPrintHelper = new GeneralPrintHelper_1.GeneralPrintHelper("OpenFormatReport", "OFDP", this.EntityPM.Id, null, this.EntityPM.ReportNumber, null);
                    if (myPrintHelper.IsLoadPrintControl) {
                        ServiceLocator_1.ServiceLocator.SendTotangoUserActivity("OpenFormatReport", "Print");
                        myPrintHelper.ShowPrintControl();
                    }
                    break;
                }
        }
    };
    OpenFormatReportMenuButtonsHandler.prototype.GetDocument = function () {
        var _this = this;
        var objectTable = window.ObjectTables.filter(function (d) { return d.Name === _this.ObjectTableName; })[0];
        this.DocumentsFilingExtendedPMService.GetDocumentsFilingByDocumentType(this.BMKDocumentType.Id, objectTable.Id, this.EntityPM.Id, this.TenantPM.Id).subscribe(function (myResult) {
            console.log("[GetLastDocumentsFilingPM]", myResult);
            var mm = myResult;
            if (!mm.HasError) {
                _this.BMKDocFilingPM = mm.Result;
                _this.DocumentsFilingExtendedPMService.GetDocumentsFilingByDocumentType(_this.INIDocumentType.Id, objectTable.Id, _this.EntityPM.Id, _this.TenantPM.Id).subscribe(function (myResult) {
                    console.log("[GetLastDocumentsFilingPM]", myResult);
                    var mm = myResult;
                    if (!mm.HasError) {
                        _this.INIDocFilingPM = mm.Result;
                        _this.GetDocumentType("INI");
                        var securityIds = _this.BMKDocFilingPM.SecurityId + "," + _this.INIDocFilingPM.SecurityId;
                        if (_this.try) {
                            DownloadManager_1.DownloadManager.DownloadPage(null, securityIds);
                            _this.try = false;
                        }
                    }
                });
            }
        });
    };
    OpenFormatReportMenuButtonsHandler.prototype.GetDocumentType = function (code) {
        var _this = this;
        this.DocumentTypePMExtendedService.GetDocumentTypeByCode(code, this.TenantPM.Id).subscribe(function (myResult) {
            console.log("[GetLastDocumentsFilingPM]", myResult);
            var mm = myResult;
            if (!mm.HasError) {
                if (code == "INI") {
                    _this.INIDocumentType = mm.Result;
                    if (_this.INIDocumentType) {
                        _this.GetDocument();
                    }
                }
            }
        });
    };
    OpenFormatReportMenuButtonsHandler.prototype.StartBusyIndicator = function (message) {
        this.CurrentSession.StartBusyIndicator(message);
    };
    OpenFormatReportMenuButtonsHandler.prototype.StopBusyIndicator = function () {
        this.CurrentSession.StopBusyIndicator();
    };
    return OpenFormatReportMenuButtonsHandler;
}());
exports.OpenFormatReportMenuButtonsHandler = OpenFormatReportMenuButtonsHandler;
//# sourceMappingURL=OpenFormatReportMenuButtonsHandler.js.map