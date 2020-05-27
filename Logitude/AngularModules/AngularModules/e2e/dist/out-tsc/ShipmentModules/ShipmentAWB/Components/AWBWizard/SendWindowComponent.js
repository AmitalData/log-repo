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
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var FeatureLocator_1 = require("../../../../Infrastructure/Utilities/FeatureLocator");
var Tools_1 = require("../../../../Shipment/Tools");
var Tools_2 = require("../../../../Infrastructure/Tools");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var CCSWebService_1 = require("../../../../Infrastructure/Services/WebServices/CCSWebService");
var SendWindowComponent = /** @class */ (function () {
    function SendWindowComponent() {
        this.IsRecipientsVisible = false;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        // Message Status
        this.IsMessageValid = false;
        this.IsMessageWarning = false;
        this.IsMessageError = false;
        this.MessageWarningText = null;
        this.MessageErrorText = null;
        // FHLs Status
        this.IsFHLsValid = false;
        this.IsFHLsWarning = false;
        this.IsFHLsError = false;
        this.FHLsWarningText = null;
        this.FHLsErrorText = null;
        this.IsFHLsStatusVisible = false;
        this.FHLsValidationList = [];
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
        this.mySendingResultClass = new CCSWebService_1.CCSResult();
        this.myValidationResultClass = new CCSWebService_1.AWBResultClass();
        this.redForeground = "#E53030";
        this.greenForeground = "#009161";
        this.ValidationErrorsList = [];
        this.ValidationWarningsList = [];
        this.PurchaseStockUri = "https://www.plimus.com/jsp/buynow.jsp?contractId=3233898&language=ENGLISH&currency=USD&custom1=" + SessionLocator_1.SessionLocator.Tenant + "&quantity=1";
    }
    SendWindowComponent.prototype.SetWindowArgs = function (args) {
        this.Tenant = args.EnttiyPM.Tenant;
        this.entityPM = args.EnttiyPM;
        this.Wizard = args.Wizard;
        this.isSendingFHLs = args.IsSendingFHLs;
        this.isSendingDEXX = args.IsSendingDEXX;
        this.isSendingCargonaut = args.IsSendingCargonaut;
        this.InitializeComponent();
    };
    SendWindowComponent.prototype.InitializeComponent = function () {
        this.isGLSHK = Tools_1.ShipmentTool.IsGLSHK();
        this.tenantZeroAirlineField = Tools_1.ShipmentTool.GetTenantZeroAirlineField(this.entityPM);
        this.OtherRecipient = this.tenantZeroAirlineField;
        this.SetRecipientsVisibility();
        this.SetDefaultRecipient();
        this.SetWarnings();
        this.SetMessageType();
        this.SetSendButton();
        if (this.isSendingFHLs) {
            this.IsFHLsStatusVisible = true;
            this.LoadFHLValidators();
        }
        else {
            this.SetMessageStatus();
        }
    };
    SendWindowComponent.prototype.InitializeWebService = function () {
        if (this.myCCSWebService == null) {
            this.myCCSWebService = new CCSWebService_1.CCSWebService();
        }
    };
    // Warnings
    SendWindowComponent.prototype.SetWarnings = function () {
        var warnings = [];
        if (this.isSendingFHLs || this.entityPM.ShipmentLevelCode == "H") {
            if (this.entityPM.FWBStatusCode == "NSEN") {
                warnings.push("The FWB is not sent");
            }
        }
        else if (this.entityPM.ShipmentLevelCode == "C") {
            if (this.entityPM.FHLStatusCode == "PSEN") {
                warnings.push("Not all the FHL(s) are sent");
            }
        }
        this.ValidationWarningsList = warnings;
    };
    SendWindowComponent.prototype.SetMessageType = function () {
        var myResult = "";
        if (this.isSendingCargonaut) {
            myResult = "Cargonaut ";
        }
        else if (this.isSendingDEXX) {
            myResult = "DEXX ";
        }
        if (this.entityPM.ShipmentLevelCode == "H") {
            myResult += "FHL";
        }
        else {
            myResult += "FWB";
        }
        this.MessageType = myResult;
    };
    SendWindowComponent.prototype.SetMessageStatus = function () {
        if (this.isSendingFHLs) {
            this.SetFWBStatus();
            this.SetFHLsStatus();
        }
        else if (this.entityPM.ShipmentLevelCode == "H") {
            this.SetFHLStatus();
        }
        else {
            this.SetFWBStatus();
        }
    };
    SendWindowComponent.prototype.SetFWBStatus = function () {
        if (this.entityPM.ShipmentLevelCode == "C") {
            var isConsolidationValid = this.IsConsolidationValid();
            if (!isConsolidationValid) {
                this.MessageWarningText = "This FWB is not connected to any FHL";
                this.IsMessageWarning = true;
            }
            else {
                this.IsMessageValid = true;
            }
        }
        else {
            this.IsMessageValid = true;
        }
    };
    SendWindowComponent.prototype.SetFHLStatus = function () {
        if (Tools_2.AppTool.IsNullOrEmpty(this.entityPM.MasterShipmentDataId)) {
            this.MessageErrorText = "This FHL is not connected to FWB";
            this.IsMessageError = true;
        }
        else {
            this.IsMessageValid = true;
        }
    };
    SendWindowComponent.prototype.IsConsolidationValid = function () {
        var isValid = false;
        if (this.entityPM.ShipmentConsoleShipments.length > 0) {
            isValid = true;
        }
        return isValid;
    };
    Object.defineProperty(SendWindowComponent.prototype, "FHLsStatusLabel", {
        get: function () {
            var myResult = "";
            if (this.isSendingCargonaut) {
                myResult = "Cargonaut FHL(s) Status";
            }
            else if (this.isSendingDEXX) {
                myResult = "DEXX FHL(s) Status";
            }
            else {
                myResult = "FHL(s) Status";
            }
            return myResult;
        },
        enumerable: true,
        configurable: true
    });
    SendWindowComponent.prototype.SetFHLsStatus = function () {
        this.IsFHLsValid = false;
        this.IsFHLsWarning = false;
        this.IsFHLsError = false;
        if (this.FHLsValidationList.length == 0) {
            this.FHLsErrorText = "No connected FHLs found";
            this.IsFHLsError = true;
        }
        else if (this.FHLsValidationList.filter(function (d) { return d.IsFHLValid; }).length == 0) {
            this.FHLsErrorText = "All FHL(s) are invalid! none will be sent";
            this.IsFHLsError = true;
        }
        else if (this.FHLsValidationList.filter(function (d) { return d.IsFHLValid; }).length > 0 && this.FHLsValidationList.filter(function (d) { return !d.IsFHLValid; }).length > 0) {
            this.FHLsWarningText = "Warning: some of the FHL(s) has got errors and will not be sent, Only valid FHL(s) will be sent";
            this.IsFHLsWarning = true;
        }
        else {
            this.IsFHLsValid = true;
        }
    };
    SendWindowComponent.prototype.LoadFHLValidators = function () {
        var _this = this;
        if (this.isSendingFHLs) {
            this.CurrentSession.StartBusyIndicator(TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.Loading"));
            this.InitializeWebService();
            this.myCCSWebService.GetFHLsValidation(this.entityPM.Id).subscribe(function (myResponse) {
                _this.FHLsValidationList = [];
                if (myResponse != null) {
                    if (myResponse.HasError) {
                        _this.ValidationErrorsList = myResponse.ErrorsArray;
                    }
                    else {
                        var myResult = myResponse.Result;
                        var list1 = [];
                        var list2 = [];
                        if (_this.isSendingCargonaut || _this.isSendingDEXX) {
                            list1 = myResult.filter(function (d) { return d.CargonautFHLStatusCode == "SENT"; });
                            list2 = myResult.filter(function (d) { return d.CargonautFHLStatusCode != "SENT"; });
                        }
                        else {
                            list1 = myResult.filter(function (d) { return d.FHLStatusCode == "SENT"; });
                            list2 = myResult.filter(function (d) { return d.FHLStatusCode != "SENT"; });
                        }
                        list1.forEach(function (item) {
                            _this.FHLsValidationList.push(new FHLStatusItemViewModel(item, _this, _this.isSendingCargonaut));
                        });
                        list2.forEach(function (item) {
                            _this.FHLsValidationList.push(new FHLStatusItemViewModel(item, _this, _this.isSendingCargonaut));
                        });
                    }
                }
                _this.SetMessageStatus();
                _this.CurrentSession.StopBusyIndicator();
            });
        }
    };
    SendWindowComponent.prototype.SetRecipientsVisibility = function () {
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
    SendWindowComponent.prototype.SetDefaultRecipient = function () {
        if (this.isSendingCargonaut && this.IsCargonautVisible) {
            this.CargonautRecipientIsChecked = true;
        }
        else if (this.isSendingDEXX && this.IsDEXXVisible) {
            this.DEXXRecipientIsChecked = true;
        }
        else if (!Tools_2.AppTool.IsNullOrEmpty(this.tenantZeroAirlineField)) {
            this.OtheRecipientIsChecked = true;
        }
        else {
            this.CargRecipientIsChecked = true;
        }
    };
    SendWindowComponent.prototype.FalseAllRadioButtons = function () {
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
    Object.defineProperty(SendWindowComponent.prototype, "SelectedRecipient", {
        get: function () { return this.selectedRecipient; },
        set: function (newValue) {
            if (this.selectedRecipient != newValue) {
                this.selectedRecipient = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SendWindowComponent.prototype, "CargRecipientIsChecked", {
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
    Object.defineProperty(SendWindowComponent.prototype, "XXArRecipientIsChecked", {
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
    Object.defineProperty(SendWindowComponent.prototype, "Fra1RecipientIsChecked", {
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
    Object.defineProperty(SendWindowComponent.prototype, "Fra2RecipientIsChecked", {
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
    Object.defineProperty(SendWindowComponent.prototype, "RichRecipientIsChecked", {
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
    Object.defineProperty(SendWindowComponent.prototype, "IhabRecipientIsChecked", {
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
    Object.defineProperty(SendWindowComponent.prototype, "CargonautRecipientIsChecked", {
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
    Object.defineProperty(SendWindowComponent.prototype, "DEXXRecipientIsChecked", {
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
    Object.defineProperty(SendWindowComponent.prototype, "OtheRecipientIsChecked", {
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
    SendWindowComponent.prototype.SetSendButton = function () {
        var isVisible = false;
        if (FeatureLocator_1.FeatureLocator.IsPackage_DVMT()) {
            isVisible = true;
        }
        else {
            if (this.ValidationWarningsList.length > 0) {
                isVisible = true;
            }
            else {
                var errors = this.ValidateSending();
                if (errors.length > 0) {
                    isVisible = true;
                }
            }
        }
        this.IsSendButtonVisible = isVisible;
        if (!isVisible) {
            this.SendClicked();
        }
    };
    SendWindowComponent.prototype.CloseClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    SendWindowComponent.prototype.SendClicked = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicator("Prepairing data...");
        this.StockErrorMessage = null;
        this.StockErrorIsVisible = false;
        this.InitializeWebService();
        var errors = this.ValidateSending();
        this.ValidationErrorsList = errors;
        if (errors.length == 0) {
            this.myCCSWebService.GetSendingValidations(this.entityPM.Id, this.SelectedRecipient, this.isSendingFHLs, this.isSendingCargonaut, this.isSendingDEXX, this.entityPM.MainCarriageCarrierId).subscribe(function (myResponse) {
                if (myResponse == null) {
                    _this.CurrentSession.StopBusyIndicator();
                }
                else if (myResponse.HasError) {
                    _this.ValidationErrorsList = myResponse.ErrorsArray;
                    _this.CurrentSession.StopBusyIndicator();
                }
                else {
                    var myResult = myResponse.Result;
                    _this.myValidationResultClass = myResult;
                    if (myResult.IsValid) {
                        _this.InitializeSendingData();
                    }
                    else {
                        _this.ValidationErrorsList = myResult.ErrorsList;
                        if (myResult.HasStockErrors) {
                            _this.StockErrorMessage = "You are trying to send (" + myResult.SendingCount + ") messages, your remaining stock is (" + myResult.StockRemainingBefore + ") which is insufficient for this operation. Please purchase another messaging stock via the link";
                            _this.StockErrorIsVisible = true;
                        }
                        _this.SendingResultForeground = _this.redForeground;
                        if (_this.entityPM.ShipmentLevelCode == "C" && _this.isSendingFHLs) {
                            _this.SendingResultMessage = "Error sending FHL(s)";
                        }
                        else {
                            _this.SendingResultMessage = "Error sending " + _this.MessageType;
                        }
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
    SendWindowComponent.prototype.ValidateSending = function () {
        this.SendingResultMessage = "";
        var errors = [];
        this.ValidateRecipient(errors);
        if (this.entityPM.ShipmentLevelCode == "H") {
            this.ValidateFHL(errors);
        }
        else {
            if (this.entityPM.ShipmentLevelCode == "C" && this.isSendingFHLs) {
                this.ValidateFHLs(errors);
            }
        }
        return errors;
    };
    SendWindowComponent.prototype.ValidateRecipient = function (errors) {
        if (FeatureLocator_1.FeatureLocator.IsPackage_DVMT()) {
            if (Tools_2.AppTool.IsNullOrEmpty(this.SelectedRecipient)) {
                errors.push("Please fill your Recipient");
            }
            else {
                this.SelectedRecipient = this.SelectedRecipient.toUpperCase();
                if (this.isGLSHK) {
                }
                else {
                    //if (this.SelectedRecipient.length != 7) {
                    //    errors.push("Recipient length must be 7");
                    //}
                }
            }
        }
    };
    SendWindowComponent.prototype.ValidateFHL = function (errors) {
        if (Tools_2.AppTool.IsNullOrEmpty(this.entityPM.MasterShipmentDataId)) {
            errors.push("This FHL is not connected to FWB");
        }
    };
    SendWindowComponent.prototype.ValidateFHLs = function (errors) {
        if (this.entityPM.ShipmentConsoleShipments.length == 0) {
            errors.push("This FWB is not connected to any FHL");
        }
        else if (this.FHLsValidationList.filter(function (d) { return d.IsFHLValid; }).length == 0) {
            errors.push("All FHL(s) are invalid");
        }
    };
    // Validate Online
    SendWindowComponent.prototype.ValidateOnline = function () {
    };
    Object.defineProperty(SendWindowComponent.prototype, "StockAreaIsVisible", {
        // Sending Report
        get: function () {
            var myResult = false;
            if (!this.DemoAreaIsVisible) {
                if (SessionLocator_1.SessionLocator.TenantManagementJS.IsAWBStockPrepaid) {
                    myResult = true;
                }
            }
            return myResult;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SendWindowComponent.prototype, "DemoAreaIsVisible", {
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
    SendWindowComponent.prototype.SetStockScreen = function () {
        if (this.isSendingFHLs) {
            this.SendingCount = this.myValidationResultClass.SendingCount;
            this.StockRemainingBefore = this.myValidationResultClass.StockRemainingBefore;
        }
        else {
            this.SendingCount = this.mySendingResultClass.SendingCount;
            this.StockRemainingBefore = this.mySendingResultClass.StockRemainingBefore;
        }
        this.StockRemainingAfter = this.mySendingResultClass.StockRemainingAfter;
        this.StockResultIsVisible = false;
        this.StockErrorIsVisible = false;
        if (this.StockRemainingAfter < this.StockRemainingBefore) {
            this.StockResultIsVisible = true;
        }
    };
    SendWindowComponent.prototype.InitializeSendingData = function () {
        this.sendingQueueIndex = 0;
        this.entitiesDataOnQueue = [];
        if (this.myValidationResultClass.IsSendingFHLs) {
            this.entitiesDataOnQueue = this.myValidationResultClass.ValidFHLsDataStringList;
        }
        else {
            this.entitiesDataOnQueue.push(this.entityPM.Id + ":" + this.entityPM.ShipmentNumber);
        }
        this.SendData();
    };
    SendWindowComponent.prototype.SendData = function () {
        if (this.entitiesDataOnQueue.length == 0) {
            this.SetStockScreen();
            this.SendingResultForeground = this.greenForeground;
            if (this.entityPM.ShipmentLevelCode == "C" && this.isSendingFHLs) {
                this.SendingResultMessage = "All valid FHL(s) have been sent Successfully";
            }
            else {
                this.SendingResultMessage = this.MessageType + " has been sent Successfully";
            }
            this.CurrentSession.StopBusyIndicator();
            this.ReloadEntity();
            //this.currentAssemlyLocator.ListControl.GetSingleList(shipmentPM.Id);
            //this.currentAssemlyLocator.CurrentSimplogWindow.StopBusyIndicator();
            //RefreshScreenEvent myEvent = SessionLocator.CurrentAssemblyLocator.EventAggregator.GetEvent<RefreshScreenEvent>();
            //myEvent.Publish(new RefreshScreenEventArgs("ChampService"));
        }
        else {
            this.sendingQueueIndex += 1;
            var itemDataString = this.entitiesDataOnQueue[0];
            var itemStringArray = itemDataString.split(':');
            var entityId = itemStringArray[0];
            var entityNumber = itemStringArray[1];
            var indexOfItem = this.entitiesDataOnQueue.indexOf(itemDataString);
            if (indexOfItem > -1) {
                this.entitiesDataOnQueue.splice(indexOfItem, 1);
            }
            this.Sending(entityId, entityNumber);
        }
    };
    SendWindowComponent.prototype.Sending = function (entityId, entityNumber) {
        var _this = this;
        var busyIndicatorText = "Sending in Progress..";
        if (this.isSendingFHLs) {
            busyIndicatorText = "Sending FHL (" + this.sendingQueueIndex + " of " + this.myValidationResultClass.ValidHousesCount + ") " + entityNumber;
        }
        this.CurrentSession.StopBusyIndicator();
        this.CurrentSession.StartBusyIndicator(busyIndicatorText);
        this.myCCSWebService.Send(entityId, this.SelectedRecipient, this.isSendingCargonaut, this.isSendingDEXX).subscribe(function (myResponse) {
            if (myResponse == null) {
                _this.CurrentSession.StopBusyIndicator();
            }
            else if (myResponse.HasError) {
                _this.ValidationErrorsList = myResponse.ErrorsArray;
                _this.CurrentSession.StopBusyIndicator();
            }
            else {
                var myResult = myResponse.Result;
                _this.mySendingResultClass = myResult;
                if (myResult == null) {
                    _this.CurrentSession.StopBusyIndicator();
                }
                else {
                    if (myResult.HasStockError) {
                        _this.SendingResultForeground = _this.redForeground;
                        _this.SendingResultMessage = "Error sending: No remaining stock";
                        _this.CurrentSession.StopBusyIndicator();
                    }
                    else {
                        _this.SendData();
                    }
                }
            }
        });
    };
    SendWindowComponent.prototype.ReloadEntity = function () {
        var _this = this;
        this.Wizard.LoadCompleted.subscribe(function ($event) {
            _this.LoadFHLValidators();
        });
        this.Wizard.ReloadEntity();
    };
    SendWindowComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './SendWindowComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], SendWindowComponent);
    return SendWindowComponent;
}());
exports.SendWindowComponent = SendWindowComponent;
var FHLStatusItemViewModel = /** @class */ (function () {
    function FHLStatusItemViewModel(item, fatherComponent, isSendingCargonaut) {
        this.item = item;
        this.fatherComponent = fatherComponent;
        this.IsFHLValid = false;
        this.Shipper = item.Shipper;
        this.StatusName = isSendingCargonaut ? item.CargonautFHLStatusName : item.FHLStatusName;
        this.ShipmentNumber = item.ShipmentNumber;
        this.IsFHLValid = item.IsFHLValid;
    }
    FHLStatusItemViewModel.prototype.ViewShipment = function () {
        var _this = this;
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Width = 960;
        logWindow.Height = 600;
        logWindow.Title = "Edit HAWB Wizard";
        logWindow.WindowArgs = this.item.ShipmentId;
        logWindow.WindowClosed.subscribe(function ($event) { return _this.fatherComponent.LoadFHLValidators(); });
        logWindow.Show('./ShipmentModules/ShipmentAWB/Components/AWBWizard/AWBWizardLoadComponent');
    };
    return FHLStatusItemViewModel;
}());
exports.FHLStatusItemViewModel = FHLStatusItemViewModel;
//# sourceMappingURL=SendWindowComponent.js.map