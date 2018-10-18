
import {UIProperties, UIProperty} from '../../Infrastructure/Components/LogitudeComponents/UIProperties';
import {ServiceHelper} from '../../Infrastructure/Utilities/ServiceHelper';
export class SharedLogisticContactPM {

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


    private cardId: string;
    public get CardId() { return this.cardId; }
    public set CardId(newValue: string) { this.cardId = newValue; this.MarkAsDirty(); }


    private contactId: string;
    public get ContactId() { return this.contactId; }
    public set ContactId(newValue: string) { this.contactId = newValue; this.MarkAsDirty(); }


    private name: string;
    public get Name() { return this.name; }
    public set Name(newValue: string) { this.name = newValue; this.MarkAsDirty(); }



    private email: string;
    public get Email() { return this.email; }
    public set Email(newValue: string) { this.email = newValue; this.MarkAsDirty(); }


    private internetAccess: boolean;
    public get InternetAccess() { return this.internetAccess; }
    public set InternetAccess(newValue: boolean) { this.internetAccess = newValue; this.MarkAsDirty(); }

    private englishName: string;
    public get EnglishName() { return this.englishName; }
    public set EnglishName(newValue: string) { this.englishName = newValue; this.MarkAsDirty(); }


    private position: string;
    public get Position() { return this.position; }
    public set Position(newValue: string) { this.position = newValue; this.MarkAsDirty(); }



    private businessPhone: string;
    public get BusinessPhone() { return this.businessPhone; }
    public set BusinessPhone(newValue: string) { this.businessPhone = newValue; this.MarkAsDirty(); }


    private mobile: string;
    public get Mobile() { return this.mobile; }
    public set Mobile(newValue: string) { this.mobile = newValue; this.MarkAsDirty(); }

    private fax: string;
    public get Fax() { return this.fax; }
    public set Fax(newValue: string) { this.fax = newValue; this.MarkAsDirty(); }


    private lastLoginDate: string;
    public get LastLoginDate() { return this.lastLoginDate; }
    public set LastLoginDate(newValue: string) { this.lastLoginDate = newValue; this.MarkAsDirty(); }

    public OldEntityPM: SharedLogisticContactPM;

    public IsDirty: boolean;
    MarkAsDirty() {
        this.IsDirty = true;

    }
    private MyClone: SharedLogisticContactPM;

    public CloneMe() {
        ServiceHelper.CloneEntityPM(this);
    }

    public RejectChanges() {
        ServiceHelper.RejectEntityPMChanges(this);
    }

}
