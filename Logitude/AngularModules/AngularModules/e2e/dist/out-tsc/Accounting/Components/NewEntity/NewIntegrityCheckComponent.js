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
var AccountingIntegrityCheckPMService_1 = require("./../../Services/StandardPMs/AccountingIntegrityCheckPMService");
var core_1 = require("@angular/core");
var BaseComponent_1 = require("../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var TextCodeTranslator_1 = require("../../../Infrastructure/Utilities/TextCodeTranslator");
var AccountingIntegrityCheckPM_1 = require("../../EntityPMs/AccountingIntegrityCheckPM");
var Tools_1 = require("../../../Infrastructure/Tools");
var EntityResourceService_1 = require("../../../Infrastructure/Services/EntityResourceService");
var NewIntegrityCheckComponent = /** @class */ (function (_super) {
    __extends(NewIntegrityCheckComponent, _super);
    function NewIntegrityCheckComponent(CD) {
        var _this = _super.call(this) || this;
        _this.CD = CD;
        _this.EntityPM = new AccountingIntegrityCheckPM_1.AccountingIntegrityCheckPM();
        _this.DataContext = _this;
        _this.ObjectTableName = "AccountingIntegrityCheck";
        _this.ValidationErrorsList = [];
        _this._entityResourceService = new EntityResourceService_1.EntityResourceService();
        _this._AccountingIntegrityCheckPMService = new AccountingIntegrityCheckPMService_1.AccountingIntegrityCheckPMService();
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        //#endregion
        //#region Date Filters Validation
        _this.isValid = true;
        return _this;
    }
    NewIntegrityCheckComponent.prototype.SetWindowArgs = function (args) {
        if (args != null) {
            this.EntityPM = args.EntityPM;
        }
    };
    NewIntegrityCheckComponent.prototype.ngOnInit = function () {
    };
    Object.defineProperty(NewIntegrityCheckComponent.prototype, "FromMonthInclusive", {
        //#region Properties
        get: function () { return this.EntityPM.FromMonthInclusive; },
        set: function (value) {
            if (this.EntityPM.FromMonthInclusive != value) {
                this.EntityPM.FromMonthInclusive = value;
                if (!this.isValid)
                    this.validateDates();
                else {
                    this.isValid = false;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewIntegrityCheckComponent.prototype, "ToMonthInclusive", {
        get: function () { return this.EntityPM.ToMonthInclusive; },
        set: function (value) {
            if (this.EntityPM.ToMonthInclusive != value) {
                this.EntityPM.ToMonthInclusive = value;
                if (!this.isValid)
                    this.validateDates();
                else {
                    this.isValid = false;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    NewIntegrityCheckComponent.prototype.validateDates = function () {
        var _this = this;
        if (this.FromMonthInclusive > this.ToMonthInclusive) {
            this.isValid = false;
            setTimeout(function () {
                _this.UIProperties.SetValidity("ToMonthInclusive", _this.ObjectTableName, false, TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.General.O.ToDateMustGreaterFromDate"));
                _this.UIProperties.SetValidity("FromMonthInclusive", _this.ObjectTableName, false, TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.General.O.FromDateMustSmallerToDate"));
                _this.CD.detectChanges();
            }, 200);
            return false;
        }
        else {
            this.isValid = true;
            setTimeout(function () {
                _this.UIProperties.SetValidity("ToMonthInclusive", _this.ObjectTableName, true, "");
                _this.UIProperties.SetValidity("FromMonthInclusive", _this.ObjectTableName, true, "");
                _this.CD.detectChanges();
            }, 200);
            return true;
        }
    };
    NewIntegrityCheckComponent.prototype.OkButtonClicked = function () {
        var errors = [];
        // Required check
        if (Tools_1.AppTool.IsNullOrEmpty(this.FromMonthInclusive) || Tools_1.AppTool.IsNullOrEmpty(this.ToMonthInclusive)) {
            if (this.FromMonthInclusive == null)
                errors.push("From Month not selected");
            if (this.ToMonthInclusive == null)
                errors.push("To Month not selected");
            // errors.push(TextCodeTranslator.Translate("Accounting.General.O.AllFieldsRequired"));
        }
        else if (this.FromMonthInclusive.getFullYear() != this.ToMonthInclusive.getFullYear()) {
            // errors.push(TextCodeTranslator.Translate("Accounting.O.FromdateandTodatemustbesameyear"));
            errors.push("From and To months must be same year");
        }
        if (!this.validateDates()) {
            errors.push("To month is greater than from month");
        }
        if (errors.length > 0) {
            this.ValidationErrorsList = errors;
        }
        else {
            this.SubmitChanges();
        }
    };
    NewIntegrityCheckComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    NewIntegrityCheckComponent.prototype.SubmitChanges = function () {
        var _this = this;
        var errors = [];
        if (errors.length == 0) {
            if (this.EntityPM != null) {
                this.CurrentSession.StartBusyIndicatorSaving();
                this._AccountingIntegrityCheckPMService.insert(this.EntityPM).subscribe(function (myResult) {
                    var mm = myResult;
                    if (!mm.HasError) {
                        _this.CurrentSession.CloseCurrentWindowEmit("ok");
                    }
                    else {
                        _this.ValidationErrorsList = mm.ErrorsArray;
                        _this.CurrentSession.StopBusyIndicator();
                    }
                });
            }
        }
        else {
            this.ValidationErrorsList = errors;
        }
    };
    NewIntegrityCheckComponent = __decorate([
        core_1.Component({
            selector: 'NewIntegrityCheckComponent',
            moduleId: module.id,
            templateUrl: './NewIntegrityCheckComponent.html',
        }),
        __metadata("design:paramtypes", [core_1.ChangeDetectorRef])
    ], NewIntegrityCheckComponent);
    return NewIntegrityCheckComponent;
}(BaseComponent_1.BaseComponent));
exports.NewIntegrityCheckComponent = NewIntegrityCheckComponent;
//# sourceMappingURL=NewIntegrityCheckComponent.js.map