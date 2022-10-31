import { HttpClient } from "@angular/common/http";
import { Component, OnInit } from "@angular/core";
import { KeyValuePair } from "CustomsModules/CustomsCourier/Components/CourierWorkSheet/CourierWorksheetComponent";
import { BaseComponent } from "Infrastructure/Components/LogitudeComponents/BaseComponent";
import { EntityResourceService } from "Infrastructure/Services/EntityResourceService";
import { AppTool } from "Infrastructure/Tools";
import { ServiceHelper } from "Infrastructure/Utilities/ServiceHelper";
import { SessionLocator } from "Infrastructure/Utilities/SessionLocator";

@Component({
    selector: 'ExportReportComponent',
    templateUrl: './ExportReportComponent.html',
})


export class ExportReportComponent extends BaseComponent implements OnInit {
    public DataContext: ExportReportComponent = this;
    public ObjectTableName: string = "Customs.Declarations";
  
    _ReportTypes: KeyValuePair[] = [];
    SelectedReportType: KeyValuePair;
    _SelectReportType_Key: string;
    ExportAsExcelButtonIsEnabled: boolean = false;
    private CurrentSession = SessionLocator;
    public ValidationErrorsList: string[];


    constructor(private _entityResourceService: EntityResourceService, private http: HttpClient) {
        super();
      
    }

    _FromExport;
    get FromExport() { return this._FromExport; }
    set FromExport(val: Date) {
        this._FromExport = val;
    }

    _ToDateExport
    get ToDateExport() { return this._ToDateExport; }
    set ToDateExport(val: Date) {
        this._ToDateExport = val;
    }



    
    ngOnInit() {
        this.ExportAsExcelButtonIsEnabled = true;
    }

    ExportExcel() {
        this.ValidationErrorsList = this.GetErrors();
        if (this.ValidationErrorsList.length == 0) {
            var url = ServiceHelper.GetLogitudeURL() + 'api/DeclarationWebService/GetExportReport2Excel?' + this.GetExportReportSettings();
            window.open(url);
        }
    }

    GetErrors(){
        var errors: string[] = [];

        if (!this.ToDateExport && this.FromExport ) {
            errors.push("חובה לבחור עד תאריך ");
        }
        if (this.ToDateExport && !this.FromExport) {
            errors.push("חובה לבחור  מתאריך");
        }
        if (!this.FromExport && !this.ToDateExport ) {
            errors.push("חובה להזין לפחות תאריך אחד");
        }
        return errors;
    }

    GetExportReportSettings() {
        var url = 'tenant=' + this.CurrentSession.Tenant.toString();
      
        url += '&ExportFromDate=' + this.GetDateWithoutTime(this.FromExport);
        url += '&ExportToDate=' + this.GetDateWithoutTime(this.ToDateExport);
       
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