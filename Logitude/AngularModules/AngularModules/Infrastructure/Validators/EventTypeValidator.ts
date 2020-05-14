import {AppTool} from '../../Infrastructure/Tools';
import {EventTypePM} from '../EntityPMs/EventTypePM';
import {ObjectFieldPM} from '../EntityPMs/ObjectFieldPM';

declare var window: any;

export class EventTypeValidator {
    public Validate(entityPM: EventTypePM) {
        var error: any = [];

        if (entityPM.CustomField) {
            var objectField: ObjectFieldPM = window.ObjectFields.filter(x => x.FieldCode === entityPM.CustomField)[0];

            if (objectField) {
                if (!objectField.DisplayOnly) {
                    error.push("Only read only date custom fields can be connected to your event");
                }
            }

        }

     
        return error;
    }
}