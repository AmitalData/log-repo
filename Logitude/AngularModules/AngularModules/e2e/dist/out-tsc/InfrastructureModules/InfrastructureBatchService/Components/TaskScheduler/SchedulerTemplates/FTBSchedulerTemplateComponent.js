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
var FTBSchedulerTemplateComponent = /** @class */ (function () {
    function FTBSchedulerTemplateComponent() {
        this.ObjectTableName = "TasksScheduler";
    }
    FTBSchedulerTemplateComponent.prototype.LoadComponent = function (dataContext) {
        this.DataContext = dataContext;
    };
    FTBSchedulerTemplateComponent.prototype.ExtensionLostFocus = function (input) {
        if (this.DataContext.Extension && this.DataContext.Extension.startsWith("."))
            this.DataContext.Extension = this.DataContext.Extension.substring(1, this.DataContext.Extension.length);
    };
    FTBSchedulerTemplateComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'FTBSchedulerTemplateComponent',
            templateUrl: './FTBSchedulerTemplateComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], FTBSchedulerTemplateComponent);
    return FTBSchedulerTemplateComponent;
}());
exports.FTBSchedulerTemplateComponent = FTBSchedulerTemplateComponent;
//# sourceMappingURL=FTBSchedulerTemplateComponent.js.map