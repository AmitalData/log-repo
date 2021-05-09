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



@Component({    
    templateUrl: './AddEditCustomerProductItemComponent.html',
})

export class AddEditCustomerProductItemComponent extends BaseComponent {
    public EntityPM: ProductItemPM;
    public ObjectTableName: string = "ProductItem";
    public TenantPM: TenantPM;
    public FatherComponent: CustomerProductItemsTabComponent;
    public DataContext: CustomerProductItem;
    public ValidationErrorsList: string[];
    private CurrentSession = SessionLocator.SelectedSession;
    public CustomerPM: CustomerPM = null;
    public CustomerProductItem: CustomerProductItem;
    public CustomerHTSCode: CustomerHTSCode;
    public HTSCodePM: HTSCodePM = null;
    public IsEditingEnabled: boolean = true;

    constructor(public entityArgs: EntityArgs) {
        super();
    }

    public IsResourcesReady: boolean = false;
    SetWindowArgs(windowArgs: any) {
        this.CustomerProductItem = windowArgs['CustomerProductItem'];
        this.EntityPM = this.CustomerProductItem.EntityPM;
        this.CustomerPM = windowArgs['CustomerPM'];
        this.CustomerHTSCode = windowArgs['CustomerHTSCode'];
        this.FatherComponent = windowArgs['CustomerProductItemsTabComponent'];
        this.HTSCodePM = this.CustomerHTSCode.EntityPM;
        this.DataContext = this.CustomerProductItem;
    }

    SetUIProperties() {

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
                if (this.FatherComponent.HTSCodes != null) {
                    this.FatherComponent.HTSCodes.Collection.forEach(item => {
                        this.FatherComponent.ProductItemEntityPM.AddHTSCodePM(item);
                    });
                }
            }

            this.CustomerProductItem.fatherComponent.BuildProductItems();
            this.CustomerProductItem.fatherComponent.BuildProductItemHTSCodes(this.CustomerProductItem.EntityPM);
            this.CurrentSession.CloseCurrentWindow();
        }
    }
}
