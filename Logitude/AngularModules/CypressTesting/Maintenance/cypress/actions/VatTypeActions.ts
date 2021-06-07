import { VatTypesSelectors } from "../selectors/VatTypesSelectors";
import { MaintenanceSelectors } from "../selectors/Selectors";
import * as Actions from "./Actions";
import { VatTypeDetails } from "../models/VatTypeDetails";
import { RequestAliases } from "../../../Base/cypress/constants/RequestAliases";
import { RestAPI } from "../../../Base/cypress/constants/RestAPI";
import { Urls } from "../constants/Urls";
import * as BaseAssertion from "../../../Base/cypress/actions/Assertion";
import { BaseSelectors } from "../../../Base/cypress/selectors/BaseSelectors";

let VatTypeCode = null;

export function FillVatTypeDetails(vatTypeDetails: VatTypeDetails, codeDigits: number) {
    if (vatTypeDetails.IsSinglePercentage) {
        cy.ClickRadio(VatTypesSelectors.SinglePercentageRadio)
        FillSingleVatType(vatTypeDetails)
    }
    else if (vatTypeDetails.IsMultiPercentage) {
        cy.ClickRadio(VatTypesSelectors.MultiPercentageRadio)
        SelectMultiVatTypes()
    }
    var RandomCode = Actions.GenerateRandomNumber(codeDigits);
    cy.FillLogTextBox(VatTypesSelectors.Code, vatTypeDetails.Code.toLowerCase() == "random" ? RandomCode : vatTypeDetails.Code)
    cy.FillLogTextBox(VatTypesSelectors.Name, vatTypeDetails.Name);
    Actions.FillCheckBoxProcess(VatTypesSelectors.IsRegionalTaxCheckBox + BaseSelectors.LastElement, vatTypeDetails.IsRegionalTax)
    cy.FillLogTextBox(VatTypesSelectors.LocalName, vatTypeDetails.LocalName);
    cy.FillLogTextBox(VatTypesSelectors.Description, vatTypeDetails.Description);
    cy.FillLogTextBox(VatTypesSelectors.LocalDescription, vatTypeDetails.LocalDescription);
}

function FillSingleVatType(vatTypeDetails) {
    let CurrentDate = "."
    cy.FillLogTextBox(VatTypesSelectors.Percentage, vatTypeDetails.Percentage);
    cy.FillLogTextBox(VatTypesSelectors.PercentageDate, vatTypeDetails.PercentageDate.toLowerCase() == "currentdate" ? CurrentDate : vatTypeDetails.PercentageDate)
}

function SelectMultiVatTypes() {
    cy.get(VatTypesSelectors.MultiSimpleGridViewRow).find(VatTypesSelectors.MultiCheckBoxes).then((checkBox)=> {
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
    BaseAssertion.AssertStatusCode(RequestAliases.PostVatType, 200).then((interception) => {
        VatTypeCode = interception.response.body.Code;
    });
}

export function SearchVatType(vatTypeCode) {
    DefineVatTypeViewsGetByFiltersRequest(vatTypeCode);
    cy.FillLogTextBox(BaseSelectors.SearchTextboxInput, vatTypeCode);
    AssertVatTypeViewsGetByFilters();
}

function DefineVatTypeViewsGetByFiltersRequest(VatTypeCode: string) {
    cy.DefineRequestWait(RestAPI.GET, Urls.GetFilterSearch(VatTypeCode), RequestAliases.GetFilterSearch);
}

function AssertVatTypeViewsGetByFilters() {
    BaseAssertion.AssertStatusCode(RequestAliases.GetFilterSearch, 200);
}

export function getVatTypeCode() {
    return VatTypeCode
}

export function AssertSearchVatType(vatTypeCode) {
    cy.get(BaseSelectors.ListDataLoaded)
    cy.get(BaseSelectors.RowClass).eq(0).invoke(BaseSelectors.TextElement).then((text) => {
        expect(text).to.contain(vatTypeCode);
    });
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