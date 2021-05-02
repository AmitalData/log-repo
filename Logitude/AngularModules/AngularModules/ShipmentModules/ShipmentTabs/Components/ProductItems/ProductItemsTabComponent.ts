import { Component, OnDestroy } from '@angular/core';
import { EntityArgs } from '../../../../Infrastructure/DataContracts/EntityArgs';
import { TextCodeTranslator } from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import { ShipmentPM } from '../../../../Shipment/EntityPMs/ShipmentPM';
import { ApiQueryFilters } from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { AppTool } from '../../../../Infrastructure/Tools';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ConfirmWindow } from '../../../../Controls/Windows/ConfirmWindow';
import { ShipmentTool } from '../../../../Shipment/Tools';
import { ObservableCollection } from '../../../../Infrastructure/Utilities/ObservableCollection';
import { ShipmentProductItemPM } from '../../../../Shipment/EntityPMs/ShipmentProductItemPM';
import { ProductItemPM } from '../../../../Common/EntityPMs/ProductItemPM';
import { Validator } from '../../../../Infrastructure/Validators/Validator';
import { PartnersDomainService } from '../../../../Common/Services/PartnersDomainService';

@Component({
    templateUrl: './ProductItemsTabComponent.html',
})

export class ProductItemsTabComponent extends BaseComponent implements OnDestroy {
    public EntityPM: ShipmentPM;
    public ObjectTableName: string;
    public TransportModeId: string = null;
    public DataContext = this;
    public IsLCLEntity: boolean = false;
    public IsFCLEntity: boolean = false;   
    private CurrentSession = SessionLocator.SelectedSession;
    public ProductItems: ObservableCollection;
    public ProductItemsFilterList: ProductItemPM[];
    constructor(public entityArgs: EntityArgs) {
        super();
        this.EntityPM = this.entityArgs.EntityPM;
        this.ObjectTableName = this.entityArgs.ObjectTableName;
        this.TransportModeId = this.EntityPM.TransportModeId;
        this.IsLCLEntity = AppTool.IsLCLEntity(this.EntityPM.TransportModeId, this.EntityPM.ShipmentTypeId);
        this.IsFCLEntity = AppTool.IsFCLEntity(this.EntityPM.TransportModeId, this.EntityPM.ShipmentTypeId);

        this.LoadConsigneeProductItems();
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

    private LoadConsigneeProductItems() {
        var service: PartnersDomainService = new PartnersDomainService();
        service.GetCustomerProductItems(this.EntityPM.ConsigneeId, this.EntityPM.ToCountryId).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                
            }
        });
    }

    public IsEditingEnabled: boolean = false;
    SetUIProperties() {
        this.IsEditingEnabled = ShipmentTool.IsEditingEnabled(this.EntityPM);
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

        this.EntityPM.ShipmentProductItems.forEach(item => {
            itemsCollection.push(new ProductItem(item, this, false));
        });

        this.ProductItems.InsertCollection(itemsCollection);
    }

    AddProductItem() {
        var item: ShipmentProductItemPM = new ShipmentProductItemPM(null);
        item.Tenant = SessionLocator.Tenant;
        item.ShipmentId = this.EntityPM.Id;
        
        this.ProductItems.Insert(new ProductItem(item, this, true));
    }

    OnRowEnded($event) {
        var errors = [];
        Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);       
        if (($event) == this.ProductItems.Length) {
            this.AddProductItem();            
        }
    }
}

export class ProductItem extends BaseComponent {
    public EntityPM: ShipmentProductItemPM;
    public ObjectTableName: string = "ShipmentProductItem";
    public IsNewEntity: boolean = false;

    constructor(entity: ShipmentProductItemPM, public fatherComponent: ProductItemsTabComponent, isNew: boolean) {
        super();
        this.EntityPM = entity;
        this.IsNewEntity = isNew;
    }

    get ProductItemId() {
        var myResult = null;

        if (this.EntityPM != null) {
            myResult = this.EntityPM.ProductItemId;
        }

        return myResult;
    }
    set ProductItemId(newValue: string) {
        if (this.EntityPM.ProductItemId != newValue) {
            this.EntityPM.ProductItemId = newValue;
        }
    }

    get HTSCode() {
        var myResult = null;

        if (this.EntityPM != null) {
            myResult = this.EntityPM.HTSCode;
        }

        return myResult;
    }
    set HTSCode(newValue: string) {
        if (this.EntityPM.HTSCode != newValue) {
            this.EntityPM.HTSCode = newValue;
        }
    }

    get Description() {
        var myResult = null;

        if (this.EntityPM != null) {
            myResult = this.EntityPM.Description;
        }

        return myResult;
    }
    set Description(newValue: string) {
        if (this.EntityPM.Description != newValue) {
            this.EntityPM.Description = newValue;
        }
    }

    RemoveLine() {
        var confirmWindow = new ConfirmWindow();
        confirmWindow.Show("Delete this item ?");
        confirmWindow.WindowClosed.subscribe((event: any) => {
            if (confirmWindow.Yes) {
                if (this.fatherComponent.EntityPM.ShipmentProductItems.indexOf(this.EntityPM) != -1) {
                    this.fatherComponent.EntityPM.RemoveProductItem(this.EntityPM);
                }

                if (this.fatherComponent.ProductItems.Collection.indexOf(this) != -1) {
                    this.fatherComponent.ProductItems.Remove(this);
                }
            }
        });
    }
}
