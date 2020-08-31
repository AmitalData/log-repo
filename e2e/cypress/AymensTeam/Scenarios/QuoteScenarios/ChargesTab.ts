import { Resolvers } from "../../Resolvers/Resolvers";

export class ChargesTab {

    public RunChargesTabScenarios() {
        this.GoToChargesTab();
        this.AddChargeType()
    }
    private GoToChargesTab() {
        cy.get('#QuoteTHCharges').click();
    }
    private AddChargeType() {
        Resolvers.ButtonResolver.Selector('#AddCharges').Click();
        Resolvers.LOVResolver.Selector('#QuoteCharge_ChargesTypeId').Type('fre');

        cy.get('#CostPrice').focus().then(() => {
            cy.get('logtextbox')
                .find('#textboxdiv_QuoteCharge_CostUnitPrice')
                .eq(0)
                .within(() => {
                    cy.get('input').type('5');
                });
            //Resolvers.TextBoxResolver.Selector('#QuoteCharge_CostUnitPrice').Type('10');
            //cy.get('#QuoteCharge_CostUnitPrice').type('10');
        });

        //textboxdiv_QuoteCharge_CostContainerType1UnitPrice
        //QuoteCharge_CostContainerType1UnitPrice
        //logtextbox 
    }
}