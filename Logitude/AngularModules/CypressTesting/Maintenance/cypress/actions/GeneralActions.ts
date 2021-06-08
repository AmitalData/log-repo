import { BaseSelectors } from "../../../Base/cypress/selectors/BaseSelectors";
import { RestAPI } from "../../../Base/cypress/constants/RestAPI";
import { Urls } from "../constants/Urls";
import { RequestAliases } from "../../../Base/cypress/constants/RequestAliases";
import * as BaseAssertion from "../../../Base/cypress/actions/Assertion";

export function Search(searchFieldValue) {
    DefineViewsGetByFiltersRequest(searchFieldValue);
    cy.FillLogTextBox(BaseSelectors.SearchTextboxInput, searchFieldValue);
    AssertViewsGetByFilters();
}

function DefineViewsGetByFiltersRequest(searchFieldValue: string) {
    cy.DefineRequestWait(RestAPI.GET, Urls.GetFilterSearch(searchFieldValue), RequestAliases.GetFilterSearch);
}

function AssertViewsGetByFilters() {
    BaseAssertion.AssertStatusCode(RequestAliases.GetFilterSearch, 200);
}

export function AssertSearch(searchFieldValue) {
    cy.get(BaseSelectors.ListDataLoaded)
    cy.get(BaseSelectors.RowClass).eq(0).invoke(BaseSelectors.TextElement).then((text) => {
        expect(text).to.contain(searchFieldValue);
    });
}
