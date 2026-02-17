
import { UIProperties, UIProperty } from '../../Infrastructure/Components/LogitudeComponents/UIProperties';

export class EmailAlertSettingPM {


    public UIProperties: UIProperties;
    constructor() {
        this.UIProperties = new UIProperties;
        this.IsDirty = false;
    }

    private id: string;
    public get Id() { return this.id; }
    public set Id(newValue: string) { this.id = newValue; this.MarkAsDirty(); }

    private tenant: number;
    public get Tenant() { return this.tenant; }
    public set Tenant(newValue: number) { this.tenant = newValue; this.MarkAsDirty(); }


    private code: string;
    public get Code() { return this.code; }
    public set Code(newValue: string) { this.code = newValue; this.MarkAsDirty(); }



    private description: string;
    public get Description() { return this.description; }
    public set Description(newValue: string) { this.description = newValue; this.MarkAsDirty(); }



    private objectTableId: string;
    public get ObjectTableId() { return this.objectTableId; }
    public set ObjectTableId(newValue: string) { this.objectTableId = newValue; this.MarkAsDirty(); }


    private inActive: boolean;
    public get InActive() { return this.inActive; }
    public set InActive(newValue: boolean) { this.inActive = newValue; this.MarkAsDirty(); }


    private settingLevelCode: string;
    public get SettingLevelCode() { return this.settingLevelCode; }
    public set SettingLevelCode(newValue: string) { this.settingLevelCode = newValue; this.MarkAsDirty(); }

    private to: string;
    public get To() { return this.to; }
    public set To(newValue: string) { this.to = newValue; this.MarkAsDirty(); }

    private indexOrder: number;
    public get IndexOrder() { return this.indexOrder; }
    public set IndexOrder(newValue: number) { this.indexOrder = newValue; this.MarkAsDirty(); }



    public IsDirty: boolean;
    MarkAsDirty() {
        this.IsDirty = true;

    }

}
                                