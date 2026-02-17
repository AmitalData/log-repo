

import {UIProperties, UIProperty} from '../../Infrastructure/Components/LogitudeComponents/UIProperties';
export class UserLoginLogPM {

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


    private userId: string;
    public get UserId() { return this.userId; }
    public set UserId(newValue: string) { this.userId = newValue; this.MarkAsDirty(); }


    private iP: string;
    public get IP() { return this.iP; }
    public set IP(newValue: string) { this.iP = newValue; this.MarkAsDirty(); }


    private browser: string;
    public get Browser() { return this.browser; }
    public set Browser(newValue: string) { this.browser = newValue; this.MarkAsDirty(); }


    private gMTDateTime: Date;
    public get GMTDateTime() { return this.gMTDateTime; }
    public set GMTDateTime(newValue: Date) { this.gMTDateTime = newValue; this.MarkAsDirty(); }

    private localDateTime: Date;
    public get LocalDateTime() { return this.localDateTime; }
    public set LocalDateTime(newValue: Date) { this.localDateTime = newValue; this.MarkAsDirty(); }

    
    private computerId: string;
    public get ComputerId() { return this.computerId; }
    public set ComputerId(newValue: string) { this.computerId = newValue; this.MarkAsDirty(); }



    private userAgent: string;
    public get UserAgent() { return this.userAgent; }
    public set UserAgent(newValue: string) { this.userAgent = newValue; this.MarkAsDirty(); }


    private divSelectBackgroud: string;
    public get DivSelectBackgroud() { return this.divSelectBackgroud; }
    public set DivSelectBackgroud(newValue: string) { this.divSelectBackgroud = newValue; this.MarkAsDirty(); }

    private iPSiteUri: string;
    public get IPSiteUri() { return this.iPSiteUri; }
    public set IPSiteUri(newValue: string) { this.iPSiteUri = newValue; this.MarkAsDirty(); }


    public IsDirty: boolean;
    MarkAsDirty() {
        this.IsDirty = true;

    }
}