import {Pipe} from '@angular/core';
import {TextCodeTranslator} from '../../Infrastructure/Utilities/TextCodeTranslator';
import { isNullOrUndefined, isUndefined } from 'util';
import { SessionLocator } from '../../Infrastructure/Utilities/SessionLocator';
import { ObjectsLocator } from '../../Infrastructure/Locators/ObjectsLocator';

@Pipe({ name: 'TextCodeTranslationPipe' })

export class TextCodeTranslationPipe {
    transform(value: string): string {
        var translation: string = "";
        translation = TextCodeTranslator.Translate(value);
        if (translation != null) {
            translation = translation.trim();
        }
        if (translation == "") {
            if (this.ShowAlertMessage(value)) {
                alert("This code:'" + value + "' Not Found!");
            }
        }
        return translation;
    }

    ShowAlertMessage(value) {
        if (isNullOrUndefined(value))
            return false;

        var productionStages: Array<string> = ["simplog", "logboxwe1", "amitalstorage"];
        if (!productionStages.find(stage => stage == ObjectsLocator.GlobalSetting.DeploymentStage.toLowerCase())) {
            if (!SessionLocator.ProtractorEmails.find(userEmail => userEmail == SessionLocator.LoggedUserPM.Email.toLowerCase()))
                return true;
        }
        return false;
    }
}
