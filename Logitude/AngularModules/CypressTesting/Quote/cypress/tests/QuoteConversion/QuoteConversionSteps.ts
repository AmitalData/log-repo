import * as QuotesActions from '../../actions/Actions';
import { Given, When, Then } from 'cypress-cucumber-preprocessor/steps';
import * as BaseAssertion from '../../../../Base/cypress/actions/Assertion';
import { RequestAliases } from '../../../../Base/cypress/constants/RequestAliases';
import { QuoteDetails } from '../../models/QuoteDetails';
import { PackagesDetails } from '../../../../Shipment/cypress/models/PackagesDetails';
import * as Assists from "../../../../Base/cypress/assists/Assists";
import { EventTypeDetails } from "../../../../Base/cypress/models/EventTypeDetails"
import * as BaseActions from "../../../../Base/cypress/actions/Actions"
import { QuoteSelectors } from "../../selectors/Selectors";

let quoteDetails: QuoteDetails;

//#region Create export air quote
Given("the user logged in and navigates to quotes workspace", () => {
  cy.Login();
  QuotesActions.NavigatesToSQuotesWorkspace();
});

Given("a quote with the following details", (dataTable) => {
  quoteDetails = Assists.CreateInstance<QuoteDetails>(dataTable, true);
  QuotesActions.FillQuoteFields(quoteDetails);
});

Given("an expected order with the following details", (dataTable) => {
  let expectedOrderDetails = Assists.CreateSet<PackagesDetails>(dataTable);
  QuotesActions.FillExpectedOrderDetailsDimensions(expectedOrderDetails);
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

//#region convert quote transport mode
Given("the user open the quote", () => {
  QuotesActions.OpenQuote(quoteDetails.QuoteNumber);
});

Given("{string} action", (action) => {
  QuotesActions.OpenConvertQuoteWizerd(action)
});

Given("fill the following details for quote conversion", (dataTable) => {
  quoteDetails = Assists.CreateInstance<QuoteDetails>(dataTable, true);
  QuotesActions.ConvertQuoteToOceanFCL(quoteDetails);
});

When("convert quote transport mode", () => {
  QuotesActions.ConvertQuote();
});

Then("the quote should update successfully", () => {
  BaseAssertion.AssertStatusCode(RequestAliases.Quotes, 200);
});

Then("following event should appear in events tab", (dataTable) => {
  let eventDetailsList = Assists.CreateSet<EventTypeDetails>(dataTable);
  BaseActions.ValidateEventsTab(eventDetailsList, QuoteSelectors.QuoteEventsTab);
});
//#endregion

//#region Change quote type to LCL
When("{string} action with {string} note", (action, note) => {
  QuotesActions.OpenQuoteAction(action, note);
});
//#endregion