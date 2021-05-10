import {Component} from '@angular/core';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {AppTool} from '../../../../Infrastructure/Tools';
import {Cloner} from '../../../../Infrastructure/Utilities/Cloner';
import {Validator} from '../../../../Infrastructure/Validators/Validator';
import { ProductItemPM } from '../../../../Common/EntityPMs/ProductItemPM';
import { TenantPM } from '../../../../Common/EntityPMs/TenantPM';
import { CustomerPM } from '../../../../Common/EntityPMs/CustomerPM';
import { EntityArgs } from '../../../../Infrastructure/DataContracts/EntityArgs';
import { CustomerHTSCode, CustomerProductItem, CustomerProductItemsTabComponent } from '../EditTabs/CustomerProductItemsTabComponent';
import { HTSCodePM } from '../../../../Common/EntityPMs/HTSCodePM';
import { CustomerValidator } from '../../../../Common/Validators/CustomerValidator';



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
    }

    SetUIProperties() {

    }

    CancelButtonClicked() {
        this.Clone();
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    }

    OkButtonClicked() {
        var errors: string[] = [];


        if (this.CustomerProductItem.IsNewEntity) {
            this.CustomerPM.AddProductItemPM(this.CustomerProductItem.EntityPM);
            if (this.FatherComponent.HTSCodes != null) {

                var currentProductItem = this.CustomerPM.CustomerProductItems.filter(a => a.Id == this.CustomerProductItem.EntityPM.Id)[0];
                this.FatherComponent.HTSCodes.Collection.forEach(item => {
                    if (item.IsNewEntity) {
                        currentProductItem.AddHTSCodePM(item.EntityPM);
                    }
                });
            }
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
    

        Validator.TryValidateObject(this.CustomerProductItem.EntityPM, this.DataContext.ObjectTableName, errors);
        var entityValidator: CustomerValidator = new CustomerValidator();
        var entityErrors = entityValidator.Validate(this.FatherComponent.EntityPM);

        if (entityErrors) {
            errors = errors.concat(entityErrors);
        }
        this.ValidationErrorsList = errors;
        if (this.ValidationErrorsList.length == 0) {
            this.CurrentSession.CloseCurrentWindow();
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

        this.myCloner.AddEntity(this.EntityPM);
        this.myCloner.AddEntity(this.CustomerPM);
    }
    private RejectChanges() {
        this.myCloner.RejectChanges();
        if (this.FatherComponent.ProductItems != null) {
            this.FatherComponent.ProductItems = null;
        }
        this.FatherComponent.BuildProductItems();
    }

}
