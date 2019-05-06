import { browser, by, element, WebDriver, protractor } from 'protractor';
import { FieldsHelper } from '../../../Helpers/FieldsHelper';
import {ShipmentHelper} from '../ShipmentHelper';

export class MasterShipment {
  private Helper: FieldsHelper;
  private ShipmentModes:ShipmentHelper;


  constructor() {
    this.Helper = new FieldsHelper();
    this.ShipmentModes=new ShipmentHelper();

  }


  public CreateMasterShipment(ReferenceNumber: string,LogitudeShipType:string, Direction:any, TransportMode:any, ShipmentType:any) {

    var AWBToggle = this.Helper.WaitByIdAndClick('NEWSHIP');
    this.Helper.WaitByIdAndClick('NEWMASTER');
    this.ShipmentModes.SelectDicrctionTransportMode( Direction,TransportMode, ShipmentType);

    this.FillMasterShipmentFields(ReferenceNumber);
    this.Helper.WaitByIdAndClick('MasterCreatebtn');

  }


  FillMasterShipmentFields(ShipperRef: string) {

    this.Helper.WaitByIdAndFill('Master_AgentId', 'a');
    this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 1);

    this.Helper.WaitByIdAndFill('Master_AgentReference1_1', ShipperRef);// test random number randomWholeNum

    this.Helper.WaitByIdAndFill('Master_MainCarriageFromPortId', 'amm');
    this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);

    this.Helper.WaitByIdAndFill('Master_MainCarriageToPortId', 'abu');
    this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);

    this.Helper.WaitByIdAndFill('Master_MainCarriageCarrierId', 'a');
    this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);

    this.Helper.WaitByIdAndFill('Master_MainCarriageCarrierNumber', '115')

    this.Helper.WaitByIdAndFill('Master_DescriptionOfGoods', 'Protractor testing - Create New Master ... ');

  }

}



