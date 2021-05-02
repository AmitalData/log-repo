import { Component, OnDestroy } from '@angular/core';
import { EntityArgs } from '../../../../Infrastructure/DataContracts/EntityArgs';
import { TextCodeTranslator } from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import { ShipmentPM } from '../../../../Shipment/EntityPMs/ShipmentPM';
import { ApiQueryFilters } from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { AppTool } from '../../../../Infrastructure/Tools';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { LogitudeWindow } from '../../../../Controls/Windows/LogitudeWindow';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ConfirmWindow } from '../../../../Controls/Windows/ConfirmWindow';
import { ObservableCollection } from '../../../../Infrastructure/Utilities/ObservableCollection';
import { CustomerPM } from '../../../../Common/EntityPMs/CustomerPM';
import { ProductItemPM } from '../../../../Common/EntityPMs/ProductItemPM';

@Component({
    templateUrl: './CustomerProductItemsTabComponent.html',
})

export class CustomerProductItemsTabComponent extends BaseComponent implements OnDestroy {
    public EntityPM: CustomerPM;
    public ObjectTableName: string;
    public TransportModeId: string = null;
    public DataContext = this;
    public IsLCLEntity: boolean = false;
    public IsFCLEntity: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    public ProductItems: ObservableCollection;
    constructor(public entityArgs: EntityArgs) {
        super();
        this.EntityPM = this.entityArgs.EntityPM;
        this.ObjectTableName = this.entityArgs.ObjectTableName;

        this.SetUIProperties();
        this.BuildProductItems();
        this.Listen();
    }

    Listen() {
        if (this.entityArgs.EditComponent) {
            this.SaveCompletedEvent = this.entityArgs.EditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                if (isSaveSuccess) {
                    this.EntityPM = this.entityArgs.EditComponent.EntityPM;

                    this.SetUIProperties();
                }                
            });

            this.LoadCompletedEvent = this.entityArgs.EditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                if (isLoadSuccess) {
                    this.EntityPM = this.entityArgs.EditComponent.EntityPM;

                    this.SetUIProperties();
                }
            });
        }

        this.SessionEvent = this.CurrentSession.SessionEvent.subscribe(s => {
            if (s == "") {
                
            }
        });
    }

    private SessionEvent: any = null;
    private SaveCompletedEvent: any = null;
    private LoadCompletedEvent: any = null;
    ngOnDestroy() {
        AppTool.KillEventEmitter(this.SessionEvent);
        AppTool.KillEventEmitter(this.SaveCompletedEvent);
        AppTool.KillEventEmitter(this.LoadCompletedEvent);
    }

    public IsEditingEnabled: boolean = true;
    SetUIProperties() {

    }

    BuildProductItems() {
        if (this.ProductItems == null) {
            this.ProductItems = new ObservableCollection([]);
        }
        else {
            this.ProductItems.Collection.forEach(item => {
                this.ProductItems.Clear();
            });
        }

        var itemsCollection: ProductItem[] = [];

        //this.EntityPM.ShipmentProductItems.forEach(item => {
        //    itemsCollection.push(new ProductItem(item, this, false));
        //});

        this.ProductItems.InsertCollection(itemsCollection);
    }

    AddProductItem() {
        //var item: ShipmentPackageItemPM = new ShipmentPackageItemPM(null);
        //item.Tenant = SessionLocator.Tenant;
        //item.PackageId = this.EntityPM.Id;
        
        //this.ProductItems.Insert(new ProductItem(item, this, true));
    }
}

export class ProductItem extends BaseComponent {
    //public EntityPM: ShipmentProductItemPM;
    public ObjectTableName: string = "ProductItem";
    public IsNewEntity: boolean = false;
   


    constructor() {
        super();
        //    this.EntityPM = entity;
        //    this.IsNewEntity = isNew;
        
    }
}
