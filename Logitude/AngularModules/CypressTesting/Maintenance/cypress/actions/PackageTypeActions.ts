import { PackageTypeDetails } from '../models/PackageTypeDetails'
import { PackageTypeSelectors } from "../selectors/PackageTypeSelectors";
import { MaintenanceSelectors } from "../selectors/Selectors";
import { BaseSelectors } from "../../../Base/cypress/selectors/BaseSelectors";
import { Urls } from "../constants/Urls";
import { RestAPI } from "../../../Base/cypress/constants/RestAPI";
import { RequestAliases } from "../../../Base/cypress/constants/RequestAliases";
import * as BaseAssertion from "../../../Base/cypress/actions/Assertion";
import { constants } from "../../../Base/cypress/constants/constants"
import * as gr from '../../../Base/cypress/actions/GenerateRandoms';
import * as GeneralActions from './GeneralActions'
let PackageTypeCode = null;
let inActivePackageType = false;

export function FillCheckBoxProcess(CheckBoxSelector: string, IsCheck: string) {
    if (IsCheck) {
        if (IsCheck.toUpperCase() == constants.YES) {
            cy.get(CheckBoxSelector).check({ force: true })
        }
        else {
            cy.get(CheckBoxSelector).find(BaseSelectors.input).uncheck({ force: true })
        }
    }
}

function GenerateRandomNumber(NumberLength: number) {
    let NewRandomCode = gr.GenerateRandomNumberAndString(NumberLength)
    return NewRandomCode;
}

export function FillPackageTypeDetails(packageTypeDetails: PackageTypeDetails) {

    let RandomNumberCode = GenerateRandomNumber(PackageTypeSelectors.CodeDigitCount)

    cy.FillLogTextBox(PackageTypeSelectors.PackageTypeCode, RandomNumberCode)
    cy.FillLogTextBox(PackageTypeSelectors.PackageTypeName, packageTypeDetails.Name)
    cy.FillLogTextBox(PackageTypeSelectors.PackageTypeLocalName, packageTypeDetails.LocalName)
    cy.FillLogTextBox(PackageTypeSelectors.PackageTypeTEU, packageTypeDetails.TEU)
    cy.FillLogTextBox(PackageTypeSelectors.PackageTypeContainerSize, packageTypeDetails.ContainerSize)
    cy.FillLogTextBox(PackageTypeSelectors.PackageTypeVolume, packageTypeDetails.Volume)
    cy.FillLogTextBox(PackageTypeSelectors.PackageTypePrintAs, packageTypeDetails.PrintAs)
    FillCheckBoxProcess(PackageTypeSelectors.PackageTypeAirCheckBox, packageTypeDetails.Air)
    FillCheckBoxProcess(PackageTypeSelectors.InActivePackageTypeCheckBox, packageTypeDetails.InActive)
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
        if (interception.response.statusCode === 400) {
            ReCreatePackageType();
        }
        else {
            AssertPostPackageType(interception.response.statusCode, 200, interception.response.body.Code)
        }
    })
}

function ReCreatePackageType() {

    let RandomNumberCode = GenerateRandomNumber(PackageTypeSelectors.CodeDigitCount)

    cy.FillLogTextBox(PackageTypeSelectors.PackageTypeCode, RandomNumberCode)
    CreatePackageType();
    AssertCreatePackageType();
}

export function AssertPostPackageType(responseStatusCode: number, expectedStatusCode: number, packageTypeCode: string) {
    assert.equal(responseStatusCode, expectedStatusCode)
    PackageTypeCode = packageTypeCode
}

export function SearchPackageType() {
    GeneralActions.Search(PackageTypeCode)
}

export function AssertSearchPackageType() {
    GeneralActions.AssertSearch(PackageTypeCode)
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

export function FillPackageTypeLocalName(LocalName: string) {
    cy.FillLogTextBox(PackageTypeSelectors.PackageTypeLocalName, LocalName)
}

export function EditPackageType() {
    DefinePutPackageTypeRequest();
    cy.Click(PackageTypeSelectors.PackageTypeSaveButton, null);
}

function DefinePutPackageTypeRequest() {
    cy.DefineRequestWait(RestAPI.PUT, Urls.PackageTypes, RequestAliases.PutPackageType);
}

export function AssertEditPackageType() {
    BaseAssertion.AssertStatusCode(RequestAliases.PutPackageType, 200)
}

export function CloseSavePackageType() {
    DefinePackageTypeViewGetSingleRequest()
    cy.Click(PackageTypeSelectors.PackageTypeSaveCloseButton, null);
}

export function AssertCloseSavePackageType() {
    AssertPackageTypeGetSingle();
}

function DefinePackageTypeViewGetSingleRequest() {
    cy.DefineRequestWait(RestAPI.GET, Urls.PackageTypesviewGetSingle, RequestAliases.GetSignle);
}