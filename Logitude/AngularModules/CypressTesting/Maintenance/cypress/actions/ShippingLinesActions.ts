import { ShippingLineSelectors } from "../selectors/ShippingLineSelectors";
import { MaintenanceSelectors } from "../selectors/Selectors";
import { BaseSelectors } from "../../../Base/cypress/selectors/BaseSelectors";
import { Urls } from "../constants/Urls";
import { RestAPI } from "../../../Base/cypress/constants/RestAPI";
import { RequestAliases } from "../../../Base/cypress/constants/RequestAliases";
import * as BaseAssertion from "../../../Base/cypress/actions/Assertion";
import { ShippingLineDetails } from 'cypress/models/ShippingLineDetails';
import * as GeneralActions from './GeneralActions'
import * as gr from '../../../Base/cypress/actions/GenerateRandoms';

let shippingLineCode = null;

export function FillShippingLineDetails(shippingLineDetails: ShippingLineDetails) {
    cy.DefineRequestWait(RestAPI.GET, Urls.NewShippingLines, RequestAliases.NewShippingLine)
    cy.Click('button', 'Add').then(() => {
        cy.wait('@' + RequestAliases.NewShippingLine).then(() => {
            cy.Click('.Button', 'New Shipping Line').then(() => {
                let RandomCodeNumber = gr.GenerateRandomNumberAndString(ShippingLineSelectors.CodeDigitCount)
                let RandomSCACCode = gr.GenerateRandomNumberAndString(ShippingLineSelectors.CodeDigitCount)

                cy.FillLogTextBox(ShippingLineSelectors.ShippingLineCode, RandomCodeNumber)
                cy.FillLogTextBox(ShippingLineSelectors.ShippingLineSCACCode, RandomSCACCode)
                cy.FillLogTextBox(ShippingLineSelectors.ShippingLineName, shippingLineDetails.Name)
                cy.FillLogTextBox(ShippingLineSelectors.ShippingLineNotes, shippingLineDetails.Notes)
            })
        })
    })
}

export function CreateShippingLine() {
    DefinePostShippingLineRequest()
    cy.Click(BaseSelectors.RedButton, BaseSelectors.ContainsOK);
}

function DefinePostShippingLineRequest() {
    cy.DefineRequestWait(RestAPI.POST, Urls.ShippingLines, RequestAliases.PostShippingLine)
}

export function AssertCreateShippingLine() {
    let intercept = cy.wait("@" + RequestAliases.PostShippingLine);
    intercept.then((interception) => {
        let status = interception.response.statusCode
        assert.equal(status, 200)
        shippingLineCode = interception.response.body.Code
    })
}

export function SearchShippingLine() {
    GeneralActions.Search(shippingLineCode)
}

export function AssertSearchShippingLine() {
    GeneralActions.AssertSearch(shippingLineCode)
}

export function OpenShippingLine() {
    DefineShippingLinesGetSingleRequest();
    cy.get(BaseSelectors.RowClass).eq(0).click();
}

function DefineShippingLinesGetSingleRequest() {
    cy.DefineRequestWait(RestAPI.GET, Urls.ShippingLinesGetSingle, RequestAliases.GetSignle);
}

export function AssertOpenShippingLine() {
    AssertShippingLineGetSingle();
    BaseAssertion.AssertElementExist(MaintenanceSelectors.GeneralEditScreen)
}

function AssertShippingLineGetSingle() {
    BaseAssertion.AssertStatusCode(RequestAliases.GetSignle, 200);
}

export function CheckShippingLineINTTRA() {
    cy.get(ShippingLineSelectors.ShippingLine_IsINTTRA).should('be.disabled')
    cy.get(ShippingLineSelectors.ShippingLine_INTTRANotes).should('have.class', 'TextAreaDisabled')
}

export function FillShippingLineAddresses(shippingLineDetails: ShippingLineDetails) {
    cy.DefineRequestWait(RestAPI.GET, Urls.NewShippingLinesAddress, RequestAliases.NewShippingLineAddress);
    cy.get("#Edit").click({ force: true })
    cy.FillLogLov('#Address_CountryId', shippingLineDetails.AddressCountry, true)
    cy.FillLogTextBox('#Address_City', shippingLineDetails.AddressCity)
    cy.FillLogLov('#Address_StateId', "Arkansas", true)
}

export function CreateShippingLineAddress() {
    cy.DefineRequestWait(RestAPI.POST, Urls.ShippingLinesAddress, RequestAliases.PostShippingLineAddress)
    cy.Click(BaseSelectors.RedButton, BaseSelectors.ContainsOK);
}

export function AssertCreateShippingLineAddress() {
    let intercept = cy.wait("@" + RequestAliases.PostShippingLineAddress);
    intercept.then((interception) => {
        let status = interception.response.statusCode
        assert.equal(status, 200)
    })
}

export function AssertPostShippingLineAddress(responseStatusCode: number, expectedStatusCode: number, shippingLineAddressName: string) {
    assert.equal(responseStatusCode, expectedStatusCode)
    assert.equal(shippingLineAddressName, shippingLineAddressName)
}

export function FillShippingLineAreas(shippingLineDetails: ShippingLineDetails) {
    cy.get('#addArea').click().then(() => {
        cy.FillLogTextBox('#CarrierArea_Name', shippingLineDetails.AreaName)
        cy.FillLogTextBox('#CarrierArea_Description', shippingLineDetails.AreaDescription)
        cy.Click('button', 'Country Ports').then(() => {
            cy.FillLogLov('#CarrierAreasPort_CountryId', shippingLineDetails.AreaCountry, true)
            cy.get("#LogitudeWindow_0_4").find(".Button").contains("Add").click()
            cy.get("#LogitudeWindow_0_4").find(".Button").contains("Close").click()
        })
        cy.Click('button', 'Ports').then(() => {
            cy.FillLogLov('#CarrierAreasPort_PortId', shippingLineDetails.AreaPort, true)
            cy.get("#LogitudeWindow_0_4").find(".Button").contains("Add").click()
            cy.get("#LogitudeWindow_0_4").find(".Button").contains("Close").click()
        })
    })
}

export function CreateShippingLineArea() {
    cy.DefineRequestWait(RestAPI.POST, Urls.ShippingLinesArea, RequestAliases.PostShippingLineArea)
    cy.Click(BaseSelectors.RedButton, BaseSelectors.ContainsOK);
}

export function AssertCreateShippingLineArea() {
    let intercept = cy.wait("@" + RequestAliases.PostShippingLineArea);
    intercept.then((interception) => {
        AssertPostShippingLineArea(interception.response.statusCode, 200, interception.response.body.Name)
    })
}

export function AssertPostShippingLineArea(responseStatusCode: number, expectedStatusCode: number, shippingLineAreaName: string) {
    assert.equal(responseStatusCode, expectedStatusCode)
}

export function FillShippingLineTariffTranslations(shippingLineDetails: ShippingLineDetails) {
    let RandomNumber = gr.GenerateRandomNumberAndString(ShippingLineSelectors.TariffCodeDigitCount)
    cy.FillLogTextBox('#TariffCarrierTranslation_PartnerCode', RandomNumber)
    cy.FillLogLov('#TariffCarrierTranslation_PortId', shippingLineDetails.TariffPort, true)
}

export function CreateShippingLineTariffTranslations() {
    cy.DefineRequestWait(RestAPI.POST, Urls.ShippingLinesTariffTranslations, RequestAliases.PostShippingLineTariffTranslations)
    cy.Click(BaseSelectors.RedButton, BaseSelectors.ContainsOK);
}

export function AssertCreateShippingLineTariffTranslations() {
    let intercept = cy.wait("@" + RequestAliases.PostShippingLineTariffTranslations);
    intercept.then((interception) => {
        AssertPostShippingLineTariffTranslations(interception.response.statusCode, 200, interception.response.body.Name)
    })
}

export function AssertPostShippingLineTariffTranslations(responseStatusCode: number, expectedStatusCode: number, shippingLineAreaName: string) {
    assert.equal(responseStatusCode, expectedStatusCode)
}

export function EditShippingLine() {
    DefinePutShippingLineRequest();
    cy.Click(ShippingLineSelectors.ShippingLineSaveButton, null);
}

function DefinePutShippingLineRequest() {
    cy.DefineRequestWait(RestAPI.PUT, Urls.ShippingLines, RequestAliases.PutShippingLine);
}

export function AssertEditShippingLine() {
    BaseAssertion.AssertStatusCode(RequestAliases.PutShippingLine, 200)
}

export function CloseSaveShippingLine() {
    DefineShippingLineViewGetSingleRequest()
    cy.Click(ShippingLineSelectors.ShippingLineSaveCloseButton, null);
}

export function AssertCloseSaveShippingLine() {
    AssertShippingLineGetSingle();
}

function DefineShippingLineViewGetSingleRequest() {
    cy.DefineRequestWait(RestAPI.GET, Urls.ShippingLinesviewGetSingle, RequestAliases.GetSignle);
}