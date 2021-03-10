import { BaseSelectors } from '../selectors/BaseSelectors';
import * as BaseAssertion from '../../cypress/actions/Assertion';
import { AccountingSelectors } from '../../../Accounting/cypress/selectors/Selectors';
import { ShipmentSelectors } from '../../../Shipment/cypress/selectors/Selectors';
import { RestAPI } from '../constants/RestAPI';
import { AccountingURLs } from '../../../Accounting/cypress/constants/URLs';
import { RequestAliases } from "../../../Base/cypress/constants/RequestAliases";

export function NavigatesToMaintenanceMenu() {
    cy.Click(BaseSelectors.MaintenanceMenu, null)
}

export function NavigatesToCustomsSettings() {
    NavigatesToMaintenanceMenu();
    cy.Click(BaseSelectors.SystemSettings, null)
    cy.Click(BaseSelectors.CustomsSettings, null)
}

export function ActivateCustomsManagementInShipments() {
    NavigatesToCustomsSettings()
    cy.get(BaseSelectors.typeCheckbox).check({ force: true })
    cy.Click(BaseSelectors.RedButton, BaseSelectors.ContainsOK)

}

export function ClearExternalIDFromShipmentLevel(ExternalIDName: string) {
    let ExternalIdSelectorTextBox: string;
    let EntitySelector = AccountingSelectors.LogLovId(ExternalIDName)
    let SaveCloseButton = BaseSelectors.SaveClose(ExternalIDName)
    let AccountingSelector = BaseSelectors.AccountTab(ExternalIDName)

    if (ExternalIDName == BaseSelectors.ChargesType) {
        ExternalIdSelectorTextBox = BaseSelectors.ChargesTypeReceivableCreditAccount
    }
    else if (ExternalIDName == BaseSelectors.Currency) {
        ExternalIdSelectorTextBox = BaseSelectors.CurrencyAccountingExternalCode
    }
    else if (ExternalIDName == BaseSelectors.Partner) {
        ExternalIdSelectorTextBox = BaseSelectors.CustomerReceivablesAccountingCard;
    }

    ClickOnMaintenanceButton(EntitySelector)
    ClickOnEditInMaintenanceButton();
    NavigateToAccountTab(ExternalIDName,AccountingSelector)
    cy.get(ExternalIdSelectorTextBox).clear();
    cy.Click(SaveCloseButton, null);
    BaseAssertion.AssertElementNotExist(SaveCloseButton)
}

function ClickOnMaintenanceButton(LogLovSelector: string) {
    cy.ClickingAfterHovering(LogLovSelector, BaseSelectors.MaintenanceButton)
}

 function ClickOnEditInMaintenanceButton() {
    cy.Click(BaseSelectors.MTCPopup,BaseSelectors.ContainsEdit,true)
}

function NavigateToAccountTab(ExternalIDName: string, AccountingSelector: string) {
    cy.Navigate(AccountingSelector);
    if (ExternalIDName == BaseSelectors.ChargesType) {
        cy.DefineRequestWait(RestAPI.GET, AccountingURLs.EntityResourceAccountingPeriod, RequestAliases.EntityResourceAccountingPeriod)
        BaseAssertion.AssertStatusCode(RequestAliases.EntityResourceAccountingPeriod, 200)
    }
}

export function GetstringWithoutLastCharacter(text: string) {
    let textWithoutLastCharacter = text.substring(0, (text.length - 1)).toString();
    return textWithoutLastCharacter;
}

export function GetLastCharacter(text: string) {
    let lastCharacterOfText = text.slice(text.length - 1).toString();
    return lastCharacterOfText
}

export function ClickOnRowDependingOnValue(value: string) {
    cy.get(BaseSelectors.RowCellClass).find(BaseSelectors.TextTrimming).contains(value)
        .parents(BaseSelectors.ListItem).click({ force: true })
}
export function CloseWindow(){
    cy.Click(BaseSelectors.button,BaseSelectors.ContainClose)
}

export function GetTodayDate(){
    var today = new Date
    var dd = today.getUTCDate();
    var mm = today.getUTCMonth()+1
    var yyyy = today.getFullYear();

    let TodayDateFormat = dd<9?"0":""+dd + '/' + "0"+ mm + '/' + yyyy;
    return TodayDateFormat
}