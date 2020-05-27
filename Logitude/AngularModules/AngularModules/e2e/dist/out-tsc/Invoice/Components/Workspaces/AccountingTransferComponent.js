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
var Tools_1 = require("../../../Infrastructure/Tools");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var FeatureLocator_1 = require("../../../Infrastructure/Utilities/FeatureLocator");
var LogitudeWindow_1 = require("../../../Controls/Windows/LogitudeWindow");
var EntityResourceService_1 = require("../../../Infrastructure/Services/EntityResourceService");
var InvoiceDomainService_1 = require("../../Services/InvoiceDomainService");
var Args_1 = require("../../../Infrastructure/Args");
var TextCodeTranslator_1 = require("../../../Infrastructure/Utilities/TextCodeTranslator");
var MessageWindow_1 = require("../../../Controls/Windows/MessageWindow");
var AccountingTransferComponent = /** @class */ (function () {
    function AccountingTransferComponent(entityResourceService) {
        var _this = this;
        this.entityResourceService = entityResourceService;
        this.IsResourcesReady = false;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.AccountingSystemCode = null;
        this.AccountingSystemName = null;
        this.HasTransferFeature = false;
        this.IsARInvoicesEnabled = false;
        this.IsAPInvoicesEnabled = false;
        this.IsARPaymentsEnabled = false;
        this.IsAPPaymentsEnabled = false;
        this.IsAccountingSettingEnabled = false;
        this.IsVisible_ARInvoice = false;
        this.IsVisible_NewTransfer = false;
        this.IsVisible_ARInvoice_NotReady = false;
        this.IsVisible_ARInvoice_MarkedAsBlocked = false;
        this.IsVisible_ARInvoice_ErrorInTransfer = false;
        this.IsVisible_ARInvoice_TransferHistory = false;
        this.IsVisible_ARInvoice_RecalculateExternals = false;
        //-----
        this.IsVisible_APInvoice = false;
        this.IsVisible_APInvoice_NotReady = false;
        this.IsVisible_APInvoice_MarkedAsBlocked = false;
        this.IsVisible_APInvoice_ErrorInTransfer = false;
        this.IsVisible_APInvoice_TransferHistory = false;
        this.IsVisible_APInvoice_RecalculateExternals = false;
        //-----
        this.IsVisible_ARPayment = false;
        this.IsVisible_ARPayment_NotReady = false;
        this.IsVisible_ARPayment_MarkedAsBlocked = false;
        this.IsVisible_ARPayment_TransferHistory = false;
        this.IsVisible_ARPayment_RecalculateExternals = false;
        //-----
        this.IsVisible_APPayment = false;
        this.IsVisible_APPayment_NotReady = false;
        this.IsVisible_APPayment_MarkedAsBlocked = false;
        this.IsVisible_APPayment_TransferHistory = false;
        this.IsVisible_APPayment_RecalculateExternals = false;
        //------
        this.IsTaxesVisible = false;
        this.ARInvoicesNotReadyCount = "0";
        this.ARInvoicesDontTransferCount = "0";
        this.ARInvoicesErrorInTransferCount = "0";
        //-----
        this.APInvoicesNotReadyCount = "0";
        this.APInvoicesDontTransferCount = "0";
        this.APInvoicesErrorInTransferCount = "0";
        //-----
        this.ARPaymentsNotReadyCount = "0";
        this.ARPaymentsDontTransferCount = "0";
        this.ARPaymentsErrorInTransferCount = "0";
        //-----
        this.APPaymentsNotReadyCount = "0";
        this.APPaymentsDontTransferCount = "0";
        this.APPaymentsErrorInTransferCount = "0";
        this.invoiceDomainService = new InvoiceDomainService_1.InvoiceDomainService();
        entityResourceService.getEntityResourceByTableName("ARInvoice").subscribe(function (res1) {
            entityResourceService.getEntityResourceByTableName("APInvoice").subscribe(function (res2) {
                entityResourceService.getEntityResourceByTableName("ARPayment").subscribe(function (res3) {
                    entityResourceService.getEntityResourceByTableName("APPayment").subscribe(function (res4) {
                        entityResourceService.getEntityResourceByTableName("AccountingSetting").subscribe(function (res5) {
                            entityResourceService.getEntityResourceByTableName("AccountingTransferHeader").subscribe(function (res6) {
                                _this.InitializeComponent();
                                _this.IsResourcesReady = true;
                                _this.LoadDataCount();
                                _this.Listen();
                            });
                        });
                    });
                });
            });
        });
    }
    AccountingTransferComponent.prototype.Listen = function () {
        var _this = this;
        this.CurrentSession.SessionEvent.subscribe(function (s) {
            if (s == "Accounting_T") {
                _this.LoadDataCount();
            }
            if (s == "RefreshTransferComponent") {
                _this.InitializeComponent();
            }
        });
    };
    AccountingTransferComponent.prototype.InitializeComponent = function () {
        this.HasTransferFeature = false;
        this.IsARInvoicesEnabled = false;
        this.IsAPInvoicesEnabled = false;
        this.IsARPaymentsEnabled = false;
        this.IsAPPaymentsEnabled = false;
        this.IsAccountingSettingEnabled = false;
        this.AccountingSystemCode = SessionLocator_1.SessionLocator.AccountingSettingPM.AccountingSystemCode;
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "ACCOUNTINGTRANSFER")) {
            this.HasTransferFeature = true;
        }
        if (SessionLocator_1.SessionLocator.AccountingSystemPM) {
            this.AccountingSystemName = SessionLocator_1.SessionLocator.AccountingSystemPM.Name;
        }
        if (this.AccountingSystemCode != "NO") {
            if (!Tools_1.AppTool.IsNullOrEmpty(SessionLocator_1.SessionLocator.AccountingSettingPM.ReceivableVATableTempCard)
                && !Tools_1.AppTool.IsNullOrEmpty(SessionLocator_1.SessionLocator.AccountingSettingPM.ReceivableVATExemptTempCard)
                && !Tools_1.AppTool.IsNullOrEmpty(SessionLocator_1.SessionLocator.AccountingSettingPM.PayableVATExemptTempCard)
                && !Tools_1.AppTool.IsNullOrEmpty(SessionLocator_1.SessionLocator.AccountingSettingPM.PayableVATExemptTempCard)) {
                this.IsAccountingSettingEnabled = true;
            }
            else if (this.AccountingSystemCode == "QB" || this.AccountingSystemCode == "GI" || this.AccountingSystemCode == "AI") {
                this.IsAccountingSettingEnabled = true;
            }
        }
        if (this.IsAccountingSettingEnabled) {
            if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "ACCOUNTINGTRANSFER")) {
                if (SessionLocator_1.SessionLocator.AccountingSystemPM) {
                    if (SessionLocator_1.SessionLocator.AccountingSystemPM.AllowARInvoicesTransfer) {
                        if (SessionLocator_1.SessionLocator.AccountingSettingPM.IsARInvoicesTransferEnabled) {
                            this.IsARInvoicesEnabled = true;
                        }
                    }
                    if (SessionLocator_1.SessionLocator.AccountingSystemPM.AllowAPInvoicesTransfer) {
                        if (SessionLocator_1.SessionLocator.AccountingSettingPM.IsAPInvoicesTransferEnabled) {
                            this.IsAPInvoicesEnabled = true;
                        }
                    }
                    if (SessionLocator_1.SessionLocator.AccountingSystemPM.AllowARPaymentsTransfer) {
                        if (SessionLocator_1.SessionLocator.AccountingSettingPM.IsARPaymentsTransferEnabled) {
                            this.IsARPaymentsEnabled = true;
                        }
                    }
                    if (SessionLocator_1.SessionLocator.AccountingSystemPM.AllowAPPaymentsTransfer) {
                        if (SessionLocator_1.SessionLocator.AccountingSettingPM.IsAPPaymentsTransferEnabled) {
                            this.IsAPPaymentsEnabled = true;
                        }
                    }
                }
            }
        }
        this.SetQueriesUIProperties();
    };
    AccountingTransferComponent.prototype.SetQueriesUIProperties = function () {
        this.IsVisible_ARInvoice = false;
        this.IsVisible_APInvoice = false;
        this.IsVisible_ARPayment = false;
        this.IsVisible_APPayment = false;
        this.IsVisible_NewTransfer = false;
        this.IsVisible_ARInvoice_TransferHistory = false;
        this.IsVisible_APInvoice_TransferHistory = false;
        this.IsVisible_ARPayment_TransferHistory = false;
        this.IsVisible_APPayment_TransferHistory = false;
        this.IsTaxesVisible = false;
        if (this.AccountingSystemCode != "QB") {
            if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("AccountingTransferHeader", "NEW")) {
                this.IsVisible_NewTransfer = true;
                this.IsVisible_ARInvoice = true;
                this.IsVisible_APInvoice = true;
                this.IsVisible_ARPayment = true;
                this.IsVisible_APPayment = true;
            }
            if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("AccountingTransferHeader", "ARInvoiceTransferHistory")) {
                this.IsVisible_ARInvoice_TransferHistory = true;
                this.IsVisible_ARInvoice = true;
            }
            if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("AccountingTransferHeader", "APInvoiceTransferHistory")) {
                this.IsVisible_APInvoice_TransferHistory = true;
                this.IsVisible_APInvoice = true;
            }
            if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("AccountingTransferHeader", "ARPaymentTransferHistory")) {
                this.IsVisible_ARPayment_TransferHistory = true;
                this.IsVisible_ARPayment = true;
            }
            if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("AccountingTransferHeader", "APPaymentTransferHistory")) {
                this.IsVisible_APPayment_TransferHistory = true;
                this.IsVisible_APPayment = true;
            }
        }
        // ARInvoice
        this.IsVisible_ARInvoice_NotReady = false;
        this.IsVisible_ARInvoice_MarkedAsBlocked = false;
        this.IsVisible_ARInvoice_ErrorInTransfer = false;
        this.IsVisible_ARInvoice_RecalculateExternals = false;
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("ARInvoice", "NOTREADYINVOICES")) {
            this.IsVisible_ARInvoice_NotReady = true;
            this.IsVisible_ARInvoice = true;
        }
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("ARInvoice", "MARKEDASBLOCKEDFORTRANSFER")) {
            this.IsVisible_ARInvoice_MarkedAsBlocked = true;
            this.IsVisible_ARInvoice = true;
        }
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("ARInvoice", "ERRORINTRANSFERINVOICES")) {
            this.IsVisible_ARInvoice_ErrorInTransfer = true;
            this.IsVisible_ARInvoice = true;
        }
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("ARInvoice", "RecalculateExternals")) {
            this.IsVisible_ARInvoice_RecalculateExternals = true;
            this.IsVisible_ARInvoice = true;
        }
        // APInvoice
        this.IsVisible_APInvoice_NotReady = false;
        this.IsVisible_APInvoice_MarkedAsBlocked = false;
        this.IsVisible_APInvoice_ErrorInTransfer = false;
        this.IsVisible_APInvoice_RecalculateExternals = false;
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("APInvoice", "NOTREADYINVOICES")) {
            this.IsVisible_APInvoice_NotReady = true;
            this.IsVisible_APInvoice = true;
        }
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("APInvoice", "MARKEDASBLOCKEDFORTRANSFER")) {
            this.IsVisible_APInvoice_MarkedAsBlocked = true;
            this.IsVisible_APInvoice = true;
        }
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("APInvoice", "ERRORINTRANSFERINVOICES")) {
            this.IsVisible_APInvoice_ErrorInTransfer = true;
            this.IsVisible_APInvoice = true;
        }
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("APInvoice", "RecalculateExternals")) {
            this.IsVisible_APInvoice_RecalculateExternals = true;
            this.IsVisible_APInvoice = true;
        }
        // ARPayment
        this.IsVisible_ARPayment_NotReady = false;
        this.IsVisible_ARPayment_MarkedAsBlocked = false;
        this.IsVisible_ARPayment_RecalculateExternals = false;
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("ARPayment", "NOTREADYPAYMENTS")) {
            this.IsVisible_ARPayment_NotReady = true;
            this.IsVisible_ARPayment = true;
        }
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("ARPayment", "MARKEDASBLOCKEDFORTRANSFER")) {
            this.IsVisible_ARPayment_MarkedAsBlocked = true;
            this.IsVisible_ARPayment = true;
        }
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("ARPayment", "RecalculateExternals")) {
            this.IsVisible_ARPayment_RecalculateExternals = true;
            this.IsVisible_ARPayment = true;
        }
        // APPayment
        this.IsVisible_APPayment_NotReady = false;
        this.IsVisible_APPayment_MarkedAsBlocked = false;
        this.IsVisible_APPayment_RecalculateExternals = false;
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("APPayment", "NOTREADYPAYMENTS")) {
            this.IsVisible_APPayment_NotReady = true;
            this.IsVisible_APPayment = true;
        }
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("APPayment", "MARKEDASBLOCKEDFORTRANSFER")) {
            this.IsVisible_APPayment_MarkedAsBlocked = true;
            this.IsVisible_APPayment = true;
        }
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("APPayment", "RecalculateExternals")) {
            this.IsVisible_APPayment_RecalculateExternals = true;
            this.IsVisible_APPayment = true;
        }
        //Open format
        if (SessionLocator_1.SessionLocator.TenantPM.CountryCode == "IL") {
            this.IsTaxesVisible = true;
        }
    };
    AccountingTransferComponent.prototype.LoadDataCount = function () {
        var _this = this;
        if (this.IsAccountingSettingEnabled) {
            this.invoiceDomainService.GetAccountingTransferSummary().subscribe(function (myResponse) {
                if (!myResponse.HasError) {
                    if (myResponse.Result) {
                        _this.ARInvoicesNotReadyCount = myResponse.Result.ARInvoicesNotReadyCount > 1000 ? "1000+" : myResponse.Result.ARInvoicesNotReadyCount + "";
                        _this.ARInvoicesDontTransferCount = myResponse.Result.ARInvoicesDontTransferCount > 1000 ? "1000+" : myResponse.Result.ARInvoicesDontTransferCount + "";
                        _this.ARInvoicesErrorInTransferCount = myResponse.Result.ARInvoicesErrorInTransferCount > 1000 ? "1000+" : myResponse.Result.ARInvoicesErrorInTransferCount + "";
                        //-----
                        _this.APInvoicesNotReadyCount = myResponse.Result.APInvoicesNotReadyCount > 1000 ? "1000+" : myResponse.Result.APInvoicesNotReadyCount + "";
                        _this.APInvoicesDontTransferCount = myResponse.Result.APInvoicesDontTransferCount > 1000 ? "1000+" : myResponse.Result.APInvoicesDontTransferCount + "";
                        _this.APInvoicesErrorInTransferCount = myResponse.Result.APInvoicesErrorInTransferCount > 1000 ? "1000+" : myResponse.Result.APInvoicesErrorInTransferCount + "";
                        //-----
                        _this.ARPaymentsNotReadyCount = myResponse.Result.ARPaymentsNotReadyCount > 1000 ? "1000+" : myResponse.Result.ARPaymentsNotReadyCount + "";
                        _this.ARPaymentsDontTransferCount = myResponse.Result.ARPaymentsDontTransferCount > 1000 ? "1000+" : myResponse.Result.ARPaymentsDontTransferCount + "";
                        _this.ARPaymentsErrorInTransferCount = myResponse.Result.ARPaymentsErrorInTransferCount > 1000 ? "1000+" : myResponse.Result.ARPaymentsErrorInTransferCount + "";
                        //-----
                        _this.APPaymentsNotReadyCount = myResponse.Result.APPaymentsNotReadyCount > 1000 ? "1000+" : myResponse.Result.APPaymentsNotReadyCount + "";
                        _this.APPaymentsDontTransferCount = myResponse.Result.APPaymentsDontTransferCount > 1000 ? "1000+" : myResponse.Result.APPaymentsDontTransferCount + "";
                        _this.APPaymentsErrorInTransferCount = myResponse.Result.APPaymentsErrorInTransferCount > 1000 ? "1000+" : myResponse.Result.APPaymentsErrorInTransferCount + "";
                    }
                }
            });
        }
    };
    AccountingTransferComponent.prototype.NewTransferClicked = function (args) {
        if (args) {
            var logWindowTitle = null;
            var transferTypeCode = null;
            switch (args) {
                case "ARInvoice": {
                    transferTypeCode = "ARIN";
                    logWindowTitle = "New AR Invoice Accounting Transfer";
                    break;
                }
                case "APInvoice": {
                    transferTypeCode = "APIN";
                    logWindowTitle = "New AP Invoice Accounting Transfer";
                    break;
                }
                case "ARPayment": {
                    transferTypeCode = "ARPA";
                    logWindowTitle = "New AR Payment Accounting Transfer";
                    break;
                }
                case "APPayment": {
                    transferTypeCode = "APPA";
                    logWindowTitle = "New AP Payment Accounting Transfer";
                    break;
                }
            }
            var logWindow = new LogitudeWindow_1.LogitudeWindow();
            logWindow.Width = 960;
            logWindow.Height = 570;
            logWindow.Title = logWindowTitle;
            logWindow.WindowArgs = transferTypeCode;
            logWindow.Show('./Invoice/Components/Workspaces/Windows/NewTransferComponent');
        }
    };
    AccountingTransferComponent.prototype.RecalculateExternalClicked = function (args) {
        if (args) {
            var logWindow = new LogitudeWindow_1.LogitudeWindow();
            logWindow.Width = 500;
            logWindow.Height = 250;
            logWindow.WindowArgs = args;
            logWindow.Title = "Recalculate " + args + " Invoices external IDs";
            logWindow.Show('./Invoice/Components/Workspaces/Windows/RecalculateExternalsComponent');
        }
    };
    AccountingTransferComponent.prototype.ViewQueryClicked = function (args) {
        var _this = this;
        if (args) {
            var argsSplit = args.split(':');
            var objectTableName = argsSplit[0];
            var myQueryCode = argsSplit[1];
            var listArgs = new Args_1.ListComponentArgs();
            listArgs.QueryCode = myQueryCode;
            listArgs.ObjectTableName = objectTableName;
            listArgs.BackButtonTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("General.MH.Accounting");
            SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', this.CurrentSession.SessionMenuLocation.viewContainerRef)
                .then(function (cmpRef) {
                _this.CurrentSession.AddMenuReference(cmpRef);
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run(listArgs);
                cmpRef.instance.BackCompleted.subscribe(function ($event) {
                    _this.LoadDataCount();
                });
            });
        }
    };
    AccountingTransferComponent.prototype.OpenAccountingSystem = function () {
        var _this = this;
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Title = "Accounting Transfer Settings";
        logWindow.Show('./Invoice/Components/Workspaces/Windows/TransferSettingsComponent');
        logWindow.WindowClosed.subscribe(function (s) {
            if (s) {
                _this.InitializeComponent();
            }
        });
    };
    AccountingTransferComponent.prototype.OpenFormatReportClicked = function () {
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Title = "Dates";
        logWindow.Show('./Invoice/Components/Workspaces/Windows/PrintTaxComponent');
        logWindow.WindowClosed.subscribe(function (s) {
            if (s) {
                var msg = new MessageWindow_1.MessageWindow();
                msg.Show("An e-mail that includes two attachments will be sent to you in a few minutes");
            }
        });
    };
    AccountingTransferComponent = __decorate([
        core_1.Component({
            selector: 'AccountingTransferComponent',
            moduleId: module.id,
            templateUrl: './AccountingTransferComponent.html',
        }),
        __metadata("design:paramtypes", [EntityResourceService_1.EntityResourceService])
    ], AccountingTransferComponent);
    return AccountingTransferComponent;
}());
exports.AccountingTransferComponent = AccountingTransferComponent;
//# sourceMappingURL=AccountingTransferComponent.js.map