import {UIProperties, UIProperty} from '../../Infrastructure/Components/LogitudeComponents/UIProperties';
import {ServiceHelper} from '../../Infrastructure/Utilities/ServiceHelper';

export class ARPaymentInvoicePM {
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

  private aRPaymentId: string;
  public get ARPaymentId() { return this.aRPaymentId; }
  public set ARPaymentId(newValue: string) { this.aRPaymentId = newValue; this.MarkAsDirty(); }

  private aRInvoiceId: string;
  public get ARInvoiceId() { return this.aRInvoiceId; }
  public set ARInvoiceId(newValue: string) { this.aRInvoiceId = newValue; this.MarkAsDirty(); }

  private localAmount: number;
  public get LocalAmount() { return this.localAmount; }
  public set LocalAmount(newValue: number) { this.localAmount = newValue; this.MarkAsDirty(); }

  private foreignAmount: number;
  public get ForeignAmount() { return this.foreignAmount; }
  public set ForeignAmount(newValue: number) { this.foreignAmount = newValue; this.MarkAsDirty(); }

  private foreignCurrencyId: string;
  public get ForeignCurrencyId() { return this.foreignCurrencyId; }
  public set ForeignCurrencyId(newValue: string) { this.foreignCurrencyId = newValue; this.MarkAsDirty(); }

  private aRInvoiceNumber: string;
  public get ARInvoiceNumber() { return this.aRInvoiceNumber; }
  public set ARInvoiceNumber(newValue: string) { this.aRInvoiceNumber = newValue; this.MarkAsDirty(); }

  private exchangeRate: number;
  public get ExchangeRate() { return this.exchangeRate; }
  public set ExchangeRate(newValue: number) { if (this.exchangeRate != newValue) { this.exchangeRate = newValue; } }

  private paymentAmount: number;
  public get PaymentAmount() { return this.paymentAmount; }
  public set PaymentAmount(newValue: number) { if (this.paymentAmount != newValue) { this.paymentAmount = newValue; } }

  private aRInvoiceMetodoPagoCode: string;
  public get ARInvoiceMetodoPagoCode() { return this.aRInvoiceMetodoPagoCode; }
  public set ARInvoiceMetodoPagoCode(newValue: string) { this.aRInvoiceMetodoPagoCode = newValue; this.MarkAsDirty(); }

    private aRInvoiceTransferStatusCode: string;
    public get ARInvoiceTransferStatusCode() { return this.aRInvoiceTransferStatusCode; }
    public set ARInvoiceTransferStatusCode(newValue: string) { this.aRInvoiceTransferStatusCode = newValue; this.MarkAsDirty(); }
    
  private changeSetOp: string;
  public get ChangeSetOp() { return this.changeSetOp; }
  public set ChangeSetOp(newValue: string) { this.changeSetOp = newValue; this.MarkAsDirty(); }

  public UniqueKey: string;
  public OldEntityPM: ARPaymentInvoicePM;

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

  private MyClone: ARPaymentInvoicePM;

  public CloneMe() {
    ServiceHelper.CloneEntityPM(this);
  }

  public RejectChanges() {
    ServiceHelper.RejectEntityPMChanges(this);
  }
}
