"use strict";
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
var PropertyChangedArgs_1 = require("../../Infrastructure/EventEmitterArgs/PropertyChangedArgs");
var UIProperties_1 = require("../../Infrastructure/Components/LogitudeComponents/UIProperties");
var ServiceLocator_1 = require("../../Infrastructure/Locators/ServiceLocator");
var ServiceHelper_1 = require("../../Infrastructure/Utilities/ServiceHelper");
var QueryPM = /** @class */ (function () {
    function QueryPM() {
        this.PropertyChanged = new core_1.EventEmitter();
        this.UIProperties = new UIProperties_1.UIProperties(this);
        this.IsDirty = false;
    }
    Object.defineProperty(QueryPM.prototype, "Id", {
        get: function () { return this.id; },
        set: function (newValue) { this.id = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QueryPM.prototype, "Tenant", {
        get: function () { return this.tenant; },
        set: function (newValue) { this.tenant = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QueryPM.prototype, "Code", {
        get: function () { return this.code; },
        set: function (newValue) { this.code = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QueryPM.prototype, "ObjectTableId", {
        get: function () { return this.objectTableId; },
        set: function (newValue) { this.objectTableId = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QueryPM.prototype, "SystemLevel", {
        get: function () { return this.systemLevel; },
        set: function (newValue) { this.systemLevel = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QueryPM.prototype, "TenantLevel", {
        get: function () { return this.tenantLevel; },
        set: function (newValue) { this.tenantLevel = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QueryPM.prototype, "OriginalQueryId", {
        get: function () { return this.originalQueryId; },
        set: function (newValue) { this.originalQueryId = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QueryPM.prototype, "QuerySection", {
        get: function () { return this.querySection; },
        set: function (newValue) { this.querySection = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QueryPM.prototype, "IndexOrder", {
        get: function () { return this.indexOrder; },
        set: function (newValue) { this.indexOrder = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QueryPM.prototype, "DisplayCount", {
        get: function () { return this.displayCount; },
        set: function (newValue) { this.displayCount = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QueryPM.prototype, "ObjectTableName", {
        get: function () { return this.objectTableName; },
        set: function (newValue) { this.objectTableName = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QueryPM.prototype, "ObjectTableIsNewWizard", {
        get: function () { return this.objectTableIsNewWizard; },
        set: function (newValue) { this.objectTableIsNewWizard = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QueryPM.prototype, "ObjectTableNewWizardControlName", {
        get: function () { return this.objectTableNewWizardControlName; },
        set: function (newValue) { this.objectTableNewWizardControlName = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QueryPM.prototype, "QueryGroupCode", {
        get: function () { return this.queryGroupCode; },
        set: function (newValue) { this.queryGroupCode = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QueryPM.prototype, "QueryGroupIndexOrder", {
        get: function () { return this.queryGroupIndexOrder; },
        set: function (newValue) { this.queryGroupIndexOrder = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QueryPM.prototype, "IsAddNewEntityEnabled", {
        get: function () { return this.isAddNewEntityEnabled; },
        set: function (newValue) { this.isAddNewEntityEnabled = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QueryPM.prototype, "UserId", {
        get: function () { return this.userId; },
        set: function (newValue) { this.userId = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QueryPM.prototype, "NameTextCodeId", {
        get: function () { return this.nameTextCodeId; },
        set: function (newValue) { this.nameTextCodeId = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QueryPM.prototype, "NameTextCodeCode", {
        get: function () { return this.nameTextCodeCode; },
        set: function (newValue) { this.nameTextCodeCode = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QueryPM.prototype, "DefaultSortDirection", {
        get: function () { return this.defaultSortDirection; },
        set: function (newValue) { this.defaultSortDirection = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QueryPM.prototype, "DefaultSortColumn", {
        get: function () { return this.defaultSortColumn; },
        set: function (newValue) { this.defaultSortColumn = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QueryPM.prototype, "SpotlightDataTemplate", {
        get: function () { return this.spotlightDataTemplate; },
        set: function (newValue) { this.spotlightDataTemplate = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QueryPM.prototype, "Internal", {
        get: function () { return this.internal; },
        set: function (newValue) { this.internal = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QueryPM.prototype, "Customer", {
        get: function () { return this.customer; },
        set: function (newValue) { this.customer = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QueryPM.prototype, "Agent", {
        get: function () { return this.agent; },
        set: function (newValue) { this.agent = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QueryPM.prototype, "IsHiddenFromView", {
        get: function () { return this.isHiddenFromView; },
        set: function (newValue) { this.isHiddenFromView = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QueryPM.prototype, "IsNewFromTenantZeroOnly", {
        get: function () { return this.isNewFromTenantZeroOnly; },
        set: function (newValue) { this.isNewFromTenantZeroOnly = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QueryPM.prototype, "FeatureId", {
        get: function () { return this.featureId; },
        set: function (newValue) { this.featureId = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QueryPM.prototype, "EditWizardName", {
        get: function () { return this.editWizardName; },
        set: function (newValue) { this.editWizardName = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QueryPM.prototype, "Perspective", {
        get: function () { return this.perspective; },
        set: function (newValue) { this.perspective = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QueryPM.prototype, "NewViewName", {
        get: function () { return this.newViewName; },
        set: function (newValue) { this.newViewName = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QueryPM.prototype, "DisplayAsCustom", {
        get: function () { return this.displayAsCustom; },
        set: function (newValue) { this.displayAsCustom = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QueryPM.prototype, "IsDummy", {
        get: function () { return this.isDummy; },
        set: function (newValue) { this.isDummy = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QueryPM.prototype, "EditWizardComponentPath", {
        get: function () { return this.editWizardComponentPath; },
        set: function (newValue) { this.editWizardComponentPath = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QueryPM.prototype, "SharedWithAll", {
        get: function () { return this.sharedWithAll; },
        set: function (newValue) { this.sharedWithAll = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QueryPM.prototype, "SharedWithSpecificUsers", {
        get: function () { return this.sharedWithSpecificUsers; },
        set: function (newValue) { this.sharedWithSpecificUsers = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QueryPM.prototype, "SharedByUserId", {
        get: function () { return this.sharedByUserId; },
        set: function (newValue) { this.sharedByUserId = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QueryPM.prototype, "SharedByUserName", {
        get: function () { return this.sharedByUserName; },
        set: function (newValue) { this.sharedByUserName = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QueryPM.prototype, "SharedByUserEmail", {
        get: function () { return this.sharedByUserEmail; },
        set: function (newValue) { this.sharedByUserEmail = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QueryPM.prototype, "SpotlightModeActivated", {
        get: function () { return this.spotlightModeActivated; },
        set: function (newValue) { this.spotlightModeActivated = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QueryPM.prototype, "SharedUserQueries", {
        get: function () {
            if (this.sharedUserQueries == null) {
                this.sharedUserQueries = [];
            }
            return this.sharedUserQueries;
        },
        set: function (newValue) {
            if (this.sharedUserQueries != newValue) {
                this.sharedUserQueries = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    QueryPM.prototype.AddSharedUserQueryPM = function (item) {
        if (item != null) {
            var index = this.SharedUserQueries.indexOf(item);
            if (index == -1) {
                item.EntityParentPM = this;
                this.SharedUserQueries.push(item);
                this.MarkAsDirty();
            }
        }
    };
    QueryPM.prototype.RemoveSharedUserQueryPM = function (item) {
        if (item != null) {
            var index = this.SharedUserQueries.indexOf(item);
            if (index > -1) {
                this.SharedUserQueries.splice(index, 1);
                this.MarkAsDirty();
            }
        }
    };
    QueryPM.prototype.MarkAsDirty = function (propertyName) {
        if (propertyName === void 0) { propertyName = null; }
        this.IsDirty = true;
        if (propertyName != null) {
            this.PropertyChanged.emit(new PropertyChangedArgs_1.PropertyChangedArgs(propertyName, this));
            ServiceLocator_1.ServiceLocator.RulesValidator.ApplyEntityChangedRules(propertyName, this, "Query");
        }
    };
    QueryPM.prototype.CloneMe = function () {
        ServiceHelper_1.ServiceHelper.CloneEntityPM(this);
    };
    QueryPM.prototype.RejectChanges = function () {
        ServiceHelper_1.ServiceHelper.RejectEntityPMChanges(this);
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", core_1.EventEmitter)
    ], QueryPM.prototype, "PropertyChanged", void 0);
    return QueryPM;
}());
exports.QueryPM = QueryPM;
//# sourceMappingURL=QueryPM.js.map