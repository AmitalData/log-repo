import { Resolvers } from "../../Resolvers/Resolvers";

export class ChargesTab {

    public RunChargesTabScenarios() {
        this.GoToChargesTab();

    }
    private GoToChargesTab() {
        cy.get('#QuoteTHCharges').click();
    }
}