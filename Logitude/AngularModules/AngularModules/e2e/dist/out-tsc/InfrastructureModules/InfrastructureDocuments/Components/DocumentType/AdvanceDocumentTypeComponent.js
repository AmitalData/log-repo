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
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
require("rxjs/add/operator/map");
var core_1 = require("@angular/core");
var AdvanceDocumentTypeComponent = /** @class */ (function () {
    function AdvanceDocumentTypeComponent() {
        this.IsHouse = false;
        this.IsDirect = false;
        this.IsMaster = false;
        this.IsInland = false;
        this.IsOcean = false;
        this.IsAir = false;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
    }
    AdvanceDocumentTypeComponent.prototype.ngOnInit = function () {
    };
    AdvanceDocumentTypeComponent.prototype.SetDataContext = function (entityPM) {
        this.EntityPM = entityPM;
        this.IsAir = this.EntityPM.IsAir;
        this.IsDirect = this.EntityPM.IsDirect;
        this.IsHouse = this.EntityPM.IsHouse;
        this.IsMaster = this.EntityPM.IsMaster;
        this.IsOcean = this.EntityPM.IsOcean;
        this.IsInland = this.EntityPM.IsInland;
        var objecttable = window.ObjectTables.filter(function (d) { return d.Name == entityPM.ObjectTableName; })[0];
        if (objecttable.Name == "Shipment") {
            this.ShowDeflut = true;
        }
        else
            this.ShowDeflut = false;
        //ObjectTablePM objectTablePm = TenantContext.Current.ObjectTable.Where(d => d.Id == EntityPm.ObjectTableId).FirstOrDefault();
        //if (objectTablePm != null) {
        //    if (objectTablePm.Name == "Shipment") {
        //        showDefaultsShipment = Visibility.Visible;
        //    }
        //    else {
        //        showDefaultsShipment = Visibility.Collapsed;
        //    }
        //}
    };
    AdvanceDocumentTypeComponent.prototype.CloseButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    AdvanceDocumentTypeComponent.prototype.SaveButtonClicked = function () {
        this.EntityPM.IsAir = this.IsAir;
        this.EntityPM.IsDirect = this.IsDirect;
        this.EntityPM.IsHouse = this.IsHouse;
        this.EntityPM.IsMaster = this.IsMaster;
        this.EntityPM.IsOcean = this.IsOcean;
        this.EntityPM.IsInland = this.IsInland;
        this.CurrentSession.CloseCurrentWindow();
    };
    AdvanceDocumentTypeComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'advancedocumentType',
            templateUrl: './AdvanceDocumentTypeComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], AdvanceDocumentTypeComponent);
    return AdvanceDocumentTypeComponent;
}());
exports.AdvanceDocumentTypeComponent = AdvanceDocumentTypeComponent;
//# sourceMappingURL=AdvanceDocumentTypeComponent.js.map