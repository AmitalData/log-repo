"use strict";
var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    }
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
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
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var TimeManagementDomainService_1 = require("../../../Services/TimeManagementDomainService");
var VacationsComponent = /** @class */ (function (_super) {
    __extends(VacationsComponent, _super);
    function VacationsComponent() {
        var _this = _super.call(this) || this;
        _this.Years = [];
        _this.Types = [];
        _this.ItemsSource = [];
        _this.mySerive = null;
        _this.selectedYear = null;
        _this.selectedType = null;
        _this.Holidays = 0;
        _this.Vacations = 0;
        _this.HalfVacations = 0;
        _this.UnpaidVacations = 0;
        _this.SicknessVacations = 0;
        _this.SickLeaves = "0";
        _this.mySerive = new TimeManagementDomainService_1.TimeManagementDomainService();
        _this.Years = [];
        _this.Types = [];
        for (var i = new Date().getFullYear(); i >= 2018; i--) {
            _this.Years.push(i);
        }
        _this.Types.push("Holidays");
        _this.Types.push("Vacations");
        _this.Types.push("Half Vacations");
        _this.Types.push("Unpaid Vacations");
        _this.Types.push("Sickness Vacations");
        _this.Types.push("Sick Leaves");
        _this.selectedYear = _this.Years[0];
        _this.selectedType = _this.Types[0];
        return _this;
    }
    Object.defineProperty(VacationsComponent.prototype, "SelectedYear", {
        get: function () { return this.selectedYear; },
        set: function (value) {
            if (this.selectedYear != value) {
                this.selectedYear = value;
                this.LoadAllScreenData();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(VacationsComponent.prototype, "SelectedType", {
        get: function () { return this.selectedType; },
        set: function (value) {
            if (this.selectedType != value) {
                this.selectedType = value;
                this.GetVacationsDetails();
            }
        },
        enumerable: true,
        configurable: true
    });
    VacationsComponent.prototype.InitTab = function () {
        this.LoadAllScreenData();
    };
    VacationsComponent.prototype.RefreshButtonClicked = function () {
        this.LoadAllScreenData();
    };
    VacationsComponent.prototype.LoadAllScreenData = function () {
        this.GetVacationsSummary();
        this.GetVacationsDetails();
    };
    VacationsComponent.prototype.GetVacationsSummary = function () {
        var _this = this;
        this.Holidays = 0;
        this.Vacations = 0;
        this.HalfVacations = 0;
        this.UnpaidVacations = 0;
        this.SicknessVacations = 0;
        this.SickLeaves = "0";
        this.mySerive.GetVacationsSummary(this.SelectedYear).subscribe(function (myResponse) {
            if (myResponse.HasError) {
                //this.ShowMessage(myResponse.ErrorsArray[0]);
            }
            else {
                _this.Holidays = myResponse.Result.Holidays;
                _this.Vacations = myResponse.Result.Vacations;
                _this.HalfVacations = myResponse.Result.HalfVacations;
                _this.UnpaidVacations = myResponse.Result.UnpaidVacations;
                _this.SicknessVacations = myResponse.Result.SicknessVacations;
                _this.SickLeaves = myResponse.Result.SickLeaves;
            }
        });
    };
    VacationsComponent.prototype.GetVacationsDetails = function () {
        var _this = this;
        this.ItemsSource = [];
        this.mySerive.GetVacationsDetails(this.SelectedYear, this.SelectedType).subscribe(function (myResponse) {
            _this.ItemsSource = [];
            if (myResponse.HasError) {
                //this.ShowMessage(myResponse.ErrorsArray[0]);
            }
            else {
                _this.ItemsSource = myResponse.Result;
            }
        });
    };
    VacationsComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './VacationsComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], VacationsComponent);
    return VacationsComponent;
}(BaseComponent_1.BaseComponent));
exports.VacationsComponent = VacationsComponent;
//# sourceMappingURL=VacationsComponent.js.map