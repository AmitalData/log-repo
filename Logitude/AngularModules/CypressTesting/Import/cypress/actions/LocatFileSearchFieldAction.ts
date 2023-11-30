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
    cy.FillLogTextBox(SearchFieldSelectors.SearchField, searchFieldDetails.File, true);

}

    export function Compare(searchFieldDetails: SearchFieldDetails) {
        
    BaseAssertion.AssertElementExist(SearchFieldSelectors.FindFileInGrid.replace("{0}",searchFieldDetails.File))

}


export function AssertSaveCompare()
{
    BaseAssertion.AssertElementNotExist(BaseSelectors.LogitudeWindow);
    BaseAssertion.AssertStatusCode(RequestAliases.LocatFileSearchField, 200)
}

// export function SaveDeclaretion()
// {
//     cy.DefineRequestWait(RestAPI.PUT, URLs.IsChanged, RequestAliases.IsChanged)
//     cy.Click(IsChangedSelectors.IsChangedSaveButton, null);
// }

// export function AssertSaveDeclaretion()
// {
//     BaseAssertion.AssertElementNotExist(BaseSelectors.LogitudeWindow);
//     BaseAssertion.AssertStatusCode(RequestAliases.IsChanged, 200)
// }

