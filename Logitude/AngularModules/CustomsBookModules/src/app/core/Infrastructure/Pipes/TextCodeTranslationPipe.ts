import {Pipe} from '@angular/core';
import {TextCodeTranslator} from '../Utilities/TextCodeTranslator';
import { SessionLocator } from '../Utilities/SessionLocator';

@Pipe({ name: 'TextCodeTranslationPipe', standalone: true })

export class TextCodeTranslationPipe {
    transform(value: string): string {
        var translation: string = "";
        translation = TextCodeTranslator.Translate(value);
        if (translation != null) {
            translation = translation.trim();
        }
        if (translation == "") {
            if (this.ShowAlertMessage(value)) {
                if (SessionLocator.LoggedUserPM.Email.includes("logitudeworld.com")) 
                {
                    alert("This code:'" + value + "' Not Found!");
                } 
         
            }
        }
        return translation;
    }

    ShowAlertMessage(value) {
   
        return false;
    }
}
