import {PackageTypeDetails} from '../models/PackageTypeDetails'
import { PackageTypeSelectors } from "../selectors/PackageTypeSelectors";
import { MaintenanceSelectors } from "../selectors/Selectors";
import { BaseSelectors } from "../../../Base/cypress/selectors/BaseSelectors";
import { Urls } from "../constants/Urls";
import { RestAPI } from "../../../Base/cypress/constants/RestAPI";
import { RequestAliases } from "../../../Base/cypress/constants/RequestAliases";
import * as BaseAssertion from "../../../Base/cypress/actions/Assertion";
import { constants } from "../../../Base/cypress/constants/constants"

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

//#region package Type

export function FillPackageTypeDetails(packageTypeDetails: PackageTypeDetails) {
    let MinRandomNumber = 1;
    let MaxRandomNumber = 1000;
    cy.FillRandomNumber(PackageTypeSelectors.PackageTypeCode, MinRandomNumber, MaxRandomNumber)
    cy.FillLogTextBox(PackageTypeSelectors.PackageTypeName, packageTypeDetails.Name)
    cy.FillLogTextBox(PackageTypeSelectors.PackageTypeLocalName, packageTypeDetails.LocalName)
    cy.FillRandomNumber(PackageTypeSelectors.PackageTypeTEU, MinRandomNumber, MaxRandomNumber)

    cy.FillRandomNumber(PackageTypeSelectors.PackageTypeContainerSize, MinRandomNumber, MaxRandomNumber )
    cy.FillRandomNumber(PackageTypeSelectors.PackageTypeVolume, MinRandomNumber, MaxRandomNumber )
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
    let MinRandomNumber = 1;
    let MaxRandomNumber = 8;
    cy.FillLogTextBox(PackageTypeSelectors.PackageTypeCode, '1234')
    cy.FillLogTextBox(PackageTypeSelectors.PackageTypeName, 'TEST')
    cy.FillLogTextBox(PackageTypeSelectors.PackageTypeLocalName, 'LocalTEST')
    cy.FillLogTextBox(PackageTypeSelectors.PackageTypeTEU, '12345')

    cy.FillLogTextBox(PackageTypeSelectors.PackageTypeContainerSize, '2321')
    cy.FillLogTextBox(PackageTypeSelectors.PackageTypeVolume, '5432')
    cy.FillLogTextBox(PackageTypeSelectors.PackageTypePrintAs, 'TESTPrint')
    FillCheckBoxProcess(PackageTypeSelectors.PackageTypeAirCheckBox, 'Yes')
    FillCheckBoxProcess(PackageTypeSelectors.InActivePackageTypeCheckBox, 'Yes')
    CreatePackageType();
    AssertCreatePackageType();
}

export function AssertPostPackageType(responseStatusCode: number, expectedStatusCode: number, packageTypeCode: string) {
    assert.equal(responseStatusCode, expectedStatusCode)
    PackageTypeCode = packageTypeCode
}

export function SearchPackageType() {
    DefinePackageTypeViewsGetByFiltersRequest(PackageTypeCode);
    cy.FillLogTextBox(BaseSelectors.SearchTextboxInput, PackageTypeCode);
    AssertPackageTypeViewsGetByFilters();
}

export function DefinePackageTypeViewsGetByFiltersRequest(PackageTypeName: string) {
    cy.DefineRequestWait(RestAPI.GET, Urls.GetFilterSearch(PackageTypeName + "&GetCount=false"), RequestAliases.GetFilterSearch);
}
export function AssertPackageTypeViewsGetByFilters() {
    BaseAssertion.AssertStatusCode(RequestAliases.GetFilterSearch, 200);
}

export function AssertSearchPackageType() {
    cy.get(BaseSelectors.RowClass).eq(0).invoke(BaseSelectors.TextElement).then((text) => {
        expect(text).to.contain(PackageTypeCode);
    });
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
    AssertPutPackageType();
}

export function AssertPutPackageType() {
    BaseAssertion.AssertStatusCode(RequestAliases.PutPackageType, 200).
        then((interception) => {
            inActivePackageType = interception.response.body.InActive;
        });
}

export function CloseSavePackageType() {
    DefinePackageTypeViewGetSingleRequest()
    cy.Click(PackageTypeSelectors.PackageTypeSaveCloseButton, null);
}

function DefinePackageTypeViewGetSingleRequest() {
    cy.DefineRequestWait(RestAPI.GET, Urls.PackageTypesGetSingle, RequestAliases.GetSignle);
}

//#endregion