import { browser, by, element, WebDriver, protractor } from 'protractor';
import { FieldsHelper } from '../../../Helpers/FieldsHelper';
import {ShipmentHelper} from '../ShipmentHelper';

export class MasterShipment {
    private Helper: FieldsHelper;
    private ShipmentModes: ShipmentHelper;

    constructor() {
        this.Helper = new FieldsHelper();
        this.ShipmentModes = new ShipmentHelper();

    }

    public CreateMasterShipment(ReferenceNumber: string, LogitudeShipType: string, Direction: any, TransportMode: any, ShipmentType: any) {
        var AWBToggle = this.Helper.WaitByIdAndClick('NEWSHIP');
        this.Helper.WaitByIdAndClick('NEWMASTER');
        this.ShipmentModes.SelectDicrctionTransportMode(Direction, TransportMode, ShipmentType);

        this.FillMasterShipmentFields(ReferenceNumber, TransportMode, Direction);
        this.Helper.WaitByIdAndClick('MasterCreatebtn');
    }
    FillMasterShipmentFields(ShipperRef: string, TransportMode: string, Direction: string) {
        this.Helper.WaitByIdAndFill('Master_AgentId', 'TestAgentExport1');
        this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'Master_AgentId', 'TestAgentExport1');

        this.Helper.WaitByIdAndFill('Master_AgentReference1_1', ShipperRef);
        if (Direction == 'Domestic') {
            this.Helper.WaitByIdAndFill('Master_MainCarriageFromPortId', 'eze');
            this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'Master_MainCarriageFromPortId', 'eze');

            this.Helper.WaitByIdAndFill('Master_MainCarriageToPortId', 'eze');
            this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'Master_MainCarriageToPortId', 'eze');
        }
        else {
            this.Helper.WaitByIdAndFill('Master_MainCarriageFromPortId', 'eze');
            this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'Master_MainCarriageFromPortId', 'eze');

            this.Helper.WaitByIdAndFill('Master_MainCarriageToPortId', 'mvd');
            this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'Master_MainCarriageToPortId', 'mvd');

        }
        // this.Helper.WaitByIdAndFill('Master_MainCarriageCarrierId', 'TestAirlineL8');
        // this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);

        // this.Helper.WaitByIdAndFill('Master_MainCarriageCarrierNumber', '115')

        if (TransportMode == 'A') {
            this.Helper.WaitByIdAndFill('Master_MainCarriageCarrierId', 'BA');
            this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'Master_MainCarriageCarrierId', 'BA');

            this.Helper.WaitByIdAndFill('Master_MainCarriageCarrierNumber', '115');
        }
        else if (TransportMode == 'O') {
            this.Helper.WaitByIdAndFill('Master_MainCarriageCarrierId', 'MAEU');
            this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'Master_MainCarriageCarrierId', 'MAEU');

            this.Helper.WaitByIdAndFill('Master_MainCarriageCarrierNumber', 'Voyage 1');
        }
        else {
            this.Helper.WaitByIdAndFill('Master_MainCarriageCarrierId', 'Trucker1London');
            this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'Master_MainCarriageCarrierId', 'Trucker1London');

            this.Helper.WaitByIdAndFill('Master_MainCarriageCarrierNumber', 'Trucker # 1');
        }

        this.Helper.WaitByIdAndFill('Master_DescriptionOfGoods', 'Protractor testing - Create New Master ... ');
    }
}



