import { Resolvers } from "../../Resolvers/Resolvers";

export class PackagesTab {
    private shipmentType: string;
    private quoteType: string;
    public RunPackagesTabsScenarios(shipmentType: string = null, quoteType: string) {
        this.shipmentType = shipmentType.toLowerCase();
        this.quoteType = quoteType.toUpperCase();

        this.GoToPackagesTab();
        if (this.shipmentType == 'fcl' || this.shipmentType == 'fcld' || this.shipmentType == 'ftl') {
            this.AddContainer('3', '40GP');
        } else {
            if (this.quoteType != 'RR') {
                if (this.shipmentType == 'lcl' || this.shipmentType == 'ltl') {
                    this.AddPackage('bal', '5', null, null, null, '10', '100');
                }
                else {
                    this.AddPackage(null, '5', '100', '100', '100', null, '100');
                    this.AddPackage(null, '5', null, null, null, '10', '100');
                }
            }
        }
    }
    private GoToPackagesTab() {
        cy.get('#QuoteTHPackages').click();
    }
    private AddPackage(packageType: string = null, quantity: string, length: string = null, width: string = null, hight: string = null, volume: string = null, grossweight: string = null) {
        Resolvers.ButtonResolver.Selector('#AddPackage').Click();
      
        Resolvers.TextBoxResolver.Selector('#QuotePackage_Quantity').Type(quantity);
        if (this.shipmentType == 'lcl' || this.shipmentType == 'ltl') {
            Resolvers.LOVResolver.Selector('#QuotePackage_PackageTypeId').Type(packageType);
        }
        if (volume == null) {
            Resolvers.TextBoxResolver.Selector('#QuotePackage_Length').Type(length);
            Resolvers.TextBoxResolver.Selector('#QuotePackage_Width').Type(width);
            Resolvers.TextBoxResolver.Selector('#QuotePackage_Height').Type(hight);
        } else {
            Resolvers.TextBoxResolver.Selector('#QuotePackage_Volume').Type(volume);
        }
        Resolvers.TextBoxResolver.Selector('#QuotePackage_GrossWeight').Type(grossweight);
        Resolvers.ButtonResolver.Selector('#OkAddPackage').Click();
    }
    private AddContainer(quantity: string, packageType: string) {
        if (this.quoteType != 'RR') {
            Resolvers.TextBoxResolver.Selector('#Quote_PackageType1Quantity').Type(quantity);
        }
        Resolvers.LOVResolver.Selector('#Quote_PackageType1Id').Type(packageType);
    }
}