import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import * as MaintenanceActions from "../../actions/Actions";
import * as Assists from "../../../../Base/cypress/assists/Assists";
import * as QuotesActions from "../../../../Quote/cypress/actions/Actions"
import * as ShipmentActions from "../../../../Shipment/cypress/actions/Actions"
import { CurrencyDetails } from "../../models/CurrencyDetails";
import { QuoteDetails } from "../../../../Quote/cypress/models/QuoteDetails";
import * as BaseAssertion from '../../../../Base/cypress/actions/Assertion';
import { RequestAliases } from "../../../../Base/cypress/constants/RequestAliases";
import { QuoteSelectors } from "../../../../Quote/cypress/selectors/Selectors";
import { ShipmentDetails } from "../../../../Shipment/cypress/models/ShipmentDetails";
import { ReceivableDetails } from "../../../../Shipment/cypress/models/ReceivableDetails";
import { ShipmentSelectors } from "../../../../Shipment/cypress/selectors/Selectors";
import { AccountingSelectors } from "../../../../Accounting/cypress/selectors/Selectors";
import { BaseSelectors } from "../../../../Base/cypress/selectors/BaseSelectors";

let quoteDetails: QuoteDetails
let shipmentDetails: ShipmentDetails

//#region Edit exchange rate value
Given("the user logged in and open {string} from tenant settings", (navigateTo) => {
    cy.Login()
    MaintenanceActions.NavigateToCurrenctRateSettings(navigateTo)
});

Given("today's exchange rate for {string} with the following details", (currency, dataTable) => {
    let currencyDetails = Assists.CreateInstance<CurrencyDetails>(dataTable, true);
    MaintenanceActions.UpdateCurrencyRate(currency, currencyDetails)
});

When("edit currency rate", () => {
    MaintenanceActions.CreateCurrencyRate();
});

Then("the currency rate should update successfully", () => {
    MaintenanceActions.AssertPostCurrencyRate();
});

Then("currency history will contain the following details", (dataTable) => {
    let historyDetails = Assists.CreateInstance<CurrencyDetails>(dataTable, true);
    MaintenanceActions.ValidateHistoryValues(historyDetails)
});
//#endregion

//#region Create export air quote
Given("the user navigates to quotes workspace", () => {
    QuotesActions.NavigatesToSQuotesWorkspace();
});

Given("a quote with the following details", (dataTable) => {
    quoteDetails = Assists.CreateInstance<QuoteDetails>(dataTable, true);
    QuotesActions.FillQuoteFields(quoteDetails);
});

When("create quote", () => {
    QuotesActions.CreateQuote();
});

Then("the quote should create successfully", () => {
    BaseAssertion.AssertStatusCode(RequestAliases.Quotes, 200).then((interception) => {
        quoteDetails.QuoteNumber = interception.response.body.QuoteNumber;
    })
});
//#endregion

//#region Quote cost currency
Given("the user open the quote and navigate to Charges", () => {
    QuotesActions.OpenQuote(quoteDetails.QuoteNumber);
    cy.Navigate(QuoteSelectors.QuoteCharges)
});

Given("the user open Add Charges window", () => {
    cy.Click(QuoteSelectors.QuoteAddCharges, null)
});

When("fill Cost Currency with {string}", (currency) => {
    cy.FillLogLov(QuoteSelectors.QuoteChargeCostCurrency, currency, true)
});

Then("cost Exchange Rate should equal to {string}", (expectedValue) => {
    MaintenanceActions.ValidateQuoteCostRate(expectedValue);
});
//#endregion

//#region Create direct export air shipment
Given("the user navigates to shipments workspace", () => {
    ShipmentActions.NavigatesToShipmentsWorkspace()
});

Given("a direct shipment with the following details", (dataTable) => {
    shipmentDetails = Assists.CreateInstance<ShipmentDetails>(dataTable, true);
    ShipmentActions.OpenNewShipmentWizard(shipmentDetails.ShipmentLevel);
    ShipmentActions.FillShipmentWizardsFields(shipmentDetails);
});

When("create shipment", () => {
    ShipmentActions.CreateShipment(shipmentDetails.ShipmentLevel);
});

Then("the direct should create successfully", () => {
    BaseAssertion.AssertStatusCode(RequestAliases.ShipmentRequest, 200).then((interception) => {
        shipmentDetails.ShipmentNumber = interception.response.body.ShipmentNumber;
    })
});
//#endregion

//#region Receivable cost currency
Given("the user open the direct shipment", () => {
    ShipmentActions.OpenShipment(shipmentDetails.ShipmentNumber)
});

Given("a receivable with the following details", (dataTable) => {
    const receivableData = Assists.CreateInstance<ReceivableDetails>(dataTable, true);
    MaintenanceActions.FillReceivableFields(receivableData);
});

When("fill {string} as receivable Currency", (currency) => {
    cy.FillLogLov(ShipmentSelectors.ReceivableCurrency, currency, true)
});

Then("the Receivable Exchange Rate should equal to {string}", (expectedValue) => {
    MaintenanceActions.ValidateReceivableCostRate(expectedValue)
});
//#endregion

//#region ARInvoice cost currency
Given("the user add new invoicee", () => {
    cy.Click(AccountingSelectors.CreateARInvoiceButton, null);
});

When("fill {string} as Invoice Currency", (currency) => {
    cy.FillLogLov(AccountingSelectors.ARInvoiceInvoiceCurrency, currency, true)
});

Then("the Invoice Exchange Rate should equal to {string}", (expectedValue) => {
    cy.get(AccountingSelectors.ARInvoiceExchangeRate).should(BaseSelectors.HaveValue, expectedValue)
});
//#endregion

//#region ARInvoice cost currency old date
When("fill {string} as Invoice Date", (date) => {
    cy.FillDate(AccountingSelectors.ARInvoiceInvoiceDate, date)
    cy.FillLogLov(AccountingSelectors.ARInvoiceInvoiceCurrency, "EUR", true)
});

Then("the Invoice Exchange Rate should equal to {string}", (expectedValue) => {
    cy.get(AccountingSelectors.ARInvoiceExchangeRate).should(BaseSelectors.HaveValue, expectedValue)
});
//#endregion
