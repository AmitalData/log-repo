import { Component } from '@angular/core';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { CustomsTransferHeaderPM } from '../../../../Shipment/EntityPMs/CustomsTransferHeaderPM';
import { CustomsTransferHeaderPMService } from '../../../../Shipment/Services/StandardPMs/CustomsTransferHeaderPMService';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { DownloadManager } from '../../../../Infrastructure/Utilities/DownloadManager';
import { ShipmentDomainService } from '../../../../Shipment/Services/ShipmentDomainService';
import { LogitudeWindow } from '../../../../Controls/Windows/LogitudeWindow';

@Component({
    
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
    public IsValidated: boolean = false;
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
        if (this.EntityPM.CustomsTransferTypeCode == "AMAS") {
            var shipmentService: ShipmentDomainService = new ShipmentDomainService();
            shipmentService.ValidateAMANACShipmentsBeforeExporting(this.EntityPM).subscribe((myResponse: ServiceResponse) => {
                this.IsExportingInProgress = false;

                if (!myResponse.HasError) {
                    if (this.EntityPM.CustomsTransferLines.filter(d => d.HasError).length > 0) {
                        this.IsValidated = true;
                    }

                    else {
                        this.Transfer();
                    }
                }
            });
        }

        else {
            this.Transfer();
        }                
    }

    Transfer() {
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
                this.CurrentSession.FireEvent("TransferCompleted");
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

    ViewErrorsClicked() {
        var logWindow = new LogitudeWindow();
        logWindow.Title = "Errors";
        logWindow.Width = 600;
        logWindow.Height = 500;
        logWindow.WindowArgs = this.EntityPM.CustomsTransferLines;
        logWindow.Show('./ShipmentModules/ShipmentAMANAC/Components/NewEntity/AMANACValidationComponent');
    }
}
