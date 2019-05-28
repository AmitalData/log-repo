import { LoginComp } from '../../Login/Login.po';
import { browser, by, element } from 'protractor';
import { DocsOutTabComponent } from './EditEntity/DocsOutTab';
import { OperationsComp } from './NewEntity/Operations.po';

import { SendMailPopup } from '../SendMailPopup/SendMailPopup';
import { PrintDocOut } from '../PrintDocOut/PrintDocOut';

describe('DocsOut', () => {

  let login: LoginComp = new LoginComp();
  let docsOutTab: DocsOutTabComponent = new DocsOutTabComponent();
  let sendMailPopup: SendMailPopup = new SendMailPopup();
  let NewDirectShipment: OperationsComp = new OperationsComp();
  let printDocOut: PrintDocOut = new PrintDocOut();


  beforeEach(() => {
    browser.driver.manage().window().maximize();
    browser.ignoreSynchronization = true;
  });


  it('Send Docs Out of Shipment To A User', function () {
    login.navigateTo('https://test.logitudeworld.com/test');
    login.DoLogin('raghad@protractor.com', '!RS123Rs');
    NewDirectShipment.DoOperations();
    docsOutTab.DocsOutTab();
    docsOutTab.QuickSearchDocOut('BRCL-S-DocsOut', 'BRCL-L-DocsOut', 'BOOKING REQUEST ');
    sendMailPopup.sendEmailToFirstUser();
  });

  it('Successfully Printing Document', function () {
    docsOutTab.QuickSearchDocOut('ETO-P-DocsOut', 'ETO-L-DocsOut', 'Export Trucking Order');
    printDocOut.isPrintingCompleted('BuildDocumentSucceededDiv', true);
  });

  it('Failing Printing Document', function () {
    docsOutTab.QuickSearchDocOut('FTDT-P-DocsOut', 'FTDT-L-DocsOut', 'Failure Test Document');
    printDocOut.isPrintingCompleted('BuildDocumentFailedDiv', false);
  });

});
