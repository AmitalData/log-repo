import { DepartmentSelectors } from "../selectors/DepartmentSelectors";
import { MaintenanceSelectors } from "../selectors/Selectors";
import * as Actions from "./Actions";
import * as GeneralActions from "./BaseActions";
import { DepartmentDetails } from "../models/DepartmentDetails";
import { RequestAliases } from "../../../Base/cypress/constants/RequestAliases";
import { RestAPI } from "../../../Base/cypress/constants/RestAPI";
import { Urls } from "../constants/Urls";
import * as BaseAssertion from "../../../Base/cypress/actions/Assertion";
import { BaseSelectors } from "../../../Base/cypress/selectors/BaseSelectors";
import * as gr from '../../../Base/cypress/actions/GenerateRandoms';

let searchFieldValue = null

export function FillCode(code: string) {
    cy.FillLogTextBox(DepartmentSelectors.Code, code)
}

export function FillDepartmentDetails(departmentDetails: DepartmentDetails, codeDigits: number) {
    cy.FillLogTextBox(DepartmentSelectors.Name, gr.GenerateCurrentDatetimeString("_"))
    cy.FillLogTextBox(DepartmentSelectors.LocalName, departmentDetails.LocalName);
    cy.FillLogTextBox(DepartmentSelectors.Code, gr.GenerateRandomNumberAndString(codeDigits));
    cy.FillLogTextBox(DepartmentSelectors.Notes, departmentDetails.Notes);
}

export function CreateDepartment() {
    DefinePostDepartmentRequest()
    Actions.DefineGetByFilterRequest()
    cy.Click(BaseSelectors.RedButton + BaseSelectors.LastElement, null);
}

function DefinePostDepartmentRequest() {
    cy.DefineRequestWait(RestAPI.POST, Urls.Departments, RequestAliases.PostDepartment);
}

export function AssertCreateDepartment() {
    AssertPostDepartment()
    Actions.AssertGetByFilters()
}

function AssertPostDepartment() {
    let intercept = cy.wait("@" + RequestAliases.PostDepartment);
    intercept.then((interception) => {
        let statusCode = interception.response.statusCode;
        assert.equal(statusCode, 200)
        searchFieldValue = interception.response.body.EnglishName
    })
}

export function SearchDepartment() {
    GeneralActions.Search(searchFieldValue)
}

export function AssertSearchDepartment() {
    GeneralActions.AssertSearch(searchFieldValue);
}

export function OpenDepartment() {
    DefineDepartmentGetSingleRequest();
    cy.get(BaseSelectors.RowClass).eq(0).click();
}

function DefineDepartmentGetSingleRequest() {
    cy.DefineRequestWait(RestAPI.GET, Urls.DepartmentsGetSingle, RequestAliases.GetSignle);
}

export function AssertOpenDepartment() {
    AssertDepartmentGetSingle();
    BaseAssertion.AssertElementExist(MaintenanceSelectors.GeneralEditScreen)
}

function AssertDepartmentGetSingle() {
    BaseAssertion.AssertStatusCode(RequestAliases.GetSignle, 200);
}

export function EditDepartmentGeneralTab(departmentDetails: DepartmentDetails) {
    cy.FillLogTextBox(DepartmentSelectors.LocalName, departmentDetails.LocalName);
    cy.FillLogTextBox(DepartmentSelectors.Notes, departmentDetails.Notes);
    Actions.FillCheckBoxProcess(DepartmentSelectors.InactiveCheckBox, departmentDetails.InActiveCheckBox)
}

export function UpdateDepartment() {
    DefinePutDepartmentRequest()
    cy.Click(DepartmentSelectors.SaveButton, null)
}

function DefinePutDepartmentRequest() {
    cy.DefineRequestWait(RestAPI.PUT, Urls.Departments, RequestAliases.PutDepartment);
}

export function AssertUpdateDepartment() {
    BaseAssertion.AssertStatusCode(RequestAliases.PutDepartment, 200);
}

export function CloseSaveDepartment() {
    DefineDepartmentViewGetSingleRequest()
    cy.Click(DepartmentSelectors.SaveCloseButton, null);
}

function DefineDepartmentViewGetSingleRequest() {
    cy.DefineRequestWait(RestAPI.GET, Urls.DepartmentsviewGetSingle, RequestAliases.GetSignle);
}

export function AssertCloseSaveDepartment() {
    BaseAssertion.AssertStatusCode(RequestAliases.GetSignle, 200);
}