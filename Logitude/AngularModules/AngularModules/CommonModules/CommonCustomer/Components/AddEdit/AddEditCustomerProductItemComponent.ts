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
import { CustomerProductItem } from '../EditTabs/CustomerProductItemsTabComponent';

@Component({    
    templateUrl: './AddEditCustomerProductItemComponent.html',
})

export class AddEditCustomerProductItemComponent extends BaseComponent {
    public EntityPM: ProductItemPM;
    public ObjectTableName: string = "ProductItem";
    public TenantPM: TenantPM;
    public DataContext: AddEditCustomerProductItemComponent = this;
    public ValidationErrorsList: string[];
    private CurrentSession = SessionLocator.SelectedSession;
    public CustomerPM: CustomerPM = null;
    public CustomerProductItem: CustomerProductItem;

    constructor(public entityArgs: EntityArgs) {
        super();        
    }

    public IsResourcesReady: boolean = false;
    SetWindowArgs(windowArgs: any) {
        this.CustomerProductItem = windowArgs['CustomerProductItem'];
        this.EntityPM = this.CustomerProductItem.EntityPM;
        this.CustomerPM = windowArgs['CustomerPM'];      
    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    OkButtonClicked() {
        var errors: string[] = [];
        Validator.TryValidateObject(this.EntityPM, this.DataContext.ObjectTableName, errors);
        this.ValidationErrorsList = errors;

        if (this.ValidationErrorsList.length == 0) {

            if (this.CustomerProductItem.IsNewEntity) {
                this.CustomerPM.AddProductItemPM(this.EntityPM);
            }

            this.CustomerProductItem.fatherComponent.BuildProductItems();

            this.CurrentSession.CloseCurrentWindow();
        }
    }
}
