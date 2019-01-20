import { browser, by, element, WebDriver, protractor } from 'protractor';
import { FieldsHelper } from '../../../Helpers/FieldsHelper';
import { ShipmentHelper } from '../ShipmentHelper';
export class DirectShipment {
  private Helper: FieldsHelper;
  private ShipmentModes: ShipmentHelper;

  constructor() {
    this.Helper = new FieldsHelper();
    this.ShipmentModes = new ShipmentHelper();
  }


  public CreateDirectShipment(ReferenceNumber: string, LogitudeShipType: string, Direction: string, TransportMode: string, ShipmentType: string) {

    var AWBToggle = this.Helper.WaitByIdAndClick('NEWSHIP');
    this.Helper.WaitByIdAndClick('NEWDIRECT');
    this.ShipmentModes.SelectDicrctionTransportMode(LogitudeShipType, Direction, TransportMode, ShipmentType);



    this.FillDirectShipmentFields(ReferenceNumber);

    this.Helper.WaitByIdAndClick('ShipmentCreatebtn');
  }

  public CreateAndCloseNewShipment(){
    var AWBToggle = this.Helper.WaitByIdAndClick('NEWSHIP');
    this.Helper.WaitByIdAndClick('NEWDIRECT');
    this.Helper.WaitByIdAndClick('ShipmentCancelbtn');
  }



  FillDirectShipmentFields(ShipperRef: string) {
    this.Helper.WaitByIdAndFill('Shipment_ShipperId', 'a');
    this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 1);

    this.Helper.WaitByIdAndFill('Shipment_ShipperReference1', ShipperRef);// test random number randomWholeNum


    this.Helper.WaitByIdAndFill('Shipment_ConsigneeId', 'r');
    this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 2);

    this.Helper.WaitByIdAndFill('Shipment_MainCarriageFromPortId', 'eze');
    // browser.driver.sleep(1000);
    this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);

    this.Helper.WaitByIdAndFill('Shipment_MainCarriageToPortId', 'mvd');
    this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);

    this.Helper.WaitByIdAndFill('Shipment_MainCarriageCarrierId', 'a');
    this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 2);

    this.Helper.WaitByIdAndFill('Shipment_MainCarriageCarrierNumber', '115');

    this.Helper.WaitByIdAndFill('Shipment_IncotermId', 'r');
    this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 1);

    this.Helper.WaitByIdAndFill('Shipment_MoveTypeId', 'd');
    this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 1);

    // this.Helper.WaitByIdAndClick('Shipment_OrderIsDangerouseGoods');
    this.Helper.WaitByIdAndFill('Shipment_DescriptionOfGoods', 'Protractor testing - Create New Direct Shipment ... ');


  }

}



