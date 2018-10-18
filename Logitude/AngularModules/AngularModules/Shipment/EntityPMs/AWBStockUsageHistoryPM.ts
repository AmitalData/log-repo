import {UIProperties, UIProperty} from '../../Infrastructure/Components/LogitudeComponents/UIProperties';

import {AWBMessagingStockPM} from './AWBMessagingStockPM';

export class AWBStockUsageHistoryPM {
    public UIProperties: UIProperties;
    constructor(_entityParentPM: any) {
        this.EntityParentPM = _entityParentPM;
        this.UIProperties = new UIProperties;
        this.IsDirty = false;
    }

    private id: string;
    public get Id() { return this.id; }
    public set Id(newValue: string) { this.id = newValue; this.MarkAsDirty(); }

    private tenant: number;
    public get Tenant() { return this.tenant; }
    public set Tenant(newValue: number) { this.tenant = newValue; this.MarkAsDirty(); }

    private stockId: string;
    public get StockId() { return this.stockId; }
    public set StockId(newValue: string) { this.stockId = newValue; this.MarkAsDirty(); }

    private entityId: string;
    public get EntityId() { return this.entityId; }
    public set EntityId(newValue: string) { this.entityId = newValue; this.MarkAsDirty(); }

    private entityNumber: string;
    public get EntityNumber() { return this.entityNumber; }
    public set EntityNumber(newValue: string) { this.entityNumber = newValue; this.MarkAsDirty(); }

    private objectTableId: string;
    public get ObjectTableId() { return this.objectTableId; }
    public set ObjectTableId(newValue: string) { this.objectTableId = newValue; this.MarkAsDirty(); }

    private messageType: string;
    public get MessageType() { return this.messageType; }
    public set MessageType(newValue: string) { this.messageType = newValue; this.MarkAsDirty(); }

    private mAWB: string;
    public get MAWB() { return this.mAWB; }
    public set MAWB(newValue: string) { this.mAWB = newValue; this.MarkAsDirty(); }

    private hAWB: string;
    public get HAWB() { return this.hAWB; }
    public set HAWB(newValue: string) { this.hAWB = newValue; this.MarkAsDirty(); }

    private actionType: string;
    public get ActionType() { return this.actionType; }
    public set ActionType(newValue: string) { this.actionType = newValue; this.MarkAsDirty(); }

    private firstActionByUserId: string;
    public get FirstActionByUserId() { return this.firstActionByUserId; }
    public set FirstActionByUserId(newValue: string) { this.firstActionByUserId = newValue; this.MarkAsDirty(); }

    private lastActionByUserId: string;
    public get LastActionByUserId() { return this.firstActionByUserId; }
    public set LastActionByUserId(newValue: string) { this.firstActionByUserId = newValue; this.MarkAsDirty(); }

    private firstActionDate: Date;
    public get FirstActionDate() { return this.firstActionDate; }
    public set FirstActionDate(newValue: Date) { this.firstActionDate = newValue; this.MarkAsDirty(); }

    private lastActionDate: Date;
    public get LastActionDate() { return this.lastActionDate; }
    public set LastActionDate(newValue: Date) { this.lastActionDate = newValue; this.MarkAsDirty(); }

    private changeSetOp: string;
    public get ChangeSetOp() { return this.changeSetOp; }
    public set ChangeSetOp(newValue: string) { this.changeSetOp = newValue; this.MarkAsDirty(); }

    public OldEntityPM: AWBStockUsageHistoryPM;

    private entityParentPM: any;
    public get EntityParentPM() { return this.entityParentPM; }
    public set EntityParentPM(newValue: any) { this.entityParentPM = newValue; }

    public UniqueKey: string;

    public IsDirty: boolean;
    MarkAsDirty() {
        this.IsDirty = true;

        if (this.EntityParentPM) {
            this.EntityParentPM.MarkAsDirty();
        }
    }
}