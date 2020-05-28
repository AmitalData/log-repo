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
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var GLAccountWithholdingTaxPM_1 = require("../../../EntityPMs/GLAccountWithholdingTaxPM");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var Tools_1 = require("../../../../Infrastructure/Tools");
var GLAccountPMService_1 = require("../../../Services/StandardPMs/GLAccountPMService");
var AddEditTaxWithholdingLineComponent = /** @class */ (function (_super) {
    __extends(AddEditTaxWithholdingLineComponent, _super);
    function AddEditTaxWithholdingLineComponent() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.ObjectTableName = "GLAccountWithholdingTax";
        _this.gLAccountPMService = new GLAccountPMService_1.GLAccountPMService();
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.validationMsg1 = null;
        _this.lessOrGreatMsg = null;
        _this.validationMsg = null;
        _this.ValidationErrorsList = [];
        return _this;
    }
    AddEditTaxWithholdingLineComponent.prototype.SetWindowArgs = function (args) {
        this.parent = args.GLAccount;
        this.entity = args.GLAccountWithholdinTax;
        this.LastLineToDate = args.LastLineToDate;
        this.newEntity = new GLAccountWithholdingTaxPM_1.GLAccountWithholdingTaxPM(this.parent);
        this.newEntity.Percentage = this.entity.Percentage;
        this.newEntity.FromDate = this.entity.FromDate;
        this.newEntity.ToDate = this.entity.ToDate;
        this.newEntity.Inactive = this.entity.Inactive;
        var today = new Date();
        this.IsNew = args.IsNew;
        if (this.LastLineToDate == null || this.LastLineToDate.valueOf() < today.valueOf()) {
            if (this.ToDate == null) {
                this.FromDate = new Date();
            }
            // this.LastLineToDate = new Date();
        }
    };
    Object.defineProperty(AddEditTaxWithholdingLineComponent.prototype, "FromDate", {
        get: function () { return this.newEntity.FromDate; },
        set: function (value) {
            this.UIProperties.SetRequired("FromDate", this.ObjectTableName, false);
            if (this.newEntity.FromDate != value) {
                if (value) {
                    this.newEntity.FromDate = value;
                    this.UIProperties.SetValidity("FromDate", this.ObjectTableName, true, null);
                    var datetocompare = Tools_1.DateTool.GetDateParts(this.LastLineToDate).DateTicks;
                    var dateFromcompare = Tools_1.DateTool.GetDateParts(this.LastLineFromDate).DateTicks;
                    var fromDateParts = Tools_1.DateTool.GetDateParts(value).DateTicks;
                    if ((this.newEntity.ToDate && value > this.newEntity.ToDate)) {
                        this.UIProperties.SetValidity("FromDate", this.ObjectTableName, false, TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.O.MustBeLess"));
                        this.lessOrGreatMsg = TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.O.MustBeLess");
                    }
                    else {
                        this.lessOrGreatMsg = null;
                        this.validationMsg = null;
                    }
                    if (this.LastLineToDate && (fromDateParts < datetocompare || fromDateParts == datetocompare)) {
                        this.UIProperties.SetValidity("FromDate", this.ObjectTableName, false, TextCodeTranslator_1.TextCodeTranslator.Translate("GLAccounts.O.NotValidDate"));
                        this.validationMsg1 = TextCodeTranslator_1.TextCodeTranslator.Translate("GLAccounts.O.NotValidDate");
                    }
                    else {
                        this.validationMsg1 = null;
                        this.validationMsg = null;
                    }
                }
                else {
                    this.validationMsg = null;
                    this.validationMsg1 = null;
                    this.fromDate = null;
                    // this.UIProperties.SetRequired("FromDate", this.ObjectTableName, true);
                    this.newEntity.FromDate = null;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditTaxWithholdingLineComponent.prototype, "ToDate", {
        get: function () { return this.newEntity.ToDate; },
        set: function (value) {
            this.UIProperties.SetRequired("ToDate", this.ObjectTableName, false);
            if (this.newEntity.ToDate != value) {
                if (value) {
                    this.newEntity.ToDate = value;
                    if (this.newEntity.FromDate && (this.newEntity.FromDate > value || this.newEntity.FromDate.valueOf() == value.valueOf())) {
                        this.UIProperties.SetValidity("ToDate", this.ObjectTableName, false, TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.O.MustBeLarger"));
                        this.lessOrGreatMsg = TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.O.MustBeLarger");
                    }
                    else {
                        // this.newEntity.ToDate = value;
                        //this.LastLineToDate = value;
                        this.lessOrGreatMsg = null;
                    }
                }
                else {
                    // this.toDate = null;
                    this.UIProperties.SetRequired("ToDate", this.ObjectTableName, true);
                    this.newEntity.ToDate = null;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditTaxWithholdingLineComponent.prototype, "Percentage", {
        get: function () { return this.newEntity.Percentage; },
        set: function (value) {
            this.UIProperties.SetRequired("Percentage", this.ObjectTableName, false);
            if (this.newEntity.Percentage != value) {
                this.newEntity.Percentage = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditTaxWithholdingLineComponent.prototype, "Inactive", {
        get: function () { return this.newEntity.Inactive; },
        set: function (value) {
            if (this.newEntity.Inactive != value) {
                this.newEntity.Inactive = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    AddEditTaxWithholdingLineComponent.prototype.OkButtonClicked = function () {
        var FIELD_IS_REQUIERD = null;
        FIELD_IS_REQUIERD = TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.FieldIsRequired");
        var errors = [];
        this.ValidationErrorsList = [];
        if (this.validationMsg != null) {
            errors.push(this.validationMsg);
        }
        if (this.validationMsg1 != null) {
            errors.push(this.validationMsg1);
        }
        if (this.lessOrGreatMsg != null) {
            errors.push(this.lessOrGreatMsg);
        }
        if (this.FromDate == undefined) {
            var s = FIELD_IS_REQUIERD.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("GLAccountWithholdingTax.F.FromDate"));
            errors.push(s);
        }
        if (this.ToDate == undefined) {
            var s = FIELD_IS_REQUIERD.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("GLAccountWithholdingTax.F.ToDate"));
            errors.push(s);
        }
        if (this.Percentage == undefined) {
            var s = FIELD_IS_REQUIERD.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("GLAccountWithholdingTax.F.Percentage"));
            errors.push(s);
        }
        //if (this.parent.GLAccountWithholdingTaxes.length > 0) {
        //    //if (this.parent.GLAccountWithholdingTaxes.find(d => d.FromDate ))
        //}
        if (errors.length == 0) {
            this.entity.Percentage = this.newEntity.Percentage;
            this.entity.FromDate = this.newEntity.FromDate;
            this.entity.ToDate = this.newEntity.ToDate;
            this.entity.Inactive = this.newEntity.Inactive;
            this.LastLineToDate = this.ToDate;
            this.CurrentSession.CloseCurrentWindowEmit("ok");
            //  this.gLAccountPMService.update(this.parent).subscribe((myResponse: ServiceResponse) => { });
            //   this.CurrentSession.CurrentEditComponent.SaveChanges();
        }
        else {
            this.ValidationErrorsList = errors;
        }
    };
    AddEditTaxWithholdingLineComponent.prototype.InactiveChecked = function (checked) {
        this.entity.Changed = true;
        var lines = this.parent.GLAccountWithholdingTaxes.filter(function (d) { return !d.Inactive; });
        var items = lines.sort(function (a, b) { return (a.LineNumber === b.LineNumber) ? 0 : (a.LineNumber < b.LineNumber) ? -1 : 1; });
        var item = items[lines.length - 1];
        if (checked) {
            this.Inactive = true;
            if (item) {
                this.LastLineToDate = item.ToDate;
            }
            else {
                this.LastLineToDate = null;
            }
        }
        else {
            this.Inactive = false;
            if (item) {
                this.LastLineToDate = item.ToDate;
                this.LastLineFromDate = item.FromDate;
                var datetocompare = Tools_1.DateTool.GetDateParts(this.LastLineToDate).DateTicks;
                var dateFromcompare = Tools_1.DateTool.GetDateParts(this.LastLineFromDate).DateTicks;
                var fromDateParts = Tools_1.DateTool.GetDateParts(this.FromDate).DateTicks;
                var toDateParts = Tools_1.DateTool.GetDateParts(this.ToDate).DateTicks;
                //var fromDateMonth: number = (this.FromDate.getMonth() +1 -1);
                //var fromDateDay: number = this.FromDate.getDay();
                //var fromDateYear: number = this.FromDate.getFullYear();
                if (fromDateParts == datetocompare || fromDateParts < datetocompare) {
                    this.validationMsg = TextCodeTranslator_1.TextCodeTranslator.Translate("GLAccounts.O.NotValidDate");
                }
                else
                    this.validationMsg = null;
            }
            else {
                this.LastLineToDate = null;
            }
        }
    };
    AddEditTaxWithholdingLineComponent.prototype.CancelButtonClicked = function () {
        //this.entity.Percentage = this.oldEntity.Percentage;
        //this.entity.FromDate = this.oldEntity.FromDate;
        //this.entity.ToDate = this.oldEntity.ToDate;
        this.CurrentSession.CloseCurrentWindow();
    };
    AddEditTaxWithholdingLineComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './AddEditTaxWithholdingLineComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], AddEditTaxWithholdingLineComponent);
    return AddEditTaxWithholdingLineComponent;
}(BaseComponent_1.BaseComponent));
exports.AddEditTaxWithholdingLineComponent = AddEditTaxWithholdingLineComponent;
//# sourceMappingURL=AddEditTaxWithholdingLineComponent.js.map