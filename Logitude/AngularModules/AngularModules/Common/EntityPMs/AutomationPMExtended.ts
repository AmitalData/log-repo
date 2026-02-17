
import {UIProperties, UIProperty} from '../../Infrastructure/Components/LogitudeComponents/UIProperties';
import {ServiceHelper} from '../../Infrastructure/Utilities/ServiceHelper';
import {ServiceLocator} from '../../Infrastructure/Locators/ServiceLocator';

export class AutomationPM {

    public UIProperties: UIProperties;
    constructor() {
        this.UIProperties = new UIProperties(this);
        this.IsDirty = false;
    }


    private id: string;
    public get Id() { return this.id; }
    public set Id(newValue: string) { this.id = newValue; this.MarkAsDirty("Id"); }


    private tenant: number;
    public get Tenant() { return this.tenant; }
    public set Tenant(newValue: number) { this.tenant = newValue; this.MarkAsDirty("Tenant"); }


    private name: string;
    public get Name() { return this.name; }
    public set Name(newValue: string) { this.name = newValue; this.MarkAsDirty("Name"); }


    private objectTableId: string;
    public get ObjectTableId() { return this.objectTableId; }
    public set ObjectTableId(newValue: string) { this.objectTableId = newValue; this.MarkAsDirty("ObjectTableId"); }


    private type: string;
    public get Type() { return this.type; }
    public set Type(newValue: string) { this.type = newValue; this.MarkAsDirty("Type"); }


    private resultCode: string;
    public get ResultCode() { return this.resultCode; }
    public set ResultCode(newValue: string) { this.resultCode = newValue; this.MarkAsDirty("ResultCode"); }


    private description: string;
    public get Description() { return this.description; }
    public set Description(newValue: string) { this.description = newValue; this.MarkAsDirty("Description"); }


    private inactive: boolean;
    public get Inactive() { return this.inactive; }
    public set Inactive(newValue: boolean) { this.inactive = newValue; this.MarkAsDirty("Inactive"); }


    private createDate: Date;
    public get CreateDate() { return this.createDate; }
    public set CreateDate(newValue: Date) { this.createDate = newValue; this.MarkAsDirty("CreateDate"); }


    private updateDate: Date;
    public get UpdateDate() { return this.updateDate; }
    public set UpdateDate(newValue: Date) { this.updateDate = newValue; this.MarkAsDirty("UpdateDate"); }


    private createdByUserId: string;
    public get CreatedByUserId() { return this.createdByUserId; }
    public set CreatedByUserId(newValue: string) { this.createdByUserId = newValue; this.MarkAsDirty("CreatedByUserId"); }


    private updatedByUserId: string;
    public get UpdatedByUserId() { return this.updatedByUserId; }
    public set UpdatedByUserId(newValue: string) { this.updatedByUserId = newValue; this.MarkAsDirty("UpdatedByUserId"); }


    private documentTypeId: string;
    public get DocumentTypeId() { return this.documentTypeId; }
    public set DocumentTypeId(newValue: string) { this.documentTypeId = newValue; this.MarkAsDirty("DocumentTypeId"); }


    private templateId: string;
    public get TemplateId() { return this.templateId; }
    public set TemplateId(newValue: string) { this.templateId = newValue; this.MarkAsDirty("TemplateId"); }


    private createdByUserName: string;
    public get CreatedByUserName() { return this.createdByUserName; }
    public set CreatedByUserName(newValue: string) { this.createdByUserName = newValue; this.MarkAsDirty("CreatedByUserName"); }


    private updatedByUserName: string;
    public get UpdatedByUserName() { return this.updatedByUserName; }
    public set UpdatedByUserName(newValue: string) { this.updatedByUserName = newValue; this.MarkAsDirty("UpdatedByUserName"); }


    private from: string;
    public get From() { return this.from; }
    public set From(newValue: string) { this.from = newValue; this.MarkAsDirty("From"); }


    private fromEmail: string;
    public get FromEmail() { return this.fromEmail; }
    public set FromEmail(newValue: string) { this.fromEmail = newValue; this.MarkAsDirty("FromEmail"); }


    private automationXML: string;
    public get AutomationXML() { return this.automationXML; }
    public set AutomationXML(newValue: string) { this.automationXML = newValue; this.MarkAsDirty("AutomationXML"); }


    private version: number;
    public get Version() { return this.version; }
    public set Version(newValue: number) { this.version = newValue; this.MarkAsDirty("Version"); }


    private order: number;
    public get Order() { return this.order; }
    public set Order(newValue: number) { this.order = newValue; this.MarkAsDirty("Order"); }



    private automatedDataBackup: any;
    public get AutomatedDataBackup() { return this.automatedDataBackup; }
    public set AutomatedDataBackup(newValue: any) { this.automatedDataBackup = newValue; this.MarkAsDirty("AutomatedDataBackup"); }


    private isChangeAutomationXaml: boolean;
    public get IsChangeAutomationXaml() { return this.isChangeAutomationXaml; }
    public set IsChangeAutomationXaml(newValue: boolean) { this.isChangeAutomationXaml = newValue; this.MarkAsDirty("IsChangeAutomationXaml"); }


    private automationResultEmailRecipientLists: any;
    public get AutomationResultEmailRecipientLists() { return this.automationResultEmailRecipientLists; }
    public set AutomationResultEmailRecipientLists(newValue: any) { this.automationResultEmailRecipientLists = newValue; this.MarkAsDirty("AutomationResultEmailRecipientLists"); }


    private code: string;
    public get Code() { return this.code; }
    public set Code(newValue: string) { if (this.code != newValue) { this.code = newValue; this.MarkAsDirty("Code"); } }


    public OldEntityPM: AutomationPM;

    public IsDirty: boolean;
    MarkAsDirty(propertyName: string = null) {
        this.IsDirty = true;

        if (propertyName != null) {
            ServiceLocator.RulesValidator.ApplyEntityChangedRules(propertyName, this, "Automation");

        }
    }
    private MyClone: AutomationPM;

    public CloneMe() {
        ServiceHelper.CloneEntityPM(this);
    }

    public RejectChanges() {
        ServiceHelper.RejectEntityPMChanges(this);
    }

}