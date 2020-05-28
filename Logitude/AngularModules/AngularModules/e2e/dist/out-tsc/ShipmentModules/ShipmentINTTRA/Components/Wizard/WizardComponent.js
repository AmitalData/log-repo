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
var FeatureLocator_1 = require("../../../../Infrastructure/Utilities/FeatureLocator");
var INTRAWebService_1 = require("../../../../Shipment/Services/INTRAWebService");
var MessageWindow_1 = require("../../../../Controls/Windows/MessageWindow");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var Cloner_1 = require("../../../../Infrastructure/Utilities/Cloner");
var CardListService_1 = require("../../../../Common/Services/StandardLists/CardListService");
var Tools_2 = require("../../../../Shipment/Tools");
var WizardComponent = /** @class */ (function (_super) {
    __extends(WizardComponent, _super);
    function WizardComponent() {
        var _this = _super.call(this) || this;
        _this.EntityPM = null;
        _this.DataContext = _this;
        _this.ObjectTableName = "Shipment";
        _this.ShipmentId = null;
        _this.ResultMessage = null;
        _this.ValidationErrorsList = [];
        _this.IsValid = true;
        _this.IsLimited = false;
        _this.IsDevelopment = false;
        _this.SimulatorIsVisible = false;
        _this.SimulatorBookingIsVisible = false;
        _this.IsEditingEnabled = false;
        _this.IsDemoAreaVisible = false;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.SaveCompletedEvent = null;
        _this.LoadCompletedEvent = null;
        _this.myService = new INTRAWebService_1.INTRAWebService();
        _this.CardListService = new CardListService_1.CardListService();
        if (SessionLocator_1.SessionLocator.Tenant == 65 || SessionLocator_1.SessionLocator.TenantManagementJS.IsINTTRAOnlyDemo) {
            _this.IsDemoAreaVisible = true;
        }
        return _this;
    }
    WizardComponent.prototype.SetWindowArgs = function (args) {
        var _this = this;
        this.EntityPM = args['Shipment'];
        this.entityArgs = args['EntityArgs'];
        this.ShipmentId = this.EntityPM.Id;
        this.IsDevelopment = FeatureLocator_1.FeatureLocator.IsPackage_DVMT();
        this.SimulatorIsVisible = FeatureLocator_1.FeatureLocator.HasFeaturePermession(this.ObjectTableName, "INTTRASimulator") ? true : false;
        this.SimulatorBookingIsVisible = FeatureLocator_1.FeatureLocator.HasFeaturePermession(this.ObjectTableName, "INTTRABookingSimulator") ? true : false;
        this.SetSendingLimitation(this.EntityPM.INTTRASIStatusCode);
        this.Clone();
        this.SetUIProperties();
        if (!this.IsLimited) {
            this.myService.Validate(this.ShipmentId).subscribe(function (myResponse) {
                if (myResponse.HasError) {
                    _this.CurrentSession.StopBusyIndicator();
                    _this.ValidationErrorsList = myResponse.ErrorsArray;
                }
                else {
                    var myResult = myResponse.Result;
                    if (!_this.IsDevelopment) {
                        _this.IsLimited = myResult.IsLimited;
                    }
                    if (myResult.Errors.length > 0) {
                        _this.CurrentSession.StopBusyIndicator();
                        _this.ValidationErrorsList = myResult.Errors;
                    }
                    else if (myResult.IsCarrierRegisteredToINTTRA == false) {
                        _this.ValidationErrorsList.push("Shipping line is not Registered To INTTRA");
                    }
                    else if (myResult.IsCarrierRegisteredToBranch == false) {
                        _this.ValidationErrorsList.push("Shipping line is not Registered To Branch");
                    }
                    else {
                        _this.ResultMessage = "Shipping instructions are ready for sending to INTTRA";
                    }
                }
            });
        }
    };
    WizardComponent.prototype.SetSendingLimitation = function (StatusCode) {
        var isLimited = false;
        if (!this.IsDevelopment) {
            if (!Tools_1.AppTool.IsNullOrEmpty(StatusCode)) {
                switch (StatusCode) {
                    case "NSEN":
                    case "RJIN":
                        {
                            break;
                        }
                    default: {
                        isLimited = true;
                        break;
                    }
                }
            }
        }
        this.IsLimited = isLimited;
    };
    WizardComponent.prototype.SetUIProperties = function () {
        this.IsEditingEnabled = Tools_2.ShipmentTool.IsEditingEnabled(this.EntityPM);
        this.UIProperties.SetEnabled("EmergencyContactId", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("INTTRAContractNumber", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("INTTRADocumentTypeCode", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("INTTRADocumentQTY", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("INTTRAIsFreighted", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("FreightPayerId", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("BasicFreightId", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("DestinationPortChargesId", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("DestinationHaulageChargesId", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("AdditionalChargesId", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("INTTRAInstructions", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("INTTRAComments", this.ObjectTableName, this.IsEditingEnabled);
        this.SetUIProperties_BasicFreight();
        this.SetUIProperties_FreightPayerAddress();
    };
    WizardComponent.prototype.SetUIProperties_BasicFreight = function () {
        var isRequired = false;
        if (Tools_1.AppTool.IsNullOrEmpty(this.BasicFreightId)) {
            isRequired = true;
        }
        this.UIProperties.SetRequired("BasicFreightId", this.ObjectTableName, isRequired);
    };
    WizardComponent.prototype.SetUIProperties_FreightPayerAddress = function () {
        var isEnabled = false;
        if (this.IsEditingEnabled) {
            if (this.FreightPayerId != null) {
                isEnabled = true;
            }
        }
        this.UIProperties.SetEnabled("FreightPayerAddressId", this.ObjectTableName, isEnabled);
        var isRequired = false;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.FreightPayerId)) {
            if (Tools_1.AppTool.IsNullOrEmpty(this.FreightPayerAddressId)) {
                isRequired = true;
            }
        }
        this.UIProperties.SetRequired("FreightPayerAddressId", this.ObjectTableName, isRequired);
    };
    Object.defineProperty(WizardComponent.prototype, "EmergencyContactId", {
        get: function () { return this.EntityPM.EmergencyContactId; },
        set: function (value) {
            if (this.EntityPM.EmergencyContactId != value) {
                this.EntityPM.EmergencyContactId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(WizardComponent.prototype, "INTTRAContractNumber", {
        get: function () { return this.EntityPM.INTTRAContractNumber; },
        set: function (value) {
            if (this.EntityPM.INTTRAContractNumber != value) {
                this.EntityPM.INTTRAContractNumber = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(WizardComponent.prototype, "INTTRAInstructions", {
        get: function () { return this.EntityPM.INTTRAInstructions; },
        set: function (value) {
            if (this.EntityPM.INTTRAInstructions != value) {
                this.EntityPM.INTTRAInstructions = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(WizardComponent.prototype, "INTTRAComments", {
        get: function () { return this.EntityPM.INTTRAComments; },
        set: function (value) {
            if (this.EntityPM.INTTRAComments != value) {
                this.EntityPM.INTTRAComments = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(WizardComponent.prototype, "INTTRADocumentQTY", {
        get: function () { return this.EntityPM.INTTRADocumentQTY; },
        set: function (value) {
            if (this.EntityPM.INTTRADocumentQTY != value) {
                this.EntityPM.INTTRADocumentQTY = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(WizardComponent.prototype, "INTTRADocumentTypeCode", {
        get: function () { return this.EntityPM.INTTRADocumentTypeCode; },
        set: function (value) {
            if (this.EntityPM.INTTRADocumentTypeCode != value) {
                this.EntityPM.INTTRADocumentTypeCode = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(WizardComponent.prototype, "INTTRAIsFreighted", {
        get: function () { return this.EntityPM.INTTRAIsFreighted; },
        set: function (value) {
            if (this.EntityPM.INTTRAIsFreighted != value) {
                this.EntityPM.INTTRAIsFreighted = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(WizardComponent.prototype, "BasicFreightId", {
        get: function () { return this.EntityPM.BasicFreightId; },
        set: function (value) {
            if (this.EntityPM.BasicFreightId != value) {
                this.EntityPM.BasicFreightId = value;
                this.SetUIProperties_BasicFreight();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(WizardComponent.prototype, "DestinationPortChargesId", {
        get: function () { return this.EntityPM.DestinationPortChargesId; },
        set: function (value) {
            if (this.EntityPM.DestinationPortChargesId != value) {
                this.EntityPM.DestinationPortChargesId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(WizardComponent.prototype, "DestinationHaulageChargesId", {
        get: function () { return this.EntityPM.DestinationHaulageChargesId; },
        set: function (value) {
            if (this.EntityPM.DestinationHaulageChargesId != value) {
                this.EntityPM.DestinationHaulageChargesId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(WizardComponent.prototype, "AdditionalChargesId", {
        get: function () { return this.EntityPM.AdditionalChargesId; },
        set: function (value) {
            if (this.EntityPM.AdditionalChargesId != value) {
                this.EntityPM.AdditionalChargesId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(WizardComponent.prototype, "FreightPayerId", {
        get: function () { return this.EntityPM.FreightPayerId; },
        set: function (value) {
            var _this = this;
            if (this.EntityPM.FreightPayerId != value) {
                this.EntityPM.FreightPayerId = value;
                if (Tools_1.AppTool.IsNullOrEmpty(value)) {
                    this.FreightPayerAddressId = null;
                }
                else {
                    this.CardListService.getSingle(value).subscribe(function (myResponse) {
                        if (!myResponse.HasError) {
                            var list = myResponse.Result;
                            if (list) {
                                _this.FreightPayerAddressId = list.MainAddressId;
                            }
                        }
                    });
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(WizardComponent.prototype, "FreightPayerAddressId", {
        get: function () { return this.EntityPM.FreightPayerAddressId; },
        set: function (value) {
            if (this.EntityPM.FreightPayerAddressId != value) {
                this.EntityPM.FreightPayerAddressId = value;
                this.SetUIProperties_FreightPayerAddress();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(WizardComponent.prototype, "SIHasAttachList", {
        get: function () { return this.EntityPM.SIHasAttachList; },
        set: function (value) {
            if (this.EntityPM.SIHasAttachList != value) {
                this.EntityPM.SIHasAttachList = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    WizardComponent.prototype.CloseButtonClicked = function () {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    };
    WizardComponent.prototype.SendButtonClicked = function () {
        this.ResultMessage = null;
        this.ValidationErrorsList = [];
        if (this.EntityPM.IsDirty) {
            this.Save();
        }
        else {
            this.Send();
        }
    };
    WizardComponent.prototype.Save = function () {
        var _this = this;
        if (!this.SaveCompletedEvent) {
            this.SaveCompletedEvent = this.entityArgs.EditComponent.SaveCompleted.subscribe(function (isSaveSuccess) {
                if (isSaveSuccess) {
                    _this.EntityPM = null;
                    _this.EntityPM = _this.entityArgs.EditComponent.EntityPM;
                    _this.Clone();
                    _this.Send();
                }
                else {
                    _this.ValidationErrorsList = _this.entityArgs.EditComponent.ValidationErrorsList;
                }
                Tools_1.AppTool.KillEventEmitter(_this.SaveCompletedEvent);
                _this.SaveCompletedEvent = null;
            });
            this.entityArgs.EditComponent.SaveChanges();
        }
    };
    WizardComponent.prototype.Send = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicator("Sending...");
        this.myService.Validate(this.ShipmentId).subscribe(function (myResponse) {
            if (myResponse.HasError) {
                _this.CurrentSession.StopBusyIndicator();
                _this.ValidationErrorsList = myResponse.ErrorsArray;
            }
            else {
                var myResult = myResponse.Result;
                if (!_this.IsDevelopment) {
                    _this.IsLimited = myResult.IsLimited;
                }
                if (myResult.Errors.length > 0) {
                    _this.CurrentSession.StopBusyIndicator();
                    _this.ValidationErrorsList = myResult.Errors;
                }
                else if (myResult.IsCarrierRegisteredToINTTRA == false) {
                    _this.CurrentSession.StopBusyIndicator();
                    var messageWindow = new MessageWindow_1.MessageWindow();
                    messageWindow.Show("Shipping line is not Registered To INTTRA");
                }
                else if (myResult.IsCarrierRegisteredToBranch == false) {
                    _this.CurrentSession.StopBusyIndicator();
                    var messageWindow = new MessageWindow_1.MessageWindow();
                    messageWindow.Show("Shipping line is not Registered To Branch");
                }
                else {
                    if (_this.IsLimited) {
                        _this.CurrentSession.StopBusyIndicator();
                        var messageWindow = new MessageWindow_1.MessageWindow();
                        messageWindow.Show("SI re-sending to INTTRA is not allowed");
                    }
                    else {
                        _this.myService.Send(_this.ShipmentId).subscribe(function (myResponse) {
                            if (myResponse.HasError) {
                                _this.ValidationErrorsList = myResponse.ErrorsArray;
                                _this.CurrentSession.StopBusyIndicator();
                            }
                            else {
                                var myResult = myResponse.Result;
                                if (myResult.Errors.length > 0) {
                                    _this.ValidationErrorsList = myResult.Errors;
                                    _this.CurrentSession.StopBusyIndicator();
                                }
                                else if (myResult.HasStockError == true) {
                                    _this.CurrentSession.StopBusyIndicator();
                                    var messageWindow = new MessageWindow_1.MessageWindow();
                                    messageWindow.Show("No Stock");
                                }
                                else {
                                    if (!_this.IsDevelopment) {
                                        _this.IsLimited = true;
                                    }
                                    _this.ResultMessage = "Message has been sent Successfully";
                                    _this.CurrentSession.StopBusyIndicator();
                                    if (!_this.LoadCompletedEvent) {
                                        _this.LoadCompletedEvent = _this.entityArgs.EditComponent.LoadCompleted.subscribe(function (isLoadSuccess) {
                                            if (isLoadSuccess) {
                                                _this.EntityPM = _this.entityArgs.EditComponent.EntityPM;
                                                _this.Clone();
                                            }
                                            else {
                                                _this.ValidationErrorsList = _this.entityArgs.EditComponent.ValidationErrorsList;
                                            }
                                            Tools_1.AppTool.KillEventEmitter(_this.SaveCompletedEvent);
                                            _this.SaveCompletedEvent = null;
                                        });
                                        _this.entityArgs.EditComponent.ReloadEntityPM();
                                    }
                                }
                            }
                        });
                    }
                }
            }
        });
    };
    WizardComponent.prototype.Clone = function () {
        this.myCloner = new Cloner_1.Cloner(this.DataContext);
        this.myCloner.AddField('EmergencyContactId');
        this.myCloner.AddField('INTTRAContractNumber');
        this.myCloner.AddField('INTTRADocumentTypeCode');
        this.myCloner.AddField('INTTRADocumentQTY');
        this.myCloner.AddField('INTTRAIsFreighted');
        this.myCloner.AddField('FreightPayerId');
        this.myCloner.AddField('FreightPayerAddressId');
        this.myCloner.AddField('BasicFreightId');
        this.myCloner.AddField('DestinationPortChargesId');
        this.myCloner.AddField('DestinationHaulageChargesId');
        this.myCloner.AddField('AdditionalChargesId');
        this.myCloner.AddField('INTTRAInstructions');
        this.myCloner.AddField('INTTRAComments');
        this.myCloner.AddField('SIHasAttachList');
        this.myCloner.AddEntity(this.EntityPM);
    };
    WizardComponent.prototype.RejectChanges = function () {
        this.myCloner.RejectChanges();
    };
    WizardComponent.prototype.SimulateClicked = function () {
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Title = "INTTRA Simulator";
        logWindow.Show('./ShipmentModules/ShipmentINTTRA/Components/Wizard/SimulatorComponent');
    };
    WizardComponent.prototype.SendBookingClicked = function () {
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Title = "INTTRA Simulator";
        logWindow.WindowArgs = this.ShipmentId;
        logWindow.Show('./ShipmentModules/ShipmentINTTRA/Components/Wizard/SimulatorBookingComponent');
    };
    WizardComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './WizardComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], WizardComponent);
    return WizardComponent;
}(BaseComponent_1.BaseComponent));
exports.WizardComponent = WizardComponent;
//# sourceMappingURL=WizardComponent.js.map