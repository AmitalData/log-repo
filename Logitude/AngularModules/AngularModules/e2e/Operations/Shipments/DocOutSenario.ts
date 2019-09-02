import { LoginComp } from '../../Login/Login.po';
import { browser, by, element } from 'protractor';
import { DocsOutTabComponent } from './EditEntity/DocsOutTab';
import { OperationsComp } from './NewEntity/Operations.po';
import { SendMailPopup } from '../SendMailPopup/SendMailPopup';
import { PrintDocOut } from '../PrintDocOut/PrintDocOut';
import { FieldsHelper } from '../../Helpers/FieldsHelper';



export class DocOutSenario {


  private login: LoginComp = new LoginComp();
  private NewDirectShipment: OperationsComp = new OperationsComp();
  private docsOutTab: DocsOutTabComponent = new DocsOutTabComponent();
  private sendMailPopup: SendMailPopup = new SendMailPopup();
  private printDocOut: PrintDocOut = new PrintDocOut();
  private helper = new FieldsHelper();



  constructor() {

  }

  public OpenDocOutTab() {
    this.NewDirectShipment.DoOperations();
    this.docsOutTab.DocsOutTab();
    this.helper.WaitBusyIndicator();

  }

  public SuccessfullyPrintingDocument() {
    this.docsOutTab.QuickSearchDocOut('ETO-P-DocsOut', 'ETO-L-DocsOut', 'Export Trucking Order');
    this.printDocOut.isPrintingCompleted('BuildDocumentSucceededDiv', true);
    this.helper.WaitWindowClosed()
    this.helper.WaitBusyIndicator();


  }

  public FailingPrintingDocument() {

    this.helper.WaitBusyIndicator();
    this.docsOutTab.QuickSearchDocOut('FTDT-P-DocsOut', 'FTDT-L-DocsOut', 'Failure Test Document');
    this.helper.WaitBusyIndicator();
    this.printDocOut.isPrintingCompleted('BuildDocumentFailedDiv', false);

  }

}
