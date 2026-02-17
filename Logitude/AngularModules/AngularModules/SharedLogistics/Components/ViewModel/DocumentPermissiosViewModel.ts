
declare var System: any;
declare var window: any;
import {Guid} from '../../../Infrastructure/Utilities/Guid';
import {DocumentTypePM} from '../../../Common/EntityPMs/DocumentTypePM';

export class DocumentPermissiosViewModel {

    public CustomerSuggestedCheckedBoxId: string;
    public CustomerChooseCheckedBoxId: string;

    public entityPM: DocumentTypePM;
    private entityPM_TenantZero: DocumentTypePM;

    public get DocumentTypeName() { return this.entityPM_TenantZero ? this.entityPM_TenantZero.Name : this.entityPM.Name }
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


    constructor(zeroDocumentType: DocumentTypePM, currentDocument: DocumentTypePM) {
        this.entityPM_TenantZero = zeroDocumentType;
        this.entityPM = currentDocument;

        this.CustomerChooseCheckedBoxId = Guid.newGuid();
        this.CustomerSuggestedCheckedBoxId = Guid.newGuid();

    }


















}