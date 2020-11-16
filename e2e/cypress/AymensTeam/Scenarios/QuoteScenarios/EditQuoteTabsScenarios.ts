import { Resolvers } from "../../Resolvers/Resolvers";
import { PartnersTab } from "./PartnersTab";
import { PackagesTab } from "./PackagesTab";
import { RoutingTab } from "./RoutingTab";
import { ChargesTab } from "./ChargesTab";

export class EditQuoteTabsScenarios {
    public  PartnersTab: PartnersTab = new PartnersTab();
    public PackagesTab: PackagesTab = new PackagesTab();
    public RoutingTab: RoutingTab = new RoutingTab();
    public ChargesTab: ChargesTab = new ChargesTab();

    public RunEditTabsScenarios(direction: string, transportMode: string, shipmentType: string = null, quoteType: string) {
        this.FillDetailsTab();
        this.PartnersTab.RunPartnersTabScenarios(direction, transportMode);
        this.PackagesTab.RunPackagesTabsScenarios(shipmentType, quoteType);
        this.RoutingTab.RunRoutingTabScenarios(direction, transportMode);
        this.ChargesTab.RunChargesTabScenarios();
    }

    private FillDetailsTab() {
        cy.get('#QuoteTHDetails').click();
        
        let today = new Date().toLocaleDateString();

        Resolvers.DatePickerResolver.Selector('#date_Quote_StartDate').Type(today);
        Resolvers.DatePickerResolver.Selector('#time_Quote_StartDate').Type('12:00 AM');

        Resolvers.TextBoxResolver.Selector("#Quote_ValueOfGoods").Type('500');
        Resolvers.LOVResolver.Selector("#Quote_ValueOfGoodsCurrencyId").Type('USD');
    }
}