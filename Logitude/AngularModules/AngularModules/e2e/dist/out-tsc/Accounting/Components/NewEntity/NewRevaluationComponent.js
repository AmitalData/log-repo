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
var RevaluationPM_1 = require("../../EntityPMs/RevaluationPM");
var Tools_1 = require("../../../Infrastructure/Tools");
var FullAccountingSettingPMService_1 = require("../../Services/StandardPMs/FullAccountingSettingPMService");
var ApiQueryFilters_1 = require("../../../Infrastructure/DataContracts/ApiQueryFilters");
var RevaluationPMService_1 = require("../../Services/StandardPMs/RevaluationPMService");
var EntityResourceService_1 = require("../../../Infrastructure/Services/EntityResourceService");
var Validator_1 = require("../../../Infrastructure/Validators/Validator");
var TextCodeTranslator_1 = require("../../../Infrastructure/Utilities/TextCodeTranslator");
var NewRevaluationComponent = /** @class */ (function (_super) {
    __extends(NewRevaluationComponent, _super);
    function NewRevaluationComponent() {
        var _this = _super.call(this) || this;
        _this.ObjectTableName = "Revaluation";
        _this.DataContext = _this;
        _this.fullAccountingSettingPMService = new FullAccountingSettingPMService_1.FullAccountingSettingPMService();
        _this.entityResourceService = new EntityResourceService_1.EntityResourceService();
        _this.ValidationErrorsList = [];
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.defaultGLAccount = true;
        _this.EntityPM = new RevaluationPM_1.RevaluationPM();
        _this.TenantPM = SessionLocator_1.SessionLocator.TenantPM;
        _this.EntityPM.Tenant = _this.TenantPM.Id;
        _this.RevaluationDate = Tools_1.DateTool.GetCurrentDateTimeAsUtc();
        _this.UIProperties.SetEnabled("GLAccountId", _this.ObjectTableName, false);
        _this.UIProperties.SetEnabled("ChartOfAccountsId", _this.ObjectTableName, false);
        _this.RevaluationService = new RevaluationPMService_1.RevaluationPMService();
        _this.EntityPM.RevaluationNumber = 0;
        _this.EntityPM.CreatedByUserId = "new";
        _this.EntityPM.CreateDate = Tools_1.DateTool.GetCurrentDateTimeAsUtc();
        _this.EntityPM.RevaluationEnabled = true;
        _this.FIELD_IS_REQUIERD = TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.FieldIsRequired");
        _this.fullAccountingSettingPMService.get(_this.TenantPM.Id.toString()).subscribe(function (myResult) {
            if (myResult) {
                if (!myResult.HasError) {
                    _this.RevaluationsGLAccountId = myResult.Result.ExchangeRateDiffGLAccountId;
                    //  this.EnglishName = myResult.Result.EnglishName;
                }
            }
        });
        _this.GLAccountFilterItems = new ApiQueryFilters_1.ApiQueryFilters();
        _this.GLAccountFilterItems.addAdditionalFilter("AccountTypeCode", "1", null, null, "Equals", false, false, false, "string");
        return _this;
    }
    NewRevaluationComponent.prototype.GetRequierdFieldErrorText = function (fieldName) {
        return this.FIELD_IS_REQUIERD.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate(fieldName));
    };
    Object.defineProperty(NewRevaluationComponent.prototype, "RevaluationDate", {
        get: function () { return this.EntityPM.RevaluationDate; },
        set: function (value) {
            this.UIProperties.SetValidity("RevaluationDate", "Revaluation", true, "");
            if (this.EntityPM.RevaluationDate != value) {
                if (value > Tools_1.DateTool.GetCurrentDateTimeAsUtc()) {
                    this.UIProperties.SetValidity("RevaluationDate", "Revaluation", false, TextCodeTranslator_1.TextCodeTranslator.Translate("Revaluation.O.FutureDateIsNotAllowed"));
                }
                this.EntityPM.RevaluationDate = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewRevaluationComponent.prototype, "RevaluationsGLAccountId", {
        get: function () { return this.EntityPM.RevaluationsGLAccountId; },
        set: function (value) {
            if (this.EntityPM.RevaluationsGLAccountId != value) {
                this.EntityPM.RevaluationsGLAccountId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewRevaluationComponent.prototype, "GLAccountId", {
        //englishName: string;
        //get EnglishName() { return this.englishName; }
        //set EnglishName(value: string) {
        //    if (this.englishName != value) {
        //        this.englishName = value;
        //    }
        //}
        get: function () { return this.EntityPM.GLAccountId; },
        set: function (value) {
            if (this.EntityPM.GLAccountId != value) {
                this.EntityPM.GLAccountId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewRevaluationComponent.prototype, "ChartOfAccountsId", {
        get: function () { return this.EntityPM.ChartOfAccountsId; },
        set: function (value) {
            if (this.EntityPM.ChartOfAccountsId != value) {
                this.EntityPM.ChartOfAccountsId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewRevaluationComponent.prototype, "ChartOfAccount", {
        get: function () { return this.chartOfAccount; },
        set: function (value) {
            this.chartOfAccount = value;
            if (value) {
                this.DefaultGLAccount = false;
                this.GLAccount = false;
                this.UIProperties.SetEnabled("GLAccountId", this.ObjectTableName, false);
                this.UIProperties.SetEnabled("ChartOfAccountsId", this.ObjectTableName, true);
                this.EntityPM.RevaluationEnabled = false;
            }
            //   this.SetChartOfAccount(value);
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewRevaluationComponent.prototype, "DefaultGLAccount", {
        get: function () { return this.defaultGLAccount; },
        set: function (value) {
            this.defaultGLAccount = value;
            if (value) {
                this.ChartOfAccount = false;
                this.GLAccount = false;
                this.UIProperties.SetEnabled("GLAccountId", this.ObjectTableName, false);
                this.UIProperties.SetEnabled("ChartOfAccountsId", this.ObjectTableName, false);
                this.EntityPM.RevaluationEnabled = true;
            }
            //   this.SetDefaultGLAccount(value);
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewRevaluationComponent.prototype, "GLAccount", {
        get: function () { return this.gLAccount; },
        set: function (value) {
            this.gLAccount = value;
            if (value) {
                this.DefaultGLAccount = false;
                this.ChartOfAccount = false;
                this.UIProperties.SetEnabled("GLAccountId", this.ObjectTableName, true);
                this.UIProperties.SetEnabled("ChartOfAccountsId", this.ObjectTableName, false);
                this.EntityPM.RevaluationEnabled = false;
            }
            //    this.SetGLAccount(value);
        },
        enumerable: true,
        configurable: true
    });
    NewRevaluationComponent.prototype.SetDefaultGLAccount = function (value) {
        this.DefaultGLAccount = value;
    };
    NewRevaluationComponent.prototype.SetChartOfAccount = function (value) {
        this.ChartOfAccount = value;
    };
    NewRevaluationComponent.prototype.SetGLAccount = function (value) {
        this.GLAccount = value;
    };
    NewRevaluationComponent.prototype.OkButtonClicked = function () {
        var errors = [];
        Validator_1.Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        // Custom Validation
        if (this.RevaluationDate == null) {
            errors.push(this.GetRequierdFieldErrorText("Revaluation.F.RevaluationDate"));
        }
        if (this.RevaluationsGLAccountId == null) {
            errors.push(this.GetRequierdFieldErrorText("Revaluation.F.RevaluationsGLAccountId"));
        }
        if (this.GLAccount && this.GLAccountId == null) {
            errors.push(TextCodeTranslator_1.TextCodeTranslator.Translate("Revaluation.O.GLAccountForRevaluation"));
        }
        if (this.ChartOfAccount && this.ChartOfAccountsId == null) {
            errors.push(TextCodeTranslator_1.TextCodeTranslator.Translate("Revaluation.O.ChartAccountForRevaluation"));
        }
        if (this.RevaluationDate > Tools_1.DateTool.GetCurrentDateTimeAsUtc()) {
            errors.push(TextCodeTranslator_1.TextCodeTranslator.Translate("Revaluation.O.FutureDateIsNotAllowed"));
        }
        this.ValidationErrorsList = errors;
        if (this.ValidationErrorsList.length == 0) {
            this.SubmitChanges();
        }
    };
    NewRevaluationComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    NewRevaluationComponent.prototype.SubmitChanges = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicator("");
        this.RevaluationService.insert(this.EntityPM).subscribe(function (Result) {
            var mm = Result;
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
                //  this.ValidationErrorsList = mm.ErrorsArray;
                _this.CurrentSession.StopBusyIndicator();
            }
        });
    };
    NewRevaluationComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './NewRevaluationComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], NewRevaluationComponent);
    return NewRevaluationComponent;
}(BaseComponent_1.BaseComponent));
exports.NewRevaluationComponent = NewRevaluationComponent;
//# sourceMappingURL=NewRevaluationComponent.js.map