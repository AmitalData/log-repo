import {UIProperties, UIProperty} from '../../Infrastructure/Components/LogitudeComponents/UIProperties';
export class CustomerCompetitorPM {

    public UIProperties: UIProperties;
    constructor(entityParentPM: any) {
        this.EntityParentPM = entityParentPM
        this.UIProperties = new UIProperties;
        this.IsDirty = false;
    }

    private customerId: string;
    public get CustomerId() { return this.customerId; }
    public set CustomerId(value: string) { this.customerId = value; this.MarkAsDirty(); }

    private competitorId: string;
    public get CompetitorId() { return this.competitorId; }
    public set CompetitorId(value: string) { this.competitorId = value; this.MarkAsDirty(); }


    private tenant: number;
    public get Tenant() { return this.tenant; }
    public set Tenant(value: number) { this.tenant = value; this.MarkAsDirty(); }


    private customerName: string;
    public get CustomerName() { return this.customerName; }
    public set CustomerName(value: string) { this.customerName = value; this.MarkAsDirty(); }


    private competitorName: string;
    public get CompetitorName() { return this.competitorName; }
    public set CompetitorName(value: string) { this.competitorName = value; this.MarkAsDirty(); }

    private competitorWebsite: string;
    public get CompetitorWebsite() { return this.competitorWebsite; }
    public set CompetitorWebsite(value: string) { this.competitorWebsite = value; this.MarkAsDirty(); }


    private competitorStrengths: string;
    public get CompetitorStrengths() { return this.competitorStrengths; }
    public set CompetitorStrengths(value: string) { this.competitorStrengths = value; this.MarkAsDirty(); }


    private competitorWeaknesses: string;
    public get CompetitorWeaknesses() { return this.competitorWeaknesses; }
    public set CompetitorWeaknesses(value: string) { this.competitorWeaknesses = value; this.MarkAsDirty(); }



    private competitorOpportunity: string;
    public get CompetitorOpportunity() { return this.competitorOpportunity; }
    public set CompetitorOpportunity(value: string) { this.competitorOpportunity = value; this.MarkAsDirty(); }


    private competitorThreat: string;
    public get CompetitorThreat() { return this.competitorThreat; }
    public set CompetitorThreat(value: string) { this.competitorThreat = value; this.MarkAsDirty(); }


    private entityParentPM: any;
    public get EntityParentPM() { return this.entityParentPM; }
    public set EntityParentPM(newValue: any) { this.entityParentPM = newValue; }

    private changeSetOp: string;
    public get ChangeSetOp() { return this.changeSetOp; }
    public set ChangeSetOp(newValue: string) { this.changeSetOp = newValue; this.MarkAsDirty(); }


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