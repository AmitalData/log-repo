import { Random } from "../../@e2e/core";
import { Resolvers } from "../../Resolvers/Resolvers";

export class NewAPScenarios {
    public EntityId: string;
    public EntityNumber: string;
    public RunScenario() {
        this.EntityNumber = Random.GetRandomNumber();
        this.CreateNewAPPayment();
    }
    private CreateNewAPPayment() {
        cy.contains('New Payment').click();
        Resolvers.LOVResolver.Selector('#APPayment_VendorId').Type('Automation Test');
        Resolvers.LOVResolver.Selector('#APPayment_AccountingPaymentMethodId').Type('Cash');
        Resolvers.TextBoxResolver.Selector('#APPayment_AmountInPaymentCurrency').Type('100');
        Resolvers.LOVResolver.Selector('#APPayment_PaymentCurrencyId').Type('EUR');
    }

}