import {Pipe} from '@angular/core';
import {TextCodeTranslator} from '../../Infrastructure/Utilities/TextCodeTranslator';
import { isNullOrUndefined, isUndefined } from 'util';
import { SessionLocator } from '../../Infrastructure/Utilities/SessionLocator';
import { ObjectsLocator } from '../../Infrastructure/Locators/ObjectsLocator';
declare var window;

@Pipe({ name: 'ObjectFieldTextCodeTranslationPipe' })

export class ObjectFieldTextCodeTranslationPipe {
    transform(value: string): string {
        let translation: string = "";
        let textCodeValue: string = "";
        let objectField = window.ObjectFields.filter(d => d.FieldCode == value);
        if (objectField && objectField[0]) {
            textCodeValue = objectField[0].FullNameTextCodeCode;
        }
        translation = TextCodeTranslator.Translate(textCodeValue);
        if (translation != null) {
            translation = translation.trim();
        }
        if (translation == "" && this.ShowAlertMessage(value) && SessionLocator.LoggedUserPM.Email.includes("logitudeworld.com")) {
            alert("This code:'" + value + "' Not Found!");
        }

        return translation;
    }

    ShowAlertMessage(value) {
        if (isNullOrUndefined(value))
            return false;

        let productionStages: Array<string> = ["simplog", "logboxwe1", "amitalstorage"];

        if (productionStages.find(stage => stage == ObjectsLocator.GlobalSetting.DeploymentStage.toLowerCase())) return false;
        if (!SessionLocator.ProtractorEmails.find(userEmail => userEmail == SessionLocator.LoggedUserPM.Email.toLowerCase())) return true;
        
        return false;
    }
}
