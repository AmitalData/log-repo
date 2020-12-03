import {UIProperties, UIProperty} from '../../Infrastructure/Components/LogitudeComponents/UIProperties';
import {ServiceHelper} from '../../Infrastructure/Utilities/ServiceHelper';

export class APTransferLinePM {
    public UIProperties: UIProperties;
    constructor(_entityParentPM: any) {
        this.EntityParentPM = _entityParentPM;
        this.UIProperties = new UIProperties;
        this.IsDirty = false;
    }

    private entityParentPM: any;
    public get EntityParentPM() { return this.entityParentPM; }
    public set EntityParentPM(newValue: any) { this.entityParentPM = newValue; }

    private id: string;
    public get Id() { return this.id; }
    public set Id(newValue: string) { this.id = newValue; this.MarkAsDirty(); }

    private aPInvoiceId: string;
    public get APInvoiceId() { return this.aPInvoiceId; }
    public set APInvoiceId(newValue: string) { this.aPInvoiceId = newValue; this.MarkAsDirty(); }

    private table: string;
    public get Table() { return this.table; }
    public set Table(newValue: string) { this.table = newValue; this.MarkAsDirty(); }

    private fieldName: string;
    public get FieldName() { return this.fieldName; }
    public set FieldName(newValue: string) { this.fieldName = newValue; this.MarkAsDirty(); }

    private fieldExternalTableId: string;
    public get FieldExternalTableId() { return this.fieldExternalTableId; }
    public set FieldExternalTableId(newValue: string) { this.fieldExternalTableId = newValue; this.MarkAsDirty(); }

    private fieldValue: string;
    public get FieldValue() { return this.fieldValue; }
    public set FieldValue(newValue: string) { this.fieldValue = newValue; this.MarkAsDirty(); }

    private description: string;
    public get Description() { return this.description; }
    public set Description(newValue: string) { this.description = newValue; this.MarkAsDirty(); }

    private descriptionValue: string;
    public get DescriptionValue() { return this.descriptionValue; }
    public set DescriptionValue(newValue: string) { this.descriptionValue = newValue; this.MarkAsDirty(); }

    private descriptionHelp: string;
    public get DescriptionHelp() { return this.descriptionHelp; }
    public set DescriptionHelp(newValue: string) { this.descriptionHelp = newValue; this.MarkAsDirty(); }

    private changeSetOp: string;
    public get ChangeSetOp() { return this.changeSetOp; }
    public set ChangeSetOp(newValue: string) { this.changeSetOp = newValue; this.MarkAsDirty(); }

    public UniqueKey: string;
    public OldEntityPM: APTransferLinePM;

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

    private MyClone: APTransferLinePM;

    public CloneMe() {
        ServiceHelper.CloneEntityPM(this);
    }

    public RejectChanges() {
        ServiceHelper.RejectEntityPMChanges(this);
    }
}
