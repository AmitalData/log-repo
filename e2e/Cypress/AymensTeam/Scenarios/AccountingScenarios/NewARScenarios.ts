import { Random } from "../../@e2e/core";
import { Resolvers } from "../../Resolvers/Resolvers";

export class NewARScenarios {

    public EntityId: string;
    public EntityNumber: string;
    public RunScenario() {
        this.CreateNewARPayment();
    }
    private CreateNewARPayment() {
        cy.contains('New Payment').click()
        // cy.contains('').click();
        Resolvers.LOVResolver.Selector('#ARPayment_BillToId').Type('TestShipperExport1');
        Resolvers.LOVResolver.Selector('#ARPayment_AccountingPaymentMethodId').Type('Cash');
        Resolvers.TextBoxResolver.Selector('#ARPayment_AmountInPaymentCurrency').Type('100');
        Resolvers.ButtonResolver.Selector('#ok-AddARPayment').Click();
        Resolvers.ButtonResolver.Selector('#ARPayment-Save').Click();
        cy.server();
        cy.route("**/arinvoiceviews/**").as('entityLoaded');
        cy.wait('@entityLoaded');
        cy.get('#ARInvoice_AmountPaid').click({ force: true });
        cy.wait('@entityLoaded');
        cy.get('#ARInvoice_AmountPaid').type('100');
        //cy.get('#APPayment-SaveClose').click({ force: true });
        cy.get('#ARPayment-Save').click();
        cy.get('#EditBackbutton').click();


    }


}