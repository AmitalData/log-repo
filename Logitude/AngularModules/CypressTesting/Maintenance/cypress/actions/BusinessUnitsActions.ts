import { BusinessUnitSelectors } from "../selectors/BusinessUnitSelectors";
import { MaintenanceSelectors } from "../selectors/Selectors";
import * as Actions from "./Actions";
import * as GeneralActions from "./GeneralActions";
import { BusinessUnitDetails } from "../models/BusinessUnitDetails";
import { RequestAliases } from "../../../Base/cypress/constants/RequestAliases";
import { RestAPI } from "../../../Base/cypress/constants/RestAPI";
import { Urls } from "../constants/Urls";
import * as BaseAssertion from "../../../Base/cypress/actions/Assertion";
import { BaseSelectors } from "../../../Base/cypress/selectors/BaseSelectors";
import { GenerateCurrentDatetimeString } from '../../../Base/cypress/actions/GenerateRandoms';

let searchFieldValue = null
let firstBusinessUnit = null

export function FillFirstBusinessUnitDetails(businessUnitDetails: BusinessUnitDetails) {
    let Name = GenerateCurrentDatetimeString("_")
    firstBusinessUnit = Name
    cy.FillLogTextBox(BusinessUnitSelectors.Name, Name)
    cy.FillLogLov(BusinessUnitSelectors.Parent, businessUnitDetails.Parent, true)
}

export function FillSecondBusinessUnitDetails() {
    cy.FillLogTextBox(BusinessUnitSelectors.Name, GenerateCurrentDatetimeString("_"))
    cy.FillLogLov(BusinessUnitSelectors.Parent, firstBusinessUnit, true)
}

export function CreateBusinessUnit() {
    DefinePostBusinessUnitRequest()
    Actions.DefineGetByFilterRequest()
    cy.Click(BaseSelectors.RedButton + BaseSelectors.LastElement, null);
}

function DefinePostBusinessUnitRequest() {
    cy.DefineRequestWait(RestAPI.POST, Urls.BusinessUnits, RequestAliases.PostBusinessUnit);
}

export function AssertCreateBusinessUnit() {
    AssertPostBusinessUnit()
    Actions.AssertGetByFilters()
}

function AssertPostBusinessUnit() {
    let intercept = cy.wait("@" + RequestAliases.PostBusinessUnit);
    intercept.then((interception) => {
        let statusCode = interception.response.statusCode;
        assert.equal(statusCode, 200)
        searchFieldValue = interception.response.body.Name
    })
}

export function SearchBusinessUnit() {
    GeneralActions.Search(searchFieldValue)
}

export function AssertSearchBusinessUnit() {
    GeneralActions.AssertSearch(searchFieldValue);
}

export function OpenBusinessUnit() {
    DefineBusinessUnitGetSingleRequest();
    cy.get(BaseSelectors.RowClass).eq(0).click();
}

function DefineBusinessUnitGetSingleRequest() {
    cy.DefineRequestWait(RestAPI.GET, Urls.BusinessUnitsGetSingle, RequestAliases.GetSignle);
}

export function AssertOpenBusinessUnit() {
    AssertBusinessUnitGetSingle();
    BaseAssertion.AssertElementExist(MaintenanceSelectors.GeneralEditScreen)
}

function AssertBusinessUnitGetSingle() {
    BaseAssertion.AssertStatusCode(RequestAliases.GetSignle, 200);
}

export function EditBusinessUnitGeneralTab(BusinessUnitDetails: BusinessUnitDetails) {
    Actions.FillInputCheckBoxProcess(BusinessUnitSelectors.InActiveCheckBox, BusinessUnitDetails.InactiveCheckBox)
}

export function AssertParentDisabled() {
    BaseAssertion.AssertElementDisabled(BusinessUnitSelectors.Parent, BaseSelectors.BeDisabled)
}

export function UpdateBusinessUnit() {
    DefinePutBusinessUnitRequest()
    cy.Click(BusinessUnitSelectors.SaveButton, null)
}

function DefinePutBusinessUnitRequest() {
    cy.DefineRequestWait(RestAPI.PUT, Urls.BusinessUnits, RequestAliases.PutBusinessUnit);
}

export function AssertUpdateBusinessUnit() {
    BaseAssertion.AssertStatusCode(RequestAliases.PutBusinessUnit, 200);
}

export function CloseSaveBusinessUnit() {
    DefineBusinessUnitViewGetSingleRequest()
    cy.Click(BusinessUnitSelectors.SaveCloseButton, null);
}

function DefineBusinessUnitViewGetSingleRequest() {
    cy.DefineRequestWait(RestAPI.GET, Urls.BusinessUnitsviewGetSingle, RequestAliases.GetSignle);
}

export function AssertCloseSaveBusinessUnit() {
    BaseAssertion.AssertStatusCode(RequestAliases.GetSignle, 200);
}