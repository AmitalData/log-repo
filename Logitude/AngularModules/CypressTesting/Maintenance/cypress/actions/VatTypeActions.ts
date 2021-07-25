import { VatTypesSelectors } from "../selectors/VatTypesSelectors";
import { MaintenanceSelectors } from "../selectors/Selectors";
import * as Actions from "./Actions";
import * as GeneralActions from "./BaseActions";
import { VatTypeDetails } from "../models/VatTypeDetails";
import { RequestAliases } from "../../../Base/cypress/constants/RequestAliases";
import { RestAPI } from "../../../Base/cypress/constants/RestAPI";
import { Urls } from "../constants/Urls";
import * as BaseAssertion from "../../../Base/cypress/actions/Assertion";
import { BaseSelectors } from "../../../Base/cypress/selectors/BaseSelectors";
import { GenerateRandomNumberAndString } from '../../../Base/cypress/actions/GenerateRandoms';

let searchFieldValue = null;

export function EnableMultiPercentage() {
    DefinePUtAccountingSettingsRequest()
    Actions.FillInputCheckBoxProcess(VatTypesSelectors.AccountingSetting_EnableMultiPercentageVATTypes, "No")
    Actions.FillInputCheckBoxProcess(VatTypesSelectors.AccountingSetting_EnableMultiPercentageVATTypes, "Yes")
    cy.Click(BaseSelectors.RedButton + BaseSelectors.LastElement, null);
    cy.Click(BaseSelectors.RedButton + BaseSelectors.LastElement, null);
}

function DefinePUtAccountingSettingsRequest() {
    cy.DefineRequestWait(RestAPI.PUT, Urls.AccountingSettings, RequestAliases.PutAccountingSettings);
}

export function AssertEnableMultiPercentage() {
    let intercept = cy.wait("@" + RequestAliases.PutAccountingSettings);
    intercept.then((interception) => {
        let statusCode = interception.response.statusCode
        assert.equal(statusCode, 200)
    })
}

export function FillVatTypeCode(vatTypeCode: string) {
    cy.FillLogTextBox(VatTypesSelectors.Code, vatTypeCode)
}

export function ClickMultiRadio() {
    cy.get(VatTypesSelectors.MultiPercentageRadio).click()
}

export function FillVatTypeDetails(vatTypeDetails: VatTypeDetails, codeDigits: number) {
    if (vatTypeDetails.IsSinglePercentage) {
        FillSingleVatType(vatTypeDetails)
    }
    else if (vatTypeDetails.IsMultiPercentage) {
        SelectMultiVatTypes()
    }
    cy.FillLogTextBox(VatTypesSelectors.Code, GenerateRandomNumberAndString(codeDigits));
    cy.FillLogTextBox(VatTypesSelectors.Name, vatTypeDetails.Name);
    Actions.FillCheckBoxProcess(VatTypesSelectors.IsRegionalTaxCheckBox + BaseSelectors.LastElement, vatTypeDetails.IsRegionalTax)
    cy.FillLogTextBox(VatTypesSelectors.LocalName, vatTypeDetails.LocalName);
    cy.FillLogTextBox(VatTypesSelectors.Description, vatTypeDetails.Description);
    cy.FillLogTextBox(VatTypesSelectors.LocalDescription, vatTypeDetails.LocalDescription);
}

function FillSingleVatType(vatTypeDetails) {
    let percentageDate = vatTypeDetails.PercentageDate.toLowerCase() == "currentdate" ? "." : vatTypeDetails.PercentageDate
    cy.FillLogTextBox(VatTypesSelectors.Percentage, vatTypeDetails.Percentage);
    cy.FillLogTextBox(VatTypesSelectors.PercentageDate, percentageDate)
}

function SelectMultiVatTypes() {
    cy.get(VatTypesSelectors.MultiSimpleGridViewRow).find(VatTypesSelectors.MultiCheckBoxes).then((checkBox) => {
        cy.wrap(checkBox[0]).check({ force: true })
        cy.wrap(checkBox[1]).check({ force: true })
    })
}

export function CreateVatType() {
    DefinePostVatTypeRequest()
    Actions.DefineGetByFilterRequest()
    cy.Click(BaseSelectors.RedButton + BaseSelectors.LastElement, null);
}

function DefinePostVatTypeRequest() {
    cy.DefineRequestWait(RestAPI.POST, Urls.VatTypes, RequestAliases.PostVatType);
}

export function AssertCreateVatType() {
    AssertPostVatType()
    Actions.AssertGetByFilters()
}

function AssertPostVatType() {
    let intercept = cy.wait("@" + RequestAliases.PostVatType);
    intercept.then((interception) => {
        let statusCode = interception.response.statusCode;
        if (statusCode === 400) {
            ReCreateVatType();
        }
        else {
            assert.equal(statusCode, 200)
            searchFieldValue = interception.response.body.Code;
        }
    })
}

function ReCreateVatType() {
    let RandomCode = Actions.GenerateRandomNumber(4);
    cy.FillLogTextBox(VatTypesSelectors.Code, RandomCode)
    CreateVatType();
    AssertCreateVatType();
}

export function SearchVatType() {
    GeneralActions.Search(searchFieldValue)
}

export function AssertSearchVatType() {
    GeneralActions.AssertSearch(searchFieldValue)
}

export function OpenVatType() {
    DefineVatTypeGetSingleRequest();
    cy.get(BaseSelectors.RowClass).eq(0).click();
}

function DefineVatTypeGetSingleRequest() {
    cy.DefineRequestWait(RestAPI.GET, Urls.VatTypesGetSingle, RequestAliases.GetSignle);
}

export function AssertOpenVatType() {
    AssertVatTypeGetSingle();
    BaseAssertion.AssertElementExist(MaintenanceSelectors.GeneralEditScreen)
}

function AssertVatTypeGetSingle() {
    BaseAssertion.AssertStatusCode(RequestAliases.GetSignle, 200);
}

export function EditVatTypeGeneralTab(vatTypeDetails: VatTypeDetails) {
    Actions.FillCheckBoxProcess(VatTypesSelectors.InActive + BaseSelectors.LastElement, vatTypeDetails.InActive)
    cy.FillLogTextBox(VatTypesSelectors.Description, vatTypeDetails.Description);
    cy.FillLogTextBox(VatTypesSelectors.LocalDescription, vatTypeDetails.LocalDescription);
}

export function NavigateEditPercentage() {
    cy.get(VatTypesSelectors.PercentageEditButton).click({ force: true })
}

export function EditVatTypePercentagesTab(vatTypeDetails: VatTypeDetails) {
    let CurrentDate = "."
    cy.FillLogTextBox(VatTypesSelectors.PercentageTab_FromDate, vatTypeDetails.PercentageDate.toLowerCase() == "currentdate" ? CurrentDate : vatTypeDetails.PercentageDate)
    cy.FillLogTextBox(VatTypesSelectors.PercentageTab_Percentage, vatTypeDetails.Percentage);
    cy.Click(BaseSelectors.RedButton + BaseSelectors.LastElement, null);
}

export function FillVatTypeAccountingTab(vatTypeDetails: VatTypeDetails) {
    cy.FillLogTextBox(VatTypesSelectors.AccountingReceivablesExternalId, vatTypeDetails.AccountingReceivablesExternalID);
    cy.FillLogTextBox(VatTypesSelectors.AccountingPayablesExternalId, vatTypeDetails.AccountingPayablesExternalID);
}

export function UpdateVatType() {
    DefinePutVatTypeRequest()
    cy.Click(VatTypesSelectors.SaveButton, null)
}

function DefinePutVatTypeRequest() {
    cy.DefineRequestWait(RestAPI.PUT, Urls.VatTypes, RequestAliases.PutVatType);
}

export function AssertUpdateVatType() {
    AssertPutVatType()
}

function AssertPutVatType() {
    BaseAssertion.AssertStatusCode(RequestAliases.PutVatType, 200);
}

export function CloseSaveVatType() {
    DefineVatTypeViewGetSingleRequest()
    cy.Click(VatTypesSelectors.SaveCloseButton, null);
}

function DefineVatTypeViewGetSingleRequest() {
    cy.DefineRequestWait(RestAPI.GET, Urls.VatTypesviewGetSingle, RequestAliases.GetSignle);
}

export function AssertCloseSaveVatType() {
    BaseAssertion.AssertStatusCode(RequestAliases.GetSignle, 200);
}