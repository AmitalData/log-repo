import { OperationsComp } from './Operations.po';
import { browser, by, element } from 'protractor';
import { LoginComp } from '../../../Login/Login.po';
//import { GeneralFunctions } from '../../../Helpers/GeneralFunctions';
import { EditTabsComponent } from '../EditEntity/EditShipmentTabs.po';


describe('Operations Module', () => {
    let page: OperationsComp = new OperationsComp();
    let EditShipmentTabs: EditTabsComponent;

    var shipperRef1;
    afterEach(() => {
        // browser.switchTo().alert().accept();

    })

    it('Create Shipment .. ', function () {
        browser.ignoreSynchronization = true;
        shipperRef1 = page.DoOperations();
        console.log(shipperRef1);
    });
    it('Search For shipment ..', function () {
        console.log('inside the search it : ' + shipperRef1);
        browser.ignoreSynchronization = true;
        page.SearchForShipment(shipperRef1);
    });
    it('Edit Shipment .. ', function () {
        browser.ignoreSynchronization = true;
        page.EditShipment(shipperRef1);
        page.SaveShip();
        //page.DoShipmentAction('OC');
    });
});
