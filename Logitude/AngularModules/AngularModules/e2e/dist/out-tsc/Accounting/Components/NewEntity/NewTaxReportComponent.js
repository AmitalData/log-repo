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
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var BaseComponent_1 = require("../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var TaxReportPM_1 = require("../../EntityPMs/TaxReportPM");
var TaxReportPMService_1 = require("../../Services/StandardPMs/TaxReportPMService");
var CodeNameClass_1 = require("../../../Infrastructure/DataContracts/CodeNameClass");
var Validator_1 = require("../../../Infrastructure/Validators/Validator");
var TextCodeTranslator_1 = require("../../../Infrastructure/Utilities/TextCodeTranslator");
var Tools_1 = require("../../../Infrastructure/Tools");
var TaxReportExtendedPMService_1 = require("../../Services/ExtendedPMs/TaxReportExtendedPMService");
var BatchTaskExecutionListService_1 = require("../../../Infrastructure/Services/StandardLists/BatchTaskExecutionListService");
var NewTaxReportComponent = /** @class */ (function (_super) {
    __extends(NewTaxReportComponent, _super);
    function NewTaxReportComponent() {
        var _this = _super.call(this) || this;
        _this.ObjectTableName = "TaxReport";
        _this.DataContext = _this;
        _this.entityPM = new TaxReportPM_1.TaxReportPM();
        _this.TaxReportPMService = new TaxReportPMService_1.TaxReportPMService();
        _this._TaxReportExtendedPMService = new TaxReportExtendedPMService_1.TaxReportExtendedPMService();
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this._BatchTaskExecutionListService = new BatchTaskExecutionListService_1.BatchTaskExecutionListService();
        _this.timerInterval = 1000;
        _this.FIELD_IS_REQUIERD = null;
        _this.ValidationErrorsList = [];
        ///  this.entityPM.TaxReportMonth = new Date();
        _this.entityPM.Tenant = SessionLocator_1.SessionLocator.Tenant;
        _this.BuildMonthList();
        return _this;
    }
    Object.defineProperty(NewTaxReportComponent.prototype, "TaxReportMonth", {
        get: function () { return this.entityPM.TaxReportMonth; },
        set: function (value) {
            if (this.entityPM.TaxReportMonth != value) {
                this.entityPM.TaxReportMonth = value;
                if (this.entityPM.Year != null) {
                    this.entityPM.TaxReportMonth.setFullYear(this.entityPM.Year);
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewTaxReportComponent.prototype, "Year", {
        get: function () { return this.entityPM.Year; },
        set: function (value) {
            if (this.entityPM.Year != value) {
                this.entityPM.Year = value;
                if (this.entityPM.TaxReportMonth != null)
                    this.entityPM.TaxReportMonth.setFullYear(value);
            }
        },
        enumerable: true,
        configurable: true
    });
    NewTaxReportComponent.prototype.SelectedItemChanged = function (item) {
        this.entityPM.TaxReportMonth = item.Code;
    };
    NewTaxReportComponent.prototype.BuildMonthList = function () {
        this.MonthsList = [];
        this.MonthsList.push(new CodeNameClass_1.CodeNameClass("1", "January"));
        this.MonthsList.push(new CodeNameClass_1.CodeNameClass("2", "February"));
        this.MonthsList.push(new CodeNameClass_1.CodeNameClass("3", "March"));
        this.MonthsList.push(new CodeNameClass_1.CodeNameClass("4", "April"));
        this.MonthsList.push(new CodeNameClass_1.CodeNameClass("5", "May"));
        this.MonthsList.push(new CodeNameClass_1.CodeNameClass("6", "June"));
        this.MonthsList.push(new CodeNameClass_1.CodeNameClass("7", "July"));
        this.MonthsList.push(new CodeNameClass_1.CodeNameClass("8", "August"));
        this.MonthsList.push(new CodeNameClass_1.CodeNameClass("9", "September"));
        this.MonthsList.push(new CodeNameClass_1.CodeNameClass("10", "October"));
        this.MonthsList.push(new CodeNameClass_1.CodeNameClass("11", "November"));
        this.MonthsList.push(new CodeNameClass_1.CodeNameClass("12", "December"));
    };
    Object.defineProperty(NewTaxReportComponent.prototype, "SelectedMonth", {
        get: function () { return this.selectedMonth; },
        set: function (value) {
            if (this.selectedMonth != value) {
                this.selectedMonth = value;
                if (value != null) {
                    this.UIProperties.SetRequired("TaxReportMonth", this.ObjectTableName, false);
                    this.entityPM.TaxReportMonth = new Date();
                    this.entityPM.TaxReportMonth.setMonth(+value.Code - 1);
                    if (this.entityPM.Year != null) {
                        this.entityPM.TaxReportMonth.setFullYear(this.entityPM.Year);
                    }
                }
                else {
                    this.UIProperties.SetRequired("TaxReportMonth", this.ObjectTableName, true);
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    NewTaxReportComponent.prototype.OkButtonClicked = function () {
        var _this = this;
        this.entityPM.CreateDate = new Date();
        this.entityPM.CreatedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
        this.entityPM.LastUpdateDate = new Date();
        this.entityPM.UpdatedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
        var errors = [];
        this.FIELD_IS_REQUIERD = TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.FieldIsRequired");
        Validator_1.Validator.TryValidateObject(this.entityPM, this.ObjectTableName, errors);
        if (Tools_1.AppTool.IsNullOrEmpty(this.entityPM.Year)) {
            var s = this.FIELD_IS_REQUIERD.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("TaxReport.F.Year"));
            errors.push(s);
        }
        if (this.SelectedMonth == null) {
            var s = this.FIELD_IS_REQUIERD.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("TaxReport.F.TaxReportMonth"));
            errors.push(s);
        }
        this.ValidationErrorsList = errors;
        if (this.ValidationErrorsList.length == 0) {
            this.CurrentSession.StartBusyIndicator("");
            this.TaxReportPMService.insert(this.entityPM).subscribe(function (myResult) {
                var mm = myResult;
                if (!mm.HasError) {
                    var entity = mm.Result;
                    //this.CurrentSession.StartBusyIndicator("");
                    //    this.CurrentSession.CloseCurrentWindowEmit("ok");
                    _this._TaxReportExtendedPMService.PostCreateTaxReportInBatch(entity).subscribe(function (myResult) {
                        var mm = myResult;
                        var entity = mm.Result;
                        _this.btePM = entity;
                        //this.ChangeStatus("inprogress");
                        _this.timer = setInterval(function () {
                            _this.GetBTE();
                        }, _this.timerInterval);
                    });
                    //SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent',
                    //    this.CurrentSession.SessionLocation.viewContainerRef)
                    //    .then(cmpRef => {
                    //        cmpRef.instance.ComponentRef = cmpRef;
                    //        cmpRef.instance.Run({ EntityId: entity.Id, ObjectTableName: this.ObjectTableName });
                    //        cmpRef.instance.BackCompleted.subscribe(($event: any) => {
                    //            this.CancelButtonClicked();
                    //        });
                    //    });
                    //this.CurrentSession.StopBusyIndicator();
                }
                else {
                    _this.ValidationErrorsList = mm.ErrorsArray;
                    _this.CurrentSession.StopBusyIndicator();
                }
            });
        }
    };
    NewTaxReportComponent.prototype.GetBTE = function () {
        var _this = this;
        this._BatchTaskExecutionListService.getSingle(this.btePM.Id).subscribe(function (myResult) {
            console.log("[_BatchTaskExecutionListService.getSingle]", myResult);
            var mm = myResult;
            if (!mm.HasError) {
                _this.bteList = mm.Result;
                if (_this.bteList.StatusCode == "D") // D- Done
                 {
                    _this.CurrentSession.StopBusyIndicator();
                    _this.CurrentSession.CloseCurrentWindowEmit("ok");
                    SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', _this.CurrentSession.SessionLocation.viewContainerRef)
                        .then(function (cmpRef) {
                        cmpRef.instance.ComponentRef = cmpRef;
                        cmpRef.instance.Run({ EntityId: _this.entityPM.Id, ObjectTableName: _this.ObjectTableName });
                        cmpRef.instance.BackCompleted.subscribe(function ($event) {
                            _this.CancelButtonClicked();
                        });
                    });
                    _this.CurrentSession.StopBusyIndicator();
                    //stop timer
                    if (_this.timer) {
                        clearInterval(_this.timer);
                    }
                }
                else if (_this.bteList.StatusCode == "F") // F- Failed
                 {
                    //stop timer
                    _this.CurrentSession.StopBusyIndicator();
                    _this.CurrentSession.CloseCurrentWindowEmit("ok");
                    if (_this.timer) {
                        clearInterval(_this.timer);
                    }
                    //update status
                    //   this.ChangeStatus("failed");
                }
            }
            else {
            }
        });
    };
    NewTaxReportComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    NewTaxReportComponent = __decorate([
        core_1.Component({
            selector: 'NewTaxReportComponent',
            moduleId: module.id,
            templateUrl: './NewTaxReportComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], NewTaxReportComponent);
    return NewTaxReportComponent;
}(BaseComponent_1.BaseComponent));
exports.NewTaxReportComponent = NewTaxReportComponent;
//# sourceMappingURL=NewTaxReportComponent.js.map