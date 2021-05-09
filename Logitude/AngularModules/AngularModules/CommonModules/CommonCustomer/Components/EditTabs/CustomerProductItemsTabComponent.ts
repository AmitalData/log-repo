import { Component, OnDestroy, OnInit } from '@angular/core';
import { EntityArgs } from '../../../../Infrastructure/DataContracts/EntityArgs';
import { AppTool } from '../../../../Infrastructure/Tools';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { LogitudeWindow } from '../../../../Controls/Windows/LogitudeWindow';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ObservableCollection } from '../../../../Infrastructure/Utilities/ObservableCollection';
import { CustomerPM } from '../../../../Common/EntityPMs/CustomerPM';
import { ProductItemPM } from '../../../../Common/EntityPMs/ProductItemPM';
import { EntityResourceService } from '../../../../Infrastructure/Services/EntityResourceService';
import { HTSCodePM } from '../../../../Common/EntityPMs/HTSCodePM';
import { Validator } from '../../../../Infrastructure/Validators/Validator';

@Component({
    templateUrl: './CustomerProductItemsTabComponent.html',
})

export class CustomerProductItemsTabComponent extends BaseComponent implements OnInit, OnDestroy {
    public EntityPM: CustomerPM;
    public ProductItemEntityPM: ProductItemPM;
    public ObjectTableName: string = "ProductItem";
    public DataContext = this;
    public ProductItems: ObservableCollection;
    public HTSCodes: ObservableCollection;
    public IsEditingEnabled: boolean = true;

    constructor(public entityArgs: EntityArgs, private entityResourceService: EntityResourceService) {
        super();
        this.EntityPM = this.entityArgs.EntityPM;
        this.HTSCodes = new ObservableCollection([]);
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

    BuildProductItemHTSCodes(item:ProductItemPM) {
        if (this.HTSCodes == null) {
            this.HTSCodes = new ObservableCollection([]);
        }
        else {
            this.HTSCodes.Collection.forEach(item => {
                this.HTSCodes.Clear();
            });
        }

        var itemsCollection: CustomerHTSCode[] = [];

        item.HTSCodes.forEach(item => {
            itemsCollection.push(new CustomerHTSCode(item, this, false));
        });
        this.HTSCodes.InsertCollection(itemsCollection);
    }

    AddProductItem() {
        var productItem: ProductItemPM = new ProductItemPM(this.EntityPM);
        productItem.Tenant = SessionLocator.Tenant;

        var htsCode: HTSCodePM = new HTSCodePM(this.EntityPM);
        htsCode.Tenant = SessionLocator.Tenant;

        var newProductItem: CustomerProductItem = new CustomerProductItem(productItem, this, true);
        var newHTSCode: CustomerHTSCode = new CustomerHTSCode(htsCode, this, true);

        this.ProductItemEntityPM = productItem;
        this.OpenEditWindow("Add Product Item", newProductItem, newHTSCode);
    }

    private OpenEditWindow(myWindowTitle: string, itemPM: CustomerProductItem, htsCodePM: CustomerHTSCode) {
        var logitudeWindow = new LogitudeWindow();
        logitudeWindow.Title = myWindowTitle;
        logitudeWindow.WindowArgs = { CustomerProductItem: itemPM, CustomerPM: this.EntityPM, CustomerHTSCode: htsCodePM, CustomerProductItemsTabComponent:this};
        logitudeWindow.Show('./CommonModules/CommonCustomer/Components/AddEdit/AddEditCustomerProductItemComponent');
    }


    AddHTSCode() {
        var hTSCodeItem: HTSCodePM = new HTSCodePM(null);
        this.HTSCodes.Insert(new CustomerHTSCode(hTSCodeItem, this, true));
        hTSCodeItem.Tenant = SessionLocator.Tenant;
        hTSCodeItem.ItemId = this.EntityPM.Id;
    }

    OnRowEnded($event) {
        var errors = [];
        Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        if (($event) == this.HTSCodes.Length) {
            this.AddHTSCode();
        }
    }

    GetIndexOfHTSCode(hTSCode: CustomerHTSCode) {
        return this.HTSCodes.GetIndex(hTSCode)+1;
    } 

    EditLineClicked(item: CustomerProductItem) {       
        this.ProductItemEntityPM = item.EntityPM;
        var htsCode: HTSCodePM = new HTSCodePM(item);
        var currentHTSCode: CustomerHTSCode = new CustomerHTSCode(htsCode, this, true);
        this.BuildProductItemHTSCodes(item.EntityPM);
        this.OpenEditWindow("Edit Product Item", item, currentHTSCode);
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
    }

    SetUIProperties() {

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

    constructor(entity: HTSCodePM, public fatherComponent: CustomerProductItemsTabComponent, isNew: boolean) {
        super();
        this.EntityPM = entity;
        this.IsNewEntity = isNew;
    }

    SetUIProperties() {

    }

    private selectedProductItemHTSCode: HTSCodePM;
    get SelectedProductItem() { return this.selectedProductItemHTSCode; }
    set SelectedProductItem(value: HTSCodePM) {
        if (this.selectedProductItemHTSCode != value) {
            this.selectedProductItemHTSCode = value;

            if (value) {
                this.HTSCodeId = value.Id;
                this.ProductItemId = value.ItemId;
                this.DestinationCountryId = value.DestinationCountryId;
                this.HTSCode = value.Code;
                this.ApprovedByCustomer = value.ApprovedByCustomer;
            }

            else {
                this.ProductItemId = null;
                this.DestinationCountryId = null;
                this.HTSCode = null;
                this.ApprovedByCustomer = null;
            }
        }
    }

    get HTSCodeId() {
        var myResult = null;

        if (this.EntityPM != null) {
            myResult = this.EntityPM.Id;
        }

        return myResult;
    }
    set HTSCodeId(newValue: string) {
        if (this.EntityPM.Id != newValue) {
            this.EntityPM.Id = newValue;
        }
    }

    get ProductItemId() {
        var myResult = null;

        if (this.EntityPM != null) {
            myResult = this.EntityPM.ItemId;
        }

        return myResult;
    }
    set ProductItemId(newValue: string) {
        if (this.EntityPM.ItemId != newValue) {
            this.EntityPM.ItemId = newValue;
        }
    }

    get HTSCode() {
        var myResult = null;

        if (this.EntityPM != null) {
            myResult = this.EntityPM.Code;
        }

        return myResult;
    }
    set HTSCode(newValue: string) {
        if (this.EntityPM.Code != newValue) {
            this.EntityPM.Code = newValue;
        }
    }

    get DestinationCountryId() {
        var myResult = null;

        if (this.EntityPM != null) {
            myResult = this.EntityPM.DestinationCountryId;
        }

        return myResult;
    }
    set DestinationCountryId(newValue: string) {
        if (this.EntityPM.DestinationCountryId != newValue) {
            this.EntityPM.DestinationCountryId = newValue;
        }
    }

    get ApprovedByCustomer() {
        var myResult = null;

        if (this.EntityPM != null) {
            myResult = this.EntityPM.ApprovedByCustomer;
        }

        return myResult;
    }
    set ApprovedByCustomer(newValue: boolean) {
        if (this.EntityPM.ApprovedByCustomer != newValue) {
            this.EntityPM.ApprovedByCustomer = newValue;
        }
    }
}
