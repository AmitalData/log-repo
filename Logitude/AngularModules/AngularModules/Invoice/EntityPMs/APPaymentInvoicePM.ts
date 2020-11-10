import {UIProperties, UIProperty} from '../../Infrastructure/Components/LogitudeComponents/UIProperties';
import {ServiceHelper} from '../../Infrastructure/Utilities/ServiceHelper';

export class APPaymentInvoicePM {
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

    private tenant: number;
    public get Tenant() { return this.tenant; }
    public set Tenant(newValue: number) { this.tenant = newValue; this.MarkAsDirty(); }


    private aPPaymentId: string;
    public get APPaymentId() { return this.aPPaymentId; }
    public set APPaymentId(newValue: string) { this.aPPaymentId = newValue; this.MarkAsDirty(); }

    private aPInvoiceId: string;
    public get APInvoiceId() { return this.aPInvoiceId; }
    public set APInvoiceId(newValue: string) { this.aPInvoiceId = newValue; this.MarkAsDirty(); }

    private localAmount: number;
    public get LocalAmount() { return this.localAmount; }
    public set LocalAmount(newValue: number) { this.localAmount = newValue; this.MarkAsDirty(); }

    private foreignAmount: number;
    public get ForeignAmount() { return this.foreignAmount; }
    public set ForeignAmount(newValue: number) { this.foreignAmount = newValue; this.MarkAsDirty(); }

    private foreignCurrencyId: string;
    public get ForeignCurrencyId() { return this.foreignCurrencyId; }
    public set ForeignCurrencyId(newValue: string) { this.foreignCurrencyId = newValue; this.MarkAsDirty(); }

    private aPInvoiceNumber: string;
    public get APInvoiceNumber() { return this.aPInvoiceNumber; }
    public set APInvoiceNumber(newValue: string) { this.aPInvoiceNumber = newValue; this.MarkAsDirty(); }

    private exchangeRate: number;
    public get ExchangeRate() { return this.exchangeRate; }
    public set ExchangeRate(newValue: number) { if (this.exchangeRate != newValue) { this.exchangeRate = newValue; } }

    private paymentAmount: number;
    public get PaymentAmount() { return this.paymentAmount; }
    public set PaymentAmount(newValue: number) { if (this.paymentAmount != newValue) { this.paymentAmount = newValue; } }

    private changeSetOp: string;
    public get ChangeSetOp() { return this.changeSetOp; }
    public set ChangeSetOp(newValue: string) { this.changeSetOp = newValue; this.MarkAsDirty(); }


    private aPInvoiceTransferStatusCode: string;
    public get APInvoiceTransferStatusCode() { return this.aPInvoiceTransferStatusCode; }
    public set APInvoiceTransferStatusCode(newValue: string) { this.aPInvoiceTransferStatusCode = newValue; this.MarkAsDirty(); }

    

    public OldEntityPM: APPaymentInvoicePM;

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

    private MyClone: APPaymentInvoicePM;

    public CloneMe() {
        ServiceHelper.CloneEntityPM(this);
    }

    public RejectChanges() {
        ServiceHelper.RejectEntityPMChanges(this);
    }
}
