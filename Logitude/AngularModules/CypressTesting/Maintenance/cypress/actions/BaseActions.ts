import { BaseSelectors } from "../../../Base/cypress/selectors/BaseSelectors";
import { RestAPI } from "../../../Base/cypress/constants/RestAPI";
import { Urls } from "../constants/Urls";
import { RequestAliases } from "../../../Base/cypress/constants/RequestAliases";
import * as BaseAssertion from "../../../Base/cypress/actions/Assertion";
import * as Actions from "./Actions"
import { ShippingLineSelectors } from "../Selectors/ShippingLineSelectors";
import { AddressDetails } from 'cypress/models/AddressDetails';
import { AddressSelectors } from "../selectors/AddressSelectors";

export function Search(searchFieldValue) {
    DefineViewsGetByFiltersRequest(searchFieldValue);
    cy.FillLogTextBox(BaseSelectors.SearchTextboxInput, searchFieldValue);
}

function DefineViewsGetByFiltersRequest(searchFieldValue: string) {
    cy.DefineRequestWait(RestAPI.GET, Urls.GetFilterSearch(searchFieldValue), RequestAliases.GetFilterSearch);
}

function AssertViewsGetByFilters() {
    BaseAssertion.AssertStatusCode(RequestAliases.GetFilterSearch, 200);
}

export function AssertSearch(searchFieldValue) {
    AssertViewsGetByFilters();
    cy.get(BaseSelectors.ListDataLoaded)
    cy.get(BaseSelectors.RowClass).eq(0).invoke(BaseSelectors.TextElement).then((text) => {
        expect(text).to.contain(searchFieldValue);
    });
}

export function MockCreate(url) {
    cy.intercept(RestAPI.POST, url, [true])
    Actions.DefineGetByFilterRequest()
    cy.Click(BaseSelectors.RedButton + BaseSelectors.LastElement, null);
}

export function AssertMockCreate() {
    BaseAssertion.AssertElementNotExist(BaseSelectors.LogitudeWindow)
    Actions.AssertGetByFilters();
}

export function ValidateSingleErrorMessage(Message: string) {
    cy.get(BaseSelectors.SingleError).should("contain.text", Message)
}

export function MockImport() {
    cy.intercept(RestAPI.GET, Urls.ImportShippingAirLine, [true])
    Actions.DefineGetByFilterRequest()
    cy.get(ShippingLineSelectors.Import).first().click()
}

export function AssertMockImport() {
    Actions.AssertGetByFilters();
}

export function NavigateAddressWizard() {
    cy.get(AddressSelectors.EditButton).click({ force: true })
}

export function FillAddressDetails(addressDetails: AddressDetails) {
    cy.FillLogTextBox(AddressSelectors.Name, addressDetails.Name)
    cy.FillLogLov(AddressSelectors.Country, addressDetails.Country, true)
    cy.FillLogTextBox(AddressSelectors.City, addressDetails.City)
    cy.FillLogLov(AddressSelectors.State, addressDetails.State, true)
}

export function CreateAddress() {
    cy.DefineRequestWait(RestAPI.POST, Urls.Address, RequestAliases.PostAddress)
    cy.Click(BaseSelectors.RedButton, BaseSelectors.ContainsOK);
}

export function AssertCreateAddress() {
    BaseAssertion.AssertStatusCode(RequestAliases.PostAddress, 200);
}