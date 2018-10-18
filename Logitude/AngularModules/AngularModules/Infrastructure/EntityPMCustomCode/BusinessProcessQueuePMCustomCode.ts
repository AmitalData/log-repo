import {BusinessProcessQueuePM} from '../EntityPMs/BusinessProcessQueuePM';
import {SessionLocator} from '../../Infrastructure/Utilities/SessionLocator';
import {AppTool} from '../../Infrastructure/Tools';

export class BusinessProcessQueuePMCustomCode {
    public static ApplyEntityChanged(propertyName: string, entityPM: BusinessProcessQueuePM) {
        entityPM.UIProperties.SetEnabled("BusinessRoleId", "BusinessProcessQueue", false);
        entityPM.UIProperties.SetEnabled("ObjectTableId", "BusinessProcessQueue", false);
    }
}