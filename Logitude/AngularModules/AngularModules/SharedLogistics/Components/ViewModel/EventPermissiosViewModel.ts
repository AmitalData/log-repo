
declare var System: any;
declare var window: any;
import {Guid} from '../../../Infrastructure/Utilities/Guid';
import {EventTypePM} from '../../../Infrastructure/EntityPMs/EventTypePM';



export class EventPermissiosViewModel {

    public CustomerSuggestedCheckedBoxId: string;
    public CustomerChooseCheckedBoxId: string;
  
    public entityPM: EventTypePM;
    private entityPM_TenantZero: EventTypePM;

    public get EventTypeName() { return this.entityPM_TenantZero ? this.entityPM_TenantZero.EnglishName : this.entityPM.EnglishName }
    public get CustomerSuggestedIsChecked() { return this.entityPM_TenantZero ? this.entityPM_TenantZero.IsCustomerView : false }
    public get AgentSuggestedIsChecked() { return this.entityPM_TenantZero ? this.entityPM_TenantZero.IsAgentView : false }


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

    public get AgentChooseIsChecked() {

        if (this.entityPM) {
            return this.entityPM.IsAgentView;
        }
        else return false;
    }
    public set AgentChooseIsChecked(value: boolean) {
        if (this.entityPM != null) {
            this.entityPM.IsAgentView = value;
        }

    }


    constructor(zeroEvent: EventTypePM, currentEvent: EventTypePM) {
        this.entityPM_TenantZero = zeroEvent;
        this.entityPM = currentEvent;


        this.CustomerChooseCheckedBoxId = Guid.newGuid();
        this.CustomerSuggestedCheckedBoxId = Guid.newGuid();

    }






   





  





}