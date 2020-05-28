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
var SessionLocator_1 = require("../../../../../Infrastructure/Utilities/SessionLocator");
var EntityArgs_1 = require("../../../../../Infrastructure/DataContracts/EntityArgs");
var BaseComponent_1 = require("../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var ProductTypeGeneralTabComponent = /** @class */ (function (_super) {
    __extends(ProductTypeGeneralTabComponent, _super);
    function ProductTypeGeneralTabComponent(entityArgs) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this.ObjectTableName = "ProductType";
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.DataContext = _this;
        _this.EntityPM = entityArgs.EntityPM;
        _this.ObjectTableName = entityArgs.ObjectTableName;
        return _this;
    }
    ProductTypeGeneralTabComponent.prototype.ngOnInit = function () {
        this.SetUIPropertiesEnabled();
    };
    ProductTypeGeneralTabComponent.prototype.SetUIPropertiesEnabled = function () {
        this.UIProperties.SetEnabled("Code", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("Name", this.ObjectTableName, false);
    };
    Object.defineProperty(ProductTypeGeneralTabComponent.prototype, "Code", {
        get: function () { return this.EntityPM.Code; },
        set: function (value) {
            if (this.EntityPM.Code != value) {
                this.EntityPM.Code = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ProductTypeGeneralTabComponent.prototype, "Name", {
        get: function () { return this.EntityPM.Name; },
        set: function (value) {
            if (this.EntityPM.Name != value) {
                this.EntityPM.Name = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ProductTypeGeneralTabComponent.prototype, "InActive", {
        get: function () { return this.EntityPM.InActive; },
        set: function (value) {
            if (this.EntityPM.InActive != value) {
                this.EntityPM.InActive = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ProductTypeGeneralTabComponent.prototype, "QuotationDefaultTemplateId", {
        get: function () { return this.EntityPM.QuotationDefaultTemplateId; },
        set: function (value) {
            if (this.EntityPM.QuotationDefaultTemplateId != value) {
                this.EntityPM.QuotationDefaultTemplateId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ProductTypeGeneralTabComponent.prototype, "RoutingRQuoteDefaultTemplateId", {
        get: function () { return this.EntityPM.RoutingRQuoteDefaultTemplateId; },
        set: function (value) {
            if (this.EntityPM.RoutingRQuoteDefaultTemplateId != value) {
                this.EntityPM.RoutingRQuoteDefaultTemplateId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    ProductTypeGeneralTabComponent = __decorate([
        core_1.Component({
            selector: 'ProductTypeGeneralTabComponent',
            moduleId: module.id,
            templateUrl: './ProductTypeGeneralTabComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs])
    ], ProductTypeGeneralTabComponent);
    return ProductTypeGeneralTabComponent;
}(BaseComponent_1.BaseComponent));
exports.ProductTypeGeneralTabComponent = ProductTypeGeneralTabComponent;
//# sourceMappingURL=ProductTypeGeneralTabComponent.js.map