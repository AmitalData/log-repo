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
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var CommonDomainService_1 = require("../../../../Common/Services/CommonDomainService");
var SelectLanguagesComponent = /** @class */ (function () {
    function SelectLanguagesComponent() {
        this.LanguagesList = [];
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.LoadLanguages();
    }
    SelectLanguagesComponent.prototype.LoadLanguages = function () {
        var _this = this;
        var myService = new CommonDomainService_1.CommonDomainService();
        myService.GetTranslationHeadersByTenant(SessionLocator_1.SessionLocator.Tenant).subscribe(function (myResult) {
            _this.LanguagesList = myResult;
            _this.LanguageSelectedItem = _this.LanguagesList.filter(function (d) { return d.Code == SessionLocator_1.SessionLocator.TenantPM.Language; })[0];
        });
    };
    Object.defineProperty(SelectLanguagesComponent.prototype, "LanguageSelectedItem", {
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
    SelectLanguagesComponent.prototype.LanguageSelectedChange = function (item) {
        this.LanguageSelectedItem = this.LanguagesList.filter(function (d) { return d.Code == item.Code; })[0];
        this.selectedLanguageCode = item.Code;
    };
    // Commands
    SelectLanguagesComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    SelectLanguagesComponent.prototype.OkButtonClicked = function () {
        this.RunTranslationWindow();
    };
    SelectLanguagesComponent.prototype.RunTranslationWindow = function () {
        var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
        logitudeWindow.IsFillScreen = true;
        logitudeWindow.Title = "Translate Labels";
        logitudeWindow.WindowArgs = this.selectedLanguageCode;
        logitudeWindow.Show('./InfrastructureModules/InfrastructureCustomization/Components/TranslationLabels/TranslateLabelsComponent');
        this.CurrentSession.CloseCurrentWindow();
    };
    SelectLanguagesComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './SelectLanguagesComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], SelectLanguagesComponent);
    return SelectLanguagesComponent;
}());
exports.SelectLanguagesComponent = SelectLanguagesComponent;
//# sourceMappingURL=SelectLanguagesComponent.js.map