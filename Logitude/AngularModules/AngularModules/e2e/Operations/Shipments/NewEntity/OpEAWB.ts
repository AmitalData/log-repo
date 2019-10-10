import { browser, by, element, WebDriver, protractor } from 'protractor';
import { FieldsHelper } from '../../../Helpers/FieldsHelper';
import { ShipmentWorkSpace } from './ShipmentWorkSpace';
import { GeneralFunctions } from '../../../Helpers/GeneralFunctions';
import { EditTabsComponent } from '../EditEntity/EditShipmentTabs.po';
import { DirectAWB } from './DirectAWB';
import { HouseAWB } from './HouseAWB';
import { MasterAWB } from './MasterAWB';

export class OpEAWB {
    private Helper: FieldsHelper;
    private ShipmentWorkSpace: ShipmentWorkSpace;
    private Operation: GeneralFunctions;
    private DirectWizard:DirectAWB;
    private HouseWizard: HouseAWB = new HouseAWB();
    private MasterWizard: MasterAWB = new MasterAWB();


    constructor() {
        this.Helper = new FieldsHelper();
        this.ShipmentWorkSpace = new ShipmentWorkSpace();
        this.Operation = new GeneralFunctions();
        this.DirectWizard=new DirectAWB();
    }
    CreateAWB(LogitudeWizardType: string,shipperRef1:string) {
        this.Operation.GoToMainMenu('General.MH.Operations');
        this.Operation.SelectMenuWorkSpaceTabs('SHIP');
        //var shipperRef1 = this.Operation.RandomNum();

        if (LogitudeWizardType == 'D') {
           this.DirectWizard.CreateDirectAWB(shipperRef1, LogitudeWizardType);
        }
        //else if (LogitudeWizardType == 'H') {
        //    this.HouseWizard.CreateHouseWizard(shipperRef1, LogitudeWizardType);
        //}
        //else if (LogitudeWizardType == 'M') {
        //    this.MasterWizard.CreateMasterWizard(shipperRef1, LogitudeWizardType);
        //}
    }
}
