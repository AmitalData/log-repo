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
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var Tools_1 = require("../../../../Infrastructure/Tools");
var Tools_2 = require("../../../../Shipment/Tools");
var CardListService_1 = require("../../../../Common/Services/StandardLists/CardListService");
var PartnersDomainService_1 = require("../../../../Common/Services/PartnersDomainService");
var FSRWebService_1 = require("../../../../Infrastructure/Services/WebServices/FSRWebService");
var MessageWindow_1 = require("../../../../Controls/Windows/MessageWindow");
var BranchListService_1 = require("../../../../Common/Services/StandardLists/BranchListService");
var SendShipmentFSRComponent = /** @class */ (function (_super) {
    __extends(SendShipmentFSRComponent, _super);
    function SendShipmentFSRComponent() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.ObjectTableName = "Master";
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.ValidationErrorsList = [];
        _this.InitializeComponent();
        return _this;
    }
    SendShipmentFSRComponent.prototype.ngOnInit = function () {
        this.SetUIProperties();
    };
    SendShipmentFSRComponent.prototype.SetWindowArgs = function (workSpace) {
        this.workSpaceComponent = workSpace;
    };
    SendShipmentFSRComponent.prototype.InitializeComponent = function () {
        this.EntityPM = Tools_2.ShipmentTool.GetNewShipmentPM();
        this.EntityPM.IsSendFSRCreatingShipment = true;
        this.EntityPM.ShipmentLevelCode = "C";
        this.EntityPM.TransportModeId = "A";
        this.EntityPM.Ratio = 6;
        this.EntityPM.AWBDeclaredValueForCarriage = "NVD";
        this.EntityPM.AWBDeclaredValueForCustoms = "NCV";
        this.EntityPM.AWBInsurrenceValue = "XXX";
        this.EntityPM.RateClassCode = "Q";
        this.EntityPM.CASSCode = SessionLocator_1.SessionLocator.TenantPM.CASSCode;
        this.EntityPM.IssuingCarrierIATACode = SessionLocator_1.SessionLocator.TenantPM.IATA;
        this.EntityPM.IssuingCarrierAgentId = SessionLocator_1.SessionLocator.TenantPM.AgentId;
        this.EntityPM.IssuingCarrierAddressId = SessionLocator_1.SessionLocator.TenantPM.AddressId;
        this.GetAWBSignature();
        this.Master = null;
        this.DirectionId = "I";
        this.AirlinePrefix = null;
        this.MainCarriageCarrierId = null;
        this.GetCardData();
    };
    SendShipmentFSRComponent.prototype.GetAWBSignature = function () {
        var _this = this;
        if (this.EntityPM.BranchId) {
            var myResult = null;
            var myService = new BranchListService_1.BranchListService();
            myService.getSingleFromCache(this.EntityPM.BranchId).subscribe(function (myResponse) {
                if (!myResponse.HasError) {
                    var list = myResponse.Result;
                    if (list) {
                        myResult = list.Signature;
                    }
                    if (Tools_1.AppTool.IsNullOrEmpty(myResult)) {
                        myResult = SessionLocator_1.SessionLocator.TenantPM.Signature;
                    }
                    _this.EntityPM.AWBSignature = myResult;
                }
            });
        }
        else {
            this.EntityPM.AWBSignature = SessionLocator_1.SessionLocator.TenantPM.Signature;
        }
    };
    SendShipmentFSRComponent.prototype.SetUIProperties = function () {
        this.SetUIProperties_Prefix();
        this.SetUIProperties_Master();
    };
    SendShipmentFSRComponent.prototype.SetUIProperties_Prefix = function () {
        this.UIProperties.SetRequired("AirlinePrefix", this.ObjectTableName, false);
        this.UIProperties.SetValidity("AirlinePrefix", this.ObjectTableName, true, "");
        if (Tools_1.AppTool.IsNullOrEmpty(this.AirlinePrefix)) {
            this.UIProperties.SetRequired("AirlinePrefix", this.ObjectTableName, true);
        }
        else if (!Tools_1.FormatTool.IsNumeric(this.AirlinePrefix)) {
            this.UIProperties.SetValidity("AirlinePrefix", this.ObjectTableName, false, "Invalid Format");
        }
        else if (this.AirlinePrefix.length != 3) {
            this.UIProperties.SetValidity("AirlinePrefix", this.ObjectTableName, false, "Prefix length is 3 digits");
        }
    };
    SendShipmentFSRComponent.prototype.SetUIProperties_Master = function () {
        this.UIProperties.SetRequired("Master", this.ObjectTableName, false);
        this.UIProperties.SetValidity("Master", this.ObjectTableName, true, "");
        if (Tools_1.AppTool.IsNullOrEmpty(this.Master)) {
            this.UIProperties.SetRequired("Master", this.ObjectTableName, true);
        }
        else {
            var myResult = Tools_1.AppTool.ValidateMasterField(this.EntityPM.Master, this.EntityPM.TransportModeId, true, true);
            if (!Tools_1.AppTool.IsNullOrEmpty(myResult)) {
                this.UIProperties.SetValidity("Master", this.ObjectTableName, false, myResult);
            }
        }
    };
    Object.defineProperty(SendShipmentFSRComponent.prototype, "AirlinePrefix", {
        get: function () { return this.EntityPM.AirlinePrefix; },
        set: function (newValue) {
            if (this.EntityPM.AirlinePrefix != newValue) {
                this.EntityPM.AirlinePrefix = newValue;
                this.SetUIProperties_Prefix();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SendShipmentFSRComponent.prototype, "Master", {
        get: function () { return this.EntityPM.Master; },
        set: function (newValue) {
            if (this.EntityPM.Master != newValue) {
                this.EntityPM.Master = newValue;
                this.SetUIProperties_Master();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SendShipmentFSRComponent.prototype, "DirectionId", {
        get: function () { return this.EntityPM.DirectionId; },
        set: function (newValue) {
            if (this.EntityPM.DirectionId != newValue) {
                this.EntityPM.DirectionId = newValue;
                if (newValue == "E") {
                    this.EntityPM.FreightPrepaidCollectId = SessionLocator_1.SessionLocator.TenantPM.MasterExportFreightPrepaidCollectId;
                    this.EntityPM.OtherPrepaidCollectId = SessionLocator_1.SessionLocator.TenantPM.MasterExportOtherPrepaidCollectId;
                }
                else {
                    this.EntityPM.FreightPrepaidCollectId = SessionLocator_1.SessionLocator.TenantPM.MasterImpFreiPrepaidCollectId;
                    this.EntityPM.OtherPrepaidCollectId = SessionLocator_1.SessionLocator.TenantPM.MasterImportOtherPrepaidCollectId;
                }
                Tools_2.ShipmentTool.BuildAWBChargesCodeCode(this.EntityPM);
            }
        },
        enumerable: true,
        configurable: true
    });
    SendShipmentFSRComponent.prototype.GetCardData = function () {
        var _this = this;
        if (this.EntityPM.IssuingCarrierAgentId != null) {
            var myService = new CardListService_1.CardListService();
            myService.getSingle(this.EntityPM.IssuingCarrierAgentId).subscribe(function (myResult) {
                var myCard = myResult;
                if (myCard != null) {
                    _this.EntityPM.IssuingCarrierAgentNote = myCard.Notes;
                    _this.EntityPM.IssuingCarrierAgentName = myCard.EnglishName;
                    _this.EntityPM.IssuingCarrierAddressId = myCard.MainAddressId;
                }
            });
        }
    };
    Object.defineProperty(SendShipmentFSRComponent.prototype, "MainCarriageCarrierId", {
        get: function () { return this.EntityPM.MainCarriageCarrierId; },
        set: function (newValue) {
            if (this.EntityPM.MainCarriageCarrierId != newValue) {
                this.EntityPM.MainCarriageCarrierId = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    SendShipmentFSRComponent.prototype.OnPrefixLostFocus = function (myPrefix) {
        if (!Tools_1.AppTool.IsNullOrEmpty(this.AirlinePrefix)) {
            if (this.AirlinePrefix.length == 3) {
                this.GetAirlineByPrefix();
            }
        }
    };
    SendShipmentFSRComponent.prototype.GetAirlineByPrefix = function () {
        var _this = this;
        var myService = new PartnersDomainService_1.PartnersDomainService();
        myService.GetAirlineByPrefix(this.AirlinePrefix).subscribe(function (myResponse) {
            if (!myResponse.HasError) {
                var list = myResponse.Result;
                if (list) {
                    _this.MainCarriageCarrierId = list.Id;
                }
            }
        });
    };
    // Commands
    SendShipmentFSRComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    SendShipmentFSRComponent.prototype.SendButtonClicked = function () {
        var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.FieldIsRequired");
        var errors = [];
        if (Tools_1.AppTool.IsNullOrEmpty(this.AirlinePrefix)) {
            errors.push(msg.replace("%FieldName", "Prefix"));
        }
        else if (!Tools_1.FormatTool.IsNumeric(this.AirlinePrefix)) {
            errors.push("Prefix invalid format");
        }
        else if (this.AirlinePrefix.length != 3) {
            errors.push("Prefix length is 3 digits");
        }
        if (Tools_1.AppTool.IsNullOrEmpty(this.Master)) {
            errors.push(msg.replace("%FieldName", "Master"));
        }
        else {
            var myResult = Tools_1.AppTool.ValidateMasterField(this.EntityPM.Master, this.EntityPM.TransportModeId, true, true);
            if (!Tools_1.AppTool.IsNullOrEmpty(myResult)) {
                errors.push(myResult);
            }
        }
        if (Tools_1.AppTool.IsNullOrEmpty(SessionLocator_1.SessionLocator.TenantManagementJS.TTY)) {
            errors.push("Tenant communication parameter (TTY) is missing");
        }
        this.ValidationErrorsList = errors;
        if (errors.length == 0) {
            this.CurrentSession.StartBusyIndicator("Sending...");
            if (this.EntityPM.DirectionId == "E" || this.EntityPM.DirectionId == "D") {
                this.EntityPM.ShipperId = this.EntityPM.IssuingCarrierAgentId;
                this.EntityPM.ShipperAddressId = this.EntityPM.IssuingCarrierAddressId;
                this.EntityPM.ShipperNote = this.EntityPM.IssuingCarrierAgentNote;
                this.EntityPM.ShipperName = this.EntityPM.IssuingCarrierAgentName;
            }
            else {
                this.EntityPM.ConsigneeId = this.EntityPM.IssuingCarrierAgentId;
                this.EntityPM.ConsigneeAddressId = this.EntityPM.IssuingCarrierAddressId;
                this.EntityPM.ConsigneeNote = this.EntityPM.IssuingCarrierAgentNote;
                this.EntityPM.ConsigneeName = this.EntityPM.IssuingCarrierAgentName;
            }
            this.Send();
        }
    };
    SendShipmentFSRComponent.prototype.Send = function () {
        var _this = this;
        var myService = new FSRWebService_1.FSRWebService();
        myService.SendFSRShipment(this.EntityPM).subscribe(function (myResponse) {
            _this.CurrentSession.StopBusyIndicator();
            if (myResponse.HasError) {
                _this.ValidationErrorsList = myResponse.ErrorsArray;
            }
            else {
                if (myResponse.Result instanceof FSRWebService_1.FSRResultClass) {
                    var messageWindow = new MessageWindow_1.MessageWindow();
                    messageWindow.Show("FSR was sent successfully Please check airline updates query for carrier’s response");
                    _this.InitializeComponent();
                    if (_this.workSpaceComponent != null) {
                        _this.workSpaceComponent.LoadQueriesCounts();
                        _this.workSpaceComponent.LoadRecentShipments();
                    }
                }
                else {
                    var errors = [];
                    errors.push("Sending FSR failed");
                    _this.ValidationErrorsList = errors;
                }
            }
        });
    };
    SendShipmentFSRComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './SendShipmentFSRComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], SendShipmentFSRComponent);
    return SendShipmentFSRComponent;
}(BaseComponent_1.BaseComponent));
exports.SendShipmentFSRComponent = SendShipmentFSRComponent;
//# sourceMappingURL=SendShipmentFSRComponent.js.map