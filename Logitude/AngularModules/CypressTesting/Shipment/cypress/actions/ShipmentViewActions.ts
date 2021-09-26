import { ShipmentSelectors } from '../selectors/Selectors';
import { BaseSelectors } from '../../../Base/cypress/selectors/BaseSelectors';
import * as BaseAssertion from '../../../Base/cypress/actions/Assertion';
import { URLs } from '../constants/URLs';
import { RestAPI } from '../../../Base/cypress/constants/RestAPI';
import { RequestAliases } from '../../../Base/cypress/constants/RequestAliases';

export function NavigateToShipmentViewWizerd() {
    cy.Click(ShipmentSelectors.ShipmentList, null)
    cy.Click(ShipmentSelectors.ShipmentViewQueryList, null)
    cy.Click(ShipmentSelectors.AddNewShipmentViewHyperLink, null, true)
}

export function FillShipmentViewName(viewName) {
    cy.FillLogTextBox(ShipmentSelectors.ShipmentViewName, viewName)
}

export function AddColumnToSelectedCoulmns(columnName) {
    cy.FillLogTextBox(ShipmentSelectors.ViewSearchField, columnName)
    cy.get(ShipmentSelectors.ListBoxItem).eq(0).click();
    cy.Click(ShipmentSelectors.ViewAddButton, null)
}

export function CreateShipmentView() {
    cy.DefineRequestWait(RestAPI.POST, URLs.Queries, RequestAliases.PostQuery);
    cy.Click(BaseSelectors.RedButton + BaseSelectors.LastElement, null);
}

export function AssertCreateShipmentView() {
    BaseAssertion.AssertStatusCode(RequestAliases.PostQuery, 200);
}

export function EditShipmentView() {
    cy.Click(ShipmentSelectors.ShipmentViewQueryList, null)
    cy.Click(ShipmentSelectors.EditViewButton, null, true)
    cy.get(ShipmentSelectors.ListBoxItem).eq(0).click();
    cy.Click(ShipmentSelectors.ViewAddButton, null)
}

export function UpdateShipmentView() {
    cy.DefineRequestWait(RestAPI.PUT, URLs.Queries, RequestAliases.PutQuery);
    cy.Click(BaseSelectors.RedButton + BaseSelectors.LastElement, null);
}

export function AssertUpdateShipmentView() {
    BaseAssertion.AssertStatusCode(RequestAliases.PutQuery, 200);
}

export function DeleteShipmentView() {
    cy.Click(ShipmentSelectors.ShipmentViewQueryList, null)
    cy.DefineRequestWait(RestAPI.DELETE, URLs.DeleteQuery, RequestAliases.DeleteQuery);
    cy.Click(ShipmentSelectors.ViewDeleteButton, null, true)
    cy.get(ShipmentSelectors.ConfirmWindowYes).click()
}

export function AssertDeleteShipmentView() {
    BaseAssertion.AssertStatusCode(RequestAliases.DeleteQuery, 200);
}