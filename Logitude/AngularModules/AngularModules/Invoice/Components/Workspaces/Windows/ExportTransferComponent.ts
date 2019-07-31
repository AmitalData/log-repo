import {Component} from '@angular/core';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {AccountingTransferHeaderPM} from '../../../EntityPMs/AccountingTransferHeaderPM';
import {AccountingTransferHeaderPMService} from '../../../Services/StandardPMs/AccountingTransferHeaderPMService';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {DownloadManager} from '../../../../Infrastructure/Utilities/DownloadManager';

@Component({
    moduleId: module.id,
    templateUrl: './ExportTransferComponent.html',
})

export class ExportTransferComponent {
    public EntityPM: AccountingTransferHeaderPM;
    public TransferredCount: number = 0;
    public TransferredEntityName: string = null;
    public IsResourcesReady: boolean = false;
    public IsExportingInProgress: boolean = true;
    public IsExportingSuccess: boolean = false;
    public IsExportingError: boolean = false;
    private entityPMService: AccountingTransferHeaderPMService;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        this.entityPMService = new AccountingTransferHeaderPMService();
    }

    Export(entityPM: AccountingTransferHeaderPM) {
        this.EntityPM = entityPM;
        this.TransferredCount = entityPM.TransferLines.length;

        switch (this.EntityPM.AccountingTransferTypeCode) {
            case "ARIN": { this.TransferredEntityName = "A/R Invoice"; break; }
            case "APIN": { this.TransferredEntityName = "A/P Invoice"; break; }
            case "ARPA": { this.TransferredEntityName = "A/R Payment"; break; }
            case "APPA": { this.TransferredEntityName = "A/P Payment"; break; }
        }

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
