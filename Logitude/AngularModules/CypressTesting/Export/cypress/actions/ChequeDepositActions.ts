import { ChequeDepositSelectors } from "../selectors/ChequeDepositSelectors";
import { BankAccountsSelectors } from "../selectors/BankAccountsSelectors";
import { ChequeDepositDetails } from "cypress/models/ChequeDepositDetails";
import { BaseSelectors } from "../../../Base/cypress/selectors/BaseSelectors";
import { RequestAliases } from "../../../Base/cypress/constants/RequestAliases";
import * as BaseAssertion from '../../../Base/cypress/actions/Assertion';
import { URLs } from '../constants/URLs';
import { RestAPI } from '../../../Base/cypress/constants/RestAPI'

export function NavigatesChequeDepositWizerd() {
    cy.Click(BankAccountsSelectors.BankAccountsTab, null)
    cy.Click(ChequeDepositSelectors.NewDepositButton, null)

    //cy.Click(BaseSelectors.Backbutton, null)
}

export function FillChequeDepositDetails(chequeDepositDetails: ChequeDepositDetails) {
    
    cy.FillLogTextBox(ChequeDepositSelectors.AccountingDate, chequeDepositDetails.AccountingDate)
    cy.FillLogLov(ChequeDepositSelectors.CashBook, chequeDepositDetails.CashBook, true)
    cy.FillLogLov(ChequeDepositSelectors.BankAccount, chequeDepositDetails.BankAccount, true)
}

export function CreateChequeDeposit() {
    cy.DefineRequestWait(RestAPI.GET, URLs.CashbookViewGetSingle, RequestAliases.CashbookViewGetSingle)
    cy.Click(BaseSelectors.RedButton + BaseSelectors.LastElement, null);
}

export function AssertCreateChequeDeposit() {
    BaseAssertion.AssertStatusCode(RequestAliases.CashbookViewGetSingle, 200)
}

export function SelectAllCheques() {
    cy.Click(BaseSelectors.CheckBoxLine + BaseSelectors.FirstElement, null)
}

export function ApproveChequeDeposit() {
    cy.DefineRequestWait(RestAPI.POST, URLs.BankDeposits, RequestAliases.PostChequeDeposit)
    cy.Click(ChequeDepositSelectors.ApproveButton, null)
}

export function AssertApproveChequeDeposit() {
    BaseAssertion.AssertStatusCode(RequestAliases.PostChequeDeposit, 200)
}