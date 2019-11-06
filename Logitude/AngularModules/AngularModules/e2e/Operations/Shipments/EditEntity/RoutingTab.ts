import { browser, by, element, WebDriver, protractor } from 'protractor';
import { FieldsHelper } from '../../../Helpers/FieldsHelper';
import { ShipmentHelper } from '../ShipmentHelper';


export class RoutingTabComponent {
    private Helper: FieldsHelper;
    private shipHelper: ShipmentHelper = new ShipmentHelper();


    constructor() {
        this.Helper = new FieldsHelper();
    }

    public RoutingTab(ShipmentLevelCode: any, ShipmentType: any, Direction: string) {
        this.Helper.WaitByIdAndClick('Shipment.TH.Routings');
        if ((Direction == 'Domestic' && ShipmentType == 'FTL') || (Direction == 'Domestic' && ShipmentType == 'LTL')) {
            // var mainCarriageCarrier = this.Helper.WaitByIdAndFill('Shipment_MainCarriageCarrierId', 'Trucker1London');
            // this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);
            this.Helper.WaitByIdAndFill('Shipment_Driver', 'Driver, Protractor ... ');
            this.Helper.WaitByIdAndFill('Shipment_TruckNumber', '985');
        }
        else {
            this.AddPickup();
            this.Helper.WaitBusyIndicator();
            if (ShipmentLevelCode == 'D' || ShipmentLevelCode == 'H') {
                this.AddPreCarriage(ShipmentType);
                this.AddOnCarriage(ShipmentType);
            }
            this.EditMainCarriage(ShipmentLevelCode, Direction, ShipmentType);
            this.AddDelivery();
            this.Helper.WaitBusyIndicator();
        }
    }
    private EditMainCarriage(ShipmentLevelCode: string, Direction: string, ShipmentType: any) {
        this.Helper.WaitByIdAndClick('Edit-MainCarriage');

        if (ShipmentLevelCode == 'D' || ShipmentLevelCode == 'M') {
            if (ShipmentType == '') {
                if (Direction == 'Domestic') {
                    var transshipment1FromPortId = this.Helper.WaitByIdAndFill('Shipment_Transshipment1FromPortId', 'eze');
                    this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'Shipment_Transshipment1FromPortId', 'eze');

                    var transshipment2FromPortId = this.Helper.WaitByIdAndFill('Shipment_Transshipment2FromPortId', 'eze');
                    this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'Shipment_Transshipment2FromPortId', 'eze');

                    var transshipment3FromPortId = this.Helper.WaitByIdAndFill('Shipment_Transshipment3FromPortId', 'eze');
                    this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'Shipment_Transshipment3FromPortId', 'eze');

                } else {
                    var transshipment1FromPortId = this.Helper.WaitByIdAndFill('Shipment_Transshipment1FromPortId', 'LHR');
                    this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'Shipment_Transshipment1FromPortId', 'LHR');

                    var transshipment2FromPortId = this.Helper.WaitByIdAndFill('Shipment_Transshipment2FromPortId', 'JFK');
                    this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'Shipment_Transshipment2FromPortId', 'JFK');

                    var transshipment3FromPortId = this.Helper.WaitByIdAndFill('Shipment_Transshipment3FromPortId', 'MAN');
                    this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'Shipment_Transshipment3FromPortId', 'MAN');
                }

                var transshipment1CarrierId = this.Helper.WaitByIdAndFill('Shipment_Transshipment1CarrierId', 'BA');
                this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'Shipment_Transshipment1CarrierId', 'BA');
                this.Helper.WaitByIdAndFill('Shipment_Transshipment1CarrierNumber', '125');

                var transshipment2CarrierId = this.Helper.WaitByIdAndFill('Shipment_Transshipment2CarrierId', 'BA');
                this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'Shipment_Transshipment2CarrierId', 'BA');
                this.Helper.WaitByIdAndFill('Shipment_Transshipment2CarrierNumber', '126');

                var transshipment3CarrierId = this.Helper.WaitByIdAndFill('Shipment_Transshipment3CarrierId', 'BA');
                this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'Shipment_Transshipment3CarrierId', 'BA');
                this.Helper.WaitByIdAndFill('Shipment_Transshipment3CarrierNumber', '127');

                this.Helper.WaitByIdAndFill('date_Shipment_MainCarriageATD', '1');
                this.Helper.WaitByIdAndFill('date_Shipment_MainCarriageATA', '1');

                //this.shipHelper.AddAirlineStock('edit');

                this.Helper.WaitByIdAndClick('MainCarriageOKBtn');
            }
            else if (ShipmentType == 'FCL' || ShipmentType == 'LCL' || ShipmentType == 'OG') {
                // First leg details
                // var mainCarriageVessel = this.Helper.WaitByIdAndFill('Shipment_MainCarriageVesselId', 'VesselIdPT');
                // this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);

                this.Helper.WaitByIdAndFill('date_Shipment_MainCarriageATD', '1');
                this.Helper.WaitByIdAndFill('date_Shipment_MainCarriageATA', '1');

                // if (Direction == 'Domestic') {
                //   var transshipment1FromPortId = this.Helper.WaitByIdAndFill('Shipment_Transshipment1FromPortId', 'MIA');
                //   this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);

                //   var transshipment2FromPortId = this.Helper.WaitByIdAndFill('Shipment_Transshipment2FromPortId', 'LAS');
                //   this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);

                //   var transshipment3FromPortId = this.Helper.WaitByIdAndFill('Shipment_Transshipment3FromPortId', 'MIA');
                //   this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);

                // } else {
                var transshipment1FromPortId = this.Helper.WaitByIdAndFill('Shipment_Transshipment1FromPortId', 'eze');
                this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'Shipment_Transshipment1FromPortId', 'eze');

                var transshipment2FromPortId = this.Helper.WaitByIdAndFill('Shipment_Transshipment2FromPortId', 'eze');
                this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'Shipment_Transshipment2FromPortId', 'eze');

                var transshipment3FromPortId = this.Helper.WaitByIdAndFill('Shipment_Transshipment3FromPortId', 'eze');
                this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'Shipment_Transshipment3FromPortId', 'eze');
                //}
                var transshipment1CarrierId = this.Helper.WaitByIdAndFill('Shipment_Transshipment1CarrierId', 'MSCU');
                this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'Shipment_Transshipment1CarrierId', 'MSCU');
                this.Helper.WaitByIdAndFill('Shipment_Transshipment1CarrierNumber', '125');

                var transshipment2CarrierId = this.Helper.WaitByIdAndFill('Shipment_Transshipment2CarrierId', 'MSCU');
                this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'Shipment_Transshipment2CarrierId', 'MSCU');
                this.Helper.WaitByIdAndFill('Shipment_Transshipment2CarrierNumber', '126');

                var transshipment3CarrierId = this.Helper.WaitByIdAndFill('Shipment_Transshipment3CarrierId', 'MSCU');
                this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'Shipment_Transshipment3CarrierId', 'MSCU');
                this.Helper.WaitByIdAndFill('Shipment_Transshipment3CarrierNumber', '127');

                this.Helper.WaitByIdAndClick('MainCarriageOKBtn');
            }
            else if (ShipmentType == 'LTL' || ShipmentType == 'FTL' || ShipmentType == 'IG') {

                if (Direction == 'Domestic') {
                    var transshipment1FromPortId = this.Helper.WaitByIdAndFill('Shipment_Transshipment1FromPortId', 'eze');
                    this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'Shipment_Transshipment1FromPortId', 'eze');

                    var transshipment2FromPortId = this.Helper.WaitByIdAndFill('Shipment_Transshipment2FromPortId', 'eze');
                    this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'Shipment_Transshipment2FromPortId', 'eze');

                    var transshipment3FromPortId = this.Helper.WaitByIdAndFill('Shipment_Transshipment3FromPortId', 'eze');
                    this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'Shipment_Transshipment3FromPortId', 'eze');

                } else {
                    var transshipment1FromPortId = this.Helper.WaitByIdAndFill('Shipment_Transshipment1FromPortId', 'MAN');
                    this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'Shipment_Transshipment1FromPortId', 'MAN');

                    var transshipment2FromPortId = this.Helper.WaitByIdAndFill('Shipment_Transshipment2FromPortId', 'LON');
                    this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'Shipment_Transshipment2FromPortId', 'LON');

                    var transshipment3FromPortId = this.Helper.WaitByIdAndFill('Shipment_Transshipment3FromPortId', 'NYC');
                    this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'Shipment_Transshipment3FromPortId', 'NYC');
                }
                var transshipment1CarrierId = this.Helper.WaitByIdAndFill('Shipment_Transshipment1CarrierId', 'Trucker1London');
                this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'Shipment_Transshipment1CarrierId', 'Trucker1London');
                this.Helper.WaitByIdAndFill('Shipment_Transshipment1CarrierNumber', '125');

                var transshipment2CarrierId = this.Helper.WaitByIdAndFill('Shipment_Transshipment2CarrierId', 'Trucker1London');
                this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'Shipment_Transshipment2CarrierId', 'Trucker1London');
                this.Helper.WaitByIdAndFill('Shipment_Transshipment2CarrierNumber', '126');

                var transshipment3CarrierId = this.Helper.WaitByIdAndFill('Shipment_Transshipment3CarrierId', 'Trucker1London');
                this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'Shipment_Transshipment3CarrierId', 'Trucker1London');
                this.Helper.WaitByIdAndFill('Shipment_Transshipment3CarrierNumber', '127');

                this.Helper.WaitByIdAndClick('MainCarriageOKBtn');
            }
        }
        else if (ShipmentLevelCode == 'H') {
            var transshipment1FromPortId = this.Helper.WaitByIdAndFill('Shipment_MainCarriageFromPortId', 'eze');
            this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'Shipment_MainCarriageFromPortId', 'eze');

            var transshipment1CarrierId = this.Helper.WaitByIdAndFill('Shipment_MainCarriageToPortId', 'eze');
            this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'Shipment_MainCarriageToPortId', 'eze');

            this.Helper.WaitByIdAndClick('OkBtn');
        }
    }
    private AddPickup() {
        this.Helper.WaitByIdAndClick('Add-PickUp');

        // From Port Details 
        this.Helper.WaitByIdAndClick('ShipmentPickUpDelivery_CarrierId');
        var fromPortBtn = element(by.id('Port_FromRadio'));
        browser.executeScript("arguments[0].click();", fromPortBtn.getWebElement());
        this.Helper.WaitByIdAndFill('ShipmentPickUpDelivery_FromPortId', 'eze');
        this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'ShipmentPickUpDelivery_FromPortId', 'eze');

        //To Port Details 
        browser.executeScript("arguments[0].click();", element(by.id('Port_ToRadio')).getWebElement());
        this.Helper.WaitByIdAndFill('ShipmentPickUpDelivery_ToPortId', 'eze');
        this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'ShipmentPickUpDelivery_ToPortId', 'eze');

        var trucker = this.Helper.WaitByIdAndFill('ShipmentPickUpDelivery_CarrierId', 'Trucker1London');
        this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'ShipmentPickUpDelivery_CarrierId', 'Trucker1London');

        this.Helper.WaitByIdAndFill('ShipmentPickUpDelivery_CarrierNumber', 'Truck Prot');
        this.Helper.WaitByIdAndFill('ShipmentPickUpDelivery_Driver', 'Trucker Driver');
        this.Helper.WaitByIdAndFill('ShipmentPickUpDelivery_TruckNumber', 'Transp Doc');
        this.Helper.WaitByIdAndFill('ShipmentPickUpDelivery_TrailerNumber', 'Trail No.');
        this.Helper.WaitByIdAndFill('ShipmentPickUpDelivery_Notes', 'Adding Pickup by protractor .. ');

        // Pickup packages details 
        this.Helper.WaitByIdAndClick('ShipmentPickUpDelivery.TH.Packages');
        this.Helper.WaitByIdAndClick('Add');
        this.Helper.WaitByIdAndFill('ShipmentPickUpDeliveryPackage_Quantity', '10');
        this.Helper.WaitByIdAndFill('ShipmentPickUpDeliveryPackage_Weight', '100');
        // this.Helper.WaitByCssButtonClick('.RedButton', 'Ok');
        this.Helper.WaitByIdAndClick('AddPickupPackage');

        this.Helper.WaitByIdAndClick('SaveBtn');
        this.Helper.WaitEditComponentBusyIndicator();
        this.Helper.WaitByIdAndClick('CloseBtn');
    }
    private AddPreCarriage(ShipmentType: string) {

        this.Helper.WaitByIdAndClick('RoutingToggle');
        this.Helper.WaitByIdAndClick('PreCarriage');
        if (ShipmentType == '') {
            var preCarriageTransportMode = this.Helper.WaitByIdAndFill('Shipment_PreCarriageTransportModeId', 'air');
            this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'Shipment_PreCarriageTransportModeId', 'air');

            var preCarriageFromPort = this.Helper.WaitByIdAndFill('Shipment_PreCarriageFromPortId', 'eze');
            this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'Shipment_PreCarriageFromPortId', 'eze');

            var preCarriageToPort = this.Helper.WaitByIdAndFill('Shipment_PreCarriageToPortId', 'eze');
            this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'Shipment_PreCarriageToPortId', 'eze');

            var preCarriageCarrier = this.Helper.WaitByIdAndFill('Shipment_PreCarriageCarrierId', 'aerolin');
            this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'Shipment_PreCarriageCarrierId', 'aerolin');

            this.Helper.WaitByIdAndFill('Shipment_PreCarriageCarrierNumber', '985');
            this.Helper.WaitByIdAndClick('PreCarriageOKBtn');
        }
        else if (ShipmentType == 'FCL' || ShipmentType == 'LCL') {
            var preCarriageTransportMode = this.Helper.WaitByIdAndFill('Shipment_PreCarriageTransportModeId', 'ocean');
            this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'Shipment_PreCarriageTransportModeId', 'ocean');

            var preCarriageFromPort = this.Helper.WaitByIdAndFill('Shipment_PreCarriageFromPortId', 'eze');
            this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'Shipment_PreCarriageFromPortId', 'eze');

            var preCarriageToPort = this.Helper.WaitByIdAndFill('Shipment_PreCarriageToPortId', 'eze');
            this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'Shipment_PreCarriageToPortId', 'eze');

            var preCarriageCarrier = this.Helper.WaitByIdAndFill('Shipment_PreCarriageCarrierId', 'Maersk lines');
            this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'Shipment_PreCarriageCarrierId', 'Maersk lines');

            this.Helper.WaitByIdAndFill('Shipment_PreCarriageCarrierNumber', '985');
            this.Helper.WaitByIdAndClick('PreCarriageOKBtn');
        }
        else if (ShipmentType == 'FTL' || ShipmentType == 'LTL') {
            var preCarriageTransportMode = this.Helper.WaitByIdAndFill('Shipment_PreCarriageTransportModeId', 'Inland');
            this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'Shipment_PreCarriageTransportModeId', 'Inland');

            var preCarriageFromPort = this.Helper.WaitByIdAndFill('Shipment_PreCarriageFromPortId', 'eze');
            this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'Shipment_PreCarriageFromPortId', 'eze');

            var preCarriageToPort = this.Helper.WaitByIdAndFill('Shipment_PreCarriageToPortId', 'eze');
            this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'Shipment_PreCarriageToPortId', 'eze');

            var preCarriageCarrier = this.Helper.WaitByIdAndFill('Shipment_PreCarriageCarrierId', 'Trucker1London');
            this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'Shipment_PreCarriageCarrierId', 'Trucker1London');

            this.Helper.WaitByIdAndFill('Shipment_PreCarriageCarrierNumber', '985');
            this.Helper.WaitByIdAndClick('PreCarriageOKBtn');
        }
    }
    private AddOnCarriage(ShipmentType: string) {
        this.Helper.WaitByIdAndClick('RoutingToggle');
        this.Helper.WaitByIdAndClick('OnCarriage');

        if (ShipmentType == '') {
            var preCarriageTransportMode = this.Helper.WaitByIdAndFill('Shipment_OnCarriageTransportModeId', 'a');
            this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'Shipment_OnCarriageTransportModeId', 'a');


            var preCarriageFromPort = this.Helper.WaitByIdAndFill('Shipment_OnCarriageFromPortId', 'eze');
            this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'Shipment_OnCarriageFromPortId', 'eze');

            var preCarriageToPort = this.Helper.WaitByIdAndFill('Shipment_OnCarriageToPortId', 'eze');
            this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'Shipment_OnCarriageToPortId', 'eze');

            var preCarriageCarrier = this.Helper.WaitByIdAndFill('Shipment_OnCarriageCarrierId', 'American');
            this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'Shipment_OnCarriageCarrierId', 'American');

            this.Helper.WaitByIdAndFill('Shipment_OnCarriageCarrierNumber', '985');
            this.Helper.WaitByIdAndClick('OnCarriageOKBtn');
        }
        else if (ShipmentType == 'FCL' || ShipmentType == 'LCL') {
            var preCarriageTransportMode = this.Helper.WaitByIdAndFill('Shipment_OnCarriageTransportModeId', 'o');
            this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'Shipment_OnCarriageTransportModeId', 'o');


            var preCarriageFromPort = this.Helper.WaitByIdAndFill('Shipment_OnCarriageFromPortId', 'eze');
            this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'Shipment_OnCarriageFromPortId', 'eze');

            var preCarriageToPort = this.Helper.WaitByIdAndFill('Shipment_OnCarriageToPortId', 'eze');
            this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'Shipment_OnCarriageToPortId', 'eze');

            var preCarriageCarrier = this.Helper.WaitByIdAndFill('Shipment_OnCarriageCarrierId', 'Maersk lines');
            this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'Shipment_OnCarriageCarrierId', 'Maersk lines');

            this.Helper.WaitByIdAndFill('Shipment_OnCarriageCarrierNumber', '985');
            this.Helper.WaitByIdAndClick('OnCarriageOKBtn');
        }
        else if (ShipmentType == 'FTL' || ShipmentType == 'LTL') {
            var preCarriageTransportMode = this.Helper.WaitByIdAndFill('Shipment_OnCarriageTransportModeId', 'I');
            this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'Shipment_OnCarriageTransportModeId', 'I');


            var preCarriageFromPort = this.Helper.WaitByIdAndFill('Shipment_OnCarriageFromPortId', 'eze');
            this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'Shipment_OnCarriageFromPortId', 'eze');

            var preCarriageToPort = this.Helper.WaitByIdAndFill('Shipment_OnCarriageToPortId', 'eze');
            this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'Shipment_OnCarriageToPortId', 'eze');

            var preCarriageCarrier = this.Helper.WaitByIdAndFill('Shipment_OnCarriageCarrierId', 'Trucker1NewYork');
            this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'Shipment_OnCarriageCarrierId', 'Trucker1NewYork');

            this.Helper.WaitByIdAndFill('Shipment_OnCarriageCarrierNumber', '985');
            this.Helper.WaitByIdAndClick('OnCarriageOKBtn');
        }
    }
    private AddDelivery() {
        this.Helper.WaitByIdAndClick('Add-Delivery');

        var trucker = this.Helper.WaitByIdAndFill('ShipmentPickUpDelivery_CarrierId', 'Trucker1London');
        this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'ShipmentPickUpDelivery_CarrierId', 'Trucker1London');

        this.Helper.WaitByIdAndFill('ShipmentPickUpDelivery_CarrierNumber', '777');
        this.Helper.WaitByIdAndFill('ShipmentPickUpDelivery_Driver', 'Trucker Driver');
        this.Helper.WaitByIdAndFill('ShipmentPickUpDelivery_TruckNumber', 'Transp Doc');
        this.Helper.WaitByIdAndFill('ShipmentPickUpDelivery_TrailerNumber', 'Trail No.');
        this.Helper.WaitByIdAndFill('ShipmentPickUpDelivery_Notes', 'Adding Delivery ');

        this.Helper.WaitByIdAndClick('SaveBtn');
        this.Helper.WaitEditComponentBusyIndicator();
        this.Helper.WaitByIdAndClick('CloseBtn');
    }
}

