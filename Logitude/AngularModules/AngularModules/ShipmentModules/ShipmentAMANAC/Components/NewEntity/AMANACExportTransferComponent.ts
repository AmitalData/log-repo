import { Component } from '@angular/core';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { CustomsTransferHeaderPM } from '../../../../Shipment/EntityPMs/CustomsTransferHeaderPM';
import { CustomsTransferHeaderPMService } from '../../../../Shipment/Services/StandardPMs/CustomsTransferHeaderPMService';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { DownloadManager } from '../../../../Infrastructure/Utilities/DownloadManager';

@Component({
    moduleId: module.id,
    templateUrl: './AMANACExportTransferComponent.html',
})

export class AMANACExportTransferComponent {
    public EntityPM: CustomsTransferHeaderPM;
    public TransferredCount: number = 0;
    public TransferredEntityName: string = null;
    public IsResourcesReady: boolean = false;
    public IsExportingInProgress: boolean = true;
    public IsExportingSuccess: boolean = false;
    public IsExportingError: boolean = false;
    private entityPMService: CustomsTransferHeaderPMService;
    private CurrentSession = SessionLocator.SelectedSession;

    constructor() {
        this.entityPMService = new CustomsTransferHeaderPMService();
    }

    Export(entityPM: CustomsTransferHeaderPM) {
        this.EntityPM = entityPM;
        this.TransferredCount = entityPM.CustomsTransferLines.length;
        this.IsResourcesReady = true;
        this.StartExporting();
    }

    StartExporting() {
        this.IsExportingInProgress = true;
        this.IsExportingSuccess = false;
        this.IsExportingError = false;

        this.entityPMService.insert(this.EntityPM).subscribe((myResponse: ServiceResponse) => {
            this.IsExportingInProgress = false;

            if (myResponse.HasError) {
                this.IsExportingError = true;
            }

            else {
                this.IsExportingSuccess = true;
                this.CurrentSession.FireEvent("TransferExportFirstTime");
            }
        });

    }

    RetryClicked() {
        this.StartExporting();
    }

    DownloadClicked() {
        DownloadManager.DownloadTransferHeaderFile(this.EntityPM.FileName);
    }

    CloseButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }
}
