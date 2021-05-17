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
import { CountryList } from '../../../../Common/EntityLists/CountryList';

@Component({
    templateUrl: './CustomerProductItemsTabComponent.html',
})

export class CustomerProductItemsTabComponent extends BaseComponent implements OnInit, OnDestroy {
    public EntityPM: CustomerPM;
    public ObjectTableName: string = "ProductItem";
    public DataContext = this;
    public ProductItems: ObservableCollection;    
    public ValidationErrorsList: string[];
    public IsEditingEnabled: boolean = true;

    constructor(public entityArgs: EntityArgs, private entityResourceService: EntityResourceService) {
        super();
        this.EntityPM = this.entityArgs.EntityPM;        
        this.ProductItems = new ObservableCollection([]);
    }

    ngOnInit() {
        this.entityResourceService.getEntityResourceByTableName(this.ObjectTableName).subscribe((res1: any) => {
            this.entityResourceService.getEntityResourceByTableName("HTSCode").subscribe((res2: any) => {
                this.SetUIProperties();
                this.BuildProductItems();
            });
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
        var itemsCollection: CustomerProductItem[] = [];

        this.EntityPM.CustomerProductItems.forEach((item) => {
            itemsCollection.push(new CustomerProductItem(item, this, false));
        })

        this.ProductItems.InsertCollection(itemsCollection);        
    }  

    AddProductItem() {
        var productItem: ProductItemPM = new ProductItemPM(null);
        productItem.Tenant = SessionLocator.Tenant;
        productItem.CustomerId = this.EntityPM.Id;
        var itemComponent: CustomerProductItem = new CustomerProductItem(productItem, this, true);
        this.OpenEditWindow("Add Product Item", itemComponent);
    }

    EditLineClicked(itemComponent: CustomerProductItem) {
        itemComponent.CopyHTSCodes();
        itemComponent.BuildHTSCodes();
        this.OpenEditWindow("Edit Product Item", itemComponent);
    }
    private OpenEditWindow(myWindowTitle: string, itemPM: CustomerProductItem) {
        var logitudeWindow = new LogitudeWindow();
        logitudeWindow.Title = myWindowTitle;
        logitudeWindow.DataContext = itemPM;
        logitudeWindow.Show('./CommonModules/CommonCustomer/Components/AddEdit/AddEditCustomerProductItemComponent');
        logitudeWindow.WindowClosed.subscribe(($event: any) => this.BuildProductItems());
    }
}

export class CustomerProductItem extends BaseComponent {
    public EntityPM: ProductItemPM;
    public ObjectTableName: string = "ProductItem";
    public IsNewEntity: boolean = false;
    public CustomerPM: CustomerPM;
    public HTSCodes: ObservableCollection;
    constructor(entity: ProductItemPM, public FatherComponent: CustomerProductItemsTabComponent, isNew: boolean) {
        super();
        this.EntityPM = entity;
        this.IsNewEntity = isNew;
        this.CustomerPM = this.FatherComponent.EntityPM;
        this.HTSCodes = new ObservableCollection([]);
        this.SetUIProperties();
        this.BuildHTSCodes();
    }

    BuildHTSCodes() {
        if (this.HTSCodes == null) {
            this.HTSCodes = new ObservableCollection([]);
        }
        else {
            this.HTSCodes.Collection.forEach(item => {
                this.HTSCodes.Clear();
            });
        }

        var itemsCollection: CustomerHTSCode[] = [];

        this.EntityPM.HTSCodes.forEach(item => {
            itemsCollection.push(new CustomerHTSCode(item, this, false));
        });

        this.HTSCodes.InsertCollection(itemsCollection);
    }

    public savedItems: HTSCodePM[] = [];
    public CopyHTSCodes() {
        this.savedItems = [];
        if (this.EntityPM.HTSCodes.length > 0) {            
            this.EntityPM.HTSCodes.forEach(item => {                
                var htsCode = new HTSCodePM(null);
                htsCode.ApprovedByCustomer = item.ApprovedByCustomer;
                htsCode.Tenant = item.Tenant;
                htsCode.Code = item.Code;
                htsCode.CountryEnglishName = item.CountryEnglishName;
                htsCode.DestinationCountryId = item.DestinationCountryId;
                htsCode.InActive = item.InActive;
                htsCode.ItemId = item.ItemId;
                this.savedItems.push(htsCode);
            });
        }
    }

    public ResetHTSCodes() {
        if (this.savedItems != null) {
            var items: HTSCodePM[] = this.EntityPM.HTSCodes;
            items.forEach(item => {
                var savedItem: HTSCodePM = this.savedItems.filter(d => d.Code == item.Code)[0];
                if (savedItem == null) {
                    if (this.EntityPM.HTSCodes.indexOf(item) != -1) {
                        this.EntityPM.RemoveHTSCodePM(item);
                    }
                }

                else {
                    item.ApprovedByCustomer = savedItem.ApprovedByCustomer;
                    item.Code = savedItem.Code;
                    item.CountryEnglishName = savedItem.CountryEnglishName;
                    item.DestinationCountryId = savedItem.DestinationCountryId;
                    item.InActive = savedItem.InActive;
                }
            });

            this.savedItems.forEach(item => {
                var list = this.EntityPM.HTSCodes.filter(d => d.Code == item.Code);
                if (list == null) {
                    this.EntityPM.HTSCodes.push(item);
                }
            });
        }
    }

    SetUIProperties() {
        if (this.IsNewEntity) {
            this.UIProperties.SetVisibility("InActive", this.ObjectTableName, false);
        }

        else {
            this.UIProperties.SetVisibility("InActive", this.ObjectTableName, true);
        }
    }

    get Id() { return this.EntityPM.Id; }

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

    AddHTSCode() {
        //this.ValidationErrorsList = this.ValidiateLastHTSCode();

        //if (this.ValidationErrorsList.length == 0) {
            var hTSCodeItem: HTSCodePM = new HTSCodePM(null);
            hTSCodeItem.Tenant = SessionLocator.Tenant;
            hTSCodeItem.ItemId = this.EntityPM.Id;
            this.HTSCodes.Insert(new CustomerHTSCode(hTSCodeItem, this, true));
        //}
    }

    OnRowEnded($event) {
        var errors = [];
        Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);

        if (($event) == this.HTSCodes.Length) {
            this.AddHTSCode();
        }
    }

    GetIndexOfHTSCode(hTSCode: CustomerHTSCode) {
        return this.HTSCodes.GetIndex(hTSCode) + 1;
    }

    ValidiateLastHTSCode() {
        var errors: string[] = [];
        if (this.HTSCodes.Length != 0) {
            var lastHTSCode = this.HTSCodes.Collection[this.HTSCodes.Length - 1];
            if (lastHTSCode.EntityPM.Code == null && lastHTSCode.EntityPM.DestinationCountryId == null) {
                errors.push("Can't Add New HTSCode Line While HTSCode Code And Country are required");
            }
            else if (lastHTSCode.EntityPM.DestinationCountryId == null) {
                errors.push("Can't Add New HTSCode Line While HTSCode Country is required");
            }
            else if (lastHTSCode.EntityPM.Code == null) {
                errors.push("Can't Add New HTSCode Line While HTSCode Code is required");
            }
        }

        return errors;
    }
}

export class CustomerHTSCode extends BaseComponent {
    public EntityPM: HTSCodePM;
    public ObjectTableName: string = "HTSCode";
    public IsNewEntity: boolean = false;
    constructor(entity: HTSCodePM, public FatherComponent: CustomerProductItem, isNew: boolean) {
        super();
        this.EntityPM = entity;
        this.IsNewEntity = isNew;
    }

    SetUIProperties() {
        this.UIProperties.SetRequired("Code", "HTSCode", AppTool.IsNullOrEmpty(this.Code));
        this.UIProperties.SetRequired("DestinationCountryId", "HTSCode", AppTool.IsNullOrEmpty(this.DestinationCountryId));
    }

    get LineNumber() {
        return this.FatherComponent.GetIndexOfHTSCode(this);
    }

    get Id() {
        var myResult = null;

        if (this.EntityPM != null) {
            myResult = this.EntityPM.Id;
        }

        return myResult;
    }
    set Id(newValue: string) {
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

    get Code() {
        var myResult = null;

        if (this.EntityPM != null) {
            myResult = this.EntityPM.Code;
        }

        return myResult;
    }
    set Code(newValue: string) {
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

    destinationCountry: CountryList;
    get DestinationCountry() { return this.destinationCountry; }
    set DestinationCountry(value: CountryList) {
        if (this.destinationCountry != value) {
            this.destinationCountry = value;
        }
        if (!AppTool.IsNullOrEmpty(value)) {
            this.CountryEnglishName = value.EnglishName;
        } else {
            this.CountryEnglishName = null;
        }
    }

    get CountryEnglishName() {
        var myResult = null;

        if (this.EntityPM != null) {
            myResult = this.EntityPM.CountryEnglishName;
        }

        return myResult;
    }
    set CountryEnglishName(newValue: string) {
        if (this.EntityPM.CountryEnglishName != newValue) {
            this.EntityPM.CountryEnglishName = newValue;
        }
    }

    get InActive() { return this.EntityPM.InActive; }
    set InActive(value: boolean) {
        if (this.EntityPM.InActive != value) {
            this.EntityPM.InActive = value;
        }
    }

}
