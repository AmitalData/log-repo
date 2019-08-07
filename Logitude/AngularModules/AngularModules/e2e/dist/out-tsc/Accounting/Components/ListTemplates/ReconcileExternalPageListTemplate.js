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
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var ObjectsLocator_1 = require("../../../Infrastructure/Locators/ObjectsLocator");
var LogitudeWindow_1 = require("../../../Controls/Windows/LogitudeWindow");
var EntityArgs_1 = require("../../../Infrastructure/DataContracts/EntityArgs");
var TextCodeTranslator_1 = require("../../../Infrastructure/Utilities/TextCodeTranslator");
var ReconcileExternalPageListTemplate = /** @class */ (function () {
    function ReconcileExternalPageListTemplate(CD) {
        this.CD = CD;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.isRTL = false;
        if (ObjectsLocator_1.ObjectsLocator.GlobalSetting)
            this.isRTL = (ObjectsLocator_1.ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
    }
    ReconcileExternalPageListTemplate.prototype.setVariables = function (rowData, fieldName, MyAdditionalData) {
        this.rowData = rowData;
        this.fieldName = fieldName;
        this.AdditionalData = MyAdditionalData;
        var isDestroyed = this.CD['destroyed'];
        if (!isDestroyed) {
            this.CD.detectChanges();
        }
    };
    ReconcileExternalPageListTemplate.prototype.ViewEvents = function (line) {
        var entityPM = this.rowData;
        var windowArgs = new EntityArgs_1.EntityArgs();
        windowArgs.ObjectTableName = "ReconcileExternalPage";
        windowArgs.EntityPM = entityPM;
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Width = 950;
        logWindow.Height = 600;
        logWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("ReconcileExternalPage") + " " + TextCodeTranslator_1.TextCodeTranslator.Translate("AccountingPeriod.TH.Events");
        logWindow.WindowArgs = windowArgs;
        this.CurrentSession.SessionEvent.emit("noselect");
        logWindow.Show('./Accounting/Components/EditTabs/BankAccount/BankPageEventsComponent');
    };
    ReconcileExternalPageListTemplate.prototype.Abs = function (number) {
        return number < 0 ? number * -1 : number;
    };
    ReconcileExternalPageListTemplate = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './ReconcileExternalPageListTemplate.html',
        }),
        __metadata("design:paramtypes", [core_1.ChangeDetectorRef])
    ], ReconcileExternalPageListTemplate);
    return ReconcileExternalPageListTemplate;
}());
exports.ReconcileExternalPageListTemplate = ReconcileExternalPageListTemplate;
//# sourceMappingURL=ReconcileExternalPageListTemplate.js.map