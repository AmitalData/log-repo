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
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var btnComponentComputingPartnerEdit = /** @class */ (function () {
    function btnComponentComputingPartnerEdit(CD) {
        this.CD = CD;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.InUseVisibile = true;
    }
    btnComponentComputingPartnerEdit.prototype.Edit = function () {
        var _this = this;
        var serviceEntity = new EntityResourceService_1.EntityResourceService();
        serviceEntity.getEntityResourceByTableName("ComputingPartnerTranslation", 0).subscribe(function (p) {
            var logWindow = new LogitudeWindow_1.LogitudeWindow();
            logWindow.Width = 500;
            logWindow.Height = 300;
            logWindow.Title = "Edit " + _this.rowData.ObjectTableName + " Translation";
            logWindow.WindowArgs = { entityPM: _this.rowData };
            logWindow.Show('./InfrastructureModules/InfrastructureComputingPartner/Components/EditTranslationComputingPartners');
            logWindow.ComponentLoaded.subscribe(function (cmpRef) {
                cmpRef.BackCompleted.subscribe(function ($event1) {
                    _this.OnBackFromEdit(_this.entityId, _this.rowData);
                });
            });
        });
    };
    btnComponentComputingPartnerEdit.prototype.OnBackFromEdit = function (selectedEntityId, $event) {
        this.CurrentSession.PseventRowSelectEvent.emit({ Name: 'btnComponentComputingPartnerEdit', Value: this.rowData, RowIndex: this.AdditionalData.rowIndex });
    };
    btnComponentComputingPartnerEdit.prototype.setVariables = function (rowData, fieldName, additionalData) {
        this.rowData = rowData;
        this.fieldName = fieldName;
        this.AdditionalData = additionalData;
        //this.Check();
        this.InUseVisibile = this.rowData.InUse;
        var isDestroyed = this.CD['destroyed'];
        if (!isDestroyed) {
            this.CD.detectChanges();
        }
    };
    btnComponentComputingPartnerEdit = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'btnComponentComputingPartnerEdit',
            templateUrl: './btnComponentComputingPartnerEdit.html',
        }),
        __metadata("design:paramtypes", [core_1.ChangeDetectorRef])
    ], btnComponentComputingPartnerEdit);
    return btnComponentComputingPartnerEdit;
}());
exports.btnComponentComputingPartnerEdit = btnComponentComputingPartnerEdit;
//# sourceMappingURL=btnComponentComputingPartnerEdit.js.map