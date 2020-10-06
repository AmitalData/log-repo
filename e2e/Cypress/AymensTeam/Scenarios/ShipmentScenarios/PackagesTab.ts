import { Resolvers } from "../../Resolvers/Resolvers";

export class PackagesTab {
    private shipmentType: string;
    private transportMode: string;

    public RunPackagesTabsScenarios(shipmentType: string = null, transportMode: string) {
        this.shipmentType = shipmentType.toLowerCase();
        this.transportMode = transportMode;
        this.GoToPackagesTab();
        if (this.shipmentType == 'fcl' || this.shipmentType == 'fcld' ||this.shipmentType == 'ftl') {
            this.AddContainer('40GP', '3');
        } else {
            if (this.shipmentType.toLowerCase() == 'lcl' || this.shipmentType.toLowerCase() == 'ltl') {
                this.AddPackage('bal', '5', null, null, null, '10', '100');
            }
            else {
                this.AddPackage(null, '5', '100', '100', '100', null, '100');
                this.AddPackage(null, '5', null, null, null, '10', '100');
            }
        }
    }
    private GoToPackagesTab() {
        cy.get('#ShipmentTHPackages').click();
    }
    private AddPackage(packageType: string = null, quantity: string, length: string = null, width: string = null, hight: string = null, volume: string = null, grossweight: string = null) {
        Resolvers.ButtonResolver.Selector('#AddPackage').Click();
      
        Resolvers.TextBoxResolver.Selector('#ShipmentPackage_Quantity').Type(quantity);
        if (this.shipmentType == 'lcl' || this.shipmentType == 'ltl') {
            Resolvers.LOVResolver.Selector('#ShipmentPackage_PackageTypeId').Type(packageType);
        }
        if (volume == null) {
            Resolvers.TextBoxResolver.Selector('#ShipmentPackage_Length').Type(length);
            Resolvers.TextBoxResolver.Selector('#ShipmentPackage_Width').Type(width);
            Resolvers.TextBoxResolver.Selector('#ShipmentPackage_Height').Type(hight);
        } else {
            Resolvers.TextBoxResolver.Selector('#ShipmentPackage_Volume').Type(volume);
        }
        Resolvers.TextBoxResolver.Selector('#ShipmentPackage_Weight').Type(grossweight);
        if (this.transportMode == 'A') {
            Resolvers.ButtonResolver.Selector('#OkAirPackage').Click();
        } else {
            Resolvers.ButtonResolver.Selector('#OkOceanPackage').Click();
        }
    }
    private AddContainer(packageType: string, weight: string) {
        Resolvers.ButtonResolver.Selector('#AddPackage').Click();
        Resolvers.LOVResolver.Selector('#ShipmentPackage_PackageTypeId').Type(packageType);
        Resolvers.TextBoxResolver.Selector('#ShipmentPackage_Weight').Type(weight);
        Resolvers.ButtonResolver.Selector('#OkOceanPackage').Click();

    }
}