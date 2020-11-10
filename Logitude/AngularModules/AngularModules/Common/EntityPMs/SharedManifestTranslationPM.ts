import { AgentSharedManifestPM } from './AgentSharedManifestPM';
import { UIProperties, UIProperty } from '../../Infrastructure/Components/LogitudeComponents/UIProperties';

export class SharedManifestTranslationPM {

    public UIProperties: UIProperties;
    constructor() {
        this.UIProperties = new UIProperties;
        this.IsDirty = false;
    }

    private changeSetOp: string;
    public get ChangeSetOp() { return this.changeSetOp; }
    public set ChangeSetOp(newValue: string) { this.changeSetOp = newValue; this.MarkAsDirty(); }
    private entityParentPM;
    public get EntityParentPM() { return this.entityParentPM; }
    public set EntityParentPM(newValue: any) { this.entityParentPM = newValue; this.MarkAsDirty(); }
    public OldEntityPM: SharedManifestTranslationPM;

    private id: string;
    public get Id() { return this.id; }
    public set Id(newValue: string) { this.id = newValue; }

    private tenant: number;
    public get Tenant() { return this.tenant; }
    public set Tenant(newValue: number) { this.tenant = newValue; }

    private objectTableName: string;
    public get ObjectTableName() { return this.objectTableName; }
    public set ObjectTableName(newValue: string) { this.objectTableName = newValue; }

    private myCode: string;
    public get MyCode() { return this.myCode; }
    public set MyCode(newValue: string) { this.myCode = newValue; }

    private agentCode: string;
    public get AgentCode() { return this.agentCode; }
    public set AgentCode(newValue: string) { this.agentCode = newValue; }

    private agentId: string;
    public get AgentId() { return this.agentId; }
    public set AgentId(newValue: string) { this.agentId = newValue; }

    private createdByUserId: string;
    public get CreatedByUserId() { return this.createdByUserId; }
    public set CreatedByUserId(newValue: string) { this.createdByUserId = newValue; }

    private createDate: Date;
    public get CreateDate() { return this.createDate; }
    public set CreateDate(newValue: Date) { this.createDate = newValue; }

    private updatedByUserId: string;
    public get UpdatedByUserId() { return this.updatedByUserId; }
    public set UpdatedByUserId(newValue: string) { this.updatedByUserId = newValue; }

    private updateDate: Date;
    public get UpdateDate() { return this.updateDate; }
    public set UpdateDate(newValue: Date) { this.updateDate = newValue; }

    public UniqueKey: string;

    public IsDirty: boolean;
    public DisableMarkAsDirty: boolean = false;
    MarkAsDirty() {
        if (!this.DisableMarkAsDirty) {
            this.IsDirty = true;
            if (this.entityParentPM) {
                this.entityParentPM.MarkAsDirty();
            }
        }
    }
}
