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
            case "P": { routeId = '#Add-PickUp'; break; } // Pickup
            case "D": { routeId = '#Add-Delivery'; break; } // Delivery 
        }
        Resolvers.ButtonResolver.Selector(routeId).Click();
        Resolvers.ButtonResolver.Selector('#CloseBtn').Click();
        Resolvers.ButtonResolver.Selector('#ConfirmWindow_Yes_0').Click();

        
        //cy.get('logcheckbox')
        //    .find('.CheckBox')
        //    .within(() => {
        //        cy.get(routeId).click({ force: true });
        //    });
    }

}