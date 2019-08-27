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
var EntityArgs_1 = require("../../../../Infrastructure/DataContracts/EntityArgs");
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var Tools_1 = require("../../../../Shipment/Tools");
var Tools_2 = require("../../../../Infrastructure/Tools");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var FeatureLocator_1 = require("../../../../Infrastructure/Utilities/FeatureLocator");
var ObjectsLocator_1 = require("../../../../Infrastructure/Locators/ObjectsLocator");
var ShipmentDomainService_1 = require("../../../../Shipment/Services/ShipmentDomainService");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var CustomsTabComponent = /** @class */ (function (_super) {
    __extends(CustomsTabComponent, _super);
    function CustomsTabComponent(entityArgs) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this.EntityPM = null;
        _this.ObjectTableName = null;
        _this.DataContext = _this;
        _this.IsSendToAESButtonVisible = false;
        _this.DeclarationNumberLabel = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.F.DeclarationNumber");
        _this.DeclarationDateLabel = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.F.DeclarationDate");
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        // Additional Fields
        _this.Retries = 0;
        _this.IsTenantUS = false;
        _this.IsOceanOrAir = false;
        _this.IsOceanImport = false;
        _this.IsDirectOrHouse = false;
        _this.IsDirectOrMaster = false;
        _this.IsExport = false;
        _this.SaveCompletedEvent = null;
        _this.LoadCompletedEvent = null;
        _this.SessionEvent = null;
        _this.IsCustomFollowUpEnabled = false;
        _this.IsEditingEnabled = false;
        //Summary
        _this.SummatyAreaIsVisible = false;
        _this.ShipmentCustomsTransmissionList = [];
        _this.EntityPM = _this.entityArgs.EntityPM;
        _this.ObjectTableName = _this.entityArgs.ObjectTableName;
        _this.SetFlags();
        _this.Listen();
        _this.LoadSummaryData();
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession(_this.ObjectTableName, "SENDTOAES")) {
            _this.IsSendToAESButtonVisible = true;
        }
        if (SessionLocator_1.SessionLocator.TenantPM.CountryCode == "US") {
            _this.DeclarationNumberLabel = "Entry Summary";
            _this.DeclarationDateLabel = "Entry Summary Date";
        }
        _this.BuildAdditionalFields();
        return _this;
    }
    CustomsTabComponent.prototype.BuildAdditionalFields = function () {
        this.RunComponent();
    };
    CustomsTabComponent.prototype.RunComponent = function () {
        if (this.viewContainerRef) {
            this.LoadChildComponent();
        }
        else {
            this.RunComponentTimer();
        }
    };
    CustomsTabComponent.prototype.RunComponentTimer = function () {
        var _this = this;
        this.Retries++;
        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }
        if (this.Retries < 3) {
            this.timerToken = setTimeout(function () { return _this.RunComponent(); }, 1);
        }
    };
    CustomsTabComponent.prototype.LoadChildComponent = function () {
        var _this = this;
        SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/GenericComponents/GeneratedComponent', this.viewContainerRef)
            .then(function (cmpRef) {
            //this.GeneratedComponent = cmpRef.instance;
            cmpRef.instance.LoadCompleted.subscribe(function (s) {
            });
            var screenCode = "Shipment.CustomsAdditionalFields";
            //cmpRef.instance.LabelWidth = 110;
            cmpRef.instance.Run(_this.EntityPM, _this.ObjectTableName, screenCode);
        });
    };
    CustomsTabComponent.prototype.SetFlags = function () {
        if (SessionLocator_1.SessionLocator.TenantPM.CountryCode) {
            if (SessionLocator_1.SessionLocator.TenantPM.CountryCode.toUpperCase() == "US") {
                this.IsTenantUS = true;
            }
        }
        this.IsOceanOrAir = this.EntityPM.TransportModeId == "O" || this.EntityPM.TransportModeId == "A" ? true : false;
        this.IsOceanImport = this.EntityPM.TransportModeId == "O" && this.EntityPM.DirectionId == "I" ? true : false;
        this.IsExport = this.EntityPM.DirectionId == "E" ? true : false;
        this.SetSummaryAreaFlags();
        switch (this.EntityPM.ShipmentLevelCode) {
            case "H": {
                this.IsDirectOrHouse = true;
                break;
            }
            case "C": {
                this.IsDirectOrMaster = true;
                break;
            }
            case "D": {
                this.IsDirectOrHouse = true;
                this.IsDirectOrMaster = true;
                break;
            }
        }
    };
    CustomsTabComponent.prototype.Listen = function () {
        var _this = this;
        if (this.entityArgs.EditComponent) {
            this.SessionEvent = this.CurrentSession.SessionEvent.subscribe(function (s) {
                if (s == "RefreshCustomsSummary") {
                    _this.LoadSummaryData();
                }
            });
            this.SaveCompletedEvent = this.entityArgs.EditComponent.SaveCompleted.subscribe(function (isSaveSuccess) {
                if (isSaveSuccess) {
                    _this.EntityPM = _this.entityArgs.EditComponent.EntityPM;
                    _this.SetUIProperties();
                    _this.LoadSummaryData();
                }
            });
            this.LoadCompletedEvent = this.entityArgs.EditComponent.LoadCompleted.subscribe(function (isLoadSuccess) {
                if (isLoadSuccess) {
                    _this.EntityPM = _this.entityArgs.EditComponent.EntityPM;
                    _this.SetUIProperties();
                    _this.LoadSummaryData();
                }
            });
        }
    };
    CustomsTabComponent.prototype.ngOnDestroy = function () {
        if (this.SaveCompletedEvent) {
            this.SaveCompletedEvent.unsubscribe();
            this.SaveCompletedEvent = null;
        }
        if (this.LoadCompletedEvent) {
            this.LoadCompletedEvent.unsubscribe();
            this.LoadCompletedEvent = null;
        }
        if (this.SessionEvent) {
            this.SessionEvent.unsubscribe();
            this.SessionEvent = null;
        }
    };
    CustomsTabComponent.prototype.ngOnInit = function () {
        if (this.EntityPM != null) {
            this.SetUIProperties();
        }
    };
    CustomsTabComponent.prototype.SetUIProperties = function () {
        var isEditingEnabled = Tools_1.ShipmentTool.IsEditingEnabled(this.EntityPM);
        var isIncludeFieldsEnabled = false;
        if (isEditingEnabled) {
            if (this.IncludesCustoms) {
                isIncludeFieldsEnabled = true;
            }
        }
        this.UIProperties.SetEnabled("IncludesCustoms", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled("DeclarationNumber", this.ObjectTableName, isIncludeFieldsEnabled);
        this.UIProperties.SetEnabled("DeclarationDate", this.ObjectTableName, isIncludeFieldsEnabled);
        this.UIProperties.SetEnabled("CustomsClearanceDate", this.ObjectTableName, isIncludeFieldsEnabled);
        this.UIProperties.SetEnabled("FreightRelease", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled("TerminalAvailable", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled("ISFNumber", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled("ISFDate", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled("ITNumber", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled("ITDate", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled("ENSNumber", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled("ENSDate", this.ObjectTableName, isEditingEnabled);
        if (!Tools_2.AppTool.IsNullOrEmpty(this.DeclarationNumber)) {
            this.UIProperties.SetRequired("DeclarationDate", this.ObjectTableName, this.DeclarationDate == null);
        }
        else {
            this.UIProperties.SetRequired("DeclarationDate", this.ObjectTableName, false);
        }
        this.IsCustomFollowUpEnabled = isIncludeFieldsEnabled;
        this.IsEditingEnabled = isEditingEnabled;
    };
    Object.defineProperty(CustomsTabComponent.prototype, "IncludesCustoms", {
        get: function () { return this.EntityPM.IncludesCustoms; },
        set: function (newValue) {
            if (this.EntityPM.IncludesCustoms != newValue) {
                this.EntityPM.IncludesCustoms = newValue;
                if (!newValue) {
                    this.DeclarationNumber = null;
                    this.DeclarationDate = null;
                    this.CustomsClearanceDate = null;
                }
                this.SetUIProperties();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomsTabComponent.prototype, "DeclarationNumber", {
        get: function () { return this.EntityPM.DeclarationNumber; },
        set: function (newValue) {
            if (this.EntityPM.DeclarationNumber != newValue) {
                this.EntityPM.DeclarationNumber = newValue;
                this.SetUIProperties();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomsTabComponent.prototype, "DeclarationDate", {
        get: function () { return this.EntityPM.DeclarationDate; },
        set: function (newValue) {
            if (this.EntityPM.DeclarationDate != newValue) {
                this.EntityPM.DeclarationDate = newValue;
                this.SetUIProperties();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomsTabComponent.prototype, "CustomsClearanceDate", {
        get: function () { return this.EntityPM.CustomsClearanceDate; },
        set: function (newValue) {
            if (this.EntityPM.CustomsClearanceDate != newValue) {
                this.EntityPM.CustomsClearanceDate = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomsTabComponent.prototype, "FreightRelease", {
        get: function () { return this.EntityPM.FreightRelease; },
        set: function (value) {
            if (this.EntityPM.FreightRelease != value) {
                this.EntityPM.FreightRelease = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomsTabComponent.prototype, "TerminalAvailable", {
        get: function () { return this.EntityPM.TerminalAvailable; },
        set: function (value) {
            if (this.EntityPM.TerminalAvailable != value) {
                this.EntityPM.TerminalAvailable = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomsTabComponent.prototype, "ISFNumber", {
        get: function () { return this.EntityPM.ISFNumber; },
        set: function (value) {
            if (this.EntityPM.ISFNumber != value) {
                this.EntityPM.ISFNumber = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomsTabComponent.prototype, "ISFDate", {
        get: function () { return this.EntityPM.ISFDate; },
        set: function (value) {
            if (this.EntityPM.ISFDate != value) {
                this.EntityPM.ISFDate = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomsTabComponent.prototype, "ITNumber", {
        get: function () { return this.EntityPM.ITNumber; },
        set: function (value) {
            if (this.EntityPM.ITNumber != value) {
                this.EntityPM.ITNumber = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomsTabComponent.prototype, "ITDate", {
        get: function () { return this.EntityPM.ITDate; },
        set: function (value) {
            if (this.EntityPM.ITDate != value) {
                this.EntityPM.ITDate = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomsTabComponent.prototype, "ENSNumber", {
        get: function () { return this.EntityPM.ENSNumber; },
        set: function (value) {
            if (this.EntityPM.ENSNumber != value) {
                this.EntityPM.ENSNumber = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomsTabComponent.prototype, "ENSDate", {
        get: function () { return this.EntityPM.ENSDate; },
        set: function (value) {
            if (this.EntityPM.ENSDate != value) {
                this.EntityPM.ENSDate = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    CustomsTabComponent.prototype.SetSummaryAreaFlags = function () {
        this.SummatyAreaIsVisible = this.CheckSummaryAreaVisibility();
    };
    CustomsTabComponent.prototype.CheckSummaryAreaVisibility = function () {
        var visible = true;
        if ((ObjectsLocator_1.ObjectsLocator.CustomsInterfaceSettingPM.LocalCustomsInterfaceCode == null || ObjectsLocator_1.ObjectsLocator.CustomsInterfaceSettingPM.LocalCustomsInterfaceCode == "NO")
            &&
                (ObjectsLocator_1.ObjectsLocator.CustomsInterfaceSettingPM.ImportToUSAInterfaceCode == null || ObjectsLocator_1.ObjectsLocator.CustomsInterfaceSettingPM.ImportToUSAInterfaceCode == "NO")
            &&
                (ObjectsLocator_1.ObjectsLocator.CustomsInterfaceSettingPM.ExportFromUSAInterfaceCode == null || ObjectsLocator_1.ObjectsLocator.CustomsInterfaceSettingPM.ExportFromUSAInterfaceCode == "NO")) {
            visible = false;
        }
        return visible;
    };
    CustomsTabComponent.prototype.CheckLocalVisibility = function () {
        var visible = false;
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("Shipment", "SendToCustoms")) {
            if (this.EntityPM.ShipmentLevelCode != "C") {
                if (ObjectsLocator_1.ObjectsLocator.CustomsInterfaceSettingPM != null) {
                    if (ObjectsLocator_1.ObjectsLocator.CustomsInterfaceSettingPM.LocalCustomsInterfaceCode == "ABM") {
                        visible = true;
                    }
                }
            }
        }
        return visible;
    };
    CustomsTabComponent.prototype.CheckArtemusVisibility_BOL = function () {
        var visible = false;
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("Shipment", "SendToArtemus")) {
            if (this.EntityPM.TransportModeId == "O" && this.EntityPM.DirectionId == "I" && (this.EntityPM.ShipmentLevelCode == "D" || this.EntityPM.ShipmentLevelCode == "H")) {
                if (ObjectsLocator_1.ObjectsLocator.CustomsInterfaceSettingPM.ImportToUSAInterfaceCode == "ART") {
                    visible = true;
                }
            }
        }
        return visible;
    };
    CustomsTabComponent.prototype.CheckArtemusVisibility_VOG = function () {
        var visible = false;
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("Shipment", "SendToArtemus")) {
            if (this.EntityPM.TransportModeId == "O" && this.EntityPM.DirectionId == "I" && (this.EntityPM.ShipmentLevelCode == "D" || this.EntityPM.ShipmentLevelCode == "C")) {
                if (ObjectsLocator_1.ObjectsLocator.CustomsInterfaceSettingPM.ImportToUSAInterfaceCode == "ART") {
                    visible = true;
                }
            }
        }
        return visible;
    };
    CustomsTabComponent.prototype.CheckAESVisibility = function () {
        var visible = false;
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("Shipment", "SENDTOAES")) {
            if (ObjectsLocator_1.ObjectsLocator.CustomsInterfaceSettingPM != null) {
                if (ObjectsLocator_1.ObjectsLocator.CustomsInterfaceSettingPM.ExportFromUSAInterfaceCode == "CBP") {
                    if (this.EntityPM.DirectionId == "E") {
                        visible = true;
                    }
                }
            }
        }
        return visible;
    };
    CustomsTabComponent.prototype.LoadSummaryData = function () {
        var _this = this;
        var myShipmentDomainService = new ShipmentDomainService_1.ShipmentDomainService();
        myShipmentDomainService.GetShipmentCustomsTransmissionByShipmnetId(this.EntityPM.Id).subscribe(function (myResponse) {
            if (!myResponse.HasError) {
                _this.ShipmentCustomsTransmissionList = myResponse.Result;
                _this.FillSummaryItemsSource();
            }
        });
    };
    CustomsTabComponent.prototype.FillSummaryItemsSource = function () {
        var _this = this;
        this.SummaryItemsSource = [];
        var notSent = "Not Sent";
        var isLocalVisible = this.CheckLocalVisibility();
        var isBOLVisible = this.CheckArtemusVisibility_BOL();
        var isVOGVisible = this.CheckArtemusVisibility_VOG();
        var isAESVisible = this.CheckAESVisibility();
        if (isLocalVisible) {
            var newItem = new SummaryItem();
            newItem.CustomsInterfaceName = "ABM Customsware";
            newItem.Code = "ABM";
            newItem.StatusName = !Tools_2.AppTool.IsNullOrEmpty(this.EntityPM.LocalCustomsTransmissionsStatusName) ? this.EntityPM.LocalCustomsTransmissionsStatusName : notSent;
            newItem.StatusDate = this.EntityPM.LocalCustomsTransmissionsStatusDate;
            newItem.StatusCode = this.EntityPM.LocalCustomsTransmissionsStatusCode;
            newItem.UserName = this.EntityPM.LocalCustomsSentByUserName;
            newItem.Error = this.EntityPM.LocalCustomsTransmissionsStatusError;
            newItem.IsSeparatorVisible = false;
            this.SummaryItemsSource.push(newItem);
        }
        if (isVOGVisible) {
            var newItem0 = new SummaryItem();
            newItem0.StatusName = notSent;
            newItem0.Code = "ASVO";
            newItem0.CustomsInterfaceName = "Artemus Voyage";
            newItem0.IsSeparatorVisible = true;
            this.SummaryItemsSource.push(newItem0);
        }
        if (isBOLVisible) {
            var newItem1 = new SummaryItem();
            newItem1.StatusName = notSent;
            newItem1.Code = "ARBL";
            newItem1.CustomsInterfaceName = "Artemus Bill of Lading";
            newItem1.IsSeparatorVisible = true;
            this.SummaryItemsSource.push(newItem1);
        }
        if (isAESVisible) {
            var newItem2 = new SummaryItem();
            newItem2.StatusName = notSent;
            newItem2.Code = "CBAS";
            newItem2.CustomsInterfaceName = "CBP AES";
            newItem2.IsSeparatorVisible = true;
            this.SummaryItemsSource.push(newItem2);
        }
        if (this.ShipmentCustomsTransmissionList.length > 0) {
            this.SummaryItemsSource.forEach(function (summaryItem) {
                _this.ShipmentCustomsTransmissionList.forEach(function (item) {
                    if (item.MessageCode == summaryItem.Code) {
                        summaryItem.StatusName = !Tools_2.AppTool.IsNullOrEmpty(item.StatusName) ? item.StatusName : notSent;
                        summaryItem.StatusDate = item.LastSendDate;
                        summaryItem.StatusCode = item.Status;
                        summaryItem.UserName = item.ByUserName;
                        summaryItem.Error = item.Error;
                    }
                });
            });
        }
    };
    __decorate([
        core_1.ViewChild('Child', { read: core_1.ViewContainerRef }),
        __metadata("design:type", core_1.ViewContainerRef)
    ], CustomsTabComponent.prototype, "viewContainerRef", void 0);
    CustomsTabComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './CustomsTabComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs])
    ], CustomsTabComponent);
    return CustomsTabComponent;
}(BaseComponent_1.BaseComponent));
exports.CustomsTabComponent = CustomsTabComponent;
var SummaryItem = /** @class */ (function () {
    function SummaryItem() {
    }
    return SummaryItem;
}());
exports.SummaryItem = SummaryItem;
//# sourceMappingURL=CustomsTabComponent.js.map