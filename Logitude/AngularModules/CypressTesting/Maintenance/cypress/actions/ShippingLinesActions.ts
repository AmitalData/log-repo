import { ShippingLineSelectors } from "../selectors/ShippingLineSelectors";
import { MaintenanceSelectors } from "../selectors/Selectors";
import { BaseSelectors } from "../../../Base/cypress/selectors/BaseSelectors";
import { Urls } from "../constants/Urls";
import { RestAPI } from "../../../Base/cypress/constants/RestAPI";
import { RequestAliases } from "../../../Base/cypress/constants/RequestAliases";
import * as BaseAssertion from "../../../Base/cypress/actions/Assertion";
import { ShippingLineDetails } from 'cypress/models/ShippingLineDetails';
import * as GeneralActions from './BaseActions'
import * as Actions from './Actions'
import { GenerateRandomNumberAndString } from '../../../Base/cypress/actions/GenerateRandoms';

let searchValueField = null;

export function FillCode(code: string) {
    cy.FillLogTextBox(ShippingLineSelectors.Code, code)
}

export function FillSCACCode(SCACCode: string) {
    cy.FillLogTextBox(ShippingLineSelectors.SCACCode, SCACCode)
}

export function FillShippingLineDetails(shippingLineDetails: ShippingLineDetails) {
    cy.FillLogTextBox(ShippingLineSelectors.Code, GenerateRandomNumberAndString(ShippingLineSelectors.CodeDigitCount))
    cy.FillLogTextBox(ShippingLineSelectors.SCACCode, GenerateRandomNumberAndString(ShippingLineSelectors.CodeDigitCount))
    cy.FillLogTextBox(ShippingLineSelectors.Name, shippingLineDetails.Name)
    cy.FillLogTextBox(ShippingLineSelectors.Notes, shippingLineDetails.Notes)
}

export function CreateShippingLine() {
    DefinePostShippingLineRequest()
    Actions.DefineGetByFilterRequest()
    cy.get(BaseSelectors.RedButton).then(($btn) => {
        if ($btn.is(":disabled")) {
            ReCreateShippingLine()
        } else {
            cy.wrap($btn).click()
        }
    })
}

function DefinePostShippingLineRequest() {
    cy.DefineRequestWait(RestAPI.POST, Urls.ShippingLines, RequestAliases.PostShippingLine)
}

export function AssertCreateShippingLine() {
    AssertPostShippingLine()
    Actions.AssertGetByFilters()
}

export function AssertPostShippingLine() {
    let intercept = cy.wait("@" + RequestAliases.PostShippingLine);
    intercept.then((interception) => {
        assert.equal(interception.response.statusCode, 200)
        searchValueField = interception.response.body.Code
    })
}

function ReCreateShippingLine() {
    cy.FillLogTextBox(ShippingLineSelectors.Code, GenerateRandomNumberAndString(ShippingLineSelectors.CodeDigitCount))
    cy.FillLogTextBox(ShippingLineSelectors.Notes, "ReCreate Shipping Line")
    CreateShippingLine();
}

export function SearchShippingLine() {
    GeneralActions.Search(searchValueField)
}

export function AssertSearchShippingLine() {
    GeneralActions.AssertSearch(searchValueField)
}

export function OpenShippingLine() {
    DefineShippingLinesGetSingleRequest();
    cy.get(BaseSelectors.RowClass).eq(0).click();
}

function DefineShippingLinesGetSingleRequest() {
    cy.DefineRequestWait(RestAPI.GET, Urls.ShippingLinesGetSingle, RequestAliases.GetSignle);
}

export function AssertOpenShippingLine() {
    BaseAssertion.AssertStatusCode(RequestAliases.GetSignle, 200);
    BaseAssertion.AssertElementExist(MaintenanceSelectors.GeneralEditScreen)
}

export function CheckShippingLineINTTRA() {
    BaseAssertion.AssertElementDisabled(ShippingLineSelectors.IsINTTRACheckBox, BaseSelectors.BeDisabled)
    BaseAssertion.AssertElementHaveClass(ShippingLineSelectors.INTTRARegistrationNotes, 'TextAreaDisabled')
}

export function FillShippingLineAddresses(shippingLineDetails: ShippingLineDetails) {
    cy.FillLogLov(ShippingLineSelectors.Address_CountryId, shippingLineDetails.AddressCountry, true)
    cy.FillLogTextBox(ShippingLineSelectors.Address_City, shippingLineDetails.AddressCity)
    cy.FillLogLov(ShippingLineSelectors.Address_StateId, shippingLineDetails.AddressState, true)
}

export function CreateShippingLineAddress() {
    cy.DefineRequestWait(RestAPI.POST, Urls.AirShippingLinesAddress, RequestAliases.PostShippingLineAddress)
    cy.Click(BaseSelectors.RedButton, BaseSelectors.ContainsOK);
}

export function AssertCreateShippingLineAddress() {
    let intercept = cy.wait("@" + RequestAliases.PostShippingLineAddress);
    intercept.then((interception) => {
        assert.equal(interception.response.statusCode, 200)
    })
}

export function FillAreaCountryPortName(areaCountryPortName) {
    cy.FillLogLov(ShippingLineSelectors.CarrierAreasPort_CountryId, areaCountryPortName, true)
}

export function AddAreaCountryPort() {
    cy.DefineRequestWait(RestAPI.GET, Urls.CountryPortviews, RequestAliases.PostShippingLineAreaCountryPort)
    cy.get(ShippingLineSelectors.AddCountry_CarrierAreasPort).click()
    cy.get(ShippingLineSelectors.CloseCountry_CarrierAreasPort).click()
}

export function AssertAddAreaCountryPort() {
    let intercept = cy.wait("@" + RequestAliases.PostShippingLineAreaCountryPort);
    intercept.then((interception) => {
        assert.equal(interception.response.statusCode, 200)
    })
}

export function FillAreaPortName(areaPortName) {
    cy.FillLogLov(ShippingLineSelectors.CarrierAreasPort_PortId, areaPortName, true)
}

export function AddAreaPort() {
    cy.DefineRequestWait(RestAPI.POST, Urls.PortPostLogsList, RequestAliases.PostShippingLineAreaPort)
    cy.get(ShippingLineSelectors.Add_CarrierAreasPort).click()
    cy.get(ShippingLineSelectors.Close_CarrierAreasPort).click()
}

export function AssertAddAreaPort() {
    let intercept = cy.wait("@" + RequestAliases.PostShippingLineAreaPort);
    intercept.then((interception) => {
        assert.equal(interception.response.statusCode, 200)
    })
}

export function FillShippingLineAreaDetails(shippingLineDetails: ShippingLineDetails) {
    cy.FillLogTextBox(ShippingLineSelectors.AreaName, shippingLineDetails.AreaName)
    cy.FillLogTextBox(ShippingLineSelectors.AreaDescription, shippingLineDetails.AreaDescription)
}

export function CreateShippingLineArea() {
    cy.DefineRequestWait(RestAPI.POST, Urls.ShippingLinesArea, RequestAliases.PostShippingLineArea)
    cy.Click(BaseSelectors.RedButton, BaseSelectors.ContainsOK);
}

export function AssertCreateShippingLineArea() {
    let intercept = cy.wait("@" + RequestAliases.PostShippingLineArea);
    intercept.then((interception) => {
        assert.equal(interception.response.statusCode, 200)
    })
}

export function FillTariffPartnerCode(partnerCode: string) {
    cy.FillLogTextBox(ShippingLineSelectors.TariffPartnerCode, partnerCode)
}

export function FillShippingLineTariffTranslations(shippingLineDetails: ShippingLineDetails) {
    cy.FillLogTextBox(ShippingLineSelectors.TariffPartnerCode, GenerateRandomNumberAndString(ShippingLineSelectors.TariffCodeDigitCount))
    cy.FillLogLov(ShippingLineSelectors.TariffPort, shippingLineDetails.TariffPort, true)
}

export function CreateShippingLineTariffTranslations() {
    cy.DefineRequestWait(RestAPI.POST, Urls.AirShippingLinesTariffTranslations, RequestAliases.PostShippingLineTariffTranslations)
    cy.Click(BaseSelectors.RedButton, BaseSelectors.ContainsOK);
}

export function AssertCreateShippingLineTariffTranslations() {
    let intercept = cy.wait("@" + RequestAliases.PostShippingLineTariffTranslations);
    intercept.then((interception) => {
        assert.equal(interception.response.statusCode, 200)
    })
}

export function EditShippingLine() {
    DefinePutShippingLineRequest();
    cy.Click(ShippingLineSelectors.SaveButton, null);
}

function DefinePutShippingLineRequest() {
    cy.DefineRequestWait(RestAPI.PUT, Urls.ShippingLines, RequestAliases.PutShippingLine);
}

export function AssertEditShippingLine() {
    BaseAssertion.AssertStatusCode(RequestAliases.PutShippingLine, 200)
}

export function CloseSaveShippingLine() {
    DefineShippingLineViewGetSingleRequest()
    cy.Click(ShippingLineSelectors.SaveCloseButton, null);
}

function DefineShippingLineViewGetSingleRequest() {
    cy.DefineRequestWait(RestAPI.GET, Urls.ShippingLinesviewGetSingle, RequestAliases.GetSignle);
}

export function AssertCloseSaveShippingLine() {
    BaseAssertion.AssertStatusCode(RequestAliases.GetSignle, 200);
}