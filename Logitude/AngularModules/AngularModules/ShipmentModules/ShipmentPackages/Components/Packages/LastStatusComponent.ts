import {Component} from '@angular/core';
import {INTRAWebService} from '../../../../Shipment/Services/INTRAWebService';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {AppTool, DateTool} from '../../../../Infrastructure/Tools';
import { ObservableCollection } from '../../../../Infrastructure/Utilities/ObservableCollection';

@Component({
    moduleId: module.id,
    templateUrl: './LastStatusComponent.html',
})

export class LastStatusComponent {
    private ShipmentId: string = null;
    private ContainerId: string = null;
    public ValidationErrorsList: string[] = [];
    public ItemsSource: ObservableCollection;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        this.ItemsSource = new ObservableCollection([]);
    }

    SetWindowArgs(args: any) {
        this.ShipmentId = args["ShipmentId"];
        this.ContainerId = args["ContainerId"];

        var myService = new INTRAWebService();
        myService.GetContainerStatuses(this.ShipmentId, this.ContainerId).subscribe((myResponse: ServiceResponse) => {

            var itemsSource: LastStatusItem[] = [];
            var itemsCollection: LastStatusItem[] = [];

            if (myResponse.HasError) {
                this.ValidationErrorsList = myResponse.ErrorsArray;
            }

            else {

                var items: any[] = myResponse.Result;

                items.forEach(item => {
                    itemsSource.push(new LastStatusItem(item));
                });

                itemsSource.sort((a, b) => { return (a.SortingValue === b.SortingValue) ? 0 : (a.SortingValue < b.SortingValue) ? -1 : 1 }).forEach(item => {
                    itemsCollection.push(item);
                });

                this.ItemsSource.InsertCollection(itemsCollection);
            }
        });
    }

    CloseClicked() {
        this.CurrentSession.CloseCurrentWindow();
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
    public VesselName: string;
    public VoyageNumber: string;
    public DepartureDateInfo: string;
    public ArrivalDateInfo: string;
    constructor(item:any) {
        if (item) {

            this.StatusName = item.StatusName;
            this.EventDate = item.EventDate;
            this.ReceivingDate = item.ReceivingDate;
            this.LocationCode = item.LocationCode;
            this.DepartureDate = item.DepartureDate;
            this.ArrivalDate = item.ArrivalDate;
            this.VesselName = item.VesselName;
            this.VoyageNumber = item.VoyageNumber;

            if (this.DepartureDate && item.TimeOfDepartureInfo) {
                this.DepartureDateInfo = item.TimeOfDepartureInfo == "E" ? "(expected)" : "(actual)";
            }

            if (this.ArrivalDate && item.TimeOfArrivalInfo) {
                this.ArrivalDateInfo = item.TimeOfArrivalInfo == "E" ? "(expected)" : "(actual)";
            }

            if (this.EventDate) {
                this.SortingValue = DateTool.GetDateParts(this.EventDate).DateTicks;
            }
        }
    }
}
