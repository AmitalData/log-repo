import { browser } from 'protractor';
import { ShipmentViewScenario } from './ShipmentViewScenario';
import { ShipmentView } from './ShipmentView';


describe('ShipmentView', () => {

    let ShipmentScenario: ShipmentViewScenario = new ShipmentViewScenario();
    let CreatView: ShipmentView = new ShipmentView();


    beforeEach(() => {

    });
    browser.ignoreSynchronization = true;


    it('OpenShipmentView', function () {

        ShipmentScenario.OpenShipmentView();

   });



   it('CreatShipmentView', function () {

      ShipmentScenario.CreatNewView();

   });

 
it('EditNewView', function () {

    ShipmentScenario.EditNewView();
});

 
 
 
 it('DeleteNewView', function () {

    ShipmentScenario.DeleteNewView();
});

});