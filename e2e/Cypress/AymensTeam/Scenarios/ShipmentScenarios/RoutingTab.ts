import { Resolvers } from "../../Resolvers/Resolvers";
import { Random } from '../../@e2e/core';

export class RoutingTab {
    private transportMode: string;
    private levelCode: string;
    private levelName: string;

    public RunRoutingTabScenarios(levelCode: string,transportMode: string) {
        this.transportMode = transportMode;
        this.levelCode = levelCode;

        if (levelCode == 'M') {
            this.levelName = 'Master';
        } else {
            this.levelName = 'Shipment';
        }
        this.GoToRoutingTab();
        this.AddPickupDeilvery('P');
        if (this.levelCode != 'M') {
        this.AddPreOnCarriage('Pre');
        this.AddPreOnCarriage('On');
        }
        if (this.levelCode != 'H') {
            this.AddMainLegs();
        }
        this.AddPickupDeilvery('D');
    }
    private GoToRoutingTab() {
        cy.get('#ShipmentTHRoutings').click();
    }
    private AddPickupDeilvery(typeName: string) {
        let typeId: string = null;
        switch (typeName) {
            case "P": { typeId = '#Add-PickUp'; break; } // Pickup
            case "D": { typeId = '#Add-Delivery'; break; } // Delivery 
        }

        Resolvers.ButtonResolver.Selector(typeId).Click();
        this.FillFromAddress('Port');
        this.FillToAddress('PART');

        cy.server();
        cy.route({
            method: 'PUT',
            url: '**/' + 'shipment',
            onResponse: (xhr) => {
                expect(xhr.status).to.eq(200);
            }
        }).as('SavingEditComponent')

        Resolvers.ButtonResolver.Selector('#CloseBtn').Click();
        Resolvers.ButtonResolver.Selector('#ConfirmWindow_Yes_0').Click();
        cy.wait('@SavingEditComponent');
        Resolvers.WindowResolver.ShouldBeClosed();
    }
    private FillFromAddress(PickupDeliveryTypeCode: string) {
        if (PickupDeliveryTypeCode == 'PART') {
            Resolvers.RadioButtonResolver.Selector('#Partner_FromRadio').Select();
            Resolvers.LOVResolver.Selector('#ShipmentPickUpDelivery_FromPartnerCardId').Type('testshipper');

        } else if (PickupDeliveryTypeCode == 'Port') {
            Resolvers.RadioButtonResolver.Selector('#Port_FromRadio').Select();
            Resolvers.LOVResolver.Selector('#ShipmentPickUpDelivery_FromPortId').Type('ez');

        } else {
            Resolvers.RadioButtonResolver.Selector('#CasualAddress_FromRadio').Select();
            Resolvers.TextBoxResolver.Selector('#ShipmentPickUpDelivery_FromAddressCity').Type('Albania');
            Resolvers.LOVResolver.Selector('#ShipmentPickUpDelivery_FromAddressCountryId').Type('Albania');
        }
    }
    private FillToAddress(PickupDeliveryTypeCode: string) {
        if (PickupDeliveryTypeCode == 'PART') {
            Resolvers.RadioButtonResolver.Selector('#Partner_ToRadio').Select();
            Resolvers.LOVResolver.Selector('#ShipmentPickUpDelivery_ToPartnerCardId').Type('testconsi');

        } else if (PickupDeliveryTypeCode == 'Port') {
            Resolvers.RadioButtonResolver.Selector('#Port_ToRadio').Select();
            Resolvers.LOVResolver.Selector('#ShipmentPickUpDelivery_ToPortId').Type('ez');

        } else {
            Resolvers.RadioButtonResolver.Selector('#CasualAddress_ToRadio').Select();
            Resolvers.TextBoxResolver.Selector('#ShipmentPickUpDelivery_ToAddressCity').Type('Albania');
            Resolvers.LOVResolver.Selector('#ShipmentPickUpDelivery_ToAddressCountryId').Type('Albania');
        }
    }
    private AddPreOnCarriage(typeName: string) {
        if (typeName == 'Pre') {
            Resolvers.ToggleButtonResolver.Selector('#RoutingToggle').SelectByIndex(1);
            this.FillPreCarriageDetails();
        } else {
            Resolvers.ToggleButtonResolver.Selector('#RoutingToggle').SelectByIndex(2);
            this.FillOnCarriageDetails();
        }
    }
    private FillPreCarriageDetails() {
        if (this.transportMode == 'A') {
            Resolvers.LOVResolver.Selector('#Shipment_PreCarriageTransportModeId').Type('air');
            Resolvers.LOVResolver.Selector('#Shipment_PreCarriageCarrierId').Type('air');
        } else if (this.transportMode == 'O') {
            Resolvers.LOVResolver.Selector('#Shipment_PreCarriageTransportModeId').Type('ocean');
            Resolvers.LOVResolver.Selector('#Shipment_PreCarriageCarrierId').Type('shi');
            Resolvers.LOVResolver.Selector('#Shipment_PreCarriageVesselId').SelectFirst();

        } else if (this.transportMode == 'I') {
            Resolvers.LOVResolver.Selector('#Shipment_PreCarriageTransportModeId').Type('inlan');
            Resolvers.LOVResolver.Selector('#Shipment_PreCarriageCarrierId').Type('tru');
        }
        Resolvers.LOVResolver.Selector('#Shipment_PreCarriageFromPortId').Type('eze');
        Resolvers.LOVResolver.Selector('#Shipment_PreCarriageToPortId').Type('mvd');
        Resolvers.DatePickerResolver.Selector('#date_Shipment_PreCarriageETD').Type('.');

        Resolvers.ButtonResolver.Selector('#PreCarriageOKBtn').Click();
    }
    private FillOnCarriageDetails() {
        if (this.transportMode == 'A') {
            Resolvers.LOVResolver.Selector('#Shipment_OnCarriageTransportModeId').Type('air');
            Resolvers.LOVResolver.Selector('#Shipment_OnCarriageCarrierId').Type('air');
        } else if (this.transportMode == 'O') {
            Resolvers.LOVResolver.Selector('#Shipment_OnCarriageTransportModeId').Type('ocean');
            Resolvers.LOVResolver.Selector('#Shipment_OnCarriageCarrierId').Type('shi');
            Resolvers.LOVResolver.Selector('#Shipment_OnCarriageVesselId').SelectFirst();

        } else if (this.transportMode == 'I') {
            Resolvers.LOVResolver.Selector('#Shipment_OnCarriageTransportModeId').Type('inlan');
            Resolvers.LOVResolver.Selector('#Shipment_OnCarriageCarrierId').Type('tru');
        }
        Resolvers.LOVResolver.Selector('#Shipment_OnCarriageFromPortId').Type('eze');
        Resolvers.LOVResolver.Selector('#Shipment_OnCarriageToPortId').Type('mvd');
        Resolvers.DatePickerResolver.Selector('#date_Shipment_OnCarriageETD').Type('.');

        Resolvers.ButtonResolver.Selector('#OnCarriageOKBtn').Click();
    }
    private AddMainLegs() {
        Resolvers.ButtonResolver.Selector('#Edit-MainCarriage').Click();
        this.FillMainCarriageDetails();
        this.FillViaPort1LegDetails();
        this.FillViaPort2LegDetails();
        this.FillViaPort3LegDetails();
        Resolvers.ButtonResolver.Selector('#MainCarriageOKBtn').Click();
    }
    private FillMainCarriageDetails() {
        Resolvers.TextBoxResolver.Selector('#Shipment_MainCarriageCarrierNumber_1').Type('123');
        Resolvers.TextBoxResolver.Selector('#Shipment_Master').Type(Random.GetRandom());
        Resolvers.DatePickerResolver.Selector('#date_Shipment_MainCarriageATD').Type('.');
        Resolvers.DatePickerResolver.Selector('#date_Shipment_MainCarriageATA').Type('.');

    }
    private FillViaPort1LegDetails(){
        Resolvers.LOVResolver.Selector('#Shipment_Transshipment1FromPortId').Type('mvd');
        Resolvers.LOVResolver.Selector('#Shipment_Transshipment1CarrierId').SelectFirst();
        Resolvers.TextBoxResolver.Selector('#Shipment_Transshipment1CarrierNumber').Type('456');

    }
    private FillViaPort2LegDetails() {
        Resolvers.LOVResolver.Selector('#Shipment_Transshipment2FromPortId').Type('mvd');
        Resolvers.LOVResolver.Selector('#Shipment_Transshipment2CarrierId').SelectFirst();
        Resolvers.TextBoxResolver.Selector('#Shipment_Transshipment2CarrierNumber').Type('789');

    }
    private FillViaPort3LegDetails() {
        Resolvers.LOVResolver.Selector('#Shipment_Transshipment3FromPortId').Type('mvd');
        Resolvers.LOVResolver.Selector('#Shipment_Transshipment3CarrierId').SelectFirst();
        Resolvers.TextBoxResolver.Selector('#Shipment_Transshipment3CarrierNumber').Type('987');

    }
}