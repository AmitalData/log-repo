import { LogboxShipment } from './CreateLogboxShipment';
import { ShipmentSearch } from './ShipmentSearch';
import { browser, by, element } from 'protractor';
import { GeneralFunctions } from '../Helpers/GeneralFunctions';

describe('Shipment search', () => {
    let shipmentSearch: ShipmentSearch = new ShipmentSearch();
    let logboxShipment = new LogboxShipment();

  beforeEach(() => {
      browser.driver.manage().window().maximize();
      browser.ignoreSynchronization = true;

  });

 
  it('Create Shipment successfully', function () {
      shipmentSearch.QuickSearch();
      logboxShipment.ClickNewShipment();
      logboxShipment.ChooseTransportMode();
      logboxShipment.InsertOrderNumber();
      logboxShipment.InsertAgent();
      logboxShipment.SaveShipment();
      //logboxShipment.SearchForCreatedShipment();
      logboxShipment.SelectShipment();
      logboxShipment.CreateDocument();

      


  });
});
