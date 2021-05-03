import { Component, OnDestroy, OnInit } from '@angular/core';
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
import { EntityResourceService } from '../../../../Infrastructure/Services/EntityResourceService';
import { HTSCodePM } from '../../../../Common/EntityPMs/HTSCodePM';

@Component({
    templateUrl: './CustomerProductItemsTabComponent.html',
})

export class CustomerProductItemsTabComponent extends BaseComponent implements OnInit, OnDestroy {
    public EntityPM: CustomerPM;
    public ObjectTableName: string;
    public TransportModeId: string = null;
    public DataContext = this;
    public IsLCLEntity: boolean = false;
    public IsFCLEntity: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    public ProductItems: ObservableCollection;
    constructor(public entityArgs: EntityArgs, private entityResourceService: EntityResourceService) {
        super();
        this.EntityPM = this.entityArgs.EntityPM;
        this.ObjectTableName = this.entityArgs.ObjectTableName;
        
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

    ngOnInit() {
        this.entityResourceService.getEntityResourceByTableName(this.ObjectTableName).subscribe((res3: any) => {
            this.SetUIProperties();
            this.BuildProductItems();
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

        var itemsCollection: CustomerProductItem[] = [];

        this.EntityPM.CustomerProductItems.forEach(item => {
            itemsCollection.push(new CustomerProductItem(item, this, false));
        });

        this.ProductItems.InsertCollection(itemsCollection);
    }

    AddProductItem() {
        var productItem: ProductItemPM = new ProductItemPM(this.EntityPM);
        productItem.Tenant = SessionLocator.Tenant;

        var newProductItem: CustomerProductItem = new CustomerProductItem(productItem, this, true);
        this.OpenEditWindow("Add Product Item", newProductItem);
    }

    private OpenEditWindow(myWindowTitle: string, itemPM: CustomerProductItem) {
        var logitudeWindow = new LogitudeWindow();
        logitudeWindow.Title = myWindowTitle;
        logitudeWindow.WindowArgs = { CustomerProductItem: itemPM, CustomerPM: this.EntityPM, };
        logitudeWindow.Show('./CommonModules/CommonCustomer/Components/AddEdit/AddEditCustomerProductItemComponent');
    }
}

export class CustomerProductItem extends BaseComponent {
    public EntityPM: ProductItemPM;
    public ObjectTableName: string = "ProductItem";
    public IsNewEntity: boolean = false;

    constructor(entity: ProductItemPM, public fatherComponent: CustomerProductItemsTabComponent, isNew: boolean) {
        super();
        this.EntityPM = entity;
        this.IsNewEntity = isNew;

        //this.SetUIProperties();
    }

    SetUIProperties() {
        //this.UIProperties.SetRequired("ItemCode", this.ObjectTableName, AppTool.IsNullOrEmpty(this.ItemCode));
    }


    get InActive() { return this.EntityPM.InActive; }
    set InActive(value: boolean) {
        if (this.EntityPM.InActive != value) {
            this.EntityPM.InActive = value;
        }
    }

    get Description() { return this.EntityPM.Description; }
    set Description(value: string) {
        if (this.EntityPM.Description != value) {
            this.EntityPM.Description = value;
        }
    }

    get ItemCode() { return this.EntityPM.ItemCode }
    set ItemCode(value: string) {
        if (this.EntityPM.ItemCode != value) {
            this.EntityPM.ItemCode = value;
        }
    }

    get SKU() { return this.EntityPM.SKU }
    set SKU(value: string) {
        if (this.EntityPM.SKU != value) {
            this.EntityPM.SKU = value;
        }
    }

    get Remarks() { return this.EntityPM.Remarks }
    set Remarks(value: string) {
        if (this.EntityPM.Remarks != value) {
            this.EntityPM.Remarks = value;
        }
    }
}

export class CustomerHTSCode extends BaseComponent {
    public EntityPM: HTSCodePM;
    public ObjectTableName: string = "HTSCode";
    public IsNewEntity: boolean = false;

    constructor(entity: HTSCodePM, public fatherComponent: CustomerProductItem, isNew: boolean) {
        super();
        this.EntityPM = entity;
        this.IsNewEntity = isNew;

        //this.SetUIProperties();
    }

    SetUIProperties() {
        //this.UIProperties.SetRequired("ItemCode", this.ObjectTableName, AppTool.IsNullOrEmpty(this.ItemCode));
    }


    
}
