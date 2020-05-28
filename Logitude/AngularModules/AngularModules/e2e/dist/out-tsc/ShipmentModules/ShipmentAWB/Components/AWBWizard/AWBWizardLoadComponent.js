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
var ShipmentPMService_1 = require("../../../../Shipment/Services/StandardPMs/ShipmentPMService");
var EntityLastActivityService_1 = require("../../../../Infrastructure/Services/EntityLastActivityService");
var Tools_1 = require("../../../../Shipment/Tools");
var Args_1 = require("../../../../Shipment/Args");
var AWBWizardLoadComponent = /** @class */ (function () {
    function AWBWizardLoadComponent() {
        this.EntityId = null;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.isViewInited = false;
    }
    AWBWizardLoadComponent.prototype.SetWindowArgs = function (entityId) {
        this.CurrentSession.StartBusyIndicatorLoading();
        this.EntityId = entityId;
        this.Load();
    };
    AWBWizardLoadComponent.prototype.ngAfterViewInit = function () {
        this.isViewInited = true;
        this.Load();
    };
    AWBWizardLoadComponent.prototype.Load = function () {
        var _this = this;
        if (this.EntityId != null && this.isViewInited) {
            var myService = new ShipmentPMService_1.ShipmentPMService();
            myService.get(this.EntityId).subscribe(function (myResponse) {
                if (myResponse != null) {
                    if (!myResponse.HasError) {
                        _this.EntityPM = myResponse.Result;
                        if (_this.EntityPM != null) {
                            _this.ImportWizard();
                            _this.SendActivityLog();
                        }
                    }
                }
                _this.CurrentSession.StopBusyIndicator();
            });
        }
    };
    AWBWizardLoadComponent.prototype.ImportWizard = function () {
        var _this = this;
        var isFullWizard = Tools_1.ShipmentTool.IsFullAWBWizard(this.EntityPM.DirectionId);
        if (isFullWizard) {
            var myAWBWizardArgs = new Args_1.AWBWizardArgs();
            myAWBWizardArgs.EntityPM = this.EntityPM;
            myAWBWizardArgs.ShipmentLevelCode = this.EntityPM.ShipmentLevelCode;
            SessionLocator_1.SessionLocator.DynamicLoader.Load('./ShipmentModules/ShipmentAWB/Components/AWBWizard/AWBWizardComponent', this.target)
                .then(function (cmpRef) {
                cmpRef.instance.SetWindowArgs(myAWBWizardArgs);
                _this.CurrentSession.StopBusyIndicator();
            });
        }
        else {
            var myFSRWizardArgs = new Args_1.FSRWizardArgs();
            myFSRWizardArgs.EntityPM = this.EntityPM;
            myFSRWizardArgs.ShipmentLevelCode = this.EntityPM.ShipmentLevelCode;
            SessionLocator_1.SessionLocator.DynamicLoader.Load('./ShipmentModules/ShipmentAWB/Components/FSRWizard/FSRWizardComponent', this.target)
                .then(function (cmpRef) {
                cmpRef.instance.SetWindowArgs(myFSRWizardArgs);
                _this.CurrentSession.StopBusyIndicator();
            });
        }
    };
    AWBWizardLoadComponent.prototype.SendActivityLog = function () {
        var ObjectTableName = "Shipment";
        var ObjectTable = window.ObjectTables.filter(function (x) { return x.Name === ObjectTableName; })[0];
        var ObjectTableId = ObjectTable.Id;
        var myService = new EntityLastActivityService_1.EntityLastActivityService();
        myService.AddActivityLog(this.EntityId, ObjectTableId, SessionLocator_1.SessionLocator.LoggedUserId, 'V').subscribe();
    };
    __decorate([
        core_1.ViewChild('WizardView', { read: core_1.ViewContainerRef }),
        __metadata("design:type", core_1.ViewContainerRef)
    ], AWBWizardLoadComponent.prototype, "target", void 0);
    AWBWizardLoadComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './AWBWizardLoadComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], AWBWizardLoadComponent);
    return AWBWizardLoadComponent;
}());
exports.AWBWizardLoadComponent = AWBWizardLoadComponent;
//# sourceMappingURL=AWBWizardLoadComponent.js.map