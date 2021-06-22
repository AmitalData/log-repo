import {Component} from '@angular/core';
import {INTRAWebService} from '../../../../Shipment/Services/INTRAWebService';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {AppTool, DateTool} from '../../../../Infrastructure/Tools';
import { ObservableCollection } from '../../../../Infrastructure/Utilities/ObservableCollection';

@Component({
    
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
    public DepartureArrivalDate: Date;
    public SortingValue: number = 0;
    public VesselName: string;
    public VoyageNumber: string;
    public DepartureDateInfo: string;
    public ArrivalDateInfo: string;
    public DepartureArrivalDateInfo: string;
    public Source: string;

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
            this.Source = item.StatusSource;

            if (this.DepartureDate) {
                this.DepartureDateInfo = item.TimeOfDepartureInfo == "E" ? "ETD" : "ATD";
            }

            if (this.ArrivalDate) {
                this.ArrivalDateInfo = item.TimeOfArrivalInfo == "E" ? "ETA" : "ATA";
            }

            if (this.EventDate) {
                this.SortingValue = DateTool.GetDateParts(this.EventDate).DateTicks;
            }

            var isDeparture = this.GetEventDirection(item.StatusCode);
            if (isDeparture) {
                this.DepartureArrivalDate = this.DepartureDate;
                this.DepartureArrivalDateInfo = this.DepartureDateInfo;
            }

            else {
                this.DepartureArrivalDate = this.ArrivalDate;
                this.DepartureArrivalDateInfo = this.ArrivalDateInfo;
            }
        }
    }

    GetEventDirection(statusCode: string) {
        var isDeparture = false;

        switch (statusCode) {
            case "2":
            case "3":
            case "AA":
            case "AC":
            case "AE":
            case "AF":
            case "AI":
            case "AW":
            case "B":
            case "BE":
            case "BF":
            case "BR":
            case "C":
            case "CA":
            case "CD":
            case "CO":
            case "CS":
            case "EE":
            case "EP":
            case "GI":
            case "I":
            case "VD":
            case "X3":
            case "X4":
            case "X7":
            case "X8":
            case "XA":
                {
                    isDeparture = true;
                    break;
                }
        }

        return isDeparture;
    }

}
