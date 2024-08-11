import { JournalSelectors } from "../selectors/JournalSelectors";
import { JournalLineActionDetails } from "cypress/models/JournalLineActionDetails";
import { RequestAliases } from "../../../Base/cypress/constants/RequestAliases";
import * as BaseAssertion from '../../../Base/cypress/actions/Assertion';
import { URLs } from '../constants/URLs';
import { RestAPI } from '../../../Base/cypress/constants/RestAPI'
import { BaseSelectors } from "../../../Base/cypress/selectors/BaseSelectors";

export function NavigatesJournalWorkspace() {
    cy.Click(JournalSelectors.JournalTab, null)
    cy.Click(JournalSelectors.NewJournalButton, null)
}

export function FillLineActionDetails(journalLineActionDetails: JournalLineActionDetails) {
    // cy.FillLogLov(JournalSelectors.ActionName, journalLineActionDetails.ActionName, true)
    // FillDropdownInRowTable('C. Account', journalLineActionDetails.CreditAccount);
    // FillDropdownInRowTable('D. Account', journalLineActionDetails.DebitAccount);
    // FillInRowTable('Amount (NIS)', journalLineActionDetails.Amount)
    // cy.get(JournalSelectors.AccountingDate).focus();


    cy.FillLogLov(JournalSelectors.ActionName, journalLineActionDetails.ActionName, true)
    FillDropdownInRowTable('חשבון זכות', journalLineActionDetails.CreditAccount);
    FillDropdownInRowTable('חשבון חובה', journalLineActionDetails.DebitAccount);
    FillInRowTable('סכום (NIS)', journalLineActionDetails.Amount)
    cy.get(JournalSelectors.AccountingDate).focus();
}


function FillDropdownInRowTable(headerText: string, value: string) {
    cy.get(`.ag-header-cell div:contains("${headerText}")`).invoke('attr', 'id').then(id => {
        if (id?.indexOf('HeaderTemplateDiv') > -1) {
            let i = id.replace('HeaderTemplateDiv', '');
            FillGLAccountDDL('[index="' + i + '"]', value);
        }
    });
}


function FillInRowTable(headerText: string, value: string) {
    cy.get(`.ag-header-cell div:contains("${headerText}")`).invoke('attr', 'id').then(id => {
        if (id?.indexOf('HeaderTemplateDiv') > -1) {
            let i = id.replace('HeaderTemplateDiv', '');
            FillGLAccountDDL1('[index="' + i + '"]', value,);
        }
    });
}


export function FillGLAccountDDL(selector, value) {
    cy.get(selector).type(value)
    cy.get(BaseSelectors.DropDownList).contains(value).then(a => {
        a[0].click();
    });
}

export function FillGLAccountDDL1(selector, value) {
    cy.get(selector).type(value)
    
}



export function SaveJournal() {
    cy.DefineRequestWait(RestAPI.POST, URLs.Journals, RequestAliases.PostJournal)
    cy.Click(JournalSelectors.SaveAsDraftButton, null)
}

export function AssertSaveJournal() {
    BaseAssertion.AssertStatusCode(RequestAliases.PostJournal, 200)
}

export function ApproveJournal() {
    cy.DefineRequestWait(RestAPI.PUT, URLs.Journals, RequestAliases.PutJournal)
    cy.Click(JournalSelectors.ApproveButton, null)
}

export function AssertApproveJournal() {
    BaseAssertion.AssertStatusCode(RequestAliases.PutJournal, 200)
    cy.wait(5000)
}

export function PrintReport() {
    cy.DefineWindowOpen(RequestAliases.PrintReportWindowOpen);
    cy.Click(JournalSelectors.MenuButtons, null);
    cy.Click(JournalSelectors.JournalPrint, null);
    cy.Click(JournalSelectors.Printbutton, null,true);
}

export function AssertPrintReport() {
    BaseAssertion.AssertWindowOpen(RequestAliases.PrintReportWindowOpen);
}