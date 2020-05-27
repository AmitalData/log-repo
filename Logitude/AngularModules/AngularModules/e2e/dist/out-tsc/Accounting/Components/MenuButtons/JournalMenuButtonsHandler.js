"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var FeatureLocator_1 = require("../../../Infrastructure/Utilities/FeatureLocator");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var MessageWindow_1 = require("../../../Controls/Windows/MessageWindow");
var DocumentOutPMService_1 = require("../../../Common/Services/ExtendedPMs/DocumentOutPMService");
var JournalOpService_1 = require("../../Services/Others/JournalOpService");
var DocumentTypePMExtendedService_1 = require("../../../Common/Services/ExtendedPMs/DocumentTypePMExtendedService");
var ExportDocumentService_1 = require("../../../Common/Services/DocumentServices/ExportDocumentService");
var ServiceLocator_1 = require("../../../Infrastructure/Locators/ServiceLocator");
var GeneralPrintHelper_1 = require("../../../Infrastructure/Helpers/GeneralPrintHelper");
var JournalExtendedPMService_1 = require("../../Services/ExtendedPMs/JournalExtendedPMService");
var JournalMenuButtonsHandler = /** @class */ (function () {
    function JournalMenuButtonsHandler() {
        this.ObjectTableName = "Journal";
        this._documentOutPMService = new DocumentOutPMService_1.DocumentOutPMService();
        this._journalOpService = new JournalOpService_1.JournalOpService();
        this._documentTypePMService = new DocumentTypePMExtendedService_1.DocumentTypePMExtendedService();
        this._exportDocumentService = new ExportDocumentService_1.ExportDocumentService();
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
    }
    JournalMenuButtonsHandler.prototype.SetEntityPM = function (entityArgs) {
        this.TenantPM = SessionLocator_1.SessionLocator.TenantPM;
        this.entityArgs = entityArgs;
        this.EntityPM = entityArgs.EntityPM;
    };
    JournalMenuButtonsHandler.prototype.CheckButtonState = function (menuButtons) {
        if (this.EntityPM != null) {
            if (this.entityArgs.EditComponent != null) {
                var table = window.ObjectTables.filter(function (d) { return d.Name === 'Journal'; })[0];
                var buttonEnabled = true;
                var eventsTabFeature = FeatureLocator_1.FeatureLocator.Features.filter(function (f) { return (f.Code == "UPDATE") && f.ObjectTableId == table.Id; })[0];
                if (!eventsTabFeature) {
                    buttonEnabled = false;
                }
                for (var i = 0; i < menuButtons.length; i++) {
                    var button = menuButtons[i];
                    // Status codes:
                    //  0- Draft
                    //  1- Waiting for Approve
                    //  2- Approved
                    //  3- Voided
                    switch (button.EventCode) {
                        case "JournalSave": // save and close
                            {
                                if (this.EntityPM.StatusCode == "2" || this.EntityPM.StatusCode == "3") {
                                    button.IsDisabled = true;
                                }
                                else {
                                    button.IsDisabled = false;
                                }
                                break;
                            }
                        case "JournalApprove":
                            {
                                if (this.EntityPM.StatusCode == "3" || this.EntityPM.StatusCode == "2") {
                                    button.IsDisabled = true;
                                }
                                else {
                                    button.IsDisabled = false;
                                }
                                break;
                            }
                        case "JournalSaveAsDraft":
                            {
                                if (this.EntityPM.StatusCode == "2" || this.EntityPM.StatusCode == "3") {
                                    button.IsDisabled = true;
                                }
                                else {
                                    button.IsDisabled = false;
                                }
                                break;
                            }
                        case "JournalVoid":
                            {
                                // the VOID button is only available on this case:          BUG #44819
                                //    - Approved Journal, not storno
                                if (this.EntityPM.StatusCode == "2" // 2- Approved
                                    && this.EntityPM.AccountingEntityCode == "1" // 1- Journal
                                    && this.EntityPM.OriginalJournalId == null) // Not Storno
                                 {
                                    button.IsDisabled = false;
                                }
                                else {
                                    button.IsDisabled = true;
                                }
                                // if (this.EntityPM.StatusCode == "3" || this.EntityPM.AccountingEntityCode != "1") { // 3- Voided | 1- Journal
                                //     button.IsDisabled = true;
                                // }
                                // else if( this.EntityPM.AccountingEntityCode == "1" && this.EntityPM.StatusCode == "2" && (this.EntityPM.OriginalJournalId != null)) // STORNO  1-Journal
                                // {
                                //     button.IsDisabled = true;
                                // }
                                // else if (this.EntityPM.StatusCode == "2" && this.EntityPM.AccountingEntityCode == "1" && this.EntityPM.OriginalJournalId == null) { // 2- Approved
                                //     button.IsDisabled = false;
                                // }
                                break;
                            }
                        case "JournalPrint":
                            {
                                button.IsDisabled = false;
                                //    if (this.EntityPM.StatusCode == "2" && this.EntityPM.OriginalJournalId == null) {
                                //    button.IsDisabled = false;
                                //}
                                //else {
                                //    button.IsDisabled = true;
                                //}
                                break;
                            }
                    }
                }
            }
        }
        return menuButtons;
    };
    JournalMenuButtonsHandler.prototype.MenuButtonClick = function (menuButton) {
        //this.copyAccountingDates();
        var _this = this;
        switch (menuButton.EventCode) {
            case "JournalSave": // save and close
                {
                    this.EntityPM.StatusCode = "1"; // Waiting
                    this.SaveChenges();
                    break;
                }
            case "JournalApprove":
                {
                    this.EntityPM.StatusCode = "2"; // Approved
                    this.EntityPM.UIProperties.SetEnabled("AccountingDate", "Journal", false);
                    this.EntityPM.UIProperties.SetEnabled("Reference1", "Journal", false);
                    this.EntityPM.UIProperties.SetEnabled("Reference2", "Journal", false);
                    this.EntityPM.UIProperties.SetEnabled("Reference3", "Journal", false);
                    this.EntityPM.UIProperties.SetEnabled("Notes", "Journal", false);
                    this.SaveChenges();
                    break;
                }
            case "JournalSaveAsDraft":
                {
                    this.EntityPM.StatusCode = "0"; // Draft
                    this.EntityPM.IsDirty = true;
                    this.SaveChenges();
                    break;
                }
            case "JournalVoid":
                {
                    this.entityArgs.EditComponent.StartBusyIndicatorSaving();
                    var myJournalExtendedPMService = new JournalExtendedPMService_1.JournalExtendedPMService();
                    myJournalExtendedPMService
                        .VoidJournal(this.EntityPM.Tenant, this.EntityPM.Id, "", "", "")
                        .subscribe(function (res) {
                        _this.entityArgs.EditComponent.StopBusyIndicator();
                        if (res.HasError) {
                            _this.entityArgs.EditComponent.ValidationErrorsList = res.ErrorsArray;
                        }
                        else {
                            _this.entityArgs.EditComponent.ReloadEntityPM();
                        }
                    });
                    //this.EntityPM.StatusCode = "3"; // Voided
                    //this.SaveChenges();
                    break;
                }
            case "JournalPrint":
                {
                    this.PrintJournal();
                    //if (this.EntityPM.StatusCode != "2") { // Approved
                    //    this.PrintJournal();
                    //} else {
                    //    this.entityArgs.EditComponent.SaveChanges();
                    //    this.entityArgs.EditComponent.SaveCompleted.subscribe(($event) => {
                    //        if ($event == true) {
                    //            this.entityArgs.EditComponent.ReloadEntityPM();
                    //            this.SetEntityPM(this.entityArgs);
                    //            this.PrintJournal();
                    //        }
                    //    });
                    //}
                    break;
                }
        }
    };
    JournalMenuButtonsHandler.prototype.SaveChenges = function () {
        var _this = this;
        // the validation will be in PM Service (custom validator)
        this.entityArgs.EditComponent.SaveChanges();
        this.entityArgs.EditComponent.SaveCompleted.subscribe(function ($event) {
            if ($event == true) {
                _this.entityArgs.EditComponent.ReloadEntityPM();
            }
        });
    };
    JournalMenuButtonsHandler.prototype.copyAccountingDates = function () {
        // Copy AccountingDate from journal to journal lines:
        for (var _i = 0, _a = this.EntityPM.JournalLines; _i < _a.length; _i++) {
            var line = _a[_i];
            if (line.AccountingDate != this.EntityPM.AccountingDate) {
                line.AccountingDate = this.EntityPM.AccountingDate;
            }
            //line.ActionTypeCode = line.ActionCode;
        }
    };
    JournalMenuButtonsHandler.prototype.StartBusyIndicator = function (message) {
        this.CurrentSession.StartBusyIndicator(message);
    };
    JournalMenuButtonsHandler.prototype.StopBusyIndicator = function () {
        this.CurrentSession.StopBusyIndicator();
    };
    JournalMenuButtonsHandler.prototype.PrintJournal = function () {
        this.BuildDocument();
    };
    JournalMenuButtonsHandler.prototype.BuildDocument = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicator("Building document....");
        var objectTable = window.ObjectTables.filter(function (d) { return d.Name === "Journal"; })[0];
        var objectTableId = objectTable.Id;
        //1
        //Get document type
        this._documentTypePMService.GetDocumentTypeByCode("JRPR", SessionLocator_1.SessionLocator.Tenant).subscribe(function (response) {
            var documentType = response.Result;
            console.log("_documentTypePMService.GetDocumentTypeByCode", response);
            if (documentType) {
                //2
                //Get document copy
                var documentTypeCopy = documentType.DocumentTypeCopies[0];
                //3
                //Get document out
                _this._documentOutPMService.getCreateDocumentOut(documentType.Id, _this.EntityPM.Id, null, null, objectTableId, SessionLocator_1.SessionLocator.Tenant).subscribe(function (res) {
                    var pmResponse = res;
                    if (!pmResponse.HasError) {
                        var documentout = pmResponse.Result;
                        console.log("_documentOutPMService.getCreateDocumentOut", response);
                        if (documentout) {
                            var documentOutCopy = documentout.DocumentOutCopies[0];
                            _this.documentOutPM = documentout;
                            //if (documentOutCopy) {
                            //4
                            //Export to pdf
                            _this._exportDocumentService.getDocumentPdfFile(documentType.Id, _this.EntityPM.Id, objectTableId, null, null, documentout.Id, documentout.Tenant, documentTypeCopy.Id, SessionLocator_1.SessionLocator.LoggedUserId).subscribe(function (res) {
                                var pmResponse = res;
                                if (!pmResponse.HasError) {
                                    console.log("_exportDocumentService.getDocumentPdfFile", pmResponse);
                                    var myResult = pmResponse.Result;
                                    if (myResult != null) {
                                        if (documentOutCopy) {
                                            //5
                                            //view page
                                            var documentName = documentOutCopy.Id;
                                            _this.ViewPage(documentName, documentOutCopy.DocoumentTypeCopyName, documentout);
                                        }
                                        else {
                                            _this.BuildDocument(); // resend the request, the method [getCreateDocumentOut] does not create document out copy!!
                                            console.warn("Cannot find document out copy, resend request...");
                                        }
                                    }
                                    else
                                        _this.StopBusyIndicator();
                                }
                                else {
                                    if (pmResponse.ErrorsArray && pmResponse.ErrorsArray.length > 0) {
                                        console.log(pmResponse.ErrorsArray[0]);
                                    }
                                    _this.StopBusyIndicator();
                                }
                            });
                            //} else {
                            //    console.warn("Cannot find document out copy, resend request...");
                            //    //this.CurrentSession.StopBusyIndicator();
                            //    this.BuildDocument(); // resend the request, the method [getCreateDocumentOut] does not create document out copy!!
                            //}
                        }
                        else {
                            console.error("Cannot create document out!", res);
                            _this.CurrentSession.StopBusyIndicator();
                        }
                    }
                });
            }
            else {
                var msg = new MessageWindow_1.MessageWindow();
                _this.CurrentSession.StopBusyIndicator();
                msg.Show("No document type found!");
            }
        });
    };
    JournalMenuButtonsHandler.prototype.ViewPage = function (documentName, docoumentTypeCopyName, documentOut) {
        //ServiceLocator.SendTotangoUserActivity(this.ObjectTableName, docoumentTypeCopyName + " Viewing");
        //DownloadManager.DownloadPage(documentName , documentOut.SecurityId);
        //this.StopBusyIndicator();
        var myPrintHelper = new GeneralPrintHelper_1.GeneralPrintHelper(this.ObjectTableName, "JRPR", this.EntityPM.Id, null, this.EntityPM.AccountingEntityReference, null);
        if (myPrintHelper.IsLoadPrintControl) {
            ServiceLocator_1.ServiceLocator.SendTotangoUserActivity(this.ObjectTableName, "Journal");
            myPrintHelper.ShowPrintControl();
        }
    };
    return JournalMenuButtonsHandler;
}());
exports.JournalMenuButtonsHandler = JournalMenuButtonsHandler;
//# sourceMappingURL=JournalMenuButtonsHandler.js.map