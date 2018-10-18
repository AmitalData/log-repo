import { browser, by, element, WebDriver, protractor } from 'protractor';
import { FieldsHelper } from '../../../Helpers/FieldsHelper';

export class DocsOutTabComponent {
    private Helper: FieldsHelper;

    constructor() {
        this.Helper = new FieldsHelper();
    }

    public DocsOutTab() {
        this.Helper.WaitByIdAndClick('Shipment.TH.DocsOut');
        this.Helper.WaitByIdAndClick('716SD-P-DocsOut');
        this.Helper.WaitByCssButtonClick('.Button', 'Close');
    }
}