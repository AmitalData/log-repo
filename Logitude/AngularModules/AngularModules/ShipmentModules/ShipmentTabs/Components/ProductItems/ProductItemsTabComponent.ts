import { Component, OnInit, OnDestroy } from '@angular/core';
import { EntityArgs } from '../../../../Infrastructure/DataContracts/EntityArgs';
import { ShipmentPM } from '../../../../Shipment/EntityPMs/ShipmentPM';
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

export class ProductItemsTabComponent extends BaseComponent implements OnInit, OnDestroy {
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
    public ToCountryId: string;
    public IsResourcesReady: boolean = false;
    constructor(public entityArgs: EntityArgs, private entityResourceService: EntityResourceService) {
        super();
        this.EntityPM = this.entityArgs.EntityPM;
        this.ObjectTableName = this.entityArgs.ObjectTableName;
        this.PartnersDomainService = new PartnersDomainService();
        this.ProductItems = new ObservableCollection([]);

        this.Listen();
    }

    ngOnInit() {
        if (this.EntityPM != null) {
            this.entityResourceService.getEntityResourceByTableName("ShipmentProductItem").subscribe((res1: any) => {
                this.IsLCLEntity = AppTool.IsLCLEntity(this.EntityPM.TransportModeId, this.EntityPM.ShipmentTypeId);
                this.IsFCLEntity = AppTool.IsFCLEntity(this.EntityPM.TransportModeId, this.EntityPM.ShipmentTypeId);
                this.TransportModeId = this.EntityPM.TransportModeId;
                this.CustomerId = this.EntityPM.CustomerId;
                this.ToCountryId = this.EntityPM.ToCountryId;

                this.IsResourcesReady = true;
                this.SetUIProperties();
                this.BuildProductItems();
            });
        }
    }

    Listen() {
        if (this.entityArgs.EditComponent) {
            this.SaveCompletedEvent = this.entityArgs.EditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                if (isSaveSuccess) {
                    this.EntityPM = this.entityArgs.EditComponent.EntityPM;
                    this.CustomerId = this.EntityPM.CustomerId;
                    this.ToCountryId = this.EntityPM.ToCountryId;
                    this.BuildProductItems();
                }
            });

            this.LoadCompletedEvent = this.entityArgs.EditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                if (isLoadSuccess) {
                    this.EntityPM = this.entityArgs.EditComponent.EntityPM;
                    this.CustomerId = this.EntityPM.CustomerId;
                    this.ToCountryId = this.EntityPM.ToCountryId;
                    this.BuildProductItems();
                }
            });
        }

        this.SessionEvent = this.CurrentSession.SessionEvent.subscribe(s => {
            if (s == "ShipmentPartnersChanged" || s == "ShipmentCustomerChanged") {
                this.CustomerId = this.EntityPM.CustomerId;
                this.BuildProductItems();
            }

            else if (s == "ShipmentProductItemsUpdated") {
                this.ToCountryId = this.EntityPM.ToCountryId;
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
        this.CurrentSession.CurrentEditComponent.ValidationErrorsList = [];
        this.ValidateHTSCodes();

        if (this.CurrentSession.CurrentEditComponent.ValidationErrorsList.length == 0) {
            var item: ShipmentProductItemPM = new ShipmentProductItemPM(null);
            item.Tenant = SessionLocator.Tenant;
            item.ShipmentId = this.EntityPM.Id;

            this.ProductItems.Insert(new ProductItem(item, this, true));
            this.EntityPM.AddProductItem(item);
        }
    }
    EditCustomerProductItem(item: ProductItem) {
        this.entityResourceService.getEntityResourceByTableName("ProductItem").subscribe((res1: any) => {
            this.entityResourceService.getEntityResourceByTableName("HTSCode").subscribe((res2: any) => {
                if (!AppTool.IsNullOrEmpty(item.ProductItemId)) {
                    this.PartnersDomainService.GetSingleCustomerProductItem(item.ProductItemId).subscribe((myResponse: ServiceResponse) => {
                        if (!myResponse.HasError) {
                            var customerProductItem: ProductItemPM = myResponse.Result;
                            if (customerProductItem) {
                                var logitudeWindow = new LogitudeWindow();
                                logitudeWindow.Title = "Edit Customer Product Item";
                                logitudeWindow.Height = 550;
                                logitudeWindow.Width = 800; 
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
        shipmentProductItem.Name = customerItem.Name;
        shipmentProductItem.Brand = customerItem.Brand;
        shipmentProductItem.ASIN = customerItem.ASIN;
        shipmentProductItem.UPC = customerItem.UPC;
        shipmentProductItem.OriginCountryId = customerItem.OriginCountryId;
        shipmentProductItem.OriginCountryName = customerItem.OriginCountryName;

        var htsCode: HTSCodePM = customerItem.HTSCodes.filter(d => d.DestinationCountryId == this.EntityPM.ToCountryId && !d.InActive)[0];
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

    private ValidateHTSCodes() {
        if (this.ProductItems != null) {
            this.ProductItems.Collection.forEach(item => {
                Validator.TryValidateObject(item, "ShipmentProductItem", this.CurrentSession.CurrentEditComponent.ValidationErrorsList);
            });
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
            this.ApprovedByCustomer = false;
        }

        else {
            this.fatherComponent.PartnersDomainService.GetCustomerProductItemHTSCodeByCountry(this.ProductItemId, this.fatherComponent.ToCountryId).subscribe((myResponse: ServiceResponse) => {
                if (!myResponse.HasError) {
                    var htsCode: HTSCodePM = myResponse.Result;
                    if (htsCode) {
                        this.HTSCode = htsCode.Code;
                        this.ApprovedByCustomer = htsCode.ApprovedByCustomer;
                    }

                    else {
                        this.HTSCode = null;
                        this.ApprovedByCustomer = false;
                    }
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
            this.Brand = value.Brand
            this.Name = value.Name
            this.ASIN = value.ASIN;
            this.UPC = value.UPC;
            this.OriginCountryId = value.OriginCountryId;
            this.OriginCountryName = value.OriginCountryName;
        }

        else {
            this.SKU = null;
            this.Description = null;
            this.Brand = null;
            this.Name = null;
            this.ASIN = null;
            this.UPC = null;
            this.OriginCountryId = null;
            this.OriginCountryName = null;
        }
    }

    get HTSCode() {return this.EntityPM.HTSCode;}
    set HTSCode(newValue: string) {
        if (this.EntityPM.HTSCode != newValue) {
            this.EntityPM.HTSCode = newValue;
        }
    }

    get ApprovedByCustomer() { return this.EntityPM.ApprovedByCustomer; }
    set ApprovedByCustomer(newValue: boolean) {
        if (this.EntityPM.ApprovedByCustomer != newValue) {
            this.EntityPM.ApprovedByCustomer = newValue;
        }
    }

    get Description() { return this.EntityPM.Description; }
    set Description(newValue: string) {
        if (this.EntityPM.Description != newValue) {
            this.EntityPM.Description = newValue;
        }
    }

    get Brand() { return this.EntityPM.Brand; }
    set Brand(newValue: string) {
        if (this.EntityPM.Brand != newValue) {
            this.EntityPM.Brand = newValue;
        }
    }

    get SKU() {return this.EntityPM.SKU; }
    set SKU(newValue: string) {
        if (this.EntityPM.SKU != newValue) {
            this.EntityPM.SKU = newValue;
        }
    }

    get Name() { return this.EntityPM.Name; }
    set Name(newValue: string) {
        if (this.EntityPM.Name != newValue) {
            this.EntityPM.Name = newValue;
        }
    }

    get ASIN() { return this.EntityPM.ASIN; }
    set ASIN(newValue: string) {
        if (this.EntityPM.ASIN != newValue) {
            this.EntityPM.ASIN = newValue;
        }
    }

    get UPC() { return this.EntityPM.UPC; }
    set UPC(newValue: string) {
        if (this.EntityPM.UPC != newValue) {
            this.EntityPM.UPC = newValue;
        }
    }

    get OriginCountryId() { return this.EntityPM.OriginCountryId; }
    set OriginCountryId(newValue: string) {
        if (this.EntityPM.OriginCountryId != newValue) {
            this.EntityPM.OriginCountryId = newValue;
        }
    }

    get OriginCountryName() { return this.EntityPM.OriginCountryName; }
    set OriginCountryName(newValue: string) {
        if (this.EntityPM.OriginCountryName != newValue) {
            this.EntityPM.OriginCountryName = newValue;
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
