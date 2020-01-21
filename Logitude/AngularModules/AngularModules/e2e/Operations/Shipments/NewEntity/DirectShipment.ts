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
        this.FillDirectShipmentFields(ReferenceNumber, TransportMode, Direction);
        this.Helper.ItemsPresent('ShipmentCreatebtn');
        this.Helper.WaitByIdAndClick('ShipmentCreatebtn');
    }

    public WaitSearchBoxResult() {

        this.Helper.WaitByIdAndClick('NEWSHIP');
        this.Helper.ItemsVisibility('NEWDIRECT');
        this.Helper.WaitByIdAndClick('NEWSHIP');

    }

    FillDirectShipmentFields(ShipperRef: string, TransportMode: string, Direction: string) {
        if (Direction == 'Domestic' && TransportMode == 'I') {
            this.Helper.WaitByIdAndFill('Shipment_ShipperId', 'TestShipper');
            this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'Shipment_ShipperId', 'TestShipper');

            this.Helper.WaitByIdAndFill('Shipment_ShipperReference1', ShipperRef);// test random number randomWholeNum

            this.Helper.WaitByIdAndFill('Shipment_ConsigneeId', 'TestShipper');
            this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'Shipment_ConsigneeId', 'TestShipper');
        } else {
            this.Helper.WaitByIdAndFill('Shipment_ShipperId', 'TestShipper');
            this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'Shipment_ShipperId', 'TestShipper');

            this.Helper.WaitByIdAndFill('Shipment_ShipperReference1', ShipperRef);// test random number randomWholeNum

            this.Helper.WaitByIdAndFill('Shipment_ConsigneeId', 'TestConsi');
            this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'Shipment_ConsigneeId', 'TestConsi');
            this.Helper.WaitByIdAndFill('Shipment_MainCarriageCarrierNumber', 'Flight555');

            if (Direction == 'Domestic') {
                this.Helper.WaitByIdAndFill('Shipment_MainCarriageFromPortId', 'eze');
                this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'Shipment_MainCarriageFromPortId', 'eze');
            
                this.Helper.WaitByIdAndFill('Shipment_MainCarriageToPortId', 'eze');
                this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'Shipment_MainCarriageToPortId', 'eze');
            } else {
                this.Helper.WaitByIdAndFill('Shipment_MainCarriageFromPortId', 'eze');
                this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'Shipment_MainCarriageFromPortId', 'eze');
          
                this.Helper.ItemsPresent('Shipment_MainCarriageToPortId');
                this.Helper.WaitByIdAndFill('Shipment_MainCarriageToPortId', 'mvd');
                this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'Shipment_MainCarriageToPortId', 'mvd');
            }
        }
        if (TransportMode == 'A') {
            this.Helper.WaitByIdAndFill('Shipment_MainCarriageCarrierId', 'BA');
            this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'Shipment_MainCarriageCarrierId', 'BA');

            this.Helper.WaitByIdAndFill('Shipment_MainCarriageCarrierNumber', '115');
            this.Helper.ItemsPresent('Shipment_MoveTypeId');
            this.Helper.WaitByIdAndFill('Shipment_MoveTypeId', 'TestMoveTypeIdAirMTA');
            this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'Shipment_MoveTypeId', 'TestMoveTypeIdAirMTA');

        }
        else if (TransportMode == 'O') {
            this.Helper.WaitByIdAndFill('Shipment_MainCarriageCarrierId', 'MAEU');
            this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'Shipment_MainCarriageCarrierId', 'MAEU');

            this.Helper.WaitByIdAndFill('Shipment_MainCarriageCarrierNumber', 'Voyage 1');

            this.Helper.WaitByIdAndFill('Shipment_MoveTypeId', 'TestMoveTypeIDOceanMTO');
            this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'Shipment_MoveTypeId', 'TestMoveTypeIDOceanMTO');

            this.Helper.WaitByIdAndFill('Shipment_MainCarriageVesselId', 'VPT');
            this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'Shipment_MainCarriageVesselId', 'VPT');
        }
        else {
            // this.Helper.WaitByIdAndFill('Shipment_MainCarriageCarrierId', 'Trucker1London');
            // this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);
            this.Helper.WaitByIdAndFill('Shipment_MainCarriageCarrierNumber', 'Trucker # 1');

            this.Helper.WaitByIdAndFill('Shipment_MoveTypeId', 'TestMoveTypeIdInlandMTI');
            this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'Shipment_MoveTypeId', 'TestMoveTypeIdInlandMTI');
        }



        // this.Helper.WaitByIdAndClick('Shipment_OrderIsDangerouseGoods');
        this.Helper.WaitByIdAndFill('Shipment_DescriptionOfGoods', 'Protractor testing - Create New Direct Shipment ... ');


    }

}



