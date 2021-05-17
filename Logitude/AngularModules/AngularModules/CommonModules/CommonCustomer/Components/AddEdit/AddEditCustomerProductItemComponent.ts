import {Component} from '@angular/core';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {AppTool} from '../../../../Infrastructure/Tools';
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
        this.ValidationErrorsList = this.ValidateProductItemAndHTsCode();

        if (this.ValidationErrorsList.length == 0) {
            this.ValidateHTSCodeUniqueCodeAndCountry(this.ValidationErrorsList);

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
    }

    private ValidateProductItemAndHTsCode() {
        var errors: string[] = [];        
        Validator.TryValidateObject(this.EntityPM, this.DataContext.ObjectTableName, errors);
        this.ValidateItemCode(errors);
        this.ValidateHTSCodes(errors);
        return errors;
    }
    private ValidateHTSCodes(errors: string[]) {
        if (this.DataContext.HTSCodes != null) {
            this.DataContext.HTSCodes.Collection.forEach(item => {
                Validator.TryValidateObject(item, "HTSCode", errors);                
            });
        }
    }
    private ValidateItemCode(errors: string[]) {
        if (this.DataContext.FatherComponent.ProductItems != null) {
            if (this.DataContext.IsNewEntity) {
                if (this.DataContext.FatherComponent.ProductItems.Collection.filter(d => d.ItemCode == this.EntityPM.ItemCode).length > 0) {
                    errors.push("A Product Item with this code already exists");
                }
            }

            else {
                if (this.DataContext.FatherComponent.ProductItems.Collection.filter(d => d.ItemCode == this.EntityPM.ItemCode && d.Id != this.EntityPM.Id).length > 0) {
                    errors.push("A Product Item with this code already exists");
                }
            }
        }
    }
    private ValidateHTSCodeUniqueCodeAndCountry(errors: string[]) {
        if (this.DataContext.HTSCodes != null) {
            this.DataContext.HTSCodes.Collection.forEach(item => {

                if (!AppTool.IsNullOrEmpty(item.Code)) {
                    var filteredHTSCodes: CustomerHTSCode[] = this.DataContext.HTSCodes.Collection.filter(d => !AppTool.IsNullOrEmpty(d.Code));
                    if (filteredHTSCodes.filter(d => d.Code == item.Code && d.LineNumber != item.LineNumber).length > 0) {
                        errors.push("An HTSCode with this code already exists");
                    }
                }

                if (!AppTool.IsNullOrEmpty(item.DestinationCountryId)) {
                    var filteredHTSCodes: CustomerHTSCode[] = this.DataContext.HTSCodes.Collection.filter(d => !AppTool.IsNullOrEmpty(d.DestinationCountryId));
                    if (filteredHTSCodes.filter(d => d.DestinationCountryId == item.DestinationCountryId && d.LineNumber != item.LineNumber).length > 0) {
                        errors.push("An HTSCode with this country already exists");
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
