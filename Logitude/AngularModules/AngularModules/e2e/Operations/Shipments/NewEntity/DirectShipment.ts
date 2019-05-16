import { browser, by, element, WebDriver, protractor } from 'protractor';
import { FieldsHelper } from '../../../Helpers/FieldsHelper';
import { ShipmentHelper } from '../ShipmentHelper';
import { IfObservable } from 'rxjs/observable/IfObservable';
export class DirectShipment {
  private Helper: FieldsHelper;
  private ShipmentModes: ShipmentHelper;

  constructor() {
    this.Helper = new FieldsHelper();
    this.ShipmentModes = new ShipmentHelper();
  }


  public CreateDirectShipment(ReferenceNumber: string, Direction: string, TransportMode: string, ShipmentType: string) {

    var AWBToggle = this.Helper.WaitByIdAndClick('NEWSHIP');
    this.Helper.WaitByIdAndClick('NEWDIRECT');
    this.ShipmentModes.SelectDicrctionTransportMode(Direction, TransportMode, ShipmentType);

    this.FillDirectShipmentFields(ReferenceNumber,TransportMode);

    this.Helper.WaitByIdAndClick('ShipmentCreatebtn');
  }


  public WaitSearchBoxResult() {

    this.Helper.WaitByIdAndClick('NEWSHIP');
    this.Helper.ItemsVisibility('NEWDIRECT');
    this.Helper.WaitByIdAndClick('NEWSHIP');

  }
  public CreateAndCloseNewShipment() {
    var AWBToggle = this.Helper.WaitByIdAndClick('NEWSHIP');
    this.Helper.WaitByIdAndClick('NEWDIRECT');
    this.Helper.WaitByIdAndClick('ShipmentCancelbtn');
  }



  FillDirectShipmentFields(ShipperRef: string ,TransportMode:string) {
    this.Helper.WaitByIdAndFill('Shipment_ShipperId', 'TestShipper');
    this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);

    this.Helper.WaitByIdAndFill('Shipment_ShipperReference1', ShipperRef);// test random number randomWholeNum


    this.Helper.WaitByIdAndFill('Shipment_ConsigneeId', 'TestConsi');
    this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);

    this.Helper.WaitByIdAndFill('Shipment_MainCarriageFromPortId', 'eze');
    this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);

    this.Helper.WaitByIdAndFill('Shipment_MainCarriageToPortId', 'mvd');
    this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);

   /* if(TransportMode=='A')
    {
      this.Helper.WaitByIdAndFill('Shipment_MainCarriageCarrierId', 'TestAirlineL8');
      this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);
  
      this.Helper.WaitByIdAndFill('Shipment_MainCarriageCarrierNumber', '115');  

      this.Helper.WaitByIdAndFill('Shipment_MoveTypeId', 'TestMoveType');
      this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);
  
    }
    else if(TransportMode=='O'){
      this.Helper.WaitByIdAndFill('Shipment_MainCarriageCarrierId', 'TestShippingLineSLPT');
      this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);
  
      this.Helper.WaitByIdAndFill('Shipment_MainCarriageCarrierNumber', 'Voyage 1');  

      this.Helper.WaitByIdAndFill('Shipment_MoveTypeId', 'TestMoveTypeIDOceanMTO1');
      this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);
  
    }
    else{
      this.Helper.WaitByIdAndFill('Shipment_MainCarriageCarrierId', 'TestTruckerTP');
      this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);
  
      this.Helper.WaitByIdAndFill('Shipment_MainCarriageCarrierNumber', 'Trucker # 1');  
  
      this.Helper.WaitByIdAndFill('Shipment_MoveTypeId', 'TestMoveTypeIDInlandMTI1');
      this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);
  
    }*/
   
    //this.Helper.WaitByIdAndFill('Shipment_IncotermId', 'TestIncoterm');
   // this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);

   
    // this.Helper.WaitByIdAndClick('Shipment_OrderIsDangerouseGoods');
    this.Helper.WaitByIdAndFill('Shipment_DescriptionOfGoods', 'Protractor testing - Create New Direct Shipment ... ');


  }

}



