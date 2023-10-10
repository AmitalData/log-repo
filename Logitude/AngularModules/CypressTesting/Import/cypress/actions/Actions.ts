import { BaseSelectors } from "../../../Base/cypress/selectors/BaseSelectors";
//import { BaseExportSelectors } from "../selectors/BaseExportSelectors";
//import { RegexSelectors } from "../selectors/RegexSelectors";
import { RequestAliases } from "../../../Base/cypress/constants/RequestAliases";
import * as BaseAssertion from '../../../Base/cypress/actions/Assertion';
import { RestAPI } from '../../../Base/cypress/constants/RestAPI'
import { URLs } from '../constants/URLs';
import { Constants } from '../constants/Constants';
import { IsChangedSelectors } from "../selectors/IsChangedSelectors";
import { IsChangedSIDetails } from "cypress/models/IsChangedSIDetails";
import { IsChangedDetails } from "cypress/models/IsChangedDetails";
import { IsChangedSISelectors } from "cypress/selectors/IsChangedSISelectors";

export function NavigatesImportDeclarationWorkspace() {

    cy.Click(IsChangedSelectors.GeneralMHDeclarationsTab, null)
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


export function SendToCustomsButtonSimulator() {

    cy.get('.x-button-drop').click();
    cy.get('li:nth-child(5) > span').click();

}


export function SendToCustomsButtonSimulator1(isChangedDetails: IsChangedDetails) {

    cy.get('senddeclarationtastcasecomponent combobox .ComboBox table td img').type(isChangedDetails.Scen1);
    cy.get('senddeclarationtastcasecomponent combobox .ComboBox .ComboBoxDropdown  div ul li:eq(0)').click()
    cy.Click(IsChangedSelectors.Save, null, true);

    cy.get(IsChangedSelectors.Approve).then($element => 
        {   if ($element.length > 0) {     cy.wrap($element).click();   } }); 

    //cy.Click(IsChangedSelectors.Approve, null, true);
    cy.Click(IsChangedSelectors.IsChangedGeneral, null)



    // let errorMessage = '';
    // do {
    //     cy.Click(IsChangedSelectors.Approve, null, true);
    //     if (errorMessage) {

    //     }
    // } while (errorMessage);


}

    




