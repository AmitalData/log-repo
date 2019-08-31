"use strict";
var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    }
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
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
var Tools_1 = require("../../../../Infrastructure/Tools");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var ConfirmWindow_1 = require("../../../../Controls/Windows/ConfirmWindow");
var AccountingSettingPMService_1 = require("../../../../Common/Services/StandardPMs/AccountingSettingPMService");
var GlobalDomainService_1 = require("../../../../Common/Services/GlobalDomainService");
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var CommonDomainService_1 = require("../../../../Common/Services/CommonDomainService");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var ObjectsUpdater_1 = require("../../../../Infrastructure/Locators/ObjectsUpdater");
var MessageWindow_1 = require("../../../../Controls/Windows/MessageWindow");
var TransferSettingsComponent = /** @class */ (function (_super) {
    __extends(TransferSettingsComponent, _super);
    function TransferSettingsComponent(entityResourceService) {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.ObjectTableName = "AccountingSetting";
        _this.IsResourcesReady = false;
        _this.CanTransferToDropbox = false;
        _this.QBOWindowSessionEvent = null;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.IsAccountingSystem_NO = false;
        _this.IsAccountingSystem_HV_RH = false;
        _this.IsAccountingSystem_QB_QBO = false;
        _this.IsAccountingSystem_AI_GI = false;
        _this.isLogedInQBO = false;
        _this.isQBO = false;
        _this.isDropBoxConnected = false;
        _this.IsQuickBooksWindowOpened = false;
        _this.OldSessionAccountingSystem = SessionLocator_1.SessionLocator.AccountingSystemPM;
        _this.QBOWindowSessionEvent = _this.CurrentSession.SessionEvent.subscribe(function (res) {
            if (res.Name == "QBOWindowCLosed") {
                _this.QBOWindowCLosed(res.Timer);
                _this.RefreshData();
            }
        });
        entityResourceService.getEntityResourceByTableName("AccountingSetting").subscribe(function (res) {
            _this.InitializeServices();
            _this.LoadData();
        });
        return _this;
    }
    Object.defineProperty(TransferSettingsComponent.prototype, "NoEntity", {
        get: function () {
            if (Tools_1.AppTool.IsNullOrEmpty(this.AccountingSystemCode))
                return true;
            return false;
        },
        enumerable: true,
        configurable: true
    });
    TransferSettingsComponent.prototype.ngOnDestroy = function () {
        Tools_1.AppTool.KillEventEmitter(this.QBOWindowSessionEvent);
    };
    TransferSettingsComponent.prototype.InitializeServices = function () {
        this.entityPMService = new AccountingSettingPMService_1.AccountingSettingPMService();
        this.myGlobalDomainService = new GlobalDomainService_1.GlobalDomainService();
        this.CommonDomainService = new CommonDomainService_1.CommonDomainService();
    };
    TransferSettingsComponent.prototype.RefreshData = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicatorLoading();
        this.entityPMService.get(SessionLocator_1.SessionLocator.Tenant).subscribe(function (myResponse) {
            if (!myResponse.HasError) {
                _this.EntityPM.QBOAccessToken = myResponse.Result.QBOAccessToken;
                _this.EntityPM.QBOAccessTokenSecret = myResponse.Result.QBOAccessTokenSecret;
                var temp = _this.EntityPM.QBOrealMeID;
                _this.EntityPM.QBOrealMeID = myResponse.Result.QBOrealMeID;
            }
            _this.SetQuickBookProperties();
            _this.CurrentSession.StopBusyIndicator();
            if (!Tools_1.AppTool.IsNullOrEmpty(temp) && _this.EntityPM.QBOrealMeID != temp) {
                var window = new MessageWindow_1.MessageWindow();
                window.ShowWarningIcon = true;
                window.Show("You are connecting to a different QBO environment than the previously connected. Please notice that old QBO translations will be erased");
                window.WindowClosed.subscribe(function (P) {
                    _this.IsResourcesReady = true;
                });
            }
            else {
                _this.IsResourcesReady = true;
            }
        });
    };
    TransferSettingsComponent.prototype.LoadData = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicatorLoading();
        this.entityPMService.get(SessionLocator_1.SessionLocator.Tenant).subscribe(function (myResponse) {
            if (!myResponse.HasError) {
                _this.EntityPM = myResponse.Result;
                if (_this.EntityPM.AccountingSystemCode != "QBO" && _this.EntityPM.AccountingSystemCode != "QBOG") {
                    _this.EntityPM.QBOAccessToken = null;
                    _this.EntityPM.QBOAccessTokenSecret = null;
                    _this.EntityPM.QBOrealMeID = null;
                }
            }
            _this.SetUIProperties();
            _this.SetQuickBookProperties();
            _this.CurrentSession.StopBusyIndicator();
            _this.IsResourcesReady = true;
        });
    };
    TransferSettingsComponent.prototype.SetUIProperties = function () {
        var isAccountingSystem_NO = false;
        var isAccountingSystem_HV_RH = false;
        var isAccountingSystem_QB_QBO = false;
        var isAccountingSystem_AI_GI = false;
        if (this.AccountingSystemCode == "NO") {
            isAccountingSystem_NO = true;
        }
        else if (this.AccountingSystemCode == "HV" || this.AccountingSystemCode == "RH") {
            isAccountingSystem_HV_RH = true;
        }
        else if (this.AccountingSystemCode == "QB" || this.AccountingSystemCode == "QBO" || this.AccountingSystemCode == "QBOG") {
            isAccountingSystem_QB_QBO = true;
        }
        else if (this.AccountingSystemCode == "GI" || this.AccountingSystemCode == "AI") {
            isAccountingSystem_AI_GI = true;
        }
        this.IsAccountingSystem_NO = isAccountingSystem_NO;
        this.IsAccountingSystem_HV_RH = isAccountingSystem_HV_RH;
        this.IsAccountingSystem_QB_QBO = isAccountingSystem_QB_QBO;
        this.IsAccountingSystem_AI_GI = isAccountingSystem_AI_GI;
        var isDemoTenant = false;
        if (SessionLocator_1.SessionLocator.Tenant == 65) {
            isDemoTenant = true;
            if (SessionLocator_1.SessionLocator.LoggedUserPM.Email) {
                if (SessionLocator_1.SessionLocator.LoggedUserPM.Email.toLowerCase() == "customercare@logitudeworld.com‏") {
                    isDemoTenant = false;
                }
            }
        }
        if (isDemoTenant) {
            this.UIProperties.SetEnabled("IsARInvoicesTransferEnabled", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("IsAPInvoicesTransferEnabled", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("IsARPaymentsTransferEnabled", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("IsAPPaymentsTransferEnabled", this.ObjectTableName, false);
        }
        else {
            var isARInvoicesTransferEnabled = false;
            var isAPInvoicesTransferEnabled = false;
            var isARPaymentsTransferEnabled = false;
            var isAPPaymentsTransferEnabled = false;
            if (SessionLocator_1.SessionLocator.AccountingSystemPM) {
                isARInvoicesTransferEnabled = SessionLocator_1.SessionLocator.AccountingSystemPM.AllowARInvoicesTransfer;
                isAPInvoicesTransferEnabled = SessionLocator_1.SessionLocator.AccountingSystemPM.AllowAPInvoicesTransfer;
                isARPaymentsTransferEnabled = SessionLocator_1.SessionLocator.AccountingSystemPM.AllowARPaymentsTransfer;
                isAPPaymentsTransferEnabled = SessionLocator_1.SessionLocator.AccountingSystemPM.AllowAPPaymentsTransfer;
            }
            this.UIProperties.SetEnabled("IsARInvoicesTransferEnabled", this.ObjectTableName, isARInvoicesTransferEnabled);
            this.UIProperties.SetEnabled("IsAPInvoicesTransferEnabled", this.ObjectTableName, isAPInvoicesTransferEnabled);
            this.UIProperties.SetEnabled("IsARPaymentsTransferEnabled", this.ObjectTableName, isARPaymentsTransferEnabled);
            this.UIProperties.SetEnabled("IsAPPaymentsTransferEnabled", this.ObjectTableName, isAPPaymentsTransferEnabled);
        }
        this.UIProperties.SetEnabled("ARInvoiceTransferStartDate", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("APInvoiceTransferStartDate", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("TransferToDropboxActivated", this.ObjectTableName, this.IsDropBoxConnected);
        var isReceivableVATableTempCardRequired = false;
        var isReceivableVATExemptTempCardRequired = false;
        var isPayableVATableTempCardRequired = false;
        var isPayableVATExemptTempCardRequired = false;
        var isReceivableVATCardRequired = false;
        var isPayableVATCardRequired = false;
        if (this.IsAccountingSystem_HV_RH) {
            if (this.IsARInvoicesTransferEnabled) {
                if (Tools_1.AppTool.IsNullOrEmpty(this.ReceivableVATableTempCard)) {
                    isReceivableVATableTempCardRequired = true;
                }
                if (Tools_1.AppTool.IsNullOrEmpty(this.ReceivableVATExemptTempCard)) {
                    isReceivableVATExemptTempCardRequired = true;
                }
                if (Tools_1.AppTool.IsNullOrEmpty(this.ReceivableVATCard)) {
                    isReceivableVATCardRequired = true;
                }
            }
            if (this.IsAPInvoicesTransferEnabled) {
                if (Tools_1.AppTool.IsNullOrEmpty(this.PayableVATableTempCard)) {
                    isPayableVATableTempCardRequired = true;
                }
                if (Tools_1.AppTool.IsNullOrEmpty(this.PayableVATExemptTempCard)) {
                    isPayableVATExemptTempCardRequired = true;
                }
                if (Tools_1.AppTool.IsNullOrEmpty(this.PayableVATCard)) {
                    isPayableVATCardRequired = true;
                }
            }
        }
        this.UIProperties.SetVisibility("ReceivableVATableTempCard", this.ObjectTableName, this.IsAccountingSystem_HV_RH);
        this.UIProperties.SetVisibility("ReceivableVATExemptTempCard", this.ObjectTableName, this.IsAccountingSystem_HV_RH);
        this.UIProperties.SetVisibility("PayableVATableTempCard", this.ObjectTableName, this.IsAccountingSystem_HV_RH);
        this.UIProperties.SetVisibility("PayableVATExemptTempCard", this.ObjectTableName, this.IsAccountingSystem_HV_RH);
        this.UIProperties.SetVisibility("ReceivableVATCard", this.ObjectTableName, this.IsAccountingSystem_HV_RH);
        this.UIProperties.SetVisibility("PayableVATCard", this.ObjectTableName, this.IsAccountingSystem_HV_RH);
        this.UIProperties.SetRequired("ReceivableVATableTempCard", this.ObjectTableName, isReceivableVATableTempCardRequired);
        this.UIProperties.SetRequired("ReceivableVATExemptTempCard", this.ObjectTableName, isReceivableVATExemptTempCardRequired);
        this.UIProperties.SetRequired("PayableVATableTempCard", this.ObjectTableName, isPayableVATableTempCardRequired);
        this.UIProperties.SetRequired("PayableVATExemptTempCard", this.ObjectTableName, isPayableVATExemptTempCardRequired);
        this.UIProperties.SetRequired("ReceivableVATCard", this.ObjectTableName, isReceivableVATCardRequired);
        this.UIProperties.SetRequired("PayableVATCard", this.ObjectTableName, isPayableVATCardRequired);
    };
    TransferSettingsComponent.prototype.SetQuickBookProperties = function () {
        if (this.EntityPM.QBOAccessToken != null) {
            this.isLogedInQBO = true;
        }
        else {
            this.isLogedInQBO = false;
        }
    };
    TransferSettingsComponent.prototype.QBOWindowCLosed = function (timer) {
        if (timer === void 0) { timer = null; }
        if (timer != null) {
            clearInterval(timer);
        }
    };
    Object.defineProperty(TransferSettingsComponent.prototype, "AccountingSystemCode", {
        get: function () { return this.EntityPM.AccountingSystemCode; },
        set: function (value) {
            var _this = this;
            if (this.EntityPM.AccountingSystemCode != value) {
                this.EntityPM.AccountingSystemCode = value;
                // For Disabling QBO Submit if not connected .
                if (value == "QBO" || value == "QBOG")
                    this.isQBO = true;
                else
                    this.isQBO = false;
                if (!Tools_1.AppTool.IsNullOrEmpty(value)) {
                    if (value == "NO") {
                        this.ReceivableVATableTempCard = null;
                        this.ReceivableVATExemptTempCard = null;
                        this.PayableVATableTempCard = null;
                        this.PayableVATExemptTempCard = null;
                        this.ReceivableVATCard = null;
                        this.PayableVATCard = null;
                    }
                    this.myGlobalDomainService.GetAccountingSystem(this.AccountingSystemCode).subscribe(function (myResponse2) {
                        if (!myResponse2.HasError) {
                            SessionLocator_1.SessionLocator.AccountingSystemPM = myResponse2.Result;
                            _this.IsARInvoicesTransferEnabled = SessionLocator_1.SessionLocator.AccountingSystemPM.AllowARInvoicesTransfer;
                            _this.IsAPInvoicesTransferEnabled = SessionLocator_1.SessionLocator.AccountingSystemPM.AllowAPInvoicesTransfer;
                            _this.IsARPaymentsTransferEnabled = SessionLocator_1.SessionLocator.AccountingSystemPM.AllowARPaymentsTransfer;
                            _this.IsAPPaymentsTransferEnabled = SessionLocator_1.SessionLocator.AccountingSystemPM.AllowAPPaymentsTransfer;
                            _this.SetUIProperties();
                        }
                    });
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    TransferSettingsComponent.prototype.SelectedItemChanged = function (AccountingSystem) {
        if (AccountingSystem != null) {
            this.SetUIProperties();
            this.CanTransferToDropbox = AccountingSystem.CanTransferToDropbox;
            if (this.CanTransferToDropbox == true) {
                this.CheckDropBoxConnection();
            }
        }
    };
    Object.defineProperty(TransferSettingsComponent.prototype, "ARInvoiceTransferStartDate", {
        get: function () { return this.EntityPM.ARInvoiceTransferStartDate; },
        set: function (value) {
            if (this.EntityPM.ARInvoiceTransferStartDate != value) {
                this.EntityPM.ARInvoiceTransferStartDate = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TransferSettingsComponent.prototype, "APInvoiceTransferStartDate", {
        get: function () { return this.EntityPM.APInvoiceTransferStartDate; },
        set: function (value) {
            if (this.EntityPM.APInvoiceTransferStartDate != value) {
                this.EntityPM.APInvoiceTransferStartDate = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TransferSettingsComponent.prototype, "ReceivableVATableTempCard", {
        get: function () { return this.EntityPM.ReceivableVATableTempCard; },
        set: function (value) {
            if (this.EntityPM.ReceivableVATableTempCard != value) {
                this.EntityPM.ReceivableVATableTempCard = value;
                this.SetUIProperties();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TransferSettingsComponent.prototype, "ReceivableVATExemptTempCard", {
        get: function () { return this.EntityPM.ReceivableVATExemptTempCard; },
        set: function (value) {
            if (this.EntityPM.ReceivableVATExemptTempCard != value) {
                this.EntityPM.ReceivableVATExemptTempCard = value;
                this.SetUIProperties();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TransferSettingsComponent.prototype, "PayableVATableTempCard", {
        get: function () { return this.EntityPM.PayableVATableTempCard; },
        set: function (value) {
            if (this.EntityPM.PayableVATableTempCard != value) {
                this.EntityPM.PayableVATableTempCard = value;
                this.SetUIProperties();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TransferSettingsComponent.prototype, "PayableVATExemptTempCard", {
        get: function () { return this.EntityPM.PayableVATExemptTempCard; },
        set: function (value) {
            if (this.EntityPM.PayableVATExemptTempCard != value) {
                this.EntityPM.PayableVATExemptTempCard = value;
                this.SetUIProperties();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TransferSettingsComponent.prototype, "ReceivableVATCard", {
        get: function () { return this.EntityPM.ReceivableVATCard; },
        set: function (value) {
            if (this.EntityPM.ReceivableVATCard != value) {
                this.EntityPM.ReceivableVATCard = value;
                this.SetUIProperties();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TransferSettingsComponent.prototype, "PayableVATCard", {
        get: function () { return this.EntityPM.PayableVATCard; },
        set: function (value) {
            if (this.EntityPM.PayableVATCard != value) {
                this.EntityPM.PayableVATCard = value;
                this.SetUIProperties();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TransferSettingsComponent.prototype, "IsAPInvoicesTransferEnabled", {
        get: function () { return this.EntityPM.IsAPInvoicesTransferEnabled; },
        set: function (value) {
            if (this.EntityPM.IsAPInvoicesTransferEnabled != value) {
                this.EntityPM.IsAPInvoicesTransferEnabled = value;
                this.SetUIProperties();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TransferSettingsComponent.prototype, "IsARPaymentsTransferEnabled", {
        get: function () { return this.EntityPM.IsARPaymentsTransferEnabled; },
        set: function (value) {
            if (this.EntityPM.IsARPaymentsTransferEnabled != value) {
                this.EntityPM.IsARPaymentsTransferEnabled = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TransferSettingsComponent.prototype, "IsARInvoicesTransferEnabled", {
        get: function () { return this.EntityPM.IsARInvoicesTransferEnabled; },
        set: function (value) {
            if (this.EntityPM.IsARInvoicesTransferEnabled != value) {
                this.EntityPM.IsARInvoicesTransferEnabled = value;
                this.SetUIProperties();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TransferSettingsComponent.prototype, "IsAPPaymentsTransferEnabled", {
        get: function () { return this.EntityPM.IsAPPaymentsTransferEnabled; },
        set: function (value) {
            if (this.EntityPM.IsAPPaymentsTransferEnabled != value) {
                this.EntityPM.IsAPPaymentsTransferEnabled = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TransferSettingsComponent.prototype, "TransferToDropboxActivated", {
        get: function () { return this.EntityPM.TransferToDropboxActivated; },
        set: function (value) {
            if (this.EntityPM.TransferToDropboxActivated != value) {
                this.EntityPM.TransferToDropboxActivated = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TransferSettingsComponent.prototype, "IsDropBoxConnected", {
        get: function () {
            return this.isDropBoxConnected;
        },
        set: function (value) {
            this.isDropBoxConnected = value;
        },
        enumerable: true,
        configurable: true
    });
    TransferSettingsComponent.prototype.RunConnectQuickBooks = function () {
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Width = 960;
        logWindow.Height = 570;
        logWindow.Title = "";
        logWindow.Show('./Invoice/Components/Workspaces/QuickBooksLogin');
    };
    TransferSettingsComponent.prototype.DissConnectQBO = function (loadeding) {
        var _this = this;
        if (loadeding === void 0) { loadeding = true; }
        if (loadeding) {
            this.CurrentSession.StartBusyIndicator("Disconnecting..");
            this.EntityPM.QBOAccessToken = null;
            this.EntityPM.QBOAccessTokenSecret = null;
        }
        if (this.EntityPM.AccountingSystemCode == "QBO" || this.EntityPM.AccountingSystemCode == "QBOG") {
            this.EntityPM.AccountingSystemCode = "NO";
            this.entityPMService.update(this.EntityPM).subscribe(function (myResponse1) {
                if (myResponse1.HasError) {
                    _this.ValidationErrorsList = myResponse1.ErrorsArray;
                    _this.CurrentSession.StopBusyIndicator();
                }
                else {
                    ObjectsUpdater_1.ObjectsUpdater.UpdateAccountingSettingPM(_this.EntityPM);
                    _this.myGlobalDomainService.GetAccountingSystem(_this.AccountingSystemCode).subscribe(function (myResponse2) {
                        if (!myResponse2.HasError) {
                            SessionLocator_1.SessionLocator.AccountingSystemPM = myResponse2.Result;
                        }
                    });
                    _this.SetQuickBookProperties();
                    _this.CurrentSession.StopBusyIndicator();
                }
            });
        }
    };
    TransferSettingsComponent.prototype.ViewXMLClicked = function () {
        var dualScreenLeft = window.screenLeft;
        var dualScreenTop = window.screenTop;
        var width = window.innerWidth ? window.innerWidth : document.documentElement.clientWidth ? document.documentElement.clientWidth : screen.width;
        var height = window.innerHeight ? window.innerHeight : document.documentElement.clientHeight ? document.documentElement.clientHeight : screen.height;
        var left = ((width / 2) - (1000 / 2)) + dualScreenLeft;
        var top = ((height / 2) - (650 / 2)) + dualScreenTop;
        var link = Tools_1.AppTool.GetLogitudeURL() + "Quickbooksonline.aspx?connect=true&tenant=" + SessionLocator_1.SessionLocator.Tenant;
        var new_window = window.open(link, "Authenticate with Quickbooks Online", 'scrollbars=yes, width=' + 1000 + ', height=' + 650 + ', top=' + top + ', left=' + left + ',directories=no,titlebar=no,toolbar=no,location=no,status=no,menubar=no,scrollbars=no,resizable=no');
        if (window.focus) {
            new_window.focus();
        }
        var timer = setInterval(function () {
            if (new_window) {
                if (new_window.closed) {
                    if (this.CurrentSession == null)
                        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
                    this.CurrentSession.SessionEvent.emit({ Name: "QBOWindowCLosed", Timer: timer });
                }
            }
        }, 500);
        this.IsQuickBooksWindowOpened = true;
    };
    TransferSettingsComponent.prototype.EditTransferStartDateClicked = function (typeCode) {
        var logWindowTitle = null;
        switch (typeCode) {
            case "ARInvoice": {
                logWindowTitle = "AR Invoice";
                break;
            }
            case "APInvoice": {
                logWindowTitle = "AP Invoice";
                break;
            }
            case "ARPayment": {
                logWindowTitle = "AR Payment";
                break;
            }
        }
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Width = 550;
        logWindow.Height = 300;
        logWindow.WindowArgs = { EntityPM: this.EntityPM, Code: typeCode };
        logWindow.Title = "Edit " + logWindowTitle + " Transfer Start Date";
        logWindow.Show('./Invoice/Components/Workspaces/Windows/TransferStartDateComponent');
    };
    TransferSettingsComponent.prototype.CancelButtonClicked = function () {
        this.ResetQBOSettings();
        SessionLocator_1.SessionLocator.AccountingSystemPM = this.OldSessionAccountingSystem;
        this.CurrentSession.CloseCurrentWindow();
    };
    TransferSettingsComponent.prototype.ResetQBOSettings = function () {
        var _this = this;
        this.entityPMService.get(SessionLocator_1.SessionLocator.Tenant).subscribe(function (myResponse) {
            var loadedEntity = myResponse.Result;
            if (myResponse.HasError) {
                _this.ValidationErrorsList = myResponse.ErrorsArray;
            }
            if (loadedEntity.AccountingSystemCode != "QBO" && loadedEntity.AccountingSystemCode != "QBOG") {
                _this.EntityPM.AccountingSystemCode = loadedEntity.AccountingSystemCode;
                _this.EntityPM.QBOrealMeID = null;
                _this.EntityPM.QBOAccessToken = null;
                _this.EntityPM.QBOAccessTokenSecret = null;
                _this.DissConnectQBO(false);
            }
        });
    };
    TransferSettingsComponent.prototype.OkButtonClicked = function () {
        var _this = this;
        var errors = [];
        var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.FieldIsRequired");
        if (this.AccountingSystemCode == "HV" || this.AccountingSystemCode == "RH") {
            if (this.IsARInvoicesTransferEnabled) {
                if (Tools_1.AppTool.IsNullOrEmpty(this.ReceivableVATableTempCard)) {
                    errors.push(TextCodeTranslator_1.TextCodeTranslator.Translate("AccountingSetting.F.ReceivableVATableTempCard"));
                }
                if (Tools_1.AppTool.IsNullOrEmpty(this.ReceivableVATExemptTempCard)) {
                    errors.push(TextCodeTranslator_1.TextCodeTranslator.Translate("AccountingSetting.F.ReceivableVATExemptTempCard"));
                }
                if (Tools_1.AppTool.IsNullOrEmpty(this.ReceivableVATCard)) {
                    errors.push(TextCodeTranslator_1.TextCodeTranslator.Translate("AccountingSetting.F.ReceivableVATCard"));
                }
            }
            if (this.IsAPInvoicesTransferEnabled) {
                if (Tools_1.AppTool.IsNullOrEmpty(this.PayableVATableTempCard)) {
                    errors.push(TextCodeTranslator_1.TextCodeTranslator.Translate("AccountingSetting.F.PayableVATableTempCard"));
                }
                if (Tools_1.AppTool.IsNullOrEmpty(this.PayableVATExemptTempCard)) {
                    errors.push(TextCodeTranslator_1.TextCodeTranslator.Translate("AccountingSetting.F.PayableVATExemptTempCard"));
                }
                if (Tools_1.AppTool.IsNullOrEmpty(this.PayableVATCard)) {
                    errors.push(TextCodeTranslator_1.TextCodeTranslator.Translate("AccountingSetting.F.PayableVATCard"));
                }
            }
        }
        this.ValidationErrorsList = errors;
        if (errors.length == 0) {
            this.CurrentSession.StartBusyIndicatorSaving();
            if (this.AccountingSystemCode != "QBO" && this.AccountingSystemCode != "QBOG") {
                //if (this.EntityPM.QBOrealMeID) {
                //    this.EntityPM.QBOrealMeID = null;
                //}
                if (this.EntityPM.QBOAccessToken) {
                    this.EntityPM.QBOAccessToken = null;
                }
                if (this.EntityPM.QBOAccessTokenSecret) {
                    this.EntityPM.QBOAccessTokenSecret = null;
                }
            }
            if (this.IsQuickBooksWindowOpened && (this.AccountingSystemCode == "QBO" || this.AccountingSystemCode == "QBOG")) {
                this.entityPMService.get(SessionLocator_1.SessionLocator.Tenant).subscribe(function (myResponse) {
                    if (myResponse.HasError) {
                        _this.ValidationErrorsList = myResponse.ErrorsArray;
                        _this.CurrentSession.StopBusyIndicator();
                    }
                    else {
                        var loadedEntity = myResponse.Result;
                        if (_this.EntityPM.QBOrealMeID != loadedEntity.QBOrealMeID) {
                            _this.EntityPM.QBOrealMeID = loadedEntity.QBOrealMeID;
                        }
                        if (_this.EntityPM.QBOAccessToken != loadedEntity.QBOAccessToken) {
                            _this.EntityPM.QBOAccessToken = loadedEntity.QBOAccessToken;
                        }
                        if (_this.EntityPM.QBOAccessTokenSecret != loadedEntity.QBOAccessTokenSecret) {
                            _this.EntityPM.QBOAccessTokenSecret = loadedEntity.QBOAccessTokenSecret;
                        }
                        _this.SaveChanges();
                    }
                });
            }
            else {
                this.SaveChanges();
            }
        }
    };
    TransferSettingsComponent.prototype.SaveChanges = function () {
        var _this = this;
        if (!this.EntityPM.IsDirty) {
            this.CurrentSession.CloseCurrentWindow();
        }
        else {
            this.entityPMService.update(this.EntityPM).subscribe(function (myResponse1) {
                if (myResponse1.HasError) {
                    _this.ValidationErrorsList = myResponse1.ErrorsArray;
                    _this.CurrentSession.StopBusyIndicator();
                }
                else {
                    SessionLocator_1.SessionLocator.AccountingSettingPM = _this.EntityPM;
                    _this.myGlobalDomainService.GetAccountingSystem(_this.AccountingSystemCode).subscribe(function (myResponse2) {
                        if (!myResponse2.HasError) {
                            SessionLocator_1.SessionLocator.AccountingSystemPM = myResponse2.Result;
                            _this.OldSessionAccountingSystem = SessionLocator_1.SessionLocator.AccountingSystemPM;
                        }
                        _this.CurrentSession.FireEvent("RefreshTransferComponent");
                        _this.CurrentSession.CloseCurrentWindowEmit("OK");
                    });
                }
            });
        }
    };
    TransferSettingsComponent.prototype.SendDropBoxChecked = function (arg) {
        var _this = this;
        if (arg == true) {
            var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
            confirmWindow.Width = 400;
            confirmWindow.Show("Activating this option will send the invoice transfer file to the connected dropbox account on invoice approval");
            confirmWindow.WindowClosed.subscribe(function (event) {
                if (confirmWindow.Yes) {
                    //nth
                }
                else if (confirmWindow.No) {
                    _this.TransferToDropboxActivated = false;
                }
            });
        }
    };
    TransferSettingsComponent.prototype.CheckDropBoxConnection = function () {
        var _this = this;
        this.CommonDomainService.GetDropBoxAccessTocken(SessionLocator_1.SessionLocator.Tenant).subscribe(function (response) {
            if (response.HasError) {
                _this.IsDropBoxConnected = false;
            }
            else {
                _this.IsDropBoxConnected = true;
            }
            _this.SetUIProperties();
        });
    };
    TransferSettingsComponent.prototype.ConnectDropBox = function () {
        var windowTitle = "Dropbox Connection";
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Width = 350;
        logWindow.Height = 225;
        logWindow.Title = windowTitle;
        logWindow.IsShowCloseButton = false;
        logWindow.Show('./InfrastructureModules/InfrastructureOthers/Components/DropBox/DropBoxConnectionComponent');
    };
    TransferSettingsComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './TransferSettingsComponent.html',
        }),
        __metadata("design:paramtypes", [EntityResourceService_1.EntityResourceService])
    ], TransferSettingsComponent);
    return TransferSettingsComponent;
}(BaseComponent_1.BaseComponent));
exports.TransferSettingsComponent = TransferSettingsComponent;
//# sourceMappingURL=TransferSettingsComponent.js.map