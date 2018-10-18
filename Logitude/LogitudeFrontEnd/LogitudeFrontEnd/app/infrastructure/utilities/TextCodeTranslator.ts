export class TextCodeTranslator {

    static transform(value: string): string {
        var translation: string = "";

        var translationObject = window.TextCodesTranslations.filter(d=> d.Code == value)[0];
        if (translationObject) {
            translation = translationObject.TranslatedText;
        }

        return "!" + translation;
    }

    static Translate(value: string): string {
        var translation: string = "";

        var translationObject = window.TextCodesTranslations.filter(d=> d.Code == value)[0];
        if (translationObject) {
            translation = translationObject.TranslatedText;
        }

        return "!" + translation;
    }

}