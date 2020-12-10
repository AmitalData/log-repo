import { LoginComp } from "../../login/Login.po";
import { LogHelper } from '../../Helpers/LogHelper';

export class ClaimSpec {

    private login: LoginComp = new LoginComp();
}

describe('New Claim', () => {
    it('New Claim Created', function () {

        LogHelper.ClickListItem('GeneralMHClaims');
        LogHelper.ClickButton('NewButton_CustomsClaim');
        LogHelper.LOVSearchAndSelectFirst("Customs.Claim_CustomerId", '1'); 
        LogHelper.LOVSearchAndSelectFirst("Customs.Claim_ClaimOfficeCode", '1'); 
        LogHelper.ClickButton('Customs_NewClaimWindow_OkButton');

        cy.log('Claim Created Successfully');
    });
});

