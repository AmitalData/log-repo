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
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var Tools_1 = require("../../../../Infrastructure/Tools");
var BIReportGeneralTabComponent = /** @class */ (function (_super) {
    __extends(BIReportGeneralTabComponent, _super);
    function BIReportGeneralTabComponent(entityArgs) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this.ObjectTableName = "BIReport";
        _this.DataContext = _this;
        _this._entityResourceService = new EntityResourceService_1.EntityResourceService();
        _this._entityResourceService.getEntityResourceByTableName("BIReport").subscribe(function (response) { });
        _this.EntityPM = _this.entityArgs.EntityPM;
        _this.SetUIProperties();
        return _this;
    }
    BIReportGeneralTabComponent.prototype.SetUIProperties = function () {
        this.UIProperties.SetEnabled("TypeCode", this.ObjectTableName, false);
        this.UIProperties.SetRequired("DWQueryId", this.ObjectTableName, Tools_1.AppTool.IsNullOrEmpty(this.DWQueryId));
    };
    Object.defineProperty(BIReportGeneralTabComponent.prototype, "Name", {
        get: function () { return this.EntityPM.Name; },
        set: function (newValue) {
            if (this.EntityPM.Name != newValue) {
                this.EntityPM.Name = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(BIReportGeneralTabComponent.prototype, "Description", {
        get: function () { return this.EntityPM.Description; },
        set: function (newValue) {
            if (this.EntityPM.Description != newValue) {
                this.EntityPM.Description = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(BIReportGeneralTabComponent.prototype, "BIReportFolderId", {
        get: function () { return this.EntityPM.BIReportFolderId; },
        set: function (newValue) {
            if (this.EntityPM.BIReportFolderId != newValue) {
                this.EntityPM.BIReportFolderId = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(BIReportGeneralTabComponent.prototype, "DWQueryId", {
        get: function () { return this.EntityPM.DWQueryId; },
        set: function (newValue) {
            if (this.EntityPM.DWQueryId != newValue) {
                this.EntityPM.DWQueryId = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(BIReportGeneralTabComponent.prototype, "TypeCode", {
        get: function () { return this.EntityPM.TypeCode; },
        set: function (newValue) {
            if (this.EntityPM.TypeCode != newValue) {
                this.EntityPM.TypeCode = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    BIReportGeneralTabComponent = __decorate([
        core_1.Component({
            selector: 'BIReportGeneralTabComponent',
            moduleId: module.id,
            templateUrl: './BIReportGeneralTabComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs])
    ], BIReportGeneralTabComponent);
    return BIReportGeneralTabComponent;
}(BaseComponent_1.BaseComponent));
exports.BIReportGeneralTabComponent = BIReportGeneralTabComponent;
//# sourceMappingURL=BIReportGeneralTabComponent.js.map