import { LoginComp } from '../../Login/Login.po';
import { browser, by, element } from 'protractor';
import { ShipmentSearch } from '../ShipmentSearch';
import { DocsOutTabComponent } from './EditEntity/DocsOutTab';
import { OperationsComp } from './NewEntity/Operations.po';

import { SendMailPopup } from '../SendMailPopup/SendMailPopup';

describe('DocsOut', () => {

  //let searchPage: ShipmentSearch = new ShipmentSearch();
  let docsOutTab: DocsOutTabComponent = new DocsOutTabComponent();
  let sendMailPopup: SendMailPopup = new SendMailPopup();
  let NewDirectShipment: OperationsComp = new OperationsComp();


  beforeEach(() => {
    browser.driver.manage().window().maximize();
    browser.ignoreSynchronization = true;
  });


  it('Send Docs Out of Shipment To A User', function () {
    NewDirectShipment.DoOperations();
    docsOutTab.DocsOutTab();
    docsOutTab.QuickSearchDocOut('BRCL-S-DocsOut');
    sendMailPopup.sendEmailToFirstUser();
    // TODO We need to check if email is delivered to the destination
  });
});
