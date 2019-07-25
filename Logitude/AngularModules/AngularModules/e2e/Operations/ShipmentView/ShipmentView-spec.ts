import { browser, by, element } from 'protractor';
import { ShipmentViewScenario } from './ShipmentViewScenario';
import { ShipmentView } from './ShipmentView';
import { LoginComp } from '../../Login/Login.po';
import { FieldsHelper } from '../../Helpers/FieldsHelper';



describe('ShipmentView', () => {

    let ShipmentScenario: ShipmentViewScenario = new ShipmentViewScenario();
    let CreatView: ShipmentView = new ShipmentView();


    beforeEach(() => {

    });
    browser.ignoreSynchronization = true;


    it('OpenShipmentView', function () {

        CreatView.OpenShipmentView();

   });



   it('CreatShipmentView', function () {

      CreatView.CreatNewView();

   });

 /*  it('DeleteNewView', function () {

    CreatView.DeleteNewView();
});


 it('EditNewView', function () {

       CreatView.EditNewView();
   });


   it('DeleteNewView', function () {

    CreatView.DeleteNewView();
});

  it('DisplayView', function () {

    CreatView.DisplayView();
});*/


});