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

      //  CreatView.OpenShipmentView();

        ShipmentScenario.OpenShipmentView();

   });



   it('CreatShipmentView', function () {

     // CreatView.CreatNewView();
      ShipmentScenario.CreatNewView();

   });

 
it('EditNewView', function () {

  //  CreatView.EditNewView();
    ShipmentScenario.EditNewView();
});

 
 
 
 //it('DeleteNewView', function () {

 //   CreatView.DeleteNewView();
    //ShipmentScenario.DeleteNewView();
//});



 /* it('DisplayView', function () {

    CreatView.DisplayView();
});*/


});