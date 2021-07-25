import { AirLineSelectors } from "../selectors/AirLineSelectors";
import { MaintenanceSelectors } from "../selectors/Selectors";
import { BaseSelectors } from "../../../Base/cypress/selectors/BaseSelectors";
import { Urls } from "../constants/Urls";
import { RestAPI } from "../../../Base/cypress/constants/RestAPI";
import { RequestAliases } from "../../../Base/cypress/constants/RequestAliases";
import * as BaseAssertion from "../../../Base/cypress/actions/Assertion";
import { AirLineDetails } from 'cypress/models/AirLineDetails';
import * as Actions from './Actions'
import * as GeneralActions from './BaseActions'
import { GenerateRandomNumberAndString } from '../../../Base/cypress/actions/GenerateRandoms';

let searchFieldValue = null

export function FillCode(code: string) {
    cy.FillLogTextBox(AirLineSelectors.Code, code)
}

export function FillICAO(ICAO: string) {
    cy.FillLogTextBox(AirLineSelectors.ICAO, ICAO)
}

export function PressOnWindowHeader() {
    cy.get(".WindowHeader").click()
}

export function FillPrefix(prefix: string) {
    cy.FillLogTextBox(AirLineSelectors.Prefix, prefix)
}

export function FillAirLineDetails(airLineDetails: AirLineDetails) {
    FillCode(GenerateRandomNumberAndString(AirLineSelectors.CodeDigitCount))
    FillICAO(GenerateRandomNumberAndString(AirLineSelectors.ICAODigitCount))
    cy.FillLogTextBox(AirLineSelectors.Name, airLineDetails.Name)
    cy.FillLogTextBox(AirLineSelectors.LocalName, airLineDetails.LocalName)
    cy.FillLogTextBox(AirLineSelectors.Notes, airLineDetails.Notes)
}

export function CreateAirLine() {
    DefinePostAirLineRequest()
    Actions.DefineGetByFilterRequest()
    cy.get("body").then($body => {
        if ($body.find(BaseSelectors.SingleError).length > 0) {
            Recreate()
        }
        else {
            cy.get(BaseSelectors.RedButton).click({ force: true })
        }
    })
}

export function Recreate() {
    FillCode(GenerateRandomNumberAndString(AirLineSelectors.CodeDigitCount))
    FillICAO(GenerateRandomNumberAndString(AirLineSelectors.ICAODigitCount))
    CreateAirLine()
}

export function MockCreateAirLine() {
    DefinePostAirLineRequest()
    cy.intercept(RestAPI.POST, Urls.AirLines, [true])
    CreateAirLine()
}

function DefinePostAirLineRequest() {
    cy.DefineRequestWait(RestAPI.POST, Urls.AirLines, RequestAliases.PostAirLine)
}

export function AssertCreateAirLine() {
    AssertPostAirLine()
    Actions.AssertGetByFilters()
}

export function AssertPostAirLine() {
    let intercept = cy.wait("@" + RequestAliases.PostAirLine);
    intercept.then((interception) => {
        assert.equal(interception.response.statusCode, 200)
        searchFieldValue = interception.response.body.Code
    })
}

export function Search() {
    GeneralActions.Search(searchFieldValue)
}

export function AssertSearch() {
    GeneralActions.AssertSearch(searchFieldValue)
}

export function OpenAirLine() {
    DefineAirLinesGetSingleRequest();
    cy.get(BaseSelectors.RowClass).eq(0).click({ force: true });
}

function DefineAirLinesGetSingleRequest() {
    cy.DefineRequestWait(RestAPI.GET, Urls.AirLinesGetSingle, RequestAliases.GetSignle);
}

export function AssertOpenAirLine() {
    BaseAssertion.AssertStatusCode(RequestAliases.GetSignle, 200);
    BaseAssertion.AssertElementExist(MaintenanceSelectors.GeneralEditScreen)
}


export function FillTariffPartnerCode(partnerCode: string) {
    cy.FillLogTextBox(AirLineSelectors.TariffPartnerCode, partnerCode)
}

export function FillAirLineTariffTranslations(airLineDetails: AirLineDetails) {
    cy.FillLogTextBox(AirLineSelectors.TariffPartnerCode, GenerateRandomNumberAndString(AirLineSelectors.TariffCodeDigitCount))
    cy.FillLogLov(AirLineSelectors.TariffPort, airLineDetails.TariffPort, true)
}

export function CreateAirLineTariffTranslations() {
    cy.DefineRequestWait(RestAPI.POST, Urls.AirLinesTariffTranslations, RequestAliases.PostAirLineTariffTranslations)
    cy.Click(BaseSelectors.RedButton, BaseSelectors.ContainsOK);
}

export function AssertCreateAirLineTariffTranslations() {
    BaseAssertion.AssertStatusCode(RequestAliases.PostAirLineTariffTranslations, 200);
}

export function NavigateSurchargeTariffwizard() {
    cy.DefineRequestWait(RestAPI.GET, Urls.GetTarrifHeaders, RequestAliases.GetTarrifHeaders);
    cy.Navigate(AirLineSelectors.SurchargeTariffTab)
    let intercept = cy.wait("@" + RequestAliases.GetTarrifHeaders);
    intercept.then(() => {
        cy.Navigate(AirLineSelectors.AddSurchargeTariff)
    })
}

export function FillAirLineSurchargeTariffDetails() {
    let currentDate = "."
    cy.FillLogTextBox(AirLineSelectors.SurchargeTarrifFromDate, currentDate)
    cy.FillLogTextBox(AirLineSelectors.SurchargeTarrifToDate, currentDate)
}

export function FillAirLineTariffCharge(airLineDetails: AirLineDetails) {
    cy.FillLogLov(AirLineSelectors.TarrifCharge_ChargesTypeId, airLineDetails.TariffChargeType, true)
    cy.FillLogTextBox(AirLineSelectors.TarrifCharge_UnitPrice, airLineDetails.TariffChargeUnitPrice)
}

export function CreateAirLineSurchargeTariff() {
    cy.DefineRequestWait(RestAPI.POST, Urls.AirLineTariffHeader, RequestAliases.PostSurchargeTariff)
    cy.Click(BaseSelectors.RedButton + BaseSelectors.LastElement, BaseSelectors.ContainsOK);
    cy.Click(BaseSelectors.RedButton + BaseSelectors.LastElement, BaseSelectors.ContainsOK);
}

export function AssertCreateAirLineSurchargeTariff() {
    BaseAssertion.AssertStatusCode(RequestAliases.PostSurchargeTariff, 200);
}

export function FillSpecialHandlingCode(code: string) {
    cy.FillLogTextBox(AirLineSelectors.AWBSpecialHandlingCode_Code, code)
}

export function FillSpecialHandlingCodesDetails(airLineDetails: AirLineDetails) {
    cy.FillLogTextBox(AirLineSelectors.AWBSpecialHandlingCode_Code, airLineDetails.AdaptionSpecialHandlingCode)
    cy.FillLogTextBox(AirLineSelectors.AWBSpecialHandlingCode_Name, airLineDetails.AdaptionSpecialHandlingName)
}

export function CreateSpecialHandlingCodes() {
    cy.DefineRequestWait(RestAPI.POST, Urls.AirLineSpecialHandlingCodes, RequestAliases.PostSpecialHandlingCodes)
    cy.Click(BaseSelectors.RedButton + BaseSelectors.LastElement, BaseSelectors.ContainsOK);
}

export function AssertCreateSpecialHandlingCodes() {
    BaseAssertion.AssertStatusCode(RequestAliases.PostSpecialHandlingCodes, 200);
}

export function EditAirLine() {
    DefinePutAirLineRequest();
    cy.Click(AirLineSelectors.SaveButton, null);
}

function DefinePutAirLineRequest() {
    cy.DefineRequestWait(RestAPI.PUT, Urls.AirLines, RequestAliases.PutAirLinee);
}

export function AssertEditAirLine() {
    BaseAssertion.AssertStatusCode(RequestAliases.PutAirLinee, 200)
}

export function CloseSaveAirLine() {
    DefineAirLineViewGetSingleRequest()
    cy.Click(AirLineSelectors.SaveCloseButton, null);
}

function DefineAirLineViewGetSingleRequest() {
    cy.DefineRequestWait(RestAPI.GET, Urls.AirLinesviewGetSingle, RequestAliases.GetSignle);
}

export function AssertCloseSaveAirLine() {
    BaseAssertion.AssertStatusCode(RequestAliases.GetSignle, 200);
}