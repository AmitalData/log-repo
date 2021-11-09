import { HttpClient } from "@angular/common/http";
import { Component, OnInit } from "@angular/core";
import { KeyValuePair } from "CustomsModules/CustomsCourier/Components/CourierWorkSheet/CourierWorksheetComponent";
import { BaseComponent } from "Infrastructure/Components/LogitudeComponents/BaseComponent";
import { EntityResourceService } from "Infrastructure/Services/EntityResourceService";
import { AppTool } from "Infrastructure/Tools";
import { ServiceHelper } from "Infrastructure/Utilities/ServiceHelper";
import { SessionLocator } from "Infrastructure/Utilities/SessionLocator";

@Component({
    selector: 'LastMileReportComponent',
    templateUrl: './LastMileReportComponent.html',
})


export class LastMileReportComponent extends BaseComponent implements OnInit {
    public DataContext: LastMileReportComponent = this;
    public ObjectTableName: string = "Customs.DeclarationCourierStatus";
    EntityResource = false;
    _ReportTypes: KeyValuePair[] = [];
    SelectedReportType: KeyValuePair;
    _SelectReportType_Key: string;
    ExportAsExcelButtonIsEnabled: boolean = false;
    private CurrentSession = SessionLocator;
    SlaReportSettings: SlaReportSettings;
    public ValidationErrorsList: string[];


    constructor(private _entityResourceService: EntityResourceService, private http: HttpClient) {
        super();
        this._entityResourceService.getEntityResourceByTableName("Customs.DeclarationCourierStatus", 0).subscribe((response: any) => {
            this.EntityResource = true;
            this.SlaReportSettings = new SlaReportSettings();
        });

        this.UIProperties.SetVisibility("FromLastMile", this.ObjectTableName, true);
    }

    _FromLastMile;
    get FromLastMile() { return this._FromLastMile; }
    set FromLastMile(val: Date) {
        this._FromLastMile = val;
    }

    _ToDateLastMile
    get ToDateLastMile() { return this._ToDateLastMile; }
    set ToDateLastMile(val: Date) {
        this._ToDateLastMile = val;
    }

    _FromHatraDate;
    get FromHatra() { return this._FromHatraDate; }
    set FromHatra(val: Date) {
        this._FromHatraDate = val;
    }

    _ToHatraDate;
    get ToHatraDate() { return this._ToHatraDate; }
    set ToHatraDate(val: Date) {
        this._ToHatraDate = val;
    }

    _Trucker;
    get Trucker() { return this._Trucker; }
    set Trucker(val: string) {
        this._Trucker = val;
    }

    _Airline;
    get Airline() { return this._Airline; }
    set Airline(value: string) {
        this._Airline=value;
    }

    _CourierHawb;
    get CourierHawb() { return this._CourierHawb; }
    set CourierHawb(value: string) {
        this._CourierHawb=value;
    }

    
    ngOnInit() {
        this.ExportAsExcelButtonIsEnabled = true;
    }

    ExportExcel() {
        var errors: string[] = [];
        if (!this.FromLastMile) {
            errors.push("חובה לבחור מתאריך ");
        }
        if (!this.ToDateLastMile) {
            errors.push("חובה לבחור עד תאריך ");
        }
        this.ValidationErrorsList = errors;
        if (this.ValidationErrorsList.length == 0) {
            this.GetSlaReportSettings();
            var url = ServiceHelper.GetLogitudeURL() + 'api/DeclarationCourierStatusWebService/GetSLAReport2Excel?' + this.GetSlaReportSettings();
            window.open(url);
        }
    }

    GetSlaReportSettings() {
        this.SlaReportSettings.Tenant = this.CurrentSession.Tenant;
        var url = 'tenant=' + this.SlaReportSettings.Tenant.toString();
        url += '&fromDate=' + this.GetDateWithoutTime(this.SlaReportSettings.FromRequestCreateDate);
        url += '&toDate=' + this.GetDateWithoutTime(this.SlaReportSettings.ToRequestCreateDate);
        url += '&reportType=' + this._SelectReportType_Key;
        return url;

    }

    GetDateWithoutTime(datetime) {
        if (datetime) {
            var date = new Date(datetime.getTime());
            date.setHours(0, 0, 0, 0);
            return date.toLocaleDateString('he-IL', { timeZone: 'Asia/Jerusalem' }).replace(/\D/g, '/')
            return date.toJSON();
        }
    }

    CancelButtonClicked() {
        this.CurrentSession.SelectedSession.CloseCurrentWindow();
    }


}

export class SlaReportSettings {
    constructor() {

    }
    private _Tenant: number;
    public get Tenant(): number {
        return this._Tenant;
    }
    public set Tenant(value: number) {
        this._Tenant = value;
    }
    private _FromRequestCreateDate: Date;
    public get FromRequestCreateDate(): Date {
        return this._FromRequestCreateDate;
    }
    public set FromRequestCreateDate(value: Date) {
        this._FromRequestCreateDate = value;
    }
    private _ToRequestCreateDate: Date;
    public get ToRequestCreateDate(): Date {
        return this._ToRequestCreateDate;
    }
    public set ToRequestCreateDate(value: Date) {
        this._ToRequestCreateDate = value;
    }
    private _IntegratorCode: string;
    public get IntegratorCode(): string {
        return this._IntegratorCode;
    }
    public set IntegratorCode(value: string) {
        this._IntegratorCode = value;
    }
    private _ReportType: string;
    public get ReportType(): string {
        return this._ReportType;
    }
    public set ReportType(value: string) {
        this._ReportType = value;
    }
}