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
var Tools_1 = require("../../../../Infrastructure/Tools");
var ChargesTypeListService_1 = require("../../../../Common/Services/StandardLists/ChargesTypeListService");
var ApiQueryFilters_1 = require("../../../../Infrastructure/DataContracts/ApiQueryFilters");
var TariffGeneralTabComponent = /** @class */ (function (_super) {
    __extends(TariffGeneralTabComponent, _super);
    function TariffGeneralTabComponent(entityArgs) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this.ObjectTableName = "Tariff";
        _this.DataContext = _this;
        _this.VisibileSurchargesArea = false;
        _this.IdProps = [];
        _this.UOMProps = [];
        _this.ValidationErrorsList = [];
        _this.firstIndex = 0;
        _this.SaveCompletedEvent = null;
        _this.EntityPM = entityArgs.EntityPM;
        _this.chargesTypePMService = new ChargesTypeListService_1.ChargesTypeListService();
        _this.FillChargesIDsAndUOMS();
        _this.BuildQueryFilters();
        if (_this.EntityPM.TypeCode == "ASC") {
            _this.VisibileSurchargesArea = true;
        }
        else {
            _this.VisibileSurchargesArea = false;
        }
        _this.Listen();
        return _this;
    }
    TariffGeneralTabComponent.prototype.BuildQueryFilters = function () {
        this.ChargeTypesQueryFilters = new ApiQueryFilters_1.ApiQueryFilters();
        this.ChargeTypesQueryFilters.addAdditionalFilter("InActive", false, null, null, "Equals", false, false, false, "Boolean");
        this.ChargeTypesQueryFilters.addAdditionalFilter("IsAir", true, null, null, "Equals", false, false, false, "Boolean");
        this.ChargeTypesQueryFilters.addAdditionalFilter("ChargesGroupCode", "FRT", null, null, "NotEqual", false, false, false, "string");
    };
    TariffGeneralTabComponent.prototype.FillChargesIDsAndUOMS = function () {
        for (var index = 1; index <= 10; index++) {
            this.IdProps.push("Surcharge" + index + "Id");
            this.UOMProps.push("Surcharge" + index + "UOM");
        }
    };
    TariffGeneralTabComponent.prototype.SetDefaultUOM = function (index) {
        var _this = this;
        this.chargesTypePMService.getSingleFromCache(this[this.IdProps[index]]).subscribe(function (res) {
            if (!res.HasError) {
                if (res.Result) {
                    var ChargesType = res.Result;
                    _this[_this.UOMProps[index]] = ChargesType.MeasurementId;
                }
            }
        });
    };
    Object.defineProperty(TariffGeneralTabComponent.prototype, "Surcharge1Id", {
        get: function () {
            return this.EntityPM.Surcharge1Id;
        },
        set: function (value) {
            if (this.EntityPM.Surcharge1Id != value) {
                this.EntityPM.Surcharge1Id = value;
                this.Validate();
                if (value != null) {
                    this.SetDefaultUOM(0);
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TariffGeneralTabComponent.prototype, "Surcharge2Id", {
        get: function () {
            return this.EntityPM.Surcharge2Id;
        },
        set: function (value) {
            if (this.EntityPM.Surcharge2Id != value) {
                this.EntityPM.Surcharge2Id = value;
                this.Validate();
                if (value != null) {
                    this.SetDefaultUOM(1);
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TariffGeneralTabComponent.prototype, "Surcharge3Id", {
        get: function () {
            return this.EntityPM.Surcharge3Id;
        },
        set: function (value) {
            if (this.EntityPM.Surcharge3Id != value) {
                this.EntityPM.Surcharge3Id = value;
                this.Validate();
                if (value != null) {
                    this.SetDefaultUOM(2);
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TariffGeneralTabComponent.prototype, "Surcharge4Id", {
        get: function () {
            return this.EntityPM.Surcharge4Id;
        },
        set: function (value) {
            if (this.EntityPM.Surcharge4Id != value) {
                this.EntityPM.Surcharge4Id = value;
                this.Validate();
                if (value != null) {
                    this.SetDefaultUOM(3);
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TariffGeneralTabComponent.prototype, "Surcharge5Id", {
        get: function () {
            return this.EntityPM.Surcharge5Id;
        },
        set: function (value) {
            if (this.EntityPM.Surcharge5Id != value) {
                this.EntityPM.Surcharge5Id = value;
                this.Validate();
                if (value != null) {
                    this.SetDefaultUOM(4);
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TariffGeneralTabComponent.prototype, "Surcharge6Id", {
        get: function () {
            return this.EntityPM.Surcharge6Id;
        },
        set: function (value) {
            if (this.EntityPM.Surcharge6Id != value) {
                this.EntityPM.Surcharge6Id = value;
                this.Validate();
                if (value != null) {
                    this.SetDefaultUOM(5);
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TariffGeneralTabComponent.prototype, "Surcharge7Id", {
        get: function () {
            return this.EntityPM.Surcharge7Id;
        },
        set: function (value) {
            if (this.EntityPM.Surcharge7Id != value) {
                this.EntityPM.Surcharge7Id = value;
                this.Validate();
                if (value != null) {
                    this.SetDefaultUOM(6);
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TariffGeneralTabComponent.prototype, "Surcharge8Id", {
        get: function () {
            return this.EntityPM.Surcharge8Id;
        },
        set: function (value) {
            if (this.EntityPM.Surcharge8Id != value) {
                this.EntityPM.Surcharge8Id = value;
                this.Validate();
                if (value != null) {
                    this.SetDefaultUOM(7);
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TariffGeneralTabComponent.prototype, "Surcharge9Id", {
        get: function () {
            return this.EntityPM.Surcharge9Id;
        },
        set: function (value) {
            if (this.EntityPM.Surcharge9Id != value) {
                this.EntityPM.Surcharge9Id = value;
                this.Validate();
                if (value != null) {
                    this.SetDefaultUOM(8);
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TariffGeneralTabComponent.prototype, "Surcharge10Id", {
        get: function () {
            return this.EntityPM.Surcharge10Id;
        },
        set: function (value) {
            if (this.EntityPM.Surcharge10Id != value) {
                this.EntityPM.Surcharge10Id = value;
                this.Validate();
                if (value != null) {
                    this.SetDefaultUOM(9);
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TariffGeneralTabComponent.prototype, "Surcharge1UOM", {
        get: function () {
            return this.EntityPM.Surcharge1UOM;
        },
        set: function (value) {
            if (this.EntityPM.Surcharge1UOM != value) {
                this.EntityPM.Surcharge1UOM = value;
                this.Validate();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TariffGeneralTabComponent.prototype, "Surcharge2UOM", {
        get: function () {
            return this.EntityPM.Surcharge2UOM;
        },
        set: function (value) {
            if (this.EntityPM.Surcharge2UOM != value) {
                this.EntityPM.Surcharge2UOM = value;
                this.Validate();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TariffGeneralTabComponent.prototype, "Surcharge3UOM", {
        get: function () {
            return this.EntityPM.Surcharge3UOM;
        },
        set: function (value) {
            if (this.EntityPM.Surcharge3UOM != value) {
                this.EntityPM.Surcharge3UOM = value;
                this.Validate();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TariffGeneralTabComponent.prototype, "Surcharge4UOM", {
        get: function () {
            return this.EntityPM.Surcharge4UOM;
        },
        set: function (value) {
            if (this.EntityPM.Surcharge4UOM != value) {
                this.EntityPM.Surcharge4UOM = value;
                this.Validate();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TariffGeneralTabComponent.prototype, "Surcharge5UOM", {
        get: function () {
            return this.EntityPM.Surcharge5UOM;
        },
        set: function (value) {
            if (this.EntityPM.Surcharge5UOM != value) {
                this.EntityPM.Surcharge5UOM = value;
                this.Validate();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TariffGeneralTabComponent.prototype, "Surcharge6UOM", {
        get: function () {
            return this.EntityPM.Surcharge6UOM;
        },
        set: function (value) {
            if (this.EntityPM.Surcharge6UOM != value) {
                this.EntityPM.Surcharge6UOM = value;
                this.Validate();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TariffGeneralTabComponent.prototype, "Surcharge7UOM", {
        get: function () {
            return this.EntityPM.Surcharge7UOM;
        },
        set: function (value) {
            if (this.EntityPM.Surcharge7UOM != value) {
                this.EntityPM.Surcharge7UOM = value;
                this.Validate();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TariffGeneralTabComponent.prototype, "Surcharge8UOM", {
        get: function () {
            return this.EntityPM.Surcharge8UOM;
        },
        set: function (value) {
            if (this.EntityPM.Surcharge8UOM != value) {
                this.EntityPM.Surcharge8UOM = value;
                this.Validate();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TariffGeneralTabComponent.prototype, "Surcharge9UOM", {
        get: function () {
            return this.EntityPM.Surcharge9UOM;
        },
        set: function (value) {
            if (this.EntityPM.Surcharge9UOM != value) {
                this.EntityPM.Surcharge9UOM = value;
                this.Validate();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TariffGeneralTabComponent.prototype, "Surcharge10UOM", {
        get: function () {
            return this.EntityPM.Surcharge10UOM;
        },
        set: function (value) {
            if (this.EntityPM.Surcharge10UOM != value) {
                this.EntityPM.Surcharge10UOM = value;
                this.Validate();
            }
        },
        enumerable: true,
        configurable: true
    });
    TariffGeneralTabComponent.prototype.ValidateSurcharge = function () {
        var _this = this;
        var IdProps = [];
        var UOMProps = [];
        var IdPropsName = [];
        var UOMPropsName = [];
        var DuplicatedChargesIds = [];
        var EmptyIndex = 1;
        var emptyLines = false;
        var FirstLineEmpty = false;
        var tempErrors = [];
        for (var index = 1; index <= 10; index++) {
            IdProps.push("Surcharge" + index + "Id");
            UOMProps.push("Surcharge" + index + "UOM");
            IdPropsName.push("Charge Type " + index);
            UOMPropsName.push("UOM " + index);
            if (this.IdProps.filter(function (p) { return _this[p + ""] == _this[IdProps[index - 1]] && (p + "" != IdProps[index - 1] + "") && _this[IdProps[index - 1]] != null; })[0] != null) {
                var chargresType = this.IdProps.filter(function (p) { return _this[p + ""] == _this[IdProps[index - 1]] && (p + "" != IdProps[index - 1] + "") && _this[IdProps[index - 1]] != null; })[0];
                if (!DuplicatedChargesIds.includes(this[chargresType + ""])) {
                    DuplicatedChargesIds.push(this[chargresType + ""]);
                    this.chargesTypePMService.getSingleFromCache(this[chargresType + ""]).subscribe(function (res) {
                        if (!res.HasError) {
                            var chargesTypeList = res.Result;
                            if (res) {
                                _this.ValidationErrorsList.push("Charge type " + chargesTypeList.EnglishName + " is duplicated");
                            }
                        }
                    });
                }
            }
            if (index == 1) {
                if (Tools_1.AppTool.IsNullOrEmpty(this[IdProps[index - 1]])) {
                    tempErrors.push(IdPropsName[index - 1] + " is required");
                    FirstLineEmpty = true;
                }
                if (Tools_1.AppTool.IsNullOrEmpty(this[UOMProps[index - 1]])) {
                    tempErrors.push(UOMPropsName[index - 1] + " is required");
                }
            }
            else {
                if (Tools_1.AppTool.IsNullOrEmpty(this[IdProps[index - 1]])) {
                    if (EmptyIndex == 1) {
                        EmptyIndex = index;
                    }
                }
                if (Tools_1.AppTool.IsNullOrEmpty(this[UOMProps[index - 1]]) && !Tools_1.AppTool.IsNullOrEmpty(this[IdProps[index - 1]])) {
                    this.ValidationErrorsList.push(IdPropsName[index - 1] + " is filled without a UOM");
                }
                if (index == 2) {
                    if (!Tools_1.AppTool.IsNullOrEmpty(this[UOMProps[index - 1]]) && !Tools_1.AppTool.IsNullOrEmpty(this[IdProps[index - 1]])) {
                        if (FirstLineEmpty) {
                            emptyLines = true;
                            this.ValidationErrorsList.push("Empty Charge Lines aren't allowed between line 1 and line 2");
                            EmptyIndex = 1;
                        }
                    }
                }
                if (index >= 3) {
                    if (!Tools_1.AppTool.IsNullOrEmpty(this[UOMProps[index - 1]]) && !Tools_1.AppTool.IsNullOrEmpty(this[IdProps[index - 1]])) {
                        if (EmptyIndex != 1) {
                            emptyLines = true;
                            this.ValidationErrorsList.push("Empty Charge Lines aren't allowed between line " + (EmptyIndex - 1) + " and line " + index);
                            EmptyIndex = 1;
                        }
                        if (Tools_1.AppTool.IsNullOrEmpty(this[IdProps[index - 2]])) {
                            //   this.ValidationErrorsList.push("no empty line between 2 charges in line "+index+ " and "+(index-2));
                        }
                    }
                }
            }
        }
        if (!emptyLines) {
            tempErrors.forEach(function (error) {
                _this.ValidationErrorsList.push(error);
            });
        }
    };
    TariffGeneralTabComponent.prototype.ngAfterViewInit = function () {
        this.Validate(true);
    };
    TariffGeneralTabComponent.prototype.Validate = function (initial) {
        if (initial === void 0) { initial = false; }
        var entered = false;
        for (var index = 1; index <= 10; index++) {
            if (initial) {
                if (!Tools_1.AppTool.IsNullOrEmpty(this[this.IdProps[index - 1]])) {
                    this.UIProperties.SetEnabled(this.IdProps[index - 1], this.ObjectTableName, false);
                    this.UIProperties.SetEnabled(this.UOMProps[index - 1], this.ObjectTableName, false);
                }
                else {
                    if (!entered) {
                        entered = true;
                        this.firstIndex = index;
                        this.UIProperties.SetEnabled(this.IdProps[index - 1], this.ObjectTableName, true);
                        this.UIProperties.SetEnabled(this.UOMProps[index - 1], this.ObjectTableName, false);
                    }
                    else {
                        this.UIProperties.SetEnabled(this.IdProps[index - 1], this.ObjectTableName, false);
                        this.UIProperties.SetEnabled(this.UOMProps[index - 1], this.ObjectTableName, false);
                    }
                }
            }
            else {
                if (index >= this.firstIndex) {
                    if (Tools_1.AppTool.IsNullOrEmpty(this[this.IdProps[index - 1]])) {
                        this[this.UOMProps[index - 1]] = null;
                        this.UIProperties.SetEnabled(this.UOMProps[index - 1], this.ObjectTableName, false);
                        if (index > 1) {
                            if (!Tools_1.AppTool.IsNullOrEmpty(this[this.UOMProps[index - 2]]) && !Tools_1.AppTool.IsNullOrEmpty(this[this.IdProps[index - 2]])) {
                                this.UIProperties.SetEnabled(this.IdProps[index - 1], this.ObjectTableName, true);
                                this.UIProperties.SetEnabled(this.UOMProps[index - 1], this.ObjectTableName, false);
                            }
                        }
                        else {
                            this.UIProperties.SetRequired(this.IdProps[index - 1], this.ObjectTableName, true);
                            this.UIProperties.SetRequired(this.UOMProps[index - 1], this.ObjectTableName, true);
                        }
                    }
                    else {
                        this.UIProperties.SetEnabled(this.UOMProps[index - 1], this.ObjectTableName, true);
                        if (index == 1) {
                            this.UIProperties.SetRequired(this.IdProps[index - 1], this.ObjectTableName, false);
                            if (Tools_1.AppTool.IsNullOrEmpty(this[this.UOMProps[index - 1]])) {
                                this.UIProperties.SetRequired(this.UOMProps[index - 1], this.ObjectTableName, true);
                            }
                            else {
                                this.UIProperties.SetRequired(this.UOMProps[index - 1], this.ObjectTableName, false);
                            }
                        }
                    }
                }
            }
        }
    };
    TariffGeneralTabComponent.prototype.Listen = function () {
        var _this = this;
        if (this.entityArgs.EditComponent != null) {
            this.SaveCompletedEvent = this.entityArgs.EditComponent.SaveCompleted.subscribe(function (isSaveSuccess) {
                if (isSaveSuccess) {
                    _this.EntityPM = _this.entityArgs.EditComponent.EntityPM;
                    _this.Validate(true);
                }
            });
        }
    };
    TariffGeneralTabComponent.prototype.ngOnDestroy = function () {
        Tools_1.AppTool.KillEventEmitter(this.SaveCompletedEvent);
    };
    Object.defineProperty(TariffGeneralTabComponent.prototype, "Name", {
        get: function () { return this.EntityPM.Name; },
        set: function (value) {
            if (this.EntityPM.Name != value) {
                this.EntityPM.Name = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TariffGeneralTabComponent.prototype, "Description", {
        get: function () { return this.EntityPM.Description; },
        set: function (value) {
            if (this.EntityPM.Description != value) {
                this.EntityPM.Description = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TariffGeneralTabComponent.prototype, "CurrencyId", {
        get: function () { return this.EntityPM.CurrencyId; },
        set: function (value) {
            if (this.EntityPM.CurrencyId != value) {
                this.EntityPM.CurrencyId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TariffGeneralTabComponent.prototype, "SellerId", {
        get: function () { return this.EntityPM.SellerId; },
        set: function (value) {
            if (this.EntityPM.SellerId != value) {
                this.EntityPM.SellerId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TariffGeneralTabComponent.prototype, "ContractNumber", {
        get: function () { return this.EntityPM.ContractNumber; },
        set: function (value) {
            if (this.EntityPM.ContractNumber != value) {
                this.EntityPM.ContractNumber = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    TariffGeneralTabComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './TariffGeneralTabComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs])
    ], TariffGeneralTabComponent);
    return TariffGeneralTabComponent;
}(BaseComponent_1.BaseComponent));
exports.TariffGeneralTabComponent = TariffGeneralTabComponent;
//# sourceMappingURL=TariffGeneralTabComponent.js.map