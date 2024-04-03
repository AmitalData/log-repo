import {Component} from '@angular/core';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {AppTool, ArrayTool} from '../../../../Infrastructure/Tools';
import {Cloner} from '../../../../Infrastructure/Utilities/Cloner';
import {Validator} from '../../../../Infrastructure/Validators/Validator';
import { ProductItemPM } from '../../../../Common/EntityPMs/ProductItemPM';
import {  CustomerProductItem, CustomerHTSCode } from '../EditTabs/CustomerProductItemsTabComponent';
import { HTSCodePM } from '../../../../Common/EntityPMs/HTSCodePM';

@Component({    
    templateUrl: './AddEditCustomerProductItemComponent.html',
})

export class AddEditCustomerProductItemComponent extends BaseComponent {
    public EntityPM: ProductItemPM;
    public ObjectTableName: string = "ProductItem";
    public DataContext: CustomerProductItem;
    public ValidationErrorsList: string[] = [];
    private CurrentSession = SessionLocator.SelectedSession;
    public IsEditingEnabled: boolean = true;
    public MainAddressCountryName: string;
    constructor() {
        super();
    }

    public IsResourcesReady: boolean = false;
    SetDataContext(dataContext: CustomerProductItem) {
        this.DataContext = dataContext;
        this.EntityPM = dataContext.EntityPM;
        this.IsEditingEnabled = dataContext.FatherComponent.IsEditingEnabled;
        this.MainAddressCountryName = this.DataContext.FatherComponent.MainAddressCountryName;

        if (this.DataContext.IsNewEntity) {
            this.AddHTSCode();
        }

        this.Clone();
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
            this.DataContext.HTSCodes.Insert(new CustomerHTSCode(hTSCodeItem, this.DataContext, true));
        }
    }

    OnRowEnded($event) {
        var errors = [];
        Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);

        if (($event) == this.DataContext.HTSCodes.Length) {
            this.AddHTSCode();
        }
    }

    CancelButtonClicked() {        
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    }

    OkButtonClicked() {
        this.ValidationErrorsList = this.ValidateProductItemAndHTsCode();

        if (this.ValidationErrorsList.length == 0) {
            this.ValidateHTSCodeUniqueCodeAndCountry(this.ValidationErrorsList);

            if (this.ValidationErrorsList.length == 0) {
                this.DataContext.HTSCodes.Collection.forEach(item => {
                    if (item != null) {
                        if (item.IsNewEntity && !AppTool.IsNullOrEmpty(item.Code) && !AppTool.IsNullOrEmpty(item.DestinationCountryId)) {
                            if (this.DataContext.EntityPM.HTSCodes.indexOf(item.EntityPM) == -1) {
                                item.IsNewEntity = false;
                                this.DataContext.EntityPM.AddHTSCodePM(item.EntityPM);
                            }
                        }
                    }
                });

                if (this.DataContext.IsNewEntity) {
                    this.DataContext.IsNewEntity = false;
                    this.DataContext.CustomerPM.AddProductItemPM(this.EntityPM);
                    this.DataContext.FatherComponent.ProductItems.Insert(this.DataContext);
                }

                this.CurrentSession.CloseCurrentWindowEmit("ok");
            }
        } 
    }

    private ValidateProductItemAndHTsCode() {
        var errors: string[] = [];        
        Validator.TryValidateObject(this.EntityPM, this.DataContext.ObjectTableName, errors);
        this.ValidateProductItemSKU(errors);
        this.ValidateHTSCodes(errors, true);
        return errors;
    }
    private ValidateHTSCodes(errors: string[], filterNewRecords: boolean = false) {
        if (this.DataContext.HTSCodes != null) {
            if (filterNewRecords) {
                this.DataContext.HTSCodes.Collection
                    .filter(d => !d.IsNewEntity || (d.IsNewEntity && (!AppTool.IsNullOrEmpty(d.Code) || !AppTool.IsNullOrEmpty(d.DestinationCountryId))))
                    .forEach(item => {
                    Validator.TryValidateObject(item, "HTSCode", errors);
                });
            }

            else {
                this.DataContext.HTSCodes.Collection.forEach(item => {
                    Validator.TryValidateObject(item, "HTSCode", errors);
                });
            }
        }
    }
    private ValidateProductItemSKU(errors: string[]) {
        if (this.DataContext.FatherComponent.ProductItems != null) {
            var sameSKURecordscount: number = this.DataContext.FatherComponent.ProductItems.Collection.filter(a => a.SKU == this.EntityPM.SKU).length;

            var sameSKURecordsCountForCompare = this.DataContext.IsNewEntity ? 0 : 1;
            if (sameSKURecordscount > sameSKURecordsCountForCompare) {
                errors.push("A Product Item with same SKU already exists");
            }
        }
    }
    private ValidateHTSCodeUniqueCodeAndCountry(errors: string[]) {
        if (this.DataContext.HTSCodes != null) {
            this.DataContext.HTSCodes.Collection.forEach(item => {
                if (!AppTool.IsNullOrEmpty(item.DestinationCountryId)) {
                    var filteredHTSCodes: CustomerHTSCode[] = this.DataContext.HTSCodes.Collection.filter(d => !AppTool.IsNullOrEmpty(d.DestinationCountryId));
                    if (filteredHTSCodes.filter(d => d.DestinationCountryId == item.DestinationCountryId && d.LineNumber != item.LineNumber).length > 0) {
                        errors.push("An HTSCode with " + item.CountryEnglishName + " country already exists");
                    }
                }               
            });
        }
    }

    private myCloner: Cloner;
    private Clone() {
        this.myCloner = new Cloner(this.DataContext);
        this.myCloner.AddField('InActive');
        this.myCloner.AddField('Description');
        this.myCloner.AddField('Name');
        this.myCloner.AddField('SKU');
        this.myCloner.AddField('Brand');
        this.myCloner.AddField('ASIN');
        this.myCloner.AddField('UPC');
        this.myCloner.AddField('OriginCountryId');
        this.myCloner.AddField('OriginCountryName');
        this.myCloner.AddField('ShipperId');
        this.myCloner.AddField('ShipperName');

        this.myCloner.AddField('ProductValue');
        this.myCloner.AddField('ProductValueCurrencyId');
        this.myCloner.AddField('Quantity');
        this.myCloner.AddField('ProductValueCurrencyCode');
        this.myCloner.AddEntity(this.EntityPM);
        this.myCloner.AddEntity(this.DataContext.CustomerPM);
    }

    private RejectChanges() {
        this.DataContext.ResetHTSCodes();
        this.myCloner.RejectChanges();
        this.DataContext.maxHTSCodesLineNumber = ArrayTool.Max(this.EntityPM.HTSCodes, "LineNumber");
    }
}
