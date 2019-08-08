"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var RulesMainComponent_1 = require("./Components/Customization/RulesComponents/RulesMainComponent");
var AddEditRuleComponent_1 = require("./Components/Customization/RulesComponents/AddEditRuleComponent");
var ObjectFieldsSearchComponent_1 = require("./Components/Customization/RulesComponents/ObjectFieldsSearchComponent");
var AddRuleFieldComponent_1 = require("./Components/Customization/RulesComponents/AddRuleFieldComponent");
var CustomizationMainComponent_1 = require("./Components/Customization/CustomizationMainComponent");
var StandardFieldsComponent_1 = require("./Components/Customization/StandardFieldsComponent");
var EditStandardFieldComponent_1 = require("./Components/Customization/EditStandardFieldComponent");
var ObjectLabelsComponent_1 = require("./Components/Customization/ObjectLabelsComponent");
var CustomFieldsComponent_1 = require("./Components/Customization/CustomFieldsComponent");
var AddEditCustomFieldComponent_1 = require("./Components/Customization/AddEditCustomFieldComponent");
var AddEditPickListComponent_1 = require("./Components/Customization/AddEditPickListComponent");
var SelectLanguagesComponent_1 = require("./Components/TranslationLabels/SelectLanguagesComponent");
var TranslateLabelsComponent_1 = require("./Components/TranslationLabels/TranslateLabelsComponent");
var TranslationComponent_1 = require("./Components/Translations/TranslationComponent");
var DefaultTranslationComponent_1 = require("./Components/Translations/DefaultTranslationComponent");
var ScreenLayoutComponent_1 = require("./Components/Customization/ScreenLayoutComponent");
var LanguageSettingsComponent_1 = require("./Components/LanguageSettings/LanguageSettingsComponent");
exports.Components = [
    CustomizationMainComponent_1.CustomizationMainComponent,
    StandardFieldsComponent_1.StandardFieldsComponent,
    EditStandardFieldComponent_1.EditStandardFieldComponent,
    ObjectLabelsComponent_1.ObjectLabelsComponent,
    SelectLanguagesComponent_1.SelectLanguagesComponent,
    TranslateLabelsComponent_1.TranslateLabelsComponent,
    CustomFieldsComponent_1.CustomFieldsComponent,
    AddEditCustomFieldComponent_1.AddEditCustomFieldComponent,
    RulesMainComponent_1.RulesMainComponent,
    AddEditRuleComponent_1.AddEditRuleComponent,
    TranslationComponent_1.TranslationComponent,
    DefaultTranslationComponent_1.DefaultTranslationComponent,
    AddEditPickListComponent_1.AddEditPickListComponent,
    ObjectFieldsSearchComponent_1.ObjectFieldsSearchComponent,
    AddRuleFieldComponent_1.AddRuleFieldComponent,
    LanguageSettingsComponent_1.LanguageSettingsComponent,
];
exports.ControlsComponents = [
    ScreenLayoutComponent_1.ScreenLayoutComponent,
];
var ModuleDeclarations = /** @class */ (function () {
    function ModuleDeclarations() {
    }
    ModuleDeclarations.Get = function (name) {
        var myResult = null;
        switch (name) {
            case "CustomizationMainComponent": {
                myResult = CustomizationMainComponent_1.CustomizationMainComponent;
                break;
            }
            case "StandardFieldsComponent": {
                myResult = StandardFieldsComponent_1.StandardFieldsComponent;
                break;
            }
            case "EditStandardFieldComponent": {
                myResult = EditStandardFieldComponent_1.EditStandardFieldComponent;
                break;
            }
            case "ObjectLabelsComponent": {
                myResult = ObjectLabelsComponent_1.ObjectLabelsComponent;
                break;
            }
            case "SelectLanguagesComponent": {
                myResult = SelectLanguagesComponent_1.SelectLanguagesComponent;
                break;
            }
            case "TranslateLabelsComponent": {
                myResult = TranslateLabelsComponent_1.TranslateLabelsComponent;
                break;
            }
            case "CustomFieldsComponent": {
                myResult = CustomFieldsComponent_1.CustomFieldsComponent;
                break;
            }
            case "AddEditCustomFieldComponent": {
                myResult = AddEditCustomFieldComponent_1.AddEditCustomFieldComponent;
                break;
            }
            case "TranslationComponent": {
                myResult = TranslationComponent_1.TranslationComponent;
                break;
            }
            case "DefaultTranslationComponent": {
                myResult = DefaultTranslationComponent_1.DefaultTranslationComponent;
                break;
            }
            case "RulesMainComponent": {
                myResult = RulesMainComponent_1.RulesMainComponent;
                break;
            }
            case "AddEditRuleComponent": {
                myResult = AddEditRuleComponent_1.AddEditRuleComponent;
                break;
            }
            case "AddEditPickListComponent": {
                myResult = AddEditPickListComponent_1.AddEditPickListComponent;
                break;
            }
            case "ObjectFieldsSearchComponent": {
                myResult = ObjectFieldsSearchComponent_1.ObjectFieldsSearchComponent;
                break;
            }
            case "AddRuleFieldComponent": {
                myResult = AddRuleFieldComponent_1.AddRuleFieldComponent;
                break;
            }
            case "ScreenLayoutComponent": {
                myResult = ScreenLayoutComponent_1.ScreenLayoutComponent;
                break;
            }
            case "LanguageSettingsComponent": {
                myResult = LanguageSettingsComponent_1.LanguageSettingsComponent;
                break;
            }
        }
        return myResult;
    };
    return ModuleDeclarations;
}());
exports.ModuleDeclarations = ModuleDeclarations;
//# sourceMappingURL=ModuleDeclarations.js.map