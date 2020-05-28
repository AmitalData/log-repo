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
var BaseComponent_1 = require("../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var AccountingPeriodPMService_1 = require("../../Services/StandardPMs/AccountingPeriodPMService");
var LedgerTransactionListService_1 = require("../../Services/StandardLists/LedgerTransactionListService");
var Tools_1 = require("../../../Infrastructure/Tools");
var ApiQueryFilters_1 = require("../../../Infrastructure/DataContracts/ApiQueryFilters");
var TextCodeTranslator_1 = require("../../../Infrastructure/Utilities/TextCodeTranslator");
var EditAccountingPeriodComponent = /** @class */ (function (_super) {
    __extends(EditAccountingPeriodComponent, _super);
    function EditAccountingPeriodComponent() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.ObjectTableName = "AccountingPeriod";
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.accountingPeriodPMService = new AccountingPeriodPMService_1.AccountingPeriodPMService();
        _this.transactionsService = new LedgerTransactionListService_1.LedgerTransactionListService();
        _this.UIProperties.SetEnabled("Year", _this.ObjectTableName, false);
        _this.UIProperties.SetEnabled("PeriodTypeCode", _this.ObjectTableName, false);
        _this.UIProperties.SetEnabled("OpenMonth", _this.ObjectTableName, false);
        _this.UIProperties.SetEnabled("ClosedMonth", _this.ObjectTableName, false);
        return _this;
    }
    EditAccountingPeriodComponent.prototype.SetWindowArgs = function (args) {
        this.EntityId = args.EntityId;
        this.accountingPeriod = args.AccountingRow;
        this.Run();
    };
    EditAccountingPeriodComponent.prototype.Run = function () {
        var _this = this;
        this.accountingPeriodPMService.get(this.EntityId).subscribe(function (myResult) {
            var result = myResult.Result;
            if (!Tools_1.AppTool.IsNullOrEmpty(result)) {
                _this.EntityPM = result;
                _this.oldClosedMonth = _this.EntityPM.ClosedMonth;
            }
            else {
                console.log("cannot find the entity!!");
            }
        });
    };
    Object.defineProperty(EditAccountingPeriodComponent.prototype, "Year", {
        get: function () { return this.EntityPM.Year; },
        set: function (value) {
            if (this.EntityPM.Year != value) {
                this.EntityPM.Year = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(EditAccountingPeriodComponent.prototype, "PeriodTypeCode", {
        get: function () { return this.EntityPM.PeriodTypeCode; },
        set: function (value) {
            if (this.EntityPM.PeriodTypeCode != value) {
                this.EntityPM.PeriodTypeCode = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(EditAccountingPeriodComponent.prototype, "OpenMonth", {
        get: function () { return this.EntityPM.OpenMonth; },
        set: function (value) {
            if (this.EntityPM.OpenMonth != value) {
                this.EntityPM.OpenMonth = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(EditAccountingPeriodComponent.prototype, "ClosedMonth", {
        get: function () { return this.EntityPM.ClosedMonth; },
        set: function (value) {
            if (this.EntityPM.ClosedMonth != value) {
                this.EntityPM.ClosedMonth = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    EditAccountingPeriodComponent.prototype.OkButtonClicked = function () {
        var errors = [];
        //Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        this.ValidationErrorsList = errors;
        if (this.ValidationErrorsList.length == 0) {
            this.SubmitChanges();
        }
    };
    EditAccountingPeriodComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    EditAccountingPeriodComponent.prototype.SubmitChanges = function () {
        var _this = this;
        this.accountingPeriodPMService.update(this.EntityPM).subscribe(function (myResult) {
            var mm = myResult;
            if (!mm.HasError) {
                _this.CurrentSession.CloseCurrentWindowEmit("ok");
            }
            else {
                _this.ValidationErrorsList = mm.ErrorsArray;
                _this.CurrentSession.StopBusyIndicator();
            }
        });
    };
    EditAccountingPeriodComponent.prototype.IncrementOpen = function () {
        this.ValidationErrorsList = [];
        if (!Tools_1.AppTool.IsNullOrEmpty(this.OpenMonth)) {
            if (this.OpenMonth < 12) {
                // begin: invoice row logic
                if (this.EntityPM.PeriodTypeCode == "2") { //2-invoice
                    if (this.OpenMonth + 1 > this.accountingPeriod.OpenMonth) {
                        return;
                    }
                }
                //end
                this.OpenMonth++;
            }
        }
    };
    EditAccountingPeriodComponent.prototype.DecrementOpen = function () {
        var _this = this;
        this.ValidationErrorsList = [];
        if (!Tools_1.AppTool.IsNullOrEmpty(this.OpenMonth)) {
            if (this.OpenMonth > 0 && (this.OpenMonth > this.ClosedMonth + 1 || this.ClosedMonth == undefined)) {
                var endDayNumber = new Date(new Date().getFullYear(), this.OpenMonth + 1, 0).getDate();
                var filters = new ApiQueryFilters_1.ApiQueryFilters(true);
                var fromDate = new Date();
                fromDate.setFullYear(this.Year);
                fromDate.setMonth(this.OpenMonth - 1);
                fromDate.setDate(1);
                var toDate = new Date();
                toDate.setFullYear(this.Year);
                toDate.setMonth(this.OpenMonth - 1);
                toDate.setDate(endDayNumber);
                filters.addAdditionalFilter("AccountingDate", fromDate, toDate, null, "Between", false, false, false, "number");
                this.transactionsService.getByFilters(filters).subscribe(function (myResponse) {
                    if (myResponse != null) {
                        if (!myResponse.HasError) {
                            var myResult = myResponse.Result;
                            if (myResult.length > 0) { // transactions exist
                                _this.ValidationErrorsList = [];
                                _this.ValidationErrorsList.push(TextCodeTranslator_1.TextCodeTranslator.Translate("AccountingPeriod.O.CantCancelOpenMonth"));
                            }
                            else {
                                //if (this.OpenMonth > 0 && this.OpenMonth > this.ClosedMonth + 1) {
                                _this.OpenMonth--;
                                //}
                            }
                        }
                    }
                });
            }
        }
    };
    EditAccountingPeriodComponent.prototype.IncrementClosed = function () {
        this.ValidationErrorsList = [];
        if (!Tools_1.AppTool.IsNullOrEmpty(this.ClosedMonth)) {
            // begin: invoice row logic
            if (this.EntityPM.PeriodTypeCode == "2") { //2-invoice
                if (this.ClosedMonth + 1 < this.accountingPeriod.ClosedMonth) {
                    return;
                }
            }
            //end
            if (this.ClosedMonth == 12)
                return;
            var endDayNumber = new Date(new Date().getFullYear(), new Date().getMonth() + 1, 0).getDate();
            if (((this.ClosedMonth == new Date().getMonth() + 1) && (new Date()).getDay() < endDayNumber) && (this.Year == (new Date().getFullYear()))) { // current month && not ended
                this.ValidationErrorsList = [];
                this.ValidationErrorsList.push(TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.O.MonthNotEndedCantClosed")); //"The month is not ended, can’t be closed");
            }
            else {
                if (this.ClosedMonth < 12 && this.ClosedMonth < this.OpenMonth - 1) {
                    this.ClosedMonth++;
                }
                else if (this.OpenMonth == 12 && this.ClosedMonth == 11) {
                    this.ClosedMonth++;
                }
            }
        }
        else {
            if (this.OpenMonth > 1) {
                if (((new Date().getMonth() + 1) == 1) && (this.Year == (new Date().getFullYear()))) { // current month
                    this.ValidationErrorsList = [];
                    this.ValidationErrorsList.push(TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.O.MonthNotEndedCantClosed")); //"The month is not ended, can’t be closed");
                }
                else {
                    this.ClosedMonth = (this.EntityPM.PeriodTypeCode == "2" ? this.accountingPeriod.ClosedMonth : 1);
                }
            }
        }
    };
    EditAccountingPeriodComponent.prototype.DecrementClosed = function () {
        // // incase: we want to allow cancelling before save the period - DON'T DELETE!
        // if(this.ClosedMonth){
        //     if(this.ClosedMonth == 1 && !this.oldClosedMonth)
        //         this.ClosedMonth = null
        //     if(this.ClosedMonth > 1 && (this.ClosedMonth > this.oldClosedMonth || !this.oldClosedMonth))
        //         this.ClosedMonth--;
        // }
        if (this.ClosedMonth) {
            if (this.ClosedMonth == 1)
                this.ClosedMonth = null;
            else if (this.ClosedMonth > 1)
                this.ClosedMonth--;
        }
    };
    EditAccountingPeriodComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'EditAccountingPeriodComponent',
            templateUrl: './EditAccountingPeriodComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], EditAccountingPeriodComponent);
    return EditAccountingPeriodComponent;
}(BaseComponent_1.BaseComponent));
exports.EditAccountingPeriodComponent = EditAccountingPeriodComponent;
//# sourceMappingURL=EditAccountingPeriodComponent.js.map