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
var TenantSettingPM_1 = require("../../../../../Infrastructure/EntityPMs/TenantSettingPM");
var BaseComponent_1 = require("../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var SessionLocator_1 = require("../../../../../Infrastructure/Utilities/SessionLocator");
var FeatureLocator_1 = require("../../../../../Infrastructure/Utilities/FeatureLocator");
var CountersDomainService_1 = require("../../../../../Common/Services/CountersDomainService");
var Validator_1 = require("../../../../../Infrastructure/Validators/Validator");
var MessageWindow_1 = require("../../../../../Controls/Windows/MessageWindow");
var CounterHAWBComponent = /** @class */ (function (_super) {
    __extends(CounterHAWBComponent, _super);
    function CounterHAWBComponent() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.ObjectTableName = "CounterDefinition";
        _this.IsFBLVisible = false;
        _this.IsHBLVisible = false;
        _this.HasAllTransportsFeature = false;
        _this.IsResourcesReady = false;
        _this.ValidationErrorsList = [];
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("Shipment", "ALLTRANSPORTMODES")) {
            _this.HasAllTransportsFeature = true;
        }
        return _this;
    }
    CounterHAWBComponent.prototype.SetWindowArgs = function (args) {
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
                    _this.CounterAPIHelper = myResponse.Result;
                    if (_this.CounterAPIHelper) {
                        _this.EntityPM = _this.CounterAPIHelper.CounterPM;
                        _this.InitializeDefinitions();
                        _this.InitializeTenantSettings();
                    }
                }
                _this.IsResourcesReady = true;
                _this.CurrentSession.StopBusyIndicator();
            });
        }
    };
    CounterHAWBComponent.prototype.InitializeDefinitions = function () {
        this.CounterDefinitionHWB = this.CounterAPIHelper.CounterDefinitions.filter(function (f) { return f.Parameter1 == "A"; })[0];
        if (this.CounterDefinitionHWB == null) {
            this.CounterDefinitionHWB = new CounterDefinitionPM_1.CounterDefinitionPM();
            this.CounterDefinitionHWB.Tenant = this.EntityPM.Tenant;
            this.CounterDefinitionHWB.CounterId = this.EntityPM.Id;
            this.CounterDefinitionHWB.Parameter1 = "A";
            this.CounterDefinitionHWB.StartNumber = 1000;
            this.CounterDefinitionHWB.StartNumber_Old = 0;
            this.CounterDefinitionHWB.UniquePerPrefix = false;
        }
        if (this.HasAllTransportsFeature) {
            this.CounterDefinitionFBL = this.CounterAPIHelper.CounterDefinitions.filter(function (f) { return f.Parameter1 == "O"; })[0];
            this.CounterDefinitionHBL = this.CounterAPIHelper.CounterDefinitions.filter(function (f) { return f.Parameter1 == "I"; })[0];
            if (this.CounterDefinitionFBL == null) {
                this.CounterDefinitionFBL = new CounterDefinitionPM_1.CounterDefinitionPM();
                this.CounterDefinitionFBL.Tenant = this.EntityPM.Tenant;
                this.CounterDefinitionFBL.CounterId = this.EntityPM.Id;
                this.CounterDefinitionFBL.Parameter1 = "O";
                this.CounterDefinitionFBL.StartNumber = 1000;
                this.CounterDefinitionFBL.StartNumber_Old = 0;
                this.CounterDefinitionFBL.UniquePerPrefix = false;
            }
            if (this.CounterDefinitionHBL == null) {
                this.CounterDefinitionHBL = new CounterDefinitionPM_1.CounterDefinitionPM();
                this.CounterDefinitionHBL.Tenant = this.EntityPM.Tenant;
                this.CounterDefinitionHBL.CounterId = this.EntityPM.Id;
                this.CounterDefinitionHBL.Parameter1 = "I";
                this.CounterDefinitionHBL.StartNumber = 1000;
                this.CounterDefinitionHBL.StartNumber_Old = 0;
                this.CounterDefinitionHBL.UniquePerPrefix = false;
            }
        }
    };
    CounterHAWBComponent.prototype.InitializeTenantSettings = function () {
        this.SettingHWB = this.CounterAPIHelper.TenantSettings.filter(function (f) { return f.SettingCode == "HAWBCounterA_E_D"; })[0];
        if (this.SettingHWB == null) {
            this.SettingHWB = new TenantSettingPM_1.TenantSettingPM();
            this.SettingHWB.Tenant = this.EntityPM.Tenant;
            this.SettingHWB.ObjectTableId = this.EntityPM.ObjectTableId;
            this.SettingHWB.SettingCode = "HAWBCounterA_E_D";
            this.SettingHWB.SettingValue = "None";
            this.CounterAPIHelper.TenantSettings.push(this.SettingHWB);
        }
        if (this.HasAllTransportsFeature) {
            this.SettingFBL = this.CounterAPIHelper.TenantSettings.filter(function (f) { return f.SettingCode == "HAWBCounterO_E_D"; })[0];
            this.SettingHBL = this.CounterAPIHelper.TenantSettings.filter(function (f) { return f.SettingCode == "HAWBCounterI_E_D"; })[0];
            if (this.SettingFBL == null) {
                this.SettingFBL = new TenantSettingPM_1.TenantSettingPM();
                this.SettingFBL.Tenant = this.EntityPM.Tenant;
                this.SettingFBL.ObjectTableId = this.EntityPM.ObjectTableId;
                this.SettingFBL.SettingCode = "HAWBCounterO_E_D";
                this.SettingFBL.SettingValue = "None";
                this.CounterAPIHelper.TenantSettings.push(this.SettingFBL);
            }
            if (this.SettingHBL == null) {
                this.SettingHBL = new TenantSettingPM_1.TenantSettingPM();
                this.SettingHBL.Tenant = this.EntityPM.Tenant;
                this.SettingHBL.ObjectTableId = this.EntityPM.ObjectTableId;
                this.SettingHBL.SettingCode = "HAWBCounterI_E_D";
                this.SettingHBL.SettingValue = "None";
                this.CounterAPIHelper.TenantSettings.push(this.SettingHBL);
            }
            this.IsFBLVisible = true;
            this.IsHBLVisible = true;
        }
    };
    Object.defineProperty(CounterHAWBComponent.prototype, "HWBSettingValue", {
        // HWB
        get: function () { return this.SettingHWB.SettingValue; },
        set: function (value) {
            if (this.SettingHWB.SettingValue != value) {
                this.SettingHWB.SettingValue = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CounterHAWBComponent.prototype, "HWBPrefix", {
        get: function () { return this.SettingHWB.Prefix; },
        set: function (value) {
            if (this.SettingHWB.Prefix != value) {
                this.SettingHWB.Prefix = value;
                //this.UIProperties.SetValidity("HWBPrefix", "CounterDefinition", true, null);
                //if (!AppTool.IsNullOrEmpty(value)) {
                //    if (value.length > 10) {
                //        this.UIProperties.SetValidity("HWBPrefix", null, false, "Prefix Field must be less than 10");
                //    }
                //}
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CounterHAWBComponent.prototype, "HWBSize", {
        get: function () { return this.SettingHWB.Size; },
        set: function (value) {
            if (this.SettingHWB.Size != value) {
                this.SettingHWB.Size = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CounterHAWBComponent.prototype, "HWBDontIncludeDirects", {
        get: function () { return this.SettingHWB.DontIncludeDirects; },
        set: function (value) {
            if (this.SettingHWB.DontIncludeDirects != value) {
                this.SettingHWB.DontIncludeDirects = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CounterHAWBComponent.prototype, "HWBStartNumber", {
        get: function () { return this.CounterDefinitionHWB.StartNumber; },
        set: function (value) {
            if (this.CounterDefinitionHWB.StartNumber != value) {
                this.CounterDefinitionHWB.StartNumber = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CounterHAWBComponent.prototype, "FBLSettingValue", {
        // FBL
        get: function () { return this.SettingFBL.SettingValue; },
        set: function (value) {
            if (this.SettingFBL.SettingValue != value) {
                this.SettingFBL.SettingValue = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CounterHAWBComponent.prototype, "FBLPrefix", {
        get: function () { return this.SettingFBL.Prefix; },
        set: function (value) {
            if (this.SettingFBL.Prefix != value) {
                this.SettingFBL.Prefix = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CounterHAWBComponent.prototype, "FBLSize", {
        get: function () { return this.SettingFBL.Size; },
        set: function (value) {
            if (this.SettingFBL.Size != value) {
                this.SettingFBL.Size = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CounterHAWBComponent.prototype, "FBLDontIncludeDirects", {
        get: function () { return this.SettingFBL.DontIncludeDirects; },
        set: function (value) {
            if (this.SettingFBL.DontIncludeDirects != value) {
                this.SettingFBL.DontIncludeDirects = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CounterHAWBComponent.prototype, "FBLStartNumber", {
        get: function () { return this.CounterDefinitionFBL.StartNumber; },
        set: function (value) {
            if (this.CounterDefinitionFBL.StartNumber != value) {
                this.CounterDefinitionFBL.StartNumber = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CounterHAWBComponent.prototype, "HBLSettingValue", {
        // HBL
        get: function () { return this.SettingHBL.SettingValue; },
        set: function (value) {
            if (this.SettingHBL.SettingValue != value) {
                this.SettingHBL.SettingValue = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CounterHAWBComponent.prototype, "HBLPrefix", {
        get: function () { return this.SettingHBL.Prefix; },
        set: function (value) {
            if (this.SettingHBL.Prefix != value) {
                this.SettingHBL.Prefix = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CounterHAWBComponent.prototype, "HBLSize", {
        get: function () { return this.SettingHBL.Size; },
        set: function (value) {
            if (this.SettingHBL.Size != value) {
                this.SettingHBL.Size = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CounterHAWBComponent.prototype, "HBLDontIncludeDirects", {
        get: function () { return this.SettingHBL.DontIncludeDirects; },
        set: function (value) {
            if (this.SettingHBL.DontIncludeDirects != value) {
                this.SettingHBL.DontIncludeDirects = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CounterHAWBComponent.prototype, "HBLStartNumber", {
        get: function () { return this.CounterDefinitionHBL.StartNumber; },
        set: function (value) {
            if (this.CounterDefinitionHBL.StartNumber != value) {
                this.CounterDefinitionHBL.StartNumber = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    CounterHAWBComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    CounterHAWBComponent.prototype.OkButtonClicked = function () {
        var _this = this;
        var isDirty = false;
        if (this.EntityPM.IsDirty) {
            isDirty = true;
        }
        if (this.CounterAPIHelper.TenantSettings.filter(function (f) { return f.IsDirty; }).length > 0) {
            isDirty = true;
        }
        if (this.CounterDefinitionHWB.Id != null && this.CounterDefinitionHWB.IsDirty) {
            isDirty = true;
        }
        if (this.CounterDefinitionFBL.Id != null && this.CounterDefinitionFBL.IsDirty) {
            isDirty = true;
        }
        if (this.CounterDefinitionHBL.Id != null && this.CounterDefinitionHBL.IsDirty) {
            isDirty = true;
        }
        if (this.HWBSettingValue == "Counter") {
            if (this.CounterAPIHelper.CounterDefinitions.filter(function (f) { return f.Parameter1 == "A"; }).length == 0) {
                this.CounterAPIHelper.CounterDefinitions.push(this.CounterDefinitionHWB);
                isDirty = true;
            }
        }
        if (this.HasAllTransportsFeature) {
            if (this.FBLSettingValue == "Counter") {
                if (this.CounterAPIHelper.CounterDefinitions.filter(function (f) { return f.Parameter1 == "O"; }).length == 0) {
                    this.CounterAPIHelper.CounterDefinitions.push(this.CounterDefinitionFBL);
                    isDirty = true;
                }
            }
            if (this.HBLSettingValue == "Counter") {
                if (this.CounterAPIHelper.CounterDefinitions.filter(function (f) { return f.Parameter1 == "I"; }).length == 0) {
                    this.CounterAPIHelper.CounterDefinitions.push(this.CounterDefinitionHBL);
                    isDirty = true;
                }
            }
        }
        if (!isDirty) {
            this.CurrentSession.CloseCurrentWindow();
        }
        else {
            var isValidGreaterStartNumber = true;
            var isFieldLengthValid_HWB = true;
            var isFieldLengthValid_FBL = true;
            var isFieldLengthValid_HBL = true;
            if (this.HWBSettingValue == "Counter") {
                if (!Tools_1.AppTool.IsNullOrEmpty(this.CounterDefinitionHWB.StartNumber)) {
                    if (this.CounterDefinitionHWB.StartNumber < this.CounterDefinitionHWB.StartNumber_Old) {
                        isValidGreaterStartNumber = false;
                    }
                    isFieldLengthValid_HWB = this.ValidateFieldLength(this.CounterDefinitionHWB.StartNumber, this.HWBSize, this.HWBPrefix);
                }
            }
            if (this.HasAllTransportsFeature) {
                if (this.FBLSettingValue == "Counter") {
                    if (!Tools_1.AppTool.IsNullOrEmpty(this.CounterDefinitionFBL.StartNumber)) {
                        if (this.CounterDefinitionFBL.StartNumber < this.CounterDefinitionFBL.StartNumber_Old) {
                            isValidGreaterStartNumber = false;
                        }
                        isFieldLengthValid_FBL = this.ValidateFieldLength(this.CounterDefinitionFBL.StartNumber, this.FBLSize, this.FBLPrefix);
                    }
                }
                if (this.HBLSettingValue == "Counter") {
                    if (!Tools_1.AppTool.IsNullOrEmpty(this.CounterDefinitionHBL.StartNumber)) {
                        if (this.CounterDefinitionHBL.StartNumber < this.CounterDefinitionHBL.StartNumber_Old) {
                            isValidGreaterStartNumber = false;
                        }
                        isFieldLengthValid_HBL = this.ValidateFieldLength(this.CounterDefinitionHBL.StartNumber, this.HBLSize, this.HBLPrefix);
                    }
                }
            }
            if (!isValidGreaterStartNumber) {
                var messageWindow = new MessageWindow_1.MessageWindow();
                messageWindow.Show("The new start number must be greater than current start number!");
            }
            else if (!isFieldLengthValid_HWB) {
                var messageWindow = new MessageWindow_1.MessageWindow();
                messageWindow.Show("HAWB number max length is 20");
            }
            else if (!isFieldLengthValid_FBL) {
                var messageWindow = new MessageWindow_1.MessageWindow();
                messageWindow.Show("FBL number max length is 20");
            }
            else if (!isFieldLengthValid_HBL) {
                var messageWindow = new MessageWindow_1.MessageWindow();
                messageWindow.Show("HBL number max length is 20");
            }
            else {
                var errors = [];
                Validator_1.Validator.TryValidateObject(this.CounterDefinitionHWB, this.ObjectTableName, errors);
                Validator_1.Validator.TryValidateObject(this.CounterDefinitionFBL, this.ObjectTableName, errors);
                Validator_1.Validator.TryValidateObject(this.CounterDefinitionHBL, this.ObjectTableName, errors);
                var isPrefixLengthValid = this.ValidatePrefixLength();
                if (!isPrefixLengthValid) {
                    errors.push("Prefix Field must be less than 10");
                }
                this.ValidationErrorsList = errors;
                if (errors.length == 0) {
                    this.CurrentSession.StartBusyIndicatorSaving();
                    var myService = new CountersDomainService_1.CountersDomainService();
                    myService.Post(this.CounterAPIHelper).subscribe(function (myResponse) {
                        _this.CurrentSession.StopBusyIndicator();
                        if (myResponse.HasError) {
                            _this.ValidationErrorsList = myResponse.ErrorsArray;
                        }
                        else {
                            SessionLocator_1.SessionLocator.TenantSettings = _this.CounterAPIHelper.TenantSettings;
                            _this.CurrentSession.CloseCurrentWindowEmit("Ok");
                        }
                    });
                }
            }
        }
    };
    CounterHAWBComponent.prototype.ValidatePrefixLength = function () {
        var isValid = true;
        if (this.HWBPrefix) {
            if (this.HWBPrefix.length > 10) {
                isValid = false;
            }
        }
        if (this.FBLPrefix) {
            if (this.FBLPrefix.length > 10) {
                isValid = false;
            }
        }
        if (this.HBLPrefix) {
            if (this.HBLPrefix.length > 10) {
                isValid = false;
            }
        }
        return isValid;
    };
    CounterHAWBComponent.prototype.ValidateFieldLength = function (myStartNumber, mySize, myPrefix) {
        var isValid = true;
        var dbFieldLength = 20;
        var expectedField = "";
        if (!Tools_1.AppTool.IsNullOrZero(this.CounterAPIHelper.LastDBValue)) {
            expectedField = this.CounterAPIHelper.LastDBValue + 1 + "";
        }
        else {
            expectedField = myStartNumber + 1 + "";
        }
        if (!Tools_1.AppTool.IsNullOrZero(mySize)) {
            expectedField = Tools_1.AppTool.PadLeft(expectedField, mySize, "0");
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(myPrefix)) {
            expectedField = myPrefix + expectedField;
        }
        if (expectedField) {
            if (expectedField.length > dbFieldLength) {
                isValid = false;
            }
        }
        return isValid;
    };
    CounterHAWBComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './CounterHAWBComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], CounterHAWBComponent);
    return CounterHAWBComponent;
}(BaseComponent_1.BaseComponent));
exports.CounterHAWBComponent = CounterHAWBComponent;
//# sourceMappingURL=CounterHAWBComponent.js.map