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
var DeclarationDocsInTabComponent = /** @class */ (function () {
    function DeclarationDocsInTabComponent(entityArgs) {
        this.entityArgs = entityArgs;
        this.EntityPM = null;
        this.ObjectTableName = "Customs.Declaration";
    }
    DeclarationDocsInTabComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.EntityPM = this.entityArgs.EntityPM;
        if (this.EntityPM != null) {
            var table = window.ObjectTables.filter(function (d) { return d.Name == _this.ObjectTableName; })[0];
            if (table)
                this.ObjectTableId = table.Id;
            this.EntityId = this.EntityPM.Id;
        }
    };
    DeclarationDocsInTabComponent = __decorate([
        core_1.Component({
            selector: 'TicketDocsInTabComponent',
            moduleId: module.id,
            templateUrl: './DeclarationDocsInTabComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs])
    ], DeclarationDocsInTabComponent);
    return DeclarationDocsInTabComponent;
}());
exports.DeclarationDocsInTabComponent = DeclarationDocsInTabComponent;
//# sourceMappingURL=DeclarationDocsInTabComponent.js.map