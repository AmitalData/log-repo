import {UIProperties, UIProperty} from '../../Infrastructure/Components/LogitudeComponents/UIProperties';
export class CustomerProductLocationActualDataPM {
    public UIProperties: UIProperties;
    constructor(_entityParentPM: any) {
        this.EntityParentPM = _entityParentPM;
        this.UIProperties = new UIProperties;
        this.IsDirty = false;
    }

    private entityParentPM: any;
    public get EntityParentPM() { return this.entityParentPM; }
    public set EntityParentPM(newValue: any) { this.entityParentPM = newValue; }

    private changeSetOp: string;
    public get ChangeSetOp() { return this.changeSetOp; }
    public set ChangeSetOp(newValue: string) { this.changeSetOp = newValue; this.MarkAsDirty(); }

    private countryName: string;
    public get CountryName() { return this.countryName; }
    public set CountryName(newValue: string) { this.countryName = newValue; this.MarkAsDirty(); }

    private countryCode: string;
    public get CountryCode() { return this.countryCode; }
    public set CountryCode(newValue: string) { this.countryCode = newValue; this.MarkAsDirty(); }

    
    private month: number;
    public get Month() { return this.month; }
    public set Month(newValue: number) { this.month = newValue; this.MarkAsDirty(); }


    private countryId: number;
    public get CountryId() { return this.countryId; }
    public set CountryId(newValue: number) { this.countryId = newValue; this.MarkAsDirty(); }

    

    private chargeableWeight: number;
    public get ChargeableWeight() { return this.chargeableWeight; }
    public set ChargeableWeight(newValue: number) { this.chargeableWeight = newValue; this.MarkAsDirty(); }


    private revenue: number;
    public get Revenue() { return this.revenue; }
    public set Revenue(newValue: number) { this.revenue = newValue; this.MarkAsDirty(); }



    private numberOfShipments: number;
    public get NumberOfShipments() { return this.numberOfShipments; }
    public set NumberOfShipments(newValue: number) { this.numberOfShipments = newValue; this.MarkAsDirty(); }

    private customerId: string;
    public get CustomerId() { return this.customerId; }
    public set CustomerId(newValue: string) { this.customerId = newValue; this.MarkAsDirty(); }


    private year: number;
    public get Year() { return this.year; }
    public set Year(newValue: number) { this.year = newValue; this.MarkAsDirty(); }


    private tenant: number;
    public get Tenant() { return this.tenant; }
    public set Tenant(newValue: number) { this.tenant = newValue; this.MarkAsDirty(); }


    private tEU: number;
    public get TEU() { return this.tEU; }
    public set TEU(newValue: number) { this.tEU = newValue; this.MarkAsDirty(); }

    public OldEntityPM: any;

    public UniqueKey: string;

    public IsDirty: boolean;
    MarkAsDirty() {
        this.IsDirty = true;
        if (this.entityParentPM) {
            this.entityParentPM.MarkAsDirty();
        }
    }
}