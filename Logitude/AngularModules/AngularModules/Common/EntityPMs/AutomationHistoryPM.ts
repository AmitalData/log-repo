
import {UIProperties, UIProperty} from '../../Infrastructure/Components/LogitudeComponents/UIProperties';
import {ServiceHelper} from '../../Infrastructure/Utilities/ServiceHelper';
import {ServiceLocator} from '../../Infrastructure/Locators/ServiceLocator';

export class AutomationHistoryPM {

    public UIProperties: UIProperties;
    constructor() {
        this.UIProperties = new UIProperties(this);
        this.IsDirty = false;
    }


    //private id: string;
    //public get Id() { return this.id; }
    //public set Id(newValue: string) { this.id = newValue; this.MarkAsDirty("Id"); }


    private automationsId: string;
    public get AutomationsId() { return this.automationsId; }
    public set AutomationsId(newValue: string) { this.automationsId = newValue; this.MarkAsDirty("AutomationsId"); }


    private tenant: number;
    public get Tenant() { return this.tenant; }
    public set Tenant(newValue: number) { this.tenant = newValue; this.MarkAsDirty("Tenant"); }


    private createDate: Date;
    public get CreateDate() { return this.createDate; }
    public set CreateDate(newValue: Date) { this.createDate = newValue; this.MarkAsDirty("CreateDate"); }


    private version: number;
    public get Version() { return this.version; }
    public set Version(newValue: number) { this.version = newValue; this.MarkAsDirty("Version"); }


    private automationXML: string;
    public get AutomationXML() { return this.automationXML; }
    public set AutomationXML(newValue: string) { this.automationXML = newValue; this.MarkAsDirty("AutomationXML"); }


    private automatedDataBackup: any;
    public get AutomatedDataBackup() { return this.automatedDataBackup; }
    public set AutomatedDataBackup(newValue: any) { this.automatedDataBackup = newValue; this.MarkAsDirty("AutomatedDataBackup"); }



    public OldEntityPM: AutomationHistoryPM;

    public IsDirty: boolean;
    MarkAsDirty(propertyName: string = null) {
        this.IsDirty = true;

        if (propertyName != null) {
            ServiceLocator.RulesValidator.ApplyEntityChangedRules(propertyName, this, "AutomationHistory");

        }
    }
   

}