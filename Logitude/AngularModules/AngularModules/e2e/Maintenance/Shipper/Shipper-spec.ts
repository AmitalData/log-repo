import { browser, by, element } from 'protractor';
import { NewShipper } from './Shipper';
import { NewShipperScenario } from './ShipperScenario';
import { LoginComp } from "../../login/Login.po";





describe('NewShipper', () => {

    let Newshipper: NewShipper = new NewShipper();
    let ShipperScenario: NewShipperScenario = new NewShipperScenario();

    beforeEach(() => {

    });

    browser.ignoreSynchronization = true;

    it('QuickSearch', function () {

        ShipperScenario.Quicksearch();
    });

    it('SearchShippertTab', function () {

        ShipperScenario.SearchShippertTab();
    });


    it('CreateNewShipper', function () {


        ShipperScenario.CreateNewShipper();
    });

    it('SearchShipper', function () {


        ShipperScenario.SearchShipper();
    });

    it('EditOnShipper', function () {
        ShipperScenario.EditOnShipper();

    });


});
