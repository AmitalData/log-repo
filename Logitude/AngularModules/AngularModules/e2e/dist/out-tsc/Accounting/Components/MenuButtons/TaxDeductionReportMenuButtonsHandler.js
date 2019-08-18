"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
//import { TaxDeductionReportExtendedPMService } from '../../Services/ExtendedPMs/TaxReportExtendedPMService';
var DownloadManager_1 = require("../../../Infrastructure/Utilities/DownloadManager");
var GeneralPrintHelper_1 = require("../../../Infrastructure/Helpers/GeneralPrintHelper");
var ServiceLocator_1 = require("../../../Infrastructure/Locators/ServiceLocator");
var EntityResourceService_1 = require("../../../Infrastructure/Services/EntityResourceService");
var DocumentsFilingExtendedPMService_1 = require("../../../Common/Services/ExtendedPMs/DocumentsFilingExtendedPMService");
var DocumentsFilingViewsExtService_1 = require("../../../Common/Services/ExtendedLists/DocumentsFilingViewsExtService");
var DocumentTypeListExtendedService_1 = require("../../../Common/Services/ExtendedLists/DocumentTypeListExtendedService");
var TaxDeductionReportMenuButtonsHandler = /** @class */ (function () {
    function TaxDeductionReportMenuButtonsHandler() {
        this.ObjectTableName = "TaxDeductionReport";
        this.EntityResourceService = new EntityResourceService_1.EntityResourceService();
        // _TaxReportExtendedPMService: TaxReportExtendedPMService = new TaxReportExtendedPMService();
        this._DocumentsFilingViewsExtService = new DocumentsFilingViewsExtService_1.DocumentsFilingViewsExtService();
        this.documentsFilingExtendedPMService = new DocumentsFilingExtendedPMService_1.DocumentsFilingExtendedPMService();
        this.documentTypeListExtendedService = new DocumentTypeListExtendedService_1.DocumentTypeListExtendedService();
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
    }
    TaxDeductionReportMenuButtonsHandler.prototype.SetEntityPM = function (entityArgs) {
        var _this = this;
        this.TenantPM = SessionLocator_1.SessionLocator.TenantPM;
        this.entityArgs = entityArgs;
        this.EntityPM = entityArgs.EntityPM;
        this.objectTable = window.ObjectTables.filter(function (d) { return d.Name === _this.ObjectTableName; })[0];
    };
    TaxDeductionReportMenuButtonsHandler.prototype.CheckButtonState = function (menuButtons) {
        if (this.EntityPM != null) {
            if (this.entityArgs.EditComponent != null) {
                var table = window.ObjectTables.filter(function (d) { return d.Name === 'TaxDeductionReport'; })[0];
                for (var i = 0; i < menuButtons.length; i++) {
                    var button = menuButtons[i];
                    switch (button.EventCode) {
                        case "TDMR":
                            {
                                button.IsDisabled = false;
                                break;
                            }
                        case "DNPD":
                        case "TXFL":
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
    TaxDeductionReportMenuButtonsHandler.prototype.MenuButtonClick = function (menuButton) {
        var _this = this;
        switch (menuButton.EventCode) {
            case "DNPD":
                {
                    var myPrintHelper = new GeneralPrintHelper_1.GeneralPrintHelper("TaxDeductionReport", "TDDP", this.EntityPM.Id, null, this.EntityPM.Email, null);
                    if (myPrintHelper.IsLoadPrintControl) {
                        ServiceLocator_1.ServiceLocator.SendTotangoUserActivity("TaxDeductionReport", "Print");
                        myPrintHelper.ShowPrintControl();
                    }
                    //this.documentTypeListExtendedService.getDocumentTypeListByCode("TDDP", this.EntityPM.Tenant).subscribe(myResult => {
                    //    var mm: ServiceResponse = myResult;
                    //    if (!mm.HasError) {
                    //        this.documentType = mm.Result;
                    //        if (this.documentType) {
                    //            this.documentsFilingExtendedPMService.GetDocumentsFilingByDocumentType(this.documentType.Id, this.objectTable.Id, this.EntityPM.Id, this.EntityPM.Tenant).subscribe(myResult => {
                    //                var mm: ServiceResponse = myResult;
                    //                if (!mm.HasError) {
                    //                    this.docFilingPM = mm.Result;
                    //                    if (this.docFilingPM) {
                    //                        DownloadManager.DownloadPage(null, this.docFilingPM.SecurityId);
                    //                    }
                    //                }
                    //            });
                    //        }
                    //    }
                    //});
                    //this._DocumentsFilingViewsExtService.get(this.EntityPM.Id, this.objectTable.Id).subscribe(myResult => {
                    //    console.log("[GetLastDocumentsFilingPM]", myResult);
                    //    var mm: ServiceResponse = myResult;
                    //    if (!mm.HasError) {
                    //        this.docFilingPM = mm.Result;
                    //        if (this.docFilingPM) {
                    //            DownloadManager.DownloadPage(null, this.docFilingPM.SecurityId);
                    //        }
                    //    }
                    //});
                    break;
                }
            case "TXFL":
                {
                    this._DocumentsFilingViewsExtService.GetLastDocumentsFilingPM(this.EntityPM.Id, this.objectTable.Id).subscribe(function (myResult) {
                        console.log("[GetLastDocumentsFilingPM]", myResult);
                        var mm = myResult;
                        if (!mm.HasError) {
                            _this.docFilingPM = mm.Result;
                            if (_this.docFilingPM) {
                                DownloadManager_1.DownloadManager.DownloadPage(null, _this.docFilingPM.SecurityId);
                            }
                        }
                    });
                    break;
                }
        }
    };
    TaxDeductionReportMenuButtonsHandler.prototype.StartBusyIndicator = function (message) {
        this.CurrentSession.StartBusyIndicator(message);
    };
    TaxDeductionReportMenuButtonsHandler.prototype.StopBusyIndicator = function () {
        this.CurrentSession.StopBusyIndicator();
    };
    return TaxDeductionReportMenuButtonsHandler;
}());
exports.TaxDeductionReportMenuButtonsHandler = TaxDeductionReportMenuButtonsHandler;
//# sourceMappingURL=TaxDeductionReportMenuButtonsHandler.js.map