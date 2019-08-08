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
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var FeatureLocator_1 = require("../../../../Infrastructure/Utilities/FeatureLocator");
var Tools_1 = require("../../../../Infrastructure/Tools");
var FSRWebService_1 = require("../../../../Infrastructure/Services/WebServices/FSRWebService");
var SendFSRComponent = /** @class */ (function () {
    function SendFSRComponent() {
        this.IsRecipientsVisible = false;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.OverviewTab = null;
        // Recipients
        this.CargRecipient = "QIFFMXS"; // Cargo Test System
        this.XXArRecipient = "LUXX9CV"; // Luxembourg
        this.Fra1Recipient = "REUDLHT"; // Frankfurt
        this.Fra2Recipient = "ZCSSMIT"; // Frankfurt Simulator
        this.RichRecipient = "QIFRHXS"; // Richard Email
        this.IhabRecipient = "QIFFMAL"; // Ihab Email
        this.CargonautRecipient = "REUCGNP";
        this.DEXXRecipient = "REUBCSP";
        this.IsDEXXVisible = false;
        this.IsCargonautVisible = false;
        this.selectedRecipient = "QIFFMXS";
        this.IsSendButtonVisible = false;
        this.redForeground = "#E53030";
        this.greenForeground = "#009161";
        this.ValidationErrorsList = [];
    }
    SendFSRComponent.prototype.SetWindowArgs = function (args) {
        this.Tenant = args.EntityPM.Tenant;
        this.entityPM = args.EntityPM;
        this.OverviewTab = args.OverviewTab;
        this.tenantZeroAirlineField = args.EntityPM.TenantZeroAirlineTTY;
        this.OtherRecipient = this.tenantZeroAirlineField;
        this.InitializeComponent();
    };
    SendFSRComponent.prototype.InitializeComponent = function () {
        this.SetRecipientsVisibility();
        this.SetDefaultRecipient();
        this.SetSendButton();
    };
    SendFSRComponent.prototype.InitializeWebService = function () {
        if (this.myFSRWebService == null) {
            this.myFSRWebService = new FSRWebService_1.FSRWebService();
        }
    };
    SendFSRComponent.prototype.SetRecipientsVisibility = function () {
        if (FeatureLocator_1.FeatureLocator.IsPackage_DVMT()) {
            this.IsRecipientsVisible = true;
            switch (this.entityPM.MainCarriageFromPortCode) {
                case "SPL":
                case "AMS":
                case "RTM":
                case "MST":
                    {
                        this.IsCargonautVisible = true;
                        break;
                    }
                case "LGG":
                case "BRU":
                    {
                        this.IsDEXXVisible = true;
                        break;
                    }
                default:
                    {
                        this.IsDEXXVisible = false;
                        this.IsCargonautVisible = false;
                        break;
                    }
            }
        }
    };
    SendFSRComponent.prototype.SetDefaultRecipient = function () {
        if (!Tools_1.AppTool.IsNullOrEmpty(this.tenantZeroAirlineField)) {
            this.OtheRecipientIsChecked = true;
        }
        else {
            this.CargRecipientIsChecked = true;
        }
    };
    SendFSRComponent.prototype.FalseAllRadioButtons = function () {
        this.cargRecipientIsChecked = false;
        this.xXArRecipientIsChecked = false;
        this.fra1RecipientIsChecked = false;
        this.fra2RecipientIsChecked = false;
        this.richRecipientIsChecked = false;
        this.ihabRecipientIsChecked = false;
        this.cargonautRecipientIsChecked = false;
        this.dEXXRecipientIsChecked = false;
        this.otheRecipientIsChecked = false;
    };
    Object.defineProperty(SendFSRComponent.prototype, "SelectedRecipient", {
        get: function () { return this.selectedRecipient; },
        set: function (newValue) {
            if (this.selectedRecipient != newValue) {
                this.selectedRecipient = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SendFSRComponent.prototype, "CargRecipientIsChecked", {
        get: function () { return this.cargRecipientIsChecked; },
        set: function (newValue) {
            if (this.cargRecipientIsChecked != newValue) {
                this.FalseAllRadioButtons();
                this.cargRecipientIsChecked = newValue;
                if (newValue) {
                    this.SelectedRecipient = this.CargRecipient;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SendFSRComponent.prototype, "XXArRecipientIsChecked", {
        get: function () { return this.xXArRecipientIsChecked; },
        set: function (newValue) {
            if (this.xXArRecipientIsChecked != newValue) {
                this.FalseAllRadioButtons();
                this.xXArRecipientIsChecked = newValue;
                if (newValue) {
                    this.SelectedRecipient = this.XXArRecipient;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SendFSRComponent.prototype, "Fra1RecipientIsChecked", {
        get: function () { return this.fra1RecipientIsChecked; },
        set: function (newValue) {
            if (this.fra1RecipientIsChecked != newValue) {
                this.FalseAllRadioButtons();
                this.fra1RecipientIsChecked = newValue;
                if (newValue) {
                    this.SelectedRecipient = this.Fra1Recipient;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SendFSRComponent.prototype, "Fra2RecipientIsChecked", {
        get: function () { return this.fra2RecipientIsChecked; },
        set: function (newValue) {
            if (this.fra2RecipientIsChecked != newValue) {
                this.FalseAllRadioButtons();
                this.fra2RecipientIsChecked = newValue;
                if (newValue) {
                    this.SelectedRecipient = this.Fra2Recipient;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SendFSRComponent.prototype, "RichRecipientIsChecked", {
        get: function () { return this.richRecipientIsChecked; },
        set: function (newValue) {
            if (this.richRecipientIsChecked != newValue) {
                this.FalseAllRadioButtons();
                this.richRecipientIsChecked = newValue;
                if (newValue) {
                    this.SelectedRecipient = this.RichRecipient;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SendFSRComponent.prototype, "IhabRecipientIsChecked", {
        get: function () { return this.ihabRecipientIsChecked; },
        set: function (newValue) {
            if (this.ihabRecipientIsChecked != newValue) {
                this.FalseAllRadioButtons();
                this.ihabRecipientIsChecked = newValue;
                if (newValue) {
                    this.SelectedRecipient = this.IhabRecipient;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SendFSRComponent.prototype, "CargonautRecipientIsChecked", {
        get: function () { return this.cargonautRecipientIsChecked; },
        set: function (newValue) {
            if (this.cargonautRecipientIsChecked != newValue) {
                this.FalseAllRadioButtons();
                this.cargonautRecipientIsChecked = newValue;
                if (newValue) {
                    this.SelectedRecipient = this.CargonautRecipient;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SendFSRComponent.prototype, "DEXXRecipientIsChecked", {
        get: function () { return this.dEXXRecipientIsChecked; },
        set: function (newValue) {
            if (this.dEXXRecipientIsChecked != newValue) {
                this.dEXXRecipientIsChecked = newValue;
                if (newValue) {
                    this.SelectedRecipient = this.DEXXRecipient;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SendFSRComponent.prototype, "OtheRecipientIsChecked", {
        get: function () { return this.otheRecipientIsChecked; },
        set: function (newValue) {
            if (this.otheRecipientIsChecked != newValue) {
                this.FalseAllRadioButtons();
                this.otheRecipientIsChecked = newValue;
                if (newValue) {
                    this.SelectedRecipient = this.OtherRecipient;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    SendFSRComponent.prototype.SetSendButton = function () {
        var isVisible = false;
        if (FeatureLocator_1.FeatureLocator.IsPackage_DVMT()) {
            isVisible = true;
        }
        else {
            var errors = this.ValidateSending();
            if (errors.length > 0) {
                isVisible = true;
            }
        }
        this.IsSendButtonVisible = isVisible;
        if (!isVisible) {
            this.SendClicked();
        }
    };
    SendFSRComponent.prototype.CloseClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    SendFSRComponent.prototype.SendClicked = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicator("Sending in Progress..");
        var objectTableName = (this.entityPM.ShipmentLevelCode == "C") ? "Master" : "Shipment";
        var ObjectTable = window.ObjectTables.filter(function (x) { return x.Name === objectTableName; })[0];
        var objectTableId = ObjectTable.Id;
        this.InitializeWebService();
        var errors = this.ValidateSending();
        this.ValidationErrorsList = errors;
        if (errors.length == 0) {
            this.myFSRWebService.SendFSR(this.entityPM.Id, objectTableId, this.SelectedRecipient).subscribe(function (myResponse) {
                if (myResponse == null) {
                    _this.CurrentSession.StopBusyIndicator();
                }
                else if (myResponse.HasError) {
                    _this.ValidationErrorsList = myResponse.ErrorsArray;
                    _this.CurrentSession.StopBusyIndicator();
                }
                else {
                    var myResult = myResponse.Result;
                    if (myResult != null) {
                        _this.CurrentSession.StopBusyIndicator();
                        _this.SendingResultForeground = _this.greenForeground;
                        _this.SendingResultMessage = "FSR has been sent Successfully";
                        _this.ReloadEntity();
                    }
                    else {
                        _this.CurrentSession.StopBusyIndicator();
                    }
                }
            });
        }
        else {
            this.CurrentSession.StopBusyIndicator();
        }
    };
    // Validate
    SendFSRComponent.prototype.ValidateSending = function () {
        this.SendingResultMessage = "";
        var errors = [];
        if (Tools_1.AppTool.IsNullOrEmpty(this.entityPM.TenantZeroAirlineTTY)) {
            errors.push("Tenant communication parameter (TTY) is missing");
        }
        this.ValidateRecipient(errors);
        return errors;
    };
    SendFSRComponent.prototype.ValidateRecipient = function (errors) {
        if (FeatureLocator_1.FeatureLocator.IsPackage_DVMT()) {
            if (Tools_1.AppTool.IsNullOrEmpty(this.SelectedRecipient)) {
                errors.push("Please fill your Recipient");
            }
            else {
                this.SelectedRecipient = this.SelectedRecipient.toUpperCase();
                //if (this.SelectedRecipient.length != 7) {
                //    errors.push("Recipient length must be 7");
                //}
            }
        }
    };
    Object.defineProperty(SendFSRComponent.prototype, "DemoAreaIsVisible", {
        get: function () {
            var myResult = false;
            if (this.Tenant == 65 || SessionLocator_1.SessionLocator.TenantManagementJS.IsEAWBOnlyDemo) {
                myResult = true;
            }
            return myResult;
        },
        enumerable: true,
        configurable: true
    });
    SendFSRComponent.prototype.ReloadEntity = function () {
        if (this.OverviewTab != null) {
            this.OverviewTab.ReloadEntity();
        }
    };
    SendFSRComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './SendFSRComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], SendFSRComponent);
    return SendFSRComponent;
}());
exports.SendFSRComponent = SendFSRComponent;
var SendFSRArgs = /** @class */ (function () {
    function SendFSRArgs() {
    }
    return SendFSRArgs;
}());
exports.SendFSRArgs = SendFSRArgs;
//# sourceMappingURL=SendFSRComponent.js.map