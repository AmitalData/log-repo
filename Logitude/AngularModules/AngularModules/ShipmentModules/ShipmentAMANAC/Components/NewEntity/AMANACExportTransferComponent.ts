import { Component } from '@angular/core';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { AccountingTransferHeaderPM } from '../../../../Invoice/EntityPMs/AccountingTransferHeaderPM';
import { AccountingTransferHeaderPMService } from '../../../../Invoice/Services/StandardPMs/AccountingTransferHeaderPMService';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { DownloadManager } from '../../../../Infrastructure/Utilities/DownloadManager';

@Component({
    moduleId: module.id,
    templateUrl: './AMANACExportTransferComponent.html',
})

export class AMANACExportTransferComponent {
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
        this.IsResourcesReady = true;
        this.StartExporting();
    }

    StartExporting() {
        this.IsExportingInProgress = true;
        this.IsExportingSuccess = false;
        this.IsExportingError = false;

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
