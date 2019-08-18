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
var ContactListService_1 = require("../../../../Common/Services/StandardLists/ContactListService");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var btnComponentComputingPartner = /** @class */ (function () {
    function btnComponentComputingPartner(CD) {
        this.CD = CD;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.InUseVisibile = true;
    }
    btnComponentComputingPartner.prototype.MoreDetails = function () {
        var _this = this;
        var ServiceContact = new ContactListService_1.ContactListService();
        this.CurrentSession.StartBusyIndicatorLoading();
        ServiceContact.getSingle(this.rowData.CreatedByUserId).subscribe(function (res) {
            if (!res.HasError) {
                if (res.Result != null)
                    _this.rowData.CreatedByUserName = res.Result.EnglishName;
            }
            ServiceContact.getSingle(_this.rowData.UpdatedByUserId).subscribe(function (res) {
                if (!res.HasError) {
                    if (res.Result != null)
                        _this.rowData.UpdatedByUserName = res.Result.EnglishName;
                }
                _this.CurrentSession.StopBusyIndicator();
                var logWindow = new LogitudeWindow_1.LogitudeWindow();
                logWindow.Width = 500;
                logWindow.Height = 300;
                logWindow.Title = "More Details";
                logWindow.WindowArgs = { entityPM: _this.rowData };
                logWindow.Show('./InfrastructureModules/InfrastructureComputingPartner/Components/TranslationDetailsComponent');
            });
        });
    };
    btnComponentComputingPartner.prototype.setVariables = function (rowData, fieldName) {
        this.rowData = rowData;
        this.fieldName = fieldName;
        //this.Check();
        this.InUseVisibile = this.rowData.InUse;
        var isDestroyed = this.CD['destroyed'];
        if (!isDestroyed) {
            this.CD.detectChanges();
        }
    };
    btnComponentComputingPartner = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'btnComponentComputingPartner',
            templateUrl: './btnComponentComputingPartner.html',
        }),
        __metadata("design:paramtypes", [core_1.ChangeDetectorRef])
    ], btnComponentComputingPartner);
    return btnComponentComputingPartner;
}());
exports.btnComponentComputingPartner = btnComponentComputingPartner;
//# sourceMappingURL=btnComponentComputingPartner.js.map