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
/// <reference path="../../../tools.ts" />
var core_1 = require("@angular/core");
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var EntityArgs_1 = require("../../../../Infrastructure/DataContracts/EntityArgs");
var PortListService_1 = require("../../../../Common/Services/StandardLists/PortListService");
var AddressListService_1 = require("../../../../Common/Services/StandardLists/AddressListService");
var Tools_1 = require("../../../../Infrastructure/Tools");
var CardListService_1 = require("../../../../Common/Services/StandardLists/CardListService");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var AddressPM_1 = require("../../../../Common/EntityPMs/AddressPM");
var WarehouseEntryRoutingsTabComponent = /** @class */ (function (_super) {
    __extends(WarehouseEntryRoutingsTabComponent, _super);
    function WarehouseEntryRoutingsTabComponent(entityArgs) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this.ObjectTableName = null;
        _this.DataContext = _this;
        _this.IsInlandDomestic = false;
        _this.CardDependencyProperty1 = "CS";
        _this.CardDependencyProperty1IsList = false;
        _this.FromPortText = "";
        _this.ToPortText = "";
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.IswarehouseEntryConnectedToShipment = false;
        _this.FromPortList = null;
        _this.ToPortList = null;
        _this.Listen();
        _this.EntityPM = entityArgs.EntityPM;
        _this.ObjectTableName = entityArgs.ObjectTableName;
        if (SessionLocator_1.SessionLocator.TenantPM.AllowAgentInCustomersLOV) {
            _this.CardDependencyProperty1 = "CS,AG";
            _this.CardDependencyProperty1IsList = true;
        }
        _this.Initialize();
        _this.LoadFromAddress();
        _this.LoadToAddress();
        return _this;
    }
    WarehouseEntryRoutingsTabComponent.prototype.ngOnInit = function () {
        if (this.EntityPM != null) {
            this.IsInlandDomestic = this.IsInlandDomesticWarehouse(this.EntityPM);
            if (this.EntityPM.ConnectedToShipment) {
                this.IswarehouseEntryConnectedToShipment = true;
                if (this.IsInlandDomestic) {
                    this.UIProperties.SetEnabled("FromPartnerId", this.ObjectTableName, !this.EntityPM.ConnectedToShipment);
                    this.UIProperties.SetEnabled("ToPartnerId", this.ObjectTableName, !this.EntityPM.ConnectedToShipment);
                    this.UIProperties.SetEnabled("FromAddressId", this.ObjectTableName, !this.EntityPM.ConnectedToShipment);
                    this.UIProperties.SetEnabled("ToAddressId", this.ObjectTableName, !this.EntityPM.ConnectedToShipment);
                }
                else {
                    this.UIProperties.SetEnabled("FromPortId", this.ObjectTableName, !this.EntityPM.ConnectedToShipment);
                    this.UIProperties.SetEnabled("ToPortId", this.ObjectTableName, !this.EntityPM.ConnectedToShipment);
                }
            }
        }
    };
    WarehouseEntryRoutingsTabComponent.prototype.Listen = function () {
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
    WarehouseEntryRoutingsTabComponent.prototype.IsInlandDomesticWarehouse = function (entityPM) {
        var isInlandDomestic = false;
        if (entityPM.DirectionId == "D" && entityPM.TransportModeId == "I") {
            isInlandDomestic = true;
        }
        return isInlandDomestic;
    };
    WarehouseEntryRoutingsTabComponent.prototype.Initialize = function () {
        this.myPortListService = new PortListService_1.PortListService();
        this.myAddressListService = new AddressListService_1.AddressListService();
        this.myCardListService = new CardListService_1.CardListService();
    };
    Object.defineProperty(WarehouseEntryRoutingsTabComponent.prototype, "FromPortId", {
        get: function () { return this.EntityPM.FromPortId; },
        set: function (value) {
            var _this = this;
            if (this.EntityPM.FromPortId != value) {
                this.EntityPM.FromPortId = value;
                this.SetUIProperties_Ports();
                if (Tools_1.AppTool.IsNullOrEmpty(value)) {
                    this.FromPortList = null;
                }
                else {
                    this.myPortListService.getSingle(value).subscribe(function (myResponse) {
                        if (!myResponse.HasError) {
                            _this.FromPortList = myResponse.Result;
                        }
                    });
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(WarehouseEntryRoutingsTabComponent.prototype, "TransportModeId", {
        get: function () { return this.EntityPM.TransportModeId; },
        set: function (newValue) {
            if (this.EntityPM.TransportModeId != newValue) {
                this.EntityPM.TransportModeId = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(WarehouseEntryRoutingsTabComponent.prototype, "TruckerReference", {
        get: function () { return this.EntityPM.TruckerReference; },
        set: function (newValue) {
            if (this.EntityPM.TruckerReference != newValue) {
                this.EntityPM.TruckerReference = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(WarehouseEntryRoutingsTabComponent.prototype, "TruckerId", {
        get: function () { return this.EntityPM.TruckerId; },
        set: function (newValue) {
            if (this.EntityPM.TruckerId != newValue) {
                this.EntityPM.TruckerId = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(WarehouseEntryRoutingsTabComponent.prototype, "FromPartnerId", {
        get: function () {
            return !Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.FromPartnerId) ? this.EntityPM.FromPartnerId : this.EntityPM.ShipperId;
        },
        set: function (value) {
            var _this = this;
            if (this.EntityPM.FromPartnerId != value) {
                this.EntityPM.FromPartnerId = value;
                if (Tools_1.AppTool.IsNullOrEmpty(value)) {
                    this.FromAddressId = null;
                    this.ShipperPartnerTypeId = null;
                }
                else {
                    this.myCardListService.getSingle(value).subscribe(function (myResponse) {
                        if (!myResponse.HasError) {
                            var list = myResponse.Result;
                            if (list) {
                                _this.FromAddressId = list.MainAddressId;
                                _this.ShipperPartnerTypeId = list.PartnerTypeId;
                            }
                        }
                    });
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(WarehouseEntryRoutingsTabComponent.prototype, "FromAddressId", {
        get: function () { return this.EntityPM.FromAddressId; },
        set: function (value) {
            if (this.EntityPM.FromAddressId != value) {
                this.EntityPM.FromAddressId = value;
                this.LoadFromAddress();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(WarehouseEntryRoutingsTabComponent.prototype, "ToPartnerId", {
        get: function () { return !Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.ToPartnerId) ? this.EntityPM.ToPartnerId : this.EntityPM.ConsigneeId; },
        set: function (value) {
            var _this = this;
            if (this.EntityPM.ToPartnerId != value) {
                this.EntityPM.ToPartnerId = value;
                if (Tools_1.AppTool.IsNullOrEmpty(value)) {
                    this.ToAddressId = null;
                    this.ConsigneePartnerTypeId = null;
                }
                else {
                    this.myCardListService.getSingle(value).subscribe(function (myResponse) {
                        if (!myResponse.HasError) {
                            var list = myResponse.Result;
                            if (list) {
                                _this.ToAddressId = list.MainAddressId;
                                _this.ConsigneePartnerTypeId = list.PartnerTypeId;
                            }
                        }
                    });
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(WarehouseEntryRoutingsTabComponent.prototype, "ToAddressId", {
        get: function () { return this.EntityPM.ToAddressId; },
        set: function (value) {
            if (this.EntityPM.ToAddressId != value) {
                this.EntityPM.ToAddressId = value;
                this.LoadToAddress();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(WarehouseEntryRoutingsTabComponent.prototype, "ToPortId", {
        get: function () { return this.EntityPM.ToPortId; },
        set: function (value) {
            var _this = this;
            if (this.EntityPM.ToPortId != value) {
                this.EntityPM.ToPortId = value;
                this.SetUIProperties_Ports();
                if (Tools_1.AppTool.IsNullOrEmpty(value)) {
                    this.ToPortList = null;
                }
                else {
                    this.myPortListService.getSingle(value).subscribe(function (myResponse) {
                        if (!myResponse.HasError) {
                            _this.ToPortList = myResponse.Result;
                        }
                    });
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    WarehouseEntryRoutingsTabComponent.prototype.SetUIProperties_Ports = function () {
        var isFromRequired = false;
        var isToRequired = false;
        var isShipperIdRequired = false;
        var isConsigneeIdRequired = false;
        var isCustomerIdRequired = false;
        if (!this.IsInlandDomestic) {
            if (Tools_1.AppTool.IsNullOrEmpty(this.FromPortId)) {
                isFromRequired = true;
            }
            if (Tools_1.AppTool.IsNullOrEmpty(this.ToPortId)) {
                isToRequired = true;
            }
        }
        this.UIProperties.SetRequired("FromPortId", this.ObjectTableName, isFromRequired);
        this.UIProperties.SetRequired("ToPortId", this.ObjectTableName, isToRequired);
    };
    WarehouseEntryRoutingsTabComponent.prototype.LoadToAddress = function () {
        var _this = this;
        if (Tools_1.AppTool.IsNullOrEmpty(this.ToAddressId)) {
            this.ToAddressList = null;
        }
        else {
            this.myAddressListService.getSingle(this.ToAddressId).subscribe(function (myResponse) {
                if (!myResponse.HasError) {
                    var list = myResponse.Result;
                    if (list) {
                        _this.ToAddressList = list;
                    }
                }
            });
        }
    };
    WarehouseEntryRoutingsTabComponent.prototype.LoadFromAddress = function () {
        var _this = this;
        if (Tools_1.AppTool.IsNullOrEmpty(this.FromAddressId)) {
            this.FromAddressList = null;
        }
        else {
            this.myAddressListService.getSingle(this.FromAddressId).subscribe(function (myResponse) {
                if (!myResponse.HasError) {
                    var list = myResponse.Result;
                    if (list) {
                        _this.FromAddressList = list;
                    }
                }
            });
        }
    };
    WarehouseEntryRoutingsTabComponent.prototype.AddAddressClicked = function (myAddressCode) {
        var _this = this;
        var entityPM = null;
        var myPartnerTypeId = null;
        var isCustomer;
        var IsShipperMyCustomer;
        if (this.EntityPM.DirectionId == "I") {
            IsShipperMyCustomer = false;
        }
        else {
            IsShipperMyCustomer = true;
        }
        switch (myAddressCode) {
            case "F": {
                entityPM = new AddressPM_1.AddressPM();
                entityPM.Tenant = SessionLocator_1.SessionLocator.Tenant;
                entityPM.AddressTypeId = "O";
                entityPM.CardId = this.FromPartnerId;
                myPartnerTypeId = this.ShipperPartnerTypeId;
                isCustomer = IsShipperMyCustomer;
                break;
            }
            case "T": {
                entityPM = new AddressPM_1.AddressPM();
                entityPM.Tenant = SessionLocator_1.SessionLocator.Tenant;
                entityPM.AddressTypeId = "O";
                entityPM.CardId = this.ToPartnerId;
                myPartnerTypeId = this.ConsigneePartnerTypeId;
                isCustomer = !IsShipperMyCustomer;
                break;
            }
        }
        if (entityPM != null) {
            var logeWindow = new LogitudeWindow_1.LogitudeWindow();
            logeWindow.Width = 630;
            logeWindow.Height = 430;
            logeWindow.Title = "Add Address";
            logeWindow.WindowArgs = { EntityPM: entityPM, PartnerTypeId: myPartnerTypeId, IsCustomer: isCustomer };
            logeWindow.Show("./Shipment/Components/NewEntity/WizardAddEditAddressComponent");
            logeWindow.WindowClosed.subscribe(function (s) {
                if (s) {
                    switch (myAddressCode) {
                        case "F": {
                            _this.FromAddressId = null;
                            _this.FromAddressId = entityPM.Id;
                            break;
                        }
                        case "T": {
                            _this.ToAddressId = null;
                            _this.ToAddressId = entityPM.Id;
                            break;
                        }
                    }
                }
            });
        }
    };
    WarehouseEntryRoutingsTabComponent.prototype.EditAddressClicked = function (myAddressCode) {
        var _this = this;
        var myAddressId = null;
        var myPartnerTypeId = null;
        var isCustomer;
        var isShipperMyCustomer;
        if (this.EntityPM.DirectionId == "I") {
            isShipperMyCustomer = false;
        }
        else {
            isShipperMyCustomer = true;
        }
        switch (myAddressCode) {
            case "F": {
                myAddressId = this.FromAddressId;
                myPartnerTypeId = this.ShipperPartnerTypeId;
                isCustomer = isShipperMyCustomer;
                break;
            }
            case "T": {
                myAddressId = this.ToAddressId;
                myPartnerTypeId = this.ConsigneePartnerTypeId;
                isCustomer = !isShipperMyCustomer;
                break;
            }
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(myAddressId)) {
            var logeWindow = new LogitudeWindow_1.LogitudeWindow();
            logeWindow.Width = 630;
            logeWindow.Height = 430;
            logeWindow.Title = "Edit Address";
            logeWindow.WindowArgs = { EntityId: myAddressId, PartnerTypeId: myPartnerTypeId, IsCustomer: isCustomer };
            logeWindow.Show("./Shipment/Components/NewEntity/WizardAddEditAddressComponent");
            logeWindow.WindowClosed.subscribe(function (s) {
                if (s) {
                    switch (myAddressCode) {
                        case "F": {
                            _this.FromAddressId = null;
                            _this.FromAddressId = myAddressId;
                            break;
                        }
                        case "T": {
                            _this.ToAddressId = null;
                            _this.ToAddressId = myAddressId;
                            break;
                        }
                    }
                }
            });
        }
    };
    WarehouseEntryRoutingsTabComponent = __decorate([
        core_1.Component({
            selector: 'WarehouseEntryRoutingsTabComponent',
            moduleId: module.id,
            templateUrl: './WarehouseEntryRoutingsTabComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs])
    ], WarehouseEntryRoutingsTabComponent);
    return WarehouseEntryRoutingsTabComponent;
}(BaseComponent_1.BaseComponent));
exports.WarehouseEntryRoutingsTabComponent = WarehouseEntryRoutingsTabComponent;
//# sourceMappingURL=WarehouseEntryRoutingsTabComponent.js.map