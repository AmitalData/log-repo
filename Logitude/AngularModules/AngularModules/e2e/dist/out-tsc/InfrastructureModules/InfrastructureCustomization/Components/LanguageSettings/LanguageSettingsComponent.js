"use strict";
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
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var InfraSettings_1 = require("../../../../Infrastructure/Utilities/InfraSettings");
var CommonDomainService_1 = require("../../../../Common/Services/CommonDomainService");
var TenantPMService_1 = require("../../../../Common/Services/StandardPMs/TenantPMService");
var LanguageSettingsComponent = /** @class */ (function () {
    function LanguageSettingsComponent() {
        this.LanguagesCollection = [];
        this.FormatsCollection = [];
        this.ValidationErrorsList = [];
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.LoadTenantPMMethod();
    }
    LanguageSettingsComponent.prototype.LoadTenantPMMethod = function () {
        var _this = this;
        var myService = new TenantPMService_1.TenantPMService();
        myService.get(SessionLocator_1.SessionLocator.TenantPM.Id).subscribe(function (response) {
            _this.TenantPM = response.Result;
            _this.LoadLanguages();
            _this.LoadFormat();
        });
    };
    LanguageSettingsComponent.prototype.LoadFormat = function () {
        this.FormatsCollection.push("en-US");
        this.FormatsCollection.push("ar-SA");
        this.FormatsCollection.push("he-IL");
        this.FormatsCollection.push("de");
    };
    LanguageSettingsComponent.prototype.LoadLanguages = function () {
        var _this = this;
        var myService = new CommonDomainService_1.CommonDomainService();
        myService.GetTranslationHeadersByTenant(SessionLocator_1.SessionLocator.Tenant).subscribe(function (myResult) {
            _this.LanguagesCollection = myResult;
            _this.LanguageSelectedItem = _this.LanguagesCollection.filter(function (d) { return d.Code == SessionLocator_1.SessionLocator.TenantPM.Language; })[0];
        });
    };
    Object.defineProperty(LanguageSettingsComponent.prototype, "LanguageSelectedItem", {
        get: function () { return this.languageSelectedItem; },
        set: function (value) {
            if (this.languageSelectedItem != value) {
                this.languageSelectedItem = value;
                if (value == null) {
                    this.selectedLanguageCode = null;
                }
                else {
                    this.selectedLanguageCode = value.Code;
                    this.TenantPM.Language = value.Description;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    LanguageSettingsComponent.prototype.LanguageSelectedChange = function (item) {
        this.LanguageSelectedItem = this.LanguagesCollection.filter(function (d) { return d.Code == item.Code; })[0];
        this.selectedLanguageCode = item.Code;
    };
    Object.defineProperty(LanguageSettingsComponent.prototype, "SelectedFormat", {
        get: function () { return this.selectedFormat; },
        set: function (value) {
            if (this.selectedFormat != value) {
                this.selectedFormat = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    LanguageSettingsComponent.prototype.FormatsSelectedChange = function (item) {
        this.SelectedFormat = item;
    };
    // Commands
    LanguageSettingsComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    LanguageSettingsComponent.prototype.OkButtonClicked = function () {
        var _this = this;
        var errors = [];
        if (this.LanguageSelectedItem == null) {
            errors.push("Language is Required");
        }
        //if (this.SelectedFormat == null) {
        //    errors.push("Format is Required");
        //}
        this.ValidationErrorsList = errors;
        if (this.ValidationErrorsList.length == 0) {
            this.CurrentSession.StartBusyIndicator("Saving...");
            var myService = new TenantPMService_1.TenantPMService();
            myService.update(this.TenantPM).subscribe(function (myResponse) {
                if (myResponse) {
                    if (!myResponse.HasError) {
                        InfraSettings_1.InfraSettings.TenantPM = _this.TenantPM;
                        _this.CurrentSession.CloseCurrentWindowEmit("ok");
                    }
                    else {
                        _this.ValidationErrorsList = myResponse.ErrorsArray;
                        _this.CurrentSession.StopBusyIndicator();
                    }
                }
            });
        }
    };
    LanguageSettingsComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './LanguageSettingsComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], LanguageSettingsComponent);
    return LanguageSettingsComponent;
}());
exports.LanguageSettingsComponent = LanguageSettingsComponent;
//# sourceMappingURL=LanguageSettingsComponent.js.map