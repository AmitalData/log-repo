import { browser, by, element, WebDriver, protractor } from 'protractor';
import { FieldsHelper } from '../../../Helpers/FieldsHelper';

export class RoutingTabComponent {
  private Helper: FieldsHelper;

  constructor() {
    this.Helper = new FieldsHelper();
  }

  public RoutingTab(LogitudeShipType: any, ShipmentType: any) {
    this.Helper.WaitByIdAndClick('Shipment.TH.Routings');
    this.AddPickup();
    this.Helper.WaitBusyIndicator();
    if (LogitudeShipType == 'D' || LogitudeShipType == 'H') {
      this.AddPreCarriage(ShipmentType);
      this.AddOnCarriage(ShipmentType);
    }
    this.EditMainCarriage(LogitudeShipType, ShipmentType);
    // this.AddDelivery();
    // this.Helper.WaitBusyIndicator();

  }


  private EditMainCarriage(LogitudeShipType: string, ShipmentType: any) {
    this.Helper.WaitByIdAndClick('Edit-MainCarriage');

    if (LogitudeShipType == 'D' || LogitudeShipType == 'M') {

      if (ShipmentType == '') {
        // this.Helper.WaitBusyIndicator();
        // browser.driver.sleep(50000);
      
        //------------------------- GET FROM STOCK ------------------------------
        //------------------------- GET FROM STOCK ------------------------------
        // this.Helper.WaitByIdAndClick('GetFromStockBtn');
        // // this.Helper.WaitByCssButtonClick('.Button','Get from stock')
        // var EC = protractor.ExpectedConditions;
        // // browser.wait(EC.elementToBeClickable(element(by.id('GetFromStockBtn'))), 40000).then(a => {
        // //   browser.wait(EC.elementToBeClickable(element(by.id('GetFromStockBtn'))), 40000).then(a => {
        // //     element(by.id('GetFromStockBtn')).click();
        // //   });
        // // });

        // browser.wait(EC.elementToBeClickable(element(by.css(".SimpleGridViewRow"))), 100000).then(a => {
        // });
        // element.all(by.css('.SimpleGridViewRow')).get(1).click();

        // this.Helper.WaitByIdAndClick('OkBtn');
        // this.Helper.WaitByIdAndClick('ConfirmWindow_Yes_0');
        // this.Helper.WaitBusyIndicator();
        //------------------------- END GET FROM STOCK ------------------------------
        //------------------------- END GET FROM STOCK ------------------------------

        var transshipment1FromPortId = this.Helper.WaitByIdAndFill('Shipment_Transshipment1FromPortId', 'am');
        this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);

        var transshipment1CarrierId = this.Helper.WaitByIdAndFill('Shipment_Transshipment1CarrierId', 'rj');
        this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);

        this.Helper.WaitByIdAndFill('Shipment_Transshipment1CarrierNumber', '125');

        var transshipment2FromPortId = this.Helper.WaitByIdAndFill('Shipment_Transshipment2FromPortId', 'ab');
        this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);

        var transshipment2CarrierId = this.Helper.WaitByIdAndFill('Shipment_Transshipment2CarrierId', 'am');
        this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);

        this.Helper.WaitByIdAndFill('Shipment_Transshipment2CarrierNumber', '126');

        var transshipment3FromPortId = this.Helper.WaitByIdAndFill('Shipment_Transshipment3FromPortId', 'am');
        this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);

        var transshipment3CarrierId = this.Helper.WaitByIdAndFill('Shipment_Transshipment3CarrierId', 'lh');
        this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);

        this.Helper.WaitByIdAndFill('Shipment_Transshipment3CarrierNumber', '127');

        this.Helper.WaitByIdAndFill('date_Shipment_MainCarriageATD', '1');
        this.Helper.WaitByIdAndFill('date_Shipment_MainCarriageATA', '1');

        this.Helper.WaitByIdAndClick('MainCarriageOKBtn');
      }
      else if (ShipmentType == 'FCL' || ShipmentType == 'LCL') {
        // First leg details
        var mainCarriageVessel = this.Helper.WaitByIdAndFill('Shipment_MainCarriageVesselId', 'log');
        this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);

        this.Helper.WaitByIdAndFill('date_Shipment_MainCarriageATD', '1');
        this.Helper.WaitByIdAndFill('date_Shipment_MainCarriageATA', '1');



        var transshipment1FromPortId = this.Helper.WaitByIdAndFill('Shipment_Transshipment1FromPortId', '225');
        this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);

        var transshipment1CarrierId = this.Helper.WaitByIdAndFill('Shipment_Transshipment1CarrierId', 'aclu');
        this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);

        this.Helper.WaitByIdAndFill('Shipment_Transshipment1CarrierNumber', '125');

        var transshipment2FromPortId = this.Helper.WaitByIdAndFill('Shipment_Transshipment2FromPortId', '45b');
        this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);

        var transshipment2CarrierId = this.Helper.WaitByIdAndFill('Shipment_Transshipment2CarrierId', 'balu');
        this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);

        this.Helper.WaitByIdAndFill('Shipment_Transshipment2CarrierNumber', '126');

        var transshipment3FromPortId = this.Helper.WaitByIdAndFill('Shipment_Transshipment3FromPortId', 'w2w');
        this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);

        var transshipment3CarrierId = this.Helper.WaitByIdAndFill('Shipment_Transshipment3CarrierId', 'votu');
        this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);

        this.Helper.WaitByIdAndFill('Shipment_Transshipment3CarrierNumber', '127');

        this.Helper.WaitByIdAndClick('MainCarriageOKBtn');
      }
      else if (ShipmentType == 'LTL' || ShipmentType == 'FTL') {
        var transshipment1FromPortId = this.Helper.WaitByIdAndFill('Shipment_Transshipment1FromPortId', 'du');
        this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);

        var transshipment1CarrierId = this.Helper.WaitByIdAndFill('Shipment_Transshipment1CarrierId', 'trk1');
        this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);

        this.Helper.WaitByIdAndFill('Shipment_Transshipment1CarrierNumber', '125');

        var transshipment2FromPortId = this.Helper.WaitByIdAndFill('Shipment_Transshipment2FromPortId', 'fra');
        this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);

        var transshipment2CarrierId = this.Helper.WaitByIdAndFill('Shipment_Transshipment2CarrierId', 'trk2');
        this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);

        this.Helper.WaitByIdAndFill('Shipment_Transshipment2CarrierNumber', '126');

        var transshipment3FromPortId = this.Helper.WaitByIdAndFill('Shipment_Transshipment3FromPortId', 'bras');
        this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);

        var transshipment3CarrierId = this.Helper.WaitByIdAndFill('Shipment_Transshipment3CarrierId', 'trk3');
        this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);

        this.Helper.WaitByIdAndFill('Shipment_Transshipment3CarrierNumber', '127');

        this.Helper.WaitByIdAndClick('MainCarriageOKBtn');
      }

    }
    else if (LogitudeShipType == 'H') {
      var transshipment1FromPortId = this.Helper.WaitByIdAndFill('Shipment_MainCarriageFromPortId', 't');
      this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);

      var transshipment1CarrierId = this.Helper.WaitByIdAndFill('Shipment_MainCarriageToPortId', 'i');
      this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);


      this.Helper.WaitByIdAndClick('OkBtn');

    }



  }

  private AddPickup() {

    this.Helper.WaitByIdAndClick('Add-PickUp');
    this.Helper.WaitByIdAndClick('Port_FromRadio');
    this.Helper.WaitByIdAndFill('ShipmentPickUpDelivery_FromPortId', 'eze');
    this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);


    this.Helper.WaitByIdAndClick('Port_ToRadio');
    this.Helper.WaitByIdAndFill('ShipmentPickUpDelivery_ToPortId', 'mvd');
    this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);

    var trucker = this.Helper.WaitByIdAndFill('ShipmentPickUpDelivery_CarrierId', 't');
    this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);

    this.Helper.WaitByIdAndFill('ShipmentPickUpDelivery_CarrierNumber', '777');
    this.Helper.WaitByIdAndFill('ShipmentPickUpDelivery_Driver', 'Trucker Driver');
    this.Helper.WaitByIdAndFill('ShipmentPickUpDelivery_TruckNumber', 'Transp Doc');
    this.Helper.WaitByIdAndFill('ShipmentPickUpDelivery_TrailerNumber', 'Trail No.');
    this.Helper.WaitByIdAndFill('ShipmentPickUpDelivery_Notes', 'Adding Pickup ');

    this.Helper.WaitByIdAndClick('ShipmentPickUpDelivery.TH.Packages');
    this.Helper.WaitByIdAndClick('Add');
    this.Helper.WaitByIdAndFill('ShipmentPickUpDeliveryPackage_Quantity', '10');
    this.Helper.WaitByIdAndFill('ShipmentPickUpDeliveryPackage_Weight', '100');
    this.Helper.WaitByCssButtonClick('.RedButton', 'Ok');


    this.Helper.WaitByIdAndClick('SaveBtn');
    this.Helper.WaitBusyIndicator();
    this.Helper.WaitByIdAndClick('CloseBtn');
  }

  private AddPreCarriage(ShipmentType: string) {

    this.Helper.WaitByIdAndClick('RoutingToggle');
    this.Helper.WaitByIdAndClick('PreCarriage');
    if (ShipmentType == '') {
      var preCarriageTransportMode = this.Helper.WaitByIdAndFill('Shipment_PreCarriageTransportModeId', 'a');
      this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);


      var preCarriageFromPort = this.Helper.WaitByIdAndFill('Shipment_PreCarriageFromPortId', 'a');
      this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);

      var preCarriageToPort = this.Helper.WaitByIdAndFill('Shipment_PreCarriageToPortId', 'is');
      this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);

      var preCarriageCarrier = this.Helper.WaitByIdAndFill('Shipment_PreCarriageCarrierId', 'sa');
      this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);

      this.Helper.WaitByIdAndFill('Shipment_PreCarriageCarrierNumber', '985');
      this.Helper.WaitByIdAndClick('PreCarriageOKBtn');
    }
    else if (ShipmentType == 'FCL' || ShipmentType == 'LCL') {
      var preCarriageTransportMode = this.Helper.WaitByIdAndFill('Shipment_PreCarriageTransportModeId', 'o');
      this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);


      var preCarriageFromPort = this.Helper.WaitByIdAndFill('Shipment_PreCarriageFromPortId', 'be');
      this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);

      var preCarriageToPort = this.Helper.WaitByIdAndFill('Shipment_PreCarriageToPortId', 'isa');
      this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);

      var preCarriageCarrier = this.Helper.WaitByIdAndFill('Shipment_PreCarriageCarrierId', 'a');
      this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);

      this.Helper.WaitByIdAndFill('Shipment_PreCarriageCarrierNumber', '985');
      this.Helper.WaitByIdAndClick('PreCarriageOKBtn');
    }
    else if (ShipmentType == 'FTL' || ShipmentType == 'LTL') {
      var preCarriageTransportMode = this.Helper.WaitByIdAndFill('Shipment_PreCarriageTransportModeId', 'I');
      this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);


      var preCarriageFromPort = this.Helper.WaitByIdAndFill('Shipment_PreCarriageFromPortId', 'antalya');
      this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);

      var preCarriageToPort = this.Helper.WaitByIdAndFill('Shipment_PreCarriageToPortId', 'abk');
      this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);

      var preCarriageCarrier = this.Helper.WaitByIdAndFill('Shipment_PreCarriageCarrierId', 'trk1');
      this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);

      this.Helper.WaitByIdAndFill('Shipment_PreCarriageCarrierNumber', '985');
      this.Helper.WaitByIdAndClick('PreCarriageOKBtn');
    }

  }

  private AddOnCarriage(ShipmentType: string) {
    this.Helper.WaitByIdAndClick('RoutingToggle');
    this.Helper.WaitByIdAndClick('OnCarriage');

    if (ShipmentType == '') {
      var preCarriageTransportMode = this.Helper.WaitByIdAndFill('Shipment_OnCarriageTransportModeId', 'a');
      this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);


      var preCarriageFromPort = this.Helper.WaitByIdAndFill('Shipment_OnCarriageFromPortId', 'a');
      this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);

      var preCarriageToPort = this.Helper.WaitByIdAndFill('Shipment_OnCarriageToPortId', 'is');
      this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);

      var preCarriageCarrier = this.Helper.WaitByIdAndFill('Shipment_OnCarriageCarrierId', 'sa');
      this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);

      this.Helper.WaitByIdAndFill('Shipment_OnCarriageCarrierNumber', '985');
      this.Helper.WaitByIdAndClick('OnCarriageOKBtn');
    }
    else if (ShipmentType == 'FCL' || ShipmentType == 'LCL') {
      var preCarriageTransportMode = this.Helper.WaitByIdAndFill('Shipment_OnCarriageTransportModeId', 'o');
      this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);


      var preCarriageFromPort = this.Helper.WaitByIdAndFill('Shipment_OnCarriageFromPortId', 'be');
      this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);

      var preCarriageToPort = this.Helper.WaitByIdAndFill('Shipment_OnCarriageToPortId', 'isa');
      this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);

      var preCarriageCarrier = this.Helper.WaitByIdAndFill('Shipment_OnCarriageCarrierId', 'f');
      this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);

      this.Helper.WaitByIdAndFill('Shipment_OnCarriageCarrierNumber', '985');
      this.Helper.WaitByIdAndClick('OnCarriageOKBtn');
    }
    else if (ShipmentType == 'FTL' || ShipmentType == 'LTL') {
      var preCarriageTransportMode = this.Helper.WaitByIdAndFill('Shipment_OnCarriageTransportModeId', 'I');
      this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);


      var preCarriageFromPort = this.Helper.WaitByIdAndFill('Shipment_OnCarriageFromPortId', 'antalya');
      this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);

      var preCarriageToPort = this.Helper.WaitByIdAndFill('Shipment_OnCarriageToPortId', 'abk');
      this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);

      var preCarriageCarrier = this.Helper.WaitByIdAndFill('Shipment_OnCarriageCarrierId', 'trk1');
      this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);

      this.Helper.WaitByIdAndFill('Shipment_OnCarriageCarrierNumber', '985');
      this.Helper.WaitByIdAndClick('OnCarriageOKBtn');
    }
  }

  private AddDelivery() {
    this.Helper.WaitByIdAndClick('Add-Delivery');

    var trucker = this.Helper.WaitByIdAndFill('ShipmentPickUpDelivery_CarrierId', 't');
    this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);

    this.Helper.WaitByIdAndFill('ShipmentPickUpDelivery_CarrierNumber', '777');
    this.Helper.WaitByIdAndFill('ShipmentPickUpDelivery_Driver', 'Trucker Driver');
    this.Helper.WaitByIdAndFill('ShipmentPickUpDelivery_TruckNumber', 'Transp Doc');
    this.Helper.WaitByIdAndFill('ShipmentPickUpDelivery_TrailerNumber', 'Trail No.');
    this.Helper.WaitByIdAndFill('ShipmentPickUpDelivery_Notes', 'Adding Pickup ');

    this.Helper.WaitByIdAndClick('SaveBtn');
    this.Helper.WaitBusyIndicator();
    this.Helper.WaitByIdAndClick('CloseBtn');
  }



}

