"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var SessionLocator_1 = require("../Utilities/SessionLocator");
var Tools_1 = require("../Tools");
var TextCodeTranslator = /** @class */ (function () {
    function TextCodeTranslator() {
    }
    TextCodeTranslator.Translate = function (value, Fix) {
        if (Fix === void 0) { Fix = true; }
        if (SessionLocator_1.SessionLocator.UseCachedData) {
            return this.TranslateCached(value, Fix);
        }
        //console.log('88888888888888:', value);
        var translation = "";
        var cachedTranslationObject = window.TranslationsCache.filter(function (d) { return d.Code === value; })[0];
        if (cachedTranslationObject) {
            translation = cachedTranslationObject.TranslatedText;
        }
        else {
            var translationObject = window.TextCodesTranslations.filter(function (d) { return d.Code == value; })[0];
            if (translationObject) {
                translation = translationObject.TranslatedText;
                window.TranslationsCache.push(translationObject);
            }
        }
        if (window.TranslationsCache.length > 200) {
            window.TranslationsCache.splice(0, 50);
        }
        if (Fix == true) {
            return TextCodeTranslator.FixTranslation(translation);
        }
        else {
            return translation;
        }
    };
    TextCodeTranslator.TranslateCached = function (value, Fix) {
        //console.log('88888888888888:', value);
        if (Fix === void 0) { Fix = true; }
        var translation = "";
        var cachedTranslationObject = window.TextCodesCache.filter(function (d) { return d.Code === value; })[0];
        if (cachedTranslationObject) {
            if (SessionLocator_1.SessionLocator.LoggedUserPM.DontShowLocal) {
                translation = cachedTranslationObject.DefaultText;
            }
            else {
                if (cachedTranslationObject.LocalDefaultText) {
                    translation = cachedTranslationObject.LocalDefaultText;
                }
                else {
                    translation = cachedTranslationObject.DefaultText;
                }
            }
            var codeTranslation = window.TenantTranslations.filter(function (d) { return d.TextCodeId === cachedTranslationObject.Id && d.TranslationHeaderCode == SessionLocator_1.SessionLocator.TenantPM.Language; })[0];
            if (codeTranslation) {
                translation = codeTranslation.TranslatedText;
            }
            else {
                var languageTranslation = window.TenantLanguageTranslations.filter(function (d) { return d.TextCodeId === cachedTranslationObject.Id; })[0];
                if (languageTranslation) {
                    translation = languageTranslation.TranslatedText;
                }
            }
        }
        else {
            var translationObject = window.TextCodes.filter(function (d) { return d.Code == value; })[0];
            if (translationObject) {
                if (SessionLocator_1.SessionLocator.LoggedUserPM.DontShowLocal) {
                    translation = translationObject.DefaultText;
                }
                else {
                    if (translationObject.LocalDefaultText) {
                        translation = translationObject.LocalDefaultText;
                    }
                    else {
                        translation = translationObject.DefaultText;
                    }
                }
                var codeTranslation = window.TenantTranslations.filter(function (d) { return d.TextCodeId === translationObject.Id && d.TranslationHeaderCode == SessionLocator_1.SessionLocator.TenantPM.Language; })[0];
                if (codeTranslation) {
                    translation = codeTranslation.TranslatedText;
                }
                else {
                    var languageTranslation = window.TenantLanguageTranslations.filter(function (d) { return d.TextCodeId === translationObject.Id; })[0];
                    if (languageTranslation) {
                        translation = languageTranslation.TranslatedText;
                    }
                }
                window.TextCodesCache.push(translationObject);
            }
        }
        if (window.TextCodesCache.length > 200) {
            window.TextCodesCache.splice(0, 50);
        }
        if (Fix == true) {
            return TextCodeTranslator.FixTranslation(translation);
        }
        else {
            return translation;
        }
    };
    TextCodeTranslator.TranslateTable = function (value) {
        if (SessionLocator_1.SessionLocator.UseCachedData) {
            return this.TranslateCached(value);
        }
        var translation = "";
        var cachedTranslationObject = window.TranslationsCache.filter(function (d) { return d.Code === value; })[0];
        if (cachedTranslationObject) {
            translation = cachedTranslationObject.TranslatedText;
        }
        else {
            var translationObject = window.TextCodesTranslations.filter(function (d) { return d.Code == value; })[0];
            if (translationObject) {
                translation = translationObject.TranslatedText;
                window.TranslationsCache.push(translationObject);
            }
        }
        if (window.TranslationsCache.length > 200) {
            window.TranslationsCache.splice(0, 50);
        }
        return TextCodeTranslator.FixTranslation(translation);
    };
    TextCodeTranslator.TranslateTablePlural = function (value) {
        if (SessionLocator_1.SessionLocator.UseCachedData) {
            return this.TranslatePluralCached(value);
        }
        var translation = "";
        var cachedTranslationObject = window.TranslationsCache.filter(function (d) { return d.Code === value; })[0];
        if (cachedTranslationObject) {
            translation = cachedTranslationObject.TranslatedTextPlural;
        }
        else {
            var translationObject = window.TextCodesTranslations.filter(function (d) { return d.Code == value; })[0];
            if (translationObject) {
                translation = translationObject.TranslatedTextPlural;
                window.TranslationsCache.push(translationObject);
            }
        }
        if (window.TranslationsCache.length > 200) {
            window.TranslationsCache.splice(0, 50);
        }
        return TextCodeTranslator.FixTranslation(translation);
    };
    TextCodeTranslator.TranslatePluralCached = function (value) {
        //console.log('88888888888888:', value);
        var translation = "";
        var cachedTranslationObject = window.TextCodesCache.filter(function (d) { return d.Code === value; })[0];
        if (cachedTranslationObject) {
            //translation = cachedTranslationObject.DefaultText;
            translation = cachedTranslationObject.DefaultTextPlural;
            var codeTranslation = window.TenantTranslations.filter(function (d) { return d.TextCodeId === cachedTranslationObject.Id && d.TranslationHeaderCode == SessionLocator_1.SessionLocator.TenantPM.Language; })[0];
            if (codeTranslation) {
                translation = codeTranslation.TranslatedTextPlural;
            }
            else {
                var languageTranslation = window.TenantLanguageTranslations.filter(function (d) { return d.TextCodeId === cachedTranslationObject.Id; })[0];
                if (languageTranslation) {
                    translation = languageTranslation.TranslatedText;
                }
            }
        }
        else {
            var translationObject = window.TextCodes.filter(function (d) { return d.Code == value; })[0];
            if (translationObject) {
                translation = translationObject.DefaultTextPlural;
                var codeTranslation = window.TenantTranslations.filter(function (d) { return d.TextCodeId === translationObject.Id && d.TranslationHeaderCode == SessionLocator_1.SessionLocator.TenantPM.Language; })[0];
                if (codeTranslation) {
                    translation = codeTranslation.TranslatedTextPlural;
                }
                else {
                    var languageTranslation = window.TenantLanguageTranslations.filter(function (d) { return d.TextCodeId === translationObject.Id; })[0];
                    if (languageTranslation) {
                        translation = languageTranslation.TranslatedText;
                    }
                }
                window.TextCodesCache.push(translationObject);
            }
        }
        if (window.TextCodesCache.length > 200) {
            window.TextCodesCache.splice(0, 50);
        }
        return TextCodeTranslator.FixTranslation(translation);
    };
    TextCodeTranslator.GetRequiredFieldForTableMessageTranslation = function (requiredTextCodeCode, fieldNameTextCode, tableNameTextCode, entityReference) {
        var message = TextCodeTranslator.Translate("Customs.General.O.FieldForTableIsRequired");
        var fieldName = TextCodeTranslator.Translate(fieldNameTextCode);
        var tableName = TextCodeTranslator.Translate(tableNameTextCode);
        message = message.replace('%FieldName', fieldName);
        message = message.replace('%TableName', tableName);
        message = message.replace('%EntityReference', entityReference ? entityReference : "");
        return message;
    };
    TextCodeTranslator.FixTranslation = function (value) {
        var myResult = "";
        if (value) {
            myResult = Tools_1.AppTool.Replace(value, "%n", "\n");
        }
        //return "!" + myResult;
        return myResult;
    };
    return TextCodeTranslator;
}());
exports.TextCodeTranslator = TextCodeTranslator;
//# sourceMappingURL=TextCodeTranslator.js.map