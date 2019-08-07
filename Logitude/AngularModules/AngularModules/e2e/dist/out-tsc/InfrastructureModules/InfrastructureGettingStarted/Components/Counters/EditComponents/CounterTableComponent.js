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
var Tools_1 = require("../../../../../Infrastructure/Tools");
var CounterDefinitionPM_1 = require("../../../../../Common/EntityPMs/CounterDefinitionPM");
var BaseComponent_1 = require("../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var SessionLocator_1 = require("../../../../../Infrastructure/Utilities/SessionLocator");
var CountersDomainService_1 = require("../../../../../Common/Services/CountersDomainService");
var Validator_1 = require("../../../../../Infrastructure/Validators/Validator");
var MessageWindow_1 = require("../../../../../Controls/Windows/MessageWindow");
var CounterTableComponent = /** @class */ (function (_super) {
    __extends(CounterTableComponent, _super);
    function CounterTableComponent() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.ObjectTableName = "CounterDefinition";
        _this.IsCounterUsed = false;
        _this.IsResourcesReady = false;
        _this.ValidationErrorsList = [];
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        return _this;
    }
    CounterTableComponent.prototype.SetWindowArgs = function (args) {
        var _this = this;
        this.CounterId = args["CounterId"];
        if (this.CounterId) {
            this.CurrentSession.StartBusyIndicatorLoading();
            var myService = new CountersDomainService_1.CountersDomainService();
            myService.GetCounterAPIHelper(this.CounterId).subscribe(function (myResponse) {
                if (myResponse.HasError) {
                    _this.ValidationErrorsList = myResponse.ErrorsArray;
                }
                else {
                    _this.APIHelper = myResponse.Result;
                    if (_this.APIHelper) {
                        _this.CounterPM = _this.APIHelper.CounterPM;
                        _this.IsCounterUsed = _this.APIHelper.IsCounterUsed;
                        _this.InitializeDefinitions();
                        _this.SetUIProperties();
                    }
                    _this.CalculateSampleValue();
                }
                _this.IsResourcesReady = true;
                _this.CurrentSession.StopBusyIndicator();
            });
        }
    };
    CounterTableComponent.prototype.SetUIProperties = function () {
        this.UIProperties.SetEnabled("Prefix", this.ObjectTableName, !this.IsCounterUsed);
        this.UIProperties.SetEnabled("StartNumber", this.ObjectTableName, !this.IsCounterUsed);
        this.UIProperties.SetEnabled("CounterSize", this.ObjectTableName, !this.IsCounterUsed);
    };
    CounterTableComponent.prototype.InitializeDefinitions = function () {
        // Dummy:Init
        this.EntityPM = new CounterDefinitionPM_1.CounterDefinitionPM();
        this.EntityPM.CounterId = this.CounterPM.Id;
        this.EntityPM.Tenant = this.CounterPM.Tenant;
        this.EntityPM.UniquePerPrefix = false;
        this.EntityPM.StartNumber = 1000;
        this.EntityPM.StartNumber_Old = 0;
        if (this.APIHelper.CounterDefinitions.length == 0) {
            this.APIHelper.CounterDefinitions.push(this.EntityPM);
        }
        else {
            this.EntityPM = this.APIHelper.CounterDefinitions[0];
        }
    };
    Object.defineProperty(CounterTableComponent.prototype, "Prefix", {
        get: function () { return this.EntityPM.Prefix; },
        set: function (value) {
            if (this.EntityPM.Prefix != value) {
                this.EntityPM.Prefix = value;
                this.CalculateSampleValue();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CounterTableComponent.prototype, "CounterSize", {
        get: function () { return this.EntityPM.CounterSize; },
        set: function (value) {
            if (this.EntityPM.CounterSize != value) {
                this.EntityPM.CounterSize = value;
                this.CalculateSampleValue();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CounterTableComponent.prototype, "StartNumber", {
        get: function () { return this.EntityPM.StartNumber; },
        set: function (value) {
            if (this.EntityPM.StartNumber != value) {
                this.EntityPM.StartNumber = value;
                this.CalculateSampleValue();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CounterTableComponent.prototype, "UniquePerPrefix", {
        get: function () { return this.EntityPM.UniquePerPrefix; },
        set: function (value) {
            if (this.EntityPM.UniquePerPrefix != value) {
                this.EntityPM.UniquePerPrefix = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    CounterTableComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    CounterTableComponent.prototype.OkButtonClicked = function () {
        var _this = this;
        var counterLength = 15;
        if (this.ObjectTableName == "Shipment" || this.ObjectTableName == "Quote") {
            counterLength = 20;
        }
        var isValidGreaterStartNumber = true;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.StartNumber)) {
            if (this.StartNumber < this.EntityPM.StartNumber_Old) {
                isValidGreaterStartNumber = false;
            }
        }
        if (!isValidGreaterStartNumber) {
            var messageWindow = new MessageWindow_1.MessageWindow();
            messageWindow.Show("The new start number must be greater than current start number!");
        }
        else {
            var errors = [];
            if (this.CounterSize > counterLength) {
                errors.push("Maximum size allowed for counter is " + counterLength);
            }
            if (this.UniquePerPrefix == true) {
                Validator_1.Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
                if (this.UniquePerPrefix && !Tools_1.AppTool.IsNullOrEmpty(this.Prefix) && !Tools_1.AppTool.IsNullOrEmpty(this.StartNumber)) {
                    if ((this.StartNumber).toString().length + Tools_1.AppTool.GetCounterPrefixLength(this.Prefix) > counterLength) {
                        errors.push("Maximum length allowed for [Startnumber + Prefix] is " + counterLength);
                    }
                }
            }
            else if ((this.StartNumber).toString().length + Tools_1.AppTool.GetCounterPrefixLength(this.Prefix) > counterLength) {
                errors.push("Maximum length allowed for [Prefix + StartNumber] is " + counterLength);
            }
            this.ValidationErrorsList = errors;
            if (errors.length == 0) {
                var isDirty = false;
                if (this.EntityPM.IsDirty) {
                    isDirty = true;
                }
                if (!isDirty) {
                    this.CurrentSession.CloseCurrentWindow();
                }
                else {
                    this.CurrentSession.StartBusyIndicatorSaving();
                    var myService = new CountersDomainService_1.CountersDomainService();
                    myService.Post(this.APIHelper).subscribe(function (myResponse) {
                        _this.CurrentSession.StopBusyIndicator();
                        if (myResponse.HasError) {
                            _this.ValidationErrorsList = myResponse.ErrorsArray;
                        }
                        else {
                            _this.CurrentSession.CloseCurrentWindowEmit("Ok");
                        }
                    });
                }
            }
        }
    };
    CounterTableComponent.prototype.CalculateSampleValue = function () {
        this.SampleValue = Tools_1.AppTool.GetCounterResolvedNumber(this.Prefix, this.StartNumber, "", this.CounterSize);
    };
    CounterTableComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './CounterTableComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], CounterTableComponent);
    return CounterTableComponent;
}(BaseComponent_1.BaseComponent));
exports.CounterTableComponent = CounterTableComponent;
//# sourceMappingURL=CounterTableComponent.js.map