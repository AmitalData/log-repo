import {Component} from '@angular/core';
import {INTRAWebService} from '../../../../Shipment/Services/INTRAWebService';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {AppTool, DateTool} from '../../../../Infrastructure/Tools';

@Component({
    moduleId: module.id,
    templateUrl: './LastStatusComponent.html',
})

export class LastStatusComponent {
    private ShipmentId: string = null;
    private ContainerId: string = null;
    public ValidationErrorsList: string[] = [];
    public ItemsSource: LastStatusItem[] = [];
    constructor() {

    }

    SetWindowArgs(args: any) {
        this.ShipmentId = args["ShipmentId"];
        this.ContainerId = args["ContainerId"];

        var myService = new INTRAWebService();
        myService.GetContainerStatuses(this.ShipmentId, this.ContainerId).subscribe((myResponse: ServiceResponse) => {

            this.ItemsSource = [];

            if (myResponse.HasError) {
                this.ValidationErrorsList = myResponse.ErrorsArray;
            }

            else {

                var itemsSource: LastStatusItem[] = [];
                var items: any[] = myResponse.Result;

                items.forEach(item => {
                    itemsSource.push(item);
                });

                itemsSource.sort((a, b) => { return (a.SortingValue === b.SortingValue) ? 0 : (a.SortingValue < b.SortingValue) ? -1 : 1 }).forEach(item => {
                    this.ItemsSource.push(item);
                });
            }
        });
    }

    CloseClicked() {
        SessionLocator.CurrentSession.CloseCurrentWindow();
    }
}

class LastStatusItem {
    public StatusName: string;
    public EventDate: Date;
    public ReceivingDate: Date;
    public LocationCode: string;
    public DepartureDate: Date;
    public ArrivalDate: Date;
    public SortingValue: number = 0;
    constructor(item:any) {
        if (item) {

            this.StatusName = item.StatusName;
            this.EventDate = item.EventDate;
            this.ReceivingDate = item.ReceivingDate;
            this.LocationCode = item.LocationCode;
            this.DepartureDate = item.DepartureDate;
            this.ArrivalDate = item.ArrivalDate;

            if (this.EventDate) {
                this.SortingValue = DateTool.GetDateParts(this.EventDate).DateTicks;
            }
        }
    }
}