import { OccasionTypeSelectors } from "../selectors/OccasionTypeSelectors";
import { MaintenanceSelectors } from "../selectors/Selectors";
import * as Actions from "./Actions";
import * as GeneralActions from "./BaseActions";
import { RequestAliases } from "../../../Base/cypress/constants/RequestAliases";
import { RestAPI } from "../../../Base/cypress/constants/RestAPI";
import { Urls } from "../constants/Urls";
import * as BaseAssertion from "../../../Base/cypress/actions/Assertion";
import { BaseSelectors } from "../../../Base/cypress/selectors/BaseSelectors";
import * as gr from '../../../Base/cypress/actions/GenerateRandoms';

let searchFieldValue = null

export function FillOccasionTypeDetails() {
    cy.FillLogTextBox(OccasionTypeSelectors.Name, gr.GenerateCurrentDatetimeString("_"))
    cy.FillLogTextBox(OccasionTypeSelectors.Code, gr.GenerateRandomNumberAndString(3))
}

export function CreateOccasionType() {
    DefinePostOccasionTypeRequest()
    Actions.DefineGetByFilterRequest()
    cy.Click(BaseSelectors.RedButton + BaseSelectors.LastElement, null);
}

function DefinePostOccasionTypeRequest() {
    cy.DefineRequestWait(RestAPI.POST, Urls.OccasionTypes, RequestAliases.PostOccasionType);
}

export function AssertCreateOccasionType() {
    AssertPostOccasionType()
    Actions.AssertGetByFilters()
}

function AssertPostOccasionType() {
    let intercept = cy.wait("@" + RequestAliases.PostOccasionType);
    intercept.then((interception) => {
        let statusCode = interception.response.statusCode;
        assert.equal(statusCode, 200)
        searchFieldValue = interception.response.body.Name
    })
}

export function SearchOccasionType() {
    GeneralActions.Search(searchFieldValue)
}

export function AssertSearchOccasionType() {
    GeneralActions.AssertSearch(searchFieldValue);
}

export function OpenOccasionType() {
    DefineOccasionTypeGetSingleRequest();
    cy.get(BaseSelectors.RowClass).eq(0).click();
}

function DefineOccasionTypeGetSingleRequest() {
    cy.DefineRequestWait(RestAPI.GET, Urls.OccasionTypesGetSingle, RequestAliases.GetSignle);
}

export function AssertOpenOccasionType() {
    AssertOccasionTypeGetSingle();
    BaseAssertion.AssertElementExist(MaintenanceSelectors.GeneralEditScreen)
}

function AssertOccasionTypeGetSingle() {
    BaseAssertion.AssertStatusCode(RequestAliases.GetSignle, 200);
}

export function EditOccasionTypeGeneralTab() {
    cy.FillLogTextBox(OccasionTypeSelectors.Name, gr.GenerateCurrentDatetimeString('-'));
}

export function UpdateOccasionType() {
    DefinePutOccasionTypeRequest()
    cy.Click(OccasionTypeSelectors.SaveButton, null)
}

function DefinePutOccasionTypeRequest() {
    cy.DefineRequestWait(RestAPI.PUT, Urls.OccasionTypes, RequestAliases.PutOccasionType);
}

export function AssertUpdateOccasionType() {
    BaseAssertion.AssertStatusCode(RequestAliases.PutOccasionType, 200);
}

export function CloseSaveOccasionType() {
    DefineOccasionTypeViewGetSingleRequest()
    cy.Click(OccasionTypeSelectors.SaveCloseButton, null);
}

function DefineOccasionTypeViewGetSingleRequest() {
    cy.DefineRequestWait(RestAPI.GET, Urls.OccasionTypesviewGetSingle, RequestAliases.GetSignle);
}

export function AssertCloseSaveOccasionType() {
    BaseAssertion.AssertStatusCode(RequestAliases.GetSignle, 200);
}