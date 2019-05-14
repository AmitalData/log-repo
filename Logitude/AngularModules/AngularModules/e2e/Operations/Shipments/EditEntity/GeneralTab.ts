import { browser, by, element, WebDriver, protractor } from 'protractor';
import { FieldsHelper } from '../../../Helpers/FieldsHelper';

export class GeneralTabComponent {

  private Helper: FieldsHelper;

  constructor() {
    this.Helper = new FieldsHelper();

  }
  GeneralTab(LogitudeShipType: string) {
    this.Helper.WaitByIdAndClick('Shipment.TH.General');
    if (LogitudeShipType == 'D' || LogitudeShipType == 'H') {
      // Fill some fields in General tab
      // var mainHarmonize = this.Helper.WaitByIdAndFill('Shipment_MainHarmonize', 'Protractor test ')

      // var otherCharges = this.Helper.WaitByIdAndFill('Shipment_OtherPrepaidCollectId', 'coll');
      // this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);

      // var freightPC = this.Helper.WaitByIdAndFill('Shipment_FreightPrepaidCollectId', 'coll');
      // this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);

      var moveType = this.Helper.WaitByIdAndFill('Shipment_MoveTypeId', 'TestMoveType');
      this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);

      var incoterm = this.Helper.WaitByIdAndFill('Shipment_IncotermId', 'TestIncoterm');
      this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);

      var AMSBL = this.Helper.WaitByIdAndFill('Shipment_AMSBL', 'Shipment AMSBL');

      var department = this.Helper.WaitByIdAndFill('Shipment_DepartmentId', 'man');
      this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);

      var department = this.Helper.WaitByIdAndFill('Shipment_BranchId', 'main');
      this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);
    }
    else if (LogitudeShipType == 'M') {
      var freightPC = this.Helper.WaitByIdAndFill('Shipment_FreightPrepaidCollectId', 'coll');
      this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);

      var otherCharges = this.Helper.WaitByIdAndFill('Shipment_OtherPrepaidCollectId', 'coll');
      this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);

      var moveType = this.Helper.WaitByIdAndFill('Shipment_MoveTypeId', 'TestMoveType');
      this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);

      var AMSBL = this.Helper.WaitByIdAndFill('Shipment_AMSBL', 'Shipment AMSBL');

    }
  }


}

