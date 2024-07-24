import { BaseSelectors } from "../../../Base/cypress/selectors/BaseSelectors";
import { RegexSelectors } from "cypress/selectors/RegexSelectors";
import { RequestAliases } from "../../../Base/cypress/constants/RequestAliases";
import * as BaseAssertion from '../../../Base/cypress/actions/Assertion';
import { RestAPI } from '../../../Base/cypress/constants/RestAPI'
import { URLs } from '../constants/URLs';
import { Constants } from '../constants/Constants';

export function NavigatesFullAccounting() {
    cy.Click(BaseSelectors.FullAccountingTab, null)
}

export function Search(searchFieldValue) {
    DefineViewsGetByFiltersRequest(searchFieldValue);
    cy.get(BaseSelectors.SearchTextboxInput).type(searchFieldValue);
}

function DefineViewsGetByFiltersRequest(searchFieldValue: string) {
    cy.DefineRequestWait(RestAPI.GET, URLs.GetFilterSearch(searchFieldValue), RequestAliases.GetFilterSearch);
}

export function AssertViewsGetByFilters() {
    BaseAssertion.AssertStatusCode(RequestAliases.GetFilterSearch, 200);
}

export function AssertSearch(searchFieldValue) {
    AssertViewsGetByFilters();
    cy.get(BaseSelectors.ListDataLoaded)
    cy.get(BaseSelectors.RowClass).eq(0).invoke(BaseSelectors.TextElement).then((text) => {
        expect(text).to.contain(searchFieldValue);
    });
}

export function AssertGetSingle() {
    BaseAssertion.AssertStatusCode(RequestAliases.GetSignle, 200);
}

export function OpenMaintenanceItemFromMaintenanceMenu(maintenanceItemNameToSearch: string, maintenanceItemSelector: string) {
    cy.Click(BaseSelectors.MaintenanceMenu, null)
    cy.FillLogTextBox(BaseSelectors.NullSearch, maintenanceItemNameToSearch);
    cy.Click(maintenanceItemSelector, null);
}

export function OpenNewWizard(tabName: string) {
    cy.Click(RegexSelectors.NewWizardButton(tabName), null);
}

export function FillCheckBoxProcess(CheckBoxSelector: string, IsCheck: string) {
    if (IsCheck) {
        if (IsCheck.toUpperCase() == Constants.YES) {
            cy.get(CheckBoxSelector).check({ force: true })
        }
        else {
            cy.get(CheckBoxSelector).find(BaseSelectors.input).uncheck({ force: true })
        }
    }
}