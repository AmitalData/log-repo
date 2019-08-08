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
var EntityArgs_1 = require("../../../../../Infrastructure/DataContracts/EntityArgs");
var ShippingLineDocsInTabComponent = /** @class */ (function () {
    function ShippingLineDocsInTabComponent(entityArgs) {
        this.entityArgs = entityArgs;
        this.ObjectTableName = "ShippingLine";
    }
    ShippingLineDocsInTabComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.EntityPM = this.entityArgs.EntityPM;
        if (this.EntityPM != null) {
            var table = window.ObjectTables.filter(function (d) { return d.Name == _this.ObjectTableName; })[0];
            if (table)
                this.ObjectTableId = table.Id;
            this.EntityId = this.EntityPM.Id;
        }
    };
    ShippingLineDocsInTabComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './ShippingLineDocsInTabComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs])
    ], ShippingLineDocsInTabComponent);
    return ShippingLineDocsInTabComponent;
}());
exports.ShippingLineDocsInTabComponent = ShippingLineDocsInTabComponent;
//# sourceMappingURL=ShippingLineDocsInTabComponent.js.map