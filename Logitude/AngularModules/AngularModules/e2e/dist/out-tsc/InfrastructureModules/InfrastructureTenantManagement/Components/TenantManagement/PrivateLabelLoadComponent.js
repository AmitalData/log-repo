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
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var TenantManagmentPrivateLabelsPMService_1 = require("../../../../Infrastructure/Services/StandardPMs/TenantManagmentPrivateLabelsPMService");
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var PrivateLabelLoadComponent = /** @class */ (function () {
    function PrivateLabelLoadComponent() {
        this.EntityId = null;
        this._entityResourceService = new EntityResourceService_1.EntityResourceService();
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.isViewInited = false;
    }
    PrivateLabelLoadComponent.prototype.SetWindowArgs = function (entityId) {
        this.CurrentSession.StartBusyIndicatorLoading();
        this.EntityId = entityId;
        this.Load();
    };
    PrivateLabelLoadComponent.prototype.ngAfterViewInit = function () {
        this.isViewInited = true;
        this.Load();
    };
    PrivateLabelLoadComponent.prototype.Load = function () {
        var _this = this;
        if (this.EntityId != null && this.isViewInited) {
            var myService = new TenantManagmentPrivateLabelsPMService_1.TenantManagmentPrivateLabelsPMService();
            myService.get(this.EntityId).subscribe(function (myResult) {
                var myResponse = myResult;
                if (!myResponse.HasError) {
                    _this.EntityPM = myResponse.Result;
                    _this.ImportWizard();
                }
                _this.CurrentSession.StopBusyIndicator();
            });
        }
    };
    PrivateLabelLoadComponent.prototype.ImportWizard = function () {
        var _this = this;
        var Args = { Entity: this.EntityPM };
        this._entityResourceService.getEntityResourceByTableName("TenantManagmentPrivateLabels", 0).subscribe(function (response) {
            SessionLocator_1.SessionLocator.DynamicLoader.Load('./InfrastructureModules/InfrastructureTenantManagement/Components/TenantManagement/AddEditPrivateLabelsComponent', _this.target)
                .then(function (cmpRef) {
                cmpRef.instance.SetWindowArgs(Args);
                _this.CurrentSession.StopBusyIndicator();
            });
        });
    };
    __decorate([
        core_1.ViewChild('WizardView', { read: core_1.ViewContainerRef }),
        __metadata("design:type", core_1.ViewContainerRef)
    ], PrivateLabelLoadComponent.prototype, "target", void 0);
    PrivateLabelLoadComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './PrivateLabelLoadComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], PrivateLabelLoadComponent);
    return PrivateLabelLoadComponent;
}());
exports.PrivateLabelLoadComponent = PrivateLabelLoadComponent;
//# sourceMappingURL=PrivateLabelLoadComponent.js.map