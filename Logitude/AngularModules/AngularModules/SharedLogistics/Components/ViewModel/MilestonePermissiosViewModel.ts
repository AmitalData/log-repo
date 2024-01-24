import {Guid} from '../../../Infrastructure/Utilities/Guid';
import { CargoTenantMilestoneDefinitionPM } from '../../../Common/EntityPMs/CargoTenantMilestoneDefinitionPM';

export class MilestonePermissiosViewModel {
    public CustomerChooseCheckedBoxId: string;
    public entityPM: any;

    public get MilestoneName() { return this.entityPM.EnglishName }
    public get ImportWeight() { return this.entityPM.Weight }
    public get ExportWeight() { return this.entityPM.ExportWeight }


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

    constructor(currentPM: any) {
        this.entityPM = currentPM;
        this.CustomerChooseCheckedBoxId = Guid.newGuid();
        this.CustomerChooseIsChecked = currentPM.CustomerChooseIsChecked;
    }
}
