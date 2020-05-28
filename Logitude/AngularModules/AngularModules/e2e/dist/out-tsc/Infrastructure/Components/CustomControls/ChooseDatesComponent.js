"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
Object.defineProperty(exports, "__esModule", { value: true });
var core_1 = require("@angular/core");
var SessionLocator_1 = require("../../Utilities/SessionLocator");
var ChooseDatesComponent = /** @class */ (function () {
    function ChooseDatesComponent() {
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.ValidationErrorsList = [];
        this.DateSelected = new core_1.EventEmitter();
    }
    Object.defineProperty(ChooseDatesComponent.prototype, "SelectedToDate", {
        get: function () { return this.selectedToDate; },
        set: function (newValue) {
            this.selectedToDate = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ChooseDatesComponent.prototype, "SelectedFromDate", {
        get: function () { return this.selectedFromDate; },
        set: function (newValue) {
            this.selectedFromDate = newValue;
        },
        enumerable: true,
        configurable: true
    });
    ChooseDatesComponent.prototype.OnSelectedFromDateChanged = function (value) {
        this.SelectedFromDate = value.SelectedDate;
    };
    ChooseDatesComponent.prototype.OnSelectedToDateChanged = function (value) {
        this.SelectedToDate = value.SelectedDate;
    };
    ChooseDatesComponent.prototype.OKbtnClick = function () {
        this.ValidationErrorsList = this.validateDates();
        if (this.ValidationErrorsList.length == 0) {
            this.DateSelected.emit({ From: this.SelectedFromDate, To: this.SelectedToDate });
            this.CurrentSession.CloseCurrentWindow();
        }
    };
    ChooseDatesComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    ChooseDatesComponent.prototype.validateDates = function () {
        var ValidationErrors = [];
        if (this.SelectedFromDate != null && this.SelectedToDate != null) {
            if (this.SelectedFromDate.getTime() > this.SelectedToDate.getTime()) {
                ValidationErrors.push("ToDate should be greater than or equal to FromDate value.");
            }
        }
        else {
            ValidationErrors.push("From date and to date are required !");
        }
        return ValidationErrors;
    };
    ChooseDatesComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'ChooseDatesComponent',
            templateUrl: './ChooseDatesComponent.html',
        })
    ], ChooseDatesComponent);
    return ChooseDatesComponent;
}());
exports.ChooseDatesComponent = ChooseDatesComponent;
//# sourceMappingURL=ChooseDatesComponent.js.map