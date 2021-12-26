import { MaintenanceSelectors } from "../selectors/Selectors";
import { Constants } from "../constants/Constants";
import { RequestAliases } from "../../../Base/cypress/constants/RequestAliases";
import * as BaseAssertion from "../../../Base/cypress/actions/Assertion";
import { AccountingSettingsDetails } from "../models/AccountingSettingsDetails";
import { RestAPI } from "../../../Base/cypress/constants/RestAPI";
import { Urls } from "../constants/Urls";
import { BaseSelectors } from "../../../Base/cypress/selectors/BaseSelectors";
import * as Actions from "./Actions"

//#region Accounting Settings
export function ChangeAccountingSettings(accountingSettings: AccountingSettingsDetails) {
    Actions.FillInputCheckBoxProcess(MaintenanceSelectors.AllowVoidARICheckBox, accountingSettings.VoidARInvoice)
    Actions.FillInputCheckBoxProcess(MaintenanceSelectors.AllowVoidAPICheckBox, accountingSettings.VoidAPInvoice)
    Actions.FillInputCheckBoxProcess(MaintenanceSelectors.AllowVoidARPayment, accountingSettings.voidARPayment)
    Actions.FillInputCheckBoxProcess(MaintenanceSelectors.AllowVoidAPPayment,accountingSettings.voidAPPayment)
}

export function FillAccountingSettingsVATNumber(VATNumber: string) {
    cy.FillLogTextBox(MaintenanceSelectors.VATNumber, VATNumber);
}


export function UpdateAccountingSettings() {
    DefinePutAccountingSettingsRequest();
    cy.Click(BaseSelectors.RedButton, BaseSelectors.ContainsOK);
}

function DefinePutAccountingSettingsRequest() {
    cy.DefineRequestWait(RestAPI.PUT, Urls.AccountingSettings, RequestAliases.AccountingSettings);
}

export function AssertUpdateInvoiceSettings() {
    AssertPutAccountingSettings();
}

function AssertPutAccountingSettings() {
    BaseAssertion.AssertStatusCode(RequestAliases.AccountingSettings, 200)
}
//#endregion