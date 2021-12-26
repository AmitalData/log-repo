import { MaintenanceSelectors } from "../selectors/Selectors";
import { Constants } from "../constants/Constants";
import { RequestAliases } from "../../../Base/cypress/constants/RequestAliases";
import * as BaseAssertion from "../../../Base/cypress/actions/Assertion";
import { InvoiceSettingsDetails } from "../models/InvoiceSettingsDetails";
import { AccountingSettingsDetails } from "../models/AccountingSettingsDetails";
import { RestAPI } from "../../../Base/cypress/constants/RestAPI";
import { Urls } from "../constants/Urls";
import { BaseSelectors } from "../../../Base/cypress/selectors/BaseSelectors";

//#region Accounting Settings
export function ChangeAccountingSettings(accountingSettings: AccountingSettingsDetails) {

    if (accountingSettings.VoidARInvoice!= null && (accountingSettings.VoidARInvoice.toUpperCase() == Constants.Allowed)) {
        cy.get(MaintenanceSelectors.AllowVoidARICheckBox).check({ force: true })
    }
    else if (accountingSettings.VoidARInvoice!= null && (accountingSettings.VoidARInvoice.toUpperCase() == Constants.NotAllowed)) {
        cy.get(MaintenanceSelectors.AllowVoidARICheckBox).uncheck({ force: true })
    }

    if (accountingSettings.VoidAPInvoice!= null &&(accountingSettings.VoidAPInvoice.toUpperCase() == Constants.Allowed)) {
        cy.get(MaintenanceSelectors.AllowVoidAPICheckBox).check({ force: true })
    }
    else if (accountingSettings.VoidAPInvoice!= null &&(accountingSettings.VoidAPInvoice.toUpperCase() == Constants.NotAllowed)) {
        cy.get(MaintenanceSelectors.AllowVoidAPICheckBox).uncheck({ force: true })
    }

    if (accountingSettings.voidARPayment!= null && (accountingSettings.voidARPayment.toUpperCase() == Constants.Allowed)) {
        cy.get(MaintenanceSelectors.AllowVoidARPayment).check({ force: true })
    }
    else if (accountingSettings.voidARPayment!= null && (accountingSettings.voidARPayment.toUpperCase() == Constants.NotAllowed) ){
        cy.get(MaintenanceSelectors.AllowVoidARPayment).uncheck({ force: true })
    }

    if (accountingSettings.voidAPPayment!=null && (accountingSettings.voidAPPayment.toUpperCase() == Constants.Allowed)) {
        cy.get(MaintenanceSelectors.AllowVoidAPPayment).check({ force: true })
    }
    else if (accountingSettings.voidAPPayment!= null && (accountingSettings.voidAPPayment.toUpperCase() == Constants.NotAllowed)) {
        cy.get(MaintenanceSelectors.AllowVoidAPPayment).uncheck({ force: true })
    }
}
export function MockSave(url) {
    cy.intercept(RestAPI.PUT, url, [true])
    DefineUpdateAcountingSettingsrequest()
    cy.Click(BaseSelectors.RedButton + BaseSelectors.LastElement, null);
}
export function DefineUpdateAcountingSettingsrequest() {
    cy.DefineRequestWait(RestAPI.GET, Urls.GetByFilter, RequestAliases.AccountingSettings);
}
export function AssertMockSave() {
    BaseAssertion.AssertElementNotExist(BaseSelectors.LogitudeWindow)
}
export function FillAccountingSettingsVATNumber(VATNumber: string) {
    cy.FillLogTextBox(MaintenanceSelectors.VATNumber, VATNumber);
}

//#endregion