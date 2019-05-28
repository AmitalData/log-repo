import { browser, by, element, WebDriver, protractor } from 'protractor';
import { FieldsHelper } from '../../../Helpers/FieldsHelper';
import { GeneralFunctions } from '../../../Helpers/GeneralFunctions';


export class PrintDocsOutTabComponent {
    private Helper: FieldsHelper;

    constructor() {
        this.Helper = new FieldsHelper();
    }

    public PrintDocsOutTab() {
        this.Helper.WaitByIdAndClick('Shipment.TH.DocsOut');
    }

    QuickSearchDocOut(docsOutId: string, docOutRow: string) {
         this.UseDocsOutSearchBox('SearchFieldsId_0_0','HAWB Lable ', docsOutId, docOutRow);
       }

       UseDocsOutSearchBox(searchFeildId: string, searchByRef: string, docOutId: string, docOutRow: string) {
        this.Helper.WaitByIdAndFill(searchFeildId, searchByRef);
        this.Helper.WaitByIdAndClick(docOutRow);
        this.Helper.WaitByIdAndClick(docOutId);
        }


        

}