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
var ControlsIdCounter_1 = require("../../Utilities/ControlsIdCounter");
var LogBooleanComponent = /** @class */ (function () {
    function LogBooleanComponent() {
        this.ControlId = null;
        this.ObjectFieldName = null;
        this.ObjectTableName = null;
    }
    LogBooleanComponent.prototype.CheckIfExists = function (IdCom) {
        var element = document.getElementById(IdCom);
        if (element != null && element != undefined) {
            return true;
        }
        return false;
    };
    LogBooleanComponent.prototype.SetControlIds = function (baseIdCombination) {
        this.ControlId = baseIdCombination;
    };
    LogBooleanComponent.prototype.FilterItemClicked = function (key) {
        if (this.FilterSelectedValue == key) {
            this.FilterSelectedValue = null;
            this.DataContext[this.ObjectFieldName] = null;
        }
        else {
            this.FilterSelectedValue = key;
            if (this.FilterSelectedValue == 'yes') {
                this.DataContext[this.ObjectFieldName] = "True";
            }
            else if (this.FilterSelectedValue == 'no') {
                this.DataContext[this.ObjectFieldName] = "False";
            }
        }
    };
    LogBooleanComponent.prototype.ngOnInit = function () {
        this.counterId = null;
        var baseIdCombination = null;
        if (this.ObjectTableName) {
            baseIdCombination = this.ObjectTableName + "_" + this.ObjectFieldName;
        }
        else {
            baseIdCombination = this.ObjectFieldName;
        }
        if (this.CheckIfExists(baseIdCombination)) {
            this.counterId = ControlsIdCounter_1.ControlsIdCounter.GetNextControlIdCounter(baseIdCombination);
        }
        if (this.counterId != null) {
            baseIdCombination = baseIdCombination + '_' + this.counterId.toString();
        }
        this.SetControlIds(baseIdCombination);
        if (this.DataContext[this.ObjectFieldName] == "True") {
            this.FilterSelectedValue = 'yes';
        }
        else if (this.DataContext[this.ObjectFieldName] == "False") {
            this.FilterSelectedValue = 'no';
        }
        else {
            this.FilterSelectedValue = null;
        }
    };
    __decorate([
        core_1.Input(),
        __metadata("design:type", String)
    ], LogBooleanComponent.prototype, "ObjectFieldName", void 0);
    __decorate([
        core_1.Input(),
        __metadata("design:type", String)
    ], LogBooleanComponent.prototype, "ObjectTableName", void 0);
    __decorate([
        core_1.Input(),
        __metadata("design:type", Object)
    ], LogBooleanComponent.prototype, "DataContext", void 0);
    __decorate([
        core_1.Input(),
        __metadata("design:type", Number)
    ], LogBooleanComponent.prototype, "Width", void 0);
    LogBooleanComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'LogBoolean',
            templateUrl: './LogBooleanComponent.html',
        })
    ], LogBooleanComponent);
    return LogBooleanComponent;
}());
exports.LogBooleanComponent = LogBooleanComponent;
//# sourceMappingURL=LogBooleanComponent.js.map