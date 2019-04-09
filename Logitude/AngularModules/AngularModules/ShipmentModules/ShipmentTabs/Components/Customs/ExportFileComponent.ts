import {Component} from '@angular/core';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {DownloadManager} from '../../../../Infrastructure/Utilities/DownloadManager';
import {ShipmentDomainService} from '../../../../Shipment/Services/ShipmentDomainService';

@Component({
    moduleId: module.id,
    templateUrl: './ExportFileComponent.html',
})

export class ExportFileComponent {
    public IsResourcesReady: boolean = false;
    public IsExportingInProgress: boolean = true;
    public IsExportingSuccess: boolean = false;
    public IsExportingError: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        
    }

    private EntityId: string;
    Export(entityId: string) {
        this.EntityId = entityId;
        this.IsResourcesReady = true;
        this.StartExporting();
    }

    private FileName: string;
    StartExporting() {
        this.IsExportingInProgress = true;
        this.IsExportingSuccess = false;
        this.IsExportingError = false;

        var service: ShipmentDomainService = new ShipmentDomainService();
        service.SendToCustoms_AES(this.EntityId).subscribe((myResponse: ServiceResponse) => {
            this.IsExportingInProgress = false;

            if (myResponse.HasError) {
                this.IsExportingError = true;
            }

            else {
                this.FileName = myResponse.Result;
                this.IsExportingSuccess = true;
            }
        });
    }

    RetryClicked() {
        this.StartExporting();
    }

    DownloadClicked() {
        DownloadManager.DownloadTransferHeaderFile(this.FileName);
    }

    CloseButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }
}
