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
import { ApiQueryFilters } from '../../../../Infrastructure/DataContracts/ApiQueryFilters';

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
    public ProductItemQueryFilters: ApiQueryFilters;
    public IsUsingVirtuallization: boolean = false;
    constructor(public entityArgs: EntityArgs, private entityResourceService: EntityResourceService) {
        super();
        this.EntityPM = this.entityArgs.EntityPM;
        this.ObjectTableName = this.entityArgs.ObjectTableName;
        this.PartnersDomainService = new PartnersDomainService();
        this.SetIsUsingVirtuallization();
        this.ProductItems = new ObservableCollection([]);
        this.ProductItemQueryFilters = new ApiQueryFilters();
        this.Listen();
    }

    ngOnInit() {
        if (this.EntityPM != null) {
            this.entityResourceService.getEntityResourceByTableName("ProductItem").subscribe((res1: any) => {
                this.entityResourceService.getEntityResourceByTableName("HTSCode").subscribe((res2: any) => {
                    this.entityResourceService.getEntityResourceByTableName("ShipmentProductItem").subscribe((res3: any) => {
                        this.IsLCLEntity = AppTool.IsLCLEntity(this.EntityPM.TransportModeId, this.EntityPM.ShipmentTypeId);
                        this.IsFCLEntity = AppTool.IsFCLEntity(this.EntityPM.TransportModeId, this.EntityPM.ShipmentTypeId);
                        this.TransportModeId = this.EntityPM.TransportModeId;
                        this.CustomerId = this.EntityPM.CustomerId;
                        this.ToCountryId = this.EntityPM.ToCountryId;

                        this.IsResourcesReady = true;
                        this.SetUIProperties();
                        this.BuildProductItems();
                        this.BuildQueryFilters();                        
                    });
                });
            });
        }
    }

    SetIsUsingVirtuallization() {
        var hasGridVirtuallizationToggleFeature = SessionLocator.FeatureToggles.filter(d => d.ToggleCode == "EVG")[0]
        if (hasGridVirtuallizationToggleFeature) {
            this.IsUsingVirtuallization = true;
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
        this.AddEmptyProductItemLine();
    }

    private AddEmptyProductItemLine() {
        if (this.ProductItems.Length == 0) {
            var item: ShipmentProductItemPM = new ShipmentProductItemPM(null);
            item.Tenant = SessionLocator.Tenant;
            item.ShipmentId = this.EntityPM.Id;
            item.IsEmptyLine = true;

            this.ProductItems.Insert(new ProductItem(item, this, true));
        }
    }

    public BuildQueryFilters() {
        this.ProductItemQueryFilters = new ApiQueryFilters();
        var addedProductItemsIds: string = null;

        this.ProductItems.Collection.forEach(item => {
            if (AppTool.IsNullOrEmpty(addedProductItemsIds)) {
                addedProductItemsIds = item.ProductItemId;
            }

            else {
                addedProductItemsIds = addedProductItemsIds + "," + item.ProductItemId;
            }
        });

        if (!AppTool.IsNullOrEmpty(addedProductItemsIds)) {
            this.ProductItemQueryFilters.addAdditionalFilter("Id", addedProductItemsIds, null, null, "Exclude", false, false, false, "string", false, true, true);
        }
    }

    AddProductItem() {
        this.CurrentSession.CurrentEditComponent.ValidationErrorsList = [];
        this.ValidateHTSCodes();

        if (this.CurrentSession.CurrentEditComponent.ValidationErrorsList.length == 0) {
            var item: ShipmentProductItemPM = new ShipmentProductItemPM(null);
            item.Tenant = SessionLocator.Tenant;
            item.ShipmentId = this.EntityPM.Id;
            item.IsEmptyLine = true;

            this.ProductItems.Insert(new ProductItem(item, this, true));        
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
                                logitudeWindow.Height = 650;
                                logitudeWindow.Width = 1000; 
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
            this.MapProductItem(shipmentProductItem, customerItem);
            this.UpdateSimilarProductItems(shipmentProductItem, customerItem);
            this.BuildProductItems();
        }
    }
    private UpdateSimilarProductItems(shipmentItem: ShipmentProductItemPM, customerItem: ProductItemPM) {
        var sameProductItems: ShipmentProductItemPM[] = this.EntityPM.ShipmentProductItems.filter(d => d.ProductItemId == shipmentItem.ProductItemId);
        if (sameProductItems != null && sameProductItems.length > 0) {
            sameProductItems.forEach(item => {
                this.MapProductItem(item, customerItem);
            });
        }
    }
    MapProductItem(shipmentProductItem: ShipmentProductItemPM, customerItem: ProductItemPM) {
        shipmentProductItem.Description = customerItem.Description;
        shipmentProductItem.SKU = customerItem.SKU;
        shipmentProductItem.Name = customerItem.Name;
        shipmentProductItem.Brand = customerItem.Brand;
        shipmentProductItem.ASIN = customerItem.ASIN;
        shipmentProductItem.UPC = customerItem.UPC;
        shipmentProductItem.OriginCountryId = customerItem.OriginCountryId;
        shipmentProductItem.OriginCountryName = customerItem.OriginCountryName;
        shipmentProductItem.ShipperId = customerItem.ShipperId;
        shipmentProductItem.ShipperName = customerItem.ShipperName;

        var htsCode: HTSCodePM = customerItem.HTSCodes.filter(d => d.DestinationCountryId == this.EntityPM.ToCountryId && !d.InActive)[0];
        if (htsCode) {
            shipmentProductItem.HTSCode = htsCode.Code;
            shipmentProductItem.ApprovedByCustomer = htsCode.ApprovedByCustomer;
            shipmentProductItem.VATPercentage = htsCode.VATPercentage;
            shipmentProductItem.DutiesPercentage = htsCode.DutiesPercentage;
            shipmentProductItem.OtherDuties = htsCode.OtherDuties;
            shipmentProductItem.Remarks = htsCode.Remarks;
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

            if (!AppTool.IsNullOrEmpty(newValue)) {
                this.EntityPM.IsEmptyLine = false;
                this.fatherComponent.EntityPM.AddProductItem(this.EntityPM);
            }

            else {
                this.EntityPM.IsEmptyLine = true;
                if (this.fatherComponent.EntityPM.ShipmentProductItems.indexOf(this.EntityPM) != -1) {
                    this.fatherComponent.EntityPM.RemoveProductItem(this.EntityPM);
                }
            }

            this.GetCustomerProductItemHTSCode();
        }
    }

    private GetCustomerProductItemHTSCode() {
        if (AppTool.IsNullOrEmpty(this.ProductItemId)) {
            this.HTSCode = null;
            this.ApprovedByCustomer = false;
            this.VATPercentage = null;
            this.DutiesPercentage = null;
            this.OtherDuties = null;
            this.Remarks = null;
        }

        else {
            this.fatherComponent.PartnersDomainService.GetCustomerProductItemHTSCodeByCountry(this.ProductItemId, this.fatherComponent.ToCountryId).subscribe((myResponse: ServiceResponse) => {
                if (!myResponse.HasError) {
                    var htsCode: HTSCodePM = myResponse.Result;
                    if (htsCode) {
                        this.HTSCode = htsCode.Code;
                        this.ApprovedByCustomer = htsCode.ApprovedByCustomer;
                        this.VATPercentage = htsCode.VATPercentage;
                        this.DutiesPercentage = htsCode.DutiesPercentage;
                        this.OtherDuties = htsCode.OtherDuties;
                        this.Remarks = htsCode.Remarks;
                    }

                    else {
                        this.HTSCode = null;
                        this.ApprovedByCustomer = false;
                        this.VATPercentage = null;
                        this.DutiesPercentage = null;
                        this.OtherDuties = null;
                        this.Remarks = null;
                    }
                }
            });
        }
    }

    private productItem: ProductItemList;
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
            this.ShipperId = value.ShipperId;
            this.ShipperName = value.ShipperName;

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
            this.ShipperId = null;
            this.ShipperName = null;
        }

        this.fatherComponent.BuildQueryFilters();
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

    get ShipperId() { return this.EntityPM.ShipperId; }
    set ShipperId(newValue: string) {
        if (this.EntityPM.ShipperId != newValue) {
            this.EntityPM.ShipperId = newValue;
        }
    }

    get ShipperName() { return this.EntityPM.ShipperName; }
    set ShipperName(newValue: string) {
        if (this.EntityPM.ShipperName != newValue) {
            this.EntityPM.ShipperName = newValue;
        }
    }

    get VATPercentage() { return this.EntityPM.VATPercentage; }
    set VATPercentage(newValue: number) {
        if (this.EntityPM.VATPercentage != newValue) {
            this.EntityPM.VATPercentage = AppTool.Round(newValue, 1);
        }
    }

    get DutiesPercentage() { return this.EntityPM.DutiesPercentage; }
    set DutiesPercentage(newValue: number) {
        if (this.EntityPM.DutiesPercentage != newValue) {
            this.EntityPM.DutiesPercentage = AppTool.Round(newValue, 1);
        }
    }

    get OtherDuties() { return this.EntityPM.OtherDuties; }
    set OtherDuties(newValue: string) {
        if (this.EntityPM.OtherDuties != newValue) {
            this.EntityPM.OtherDuties = newValue;
        }
    }

    get Remarks() { return this.EntityPM.Remarks; }
    set Remarks(newValue: string) {
        if (this.EntityPM.Remarks != newValue) {
            this.EntityPM.Remarks = newValue;
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
                    this.fatherComponent.BuildQueryFilters();
                }
            }
        });
    }
}
