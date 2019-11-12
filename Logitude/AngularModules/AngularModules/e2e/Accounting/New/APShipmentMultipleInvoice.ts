import { browser, by, element, WebDriver, protractor } from 'protractor';
import { FieldsHelper } from '../Helpers/FieldsHelper';
import { GeneralFunctions } from '../Helpers/GeneralFunctions';


export class APShipmentMultipleInvoiceComponent {
    private Helper: FieldsHelper;
    constructor() {
        this.Helper = new FieldsHelper();
    }
}
