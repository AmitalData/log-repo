import { Resolvers } from "../../Resolvers/Resolvers";
import { PartnersTab } from "./PartnersTab";
import { PackagesTab } from "./PackagesTab";
import { RoutingTab } from "./RoutingTab";
import { PayablesTab } from "./PayablesTab";
import { ReceivablesTab } from "./ReceivablesTab";
import { OrderTab } from "./OrderTab";

export class EditShipmentTabsScenarios {
    public PartnersTab: PartnersTab = new PartnersTab();
    public PackagesTab: PackagesTab = new PackagesTab();
    public RoutingTab: RoutingTab = new RoutingTab();
    public PayablesTab: PayablesTab = new PayablesTab();
    public ReceivablesTab: ReceivablesTab = new ReceivablesTab();
    public OrderTab: OrderTab = new OrderTab();

    public RunEditTabsScenarios(levelCode: string, transportMode: string, direction: string, shipmentType: string = null) {
        //this.FillGeneralTab();
        //this.OrderTab.RunOrderTabScenarios(shipmentType);
        //this.PartnersTab.RunPartnersTabScenarios(levelCode,direction, transportMode);
        //this.PackagesTab.RunPackagesTabsScenarios(shipmentType, transportMode);
        //this.RoutingTab.RunRoutingTabScenarios(levelCode,transportMode);
        //this.PayablesTab.RunPayablesTabScenarios();
         this.ReceivablesTab.RunReceivablesTabScenarios();
    }
    private FillGeneralTab() {
        cy.get('#ShipmentTHGeneral').click();
        Resolvers.TextBoxResolver.Selector("#Shipment_ValueOfGoods").Type('500');
        Resolvers.LOVResolver.Selector("#Shipment_ValueOfGoodsCurrencyId").Type('USD');
    }
}