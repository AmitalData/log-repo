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
var Tools_1 = require("../../../Infrastructure/Tools");
var BaseComponent_1 = require("../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var TariffSettingPM_1 = require("../../../TariffModule/EntityPMs/TariffSettingPM");
var EntityResourceService_1 = require("../../../Infrastructure/Services/EntityResourceService");
var TariffDomainService_1 = require("../../../TariffModule/Services/TariffDomainService");
var TariffSettingPMService_1 = require("../../../TariffModule/Services/StandardPMs/TariffSettingPMService");
var Validator_1 = require("../../../Infrastructure/Validators/Validator");
var TariffSettingComponent = /** @class */ (function (_super) {
    __extends(TariffSettingComponent, _super);
    function TariffSettingComponent(entityResourceService) {
        var _this = _super.call(this) || this;
        _this.entityResourceService = entityResourceService;
        _this.ObjectTableName = "TariffSetting";
        _this.DataContext = _this;
        _this.IsResourcesReady = false;
        _this.ValidationErrorsList = [];
        _this.ItemsSource = [];
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.myService = new TariffSettingPMService_1.TariffSettingPMService();
        _this.myDomainService = new TariffDomainService_1.TariffDomainService();
        _this.entityResourceService.getEntityResourceByTableName(_this.ObjectTableName).subscribe(function (res1) {
            _this.myDomainService.GetTenantTariffSetting().subscribe(function (myResponse) {
                if (myResponse.HasError) {
                    _this.ValidationErrorsList = myResponse.ErrorsArray;
                }
                else {
                    _this.EntityPM = myResponse.Result;
                    if (!_this.EntityPM) {
                        _this.EntityPM = new TariffSettingPM_1.TariffSettingPM();
                        _this.EntityPM.Tenant = SessionLocator_1.SessionLocator.Tenant;
                    }
                    _this.BuildItemsSource();
                    _this.IsResourcesReady = true;
                }
            });
        });
        return _this;
    }
    Object.defineProperty(TariffSettingComponent.prototype, "DefaultWarningPercentage", {
        get: function () {
            if (this.EntityPM != null) {
                return this.EntityPM.DefaultWarningPercentage;
            }
        },
        set: function (value) {
            if (this.EntityPM.DefaultWarningPercentage != value) {
                this.EntityPM.DefaultWarningPercentage = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TariffSettingComponent.prototype, "DefaultPriceSteps", {
        get: function () { return this.EntityPM.DefaultPriceSteps; },
        set: function (value) {
            if (this.EntityPM.DefaultPriceSteps != value) {
                this.EntityPM.DefaultPriceSteps = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    TariffSettingComponent.prototype.BuildItemsSource = function () {
        var _this = this;
        var Steps = [];
        if (this.DefaultPriceSteps) {
            Steps = this.DefaultPriceSteps.split(',');
        }
        var index = 0;
        Steps.forEach(function (item) {
            _this.ItemsSource.push(new TariffSettingStep(item, index, _this));
            index++;
        });
        if (Steps.length < 8) {
            for (var i = Steps.length; i < 8; i++) {
                this.ItemsSource.push(new TariffSettingStep(null, index, this));
                index++;
            }
        }
    };
    TariffSettingComponent.prototype.BuildDefaultPriceSteps = function () {
        var iDefaultPriceSteps = null;
        this.ItemsSource.filter(function (f) { return f.Step != null; }).sort(function (a, b) { return (a.Index === b.Index) ? 0 : (a.Index < b.Index) ? -1 : 1; }).forEach(function (item) {
            if (Tools_1.AppTool.IsNullOrEmpty(iDefaultPriceSteps)) {
                iDefaultPriceSteps = "" + item.Step;
            }
            else {
                iDefaultPriceSteps += "," + item.Step;
            }
        });
        this.DefaultPriceSteps = iDefaultPriceSteps;
    };
    TariffSettingComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    TariffSettingComponent.prototype.OkButtonClicked = function () {
        var _this = this;
        this.ValidationErrorsList = [];
        if (!this.EntityPM.IsDirty) {
            this.CurrentSession.CloseCurrentWindow();
        }
        else {
            var errors = [];
            Validator_1.Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
            if (this.DefaultWarningPercentage == null || this.DefaultWarningPercentage > 100 || this.DefaultWarningPercentage < 0) {
                errors.push("Warning Percentage must be between 0-100");
            }
            var isValidSort = true;
            var SortedItemStep = 0;
            this.ItemsSource.filter(function (f) { return f.Step != null; }).sort(function (a, b) { return (a.Index === b.Index) ? 0 : (a.Index < b.Index) ? -1 : 1; }).forEach(function (item) {
                if (SortedItemStep == 0) {
                    SortedItemStep = item.Step;
                }
                else {
                    if (item.Step <= SortedItemStep) {
                        isValidSort = false;
                    }
                    else {
                        SortedItemStep = item.Step;
                    }
                }
            });
            if (!isValidSort) {
                errors.push("Price steps must be sorted");
            }
            this.ValidationErrorsList = this.ValidationErrorsList.concat(errors);
            if (this.ValidationErrorsList.length == 0) {
                this.CurrentSession.StartBusyIndicatorSaving();
                if (this.EntityPM.Id) {
                    this.myService.update(this.EntityPM).subscribe(function (myResponse) {
                        _this.CurrentSession.StopBusyIndicator();
                        if (myResponse.HasError) {
                            _this.ValidationErrorsList = myResponse.ErrorsArray;
                        }
                        else {
                            _this.CurrentSession.CloseCurrentWindow();
                        }
                    });
                }
                else {
                    this.myService.insert(this.EntityPM).subscribe(function (myResponse) {
                        _this.CurrentSession.StopBusyIndicator();
                        if (myResponse.HasError) {
                            _this.ValidationErrorsList = myResponse.ErrorsArray;
                        }
                        else {
                            _this.CurrentSession.CloseCurrentWindow();
                        }
                    });
                }
            }
        }
    };
    TariffSettingComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './TariffSettingComponent.html',
        }),
        __metadata("design:paramtypes", [EntityResourceService_1.EntityResourceService])
    ], TariffSettingComponent);
    return TariffSettingComponent;
}(BaseComponent_1.BaseComponent));
exports.TariffSettingComponent = TariffSettingComponent;
var TariffSettingStep = /** @class */ (function (_super) {
    __extends(TariffSettingStep, _super);
    function TariffSettingStep(iStep, index, father) {
        var _this = _super.call(this) || this;
        _this.father = father;
        _this.DataContext = _this;
        _this.step = null;
        _this.Index = index;
        if (iStep) {
            _this.step = +iStep;
        }
        return _this;
    }
    Object.defineProperty(TariffSettingStep.prototype, "Step", {
        get: function () { return this.step; },
        set: function (value) {
            if (this.step != value) {
                this.step = value;
                this.father.BuildDefaultPriceSteps();
            }
        },
        enumerable: true,
        configurable: true
    });
    TariffSettingStep.prototype.DeleteClicked = function () {
        this.Step = null;
    };
    return TariffSettingStep;
}(BaseComponent_1.BaseComponent));
//# sourceMappingURL=TariffSettingComponent.js.map