import { browser, by, element, WebDriver, protractor } from 'protractor';
import { FieldsHelper } from '../../../Helpers/FieldsHelper';
import { GeneralFunctions } from '../../../Helpers/GeneralFunctions';


export class DocsOutTabComponent {
    private Helper: FieldsHelper;

    constructor() {
        this.Helper = new FieldsHelper();
    }

    public DocsOutTab() {
        this.Helper.WaitByIdAndClick('Shipment.TH.DocsOut');
    }

    QuickSearchDocOut(docsOutId) {
         this.UseDocsOutSearchBox('SearchFieldsId_0_0','BOOKING REQUEST ', docsOutId);
       }

       UseDocsOutSearchBox(searchFeildId: string, searchByRef: string, docOutId: string) {
        this.Helper.WaitByIdAndFill(searchFeildId, searchByRef);
        this.Helper.WaitByIdAndClick(docOutId);
        }

}