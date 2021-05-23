import { Component } from '@angular/core';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { TextCodeTranslator } from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import { ShipmentPM } from '../../../../Shipment/EntityPMs/ShipmentPM';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { ProductItemPM } from '../../../../Common/EntityPMs/ProductItemPM';
import { ObservableCollection } from '../../../../Infrastructure/Utilities/ObservableCollection';
import { HTSCodePM } from '../../../../Common/EntityPMs/HTSCodePM';
import { Validator } from '../../../../Infrastructure/Validators/Validator';
import { AppTool } from '../../../../Infrastructure/Tools';
import { CountryList } from '../../../../Common/EntityLists/CountryList';
import { ShipmentTool } from '../../../../Shipment/Tools';

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
    constructor() {
        super();
    }

    SetWindowArgs(windowArgs: any) {
        this.ShipmentPM = windowArgs['ShipmentPM'];
        this.EntityPM = windowArgs['EntityPM'];
        this.IsEditingEnabled = ShipmentTool.IsEditingEnabled(this.ShipmentPM);
        this.DataContext = this;

        this.SetUIProperties();
        this.BuildHTSCodes();
    }

    private SetUIProperties() {

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

    //public savedItems: HTSCodePM[] = [];
    //private CopyHTSCodes() {
    //    this.savedItems = [];
    //    if (this.EntityPM.HTSCodes.length > 0) {
    //        this.EntityPM.HTSCodes.forEach(item => {
    //            var htsCode = new HTSCodePM(null);
    //            htsCode.ApprovedByCustomer = item.ApprovedByCustomer;
    //            htsCode.Tenant = item.Tenant;
    //            htsCode.Code = item.Code;
    //            htsCode.CountryEnglishName = item.CountryEnglishName;
    //            htsCode.DestinationCountryId = item.DestinationCountryId;
    //            htsCode.InActive = item.InActive;
    //            htsCode.ItemId = item.ItemId;
    //            this.savedItems.push(htsCode);
    //        });
    //    }
    //}

    //public ResetHTSCodes() {
    //    if (this.savedItems != null) {
    //        var items: HTSCodePM[] = this.EntityPM.HTSCodes;
    //        items.forEach(item => {
    //            var savedItem: HTSCodePM = this.savedItems.filter(d => d.Code == item.Code)[0];
    //            if (savedItem == null) {
    //                if (this.EntityPM.HTSCodes.indexOf(item) != -1) {
    //                    this.EntityPM.RemoveHTSCodePM(item);
    //                }
    //            }

    //            else {
    //                item.ApprovedByCustomer = savedItem.ApprovedByCustomer;
    //                item.Code = savedItem.Code;
    //                item.CountryEnglishName = savedItem.CountryEnglishName;
    //                item.DestinationCountryId = savedItem.DestinationCountryId;
    //                item.InActive = savedItem.InActive;
    //            }
    //        });

    //        this.savedItems.forEach(item => {
    //            var list = this.EntityPM.HTSCodes.filter(d => d.Code == item.Code);
    //            if (list == null) {
    //                this.EntityPM.HTSCodes.push(item);
    //            }
    //        });
    //    }
    //}

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

    get Remarks() { return this.EntityPM.Remarks }
    set Remarks(value: string) {
        if (this.EntityPM.Remarks != value) {
            this.EntityPM.Remarks = value;
        }
    }

    get Brand() { return this.EntityPM.Brand }
    set Brand(value: string) {
        if (this.EntityPM.Brand != value) {
            this.EntityPM.Brand = value;
        }
    }

    AddHTSCode() {
        var hTSCodeItem: HTSCodePM = new HTSCodePM(null);
        hTSCodeItem.Tenant = SessionLocator.Tenant;
        hTSCodeItem.ItemId = this.EntityPM.Id;
        this.HTSCodes.Insert(new CustomerHTSCode(hTSCodeItem, this, true));       
    }

    OnRowEnded($event) {
        var errors = [];
        Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);

        if (($event) == this.HTSCodes.Length) {
            this.AddHTSCode();
        }
    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }
    OkButtonClicked() {
        this.ValidationErrorsList = [];

        var msg: string = TextCodeTranslator.Translate("General.M.FieldIsRequired");

        if (this.ValidationErrorsList.length == 0) {
           

            //this.CurrentSession.CloseCurrentWindowEmit("OK");
            //this.CurrentSession.FireEvent("ShipmentPartnersChanged");
        }
    }    

}

export class CustomerHTSCode extends BaseComponent {
    public EntityPM: HTSCodePM;
    public ObjectTableName: string = "HTSCode";
    public IsNewEntity: boolean = false;
    constructor(entity: HTSCodePM, public FatherComponent: EditCustomerProductItemComponent, isNew: boolean) {
        super();
        this.EntityPM = entity;
        this.IsNewEntity = isNew;
    }

    SetUIProperties() {
        this.UIProperties.SetRequired("Code", "HTSCode", AppTool.IsNullOrEmpty(this.Code));
        this.UIProperties.SetRequired("DestinationCountryId", "HTSCode", AppTool.IsNullOrEmpty(this.DestinationCountryId));
    }

    //get LineNumber() {
    //    return this.FatherComponent.GetIndexOfHTSCode(this);
    //}

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
