import { SettingsSelectors } from "../selectors/Selectors";
import { BaseSelectors } from "../../../Base/cypress/selectors/BaseSelectors";
import { URLs } from "../constants/URLs";
import { RestAPI } from "../../../Base/cypress/constants/RestAPI";
import { RequestAliases } from "../../../Base/cypress/constants/RequestAliases";
import * as BaseAssertion from "../../../Base/cypress/actions/Assertion";
import * as ShipmentActions from "../../../Shipment/cypress/actions/Actions"
import { CurrencyDetails } from "../models/CurrencyDetails";
import { QuoteSelectors } from "../../../Quote/cypress/selectors/Selectors";
import { ShipmentSelectors } from "../../../Shipment/cypress/selectors/Selectors";
import { ReceivableDetails } from "../../../Shipment/cypress/models/ReceivableDetails";
//#region Currency Settings 
export function NavigateToCurrenctRateSettings(navigateTo:string){
    cy.DefineRequestWait(RestAPI.GET, URLs.GetCurrenciesExchangeRateValue, RequestAliases.GetCurrencyRate);
    cy.Click(SettingsSelectors.SettingButton,null)
    cy.Click(BaseSelectors.button,navigateTo);
    BaseAssertion.AssertStatusCode(RequestAliases.GetCurrencyRate, 200);
}

export function UpdateCurrencyRate(currency: string, currencyDetails: CurrencyDetails) {
    cy.get(SettingsSelectors.CurrencyDate(currency)).invoke('text').then((text) => {
        if (text.trim() != currencyDetails.ExchangeDate) {
            EditCurrencyRate(currency, currencyDetails)
        } else {
            CurrencyDetails.IsUpdated = true
        }
    })
}

function EditCurrencyRate(currency: string, currencyDetails: CurrencyDetails) {
    cy.Click(SettingsSelectors.CurrencyEditButton(currency), null)
    cy.FillDate(SettingsSelectors.RatesTableDate, currencyDetails.ExchangeDate);
    cy.FillLogTextBox(SettingsSelectors.RatesTableRate, currencyDetails.Rate);
}

export function CreateCurrencyRate() {
    if (!CurrencyDetails.IsUpdated) {
        DefineCreateCurrencyRate()
    }else{
        cy.log("Currency is already updated for today")
    }
}

export function AssertPostCurrencyRate() {
    if (!CurrencyDetails.IsUpdated) {
        BaseAssertion.AssertStatusCode(RequestAliases.PostCurrencyRate, 200);
    }else{
        cy.log("Currency is already updated for today")
    }
}

export function ValidateHistoryValues(historyDetails: CurrencyDetails) {
    OpenViewHistory(historyDetails.Currency)
    cy.get(BaseSelectors.RowHover).eq(0).invoke(BaseSelectors.TextElement).then((text) => {
        expect(text).to.contain(historyDetails.ExchangeDate);
        expect(text).to.contain(historyDetails.Rate);
    });
    cy.Click(BaseSelectors.RedButton + BaseSelectors.LastElement, "Close");
    cy.Click(BaseSelectors.RedButton + BaseSelectors.LastElement, "Close");
}

function DefineCreateCurrencyRate() {
    DefinePostCurrencyRateRequest()
    cy.Click(BaseSelectors.RedButton + BaseSelectors.LastElement, "Ok");
    cy.Click(BaseSelectors.RedButton + BaseSelectors.LastElement, "Yes");
}

function DefinePostCurrencyRateRequest() {
    cy.DefineRequestWait(RestAPI.POST, URLs.CurrencyRate, RequestAliases.PostCurrencyRate);
}

function OpenViewHistory(Currency:string){
    DefineGetByFilterRequest()
    cy.Click(SettingsSelectors.CurrencyHistoryButton(Currency), null)
    AssertGetByFilters()
}
function DefineGetByFilterRequest() {
    cy.DefineRequestWait(RestAPI.GET, URLs.GetByFilter, RequestAliases.GetByFilter);
}
function AssertGetByFilters() {
    BaseAssertion.AssertStatusCode(RequestAliases.GetByFilter, 200);
}
export function ValidateQuoteCostRate(expectedValue:string){
    cy.get(QuoteSelectors.QuoteChargeCostExchangeRate).should(BaseSelectors.HaveValue, expectedValue)
    cy.Click(QuoteSelectors.QuoteCancelAddCharges,null)
    cy.BackButton(QuoteSelectors.ContaintsQuote)
}

export function FillReceivableFields(receivableData:ReceivableDetails){
    cy.Click(ShipmentSelectors.ReceivablesTab, null) 
    cy.Click(ShipmentSelectors.AddNewReceivableLine, null)
    cy.FillLogLov(ShipmentSelectors.ReceivableChargesType, receivableData.ChargesType, true)
    cy.FillLogLov(ShipmentSelectors.ReceivableMeasurement, receivableData.UOM, true)
    cy.FillLogTextBox(ShipmentSelectors.ReceivableQuantity,receivableData.Quantity.toString())
    cy.FillLogTextBox(ShipmentSelectors.ReceivableUnitPrice,receivableData.UnitPrice.toString())
}

export function ValidateReceivableCostRate(expectedValue:string){
    cy.get(ShipmentSelectors.ShipmentReceivableRate).should(BaseSelectors.HaveValue, expectedValue)
    cy.Click(ShipmentSelectors.AddReceivableOkButton, null);
    ShipmentActions.UpdateShipment(ShipmentSelectors.ShipmentSaveButton);
    BaseAssertion.AssertStatusCode(RequestAliases.ShipmentRequest, 200);
}

//#endregion