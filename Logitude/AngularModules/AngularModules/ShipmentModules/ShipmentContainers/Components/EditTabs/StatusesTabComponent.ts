import { Component, OnInit } from '@angular/core';
import { EntityArgs } from '../../../../Infrastructure/DataContracts/EntityArgs';
import { ContainerPM } from '../../../../Shipment/EntityPMs/ContainerPM';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { ObservableCollection } from '../../../../Infrastructure/Utilities/ObservableCollection';
import { INTRAWebService } from '../../../../Shipment/Services/INTRAWebService';
import { LastStatusItem } from '../../../../Shipment/EntityPMs/LastStatusItem'

@Component({
    templateUrl: './StatusesTabComponent.html',
})

export class StatusesTabComponent extends BaseComponent implements OnInit {
    public EntityPM: ContainerPM = null;
    public ObjectTableName = "Container";
    public DataContext = this;
    public ItemsSource: ObservableCollection;

    constructor(private entityArgs: EntityArgs) {
        super();
        this.EntityPM = entityArgs.EntityPM;
        this.ItemsSource = new ObservableCollection([]);
    }

    ngOnInit() {
        this.GetContainerStatuses();
    }
    GetContainerStatuses() {
        var shipmentId = this.EntityPM.ShipmentId;
        var containerId = this.EntityPM.ShipmentPackagesId;

        var myService = new INTRAWebService();
        myService.GetContainerStatuses(shipmentId, containerId).subscribe((myResponse: ServiceResponse) => {

            var itemsSource: LastStatusItem[] = [];
            var itemsCollection: LastStatusItem[] = [];

            if (myResponse.HasError) {
                this.entityArgs.EditComponent.ValidationErrorsList = myResponse.ErrorsArray;
            }

            else {
                var items: any[] = myResponse.Result.filter(a => a.StatusSource == "OIN");
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
  
}
