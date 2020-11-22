

import {UIProperties, UIProperty} from '../../Infrastructure/Components/LogitudeComponents/UIProperties';
export class EntityChangePM {


    public UIProperties: UIProperties;

    constructor() {
        this.UIProperties = new UIProperties;
        this.IsDirty = false;
    }

    public IsDirty: boolean;


    private id: string;
    public get Id() { return this.id; }
    public set Id(newValue: string) { this.id = newValue; this.MarkAsDirty(); }

    private tenant: number;
    public get Tenant() { return this.tenant; }
    public set Tenant(newValue: number) { this.tenant = newValue; this.MarkAsDirty(); }

    private objectTableId: string;
    public get ObjectTableId() { return this.objectTableId; }
    public set ObjectTableId(newValue: string) { this.objectTableId = newValue; this.MarkAsDirty(); }


    private entityId: string;
    public get EntityId() { return this.entityId; }
    public set EntityId(newValue: string) { this.entityId = newValue; this.MarkAsDirty(); }

    private createDate: Date;
    public get CreateDate() { return this.createDate; }
    public set CreateDate(newValue: Date) { this.createDate = newValue; this.MarkAsDirty(); }


    private createByUserId: string;
    public get CreateByUserId() { return this.createByUserId; }
    public set CreateByUserId(newValue: string) { this.createByUserId = newValue; this.MarkAsDirty(); }

    private createByUserName: string;
    public get CreateByUserName() { return this.createByUserName; }
    public set CreateByUserName(newValue: string) { this.createByUserName = newValue; this.MarkAsDirty(); }

    private automationConditionFieldsXml: boolean;
    public get AutomationConditionFieldsXml() { return this.automationConditionFieldsXml; }
    public set AutomationConditionFieldsXml(newValue: boolean) { this.automationConditionFieldsXml = newValue; this.MarkAsDirty(); }



    private changesFieldsXml: string;
    public get ChangesFieldsXml() { return this.changesFieldsXml; }
    public set ChangesFieldsXml(newValue: string) { this.changesFieldsXml = newValue; this.MarkAsDirty(); }



    private changesAutomationFieldsXml: string;
    public get ChangesAutomationFieldsXml() { return this.changesAutomationFieldsXml; }
    public set ChangesAutomationFieldsXml(newValue: string) { this.changesAutomationFieldsXml = newValue; this.MarkAsDirty(); }



    private setAutomationSsucceedXml: string;
    public get SetAutomationSsucceedXml() { return this.setAutomationSsucceedXml; }
    public set SetAutomationSsucceedXml(newValue: string) { this.setAutomationSsucceedXml = newValue; this.MarkAsDirty(); }


    private emailAutomationSsucceedXml: string;
    public get EmailAutomationSsucceedXml() { return this.emailAutomationSsucceedXml; }
    public set EmailAutomationSsucceedXml(newValue: string) { this.emailAutomationSsucceedXml = newValue; this.MarkAsDirty(); }


    private setAutomationFailedXml: string;
    public get SetAutomationFailedXml() { return this.setAutomationFailedXml; }
    public set SetAutomationFailedXml(newValue: string) { this.setAutomationFailedXml = newValue; this.MarkAsDirty(); }




    private emailAutomationFailedXml: string;
    public get EmailAutomationFailedXml() { return this.emailAutomationFailedXml; }
    public set EmailAutomationFailedXml(newValue: string) { this.emailAutomationFailedXml = newValue; this.MarkAsDirty(); }


    private hasExecutedRecord: boolean;
    public get HasExecutedRecord() { return this.hasExecutedRecord; }
    public set HasExecutedRecord(newValue: boolean) { this.hasExecutedRecord = newValue; this.MarkAsDirty(); }

    private checkStartDate: Date;
    public get CheckStartDate() { return this.checkStartDate; }
    public set CheckStartDate(newValue: Date) { this.checkStartDate = newValue; this.MarkAsDirty(); }

    private doneDate: Date;
    public get DoneDate() { return this.doneDate; }
    public set DoneDate(newValue: Date) { this.doneDate = newValue; this.MarkAsDirty(); }

    private executionTime: number;
    public get ExecutionTime() { return this.executionTime; }
    public set ExecutionTime(newValue: number) { this.executionTime = newValue; this.MarkAsDirty(); }

    private followUpAutomationFailedXml: string;
    public get FollowUpAutomationFailedXml() { return this.followUpAutomationFailedXml; }
    public set FollowUpAutomationFailedXml(newValue: string) { this.followUpAutomationFailedXml = newValue; this.MarkAsDirty(); }

    private setSLAAutomationFailedXml: string;
    public get SetSLAAutomationFailedXml() { return this.setSLAAutomationFailedXml; }
    public set SetSLAAutomationFailedXml(newValue: string) { this.setSLAAutomationFailedXml = newValue; this.MarkAsDirty(); }

    private followUpAutomationSsucceedXml: string;
    public get FollowUpAutomationSsucceedXml() { return this.followUpAutomationSsucceedXml; }
    public set FollowUpAutomationSsucceedXml(newValue: string) { this.followUpAutomationSsucceedXml = newValue; this.MarkAsDirty(); }


    private setSLAAutomationSsucceedXml: string;
    public get SetSLAAutomationSsucceedXml() { return this.setSLAAutomationSsucceedXml; }
    public set SetSLAAutomationSsucceedXml(newValue: string) { this.setSLAAutomationSsucceedXml = newValue; this.MarkAsDirty(); }

    private queuedTaskAutomationFailedXml: string;
    public get QueuedTaskAutomationFailedXml() { return this.queuedTaskAutomationFailedXml; }
    public set QueuedTaskAutomationFailedXml(newValue: string) { this.queuedTaskAutomationFailedXml = newValue; this.MarkAsDirty(); }


    private queuedTaskAutomationSsucceedXml: string;
    public get QueuedTaskAutomationSsucceedXml() { return this.queuedTaskAutomationSsucceedXml; }
    public set QueuedTaskAutomationSsucceedXml(newValue: string) { this.queuedTaskAutomationSsucceedXml = newValue; this.MarkAsDirty(); }

    private sendInterfaceAutomationFailedXml: string;
    public get SendInterfaceAutomationFailedXml() { return this.sendInterfaceAutomationFailedXml; }
    public set SendInterfaceAutomationFailedXml(newValue: string) { this.sendInterfaceAutomationFailedXml = newValue; this.MarkAsDirty(); }

    private sendInterfaceAutomationSsucceedXml: string;
    public get SendInterfaceAutomationSsucceedXml() { return this.sendInterfaceAutomationSsucceedXml; }
    public set SendInterfaceAutomationSsucceedXml(newValue: string) { this.sendInterfaceAutomationSsucceedXml = newValue; this.MarkAsDirty(); }


    MarkAsDirty() {
        this.IsDirty = true;
    }
}
