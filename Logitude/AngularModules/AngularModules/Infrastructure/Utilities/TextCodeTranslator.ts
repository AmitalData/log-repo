declare var window: any;
import {SessionLocator} from '../Utilities/SessionLocator';
import { AppTool } from '../Tools';
import { retry } from 'rxjs/operator/retry';

export class TextCodeTranslator {

    static Translate(value: string, Fix: boolean = true): string {
        if (SessionLocator.UseCachedData) {

            return this.TranslateCached(value, Fix);
        }
        //console.log('88888888888888:', value);

        var translation: string = "";
        var cachedTranslationObject = window.TranslationsCache.filter((d: any) => d.Code === value)[0];

        if (cachedTranslationObject) {
            translation = cachedTranslationObject.TranslatedText;
        }

        else {
            var translationObject = window.TextCodesTranslations.filter((d: any) => d.Code == value)[0];
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
    }
    static TranslateCached(value: string, Fix: boolean = true): string {

        //console.log('88888888888888:', value);

        var translation: string = "";
        var cachedTranslationObject = window.TextCodesCache.filter((d: any) => d.Code === value)[0];

        if (cachedTranslationObject) {
            if (SessionLocator.LoggedUserPM.DontShowLocal) {
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

            var codeTranslation = window.TenantTranslations.filter((d: any) => d.TextCodeId === cachedTranslationObject.Id && d.TranslationHeaderCode == SessionLocator.TenantPM.Language)[0];
            if (codeTranslation) {
                translation = codeTranslation.TranslatedText;
            }
            else {
                var languageTranslation = window.TenantLanguageTranslations.filter((d: any) => d.TextCodeId === cachedTranslationObject.Id)[0];
                if (languageTranslation) {
                    translation = languageTranslation.TranslatedText;
                }
            }
        }

        else {
            var translationObject = window.TextCodes.filter(d => d.Code == value)[0];
            if (translationObject) {
                if (SessionLocator.LoggedUserPM.DontShowLocal) {
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

                var codeTranslation = window.TenantTranslations.filter((d: any) => d.TextCodeId === translationObject.Id && d.TranslationHeaderCode == SessionLocator.TenantPM.Language)[0];
                if (codeTranslation) {
                    translation = codeTranslation.TranslatedText;
                }
                else {
                    var languageTranslation = window.TenantLanguageTranslations.filter((d: any) => d.TextCodeId === translationObject.Id)[0];
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
    }
    static TranslateTable(value: string): string {

        if (SessionLocator.UseCachedData) {
            return this.TranslateCached(value);
        }

        var translation: string = "";
        var cachedTranslationObject = window.TranslationsCache.filter((d: any) => d.Code === value)[0];

        if (cachedTranslationObject) {
            translation = cachedTranslationObject.TranslatedText;
        }

        else {
            var translationObject = window.TextCodesTranslations.filter((d: any) => d.Code == value)[0];
            if (translationObject) {
                translation = translationObject.TranslatedText;
                window.TranslationsCache.push(translationObject);
            }
        }

        if (window.TranslationsCache.length > 200) {
            window.TranslationsCache.splice(0, 50);
        }

        return TextCodeTranslator.FixTranslation(translation);
    }
    static TranslateTablePlural(value: string): string {

        if (SessionLocator.UseCachedData) {
            return this.TranslatePluralCached(value);
        }

        var translation: string = "";
        var cachedTranslationObject = window.TranslationsCache.filter((d: any) => d.Code === value)[0];

        if (cachedTranslationObject) {
            translation = cachedTranslationObject.TranslatedTextPlural;
        }

        else {
            var translationObject = window.TextCodesTranslations.filter((d: any) => d.Code == value)[0];
            if (translationObject) {
                translation = translationObject.TranslatedTextPlural;
                window.TranslationsCache.push(translationObject);
            }
        }

        if (window.TranslationsCache.length > 200) {
            window.TranslationsCache.splice(0, 50);
        }

        return TextCodeTranslator.FixTranslation(translation);
    }
    static TranslatePluralCached(value: string): string {

        //console.log('88888888888888:', value);

        var translation: string = "";
        var cachedTranslationObject = window.TextCodesCache.filter((d: any) => d.Code === value)[0];

        if (cachedTranslationObject) {
            //translation = cachedTranslationObject.DefaultText;
            translation = cachedTranslationObject.DefaultTextPlural;
            var codeTranslation = window.TenantTranslations.filter((d: any) => d.TextCodeId === cachedTranslationObject.Id && d.TranslationHeaderCode == SessionLocator.TenantPM.Language)[0];
            if (codeTranslation) {
                translation = codeTranslation.TranslatedTextPlural;
            }
            else {
                var languageTranslation = window.TenantLanguageTranslations.filter((d: any) => d.TextCodeId === cachedTranslationObject.Id)[0];
                if (languageTranslation) {
                    translation = languageTranslation.TranslatedText;
                }
            }
        }

        else {
            var translationObject = window.TextCodes.filter(d => d.Code == value)[0];
            if (translationObject) {
                translation = translationObject.DefaultTextPlural;

                var codeTranslation = window.TenantTranslations.filter((d: any) => d.TextCodeId === translationObject.Id && d.TranslationHeaderCode == SessionLocator.TenantPM.Language)[0];
                if (codeTranslation) {
                    translation = codeTranslation.TranslatedTextPlural;
                }
                else {
                    var languageTranslation = window.TenantLanguageTranslations.filter((d: any) => d.TextCodeId === translationObject.Id)[0];
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
    }
    static GetRequiredFieldForTableMessageTranslation(requiredTextCodeCode: string, fieldNameTextCode: string, tableNameTextCode: string, entityReference: string) {
        var message = TextCodeTranslator.Translate("Customs.General.O.FieldForTableIsRequired");
        var fieldName = TextCodeTranslator.Translate(fieldNameTextCode);
        var tableName = TextCodeTranslator.Translate(tableNameTextCode);

        message = message.replace('%FieldName', fieldName);
        message = message.replace('%TableName', tableName);
        message = message.replace('%EntityReference', entityReference ? entityReference : "");

        return message;
    }

    static FixTranslation(value: string) {
        var myResult: string = "";

        if (value) {
            myResult = AppTool.Replace(value, "%n", "\n");
        }

        return "!" + myResult;

        //return myResult;
    }
}
