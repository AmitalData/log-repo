"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var MessageWindow_1 = require("../../../Controls/Windows/MessageWindow");
var ConfirmWindow_1 = require("../../../Controls/Windows/ConfirmWindow");
var Tools_1 = require("../../../Infrastructure/Tools");
var FeatureLocator_1 = require("../../../Infrastructure/Utilities/FeatureLocator");
var TextCodeTranslator_1 = require("../../../Infrastructure/Utilities/TextCodeTranslator");
var ApiQueryFilters_1 = require("../../../Infrastructure/DataContracts/ApiQueryFilters");
var DocumentsFilingExtendedPMService_1 = require("../../../Common/Services/ExtendedPMs/DocumentsFilingExtendedPMService");
var DocumentTypeListService_1 = require("../../../Common/Services/StandardLists/DocumentTypeListService");
var DownloadManager_1 = require("../../../Infrastructure/Utilities/DownloadManager");
var PaymentOrderMenuButtonsHandler = /** @class */ (function () {
    function PaymentOrderMenuButtonsHandler() {
        this.isValid = false;
        this.isButtonClicked = false;
        this.MenuButtonCode = null;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
    }
    PaymentOrderMenuButtonsHandler.prototype.SetEntityPM = function (entityArgs) {
        this.EntityPM = entityArgs.EntityPM;
        this.Listen();
    };
    PaymentOrderMenuButtonsHandler.prototype.Listen = function () {
        var _this = this;
        if (this.CurrentSession.CurrentEditComponent != null) {
            this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe(function (isSaveSuccess) {
                if (isSaveSuccess) {
                    _this.EntityPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                }
            });
            this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe(function (isLoadSuccess) {
                if (isLoadSuccess) {
                    _this.EntityPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                }
            });
        }
    };
    PaymentOrderMenuButtonsHandler.prototype.CheckButtonState = function (menuButtons) {
        if (this.EntityPM != null) {
            if (this.CurrentSession.CurrentEditComponent != null) {
                var table = window.ObjectTables.filter(function (d) { return d.Name === 'Shipment'; })[0];
                var buttonEnabled = true;
                var eventsTabFeature = FeatureLocator_1.FeatureLocator.Features.filter(function (f) { return (f.Code == "UPDATE") && f.ObjectTableId == table.Id; })[0];
                if (!eventsTabFeature) {
                    buttonEnabled = false;
                }
                for (var i = 0; i < menuButtons.length; i++) {
                    var button = menuButtons[i];
                    if (button.EventCode == "More") {
                        button.IsDisabled = false;
                        button.IsHidden = false;
                    }
                    if (button.EventCode == "PrintPaymentOrder") {
                        if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.DocumentPaymentId)) {
                            button.IsDisabled = true;
                        }
                        else {
                            button.IsDisabled = false;
                        }
                    }
                    if (button.EventCode == "SendPaymentOrder") {
                        if (this.EntityPM.IsClosed) {
                            button.IsDisabled = true;
                        }
                        else {
                            button.IsDisabled = false;
                        }
                        button.IsHidden = false;
                    }
                    if (button.EventCode == "ClosePaymentOrder") {
                        if (this.EntityPM.IsClosed) {
                            button.IsDisabled = true;
                        }
                        else {
                            button.IsDisabled = false;
                        }
                        button.IsHidden = false;
                    }
                    if (button.EventCode == "UnClosePaymentOrder") {
                        if (!this.EntityPM.IsClosed || (this.EntityPM.IsClosed && this.EntityPM.ActualPayDate != null)) {
                            button.IsDisabled = true;
                        }
                        else {
                            button.IsDisabled = false;
                        }
                        button.IsHidden = false;
                    }
                }
                return menuButtons;
            }
        }
    };
    PaymentOrderMenuButtonsHandler.prototype.MenuButtonClick = function (menuButton) {
        if (true) { //if (!this.isButtonClicked) {
            this.isButtonClicked = true;
            this.MenuButtonCode = menuButton.EventCode;
            if (true) { //if (this.isValid) {
                switch (this.MenuButtonCode) {
                    case "SendPaymentOrder":
                        {
                            //SendPaymentOrder(paymentOrderViewModel);
                            break;
                        }
                    case "ClosePaymentOrder":
                        {
                            this.ClosePaymentOrderMethod();
                            break;
                        }
                    case "UnClosePaymentOrder":
                        {
                            this.UnClosePaymentOrderMethod();
                            break;
                        }
                    case "PrintPaymentOrder":
                        {
                            this.PrintDeclarationFormMethod();
                            break;
                        }
                    case "PrintDeficit":
                        {
                            this.PrintDeficitFormMethod();
                            break;
                        }
                }
            }
        }
    };
    PaymentOrderMenuButtonsHandler.prototype.ClosePaymentOrderMethod = function () {
        var _this = this;
        var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
        confirmWindow.Width = 400;
        confirmWindow.Show(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.PaymentOrder.O.ClosePaymentOrder"));
        confirmWindow.WindowClosed.subscribe(function (event) {
            if (confirmWindow.Yes) {
                _this.EntityPM.IsClosed = true;
                _this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe(function (isSave) {
                    _this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                    var window = new MessageWindow_1.MessageWindow();
                });
                _this.CurrentSession.CurrentEditComponent.SaveChanges();
            }
        });
    };
    PaymentOrderMenuButtonsHandler.prototype.UnClosePaymentOrderMethod = function () {
        var _this = this;
        var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
        confirmWindow.Width = 400;
        confirmWindow.Show(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.PaymentOrder.O.ReOpenPaymentOrder"));
        confirmWindow.WindowClosed.subscribe(function (event) {
            if (confirmWindow.Yes) {
                _this.EntityPM.IsClosed = false;
                _this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe(function (isSave) {
                    _this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                    var window = new MessageWindow_1.MessageWindow();
                });
                _this.CurrentSession.CurrentEditComponent.SaveChanges();
            }
        });
    };
    PaymentOrderMenuButtonsHandler.prototype.PrintDeclarationFormMethod = function () {
        if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.DocumentPaymentId)) {
            var msg = new MessageWindow_1.MessageWindow();
            msg.Width = 350;
            msg.Show("There are no Declaration form  Document");
            return;
        }
        var documentName = this.EntityPM.Tenant + "_" + this.EntityPM.DocumentPaymentId;
        this.ShowDocument(documentName);
    };
    PaymentOrderMenuButtonsHandler.prototype.PrintDeficitFormMethod = function () {
        var _this = this;
        var documentTypeId = "";
        var documentTypeListService = new DocumentTypeListService_1.DocumentTypeListService();
        var table = window.ObjectTables.filter(function (d) { return d.Name == 'Customs.Declaration'; })[0];
        var filters = new ApiQueryFilters_1.ApiQueryFilters();
        //filters.addAdditionalFilter("ObjectTableId", table.Id, null, null, "Equals", false, false, false, "string");
        //filters.addAdditionalFilter("Code", "DEF", null, null, "Equals", false, false, false, "string");
        filters.GetAll = true;
        filters.Tenant = SessionLocator_1.SessionLocator.Tenant;
        documentTypeListService.getAllFromCache(filters).subscribe(function (response) {
            if (!response.HasError) {
                var myResult = response.Result;
                if (Tools_1.AppTool.IsNullOrEmpty(myResult)) {
                    var msg = new MessageWindow_1.MessageWindow();
                    msg.Width = 350;
                    msg.Show("Declaration Document is missing");
                    return;
                }
                var item = myResult.filter(function (d) { return d.Code == "DEF"; })[0];
                if (!Tools_1.AppTool.IsNullOrEmpty(item)) {
                    documentTypeId = item.Id;
                }
                _this.GetDocumentByPaymentNumber(documentTypeId);
            }
        });
    };
    PaymentOrderMenuButtonsHandler.prototype.GetDocumentByPaymentNumber = function (documentTypeId) {
        var _this = this;
        if (Tools_1.AppTool.IsNullOrEmpty(documentTypeId)) {
            var msg = new MessageWindow_1.MessageWindow();
            msg.Width = 350;
            msg.Show("The value 'DEF' does not exist in Document Type");
            return;
        }
        var documentFiling;
        var documentsFilingExtendedPMService = new DocumentsFilingExtendedPMService_1.DocumentsFilingExtendedPMService();
        documentsFilingExtendedPMService.GetSingleDocumentsFilingByChild(documentTypeId, this.EntityPM.PaymentNumber, this.EntityPM.Tenant).subscribe(function (res) {
            var pmResponse = res;
            if (!pmResponse.HasError && !Tools_1.AppTool.IsNullOrEmpty(pmResponse.Result)) {
                var myResult = pmResponse.Result;
                if (myResult) {
                    documentFiling = myResult;
                    if (Tools_1.AppTool.IsNullOrEmpty(documentFiling)) {
                        var msg = new MessageWindow_1.MessageWindow();
                        msg.Width = 350;
                        msg.Show("Declaration Document is missing");
                        return;
                    }
                    else {
                        var documentName = _this.EntityPM.Tenant + "_" + documentFiling.DocumentId;
                        _this.ShowDocument(documentName);
                    }
                }
            }
            else {
                var msg = new MessageWindow_1.MessageWindow();
                msg.Width = 350;
                msg.Show("לא נמצאה הודעת חיוב ");
                return;
            }
        });
    };
    PaymentOrderMenuButtonsHandler.prototype.ShowDocument = function (documentName) {
        DownloadManager_1.DownloadManager.DownloadPage(documentName);
        //if(AmitalGatewayUtil.Instance.AmitalBrowserInUse) {
        //    AmitalGatewayUtil.Instance.DeclarationMessaging.RaiseOpenNewBrowser(uri);
        //    return;
        //}
    };
    return PaymentOrderMenuButtonsHandler;
}());
exports.PaymentOrderMenuButtonsHandler = PaymentOrderMenuButtonsHandler;
//# sourceMappingURL=PaymentOrderMenuButtonsHandler.js.map