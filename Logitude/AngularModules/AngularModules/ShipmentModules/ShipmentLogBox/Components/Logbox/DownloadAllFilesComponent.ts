import {Component, Output, EventEmitter} from '@angular/core';
import {TextCodeTranslationPipe} from '../../../../Controls/Pipes/TextCodeTranslationPipe';
import {WebFreightDomainService} from '../../../../Infrastructure/Services/WebFreightDomainService';
import {ApiQueryFilters} from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {ServiceHelper} from '../../../../Infrastructure/Utilities/ServiceHelper';
import { MixPanelLocator } from 'Common/MixPanel/MixPanelLocator';

@Component({
    

    templateUrl: './DownloadAllFilesComponent.html',
    //pipes: [TextCodeTranslationPipe],
    //providers: [ServiceArgs]
})

export class DownloadAllFilesComponent {
    btnRetryVisibile = false;
    busyExportingVisibile = true;
    btnSaveToFileVisibile = false;
    Filters: ApiQueryFilters;
    url: string;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {

    }
    ObjectTableId: string;
    FileName : string;
    tenant: number;
    ShipmentId: string;
    SetWindowArgs(args: any) {
        var myService: WebFreightDomainService = new WebFreightDomainService();
        this.ObjectTableId = args.ObjectTableId;
        this.tenant = SessionLocator.Tenant; 
        this.ShipmentId = args.ShipmentId;
        myService.DownLoadAllFilesForShipments(this.ShipmentId, this.ObjectTableId, this.tenant).subscribe((myResult:any) => {
            if (myResult == "Faild") {
                this.btnRetryVisibile = true;
                this.busyExportingVisibile = false;
                this.btnSaveToFileVisibile = false;
            }
            else {
                this.FileName = myResult;
                this.btnRetryVisibile = false;
                this.busyExportingVisibile = false;
                this.btnSaveToFileVisibile = true;
            }
           
        });
    }

    SaveExcelFile(tenant: number, fileName: string) {
        var token = ServiceHelper.GetLDocumentDownloadToken();
        var url = ServiceHelper.GetLogitudeURL() + "WebPages/DownloadFileName.aspx?id=" + fileName + "&tempId=" + token;
        window.open(url);
        this.CurrentSession.CloseCurrentWindow();
    }

    SaveBtnCLicked() {
        MixPanelLocator.Action({ ProjectName:"LogBox", ActionName: "Download all shipment docs" });
        this.SaveExcelFile(this.tenant, this.FileName);
    }
    RetryBtnClicked() {
        this.btnRetryVisibile = false;
        this.busyExportingVisibile = true;
        this.btnSaveToFileVisibile = false;
        var myService: WebFreightDomainService = new WebFreightDomainService();
        myService.DownLoadAllFilesForShipments(this.ShipmentId, this.ObjectTableId, this.tenant).subscribe((myResult:any) => {
            if (myResult == "Faild") {
                this.btnRetryVisibile = true;
                this.busyExportingVisibile = false;
                this.btnSaveToFileVisibile = false;
            }
            else {
                this.FileName = myResult;
                this.btnRetryVisibile = false;
                this.busyExportingVisibile = false;
                this.btnSaveToFileVisibile = true;
            }

        });
    }

    CancelButtonClicked() {
        /* I need to abort the process */
        //if (exportExcelService != null) {
        //    exportExcelService.CloseAsync();
        //    exportExcelService.Abort();
        //    exportExcelService.ExportQueryToExcelCompleted -= new EventHandler<ExportQueryToExcelCompletedEventArgs>(exportExcelService_ExportQueryToExcelCompleted);
        //}
        this.CurrentSession.CloseCurrentWindow();
    }
    
}
