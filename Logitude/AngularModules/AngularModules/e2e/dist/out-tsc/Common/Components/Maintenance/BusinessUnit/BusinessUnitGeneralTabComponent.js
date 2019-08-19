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
var BusinessUnitListService_1 = require("../../../Services/StandardLists/BusinessUnitListService");
var Tools_1 = require("../../../../Infrastructure/Tools");
var EntityArgs_1 = require("../../../../Infrastructure/DataContracts/EntityArgs");
var BusinessUnitGeneralTabComponent = /** @class */ (function (_super) {
    __extends(BusinessUnitGeneralTabComponent, _super);
    function BusinessUnitGeneralTabComponent(entityArgs) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this.ObjectTableName = "BusinessUnit";
        _this.DataContext = _this;
        _this.EntityPM = entityArgs.EntityPM;
        _this.SetUIProperties();
        return _this;
    }
    BusinessUnitGeneralTabComponent.prototype.SetUIProperties = function () {
        this.UIProperties.SetEnabled("ParentId", this.ObjectTableName, false);
    };
    Object.defineProperty(BusinessUnitGeneralTabComponent.prototype, "Name", {
        //Properties
        get: function () { return this.EntityPM.Name; },
        set: function (value) {
            if (this.EntityPM.Name != value) {
                this.EntityPM.Name = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(BusinessUnitGeneralTabComponent.prototype, "ParentId", {
        get: function () { return this.EntityPM.ParentId; },
        set: function (value) {
            if (this.EntityPM.ParentId != value) {
                this.EntityPM.ParentId = value;
                this.UpdateParentName();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(BusinessUnitGeneralTabComponent.prototype, "ParentName", {
        get: function () { return this.EntityPM.ParentName; },
        set: function (value) {
            if (this.EntityPM.ParentName != value) {
                this.EntityPM.ParentName = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    BusinessUnitGeneralTabComponent.prototype.UpdateParentName = function () {
        var _this = this;
        if (Tools_1.AppTool.IsNullOrEmpty(this.ParentId)) {
            this.ParentName = null;
        }
        else {
            var service = new BusinessUnitListService_1.BusinessUnitListService();
            service.getAll().subscribe(function (response) {
                if (!response.HasError) {
                    var list = response.Result;
                    var businessunit = list.filter(function (d) { return d.Id == _this.ParentId; })[0];
                    if (businessunit != null) {
                        _this.ParentName = list.Name;
                    }
                }
            });
        }
    };
    Object.defineProperty(BusinessUnitGeneralTabComponent.prototype, "InActive", {
        get: function () { return this.EntityPM.InActive; },
        set: function (value) {
            if (this.EntityPM.InActive != value) {
                this.EntityPM.InActive = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    BusinessUnitGeneralTabComponent = __decorate([
        core_1.Component({
            selector: 'BusinessUnitGeneralTabComponent',
            moduleId: module.id,
            templateUrl: './BusinessUnitGeneralTabComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs])
    ], BusinessUnitGeneralTabComponent);
    return BusinessUnitGeneralTabComponent;
}(BaseComponent_1.BaseComponent));
exports.BusinessUnitGeneralTabComponent = BusinessUnitGeneralTabComponent;
//# sourceMappingURL=BusinessUnitGeneralTabComponent.js.map