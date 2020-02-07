import {Pipe} from '@angular/core';
import {TextCodeTranslator} from '../../Infrastructure/Utilities/TextCodeTranslator';
import { isNullOrUndefined, isUndefined } from 'util';
import { SessionLocator } from '../../Infrastructure/Utilities/SessionLocator';

@Pipe({ name: 'TextCodeTranslationPipe' })

export class TextCodeTranslationPipe {
    transform(value: string): string {
        var translation: string = "";
        translation = TextCodeTranslator.Translate(value);
        if (translation != null) {
            translation = translation.trim();
        }
        if (translation == "" && !isNullOrUndefined(value)) {
            if (!SessionLocator.ProtractorEmails.find(userEmail => userEmail == SessionLocator.LoggedUserPM.Email.toLowerCase())) {
                alert("This code:'" + value + "' Not Found!");
            }
        }
        return translation;
    }
}
