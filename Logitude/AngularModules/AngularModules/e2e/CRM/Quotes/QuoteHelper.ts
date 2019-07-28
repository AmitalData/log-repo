import { browser, by, element, WebDriver, protractor } from 'protractor';
import { FieldsHelper } from './../../Helpers/FieldsHelper';

export class QuoteHelper {
  private Helper: FieldsHelper;


  constructor() {
    this.Helper = new FieldsHelper();

  }

  SelectDicrctionTransportMode( Direction: string, TransportMode: string, ShipmentType: string, QuoteType: string) {
    var directionBtn: any;
    var transportModeBtn: any;
    var shipmentTypeBtn: any;
    var EC = protractor.ExpectedConditions;
    var directionID: string;
    var shipmentTypeID: string;

    if (TransportMode == 'A' && ShipmentType == '') {

      var EC = protractor.ExpectedConditions;
      browser.wait(EC.elementToBeClickable(element(by.css('.RadioButton'))), 20000).then(a => {
        if(Direction == 'Export'){
          directionBtn = element(by.id('DirectionRadio_0E'));
  
        }
        else if(Direction == 'Import'){
          directionBtn = element(by.id('DirectionRadio_0I'));
  
        }
        else if(Direction == 'Domestic'){
          directionBtn = element(by.id('DirectionRadio_0D'));

        }
        else if(Direction == 'Drop'){
          directionBtn = element(by.id('DirectionRadio_0R'));
        }
        browser.executeScript("arguments[0].click();", directionBtn.getWebElement());

        transportModeBtn = element(by.id('TransportModeRadio_0A'));
        browser.executeScript("arguments[0].click();", transportModeBtn.getWebElement());
      });
    }
    else if (TransportMode == 'O' && ShipmentType != '') {

      var EC = protractor.ExpectedConditions;
      browser.wait(EC.elementToBeClickable(element(by.css('.RadioButton'))), 20000).then(a => {
        if(Direction == 'Export'){
          directionBtn = element(by.id('DirectionRadio_0E'));
  
        }
        else if(Direction == 'Import'){
          directionBtn = element(by.id('DirectionRadio_0I'));
  
        }
        else if(Direction == 'Domestic'){
          directionBtn = element(by.id('DirectionRadio_0D'));

        }
        else if(Direction == 'Drop'){
          directionBtn = element(by.id('DirectionRadio_0R'));
        }
        browser.executeScript("arguments[0].click();", directionBtn.getWebElement());
        
        transportModeBtn = element(by.id('TransportModeRadio_0O'));
        browser.executeScript("arguments[0].click();", transportModeBtn.getWebElement());

        if(ShipmentType == 'FCL'){
          shipmentTypeBtn = element(by.id('ShipmentTypeRadio_0FCLD'));  
        }
        else if (ShipmentType == 'LCL') {
          shipmentTypeBtn = element(by.id('ShipmentTypeRadio_0LCLD'));  
        }
        else if(ShipmentType == 'OG')
        shipmentTypeBtn = element(by.id('ShipmentTypeRadio_0MyGO'));  
    
        browser.executeScript("arguments[0].click();", shipmentTypeBtn.getWebElement());

      });
    }
    else if (TransportMode == 'I' && ShipmentType != '') {

      var EC = protractor.ExpectedConditions;
      browser.wait(EC.elementToBeClickable(element(by.css('.RadioButton'))), 20000).then(a => {
        if(Direction == 'Export'){
          directionBtn = element(by.id('DirectionRadio_0E'));
  
        }
        else if(Direction == 'Import'){
          directionBtn = element(by.id('DirectionRadio_0I'));
  
        }
        else if(Direction == 'Domestic'){
          directionBtn = element(by.id('DirectionRadio_0D'));

        }
        else if(Direction == 'Drop'){
          directionBtn = element(by.id('DirectionRadio_0R'));
        }
        browser.executeScript("arguments[0].click();", directionBtn.getWebElement());
        
        transportModeBtn = element(by.id('TransportModeRadio_0I'));
        browser.executeScript("arguments[0].click();", transportModeBtn.getWebElement());

        if (ShipmentType == 'FTL') {
          shipmentTypeBtn = element(by.id('ShipmentTypeRadio_0FTL'));  
        }
        else if (ShipmentType == 'LTL') {
          shipmentTypeBtn = element(by.id('ShipmentTypeRadio_0LTL'));  
        }
        else if((ShipmentType == 'IG'))
        shipmentTypeBtn = element(by.id('ShipmentTypeRadio_0MyGI'));  
    
        browser.executeScript("arguments[0].click();", shipmentTypeBtn.getWebElement());

      });
    }
  }

  CreateAndCloseNewQuote(Direction:string,TransportMode:string, ShipmentType:string) {
    this.Helper.WaitByIdAndClick('NewQuote');
    this.SelectDicrctionTransportMode( Direction,TransportMode,ShipmentType);

    this.Helper.WaitByIdAndClick("CancelQuote");
  }

}