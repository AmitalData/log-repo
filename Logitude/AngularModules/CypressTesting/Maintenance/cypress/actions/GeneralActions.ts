import { BaseSelectors } from "../../../Base/cypress/selectors/BaseSelectors";
import { RestAPI } from "../../../Base/cypress/constants/RestAPI";
import { Urls } from "../constants/Urls";
import { RequestAliases } from "../../../Base/cypress/constants/RequestAliases";
import * as BaseAssertion from "../../../Base/cypress/actions/Assertion";

export function Search(SearchFieldValue) {
    DefineViewsGetByFiltersRequest(SearchFieldValue);
    cy.FillLogTextBox(BaseSelectors.SearchTextboxInput, SearchFieldValue);
    AssertViewsGetByFilters();
}

function DefineViewsGetByFiltersRequest(SearchFieldValue: string) {
    cy.DefineRequestWait(RestAPI.GET, Urls.GetFilterSearch(SearchFieldValue), RequestAliases.GetFilterSearch);
}

function AssertViewsGetByFilters() {
    BaseAssertion.AssertStatusCode(RequestAliases.GetFilterSearch, 200);
}

export function AssertSearch(SearchFieldValue) {
    cy.get(BaseSelectors.ListDataLoaded)
    cy.get(BaseSelectors.RowClass).eq(0).invoke(BaseSelectors.TextElement).then((text) => {
        expect(text).to.contain(SearchFieldValue);
    });
}
