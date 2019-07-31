import { LogboxShipment } from './CreateLogboxShipment';
import { ShipmentSearch } from './ShipmentSearch';
import { browser, by, element } from 'protractor';
import { GeneralFunctions } from '../Helpers/GeneralFunctions';

describe('Shipment search', () => {
    let shipmentSearch: ShipmentSearch = new ShipmentSearch();
    let logboxShipment = new LogboxShipment();
    let GeneralFun = new GeneralFunctions();

  beforeEach(() => {
      browser.driver.manage().window().maximize();
      browser.ignoreSynchronization = true;

  });

 
  it('Create Shipment successfully', function () {
      shipmentSearch.QuickSearch();
      var orderNumber = GeneralFun.RandomNum();

      logboxShipment.CreateShip(orderNumber);
      logboxShipment.SearchForCreatedShipment('SearchFieldsId_0_0', orderNumber);
      logboxShipment.AddDocument();
      //logboxShipment.ClickNewShipment();
      //logboxShipment.ChooseTransportMode();
      //var orderNo= logboxShipment.InsertOrderNumber();
      //logboxShipment.InsertAgent();
      //logboxShipment.SaveShipment();
      ////logboxShipment.SearchForCreatedShipment();
      //logboxShipment.SelectShipment();
      //logboxShipment.CreateDocument();

      


  });
});
