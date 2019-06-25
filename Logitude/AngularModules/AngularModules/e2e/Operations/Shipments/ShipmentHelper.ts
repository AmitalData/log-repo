import { browser, by, element, WebDriver, protractor } from 'protractor';
import { FieldsHelper } from './../../Helpers/FieldsHelper';

export class ShipmentHelper {
  private Helper: FieldsHelper;


  constructor() {
    this.Helper = new FieldsHelper();

  }

  SelectDicrctionTransportMode( Direction: string, TransportMode: string, ShipmentType: string) {
    var directionBtn: any;
    var transportModeBtn: any;
    var shipmentTypeBtn: any;
    var EC = protractor.ExpectedConditions;
    var directionID: string;
    var shipmentTypeID: string;

    // if (Direction == 'Export') {
    //   directionID = "DirectionRadio_0E";
    // }
    // else if (Direction == 'Import') {
    //   directionID == 'DirectionRadio_0I';
    // }
    // else if (Direction == 'Domestic') {
    //   directionID = 'DirectionRadio_0D';
    // }
    // else if (Direction == 'Drop') {
    //   directionID = 'DirectionRadio_0R';
    // }

    // if (ShipmentType == 'FCL') {
    //   shipmentTypeID = 'ShipmentTypeRadio_0FCLD';
    // }
    // else if (ShipmentType == 'LCL') {
    //   shipmentTypeID == 'ShipmentTypeRadio_0LCLD';
    // }
    // else if (ShipmentType == 'FTL') {
    //   shipmentTypeID = 'ShipmentTypeRadio_0FTL';
    // }
    // else if (ShipmentType == 'LTL') {
    //   shipmentTypeID = 'ShipmentTypeRadio_0LTL';
    // }
    // else
    //   shipmentTypeID = 'ShipmentTypeRadio_0MyGO';



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
        else
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
        else
        shipmentTypeBtn = element(by.id('ShipmentTypeRadio_0MyGO'));  
    
        browser.executeScript("arguments[0].click();", shipmentTypeBtn.getWebElement());

      });
    }
  }

  CreateAndCloseNewShipment(MasterDirectType: string, CancelBtnId: string,Direction:string,TransportMode:string, ShipmentType:string) {
    var AWBToggle = this.Helper.WaitByIdAndClick('NEWSHIP');
    this.Helper.WaitByIdAndClick(MasterDirectType);
    this.SelectDicrctionTransportMode( Direction,TransportMode, ShipmentType);

    this.Helper.WaitByIdAndClick(CancelBtnId);
  }

  OperationalCloseShipment() {
    this.Helper.WaitByIdAndClick('MenuButtons');
    this.Helper.WaitByIdAndClick('Shipment.B.OperationalClose');

    this.Helper.WaitByCssStringAndClick('.RedButton', 'Confirm');
    this.Helper.WaitBusyIndicator();
  }
  OperationalReopenShipment() {
    this.Helper.WaitByIdAndClick('MenuButtons');
    this.Helper.WaitByIdAndClick('Shipment.B.OperationalReopen');
    this.Helper.WaitByIdAndFill('EventNotes', 'Operational ReOpen - Protractor Testing .. ')

    this.Helper.WaitByCssStringAndClick('.RedButton', 'Confirm');
    this.Helper.WaitBusyIndicator();
    }
  AccountingCloseShipment() {
    this.Helper.WaitByIdAndClick('MenuButtons');
    this.Helper.WaitByIdAndClick('Shipment.B.AccountingClose');

    this.Helper.WaitByCssStringAndClick('.RedButton', 'Confirm');
    this.Helper.WaitBusyIndicator();
  }
  AccountedReopenShipment() {
    this.Helper.WaitByIdAndClick('MenuButtons');
    this.Helper.WaitByIdAndClick('Shipment.B.AccountedReopen');
    this.Helper.WaitByIdAndFill('EventNotes', 'Accounting ReOpen - Protractor Testing .. ');

    this.Helper.WaitByCssStringAndClick('.RedButton', 'Confirm');
    this.Helper.WaitBusyIndicator();
  }
  CopyShipment(){
    
    this.Helper.WaitByIdAndClick('MenuButtons');
    this.Helper.WaitByIdAndClick('Shipment.B.CopyShipment');


    // this.Helper.WaitByIdAndClick('Shipment.B.CopyShipment');
    // this.Helper.WaitByIdAndClick('CheckBox_0_5');//Include pickup

this.Helper.WaitByCssStringAndClick('.LogitudeCheckBox','Include PickUp')

    // this.Helper.WaitByIdAndClick('CheckBox_0_6');//Include Delivery
    // this.Helper.WaitByIdAndClick('CheckBox_0_7');//Include Flight
    // this.Helper.WaitByIdAndClick('CheckBox_0_8');//Include PreCarriage
    // this.Helper.WaitByIdAndClick('CheckBox_0_9');//Include OnCarriage

    this.Helper.WaitByIdAndClick('ShipmentCreatebtn');
    this.Helper.WaitBusyIndicator();


  }
} 