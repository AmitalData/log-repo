import {Pipe} from '@angular/core';
import {TextCodeTranslator} from '../../Infrastructure/Utilities/TextCodeTranslator';

@Pipe({ name: 'TextCodeTranslationPipe' })

export class TextCodeTranslationPipe {
    transform(value: string): string {
        var translation: string = "";
        translation = TextCodeTranslator.Translate(value);
        if (translation != null) {
            translation = translation.trim();
        }
        if (translation == "") {
            alert("This code:'" + value + "' Not Found!");
        }
        return translation;
    }
}
