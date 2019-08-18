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
var TaxDeductionReportPM_1 = require("../../EntityPMs/TaxDeductionReportPM");
var TaxDeductionReportPMService_1 = require("../../Services/StandardPMs/TaxDeductionReportPMService");
var Validator_1 = require("../../../Infrastructure/Validators/Validator");
var TextCodeTranslator_1 = require("../../../Infrastructure/Utilities/TextCodeTranslator");
var Tools_1 = require("../../../Infrastructure/Tools");
var NewTaxDeductionReportComponent = /** @class */ (function (_super) {
    __extends(NewTaxDeductionReportComponent, _super);
    function NewTaxDeductionReportComponent() {
        var _this = _super.call(this) || this;
        _this.ObjectTableName = "TaxDeductionReport";
        _this.DataContext = _this;
        _this.entityPM = new TaxDeductionReportPM_1.TaxDeductionReportPM();
        _this.TaxDeductionReportPMService = new TaxDeductionReportPMService_1.TaxDeductionReportPMService();
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.FIELD_IS_REQUIERD = null;
        _this.ValidationErrorsList = [];
        var date = new Date();
        _this.entityPM.TaxYear = date.getFullYear();
        _this.entityPM.Tenant = SessionLocator_1.SessionLocator.Tenant;
        _this.entityPM.Email = SessionLocator_1.SessionLocator.LoggedUserPM.Email;
        return _this;
    }
    Object.defineProperty(NewTaxDeductionReportComponent.prototype, "Email", {
        get: function () { return this.entityPM.Email; },
        set: function (value) {
            if (this.entityPM.Email != value) {
                this.entityPM.Email = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewTaxDeductionReportComponent.prototype, "TaxYear", {
        get: function () { return this.entityPM.TaxYear; },
        set: function (value) {
            if (this.entityPM.TaxYear != value) {
                this.entityPM.TaxYear = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewTaxDeductionReportComponent.prototype, "IsAdditionalReportExist", {
        get: function () { return this.entityPM.IsAdditionalReportExist; },
        set: function (value) {
            if (this.entityPM.IsAdditionalReportExist != value) {
                this.entityPM.IsAdditionalReportExist = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    NewTaxDeductionReportComponent.prototype.OkButtonClicked = function () {
        var _this = this;
        this.entityPM.CreateDate = new Date();
        this.entityPM.CreatedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
        this.entityPM.UpdateDate = new Date();
        this.entityPM.UpdatedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
        var errors = [];
        this.FIELD_IS_REQUIERD = TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.FieldIsRequired");
        Validator_1.Validator.TryValidateObject(this.entityPM, this.ObjectTableName, errors);
        if (Tools_1.AppTool.IsNullOrEmpty(this.entityPM.TaxYear)) {
            var s = this.FIELD_IS_REQUIERD.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("TaxDeductionReport.F.TaxYear"));
            errors.push(s);
        }
        if (Tools_1.AppTool.IsNullOrEmpty(this.entityPM.Email)) {
            var s = this.FIELD_IS_REQUIERD.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("TaxDeductionReport.F.Email"));
            errors.push(s);
        }
        this.ValidationErrorsList = errors;
        if (this.ValidationErrorsList.length == 0) {
            this.CurrentSession.StartBusyIndicator("");
            this.TaxDeductionReportPMService.insert(this.entityPM).subscribe(function (myResult) {
                var mm = myResult;
                if (!mm.HasError) {
                    var entity = mm.Result;
                    _this.CurrentSession.CloseCurrentWindowEmit("ok");
                    SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', _this.CurrentSession.SessionLocation.viewContainerRef)
                        .then(function (cmpRef) {
                        cmpRef.instance.ComponentRef = cmpRef;
                        cmpRef.instance.Run({ EntityId: entity.Id, ObjectTableName: _this.ObjectTableName });
                        cmpRef.instance.BackCompleted.subscribe(function ($event) {
                            _this.CancelButtonClicked();
                        });
                    });
                    _this.CurrentSession.StopBusyIndicator();
                }
                else {
                    _this.ValidationErrorsList = mm.ErrorsArray;
                    _this.CurrentSession.StopBusyIndicator();
                }
            });
        }
    };
    NewTaxDeductionReportComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    NewTaxDeductionReportComponent = __decorate([
        core_1.Component({
            selector: 'NewTaxDeductionReportComponent',
            moduleId: module.id,
            templateUrl: './NewTaxDeductionReportComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], NewTaxDeductionReportComponent);
    return NewTaxDeductionReportComponent;
}(BaseComponent_1.BaseComponent));
exports.NewTaxDeductionReportComponent = NewTaxDeductionReportComponent;
//# sourceMappingURL=NewTaxDeductionReportComponent.js.map