import { Resolvers } from "../../Resolvers/Resolvers";

export class ChargesTab {

    public RunChargesTabScenarios() {
        this.GoToChargesTab();
        this.AddCharges()
    }
    private GoToChargesTab() {
        cy.get('#QuoteTHCharges').click();
    }
    private AddCharges() {
        this.AddChargeType('Del');
        //this.AddChargeType('Del');
    }
    private AddChargeType(chargeType: string) {
        Resolvers.ButtonResolver.Selector('#AddCharges').Click();
        Resolvers.LOVResolver.Selector('#QuoteCharge_ChargesTypeId').Type(chargeType);
        Resolvers.LOVResolver.Selector('#QuoteCharge_CostMeasurementId').Type('fix');
        Resolvers.LOVResolver.Selector('#QuoteCharge_SaleMeasurementId').Type('fix');
        //Resolvers.TextBoxResolver.Selector("#edit-log-grid_0_20_1_0").IsEditGrid().Type('678');
        Resolvers.TextBoxResolver.Selector("#CostPrice").IsEditGrid().Type('50');
        Resolvers.ButtonResolver.Selector('#OKAddCharges').Click();
    }
}