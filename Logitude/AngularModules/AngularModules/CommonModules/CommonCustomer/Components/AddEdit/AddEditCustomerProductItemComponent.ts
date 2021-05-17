import {Component} from '@angular/core';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {AppTool} from '../../../../Infrastructure/Tools';
import {Cloner} from '../../../../Infrastructure/Utilities/Cloner';
import {Validator} from '../../../../Infrastructure/Validators/Validator';
import { ProductItemPM } from '../../../../Common/EntityPMs/ProductItemPM';
import {  CustomerProductItem } from '../EditTabs/CustomerProductItemsTabComponent';

@Component({    
    templateUrl: './AddEditCustomerProductItemComponent.html',
})

export class AddEditCustomerProductItemComponent extends BaseComponent {
    public EntityPM: ProductItemPM;
    public ObjectTableName: string = "ProductItem";
    public DataContext: CustomerProductItem;
    public ValidationErrorsList: string[];
    private CurrentSession = SessionLocator.SelectedSession;
    public IsEditingEnabled: boolean = true;
    constructor() {
        super();
    }

    public IsResourcesReady: boolean = false;
    SetDataContext(dataContext: CustomerProductItem) {
        this.DataContext = dataContext;
        this.EntityPM = dataContext.EntityPM;
        this.IsEditingEnabled = dataContext.FatherComponent.IsEditingEnabled;
        this.Clone();
    }

    CancelButtonClicked() {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    }

    OkButtonClicked() {
        var errors: string[] = [];
        errors = this.ValidateProductItemAndHTsCode();
        this.ValidationErrorsList = errors;

        if (this.ValidationErrorsList.length == 0) {
            this.DataContext.HTSCodes.Collection.forEach(item => {
                if (item != null) {
                    if (item.IsNewEntity) {
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

            this.CurrentSession.CloseCurrentWindow();
        } 
    }

    private ValidateProductItemAndHTsCode() {
        var errors: string[] = [];
        
        Validator.TryValidateObject(this.EntityPM, this.DataContext.ObjectTableName, errors);
        this.ValidationErrorsList = errors;

        var htsCodeErrors = this.ValidateHTSCodes();
        if (htsCodeErrors) {
            errors = errors.concat(htsCodeErrors);
        }
        var countryError = this.ValidateCountry();
        if (countryError) {
            errors = errors.concat(countryError);  
        }
        var itemCodeError = this.ValidateItemCode();
        if (itemCodeError) {
            errors = errors.concat(itemCodeError);
        }
        return errors;
    }

    private ValidateHTSCodes() {
        var errors: string[] = [];
        if (this.DataContext.HTSCodes != null) {
            this.DataContext.HTSCodes.Collection.forEach(item => {
                Validator.TryValidateObject(item, "HTSCode", errors);
                if (AppTool.IsNullOrEmpty(item.Code)) {
                    errors.push("HTSCode Code is required");
                }
                if (AppTool.IsNullOrEmpty(item.DestinationCountryId)) {
                    errors.push("HTSCode Country is required");
                }
            });
        }
        return errors;
    }

    private ValidateCountry() {
        var errorMassage;
        if (this.DataContext.HTSCodes != null) {
            var HTSCodes = this.DataContext.HTSCodes.Collection;

            var destinationCountryAndItsCount = HTSCodes.reduce((a, e) => {
                a[e.DestinationCountryId] = ++a[e.DestinationCountryId] || 0;
                return a;
            }, {});

            var duplicatedDestinationCountry = HTSCodes.filter(e => destinationCountryAndItsCount[e.DestinationCountryId]);
        }
        if (duplicatedDestinationCountry.length > 0) {
            errorMassage = "An HTSCode with this Country already exists";
        }
        return errorMassage;
    }

    private ValidateItemCode() {
        var errorMassage;
        if (this.DataContext.FatherComponent.ProductItems != null) {
            var ProductItems = this.DataContext.FatherComponent.ProductItems.Collection;

            var itemCodeAndItsCount = ProductItems.reduce((a, e) => {
                a[e.ItemCode] = ++a[e.ItemCode] || 0;
                return a;
            }, {});

            var duplicatedDestinationCountry = ProductItems.filter(e => itemCodeAndItsCount[e.ItemCode]);
        }
        if (duplicatedDestinationCountry.length > 0) {
            errorMassage = "A Product Item with this code already exists";
        }
        return errorMassage;
    }

    private myCloner: Cloner;
    private Clone() {
        this.myCloner = new Cloner(this.DataContext);
        this.myCloner.AddField('InActive');
        this.myCloner.AddField('Description');
        this.myCloner.AddField('ItemCode');
        this.myCloner.AddField('SKU');
        this.myCloner.AddField('Remarks');
        this.myCloner.AddField('Code');
        this.myCloner.AddField('DestinationCountryId');
        this.myCloner.AddField('CountryEnglishName');
        this.myCloner.AddField('ItemId');
        this.myCloner.AddEntity(this.EntityPM);
        this.myCloner.AddEntity(this.DataContext.CustomerPM);
    }

    private RejectChanges() {
        this.DataContext.ResetHTSCodes();
        this.myCloner.RejectChanges();
    }
}
