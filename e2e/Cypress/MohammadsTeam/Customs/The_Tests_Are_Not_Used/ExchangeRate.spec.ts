import { LogHelper } from '../../Helpers/LogHelper';
import { LoginComp } from '../../Login/Login.po';


export class ExchangeRateSpec {

    private login: LoginComp = new LoginComp();
}

describe('Exchange Rate Queried', () => {
    it('Exchange Rate Got Results', function () {

        LogHelper.ClickListItem('GeneralMHCustoms');
        cy.get('#RequestSheet_8347').click(); // 8347 - customs request interface code
        LogHelper.TypeInput('date_Customs.Declaration_FromDate','-999');
        LogHelper.TypeInput('date_Customs.Declaration_ToDate','.');
        LogHelper.LOVSearchAndSelectFirst("Customs.Declaration_CurrencyTypeCode", 'USD'); 
        LogHelper.ClickButton('CustomSendOptionsComponent_3');

        LogHelper.AssertEditGridHaveItems("CustomsRequest_ExchangeRates_RatesGrid");

        cy.log('ExchangeRate Got Results');
    });
});

