//import { Random } from "../../@e2e/core";
//import { Resolvers } from "../../Resolvers/Resolvers";

//export class NewARScenarios {

//    public EntityId: string;
//    public EntityNumber: string;
//    public RunScenario() {
//        this.CreateNewARPayment();
//    }
//    private CreateNewARPayment() {
//        cy.contains('New Payment').click()
//        Resolvers.LOVResolver.Selector('#ARPayment_BillToId').Type('TestShipperExport1');
//        Resolvers.LOVResolver.Selector('#ARPayment_AccountingPaymentMethodId').Type('Cash');
//        Resolvers.LOVResolver.Selector('#ARPayment_PaymentCurrencyId').Type('NIS');
//        Resolvers.TextBoxResolver.Selector('#ARPayment_AmountInPaymentCurrency').Type('100');
//        Resolvers.ButtonResolver.Selector('#ok-AddARPayment').Click();
//        Resolvers.ButtonResolver.Selector('#ARPayment-Save').Click();
//        cy.server();
//        cy.route("**/arinvoiceviews/**").as('entityLoaded');
//        cy.wait('@entityLoaded');
//        cy.get('#ARInvoice_AmountPaid').click({ force: true });
//        cy.wait('@entityLoaded');
//        cy.get('#ARInvoice_AmountPaid').type('100');
//        Resolvers.ButtonResolver.Selector('#ARPayment-Save').Click();
//        cy.get('#EditBackbutton').click();
//    }
//}