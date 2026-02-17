import { Component, Output, EventEmitter } from '@angular/core';
import { TextCodeTranslationPipe } from '../../../Controls/Pipes/TextCodeTranslationPipe';
import { Http } from '@angular/http';
import { WebFreightDomainService } from '../../../Infrastructure/Services/WebFreightDomainService';
import { ApiQueryFilters } from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { TextCodeTranslator } from '../../../Infrastructure/Utilities/TextCodeTranslator';
import { ServiceHelper } from '../../../Infrastructure/Utilities/ServiceHelper';
import { AmitalGatewayUtil } from '../../../Infrastructure/Utilities/AmitalGatewayUtil';
import { ObjectsLocator } from '../../../Infrastructure/Locators/ObjectsLocator';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { BIReportXMLData} from '../../../Infrastructure/Services/InfrastructureDomainService';

@Component({
    moduleId: module.id,
    templateUrl: './ExportBI2ExcelControl.html',
})

export class ExportBI2ExcelControl {
    btnRetryVisibile = false;
    busyExportingVisibile = true;
    btnSaveToFileVisibile = false;
    Filters: ApiQueryFilters;
    url: string;
    RTL: boolean = ObjectsLocator.GlobalSetting == undefined ? false : (ObjectsLocator.GlobalSetting.LayoutDirection == 'rtl' ? true : false);//true;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private http: Http) {
        ServiceHelper.Http = http;
    }
    ObjectTableName: string;
    FileName: string;
    tenant: number;
    queryName: string;
    queryId: string;
    reportId: string;
    userid: string;
    BIReportXMLData: BIReportXMLData = null;

    SetWindowArgs(args: any) {
        var myService: WebFreightDomainService = new WebFreightDomainService();
        this.queryId = args.queryId;
        this.reportId = args.reportId;
        this.queryName = args.reportName;
        this.BIReportXMLData = args.BIReportXMLData;
        myService.GetExportBIReportToExcel(this.BIReportXMLData).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                if (myResponse.Result == "Faild") {
                    this.btnRetryVisibile = true;
                    this.busyExportingVisibile = false;
                    this.btnSaveToFileVisibile = false;
                }
                else {
                    this.FileName = myResponse.Result;
                    this.btnRetryVisibile = false;
                    this.busyExportingVisibile = false;
                    this.btnSaveToFileVisibile = true;
                }
            }
        });
    }

    SaveExcelFile(tenant: number, FileName: string, OTName: string) {
        var tempDate = new Date();
        var MyDate = tempDate.getDate() + "-" + (tempDate.getMonth() + 1) + "-" + tempDate.getFullYear();
        var url = ServiceHelper.GetLogitudeURL() + "WebPages/DawnLoadExcelPage.aspx?fileName=" + FileName + "&tempId=" + ServiceHelper.GetLDocumentDownloadToken() + "&qname=" + this.queryName + "_" + MyDate + "&Type=SaveToMicrosoftExcel2007"; //+ "&bireport=" + "bireport";
        {
            window.open(url);
        }
        this.CurrentSession.CloseCurrentWindow();
    }

    SaveBtnCLicked() {
        this.SaveExcelFile(this.tenant, this.FileName, this.ObjectTableName);
    }
    RetryBtnClicked() {
        this.btnRetryVisibile = false;
        this.busyExportingVisibile = true;
        this.btnSaveToFileVisibile = false;
        var myService: WebFreightDomainService = new WebFreightDomainService();
        myService.GetExportBIReportToExcel(this.BIReportXMLData).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                if (myResponse.Result == "Faild") {
                    this.btnRetryVisibile = true;
                    this.busyExportingVisibile = false;
                    this.btnSaveToFileVisibile = false;
                }
                else {
                    this.FileName = myResponse.Result;
                    this.btnRetryVisibile = false;
                    this.busyExportingVisibile = false;
                    this.btnSaveToFileVisibile = true;
                }
            }
        });
    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }
}
