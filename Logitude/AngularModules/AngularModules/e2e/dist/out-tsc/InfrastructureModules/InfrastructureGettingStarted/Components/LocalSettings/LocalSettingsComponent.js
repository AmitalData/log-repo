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
var DateTimeZone_1 = require("../../../../Infrastructure/Utilities/DateTimeZone");
var TenantPM_1 = require("../../../../Common/EntityPMs/TenantPM");
var Validator_1 = require("../../../../Infrastructure/Validators/Validator");
var CommonDomainService_1 = require("../../../../Common/Services/CommonDomainService");
var TenantPMService_1 = require("../../../../Common/Services/StandardPMs/TenantPMService");
var InfraSettings_1 = require("../../../../Infrastructure/Utilities/InfraSettings");
var MessageWindow_1 = require("../../../../Controls/Windows/MessageWindow");
var LocalSettingsComponent = /** @class */ (function (_super) {
    __extends(LocalSettingsComponent, _super);
    function LocalSettingsComponent() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.ObjectTableName = "Tenant";
        _this.TenantPm = new TenantPM_1.TenantPM();
        _this.demoMessageVisibility = false;
        _this.IsVisible = false;
        _this.oldLanguageCode = null;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        //Languages List 
        _this.LanguagesList = [];
        //TimeZonesList 
        _this.TimeZonesList = [];
        //DateTimeFormatsList
        _this.DateTimeFormatsList = [];
        _this.TimeZonesList = DateTimeZone_1.DateTimeZone.GetTimeZonesList();
        _this.DateTimeFormatsList = DateTimeZone_1.DateTimeZone.GetDateTimeFormats();
        return _this;
    }
    LocalSettingsComponent.prototype.ngOnInit = function () {
        this.LoadTenantPMMethod();
    };
    Object.defineProperty(LocalSettingsComponent.prototype, "DemoMessageVisibility", {
        //Tenant 65
        get: function () {
            var result = false;
            if (this.TenantPm.Id == 65) {
                result = true;
                if (SessionLocator_1.SessionLocator.LoggedUserPM.Email.toLowerCase() == "customercare@logitudeworld.com‏") {
                    result = false;
                }
            }
            return result;
        },
        enumerable: true,
        configurable: true
    });
    LocalSettingsComponent.prototype.SetUIPropertiesHitVisible = function () {
        this.UIProperties.SetEnabled("ShowDayLightSettings", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("DayLightStartDate", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("DayLightEndDate", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("DayLightOffset", this.ObjectTableName, false);
    };
    // Load Tenant 
    LocalSettingsComponent.prototype.LoadTenantPMMethod = function () {
        var _this = this;
        var myService = new TenantPMService_1.TenantPMService();
        myService.get(SessionLocator_1.SessionLocator.TenantPM.Id).subscribe(function (response) {
            _this.TenantPm = response.Result;
            _this.oldLanguageCode = _this.TenantPm.Language;
            if (_this.TenantPm.DayLightOffset != 0) {
                _this.ShowDayLightSettings = true;
            }
            if (_this.TenantPm.Id == 65 && SessionLocator_1.SessionLocator.LoggedUserPM.Email.toLowerCase() != "customercare@logitudeworld.com‏") {
                _this.SetUIPropertiesHitVisible();
            }
            _this.TimeZoneSelectedItem = _this.TimeZonesList.filter(function (a) { return a.BaseUtcOffset == _this.TenantPm.TimeZoneOffset; })[0];
            _this.LoadLanguagesListMethod();
            _this.IsVisible = true;
        });
    };
    LocalSettingsComponent.prototype.LoadLanguagesListMethod = function () {
        var _this = this;
        var myService = new CommonDomainService_1.CommonDomainService();
        myService.GetTranslationHeadersByTenant(this.TenantPm.Id).subscribe(function (myResult) {
            _this.LanguagesList = myResult;
            _this.LanguageSelectedItem = _this.LanguagesList.filter(function (d) { return d.Code == _this.TenantPm.Language; })[0];
        });
    };
    Object.defineProperty(LocalSettingsComponent.prototype, "LanguageSelectedItem", {
        get: function () { return this.languageSelectedItem; },
        set: function (value) {
            if (this.languageSelectedItem != value) {
                this.languageSelectedItem = value;
                if (value == null) {
                    this.selectedLanguageCode = null;
                }
                else {
                    this.selectedLanguageCode = value.Code;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    LocalSettingsComponent.prototype.LanguageSelectedChange = function (item) {
        this.LanguageSelectedItem = this.LanguagesList.filter(function (d) { return d.Code == item.Code; })[0];
        this.selectedLanguageCode = item.Code;
    };
    Object.defineProperty(LocalSettingsComponent.prototype, "TimeZoneSelectedItem", {
        get: function () { return this.timeZoneSelectedItem; },
        set: function (value) {
            if (this.timeZoneSelectedItem != value) {
                this.timeZoneSelectedItem = value;
                if (value == null) {
                    this.TimeZoneOffset = null;
                }
                else {
                    this.TimeZoneOffset = value.BaseUtcOffset;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(LocalSettingsComponent.prototype, "DateTimeFormatSelectedItem", {
        get: function () {
            var _this = this;
            return this.DateTimeFormatsList.filter(function (d) { return d.Format == _this.TenantPm.DateTimeFormat; })[0];
        },
        set: function (value) {
            this.TenantPm.DateTimeFormat = value.Format;
        },
        enumerable: true,
        configurable: true
    });
    LocalSettingsComponent.prototype.DateTimeFormatSelectedChange = function (item) {
        this.TenantPm.DateTimeFormat = item.Format;
    };
    Object.defineProperty(LocalSettingsComponent.prototype, "TimeZoneOffset", {
        //Props
        get: function () {
            return this.TenantPm.TimeZoneOffset;
        },
        set: function (value) {
            if (this.TenantPm.TimeZoneOffset != value) {
                this.TenantPm.TimeZoneOffset = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(LocalSettingsComponent.prototype, "DayLightStartDate", {
        get: function () {
            return this.TenantPm.DayLightStartDate;
        },
        set: function (value) {
            if (this.TenantPm.DayLightStartDate != value) {
                this.TenantPm.DayLightStartDate = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(LocalSettingsComponent.prototype, "NumberFormatCode", {
        get: function () {
            if (this.TenantPm.NumberFormatCode == null || this.TenantPm.NumberFormatCode == "")
                return "CD";
            else
                return this.TenantPm.NumberFormatCode;
        },
        set: function (value) {
            if (this.TenantPm.NumberFormatCode != value) {
                this.TenantPm.NumberFormatCode = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(LocalSettingsComponent.prototype, "DayLightEndDate", {
        get: function () {
            return this.TenantPm.DayLightEndDate;
        },
        set: function (value) {
            if (this.TenantPm.DayLightEndDate != value) {
                this.TenantPm.DayLightEndDate = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(LocalSettingsComponent.prototype, "DayLightOffset", {
        get: function () {
            return this.TenantPm.DayLightOffset;
        },
        set: function (value) {
            if (this.TenantPm.DayLightOffset != value) {
                this.TenantPm.DayLightOffset = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(LocalSettingsComponent.prototype, "ShowDayLightSettings", {
        get: function () { return this.showDayLightSettings; },
        set: function (value) {
            if (this.showDayLightSettings != value) {
                this.showDayLightSettings = value;
                if (!value) {
                    this.DayLightOffset = 0;
                    this.DayLightStartDate = null;
                    this.DayLightEndDate = null;
                }
                else {
                    this.DayLightOffset = 1;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(LocalSettingsComponent.prototype, "IsHitTestVisible", {
        get: function () {
            var result = false;
            if (this.TenantPm.Id == 65) {
                result = true;
                if (SessionLocator_1.SessionLocator.LoggedUserPM.Email.toLowerCase() == "customercare@logitudeworld.com‏") {
                    result = false;
                }
            }
            return result;
        },
        enumerable: true,
        configurable: true
    });
    // Commands 
    LocalSettingsComponent.prototype.CancelButtonClicked = function () {
        this.TenantPm = null;
        this.CurrentSession.CloseCurrentWindow();
    };
    LocalSettingsComponent.prototype.OkButtonClicked = function () {
        var _this = this;
        var errors = [];
        Validator_1.Validator.TryValidateObject(this.DataContext.TenantPm, this.DataContext.ObjectTableName, errors);
        if (this.TenantPm.DayLightOffset != 0) {
            if (this.TenantPm.DayLightEndDate == null || this.TenantPm.DayLightStartDate == null) {
                errors.push("DayLightStartDate and DayLightEndDate should have values");
            }
        }
        this.ValidationErrorsList = errors;
        if (this.ValidationErrorsList.length == 0) {
            if (SessionLocator_1.SessionLocator.TenantPM.Language != this.selectedLanguageCode) {
                this.TenantPm.Language = this.selectedLanguageCode;
                this.reloadingTranslation = true;
                var messageWindow = new MessageWindow_1.MessageWindow();
                messageWindow.Show("In Order to apply the language change, please logout and login again");
                messageWindow.WindowClosed.subscribe(function ($event) {
                    _this.SubmitChanges();
                });
            }
            else {
                this.SubmitChanges();
            }
        }
    };
    LocalSettingsComponent.prototype.SubmitChanges = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicator("Saving...");
        var myService = new TenantPMService_1.TenantPMService();
        myService.update(this.TenantPm).subscribe(function (myResponse) {
            if (myResponse) {
                if (!myResponse.HasError) {
                    InfraSettings_1.InfraSettings.TenantPM = _this.TenantPm;
                    _this.CurrentSession.CloseCurrentWindowEmit("ok");
                    ////Complete work 
                    //var datetimeformat: string = "dd\\/MM\\/yyyy";
                    //if (!LogitudeUtilitie3s.IsNullOrEmpty(this.TenantPm.DateTimeFormat)) {
                    //    datetimeformat = this.TenantPm.DateTimeFormat;
                    //}
                    //if (this.reloadingTranslation) {
                    //    this.ReloadTranslationMethod();
                    //}
                }
                else {
                    _this.ValidationErrorsList = myResponse.ErrorsArray;
                    _this.CurrentSession.StopBusyIndicator();
                }
            }
        });
    };
    LocalSettingsComponent.prototype.ReloadTranslationMethod = function () {
    };
    LocalSettingsComponent = __decorate([
        core_1.Component({
            selector: 'LocalSettingsComponent',
            moduleId: module.id,
            templateUrl: './LocalSettingsComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], LocalSettingsComponent);
    return LocalSettingsComponent;
}(BaseComponent_1.BaseComponent));
exports.LocalSettingsComponent = LocalSettingsComponent;
//# sourceMappingURL=LocalSettingsComponent.js.map