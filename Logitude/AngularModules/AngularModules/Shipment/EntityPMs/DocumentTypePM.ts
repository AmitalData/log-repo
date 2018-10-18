
import {UIProperties, UIProperty} from '../../Infrastructure/Components/LogitudeComponents/UIProperties';
export class DocumentTypePM {


    public UIProperties: UIProperties;

    constructor() {
        this.UIProperties = new UIProperties;
        this.IsDirty = false;
    }

    public IsDirty: boolean;


    private _Id: string;
    public get Id() { return this._Id; }
    public set Id(newValue: string) { this._Id = newValue; this.MarkAsDirty(); }



    private _Tenant: number;
    public get Tenant() { return this._Tenant; }
    public set Tenant(newValue: number) { this._Tenant = newValue; this.MarkAsDirty(); }

    private _Code: string;
    public get Code() { return this._Code; }
    public set Code(newValue: string) { this._Code = newValue; this.MarkAsDirty(); }

    private _Name: string;
    public get Name() { return this._Name; }
    public set Name(newValue: string) { this._Name = newValue; this.MarkAsDirty(); }

    private _Notes: string;
    public get Notes() { return this._Notes; }
    public set Notes(newValue: string) { this._Notes = newValue; this.MarkAsDirty(); }



    private _IsAir: boolean;
    public get IsAir() { return this._IsAir; }
    public set IsAir(newValue: boolean) { this._IsAir = newValue; this.MarkAsDirty(); }



    private _IsOcean: boolean;
    public get IsOcean() { return this._IsOcean; }
    public set IsOcean(newValue: boolean) { this._IsOcean = newValue; this.MarkAsDirty(); }


    private _IsInland: boolean;
    public get IsInland() { return this._IsInland; }
    public set IsInland(newValue: boolean) { this._IsInland = newValue; this.MarkAsDirty(); }


    private _IsDocIn: boolean;
    public get IsDocIn() { return this._IsDocIn; }
    public set IsDocIn(newValue: boolean) { this._IsDocIn = newValue; this.MarkAsDirty(); }



    private _IsDocOut: boolean;
    public get IsDocOut() { return this._IsDocOut; }
    public set IsDocOut(newValue: boolean) { this._IsDocOut = newValue; this.MarkAsDirty(); }



    private _InActive: boolean;
    public get InActive() { return this._InActive; }
    public set InActive(newValue: boolean) { this._InActive = newValue; this.MarkAsDirty(); }

    private _TemplateFormatCode: string;
    public get TemplateFormatCode() { return this._TemplateFormatCode; }
    public set TemplateFormatCode(newValue: string) { this._TemplateFormatCode = newValue; this.MarkAsDirty(); }


    private _IsMaster: boolean;
    public get IsMaster() { return this._IsMaster; }
    public set IsMaster(newValue: boolean) { this._IsMaster = newValue; this.MarkAsDirty(); }




    private _IsDirect: boolean;
    public get IsDirect() { return this._IsDirect; }
    public set IsDirect(newValue: boolean) { this._IsDirect = newValue; this.MarkAsDirty(); }



    private _IsHouse: boolean;
    public get IsHouse() { return this._IsHouse; }
    public set IsHouse(newValue: boolean) { this._IsHouse = newValue; this.MarkAsDirty(); }


    private _IsReadOnly: boolean;
    public get IsReadOnly() { return this._IsReadOnly; }
    public set IsReadOnly(newValue: boolean) { this._IsReadOnly = newValue; this.MarkAsDirty(); }

    private _Subject: string;
    public get Subject() { return this._Subject; }
    public set Subject(newValue: string) { this._Subject = newValue; this.MarkAsDirty(); }

    private _FollowUpTypeId: string;
    public get FollowUpTypeId() { return this._FollowUpTypeId; }
    public set FollowUpTypeId(newValue: string) { this._FollowUpTypeId = newValue; this.MarkAsDirty(); }



    private _FollowUpTypeName: string;
    public get FollowUpTypeName() { return this._FollowUpTypeName; }
    public set FollowUpTypeName(newValue: string) { this._FollowUpTypeName = newValue; this.MarkAsDirty(); }



    private _ObjectTableId: string;
    public get ObjectTableId() { return this._ObjectTableId; }
    public set ObjectTableId(newValue: string) { this._ObjectTableId = newValue; this.MarkAsDirty(); }


    private _ChildEntityId: string;
    public get ChildEntityId() { return this._ChildEntityId; }
    public set ChildEntityId(newValue: string) { this._ChildEntityId = newValue; this.MarkAsDirty(); }

    private _ChildEntityReference: string;
    public get ChildEntityReference() { return this._ChildEntityReference; }
    public set ChildEntityReference(newValue: string) { this._ChildEntityReference = newValue; this.MarkAsDirty(); }


MarkAsDirty() {
    this.IsDirty = true;
}
}