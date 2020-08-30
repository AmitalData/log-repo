import { Resolvers } from "../../Resolvers/Resolvers";

export class RoutingTab {

    public RunRoutingTabScenarios() {
        this.GoToRoutingTab();
        this.FillDates();
        this.AddPickupDeilvery();

    }
    private GoToRoutingTab() {
        cy.get('#QuoteTHRoutings').click();
    }
    private FillDates() {
        Resolvers.DatePickerResolver.Selector('#date_Quote_ETD').Type('.');
        Resolvers.DatePickerResolver.Selector('#date_Quote_ETA').Type('.');
    }
    private AddPickupDeilvery() {
        this.addRoute('P');
        this.addRoute('D');

    }
    private addRoute(routeName: string) {
        let routeId: string = null;
        switch (routeName) {
            case "P": { routeId = '#Quote_IncludePickUp'; break; } // Pickup
            case "D": { routeId = '#Quote_IncludeDelivery'; break; } // Delivery 
        }
        cy.get('logcheckbox')
            .find('.CheckBox')
            .within(() => {
                cy.get(routeId).click({ force: true });
            });
    }

}