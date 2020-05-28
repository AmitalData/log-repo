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
var BaseComponent_1 = require("../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var Tools_1 = require("../../../../../Infrastructure/Tools");
var Tools_2 = require("../../../../../Shipment/Tools");
var PortListService_1 = require("../../../../../Common/Services/StandardLists/PortListService");
var AWBHouseRoutingsTabComponent = /** @class */ (function (_super) {
    __extends(AWBHouseRoutingsTabComponent, _super);
    function AWBHouseRoutingsTabComponent() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.IsEditingEnabled = false;
        _this.ShowWarning_House = false;
        if (_this.myPortListService == null) {
            _this.myPortListService = new PortListService_1.PortListService();
        }
        return _this;
    }
    AWBHouseRoutingsTabComponent.prototype.ngAfterViewInit = function () {
        this.SetUIProperties();
    };
    AWBHouseRoutingsTabComponent.prototype.InitTab = function (wizard) {
        this.Wizard = wizard;
        this.EntityPM = this.Wizard.EntityPM;
        this.ObjectTableName = this.Wizard.ObjectTableName;
        this.Listen();
        this.Validate();
        this.SetUIProperties();
    };
    AWBHouseRoutingsTabComponent.prototype.RefreshTab = function () {
        this.Validate();
        this.SetUIProperties();
    };
    AWBHouseRoutingsTabComponent.prototype.Listen = function () {
        var _this = this;
        if (this.Wizard != null) {
            this.Wizard.SaveCompleted.subscribe(function (isSaveSuccess) {
                if (isSaveSuccess) {
                    _this.EntityPM = _this.Wizard.EntityPM;
                    _this.SetUIProperties();
                }
            });
        }
    };
    AWBHouseRoutingsTabComponent.prototype.SetUIProperties = function () {
        this.IsEditingEnabled = Tools_2.ShipmentTool.IsEditingEnabled(this.EntityPM);
        var isPortsEnabled = this.IsEditingEnabled;
        if (isPortsEnabled) {
            if (this.EntityPM.MasterShipmentDataId != null) {
                isPortsEnabled = false;
            }
        }
        if (isPortsEnabled) {
            if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.BookingId)) {
                isPortsEnabled = false;
            }
        }
        this.UIProperties.SetEnabled("House", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("MainCarriageFromPortId", this.ObjectTableName, isPortsEnabled);
        this.UIProperties.SetEnabled("MainCarriageToPortId", this.ObjectTableName, isPortsEnabled);
        this.UIProperties.SetEnabled("ToPortId", this.ObjectTableName, isPortsEnabled);
        this.UIProperties.SetEnabled("HasOnCarriage", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("HasOnCarriage", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("PreCarriageFromPortId", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("OnCarriageToPortId", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetRequired("MainCarriageFromPortId", this.ObjectTableName, Tools_1.AppTool.IsNullOrEmpty(this.MainCarriageFromPortId) ? true : false);
        this.UIProperties.SetRequired("ToPortId", this.ObjectTableName, Tools_1.AppTool.IsNullOrEmpty(this.ToPortId) ? true : false);
        this.UIProperties.SetRequired("MainCarriageToPortId", this.ObjectTableName, Tools_1.AppTool.IsNullOrEmpty(this.ToPortId) ? true : false);
        this.SetUIProperties_PreCarriage();
        this.SetUIProperties_OnCarriage();
    };
    AWBHouseRoutingsTabComponent.prototype.SetUIProperties_PreCarriage = function () {
        var isPreCarriagePortRequired = false;
        if (this.HasPreCarriage && Tools_1.AppTool.IsNullOrEmpty(this.PreCarriageFromPortId)) {
            isPreCarriagePortRequired = true;
        }
        this.UIProperties.SetRequired("PreCarriageFromPortId", this.ObjectTableName, isPreCarriagePortRequired);
        this.FireWizardEvent();
    };
    AWBHouseRoutingsTabComponent.prototype.SetUIProperties_OnCarriage = function () {
        var isOnCarriagePortRequired = false;
        if (this.HasOnCarriage && Tools_1.AppTool.IsNullOrEmpty(this.OnCarriageToPortId)) {
            isOnCarriagePortRequired = true;
        }
        this.UIProperties.SetRequired("OnCarriageToPortId", this.ObjectTableName, isOnCarriagePortRequired);
        this.FireWizardEvent();
    };
    AWBHouseRoutingsTabComponent.prototype.FireWizardEvent = function () {
        this.Wizard.ValidateScreen_ROU();
        this.Wizard.ValidateScreen_GEN();
        this.Wizard.ValidateScreen_PAC();
    };
    AWBHouseRoutingsTabComponent.prototype.Validate = function () {
        if (!this.Wizard.IsImportWizard) {
            this.ShowWarning_House = Tools_1.AppTool.IsNullOrEmpty(this.House) ? true : false;
        }
    };
    AWBHouseRoutingsTabComponent.prototype.AddPort = function (list) {
        if (this.AllPorts == null) {
            this.AllPorts = [];
        }
        if (list != null) {
            if (this.AllPorts.indexOf(list) == -1) {
                this.AllPorts.push(list);
            }
        }
    };
    Object.defineProperty(AWBHouseRoutingsTabComponent.prototype, "HasPreCarriage", {
        get: function () { return this.EntityPM.HasPreCarriage; },
        set: function (newValue) {
            if (this.EntityPM.HasPreCarriage != newValue) {
                this.EntityPM.HasPreCarriage = newValue;
                this.SetPreCarriage();
                this.SetUIProperties_PreCarriage();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AWBHouseRoutingsTabComponent.prototype, "PreCarriageFromPortId", {
        get: function () { return this.EntityPM.PreCarriageFromPortId; },
        set: function (newValue) {
            if (this.EntityPM.PreCarriageFromPortId != newValue) {
                this.EntityPM.PreCarriageFromPortId = newValue;
                this.SetUIProperties_PreCarriage();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AWBHouseRoutingsTabComponent.prototype, "PreCarriageFromPort", {
        get: function () { return this.preCarriageFromPort; },
        set: function (list) {
            if (this.preCarriageFromPort != list) {
                this.preCarriageFromPort = list;
                this.AddPort(list);
                var Code = list == null ? null : list.Code;
                if (Code != this.EntityPM.PreCarriageFromPortCode) {
                    if (list == null) {
                        this.EntityPM.PreCarriageFromPortCode = null;
                        this.EntityPM.PreCarriageFromPortName = null;
                        this.EntityPM.PreCarriageFromPortCountryCode = null;
                        this.EntityPM.PreCarriageFromPortCountryName = null;
                    }
                    else {
                        this.EntityPM.PreCarriageFromPortCode = list.Code;
                        this.EntityPM.PreCarriageFromPortName = list.EnglishName;
                        this.EntityPM.PreCarriageFromPortCountryCode = list.CountryCode;
                        this.EntityPM.PreCarriageFromPortCountryName = list.CountryName;
                    }
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AWBHouseRoutingsTabComponent.prototype, "MainCarriageFromPortId", {
        get: function () { return this.EntityPM.MainCarriageFromPortId; },
        set: function (newValue) {
            if (this.EntityPM.MainCarriageFromPortId != newValue) {
                this.EntityPM.FromPortId = newValue;
                this.EntityPM.MainCarriageFromPortId = newValue;
                this.SetPreCarriage();
                this.FireWizardEvent();
                this.SetUIProperties();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AWBHouseRoutingsTabComponent.prototype, "MainCarriageFromPort", {
        get: function () { return this.mainCarriageFromPort; },
        set: function (list) {
            if (this.mainCarriageFromPort != list) {
                this.mainCarriageFromPort = list;
                this.AddPort(list);
                var Code = list == null ? null : list.Code;
                if (Code != this.EntityPM.MainCarriageFromPortCode) {
                    if (list == null) {
                        this.EntityPM.FromCountryId = null;
                        this.EntityPM.FromCountryIsEC = false;
                        this.EntityPM.MainCarriageFromPortCode = null;
                        this.EntityPM.MainCarriageFromPortName = null;
                        this.EntityPM.MainCarriageFromPortCountryCode = null;
                        this.EntityPM.MainCarriageFromPortCountryName = null;
                        Tools_2.ShipmentTool.ComputeSCI(this.EntityPM);
                        Tools_2.ShipmentTool.BuildAWBPlaceField(this.EntityPM);
                    }
                    else {
                        this.EntityPM.FromCountryId = list.CountryId;
                        this.EntityPM.FromCountryIsEC = list.CountryEC;
                        this.EntityPM.MainCarriageFromPortCode = list.Code;
                        this.EntityPM.MainCarriageFromPortName = list.EnglishName;
                        this.EntityPM.MainCarriageFromPortCountryCode = list.CountryCode;
                        this.EntityPM.MainCarriageFromPortCountryName = list.CountryName;
                        Tools_2.ShipmentTool.ComputeSCI(this.EntityPM);
                        Tools_2.ShipmentTool.BuildAWBPlaceField(this.EntityPM);
                    }
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AWBHouseRoutingsTabComponent.prototype, "ToPortId", {
        get: function () { return this.EntityPM.ToPortId; },
        set: function (value) {
            if (this.EntityPM.ToPortId != value) {
                this.EntityPM.ToPortId = value;
                this.EntityPM.MainCarriageToPortId = value;
                this.EntityPM.MainCarriageFinalDestinationPortId = value;
                this.SetOnCarriage();
                this.FireWizardEvent();
                this.SetUIProperties();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AWBHouseRoutingsTabComponent.prototype, "ToPort", {
        get: function () { return this.toPort; },
        set: function (list) {
            if (this.toPort != list) {
                this.toPort = list;
                this.AddPort(list);
                if (list == null) {
                    this.EntityPM.ToCountryId = null;
                    this.EntityPM.ToCountryIsEC = false;
                    this.EntityPM.FinalDistenationPortId = null;
                    Tools_2.ShipmentTool.ComputeSCI(this.EntityPM);
                }
                else {
                    this.EntityPM.ToCountryId = list.CountryId;
                    this.EntityPM.ToCountryIsEC = list.CountryEC;
                    this.EntityPM.FinalDistenationPortId = list.Id;
                    Tools_2.ShipmentTool.ComputeSCI(this.EntityPM);
                }
                //var Code = list == null ? null : list.Code;
                //if (Code != this.EntityPM.MainCarriageToPortCode) {
                //    if (list == null) {
                //        this.EntityPM.MainCarriageToPortCode = null;
                //        this.EntityPM.MainCarriageToPortName = null;
                //        this.EntityPM.MainCarriageToPortCountryCode = null;
                //        this.EntityPM.MainCarriageToPortCountryName = null;
                //        //this.BuildLegs();
                //    }
                //    else {
                //        this.EntityPM.MainCarriageToPortCode = list.Code;
                //        this.EntityPM.MainCarriageToPortName = list.EnglishName;
                //        this.EntityPM.MainCarriageToPortCountryCode = list.CountryCode;
                //        this.EntityPM.MainCarriageToPortCountryName = list.CountryName;
                //        //this.BuildLegs();
                //    }
                //}
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AWBHouseRoutingsTabComponent.prototype, "HasOnCarriage", {
        //get MainCarriageToPortId() { return this.EntityPM.MainCarriageToPortId; }
        //set MainCarriageToPortId(newValue: string) {
        //    if (this.EntityPM.MainCarriageToPortId != newValue) {
        //        this.EntityPM.ToPortId = newValue;
        //        this.EntityPM.MainCarriageToPortId = newValue;
        //        this.EntityPM.MainCarriageFinalDestinationPortId = newValue;
        //        this.SetOnCarriage();            
        //        this.FireWizardEvent();
        //        this.SetUIProperties();
        //    }
        //}
        //private mainCarriageToPort: PortList;
        //get MainCarriageToPort() { return this.mainCarriageToPort; }
        //set MainCarriageToPort(list: PortList) {
        //    if (this.mainCarriageToPort != list) {
        //        this.mainCarriageToPort = list;
        //        this.AddPort(list);
        //        var Code = list == null ? null : list.Code;
        //        if (Code != this.EntityPM.MainCarriageToPortCode) {
        //            if (list == null) {
        //                this.EntityPM.MainCarriageToPortCode = null;
        //                this.EntityPM.MainCarriageToPortName = null;
        //                this.EntityPM.MainCarriageToPortCountryCode = null;
        //                this.EntityPM.MainCarriageToPortCountryName = null;
        //                //this.BuildLegs();
        //            }
        //            else {
        //                this.EntityPM.MainCarriageToPortCode = list.Code;
        //                this.EntityPM.MainCarriageToPortName = list.EnglishName;
        //                this.EntityPM.MainCarriageToPortCountryCode = list.CountryCode;
        //                this.EntityPM.MainCarriageToPortCountryName = list.CountryName;
        //                //this.BuildLegs();
        //            }
        //        }
        //    }
        //}
        get: function () { return this.EntityPM.HasOnCarriage; },
        set: function (newValue) {
            if (this.EntityPM.HasOnCarriage != newValue) {
                this.EntityPM.HasOnCarriage = newValue;
                this.SetOnCarriage();
                this.SetUIProperties_OnCarriage();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AWBHouseRoutingsTabComponent.prototype, "OnCarriageToPortId", {
        get: function () { return this.EntityPM.OnCarriageToPortId; },
        set: function (newValue) {
            if (this.EntityPM.OnCarriageToPortId != newValue) {
                this.EntityPM.OnCarriageToPortId = newValue;
                this.SetUIProperties_OnCarriage();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AWBHouseRoutingsTabComponent.prototype, "OnCarriageToPort", {
        get: function () { return this.onCarriageToPort; },
        set: function (list) {
            if (this.onCarriageToPort != list) {
                this.onCarriageToPort = list;
                this.AddPort(list);
                var Code = list == null ? null : list.Code;
                if (Code != this.EntityPM.OnCarriageToPortCode) {
                    if (list == null) {
                        this.EntityPM.OnCarriageToPortCode = null;
                        this.EntityPM.OnCarriageToPortName = null;
                        this.EntityPM.OnCarriageToPortCountryCode = null;
                        this.EntityPM.OnCarriageToPortCountryName = null;
                    }
                    else {
                        this.EntityPM.OnCarriageToPortCode = list.Code;
                        this.EntityPM.OnCarriageToPortName = list.EnglishName;
                        this.EntityPM.OnCarriageToPortCountryCode = list.CountryCode;
                        this.EntityPM.OnCarriageToPortCountryName = list.CountryName;
                    }
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    AWBHouseRoutingsTabComponent.prototype.SetPreCarriage = function () {
        if (this.HasPreCarriage) {
            this.EntityPM.PreCarriageTransportModeId = "A";
            this.EntityPM.PreCarriageToPortId = this.EntityPM.FromPortId;
            this.EntityPM.PreCarriageToPortCode = this.EntityPM.MainCarriageFromPortCode;
            this.EntityPM.PreCarriageToPortName = this.EntityPM.MainCarriageFromPortName;
            this.EntityPM.PreCarriageToPortCountryCode = this.EntityPM.MainCarriageFromPortCountryCode;
            this.EntityPM.PreCarriageToPortCountryName = this.EntityPM.MainCarriageFromPortCountryName;
        }
        else {
            Tools_2.RoutingHelper.RemovePreCarriageLeg(this.EntityPM);
        }
    };
    AWBHouseRoutingsTabComponent.prototype.SetOnCarriage = function () {
        if (this.HasOnCarriage) {
            this.EntityPM.OnCarriageTransportModeId = "A";
            this.EntityPM.OnCarriageFromPortId = this.EntityPM.ToPortId;
            this.EntityPM.OnCarriageFromPortCode = this.EntityPM.MainCarriageToPortCode;
            this.EntityPM.OnCarriageFromPortName = this.EntityPM.MainCarriageToPortName;
            this.EntityPM.OnCarriageFromPortCountryCode = this.EntityPM.MainCarriageToPortCountryCode;
            this.EntityPM.OnCarriageFromPortCountryName = this.EntityPM.MainCarriageToPortCountryName;
        }
        else {
            Tools_2.RoutingHelper.RemoveOnCarriageLeg(this.EntityPM);
        }
    };
    Object.defineProperty(AWBHouseRoutingsTabComponent.prototype, "House", {
        get: function () { return this.EntityPM.House; },
        set: function (newValue) {
            if (this.EntityPM.House != newValue) {
                this.EntityPM.House = newValue;
                this.Validate();
                this.FireWizardEvent();
            }
        },
        enumerable: true,
        configurable: true
    });
    AWBHouseRoutingsTabComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'AWBHouseRoutingsTabComponent',
            templateUrl: './AWBHouseRoutingsTabComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], AWBHouseRoutingsTabComponent);
    return AWBHouseRoutingsTabComponent;
}(BaseComponent_1.BaseComponent));
exports.AWBHouseRoutingsTabComponent = AWBHouseRoutingsTabComponent;
//# sourceMappingURL=AWBHouseRoutingsTabComponent.js.map