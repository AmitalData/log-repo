import { Component } from '@angular/core';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { ShipmentDomainService } from '../../../../Shipment/Services/ShipmentDomainService';
import { ServiceHelper } from '../../../../Infrastructure/Utilities/ServiceHelper';
import { AppTool } from '../../../../Infrastructure/Tools';
import { DownloadManager } from '../../../../Infrastructure/Utilities/DownloadManager';

@Component({
    moduleId: module.id,
    templateUrl: './DownloadPackagesFileComponent.html',
})

export class DownloadPackagesFileComponent {
    public IsResourcesReady: boolean = false;
    public IsDownloadInProgress: boolean = true;
    public IsDownloadingSuccess: boolean = false;
    public IsDownloadingError: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {

    }

    private EntityId: string;
    Download(entityId: string) {
        this.EntityId = entityId;
        this.IsResourcesReady = true;
        this.Start();
    }

    private FileName: string;
    Start() {        
        this.IsDownloadInProgress = true;
        this.IsDownloadingSuccess = false;
        this.IsDownloadingError = false;

        var myDomainService = new ShipmentDomainService();
        myDomainService.DownloadShipmentPackages(this.EntityId).subscribe((myResponse: ServiceResponse) => {
            this.IsDownloadInProgress = false;

            if (myResponse.HasError) {
                this.IsDownloadingError = true;
            }

            else {
                this.FileName = myResponse.Result;
                this.IsDownloadingSuccess = true;
            }
        });
    }

    RetryClicked() {
        this.Start();
    }

    DownloadClicked() {
        var tempDate = new Date();
        var MyDate = tempDate.getDate() + "-" + (tempDate.getMonth() + 1) + "-" + tempDate.getFullYear();

        var url = ServiceHelper.GetLogitudeURL() + "WebPages/DawnLoadExcelPage.aspx?fileName=" + this.FileName + "&tempId=" + ServiceHelper.GetLDocumentDownloadToken() + "&qname=" + "ShipmentPackages" + "_" + MyDate + "&Type=SaveToMicrosoftExcel2007";
        {
            window.open(url);
        }

        this.CurrentSession.CloseCurrentWindow();
    }

    CloseButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }
}
