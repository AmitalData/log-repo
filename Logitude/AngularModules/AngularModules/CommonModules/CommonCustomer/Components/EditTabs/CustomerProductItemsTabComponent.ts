import { Component, OnDestroy, OnInit } from '@angular/core';
import { EntityArgs } from '../../../../Infrastructure/DataContracts/EntityArgs';
import { AppTool, ArrayTool } from '../../../../Infrastructure/Tools';
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
import { CardList } from '../../../../Common/EntityLists/CardList';
import { CurrencyPM } from 'Common/EntityPMs/CurrencyPM';

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
    private CurrentSession = SessionLocator.SelectedSession;
    public MainAddressCountryName: string;
    constructor(public entityArgs: EntityArgs, private entityResourceService: EntityResourceService) {
        super();
        this.EntityPM = this.entityArgs.EntityPM;
        this.MainAddressCountryName = this.EntityPM.CountryName;
        this.ProductItems = new ObservableCollection([]);
        this.Listen();
    }

    ngOnInit() {
        this.entityResourceService.getEntityResourceByTableName(this.ObjectTableName).subscribe((res1: any) => {
            this.entityResourceService.getEntityResourceByTableName("HTSCode").subscribe((res2: any) => {
                this.SetUIProperties();
                this.BuildProductItems();
            });
        });
    }

    private Listen() {
        if (this.entityArgs.EditComponent) {
            this.SaveCompletedEvent = this.entityArgs.EditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                if (isSaveSuccess) {
                    this.EntityPM = this.entityArgs.EditComponent.EntityPM;
                    this.MainAddressCountryName = this.EntityPM.CountryName;

                    this.SetUIProperties();
                    this.BuildProductItems();
                }
            });

            this.LoadCompletedEvent = this.entityArgs.EditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                if (isLoadSuccess) {
                    this.EntityPM = this.entityArgs.EditComponent.EntityPM;
                    this.MainAddressCountryName = this.EntityPM.CountryName;

                    this.SetUIProperties();
                    this.BuildProductItems();
                }
            });
        }
    }

    private SaveCompletedEvent: any = null;
    private LoadCompletedEvent: any = null;
    ngOnDestroy() {
        AppTool.KillEventEmitter(this.LoadCompletedEvent);
        AppTool.KillEventEmitter(this.SaveCompletedEvent);
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
        logitudeWindow.Height = 650;
        logitudeWindow.Width = 1000; 
        logitudeWindow.Show('./CommonModules/CommonCustomer/Components/AddEdit/AddEditCustomerProductItemComponent');
    }
}

export class CustomerProductItem extends BaseComponent {
    public EntityPM: ProductItemPM;
    public ObjectTableName: string = "ProductItem";
    public IsNewEntity: boolean = false;
    public CustomerPM: CustomerPM;
    public HTSCodes: ObservableCollection;
    public maxHTSCodesLineNumber: number = 0;
    constructor(entity: ProductItemPM, public FatherComponent: CustomerProductItemsTabComponent, isNew: boolean) {
        super();
        this.EntityPM = entity;
        this.IsNewEntity = isNew;
        this.CustomerPM = this.FatherComponent.EntityPM;
        this.HTSCodes = new ObservableCollection([]);
        
        this.SetUIProperties();
        this.BuildHTSCodes();        
        this.maxHTSCodesLineNumber = ArrayTool.Max(this.HTSCodes.Collection, "LineNumber");
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
                htsCode.LineNumber = item.LineNumber;
                this.savedItems.push(htsCode);
            });
        }
    }

    public ResetHTSCodes() {
        if (this.savedItems != null) {
            var items: HTSCodePM[] = this.EntityPM.HTSCodes;
            items.forEach(item => {
                var savedItem: HTSCodePM = this.savedItems.filter(d => d.LineNumber == item.LineNumber)[0];
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
                var list = this.EntityPM.HTSCodes.filter(d => d.LineNumber == item.LineNumber);
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

    get Name() { return this.EntityPM.Name }
    set Name(value: string) {
        if (this.EntityPM.Name != value) {
            this.EntityPM.Name = value;
        }
    }

    get SKU() { return this.EntityPM.SKU }
    set SKU(value: string) {
        if (this.EntityPM.SKU != value) {
            this.EntityPM.SKU = value;
        }
    }

    get Brand() { return this.EntityPM.Brand }
    set Brand(value: string) {
        if (this.EntityPM.Brand != value) {
            this.EntityPM.Brand = value;
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

    originCountry: CountryList;
    get OriginCountry() { return this.originCountry; }
    set OriginCountry(value: CountryList) {
        if (this.originCountry != value) {
            this.originCountry = value;
        }

        if (value) {
            this.OriginCountryName = value.EnglishName;
        }
        else {
            this.OriginCountryName = null;
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

    shipper: CardList;
    get Shipper() { return this.shipper; }
    set Shipper(value: CardList) {
        if (this.shipper != value) {
            this.shipper = value;
        }

        if (value) {
            this.ShipperName = value.EnglishName;
        }
        else {
            this.ShipperName = null;
        }
    }

    get ProductValue() { return this.EntityPM.ProductValue; }
    set ProductValue(newValue: number) {
        if (this.EntityPM.ProductValue != newValue) {
            this.EntityPM.ProductValue = newValue;
        }
    }

    get Quantity() { return this.EntityPM.Quantity; }
    set Quantity(newValue: number) {
        if (this.EntityPM.Quantity != newValue) {
            this.EntityPM.Quantity = newValue;
        }
    }

    get ProductValueCurrencyId() { return this.EntityPM.ProductValueCurrencyId; }
    set ProductValueCurrencyId(newValue: string) {
        if (this.EntityPM.ProductValueCurrencyId != newValue) {
            this.EntityPM.ProductValueCurrencyId = newValue;
        }
    }

    get ProductValueCurrencyCode() { return this.EntityPM.ProductValueCurrencyCode; }
    set ProductValueCurrencyCode(newValue: string) {
        if (this.EntityPM.ProductValueCurrencyCode != newValue) {
            this.EntityPM.ProductValueCurrencyCode = newValue;
        }
    }

    productValueCurrency: CurrencyPM;
    get ProductValueCurrency() { return this.productValueCurrency; }
    set ProductValueCurrency(value: CurrencyPM) {
        if (this.productValueCurrency != value) {
            this.productValueCurrency = value;
        }
        if (value) this.ProductValueCurrencyCode = value.Code;    
        else this.ProductValueCurrencyCode = null;    
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
        var myResult = null;
        if (this.EntityPM != null) {
            myResult = this.EntityPM.LineNumber;
        }
        return myResult;
    }
    set LineNumber(newValue: number) {
        if (this.EntityPM.LineNumber != newValue) {
            this.EntityPM.LineNumber = newValue;
        }
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

        if (value) {
            this.CountryEnglishName = value.EnglishName;
        }
        else {
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

    get IsCheckBoxesEnabled() {
        var isEnabled = false;

        if (!AppTool.IsNullOrEmpty(this.Code) || !AppTool.IsNullOrEmpty(this.DestinationCountryId)) {
            isEnabled = true;
        }

        return isEnabled;
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
}
