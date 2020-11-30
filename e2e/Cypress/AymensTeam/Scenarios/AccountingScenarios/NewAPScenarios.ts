import { Random } from "../../@e2e/core";
import { Resolvers } from "../../Resolvers/Resolvers";

export class NewAPScenarios {

    public EntityId: string;
    public EntityNumber: string;
    public RunScenario() {
        this.CreateNewAPPayment();
    }
    private CreateNewAPPayment() {
        cy.get("#NewAPPayment").contains('New Payment').click()
       // cy.contains('').click();
        Resolvers.LOVResolver.Selector('#APPayment_VendorId').Type('AR');
        Resolvers.LOVResolver.Selector('#APPayment_AccountingPaymentMethodId').Type('Cash');
        Resolvers.TextBoxResolver.Selector('#APPayment_AmountInPaymentCurrency').Type('100');
        Resolvers.LOVResolver.Selector('#APPayment_PaymentCurrencyId').Type('NIS');
        Resolvers.ButtonResolver.Selector('#APPayment-Save').Click();
        cy.server();
        cy.route("**/apinvoiceviews/**").as('entityLoaded');
        cy.wait('@entityLoaded');
        cy.get('#APInvoice_AmountPaid').click({ force: true });
        cy.wait('@entityLoaded');
        cy.get('#APInvoice_AmountPaid').type('100'); 
        cy.wait('@entityLoaded');
        //cy.get('#APPayment-SaveClose').click({ force: true });
        Resolvers.ButtonResolver.Selector('#APPayment-Save').Click();
        cy.get('#EditBackbutton').click();

       
    }
 
   
}