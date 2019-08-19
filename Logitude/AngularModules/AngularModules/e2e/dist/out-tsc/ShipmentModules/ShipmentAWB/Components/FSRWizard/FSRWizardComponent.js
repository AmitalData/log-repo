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
var LocationDirective_1 = require("../../../../Infrastructure/Utilities/LocationDirective");
var EntityArgs_1 = require("../../../../Infrastructure/DataContracts/EntityArgs");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var Tools_1 = require("../../../../Infrastructure/Tools");
var ShipmentPMService_1 = require("../../../../Shipment/Services/StandardPMs/ShipmentPMService");
var FSRWizardComponent = /** @class */ (function () {
    function FSRWizardComponent(entityArgs) {
        this.entityArgs = entityArgs;
        this.LoadCompleted = new core_1.EventEmitter();
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.isViewInited = false;
        this.myService = null;
        this.ValidationErrorsList = [];
        this.ValidationWarningsList = [];
    }
    FSRWizardComponent.prototype.SetWindowArgs = function (windowArgs) {
        this.EntityPM = windowArgs.EntityPM;
        this.ShipmentLevelCode = windowArgs.ShipmentLevelCode;
        this.ObjectTableName = this.ShipmentLevelCode == "C" ? "Master" : "Shipment";
        this.entityArgs.EntityPM = this.EntityPM;
        this.entityArgs.ObjectTableName = this.ObjectTableName;
        this.InitializeWizard();
    };
    FSRWizardComponent.prototype.ngAfterViewInit = function () {
        this.isViewInited = true;
        this.InitializeWizard();
    };
    FSRWizardComponent.prototype.InitializeWizard = function () {
        var _this = this;
        if (this.EntityPM != null && this.isViewInited) {
            var myLocation = this.AllLocations.toArray().filter(function (d) { return d.Code == 'OVE'; })[0];
            if (myLocation != null) {
                SessionLocator_1.SessionLocator.DynamicLoader.Load("./ShipmentModules/ShipmentAWB/Components/AWBWizard/Overview/AWBOverviewTabComponent", myLocation.viewContainerRef)
                    .then(function (cmpRef) {
                    cmpRef.instance.InitTab(_this.EntityPM, _this);
                });
            }
        }
    };
    FSRWizardComponent.prototype.ValidateFSR = function () {
        var isValid = true;
        var warnings = [];
        var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.FieldIsRequired");
        if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.MainCarriageCarrierId)) {
            warnings.push(msg.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Routings.Airline")));
        }
        if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Master) && Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.MAWBStackNumber)) {
            warnings.push(msg.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Routings.MAWB")));
        }
        this.ValidationWarningsList = warnings;
        if (warnings.length > 0) {
            isValid = false;
        }
        return isValid;
    };
    FSRWizardComponent.prototype.CloseButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    FSRWizardComponent.prototype.ReloadEntity = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicatorLoading();
        if (this.myService == null) {
            this.myService = new ShipmentPMService_1.ShipmentPMService();
        }
        this.myService.get(this.EntityPM.Id).subscribe(function (myResponse) {
            if (myResponse != null) {
                if (!myResponse.HasError) {
                    _this.EntityPM = myResponse.Result;
                    _this.LoadCompleted.emit(true);
                    _this.CurrentSession.StopBusyIndicator();
                }
                else {
                    _this.ValidationErrorsList = myResponse.ErrorsArray;
                    _this.LoadCompleted.emit(false);
                    _this.CurrentSession.StopBusyIndicator();
                }
            }
        });
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", core_1.EventEmitter)
    ], FSRWizardComponent.prototype, "LoadCompleted", void 0);
    __decorate([
        core_1.ViewChildren(LocationDirective_1.LocationDirective),
        __metadata("design:type", core_1.QueryList)
    ], FSRWizardComponent.prototype, "AllLocations", void 0);
    FSRWizardComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './FSRWizardComponent.html',
            providers: [EntityArgs_1.EntityArgs],
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs])
    ], FSRWizardComponent);
    return FSRWizardComponent;
}());
exports.FSRWizardComponent = FSRWizardComponent;
//# sourceMappingURL=FSRWizardComponent.js.map