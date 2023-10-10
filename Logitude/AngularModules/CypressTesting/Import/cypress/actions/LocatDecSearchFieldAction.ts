import { SearchFieldDetails } from "cypress/models/SearchFieldDetails";
import { BaseSelectors } from "../../../Base/cypress/selectors/BaseSelectors";
import { RequestAliases } from "../constants/RequestAliases";
import * as BaseAssertion from '../../../Base/cypress/actions/Assertion';
import { URLs } from '../constants/URLs';
import { RestAPI } from '../../../Base/cypress/constants/RestAPI';
import { SearchFieldSelectors } from "../selectors/SearchFieldSelectors";



export function NavigatesImportDeclarationWorkspace() {
    cy.Click(SearchFieldSelectors.GeneralMHDeclarationsTab, null)

}

export function FillSearchField(searchFieldDetails: SearchFieldDetails) {
    cy.FillLogTextBox(SearchFieldSelectors.SearchField, searchFieldDetails.Declaration, true) 

}

export function Compare(searchFieldDetails: SearchFieldDetails) {
    debugger
    BaseAssertion.AssertElementExist(SearchFieldSelectors.FindDecInGrid.replace("{0}",searchFieldDetails.Declaration))

}

export function AssertSaveCompare()
{
    BaseAssertion.AssertElementNotExist(BaseSelectors.LogitudeWindow);
    BaseAssertion.AssertStatusCode(RequestAliases.LocatFileSearchField, 200)
}

