import { browser, by, element, WebDriver, protractor } from 'protractor';
import { FieldsHelper } from '../../../Helpers/FieldsHelper';
import {ShipmentHelper} from '../ShipmentHelper';

export class HouseShipment {
  private Helper: FieldsHelper;
  private ShipmentModes:ShipmentHelper;


  constructor() {
    this.Helper = new FieldsHelper();
    this.ShipmentModes=new ShipmentHelper();
  }


  public CreateHouseShipment(ReferenceNumber: string,LogitudeShipType:string, Direction:any, TransportMode:any, ShipmentType:any) {

    var AWBToggle = this.Helper.WaitByIdAndClick('NEWSHIP');
    this.Helper.WaitByIdAndClick('NEWHOUSE');
    this.ShipmentModes.SelectDicrctionTransportMode(Direction, TransportMode, ShipmentType);

    this.FillHouseShipmentFields(ReferenceNumber);
    this.Helper.WaitByIdAndClick('ShipmentCreatebtn');
  }


  FillHouseShipmentFields(ShipperRef: string) {
    this.Helper.WaitByIdAndFill('Shipment_ShipperId', 'TestShipper');
    this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);

    this.Helper.WaitByIdAndFill('Shipment_ShipperReference1', ShipperRef);// test random number randomWholeNum

    this.Helper.WaitByIdAndFill('Shipment_ConsigneeId', 'TestConsignee');
    this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);

    if(ShipperRef!='CreatedFromMaster'){

    this.Helper.WaitByIdAndFill('Shipment_MainCarriageFromPortId', 'eze');
    this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);

    this.Helper.WaitByIdAndFill('Shipment_MainCarriageToPortId', 'mvd');
    this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);
  }
    this.Helper.WaitByIdAndFill('Shipment_IncotermId', 'TestIncoterm');
    this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);

    // this.Helper.WaitByIdAndFill('Shipment_MoveTypeId', 'TestMoveType');
    // this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);

    // this.Helper.WaitByIdAndFill('Shipment_DescriptionOfGoods', 'Protractor testing - Create New House ... ');
  }
}



