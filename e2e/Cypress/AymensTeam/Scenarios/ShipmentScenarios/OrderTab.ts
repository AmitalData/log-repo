import { Resolvers } from "../../Resolvers/Resolvers";

export class OrderTab {
    private shipmentType: string;
     
    public RunOrderTabScenarios(shipmentType: string = null) {
        this.shipmentType = shipmentType != null ? shipmentType.toLowerCase() : shipmentType;

        this.GoToOrderTab();
        if (this.shipmentType == 'fcl' || this.shipmentType == 'fcld' || this.shipmentType == 'ftl') {
            this.AddContainer('3', '40GP','100');
        } else {
            if (this.shipmentType == 'lcl' || this.shipmentType == 'ltl') {
                this.AddPackage('bal', '5', null, null, null, '10', '100');
            }
            else {
                this.AddPackage(null, '5', '100', '100', '100', null, '100');
                this.AddPackage(null, '5', null, null, null, '10', '100');
            }
        }
    }
    private GoToOrderTab() {
        cy.get('#ShipmentTHOrders').click();
    }
    private AddPackage(packageType: string = null, quantity: string, length: string = null, width: string = null, hight: string = null, volume: string = null, grossweight: string = null) {
        Resolvers.ButtonResolver.Selector('#Orders-AddPackage').Click();
      
        Resolvers.TextBoxResolver.Selector('#ShipmentOrderPackage_Quantity').Type(quantity);
        if (this.shipmentType == 'lcl' || this.shipmentType == 'ltl') {
            Resolvers.LOVResolver.Selector('#ShipmentOrderPackage_PackageTypeId').Type(packageType);
        }
        if (volume == null) {
            Resolvers.TextBoxResolver.Selector('#ShipmentOrderPackage_Length').Type(length);
            Resolvers.TextBoxResolver.Selector('#ShipmentOrderPackage_Width').Type(width);
            Resolvers.TextBoxResolver.Selector('#ShipmentOrderPackage_Height').Type(hight);
        } else {
            Resolvers.TextBoxResolver.Selector('#ShipmentOrderPackage_Volume').Type(volume);
        }
        Resolvers.TextBoxResolver.Selector('#ShipmentOrderPackage_GrossWeight').Type(grossweight);
        Resolvers.ButtonResolver.Selector('#OrderOKbtn').Click();
    }
    private AddContainer(quantity: string, packageType: string, grossWeight: string,) {
        Resolvers.ButtonResolver.Selector('#Orders-AddPackage').Click();
        Resolvers.TextBoxResolver.Selector('#ShipmentOrderPackage_Quantity').Type(quantity);
        Resolvers.LOVResolver.Selector('#ShipmentOrderPackage_PackageTypeId').Type(packageType);
        Resolvers.TextBoxResolver.Selector('#ShipmentOrderPackage_GrossWeight').Type(grossWeight);
        Resolvers.ButtonResolver.Selector('#OrderOKbtn').Click();
    }
}