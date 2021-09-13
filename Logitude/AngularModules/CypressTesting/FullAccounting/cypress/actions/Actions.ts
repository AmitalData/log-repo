import { BaseSelectors } from "../../../Base/cypress/selectors/BaseSelectors";
import { RequestAliases } from "../../../Base/cypress/constants/RequestAliases";
import * as BaseAssertion from '../../../Base/cypress/actions/Assertion';
import { RestAPI } from '../../../Base/cypress/constants/RestAPI'
import { URLs } from '../constants/URLs';

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