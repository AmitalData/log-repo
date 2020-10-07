import { Resolvers } from "../../Resolvers/Resolvers";

export class PayablesTab {

    public RunPayablesTabScenarios() {
        this.GoToChargesTab();
        this.AddChargeType()
    }
    private GoToChargesTab() {
        cy.get('#QuoteTHCharges').click();
    }
    private AddChargeType() {
        Resolvers.ButtonResolver.Selector('#AddCharges').Click();
        Resolvers.LOVResolver.Selector('#QuoteCharge_ChargesTypeId').Type('del');
        Resolvers.TextBoxResolver.Selector("#edit-log-grid_0_20_1_0").IsEditGrid().Type('678');
        Resolvers.ButtonResolver.Selector('#OKAddCharges').Click();
       
    }
}