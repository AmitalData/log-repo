import {Component} from '@angular/core';
import {AppTool} from '../../../../Infrastructure/Tools';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {GlobalDomainService} from '../../../../Common/Services/GlobalDomainService';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {MessageWindow} from '../../../../Controls/Windows/MessageWindow';

@Component({
    moduleId: module.id,
    templateUrl: './LoadSampleDataComponent.html',
})

export class LoadSampleDataComponent {
    public ShipmentNumbersList: number[] = [];
    public ErrorsMessage: string;
    private myService: GlobalDomainService;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        this.ShipmentNumbersList.push(100);
        this.ShipmentNumbersList.push(1000);
        this.ShipmentNumbersList.push(5000);
        this.ShipmentNumbersList.push(10000);
        this.ShipmentNumbersList.push(50000);
        this.myService = new GlobalDomainService();
    }

    private selectedShipmentsNumber: number;
    get SelectedShipmentsNumber() { return this.selectedShipmentsNumber; }
    set SelectedShipmentsNumber(value: number) {
        if (this.selectedShipmentsNumber != value) {
            this.selectedShipmentsNumber = value;
            this.Validate();
        }
    }

    ButtonClicked(myCommand: string) {
        if (!AppTool.IsNullOrEmpty(myCommand)) {

            this.CurrentSession.StartBusyIndicatorLoading();

            this.myService.UpdateTenantZeroService(myCommand).subscribe((myResponse: ServiceResponse) => {

                this.CurrentSession.StopBusyIndicator();

                var messageWindow = new MessageWindow();

                if (myResponse.HasError) {
                    messageWindow.Show(myResponse.ErrorsArray[0]);
                }

                else {
                    messageWindow.Show("Updating tenant 0 an email will be sent as soon as update completes.");
                }
            });
        }
    }

    Validate() {
        var errorsMessage: string;

        if (AppTool.IsNullOrEmpty(this.SelectedShipmentsNumber)) {
            errorsMessage = "Please select number of shipments";
        }

        this.ErrorsMessage = errorsMessage;
    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    OkButtonClicked() {
        this.Validate();
    }
}
