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
var SessionLocator_1 = require("../../Infrastructure/Utilities/SessionLocator");
var SharedLogisticsWizardComponent = /** @class */ (function () {
    function SharedLogisticsWizardComponent() {
        this.OnCloseWindowEvent = new core_1.EventEmitter();
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
    }
    SharedLogisticsWizardComponent.prototype.ngOnInit = function () {
        this.SelectedTabCode = "GEN";
    };
    SharedLogisticsWizardComponent.prototype.SetDataContext = function (entityPM) {
    };
    SharedLogisticsWizardComponent.prototype.CloseButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    SharedLogisticsWizardComponent.prototype.SaveButtonClicked = function () {
        this.OnCloseWindowEvent.emit("Save"); //pass the Id
    };
    SharedLogisticsWizardComponent.prototype.SetWindowArgs = function (args) {
        this.TenantPM = args.TenantPM;
        // this.Run();
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], SharedLogisticsWizardComponent.prototype, "OnCloseWindowEvent", void 0);
    SharedLogisticsWizardComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'SharedLogisticsWizard',
            templateUrl: './SharedLogisticsWizardComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], SharedLogisticsWizardComponent);
    return SharedLogisticsWizardComponent;
}());
exports.SharedLogisticsWizardComponent = SharedLogisticsWizardComponent;
//# sourceMappingURL=SharedLogisticsWizardComponent.js.map