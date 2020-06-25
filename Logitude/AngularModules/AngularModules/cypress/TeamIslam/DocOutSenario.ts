/// <reference types="cypress" />

import { LoginComp } from '../../Login/Login.po';
import { DocsOutTabComponent } from './DocsOutTab';
import { CreateEditShipment } from './CreateEditShipment';
import { SendMailPopup } from './SendMailPopup';
import { PrintDocOut } from './PrintDocOut';



export class DocOutSenario {


  private login: LoginComp = new LoginComp();
  private NewDirectShipment: CreateEditShipment = new CreateEditShipment();
  private docsOutTab: DocsOutTabComponent = new DocsOutTabComponent();
  private sendMailPopup: SendMailPopup = new SendMailPopup();
  private printDocOut: PrintDocOut = new PrintDocOut();

  



  constructor() {

  }
  public OpenDocOutTab() {
    var shipperRef;
      shipperRef = this.NewDirectShipment.C();
    this.NewDirectShipment.SearchForShipment(shipperRef);
      this.NewDirectShipment.EditShipment(shipperRef, browser.params.ShipParams.ShipmentLevelCode, browser.params.ShipParams.ShipmentType, browser.params.ShipParams.Direction);

}

    public SuccessfullyPrintingDocument() {

    this.helper.WaitEditComponentBusyIndicator();
    this.docsOutTab.QuickSearchDocOut('ETO-P-DocsOut', 'ETO-L-DocsOut', 'Export Trucking Order');
    this.printDocOut.isPrintingCompleted('BuildDocumentSucceededDiv', true);
    this.helper.WaitWindowClosed()
    this.helper.WaitBusyIndicator();


  }

  public FailingPrintingDocument() {

    // this.helper.WaitBusyIndicator();
    this.helper.WaitEditComponentBusyIndicator();
    this.helper.WaitWindowClosed();
    this.docsOutTab.QuickSearchDocOut('FTDT-P-DocsOut', 'FTDT-L-DocsOut', 'Failure Test Document');
    this.helper.WaitBusyIndicator();
    this.printDocOut.isPrintingCompleted('BuildDocumentFailedDiv', false);

  }

}
