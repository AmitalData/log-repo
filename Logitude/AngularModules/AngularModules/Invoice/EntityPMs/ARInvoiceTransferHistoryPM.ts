import {UIProperties, UIProperty} from '../../Infrastructure/Components/LogitudeComponents/UIProperties';
import {ServiceHelper} from '../../Infrastructure/Utilities/ServiceHelper';

export class ARInvoiceTransferHistoryPM {
    public UIProperties: UIProperties;
    constructor() {
        this.UIProperties = new UIProperties;
        this.IsDirty = false;
    }

    private id: string;
    public get Id() { return this.id; }
    public set Id(newValue: string) { this.id = newValue; this.MarkAsDirty(); }

    private aRInvoiceId: string;
    public get ARInvoiceId() { return this.aRInvoiceId; }
    public set ARInvoiceId(newValue: string) { this.aRInvoiceId = newValue; this.MarkAsDirty(); }

    private transferNumber: string;
    public get TransferNumber() { return this.transferNumber; }
    public set TransferNumber(newValue: string) { this.transferNumber = newValue; this.MarkAsDirty(); }

    private transferDate: Date;
    public get TransferDate() { return this.transferDate; }
    public set TransferDate(newValue: Date) { this.transferDate = newValue; this.MarkAsDirty(); }

    private fileName: string;
    public get FileName() { return this.fileName; }
    public set FileName(newValue: string) { this.fileName = newValue; this.MarkAsDirty(); }

    public OldEntityPM: ARInvoiceTransferHistoryPM;

    public IsDirty: boolean;
    public DisableMarkAsDirty: boolean = false;
    MarkAsDirty() {

        this.IsDirty = true;

    }
    private MyClone: ARInvoiceTransferHistoryPM;

    public CloneMe() {
        ServiceHelper.CloneEntityPM(this);
    }

    public RejectChanges() {
        ServiceHelper.RejectEntityPMChanges(this);
    }
}
