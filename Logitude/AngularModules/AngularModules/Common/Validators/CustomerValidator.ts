import { AppTool, FormatTool } from '../../Infrastructure/Tools';
import { Validator } from '../../Infrastructure/Validators/Validator';
import { TextCodeTranslator } from '../../Infrastructure/Utilities/TextCodeTranslator';
import { CustomerPM } from '../EntityPMs/CustomerPM';
import { ProductItemPM } from '../EntityPMs/ProductItemPM';

export interface ICustomerValidator {
    Validate(customerPM: CustomerPM): any[];
}

export class CustomerValidator implements ICustomerValidator {
    private Errors: string[] = [];
    private entityPM: CustomerPM;

    constructor() {
        this.Errors = [];
    }


    Validate  = (customerPM: CustomerPM): any[] => {
        this.Errors = [];
        this.entityPM = customerPM;
        this.ValidateProductItems();
        return this.Errors;
    }


    private ValidateProductItems() {
        if (this.entityPM.CustomerProductItems != null) {
            this.entityPM.CustomerProductItems.forEach(item => {
                Validator.TryValidateObject(item, "ProductItem", this.Errors);
                this.ValidateProductItemHTSCodes(item);

            });
        }
    }

    private ValidateProductItemHTSCodes(productItem: ProductItemPM) {

        if (productItem.HTSCodes != null) {
            productItem.HTSCodes.forEach(item => {
                Validator.TryValidateObject(item, "HTSCode", this.Errors);

                if (AppTool.IsNullOrEmpty(item.Code)) {
                    this.Errors.push("HTSCode Code is required");
                }

                if (AppTool.IsNullOrEmpty(item.DestinationCountryId)) {
                    this.Errors.push("HTSCode Country is required");
                }

            });
        }
    }

}

