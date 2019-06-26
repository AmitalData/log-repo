import { LoginComp } from '../../Login/Login.po';
import { browser, by, element } from 'protractor';
import { DocsOutTabComponent } from './EditEntity/DocsOutTab';
import { OperationsComp } from './NewEntity/Operations.po';
import { SendMailPopup } from '../SendMailPopup/SendMailPopup';
import { PrintDocOut } from '../PrintDocOut/PrintDocOut';
import { FieldsHelper } from '../../Helpers/FieldsHelper';



export class DocOutSenario {


     private login: LoginComp = new LoginComp();
    //  private docsOutTab: DocsOutTabComponent = new DocsOutTabComponent();
     // private sendMailPopup: SendMailPopup = new SendMailPopup();
      private NewDirectShipment: OperationsComp = new OperationsComp();
     // private printDocOut: PrintDocOut = new PrintDocOut();
     // private helper = new FieldsHelper();



    constructor() {

    }

    public SuccessfullyPrintingDocument() {

     // this.NewDirectShipment.DoOperations();
      //this.helper.WaitBusyIndicator();
      //this.docsOutTab.DocsOutTab();
      //this.docsOutTab.QuickSearchDocOut('ETO-P-DocsOut', 'ETO-L-DocsOut', 'Export Trucking Order');
      //this.printDocOut.isPrintingCompleted('BuildDocumentSucceededDiv', true);


    }

    public FailingPrintingDocument() {
      //this.docsOutTab.QuickSearchDocOut('FTDT-P-DocsOut', 'FTDT-L-DocsOut', 'Failure Test Document');
      //this.printDocOut.isPrintingCompleted('BuildDocumentFailedDiv', false);

    }   


}
