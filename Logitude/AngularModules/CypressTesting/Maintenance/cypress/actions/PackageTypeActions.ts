import { PackageTypeDetails } from '../models/PackageTypeDetails'
import { PackageTypeSelectors } from "../selectors/PackageTypeSelectors";
import { MaintenanceSelectors } from "../selectors/Selectors";
import { BaseSelectors } from "../../../Base/cypress/selectors/BaseSelectors";
import { Urls } from "../constants/Urls";
import { RestAPI } from "../../../Base/cypress/constants/RestAPI";
import { RequestAliases } from "../../../Base/cypress/constants/RequestAliases";
import * as BaseAssertion from "../../../Base/cypress/actions/Assertion";
import { GenerateRandomNumberAndString } from '../../../Base/cypress/actions/GenerateRandoms';
import * as GeneralActions from './BaseActions'
import * as Actions from './Actions'

let searchFieldValue = null;

export function FillCode(code: string) {
    cy.FillLogTextBox(PackageTypeSelectors.Code, code)
}

export function FillPrintAs(printAs: string) {
    cy.FillLogTextBox(PackageTypeSelectors.PrintAs, printAs)
}

export function FillRequiredData(packageTypeDetails: PackageTypeDetails) {
    cy.FillLogTextBox(PackageTypeSelectors.Code, packageTypeDetails.Code)
    cy.FillLogTextBox(PackageTypeSelectors.Name, packageTypeDetails.Name)
    cy.FillLogTextBox(PackageTypeSelectors.PrintAs, packageTypeDetails.PrintAs)
    Actions.FillCheckBoxProcess(PackageTypeSelectors.OceanCheckBox, packageTypeDetails.Ocean)
}

export function FillPackageTypeDetails(packageTypeDetails: PackageTypeDetails) {
    FillTextBoxes(packageTypeDetails)
    FillCheckBoxes(packageTypeDetails)
}

function FillTextBoxes(packageTypeDetails: PackageTypeDetails) {
    cy.FillLogTextBox(PackageTypeSelectors.Code, GenerateRandomNumberAndString(4))
    cy.FillLogTextBox(PackageTypeSelectors.LocalName, packageTypeDetails.LocalName)
    cy.FillLogTextBox(PackageTypeSelectors.TEU, packageTypeDetails.TEU)
    cy.FillLogTextBox(PackageTypeSelectors.ContainerSize, packageTypeDetails.ContainerSize)
    cy.FillLogTextBox(PackageTypeSelectors.Volume, packageTypeDetails.Volume)
    cy.FillLogTextBox(PackageTypeSelectors.Notes, packageTypeDetails.Notes)
}

function FillCheckBoxes(packageTypeDetails: PackageTypeDetails) {
    Actions.FillCheckBoxProcess(PackageTypeSelectors.IsContainerCheckBox, packageTypeDetails.IsContainer)
    Actions.FillCheckBoxProcess(PackageTypeSelectors.RefrigeratedCheckBox, packageTypeDetails.Refrigerated)
    Actions.FillCheckBoxProcess(PackageTypeSelectors.AirCheckBox, packageTypeDetails.Air)
    Actions.FillCheckBoxProcess(PackageTypeSelectors.InlandCheckBox, packageTypeDetails.Inland)
}

export function CreatePackageType() {
    DefinePostPackageTypeRequest()
    cy.Click(BaseSelectors.RedButton, BaseSelectors.ContainsOK);
}

function DefinePostPackageTypeRequest() {
    cy.DefineRequestWait(RestAPI.POST, Urls.PackageTypes, RequestAliases.PostPackageType)
}

export function AssertCreatePackageType() {
    let intercept = cy.wait("@" + RequestAliases.PostPackageType);
    intercept.then((interception) => {
        let statusCode = interception.response.statusCode;
        if (statusCode === 400) {
            ReCreatePackageType();
        }
        else {
            assert.equal(statusCode, 200)
            searchFieldValue = interception.response.body.Code
        }
    })
}

function ReCreatePackageType() {
    cy.FillLogTextBox(PackageTypeSelectors.Code, GenerateRandomNumberAndString(4))
    CreatePackageType();
    AssertCreatePackageType();
}

export function SearchPackageType() {
    GeneralActions.Search(searchFieldValue)
}

export function AssertSearchPackageType() {
    GeneralActions.AssertSearch(searchFieldValue)
}

export function OpenPackageType() {
    DefinePackageTypesGetSingleRequest();
    cy.get(BaseSelectors.RowClass).eq(0).click();
}

function DefinePackageTypesGetSingleRequest() {
    cy.DefineRequestWait(RestAPI.GET, Urls.PackageTypesGetSingle, RequestAliases.GetSignle);
}

export function AssertOpenPackageType() {
    AssertPackageTypeGetSingle();
    BaseAssertion.AssertElementExist(MaintenanceSelectors.GeneralEditScreen)
}

function AssertPackageTypeGetSingle() {
    BaseAssertion.AssertStatusCode(RequestAliases.GetSignle, 200);
}

export function EditGeneralTab(packageTypeDetails: PackageTypeDetails) {
    cy.FillLogTextBox(PackageTypeSelectors.ContainerSize, packageTypeDetails.ContainerSize)
    cy.FillLogTextBox(PackageTypeSelectors.Volume, packageTypeDetails.Volume)
    cy.FillLogTextBox(PackageTypeSelectors.Notes, packageTypeDetails.Notes)
    Actions.FillCheckBoxProcess(PackageTypeSelectors.InActivePackageTypeCheckBox, packageTypeDetails.InActive)
}

export function EditPackageType() {
    DefinePutPackageTypeRequest();
    cy.Click(PackageTypeSelectors.SaveButton, null);
}

function DefinePutPackageTypeRequest() {
    cy.DefineRequestWait(RestAPI.PUT, Urls.PackageTypes, RequestAliases.PutPackageType);
}

export function AssertEditPackageType() {
    BaseAssertion.AssertStatusCode(RequestAliases.PutPackageType, 200)
}

export function CloseSavePackageType() {
    DefinePackageTypeViewGetSingleRequest()
    cy.Click(PackageTypeSelectors.SaveCloseButton, null);
}

function DefinePackageTypeViewGetSingleRequest() {
    cy.DefineRequestWait(RestAPI.GET, Urls.PackageTypesviewGetSingle, RequestAliases.GetSignle);
}

export function AssertCloseSavePackageType() {
    BaseAssertion.AssertStatusCode(RequestAliases.GetSignle, 200);
}