import { Resolvers } from "../../Resolvers/Resolvers";


export class QuoteActions {

    public RunQuoteActions() {


    }

    private FillDetailsTab() {
        cy.get('#QuoteTHDetails').click();

        let today = new Date().toLocaleDateString();

        Resolvers.DatePickerResolver.Selector('#date_Quote_StartDate').Type(today);
        Resolvers.DatePickerResolver.Selector('#time_Quote_StartDate').Type('12:00 AM');

        Resolvers.TextBoxResolver.Selector("#Quote_ValueOfGoods").Type('500');
        Resolvers.LOVResolver.Selector("#Quote_ValueOfGoodsCurrencyId").Type('USD');
    }
}