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
var ReconcileEventManager_1 = require("../../Utilities/ReconcileEventManager");
var ObjectsLocator_1 = require("../../../Infrastructure/Locators/ObjectsLocator");
var ReconcileExternalPageLineListTemplate = /** @class */ (function () {
    function ReconcileExternalPageLineListTemplate(CD) {
        this.CD = CD;
        this.isRTL = false;
        this.checkBoxState = false;
        if (ObjectsLocator_1.ObjectsLocator.GlobalSetting)
            this.isRTL = (ObjectsLocator_1.ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
    }
    Object.defineProperty(ReconcileExternalPageLineListTemplate.prototype, "CheckBoxState", {
        get: function () { return this.checkBoxState; },
        set: function (isChecked) {
            console.log("Changed to: ", isChecked);
            this.checkBoxState = isChecked;
        },
        enumerable: true,
        configurable: true
    });
    ReconcileExternalPageLineListTemplate.prototype.setVariables = function (rowData, fieldName, MyAdditionalData) {
        this.rowData = rowData;
        this.fieldName = fieldName;
        this.AdditionalData = MyAdditionalData;
        var isDestroyed = this.CD['destroyed'];
        if (!isDestroyed) {
            this.CD.detectChanges();
        }
    };
    ReconcileExternalPageLineListTemplate.prototype.CheckBoxClicked = function (checked) {
        console.log("clicked: ", checked);
        this.rowData['IsChecked'] = checked;
        ReconcileEventManager_1.ReconcileEventManager.CheckBoxChecked.emit({ line: this.rowData, isChecked: checked, RowIndex: this.AdditionalData.rowIndex });
    };
    ReconcileExternalPageLineListTemplate.prototype.BankCheckBoxClicked = function (checked) {
        console.log("clicked: ", checked);
        this.rowData['IsChecked'] = checked;
        ReconcileEventManager_1.ReconcileEventManager.BankCheckBoxChecked.emit({ line: this.rowData, isChecked: checked, RowIndex: this.AdditionalData.rowIndex });
    };
    ReconcileExternalPageLineListTemplate.prototype.Abs = function (number) {
        return number < 0 ? number * -1 : number;
    };
    ReconcileExternalPageLineListTemplate = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './ReconcileExternalPageLineListTemplate.html',
        }),
        __metadata("design:paramtypes", [core_1.ChangeDetectorRef])
    ], ReconcileExternalPageLineListTemplate);
    return ReconcileExternalPageLineListTemplate;
}());
exports.ReconcileExternalPageLineListTemplate = ReconcileExternalPageLineListTemplate;
//# sourceMappingURL=ReconcileExternalPageLineListTemplate.js.map