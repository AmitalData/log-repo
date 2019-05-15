import { browser, by, element, WebDriver, protractor } from 'protractor';
import { FieldsHelper } from '../../../Helpers/FieldsHelper';
import { ShipmentWorkSpace } from './ShipmentWorkSpace';
import { GeneralFunctions } from '../../../Helpers/GeneralFunctions';

export class OperationsComp {
  private Helper: FieldsHelper;
  private ShipmentWorkSpace: ShipmentWorkSpace;
  private Operation: GeneralFunctions;

  constructor() {
    this.Helper = new FieldsHelper();
    this.ShipmentWorkSpace = new ShipmentWorkSpace();
    this.Operation = new GeneralFunctions();
  }
  DoOperations() {
    this.Operation.GoToMainMenu('General.MH.Operations');
    this.Operation.SelectMenuWorkSpaceTabs('SHIP');
  //  this.ShipmentWorkSpace.CreateWizard('M');

      this.ShipmentWorkSpace.CreateShipment('D', 'Export', 'A', '');
     


  }
}

