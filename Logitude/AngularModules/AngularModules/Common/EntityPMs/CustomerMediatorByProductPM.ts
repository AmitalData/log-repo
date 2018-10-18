import {UIProperties, UIProperty} from '../../Infrastructure/Components/LogitudeComponents/UIProperties';
export class CustomerMediatorByProductPM {
    public UIProperties: UIProperties;
    constructor(entityParentPM: any) {
        this.EntityParentPM = entityParentPM;
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

    private mediatorId: string;
    public get MediatorId() { return this.mediatorId; }
    public set MediatorId(value: string) { this.mediatorId = value; this.MarkAsDirty(); }

    private customerId: string;
    public get CustomerId() { return this.customerId; }
    public set CustomerId(value: string) { this.customerId = value; this.MarkAsDirty(); }


    private tenant: number;
    public get Tenant() { return this.tenant; }
    public set Tenant(value: number) { this.tenant = value; this.MarkAsDirty(); }

    private mediatorName: string;
    public get MediatorName() { return this.mediatorName; }
    public set MediatorName(value: string) { this.mediatorName = value; this.MarkAsDirty(); }



    public OldEntityPM: any;

    public UniqueKey: string;

    public IsDirty: boolean;
    MarkAsDirty() {
        this.IsDirty = true;
        if (this.entityParentPM) {
            this.entityParentPM.MarkAsDirty();
        }
    }}