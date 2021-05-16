import {Component} from '@angular/core';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {AppTool} from '../../../../Infrastructure/Tools';
import {Cloner} from '../../../../Infrastructure/Utilities/Cloner';
import {Validator} from '../../../../Infrastructure/Validators/Validator';
import { ProductItemPM } from '../../../../Common/EntityPMs/ProductItemPM';
import { CustomerPM } from '../../../../Common/EntityPMs/CustomerPM';
import { EntityArgs } from '../../../../Infrastructure/DataContracts/EntityArgs';
import {  CustomerProductItem, CustomerProductItemsTabComponent } from '../EditTabs/CustomerProductItemsTabComponent';

@Component({    
    templateUrl: './AddEditCustomerProductItemComponent.html',
})

export class AddEditCustomerProductItemComponent extends BaseComponent {
    public EntityPM: ProductItemPM;
    public ObjectTableName: string = "ProductItem";
    public FatherComponent: CustomerProductItemsTabComponent;
    public DataContext: CustomerProductItem;
    public ValidationErrorsList: string[];
    private CurrentSession = SessionLocator.SelectedSession;
    public CustomerPM: CustomerPM = null;
    public CustomerProductItem: CustomerProductItem;
    public IsEditingEnabled: boolean = true;

    constructor(public entityArgs: EntityArgs) {
        super();
    }

    public IsResourcesReady: boolean = false;
    SetWindowArgs(windowArgs: any) {
        this.CustomerProductItem = windowArgs['CustomerProductItem'];
        this.EntityPM = this.CustomerProductItem.EntityPM;
        this.CustomerPM = windowArgs['CustomerPM'];
        this.FatherComponent = windowArgs['CustomerProductItemsTabComponent'];
        this.DataContext = this.CustomerProductItem;
        this.Clone();
    }

    SetUIProperties() {

    }

    CancelButtonClicked() {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
        this.FatherComponent.ProductItems.Collection.forEach(item => {
            if (item.EntityPM.ItemCode == null)
                this.FatherComponent.ProductItems.Remove(item);
        });
    }

    OkButtonClicked() {
        var errors: string[] = [];
        this.ClearValidationErrorsLists();
        errors = this.ValidateProductItemAndHTsCode();
        this.ValidationErrorsList = errors;
        if (this.ValidationErrorsList.length == 0) {
            if (this.CustomerProductItem.IsNewEntity) {
                if (this.FatherComponent.HTSCodes != null) {
                    this.FatherComponent.HTSCodes.Collection.forEach(item => {
                        if (item.IsNewEntity) {
                            this.CustomerProductItem.EntityPM.AddHTSCodePM(item.EntityPM);
                        }
                    });
                }
                this.CustomerPM.AddProductItemPM(this.CustomerProductItem.EntityPM);
            } else {
                if (this.FatherComponent.HTSCodes != null) {
                    var currentProductItem = this.CustomerPM.CustomerProductItems.filter(a => a.Id == this.CustomerProductItem.EntityPM.Id)[0];
                    this.FatherComponent.HTSCodes.Collection.forEach(item => {
                        if (item.IsNewEntity) {
                            currentProductItem.AddHTSCodePM(item.EntityPM);
                        }
                    });
                }
            }
            this.CurrentSession.CloseCurrentWindow();
        } 
    }

    ClearValidationErrorsLists() {
        this.ValidationErrorsList = [];
        this.FatherComponent.ValidationErrorsList = [];
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
        if (this.FatherComponent.HTSCodes != null) {
            this.FatherComponent.HTSCodes.Collection.forEach(item => {
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
        if (this.FatherComponent.HTSCodes != null) {
            var HTSCodes = this.FatherComponent.HTSCodes.Collection;

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
        if (this.FatherComponent.ProductItems != null) {
            var ProductItems = this.FatherComponent.ProductItems.Collection;

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
        this.myCloner.AddEntity(this.FatherComponent);
        this.myCloner.AddEntity(this.FatherComponent.ProductItems);
        this.myCloner.AddEntity(this.FatherComponent.HTSCodes);
        this.myCloner.AddEntity(this.FatherComponent.EntityPM);
        this.myCloner.AddEntity(this.DataContext.EntityPM);
        this.myCloner.AddEntity(this.CustomerPM);
    }

    private RejectChanges() {
        this.myCloner.RejectChanges();
    }

}
