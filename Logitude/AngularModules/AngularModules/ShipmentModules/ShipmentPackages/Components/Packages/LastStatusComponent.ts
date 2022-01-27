
import {Component} from '@angular/core';
import {INTRAWebService} from '../../../../Shipment/Services/INTRAWebService';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {AppTool, DateTool} from '../../../../Infrastructure/Tools';
import { ObservableCollection } from '../../../../Infrastructure/Utilities/ObservableCollection';
import { LastStatusItem } from '../../../../Shipment/EntityPMs/LastStatusItem'

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

                var items: any[] = myResponse.Result.filter(a => a.StatusSource == "INT");

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


