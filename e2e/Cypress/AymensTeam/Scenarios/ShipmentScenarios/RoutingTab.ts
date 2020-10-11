import { Resolvers } from "../../Resolvers/Resolvers";

export class RoutingTab {

    public RunRoutingTabScenarios() {
        this.GoToRoutingTab();
        //this.FillDates();
        this.AddPickupDeilvery();

    }
    private GoToRoutingTab() {
        cy.get('#ShipmentTHRoutings').click();
    }
   
    private AddPickupDeilvery() {
        this.AddRoute('P');
        this.AddRoute('D');

    }
    private AddRoute(typeName: string) {
        let typeId: string = null;
        switch (typeName) {
            case "P": { typeId = '#Add-PickUp'; break; } // Pickup
            case "D": { typeId = '#Add-Delivery'; break; } // Delivery 
        }

        Resolvers.ButtonResolver.Selector(typeId).Click();
        this.FillFromAddress('Port');
        this.FillToAddress('PART');
        
        Resolvers.ButtonResolver.Selector('#CloseBtn').Click();
        Resolvers.ButtonResolver.Selector('#ConfirmWindow_Yes_0').Click();

    }
    private FillFromAddress(PickupDeliveryTypeCode: string) {

        if (PickupDeliveryTypeCode == 'PART') {
            Resolvers.RadioButtonResolver.Selector('#Partner_FromRadio').Select();
            Resolvers.LOVResolver.Selector('#ShipmentPickUpDelivery_FromPartnerCardId').Type('raza');

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
            Resolvers.RadioButtonResolver.Selector('#Partner_FromRadio').Select();
            Resolvers.LOVResolver.Selector('#ShipmentPickUpDelivery_ToPartnerCardId').Type('raza');

        } else if (PickupDeliveryTypeCode == 'Port') {
            Resolvers.RadioButtonResolver.Selector('#Port_FromRadio').Select();
            Resolvers.LOVResolver.Selector('#ShipmentPickUpDelivery_ToPortId').Type('ez');

        } else {
            Resolvers.RadioButtonResolver.Selector('#CasualAddress_FromRadio').Select();
            Resolvers.TextBoxResolver.Selector('#ShipmentPickUpDelivery_ToAddressCity').Type('Albania');
            Resolvers.LOVResolver.Selector('#ShipmentPickUpDelivery_ToAddressCountryId').Type('Albania');
        }
    }
}