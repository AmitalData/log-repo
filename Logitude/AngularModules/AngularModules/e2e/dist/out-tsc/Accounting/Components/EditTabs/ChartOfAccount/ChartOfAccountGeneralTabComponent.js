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
var EntityArgs_1 = require("../../../../Infrastructure/DataContracts/EntityArgs");
var ApiQueryFilters_1 = require("../../../../Infrastructure/DataContracts/ApiQueryFilters");
var ChartOfAccountGeneralTabComponent = /** @class */ (function (_super) {
    __extends(ChartOfAccountGeneralTabComponent, _super);
    function ChartOfAccountGeneralTabComponent(entityArgs, CD) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this.CD = CD;
        _this.oldCurrency = null;
        _this.EntityPM = null;
        _this.ObjectTableName = "ChartOfAccount";
        _this.DataContext = _this;
        _this.DisableChartOfAccount = false;
        _this.IsEditMode = false;
        _this.IsCustomerAccount = false;
        // Set Entity
        _this.EntityPM = entityArgs.EntityPM;
        // initialize query filters for Parent Account
        _this.ParentsFilterItems = new ApiQueryFilters_1.ApiQueryFilters();
        _this.ParentsFilterItems.addAdditionalFilter("Id", _this.EntityPM.Id, null, null, "Exclude", false, false, false, "string");
        _this.SetUIProperties();
        return _this;
    }
    Object.defineProperty(ChartOfAccountGeneralTabComponent.prototype, "Inactive", {
        // Properties
        get: function () { return this.EntityPM.Inactive == null ? false : this.EntityPM.Inactive; },
        set: function (value) {
            if (this.Inactive != value) {
                this.EntityPM.Inactive = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ChartOfAccountGeneralTabComponent.prototype, "Code", {
        get: function () { return this.EntityPM.Code; },
        set: function (value) {
            if (this.EntityPM.Code != value) {
                this.EntityPM.Code = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ChartOfAccountGeneralTabComponent.prototype, "LocalName", {
        get: function () { return this.EntityPM.LocalName; },
        set: function (value) {
            if (this.EntityPM.LocalName != value) {
                this.EntityPM.LocalName = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ChartOfAccountGeneralTabComponent.prototype, "EnglishName", {
        get: function () { return this.EntityPM.EnglishName; },
        set: function (value) {
            if (this.EntityPM.EnglishName != value) {
                this.EntityPM.EnglishName = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ChartOfAccountGeneralTabComponent.prototype, "TypeCode", {
        get: function () { return this.EntityPM.TypeCode; },
        set: function (value) {
            if (this.EntityPM.TypeCode != value) {
                this.EntityPM.TypeCode = value;
                if (value != null) {
                    this.UIProperties.SetEnabled("ParentId", this.ObjectTableName, true);
                }
                else {
                    this.ParentId = null;
                    this.UIProperties.SetEnabled("ParentId", this.ObjectTableName, false);
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ChartOfAccountGeneralTabComponent.prototype, "ParentId", {
        get: function () { return this.EntityPM.ParentId; },
        set: function (value) {
            if (this.EntityPM.ParentId != value) {
                this.EntityPM.ParentId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    ChartOfAccountGeneralTabComponent.prototype.SetUIProperties = function () {
        if (!this.EntityPM.TypeCode) {
            this.UIProperties.SetEnabled("ParentId", this.ObjectTableName, false);
        }
    };
    ChartOfAccountGeneralTabComponent.prototype.OnLovItemChanged = function (item) {
        //if (item == null) {
        //    this.ChartOfAccountsId = null;
        //    this.UIProperties.SetEnabled("ChartOfAccountsId", this.ObjectTableName, false);
        //    this.UIProperties.SetRequired("ChartOfAccountsId", this.ObjectTableName, false);
        //    this.UIProperties.SetValidity("ChartOfAccountsId", this.ObjectTableName, true, "Chart Of Accounts is requierd");
        //} else {
        //    this.UIProperties.SetEnabled("ChartOfAccountsId", this.ObjectTableName, true);
        //    if (!this.EntityPM.ChartOfAccountsId) {
        //        this.UIProperties.SetValidity("ChartOfAccountsId", this.ObjectTableName, false, "");
        //        this.UIProperties.SetRequired("ChartOfAccountsId", this.ObjectTableName, true);
        //    }
        //}
    };
    ChartOfAccountGeneralTabComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './ChartOfAccountGeneralTabComponent.html'
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs, core_1.ChangeDetectorRef])
    ], ChartOfAccountGeneralTabComponent);
    return ChartOfAccountGeneralTabComponent;
}(BaseComponent_1.BaseComponent));
exports.ChartOfAccountGeneralTabComponent = ChartOfAccountGeneralTabComponent;
//# sourceMappingURL=ChartOfAccountGeneralTabComponent.js.map