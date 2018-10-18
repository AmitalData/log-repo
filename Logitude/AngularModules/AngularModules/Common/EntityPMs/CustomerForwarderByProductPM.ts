
import {UIProperties, UIProperty} from '../../Infrastructure/Components/LogitudeComponents/UIProperties';

export class CustomerForwarderByProductPM {
    public UIProperties: UIProperties;
    constructor(entityParentPM: any) {
        this.UIProperties = new UIProperties;
        this.IsDirty = false;
    }

    private entityParentPM: any;
    public get EntityParentPM() { return this.entityParentPM; }
    public set EntityParentPM(newValue: any) { this.entityParentPM = newValue; }

    private changeSetOp: string;
    public get ChangeSetOp() { return this.changeSetOp; }
    public set ChangeSetOp(newValue: string) { this.changeSetOp = newValue; this.MarkAsDirty(); }


    private productTypeCode: string;
    public get ProductTypeCode() { return this.productTypeCode; }
    public set ProductTypeCode(value: string) { this.productTypeCode = value; this.MarkAsDirty(); }

    private forwarderId: string;
    public get ForwarderId() { return this.forwarderId; }
    public set ForwarderId(value: string) { this.forwarderId = value; this.MarkAsDirty(); }

    private customerId: string;
    public get CustomerId() { return this.customerId; }
    public set CustomerId(value: string) { this.customerId = value; this.MarkAsDirty(); }


    private tenant: number;
    public get Tenant() { return this.tenant; }
    public set Tenant(value: number) { this.tenant = value; this.MarkAsDirty(); }

    private forwarderName: string;
    public get ForwarderName() { return this.forwarderName; }
    public set ForwarderName(value: string) { this.forwarderName = value; this.MarkAsDirty(); }





    public OldEntityPM: any;

    public UniqueKey: string;

    public IsDirty: boolean;
    MarkAsDirty() {
        this.IsDirty = true;
        if (this.EntityParentPM) {
            this.EntityParentPM.MarkAsDirty();
        }
    }
}