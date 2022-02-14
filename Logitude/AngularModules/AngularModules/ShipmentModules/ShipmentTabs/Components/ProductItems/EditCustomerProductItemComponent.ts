import { Component } from '@angular/core';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ShipmentPM } from '../../../../Shipment/EntityPMs/ShipmentPM';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { ProductItemPM } from '../../../../Common/EntityPMs/ProductItemPM';
import { ObservableCollection } from '../../../../Infrastructure/Utilities/ObservableCollection';
import { HTSCodePM } from '../../../../Common/EntityPMs/HTSCodePM';
import { Validator } from '../../../../Infrastructure/Validators/Validator';
import { AppTool, ArrayTool } from '../../../../Infrastructure/Tools';
import { CountryList } from '../../../../Common/EntityLists/CountryList';
import { ShipmentTool } from '../../../../Shipment/Tools';
import { PartnersDomainService } from '../../../../Common/Services/PartnersDomainService';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { CountryListService } from '../../../../Common/Services/StandardLists/CountryListService';
import { CardList } from '../../../../Common/EntityLists/CardList';

@Component({
    templateUrl: './EditCustomerProductItemComponent.html',
})

export class EditCustomerProductItemComponent extends BaseComponent {
    public EntityPM: ProductItemPM;
    public ShipmentPM: ShipmentPM;
    public DataContext: EditCustomerProductItemComponent;
    public ObjectTableName: string = "ProductItem";   
    public ValidationErrorsList: string[];
    public HTSCodes: ObservableCollection;
    private CurrentSession = SessionLocator.SelectedSession;
    public IsEditingEnabled: boolean = false;
    public CustomerMainAddressCountryName: string;
    private maxHTSCodesLineNumber: number = 0;
    constructor() {
        super();
        this.HTSCodes = new ObservableCollection([]);
    }

    SetWindowArgs(windowArgs: any) {
        this.ShipmentPM = windowArgs['ShipmentPM'];
        this.EntityPM = windowArgs['EntityPM'];
        this.IsEditingEnabled = ShipmentTool.IsEditingEnabled(this.ShipmentPM);
        this.DataContext = this;
        this.GetConsigneeCountry();
        
    }

    private GetConsigneeCountry() {
        var countryService: CountryListService = new CountryListService();
        countryService.getSingle(this.ShipmentPM.ConsigneeCountryId).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                var country: CountryList = myResponse.Result;
                if (country) {
                    this.CustomerMainAddressCountryName = country.EnglishName;
                }

                this.BuildHTSCodes();
                this.maxHTSCodesLineNumber = ArrayTool.Max(this.HTSCodes.Collection, "LineNumber");
            }
        });
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

        if (!AppTool.IsNullOrEmpty(value)) {
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

    AddHTSCode() {
        this.ValidationErrorsList = [];
        this.ValidateHTSCodes(this.ValidationErrorsList);

        if (this.ValidationErrorsList.length == 0) {
            this.DataContext.maxHTSCodesLineNumber += 1;
            var hTSCodeItem: HTSCodePM = new HTSCodePM(null);
            hTSCodeItem.Tenant = SessionLocator.Tenant;
            hTSCodeItem.ItemId = this.EntityPM.Id;
            hTSCodeItem.LineNumber = this.DataContext.maxHTSCodesLineNumber;
            this.HTSCodes.Insert(new CustomerHTSCode(hTSCodeItem, this, true));
        }
    }

    OnRowEnded($event) {
        if (($event) == this.HTSCodes.Length) {
            this.AddHTSCode();
        }
    }

    CancelButtonClicked() {
        this.DataContext.maxHTSCodesLineNumber = ArrayTool.Max(this.HTSCodes.Collection, "LineNumber");
        this.CurrentSession.CloseCurrentWindow();
    }
    OkButtonClicked() {
        this.ValidationErrorsList = this.ValidateProductItemAndHTsCode();

        if (this.ValidationErrorsList.length == 0) {
            this.ValidateHTSCodeUniqueCodeAndCountry(this.ValidationErrorsList);

            if (this.ValidationErrorsList.length == 0) {
                this.CurrentSession.StartBusyIndicatorSaving();
                this.HTSCodes.Collection.forEach(item => {
                    if (item != null) {
                        if (item.IsNewEntity && !AppTool.IsNullOrEmpty(item.Code) && !AppTool.IsNullOrEmpty(item.DestinationCountryId)) {
                            if (this.EntityPM.HTSCodes.indexOf(item.EntityPM) == -1) {
                                item.IsNewEntity = false;
                                this.EntityPM.AddHTSCodePM(item.EntityPM);
                            }
                        }
                    }
                });

                var partnersDomainService: PartnersDomainService = new PartnersDomainService();
                partnersDomainService.UpdateCustomerProductItem(this.EntityPM).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        this.EntityPM = myResponse.Result;
                        this.CurrentSession.CloseCurrentWindowEmit("OK");
                    }

                    else {
                        this.ValidationErrorsList = myResponse.ErrorsArray;
                    }

                    this.CurrentSession.StopBusyIndicator();
                });
            }   
        }
    }

    private ValidateProductItemAndHTsCode() {
        var errors: string[] = [];
        Validator.TryValidateObject(this.EntityPM, this.DataContext.ObjectTableName, errors);
        this.ValidateHTSCodes(errors, true);
        return errors;
    }
    private ValidateHTSCodes(errors: string[], filterNewRecords: boolean = false) {
        if (this.HTSCodes != null) {
            if (filterNewRecords) {
                this.HTSCodes.Collection
                    .filter(d => !d.IsNewEntity || (d.IsNewEntity && (!AppTool.IsNullOrEmpty(d.Code) || !AppTool.IsNullOrEmpty(d.DestinationCountryId))))
                    .forEach(item => {
                        Validator.TryValidateObject(item, "HTSCode", errors);
                    });
            }

            else {
                this.HTSCodes.Collection.forEach(item => {
                    Validator.TryValidateObject(item, "HTSCode", errors);
                });
            }
        }
    }
    private ValidateHTSCodeUniqueCodeAndCountry(errors: string[]) {
        if (this.HTSCodes != null) {
            this.HTSCodes.Collection.forEach(item => {
                if (!AppTool.IsNullOrEmpty(item.DestinationCountryId)) {
                    var filteredHTSCodes: CustomerHTSCode[] = this.HTSCodes.Collection.filter(d => !AppTool.IsNullOrEmpty(d.DestinationCountryId));
                    if (filteredHTSCodes.filter(d => d.DestinationCountryId == item.DestinationCountryId && d.LineNumber != item.LineNumber).length > 0) {
                        errors.push("An HTSCode with " + item.CountryEnglishName + " country already exists");
                    }
                }
            });
        }
    }
}

export class CustomerHTSCode extends BaseComponent {
    public EntityPM: HTSCodePM;
    public ObjectTableName: string = "HTSCode";
    public IsNewEntity: boolean = false;
    public DataContext = this;
    constructor(entity: HTSCodePM, public FatherComponent: EditCustomerProductItemComponent, isNew: boolean) {
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

    get IsCheckBoxesEnabled() {
        var isEnabled = false;

        if (!AppTool.IsNullOrEmpty(this.Code) || !AppTool.IsNullOrEmpty(this.DestinationCountryId)) {
            isEnabled = true;
        }

        return isEnabled;
    }
}
