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
import { ProductItemList } from '../../../../Common/EntityLists/ProductItemList';
import { LogitudeWindow } from '../../../../Controls/Windows/LogitudeWindow';
import { EntityResourceService } from '../../../../Infrastructure/Services/EntityResourceService';
import { HTSCodePM } from '../../../../Common/EntityPMs/HTSCodePM';

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
    public PartnersDomainService: PartnersDomainService;
    public CustomerId: string;
    constructor(public entityArgs: EntityArgs) {
        super();
        this.EntityPM = this.entityArgs.EntityPM;
        this.ObjectTableName = this.entityArgs.ObjectTableName;
        this.TransportModeId = this.EntityPM.TransportModeId;
        this.IsLCLEntity = AppTool.IsLCLEntity(this.EntityPM.TransportModeId, this.EntityPM.ShipmentTypeId);
        this.IsFCLEntity = AppTool.IsFCLEntity(this.EntityPM.TransportModeId, this.EntityPM.ShipmentTypeId);
        this.CustomerId = this.EntityPM.CustomerId;

        this.PartnersDomainService = new PartnersDomainService();
        this.ProductItems = new ObservableCollection([]);

        this.SetUIProperties();
        this.BuildProductItems();    
        this.Listen();
    }

    Listen() {
        if (this.entityArgs.EditComponent) {
            this.SaveCompletedEvent = this.entityArgs.EditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                if (isSaveSuccess) {
                    this.EntityPM = this.entityArgs.EditComponent.EntityPM;
                    this.CustomerId = this.EntityPM.CustomerId;
                }                
            });

            this.LoadCompletedEvent = this.entityArgs.EditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                if (isLoadSuccess) {
                    this.EntityPM = this.entityArgs.EditComponent.EntityPM;
                    this.CustomerId = this.EntityPM.CustomerId;
                }
            });
        }

        this.SessionEvent = this.CurrentSession.SessionEvent.subscribe(s => {
            if (s == "ShipmentPartnersChanged") {
                this.CustomerId = this.EntityPM.CustomerId;
                this.BuildProductItems();
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
        this.EntityPM.AddProductItem(item);
    }
    EditCustomerProductItem(item: ProductItem) {
        var entityResourceService: EntityResourceService = new EntityResourceService();
        entityResourceService.getEntityResourceByTableName("ProductItem").subscribe((res1: any) => {
            entityResourceService.getEntityResourceByTableName("HTSCode").subscribe((res2: any) => {
                if (!AppTool.IsNullOrEmpty(item.ProductItemId)) {
                    this.PartnersDomainService.GetSingleCustomerProductItem(item.ProductItemId).subscribe((myResponse: ServiceResponse) => {
                        if (!myResponse.HasError) {
                            var customerProductItem: ProductItemPM = myResponse.Result;
                            if (customerProductItem) {
                                var logitudeWindow = new LogitudeWindow();
                                logitudeWindow.Title = "Edit Customer Product Item";
                                logitudeWindow.WindowArgs = { EntityPM: customerProductItem, ShipmentPM: this.EntityPM };
                                logitudeWindow.Show('./ShipmentModules/ShipmentTabs/Components/ProductItems/EditCustomerProductItemComponent');
                                logitudeWindow.ComponentLoaded.subscribe(comp => {
                                    logitudeWindow.WindowClosed.subscribe(s => {
                                        if (s) {
                                            this.UpdateShipmentProductItem(item, comp.EntityPM);
                                        }
                                    });
                                });
                            }
                        }
                    });
                }
            });
        });
    }
    private UpdateShipmentProductItem(shipmentItem: ProductItem, customerItem: ProductItemPM) {
        var shipmentProductItem: ShipmentProductItemPM = this.EntityPM.ShipmentProductItems.filter(d => d.Id == shipmentItem.EntityPM.Id)[0];
        if (shipmentProductItem) {
            this.MapProductItems(shipmentProductItem, customerItem);
            this.BuildProductItems();
        }
    }
    MapProductItems(shipmentProductItem: ShipmentProductItemPM, customerItem: ProductItemPM) {
        shipmentProductItem.Description = customerItem.Description;        
        shipmentProductItem.SKU = customerItem.SKU;

        var htsCode: HTSCodePM = customerItem.HTSCodes.filter(d => d.DestinationCountryId == this.EntityPM.ToCountryId)[0];
        if (htsCode) {
            shipmentProductItem.HTSCode = htsCode.Code;
            shipmentProductItem.ApprovedByCustomer = htsCode.ApprovedByCustomer;
        }
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

    get ProductItemId() { return this.EntityPM.ProductItemId; }
    set ProductItemId(newValue: string) {
        if (this.EntityPM.ProductItemId != newValue) {
            this.EntityPM.ProductItemId = newValue;

            this.GetCustomerProductItemHTSCode();          
        }
    }

    private GetCustomerProductItemHTSCode() {
        if (AppTool.IsNullOrEmpty(this.ProductItemId)) {
            this.HTSCode = null;
        }

        else {
            this.fatherComponent.PartnersDomainService.GetCustomerProductItemHTSCode(this.ProductItemId, this.fatherComponent.EntityPM.ToCountryId).subscribe((myResponse: ServiceResponse) => {
                if (!myResponse.HasError) {
                    this.HTSCode = myResponse.Result;
                }
            });
        }
    }

    productItem: ProductItemList;
    get ProductItem() { return this.productItem; }
    set ProductItem(value: ProductItemList) {
        if (this.productItem != value) {
            this.productItem = value;
        }

        if (!AppTool.IsNullOrEmpty(value)) {
            this.SKU = value.SKU;
            this.Description = value.Description;            
        }

        else {
            this.SKU = null;
            this.Description = null; 
        }
    }

    get HTSCode() {return this.EntityPM.HTSCode;}
    set HTSCode(newValue: string) {
        if (this.EntityPM.HTSCode != newValue) {
            this.EntityPM.HTSCode = newValue;
        }
    }

    get Description() { return this.EntityPM.Description; }
    set Description(newValue: string) {
        if (this.EntityPM.Description != newValue) {
            this.EntityPM.Description = newValue;
        }
    }

    get SKU() {return this.EntityPM.SKU; }
    set SKU(newValue: string) {
        if (this.EntityPM.SKU != newValue) {
            this.EntityPM.SKU = newValue;
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
