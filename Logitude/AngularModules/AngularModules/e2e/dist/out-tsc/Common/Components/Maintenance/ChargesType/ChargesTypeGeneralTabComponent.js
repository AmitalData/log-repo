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
var Tools_1 = require("../../../../Infrastructure/Tools");
var EntityArgs_1 = require("../../../../Infrastructure/DataContracts/EntityArgs");
var InfraSettings_1 = require("../../../../Infrastructure/Utilities/InfraSettings");
var ChargesGroupListService_1 = require("../../../../Infrastructure/Services/StandardLists/ChargesGroupListService");
var ObjectsLocator_1 = require("../../../../Infrastructure/Locators/ObjectsLocator");
var ChargesTypeGeneralTabComponent = /** @class */ (function (_super) {
    __extends(ChargesTypeGeneralTabComponent, _super);
    function ChargesTypeGeneralTabComponent(entityArgs) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this.DataContext = _this;
        _this.ObjectTableName = "ChargesType";
        _this.DisplaySATSettings = false;
        _this.CustomsFieldsIsVisible = false;
        _this.EntityPM = _this.entityArgs.EntityPM;
        if (SessionLocator_1.SessionLocator.SATInterfaceSettings.SATInterfaceCode == "PROF33") {
            _this.DisplaySATSettings = true;
        }
        return _this;
    }
    ChargesTypeGeneralTabComponent.prototype.ngOnInit = function () {
        if (this.EntityPM != null) {
            this.SetUIProperties();
            this.CheckWarnings();
        }
    };
    ChargesTypeGeneralTabComponent.prototype.SetUIProperties = function () {
        var fieldsEnabled = true;
        var awbFieldsEnabled = true;
        if (InfraSettings_1.InfraSettings.TenantPM.Id == 65) {
            if (!SessionLocator_1.SessionLocator.LoggedUserPM.IsCustomerCare) {
                fieldsEnabled = false;
                awbFieldsEnabled = false;
            }
        }
        else {
            if (!this.IsAir || (this.ChargesGroupCode == "FRT")) {
                awbFieldsEnabled = false;
            }
        }
        if (ObjectsLocator_1.ObjectsLocator.CustomsInterfaceSettingPM != null) {
            if (ObjectsLocator_1.ObjectsLocator.CustomsInterfaceSettingPM.ActivateCustomsManagementInShipments) {
                this.CustomsFieldsIsVisible = true;
            }
        }
        //this.UIProperties.SetEnabled("Code", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("EnglishName", this.ObjectTableName, fieldsEnabled);
        this.UIProperties.SetEnabled("LocalName", this.ObjectTableName, fieldsEnabled);
        this.UIProperties.SetEnabled("ChargesGroupId", this.ObjectTableName, fieldsEnabled);
        this.UIProperties.SetEnabled("MeasurementCode", this.ObjectTableName, fieldsEnabled);
        this.UIProperties.SetEnabled("ContainerMeasurementId", this.ObjectTableName, fieldsEnabled);
        this.UIProperties.SetEnabled("VatTypeId", this.ObjectTableName, fieldsEnabled);
        this.UIProperties.SetEnabled("ViewOrder", this.ObjectTableName, fieldsEnabled);
        this.UIProperties.SetEnabled("Description", this.ObjectTableName, fieldsEnabled);
        this.UIProperties.SetEnabled("IsReceivable", this.ObjectTableName, fieldsEnabled);
        this.UIProperties.SetEnabled("IsPayable", this.ObjectTableName, fieldsEnabled);
        this.UIProperties.SetEnabled("IsCustoms", this.ObjectTableName, fieldsEnabled);
        //this.UIProperties.SetEnabled("IsBackToBack", this.ObjectTableName, fieldsEnabled);
        this.UIProperties.SetEnabled("IsExpense", this.ObjectTableName, fieldsEnabled);
        this.UIProperties.SetEnabled("InActive", this.ObjectTableName, fieldsEnabled);
        this.UIProperties.SetEnabled("IsAir", this.ObjectTableName, fieldsEnabled);
        this.UIProperties.SetEnabled("IsInland", this.ObjectTableName, fieldsEnabled);
        this.UIProperties.SetEnabled("IsOcean", this.ObjectTableName, fieldsEnabled);
        this.UIProperties.SetEnabled("IsAutoDisplayInQuote", this.ObjectTableName, fieldsEnabled);
        this.UIProperties.SetEnabled("IsAutoDisplayInShipment", this.ObjectTableName, fieldsEnabled);
        this.UIProperties.SetEnabled("IsAutoDisplayInConsolidation", this.ObjectTableName, fieldsEnabled);
        this.UIProperties.SetEnabled("IsAutoDisplayInCustoms", this.ObjectTableName, fieldsEnabled);
        this.UIProperties.SetEnabled("DueTypeCode", this.ObjectTableName, awbFieldsEnabled);
        this.UIProperties.SetEnabled("IATACodeId", this.ObjectTableName, awbFieldsEnabled);
        this.UIProperties.SetEnabled("AWBPrintDescription", this.ObjectTableName, awbFieldsEnabled);
        this.UIProperties.SetEnabled("SATExternalId", this.ObjectTableName, fieldsEnabled);
        //if (this.IsBackToBack) {
        //    this.UIProperties.SetEnabled("IsReceivable", this.ObjectTableName, false);
        //}
    };
    ChargesTypeGeneralTabComponent.prototype.CheckWarnings = function () {
        this.ValidationWarningsList = [];
        if (!this.IsAir) {
            this.ValidationWarningsList.push("This Charge Type will not be used in Air Transport Mode");
        }
        if (this.ChargesGroupCode == "FRT") {
            this.ValidationWarningsList.push("Charges that belong to (Freight) group will not be printed on AWB");
        }
    };
    Object.defineProperty(ChargesTypeGeneralTabComponent.prototype, "Code", {
        // Properties
        get: function () { return this.EntityPM.Code; },
        set: function (newValue) {
            if (this.EntityPM.Code != newValue) {
                this.EntityPM.Code = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ChargesTypeGeneralTabComponent.prototype, "EnglishName", {
        get: function () { return this.EntityPM.EnglishName; },
        set: function (newValue) {
            if (this.EntityPM.EnglishName != newValue) {
                this.EntityPM.EnglishName = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ChargesTypeGeneralTabComponent.prototype, "LocalName", {
        get: function () { return this.EntityPM.LocalName; },
        set: function (newValue) {
            if (this.EntityPM.LocalName != newValue) {
                this.EntityPM.LocalName = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ChargesTypeGeneralTabComponent.prototype, "ChargesGroupCode", {
        get: function () { return this.EntityPM.ChargesGroupCode; },
        set: function (newValue) {
            if (this.EntityPM.ChargesGroupCode != newValue) {
                this.EntityPM.ChargesGroupCode = newValue;
                this.SetUIProperties();
                this.CheckWarnings();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ChargesTypeGeneralTabComponent.prototype, "ChargesGroupId", {
        get: function () { return this.EntityPM.ChargesGroupId; },
        set: function (newValue) {
            var _this = this;
            if (this.EntityPM.ChargesGroupId != newValue) {
                this.EntityPM.ChargesGroupId = newValue;
                if (!Tools_1.AppTool.IsNullOrEmpty(newValue)) {
                    var myService = new ChargesGroupListService_1.ChargesGroupListService();
                    myService.getSingleFromCache(this.EntityPM.ChargesGroupId).subscribe(function (myResponse) {
                        if (!myResponse.HasError && myResponse.Result) {
                            _this.ChargesGroupCode = myResponse.Result.Code;
                        }
                    });
                }
                else
                    this.ChargesGroupCode = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ChargesTypeGeneralTabComponent.prototype, "MeasurementId", {
        get: function () { return this.EntityPM.MeasurementId; },
        set: function (newValue) {
            if (this.EntityPM.MeasurementId != newValue) {
                this.EntityPM.MeasurementId = newValue;
                this.SetUIProperties();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ChargesTypeGeneralTabComponent.prototype, "ContainerMeasurementId", {
        get: function () { return this.EntityPM.ContainerMeasurementId; },
        set: function (newValue) {
            if (this.EntityPM.ContainerMeasurementId != newValue) {
                this.EntityPM.ContainerMeasurementId = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ChargesTypeGeneralTabComponent.prototype, "VatTypeId", {
        get: function () { return this.EntityPM.VatTypeId; },
        set: function (newValue) {
            if (this.EntityPM.VatTypeId != newValue) {
                this.EntityPM.VatTypeId = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ChargesTypeGeneralTabComponent.prototype, "ViewOrder", {
        get: function () { return this.EntityPM.ViewOrder; },
        set: function (newValue) {
            if (this.EntityPM.ViewOrder != newValue) {
                this.EntityPM.ViewOrder = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ChargesTypeGeneralTabComponent.prototype, "Description", {
        get: function () { return this.EntityPM.Description; },
        set: function (newValue) {
            if (this.EntityPM.Description != newValue) {
                this.EntityPM.Description = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ChargesTypeGeneralTabComponent.prototype, "IsReceivable", {
        get: function () { return this.EntityPM.IsReceivable; },
        set: function (newValue) {
            if (this.EntityPM.IsReceivable != newValue) {
                this.EntityPM.IsReceivable = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ChargesTypeGeneralTabComponent.prototype, "IsPayable", {
        get: function () { return this.EntityPM.IsPayable; },
        set: function (newValue) {
            if (this.EntityPM.IsPayable != newValue) {
                this.EntityPM.IsPayable = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ChargesTypeGeneralTabComponent.prototype, "IsCustoms", {
        get: function () { return this.EntityPM.IsCustoms; },
        set: function (newValue) {
            if (this.EntityPM.IsCustoms != newValue) {
                this.EntityPM.IsCustoms = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ChargesTypeGeneralTabComponent.prototype, "PayablesDefaultCurrencyId", {
        get: function () { return this.EntityPM.PayablesDefaultCurrencyId; },
        set: function (newValue) {
            if (this.EntityPM.PayablesDefaultCurrencyId != newValue) {
                this.EntityPM.PayablesDefaultCurrencyId = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ChargesTypeGeneralTabComponent.prototype, "ReceivablesDefaultCurrencyId", {
        get: function () { return this.EntityPM.ReceivablesDefaultCurrencyId; },
        set: function (newValue) {
            if (this.EntityPM.ReceivablesDefaultCurrencyId != newValue) {
                this.EntityPM.ReceivablesDefaultCurrencyId = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ChargesTypeGeneralTabComponent.prototype, "IsExpense", {
        //get IsBackToBack() { return this.EntityPM.IsBackToBack; }
        //set IsBackToBack(newValue: boolean) {
        //    if (this.EntityPM.IsBackToBack != newValue) {
        //        this.EntityPM.IsBackToBack = newValue;
        //        if (newValue) {
        //            this.IsReceivable = false;
        //            this.UIProperties.SetEnabled("IsReceivable", this.ObjectTableName, false);
        //        }
        //        else {
        //            this.UIProperties.SetEnabled("IsReceivable", this.ObjectTableName, true);
        //        }
        //    }
        //}
        get: function () { return this.EntityPM.IsExpense; },
        set: function (newValue) {
            if (this.EntityPM.IsExpense != newValue) {
                this.EntityPM.IsExpense = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ChargesTypeGeneralTabComponent.prototype, "InActive", {
        get: function () { return this.EntityPM.InActive; },
        set: function (newValue) {
            if (this.EntityPM.InActive != newValue) {
                this.EntityPM.InActive = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ChargesTypeGeneralTabComponent.prototype, "IsAir", {
        get: function () { return this.EntityPM.IsAir; },
        set: function (newValue) {
            if (this.EntityPM.IsAir != newValue) {
                this.EntityPM.IsAir = newValue;
                this.SetUIProperties();
                this.CheckWarnings();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ChargesTypeGeneralTabComponent.prototype, "IsInland", {
        get: function () { return this.EntityPM.IsInland; },
        set: function (newValue) {
            if (this.EntityPM.IsInland != newValue) {
                this.EntityPM.IsInland = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ChargesTypeGeneralTabComponent.prototype, "IsOcean", {
        get: function () { return this.EntityPM.IsOcean; },
        set: function (newValue) {
            if (this.EntityPM.IsOcean != newValue) {
                this.EntityPM.IsOcean = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ChargesTypeGeneralTabComponent.prototype, "IsAutoDisplayInQuote", {
        get: function () { return this.EntityPM.IsAutoDisplayInQuote; },
        set: function (newValue) {
            if (this.EntityPM.IsAutoDisplayInQuote != newValue) {
                this.EntityPM.IsAutoDisplayInQuote = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ChargesTypeGeneralTabComponent.prototype, "IsAutoDisplayInShipment", {
        get: function () { return this.EntityPM.IsAutoDisplayInShipment; },
        set: function (newValue) {
            if (this.EntityPM.IsAutoDisplayInShipment != newValue) {
                this.EntityPM.IsAutoDisplayInShipment = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ChargesTypeGeneralTabComponent.prototype, "IsAutoDisplayInConsolidation", {
        get: function () { return this.EntityPM.IsAutoDisplayInConsolidation; },
        set: function (newValue) {
            if (this.EntityPM.IsAutoDisplayInConsolidation != newValue) {
                this.EntityPM.IsAutoDisplayInConsolidation = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ChargesTypeGeneralTabComponent.prototype, "IsAutoDisplayInCustoms", {
        get: function () { return this.EntityPM.IsAutoDisplayInCustoms; },
        set: function (newValue) {
            if (this.EntityPM.IsAutoDisplayInCustoms != newValue) {
                this.EntityPM.IsAutoDisplayInCustoms = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ChargesTypeGeneralTabComponent.prototype, "DueTypeCode", {
        get: function () { return this.EntityPM.DueTypeCode; },
        set: function (newValue) {
            if (this.EntityPM.DueTypeCode != newValue) {
                this.EntityPM.DueTypeCode = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ChargesTypeGeneralTabComponent.prototype, "IATACodeId", {
        get: function () { return this.EntityPM.IATACodeId; },
        set: function (newValue) {
            if (this.EntityPM.IATACodeId != newValue) {
                this.EntityPM.IATACodeId = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ChargesTypeGeneralTabComponent.prototype, "AWBPrintDescription", {
        get: function () { return this.EntityPM.AWBPrintDescription; },
        set: function (newValue) {
            if (this.EntityPM.AWBPrintDescription != newValue) {
                this.EntityPM.AWBPrintDescription = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ChargesTypeGeneralTabComponent.prototype, "SATExternalId", {
        get: function () { return this.EntityPM.SATExternalId; },
        set: function (newValue) {
            if (this.EntityPM.SATExternalId != newValue) {
                this.EntityPM.SATExternalId = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ChargesTypeGeneralTabComponent.prototype, "IsImport", {
        get: function () { return this.EntityPM.IsImport; },
        set: function (newValue) {
            if (this.EntityPM.IsImport != newValue) {
                this.EntityPM.IsImport = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ChargesTypeGeneralTabComponent.prototype, "IsExport", {
        get: function () { return this.EntityPM.IsExport; },
        set: function (newValue) {
            if (this.EntityPM.IsExport != newValue) {
                this.EntityPM.IsExport = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ChargesTypeGeneralTabComponent.prototype, "IsDrop", {
        get: function () { return this.EntityPM.IsDrop; },
        set: function (newValue) {
            if (this.EntityPM.IsDrop != newValue) {
                this.EntityPM.IsDrop = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ChargesTypeGeneralTabComponent.prototype, "IsDomestic", {
        get: function () { return this.EntityPM.IsDomestic; },
        set: function (newValue) {
            if (this.EntityPM.IsDomestic != newValue) {
                this.EntityPM.IsDomestic = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    ChargesTypeGeneralTabComponent = __decorate([
        core_1.Component({
            selector: 'ChargesTypeGeneralTabComponent',
            moduleId: module.id,
            templateUrl: './ChargesTypeGeneralTabComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs])
    ], ChargesTypeGeneralTabComponent);
    return ChargesTypeGeneralTabComponent;
}(BaseComponent_1.BaseComponent));
exports.ChargesTypeGeneralTabComponent = ChargesTypeGeneralTabComponent;
//# sourceMappingURL=ChargesTypeGeneralTabComponent.js.map