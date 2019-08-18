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
var ColumnCheckBoxComponent = /** @class */ (function () {
    function ColumnCheckBoxComponent(CD) {
        this.CD = CD;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
    }
    ColumnCheckBoxComponent.prototype.setVariables = function (rowData, fieldName) {
        this.rowData = rowData;
        this.fieldName = fieldName.split(",");
        this.packageCode = this.fieldName[0];
        this.columnIndex = this.fieldName[1];
        this.SetIsChecked();
        var isDestroyed = this.CD['destroyed'];
        if (!isDestroyed) {
            this.CD.detectChanges();
        }
    };
    ColumnCheckBoxComponent.prototype.SetIsChecked = function () {
        switch (this.columnIndex) {
            case "1":
                {
                    this.isChecked = this.rowData.IsChecked1;
                    break;
                }
            case "2":
                {
                    this.isChecked = this.rowData.IsChecked2;
                    break;
                }
            case "3":
                {
                    this.isChecked = this.rowData.IsChecked3;
                    break;
                }
            case "4":
                {
                    this.isChecked = this.rowData.IsChecked4;
                    break;
                }
            case "5":
                {
                    this.isChecked = this.rowData.IsChecked5;
                    break;
                }
            case "6":
                {
                    this.isChecked = this.rowData.IsChecked6;
                    break;
                }
            case "7":
                {
                    this.isChecked = this.rowData.IsChecked7;
                    break;
                }
            case "8":
                {
                    this.isChecked = this.rowData.IsChecked8;
                    break;
                }
            case "9":
                {
                    this.isChecked = this.rowData.IsChecked9;
                    break;
                }
            case "10":
                {
                    this.isChecked = this.rowData.IsChecked10;
                    break;
                }
        }
    };
    Object.defineProperty(ColumnCheckBoxComponent.prototype, "IsChecked", {
        get: function () { return this.isChecked; },
        set: function (newValue) {
            if (this.isChecked != newValue) {
                this.isChecked = newValue;
                if (newValue) {
                    this.CurrentSession.PseventRowSelectEvent.emit({ Name: "Add", User: this.rowData, PackageCode: this.packageCode });
                }
                else {
                    this.CurrentSession.PseventRowSelectEvent.emit({ Name: "Remove", User: this.rowData, PackageCode: this.packageCode });
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    ColumnCheckBoxComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './ColumnCheckBoxComponent.html',
        }),
        __metadata("design:paramtypes", [core_1.ChangeDetectorRef])
    ], ColumnCheckBoxComponent);
    return ColumnCheckBoxComponent;
}());
exports.ColumnCheckBoxComponent = ColumnCheckBoxComponent;
//# sourceMappingURL=ColumnCheckBoxComponent.js.map