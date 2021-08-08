import {Guid} from '../../../Infrastructure/Utilities/Guid';
import {EventTypePM} from '../../../Infrastructure/EntityPMs/EventTypePM';

export class MilestonePermissiosViewModel {
    public CustomerChooseCheckedBoxId: string;
    public entityPM: EventTypePM;

    public get MilestoneName() { return this.entityPM.EnglishName }

    public get CustomerChooseIsChecked() {
        if (this.entityPM) {
            return this.entityPM.IsCustomerView;
        }
        else return false;
    }
    public set CustomerChooseIsChecked(value: boolean) {
        if (this.entityPM != null) {
            this.entityPM.IsCustomerView = value;
        }
    }

    constructor(currentEvent: EventTypePM) {
        this.entityPM = currentEvent;
        this.CustomerChooseCheckedBoxId = Guid.newGuid();
    }
}
