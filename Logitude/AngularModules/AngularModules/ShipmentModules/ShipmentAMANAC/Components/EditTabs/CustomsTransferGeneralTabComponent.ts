import { Component } from '@angular/core';
import { EntityArgs } from '../../../../Infrastructure/DataContracts/EntityArgs';
import { CustomsTransferHeaderPM } from '../../../../Shipment/EntityPMs/CustomsTransferHeaderPM';
import { CustomsTransferLinePM } from '../../../../Shipment/EntityPMs/CustomsTransferLinePM';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { DownloadManager } from '../../../../Infrastructure/Utilities/DownloadManager';
import { ShipmentDomainService } from '../../../../Shipment/Services/ShipmentDomainService';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';

@Component({
    moduleId: module.id,
    templateUrl: './CustomsTransferGeneralTabComponent.html',
})

export class CustomsTransferGeneralTabComponent extends BaseComponent {
    public EntityPM: CustomsTransferHeaderPM = null;
    public ObjectTableName = "CustomsTransferHeader";
    public DataContext = this;
    public ItemsSource: CustomsTransferLinePM[] = [];
    public SelectedItem: CustomsTransferLinePM = null;
    private shipmentDomainService: ShipmentDomainService;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private entityArgs: EntityArgs) {
        super();
        this.EntityPM = entityArgs.EntityPM;
        this.ItemsSource = this.EntityPM.CustomsTransferLines;
        this.shipmentDomainService = new ShipmentDomainService();        
    }
    
    RebuildClicked() {
        this.CurrentSession.StartBusyIndicator("Rebuilding ...");

        this.shipmentDomainService.RebuildTransferFile(this.EntityPM.Id).subscribe((myResponse: ServiceResponse) => {
            this.CurrentSession.StopBusyIndicator();
        });
    }

    DownloadClicked() {
        DownloadManager.DownloadTransferHeaderFile(this.EntityPM.FileName);
    }
}
