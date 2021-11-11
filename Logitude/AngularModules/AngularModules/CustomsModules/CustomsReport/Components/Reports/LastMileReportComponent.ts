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
    public ValidationErrorsList: string[];


    constructor(private _entityResourceService: EntityResourceService, private http: HttpClient) {
        super();
        this._entityResourceService.getEntityResourceByTableName("Customs.DeclarationCourierStatus", 0).subscribe((response: any) => {
            this.EntityResource = true;
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
    get FromDateHatra() { return this._FromHatraDate; }
    set FromDateHatra(val: Date) {
        this._FromHatraDate = val;
    }

    _ToHatraDate;
    get ToDateHatra() { return this._ToHatraDate; }
    set ToDateHatra(val: Date) {
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
        /*if (!this.FromLastMile) {
            errors.push("חובה לבחור מתאריך ");
        }
        if (!this.ToDateLastMile) {
            errors.push("חובה לבחור עד תאריך ");
        }*/
        this.ValidationErrorsList = errors;
        debugger;
        if (this.ValidationErrorsList.length == 0) {
            var url = ServiceHelper.GetLogitudeURL() + 'api/DeclarationCourierStatusWebService/GetLastMileReport2Excel?' + this.GetLastMileReportSettings();
            window.open(url);
        }
    }

    GetLastMileReportSettings() {
        var url = 'tenant=' + this.CurrentSession.Tenant.toString();
        url += '&hatraFromDate=' + this.GetDateWithoutTime(this.FromDateHatra);
        url += '&hatraToDate=' + this.GetDateWithoutTime(this.ToDateHatra);
        url += '&lastMileFromDate=' + this.GetDateWithoutTime(this.FromLastMile);
        url += '&LastMileToDate=' + this.GetDateWithoutTime(this.ToDateLastMile);
        url += '&airline=' + this.Airline;
        url += '&trucker=' + this.Trucker;
        url += '&courierHawb=' + this.CourierHawb;
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