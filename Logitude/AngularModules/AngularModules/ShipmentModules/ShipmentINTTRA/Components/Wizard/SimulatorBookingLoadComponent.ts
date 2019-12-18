declare var window: any;
import { Component, AfterViewInit, ViewChild, ViewContainerRef } from '@angular/core';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { ShipmentPM } from '../../../../Shipment/EntityPMs/ShipmentPM';
import { ShipmentPMService } from '../../../../Shipment/Services/StandardPMs/ShipmentPMService';
import { EntityLastActivityService } from '../../../../Infrastructure/Services/EntityLastActivityService';
import { ShipmentTool } from '../../../../Shipment/Tools';
import { AWBWizardArgs, FSRWizardArgs } from '../../../../Shipment/Args';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';

@Component({
    moduleId: module.id,

    templateUrl: './SimulatorBookingLoadComponent.html',
})

export class SimulatorBookingLoadComponent implements AfterViewInit {
    public EntityId: string = null;
    public EntityPM: ShipmentPM;
    @ViewChild('WizardView', { read: ViewContainerRef }) target: ViewContainerRef;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {

    }

    SetWindowArgs(entityId: string) {
        this.CurrentSession.StartBusyIndicatorLoading();
        this.EntityId = entityId;
        this.Load();
    }

    private isViewInited = false;
    ngAfterViewInit() {
        this.isViewInited = true;
        this.Load();
    }

    private Load() {
        if (this.EntityId != null && this.isViewInited) {

            var myService: ShipmentPMService = new ShipmentPMService();

            myService.get(this.EntityId).subscribe((myResponse: ServiceResponse) => {
                if (myResponse != null) {
                    if (!myResponse.HasError) {
                        this.EntityPM = myResponse.Result;

                        if (this.EntityPM != null) {
                            this.RunINTTRABookingWizard();
                        }
                    }
                }

                this.CurrentSession.StopBusyIndicator();
            });
        }
    }

    private RunINTTRABookingWizard() {
        var WindowArgs = { Shipment: this.EntityPM, IsEditMode: true };
        SessionLocator.DynamicLoader.Load('./ShipmentModules/ShipmentINTTRA/Components/Wizard/SimulatorBookingComponent', this.target)
            .then(cmpRef => {
                cmpRef.instance.SetWindowArgs(WindowArgs);
                this.CurrentSession.StopBusyIndicator();
            });
    }

}
