import { HttpClient } from "@angular/common/http";
import { Component, OnInit } from "@angular/core";
import { KeyValuePair } from "CustomsModules/CustomsCourier/Components/CourierWorkSheet/CourierWorksheetComponent";
import { resetHistory } from "cypress/types/sinon";
import { BaseComponent } from "Infrastructure/Components/LogitudeComponents/BaseComponent";
import { EntityResourceService } from "Infrastructure/Services/EntityResourceService";
import { AppTool, DateTool } from "Infrastructure/Tools";
import { ServiceHelper } from "Infrastructure/Utilities/ServiceHelper";
import { SessionLocator } from "Infrastructure/Utilities/SessionLocator";

@Component({
    selector: 'SLAReportComponent',
    templateUrl: './SLAReportComponent.html',
})


export class SLAReportComponent extends BaseComponent implements OnInit {
    public DataContext: SLAReportComponent = this;
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
            this._ReportTypes.push(new KeyValuePair("1", "פירוט"));
            this._ReportTypes.push(new KeyValuePair("2", "ריכוז"));
            this.SlaReportSettings = new SlaReportSettings();
        });

        this.UIProperties.SetVisibility("FromRequestCreateDate", this.ObjectTableName, true);
    }

    _FromRequestCreateDate;
    get FromRequestCreateDate() { return this._FromRequestCreateDate; }
    set FromRequestCreateDate(val: Date) {
        this._FromRequestCreateDate = val;
        this.SlaReportSettings.FromRequestCreateDate = val;
    }

    _ToRequestCreateDate
    get ToRequestCreateDate() { return this._ToRequestCreateDate; }
    set ToRequestCreateDate(val: Date) {
        this._ToRequestCreateDate=val;
        this.SlaReportSettings.ToRequestCreateDate = val;
    }

    _IntegratorCode: string;
    get IntegratorCode() { return this._IntegratorCode; }
    set IntegratorCode(val: string) {
        this._IntegratorCode = val;
    }

    _SelectedKeywordtypeCode: String;
    KeywordtypeCodeClicked(SelectReportType_Key) {
        this._SelectReportType_Key = SelectReportType_Key;
    }

    ngOnInit() {
        this.ExportAsExcelButtonIsEnabled = true;
        // this.FromRequestCreateDate = DateTool.AddDays(DateTool.GetCurrentDateAsUtc(), 0);
    }

    ExportExcel() {
        var errors: string[] = [];
        if (!this.FromRequestCreateDate) {
            errors.push("חובה לבחור מתאריך ");
        }
        if (!this.ToRequestCreateDate) {
            errors.push("חובה לבחור עד תאריך ");
        }
        if (AppTool.IsNullOrEmpty(this._SelectReportType_Key)) {
            errors.push("חובה לבחור סוג דוח ");
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
        url += '&integratorCode=' + this.IntegratorCode;
        url += '&reportType=' + this._SelectReportType_Key;
        return url;

    }

    GetDateWithoutTime(datetime) {
        if (datetime) {
            var date = new Date(datetime.getTime());
            date.setHours(0, 0, 0, 0);
            return date.toString();
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