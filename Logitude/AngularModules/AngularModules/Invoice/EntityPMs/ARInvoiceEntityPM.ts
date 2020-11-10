import {UIProperties, UIProperty} from '../../Infrastructure/Components/LogitudeComponents/UIProperties';
import {ServiceHelper} from '../../Infrastructure/Utilities/ServiceHelper';

export class ARInvoiceEntityPM {
    public UIProperties: UIProperties;
    constructor() {
        this.UIProperties = new UIProperties;
        this.IsDirty = false;
    }


    private id: string;
    public get Id() { return this.id; }
    public set Id(newValue: string) { this.id = newValue; this.MarkAsDirty(); }

    private tenant: number;
    public get Tenant() { return this.tenant; }
    public set Tenant(newValue: number) { this.tenant = newValue; this.MarkAsDirty(); }

    private aRInvoiceId: string;
    public get ARInvoiceId() { return this.aRInvoiceId; }
    public set ARInvoiceId(newValue: string) { this.aRInvoiceId = newValue; this.MarkAsDirty(); }

    private entityId: string;
    public get EntityId() { return this.entityId; }
    public set EntityId(newValue: string) { this.entityId = newValue; this.MarkAsDirty(); }

    private objectTableId: string;
    public get ObjectTableId() { return this.objectTableId; }
    public set ObjectTableId(newValue: string) { this.objectTableId = newValue; this.MarkAsDirty(); }

    private entityReference: string;
    public get EntityReference() { return this.entityReference; }
    public set EntityReference(newValue: string) { this.entityReference = newValue; this.MarkAsDirty(); }

    private changeSetOp: string;
    public get ChangeSetOp() { return this.changeSetOp; }
    public set ChangeSetOp(newValue: string) { this.changeSetOp = newValue; this.MarkAsDirty(); }


    public OldEntityPM: ARInvoiceEntityPM;

    private entityParentPM: any;
    public get EntityParentPM() { return this.entityParentPM; }
    public set EntityParentPM(newValue: any) { this.entityParentPM = newValue; }

    public UniqueKey: string;

    public IsDirty: boolean;
    public DisableMarkAsDirty: boolean = false;
    MarkAsDirty() {
        if (!this.DisableMarkAsDirty) {
            this.IsDirty = true;
            if (this.EntityParentPM) {
                this.EntityParentPM.MarkAsDirty();
            }
        }
    }
    private MyClone: ARInvoiceEntityPM;

    public CloneMe() {
        ServiceHelper.CloneEntityPM(this);
    }

    public RejectChanges() {
        ServiceHelper.RejectEntityPMChanges(this);
    }

}
